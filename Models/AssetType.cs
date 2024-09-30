using System;
using System.Collections.Generic;

namespace Assetmanagementsystem.Models;

public partial class AssetType
{
    public int AtId { get; set; }

    public string AtName { get; set; } = null!;

    public virtual ICollection<AssetDefinition> AssetDefinitions { get; set; } = new List<AssetDefinition>();

    public virtual ICollection<AssetMaster> AssetMasters { get; set; } = new List<AssetMaster>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}
