using Scrapper.DataAccess.Files;
using Scrapper.Domain.Model;
using Scrapper.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scrapper.Shell
{
    public class InputDataProviderFactory
    {
        private static async Task<InputDataProvider> GetInputDataProvider()
        {
            var inputData = new InputDataProvider(new FileReader());
            await inputData.FillData();
            return inputData;
        }

        public static async Task<InputDataProvider> GetInputDataProviderForPerson()
        {
            return await GetInputDataProvider();
        }

        public static async Task<InputDataProvider> GetInputDataProviderForPersonWithSpecialDomen(string domen)
        {
            var inputData = await GetInputDataProvider();
            inputData.Data.Domains = new List<string> { domen };

            return inputData;
        }
    }
}
