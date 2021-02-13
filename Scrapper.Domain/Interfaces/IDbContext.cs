using Microsoft.EntityFrameworkCore;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapper.Domain.Interfaces
{
    public interface IDbContext
    {
        DbSet<Candidate> Persons { get; set; }
        DbSet<WebSite> WebSites { get; set; }
    }
}
