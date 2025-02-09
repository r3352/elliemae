// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompPlan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents an external LO Comp Plan</summary>
  public class ExternalLoanCompPlan : IExternalLoanCompPlan
  {
    private LoanCompPlan loanCompPlan;

    internal ExternalLoanCompPlan(LoanCompPlan loanCompPlan) => this.loanCompPlan = loanCompPlan;

    /// <summary>Gets id of the LO Comp Plan</summary>
    public int Id => this.loanCompPlan.Id;

    /// <summary>Gets the name of lo comp plan</summary>
    public string Name => this.loanCompPlan.Name;

    /// <summary>Gets the Description of lo comp plan</summary>
    public string Description => this.loanCompPlan.Description;

    /// <summary>Gets PlanType of lo comp plan</summary>
    public LoanCompPlanType PlanType => this.loanCompPlan.PlanType;

    /// <summary>Gets Status flag</summary>
    public bool Status => this.loanCompPlan.Status;

    /// <summary>Gets activation date of lo comp plan</summary>
    public DateTime ActivationDate => this.loanCompPlan.ActivationDate;

    /// <summary>Gets minimum term days of lo comp plan</summary>
    public int MinTermDays => this.loanCompPlan.MinTermDays;

    /// <summary>Gets percent amount</summary>
    public Decimal PercentAmt => this.loanCompPlan.PercentAmt;

    /// <summary>Gets percent amount base</summary>
    public int PercentAmtBase => this.loanCompPlan.PercentAmtBase;

    /// <summary>Gets rounding method</summary>
    public int RoundingMethod => this.loanCompPlan.RoundingMethod;

    /// <summary>Gets dollar amount</summary>
    public Decimal DollarAmount => this.loanCompPlan.DollarAmount;

    /// <summary>Gets minimum dollar amount</summary>
    public Decimal MinDollarAmount => this.loanCompPlan.MinDollarAmount;

    /// <summary>Gets maximum dollar amount</summary>
    public Decimal MaxDollarAmount => this.loanCompPlan.MaxDollarAmount;

    /// <summary>Gets trigger field</summary>
    public string TriggerField => this.loanCompPlan.TriggerField;

    internal static ExternalLoanCompPlanList ToList(List<LoanCompPlan> plans)
    {
      ExternalLoanCompPlanList list = new ExternalLoanCompPlanList();
      for (int index = 0; index < plans.Count; ++index)
        list.Add(new ExternalLoanCompPlan(plans[index]));
      return list;
    }
  }
}
