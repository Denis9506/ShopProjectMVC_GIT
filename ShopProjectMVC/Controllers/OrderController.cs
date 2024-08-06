using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;
using ShopProjectMVC.Filters;

namespace ShopProjectMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [AuthorizeFilter]
        public IActionResult Orders()
        {
            int id = HttpContext.Session.GetInt32("userId").Value;
            IEnumerable<Order> orders = _orderService.GetOrders(id).ToList();
            return View(orders);
        }
    }
}
