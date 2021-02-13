using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IFileReader
    {
        Task<List<string>> GetStrings(string fileName);
    }
}
