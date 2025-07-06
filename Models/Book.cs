using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        [Display(Name = "Published Year")]
        public int PublishedYear { get; set; }
        public string Genre { get; set; }
        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<BookLoan>? BookLoans { get; set; }
    }
}
