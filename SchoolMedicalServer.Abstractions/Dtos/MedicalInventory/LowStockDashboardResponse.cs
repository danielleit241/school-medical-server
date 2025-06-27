namespace SchoolMedicalServer.Abstractions.Dtos.MedicalInventory
{
    public class LowStockDashboardResponse
    {
        public int QuantityInStock { get; set; }
        public int MinimumStockLevel { get; set; }
    }
}
