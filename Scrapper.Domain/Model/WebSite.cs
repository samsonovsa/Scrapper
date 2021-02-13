using System.ComponentModel.DataAnnotations.Schema;

namespace Scrapper.Domain.Model
{
    [Table("LinkedinCandidatesWebSites")]
    public class WebSite
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string WebSiteName { get; set; }
        public string WebSiteUrl { get; set; }
        public bool IsMessenger { get; set; }
        public string WebSiteUrlComparison { get; set; }
    }
}
