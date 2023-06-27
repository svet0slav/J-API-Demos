namespace MockyProducts.Shared.Requests
{
    /// <summary>
    /// Filter products specification /filter?minprice=5&maxprice=20&size=medium&highlight=green,blue 
    /// </summary>
    public class GetProductsRequest
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string? Size { get; set; }
        public string? Highlight { get; set; }
    }
}
