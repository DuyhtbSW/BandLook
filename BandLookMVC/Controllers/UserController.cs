using BandLookMVC.Repositories;
using BandLookMVC.Response;
using BrandLook.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BandLookMVC.Controllers;

public class UserController : Controller
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IRequestRepository _requestRepository;

    public UserController(IArtistRepository artistRepository, IAccountRepository accountRepository, IRequestRepository requestRepository)
    {
        _artistRepository = artistRepository;
        _accountRepository = accountRepository;
        _requestRepository = requestRepository;
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
    
    public async Task<IActionResult> Request()
    {
        var id = HttpContext.Session.GetInt32("Id");
        var user = await _accountRepository.Detail(id.Value);
        
        return View(user);
    }
    [HttpPost]
    public async Task<IActionResult> Request(int accountId, string fullname, string catxe, string job, string email, string phone, string address, string description, string image)
    {
        _requestRepository.Add(accountId, "");

        _artistRepository.Add(accountId, fullname, catxe, job, email, phone, address, description, image);

        return RedirectToAction("Home", "Home");
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
    
    public class ArtistDetailViewModel
    {
        public ArtistDetailResponse Artist { get; set; }
        public List<Schedule> Schedule { get; set; }
    }

    public async Task<IActionResult> Calendar()
    {
        var id = HttpContext.Session.GetInt32("Id").Value;
        var artist = await _artistRepository.Detail(id);
        var schedule = await _artistRepository.GetArtistSchedule(id);
        var viewModel = new ArtistDetailViewModel
        {
            Artist = artist,
            Schedule = schedule,
        };
        
        return View(viewModel);
    }
    
    public async Task<IActionResult> GetBooking(string selectedDate)
    {
        var id = HttpContext.Session.GetInt32("Id").Value;
        var bookings = await _artistRepository.GetArtistBooking(id, selectedDate); 

        return Json(bookings);
    }
    
    public async Task<IActionResult> UpdateCalendar(int artistId, string selectedDate, string selectedTimeSlots)
    {
        try
        {
            var currentSchedule = await _artistRepository.GetArtistScheduleToUpdate(artistId);

            DateTime date = DateTime.Parse(selectedDate);

            var currentDaySlots = currentSchedule
                .Where(s => s.Start_date.Date == date.Date)
                .ToList();

            foreach (var slot in currentDaySlots)
            {
                string startHour = slot.Start_time.Hours.ToString();
                if (!selectedTimeSlots.Contains(startHour))
                {
                    await _artistRepository.DeleteArtistSchedule(artistId, slot.Start_date.Date.ToString("yyyy-MM-dd"), slot.Start_time);
                }
            }
            
            List<string> timeSlots = JsonConvert.DeserializeObject<List<string>>(selectedTimeSlots);
            
            foreach (var hour in timeSlots)
            {
                if (!currentDaySlots.Any(s => s.Start_time.Hours.ToString() == hour))
                {
                    await _artistRepository.AddArtistSchedule(artistId, date.Date.ToString("yyyy-MM-dd"), date.Date.AddDays(1).ToString("yyyy-MM-dd"), new TimeSpan(int.Parse(hour) , 0, 0), new TimeSpan(int.Parse(hour) + 1, 0, 0));
                }
            }

            return RedirectToAction("Calendar");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "An error occurred while updating the schedule.";
            return RedirectToAction("Calendar");
        }
    }


}