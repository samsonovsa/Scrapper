using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Scrapper.DataAccess.DataBase;
using Scrapper.DataAccess.Files;
using Scrapper.DataAccess.Reader;
using Scrapper.DataAccess.Reader.Services;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using Scrapper.Domain.Services;
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
            Console.WriteLine("Выберите дальнейшее действие:");

            Console.WriteLine("1 Сбор информации по доменам.");
            Console.WriteLine("2 Сбор информации по telegram.");
            Console.WriteLine("0 Завершение работы.");

            Console.WriteLine("Scrapper start:");
            var options = GetDbContextOptions();

            var dbContext = new ScrapperContext(options);
            var inputData = new InputDataProvider(new FileReader());
            await inputData.FillData();

            IScrapper<Person> scrapper = new Scrapper<Person>();
            IScrapersManager scrapperManager = new ScrapersManager(inputData, dbContext, scrapper);
            scrapperManager.Notify += ScrapperManagerNotify;

            await scrapperManager.ScrapLinkidinDataAsync();

            //using (ScrapperContext db = new ScrapperContext(options))
            //{
            //    var persons = db.Persons.ToList();
            //    foreach (var person in persons)
            //    {
            //        Console.WriteLine($"{person.Id}.{person.Name}");
            //    }
            //}



            //var parser = new GoogleParser();
            //await parser.GetSearchPage();
            Console.WriteLine("Scrapping finished");
            Console.ReadKey();
        }

        private static void ScrapperManagerNotify(object sender, ScrapperEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static IConfiguration GetConfiguration(string configFileName)
        {
            var builder = new ConfigurationBuilder();
            return builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName)
                .Build();
        }

        private static DbContextOptions<ScrapperContext> GetDbContextOptions()
        {
            var configuration = GetConfiguration("appsettings.json");
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ScrapperContext>();
            return optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
        }
    }
}
