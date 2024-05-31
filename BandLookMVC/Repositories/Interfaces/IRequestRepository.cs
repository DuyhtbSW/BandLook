namespace BandLookMVC.Repositories;

public interface IRequestRepository
{
    Task Add(int accountId, string reason);
}