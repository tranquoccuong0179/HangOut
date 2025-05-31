using HangOut.API.Common.Utils;
using HangOut.API.Services.Interface;
using HangOut.Domain.Constants;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.User;
using HangOut.Domain.Payload.Response.User;
using Microsoft.AspNetCore.Mvc;

namespace HangOut.API.Controllers;

[ApiController]
[Route(ApiEndPointConstant.User.UserEndpoint)]
public class UserController : BaseController<UserController>
{
    private readonly IUserService _userService;
    public UserController(ILogger logger, IUserService userService) : base(logger)
    {
        _userService = userService;
    }
    [HttpGet(ApiEndPointConstant.User.Profile)]
    [ProducesResponseType(typeof(ApiResponse<GetUserProfileResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfileAsync()
    {
        var accountId = UserUtil.GetAccountId(HttpContext);
        
        var response = await _userService.GetUserProfileAsync(accountId);
        return Ok(response);
    }
    [HttpPatch(ApiEndPointConstant.User.Profile)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserProfileAsync([FromForm] UpdateProfileRequest request)
    {
        var accountId = UserUtil.GetAccountId(HttpContext);
        if (accountId == null)
        {
            return BadRequest("Không tìm thấy thông tin người dùng");
        }
        
        var response = await _userService.UpdateUserProfileAsync(accountId, request);
        return Ok(response);
    }
}