using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class Request
    {
        [Display(Name = "Request ID")]
        public int Id { get; set; }
        public ApplicationUser? User { get; set; }
        public Book? Book{ get; set; }
        [Display(Name = "Confirmation Date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? ConfirmationDate { get; set; }
        [Display(Name = "Confirmed")]
        public bool IsConfirmed { get; set; }
    }
}
