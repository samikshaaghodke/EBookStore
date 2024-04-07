using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using BookShoppingCartMvcUI.Controllers;
using BookShoppingCartMvcUI.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookShoppingCartMvcUITests
{
    [TestFixture]
    public class UserOrderControllerTests
    {
        private Mock<ILogger<UserOrderController>> _loggerMock;
        private Mock<IUserOrderRepository> _userOrderRepoMock;
        private UserOrderController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<UserOrderController>>();
            _userOrderRepoMock = new Mock<IUserOrderRepository>();
            _controller = new UserOrderController(_loggerMock.Object, _userOrderRepoMock.Object);
        }

        // Test cases will go here
        [Test]
        public async Task UserOrders_ReturnsViewWithOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    UserId = "15e5b60b-b852-4c6c-b0a6-2419fb0fbefc",
                    CreateDate = DateTime.Now,
                    OrderStatusId = 1, 
                    IsDeleted = false,
                    OrderStatus = new OrderStatus
                    {
                        Id = 1,
                        StatusName = "Pending" // Example status
                    },
                    OrderDetail = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            Id = 1,
                            OrderId = 1,
                            BookId = 4, 
                            Quantity = 2,
                            UnitPrice = 15,
                            Book = new Book 
                            {
                                Id = 1,
                                BookName = "The Great Gatsby",
                                Price = 15,
                                AuthorName = "F. Scott Fitzgerald",
                                GenreId = 12 
                            }
                        }
                    }
                }
            };
            _userOrderRepoMock.Setup(repo => repo.UserOrders()).ReturnsAsync(orders);

            // Act
            var result = await _controller.UserOrders();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(orders, viewResult.Model);
        }

        [Test]
        public async Task UserOrders_RedirectsToError_WhenExceptionOccurs()
        {
            // Arrange
            _userOrderRepoMock.Setup(repo => repo.UserOrders()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.UserOrders();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }


    }
}
