using BandLookMVC.Response;
using BrandLook.Entities;

namespace BandLookMVC.Repositories;

public interface IArtistRepository
{
    public Task<IEnumerable<ListArtistResponse>> ListTop(int top);
    public Task<ArtistDetailResponse> Detail(int id);
    public Task<List<ListArtistResponse>> List(string fullname, string job, string address, string sortBy);
    public Task<List<Schedule>> GetArtistSchedule(int artistId);
    public Task<List<Booking>> GetArtistBooking(int artistId, string startDate);
    public Task<List<Schedule>> GetArtistScheduleToUpdate(int artistId);
    public Task Update(int artistId, string description, List<string> images);
    Task<int> DeleteArtistSchedule(int artistId, string date, TimeSpan startTime);
    Task<int> AddArtistSchedule(int artistId, string date, string end, TimeSpan startTime, TimeSpan endTime);
    Task Add(int accountId, string fullname, string catxe, string job, string email, string phone, string address, string description, string image);
}