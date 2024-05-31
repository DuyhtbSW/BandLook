using Dapper;

namespace BandLookMVC.Repositories;

public class RequestRepository : IRequestRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public RequestRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Add(int accountId, string reason)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"INSERT INTO `request`
                            (`account_id`,
                            `reason`,
                            `status`)
                            VALUES
                            (@accountId,
                            @reason,
                            0)
                            ";

            await conn.ExecuteAsync(sql, new { accountId, reason});
        }
    }
}