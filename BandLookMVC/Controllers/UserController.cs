using BandLookMVC.Repositories;
using BandLookMVC.Response;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class UserController : Controller
{
    private readonly IArtistRepository _artistRepository;

    public UserController(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }
    
    public class ProfileViewModel
    {
        public ArtistDetailResponse Artist { get; set; }
        public string Email { get; set; }
    }
    // GET
    public async Task<IActionResult> Profile()
    {
        var id = HttpContext.Session.GetInt32("Id");
        var email = HttpContext.Session.GetString("Email");
        var artist = await _artistRepository.Detail(id.Value);
        
        var viewModel = new ProfileViewModel
        {
            Artist = artist,
            Email = email,
        };

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Update(int artistId, string description, List<string> images)
    {
        images = images.Where(img => !string.IsNullOrWhiteSpace(img)).ToList();
    
        try
        {
            await _artistRepository.Update(artistId, description, images);
            return RedirectToAction("Home", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Có lỗi xảy ra. Vui lòng kiểm tra lại!";
        
            var id = HttpContext.Session.GetInt32("Id");
            var email = HttpContext.Session.GetString("Email");
            var artist = await _artistRepository.Detail(id.Value);
        
            var viewModel = new ProfileViewModel
            {
                Artist = artist,
                Email = email,
            };

            return View("Profile", viewModel);
        }
    }

}