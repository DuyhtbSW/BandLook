using BandLookMVC.Response;
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
}