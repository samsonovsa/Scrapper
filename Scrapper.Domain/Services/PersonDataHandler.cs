using Scrapper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Scrapper.Domain.Model;
using System.Linq;
using System.Text.RegularExpressions;
using Scrapper.Domain.Extensions;

namespace Scrapper.Domain.Services
{
    public class PersonDataHandler<T> : BaseDataHandler<T>
        where T: Person
    {
        private readonly Regex regexEmail = new Regex("\\S+@[0-9a-zA-Z_]+\\.\\S{2,3}");
        private readonly Regex regexEmailWithSpaces = new Regex("\\S+\\s?@\\s?[0-9a-zA-Z_]+\\s?\\.\\s?\\S{2,3}");
        private readonly Regex regexPhone = new Regex("(((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]){10,15}");
        //private readonly Regex regexPhone = new Regex("((\\+?7|8)[ \\-] ?)?((\\(\\d{3}\\))|(\\d{3}))?([ \\-])?(\\d{ 3}[\\- ]?\\d{ 2}[\\- ]?\\d{ 2})");
       

        public PersonDataHandler(IDbContext dbContext)
            : base(dbContext) { }

        public override List<T> PreprocessingEntities(List<T> entities)
        {
            return entities.Distinct(new PersonComparer<T>()).ToList();
        }

        public override async Task HandleEntity(T person)
        {
            EvaluatePerson(person);
            await AddOrUpdateIfNeed(person);
        }

        public void EvaluatePerson(Person person)
        {
            ExtractPhone(person);
            ExtractEmail(person);
            HahdleEmail(person);
            HahdleUrl(person);
            HahdleDescription(person);
        }

        private void HahdleDescription(Person person)
        {
            if (person.Description.IndexOf("-") > 1)
            {
                person.Name = person.Description.Substring(0, person.Description.IndexOf("-")).Trim();
                person.Description = person.Description.Remove(0, person.Description.IndexOf("-") + 1).Trim();

                if (person.Description.IndexOf("|") > 1)
                    person.Description = person.Description.Substring(0, person.Description.IndexOf(" | ")).Trim();
                else if (person.Description.IndexOf("...") > 1)
                    person.Description = person.Description.Substring(0, person.Description.IndexOf("...")).Trim();
            }

        }

        public void ExtractPhone(Person person)
        {
            foreach (var match in regexPhone.Matches(person.Description))
            {
                var phone = match.ToString().Trim();

                if (person.Url.Trim().IndexOf(phone.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    return;

                if ( !isExists(person.Phone, phone))
                    person.Phone = person.Phone.AddWithComma(match.ToString().Trim());
            }
        }

        private void ExtractEmail(Person person)
        {
            foreach (var match in regexEmail.Matches(person.Description))
            {
                person.Email = person.Email.AddWithComma(match.ToString());
            }

            if (string.IsNullOrEmpty(person.Email))
                foreach (var match in regexEmailWithSpaces.Matches(person.Description))
                {
                    var emailWithoutSpasces = match.ToString().Replace(" ", "");
                    person.Email = person.Email.AddWithComma(emailWithoutSpasces);
                }
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
            // person.UrlComparison = person.Url;
            person.Url = person.Url.Replace(@"https://ru.linkedin.com", @"https://www.linkedin.com");
            person.Url = person.Url.RemoveFrom("?");
            person.Url = person.Url.RemoveFrom("/ru-ru");
            //  person.Url = HttpUtility.UrlEncode(person.Url, Encoding.Unicode);
        }

        private async Task AddOrUpdateIfNeed(Person person)
        {
            var existPerson = DbContext.Persons.FirstOrDefault(p => p.Url.ToLower().Trim().Equals(person.Url.ToLower().Trim()));
            if (existPerson == null)
            {
               await Add(person);
            }
            else
            {
                Update(person, existPerson);
            }
        }

        private void Update(Person person, Person existPerson)
        {
            if (!string.IsNullOrEmpty(existPerson.Email)
                && !string.IsNullOrEmpty(person.Email)
                && !isExists(existPerson.Email, person.Email))
            {
                existPerson.Email = existPerson.Email.AddWithComma(person.Email);
            }

            if (!string.IsNullOrEmpty(existPerson.Phone)
                && !string.IsNullOrEmpty(person.Phone)
                && !isExists(existPerson.Phone, person.Phone))
            {
                existPerson.Phone = existPerson.Phone.AddWithComma(person.Phone);
            }
            existPerson.Photo = person.Photo;

            DbContext.Persons.Update(existPerson);
        }

        private async Task Add(Person person)
        { 
           if ((!string.IsNullOrEmpty(person.Email) || !string.IsNullOrEmpty(person.Phone)))
                await DbContext.Persons.AddAsync(person);
        }

        private bool isExists(string baseString, string newString)
        {
            if (string.IsNullOrEmpty(baseString))
                return false;

            return baseString.Trim().IndexOf(newString.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        private Person GetPersonFormDbContext(Person person)
        {
            return DbContext.Persons.FirstOrDefault(
                p => p.Url.Trim().Equals(person.Url.Trim(), System.StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
