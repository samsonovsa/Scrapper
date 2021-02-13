using Scrapper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public class ScrapersManager : IScrapersManager
    {
        private readonly IFileReader _fileReader;
        private readonly IDbContext _dbContext;

        public ScrapersManager(IFileReader fileReader, IDbContext dbContext)
        {
            _fileReader = fileReader;
            _dbContext = dbContext;
        }

        public Task ScrapLinkidinData()
        {
            throw new NotImplementedException();
        }

        public Task ScrapTlegramData()
        {
            throw new NotImplementedException();
        }
    }
}
