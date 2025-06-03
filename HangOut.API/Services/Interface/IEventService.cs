using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Event;

namespace HangOut.API.Services.Interface
{
    public interface IEventService
    {
        Task<ApiResponse> CreateEvent(CreateEventRequest request);
    }
}
