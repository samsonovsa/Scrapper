using Microsoft.EntityFrameworkCore;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;

namespace Scrapper.DataAccess.DataBase
{
    public class ApplicationContext : DbContext, IDbContext
    {
        public DbSet<Candidate> Persons { get; set; }
        public DbSet<WebSite> WebSites { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        //}

    }
}
