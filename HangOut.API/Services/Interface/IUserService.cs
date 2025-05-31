using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.User;
using HangOut.Domain.Payload.Response.User;

namespace HangOut.API.Services.Interface;

public interface IUserService
{
    Task<ApiResponse<GetUserProfileResponse>> GetUserProfileAsync(Guid? accountId);
    Task<ApiResponse> UpdateUserProfileAsync(Guid? accountId, UpdateProfileRequest request);
}