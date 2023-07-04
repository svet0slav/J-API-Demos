using MockyProducts.Shared.Data;
using MockyProducts.Shared.Dto;
using System.Collections.Concurrent;
using System.Linq;

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

            //does not work await Task.WhenAll(taskMinPrice, taskMaxPrice, taskSizes);
            taskMinPrice.Start();
            taskMaxPrice.Start();
            taskSizes.Start();
            Task.WaitAll(taskMinPrice, taskMaxPrice , taskSizes);

            result.TotalMinPrice = taskMinPrice.Result;
            result.TotalMaxPrice = taskMaxPrice.Result;
            result.AllSizes = taskSizes.Result;
            return result;
        }
    }
}
