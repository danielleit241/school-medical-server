using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class MedicalRequest
{
    public Guid RequestId { get; set; }

    public Guid? MedicalEventId { get; set; }

    public Guid? ItemId { get; set; }

    public int? RequestQuantity { get; set; }

    public string? Purpose { get; set; }

    public DateOnly? RequestDate { get; set; }

    public virtual MedicalEvent? Event { get; set; }

    public virtual MedicalInventory? Item { get; set; }
}
