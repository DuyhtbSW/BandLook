using BandLookMVC.Request;
using BandLookMVC.Response;
using BrandLook.Entities;

namespace BandLookMVC.Repositories;

public interface IAccountRepository
{
    Task<Account> Login(LoginRequest request);
}