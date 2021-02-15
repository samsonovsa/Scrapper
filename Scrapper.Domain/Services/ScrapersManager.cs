using Scrapper.Domain.Extensions;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Scrapper.Domain.Services
{
    public class ScrapersManager : IScrapersManager
    {
        private readonly IInputDataProvider _inputDataProvider;
        private readonly IDbContext _dbContext;
        private readonly IScrapper<Person> _scrapper;

        public event IScrapersManager.ScrapperEventHandler Notify;
        public delegate void ScrapperEventHandler(object sender, ScrapperEventArgs e);

        public ScrapersManager(IInputDataProvider inputDataProvider, IDbContext dbContext, IScrapper<Person> scrapper)
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

                            var persons = await _scrapper.GetPersonsAsync(inputData);
                            await CheckAndSaveAsync(persons);
                            Notify?.Invoke(this, new ScrapperEventArgs(inputData));
                        }
                    }
                }
            }
        }

        private async Task CheckAndSaveAsync(List<Person> persons)
        {
            foreach (var person in persons)
            {
                EvaluatePerson(person);
                UpdateIfExist(person);
                await AddIfNeed(person);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
            }
        }

        private void EvaluatePerson(Person person)
        {
            HahdleUrl(person);
            HahdleEmail(person);
          
        }

        private void HahdleEmail(Person person)
        {
            if (string.IsNullOrEmpty(person.Email))
                return;

            if (char.IsPunctuation(person.Email.Last()))
                person.Email = person.Email.Remove(person.Email.Length - 1);
        }

        private void HahdleUrl(Person person)
        {
            //person.Photo = person.Url;

            person.Url = person.Url.Replace(@"https://ru.linkedin.com", @"https://www.linkedin.com");
            person.Url = person.Url.RemoveFrom("?");
            person.Url = person.Url.RemoveFrom("/ru-ru");
          //  person.Url = HttpUtility.UrlEncode(person.Url, Encoding.Unicode);
        }

        private void UpdateIfExist(Person person)
        {
            var existPerson = _dbContext.Persons.FirstOrDefault(p => p.Url.ToLower().Equals(person.Url.ToLower()));
            if (existPerson == null)
                return;

            if(!string.IsNullOrEmpty(existPerson.Email)
                && !string.IsNullOrEmpty(person.Email)
                && !isExistsEmail(existPerson.Email, person.Email))
            {
                existPerson.Email = existPerson.Email.AddWithComma(person.Email);
            }

            existPerson.Photo = person.Photo;

            _dbContext.Persons.Update(existPerson);
        }

        private async Task AddIfNeed(Person person)
        {
            if (!_dbContext.Persons.Any(p => p.Url.ToLower().Equals(person.Url.ToLower()))
                && !string.IsNullOrEmpty(person.Email))
            {
                await _dbContext.Persons.AddAsync(person);
            }
        }

        private bool isExistsEmail(string baseEmail, string newEmail)
        {
          return  baseEmail.Trim().IndexOf(newEmail.Trim(), StringComparison.InvariantCultureIgnoreCase)>=0;
        }
    }
}
