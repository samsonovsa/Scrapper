using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrapper.Domain.Model
{
    [Table("LinkedinCandidatesWebSites")]
    public class WebSite
    {
        public long Id { get; set; }
        public string WebSiteName { get; set; }
        public string WebSiteUrl { get; set; }
        public bool IsMessenger { get; set; }
        [MaxLength(500)]
        public string WebSiteUrlComparison { get; set; }

        [ForeignKey("Person")]
        public int CandidateId { get; set; }
        public Person Person { get; set; }
    }
}
