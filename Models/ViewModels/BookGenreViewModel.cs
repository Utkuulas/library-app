using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.Models.ViewModels
{
    public class BookGenreViewModel
    {
        public List<BookListItemViewModel>? BookListItems { get; set; }
        public SelectList? Genres { get; set; }
        public string? BookGenre { get; set; }
        public string? SearchString { get; set; }
    }
}
