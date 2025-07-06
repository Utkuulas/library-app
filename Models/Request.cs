namespace LibraryApp.Models
{
    public class Request
    {
        public int Id { get; set; }
        public BookLoan? BookLoan { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
