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

            return await conn.QueryFirstAsync<Account>(sql, new {request.Email, request.Password});
        }
    }
}