// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.ULDDExportCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class ULDDExportCalculation : CalculationBase
  {
    private SessionObjects sessionObjects;
    private static List<string> unconditionalRequiredFieldsFreddie = new List<string>((IEnumerable<string>) new string[51]
    {
      "ULDD.X150",
      "748",
      "19",
      "5",
      "HMDA.X13",
      "HMDA.X15",
      "3237",
      "3238",
      "356",
      "L770",
      "ULDD.X133",
      "745",
      "677",
      "675",
      "353",
      "976",
      "975",
      "1149",
      "1543",
      "11",
      "12",
      "14",
      "15",
      "1811",
      "18",
      "ULDD.FRE.X1172",
      "2",
      "608",
      "325",
      "3",
      "4",
      "420",
      "ULDD.X177",
      "16",
      "78",
      "736",
      "1742",
      "GLOBAL.S3",
      "ULDD.X11",
      "ULDD.X29",
      "ULDD.X42",
      "ULDD.X43",
      "ULDD.X54",
      "ULDD.X56",
      "ULDD.X89",
      "ULDD.X172",
      "1731",
      "3290",
      "ULDD.X21",
      "761",
      "ULDD.TotalMortgagedPropertiesCount"
    });
    private static List<string> unconditionalRequiredFieldsFannie = new List<string>((IEnumerable<string>) new string[51]
    {
      "ULDD.X150",
      "748",
      "19",
      "5",
      "HMDA.X13",
      "ULDD.FNM.HMDA.X15",
      "3237",
      "3238",
      "356",
      "L770",
      "745",
      "ULDD.X133",
      "745",
      "677",
      "675",
      "ULDD.FNM.MORNET.X75",
      "ULDD.FNM.MORNET.X76",
      "1149",
      "1543",
      "11",
      "12",
      "14",
      "15",
      "1811",
      "18",
      "2",
      "608",
      "325",
      "3",
      "4",
      "420",
      "ULDD.X177",
      "16",
      "78",
      "736",
      "1742",
      "GLOBAL.S3",
      "ULDD.FNM.975",
      "ULDD.FNM.X43",
      "ULDD.X1",
      "ULDD.X11",
      "ULDD.X29",
      "ULDD.X44",
      "ULDD.X54",
      "ULDD.X56",
      "ULDD.X172",
      "1731",
      "3290",
      "ULDD.X21",
      "761",
      "ULDD.TotalMortgagedPropertiesCount"
    });
    private static List<string> unconditionalRequiredFieldsGinnie = new List<string>((IEnumerable<string>) new string[29]
    {
      "2",
      "3",
      "5",
      "11",
      "12",
      "14",
      "15",
      "16",
      "ULDD.X187",
      "976",
      "608",
      "ULDD.GNM.GovUpFrontPrmAmt",
      "ULDD.GNM.GovUpFrontPrmPrcnt",
      "ULDD.X181",
      "L770",
      "353",
      "78",
      "325",
      "GLOBAL.S3",
      "742",
      "1172",
      "ULDD.GNM.MrtggPrgrmType",
      "ULDD.X4",
      "ULDD.GNM.MERSOrgnalMrtggeeOfRcrdIndctr",
      "ULDD.X3",
      "ULDD.X1",
      "1149",
      "ULDD.GNM.MortgageOriginator",
      "ULDD.GNM.X1172"
    });
    private static string[] calcAllTriggerFields = new string[44]
    {
      "MORNET.X76",
      "MORNET.X77",
      "HMDA.X15",
      "MORNET.X75",
      "975",
      "ULDD.X43",
      "ULDD.X50",
      "430",
      "ULDD.X70",
      "4000",
      "4004",
      "4001",
      "4005",
      "1859",
      "ULDD.X24",
      "ULDD.X29",
      "ULDD.X51",
      "ULDD.X89",
      "ULDD.X102",
      "ULDD.X106",
      "697",
      "695",
      "696",
      "694",
      "ULDD.X108",
      "ULDD.X119",
      "ULDD.X120",
      "2847",
      "1543",
      "1553",
      "ULDD.X134",
      "1172",
      "965",
      "466",
      "985",
      "467",
      "ULDD.X45",
      "ULDD.X86",
      "ULDD.X98",
      "ULDD.X173",
      "ULDD.X207",
      "ULDD.X208",
      "ULDD.X211",
      "ULDD.X212"
    };
    private static string[] calcMISMOImportTriggerFields = new string[2]
    {
      "1041",
      "934"
    };
    private List<string> validateResult = new List<string>();
    internal Routine CalcFannieMaeExportFields;
    internal Routine ConcatenateSfcCodes;
    internal Routine CopyAlienStatus;
    private readonly ULDDExportCalculationServant mUlddExportClCalculationServant;

    internal ULDDExportCalculation(
      SessionObjects sessionObjects,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calcObjs)
      : base(l, calcObjs)
    {
      this.sessionObjects = sessionObjects;
      this.CalcFannieMaeExportFields = this.RoutineX(new Routine(this.calculateFannieMaeExportFields));
      this.ConcatenateSfcCodes = this.RoutineX(new Routine(this.concatenateSfcCodes));
      this.CopyAlienStatus = this.RoutineX(new Routine(this.copyAlienStatus));
      this.addFieldHandlers(l);
      this.mUlddExportClCalculationServant = new ULDDExportCalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers(LoanData l)
    {
      this.AddFieldHandler("MORNET.X76", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.MORNET.X76", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("MORNET.X77", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.MORNET.X77", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("HMDA.X15", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.HMDA.X15", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("MORNET.X75", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("975", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.975", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X43", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.X43", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X50", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.X50", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("430", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.430", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("4000", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.4000", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("4004", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.4004", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("4001", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.4001", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("4005", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.4005", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1859", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.1859", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X70", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FNM.X70", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X24", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X29", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X51", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X89", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X102", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X106", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X108", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X119", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X120", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("2847", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1553", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1172", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("965", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("466", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("985", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("467", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X45", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X86", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X98", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X173", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("19", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1012", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1041", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X36", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X179", this.CalcFannieMaeExportFields);
      for (int index = 5; index <= 8; ++index)
        this.AddFieldHandler("URLA.X20" + index.ToString(), this.CalcFannieMaeExportFields);
      this.AddFieldHandler("338", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("SYS.X319", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X11", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("SERVICE.X13", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("SERVICE.X14", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("696", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("697", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("695", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("694", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("SERVICE.X32", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("SERVICE.X57", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("34", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("364", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPaymentType", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPmt2SourceType", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPmt2Type", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPmt3SourceType", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPmt3Type", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPmt4SourceType", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.FRE.DownPmt4Type", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X32", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1264", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X178", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("676", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1109", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("136", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("356", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1821", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X187", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.GNM.DwnPymntFndsType", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("232", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1149", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("403", this.calObjs.USDACal.CalcFirstTimeHomeBuyer + this.CalcFannieMaeExportFields);
      this.AddFieldHandler("1108", this.calObjs.USDACal.CalcFirstTimeHomeBuyer + this.CalcFannieMaeExportFields);
      this.AddFieldHandler("SFC0001", this.ConcatenateSfcCodes);
      this.AddFieldHandler("SFC0004", this.ConcatenateSfcCodes);
      this.AddFieldHandler("4709", this.CopyAlienStatus);
      this.AddFieldHandler("4710", this.CopyAlienStatus);
      this.AddFieldHandler("ULDD.X207", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X208", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X211", this.CalcFannieMaeExportFields);
      this.AddFieldHandler("ULDD.X212", this.CalcFannieMaeExportFields);
    }

    internal void CalculateAll()
    {
      foreach (string calcAllTriggerField in ULDDExportCalculation.calcAllTriggerFields)
        this.calculateFannieMaeExportFields(calcAllTriggerField, this.Val(calcAllTriggerField));
    }

    internal void CalculateMISMOImportField()
    {
      foreach (string importTriggerField in ULDDExportCalculation.calcMISMOImportTriggerFields)
        this.calculateFannieMaeExportFields(importTriggerField, this.Val(importTriggerField));
    }

    internal void calculateFannieMaeExportFields(string id, string val)
    {
      this.mUlddExportClCalculationServant.CalculateFannieMaeExportFields(id, val);
    }

    public static int ParseNumeric3(object value)
    {
      int num = !Utils.IsDouble(value) ? (!Utils.IsInt(value) ? ULDDExportCalculation.formatToInteger(Utils.ParseDouble(value, 0.0)) : Utils.ParseInt(value, 0)) : ULDDExportCalculation.formatToInteger(Utils.ParseDouble(value, 0.0));
      return num > 999 || num < 0 ? 0 : num;
    }

    public static Decimal ParseDecimal(object value, int maxLength, int decimalPlaces)
    {
      if (!Utils.IsDecimal(value) || maxLength <= decimalPlaces)
        return 0M;
      double num = Utils.ParseDouble(value, 0.0);
      int y = maxLength - decimalPlaces;
      if (num > Math.Pow(10.0, (double) y) || num < 0.0)
        return 0M;
      string format = "#.";
      for (int index = 0; index < decimalPlaces; ++index)
        format += "0";
      return Utils.ParseDecimal((object) num.ToString(format));
    }

    private static int formatToInteger(double rate)
    {
      rate -= 0.005;
      rate = Utils.ArithmeticRounding(rate, 2);
      rate += 0.49;
      rate = Utils.ArithmeticRounding(rate, 0);
      return Utils.ParseInt((object) rate.ToString());
    }

    internal List<string> FannieInvalidFields()
    {
      this.clearInvalidFieldList();
      foreach (string fieldID in ULDDExportCalculation.unconditionalRequiredFieldsFannie.ToArray())
        this.requireField(fieldID);
      this.appraisalInfoSectionChecking(true);
      this.borrowerInfoSectionChecking(true);
      this.constructionMortgageSectionChecking(true);
      this.declarationsSectionChecking(true);
      this.incomeAndExpensesSectionChecking(true);
      this.infoForGovMonitoringSectionChecking(true);
      this.loanDetailsSectionChecking(true);
      this.loanTermsAndAmountsSectionChecking(true);
      this.typesOfMortgageAndTermsOfLoan(true);
      this.loanToValueRatiosSectionChecking(true);
      this.mailingAddressSectionChecking(true);
      this.mortgageInsuranceSectionChecking(true);
      this.rateAdjustmentsSectionChecking(true);
      this.riskAssessmentSectionChecking(true);
      this.subjectPropertyInfoSectionChecking(true);
      this.transactionDetailsSectionChecking(true);
      this.escrowItemsSectionChecking(true);
      this.refinanceCashOutAmountChecking(true);
      return this.validateResult;
    }

    internal List<string> FreddieInvalidFields()
    {
      this.clearInvalidFieldList();
      foreach (string fieldID in ULDDExportCalculation.unconditionalRequiredFieldsFreddie.ToArray())
        this.requireField(fieldID);
      this.appraisalInfoSectionChecking(false);
      this.borrowerInfoSectionChecking(false);
      this.constructionMortgageSectionChecking(false);
      this.declarationsSectionChecking(false);
      this.incomeAndExpensesSectionChecking(false);
      this.infoForGovMonitoringSectionChecking(false);
      this.loanDetailsSectionChecking(false);
      this.loanTermsAndAmountsSectionChecking(false);
      this.typesOfMortgageAndTermsOfLoan(false);
      this.loanToValueRatiosSectionChecking(false);
      this.mailingAddressSectionChecking(false);
      this.mortgageInsuranceSectionChecking(false);
      this.rateAdjustmentsSectionChecking(false);
      this.riskAssessmentSectionChecking(false);
      this.subjectPropertyInfoSectionChecking(false);
      this.transactionDetailsSectionChecking(false);
      this.escrowItemsSectionChecking(false);
      this.refinanceCashOutAmountChecking(false);
      return this.validateResult;
    }

    internal List<string> GinnieInvalidFields()
    {
      this.clearInvalidFieldList();
      foreach (string fieldID in ULDDExportCalculation.unconditionalRequiredFieldsGinnie.ToArray())
        this.requireField(fieldID);
      this.ACHInfoSectionChecking();
      this.poolLoanSectionChecking();
      this.ginnieSubjectPropertyInfoSectionChecking();
      this.ginnieMortgageTypeLoanTermsInfoSectionChecking();
      this.ginnieDownPaymentInfoSectionChecking();
      this.ginnieAdditionalChecking();
      this.ginnieGovermentInformation();
      return this.validateResult;
    }

    private void appraisalInfoSectionChecking(bool isFannie)
    {
      if (isFannie && this.loan.GetField("ULDD.X29") == "Other")
        this.addInvalidField("ULDD.X29");
      string field = this.loan.GetField("ULDD.X29");
      if (!(field == "DriveBy") && !(field == "FullAppraisal") && !(field == "PriorAppraisalUsed"))
        return;
      this.requireField("974");
    }

    private void borrowerInfoSectionChecking(bool isFannie)
    {
      Borrower coBorrower = this.loan.GetBorrowerPairs()[0].CoBorrower;
      Borrower borrower = this.loan.GetBorrowerPairs()[0].Borrower;
      if (coBorrower != null && (coBorrower.FirstName != "" || coBorrower.LastName != ""))
      {
        this.requireField("ULDD.X151");
        if (this.loan.GetField("ULDD.X151") == "Individual")
        {
          if (isFannie)
            this.requireField("ULDD.FNM.4004");
          else
            this.requireField("4004");
          this.requireField("4006");
          this.requireField("1403");
          this.requireField("97");
          this.requireField("70");
        }
      }
      if (borrower != null && (borrower.FirstName != "" || borrower.LastName != ""))
        this.requireField("ULDD.X150");
      if (this.loan.GetField("ULDD.X150") == "Individual")
      {
        if (isFannie)
          this.requireField("ULDD.FNM.4000");
        else
          this.requireField("4000");
        this.requireField("4002");
        this.requireField("1402");
        this.requireField("65");
        this.requireField("38");
      }
      else
      {
        if (isFannie)
          this.requireField("ULDD.FNM.1859");
        else
          this.requireField("1859");
        this.requireField("1862");
        if (isFannie)
          this.requireField("ULDD.X120");
        else
          this.requireField("ULDD.X119");
      }
      if (isFannie)
      {
        if (!(this.loan.GetField("ULDD.X120") == "Other"))
          return;
        this.requireField("ULDD.X122");
      }
      else
      {
        if (!(this.loan.GetField("ULDD.X119") == "Other"))
          return;
        this.requireField("ULDD.X121");
      }
    }

    private void constructionMortgageSectionChecking(bool isFannie)
    {
      if (this.loan.GetField("19") == "ConstructionToPermanent")
        this.requireField("1963");
      if (!(this.loan.GetField("ULDD.X11") == "Simple"))
        return;
      this.requireField("SYS.X2");
    }

    private void declarationsSectionChecking(bool isFannie)
    {
      Borrower coBorrower = this.loan.GetBorrowerPairs()[0].CoBorrower;
      Borrower borrower = this.loan.GetBorrowerPairs()[0].Borrower;
      if (borrower != null && (borrower.FirstName != "" || borrower.LastName != ""))
      {
        if (this.loan.GetField("ULDD.X150") == "Individual")
        {
          this.requireField("965");
          this.requireField("466");
        }
        string field1 = this.loan.GetField("965", 0);
        string field2 = this.loan.GetField("466", 0);
        if (!isFannie && field1 != "Y" && field2 != "Y" && this.loan.GetField("ULDD.X150") == "Individual")
          this.requireField("ULDD.X123");
      }
      if (coBorrower == null || !(coBorrower.FirstName != "") && !(coBorrower.LastName != ""))
        return;
      if (this.loan.GetField("ULDD.X151") == "Individual")
      {
        this.requireField("985");
        this.requireField("467");
      }
      string field3 = this.loan.GetField("985", 0);
      string field4 = this.loan.GetField("467", 0);
      if (isFannie || !(field3 != "Y") || !(field4 != "Y") || !(this.loan.GetField("ULDD.X151") == "Individual"))
        return;
      this.requireField("ULDD.X148");
    }

    private void incomeAndExpensesSectionChecking(bool isFannie)
    {
    }

    private void infoForGovMonitoringSectionChecking(bool isFannie)
    {
      Borrower coBorrower = this.loan.GetBorrowerPairs()[0].CoBorrower;
      Borrower borrower = this.loan.GetBorrowerPairs()[0].Borrower;
      bool flag1 = false;
      if (borrower != null && (borrower.FirstName != "" || borrower.LastName != ""))
      {
        this.requireField("1523");
        for (int index = 1524; index < 1531; ++index)
        {
          if (this.loan.GetField(string.Concat((object) index)) == "Y")
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1)
        {
          for (int index = 1524; index < 1531; ++index)
            this.addInvalidField(string.Concat((object) index));
        }
      }
      if (coBorrower == null || !(coBorrower.FirstName != "") && !(coBorrower.LastName != ""))
        return;
      this.requireField("1531");
      bool flag2 = false;
      for (int index = 1532; index < 1539; ++index)
      {
        if (this.loan.GetField(string.Concat((object) index)) == "Y")
        {
          flag2 = true;
          break;
        }
      }
      if (flag2)
        return;
      for (int index = 1532; index < 1539; ++index)
        this.addInvalidField(string.Concat((object) index));
    }

    private void loanDetailsSectionChecking(bool isFannie)
    {
      if (this.loan.GetField("19") == "Purchase" && this.loan.GetField("1811") == "PrimaryResidence")
        this.requireField("934");
      if (this.loan.GetField("ULDD.X124") == "Y")
      {
        if (this.loan.GetField("1172") == "HELOC")
        {
          this.requireField("CASASRN.X167");
          this.requireField("1888");
        }
        if (isFannie)
          this.requireField("ULDD.X128");
        else
          this.requireField("ULDD.X127");
        this.requireField("ULDD.X132");
      }
      if (this.loan.GetField("1551") == "Y")
        this.requireField("2847");
      if (isFannie)
      {
        if (this.loan.GetField("1172") != "Conventional")
          this.requireField("1039");
        if (this.loan.GetField("608") == "AdjustableRate")
          this.requireField("5");
        if (this.loan.GetField("ULDD.X46") == "FNM")
          this.requireField("352");
        if (this.loan.GetField("ULDD.X125") == "Y")
        {
          this.requireField("428");
          this.requireField("ULDD.X131");
        }
      }
      else
      {
        string field = this.loan.GetField("1172");
        if (field == "FHA" || field == "FarmersHomeAdministration" || field == "Other")
          this.requireField("1039");
        if (this.loan.GetField("ULDD.X99") == "FRE")
          this.requireField("352");
        if (Utils.IsDouble((object) this.loan.GetField("1335")) && Utils.ParseDouble((object) this.loan.GetField("1335")) > 1.0)
          this.requireField("ULDD.FRE.DownPaymentType");
        if (this.loan.GetField("ULDD.FRE.DownPaymentType") == "OtherTypeOfDownPayment")
          this.requireField("ULDD.FRE.ExplanationofDownPayment");
      }
      if (!(this.loan.GetField("ULDD.X11") == "Compound"))
        return;
      this.addInvalidField("ULDD.X11");
    }

    private void loanTermsAndAmountsSectionChecking(bool isFannie)
    {
      if (isFannie)
        return;
      if (this.loan.GetField("ULDD.X91") == "Exercised")
      {
        this.requireField("5");
        this.requireField("682");
      }
      if (!(this.loan.GetField("1290") == "Y"))
        return;
      this.requireField("1401");
    }

    private void loanToValueRatiosSectionChecking(bool isFannie)
    {
      if (isFannie)
      {
        if (!(this.loan.GetField("1172") == "HELOC") || !(this.loan.GetField("ULDD.X126") == "Y"))
          return;
        this.requireField("MORNET.X77");
      }
      else
      {
        if (!(this.loan.GetField("1172") == "HELOC") || !(this.loan.GetField("ULDD.X126") == "Y"))
          return;
        this.requireField("1540");
      }
    }

    private void mailingAddressSectionChecking(bool isFannie)
    {
      Borrower borrower = this.loan.GetBorrowerPairs()[0].Borrower;
      if (borrower == null || !(borrower.FirstName != "") && !(borrower.LastName != "") || !(this.loan.GetField("ULDD.X26") != "Y"))
        return;
      this.requireField("1417");
      string field = this.loan.GetField("ULDD.X27");
      if (field == "US" || field == "CA")
      {
        this.requireField("1419");
        this.requireField("1418");
      }
      this.requireField("1416");
    }

    private void mortgageInsuranceSectionChecking(bool isFannie)
    {
      if (this.loan.GetField("VEND.X167") != "")
      {
        if (isFannie)
          this.requireField("ULDD.FNM.430");
        else
          this.requireField("430");
        this.requireField("ULDD.X134");
      }
      if (isFannie)
      {
        if (this.loan.GetField("ULDD.X134") == "Other")
          this.requireField("ULDD.X136");
        if (!(this.loan.GetField("ULDD.X52") == "NoMIBasedOnInvestorRequirements"))
          return;
        this.addInvalidField("ULDD.X52");
      }
      else
      {
        if (!(this.loan.GetField("ULDD.X134") == "Other"))
          return;
        this.requireField("ULDD.X135");
      }
    }

    private void rateAdjustmentsSectionChecking(bool isFannie)
    {
      if (this.loan.GetField("608") == "AdjustableRate")
      {
        if (this.loan.GetField("SYS.X1") == "Y")
          this.requireField("1700");
        this.requireField("247");
        if (isFannie)
          this.requireField("ULDD.FNM.ARMIndexType");
        else
          this.requireField("ULDD.FRE.ARMIndexType");
        this.requireField("689");
        this.requireField("688");
        this.requireField("1827");
      }
      if (this.loan.GetField("ULDD.X4") == "Y" && this.loan.GetField("608") == "AdjustableRate")
        this.requireField("696");
      if (isFannie)
      {
        if (!(this.loan.GetField("425") == "Y"))
          return;
        this.requireField("ULDD.X137");
      }
      else
      {
        if (!(this.loan.GetField("425") == "Y"))
          return;
        this.requireField("CASASRN.X141");
      }
    }

    private void riskAssessmentSectionChecking(bool isFannie)
    {
      if (isFannie)
      {
        if (!(this.loan.GetField("1543") == "DU"))
          return;
        this.requireField("DU.LP.ID");
      }
      else
      {
        if (this.loan.GetField("1543") == "Other")
          this.requireField("ULDD.X149");
        if (!(this.loan.GetField("1543") == "LP"))
          return;
        this.requireField("DU.LP.ID");
      }
    }

    private void subjectPropertyInfoSectionChecking(bool isFannie)
    {
      if (isFannie)
      {
        if (this.loan.GetField("3050") == "")
        {
          switch (this.loan.GetField("1012"))
          {
            case "C_ICondominium":
            case "B_IICondominium":
            case "A_IIICondominium":
              if (this.loan.GetField("ULDD.X143") == "Attached")
              {
                this.requireField("16");
                this.requireField("ULDD.X138");
              }
              this.requireField("ULDD.X143");
              break;
            case "OneCooperative":
            case "TwoCooperative":
            case "TCooperative":
              if (this.loan.GetField("ULDD.X143") == "Attached")
              {
                this.requireField("16");
                this.requireField("ULDD.X138");
                break;
              }
              break;
          }
        }
        switch (this.loan.GetField("1012"))
        {
          case "C_ICondominium":
          case "B_IICondominium":
          case "A_IIICondominium":
            if (this.loan.GetField("ULDD.X143") == "Attached")
              this.requireField("ULDD.X140");
            this.requireField("1298");
            break;
          case "OneCooperative":
          case "TwoCooperative":
          case "TCooperative":
            this.requireField("ULDD.X140");
            break;
        }
        if (this.loan.GetField("1172") == "Conventional")
          this.requireField("ULDD.X142");
        if (this.loan.GetField("1811") == "Investment" || Utils.IsInt((object) this.loan.GetField("ULDD.X139")) && Utils.ParseInt((object) this.loan.GetField("ULDD.X139")) > 1)
          this.requireField("ULDD.X170");
        if (this.loan.GetField("ULDD.X140") == "Other")
          this.addInvalidField("ULDD.X140");
        if (this.loan.GetField("ULDD.X120") == "Other")
          this.addInvalidField("ULDD.X120");
        if (this.loan.GetField("ULDD.FNM.ARMIndexType") == "NationalMonthlyMedianCostOfFundsRateMonthlyAverage")
          this.addInvalidField("ULDD.FNM.ARMIndexType");
        if (this.loan.GetField("ULDD.X172") == "Manufactured" && this.loan.GetField("ULDD.ManufacturedHomeWidthType") == "")
          this.addInvalidField("ULDD.ManufacturedHomeWidthType");
      }
      else
      {
        switch (this.loan.GetField("1012"))
        {
          case "C_ICondominium":
          case "B_IICondominium":
          case "A_IIICondominium":
            this.requireField("16");
            this.requireField("ULDD.X138");
            this.requireField("ULDD.X140");
            this.requireField("ULDD.X141");
            this.requireField("ULDD.X143");
            this.requireField("1298");
            break;
          case "OneCooperative":
          case "TwoCooperative":
          case "TCooperative":
            this.requireField("ULDD.X140");
            break;
        }
        string field = this.loan.GetField("ULDD.X29");
        if ((field == "FullAppraisal" || field == "PriorAppraisalUsed") && (this.loan.GetField("1811") == "Investment" || Utils.IsInt((object) this.loan.GetField("ULDD.X139")) && Utils.ParseInt((object) this.loan.GetField("ULDD.X139")) > 1))
          this.requireField("ULDD.X170");
        if (this.loan.GetField("1811") == "Investment" || Utils.IsInt((object) this.loan.GetField("ULDD.X139")) && Utils.ParseInt((object) this.loan.GetField("ULDD.X139")) > 1)
          this.requireField("ULDD.X171");
        if (this.loan.GetField("ULDD.X172") == "Modular")
          this.addInvalidField("ULDD.X172");
        if (this.loan.GetField("ULDD.X172") == "Manufactured" && this.loan.GetField("ULDD.ManufacturedHomeWidthType") == "")
          this.addInvalidField("ULDD.ManufacturedHomeWidthType");
      }
      switch (this.loan.GetField("1553"))
      {
        case "Manufactured Housing Single Wide":
        case "Manufactured Housing Multiwide":
          this.requireField("ULDD.X144");
          break;
        case "Condominium":
        case "Co-op":
          this.requireField("1012");
          break;
      }
    }

    private void escrowItemsSectionChecking(bool isFannie)
    {
      if (isFannie || !(this.loan.GetField("1550") == "Y") || !(this.loan.GetField("ULDD.X100") == "Y"))
        return;
      this.requireField("230");
      this.requireField("231");
      this.requireField("L268");
      this.requireField("235");
      this.requireField("1630");
      this.requireField("253");
      this.requireField("254");
    }

    private void refinanceCashOutAmountChecking(bool isFannie)
    {
      if (!this.loan.GetField("ULDD.X18").Equals("CashOut", StringComparison.InvariantCultureIgnoreCase))
        return;
      this.requireField("ULDD.RefinanceCashOutAmount");
    }

    private void transactionDetailsSectionChecking(bool isFannie)
    {
      if (!(this.loan.GetField("ULDD.X33") == "Y"))
        return;
      this.requireField("996");
    }

    private void typesOfMortgageAndTermsOfLoan(bool isFannie)
    {
      if (this.loan.GetField("19") == "Purchase" && this.loan.GetField("420") == "First")
        this.requireField("136");
      if (isFannie)
      {
        if (this.loan.GetField("ULDD.X124") == "Y")
          this.requireField("ULDD.X147");
      }
      else
      {
        if (this.loan.GetField("608") == "Other")
          this.requireField("1063");
        if (this.loan.GetField("ULDD.X124") == "Y")
          this.requireField("ULDD.X146");
        if (this.loan.GetField("1551") == "Y" && this.loan.GetField("19") == "Purchase")
          this.requireField("1335");
      }
      if (!(this.loan.GetField("1172") != "Other"))
        return;
      this.requireField("ULDD.FNM.X1172");
    }

    internal void ULDDCLEAR()
    {
      string[] strArray = this.loan.GetField("ULDD.X36").Trim().Split(' ');
      if (strArray.Length > 10)
        this.SetVal("ULDD.X36", "");
      foreach (string str in strArray)
      {
        if (str.Length != 3)
        {
          this.SetVal("ULDD.X36", "");
          break;
        }
        foreach (char ch in str.ToCharArray())
        {
          if (!Utils.IsInt((object) (ch.ToString() ?? "")))
          {
            this.SetVal("ULDD.X36", "");
            break;
          }
        }
      }
      if (!this.loan.IsValidValue("ULDD.X179"))
      {
        this.SetVal("ULDD.X179", "");
      }
      else
      {
        if (this.loan.IsValidValue("ULDD.RefinanceCashOutAmount"))
          return;
        this.SetVal("ULDD.RefinanceCashOutAmount", "");
      }
    }

    private void ACHInfoSectionChecking()
    {
      if (this.loan.GetField("ULDD.GNM.ACHBnkAccntPrpsTyp") == "PrincipalAndInterest")
      {
        this.requireField("ULDD.GNM.ACHABARtngAndTrnstNmbr");
        this.requireField("ULDD.GNM.ACHBnkAccntIdentfr");
      }
      else
      {
        if (!(this.loan.GetField("ULDD.GNM.ACHBnkAccntPrpsTyp") == "TaxesAndInsurance"))
          return;
        this.requireField("ULDD.GNM.ACHBnkAccntPrpsTrnstIdntfr");
        this.requireField("ULDD.GNM.ACHBnkAccntIdentfr");
      }
    }

    private void poolLoanSectionChecking()
    {
      if (this.loan.GetField("ULDD.X72") == "SN")
      {
        this.requireField("ULDD.GNM.CertType");
        this.requireField("ULDD.GNM.CertId");
        this.requireField("ULDD.GNM.CertMaturityDate");
        this.requireField("ULDD.GNM.CertPrinBalAmt");
      }
      if (!(this.loan.GetField("ULDD.GNM.DocReqIndic") == "Y"))
        return;
      this.requireField("ULDD.GNM.DocSubmissionIndic");
    }

    private void ginnieSubjectPropertyInfoSectionChecking()
    {
      if (!(this.loan.GetField("ULDD.X187") == "Other"))
        return;
      this.requireField("ULDD.X188");
    }

    private void ginnieMortgageTypeLoanTermsInfoSectionChecking()
    {
      if (!(this.loan.GetField("608") == "AdjustableRate"))
        return;
      this.requireField("ULDD.GNM.IndxType");
      this.requireField("2625");
      this.requireField("1699");
      this.requireField("689");
    }

    private void ginnieDownPaymentInfoSectionChecking()
    {
      if (this.loan.GetField("19") == "Purchase" && double.Parse(this.loan.GetField("1335")) > 0.0)
        this.requireField("ULDD.GNM.DwnPymntFndsType");
      if (!(this.loan.GetField("ULDD.GNM.DwnPymntFndsType") == "Other"))
        return;
      this.requireField("ULDD.GNM.OtherDwnPymntFndsType");
    }

    private void ginnieAdditionalChecking()
    {
      if (this.loan.GetField("ULDD.X180") == "NoCashOut")
        this.requireField("ULDD.GNM.GovRefType");
      if (this.loan.GetField("ULDD.X187") == "Manufactured")
        this.requireField("745");
      if (this.loan.GetField("19") == "NoCash-Out Refinance" || this.loan.GetField("19") == "Cash-Out Refinance")
        this.requireField("ULDD.X180");
      if (this.loan.GetField("ULDD.X4") == "N")
      {
        this.requireField("19");
        this.requireField("L770");
      }
      if (this.loan.GetField("ULDD.GNM.MntryEvntTyp") != "")
      {
        this.requireField("ULDD.GNM.MntryEvntAppldDt");
        this.requireField("ULDD.GNM.MntryEvntGrssPrncpalAmnt");
      }
      if (this.loan.GetField("ULDD.GNM.BondFinPoolIndic") == "Y")
      {
        this.requireField("ULDD.GNM.BondFinProgType");
        this.requireField("ULDD.GNM.BondFinProgName");
      }
      if (this.loan.GetField("ULDD.X66") == "AdjustableRate")
      {
        this.requireField("ULDD.GNM.PoolIntAdjEffDate");
        this.requireField("ULDD.X77");
      }
      if (this.loan.GetField("1172") == "FHA")
      {
        this.requireField("ULDD.GNM.GovAnnlPrmPrcnt");
        this.requireField("ULDD.GNM.GovAnnlPrmAmt");
      }
      else
      {
        if (!(this.loan.GetField("1172") == "VA"))
          return;
        this.requireField("VASUMM.X3");
        this.requireField("ULDD.GNM.GRNTYPrcnt");
      }
    }

    private void ginnieGovermentInformation()
    {
      if (this.loan.GetField("ULDD.GNM.X1172") == "VA")
        this.requireField("ULDD.GNM.GrntyPrcnt");
      if (!(this.loan.GetField("ULDD.GNM.X1172") == "FHA"))
        return;
      this.requireField("ULDD.GNM.GovAnnlPrmPrcnt");
      this.requireField("ULDD.GNM.GovAnnlPrmAmt");
    }

    private void concatenateSfcCodes(string id, string val)
    {
      int specialFeatureCode = this.loan.GetNumberOfSpecialFeatureCode();
      string val1 = "";
      string val2 = "";
      for (int index = 1; index <= specialFeatureCode; ++index)
      {
        string str = this.Val("SFC" + index.ToString("00") + "01");
        if (!string.IsNullOrEmpty(str))
        {
          switch (this.Val("SFC" + index.ToString("00") + "04"))
          {
            case "FannieMae":
              val1 = val1 + (val1 != "" ? " " : "") + str;
              continue;
            case "FreddieMac":
              val2 = val2 + (val2 != "" ? " " : "") + str;
              continue;
            default:
              continue;
          }
        }
      }
      this.SetVal("ULDD.X36", val1);
      this.SetVal("ULDD.X179", val2);
    }

    private void copyAlienStatus(string id, string val)
    {
      switch (id)
      {
        case "4709":
          this.SetVal("ULDD.X123", this.loan.GetField("4709", 0));
          break;
        case "4710":
          this.SetVal("ULDD.X148", this.loan.GetField("4710", 0));
          break;
      }
    }

    private void addInvalidField(string fieldID)
    {
      if (this.validateResult.Contains(fieldID))
        return;
      this.validateResult.Add(fieldID);
    }

    private void clearInvalidFieldList() => this.validateResult.Clear();

    private void requireField(string fieldID)
    {
      if (!(this.loan.GetField(fieldID) == "") && !(this.loan.GetField(fieldID) == "//"))
        return;
      this.addInvalidField(fieldID);
    }
  }
}
