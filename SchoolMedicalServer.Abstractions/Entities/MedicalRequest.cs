using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class MedicalRequest
{
    public Guid RequestItemId { get; set; }

    public Guid? EventId { get; set; }

    public Guid? ItemId { get; set; }

    public int? RequestedQuantity { get; set; }

    public string? Purpose { get; set; }

    public DateOnly? RequestDate { get; set; }

    public string? Notes { get; set; }

    public virtual MedicalEvent? Event { get; set; }

    public virtual MedicalInventory? Item { get; set; }
}
