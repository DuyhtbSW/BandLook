using BandLookMVC.Response;

namespace BandLookMVC.Repositories;

public interface IArtistRepository
{
    public Task<IEnumerable<ListArtistResponse>> ListTop(int top);
}