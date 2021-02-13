using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IScrapersManager
    {
        Task ScrapLinkidinData();
        Task ScrapTlegramData();
    }
}
