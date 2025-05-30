using HangOut.API.Common.Utils;
using HangOut.API.Services.Interface;
using HangOut.Domain.Payload.Request.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HangOut.API.Controllers
{
    [ApiController]
    [Route("api/v1/business")]
    public class BusinessController(ILogger _looger, IBusinessService _businessService) : Controller
    {
        [HttpPost("create-business")]
        [Authorize(Roles = "BusinessOwner")]
        public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var accountId = UserUtil.GetAccountId(HttpContext);
                 var response = await _businessService.CreateBusiness(accountId!.Value,request);
                return StatusCode(response.Status, response);
            }
            catch (Exception ex)
            {
                _looger.Error("[Create Business API] " + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
