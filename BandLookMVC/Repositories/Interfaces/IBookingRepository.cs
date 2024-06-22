using BandLookMVC.Request;

namespace BandLookMVC.Repositories;

public interface IBookingRepository
{
    Task<string> create(BookingRequest request);
}