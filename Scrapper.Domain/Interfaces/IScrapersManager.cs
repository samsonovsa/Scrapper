using Scrapper.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IScrapersManager
    {
        Task ScrapDataAsync();

        delegate void ScrapperEventHandler(object sender, ScrapperEventArgs e);
        event ScrapperEventHandler Notify;
    }
}
