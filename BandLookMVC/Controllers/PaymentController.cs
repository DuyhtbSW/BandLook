using BandLookMVC.Repositories;
using BandLookMVC.Response;
using BandLookMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using BandLookMVC.Request;

namespace BandLookMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IVNPayPaymentService _vnPayPaymentService;

        public PaymentController(IArtistRepository artistRepository, IBookingRepository bookingRepository, IVNPayPaymentService vnPayPaymentService)
        {
            _artistRepository = artistRepository;
            _bookingRepository = bookingRepository;
            _vnPayPaymentService = vnPayPaymentService;
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

        [HttpPost]
        public async Task<IActionResult> VNPay(int artistId, string email, string phone, string name, string paymentMethod, string selectedDate, int totalPrice, string selectedTimeSlots)
        {
            var timeSlotStrings = selectedTimeSlots.Split(',');

            var timeSlotsList = new List<string>();
            foreach (var slot in timeSlotStrings)
            {
                var trimmedSlot = slot.Trim();
                if (IsValidTimeRange(trimmedSlot))
                {
                    timeSlotsList.Add(trimmedSlot);
                }
                else
                {
                    ModelState.AddModelError("selectedTimeSlots", $"Invalid time slot value: {slot}");
                    return View("Error");
                }
            }

            if (paymentMethod == "vnpay")
            {
                var orderId = "";
                foreach (var slot in timeSlotsList)
                {
                    var times = slot.Split('-');
                    var startTime = TimeSpan.Parse(times[0].Trim());
                    var endTime = TimeSpan.Parse(times[1].Trim());

                    orderId = await _bookingRepository.create(new BookingRequest
                    {
                        ArtistId = artistId,
                        Email = email,
                        Phone = phone,
                        Name = name,
                        Start_date = selectedDate,
                        End_date = selectedDate,
                        Start_time = startTime.ToString(),
                        End_time = endTime.ToString(),
                        Status = 0,
                        Description = "Booking description",
                        Price = totalPrice
                    });
                }
                var orderInfo = $"Thanh toan don hang #{orderId}";
                var payUrl = _vnPayPaymentService.CreatePayment(totalPrice.ToString(), orderId.ToString(), orderInfo, HttpContext);
                return Redirect(payUrl);
            }

            return RedirectToAction("Home", "Home");
        }

        private bool IsValidTimeRange(string timeRange)
        {
            var times = timeRange.Split('-');
            if (times.Length != 2)
                return false;

            return TimeSpan.TryParse(times[0].Trim(), out _) && TimeSpan.TryParse(times[1].Trim(), out _);
        }


        private string ConvertToTimeRange(int slot)
        {
            var startHour = slot;
            var endHour = slot + 1;
            return $"{startHour:D2}:00 - {endHour:D2}:00";
        }
    }
}
