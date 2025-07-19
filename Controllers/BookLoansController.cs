using Azure.Core;
using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    [Authorize]
    public class BookLoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookLoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookLoans
        public async Task<IActionResult> Index(int returningSwitch, string searchString, string currentFilter, string sortOrder, int? pageNumber)
        {
            if (_context.BookLoans == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookLoan' is null.");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["LoanDateSortParameter"] = sortOrder == "date" ? "date_desc" : "date";

            var bookLoans = _context.BookLoans
                .Include(b => b.Book)
                .Include(b => b.User).AsQueryable();

            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole != "Admin")
            {
                bookLoans = bookLoans.Where(bl => bl.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                bookLoans = bookLoans.Where(s => s.Book!.Title!.ToUpper().Contains(searchString.ToUpper()) ||
                                                  s.Book.ISBN!.ToUpper().Contains(searchString.ToUpper()) ||
                                                  s.User!.UserName!.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (returningSwitch)
            {
                case 1:
                    break;
                case 2:
                    bookLoans = bookLoans.Where(r => r.ReturnDate != null);
                    break;
                case 3:
                    bookLoans = bookLoans.Where(r => r.ReturnDate == null);
                    break;
            }

            switch (sortOrder)
            {
                case "date":
                    bookLoans = bookLoans.OrderBy(b => b.LoanDate);
                    break;
                case "date_desc":
                    bookLoans = bookLoans.OrderByDescending(b => b.LoanDate);
                    break;
                default:
                    bookLoans = bookLoans.OrderBy(b => b.Id);
                    break;
            }

            int pageSize = 10;
            var paginatedBookLoans = await PaginatedList<BookLoan>.CreateAsync(bookLoans, pageNumber ?? 1, pageSize);

            var bookLoanVM = new BookLoanViewModel
            {
                BookLoans = paginatedBookLoans.Select(b => new BookLoan
                {
                    Id = b.Id,
                    Book = b.Book,
                    LoanDate = b.LoanDate,
                    DueDate = b.DueDate,
                    ReturnDate = b.ReturnDate,
                    User = b.User
                }).ToList(),
                Pagination = paginatedBookLoans,
                ReturningSwitch = returningSwitch,
                SearchString = searchString
            };

            return View(bookLoanVM);
        }

        // GET: BookLoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            return View(bookLoan);
        }

        // GET: BookLoans/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            return View();
        }

        // POST: BookLoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoanDate,DueDate,ReturnDate,BookId,UserId")] BookLoan bookLoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookLoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookLoan.BookId);
            return View(bookLoan);
        }

        // GET: BookLoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookLoan.BookId);
            return View(bookLoan);
        }

        // POST: BookLoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LoanDate,DueDate,ReturnDate,BookId,UserId")] BookLoan bookLoan)
        {
            if (id != bookLoan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookLoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookLoanExists(bookLoan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookLoan.BookId);
            return View(bookLoan);
        }

        // GET: BookLoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            return View(bookLoan);
        }

        // POST: BookLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan != null)
            {
                _context.BookLoans.Remove(bookLoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkAsReturned(int id)
        {
            var bookLoan = await _context.BookLoans.Include(b => b.Book).FirstOrDefaultAsync(r => r.Id == id);

            if (bookLoan == null)
            {
                return NotFound();
            }

            bookLoan.Book!.IsAvailable = true;
            bookLoan.ReturnDate = DateTime.Now;
            _context.Update(bookLoan.Book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookLoanExists(int id)
        {
            return _context.BookLoans.Any(e => e.Id == id);
        }
    }
}
