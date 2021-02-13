using Scrapper.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IScrapersManager
    {
        Task ScrapLinkidinDataAsync();
        Task ScrapTlegramDataAsync();
        delegate void ScrapperEventHandler(object sender, ScrapperEventArgs e);
        event ScrapperEventHandler Notify;
    }
}
