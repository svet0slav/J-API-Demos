using System.Text.Json.Serialization;

namespace MockyProducts.Shared.Data
{
    public interface IProduct
    {
        int Id { get; set; }

        string? Title { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public List<string>? Sizes { get; set; }
    }
}
