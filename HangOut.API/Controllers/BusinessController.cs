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
        [HttpPost("create-business-owner")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBusiness([FromForm]CreateBusinessOwnerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _businessService.CreateBusinessOwner(request);
                return StatusCode(response.Status, response);
            }
            catch (Exception ex)
            {
                _looger.Error("[Create Business API] " + ex.Message, ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPost("register-business-account")]
        public async Task<IActionResult> RegisterBusinessOwner([FromForm] RegisterBusinessRequest request)
        {
            try
            {
                var response = await _businessService.RegisterBusinessOwner(request);
                return StatusCode(response.Status, response);
            }
            catch (Exception ex)
            {
                _looger.Error("[Register Business Owner]" + ex.Message, ex.StackTrace);
                return StatusCode(500,ex.ToString());
            }
        }

        [HttpPatch("edit-business/{businessId}")]
        public async  Task<IActionResult>EditBusiness(Guid businessId, [FromForm]EditBusinessRequest request)
        {
            try
            {
                var response = await _businessService.EditBusiness(businessId, request);
                return StatusCode(response.Status, response);

            }catch(Exception ex)
            {
                _looger.Error("[Edit Business API] " + ex.Message,ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("get-business")]
        public async Task<IActionResult> GetBusiness([FromQuery]int pageNumber, [FromQuery]int pageSize, [FromQuery]string? category, [FromQuery]string? province)
        {
            Guid? accountId = UserUtil.GetAccountId(HttpContext);
            var response = await _businessService.GetAllBusinessResponse(accountId, pageNumber, pageSize, category,province);
            return StatusCode(response.Status, response);
        }

        [HttpDelete("delete-business/{businessId}")]
        [Authorize(Roles = "BusinessOwner")]
        public async Task<IActionResult> DeleteBusiness(Guid businessId)
        {
            try
            {
                var response = await _businessService.DeleteBusiness(businessId);
                return StatusCode(response.Status, response);
            }
            catch (Exception ex) { 
                
                _looger.Error("[Delete Business API] " + ex.Message ,ex.StackTrace);
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("get-business-detail")]
        public async Task<IActionResult> GetBusinessDetail([FromQuery] Guid businessId)
        {
            var response = await _businessService.GetBusinessDetail(businessId);
            return StatusCode(response.Status, response);
        }
    }
}
