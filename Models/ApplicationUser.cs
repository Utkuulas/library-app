using Microsoft.AspNetCore.Identity;

namespace LibraryApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Book>? Books { get; set; }
    }
}
