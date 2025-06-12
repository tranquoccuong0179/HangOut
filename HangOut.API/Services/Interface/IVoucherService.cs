using HangOut.Domain.Paginate;
using HangOut.Domain.Payload.Base;
using HangOut.Domain.Payload.Request.Voucher;
using HangOut.Domain.Payload.Response.Voucher;

namespace HangOut.API.Services.Interface
{
    public interface IVoucherService
    {
        Task<ApiResponse<string>> CreateVoucher(CreateVoucherRequest request);
        Task<ApiResponse<Paginate<GetVouchersResponse>>> GetVoucherByBusiness(Guid businessId, int pageNumber, int pageSize);
        Task<ApiResponse<GetVoucher>> GetVoucher(Guid voucherId);
        Task<ApiResponse<EditVoucherRequest>> EditVoucher(EditVoucherRequest request);
        Task<ApiResponse<string>> ReceiveVoucher(Guid accountId, Guid voucherId);
        Task<ApiResponse<Paginate<GetMyVoucherResponse>>> GetVoucherByAccount(Guid accountId, int pageNumber, int pageSize);
        Task<ApiResponse<string>> DeleteVoucher(Guid voucherId);
    }
}
