using System.Text.Json.Serialization;

namespace MockyProducts.Shared.Dto
{
    /// <summary>
    /// Products list statistics.
    ///     /// Add 3.c. A filter object to contain: 
    ///     i.The minimum and the maximum price of all products in the source URL
    ///     ii.An array of strings to contain all sizes of all products in the source URL
    ///     iii. An string array of size ten to contain most common words in the product descriptions, excluding the most common five in source URL
    /// </summary>
    public class ProductStatDto
    {
        /// <summary>
        /// 3.c.i.The minimum price of all products in the source URL.
        /// </summary>
        [JsonPropertyName("minPriceForAll")]
        public double? TotalMinPrice { get; set; }

        /// <summary>
        /// 3.c.i.The maximum price of all products in the source URL.
        /// </summary>
        [JsonPropertyName("maxPriceForAll")]
        public double? TotalMaxPrice { get; set; }

        /// <summary>
        /// 3.c.ii.An array of strings to contain all sizes of all products in the source URL.
        /// </summary>
        [JsonPropertyName("allSizes")]
        public List<string>? AllSizes { get; set; }

        /// <summary>
        /// 3.c.iii. A string array of size ten to contain most common words in the product descriptions, excluding the most common five in source URL.
        /// </summary>
        [JsonPropertyName("mostCommonWords")]
        public List<string>? MostCommonWords { get; set; }
    }
}
