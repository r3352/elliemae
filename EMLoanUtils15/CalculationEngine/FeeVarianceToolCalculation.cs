// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.FeeVarianceToolCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class FeeVarianceToolCalculation : CalculationBase
  {
    private const string className = "FeeVarianceToolCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private IDisclosureTracking2015Log LEInitial;
    private IDisclosureTracking2015Log CDInitial;
    private IDisclosureTracking2015Log LELatest;
    private IDisclosureTracking2015Log CDLatest;
    private IDisclosureTracking2015Log LEBaseline_CannotDecrease;
    private IDisclosureTracking2015Log CDBaseline_CannotDecrease;
    private IDisclosureTracking2015Log LEBaseline_CannotIncrease;
    private IDisclosureTracking2015Log CDBaseline_CannotIncrease;
    private IDisclosureTracking2015Log LEBaseline_CannotIncrease10;
    private IDisclosureTracking2015Log CDBaseline_CannotIncrease10;
    private IDisclosureTracking2015Log CD_Recent_AppliedCure;
    private string cannotDecreaseVarianceFieldIDs = "";
    private string cannotIncreaseVarianceFieldIDs = "";
    private string cannotIncrease10VarianceFieldIDs = "";
    private bool leBaselineUsed_CannotDecrease;
    private bool leBaselineUsed_CannotIncrease;
    private bool leBaselineUsed_CannotIncrease10;
    private Dictionary<string, string> listValues = new Dictionary<string, string>();
    private BusinessCalendar businessCalendar;
    private DateTime issueDate;
    private DateTime acceptableClosingTime;
    private DateTime closingDate;
    private DateTime cdRebaseDate = Utils.ParseDate((object) "06/01/2018");
    private bool useNewCDRebaseline;
    private Hashtable allLogsWithAlertCoCs = new Hashtable();
    private bool replaceLEFee_Cannot_Descrease = true;
    private bool replaceLEFee_Cannot_Increase = true;
    private bool replaceLEFee_Cannot_Increase10 = true;
    private bool replaceCDFee_Cannot_Descrease = true;
    private bool replaceCDFee_Cannot_Increase = true;
    private bool replaceCDFee_Cannot_Increase10 = true;

    public FeeVarianceToolCalculation(
      SessionObjects sessionObjects,
      ILoanConfigurationInfo configInfo,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs,
      bool skipLogCalcs = false)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      object policySetting = this.sessionObjects.StartupInfo.PolicySettings[(object) "Policies.DefaulttoUseCDRebaselineDate"];
      if (policySetting != null)
        this.cdRebaseDate = Utils.ParseDate((object) policySetting.ToString());
      this.updateDictionaryFromLoan();
      if (skipLogCalcs && sessionObjects.StartupInfo.SkipLogInitializationForLoanCalculatorClone)
        return;
      this.UpdateLogs();
    }

    public override void FormCal(string id, string val) => this.calculateFeeVariance(true);

    private void addFieldHandlers()
    {
    }

    private void readLogsfromField()
    {
      this.LEInitial = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X356"]);
      this.LELatest = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X357"]);
      this.CDInitial = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X358"]);
      this.CDLatest = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X359"]);
      this.LEBaseline_CannotDecrease = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X360"]);
      this.CDBaseline_CannotDecrease = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X361"]);
      this.LEBaseline_CannotIncrease = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X362"]);
      this.CDBaseline_CannotIncrease = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X363"]);
      this.LEBaseline_CannotIncrease10 = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X364"]);
      this.CDBaseline_CannotIncrease10 = this.loan.GetLogList().GetIDisclosureTracking2015LogByGUID(this.listValues["FV.X365"]);
    }

    public void UpdateLogs()
    {
      this.LEInitial = this.CDInitial = this.LELatest = this.CDLatest = this.LEBaseline_CannotDecrease = this.CDBaseline_CannotDecrease = this.LEBaseline_CannotIncrease = this.CDBaseline_CannotIncrease = this.LEBaseline_CannotIncrease10 = this.CDBaseline_CannotIncrease10 = (IDisclosureTracking2015Log) null;
      DateTime date = Utils.ParseDate((object) this.Val("748"));
      this.useNewCDRebaseline = date == DateTime.MinValue || date.Date >= this.cdRebaseDate.Date;
      this.UpdateLELog();
      this.UpdateCDLog();
      if (this.CDInitial != null)
      {
        this.businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
        this.issueDate = Utils.ParseDate((object) this.CDInitial.GetDisclosedField("CD1.X1"));
        if (this.issueDate != DateTime.MinValue)
          this.acceptableClosingTime = this.businessCalendar.AddBusinessDays(this.issueDate, 7, true);
        this.closingDate = Utils.ParseDate((object) this.CDInitial.GetDisclosedField("748"));
      }
      this.resetInvalidFeeReplacementTriggers();
      this.getLEandCDBaselineCannotDecrease();
      this.getLEandCDBaselineCannotIncrease();
      this.getLEandCDBaselineCannotIncrease10();
      this.updateAppliedCureAmount();
      this.replaceFeesFromInvalidReason();
    }

    public void CalculateFeeVariance() => this.calculateFeeVariance(this.needToUpdateLogs());

    private void calculateFeeVariance(bool updateLogIsRequired)
    {
      if (updateLogIsRequired)
        this.UpdateLogs();
      this.readDTLogs();
      this.listValues["FV.X356"] = this.LEInitial != null ? this.LEInitial.Guid : "";
      this.listValues["FV.X357"] = this.LELatest != null ? this.LELatest.Guid : "";
      this.listValues["FV.X358"] = this.CDInitial != null ? this.CDInitial.Guid : "";
      this.listValues["FV.X359"] = this.CDLatest != null ? this.CDLatest.Guid : "";
      this.listValues["FV.X360"] = this.LEBaseline_CannotDecrease != null ? this.LEBaseline_CannotDecrease.Guid : "";
      this.listValues["FV.X361"] = this.CDBaseline_CannotDecrease != null ? this.CDBaseline_CannotDecrease.Guid : "";
      this.listValues["FV.X362"] = this.LEBaseline_CannotIncrease != null ? this.LEBaseline_CannotIncrease.Guid : "";
      this.listValues["FV.X363"] = this.CDBaseline_CannotIncrease != null ? this.CDBaseline_CannotIncrease.Guid : "";
      this.listValues["FV.X364"] = this.LEBaseline_CannotIncrease10 != null ? this.LEBaseline_CannotIncrease10.Guid : "";
      this.listValues["FV.X365"] = this.CDBaseline_CannotIncrease10 != null ? this.CDBaseline_CannotIncrease10.Guid : "";
      this.listValues["FV.X374"] = this.LEBaselineUsed_CannotDecrease ? "Y" : "N";
      this.listValues["FV.X375"] = this.LEBaselineUsed_CannotIncrease ? "Y" : "N";
      this.listValues["FV.X376"] = this.LEBaselineUsed_CannotIncrease10 ? "Y" : "N";
      this.updatelLoanField();
    }

    private void readDTLogs()
    {
      if (this.LEInitial == null)
      {
        for (int index = 1; index <= 366; ++index)
        {
          if (index != 348 || !this.loan.IsLocked("FV.X348"))
            this.listValues["FV.X" + (object) index] = "";
        }
        for (int index = 370; index <= 376; ++index)
          this.listValues["FV.X" + (object) index] = "";
        for (int index = 377; index <= 384; ++index)
        {
          if (index == 379 || index == 380 || index == 383 || index == 384)
            this.listValues["FV.X" + (object) index] = this.loan.GetField("FV.X" + (object) index);
          else
            this.listValues["FV.X" + (object) index] = "";
        }
        this.listValues["FV.X388"] = "";
      }
      else
      {
        this.cannotDecreaseVarianceFieldIDs = this.cannotIncrease10VarianceFieldIDs = this.cannotIncreaseVarianceFieldIDs = "";
        this.cal_ItemsThatCannotDecrease("", "");
        this.cal_ChargesThatCannotIncrease("", "");
        this.cal_ChargesThatCannotIncreaseMoreThan10("", "");
        this.cal_ChargesThatCanChange("", "");
        this.cal_TotalGoodFaithAmount("", "");
        this.calculateToleranceCure();
      }
    }

    private bool needToUpdateLogs()
    {
      return this.LEInitial == null && this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.LE, DisclosureTracking2015Log.DisclosureTypeEnum.Initial) != null || this.LEInitial != null && this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.LE, DisclosureTracking2015Log.DisclosureTypeEnum.Initial) == null || this.LELatest == null && this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE) != null || this.LELatest != null && this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE) == null || this.CDInitial == null && this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial) != null || this.CDInitial != null && this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial) == null || this.CDLatest == null && this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD) != null || this.CDLatest != null && this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD) == null;
    }

    public void UpdateLELog()
    {
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && disclosureTracking2015Log.IntentToProceed)
        {
          this.LEInitial = disclosureTracking2015Log;
          break;
        }
      }
      if (this.LEInitial == null)
        this.LEInitial = this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.LE, DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
      this.LELatest = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
    }

    public void UpdateCDLog()
    {
      this.CDInitial = this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
      this.CDLatest = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.CD);
    }

    private void updatelLoanField()
    {
      foreach (KeyValuePair<string, string> listValue in this.listValues)
      {
        try
        {
          if (!(listValue.Key == "FV.X366"))
          {
            if (!(listValue.Key == "FV.X217"))
            {
              if (!(listValue.Key == "FV.X222"))
              {
                if (listValue.Key == "FV.X348")
                {
                  if (this.loan.IsLocked("FV.X348"))
                    continue;
                }
                if (!(listValue.Key == "FV.X379"))
                {
                  if (!(listValue.Key == "FV.X380"))
                  {
                    if (!(listValue.Key == "FV.X383"))
                    {
                      if (!(listValue.Key == "FV.X384"))
                        this.loan.SetField(listValue.Key, listValue.Value);
                    }
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void updateDictionaryFromLoan()
    {
      if (this.listValues.Count > 0)
        return;
      for (int index = 1; index <= 366; ++index)
        this.listValues["FV.X" + (object) index] = this.loan.GetField("FV.X" + (object) index);
      for (int index = 370; index <= 373; ++index)
        this.listValues["FV.X" + (object) index] = this.loan.GetField("FV.X" + (object) index);
      for (int index = 377; index <= 384; ++index)
        this.listValues["FV.X" + (object) index] = this.loan.GetField("FV.X" + (object) index);
      this.listValues["FV.X388"] = this.loan.GetField("FV.X388");
    }

    public void ReCal_ItemsThatCannotDecrease()
    {
      if (this.LEInitial == null)
        return;
      this.cal_ItemsThatCannotDecrease("", "");
      this.cal_TotalGoodFaithAmount("", "");
      this.calculateToleranceCure();
      this.updatelLoanField();
    }

    public void ReCal_ChargesThatCannotIncreaseMoreThan10()
    {
      this.cal_ChargesThatCannotIncreaseMoreThan10("", "");
      this.cal_TotalGoodFaithAmount("", "");
      this.calculateToleranceCure();
      this.updatelLoanField();
    }

    public void ReCal_ChargesThatCannotIncrease()
    {
      this.cal_ChargesThatCannotIncrease("", "");
      this.cal_TotalGoodFaithAmount("", "");
      this.calculateToleranceCure();
      this.updatelLoanField();
    }

    private void cal_ItemsThatCannotDecrease(string id, string val)
    {
      if (this.LEInitial != null)
      {
        this.listValues["FV.X1"] = this.LEInitial.DisclosedDate.ToString();
        this.listValues["FV.X4"] = (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD.X1144")) + this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD.X1146")) + this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD.X1148"))).ToString();
        Decimal num = 0M;
        this.listValues["FV.X8"] = (!(this.LEInitial.GetDisclosedField("4796") == "Y") ? num + (this.LEInitial.GetDisclosedField("202") == "LenderCredit" ? this.toDecimal(this.LEInitial.GetDisclosedField("141")) : 0M) + (this.LEInitial.GetDisclosedField("1091") == "LenderCredit" ? this.toDecimal(this.LEInitial.GetDisclosedField("1095")) : 0M) + (this.LEInitial.GetDisclosedField("1106") == "LenderCredit" ? this.toDecimal(this.LEInitial.GetDisclosedField("1115")) : 0M) + (this.LEInitial.GetDisclosedField("1646") == "LenderCredit" ? this.toDecimal(this.LEInitial.GetDisclosedField("1647")) : 0M) : this.toDecimal(this.LEInitial.GetDisclosedField("4794"))).ToString();
        this.listValues["FV.X12"] = this.calculateLenderPaidClosingClosts("LEInitial").ToString();
        this.listValues["FV.X16"] = (this.toDecimal(this.listValues["FV.X8"]) + Math.Max(this.toDecimal(this.listValues["FV.X4"]), this.toDecimal(this.listValues["FV.X12"])) + this.toDecimal(this.LEInitial.GetDisclosedField("FV.X396"))).ToString();
        this.listValues["FV.X20"] = this.LEInitial.GetDisclosedField("LE2.XLC");
      }
      if (this.LEBaseline_CannotDecrease != null)
      {
        this.listValues["FV.X2"] = this.LEBaseline_CannotDecrease.DisclosedDate.ToString();
        this.listValues["FV.X5"] = (this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("NEWHUD.X1144")) + this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("NEWHUD.X1146")) + this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("NEWHUD.X1148"))).ToString();
        Decimal num = 0M;
        this.listValues["FV.X9"] = (!(this.LEBaseline_CannotDecrease.GetDisclosedField("4796") == "Y") ? num + (this.LEBaseline_CannotDecrease.GetDisclosedField("202") == "LenderCredit" ? this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("141")) : 0M) + (this.LEBaseline_CannotDecrease.GetDisclosedField("1091") == "LenderCredit" ? this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("1095")) : 0M) + (this.LEBaseline_CannotDecrease.GetDisclosedField("1106") == "LenderCredit" ? this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("1115")) : 0M) + (this.LEBaseline_CannotDecrease.GetDisclosedField("1646") == "LenderCredit" ? this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("1647")) : 0M) : this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("4794"))).ToString();
        this.listValues["FV.X13"] = this.calculateLenderPaidClosingClosts("LELatest").ToString();
        this.listValues["FV.X17"] = (this.toDecimal(this.listValues["FV.X9"]) + Math.Max(this.toDecimal(this.listValues["FV.X5"]), this.toDecimal(this.listValues["FV.X13"])) + this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("FV.X396"))).ToString();
        this.listValues["FV.X21"] = this.LELatest.GetDisclosedField("LE2.XLC");
      }
      else
        this.listValues["FV.X17"] = "";
      Decimal num1;
      if (this.CDBaseline_CannotDecrease != null)
      {
        this.listValues["FV.X3"] = this.CDBaseline_CannotDecrease.DisclosedDate.ToString();
        this.listValues["FV.X6"] = (this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("NEWHUD.X1144")) + this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("NEWHUD.X1146")) + this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("NEWHUD.X1148"))).ToString();
        Decimal num2 = 0M;
        this.listValues["FV.X10"] = (!(this.CDBaseline_CannotDecrease.GetDisclosedField("4796") == "Y") ? num2 + (this.CDBaseline_CannotDecrease.GetDisclosedField("202") == "LenderCredit" ? this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("141")) : 0M) + (this.CDBaseline_CannotDecrease.GetDisclosedField("1091") == "LenderCredit" ? this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("1095")) : 0M) + (this.CDBaseline_CannotDecrease.GetDisclosedField("1106") == "LenderCredit" ? this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("1115")) : 0M) + (this.CDBaseline_CannotDecrease.GetDisclosedField("1646") == "LenderCredit" ? this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("1647")) : 0M) : this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("4794"))).ToString();
        this.listValues["FV.X14"] = this.calculateLenderPaidClosingClosts("CDLatest").ToString();
        this.listValues["FV.X388"] = this.CDBaseline_CannotDecrease.GetDisclosedField("FV.X366");
        Dictionary<string, string> listValues = this.listValues;
        num1 = this.toDecimal(this.listValues["FV.X10"]) + Math.Max(this.toDecimal(this.listValues["FV.X6"]), this.toDecimal(this.listValues["FV.X14"])) + this.toDecimal(this.CDBaseline_CannotDecrease.GetDisclosedField("FV.X396"));
        string str = num1.ToString();
        listValues["FV.X18"] = str;
      }
      else if (this.leBaselineUsed_CannotDecrease)
      {
        this.listValues["FV.X3"] = "";
        this.listValues["FV.X6"] = this.listValues["FV.X5"];
        this.listValues["FV.X10"] = this.listValues["FV.X9"];
        this.listValues["FV.X14"] = this.listValues["FV.X13"];
        this.listValues["FV.X18"] = this.listValues["FV.X17"];
      }
      else
        this.listValues["FV.X3"] = this.listValues["FV.X6"] = this.listValues["FV.X10"] = this.listValues["FV.X14"] = this.listValues["FV.X18"] = this.listValues["FV.X388"] = "";
      this.listValues["FV.X22"] = this.CDLatest == null ? "" : this.CDLatest.GetDisclosedField("CD2.XSTLC");
      Dictionary<string, string> listValues1 = this.listValues;
      num1 = this.toDecimal(this.loan.GetField("NEWHUD.X1144")) + this.toDecimal(this.loan.GetField("NEWHUD.X1146")) + this.toDecimal(this.loan.GetField("NEWHUD.X1148"));
      string str1 = num1.ToString();
      listValues1["FV.X7"] = str1;
      Decimal num3 = 0M;
      this.listValues["FV.X11"] = (!(this.loan.GetField("4796") == "Y") ? num3 + (this.loan.GetField("202") == "LenderCredit" ? this.toDecimal(this.loan.GetField("141")) : 0M) + (this.loan.GetField("1091") == "LenderCredit" ? this.toDecimal(this.loan.GetField("1095")) : 0M) + (this.loan.GetField("1106") == "LenderCredit" ? this.toDecimal(this.loan.GetField("1115")) : 0M) + (this.loan.GetField("1646") == "LenderCredit" ? this.toDecimal(this.loan.GetField("1647")) : 0M) : this.toDecimal(this.loan.GetField("4794"))).ToString();
      Dictionary<string, string> listValues2 = this.listValues;
      num1 = this.calculateLenderPaidClosingClosts("Itemization");
      string str2 = num1.ToString();
      listValues2["FV.X15"] = str2;
      this.listValues["FV.X366"] = this.loan.GetField("FV.X366");
      Dictionary<string, string> listValues3 = this.listValues;
      num1 = this.toDecimal(this.listValues["FV.X11"]) + Math.Max(this.toDecimal(this.listValues["FV.X7"]), this.toDecimal(this.listValues["FV.X15"])) + this.toDecimal(this.loan.GetField("FV.X396"));
      string str3 = num1.ToString();
      listValues3["FV.X19"] = str3;
      Dictionary<string, string> listValues4 = this.listValues;
      num1 = Math.Max(0M, (this.toDecimal(this.listValues["FV.X11"]) + this.toDecimal(this.listValues["FV.X7"]) + this.toDecimal(this.listValues["FV.X15"]) - this.toDecimal(this.listValues["FV.X9"]) - this.toDecimal(this.listValues["FV.X5"]) - this.toDecimal(this.listValues["FV.X13"])) * -1M);
      string str4 = num1.ToString();
      listValues4["FV.X24"] = str4;
      if (this.CDBaseline_CannotDecrease != null || this.leBaselineUsed_CannotDecrease)
      {
        Dictionary<string, string> listValues5 = this.listValues;
        num1 = Math.Max(0M, (this.toDecimal(this.listValues["FV.X22"]) - this.toDecimal(this.listValues["FV.X21"])) * -1M);
        string str5 = num1.ToString();
        listValues5["FV.X25"] = str5;
        Dictionary<string, string> listValues6 = this.listValues;
        num1 = Math.Max(0M, (this.toDecimal(this.listValues["FV.X11"]) + this.toDecimal(this.listValues["FV.X7"]) + this.toDecimal(this.listValues["FV.X15"]) - this.toDecimal(this.listValues["FV.X10"]) - this.toDecimal(this.listValues["FV.X6"]) - this.toDecimal(this.listValues["FV.X14"])) * -1M);
        string str6 = num1.ToString();
        listValues6["FV.X26"] = str6;
      }
      else
        this.listValues["FV.X25"] = this.listValues["FV.X26"] = "";
      if (this.CDBaseline_CannotDecrease != null)
      {
        this.cannotDecreaseVarianceFieldIDs = this.getVarianceLine(this.CDBaseline_CannotDecrease, "NEWHUD.X1144", true);
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotDecrease, "NEWHUD.X1146", true);
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotDecrease, "NEWHUD.X1148", true);
        if (this.CDBaseline_CannotDecrease.GetDisclosedField("4796") == "Y")
        {
          this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotDecrease, "4794", true);
        }
        else
        {
          this.cannotDecreaseVarianceFieldIDs += this.CDBaseline_CannotDecrease.GetDisclosedField("202") == "LenderCredit" ? this.getVarianceLine(this.CDBaseline_CannotDecrease, "141", true) : "";
          this.cannotDecreaseVarianceFieldIDs += this.CDBaseline_CannotDecrease.GetDisclosedField("1091") == "LenderCredit" ? this.getVarianceLine(this.CDBaseline_CannotDecrease, "1095", true) : "";
          this.cannotDecreaseVarianceFieldIDs += this.CDBaseline_CannotDecrease.GetDisclosedField("1106") == "LenderCredit" ? this.getVarianceLine(this.CDBaseline_CannotDecrease, "1115", true) : "";
          this.cannotDecreaseVarianceFieldIDs += this.CDBaseline_CannotDecrease.GetDisclosedField("1646") == "LenderCredit" ? this.getVarianceLine(this.CDBaseline_CannotDecrease, "1647", true) : "";
        }
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLinesLP(this.CDBaseline_CannotDecrease, 0, 137, true);
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotDecrease, "FV.X366", true);
      }
      else
      {
        if (this.LEBaseline_CannotDecrease == null)
          return;
        this.cannotDecreaseVarianceFieldIDs = this.getVarianceLine(this.LEBaseline_CannotDecrease, "NEWHUD.X1144", true);
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.LEBaseline_CannotDecrease, "NEWHUD.X1146", true);
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.LEBaseline_CannotDecrease, "NEWHUD.X1148", true);
        if (this.LEBaseline_CannotDecrease.GetDisclosedField("4796") == "Y")
        {
          this.cannotDecreaseVarianceFieldIDs += this.getVarianceLine(this.LEBaseline_CannotDecrease, "4794", true);
        }
        else
        {
          this.cannotDecreaseVarianceFieldIDs += this.LEBaseline_CannotDecrease.GetDisclosedField("202") == "LenderCredit" ? this.getVarianceLine(this.LEBaseline_CannotDecrease, "141", true) : "";
          this.cannotDecreaseVarianceFieldIDs += this.LEBaseline_CannotDecrease.GetDisclosedField("1091") == "LenderCredit" ? this.getVarianceLine(this.LEBaseline_CannotDecrease, "1095", true) : "";
          this.cannotDecreaseVarianceFieldIDs += this.LEBaseline_CannotDecrease.GetDisclosedField("1106") == "LenderCredit" ? this.getVarianceLine(this.LEBaseline_CannotDecrease, "1115", true) : "";
          this.cannotDecreaseVarianceFieldIDs += this.LEBaseline_CannotDecrease.GetDisclosedField("1646") == "LenderCredit" ? this.getVarianceLine(this.LEBaseline_CannotDecrease, "1647", true) : "";
        }
        this.cannotDecreaseVarianceFieldIDs += this.getVarianceLinesLP(this.LEBaseline_CannotDecrease, 0, 137, true);
      }
    }

    private void cal_ChargesThatCannotIncrease(string id, string val)
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      Decimal[] numArray = this.sectionBorCForCannotIncrease();
      Decimal[] taxesCannotIncrease = this.calculatePropertyTaxesCannotIncrease();
      DateTime disclosedDate;
      Decimal num;
      if (this.LEInitial != null)
      {
        Dictionary<string, string> listValues1 = this.listValues;
        disclosedDate = this.LEInitial.DisclosedDate;
        string str1 = disclosedDate.ToString();
        listValues1["FV.X27"] = str1;
        this.listValues["FV.X30"] = (this.getSumOfRange(this.LEInitial, 4, 21) + this.getSumOfRange(this.LEInitial, 23, 26)).ToString();
        this.listValues["FV.X34"] = numArray?[0].ToString() ?? "";
        this.listValues["FV.X38"] = numArray?[1].ToString() ?? "";
        this.listValues["FV.X42"] = (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3695")) + (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3728"))) + (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3761")))).ToString();
        this.listValues["FV.X370"] = taxesCannotIncrease[0].ToString();
        Dictionary<string, string> listValues2 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X30"]) + this.toDecimal(this.listValues["FV.X34"]) + this.toDecimal(this.listValues["FV.X38"]) + this.toDecimal(this.listValues["FV.X42"]) + this.toDecimal(this.listValues["FV.X370"]);
        string str2 = num.ToString();
        listValues2["FV.X46"] = str2;
      }
      if (this.LEBaseline_CannotIncrease != null)
      {
        Dictionary<string, string> listValues3 = this.listValues;
        disclosedDate = this.LEBaseline_CannotIncrease.DisclosedDate;
        string str3 = disclosedDate.ToString();
        listValues3["FV.X28"] = str3;
        Dictionary<string, string> listValues4 = this.listValues;
        num = this.getSumOfRange(this.LEBaseline_CannotIncrease, 4, 21) + this.getSumOfRange(this.LEBaseline_CannotIncrease, 23, 26);
        string str4 = num.ToString();
        listValues4["FV.X31"] = str4;
        this.listValues["FV.X35"] = numArray?[2].ToString() ?? "";
        this.listValues["FV.X39"] = numArray?[3].ToString() ?? "";
        Dictionary<string, string> listValues5 = this.listValues;
        num = this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3695")) + (this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3728"))) + (this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3761")));
        string str5 = num.ToString();
        listValues5["FV.X43"] = str5;
        this.listValues["FV.X371"] = taxesCannotIncrease[1].ToString();
        Dictionary<string, string> listValues6 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X31"]) + this.toDecimal(this.listValues["FV.X35"]) + this.toDecimal(this.listValues["FV.X39"]) + this.toDecimal(this.listValues["FV.X43"]) + this.toDecimal(this.listValues["FV.X371"]);
        string str6 = num.ToString();
        listValues6["FV.X377"] = str6;
        Dictionary<string, string> listValues7 = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X377"]) - this.toDecimal(this.loan.GetField("FV.X379")));
        string str7 = num.ToString();
        listValues7["FV.X47"] = str7;
      }
      else
        this.listValues["FV.X28"] = this.listValues["FV.X31"] = this.listValues["FV.X35"] = this.listValues["FV.X39"] = this.listValues["FV.X43"] = this.listValues["FV.X47"] = this.listValues["FV.X371"] = "";
      if (this.CDBaseline_CannotIncrease != null)
      {
        Dictionary<string, string> listValues8 = this.listValues;
        disclosedDate = this.CDBaseline_CannotIncrease.DisclosedDate;
        string str8 = disclosedDate.ToString();
        listValues8["FV.X29"] = str8;
        Dictionary<string, string> listValues9 = this.listValues;
        num = this.getSumOfRange(this.CDBaseline_CannotIncrease, 4, 21) + this.getSumOfRange(this.CDBaseline_CannotIncrease, 23, 26);
        string str9 = num.ToString();
        listValues9["FV.X32"] = str9;
        this.listValues["FV.X36"] = numArray?[4].ToString() ?? "";
        this.listValues["FV.X40"] = numArray?[5].ToString() ?? "";
        Dictionary<string, string> listValues10 = this.listValues;
        num = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3695")) + (this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3728"))) + (this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField("NEWHUD2.X3761")));
        string str10 = num.ToString();
        listValues10["FV.X44"] = str10;
        this.listValues["FV.X372"] = taxesCannotIncrease[2].ToString();
        Dictionary<string, string> listValues11 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X32"]) + this.toDecimal(this.listValues["FV.X36"]) + this.toDecimal(this.listValues["FV.X40"]) + this.toDecimal(this.listValues["FV.X44"]) + this.toDecimal(this.listValues["FV.X372"]);
        string str11 = num.ToString();
        listValues11["FV.X378"] = str11;
        Dictionary<string, string> listValues12 = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X378"]) - this.toDecimal(this.loan.GetField("FV.X380")));
        string str12 = num.ToString();
        listValues12["FV.X48"] = str12;
      }
      else if (this.leBaselineUsed_CannotIncrease)
      {
        this.listValues["FV.X29"] = "";
        this.listValues["FV.X32"] = this.listValues["FV.X31"];
        this.listValues["FV.X36"] = this.listValues["FV.X35"];
        this.listValues["FV.X40"] = this.listValues["FV.X39"];
        this.listValues["FV.X44"] = this.listValues["FV.X43"];
        this.listValues["FV.X372"] = this.listValues["FV.X371"];
        this.listValues["FV.X378"] = this.listValues["FV.X377"];
        Dictionary<string, string> listValues = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X378"]) - this.toDecimal(this.loan.GetField("FV.X380")));
        string str = num.ToString();
        listValues["FV.X48"] = str;
      }
      else
        this.listValues["FV.X29"] = this.listValues["FV.X32"] = this.listValues["FV.X36"] = this.listValues["FV.X40"] = this.listValues["FV.X44"] = this.listValues["FV.X48"] = this.listValues["FV.X372"] = this.listValues["FV.X378"] = this.listValues["FV.X380"] = "";
      Dictionary<string, string> listValues13 = this.listValues;
      num = this.getSumOfRange((IDisclosureTracking2015Log) null, 4, 21) + this.getSumOfRange((IDisclosureTracking2015Log) null, 23, 26);
      string str13 = num.ToString();
      listValues13["FV.X33"] = str13;
      this.listValues["FV.X37"] = numArray?[6].ToString() ?? "";
      this.listValues["FV.X41"] = numArray?[7].ToString() ?? "";
      Dictionary<string, string> listValues14 = this.listValues;
      num = this.toDecimal(this.loan.GetField("NEWHUD2.X3666")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3695")) + (this.toDecimal(this.loan.GetField("NEWHUD2.X3699")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3728"))) + (this.toDecimal(this.loan.GetField("NEWHUD2.X3732")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3761")));
      string str14 = num.ToString();
      listValues14["FV.X45"] = str14;
      this.listValues["FV.X373"] = taxesCannotIncrease[3].ToString();
      Dictionary<string, string> listValues15 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X33"]) + this.toDecimal(this.listValues["FV.X37"]) + this.toDecimal(this.listValues["FV.X41"]) + this.toDecimal(this.listValues["FV.X45"]) + this.toDecimal(this.listValues["FV.X373"]);
      string str15 = num.ToString();
      listValues15["FV.X49"] = str15;
      this.listValues["FV.X349"] = this.listValues["FV.X46"];
      if (this.LELatest != null && numArray != null)
      {
        Dictionary<string, string> listValues16 = this.listValues;
        num = this.getSumOfRange(this.LELatest, 4, 21) + this.getSumOfRange(this.LELatest, 23, 26) + numArray[10] + (this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X3695"))) + (this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X3728"))) + (this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X3761"))) + taxesCannotIncrease[6];
        string str16 = num.ToString();
        listValues16["FV.X350"] = str16;
      }
      else
        this.listValues["FV.X350"] = "";
      if (this.CDLatest != null && numArray != null)
      {
        Dictionary<string, string> listValues17 = this.listValues;
        num = this.getSumOfRange(this.CDLatest, 4, 21) + this.getSumOfRange(this.CDLatest, 23, 26) + numArray[11] + (this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X3695"))) + (this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X3728"))) + (this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X3761"))) + taxesCannotIncrease[7];
        string str17 = num.ToString();
        listValues17["FV.X351"] = str17;
      }
      else
        this.listValues["FV.X351"] = "";
      if (this.LEBaseline_CannotIncrease != null && numArray != null)
      {
        Dictionary<string, string> listValues18 = this.listValues;
        num = Math.Max(0M, this.getVariance(this.LEBaseline_CannotIncrease, 4, 21) + this.getVariance(this.LEBaseline_CannotIncrease, 23, 26) + numArray[8] + Math.Max(0M, this.toDecimal(this.listValues["FV.X45"]) - this.toDecimal(this.listValues["FV.X43"])) + taxesCannotIncrease[4] + this.toDecimal(this.loan.GetField("FV.X379")));
        string str18 = num.ToString();
        listValues18["FV.X50"] = str18;
      }
      if (this.CDLatest != null)
      {
        Dictionary<string, string> listValues19 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X351"]) - this.toDecimal(this.listValues["FV.X350"]);
        string str19 = num.ToString();
        listValues19["FV.X51"] = str19;
      }
      else
        this.listValues["FV.X51"] = "";
      if (this.CDBaseline_CannotIncrease != null && numArray != null)
      {
        Dictionary<string, string> listValues20 = this.listValues;
        num = Math.Max(0M, this.getVariance(this.CDBaseline_CannotIncrease, 4, 21) + this.getVariance(this.CDBaseline_CannotIncrease, 23, 26) + numArray[9] + Math.Max(0M, this.toDecimal(this.listValues["FV.X45"]) - this.toDecimal(this.listValues["FV.X44"])) + taxesCannotIncrease[5] + this.toDecimal(this.loan.GetField("FV.X380")));
        string str20 = num.ToString();
        listValues20["FV.X52"] = str20;
      }
      else if (this.leBaselineUsed_CannotIncrease && numArray != null)
      {
        Dictionary<string, string> listValues21 = this.listValues;
        num = Math.Max(0M, this.getVariance(this.LEBaseline_CannotIncrease, 4, 21) + this.getVariance(this.LEBaseline_CannotIncrease, 23, 26) + numArray[8] + Math.Max(0M, this.toDecimal(this.listValues["FV.X45"]) - this.toDecimal(this.listValues["FV.X43"])) + taxesCannotIncrease[4] + this.toDecimal(this.loan.GetField("FV.X380")));
        string str21 = num.ToString();
        listValues21["FV.X52"] = str21;
      }
      else
        this.listValues["FV.X52"] = "";
      if (this.CDBaseline_CannotIncrease != null)
      {
        this.cannotIncreaseVarianceFieldIDs = this.cannotIncreaseVarianceFieldIDs + this.getVarianceLines(this.CDBaseline_CannotIncrease, 4, 21) + this.getVarianceLines(this.CDBaseline_CannotIncrease, 23, 26);
        this.cannotIncreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotIncrease, "NEWHUD2.X3666", "NEWHUD2.X3695");
        this.cannotIncreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotIncrease, "NEWHUD2.X3699", "NEWHUD2.X3728");
        this.cannotIncreaseVarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotIncrease, "NEWHUD2.X3732", "NEWHUD2.X3761");
      }
      else
      {
        this.cannotIncreaseVarianceFieldIDs = this.cannotIncreaseVarianceFieldIDs + this.getVarianceLines(this.LEBaseline_CannotIncrease, 4, 21) + this.getVarianceLines(this.LEBaseline_CannotIncrease, 23, 26);
        this.cannotIncreaseVarianceFieldIDs += this.getVarianceLine(this.LEBaseline_CannotIncrease, "NEWHUD2.X3666", "NEWHUD2.X3695");
        this.cannotIncreaseVarianceFieldIDs += this.getVarianceLine(this.LEBaseline_CannotIncrease, "NEWHUD2.X3699", "NEWHUD2.X3728");
        this.cannotIncreaseVarianceFieldIDs += this.getVarianceLine(this.LEBaseline_CannotIncrease, "NEWHUD2.X3732", "NEWHUD2.X3761");
      }
    }

    private void cal_ChargesThatCannotIncreaseMoreThan10(string id, string val)
    {
      if (this.LEInitial != null)
      {
        this.listValues["FV.X53"] = this.LEInitial.DisclosedDate.ToString();
        this.listValues["FV.X56"] = (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3633")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3662")) + (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3765")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3794"))) + (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3798")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3827"))) + (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3831")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3860")))).ToString();
      }
      if (this.LEBaseline_CannotIncrease10 != null)
      {
        this.listValues["FV.X54"] = this.LEBaseline_CannotIncrease10.DisclosedDate.ToString();
        this.listValues["FV.X57"] = (this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3633")) - this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3662")) + (this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3765")) - this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3794"))) + (this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3798")) - this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3827"))) + (this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3831")) - this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3860")))).ToString();
      }
      if (this.CDBaseline_CannotIncrease10 != null)
      {
        this.listValues["FV.X55"] = this.CDBaseline_CannotIncrease10.DisclosedDate.ToString();
        this.listValues["FV.X58"] = (this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3633")) - this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3662")) + (this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3765")) - this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3794"))) + (this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3798")) - this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3827"))) + (this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3831")) - this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField("NEWHUD2.X3860")))).ToString();
      }
      else if (this.leBaselineUsed_CannotIncrease10)
      {
        this.listValues["FV.X55"] = "";
        this.listValues["FV.X58"] = this.listValues["FV.X57"];
      }
      else
        this.listValues["FV.X55"] = this.listValues["FV.X58"] = "";
      this.listValues["FV.X59"] = (this.toDecimal(this.loan.GetField("NEWHUD2.X3633")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3662")) + (this.toDecimal(this.loan.GetField("NEWHUD2.X3765")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3794"))) + (this.toDecimal(this.loan.GetField("NEWHUD2.X3798")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3827"))) + (this.toDecimal(this.loan.GetField("NEWHUD2.X3831")) - this.toDecimal(this.loan.GetField("NEWHUD2.X3860")))).ToString();
      this.calculateCannotIncreaseMoreThan10Lines();
      this.listValues["FV.X204"] = (this.toDecimal(this.listValues["FV.X204"]) + this.toDecimal(this.listValues["FV.X56"])).ToString();
      Decimal num;
      if (this.LEBaseline_CannotIncrease10 != null)
      {
        this.listValues["FV.X381"] = (this.toDecimal(this.listValues["FV.X205"]) + this.toDecimal(this.listValues["FV.X57"])).ToString();
        Dictionary<string, string> listValues = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X381"]) - this.toDecimal(this.loan.GetField("FV.X383")));
        string str = num.ToString();
        listValues["FV.X205"] = str;
      }
      if (this.CDBaseline_CannotIncrease10 != null)
      {
        Dictionary<string, string> listValues1 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X206"]) + this.toDecimal(this.listValues["FV.X58"]);
        string str1 = num.ToString();
        listValues1["FV.X382"] = str1;
        Dictionary<string, string> listValues2 = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X382"]) - this.toDecimal(this.loan.GetField("FV.X384")));
        string str2 = num.ToString();
        listValues2["FV.X206"] = str2;
      }
      else if (this.leBaselineUsed_CannotIncrease10)
      {
        this.listValues["FV.X382"] = this.listValues["FV.X381"];
        Dictionary<string, string> listValues = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X382"]) - this.toDecimal(this.loan.GetField("FV.X384")));
        string str = num.ToString();
        listValues["FV.X206"] = str;
      }
      else
      {
        this.listValues["FV.X382"] = "";
        this.listValues["FV.X206"] = "";
      }
      Dictionary<string, string> listValues3 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X207"]) + this.toDecimal(this.listValues["FV.X59"]);
      string str3 = num.ToString();
      listValues3["FV.X207"] = str3;
      Dictionary<string, string> listValues4 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X353"]) + this.toDecimal(this.listValues["FV.X56"]);
      string str4 = num.ToString();
      listValues4["FV.X353"] = str4;
      if (this.LELatest != null)
      {
        Dictionary<string, string> listValues5 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X354"]) + this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD.X607"));
        string str5 = num.ToString();
        listValues5["FV.X354"] = str5;
      }
      else
        this.listValues["FV.X354"] = "";
      if (this.CDLatest != null)
      {
        Dictionary<string, string> listValues6 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X355"]) + this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD.X607"));
        string str6 = num.ToString();
        listValues6["FV.X355"] = str6;
      }
      else
        this.listValues["FV.X355"] = "";
      Dictionary<string, string> listValues7 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X204"]) * this.toDecimal("1.1");
      string str7 = num.ToString();
      listValues7["FV.X211"] = str7;
      Dictionary<string, string> listValues8 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X205"]) * this.toDecimal("1.1");
      string str8 = num.ToString();
      listValues8["FV.X212"] = str8;
      if (this.listValues["FV.X206"] == "")
      {
        this.listValues["FV.X213"] = "";
      }
      else
      {
        Dictionary<string, string> listValues9 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X206"]) * this.toDecimal("1.1");
        string str9 = num.ToString();
        listValues9["FV.X213"] = str9;
      }
      this.listValues["FV.X352"] = this.CDBaseline_CannotIncrease10 == null ? (this.LEBaseline_CannotIncrease10 == null ? "" : this.listValues["FV.X212"]) : this.listValues["FV.X213"];
      Dictionary<string, string> listValues10 = this.listValues;
      num = Math.Max(0M, this.toDecimal(this.listValues["FV.X207"]) - this.toDecimal(this.listValues["FV.X212"]));
      string str10 = num.ToString();
      listValues10["FV.X208"] = str10;
      if (this.listValues["FV.X355"] == "" || this.listValues["FV.X354"] == "")
      {
        this.listValues["FV.X209"] = "";
      }
      else
      {
        Dictionary<string, string> listValues11 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X355"]) - this.toDecimal(this.listValues["FV.X354"]);
        string str11 = num.ToString();
        listValues11["FV.X209"] = str11;
      }
      if (this.CDBaseline_CannotIncrease10 != null)
      {
        Dictionary<string, string> listValues12 = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X207"]) - this.toDecimal(this.listValues["FV.X213"]));
        string str12 = num.ToString();
        listValues12["FV.X210"] = str12;
      }
      else if (this.leBaselineUsed_CannotIncrease10)
      {
        Dictionary<string, string> listValues13 = this.listValues;
        num = Math.Max(0M, this.toDecimal(this.listValues["FV.X207"]) - this.toDecimal(this.listValues["FV.X213"]));
        string str13 = num.ToString();
        listValues13["FV.X210"] = str13;
      }
      else
        this.listValues["FV.X210"] = "";
      if (this.CDBaseline_CannotIncrease10 != null)
      {
        this.cannotIncrease10VarianceFieldIDs += this.getVarianceLine(this.CDBaseline_CannotIncrease10, "NEWHUD.X607", percent: true);
      }
      else
      {
        this.cannotIncrease10VarianceFieldIDs += this.getVarianceLines(this.LEBaseline_CannotIncrease10, 110, 110, percent: true);
        this.cannotIncrease10VarianceFieldIDs += this.getVarianceLines(this.LEBaseline_CannotIncrease10, 114, 116, percent: true);
      }
    }

    private void cal_ChargesThatCanChange(string id, string val)
    {
      if (this.LEInitial != null)
      {
        this.listValues["FV.X214"] = this.LEInitial.DisclosedDate.ToString();
        this.listValues["FV.X218"] = (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD.X1719")) + this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD.X1713")) - this.toDecimal(this.LEInitial.GetDisclosedField("558"))).ToString();
        this.listValues["FV.X223"] = (this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X2148")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X2177"))).ToString();
      }
      if (this.LELatest != null)
      {
        this.listValues["FV.X215"] = this.LELatest.DisclosedDate.ToString();
        this.listValues["FV.X219"] = (this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD.X1719")) + this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD.X1713")) - this.toDecimal(this.LELatest.GetDisclosedField("558"))).ToString();
        this.listValues["FV.X224"] = (this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X2148")) - this.toDecimal(this.LELatest.GetDisclosedField("NEWHUD2.X2177"))).ToString();
      }
      else
        this.listValues["FV.X215"] = this.listValues["FV.X219"] = this.listValues["FV.X224"] = "";
      if (this.CDLatest != null)
      {
        this.listValues["FV.X216"] = this.CDLatest.DisclosedDate.ToString();
        this.listValues["FV.X220"] = (this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD.X1719")) + this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD.X1713")) - this.toDecimal(this.CDLatest.GetDisclosedField("558"))).ToString();
        this.listValues["FV.X225"] = (this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X2148")) - this.toDecimal(this.CDLatest.GetDisclosedField("NEWHUD2.X2177"))).ToString();
      }
      else
        this.listValues["FV.X216"] = this.listValues["FV.X220"] = this.listValues["FV.X225"] = "";
      this.listValues["FV.X221"] = (this.toDecimal(this.loan.GetField("NEWHUD.X1719")) + this.toDecimal(this.loan.GetField("NEWHUD.X1713")) - this.toDecimal(this.loan.GetField("558"))).ToString();
      this.listValues["FV.X226"] = (this.toDecimal(this.loan.GetField("NEWHUD2.X2148")) - this.toDecimal(this.loan.GetField("NEWHUD2.X2177"))).ToString();
      this.calculateCanChangeLines();
      Dictionary<string, string> listValues1 = this.listValues;
      Decimal num = this.toDecimal(this.listValues["FV.X317"]) + this.toDecimal(this.listValues["FV.X218"]) + this.toDecimal(this.listValues["FV.X223"]);
      string str1 = num.ToString();
      listValues1["FV.X317"] = str1;
      Dictionary<string, string> listValues2 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X318"]) + this.toDecimal(this.listValues["FV.X219"]) + this.toDecimal(this.listValues["FV.X224"]);
      string str2 = num.ToString();
      listValues2["FV.X318"] = str2;
      if (this.CDLatest != null)
      {
        Dictionary<string, string> listValues3 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X319"]) + this.toDecimal(this.listValues["FV.X220"]) + this.toDecimal(this.listValues["FV.X225"]);
        string str3 = num.ToString();
        listValues3["FV.X319"] = str3;
      }
      else
        this.listValues["FV.X319"] = "";
      Dictionary<string, string> listValues4 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X320"]) + this.toDecimal(this.listValues["FV.X221"]) + this.toDecimal(this.listValues["FV.X226"]);
      string str4 = num.ToString();
      listValues4["FV.X320"] = str4;
      Dictionary<string, string> listValues5 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X320"]) - this.toDecimal(this.listValues["FV.X318"]);
      string str5 = num.ToString();
      listValues5["FV.X321"] = str5;
      if (this.CDLatest != null)
      {
        Dictionary<string, string> listValues6 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X319"]) - this.toDecimal(this.listValues["FV.X318"]);
        string str6 = num.ToString();
        listValues6["FV.X322"] = str6;
        Dictionary<string, string> listValues7 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X320"]) - this.toDecimal(this.listValues["FV.X319"]);
        string str7 = num.ToString();
        listValues7["FV.X323"] = str7;
      }
      else
        this.listValues["FV.X322"] = this.listValues["FV.X323"] = "";
    }

    private void cal_TotalGoodFaithAmount(string id, string val)
    {
      if (this.LEInitial != null)
        this.listValues["FV.X324"] = this.LEInitial.DisclosedDate.ToString();
      this.listValues["FV.X325"] = this.listValues["FV.X16"];
      this.listValues["FV.X326"] = this.listValues["FV.X17"];
      this.listValues["FV.X327"] = this.listValues["FV.X18"];
      this.listValues["FV.X328"] = this.listValues["FV.X19"];
      this.listValues["FV.X329"] = this.listValues["FV.X46"];
      this.listValues["FV.X330"] = this.listValues["FV.X47"];
      this.listValues["FV.X331"] = this.listValues["FV.X48"];
      this.listValues["FV.X332"] = this.listValues["FV.X49"];
      this.listValues["FV.X333"] = this.listValues["FV.X204"];
      this.listValues["FV.X334"] = this.listValues["FV.X205"];
      this.listValues["FV.X335"] = this.listValues["FV.X206"];
      this.listValues["FV.X336"] = this.listValues["FV.X207"];
      this.listValues["FV.X337"] = (this.toDecimal(this.listValues["FV.X329"]) + this.toDecimal(this.listValues["FV.X333"])).ToString();
      this.listValues["FV.X338"] = (this.toDecimal(this.listValues["FV.X330"]) + this.toDecimal(this.listValues["FV.X334"])).ToString();
      Decimal num;
      if (this.listValues["FV.X331"] == "" && this.listValues["FV.X335"] == "")
      {
        this.listValues["FV.X339"] = "";
      }
      else
      {
        Dictionary<string, string> listValues = this.listValues;
        num = this.toDecimal(this.listValues["FV.X331"]) + this.toDecimal(this.listValues["FV.X335"]);
        string str = num.ToString();
        listValues["FV.X339"] = str;
      }
      Dictionary<string, string> listValues1 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X332"]) + this.toDecimal(this.listValues["FV.X336"]);
      string str1 = num.ToString();
      listValues1["FV.X340"] = str1;
      Dictionary<string, string> listValues2 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X329"]) + this.toDecimal(this.listValues["FV.X333"]) * this.toDecimal("1.1");
      string str2 = num.ToString();
      listValues2["FV.X341"] = str2;
      Dictionary<string, string> listValues3 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X330"]) + this.toDecimal(this.listValues["FV.X334"]) * this.toDecimal("1.1");
      string str3 = num.ToString();
      listValues3["FV.X342"] = str3;
      if (this.listValues["FV.X331"] == "" && this.listValues["FV.X335"] == "")
      {
        this.listValues["FV.X343"] = "";
      }
      else
      {
        Dictionary<string, string> listValues4 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X331"]) + this.toDecimal(this.listValues["FV.X335"]) * this.toDecimal("1.1");
        string str4 = num.ToString();
        listValues4["FV.X343"] = str4;
      }
      this.listValues["FV.X344"] = !(this.listValues["FV.X343"] != "") ? this.listValues["FV.X342"] : this.listValues["FV.X343"];
      Dictionary<string, string> listValues5 = this.listValues;
      num = this.toDecimal(this.listValues["FV.X24"]) + this.toDecimal(this.listValues["FV.X50"]) + this.toDecimal(this.listValues["FV.X208"]);
      string str5 = num.ToString();
      listValues5["FV.X345"] = str5;
      if (this.listValues["FV.X25"] == "" && this.listValues["FV.X51"] == "" && this.listValues["FV.X209"] == "")
      {
        this.listValues["FV.X346"] = "";
      }
      else
      {
        Dictionary<string, string> listValues6 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X25"]) + this.toDecimal(this.listValues["FV.X51"]) + this.toDecimal(this.listValues["FV.X209"]);
        string str6 = num.ToString();
        listValues6["FV.X346"] = str6;
      }
      if (this.listValues["FV.X26"] == "" && this.listValues["FV.X52"] == "" && this.listValues["FV.X210"] == "")
      {
        this.listValues["FV.X347"] = "";
      }
      else
      {
        Dictionary<string, string> listValues7 = this.listValues;
        num = this.toDecimal(this.listValues["FV.X26"]) + this.toDecimal(this.listValues["FV.X52"]) + this.toDecimal(this.listValues["FV.X210"]);
        string str7 = num.ToString();
        listValues7["FV.X347"] = str7;
      }
      if (this.CD_Recent_AppliedCure != null)
        return;
      this.updateAppliedCureAmount();
    }

    private void updateAppliedCureAmount()
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      this.CD_Recent_AppliedCure = (IDisclosureTracking2015Log) null;
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        if (disclosureTracking2015Log.CDReasonIsToleranceCure && (disclosureTracking2015Log.GetDisclosedField("4461") != "Y" || disclosureTracking2015Log.GetDisclosedField("4461") == "Y" && Utils.ToDouble(disclosureTracking2015Log.GetDisclosedField("FV.X348")) <= Utils.ToDouble(disclosureTracking2015Log.GetDisclosedField("FV.X366"))))
        {
          if (this.CD_Recent_AppliedCure == null)
            this.CD_Recent_AppliedCure = disclosureTracking2015Log;
          else if (disclosureTracking2015Log.CompareDisclosedDate(this.CD_Recent_AppliedCure) > 0)
            this.CD_Recent_AppliedCure = disclosureTracking2015Log;
        }
      }
      if (this.CD_Recent_AppliedCure == null)
        return;
      this.loan.SetField("FV.X386", this.CD_Recent_AppliedCure.Guid);
    }

    private void calculateToleranceCure()
    {
      if (!this.loan.IsLocked("FV.X348"))
      {
        string listValue1 = this.listValues["FV.X347"];
        if (listValue1 != "")
        {
          double num1 = 0.0;
          if (this.CD_Recent_AppliedCure == null)
            this.updateAppliedCureAmount();
          if (this.CD_Recent_AppliedCure != null)
          {
            double num2 = this.CDBaseline_CannotDecrease == null || this.CDBaseline_CannotDecrease.CompareDisclosedDate(this.CD_Recent_AppliedCure) < 0 ? (this.CDBaseline_CannotDecrease != null || !this.leBaselineUsed_CannotDecrease ? num1 + (Utils.ToDouble(this.listValues["FV.X26"]) - Utils.ToDouble(this.CD_Recent_AppliedCure.GetDisclosedField("FV.X26"))) : num1 + (Utils.ToDouble(this.listValues["FV.X26"]) - Utils.ToDouble(this.CD_Recent_AppliedCure.GetDisclosedField("FV.X24")))) : num1 + Utils.ToDouble(this.listValues["FV.X26"]);
            double num3 = this.CDBaseline_CannotIncrease == null || this.CDBaseline_CannotIncrease.CompareDisclosedDate(this.CD_Recent_AppliedCure) < 0 ? (this.CDBaseline_CannotIncrease != null || !this.LEBaselineUsed_CannotIncrease ? num2 + (Utils.ToDouble(this.listValues["FV.X52"]) - Utils.ToDouble(this.CD_Recent_AppliedCure.GetDisclosedField("FV.X52"))) : num2 + (Utils.ToDouble(this.listValues["FV.X52"]) - Utils.ToDouble(this.CD_Recent_AppliedCure.GetDisclosedField("FV.X50")))) : num2 + Utils.ToDouble(this.listValues["FV.X52"]);
            double num4 = this.CDBaseline_CannotIncrease10 == null || this.CDBaseline_CannotIncrease10.CompareDisclosedDate(this.CD_Recent_AppliedCure) < 0 ? (this.CDBaseline_CannotIncrease10 != null || !this.leBaselineUsed_CannotIncrease10 ? num3 + (Utils.ToDouble(this.listValues["FV.X210"]) - Utils.ToDouble(this.CD_Recent_AppliedCure.GetDisclosedField("FV.X210"))) : num3 + (Utils.ToDouble(this.listValues["FV.X210"]) - Utils.ToDouble(this.CD_Recent_AppliedCure.GetDisclosedField("FV.X208")))) : num3 + Utils.ToDouble(this.listValues["FV.X210"]);
            this.listValues["FV.X348"] = num4.ToString();
            this.loan.SetCurrentField("FV.X348", num4.ToString());
          }
          else
          {
            this.listValues["FV.X348"] = listValue1.ToString();
            this.loan.SetCurrentField("FV.X348", listValue1.ToString());
          }
        }
        else
        {
          string listValue2 = this.listValues["FV.X345"];
          if (!(listValue2 != ""))
            return;
          this.listValues["FV.X348"] = listValue2;
          this.loan.SetCurrentField("FV.X348", listValue2);
        }
      }
      else
        this.listValues["FV.X348"] = this.loan.GetField("FV.X348");
    }

    private bool skipLenderObligatedFee(string lineNumber, IDisclosureTracking2015Log log)
    {
      int lineNumber1 = Utils.ParseInt((object) lineNumber);
      if (!Utils.IsLenderObligatedFee(lineNumber1))
        return false;
      string indicatorFieldId = Utils.GetLenderObligatedIndicatorFieldID(lineNumber1);
      return (log == null ? this.loan.GetField(indicatorFieldId) : log.GetDisclosedField(indicatorFieldId)) == "Y";
    }

    private Decimal calculateLenderPaidClosingClosts(string version)
    {
      IDisclosureTracking2015Log log = (IDisclosureTracking2015Log) null;
      Decimal paidClosingClosts = 0M;
      switch (version)
      {
        case "LEInitial":
          log = this.LEInitial;
          break;
        case "LELatest":
          log = this.LEBaseline_CannotDecrease;
          break;
        case "CDLatest":
          log = this.CDBaseline_CannotDecrease;
          break;
      }
      if (version != "Itemization" && log != null)
      {
        for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          if ((!(strArray[0] == "0802") || !(strArray[1] == "x")) && !this.skipLenderObligatedFee(strArray[0], log) && log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) != "Broker")
            paidClosingClosts += this.toDecimal(log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]));
        }
      }
      else
      {
        for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          if ((!(strArray[0] == "0802") || !(strArray[1] == "x")) && !this.skipLenderObligatedFee(strArray[0], (IDisclosureTracking2015Log) null) && this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) != "Broker")
            paidClosingClosts += this.toDecimal(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]));
        }
      }
      return paidClosingClosts;
    }

    private void calculateCannotIncreaseMoreThan10Lines()
    {
      List<string> stringList = new List<string>()
      {
        "Seller",
        "Other"
      };
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) null;
      int num1 = 0;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      string str1 = "";
      string str2 = "";
      dictionary.Add(28, 58);
      dictionary.Add(62, 72);
      dictionary.Add(82, 106);
      dictionary.Add(119, 126);
      dictionary.Add(138, 141);
      if (this.CDBaseline_CannotIncrease10 != null)
        disclosureTracking2015Log = this.CDBaseline_CannotIncrease10;
      else if (this.LEBaseline_CannotIncrease10 != null)
        disclosureTracking2015Log = this.LEBaseline_CannotIncrease10;
      else if (this.LEInitial != null)
        disclosureTracking2015Log = this.LEInitial;
      if (disclosureTracking2015Log == null)
        return;
      for (int index = 60; index <= 203; ++index)
        this.listValues["FV.X" + (object) index] = "";
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        for (int key = keyValuePair.Key; key <= keyValuePair.Value; ++key)
        {
          int num8 = 15 - (67 - key) * 2;
          string homeID = "NEWHUD2.X44" + num8.ToString();
          num8 = 35 - (67 - key);
          string propertyID = "NEWHUD2.X44" + num8.ToString();
          num8 = 16 - (67 - key) * 2;
          string otherID = "NEWHUD2.X44" + num8.ToString();
          if (key != 99 && key != 63 && key != 64 && key != 66)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key];
            bool flag = false;
            Decimal num9 = 0M;
            Decimal num10 = 0M;
            string str3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string str4 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            GFEItem numberAndComponentId = GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentId(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER], strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID]);
            if (stringList.Contains(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && (this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "") && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, (IDisclosureTracking2015Log) null)) && this.toDecimal(this.loan.GetField(str3)) != 0M)
            {
              num9 = this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4));
              if (num1 < 24)
              {
                if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                  this.listValues["FV.X" + (object) (60 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                else
                  this.listValues["FV.X" + (object) (60 + num1 * 6)] = numberAndComponentId.Description;
                this.listValues["FV.X" + (object) (61 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                this.listValues["FV.X" + (object) (65 + num1 * 6)] = num9.ToString();
                flag = true;
              }
              num7 += num9;
            }
            if (stringList.Contains(this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && (this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "") && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.LEInitial)) && this.toDecimal(this.LEInitial.GetDisclosedField(str3)) != 0M)
            {
              if (num1 < 24)
              {
                if (!flag)
                {
                  if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                    this.listValues["FV.X" + (object) (60 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                  else
                    this.listValues["FV.X" + (object) (60 + num1 * 6)] = numberAndComponentId.Description;
                  this.listValues["FV.X" + (object) (61 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                  flag = true;
                }
                if (this.loan.GetField(str3) == "" && this.loan.GetField(str4) == "")
                {
                  this.listValues["FV.X" + (object) (65 + num1 * 6)] = "0.00";
                  this.listValues["FV.X" + (object) (63 + num1 * 6)] = "0.00";
                  this.listValues["FV.X" + (object) (62 + num1 * 6)] = (this.toDecimal(this.LEInitial.GetDisclosedField(str3)) - this.toDecimal(this.LEInitial.GetDisclosedField(str4))).ToString();
                }
                else
                  this.listValues["FV.X" + (object) (62 + num1 * 6)] = this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4)) == 0M ? "0" : (this.toDecimal(this.LEInitial.GetDisclosedField(str3)) - this.toDecimal(this.LEInitial.GetDisclosedField(str4))).ToString();
              }
              if (this.loan.GetField(str3) == "" && this.loan.GetField(str4) == "")
                num2 += this.toDecimal(this.LEInitial.GetDisclosedField(str3)) - this.toDecimal(this.LEInitial.GetDisclosedField(str4));
              else
                num2 += this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4)) == 0M ? 0M : this.toDecimal(this.LEInitial.GetDisclosedField(str3)) - this.toDecimal(this.LEInitial.GetDisclosedField(str4));
            }
            if (this.LEBaseline_CannotIncrease10 != null)
            {
              if (stringList.Contains(this.LEBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && this.LEBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && (this.LEBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.LEBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "") && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.LEBaseline_CannotIncrease10)) && this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField(str3)) != 0M)
              {
                num10 = !(this.loan.GetField(str3) == "") || !(this.loan.GetField(str4) == "") || this.LEBaseline_CannotIncrease10 == null || !(this.LEBaseline_CannotIncrease10.Guid != this.LEInitial.Guid) ? (this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4)) == 0M ? 0M : this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField(str3)) - this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField(str4))) : this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField(str3)) - this.toDecimal(this.LEBaseline_CannotIncrease10.GetDisclosedField(str4));
                if (num1 < 24)
                {
                  if (!flag)
                  {
                    if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                      this.listValues["FV.X" + (object) (60 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                    else
                      this.listValues["FV.X" + (object) (60 + num1 * 6)] = numberAndComponentId.Description;
                    this.listValues["FV.X" + (object) (61 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                    flag = true;
                  }
                  this.listValues["FV.X" + (object) (63 + num1 * 6)] = num10.ToString();
                }
                if (this.listValues["FV.X" + (object) (65 + num1 * 6)].Trim() != "")
                  num5 += num10;
              }
              if (Math.Max(0M, num9 - num10) > 0M)
                str1 = str1 + str3 + ",";
            }
            if (this.CDBaseline_CannotIncrease10 != null)
            {
              if (stringList.Contains(this.CDBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && this.CDBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && (this.CDBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.CDBaseline_CannotIncrease10.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "") && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.CDBaseline_CannotIncrease10)) && this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField(str3)) != 0M)
              {
                num10 = this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4)) == 0M ? 0M : this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField(str3)) - this.toDecimal(this.CDBaseline_CannotIncrease10.GetDisclosedField(str4));
                if (num1 < 24)
                {
                  if (!flag)
                  {
                    if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                      this.listValues["FV.X" + (object) (60 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                    else
                      this.listValues["FV.X" + (object) (60 + num1 * 6)] = numberAndComponentId.Description;
                    this.listValues["FV.X" + (object) (61 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                    flag = true;
                  }
                  this.listValues["FV.X" + (object) (64 + num1 * 6)] = num10.ToString();
                }
                if (this.listValues["FV.X" + (object) (65 + num1 * 6)].Trim() != "")
                  num6 += num10;
              }
              if (Math.Max(0M, num9 - num10) > 0M)
                str2 = str2 + str3 + ",";
            }
            if (this.LELatest != null && stringList.Contains(this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && (this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "") && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.LELatest)))
            {
              if (this.loan.GetField(str3) == "" && this.loan.GetField(str4) == "" && this.LELatest.GetDisclosedField(str3) != "")
                num3 += this.toDecimal(this.LELatest.GetDisclosedField(str3)) - this.toDecimal(this.LELatest.GetDisclosedField(str4));
              else
                num3 += this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4)) == 0M ? 0M : this.toDecimal(this.LELatest.GetDisclosedField(str3)) - this.toDecimal(this.LELatest.GetDisclosedField(str4));
            }
            if (this.CDLatest != null && stringList.Contains(this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && (this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "") && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.CDLatest)))
            {
              if (this.loan.GetField(str3) == "" && this.loan.GetField(str4) == "" && this.CDLatest.GetDisclosedField(str3) != "")
                num4 += this.toDecimal(this.CDLatest.GetDisclosedField(str3)) - this.toDecimal(this.CDLatest.GetDisclosedField(str4));
              else
                num4 += this.toDecimal(this.loan.GetField(str3)) - this.toDecimal(this.loan.GetField(str4)) == 0M ? 0M : this.toDecimal(this.CDLatest.GetDisclosedField(str3)) - this.toDecimal(this.CDLatest.GetDisclosedField(str4));
            }
            if (flag)
              ++num1;
          }
        }
      }
      this.listValues["FV.X204"] = num2.ToString();
      this.listValues["FV.X205"] = num5.ToString();
      this.listValues["FV.X206"] = num6.ToString();
      this.listValues["FV.X207"] = num7.ToString();
      this.listValues["FV.X353"] = num2.ToString();
      this.listValues["FV.X354"] = num3.ToString();
      this.listValues["FV.X355"] = num4.ToString();
      if (this.CDBaseline_CannotIncrease10 == null && this.leBaselineUsed_CannotIncrease10)
      {
        for (int index = 64; index <= 202; index += 6)
          this.listValues["FV.X" + (object) index] = this.listValues["FV.X" + (object) (index - 1)];
        this.listValues["FV.X206"] = this.listValues["FV.X205"];
        this.listValues["FV.X213"] = this.listValues["FV.X212"];
      }
      if (this.CDBaseline_CannotIncrease10 != null)
        this.cannotIncrease10VarianceFieldIDs += str2;
      else
        this.cannotIncrease10VarianceFieldIDs += str1;
    }

    private bool evaluateUseNewComplianceFor900()
    {
      string field1 = this.loan.GetField("FV.X370");
      string field2 = this.loan.GetField("FV.X371");
      string field3 = this.loan.GetField("FV.X372");
      string field4 = this.loan.GetField("FV.X373");
      return !(this.toDecimal(field1) != 0M) && !(this.toDecimal(field2) != 0M) && !(this.toDecimal(field3) != 0M) && !(this.toDecimal(field4) != 0M) || this.calObjs.NewHud2015Cal.UseNewCompliance(18.3M);
    }

    private bool evaluateCannotIncreaseFor900(
      string homeID,
      string propertyID,
      string otherID,
      IDisclosureTracking2015Log log)
    {
      if (this.evaluateUseNewComplianceFor900())
        return false;
      if (log == null)
      {
        if (this.loan.GetField(homeID) == "Y" || this.loan.GetField(propertyID) == "Y" || this.loan.GetField(otherID) == "Y")
          return false;
      }
      else if (log.GetDisclosedField(homeID) == "Y" || log.GetDisclosedField(propertyID) == "Y" || log.GetDisclosedField(otherID) == "Y")
        return false;
      return true;
    }

    private bool evaluateCanChange900(
      int i,
      string homeID,
      string propertyID,
      string otherID,
      string canShop,
      IDisclosureTracking2015Log log)
    {
      if ((i == 64 || i >= 67 && i <= 72) && this.evaluateUseNewComplianceFor900())
        return true;
      if (log == null)
      {
        switch (i)
        {
          case 64:
            return this.loan.GetField(canShop) == "Y";
          case 67:
          case 68:
          case 69:
          case 70:
          case 71:
          case 72:
            if (this.loan.GetField(homeID) == "Y")
              return true;
            if (this.loan.GetField(propertyID) == "Y" || this.loan.GetField(otherID) == "Y")
              return this.loan.GetField(canShop) == "Y";
            break;
        }
      }
      else
      {
        switch (i)
        {
          case 64:
            return log.GetDisclosedField(canShop) == "Y";
          case 67:
          case 68:
          case 69:
          case 70:
          case 71:
          case 72:
            if (log.GetDisclosedField(homeID) == "Y")
              return true;
            if (log.GetDisclosedField(propertyID) == "Y" || log.GetDisclosedField(otherID) == "Y")
              return log.GetDisclosedField(canShop) == "Y";
            break;
        }
      }
      return false;
    }

    private void calculateCanChangeLines()
    {
      List<string> stringList = new List<string>()
      {
        "Seller",
        "Other"
      };
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      int num1 = 0;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      dictionary.Add(62, 72);
      dictionary.Add(82, 106);
      dictionary.Add(119, 126);
      dictionary.Add((int) sbyte.MaxValue, 137);
      dictionary.Add(138, 141);
      string str1 = "NEWHUD2.X44";
      for (int index = 227; index <= 316; ++index)
        this.listValues["FV.X" + (object) index] = "";
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        for (int key1 = keyValuePair.Key; key1 <= keyValuePair.Value; ++key1)
        {
          switch (key1)
          {
            case 99:
              continue;
            case (int) sbyte.MaxValue:
              stringList.Add("Affiliate");
              break;
          }
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key1];
          string str2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
          string str3 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
          bool flag = false;
          GFEItem numberAndComponentId = GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentId(strArray[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER], strArray[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID]);
          string str4 = str1;
          int num6 = 15 - (67 - key1) * 2;
          string str5 = num6.ToString();
          string homeID = str4 + str5;
          string str6 = str1;
          num6 = 35 - (67 - key1);
          string str7 = num6.ToString();
          string propertyID = str6 + str7;
          string str8 = str1;
          num6 = 16 - (67 - key1) * 2;
          string str9 = num6.ToString();
          string otherID = str8 + str9;
          Decimal num7;
          if ((stringList.Contains(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 64 || key1 >= 66 && key1 <= 72) && (key1 < (int) sbyte.MaxValue && this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 66 || this.evaluateCanChange900(key1, homeID, propertyID, otherID, strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], (IDisclosureTracking2015Log) null)) && (key1 >= 138 && key1 <= 141 && this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" && this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" || key1 < 138))
          {
            if (!(this.toDecimal(this.loan.GetField(str2)) == 0M))
            {
              if (num1 < 15)
              {
                if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                else
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = numberAndComponentId.Description;
                this.listValues["FV.X" + (object) (228 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                Dictionary<string, string> listValues = this.listValues;
                string key2 = "FV.X" + (object) (232 + num1 * 6);
                num7 = this.toDecimal(this.loan.GetField(str2)) - this.toDecimal(this.loan.GetField(str3));
                string str10 = num7.ToString();
                listValues[key2] = str10;
                flag = true;
              }
              num5 += this.toDecimal(this.loan.GetField(str2)) - this.toDecimal(this.loan.GetField(str3));
            }
            else
              continue;
          }
          if ((stringList.Contains(this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 64 || key1 >= 66 && key1 <= 72) && (key1 < (int) sbyte.MaxValue && this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 66 || this.evaluateCanChange900(key1, homeID, propertyID, otherID, strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], this.LEInitial)) && (key1 >= 138 && key1 <= 141 && this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" && this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" || key1 < 138) && this.toDecimal(this.LEInitial.GetDisclosedField(str2)) != 0M)
          {
            if (num1 < 15)
            {
              if (!flag)
              {
                if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                else
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = numberAndComponentId.Description;
                this.listValues["FV.X" + (object) (228 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                flag = true;
              }
              Dictionary<string, string> listValues = this.listValues;
              string key3 = "FV.X" + (object) (229 + num1 * 6);
              num7 = this.toDecimal(this.LEInitial.GetDisclosedField(str2)) - this.toDecimal(this.LEInitial.GetDisclosedField(str3));
              string str11 = num7.ToString();
              listValues[key3] = str11;
            }
            num2 += this.toDecimal(this.LEInitial.GetDisclosedField(str2)) - this.toDecimal(this.LEInitial.GetDisclosedField(str3));
          }
          if (this.LELatest != null && (stringList.Contains(this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 64 || key1 >= 66 && key1 <= 72) && (key1 < (int) sbyte.MaxValue && this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 66 || this.evaluateCanChange900(key1, homeID, propertyID, otherID, strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], this.LELatest)) && (key1 >= 138 && key1 <= 141 && this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" && this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" || key1 < 138) && this.toDecimal(this.LELatest.GetDisclosedField(str2)) != 0M)
          {
            if (num1 < 15)
            {
              if (!flag)
              {
                if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                else
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = numberAndComponentId.Description;
                this.listValues["FV.X" + (object) (228 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                flag = true;
              }
              Dictionary<string, string> listValues = this.listValues;
              string key4 = "FV.X" + (object) (230 + num1 * 6);
              num7 = this.toDecimal(this.LELatest.GetDisclosedField(str2)) - this.toDecimal(this.LELatest.GetDisclosedField(str3));
              string str12 = num7.ToString();
              listValues[key4] = str12;
            }
            num3 += this.toDecimal(this.LELatest.GetDisclosedField(str2)) - this.toDecimal(this.LELatest.GetDisclosedField(str3));
          }
          if (this.CDLatest != null && (stringList.Contains(this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 64 || key1 >= 66 && key1 <= 72) && (key1 < (int) sbyte.MaxValue && this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" || key1 >= (int) sbyte.MaxValue || key1 == 63 || key1 == 66 || this.evaluateCanChange900(key1, homeID, propertyID, otherID, strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP], this.CDLatest)) && (key1 >= 138 && key1 <= 141 && this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" && this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" || key1 < 138) && this.toDecimal(this.CDLatest.GetDisclosedField(str2)) != 0M)
          {
            if (num1 < 15)
            {
              if (!flag)
              {
                if (numberAndComponentId.Description.Length <= 4 || numberAndComponentId.Description.StartsWith("NEWHUD.X") || numberAndComponentId.Description.StartsWith("NEWHUD2.X"))
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = this.loan.GetField(numberAndComponentId.Description);
                else
                  this.listValues["FV.X" + (object) (227 + num1 * 6)] = numberAndComponentId.Description;
                this.listValues["FV.X" + (object) (228 + num1 * 6)] = numberAndComponentId.LineNumber.ToString() + numberAndComponentId.ComponentID;
                flag = true;
              }
              Dictionary<string, string> listValues = this.listValues;
              string key5 = "FV.X" + (object) (231 + num1 * 6);
              num7 = this.toDecimal(this.CDLatest.GetDisclosedField(str2)) - this.toDecimal(this.CDLatest.GetDisclosedField(str3));
              string str13 = num7.ToString();
              listValues[key5] = str13;
            }
            num4 += this.toDecimal(this.CDLatest.GetDisclosedField(str2)) - this.toDecimal(this.CDLatest.GetDisclosedField(str3));
          }
          if (flag)
            ++num1;
        }
      }
      this.listValues["FV.X317"] = num2.ToString();
      this.listValues["FV.X318"] = num3.ToString();
      this.listValues["FV.X319"] = num4.ToString();
      this.listValues["FV.X320"] = num5.ToString();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        string key6 = "FV.X" + (object) (227 + index1 * 6);
        for (int index2 = index1 + 1; index2 < num1; ++index2)
        {
          string key7 = "FV.X" + (object) (227 + index2 * 6);
          if (string.Compare(this.listValues[key6], this.listValues[key7], true) > 0)
          {
            for (int index3 = 0; index3 < 6; ++index3)
            {
              string key8 = "FV.X" + (object) (227 + index1 * 6 + index3);
              string key9 = "FV.X" + (object) (227 + index2 * 6 + index3);
              string listValue = this.listValues[key8];
              this.listValues[key8] = this.listValues[key9];
              this.listValues[key9] = listValue;
            }
          }
        }
      }
    }

    private void getLEandCDBaselineCannotDecrease()
    {
      this.replaceLEFee_Cannot_Descrease = false;
      this.replaceCDFee_Cannot_Descrease = false;
      if (this.LELatest == null)
        return;
      this.LEBaseline_CannotDecrease = this.LEInitial;
      Decimal num1 = this.toDecimal(this.LEInitial.GetDisclosedField("TotalLenderCredit"));
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && (this.CDInitial == null || disclosureTracking2015Log.CompareDisclosedDate(this.CDInitial) <= 0))
        {
          if (disclosureTracking2015Log.LEReasonIsRevisionsRequestedByConsumer)
          {
            this.LEBaseline_CannotDecrease = disclosureTracking2015Log;
            num1 = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("TotalLenderCredit"));
            this.replaceLEFee_Cannot_Descrease = true;
          }
          else if (this.toDecimal(disclosureTracking2015Log.GetDisclosedField("TotalLenderCredit")) >= num1)
          {
            this.LEBaseline_CannotDecrease = disclosureTracking2015Log;
            num1 = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("TotalLenderCredit"));
            this.replaceLEFee_Cannot_Descrease = false;
          }
          else if (disclosureTracking2015Log.LEReasonIsInterestRateDependentCharges || disclosureTracking2015Log.LEReasonIsRevisionsRequestedByConsumer || disclosureTracking2015Log.LEReasonIsExpiration || disclosureTracking2015Log.LEReasonIsChangedCircumstanceEligibility)
          {
            this.LEBaseline_CannotDecrease = disclosureTracking2015Log;
            num1 = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("TotalLenderCredit"));
            this.replaceLEFee_Cannot_Descrease = true;
          }
        }
      }
      if (this.LEBaseline_CannotDecrease == null)
        this.LEBaseline_CannotDecrease = this.LEInitial;
      this.cal_ItemsThatCannotDecrease((string) null, (string) null);
      if (this.CDInitial == null || this.CDLatest == null)
      {
        this.CDBaseline_CannotDecrease = (IDisclosureTracking2015Log) null;
        this.leBaselineUsed_CannotDecrease = false;
      }
      else
      {
        Decimal num2 = this.toDecimal(this.CDInitial.GetDisclosedField("TotalLenderCredit"));
        foreach (IDisclosureTracking2015Log log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
        {
          if (log.DisclosedForCD && log.CompareDisclosedDate(this.LELatest) >= 0)
          {
            if (log.CDReasonIsRevisionsRequestedByConsumer)
            {
              this.CDBaseline_CannotDecrease = log;
              num2 = this.toDecimal(log.GetDisclosedField("TotalLenderCredit"));
              this.replaceCDFee_Cannot_Descrease = true;
            }
            else
            {
              Decimal num3 = this.toDecimal(log.GetDisclosedField("TotalLenderCredit"));
              if (this.validCDChangeCircumstanceDateWithClosingDate(log, num3 > num2) && (!log.CDReasonIsChangeInSettlementCharges || log.CDReasonIsChangeInAPR || log.CDReasonIsChangeInLoanProduct || log.CDReasonIsPrepaymentPenaltyAdded || log.CDReasonIsChangedCircumstanceEligibility || log.CDReasonIsRevisionsRequestedByConsumer || log.CDReasonIsInterestRateDependentCharges || log.CDReasonIs24HourAdvancePreview || log.CDReasonIsToleranceCure || log.CDReasonIsClericalErrorCorrection || log.CDReasonIsOther || !(num3 < this.toDecimal(this.listValues["FV.X18"]))))
              {
                if (!this.leBaselineUsed_CannotDecrease && this.listValues["FV.X18"] != "" && num3 >= num2)
                {
                  if (log != this.CDInitial)
                  {
                    this.CDBaseline_CannotDecrease = log;
                    num2 = num3;
                    this.replaceCDFee_Cannot_Descrease = false;
                  }
                }
                else if (this.listValues["FV.X18"] != "" && !this.leBaselineUsed_CannotDecrease)
                {
                  if (log.CDReasonIsInterestRateDependentCharges || log.CDReasonIsChangedCircumstanceEligibility || log.CDReasonIsRevisionsRequestedByConsumer)
                  {
                    this.CDBaseline_CannotDecrease = log;
                    num2 = num3;
                    this.replaceCDFee_Cannot_Descrease = true;
                  }
                }
                else if (this.toDecimal(this.listValues["FV.X24"]) >= 0M && (log.CDReasonIsInterestRateDependentCharges || log.CDReasonIsChangedCircumstanceEligibility || log.CDReasonIsRevisionsRequestedByConsumer))
                {
                  if (log != this.CDInitial)
                  {
                    this.CDBaseline_CannotDecrease = log;
                    num2 = num3;
                    this.replaceCDFee_Cannot_Descrease = true;
                  }
                  else
                  {
                    this.CDBaseline_CannotDecrease = log;
                    num2 = num3;
                    this.replaceCDFee_Cannot_Descrease = true;
                  }
                }
              }
            }
          }
        }
        if (this.CDBaseline_CannotDecrease == null && this.LEBaseline_CannotDecrease != null)
        {
          Decimal num4 = this.toDecimal(this.LEBaseline_CannotDecrease.GetDisclosedField("TotalLenderCredit"));
          if (num2 > num4)
            this.CDBaseline_CannotDecrease = this.CDInitial;
        }
        if (this.CDBaseline_CannotDecrease == null)
          this.leBaselineUsed_CannotDecrease = true;
        else
          this.leBaselineUsed_CannotDecrease = false;
      }
    }

    private void getLEandCDBaselineCannotIncrease()
    {
      this.replaceLEFee_Cannot_Increase = false;
      this.replaceCDFee_Cannot_Increase = false;
      if (this.LELatest == null)
        return;
      this.LEBaseline_CannotIncrease = this.LEInitial;
      this.leBaselineUsed_CannotIncrease = false;
      this.cal_ChargesThatCannotIncrease((string) null, (string) null);
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && (this.CDInitial == null || disclosureTracking2015Log.CompareDisclosedDate(this.CDInitial) <= 0))
        {
          if (disclosureTracking2015Log.LEReasonIsRevisionsRequestedByConsumer)
          {
            this.LEBaseline_CannotIncrease = disclosureTracking2015Log;
            this.cal_ChargesThatCannotIncrease((string) null, (string) null);
            this.replaceLEFee_Cannot_Increase = true;
          }
          else if (!disclosureTracking2015Log.LEReasonIsInterestRateDependentCharges || disclosureTracking2015Log.LEReasonIsChangedCircumstanceSettlementCharges || disclosureTracking2015Log.LEReasonIsChangedCircumstanceEligibility || disclosureTracking2015Log.LEReasonIsExpiration || disclosureTracking2015Log.LEReasonIsDelayedSettlementOnConstructionLoans || disclosureTracking2015Log.LEReasonIsOther || !this.feeIsNotChangedExceptTransferTaxes(this.LEInitial, disclosureTracking2015Log, true) || !(this.getSumOfRange(disclosureTracking2015Log, 4, 21) + this.getSumOfRange(disclosureTracking2015Log, 23, 26) == this.getSumOfRange(this.LEInitial, 4, 21) + this.getSumOfRange(this.LEInitial, 23, 26)) || !(this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3695")) + this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3728")) + this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3761")) > this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3695")) + this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3728")) + this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.LEInitial.GetDisclosedField("NEWHUD2.X3761"))))
          {
            if (this.listValues["FV.X50"] == "" || this.toDecimal(this.listValues["FV.X50"]) > 0M)
            {
              if (disclosureTracking2015Log.LEReasonAnyChecked)
              {
                this.LEBaseline_CannotIncrease = disclosureTracking2015Log;
                this.cal_ChargesThatCannotIncrease((string) null, (string) null);
                this.replaceLEFee_Cannot_Increase = true;
              }
            }
            else if (this.toDecimal(this.listValues["FV.X50"]) == 0M)
            {
              this.LEBaseline_CannotIncrease = disclosureTracking2015Log;
              this.cal_ChargesThatCannotIncrease((string) null, (string) null);
              this.replaceLEFee_Cannot_Increase = true;
            }
          }
        }
      }
      if (this.LEBaseline_CannotIncrease == null)
        this.LEBaseline_CannotIncrease = this.LEInitial;
      if (this.CDInitial == null || this.CDLatest == null)
      {
        this.CDBaseline_CannotIncrease = (IDisclosureTracking2015Log) null;
        this.leBaselineUsed_CannotIncrease = false;
      }
      else
      {
        this.cal_ChargesThatCannotIncrease((string) null, (string) null);
        Decimal num = this.toDecimal(this.CDInitial.GetDisclosedField("CannotIncrease"));
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
        {
          if (disclosureTracking2015Log.DisclosedForCD && disclosureTracking2015Log.CompareDisclosedDate(this.LELatest) >= 0)
          {
            if (disclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer)
            {
              this.CDBaseline_CannotIncrease = disclosureTracking2015Log;
              num = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease"));
              this.cal_ChargesThatCannotIncrease((string) null, (string) null);
              this.replaceCDFee_Cannot_Increase = true;
            }
            else if (disclosureTracking2015Log != this.CDInitial && this.toDecimal(this.listValues["FV.X52"]) == 0M && (disclosureTracking2015Log.CDReasonIsInterestRateDependentCharges || disclosureTracking2015Log.CDReasonIsChangeInSettlementCharges || disclosureTracking2015Log.CDReasonIsChangedCircumstanceEligibility || disclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer))
            {
              this.CDBaseline_CannotIncrease = disclosureTracking2015Log;
              num = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease"));
              this.cal_ChargesThatCannotIncrease((string) null, (string) null);
              this.replaceCDFee_Cannot_Increase = true;
            }
            else if (this.validCDChangeCircumstanceDateWithClosingDate(disclosureTracking2015Log, false) && (!disclosureTracking2015Log.CDReasonIsInterestRateDependentCharges || disclosureTracking2015Log.CDReasonIsChangeInSettlementCharges || disclosureTracking2015Log.CDReasonIsChangeInAPR || disclosureTracking2015Log.CDReasonIsChangeInLoanProduct || disclosureTracking2015Log.CDReasonIsPrepaymentPenaltyAdded || disclosureTracking2015Log.CDReasonIsChangedCircumstanceEligibility || disclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer || disclosureTracking2015Log.CDReasonIs24HourAdvancePreview || disclosureTracking2015Log.CDReasonIsToleranceCure || disclosureTracking2015Log.CDReasonIsClericalErrorCorrection || disclosureTracking2015Log.CDReasonIsOther || !this.feeIsNotChangedExceptTransferTaxes(this.CDInitial, disclosureTracking2015Log, true) || !(this.getSumOfRange(disclosureTracking2015Log, 4, 21) + this.getSumOfRange(disclosureTracking2015Log, 23, 26) == this.getSumOfRange(this.CDInitial, 4, 21) + this.getSumOfRange(this.CDInitial, 23, 26)) || !(this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3695")) + this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3728")) + this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(disclosureTracking2015Log.GetDisclosedField("NEWHUD2.X3761")) > this.toDecimal(this.CDInitial.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(this.CDInitial.GetDisclosedField("NEWHUD2.X3695")) + this.toDecimal(this.CDInitial.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(this.CDInitial.GetDisclosedField("NEWHUD2.X3728")) + this.toDecimal(this.CDInitial.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(this.CDInitial.GetDisclosedField("NEWHUD2.X3761")))))
            {
              if (!this.leBaselineUsed_CannotIncrease && this.listValues["FV.X48"] != "" && this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease")) <= num)
              {
                if (disclosureTracking2015Log != this.CDInitial)
                {
                  this.CDBaseline_CannotIncrease = disclosureTracking2015Log;
                  num = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease"));
                  this.cal_ChargesThatCannotIncrease((string) null, (string) null);
                  this.replaceCDFee_Cannot_Increase = true;
                }
              }
              else if (this.listValues["FV.X48"] != "" && !this.leBaselineUsed_CannotIncrease)
              {
                if ((disclosureTracking2015Log.CDReasonIsInterestRateDependentCharges || disclosureTracking2015Log.CDReasonIsChangeInSettlementCharges || disclosureTracking2015Log.CDReasonIsChangedCircumstanceEligibility || disclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer) && this.toDecimal(this.listValues["FV.X52"]) > 0M)
                {
                  this.CDBaseline_CannotIncrease = disclosureTracking2015Log;
                  num = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease"));
                  this.cal_ChargesThatCannotIncrease((string) null, (string) null);
                  this.replaceCDFee_Cannot_Increase = true;
                }
              }
              else if ((this.toDecimal(this.listValues["FV.X50"]) == 0M || this.toDecimal(this.listValues["FV.X50"]) > 0M) && (this.toDecimal(this.listValues["FV.X50"]) == 0M || this.toDecimal(this.listValues["FV.X50"]) > 0M && (disclosureTracking2015Log.CDReasonIsInterestRateDependentCharges || disclosureTracking2015Log.CDReasonIsChangeInSettlementCharges || disclosureTracking2015Log.CDReasonIsChangedCircumstanceEligibility || disclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer)))
              {
                if (disclosureTracking2015Log != this.CDInitial)
                {
                  if (this.toDecimal(this.listValues["FV.X52"]) > 0M || this.CDBaseline_CannotIncrease == null)
                  {
                    this.CDBaseline_CannotIncrease = disclosureTracking2015Log;
                    num = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease"));
                    this.cal_ChargesThatCannotIncrease((string) null, (string) null);
                    this.replaceCDFee_Cannot_Increase = true;
                  }
                }
                else
                {
                  this.CDBaseline_CannotIncrease = disclosureTracking2015Log;
                  num = this.toDecimal(disclosureTracking2015Log.GetDisclosedField("CannotIncrease"));
                  this.cal_ChargesThatCannotIncrease((string) null, (string) null);
                  this.replaceCDFee_Cannot_Increase = true;
                }
              }
            }
          }
        }
        if (this.CDBaseline_CannotIncrease == null)
        {
          this.leBaselineUsed_CannotIncrease = true;
          this.cal_ChargesThatCannotIncrease((string) null, (string) null);
          this.loan.SetField("FV.X380", this.loan.GetField("FV.X379"));
        }
        else
          this.leBaselineUsed_CannotIncrease = false;
      }
    }

    private void getLEandCDBaselineCannotIncrease10()
    {
      this.replaceLEFee_Cannot_Increase10 = false;
      this.replaceCDFee_Cannot_Increase10 = false;
      if (this.LELatest == null)
        return;
      this.LEBaseline_CannotIncrease10 = this.LEInitial;
      this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.DisclosedForLE && (this.CDInitial == null || disclosureTracking2015Log.CompareDisclosedDate(this.CDInitial) <= 0))
        {
          if (disclosureTracking2015Log.LEReasonIsRevisionsRequestedByConsumer)
          {
            this.LEBaseline_CannotIncrease10 = disclosureTracking2015Log;
            this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
            this.replaceLEFee_Cannot_Increase10 = true;
          }
          else if (disclosureTracking2015Log.GetDisclosedField("FV.X208") == "" || this.toDecimal(disclosureTracking2015Log.GetDisclosedField("FV.X208")) > 0M)
          {
            if (disclosureTracking2015Log.LEReasonAnyChecked)
            {
              this.LEBaseline_CannotIncrease10 = disclosureTracking2015Log;
              this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
              this.replaceLEFee_Cannot_Increase10 = true;
            }
          }
          else if (this.toDecimal(disclosureTracking2015Log.GetDisclosedField("FV.X208")) == 0M && this.toDecimal(disclosureTracking2015Log.GetDisclosedField("FV.X207")) < this.toDecimal(disclosureTracking2015Log.GetDisclosedField("FV.X205")))
          {
            this.LEBaseline_CannotIncrease10 = disclosureTracking2015Log;
            this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
            this.replaceLEFee_Cannot_Increase10 = false;
          }
        }
      }
      if (this.LEBaseline_CannotIncrease10 == null)
        this.LEBaseline_CannotIncrease10 = this.LEInitial;
      if (this.CDInitial == null || this.CDLatest == null)
      {
        this.CDBaseline_CannotIncrease10 = (IDisclosureTracking2015Log) null;
        this.leBaselineUsed_CannotIncrease10 = false;
      }
      else
      {
        foreach (IDisclosureTracking2015Log log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
        {
          if (log.DisclosedForCD && log.CompareDisclosedDate(this.LELatest) >= 0)
          {
            if (log.CDReasonIsRevisionsRequestedByConsumer)
            {
              this.CDBaseline_CannotIncrease10 = log;
              this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
              this.replaceCDFee_Cannot_Increase10 = true;
            }
            else if (log != this.CDInitial && this.toDecimal(log.GetDisclosedField("FV.X210")) == 0M && this.toDecimal(log.GetDisclosedField("FV.X207")) < this.toDecimal(log.GetDisclosedField("FV.X206")))
            {
              this.CDBaseline_CannotIncrease10 = log;
              this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
              this.replaceCDFee_Cannot_Increase10 = false;
            }
            else if (this.validCDChangeCircumstanceDateWithClosingDate(log, false))
            {
              if (this.listValues["FV.X206"] != "" && !this.leBaselineUsed_CannotIncrease10)
              {
                if ((log.CDReasonIsChangeInSettlementCharges || log.CDReasonIsChangedCircumstanceEligibility || log.CDReasonIsRevisionsRequestedByConsumer || log.CDReasonIsInterestRateDependentCharges) && this.toDecimal(log.GetDisclosedField("FV.X210")) > 0M)
                {
                  this.CDBaseline_CannotIncrease10 = log;
                  this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
                  this.replaceCDFee_Cannot_Increase10 = true;
                }
              }
              else if ((log.GetDisclosedField("FV.X208") == "" || this.toDecimal(log.GetDisclosedField("FV.X208")) > 0M) && (log.GetDisclosedField("FV.X208") == "" || this.toDecimal(log.GetDisclosedField("FV.X208")) > 0M && (log.CDReasonIsChangeInSettlementCharges || log.CDReasonIsChangedCircumstanceEligibility || log.CDReasonIsRevisionsRequestedByConsumer || log.CDReasonIsInterestRateDependentCharges)))
              {
                if (log != this.CDInitial)
                {
                  if (this.toDecimal(log.GetDisclosedField("FV.X210")) > 0M || this.CDBaseline_CannotIncrease10 == null)
                  {
                    this.CDBaseline_CannotIncrease10 = log;
                    this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
                    this.replaceCDFee_Cannot_Increase10 = true;
                  }
                }
                else
                {
                  this.CDBaseline_CannotIncrease10 = log;
                  this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
                  this.replaceCDFee_Cannot_Increase10 = true;
                }
              }
            }
          }
        }
        if (this.CDBaseline_CannotIncrease10 == null)
        {
          this.leBaselineUsed_CannotIncrease10 = true;
          this.loan.SetField("FV.X384", this.loan.GetField("FV.X383"));
        }
        else
          this.leBaselineUsed_CannotIncrease10 = false;
      }
    }

    private bool validCDChangeCircumstanceDateWithClosingDate(
      IDisclosureTracking2015Log log,
      bool ignoreCoCDate)
    {
      if (this.useNewCDRebaseline && this.closingDate > this.issueDate || !this.useNewCDRebaseline && this.closingDate > this.issueDate && this.closingDate <= this.acceptableClosingTime)
      {
        if (this.businessCalendar == null | ignoreCoCDate)
          return true;
        DateTime date1 = Utils.ParseDate((object) log.GetDisclosedField("CD1.X62")).Date;
        DateTime date2 = Utils.ParseDate((object) log.GetDisclosedField("CD1.X1")).Date;
        if (!this.useNewCDRebaseline && date2 != DateTime.MinValue)
          date2 = this.businessCalendar.AddBusinessDays(date2, 3, true);
        if (date1.Date <= date2.Date)
        {
          if (this.useNewCDRebaseline && date1 != DateTime.MinValue)
            return this.businessCalendar.AddBusinessDays(date1, 3, true).Date >= date2.Date;
          if (log.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.Initial || !this.useNewCDRebaseline && date1 != DateTime.MinValue && Utils.ParseDate((object) log.GetDisclosedField("748")).Date <= this.businessCalendar.AddBusinessDays(date1, 7, true))
            return true;
        }
      }
      return false;
    }

    private Decimal[] calculatePropertyTaxesCannotIncrease()
    {
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      string str1 = "";
      string str2 = "";
      Decimal num7 = 0M;
      Decimal num8 = 0M;
      if (this.evaluateUseNewComplianceFor900())
        return new Decimal[8]
        {
          num1,
          num2,
          num3,
          num4,
          num5,
          num6,
          num7,
          num8
        };
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      dictionary.Add(64, 64);
      dictionary.Add(67, 72);
      string str3 = "NEWHUD2.X44";
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        for (int key = keyValuePair.Key; key <= keyValuePair.Value; ++key)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key];
          string str4 = str3;
          int num9 = 35 - (67 - key);
          string str5 = num9.ToString();
          string str6 = str4 + str5;
          string str7 = str3;
          num9 = 16 - (67 - key) * 2;
          string str8 = num9.ToString();
          string str9 = str7 + str8;
          string str10 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP];
          string str11 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
          string str12 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
          Decimal num10 = 0M;
          Decimal num11 = this.toDecimal(this.loan.GetField(str11)) - this.toDecimal(this.loan.GetField(str12));
          if (num11 > 0M && (key == 64 || this.loan.GetField(str6) == "Y" || this.loan.GetField(str9) == "Y") && this.loan.GetField(str10) != "Y")
          {
            num10 = num11;
            num4 += num10;
          }
          if (this.LEInitial != null)
          {
            Decimal num12 = this.toDecimal(this.LEInitial.GetDisclosedField(str11)) - this.toDecimal(this.LEInitial.GetDisclosedField(str12));
            if (num12 > 0M && (key == 64 || this.LEInitial.GetDisclosedField(str6) == "Y" || this.LEInitial.GetDisclosedField(str9) == "Y") && this.LEInitial.GetDisclosedField(str10) != "Y")
              num1 += num12;
          }
          if (this.LEBaseline_CannotIncrease != null)
          {
            Decimal num13 = this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str11)) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str12));
            Decimal num14;
            if ((key == 64 || this.LEBaseline_CannotIncrease.GetDisclosedField(str6) == "Y" || this.LEBaseline_CannotIncrease.GetDisclosedField(str9) == "Y") && this.LEBaseline_CannotIncrease.GetDisclosedField(str10) != "Y")
            {
              num2 += num13;
              num14 = Math.Max(0M, num10 - num13);
            }
            else
              num14 = Math.Max(0M, num10 - 0M);
            num5 += num14;
            if (num14 > 0M)
              str1 = str1 + str11 + ",";
          }
          if (this.CDBaseline_CannotIncrease != null)
          {
            Decimal num15 = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str11)) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str12));
            Decimal num16;
            if ((key == 64 || this.CDBaseline_CannotIncrease.GetDisclosedField(str6) == "Y" || this.CDBaseline_CannotIncrease.GetDisclosedField(str9) == "Y") && this.CDBaseline_CannotIncrease.GetDisclosedField(str10) != "Y")
            {
              num3 += num15;
              num16 = Math.Max(0M, num10 - num15);
            }
            else
              num16 = Math.Max(0M, num10 - 0M);
            num6 += num16;
            if (num16 > 0M)
              str2 = str2 + str11 + ",";
          }
          if (this.LELatest != null)
          {
            Decimal num17 = this.toDecimal(this.LELatest.GetDisclosedField(str11)) - this.toDecimal(this.LELatest.GetDisclosedField(str12));
            if (num17 > 0M && (key == 64 || this.LELatest.GetDisclosedField(str6) == "Y" || this.LELatest.GetDisclosedField(str9) == "Y") && this.LELatest.GetDisclosedField(str10) != "Y")
              num7 += num17;
          }
          if (this.CDLatest != null)
          {
            Decimal num18 = this.toDecimal(this.CDLatest.GetDisclosedField(str11)) - this.toDecimal(this.CDLatest.GetDisclosedField(str12));
            if (num18 > 0M && (key == 64 || this.CDLatest.GetDisclosedField(str6) == "Y" || this.CDLatest.GetDisclosedField(str9) == "Y") && this.CDLatest.GetDisclosedField(str10) != "Y")
              num8 += num18;
          }
        }
      }
      this.cannotIncreaseVarianceFieldIDs = this.CDBaseline_CannotIncrease == null ? this.cannotIncreaseVarianceFieldIDs + str1 : this.cannotIncreaseVarianceFieldIDs + str2;
      return new Decimal[8]
      {
        num1,
        num2,
        num3,
        num4,
        num5,
        num6,
        num7,
        num8
      };
    }

    private bool feeIsNotChangedExceptTransferTaxes(
      IDisclosureTracking2015Log initialLog,
      IDisclosureTracking2015Log log,
      bool checkLE)
    {
      List<string> stringList = new List<string>()
      {
        "",
        "Lender",
        "Broker",
        "Investor",
        "Affiliate"
      };
      Dictionary<int, int> dictionary1 = new Dictionary<int, int>()
      {
        {
          28,
          60
        },
        {
          62,
          62
        },
        {
          65,
          65
        },
        {
          67,
          72
        },
        {
          84,
          106
        },
        {
          119,
          126
        }
      };
      string field = this.loan.GetField("1172");
      string str = "NEWHUD2.X44";
      foreach (KeyValuePair<int, int> keyValuePair in dictionary1)
      {
        for (int key = keyValuePair.Key; key <= keyValuePair.Value; ++key)
        {
          if (key != 99 && key != 90)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key];
            string fieldId1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string fieldId2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            if ((key >= 28 && key <= 58 || key == 65 || key == 62 && (field == "FHA" || field == "VA" || field == "FarmersHomeAdministration")) && this.toDecimal(initialLog.GetDisclosedField(fieldId1)) - this.toDecimal(initialLog.GetDisclosedField(fieldId2)) != this.toDecimal(log.GetDisclosedField(fieldId1)) - this.toDecimal(log.GetDisclosedField(fieldId2)))
              return false;
            if (key >= 82 && key <= 106 || key == 62 && field != "FHA" && field != "VA" && field != "FarmersHomeAdministration")
            {
              Decimal num1 = 0M;
              Decimal num2 = num1;
              if (initialLog.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || initialLog.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                num1 = this.toDecimal(initialLog.GetDisclosedField(fieldId1)) - this.toDecimal(initialLog.GetDisclosedField(fieldId2));
              if (log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                num2 = this.toDecimal(log.GetDisclosedField(fieldId1)) - this.toDecimal(log.GetDisclosedField(fieldId2));
              if (num1 != num2)
                return false;
              Decimal num3 = 0M;
              Decimal num4 = num3;
              if (initialLog.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && stringList.Contains(initialLog.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                num3 = this.toDecimal(initialLog.GetDisclosedField(fieldId1)) - this.toDecimal(initialLog.GetDisclosedField(fieldId2));
              if (log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y" && stringList.Contains(log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                num4 = this.toDecimal(log.GetDisclosedField(fieldId1)) - this.toDecimal(log.GetDisclosedField(fieldId2));
              if (num3 != num4)
                return false;
            }
            if (key >= 119 && key <= 126 || key >= 67 && key <= 72)
            {
              string homeID = str + (15 - (67 - key) * 2).ToString();
              string propertyID = str + (35 - (67 - key)).ToString();
              string otherID = str + (16 - (67 - key) * 2).ToString();
              Decimal num5 = 0M;
              Decimal num6 = num5;
              if (stringList.Contains(initialLog.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, initialLog) && initialLog.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                num5 = this.toDecimal(initialLog.GetDisclosedField(fieldId1)) - this.toDecimal(initialLog.GetDisclosedField(fieldId2));
              if (stringList.Contains(log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, log) && log.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                num6 = this.toDecimal(log.GetDisclosedField(fieldId1)) - this.toDecimal(log.GetDisclosedField(fieldId2));
              if (num5 != num6)
                return false;
            }
          }
        }
      }
      if (!this.evaluateUseNewComplianceFor900())
      {
        Dictionary<int, int> dictionary2 = new Dictionary<int, int>()
        {
          {
            64,
            64
          },
          {
            67,
            72
          }
        };
        foreach (KeyValuePair<int, int> keyValuePair in dictionary2)
        {
          for (int key = keyValuePair.Key; key <= keyValuePair.Value; ++key)
          {
            Decimal num7 = 0M;
            Decimal num8 = num7;
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key];
            string fieldId3 = str + (35 - (67 - key)).ToString();
            string fieldId4 = str + (16 - (67 - key) * 2).ToString();
            string fieldId5 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP];
            string fieldId6 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string fieldId7 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            if ((key == 64 || initialLog.GetDisclosedField(fieldId3) == "Y" || initialLog.GetDisclosedField(fieldId4) == "Y") && initialLog.GetDisclosedField(fieldId5) != "Y")
              num7 = this.toDecimal(initialLog.GetDisclosedField(fieldId6)) - this.toDecimal(initialLog.GetDisclosedField(fieldId7));
            if ((key == 64 || log.GetDisclosedField(fieldId3) == "Y" || log.GetDisclosedField(fieldId4) == "Y") && log.GetDisclosedField(fieldId5) != "Y")
              num8 = this.toDecimal(log.GetDisclosedField(fieldId6)) - this.toDecimal(log.GetDisclosedField(fieldId7));
            if (num7 != num8)
              return false;
          }
        }
      }
      return true;
    }

    private Decimal[] sectionBorCForCannotIncrease()
    {
      List<string> stringList = new List<string>()
      {
        "",
        "Lender",
        "Broker",
        "Investor",
        "Affiliate"
      };
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      Decimal num8 = 0M;
      Decimal num9 = 0M;
      Decimal num10 = 0M;
      string str1 = "";
      string str2 = "";
      Decimal num11 = 0M;
      Decimal num12 = 0M;
      string str3 = "";
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      if (this.CDBaseline_CannotIncrease != null)
        str3 = "CD";
      else if (this.LEBaseline_CannotIncrease != null)
        str3 = "LE";
      else if (this.LEInitial != null)
        str3 = "LE";
      if (str3 == "")
        return (Decimal[]) null;
      dictionary.Add(28, 60);
      dictionary.Add(62, 62);
      dictionary.Add(65, 65);
      dictionary.Add(67, 72);
      dictionary.Add(84, 106);
      dictionary.Add(119, 126);
      dictionary.Add(138, 141);
      string field = this.loan.GetField("1172");
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        for (int key = keyValuePair.Key; key <= keyValuePair.Value; ++key)
        {
          if (key != 99 && key != 90)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key];
            string str4 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string str5 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            if (key >= 28 && key <= 58 || key == 65 || key == 62 && (field == "FHA" || field == "VA" || field == "FarmersHomeAdministration"))
            {
              if (this.LEInitial != null)
                num1 += this.toDecimal(this.LEInitial.GetDisclosedField(str4)) - this.toDecimal(this.LEInitial.GetDisclosedField(str5));
              Decimal num13 = this.toDecimal(this.loan.GetField(str4)) - this.toDecimal(this.loan.GetField(str5));
              num7 += num13;
              if (this.LEBaseline_CannotIncrease != null)
              {
                Decimal num14 = this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str5));
                num3 += num14;
                Decimal num15 = Math.Max(0M, num13 - num14);
                num9 += num15;
                if (num15 > 0M)
                  str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
              }
              if (this.CDBaseline_CannotIncrease != null)
              {
                Decimal num16 = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str5));
                num5 += num16;
                Decimal num17 = Math.Max(0M, num13 - num16);
                num10 += num17;
                if (num17 > 0M)
                  str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
              }
              if (this.LELatest != null)
                num11 += this.toDecimal(this.LELatest.GetDisclosedField(str4)) - this.toDecimal(this.LELatest.GetDisclosedField(str5));
              if (this.CDLatest != null)
                num12 += this.toDecimal(this.CDLatest.GetDisclosedField(str4)) - this.toDecimal(this.CDLatest.GetDisclosedField(str5));
            }
            if (key == 59 || key == 60)
            {
              if (this.CDBaseline_CannotIncrease != null)
              {
                Decimal val2 = 0M;
                if (!this.skipLenderObligatedFee(strArray[0], (IDisclosureTracking2015Log) null))
                  val2 = this.toDecimal(this.loan.GetField(str4)) - this.toDecimal(this.loan.GetField(str5));
                num7 += val2;
                Decimal num18 = 0M;
                if (!this.skipLenderObligatedFee(strArray[0], this.CDBaseline_CannotIncrease))
                  num18 = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str5));
                num5 += num18;
                Decimal num19 = Math.Max(0M, val2 - num18);
                num10 += num19;
                if (num19 > 0M)
                  str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                num9 += Math.Max(0M, val2);
                if (val2 > 0M)
                  str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
              }
              if (this.CDLatest != null && !this.skipLenderObligatedFee(strArray[0], this.CDLatest))
                num12 += this.toDecimal(this.CDLatest.GetDisclosedField(str4)) - this.toDecimal(this.CDLatest.GetDisclosedField(str5));
            }
            else
            {
              if (key >= 82 && key <= 106 || key == 62 && field != "FHA" && field != "VA" && field != "FarmersHomeAdministration" || key >= 138 && key <= 141 && (this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "N" || this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == ""))
              {
                Decimal val2 = 0M;
                if (this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                {
                  val2 = this.toDecimal(this.loan.GetField(str4)) - this.toDecimal(this.loan.GetField(str5));
                  num7 += val2;
                }
                else if (stringList.Contains(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                {
                  val2 = this.toDecimal(this.loan.GetField(str4)) - this.toDecimal(this.loan.GetField(str5));
                  num8 += val2;
                }
                if (this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                  num1 += this.toDecimal(this.LEInitial.GetDisclosedField(str4)) - this.toDecimal(this.LEInitial.GetDisclosedField(str5));
                else if (stringList.Contains(this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                  num2 += this.toDecimal(this.LEInitial.GetDisclosedField(str4)) - this.toDecimal(this.LEInitial.GetDisclosedField(str5));
                if (this.LEBaseline_CannotIncrease != null)
                {
                  if (this.LEBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || this.LEBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                  {
                    Decimal num20 = this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str5));
                    num3 += num20;
                    Decimal num21 = Math.Max(0M, val2 - num20);
                    num9 += num21;
                    if (num21 > 0M)
                      str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                  else if (stringList.Contains(this.LEBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                  {
                    Decimal num22 = this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str5));
                    num4 += num22;
                    Decimal num23 = Math.Max(0M, val2 - num22);
                    num9 += num23;
                    if (num23 > 0M)
                      str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                  else
                  {
                    Decimal num24 = Math.Max(0M, val2);
                    num9 += num24;
                    if (num24 > 0M)
                      str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                }
                if (this.CDBaseline_CannotIncrease != null)
                {
                  if (this.CDBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || this.CDBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                  {
                    Decimal num25 = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str5));
                    num5 += num25;
                    Decimal num26 = Math.Max(0M, val2 - num25);
                    num10 += num26;
                    if (num26 > 0M)
                      str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                  else if (stringList.Contains(this.CDBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                  {
                    Decimal num27 = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str5));
                    num6 += num27;
                    Decimal num28 = Math.Max(0M, val2 - num27);
                    num10 += num28;
                    if (num28 > 0M)
                      str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                  else
                  {
                    Decimal num29 = Math.Max(0M, val2);
                    num10 += num29;
                    if (num29 > 0M)
                      str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                }
                if (this.LELatest != null)
                {
                  if (this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                    num11 += this.toDecimal(this.LELatest.GetDisclosedField(str4)) - this.toDecimal(this.LELatest.GetDisclosedField(str5));
                  else if (stringList.Contains(this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                    num11 += this.toDecimal(this.LELatest.GetDisclosedField(str4)) - this.toDecimal(this.LELatest.GetDisclosedField(str5));
                }
                if (this.CDLatest != null)
                {
                  if (this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                    num12 += this.toDecimal(this.CDLatest.GetDisclosedField(str4)) - this.toDecimal(this.CDLatest.GetDisclosedField(str5));
                  else if (stringList.Contains(this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                    num12 += this.toDecimal(this.CDLatest.GetDisclosedField(str4)) - this.toDecimal(this.CDLatest.GetDisclosedField(str5));
                }
              }
              if (key >= 119 && key <= 126 || key >= 67 && key <= 72)
              {
                int num30 = 15 - (67 - key) * 2;
                string homeID = "NEWHUD2.X44" + num30.ToString();
                num30 = 35 - (67 - key);
                string propertyID = "NEWHUD2.X44" + num30.ToString();
                num30 = 16 - (67 - key) * 2;
                string otherID = "NEWHUD2.X44" + num30.ToString();
                Decimal val2 = 0M;
                if (stringList.Contains(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, (IDisclosureTracking2015Log) null) && this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                {
                  val2 = this.toDecimal(this.loan.GetField(str4)) - this.toDecimal(this.loan.GetField(str5));
                  num8 += val2;
                }
                if (stringList.Contains(this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.LEInitial) && this.LEInitial.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                  num2 += this.toDecimal(this.LEInitial.GetDisclosedField(str4)) - this.toDecimal(this.LEInitial.GetDisclosedField(str5));
                if (this.LEBaseline_CannotIncrease != null)
                {
                  if (stringList.Contains(this.LEBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.LEBaseline_CannotIncrease) && this.LEBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                  {
                    Decimal num31 = this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.LEBaseline_CannotIncrease.GetDisclosedField(str5));
                    num4 += num31;
                    Decimal num32 = Math.Max(0M, val2 - num31);
                    num9 += num32;
                    if (num32 > 0M)
                      str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                  else
                  {
                    Decimal num33 = Math.Max(0M, val2);
                    num9 += num33;
                    if (num33 > 0M)
                      str1 = str1 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                }
                if (this.CDBaseline_CannotIncrease != null)
                {
                  if (stringList.Contains(this.CDBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.CDBaseline_CannotIncrease) && this.CDBaseline_CannotIncrease.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                  {
                    Decimal num34 = this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str4)) - this.toDecimal(this.CDBaseline_CannotIncrease.GetDisclosedField(str5));
                    num6 += num34;
                    Decimal num35 = Math.Max(0M, val2 - num34);
                    num10 += num35;
                    if (num35 > 0M)
                      str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                  else
                  {
                    Decimal num36 = Math.Max(0M, val2);
                    num10 += num36;
                    if (num36 > 0M)
                      str2 = str2 + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] + ",";
                  }
                }
                if (this.LELatest != null && stringList.Contains(this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.LELatest) && this.LELatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                  num11 += this.toDecimal(this.LELatest.GetDisclosedField(str4)) - this.toDecimal(this.LELatest.GetDisclosedField(str5));
                if (this.CDLatest != null && stringList.Contains(this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])) && (key < 67 || key > 72 || this.evaluateCannotIncreaseFor900(homeID, propertyID, otherID, this.CDLatest) && this.CDLatest.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y"))
                  num12 += this.toDecimal(this.CDLatest.GetDisclosedField(str4)) - this.toDecimal(this.CDLatest.GetDisclosedField(str5));
              }
            }
          }
        }
      }
      this.cannotIncreaseVarianceFieldIDs = this.CDBaseline_CannotIncrease == null ? this.cannotIncreaseVarianceFieldIDs + str1 : this.cannotIncreaseVarianceFieldIDs + str2;
      return new Decimal[12]
      {
        num1,
        num2,
        num3,
        num4,
        num5,
        num6,
        num7,
        num8,
        num9,
        num10,
        num11,
        num12
      };
    }

    public string SetTotalLenderCredit(IDisclosureTracking2015Log logNew)
    {
      Decimal val1 = this.toDecimal(logNew.GetDisclosedField("NEWHUD.X1144")) + this.toDecimal(logNew.GetDisclosedField("NEWHUD.X1146")) + this.toDecimal(logNew.GetDisclosedField("NEWHUD.X1148"));
      Decimal num1 = 0M;
      Decimal num2 = !(logNew.GetDisclosedField("4796") == "Y") ? num1 + (logNew.GetDisclosedField("202") == "LenderCredit" ? this.toDecimal(logNew.GetDisclosedField("141")) : 0M) + (logNew.GetDisclosedField("1091") == "LenderCredit" ? this.toDecimal(logNew.GetDisclosedField("1095")) : 0M) + (logNew.GetDisclosedField("1106") == "LenderCredit" ? this.toDecimal(logNew.GetDisclosedField("1115")) : 0M) + (logNew.GetDisclosedField("1646") == "LenderCredit" ? this.toDecimal(logNew.GetDisclosedField("1647")) : 0M) : num1 + this.toDecimal(logNew.GetDisclosedField("4794"));
      Decimal val2 = 0M;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        if (this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) != "Broker")
          val2 += this.toDecimal(logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]));
      }
      Decimal num3 = 0M;
      if (logNew.DisclosedForCD)
        num3 = this.toDecimal(logNew.GetDisclosedField("FV.X366"));
      return (num2 + Math.Max(val1, val2) + num3).ToString();
    }

    public string SetCannotIncrease(IDisclosureTracking2015Log logNew)
    {
      List<string> stringList = new List<string>()
      {
        "",
        "Lender",
        "Broker",
        "Investor",
        "Affiliate"
      };
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      dictionary.Add(28, 60);
      dictionary.Add(62, 62);
      dictionary.Add(65, 65);
      dictionary.Add(82, 106);
      dictionary.Add(119, 126);
      string field = this.loan.GetField("1172");
      for (int index = 4; index <= 21; ++index)
      {
        if (index != 9)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          num1 += this.toDecimal(logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT])) - this.toDecimal(logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]));
        }
      }
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        for (int key = keyValuePair.Key; key <= keyValuePair.Value; ++key)
        {
          if (key != 99 && key != 90)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[key];
            string fieldId1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string fieldId2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            if (key >= 28 && key <= 58 || key == 65)
              num2 += this.toDecimal(logNew.GetDisclosedField(fieldId1)) - this.toDecimal(logNew.GetDisclosedField(fieldId2));
            if (key == 59 || key == 60)
            {
              num3 += this.toDecimal(logNew.GetDisclosedField(fieldId1)) - this.toDecimal(logNew.GetDisclosedField(fieldId2));
            }
            else
            {
              if (key == 62 && (field == "FHA" || field == "VA" || field == "FarmersHomeAdministration"))
                num2 += this.toDecimal(logNew.GetDisclosedField(fieldId1)) - this.toDecimal(logNew.GetDisclosedField(fieldId2));
              if (key >= 82 && key <= 106 || key == 62 && field != "FHA" && field != "VA" && field != "FarmersHomeAdministration")
              {
                if (logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "N" || logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "")
                  num2 += this.toDecimal(logNew.GetDisclosedField(fieldId1)) - this.toDecimal(logNew.GetDisclosedField(fieldId2));
                else if (stringList.Contains(logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                  num3 += this.toDecimal(logNew.GetDisclosedField(fieldId1)) - this.toDecimal(logNew.GetDisclosedField(fieldId2));
              }
              if (key >= 119 && key <= 126 && stringList.Contains(logNew.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO])))
                num3 += this.toDecimal(logNew.GetDisclosedField(fieldId1)) - this.toDecimal(logNew.GetDisclosedField(fieldId2));
            }
          }
        }
      }
      Decimal num4 = 0M;
      foreach (KeyValuePair<string, string> keyValuePair in new Dictionary<string, string>()
      {
        {
          "NEWHUD2.X2247,NEWHUD2.X2275",
          ""
        },
        {
          "NEWHUD2.X2346,NEWHUD2.X2374",
          "NEWHUD2.X4435"
        },
        {
          "NEWHUD2.X2379,NEWHUD2.X2407",
          "NEWHUD2.X4436"
        },
        {
          "NEWHUD2.X2412,NEWHUD2.X2440",
          "NEWHUD2.X4437"
        },
        {
          "NEWHUD2.X2445,NEWHUD2.X2473",
          "NEWHUD2.X4438"
        },
        {
          "NEWHUD2.X2478,NEWHUD2.X2506",
          "NEWHUD2.X4439"
        },
        {
          "NEWHUD2.X2511,NEWHUD2.X2539",
          "NEWHUD2.X4440"
        }
      })
      {
        string fieldId3 = keyValuePair.Key.Split(',')[0];
        string fieldId4 = keyValuePair.Key.Split(',')[1];
        Decimal num5 = this.toDecimal(logNew.GetDisclosedField(fieldId3)) - this.toDecimal(logNew.GetDisclosedField(fieldId4));
        if (num5 > 0M && (keyValuePair.Value == "" || keyValuePair.Value != "" && logNew.GetDisclosedField(keyValuePair.Value) == "Y"))
          num4 += num5;
      }
      return (num1 + num2 + num3 + (this.toDecimal(logNew.GetDisclosedField("NEWHUD2.X3666")) - this.toDecimal(logNew.GetDisclosedField("NEWHUD2.X3695"))) + (this.toDecimal(logNew.GetDisclosedField("NEWHUD2.X3699")) - this.toDecimal(logNew.GetDisclosedField("NEWHUD2.X3728"))) + (this.toDecimal(logNew.GetDisclosedField("NEWHUD2.X3732")) - this.toDecimal(logNew.GetDisclosedField("NEWHUD2.X3761"))) + num4).ToString();
    }

    private Decimal getSumOfRange(IDisclosureTracking2015Log versionDT, int start, int end)
    {
      Decimal sumOfRange = 0M;
      if (versionDT != null)
      {
        for (int index = start; index <= end; ++index)
        {
          if (index != 9)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
            string fieldId1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string fieldId2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            sumOfRange += this.toDecimal(versionDT.GetDisclosedField(fieldId1)) - this.toDecimal(versionDT.GetDisclosedField(fieldId2));
          }
        }
      }
      else
      {
        for (int index = start; index <= end; ++index)
        {
          if (index != 9)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
            string id1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
            string id2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
            sumOfRange += this.toDecimal(this.loan.GetField(id1)) - this.toDecimal(this.loan.GetField(id2));
          }
        }
      }
      return sumOfRange;
    }

    private Decimal getVariance(IDisclosureTracking2015Log versionDT, int start, int end)
    {
      Decimal variance = 0M;
      for (int index = start; index <= end; ++index)
      {
        if (index != 9)
        {
          string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
          string str1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
          string str2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
          variance += Math.Max(0M, this.toDecimal(this.loan.GetField(str1)) - this.toDecimal(this.loan.GetField(str2)) - (this.toDecimal(versionDT.GetDisclosedField(str1)) - this.toDecimal(versionDT.GetDisclosedField(str2))));
        }
      }
      return variance;
    }

    private string getVarianceLines(
      IDisclosureTracking2015Log versionDT,
      int start,
      int end,
      bool decrease = false,
      bool percent = false)
    {
      string varianceLines = "";
      for (int index = start; index <= end; ++index)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        string str1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
        string str2 = strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
        if ((!decrease ? (!percent ? Math.Max(0M, this.toDecimal(this.loan.GetField(str1)) - this.toDecimal(this.loan.GetField(str2)) - (this.toDecimal(versionDT.GetDisclosedField(str1)) - this.toDecimal(versionDT.GetDisclosedField(str2)))) : Math.Max(0M, this.toDecimal(this.loan.GetField(str1)) - this.toDecimal(this.loan.GetField(str2)) - (this.toDecimal(versionDT.GetDisclosedField(str1)) - this.toDecimal(versionDT.GetDisclosedField(str2))) * this.toDecimal("1.1"))) : Math.Max(0M, this.toDecimal(versionDT.GetDisclosedField(str1)) - this.toDecimal(versionDT.GetDisclosedField(str2)) - (this.toDecimal(this.loan.GetField(str1)) - this.toDecimal(this.loan.GetField(str2))))) > 0M)
          varianceLines = varianceLines + str1 + ",";
      }
      return varianceLines;
    }

    private string getVarianceLinesLP(
      IDisclosureTracking2015Log versionDT,
      int start,
      int end,
      bool decrease = false,
      bool percent = false)
    {
      string varianceLinesLp = "";
      for (int index = start; index <= end; ++index)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        if (!(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] == "NEWHUD.X1149"))
        {
          Decimal num1 = 0M;
          if (!this.skipLenderObligatedFee(strArray[0], (IDisclosureTracking2015Log) null))
            num1 = this.toDecimal(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]));
          Decimal num2 = 0M;
          if (!this.skipLenderObligatedFee(strArray[0], versionDT))
            num2 = this.toDecimal(versionDT.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID]));
          if ((!decrease ? (!percent ? Math.Max(0M, num1 - num2) : Math.Max(0M, num1 - num2 * this.toDecimal("1.1"))) : (!(this.loan.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) == "Broker") || !(versionDT.GetDisclosedField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) != "Broker") ? Math.Max(0M, num2 - num1) : Math.Max(0M, num2 - 0M))) > 0M)
            varianceLinesLp = varianceLinesLp + strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID] + ",";
        }
      }
      return varianceLinesLp;
    }

    private string getVarianceLine(
      IDisclosureTracking2015Log versionDT,
      string fieldID,
      bool decrease = false,
      bool percent = false)
    {
      return this.getVarianceLine(versionDT, fieldID, (string) null, decrease, percent);
    }

    private string getVarianceLine(
      IDisclosureTracking2015Log versionDT,
      string fieldID,
      string sellerObligateFieldID,
      bool decrease = false,
      bool percent = false)
    {
      string varianceLine = "";
      if ((versionDT == this.CDBaseline_CannotIncrease10 || versionDT == this.LEBaseline_CannotIncrease10 ? (!decrease ? (string.IsNullOrEmpty(sellerObligateFieldID) ? Math.Max(0M, this.toDecimal(this.loan.GetField(fieldID)) - this.toDecimal(versionDT.GetDisclosedField(fieldID))) : Math.Max(0M, this.toDecimal(this.loan.GetField(fieldID)) - this.toDecimal(this.loan.GetField(sellerObligateFieldID)) - (this.toDecimal(versionDT.GetDisclosedField(fieldID)) - this.toDecimal(versionDT.GetDisclosedField(sellerObligateFieldID))))) : Math.Max(0M, this.toDecimal(versionDT.GetDisclosedField(fieldID)) - this.toDecimal(this.loan.GetField(fieldID)))) : (!decrease ? (!percent ? (string.IsNullOrEmpty(sellerObligateFieldID) ? Math.Max(0M, this.toDecimal(this.loan.GetField(fieldID)) - this.toDecimal(versionDT.GetDisclosedField(fieldID))) : Math.Max(0M, this.toDecimal(this.loan.GetField(fieldID)) - this.toDecimal(this.loan.GetField(sellerObligateFieldID)) - (this.toDecimal(versionDT.GetDisclosedField(fieldID)) - this.toDecimal(versionDT.GetDisclosedField(sellerObligateFieldID))))) : Math.Max(0M, this.toDecimal(this.loan.GetField(fieldID)) - this.toDecimal(versionDT.GetDisclosedField(fieldID)) * this.toDecimal("1.1"))) : Math.Max(0M, this.toDecimal(versionDT.GetDisclosedField(fieldID)) - this.toDecimal(this.loan.GetField(fieldID))))) > 0M)
        varianceLine = varianceLine + fieldID + ",";
      return varianceLine;
    }

    private Decimal toDecimal(string value)
    {
      Decimal result = 0M;
      Decimal.TryParse(value, out result);
      return result;
    }

    public Hashtable GetGFFVarianceAlertDetails()
    {
      this.calculateFeeVariance(false);
      Hashtable triggerFields = new Hashtable();
      ArrayList arrayList1 = new ArrayList();
      IDisclosureTracking2015Log dtBaseline1 = (IDisclosureTracking2015Log) null;
      string fieldId = "";
      string description = "";
      string initialLEValue = "";
      string baseline = "";
      string disclosedValue = "";
      string itemizationValue = "";
      string varianceValue = "";
      string varianceLimit = "";
      Hashtable hashtable = new Hashtable();
      if (this.CDBaseline_CannotDecrease != null)
      {
        if (this.loan.GetField("FV.X26") != "" && this.toDecimal(this.loan.GetField("FV.X26")) > 0M)
        {
          fieldId = "FV.X26";
          description = "Cannot Decrease";
          initialLEValue = this.loan.GetField("FV.X16");
          baseline = "CD [" + this.loan.GetField("FV.X3") + "]";
          disclosedValue = this.loan.GetField("FV.X22");
          itemizationValue = this.loan.GetField("FV.X19");
          varianceValue = this.loan.GetField("FV.X26");
          varianceLimit = "Cannot Decrease";
          dtBaseline1 = this.CDBaseline_CannotDecrease;
        }
      }
      else if (this.LEBaseline_CannotDecrease != null && this.loan.GetField("FV.X24") != "" && this.toDecimal(this.loan.GetField("FV.X24")) > 0M)
      {
        fieldId = "FV.X24";
        description = "Cannot Decrease";
        initialLEValue = this.loan.GetField("FV.X16");
        baseline = "LE [" + this.loan.GetField("FV.X2") + "]";
        disclosedValue = this.loan.GetField("FV.X21");
        itemizationValue = this.loan.GetField("FV.X19");
        varianceValue = this.loan.GetField("FV.X24");
        varianceLimit = "Cannot Decrease";
        dtBaseline1 = this.LEBaseline_CannotDecrease;
      }
      if (dtBaseline1 != null)
      {
        arrayList1.Add((object) new GFFVAlertTriggerField(fieldId, description, initialLEValue, baseline, disclosedValue, itemizationValue, varianceValue, varianceLimit));
        Hashtable descriptionTable = this.getFeeDescriptionTable(((IEnumerable<string>) this.cannotDecreaseVarianceFieldIDs.Split(',')).Distinct<string>().ToArray<string>());
        string varianceFieldIds = this.cannotDecreaseVarianceFieldIDs;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in ((IEnumerable<string>) varianceFieldIds.Split(chArray)).Distinct<string>().ToArray<string>())
        {
          if (str.Trim() != "")
            arrayList1.Add((object) this.getGoodFaithFeeVarianceField(str, descriptionTable[(object) str].ToString(), baseline, varianceLimit, dtBaseline1));
        }
        triggerFields.Add((object) "Cannot Decrease", (object) arrayList1);
      }
      IDisclosureTracking2015Log dtBaseline2 = (IDisclosureTracking2015Log) null;
      ArrayList arrayList2 = new ArrayList();
      if (this.CDBaseline_CannotIncrease != null)
      {
        if (this.loan.GetField("FV.X52") != "" && this.toDecimal(this.loan.GetField("FV.X52")) > 0M)
        {
          fieldId = "FV.X52";
          description = "Cannot Increase";
          initialLEValue = this.loan.GetField("FV.X46");
          baseline = "CD [" + this.loan.GetField("FV.X29") + "]";
          disclosedValue = this.loan.GetField("FV.X351");
          itemizationValue = this.loan.GetField("FV.X49");
          varianceValue = this.loan.GetField("FV.X52");
          varianceLimit = "Cannot Increase";
          dtBaseline2 = this.CDBaseline_CannotIncrease;
        }
      }
      else if (this.LEBaseline_CannotIncrease != null && this.loan.GetField("FV.X50") != "" && this.toDecimal(this.loan.GetField("FV.X50")) > 0M)
      {
        fieldId = "FV.X50";
        description = "Cannot Increase";
        initialLEValue = this.loan.GetField("FV.X46");
        baseline = "LE [" + this.loan.GetField("FV.X28") + "]";
        disclosedValue = this.loan.GetField("FV.X350");
        itemizationValue = this.loan.GetField("FV.X49");
        varianceValue = this.loan.GetField("FV.X50");
        varianceLimit = "Cannot Increase";
        dtBaseline2 = this.LEBaseline_CannotIncrease;
      }
      if (dtBaseline2 != null)
      {
        arrayList2.Add((object) new GFFVAlertTriggerField(fieldId, description, initialLEValue, baseline, disclosedValue, itemizationValue, varianceValue, varianceLimit));
        Hashtable descriptionTable = this.getFeeDescriptionTable(((IEnumerable<string>) this.cannotIncreaseVarianceFieldIDs.Split(',')).Distinct<string>().ToArray<string>());
        string varianceFieldIds = this.cannotIncreaseVarianceFieldIDs;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in ((IEnumerable<string>) varianceFieldIds.Split(chArray)).Distinct<string>().ToArray<string>())
        {
          if (str.Trim() != "")
            arrayList2.Add((object) this.getGoodFaithFeeVarianceField(str, descriptionTable[(object) str].ToString(), baseline, varianceLimit, dtBaseline2));
        }
        triggerFields.Add((object) "Cannot Increase", (object) arrayList2);
      }
      IDisclosureTracking2015Log dtBaseline3 = (IDisclosureTracking2015Log) null;
      ArrayList arrayList3 = new ArrayList();
      if (this.CDBaseline_CannotIncrease10 != null)
      {
        if (this.loan.GetField("FV.X210") != "" && this.toDecimal(this.loan.GetField("FV.X210")) > 0M)
        {
          fieldId = "FV.X210";
          description = "10% Variance";
          initialLEValue = this.loan.GetField("FV.X204");
          baseline = "CD [" + this.loan.GetField("FV.X55") + "]";
          disclosedValue = this.loan.GetField("FV.X355");
          itemizationValue = this.loan.GetField("FV.X207");
          varianceValue = this.loan.GetField("FV.X210");
          varianceLimit = "Cannot Increase > 10%";
          dtBaseline3 = this.CDBaseline_CannotIncrease10;
        }
      }
      else if (this.LEBaseline_CannotIncrease10 != null && this.loan.GetField("FV.X208") != "" && this.toDecimal(this.loan.GetField("FV.X208")) > 0M)
      {
        fieldId = "FV.X208";
        description = "10% Variance";
        initialLEValue = this.loan.GetField("FV.X204");
        baseline = "LE [" + this.loan.GetField("FV.X54") + "]";
        disclosedValue = this.loan.GetField("FV.X354");
        itemizationValue = this.loan.GetField("FV.X207");
        varianceValue = this.loan.GetField("FV.X208");
        varianceLimit = "Cannot Increase > 10%";
        dtBaseline3 = this.LEBaseline_CannotIncrease10;
      }
      if (dtBaseline3 != null)
      {
        arrayList3.Add((object) new GFFVAlertTriggerField(fieldId, description, initialLEValue, baseline, disclosedValue, itemizationValue, varianceValue, varianceLimit));
        Hashtable descriptionTable = this.getFeeDescriptionTable(((IEnumerable<string>) this.cannotIncrease10VarianceFieldIDs.Split(',')).Distinct<string>().ToArray<string>());
        string varianceFieldIds = this.cannotIncrease10VarianceFieldIDs;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in ((IEnumerable<string>) varianceFieldIds.Split(chArray)).Distinct<string>().ToArray<string>())
        {
          if (str.Trim() != "")
            arrayList3.Add((object) this.getGoodFaithFeeVarianceField(str, descriptionTable[(object) str].ToString(), baseline, varianceLimit, dtBaseline3));
        }
        triggerFields.Add((object) "10% Variance", (object) arrayList3);
      }
      this.loan.SyncAlertCoCRecords(triggerFields);
      return triggerFields;
    }

    private GFFVAlertTriggerField getGoodFaithFeeVarianceField(
      string feeFieldId,
      string fieldDesc,
      string baseline,
      string varianceLimit,
      IDisclosureTracking2015Log dtBaseline)
    {
      bool flag = baseline.StartsWith("LE");
      string disclosedValue = flag ? this.LELatest.GetDisclosedField(feeFieldId) : this.CDLatest.GetDisclosedField(feeFieldId);
      string field = this.loan.GetField(feeFieldId);
      string disclosedField = this.LEInitial.GetDisclosedField(feeFieldId);
      if (feeFieldId == "NEWHUD2.X3666" || feeFieldId == "NEWHUD2.X3699" || feeFieldId == "NEWHUD2.X3732")
      {
        string str1;
        switch (feeFieldId)
        {
          case "NEWHUD2.X3699":
            str1 = "NEWHUD2.X3728";
            break;
          case "NEWHUD2.X3732":
            str1 = "NEWHUD2.X3761";
            break;
          default:
            str1 = "NEWHUD2.X3695";
            break;
        }
        string str2 = str1;
        Decimal num1 = Utils.ParseDecimal((object) disclosedValue) - (flag ? Utils.ParseDecimal((object) this.LELatest.GetDisclosedField(str2)) : Utils.ParseDecimal((object) this.CDLatest.GetDisclosedField(str2)));
        if (num1 != 0M)
          disclosedValue = num1.ToString("N2");
        Decimal num2 = Utils.ParseDecimal((object) field) - Utils.ParseDecimal((object) this.loan.GetField(str2));
        if (num2 != 0M)
          field = num2.ToString("N2");
        Decimal num3 = Utils.ParseDecimal((object) disclosedField) - Utils.ParseDecimal((object) this.LEInitial.GetDisclosedField(str2));
        if (num3 != 0M)
          disclosedField = num3.ToString("N2");
      }
      return new GFFVAlertTriggerField(feeFieldId, fieldDesc, disclosedField, baseline, disclosedValue, field, "", varianceLimit);
    }

    private Hashtable getFeeDescriptionTable(string[] feeFields)
    {
      Hashtable descriptionTable = new Hashtable();
      List<GFEItem> gfeItemList = this.UseNewGFEHUD || this.UseNew2015GFEHUD ? GFEItemCollection.GFEItems2010 : GFEItemCollection.GFEItems;
      foreach (string feeField in feeFields)
      {
        if (!(feeField.Trim() == ""))
        {
          descriptionTable[(object) feeField] = (object) EncompassFields.GetDescription(feeField);
          for (int index1 = 0; index1 < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index1)
          {
            string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index1];
            if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] == feeField)
            {
              for (int index2 = 0; index2 < gfeItemList.Count; ++index2)
              {
                GFEItem gfeItem = gfeItemList[index2];
                if (gfeItem.PayeeFieldID == strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME])
                {
                  string str = gfeItem.LineNumber != 802 || !(gfeItem.ComponentID == "e") || !this.UseNew2015GFEHUD ? (gfeItem.Description.Length <= 4 || gfeItem.Description.StartsWith("NEWHUD.X") || gfeItem.Description.StartsWith("NEWHUD2.X") || gfeItem.Description.StartsWith("CD3.X") ? this.Val(gfeItem.Description) : gfeItem.Description) : this.Val("NEWHUD2.X928") + " % of Loan Amount (Points)";
                  if (feeField != "NEWHUD.X1149")
                  {
                    descriptionTable[(object) feeField] = (object) str;
                    break;
                  }
                  break;
                }
              }
              break;
            }
          }
        }
      }
      return descriptionTable;
    }

    public bool LEBaselineUsed_CannotDecrease => this.leBaselineUsed_CannotDecrease;

    public bool LEBaselineUsed_CannotIncrease => this.leBaselineUsed_CannotIncrease;

    public bool LEBaselineUsed_CannotIncrease10 => this.leBaselineUsed_CannotIncrease10;

    public string GetHelpTargetName() => "Fee Variance Worksheet";

    public void CalculateFeeVarianceTotals(string id)
    {
      if (id == "FV.X366")
        this.ReCal_ItemsThatCannotDecrease();
      if (id == "FV.X379" || id == "FV.X380")
        this.ReCal_ChargesThatCannotIncrease();
      if (!(id == "FV.X383") && !(id == "FV.X384"))
        return;
      this.ReCal_ChargesThatCannotIncreaseMoreThan10();
    }

    private void resetInvalidFeeReplacementTriggers()
    {
      this.replaceLEFee_Cannot_Descrease = this.replaceLEFee_Cannot_Increase = this.replaceLEFee_Cannot_Increase10 = this.replaceCDFee_Cannot_Descrease = this.replaceCDFee_Cannot_Increase = this.replaceCDFee_Cannot_Increase10 = true;
    }

    private void replaceFeesFromInvalidReason()
    {
      if (this.LEBaseline_CannotDecrease != null)
        this.LEBaseline_CannotDecrease.ResetLoanDataFromOtherLogs();
      if (this.LEBaseline_CannotIncrease != null)
        this.LEBaseline_CannotIncrease.ResetLoanDataFromOtherLogs();
      if (this.LEBaseline_CannotIncrease10 != null)
        this.LEBaseline_CannotIncrease10.ResetLoanDataFromOtherLogs();
      if (this.CDBaseline_CannotDecrease != null)
        this.CDBaseline_CannotDecrease.ResetLoanDataFromOtherLogs();
      if (this.CDBaseline_CannotIncrease != null)
        this.CDBaseline_CannotIncrease.ResetLoanDataFromOtherLogs();
      if (this.CDBaseline_CannotIncrease10 != null)
        this.CDBaseline_CannotIncrease10.ResetLoanDataFromOtherLogs();
      if (this.Val("4461") != "Y")
        return;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      List<IDisclosureTracking2015Log> logs1 = new List<IDisclosureTracking2015Log>();
      if (this.LEBaseline_CannotDecrease != null && this.replaceLEFee_Cannot_Descrease || this.LEBaseline_CannotIncrease != null && this.replaceLEFee_Cannot_Increase || this.LEBaseline_CannotIncrease10 != null && this.replaceLEFee_Cannot_Increase10)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
        {
          if (disclosureTracking2015Log.DisclosedForLE && (this.CDInitial == null || disclosureTracking2015Log.CompareDisclosedDate(this.CDInitial) <= 0))
            logs1.Insert(0, disclosureTracking2015Log);
        }
        if (this.LEBaseline_CannotDecrease != null && this.replaceLEFee_Cannot_Descrease && this.replaceFeeFromInvalidReason(logs1, this.LEBaseline_CannotDecrease, FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotDecrease))
          flag1 = true;
        if (this.LEBaseline_CannotIncrease != null && this.replaceLEFee_Cannot_Increase && this.replaceFeeFromInvalidReason(logs1, this.LEBaseline_CannotIncrease, FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotIncrease))
          flag2 = true;
        if (this.LEBaseline_CannotIncrease10 != null && this.replaceLEFee_Cannot_Increase10 && this.replaceFeeFromInvalidReason(logs1, this.LEBaseline_CannotIncrease10, FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotIncrease10))
          flag3 = true;
      }
      if (this.CDBaseline_CannotDecrease != null && this.replaceCDFee_Cannot_Descrease || this.CDBaseline_CannotIncrease != null && this.replaceCDFee_Cannot_Increase || this.CDBaseline_CannotIncrease10 != null && this.replaceCDFee_Cannot_Increase10)
      {
        List<IDisclosureTracking2015Log> logs2 = new List<IDisclosureTracking2015Log>();
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
          logs2.Insert(0, disclosureTracking2015Log);
        if (this.CDBaseline_CannotDecrease != null && this.replaceCDFee_Cannot_Descrease && this.replaceFeeFromInvalidReason(logs2, this.CDBaseline_CannotDecrease, FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotDecrease))
          flag1 = true;
        if (this.CDBaseline_CannotIncrease != null && this.replaceCDFee_Cannot_Increase && this.replaceFeeFromInvalidReason(logs2, this.CDBaseline_CannotIncrease, FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotIncrease))
          flag2 = true;
        if (this.CDBaseline_CannotIncrease10 != null && this.replaceCDFee_Cannot_Increase10 && this.replaceFeeFromInvalidReason(logs2, this.CDBaseline_CannotIncrease10, FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotIncrease10))
          flag3 = true;
      }
      if (flag1)
        this.cal_ItemsThatCannotDecrease((string) null, (string) null);
      if (flag2)
        this.cal_ChargesThatCannotIncrease((string) null, (string) null);
      if (!flag3)
        return;
      this.cal_ChargesThatCannotIncreaseMoreThan10((string) null, (string) null);
    }

    private bool replaceFeeFromInvalidReason(
      List<IDisclosureTracking2015Log> logs,
      IDisclosureTracking2015Log mainLog,
      FeeVarianceToolCalculation.feeVarianceCatetories feeVarianceCategory)
    {
      int changeOfCircumstance = mainLog.NumberOfGoodFaithChangeOfCircumstance;
      if (changeOfCircumstance == 0)
        return false;
      string str1 = "";
      switch (feeVarianceCategory)
      {
        case FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotDecrease:
          str1 = "|Interest Rate dependent charges (Rate Lock)|Revisions requested by the Consumer|Expiration (Intent to Proceed received after 10 business days)|Changed Circumstance - Eligibility|";
          break;
        case FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotIncrease:
        case FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotIncrease10:
          str1 = "|Interest Rate dependent charges (Rate Lock)|Changed Circumstance - Settlement Charges|Changed Circumstance - Eligibility|Revisions requested by the Consumer|Expiration (Intent to Proceed received after 10 business days)|Delayed Settlement on Construction Loans|";
          break;
        case FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotDecrease:
          str1 = "|Interest Rate dependent charges (Rate Lock)|Changed Circumstance - Eligibility|Revisions requested by the Consumer|";
          break;
        case FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotIncrease:
          str1 = "|Interest Rate dependent charges (Rate Lock)|Changed Circumstance - Settlement Charges|Changed Circumstance - Eligibility|Revisions requested by the Consumer|";
          break;
        case FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotIncrease10:
          str1 = "|Changed Circumstance - Settlement Charges|Changed Circumstance - Eligibility|Revisions requested by the Consumer|Interest Rate dependent charges (Rate Lock)|";
          break;
      }
      int num = -1;
      for (int index = 0; index < logs.Count; ++index)
      {
        if (logs[index] == mainLog)
        {
          num = index + 1;
          break;
        }
      }
      if (num < 0 || num >= logs.Count)
        return false;
      bool flag1 = false;
      for (int index1 = 1; index1 <= changeOfCircumstance; ++index1)
      {
        string disclosedField1 = mainLog.GetDisclosedField("XCOC" + index1.ToString("00") + "07");
        string disclosedField2 = mainLog.GetDisclosedField("XCOC" + index1.ToString("00") + "01");
        string disclosedField3 = mainLog.GetDisclosedField("XCOC" + index1.ToString("00") + "09");
        if (!(disclosedField2 == "") && (feeVarianceCategory != FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotDecrease && feeVarianceCategory != FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotDecrease || string.Compare(disclosedField3, "Cannot Decrease", true) == 0) && (feeVarianceCategory != FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotIncrease && feeVarianceCategory != FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotIncrease || string.Compare(disclosedField3, "Cannot Increase", true) == 0) && (feeVarianceCategory != FeeVarianceToolCalculation.feeVarianceCatetories.LE_CannotIncrease10 && feeVarianceCategory != FeeVarianceToolCalculation.feeVarianceCatetories.CD_CannotIncrease10 || string.Compare(disclosedField3, "Cannot Increase > 10%", true) == 0) && str1.IndexOf("|" + disclosedField1 + "|") == -1)
        {
          bool flag2 = false;
          for (int index2 = num; index2 < logs.Count; ++index2)
          {
            if (!this.allLogsWithAlertCoCs.ContainsKey((object) logs[index2]))
              this.allLogsWithAlertCoCs.Add((object) logs[index2], (object) this.getAllAlertCoCRecordsInLog(logs[index2]));
            Dictionary<string, string> logsWithAlertCoC = (Dictionary<string, string>) this.allLogsWithAlertCoCs[(object) logs[index2]];
            if (!logsWithAlertCoC.ContainsKey(disclosedField2))
            {
              flag2 = true;
            }
            else
            {
              string str2 = logsWithAlertCoC[disclosedField2];
              if (str1.IndexOf("|" + str2 + "|") > -1)
                flag2 = true;
            }
            if (flag2)
            {
              mainLog.SetLoanDataFromOtherLogs(disclosedField2, logs[index2].GetDisclosedField(disclosedField2, false));
              flag1 = true;
              break;
            }
          }
        }
      }
      return flag1;
    }

    private Dictionary<string, string> getAllAlertCoCRecordsInLog(IDisclosureTracking2015Log log)
    {
      Dictionary<string, string> alertCoCrecordsInLog = new Dictionary<string, string>();
      int changeOfCircumstance = log.NumberOfGoodFaithChangeOfCircumstance;
      for (int index = 1; index <= changeOfCircumstance; ++index)
      {
        string disclosedField = log.GetDisclosedField("XCOC" + index.ToString("00") + "01");
        if (!alertCoCrecordsInLog.ContainsKey(disclosedField))
          alertCoCrecordsInLog.Add(disclosedField, log.GetDisclosedField("XCOC" + index.ToString("00") + "07"));
      }
      return alertCoCrecordsInLog;
    }

    private enum feeVarianceCatetories
    {
      LE_CannotDecrease,
      LE_CannotIncrease,
      LE_CannotIncrease10,
      CD_CannotDecrease,
      CD_CannotIncrease,
      CD_CannotIncrease10,
    }
  }
}
