using MockyProducts.Repository.Data;
using MockyProducts.Repository.Requests;

namespace MockyProducts.Repository.Readers
{
    public interface IMockyJsonReader
    {
        Task<ProductsSource?> GetRawDataFromSource(MockyRawDataParams param);
    }
}