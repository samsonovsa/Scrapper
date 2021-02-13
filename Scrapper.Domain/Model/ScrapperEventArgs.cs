using System;

namespace Scrapper.Domain.Model
{
    public class ScrapperEventArgs: EventArgs
    {
        public string Message { get; }

        public ScrapperEventArgs(string message)
        {
            Message = message;
        }

        public ScrapperEventArgs(InputData inputData)
        {
            Message = $"scrapping {inputData.Keyword}-{inputData.Location}-{inputData.Domain}-{inputData.Site} complite";
        }
    }
}
