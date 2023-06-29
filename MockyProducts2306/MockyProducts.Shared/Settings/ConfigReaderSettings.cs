namespace MockyProducts.Shared.Settings
{
    public class ConfigReaderSettings
    {
        public string? Url { get; set; }
        public int? TimeoutSeconds { get; set; } = 120;
    }
}
