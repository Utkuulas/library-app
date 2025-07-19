using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.Models.ViewModels
{
    public class RequestViewModel
    {
        public List<Request>? Requests { get; set; }
        public SelectList? Confirmation { get; set; }
        public PaginatedList<Request>? Pagination { get; set; }
        public int ConfirmationSwitch { get; set; }
        public string? SearchString { get; set; }
    }
}
