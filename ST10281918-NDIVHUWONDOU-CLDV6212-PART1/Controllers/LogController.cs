using Microsoft.AspNetCore.Mvc;
using ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services.Storage;
using System.Threading.Tasks;
using System.Text;
namespace ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Controllers
{
    public class LogController : Controller
    {
        private readonly QueueStorageService _queueStorageService;
        private readonly FileShareStorageService _fileShareService;
        public LogController(QueueStorageService queueStorageService, FileShareStorageService fileShareService)
        {
            _queueStorageService = queueStorageService;
            _fileShareService = fileShareService;
        }
        public async Task<IActionResult> Index()
        {
            var log = await _queueStorageService.GetLogEntriesAsync();
            return View(log);
        }

        //Code Attribution
        //This method was taken form stackoverflow
        //https://stackoverflow.com/questions/18757097/writing-data-into-csv-file-in-c-sharp
        //TylerH and Pavel Murygin
        //https://stackoverflow.com/users/2756409/tylerh
        //https://stackoverflow.com/users/731793/pavel-murygin


        // POST: /AuditLogs/Export
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export()
        {
            var logs = await _queueStorageService.GetLogEntriesAsync();

            // Optional: include timestamp in filename
            var fileName = $"QueueLog_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                await writer.WriteLineAsync("MessageId,InsertionTime,Message");

                foreach (var log in logs)
                {
                    var logText = log.RawMessage?.Replace("\"", "\"\"");
                    await writer.WriteLineAsync($"\"{log.MessageId}\", \"{log.InsertionTime?.ToString("yyyyMMdd_HHmmss")}\",\"{logText}\"");
                }

                await writer.FlushAsync();

                stream.Position = 0;
                await _fileShareService.UpLoadFile(fileName, stream);
            }


            //await _fileShareService.UpLoadFile(fileName,fileStream);

            TempData["Message"] = $"Queue log exported successfully as {fileName}";
            return RedirectToAction(nameof(Index));
        }
    }
}
