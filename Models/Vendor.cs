using System;
using System.Collections.Generic;

namespace Assetmanagementsystem.Models;

public partial class Vendor
{
    public int VdId { get; set; }

    public string VdName { get; set; } = null!;

    public string? VdAddr { get; set; }

    public DateOnly VdFromDate { get; set; }

    public DateOnly VdToDate { get; set; }

    public string? VdType { get; set; }

    public int? VdAtypeId { get; set; }

    public virtual ICollection<AssetMaster> AssetMasters { get; set; } = new List<AssetMaster>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual AssetType? VdAtype { get; set; }
}
