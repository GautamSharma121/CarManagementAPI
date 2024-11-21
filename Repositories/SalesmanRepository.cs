using CarModelManagementAPI.DataAccess;
using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.Models;
using System.Data;
using System.Data.Common;

namespace CarModelManagementAPI.Repositories
{
    public class SalesmanRepository : ISalesmanRepo
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<SalesmanRepository> _logger;
        public SalesmanRepository(IDbConnectionFactory dbConnectionFactory, ILogger<SalesmanRepository> logger)
        {
            _connectionFactory = dbConnectionFactory;
            _logger = logger;
        }
        public async Task<Salesman> GetSalesmanById(int salesmanId)
        {
            Salesman salesman = null;
            try
            {
                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await CreateGetSalesmanCmd(con, salesmanId))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                salesman = new Salesman
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString(),
                                    TotalSalesPreviousYear = Convert.ToDecimal(reader["TotalSalesPreviousYear"]),
                                    SalesRecords = await GetSalesRecords(salesmanId)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching salesman by ID: {ex.Message}", ex);
                throw new Exception("An error occurred while retrieving the salesman data.", ex);
            }

            return salesman;
        }       

        public async Task<List<SaleRecord>> GetSalesRecords(int salesmanId)
        {
            var salesRecords = new List<SaleRecord>();
            try
            {
                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await CreateGetSalesRecordsCmd(con, salesmanId))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                salesRecords.Add(new SaleRecord
                                {
                                    CarBrand = reader["CarBrand"].ToString(),
                                    CarClass = reader["CarClass"].ToString(),
                                    ModelPrice = Convert.ToDecimal(reader["ModelPrice"]),
                                    SaleDate = Convert.ToDateTime(reader["SaleDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching sales records: {ex.Message}", ex);
                throw new Exception("An error occurred while retrieving sales records.", ex);
            }

            return salesRecords;
        }

        public async Task<BrandCommission> GetBrandCommission(string brand)
        {
            BrandCommission brandCommission = null;
            try
            {
                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await CreateGetBrandCommissionCmd(con, brand))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                brandCommission = new BrandCommission
                                {
                                    Brand = reader["Brand"].ToString(),
                                    FixedCommission = Convert.ToDecimal(reader["FixedCommission"]),
                                   // AdditionalClassCommission = Convert.ToDecimal(reader["AdditionalClassCommission"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching brand commission: {ex.Message}", ex);
                throw new Exception("An error occurred while retrieving the brand commission.", ex);
            }

            return brandCommission;
        }
        public async Task<ClassCommission> GetClassCommission(string carClass)
        {
            ClassCommission classCommission = null;
            try
            {
                using (var con = _connectionFactory.CreateConnection())
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (DbCommand cmd = await CreateGetClassCommissionCmd(con, carClass))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                classCommission = new ClassCommission
                                {
                                    CarClass = reader["CarClass"].ToString(),
                                    AdditionalPercentage = Convert.ToDecimal(reader["AdditionalPercentage"])
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching class commission: {ex.Message}", ex);
                throw new Exception("An error occurred while retrieving the class commission.", ex);
            }

            return classCommission;
        }

        public async Task<SalesmanCommissionReport> GetSalesmanCommissionReport(int salesmanId)
        {
            SalesmanCommissionReport report = new SalesmanCommissionReport();
            try
            {
                var salesman = await GetSalesmanById(salesmanId);
                report.SalesmanName = salesman.Name;

                decimal totalCommission = 0;

                foreach (var record in salesman.SalesRecords)
                {
                    var brandCommission = await GetBrandCommission(record.CarBrand);
                    var classCommission = await GetClassCommission(record.CarClass);

                    decimal baseCommission = brandCommission.FixedCommission;
                    decimal classCommissionAmount = record.ModelPrice * (classCommission.AdditionalPercentage / 100);
                    totalCommission += baseCommission + classCommissionAmount;
                }

                if (salesman.TotalSalesPreviousYear > 500000)
                {
                    // Apply an additional 2% commission on Class A cars if the previous year's sales exceeded $500,000
                    var classARecords = salesman.SalesRecords.FindAll(r => r.CarClass == "A-Class");
                    foreach (var record in classARecords)
                    {
                        totalCommission += record.ModelPrice * 0.02m; // Additional 2% commission for Class A cars
                    }
                }

                report.TotalCommission = totalCommission;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calculating salesman commission: {ex.Message}", ex);
                throw new Exception("An error occurred while calculating the salesman commission report.", ex);
            }

            return report;
        }


        #region Private Methods

        private async Task<DbCommand> CreateGetSalesRecordsCmd(IDbConnection connection, int salesmanId)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM SalesRecords WHERE SalesmanId = @SalesmanId";
                AddParameter(command, "@SalesmanId", salesmanId, DbType.Int32);
                return await Task.FromResult(command);
            }

            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }
        private async Task<DbCommand> CreateGetSalesmanCmd(IDbConnection connection, int salesmanId)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Salesmen WHERE Id = @SalesmanId";
                AddParameter(command, "@SalesmanId", salesmanId, DbType.Int32);
                return await Task.FromResult(command);
            }

            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }

        private async Task<DbCommand> CreateGetBrandCommissionCmd(IDbConnection connection, string brand)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM BrandCommissions WHERE Brand = @Brand";
                AddParameter(command, "@Brand", brand, DbType.String);
                return await Task.FromResult(command);
            }

            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }

        private async Task<DbCommand> CreateGetClassCommissionCmd(IDbConnection connection, string carClass)
        {
            if (connection is DbConnection dbConnection)
            {
                var command = dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM ClassCommissions WHERE CarClass = @CarClass";
                AddParameter(command, "@CarClass", carClass, DbType.String);
                return await Task.FromResult(command);
            }

            throw new InvalidOperationException("The connection must be of type DbConnection.");
        }
        private void AddParameter(DbCommand command, string paramName, object value, DbType dbType)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = paramName;
            parameter.Value = value ?? DBNull.Value;
            parameter.DbType = dbType;
            command.Parameters.Add(parameter);
        }

        #endregion
    }
}
