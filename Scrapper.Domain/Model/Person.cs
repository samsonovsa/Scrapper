using System.ComponentModel.DataAnnotations.Schema;

namespace Scrapper.Domain.Model
{
    [Table("LinkedinCandidates")]
    public class Person
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string Location { get; set; }
    }
}
