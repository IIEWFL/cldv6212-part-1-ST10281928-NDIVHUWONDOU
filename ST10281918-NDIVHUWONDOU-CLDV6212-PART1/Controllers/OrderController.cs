using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Models;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services.Storage;
using System.Threading.Tasks;

namespace ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        //private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly QueueStorageService _queueStorageService;
        private readonly FileShareStorageService _fileShareService;

        public OrderController(OrderService orderService, CustomerService customerService, ProductService productService, QueueStorageService queueStorageService, FileShareStorageService fileShareStorageService)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
            _queueStorageService = queueStorageService;
            _fileShareService = fileShareStorageService;

        }

        public async Task<IActionResult> Index()
        {
            var customer = await _customerService.GetAllCustomersAsync();
            var product = await _productService.GetAllProductAsync();
            var order = await _orderService.GetAllOrdersAsync();

            var orderList = order.Select(o => new
            {
                Order = o,
                CustomerName = customer.FirstOrDefault(c => c.RowKey == o.CustomerId)?.CustomerFirstName ?? "Unknown",
                ProductName = product.FirstOrDefault(p => p.RowKey == p.ProductId)?.ProductName ?? "Unknown",
            });

            ViewBag.Orders = orderList;
            return View(order);
        }

        // GET: /Releases/Details/{partitionKey}/{rowKey}
        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var order = await _orderService.GetOrderAsync(partitionKey, rowKey);
            if (order == null) return NotFound();

            var customer = await _customerService.GetCustomerAsync("Customer", order.CustomerId!);
            var product = await _productService.GetProductAsync("Product", order.ProductId!);

            ViewBag.CustomerName = customer?.CustomerFirstName ?? "Unknown";
            ViewBag.ProductName = product?.ProductName ?? "Unknown";

            return View(order);
        }

        // GET: /Releases/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // POST: /Releases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.PartitionKey = "order";
                order.RowKey = Guid.NewGuid().ToString();
                order.OrderDate = DateTime.UtcNow;

                await _orderService.AddOrderAsync(order);


                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(order);
        }

        // GET: /Releases/Edit/{partitionKey}/{rowKey}
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var release = await _orderService.GetOrderAsync(partitionKey, rowKey);
            if (release == null) return NotFound();

            await PopulateDropdowns();
            return View(release);
        }

        // POST: /Release/Edit/{partitionKey}/{rowKey}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.UpdateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdowns();
            return View(order);
        }

        // GET: /Releases/Delete/{partitionKey}/{rowKey}
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var order = await _orderService.GetOrderAsync(partitionKey, rowKey);
            if (order == null) return NotFound();

            var customer = await _customerService.GetCustomerAsync("Customer", order.CustomerId!);
            var product = await _productService.GetProductAsync("Product", order.ProductId!);

            ViewBag.CustomerName = customer?.CustomerFirstName ?? "Unknown";
            ViewBag.ProductName = product?.ProductName ?? "Unknown";

            return View(order);
        }

        // POST: /Releases/DeleteConfirmed/{partitionKey}/{rowKey}
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            await _orderService.DeleteOrderAsync(partitionKey, rowKey);
            return RedirectToAction(nameof(Index));
        }

        // Helper: populate dropdown lists
        private async Task PopulateDropdowns()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var products = await _productService.GetAllProductAsync();

            ViewBag.Customers = new SelectList(customers, "RowKey", "Name");
            ViewBag.Products = new SelectList(products, "RowKey", "Name");
        }
    }
}

