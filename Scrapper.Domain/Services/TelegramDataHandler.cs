using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public sealed class TelegramDataHandler<T> : PersonDataHandler<T>
        where T : Person
    {

        public TelegramDataHandler(IDbContext dbContext)
            : base(dbContext) { }

        public override async Task HandleEntity(T person)
        {
            var url = GetTelegramUrl(person);
            await UpdateIfExistPersonAsync(person, url);
        }

        private string GetTelegramUrl(Person person)
        {
            string resultUrl = string.Empty;
            //string beginUrl = "https://t.me/";
            string beginUrl = "t.me/";
            //Regex regexSite = new Regex($"{beginUrl}S+");
            //resultUrl = regexSite.Match(person.Description).ToString().Trim();

            int indexOfStartUrl = person.Description.IndexOf(beginUrl);
            int indexOfSpace = 0;

            if (indexOfStartUrl > 1)
            {
                indexOfSpace = person.Description.IndexOf(" ", indexOfStartUrl);
                if(indexOfSpace < 0)
                    indexOfSpace = person.Description.IndexOf("...", indexOfStartUrl);

                var urlLength = indexOfSpace - indexOfStartUrl;
                if (indexOfStartUrl > 1 && urlLength > 0)
                    resultUrl = person.Description.Substring(indexOfStartUrl, urlLength).Trim();
            }

            return resultUrl;
        }

        private async Task UpdateIfExistPersonAsync(Person person, string telegramUrl)
        {
            if (string.IsNullOrEmpty(telegramUrl))
                return;

            telegramUrl = "http://" + telegramUrl;

            EvaluatePerson(person);
            var existPerson = GetPersonFormDbContext(person);
            if (existPerson == null)
                existPerson = await AddPerson(person);

            if (existPerson != null)
            {
                var site = DbContext.WebSites.FirstOrDefault(s => s.CandidateId == existPerson.Id);
                if(site != null)
                {
                    site.WebSiteName = "Telegram";
                    site.WebSiteUrl = telegramUrl;
                }
                else
                {
                    await DbContext.WebSites.AddAsync(new WebSite 
                    { 
                        Person = person,
                        WebSiteName = "Telegram",
                        WebSiteUrl = telegramUrl
                    });
                }
            }
        }

        private async Task<Person> AddPerson(Person person)
        {
            await DbContext.Persons.AddAsync(person);
            return person;
        }

        private Person GetPersonFormDbContext(Person person)
        {
            return DbContext.Persons.FirstOrDefault(
                p => p.Url.ToLower().Trim().Equals(person.Url.ToLower().Trim()));
        }
    }
}
