using PlaywrightSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.DataAccess.Reader
{
    public class GoogleParser
    {
        private string requestUrl;

        public async Task GetScreenShot()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GoToAsync("http://www.google.com");
            await page.ScreenshotAsync(path: "screen.jpg");
        }

        public async Task GetSearchPage()
        {
            string url = "https://cse.google.com/cse?cx=009462381166450434430:dqo-6rxvieq";
            string query = "'developer' 'moscow' 'gmail'  -intitle:'profiles' -inurl:'dir/' site:ru.linkedin.com/in/";

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync(headless: false);
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            // gsc-i-id1
            // name
            var searchBar = "//input[@name='search']";
            await page.TypeAsync(searchBar, query);
           // await page.PressAsync(searchBar, "Enter");

            var buttonSearch = ".gsc-search-button.gsc-search-button-v2";

            await page.ClickAsync(buttonSearch);
            await page.WaitForLoadStateAsync(LifecycleEvent.Networkidle);
            var timeout = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
            var pageButtons = page.QuerySelectorAsync(".gsc-cursor-page");

            if (pageButtons != null)
            {
                for (int i = 2; i < 11; i++)
                {
                    // Intercept network requests
                    await page.RouteAsync("**", (route, _) =>
                    {
                        Console.WriteLine($"step {i}");
                        Console.WriteLine(route.Request.Url);
                        route.ContinueAsync();
                         //string newUrl = route.Request.Url.Replace("page=2", $"page={i}");
                         //route.ContinueAsync(url: newUrl);
                    });

                    var dataBlocks = await page.QuerySelectorAllAsync(".gsc-webResult.gsc-result");
                    foreach (var block in dataBlocks)
                    {
                        await GetData(page, block);
                    }


                    await page.ClickAsync($".gsc-cursor-page[aria-label='Page {i}']", timeout);
                    //await page.ClickAsync($".gsc-cursor-page[aria-label='Page 2']", timeout);
                }
            }
        }

        private async Task GetData(IPage page, IElementHandle element)
        {
            var innerText = await element.EvaluateAsync<string>("e => e.innerText");
            Console.WriteLine(innerText);
        }

        public async Task RequestPageWithSearch()
        {
            string url = "https://cse.google.com/cse?cx=009462381166450434430:dqo-6rxvieq";
            string query = "'developer' 'moscow' 'gmail'  -intitle:'profiles' -inurl:'dir/' site:ru.linkedin.com/in/";

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync(headless: false);
            var page = await browser.NewPageAsync();

            await page.GoToAsync(url);
            // gsc-i-id1
            // name
            var searchBar = "//input[@name='search']";
            await page.TypeAsync(searchBar, query);
            // await page.PressAsync(searchBar, "Enter");

            var buttonSearch = ".gsc-search-button.gsc-search-button-v2";

            var responseTcs = new TaskCompletionSource<IResponse>();
            page.Response += (sender, e) =>
            {
                responseTcs.TrySetResult(e.Response);
            };

            await page.ClickAsync(buttonSearch);
            var response = await responseTcs.Task;
            var json = await response.GetTextAsync();


            //page.Response += Page_Response;
            //await page.ClickAsync(buttonSearch);
            //page.Response -= Page_Response;

           // var timeout = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;           
           // var requestTask = page.WaitForRequestAsync(requestUrl, timeout);
           // var responseTask = page.WaitForResponseAsync(requestUrl, timeout);
           //// await page.GoToAsync("https://github.com/microsoft/playwright-sharp");
           // await Task.WhenAll(requestTask, responseTask);

            //using(HttpClient client = new HttpClient())
            //{
            //    var response = await client.GetAsync(requestUrl);
            //    if (response.IsSuccessStatusCode)
            //        await response.Content.ReadAsStringAsync();
            //}

            await page.WaitForLoadStateAsync(LifecycleEvent.Networkidle);
        }

        private async void Page_Response(object sender, ResponseEventArgs e)
        {
            //var response = await  e.Response.GetJsonAsync();

            requestUrl = e.Response.Request.Url;

          //  Console.WriteLine(response.ToString());
        }
    }
}
