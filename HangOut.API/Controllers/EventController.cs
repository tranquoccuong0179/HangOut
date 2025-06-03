
using HangOut.API.Services.Implement;
using HangOut.API.Services.Interface;
using HangOut.Domain.Constants;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Event;
using Microsoft.AspNetCore.Mvc;

namespace HangOut.API.Controllers
{
    public class EventController : BaseController<EventController>
    {
        private readonly IEventService _eventService;
        public EventController(ILogger logger, IEventService eventService) : base(logger)
        {
            _eventService = eventService;
        }

        [HttpPost(ApiEndPointConstant.Event.CreateEvent)]
        public async Task<IActionResult> Register([FromForm] CreateEventRequest request)
        {
            var response = await _eventService.CreateEvent(request);
            return CreatedAtAction(nameof(Register), response);
        }
    }
}
