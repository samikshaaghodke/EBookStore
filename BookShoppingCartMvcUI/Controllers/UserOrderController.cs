using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly ILogger<UserOrderController> _logger;
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(ILogger<UserOrderController> logger, IUserOrderRepository userOrderRepo)
        {
            _logger = logger;
            _userOrderRepo = userOrderRepo;
        }

        public async Task<IActionResult> UserOrders()
        {
            try
            {
                var orders = await _userOrderRepo.UserOrders();
                return View(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user orders.");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
