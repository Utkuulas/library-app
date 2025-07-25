﻿using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public RequestsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: Requests
        public async Task<IActionResult> Index(int confirmationSwitch, string searchString, string currentFilter, string sortOrder, int? pageNumber)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Request' is null.");
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ConfirmationDateSortParameter"] = sortOrder == "date" ? "date_desc" : "date";

            var requests = from r in _context.Requests
                           .Include(r => r.Book)
                           .Include(r => r.User)
                           select r;

            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole != "Admin")
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                requests = requests.Where(r => r.User!.Id == currentUserId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                requests = requests.Where(s => s.Book!.ISBN!.ToUpper().Contains(searchString.ToUpper()) ||
                                                s.Book!.Title!.ToUpper().Contains(searchString.ToUpper()) ||
                                                s.User!.Id.ToUpper().Contains(searchString.ToUpper()) ||
                                                s.User!.Email!.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (confirmationSwitch)
            {
                case 1:
                    break;
                case 2:
                    requests = requests.Where(r => r.IsConfirmed == true);
                    break;
                case 3:
                    requests = requests.Where(r => r.IsConfirmed == false);
                    break;
            }

            switch (sortOrder)
            {
                case "date":
                    requests = requests.OrderBy(r => r.ConfirmationDate);
                    break;
                case "date_desc":
                    requests = requests.OrderByDescending(r => r.ConfirmationDate);
                    break;
                default:
                    requests = requests.OrderBy(r => r.Id);
                    break;
            }

            int pageSize = 10;
            var paginatedRequests = await PaginatedList<Request>.CreateAsync(requests, pageNumber ?? 1, pageSize);

            var requestVM = new RequestViewModel
            {
                Requests = paginatedRequests.Select(r => new Request
                {
                    Id = r.Id,
                    Book = r.Book,
                    User = r.User,
                    ConfirmationDate = r.ConfirmationDate,
                    IsConfirmed = r.IsConfirmed,
                }).ToList(),
                Pagination = paginatedRequests,
                SearchString = searchString,
                ConfirmationSwitch = confirmationSwitch
            };

            return View(requestVM);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IsConfirmed")] Request request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsConfirmed")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
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
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(x => x.Book)
                .Include(x => x.Book!.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int id)
        {
            //var request = await _context.Request.FindAsync(id);
            var request = await _context.Requests
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            if (!request.Book!.IsAvailable)
            {
                return NotFound();
            }

            request.IsConfirmed = true;
            request.Book!.IsAvailable = false;
            request.ConfirmationDate = DateTime.Now;
            _context.Update(request);

            var bookLoan = new BookLoan
            {
                Book = request.Book,
                User = request.User,
                LoanDate = (DateTime)request.ConfirmationDate,
                DueDate = ((DateTime)request.ConfirmationDate).AddDays(Convert.ToDouble(_config["DueDateOffsetDays"]))
            };
            _context.BookLoans.Add(bookLoan);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
