using CarModelManagementAPI.DataAccess;
using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.Models;
using System.Data;
using System.Data.Common;

namespace CarModelManagementAPI.Repositories
{
    public class MasterDataRepository:IMasterDataRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<MasterDataRepository> _logger;
        public MasterDataRepository(IDbConnectionFactory dbConnectionFactory, ILogger<MasterDataRepository> logger)
        {
            _connectionFactory = dbConnectionFactory;
            _logger = logger;
        }
        public async Task<List<MasterData>> GetBrands()
        {
            try
            {
                var brands = new List<MasterData>();

                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (var cmd = GetMasterDataCommand(con, "Brand"))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var brand = new MasterData
                                {
                                    Id = (int)reader["Id"],
                                    Type = reader["Type"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    IsActive = (bool)reader["IsActive"]
                                };
                                brands.Add(brand);
                            }
                        }
                    }
                }

                return brands; // Return the list of brands
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching brands: {ex.Message}");
                throw;
            }
        }

        public async Task<List<MasterData>> GetCarClasses()
        {
            try
            {
                var carClasses = new List<MasterData>();

                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (var cmd = GetMasterDataCommand(con, "Class"))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var carClass = new MasterData
                                {
                                    Id = (int)reader["Id"],
                                    Type = reader["Type"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    IsActive = (bool)reader["IsActive"]
                                };
                                carClasses.Add(carClass);
                            }
                        }
                    }
                }

                return carClasses; // Return the list of car classes
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching car classes: {ex.Message}");
                throw;
            }
        }

        #region private methods
        private DbCommand GetMasterDataCommand(IDbConnection connection, string type)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT Id, Type, Name, IsActive
        FROM MasterData
        WHERE Type = @Type AND IsActive = 1";

            var typeParam = command.CreateParameter();
            typeParam.ParameterName = "@Type";
            typeParam.Value = type;
            typeParam.DbType = DbType.String;

            command.Parameters.Add(typeParam);
            return (DbCommand)command;
        }

        #endregion


    }
}
