using System.Data;

namespace CarModelManagementAPI.DataAccess
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
