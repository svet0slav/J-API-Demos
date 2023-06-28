using MockyProducts.Shared.Requests;

namespace MockyProducts.Shared.ServiceRequests.Mappers
{
    public static class GetProductsRequestToServiceMapper
    {
        public static ProductServiceFilterRequest? ToProductServiceFilterRequest(this GetProductsRequest? request)
        {
            if (request == null) return null;

            var result = new ProductServiceFilterRequest();

            if (double.TryParse(request?.MinPrice, out var minPrice))
            {
                result.MinPrice = minPrice;
            }

            if (double.TryParse(request?.MaxPrice, out var maxPrice))
            {
                result.MaxPrice = maxPrice;
            }

            result.Size = request?.Size;

            if (!string.IsNullOrEmpty(request?.Highlight))
            {
                var words = request?.Highlight.Split(',');
                if (words?.Length > 0)
                {
                    var filtered = words.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    result.Highlight = new List<string>(filtered);
                }
            }

            return result;
        }
    }
}
