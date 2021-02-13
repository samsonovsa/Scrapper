using Scrapper.Domain.Model;
using System.Threading.Tasks;

namespace Scrapper.Domain.Interfaces
{
    public interface IInputDataProvider
    {
        InputDataLists Data { get; set; }
        Task FillData();
    }
}
