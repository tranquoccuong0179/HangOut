
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

        [HttpGet("get-events")]
        public async Task<IActionResult> GetEvents([FromQuery]int pageNumber, [FromQuery]int pageSize, 
            [FromQuery]string? searchKey, [FromQuery]string? location, [FromQuery]string? businessName)
        {
            var response  = await _eventService.GetEvents(pageNumber,pageSize, searchKey, location, businessName);
            return StatusCode(response.Status, response);
        }

        [HttpGet("get-event")]
        public async Task<IActionResult> GetEvent([FromQuery]Guid eventId)
        {
            var response = await _eventService.GetEvent(eventId);
            return StatusCode(response.Status, response);
        }
    }
}
