namespace BrandLook.Entities;

public class Schedule
{
    public int Id { get; set; }
    public int Artist_Id { get; set; }
    public DateTime Start_date { get; set; }
    public DateTime End_date { get; set; }
    public TimeSpan Start_time { get; set; }
    public TimeSpan End_time { get; set; }
}
