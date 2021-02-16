using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public class InputDataProvider : IInputDataProvider
    {
        private readonly IFileReader _fileReader;
        private const string KeywordsFileName = "Keywords.txt";
        private const string LocationFileName = "Location.txt";
        private const string DomainFileName = "Domain.txt";
        private const string LinkedinFileName = "Linkedin.txt";

        public InputDataLists Data { get; set; }

        public InputDataProvider(IFileReader fileReader)
        {
            _fileReader = fileReader;
            Data = new InputDataLists();
        }

        public virtual async Task FillData()
        {
            try
            {
                Data.Keywords = await _fileReader.GetStringsAsync(KeywordsFileName);
                Data.Locations = await _fileReader.GetStringsAsync(LocationFileName);
                Data.Domains = await _fileReader.GetStringsAsync(DomainFileName);
                Data.Sites = await _fileReader.GetStringsAsync(LinkedinFileName);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка чтения файла", e);
            }
        }
    }
}
