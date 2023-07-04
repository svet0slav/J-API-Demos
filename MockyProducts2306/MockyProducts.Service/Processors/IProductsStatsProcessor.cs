using MockyProducts.Shared.Data;
using MockyProducts.Shared.Dto;

namespace MockyProducts.Service.Processors
{
    public interface IProductsStatsProcessor
    {
        /// <summary>
        /// Calculate the statistics from all products in the source URL.
        /// </summary>
        /// <param name="products">List of products (may be null)</param>
        /// <returns>The statistics object. If empty list, creates object as empty</returns>
        Task<ProductStatDto> Summarize(IEnumerable<IProduct>? products, CancellationToken cancellationToken);
    }
}
