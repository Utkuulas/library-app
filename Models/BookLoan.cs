using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models
{
    public class BookLoan
    {
        public int Id { get; set; }
        [Display(Name = "Loan Date")]
        public DateTime LoanDate { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
