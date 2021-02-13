using Scrapper.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IScrapper<TEntity>
    {
        Task<List<TEntity>> GetPersonsAsync(InputData inputData);
    }
}
