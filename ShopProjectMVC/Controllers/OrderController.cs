using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;

namespace ShopProjectMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Orders()
        {
            int userId = 1; 
            IEnumerable<Order> orders = _orderService.GetOrders(userId).ToList();
            return View(orders);
        }
    }
}
