using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public class ScrapersManager : IScrapersManager
    {
        private readonly IInputDataProvider _inputDataProvider;
        private readonly IDataHandler<Person> _dataHandler;
        private readonly IScrapper<Person> _scrapper;


        public event IScrapersManager.ScrapperEventHandler Notify;
        public delegate void ScrapperEventHandler(object sender, ScrapperEventArgs e);

        public ScrapersManager(IInputDataProvider inputDataProvider, IDataHandler<Person> dataHandler, IScrapper<Person> scrapper)
        {
            _inputDataProvider = inputDataProvider;
            _dataHandler = dataHandler;
            _scrapper = scrapper;
        }

        public async Task ScrapLinkidinDataAsync()
        {
            await ScrapProcessAsync(_inputDataProvider.Data);
        }

        public async Task ScrapTlegramDataAsync()
        {
            var telegramInputDataList = new InputDataLists
            {
                Keywords = _inputDataProvider.Data.Keywords,
                Locations = _inputDataProvider.Data.Locations,
                Sites = _inputDataProvider.Data.Sites,
                Domains = new List<string>
                {
                   "t.me/"
                }
            };

            await ScrapProcessAsync(telegramInputDataList);
        }

        private async Task ScrapProcessAsync(InputDataLists inputDataLists)
        {
            foreach (var keyword in inputDataLists.Keywords)
            {
                foreach (var location in inputDataLists.Locations)
                {
                    foreach (var domain in inputDataLists.Domains)
                    {
                        foreach (var site in inputDataLists.Sites)
                        {
                            var inputData = new InputData
                            {
                                Keyword = keyword,
                                Domain = domain,
                                Location = location,
                                Site = site,
                            };

                            var persons = await _scrapper.GetPersonsAsync(inputData);
                            await _dataHandler.HandleAndStoreAsync(persons);
                            Notify?.Invoke(this, new ScrapperEventArgs(inputData));
                        }
                    }
                }
            }
        } 
    }
}
