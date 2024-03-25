using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize]
     //restricts access to the controller actions to authenticated users only, ensuring that users must be logged in to perform related operations.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository; //abtraction for interacting with the Home repository data storage

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sterm = "", int genreId = 0)
        {
            try
            {

                IEnumerable<Book> books = await _homeRepository.GetBooks(sterm, genreId);
                IEnumerable<Genre> genres = await _homeRepository.Genres();
                BookDisplayModel bookModel = new BookDisplayModel
                {
                    Books = books,
                    Genres = genres,
                    STerm = sterm,
                    GenreId = genreId

                };
                return View(bookModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching books or genres.");
                return RedirectToAction("Error");
            }
          
        }

        public IActionResult ReturnPolicy()
        {
            return View();
        }

        //Handles errors in the application.

       [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}