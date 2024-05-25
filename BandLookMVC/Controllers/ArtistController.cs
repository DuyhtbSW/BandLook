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
        public List<Booking> Bookings { get; set; }
    }

    public async Task<IActionResult> Detail(int id, string? selectedDate)
    {
        var artist = await _artistRepository.Detail(id);
        var schedule = await _artistRepository.GetArtistSchedule(id);
        var bookings = await _artistRepository.GetArtistBooking(id, selectedDate); 
        var viewModel = new ArtistDetailViewModel
        {
            Artist = artist,
            Schedule = schedule,
            Bookings = bookings 
        };
        
        return View(viewModel);
    }


}