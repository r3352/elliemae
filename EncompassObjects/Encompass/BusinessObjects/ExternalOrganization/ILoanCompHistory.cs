// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ILoanCompHistory
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Interface for ILoanCompHistory class to support LoanCompHistory
  /// </summary>
  /// <exclude />
  public interface ILoanCompHistory
  {
    string Id { get; }

    string PlanName { get; }

    int CompPlanId { get; }

    DateTime StartDate { get; set; }

    DateTime EndDate { get; set; }

    int MinTermDays { get; set; }

    Decimal PercentAmt { get; set; }

    int PercentAmtBase { get; set; }

    int RoundingMethod { get; set; }

    Decimal DollarAmount { get; set; }

    Decimal MinDollarAmount { get; set; }

    Decimal MaxDollarAmount { get; set; }
  }
}
