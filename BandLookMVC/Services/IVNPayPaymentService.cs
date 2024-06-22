using BandLookMVC.Response;

namespace BandLookMVC.Services;

public interface IVNPayPaymentService
{
    string CreatePayment(string amount, string orderId, string orderInfo, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}