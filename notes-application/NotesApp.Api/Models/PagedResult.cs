namespace NotesApp.Api.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

}