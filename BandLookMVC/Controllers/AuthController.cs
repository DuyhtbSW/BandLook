using BandLookMVC.Repositories;
using BandLookMVC.Request;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class AuthController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AuthController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // GET
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Login(LoginRequest request)
    {
        if (ModelState.IsValid)
        {
            var user = _accountRepository.Login(request);

            var tt = user.Result;
            
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Result.User_name);

                return RedirectToAction("Home", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(request);
    }
    
    
}