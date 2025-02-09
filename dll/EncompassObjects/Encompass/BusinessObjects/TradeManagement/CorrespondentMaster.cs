// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMaster
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class CorrespondentMaster
  {
    public MasterCommitmentType CommitmentType;

    public string MasterCommitmentNumber { get; set; }

    public string CompanyName { get; set; }

    public string TPOId { get; set; }

    public string OrganizationId { get; set; }

    public Decimal commitmentAmount { get; set; }

    public DateTime EffectiveDate { get; set; }

    public DateTime expireDate { get; set; }

    public Decimal AvailableAmount { get; set; }

    public string PriceGroup { get; set; }

    public MasterCommitmentStatus Status { get; set; }

    public List<CorrespondentMasterDelivery> DeliveryTypes { get; set; }
  }
}
