using BandLookMVC.Response;

namespace BandLookMVC.Repositories;

public interface IPaymentRepository
{
    Task<IEnumerable<ListPaymentResponse>> List();
    Task Confirm(int id, int status);
}