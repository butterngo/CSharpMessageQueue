namespace Client1.Controllers
{
    using Client1.Events;
    using CSharpEventBus;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("/api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public NotificationsController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [Route("")]
        public async Task<IActionResult> Post([FromBody] UserEvent dto)
        {
            await _eventBus.PublishAsync(dto, new string[] { "client2" });

            return Ok("Done");
        }
    }
}
