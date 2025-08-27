using Microsoft.AspNetCore.Mvc;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services.Storage;

namespace ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly BlobStorageService _blobStorageService;
        private readonly QueueStorageService _queueStorageService;
        private readonly FileShareStorageService _fileShareService;

        public ProductController(ProductService productService, BlobStorageService blobStorageService, QueueStorageService queueStorageService, FileShareStorageService fileShareStorageService)
        {
            _productService = productService;
            _blobStorageService = blobStorageService;
            _queueStorageService = queueStorageService;
            _fileShareService = fileShareStorageService;
        }
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
