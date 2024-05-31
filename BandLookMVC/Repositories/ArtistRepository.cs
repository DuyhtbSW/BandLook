using BandLookMVC.Response;
using BrandLook.Entities;
using Dapper;

namespace BandLookMVC.Repositories;

public class ArtistRepository : IArtistRepository
{
    public ArtistRepository(MySqlConnectionFactory connectionFactory, ILogger<ArtistRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    private readonly MySqlConnectionFactory _connectionFactory;
    private readonly ILogger<ArtistRepository> _logger;

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
    
    public async Task<List<Schedule>> GetArtistScheduleToUpdate(int artistId)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"SELECT id, artist_id, 
               start_date, 
               end_date, 
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
            var sql = @"SELECT start_date, end_date, start_time, end_time
                        FROM booking_artist
                        WHERE  start_date = @startDate AND artist_id = @artistId;";
            
            return (await conn.QueryAsync<Booking>(sql, new {startDate, artistId })).ToList();
        }
    }

    public async Task Update(int artistId, string description, List<string> images)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
    
                    var sql = @"UPDATE `artist`
                            SET `description` = @description
                            WHERE `id` = @artistId;";
                
                    await conn.ExecuteAsync(sql, new { artistId, description });

                    var deleteImagesSql = @"DELETE FROM `artist_image`
                                        WHERE `artist_id` = @artistId;";
                
                    await conn.ExecuteAsync(deleteImagesSql, new { artistId });

                    var insertImageSql = @"INSERT INTO `artist_image` (`artist_id`, `image`)
                                       VALUES (@artistId, @imageUrl);";
                
                    foreach (var imageUrl in images)
                    {
                        await conn.ExecuteAsync(insertImageSql, new { artistId, imageUrl });
                    }
        }
    }
    
    public async Task<int> DeleteArtistSchedule(int artistId, string date, TimeSpan startTime)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            try
            {
                var sql = @"DELETE FROM artist_carlender
                        WHERE artist_id = @ArtistId 
                        AND start_date = @Date 
                        AND start_time = @StartTime";

                var param = new { ArtistId = artistId, Date = date, StartTime = startTime };

                var a = await conn.ExecuteAsync(sql, param);
                return a;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while deleting artist schedule.");
                throw;
            }
        }
    }

    public async Task<int> AddArtistSchedule(int artistId, string date, string end, TimeSpan startTime, TimeSpan endTime)
    {
        var sql = @"INSERT INTO artist_carlender (artist_id, start_date, end_date, start_time, end_time)
                VALUES (@ArtistId, @StartDate, @EndDate, @StartTime, @EndTime)";

        try
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                var parameters = new
                {
                    ArtistId = artistId,
                    StartDate = date,
                    EndDate = end,
                    StartTime = startTime,
                    EndTime = endTime
                };

                var a = await conn.ExecuteAsync(sql, parameters);
                return a;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while adding artist schedule.");
            throw;
        }
    }

    public async Task Add(int accountId, string fullname, string catxe, string job, string email, string phone, string address, string description, string image)
    {
        var sql = @"INSERT INTO `bandlook`.`artist`
                            (`account_id`,
                            `fullname`,
                            `catxe`,
                            `job`,
                            `rating`,
                            `description`,
                            `address`,
                            `dob`,
                            `phone`)
                            VALUES
                            (@accountId,
                            @fullname,
                            @catxe,
                            @job,
                            5,
                            @description,
                            @address,
                            @dob,
                            @phone);
                            ";

        var sql1 = @"INSERT INTO `artist_image`
                        (`artist_id`,`image`)
                        VALUES(@artistId, @image);";
        try
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                var parameters = new
                {
                    accountId,
                    fullname,
                    catxe,
                    job,
                    description,
                    address,
                     dob = "07-12-1995",
                    phone,
                };

                await conn.ExecuteAsync(sql, parameters);

                var artistId = conn.QueryFirstAsync("SELECT id FROM bandlook.artist order by id desc limit 1;");

                await conn.ExecuteAsync(sql1, new { artistId.Result, image });
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while adding artist schedule.");
            throw;
        }
    }
}