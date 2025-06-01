using HangOut.Domain.Paginate;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Business;
using Supabase.Functions.Responses;

namespace HangOut.API.Services.Interface
{
    public interface IBusinessService
    {
        Task<ApiResponse<string>> CreateBusinessOwner(CreateBusinessOwnerRequest request);
        Task<ApiResponse<string>> RegisterBusinessOwner(RegisterBusinessRequest request);
        Task<ApiResponse<string>> EditBusiness(EditBusinessRequest request);
    }
}
