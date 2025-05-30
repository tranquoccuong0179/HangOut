using HangOut.Domain.Enums;

namespace HangOut.Domain.Payload.Response.Authentication;

public class LoginResponse
{
    public Guid AccountId { get; set; }
    public string Email { get; set; } = string.Empty;
    public ERoleEnum Role { get; set; }
    public string AccessToken { get; set; } = string.Empty;
}