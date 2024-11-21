namespace CarModelManagementAPI.Models
{
    public class SaleRecord
    {
        public string CarBrand { get; set; }
        public string CarClass { get; set; }
        public decimal ModelPrice { get; set; }
        public DateTime SaleDate { get; set; }
    }

}
