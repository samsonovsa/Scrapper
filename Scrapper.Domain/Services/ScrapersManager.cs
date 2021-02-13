using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public class ScrapersManager : IScrapersManager
    {
        private readonly IInputDataProvider _inputDataProvider;
        private readonly IDbContext _dbContext;
        private readonly IScrapper _scrapper;

        public event IScrapersManager.ScrapperEventHandler Notify;
        public delegate void ScrapperEventHandler(object sender, ScrapperEventArgs e);

        public ScrapersManager(IInputDataProvider inputDataProvider, IDbContext dbContext, IScrapper scrapper)
        {
            _inputDataProvider = inputDataProvider;
            _dbContext = dbContext;
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

                            var persons = await _scrapper.GetPersons(inputData);
                            await SaveAsync(persons);
                            Notify?.Invoke(this, new ScrapperEventArgs(inputData));
                        }
                    }
                }
            }
        }


        private async Task SaveAsync(List<Person> persons)
        {
            await _dbContext.Persons.AddRangeAsync(persons);
            await _dbContext.SaveChangesAsync();
        }
    }
}
