namespace BandLookMVC.Request;

public class BookingRequest
{
    public int ArtistId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
    public string Start_date { get; set; }
    public string End_date { get; set; }
    public string Start_time { get; set; }
    public string End_time { get; set; }
    public int Status { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}