using BandLookMVC.Response;
using BrandLook.Entities;
using Dapper;

namespace BandLookMVC.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public ArtistRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ListArtistResponse>> ListTop(int top)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"WITH ArtistImages AS (
                            SELECT 
                                a.id AS Id, 
                                m.image, 
                                a.catxe, 
                                a.job, 
                                a.rating, 
                                a.description, 
                                a.address, 
                                a.dob, 
                                a.phone,
                                a.fullname,    
                                ROW_NUMBER() OVER (PARTITION BY a.id ORDER BY m.image) AS rn
                            FROM 
                                artist a 
                            JOIN 
                                artist_image m 
                            ON 
                                a.id = m.artist_id
                        )
                        SELECT 
                            Id, 
                            image, 
                            catxe, 
                            job, 
                            rating, 
                            description, 
                            address, 
                            fullname,
                            dob, 
                            phone
                        FROM 
                            ArtistImages
                        WHERE 
                            rn = 1
                        LIMIT @top;
                        ";

         return  await conn.QueryAsync<ListArtistResponse>(sql, new {top});
        }
    }
    
    public async Task<ArtistDetailResponse> Detail(int id)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"
            SELECT a.id, a.fullname, a.job, a.address, a.catxe, a.description, a.phone, a.rating, a.dob, am.image
            FROM artist a 
            JOIN artist_image am ON a.id = am.artist_id 
            WHERE a.id = @id";

            var artistDictionary = new Dictionary<int, ArtistDetailResponse>();

            var result = await conn.QueryAsync<ArtistDetailResponse, string, ArtistDetailResponse>(
                sql,
                (artist, image) =>
                {
                    if (!artistDictionary.TryGetValue(id, out var currentArtist))
                    {
                        currentArtist = artist;
                        currentArtist.Images = new List<string>();
                        artistDictionary.Add(id, currentArtist);
                    }

                    currentArtist.Images.Add(image);
                    return currentArtist;
                },
                new { id },
                splitOn: "image");

            return artistDictionary.Values.FirstOrDefault();
        }
    }
    
    public async Task<List<Schedule>> GetArtistSchedule(int artistId)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"SELECT id, artist_id, 
       DATE_ADD(start_date, INTERVAL 1 DAY) as start_date, 
       DATE_ADD(end_date, INTERVAL 1 DAY) as end_date, 
       start_time, end_time 
FROM artist_carlender 
WHERE artist_id = @artistId";
            
            return (await conn.QueryAsync<Schedule>(sql, new { artistId })).ToList();
        }
    }
    
    public async Task<List<Booking>> GetArtistBooking(int artistId, string startDate)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"SELECT DATE_ADD(start_date, INTERVAL 1 DAY) as start_date, 
       DATE_ADD(end_date, INTERVAL 1 DAY) as end_date
FROM booking_artist
WHERE start_date = @startDate AND artist_id = @artistId;";
            
            return (await conn.QueryAsync<Booking>(sql, new {startDate, artistId })).ToList();
        }
    }


}