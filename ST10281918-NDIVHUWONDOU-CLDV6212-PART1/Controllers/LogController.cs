using Microsoft.AspNetCore.Mvc;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services.Storage;
using System.Threading.Tasks;

namespace ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Controllers
{
    public class LogController : Controller
    {
        private readonly QueueStorageService _queueStorageService;

        public LogController(QueueStorageService queueStorageService)
        {
            _queueStorageService = queueStorageService; 
        }
        public async Task<IActionResult> Index()
        {
            var log = await _queueStorageService.GetLogEntriesAsync();
            return View(log);
        }
    }
}
