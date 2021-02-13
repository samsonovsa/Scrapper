using Scrapper.DataAccess.Reader;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Scrapper.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Should_Get_ScreenShot()
        {
            var parser = new GoogleParser();
            await parser.GetScreenShot();
        }
        
        [Fact]
        public async Task Should_Open_GoogleSearsh()
        {
            var parser = new GoogleParser();
            await parser.GetSearchPage();
        }

        [Fact]
        public async Task Should_Intersect_Request()
        {
            var parser = new GoogleParser();
            await parser.RequestPageWithSearch();
        }
    }
}
