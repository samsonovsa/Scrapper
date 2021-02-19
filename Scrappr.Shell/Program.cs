using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Scrapper.DataAccess.DataBase;
using Scrapper.DataAccess.Files;
using Scrapper.DataAccess.Reader.Services;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using Scrapper.Domain.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Scrapper.Shell
{
    class Program
    {
        private static readonly string settingsFileName = "appsettings.json";

        static async Task Main(string[] args)
        {
            ShowMenu();
            await HandleMenuAsync();

            Console.WriteLine("Scrapping finished");
            Console.ReadKey();
        }

        private static void ShowMenu()
        {
            Console.WriteLine(@"Выберите дальнейшее действие:");
            Console.WriteLine("------------------------------\n");
            Console.WriteLine("\t1 - Сбор информации по доменам.");
            Console.WriteLine("\t2 - Сбор информации по телефонам.");
            Console.WriteLine("\t3 - Сбор информации по telegram.");
            Console.WriteLine("\t0 - Завершение работы.\n");
            Console.Write("Ваш выбор? ");
        }


        private static async Task HandleMenuAsync()
        {
            var configuration = GetConfiguration(settingsFileName);
            var options = GetDbContextOptions(configuration);
            var dbContext = new ScrapperContext(options);
            IDataHandler<Person> handler = new PersonDataHandler<Person>(dbContext);
            InputDataProvider inputData = await InputDataProviderFactory.GetInputDataProviderForPerson();

            switch (Console.ReadLine())
            {
                case "1":
                    break;
                case "2":
                    string PhoneTemplate = configuration[nameof(PhoneTemplate)];
                    inputData = await InputDataProviderFactory.GetInputDataProviderForPersonWithSpecialDomen(PhoneTemplate);
                    break;
                case "3":
                    string SiteTemplate = configuration[nameof(SiteTemplate)];
                    handler = new TelegramDataHandler<Person>(dbContext);
                    inputData = await InputDataProviderFactory.GetInputDataProviderForPersonWithSpecialDomen(SiteTemplate);
                    break;
                case "0":
                    return;
            }

            Console.WriteLine();
            Console.WriteLine("Start scrapping process...\n");

            await RunScrappingAsync(handler, inputData);
        }

        private static async Task RunScrappingAsync(IDataHandler<Person> handler, InputDataProvider inputData)
        {
            IScrapper<Person> scrapper = new Scrapper<Person>();
            IScrapersManager scrapperManager = new ScrapersManager(inputData, handler, scrapper);
            scrapperManager.Notify += ScrapperManagerNotify;

            await scrapperManager.ScrapDataAsync();
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

        private static DbContextOptions<ScrapperContext> GetDbContextOptions(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ScrapperContext>();
            return optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
        }
    }
}
