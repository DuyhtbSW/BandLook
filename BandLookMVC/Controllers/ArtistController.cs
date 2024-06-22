using BandLookMVC.Repositories;
using BandLookMVC.Response;
using BrandLook.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class ArtistController : Controller
{
    private readonly IArtistRepository _artistRepository;

    public ArtistController(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    // GET
    public class ArtistDetailViewModel
    {
        public ArtistDetailResponse Artist { get; set; }
        public List<Schedule> Schedule { get; set; }
    }

    public async Task<IActionResult> Detail(int id)
    {
        var artist = await _artistRepository.Detail(id);
        var schedule = await _artistRepository.GetArtistSchedule(id);
        var viewModel = new ArtistDetailViewModel
        {
            Artist = artist,
            Schedule = schedule,
        };
        
        return View(viewModel);
    }
    
    public async Task<IActionResult> GetBooking(int id, string selectedDate)
    {
        var bookings = await _artistRepository.GetArtistBooking(id, selectedDate); 

        return Json(bookings);
    }
    public class ArtistListViewModel
    {
        public List<ListArtistResponse> Artists { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public async Task<IActionResult> List(string? fullname, string? job, string? address, string? sortBy, int page = 1, int pageSize = 4)
    {
        var result = await _artistRepository.List(fullname, job, address, sortBy, page, pageSize);
        var totalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);

        var viewModel = new ArtistListViewModel
        {
            Artists = result.Artists,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }


}