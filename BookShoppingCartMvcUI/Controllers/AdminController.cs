using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IActionResult> ManageGenres()
        {
            var genres = await _adminRepository.GetAllGenresAsync();
            return View(genres);
        }

        [HttpGet]
        public IActionResult AddGenre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre([Bind("GenreName")] GenreViewModel genreViewModel)
        {
            if (ModelState.IsValid)
            {
                var genre = new Genre { GenreName = genreViewModel.GenreName };

                // Attempt to add the genre
                var isGenreAdded = await _adminRepository.AddGenreAsync(genre);
                if (!isGenreAdded)
                {
                    ModelState.AddModelError(string.Empty, "Genre already exists.");                    
                    return View(genreViewModel);
                }
                else
                {                    
                    return RedirectToAction(nameof(ManageGenres));
                }
            }           
            return View(genreViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditGenre(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _adminRepository.GetGenreByIdAsync(id.Value);
            if (genre == null)
            {
                return NotFound();
            }
            var genreViewModel = new GenreViewModel { Id = genre.Id, GenreName = genre.GenreName };
            return View(genreViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditGenre(int id, [Bind("Id,GenreName")] GenreViewModel genreViewModel)
        {
            if (id != genreViewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Map the GenreViewModel back to Genre entity
                var genre = new Genre
                {
                    Id = genreViewModel.Id,
                    GenreName = genreViewModel.GenreName
                };

                await _adminRepository.UpdateGenreAsync(genre);
                return RedirectToAction(nameof(ManageGenres));
            }

            // If ModelState is not valid, return the view with the genreViewModel
            return View(genreViewModel);
        }
        // Now, for the book-related actions:

        public async Task<IActionResult> ManageBooks()
        {
            var books = await _adminRepository.GetAllBooksAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            ViewData["GenreId"] = new SelectList(_adminRepository.GetAllGenresAsync().Result, "Id", "GenreName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bookExists = await _adminRepository.BookExistsAsync(model.BookName, model.AuthorName, model.GenreId);
                if (bookExists)
                {
                    ModelState.AddModelError(string.Empty, "A book with the same name and author already exists.");
                }
                else
                {
                    var book = new Book
                    {
                        BookName = model.BookName,
                        AuthorName = model.AuthorName,
                        Price = model.Price,                       
                        GenreId = model.GenreId
                    };

                    bool isSuccess = await _adminRepository.AddBookAsync(book);
                    if (isSuccess)
                    {
                        return RedirectToAction(nameof(ManageBooks));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add the book. Please try again.");
                    }
                }
            }

            // If the operation was not successful, reload the form with the existing data
            ViewData["GenreId"] = new SelectList(await _adminRepository.GetAllGenresAsync(), "Id", "GenreName", model.GenreId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditBook(int? id)
        {
            if (id == null) return NotFound();

            var book = await _adminRepository.GetBookByIdAsync(id.Value);
            if (book == null) return NotFound();

            var model = new BookViewModel
            {
                Id = book.Id,
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                Price = book.Price,               
                GenreId = book.GenreId
            };

            ViewBag.GenreId = new SelectList(await _adminRepository.GetAllGenresAsync(), "Id", "GenreName", book.GenreId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Ensure the updated book does not conflict with existing books (excluding itself)
                var bookExists = await _adminRepository.BookExistsAsync(model.BookName, model.AuthorName, model.GenreId, model.Id);
                if (bookExists)
                {
                    ModelState.AddModelError("", "A book with the same name, author, and genre already exists.");
                    ViewData["GenreId"] = new SelectList(await _adminRepository.GetAllGenresAsync(), "Id", "GenreName", model.GenreId);
                    return View(model);
                }

                var book = await _adminRepository.GetBookByIdAsync(model.Id);
                if (book == null) return NotFound();

                book.BookName = model.BookName;
                book.AuthorName = model.AuthorName;
                book.Price = model.Price;               
                book.GenreId = model.GenreId;

                await _adminRepository.UpdateBookAsync(book);              
                return RedirectToAction(nameof(ManageBooks));
            }

            ViewData["GenreId"] = new SelectList(await _adminRepository.GetAllGenresAsync(), "Id", "GenreName", model.GenreId);
            return View(model);
        }

    }
}
