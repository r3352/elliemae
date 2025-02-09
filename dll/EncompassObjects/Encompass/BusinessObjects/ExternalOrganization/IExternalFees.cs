// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalFees
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public interface IExternalFees
  {
    int FeeManagementID { get; }

    int ExternalOrgID { get; }

    string FeeName { get; set; }

    string Description { get; set; }

    string Code { get; set; }

    ExternalOrganizationEntityType Channel { get; set; }

    DateTime StartDate { get; set; }

    DateTime EndDate { get; set; }

    int Condition { get; }

    string AdvancedCode { get; }

    string AdvancedCodeXml { get; }

    double FeePercent { get; set; }

    double FeeAmount { get; set; }

    int FeeBasedOn { get; set; }

    string CreatedBy { get; set; }

    DateTime DateCreated { get; set; }

    string UpdatedBy { get; set; }

    DateTime DateUpdated { get; set; }

    ExternalOriginatorFeeStatus Status { get; }
  }
}
