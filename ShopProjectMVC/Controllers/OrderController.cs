using Microsoft.AspNetCore.Mvc;
using ShopProjectMVC.Core.Interfaces;
using ShopProjectMVC.Core.Models;
using ShopProjectMVC.Filters;
using System;

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
            try
            {
                int userId = HttpContext.Session.GetInt32("userId").Value;
                IEnumerable<Order> orders = _orderService.GetOrders(userId).ToList();
                return View(orders);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeFilter]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int userId = HttpContext.Session.GetInt32("userId").Value;
                var order = _orderService.GetOrders(userId).FirstOrDefault(x => x.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [AuthorizeFilter]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                int userId = HttpContext.Session.GetInt32("userId").Value;
                var order = _orderService.GetOrders(userId).FirstOrDefault(x => x.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                await _orderService.DeleteOrder(id);
                return RedirectToAction("Orders");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
