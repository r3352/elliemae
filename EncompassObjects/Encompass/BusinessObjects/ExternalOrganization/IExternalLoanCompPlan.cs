// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalLoanCompPlan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Interface for IExternalLoanCompPlan class to support LoanCompPlan
  /// </summary>
  /// <exclude />
  public interface IExternalLoanCompPlan
  {
    int Id { get; }

    string Name { get; }

    string Description { get; }

    LoanCompPlanType PlanType { get; }

    bool Status { get; }

    DateTime ActivationDate { get; }

    int MinTermDays { get; }

    Decimal PercentAmt { get; }

    int PercentAmtBase { get; }

    int RoundingMethod { get; }

    Decimal DollarAmount { get; }

    Decimal MinDollarAmount { get; }

    Decimal MaxDollarAmount { get; }

    string TriggerField { get; }
  }
}
