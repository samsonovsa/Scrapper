using System.Collections.Generic;

namespace Scrapper.Domain.Model
{
    public class InputDataLists
    {
        public List<string> Keywords { get; set; }
        public List<string> Locations { get; set; }
        public List<string> Domains { get; set; }
        public List<string> Sites { get; set; }

        public InputDataLists()
        {
            Keywords = new List<string>();
            Locations = new List<string>();
            Domains = new List<string>();
            Sites = new List<string>();

        }
    }
}
