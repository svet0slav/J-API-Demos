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
            var taskMinPrice = new Task(() =>
            {
                result.TotalMinPrice = products
                    .AsParallel()
                    .Where(p => p.Price != null)
                    .Min(p => p.Price);
            });

            if (cancellationToken.IsCancellationRequested) return result;

            // 3.c.i.The maximum price of all products in the source URL.
            var taskMaxPrice = new Task(() =>
            {
                result.TotalMaxPrice = products
                .AsParallel()
                .Where(p => p.Price != null)
                .Max(p => p.Price);
            }, cancellationToken);

            if (cancellationToken.IsCancellationRequested) return result;

            // 3.c.ii.An array of strings to contain all sizes of all products in the source URL.
            var taskSizes = new Task(() =>
            {
                var sizesAgg = new ConcurrentDictionary<string, int>();
                products.AsParallel().ForAll(p =>
                {
                    if (p.Sizes?.Count > 0)
                    {
                        p.Sizes.ForEach(size =>
                        {
                            if (!sizesAgg.ContainsKey(size)) sizesAgg.TryAdd(size, 0);
                        });
                    }
                });
                result.AllSizes = sizesAgg.Keys.ToList();
            });

            await Task.WhenAll(taskMinPrice, taskMaxPrice, taskSizes);

            return result;
        }
    }
}
