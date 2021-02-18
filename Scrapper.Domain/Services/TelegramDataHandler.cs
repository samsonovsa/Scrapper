using Scrapper.Domain.Extensions;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public sealed class TelegramDataHandler<T> : BaseDataHandler<T>
        where T : Person
    {
        public TelegramDataHandler(IDbContext dbContext)
            : base(dbContext) { }

        public override List<T> PreprocessingEntities(List<T> entities)
        {
            return entities.Distinct(new PersonComparer<T>()).ToList();
        }

        public override async Task HandleEntity(T person)
        {
            var url = GetTelegramUrl(person);
            await UpdateIfExistPersonAsync(person, url);
        }

        private string GetTelegramUrl(Person person)
        {
            string resultUrl = string.Empty;
            string beginUrl = "https://t.me/";
            int indexOfStartUrl = person.Description.IndexOf(beginUrl);
            int indexOfSpace = 0;

            if (indexOfStartUrl > 1)
            {
                indexOfSpace = person.Description.IndexOf(" ", indexOfStartUrl);

                var urlLength = indexOfSpace - indexOfStartUrl;
                if (indexOfStartUrl > 1 && urlLength > 0)
                    resultUrl = person.Description.Substring(indexOfStartUrl, urlLength).Trim();
            }

            return resultUrl;
        }

        private async Task UpdateIfExistPersonAsync(Person person, string telegramUrl)
        {
            var existPerson = DbContext.Persons.FirstOrDefault(p => p.Url.ToLower().Equals(person.Url.ToLower()));

            if (existPerson != null
                 && !string.IsNullOrEmpty(telegramUrl))
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
                        CandidateId = existPerson.Id,
                        WebSiteName = "Telegram",
                        WebSiteUrl = telegramUrl
                    });
                }
            }
        }
    }
}
