// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.D1003Calculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class D1003Calculation : CalculationBase
  {
    private const string className = "D1003Calculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    internal Routine AdjConstTotal;
    internal Routine CalcHousingExp;
    internal Routine CalcBorrowerIncome;
    internal Routine CalcNetWorth;
    internal Routine CalcLoansubAndTSUM;
    internal Routine CalcTotalAssets;
    internal Routine CopyOtherIncome;
    internal Routine CopyBrokerInfoToLender;
    internal Routine CalcLiquidAssets;
    internal Routine CalcTotalOtherAssets;
    internal Routine CopyInfoToLockRequest;
    internal Routine PopulateOtherFields;
    internal Routine CopyBuyDownToLockRequest;
    internal Routine CopyLockRequestAdditionalFields;
    internal Routine CalcField1109;
    internal Routine CopySameMailingAddress;
    internal Routine CopyCitizenshipAndAge;
    internal Routine CalcLenderChannel;
    internal Routine CalcIntrOnly;
    internal Routine CopyLenderToSSNCompany;
    internal Routine CopyCountyToJurisdiction;
    internal Routine CalcEthnicityRaceSex;
    internal Routine CalcCompanyStateLicense;
    internal Routine CalcNMLSResidentialMortgageType;
    internal Routine PopulateLoanAmountToNmlsApplicationAmounts;
    internal Routine CalcUli;
    internal Routine Calc2018DI;
    internal Routine CalcLienPosition;
    internal Routine CalcConformingLimit;
    internal Routine CalcOtherIncome;
    internal Routine CalcNMLSRefiPurpose;
    internal Routine CalculatePMIIndicator;
    internal Routine ClearIndexDate;
    private readonly D1003CalculationServant d1003CalculationServant;
    private DateTime priorRateLockDate = DateTime.MinValue;
    private string initialApplicaitonDateID = "3142";

    internal D1003Calculation(
      SessionObjects sessionObjects,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
      this.AdjConstTotal = this.RoutineX(new Routine(this.adjustConstructionTotal));
      this.CalcHousingExp = this.RoutineX(new Routine(this.calculateHousingExpense));
      this.CalcBorrowerIncome = this.RoutineX(new Routine(this.calculateBorrowerIncome));
      this.CalcNetWorth = this.RoutineX(new Routine(this.calculateNetWorth));
      this.CalcLoansubAndTSUM = this.RoutineX(new Routine(this.calculateLoansubAndTSUM));
      this.CalcTotalAssets = this.RoutineX(new Routine(this.calculateTotalAssets));
      this.CopyOtherIncome = this.RoutineX(new Routine(this.copyAllOtherIncome));
      this.CopyBrokerInfoToLender = this.RoutineX(new Routine(this.copyBrokerToLender));
      this.CalcLiquidAssets = this.RoutineX(new Routine(this.calculateLiquidAssets));
      this.CalcTotalOtherAssets = this.RoutineX(new Routine(this.calculateTotalOtherAssets));
      this.CopyInfoToLockRequest = this.RoutineX(new Routine(this.copyBorrowersToLockRequest));
      this.PopulateOtherFields = this.RoutineX(new Routine(this.populateOthers));
      this.CopyBuyDownToLockRequest = this.RoutineX(new Routine(this.copyBuyDownToLockRequest));
      this.CopyLockRequestAdditionalFields = this.RoutineX(new Routine(this.copyLoanToLockRequestAdditionalFields));
      this.CalcField1109 = this.RoutineX(new Routine(this.calculationField1109));
      this.CopySameMailingAddress = this.RoutineX(new Routine(this.populateSameMailingAddress));
      this.CopyCitizenshipAndAge = this.RoutineX(new Routine(this.adjustCitizenshipAndAge));
      this.CalcLenderChannel = this.RoutineX(new Routine(this.calculateLenderChannel));
      this.CalcIntrOnly = this.RoutineX(new Routine(this.calculateInterestOnly));
      this.CopyLenderToSSNCompany = this.RoutineX(new Routine(this.copyLenderInfoToSSNCompany));
      this.CopyCountyToJurisdiction = this.RoutineX(new Routine(this.copyPropertyCountyToJurisdiction));
      this.CalcEthnicityRaceSex = this.RoutineX(new Routine(this.calculateEthnicityRaceSex));
      this.CalcCompanyStateLicense = this.RoutineX(new Routine(this.PopulateCompanyStateLicense));
      this.CalcNMLSResidentialMortgageType = this.RoutineX(new Routine(this.calculateNMLSResidentialMortgageType));
      this.PopulateLoanAmountToNmlsApplicationAmounts = this.RoutineX(new Routine(this.populateLoanAmountToNmlsApplicationAmounts));
      this.CalcUli = this.RoutineX(new Routine(this.calculateULI));
      this.Calc2018DI = this.RoutineX(new Routine(this.calculate2018DI));
      this.CalcLienPosition = this.RoutineX(new Routine(this.calculateLienPosition));
      this.CalcConformingLimit = this.RoutineX(new Routine(this.calculateConformingLimit));
      this.CalcOtherIncome = this.RoutineX(new Routine(this.calculateOtherIncome));
      this.CalcNMLSRefiPurpose = this.RoutineX(new Routine(this.calculateNMLSRefiPurpose));
      this.CalculatePMIIndicator = this.RoutineX(new Routine(this.calculatePMIIndicator));
      this.ClearIndexDate = this.RoutineX(new Routine(this.clearIndexDate));
      this.priorRateLockDate = Utils.ParseDate((object) l.GetField("761"));
      this.addFieldHandlers();
      this.d1003CalculationServant = new D1003CalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateBorrowerIncome));
      this.AddFieldHandler("1169", routine1);
      this.AddFieldHandler("1170", routine1);
      this.AddFieldHandler("1815", routine1);
      this.AddFieldHandler("1816", routine1);
      this.AddFieldHandler("307", routine1);
      this.AddFieldHandler("35", routine1);
      this.AddFieldHandler("181", routine1);
      this.AddFieldHandler("68", routine1);
      this.AddFieldHandler("69", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateOtherIncome));
      this.AddFieldHandler("144", routine2 + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("145", this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("146", routine2 + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("147", routine2 + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("148", this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("149", routine2 + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("150", routine2 + this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("151", this.calObjs.USDACal.CalcIncomeWorksheet);
      this.AddFieldHandler("152", routine2 + this.calObjs.USDACal.CalcIncomeWorksheet);
      Routine routine3 = this.RoutineX(new Routine(this.calculateHousingExpense));
      this.AddFieldHandler("122", routine3);
      this.AddFieldHandler("124", routine3);
      this.AddFieldHandler("125", routine3);
      this.AddFieldHandler("126", routine3);
      this.AddFieldHandler("228", routine3);
      this.AddFieldHandler("190", routine3);
      this.AddFieldHandler("1005", routine3);
      this.AddFieldHandler("1487", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculateLiquidAssets));
      this.AddFieldHandler("1605", routine4);
      this.AddFieldHandler("1607", routine4);
      this.AddFieldHandler("1609", routine4);
      this.AddFieldHandler("210", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.calculateTotalAssets));
      this.AddFieldHandler("915", routine5);
      this.AddFieldHandler("919", routine5);
      this.AddFieldHandler("212", routine5);
      this.AddFieldHandler("213", routine5);
      this.AddFieldHandler("215", routine5);
      this.AddFieldHandler("217", routine5);
      this.AddFieldHandler("222", routine5);
      this.AddFieldHandler("224", routine5);
      this.AddFieldHandler("1053", routine5);
      this.AddFieldHandler("1055", routine5);
      this.AddFieldHandler("1718", routine5);
      this.AddFieldHandler("732", this.RoutineX(new Routine(this.calculateNetWorth)));
      Routine routine6 = this.RoutineX(new Routine(this.adjustCitizenshipAndAge));
      this.AddFieldHandler("965", routine6);
      this.AddFieldHandler("466", routine6);
      this.AddFieldHandler("985", routine6);
      this.AddFieldHandler("467", routine6);
      Routine routine7 = this.RoutineX(new Routine(this.calculateRatios));
      this.AddFieldHandler("1389", routine7);
      this.AddFieldHandler("1731", routine7);
      this.AddFieldHandler("273", routine7);
      this.AddFieldHandler("1168", routine7);
      this.AddFieldHandler("1171", routine7);
      this.AddFieldHandler("1742", routine7);
      Routine routine8 = this.RoutineX(new Routine(this.calculateLoansubAndTSUM));
      this.AddFieldHandler("462", routine8);
      this.AddFieldHandler("382", routine8);
      this.AddFieldHandler("LOANSUB.X1", routine8);
      this.AddFieldHandler("1723", routine8);
      this.AddFieldHandler("1727", routine8);
      this.AddFieldHandler("1728", routine8);
      this.AddFieldHandler("1729", routine8);
      this.AddFieldHandler("1730", routine8);
      this.AddFieldHandler("1733", routine8);
      this.AddFieldHandler("1379", routine8);
      Routine routine9 = this.RoutineX(new Routine(this.populateSameMailingAddress));
      this.AddFieldHandler("BR0004", routine9);
      this.AddFieldHandler("BR0006", routine9);
      this.AddFieldHandler("BR0007", routine9);
      this.AddFieldHandler("BR0008", routine9);
      this.AddFieldHandler("BR0025", routine9);
      this.AddFieldHandler("BR0026", routine9);
      this.AddFieldHandler("BR0027", routine9);
      this.AddFieldHandler("CR0004", routine9);
      this.AddFieldHandler("CR0004", routine9);
      this.AddFieldHandler("CR0006", routine9);
      this.AddFieldHandler("CR0007", routine9);
      this.AddFieldHandler("CR0008", routine9);
      this.AddFieldHandler("CR0025", routine9);
      this.AddFieldHandler("CR0026", routine9);
      this.AddFieldHandler("CR0027", routine9);
      this.AddFieldHandler("1819", routine9);
      this.AddFieldHandler("1820", routine9);
      Routine routine10 = this.RoutineX(new Routine(this.populateOthers));
      this.AddFieldHandler("1481", routine10);
      this.AddFieldHandler("364", routine10 + this.CalcUli);
      this.AddFieldHandler("MORNET.X67", routine10);
      this.AddFieldHandler("1484", routine10);
      this.AddFieldHandler("1502", routine10);
      this.AddFieldHandler("11", routine10);
      this.AddFieldHandler("12", routine10);
      this.AddFieldHandler("14", routine10);
      this.AddFieldHandler("15", routine10);
      this.AddFieldHandler("3315", routine10);
      this.AddFieldHandler("3316", routine10);
      this.AddFieldHandler("995", routine10);
      this.AddFieldHandler("994", routine10);
      this.AddFieldHandler("2293", routine10);
      this.AddFieldHandler("2294", routine10);
      this.AddFieldHandler("2216", routine10);
      this.AddFieldHandler("2217", routine10);
      this.AddFieldHandler("1401", routine10);
      this.AddFieldHandler("2861", routine10);
      this.AddFieldHandler("SYS.X11", routine10);
      this.AddFieldHandler("934", routine10);
      this.AddFieldHandler("3515", routine10);
      this.AddFieldHandler("16", routine10);
      this.AddFieldHandler("3533", routine10);
      this.AddFieldHandler("3000", routine10);
      this.AddFieldHandler("CASASRN.X167", routine10);
      this.AddFieldHandler("3891", routine10);
      this.AddFieldHandler("5015", routine10);
      for (int index = 5; index <= 8; ++index)
        this.AddFieldHandler("URLA.X20" + index.ToString(), routine10);
      Routine routine11 = this.RoutineX(new Routine(this.copyBrokerToLender));
      this.AddFieldHandler("1969", routine11);
      this.AddFieldHandler("1263", routine11);
      this.AddFieldHandler("319", routine11);
      this.AddFieldHandler("313", routine11);
      this.AddFieldHandler("321", routine11);
      this.AddFieldHandler("323", routine11);
      this.AddFieldHandler("324", routine11);
      this.AddFieldHandler("326", routine11);
      this.AddFieldHandler("1406", routine11);
      this.AddFieldHandler("1407", routine11);
      this.AddFieldHandler("1149", routine11);
      this.AddFieldHandler("3639", routine11);
      this.AddFieldHandler("3244", routine11);
      this.AddFieldHandler("3237", routine11);
      Routine routine12 = this.RoutineX(new Routine(this.copyBorrowersToLockRequest));
      this.AddFieldHandler("4000", routine12);
      this.AddFieldHandler("4001", routine12);
      this.AddFieldHandler("4003", routine12);
      this.AddFieldHandler("4004", routine12);
      this.AddFieldHandler("4005", routine12);
      this.AddFieldHandler("4007", routine12);
      this.AddFieldHandler("36", routine12);
      this.AddFieldHandler("37", routine12);
      this.AddFieldHandler("68", routine12);
      this.AddFieldHandler("69", routine12);
      this.AddFieldHandler("FE0115", routine12);
      this.AddFieldHandler("FE0215", routine12);
      Routine routine13 = this.RoutineX(new Routine(this.calculateNMLSDocType));
      this.AddFieldHandler("MORNET.X67", routine13);
      this.AddFieldHandler("CASASRN.X144", routine13);
      Routine routine14 = this.RoutineX(new Routine(this.calculateNMLSLoanType));
      this.AddFieldHandler("16", routine14);
      this.AddFieldHandler("19", routine14);
      this.AddFieldHandler("1172", routine14);
      this.AddFieldHandler("420", routine14);
      Routine routine15 = this.RoutineX(new Routine(this.calculateNMLSResidentialMortgageType));
      this.AddFieldHandler("608", routine15);
      this.AddFieldHandler("1172", routine15);
      this.AddFieldHandler("608", this.RoutineX(new Routine(this.calculateNMLSARMType)));
      Routine routine16 = this.RoutineX(new Routine(this.calculateNMLSPiggybackHELOCValue));
      this.AddFieldHandler("428", routine16);
      this.AddFieldHandler("1732", routine16);
      this.AddFieldHandler("19", this.RoutineX(new Routine(this.calculateNMLSRefiPurpose)));
      Routine routine17 = this.RoutineX(new Routine(this.calculateConformingLimit));
      this.AddFieldHandler("13", routine17);
      this.AddFieldHandler("14", routine17);
      this.AddFieldHandler("16", routine17);
      this.AddFieldHandler("2", routine17);
      this.AddFieldHandler("1811", this.RoutineX(new Routine(this.calculateOccupancy)));
      this.AddFieldHandler("1766", this.RoutineX(new Routine(this.calculatePMIIndicator)));
      this.AddFieldHandler("761", this.RoutineX(new Routine(this.setLockDateTimestamp)));
      Routine routine18 = this.RoutineX(new Routine(this.calculateEthnicityRaceSex));
      this.AddFieldHandler("3840", routine18);
      this.AddFieldHandler("1531", routine18);
      this.AddFieldHandler("4147", routine18);
      this.AddFieldHandler("4154", routine18);
      this.AddFieldHandler("4158", routine18);
      this.AddFieldHandler("4162", routine18);
      this.AddFieldHandler("4169", routine18);
      this.AddFieldHandler("4173", routine18);
      Routine routine19 = this.RoutineX(new Routine(this.calculateULI));
      this.AddFieldHandler("HMDA.X28", routine19);
      this.AddFieldHandler("HMDA.X105", routine19);
      this.AddFieldHandler("HMDA.X106", routine19);
    }

    private void setLockDateTimestamp(string id, string val)
    {
      if (!(id == "761"))
        return;
      DateTime date = Utils.ParseDate((object) val);
      if (!(date != this.priorRateLockDate))
        return;
      this.loan.SetField("3200", Utils.DateTimeToUTCString(DateTime.Now));
      this.priorRateLockDate = date;
    }

    private void adjustConstructionTotal(string id, string val)
    {
      this.d1003CalculationServant.AdjustConstructionTotal(id, val);
      this.SetCurrentNum("1074", this.FltVal("22") + this.FltVal("23"));
    }

    private void calculateOtherIncome(string id, string val)
    {
      Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateOtherIncome ID: " + id);
      switch (id)
      {
        case "144":
        case "145":
          if (this.Val("146") == CalculationBase.nil)
            return;
          if (id == "144" & val == CalculationBase.nil)
          {
            this.SetVal("145", CalculationBase.nil);
            this.SetVal("146", CalculationBase.nil);
            break;
          }
          break;
        case "147":
        case "148":
          if (this.Val("149") == CalculationBase.nil)
            return;
          if (id == "147" & val == CalculationBase.nil)
          {
            this.SetVal("148", CalculationBase.nil);
            this.SetVal("149", CalculationBase.nil);
            break;
          }
          break;
        case "150":
        case "151":
          if (this.Val("152") == CalculationBase.nil)
            return;
          if (id == "150" & val == CalculationBase.nil)
          {
            this.SetVal("151", CalculationBase.nil);
            this.SetVal("152", CalculationBase.nil);
            break;
          }
          break;
      }
      this.loan.ClearOtherIncomeItems();
      double brwTotal = 0.0;
      double coBrwTotal = 0.0;
      if (this.loan.Use2020URLA)
      {
        this.calculateOtherTotal(ref brwTotal, ref coBrwTotal);
      }
      else
      {
        string str1;
        if ((str1 = this.Val("144")) != CalculationBase.nil)
        {
          string val1 = this.Val("146");
          if (str1 == "B")
            brwTotal += this.Flt(val1);
          else
            coBrwTotal += this.Flt(val1);
        }
        string str2;
        if ((str2 = this.Val("147")) != CalculationBase.nil)
        {
          string val2 = this.Val("149");
          if (str2 == "B")
            brwTotal += this.Flt(val2);
          else
            coBrwTotal += this.Flt(val2);
        }
        string str3;
        if ((str3 = this.Val("150")) != CalculationBase.nil)
        {
          string val3 = this.Val("152");
          if (str3 == "B")
            brwTotal += this.Flt(val3);
          else
            coBrwTotal += this.Flt(val3);
        }
      }
      this.SetCurrentNum("108", brwTotal);
      this.SetCurrentNum("117", coBrwTotal);
      this.calculateBorrowerIncome(CalculationBase.nil, CalculationBase.nil);
      this.calObjs.VACal.CalcVALA(id, val);
    }

    private void calculateOtherTotal(ref double brwTotal, ref double coBrwTotal)
    {
      brwTotal = this.FltVal("URLA.X42");
      coBrwTotal = this.FltVal("URLA.X43");
      int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
      {
        if (this.Val("BE" + index.ToString("00") + "09") == "Y")
          brwTotal += this.FltVal("BE" + index.ToString("00") + "53");
      }
      int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
      {
        if (this.Val("CE" + index.ToString("00") + "09") == "Y")
          coBrwTotal += this.FltVal("CE" + index.ToString("00") + "53");
      }
    }

    private void copyAllOtherIncome(string id, string val)
    {
      double brwTotal = 0.0;
      double coBrwTotal = 0.0;
      if (this.loan.Use2020URLA)
      {
        this.calculateOtherTotal(ref brwTotal, ref coBrwTotal);
      }
      else
      {
        string str1 = this.Val("144");
        double num1 = this.FltVal("146");
        switch (str1)
        {
          case "B":
            brwTotal += num1;
            break;
          case "C":
            coBrwTotal += num1;
            break;
        }
        string str2 = this.Val("147");
        double num2 = this.FltVal("149");
        switch (str2)
        {
          case "B":
            brwTotal += num2;
            break;
          case "C":
            coBrwTotal += num2;
            break;
        }
        string str3 = this.Val("150");
        double num3 = this.FltVal("152");
        switch (str3)
        {
          case "B":
            brwTotal += num3;
            break;
          case "C":
            coBrwTotal += num3;
            break;
        }
      }
      this.SetCurrentNum("108", brwTotal);
      this.SetCurrentNum("117", coBrwTotal);
    }

    internal double GetVOLLienAmount(bool isFirstLien)
    {
      if (!this.USEURLA2020)
        return 0.0;
      BorrowerPair pair = (BorrowerPair) null;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      if (borrowerPairs.Length > 1)
      {
        string id = this.loan.CurrentBorrowerPair.Id;
        for (int index = 1; index < borrowerPairs.Length; ++index)
        {
          if (id == borrowerPairs[index].Id)
          {
            pair = borrowerPairs[index];
            this.loan.SetBorrowerPair(borrowerPairs[0]);
            break;
          }
        }
      }
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int num = 0;
      double volLienAmount = 0.0;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        string field = this.loan.GetField(str + "29");
        if (isFirstLien)
        {
          if (!(field != "1"))
          {
            ++num;
            if (num == 1 && this.loan.GetField(str + "27") == "Y" && this.loan.GetField(str + "18") != "Y")
            {
              volLienAmount = Utils.ParseDouble((object) this.loan.GetField(str + "11"));
            }
            else
            {
              volLienAmount = 0.0;
              break;
            }
          }
        }
        else if (!(field == "1") && !(field == string.Empty) && this.loan.GetField(str + "27") == "Y" && this.loan.GetField(str + "18") != "Y")
          volLienAmount += Utils.ParseDouble((object) this.loan.GetField(str + "11"));
      }
      if (pair != null && pair.Id != this.loan.CurrentBorrowerPair.Id)
        this.loan.SetBorrowerPair(pair);
      return volLienAmount;
    }

    private void calculateHousingExpense(string id, string val)
    {
      if (Tracing.IsSwitchActive(D1003Calculation.sw, TraceLevel.Info))
        Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateHousingExpense ID: " + id);
      if (this.FltVal("1014") > 100.0)
        this.SetCurrentNum("1014", 100.0);
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (id == "1811" || string.Compare(id, "IMPORT", true) == 0)
      {
        string val1 = this.Val("1811", borrowerPairs[0]);
        if (val1 == "N")
          this.SetVal("190", "");
        else
          this.SetVal("190", val1);
      }
      if (this.Val("190") == "")
      {
        string val2 = this.Val("1811", borrowerPairs[0]);
        if (val2 == "N")
          this.SetVal("190", "");
        else
          this.SetVal("190", val2);
      }
      double insurance = this.calObjs.GFECal.CalculateInsurance("231");
      switch (id)
      {
        case "IMPORT":
          if (this.FltVal("231") != 0.0 && this.FltVal("1405") == 0.0 || string.Compare(this.Val("USEGFETAX"), "Y", true) == 0)
          {
            this.SetCurrentNum("1405", this.FltVal("231"));
            break;
          }
          if (this.FltVal("1405") != 0.0)
          {
            this.SetCurrentNum("231", this.FltVal("1405"));
            break;
          }
          break;
        case "231":
        case "1405":
          if (insurance != this.ToDouble(val))
          {
            this.SetCurrentNum("1752", 0.0);
            goto default;
          }
          else
            goto default;
        default:
          if (string.Compare(this.Val("USEGFETAX"), "Y", true) != 0)
          {
            if (id == "1405")
            {
              this.SetCurrentNum("231", this.ToDouble(this.Val("1405")));
              break;
            }
            if (id == "231")
            {
              double num = this.FltVal("231");
              if (num != 0.0)
              {
                this.SetVal("1405", num.ToString("N2"));
                break;
              }
              this.SetVal("1405", "");
              break;
            }
            break;
          }
          break;
      }
      if (id == "232")
        this.calObjs.ULDDExpCal.CalcFannieMaeExportFields(id, val);
      if (string.Compare(this.Val("USEGFETAX"), "Y", true) == 0)
      {
        double num = this.FltVal("L268") + this.FltVal("231");
        if (SharedCalculations.IsTaxFee(this.Val("1628")))
          num += this.FltVal("1630");
        if (SharedCalculations.IsTaxFee(this.Val("660")))
          num += this.FltVal("253");
        if (SharedCalculations.IsTaxFee(this.Val("661")))
          num += this.FltVal("254");
        this.SetVal("1405", num.ToString("N2"));
      }
      double num1 = 0.0;
      for (int index = 119; index <= 126; ++index)
        num1 += this.FltVal(index.ToString());
      if (this.USEURLA2020)
        num1 += this.FltVal("URLA.X212");
      this.SetCurrentNum("737", num1);
      this.SetCurrentNum("1299", num1);
      string str1 = this.Val("420");
      this.calObjs.VERIFCal.CalcHelocLineTotal(id, val);
      double num2 = this.FltVal("5");
      bool useurlA2020 = this.USEURLA2020;
      switch (str1)
      {
        case "FirstLien":
          if (useurlA2020)
          {
            double num3 = this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false) + this.GetVOLLienAmount(false);
            this.SetCurrentNum("229", num3, (this.UseNoPayment(num3) || this.UseLinkedLoanNoPayment(num3)) && (this.loan.GetNumberOfAdditionalLoans() > 0 || !string.IsNullOrEmpty(this.loan.LinkGUID)));
          }
          else if (id == "420")
            this.SetVal("229", string.Empty);
          this.SetCurrentNum("228", num2, this.UseNoPayment(num2) || this.UseLinkedLoanNoPayment(num2));
          break;
        case "SecondLien":
          if (useurlA2020)
          {
            double num4 = this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(true) + this.GetVOLLienAmount(true);
            this.SetCurrentNum("228", num4, this.UseNoPayment(num4) || this.UseLinkedLoanNoPayment(num4));
          }
          else if (id == "420")
            this.SetVal("228", string.Empty);
          double num5 = num2 + this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false) + this.GetVOLLienAmount(false);
          this.SetCurrentNum("229", num5, (this.UseNoPayment(num5) || this.UseLinkedLoanNoPayment(num5)) && (this.loan.GetNumberOfAdditionalLoans() > 0 || !string.IsNullOrEmpty(this.loan.LinkGUID)));
          break;
      }
      string str2 = this.Val("1172");
      bool flag = str2 == "FarmersHomeAdministration";
      double num6 = this.FltVal("1766");
      if (num6 != 0.0)
      {
        if (flag)
        {
          this.SetCurrentNum("NEWHUD.X1707", num6);
          if (this.Val("232") != string.Empty)
            this.calObjs.USDACal.CopyPOCPTCAPRFromLine1003ToLine1010(id, val);
          this.SetVal("232", "");
          this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("232", this.Val("232"));
        }
        else
        {
          this.SetVal("NEWHUD.X1707", "");
          this.SetVal("232", num6.ToString("N2"));
          this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("232", this.Val("232"));
        }
      }
      this.calObjs.GFECal.CalcPrepaid(id, val);
      string empty = string.Empty;
      string val3 = this.Val("228");
      string val4 = this.Val("229");
      double num7;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (this.Val("1811", borrowerPairs[index]) == "PrimaryResidence")
        {
          if (str2 != "HELOC")
          {
            if (str1 != "FirstLien")
            {
              double num8 = this.Flt(val3) + this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(true, true) - this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(true);
              this.SetCurrentNum("1724", num8, borrowerPairs[index], this.UseNoPayment(num8));
            }
            if (str1 != "SecondLien")
            {
              double num9 = this.Flt(val4) + this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false, true) - this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false);
              this.SetCurrentNum("1725", num9, borrowerPairs[index], this.UseNoPayment(num9));
            }
          }
          this.SetVal("1726", this.Val("230"), borrowerPairs[index]);
          this.SetVal("1727", this.Val("1405"), borrowerPairs[index]);
          if (flag)
            this.SetVal("1728", "", borrowerPairs[index]);
          else
            this.SetVal("1728", this.Val("232"), borrowerPairs[index]);
          this.SetVal("1729", this.Val("233"), borrowerPairs[index]);
          if (this.USEURLA2020)
          {
            num7 = this.calculateOtherProposedExpense();
            this.SetVal("234", num7.ToString("N2"));
          }
          else if (id == "L268" || id == "235" || id == "1630" || id == "253" || id == "254" || id == "NEWHUD.X1707" || id == "1771" || id == "1109" || id == "136")
          {
            num7 = 0.0;
            if (string.Compare(this.Val("USEGFETAX"), "Y", true) == 0)
            {
              num7 = this.FltVal("NEWHUD.X1707") + this.FltVal("235");
              if (!SharedCalculations.IsTaxFee(this.Val("1628")))
                num7 += this.FltVal("1630");
              if (!SharedCalculations.IsTaxFee(this.Val("660")))
                num7 += this.FltVal("253");
              if (!SharedCalculations.IsTaxFee(this.Val("661")))
                num7 += this.FltVal("254");
            }
            else
            {
              if (this.Val("1800") != "Y")
                num7 += this.FltVal("L268");
              if (this.Val("1801") != "Y")
                num7 += this.FltVal("235");
              if (this.Val("1802") != "Y")
                num7 += this.FltVal("1630");
              if (this.Val("1803") != "Y")
                num7 += this.FltVal("253");
              if (this.Val("1804") != "Y")
                num7 += this.FltVal("254");
              if (this.Val("3357") != "Y")
                num7 += this.FltVal("NEWHUD.X1707");
              num7 += this.FltVal("1799");
            }
            this.SetVal("234", num7.ToString("N2"));
          }
          this.SetCurrentNum("1730", this.FltVal("234"), borrowerPairs[index]);
          if (this.USEURLA2020)
            this.SetVal("1726", this.FltVal("230", borrowerPairs[index]).ToString("N2"), borrowerPairs[index]);
          else if (this.FltVal("235") > 0.0 && this.Val("1801") != "Y")
          {
            double num10 = this.FltVal("1730", borrowerPairs[index]) - this.FltVal("235");
            if (num10 != 0.0)
              this.SetVal("1730", num10.ToString("N2"), borrowerPairs[index]);
            else
              this.SetVal("1730", "", borrowerPairs[index]);
            this.SetVal("1726", (this.FltVal("1726", borrowerPairs[index]) + this.FltVal("235")).ToString("N2"), borrowerPairs[index]);
          }
        }
        else if (this.USEURLA2020 || id == "1799" || id == "1630" || id == "253" || id == "254" || id == "235" || id == "NEWHUD.X1707")
        {
          if (this.USEURLA2020)
          {
            num7 = this.calculateOtherProposedExpense();
          }
          else
          {
            num7 = 0.0;
            if (this.Val("1800", borrowerPairs[index]) != "Y")
              num7 += this.FltVal("L268", borrowerPairs[index]);
            if (this.Val("1801", borrowerPairs[index]) != "Y")
              num7 += this.FltVal("235", borrowerPairs[index]);
            if (this.Val("1802", borrowerPairs[index]) != "Y")
              num7 += this.FltVal("1630", borrowerPairs[index]);
            if (this.Val("1803", borrowerPairs[index]) != "Y")
              num7 += this.FltVal("253", borrowerPairs[index]);
            if (this.Val("1804", borrowerPairs[index]) != "Y")
              num7 += this.FltVal("254", borrowerPairs[index]);
            if (this.Val("3357", borrowerPairs[index]) != "Y")
              num7 += this.FltVal("NEWHUD.X1707", borrowerPairs[index]);
            num7 += this.FltVal("1799", borrowerPairs[index]);
          }
          this.SetVal("234", num7.ToString("N2"));
        }
      }
      num7 = this.FltVal("228") + this.FltVal("229") + this.FltVal("230") + this.FltVal("1405") + this.FltVal("232") + this.FltVal("233") + this.FltVal("234");
      if (this.USEURLA2020)
        num7 += this.FltVal("URLA.X144");
      this.SetCurrentNum("912", num7, this.UseNoPayment(num7));
      this.calObjs.RegzCal.CalcLateFeePaymentInRegz(id, val);
      num7 = this.FltVal("229") + this.FltVal("233") + this.FltVal("234");
      if (this.USEURLA2020)
        num7 += this.FltVal("URLA.X144");
      this.SetCurrentNum("1348", num7);
      num7 = this.FltVal("230") + this.FltVal("1405") + this.FltVal("234") + this.FltVal("233");
      this.SetCurrentNum("1075", num7);
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (this.Val("1811", borrowerPairs[index]) != "PrimaryResidence")
        {
          this.SetCurrentNum("1723", this.FltVal("119", borrowerPairs[index]), borrowerPairs[index]);
          if (str2 != "HELOC")
          {
            string val5 = this.Val("120", borrowerPairs[index]);
            string val6 = this.Val("121", borrowerPairs[index]);
            this.SetCurrentNum("1724", this.Flt(val5), borrowerPairs[index], !string.IsNullOrEmpty(val5) && this.UseNoPayment(this.Flt(val5)));
            this.SetCurrentNum("1725", this.Flt(val6), borrowerPairs[index], !string.IsNullOrEmpty(val6) && this.UseNoPayment(this.Flt(val6)) && (this.loan.GetNumberOfAdditionalLoans() > 0 || !string.IsNullOrEmpty(this.loan.LinkGUID)));
          }
          this.SetVal("1726", this.Val("122", borrowerPairs[index]), borrowerPairs[index]);
          this.SetVal("1727", this.Val("123", borrowerPairs[index]), borrowerPairs[index]);
          this.SetVal("1728", this.Val("124", borrowerPairs[index]), borrowerPairs[index]);
          this.SetVal("1729", this.Val("125", borrowerPairs[index]), borrowerPairs[index]);
          this.SetCurrentNum("1730", this.FltVal("126", borrowerPairs[index]), borrowerPairs[index]);
        }
        else
          this.SetCurrentNum("1723", 0.0, borrowerPairs[index]);
      }
      string propertyType = this.Val("1811");
      double num11 = 0.0;
      if (propertyType != "PrimaryResidence" || this.IsPrimaryPair())
        num11 = this.FindCashflowAmount(propertyType);
      if (num11 < 0.0)
      {
        this.SetCurrentNum("462", num11 * -1.0);
        this.SetCurrentNum("1169", 0.0);
      }
      else
      {
        this.SetCurrentNum("462", 0.0);
        this.SetCurrentNum("1169", num11);
      }
      num7 = this.FltVal("1732");
      if (str1 == "FirstLien")
        num7 += this.FltVal("428");
      else
        num7 += this.FltVal("427");
      this.SetCurrentNum("2398", num7);
      this.calObjs.VERIFCal.CalcRealEstate(id, val);
      this.calObjs.D1003URLA2020Cal.CalcBackEndRatio(id, val);
      this.calObjs.ComortCal.CalcComortgagor(id, val);
      this.calObjs.FHACal.CalcFredMac(id, val);
    }

    public double FindCashflowAmount(string propertyType)
    {
      string str1 = this.Val("1172");
      string str2 = this.Val("420");
      double num1 = this.FltVal("1487") / 100.0;
      if (num1 == 0.0)
        num1 = 1.0;
      double cashflowAmount = this.FltVal("1005") * num1;
      string str3 = "none";
      if (propertyType != "PrimaryResidence")
      {
        cashflowAmount -= this.FltVal("912");
        str3 = "forBoth";
      }
      double num2 = this.FltVal("1014");
      if ((propertyType == "Investor" || propertyType == "SecondHome") && (num2 > 0.0 || this.Val("19") == "ConstructionToPermanent" || str1 == "HELOC"))
      {
        double num3 = num2 != 0.0 || !(str1 == "HELOC") ? this.FindQualifyRatePayment(true) : this.calObjs.RegzCal.FindHELOCQualifyingPaymentFromQualifyingBasisInputs();
        switch (str2)
        {
          case "FirstLien":
            cashflowAmount = cashflowAmount + this.FltVal("228") - num3;
            str3 = "for229";
            break;
          case "SecondLien":
            cashflowAmount = cashflowAmount + this.FltVal("229") - num3;
            str3 = "for228";
            break;
        }
      }
      double num4 = 0.0;
      double num5 = 0.0;
      if (str3 != "none")
      {
        if (str2 == "SecondLien")
          num4 = this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(true, true) - this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(true);
        num5 = this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false, true) - this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false);
      }
      if (str3 == "for228" || str3 == "forBoth")
        cashflowAmount -= num4;
      if (str3 == "for229" || str3 == "forBoth")
        cashflowAmount -= num5;
      return cashflowAmount;
    }

    private double calculateOtherProposedExpense()
    {
      double otherProposedExpense = this.FltVal("1799");
      if (this.Val("3357") != "Y")
        otherProposedExpense += this.FltVal("NEWHUD.X1707");
      if (this.Val("1802") != "Y" && !SharedCalculations.IsInsuranceOrTax(this.Val("1628")))
        otherProposedExpense += this.FltVal("1630");
      if (this.Val("1803") != "Y" && !SharedCalculations.IsInsuranceOrTax(this.Val("660")))
        otherProposedExpense += this.FltVal("253");
      if (this.Val("1804") != "Y" && !SharedCalculations.IsInsuranceOrTax(this.Val("661")))
        otherProposedExpense += this.FltVal("254");
      return otherProposedExpense;
    }

    private void calculateLiquidAssets(string id, string val)
    {
      Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: CalculateLiquidAssets ID: " + id);
      this.d1003CalculationServant.CalculateLiquidAssets(id);
      this.calculateTotalOtherAssets(id, val);
      this.calObjs.FHACal.CalcFredMac(id, val);
      this.calObjs.D1003URLA2020Cal.CalcBackEndRatio(id, val);
      this.calObjs.ComortCal.CalcComortgagor(id, val);
      this.calObjs.FHACal.CalcMACAWP(id, (string) null);
    }

    private void calculateTotalOtherAssets(string id, string val)
    {
      double num = this.FltVal("183") + this.FltVal("1716") + this.FltVal("979") + this.FltVal("1605") + this.FltVal("1607") + this.FltVal("1609") + this.FltVal("210");
      if (this.USEURLA2020)
        this.SetCurrentNum("915", this.FltVal("979") + this.calculateTotalOtherLiquidAssets());
      else
        this.SetCurrentNum("915", num);
      this.SetCurrentNum("199", num - (this.FltVal("183") - this.FltVal("1716")));
      this.calculateTotalAssets(CalculationBase.nil, CalculationBase.nil);
      this.calObjs.ComortCal.CalcComortgagor((string) null, (string) null);
      this.calObjs.FHACal.CalcFredMac((string) null, (string) null);
    }

    private void calculateBorrowerIncome(string id, string val)
    {
      if (Tracing.IsSwitchActive(D1003Calculation.sw, TraceLevel.Info))
        Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateBorrowerIncome");
      if (this.Val("68") != string.Empty && this.Val("69") != string.Empty)
      {
        if ((id == "68" || id == "69") && this.Val("307") == "Y" && this.Val("35") == "Y")
          this.SetVal("35", "");
        if (id == "307" && val == "Y")
          this.SetVal("35", "");
        else if (id == "35" && val == "Y")
          this.SetVal("307", "");
        else if (id == "181" && val == "Jointly")
        {
          this.SetVal("307", "Y");
          this.SetVal("35", "");
        }
        else if (id == "181" && val == "NotJointly")
        {
          this.SetVal("307", "");
          this.SetVal("35", "Y");
        }
        else if (id == "181" && val == "")
        {
          this.SetVal("307", "");
          this.SetVal("35", "");
        }
        if (this.Val("307") == "Y")
          this.SetVal("181", "Jointly");
        else if (this.Val("35") == "Y")
          this.SetVal("181", "NotJointly");
        else
          this.SetVal("181", "");
      }
      double num1 = this.FltVal("101");
      double num2 = this.FltVal("102");
      double num3 = this.FltVal("103");
      double num4 = this.FltVal("104");
      double num5 = this.FltVal("105");
      double num6 = this.FltVal("106");
      double num7 = this.FltVal("107");
      double num8 = this.FltVal("110");
      double num9 = this.FltVal("111");
      double num10 = this.FltVal("112");
      double num11 = this.FltVal("113");
      double num12 = this.FltVal("114");
      double num13 = this.FltVal("115");
      double num14 = this.FltVal("116");
      double num15 = this.FltVal("108");
      double num16 = this.FltVal("117");
      this.SetCurrentNum("901", num1 + num8);
      this.SetCurrentNum("902", num2 + num9);
      this.SetCurrentNum("903", num3 + num10);
      this.SetCurrentNum("904", num4 + num11);
      this.SetCurrentNum("905", num5 + num12);
      this.SetCurrentNum("906", num6 + num13);
      this.SetCurrentNum("907", num7 + num14);
      this.SetCurrentNum("908", num15 + num16);
      double num17;
      double num18;
      if (this.USEURLA2020)
      {
        num17 = num1 + num2 + num3 + num4 + num6 + num7 + num15;
        num18 = num8 + num9 + num10 + num11 + num13 + num14 + num16;
      }
      else
      {
        num17 = num1 + num2 + num3 + num4 + num5 + num6 + num7 + num15;
        num18 = num8 + num9 + num10 + num11 + num12 + num13 + num14 + num16;
      }
      this.SetCurrentNum("910", num17);
      this.SetCurrentNum("911", num18);
      this.SetCurrentNum("736", num17 + num18);
      double num19 = num17 - num1;
      this.SetCurrentNum("936", num19 - this.FltVal("106"));
      this.SetCurrentNum("1145", Utils.ArithmeticRounding(num19 - this.FltVal("1169"), 2));
      this.SetCurrentNum("1146", num18 - num8 - this.FltVal("1170"));
      this.SetCurrentNum("273", num1 + num8);
      this.SetCurrentNum("1168", this.FltVal("1145") + this.FltVal("1146"));
      this.SetCurrentNum("1817", this.FltVal("1815") + this.FltVal("1816"));
      this.SetCurrentNum("1171", this.FltVal("1169") + this.FltVal("1170"));
      this.calObjs.D1003URLA2020Cal.CalcBackEndRatio(id, val);
      this.calObjs.ComortCal.CalcComortgagor(id, val);
      this.calculateRatios(id, val);
      this.calObjs.PrequalCal.CalcMinMax(id, val);
      this.calObjs.FHACal.CalcMACAWP(id, val);
      this.calObjs.MLDSCal.CalcMLDSScenarios(id, val);
      this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod(id, val);
      this.calObjs.NewHudCal.CalcGFEApplicationDate(id, val);
      this.calObjs.NewHudCal.CalcInitialDisclosureDueDate(id, val);
      this.calObjs.USDACal.CalcIncomeWorksheet(id, val);
    }

    private void calculateTotalAssets(string id, string val)
    {
      if (Tracing.IsSwitchActive(D1003Calculation.sw, TraceLevel.Info))
        Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateTotalAssets ID: " + id);
      this.SetCurrentNum("732", !this.USEURLA2020 ? this.FltVal("915") + this.FltVal("919") + this.FltVal("212") + this.FltVal("213") + this.FltVal("215") + this.FltVal("217") + this.FltVal("1718") + this.FltVal("222") + this.FltVal("224") + this.FltVal("1053") + this.FltVal("1055") : this.FltVal("979") + this.FltVal("919") + this.FltVal("URLA.X54"));
      this.calculateNetWorth(CalculationBase.nil, CalculationBase.nil);
    }

    private double calculateTotalOtherLiquidAssets()
    {
      int numberOfOtherAssets = this.loan.GetNumberOfOtherAssets();
      double otherLiquidAssets = 0.0;
      for (int index = 1; index <= numberOfOtherAssets; ++index)
      {
        string str = "URLAROA" + index.ToString("00");
        switch (this.Val(str + "02"))
        {
          case "BridgeLoanNotDeposited":
          case "CashOnHand":
          case "PendingNetSaleProceedsFromRealEstateAssets":
          case "LeasePurchaseFund":
          case "ProceedsFromSaleOfNonRealEstateAsset":
          case "ProceedsFromSecuredLoan":
          case "ProceedsFromUnsecuredLoan":
            otherLiquidAssets += this.FltVal(str + "03");
            break;
          case "Other":
            if (!(this.Val(str + "04") == "OtherLiquidAsset"))
              break;
            goto case "BridgeLoanNotDeposited";
        }
      }
      int ofGiftsAndGrants = this.loan.GetNumberOfGiftsAndGrants();
      for (int index = 1; index <= ofGiftsAndGrants; ++index)
      {
        string str = "URLARGG" + index.ToString("00");
        if (this.Val(str + "20") != "Y")
          otherLiquidAssets += this.FltVal(str + "21");
      }
      return otherLiquidAssets;
    }

    private void calculateNetWorth(string id, string gval)
    {
      if (Tracing.IsSwitchActive(D1003Calculation.sw, TraceLevel.Info))
        Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateNetWorth ID: " + id);
      this.SetCurrentNum("734", this.FltVal("732") - this.FltVal("733"));
    }

    private void adjustCitizenshipAndAge(string id, string val)
    {
      if (this.loan.Settings != null && this.loan.Settings.HMDAInfo != null && this.loan.Settings.HMDAInfo.HMDAApplicationDate == id || id == "745")
      {
        int index1 = -1;
        string id1 = this.loan.CurrentBorrowerPair.Id;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index2]);
          this.d1003CalculationServant.AdjustCitizenshipAndAge("1402", (string) null);
          this.d1003CalculationServant.AdjustCitizenshipAndAge("1403", (string) null);
          if (id1 == borrowerPairs[index2].Id)
            index1 = index2;
        }
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
      }
      else
        this.d1003CalculationServant.AdjustCitizenshipAndAge(id, val);
      switch (id)
      {
        case "965":
          if (val == "Y")
          {
            this.SetVal("466", "N");
            this.SetVal("BORCIT", "USCitizen");
            break;
          }
          if (!(this.Val("466") != "Y"))
            break;
          this.SetVal("BORCIT", CalculationBase.nil);
          break;
        case "466":
          if (val == "Y")
          {
            this.SetVal("965", "N");
            this.SetVal("BORCIT", "PermanentResidentAlien");
            break;
          }
          if (!(this.Val("965") != "Y"))
            break;
          this.SetVal("BORCIT", CalculationBase.nil);
          break;
        case "985":
          if (val == "Y")
          {
            this.SetVal("467", "N");
            this.SetVal("COBORCIT", "USCitizen");
            break;
          }
          if (!(this.Val("467") != "Y"))
            break;
          this.SetVal("COBORCIT", CalculationBase.nil);
          break;
        case "467":
          if (val == "Y")
          {
            this.SetVal("985", "N");
            this.SetVal("COBORCIT", "PermanentResidentAlien");
            break;
          }
          if (!(this.Val("985") != "Y"))
            break;
          this.SetVal("COBORCIT", CalculationBase.nil);
          break;
      }
    }

    private void calculateRatios(string id, string val)
    {
      if (Tracing.IsSwitchActive(D1003Calculation.sw, TraceLevel.Info))
        Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateRatios ID: " + id);
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double num1 = 0.0;
      double num2 = 0.0;
      int index1 = 0;
      string str1;
      string str2;
      if (this.loan.Use2020URLA)
      {
        str1 = "55";
        str2 = "Investment";
      }
      else
      {
        str1 = "41";
        str2 = "Investor";
      }
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int numberOfMortgages = this.loan.GetNumberOfMortgages();
        for (int index3 = 1; index3 <= numberOfMortgages; ++index3)
        {
          if (!(this.Val("FM" + index3.ToString("00") + str1) != str2))
          {
            if (index2 == 0)
              num2 += this.FltVal("FM" + index3.ToString("00") + "32");
            else
              num1 += this.FltVal("FM" + index3.ToString("00") + "32");
          }
        }
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      double num3 = 0.0;
      if (num1 > 0.0 && num2 < 0.0)
        num3 = num2 * -1.0 > num1 ? num1 : num2 * -1.0;
      else if (num1 < 0.0 && num2 > 0.0)
        num3 = num1 * -1.0 > num2 ? num2 : num1 * -1.0;
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
      for (int index4 = 0; index4 < borrowerPairs.Length; ++index4)
      {
        double num4;
        double num5 = num4 = this.FltVal("1389", borrowerPairs[index4]);
        double val1;
        double num6;
        if (num4 != 0.0)
        {
          num6 = val1 = this.FltVal("1731", borrowerPairs[index4]) / num4 * 100.0;
          if (index4 == 0 && num3 > 0.0)
          {
            num5 = num4 - num3;
            num6 = num5 == 0.0 ? 0.0 : this.FltVal("1731", borrowerPairs[index4]) / num5 * 100.0;
          }
        }
        else
          num6 = val1 = 0.0;
        this.SetCurrentNum("MORNET.X160", Math.Round(num5, 2), borrowerPairs[index4]);
        this.SetCurrentNum("740", Math.Round(val1, 3), borrowerPairs[index4]);
        this.SetCurrentNum("CASASRN.X201", Utils.ArithmeticRounding(val1, 0), borrowerPairs[index4]);
        this.SetCurrentNum("MORNET.X158", Math.Round(num6, 3), borrowerPairs[index4]);
        double val2;
        double num7;
        if (num4 != 0.0)
        {
          num7 = val2 = this.FltVal("1742", borrowerPairs[index4]) / num4 * 100.0;
          if (index4 == 0 && num3 > 0.0)
            num7 = num5 == 0.0 ? 0.0 : (this.FltVal("1742", borrowerPairs[index4]) - num3) / num5 * 100.0;
        }
        else
          num7 = val2 = 0.0;
        this.SetCurrentNum("742", Math.Round(val2, 3), borrowerPairs[index4]);
        this.SetCurrentNum("CASASRN.X202", Utils.ArithmeticRounding(val2, 0), borrowerPairs[index4]);
        this.SetCurrentNum("MORNET.X159", Math.Round(num7, 3), borrowerPairs[index4]);
      }
      this.calObjs.ATRQMCal.CalcMaxTotalPayments(id, val);
      this.calObjs.ATRQMCal.CalcuHousingDebtRatios(id, val);
      this.calObjs.ATRQMCal.CalcEligibility(id, val);
    }

    private void calculateLoansubAndTSUM(string id, string val)
    {
      if (Tracing.IsSwitchActive(D1003Calculation.sw, TraceLevel.Info))
        Tracing.Log(D1003Calculation.sw, TraceLevel.Info, nameof (D1003Calculation), "routine: calculateLoansubAndTSUM ID: " + id);
      string str1 = this.Val("1811");
      double num1 = this.FltVal("1014");
      double num2 = this.FltVal("3");
      string str2 = this.Val("1086");
      if (id == "1014" && val == "")
        this.SetVal("1086", "");
      if (id == "1660" || id == "IMPORT")
      {
        switch (str2)
        {
          case "Above":
            num1 = num2 + this.FltVal("1660");
            break;
          case "Below":
            num1 = num2 - this.FltVal("1660");
            break;
          case "NoteRate":
            num1 = num2;
            break;
        }
        this.SetCurrentNum("1014", num1);
      }
      else if (id == "1086")
      {
        if (str2 == "Other")
          this.SetCurrentNum("1660", 0.0);
        if (str2 == "NoteRate")
        {
          this.SetCurrentNum("1660", 0.0);
          this.SetCurrentNum("1014", num2);
        }
        else if (str2 == "")
        {
          this.SetVal("1660", "");
          this.SetVal("1014", "");
        }
      }
      if (num1 == 0.0)
        num1 = num2;
      double num3 = num1 - num2;
      if (str2 != "Other" && id != "1086" && id != "IMPORT" && this.FltVal("1014") != 0.0)
      {
        if (num3 > 0.0)
        {
          this.SetCurrentNum("1660", num3);
          this.SetVal("1086", "Above");
        }
        else if (num3 < 0.0)
        {
          this.SetCurrentNum("1660", num3 * -1.0);
          this.SetVal("1086", "Below");
        }
        else
        {
          this.SetCurrentNum("1660", 0.0);
          this.SetVal("1086", "NoteRate");
        }
      }
      if (str1 == "PrimaryResidence")
      {
        double num4 = this.FltVal("1014") == 0.0 ? this.FltVal("5") : this.FindQualifyRatePayment(true);
        if (this.FltVal("1014") != 0.0 || this.Val("19") != "ConstructionToPermanent")
        {
          string str3 = this.Val("420");
          if (this.Val("1172") != "HELOC")
          {
            switch (str3)
            {
              case "FirstLien":
                this.SetCurrentNum("1724", num4, this.UseNoPayment(num4));
                break;
              case "SecondLien":
                this.SetCurrentNum("1725", num4 + this.calObjs.D1003URLA2020Cal.GetVOALLienAmount(false, true) + this.calObjs.D1003Cal.GetVOLLienAmount(false), this.UseNoPayment(num4) && (this.loan.GetNumberOfAdditionalLoans() > 0 || !string.IsNullOrEmpty(this.loan.LinkGUID)));
                break;
            }
          }
        }
      }
      this.calObjs.FHACal.CalcFredMac(id, val);
      this.calObjs.D1003URLA2020Cal.CalcBackEndRatio(id, val);
      this.calObjs.ComortCal.CalcComortgagor((string) null, (string) null);
      this.calObjs.FHACal.CalcMACAWP((string) null, (string) null);
      this.calObjs.FHACal.CalcCAWRefi((string) null, (string) null);
      this.calculateRatios((string) null, (string) null);
      this.calObjs.ATRQMCal.CalcMax5YearsPandI(id, val);
      this.calObjs.D1003URLA2020Cal.UpdateLinkedVoal("QM.X337", (string) null);
      this.calObjs.VERIFCal.CalcHelocLineTotal(id, val);
      this.calObjs.ATRQMCal.CalcMaxTotalPayments(id, val);
      if (!(id == "462") && !(id == "1733"))
        return;
      this.calObjs.ATRQMCal.CalcMaxTotalPayments(id, val);
      this.calObjs.ATRQMCal.CalcuHousingDebtRatios(id, val);
    }

    public double FindQualifyRatePayment(bool forMonthlyPI = false)
    {
      double currentRate = this.FltVal("1014");
      double num1 = this.FltVal("2");
      int num2 = this.IntVal("4");
      string str = this.Val("19");
      if (currentRate == 0.0 && str == "ConstructionToPermanent")
        currentRate = this.FltVal("3");
      double num3 = currentRate / 1200.0;
      bool flag1 = this.Val("423") == "Biweekly";
      if (flag1)
      {
        int num4 = this.IntVal("SYS.X2");
        if (num4 == 0)
          num4 = 360;
        num3 = currentRate / (100.0 * ((double) num4 / 14.0));
      }
      double num5;
      if (this.IntVal("1177") > 0 && this.Val("1853") != "Y")
      {
        bool flag2 = this.checkIfSimpleInterest();
        DateTime firstPaymentDate = this.findFirstPaymentDate();
        num5 = !flag2 ? num1 * num3 : RegzCalculation.calculateMonthlyInterestForSimpleInterest(num1, currentRate, firstPaymentDate, firstPaymentDate.AddMonths(1), this.findConstInterestType(), this.findSimpleInterestUse366ForLeapYear());
      }
      else if (flag1)
      {
        int num6 = this.IntVal("SYS.X2");
        if (num6 == 0)
          num6 = 360;
        int unpaidTerm = Utils.ParseInt((object) ((double) (num2 - num2 % 12) / 12.0 * 26.0));
        double num7 = currentRate / (100.0 * ((double) num6 / 14.0));
        num5 = RegzCalculation.CalcRawMonthlyPayment(unpaidTerm, num1, currentRate, false, true, (double) num2, 0.0, 0.0, 26, num2, 0, num1, this.Val("4746"), this.checkIfSimpleInterest(), this.findFirstPaymentDate(), this.findConstInterestType(), this.findSimpleInterestUse366ForLeapYear());
      }
      else
      {
        bool useSimpleInterest = this.checkIfSimpleInterest();
        if (str == "ConstructionOnly")
        {
          LoanData loanData = new LoanData(this.loan.XmlDocClone, this.loan.Settings, false, true);
          LoanCalculator loanCalculator = new LoanCalculator(this.sessionObjects, this.calObjs.LoanConfiguration, loanData, true, this.calObjs.ExternalLateFeeSettings, true);
          loanData.Calculator.UseBestCaseScenario = false;
          loanData.Calculator.UseWorstCaseScenario = false;
          if (this.loan.IsLocked("5"))
            loanData.RemoveLock("5");
          loanData.SetCurrentField("1014", "");
          loanData.SetField("3", currentRate.ToString());
          num5 = Utils.ToDouble(loanData.GetField("5"));
        }
        else
        {
          if (forMonthlyPI && str == "ConstructionToPermanent" && this.Val("CONST.X1") == "Y")
            num2 -= this.IntVal("1176");
          num5 = RegzCalculation.CalcRawMonthlyPayment(num2, num1, currentRate, num2, 0, num1, this.Val("4746"), useSimpleInterest, this.findFirstPaymentDate(), this.findConstInterestType(), this.findSimpleInterestUse366ForLeapYear());
        }
      }
      if (flag1)
        num5 = num5 * 26.0 / 12.0;
      return Math.Round(num5, 2);
    }

    public void UpdateCurrentMailingAddress()
    {
      this.populateSameMailingAddress((string) null, (string) null);
    }

    private void populateSameMailingAddress(string id, string val)
    {
      if (this.Val("1819") == "Y")
      {
        if (this.Val("1825") == "2020")
        {
          this.SetVal("URLA.X7", this.Val("FR0125"));
          this.SetVal("URLA.X8", this.Val("FR0127"));
          this.SetVal("URLA.X197", this.Val("FR0126"));
        }
        this.SetVal("URLA.X267", this.Val("FR0129"));
        this.SetVal("URLA.X269", this.Val("FR0130"));
        this.SetVal("1416", this.Val("FR0104"));
        this.SetVal("1417", this.Val("FR0106"));
        this.SetVal("1418", this.Val("FR0107"));
        this.SetVal("1419", this.Val("FR0108"));
      }
      if (!(this.Val("1820") == "Y"))
        return;
      if (this.Val("1825") == "2020")
      {
        this.SetVal("URLA.X9", this.Val("FR0225"));
        this.SetVal("URLA.X10", this.Val("FR0227"));
        this.SetVal("URLA.X198", this.Val("FR0226"));
      }
      this.SetVal("URLA.X268", this.Val("FR0229"));
      this.SetVal("URLA.X270", this.Val("FR0230"));
      this.SetVal("1519", this.Val("FR0204"));
      this.SetVal("1520", this.Val("FR0206"));
      this.SetVal("1521", this.Val("FR0207"));
      this.SetVal("1522", this.Val("FR0208"));
    }

    private void populateOthers(string id, string val)
    {
      if (this.IsPrimaryPair())
      {
        if (id == "36" || id == "37" || id == "updatetitle" || id == "IMPORT" || id == "4000" || id == "4001" || id == "4002" || id == "4003")
          this.SetVal("31", (this.Val("36") + " " + this.Val("37")).Trim());
        if (id == "68" || id == "69" || id == "updatetitle" || id == "IMPORT" || id == "4004" || id == "4005" || id == "4006" || id == "4007")
          this.SetVal("1602", (this.Val("68") + " " + this.Val("69")).Trim());
        this.updateCoBorrowerTitle();
        if (id == "updatetitle")
          return;
      }
      else
        this.updateCoBorrowerTitle();
      if ((id == "1481" || id == "364") && this.Val("1481") == "Y")
        this.SetVal("305", this.Val("364"));
      string str = this.Val("1172");
      if (id == "1172" && str == "FarmersHomeAdministration")
      {
        this.SetVal("1757", "Loan Amount");
        this.SetVal("1775", "Y");
      }
      if (str == "FarmersHomeAdministration")
        this.SetVal("USDA.X43", "Section 502");
      else
        this.SetVal("USDA.X43", "");
      if (str == "FHA")
        this.SetVal("2978", "");
      if (!this.calObjs.SkipLockRequestSync && !this.calObjs.RegzCal.UseBestCaseScenario && !this.calObjs.RegzCal.UseWorstCaseScenario)
      {
        for (int index = 0; index < LockRequestLog.RequestFieldMap.Count; ++index)
        {
          if ((id == LockRequestLog.RequestFieldMap[index].Value || id == "IMPORT") && !(LockRequestLog.RequestFieldMap[index].Value == "353") && !(LockRequestLog.RequestFieldMap[index].Value == "976") && !(LockRequestLog.RequestFieldMap[index].Value == "3"))
          {
            if (id == "IMPORT")
            {
              this.SetVal(LockRequestLog.RequestFieldMap[index].Key, this.Val(LockRequestLog.RequestFieldMap[index].Value));
            }
            else
            {
              this.SetVal(LockRequestLog.RequestFieldMap[index].Key, val);
              break;
            }
          }
        }
      }
      if (id == "19" || id == "MORNET.X40" || id == "2997" || id == "2998" || id == "2999" || id == "3000")
      {
        string val1 = this.Val("19");
        if (val1 != string.Empty && this.Val("MORNET.X40") != string.Empty)
          val1 = val1 + ";" + this.Val("MORNET.X40");
        if (val1 != string.Empty && this.Val("2997") == "Y")
          val1 += ";Energy Efficient Mortgage";
        if (val1 != string.Empty && this.Val("2998") == "Y")
          val1 += ";Building On Own Land";
        if (val1 != string.Empty && this.Val("2999") == "Y")
          val1 += ";HUD REO";
        if (val1 != string.Empty && this.Val("3000") == "Y")
          val1 += ";203(k)";
        this.SetVal("3031", val1);
      }
      if (HUDGFE2010Fields.PREPAYMENTPENALTYFIELDS.Contains(id) || id == "LOANTERMTABLE.CUSTOMIZE" && this.UseNew2015GFEHUD)
      {
        this.SetVal("675", "N");
        this.SetVal("RE88395.X322", "N");
        for (int index = 0; index < HUDGFE2010Fields.PREPAYMENTPENALTYFIELDS.Count; ++index)
        {
          if (this.Val(HUDGFE2010Fields.PREPAYMENTPENALTYFIELDS[index]) != string.Empty)
          {
            this.SetVal("675", "Y");
            this.SetVal("RE88395.X322", "Y");
            break;
          }
        }
      }
      else
      {
        if (!(id == "675") || !(val == "N"))
          return;
        for (int index = 0; index < HUDGFE2010Fields.PREPAYMENTPENALTYFIELDS.Count; ++index)
          this.SetVal(HUDGFE2010Fields.PREPAYMENTPENALTYFIELDS[index], "");
      }
    }

    private void copyBuyDownToLockRequest(string id, string val)
    {
      switch (id)
      {
        case "CASASRN.X141":
          this.SetVal("4631", val);
          break;
        case "4645":
          this.SetVal("4632", val);
          break;
      }
      if (string.Equals(this.Val("CASASRN.X141"), "Borrower"))
      {
        int num1 = 4633;
        int num2 = 1269;
        while (num1 <= 4638)
        {
          this.SetVal(string.Concat((object) num1), this.Val(string.Concat((object) num2)));
          ++num1;
          ++num2;
        }
        int num3 = 4639;
        int num4 = 1613;
        while (num3 <= 4644)
        {
          this.SetVal(string.Concat((object) num3), this.Val(string.Concat((object) num4)));
          ++num3;
          ++num4;
        }
      }
      else
      {
        int num5 = 4633;
        int num6 = 4535;
        while (num5 <= 4644)
        {
          this.SetVal(string.Concat((object) num5), this.Val(string.Concat((object) num6)));
          ++num5;
          ++num6;
        }
      }
    }

    private void updateCoBorrowerTitle()
    {
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string empty = string.Empty;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (index > 0)
        {
          string str = this.Val("36", borrowerPairs[index]) + " " + this.Val("37", borrowerPairs[index]);
          if (str.Trim() != string.Empty)
          {
            this.SetVal("1602", str.Trim(), borrowerPairs[0]);
            break;
          }
        }
        string str1 = this.Val("68", borrowerPairs[index]) + " " + this.Val("69", borrowerPairs[index]);
        if (str1.Trim() != string.Empty)
        {
          this.SetVal("1602", str1.Trim(), borrowerPairs[0]);
          break;
        }
      }
    }

    internal void PopulateCompanyStateLicense(string id, string val)
    {
      bool clearFieldIfNotFound = true;
      string companyName = this.Val("315");
      if (string.IsNullOrEmpty(companyName))
      {
        if (!clearFieldIfNotFound)
          return;
        this.SetVal("3629", "");
      }
      else
      {
        string streetAddress = this.Val("319");
        string loID = this.Val("LOID");
        if (string.IsNullOrWhiteSpace(loID))
          loID = this.Val("3239");
        OrgInfo withStateLicensing = this.calObjs.SessionObjects.OrganizationManager.GetFirstOrganizationWithStateLicensing(companyName, streetAddress, loID);
        if (withStateLicensing != null && withStateLicensing.OrgBranchLicensing != null)
        {
          this.PopulateCompanyStateLicense(withStateLicensing.OrgBranchLicensing, clearFieldIfNotFound);
        }
        else
        {
          if (!clearFieldIfNotFound)
            return;
          this.SetVal("3629", "");
        }
      }
    }

    internal void PopulateCompanyStateLicense(
      BranchExtLicensing branchLicense,
      bool clearFieldIfNotFound)
    {
      string stateName = this.Val("14");
      if (!string.IsNullOrEmpty(stateName) && branchLicense != null)
      {
        List<StateLicenseExtType> licenses = branchLicense.GetLicenses(stateName);
        if (licenses != null && licenses.Count > 0)
        {
          for (int index = 0; index < licenses.Count; ++index)
          {
            if (licenses[index].Selected && licenses[index].LicenseType != "NL")
            {
              this.SetVal("3629", licenses[index].LicenseNo ?? "");
              if (!(this.Val("1969") == "Y"))
                return;
              this.SetVal("3032", this.Val("3629"));
              return;
            }
          }
        }
      }
      if (!clearFieldIfNotFound)
        return;
      this.SetVal("3629", "");
      if (!(this.Val("1969") == "Y"))
        return;
      this.SetVal("3032", this.Val("3629"));
    }

    private void copyBrokerToLender(string id, string val)
    {
      if (this.Val("1969") == "Y")
      {
        this.SetVal("1264", this.Val("315"));
        this.SetVal("1257", this.Val("319"));
        this.SetVal("1258", this.Val("313"));
        this.SetVal("1259", this.Val("321"));
        this.SetVal("1260", this.Val("323"));
        this.SetVal("1263", this.Val("326"));
        this.SetVal("1256", this.Val("317"));
        this.SetVal("1262", this.Val("1406") == string.Empty ? this.Val("324") : this.Val("1406"));
        this.SetVal("95", this.Val("1407"));
        this.SetVal("3032", this.Val("3629"));
        this.SetVal("3244", this.Val("3237"));
      }
      if (this.Val("2626") == "Banked - Wholesale")
      {
        this.SetVal("3342", this.Val("1264"));
        this.SetVal("3343", this.Val("1257"));
        this.SetVal("3344", this.Val("1258"));
        this.SetVal("3345", this.Val("1259"));
        this.SetVal("3346", this.Val("1260"));
      }
      else
      {
        this.SetVal("3342", this.Val("315"));
        this.SetVal("3343", this.Val("319"));
        this.SetVal("3344", this.Val("313"));
        this.SetVal("3345", this.Val("321"));
        this.SetVal("3346", this.Val("323"));
      }
      string empty = string.Empty;
      if (id == "1149")
      {
        string val1 = this.Val("VEND.X293");
        if (val == "ThirdParty" && val1 != string.Empty)
          this.SetVal("1133", val1);
        else if (val != string.Empty && val != "Seller")
          this.SetVal("1133", this.Val("315"));
        else
          this.SetVal("1133", "");
      }
      if (!(this.Val("3639") == "Y"))
        return;
      this.SetVal("3632", this.Val("1264"));
      this.SetVal("3633", this.Val("1257"));
      this.SetVal("3634", this.Val("1258"));
      this.SetVal("3635", this.Val("1259"));
      this.SetVal("3636", this.Val("1260"));
      this.SetVal("3637", this.Val("3244"));
    }

    private void copyBorrowersToLockRequest(string id, string val)
    {
      this.d1003CalculationServant.CopyBorrowersToLockRequest(id, val);
    }

    private void copyLoanToLockRequestAdditionalFields(string id, string val)
    {
      if (!this.loan.Settings.FieldSettings.LockRequestAdditionalFields.IsAdditionalField(id, true))
        return;
      this.SetVal(LockRequestCustomField.GenerateCustomFieldID(id), this.Val(id));
    }

    private void copyLenderInfoToSSNCompany(string id, string val)
    {
      if (this.Val("1264") != "")
      {
        this.SetVal("3537", this.Val("1264"));
        this.SetVal("3538", this.Val("1257"));
        this.SetVal("3539", this.Val("1258"));
        this.SetVal("3540", this.Val("1259"));
        this.SetVal("3541", this.Val("1260"));
      }
      else
      {
        if (!(this.Val("VEND.X293") != ""))
          return;
        this.SetVal("3537", this.Val("VEND.X293"));
        this.SetVal("3538", this.Val("VEND.X294"));
        this.SetVal("3539", this.Val("VEND.X295"));
        this.SetVal("3540", this.Val("VEND.X296"));
        this.SetVal("3541", this.Val("VEND.X297"));
      }
    }

    private void calculationField1109(string id, string val)
    {
      this.calObjs.D1003Cal.PopulateOtherFields(id, val);
      if (this.Val("1172") == "FarmersHomeAdministration")
        this.calObjs.PrequalCal.CalcUSDAMIP(id, val);
      this.calObjs.PrequalCal.CalcMIP(id, val);
      if (!this.CalculationObjects.SkipLockRequestSync)
        this.calObjs.PrequalCal.CalcDownPay(id, val);
      this.calObjs.VERIFCal.CalcHelocLineTotal(id, val);
      this.calObjs.ToolCal.CalcLOCompensation(id, val);
      this.calObjs.RegzCal.BuildPaySchedule(id, val);
      this.calObjs.FHACal.CalcMACAWP(id, val);
      this.calObjs.FHACal.CalcExisting23KDebt((string) null, (string) null);
      this.calObjs.FHACal.CalcMAX23K(id, val);
      this.calObjs.GFECal.CalcPrepaid(id, val);
      this.calObjs.D1003Cal.CalcHousingExp(id, val);
      this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes(id, val);
      this.calObjs.PrequalCal.CalcRentOwn(id, val);
      this.calObjs.GFECal.CalcGFEFees(id, val);
      this.calObjs.Hud1Cal.CalcHUD1ES(id, val);
      this.calObjs.GFECal.CalcOthers(id, val);
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015AllFeeDetails(id, val);
      if (this.UseNewGFEHUD && id != null)
        this.calObjs.NewHudCal.CopySpecialHUD2010ToGFE2010(id, val);
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        this.calObjs.NewHudCal.FormCal(id, val);
      if (this.UseNew2015GFEHUD)
        new UCDXmlExporter(this.loan).TriggerCalculation();
      this.calObjs.RegzCal.CalculateHELOCPayment(id, val);
      this.calObjs.D1003Cal.CalcHousingExp(id, val);
      this.calObjs.MLDSCal.UpdateInsurance(id, val);
      this.calObjs.LoansubCal.CalcLoansub(id, val);
      this.calObjs.Cal.CalcLTV(id, val);
      this.calObjs.ToolCal.CalcProfit(id, val);
      this.calObjs.PrequalCal.CalcMinMax(id, val);
      this.calObjs.VACal.CalcVARRRWS(id, val);
      this.calObjs.RegzCal.CalcAPR(id, val);
      this.calObjs.GFECal.CalcPrepaid(id, val);
      this.calObjs.MLDSCal.CalcMLDSScenarios(id, val);
      this.calObjs.HMDACal.CalcPrepaymentPenaltyPeriod(id, val);
      this.calObjs.PrequalCal.CalcLockRequestLoan(id, val);
      this.calObjs.D1003Cal.CalcConformingLimit(id, val);
      this.calObjs.FHACal.CalcEEMWorksheet(id, val);
      this.calObjs.ToolCal.CalcSafeHarbor(id, val);
      this.calObjs.ATRQMCal.CalcPrepaymentPenaltyPercent(id, val);
      this.calObjs.ATRQMCal.CalcDiscountPoints(id, val);
      this.calObjs.VACal.CalcVARecoupment(id, val);
      this.calObjs.NewHud2015Cal.Calc60Payments(id, val);
    }

    private void calculateInterestOnly(string id, string val)
    {
      if (this.Val("1177") == "")
        this.SetVal("Terms.IntrOnly", "");
      else if (this.IntVal("1177") > 0)
        this.SetVal("Terms.IntrOnly", "Y");
      else
        this.SetVal("Terms.IntrOnly", "N");
    }

    private void copyPropertyCountyToJurisdiction(string id, string val)
    {
      this.SetVal("3558", "County");
      this.SetVal("3559", this.Val("13"));
    }

    private void calculate2018DI(string id, string val)
    {
      if (id == "1825" && val == "2020")
        this.SetVal("4142", "Y");
      if (id == "HMDA.X27" && (this.Val("HMDA.X27") == string.Empty ? 0 : (int) short.Parse(this.Val("HMDA.X27"), NumberStyles.AllowThousands)) >= 2018 && this.Val("1825") != "2020")
        this.SetVal("1825", "2009");
      if (!this.USEURLA2020 || this.calObjs.AllowURLA2020)
        return;
      this.SetVal("1825", "2009");
    }

    private void calculateULI(string id, string val)
    {
      if (this.Val("HMDA.X113") == "Y" && this.loan.Settings.HMDAInfo != null && this.loan.Settings.HMDAInfo.HMDANuli)
      {
        this.SetVal("HMDA.X28", this.loan.GetField("364"));
      }
      else
      {
        string a = this.Val("2626");
        if (string.Equals(a, "Brokered") || string.Equals(a, "Correspondent"))
          return;
        string LEI = string.Empty;
        if (string.Equals(this.Val("HMDA.X105"), "Reporting"))
          LEI = this.Val("HMDA.X106");
        else if (this.loan.Settings.HMDAInfo != null)
          LEI = this.loan.Settings.HMDAInfo.HMDALEI;
        if (string.IsNullOrEmpty(LEI))
          return;
        string checkDigit = D1003Calculation.GenerateCheckDigit(LEI, this.loan.GetField("364"), this.IsLocked("HMDA.X28"));
        if (string.IsNullOrEmpty(checkDigit))
          return;
        this.SetVal("HMDA.X28", checkDigit);
      }
    }

    public static string GenerateCheckDigit(string LEI, string loanNumber, bool isLocked)
    {
      return D1003CalculationServant.GenerateCheckDigit(LEI, loanNumber, isLocked);
    }

    private void calculateEthnicityRaceSex(string id, string val)
    {
      switch (id)
      {
        case "1523":
          if (val != "Hispanic or Latino")
          {
            this.SetVal("4125", "");
            this.SetVal("4144", "");
            this.SetVal("4145", "");
            this.SetVal("4146", "");
            this.SetVal("4147", "");
            break;
          }
          break;
        case "1524":
        case "1525":
        case "1526":
        case "1527":
        case "1528":
        case "4148":
        case "4149":
        case "4150":
        case "4151":
        case "4152":
        case "4153":
        case "4154":
        case "4155":
        case "4156":
        case "4157":
        case "4158":
        case "4252":
          if (val == "Y")
          {
            this.SetVal("1530", "");
            if (id != "4252")
            {
              this.SetVal("4244", "");
              this.SetVal("1529", "");
              break;
            }
            break;
          }
          break;
        case "1529":
          if (val == "Y")
          {
            for (int index = 1524; index <= 1530; ++index)
            {
              if (index != 1529)
                this.SetVal(index.ToString(), "");
            }
            this.SetVal("4244", "Y");
            break;
          }
          if (this.Val("4244") == "Y")
          {
            this.SetVal("4244", "");
            break;
          }
          break;
        case "1530":
          if (val == "Y")
          {
            this.SetVal("4244", "");
            this.SetVal("1529", "");
            break;
          }
          break;
        case "1531":
          if (val != "Y")
          {
            this.SetVal("4136", "");
            this.SetVal("4159", "");
            this.SetVal("4160", "");
            this.SetVal("4161", "");
            this.SetVal("4162", "");
            break;
          }
          break;
        case "1532":
        case "1533":
        case "1534":
        case "1535":
        case "1536":
        case "1538":
        case "4163":
        case "4164":
        case "4165":
        case "4166":
        case "4167":
        case "4168":
        case "4169":
        case "4170":
        case "4171":
        case "4172":
        case "4173":
        case "4253":
          if (val == "Y")
          {
            if (id != "1538")
              this.SetVal("1538", "");
            this.SetVal("3174", "");
            if (id != "4253")
            {
              this.SetVal("4247", "");
              this.SetVal("1537", "");
              break;
            }
            break;
          }
          break;
        case "1537":
          if (val == "Y")
          {
            for (int index = 1532; index <= 1538; ++index)
            {
              if (index != 1537)
                this.SetVal(index.ToString(), "");
            }
            this.SetVal("4247", "Y");
            break;
          }
          if (this.Val("4247") == "Y")
          {
            this.SetVal("4247", "");
            break;
          }
          break;
        case "4131":
          if (val == "FaceToFace")
          {
            this.SetVal("4246", "");
            this.SetVal("4247", "");
            this.SetVal("4248", "");
            break;
          }
          break;
        case "4143":
          if (val == "FaceToFace")
          {
            this.SetVal("4243", "");
            this.SetVal("4244", "");
            this.SetVal("4245", "");
            break;
          }
          break;
        case "4244":
          if (val == "Y")
          {
            for (int index = 1524; index <= 1530; ++index)
              this.SetVal(index.ToString(), index == 1529 ? "Y" : "");
            for (int index = 4148; index <= 4158; ++index)
              this.SetVal(index.ToString(), "");
            this.SetVal("4126", "");
            this.SetVal("4128", "");
            this.SetVal("4130", "");
            break;
          }
          if (this.Val("1529") == "Y")
          {
            this.SetVal("1529", "");
            break;
          }
          break;
        case "4247":
          if (val == "Y")
          {
            for (int index = 1532; index <= 1538; ++index)
              this.SetVal(index.ToString(), index == 1537 ? "Y" : "");
            for (int index = 4163; index <= 4173; ++index)
              this.SetVal(index.ToString(), "");
            this.SetVal("3174", "");
            this.SetVal("4137", "");
            this.SetVal("4139", "");
            this.SetVal("4141", "");
            break;
          }
          if (this.Val("1537") == "Y")
          {
            this.SetVal("1537", "");
            break;
          }
          break;
      }
      if (id == "1530" || !(id == "3840"))
        return;
      this.SetVal("4188", val);
      this.SetVal("4189", val);
      if (val == "Y")
      {
        this.SetVal("1531", "No co-applicant");
        this.SetVal("4132", "NoCoApplicant");
        this.SetVal("4133", "NoCoApplicant");
        this.SetVal("4134", "NoCoApplicant");
        this.SetVal("4213", "");
        this.SetVal("4214", "");
        this.SetVal("4215", "");
        this.SetVal("4206", "");
        this.SetVal("4246", "");
        this.SetVal("4247", "");
        this.SetVal("4248", "");
        this.SetVal("4253", "");
        for (int index = 4159; index <= 4173; ++index)
          this.SetVal(string.Concat((object) index), "");
        this.SetVal("4136", "");
        this.SetVal("4137", "");
        this.SetVal("4139", "");
        this.SetVal("4141", "");
        for (int index = 1532; index <= 1538; ++index)
          this.SetVal(string.Concat((object) index), "");
        this.SetVal("3174", "Y");
        this.SetVal("478", "No co-applicant");
        for (int index = 4197; index <= 4200; ++index)
          this.SetVal(index.ToString(), "");
      }
      else
      {
        if (this.Val("1531") == "No co-applicant")
          this.SetVal("1531", "");
        if (this.Val("3174") == "Y")
          this.SetVal("3174", "");
        if (this.Val("478") == "No co-applicant")
          this.SetVal("478", "");
        this.SetVal("4132", "");
        this.SetVal("4133", "");
        this.SetVal("4134", "");
      }
    }

    internal bool IsPiggybackHELOC
    {
      get
      {
        return this.loan != null && this.loan.LinkedData != null && ((this.loan.LinkedData.GetSimpleField("1172") == "HELOC" || this.loan.LinkedData.GetSimpleField("1172") == "Other") && this.loan.LinkedData.GetSimpleField("420") == "SecondLien" || (this.Val("1172") == "HELOC" || this.Val("1172") == "Other") && this.Val("420") == "SecondLien");
      }
    }

    internal bool IsHELOCOrOtherLoan
    {
      get
      {
        if (this.loan == null)
          return false;
        return this.Val("1172") == "HELOC" || this.Val("1172") == "Other";
      }
    }

    public void RecalculateNMLS()
    {
      this.calculateNMLSLoanType((string) null, (string) null);
      this.calculateNMLSDocType((string) null, (string) null);
      this.calculateNMLSARMType((string) null, (string) null);
      this.calculateNMLSPiggybackHELOCValue((string) null, (string) null);
      this.calculateNMLSRefiPurpose((string) null, (string) null);
      this.calculateLenderChannel((string) null, (string) null);
      this.calculateOccupancy((string) null, (string) null);
      this.calculatePMIIndicator((string) null, (string) null);
      this.calculateConformingLimit((string) null, (string) null);
    }

    private void calculateNMLSLoanType(string id, string val)
    {
      this.SetVal("NMLS.X1", NMLSCalculations.CalculateNMLSLoanType(this.Val("19"), this.Val("1172"), this.Val("420"), this.Val("16")));
    }

    private void calculateNMLSResidentialMortgageType(string id, string val)
    {
      string field608 = this.Val("608");
      string field1172 = this.Val("1172");
      string field3331 = this.Val("3331");
      this.SetVal("NMLS.X2", NMLSCalculations.CalculateNMLSResidentialMortgageType(field1172, field608, field3331));
    }

    private void calculateNMLSDocType(string id, string val)
    {
      string str = this.Val("NMLS.X3");
      if (id == "CASASRN.X144" && val != "")
      {
        if (val == "Z")
          this.SetVal("NMLS.X3", "Full");
        else
          this.SetVal("NMLS.X3", "Alt");
      }
      else if (id == "MORNET.X67" && val != "")
      {
        if (val == "FullDocumentation" || val == "(F) Full Documentation")
          this.SetVal("NMLS.X3", "Full");
        else
          this.SetVal("NMLS.X3", "Alt");
      }
      else if ((id == "CASASRN.X144" || str == "") && this.Val("MORNET.X67") != "")
      {
        if (this.Val("MORNET.X67") == "FullDocumentation" || this.Val("MORNET.X67") == "(F) Full Documentation")
          this.SetVal("NMLS.X3", "Full");
        else
          this.SetVal("NMLS.X3", "Alt");
      }
      else if ((id == "MORNET.X67" || str == "") && this.Val("CASASRN.X144") != "")
      {
        if (this.Val("CASASRN.X144") == "Z")
          this.SetVal("NMLS.X3", "Full");
        else
          this.SetVal("NMLS.X3", "Alt");
      }
      else
      {
        if (!(this.Val("CASASRN.X144") == "") || !(this.Val("MORNET.X67") == ""))
          return;
        this.SetVal("NMLS.X3", "");
      }
    }

    private void calculateNMLSARMType(string id, string val)
    {
      string field608 = this.Val("608");
      if (field608 == "AdjustableRate")
      {
        if (this.IsLocked("NMLS.X4"))
          return;
        this.SetVal("NMLS.X4", "");
        if (this.loan == null)
          return;
        this.loan.AddLock("NMLS.X4");
      }
      else
      {
        if (this.IsLocked("NMLS.X4") && this.loan != null)
          this.loan.RemoveLock("NMLS.X4");
        this.SetVal("NMLS.X4", NMLSCalculations.CalculateNMLSARMType(field608));
      }
    }

    private void calculateNMLSPiggybackHELOCValue(string id, string val)
    {
      this.SetVal("NMLS.X5", NMLSCalculations.CalculateNMLSSecondLienType(this.Val("428"), this.Val("1732")));
    }

    private void calculateNMLSRefiPurpose(string id, string val)
    {
      this.SetVal("NMLS.X6", NMLSCalculations.CalculateNMLSRefiPurpose(this.Val("19"), this.Val("299")));
    }

    private void calculateLenderChannel(string id, string val)
    {
      this.SetVal("3332", NMLSCalculations.CalculateLenderChannel(this.Val("2626")));
    }

    private void calculateOccupancy(string id, string val)
    {
      this.SetVal("3335", NMLSCalculations.CalculateOccupancyType(this.Val("1811", this.GetBorrowerPairs()[0])));
    }

    private void calculatePMIIndicator(string id, string val)
    {
      this.SetVal("3336", NMLSCalculations.CalculatePMIIndicator(this.Val("1766"), this.Val("1045")));
    }

    private void calculateConformingLimit(string id, string val)
    {
      if (this.loan == null)
        return;
      try
      {
        this.SetVal("3331", ConformingLoanLimits.IsConforming(this.loan, this.sessionObjects) ? "Conforming" : "Jumbo");
      }
      catch
      {
        this.SetVal("3331", "");
      }
      this.calculateNMLSResidentialMortgageType(id, val);
    }

    internal string InitialApplicaitonDateID => this.initialApplicaitonDateID;

    internal void AddInitialApplicationDateTrigger(string initialApplicaitonDateID)
    {
      this.initialApplicaitonDateID = initialApplicaitonDateID;
      if (!(initialApplicaitonDateID != "") || !(initialApplicaitonDateID != "3142"))
        return;
      this.AddFieldHandler(initialApplicaitonDateID, this.calObjs.PrequalCal.CalcMIP);
    }

    private void populateLoanAmountToNmlsApplicationAmounts(string id, string val)
    {
      if (!Utils.IsDate((object) val) || !(this.Val("NMLS.X11") == ""))
        return;
      this.SetCurrentNum("NMLS.X11", this.FltVal("2"));
    }

    internal void CalcBorrowerDependentFields()
    {
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (!(borrowerPairs[index].Id == currentBorrowerPair.Id))
        {
          this.loan.SetBorrowerPair(borrowerPairs[index]);
          this.calculateBorrowerIncome((string) null, (string) null);
          if (this.USEURLA2020)
          {
            this.loan.TriggerCalculation("119", this.Val("119"));
            this.loan.TriggerCalculation("120", this.Val("120"));
          }
        }
      }
      if (currentBorrowerPair != null && (this.loan.CurrentBorrowerPair == null || currentBorrowerPair.Id != this.loan.CurrentBorrowerPair.Id))
        this.loan.SetBorrowerPair(currentBorrowerPair);
      this.calObjs.D1003Cal.CalcHousingExp((string) null, (string) null);
    }

    internal void calculateLienPosition(string id, string val)
    {
      string str1 = this.Val("420");
      string str2 = this.Val("4494");
      switch (str1)
      {
        case "FirstLien":
          this.SetVal("4494", "1");
          break;
        case "SecondLien":
          if (!(str2 == "1") && !(str2 == ""))
            break;
          this.SetVal("4494", "2");
          break;
        default:
          this.SetVal("4494", "");
          break;
      }
    }

    private void clearIndexDate(string id, string val)
    {
      if (!(id == "688") || !string.IsNullOrEmpty(this.Val("688")))
        return;
      this.SetVal("4898", "");
    }
  }
}
