using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrapper.Domain.Model
{
    [Table("LinkedinCandidates")]
    public class Person
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        public string Description { get; set; }
        [MaxLength(200)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string Location { get; set; }
        [MaxLength(500)]
        public string UrlComparison { get; set; }
    }
}
