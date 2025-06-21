namespace SchoolMedicalServer.Abstractions.Dtos.MedicalInventory
{
    public class MedicalInventoryDashboardResponse
    {
        public int Quantity { get; set; }
        public int DaysLeft { get; set; }
        public DateOnly? ExpiredDate { get; set; }
    }
}
