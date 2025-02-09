// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory
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
  public class ExternalLoanCompHistory : ILoanCompHistory
  {
    private LoanCompHistory loanCompHistory;

    internal ExternalLoanCompHistory(LoanCompHistory loanCompHistory)
    {
      this.loanCompHistory = loanCompHistory;
    }

    public string Id => this.loanCompHistory.Id;

    public string PlanName => this.loanCompHistory.PlanName;

    public int CompPlanId => this.loanCompHistory.CompPlanId;

    public DateTime StartDate
    {
      get => this.loanCompHistory.StartDate;
      set => this.loanCompHistory.StartDate = value;
    }

    public DateTime EndDate
    {
      get => this.loanCompHistory.EndDate;
      set => this.loanCompHistory.EndDate = value;
    }

    public int MinTermDays
    {
      get => this.loanCompHistory.MinTermDays;
      set => this.loanCompHistory.MinTermDays = value;
    }

    public Decimal PercentAmt
    {
      get => this.loanCompHistory.PercentAmt;
      set => this.loanCompHistory.PercentAmt = value;
    }

    public int PercentAmtBase
    {
      get => this.loanCompHistory.PercentAmtBase;
      set => this.loanCompHistory.PercentAmtBase = value;
    }

    public int RoundingMethod
    {
      get => this.loanCompHistory.RoundingMethod;
      set => this.loanCompHistory.RoundingMethod = value;
    }

    public Decimal DollarAmount
    {
      get => this.loanCompHistory.DollarAmount;
      set => this.loanCompHistory.DollarAmount = value;
    }

    public Decimal MinDollarAmount
    {
      get => this.loanCompHistory.MinDollarAmount;
      set => this.loanCompHistory.MinDollarAmount = value;
    }

    public Decimal MaxDollarAmount
    {
      get => this.loanCompHistory.MaxDollarAmount;
      set => this.loanCompHistory.MaxDollarAmount = value;
    }

    internal static ExternalLoanCompHistoryList ToList(LoanCompHistoryList comp)
    {
      ExternalLoanCompHistoryList list = new ExternalLoanCompHistoryList();
      for (int index = 0; index < comp.Count; ++index)
        list.Add(new ExternalLoanCompHistory(comp.GetHistoryAt(index)));
      return list;
    }

    internal static ExternalLoanCompHistoryList ToList(List<LoanCompHistory> comp)
    {
      ExternalLoanCompHistoryList list = new ExternalLoanCompHistoryList();
      for (int index = 0; index < comp.Count; ++index)
        list.Add(new ExternalLoanCompHistory(comp[index]));
      return list;
    }

    internal static LoanCompHistory ToLoanCompHistory(ExternalLoanCompHistory extComp)
    {
      return new LoanCompHistory(extComp.Id, extComp.PlanName, extComp.CompPlanId, extComp.StartDate, extComp.EndDate)
      {
        MinTermDays = extComp.MinTermDays,
        PercentAmt = extComp.PercentAmt,
        DollarAmount = extComp.DollarAmount,
        MinDollarAmount = extComp.MinDollarAmount,
        MaxDollarAmount = extComp.MaxDollarAmount,
        PercentAmtBase = extComp.PercentAmtBase,
        RoundingMethod = extComp.RoundingMethod
      };
    }
  }
}
