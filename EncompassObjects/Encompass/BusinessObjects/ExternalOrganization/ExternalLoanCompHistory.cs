// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory
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
  /// <summary>Represents a single ExternalLoanCompHistory.</summary>
  public class ExternalLoanCompHistory : ILoanCompHistory
  {
    private LoanCompHistory loanCompHistory;

    internal ExternalLoanCompHistory(LoanCompHistory loanCompHistory)
    {
      this.loanCompHistory = loanCompHistory;
    }

    /// <summary>Gets Lo Comp history id</summary>
    public string Id => this.loanCompHistory.Id;

    /// <summary>Gets LO comp plan name</summary>
    public string PlanName => this.loanCompHistory.PlanName;

    /// <summary>Gets LO Comp Plan Id</summary>
    public int CompPlanId => this.loanCompHistory.CompPlanId;

    /// <summary>Gets or sets the start date of the LO Comp history</summary>
    public DateTime StartDate
    {
      get => this.loanCompHistory.StartDate;
      set => this.loanCompHistory.StartDate = value;
    }

    /// <summary>Gets or sets the end date of the LO Comp history</summary>
    public DateTime EndDate
    {
      get => this.loanCompHistory.EndDate;
      set => this.loanCompHistory.EndDate = value;
    }

    /// <summary>Gets or sets the minimum term days</summary>
    public int MinTermDays
    {
      get => this.loanCompHistory.MinTermDays;
      set => this.loanCompHistory.MinTermDays = value;
    }

    /// <summary>Gets or sets the percent amount</summary>
    public Decimal PercentAmt
    {
      get => this.loanCompHistory.PercentAmt;
      set => this.loanCompHistory.PercentAmt = value;
    }

    /// <summary>Gets or sets the percent amount base</summary>
    public int PercentAmtBase
    {
      get => this.loanCompHistory.PercentAmtBase;
      set => this.loanCompHistory.PercentAmtBase = value;
    }

    /// <summary>Gets or sets the rounding method</summary>
    public int RoundingMethod
    {
      get => this.loanCompHistory.RoundingMethod;
      set => this.loanCompHistory.RoundingMethod = value;
    }

    /// <summary>Gets or sets the dollar amount</summary>
    public Decimal DollarAmount
    {
      get => this.loanCompHistory.DollarAmount;
      set => this.loanCompHistory.DollarAmount = value;
    }

    /// <summary>Gets or sets the minimum dollar amount</summary>
    public Decimal MinDollarAmount
    {
      get => this.loanCompHistory.MinDollarAmount;
      set => this.loanCompHistory.MinDollarAmount = value;
    }

    /// <summary>Gets or sets the maximum dollar amount</summary>
    public Decimal MaxDollarAmount
    {
      get => this.loanCompHistory.MaxDollarAmount;
      set => this.loanCompHistory.MaxDollarAmount = value;
    }

    internal static ExternalLoanCompHistoryList ToList(LoanCompHistoryList comp)
    {
      ExternalLoanCompHistoryList list = new ExternalLoanCompHistoryList();
      for (int i = 0; i < comp.Count; ++i)
        list.Add(new ExternalLoanCompHistory(comp.GetHistoryAt(i)));
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
