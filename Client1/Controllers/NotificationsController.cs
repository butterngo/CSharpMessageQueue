namespace Client1.Controllers
{
    using CSharpMessageQueueClient;
    using CSharpMessageQueueClient.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;
    using System.Threading.Tasks;

    [ApiController]
    [Route("/api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly ICSharpClientConnectionFactory _client;

        public NotificationsController(ICSharpClientConnectionFactory client)
        {
            _client = client;
        }

        [Route("{message}")]
        public async Task<IActionResult> Post(string message)
        {
            await _client.SendAsync(new CSharpMessage
            {
                Body = Encoding.UTF8.GetBytes(message),
                Tos = new string[] { "client2" },
                Label = "test send to client 2"
            });

            return Ok("Done");
        }
    }
}
