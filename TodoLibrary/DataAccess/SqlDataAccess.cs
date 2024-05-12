using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TodoLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
   private readonly IConfiguration _config;

   public SqlDataAccess(IConfiguration config)
   {
      _config = config;
   }

   // We will use it for Get, GetAll and POST/Create (becuase we need create new record and get back data)
   public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
   {
      string connectionString = _config.GetConnectionString(connectionStringName);

      using IDbConnection connection = new SqlConnection(connectionString);

      var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
         commandType: CommandType.StoredProcedure);

      return rows.ToList();
   }

   // We don't need anything back so we don't need to mark it as async and await for data back becuase
   // it will consume some resources
   // Task by itdelf is equivelant to void
   public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
   {
      string connectionString = _config.GetConnectionString(connectionStringName);

      using IDbConnection connection = new SqlConnection(connectionString);
      // Make async ExecuteAsync call and forget about it
      // The caller will wait on it and will stay marked as asynchronous
      // But we don't wait on it and we get back / return Task which is void that its
      // not marked as async becuase we are not awaiting something
      await connection.ExecuteAsync(storedProcedure, parameters,
         commandType: CommandType.StoredProcedure);

   }

}
