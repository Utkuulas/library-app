using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Author is required.")]
        public string? Author { get; set; }
        [Required(ErrorMessage = "ISBN is required.")]
        public string? ISBN { get; set; }
        [Display(Name = "Published Year")]
        [Range(868, 9999, ErrorMessage = "Type a valid year")]
        public int PublishedYear { get; set; }
        [Required(ErrorMessage = "Genre is required.")]
        public string? Genre { get; set; }
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }
        public FileEntity? Image { get; set; }
        public virtual ICollection<BookLoan>? BookLoans { get; set; }
    }
}
