using BandLookMVC.Request;
using BandLookMVC.Response;
using BrandLook.Entities;

namespace BandLookMVC.Repositories;

public interface IAccountRepository
{
    Task<Account> Login(LoginRequest request);
    Task Register(RegisterRequest request);
    Task Update(int id, int roleId, string username, string email);
    Task<Account> Detail(int id);
    Task<IEnumerable<ListUserResponse>> List();
}