namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class MedicalInventoryDto
    {
        public Guid ItemId { get; set; }

        public string? ItemName { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public int QuantityInStock { get; set; }

        public string? UnitOfMeasure { get; set; }

        public int MinimumStockLevel { get; set; }

        public int MaximumStockLevel { get; set; }

        public DateTime? LastImportDate { get; set; }

        public DateTime? LastExportDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool Status { get; set; }
    }
}
