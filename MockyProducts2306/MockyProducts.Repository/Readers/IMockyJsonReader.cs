using MockyProducts.Repository.Data;
using MockyProducts.Repository.Requests;
using System.Threading;

namespace MockyProducts.Repository.Readers
{
    public interface IMockyJsonReader
    {
        Task<ProductsSource?> GetRawDataFromSource(MockyRawDataParams param, CancellationToken cancellationToken);
    }
}