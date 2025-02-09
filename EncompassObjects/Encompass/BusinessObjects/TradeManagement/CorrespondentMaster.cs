// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMaster
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>CorrespondentMaster Class</summary>
  public class CorrespondentMaster
  {
    /// <summary>Gets the commitment type.</summary>
    public MasterCommitmentType CommitmentType;

    /// <summary>Gets the Master Commitment Number.</summary>
    public string MasterCommitmentNumber { get; set; }

    /// <summary>Gets the TPO Company Name.</summary>
    public string CompanyName { get; set; }

    /// <summary>Gets the TPO ID.</summary>
    public string TPOId { get; set; }

    /// <summary>Gets the TPO Organization ID.</summary>
    public string OrganizationId { get; set; }

    /// <summary>Gets the commitment amount.</summary>
    public Decimal commitmentAmount { get; set; }

    /// <summary>Gets the correspondent master effective date.</summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>Gets the correspondent master expire date.</summary>
    public DateTime expireDate { get; set; }

    /// <summary>Gets the correspondent master available amount.</summary>
    public Decimal AvailableAmount { get; set; }

    /// <summary>Gets the price group.</summary>
    public string PriceGroup { get; set; }

    /// <summary>Gets the status of corresposndent master.</summary>
    public MasterCommitmentStatus Status { get; set; }

    /// <summary>
    /// Gets the list of corresposndent master Delivery Types.
    /// </summary>
    public List<CorrespondentMasterDelivery> DeliveryTypes { get; set; }
  }
}
