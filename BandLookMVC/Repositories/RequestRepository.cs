using BandLookMVC.Response;
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

    public async Task Confirm(int accountId)
    {
        try
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                var sql = @"UPDATE request
                            SET
                            `status` = 1
                            WHERE `account_id` = @accountId;";

                var sql1 = @"UPDATE `account`
                            SET
                            `role_id` = 1
                            WHERE `id` = @accountId;
";

                await conn.ExecuteAsync(sql, new { accountId});
                await conn.ExecuteAsync(sql1, new { accountId});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task Reject(int accountId)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"UPDATE request
                            SET
                            `status` = -1
                            WHERE `account_id` = @accountId;";

            await conn.ExecuteAsync(sql, new { accountId});
        }
    }

    public async Task<IEnumerable<RequestListResponse>> List()
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"WITH ArtistImages AS (
                        SELECT 
                            r.status as status,
                            a.address as address, a.catxe as catxe, a.job as job, a.dob as dob, a.fullname as fullname, a.phone as phone,
                            am.image as image, a.account_id as id,
                            ROW_NUMBER() OVER (PARTITION BY a.id ORDER BY am.image) AS rn
                        FROM 
                            request r 
                            JOIN artist a ON r.account_id = a.account_id
                            JOIN artist_image am ON am.artist_id = a.id
                    )
                    SELECT * 
                    FROM ArtistImages
                    WHERE rn = 1;
                    ";

         return   await conn.QueryAsync<RequestListResponse>(sql);
        }
    }
}