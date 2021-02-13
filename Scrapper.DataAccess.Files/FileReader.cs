using Scrapper.Domain.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Scrapper.DataAccess.Files
{
    public class FileReader : IFileReader
    {
        public async Task<List<string>> GetStrings(string fileName)
        {
            List<string> result = new List<string>();

            FileInfo fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
                throw new FileNotFoundException(fileName);

            using (StreamReader reader = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    result.Add(line);
                }
            }

            return result;
        }
    }
}
