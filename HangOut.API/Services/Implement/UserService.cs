using HangOut.API.Common.Exceptions;
using HangOut.API.Services.Interface;
using HangOut.Domain.Entities;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.User;
using HangOut.Domain.Payload.Response.User;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HangOut.API.Services.Implement;

public class UserService : BaseService<UserService>, IUserService
{
    private readonly IUploadService _uploadService;
    public UserService(IUnitOfWork<HangOutContext> unitOfWork, ILogger logger, 
        IHttpContextAccessor httpContextAccessor, IUploadService uploadService) : base(unitOfWork, logger, httpContextAccessor)
    {
        _uploadService = uploadService;
    }

    public async Task<ApiResponse<GetUserProfileResponse>> GetUserProfileAsync(Guid? accountId)
    {
        if (accountId == Guid.Empty)
            throw new BadHttpRequestException("Không tìm thấy thông tin người dùng");
        var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
            predicate: x => x.AccountId.Equals(accountId),
            include: x => x.Include(x => x.Account)
        );
        if (user == null) 
            throw new NotFoundException("Không tìm thấy thông tin người dùng");

        var response = new GetUserProfileResponse()
        {
            UserId = user.Id,
            Name = user.Name,
            Phone = user.Account.Phone,
            Email = user.Account.Email,
            Avatar = user.Avatar,
            CreatedDate = user.CreatedDate,
            LastModifiedDate = user.LastModifiedDate
        };
        return new ApiResponse<GetUserProfileResponse>()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin người dùng thành công",
            Data = response
        };
    }

    public async Task<ApiResponse> UpdateUserProfileAsync(Guid? accountId, UpdateProfileRequest request)
    {
        var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
            predicate: x => x.AccountId.Equals(accountId),
            include: x => x.Include(x => x.Account)
        );
        if (user == null)
            throw new NotFoundException("Không tìm thấy thông tin người dùng");
        user.Name = request.Name ?? user.Name;
        user.Account.Phone = request.Phone ?? user.Account.Phone;
        
        if(request.Image != null)
        {
            var imageUrl = await _uploadService.UploadImageAsync(request.Image);
            if (string.IsNullOrEmpty(imageUrl))
                throw new BadHttpRequestException("Không thể tải lên ảnh đại diện");
            if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar != imageUrl)
            {
                var isDeleteSuccess = await _uploadService.DeleteImageAsync(user.Avatar);
                if (!isDeleteSuccess)
                    throw new BadHttpRequestException("Không thể xóa ảnh đại diện cũ");
            }
            user.Avatar = imageUrl;
        }

        _unitOfWork.GetRepository<User>().UpdateAsync(user);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
            throw new BadHttpRequestException("Cập nhật thông tin người dùng thất bại");
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật thông tin người dùng thành công"
        };
    }
}