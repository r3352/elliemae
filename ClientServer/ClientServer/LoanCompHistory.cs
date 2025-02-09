// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanCompHistory
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanCompHistory
  {
    private string id = string.Empty;
    private string planName = string.Empty;
    private int compPlanId = -1;
    private DateTime startDate = DateTime.MinValue;
    private DateTime endDate = DateTime.MaxValue;
    private string description = string.Empty;
    private string status = string.Empty;
    private string type = string.Empty;
    private string triggerFieldID = string.Empty;
    private bool newRecord;
    private int minTermDays;
    private Decimal percentAmt;
    private Decimal dollarAmount;
    private Decimal minDollarAmount;
    private Decimal maxDollarAmount;
    private int percentAmtBase;
    private int roundingMethod;

    public LoanCompHistory(
      string id,
      string planName,
      int compPlanId,
      DateTime startDate,
      DateTime endDate)
    {
      this.id = id;
      this.planName = planName;
      this.compPlanId = compPlanId;
      this.startDate = startDate;
      this.endDate = endDate;
    }

    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    public int IdForOrg => Convert.ToInt32(this.id);

    public string PlanName => this.planName;

    public int CompPlanId => this.compPlanId;

    public DateTime StartDate
    {
      get => this.startDate;
      set => this.startDate = value;
    }

    public DateTime EndDate
    {
      get => this.endDate;
      set => this.endDate = value;
    }

    public object Clone()
    {
      return (object) new LoanCompHistory(this.id, this.planName, this.compPlanId, this.startDate, this.endDate)
      {
        MinTermDays = this.minTermDays,
        PercentAmt = this.percentAmt,
        DollarAmount = this.dollarAmount,
        MinDollarAmount = this.minDollarAmount,
        MaxDollarAmount = this.MaxDollarAmount,
        PercentAmtBase = this.percentAmtBase,
        RoundingMethod = this.roundingMethod
      };
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public string Status
    {
      get => this.status;
      set => this.status = value;
    }

    public string Type
    {
      get => this.type;
      set => this.type = value;
    }

    public string TriggerFieldID
    {
      get => this.triggerFieldID;
      set => this.triggerFieldID = value;
    }

    public bool NewRecord
    {
      get => this.newRecord;
      set => this.newRecord = value;
    }

    public int MinTermDays
    {
      get => this.minTermDays;
      set => this.minTermDays = value;
    }

    public Decimal PercentAmt
    {
      get => this.percentAmt;
      set => this.percentAmt = value;
    }

    public Decimal DollarAmount
    {
      get => this.dollarAmount;
      set => this.dollarAmount = value;
    }

    public Decimal MinDollarAmount
    {
      get => this.minDollarAmount;
      set => this.minDollarAmount = value;
    }

    public Decimal MaxDollarAmount
    {
      get => this.maxDollarAmount;
      set => this.maxDollarAmount = value;
    }

    public int PercentAmtBase
    {
      get => this.percentAmtBase;
      set => this.percentAmtBase = value;
    }

    public string PercentAmtBaseToUIStr
    {
      get
      {
        if (this.percentAmtBase == 1)
          return "Total Loan";
        return this.percentAmtBase != 2 ? "" : "Base Loan";
      }
    }

    public int RoundingMethod
    {
      get => this.roundingMethod;
      set => this.roundingMethod = value;
    }

    public string RoundingMethodToUIStr
    {
      get
      {
        if (this.roundingMethod == 1)
          return "To Nearest Cent";
        return this.roundingMethod != 2 ? "" : "To Nearest Dollar";
      }
    }
  }
}
