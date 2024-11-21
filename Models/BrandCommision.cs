namespace CarModelManagementAPI.Models
{
    public class BrandCommission
    {
        public string Brand { get; set; }
        public decimal FixedCommission { get; set; }
    }
    public class ClassCommission
    {
        public string CarClass { get; set; }
        public decimal AdditionalPercentage { get; set; }
    }



}
