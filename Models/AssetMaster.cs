using System;
using System.Collections.Generic;

namespace Assetmanagementsystem.Models;

public partial class AssetMaster
{
    public int AmId { get; set; }

    public int? AmAtypeid { get; set; }

    public int? AmMakeid { get; set; }

    public int? AmAdId { get; set; }

    public string? AmModel { get; set; }

    public string? AmSnnumber { get; set; }

    public string? AmMyyear { get; set; }

    public DateOnly AmPdate { get; set; }

    public string? AmWarranty { get; set; }

    public DateOnly? AmFromDate { get; set; }

    public DateOnly? AmToDate { get; set; }

    public virtual AssetDefinition? AmAd { get; set; }

    public virtual AssetType? AmAtype { get; set; }

    public virtual Vendor? AmMake { get; set; }
}
