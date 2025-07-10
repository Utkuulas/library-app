using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class BookLoan
    {
        [Display(Name = "Book Loan ID")]
        public int Id { get; set; }
        [Display(Name = "Loan Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime LoanDate { get; set; }
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Return Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? ReturnDate { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
