using HangOut.API.Services.Interface;
using HangOut.Domain.Entities;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Business;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;

namespace HangOut.API.Services.Implement
{
    public class BusinessService : BaseService<BusinessService>, IBusinessService
    {
        public BusinessService(IUnitOfWork<HangOutContext> unitOfWork, ILogger logger, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, httpContextAccessor)
        {
        }

        public async Task<ApiResponse<string>> CreateBusiness(Guid accountId, CreateBusinessRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var createBusiness = new Business
                {
                    Id = Guid.NewGuid(),
                    Active = request.Active,
                    Vibe = request.Vibe,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Address = request.Address,
                    Province = request.Province,
                    Name = request.Name,
                    Description = request.Description,
                    CreatedDate = request.CreatedDate,
                    LastModifiedDate = null,
                    AccountId = accountId
                };

                await _unitOfWork.GetRepository<Business>().InsertAsync(createBusiness);

                foreach(var image in request.ImageUrl)
                {
                    var newImage = new Image
                    {
                        Id= Guid.NewGuid(),
                        Url = image,
                        CreatedDate= DateTime.Now,
                        ObjectId = createBusiness.Id,
                        ImageType = Domain.Enums.EImageType.Business_Image,
                        LastModifiedDate = null,
                        IsMain = false,
                        EntityType = Domain.Enums.EntityTypeEnum.Business
                    };

                    await _unitOfWork.GetRepository<Image>().InsertAsync(newImage);
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new ApiResponse<string>
                {
                    Status = 200,
                    Message = "Create business success",
                    Data  = null
                };


            }
            catch (Exception ex) { 
            
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.ToString());
            }
        }
    }
}
