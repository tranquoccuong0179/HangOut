using System.Linq.Expressions;
using HangOut.API.Common.Exceptions;
using HangOut.API.Common.Utils;
using HangOut.API.Services.Interface;
using HangOut.Domain.Entities;
using HangOut.Domain.Paginate;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Event;
using HangOut.Domain.Payload.Response.Event;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using MimeKit.Utils;

namespace HangOut.API.Services.Implement
{
    public class EventService : BaseService<EventService>, IEventService
    {
        private readonly IUploadService _uploadService;
        public EventService(IUnitOfWork<HangOutContext> unitOfWork, ILogger logger, IHttpContextAccessor httpContextAccessor, IUploadService uploadService) : base(unitOfWork, logger, httpContextAccessor)
        {
            _uploadService = uploadService;
        }

        public async Task<ApiResponse> CreateEvent(CreateEventRequest request)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(accountId) && a.Active == true);
            if (account == null)
            {
                throw new NotFoundException("Không tìm thấy tài khoản");
            }

            var business = await _unitOfWork.GetRepository<Business>().SingleOrDefaultAsync(
                predicate: b => b.AccountId.Equals(accountId) && b.Active == true);

            if (account == null)
            {
                throw new NotFoundException("Không tìm thấy tài khoản");
            }
            var eventBusiness = new Event()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                StartDate = request.StartDate,
                DueDate = request.DueDate,
                Location = request.Location,
                Active = true,
                Description = request.Description,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                MainImageUrl = await _uploadService.UploadImageAsync(request.MainImageUrl),
                BusinessId = business.Id,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            await _unitOfWork.GetRepository<Event>().InsertAsync(eventBusiness);

            if (request.Images != null)
            {
                foreach (var image in request.Images)
                {
                    var eventImage = new EventImage()
                    {
                        Id = Guid.NewGuid(),
                        Url = await _uploadService.UploadImageAsync(image),
                        EventId = eventBusiness.Id,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    };
                    await _unitOfWork.GetRepository<EventImage>().InsertAsync(eventImage);
                }
            }
            await _unitOfWork.CommitAsync();

            return new ApiResponse
            {
                Status = 200,
                Message = "Tạo event thành công",
                Data = null
            };
        }

        public async Task<ApiResponse<Paginate<GetEventsResponse>>> GetEvents(int pageNumber, int pageSize, string searchKey, string location, string businessName)
        {
            Expression<Func<Event, bool>> predicate = x => x.Active == true && x.StartDate > DateTime.Now;

            if (!string.IsNullOrEmpty(searchKey))
            {
                predicate = x => x.Active == true && x.StartDate > DateTime.Now && x.Name.Contains(searchKey);
            }

            if (!string.IsNullOrEmpty(location))
            {
                predicate = x => x.Active == true && x.StartDate > DateTime.Now && x.Location.Equals(location);
            }

            if (!string.IsNullOrEmpty(businessName))
            {
                predicate = x => x.Active == true && x.StartDate > DateTime.Now && x.Location.Equals(businessName);
            }


            var getEvents = await _unitOfWork.GetRepository<Event>().GetPagingListAsync(
                predicate: predicate,
                include: x => x.Include(b => b.Business).Include(i => i.EventImages),
                page: pageNumber,
                size: pageSize);

            var mapItem = getEvents.Items.Select(x => new GetEventsResponse
            {
                Id = x.Id,
                Name = x.Name,
                MainImage = x.MainImageUrl,
                Description = x.Description,
                ComingDay = DateUtil.GetDurationString(DateTime.Now,x.StartDate)
            }).ToList();

            var pagedResponse = new Paginate<GetEventsResponse>
            {
                Items = mapItem.ToList(),
                Page = pageNumber,
                Size = pageSize,
                Total = getEvents.Total,
                TotalPages = (int)Math.Ceiling((double)getEvents.Total / pageSize)
            };

            return new ApiResponse<Paginate<GetEventsResponse>>
            {
                Status = StatusCodes.Status200OK,
                Message = "Get events success",
                Data = pagedResponse,
            };
        }

        public async Task<ApiResponse<GetEventResponse>> GetEvent(Guid eventId)
        {
            var getEvent = await _unitOfWork.GetRepository<Event>().SingleOrDefaultAsync(
                predicate: x => x.Id ==  eventId,
                include: i => i.Include(i => i.EventImages)
                );

            if (getEvent == null)
            {
                return new ApiResponse<GetEventResponse>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "Event not found",
                    Data = null
                };
            }

            var mapItem = new GetEventResponse
            {
                EventId = getEvent.Id,
                Name = getEvent.Name,
                Description = getEvent.Description,
                MainImageUrl = getEvent.MainImageUrl,
                StartDate = getEvent.StartDate,
                DueDate = getEvent.DueDate,
                Active = getEvent.Active,
                Latitude = getEvent.Latitude,
                Longitude = getEvent.Longitude,
                Location = getEvent.Location,
                ComingDay = DateUtil.GetDurationString(DateTime.Now, getEvent.StartDate),
                Images = getEvent.EventImages.Select(c => new Images
                {
                    Id = c.Id,
                    ImageUrl = c.Url
                }).ToList()
            };

            return new ApiResponse<GetEventResponse>
            {
                Status = StatusCodes.Status200OK,
                Message = "Get event success",
                Data = mapItem
            };
        }
    }
}
