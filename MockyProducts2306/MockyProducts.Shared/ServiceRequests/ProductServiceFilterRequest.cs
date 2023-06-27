namespace MockyProducts.Shared.ServiceRequests
{
    public class ProductServiceFilterRequest
    {
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string? Size { get; set; }
        public List<string>? Highlight { get; set; }
    }
}
