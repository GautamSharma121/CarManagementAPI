namespace CarModelManagementAPI.Models
{
    public class Salesman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalSalesPreviousYear { get; set; }
        public List<SaleRecord> SalesRecords { get; set; }
    }

}
