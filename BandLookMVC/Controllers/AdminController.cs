using BandLookMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class AdminController : Controller
{
    private readonly IRequestRepository _requestRepository;
    private readonly IPaymentRepository _paymentRepository;

    public AdminController(IRequestRepository requestRepository, IPaymentRepository paymentRepository)
    {
        _requestRepository = requestRepository;
        _paymentRepository = paymentRepository;
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
    
    public IActionResult Confirm(int id)
    {
        _requestRepository.Confirm(id);
        return RedirectToAction("Request"); // Use RedirectToAction instead of View to reload the request list.
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