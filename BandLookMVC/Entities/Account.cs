namespace BrandLook.Entities;

public class Account
{
    public int Id { get; set; }
    public int Role_Id { get; set; }
    public string User_name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int Status { get; set; }
    
}