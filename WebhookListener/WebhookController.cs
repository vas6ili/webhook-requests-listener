using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace WebhookListener
{
    public class WebhookController : Controller
    {
        private readonly IHubContext<EventsHub> _hubContext;

        public WebhookController(IHubContext<EventsHub> hubContext)
        {
            _hubContext = hubContext;
        }


        [HttpPost("/delay/{timeout:int}/{*path}")]
        public async Task<IActionResult> Delay(int timeout, string path, [FromBody] JObject payload)
        {
            if (payload == null)
                return NotFound();

            await Task.Delay(timeout);

            await SendWebhookEvents(payload);
            return NoContent();
        }

        [HttpPost("/error/{*path}")]
        public async Task<IActionResult> Error(string path, [FromBody] JObject payload)
        {
            if (payload == null)
                return NotFound();

            await SendWebhookEvents(payload);
            return StatusCode(500);
        }

        [HttpPost("/webhook/{*path}")]
        public async Task<IActionResult> Success(string path, [FromBody] JObject payload)
        {
            if (payload == null)
            {              
                return NotFound();
            }

            await SendWebhookEvents(payload);
            return NoContent();
        }

        private async Task SendWebhookEvents(JObject payload)
        {
            var url = $"{HttpContext.Request.Method} {HttpContext.Request.GetDisplayUrl()}";
            var headers = HttpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            var requestAborted = HttpContext.RequestAborted.IsCancellationRequested;
            await _hubContext.Clients.All.SendAsync("addEvent", url, headers, payload, requestAborted);
        }
    }
}
