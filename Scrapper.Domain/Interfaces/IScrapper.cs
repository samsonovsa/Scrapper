using Scrapper.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IScrapper
    {
        Task<List<Person>> GetPersonsAsync(InputData inputData);
    }
}
