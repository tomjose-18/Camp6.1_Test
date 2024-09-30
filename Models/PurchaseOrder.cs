using System;
using System.Collections.Generic;

namespace Assetmanagementsystem.Models;

public partial class PurchaseOrder
{
    public int PdId { get; set; }

    public string PdOrderNo { get; set; } = null!;

    public int? PdAdId { get; set; }

    public int? PdTypeId { get; set; }

    public int PdQty { get; set; }

    public int? PdVendorId { get; set; }

    public DateOnly PdDate { get; set; }

    public DateOnly PdDdate { get; set; }

    public string? PdStatus { get; set; }

    public virtual AssetDefinition? PdAd { get; set; }

    public virtual AssetType? PdType { get; set; }

    public virtual Vendor? PdVendor { get; set; }
}
