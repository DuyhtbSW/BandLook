using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class ArtistController : Controller
{
    // GET
    public IActionResult Detail()
    {
        return View();
    }
}