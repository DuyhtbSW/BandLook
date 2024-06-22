using BandLookMVC.Request;
using Dapper;

namespace BandLookMVC.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public BookingRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<string> create(BookingRequest request)
    {
        var orderId = Guid.NewGuid().ToString();
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"INSERT INTO `bandlook`.`booking_artist`
                                            (`id`,`artist_id`,
                                            `start_date`,
                                            `end_date`,
                                            `start_time`,
                                            `end_time`,
                                            `status`,
                                            `description`,
                                            `price`, `email`, `phone`, `name`)
                                            VALUES
                                            (?,
                                            ?,
                                            ?,
                                            ?,
                                            ?,
                                            ?,
                                            ?,
                                            ?,
                                            ?, ?, ?, ?);
";
            

            await conn.ExecuteAsync(sql, new
            {orderId,
                request.ArtistId, request.Start_date
                , request.End_date, request.Start_time, request.End_time
                , request.Status, request.Description, request.Price,
                request.Email, request.Phone, request.Name
            });
        }

        return orderId;
    }
}