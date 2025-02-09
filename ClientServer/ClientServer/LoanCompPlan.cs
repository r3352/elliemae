// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanCompPlan
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanCompPlan
  {
    private int id = -1;
    private string name = string.Empty;
    private string description = string.Empty;
    private LoanCompPlanType planType;
    private bool status;
    private DateTime activationDate;
    private int minTermDays;
    private Decimal percentAmt;
    private int percentAmtBase;
    private int roundingMethod;
    private Decimal dollarAmount;
    private Decimal minDollarAmount;
    private Decimal maxDollarAmount;
    private string triggerField;

    public LoanCompPlan()
    {
    }

    public LoanCompPlan(
      int id,
      string name,
      string description,
      LoanCompPlanType planType,
      bool Status,
      DateTime activationDate,
      int minTermDays,
      Decimal percentAmt,
      int percentAmtBase,
      int roundingMethod,
      Decimal dollarAmount,
      Decimal minDollarAmount,
      Decimal maxDollarAmount,
      string triggerField)
    {
      this.id = id;
      this.name = name;
      this.description = description;
      this.planType = planType;
      this.Status = Status;
      this.activationDate = activationDate;
      this.minTermDays = minTermDays;
      this.percentAmt = percentAmt;
      this.percentAmtBase = percentAmtBase;
      this.roundingMethod = roundingMethod;
      this.dollarAmount = dollarAmount;
      this.minDollarAmount = minDollarAmount;
      this.maxDollarAmount = maxDollarAmount;
      this.triggerField = triggerField;
    }

    public LoanCompPlan(
      string name,
      string description,
      LoanCompPlanType planType,
      bool status,
      DateTime activationDate,
      int minTermDays,
      Decimal percentAmt,
      int percentAmtBase,
      int roundingMethod,
      Decimal dollarAmount,
      Decimal minDollarAmount,
      Decimal maxDollarAmount,
      string triggerField)
      : this(-1, name, description, planType, status, activationDate, minTermDays, percentAmt, percentAmtBase, roundingMethod, dollarAmount, minDollarAmount, maxDollarAmount, triggerField)
    {
    }

    public LoanCompPlan(string param)
    {
      string[] strArray = param.Split(',');
      if (strArray.Length != 14)
        return;
      this.id = Convert.ToInt32(strArray[0]);
      this.name = strArray[1];
      this.description = strArray[2];
      this.planType = (LoanCompPlanType) Enum.ToObject(typeof (LoanCompPlanType), Convert.ToInt32(strArray[3]));
      this.status = Convert.ToBoolean(strArray[4]);
      this.activationDate = Convert.ToDateTime(strArray[5]);
      this.minTermDays = Convert.ToInt32(strArray[6]);
      this.percentAmt = Convert.ToDecimal(strArray[7]);
      this.percentAmtBase = Convert.ToInt32(strArray[8]);
      this.roundingMethod = Convert.ToInt32(strArray[9]);
      this.dollarAmount = Convert.ToDecimal(strArray[10]);
      this.minDollarAmount = Convert.ToDecimal(strArray[11]);
      this.maxDollarAmount = Convert.ToDecimal(strArray[12]);
      this.triggerField = strArray[13];
    }

    public int Id
    {
      get => this.id;
      set => this.id = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public LoanCompPlanType PlanType
    {
      get => this.planType;
      set => this.planType = value;
    }

    public bool Status
    {
      get => this.status;
      set => this.status = value;
    }

    public DateTime ActivationDate
    {
      get => this.activationDate;
      set => this.activationDate = value;
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

    public int PercentAmtBase
    {
      get => this.percentAmtBase;
      set => this.percentAmtBase = value;
    }

    public int RoundingMethod
    {
      get => this.roundingMethod;
      set => this.roundingMethod = value;
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

    public string TriggerField
    {
      get => this.triggerField;
      set => this.triggerField = value;
    }

    public override string ToString()
    {
      return this.name == "" ? this.id.ToString() : this.name + "/" + (object) this.id;
    }

    public override bool Equals(object obj)
    {
      return obj is LoanCompPlan loanCompPlan && this.id == loanCompPlan.id && string.Compare(this.name, loanCompPlan.name, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.name + (object) this.id);
    }

    public object Clone()
    {
      return (object) new LoanCompPlan()
      {
        Name = (this.name + " - Copy"),
        Description = this.description,
        PlanType = this.planType,
        Status = this.status,
        ActivationDate = this.activationDate,
        MinTermDays = this.minTermDays,
        PercentAmt = this.percentAmt,
        PercentAmtBase = this.percentAmtBase,
        RoundingMethod = this.roundingMethod,
        DollarAmount = this.dollarAmount,
        MinDollarAmount = this.minDollarAmount,
        MaxDollarAmount = this.maxDollarAmount,
        TriggerField = this.triggerField
      };
    }
  }
}
