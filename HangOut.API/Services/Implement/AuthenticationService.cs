using HangOut.API.Common.Utils;
using HangOut.API.Services.Interface;
using HangOut.Domain.Entities;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Authentication;
using HangOut.Domain.Payload.Response.Authentication;
using HangOut.Domain.Payload.Settings;
using HangOut.Domain.Persistence;
using HangOut.Repository.Interface;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using IAuthenticationService = HangOut.API.Services.Interface.IAuthenticationService;
using LoginRequest = HangOut.Domain.Payload.Request.Authentication.LoginRequest;

namespace HangOut.API.Services.Implement;

public class AuthenticationService : BaseService<AuthenticationService>, IAuthenticationService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailService _emailService;
    private readonly IRedisService _redisService;
    public AuthenticationService(IUnitOfWork<HangOutContext> unitOfWork, ILogger logger, IHttpContextAccessor httpContextAccessor,
        IOptions<JwtSettings> jwtSettings, 
        IWebHostEnvironment env, IEmailService emailService, IRedisService redisService) : base(unitOfWork, logger, httpContextAccessor)
    {
        _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        _env = env ?? throw new ArgumentNullException(nameof(env));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _redisService = redisService ?? throw new ArgumentNullException(nameof(redisService));
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

    public async Task<ApiResponse> SendOtpRequest(SendOtpRequest sendOtpRequest)
    {
        var key = "otp:" + sendOtpRequest.Email;
        
        var existingOtp = await _redisService.GetStringAsync(key);
        
        if (!string.IsNullOrEmpty(existingOtp))
            throw new BadHttpRequestException("Mã OTP đã được gửi");
        
        var accounts = await _unitOfWork.GetRepository<Account>().GetListAsync();
        if (accounts.Any(x => x.Email.Equals(sendOtpRequest.Email)))
            throw new BadHttpRequestException("Email đã được sử dụng");
        if (accounts.Any(x => x.Phone.Equals(sendOtpRequest.Phone)))
            throw new BadHttpRequestException("Số điện thoại đã được sử dụng");
        
        var otp = OtpUtil.GenerateOtp();
        
        var emailTemplate = GetTemplate(sendOtpRequest.Email, otp);
        var emailMessage = new EmailMessage()
        {
            ToAddress = sendOtpRequest.Email,
            Body = emailTemplate,
            Subject = "[HangOut] Xác thực email"
        };
        await _emailService.SendEmailAsync(emailMessage);
        
        var redisSuccess = await _redisService.SetStringAsync(key, otp, TimeSpan.FromMinutes(2));
        
        if (!redisSuccess)
            throw new BadHttpRequestException("Lỗi khi lưu mã OTP");
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Gửi mã OTP thành công",
            Data = null
        };
    }
    private string GetTemplate(string email, string otp)
    {
        var templatePath = Path.Combine(_env.WebRootPath, "assets/html", "template.html");
        if (!File.Exists(templatePath))
            throw new FileNotFoundException("Email template not found", templatePath);
        
        var template = File.ReadAllText(templatePath);
        template = template.Replace("{Email}", email);
        template = template.Replace("{otp}", otp);
        return template;
    }
}