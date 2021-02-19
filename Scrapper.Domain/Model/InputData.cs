namespace Scrapper.Domain.Model
{
    public class InputData
    {
        public string Keyword { get; set; }
        public string Location { get; set; }
        public string Domain { get; set; }
        public string Site { get; set; }

        public override string ToString()
        {
            return $"{Keyword} - {Location} - {Domain} - {Site}";
        }
    }
}
