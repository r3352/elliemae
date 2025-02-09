// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.USDACalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class USDACalculation : CalculationBase
  {
    private const string className = "USDACalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcIncomeWorksheet;
    internal Routine CalcLoanFundsUsage;
    internal Routine CalcRefinanceIndicator;
    internal Routine CalcFirstTimeHomeBuyer;
    internal Routine CalcRHSEmployee;
    internal Routine ClearLine819;
    internal Routine ClearMIPFields;
    internal Routine UpdateField234;
    internal Routine CopyPOCPTCAPRFromLine902ToLine819;
    internal Routine CopyPOCPTCAPRFromLine819ToLine902;
    internal Routine CopyPOCPTCAPRFromLine1003ToLine1010;
    internal Routine CalcBorrowerId;
    private readonly UsdaCalculationServant _usdaCalculationServant;

    internal event EventHandler OnUSDAChanged;

    internal USDACalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcIncomeWorksheet = this.RoutineX(new Routine(this.calculateIncomeWorksheet));
      this.CalcLoanFundsUsage = this.RoutineX(new Routine(this.calculateLoanFundsUsage));
      this.CalcRefinanceIndicator = this.RoutineX(new Routine(this.calculateRefinanceIndicator));
      this.CalcFirstTimeHomeBuyer = this.RoutineX(new Routine(this.calculateFirstTimeHomeBuyer));
      this.CalcRHSEmployee = this.RoutineX(new Routine(this.calculateRHSEmployee));
      this.ClearLine819 = this.RoutineX(new Routine(this.clearLine819));
      this.ClearMIPFields = this.RoutineX(new Routine(this.clearMIPFields));
      this.UpdateField234 = this.RoutineX(new Routine(this.updateField234));
      this.CopyPOCPTCAPRFromLine902ToLine819 = this.RoutineX(new Routine(this.copyPOCPTCAPRFromLine902ToLine819));
      this.CopyPOCPTCAPRFromLine819ToLine902 = this.RoutineX(new Routine(this.copyPOCPTCAPRFromLine819ToLine902));
      this.CopyPOCPTCAPRFromLine1003ToLine1010 = this.RoutineX(new Routine(this.copyPOCPTCAPRFromLine1003ToLine1010));
      this.CalcBorrowerId = this.RoutineX(new Routine(this.calculateBorrowerId));
      this.addFieldHandlers();
      this._usdaCalculationServant = new UsdaCalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateIncomeWorksheet));
      this.AddFieldHandler("USDA.X52", routine1);
      this.AddFieldHandler("USDA.X60", routine1);
      this.AddFieldHandler("USDA.X68", routine1);
      this.AddFieldHandler("USDA.X76", routine1);
      this.AddFieldHandler("USDA.X84", routine1);
      this.AddFieldHandler("USDA.X92", routine1);
      this.AddFieldHandler("USDA.X96", routine1);
      this.AddFieldHandler("USDA.X97", routine1);
      this.AddFieldHandler("USDA.X164", routine1);
      this.AddFieldHandler("USDA.X165", routine1);
      this.AddFieldHandler("USDA.X167", routine1);
      this.AddFieldHandler("USDA.X168", routine1);
      this.AddFieldHandler("USDA.X170", routine1);
      this.AddFieldHandler("USDA.X172", routine1);
      this.AddFieldHandler("USDA.X173", routine1);
      this.AddFieldHandler("USDA.X174", routine1);
      this.AddFieldHandler("USDA.X175", routine1);
      this.AddFieldHandler("USDA.X176", routine1);
      this.AddFieldHandler("USDA.X182", routine1);
      this.AddFieldHandler("USDA.X183", routine1);
      this.AddFieldHandler("USDA.X185", routine1);
      for (int index = 49; index <= 94; ++index)
        this.AddFieldHandler("USDA.X" + (object) index, routine1);
      for (int index = 187; index <= 192; ++index)
        this.AddFieldHandler("USDA.X" + (object) index, routine1);
      this.AddFieldHandler("USDA.X202", routine1);
      this.AddFieldHandler("USDA.X204", routine1);
      this.AddFieldHandler("USDA.X16", routine1);
      this.AddFieldHandler("USDA.X17", routine1);
      this.AddFieldHandler("USDA.X240", routine1);
      this.AddFieldHandler("USDA.X241", routine1);
      for (int index = 224; index <= 238; ++index)
        this.AddFieldHandler("USDA.X" + (object) index, routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateLoanFundsUsage));
      this.AddFieldHandler("USDA.X22", routine2);
      this.AddFieldHandler("USDA.X24", routine2);
      this.AddFieldHandler("USDA.X198", routine2);
      this.AddFieldHandler("USDA.X217", routine2);
      this.AddFieldHandler("USDA.X7", this.RoutineX(new Routine(this.calculateRefinanceIndicator)));
      Routine routine3 = this.RoutineX(new Routine(this.calculateFirstTimeHomeBuyer));
      this.AddFieldHandler("418", routine3);
      this.AddFieldHandler("1343", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.calculateRHSEmployee));
      this.AddFieldHandler("USDA.X44", routine4);
      this.AddFieldHandler("USDA.X46", routine4);
      this.AddFieldHandler("USDA.X10", routine4);
      this.AddFieldHandler("USDA.X13", routine4);
      this.AddFieldHandler("USDA.X220", this.RoutineX(new Routine(this.calculateBorrowerId)));
    }

    private double addUrla2020OtherIncome(bool isBaseIncome, bool isBorrower)
    {
      int otherIncomeSources = this.loan.GetNumberOfOtherIncomeSources();
      string str1 = "URLAROIS";
      double num1 = 0.0;
      for (int index = 1; index <= otherIncomeSources; ++index)
      {
        string str2 = str1 + index.ToString("00");
        string str3 = this.Val(str2 + "18");
        string str4 = this.Val(str2 + "02");
        double num2 = this.FltVal(str2 + "22");
        if (isBaseIncome)
        {
          switch (str3.ToLower())
          {
            case "alimony":
            case "childsupport":
            case "fostercare":
            case "notesreceivableinstallment":
            case "other":
            case "pension":
            case "socialsecurity":
            case "tipincome":
            case "trust":
              if (isBorrower && str4 == "Borrower")
              {
                num1 += num2;
                continue;
              }
              if (!isBorrower && str4 == "CoBorrower")
              {
                num1 += num2;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
        else
        {
          switch (str3.ToLower())
          {
            case "accessoryunitincome":
            case "automobileallowance":
            case "boarderincome":
            case "capitalgains":
            case "definedcontributionplan":
            case "disability":
            case "dividendsinterest":
            case "employmentrelatedaccount":
            case "housingallowance":
            case "housingchoicevoucherprogram":
            case "mortgagecreditcertificate":
            case "mortgagedifferential":
            case "nonborrowerhouseholdincome":
            case "publicassistance":
            case "royalties":
            case "separatemaintenance":
            case "temporaryleave":
            case "unemployment":
            case "vabenefitsnoneducational":
              num1 += num2;
              continue;
            default:
              continue;
          }
        }
      }
      return num1;
    }

    private void calculateIncomeWorksheet(string id, string val)
    {
      bool flag = this.Val("1825") == "2020";
      double num1 = this.FltVal("101") + this.FltVal("102") + this.FltVal("103") + this.FltVal("104") + this.FltVal("107");
      if (flag)
      {
        num1 += this.addUrla2020OtherIncome(true, true);
      }
      else
      {
        if (this.Val("144") == "B")
          num1 += this.addSpecificOtherIncome("145", "146", true);
        if (this.Val("147") == "B")
          num1 += this.addSpecificOtherIncome("148", "149", true);
        if (this.Val("150") == "B")
          num1 += this.addSpecificOtherIncome("151", "152", true);
      }
      this.SetCurrentNum("USDA.X164", num1 * 12.0);
      double num2 = this.FltVal("110") + this.FltVal("111") + this.FltVal("112") + this.FltVal("113") + this.FltVal("116");
      if (flag)
      {
        num2 += this.addUrla2020OtherIncome(true, false);
      }
      else
      {
        if (this.Val("144") == "C")
          num2 += this.addSpecificOtherIncome("145", "146", true);
        if (this.Val("147") == "C")
          num2 += this.addSpecificOtherIncome("148", "149", true);
        if (this.Val("150") == "C")
          num2 += this.addSpecificOtherIncome("151", "152", true);
      }
      this.SetCurrentNum("USDA.X165", num2 * 12.0);
      double num3;
      if (flag)
      {
        num3 = this.addUrla2020OtherIncome(false, false);
        int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
        string str1 = "BE";
        for (int index = 1; index <= numberOfEmployer1; ++index)
        {
          string str2 = str1 + index.ToString("00");
          if (this.Val(str2 + "09") == "Y")
            num3 += this.FltVal(str2 + "53");
        }
        int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
        string str3 = "CE";
        for (int index = 1; index <= numberOfEmployer2; ++index)
        {
          string str4 = str3 + index.ToString("00");
          if (this.Val(str4 + "09") == "Y")
            num3 += this.FltVal(str4 + "53");
        }
      }
      else
        num3 = this.addSpecificOtherIncome("145", "146", false) + this.addSpecificOtherIncome("148", "149", false) + this.addSpecificOtherIncome("151", "152", false);
      this.SetCurrentNum("USDA.X168", num3 * 12.0);
      this.SetCurrentNum("USDA.X170", this.FltVal("906") > 0.0 ? (this.FltVal("105") + this.FltVal("114") + this.FltVal("906")) * 12.0 : (this.FltVal("105") + this.FltVal("114")) * 12.0);
      double num4 = 0.0;
      int num5 = 186;
      int num6 = 0;
      for (int index = 49; index <= 233; index += 8)
      {
        if (index <= 90 || index >= 224)
        {
          if (index == 225)
            num5 = 239;
          if (!(this.Val("USDA.X" + (object) ++num5) != string.Empty))
          {
            int num7 = this.IntVal("USDA.X" + (object) index);
            if (num7 >= 18)
              num4 += this.FltVal("USDA.X" + (object) (index + 3)) + this.FltVal("USDA.X" + (object) (index + 5));
            if (num7 > 0 && num7 < 18)
              ++num6;
            else if (this.Val("USDA.X" + (object) (index + 1)) == "Y")
              ++num6;
            else if (this.Val("USDA.X" + (object) (index + 2)) == "Y")
              ++num6;
          }
        }
      }
      this.SetCurrentNum("USDA.X167", num4);
      this.SetCurrentNum("USDA.X185", (double) num6);
      this.SetCurrentNum("USDA.X172", this.FltVal("USDA.X185") * 480.0);
      this.SetCurrentNum("USDA.X173", this.FltVal("USDA.X96") == 0.0 ? this.FltVal("USDA.X97") * 12.0 : this.FltVal("USDA.X96") * 52.0);
      double num8 = 0.0;
      for (int index = 172; index <= 176; ++index)
        num8 += this.FltVal("USDA.X" + (object) index);
      this.SetCurrentNum("USDA.X177", num8);
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double num9 = this.FltVal("USDA.X167");
      double num10 = num9;
      foreach (BorrowerPair pair in borrowerPairs)
      {
        double num11 = this.FltVal("USDA.X164", pair) + this.FltVal("USDA.X165", pair) + this.FltVal("USDA.X168", pair) + this.FltVal("USDA.X170", pair);
        double num12 = num11 + num9;
        this.SetCurrentNum("USDA.X16", num12, pair);
        this.SetCurrentNum("USDA.X17", num12 - num8, pair);
        num10 += num11;
      }
      this.SetCurrentNum("USDA.X222", num10);
      this.SetCurrentNum("USDA.X223", num10 - num8);
      this.SetCurrentNum("USDA.X201", this.FltVal("USDA.X182") + this.FltVal("USDA.X183"));
      this.SetCurrentNum("USDA.X206", this.FltVal("USDA.X202") + this.FltVal("USDA.X204"));
      this.SetCurrentNum("USDA.X207", this.FltVal("USDA.X182") + this.FltVal("USDA.X202"));
      this.SetCurrentNum("USDA.X208", this.FltVal("USDA.X183") + this.FltVal("USDA.X204"));
      this.SetCurrentNum("USDA.X184", this.FltVal("USDA.X207") + this.FltVal("USDA.X208"));
    }

    private double addSpecificOtherIncome(
      string incomeTypeID,
      string amoutID,
      bool forChildSupport)
    {
      string lower = this.Val(incomeTypeID).ToLower();
      if (forChildSupport)
      {
        switch (lower)
        {
          case "alimony":
          case "alimonychildsupport":
          case "child support":
          case "fostercare":
          case "notesreceivableinstallment":
          case "otherincome":
          case "pension":
          case "socialsecurity":
          case "tipincome":
          case "trust":
            return this.FltVal(amoutID);
        }
      }
      else
      {
        switch (lower)
        {
          case "accessoryunitincome":
          case "automobileexpenseaccount":
          case "capitalgains":
          case "employmentrelatedassets":
          case "fnmboarderincome":
          case "fnmgovernmentmortgagecreditcertificate":
          case "foreignincome":
          case "militarybasepay":
          case "militaryclothesallowance":
          case "militarycombatpay":
          case "militaryflightpay":
          case "militaryhazardpay":
          case "militaryoverseaspay":
          case "militaryproppay":
          case "militaryquartersallowance":
          case "militaryrationsallowance":
          case "militaryvariablehousingallowance":
          case "mortgagedifferential":
          case "non-borrowerhouseholdincome":
          case "royaltypayment":
          case "seasonalincome":
          case "section8":
          case "temporaryleave":
          case "unemployment":
          case "vabenefitsnoneducational":
            return this.FltVal(amoutID);
        }
      }
      return 0.0;
    }

    private void calculateLoanFundsUsage(string id, string val)
    {
      this.SetCurrentNum("USDA.X217", this.FltVal("NEWHUD.X1585") - this.FltVal("1045"));
      this.SetCurrentNum("USDA.X26", this.FltVal("USDA.X198") + this.FltVal("USDA.X217") + this.FltVal("USDA.X24") + this.FltVal("1045"));
    }

    private void calculateRefinanceIndicator(string id, string val)
    {
      string str = this.Val("19");
      this.SetVal("USDA.X7", str == "NoCash-Out Refinance" || str == "Cash-Out Refinance" ? "Y" : (str != "" ? "N" : ""));
      if (!(this.Val("USDA.X7") != "Y"))
        return;
      this.SetVal("USDA.X8", "");
      this.SetVal("USDA.X218", "");
    }

    private void calculateFirstTimeHomeBuyer(string id, string val)
    {
      switch (id)
      {
        case "403":
        case "418":
          string str1 = this.Val("403");
          string str2 = this.Val("418");
          if (str2 == "Yes" && str1 == "Yes")
          {
            this.SetVal("USDA.X3", "N");
            break;
          }
          if (str2 == "Yes" && str1 == "No")
          {
            this.SetVal("USDA.X3", "Y");
            break;
          }
          this.SetVal("USDA.X3", "");
          break;
        case "1343":
        case "1108":
          string str3 = this.Val("1108");
          string str4 = this.Val("1343");
          if (str4 == "Yes" && str3 == "Yes")
          {
            this.SetVal("USDA.X6", "N");
            break;
          }
          if (str4 == "Yes" && str3 == "No")
          {
            this.SetVal("USDA.X6", "Y");
            break;
          }
          this.SetVal("USDA.X6", "");
          break;
      }
    }

    private void calculateRHSEmployee(string id, string val)
    {
      if (this.Val("USDA.X10") != "Y")
      {
        this.SetVal("USDA.X11", "");
        this.SetVal("USDA.X45", "");
      }
      if (!(this.Val("USDA.X13") != "Y"))
        return;
      this.SetVal("USDA.X14", "");
      this.SetVal("USDA.X47", "");
    }

    private void updateField234(string id, string val)
    {
      if (!(this.Val("1172") != "FarmersHomeAdministration") || this.FltVal("NEWHUD.X1707") == 0.0)
        return;
      this.SetCurrentNum("234", this.FltVal("234") - this.FltVal("NEWHUD.X1707"));
    }

    private void copyPOCPTCAPRFromLine819ToLine902(string id, string val)
    {
      if (!this.IsLocked("NEWHUD.X1301") || string.Compare(this.Val("NEWHUD.X1299"), "Guarantee Fee", true) != 0)
        return;
      this.SetVal("L248", this.Val("NEWHUD.X1300"));
      this.SetVal("562", this.Val("NEWHUD.X1302"));
      this.SetVal("337", this.Val("NEWHUD.X1301"));
      this.SetVal("NEWHUD.X622", this.Val("NEWHUD.X1526"));
      this.SetVal("SYS.X38", this.Val("NEWHUD.X1305"));
      this.SetVal("SYS.X306", this.Val("NEWHUD.X1306"));
      this.SetVal("SYS.X305", this.Val("NEWHUD.X1303"));
      this.SetVal("SYS.X158", this.Val("NEWHUD.X1304"));
      this.SetVal("NEWHUD.X849", this.Val("NEWHUD.X1441"));
      this.SetVal("NEWHUD.X912", this.Val("NEWHUD.X1464"));
      this.SetVal("PTC.X30", this.Val("PTC.X255"));
      this.SetVal("PTC.X108", this.Val("PTC.X278"));
      this.SetVal("PTC.X186", this.Val("PTC.X301"));
      this.SetVal("POPT.X30", this.Val("POPT.X255"));
      this.SetVal("POPT.X108", this.Val("POPT.X278"));
      this.SetVal("POPT.X186", this.Val("POPT.X301"));
      for (int index = 1587; index <= 1616; ++index)
      {
        if (index != 1591 && index != 1592 && index != 1617 && index != 1618 && index != 1619)
          this.SetVal("NEWHUD2.X" + (object) (index + 594), this.Val("NEWHUD2.X" + (object) index));
      }
      this.SetVal("NEWHUD.X1299", "");
      this.RemoveCurrentLock("NEWHUD.X1301");
      this.SetVal("NEWHUD.X1301", "");
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
      this.loan.AddLock("NEWHUD.X1301");
      this.SetVal("NEWHUD.X1526", "");
      this.SetVal("NEWHUD.X1300", "");
      this.SetVal("NEWHUD.X1302", "");
      this.SetVal("NEWHUD.X1305", "");
      this.SetVal("NEWHUD.X1306", "");
      this.SetVal("NEWHUD.X1303", "");
      this.SetVal("NEWHUD.X1304", "");
      this.SetVal("NEWHUD.X1441", "");
      this.SetVal("NEWHUD.X1464", "");
      this.SetVal("PTC.X255", "");
      this.SetVal("PTC.X278", "");
      this.SetVal("PTC.X301", "");
      this.SetVal("POPT.X255", "");
      this.SetVal("POPT.X278", "");
      this.SetVal("POPT.X301", "");
      for (int index = 1587; index <= 1616; ++index)
      {
        if (index != 1591 && index != 1592 && index != 1617 && index != 1618 && index != 1619)
          this.SetVal("NEWHUD2.X" + (object) index, "");
      }
    }

    private void copyPOCPTCAPRFromLine902ToLine819(string id, string val)
    {
      if (this.IsLocked("NEWHUD.X1301"))
        return;
      this.SetVal("NEWHUD.X1300", this.Val("L248"));
      this.SetVal("NEWHUD.X1301", this.Val("337"));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
      this.SetVal("NEWHUD.X1302", this.Val("562"));
      this.SetVal("NEWHUD.X1526", this.Val("NEWHUD.X622"));
      this.SetVal("NEWHUD.X1305", this.Val("SYS.X38"));
      this.SetVal("NEWHUD.X1306", this.Val("SYS.X306"));
      this.SetVal("NEWHUD.X1303", this.Val("SYS.X305"));
      this.SetVal("NEWHUD.X1304", this.Val("SYS.X158"));
      this.SetVal("NEWHUD.X1441", this.Val("NEWHUD.X849"));
      this.SetVal("NEWHUD.X1464", this.Val("NEWHUD.X912"));
      this.SetVal("PTC.X255", this.Val("PTC.X30"));
      this.SetVal("PTC.X278", this.Val("PTC.X108"));
      this.SetVal("PTC.X301", this.Val("PTC.X186"));
      this.SetVal("POPT.X255", this.Val("POPT.X30"));
      this.SetVal("POPT.X278", this.Val("POPT.X108"));
      this.SetVal("POPT.X301", this.Val("POPT.X186"));
      for (int index = 1587; index <= 1616; ++index)
      {
        if (index != 1591 && index != 1592 && index != 1617 && index != 1618 && index != 1619)
          this.SetVal("NEWHUD2.X" + (object) index, this.Val("NEWHUD2.X" + (object) (index + 594)));
      }
      this.SetVal("L248", "");
      this.SetVal("562", "");
      this.SetVal("SYS.X38", "");
      this.SetVal("SYS.X306", "");
      this.SetVal("SYS.X305", "");
      this.SetVal("SYS.X158", "");
      this.SetVal("NEWHUD.X849", "");
      this.SetVal("NEWHUD.X912", "");
      this.SetVal("PTC.X30", "");
      this.SetVal("PTC.X108", "");
      this.SetVal("PTC.X186", "");
      this.SetVal("POPT.X30", "");
      this.SetVal("POPT.X108", "");
      this.SetVal("POPT.X186", "");
      this.SetVal("NEWHUD.X622", "");
      for (int index = 1587; index <= 1616; ++index)
      {
        if (index != 1591 && index != 1592 && index != 1617 && index != 1618 && index != 1619)
          this.SetVal("NEWHUD2.X" + (object) (index + 594), "");
      }
    }

    private void copyPOCPTCAPRFromLine1003ToLine1010(string id, string val)
    {
      this.SetVal("NEWHUD.X1706", this.Val("1296"));
      this.SetVal("NEWHUD.X1707", this.Val("232"));
      this.SetVal("NEWHUD.X1711", this.Val("SYS.X43"));
      this.SetVal("NEWHUD.X1710", this.Val("SYS.X319"));
      this.SetVal("PTC.X346", this.Val("PTC.X40"));
      this.SetVal("PTC.X347", this.Val("PTC.X118"));
      this.SetVal("PTC.X348", this.Val("PTC.X196"));
      this.SetVal("POPT.X346", this.Val("POPT.X40"));
      this.SetVal("POPT.X347", this.Val("POPT.X118"));
      this.SetVal("POPT.X348", this.Val("POPT.X196"));
      this.SetVal("1296", "");
      this.SetVal("232", "");
      this.SetVal("SYS.X43", "");
      this.SetVal("SYS.X319", "");
      this.SetVal("PTC.X40", "");
      this.SetVal("PTC.X118", "");
      this.SetVal("PTC.X196", "");
      this.SetVal("POPT.X40", "");
      this.SetVal("POPT.X118", "");
      this.SetVal("POPT.X196", "");
    }

    private void clearMIPFields(string id, string val)
    {
      this._usdaCalculationServant.ClearMipFields(id, val);
    }

    private void clearLine819(string id, string val)
    {
      bool flag = true;
      if (id == "1172")
      {
        if (val == "FarmersHomeAdministration" && string.Compare(this.Val("NEWHUD.X1299"), "Guarantee Fee", true) == 0 || val != "FarmersHomeAdministration" && string.Compare(this.Val("NEWHUD.X1299"), "Guarantee Fee", true) != 0)
          flag = false;
        if (val != "FarmersHomeAdministration")
        {
          this.RemoveCurrentLock("NEWHUD.X1301");
          this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
        }
      }
      if (!flag || this.IsLocked("NEWHUD.X1301"))
        return;
      for (int index = 1299; index <= 1304; ++index)
        this.SetVal("NEWHUD.X" + (object) index, "");
      this.SetVal("NEWHUD.X1441", "");
      this.SetVal("NEWHUD.X1464", "");
      this.SetVal("PTC.X255", "");
      this.SetVal("PTC.X278", "");
      this.SetVal("PTC.X301", "");
      this.SetVal("NEWHUD.X1526", "");
      this.SetVal("POPT.X255", "");
      this.SetVal("POPT.X278", "");
      this.SetVal("POPT.X301", "");
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem819("NEWHUD.X1301", "");
    }

    internal void OnUSDACalculationChanged(string id, string val)
    {
      if (this.OnUSDAChanged == null)
        return;
      this.OnUSDAChanged((object) id, new EventArgs());
    }

    private void calculateBorrowerId(string id, string val)
    {
      string val1 = this.Val("USDA.X220", this.GetBorrowerPairs()[0]);
      if (!string.IsNullOrEmpty(val1))
        this.SetVal("USDA.X122", val1);
      else
        this.SetVal("USDA.X122", "");
    }
  }
}
