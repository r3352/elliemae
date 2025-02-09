// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RegulationAlerts
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class RegulationAlerts
  {
    public static string[][] GetAUSDataDiscrepancyAlertFields(
      bool forLP,
      bool isPrimary = false,
      bool isZeroQualifyRate = false)
    {
      if (forLP)
      {
        string[][] discrepancyAlertFields = new string[12][]
        {
          new string[3]{ "CASASRN.X217", "AUS.X11", "AUS.CuredX11" },
          new string[3]{ "CASASRN.X218", "AUS.X12", "AUS.CuredX12" },
          new string[3]{ "CASASRN.X201", "AUS.X14", "AUS.CuredX14" },
          new string[3]{ "CASASRN.X202", "AUS.X15", "AUS.CuredX15" },
          new string[3]{ "2", "AUS.X16", "AUS.CuredX16" },
          new string[3]{ "1821", "AUS.X18", "AUS.CuredX18" },
          new string[3]{ "3", "AUS.X19", "AUS.CuredX19" },
          new string[3]{ "1172", "AUS.X20", "AUS.CuredX20" },
          new string[3]{ "4", "AUS.X21", "AUS.CuredX21" },
          new string[3]{ "608", "AUS.X22", "AUS.CuredX22" },
          null,
          null
        };
        string[] strArray;
        if (!isPrimary)
          strArray = new string[3]
          {
            "912",
            "AUS.X32",
            "AUS.CuredX32"
          };
        else
          strArray = new string[3]
          {
            "1731",
            "AUS.X32",
            "AUS.CuredX32"
          };
        discrepancyAlertFields[10] = strArray;
        discrepancyAlertFields[11] = new string[3]
        {
          "1389",
          "AUS.X41",
          "AUS.CuredX41"
        };
        return discrepancyAlertFields;
      }
      string[][] discrepancyAlertFields1 = new string[14][]
      {
        new string[3]{ "MORNET.X75", "AUS.X11", "AUS.CuredX11" },
        new string[3]{ "MORNET.X76", "AUS.X12", "AUS.CuredX12" },
        new string[3]{ "740", "AUS.X14", "AUS.CuredX14" },
        new string[3]{ "742", "AUS.X15", "AUS.CuredX15" },
        new string[3]{ "2", "AUS.X16", "AUS.CuredX16" },
        new string[3]{ "356", "AUS.X18", "AUS.CuredX18" },
        new string[3]{ "3", "AUS.X19", "AUS.CuredX19" },
        new string[3]{ "1172", "AUS.X20", "AUS.CuredX20" },
        new string[3]{ "4", "AUS.X21", "AUS.CuredX21" },
        new string[3]{ "608", "AUS.X22", "AUS.CuredX22" },
        new string[3]{ "19", "AUS.X23", "AUS.CuredX23" },
        null,
        null,
        null
      };
      string[] strArray1;
      if (!(isPrimary & isZeroQualifyRate))
        strArray1 = new string[3]
        {
          "912",
          "AUS.X32",
          "AUS.CuredX32"
        };
      else
        strArray1 = new string[3]
        {
          "1731",
          "AUS.X32",
          "AUS.CuredX32"
        };
      discrepancyAlertFields1[11] = strArray1;
      discrepancyAlertFields1[12] = new string[3]
      {
        "1389",
        "AUS.X41",
        "AUS.CuredX41"
      };
      discrepancyAlertFields1[13] = new string[3]
      {
        "4752",
        "AUS.X199",
        "AUS.CuredX199"
      };
      return discrepancyAlertFields1;
    }

    public static PipelineInfo.Alert[] GetAlerts(LoanData loan)
    {
      return new List<PipelineInfo.Alert>()
      {
        RegulationAlerts.GetInitialDisclosureAlert(loan),
        RegulationAlerts.GetThreeDayDisclosureRequirementsAlert(loan),
        RegulationAlerts.GetAtAppDisclosureRequirements(loan),
        RegulationAlerts.GeteSignConsentAlert(loan),
        RegulationAlerts.GetKeyPricingFieldAlert(loan),
        RegulationAlerts.GetGFEExpiresAlert(loan),
        RegulationAlerts.GetLEExpiresAlert(loan),
        RegulationAlerts.GetRediscloseGFERateLockAlert(loan),
        RegulationAlerts.GetRediscloseCDRateLockAlert(loan),
        RegulationAlerts.GetRediscloseGFEChangedCircumstanceAlert(loan),
        RegulationAlerts.GetRediscloseLEChangedCircumstanceAlert(loan),
        RegulationAlerts.GetHUDToleranceViolationAlert(loan),
        RegulationAlerts.GetRediscloseTILAlertRateChange(loan),
        RegulationAlerts.GetRediscloseCDAPR_Product_Prepay(loan),
        RegulationAlerts.GetClosingDateViolationAlert(loan),
        RegulationAlerts.GetAbilityToRepayLoanTypeNotDetermined(loan),
        RegulationAlerts.GetQualifiedMortgageTypeNotDetermined(loan),
        RegulationAlerts.GetQMSafeHarborEligibilityNotDetermined(loan),
        RegulationAlerts.GetResidualIncomeAssessmentRecommended(loan),
        RegulationAlerts.GetStandardQMDTIExceeded(loan),
        RegulationAlerts.GetStandardQMPriceExceeded(loan),
        RegulationAlerts.GetGeneralQMLoanFeatureViolation(loan),
        RegulationAlerts.GetAbilitytoRepayExemptionReasonNotDetermined(loan),
        RegulationAlerts.GetAUSDataDiscrepancyAlert(loan),
        RegulationAlerts.GetRediscloseLERateLockAlert(loan),
        RegulationAlerts.GetRediscloseCDChangedCircumstanceAlert(loan),
        RegulationAlerts.GetGoodFaithFeeVarianceViolationAlert(loan),
        RegulationAlerts.GetWithdrawnLoanAlert(loan),
        RegulationAlerts.GetVADiscountChargeViolationAlert(loan),
        RegulationAlerts.GetPositiveAggregateEscrowAdjustment(loan),
        RegulationAlerts.GetRediscloseCdAfterVestingNboAddedAlert(loan)
      }.Where<PipelineInfo.Alert>((Func<PipelineInfo.Alert, bool>) (a => a != null)).ToArray<PipelineInfo.Alert>();
    }

    public static PipelineInfo.Alert GetVADiscountChargeViolationAlert(LoanData loan)
    {
      return loan.GetField("1172") == "VA" && Utils.ParseDouble((object) loan.GetField("VARRRWS.X9")) > 2.0 && loan.GetField("958") == "IRRRL" ? new PipelineInfo.Alert(62, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static string GetAUSData(LoanData loan, string triggerField, bool hasCuredValue)
    {
      string ausData = "";
      AUSTrackingHistoryLog trackingLog = (AUSTrackingHistoryLog) null;
      for (int i = 0; i < loan.GetAUSTrackingHistoryList().HistoryCount; ++i)
      {
        AUSTrackingHistoryLog historyAt = loan.GetAUSTrackingHistoryList().GetHistoryAt(i);
        List<string> stringList = new List<string>()
        {
          "LQA",
          "EarlyCheck"
        };
        string field = historyAt.DataValues.GetField("AUS.X1");
        if (historyAt != null && !stringList.Contains(field))
        {
          trackingLog = historyAt;
          break;
        }
      }
      if (trackingLog == null)
        return (string) null;
      if (!hasCuredValue)
      {
        ausData = trackingLog.GetField(triggerField);
      }
      else
      {
        string[] strArray = ((IEnumerable<string[]>) RegulationAlerts.GetAUSDataDiscrepancyAlertFields(loan.CheckIfAUSTrackingForLP(trackingLog), loan.GetField("1811") == "PrimaryResidence", Utils.ParseDouble((object) loan.GetField("1014")) == 0.0)).FirstOrDefault<string[]>((Func<string[], bool>) (x => x[1] == triggerField));
        if (strArray != null)
          ausData = loan.GetField(strArray[2]);
      }
      return ausData;
    }

    public static bool HasAUSCuredValue(LoanData loan)
    {
      bool flag = false;
      if (Utils.IsDate((object) loan.GetField("AUSF.X19")))
        flag = true;
      return flag;
    }

    public static PipelineInfo.Alert GetAUSDataDiscrepancyAlert(LoanData loan)
    {
      if (loan.GetAUSTrackingHistoryList().HistoryCount == 0)
        return (PipelineInfo.Alert) null;
      bool hasCuredValue = RegulationAlerts.HasAUSCuredValue(loan);
      bool flag = false;
      AUSTrackingHistoryLog trackingLog = (AUSTrackingHistoryLog) null;
      for (int i = 0; i < loan.GetAUSTrackingHistoryList().HistoryCount; ++i)
      {
        AUSTrackingHistoryLog historyAt = loan.GetAUSTrackingHistoryList().GetHistoryAt(i);
        List<string> stringList = new List<string>()
        {
          "LQA",
          "EarlyCheck"
        };
        string field = historyAt.DataValues.GetField("AUS.X1");
        if (historyAt != null && !stringList.Contains(field))
        {
          trackingLog = historyAt;
          break;
        }
      }
      if (trackingLog == null)
        return (PipelineInfo.Alert) null;
      foreach (string[] discrepancyAlertField in RegulationAlerts.GetAUSDataDiscrepancyAlertFields(loan.CheckIfAUSTrackingForLP(trackingLog), loan.GetField("1811") == "PrimaryResidence", Utils.ParseDouble((object) loan.GetField("1014")) == 0.0))
      {
        string strA = loan.GetField(discrepancyAlertField[0]);
        string ausData = RegulationAlerts.GetAUSData(loan, discrepancyAlertField[1], hasCuredValue);
        Decimal num1;
        switch (discrepancyAlertField[0].ToLower())
        {
          case "1389":
            if (string.Equals(trackingLog.RecordType, "DU") || string.Equals(trackingLog.SubmissionType, "DU"))
            {
              strA = loan.GetField("MORNET.X160");
              break;
            }
            break;
          case "4752":
            if (loan.GetField("4830") == "Y")
            {
              strA = "not applicable";
              break;
            }
            break;
          case "740":
          case "742":
            if (string.Equals(discrepancyAlertField[0], "740") && (string.Equals(trackingLog.RecordType, "DU") || string.Equals(trackingLog.SubmissionType, "DU")))
              strA = loan.GetField("MORNET.X158");
            if (string.Equals(discrepancyAlertField[0], "742") && (string.Equals(trackingLog.RecordType, "DU") || string.Equals(trackingLog.SubmissionType, "DU")))
              strA = loan.GetField("MORNET.X159");
            if (!loan.CheckIfAUSTrackingForLP(trackingLog))
            {
              if (strA != "" && Convert.ToDecimal(strA) != 0M)
              {
                num1 = Math.Truncate(100M * Convert.ToDecimal(strA)) / 100M;
                strA = num1.ToString();
              }
              if (ausData != "" && Convert.ToDecimal(ausData) != 0M)
              {
                num1 = Math.Truncate(100M * Convert.ToDecimal(ausData)) / 100M;
                ausData = num1.ToString();
                break;
              }
              break;
            }
            break;
          case "casasrn.x217":
            if (!string.IsNullOrEmpty(strA) && !string.IsNullOrEmpty(ausData) && Convert.ToDecimal(strA) != 0M && Convert.ToDecimal(ausData) != 0M)
            {
              Decimal val = Convert.ToDecimal(strA);
              Decimal num2 = Convert.ToDecimal(ausData);
              if (Utils.ArithmeticRounding(val, 3) == num2)
              {
                num1 = Utils.ArithmeticRounding(val, 3);
                strA = num1.ToString("N3");
              }
              else if (Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M == num2)
              {
                num1 = Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M;
                strA = num1.ToString("N3");
              }
              ausData = num2.ToString("N3");
              break;
            }
            break;
          case "casasrn.x218":
            if (!string.IsNullOrEmpty(strA) && !string.IsNullOrEmpty(ausData) && Convert.ToDecimal(strA) != 0M && Convert.ToDecimal(ausData) != 0M)
            {
              Decimal val = Convert.ToDecimal(strA);
              Decimal num3 = Convert.ToDecimal(ausData);
              if (Utils.ArithmeticRounding(val, 3) == num3)
              {
                num1 = Utils.ArithmeticRounding(val, 3);
                strA = num1.ToString("N3");
              }
              else if (Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M == num3)
              {
                num1 = Math.Truncate(1000M * Convert.ToDecimal(strA)) / 1000M;
                strA = num1.ToString("N3");
              }
              ausData = num3.ToString("N3");
              break;
            }
            break;
          case "mornet.x75":
          case "mornet.x76":
            strA += "0";
            break;
        }
        if (string.Compare(strA, ausData, true) != 0)
        {
          flag = true;
          break;
        }
      }
      return loan.GetField("1544") != "" & flag ? new PipelineInfo.Alert(42, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetAbilitytoRepayExemptionReasonNotDetermined(LoanData loan)
    {
      return loan.GetField("QM.X23") == "Exempt" && loan.GetField("QM.X105") == "" && loan.GetField("QM.X107") == "" && loan.GetField("QM.X108") == "" && loan.GetField("QM.X110") == "" ? new PipelineInfo.Alert(41, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetGeneralQMLoanFeatureViolation(LoanData loan)
    {
      return loan.GetField("QM.X24") == "General QM" && loan.GetField("QM.X23") == "Qualified Mortgage" && (loan.GetField("2982") == "Y" || loan.GetField("NEWHUD.X6") == "Y" || loan.GetField("1659") == "Y" || Utils.ParseInt((object) loan.GetField("RE88395.X316"), 0) > 36 || Utils.ParseDecimal((object) loan.GetField("QM.X112"), 0M) > 2M || loan.GetField("QM.X124") == "does") ? new PipelineInfo.Alert(40, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetStandardQMDTIExceeded(LoanData loan)
    {
      return loan.GetField("QM.X24") == "General QM" && loan.GetField("QM.X23") == "Qualified Mortgage" && Utils.ParseDecimal((object) loan.GetField("QM.X119"), 0M) > Decimal.Parse("43") && loan.GetField("QM.X383") == "N" ? new PipelineInfo.Alert(39, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetStandardQMPriceExceeded(LoanData loan)
    {
      return loan.GetField("QM.X24") == "General QM" && loan.GetField("QM.X23") == "Qualified Mortgage" && loan.GetField("QM.X383") == "Y" && loan.GetField("QM.X40") == "Not Meet" ? new PipelineInfo.Alert(70, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetAbilityToRepayLoanTypeNotDetermined(LoanData loan)
    {
      return loan.GetField("QM.X23") == "" ? new PipelineInfo.Alert(35, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetQualifiedMortgageTypeNotDetermined(LoanData loan)
    {
      return loan.GetField("QM.X24") == "" && loan.GetField("QM.X23") == "Qualified Mortgage" ? new PipelineInfo.Alert(36, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetQMSafeHarborEligibilityNotDetermined(LoanData loan)
    {
      return loan.GetField("QM.X25") == "" && loan.GetField("QM.X23") == "Qualified Mortgage" ? new PipelineInfo.Alert(37, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetResidualIncomeAssessmentRecommended(LoanData loan)
    {
      return loan.GetField("QM.X25") == "No" && loan.GetField("QM.X23") == "Qualified Mortgage" || loan.GetField("QM.X23") == "General ATR" ? new PipelineInfo.Alert(38, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static bool DisableKeyPricingAlerts { get; set; }

    public static PipelineInfo.Alert GetKeyPricingFieldAlert(LoanData loan)
    {
      if (RegulationAlerts.DisableKeyPricingAlerts || loan == null)
        return (PipelineInfo.Alert) null;
      bool flag = false;
      LockConfirmLog confirmForCurrentLock = loan.GetLogList().GetMostRecentConfirmForCurrentLock();
      if (confirmForCurrentLock != null)
        flag = confirmForCurrentLock.CommitmentTermEnabled;
      if (string.Equals(loan.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) & flag)
      {
        if (string.IsNullOrEmpty(loan.GetField("TPO.X14")))
          return (PipelineInfo.Alert) null;
        if (loan.GetField("4532") != "Y")
          return (PipelineInfo.Alert) null;
        return loan.GetField("4062") == "Y" ? (PipelineInfo.Alert) null : RegulationAlerts.GetKeyPricingAlerts(loan);
      }
      return loan.GetField("2400") == "Y" && loan.GetField("4062") != "Y" ? RegulationAlerts.GetKeyPricingAlerts(loan) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetKeyPricingAlerts(LoanData loan)
    {
      if (loan != null)
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        AlertConfig alertConfig = loan.Settings.AlertSetupData.GetAlertConfig(43);
        RegulationAlert definition = (RegulationAlert) alertConfig.Definition;
        LockRequestLog lastConfirmedLock = loan.GetLogList().GetLastConfirmedLock();
        if (lastConfirmedLock == null)
          return (PipelineInfo.Alert) null;
        Hashtable lockRequestSnapshot = lastConfirmedLock.GetLockRequestSnapshot();
        bool flag1 = lockRequestSnapshot.Contains((object) "CASASRN.X141") && lockRequestSnapshot[(object) "CASASRN.X141"].ToString().Equals("Borrower");
        foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) definition.TriggerFields)
        {
          AlertTriggerField field = triggerField;
          int result = -1;
          bool flag2 = int.TryParse(field.FieldID, out result);
          string key;
          int num;
          if (LockRequestLog.RequestFieldMap.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (f => f.Value == field.FieldID)))
            key = LockRequestLog.RequestFieldMap.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (f => f.Value == field.FieldID)).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (f => f.Key)).First<string>();
          else if (flag2 && result >= 4535 && result <= 4546)
          {
            string str;
            if (flag1)
            {
              str = (string) null;
            }
            else
            {
              num = 4633 + (result - 4535);
              str = num.ToString();
            }
            key = str;
          }
          else if (flag2 && result >= 1269 && result <= 1274)
          {
            string str;
            if (!flag1)
            {
              str = (string) null;
            }
            else
            {
              num = 4633 + (result - 1269);
              str = num.ToString();
            }
            key = str;
          }
          else if (flag2 && result >= 1613 && result <= 1618)
          {
            string str;
            if (!flag1)
            {
              str = (string) null;
            }
            else
            {
              num = 4639 + (result - 1613);
              str = num.ToString();
            }
            key = str;
          }
          else
            key = field.FieldID;
          if (key != null)
            dictionary.Add(field.FieldID, lockRequestSnapshot.Contains((object) key) ? lockRequestSnapshot[(object) key].ToString() : "");
        }
        if (alertConfig.TriggerFieldList != null)
        {
          foreach (string triggerField in alertConfig.TriggerFieldList)
          {
            string fieldId = triggerField;
            string key = !LockRequestLog.RequestFieldMap.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (f => f.Value == fieldId)) ? fieldId : LockRequestLog.RequestFieldMap.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (f => f.Value == fieldId)).Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (f => f.Key)).First<string>();
            dictionary.Add(fieldId, lockRequestSnapshot.Contains((object) ("CLRFIELD." + key)) ? lockRequestSnapshot[(object) ("CLRFIELD." + key)].ToString() : (lockRequestSnapshot.Contains((object) key) ? lockRequestSnapshot[(object) key].ToString() : ""));
          }
        }
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          if (loan.GetField(keyValuePair.Key) != keyValuePair.Value)
            return new PipelineInfo.Alert(43, "", "", DateTime.Now, (string) null, (string) null);
        }
      }
      return (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetInitialDisclosureAlert(LoanData loan)
    {
      if (loan.IsAdverse())
        return (PipelineInfo.Alert) null;
      if (loan.GetField("1172") == "HELOC")
        return (PipelineInfo.Alert) null;
      DateTime date = Utils.ParseDate((object) loan.GetField("3143"), false);
      if (date == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      DisclosureTrackingLog.DisclosureTrackingType requiredDisclosures = loan.GetRequiredDisclosures();
      bool flag1 = true;
      bool flag2 = true;
      if ((requiredDisclosures & DisclosureTrackingLog.DisclosureTrackingType.GFE) != DisclosureTrackingLog.DisclosureTrackingType.None)
        flag1 &= Utils.IsDate((object) loan.GetSimpleField("3148"));
      if ((requiredDisclosures & DisclosureTrackingLog.DisclosureTrackingType.TIL) != DisclosureTrackingLog.DisclosureTrackingType.None)
        flag1 &= Utils.IsDate((object) loan.GetSimpleField("3152"));
      if (Utils.CheckIf2015RespaTila(loan.GetField("3969")))
      {
        flag2 &= Utils.IsDate((object) loan.GetSimpleField("3152"));
        if (loan.ISLESectionCPopulated())
          flag2 &= Utils.IsDate((object) loan.GetSimpleField("4014"));
      }
      if (flag1 && loan.GetField("3969") == "RESPA 2010 GFE and HUD-1")
        return (PipelineInfo.Alert) null;
      return flag2 && Utils.CheckIf2015RespaTila(loan.GetField("3969")) ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(22, "", "", date, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetThreeDayDisclosureRequirementsAlert(LoanData loan)
    {
      if (!loan.Use2020URLA)
        return (PipelineInfo.Alert) null;
      DateTime date = Utils.ParseDate((object) loan.GetField("3142"), false);
      if (date == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      return !Utils.IsEmptyDate((object) loan.GetField("3152")) ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(72, "", "", date, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetAtAppDisclosureRequirements(LoanData loan)
    {
      if (!loan.Use2020URLA)
        return (PipelineInfo.Alert) null;
      DateTime date = Utils.ParseDate((object) loan.GetField("HMDA.X29"), false);
      if (date == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      return !Utils.IsEmptyDate((object) loan.GetField("3152")) ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(73, "", "", date, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseCdAfterVestingNboAddedAlert(LoanData loan)
    {
      if (loan.GetField("NewVestingNboAlert") == "N")
        return (PipelineInfo.Alert) null;
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = loan.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      if (idisclosureTracking2015Log == null || idisclosureTracking2015Log.Length == 0)
      {
        loan.SetCurrentField("NewVestingNboAlert", "");
        return (PipelineInfo.Alert) null;
      }
      IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) null;
      for (int index = 0; index < idisclosureTracking2015Log.Length; ++index)
      {
        if (!(idisclosureTracking2015Log[index].BorrowerPairID != loan.CurrentBorrowerPair.Id) && disclosureTracking2015Log == null)
        {
          disclosureTracking2015Log = idisclosureTracking2015Log[index];
          break;
        }
      }
      if (disclosureTracking2015Log == null)
      {
        loan.SetCurrentField("NewVestingNboAlert", "");
        return (PipelineInfo.Alert) null;
      }
      VestingPartyFields[] vestingPartyFields = loan.GetVestingPartyFields(false);
      if (vestingPartyFields == null || vestingPartyFields.Length < 1)
      {
        loan.SetCurrentField("NewVestingNboAlert", "");
        return (PipelineInfo.Alert) null;
      }
      if (disclosureTracking2015Log.NonBorrowerOwnerCollections.Count < vestingPartyFields.Length)
      {
        loan.SetCurrentField("NewVestingNboAlert", "Y");
        return new PipelineInfo.Alert(71, "", "", DateTime.Today, (string) null, (string) null);
      }
      loan.SetCurrentField("NewVestingNboAlert", "");
      return (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GeteSignConsentAlert(LoanData loan)
    {
      if (!Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      if (loan.GetField("4072") == "Y")
        return (PipelineInfo.Alert) null;
      bool alertSetting1 = (bool) loan.Settings.AlertSettings[(object) "Alert.USEAPPLICATIONDATE"];
      bool alertSetting2 = (bool) loan.Settings.AlertSettings[(object) "Alert.DONOTSHOWDECLINECONSENT"];
      DateTime dateTime = new DateTime();
      if (loan.IsAdverse())
        return (PipelineInfo.Alert) null;
      if (Utils.ParseDate((object) loan.GetField("3142")) == DateTime.MinValue & alertSetting1)
        return (PipelineInfo.Alert) null;
      if (Utils.ParseDate((object) loan.GetField("MS.START")) == DateTime.MinValue && !alertSetting1)
        return (PipelineInfo.Alert) null;
      DateTime date = !alertSetting1 ? Utils.ParseDate((object) loan.GetField("MS.START")) : Utils.ParseDate((object) loan.GetField("3142"));
      string[] strArray1 = new string[12]
      {
        "3985",
        "3989",
        "3993",
        "3997",
        "4024",
        "4028",
        "4032",
        "4036",
        "4040",
        "4044",
        "4048",
        "4052"
      };
      string[] strArray2 = new string[12]
      {
        "3984",
        "3988",
        "3992",
        "3996",
        "4023",
        "4027",
        "4031",
        "4035",
        "4039",
        "4043",
        "4047",
        "4051"
      };
      List<int> linkedVestingIdxList = loan.GetNBOLinkedVestingIdxList();
      int index = -2;
      if (!alertSetting2)
      {
        foreach (BorrowerPair borrowerPair in loan.GetBorrowerPairs())
        {
          index += 2;
          if (!(loan.GetField(strArray2[index]) == "Accepted") || (borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName).Trim() != "" && !(loan.GetField(strArray2[index + 1]) == "Accepted"))
            return new PipelineInfo.Alert(51, "", "", date, (string) null, (string) null);
        }
        foreach (int num in linkedVestingIdxList)
        {
          string id = "NBOC" + num.ToString("00") + "17";
          if (!(loan.GetField(id) == "Accepted"))
            return new PipelineInfo.Alert(51, "", "", date, (string) null, (string) null);
        }
      }
      else
      {
        foreach (BorrowerPair borrowerPair in loan.GetBorrowerPairs())
        {
          index += 2;
          if (!(loan.GetField(strArray2[index]) == "Accepted") && !(loan.GetField(strArray2[index]) == "Rejected") || (borrowerPair.CoBorrower.FirstName + " " + borrowerPair.CoBorrower.LastName).Trim() != "" && !(loan.GetField(strArray2[index + 1]) == "Accepted") && !(loan.GetField(strArray2[index + 1]) == "Rejected"))
            return new PipelineInfo.Alert(51, "", "", date, (string) null, (string) null);
        }
        foreach (int num in linkedVestingIdxList)
        {
          string id = "NBOC" + num.ToString("00") + "17";
          string field = loan.GetField(id);
          if (!(field == "Accepted") && !(field == "Rejected"))
            return new PipelineInfo.Alert(51, "", "", date, (string) null, (string) null);
        }
      }
      return (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetGFEExpiresAlert(LoanData loan)
    {
      if (loan.GetField("NEWHUD.X354") != "Y")
        return (PipelineInfo.Alert) null;
      if (Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      string field = loan.GetField("3140");
      if (!Utils.IsDate((object) field))
        return (PipelineInfo.Alert) null;
      return loan.GetField("2400") == "Y" ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(23, "", "", Utils.ParseDate((object) field), (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetLEExpiresAlert(LoanData loan)
    {
      if (loan.GetField("1172") == "HELOC")
        return (PipelineInfo.Alert) null;
      if (loan.GetField("NEWHUD.X1139") != "Y")
        return (PipelineInfo.Alert) null;
      string field = loan.GetField("LE1.X28");
      if (!Utils.IsDate((object) field))
        return (PipelineInfo.Alert) null;
      if (Convert.ToDateTime(field).Date > DateTime.Now.Date)
        return (PipelineInfo.Alert) null;
      return loan.GetField("3164") == "Y" ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(45, "", "", Utils.ParseDate((object) field), (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseGFERateLockAlert(LoanData loan)
    {
      if (loan.GetField("NEWHUD.X354") != "Y")
        return (PipelineInfo.Alert) null;
      if (loan.GetField("3969") != "RESPA 2010 GFE and HUD-1")
        return (PipelineInfo.Alert) null;
      string field = loan.GetField("3201");
      if (loan.GetField("3201") == "N")
        return (PipelineInfo.Alert) null;
      DateTime date1 = Utils.ParseDate((object) loan.GetSimpleField("761"));
      if (date1 == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      if (field == "")
      {
        DateTime date2 = Utils.ParseDate((object) loan.GetSimpleField("3137"));
        if (date2 == DateTime.MinValue || date2.Date >= date1.Date)
          return (PipelineInfo.Alert) null;
      }
      return new PipelineInfo.Alert(24, "", "", date1, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseCDRateLockAlert(LoanData loan)
    {
      if (loan.GetField("3969") == "RESPA 2010 GFE and HUD-1")
        return (PipelineInfo.Alert) null;
      if (loan.GetField("NEWHUD.X1139") != "Y")
        return (PipelineInfo.Alert) null;
      string field = loan.GetField("3201");
      if (loan.GetField("3201") == "N")
        return (PipelineInfo.Alert) null;
      DateTime date1 = Utils.ParseDate((object) loan.GetSimpleField("761"));
      if (date1 == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      if (Utils.ParseDate((object) loan.GetSimpleField("CD1.X47")) == DateTime.MinValue || !Utils.IsDate((object) loan.GetField("CD1.X47")))
        return (PipelineInfo.Alert) null;
      if (field == "")
      {
        DateTime date2 = Utils.ParseDate((object) loan.GetSimpleField("CD1.X47"));
        if (date2 == DateTime.MinValue || date2.Date >= date1.Date)
          return (PipelineInfo.Alert) null;
      }
      return new PipelineInfo.Alert(55, "", "", date1, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseLERateLockAlert(LoanData loan)
    {
      if (loan.GetField("3969") == "RESPA 2010 GFE and HUD-1")
        return (PipelineInfo.Alert) null;
      if (loan.GetField("NEWHUD.X1139") != "Y")
        return (PipelineInfo.Alert) null;
      string field = loan.GetField("3201");
      if (loan.GetField("3201") == "N")
        return (PipelineInfo.Alert) null;
      if (Utils.IsDate((object) loan.GetField("CD1.X1")) || Utils.ParseDate((object) loan.GetField("CD1.X1")) != DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      DateTime date1 = Utils.ParseDate((object) loan.GetSimpleField("761"));
      if (date1 == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      if (field == "")
      {
        DateTime date2 = Utils.ParseDate((object) loan.GetSimpleField("LE1.X33"));
        if (date2 == DateTime.MinValue || date2.Date >= date1.Date)
          return (PipelineInfo.Alert) null;
      }
      return new PipelineInfo.Alert(47, "", "", date1, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseGFEChangedCircumstanceAlert(LoanData loan)
    {
      if (Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      if (loan.GetField("3168") != "Y")
        return (PipelineInfo.Alert) null;
      DateTime date = Utils.ParseDate((object) loan.GetField("3167"));
      return date == DateTime.MinValue ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(27, "", "", date, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseCDChangedCircumstanceAlert(LoanData loan)
    {
      if (!Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      bool flag = true;
      if ((loan.GetField("CD1.X61") != "Y" || loan.GetField("CD1.X57") != "Y" && loan.GetField("CD1.X55") != "Y" && loan.GetField("CD1.X58") != "Y" && loan.GetField("CD1.X59") != "Y" && loan.GetField("CD1.X68") != "Y" && loan.GetField("CD1.X66") != "Y" && loan.GetField("CD1.X67") != "Y") && flag)
        return (PipelineInfo.Alert) null;
      DateTime date = Utils.ParseDate((object) loan.GetField("CD1.X63"));
      return date == DateTime.MinValue ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(48, "", "", date, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetWithdrawnLoanAlert(LoanData loan)
    {
      DateTime date = Utils.ParseDate((object) loan.GetSimpleField("4120"));
      return date == DateTime.MinValue ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(58, "Withdrawn loan", "", date, 58.ToString(), (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseLEChangedCircumstanceAlert(LoanData loan)
    {
      if (loan.GetField("3969") == "RESPA 2010 GFE and HUD-1")
        return (PipelineInfo.Alert) null;
      if (loan.GetField("3168") != "Y")
        return (PipelineInfo.Alert) null;
      DateTime date = Utils.ParseDate((object) loan.GetField("3167"));
      return date == DateTime.MinValue ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(46, "", "", date, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetHUDToleranceViolationAlert(LoanData loan)
    {
      if (loan.GetField("NEWHUD.X354") != "Y")
        return (PipelineInfo.Alert) null;
      if (Utils.ParseDate((object) loan.GetField("3137")) == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      if (Utils.IsDate((object) loan.GetField("3171")))
      {
        if (loan.GetField("NewHud.CuredX12") == "" && loan.GetField("NewHud.CuredX13") == "" && loan.GetField("NewHud.CuredX16") == "" && loan.GetField("NewHud.CuredX76") == "" && loan.GetField("NewHud.CuredX312") == "")
          return (PipelineInfo.Alert) null;
        if (loan.GetField("NewHud.CuredX12") == loan.GetField("NewHud.X12") && loan.GetField("NewHud.CuredX13") == loan.GetField("NewHud.X13") && loan.GetField("NewHud.CuredX16") == loan.GetField("NewHud.X16") && loan.GetField("NewHud.CuredX76") == loan.GetField("NewHud.X76") && loan.GetField("NewHud.CuredX312") == loan.GetField("NewHud.X312"))
          return (PipelineInfo.Alert) null;
      }
      return loan.GetField("3160") != "Y" ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(25, "", "", DateTime.Today, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetGoodFaithFeeVarianceViolationAlert(LoanData loan)
    {
      if (!Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      return Utils.ParseDouble((object) loan.GetField("FV.X348")) > 0.0 ? new PipelineInfo.Alert(50, "", "", DateTime.Today, (string) null, (string) null) : (PipelineInfo.Alert) null;
    }

    public static PipelineInfo.Alert GetClosingDateViolationAlert(LoanData loan)
    {
      if (!(loan.GetField("NEWHUD.X354") == "Y") && !Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      DateTime date1 = Utils.ParseDate((object) loan.GetField("763"));
      if (date1 == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      DateTime date2 = Utils.ParseDate((object) loan.GetField("3147"));
      if (date2 == DateTime.MinValue)
        return (PipelineInfo.Alert) null;
      return date1.Date >= date2.Date ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(26, "", "", DateTime.Today, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseTILAlertRateChange(LoanData loan)
    {
      double disclosedAPR = Utils.ParseDouble((object) loan.GetField("3121"));
      double currentAPR = Utils.ParseDouble((object) loan.GetField("799"));
      return RegulationAlerts.GetRediscloseTILAlertRateChange(loan, currentAPR, disclosedAPR);
    }

    public static PipelineInfo.Alert GetRediscloseTILAlertRateChange(
      LoanData loan,
      double currentAPR,
      double disclosedAPR)
    {
      if (disclosedAPR == 0.0)
        return (PipelineInfo.Alert) null;
      if (Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      double num1 = 0.125;
      bool complianceSetting1 = (bool) loan.Settings.ComplianceSettings[(object) "Compliance.ApplyFixedAPRToleranceToARMs"];
      string field = loan.GetField("19");
      if (!complianceSetting1 && (loan.GetField("608") == "AdjustableRate" || field == "ConstructionOnly" || field == "ConstructionToPermanent"))
        num1 = 0.25;
      bool complianceSetting2 = (bool) loan.Settings.ComplianceSettings[(object) "Compliance.SuppressNegativeAPRAlert"];
      double num2 = Utils.ArithmeticRounding(currentAPR - disclosedAPR, 3);
      if (complianceSetting2 && num2 <= num1)
        return (PipelineInfo.Alert) null;
      return !complianceSetting2 && Math.Abs(num2) <= num1 ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(21, "", "", DateTime.Today, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetRediscloseCDAPR_Product_Prepay(LoanData loan)
    {
      double disclosedAPR = Utils.ParseDouble((object) loan.GetField("3121"));
      double currentAPR = Utils.ParseDouble((object) loan.GetField("799"));
      return RegulationAlerts.GetRediscloseCDAPR_Product_Prepay(loan, currentAPR, disclosedAPR);
    }

    public static PipelineInfo.Alert GetRediscloseCDAPR_Product_Prepay(
      LoanData loan,
      double currentAPR,
      double disclosedAPR)
    {
      if (!Utils.CheckIf2015RespaTila(loan.GetField("3969")))
        return (PipelineInfo.Alert) null;
      bool flag1 = true;
      bool flag2 = !(loan.GetField("4018") == "N") || !(loan.GetField("675") == "Y") ? flag1 : !loan.IsLocked("4018") && !Utils.IsDate((object) loan.GetField("CD1.X47")) && flag1;
      bool flag3 = loan.GetField("4017") == loan.GetField("LE1.X5") || loan.GetField("4017") == string.Empty ? flag2 : !loan.IsLocked("4017") && !Utils.IsDate((object) loan.GetField("CD1.X47")) && flag2;
      bool flag4;
      if (disclosedAPR == 0.0)
      {
        flag4 = flag3;
      }
      else
      {
        double num1 = 0.125;
        bool complianceSetting1 = (bool) loan.Settings.ComplianceSettings[(object) "Compliance.ApplyFixedAPRToleranceToARMs2015"];
        string field = loan.GetField("19");
        if (!complianceSetting1 && (loan.GetField("608") == "AdjustableRate" || field == "ConstructionOnly" || field == "ConstructionToPermanent"))
          num1 = 0.25;
        bool complianceSetting2 = (bool) loan.Settings.ComplianceSettings[(object) "Compliance.SuppressNegativeAPRAlert2015"];
        double num2 = Utils.ArithmeticRounding(currentAPR - disclosedAPR, 3);
        flag4 = !complianceSetting2 || num2 > num1 ? (complianceSetting2 || Math.Abs(num2) > num1 ? !Utils.IsDate((object) loan.GetField("CD1.X47")) && flag3 : flag3) : flag3;
      }
      return flag4 ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(49, "", "", DateTime.Today, (string) null, (string) null);
    }

    public static PipelineInfo.Alert GetPositiveAggregateEscrowAdjustment(LoanData loan)
    {
      return Utils.ParseDouble((object) loan.GetField("558")) <= 0.0 || loan.IsLocked("558") ? (PipelineInfo.Alert) null : new PipelineInfo.Alert(63, "", "", DateTime.Today, (string) null, (string) null);
    }
  }
}
