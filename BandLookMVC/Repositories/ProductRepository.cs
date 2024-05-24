using BrandLook.Entities;
using Dapper;

namespace BandLookMVC.Repositories;

public class ProductRepository :IProductRepository
{
    private readonly MySqlConnectionFactory _connectionFactory;

    public ProductRepository(MySqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task<IEnumerable<Product>> ListProduct(int top)
    {
        using (var conn = _connectionFactory.CreateConnection())
        {
            var sql = @"SELECT  * FROM product ORDER BY new_rate DESC LIMIT @top ";

            return await conn.QueryAsync<Product>(sql, new {top});
        }
    }
}