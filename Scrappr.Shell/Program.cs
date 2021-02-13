using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Scrapper.DataAccess.DataBase;
using Scrapper.DataAccess.Reader;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scrapper.Shell
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Scrapper start:");

            var options = GetDbContextOptions();

            using (ApplicationContext db = new ApplicationContext(options))
            {
                var persons = db.Persons.ToList();
                foreach (var person in persons)
                {
                    Console.WriteLine($"{person.Id}.{person.Name}");
                }
            }

            var parser = new GoogleParser();
            await parser.GetSearchPage();
           
            Console.ReadKey();
        }

        private static IConfiguration GetConfiguration(string configFileName)
        {
            var builder = new ConfigurationBuilder();
            return builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName)
                .Build();
        }

        private static DbContextOptions<ApplicationContext> GetDbContextOptions()
        {
            var configuration = GetConfiguration("appsettings.json");
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            return optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
        }
    }
}
