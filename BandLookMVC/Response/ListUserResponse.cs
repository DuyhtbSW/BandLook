namespace BandLookMVC.Response;

public class ListUserResponse
{
    public int Id { get; set; }
    public string user_name { get; set; }
    public int role_id { get; set; }
    public string email { get; set; }
    public int status { get; set; }
}