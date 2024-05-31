using BandLookMVC.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class AdminController : Controller
{
    private readonly IRequestRepository _requestRepository;

    public AdminController(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
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
    
    public IActionResult Confirm(int id)
    {
        _requestRepository.Confirm(id);
        return RedirectToAction("Request"); // Use RedirectToAction instead of View to reload the request list.
    }

    public IActionResult Reject(int id)
    {
        _requestRepository.Reject(id);
        return RedirectToAction("Request"); // Use RedirectToAction instead of View to reload the request list.
    }

}