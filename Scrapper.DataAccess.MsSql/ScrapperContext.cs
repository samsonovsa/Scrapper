using Microsoft.EntityFrameworkCore;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System.Threading.Tasks;

namespace Scrapper.DataAccess.DataBase
{
    public class ScrapperContext : DbContext, IDbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<WebSite> WebSites { get; set; }

        public ScrapperContext(DbContextOptions<ScrapperContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
