// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AIQEmploymentData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class AIQEmploymentData : IComparable
  {
    public string employerFullname { get; set; }

    public string startDate { get; set; }

    public string endDate { get; set; }

    public string currentOrPrior { get; set; }

    public string primaryOrSecondary { get; set; }

    public string baseAmt { get; set; }

    public string bonusAmt { get; set; }

    public string commissionAmt { get; set; }

    public string overtimeAmt { get; set; }

    public string militaryRationsAmt { get; set; }

    public string militaryHousingAmt { get; set; }

    public bool isSelfEmployed { get; set; }

    public string losId { get; set; }

    public bool isSelected { get; set; }

    public int linkedEncompassVOEBlockNumber { get; set; }

    internal AIQEmploymentData(JAIQEmployers aiqData)
    {
      this.linkedEncompassVOEBlockNumber = -1;
      this.startDate = this.endDate = this.currentOrPrior = this.primaryOrSecondary = this.losId = this.baseAmt = this.bonusAmt = this.commissionAmt = this.militaryRationsAmt = this.militaryHousingAmt = this.overtimeAmt = string.Empty;
      if (aiqData.legalEntity != null)
        this.employerFullname = aiqData.legalEntity.fullName;
      if (aiqData.employment != null)
      {
        this.startDate = aiqData.employment.startDate;
        this.endDate = aiqData.employment.endDate;
        this.currentOrPrior = aiqData.employment.statusType;
        this.primaryOrSecondary = aiqData.employment.classificationType;
        this.losId = aiqData.employment.losId;
      }
      IList<JAIQCurrentIncome> currentIncome = aiqData?.currentIncome;
      if (currentIncome == null)
        return;
      double monthlyTotalAmount;
      foreach (JAIQCurrentIncome jaiqCurrentIncome in (IEnumerable<JAIQCurrentIncome>) currentIncome)
      {
        if (jaiqCurrentIncome.selected)
        {
          switch (jaiqCurrentIncome.type)
          {
            case "Base":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.baseAmt = monthlyTotalAmount.ToString("N2");
              continue;
            case "Bonus":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.bonusAmt = monthlyTotalAmount.ToString("N2");
              continue;
            case "Commissions":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.commissionAmt = monthlyTotalAmount.ToString("N2");
              continue;
            case "MilitaryRationsAllowance":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.militaryRationsAmt = monthlyTotalAmount.ToString("N2");
              continue;
            case "MilitaryVariableHousingAllowance":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.militaryHousingAmt = monthlyTotalAmount.ToString("N2");
              continue;
            case "Overtime":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.overtimeAmt = monthlyTotalAmount.ToString("N2");
              continue;
            case "SelfEmploymentIncome":
              monthlyTotalAmount = jaiqCurrentIncome.monthlyTotalAmount;
              this.baseAmt = monthlyTotalAmount.ToString("N2");
              this.isSelfEmployed = true;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public string GetField(string id)
    {
      switch (id)
      {
        case "AIQBE0002":
          return this.employerFullname;
        case "AIQBE0014":
          return this.endDate;
        case "AIQBE0019":
          return this.baseAmt;
        case "AIQBE0020":
          return this.overtimeAmt;
        case "AIQBE0021":
          return this.bonusAmt;
        case "AIQBE0022":
          return this.commissionAmt;
        case "AIQBE0051":
          return this.startDate;
        case "AIQBE0070":
          return this.militaryRationsAmt;
        case "AIQBE0071":
          return this.militaryHousingAmt;
        default:
          return "";
      }
    }

    public int CompareTo(object obj)
    {
      AIQEmploymentData aiqEmploymentData = obj as AIQEmploymentData;
      if (this.currentOrPrior == aiqEmploymentData.currentOrPrior)
      {
        if (this.primaryOrSecondary == aiqEmploymentData.primaryOrSecondary)
          return 0;
        return this.primaryOrSecondary == "Primary" ? -1 : 1;
      }
      return this.currentOrPrior == "Current" ? -1 : 1;
    }

    public void import(LoanData loan, bool bOrC)
    {
      try
      {
        string str1 = bOrC ? "BE" : "CE";
        if (this.linkedEncompassVOEBlockNumber == -1)
          this.linkedEncompassVOEBlockNumber = loan.NewEmployer(bOrC, this.currentOrPrior == "Current") + 1;
        string str2 = str1 + this.linkedEncompassVOEBlockNumber.ToString("00");
        loan.SetField(str2 + "02", this.employerFullname);
        loan.SetField(str2 + "51", this.startDate);
        loan.SetField(str2 + "14", this.endDate);
        loan.SetField(str2 + "21", this.bonusAmt);
        loan.SetField(str2 + "22", this.commissionAmt);
        loan.SetField(str2 + "20", this.overtimeAmt);
        loan.SetField(str2 + "70", this.militaryRationsAmt);
        loan.SetField(str2 + "71", this.militaryHousingAmt);
        if (this.isSelfEmployed)
        {
          loan.SetField(str2 + "15", "Y");
          loan.SetField(str2 + "56", this.baseAmt);
        }
        else
        {
          loan.SetField(str2 + "15", "N");
          loan.SetField(str2 + "19", this.baseAmt);
        }
        DateTime now = DateTime.Now;
        loan.SetField(str2 + "81", "Income Analyzer " + now.ToString("MM/dd/yyyy") + " " + now.ToLongTimeString() + " " + Utils.CurrentTimeZoneName);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
