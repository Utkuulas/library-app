using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.Models
{
    public class BookGenreViewModel
    {
        public List<Book>? Books { get; set; }
        public SelectList? Genres { get; set; }
        public string? BookGenre { get; set; }
        public string? SearchString { get; set; }
    }
}
