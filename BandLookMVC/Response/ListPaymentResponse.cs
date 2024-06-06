namespace BandLookMVC.Response;

public class ListPaymentResponse
{
    public int Id { get; set; }
    public int Status { get; set; }
    public string Fullname { get; set; }
    public string Pay_time { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public string Price { get; set; }
}