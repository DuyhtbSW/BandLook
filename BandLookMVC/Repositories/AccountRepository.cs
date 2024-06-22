using BandLookMVC.Request;
using BandLookMVC.Response;
using BrandLook.Entities;
using Dapper;

namespace BandLookMVC.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public AccountRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Account> Login(LoginRequest request)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"SELECT * FROM account WHERE email = @email AND password = @password;";

            return await conn.QueryFirstAsync<Account>(sql, new { request.Email, request.Password });
        }
    }

    public async Task Register(RegisterRequest request)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"INSERT INTO `account`
                (`role_id`,
                `user_name`,
                `password`,
                `email`,
                `status`)
                VALUES
                (2, @username, @password, @email, 1);
                ";

            await conn.ExecuteAsync(sql, new { request.Username, request.Password, request.Email });
        }
    }

    public async Task Update(int id, int roleId, string username, string email)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"UPDATE `account`
SET
`role_id` = @roleId,
`user_name` = @username,
`email` = @email
WHERE `id` = @id;
                ";

            await conn.ExecuteAsync(sql, new { roleId, username, email, id });
        }
    }

    public async Task Deactivate(int id, int status)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"UPDATE `account`
SET
`status` = @status
WHERE `id` = @id; ";

            await conn.ExecuteAsync(sql, new { id, status });
        }
    }

    public async Task<Account> Detail(int id)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"
            SELECT * FROM Account
            WHERE id = @id";

            return await conn.QueryFirstAsync<Account>(sql, new { id = id });
        }
    }

    public async Task<IEnumerable<ListUserResponse>> List()
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"
            SELECT * FROM Account";

            return await conn.QueryAsync<ListUserResponse>(sql);
        }
    }
}