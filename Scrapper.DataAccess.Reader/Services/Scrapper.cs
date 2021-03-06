﻿using PlaywrightSharp;
using Scrapper.Domain.Extensions;
using Scrapper.Domain.Interfaces;
using Scrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scrapper.DataAccess.Reader.Services
{
    public class Scrapper<TEntity> : IScrapper<TEntity>
        where TEntity: Person, new()
    {
        private readonly IPage _page;
        private const string engineUrl = @"https://cse.google.com/cse?cx=009462381166450434430:dqo-6rxvieq";

        public Scrapper()
        {
        }

        public async Task<List<TEntity>> GetPersonsAsync(InputData inputData)
        {
            string query = $"'{inputData.Keyword}' '{inputData.Location}' '{inputData.Domain}'  -intitle:'profiles' -inurl:'dir/' site:{inputData.Site}/in/";
            return await GetPersonsFromSearchEngineTrackAsync(engineUrl, query);
        }

        private async Task<List<TEntity>> GetPersonsFromSearchEngineTrackAsync(string url, string query)
        {
            var entities = new List<TEntity>();
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync(headless: false);
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url);

            var searchBar = "//input[@name='search']";
            await page.TypeAsync(searchBar, query);
            await page.PressAsync(searchBar, "Enter");
            await page.WaitForLoadStateAsync(LifecycleEvent.Networkidle);

            var pageButtons = page.QuerySelectorAsync(".gsc-cursor-page");

            if (pageButtons != null)
            {
                for (int i = 2; i < 11; i++)
                {
                    var dataBlocks = await page.QuerySelectorAllAsync(".gsc-webResult.gsc-result");
                    foreach (var block in dataBlocks)
                    {
                        var entity = await GetDataAsync(block);
                        if(entity != null && !string.IsNullOrEmpty(entity.Url))
                            entities.Add(entity);
                    }

                    var nextButton = await page.QuerySelectorAsync($".gsc-cursor-page[aria-label='Page {i}']");
                    if (nextButton == null)
                        break;

                    try
                    {
                        await page.ClickAsync($".gsc-cursor-page[aria-label='Page {i}']");
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }

            return entities;
        }

        private async Task<TEntity> GetDataAsync(IElementHandle element)
        {
            var entity = new TEntity();

            var title = await element.QuerySelectorAsync("a.gs-title");
            if (title == null)
                return entity;

            entity.Url = await title.EvaluateAsync<string>("e => e.getAttribute('href')");
            var innerText = await element.EvaluateAsync<string>("e => e.innerText");
            entity.Description = innerText;
            entity.Photo =  await GetPhoto(element);

            return entity;
        }

        private async Task<string> GetPhoto(IElementHandle element)
        {
            var photoTag = await element.QuerySelectorAsync("img.gs-image");

            if (photoTag != null)
                return await photoTag.EvaluateAsync<string>("e => e.getAttribute('src')");
            else
                return string.Empty;
        }
    }
}
