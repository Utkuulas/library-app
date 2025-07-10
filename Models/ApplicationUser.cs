using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "User ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public virtual ICollection<BookLoan>? BookLoans { get; set; }
    }
}
