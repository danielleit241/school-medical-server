namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class DashboardResponse
    {
        public Item Item { get; set; } = new();
    }

    public class Item
    {
        public string? Name { get; set; }
        public int Count { get; set; }
    }
}
