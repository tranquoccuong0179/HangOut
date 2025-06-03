using HangOut.API.Common.Exceptions;
using HangOut.API.Common.Utils;
using HangOut.API.Services.Interface;
using HangOut.Domain.Entities;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Event;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;

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
    }
}
