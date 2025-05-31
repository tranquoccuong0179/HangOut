using HangOut.API.Common.Utils;
using HangOut.API.Services.Interface;
using HangOut.Domain.Entities;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Business;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;
using Microsoft.EntityFrameworkCore.Internal;

namespace HangOut.API.Services.Implement
{
    public class BusinessService : BaseService<BusinessService>, IBusinessService
    {
        private readonly IUploadService _uploadService;
        private readonly IRedisService _redisService;
        public BusinessService(IUnitOfWork<HangOutContext> unitOfWork, ILogger logger, 
            IHttpContextAccessor httpContextAccessor, IUploadService uploadService,
            IRedisService redisService) : base(unitOfWork, logger, httpContextAccessor)
        {
            _uploadService = uploadService;
            _redisService = redisService;
        }

        public async Task<ApiResponse<string>> CreateBusinessOwner(CreateBusinessOwnerRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var checkPhone = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Phone.Equals(request.Phone));
                if (checkPhone != null)
                {
                    return new ApiResponse<string>
                    {
                       Status = 208,
                       Message = "Phone already exist",
                       Data = request.Phone
                    };
                }

                var checkEmail = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Email.Equals(request.Email));
                if (checkEmail != null)
                {
                    return new ApiResponse<string>
                    {
                        Status = 208,
                        Message = "Email already exist",
                        Data = request.Email
                    };
                }

                var creaNewAccount = new Account
                {
                    Id = Guid.NewGuid(),
                    Phone = request.Phone,
                    Email = request.Email,
                    Active = true,
                    Password = PasswordUtil.HashPassword(request.Password),
                    Role  = Domain.Enums.ERoleEnum.BusinessOwner,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = null
                };

                await _unitOfWork.GetRepository<Account>().InsertAsync(creaNewAccount);

                var newUser = new User
                {
                    Name = request.Name,
                    Active = creaNewAccount.Active,
                    Avatar = await _uploadService.UploadImageAsync(request.AvatarImage),
                    AccountId = creaNewAccount.Id,
                    CreatedDate = creaNewAccount.CreatedDate,
                    LastModifiedDate = creaNewAccount.LastModifiedDate,
                    
                };

                await _unitOfWork.GetRepository<User>().InsertAsync(newUser);

                var createBusiness = new Business
                {
                    Id = Guid.NewGuid(),
                    Vibe = request.Vibe,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Address = request.Address,
                    Province = request.Province,
                    Name = request.Name,
                    Description = request.Description,
                    AccountId = creaNewAccount.Id,
                    Active = true,
                    MainImageUrl = request.MainImage != null
                        ? await _uploadService.UploadImageAsync(request.MainImage) 
                        : null,
                };
           

                await _unitOfWork.GetRepository<Business>().InsertAsync(createBusiness);

                if (request.Image != null)
                {
                    foreach(var image in request.Image)
                    {
                        var newImage = new Image
                        {
                            Id= Guid.NewGuid(),
                            Url = await _uploadService.UploadImageAsync(image),
                            CreatedDate= DateTime.Now,
                            ObjectId = createBusiness.Id,
                            ImageType = Domain.Enums.EImageType.Business_Image,
                            LastModifiedDate = null,
                            EntityType = Domain.Enums.EntityTypeEnum.Business
                        };

                        await _unitOfWork.GetRepository<Image>().InsertAsync(newImage);
                        Console.WriteLine($"Adding Image: ObjectId={newImage.ObjectId}, EntityType={newImage.EntityType}");
                    }

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
        public  async Task<ApiResponse<string>> RegisterBusinessOwner(RegisterBusinessRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var checkPhone = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Phone.Equals(request.Phone));
                if (checkPhone != null)
                {
                    return new ApiResponse<string>
                    {
                        Status = 208,
                        Message = "Phone already exist",
                        Data = request.Phone
                    };
                }

                var checkEmail = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(predicate: x => x.Email.Equals(request.Email));
                if (checkEmail != null)
                {
                    return new ApiResponse<string>
                    {
                        Status = 208,
                        Message = "Email already exist",
                        Data = request.Email
                    };
                }

                var key = "otp:" + request.Email;
                var existingOtp = await _redisService.GetStringAsync(key);

                if (string.IsNullOrEmpty(existingOtp))
                    throw new BadHttpRequestException("Không tìm thấy mã OTP");
                if (!existingOtp.Equals(request.Otp))
                    throw new BadHttpRequestException("Mã OTP không chính xác");

                var creaNewAccount = new Account
                {
                    Id = Guid.NewGuid(),
                    Phone = request.Phone,
                    Email = request.Email,
                    Active = false,
                    Password = PasswordUtil.HashPassword(request.Password),
                    Role = Domain.Enums.ERoleEnum.BusinessOwner,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = null
                  
                };

                await _unitOfWork.GetRepository<Account>().InsertAsync(creaNewAccount);

                var newUser = new User
                {
                    Name = request.Name,
                    Active = creaNewAccount.Active,
                    Avatar = await _uploadService.UploadImageAsync(request.AvatarImage),
                    AccountId = creaNewAccount.Id,
                    CreatedDate = creaNewAccount.CreatedDate,
                    LastModifiedDate = creaNewAccount.LastModifiedDate,
                    
                };

                await _unitOfWork.GetRepository<User>().InsertAsync(newUser);

                var createBusiness = new Business
                {
                    Id = Guid.NewGuid(),
                    Vibe = request.Vibe,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Address = request.Address,
                    Province = request.Province,
                    Name = request.Name,
                    Description = request.Description,
                    AccountId = creaNewAccount.Id,
                    Active = false,
                    MainImageUrl = request.MainImage != null
                        ? await _uploadService.UploadImageAsync(request.MainImage)
                        : null,
                };


                await _unitOfWork.GetRepository<Business>().InsertAsync(createBusiness);

                if (request.Image != null)
                {
                    foreach (var image in request.Image)
                    {
                        var newImage = new Image
                        {
                            Id = Guid.NewGuid(),
                            Url = await _uploadService.UploadImageAsync(image),
                            CreatedDate = DateTime.Now,
                            ObjectId = createBusiness.Id,
                            ImageType = Domain.Enums.EImageType.Business_Image,
                            LastModifiedDate = null,
                            EntityType = Domain.Enums.EntityTypeEnum.Business
                        };

                        await _unitOfWork.GetRepository<Image>().InsertAsync(newImage);
                        Console.WriteLine($"Adding Image: ObjectId={newImage.ObjectId}, EntityType={newImage.EntityType}");
                    }

                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new ApiResponse<string>
                {
                    Status = 200,
                    Message = "Create business success",
                    Data = null
                };


            }
            catch (Exception ex)
            {

                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.ToString());
            }

        }


    }
}
