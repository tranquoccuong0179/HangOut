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
        [HttpPost("register-business-owner")]
        public async Task<IActionResult> CreateBusiness([FromForm]CreateBusinessOwnerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isAdmin =false;
                var role = UserUtil.GetRole(HttpContext);
              
                if (role != null)
                {
                    isAdmin = true;
                }

                 var response = await _businessService.CreateBusinessOwner(isAdmin,request);
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
