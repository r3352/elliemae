// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompPlan
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalLoanCompPlan : IExternalLoanCompPlan
  {
    private LoanCompPlan loanCompPlan;

    internal ExternalLoanCompPlan(LoanCompPlan loanCompPlan) => this.loanCompPlan = loanCompPlan;

    public int Id => this.loanCompPlan.Id;

    public string Name => this.loanCompPlan.Name;

    public string Description => this.loanCompPlan.Description;

    public LoanCompPlanType PlanType => this.loanCompPlan.PlanType;

    public bool Status => this.loanCompPlan.Status;

    public DateTime ActivationDate => this.loanCompPlan.ActivationDate;

    public int MinTermDays => this.loanCompPlan.MinTermDays;

    public Decimal PercentAmt => this.loanCompPlan.PercentAmt;

    public int PercentAmtBase => this.loanCompPlan.PercentAmtBase;

    public int RoundingMethod => this.loanCompPlan.RoundingMethod;

    public Decimal DollarAmount => this.loanCompPlan.DollarAmount;

    public Decimal MinDollarAmount => this.loanCompPlan.MinDollarAmount;

    public Decimal MaxDollarAmount => this.loanCompPlan.MaxDollarAmount;

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
