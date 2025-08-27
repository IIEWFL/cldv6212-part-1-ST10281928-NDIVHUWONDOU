using Microsoft.AspNetCore.Mvc;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Models;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services.Storage;
using System.Threading.Tasks;

namespace ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly QueueStorageService _queueStorageService;
        private readonly FileShareStorageService _fileShareService;

        public OrderController(OrderService orderService, QueueStorageService queueStorageService, FileShareStorageService fileShareStorageService)
        {
            _orderService = orderService;
            _queueStorageService = queueStorageService;
            _fileShareService = fileShareStorageService;

        }

        public async Task<IActionResult> Index()
        {
            var order = await _orderService.GetAllOrdersAsync();
            return View(order);
        }
    }
}
