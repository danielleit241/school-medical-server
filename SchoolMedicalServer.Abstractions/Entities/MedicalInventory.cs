using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class MedicalInventory
{
    public Guid ItemId { get; set; }

    public string? ItemName { get; set; }

    public string? Category { get; set; }

    public string? Description { get; set; }

    public int? CurrentQuantity { get; set; }

    public string? UnitOfMeasure { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public virtual ICollection<MedicalRequest> MedicalRequests { get; set; } = new List<MedicalRequest>();
}
