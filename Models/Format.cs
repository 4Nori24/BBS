namespace BBSWebApp.Models
{
    public class Format
    {
        public int PostId { get; set; }
        public int RowId { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }   
        public string Description { get; set; }
        public string Author { get; set; }
        public string AuthorUrl { get; set; }
        public string PostUrl { get; set; }
    }
}
