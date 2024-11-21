using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IRepository
{
    public interface ISalesmanRepo
    {
        public Task<Salesman> GetSalesmanById(int salesmanId);
        public Task<BrandCommission> GetBrandCommission(string brand);
        public Task<ClassCommission> GetClassCommission(string carClass);
    }
}
