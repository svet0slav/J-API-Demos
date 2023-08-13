using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPapSci.Service.Common
{
    /// <summary>
    /// Provides a random generator response and chooses from 0 to N-1 for N items.
    /// </summary>
    public interface IRandomGeneratorService
    {
        Task<int> GetRandom(int N, CancellationToken cancellationToken);
    }
}
