using BandLookMVC.Request;
using BrandLook.Entities;

namespace BandLookMVC.Repositories;

public interface IAccountRepository
{
    Task<Account> Login(LoginRequest request);
    Task Register(RegisterRequest request);
    Task<Account> Detail(int id);
}