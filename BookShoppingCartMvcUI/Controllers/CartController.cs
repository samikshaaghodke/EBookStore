using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartRepository _cartRepo;

        public CartController(ILogger<CartController> logger, ICartRepository cartRepo)
        {
            _logger = logger;
            _cartRepo = cartRepo;
        }

        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            try
            {
                var cartCount = await _cartRepo.AddItem(bookId, qty);
                if (redirect == 0)
                    return Ok(cartCount);
                return RedirectToAction("GetUserCart"); // Redirects to Cart View
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an item to the cart.");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            try
            {
                var cartCount = await _cartRepo.RemoveItem(bookId);
                return RedirectToAction("GetUserCart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing an item from the cart.");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetUserCart()
        {
            try
            {
                var cart = await _cartRepo.GetUserCart();
                return View(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the user's cart.");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            try
            {
                int cartItem = await _cartRepo.GetCartItemCount();
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the total count of items in the cart.");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Checkout()
        {
            try
            {
                bool isCheckedOut = await _cartRepo.DoCheckout();
                if (!isCheckedOut)
                    throw new Exception("Something happened at the server side.");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking out.");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
