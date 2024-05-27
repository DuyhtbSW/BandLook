using BandLookMVC.Response;
using BrandLook.Entities;

namespace BandLookMVC.Repositories;

public interface IArtistRepository
{
    public Task<IEnumerable<ListArtistResponse>> ListTop(int top);
    public Task<ArtistDetailResponse> Detail(int id);
    public Task<List<Schedule>> GetArtistSchedule(int artistId);
    public Task<List<Booking>> GetArtistBooking(int artistId, string startDate);
    public Task Update(int artistId, string description, List<string> images);



}