using CarModelManagementAPI.DataAccess;
using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.Models;
using Microsoft.AspNetCore.Connections;
using System.Data;
using System.Data.Common;

namespace CarModelManagementAPI.Repositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<CarModelRepository> _logger;
        public CarModelRepository(IDbConnectionFactory dbConnectionFactory, ILogger<CarModelRepository> logger)
        {
            _connectionFactory = dbConnectionFactory;
            _logger = logger;
        }
        public async Task<CarModel> CreateCarModel(CarModel carModel)
        {
            try
            {

                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    using (DbCommand cmd = await CreateModelCmd(con, carModel))
                    {
                        carModel.ID = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Error: {ex.Message}", ex);
                throw new Exception("An error occurred while creating the CarModel.", ex);
            }

            return carModel;
        }

        public async Task<int> GetCarModelById(int id)
        {
            try
            {
                int carModelId = 0;
                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await GetCarModelByIdCmd(con, id))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                carModelId = (int)DbField.GetInt(reader, "ID");
                            }
                        }
                    }
                }

                return carModelId; // Return the result
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AddCarImage(int carModelID, string imagePath)
        {
            bool successfullyUploaded = false;
            try
            {
                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open(); // Ensure asynchronous opening of the connection
                    }

                    using (DbCommand cmd = await CreateAddCarImageCommand(con, carModelID, imagePath))
                    {
                        int insertedRecord = await cmd.ExecuteNonQueryAsync(); // Ensure async execution
                        if (insertedRecord > 0)
                        {
                            successfullyUploaded = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"General Error: {ex.Message}", ex);
                throw new Exception("An error occurred while adding the car image.", ex);
            }

            return successfullyUploaded;
        }

        public async Task<List<CarModel>> SearchCarModels(string modelName, string modelCode)
        {
            try
            {
                var carModels = new List<CarModel>();

                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await SearchCarModelsCmd(con, modelName, modelCode))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var carModel = new CarModel
                                {
                                    ID = (int)reader["Id"],
                                    Brand = reader["Brand"].ToString(),
                                    Class = reader["Class"].ToString(),
                                    ModelName = reader["ModelName"].ToString(),
                                    ModelCode = reader["ModelCode"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Features = reader["Features"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    DateOfManufacturing = (DateTime)reader["DateOfManufacturing"],
                                    Active = (bool)reader["Active"],
                                    SortOrder = (int)reader["SortOrder"]
                                };
                                carModels.Add(carModel);
                            }
                        }
                    }
                }

                return carModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while searching car models: {ex.Message}");
                throw;
            }
        }


        public async  Task<List<CarModel>> GetCarModels(string orderBy)
        {
            try
            {
                var carModels = new List<CarModel>();

                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await GetCarModelsCmd(con, orderBy))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var carModel = new CarModel
                                {
                                    ID = (int)reader["Id"],
                                    Brand = reader["Brand"].ToString(),
                                    Class = reader["Class"].ToString(),
                                    ModelName = reader["ModelName"].ToString(),
                                    ModelCode = reader["ModelCode"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Features = reader["Features"].ToString(),
                                    Price = (decimal)reader["Price"],
                                    DateOfManufacturing = (DateTime)reader["DateOfManufacturing"],
                                    Active = (bool)reader["Active"],
                                    SortOrder = (int)reader["SortOrder"]
                                };
                                carModels.Add(carModel);
                            }
                        }
                    }
                }

                return carModels;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching car models: {ex.Message}");
                throw;
            }
        }



        #region Private Methods
        private async Task<DbCommand> CreateModelCmd(IDbConnection connection, CarModel carModel)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();

                command.CommandText = @"
        INSERT INTO CarModels 
        (Brand, Class, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing, Active, SortOrder)
        OUTPUT INSERTED.Id
        VALUES 
        (@Brand, @Class, @ModelName, @ModelCode, @Description, @Features, @Price, @DateOfManufacturing, @Active, @SortOrder)";

                // Add parameters with proper value assignments
                AddParameter(command, "@Brand", carModel.Brand, DbType.String);
                AddParameter(command, "@Class", carModel.Class, DbType.String);
                AddParameter(command, "@ModelName", carModel.ModelName, DbType.String);
                AddParameter(command, "@ModelCode", carModel.ModelCode, DbType.String);
                AddParameter(command, "@Description", carModel.Description, DbType.String);
                AddParameter(command, "@Features", carModel.Features, DbType.String);
                AddParameter(command, "@Price", carModel.Price, DbType.Decimal);
                AddParameter(command, "@DateOfManufacturing", carModel.DateOfManufacturing, DbType.DateTime);
                AddParameter(command, "@Active", carModel.Active, DbType.Boolean);
                AddParameter(command, "@SortOrder", carModel.SortOrder, DbType.Int32);

                return await Task.FromResult(command);
            }

            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }

        private async Task<DbCommand> GetCarModelByIdCmd(IDbConnection connection, int CarModelId)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();

                command.CommandText = @"SELECT  ID FROM CarModels WHERE ID=@carID";
                AddParameter(command, "@carID", CarModelId, DbType.Int32);

                return await Task.FromResult(command);
            }
            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }


        private async Task<DbCommand> CreateAddCarImageCommand(IDbConnection connection, int CarModelId, string imagePath)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();

                command.CommandText = @"INSERT INTO CarModelImages (CarModelId, ImagePath)";
                AddParameter(command, "@CarModelId", CarModelId, DbType.String);
                AddParameter(command, "@ImagePath", imagePath, DbType.String);
                return await Task.FromResult(command);
            }
            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }
        private async Task<DbCommand> SearchCarModelsCmd(IDbConnection connection, string modelName, string modelCode)
        {



            if (connection is DbConnection dbConn)
            {
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                                   SELECT * 
                                   FROM CarModels
                                   WHERE (@ModelName IS NULL OR ModelName LIKE '%' + @ModelName + '%')
                                   AND (@ModelCode IS NULL OR ModelCode = @ModelCode)";
                AddParameter(cmd, "@ModelName", modelName, DbType.String);
                AddParameter(cmd, "@ModelCode", modelCode, DbType.String);
                return await Task.FromResult(cmd);
            }

            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }

        private async Task<DbCommand> GetCarModelsCmd(IDbConnection connection, string orderBy)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();
                command.CommandText = @"SELECT  * FROM CarModels Order By @orderBy";
                AddParameter(command, "@orderBy", orderBy, DbType.String);
                return await Task.FromResult(command);
            }
            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }
        private void AddParameter(DbCommand command, string parameterName, object value, DbType dbType)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            parameter.DbType = dbType;
            command.Parameters.Add(parameter);
        }

        #endregion
    }
}
