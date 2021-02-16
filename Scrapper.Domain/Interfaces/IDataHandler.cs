using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IDataHandler<TData>
    {
        Task HandleAndStoreAsync(List<TData> data);
    }
}
