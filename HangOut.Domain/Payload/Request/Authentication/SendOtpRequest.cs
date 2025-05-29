using System.ComponentModel.DataAnnotations;

namespace HangOut.Domain.Payload.Request.Authentication;

public class SendOtpRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
    public string Email { get; set; } = null!;
    [Required]
    [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
    public string Phone { get; set; } = null!;
}