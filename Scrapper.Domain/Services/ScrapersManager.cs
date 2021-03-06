﻿using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;

namespace Scrapper.Domain.Services
{
    public class ScrapersManager : IScrapersManager
    {
        private readonly IInputDataProvider _inputDataProvider;
        private readonly IDataHandler<Person> _dataHandler;
        private readonly IScrapper<Person> _scrapper;
        private readonly ILog _log;

        public event IScrapersManager.ScrapperEventHandler Notify;
        public delegate void ScrapperEventHandler(object sender, ScrapperEventArgs e);

        public ScrapersManager(IInputDataProvider inputDataProvider, IDataHandler<Person> dataHandler, IScrapper<Person> scrapper, ILog log)
        {
            _inputDataProvider = inputDataProvider;
            _dataHandler = dataHandler;
            _scrapper = scrapper;
            _log = log;
        }

        public async Task ScrapDataAsync()
        {
            await ScrapProcessAsync(_inputDataProvider.Data);
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
                            await _dataHandler.HandleEntitiesAsync(persons);
                            Notify?.Invoke(this, new ScrapperEventArgs(inputData));
                            _log.Info(inputData.ToString());
                        }
                    }
                }
            }
        } 
    }
}
