using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace LibraryApp.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Books
        public async Task<IActionResult> Index(string bookGenre, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            ClearTempFiles();

            // Add sorting parameters
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentGenre"] = bookGenre;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParameter"] = sortOrder == "title" ? "title_desc" : "title";
            ViewData["AuthorSortParameter"] = sortOrder == "author" ? "author_desc" : "author";
            ViewData["ISBNSortParameter"] = sortOrder == "isbn" ? "isbn_desc" : "isbn";
            ViewData["PublishedYearSortParameter"] = sortOrder == "published_year" ? "published_year_desc" : "published_year";
            ViewData["GenreSortParameter"] = sortOrder == "genre" ? "genre_desc" : "genre";
            ViewData["IsAvailableSortParameter"] = sortOrder == "is_available" ? "is_available_desc" : "is_available";

            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book' is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from b in _context.Books
                                            orderby b.Genre
                                            select b.Genre;
            var books = from b in _context.Books.Include(b => b.Image)
                        select b;

            // Add searching filters
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()) ||
                s.ISBN!.ToUpper().Contains(searchString.ToUpper()) ||
                s.Author!.ToUpper().Contains(searchString.ToUpper()) ||
                s.PublishedYear.ToString().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(x => x.Genre == bookGenre);
            }

            switch (sortOrder)
            {
                case "title":
                    books = books.OrderBy(s => s.Title);
                    break;
                case "title_desc":
                    books = books.OrderByDescending(s => s.Title);
                    break;
                case "author":
                    books = books.OrderBy(s => s.Author);
                    break;
                case "isbn":
                    books = books.OrderBy(s => s.ISBN);
                    break;
                case "isbn_desc":
                    books = books.OrderByDescending(s => s.ISBN);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(s => s.Author);
                    break;
                case "published_year":
                    books = books.OrderBy(s => s.PublishedYear);
                    break;
                case "published_year_desc":
                    books = books.OrderByDescending(s => s.PublishedYear);
                    break;
                case "genre":
                    books = books.OrderBy(s => s.Genre);
                    break;
                case "genre_desc":
                    books = books.OrderByDescending(s => s.Genre);
                    break;
                case "is_available":
                    books = books.OrderBy(s => s.IsAvailable);
                    break;
                case "is_available_desc":
                    books = books.OrderByDescending(s => s.PublishedYear);
                    break;
                default:
                    books = books.OrderBy(s => s.Id);
                    break;
            }

            // Add pagination
            int pageSize = 3;
            var paginatedBooks = await PaginatedList<Book>.CreateAsync(books, pageNumber ?? 1, pageSize);

            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            var bookGenreVM = new BookGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                BookListItems = paginatedBooks.Select(b => new BookListItemViewModel
                {
                    Book = b,
                    IsRequestSent = currentUserRole == "Admin" ? false : _context.Requests.Any(r => r.Book!.Id == b.Id && r.User!.Id == currentUserId && r.IsConfirmed == false)
                }).ToList(),
                Pagination = paginatedBooks,
                SearchString = searchString,
                BookGenre = bookGenre
            };

            return View(bookGenreVM);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Image)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (TempData["FormData"] is string jsonData)
            {
                var book = JsonSerializer.Deserialize<Book>(jsonData);
                return View(book);
            }
            return View(new Book());
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, string tempImagePath)
        {
            ModelState.Remove(nameof(tempImagePath));

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(tempImagePath))
                {
                    AddImageFromTempFile(tempImagePath, book);
                }
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Image)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, string tempImagePath)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(tempImagePath));

            if (ModelState.IsValid)
            {
                try
                {
                    if (!tempImagePath.IsNullOrEmpty())
                    {
                        var oldImage = book.Image;

                        if(oldImage != null)
                        {
                            _context.Remove(oldImage);
                        }

                        AddImageFromTempFile(tempImagePath, book);
                    }
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Image).Where(b => b.Id == id).FirstOrDefaultAsync(); ;

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relatedRequests = await _context.Requests.Where(r => r.Book!.Id == id).ToListAsync();
            _context.Requests.RemoveRange(relatedRequests);

            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GenerateRequest(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            if (!book.IsAvailable)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId != null)
            {
                var isRequestSent = await _context.Requests.AnyAsync(r => r.Id == book.Id && r.User!.Id == currentUserId && !r.IsConfirmed && book.IsAvailable);

                if (isRequestSent)
                {
                    // If a request already exists, redirect to Index
                    return RedirectToAction(nameof(Index));
                }

                var request = new Request
                {
                    User = await _context.Users.FindAsync(currentUserId),
                    Book = await _context.Books.FindAsync(id),
                    IsConfirmed = false
                };
                _context.Add(request);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadTempImage(int? id, IFormFile file, Book book, string process)
        {
            if (id == 0)
            {
                TempData["FormData"] = JsonSerializer.Serialize(book);
            }       

            if (file != null && file.Length > 0)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var tempPath = Path.Combine("wwwroot/temp/", $"{currentUserId}_{file.FileName}");

                if (System.IO.File.Exists(tempPath))
                {
                    var counter = 1;

                    while (System.IO.File.Exists(tempPath))
                    {
                        // If the file already exists, append a number to the filename
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                        var extension = Path.GetExtension(file.FileName);

                        tempPath = Path.Combine("wwwroot/temp", $"{currentUserId}_{fileNameWithoutExtension}_{counter}{extension}");
                        counter++;
                    }
                }

                using (var fs = new FileStream(tempPath, FileMode.Create))
                {
                    TempData["TempImagePath"] = "/temp/" + Path.GetFileName(fs.Name);
                    await file.CopyToAsync(fs);
                }

                if (process.Equals("edit"))
                {
                    return RedirectToAction(nameof(Edit), "Books", new { id });
                }

                return RedirectToAction(nameof(Create));
            }

            TempData["TempImagePath"] = null;

            if (process.Equals("edit"))
            {
                return RedirectToAction(nameof(Edit), "Books", new { id });
            }

            return RedirectToAction(nameof(Create));
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        private void ClearTempFiles()
        {
            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null && currentUserRole == "Admin") 
            { 
                foreach(var filePath in Directory.GetFiles($"{_env.WebRootPath}/temp"))
                {
                    if (filePath.Contains(currentUserId))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }

        private void AddImageFromTempFile(string tempImagePath, Book book)
        {
            if (!tempImagePath.IsNullOrEmpty() && book != null)
            {
                var webRootPath = _env.WebRootPath;
                var fullPath = Path.Combine(webRootPath, tempImagePath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                {
                    var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                    var fileExtension = Path.GetExtension(fullPath);
                    var fileName = Path.GetFileName(fullPath);
                    var contentType = GetContentType(fullPath);

                    var image = new FileEntity
                    {
                        FileName = fileName,
                        FileType = contentType,
                        Data = fileBytes
                    };

                    _context.Files.Add(image);
                    book.Image = image;
                    ClearTempFiles();
                }
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        //private void ClearTempFile(string fullPath)
        //{
        //    if (System.IO.File.Exists(fullPath))
        //    {
        //        System.IO.File.Delete(fullPath);
        //    }
        //    else
        //    {
        //        Console.WriteLine("The file not found.");
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> UploadFile(int id, IFormFile file)
        //{
        //    var book = await _context.Books
        //        .Include(b => b.Image)
        //        .FirstOrDefaultAsync(b => b.Id == id);

        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    if (file != null && file.Length > 0)
        //    {
        //        using var memoryStream = new MemoryStream();
        //        await file.CopyToAsync(memoryStream);

        //        var image = new FileEntity
        //        {
        //            FileName = file.FileName,
        //            FileType = file.ContentType,
        //            Data = memoryStream.ToArray()
        //        };

        //        if (book.Image != null)
        //        {
        //            _context.Files.Remove(book.Image);
        //        }
        //        else
        //        {
        //            _context.Files.Add(image);
        //        }


        //        book.Image = image;
        //        _context.Books.Update(book);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Edit", "Books", new { id });
        //    }

        //    return RedirectToAction("Edit", "Books", new { id });
        //}

        //public IActionResult Download(int id)
        //{
        //    var file = _context.Files.Find(id);
        //    if (file == null)
        //        return NotFound();

        //    return File(file.Data, file.FileType, file.FileName);
        //}
    }
}
