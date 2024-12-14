using MotelRoomOnline.Models;

namespace MotelRoomOnline.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        string CreatePaymentUrl();
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
