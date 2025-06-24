namespace SchoolMedicalServer.Abstractions.Dtos.Dashboard
{
    public class DashboardResponse
    {
        public Item Item { get; set; } = new();
    }

    public class Item
    {
        public string? Name { get; set; }
        public int Count { get; set; }
        public List<ItemDetails>? Details { get; set; } = [];
    }

    public class ItemDetails
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}
