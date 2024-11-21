using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IServices
{
    public interface ISalesmanService
    {
        public Task<Result<SalesmanCommissionReport>> GetSalesmanCommissionReport(int salesmanId);
    }
}
