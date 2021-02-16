using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IDataHandler<TEntity>
    {
        Task HandleEntitiesAsync(List<TEntity> entities);
    }
}
