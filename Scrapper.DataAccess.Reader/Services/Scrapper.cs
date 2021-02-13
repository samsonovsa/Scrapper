using PlaywrightSharp;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.DataAccess.Reader.Services
{
    public class Scrapper : IScrapper
    {
        private readonly IPage _page;
        private const string engineUrl = @"https://cse.google.com/cse?cx=009462381166450434430:dqo-6rxvieq";

        public Scrapper()
        {
        }

        public async Task<List<Person>> GetPersonsAsync(InputData inputData)
        {
            string query = $"'{inputData.Keyword}' '{inputData.Location}' '{inputData.Domain}'  -intitle:'profiles' -inurl:'dir/' site:{inputData.Site}";
            await SearchEngineTrackAsync(engineUrl, query);

        }

        private async Task SearchEngineTrackAsync(string url, string query)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync(headless: false);
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            var searchBar = "//input[@name='search']";
            await page.TypeAsync(searchBar, query);
            await page.PressAsync(searchBar, "Enter");
            await page.WaitForLoadStateAsync(LifecycleEvent.Networkidle);

            var timeout = (int)TimeSpan.FromMilliseconds(500).TotalMilliseconds;
            var pageButtons = page.QuerySelectorAsync(".gsc-cursor-page");

            if (pageButtons != null)
            {
                for (int i = 2; i < 11; i++)
                {
                    var dataBlocks = await page.QuerySelectorAllAsync(".gsc-webResult.gsc-result");
                    foreach (var block in dataBlocks)
                    {
                        await GetDataAsync(page, block);
                    }

                    await page.ClickAsync($".gsc-cursor-page[aria-label='Page {i}']", timeout);
                }
            }
        }

        private async Task GetDataAsync(IPage page, IElementHandle element)
        {
            var innerText = await element.EvaluateAsync<string>("e => e.innerText");
        }

        //private  IPage GetPageAsync()
        //{

        //}
    }
}
