using BandLookMVC.Response;

namespace BandLookMVC.Repositories;

public interface IRequestRepository
{
    Task Add(int accountId, string reason);
    Task Confirm(int accountId);
    Task Reject(int accountId);
    Task<IEnumerable<RequestListResponse>> List();
}