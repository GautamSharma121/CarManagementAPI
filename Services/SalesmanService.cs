using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Models;
using CarModelManagementAPI.Repositories;

namespace CarModelManagementAPI.Services
{
    public class SalesmanService:ISalesmanService
    {
        private readonly ISalesmanRepo _salesmanRepository;

        public SalesmanService(ISalesmanRepo salesmanRepository)
        {
            _salesmanRepository = salesmanRepository;
        }

        public async Task<Result<SalesmanCommissionReport>> GetSalesmanCommissionReport(int salesmanId)
        {
            var result = new Result<SalesmanCommissionReport>();
            var salesman = await _salesmanRepository.GetSalesmanById(salesmanId);

            if (salesman == null)
            {
                throw new ArgumentException("Salesman not found");
            }

            var report = new SalesmanCommissionReport
            {
                SalesmanName = salesman.Name,
                BrandCommissions = new List<BrandCommission>(),
                TotalCommission = 0,
                AdditionalCommission = 0  // Track additional commission here
            };

            decimal totalCommission = 0;
            decimal additionalCommission = 0;

            // Iterate over each sale to calculate the commissions
            foreach (var sale in salesman.SalesRecords)
            {
                var brandCommission = await _salesmanRepository.GetBrandCommission(sale.CarBrand);
                var classCommission = await _salesmanRepository.GetClassCommission(sale.CarClass);

                // Base commission from the brand
                decimal commission = brandCommission.FixedCommission;

                // Additional commission based on the car class
                commission += sale.ModelPrice * classCommission.AdditionalPercentage;

                // If the salesman qualifies for the additional 2% bonus for A-Class cars
                if (sale.CarClass == "A-Class" && salesman.TotalSalesPreviousYear > 500000)
                {
                    commission += sale.ModelPrice * 0.02M;
                    additionalCommission += sale.ModelPrice * 0.02M; // Track additional commission here
                }

                totalCommission += commission;

                // Add/update the brand commission record in the report
                var brandCommissionRecord = report.BrandCommissions
                    .FirstOrDefault(b => b.Brand == sale.CarBrand);

                if (brandCommissionRecord == null)
                {
                    brandCommissionRecord = new BrandCommission
                    {
                        Brand = sale.CarBrand,
                        FixedCommission = brandCommission.FixedCommission
                    };
                    report.BrandCommissions.Add(brandCommissionRecord);
                }
            }

            // Set the final total and additional commission values in the report
            report.TotalCommission = totalCommission;
            report.AdditionalCommission = additionalCommission;

            // Set the result data and return it
            result.Data = report;
            return result;
        }


    }
}
