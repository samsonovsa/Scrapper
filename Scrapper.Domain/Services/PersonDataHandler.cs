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
    public class PersonDataHandler<T> : IDataHandler<T>
        where T: Person
    {
        private readonly IDbContext _dbContext;
        private readonly Regex regexEmail = new Regex("\\S+@[0-9a-zA-Z_]+\\.\\S{2,3}");
        private readonly Regex regexEmailWithSpaces = new Regex("\\S+\\s?@\\s?[0-9a-zA-Z_]+\\s?\\.\\s?\\S{2,3}");

        public PersonDataHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAndStoreAsync(List<T> persons)
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
                Console.WriteLine(e.Message);
            }
        }

        private void EvaluatePerson(Person person)
        {
            ExtractEmail(person);
            HahdleEmail(person);
            HahdleUrl(person);
            //  HahdleDescription(person);
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

        private void HahdleEmail(Person person)
        {
            if (string.IsNullOrEmpty(person.Email))
                return;

            if (char.IsPunctuation(person.Email.Last()))
                person.Email = person.Email.Remove(person.Email.Length - 1);
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

        private void HahdleUrl(Person person)
        {
            // person.UrlComparison = person.Url;
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

            if (!string.IsNullOrEmpty(existPerson.Email)
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
            if (!_dbContext.Persons.Any(p => p.Url.ToLower().Equals(person.Url.ToLower())))
            // && !string.IsNullOrEmpty(person.Email))
            {
                await _dbContext.Persons.AddAsync(person);
            }
        }

        private bool isExistsEmail(string baseEmail, string newEmail)
        {
            return baseEmail.Trim().IndexOf(newEmail.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
