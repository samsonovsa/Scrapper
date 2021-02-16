using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Domain.Services
{
    public sealed class PhoneDataHandler<T> : BaseDataHandler<T>
        where T : Person
    {
        public PhoneDataHandler(IDbContext dbContext)
            : base(dbContext) { }

        public override Task HandleEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
