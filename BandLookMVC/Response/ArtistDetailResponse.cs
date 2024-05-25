using System.Globalization;

namespace BandLookMVC.Response;

public class ArtistDetailResponse
{
    public string Fullname { get; set; }
    public string Catxe { get; set; }
    public string Job { get; set; }
    public string Description { get; set; }
    public int  Rating { get; set; }
    public string Address { get; set; }
    public string Dob { get; set; }
    public int Phone { get; set; }
    public List<string> Images { get; set; }
}