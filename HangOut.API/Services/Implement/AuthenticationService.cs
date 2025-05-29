using HangOut.API.Common.Utils;
using HangOut.Domain.Entities;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Response.Authentication;
using HangOut.Domain.Payload.Settings;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;
using Microsoft.Extensions.Options;
using IAuthenticationService = HangOut.API.Services.Interface.IAuthenticationService;
using LoginRequest = HangOut.Domain.Payload.Request.Authentication.LoginRequest;

namespace HangOut.API.Services.Implement;

public class AuthenticationService : BaseService<AuthenticationService>, IAuthenticationService
{
    private readonly JwtSettings _jwtSettings;
    public AuthenticationService(IUnitOfWork<HangOutContext> unitOfWork, ILogger logger, IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtSettings) : base(unitOfWork, logger, httpContextAccessor)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => (x.Email.Equals(loginRequest.EmailOrPhoneNumber) 
                             || x.Phone.Equals(loginRequest.EmailOrPhoneNumber))
        );
        if(account == null)
            throw new BadHttpRequestException("Tài khoản không tồn tại");
        if(account.Password != PasswordUtil.HashPassword(loginRequest.Password))
            throw new BadHttpRequestException("Tài khoản hoặc mật khẩu không đúng");

        var loginResponse = new LoginResponse()
        {
            Email = account.Email,
            AccountId = account.Id,
            AccessToken = JwtUtil.GenerateJwtToken(account, _jwtSettings),
            Role = account.Role
        };
        return new ApiResponse<LoginResponse>()
        {
            Status = StatusCodes.Status200OK,
            Message = "Đăng nhập thành công",
            Data = loginResponse,
        };

    }
}