using MockyProducts.Shared.Data;
using MockyProducts.Shared.Dto;
using System.Collections.Concurrent;

namespace MockyProducts.Service.Processors
{
    public class ProductsStatsProcessor : IProductsStatsProcessor
    {
        public ProductsStatsProcessor() {
        }

        /// <summary>
        /// Calculate the statistics from all products.
        /// </summary>
        /// <param name="products">List of products (may be null)</param>
        /// <returns>The statistics object. If empty list, creates object as empty</returns>
        public async Task<ProductStatDto> Summarize(IEnumerable<IProduct>? products, CancellationToken cancellationToken)
        {
            var result = new ProductStatDto();
            if (products == null) return result;

            // 3.c.i.The minimum price of all products in the source URL.
            var taskMinPrice = new Task<double?>(() =>
            {
                return products
                    .Where(p => p.Price != null)
                    .Min(p => p.Price);
            });

            if (cancellationToken.IsCancellationRequested) return result;

            // 3.c.i.The maximum price of all products in the source URL.
            var taskMaxPrice = new Task<double?>(() =>
            {
                return products
                .Where(p => p.Price != null)
                .Max(p => p.Price);
            }, cancellationToken);

            if (cancellationToken.IsCancellationRequested) return result;

            // 3.c.ii.An array of strings to contain all sizes of all products in the source URL.
            var taskSizes = new Task<List<string>>(() =>
            {
                var sizesAgg = new ConcurrentDictionary<string, int>();
                foreach(var p in products)
                {
                    if (p.Sizes?.Count > 0)
                    {
                        p.Sizes.ForEach(size =>
                        {
                            if (!sizesAgg.ContainsKey(size)) sizesAgg.TryAdd(size, 0);
                        });
                    }
                };
                return sizesAgg.Keys.ToList();
            });

            // 3.c.iii. An string array of size ten to contain most common words in the product descriptions, excluding the most common five in source URL
            var taskWords = new Task<List<string>>(() =>
            {
                var wordsAgg = new ConcurrentDictionary<string, int>();
                foreach (var p in products)
                {
                    var words = GetWords(p.Description);
                    if (words.Count() > 0)
                    {
                        foreach(var word in words)
                        {
                            if (!wordsAgg.ContainsKey(word)) {
                                wordsAgg.TryAdd(word, 1);
                            }
                            else
                            {
                                wordsAgg[word] += 1;
                            }
                        }
                    }
                };

                var sorted = wordsAgg.OrderByDescending(x => x.Value);
                return sorted.Select(x => x.Key).ToList();
            });

            Task[] tasks = { taskMinPrice, taskMaxPrice, taskSizes, taskWords };
            foreach(Task t in tasks) { t.Start(); }
            Task.WaitAll(tasks, cancellationToken);

            result.TotalMinPrice = taskMinPrice.Result;
            result.TotalMaxPrice = taskMaxPrice.Result;
            result.AllSizes = taskSizes.Result;
            result.MostCommonWords = taskWords.Result;
            return result;
        }

        public IEnumerable<string> GetWords(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Enumerable.Empty<string>();

            var wordsRaw = description.Trim().Split(' ');
            var words = wordsRaw.Where(w => !string.IsNullOrWhiteSpace(w)).Select(w =>
                w.Replace(".", "").Replace("!", "").Replace(";", "").Replace(",", ""));
            words = words.Where(w => !string.IsNullOrWhiteSpace(w)).Select(w =>
                w.Replace("?", "").Replace(")", "").Replace("(", "").Replace(":", ""));
            words = words.Where(w => !string.IsNullOrWhiteSpace(w));
            return words ?? Enumerable.Empty<string>();
        }
    }
}
