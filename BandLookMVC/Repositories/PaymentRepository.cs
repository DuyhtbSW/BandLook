using BandLookMVC.Response;
using Dapper;

namespace BandLookMVC.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public PaymentRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ListPaymentResponse>> List()
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql =
                @"SELECT ph.id as id, ph.status as status, ph.pay_time as Pay_time,  a.fullname as fullname, ph.phone as Phone, ph.email as email, ph.type as type, b.price as price
FROM payment_history ph JOIN booking_artist b ON ph.booking_id = b.id JOIN artist a ON a.id = b.artist_id;";

            return await conn.QueryAsync<ListPaymentResponse>(sql);
        }
    }

    public async Task Confirm(int id, int status)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"UPDATE `payment_history`
                        SET
                        `status` = @status
                        WHERE `id` = @Id;";

            await conn.ExecuteAsync(sql, new { status, id });
        }
    }
}