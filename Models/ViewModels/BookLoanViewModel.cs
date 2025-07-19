using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.Models.ViewModels
{
    public class BookLoanViewModel
    {
        public List<BookLoan>? BookLoans { get; set; }
        public SelectList? Returning { get; set; }
        public PaginatedList<BookLoan>? Pagination { get; set; }
        public int ReturningSwitch { get; set; }
        public string? SearchString { get; set; }
    }
}
