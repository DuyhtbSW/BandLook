using BandLookMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class AdminController : Controller
{
    private readonly IRequestRepository _requestRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IAccountRepository _accountRepository;

    public AdminController(IRequestRepository requestRepository, IPaymentRepository paymentRepository, IAccountRepository accountRepository)
    {
        _requestRepository = requestRepository;
        _paymentRepository = paymentRepository;
        _accountRepository = accountRepository;
    }

    // GET
    public IActionResult Home()
    {
        return View();
    }
    
    public IActionResult Request()
    {
        var result = _requestRepository.List();
        
        return View(result.Result);
    }
    
    public IActionResult Payment()
    {
        var result = _paymentRepository.List();
        
        return View(result.Result);
    }
    
    public IActionResult User()
    {
        var result = _accountRepository.List();
        return View(result.Result);
    }
    
    public IActionResult Confirm(int id)
    {
        _requestRepository.Confirm(id);
        return RedirectToAction("Request"); // Use RedirectToAction instead of View to reload the request list.
    }
    [HttpPost]
    public IActionResult UpdateUser(int id, int roleId, string username, string email)
    {
        _accountRepository.Update(id, roleId, username, email);
        return RedirectToAction("User"); // Use RedirectToAction instead of View to reload the request list.
    }
    public IActionResult DeactivateUser(int id, int status)
    {
        _accountRepository.Deactivate(id, status);
        return RedirectToAction("User"); // Use RedirectToAction instead of View to reload the request list.
    }
    
    public IActionResult PaymentConfirm(int id, int status)
    {
        _paymentRepository.Confirm(id, status);
        return RedirectToAction("Payment"); // Use RedirectToAction instead of View to reload the request list.
    }

    public IActionResult Reject(int id)
    {
        _requestRepository.Reject(id);
        return RedirectToAction("Request"); // Use RedirectToAction instead of View to reload the request list.
    }

}