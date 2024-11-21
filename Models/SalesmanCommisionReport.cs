namespace CarModelManagementAPI.Models
{
    public class SalesmanCommissionReport
    {
        public string SalesmanName { get; set; }
        public List<BrandCommission> BrandCommissions { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal AdditionalCommission { get; set; } // Additional commission (e.g., 2% for Class A)
    }

}
