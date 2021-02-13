using Microsoft.EntityFrameworkCore;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IDbContext
    {
        DbSet<Person> Persons { get; set; }
        DbSet<WebSite> WebSites { get; set; }

        Task SaveChangesAsync();
    }
}
