using BandLookMVC.Repositories;
using BandLookMVC.Response;
using Microsoft.AspNetCore.Mvc;

namespace BandLookMVC.Controllers;

public class PaymentController : Controller
{
    private readonly IArtistRepository _artistRepository;

    public PaymentController(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public class PaymentViewModel
    {
        public ArtistDetailResponse Artist { get; set; }
        public string SelectedDate { get; set; }
        public int TotalPrice { get; set; }
        public List<string> SelectedTimeSlots { get; set; }
    }

    // GET
    public async Task<IActionResult> Pay(int id, string selectedDate, int totalPrice, string selectedTimeSlots)
    {
        var artist = await _artistRepository.Detail(id);

        var timeSlotsList = selectedTimeSlots.Split(',')
            .Select(slot => ConvertToTimeRange(int.Parse(slot.Trim())))
            .ToList();

        var viewModel = new PaymentViewModel
        {
            Artist = artist,
            SelectedDate = selectedDate,
            TotalPrice = totalPrice,
            SelectedTimeSlots = timeSlotsList
        };

        return View(viewModel);
    }

    private string ConvertToTimeRange(int slot)
    {
        var startHour = slot;
        var endHour = slot + 1;
        return $"{startHour:D2}:00 - {endHour:D2}:00";
    }

}