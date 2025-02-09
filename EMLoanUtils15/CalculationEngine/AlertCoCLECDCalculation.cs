// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.AlertCoCLECDCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class AlertCoCLECDCalculation : CalculationBase
  {
    private const string className = "AlertCoCLECDCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private static string[] LE_COC_Fields = new string[13]
    {
      "3165",
      "3167",
      "3168",
      "3169",
      "LE1.X78",
      "LE1.X79",
      "LE1.X80",
      "LE1.X81",
      "LE1.X82",
      "LE1.X83",
      "LE1.X84",
      "LE1.X85",
      "LE1.X86"
    };
    private static string[] CD_COC_Fields = new string[17]
    {
      "CD1.X52",
      "CD1.X53",
      "CD1.X54",
      "CD1.X55",
      "CD1.X56",
      "CD1.X57",
      "CD1.X58",
      "CD1.X59",
      "CD1.X60",
      "CD1.X61",
      "CD1.X62",
      "CD1.X63",
      "CD1.X64",
      "CD1.X65",
      "CD1.X66",
      "CD1.X67",
      "CD1.X68"
    };

    internal AlertCoCLECDCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      this.AddFieldHandler("XCOC0003", this.RoutineX(new Routine(this.calculateRevisedXCOCDueDate)));
    }

    private void calculateRevisedXCOCDueDate(string id, string val)
    {
      if (!id.StartsWith("XCOC") || !id.EndsWith("03"))
        return;
      string verifBlockNo = this.GetVerifBlockNo(id, 4);
      if (verifBlockNo == "")
        return;
      this.calObjs.NewHudCal.CalculateRevisedDueDate("XCOC" + verifBlockNo + "03", "XCOC" + verifBlockNo + "04", (bool) this.loan.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRediscloseDueDate"], true, true);
    }

    internal void CopyAlertCoCToLECDPage1(bool copyToCD)
    {
      this.clearCDLEPage1();
      if (this.loan.GetNumberOfGoodFaithChangeOfCircumstance() == 0)
      {
        this.SetVal("3627", "");
      }
      else
      {
        this.setCDLEPage1Description(copyToCD);
        this.SetVal("3627", this.calObjs.NewHudCal.GetAllChangeCircumstance());
      }
    }

    private void clearCDLEPage1()
    {
      for (int index = 0; index < AlertCoCLECDCalculation.LE_COC_Fields.Length; ++index)
      {
        if (!this.loan.Locked_COC_Fields.Contains((object) AlertCoCLECDCalculation.LE_COC_Fields[index]))
        {
          if (this.loan.IsLocked(AlertCoCLECDCalculation.LE_COC_Fields[index]))
          {
            this.loan.RemoveLock(AlertCoCLECDCalculation.LE_COC_Fields[index]);
            this.SetVal(AlertCoCLECDCalculation.LE_COC_Fields[index], "");
            this.loan.AddLock(AlertCoCLECDCalculation.LE_COC_Fields[index]);
          }
          else
            this.SetVal(AlertCoCLECDCalculation.LE_COC_Fields[index], "");
        }
      }
      for (int index = 0; index < AlertCoCLECDCalculation.CD_COC_Fields.Length; ++index)
      {
        if (!this.loan.Locked_COC_Fields.Contains((object) AlertCoCLECDCalculation.CD_COC_Fields[index]))
        {
          if (this.loan.IsLocked(AlertCoCLECDCalculation.CD_COC_Fields[index]))
          {
            this.loan.RemoveLock(AlertCoCLECDCalculation.CD_COC_Fields[index]);
            this.SetVal(AlertCoCLECDCalculation.CD_COC_Fields[index], "");
            this.loan.AddLock(AlertCoCLECDCalculation.CD_COC_Fields[index]);
          }
          this.SetVal(AlertCoCLECDCalculation.CD_COC_Fields[index], "");
        }
      }
    }

    internal bool COCInLECDIsModified(bool forCD)
    {
      Dictionary<string, string> page1DisclosureInfo = this.getCDLEPage1DisclosureInfo(forCD);
      if (forCD)
      {
        for (int index = 0; index < AlertCoCLECDCalculation.CD_COC_Fields.Length; ++index)
        {
          if (this.compareSyncCoCwithPage(AlertCoCLECDCalculation.CD_COC_Fields[index], page1DisclosureInfo.ContainsKey(AlertCoCLECDCalculation.CD_COC_Fields[index]) ? page1DisclosureInfo[AlertCoCLECDCalculation.CD_COC_Fields[index]] : ""))
            return true;
        }
      }
      else
      {
        for (int index = 0; index < AlertCoCLECDCalculation.LE_COC_Fields.Length; ++index)
        {
          if (this.compareSyncCoCwithPage(AlertCoCLECDCalculation.LE_COC_Fields[index], page1DisclosureInfo.ContainsKey(AlertCoCLECDCalculation.LE_COC_Fields[index]) ? page1DisclosureInfo[AlertCoCLECDCalculation.LE_COC_Fields[index]] : ""))
            return true;
        }
      }
      return false;
    }

    private bool compareSyncCoCwithPage(string id, string syncValue)
    {
      StandardField field = StandardFields.GetField(id);
      string dateValue = this.Val(id);
      if (field.Format == FieldFormat.YN && dateValue != "Y")
      {
        if (syncValue == "Y")
          return true;
      }
      else if (field.Format == FieldFormat.DATE || field.Format == FieldFormat.DATETIME)
      {
        if (DateTime.Compare(Utils.ToDate(syncValue), Utils.ToDate(dateValue)) != 0)
          return true;
      }
      else if (!string.Equals(syncValue.Replace("\r\n", "").Replace(" ", ""), dateValue.Replace("\r\n", "").Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
        return true;
      return false;
    }

    private void setCDLEPage1Description(bool forCD)
    {
      foreach (KeyValuePair<string, string> keyValuePair in this.getCDLEPage1DisclosureInfo(forCD))
        this.SetVal(keyValuePair.Key, keyValuePair.Value);
    }

    private Dictionary<string, string> getCDLEPage1DisclosureInfo(bool forCD)
    {
      Dictionary<string, string> page1DisclosureInfo = new Dictionary<string, string>();
      if (this.loan.LoanDisclosedClearGFE)
      {
        this.loan.LoanDisclosedClearGFE = false;
        return page1DisclosureInfo;
      }
      string circumstanceRecordFieldStr1 = this.loan.GetGoodFaithChangeOfCircumstanceRecordFieldStr("05");
      if (!this.loan.Locked_COC_Fields.Contains(forCD ? (object) "CD1.X64" : (object) "3169") && !page1DisclosureInfo.ContainsKey(forCD ? "CD1.X64" : "3169"))
        page1DisclosureInfo.Add(forCD ? "CD1.X64" : "3169", circumstanceRecordFieldStr1);
      string circumstanceRecordFieldStr2 = this.loan.GetGoodFaithChangeOfCircumstanceRecordFieldStr("06");
      if (!this.loan.Locked_COC_Fields.Contains(forCD ? (object) "CD1.X65" : (object) "LE1.X86") && !page1DisclosureInfo.ContainsKey(forCD ? "CD1.X65" : "LE1.X86"))
        page1DisclosureInfo.Add(forCD ? "CD1.X65" : "LE1.X86", circumstanceRecordFieldStr2);
      if (!this.loan.Locked_COC_Fields.Contains(forCD ? (object) "CD1.X61" : (object) "3168") && !page1DisclosureInfo.ContainsKey(forCD ? "CD1.X61" : "3168"))
        page1DisclosureInfo.Add(forCD ? "CD1.X61" : "3168", "Y");
      string earliestDates1 = this.getEarliestDates(this.loan.GetGoodFaithChangeOfCircumstanceRecordField("03"));
      if (!this.loan.Locked_COC_Fields.Contains(forCD ? (object) "CD1.X62" : (object) "3165") && !page1DisclosureInfo.ContainsKey(forCD ? "CD1.X62" : "3165"))
        page1DisclosureInfo.Add(forCD ? "CD1.X62" : "3165", earliestDates1);
      string earliestDates2 = this.getEarliestDates(this.loan.GetGoodFaithChangeOfCircumstanceRecordField("04"));
      if (!page1DisclosureInfo.ContainsKey(forCD ? "CD1.X63" : "3167"))
        page1DisclosureInfo.Add(forCD ? "CD1.X63" : "3167", earliestDates2);
      foreach (string str in this.loan.GetGoodFaithChangeOfCircumstanceRecordField("07"))
      {
        switch (str)
        {
          case "24-hour Advanced Preview":
            if (!this.loan.Locked_COC_Fields.Contains((object) "CD1.X56") && !page1DisclosureInfo.ContainsKey("CD1.X56"))
            {
              page1DisclosureInfo.Add("CD1.X56", "Y");
              break;
            }
            break;
          case "Change in APR":
            if (!this.loan.Locked_COC_Fields.Contains((object) "CD1.X52") && !page1DisclosureInfo.ContainsKey("CD1.X52"))
            {
              page1DisclosureInfo.Add("CD1.X52", "Y");
              break;
            }
            break;
          case "Change in Loan Product":
            if (!this.loan.Locked_COC_Fields.Contains((object) "CD1.X53") && !page1DisclosureInfo.ContainsKey("CD1.X53"))
            {
              page1DisclosureInfo.Add("CD1.X53", "Y");
              break;
            }
            break;
          case "Changed Circumstance - Eligibility":
            if (!page1DisclosureInfo.ContainsKey(forCD ? "CD1.X68" : "LE1.X79"))
            {
              page1DisclosureInfo.Add(forCD ? "CD1.X68" : "LE1.X79", "Y");
              break;
            }
            break;
          case "Changed Circumstance - Settlement Charges":
            if (!page1DisclosureInfo.ContainsKey(forCD ? "CD1.X55" : "LE1.X78"))
            {
              page1DisclosureInfo.Add(forCD ? "CD1.X55" : "LE1.X78", "Y");
              break;
            }
            break;
          case "Clerical Error Correction":
            if (!this.loan.Locked_COC_Fields.Contains((object) "CD1.X58") && !page1DisclosureInfo.ContainsKey("CD1.X58"))
            {
              page1DisclosureInfo.Add("CD1.X58", "Y");
              break;
            }
            break;
          case "Delayed Settlement on Construction Loans":
            if (!page1DisclosureInfo.ContainsKey("LE1.X83"))
            {
              page1DisclosureInfo.Add("LE1.X83", "Y");
              break;
            }
            break;
          case "Expiration (Intent to Proceed received after 10 business days)":
            if (!page1DisclosureInfo.ContainsKey("LE1.X82"))
            {
              page1DisclosureInfo.Add("LE1.X82", "Y");
              break;
            }
            break;
          case "Interest Rate dependent charges (Rate Lock)":
            if (!page1DisclosureInfo.ContainsKey(forCD ? "CD1.X67" : "LE1.X81"))
            {
              page1DisclosureInfo.Add(forCD ? "CD1.X67" : "LE1.X81", "Y");
              break;
            }
            break;
          case "Other":
            if (!this.loan.Locked_COC_Fields.Contains(forCD ? (object) "CD1.X59" : (object) "LE1.X84") && !page1DisclosureInfo.ContainsKey(forCD ? "CD1.X59" : "LE1.X84"))
            {
              page1DisclosureInfo.Add(forCD ? "CD1.X59" : "LE1.X84", "Y");
              break;
            }
            break;
          case "Prepayment Penalty Added":
            if (!this.loan.Locked_COC_Fields.Contains((object) "CD1.X54") && !page1DisclosureInfo.ContainsKey("CD1.X54"))
            {
              page1DisclosureInfo.Add("CD1.X54", "Y");
              break;
            }
            break;
          case "Revisions requested by the Consumer":
            if (!page1DisclosureInfo.ContainsKey(forCD ? "CD1.X66" : "LE1.X80"))
            {
              page1DisclosureInfo.Add(forCD ? "CD1.X66" : "LE1.X80", "Y");
              break;
            }
            break;
          case "Tolerance Cure":
            if (!this.loan.Locked_COC_Fields.Contains((object) "CD1.X57") && !page1DisclosureInfo.ContainsKey("CD1.X57"))
            {
              page1DisclosureInfo.Add("CD1.X57", "Y");
              break;
            }
            break;
        }
      }
      string str1 = this.loan.GetGoodFaithChangeOfCircumstanceRecordFieldStr("08").Replace("\r\n", "; ").TrimEnd(';', ' ');
      if (!this.loan.Locked_COC_Fields.Contains(forCD ? (object) "CD1.X60" : (object) "LE1.X85") && !page1DisclosureInfo.ContainsKey(forCD ? "CD1.X60" : "LE1.X85"))
        page1DisclosureInfo.Add(forCD ? "CD1.X60" : "LE1.X85", str1);
      return page1DisclosureInfo;
    }

    private string getEarliestDates(string[] dates)
    {
      DateTime dateTime1 = DateTime.MaxValue;
      if (dates != null)
      {
        for (int index = 0; index < dates.Length; ++index)
        {
          if (dates[index].Length > 0)
          {
            try
            {
              DateTime dateTime2 = Convert.ToDateTime(dates[index]);
              if (dateTime2 < dateTime1)
                dateTime1 = dateTime2;
            }
            catch
            {
            }
          }
        }
      }
      return !(dateTime1 == DateTime.MaxValue) ? dateTime1.ToShortDateString() : "";
    }
  }
}
