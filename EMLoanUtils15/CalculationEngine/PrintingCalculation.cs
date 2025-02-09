// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.PrintingCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class PrintingCalculation : CalculationBase
  {
    private SessionObjects sessionObjects;
    private RoleInfo[] roles;
    private DocumentGroupSetup docGroupSetup;
    private DocumentTrackingSetup docsSetup;
    private string pageDone;
    private bool printLicense;
    private NameValueCollection fieldCollect;
    private string formID = string.Empty;
    private int item801Count;
    private int item802Count;
    private ConditionalLetterLayout letterLayout;
    private string[] discloseMethod = new string[5]
    {
      "U.S. Mail",
      "In Person",
      "Other",
      "Fax",
      "eFolder eDisclosures"
    };
    private int lineCount;
    private int fieldPos;
    private int subItemCnt;
    private string blockID = string.Empty;
    private bool calcOnly;
    private double total801POCAmt;
    private double total801PTCAmt;
    private double total801NonPOCPTCAmt;
    private bool print801Header;
    private double total802POCAmt;
    private double total802PTCAmt;
    private double total802NonPOCPTCAmt;
    private bool print802Header;
    private double total1101POCAmt;
    private double total1101PTCAmt;
    private double total1101NonPOCPTCAmt;
    private bool printFinancedNote;
    private List<string[]> outputGrid = new List<string[]>();

    internal PrintingCalculation(
      SessionObjects sessionObjects,
      LoanData l,
      EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.sessionObjects = sessionObjects;
    }

    internal string PageDone
    {
      get => this.pageDone;
      set => this.pageDone = value;
    }

    internal bool PrintLicense
    {
      set => this.printLicense = value;
    }

    internal NameValueCollection FieldCollect
    {
      get => this.fieldCollect;
      set => this.fieldCollect = value;
    }

    internal void CalcPrinting(string formID)
    {
      if (formID != string.Empty)
        this.checkTodayDate("363");
      this.formID = formID.ToUpper();
      switch (this.formID)
      {
        case "2010 ADDENDUM TO HUD SETTLEMENT STATEMENT":
          this.print2010HUDAddendum();
          break;
        case "2010 GFE PAGE 1":
          this.printHUDGFEPage1((LoanData) null);
          break;
        case "2010 GFE PAGE 2":
          this.printHUDGFEPage2((LoanData) null, false);
          break;
        case "2010 HUD-1 SETTLEMENT STATEMENT PAGE 2":
          this.print2010HUD1Page2();
          break;
        case "2010 HUD-1 SETTLEMENT STATEMENT PAGE 3":
        case "2010 HUD-1A SETTLEMENT STATEMENT PAGE 1":
        case "2010 HUD-1A SETTLEMENT STATEMENT PAGE 3":
        case "HUD-1A SETTLEMENT STATEMENT":
          this.printHUD1A(this.formID);
          break;
        case "2010 ITEMIZATION":
        case "ABILITY-TO-REPAY PAGE 4":
          this.print2010Itemization();
          break;
        case "203K MAX MORTGAGE WS PAGE2":
          this.print203KMAX();
          break;
        case "ABILITY-TO-PAY PAGE 5":
          this.printOthers(this.formID);
          break;
        case "ACKNOWLEDGEMENT OF INTENT TO PROCEED":
        case "ACKNOWLEDGEMENT OF RECEIPT OF GOOD FAITH ESTIMATE":
        case "GOOD FAITH ESTIMATE NEW CONSTRUCTION DISCLOSURE":
          this.printAcknowledgementForms();
          break;
        case "AMORT SCHEDULE, COMPLETE":
          this.printAmortMonthly();
          break;
        case "AMORT SCHEDULE, ENDING SUMMARIES":
          this.printAmortSummary();
          break;
        case "AMORT SCHEDULE, YEARLY TOTALS":
          this.printAmortYearly();
          break;
        case "ANNUAL AND REPAYMENT INCOME WORKSHEET PAGE 1":
        case "ANNUAL AND REPAYMENT INCOME WORKSHEET PAGE 2":
        case "ANNUAL AND REPAYMENT INCOME WORKSHEET PAGE 3":
          this.printAnnualAndRepaymentIncomeWorksheet(formID);
          break;
        case "ANTI-STEERING SAFE HARBOR DISCLOSURE PAGE 2":
          this.printLoanOptionsDisclosure();
          break;
        case "BORROWER SUMMARY":
          this.printBorrowerSummary();
          break;
        case "CA PRIVACY POLICY DISCLOSURE":
          this.printCAPrivacyPolicy();
          break;
        case "CONVERSATION LOG ENTRIES":
          this.printConversationLogEntries();
          break;
        case "DEBT CONSOLIDATION":
          this.printDebtConsolidation();
          break;
        case "DIS - AFFILIATED BUSINESS":
          this.printAffiliateBusinessArrangements();
          break;
        case "DISCLOSURE TRACKING DETAILS":
          this.printDisclosureTrackingDetails();
          break;
        case "DISCLOSURE TRACKING SUMMARY":
          this.printDisclosureTrackingSummary();
          break;
        case "DOCUMENT LIST":
          this.printDocumentTrackingSummary();
          break;
        case "EEM DETAILS":
          this.printEEM();
          break;
        case "FACT ACT DISCLOSURE P1":
          this.printFactActDisclosure(true);
          break;
        case "FACT ACT DISCLOSURE P2":
          this.printFactActDisclosure(false);
          break;
        case "FL BROKER CONTRACT W/ COMMIT PAGE 1":
        case "FL BROKER CONTRACT W/O COMMIT":
          this.printFLBroker();
          break;
        case "FL MORTGAGE LOAN COMMITMENT":
          if (Utils.ParseDate((object) this.loan.GetSimpleField("3094")) == DateTime.MinValue)
            this.fieldCollect.Set("3094", DateTime.Today.ToString("MM/dd/yyyy"));
          this.pageDone = string.Empty;
          break;
        case "FUNDING BALANCING WORKSHEET":
          this.printBalancingWorksheet();
          break;
        case "FUNDING WORKSHEET":
          this.PrintFundingWorksheet(false, false);
          break;
        case "FUNDING WORKSHEET ADDITIONAL":
          this.PrintFundingWorksheet(true, false);
          this.PrintFundingWorksheet(false, true);
          break;
        case "GFE - ITEMIZATION":
          this.printREGGFE();
          break;
        case "GFE BROKER":
          this.printGFEBroker();
          break;
        case "GFE BROKER EXPANDED":
        case "GFE LENDER EXPANDED":
          this.printGFEExpanded();
          break;
        case "GFE CA - RE885 P1":
        case "GFE CA-RE88395 P1":
        case "HUD-1 SETTLEMENT STATEMENT PAGE 2":
          this.printHUD1Page2();
          break;
        case "GFE CA - RE885 P2":
          this.printMLDS885Page2();
          break;
        case "GFE CA - RE885 P3":
          this.printMLDSPage3();
          break;
        case "GFE CA-RE882 P2":
          this.printRE882Page2();
          break;
        case "GFE CA-RE88395 P2":
          this.printMLDSPage2();
          break;
        case "GFE CA-RE88395 P3":
          this.printMLDSPage3();
          break;
        case "GFE LENDER":
          this.printGFELender();
          break;
        case "GFE PROVIDER":
        case "GFE PROVIDER (LEGAL)":
          this.printSettlementServiceProviderlist(true, this.formID);
          break;
        case "GFE PROVIDER CONTINUED PAGE":
        case "GFE PROVIDER CONTINUED PAGE (LEGAL)":
          this.printSettlementServiceProviderlist(false, this.formID);
          break;
        case "HOMEOWNERSHIP COUNSELING ORGANIZATION LIST PAGE 1":
          this.printHomeownershipCounseling(1);
          break;
        case "HOMEOWNERSHIP COUNSELING ORGANIZATION LIST PAGE 2":
          this.printHomeownershipCounseling(2);
          break;
        case "IRS1098 - TAX INFO PAGE1":
          this.print1098();
          break;
        case "IRS1098 - TAX INFO PAGE3":
          this.print1098();
          break;
        case "IRS4506 - COPY REQUEST PAGE 1 (CLASSIC)":
          this.printIRS4506Classic(true);
          break;
        case "IRS4506T - TRANS REQUEST PAGE 1 (CLASSIC)":
          break;
        case "ITEMIZED FEE WORKSHEET":
          this.printItemizedFeeWorksheet(1);
          break;
        case "ITEMIZED FEE WORKSHEET PAGE 2":
          this.printItemizedFeeWorksheet(2);
          break;
        case "LOAN LOG ENTRIES":
          this.printLoanLogEntries();
          break;
        case "LOANS WHERE CREDIT SCORE IS NOT AVAILABLE":
          this.printCreditScoreH5();
          break;
        case "MTG LOAN COMM PAGE1":
          this.printLoanCommitment1();
          break;
        case "MTG LOAN COMM PAGE2":
          this.printLoanCommitment2();
          break;
        case "MTG LOAN COMM PAGE3":
          this.printLoanCommitment3();
          break;
        case "NET TANGIBLE BENEFIT DISCLOSURE PAGE 2":
        case "NET TANGIBLE BENEFIT WORKSHEET PAGE 2":
          this.printNetTangibleBenefit(this.formID);
          break;
        case "PAYMENT HISTORY":
          this.printPaymentHistory();
          break;
        case "POST-CLOSING CONDITION LIST":
          this.printConditionTrackingSummary(true);
          break;
        case "PRELIMINARY CONDITION LIST":
          this.printPreliminaryConditionSummary();
          break;
        case "PRIVACY POLICY - AFFILIATE NO OPT-OUT PAGE 1":
        case "PRIVACY POLICY - AFFILIATE OPT-OUT PAGE 1":
        case "PRIVACY POLICY - NO AFFILIATE NO OPT-OUT PAGE 1":
        case "PRIVACY POLICY - NO AFFILIATE OPT-OUT PAGE 1":
          this.printPrivacyPolicyPage1();
          break;
        case "PRIVACY POLICY - AFFILIATE NO OPT-OUT PAGE 2":
        case "PRIVACY POLICY - AFFILIATE OPT-OUT PAGE 2":
        case "PRIVACY POLICY - NO AFFILIATE NO OPT-OUT PAGE 2":
        case "PRIVACY POLICY - NO AFFILIATE OPT-OUT PAGE 2":
          this.printPrivacyPolicyPage2();
          break;
        case "RD 3555-21 REQUEST FOR SINGLE FAMILY HOUSING LOAN GUARANTEE PAGE 3":
        case "RD 3555-21 REQUEST FOR SINGLE FAMILY HOUSING LOAN GUARANTEE PAGE 4":
        case "RD 3555-21 REQUEST FOR SINGLE FAMILY HOUSING LOAN GUARANTEE PAGE 5":
          this.printRD3555(formID);
          break;
        case "REGZ - TIL":
          this.printREGZTIL();
          break;
        case "RISK-BASED PRICING NOTICE":
          this.printCreditScoreH1();
          break;
        case "SCHEDULE OF REAL ESTATE OWNED":
          this.printSOR(3);
          break;
        case "SCHEDULE OF REAL ESTATE OWNED (LETTER)":
          this.printSOR(2);
          break;
        case "SECTION 32 TIL DISCLOSURE PAGE 1":
          this.printSection32();
          break;
        case "STATEMENT OF DENIAL":
        case "STATEMENT OF DENIAL_COBORROWER_P2":
        case "STATEMENT OF DENIAL_COBORROWER_P3":
        case "STATEMENT OF DENIAL_P2":
        case "STATEMENT OF DENIAL_P3":
          this.printStatementOfDenial();
          break;
        case "TASKS LIST":
          this.printTasksSummary();
          break;
        case "TAX AND INSURANCE INFORMATION SHEET":
          this.printTaxAndInsurance();
          break;
        case "UNDERWRITER SUMMARY PAGE 2":
          this.printUnderwriterSummary(2);
          break;
        case "UNDERWRITER SUMMARY PAGE 3":
          this.printUnderwriterSummary(3);
          break;
        case "UNDERWRITING CONDITION LIST":
          this.printConditionTrackingSummary(false);
          break;
        case "UNDERWRITING CONDITIONS":
          this.printUnderwritingConditions();
          break;
        case "VA 26-1820 LOAN DISBURSE PAGE 1":
          this.printVA261820(1);
          break;
        case "VA 26-1820 LOAN DISBURSE PAGE 2":
          this.printVA261820(2);
          break;
        case "VA 26-1880 ELIG CERT PAGE 1":
          this.printVAELIG();
          break;
        case "VA 26-6393 LOAN ANALYSIS ADDENDUM":
          this.printVALAAddendum();
          break;
        default:
          this.pageDone = string.Empty;
          break;
      }
    }

    internal void CalcPrinting(string formID, int blockNo)
    {
      switch (formID)
      {
        case "Credit Score Disclosure Exception Model H3 Page 1":
          this.printCreditScoreH3(1, blockNo);
          break;
        case "Credit Score Disclosure Exception Model H3 Page 2":
          this.printCreditScoreH3(2, blockNo);
          break;
        case "Credit Score Disclosure Exception Model H3 Page 3":
          this.printCreditScoreH3(3, blockNo);
          break;
        case "Risk Based Pricing Notice Model H6 Page 1":
          this.printCreditScoreH6(1, blockNo);
          break;
        case "Risk Based Pricing Notice Model H6 Page 2":
          this.printCreditScoreH6(2, blockNo);
          break;
        case "VOD":
        case "VOL":
          this.printVerifs(formID, blockNo);
          break;
        case "VOM":
          this.printVOM(blockNo);
          break;
        default:
          this.pageDone = string.Empty;
          break;
      }
    }

    internal void CalcPrinting(string formID, int blockNo, bool borrower)
    {
      switch (formID)
      {
        case "IRS4506 TRANST REQ P1":
          this.printIRS4506(blockNo, true);
          break;
        case "IRS4506T TRANST REQ P1":
          this.printIRS4506(blockNo, false);
          break;
        case "VOD":
        case "VOL":
          this.printVerifs(formID, blockNo);
          break;
        case "VOE":
        case "VOR":
          this.printVORandVOE(formID, blockNo, borrower);
          break;
        case "VOM":
          this.printVOM(blockNo);
          break;
        default:
          this.pageDone = string.Empty;
          break;
      }
    }

    internal string OutputFormSizeCheck(string formID)
    {
      return formID == "GFEP1_2010" || formID == "GFEP2_2010" || formID == "GFEP3_2010" ? this.get2010GFEFormSizeNeeded(formID) : formID;
    }

    private void printIRS4506Classic(bool for4506)
    {
      this.pageDone = string.Empty;
      string str = this.loan.GetField("IRS4506.X27").Replace("-", "").Replace(" ", "");
      if (str.Length >= 3)
        this.fieldCollect.Set("IRS4506X27_1", "(" + str.Substring(0, 3) + ")");
      else
        this.fieldCollect.Set("IRS4506X27_1", str);
      if (str.Length > 3)
        this.fieldCollect.Set("IRS4506X27_2", (str.Length >= 6 ? str.Substring(3, 3) : str.Substring(3)) + (str.Length > 6 ? "-" : "") + (str.Length >= 10 ? str.Substring(6, 4) : (str.Length > 6 ? str.Substring(6) : "")) + (str.Length > 10 ? " " + str.Substring(10) : ""));
      else
        this.fieldCollect.Set("IRS4506X27_2", "");
      if (for4506)
        this.fieldCollect.Set("IRS4506X64", this.loan.GetField("IRS4506.X61") == "Y" ? "X" : "");
      else
        this.fieldCollect.Set("IRS4506X64", this.loan.GetField("IRS4506.X62") == "Y" ? "X" : "");
    }

    private void printIRS4506(int blockNo, bool for4506)
    {
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(for4506);
      if (blockNo > numberOfTaX4506Ts)
        return;
      this.pageDone = string.Empty;
      string empty = string.Empty;
      string str1 = (for4506 ? "AR" : "IR") + blockNo.ToString("00");
      this.fieldCollect.Set("IRS4506X2_X3", this.loan.GetField(str1 + "02") + " " + this.loan.GetField(str1 + "03"));
      this.fieldCollect.Set("IRS4506X4", this.loan.GetField(str1 + "04"));
      this.fieldCollect.Set("IRS4506X6_X7", this.loan.GetField(str1 + "06") + " " + this.loan.GetField(str1 + "07"));
      this.fieldCollect.Set("IRS4506X5", this.loan.GetField(str1 + "05"));
      this.fieldCollect.Set("IRS4506X39_X40", this.loan.GetField(str1 + "39") + " " + this.loan.GetField(str1 + "40"));
      string field1 = this.loan.GetField(str1 + "35");
      this.fieldCollect.Set("IRS4506X35_X36_X37_X38", field1 + (field1 != "" ? ", " : "") + this.loan.GetField(str1 + "36") + (this.loan.GetField(str1 + "36") != string.Empty ? ", " : "") + this.loan.GetField(str1 + "37") + " " + this.loan.GetField(str1 + "38"));
      string field2 = this.loan.GetField(str1 + "41");
      this.fieldCollect.Set("IRS4506X41_X42_X43_X44", field2 + (field2 != "" ? ", " : "") + this.loan.GetField(str1 + "42") + (this.loan.GetField(str1 + "42") != "" ? ", " : "") + this.loan.GetField(str1 + "43") + " " + this.loan.GetField(str1 + "44"));
      this.fieldCollect.Set("IRS4506X8_X9", this.loan.GetField(str1 + "08") + " " + this.loan.GetField(str1 + "09"));
      this.fieldCollect.Set("IRS4506X64", this.loan.GetField(str1 + "64") == "Y" ? "X" : "");
      if (for4506)
      {
        this.fieldCollect.Set("IRS4506X10", this.loan.GetField(str1 + "10"));
        this.fieldCollect.Set("IRS4506X11_X12_X13", this.loan.GetField(str1 + "11") + (this.loan.GetField(str1 + "11") != string.Empty ? ", " : "") + this.loan.GetField(str1 + "12") + " " + this.loan.GetField(str1 + "13"));
      }
      else
        this.fieldCollect.Set("IRS4506X10_X11_X12_X13", this.loan.GetField(str1 + "10") + (this.loan.GetField(str1 + "10") != string.Empty ? ", " : "") + this.loan.GetField(str1 + "11") + (this.loan.GetField(str1 + "11") != string.Empty ? ", " : "") + this.loan.GetField(str1 + "12") + " " + this.loan.GetField(str1 + "13"));
      this.fieldCollect.Set("IRS4506X45", this.loan.GetField(str1 + "45"));
      if (for4506)
      {
        this.fieldCollect.Set("IRS4506X14", this.loan.GetField(str1 + "14") == "Y" ? "X" : "");
        this.fieldCollect.Set("IRS4506X18", this.loan.GetField(str1 + "18") == "Y" ? "X" : "");
      }
      this.fieldCollect.Set("IRS4506X24", this.loan.GetField(str1 + "24"));
      this.fieldCollect.Set("IRS4506X25", this.loan.GetField(str1 + "25") != "//" ? this.loan.GetField(str1 + "25") : "");
      this.fieldCollect.Set("IRS4506X26", this.loan.GetField(str1 + "26") != "//" ? this.loan.GetField(str1 + "26") : "");
      this.fieldCollect.Set("IRS4506X29", this.loan.GetField(str1 + "29") != "//" ? this.loan.GetField(str1 + "29") : "");
      this.fieldCollect.Set("IRS4506X30", this.loan.GetField(str1 + "30") != "//" ? this.loan.GetField(str1 + "30") : "");
      if (for4506)
      {
        this.fieldCollect.Set("IRS4506X53", this.loan.GetField(str1 + "53") != "//" ? this.loan.GetField(str1 + "53") : "");
        this.fieldCollect.Set("IRS4506X54", this.loan.GetField(str1 + "54") != "//" ? this.loan.GetField(str1 + "54") : "");
        this.fieldCollect.Set("IRS4506X55", this.loan.GetField(str1 + "55") != "//" ? this.loan.GetField(str1 + "55") : "");
        this.fieldCollect.Set("IRS4506X56", this.loan.GetField(str1 + "56") != "//" ? this.loan.GetField(str1 + "56") : "");
        this.fieldCollect.Set("IRS4506X52", this.loan.GetField(str1 + "52"));
        this.fieldCollect.Set("IRS4506X31", this.loan.GetField(str1 + "31"));
        this.fieldCollect.Set("IRS4506X32", this.loan.GetField(str1 + "32"));
      }
      else
      {
        this.fieldCollect.Set("IRS4506X46", this.loan.GetField(str1 + "46") == "Y" ? "X" : "");
        this.fieldCollect.Set("IRS4506X47", this.loan.GetField(str1 + "47") == "Y" ? "X" : "");
        this.fieldCollect.Set("IRS4506X48", this.loan.GetField(str1 + "48") == "Y" ? "X" : "");
        this.fieldCollect.Set("IRS4506X49", this.loan.GetField(str1 + "49") == "Y" ? "X" : "");
        this.fieldCollect.Set("IRS4506X50", this.loan.GetField(str1 + "50") == "Y" ? "X" : "");
        this.fieldCollect.Set("IRS4506X60", this.loan.GetField(str1 + "60") == "Y" ? "X" : "");
      }
      this.fieldCollect.Set("IRS4506X28", this.loan.GetField(str1 + "28"));
      string str2 = this.loan.GetField(str1 + "27").Replace("-", "").Replace(" ", "");
      if (str2.Length >= 3)
        this.fieldCollect.Set("IRS4506X27_1", "(" + str2.Substring(0, 3) + ")");
      else
        this.fieldCollect.Set("IRS4506X27_1", str2);
      if (str2.Length > 3)
        this.fieldCollect.Set("IRS4506X27_2", (str2.Length >= 6 ? str2.Substring(3, 3) : str2.Substring(3)) + (str2.Length > 6 ? "-" : "") + (str2.Length >= 10 ? str2.Substring(6, 4) : (str2.Length > 6 ? str2.Substring(6) : "")) + (str2.Length > 10 ? " " + str2.Substring(10) : ""));
      else
        this.fieldCollect.Set("IRS4506X27_2", "");
    }

    internal void printHUD1A(string formID)
    {
      this.pageDone = string.Empty;
      if (this.loan.GetField("SYS.X8") == "Y")
      {
        double num = Utils.ParseDouble((object) this.loan.GetField("333"));
        if (num != 0.0)
          this.fieldCollect.Set("333", num.ToString("N2"));
      }
      if (formID == "2010 HUD-1A SETTLEMENT STATEMENT PAGE 3" || formID == "2010 HUD-1 SETTLEMENT STATEMENT PAGE 3")
      {
        DateTime dateTime = Utils.ParseDate((object) this.loan.GetField("682"));
        if (dateTime == DateTime.MinValue)
          dateTime = DateTime.Today;
        if (this.loan.GetField("NEWHUD.X5") == "Y")
          this.fieldCollect.Set("NEWHUD_X556", dateTime.AddMonths(Utils.ParseInt((object) this.loan.GetField("NEWHUD.X556")) - 1).ToString("MM/dd/yyyy"));
        if (this.loan.GetField("NEWHUD.X8") == "Y")
          this.fieldCollect.Set("NEWHUD_X9", dateTime.AddMonths(Utils.ParseInt((object) this.loan.GetField("NEWHUD.X9"))).ToString("MM/dd/yyyy"));
      }
      if (formID == "2010 HUD-1A SETTLEMENT STATEMENT PAGE 3" || formID == "2010 HUD-1 SETTLEMENT STATEMENT PAGE 3")
        return;
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int num1 = 1500;
      int num2 = -1;
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        if (!(this.loan.GetField(str + "18") != "Y"))
        {
          ++num1;
          if (num1 > 1518)
          {
            num2 = index;
            break;
          }
          string field1 = this.loan.GetField(str + "02");
          string field2 = this.loan.GetField(str + "16");
          this.fieldCollect.Set(num1.ToString() + "_D", field1);
          this.fieldCollect.Set(num1.ToString() + "_A", field2);
        }
      }
      if (num2 > -1)
      {
        double num3 = 0.0;
        for (int index = num2; index <= exlcudingAlimonyJobExp; ++index)
        {
          string str = "FL" + index.ToString("00");
          if (!(this.loan.GetField(str + "18") != "Y"))
            num3 += this.ToDouble(this.loan.GetField(str + "16"));
        }
        this.fieldCollect.Set("1519_D", "Other Liabilities");
        this.fieldCollect.Set("1519_A", num3.ToString("N2"));
      }
      this.fieldCollect.Set("1520", this.loan.GetField("HUD1A.X31"));
    }

    internal void CalcPrinting(LoanData dummyLoan, int pageNo)
    {
      if (pageNo == 1)
      {
        this.printHUDGFEPage1(dummyLoan);
      }
      else
      {
        if (pageNo != 2)
          return;
        this.printHUDGFEPage2(dummyLoan, false);
      }
    }

    internal void CalcPrinting(ConditionLog condition)
    {
      this.checkTodayDate("363");
      this.pageDone = string.Empty;
      switch (condition)
      {
        case PostClosingConditionLog _:
          PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) condition;
          this.fieldCollect.Set("CREATEDBY", closingConditionLog.AddedBy);
          this.fieldCollect.Set("NAME", closingConditionLog.Title);
          this.fieldCollect.Set("BOR_PAIR", "");
          BorrowerPair[] borrowerPairs1 = this.loan.GetBorrowerPairs();
          for (int index = 0; index < borrowerPairs1.Length; ++index)
          {
            if (borrowerPairs1[index].Id == closingConditionLog.PairId)
            {
              this.fieldCollect.Set("BOR_PAIR", borrowerPairs1[index].Borrower.FirstName + " " + borrowerPairs1[index].Borrower.LastName);
              break;
            }
          }
          this.fieldCollect.Set("SOURCE", closingConditionLog.Source);
          this.fieldCollect.Set("RECIPIENT", closingConditionLog.Recipient);
          this.fieldCollect.Set("DAYSRECEIVE", closingConditionLog.DaysTillDue > 0 ? closingConditionLog.DaysTillDue.ToString() : "");
          this.fieldCollect.Set("DATE_EXPECTED", closingConditionLog.DateExpected != DateTime.MinValue ? closingConditionLog.DateExpected.ToString("MM/dd/yyyy") : "");
          this.setPDF_Date("DATE_ADDED", closingConditionLog.DateAdded);
          this.setPDF_Date("DATE_REQUESTED", closingConditionLog.DateRequested);
          this.setPDF_Date("DATE_REREQUESTED", closingConditionLog.DateRerequested);
          this.setPDF_Date("DATE_RECEIVED", closingConditionLog.DateReceived);
          this.setPDF_Date("DATE_SENT", closingConditionLog.DateSent);
          this.setPDF_Date("DATE_CLEARED", closingConditionLog.DateCleared);
          string description1 = closingConditionLog.Description;
          this.fieldCollect.Set("DESCRIPTION", Utils.StringWrapping(ref description1, 120, 20, 1));
          description1 = closingConditionLog.Comments.ToString();
          this.fieldCollect.Set("COMMENTS", Utils.StringWrapping(ref description1, 120, 20, 1));
          break;
        case PreliminaryConditionLog _:
          PreliminaryConditionLog preliminaryConditionLog = (PreliminaryConditionLog) condition;
          this.fieldCollect.Set("CREATEDBY", preliminaryConditionLog.AddedBy);
          this.fieldCollect.Set("NAME", preliminaryConditionLog.Title);
          this.fieldCollect.Set("FOR", "");
          BorrowerPair[] borrowerPairs2 = this.loan.GetBorrowerPairs();
          for (int index = 0; index < borrowerPairs2.Length; ++index)
          {
            if (borrowerPairs2[index].Id == preliminaryConditionLog.PairId)
            {
              this.fieldCollect.Set("FOR", borrowerPairs2[index].Borrower.FirstName + " " + borrowerPairs2[index].Borrower.LastName);
              break;
            }
          }
          this.fieldCollect.Set("SOURCE", preliminaryConditionLog.Source);
          this.fieldCollect.Set("CATEGORY", preliminaryConditionLog.Category);
          this.fieldCollect.Set("PRIORTO", LoanConstants.PriorToUIConversion(preliminaryConditionLog.PriorTo));
          this.fieldCollect.Set("ADDEDON", preliminaryConditionLog.DateAdded != DateTime.MinValue ? preliminaryConditionLog.DateAdded.ToString("MM/dd/yyyy") : "");
          this.fieldCollect.Set("FULFILLEDON", preliminaryConditionLog.DateFulfilled != DateTime.MinValue ? preliminaryConditionLog.DateFulfilled.ToString("MM/dd/yyyy") : "");
          this.setPDF_Boolean("UWACCESS", preliminaryConditionLog.UnderwriterAccess);
          string description2 = preliminaryConditionLog.Description;
          this.fieldCollect.Set("DESCRIPTION", Utils.StringWrapping(ref description2, 120, 20, 1));
          string val1 = preliminaryConditionLog.Comments.ToString();
          this.fieldCollect.Set("COMMENTS", Utils.StringWrapping(ref val1, 120, 20, 1));
          break;
        case UnderwritingConditionLog _:
          UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) condition;
          this.fieldCollect.Set("CREATEDBY", underwritingConditionLog.AddedBy);
          this.fieldCollect.Set("NAME", underwritingConditionLog.Title);
          RoleInfo roleFunction = this.sessionObjects.BpmManager.GetRoleFunction(underwritingConditionLog.ForRoleID);
          if (roleFunction != null)
            this.fieldCollect.Set("FOR", roleFunction.RoleName);
          else
            this.fieldCollect.Set("FOR", "");
          this.fieldCollect.Set("SOURCE", underwritingConditionLog.Source);
          this.fieldCollect.Set("CATEGORY", underwritingConditionLog.Category);
          this.fieldCollect.Set("PRIORTO", LoanConstants.PriorToUIConversion(underwritingConditionLog.PriorTo));
          this.fieldCollect.Set("OWNER", "");
          BorrowerPair[] borrowerPairs3 = this.loan.GetBorrowerPairs();
          for (int index = 0; index < borrowerPairs3.Length; ++index)
          {
            if (borrowerPairs3[index].Id == underwritingConditionLog.PairId)
            {
              this.fieldCollect.Set("OWNER", borrowerPairs3[index].Borrower.FirstName + " " + borrowerPairs3[index].Borrower.LastName);
              break;
            }
          }
          this.setPDF_Date("DATE_ADDED", underwritingConditionLog.DateAdded);
          this.setPDF_Date("DATE_FULFILLED", underwritingConditionLog.DateFulfilled);
          this.setPDF_Date("DATE_RECEIVED", underwritingConditionLog.DateReceived);
          this.setPDF_Date("DATE_REVIEWED", underwritingConditionLog.DateReviewed);
          this.setPDF_Date("DATE_REJECTED", underwritingConditionLog.DateRejected);
          this.setPDF_Date("DATE_CLEARED", underwritingConditionLog.DateCleared);
          this.setPDF_Date("DATE_WAIVED", underwritingConditionLog.DateWaived);
          this.setPDF_Boolean("CLEARED_CHECK", underwritingConditionLog.AllowToClear);
          this.setPDF_Boolean("INTERNAL_CHECK", underwritingConditionLog.IsInternal);
          this.setPDF_Boolean("EXTERNAL_CHECK", underwritingConditionLog.IsExternal);
          string description3 = underwritingConditionLog.Description;
          this.fieldCollect.Set("CONDITION", Utils.StringWrapping(ref description3, 120, 20, 1));
          string val2 = underwritingConditionLog.Comments.ToString();
          this.fieldCollect.Set("COMMENTS", Utils.StringWrapping(ref val2, 120, 20, 1));
          break;
      }
    }

    private void setPDF_Date(string id, DateTime d)
    {
      this.fieldCollect.Set(id, d != DateTime.MinValue ? d.ToString("MM/dd/yyyy") : "");
    }

    private void setPDF_Boolean(string id, bool isChecked)
    {
      this.fieldCollect.Set(id, isChecked ? "X" : "");
    }

    internal void CalcPrinting(DocumentLog doc)
    {
      this.checkTodayDate("363");
      this.pageDone = string.Empty;
      string empty1 = string.Empty;
      if (doc.AllowedRoles.Length == 0)
      {
        this.fieldCollect.Set("ACCESS", "");
      }
      else
      {
        if (this.roles == null)
          this.roles = this.sessionObjects.BpmManager.GetAllRoleFunctions();
        string empty2 = string.Empty;
        foreach (int allowedRole in doc.AllowedRoles)
        {
          if (empty2 != string.Empty)
            empty2 += ",";
          empty2 += this.getRoleName(allowedRole);
        }
        this.fieldCollect.Set("ACCESS", empty2);
      }
      this.fieldCollect.Set("TITLE", doc.Title);
      string str1 = string.Empty;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Id == doc.PairId)
        {
          str1 = (borrowerPairs[index].Borrower.FirstName + " " + borrowerPairs[index].Borrower.LastName).Trim();
          break;
        }
      }
      this.fieldCollect.Set("BOR_PAIR", str1);
      this.fieldCollect.Set("MILESTONE", doc.Stage);
      if (doc.Conditions == null || doc.Conditions.Count == 0)
      {
        this.fieldCollect.Set("FORCONDITION", "");
      }
      else
      {
        int num = 0;
        foreach (ConditionLog condition in doc.Conditions)
        {
          ++num;
          switch (num)
          {
            case 1:
              this.fieldCollect.Set("FORCONDITION", condition.Title);
              continue;
            case 2:
              this.fieldCollect.Set("FORCONDITION2", condition.Title);
              continue;
            default:
              goto label_26;
          }
        }
      }
label_26:
      if (this.docGroupSetup == null)
        this.docGroupSetup = this.sessionObjects.ConfigurationManager.GetDocumentGroupSetup();
      if (this.docsSetup == null)
        this.docsSetup = this.sessionObjects.ConfigurationManager.GetDocumentTrackingSetup();
      DocumentTemplate byName = this.docsSetup.GetByName(doc.Title);
      if (byName == null)
      {
        this.fieldCollect.Set("FORGROUP", "");
      }
      else
      {
        string empty3 = string.Empty;
        foreach (DocumentGroup documentGroup in (CollectionBase) this.docGroupSetup)
        {
          if (documentGroup.Contains(byName))
          {
            if (empty3 != string.Empty)
              empty3 += ",";
            empty3 += (string) (object) documentGroup;
          }
        }
        this.fieldCollect.Set("FORGROUP", empty3);
      }
      this.fieldCollect.Set("FORGROUP", "");
      this.fieldCollect.Set("REQUESTEDFROM", doc.RequestedBy);
      NameValueCollection fieldCollect1 = this.fieldCollect;
      int num1 = doc.DaysDue;
      string str2 = num1.ToString();
      fieldCollect1.Set("DAYS_RECEIVED", str2);
      this.fieldCollect.Set("DATE_EXPECTED", doc.DateExpected != DateTime.MinValue ? doc.DateExpected.ToString("MM/dd/yyyy") : "");
      NameValueCollection fieldCollect2 = this.fieldCollect;
      num1 = doc.DaysTillExpire;
      string str3 = num1.ToString();
      fieldCollect2.Set("DAYS_EXPIRATION", str3);
      this.fieldCollect.Set("DATE_EXPIRATION", doc.DateExpires != DateTime.MinValue ? doc.DateExpires.ToString("MM/dd/yyyy") : "");
      this.fieldCollect.Set("DATE_ORDERED", doc.DateRequested != DateTime.MinValue ? doc.DateRequested.ToString("MM/dd/yyyy") : "");
      this.fieldCollect.Set("DATE_REORDERED", doc.DateRerequested != DateTime.MinValue ? doc.DateRerequested.ToString("MM/dd/yyyy") : "");
      this.fieldCollect.Set("DATE_RECEIVED", doc.DateReceived != DateTime.MinValue ? doc.DateReceived.ToString("MM/dd/yyyy") : "");
      string val = doc.Comments.ToString();
      this.fieldCollect.Set("COMMENTS", Utils.StringWrapping(ref val, 120, 20, 1));
    }

    private string getIndex(int i)
    {
      string index;
      switch (i)
      {
        case 0:
          index = "1st";
          break;
        case 1:
          index = "2nd";
          break;
        case 2:
          index = "3rd";
          break;
        default:
          index = (i + 1).ToString() + "th";
          break;
      }
      return index;
    }

    internal void CalcPrinting(MilestoneTaskLog taskLog)
    {
      if (this.roles == null)
        this.roles = this.sessionObjects.BpmManager.GetAllRoleFunctions();
      this.checkTodayDate("363");
      this.pageDone = string.Empty;
      this.fieldCollect.Set("TASKNAME", taskLog.TaskName);
      this.fieldCollect.Set("TASKMILESTONE", taskLog.Stage);
      this.fieldCollect.Set("TASKPRIORITY", taskLog.TaskPriority);
      this.fieldCollect.Set("DAYSTOCOMPLETE", taskLog.DaysToComplete > 0 ? taskLog.DaysToComplete.ToString() : "");
      NameValueCollection fieldCollect1 = this.fieldCollect;
      DateTime dateTime;
      string str1;
      if (!(taskLog.ExpectedDate != DateTime.MinValue))
      {
        str1 = "";
      }
      else
      {
        dateTime = taskLog.ExpectedDate;
        str1 = dateTime.ToString("MM/dd/yyyy");
      }
      fieldCollect1.Set("DUEDATE", str1);
      string taskDescription = taskLog.TaskDescription;
      this.fieldCollect.Set("TASKDESCRIPTION", Utils.StringWrapping(ref taskDescription, 40, 8, 1));
      if (taskLog.ContactCount > 0)
      {
        for (int i = 0; i < taskLog.ContactCount && i <= 3; ++i)
        {
          MilestoneTaskLog.TaskContact taskContactAt = taskLog.GetTaskContactAt(i);
          if (taskContactAt != null)
          {
            int num = i * 8 + 1;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactName);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactRole);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactPhone);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactEmail);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactAddress);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactCity);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactState);
            ++num;
            this.fieldCollect.Set("CONTACT_" + num.ToString(), taskContactAt.ContactZip);
          }
        }
      }
      NameValueCollection fieldCollect2 = this.fieldCollect;
      dateTime = taskLog.AddDate;
      string str2 = dateTime.ToString("MM/dd/yyyy");
      fieldCollect2.Set("TASKADDEDDATE", str2);
      this.fieldCollect.Set("TASKADDEDBY", taskLog.AddedBy);
      if (taskLog.CompletedDate != DateTime.MinValue)
      {
        NameValueCollection fieldCollect3 = this.fieldCollect;
        dateTime = taskLog.CompletedDate;
        string str3 = dateTime.ToString("MM/dd/yyyy");
        fieldCollect3.Set("TASKCOMPLETEDDATE", str3);
        this.fieldCollect.Set("TASKCOMPLETED", "X");
      }
      else
        this.fieldCollect.Set("TASKADDED", "X");
      this.fieldCollect.Set("TASKCOMPLETEDBY", taskLog.CompletedBy);
      if (taskLog.AlertList.Count > 0)
      {
        for (int index = 0; index < taskLog.AlertList.Count; ++index)
        {
          if (taskLog.AlertList[index] != null)
          {
            int num = index * 3 + 1;
            this.fieldCollect.Set("ALERT_" + num.ToString(), this.getRoleName(taskLog.AlertList[index].RoleId));
            ++num;
            NameValueCollection fieldCollect4 = this.fieldCollect;
            string name1 = "ALERT_" + num.ToString();
            string str4;
            if (!(taskLog.AlertList[index].DueDate != DateTime.MinValue))
            {
              str4 = "";
            }
            else
            {
              dateTime = taskLog.AlertList[index].DueDate;
              str4 = dateTime.ToString("MM/dd/yyyy");
            }
            fieldCollect4.Set(name1, str4);
            ++num;
            NameValueCollection fieldCollect5 = this.fieldCollect;
            string name2 = "ALERT_" + num.ToString();
            string str5;
            if (!(taskLog.AlertList[index].FollowedUpDate != DateTime.MinValue))
            {
              str5 = "";
            }
            else
            {
              dateTime = taskLog.AlertList[index].FollowedUpDate;
              str5 = dateTime.ToString("MM/dd/yyyy");
            }
            fieldCollect5.Set(name2, str5);
          }
        }
      }
      string comments = taskLog.Comments;
      this.fieldCollect.Set("TASKCOMMENT", Utils.StringWrapping(ref comments, 110, 20, 1));
    }

    private void printItemizedFeeWorksheet(int pageNo)
    {
      if (pageNo == 1)
      {
        for (int index = 1; index <= 12; ++index)
        {
          this.fieldCollect.Set("801_" + (object) index + "_4", "");
          this.fieldCollect.Set("801_" + (object) index + "_5", "");
        }
        for (int index = 1; index <= 4; ++index)
        {
          this.fieldCollect.Set("802_" + (object) index + "_4", "");
          this.fieldCollect.Set("802_" + (object) index + "_5", "");
        }
        this.printItemizedDetail("559", "SYS.X251");
        this.printItemizedDetail("L229", "SYS.X261");
        this.printItemizedDetail("1622", "SYS.X269");
        this.printItemizedDetail("569", "SYS.X271");
        this.printItemizedDetail("572", "SYS.X265");
        this.printItemizedDetail("NEWHUD.X226", "NEWHUD.X227");
        this.item801Count = 0;
        this.item802Count = 0;
        if (this.FltVal("155") != 0.0 || this.FltVal("200") != 0.0 || this.Val("154") != string.Empty)
          this.printItemizedDetail("200", "SYS.X289");
        if (this.FltVal("1625") != 0.0 || this.FltVal("1626") != 0.0 || this.Val("1627") != string.Empty)
          this.printItemizedDetail("1626", "SYS.X291");
        if (this.FltVal("1839") != 0.0 || this.FltVal("1840") != 0.0 || this.Val("1838") != string.Empty)
          this.printItemizedDetail("1840", "SYS.X296");
        if (this.FltVal("1842") != 0.0 || this.FltVal("1843") != 0.0 || this.Val("1841") != string.Empty)
          this.printItemizedDetail("1843", "SYS.X301");
        if (this.FltVal("NEWHUD.X733") != 0.0 || this.FltVal("NEWHUD.X779") != 0.0 || this.Val("NEWHUD.X732") != string.Empty)
          this.printItemizedDetail("NEWHUD.X779", "NEWHUD.X748");
        if (this.FltVal("NEWHUD.X1237") != 0.0 || this.FltVal("NEWHUD.X1238") != 0.0 || this.Val("NEWHUD.X1235") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1238", "NEWHUD.X1239");
        if (this.FltVal("NEWHUD.X1245") != 0.0 || this.FltVal("NEWHUD.X1246") != 0.0 || this.Val("NEWHUD.X1243") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1246", "NEWHUD.X1247");
        if (this.FltVal("NEWHUD.X1253") != 0.0 || this.FltVal("NEWHUD.X1254") != 0.0 || this.Val("NEWHUD.X1251") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1254", "NEWHUD.X1255");
        if (this.FltVal("NEWHUD.X1261") != 0.0 || this.FltVal("NEWHUD.X1262") != 0.0 || this.Val("NEWHUD.X1259") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1262", "NEWHUD.X1263");
        if (this.FltVal("NEWHUD.X1269") != 0.0 || this.FltVal("NEWHUD.X1270") != 0.0 || this.Val("NEWHUD.X1267") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1270", "NEWHUD.X1271");
        if (this.FltVal("NEWHUD.X1277") != 0.0 || this.FltVal("NEWHUD.X1278") != 0.0 || this.Val("NEWHUD.X1275") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1278", "NEWHUD.X1279");
        if (this.FltVal("NEWHUD.X1285") != 0.0 || this.FltVal("NEWHUD.X1286") != 0.0 || this.Val("NEWHUD.X1283") != string.Empty)
          this.printItemizedDetail("NEWHUD.X1286", "NEWHUD.X1287");
        if (this.Val("NEWHUD.X1139") == "Y")
        {
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1167");
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1169");
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1171");
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1173");
          this.printItemizedDetail("NEWHUD.X1152", "NEWHUD.X1175");
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1179");
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1183");
          this.printItemizedDetail("NEWHUD.X1156", "NEWHUD.X1187");
        }
        else
          this.printItemizedDetail("NEWHUD.X788", "NEWHUD.X749");
        double num = Utils.ParseDouble((object) this.fieldCollect["454"]) + Utils.ParseDouble((object) this.fieldCollect["L228"]) + Utils.ParseDouble((object) this.fieldCollect["1621"]) + Utils.ParseDouble((object) this.fieldCollect["367"]) + Utils.ParseDouble((object) this.fieldCollect["439"]) + Utils.ParseDouble((object) this.fieldCollect["NEWHUD_X687"]) + Utils.ParseDouble((object) this.fieldCollect["NEWHUD_X225"]);
        for (int index = 1; index <= 12; ++index)
          num += Utils.ParseDouble((object) this.fieldCollect["801_" + (object) index + "_5"]);
        for (int index = 1; index <= 4; ++index)
          num += Utils.ParseDouble((object) this.fieldCollect["802_" + (object) index + "_5"]);
        this.fieldCollect.Set("NEWHUD_X688", num != 0.0 ? num.ToString("N2") : "");
        this.printItemizedDetail("581", "SYS.X255");
        this.printItemizedDetail("580", "SYS.X257");
        this.printItemizedDetail("565", "SYS.X267");
        this.printItemizedDetail("NEWHUD.X781", "NEWHUD.X742");
        this.printItemizedDetail("NEWHUD.X147", "NEWHUD.X157");
        this.printItemizedDetail("NEWHUD.X148", "NEWHUD.X158");
        this.printItemizedDetail("NEWHUD.X149", "NEWHUD.X159");
        this.printItemizedDetail("NEWHUD.X150", "NEWHUD.X160");
        this.printItemizedDetail("NEWHUD.X151", "NEWHUD.X161");
        this.printItemizedDetail("574", "SYS.X275");
        this.printItemizedDetail("575", "SYS.X277");
        this.printItemizedDetail("96", "SYS.X279");
        this.printItemizedDetail("1345", "SYS.X281");
        this.printItemizedDetail("6", "SYS.X283");
        this.printItemizedDetail("NEWHUD.X1294", "NEWHUD.X1295");
        this.printItemizedDetail("NEWHUD.X1302", "NEWHUD.X1303");
        this.printItemizedDetail("NEWHUD.X1310", "NEWHUD.X1311");
        this.printItemizedDetail("NEWHUD.X1318", "NEWHUD.X1319");
        this.printItemizedDetail("NEWHUD.X1326", "NEWHUD.X1327");
        this.printItemizedDetail("NEWHUD.X1334", "NEWHUD.X1335");
        this.printItemizedDetail("NEWHUD.X1342", "NEWHUD.X1343");
        this.printItemizedDetail("NEWHUD.X1350", "NEWHUD.X1351");
        this.printItemizedDetail("NEWHUD.X1358", "NEWHUD.X1359");
        this.printItemizedDetail("NEWHUD.X1366", "NEWHUD.X1367");
        this.printItemizedDetail("NEWHUD.X1374", "NEWHUD.X1375");
        this.printItemizedDetail("NEWHUD.X1382", "NEWHUD.X1383");
        this.printItemizedDetail("NEWHUD.X1390", "NEWHUD.X1391");
        this.printItemizedDetail("NEWHUD.X1398", "NEWHUD.X1399");
        this.printItemizedDetail("NEWHUD.X1406", "NEWHUD.X1407");
        this.printItemizedDetail("NEWHUD.X1414", "NEWHUD.X1415");
        this.printItemizedDetail("678", "SYS.X285");
        this.printItemizedDetail("NEWHUD.X658", "NEWHUD.X162");
        this.printItemizedDetail("NEWHUD.X953", "NEWHUD.X955");
        this.printItemizedDetail("NEWHUD.X962", "NEWHUD.X964");
        this.printItemizedDetail("NEWHUD.X971", "NEWHUD.X973");
        this.printItemizedDetail("NEWHUD.X980", "NEWHUD.X982");
        this.printItemizedDetail("NEWHUD.X989", "NEWHUD.X991");
        this.printItemizedDetail("NEWHUD.X998", "NEWHUD.X1000");
        this.printItemizedDetail("NEWHUD.X782", "NEWHUD.X743");
        this.printItemizedDetail("NEWHUD.X783", "NEWHUD.X744");
        this.printItemizedDetail("NEWHUD.X784", "NEWHUD.X745");
        this.printItemizedDetail("NEWHUD.X218", "NEWHUD.X221");
        this.printItemizedDetail("NEWHUD.X219", "NEWHUD.X222");
        this.printItemizedDetail("1764", "SYS.X347");
        this.printItemizedDetail("1769", "SYS.X349");
        this.printItemizedDetail("1774", "SYS.X351");
        this.printItemizedDetail("1779", "SYS.X353");
        this.printItemizedDetail("NEWHUD.X1605", "NEWHUD.X1606");
        this.printItemizedDetail("NEWHUD.X1613", "NEWHUD.X1614");
        this.printItemizedDetail("587", "SYS.X355");
        this.printItemizedDetail("NEWHUD.X787", "NEWHUD.X261");
        this.printItemizedDetail("593", "SYS.X357");
        this.printItemizedDetail("594", "SYS.X359");
        this.printItemizedDetail("576", "SYS.X361");
        this.printItemizedDetail("1642", "SYS.X363");
        this.printItemizedDetail("1645", "SYS.X365");
        this.printItemizedDetail("NEWHUD.X1621", "NEWHUD.X1622");
        this.printItemizedDetail("NEWHUD.X1628", "NEWHUD.X1629");
        this.printItemizedDetail("NEWHUD.X258", "NEWHUD.X262");
        this.printItemizedDetail("590", "SYS.X374");
        this.printItemizedDetail("591", "SYS.X376");
        this.printItemizedDetail("42", "SYS.X378");
        this.printItemizedDetail("55", "SYS.X380");
        this.printItemizedDetail("1784", "SYS.X382");
        this.printItemizedDetail("1789", "SYS.X384");
        this.printItemizedDetail("1794", "SYS.X386");
        this.printItemizedDetail("NEWHUD.X259", "NEWHUD.X263");
        this.printItemizedDetail("NEWHUD.X260", "NEWHUD.X264");
        this.printItemizedDetail("NEWHUD.X1635", "NEWHUD.X1636");
        this.printItemizedDetail("NEWHUD.X1643", "NEWHUD.X1644");
        this.printItemizedDetail("NEWHUD.X1651", "NEWHUD.X1652");
        this.printItemizedDetail("NEWHUD.X1659", "NEWHUD.X1660");
        this.printItemizedDetail("NEWHUD2.X4613", "NEWHUD2.X4614");
        this.printItemizedDetail("NEWHUD2.X4620", "NEWHUD2.X4621");
        this.printItemizedDetail("NEWHUD2.X4627", "NEWHUD2.X4628");
        this.printItemizedDetail("NEWHUD2.X4634", "NEWHUD2.X4635");
        this.printItemizedDetail("NEWHUD2.X4641", "NEWHUD2.X4642");
      }
      else
      {
        this.printItemizedDetail("561", "SYS.X303");
        this.printItemizedDetail("562", "SYS.X305");
        this.printItemizedDetail("578", "SYS.X307");
        this.printItemizedDetail("NEWHUD.X594", "NEWHUD.X163");
        this.printItemizedDetail("571", "SYS.X311");
        this.printItemizedDetail("579", "SYS.X309");
        this.printItemizedDetail("L261", "SYS.X313");
        this.printItemizedDetail("1668", "SYS.X315");
        this.printItemizedDetail("NEWHUD.X595", "NEWHUD.X164");
        this.printItemizedDetail("NEWHUD.X596", "NEWHUD.X165");
        this.printItemizedDetail("596", "SYS.X317");
        this.printItemizedDetail("563", "SYS.X319");
        this.printItemizedDetail("595", "SYS.X323");
        this.printItemizedDetail("L270", "SYS.X321");
        this.printItemizedDetail("597", "SYS.X325");
        this.printItemizedDetail("1632", "SYS.X327");
        this.printItemizedDetail("598", "SYS.X329");
        this.printItemizedDetail("599", "SYS.X331");
        this.printItemizedDetail("NEWHUD.X1714", "NEWHUD.X1710");
      }
      int num1 = 0;
      if (this.Val("143") != "")
      {
        ++num1;
        this.fieldCollect.Set("CC_SUMMARY_" + num1.ToString(), "Total Seller Paid CC");
        this.fieldCollect.Set("CCS_" + num1.ToString(), this.Val("143"));
      }
      if (this.loan.GetSimpleField("BKRPCC") != "")
      {
        ++num1;
        this.fieldCollect.Set("CC_SUMMARY_" + num1.ToString(), "Total Broker Paid CC");
        this.fieldCollect.Set("CCS_" + num1.ToString(), this.Val("BKRPCC"));
      }
      if (this.loan.GetSimpleField("LENPCC") != "")
      {
        ++num1;
        this.fieldCollect.Set("CC_SUMMARY_" + num1.ToString(), "Total Lender Paid CC");
        this.fieldCollect.Set("CCS_" + num1.ToString(), this.Val("LENPCC"));
      }
      if (this.loan.GetSimpleField("OTHPCC") != "")
      {
        ++num1;
        this.fieldCollect.Set("CC_SUMMARY_" + num1.ToString(), "Total Other Paid CC");
        this.fieldCollect.Set("CCS_" + num1.ToString(), this.Val("OTHPCC"));
      }
      if (!(this.loan.GetSimpleField("SYS.X8") == "Y"))
        return;
      this.fieldCollect.Set("333", Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetSimpleField("333")), 2).ToString("N2"));
    }

    private void printItemizedDetail(string selID, string paidByID)
    {
      if (!HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID) && paidByID != "NEWHUD.X1167" && paidByID != "NEWHUD.X1169" && paidByID != "NEWHUD.X1171" && paidByID != "NEWHUD.X1173")
        return;
      string[] strArray1 = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      string id1 = strArray1 != null ? strArray1[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] : "";
      string id2 = strArray1 != null ? strArray1[HUDGFE2010Fields.PTCPOCINDEX_APR] : "";
      string id3 = strArray1 != null ? strArray1[HUDGFE2010Fields.PTCPOCINDEX_POC] : "";
      switch (paidByID)
      {
        case "NEWHUD.X1167":
          id1 = "NEWHUD.X1142";
          break;
        case "NEWHUD.X1169":
          id1 = "NEWHUD.X1144";
          break;
        case "NEWHUD.X1171":
          id1 = "NEWHUD.X1146";
          break;
        case "NEWHUD.X1173":
          id1 = "NEWHUD.X1148";
          break;
        case "NEWHUD.X1175":
          id1 = "NEWHUD.X1151";
          break;
        case "NEWHUD.X1179":
          id1 = "NEWHUD.X1155";
          break;
        case "NEWHUD.X1183":
          id1 = "NEWHUD.X1159";
          break;
        case "NEWHUD.X1187":
          id1 = "NEWHUD.X1163";
          break;
      }
      double num1 = this.FltVal(id1);
      double num2 = this.FltVal(selID);
      double num3 = selID == string.Empty ? 0.0 : this.FltVal(selID);
      string name1 = id1.Replace(".", "_");
      string name2 = id2 != string.Empty ? id2.Replace(".", "_") : "";
      string name3 = paidByID.Replace(".", "_");
      string name4 = id3 != string.Empty ? id3.Replace(".", "_") : "";
      if (id1 == "155" || id1 == "1625" || id1 == "1839" || id1 == "1842" || id1 == "NEWHUD.X733" || id1 == "1663" || id1 == "NEWHUD.X15" && this.Val("NEWHUD.X713") == "Origination Charge" || id1 == "NEWHUD.X1237" || id1 == "NEWHUD.X1245" || id1 == "NEWHUD.X1253" || id1 == "NEWHUD.X1261" || id1 == "NEWHUD.X1269" || id1 == "NEWHUD.X1277" || id1 == "NEWHUD.X1285")
      {
        ++this.item801Count;
        name2 = "801_" + (object) this.item801Count + "_1";
        name4 = "801_" + (object) this.item801Count + "_2";
        name3 = "801_" + (object) this.item801Count + "_3";
        name1 = "801_" + (object) this.item801Count + "_5";
        switch (id1)
        {
          case "155":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("154") + (this.Val("NEWHUD.X1045") != string.Empty ? " to " + this.Val("NEWHUD.X1045") : ""));
            break;
          case "1625":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("1627") + (this.Val("NEWHUD.X1046") != string.Empty ? " to " + this.Val("NEWHUD.X1046") : ""));
            break;
          case "1839":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("1838") + (this.Val("NEWHUD.X1047") != string.Empty ? " to " + this.Val("NEWHUD.X1047") : ""));
            break;
          case "1842":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("1841") + (this.Val("NEWHUD.X1048") != string.Empty ? " to " + this.Val("NEWHUD.X1048") : ""));
            break;
          case "NEWHUD.X1237":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1235") + (this.Val("NEWHUD.X1236") != string.Empty ? " to " + this.Val("NEWHUD.X1236") : ""));
            break;
          case "NEWHUD.X1245":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1243") + (this.Val("NEWHUD.X1244") != string.Empty ? " to " + this.Val("NEWHUD.X1244") : ""));
            break;
          case "NEWHUD.X1253":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1251") + (this.Val("NEWHUD.X1252") != string.Empty ? " to " + this.Val("NEWHUD.X1252") : ""));
            break;
          case "NEWHUD.X1261":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1259") + (this.Val("NEWHUD.X1260") != string.Empty ? " to " + this.Val("NEWHUD.X1260") : ""));
            break;
          case "NEWHUD.X1269":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1267") + (this.Val("NEWHUD.X1268") != string.Empty ? " to " + this.Val("NEWHUD.X1268") : ""));
            break;
          case "NEWHUD.X1277":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1275") + (this.Val("NEWHUD.X1276") != string.Empty ? " to " + this.Val("NEWHUD.X1276") : ""));
            break;
          case "NEWHUD.X1285":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X1283") + (this.Val("NEWHUD.X1284") != string.Empty ? " to " + this.Val("NEWHUD.X1284") : ""));
            break;
          case "NEWHUD.X15":
            if (this.Val("NEWHUD.X715") == "Include Origination Credit")
            {
              this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", "Origination Credit " + this.FltVal("1847").ToString("N3") + "%" + (this.Val("NEWHUD.X734") != string.Empty ? "+$" + this.FltVal("NEWHUD.X734").ToString("N2") : ""));
              break;
            }
            if (this.Val("NEWHUD.X715") == "Include Origination Points")
            {
              if (this.FltVal("3119") != 0.0)
              {
                this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", "Mortgage Buydown");
                break;
              }
              this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", "Origination Points " + this.FltVal("1061").ToString("N3") + "%" + (this.Val("436") != string.Empty ? "+$" + this.FltVal("436").ToString("N2") : ""));
              break;
            }
            break;
          case "NEWHUD.X733":
            this.fieldCollect.Set("801_" + (object) this.item801Count + "_4", this.Val("NEWHUD.X732") + (this.Val("NEWHUD.X1049") != string.Empty ? " to " + this.Val("NEWHUD.X1049") : ""));
            break;
        }
      }
      else if (id1 == "NEWHUD.X15")
      {
        name2 = "802_1_1";
        name4 = "802_1_2";
        name3 = "802_1_3";
        name1 = "802_1_5";
        if (this.Val("NEWHUD.X715") == "Include Origination Credit")
          this.fieldCollect.Set("802_1_4", "Origination Credit" + this.FltVal("1847").ToString("N3") + "%" + (this.Val("NEWHUD.X734") != string.Empty ? "+$" + this.FltVal("NEWHUD.X734").ToString("N2") : ""));
        else if (this.Val("NEWHUD.X715") == "Include Origination Points")
        {
          if (this.FltVal("3119") != 0.0)
            this.fieldCollect.Set("802_1_4", "Mortgage Buydown");
          else
            this.fieldCollect.Set("802_1_4", "Origination Points " + this.FltVal("1061").ToString("N3") + "%" + (this.Val("436") != string.Empty ? "+$" + this.FltVal("436").ToString("N2") : ""));
        }
      }
      else if (id1 == "NEWHUD.X1142" || id1 == "NEWHUD.X1144" || id1 == "NEWHUD.X1146" || id1 == "NEWHUD.X1148" || id1 == "NEWHUD.X1151" || id1 == "NEWHUD.X1155" || id1 == "NEWHUD.X1159" || id1 == "NEWHUD.X1163")
      {
        if (num1 == 0.0 && num3 == 0.0)
          return;
        string empty = string.Empty;
        string name5;
        if (this.Val("NEWHUD.X713") == "Origination Charge")
        {
          ++this.item801Count;
          name3 = "801_" + (object) this.item801Count + "_3";
          name5 = "801_" + (object) this.item801Count + "_4";
          name1 = "801_" + (object) this.item801Count + "_5";
        }
        else
        {
          ++this.item802Count;
          name2 = "802_" + (object) this.item802Count + "_1";
          name3 = "802_" + (object) this.item802Count + "_3";
          name5 = "802_" + (object) this.item802Count + "_4";
          name1 = "802_" + (object) this.item802Count + "_5";
        }
        switch (id1)
        {
          case "NEWHUD.X1142":
            NameValueCollection fieldCollect = this.fieldCollect;
            string name6 = name5;
            string[] strArray2 = new string[6]
            {
              "Lender ",
              this.Val("NEWHUD.X1225") != string.Empty ? "Comp." : "Compensation",
              " Credit ",
              null,
              null,
              null
            };
            double num4 = this.FltVal("NEWHUD.X1141");
            strArray2[3] = num4.ToString("N3");
            strArray2[4] = "%";
            string str1;
            if (!(this.Val("NEWHUD.X1225") != string.Empty))
            {
              str1 = "";
            }
            else
            {
              num4 = this.FltVal("NEWHUD.X1225");
              str1 = "+$" + num4.ToString("N2");
            }
            strArray2[5] = str1;
            string str2 = string.Concat(strArray2);
            fieldCollect.Set(name6, str2);
            break;
          case "NEWHUD.X1144":
            this.fieldCollect.Set(name5, "Origination Credit " + this.FltVal("NEWHUD.X1143").ToString("N3") + "%" + (this.Val("NEWHUD.X1226") != string.Empty ? "+$" + this.FltVal("NEWHUD.X1226").ToString("N2") : ""));
            break;
          case "NEWHUD.X1146":
            this.fieldCollect.Set(name5, this.Val("NEWHUD.X1145"));
            break;
          case "NEWHUD.X1148":
            this.fieldCollect.Set(name5, this.Val("NEWHUD.X1147"));
            break;
          case "NEWHUD.X1151":
            this.fieldCollect.Set(name5, "Origination Points " + this.FltVal("NEWHUD.X1150").ToString("N3") + "%" + (this.Val("NEWHUD.X1227") != string.Empty ? "+$" + this.FltVal("NEWHUD.X1227").ToString("N2") : ""));
            break;
          case "NEWHUD.X1155":
            this.fieldCollect.Set(name5, this.Val("NEWHUD.X1153"));
            break;
          case "NEWHUD.X1159":
            this.fieldCollect.Set(name5, this.Val("NEWHUD.X1157"));
            break;
          case "NEWHUD.X1163":
            this.fieldCollect.Set(name5, this.Val("NEWHUD.X1161"));
            break;
        }
      }
      if (id1 == "NEWHUD.X15" && this.Val("NEWHUD.X715") == "Include Origination Credit")
      {
        num1 = this.FltVal("1663") * -1.0;
        num3 = 0.0;
      }
      else if (id1 == "NEWHUD.X1142" || id1 == "NEWHUD.X1144" || id1 == "NEWHUD.X1146" || id1 == "NEWHUD.X1148")
      {
        num1 = this.FltVal(id1) * -1.0;
        num3 = 0.0;
      }
      else if (id1 == "NEWHUD.X1155" || id1 == "NEWHUD.X1159" || id1 == "NEWHUD.X1163")
        num3 = 0.0;
      this.fieldCollect.Set(name2, !(id2 != string.Empty) || !(this.Val(id2) == "Y") || num1 == 0.0 || !(this.Val(paidByID) == "") ? "" : "A");
      if (id3 != string.Empty && this.Val(id3) == "Y")
      {
        double num5 = this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
        this.fieldCollect.Set(name1 + "_$", num5 != 0.0 ? "$" : "");
        this.fieldCollect.Set(name1 + "_POC", num5 != 0.0 ? num5.ToString("N2") : "");
        double num6 = num1 - num5 + num3;
        this.fieldCollect.Set(name1, num6 != 0.0 ? num6.ToString("N2") : "");
        this.fieldCollect.Set(name4, "P");
        if (this.Val(id2) == "Y" && (this.Val(paidByID) == "" && this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) != "" && num1 > num5 || this.Val(paidByID) != "" && this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) == "" && num5 > 0.0 || this.Val(paidByID) == "" && this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) == ""))
          this.fieldCollect.Set(name2, "A");
      }
      else
      {
        double num7 = num1 + num3;
        if (num7 != 0.0)
          this.fieldCollect.Set(name1, num7.ToString("N2"));
        else
          this.fieldCollect.Set(name1, "");
        num2 = num1;
      }
      if (num1 != 0.0 && num3 != 0.0)
      {
        this.fieldCollect.Set(name3, "S/");
      }
      else
      {
        switch (this.Val(paidByID) ?? "")
        {
          case "":
            if (num3 != 0.0)
            {
              this.fieldCollect.Set(name3, "S");
              break;
            }
            this.fieldCollect.Set(name3, " ");
            break;
          case "Seller":
            this.fieldCollect.Set(name3, num2 != 0.0 || num3 != 0.0 ? "S" : "");
            break;
          case "Broker":
            this.fieldCollect.Set(name3, num2 != 0.0 ? "B" : "");
            break;
          case "Lender":
            this.fieldCollect.Set(name3, num2 != 0.0 ? "L" : "");
            break;
          case "Other":
            this.fieldCollect.Set(name3, num2 != 0.0 ? "O" : "");
            break;
        }
      }
    }

    private void printGFEExpanded()
    {
      this.pageDone = string.Empty;
      int length = CalculationBase.BorrowerFields.Length;
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index = 0; index < length; ++index)
      {
        if (this.loan.GetSimpleField("SYS.X" + CalculationBase.PFCChecks[index]) == "Y")
          this.fieldCollect.Set("SYS_X" + CalculationBase.PFCChecks[index], "A");
        string simpleField = this.loan.GetSimpleField("SYS.X" + CalculationBase.POCChecks[index]);
        bool flag = false;
        if (CalculationBase.BorrowerFields[index] == "641" && (this.Val("GFE1") == "Y" || this.ToDouble(this.Val("448")) > 0.0))
          flag = true;
        if (CalculationBase.BorrowerFields[index] == "640" && (this.Val("GFE2") == "Y" || this.ToDouble(this.Val("449")) > 0.0))
          flag = true;
        if (CalculationBase.BorrowerFields[index] == "1621" && (this.Val("GFE3") == "Y" || this.ToDouble(this.Val("1878")) > 0.0))
          flag = true;
        double num3 = this.ToDouble(this.loan.GetField(CalculationBase.SellerFields[index]));
        double num4 = 0.0;
        if (simpleField == "Y" | flag)
        {
          if (CalculationBase.BorrowerFields[index] == "641" && this.ToDouble(this.Val("448")) > 0.0)
          {
            num4 = this.ToDouble(this.Val("448"));
            if (simpleField == "Y")
              num4 += this.ToDouble(this.loan.GetField(CalculationBase.BorrowerFields[index]));
          }
          else if (CalculationBase.BorrowerFields[index] == "640" && this.ToDouble(this.Val("449")) > 0.0)
          {
            num4 = this.ToDouble(this.Val("449"));
            if (simpleField == "Y")
              num4 += this.ToDouble(this.loan.GetField(CalculationBase.BorrowerFields[index]));
          }
          else if (CalculationBase.BorrowerFields[index] == "1621" && this.ToDouble(this.Val("1878")) > 0.0)
          {
            num4 = this.ToDouble(this.Val("1878"));
            if (simpleField == "Y")
              num4 += this.ToDouble(this.loan.GetField(CalculationBase.BorrowerFields[index]));
          }
          else
            num4 = this.ToDouble(this.loan.GetField(CalculationBase.BorrowerFields[index]));
          string str1;
          if (num4 != 0.0)
          {
            this.fieldCollect.Set(CalculationBase.BorrowerFields[index] + "_$", "$");
            str1 = num4.ToString("N2");
            if (simpleField == "Y")
              this.fieldCollect.Set("SYS_X" + CalculationBase.POCChecks[index], "P");
            if (flag)
              this.fieldCollect.Set(CalculationBase.BorrowerFields[index] + "_Paid", "Paid");
          }
          else
            str1 = "";
          this.fieldCollect.Set(CalculationBase.BorrowerFields[index] + "_POC", str1);
          if (CalculationBase.BorrowerFields[index] == "641" && this.ToDouble(this.Val("448")) > 0.0 && simpleField != "Y")
            num3 += this.ToDouble(this.Val("641"));
          if (CalculationBase.BorrowerFields[index] == "640" && this.ToDouble(this.Val("449")) > 0.0 && simpleField != "Y")
            num3 += this.ToDouble(this.Val("640"));
          if (CalculationBase.BorrowerFields[index] == "1621" && this.ToDouble(this.Val("1878")) > 0.0 && simpleField != "Y")
            num3 += this.ToDouble(this.Val("1621"));
          string str2 = num3 == 0.0 ? "" : num3.ToString("N2");
          this.fieldCollect.Set(CalculationBase.BorrowerFields[index], str2);
        }
        else
        {
          num3 += this.ToDouble(this.loan.GetField(CalculationBase.BorrowerFields[index]));
          string str = num3 == 0.0 ? "" : num3.ToString("N2");
          this.fieldCollect.Set(CalculationBase.BorrowerFields[index], str);
          this.fieldCollect.Set(CalculationBase.BorrowerFields[index] + "_$", "");
          this.fieldCollect.Set(CalculationBase.BorrowerFields[index] + "_POC", "");
        }
        if (CalculationBase.PREPAIDLIST.IndexOf("|" + CalculationBase.BorrowerFields[index] + "|") > -1)
          num2 += num3;
        else
          num1 += num3;
        if (this.loan.GetSimpleField(CalculationBase.BorrowerFields[index]) != "" && this.loan.GetSimpleField(CalculationBase.SellerFields[index]) != "")
        {
          this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], "S/");
        }
        else
        {
          switch (this.loan.GetSimpleField("SYS.X" + CalculationBase.PaidByChecks[index]))
          {
            case "":
            case null:
              if (this.loan.GetSimpleField(CalculationBase.SellerFields[index]) != "")
              {
                this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], "S");
                continue;
              }
              this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], " ");
              continue;
            case "Seller":
              if (num3 != 0.0)
              {
                this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], "S");
                continue;
              }
              continue;
            case "Broker":
              if (num3 != 0.0 || num4 != 0.0)
              {
                this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], "B");
                continue;
              }
              continue;
            case "Lender":
              if (num3 != 0.0 || num4 != 0.0)
              {
                this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], "L");
                continue;
              }
              continue;
            case "Other":
              if (num3 != 0.0 || num4 != 0.0)
              {
                this.fieldCollect.Set("SYS_X" + CalculationBase.PaidByChecks[index], "O");
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      if (num1 != 0.0)
        this.fieldCollect.Set("TOTAL_CC", num1.ToString("N2"));
      if (num2 != 0.0)
        this.fieldCollect.Set("TOTAL_PREPAID", num2.ToString("N2"));
      int num5 = 0;
      if (this.loan.GetSimpleField("143") != "")
      {
        ++num5;
        this.fieldCollect.Set("CC_SUMMARY_" + num5.ToString(), "Total Seller Paid CC");
        this.fieldCollect.Set("CCS_" + num5.ToString(), this.loan.GetField("143"));
      }
      if (this.loan.GetSimpleField("BKRPCC") != "")
      {
        ++num5;
        this.fieldCollect.Set("CC_SUMMARY_" + num5.ToString(), "Total Broker Paid CC");
        this.fieldCollect.Set("CCS_" + num5.ToString(), this.loan.GetField("BKRPCC"));
      }
      if (this.loan.GetSimpleField("LENPCC") != "")
      {
        ++num5;
        this.fieldCollect.Set("CC_SUMMARY_" + num5.ToString(), "Total Lender Paid CC");
        this.fieldCollect.Set("CCS_" + num5.ToString(), this.loan.GetField("LENPCC"));
      }
      if (this.loan.GetSimpleField("OTHPCC") != "")
      {
        ++num5;
        this.fieldCollect.Set("CC_SUMMARY_" + num5.ToString(), "Total Other Paid CC");
        this.fieldCollect.Set("CCS_" + num5.ToString(), this.loan.GetField("OTHPCC"));
      }
      if (!(this.loan.GetSimpleField("SYS.X8") == "Y"))
        return;
      this.fieldCollect.Set("333", Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetSimpleField("333")), 2).ToString("N2"));
    }

    private void printHUD1Page2()
    {
      this.pageDone.Trim();
      this.pageDone = string.Empty;
      if (this.loan.GetSimpleField("SYS.X8") == "Y")
        this.fieldCollect.Set("333", Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetSimpleField("333")), 2).ToString("N2"));
      int num1 = Utils.ParseInt((object) this.loan.GetField("L251"));
      int num2 = (num1 - num1 % 12) / 12;
      if (num2 > 0)
        this.fieldCollect.Set("L251", num2.ToString());
      else
        this.fieldCollect.Set("L251", "");
    }

    private void print2010HUDAddendum()
    {
      this.blockID = "REGZGFE_";
      this.lineCount = 0;
      if (this.pageDone == string.Empty)
      {
        this.print2010HUDAddendumDetail("801", "Our Origination Charge", (string) null, (string) null);
        ++this.lineCount;
        int lineCount1 = this.lineCount;
        this.print2010HUDAddendumDetail("801a", "Loan Origination Fees " + (this.Val("388") != string.Empty ? this.Val("388") + "%" : ""), "559", "SYS.X251");
        this.print2010HUDAddendumDetail("801b", "Application Fees", "L229", "SYS.X261");
        this.print2010HUDAddendumDetail("801c", "Processing Fees", "1622", "SYS.X269");
        this.print2010HUDAddendumDetail("801d", "Underwriting Fees", "569", "SYS.X271");
        this.print2010HUDAddendumDetail("801e", "Broker Fees " + (this.Val("389") != string.Empty ? this.Val("389") + "% + " : "") + (this.Val("1620") != string.Empty ? "$ " + this.Val("1620") : ""), "572", "SYS.X265");
        this.print2010HUDAddendumDetail("801f", "Broker Compensation " + (this.Val("NEWHUD.X223") != string.Empty ? this.Val("NEWHUD.X223") + "% + " : "") + (this.Val("NEWHUD.X224") != string.Empty ? "$ " + this.Val("NEWHUD.X224") : ""), "NEWHUD.X226", "NEWHUD.X227");
        this.print2010HUDAddendumDetail("801g", this.Val("154") + (this.Val("NEWHUD.X1045") != string.Empty ? " to " + this.Val("NEWHUD.X1045") : ""), "200", "SYS.X289");
        this.print2010HUDAddendumDetail("801h", this.Val("1627") + (this.Val("NEWHUD.X1046") != string.Empty ? " to " + this.Val("NEWHUD.X1046") : ""), "1626", "SYS.X291");
        this.print2010HUDAddendumDetail("801i", this.Val("1838") + (this.Val("NEWHUD.X1047") != string.Empty ? " to " + this.Val("NEWHUD.X1047") : ""), "1840", "SYS.X296");
        this.print2010HUDAddendumDetail("801j", this.Val("1841") + (this.Val("NEWHUD.X1048") != string.Empty ? " to " + this.Val("NEWHUD.X1048") : ""), "1843", "SYS.X301");
        this.print2010HUDAddendumDetail("801k", this.Val("NEWHUD.X732") + (this.Val("NEWHUD.X1049") != string.Empty ? " to " + this.Val("NEWHUD.X1049") : ""), "NEWHUD.X779", "NEWHUD.X748");
        this.print2010HUDAddendumDetail("801l", this.Val("NEWHUD.X1235") + (this.Val("NEWHUD.X1236") != string.Empty ? " to " + this.Val("NEWHUD.X1236") : ""), "NEWHUD.X1238", "NEWHUD.X1239");
        this.print2010HUDAddendumDetail("801m", this.Val("NEWHUD.X1243") + (this.Val("NEWHUD.X1244") != string.Empty ? " to " + this.Val("NEWHUD.X1244") : ""), "NEWHUD.X1246", "NEWHUD.X1247");
        this.print2010HUDAddendumDetail("801n", this.Val("NEWHUD.X1251") + (this.Val("NEWHUD.X1252") != string.Empty ? " to " + this.Val("NEWHUD.X1252") : ""), "NEWHUD.X1254", "NEWHUD.X1255");
        this.print2010HUDAddendumDetail("801o", this.Val("NEWHUD.X1259") + (this.Val("NEWHUD.X1260") != string.Empty ? " to " + this.Val("NEWHUD.X1260") : ""), "NEWHUD.X1262", "NEWHUD.X1263");
        this.print2010HUDAddendumDetail("801p", this.Val("NEWHUD.X1267") + (this.Val("NEWHUD.X1268") != string.Empty ? " to " + this.Val("NEWHUD.X1268") : ""), "NEWHUD.X1270", "NEWHUD.X1271");
        this.print2010HUDAddendumDetail("801q", this.Val("NEWHUD.X1275") + (this.Val("NEWHUD.X1276") != string.Empty ? " to " + this.Val("NEWHUD.X1276") : ""), "NEWHUD.X1278", "NEWHUD.X1279");
        this.print2010HUDAddendumDetail("801r", this.Val("NEWHUD.X1283") + (this.Val("NEWHUD.X1284") != string.Empty ? " to " + this.Val("NEWHUD.X1284") : ""), "NEWHUD.X1286", "NEWHUD.X1287");
        if (this.Val("NEWHUD.X713") == "Origination Charge")
        {
          if (this.Val("NEWHUD.X1139") == "Y")
          {
            this.print2010HUDAddendumDetail("801s", "Lender Compensation Credit" + (this.Val("NEWHUD.X1141") != string.Empty ? " " + this.Val("NEWHUD.X1141") + "%" : ""), "", "NEWHUD.X1167");
            this.print2010HUDAddendumDetail("801t", "Origination Credit" + (this.Val("NEWHUD.X1143") != string.Empty ? " " + this.Val("NEWHUD.X1143") + "%" : ""), "", "NEWHUD.X1169");
            this.print2010HUDAddendumDetail("801u", this.Val("NEWHUD.X1145"), "", "NEWHUD.X1171");
            this.print2010HUDAddendumDetail("801v", this.Val("NEWHUD.X1147"), "", "NEWHUD.X1173");
            this.print2010HUDAddendumDetail("801w", "Origination Points" + (this.Val("NEWHUD.X1150") != string.Empty ? " " + this.Val("NEWHUD.X1150") + "%" : ""), "NEWHUD.X1152", "NEWHUD.X1175");
            this.print2010HUDAddendumDetail("801x", this.Val("NEWHUD.X1153") + (this.Val("NEWHUD.X1154") != string.Empty ? " " + this.Val("NEWHUD.X1154") + "%" : ""), "NEWHUD.X1156", "NEWHUD.X1179");
            this.print2010HUDAddendumDetail("801y", this.Val("NEWHUD.X1157") + (this.Val("NEWHUD.X1158") != string.Empty ? " " + this.Val("NEWHUD.X1158") + "%" : ""), "NEWHUD.X1160", "NEWHUD.X1183");
            this.print2010HUDAddendumDetail("801z", this.Val("NEWHUD.X1161") + (this.Val("NEWHUD.X1162") != string.Empty ? " " + this.Val("NEWHUD.X1162") + "%" : ""), "NEWHUD.X1164", "NEWHUD.X1187");
          }
          else if (this.Val("NEWHUD.X715") == "Include Origination Credit")
            this.print2010HUDAddendumDetail("801s", "Origination Credit " + (this.Val("1847") != string.Empty ? this.Val("1847") + "% + " : "") + (this.Val("NEWHUD.X734") != string.Empty ? "$ " + this.Val("NEWHUD.X734") : ""), "NEWHUD.X788", "NEWHUD.X749");
          else if (this.Val("NEWHUD.X715") == "Include Origination Points")
          {
            if (this.FltVal("3119") > 0.0)
              this.print2010HUDAddendumDetail("801s", "Mortgage Buydown", "NEWHUD.X788", "NEWHUD.X749");
            else
              this.print2010HUDAddendumDetail("801s", "Origination Points " + (this.Val("1061") != string.Empty ? this.Val("1061") + "% + " : "") + (this.Val("436") != string.Empty ? "$ " + this.Val("436") : ""), "NEWHUD.X788", "NEWHUD.X749");
          }
        }
        if (this.lineCount == lineCount1)
        {
          --this.lineCount;
          this.print2010HUDAddendumDetail((string) null, (string) null, (string) null, (string) null);
        }
        else
          ++this.lineCount;
        this.print2010HUDAddendumDetail("802", "Your Credit or Points", (string) null, (string) null);
        ++this.lineCount;
        int lineCount2 = this.lineCount;
        if (this.Val("NEWHUD.X713") != "Origination Charge" && this.Val("NEWHUD.X1139") == "Y")
        {
          this.print2010HUDAddendumDetail("802a", "Lender Compensation Credit" + (this.Val("NEWHUD.X1141") != string.Empty ? " " + this.Val("NEWHUD.X1141") + "%" : ""), "", "NEWHUD.X1167");
          this.print2010HUDAddendumDetail("802b", "Origination Credit" + (this.Val("NEWHUD.X1143") != string.Empty ? " " + this.Val("NEWHUD.X1143") + "%" : ""), "", "NEWHUD.X1169");
          this.print2010HUDAddendumDetail("802c", this.Val("NEWHUD.X1145"), "", "NEWHUD.X1171");
          this.print2010HUDAddendumDetail("802d", this.Val("NEWHUD.X1147"), "", "NEWHUD.X1173");
          this.print2010HUDAddendumDetail("802e", "Origination Points" + (this.Val("NEWHUD.X1150") != string.Empty ? " " + this.Val("NEWHUD.X1150") + "%" : ""), "NEWHUD.X1152", "NEWHUD.X1175");
          this.print2010HUDAddendumDetail("802f", this.Val("NEWHUD.X1153") + (this.Val("NEWHUD.X1154") != string.Empty ? " " + this.Val("NEWHUD.X1154") + "%" : ""), "NEWHUD.X1156", "NEWHUD.X1179");
          this.print2010HUDAddendumDetail("802g", this.Val("NEWHUD.X1157") + (this.Val("NEWHUD.X1158") != string.Empty ? " " + this.Val("NEWHUD.X1158") + "%" : ""), "NEWHUD.X1160", "NEWHUD.X1183");
          this.print2010HUDAddendumDetail("802h", this.Val("NEWHUD.X1161") + (this.Val("NEWHUD.X1162") != string.Empty ? " " + this.Val("NEWHUD.X1162") + "%" : ""), "NEWHUD.X1164", "NEWHUD.X1187");
        }
        if (this.lineCount == lineCount2)
        {
          --this.lineCount;
          this.print2010HUDAddendumDetail((string) null, (string) null, (string) null, (string) null);
        }
        else
          ++this.lineCount;
        int lineCount3 = this.lineCount;
        this.print2010HUDAddendumDetail("820", this.Val("NEWHUD.X1307") + (this.Val("NEWHUD.X1308") != string.Empty ? " to " + this.Val("NEWHUD.X1308") : ""), "NEWHUD.X1310", "NEWHUD.X1311");
        this.print2010HUDAddendumDetail("821", this.Val("NEWHUD.X1315") + (this.Val("NEWHUD.X1316") != string.Empty ? " to " + this.Val("NEWHUD.X1316") : ""), "NEWHUD.X1318", "NEWHUD.X1319");
        this.print2010HUDAddendumDetail("822", this.Val("NEWHUD.X1323") + (this.Val("NEWHUD.X1324") != string.Empty ? " to " + this.Val("NEWHUD.X1324") : ""), "NEWHUD.X1326", "NEWHUD.X1327");
        this.print2010HUDAddendumDetail("823", this.Val("NEWHUD.X1331") + (this.Val("NEWHUD.X1332") != string.Empty ? " to " + this.Val("NEWHUD.X1332") : ""), "NEWHUD.X1334", "NEWHUD.X1335");
        this.print2010HUDAddendumDetail("824", this.Val("NEWHUD.X1339") + (this.Val("NEWHUD.X1340") != string.Empty ? " to " + this.Val("NEWHUD.X1340") : ""), "NEWHUD.X1342", "NEWHUD.X1343");
        this.print2010HUDAddendumDetail("825", this.Val("NEWHUD.X1347") + (this.Val("NEWHUD.X1348") != string.Empty ? " to " + this.Val("NEWHUD.X1348") : ""), "NEWHUD.X1350", "NEWHUD.X1351");
        this.print2010HUDAddendumDetail("826", this.Val("NEWHUD.X1355") + (this.Val("NEWHUD.X1356") != string.Empty ? " to " + this.Val("NEWHUD.X1356") : ""), "NEWHUD.X1358", "NEWHUD.X1359");
        this.print2010HUDAddendumDetail("827", this.Val("NEWHUD.X1363") + (this.Val("NEWHUD.X1364") != string.Empty ? " to " + this.Val("NEWHUD.X1364") : ""), "NEWHUD.X1366", "NEWHUD.X1367");
        this.print2010HUDAddendumDetail("828", this.Val("NEWHUD.X1371") + (this.Val("NEWHUD.X1372") != string.Empty ? " to " + this.Val("NEWHUD.X1372") : ""), "NEWHUD.X1374", "NEWHUD.X1375");
        this.print2010HUDAddendumDetail("829", this.Val("NEWHUD.X1379") + (this.Val("NEWHUD.X1380") != string.Empty ? " to " + this.Val("NEWHUD.X1380") : ""), "NEWHUD.X1382", "NEWHUD.X1383");
        this.print2010HUDAddendumDetail("830", this.Val("NEWHUD.X1387") + (this.Val("NEWHUD.X1388") != string.Empty ? " to " + this.Val("NEWHUD.X1388") : ""), "NEWHUD.X1390", "NEWHUD.X1391");
        this.print2010HUDAddendumDetail("831", this.Val("NEWHUD.X1395") + (this.Val("NEWHUD.X1396") != string.Empty ? " to " + this.Val("NEWHUD.X1396") : ""), "NEWHUD.X1398", "NEWHUD.X1399");
        this.print2010HUDAddendumDetail("832", this.Val("NEWHUD.X1403") + (this.Val("NEWHUD.X1404") != string.Empty ? " to " + this.Val("NEWHUD.X1404") : ""), "NEWHUD.X1406", "NEWHUD.X1407");
        this.print2010HUDAddendumDetail("833", this.Val("NEWHUD.X1411") + (this.Val("NEWHUD.X1412") != string.Empty ? " to " + this.Val("NEWHUD.X1412") : ""), "NEWHUD.X1414", "NEWHUD.X1415");
        this.print2010HUDAddendumDetail("834", this.Val("410") + (this.Val("NEWHUD.X1060") != string.Empty ? " to " + this.Val("NEWHUD.X1060") : ""), "678", "SYS.X285");
        this.print2010HUDAddendumDetail("835", this.Val("NEWHUD.X656") + (this.Val("NEWHUD.X1061") != string.Empty ? " to " + this.Val("NEWHUD.X1061") : ""), "NEWHUD.X658", "NEWHUD.X162");
        if (this.lineCount > lineCount3)
          ++this.lineCount;
        int lineCount4 = this.lineCount;
        this.print2010HUDAddendumDetail("911", this.Val("NEWHUD.X1586") + (this.Val("NEWHUD.X1587") != string.Empty ? " to " + this.Val("NEWHUD.X1587") : ""), "NEWHUD.X1589", "NEWHUD.X1590");
        this.print2010HUDAddendumDetail("912", this.Val("NEWHUD.X1594") + (this.Val("NEWHUD.X1595") != string.Empty ? " to " + this.Val("NEWHUD.X1595") : ""), "NEWHUD.X1597", "NEWHUD.X1598");
        if (this.lineCount > lineCount4)
          ++this.lineCount;
        this.print2010HUDAddendumDetail("1101", "Title Services and Lender's Title Insurance", (string) null, (string) null);
        ++this.lineCount;
        int lineCount5 = this.lineCount;
        this.print2010HUDAddendumDetail("1101a", this.Val("NEWHUD.X951") + (this.Val("NEWHUD.X1070") != string.Empty ? " to " + this.Val("NEWHUD.X1070") : ""), "NEWHUD.X953", "NEWHUD.X955");
        this.print2010HUDAddendumDetail("1101b", this.Val("NEWHUD.X960") + (this.Val("NEWHUD.X1071") != string.Empty ? " to " + this.Val("NEWHUD.X1071") : ""), "NEWHUD.X962", "NEWHUD.X964");
        this.print2010HUDAddendumDetail("1101c", this.Val("NEWHUD.X969") + (this.Val("NEWHUD.X1072") != string.Empty ? " to " + this.Val("NEWHUD.X1072") : ""), "NEWHUD.X971", "NEWHUD.X973");
        this.print2010HUDAddendumDetail("1101d", this.Val("NEWHUD.X978") + (this.Val("NEWHUD.X1073") != string.Empty ? " to " + this.Val("NEWHUD.X1073") : ""), "NEWHUD.X980", "NEWHUD.X982");
        this.print2010HUDAddendumDetail("1101e", this.Val("NEWHUD.X987") + (this.Val("NEWHUD.X1074") != string.Empty ? " to " + this.Val("NEWHUD.X1074") : ""), "NEWHUD.X989", "NEWHUD.X991");
        this.print2010HUDAddendumDetail("1101f", this.Val("NEWHUD.X996") + (this.Val("NEWHUD.X1075") != string.Empty ? " to " + this.Val("NEWHUD.X1075") : ""), "NEWHUD.X998", "NEWHUD.X1000");
        this.print2010HUDAddendumDetail("1102", "Settlement or Closing Fee" + (this.Val("NEWHUD.X203") != string.Empty ? " to " + this.Val("NEWHUD.X203") : ""), "NEWHUD.X782", "NEWHUD.X743");
        this.print2010HUDAddendumDetail("1104", "Lender's Title Insurance" + (this.Val("NEWHUD.X205") != string.Empty ? " to " + this.Val("NEWHUD.X205") : ""), "NEWHUD.X784", "NEWHUD.X745");
        this.print2010HUDAddendumDetail("1109", this.Val("NEWHUD.X208") + (this.Val("NEWHUD.X1076") != string.Empty ? " to " + this.Val("NEWHUD.X1076") : ""), "NEWHUD.X218", "NEWHUD.X221");
        this.print2010HUDAddendumDetail("1110", this.Val("NEWHUD.X209") + (this.Val("NEWHUD.X1077") != string.Empty ? " to " + this.Val("NEWHUD.X1077") : ""), "NEWHUD.X219", "NEWHUD.X222");
        this.print2010HUDAddendumDetail("1111", this.Val("1762") + (this.Val("NEWHUD.X1078") != string.Empty ? " to " + this.Val("NEWHUD.X1078") : ""), "1764", "SYS.X347");
        this.print2010HUDAddendumDetail("1112", this.Val("1767") + (this.Val("NEWHUD.X1079") != string.Empty ? " to " + this.Val("NEWHUD.X1079") : ""), "1769", "SYS.X349");
        this.print2010HUDAddendumDetail("1113", this.Val("1772") + (this.Val("NEWHUD.X1080") != string.Empty ? " to " + this.Val("NEWHUD.X1080") : ""), "1774", "SYS.X351");
        this.print2010HUDAddendumDetail("1114", this.Val("1777") + (this.Val("NEWHUD.X1081") != string.Empty ? " to " + this.Val("NEWHUD.X1081") : ""), "1779", "SYS.X353");
        this.print2010HUDAddendumDetail("1115", this.Val("NEWHUD.X1602") + (this.Val("NEWHUD.X1603") != string.Empty ? " to " + this.Val("NEWHUD.X1603") : ""), "NEWHUD.X1605", "NEWHUD.X1606");
        this.print2010HUDAddendumDetail("1116", this.Val("NEWHUD.X1610") + (this.Val("NEWHUD.X1611") != string.Empty ? " to " + this.Val("NEWHUD.X1611") : ""), "NEWHUD.X1613", "NEWHUD.X1614");
        if (this.lineCount == lineCount5)
        {
          --this.lineCount;
          this.print2010HUDAddendumDetail((string) null, (string) null, (string) null, (string) null);
        }
        else
          ++this.lineCount;
        int lineCount6 = this.lineCount;
        this.print2010HUDAddendumDetail("1209", this.Val("NEWHUD.X1618") + (this.Val("NEWHUD.X1619") != string.Empty ? " to " + this.Val("NEWHUD.X1619") : ""), "NEWHUD.X1621", "NEWHUD.X1622");
        this.print2010HUDAddendumDetail("1210", this.Val("NEWHUD.X1625") + (this.Val("NEWHUD.X1626") != string.Empty ? " to " + this.Val("NEWHUD.X1626") : ""), "NEWHUD.X1628", "NEWHUD.X1629");
        if (this.lineCount > lineCount6)
          ++this.lineCount;
      }
      if (this.lineCount > 69)
      {
        this.pageDone = "1310";
      }
      else
      {
        if (this.pageDone == "1310" || this.pageDone == string.Empty)
        {
          this.pageDone = string.Empty;
          this.print2010HUDAddendumDetail("1310", this.Val("NEWHUD.X252") + (this.Val("NEWHUD.X1093") != string.Empty ? " to " + this.Val("NEWHUD.X1093") : ""), "NEWHUD.X259", "NEWHUD.X263");
          if (this.lineCount > 69)
          {
            this.pageDone = "1311";
            return;
          }
        }
        if (this.pageDone == "1311" || this.pageDone == string.Empty)
        {
          this.pageDone = string.Empty;
          this.print2010HUDAddendumDetail("1311", this.Val("NEWHUD.X253") + (this.Val("NEWHUD.X1094") != string.Empty ? " to " + this.Val("NEWHUD.X1094") : ""), "NEWHUD.X260", "NEWHUD.X264");
          if (this.lineCount > 69)
          {
            this.pageDone = "1312";
            return;
          }
        }
        if (this.pageDone == "1312" || this.pageDone == string.Empty)
        {
          this.pageDone = string.Empty;
          this.print2010HUDAddendumDetail("1312", this.Val("NEWHUD.X1632") + (this.Val("NEWHUD.X1633") != string.Empty ? " to " + this.Val("NEWHUD.X1633") : ""), "NEWHUD.X1635", "NEWHUD.X1636");
          if (this.lineCount > 69)
          {
            this.pageDone = "1313";
            return;
          }
        }
        if (this.pageDone == "1313" || this.pageDone == string.Empty)
        {
          this.pageDone = string.Empty;
          this.print2010HUDAddendumDetail("1313", this.Val("NEWHUD.X1640") + (this.Val("NEWHUD.X1641") != string.Empty ? " to " + this.Val("NEWHUD.X1641") : ""), "NEWHUD.X1643", "NEWHUD.X1644");
          if (this.lineCount > 69)
          {
            this.pageDone = "1314";
            return;
          }
        }
        if (this.pageDone == "1314" || this.pageDone == string.Empty)
        {
          this.pageDone = string.Empty;
          this.print2010HUDAddendumDetail("1314", this.Val("NEWHUD.X1648") + (this.Val("NEWHUD.X1649") != string.Empty ? " to " + this.Val("NEWHUD.X1649") : ""), "NEWHUD.X1651", "NEWHUD.X1652");
          if (this.lineCount > 70)
          {
            this.pageDone = "1315";
            return;
          }
        }
        if (this.pageDone == "1315" || this.pageDone == string.Empty)
          this.print2010HUDAddendumDetail("1315", this.Val("NEWHUD.X1656") + (this.Val("NEWHUD.X1657") != string.Empty ? " to " + this.Val("NEWHUD.X1657") : ""), "NEWHUD.X1659", "NEWHUD.X1660");
        this.pageDone = string.Empty;
      }
    }

    private void print2010HUDAddendumDetail(
      string lineID,
      string feeDescription,
      string selID,
      string paidByID)
    {
      switch (lineID)
      {
        case null:
          this.fieldPos = this.lineCount * 6;
          for (int index = 1; index <= 6; ++index)
            this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + index), "");
          this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + 6) + "a", "");
          break;
        case "1101":
        case "801":
        case "802":
          this.fieldPos = this.lineCount * 6;
          this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + 1), lineID);
          this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + 2), feeDescription);
          break;
        default:
          string[] strArray = (string[]) null;
          if (HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID))
            strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
          string id = strArray != null ? strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] : string.Empty;
          string str1 = strArray != null ? strArray[HUDGFE2010Fields.PTCPOCINDEX_POC] : string.Empty;
          string str2 = strArray != null ? strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY] : string.Empty;
          string str3 = strArray != null ? strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC] : string.Empty;
          switch (paidByID)
          {
            case "NEWHUD.X1167":
              id = "NEWHUD.X1142";
              break;
            case "NEWHUD.X1169":
              id = "NEWHUD.X1144";
              break;
            case "NEWHUD.X1171":
              id = "NEWHUD.X1146";
              break;
            case "NEWHUD.X1173":
              id = "NEWHUD.X1148";
              break;
          }
          double num1 = this.FltVal(id);
          double num2 = 0.0;
          double num3 = strArray != null ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]) : 0.0;
          double num4 = strArray != null ? this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]) : 0.0;
          if (selID != "")
            num2 = this.FltVal(selID);
          if (num1 + num2 == 0.0)
            break;
          bool flag = Utils.ParseInt((object) lineID) >= 820 && Utils.ParseInt((object) lineID) <= 835 || Utils.ParseInt((object) lineID) >= 911 && Utils.ParseInt((object) lineID) <= 912 || Utils.ParseInt((object) lineID) >= 1115 && Utils.ParseInt((object) lineID) <= 1116 || Utils.ParseInt((object) lineID) >= 1209 && Utils.ParseInt((object) lineID) <= 1210 || Utils.ParseInt((object) lineID) >= 1310 && Utils.ParseInt((object) lineID) <= 1315;
          this.fieldPos = this.lineCount * 6;
          ++this.lineCount;
          if (flag)
          {
            this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + 1), lineID);
            this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + 2), feeDescription);
          }
          else
            this.fieldCollect.Set(this.blockID + (object) (this.fieldPos + 2), lineID + ". " + feeDescription);
          string name1 = this.blockID + (object) (this.fieldPos + 3);
          string name2 = this.blockID + (object) (this.fieldPos + 4);
          string name3 = this.blockID + (object) (this.fieldPos + 5) + "a";
          string name4 = this.blockID + (object) (this.fieldPos + 5);
          string name5 = this.blockID + (object) (this.fieldPos + 6) + "a";
          string name6 = this.blockID + (object) (this.fieldPos + 6);
          if (num3 > 0.0)
          {
            string str4 = "(";
            string str5;
            switch (strArray != null ? this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]) : "")
            {
              case "Broker":
                str5 = str4 + "B";
                break;
              case "Lender":
                str5 = str4 + "L";
                break;
              case "Other":
                str5 = str4 + "O";
                break;
              case "Seller":
                str5 = str4 + "S";
                break;
              default:
                str5 = str4 + "P";
                break;
            }
            string str6 = str5 + " $ " + num3.ToString("N2") + ")";
            this.fieldCollect.Set(name2, str6);
          }
          if (num4 > 0.0)
          {
            string str7 = "(";
            string str8;
            switch (strArray != null ? this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]) : "")
            {
              case "Broker":
                str8 = str7 + "B";
                break;
              case "Lender":
                str8 = str7 + "L";
                break;
              case "Other":
                str8 = str7 + "O";
                break;
              case "Seller":
                str8 = str7 + "S";
                break;
              default:
                str8 = str7 + "P";
                break;
            }
            string str9 = str8 + " $ " + num4.ToString("N2") + ")";
            this.fieldCollect.Set(name1, str9);
          }
          double num5 = num1 - num3 - num4;
          if (num5 > 0.0)
          {
            if (lineID == "801s" || lineID == "801t" || lineID == "801u" || lineID == "801v" || lineID == "802a" || lineID == "802b" || lineID == "802c" || lineID == "802d")
            {
              if (flag)
                this.fieldCollect.Set(name5, num5 != 0.0 ? "($ " + num5.ToString("N2") + ")" : "");
              else
                this.fieldCollect.Set(name3, num5 != 0.0 ? "($ " + num5.ToString("N2") + ")" : "");
            }
            else if (flag)
              this.fieldCollect.Set(name5, num5 != 0.0 ? "$ " + num5.ToString("N2") : "");
            else
              this.fieldCollect.Set(name3, num5 != 0.0 ? "$ " + num5.ToString("N2") : "");
          }
          if (num2 <= 0.0)
            break;
          if (flag)
          {
            this.fieldCollect.Set(name6, num2 != 0.0 ? "$ " + num2.ToString("N2") : "");
            break;
          }
          this.fieldCollect.Set(name4, num2 != 0.0 ? "$ " + num2.ToString("N2") : "");
          break;
      }
    }

    private void print2010HUD1Page2()
    {
      this.printHUD1Page2();
      double num = this.FltVal("454") + this.FltVal("559");
      if (num > 0.0)
        this.fieldCollect.Set("454", "(Includes Origination Points " + this.loan.GetField("388") + " % or $ " + num.ToString("N2") + ")");
      this.printPOCIn2010HUD1Page2("641", "581", "NEWHUD.X609", "SYS.X231", "SYS.X255", "NEWHUD.X832", "NEWHUD.X895", false);
      this.printPOCIn2010HUD1Page2("640", "580", "NEWHUD.X610", "SYS.X232", "SYS.X257", "NEWHUD.X833", "NEWHUD.X896", false);
      this.printPOCIn2010HUD1Page2("336", "565", "NEWHUD.X611", "SYS.X141", "SYS.X267", "NEWHUD.X834", "NEWHUD.X897", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X400", "NEWHUD.X781", "NEWHUD.X612", "NEWHUD.X751", "NEWHUD.X742", "NEWHUD.X835", "NEWHUD.X898", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X136", "NEWHUD.X147", "NEWHUD.X662", "NEWHUD.X752", "NEWHUD.X157", "NEWHUD.X836", "NEWHUD.X899", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X137", "NEWHUD.X148", "NEWHUD.X663", "NEWHUD.X753", "NEWHUD.X158", "NEWHUD.X837", "NEWHUD.X900", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X138", "NEWHUD.X149", "NEWHUD.X664", "NEWHUD.X754", "NEWHUD.X159", "NEWHUD.X838", "NEWHUD.X901", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X139", "NEWHUD.X150", "NEWHUD.X665", "NEWHUD.X755", "NEWHUD.X160", "NEWHUD.X839", "NEWHUD.X902", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X140", "NEWHUD.X151", "NEWHUD.X666", "NEWHUD.X756", "NEWHUD.X161", "NEWHUD.X840", "NEWHUD.X903", false);
      this.printPOCIn2010HUD1Page2("370", "574", "NEWHUD.X667", "SYS.X149", "SYS.X275", "NEWHUD.X841", "NEWHUD.X904", false);
      this.printPOCIn2010HUD1Page2("372", "575", "NEWHUD.X668", "SYS.X150", "SYS.X277", "NEWHUD.X842", "NEWHUD.X905", false);
      this.printPOCIn2010HUD1Page2("349", "96", "NEWHUD.X669", "SYS.X151", "SYS.X279", "NEWHUD.X843", "NEWHUD.X906", false);
      this.printPOCIn2010HUD1Page2("932", "1345", "NEWHUD.X670", "SYS.X152", "SYS.X281", "NEWHUD.X844", "NEWHUD.X907", false);
      this.printPOCIn2010HUD1Page2("1009", "6", "NEWHUD.X671", "SYS.X153", "SYS.X283", "NEWHUD.X845", "NEWHUD.X908", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X1293", "NEWHUD.X1294", "NEWHUD.X1525", "NEWHUD.X1296", "NEWHUD.X1295", "NEWHUD.X1440", "NEWHUD.X1463", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X1301", "NEWHUD.X1302", "NEWHUD.X1526", "NEWHUD.X1304", "NEWHUD.X1303", "NEWHUD.X1441", "NEWHUD.X1464", false);
      this.printPOCIn2010HUD1Page2("334", "561", "NEWHUD.X701", "SYS.X157", "SYS.X303", "NEWHUD.X848", "NEWHUD.X911", false);
      this.printPOCIn2010HUD1Page2("337", "562", "NEWHUD.X622", "SYS.X158", "SYS.X305", "NEWHUD.X849", "NEWHUD.X912", false);
      this.printPOCIn2010HUD1Page2("642", "578", "NEWHUD.X650", "SYS.X159", "SYS.X307", "NEWHUD.X850", "NEWHUD.X913", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X591", "NEWHUD.X594", "NEWHUD.X585", "NEWHUD.X647", "NEWHUD.X163", "NEWHUD.X851", "NEWHUD.X914", false);
      this.printPOCIn2010HUD1Page2("1050", "571", "NEWHUD.X661", "SYS.X235", "SYS.X311", "NEWHUD.X852", "NEWHUD.X915", false);
      this.printPOCIn2010HUD1Page2("643", "579", "NEWHUD.X586", "SYS.X160", "SYS.X309", "NEWHUD.X853", "NEWHUD.X916", false);
      this.printPOCIn2010HUD1Page2("L260", "L261", "NEWHUD.X587", "SYS.X161", "SYS.X313", "NEWHUD.X854", "NEWHUD.X917", false);
      this.printPOCIn2010HUD1Page2("1667", "1668", "NEWHUD.X588", "SYS.X238", "SYS.X315", "NEWHUD.X855", "NEWHUD.X918", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X592", "NEWHUD.X595", "NEWHUD.X589", "NEWHUD.X648", "NEWHUD.X164", "NEWHUD.X856", "NEWHUD.X919", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X593", "NEWHUD.X596", (string) null, "NEWHUD.X649", "NEWHUD.X166", "NEWHUD.X857", "NEWHUD.X920", false);
      this.printPOCIn2010HUD1Page2("656", "596", "NEWHUD.X692", (string) null, "SYS.X317", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("338", "563", "NEWHUD.X693", (string) null, "SYS.X319", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("655", "595", "NEWHUD.X694", (string) null, "SYS.X323", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("L269", "L270", "NEWHUD.X695", (string) null, "SYS.X321", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("657", "597", "NEWHUD.X696", (string) null, "SYS.X325", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("1631", "1632", "NEWHUD.X697", (string) null, "SYS.X327", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("658", "598", "NEWHUD.X698", (string) null, "SYS.X329", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("659", "599", "NEWHUD.X699", (string) null, "SYS.X331", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X1708", "NEWHUD.X1714", "NEWHUD.X1709", (string) null, "NEWHUD.X1710", (string) null, (string) null, false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X952", "NEWHUD.X953", "NEWHUD.X954", "NEWHUD.X956", "NEWHUD.X955", "NEWHUD.X1005", "NEWHUD.X1011", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X961", "NEWHUD.X962", "NEWHUD.X963", "NEWHUD.X965", "NEWHUD.X964", "NEWHUD.X1006", "NEWHUD.X1012", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X970", "NEWHUD.X971", "NEWHUD.X972", "NEWHUD.X974", "NEWHUD.X973", "NEWHUD.X1007", "NEWHUD.X1013", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X979", "NEWHUD.X980", "NEWHUD.X981", "NEWHUD.X983", "NEWHUD.X982", "NEWHUD.X1008", "NEWHUD.X1014", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X988", "NEWHUD.X989", "NEWHUD.X990", "NEWHUD.X992", "NEWHUD.X991", "NEWHUD.X1009", "NEWHUD.X1015", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X997", "NEWHUD.X998", "NEWHUD.X999", "NEWHUD.X1001", "NEWHUD.X1000", "NEWHUD.X1010", "NEWHUD.X1016", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X645", "NEWHUD.X782", "NEWHUD.X210", "NEWHUD.X758", "NEWHUD.X743", "NEWHUD.X858", "NEWHUD.X921", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X572", "NEWHUD.X783", "NEWHUD.X39", "NEWHUD.X759", "NEWHUD.X744", "NEWHUD.X859", "NEWHUD.X922", false);
      this.printPOCIn2010HUD1Page2("NEWHUD.X639", "NEWHUD.X784", "NEWHUD.X211", "NEWHUD.X760", "NEWHUD.X745", "NEWHUD.X860", "NEWHUD.X923", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X215", "NEWHUD.X218", "NEWHUD.X565", "NEWHUD.X763", "NEWHUD.X221", "NEWHUD.X861", "NEWHUD.X924", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X216", "NEWHUD.X219", "NEWHUD.X566", "NEWHUD.X764", "NEWHUD.X222", "NEWHUD.X862", "NEWHUD.X925", true);
      this.printPOCIn2010HUD1Page2("1763", "1764", "NEWHUD.X567", "SYS.X244", "SYS.X347", "NEWHUD.X863", "NEWHUD.X926", true);
      this.printPOCIn2010HUD1Page2("1768", "1769", "NEWHUD.X568", "SYS.X245", "SYS.X349", "NEWHUD.X864", "NEWHUD.X927", true);
      this.printPOCIn2010HUD1Page2("1773", "1774", "NEWHUD.X569", "SYS.X246", "SYS.X351", "NEWHUD.X865", "NEWHUD.X928", true);
      this.printPOCIn2010HUD1Page2("1778", "1779", "NEWHUD.X570", "SYS.X247", "SYS.X353", "NEWHUD.X866", "NEWHUD.X929", true);
      this.printPOCIn2010HUD1Page2("390", "587", "NEWHUD.X604", "SYS.X182", "SYS.X355", "NEWHUD.X867", "NEWHUD.X930", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X731", "NEWHUD.X787", "NEWHUD.X730", "NEWHUD.X765", "NEWHUD.X261", "NEWHUD.X868", "NEWHUD.X931", true);
      this.printPOCIn2010HUD1Page2("647", "593", "NEWHUD.X605", "SYS.X183", "SYS.X357", "NEWHUD.X869", "NEWHUD.X932", true);
      this.printPOCIn2010HUD1Page2("648", "594", "NEWHUD.X606", "SYS.X184", "SYS.X359", "NEWHUD.X870", "NEWHUD.X933", true);
      this.printPOCIn2010HUD1Page2("374", "576", "NEWHUD.X724", "SYS.X185", "SYS.X361", "NEWHUD.X871", "NEWHUD.X934", true);
      this.printPOCIn2010HUD1Page2("1641", "1642", "NEWHUD.X771", "SYS.X241", "SYS.X363", "NEWHUD.X872", "NEWHUD.X935", true);
      this.printPOCIn2010HUD1Page2("1644", "1645", "NEWHUD.X772", "SYS.X242", "SYS.X365", "NEWHUD.X873", "NEWHUD.X936", true);
      this.printPOCIn2010HUD1Page2("NEWHUD.X254", "NEWHUD.X258", "NEWHUD.X42", "NEWHUD.X766", "NEWHUD.X262", "NEWHUD.X874", "NEWHUD.X937", true);
      this.printPOCIn2010HUD1Page2("644", "590", "NEWHUD.X44", "SYS.X190", "SYS.X374", "NEWHUD.X875", "NEWHUD.X938", true);
      this.printPOCIn2010HUD1Page2("645", "591", "NEWHUD.X46", "SYS.X191", "SYS.X376", "NEWHUD.X876", "NEWHUD.X939", true);
      this.printPOCIn2010HUD1Page2("41", "42", "NEWHUD.X48", "SYS.X192", "SYS.X378", "NEWHUD.X877", "NEWHUD.X940", true);
      this.printPOCIn2010HUD1Page2("44", "55", "NEWHUD.X50", "SYS.X193", "SYS.X380", "NEWHUD.X878", "NEWHUD.X941", true);
      this.printPOCIn2010HUD1Page2("1783", "1784", "NEWHUD.X52", "SYS.X248", "SYS.X382", "NEWHUD.X879", "NEWHUD.X942", true);
      this.printPOCIn2010HUD1Page2("1788", "1789", "NEWHUD.X54", "SYS.X249", "SYS.X384", "NEWHUD.X880", "NEWHUD.X943", true);
      this.printPOCIn2010HUD1Page2("1793", "1794", "NEWHUD.X56", "SYS.X250", "SYS.X386", "NEWHUD.X881", "NEWHUD.X944", true);
    }

    private void printPOCIn2010HUD1Page2(
      string borID,
      string selID,
      string gfeID,
      string pocID,
      string paidByID,
      string pocAmtID,
      string pocPaidByID,
      bool printComponentFee)
    {
      double num1 = this.FltVal(borID);
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = pocAmtID == null ? 0.0 : this.FltVal(pocAmtID);
      if (selID != "")
        num2 = this.FltVal(selID);
      if (gfeID != null)
        num3 = this.FltVal(gfeID);
      double num5 = num1 + num2;
      borID = borID.Replace(".", "_");
      selID = selID.Replace(".", "_");
      if (pocID != null && this.Val(pocID) == "Y" && num1 > 0.0)
      {
        double num6 = num1 + num2 - num4;
        if (num6 > 0.0 && !printComponentFee && gfeID != null && num3 != 0.0 && num3 != num1)
        {
          this.fieldCollect.Set(borID, num6.ToString("N2"));
        }
        else
        {
          num6 = num1 - num4;
          this.fieldCollect.Set(borID, num6 > 0.0 ? num6.ToString("N2") : "");
        }
        string str1 = "POC(";
        string str2;
        switch (this.Val(pocPaidByID))
        {
          case "Broker":
            str2 = str1 + "B";
            break;
          case "Lender":
            str2 = str1 + "L";
            break;
          case "Other":
            str2 = str1 + "O";
            break;
          case "Seller":
            str2 = str1 + "S";
            break;
          default:
            str2 = str1 + "P";
            break;
        }
        string str3 = str2 + " *-";
        this.fieldCollect.Set(borID + "_P$", str3);
        this.fieldCollect.Set(borID + "_P", "$ " + num4.ToString("N2") + ")");
        if (!(selID != ""))
          return;
        this.fieldCollect.Set(selID, "");
        if (num2 != 0.0 && gfeID == null)
          this.fieldCollect.Set(selID, num2 != 0.0 ? num2.ToString("N2") : "");
        if (printComponentFee && num2 != 0.0 && num3 != num1 && num3 != 0.0)
        {
          num6 = num2 + num1 - num4;
          this.fieldCollect.Set(selID + "_S", num6 != 0.0 ? "$ " + num6.ToString("N2") : "");
        }
        else
        {
          if (num3 != 0.0 && num3 != num1)
            return;
          this.fieldCollect.Set(selID, num2 != 0.0 ? num2.ToString("N2") : "");
          num6 = num1 - num4;
          if (!printComponentFee || num6 <= 0.0)
            return;
          this.fieldCollect.Set(selID + "_S", num6 != 0.0 ? "$ " + num6.ToString("N2") : "");
        }
      }
      else
      {
        this.fieldCollect.Set(borID + "_P$", "");
        this.fieldCollect.Set(borID + "_P", "");
        if (printComponentFee)
        {
          if (num1 > 0.0 || gfeID != string.Empty && num3 != 0.0)
          {
            if (num3 == 0.0 || num3 == num1)
            {
              this.fieldCollect.Set(borID + "_P", num1 != 0.0 ? "$ " + num1.ToString("N2") : "");
              this.fieldCollect.Set(selID, num2 != 0.0 ? num2.ToString("N2") : "");
            }
            else
            {
              this.fieldCollect.Set(borID + "_P", num5 != 0.0 ? "$ " + num5.ToString("N2") : "");
              this.fieldCollect.Set(selID, "");
            }
          }
          else
          {
            if (num2 <= 0.0)
              return;
            this.fieldCollect.Set(selID, num2.ToString("N2"));
          }
        }
        else
        {
          this.fieldCollect.Set(borID + "_P", "");
          if (num5 != 0.0)
          {
            if (num1 > 0.0 || gfeID != null && num3 != 0.0)
            {
              if (num3 == num1 || num3 == 0.0)
              {
                this.fieldCollect.Set(borID, num1.ToString("N2"));
                this.fieldCollect.Set(selID, num2 != 0.0 ? num2.ToString("N2") : "");
              }
              else
              {
                this.fieldCollect.Set(borID, num5.ToString("N2"));
                this.fieldCollect.Set(selID, "");
              }
            }
            else
            {
              if (num2 <= 0.0)
                return;
              this.fieldCollect.Set(selID, num2.ToString("N2"));
            }
          }
          else if (borID == "NEWHUD_X572")
            this.fieldCollect.Set(borID, this.loan.GetField("NEWHUD.X572"));
          else
            this.fieldCollect.Set(borID, "");
        }
      }
    }

    private void printLoanLogEntries()
    {
      string val1 = this.pageDone.Trim();
      this.pageDone = string.Empty;
      LogRecordBase[] allDatedRecords = this.loan.GetLogList().GetAllDatedRecords();
      ArrayList arrayList = new ArrayList();
      int num1 = 0;
      if (val1 != string.Empty)
        num1 = this.ToInt(val1);
      int num2 = 0;
      for (int index = num1; index < allDatedRecords.Length; ++index)
      {
        ++num2;
        if (num2 > 8)
        {
          this.pageDone = index.ToString();
          break;
        }
        LogRecordBase logRecordBase = allDatedRecords[index];
        DateTime dateTime1 = DateTime.MinValue;
        string str1 = string.Empty;
        string str2 = string.Empty;
        string str3 = string.Empty;
        switch (logRecordBase)
        {
          case MilestoneLog _:
            MilestoneLog milestoneLog = (MilestoneLog) logRecordBase;
            str1 = !milestoneLog.Done ? milestoneLog.ExpText : milestoneLog.DoneText;
            dateTime1 = logRecordBase.Date;
            str2 = "Milestone";
            str3 = milestoneLog.Comments;
            break;
          case DocumentLog _:
            DocumentLog documentLog = (DocumentLog) logRecordBase;
            dateTime1 = logRecordBase.Date;
            str1 = ((!(documentLog is VerifLog) ? documentLog.Title : documentLog.Title + "-" + documentLog.RequestedFrom) + " " + documentLog.Status).Trim();
            str2 = "Document";
            str3 = documentLog.Comments.ToString();
            break;
          case ConversationLog _:
            ConversationLog conversationLog = logRecordBase as ConversationLog;
            if (!arrayList.Contains((object) conversationLog))
              arrayList.Add((object) conversationLog);
            DateTime minValue1 = DateTime.MinValue;
            StringBuilder stringBuilder1 = new StringBuilder();
            LogAlert mostCriticalAlert1 = conversationLog.AlertList.GetMostCriticalAlert();
            DateTime dateTime2;
            if (mostCriticalAlert1 == null)
            {
              dateTime2 = conversationLog.Date;
              if (conversationLog.IsEmail)
                stringBuilder1.Append("Emailed " + conversationLog.Name);
              else
                stringBuilder1.Append("Called " + conversationLog.Name);
            }
            else if (DateTime.MinValue == mostCriticalAlert1.FollowedUpDate)
            {
              dateTime2 = mostCriticalAlert1.DueDate;
              stringBuilder1.Append("Follow up w/ " + conversationLog.Name);
            }
            else
            {
              dateTime2 = mostCriticalAlert1.FollowedUpDate;
              stringBuilder1.Append("Followed up w/ " + conversationLog.Name);
            }
            str2 = "Conversation";
            dateTime1 = dateTime2;
            str1 = stringBuilder1.ToString();
            str3 = conversationLog.Comments;
            break;
          case LogEntryLog _:
            LogEntryLog logEntryLog = (LogEntryLog) logRecordBase;
            DateTime minValue2 = DateTime.MinValue;
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(logEntryLog.Description);
            LogAlert mostCriticalAlert2 = logEntryLog.AlertList.GetMostCriticalAlert();
            DateTime dateTime3;
            if (mostCriticalAlert2 == null)
            {
              dateTime3 = logEntryLog.Date;
              stringBuilder2.Append(" created on");
            }
            else if (DateTime.MinValue == mostCriticalAlert2.FollowedUpDate)
            {
              dateTime3 = mostCriticalAlert2.DueDate;
              stringBuilder2.Append(" needs follow up on");
            }
            else
            {
              dateTime3 = mostCriticalAlert2.FollowedUpDate;
              stringBuilder2.Append(" followed up on");
            }
            str2 = "Log Entry";
            dateTime1 = dateTime3;
            str1 = stringBuilder2.ToString();
            str3 = logEntryLog.Comments;
            break;
          case StatusOnlineLog _:
            StatusOnlineLog statusOnlineLog = (StatusOnlineLog) logRecordBase;
            dateTime1 = statusOnlineLog.Date;
            str1 = statusOnlineLog.Description;
            str2 = "Status Online Log";
            str3 = statusOnlineLog.Comments;
            break;
          case HtmlEmailLog _:
            HtmlEmailLog htmlEmailLog = (HtmlEmailLog) logRecordBase;
            dateTime1 = htmlEmailLog.Date;
            str1 = htmlEmailLog.Description;
            str2 = "Html Email Log";
            StringBuilder stringBuilder3 = new StringBuilder();
            stringBuilder3.AppendLine("From: " + htmlEmailLog.Sender);
            stringBuilder3.AppendLine("To: " + htmlEmailLog.Recipient);
            stringBuilder3.AppendLine("Subject: " + htmlEmailLog.Subject);
            str3 = stringBuilder3.ToString();
            break;
          case SystemLog _:
            SystemLog systemLog = (SystemLog) logRecordBase;
            dateTime1 = systemLog.Date;
            str1 = systemLog.Description;
            str2 = "System Log";
            str3 = systemLog.Comments;
            break;
          case VerifLog _:
            VerifLog verifLog = (VerifLog) logRecordBase;
            dateTime1 = verifLog.Date;
            str1 = verifLog.Title;
            str2 = "Verification Log";
            str3 = verifLog.Comments.ToString();
            break;
        }
        string val2 = str3;
        string str4 = Utils.StringWrapping(ref val2, 120, 4, 1);
        if (val2 != string.Empty)
          str4 += " ...";
        int num3 = (num2 - 1) * 4;
        int num4 = num3 + 1;
        if (dateTime1 != DateTime.MinValue)
          this.fieldCollect.Set(num4.ToString(), dateTime1.ToString("MM/dd/yyyy"));
        else
          this.fieldCollect.Set(num4.ToString(), "");
        num4 = num3 + 2;
        this.fieldCollect.Set(num4.ToString(), str1);
        num4 = num3 + 3;
        this.fieldCollect.Set(num4.ToString(), str2);
        num4 = num3 + 4;
        this.fieldCollect.Set(num4.ToString(), str4);
      }
    }

    private void printConversationLogEntries()
    {
      if (this.roles == null)
        this.roles = this.sessionObjects.BpmManager.GetAllRoleFunctions();
      int maxLength = 130;
      int maxLine = 60;
      string val1 = this.pageDone;
      this.pageDone = string.Empty;
      int num1 = 0;
      int num2 = 0;
      string str1 = string.Empty;
      if (val1 == string.Empty)
      {
        ConversationLog[] allConversations = this.loan.GetLogList().GetAllConversations();
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < allConversations.Length; ++index)
        {
          ConversationLog conversationLog = allConversations[index];
          if (index > 0)
            stringBuilder.Append("___________________________________________________________________________________________________________________________\r\n");
          stringBuilder.Append("Date:  ");
          if (DateTime.MinValue != conversationLog.Date)
            stringBuilder.Append(conversationLog.Date.ToString("MM/dd/yyyy"));
          stringBuilder.Append("\r\n");
          stringBuilder.Append("Name:  " + conversationLog.Name + "\r\n");
          stringBuilder.Append("Company:  " + conversationLog.Company + "\r\n");
          stringBuilder.Append("Phone:  " + conversationLog.Phone + "\r\n");
          stringBuilder.Append("Email:  " + conversationLog.Email + "\r\n\r\n");
          if (conversationLog.NewComments != string.Empty)
          {
            string str2 = conversationLog.Date.ToString("MM/dd/yy h:mm tt (PST)");
            UserInfo user = this.sessionObjects.OrganizationManager.GetUser(conversationLog.UserId);
            if ((UserInfo) null != user)
              str2 = str2 + " " + user.FullName + "> ";
            string str3 = str2 + conversationLog.NewComments;
            stringBuilder.Append("Comments: \r\n\r\n" + str3 + "\r\n" + conversationLog.Comments + "\r\n\r\n");
          }
          else
            stringBuilder.Append("Comments: \r\n\r\n" + conversationLog.Comments + "\r\n\r\n");
          if (0 < conversationLog.AlertList.Count)
          {
            bool flag = true;
            foreach (LogAlert alert in (CollectionBase) conversationLog.AlertList)
            {
              if (0 < alert.RoleId)
              {
                if (flag)
                {
                  stringBuilder.Append("Alerts: \r\n\r\n");
                  flag = false;
                }
                if (DateTime.MinValue == alert.FollowedUpDate)
                  stringBuilder.Append("Alert " + this.getRoleName(alert.RoleId) + " to follow up on " + alert.DueDate.ToString("MM/dd/yyyy"));
                else
                  stringBuilder.Append(this.getRoleName(alert.RoleId) + " followed up on " + alert.FollowedUpDate.ToString("MM/dd/yyyy"));
                stringBuilder.Append("\r\n");
              }
            }
            stringBuilder.Append("\r\n\r\n");
          }
        }
        string val2 = stringBuilder.ToString();
        num2 = 1;
        while (val2 != string.Empty)
        {
          str1 = Utils.StringWrapping(ref val2, maxLength, maxLine, 1);
          if (val2 != string.Empty)
            ++num2;
        }
        val1 = num1.ToString() + "|" + (object) num2 + "|" + stringBuilder.ToString();
      }
      int length1 = val1.IndexOf("|");
      if (length1 > -1)
      {
        num1 = this.ToInt(val1.Substring(0, length1));
        val1 = val1.Substring(length1 + 1);
      }
      int length2 = val1.IndexOf("|");
      if (length2 > -1)
      {
        num2 = this.ToInt(val1.Substring(0, length2));
        val1 = val1.Substring(length2 + 1);
      }
      int num3 = num1 + 1;
      this.fieldCollect.Set("CONVERSATIONLOG", Utils.StringWrapping(ref val1, maxLength, maxLine, 1));
      this.fieldCollect.Set("PAGENUMBER", "Page " + num3.ToString() + " of " + num2.ToString());
      if (!(val1 != string.Empty))
        return;
      this.pageDone = num3.ToString() + "|" + (object) num2 + "|" + val1;
    }

    private string getRoleName(int roleId)
    {
      if (this.roles != null && this.roles.Length != 0)
      {
        foreach (RoleInfo role in this.roles)
        {
          if (role.RoleID == roleId)
            return role.RoleName;
        }
      }
      return string.Empty;
    }

    private void printFLBroker()
    {
      if (this.loan.GetSimpleField("694") == "0")
        this.fieldCollect.Set("694", "0");
      else
        this.fieldCollect.Set("694", this.loan.GetField("694"));
      if (this.loan.GetSimpleField("696") == "0")
        this.fieldCollect.Set("696", "0");
      else
        this.fieldCollect.Set("696", this.loan.GetField("696"));
    }

    private void printConditionTrackingSummary(bool isPosting)
    {
      string val = this.pageDone.Trim();
      this.pageDone = string.Empty;
      LogList logList = this.loan.GetLogList();
      if (logList == null)
        return;
      ConditionLog[] conditionLogArray = !isPosting ? logList.GetAllConditions(ConditionType.Underwriting) : logList.GetAllConditions(ConditionType.PostClosing);
      if (conditionLogArray == null || conditionLogArray.Length == 0)
        return;
      int num1 = 0;
      if (val != string.Empty)
        num1 = this.ToInt(val);
      int num2 = 0;
      for (int index = num1; index < conditionLogArray.Length; ++index)
      {
        ++num2;
        if (num2 > 29)
        {
          this.pageDone = index.ToString();
          break;
        }
        if (isPosting)
        {
          PostClosingConditionLog closingConditionLog = (PostClosingConditionLog) conditionLogArray[index];
          int num3 = (num2 - 1) * 8;
          int num4 = num3 + 1;
          this.fieldCollect.Set(num4.ToString(), closingConditionLog.Title);
          num4 = num3 + 2;
          this.fieldCollect.Set(num4.ToString(), closingConditionLog.Source);
          num4 = num3 + 3;
          this.fieldCollect.Set(num4.ToString(), StandardConditionLog.GetStatusString(closingConditionLog.Status));
          num4 = num3 + 4;
          if (closingConditionLog.DateAdded != DateTime.MinValue)
            this.fieldCollect.Set(num4.ToString(), closingConditionLog.DateAdded.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num4.ToString(), "");
          num4 = num3 + 5;
          if (closingConditionLog.DateExpected != DateTime.MinValue)
            this.fieldCollect.Set(num4.ToString(), closingConditionLog.DateExpected.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num4.ToString(), "");
          num4 = num3 + 6;
          if (closingConditionLog.DateRequested != DateTime.MinValue)
            this.fieldCollect.Set(num4.ToString(), closingConditionLog.DateRequested.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num4.ToString(), "");
          num4 = num3 + 7;
          if (closingConditionLog.DateReceived != DateTime.MinValue)
            this.fieldCollect.Set(num4.ToString(), closingConditionLog.DateReceived.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num4.ToString(), "");
          num4 = num3 + 8;
          if (closingConditionLog.DateCleared != DateTime.MinValue)
            this.fieldCollect.Set(num4.ToString(), closingConditionLog.DateCleared.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num4.ToString(), "");
        }
        else
        {
          UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) conditionLogArray[index];
          int num5 = (num2 - 1) * 8;
          int num6 = num5 + 1;
          this.fieldCollect.Set(num6.ToString(), LoanConstants.PriorToUIConversion(underwritingConditionLog.PriorTo));
          num6 = num5 + 2;
          this.fieldCollect.Set(num6.ToString(), underwritingConditionLog.Title);
          num6 = num5 + 3;
          RoleInfo roleFunction = this.sessionObjects.BpmManager.GetRoleFunction(underwritingConditionLog.ForRoleID);
          if (roleFunction != null)
            this.fieldCollect.Set(num6.ToString(), roleFunction.RoleName);
          else
            this.fieldCollect.Set(num6.ToString(), underwritingConditionLog.ForRoleID.ToString());
          num6 = num5 + 4;
          this.fieldCollect.Set(num6.ToString(), StandardConditionLog.GetStatusString(underwritingConditionLog.Status));
          num6 = num5 + 5;
          if (underwritingConditionLog.DateAdded != DateTime.MinValue)
            this.fieldCollect.Set(num6.ToString(), underwritingConditionLog.DateAdded.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num6.ToString(), "");
          num6 = num5 + 6;
          if (underwritingConditionLog.DateReceived != DateTime.MinValue)
            this.fieldCollect.Set(num6.ToString(), underwritingConditionLog.DateReceived.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num6.ToString(), "");
          num6 = num5 + 7;
          if (underwritingConditionLog.DateReviewed != DateTime.MinValue)
            this.fieldCollect.Set(num6.ToString(), underwritingConditionLog.DateReviewed.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num6.ToString(), "");
          num6 = num5 + 8;
          if (underwritingConditionLog.DateCleared != DateTime.MinValue)
            this.fieldCollect.Set(num6.ToString(), underwritingConditionLog.DateCleared.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num6.ToString(), "");
        }
      }
    }

    private void printBalancingWorksheet()
    {
      double num1 = 0.0;
      if (this.fieldCollect.Get("1999") == string.Empty)
        this.fieldCollect.Set("1999", DateTime.Today.ToString("MM/dd/yyyy"));
      int num2;
      if (this.pageDone.IndexOf("|") > -1)
      {
        num2 = Utils.ParseInt((object) this.getPageBufferValue(this.pageDone, 0));
        num1 = Utils.ParseDouble((object) this.getPageBufferValue(this.pageDone, 1));
      }
      else
        num2 = Utils.ParseInt((object) this.pageDone.Trim());
      if (num2 == -1)
        num2 = 0;
      this.pageDone = string.Empty;
      double amount = 0.0;
      if (num2 == 0)
      {
        amount = Utils.ParseDouble((object) this.loan.GetField("2"));
        double num3 = Utils.ParseDouble((object) this.loan.GetField("4083"));
        if (num3 > 0.0)
        {
          this.fieldCollect.Set("L_1", "Lender Credits");
          this.fieldCollect.Set("L_2", num3.ToString("N2"));
          this.fieldCollect.Set("L_7", "Total Loan Amount");
          this.fieldCollect.Set("L_8", amount.ToString("N2"));
          this.fieldCollect.Set("TOTAL_DEBITS", (amount + num3).ToString("N2"));
        }
        else
        {
          this.fieldCollect.Set("L_1", "Total Loan Amount");
          this.fieldCollect.Set("L_2", amount.ToString("N2"));
          this.fieldCollect.Set("TOTAL_DEBITS", amount.ToString("N2"));
        }
      }
      int num4 = 0;
      string empty = string.Empty;
      List<FundingFee> fundingFees = this.loan.Use2015RESPA ? this.loan.GetFundingFees(true) : (List<FundingFee>) null;
      Hashtable hashtable = this.loan.Use2015RESPA ? new Hashtable() : (Hashtable) null;
      if (fundingFees != null)
      {
        for (int index = 0; index < fundingFees.Count; ++index)
        {
          if (!hashtable.ContainsKey((object) fundingFees[index].LineID))
            hashtable.Add((object) fundingFees[index].LineID, (object) fundingFees[index]);
        }
      }
      List<GFEItem> gfeItemList = this.loan.GetField("NEWHUD.X354") == "Y" || Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) ? GFEItemCollection.GFEItems2010 : GFEItemCollection.GFEItems;
      int num5;
      for (int index = num2; index < gfeItemList.Count; ++index)
      {
        GFEItem gfeItemObject = gfeItemList[index];
        if ((this.loan.Use2015RESPA || gfeItemObject.LineNumber >= 100 && !(gfeItemObject.For2015 == "Y")) && (!this.loan.Use2015RESPA || !GFEItemCollection.Excluded2015GFEItem.Contains(gfeItemObject.LineNumber)) && this.loan.Calculator.CalcFundingBalancingWorksheet((object) gfeItemObject, ref empty, ref amount))
        {
          ++num4;
          if (num4 > 62)
          {
            this.pageDone = index.ToString() + "|" + num1.ToString();
            return;
          }
          FundingFee fundingFee = (FundingFee) null;
          string key = gfeItemObject.LineNumber.ToString() + gfeItemObject.ComponentID + ".";
          if (gfeItemObject.LineNumber < 700 && hashtable != null && hashtable.ContainsKey((object) key))
          {
            key = ((FundingFee) hashtable[(object) key]).CDLineID;
            fundingFee = (FundingFee) hashtable[(object) key];
          }
          int num6 = (num4 - 1) * 6;
          num5 = num6 + 3;
          this.fieldCollect.Set("L_" + num5.ToString(), key);
          num5 = num6 + 4;
          if (fundingFee != null)
            this.fieldCollect.Set("L_" + num5.ToString(), this.loan.Use2015RESPA ? fundingFee.FeeDescription2015 : fundingFee.FeeDescription);
          else if (gfeItemObject.LineNumber == 802 && gfeItemObject.ComponentID == "e" && this.loan.Use2015RESPA)
            this.fieldCollect.Set("L_" + num5.ToString(), this.loan.GetField("NEWHUD2.X928") + " % of Loan Amount (Points)");
          else if (gfeItemObject.Description.Length <= 4 || gfeItemObject.Description.ToUpper().StartsWith("NEWHUD.X") || gfeItemObject.Description.ToUpper().StartsWith("CD3.X"))
            this.fieldCollect.Set("L_" + num5.ToString(), this.loan.GetField(gfeItemObject.Description));
          else
            this.fieldCollect.Set("L_" + num5.ToString(), gfeItemObject.Description);
          num5 = num6 + 5;
          this.fieldCollect.Set("L_" + num5.ToString(), amount.ToString("N2"));
          num1 += amount;
        }
      }
      amount = Utils.ParseDouble((object) this.loan.GetField("1990"));
      if (amount != 0.0)
      {
        int num7 = num4 + 1;
        if (num7 > 62)
        {
          this.pageDone = gfeItemList.Count.ToString() + "|" + num1.ToString();
          return;
        }
        int num8 = (num7 - 1) * 6;
        num5 = num8 + 3;
        this.fieldCollect.Set("L_" + num5.ToString(), "");
        num5 = num8 + 4;
        this.fieldCollect.Set("L_" + num5.ToString(), "Wire Transfer Amount");
        num5 = num8 + 5;
        this.fieldCollect.Set("L_" + num5.ToString(), amount.ToString("N2"));
        num1 += amount;
      }
      this.fieldCollect.Set("TOTAL_CREDITS", num1.ToString("N2"));
    }

    private string getPageBufferValue(string val, int valueIndex)
    {
      string[] strArray = val.Split('|');
      return valueIndex + 1 > strArray.Length ? string.Empty : strArray[valueIndex].Trim();
    }

    public int PrintFundingWorksheet(bool testOnly, bool isAdditional)
    {
      int num1 = 29;
      if (isAdditional)
        num1 = 65;
      int num2 = Utils.ParseInt((object) this.pageDone.Trim());
      if (!isAdditional && num2 == -1)
        num2 = 0;
      this.pageDone = string.Empty;
      this.fieldCollect.Set("363", DateTime.Today.ToString("MM/dd/yyyy"));
      List<FundingFee> fundingFees = this.loan.Calculator.GetFundingFees(true);
      List<FundingFee> fundingFeeList = new List<FundingFee>();
      for (int index = 0; index < fundingFees.Count; ++index)
      {
        if (fundingFees[index].BalanceChecked)
          fundingFeeList.Add(fundingFees[index]);
      }
      if (fundingFeeList == null || fundingFeeList.Count == 0)
      {
        this.pageDone = "";
        return 0;
      }
      int num3 = 0;
      for (int index = num2; index < fundingFeeList.Count; ++index)
      {
        ++num3;
        if (fundingFeeList[index].PaidBy == "Borrower" && index + 1 < fundingFeeList.Count && num3 + 1 > num1)
        {
          if (isAdditional | testOnly)
            this.pageDone = index.ToString();
          return index;
        }
        if (fundingFeeList[index].PaidBy != "Borrower" && num3 > num1)
        {
          if (isAdditional | testOnly)
            this.pageDone = index.ToString();
          return index;
        }
        if (!testOnly)
        {
          int num4 = (num3 - 1) * 6;
          this.fieldCollect.Set("L_" + (num4 + 1).ToString(), this.loan.Use2015RESPA ? fundingFeeList[index].CDLineID + "." : fundingFeeList[index].LineID);
          this.fieldCollect.Set("L_" + (num4 + 2).ToString(), this.loan.Use2015RESPA ? fundingFeeList[index].FeeDescription2015 : fundingFeeList[index].FeeDescription);
          this.fieldCollect.Set("L_" + (num4 + 3).ToString(), fundingFeeList[index].Payee);
          this.fieldCollect.Set("L_" + (num4 + 4).ToString(), fundingFeeList[index].PaidBy);
          this.fieldCollect.Set("L_" + (num4 + 5).ToString(), fundingFeeList[index].PaidTo);
          this.fieldCollect.Set("L_" + (num4 + 6).ToString(), fundingFeeList[index].Amount.ToString("N2"));
        }
      }
      this.pageDone = "";
      return 0;
    }

    private void printPaymentHistory()
    {
      string val = this.pageDone.Trim();
      this.pageDone = string.Empty;
      this.fieldCollect.Set("363", DateTime.Today.ToString("MM/dd/yyyy"));
      PaymentScheduleSnapshot scheduleSnapshot = this.loan.GetPaymentScheduleSnapshot();
      if (scheduleSnapshot != null)
      {
        this.fieldCollect.Set("2_a", scheduleSnapshot.GetField("2"));
        this.fieldCollect.Set("HUD23", scheduleSnapshot.GetField("ESCROWBEGINBALANCE") != "" ? Utils.ParseDouble((object) scheduleSnapshot.GetField("ESCROWBEGINBALANCE")).ToString("N2") : "");
      }
      ServicingTransactionBase[] servicingTransactions = this.loan.GetServicingTransactions(true);
      if (servicingTransactions == null || servicingTransactions.Length == 0)
        return;
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index < servicingTransactions.Length; ++index)
        hashtable.Add((object) servicingTransactions[index].TransactionGUID, (object) servicingTransactions[index]);
      int num1 = 0;
      if (val != string.Empty)
        num1 = this.ToInt(val);
      int num2 = 0;
      int num3 = 1;
      for (int index = num1; index < servicingTransactions.Length; ++index)
      {
        if (!(servicingTransactions[index] is LoanPurchaseLog))
        {
          ++num2;
          if (num2 > 24)
          {
            this.pageDone = index.ToString();
            break;
          }
          int num4 = (num2 - 1) * 9;
          int num5 = num4 + 1;
          this.fieldCollect.Set(num5.ToString(), num3.ToString());
          ++num3;
          num5 = num4 + 3;
          if (servicingTransactions[index].CreatedDateTime != DateTime.MinValue)
            this.fieldCollect.Set(num5.ToString(), servicingTransactions[index].CreatedDateTime.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num5.ToString(), "");
          string ui = ServicingEnum.TransactionTypesToUI(servicingTransactions[index].TransactionType);
          DateTime dateTime = DateTime.MinValue;
          double num6 = servicingTransactions[index].TransactionAmount;
          double num7 = 0.0;
          double num8 = 0.0;
          double num9 = 0.0;
          double num10 = 0.0;
          string str1 = string.Empty;
          string str2 = string.Empty;
          if (servicingTransactions[index] is PaymentTransactionLog)
          {
            PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) servicingTransactions[index];
            dateTime = paymentTransactionLog.PaymentDueDate;
            num6 = paymentTransactionLog.TotalAmountReceived;
            num7 = paymentTransactionLog.Principal;
            num8 = paymentTransactionLog.Interest;
            num9 = paymentTransactionLog.Escrow;
            num10 = Utils.ArithmeticRounding(paymentTransactionLog.TotalAmountReceived - paymentTransactionLog.Principal - paymentTransactionLog.Interest - paymentTransactionLog.Escrow, 2);
          }
          else if (servicingTransactions[index] is EscrowDisbursementLog)
          {
            EscrowDisbursementLog escrowDisbursementLog = (EscrowDisbursementLog) servicingTransactions[index];
            ui = ServicingEnum.DisbursementTypesToUI(escrowDisbursementLog.DisbursementType);
            dateTime = escrowDisbursementLog.DisbursementDueDate;
            num6 = escrowDisbursementLog.TransactionAmount;
            num9 = escrowDisbursementLog.TransactionAmount;
            str1 = "(";
            str2 = ")";
          }
          else if (servicingTransactions[index] is EscrowInterestLog)
            num6 = servicingTransactions[index].TransactionAmount;
          else if (servicingTransactions[index] is PaymentReversalLog)
          {
            PaymentReversalLog paymentReversalLog = (PaymentReversalLog) servicingTransactions[index];
            if (paymentReversalLog.ReversalType == ServicingTransactionTypes.EscrowDisbursementReversal)
            {
              if (hashtable.ContainsKey((object) paymentReversalLog.PaymentGUID))
              {
                EscrowDisbursementLog escrowDisbursementLog = (EscrowDisbursementLog) hashtable[(object) paymentReversalLog.PaymentGUID];
                if (escrowDisbursementLog != null)
                {
                  ui = ServicingEnum.DisbursementTypesToUI(escrowDisbursementLog.DisbursementType);
                  dateTime = escrowDisbursementLog.DisbursementDueDate;
                  num6 = escrowDisbursementLog.TransactionAmount;
                  num9 = escrowDisbursementLog.TransactionAmount;
                }
              }
            }
            else
            {
              if (hashtable.ContainsKey((object) paymentReversalLog.PaymentGUID))
              {
                PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) hashtable[(object) paymentReversalLog.PaymentGUID];
                if (paymentTransactionLog != null)
                {
                  dateTime = paymentTransactionLog.PaymentDueDate;
                  num6 = paymentTransactionLog.TotalAmountReceived;
                  num7 = paymentTransactionLog.Principal;
                  num8 = paymentTransactionLog.Interest;
                  num9 = paymentTransactionLog.Escrow;
                  num10 = Utils.ArithmeticRounding(paymentTransactionLog.TotalAmountReceived - paymentTransactionLog.Principal - paymentTransactionLog.Interest - paymentTransactionLog.Escrow, 2);
                }
              }
              str1 = "(";
              str2 = ")";
            }
          }
          else if (servicingTransactions[index] is LoanPurchaseLog)
            continue;
          num5 = num4 + 2;
          this.fieldCollect.Set(num5.ToString(), ui);
          num5 = num4 + 4;
          if (dateTime != DateTime.MinValue)
            this.fieldCollect.Set(num5.ToString(), dateTime.ToString("MM/dd/yyyy"));
          else
            this.fieldCollect.Set(num5.ToString(), "");
          num5 = num4 + 5;
          if (num6 != 0.0)
            this.fieldCollect.Set(num5.ToString(), str1 + num6.ToString("N2") + str2);
          else
            this.fieldCollect.Set(num5.ToString(), "");
          num5 = num4 + 6;
          if (num7 != 0.0)
            this.fieldCollect.Set(num5.ToString(), str1 + num7.ToString("N2") + str2);
          else
            this.fieldCollect.Set(num5.ToString(), "");
          num5 = num4 + 7;
          if (num8 != 0.0)
            this.fieldCollect.Set(num5.ToString(), str1 + num8.ToString("N2") + str2);
          else
            this.fieldCollect.Set(num5.ToString(), "");
          num5 = num4 + 8;
          if (num9 != 0.0)
            this.fieldCollect.Set(num5.ToString(), str1 + num9.ToString("N2") + str2);
          else
            this.fieldCollect.Set(num5.ToString(), "");
          num5 = num4 + 9;
          if (num10 != 0.0)
            this.fieldCollect.Set(num5.ToString(), str1 + num10.ToString("N2") + str2);
          else
            this.fieldCollect.Set(num5.ToString(), "");
        }
      }
    }

    private void printDocumentTrackingSummary()
    {
      string val = this.pageDone.Trim();
      this.pageDone = string.Empty;
      LogList logList = this.loan.GetLogList();
      if (logList == null)
        return;
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      if (allDocuments == null || allDocuments.Length == 0)
        return;
      int num1 = 0;
      if (val != string.Empty)
        num1 = this.ToInt(val);
      int num2 = 0;
      for (int index = num1; index < allDocuments.Length; ++index)
      {
        ++num2;
        if (num2 > 29)
        {
          this.pageDone = index.ToString();
          break;
        }
        DocumentLog documentLog = allDocuments[index];
        int num3 = (num2 - 1) * 8;
        int num4 = num3 + 1;
        this.fieldCollect.Set(num4.ToString(), this.getBorrowerIndex(this.loan.GetPairIndex(documentLog.PairId)));
        num4 = num3 + 2;
        this.fieldCollect.Set(num4.ToString(), documentLog.Title + " - " + documentLog.RequestedFrom);
        num4 = num3 + 3;
        if (documentLog.Stage == string.Empty || documentLog.Stage == null)
        {
          this.fieldCollect.Set(num4.ToString(), "");
        }
        else
        {
          string stage = documentLog.Stage;
          this.fieldCollect.Set(num4.ToString(), stage);
        }
        num4 = num3 + 4;
        if (documentLog.Rerequested)
          this.fieldCollect.Set(num4.ToString(), documentLog.DateRequested.ToString("MM/dd/yyyy"));
        else if (documentLog.Requested)
          this.fieldCollect.Set(num4.ToString(), documentLog.DateRequested.ToString("MM/dd/yyyy"));
        else
          this.fieldCollect.Set(num4.ToString(), "");
        num4 = num3 + 5;
        if (documentLog.Expected)
          this.fieldCollect.Set(num4.ToString(), documentLog.DateExpected.ToString("MM/dd/yyyy"));
        else
          this.fieldCollect.Set(num4.ToString(), "");
        num4 = num3 + 6;
        if (documentLog.Received)
          this.fieldCollect.Set(num4.ToString(), documentLog.DateReceived.ToString("MM/dd/yyyy"));
        else
          this.fieldCollect.Set(num4.ToString(), "");
        num4 = num3 + 7;
        if (documentLog.Expires)
          this.fieldCollect.Set(num4.ToString(), documentLog.DateExpires.ToString("MM/dd/yyyy"));
        else
          this.fieldCollect.Set(num4.ToString(), "");
      }
    }

    private string getBorrowerIndex(int i)
    {
      string borrowerIndex;
      switch (i)
      {
        case 0:
          borrowerIndex = "1st";
          break;
        case 1:
          borrowerIndex = "2nd";
          break;
        case 2:
          borrowerIndex = "3rd";
          break;
        default:
          borrowerIndex = (i + 1).ToString() + "th";
          break;
      }
      return borrowerIndex;
    }

    private void printDebtConsolidation()
    {
      string val = this.pageDone.Trim();
      this.pageDone = string.Empty;
      int num1 = 1;
      if (val != string.Empty && val != null)
        num1 = this.ToInt(val);
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int num2 = 0;
      for (int index = num1; index <= exlcudingAlimonyJobExp; ++index)
      {
        ++num2;
        if (num2 > 28)
        {
          this.pageDone = index.ToString();
          break;
        }
        string str = "FL" + num2.ToString("00");
        this.fieldCollect.Set(str + "02", this.loan.GetField("FL" + index.ToString("00") + "02"));
        string field = this.loan.GetField("FL" + index.ToString("00") + "08");
        this.fieldCollect.Set(str + "08", field);
        double num3 = this.ToDouble(this.loan.GetField("FL" + index.ToString("00") + "11"));
        this.fieldCollect.Set(str + "11", num3.ToString("N2"));
        this.fieldCollect.Set(str + "13", this.loan.GetField("FL" + index.ToString("00") + "13"));
        int num4 = this.ToInt(this.loan.GetField("FL" + index.ToString("00") + "12"));
        this.fieldCollect.Set(str + "12", num4.ToString());
        double num5 = num3 * (double) num4;
        this.fieldCollect.Set(str + "11_" + str + "12", num5.ToString("N2"));
        if (this.loan.GetField("FL" + index.ToString("00") + "18") == "Y")
          this.fieldCollect.Set(str + "18", "Yes");
      }
    }

    private double calcLTVRatio(bool getLTV, int option)
    {
      double num1 = this.ToDouble(this.loan.GetField("356"));
      double num2 = option != 1 ? this.ToDouble(this.loan.GetField("PREQUAL.X73")) : this.ToDouble(this.loan.GetField("PREQUAL.X33"));
      string empty = string.Empty;
      string str = option != 1 ? this.loan.GetField("LP0205") : this.loan.GetField("LP0105");
      double num3 = num1 <= num2 || str.IndexOf("Refinance") >= 0 ? (num1 <= 0.0 ? num2 : num1) : num2;
      if (num3 <= 0.0)
        return 0.0;
      double num4 = option != 1 ? this.ToDouble(this.loan.GetField("PREQUAL.X74")) / num3 * 100.0 : this.ToDouble(this.loan.GetField("PREQUAL.X34")) / num3 * 100.0;
      if (getLTV)
        return num4;
      double num5 = this.ToDouble(this.loan.GetField("427")) + this.ToDouble(this.loan.GetField("428"));
      if (this.loan.GetField("420") != "SecondLien")
        num5 += this.ToDouble(this.loan.GetField("1732"));
      return num5 / num3 * 100.0;
    }

    private void printLoanCommitment1()
    {
      this.pageDone = string.Empty;
      if (Utils.ParseDate((object) this.loan.GetSimpleField("3094")) == DateTime.MinValue)
        this.fieldCollect.Set("3094", DateTime.Today.ToString("MM/dd/yyyy"));
      string simpleField = this.loan.GetSimpleField("UWC.NOTCLEARED");
      this.fieldCollect.Set("Open_Cond", Utils.StringWrapping(ref simpleField, 110, 19, 1));
      int page = 0;
      while (simpleField.Trim() != string.Empty)
      {
        ++page;
        Utils.StringWrapping(ref simpleField, 110, 73, page);
      }
      this.fieldCollect.Set("PAGE_NUM", "Page 1 of " + (page + 2).ToString());
    }

    private void printLoanCommitment2()
    {
      if (Utils.ParseDate((object) this.loan.GetSimpleField("3094")) == DateTime.MinValue)
        this.fieldCollect.Set("3094", DateTime.Today.ToString("MM/dd/yyyy"));
      int page1 = 0;
      int num1 = 0;
      if (this.pageDone != "" && this.pageDone != null)
      {
        string[] strArray = this.pageDone.Split('|');
        page1 = this.ToInt(strArray[0]);
        num1 = this.ToInt(strArray[1]);
      }
      this.pageDone = string.Empty;
      string simpleField = this.loan.GetSimpleField("UWC.NOTCLEARED");
      string str = Utils.StringWrapping(ref simpleField, 110, 19, 1);
      if (page1 == 0)
      {
        string val = simpleField;
        while (val.Trim() != string.Empty)
        {
          ++num1;
          str = Utils.StringWrapping(ref val, 110, 73, page1);
        }
        num1 += 2;
      }
      int page2 = page1 + 1;
      this.fieldCollect.Set("Open_Cond", Utils.StringWrapping(ref simpleField, 110, 73, page2));
      int num2 = page2 + 1;
      this.fieldCollect.Set("PAGE_NUM", "Page " + num2.ToString() + " of " + num1.ToString());
      int num3 = num2 - 1;
      if (!(simpleField.Trim() != string.Empty))
        return;
      this.pageDone = num3.ToString() + "|" + num1.ToString();
    }

    private void printLoanCommitment3()
    {
      if (Utils.ParseDate((object) this.loan.GetSimpleField("3094")) == DateTime.MinValue)
        this.fieldCollect.Set("3094", DateTime.Today.ToString("MM/dd/yyyy"));
      this.pageDone = string.Empty;
      string simpleField = this.loan.GetSimpleField("UWC.NOTCLEARED");
      string str = Utils.StringWrapping(ref simpleField, 110, 19, 1);
      int page = 0;
      while (simpleField.Trim() != string.Empty)
      {
        ++page;
        str = Utils.StringWrapping(ref simpleField, 110, 73, page);
      }
      int num = page + 2;
      this.fieldCollect.Set("PAGE_NUM", "Page " + num.ToString() + " of " + num.ToString());
    }

    private void printMLDSPage2()
    {
      double num1 = this.ToDouble(this.loan.GetField("RE88395.X118"));
      if (num1 < 0.0)
      {
        this.fieldCollect.Set("RE88395X117_ToYou", "X");
        this.fieldCollect.Set("RE88395X118", (num1 * -1.0).ToString("N2"));
      }
      else if (num1 > 0.0)
      {
        this.fieldCollect.Set("RE88395X117_ThatYouPay", "X");
        this.fieldCollect.Set("RE88395X118", num1.ToString("N2"));
      }
      string field1 = this.loan.GetField("RE88395.X317");
      this.fieldCollect.Set("RE88395X317_a", Utils.StringWrapping(ref field1, 120, 1, 1));
      this.fieldCollect.Set("RE88395X317_b", Utils.StringWrapping(ref field1, 120, 1, 1));
      if (!Utils.ParseBoolean((object) this.loan.GetField("RE88395.X123")))
      {
        double num2 = Utils.ParseDouble((object) this.loan.GetField("RE88395.X316")) / 12.0;
        if (num2 % 12.0 == 0.0)
        {
          if (Utils.ParseBoolean((object) this.loan.GetField("RE88395.X322")))
          {
            int num3;
            if (num2 != 0.0)
            {
              NameValueCollection fieldCollect = this.fieldCollect;
              num3 = Utils.ParseInt((object) num2);
              string str = num3.ToString("0");
              fieldCollect.Set("RE88395X316", str);
            }
            if (num2 != 0.0)
            {
              NameValueCollection fieldCollect = this.fieldCollect;
              num3 = Utils.ParseInt((object) num2);
              string str = num3.ToString("0");
              fieldCollect.Set("RE88395X316_b", str);
            }
          }
          else if (num2 != 0.0)
            this.fieldCollect.Set("RE88395X316_c", Utils.ParseInt((object) num2).ToString("0"));
        }
        else if (Utils.ParseBoolean((object) this.loan.GetField("RE88395.X322")))
        {
          if (num2 != 0.0)
            this.fieldCollect.Set("RE88395X316", num2.ToString());
          if (num2 != 0.0)
            this.fieldCollect.Set("RE88395X316_b", num2.ToString());
        }
        else if (num2 != 0.0)
          this.fieldCollect.Set("RE88395X316_c", num2.ToString());
      }
      this.fieldCollect.Set("TERM_LEFT", (Utils.ParseInt((object) this.loan.GetField("4")) - Utils.ParseInt((object) this.loan.GetField("RE88395.X327"))).ToString());
      string field2 = this.loan.GetField("RE88395.X326");
      this.fieldCollect.Set("5_b", "");
      this.fieldCollect.Set("RE88395X327_b", "");
      this.fieldCollect.Set("5_c", "");
      this.fieldCollect.Set("RE88395X327_c", "");
      this.fieldCollect.Set("696_d", "");
      this.fieldCollect.Set("696_e", "");
      this.fieldCollect.Set("RE88395X327_d", "");
      switch (field2)
      {
        case "Initial Fixed Rate Loan":
          this.fieldCollect.Set("5_b", this.loan.GetField("5"));
          this.fieldCollect.Set("RE88395X327_b", this.loan.GetField("RE88395.X327"));
          break;
        case "Adjustable Rate Loan":
          this.fieldCollect.Set("5_c", this.loan.GetField("5"));
          this.fieldCollect.Set("RE88395X327_c", this.loan.GetField("RE88395.X327"));
          break;
        case "Initial Adjustable Rate Loan":
          this.fieldCollect.Set("696_d", this.loan.GetField("696"));
          this.fieldCollect.Set("696_e", this.loan.GetField("696"));
          this.fieldCollect.Set("RE88395X327_d", this.loan.GetField("RE88395.X327"));
          break;
      }
      if (Utils.ParseDouble((object) this.loan.GetField("HUD24")) <= 0.0)
        return;
      if (Utils.ParseDouble((object) this.loan.GetField("231")) > 0.0 && this.loan.GetField("HUD0141").Length == 10)
        this.fieldCollect.Set("231", "X");
      if (Utils.ParseDouble((object) this.loan.GetField("230")) > 0.0 && this.loan.GetField("HUD0142").Length == 10)
        this.fieldCollect.Set("230", "X");
      if (Utils.ParseDouble((object) this.loan.GetField("232")) > 0.0 && this.loan.GetField("HUD0143").Length == 10)
        this.fieldCollect.Set("232", "X");
      if (Utils.ParseDouble((object) this.loan.GetField("235")) > 0.0 && this.loan.GetField("HUD0144").Length == 10)
        this.fieldCollect.Set("235", "X");
      if ((Utils.ParseDouble((object) this.loan.GetField("L268")) <= 0.0 || this.loan.GetField("HUD0145").Length != 10) && (Utils.ParseDouble((object) this.loan.GetField("1630")) <= 0.0 || this.loan.GetField("HUD0146").Length != 10) && (Utils.ParseDouble((object) this.loan.GetField("253")) <= 0.0 || this.loan.GetField("HUD0147").Length != 10) && (Utils.ParseDouble((object) this.loan.GetField("254")) <= 0.0 || this.loan.GetField("HUD0148").Length != 10))
        return;
      this.fieldCollect.Set("234", "X");
    }

    private void printMLDS885Page2()
    {
      double num1 = this.ToDouble(this.loan.GetField("RE88395.X118"));
      if (num1 < 0.0)
      {
        this.fieldCollect.Set("RE88395X117_ToYou", "X");
        this.fieldCollect.Set("RE88395X118", (num1 * -1.0).ToString("N2"));
      }
      else if (num1 > 0.0)
      {
        this.fieldCollect.Set("RE88395X117_ThatYouPay", "X");
        this.fieldCollect.Set("RE88395X118", num1.ToString("N2"));
      }
      string field1 = this.loan.GetField("RE88395.X317");
      this.fieldCollect.Set("RE88395X317_a", Utils.StringWrapping(ref field1, 120, 1, 1));
      this.fieldCollect.Set("RE88395X317_b", Utils.StringWrapping(ref field1, 120, 1, 1));
      if (this.Val("RE88395.X123") != "Y")
      {
        int num2 = this.IntVal("RE88395.X316");
        if (num2 > 0)
        {
          int num3 = num2 % 12 != 0 ? (int) ((double) num2 / 12.0) + 1 : num2 / 12;
          if (this.Val("RE88395.X322") == "Y")
            this.fieldCollect.Set("RE88395X316", num3.ToString("0"));
          if (this.Val("RE88395.X191") == "Y")
            this.fieldCollect.Set("RE88395X316_b", num3.ToString("0"));
          if (this.Val("RE88395.X124") == "Y")
            this.fieldCollect.Set("RE88395X316_c", num3.ToString("0"));
        }
      }
      this.fieldCollect.Set("TERM_LEFT", (Utils.ParseInt((object) this.loan.GetField("4")) - Utils.ParseInt((object) this.loan.GetField("RE88395.X327"))).ToString());
      string field2 = this.loan.GetField("RE88395.X326");
      this.fieldCollect.Set("5_b", "");
      this.fieldCollect.Set("RE88395X327_b", "");
      this.fieldCollect.Set("5_c", "");
      this.fieldCollect.Set("RE88395X327_c", "");
      this.fieldCollect.Set("696_d", "");
      this.fieldCollect.Set("696_e", "");
      this.fieldCollect.Set("RE88395X327_d", "");
      switch (field2)
      {
        case "Initial Fixed Rate Loan":
          this.fieldCollect.Set("5_b", this.loan.GetField("5"));
          this.fieldCollect.Set("RE88395X327_b", this.loan.GetField("RE88395.X327"));
          break;
        case "Adjustable Rate Loan":
          this.fieldCollect.Set("5_c", this.loan.GetField("5"));
          this.fieldCollect.Set("RE88395X327_c", this.loan.GetField("RE88395.X327"));
          break;
        case "Initial Adjustable Rate Loan":
          this.fieldCollect.Set("696_d", this.loan.GetField("696"));
          this.fieldCollect.Set("696_e", this.loan.GetField("696"));
          this.fieldCollect.Set("RE88395X327_d", this.loan.GetField("RE88395.X327"));
          break;
      }
      this.fieldCollect.Set("1712", this.loan.GetField("1712"));
      this.fieldCollect.Set("RE88395X325", this.loan.GetField("RE88395.X325"));
      double num4 = this.ToDouble(this.loan.GetField("1109"));
      this.fieldCollect.Set("698_1109", string.Concat((object) (this.ToDouble(this.loan.GetField("698")) / 100.0 * num4)));
      if (Utils.ParseDouble((object) this.loan.GetField("HUD24")) <= 0.0)
        return;
      if (Utils.ParseDouble((object) this.loan.GetField("231")) > 0.0 && this.loan.GetField("HUD0141").Length == 10)
        this.fieldCollect.Set("231", "X");
      if (Utils.ParseDouble((object) this.loan.GetField("230")) > 0.0 && this.loan.GetField("HUD0142").Length == 10)
        this.fieldCollect.Set("230", "X");
      if (Utils.ParseDouble((object) this.loan.GetField("232")) > 0.0 && this.loan.GetField("HUD0143").Length == 10)
        this.fieldCollect.Set("232", "X");
      if (Utils.ParseDouble((object) this.loan.GetField("235")) > 0.0 && this.loan.GetField("HUD0144").Length == 10)
        this.fieldCollect.Set("235", "X");
      if ((Utils.ParseDouble((object) this.loan.GetField("L268")) <= 0.0 || this.loan.GetField("HUD0145").Length != 10) && (Utils.ParseDouble((object) this.loan.GetField("1630")) <= 0.0 || this.loan.GetField("HUD0146").Length != 10) && (Utils.ParseDouble((object) this.loan.GetField("253")) <= 0.0 || this.loan.GetField("HUD0147").Length != 10) && (Utils.ParseDouble((object) this.loan.GetField("254")) <= 0.0 || this.loan.GetField("HUD0148").Length != 10))
        return;
      this.fieldCollect.Set("234", "X");
    }

    private void printMLDSPage3()
    {
      switch (this.loan.GetField("RE88395.X149").ToUpper())
      {
        case "MAY":
          this.fieldCollect.Set("RE88395X149_May", "X");
          break;
        case "WILL":
          this.fieldCollect.Set("RE88395X149_Will", "X");
          break;
        case "WILLNOT":
        case "WILL NOT":
          this.fieldCollect.Set("RE88395X149_Willnot", "X");
          break;
      }
    }

    private void printAmortSummary()
    {
      this.pageDone = string.Empty;
      PaymentSchedule[] schedule = this.calObjs.RegzCal.Schedule;
      int numberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index = 0; index < numberOfTerm; ++index)
      {
        num1 += schedule[index].Payment;
        num2 += schedule[index].Interest;
      }
      this.fieldCollect.Set("1206", num2.ToString("#,0.00"));
      this.fieldCollect.Set("1207", num1.ToString("#,0.00"));
    }

    private void printAmortYearly()
    {
      string pageDone = this.pageDone;
      this.pageDone = string.Empty;
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = 0.0;
      string str1 = string.Empty;
      int num6 = 0;
      int num7 = 0;
      if (pageDone != string.Empty)
      {
        string[] strArray = pageDone.Split('|');
        num6 = this.ToInt(strArray[0]);
        str1 = strArray[1];
        num1 = this.ToDouble(strArray[2]);
        num2 = this.ToDouble(strArray[3]);
        num3 = this.ToDouble(strArray[4]);
        num4 = this.ToDouble(strArray[5]);
        num5 = this.ToDouble(strArray[6]);
        num7 = this.ToInt(strArray[7]);
      }
      int num8 = 12;
      if (this.loan.GetField("423") == "Biweekly")
        num8 = 26;
      PaymentSchedule[] schedule = this.calObjs.RegzCal.Schedule;
      int numberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      int num9 = 0;
      string str2 = "YRAMORT_";
      for (int index1 = num6; index1 < numberOfTerm; ++index1)
      {
        int num10 = index1 + 1;
        if (num10 % num8 == 1)
        {
          str1 = schedule[index1].PayDate;
          num1 = schedule[index1].CurrentRate;
          num2 = schedule[index1].Payment;
          num3 = schedule[index1].Principal;
          num4 = schedule[index1].Interest;
          num5 = schedule[index1].MortgageInsurance;
        }
        else if (num10 % num8 != 0 || index1 != num6 || num6 == 0)
        {
          num2 += schedule[index1].Payment;
          num3 += schedule[index1].Principal;
          num4 += schedule[index1].Interest;
          num5 += schedule[index1].MortgageInsurance;
        }
        if (num10 % num8 == 0)
        {
          ++num9;
          if (num9 > 18)
          {
            this.pageDone = index1.ToString() + "|" + str1 + "|" + num1.ToString() + "|" + num2.ToString() + "|" + num3.ToString() + "|" + num4.ToString() + "|" + num5.ToString() + "|" + num7.ToString();
            break;
          }
          int num11 = (num9 - 1) * 21;
          ++num7;
          int num12 = num11 + 1;
          this.fieldCollect.Set(str2 + num12.ToString(), num10.ToString());
          num12 = num11 + 3;
          this.fieldCollect.Set(str2 + num12.ToString(), schedule[index1].Interest.ToString("N2"));
          num12 = num11 + 4;
          this.fieldCollect.Set(str2 + num12.ToString(), schedule[index1].Principal.ToString("N2"));
          num12 = num11 + 5;
          this.fieldCollect.Set(str2 + num12.ToString(), schedule[index1].MortgageInsurance.ToString("#,0.00"));
          num12 = num11 + 6;
          this.fieldCollect.Set(str2 + num12.ToString(), schedule[index1].Payment.ToString("#,0.00"));
          this.fieldCollect.Set("AMORTDATE_" + ((num9 - 1) * 4 + 1).ToString(), schedule[index1].PayDate);
          for (int index2 = 10; index2 <= 13; ++index2)
          {
            num12 = num11 + index2;
            this.fieldCollect.Set(str2 + num12.ToString(), "==========");
          }
          num12 = num11 + 9;
          this.fieldCollect.Set(str2 + num12.ToString(), "");
          num12 = num11 + 16;
          this.fieldCollect.Set(str2 + num12.ToString(), "");
          num12 = num11 + 14;
          this.fieldCollect.Set(str2 + num12.ToString(), "");
          num12 = num11 + 21;
          this.fieldCollect.Set(str2 + num12.ToString(), "");
          num12 = num11 + 8;
          this.fieldCollect.Set(str2 + num12.ToString(), "Yearly");
          num12 = num11 + 15;
          this.fieldCollect.Set(str2 + num12.ToString(), "Totals");
          num12 = num11 + 2;
          this.fieldCollect.Set(str2 + num12.ToString(), num7.ToString());
          num12 = num11 + 17;
          this.fieldCollect.Set(str2 + num12.ToString(), num4.ToString("N2"));
          num12 = num11 + 18;
          this.fieldCollect.Set(str2 + num12.ToString(), num3.ToString("N2"));
          num12 = num11 + 19;
          this.fieldCollect.Set(str2 + num12.ToString(), num5.ToString("#,0.00"));
          num12 = num11 + 20;
          this.fieldCollect.Set(str2 + num12.ToString(), num2.ToString("#,0.00"));
          num12 = num11 + 21;
          this.fieldCollect.Set(str2 + num12.ToString(), schedule[index1].Balance.ToString("#,0.00"));
        }
      }
    }

    private void printAmortMonthly()
    {
      int num1 = this.ToInt(this.pageDone);
      this.pageDone = string.Empty;
      PaymentSchedule[] schedule = this.calObjs.RegzCal.Schedule;
      int numberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      int num2 = 0;
      string str = "MOAMORT_";
      int num3 = 12;
      if (this.loan.GetField("423") == "Biweekly")
        num3 = 26;
      for (int index1 = num1; index1 < numberOfTerm; ++index1)
      {
        ++num2;
        if (num2 > 70)
        {
          this.pageDone = index1.ToString();
          break;
        }
        int num4 = (num2 - 1) * 7;
        int num5 = index1 + 1;
        int num6;
        if (num5 % num3 == 1 && num5 != 1)
        {
          for (int index2 = 1; index2 <= 7; ++index2)
          {
            num6 = num4 + index2;
            this.fieldCollect.Set(str + num6.ToString(), "");
          }
          ++num2;
          num4 = (num2 - 1) * 7;
          if (num2 > 70)
          {
            this.pageDone = index1.ToString();
            break;
          }
        }
        int num7 = (index1 - index1 % num3) / num3 + 1;
        this.fieldCollect.Set("AMORTNO_" + num2.ToString(), num5.ToString());
        num6 = num4 + 1;
        this.fieldCollect.Set(str + num6.ToString(), schedule[index1].PayDate);
        num6 = num4 + 2;
        this.fieldCollect.Set(str + num6.ToString(), num7.ToString());
        num6 = num4 + 3;
        this.fieldCollect.Set(str + num6.ToString(), schedule[index1].Interest.ToString("#,0.00"));
        num6 = num4 + 4;
        this.fieldCollect.Set(str + num6.ToString(), schedule[index1].Principal.ToString("#,0.00"));
        num6 = num4 + 5;
        this.fieldCollect.Set(str + num6.ToString(), schedule[index1].MortgageInsurance.ToString("#,0.00"));
        num6 = num4 + 6;
        this.fieldCollect.Set(str + num6.ToString(), schedule[index1].Payment.ToString("#,0.00"));
        num6 = num4 + 7;
        this.fieldCollect.Set(str + num6.ToString(), schedule[index1].Balance.ToString("#,0.00"));
      }
    }

    private string convertVOMPropertyType(string originalType)
    {
      switch (originalType)
      {
        case "SINGLEFAMILY":
          return "SFR";
        case "CONDOMINIUM":
          return "CONDO";
        case "TOWNHOUSE":
          return "TOWN";
        case "COOPERATIVE":
          return "COOP";
        case "TWOTOFOURUNITPROPERTY":
          return "2-4";
        case "MULTIFAMILYMORETHANFOURUNITS":
          return "MULTI";
        case "MANUFACTUREDMOBILEHOME":
          return "MOBIL";
        case "COMMERCIALNONRESIDENTIAL":
          return "COMNR";
        case "MIXEDUSERESIDENTIAL":
          return "MIXED";
        case "FARM":
          return "FARM";
        case "HOMEANDBUSINESSCOMBINED":
          return "COMR";
        case "LAND":
          return "LAND";
        default:
          return originalType;
      }
    }

    private void printSOR(int maxLine)
    {
      int num1 = this.ToInt(this.pageDone);
      if (num1 == 0)
        num1 = 1;
      this.pageDone = string.Empty;
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int num2 = 0;
      string str1 = "VOM_";
      string str2 = string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      int num3 = 40;
      double[] numArray = new double[12];
      int num4;
      for (int index1 = num1; index1 <= numberOfMortgages; ++index1)
      {
        string str3 = "FM" + index1.ToString("00");
        ++num2;
        if (num2 > maxLine)
        {
          this.pageDone = index1.ToString();
          break;
        }
        string field1 = this.loan.GetField(str3 + "04");
        string str4 = this.loan.GetField(str3 + "06").Trim();
        string str5;
        if (str4 == string.Empty)
          str5 = this.loan.GetField(str3 + "07") + " " + this.loan.GetField(str3 + "08");
        else
          str5 = str4 + ", " + this.loan.GetField(str3 + "07") + " " + this.loan.GetField(str3 + "08");
        num4 = (num2 - 1) * num3 + 1;
        this.fieldCollect.Set(str1 + num4.ToString(), field1);
        num4 = (num2 - 1) * num3 + 2;
        this.fieldCollect.Set(str1 + num4.ToString(), str5);
        string str6 = string.Empty;
        switch (this.loan.GetField(str3 + "24"))
        {
          case "PendingSale":
            str6 = "Pending Sale";
            break;
          case "Sold":
            str6 = "Sold";
            break;
          case "RetainForRental":
            str6 = "Retain For Rental";
            break;
        }
        num4 = (num2 - 1) * num3 + 4;
        this.fieldCollect.Set(str1 + num4.ToString(), str6);
        num4 = (num2 - 1) * num3 + 5;
        string field2 = this.loan.GetField(str3 + "14");
        this.fieldCollect.Set(str1 + num4.ToString(), field2);
        double num5 = this.ToDouble(field2) / 100.0;
        if (num5 == 0.0)
          num5 = 1.0;
        num4 = (num2 - 1) * num3 + 6;
        string field3 = this.loan.GetField(str3 + "22");
        this.fieldCollect.Set(str1 + num4.ToString(), field3);
        num4 = (num2 - 1) * num3 + 7;
        string field4 = this.loan.GetField(str3 + "23");
        this.fieldCollect.Set(str1 + num4.ToString(), field4);
        num4 = (num2 - 1) * num3 + 8;
        string str7 = this.loan.GetField(str3 + "18").ToUpper();
        switch (str7)
        {
          case "SINGLEFAMILY":
            str7 = "Single Family";
            break;
          case "CONDOMINIUM":
            str7 = "Condominium";
            break;
          case "TOWNHOUSE":
            str7 = "Town House";
            break;
          case "COOPERATIVE":
            str7 = "CO-Operative";
            break;
          case "TWOTOFOURUNITPROPERTY":
            str7 = "2-4 Unit Property";
            break;
          case "MULTIFAMILYMORETHANFOURUNITS":
            str7 = "Multi Family More Than 4 Units";
            break;
          case "MANUFACTUREDMOBILEHOME":
            str7 = "Manufactured Mobile Home";
            break;
          case "COMMERCIALNONRESIDENTIAL":
            str7 = "Commercial Non-residential";
            break;
          case "MIXEDUSERESIDENTIAL":
            str7 = "Mixed Use Residential";
            break;
          case "FARM":
            str7 = "FARM";
            break;
          case "HOMEANDBUSINESSCOMBINED":
            str7 = "Home And Business Combined";
            break;
          case "LAND":
            str7 = "Land";
            break;
        }
        this.fieldCollect.Set(str1 + num4.ToString(), str7);
        num4 = (num2 - 1) * num3 + 9;
        this.fieldCollect.Set(str1 + num4.ToString(), this.loan.GetField(str3 + "25"));
        num4 = (num2 - 1) * num3 + 10;
        string field5 = this.loan.GetField(str3 + "19");
        this.fieldCollect.Set(str1 + num4.ToString(), field5);
        double num6 = this.ToDouble(field5) * num5;
        numArray[0] += num6;
        if (num5 != 1.0)
        {
          num4 = (num2 - 1) * num3 + 16;
          this.fieldCollect.Set(str1 + num4.ToString(), num6.ToString("N2"));
        }
        num4 = (num2 - 1) * num3 + 11;
        string field6 = this.loan.GetField(str3 + "17");
        this.fieldCollect.Set(str1 + num4.ToString(), field6);
        double num7 = this.ToDouble(field6) * num5;
        numArray[1] += num7;
        if (num5 != 1.0)
        {
          num4 = (num2 - 1) * num3 + 17;
          this.fieldCollect.Set(str1 + num4.ToString(), num7.ToString("N2"));
        }
        num4 = (num2 - 1) * num3 + 12;
        string field7 = this.loan.GetField(str3 + "20");
        this.fieldCollect.Set(str1 + num4.ToString(), field7);
        double num8 = this.ToDouble(field7) * num5;
        numArray[2] += num8;
        if (num5 != 1.0)
        {
          num4 = (num2 - 1) * num3 + 18;
          this.fieldCollect.Set(str1 + num4.ToString(), num8.ToString("N2"));
        }
        num4 = (num2 - 1) * num3 + 13;
        string field8 = this.loan.GetField(str3 + "16");
        this.fieldCollect.Set(str1 + num4.ToString(), field8);
        double num9 = this.ToDouble(field8) * num5;
        numArray[3] += num9;
        if (num5 != 1.0)
        {
          num4 = (num2 - 1) * num3 + 19;
          this.fieldCollect.Set(str1 + num4.ToString(), num9.ToString("N2"));
        }
        num4 = (num2 - 1) * num3 + 14;
        string val = this.ToDouble(this.loan.GetField(str3 + "21")).ToString("N2");
        this.fieldCollect.Set(str1 + num4.ToString(), val);
        double num10 = this.ToDouble(val) * num5;
        numArray[4] += num10;
        if (num5 != 1.0)
        {
          num4 = (num2 - 1) * num3 + 20;
          this.fieldCollect.Set(str1 + num4.ToString(), num10.ToString("N2"));
        }
        string field9 = this.loan.GetField(str3 + "32");
        numArray[5] += this.ToDouble(field9);
        if (num5 != 1.0)
        {
          num4 = (num2 - 1) * num3 + 21;
          this.fieldCollect.Set(str1 + num4.ToString(), this.ToDouble(field9).ToString("N2"));
        }
        string str8 = (this.ToDouble(field9) / num5).ToString("N2");
        num4 = (num2 - 1) * num3 + 15;
        this.fieldCollect.Set(str1 + num4.ToString(), str8);
        string field10 = this.loan.GetField(str3 + "43");
        if (field10 != string.Empty)
        {
          string empty3 = string.Empty;
          string empty4 = string.Empty;
          string str9 = string.Empty;
          int num11 = 22;
          for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
          {
            string str10 = "FL" + index2.ToString("00");
            string field11 = this.loan.GetField(str10 + "25");
            if (field11 != string.Empty && field11 == field10)
            {
              num4 = (num2 - 1) * num3 + num11;
              this.fieldCollect.Set(str1 + num4.ToString(), this.loan.GetField(str10 + "02"));
              string field12 = this.loan.GetField(str10 + "04");
              string str11 = this.loan.GetField(str10 + "05").Trim();
              string str12;
              if (str11 == string.Empty)
                str12 = this.loan.GetField(str10 + "06") + " " + this.loan.GetField(str10 + "07");
              else
                str12 = str11 + ", " + this.loan.GetField(str10 + "06") + " " + this.loan.GetField(str10 + "07");
              num4 = (num2 - 1) * num3 + num11 + 1;
              this.fieldCollect.Set(str1 + num4.ToString(), field12);
              num4 = (num2 - 1) * num3 + num11 + 2;
              this.fieldCollect.Set(str1 + num4.ToString(), str12);
              num4 = (num2 - 1) * num3 + num11 + 3;
              this.fieldCollect.Set(str1 + num4.ToString(), this.loan.GetField(str10 + "10"));
              str9 = str9 + "|" + this.loan.GetField(str10 + "15").ToLower();
              switch (num11)
              {
                case 22:
                  num11 = 26;
                  continue;
                case 26:
                  goto label_49;
                default:
                  continue;
              }
            }
          }
label_49:
          num4 = (num2 - 1) * num3 + 3;
          if (str9.IndexOf("|borrower") > -1 && str9.IndexOf("|coborrower") == -1)
            this.fieldCollect.Set(str1 + num4.ToString(), "Borrower");
          else if (str9.IndexOf("|borrower") == -1 && str9.IndexOf("|coborrower") > -1)
            this.fieldCollect.Set(str1 + num4.ToString(), "Coborrower");
          else if (str9 == string.Empty)
            this.fieldCollect.Set(str1 + num4.ToString(), "Borrower");
          else
            this.fieldCollect.Set(str1 + num4.ToString(), "Both");
        }
      }
      for (int index = 0; index < 6; ++index)
      {
        num4 = index + 1;
        this.fieldCollect.Set("TOTAL_" + num4.ToString(), numArray[index].ToString("N2"));
      }
      for (int index = 1; index <= numberOfMortgages; ++index)
      {
        string str13 = "FM" + index.ToString("00");
        numArray[6] += this.ToDouble(this.loan.GetField(str13 + "19"));
        numArray[7] += this.ToDouble(this.loan.GetField(str13 + "17"));
        numArray[8] += this.ToDouble(this.loan.GetField(str13 + "20"));
        numArray[9] += this.ToDouble(this.loan.GetField(str13 + "16"));
        numArray[10] += this.ToDouble(this.loan.GetField(str13 + "21"));
        str2 = this.loan.GetField(str13 + "14");
        double num12 = this.ToDouble(this.loan.GetField(str13 + "14")) / 100.0;
        if (num12 == 0.0)
          num12 = 1.0;
        if (num12 != 1.0)
          numArray[11] += this.ToDouble(this.loan.GetField(str13 + "32")) / num12;
        else
          numArray[11] += this.ToDouble(this.loan.GetField(str13 + "32"));
      }
      for (int index = 6; index < 12; ++index)
      {
        num4 = index + 1;
        this.fieldCollect.Set("TOTAL_" + num4.ToString(), numArray[index].ToString("N2"));
      }
    }

    private void printRD410Page4()
    {
      this.pageDone = string.Empty;
      if (this.loan.GetNumberOfMortgages() <= 3)
        return;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 4; index <= 5; ++index)
      {
        string str1 = index.ToString("00");
        this.fieldCollect.Set("VOM" + (object) index + "_1", this.loan.GetSimpleField("FM" + str1 + "04"));
        string str2 = this.loan.GetSimpleField("FM" + str1 + "24");
        switch (str2)
        {
          case "PendingSale":
            str2 = "PS";
            break;
          case "Sold":
            str2 = "S";
            break;
          case "RetainForRental":
            str2 = "R";
            break;
        }
        this.fieldCollect.Set("VOM" + (object) index + "_2", str2);
        this.fieldCollect.Set("VOM" + (object) index + "_3", this.convertVOMPropertyType(this.loan.GetSimpleField("FM" + str1 + "18").ToUpper()));
        this.fieldCollect.Set("VOM" + (object) index + "_4", this.loan.GetSimpleField("FM" + (object) index + "19"));
        this.fieldCollect.Set("VOM" + (object) index + "_5", this.loan.GetSimpleField("FM" + (object) index + "17"));
        this.fieldCollect.Set("VOM" + (object) index + "_6", this.loan.GetSimpleField("FM" + (object) index + "20"));
        this.fieldCollect.Set("VOM" + (object) index + "_7", this.loan.GetSimpleField("FM" + (object) index + "16"));
        this.fieldCollect.Set("VOM" + (object) index + "_8", this.loan.GetSimpleField("FM" + (object) index + "21"));
        this.fieldCollect.Set("VOM" + (object) index + "_9", this.loan.GetSimpleField("FM" + (object) index + "32"));
      }
    }

    private void printBorrowerSummary()
    {
      this.pageDone = string.Empty;
      ConversationLog[] allConversations = this.loan.GetLogList().GetAllConversations();
      if (allConversations.Length == 0)
        return;
      int num1 = allConversations.Length - 3;
      if (num1 < 0)
        num1 = 0;
      int num2 = 0;
      for (int index = num1; index < allConversations.Length; ++index)
      {
        ++num2;
        this.fieldCollect.Set("BORSUMX" + Convert.ToString((num2 - 1) * 3 + 3), allConversations[index].Comments);
      }
      this.pageDone = string.Empty;
    }

    private void print203KMAX()
    {
      string field = this.loan.GetField("MAX23K.X53");
      this.fieldCollect.Set("MAX23KX53", Utils.StringWrapping(ref field, 120, 30, 1));
      this.pageDone = string.Empty;
    }

    private void printStatementOfDenial()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (this.formID == "STATEMENT OF DENIAL" || this.formID == "STATEMENT OF DENIAL_P3" || this.formID == "STATEMENT OF DENIAL_COBORROWER_P3")
      {
        string simpleField = this.loan.GetSimpleField("DENIAL.X27");
        this.fieldCollect.Set("3791", Utils.StringWrapping(ref simpleField, 100, 3, 1));
      }
      if (this.formID == "STATEMENT OF DENIAL_P2")
      {
        if (this.loan.GetField("DISCLOSURE.X625") == "Y")
          this.populateFactActComments(13, 16, "DISCLOSURE_X13_X14_X15_X16");
        if (this.loan.GetField("DISCLOSURE.X627") == "Y")
          this.populateFactActComments(33, 36, "DISCLOSURE_X33_X34_X35_X36");
      }
      if (this.formID == "STATEMENT OF DENIAL_P3" && this.loan.GetField("DISCLOSURE.X629") == "Y")
        this.populateFactActComments(53, 56, "DISCLOSURE_X53_X54_X55_X56");
      if (this.formID == "STATEMENT OF DENIAL_COBORROWER_P2")
      {
        if (this.loan.GetField("DISCLOSURE.X626") == "Y")
          this.populateFactActComments(17, 20, "DISCLOSURE_X17_X18_X19_X20");
        if (this.loan.GetField("DISCLOSURE.X628") == "Y")
          this.populateFactActComments(37, 40, "DISCLOSURE_X37_X38_X39_X40");
      }
      if (this.formID == "STATEMENT OF DENIAL_COBORROWER_P3" && this.loan.GetField("DISCLOSURE.X630") == "Y")
        this.populateFactActComments(57, 60, "DISCLOSURE_X57_X58_X59_X60");
      this.pageDone = string.Empty;
    }

    private void populateFactActComments(int startingID, int endingID, string targetField)
    {
      string val = string.Empty;
      for (int index = startingID; index <= endingID; ++index)
        val = val + this.loan.GetSimpleField("DISCLOSURE.X" + (object) index).Trim() + "\r\n";
      this.fieldCollect.Set(targetField, Utils.StringWrapping(ref val, 90, 7, 1));
    }

    private void print1098()
    {
      this.pageDone = string.Empty;
      DateTime dateTime = DateTime.MinValue;
      try
      {
        dateTime = DateTime.Parse(this.loan.GetField("LOANSUB.X16")).Date;
      }
      catch (Exception ex)
      {
      }
      PaymentSchedule[] schedule = this.calObjs.RegzCal.Schedule;
      int numberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      double num1 = 0.0;
      for (int index = 0; index < numberOfTerm; ++index)
      {
        try
        {
          DateTime date = DateTime.Parse(schedule[index].PayDate).Date;
          if ((dateTime - date).Days >= 0)
            num1 += schedule[index].Interest;
          else
            break;
        }
        catch (Exception ex)
        {
        }
      }
      double num2 = num1 + this.FltVal("334");
      if (num2 > 0.0)
      {
        this.fieldCollect.Set("TAX1098X1", num2.ToString("N2"));
      }
      else
      {
        if (num2 >= 0.0)
          return;
        this.fieldCollect.Set("TAX1098X2", num2.ToString("N2"));
      }
    }

    private void printTaxAndInsurance()
    {
      int count = 0;
      string empty = string.Empty;
      if (this.loan.GetField("VEND.X482") == "ForTax")
      {
        ++count;
        this.insertTaxLabels(count, new string[11]
        {
          "VEND.X342",
          "VEND.X343",
          "VEND.X344",
          "VEND.X345",
          "VEND.X347",
          "VEND.X458",
          "VEND.X459",
          "VEND.X460",
          "VEND.X461",
          "VEND.X462",
          "VEND.X463"
        });
        string upper = this.loan.GetField("1628").ToUpper();
        if (upper != string.Empty)
          upper += ":  ";
        string str = upper + this.loan.GetField("VEND.X341");
        this.fieldCollect.Set("H" + count.ToString(), str);
      }
      else if (this.loan.GetField("VEND.X482") == "ForInsurance")
      {
        ++count;
        this.insertInsuranceLabels(count, new string[10]
        {
          "VEND.X342",
          "VEND.X343",
          "VEND.X344",
          "VEND.X345",
          "VEND.X347",
          "VEND.X346",
          "VEND.X455",
          "VEND.X485",
          "VEND.X456",
          "VEND.X457"
        });
        string upper = this.loan.GetField("1628").ToUpper();
        if (upper != string.Empty)
          upper += ":  ";
        string str = upper + this.loan.GetField("VEND.X341");
        this.fieldCollect.Set("H" + count.ToString(), str);
      }
      if (this.loan.GetField("VEND.X483") == "ForTax")
      {
        ++count;
        this.insertTaxLabels(count, new string[11]
        {
          "VEND.X351",
          "VEND.X352",
          "VEND.X353",
          "VEND.X354",
          "VEND.X356",
          "VEND.X467",
          "VEND.X468",
          "VEND.X469",
          "VEND.X470",
          "VEND.X471",
          "VEND.X472"
        });
        string upper = this.loan.GetField("660").ToUpper();
        if (upper != string.Empty)
          upper += ":  ";
        string str = upper + this.loan.GetField("VEND.X350");
        this.fieldCollect.Set("H" + count.ToString(), str);
      }
      else if (this.loan.GetField("VEND.X483") == "ForInsurance")
      {
        ++count;
        this.insertInsuranceLabels(count, new string[10]
        {
          "VEND.X351",
          "VEND.X352",
          "VEND.X353",
          "VEND.X354",
          "VEND.X356",
          "VEND.X355",
          "VEND.X464",
          "VEND.X486",
          "VEND.X465",
          "VEND.X466"
        });
        string upper = this.loan.GetField("660").ToUpper();
        if (upper != string.Empty)
          upper += ":  ";
        string str = upper + this.loan.GetField("VEND.X350");
        this.fieldCollect.Set("H" + count.ToString(), str);
      }
      if (this.loan.GetField("VEND.X484") == "ForTax")
      {
        ++count;
        this.insertTaxLabels(count, new string[11]
        {
          "VEND.X360",
          "VEND.X361",
          "VEND.X362",
          "VEND.X363",
          "VEND.X365",
          "VEND.X476",
          "VEND.X477",
          "VEND.X478",
          "VEND.X479",
          "VEND.X480",
          "VEND.X481"
        });
        string upper = this.loan.GetField("661").ToUpper();
        if (upper != string.Empty)
          upper += ":  ";
        string str = upper + this.loan.GetField("VEND.X359");
        this.fieldCollect.Set("H" + count.ToString(), str);
      }
      else if (this.loan.GetField("VEND.X484") == "ForInsurance")
      {
        ++count;
        this.insertInsuranceLabels(count, new string[10]
        {
          "VEND.X360",
          "VEND.X361",
          "VEND.X362",
          "VEND.X363",
          "VEND.X365",
          "VEND.X473",
          "VEND.X364",
          "VEND.X487",
          "VEND.X474",
          "VEND.X475"
        });
        string upper = this.loan.GetField("661").ToUpper();
        if (upper != string.Empty)
          upper += ":  ";
        string str = upper + this.loan.GetField("VEND.X359");
        this.fieldCollect.Set("H" + count.ToString(), str);
      }
      string str1 = "___________________________________________________________________________________________________________________________________________";
      if (count > 1)
        this.fieldCollect.Set("DASH1", str1);
      if (count <= 2)
        return;
      this.fieldCollect.Set("DASH2", str1);
    }

    private void insertTaxLabels(int count, string[] ids)
    {
      string str1 = "C" + count.ToString();
      this.fieldCollect.Set(str1 + "_1", "Address:");
      this.fieldCollect.Set(str1 + "_2", "Phone Number:");
      this.fieldCollect.Set(str1 + "_3", "Payment Schedule:");
      this.fieldCollect.Set(str1 + "_4", "Amount Last Paid:");
      this.fieldCollect.Set(str1 + "_5", "Date Paid:");
      this.fieldCollect.Set(str1 + "_6", "Amount Next Due:");
      this.fieldCollect.Set(str1 + "_7", "Next Due Date:");
      this.fieldCollect.Set(str1 + "_8", "Date Taxes Delinquent:");
      string str2 = "C" + count.ToString() + count.ToString();
      this.fieldCollect.Set(str2 + "_1", this.loan.GetField(ids[0]));
      string field = this.loan.GetField(ids[1]);
      if (this.loan.GetField(ids[2]) != "")
        field += ", ";
      string str3 = field + this.loan.GetField(ids[2]) + " " + this.loan.GetField(ids[3]);
      this.fieldCollect.Set(str2 + "_2", str3);
      this.fieldCollect.Set(str2 + "_3", this.loan.GetField(ids[4]));
      this.fieldCollect.Set(str2 + "_4", this.loan.GetField(ids[5]));
      if (Utils.ParseDouble((object) this.loan.GetField(ids[6])) != 0.0)
        this.fieldCollect.Set(str2 + "_5", this.loan.GetField(ids[6]));
      else
        this.fieldCollect.Set(str2 + "_5", "");
      if (Utils.ParseDate((object) this.loan.GetField(ids[7])) != DateTime.MinValue)
        this.fieldCollect.Set(str2 + "_6", this.loan.GetField(ids[7]));
      else
        this.fieldCollect.Set(str2 + "_6", "");
      if (Utils.ParseDouble((object) this.loan.GetField(ids[8])) != 0.0)
        this.fieldCollect.Set(str2 + "_7", this.loan.GetField(ids[8]));
      else
        this.fieldCollect.Set(str2 + "_7", "");
      if (Utils.ParseDate((object) this.loan.GetField(ids[9])) != DateTime.MinValue)
        this.fieldCollect.Set(str2 + "_8", this.loan.GetField(ids[9]));
      else
        this.fieldCollect.Set(str2 + "_8", "");
      if (Utils.ParseDate((object) this.loan.GetField(ids[10])) != DateTime.MinValue)
        this.fieldCollect.Set(str2 + "_9", this.loan.GetField(ids[10]));
      else
        this.fieldCollect.Set(str2 + "_9", "");
    }

    private void insertInsuranceLabels(int count, string[] ids)
    {
      string str1 = "C" + count.ToString();
      this.fieldCollect.Set(str1 + "_1", "Address:");
      this.fieldCollect.Set(str1 + "_2", "Phone Number:");
      this.fieldCollect.Set(str1 + "_3", "Agent:");
      this.fieldCollect.Set(str1 + "_4", "Policy Number:");
      this.fieldCollect.Set(str1 + "_5", "Coverage Amount:");
      this.fieldCollect.Set(str1 + "_6", "Premium:");
      this.fieldCollect.Set(str1 + "_7", "Renewal Date:");
      this.fieldCollect.Set(str1 + "_8", "");
      string str2 = "C" + count.ToString() + count.ToString();
      this.fieldCollect.Set(str2 + "_1", this.loan.GetField(ids[0]));
      string field = this.loan.GetField(ids[1]);
      if (this.loan.GetField(ids[2]) != "")
        field += ", ";
      string str3 = field + this.loan.GetField(ids[2]) + " " + this.loan.GetField(ids[3]);
      this.fieldCollect.Set(str2 + "_2", str3);
      this.fieldCollect.Set(str2 + "_3", this.loan.GetField(ids[4]));
      this.fieldCollect.Set(str2 + "_4", this.loan.GetField(ids[5]));
      this.fieldCollect.Set(str2 + "_5", this.loan.GetField(ids[6]));
      if (Utils.ParseDouble((object) this.loan.GetField(ids[7])) != 0.0)
        this.fieldCollect.Set(str2 + "_6", this.loan.GetField(ids[7]));
      else
        this.fieldCollect.Set(str2 + "_6", "");
      if (Utils.ParseDouble((object) this.loan.GetField(ids[8])) != 0.0)
        this.fieldCollect.Set(str2 + "_7", this.loan.GetField(ids[8]));
      else
        this.fieldCollect.Set(str2 + "_7", "");
      if (Utils.ParseDate((object) this.loan.GetField(ids[9])) != DateTime.MinValue)
        this.fieldCollect.Set(str2 + "_8", this.loan.GetField(ids[9]));
      else
        this.fieldCollect.Set(str2 + "_8", "");
      this.fieldCollect.Set(str2 + "_9", "");
    }

    internal void CalcPrinting(
      ConditionalLetterPrintOption letterOption,
      List<string> selectedConditions)
    {
      int num1 = 0;
      int startingPageNumber = letterOption.StartingPageNumber;
      this.fieldCollect.Set("363", DateTime.Today.ToString("MM/dd/yyyy"));
      if ((this.pageDone ?? "") != string.Empty)
      {
        string[] strArray = this.pageDone.Split('|');
        num1 = Utils.ParseInt((object) strArray[0]);
        startingPageNumber = Utils.ParseInt((object) strArray[1]);
      }
      if (this.letterLayout == null || this.pageDone == string.Empty)
      {
        Hashtable loanAssociates = new Hashtable();
        MilestoneLog[] allMilestones = this.loan.GetLogList().GetAllMilestones();
        for (int index = 0; index < allMilestones.Length; ++index)
        {
          if (allMilestones[index].LoanAssociateName != string.Empty)
            loanAssociates[(object) allMilestones[index].RoleID] = (object) allMilestones[index].LoanAssociateName;
        }
        bool flag1 = this.loan.GetField("ENHANCEDCOND.X1") == "Y";
        ConditionLog[] conditionLogArray = flag1 ? this.loan.GetLogList().GetAllConditions(ConditionType.Enhanced) : this.loan.GetLogList().GetAllConditions(ConditionType.Underwriting);
        if (conditionLogArray == null || conditionLogArray.Length == 0)
        {
          this.pageDone = string.Empty;
          return;
        }
        List<ConditionLog> conditionsToPrint = new List<ConditionLog>();
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) null;
        EnhancedConditionLog enhancedConditionLog = (EnhancedConditionLog) null;
        foreach (ConditionLog conditionLog in conditionLogArray)
        {
          if (flag1)
          {
            enhancedConditionLog = (EnhancedConditionLog) conditionLog;
            if (string.Compare(enhancedConditionLog.EnhancedConditionType, "Underwriting", true) == 0)
            {
              bool? nullable;
              if (letterOption.LetterType == 0)
              {
                nullable = enhancedConditionLog.ExternalPrint;
                bool flag2 = false;
                if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
                  continue;
              }
              if (letterOption.LetterType == 1)
              {
                nullable = enhancedConditionLog.InternalPrint;
                bool flag3 = false;
                if (nullable.GetValueOrDefault() == flag3 & nullable.HasValue)
                  continue;
              }
            }
            else
              continue;
          }
          else
          {
            underwritingConditionLog = (UnderwritingConditionLog) conditionLog;
            if (letterOption.LetterType == 0 && !underwritingConditionLog.IsExternal || letterOption.LetterType == 1 && !underwritingConditionLog.IsInternal)
              continue;
          }
          if ((letterOption.ConditionOption != 1 || (flag1 || !underwritingConditionLog.Waived && !underwritingConditionLog.Cleared) && (!flag1 || enhancedConditionLog.StatusOpen)) && (letterOption.ConditionOption != 2 || selectedConditions.Contains(conditionLog.Guid)))
          {
            if (flag1)
              conditionsToPrint.Add((ConditionLog) enhancedConditionLog);
            else
              conditionsToPrint.Add((ConditionLog) underwritingConditionLog);
          }
        }
        this.letterLayout = new ConditionalLetterLayout(conditionsToPrint, letterOption, loanAssociates, this.sessionObjects);
      }
      this.printUnderwritingConditions();
      if (letterOption.ShowPageNumber)
        this.fieldCollect.Set("PAGENO", "Page " + (object) startingPageNumber);
      int num2 = startingPageNumber + 1;
      int num3 = 0;
      int num4 = 34;
      if (letterOption.UseLegalSize)
        num4 = !letterOption.SpaceCondensed ? 46 : 58;
      else if (letterOption.SpaceCondensed)
        num4 = 43;
      for (int i = num1; i < this.letterLayout.LineCount; ++i)
      {
        string[] printLine = this.letterLayout.GetPrintLine(i);
        if (printLine != null || num3 != 0)
        {
          if (printLine != null && printLine[0] == "pagebreak")
          {
            this.pageDone = (i + 1).ToString() + "|" + (object) num2;
            return;
          }
          if (printLine != null)
          {
            int num5 = Utils.ParseInt((object) printLine[6]);
            if (num5 > 0 && num3 + num5 > num4)
            {
              this.pageDone = i.ToString() + "|" + (object) num2;
              return;
            }
          }
          ++num3;
          if (num3 > num4)
          {
            this.pageDone = i.ToString() + "|" + (object) num2;
            return;
          }
          int num6 = (num3 - 1) * 4 + 1;
          this.fieldCollect.Set("UC_" + (object) num6, printLine == null ? "" : printLine[2]);
          int num7 = num6 + 1;
          this.fieldCollect.Set("UC_" + (object) num7, printLine == null ? "" : printLine[3]);
          int num8 = num7 + 1;
          this.fieldCollect.Set("UC_" + (object) num8, printLine == null ? "" : printLine[4]);
          this.fieldCollect.Set("UC_" + (object) (num8 + 1), printLine == null ? "" : printLine[5]);
        }
      }
      this.pageDone = string.Empty;
    }

    private void printPreliminaryConditionSummary()
    {
      string val = this.pageDone.Trim();
      this.pageDone = string.Empty;
      LogList logList = this.loan.GetLogList();
      if (logList == null)
        return;
      ConditionLog[] allConditions = logList.GetAllConditions(ConditionType.Preliminary);
      if (allConditions == null || allConditions.Length == 0)
        return;
      int num1 = 0;
      if (val != string.Empty)
        num1 = this.ToInt(val);
      int num2 = 0;
      for (int index = num1; index < allConditions.Length; ++index)
      {
        ++num2;
        if (num2 > 30)
        {
          this.pageDone = index.ToString();
          break;
        }
        PreliminaryConditionLog preliminaryConditionLog = (PreliminaryConditionLog) allConditions[index];
        int num3 = (num2 - 1) * 8;
        int num4 = num3 + 1;
        this.fieldCollect.Set(num4.ToString(), preliminaryConditionLog.Title);
        num4 = num3 + 2;
        this.fieldCollect.Set(num4.ToString(), preliminaryConditionLog.Category);
        num4 = num3 + 3;
        if (preliminaryConditionLog.PriorTo == "PTA")
          this.fieldCollect.Set(num4.ToString(), "Approval");
        else if (preliminaryConditionLog.PriorTo == "PTD")
          this.fieldCollect.Set(num4.ToString(), "Docs");
        else if (preliminaryConditionLog.PriorTo == "PTF")
          this.fieldCollect.Set(num4.ToString(), "Funding");
        else if (preliminaryConditionLog.PriorTo == "AC")
          this.fieldCollect.Set(num4.ToString(), "Closing");
        else if (preliminaryConditionLog.PriorTo == "PTP")
          this.fieldCollect.Set(num4.ToString(), "Purchase");
        else
          this.fieldCollect.Set(num4.ToString(), "");
        num4 = num3 + 4;
        this.fieldCollect.Set(num4.ToString(), preliminaryConditionLog.Source);
        num4 = num3 + 5;
        this.fieldCollect.Set(num4.ToString(), StandardConditionLog.GetStatusString(preliminaryConditionLog.Status));
        num4 = num3 + 6;
        NameValueCollection fieldCollect1 = this.fieldCollect;
        string name1 = num4.ToString();
        DateTime dateTime;
        string str1;
        if (!(preliminaryConditionLog.DateAdded != DateTime.MinValue))
        {
          str1 = "";
        }
        else
        {
          dateTime = preliminaryConditionLog.DateAdded;
          str1 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect1.Set(name1, str1);
        num4 = num3 + 7;
        NameValueCollection fieldCollect2 = this.fieldCollect;
        string name2 = num4.ToString();
        string str2;
        if (!(preliminaryConditionLog.DateFulfilled != DateTime.MinValue))
        {
          str2 = "";
        }
        else
        {
          dateTime = preliminaryConditionLog.DateFulfilled;
          str2 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect2.Set(name2, str2);
      }
    }

    private void printUnderwritingConditions()
    {
      this.checkTodayDate("363");
      this.pageDone = string.Empty;
      DateTime date1 = Utils.ParseDate((object) this.Val("2989"));
      DateTime date2 = Utils.ParseDate((object) this.Val("2301"));
      if (date1 != DateTime.MinValue)
      {
        this.fieldCollect.Set("363", date1.ToString("MM/dd/yyyy"));
      }
      else
      {
        if (!(date2 != DateTime.MinValue))
          return;
        this.fieldCollect.Set("363", date2.ToString("MM/dd/yyyy"));
      }
    }

    private void printUnderwriterSummary(int uwPage)
    {
      if (uwPage == 2)
      {
        string field1 = this.loan.GetField("2311");
        this.fieldCollect.Set("2311", Utils.StringWrapping(ref field1, 120, 10, 1));
        string field2 = this.loan.GetField("2319");
        this.fieldCollect.Set("2319", Utils.StringWrapping(ref field2, 120, 10, 1));
        string field3 = this.loan.GetField("2320");
        this.fieldCollect.Set("2320", Utils.StringWrapping(ref field3, 120, 10, 1));
        string field4 = this.loan.GetField("2321");
        this.fieldCollect.Set("2321", Utils.StringWrapping(ref field4, 120, 10, 1));
        string field5 = this.loan.GetField("2322");
        this.fieldCollect.Set("2322", Utils.StringWrapping(ref field5, 120, 10, 1));
        string field6 = this.loan.GetField("2323");
        this.fieldCollect.Set("2323", Utils.StringWrapping(ref field6, 120, 13, 1));
      }
      else
      {
        string field = this.loan.GetField("2362");
        this.fieldCollect.Set("2362", Utils.StringWrapping(ref field, 120, 20, 1));
      }
    }

    private void printTasksSummary()
    {
      string val = this.pageDone.Trim();
      this.pageDone = string.Empty;
      LogList logList = this.loan.GetLogList();
      if (logList == null)
        return;
      MilestoneTaskLog[] milestoneTaskLogs = logList.GetAllMilestoneTaskLogs((string) null);
      if (milestoneTaskLogs == null || milestoneTaskLogs.Length == 0)
        return;
      int num1 = 0;
      if (val != string.Empty)
        num1 = this.ToInt(val);
      int num2 = 0;
      for (int index1 = num1; index1 < milestoneTaskLogs.Length; ++index1)
      {
        ++num2;
        if (num2 > 29)
        {
          this.pageDone = index1.ToString();
          break;
        }
        MilestoneTaskLog milestoneTaskLog = milestoneTaskLogs[index1];
        int num3 = (num2 - 1) * 8;
        int num4 = num3 + 1;
        this.fieldCollect.Set(num4.ToString(), milestoneTaskLog.TaskName);
        num4 = num3 + 2;
        if (milestoneTaskLog.Stage == string.Empty || milestoneTaskLog.Stage == null)
        {
          this.fieldCollect.Set(num4.ToString(), "");
        }
        else
        {
          string stage = milestoneTaskLog.Stage;
          this.fieldCollect.Set(num4.ToString(), stage);
        }
        if (milestoneTaskLog.ContactCount > 0)
        {
          MilestoneTaskLog.TaskContact taskContactAt = milestoneTaskLog.GetTaskContactAt(0);
          if (taskContactAt != null)
          {
            num4 = num3 + 3;
            this.fieldCollect.Set(num4.ToString(), taskContactAt.ContactName);
          }
        }
        num4 = num3 + 4;
        DateTime dateTime;
        if (milestoneTaskLog.CompletedBy != string.Empty)
        {
          this.fieldCollect.Set(num4.ToString(), "Completed");
          if (milestoneTaskLog.CompletedDate != DateTime.MinValue)
          {
            num4 = num3 + 5;
            NameValueCollection fieldCollect = this.fieldCollect;
            string name = num4.ToString();
            dateTime = milestoneTaskLog.CompletedDate;
            string str = dateTime.ToString("MM/dd/yyyy");
            fieldCollect.Set(name, str);
          }
          num4 = num3 + 6;
          this.fieldCollect.Set(num4.ToString(), milestoneTaskLog.CompletedBy);
        }
        else
        {
          this.fieldCollect.Set(num4.ToString(), "Added");
          num4 = num3 + 5;
          NameValueCollection fieldCollect = this.fieldCollect;
          string name = num4.ToString();
          dateTime = milestoneTaskLog.Date;
          string str = dateTime.ToString("MM/dd/yyyy");
          fieldCollect.Set(name, str);
          num4 = num3 + 6;
          this.fieldCollect.Set(num4.ToString(), milestoneTaskLog.AddedBy);
        }
        if (milestoneTaskLog.AlertList.Count > 0)
        {
          for (int index2 = 0; index2 < milestoneTaskLog.AlertList.Count; ++index2)
          {
            LogAlert alert = milestoneTaskLog.AlertList[index2];
            if (alert.RoleId != 0 && !alert.IsFollowedUp)
            {
              num4 = num3 + 7;
              this.fieldCollect.Set(num4.ToString(), "Y");
              break;
            }
          }
        }
      }
    }

    private DisclosureTrackingLog[] getAllDisclosureTrackingLog(bool validLogOnly, bool descSort)
    {
      List<DisclosureTrackingLog> disclosureTrackingLogList1 = new List<DisclosureTrackingLog>((IEnumerable<DisclosureTrackingLog>) this.loan.GetLogList().GetAllDisclosureTrackingLog(validLogOnly));
      disclosureTrackingLogList1.Sort();
      if (!descSort)
        return disclosureTrackingLogList1.ToArray();
      List<DisclosureTrackingLog> disclosureTrackingLogList2 = new List<DisclosureTrackingLog>();
      for (int index = disclosureTrackingLogList1.Count - 1; index >= 0; --index)
        disclosureTrackingLogList2.Add(disclosureTrackingLogList1[index]);
      return disclosureTrackingLogList2.ToArray();
    }

    private void printDisclosureTrackingDetails()
    {
      string str1 = this.pageDone.Trim();
      this.pageDone = string.Empty;
      DisclosureTrackingLog[] disclosureTrackingLog1 = this.getAllDisclosureTrackingLog(false, true);
      if (disclosureTrackingLog1.Length == 0)
        return;
      int index1 = 0;
      int num1 = 0;
      if (str1 != string.Empty)
      {
        string[] strArray = str1.Split(',');
        index1 = Utils.ParseInt((object) strArray[0]);
        num1 = Utils.ParseInt((object) strArray[1]);
      }
      if (index1 < disclosureTrackingLog1.Length)
      {
        DisclosureTrackingLog disclosureTrackingLog2 = disclosureTrackingLog1[index1];
        this.fieldCollect.Set("36_37", disclosureTrackingLog2.BorrowerName);
        this.fieldCollect.Set("68_69", disclosureTrackingLog2.CoBorrowerName);
        NameValueCollection fieldCollect1 = this.fieldCollect;
        DateTime dateTime;
        string str2;
        if (!(disclosureTrackingLog2.DisclosedDate == DateTime.MinValue))
        {
          dateTime = disclosureTrackingLog2.DisclosedDate;
          str2 = dateTime.ToString("MM/dd/yyyy");
        }
        else
          str2 = "";
        fieldCollect1.Set("3137", str2);
        if (!disclosureTrackingLog2.IsDisclosedByLocked)
          this.fieldCollect.Set("3139", disclosureTrackingLog2.DisclosedByFullName + "(" + disclosureTrackingLog2.DisclosedBy + ")");
        else
          this.fieldCollect.Set("3139", disclosureTrackingLog2.DisclosedByFullName);
        switch (disclosureTrackingLog2.DisclosureMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            this.fieldCollect.Set("3138", this.discloseMethod[4]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            this.fieldCollect.Set("3138", this.discloseMethod[3]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            this.fieldCollect.Set("3138", this.discloseMethod[1]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            this.fieldCollect.Set("3138", this.discloseMethod[2]);
            break;
          default:
            this.fieldCollect.Set("3138", this.discloseMethod[0]);
            break;
        }
        NameValueCollection fieldCollect2 = this.fieldCollect;
        string str3;
        if (!(disclosureTrackingLog2.ReceivedDate == DateTime.MinValue))
        {
          dateTime = disclosureTrackingLog2.ReceivedDate;
          str3 = dateTime.ToString("MM/dd/yyyy");
        }
        else
          str3 = "";
        fieldCollect2.Set("borrowerReceivedDate", str3);
        this.fieldCollect.Set("3121", disclosureTrackingLog2.DisclosedAPR);
        this.fieldCollect.Set("1401", disclosureTrackingLog2.LoanProgram);
        this.fieldCollect.Set("2a", disclosureTrackingLog2.LoanAmount);
        this.fieldCollect.Set("1206", disclosureTrackingLog2.FinanceCharge);
        NameValueCollection fieldCollect3 = this.fieldCollect;
        string str4;
        if (!(disclosureTrackingLog2.ApplicationDate == DateTime.MinValue))
        {
          dateTime = disclosureTrackingLog2.ApplicationDate;
          str4 = dateTime.ToString("MM/dd/yyyy");
        }
        else
          str4 = "";
        fieldCollect3.Set("applicationDate", str4);
        string comments = disclosureTrackingLog2.Comments;
        this.fieldCollect.Set("3166", Utils.StringWrapping(ref comments, 60, 4, 1));
        this.fieldCollect.Set("11b", disclosureTrackingLog2.PropertyAddress);
        this.fieldCollect.Set("12_14_15b", disclosureTrackingLog2.PropertyCity + (disclosureTrackingLog2.PropertyState != "" ? "," : "") + disclosureTrackingLog2.PropertyState + " " + disclosureTrackingLog2.PropertyZip);
        NameValueCollection fieldCollect4 = this.fieldCollect;
        string str5;
        if (!(disclosureTrackingLog2.eDisclosurePackageCreatedDate != DateTime.MinValue))
        {
          str5 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosurePackageCreatedDate;
          str5 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect4.Set("eDisclosure_1", str5);
        NameValueCollection fieldCollect5 = this.fieldCollect;
        string str6;
        if (!(disclosureTrackingLog2.eDisclosureBorrowerViewMessageDate != DateTime.MinValue))
        {
          str6 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureBorrowerViewMessageDate;
          str6 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect5.Set("eDisclosure_2", str6);
        NameValueCollection fieldCollect6 = this.fieldCollect;
        string str7;
        if (!(disclosureTrackingLog2.eDisclosureBorrowerAcceptConsentDate != DateTime.MinValue))
        {
          str7 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureBorrowerAcceptConsentDate;
          str7 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect6.Set("eDisclosure_3", str7);
        NameValueCollection fieldCollect7 = this.fieldCollect;
        string str8;
        if (!(disclosureTrackingLog2.eDisclosureBorrowerRejectConsentDate != DateTime.MinValue))
        {
          str8 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureBorrowerRejectConsentDate;
          str8 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect7.Set("eDisclosure_4", str8);
        NameValueCollection fieldCollect8 = this.fieldCollect;
        string str9;
        if (!(disclosureTrackingLog2.eDisclosureBorrowereSignedDate != DateTime.MinValue))
        {
          str9 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureBorrowereSignedDate;
          str9 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect8.Set("eDisclosure_5", str9);
        this.fieldCollect.Set("eDisclosure_6", disclosureTrackingLog2.FulfillmentOrderedBy);
        NameValueCollection fieldCollect9 = this.fieldCollect;
        string str10;
        if (!(disclosureTrackingLog2.FullfillmentProcessedDate != DateTime.MinValue))
        {
          str10 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.FullfillmentProcessedDate;
          str10 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect9.Set("eDisclosure_7", str10);
        NameValueCollection fieldCollect10 = this.fieldCollect;
        string str11;
        if (!(disclosureTrackingLog2.eDisclosureCoBorrowerViewConsentDate != DateTime.MinValue))
        {
          str11 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureCoBorrowerViewConsentDate;
          str11 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect10.Set("eDisclosure_8", str11);
        NameValueCollection fieldCollect11 = this.fieldCollect;
        string str12;
        if (!(disclosureTrackingLog2.eDisclosureCoBorrowerAcceptConsentDate != DateTime.MinValue))
        {
          str12 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureCoBorrowerAcceptConsentDate;
          str12 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect11.Set("eDisclosure_9", str12);
        NameValueCollection fieldCollect12 = this.fieldCollect;
        string str13;
        if (!(disclosureTrackingLog2.eDisclosureCoBorrowerRejectConsentDate != DateTime.MinValue))
        {
          str13 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureCoBorrowerRejectConsentDate;
          str13 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect12.Set("eDisclosure_10", str13);
        NameValueCollection fieldCollect13 = this.fieldCollect;
        string str14;
        if (!(disclosureTrackingLog2.eDisclosureCoBorrowereSignedDate != DateTime.MinValue))
        {
          str14 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureCoBorrowereSignedDate;
          str14 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect13.Set("eDisclosure_11", str14);
        this.fieldCollect.Set("eDisclosure_12", disclosureTrackingLog2.eDisclosureLOName);
        NameValueCollection fieldCollect14 = this.fieldCollect;
        string str15;
        if (!(disclosureTrackingLog2.eDisclosureLOViewMessageDate != DateTime.MinValue))
        {
          str15 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureLOViewMessageDate;
          str15 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect14.Set("eDisclosure_13", str15);
        NameValueCollection fieldCollect15 = this.fieldCollect;
        string str16;
        if (!(disclosureTrackingLog2.eDisclosureLOeSignedDate != DateTime.MinValue))
        {
          str16 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureLOeSignedDate;
          str16 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect15.Set("eDisclosure_14", str16);
        this.fieldCollect.Set("eDisclosure_15", disclosureTrackingLog2.eDisclosureLOeSignedIP);
        this.fieldCollect.Set("eDisclosure_16", disclosureTrackingLog2.eDisclosureBorrowerName);
        this.fieldCollect.Set("eDisclosure_17", disclosureTrackingLog2.eDisclosureBorrowerAcceptConsentIP);
        this.fieldCollect.Set("eDisclosure_18", disclosureTrackingLog2.eDisclosureBorrowerRejectConsentIP);
        this.fieldCollect.Set("eDisclosure_19", disclosureTrackingLog2.eDisclosureBorrowerEmail);
        NameValueCollection fieldCollect16 = this.fieldCollect;
        string str17;
        if (!(disclosureTrackingLog2.eDisclosureBorrowerAuthenticatedDate != DateTime.MinValue))
        {
          str17 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureBorrowerAuthenticatedDate;
          str17 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect16.Set("eDisclosure_20", str17);
        this.fieldCollect.Set("eDisclosure_21", disclosureTrackingLog2.eDisclosureBorrowerAuthenticatedIP);
        this.fieldCollect.Set("eDisclosure_22", disclosureTrackingLog2.eDisclosureBorrowereSignedIP);
        this.fieldCollect.Set("eDisclosure_23", disclosureTrackingLog2.eDisclosureCoBorrowerName);
        this.fieldCollect.Set("eDisclosure_24", disclosureTrackingLog2.eDisclosureCoBorrowerAcceptConsentIP);
        this.fieldCollect.Set("eDisclosure_25", disclosureTrackingLog2.eDisclosureCoBorrowerRejectConsentIP);
        this.fieldCollect.Set("eDisclosure_26", disclosureTrackingLog2.eDisclosureCoBorrowerEmail);
        NameValueCollection fieldCollect17 = this.fieldCollect;
        string str18;
        if (!(disclosureTrackingLog2.eDisclosureCoBorrowerAuthenticatedDate != DateTime.MinValue))
        {
          str18 = "";
        }
        else
        {
          dateTime = disclosureTrackingLog2.eDisclosureCoBorrowerAuthenticatedDate;
          str18 = dateTime.ToString("MM/dd/yyyy");
        }
        fieldCollect17.Set("eDisclosure_27", str18);
        this.fieldCollect.Set("eDisclosure_28", disclosureTrackingLog2.eDisclosureCoBorrowerAuthenticatedIP);
        this.fieldCollect.Set("eDisclosure_29", disclosureTrackingLog2.eDisclosureCoBorrowereSignedIP);
        if (disclosureTrackingLog2.DisclosedFormList != null)
        {
          int num2 = 1;
          for (int index2 = num1; index2 < disclosureTrackingLog2.DisclosedFormList.Length; ++index2)
          {
            if (num2 > 14)
            {
              this.pageDone = index1.ToString() + "," + (object) index2;
              return;
            }
            int num3 = (num2 - 1) * 2;
            this.fieldCollect.Set(string.Concat((object) (num3 + 1)), disclosureTrackingLog2.DisclosedFormList[index2].FormName);
            this.fieldCollect.Set(string.Concat((object) (num3 + 2)), disclosureTrackingLog2.DisclosedFormList[index2].OutputFormTypeName);
            ++num2;
          }
        }
      }
      if (index1 + 1 >= disclosureTrackingLog1.Length)
        return;
      this.pageDone = (index1 + 1).ToString() + ",0";
    }

    private void printDisclosureTrackingSummary()
    {
      string str = this.pageDone.Trim();
      DisclosureTrackingLog disclosureTrackingLog1 = this.loan.GetLogList().GetLatestDisclosureTrackingLog(DisclosureTrackingLog.DisclosureTrackingType.All);
      this.fieldCollect.Set("1401", disclosureTrackingLog1 != null ? disclosureTrackingLog1.LoanProgram : "");
      this.fieldCollect.Set("2a", disclosureTrackingLog1 != null ? disclosureTrackingLog1.LoanAmount : "");
      this.fieldCollect.Set("1206", disclosureTrackingLog1 != null ? disclosureTrackingLog1.FinanceCharge : "");
      this.fieldCollect.Set("3121", disclosureTrackingLog1 != null ? disclosureTrackingLog1.DisclosedAPR : "");
      DisclosureTrackingLog[] disclosureTrackingLog2 = this.getAllDisclosureTrackingLog(false, true);
      if (disclosureTrackingLog2 == null || disclosureTrackingLog2.Length == 0)
        return;
      int num1 = 0;
      if (str != string.Empty)
        num1 = Utils.ParseInt((object) str);
      int num2 = 1;
      for (int index = num1; index < disclosureTrackingLog2.Length; ++index)
      {
        if (num2 > 30)
        {
          this.pageDone = string.Concat((object) index);
          return;
        }
        DisclosureTrackingLog disclosureTrackingLog3 = disclosureTrackingLog2[index];
        int num3 = (num2 - 1) * 8;
        this.fieldCollect.Set(string.Concat((object) (num3 + 1)), disclosureTrackingLog3.DisclosedDate.ToString("MM/dd/yyyy"));
        switch (disclosureTrackingLog3.DisclosureMethod)
        {
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            this.fieldCollect.Set(string.Concat((object) (num3 + 2)), this.discloseMethod[4]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            this.fieldCollect.Set(string.Concat((object) (num3 + 2)), this.discloseMethod[3]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            this.fieldCollect.Set(string.Concat((object) (num3 + 2)), this.discloseMethod[1]);
            break;
          case DisclosureTrackingBase.DisclosedMethod.Other:
            this.fieldCollect.Set(string.Concat((object) (num3 + 2)), this.discloseMethod[2]);
            break;
          default:
            this.fieldCollect.Set(string.Concat((object) (num3 + 2)), this.discloseMethod[0]);
            break;
        }
        if (!disclosureTrackingLog3.IsDisclosedByLocked)
          this.fieldCollect.Set(string.Concat((object) (num3 + 3)), disclosureTrackingLog3.DisclosedByFullName + "(" + disclosureTrackingLog3.DisclosedBy + ")");
        else
          this.fieldCollect.Set(string.Concat((object) (num3 + 3)), disclosureTrackingLog3.DisclosedByFullName);
        this.fieldCollect.Set(string.Concat((object) (num3 + 4)), string.Concat((object) disclosureTrackingLog3.NumOfDisclosedDocs));
        this.fieldCollect.Set(string.Concat((object) (num3 + 5)), disclosureTrackingLog3.DisclosedForGFE ? "Yes" : "No");
        this.fieldCollect.Set(string.Concat((object) (num3 + 6)), disclosureTrackingLog3.DisclosedForTIL ? "Yes" : "No");
        if (disclosureTrackingLog3.CoBorrowerName.Trim() != "")
          this.fieldCollect.Set(string.Concat((object) (num3 + 7)), disclosureTrackingLog3.BorrowerName + " and " + disclosureTrackingLog3.CoBorrowerName);
        else
          this.fieldCollect.Set(string.Concat((object) (num3 + 7)), disclosureTrackingLog3.BorrowerName);
        if (disclosureTrackingLog3.IsDisclosed)
          this.fieldCollect.Set(string.Concat((object) (num3 + 8)), "Yes");
        else
          this.fieldCollect.Set(string.Concat((object) (num3 + 8)), "No");
        ++num2;
      }
      this.pageDone = string.Empty;
    }

    private void printCreditScoreH1()
    {
      int num = 1;
      if (this.pageDone != string.Empty)
        num = Utils.ParseInt((object) this.pageDone);
      for (int index = num; index <= 3; ++index)
      {
        string val = this.Val("DISCLOSURE.X637");
        this.fieldCollect.Set("DISCLOSURE_X637", Utils.StringWrapping(ref val, 36, 1, 1));
        this.fieldCollect.Set("DISCLOSURE_X637b", Utils.StringWrapping(ref val, 120, 1, 1));
        switch (index)
        {
          case 1:
            if (!(this.Val("DISCLOSURE.X1") == string.Empty))
            {
              this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X1"));
              this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X1") + (this.Val("DISCLOSURE.X1") != string.Empty ? ":" : ""));
              this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X6"));
              this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X2"));
              string str = this.Val("DISCLOSURE.X3");
              if (str != string.Empty)
                str += ",";
              this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str + this.Val("DISCLOSURE.X4") + " " + this.Val("DISCLOSURE.X5"));
              this.fieldCollect.Set("DISCLOSURE_X638", this.Val("DISCLOSURE.X638"));
              if (this.Val("DISCLOSURE.X21") != string.Empty)
              {
                this.pageDone = "2";
                return;
              }
              if (this.Val("DISCLOSURE.X41") != string.Empty)
              {
                this.pageDone = "3";
                return;
              }
              this.pageDone = string.Empty;
              return;
            }
            break;
          case 2:
            if (!(this.Val("DISCLOSURE.X21") == string.Empty))
            {
              this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X21"));
              this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X21") + (this.Val("DISCLOSURE.X21") != string.Empty ? ":" : ""));
              this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X26"));
              this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X22"));
              string str = this.Val("DISCLOSURE.X23");
              if (str != string.Empty)
                str += ",";
              this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str + this.Val("DISCLOSURE.X24") + " " + this.Val("DISCLOSURE.X25"));
              this.fieldCollect.Set("DISCLOSURE_X638", this.Val("DISCLOSURE.X639"));
              if (this.Val("DISCLOSURE.X41") != string.Empty)
              {
                this.pageDone = "3";
                return;
              }
              this.pageDone = string.Empty;
              return;
            }
            break;
          case 3:
            if (!(this.Val("DISCLOSURE.X41") == string.Empty))
            {
              this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X41"));
              this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X41") + (this.Val("DISCLOSURE.X41") != string.Empty ? ":" : ""));
              this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X46"));
              this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X42"));
              string str = this.Val("DISCLOSURE.X43");
              if (str != string.Empty)
                str += ",";
              this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str + this.Val("DISCLOSURE.X44") + " " + this.Val("DISCLOSURE.X45"));
              this.fieldCollect.Set("DISCLOSURE_X638", this.Val("DISCLOSURE.X640"));
              this.pageDone = string.Empty;
              break;
            }
            break;
        }
      }
    }

    private void printCreditScoreH3(int page, int borrower)
    {
      if (page == 1)
      {
        if (borrower == 1 || borrower == 2)
        {
          this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X1"));
          this.fieldCollect.Set("DISCLOSURE_X9", this.Val("DISCLOSURE.X9"));
          this.fieldCollect.Set("DISCLOSURE_X10", this.Val("DISCLOSURE.X10"));
        }
        else if (borrower == 3 || borrower == 4)
        {
          this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X21"));
          this.fieldCollect.Set("DISCLOSURE_X9", this.Val("DISCLOSURE.X29"));
          this.fieldCollect.Set("DISCLOSURE_X10", this.Val("DISCLOSURE.X30"));
        }
        else if (borrower == 5 || borrower == 6)
        {
          this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X41"));
          this.fieldCollect.Set("DISCLOSURE_X9", this.Val("DISCLOSURE.X49"));
          this.fieldCollect.Set("DISCLOSURE_X10", this.Val("DISCLOSURE.X50"));
        }
      }
      if (borrower == 1 || borrower == 3 || borrower == 5)
        this.fieldCollect.Set("36_37", this.Val("36") + " " + this.Val("37"));
      else
        this.fieldCollect.Set("36_37", this.Val("68") + " " + this.Val("69"));
      if (borrower == 1 && page == 1)
      {
        this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X11"));
        this.fieldCollect.Set("67", this.Val("67"));
        this.fieldCollect.Set("DISCLOSURE_X631", this.Val("DISCLOSURE.X631"));
        string val = this.Val("DISCLOSURE.X13") + "\r\n" + this.Val("DISCLOSURE.X14") + "\r\n" + this.Val("DISCLOSURE.X15") + "\r\n" + this.Val("DISCLOSURE.X16");
        if (this.Val("DISCLOSURE.X173") == "Y")
          val += "\r\n[X] Number of Recent Inquiries on Credt Report";
        this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
      }
      if (borrower == 3 && page == 1)
      {
        this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X31"));
        this.fieldCollect.Set("67", this.Val("1450"));
        this.fieldCollect.Set("DISCLOSURE_X631", this.Val("DISCLOSURE.X633"));
        string val = this.Val("DISCLOSURE.X33") + "\r\n" + this.Val("DISCLOSURE.X34") + "\r\n" + this.Val("DISCLOSURE.X35") + "\r\n" + this.Val("DISCLOSURE.X36");
        if (this.Val("DISCLOSURE.X174") == "Y")
          val += "\r\n[X] Number of Recent Inquiries on Credt Report";
        this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
      }
      if (borrower == 5 && page == 1)
      {
        this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X51"));
        this.fieldCollect.Set("67", this.Val("1414"));
        this.fieldCollect.Set("DISCLOSURE_X631", this.Val("DISCLOSURE.X635"));
        string val = this.Val("DISCLOSURE.X53") + "\r\n" + this.Val("DISCLOSURE.X54") + "\r\n" + this.Val("DISCLOSURE.X55") + "\r\n" + this.Val("DISCLOSURE.X56");
        if (this.Val("DISCLOSURE.X175") == "Y")
          val += "\r\n[X] Number of Recent Inquiries on Credt Report";
        this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
      }
      if (borrower == 2 && page == 1)
      {
        this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X12"));
        this.fieldCollect.Set("67", this.Val("60"));
        this.fieldCollect.Set("DISCLOSURE_X631", this.Val("DISCLOSURE.X632"));
        string val = this.Val("DISCLOSURE.X17") + "\r\n" + this.Val("DISCLOSURE.X18") + "\r\n" + this.Val("DISCLOSURE.X19") + "\r\n" + this.Val("DISCLOSURE.X20");
        if (this.Val("DISCLOSURE.X176") == "Y")
          val += "\r\n[X] Number of Recent Inquiries on Credt Report";
        this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
      }
      if (borrower == 4 && page == 1)
      {
        this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X32"));
        this.fieldCollect.Set("67", this.Val("1452"));
        this.fieldCollect.Set("DISCLOSURE_X631", this.Val("DISCLOSURE.X634"));
        string val = this.Val("DISCLOSURE.X37") + "\r\n" + this.Val("DISCLOSURE.X38") + "\r\n" + this.Val("DISCLOSURE.X39") + "\r\n" + this.Val("DISCLOSURE.X40");
        if (this.Val("DISCLOSURE.X177") == "Y")
          val += "\r\n[X] Number of Recent Inquiries on Credt Report";
        this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
      }
      if (borrower == 6 && page == 1)
      {
        this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X52"));
        this.fieldCollect.Set("67", this.Val("1415"));
        this.fieldCollect.Set("DISCLOSURE_X631", this.Val("DISCLOSURE.X636"));
        string val = this.Val("DISCLOSURE.X57") + "\r\n" + this.Val("DISCLOSURE.X58") + "\r\n" + this.Val("DISCLOSURE.X59") + "\r\n" + this.Val("DISCLOSURE.X60");
        if (this.Val("DISCLOSURE.X178") == "Y")
          val += "\r\n[X] Number of Recent Inquiries on Credt Report";
        this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
      }
      this.pageDone = string.Empty;
    }

    private void printCreditScoreH5()
    {
      int index1 = 0;
      if (this.pageDone != string.Empty)
        index1 = Utils.ParseInt((object) this.pageDone);
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      for (int index2 = 1; index2 <= 6; ++index2)
      {
        if (index2 == 1 && this.Val("DISCLOSURE.X1") != string.Empty && (this.Val("36") != string.Empty || this.Val("37") != string.Empty))
        {
          stringList1.Add(this.Val("DISCLOSURE.X1"));
          stringList2.Add(this.Val("36") + " " + this.Val("37"));
        }
        else if (index2 == 2 && this.Val("DISCLOSURE.X1") != string.Empty && (this.Val("68") != string.Empty || this.Val("69") != string.Empty))
        {
          stringList1.Add(this.Val("DISCLOSURE.X1"));
          stringList2.Add(this.Val("68") + " " + this.Val("69"));
        }
        else if (index2 == 3 && this.Val("DISCLOSURE.X21") != string.Empty && (this.Val("36") != string.Empty || this.Val("37") != string.Empty))
        {
          stringList1.Add(this.Val("DISCLOSURE.X21"));
          stringList2.Add(this.Val("36") + " " + this.Val("37"));
        }
        else if (index2 == 4 && this.Val("DISCLOSURE.X21") != string.Empty && (this.Val("68") != string.Empty || this.Val("69") != string.Empty))
        {
          stringList1.Add(this.Val("DISCLOSURE.X21"));
          stringList2.Add(this.Val("68") + " " + this.Val("69"));
        }
        else if (index2 == 5 && this.Val("DISCLOSURE.X41") != string.Empty && (this.Val("36") != string.Empty || this.Val("37") != string.Empty))
        {
          stringList1.Add(this.Val("DISCLOSURE.X41"));
          stringList2.Add(this.Val("36") + " " + this.Val("37"));
        }
        else if (index2 == 6 && this.Val("DISCLOSURE.X41") != string.Empty && (this.Val("68") != string.Empty || this.Val("69") != string.Empty))
        {
          stringList1.Add(this.Val("DISCLOSURE.X41"));
          stringList2.Add(this.Val("68") + " " + this.Val("69"));
        }
      }
      if (stringList1.Count == 0)
      {
        stringList1.Add("");
        stringList2.Add(this.Val("36") + " " + this.Val("37"));
        if (this.Val("68") != string.Empty || this.Val("69") != string.Empty)
        {
          stringList1.Add("");
          stringList2.Add(this.Val("68") + " " + this.Val("69"));
        }
      }
      this.fieldCollect.Set("DISCLOSURE_X1", index1 < stringList1.Count ? stringList1[index1] : "");
      this.fieldCollect.Set("36_37", index1 < stringList2.Count ? stringList2[index1] : "");
      int num = index1 + 1;
      if (num < stringList1.Count)
        this.pageDone = num.ToString();
      else
        this.pageDone = string.Empty;
    }

    private void printCreditScoreH6(int page, int borrower)
    {
      switch (borrower)
      {
        case 1:
        case 2:
          this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X1"));
          break;
        case 3:
        case 4:
          this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X21"));
          break;
        case 5:
        case 6:
          this.fieldCollect.Set("DISCLOSURE_X1", this.Val("DISCLOSURE.X41"));
          break;
      }
      if (page == 1)
      {
        string val = this.Val("DISCLOSURE.X637");
        this.fieldCollect.Set("DISCLOSURE_X637", Utils.StringWrapping(ref val, 36, 1, 1));
        this.fieldCollect.Set("DISCLOSURE_X637b", Utils.StringWrapping(ref val, 120, 1, 1));
        if (borrower == 1 || borrower == 3 || borrower == 5)
          this.fieldCollect.Set("36_37", this.Val("36") + " " + this.Val("37"));
        else
          this.fieldCollect.Set("36_37", this.Val("68") + " " + this.Val("69"));
        switch (borrower)
        {
          case 1:
          case 2:
            this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X1") + (this.Val("DISCLOSURE.X1") != string.Empty ? ":" : ""));
            this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X6"));
            this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X2"));
            string str1 = this.Val("DISCLOSURE.X3");
            if (str1 != string.Empty)
              str1 += ",";
            this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str1 + this.Val("DISCLOSURE.X4") + " " + this.Val("DISCLOSURE.X5"));
            this.fieldCollect.Set("DISCLOSURE_X638", this.Val("DISCLOSURE.X638"));
            break;
          case 3:
          case 4:
            this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X21") + (this.Val("DISCLOSURE.X21") != string.Empty ? ":" : ""));
            this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X26"));
            this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X22"));
            string str2 = this.Val("DISCLOSURE.X23");
            if (str2 != string.Empty)
              str2 += ",";
            this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str2 + this.Val("DISCLOSURE.X24") + " " + this.Val("DISCLOSURE.X25"));
            this.fieldCollect.Set("DISCLOSURE_X638", this.Val("DISCLOSURE.X639"));
            break;
          case 5:
          case 6:
            this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X41") + (this.Val("DISCLOSURE.X41") != string.Empty ? ":" : ""));
            this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X46"));
            this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X42"));
            string str3 = this.Val("DISCLOSURE.X43");
            if (str3 != string.Empty)
              str3 += ",";
            this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str3 + this.Val("DISCLOSURE.X44") + " " + this.Val("DISCLOSURE.X45"));
            this.fieldCollect.Set("DISCLOSURE_X638", this.Val("DISCLOSURE.X640"));
            break;
        }
      }
      else if (page == 2)
      {
        switch (borrower)
        {
          case 1:
          case 2:
            this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X1"));
            this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X2"));
            string str4 = this.Val("DISCLOSURE.X3");
            if (str4 != string.Empty)
              str4 += ",";
            this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str4 + this.Val("DISCLOSURE.X4") + " " + this.Val("DISCLOSURE.X5"));
            this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X6"));
            this.fieldCollect.Set("DISCLOSURE_X9", this.Val("DISCLOSURE.X9"));
            this.fieldCollect.Set("DISCLOSURE_X10", this.Val("DISCLOSURE.X10"));
            break;
          case 3:
          case 4:
            this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X21"));
            this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X22"));
            string str5 = this.Val("DISCLOSURE.X23");
            if (str5 != string.Empty)
              str5 += ",";
            this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str5 + this.Val("DISCLOSURE.X24") + " " + this.Val("DISCLOSURE.X25"));
            this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X26"));
            this.fieldCollect.Set("DISCLOSURE_X9", this.Val("DISCLOSURE.X29"));
            this.fieldCollect.Set("DISCLOSURE_X10", this.Val("DISCLOSURE.X30"));
            break;
          case 5:
          case 6:
            this.fieldCollect.Set("DISCLOSURE_X1a", this.Val("DISCLOSURE.X41"));
            this.fieldCollect.Set("DISCLOSURE_X2", this.Val("DISCLOSURE.X42"));
            string str6 = this.Val("DISCLOSURE.X43");
            if (str6 != string.Empty)
              str6 += ",";
            this.fieldCollect.Set("DISCLOSURE_X3_X4_X5", str6 + this.Val("DISCLOSURE.X44") + " " + this.Val("DISCLOSURE.X45"));
            this.fieldCollect.Set("DISCLOSURE_X6", this.Val("DISCLOSURE.X46"));
            this.fieldCollect.Set("DISCLOSURE_X9", this.Val("DISCLOSURE.X49"));
            this.fieldCollect.Set("DISCLOSURE_X10", this.Val("DISCLOSURE.X50"));
            break;
        }
      }
      if (page == 2)
      {
        if (borrower == 1)
        {
          this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X11"));
          this.fieldCollect.Set("67", this.Val("67"));
          string val = this.Val("DISCLOSURE.X13") + "\r\n" + this.Val("DISCLOSURE.X14") + "\r\n" + this.Val("DISCLOSURE.X15") + "\r\n" + this.Val("DISCLOSURE.X16");
          if (this.Val("DISCLOSURE.X173") == "Y")
            val += "\r\n[X] Number of Recent Inquiries on Credt Report";
          this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
        }
        if (borrower == 3)
        {
          this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X31"));
          this.fieldCollect.Set("67", this.Val("1450"));
          string val = this.Val("DISCLOSURE.X33") + "\r\n" + this.Val("DISCLOSURE.X34") + "\r\n" + this.Val("DISCLOSURE.X35") + "\r\n" + this.Val("DISCLOSURE.X36");
          if (this.Val("DISCLOSURE.X174") == "Y")
            val += "\r\n[X] Number of Recent Inquiries on Credt Report";
          this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
        }
        if (borrower == 5)
        {
          this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X51"));
          this.fieldCollect.Set("67", this.Val("1414"));
          string val = this.Val("DISCLOSURE.X53") + "\r\n" + this.Val("DISCLOSURE.X54") + "\r\n" + this.Val("DISCLOSURE.X55") + "\r\n" + this.Val("DISCLOSURE.X56");
          if (this.Val("DISCLOSURE.X175") == "Y")
            val += "\r\n[X] Number of Recent Inquiries on Credt Report";
          this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
        }
        if (borrower == 2)
        {
          this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X12"));
          this.fieldCollect.Set("67", this.Val("60"));
          string val = this.Val("DISCLOSURE.X17") + "\r\n" + this.Val("DISCLOSURE.X18") + "\r\n" + this.Val("DISCLOSURE.X19") + "\r\n" + this.Val("DISCLOSURE.X20");
          if (this.Val("DISCLOSURE.X176") == "Y")
            val += "\r\n[X] Number of Recent Inquiries on Credt Report";
          this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
        }
        if (borrower == 4)
        {
          this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X32"));
          this.fieldCollect.Set("67", this.Val("1452"));
          string val = this.Val("DISCLOSURE.X37") + "\r\n" + this.Val("DISCLOSURE.X38") + "\r\n" + this.Val("DISCLOSURE.X39") + "\r\n" + this.Val("DISCLOSURE.X40");
          if (this.Val("DISCLOSURE.X177") == "Y")
            val += "\r\n[X] Number of Recent Inquiries on Credt Report";
          this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
        }
        if (borrower == 6)
        {
          this.fieldCollect.Set("DISCLOSURE_X11", this.Val("DISCLOSURE.X52"));
          this.fieldCollect.Set("67", this.Val("1415"));
          string val = this.Val("DISCLOSURE.X57") + "\r\n" + this.Val("DISCLOSURE.X58") + "\r\n" + this.Val("DISCLOSURE.X59") + "\r\n" + this.Val("DISCLOSURE.X60");
          if (this.Val("DISCLOSURE.X178") == "Y")
            val += "\r\n[X] Number of Recent Inquiries on Credt Report";
          this.fieldCollect.Set("DISCLOSURE_X173", Utils.StringWrapping(ref val, 75, 9, 1));
        }
      }
      this.pageDone = string.Empty;
    }

    private void printPrivacyPolicyPage1()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 65; index <= 71; ++index)
      {
        string val = this.fieldCollect.Get("NOTICES_X" + (object) index);
        if (string.Compare(val, "Yes - It is required/provides an Opt-Out", true) == 0)
          val = "Yes";
        else if (string.Compare(val, "No - Does not provide an Opt-Out", true) == 0)
          val = "No";
        else if (string.Compare(val, "No - We don't share", true) == 0)
          val = "We Don't Share";
        string str = Utils.StringWrapping(ref val, 30, 4, 1);
        this.fieldCollect.Set("NOTICES_X" + (object) index, str);
      }
      string val1 = this.Val("315");
      this.fieldCollect.Set("315b", Utils.StringWrapping(ref val1, 24, 1, 1));
      string val2 = val1.Trim();
      this.fieldCollect.Set("315c", Utils.StringWrapping(ref val2, 24, 1, 1));
    }

    private void printPrivacyPolicyPage2()
    {
      string val1 = this.Val("NOTICES.X72");
      this.fieldCollect.Set("NOTICES_X72", Utils.StringWrapping(ref val1, 70, 3, 1));
      string val2 = this.Val("NOTICES.X87");
      this.fieldCollect.Set("NOTICES_X87", Utils.StringWrapping(ref val2, 120, 30, 1));
      string val3 = this.fieldCollect.Get("NOTICES_X79");
      this.fieldCollect.Set("NOTICES_X79", Utils.StringWrapping(ref val3, 68, 3, 1));
      string val4 = this.fieldCollect.Get("NOTICES_X83");
      this.fieldCollect.Set("NOTICES_X83", Utils.StringWrapping(ref val4, 68, 3, 1));
      string val5 = this.fieldCollect.Get("NOTICES_X85");
      this.fieldCollect.Set("NOTICES_X85", Utils.StringWrapping(ref val5, 68, 3, 1));
      string val6 = this.fieldCollect.Get("NOTICES_X88");
      this.fieldCollect.Set("NOTICES_X88", Utils.StringWrapping(ref val6, 68, 3, 1));
      string val7 = this.Val("NOTICES.X94");
      if (val7 != string.Empty)
        val7 += ".";
      this.fieldCollect.Set("NOTICES_X94", Utils.StringWrapping(ref val7, 76, 2, 1));
      this.pageDone = string.Empty;
    }

    private void printCAPrivacyPolicy()
    {
      int num = 1;
      if (this.pageDone != string.Empty)
        num = Utils.ParseInt((object) this.pageDone);
      switch (num)
      {
        case 1:
          this.fieldCollect.Set("36_37", this.Val("36") + " " + this.Val("37"));
          if (this.Val("68") == string.Empty && this.Val("69") == string.Empty)
          {
            this.pageDone = string.Empty;
            return;
          }
          this.pageDone = "2";
          return;
        case 2:
          this.fieldCollect.Set("36_37", this.Val("68") + " " + this.Val("69"));
          break;
      }
      this.pageDone = string.Empty;
    }

    private void printAffiliateBusinessArrangements()
    {
      int numberOfAffiliates = this.loan.GetNumberOfAffiliates();
      int num1 = 1;
      if (this.pageDone != string.Empty)
        num1 = Utils.ParseInt((object) this.pageDone) + 1;
      string str1 = "AB" + num1.ToString("00");
      this.fieldCollect.Set("381", this.Val(str1 + "01"));
      this.fieldCollect.Set("383", this.Val(str1 + "02"));
      this.fieldCollect.Set("384_385_386", this.Val(str1 + "03") + (this.Val(str1 + "04") != string.Empty ? ", " : "") + this.Val(str1 + "04") + " " + this.Val(str1 + "05"));
      this.fieldCollect.Set("381_A", this.Val(str1 + "01"));
      this.fieldCollect.Set("381_AA", this.Val(str1 + "01"));
      this.fieldCollect.Set("381_AAA", this.Val(str1 + "01"));
      this.fieldCollect.Set("AFFX2", this.Val(str1 + "06"));
      this.fieldCollect.Set("AFFX3", this.Val(str1 + "07") + (this.Val(str1 + "28") != string.Empty ? " " + this.Val(str1 + "28") + "%" : ""));
      this.fieldCollect.Set("AFFX5", this.Val(str1 + "08") == "Y" ? "X" : "");
      this.fieldCollect.Set("AFFX6", this.Val(str1 + "09") == "Y" ? "X" : "");
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index1 = 1; index1 <= 2; ++index1)
      {
        int num2 = index1 == 1 ? 11360 : 11373;
        for (int index2 = 22; index2 <= 27; ++index2)
        {
          if (index1 == 1 && this.Val(str1 + (object) index2) != "Y" || index1 == 2 && this.Val(str1 + (object) index2) == "Y")
          {
            string str2 = str1;
            int num3 = index2 - 12;
            string str3 = num3.ToString("00");
            string id1 = str2 + str3;
            string str4 = str1;
            num3 = index2 - 6;
            string str5 = num3.ToString("00");
            string id2 = str4 + str5;
            if (this.Val(id1) != string.Empty || this.Val(id2) != string.Empty)
            {
              this.fieldCollect.Set(string.Concat((object) num2), this.Val(id1));
              int num4 = num2 + 1;
              this.fieldCollect.Set(string.Concat((object) num4), this.Val(id2));
              num2 = num4 + 1;
            }
          }
        }
        if (index1 == 1 && num2 > 11360)
          this.fieldCollect.Set("AFFX4", "X");
        else if (index1 == 2 && num2 > 11373)
          this.fieldCollect.Set("AFFX31", "X");
      }
      if (num1 >= numberOfAffiliates)
        this.pageDone = string.Empty;
      else
        this.pageDone = string.Concat((object) num1);
    }

    private void printAnnualAndRepaymentIncomeWorksheet(string formID)
    {
      if (formID.EndsWith("1"))
      {
        for (int index = 209; index <= 213; ++index)
        {
          string val = this.Val("USDA.X" + (object) index);
          this.fieldCollect.Set("USDA_X" + (object) index, Utils.StringWrapping(ref val, 110, 5, 1));
        }
      }
      else if (formID.EndsWith("2"))
      {
        for (int index = 214; index <= 216; ++index)
        {
          string val = this.Val("USDA.X" + (object) index);
          this.fieldCollect.Set("USDA_X" + (object) index, Utils.StringWrapping(ref val, 110, 10, 1));
        }
      }
      else
      {
        string val = this.Val("USDA.X193");
        this.fieldCollect.Set("USDA_X193", Utils.StringWrapping(ref val, 42, 10, 1));
        val = this.Val("USDA.X200");
        this.fieldCollect.Set("USDA_X200", Utils.StringWrapping(ref val, 42, 10, 1));
        val = this.Val("USDA.X203");
        this.fieldCollect.Set("USDA_X203", Utils.StringWrapping(ref val, 42, 10, 1));
        val = this.Val("USDA.X205");
        this.fieldCollect.Set("USDA_X205", Utils.StringWrapping(ref val, 42, 10, 1));
      }
    }

    private void printRD3555(string formID)
    {
      if (formID.EndsWith("Page 3"))
      {
        for (int index = 209; index <= 213; ++index)
        {
          string val = this.Val("USDA.X" + (object) index);
          this.fieldCollect.Set("USDA_X" + (object) index, Utils.StringWrapping(ref val, 102, 4, 1));
        }
      }
      else if (formID.EndsWith("Page 4"))
      {
        for (int index = 214; index <= 216; ++index)
        {
          string val = this.Val("USDA.X" + (object) index);
          this.fieldCollect.Set("USDA_X" + (object) index, Utils.StringWrapping(ref val, 90, 10, 1));
        }
      }
      else
      {
        if (!formID.EndsWith("Page 5"))
          return;
        string val1 = this.Val("USDA.X193");
        this.fieldCollect.Set("USDA_X193", Utils.StringWrapping(ref val1, 40, 10, 1));
        string val2 = this.Val("USDA.X200");
        this.fieldCollect.Set("USDA_X200", Utils.StringWrapping(ref val2, 40, 10, 1));
        string val3 = this.Val("USDA.X203");
        this.fieldCollect.Set("USDA_X203", Utils.StringWrapping(ref val3, 40, 10, 1));
        val3 = this.Val("USDA.X205");
        this.fieldCollect.Set("USDA_X205", Utils.StringWrapping(ref val3, 40, 10, 1));
      }
    }

    private void printHomeownershipCounseling(int pageNo)
    {
      int[] counselingProviders = this.loan.GetSelectedHomeCounselingProviders();
      int num1 = 1;
      if (this.pageDone != string.Empty)
        num1 = Utils.ParseInt((object) this.pageDone);
      int length = counselingProviders.Length;
      if (length > 0)
      {
        int num2 = length / 4 + 1 + (length % 4 > 0 ? 1 : 0);
        if (pageNo == 1)
          this.fieldCollect.Set("pageno", "1 of " + (object) num2);
        else
          this.fieldCollect.Set("pageno", (num1 / 4 + 2).ToString() + " of " + (object) num2);
      }
      else if (pageNo == 1)
        this.fieldCollect.Set("pageno", "1 of 1");
      if (pageNo == 1)
      {
        this.pageDone = "";
      }
      else
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        string empty4 = string.Empty;
        int num3 = 0;
        for (int index1 = num1; index1 <= counselingProviders.Length; ++index1)
        {
          if (num3 >= 4)
          {
            this.pageDone = string.Concat((object) index1);
            return;
          }
          string str1 = "HC" + counselingProviders[index1 - 1].ToString("00");
          string str2 = "HC" + (num3 + 1).ToString("00");
          string val = this.Val(str1 + "02");
          this.fieldCollect.Set(str2 + "02", Utils.StringWrapping(ref val, 31, 3, 1));
          this.fieldCollect.Set(str2 + "03", this.Val(str1 + "03"));
          string str3 = this.Val(str1 + "04") + (this.Val(str1 + "05") != string.Empty ? ", " : "") + this.Val(str1 + "05") + " " + this.Val(str1 + "06");
          this.fieldCollect.Set(str2 + "04", str3);
          for (int index2 = 7; index2 <= 11; ++index2)
            this.fieldCollect.Set(str2 + index2.ToString("00"), this.Val(str1 + index2.ToString("00")));
          this.fieldCollect.Set(str2 + "15", this.Val(str1 + "15"));
          val = this.Val(str1 + "12");
          this.fieldCollect.Set(str2 + "12", Utils.StringWrapping(ref val, 50, 6, 1));
          this.fieldCollect.Set(str2 + "17", this.Val(str1 + "17"));
          val = this.Val(str1 + "13");
          this.fieldCollect.Set(str2 + "13", Utils.StringWrapping(ref val, 60, 20, 1));
          ++num3;
        }
        this.pageDone = "";
      }
    }

    private void printVA261820(int pageNo)
    {
      if (pageNo == 1)
      {
        string field1 = this.loan.GetField("VASUMM.X61");
        this.fieldCollect.Set("VASUMM_X61", Utils.StringWrapping(ref field1, 106, 2, 1));
        string field2 = this.loan.GetField("VASUMM.X62");
        this.fieldCollect.Set("VASUMM_X62", Utils.StringWrapping(ref field2, 160, 2, 1));
      }
      else if (this.GetFieldFromCal("VASUMM.X31") == "Borrower")
      {
        this.fieldCollect.Set("1523_1", this.loan.GetField("1523") == "Hispanic or Latino" ? "X" : "");
        this.fieldCollect.Set("1523_2", this.loan.GetField("1523") == "Not Hispanic or Latino" ? "X" : "");
        for (int index = 1524; index <= 1528; ++index)
          this.fieldCollect.Set(string.Concat((object) index), this.loan.GetField(string.Concat((object) index)) == "Y" ? "X" : "");
        this.fieldCollect.Set("471_1", this.loan.GetField("471") == "Male" ? "X" : "");
        this.fieldCollect.Set("471_2", this.loan.GetField("471") == "Female" ? "X" : "");
        this.fieldCollect.Set("1531_1", this.loan.GetField("1531") == "Hispanic or Latino" ? "X" : "");
        this.fieldCollect.Set("1531_2", this.loan.GetField("1531") == "Not Hispanic or Latino" ? "X" : "");
        for (int index = 1524; index <= 1528; ++index)
          this.fieldCollect.Set(string.Concat((object) (index + 8)), this.loan.GetField(string.Concat((object) (index + 8))) == "Y" ? "X" : "");
        this.fieldCollect.Set("478_1", this.loan.GetField("478") == "Male" ? "X" : "");
        this.fieldCollect.Set("478_2", this.loan.GetField("478") == "Female" ? "X" : "");
      }
      else
      {
        if (!(this.GetFieldFromCal("VASUMM.X31") == "CoBorrower"))
          return;
        this.fieldCollect.Set("1523_1", this.loan.GetField("1531") == "Hispanic or Latino" ? "X" : "");
        this.fieldCollect.Set("1523_2", this.loan.GetField("1531") == "Not Hispanic or Latino" ? "X" : "");
        for (int index = 1532; index <= 1536; ++index)
          this.fieldCollect.Set(string.Concat((object) (index - 8)), this.loan.GetField(string.Concat((object) index)) == "Y" ? "X" : "");
        this.fieldCollect.Set("471_1", this.loan.GetField("478") == "Male" ? "X" : "");
        this.fieldCollect.Set("471_2", this.loan.GetField("478") == "Female" ? "X" : "");
        this.fieldCollect.Set("1531_1", this.loan.GetField("1523") == "Hispanic or Latino" ? "X" : "");
        this.fieldCollect.Set("1531_2", this.loan.GetField("1523") == "Not Hispanic or Latino" ? "X" : "");
        for (int index = 1524; index <= 1528; ++index)
          this.fieldCollect.Set(string.Concat((object) (index + 8)), this.loan.GetField(string.Concat((object) index)) == "Y" ? "X" : "");
        this.fieldCollect.Set("478_1", this.loan.GetField("471") == "Male" ? "X" : "");
        this.fieldCollect.Set("478_2", this.loan.GetField("471") == "Female" ? "X" : "");
      }
    }

    private void printFactActDisclosure(bool firstPage)
    {
      string empty = string.Empty;
      int maxLength = 100;
      int maxLine = 10;
      if (firstPage)
      {
        string val1 = this.loan.GetField("DISCLOSURE.X13") + "\r\n" + this.loan.GetField("DISCLOSURE.X14") + "\r\n" + this.loan.GetField("DISCLOSURE.X15") + "\r\n" + this.loan.GetField("DISCLOSURE.X16");
        this.fieldCollect.Set("DISCLOSURE_X13_X14_X15_X16", Utils.StringWrapping(ref val1, maxLength, maxLine, 1));
        string val2 = this.loan.GetField("DISCLOSURE.X17") + "\r\n" + this.loan.GetField("DISCLOSURE.X18") + "\r\n" + this.loan.GetField("DISCLOSURE.X19") + "\r\n" + this.loan.GetField("DISCLOSURE.X20");
        this.fieldCollect.Set("DISCLOSURE_X17_X18_X19_X20", Utils.StringWrapping(ref val2, maxLength, maxLine, 1));
      }
      else
      {
        string val3 = this.loan.GetField("DISCLOSURE.X33") + "\r\n" + this.loan.GetField("DISCLOSURE.X34") + "\r\n" + this.loan.GetField("DISCLOSURE.X35") + "\r\n" + this.loan.GetField("DISCLOSURE.X36");
        this.fieldCollect.Set("DISCLOSURE_X33_X34_X35_X36", Utils.StringWrapping(ref val3, maxLength, maxLine, 1));
        string val4 = this.loan.GetField("DISCLOSURE.X37") + "\r\n" + this.loan.GetField("DISCLOSURE.X38") + "\r\n" + this.loan.GetField("DISCLOSURE.X39") + "\r\n" + this.loan.GetField("DISCLOSURE.X40");
        this.fieldCollect.Set("DISCLOSURE_X37_X38_X39_X40", Utils.StringWrapping(ref val4, maxLength, maxLine, 1));
        string val5 = this.loan.GetField("DISCLOSURE.X53") + "\r\n" + this.loan.GetField("DISCLOSURE.X54") + "\r\n" + this.loan.GetField("DISCLOSURE.X55") + "\r\n" + this.loan.GetField("DISCLOSURE.X56");
        this.fieldCollect.Set("DISCLOSURE_X53_X54_X55_X56", Utils.StringWrapping(ref val5, maxLength, maxLine, 1));
        string val6 = this.loan.GetField("DISCLOSURE.X57") + "\r\n" + this.loan.GetField("DISCLOSURE.X58") + "\r\n" + this.loan.GetField("DISCLOSURE.X59") + "\r\n" + this.loan.GetField("DISCLOSURE.X60");
        this.fieldCollect.Set("DISCLOSURE_X57_X58_X59_X60", Utils.StringWrapping(ref val6, maxLength, maxLine, 1));
      }
      this.pageDone = string.Empty;
    }

    private void printVALAAddendum()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int num1 = 8;
      if (this.pageDone != string.Empty)
        num1 = Utils.ParseInt((object) this.pageDone);
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      bool flag = this.loan.GetField("VALA.X28") == "Y";
      for (int index = num1; index <= exlcudingAlimonyJobExp; ++index)
      {
        int num5 = (index - num1) * 4;
        if (num5 >= 248)
        {
          this.fieldCollect.Set("249", num2.ToString("N2"));
          this.fieldCollect.Set("250", num4.ToString("N2"));
          this.pageDone = string.Concat((object) index);
          return;
        }
        num2 = Utils.ParseDouble((object) this.loan.GetField("FL" + index.ToString("00") + "11"));
        double num6 = Utils.ParseDouble((object) this.loan.GetField("FL" + index.ToString("00") + "13"));
        this.fieldCollect.Set(string.Concat((object) (num5 + 1)), this.loan.GetField("FL" + index.ToString("00") + "02"));
        this.fieldCollect.Set(string.Concat((object) (num5 + 2)), flag ? "X" : "");
        this.fieldCollect.Set(string.Concat((object) (num5 + 3)), num2 == 0.0 ? "" : num2.ToString("N2"));
        this.fieldCollect.Set(string.Concat((object) (num5 + 4)), num6 == 0.0 ? "" : num6.ToString("N2"));
        num3 += num2;
        num4 += num6;
      }
      this.fieldCollect.Set("249", num3.ToString("N2"));
      this.fieldCollect.Set("250", num4.ToString("N2"));
      this.pageDone = string.Empty;
    }

    private void printVAELIG()
    {
      this.fieldCollect.Set("VA_X9A_NO", "X");
      for (int index = 2; index <= 12; index += 5)
      {
        if (this.Val("VAELIG.X" + (object) index) != "" && this.Val("VAELIG.X" + (object) index) != "//" && (this.Val("VAELIG.X" + (object) (index + 1)) == "" || this.Val("VAELIG.X" + (object) (index + 1)) == "//"))
        {
          this.fieldCollect.Set("VA_X9A_YES", "X");
          this.fieldCollect.Set("VA_X9A_NO", "");
          break;
        }
      }
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      int[] numArray = new int[6]{ 24, 30, 36, 42, 48, 60 };
      for (int index = 0; index < numArray.Length; ++index)
      {
        string name;
        switch (this.Val("VAELIG.X" + (object) numArray[index]))
        {
          case "VA-Guaranteed Loan":
            ++num1;
            name = "VAELIG_X10A_" + (object) num1;
            flag1 = true;
            break;
          case "One-Time Only Restoration":
            ++num2;
            name = "VAELIG_X11A_" + (object) num2;
            flag2 = true;
            break;
          case "Regular Cash-Out Refinance":
            ++num3;
            name = "VAELIG_X12A_" + (object) num3;
            flag3 = true;
            break;
          case "Regular None Cash-Out Refinance":
            ++num4;
            name = "VAELIG_X13A_" + (object) num4;
            flag4 = true;
            break;
          default:
            continue;
        }
        string str1 = this.Val("VAELIG.X" + (object) (numArray[index] + 2));
        string str2 = this.Val("VAELIG.X" + (object) (numArray[index] + 1));
        int num5 = index * 3 + 79;
        string str3 = this.Val("VAELIG.X" + (object) num5) + (this.Val("VAELIG.X" + (object) num5) != string.Empty ? ", " : "") + this.Val("VAELIG.X" + (object) (num5 + 1)) + " " + this.Val("VAELIG.X" + (object) (num5 + 2));
        this.fieldCollect.Set(name, str1);
        this.fieldCollect.Set(name + "a", str2);
        this.fieldCollect.Set(name + "b", str3);
      }
      if (!flag1 && this.Val("VAELIG.X113") != "Y")
        this.fieldCollect.Set("VAELIG_X10A_NO", "X");
      else if (flag1)
        this.fieldCollect.Set("VAELIG_X10A_YES", "X");
      if (!flag2)
        this.fieldCollect.Set("VAELIG_X11A_NO", "X");
      else
        this.fieldCollect.Set("VAELIG_X11A_YES", "X");
      if (!flag3)
        this.fieldCollect.Set("VAELIG_X12A_NO", "X");
      else
        this.fieldCollect.Set("VAELIG_X12A_YES", "X");
      if (!flag4)
        this.fieldCollect.Set("VAELIG_X13A_NO", "X");
      else
        this.fieldCollect.Set("VAELIG_X13A_YES", "X");
      this.pageDone = string.Empty;
    }

    private void printEEM()
    {
      string str = string.Empty;
      for (int index = 1216; index <= 1222; ++index)
        str = str + (str != string.Empty ? " " : "") + this.loan.GetField(string.Concat((object) index)).Trim();
      string val = str + (str != string.Empty ? " " : "") + this.loan.GetField("1829").Trim();
      this.fieldCollect.Set("1216", Utils.StringWrapping(ref val, 100, 10, 1));
    }

    private void printNetTangibleBenefit(string formID)
    {
      int maxLength = 85;
      if (string.Compare(formID, "NET TANGIBLE BENEFIT WORKSHEET PAGE 2", true) == 0)
        maxLength = 105;
      string field = this.loan.GetField("NTB.X58");
      this.fieldCollect.Set("NTB_X58", Utils.StringWrapping(ref field, maxLength, 10, 1));
      field = this.loan.GetField("NTB.X59");
      this.fieldCollect.Set("NTB_X59", Utils.StringWrapping(ref field, maxLength, 10, 1));
      this.pageDone = string.Empty;
    }

    private void printRE882Page2()
    {
      int num1 = Utils.ParseInt((object) this.Val("RE88395.X316"));
      if (num1 == -1)
        return;
      int num2 = num1 % 12;
      int num3 = (num1 - num2) / 12;
      NameValueCollection fieldCollect = this.fieldCollect;
      string str1 = num3 > 0 ? num3.ToString() + (num2 > 0 ? (object) (" year" + (num3 > 1 ? "s" : "")) : (object) "") : "";
      string str2;
      if (num2 <= 0)
        str2 = "";
      else
        str2 = " " + (object) num2 + " month" + (num2 > 1 ? (object) "s" : (object) "");
      string str3 = str1 + str2;
      fieldCollect.Set("RE88395X316", str3);
      this.pageDone = string.Empty;
    }

    private void printLoanOptionsDisclosure()
    {
      string val1 = this.Val("DISCLOSURE.X734");
      this.fieldCollect.Set("DISCLOSUREX734", Utils.StringWrapping(ref val1, 110, 20, 1));
      string val2 = this.Val("DISCLOSURE.X688");
      this.fieldCollect.Set("DISCLOSUREX688", Utils.StringWrapping(ref val2, 20, 5, 1));
      string val3 = this.Val("DISCLOSURE.X701");
      this.fieldCollect.Set("DISCLOSUREX701", Utils.StringWrapping(ref val3, 20, 5, 1));
      string val4 = this.Val("DISCLOSURE.X708");
      this.fieldCollect.Set("DISCLOSUREX708", Utils.StringWrapping(ref val4, 20, 5, 1));
      string val5 = this.Val("DISCLOSURE.X720");
      this.fieldCollect.Set("DISCLOSUREX720", Utils.StringWrapping(ref val5, 20, 5, 1));
    }

    private void printAcknowledgementForms()
    {
      int num = 0;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        for (int index2 = 1; index2 <= 2; ++index2)
        {
          string str1 = string.Empty;
          if (index2 == 1)
          {
            if (borrowerPairs[index1].Borrower.FirstName != string.Empty || borrowerPairs[index1].Borrower.LastName != string.Empty)
              str1 = borrowerPairs[index1].Borrower.FirstName + " " + borrowerPairs[index1].Borrower.LastName;
          }
          else if (borrowerPairs[index1].CoBorrower.FirstName != string.Empty || borrowerPairs[index1].CoBorrower.LastName != string.Empty)
            str1 = borrowerPairs[index1].CoBorrower.FirstName + " " + borrowerPairs[index1].CoBorrower.LastName;
          string str2 = str1.Trim();
          if (!(str2 == string.Empty))
          {
            ++num;
            this.fieldCollect.Set("BORROWER_" + (object) num, str2);
            this.fieldCollect.Set("BORROWER_a_" + (object) num, str2);
            if (num >= 6)
              return;
          }
        }
      }
    }

    private void printSection32()
    {
      PaymentSchedule[] schedule = this.calObjs.RegzCal.Schedule;
      int numberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      if (schedule != null)
        this.createPaymentScheduleTables(schedule, this.calObjs.RegzCal.NumberOfTerm, 0);
      double num1 = Utils.ParseDouble((object) this.loan.GetField("325"));
      double num2 = Utils.ParseDouble((object) this.loan.GetField("4"));
      if (num1 < num2 && num1 != 0.0 && numberOfTerm > 1)
        this.fieldCollect.Set("S32DISC_X8", "At the end of your loan, you will still owe us: $" + schedule[numberOfTerm - 1].Principal.ToString("N2"));
      if (!(this.loan.GetField("608") == "AdjustableRate"))
        return;
      int num3 = 0;
      double num4 = 0.0;
      for (int index = 0; index < numberOfTerm; ++index)
      {
        if (schedule[index].Payment > num4)
        {
          num4 = schedule[index].Payment;
          num3 = index;
        }
      }
      if (num3 <= 0 || num4 <= 0.0)
        return;
      int num5 = (num3 - num3 % 12) / 12 + 1;
      this.fieldCollect.Set("S32DISC_X6", num4.ToString("N2"));
      string empty = string.Empty;
      string str;
      switch (num5 % 10)
      {
        case 1:
          str = num5.ToString() + "st";
          break;
        case 2:
          str = num5.ToString() + "nd";
          break;
        case 3:
          str = num5.ToString() + "rd";
          break;
        default:
          str = num5.ToString() + "th";
          break;
      }
      if (num5 >= 11 && num5 <= 13)
        str = num5.ToString() + "th";
      this.fieldCollect.Set("S32DISC_X7", str);
    }

    private void printREGZTIL()
    {
      this.populatePaymentSchedule();
      string str1 = string.Empty;
      switch (this.loan.GetField("19"))
      {
        case "ConstructionToPermanent":
          str1 = "Interest on the amount of credit outstanding during the construction period will be paid monthly starting " + this.loan.GetField("682") + ".";
          break;
        case "ConstructionOnly":
          str1 = "Interest on the amount of credit outstanding will be paid monthly starting " + this.loan.GetField("682") + ".";
          break;
      }
      this.fieldCollect.Set("SX121", str1);
      this.checkTodayDate("3122");
      if (!(this.loan.GetField("1172") == "FHA"))
        return;
      string str2 = "IF YOU PREPAY YOUR LOAN OTHER THAN ON THE REGULAR INSTALLMENT DATE YOU MAY BE ASSESSED INTEREST CHARGES UNTIL THE END OF THE MONTH AND YOU MAY BE ENTITLED TO A REFUND OF PART OF THE FINANCE CHARGE.";
      DateTime date1 = Utils.ParseDate((object) this.loan.GetField("745"));
      DateTime date2 = Utils.ParseDate((object) "01/21/2015").Date;
      if (DateTime.Compare(date2, date1.Date) <= 0)
      {
        str2 = "";
      }
      else
      {
        DateTime date3 = Utils.ParseDate((object) this.loan.GetField("763"));
        if (DateTime.Compare(date2, date3.Date) <= 0)
        {
          str2 = "";
        }
        else
        {
          DateTime date4 = Utils.ParseDate((object) this.loan.GetField("748"));
          if (DateTime.Compare(date2, date4.Date) <= 0)
            str2 = "";
        }
      }
      this.fieldCollect.Set("FHA_Message", str2);
    }

    private void populatePaymentSchedule()
    {
      string field = this.loan.GetField("19");
      RegzSummaryTableType regzSummaryType = this.loan.Calculator.RegzSummaryType;
      if (field == "ConstructionToPermanent")
      {
        this.fieldCollect.Set("STATEMENT_AT_TOP1", "Repayment: Interest on the amount outstanding during the construction period will be paid " + (this.Val("423") == "Biweekly" ? "biweekly" : "monthly") + " and may");
        this.fieldCollect.Set("STATEMENT_AT_TOP2", "vary, followed by:");
      }
      int lineCount = 0;
      if ((field == "ConstructionToPermanent" || field == "ConstructionOnly") && this.Val("SYS.X6") == "A")
      {
        this.fieldCollect.Set("CONSTRUCTIONONLY", "Interest on outstanding credit during construction will be paid monthly varying.");
        lineCount = 1;
      }
      PaymentSchedule[] schedule = this.calObjs.RegzCal.Schedule;
      int numberOfTerm = this.calObjs.RegzCal.NumberOfTerm;
      if (schedule != null && (regzSummaryType == RegzSummaryTableType.None || regzSummaryType == RegzSummaryTableType.InvalidConstIO || regzSummaryType == RegzSummaryTableType.InvalidNegARM || regzSummaryType == RegzSummaryTableType.InvalidPermB))
        this.createPaymentScheduleTables(schedule, numberOfTerm, lineCount);
      if (this.FltVal("3288") > 0.0)
        this.fieldCollect.Set("STATEMENT_AT_BOTTOM", "Final Balloon Payment due on " + this.Val("3287") + " : $ " + this.FltVal("3288").ToString("N2"));
      if (regzSummaryType == RegzSummaryTableType.ARMLess5Years || regzSummaryType == RegzSummaryTableType.ARMGreater5Years || regzSummaryType == RegzSummaryTableType.ARMIntOnly || regzSummaryType == RegzSummaryTableType.ARMIntOnly31 || regzSummaryType == RegzSummaryTableType.ARMIO_L60 || regzSummaryType == RegzSummaryTableType.ARMIntOnly51 || regzSummaryType == RegzSummaryTableType.ARMIntOnly7_1or10_1 || regzSummaryType == RegzSummaryTableType.ARMIntOnly3C)
      {
        if (this.FltVal("3296") == 0.0 && this.Val("608") == "AdjustableRate")
        {
          this.calObjs.RegzCal.CalcRateCap();
          this.fieldCollect.Set("3296", this.Val("3296"));
        }
        if (this.FltVal("3") >= this.FltVal("3296"))
        {
          this.fieldCollect.Set("3_a", "N/A");
          this.fieldCollect.Set("3296", "N/A");
          this.fieldCollect.Set("696_a", "N/A");
          this.fieldCollect.Set("3289", "N/A");
        }
      }
      if (this.Val("3269") == string.Empty)
      {
        this.fieldCollect.Set("3269", "N/A");
        this.fieldCollect.Set("3294", "N/A");
        this.fieldCollect.Set("3270", "N/A");
        this.fieldCollect.Set("3271", "N/A");
        this.fieldCollect.Set("3272", "N/A");
        this.fieldCollect.Set("3273", "N/A");
      }
      if (regzSummaryType == RegzSummaryTableType.Buydown && this.Val("3274") == string.Empty)
      {
        this.fieldCollect.Set("3274", "N/A");
        this.fieldCollect.Set("3275", "N/A");
        this.fieldCollect.Set("3285", "N/A");
        this.fieldCollect.Set("3278", "N/A");
        this.fieldCollect.Set("3279", "N/A");
      }
      if (this.Val("3280") == string.Empty)
      {
        this.fieldCollect.Set("3280", "N/A");
        switch (regzSummaryType)
        {
          case RegzSummaryTableType.FixedIntOnly:
            this.fieldCollect.Set("3_a", "N/A");
            break;
          case RegzSummaryTableType.FixedBalloonIntOnlyGreater:
          case RegzSummaryTableType.FixedBalloonIntOnlyLesser:
          case RegzSummaryTableType.ConstOnlyA:
          case RegzSummaryTableType.ConstOnlyB:
            this.fieldCollect.Set("3295", "N/A");
            break;
        }
        this.fieldCollect.Set("3281", "N/A");
        this.fieldCollect.Set("3282", "N/A");
        this.fieldCollect.Set("3283", "N/A");
        this.fieldCollect.Set("3284", "N/A");
      }
      if (this.Val("3276") == string.Empty && this.Val("3274") != string.Empty)
        this.fieldCollect.Set("3276", "-none-");
      if (this.Val("3281") == string.Empty && this.Val("3280") != string.Empty)
        this.fieldCollect.Set("3281", "-none-");
      double num1 = 0.0;
      double num2;
      if (this.Val("3876") == string.Empty)
      {
        string lower = this.loan.GetField("1719").ToLower();
        if (lower == "of the payment" || lower == "of any installment")
          num1 = this.ToDouble(this.loan.GetField("912"));
        else if (lower == "of the overdue payment")
          num1 = this.ToDouble(this.loan.GetField("5"));
        else if (lower == "of the interest payment due" && numberOfTerm > 0)
          num1 = schedule[0].Interest;
        else if (lower == "of the principal and interest overdue" && numberOfTerm > 0)
          num1 = schedule[0].Interest + schedule[0].Principal;
        num2 = Utils.ArithmeticRounding(num1 * (this.ToDouble(this.loan.GetField("674")) * 0.01), 2);
      }
      else
        num2 = this.FltVal("3876");
      if (num2 != 0.0)
        this.fieldCollect.Set("REGZ50_1", num2.ToString("N2"));
      else
        this.fieldCollect.Set("REGZ50_1", "");
    }

    private void createPaymentScheduleTables(
      PaymentSchedule[] paySchedule,
      int loanPeriods,
      int lineCount)
    {
      int periodCount = 0;
      double num1 = 0.0;
      int num2 = 0;
      string dueInDate = CalculationBase.nil;
      double monthlyPay = 0.0;
      double num3 = 0.0;
      for (int index = 0; index < loanPeriods; ++index)
      {
        if (index == 0)
        {
          dueInDate = paySchedule[index].PayDate;
          monthlyPay = paySchedule[index].Payment;
          num3 = paySchedule[index].CurrentRate;
        }
        num1 += paySchedule[index].Payment;
        if (monthlyPay != paySchedule[index].Payment || num3 != paySchedule[index].CurrentRate)
        {
          lineCount = this.populateTable(lineCount, periodCount, monthlyPay, dueInDate);
          num2 += periodCount;
          dueInDate = paySchedule[index].PayDate;
          periodCount = 0;
        }
        ++periodCount;
        monthlyPay = paySchedule[index].Payment;
        num3 = paySchedule[index].CurrentRate;
      }
      int num4 = num2 + periodCount;
      if (loanPeriods > 0)
        lineCount = this.populateTable(lineCount, periodCount, monthlyPay, dueInDate);
      ++lineCount;
      for (int index = lineCount; index <= 36; ++index)
      {
        this.fieldCollect.Set("SX" + (object) ((index - 1) * 3 + 1), "");
        this.fieldCollect.Set("SX" + (object) ((index - 1) * 3 + 2), "");
        this.fieldCollect.Set("SX" + (object) ((index - 1) * 3 + 3), "");
      }
    }

    private int populateTable(int line, int periodCount, double monthlyPay, string dueInDate)
    {
      if (line >= 36)
        return 999;
      ++line;
      this.fieldCollect.Set("SX" + (object) ((line - 1) * 3 + 1), periodCount.ToString());
      if (monthlyPay != 0.0)
        this.fieldCollect.Set("SX" + (object) ((line - 1) * 3 + 2), monthlyPay.ToString("N2"));
      else
        this.fieldCollect.Set("SX" + (object) ((line - 1) * 3 + 2), "0.00");
      this.fieldCollect.Set("SX" + (object) ((line - 1) * 3 + 3), dueInDate);
      return line;
    }

    private void printVerifs(string form, int block)
    {
      string blockID = "";
      switch (form)
      {
        case "VOD":
          blockID = "DD" + block.ToString("00");
          switch (this.loan.GetField(blockID + "24"))
          {
            case "Borrower":
              this.fieldCollect.Set("FD0105", this.getBorrowerInfo(true, 1));
              this.fieldCollect.Set("FD0106", this.getBorrowerInfo(true, 3));
              this.fieldCollect.Set("FD0107", "");
              this.fieldCollect.Set("FD0108", "");
              break;
            case "CoBorrower":
              this.fieldCollect.Set("FD0105", this.getBorrowerInfo(false, 1));
              this.fieldCollect.Set("FD0106", this.getBorrowerInfo(false, 3));
              this.fieldCollect.Set("FD0107", "");
              this.fieldCollect.Set("FD0108", "");
              break;
            case "Both":
              this.fieldCollect.Set("FD0105", this.getBorrowerInfo(true, 1));
              this.fieldCollect.Set("FD0106", this.getBorrowerInfo(true, 3));
              this.fieldCollect.Set("FD0107", this.getBorrowerInfo(false, 1));
              this.fieldCollect.Set("FD0108", this.getBorrowerInfo(false, 3));
              break;
          }
          this.fieldCollect.Set("FD0101", this.loan.GetField(blockID + "02"));
          this.fieldCollect.Set("FD0102", this.loan.GetField(blockID + "03"));
          this.fieldCollect.Set("FD0103", this.loan.GetField(blockID + "04"));
          this.fieldCollect.Set("FD0104", this.loan.GetField(blockID + "05").Trim() + ", " + this.loan.GetField(blockID + "06").Trim() + " " + this.loan.GetField(blockID + "07").Trim());
          this.fieldCollect.Set("FD0109", this.translateDepositType(this.loan.GetField(blockID + "08")));
          this.fieldCollect.Set("FD0113", this.translateDepositType(this.loan.GetField(blockID + "12")));
          this.fieldCollect.Set("FD0117", this.translateDepositType(this.loan.GetField(blockID + "16")));
          this.fieldCollect.Set("FD0121", this.translateDepositType(this.loan.GetField(blockID + "20")));
          this.fieldCollect.Set("FD0110", this.loan.GetField(blockID + "09"));
          this.fieldCollect.Set("FD0114", this.loan.GetField(blockID + "13"));
          this.fieldCollect.Set("FD0118", this.loan.GetField(blockID + "17"));
          this.fieldCollect.Set("FD0122", this.loan.GetField(blockID + "21"));
          this.fieldCollect.Set("FD0111", this.loan.GetField(blockID + "10"));
          this.fieldCollect.Set("FD0115", this.loan.GetField(blockID + "14"));
          this.fieldCollect.Set("FD0119", this.loan.GetField(blockID + "18"));
          this.fieldCollect.Set("FD0123", this.loan.GetField(blockID + "22"));
          this.fieldCollect.Set("FD0112", this.loan.GetField(blockID + "11"));
          this.fieldCollect.Set("FD0116", this.loan.GetField(blockID + "15"));
          this.fieldCollect.Set("FD0120", this.loan.GetField(blockID + "19"));
          this.fieldCollect.Set("FD0124", this.loan.GetField(blockID + "23"));
          string field1 = this.loan.GetField(blockID + "26");
          if (field1 != string.Empty)
            this.fieldCollect.Set("FD0126", "Phone  " + field1);
          string field2 = this.loan.GetField(blockID + "27");
          if (field2 != string.Empty)
            this.fieldCollect.Set("FD0127", "Fax  " + field2);
          string str1 = this.loan.GetField(blockID + "98").Trim();
          if (str1 == null || str1 == "//")
            str1 = string.Empty;
          this.fieldCollect.Set("363", str1);
          break;
        case "VOL":
          blockID = "FL" + block.ToString("00");
          switch (this.loan.GetField(blockID + "15"))
          {
            case "Borrower":
              this.fieldCollect.Set("FL0105", this.getBorrowerInfo(true, 1));
              this.fieldCollect.Set("FL0106", this.getBorrowerInfo(true, 3));
              this.fieldCollect.Set("FL0107", "");
              this.fieldCollect.Set("FL0108", "");
              break;
            case "CoBorrower":
              this.fieldCollect.Set("FL0105", this.getBorrowerInfo(false, 1));
              this.fieldCollect.Set("FL0106", this.getBorrowerInfo(false, 3));
              this.fieldCollect.Set("FL0107", "");
              this.fieldCollect.Set("FL0108", "");
              break;
            case "Both":
              this.fieldCollect.Set("FL0105", this.getBorrowerInfo(true, 1));
              this.fieldCollect.Set("FL0106", this.getBorrowerInfo(true, 3));
              this.fieldCollect.Set("FL0107", this.getBorrowerInfo(false, 1));
              this.fieldCollect.Set("FL0108", this.getBorrowerInfo(false, 3));
              break;
          }
          this.fieldCollect.Set("FL0101", this.loan.GetField(blockID + "02"));
          this.fieldCollect.Set("FL0102", this.loan.GetField(blockID + "03"));
          this.fieldCollect.Set("FL0103", this.loan.GetField(blockID + "04"));
          this.fieldCollect.Set("FL0104", this.loan.GetField(blockID + "05").Trim() + ", " + this.loan.GetField(blockID + "06").Trim() + " " + this.loan.GetField(blockID + "07").Trim());
          this.fieldCollect.Set("FL0109", this.translateDepositType(this.loan.GetField(blockID + "08")));
          this.fieldCollect.Set("FL0110", this.translateDepositType(this.loan.GetField(blockID + "09")));
          this.fieldCollect.Set("FL0111", this.translateDepositType(this.loan.GetField(blockID + "10")));
          this.fieldCollect.Set("FL0112", this.translateDepositType(this.loan.GetField(blockID + "13")));
          string field3 = this.loan.GetField(blockID + "20");
          if (field3 != string.Empty)
            this.fieldCollect.Set("FL0120", "Phone  " + field3);
          string field4 = this.loan.GetField(blockID + "21");
          if (field4 != string.Empty)
            this.fieldCollect.Set("FL0121", "Fax  " + field4);
          string str2 = this.loan.GetField(blockID + "98").Trim();
          if (str2 == null || str2 == "//")
            str2 = string.Empty;
          this.fieldCollect.Set("363", str2);
          break;
      }
      this.setDefaultSignature(blockID);
      this.pageDone = string.Empty;
    }

    private string translateDepositType(string typeID)
    {
      switch (typeID)
      {
        case "Automobile":
          return typeID;
        case "Bond":
          return typeID;
        case "BridgeLoanNotDeposited":
          return "Bridge Loan Not Deposited";
        case "CashDepositOnSalesContract":
          return "Cash Deposit On Sales Contract";
        case "CashOnHand":
          return "Cash On Hand";
        case "CertificateOfDepositTimeDeposit":
          return "Certificate Of Deposit Time Deposit";
        case "CheckingAccount":
          return "Checking Account";
        case "EarnestMoneyCashDepositTowardPurchase":
          return "Earnest Money Cash Deposit Toward Purchase";
        case "GiftsNotDeposited":
          return "Gifts Not Deposited";
        case "GiftsTotal":
          return "Gifts Total";
        case "LifeInsurance":
          return "Life Insurance";
        case "MoneyMarketFund":
          return "Money Market Fund";
        case "MutualFund":
          return "Mutual Fund";
        case "NetEquity":
          return "Net Equity";
        case "NetWorthOfBusinessOwned":
          return "Net Worth Of Business Owned";
        case "OtherLiquidAssets":
          return "Other Liquid Assets";
        case "OtherNonLiquidAssets":
          return "Other Non Liquid Assets";
        case "PendingNetSaleProceedsFromRealEstateAssets":
          return "Pending Net Sale Proceeds From Real EstateAssets";
        case "RetirementFund":
          return "Retirement Fund";
        case "SavingsAccount":
          return "Savings Account";
        case "SecuredBorrowedFundsNotDeposited":
          return "Secured Borrowed Funds Not Deposited";
        case "Stock":
          return typeID;
        case "TrustAccount":
          return "Trust Account";
        default:
          return typeID;
      }
    }

    private void printVOM(int block)
    {
      string simpleField = this.loan.GetSimpleField("FM" + block.ToString("00") + "43");
      if (simpleField == string.Empty)
        return;
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int num = this.ToInt(this.pageDone);
      if (num == 0)
        num = 1;
      bool flag = false;
      this.pageDone = string.Empty;
      this.setDefaultSignature("FM" + block.ToString("00"));
      for (int index = num; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str1 = "FL" + index.ToString("00");
        if (!(this.loan.GetSimpleField(str1 + "25") != simpleField))
        {
          if (flag)
          {
            this.pageDone = index.ToString();
            return;
          }
          switch (this.loan.GetField(str1 + "15"))
          {
            case "Borrower":
              this.fieldCollect.Set("FM0105", this.getBorrowerInfo(true, 1));
              this.fieldCollect.Set("FM0106", this.getBorrowerInfo(true, 3));
              this.fieldCollect.Set("FM0107", "");
              this.fieldCollect.Set("FM0108", "");
              break;
            case "CoBorrower":
              this.fieldCollect.Set("FM0105", this.getBorrowerInfo(false, 1));
              this.fieldCollect.Set("FM0106", this.getBorrowerInfo(false, 3));
              this.fieldCollect.Set("FM0107", "");
              this.fieldCollect.Set("FM0108", "");
              break;
            case "Both":
              this.fieldCollect.Set("FM0105", this.getBorrowerInfo(true, 1));
              this.fieldCollect.Set("FM0106", this.getBorrowerInfo(true, 3));
              this.fieldCollect.Set("FM0107", this.getBorrowerInfo(false, 1));
              this.fieldCollect.Set("FM0108", this.getBorrowerInfo(false, 3));
              break;
            default:
              this.fieldCollect.Set("FM0105", "");
              this.fieldCollect.Set("FM0106", "");
              this.fieldCollect.Set("FM0107", "");
              this.fieldCollect.Set("FM0108", "");
              break;
          }
          this.fieldCollect.Set("FM0101", this.loan.GetField(str1 + "02"));
          this.fieldCollect.Set("FM0102", this.loan.GetField(str1 + "03"));
          this.fieldCollect.Set("FM0103", this.loan.GetField(str1 + "04"));
          this.fieldCollect.Set("FM0104", this.loan.GetField(str1 + "05") + ", " + this.loan.GetField(str1 + "06") + this.loan.GetField(str1 + "07"));
          string field1 = this.loan.GetField(str1 + "20");
          if (field1 != string.Empty)
            this.fieldCollect.Set("FL0120", "Phone  " + field1);
          string field2 = this.loan.GetField(str1 + "21");
          if (field2 != string.Empty)
            this.fieldCollect.Set("FL0121", "Fax  " + field2);
          string str2 = "FM" + block.ToString("00");
          this.fieldCollect.Set("FM0109", this.loan.GetField(str2 + "04"));
          this.fieldCollect.Set("FM0110", this.loan.GetField(str2 + "06") + ", " + this.loan.GetField(str2 + "07").Trim() + " " + this.loan.GetField(str2 + "08").Trim());
          string str3 = this.loan.GetField(str2 + "98").Trim();
          if (str3 == null || str3 == "//")
            str3 = string.Empty;
          this.fieldCollect.Set("363", str3);
          this.fieldCollect.Set("FM0111", this.loan.GetField(str1 + "10"));
          this.fieldCollect.Set("FM0112", this.loan.GetField(str1 + "09"));
          flag = true;
        }
      }
      if (flag || num != 1)
        return;
      this.fieldCollect.Set("FM0101", "");
      this.fieldCollect.Set("FM0102", "");
      this.fieldCollect.Set("FM0103", "");
      this.fieldCollect.Set("FM0104", "");
      this.fieldCollect.Set("FM0111", "");
      this.fieldCollect.Set("FM0112", "");
      this.fieldCollect.Set("FM0105", this.getBorrowerInfo(true, 1));
      this.fieldCollect.Set("FM0106", this.getBorrowerInfo(true, 3));
      this.fieldCollect.Set("FM0107", "");
      this.fieldCollect.Set("FM0108", "");
      string str = "FM" + block.ToString("00");
      this.fieldCollect.Set("FM0109", this.loan.GetField(str + "04"));
      this.fieldCollect.Set("FM0110", this.loan.GetField(str + "06") + ", " + this.loan.GetField(str + "07").Trim() + this.loan.GetField(str + "08").Trim());
    }

    private void printVORandVOE(string form, int block, bool isBorrower)
    {
      this.pageDone = string.Empty;
      string blockID = string.Empty;
      if (isBorrower)
      {
        switch (form)
        {
          case "VOR":
            blockID = "BR";
            break;
          case "VOE":
            blockID = "BE";
            break;
        }
      }
      else
      {
        switch (form)
        {
          case "VOR":
            blockID = "CR";
            break;
          case "VOE":
            blockID = "CE";
            break;
        }
      }
      if (blockID == string.Empty)
        return;
      switch (blockID)
      {
        case "BR":
        case "CR":
          if (blockID == "BR")
          {
            this.fieldCollect.Set("FR0105", this.getBorrowerInfo(true, 1));
            this.fieldCollect.Set("FR0106", this.getBorrowerInfo(true, 3));
          }
          else
          {
            this.fieldCollect.Set("FR0105", this.getBorrowerInfo(false, 1));
            this.fieldCollect.Set("FR0106", this.getBorrowerInfo(false, 3));
          }
          this.fieldCollect.Set("FR0107", "");
          this.fieldCollect.Set("FR0108", "");
          blockID += block.ToString("00");
          this.fieldCollect.Set("FR0101", this.loan.GetField(blockID + "02"));
          this.fieldCollect.Set("FR0102", this.loan.GetField(blockID + "03"));
          this.fieldCollect.Set("FR0103", this.loan.GetField(blockID + "05"));
          string str1 = this.loan.GetField(blockID + "98").Trim();
          if (str1 == null || str1 == "//")
            str1 = string.Empty;
          this.fieldCollect.Set("363", str1);
          this.fieldCollect.Set("FR0104", this.loan.GetField(blockID + "09").Trim() + ", " + this.loan.GetField(blockID + "10").Trim() + " " + this.loan.GetField(blockID + "11").Trim());
          this.fieldCollect.Set("FR0109", this.loan.GetField(blockID + "04"));
          this.fieldCollect.Set("FR0110", this.loan.GetField(blockID + "06").Trim() + ", " + this.loan.GetField(blockID + "07").Trim() + " " + this.loan.GetField(blockID + "08").Trim());
          this.fieldCollect.Set("FR0111", this.loan.GetField(blockID + "14"));
          string field1 = this.loan.GetField(blockID + "18");
          if (field1 != string.Empty)
            this.fieldCollect.Set("FR0118", "Phone  " + field1);
          string field2 = this.loan.GetField(blockID + "19");
          if (field2 != string.Empty)
          {
            this.fieldCollect.Set("FR0119", "Fax  " + field2);
            break;
          }
          break;
        case "BE":
        case "CE":
          if (blockID == "BE")
          {
            this.fieldCollect.Set("FE0105", this.getBorrowerInfo(true, 1));
            this.fieldCollect.Set("FE0106", this.getBorrowerInfo(true, 3));
          }
          else
          {
            this.fieldCollect.Set("FE0105", this.getBorrowerInfo(false, 1));
            this.fieldCollect.Set("FE0106", this.getBorrowerInfo(false, 3));
          }
          blockID += block.ToString("00");
          this.fieldCollect.Set("FE0107", this.loan.GetField(blockID + "39"));
          this.fieldCollect.Set("FE0101", this.loan.GetField(blockID + "02"));
          this.fieldCollect.Set("FE0102", this.loan.GetField(blockID + "03"));
          this.fieldCollect.Set("FE0103", this.loan.GetField(blockID + "04"));
          string str2 = this.loan.GetField(blockID + "98").Trim();
          if (str2 == null || str2 == "//")
            str2 = string.Empty;
          this.fieldCollect.Set("363", str2);
          this.fieldCollect.Set("FE0104", this.loan.GetField(blockID + "05").Trim() + ", " + this.loan.GetField(blockID + "06").Trim() + " " + this.loan.GetField(blockID + "07").Trim());
          string field3 = this.loan.GetField(blockID + "17");
          if (field3 != string.Empty)
            this.fieldCollect.Set("FE0117", "Phone  " + field3);
          string field4 = this.loan.GetField(blockID + "29");
          if (field4 != string.Empty)
          {
            this.fieldCollect.Set("FE0129", "Fax  " + field4);
            break;
          }
          break;
      }
      this.setDefaultSignature(blockID);
    }

    private void setDefaultSignature(string blockID)
    {
      if (this.loan.GetField(blockID + "36") == "Y")
        this.fieldCollect.Set("207", "See attached borrower's authorization");
      bool flag = this.loan.GetField(blockID + "38") == "Y";
      string verifContactId = this.calObjs.VERIFCal.VerifContactID;
      string empty = string.Empty;
      string contactTitle = string.Empty;
      string contactPhone = string.Empty;
      string contactFax = string.Empty;
      this.calObjs.VERIFCal.GetVerificationContactInformation(ref empty, ref contactTitle, ref contactPhone, ref contactFax);
      if (this.loan.GetField(blockID + "37").Trim() != string.Empty)
        contactTitle = this.loan.GetField(blockID + "37").Trim();
      if (this.loan.GetField(blockID + "44").Trim() != string.Empty)
        contactPhone = this.loan.GetField(blockID + "44").Trim();
      if (this.loan.GetField(blockID + "45").Trim() != string.Empty)
        contactFax = this.loan.GetField(blockID + "45").Trim();
      if (empty != string.Empty)
        this.fieldCollect.Set("362", empty);
      if (contactPhone != string.Empty)
        this.fieldCollect.Set("324", "Phone  " + contactPhone);
      if (contactFax != string.Empty)
        this.fieldCollect.Set("326", "Fax  " + contactFax);
      if (flag)
      {
        if (empty != string.Empty)
          this.fieldCollect.Set("Title", empty);
        else
          this.fieldCollect.Set("Title", this.loan.GetField("362").Trim());
      }
      else
        this.fieldCollect.Set("Title", contactTitle);
    }

    private string getBorrowerInfo(bool isPrimary, int infoType)
    {
      string borrowerInfo = string.Empty;
      if (isPrimary)
      {
        if (infoType == 1)
          borrowerInfo = this.loan.GetField("36") + " " + this.loan.GetField("37");
        if (infoType == 2)
          borrowerInfo = this.loan.GetField("65");
        if (infoType == 3)
        {
          string str1 = this.loan.GetField("FR0104").Trim();
          string str2 = !(str1 == string.Empty) ? str1 + ", " + this.loan.GetField("FR0106") : str1 + this.loan.GetField("FR0106").Trim();
          if (str2 == string.Empty)
            borrowerInfo = str2 + this.loan.GetField("FR0107").Trim() + " " + this.loan.GetField("FR0108").Trim();
          else
            borrowerInfo = str2 + ", " + this.loan.GetField("FR0107").Trim() + " " + this.loan.GetField("FR0108").Trim();
        }
      }
      else
      {
        if (infoType == 1)
          borrowerInfo = this.loan.GetField("68") + " " + this.loan.GetField("69");
        if (infoType == 2)
          borrowerInfo = this.loan.GetField("97");
        if (infoType == 3)
        {
          string str3 = this.loan.GetField("FR0204").Trim();
          string str4 = !(str3 == string.Empty) ? str3 + ", " + this.loan.GetField("FR0206") : str3 + this.loan.GetField("FR0206").Trim();
          if (str4 == string.Empty)
            borrowerInfo = str4 + this.loan.GetField("FR0207").Trim() + " " + this.loan.GetField("FR0208").Trim();
          else
            borrowerInfo = str4 + ", " + this.loan.GetField("FR0207").Trim() + " " + this.loan.GetField("FR0208").Trim();
        }
      }
      return borrowerInfo;
    }

    private void printSettlementServiceProviderlist(bool print1stPage, string formID)
    {
      int num1 = formID.IndexOf("LEGAL") > -1 ? 10 : 7;
      this.fieldCollect.Set("3170", this.Val("3170") == "" || this.Val("3170") == "//" ? DateTime.Today.ToString("MM/dd/yyyy") : this.Val("3170"));
      int serviceProviders = this.loan.GetNumberOfSettlementServiceProviders();
      int num2 = 1;
      if (this.pageDone != string.Empty)
        num2 = Utils.ParseInt((object) this.pageDone);
      else if (!print1stPage)
        num2 = formID.IndexOf("LEGAL") > -1 ? 11 : 8;
      this.pageDone = string.Empty;
      int num3 = 0;
      int num4 = (serviceProviders - serviceProviders % num1) / num1 + (serviceProviders % num1 > 0 ? 1 : 0);
      if (num4 == 0)
        num4 = 1;
      this.fieldCollect.Set("PageNo", "Page " + (object) ((num2 - num2 % num1) / num1 + (num2 % num1 > 0 ? 1 : 0)) + " of " + (object) num4);
      for (int index = num2; index <= serviceProviders; ++index)
      {
        ++num3;
        if (num3 > num1)
        {
          if (print1stPage)
            break;
          this.pageDone = index.ToString();
          break;
        }
        int num5 = (num3 - 1) * 9;
        this.fieldCollect.Set("GFESCRENX" + (object) (num5 + 1), this.Val("SP" + index.ToString("00") + "02"));
        this.fieldCollect.Set("GFESCRENX" + (object) (num5 + 2), this.Val("SP" + index.ToString("00") + "09"));
        string str = this.Val("SP" + index.ToString("00") + "03") + ", " + this.Val("SP" + index.ToString("00") + "04") + ", " + this.Val("SP" + index.ToString("00") + "05") + " " + this.Val("SP" + index.ToString("00") + "06");
        this.fieldCollect.Set("GFESCRENX" + (object) (num5 + 3), str.Trim() == ", ," ? "" : str.Trim());
        this.fieldCollect.Set("GFESCRENX" + (object) (num5 + 4), this.Val("SP" + index.ToString("00") + "01"));
        this.fieldCollect.Set("GFESCRENX" + (object) (num5 + 8), this.Val("SP" + index.ToString("00") + "07"));
        this.fieldCollect.Set("GFESCRENX" + (object) (num5 + 9), this.Val("SP" + index.ToString("00") + "08"));
        if (string.Compare(this.Val("SP" + index.ToString("00") + "01"), "title insurance", true) == 0 || string.Compare(this.Val("SP" + index.ToString("00") + "01"), "escrow company", true) == 0)
        {
          this.fieldCollect.Set("SERVICEDESC" + (object) num3, "Service Type:");
          this.fieldCollect.Set("SERVICETYPE" + (object) num3, this.Val("SP" + index.ToString("00") + "10"));
          this.fieldCollect.Set("COSTDESC" + (object) num3, "Cost:");
          double num6 = this.FltVal("SP" + index.ToString("00") + "11");
          if (num6 != 0.0)
            this.fieldCollect.Set("COSTAMT" + (object) num3, "$ " + num6.ToString("N2"));
        }
      }
    }

    private void printHUDGFEPage1(LoanData currentLoan)
    {
      if (currentLoan == null)
        currentLoan = this.loan;
      this.checkTodayDate(currentLoan, "3170");
      BorrowerPair[] borrowerPairs = currentLoan.GetBorrowerPairs();
      int num = 1;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Borrower.FirstName != string.Empty && borrowerPairs[index].Borrower.LastName != string.Empty)
        {
          this.fieldCollect.Set("borrower_" + (object) num, (num > 6 ? ", " : "") + borrowerPairs[index].Borrower.FirstName + " " + borrowerPairs[index].Borrower.LastName);
          ++num;
        }
        if (borrowerPairs[index].CoBorrower.FirstName != string.Empty && borrowerPairs[index].CoBorrower.LastName != string.Empty)
        {
          this.fieldCollect.Set("borrower_" + (object) num, (num > 6 ? ", " : "") + borrowerPairs[index].CoBorrower.FirstName + " " + borrowerPairs[index].CoBorrower.LastName);
          ++num;
        }
      }
      if (!(this.Val("NEWHUD.X8") == "Y"))
        return;
      this.loan.Calculator.CalculateProjectedPaymentTable();
      this.fieldCollect.Set("NEWHUD_X11", this.Val("NEWHUD.X11"));
    }

    private string get2010GFEFormSizeNeeded(string formID)
    {
      return this.printHUDGFEPage2((LoanData) null, true) >= 35 ? formID + "L.pdf" : formID + ".pdf";
    }

    private int printHUDGFEPage2(LoanData currentLoan, bool validationOnly)
    {
      if (currentLoan == null)
        currentLoan = this.loan;
      int count = 1;
      this.populateHUDGFESection3("Appraisal Fee", "NEWHUD.X609", ref count, true, currentLoan, validationOnly);
      this.populateHUDGFESection3("Credit Report", "NEWHUD.X610", ref count, true, currentLoan, validationOnly);
      this.populateHUDGFESection3("Tax Service", "NEWHUD.X611", ref count, true, currentLoan, validationOnly);
      this.populateHUDGFESection3("Flood Certification", "NEWHUD.X612", ref count, true, currentLoan, validationOnly);
      this.populateHUDGFESection3("Mortgage Insurance Premium", "NEWHUD.X622", ref count, true, currentLoan, validationOnly);
      for (int index = 18; index <= 36; index += 2)
        this.populateHUDGFESection3("NEWHUD.X" + (object) index, "NEWHUD.X" + (object) (index + 1), ref count, false, currentLoan, validationOnly);
      for (int index = 1541; index <= 1573; index += 2)
        this.populateHUDGFESection3("NEWHUD.X" + (object) index, "NEWHUD.X" + (object) (index + 1), ref count, false, currentLoan, validationOnly);
      return count;
    }

    private void populateHUDGFESection3(
      string desID,
      string amtID,
      ref int count,
      bool useDesc,
      LoanData currentLoan,
      bool validationOnly)
    {
      if (count > 63)
        return;
      double num = Utils.ParseDouble((object) currentLoan.GetField(amtID));
      string str = useDesc ? desID : currentLoan.GetField(desID);
      if (currentLoan.GetField(amtID) == string.Empty)
        return;
      if (!validationOnly)
      {
        this.fieldCollect.Set("S3_" + (object) count, str);
        if (num != 0.0)
          this.fieldCollect.Set("S3_" + (object) (count + 1), num.ToString("N2"));
        else if (currentLoan.GetField(amtID) != string.Empty)
          this.fieldCollect.Set("S3_" + (object) (count + 1), num.ToString("N2"));
        else
          this.fieldCollect.Set("S3_" + (object) (count + 1), "");
      }
      count += 2;
    }

    private void print2010Itemization()
    {
      this.printFinancedNote = false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.blockID = "REGZGFE_";
      this.outputGrid = new List<string[]>();
      for (int lineID = 801; lineID <= 1315; ++lineID)
        this.set2010Itemization(lineID, true);
      if (this.loan.GetField("949") != string.Empty || this.outputGrid.Count > 0)
      {
        this.outputGrid.Add((string[]) null);
        string str = "PREPAID FINANCE CHARGE";
        string field = this.loan.GetField("949");
        this.outputGrid.Add(new string[8]
        {
          "",
          str,
          "",
          "",
          "",
          "",
          field != string.Empty ? "$" : "",
          field
        });
        this.outputGrid.Add((string[]) null);
      }
      for (int lineID = 801; lineID <= 1315; ++lineID)
        this.set2010Itemization(lineID, false);
      this.outputGrid.Add((string[]) null);
      string str1 = "AMT PAID ON YOUR ACCT/PAID TO OTHERS ON YOUR BEHALF";
      double num1 = this.FltVal("NEWHUD.X277") - (this.FltVal("L215") + this.FltVal("L218"));
      for (int index = 821; index <= 830; ++index)
        num1 -= this.FltVal("NEWHUD.X" + (object) index);
      for (int index = 1433; index <= 1439; ++index)
        num1 -= this.FltVal("NEWHUD.X" + (object) index);
      double num2 = num1 - this.FltVal("NEWHUD.X231");
      double num3 = !(this.Val("NEWHUD.X1139") == "Y") ? num2 - this.FltVal("NEWHUD.X831") : num2 - (this.FltVal("NEWHUD.X1192") + this.FltVal("NEWHUD.X1194") + this.FltVal("NEWHUD.X1196") + this.FltVal("NEWHUD.X1198"));
      this.outputGrid.Add(new string[8]
      {
        "",
        str1,
        "",
        "",
        "",
        "",
        num3 != 0.0 ? "$" : "",
        num3.ToString("N2")
      });
      if (this.printFinancedNote)
      {
        this.outputGrid.Add(new string[8]
        {
          "",
          "",
          "",
          "",
          "",
          "",
          "",
          ""
        });
        this.outputGrid.Add(new string[8]
        {
          "",
          "*F: Financed Fees",
          "",
          "",
          "",
          "",
          "",
          ""
        });
      }
      this.lineCount = 0;
      int num4 = 0;
      if (this.pageDone != string.Empty)
        num4 = Utils.ParseInt((object) this.pageDone);
      for (int index1 = num4; index1 < this.outputGrid.Count; ++index1)
      {
        if (this.lineCount > 76)
        {
          this.pageDone = string.Concat((object) index1);
          return;
        }
        string[] strArray = this.outputGrid[index1];
        int num5 = this.lineCount * 6;
        for (int index2 = 1; index2 <= 8; ++index2)
        {
          switch (index2)
          {
            case 1:
            case 2:
              this.fieldCollect.Set(this.blockID + (object) (num5 + index2), strArray != null ? strArray[index2 - 1] : "");
              break;
            case 3:
              this.fieldCollect.Set(this.blockID + (object) (num5 + 4) + "P", strArray != null ? strArray[index2 - 1] : "");
              break;
            case 4:
              this.fieldCollect.Set(this.blockID + (object) (num5 + 6) + "a", strArray != null ? strArray[index2 - 1] : "");
              break;
            case 5:
            case 6:
            case 7:
            case 8:
              this.fieldCollect.Set(this.blockID + (object) (num5 + index2 - 2), strArray != null ? strArray[index2 - 1] : "");
              break;
          }
        }
        ++this.lineCount;
      }
      this.pageDone = string.Empty;
    }

    private void set2010Itemization(int lineID, bool inPrepaid)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      this.calcOnly = false;
      this.total801POCAmt = 0.0;
      this.total801PTCAmt = 0.0;
      this.total801NonPOCPTCAmt = 0.0;
      this.print801Header = false;
      this.total1101POCAmt = 0.0;
      this.total1101PTCAmt = 0.0;
      this.total1101NonPOCPTCAmt = 0.0;
      this.total802POCAmt = 0.0;
      this.total802PTCAmt = 0.0;
      this.total802NonPOCPTCAmt = 0.0;
      this.print802Header = false;
      switch (lineID)
      {
        case 801:
          for (int index = 1; index <= 2; ++index)
          {
            this.calcOnly = index == 1;
            if (this.calcOnly || this.Val("NEWHUD.X750") == "Y")
            {
              double num1 = this.FltVal("388");
              string feeDescription1 = "  801a. Loan Origination Fees" + " @ " + num1.ToString("N3") + " %";
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription1, "SYS.X251", true);
              string feeDescription2 = "  801b. Application Fees";
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription2, "SYS.X261", true);
              string feeDescription3 = "  801c. Processing Fees";
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription3, "SYS.X269", true);
              string feeDescription4 = "  801d. Underwriting Fees";
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription4, "SYS.X271", true);
              num1 = this.FltVal("389");
              string feeDescription5 = "  801e. Broker Fees @ " + num1.ToString("N3") + " %";
              if (this.FltVal("1620") > 0.0)
                feeDescription5 = feeDescription5 + " + $" + this.Val("1620");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription5, "SYS.X265", true);
              num1 = this.FltVal("NEWHUD.X223");
              string feeDescription6 = "  801f. Broker Compensation @ " + num1.ToString("N3") + " %";
              if (this.FltVal("NEWHUD.X224") > 0.0)
                feeDescription6 = feeDescription6 + " + $" + this.Val("NEWHUD.X224");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription6, "NEWHUD.X227", true);
              string feeDescription7 = "  801g. " + this.Val("154") + (this.Val("NEWHUD.X1045") != string.Empty ? " to " + this.Val("NEWHUD.X1045") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription7, "SYS.X289", true);
              string feeDescription8 = "  801h. " + this.Val("1627") + (this.Val("NEWHUD.X1046") != string.Empty ? " to " + this.Val("NEWHUD.X1046") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription8, "SYS.X291", true);
              string feeDescription9 = "  801i. " + this.Val("1838") + (this.Val("NEWHUD.X1047") != string.Empty ? " to " + this.Val("NEWHUD.X1047") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription9, "SYS.X296", true);
              string feeDescription10 = "  801j. " + this.Val("1841") + (this.Val("NEWHUD.X1048") != string.Empty ? " to " + this.Val("NEWHUD.X1048") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription10, "SYS.X301", true);
              string feeDescription11 = "  801k. " + this.Val("NEWHUD.X732") + (this.Val("NEWHUD.X1049") != string.Empty ? " to " + this.Val("NEWHUD.X1049") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription11, "NEWHUD.X748", true);
              string feeDescription12 = "  801l. " + this.Val("NEWHUD.X1235") + (this.Val("NEWHUD.X1236") != string.Empty ? " to " + this.Val("NEWHUD.X1236") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription12, "NEWHUD.X1239", true);
              string feeDescription13 = "  801m. " + this.Val("NEWHUD.X1243") + (this.Val("NEWHUD.X1244") != string.Empty ? " to " + this.Val("NEWHUD.X1244") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription13, "NEWHUD.X1247", true);
              string feeDescription14 = "  801n. " + this.Val("NEWHUD.X1251") + (this.Val("NEWHUD.X1252") != string.Empty ? " to " + this.Val("NEWHUD.X1252") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription14, "NEWHUD.X1255", true);
              string feeDescription15 = "  801o. " + this.Val("NEWHUD.X1259") + (this.Val("NEWHUD.X1260") != string.Empty ? " to " + this.Val("NEWHUD.X1260") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription15, "NEWHUD.X1263", true);
              string feeDescription16 = "  801p. " + this.Val("NEWHUD.X1267") + (this.Val("NEWHUD.X1268") != string.Empty ? " to " + this.Val("NEWHUD.X1268") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription16, "NEWHUD.X1271", true);
              string feeDescription17 = "  801q. " + this.Val("NEWHUD.X1275") + (this.Val("NEWHUD.X1276") != string.Empty ? " to " + this.Val("NEWHUD.X1276") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription17, "NEWHUD.X1279", true);
              string feeDescription18 = "  801r. " + this.Val("NEWHUD.X1283") + (this.Val("NEWHUD.X1284") != string.Empty ? " to " + this.Val("NEWHUD.X1284") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription18, "NEWHUD.X1287", true);
              if (this.loan.GetField("NEWHUD.X713") == "Origination Charge")
              {
                if (this.loan.GetField("NEWHUD.X1139") == "Y")
                {
                  string[] strArray1 = new string[5]
                  {
                    "  801s. Lender Compensation Credit",
                    " @ ",
                    null,
                    null,
                    null
                  };
                  num1 = this.FltVal("NEWHUD.X1141");
                  strArray1[2] = num1.ToString("N3");
                  strArray1[3] = " %";
                  string str1;
                  if (this.FltVal("NEWHUD.X1225") == 0.0)
                  {
                    str1 = "";
                  }
                  else
                  {
                    num1 = this.FltVal("NEWHUD.X1225");
                    str1 = " + $" + num1.ToString("N2");
                  }
                  strArray1[4] = str1;
                  string feeDescription19 = string.Concat(strArray1);
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription19, "NEWHUD.X1167", true);
                  string[] strArray2 = new string[5]
                  {
                    "  801t. Origination Credit",
                    " @ ",
                    null,
                    null,
                    null
                  };
                  num1 = this.FltVal("NEWHUD.X1143");
                  strArray2[2] = num1.ToString("N3");
                  strArray2[3] = " %";
                  string str2;
                  if (this.FltVal("NEWHUD.X1226") == 0.0)
                  {
                    str2 = "";
                  }
                  else
                  {
                    num1 = this.FltVal("NEWHUD.X1226");
                    str2 = " + $" + num1.ToString("N2");
                  }
                  strArray2[4] = str2;
                  string feeDescription20 = string.Concat(strArray2);
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription20, "NEWHUD.X1169", true);
                  string feeDescription21 = "  801u. " + this.Val("NEWHUD.X1145");
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription21, "NEWHUD.X1171", true);
                  string feeDescription22 = "  801v. " + this.Val("NEWHUD.X1147");
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription22, "NEWHUD.X1173", true);
                  num1 = this.FltVal("NEWHUD.X1150");
                  string str3 = num1.ToString("N3");
                  string str4;
                  if (this.FltVal("NEWHUD.X1227") == 0.0)
                  {
                    str4 = "";
                  }
                  else
                  {
                    num1 = this.FltVal("NEWHUD.X1227");
                    str4 = " + $" + num1.ToString("N2");
                  }
                  string feeDescription23 = "  801w. Origination Points @ " + str3 + " %" + str4;
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription23, "NEWHUD.X1175", true);
                  if (!this.calcOnly && !inPrepaid && this.FltVal("NEWHUD.X1152") != 0.0 && this.FltVal("NEWHUD.X1149") != 0.0)
                  {
                    this.print801Header = true;
                    List<string[]> outputGrid = this.outputGrid;
                    string[] strArray3 = new string[8];
                    strArray3[0] = "";
                    strArray3[1] = "  801w. Origination Points Paid by Seller";
                    strArray3[2] = "";
                    num1 = this.FltVal("NEWHUD.X1152");
                    strArray3[3] = num1.ToString("N2");
                    strArray3[4] = "";
                    strArray3[5] = "";
                    strArray3[6] = "";
                    strArray3[7] = "";
                    outputGrid.Add(strArray3);
                  }
                  string[] strArray4 = new string[5]
                  {
                    "  801x. ",
                    this.Val("NEWHUD.X1153"),
                    " @ ",
                    null,
                    null
                  };
                  num1 = this.FltVal("NEWHUD.X1154");
                  strArray4[3] = num1.ToString("N3");
                  strArray4[4] = " %";
                  string feeDescription24 = string.Concat(strArray4);
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription24, "NEWHUD.X1179", true);
                  string[] strArray5 = new string[5]
                  {
                    "  801y. ",
                    this.Val("NEWHUD.X1157"),
                    " @ ",
                    null,
                    null
                  };
                  num1 = this.FltVal("NEWHUD.X1158");
                  strArray5[3] = num1.ToString("N3");
                  strArray5[4] = " %";
                  string feeDescription25 = string.Concat(strArray5);
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription25, "NEWHUD.X1183", true);
                  string[] strArray6 = new string[5]
                  {
                    "  801z. ",
                    this.Val("NEWHUD.X1161"),
                    " @ ",
                    null,
                    null
                  };
                  num1 = this.FltVal("NEWHUD.X1162");
                  strArray6[3] = num1.ToString("N3");
                  strArray6[4] = " %";
                  string feeDescription26 = string.Concat(strArray6);
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription26, "NEWHUD.X1187", true);
                }
                else if (this.loan.GetField("NEWHUD.X715") == "Include Origination Credit")
                {
                  double num2 = inPrepaid && this.Val("NEWHUD.X353") == "Y" && this.Val("NEWHUD.X749") == string.Empty || !inPrepaid && (this.Val("NEWHUD.X353") != "Y" || this.Val("NEWHUD.X749") != string.Empty) ? this.FltVal("1663") : 0.0;
                  if (num2 != 0.0)
                  {
                    string str = "  801s. Origination Credit ";
                    double num3 = num2 * -1.0;
                    if (!this.calcOnly)
                      this.outputGrid.Add(new string[8]
                      {
                        "",
                        str,
                        "",
                        num3.ToString("N2"),
                        "",
                        "",
                        "",
                        ""
                      });
                    else
                      this.total801NonPOCPTCAmt += num3;
                  }
                }
                else if (this.loan.GetField("NEWHUD.X715") == "Include Origination Points")
                {
                  string feeDescription27 = this.FltVal("3119") <= 0.0 ? "  801s. Origination Points" : "  801s. Mortgage Buydown";
                  this.set2010ItemizationLine(inPrepaid, lineID, feeDescription27, "NEWHUD.X749", true);
                }
              }
            }
            if (index == 1 && this.print801Header)
            {
              string str = "Our Origination Charge";
              if (this.Val("NEWHUD.X750") == "Y")
                str += " Includes";
              this.outputGrid.Add(new string[8]
              {
                string.Concat((object) lineID),
                str,
                "",
                "",
                this.total801POCAmt != 0.0 || this.total801PTCAmt != 0.0 ? "$" : "",
                this.total801POCAmt != 0.0 || this.total801PTCAmt != 0.0 ? this.total801POCAmt.ToString("N2") + " / " + this.total801PTCAmt.ToString("N2") : "",
                this.total801NonPOCPTCAmt != 0.0 ? "$" : "",
                this.total801NonPOCPTCAmt.ToString("N2")
              });
            }
          }
          break;
        case 802:
          if (this.loan.GetField("NEWHUD.X1139") == "Y")
          {
            if (!(this.loan.GetField("NEWHUD.X713") != "Origination Charge"))
              break;
            for (int index = 1; index <= 2; ++index)
            {
              this.calcOnly = index == 1;
              if (this.calcOnly || this.Val("NEWHUD.X1140") == "Y")
              {
                string[] strArray7 = new string[5]
                {
                  "  802a. Lender Compensation Credit",
                  " @ ",
                  null,
                  null,
                  null
                };
                double num = this.FltVal("NEWHUD.X1141");
                strArray7[2] = num.ToString("N3");
                strArray7[3] = " %";
                string str5;
                if (this.FltVal("NEWHUD.X1225") == 0.0)
                {
                  str5 = "";
                }
                else
                {
                  num = this.FltVal("NEWHUD.X1225");
                  str5 = " + $" + num.ToString("N2");
                }
                strArray7[4] = str5;
                string feeDescription28 = string.Concat(strArray7);
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription28, "NEWHUD.X1167", true);
                string[] strArray8 = new string[5]
                {
                  "  802b. Origination Credit",
                  " @ ",
                  null,
                  null,
                  null
                };
                num = this.FltVal("NEWHUD.X1143");
                strArray8[2] = num.ToString("N3");
                strArray8[3] = " %";
                string str6;
                if (this.FltVal("NEWHUD.X1226") == 0.0)
                {
                  str6 = "";
                }
                else
                {
                  num = this.FltVal("NEWHUD.X1226");
                  str6 = " + $" + num.ToString("N2");
                }
                strArray8[4] = str6;
                string feeDescription29 = string.Concat(strArray8);
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription29, "NEWHUD.X1169", true);
                string feeDescription30 = "  802c. " + this.Val("NEWHUD.X1145");
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription30, "NEWHUD.X1171", true);
                string feeDescription31 = "  802d. " + this.Val("NEWHUD.X1147");
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription31, "NEWHUD.X1173", true);
                num = this.FltVal("NEWHUD.X1150");
                string str7 = num.ToString("N3");
                string str8;
                if (this.FltVal("NEWHUD.X1227") == 0.0)
                {
                  str8 = "";
                }
                else
                {
                  num = this.FltVal("NEWHUD.X1227");
                  str8 = " + $" + num.ToString("N2");
                }
                string feeDescription32 = "  802e. Origination Points @ " + str7 + " %" + str8;
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription32, "NEWHUD.X1175", true);
                if (!this.calcOnly && !inPrepaid && this.FltVal("NEWHUD.X1152") != 0.0 && this.FltVal("NEWHUD.X1149") != 0.0)
                {
                  this.print802Header = true;
                  List<string[]> outputGrid = this.outputGrid;
                  string[] strArray9 = new string[8];
                  strArray9[0] = "";
                  strArray9[1] = "  802e. Origination Points Paid by Seller";
                  strArray9[2] = "";
                  num = this.FltVal("NEWHUD.X1152");
                  strArray9[3] = num.ToString("N2");
                  strArray9[4] = "";
                  strArray9[5] = "";
                  strArray9[6] = "";
                  strArray9[7] = "";
                  outputGrid.Add(strArray9);
                }
                string[] strArray10 = new string[5]
                {
                  "  802f. ",
                  this.Val("NEWHUD.X1153"),
                  " @ ",
                  null,
                  null
                };
                num = this.FltVal("NEWHUD.X1154");
                strArray10[3] = num.ToString("N3");
                strArray10[4] = " %";
                string feeDescription33 = string.Concat(strArray10);
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription33, "NEWHUD.X1179", true);
                string[] strArray11 = new string[5]
                {
                  "  802g. ",
                  this.Val("NEWHUD.X1157"),
                  " @ ",
                  null,
                  null
                };
                num = this.FltVal("NEWHUD.X1158");
                strArray11[3] = num.ToString("N3");
                strArray11[4] = " %";
                string feeDescription34 = string.Concat(strArray11);
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription34, "NEWHUD.X1183", true);
                string[] strArray12 = new string[5]
                {
                  "  802h. ",
                  this.Val("NEWHUD.X1161"),
                  " @ ",
                  null,
                  null
                };
                num = this.FltVal("NEWHUD.X1162");
                strArray12[3] = num.ToString("N3");
                strArray12[4] = " %";
                string feeDescription35 = string.Concat(strArray12);
                this.set2010ItemizationLine(inPrepaid, lineID, feeDescription35, "NEWHUD.X1187", true);
              }
              if (index == 1 && this.print802Header)
              {
                string str = "Your Credit or Points";
                if (this.Val("NEWHUD.X1140") == "Y")
                  str += " Includes";
                this.outputGrid.Add(new string[8]
                {
                  string.Concat((object) lineID),
                  str,
                  "",
                  "",
                  this.total802POCAmt != 0.0 || this.total802PTCAmt != 0.0 ? "$" : "",
                  this.total802POCAmt != 0.0 || this.total802PTCAmt != 0.0 ? this.total802POCAmt.ToString("N2") + " / " + this.total802PTCAmt.ToString("N2") : "",
                  this.total802NonPOCPTCAmt != 0.0 ? "$" : "",
                  this.total802NonPOCPTCAmt.ToString("N2")
                });
              }
            }
            break;
          }
          if (!(this.loan.GetField("NEWHUD.X713") != "Origination Charge"))
            break;
          if (this.loan.GetField("NEWHUD.X715") == "Include Origination Credit")
          {
            double num4 = inPrepaid && this.Val("NEWHUD.X353") == "Y" && this.Val("NEWHUD.X749") == string.Empty || !inPrepaid && (this.Val("NEWHUD.X353") != "Y" || this.Val("NEWHUD.X749") != string.Empty) ? this.FltVal("1663") : 0.0;
            if (num4 == 0.0)
              break;
            double num5 = num4 * -1.0;
            string str = "Origination Credit ";
            this.outputGrid.Add(new string[8]
            {
              string.Concat((object) lineID),
              str,
              "",
              "",
              "",
              "",
              "$",
              num5.ToString("N2")
            });
            break;
          }
          if (!(this.loan.GetField("NEWHUD.X715") == "Include Origination Points"))
            break;
          string feeDescription36 = this.FltVal("3119") <= 0.0 ? "Origination Points" : "Mortgage Buydown";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription36, "NEWHUD.X749", false);
          break;
        case 804:
          this.set2010ItemizationLine(inPrepaid, lineID, "Appraisal Fee" + (this.Val("617") != string.Empty ? " to " + this.Val("617") : ""), "SYS.X255", false);
          break;
        case 805:
          this.set2010ItemizationLine(inPrepaid, lineID, "Credit Report" + (this.Val("624") != string.Empty ? " to " + this.Val("624") : ""), "SYS.X257", false);
          break;
        case 806:
          this.set2010ItemizationLine(inPrepaid, lineID, "Tax Service" + (this.Val("L224") != string.Empty ? " to " + this.Val("L224") : ""), "SYS.X267", false);
          break;
        case 807:
          this.set2010ItemizationLine(inPrepaid, lineID, "Flood Certification" + (this.Val("NEWHUD.X399") != string.Empty ? " to " + this.Val("NEWHUD.X399") : ""), "NEWHUD.X742", false);
          break;
        case 808:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X126") + (this.Val("NEWHUD.X1050") != string.Empty ? " to " + this.Val("NEWHUD.X1050") : ""), "NEWHUD.X157", false);
          break;
        case 809:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X127") + (this.Val("NEWHUD.X1051") != string.Empty ? " to " + this.Val("NEWHUD.X1051") : ""), "NEWHUD.X158", false);
          break;
        case 810:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X128") + (this.Val("NEWHUD.X1052") != string.Empty ? " to " + this.Val("NEWHUD.X1052") : ""), "NEWHUD.X159", false);
          break;
        case 811:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X129") + (this.Val("NEWHUD.X1053") != string.Empty ? " to " + this.Val("NEWHUD.X1053") : ""), "NEWHUD.X160", false);
          break;
        case 812:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X130") + (this.Val("NEWHUD.X1054") != string.Empty ? " to " + this.Val("NEWHUD.X1054") : ""), "NEWHUD.X161", false);
          break;
        case 813:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("369") + (this.Val("NEWHUD.X1055") != string.Empty ? " to " + this.Val("NEWHUD.X1055") : ""), "SYS.X275", false);
          break;
        case 814:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("371") + (this.Val("NEWHUD.X1056") != string.Empty ? " to " + this.Val("NEWHUD.X1056") : ""), "SYS.X277", false);
          break;
        case 815:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("348") + (this.Val("NEWHUD.X1057") != string.Empty ? " to " + this.Val("NEWHUD.X1057") : ""), "SYS.X279", false);
          break;
        case 816:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("931") + (this.Val("NEWHUD.X1058") != string.Empty ? " to " + this.Val("NEWHUD.X1058") : ""), "SYS.X281", false);
          break;
        case 817:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1390") + (this.Val("NEWHUD.X1059") != string.Empty ? " to " + this.Val("NEWHUD.X1059") : ""), "SYS.X283", false);
          break;
        case 818:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1291") + (this.Val("NEWHUD.X1292") != string.Empty ? " to " + this.Val("NEWHUD.X1292") : ""), "NEWHUD.X1295", false);
          break;
        case 819:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1299") + (this.Val("NEWHUD.X1300") != string.Empty ? " to " + this.Val("NEWHUD.X1300") : ""), "NEWHUD.X1303", false);
          break;
        case 820:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1307") + (this.Val("NEWHUD.X1308") != string.Empty ? " to " + this.Val("NEWHUD.X1308") : ""), "NEWHUD.X1311", false);
          break;
        case 821:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1315") + (this.Val("NEWHUD.X1316") != string.Empty ? " to " + this.Val("NEWHUD.X1316") : ""), "NEWHUD.X1319", false);
          break;
        case 822:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1323") + (this.Val("NEWHUD.X1324") != string.Empty ? " to " + this.Val("NEWHUD.X1324") : ""), "NEWHUD.X1327", false);
          break;
        case 823:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1331") + (this.Val("NEWHUD.X1332") != string.Empty ? " to " + this.Val("NEWHUD.X1332") : ""), "NEWHUD.X1335", false);
          break;
        case 824:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1339") + (this.Val("NEWHUD.X1340") != string.Empty ? " to " + this.Val("NEWHUD.X1340") : ""), "NEWHUD.X1343", false);
          break;
        case 825:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1347") + (this.Val("NEWHUD.X1348") != string.Empty ? " to " + this.Val("NEWHUD.X1348") : ""), "NEWHUD.X1351", false);
          break;
        case 826:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1355") + (this.Val("NEWHUD.X1356") != string.Empty ? " to " + this.Val("NEWHUD.X1356") : ""), "NEWHUD.X1359", false);
          break;
        case 827:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1363") + (this.Val("NEWHUD.X1364") != string.Empty ? " to " + this.Val("NEWHUD.X1364") : ""), "NEWHUD.X1367", false);
          break;
        case 828:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1371") + (this.Val("NEWHUD.X1372") != string.Empty ? " to " + this.Val("NEWHUD.X1372") : ""), "NEWHUD.X1375", false);
          break;
        case 829:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1379") + (this.Val("NEWHUD.X1380") != string.Empty ? " to " + this.Val("NEWHUD.X1380") : ""), "NEWHUD.X1383", false);
          break;
        case 830:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1387") + (this.Val("NEWHUD.X1388") != string.Empty ? " to " + this.Val("NEWHUD.X1388") : ""), "NEWHUD.X1391", false);
          break;
        case 831:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1395") + (this.Val("NEWHUD.X1396") != string.Empty ? " to " + this.Val("NEWHUD.X1396") : ""), "NEWHUD.X1399", false);
          break;
        case 832:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1403") + (this.Val("NEWHUD.X1404") != string.Empty ? " to " + this.Val("NEWHUD.X1404") : ""), "NEWHUD.X1407", false);
          break;
        case 833:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1411") + (this.Val("NEWHUD.X1412") != string.Empty ? " to " + this.Val("NEWHUD.X1412") : ""), "NEWHUD.X1415", false);
          break;
        case 834:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("410") + (this.Val("NEWHUD.X1060") != string.Empty ? " to " + this.Val("NEWHUD.X1060") : ""), "SYS.X285", false);
          break;
        case 835:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X656") + (this.Val("NEWHUD.X1061") != string.Empty ? " to " + this.Val("NEWHUD.X1061") : ""), "NEWHUD.X162", false);
          break;
        case 901:
          string empty5 = string.Empty;
          string str9 = !(this.Val("SYS.X8") == "Y") ? this.Val("333") : Utils.ArithmeticRounding(Utils.ParseDouble((object) this.Val("333")), 2).ToString("N2");
          string feeDescription37 = "Prepaid Interest " + this.Val("332") + " days @ $" + str9;
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription37, "SYS.X303", false);
          break;
        case 902:
          this.set2010ItemizationLine(inPrepaid, lineID, "Mortgage Insurance Premium" + (this.Val("L248") != string.Empty ? " to " + this.Val("L248") : ""), "SYS.X305", false);
          break;
        case 903:
          string feeDescription38 = "First Year Homeowner's Insurance for " + this.Val("L251") + " mths to " + this.Val("L252");
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription38, "SYS.X307", false);
          break;
        case 904:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X582") + (this.Val("NEWHUD.X1062") != string.Empty ? " to " + this.Val("NEWHUD.X1062") : ""), "NEWHUD.X163", false);
          break;
        case 905:
          this.set2010ItemizationLine(inPrepaid, lineID, "VA Funding Fee" + (this.Val("1956") != string.Empty ? " to " + this.Val("1956") : ""), "SYS.X311", false);
          break;
        case 906:
          this.set2010ItemizationLine(inPrepaid, lineID, "First Year Flood Insurance Premium" + (this.Val("1500") != string.Empty ? " to " + this.Val("1500") : ""), "SYS.X309", false);
          break;
        case 907:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("L259") + (this.Val("NEWHUD.X1063") != string.Empty ? " to " + this.Val("NEWHUD.X1063") : ""), "SYS.X313", false);
          break;
        case 908:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1666") + (this.Val("NEWHUD.X1064") != string.Empty ? " to " + this.Val("NEWHUD.X1064") : ""), "SYS.X315", false);
          break;
        case 909:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X583") + (this.Val("NEWHUD.X1065") != string.Empty ? " to " + this.Val("NEWHUD.X1065") : ""), "NEWHUD.X164", false);
          break;
        case 910:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X584") + (this.Val("NEWHUD.X1066") != string.Empty ? " to " + this.Val("NEWHUD.X1066") : ""), "NEWHUD.X165", false);
          break;
        case 911:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1586") + (this.Val("NEWHUD.X1587") != string.Empty ? " to " + this.Val("NEWHUD.X1587") : ""), "NEWHUD.X1590", false);
          break;
        case 912:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1594") + (this.Val("NEWHUD.X1595") != string.Empty ? " to " + this.Val("NEWHUD.X1595") : ""), "NEWHUD.X1598", false);
          break;
        case 1002:
          string feeDescription39 = "Homeowner's Insurance " + this.Val("1387") + " months @ $" + this.Val("230") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription39, "SYS.X317", false);
          break;
        case 1003:
          string feeDescription40 = "PMI/MMI Impounds " + this.Val("1296") + " months @ $" + this.Val("232") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription40, "SYS.X319", false);
          break;
        case 1004:
          string feeDescription41 = "Taxes " + this.Val("1386") + " months @ $" + this.Val("231") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription41, "SYS.X323", false);
          break;
        case 1005:
          string feeDescription42 = "City Property Taxes  " + this.Val("L267") + " months @ $" + this.Val("L268") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription42, "SYS.X321", false);
          break;
        case 1006:
          string feeDescription43 = "Flood Insurance " + this.Val("1388") + " months @ $" + this.Val("235") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription43, "SYS.X325", false);
          break;
        case 1007:
          string feeDescription44 = this.Val("1628") + (this.Val("VEND.X341") != string.Empty ? " to " + this.Val("VEND.X341") + " " : " ") + this.Val("1629") + " months @ $" + this.Val("1630") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription44, "SYS.X327", false);
          break;
        case 1008:
          string feeDescription45 = this.Val("660") + (this.Val("VEND.X350") != string.Empty ? " to " + this.Val("VEND.X350") + " " : " ") + this.Val("340") + " months @ $" + this.Val("253") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription45, "SYS.X329", false);
          break;
        case 1009:
          string feeDescription46 = this.Val("661") + (this.Val("VEND.X359") != string.Empty ? " to " + this.Val("VEND.X359") + " " : " ") + this.Val("341") + " months @ $" + this.Val("254") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription46, "SYS.X331", false);
          break;
        case 1010:
          string feeDescription47 = "USDA Annual Fee" + (this.Val("NEWHUD.X1705") != string.Empty ? " to " + this.Val("NEWHUD.X1705") + " " : " ") + this.Val("NEWHUD.X1706") + " months @ $" + this.Val("NEWHUD.X1707") + " per month";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription47, "NEWHUD.X1710", false);
          break;
        case 1011:
          this.set2010ItemizationLine(inPrepaid, lineID, "Aggregate Analysis Adjustment", "", false);
          break;
        case 1101:
          for (int index = 1; index <= 2; ++index)
          {
            this.calcOnly = index == 1;
            if (this.calcOnly || this.Val("NEWHUD.X1017") == "Y")
            {
              string feeDescription48 = "  1101a. " + this.Val("NEWHUD.X951") + (this.Val("NEWHUD.X1070") != string.Empty ? " to " + this.Val("NEWHUD.X1070") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription48, "NEWHUD.X955", true);
              string feeDescription49 = "  1101b. " + this.Val("NEWHUD.X960") + (this.Val("NEWHUD.X1071") != string.Empty ? " to " + this.Val("NEWHUD.X1071") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription49, "NEWHUD.X964", true);
              string feeDescription50 = "  1101c. " + this.Val("NEWHUD.X969") + (this.Val("NEWHUD.X1072") != string.Empty ? " to " + this.Val("NEWHUD.X1072") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription50, "NEWHUD.X973", true);
              string feeDescription51 = "  1101d. " + this.Val("NEWHUD.X978") + (this.Val("NEWHUD.X1073") != string.Empty ? " to " + this.Val("NEWHUD.X1073") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription51, "NEWHUD.X982", true);
              string feeDescription52 = "  1101e. " + this.Val("NEWHUD.X987") + (this.Val("NEWHUD.X1074") != string.Empty ? " to " + this.Val("NEWHUD.X1074") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription52, "NEWHUD.X991", true);
              string feeDescription53 = "  1101f. " + this.Val("NEWHUD.X996") + (this.Val("NEWHUD.X1075") != string.Empty ? " to " + this.Val("NEWHUD.X1075") : "");
              this.set2010ItemizationLine(inPrepaid, lineID, feeDescription53, "NEWHUD.X1000", true);
            }
            string feeDescription54 = "  1102.  Settlement or Closing Fee" + (this.Val("NEWHUD.X203") != string.Empty ? " to " + this.Val("NEWHUD.X203") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription54, "NEWHUD.X743", true);
            string feeDescription55 = "  1104.  Lender's Title Insurance" + (this.Val("NEWHUD.X205") != string.Empty ? " to " + this.Val("NEWHUD.X205") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription55, "NEWHUD.X745", true);
            string feeDescription56 = "  1109.  " + this.Val("NEWHUD.X208") + (this.Val("NEWHUD.X1076") != string.Empty ? " to " + this.Val("NEWHUD.X1076") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription56, "NEWHUD.X221", true);
            string feeDescription57 = "  1110.  " + this.Val("NEWHUD.X209") + (this.Val("NEWHUD.X1077") != string.Empty ? " to " + this.Val("NEWHUD.X1077") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription57, "NEWHUD.X222", true);
            string feeDescription58 = "  1111.  " + this.Val("1762") + (this.Val("NEWHUD.X1078") != string.Empty ? " to " + this.Val("NEWHUD.X1078") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription58, "SYS.X347", true);
            string feeDescription59 = "  1112.  " + this.Val("1767") + (this.Val("NEWHUD.X1079") != string.Empty ? " to " + this.Val("NEWHUD.X1079") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription59, "SYS.X349", true);
            string feeDescription60 = "  1113.  " + this.Val("1772") + (this.Val("NEWHUD.X1080") != string.Empty ? " to " + this.Val("NEWHUD.X1080") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription60, "SYS.X351", true);
            string feeDescription61 = "  1114.  " + this.Val("1777") + (this.Val("NEWHUD.X1081") != string.Empty ? " to " + this.Val("NEWHUD.X1081") : "");
            this.set2010ItemizationLine(inPrepaid, lineID, feeDescription61, "SYS.X353", true);
            if (index == 1 && (this.total1101POCAmt != 0.0 || this.total1101PTCAmt != 0.0 || this.total1101NonPOCPTCAmt != 0.0))
            {
              string str10 = "Title Services and Lender's Title Insurance Includes";
              this.outputGrid.Add(new string[8]
              {
                string.Concat((object) lineID),
                str10,
                "",
                "",
                this.total1101POCAmt != 0.0 || this.total1101PTCAmt != 0.0 ? "$" : "",
                this.total1101POCAmt != 0.0 || this.total1101PTCAmt != 0.0 ? this.total1101POCAmt.ToString("N2") + " / " + this.total1101PTCAmt.ToString("N2") : "",
                this.total1101NonPOCPTCAmt != 0.0 ? "$" : "",
                this.total1101NonPOCPTCAmt != 0.0 ? this.total1101NonPOCPTCAmt.ToString("N2") : ""
              });
              ++this.lineCount;
            }
          }
          break;
        case 1103:
          this.set2010ItemizationLine(inPrepaid, lineID, "Owner's Title Insurance" + (this.Val("NEWHUD.X204") != string.Empty ? " to " + this.Val("NEWHUD.X204") : ""), "NEWHUD.X744", false);
          break;
        case 1105:
          string feeDescription62 = "Lender's Title Policy Limit: $ " + this.Val("646");
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription62, "", false);
          break;
        case 1106:
          string feeDescription63 = "Owner's Title Policy Limit: $ " + this.Val("1634");
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription63, "", false);
          break;
        case 1107:
          string feeDescription64 = "Agent's Portion of The Total Title Insurance Premium";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription64, "", false);
          if (inPrepaid || !(this.Val("NEWHUD.X206") != string.Empty))
            break;
          this.outputGrid.Add(new string[8]
          {
            "",
            "to " + this.Val("NEWHUD.X206"),
            "",
            "",
            "",
            "",
            "",
            ""
          });
          break;
        case 1108:
          string feeDescription65 = "Underwriter's Portion of The Total Title Insurance Premium";
          this.set2010ItemizationLine(inPrepaid, lineID, feeDescription65, "", false);
          if (inPrepaid || !(this.Val("NEWHUD.X207") != string.Empty))
            break;
          this.outputGrid.Add(new string[8]
          {
            "",
            "to " + this.Val("NEWHUD.X207"),
            "",
            "",
            "",
            "",
            "",
            ""
          });
          break;
        case 1115:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1602") + (this.Val("NEWHUD.X1603") != string.Empty ? " to " + this.Val("NEWHUD.X1603") : ""), "NEWHUD.X1606", false);
          break;
        case 1116:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1610") + (this.Val("NEWHUD.X1611") != string.Empty ? " to " + this.Val("NEWHUD.X1611") : ""), "NEWHUD.X1614", false);
          break;
        case 1202:
          int lineCount = this.lineCount;
          this.set2010ItemizationLine(inPrepaid, lineID, "Recording Fees" + (this.Val("1636") != string.Empty ? " to " + this.Val("1636") : ""), "SYS.X355", false);
          if (lineCount >= this.lineCount || !this.Val("1636").ToLower().StartsWith("deed $"))
            break;
          this.outputGrid.Add(new string[8]
          {
            "",
            "to " + this.Val("1636"),
            "",
            "",
            "",
            "",
            "",
            ""
          });
          ++this.lineCount;
          break;
        case 1203:
          this.set2010ItemizationLine(inPrepaid, lineID, "Transfer Taxes" + (this.Val("NEWHUD.X947") != string.Empty ? " to " + this.Val("NEWHUD.X947") : ""), "NEWHUD.X261", false);
          break;
        case 1204:
          this.set2010ItemizationLine(inPrepaid, lineID, "City/County Tax/Stamps" + (this.Val("1637") != string.Empty ? " to " + this.Val("1637") : ""), "SYS.X357", false);
          break;
        case 1205:
          this.set2010ItemizationLine(inPrepaid, lineID, "State Tax/Stamps" + (this.Val("1638") != string.Empty ? " to " + this.Val("1638") : ""), "SYS.X359", false);
          break;
        case 1206:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("373") + (this.Val("NEWHUD.X1082") != string.Empty ? " to " + this.Val("NEWHUD.X1082") : ""), "SYS.X361", false);
          break;
        case 1207:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1640") + (this.Val("NEWHUD.X1083") != string.Empty ? " to " + this.Val("NEWHUD.X1083") : ""), "SYS.X363", false);
          break;
        case 1208:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1643") + (this.Val("NEWHUD.X1084") != string.Empty ? " to " + this.Val("NEWHUD.X1084") : ""), "SYS.X365", false);
          break;
        case 1209:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1618") + (this.Val("NEWHUD.X1619") != string.Empty ? " to " + this.Val("NEWHUD.X1619") : ""), "NEWHUD.X1622", false);
          break;
        case 1210:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1625") + (this.Val("NEWHUD.X1626") != string.Empty ? " to " + this.Val("NEWHUD.X1626") : ""), "NEWHUD.X1629", false);
          break;
        case 1302:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X251") + (this.Val("NEWHUD.X1085") != string.Empty ? " to " + this.Val("NEWHUD.X1085") : ""), "NEWHUD.X262", false);
          break;
        case 1303:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("650") + (this.Val("NEWHUD.X1086") != string.Empty ? " to " + this.Val("NEWHUD.X1086") : ""), "SYS.X374", false);
          break;
        case 1304:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("651") + (this.Val("NEWHUD.X1087") != string.Empty ? " to " + this.Val("NEWHUD.X1087") : ""), "SYS.X376", false);
          break;
        case 1305:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("40") + (this.Val("NEWHUD.X1088") != string.Empty ? " to " + this.Val("NEWHUD.X1088") : ""), "SYS.X378", false);
          break;
        case 1306:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("43") + (this.Val("NEWHUD.X1089") != string.Empty ? " to " + this.Val("NEWHUD.X1089") : ""), "SYS.X380", false);
          break;
        case 1307:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1782") + (this.Val("NEWHUD.X1090") != string.Empty ? " to " + this.Val("NEWHUD.X1090") : ""), "SYS.X382", false);
          break;
        case 1308:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1787") + (this.Val("NEWHUD.X1091") != string.Empty ? " to " + this.Val("NEWHUD.X1091") : ""), "SYS.X384", false);
          break;
        case 1309:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("1792") + (this.Val("NEWHUD.X1092") != string.Empty ? " to " + this.Val("NEWHUD.X1092") : ""), "SYS.X386", false);
          break;
        case 1310:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X252") + (this.Val("NEWHUD.X1093") != string.Empty ? " to " + this.Val("NEWHUD.X1093") : ""), "NEWHUD.X263", false);
          break;
        case 1311:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X253") + (this.Val("NEWHUD.X1094") != string.Empty ? " to " + this.Val("NEWHUD.X1094") : ""), "NEWHUD.X264", false);
          break;
        case 1312:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1632") + (this.Val("NEWHUD.X1633") != string.Empty ? " to " + this.Val("NEWHUD.X1633") : ""), "NEWHUD.X1636", false);
          break;
        case 1313:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1640") + (this.Val("NEWHUD.X1641") != string.Empty ? " to " + this.Val("NEWHUD.X1641") : ""), "NEWHUD.X1644", false);
          break;
        case 1314:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1648") + (this.Val("NEWHUD.X1649") != string.Empty ? " to " + this.Val("NEWHUD.X1649") : ""), "NEWHUD.X1652", false);
          break;
        case 1315:
          this.set2010ItemizationLine(inPrepaid, lineID, this.Val("NEWHUD.X1656") + (this.Val("NEWHUD.X1657") != string.Empty ? " to " + this.Val("NEWHUD.X1657") : ""), "NEWHUD.X1660", false);
          break;
      }
    }

    private void set2010ItemizationLine(
      bool forAPR,
      int lineID,
      string feeDescription,
      string paidByID,
      bool isDetail)
    {
      bool flag1 = false;
      bool flag2 = this.Val("NEWHUD.X1139") == "Y";
      string[] strArray1 = (string[]) null;
      if (HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID))
        strArray1 = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      string str1 = strArray1 != null ? strArray1[HUDGFE2010Fields.PTCPOCINDEX_POC] : string.Empty;
      if (strArray1 != null)
      {
        flag1 = this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_APR]) == "Y";
      }
      else
      {
        switch (lineID)
        {
          case 1002:
          case 1003:
          case 1004:
          case 1005:
          case 1006:
          case 1007:
          case 1008:
          case 1009:
          case 1010:
            flag1 = this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_APR]) == "Y";
            break;
          case 1011:
          case 1105:
          case 1106:
          case 1107:
          case 1108:
            flag1 = false;
            break;
        }
      }
      if (forAPR && !flag1)
        return;
      string[] strArray2 = new string[8]
      {
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        ""
      };
      if (!this.calcOnly)
      {
        if (isDetail)
        {
          strArray2[0] = "";
        }
        else
        {
          switch (lineID)
          {
            case 11011:
              strArray2[0] = "1101a";
              break;
            case 11012:
              strArray2[0] = "1101b";
              break;
            case 11013:
              strArray2[0] = "1101c";
              break;
            case 11014:
              strArray2[0] = "1101d";
              break;
            case 11015:
              strArray2[0] = "1101e";
              break;
            case 11016:
              strArray2[0] = "1101f";
              break;
            default:
              strArray2[0] = string.Concat((object) lineID);
              break;
          }
        }
        strArray2[1] = lineID != 1202 || !this.Val("1636").ToLower().StartsWith("deed $") ? feeDescription : "Recording Fees";
      }
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      string empty = string.Empty;
      if (strArray1 != null)
      {
        this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
        this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
        double num4 = this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
        string str2 = this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
        double num5 = this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
        string str3 = this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
        double num6 = this.FltVal(strArray1[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]);
        this.GetAPRAmount(strArray1[HUDGFE2010Fields.PTCPOCINDEX_APR], paidByID);
        if (flag1)
        {
          if (!(str2 == string.Empty))
          {
            switch (str2)
            {
              case "Other":
                break;
              case "Broker":
                if (this.calObjs.NewHudCal.UsingTableFunded)
                  goto label_27;
                else
                  break;
              default:
                goto label_27;
            }
          }
          num1 += num4;
label_27:
          if (!(str3 == string.Empty))
          {
            switch (str3)
            {
              case "Other":
                break;
              case "Broker":
                if (this.calObjs.NewHudCal.UsingTableFunded)
                  goto label_31;
                else
                  break;
              default:
                goto label_31;
            }
          }
          num2 += num5;
label_31:
          num3 += num6;
        }
        if (!forAPR)
        {
          double nonAprAmount = this.GetNonAPRAmount(strArray1[HUDGFE2010Fields.PTCPOCINDEX_APR], paidByID);
          num1 = num4 - num1;
          num2 = num5 - num2;
          double num7 = nonAprAmount - (num1 + num2);
          num3 = num7 != 0.0 ? num7 : 0.0;
        }
      }
      else
      {
        switch (lineID)
        {
          case 1002:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("656") : 0.0;
            break;
          case 1003:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("338") : 0.0;
            break;
          case 1004:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("655") : 0.0;
            break;
          case 1005:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("L269") : 0.0;
            break;
          case 1006:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("657") : 0.0;
            break;
          case 1007:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("1631") : 0.0;
            break;
          case 1008:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("658") : 0.0;
            break;
          case 1009:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("659") : 0.0;
            break;
          case 1010:
            num3 = forAPR & flag1 && empty == string.Empty || !forAPR && (!flag1 || empty != string.Empty) ? this.FltVal("NEWHUD.X1708") : 0.0;
            break;
          case 1011:
            num3 = !forAPR ? this.FltVal("558") : 0.0;
            break;
          case 1107:
            num3 = !forAPR ? this.FltVal("NEWHUD.X640") : 0.0;
            break;
          case 1108:
            num3 = !forAPR ? this.FltVal("NEWHUD.X641") : 0.0;
            break;
        }
        if (!forAPR & flag2)
        {
          if (feeDescription.Trim().StartsWith("802a.") || feeDescription.Trim().StartsWith("801l."))
            num3 -= this.FltVal("NEWHUD.X1142");
          if (feeDescription.Trim().StartsWith("802b.") || feeDescription.Trim().StartsWith("801m."))
            num3 -= this.FltVal("NEWHUD.X1144");
          if (feeDescription.Trim().StartsWith("802c.") || feeDescription.Trim().StartsWith("801n."))
            num3 -= this.FltVal("NEWHUD.X1146");
          if (feeDescription.Trim().StartsWith("802d.") || feeDescription.Trim().StartsWith("801o."))
            num3 -= this.FltVal("NEWHUD.X1148");
        }
      }
      if (flag2 && feeDescription.Trim().StartsWith("801w.") && this.FltVal("NEWHUD.X1149") != 0.0)
        this.total801NonPOCPTCAmt += this.FltVal("NEWHUD.X1152");
      if (flag2 && feeDescription.Trim().StartsWith("802e.") && this.FltVal("NEWHUD.X1149") != 0.0)
        this.total802NonPOCPTCAmt += this.FltVal("NEWHUD.X1152");
      if (num1 == 0.0 && num2 == 0.0 && num3 == 0.0)
        return;
      if (this.calcOnly)
      {
        if (lineID == 801)
          this.print801Header = true;
        if (lineID == 802)
          this.print802Header = true;
        this.total801NonPOCPTCAmt += num3;
        this.total801POCAmt += num1;
        this.total801PTCAmt += num2;
        this.total802NonPOCPTCAmt += num3;
        this.total802POCAmt += num1;
        this.total802PTCAmt += num2;
        this.total1101NonPOCPTCAmt += num3;
        this.total1101POCAmt += num1;
        this.total1101PTCAmt += num2;
      }
      else
      {
        if (lineID == 1107 || lineID == 1108)
          strArray2[3] = num3 != 0.0 ? num3.ToString("N2") : "";
        else if (isDetail)
        {
          string[] strArray3 = strArray2;
          string str4;
          if (num1 == 0.0 && num2 == 0.0)
            str4 = "";
          else
            str4 = "(" + num1.ToString("N2") + "/" + num2.ToString("N2") + ")";
          strArray3[2] = str4;
          strArray2[3] = strArray1 == null || !(this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]) == "Y") ? (num3 != 0.0 ? num3.ToString("N2") : "") : (num3 != 0.0 ? "*F " + num3.ToString("N2") : "");
        }
        else
        {
          strArray2[4] = num1 != 0.0 || num2 != 0.0 ? "$" : "";
          strArray2[5] = num1 != 0.0 || num2 != 0.0 ? num1.ToString("N2") + " / " + num2.ToString("N2") : "";
          if (strArray1 != null && this.Val(strArray1[HUDGFE2010Fields.PTCPOCINDEX_FINANCED]) == "Y")
          {
            this.printFinancedNote = true;
            strArray2[6] = num3 != 0.0 ? "*F$" : "";
          }
          else
            strArray2[6] = num3 != 0.0 ? "$" : "";
          strArray2[7] = num3 != 0.0 ? num3.ToString("N2") : "";
        }
        this.outputGrid.Add(strArray2);
        ++this.lineCount;
      }
    }

    private void printREGGFE()
    {
      string nil = CalculationBase.nil;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.blockID = "REGZGFE_";
      this.lineCount = 0;
      for (int index = 801; index <= 1309; ++index)
      {
        if (this.getAPRSetting(index, false) == "Y")
          this.setRGZStaticField(index);
      }
      if (this.loan.GetField("949") != string.Empty)
      {
        this.addGFEEmptyLine(6);
        this.setREGZGFEValues("PREPAID FINANCE CHARGE", this.loan.GetField("949"), "$");
        this.addGFEEmptyLine(6);
      }
      for (int index = 801; index <= 1309; ++index)
      {
        if (index == 824 || index == 825)
        {
          if (this.loan.GetField("1970") != "Y" && (this.checkValues3("1662", "1847", "1663") || this.checkValues3("1664", "1848", "1665")))
          {
            this.setREGZGFEValues("COMPENSATION TO BROKER (Not Paid Out of Loan Proceeds):", string.Empty, string.Empty);
            this.setRGZStaticField(824);
            this.setRGZStaticField(825);
            index = 825;
          }
        }
        else if (this.getAPRSetting(index, false) != "Y")
          this.setRGZStaticField(index);
      }
      this.addGFEEmptyLine(6);
      this.setREGZGFEValues("AMOUNT PAID ON YOUR ACCOUNT/PAID TO OTHERS ON YOUR BEHALF", this.loan.GetField("304"), "$");
    }

    private string getAPRSetting(int line, bool for2010)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string id;
      switch (line)
      {
        case 801:
          id = for2010 ? "NEWHUD.X689" : "SYS.X17";
          break;
        case 802:
          id = for2010 ? "NEWHUD.X353" : "SYS.X18";
          break;
        case 803:
          id = for2010 ? "" : "SYS.X19";
          break;
        case 804:
          id = for2010 ? "SYS.X19" : "SYS.X20";
          break;
        case 805:
          id = for2010 ? "SYS.X20" : "SYS.X21";
          break;
        case 806:
          id = for2010 ? "SYS.X22" : "SYS.X116";
          break;
        case 807:
          id = for2010 ? "NEWHUD.X178" : "SYS.X117";
          break;
        case 808:
          id = for2010 ? "NEWHUD.X179" : "SYS.X28";
          break;
        case 809:
          id = for2010 ? "NEWHUD.X180" : "SYS.X22";
          break;
        case 810:
          id = for2010 ? "NEWHUD.X181" : "SYS.X201";
          break;
        case 811:
          id = for2010 ? "NEWHUD.X182" : "SYS.X23";
          break;
        case 812:
          id = for2010 ? "NEWHUD.X183" : "SYS.X203";
          break;
        case 813:
          id = "SYS.X30";
          break;
        case 814:
          id = "SYS.X31";
          break;
        case 815:
          id = "SYS.X32";
          break;
        case 816:
          id = "SYS.X33";
          break;
        case 817:
          id = "SYS.X34";
          break;
        case 818:
          id = for2010 ? "NEWHUD.X1297" : "SYS.X35";
          break;
        case 819:
          id = for2010 ? "NEWHUD.X1305" : "SYS.X36";
          break;
        case 820:
          id = for2010 ? "NEWHUD.X1313" : "SYS.X37";
          break;
        case 821:
          id = for2010 ? "NEWHUD.X1321" : "SYS.X215";
          break;
        case 822:
          id = for2010 ? "NEWHUD.X1329" : "SYS.X294";
          break;
        case 823:
          id = for2010 ? "NEWHUD.X1337" : "SYS.X299";
          break;
        case 824:
          id = for2010 ? "NEWHUD.X1345" : "";
          break;
        case 825:
          id = for2010 ? "NEWHUD.X1353" : "";
          break;
        case 826:
          id = for2010 ? "NEWHUD.X1361" : "";
          break;
        case 827:
          id = for2010 ? "NEWHUD.X1369" : "";
          break;
        case 828:
          id = for2010 ? "NEWHUD.X1377" : "";
          break;
        case 829:
          id = for2010 ? "NEWHUD.X1385" : "";
          break;
        case 830:
          id = for2010 ? "NEWHUD.X1393" : "";
          break;
        case 831:
          id = for2010 ? "NEWHUD.X1401" : "";
          break;
        case 832:
          id = for2010 ? "NEWHUD.X1409" : "";
          break;
        case 833:
          id = for2010 ? "NEWHUD.X1417" : "";
          break;
        case 834:
          id = for2010 ? "SYS.X35" : "";
          break;
        case 835:
          id = for2010 ? "NEWHUD.X659" : "";
          break;
        case 901:
          id = "SYS.X4";
          break;
        case 902:
          id = "SYS.X38";
          break;
        case 903:
          id = "SYS.X39";
          break;
        case 904:
          id = for2010 ? "NEWHUD.X629" : "SYS.X389";
          break;
        case 905:
          id = "SYS.X29";
          break;
        case 906:
          id = "SYS.X40";
          break;
        case 907:
          id = "SYS.X118";
          break;
        case 908:
          id = "SYS.X205";
          break;
        case 909:
          id = for2010 ? "NEWHUD.X623" : "";
          break;
        case 910:
          id = for2010 ? "NEWHUD.X624" : "";
          break;
        case 1001:
          id = for2010 ? "" : "SYS.X42";
          break;
        case 1002:
          id = "SYS.X42";
          break;
        case 1003:
          id = for2010 ? "SYS.X43" : "SYS.X119";
          break;
        case 1004:
          id = "SYS.X44";
          break;
        case 1005:
          id = for2010 ? "SYS.X119" : "";
          break;
        case 1006:
          id = "SYS.X45";
          break;
        case 1007:
          id = "SYS.X207";
          break;
        case 1008:
          id = "SYS.X46";
          break;
        case 1009:
          id = "SYS.X47";
          break;
        case 1101:
          id = for2010 ? "" : "SYS.X15";
          break;
        case 1102:
          id = for2010 ? "NEWHUD.X234" : "SYS.X121";
          break;
        case 1103:
          id = for2010 ? "NEWHUD.X235" : "SYS.X122";
          break;
        case 1104:
          id = for2010 ? "NEWHUD.X236" : "SYS.X123";
          break;
        case 1105:
          id = for2010 ? "" : "SYS.X48";
          break;
        case 1106:
          id = for2010 ? "" : "SYS.X49";
          break;
        case 1107:
          id = for2010 ? "" : "SYS.X16";
          break;
        case 1108:
          id = for2010 ? "" : "SYS.X50";
          break;
        case 1109:
          id = for2010 ? "NEWHUD.X241" : "SYS.X52";
          break;
        case 1110:
          id = for2010 ? "NEWHUD.X242" : "SYS.X209";
          break;
        case 1111:
          id = "SYS.X217";
          break;
        case 1112:
          id = "SYS.X219";
          break;
        case 1113:
          id = "SYS.X221";
          break;
        case 1114:
          id = "SYS.X223";
          break;
        case 1201:
          id = for2010 ? "" : "SYS.X53";
          break;
        case 1202:
          id = for2010 ? "SYS.X53" : "SYS.X54";
          break;
        case 1203:
          id = for2010 ? "NEWHUD.X249" : "SYS.X55";
          break;
        case 1204:
          id = for2010 ? "SYS.X54" : "SYS.X56";
          break;
        case 1205:
          id = for2010 ? "SYS.X55" : "SYS.X211";
          break;
        case 1206:
          id = for2010 ? "SYS.X56" : "SYS.X213";
          break;
        case 1207:
          id = for2010 ? "SYS.X211" : "";
          break;
        case 1208:
          id = for2010 ? "SYS.X213" : "";
          break;
        case 1301:
          id = for2010 ? "" : "SYS.X57";
          break;
        case 1302:
          id = for2010 ? "NEWHUD.X270" : "SYS.X58";
          break;
        case 1303:
          id = "SYS.X61";
          break;
        case 1304:
          id = "SYS.X62";
          break;
        case 1305:
          id = "SYS.X63";
          break;
        case 1306:
          id = "SYS.X64";
          break;
        case 1307:
          id = "SYS.X225";
          break;
        case 1308:
          id = "SYS.X227";
          break;
        case 1309:
          id = "SYS.X229";
          break;
        case 1310:
          id = for2010 ? "NEWHUD.X271" : "";
          break;
        case 1311:
          id = for2010 ? "NEWHUD.X272" : "";
          break;
        default:
          id = string.Empty;
          break;
      }
      return !(id != string.Empty) ? string.Empty : this.loan.GetField(id);
    }

    private void setREGZGFEValues(string value1, string value2, string value3)
    {
      this.fieldPos = this.lineCount * 6;
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 1).ToString(), "");
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 2).ToString(), value1);
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 3).ToString(), "");
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 4).ToString(), "");
      if (value2 == string.Empty)
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 5).ToString(), "");
      else
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 5).ToString(), value3);
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 6).ToString(), value2);
      ++this.lineCount;
    }

    private void setRGZStaticField(int lineID)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      switch (lineID)
      {
        case 801:
          if (!this.checkValues("388", "1619"))
            break;
          string str1 = this.formID == "GFE LENDER" || this.formID == "GFE BROKER" || this.formID == "GFE - ITEMIZATION" ? "Loan Origination Fee  @ " : "Loan Origination Fee Due Lender  @ ";
          string poc1 = "SYS.X136";
          string str2 = str1 + this.loan.GetField("388") + " %";
          if (this.FltVal("1619") > 0.0)
            str2 = str2 + " + $" + this.loan.GetField("1619");
          if (this.Val("REGZGFE.X5") != string.Empty)
            str2 = str2 + "  to " + this.Val("REGZGFE.X5");
          string field1 = this.loan.GetField("454");
          this.setREGZValue1(lineID, str2, field1, poc1);
          break;
        case 802:
          if (!this.checkValues("1061", "1093"))
            break;
          string poc2 = "SYS.X137";
          string str3 = !(this.loan.GetField("436") != string.Empty) ? "Discount Fee @ " + this.loan.GetField("1061") + " %" : "Discount Fee @ " + this.loan.GetField("1061") + " % + " + this.loan.GetField("436");
          if (this.Val("REGZGFE.X6") != string.Empty)
            str3 = str3 + "  to " + this.Val("REGZGFE.X6");
          string field2 = this.loan.GetField("1093");
          this.setREGZValue1(lineID, str3, field2, poc2);
          break;
        case 803:
          if (!this.checkValues3("617", "448", "641"))
            break;
          string poc3 = "SYS.X231";
          string str4 = "Appraisal Fee to " + this.loan.GetField("617");
          string field3 = this.loan.GetField("641");
          this.setREGZValue1(lineID, str4, field3, poc3);
          break;
        case 804:
          if (!this.checkValues3("624", "449", "640"))
            break;
          string poc4 = "SYS.X232";
          string str5 = "Credit Report Fee to " + this.loan.GetField("624");
          string field4 = this.loan.GetField("640");
          this.setREGZValue1(lineID, str5, field4, poc4);
          break;
        case 805:
          if (!this.checkValues3("L704", "", "329"))
            break;
          string poc5 = "SYS.X138";
          string str6 = "Lender's Inspection Fee to " + this.loan.GetField("L704");
          string field5 = this.loan.GetField("329");
          this.setREGZValue1(lineID, str6, field5, poc5);
          break;
        case 806:
          if (!this.checkValues3("L227", "", "L228"))
            break;
          string poc6 = "SYS.X139";
          string str7 = "Mortgage Insurance Application Fee to " + this.loan.GetField("L227");
          string field6 = this.loan.GetField("L228");
          this.setREGZValue1(lineID, str7, field6, poc6);
          break;
        case 807:
          if (!this.checkValues3("REGZGFE.X7", "", "L230"))
            break;
          string poc7 = "SYS.X140";
          string str8 = "Assumption Fee to " + this.loan.GetField("REGZGFE.X7");
          string field7 = this.loan.GetField("L230");
          this.setREGZValue1(lineID, str8, field7, poc7);
          break;
        case 808:
          if (!this.checkValues3("389", "1620", "439"))
            break;
          string poc8 = "SYS.X147";
          string str9 = !(this.loan.GetField("1620") != string.Empty) ? "Mortgage Broker Fee @ " + this.loan.GetField("389") + " %" : "Mortgage Broker Fee @ " + this.loan.GetField("389") + " % + " + this.loan.GetField("1620");
          if (this.Val("REGZGFE.X14") != string.Empty)
            str9 = str9 + "  to " + this.Val("REGZGFE.X14");
          string field8 = this.loan.GetField("439");
          this.setREGZValue1(lineID, str9, field8, poc8);
          break;
        case 809:
          if (!this.checkValues3("L224", "", "336"))
            break;
          string poc9 = "SYS.X141";
          string str10 = "Tax Servicing Fee to " + this.loan.GetField("L224");
          string field9 = this.loan.GetField("336");
          this.setREGZValue1(lineID, str10, field9, poc9);
          break;
        case 810:
          if (!this.checkValues3("1812", "1878", "1621"))
            break;
          string poc10 = "SYS.X233";
          string str11 = "Processing Fee to " + this.loan.GetField("1812");
          string field10 = this.loan.GetField("1621");
          this.setREGZValue1(lineID, str11, field10, poc10);
          break;
        case 811:
          if (!this.checkValues3("REGZGFE.X8", "", "367"))
            break;
          string poc11 = "SYS.X142";
          string str12 = "Underwriting Fee to " + this.loan.GetField("REGZGFE.X8");
          string field11 = this.loan.GetField("367");
          this.setREGZValue1(lineID, str12, field11, poc11);
          break;
        case 812:
          if (!this.checkValues3("1813", "", "1623"))
            break;
          string poc12 = "SYS.X234";
          string str13 = "Wire Transfer Fee to " + this.loan.GetField("1813");
          string field12 = this.loan.GetField("1623");
          this.setREGZValue1(lineID, str13, field12, poc12);
          break;
        case 813:
          if (!this.checkValues3("369", "", "370"))
            break;
          string poc13 = "SYS.X149";
          string field13 = this.loan.GetField("369");
          string field14 = this.loan.GetField("370");
          this.setREGZValue1(lineID, field13, field14, poc13);
          break;
        case 814:
          if (!this.checkValues3("371", "", "372"))
            break;
          string poc14 = "SYS.X150";
          string field15 = this.loan.GetField("371");
          string field16 = this.loan.GetField("372");
          this.setREGZValue1(lineID, field15, field16, poc14);
          break;
        case 815:
          if (!this.checkValues3("348", "", "349"))
            break;
          string poc15 = "SYS.X151";
          string field17 = this.loan.GetField("348");
          string field18 = this.loan.GetField("349");
          this.setREGZValue1(lineID, field17, field18, poc15);
          break;
        case 816:
          if (!this.checkValues3("931", "", "932"))
            break;
          string poc16 = "SYS.X152";
          string field19 = this.loan.GetField("931");
          string field20 = this.loan.GetField("932");
          this.setREGZValue1(lineID, field19, field20, poc16);
          break;
        case 817:
          if (!this.checkValues3("1390", "", "1009"))
            break;
          string poc17 = "SYS.X153";
          string field21 = this.loan.GetField("1390");
          string field22 = this.loan.GetField("1009");
          this.setREGZValue1(lineID, field21, field22, poc17);
          break;
        case 818:
          if (!this.checkValues3("410", "", "554"))
            break;
          string poc18 = "SYS.X154";
          string field23 = this.loan.GetField("410");
          string field24 = this.loan.GetField("554");
          this.setREGZValue1(lineID, field23, field24, poc18);
          break;
        case 819:
          if (!this.checkValues3("1391", "", "81"))
            break;
          string poc19 = "SYS.X155";
          string field25 = this.loan.GetField("1391");
          string field26 = this.loan.GetField("81");
          this.setREGZValue1(lineID, field25, field26, poc19);
          break;
        case 820:
          if (!this.checkValues3("154", "", "155"))
            break;
          string poc20 = "SYS.X156";
          string field27 = this.loan.GetField("154");
          string field28 = this.loan.GetField("155");
          this.setREGZValue1(lineID, field27, field28, poc20);
          break;
        case 821:
          if (!this.checkValues3("1627", "", "1625"))
            break;
          string poc21 = "SYS.X243";
          string field29 = this.loan.GetField("1627");
          string field30 = this.loan.GetField("1625");
          this.setREGZValue1(lineID, field29, field30, poc21);
          break;
        case 822:
          if (!this.checkValues3("1838", "", "1839"))
            break;
          string poc22 = "SYS.X293";
          string field31 = this.loan.GetField("1838");
          string field32 = this.loan.GetField("1839");
          this.setREGZValue1(lineID, field31, field32, poc22);
          break;
        case 823:
          if (!this.checkValues3("1841", "", "1842"))
            break;
          string poc23 = "SYS.X298";
          string field33 = this.loan.GetField("1841");
          string field34 = this.loan.GetField("1842");
          this.setREGZValue1(lineID, field33, field34, poc23);
          break;
        case 824:
          if (!this.checkValues3("1662", "", "1663") || !(this.loan.GetField("1970") != "Y"))
            break;
          string poc24 = "Y";
          string str14 = this.loan.GetField("1662");
          if (this.loan.GetField("1847") != "")
            str14 = str14 + " " + this.loan.GetField("1847") + " %";
          string field35 = this.loan.GetField("1663");
          this.setREGZValue1(lineID, str14, field35, poc24);
          break;
        case 825:
          if (!this.checkValues3("1664", "", "1665") || !(this.loan.GetField("1970") != "Y"))
            break;
          string poc25 = "Y";
          string str15 = this.loan.GetField("1664");
          if (this.loan.GetField("1848") != "")
            str15 = str15 + " " + this.loan.GetField("1848") + " %";
          string field36 = this.loan.GetField("1665");
          this.setREGZValue1(lineID, str15, field36, poc25);
          break;
        case 901:
          if (!this.checkValues("332", "333") && !this.checkValues("", "334"))
            break;
          string poc26 = "SYS.X157";
          string empty5 = string.Empty;
          string str16 = !(this.loan.GetSimpleField("SYS.X8") == "Y") ? this.loan.GetField("333") : Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetSimpleField("333")), 2).ToString("N2");
          string str17 = "Prepaid Interest " + this.loan.GetField("332") + " days @ $" + str16;
          string field37 = this.loan.GetField("334");
          this.setREGZValue1(lineID, str17, field37, poc26);
          break;
        case 902:
          if (!this.checkValues3("L248", "", "337"))
            break;
          string poc27 = "SYS.X158";
          string str18 = "Private Mortgage Insurance Premium to " + this.loan.GetField("L248");
          string field38 = this.loan.GetField("337");
          this.setREGZValue1(lineID, str18, field38, poc27);
          break;
        case 903:
          if (!this.checkValues3("L252", "L251", "642"))
            break;
          string poc28 = "SYS.X159";
          string str19 = "First Year Hazard Insurance Premium for " + this.loan.GetField("L251") + " mths to " + this.loan.GetField("L252");
          string field39 = this.loan.GetField("642");
          this.setREGZValue1(lineID, str19, field39, poc28);
          break;
        case 904:
          if (!this.checkValues("1955", "1849"))
            break;
          string poc29 = "SYS.X388";
          string str20 = "County Property Taxes to " + this.loan.GetField("1955");
          string field40 = this.loan.GetField("1849");
          this.setREGZValue1(lineID, str20, field40, poc29);
          break;
        case 905:
          if (!this.checkValues("1956", "1050"))
            break;
          string poc30 = "SYS.X235";
          string str21 = "VA Funding Fee to " + this.loan.GetField("1956");
          string field41 = this.loan.GetField("1050");
          this.setREGZValue1(lineID, str21, field41, poc30);
          break;
        case 906:
          if (!this.checkValues3("1500", "", "643"))
            break;
          string poc31 = "SYS.X160";
          string str22 = "First Year Flood Insurance Premium to " + this.loan.GetField("1500");
          string field42 = this.loan.GetField("643");
          this.setREGZValue1(lineID, str22, field42, poc31);
          break;
        case 907:
          if (!this.checkValues3("L259", "", "L260"))
            break;
          string poc32 = "SYS.X161";
          string field43 = this.loan.GetField("L259");
          string field44 = this.loan.GetField("L260");
          this.setREGZValue1(lineID, field43, field44, poc32);
          break;
        case 908:
          if (!this.checkValues3("1666", "", "1667"))
            break;
          string poc33 = "SYS.X238";
          string field45 = this.loan.GetField("1666");
          string field46 = this.loan.GetField("1667");
          this.setREGZValue1(lineID, field45, field46, poc33);
          break;
        case 1001:
          if (this.checkValues("", "656"))
          {
            string poc34 = "SYS.X162";
            string str23 = "Hazard Insurance Impounds " + this.loan.GetField("1387") + " months @ $" + this.loan.GetField("230") + " per month";
            string field47 = this.loan.GetField("656");
            this.setREGZValue1(lineID, str23, field47, poc34);
            break;
          }
          if (!this.checkValues("1387", "230"))
            break;
          string str24;
          if (this.loan.GetField("1387") != string.Empty)
            str24 = "Hazard Insurance Impounds " + this.loan.GetField("1387") + " months @ $" + this.loan.GetField("230") + " per month";
          else
            str24 = "Hazard Insurance Impounds        months @ $" + this.loan.GetField("230") + " per month";
          this.setGFEDescription(lineID, str24);
          break;
        case 1002:
          if (this.checkValues("", "338"))
          {
            string poc35 = "SYS.X163";
            string str25 = "PMI/MMI Impounds " + this.loan.GetField("1296") + " months @ $" + this.loan.GetField("232") + " per month";
            string field48 = this.loan.GetField("338");
            this.setREGZValue1(lineID, str25, field48, poc35);
            break;
          }
          if (!this.checkValues("1296", "232"))
            break;
          string str26;
          if (this.loan.GetField("1296") != string.Empty)
            str26 = "PMI/MMI Impounds " + this.loan.GetField("1296") + " months @ $" + this.loan.GetField("232") + " per month";
          else
            str26 = "PMI/MMI Impounds        months @ $" + this.loan.GetField("232") + " per month";
          this.setGFEDescription(lineID, str26);
          break;
        case 1003:
          if (this.checkValues("", "L269"))
          {
            string poc36 = "SYS.X164";
            string str27 = "City Property Taxes  " + this.loan.GetField("L267") + " months @ $" + this.loan.GetField("L268") + " per month";
            string field49 = this.loan.GetField("L269");
            this.setREGZValue1(lineID, str27, field49, poc36);
            break;
          }
          if (!this.checkValues("L267", "L268"))
            break;
          string str28;
          if (this.loan.GetField("L267") != string.Empty)
            str28 = "City Property Taxes  " + this.loan.GetField("L267") + " months @ $" + this.loan.GetField("L268") + " per month";
          else
            str28 = "City Property Taxes         months @ $" + this.loan.GetField("L268") + " per month";
          this.setGFEDescription(lineID, str28);
          break;
        case 1004:
          if (this.checkValues("", "655"))
          {
            string poc37 = "SYS.X165";
            string str29 = "Taxes " + this.loan.GetField("1386") + " months @ $" + this.loan.GetField("231") + " per month";
            string field50 = this.loan.GetField("655");
            this.setREGZValue1(lineID, str29, field50, poc37);
            break;
          }
          if (!this.checkValues("1386", "231"))
            break;
          string str30;
          if (this.loan.GetField("1386") != string.Empty)
            str30 = "Taxes " + this.loan.GetField("1386") + " months @ $" + this.loan.GetField("231") + " per month";
          else
            str30 = "Taxes        months @ $" + this.loan.GetField("231") + " per month";
          this.setGFEDescription(lineID, str30);
          break;
        case 1006:
          if (this.checkValues("", "657"))
          {
            string poc38 = "SYS.X167";
            string str31 = "Flood Insurance " + this.loan.GetField("1388") + " months @ $" + this.loan.GetField("235") + " per month";
            string field51 = this.loan.GetField("657");
            this.setREGZValue1(lineID, str31, field51, poc38);
            break;
          }
          if (!this.checkValues("1388", "235"))
            break;
          string str32;
          if (this.loan.GetField("1388") != string.Empty)
            str32 = "Flood Insurance  " + this.loan.GetField("1388") + " months @ $" + this.loan.GetField("235") + " per month";
          else
            str32 = "Flood Insurance         months @ $" + this.loan.GetField("235") + " per month";
          this.setGFEDescription(lineID, str32);
          break;
        case 1007:
          if (this.checkValues("", "1631"))
          {
            string poc39 = "SYS.X239";
            string str33 = this.loan.GetField("1628") + " " + this.loan.GetField("1629") + " months @ $" + this.loan.GetField("1630") + " per month";
            string field52 = this.loan.GetField("1631");
            this.setREGZValue1(lineID, str33, field52, poc39);
            break;
          }
          if (!this.checkValues3("1628", "1629", "1630"))
            break;
          string str34;
          if (this.loan.GetField("1629") != string.Empty)
            str34 = this.loan.GetField("1628") + " " + this.loan.GetField("1629") + " months @ $" + this.loan.GetField("1630") + " per month";
          else
            str34 = this.loan.GetField("1628") + "        months @ $" + this.loan.GetField("1630") + " per month";
          this.setGFEDescription(lineID, str34);
          break;
        case 1008:
          if (this.checkValues("", "658"))
          {
            string poc40 = "SYS.X168";
            string str35 = this.loan.GetField("660") + " " + this.loan.GetField("340") + " months @ $" + this.loan.GetField("253") + " per month";
            string field53 = this.loan.GetField("658");
            this.setREGZValue1(lineID, str35, field53, poc40);
            break;
          }
          if (!this.checkValues3("660", "340", "253"))
            break;
          string str36;
          if (this.loan.GetField("340") != string.Empty)
            str36 = this.loan.GetField("660") + " " + this.loan.GetField("340") + " months @ $" + this.loan.GetField("253") + " per month";
          else
            str36 = this.loan.GetField("660") + "        months @ $" + this.loan.GetField("253") + " per month";
          this.setGFEDescription(lineID, str36);
          break;
        case 1009:
          if (this.checkValues("", "659"))
          {
            string poc41 = "SYS.X169";
            string str37 = this.loan.GetField("661") + " " + this.loan.GetField("341") + " months @ $" + this.loan.GetField("254") + " per month";
            string field54 = this.loan.GetField("659");
            this.setREGZValue1(lineID, str37, field54, poc41);
            break;
          }
          if (!this.checkValues3("661", "341", "254"))
            break;
          string str38;
          if (this.loan.GetField("341") != string.Empty)
            str38 = this.loan.GetField("661") + " " + this.loan.GetField("341") + " months @ $" + this.loan.GetField("254") + " per month";
          else
            str38 = this.loan.GetField("661") + "        months @ $" + this.loan.GetField("254") + " per month";
          this.setGFEDescription(lineID, str38);
          break;
        case 1010:
          if (!this.checkValues("", "558"))
            break;
          string poc42 = "";
          string str39 = "Aggregate Analysis Adjustment";
          string field55 = this.loan.GetField("558");
          this.setREGZValue1(lineID, str39, field55, poc42);
          break;
        case 1101:
          if (!this.checkValues3("610", "", "387"))
            break;
          string poc43 = "SYS.X170";
          string str40 = "Closing or Escrow Fee to " + this.loan.GetField("610");
          string field56 = this.loan.GetField("387");
          this.setREGZValue1(lineID, str40, field56, poc43);
          break;
        case 1102:
          if (!this.checkValues3("L287", "", "L288"))
            break;
          string poc44 = "SYS.X171";
          string str41 = "Abst. or Title Search to " + this.loan.GetField("L287");
          string field57 = this.loan.GetField("L288");
          this.setREGZValue1(lineID, str41, field57, poc44);
          break;
        case 1103:
          if (!this.checkValues3("L290", "", "L291"))
            break;
          string poc45 = "SYS.X172";
          string str42 = "Title Examination to " + this.loan.GetField("L290");
          string field58 = this.loan.GetField("L291");
          this.setREGZValue1(lineID, str42, field58, poc45);
          break;
        case 1104:
          if (!this.checkValues3("L293", "", "L294"))
            break;
          string poc46 = "SYS.X173";
          string str43 = "Title Ins. Binder to " + this.loan.GetField("L293");
          string field59 = this.loan.GetField("L294");
          this.setREGZValue1(lineID, str43, field59, poc46);
          break;
        case 1105:
          if (!this.checkValues3("395", "", "396"))
            break;
          string poc47 = "SYS.X174";
          string str44 = "Document Preparation Fee to " + this.loan.GetField("395");
          string field60 = this.loan.GetField("396");
          this.setREGZValue1(lineID, str44, field60, poc47);
          break;
        case 1106:
          if (!this.checkValues3("391", "", "392"))
            break;
          string poc48 = "SYS.X175";
          string str45 = "Notary Fee to " + this.loan.GetField("391");
          string field61 = this.loan.GetField("392");
          this.setREGZValue1(lineID, str45, field61, poc48);
          break;
        case 1107:
          if (!this.checkValues3("56", "", "978"))
            break;
          string poc49 = "SYS.X176";
          string str46 = "Attorney Fee to " + this.loan.GetField("56");
          string field62 = this.loan.GetField("978");
          this.setREGZValue1(lineID, str46, field62, poc49);
          break;
        case 1108:
          if (!this.checkValues3("411", "", "385"))
            break;
          string poc50 = "SYS.X177";
          string str47 = "Title Insurance Premium to " + this.loan.GetField("411");
          string field63 = this.loan.GetField("385");
          this.setREGZValue1(lineID, str47, field63, poc50);
          break;
        case 1109:
          if (!this.checkValues3("652", "", "646"))
            break;
          string poc51 = "SYS.X181";
          string field64 = this.loan.GetField("652");
          string field65 = this.loan.GetField("646");
          this.setREGZValue1(lineID, field64, field65, poc51);
          break;
        case 1110:
          if (!this.checkValues3("1633", "", "1634"))
            break;
          string poc52 = "SYS.X240";
          string field66 = this.loan.GetField("1633");
          string field67 = this.loan.GetField("1634");
          this.setREGZValue1(lineID, field66, field67, poc52);
          break;
        case 1111:
          if (!this.checkValues3("1762", "", "1763"))
            break;
          string poc53 = "SYS.X244";
          string field68 = this.loan.GetField("1762");
          string field69 = this.loan.GetField("1763");
          this.setREGZValue1(lineID, field68, field69, poc53);
          break;
        case 1112:
          if (!this.checkValues3("1767", "", "1768"))
            break;
          string poc54 = "SYS.X245";
          string field70 = this.loan.GetField("1767");
          string field71 = this.loan.GetField("1768");
          this.setREGZValue1(lineID, field70, field71, poc54);
          break;
        case 1113:
          if (!this.checkValues3("1772", "", "1773"))
            break;
          string poc55 = "SYS.X246";
          string field72 = this.loan.GetField("1772");
          string field73 = this.loan.GetField("1773");
          this.setREGZValue1(lineID, field72, field73, poc55);
          break;
        case 1114:
          if (!this.checkValues3("1777", "", "1778"))
            break;
          string poc56 = "SYS.X247";
          string field74 = this.loan.GetField("1777");
          string field75 = this.loan.GetField("1778");
          this.setREGZValue1(lineID, field74, field75, poc56);
          break;
        case 1201:
          if (!this.checkValues3("1636", "", "390"))
            break;
          string poc57 = "SYS.X182";
          string str48 = "Recording Fee to " + this.loan.GetField("1636");
          string field76 = this.loan.GetField("390");
          this.setREGZValue1(lineID, str48, field76, poc57);
          break;
        case 1202:
          if (!this.checkValues3("1637", "", "647"))
            break;
          string poc58 = "SYS.X183";
          string str49 = "Local Tax/Stamps to " + this.loan.GetField("1637");
          string field77 = this.loan.GetField("647");
          this.setREGZValue1(lineID, str49, field77, poc58);
          break;
        case 1203:
          if (!this.checkValues3("1638", "", "648"))
            break;
          string poc59 = "SYS.X184";
          string str50 = "State Tax/Stamps to " + this.loan.GetField("1638");
          string field78 = this.loan.GetField("648");
          this.setREGZValue1(lineID, str50, field78, poc59);
          break;
        case 1204:
          if (!this.checkValues3("373", "", "374"))
            break;
          string poc60 = "SYS.X185";
          string field79 = this.loan.GetField("373");
          string field80 = this.loan.GetField("374");
          this.setREGZValue1(lineID, field79, field80, poc60);
          break;
        case 1205:
          if (!this.checkValues3("1640", "", "1641"))
            break;
          string poc61 = "SYS.X241";
          string field81 = this.loan.GetField("1640");
          string field82 = this.loan.GetField("1641");
          this.setREGZValue1(lineID, field81, field82, poc61);
          break;
        case 1206:
          if (!this.checkValues3("1643", "", "1644"))
            break;
          string poc62 = "SYS.X242";
          string field83 = this.loan.GetField("1643");
          string field84 = this.loan.GetField("1644");
          this.setREGZValue1(lineID, field83, field84, poc62);
          break;
        case 1301:
          if (!this.checkValues3("375", "", "383"))
            break;
          string poc63 = "SYS.X186";
          string str51 = "Survey to " + this.loan.GetField("375");
          string field85 = this.loan.GetField("383");
          this.setREGZValue1(lineID, str51, field85, poc63);
          break;
        case 1302:
          if (!this.checkValues3("REGZGFE.X15", "", "339"))
            break;
          string poc64 = "SYS.X187";
          string str52 = "Termite/Pest Inspection Fee to " + this.loan.GetField("REGZGFE.X15");
          string field86 = this.loan.GetField("339");
          this.setREGZValue1(lineID, str52, field86, poc64);
          break;
        case 1303:
          if (!this.checkValues3("650", "", "644"))
            break;
          string poc65 = "SYS.X190";
          string field87 = this.loan.GetField("650");
          string field88 = this.loan.GetField("644");
          this.setREGZValue1(lineID, field87, field88, poc65);
          break;
        case 1304:
          if (!this.checkValues3("651", "", "645"))
            break;
          string poc66 = "SYS.X191";
          string field89 = this.loan.GetField("651");
          string field90 = this.loan.GetField("645");
          this.setREGZValue1(lineID, field89, field90, poc66);
          break;
        case 1305:
          if (!this.checkValues3("40", "", "41"))
            break;
          string poc67 = "SYS.X192";
          string field91 = this.loan.GetField("40");
          string field92 = this.loan.GetField("41");
          this.setREGZValue1(lineID, field91, field92, poc67);
          break;
        case 1306:
          if (!this.checkValues3("43", "", "44"))
            break;
          string poc68 = "SYS.X193";
          string field93 = this.loan.GetField("43");
          string field94 = this.loan.GetField("44");
          this.setREGZValue1(lineID, field93, field94, poc68);
          break;
        case 1307:
          if (!this.checkValues3("1782", "", "1783"))
            break;
          string poc69 = "SYS.X248";
          string field95 = this.loan.GetField("1782");
          string field96 = this.loan.GetField("1783");
          this.setREGZValue1(lineID, field95, field96, poc69);
          break;
        case 1308:
          if (!this.checkValues3("1787", "", "1788"))
            break;
          string poc70 = "SYS.X249";
          string field97 = this.loan.GetField("1787");
          string field98 = this.loan.GetField("1788");
          this.setREGZValue1(lineID, field97, field98, poc70);
          break;
        case 1309:
          if (!this.checkValues3("1792", "", "1793"))
            break;
          string poc71 = "SYS.X250";
          string field99 = this.loan.GetField("1792");
          string field100 = this.loan.GetField("1793");
          this.setREGZValue1(lineID, field99, field100, poc71);
          break;
      }
    }

    private void setREGZValue1(int lineID, string value1, string value2, string poc)
    {
      this.fieldPos = this.lineCount * 6;
      int num1 = this.fieldPos + 1;
      if (lineID == 1401 || lineID == 1402)
        this.fieldCollect.Set(this.blockID + num1.ToString(), string.Empty);
      else
        this.fieldCollect.Set(this.blockID + num1.ToString(), lineID.ToString());
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 2).ToString(), value1);
      string str = string.Empty;
      switch (lineID)
      {
        case 803:
          if (this.loan.GetField("GFE1") == "Y" || this.ToDouble(this.loan.GetField("448")) > 0.0)
          {
            str = " (PAID)";
            break;
          }
          break;
        case 804:
          if (this.loan.GetField("GFE2") == "Y" || this.ToDouble(this.loan.GetField("449")) > 0.0)
          {
            str = " (PAID)";
            break;
          }
          break;
        case 810:
          if (this.loan.GetField("GFE3") == "Y" || this.ToDouble(this.loan.GetField("1878")) > 0.0)
          {
            str = " (PAID)";
            break;
          }
          break;
      }
      if (poc == "Y")
      {
        double num2 = this.ToDouble(value2);
        switch (lineID)
        {
          case 803:
            if (this.ToDouble(this.loan.GetField("448")) > 0.0)
            {
              num2 += this.ToDouble(this.loan.GetField("448"));
              break;
            }
            break;
          case 804:
            if (this.ToDouble(this.loan.GetField("449")) > 0.0)
            {
              num2 += this.ToDouble(this.loan.GetField("449"));
              break;
            }
            break;
          case 810:
            if (this.ToDouble(this.loan.GetField("1878")) > 0.0)
            {
              num2 += this.ToDouble(this.loan.GetField("1878"));
              break;
            }
            break;
        }
        value2 = num2 == 0.0 ? string.Empty : num2.ToString("N2");
        int num3 = this.fieldPos + 3;
        if (value2 == string.Empty)
          this.fieldCollect.Set(this.blockID + num3.ToString(), str ?? "");
        else if (lineID == 1401 || lineID == 1402)
          this.fieldCollect.Set(this.blockID + num3.ToString(), str);
        else
          this.fieldCollect.Set(this.blockID + num3.ToString(), "$" + str);
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 4).ToString(), value2);
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 5).ToString(), "");
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 6).ToString(), "");
      }
      else
      {
        double num4 = 0.0;
        switch (lineID)
        {
          case 803:
            if (this.ToDouble(this.loan.GetField("448")) > 0.0)
            {
              num4 = this.ToDouble(this.loan.GetField("448"));
              break;
            }
            break;
          case 804:
            if (this.ToDouble(this.loan.GetField("449")) > 0.0)
            {
              num4 = this.ToDouble(this.loan.GetField("449"));
              break;
            }
            break;
          case 810:
            if (this.ToDouble(this.loan.GetField("1878")) > 0.0)
            {
              num4 = this.ToDouble(this.loan.GetField("1878"));
              break;
            }
            break;
        }
        int num5 = this.fieldPos + 3;
        int num6;
        if (num4 != 0.0)
        {
          this.fieldCollect.Set(this.blockID + num5.ToString(), "$" + str);
          this.fieldCollect.Set(this.blockID + (this.fieldPos + 4).ToString(), num4.ToString("N2"));
        }
        else
        {
          this.fieldCollect.Set(this.blockID + num5.ToString(), "");
          num6 = this.fieldPos + 4;
          this.fieldCollect.Set(this.blockID + num6.ToString(), "");
        }
        num6 = this.fieldPos + 5;
        if (value2 == string.Empty)
          this.fieldCollect.Set(this.blockID + num6.ToString(), "");
        else
          this.fieldCollect.Set(this.blockID + num6.ToString(), "$");
        num6 = this.fieldPos + 6;
        this.fieldCollect.Set(this.blockID + num6.ToString(), value2);
      }
      ++this.lineCount;
    }

    private void printOthers(string formID)
    {
      if (!(formID == "ABILITY-TO-PAY PAGE 5") || this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp() <= 27)
        return;
      this.fieldCollect.Set("FL2702", "See Attached");
      this.fieldCollect.Set("FL2708", "");
      this.fieldCollect.Set("FL2711", "");
      this.fieldCollect.Set("FL2712", "");
      this.fieldCollect.Set("FL2717", "");
    }

    private void printGFEBroker()
    {
      this.blockID = "GFEBRKR2_";
      this.printGFE();
      this.pageDone = string.Empty;
    }

    private void printGFELender()
    {
      this.blockID = "GFELEND2_";
      this.printGFE();
      this.pageDone = string.Empty;
    }

    private void printGFE()
    {
      this.lineCount = 0;
      this.setSubtitle("800", "ITEMS PAYABLE IN CONNECTION WITH LOAN:");
      this.subItemCnt = 0;
      for (int lineID = 801; lineID <= 823; ++lineID)
        this.setGFEStaticField(lineID);
      if (this.subItemCnt == 0)
        --this.lineCount;
      else
        this.addGFEEmptyLine(5);
      this.setSubtitle("", "COMPENSATION TO BROKER (Not Paid Out of Loan Proceeds):");
      this.subItemCnt = 0;
      for (int lineID = 824; lineID <= 825; ++lineID)
        this.setGFEStaticField(lineID);
      if (this.subItemCnt == 0)
        --this.lineCount;
      else
        this.addGFEEmptyLine(5);
      this.setSubtitle("900", "ITEMS REQUIRED BY LENDER TO BE PAID IN ADVANCE:");
      this.subItemCnt = 0;
      for (int lineID = 901; lineID <= 908; ++lineID)
        this.setGFEStaticField(lineID);
      if (this.subItemCnt == 0)
        --this.lineCount;
      else
        this.addGFEEmptyLine(5);
      this.setSubtitle("1000", "RESERVES DEPOSITED WITH LENDER:");
      this.subItemCnt = 0;
      for (int lineID = 1001; lineID <= 1010; ++lineID)
        this.setGFEStaticField(lineID);
      if (this.subItemCnt == 0)
        --this.lineCount;
      else
        this.addGFEEmptyLine(5);
      this.setSubtitle("1100", "TITLE CHARGES:");
      this.subItemCnt = 0;
      for (int lineID = 1101; lineID <= 1114; ++lineID)
        this.setGFEStaticField(lineID);
      if (this.subItemCnt == 0)
        --this.lineCount;
      else
        this.addGFEEmptyLine(5);
      this.setSubtitle("1200", "GOVERNMENT RECORDING AND TRANSFER CHARGES:");
      this.subItemCnt = 0;
      for (int lineID = 1201; lineID <= 1206; ++lineID)
        this.setGFEStaticField(lineID);
      if (this.subItemCnt == 0)
        --this.lineCount;
      else
        this.addGFEEmptyLine(5);
      this.setSubtitle("1300", "ADDITIONAL SETTLEMENT CHARGES:");
      this.subItemCnt = 0;
      for (int lineID = 1301; lineID <= 1309; ++lineID)
        this.setGFEStaticField(lineID);
    }

    private void setGFEStaticField(int lineID)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      switch (lineID)
      {
        case 801:
          if (!this.checkValues("388", "1619") && !this.checkValues("454", "559"))
            break;
          string str1 = this.formID == "GFE LENDER" || this.formID == "GFE BROKER" || this.formID == "GFE - ITEMIZATION" ? "Loan Origination Fee  @ " : "Loan Origination Fee Due Lender  @ ";
          string field1 = this.loan.GetField("SYS.X136");
          string str2 = !(this.loan.GetField("1619") != string.Empty) ? str1 + this.loan.GetField("388") + " %" : str1 + this.loan.GetField("388") + " % + " + this.loan.GetField("1619");
          if (this.Val("REGZGFE.X5") != string.Empty)
            str2 = str2 + "  to " + this.Val("REGZGFE.X5");
          string field2 = this.loan.GetField("454");
          string field3 = this.loan.GetField("559");
          this.setValues(lineID, str2, field2, field3, field1);
          break;
        case 802:
          if (!this.checkValues("1061", "436") && !this.checkValues("1093", "560"))
            break;
          string field4 = this.loan.GetField("SYS.X137");
          string str3 = !(this.loan.GetField("436") != string.Empty) ? "Loan Discount Fee @ " + this.loan.GetField("1061") + " %" : "Loan Discount Fee @ " + this.loan.GetField("1061") + " % + " + this.loan.GetField("436");
          if (this.Val("REGZGFE.X6") != string.Empty)
            str3 = str3 + "  to " + this.Val("REGZGFE.X6");
          string field5 = this.loan.GetField("1093");
          string field6 = this.loan.GetField("560");
          this.setValues(lineID, str3, field5, field6, field4);
          break;
        case 803:
          if (!this.checkValues3("617", "641", "581"))
            break;
          string field7 = this.loan.GetField("SYS.X231");
          string str4 = "Appraisal Fee to " + this.loan.GetField("617");
          string field8 = this.loan.GetField("641");
          string field9 = this.loan.GetField("581");
          this.setValues(lineID, str4, field8, field9, field7);
          break;
        case 804:
          if (!this.checkValues3("624", "640", "580"))
            break;
          string field10 = this.loan.GetField("SYS.X232");
          string str5 = "Credit Report to " + this.loan.GetField("624");
          string field11 = this.loan.GetField("640");
          string field12 = this.loan.GetField("580");
          this.setValues(lineID, str5, field11, field12, field10);
          break;
        case 805:
          if (!this.checkValues3("L704", "329", "557"))
            break;
          string field13 = this.loan.GetField("SYS.X138");
          string str6 = "Lender's Inspection Fee to " + this.loan.GetField("L704");
          string field14 = this.loan.GetField("329");
          string field15 = this.loan.GetField("557");
          this.setValues(lineID, str6, field14, field15, field13);
          break;
        case 806:
          if (!this.checkValues3("L227", "L228", "L229"))
            break;
          string field16 = this.loan.GetField("SYS.X139");
          string str7 = "Mortgage Insurance Application Fee to " + this.loan.GetField("L227");
          string field17 = this.loan.GetField("L228");
          string field18 = this.loan.GetField("L229");
          this.setValues(lineID, str7, field17, field18, field16);
          break;
        case 807:
          if (!this.checkValues3("REGZGFE.X7", "L230", "L231"))
            break;
          string field19 = this.loan.GetField("SYS.X140");
          string str8 = "Assumption Fee to " + this.loan.GetField("REGZGFE.X7");
          string field20 = this.loan.GetField("L230");
          string field21 = this.loan.GetField("L231");
          this.setValues(lineID, str8, field20, field21, field19);
          break;
        case 808:
          if (!this.checkValues("389", "1620") && !this.checkValues("439", "572"))
            break;
          string field22 = this.loan.GetField("SYS.X147");
          string str9 = !(this.loan.GetField("1620") != string.Empty) ? "Mortgage Broker Fee  @ " + this.loan.GetField("389") + " %" : "Mortgage Broker Fee  @ " + this.loan.GetField("389") + " % + " + this.loan.GetField("1620");
          if (this.Val("REGZGFE.X14") != string.Empty)
            str9 = str9 + "  to " + this.Val("REGZGFE.X14");
          string field23 = this.loan.GetField("439");
          string field24 = this.loan.GetField("572");
          this.setValues(lineID, str9, field23, field24, field22);
          break;
        case 809:
          if (!this.checkValues3("L224", "336", "565"))
            break;
          string field25 = this.loan.GetField("SYS.X141");
          string str10 = "Tax Servicing Fee to " + this.loan.GetField("L224");
          string field26 = this.loan.GetField("336");
          string field27 = this.loan.GetField("565");
          this.setValues(lineID, str10, field26, field27, field25);
          break;
        case 810:
          if (!this.checkValues3("1812", "1621", "1622"))
            break;
          string field28 = this.loan.GetField("SYS.X233");
          string str11 = "Processing Fee to " + this.loan.GetField("1812");
          string field29 = this.loan.GetField("1621");
          string field30 = this.loan.GetField("1622");
          this.setValues(lineID, str11, field29, field30, field28);
          break;
        case 811:
          if (!this.checkValues3("REGZGFE.X8", "367", "569"))
            break;
          string field31 = this.loan.GetField("SYS.X142");
          string str12 = "Underwriting Fee to " + this.loan.GetField("REGZGFE.X8");
          string field32 = this.loan.GetField("367");
          string field33 = this.loan.GetField("569");
          this.setValues(lineID, str12, field32, field33, field31);
          break;
        case 812:
          if (!this.checkValues3("1813", "1623", "1624"))
            break;
          string field34 = this.loan.GetField("SYS.X234");
          string str13 = "Wire Transfer Fee to " + this.loan.GetField("1813");
          string field35 = this.loan.GetField("1623");
          string field36 = this.loan.GetField("1624");
          this.setValues(lineID, str13, field35, field36, field34);
          break;
        case 813:
          if (!this.checkValues3("369", "370", "574"))
            break;
          string field37 = this.loan.GetField("SYS.X149");
          string field38 = this.loan.GetField("369");
          string field39 = this.loan.GetField("370");
          string field40 = this.loan.GetField("574");
          this.setValues(lineID, field38, field39, field40, field37);
          break;
        case 814:
          if (!this.checkValues3("371", "372", "575"))
            break;
          string field41 = this.loan.GetField("SYS.X150");
          string field42 = this.loan.GetField("371");
          string field43 = this.loan.GetField("372");
          string field44 = this.loan.GetField("575");
          this.setValues(lineID, field42, field43, field44, field41);
          break;
        case 815:
          if (!this.checkValues3("348", "349", "96"))
            break;
          string field45 = this.loan.GetField("SYS.X151");
          string field46 = this.loan.GetField("348");
          string field47 = this.loan.GetField("349");
          string field48 = this.loan.GetField("96");
          this.setValues(lineID, field46, field47, field48, field45);
          break;
        case 816:
          if (!this.checkValues3("931", "932", "1345"))
            break;
          string field49 = this.loan.GetField("SYS.X152");
          string field50 = this.loan.GetField("931");
          string field51 = this.loan.GetField("932");
          string field52 = this.loan.GetField("1345");
          this.setValues(lineID, field50, field51, field52, field49);
          break;
        case 817:
          if (!this.checkValues3("1390", "1009", "6"))
            break;
          string field53 = this.loan.GetField("SYS.X153");
          string field54 = this.loan.GetField("1390");
          string field55 = this.loan.GetField("1009");
          string field56 = this.loan.GetField("6");
          this.setValues(lineID, field54, field55, field56, field53);
          break;
        case 818:
          if (!this.checkValues3("410", "554", "678"))
            break;
          string field57 = this.loan.GetField("SYS.X154");
          string field58 = this.loan.GetField("410");
          string field59 = this.loan.GetField("554");
          string field60 = this.loan.GetField("678");
          this.setValues(lineID, field58, field59, field60, field57);
          break;
        case 819:
          if (!this.checkValues3("1391", "81", "82"))
            break;
          string field61 = this.loan.GetField("SYS.X155");
          string field62 = this.loan.GetField("1391");
          string field63 = this.loan.GetField("81");
          string field64 = this.loan.GetField("82");
          this.setValues(lineID, field62, field63, field64, field61);
          break;
        case 820:
          if (!this.checkValues3("154", "155", "200"))
            break;
          string field65 = this.loan.GetField("SYS.X156");
          string field66 = this.loan.GetField("154");
          string field67 = this.loan.GetField("155");
          string field68 = this.loan.GetField("200");
          this.setValues(lineID, field66, field67, field68, field65);
          break;
        case 821:
          if (!this.checkValues3("1627", "1625", "1626"))
            break;
          string field69 = this.loan.GetField("SYS.X243");
          string field70 = this.loan.GetField("1627");
          string field71 = this.loan.GetField("1625");
          string field72 = this.loan.GetField("1626");
          this.setValues(lineID, field70, field71, field72, field69);
          break;
        case 822:
          if (!this.checkValues3("1838", "1839", "1840"))
            break;
          string field73 = this.loan.GetField("SYS.X293");
          string field74 = this.loan.GetField("1838");
          string field75 = this.loan.GetField("1839");
          string field76 = this.loan.GetField("1840");
          this.setValues(lineID, field74, field75, field76, field73);
          break;
        case 823:
          if (!this.checkValues3("1841", "1842", "1843"))
            break;
          string field77 = this.loan.GetField("SYS.X298");
          string field78 = this.loan.GetField("1841");
          string field79 = this.loan.GetField("1842");
          string field80 = this.loan.GetField("1843");
          this.setValues(lineID, field78, field79, field80, field77);
          break;
        case 824:
          if (!this.checkValues3("1662", "1847", "1663") || !(this.loan.GetField("1970") != "Y"))
            break;
          string poc1 = "Y";
          string str14 = this.loan.GetField("1662") + " @ " + this.loan.GetField("1847") + " %";
          string field81 = this.loan.GetField("1663");
          string empty7 = string.Empty;
          this.setValues(lineID, str14, field81, empty7, poc1);
          break;
        case 825:
          if (!this.checkValues3("1664", "1848", "1665") || !(this.loan.GetField("1970") != "Y"))
            break;
          string poc2 = "Y";
          string str15 = this.loan.GetField("1664") + " @ " + this.loan.GetField("1848") + " %";
          string field82 = this.loan.GetField("1665");
          string empty8 = string.Empty;
          this.setValues(lineID, str15, field82, empty8, poc2);
          break;
        case 901:
          if (!this.checkValues("332", "333") && !this.checkValues("334", "561"))
            break;
          string field83 = this.loan.GetField("SYS.X157");
          string empty9 = string.Empty;
          string str16 = !(this.loan.GetSimpleField("SYS.X8") == "Y") ? this.loan.GetField("333") : Utils.ArithmeticRounding(Utils.ParseDouble((object) this.loan.GetSimpleField("333")), 2).ToString("N2");
          string str17 = "Prepaid Interest " + this.loan.GetField("332") + " days @ " + str16;
          string field84 = this.loan.GetField("334");
          string field85 = this.loan.GetField("561");
          this.setValues(lineID, str17, field84, field85, field83);
          break;
        case 902:
          if (!this.checkValues3("L248", "337", "562"))
            break;
          string field86 = this.loan.GetField("SYS.X158");
          string str18 = "Private Mortgage Insurance Premium To " + this.loan.GetField("L248");
          string field87 = this.loan.GetField("337");
          string field88 = this.loan.GetField("562");
          this.setValues(lineID, str18, field87, field88, field86);
          break;
        case 903:
          if (!this.checkValues("L251", "L252") && !this.checkValues("642", "578"))
            break;
          string field89 = this.loan.GetField("SYS.X159");
          string str19 = "First Year Hazard Insurance Premium for " + this.loan.GetField("L251") + " mths to " + this.loan.GetField("L252");
          string field90 = this.loan.GetField("642");
          string field91 = this.loan.GetField("578");
          this.setValues(lineID, str19, field90, field91, field89);
          break;
        case 904:
          if (!this.checkValues3("1955", "1849", "1850"))
            break;
          string field92 = this.loan.GetField("SYS.X388");
          string str20 = "County Property Taxes to " + this.loan.GetField("1955");
          string field93 = this.loan.GetField("1849");
          string field94 = this.loan.GetField("1850");
          this.setValues(lineID, str20, field93, field94, field92);
          break;
        case 905:
          if (!this.checkValues3("1956", "1050", "571"))
            break;
          string field95 = this.loan.GetField("SYS.X235");
          string str21 = "VA Funding Fee to " + this.loan.GetField("1956");
          string field96 = this.loan.GetField("1050");
          string field97 = this.loan.GetField("571");
          this.setValues(lineID, str21, field96, field97, field95);
          break;
        case 906:
          if (!this.checkValues3("1500", "643", "579"))
            break;
          string field98 = this.loan.GetField("SYS.X160");
          string str22 = "First Year Flood Insurance Premium To " + this.loan.GetField("1500");
          string field99 = this.loan.GetField("643");
          string field100 = this.loan.GetField("579");
          this.setValues(lineID, str22, field99, field100, field98);
          break;
        case 907:
          if (!this.checkValues3("L259", "L260", "L261"))
            break;
          string field101 = this.loan.GetField("SYS.X161");
          string field102 = this.loan.GetField("L259");
          string field103 = this.loan.GetField("L260");
          string field104 = this.loan.GetField("L261");
          this.setValues(lineID, field102, field103, field104, field101);
          break;
        case 908:
          if (!this.checkValues3("1666", "1667", "1668"))
            break;
          string field105 = this.loan.GetField("SYS.X238");
          string field106 = this.loan.GetField("1666");
          string field107 = this.loan.GetField("1667");
          string field108 = this.loan.GetField("1668");
          this.setValues(lineID, field106, field107, field108, field105);
          break;
        case 1001:
          if (this.checkValues("656", "596"))
          {
            string field109 = this.loan.GetField("SYS.X162");
            string str23 = "Hazard Insurance Impounds " + this.loan.GetField("1387") + " months @ " + this.loan.GetField("230") + " per month";
            string field110 = this.loan.GetField("656");
            string field111 = this.loan.GetField("596");
            this.setValues(lineID, str23, field110, field111, field109);
            break;
          }
          if (!this.checkValues("1387", "230"))
            break;
          string str24;
          if (this.loan.GetField("1387") != string.Empty)
            str24 = "Hazard Insurance Impounds " + this.loan.GetField("1387") + " months @ " + this.loan.GetField("230") + " per month";
          else
            str24 = "Hazard Insurance Impounds        months @ " + this.loan.GetField("230") + " per month";
          this.setGFEDescription(lineID, str24);
          break;
        case 1002:
          if (this.checkValues("338", "563"))
          {
            string field112 = this.loan.GetField("SYS.X163");
            string str25 = "Mortgage Insurance Impounds " + this.loan.GetField("1296") + " months @ " + this.loan.GetField("232") + " per month";
            string field113 = this.loan.GetField("338");
            string field114 = this.loan.GetField("563");
            this.setValues(lineID, str25, field113, field114, field112);
            break;
          }
          if (!this.checkValues("1296", "232"))
            break;
          string str26;
          if (this.loan.GetField("1296") != "")
            str26 = "Mortgage Insurance Impounds " + this.loan.GetField("1296") + " months @ " + this.loan.GetField("232") + " per month";
          else
            str26 = "Mortgage Insurance Impounds        months @ " + this.loan.GetField("232") + " per month";
          this.setGFEDescription(lineID, str26);
          break;
        case 1003:
          if (this.checkValues("L269", "L270"))
          {
            string field115 = this.loan.GetField("SYS.X164");
            string str27 = "City Property Taxes " + this.loan.GetField("L267") + " months @ " + this.loan.GetField("L268") + " per month";
            string field116 = this.loan.GetField("L269");
            string field117 = this.loan.GetField("L270");
            this.setValues(lineID, str27, field116, field117, field115);
            break;
          }
          if (!this.checkValues("L268", "L267"))
            break;
          string str28;
          if (this.loan.GetField("L267") != "")
            str28 = "City Property Taxes  " + this.loan.GetField("L267") + " months @ " + this.loan.GetField("L268") + " per month";
          else
            str28 = "City Property Taxes        months @ " + this.loan.GetField("L268") + " per month";
          this.setGFEDescription(lineID, str28);
          break;
        case 1004:
          if (this.checkValues("655", "595"))
          {
            string field118 = this.loan.GetField("SYS.X165");
            string str29 = "Property Tax Impounds " + this.loan.GetField("1386") + " months @ " + this.loan.GetField("231") + " per month";
            string field119 = this.loan.GetField("655");
            string field120 = this.loan.GetField("595");
            this.setValues(lineID, str29, field119, field120, field118);
            break;
          }
          if (!this.checkValues("1386", "231"))
            break;
          string str30;
          if (this.loan.GetField("1386") != "")
            str30 = "Property Tax Impounds " + this.loan.GetField("1386") + " months @ " + this.loan.GetField("231") + " per month";
          else
            str30 = "Property Tax Impounds        months @ " + this.loan.GetField("231") + " per month";
          this.setGFEDescription(lineID, str30);
          break;
        case 1006:
          if (this.checkValues("657", "597"))
          {
            string field121 = this.loan.GetField("SYS.X167");
            string str31 = "Flood Insurance Impounds " + this.loan.GetField("1388") + " months @ " + this.loan.GetField("235") + " per month";
            string field122 = this.loan.GetField("657");
            string field123 = this.loan.GetField("597");
            this.setValues(lineID, str31, field122, field123, field121);
            break;
          }
          if (!this.checkValues("1388", "235"))
            break;
          string str32;
          if (this.loan.GetField("1388") != "")
            str32 = "Flood Insurance Impounds " + this.loan.GetField("1388") + " months @ " + this.loan.GetField("235") + " per month";
          else
            str32 = "Flood Insurance Impounds        months @ " + this.loan.GetField("235") + " per month";
          this.setGFEDescription(lineID, str32);
          break;
        case 1007:
          if (this.checkValues3("1628", "1631", "1632"))
          {
            string field124 = this.loan.GetField("SYS.X239");
            string str33 = this.loan.GetField("1628") + " " + this.loan.GetField("1629") + " months @ " + this.loan.GetField("1630") + " per month";
            string field125 = this.loan.GetField("1631");
            string field126 = this.loan.GetField("1632");
            this.setValues(lineID, str33, field125, field126, field124);
            break;
          }
          if (!this.checkValues("1629", "1630"))
            break;
          string str34;
          if (this.loan.GetField("1388") != "")
            str34 = this.loan.GetField("1628") + " " + this.loan.GetField("1629") + " months @ " + this.loan.GetField("1630") + " per month";
          else
            str34 = this.loan.GetField("1628") + "        months @ " + this.loan.GetField("1630") + " per month";
          this.setGFEDescription(lineID, str34);
          break;
        case 1008:
          if (this.checkValues3("660", "658", "598"))
          {
            string field127 = this.loan.GetField("SYS.X168");
            string str35 = this.loan.GetField("660") + " " + this.loan.GetField("340") + " months @ " + this.loan.GetField("253") + " per month";
            string field128 = this.loan.GetField("658");
            string field129 = this.loan.GetField("598");
            this.setValues(lineID, str35, field128, field129, field127);
            break;
          }
          if (!this.checkValues("340", "253"))
            break;
          string str36;
          if (this.loan.GetField("340") != "")
            str36 = this.loan.GetField("660") + " " + this.loan.GetField("340") + " months @ " + this.loan.GetField("253") + " per month";
          else
            str36 = this.loan.GetField("660") + "        months @ " + this.loan.GetField("253") + " per month";
          this.setGFEDescription(lineID, str36);
          break;
        case 1009:
          if (this.checkValues3("661", "659", "599"))
          {
            string field130 = this.loan.GetField("SYS.X169");
            string str37 = this.loan.GetField("661") + " " + this.loan.GetField("341") + " months @ " + this.loan.GetField("254") + " per month";
            string field131 = this.loan.GetField("659");
            string field132 = this.loan.GetField("599");
            this.setValues(lineID, str37, field131, field132, field130);
            break;
          }
          if (!this.checkValues("341", "254"))
            break;
          string str38;
          if (this.loan.GetField("341") != "")
            str38 = this.loan.GetField("661") + " " + this.loan.GetField("341") + " months @ " + this.loan.GetField("254") + " per month";
          else
            str38 = this.loan.GetField("661") + "        months @ " + this.loan.GetField("254") + " per month";
          this.setGFEDescription(lineID, str38);
          break;
        case 1010:
          if (!this.checkValues("558", ""))
            break;
          string poc3 = "";
          string str39 = "Aggregate Analysis Adjustment";
          string field133 = this.loan.GetField("558");
          string str40 = "";
          this.setValues(lineID, str39, field133, str40, poc3);
          break;
        case 1101:
          if (!this.checkValues3("610", "387", "582"))
            break;
          string field134 = this.loan.GetField("SYS.X170");
          string str41 = "Settlement or Closing Fee to " + this.loan.GetField("610");
          string field135 = this.loan.GetField("387");
          string field136 = this.loan.GetField("582");
          this.setValues(lineID, str41, field135, field136, field134);
          break;
        case 1102:
          if (!this.checkValues3("L287", "L288", "L289"))
            break;
          string field137 = this.loan.GetField("SYS.X171");
          string str42 = "Abst. or Title Search to " + this.loan.GetField("L287");
          string field138 = this.loan.GetField("L288");
          string field139 = this.loan.GetField("L289");
          this.setValues(lineID, str42, field138, field139, field137);
          break;
        case 1103:
          if (!this.checkValues3("L290", "L291", "L292"))
            break;
          string field140 = this.loan.GetField("SYS.X172");
          string str43 = "Title Examination to " + this.loan.GetField("L290");
          string field141 = this.loan.GetField("L291");
          string field142 = this.loan.GetField("L292");
          this.setValues(lineID, str43, field141, field142, field140);
          break;
        case 1104:
          if (!this.checkValues3("L293", "L294", "L295"))
            break;
          string field143 = this.loan.GetField("SYS.X173");
          string str44 = "Title Ins. Binder to " + this.loan.GetField("L293");
          string field144 = this.loan.GetField("L294");
          string field145 = this.loan.GetField("L295");
          this.setValues(lineID, str44, field144, field145, field143);
          break;
        case 1105:
          if (!this.checkValues3("395", "396", "583"))
            break;
          string field146 = this.loan.GetField("SYS.X174");
          string str45 = "Document Preparation Fee to " + this.loan.GetField("395");
          string field147 = this.loan.GetField("396");
          string field148 = this.loan.GetField("583");
          this.setValues(lineID, str45, field147, field148, field146);
          break;
        case 1106:
          if (!this.checkValues3("391", "392", "584"))
            break;
          string field149 = this.loan.GetField("SYS.X175");
          string str46 = "Notary Fee to " + this.loan.GetField("391");
          string field150 = this.loan.GetField("392");
          string field151 = this.loan.GetField("584");
          this.setValues(lineID, str46, field150, field151, field149);
          break;
        case 1107:
          if (!this.checkValues3("56", "978", "1049"))
            break;
          string field152 = this.loan.GetField("SYS.X176");
          string str47 = "Attorney Fee to " + this.loan.GetField("56");
          string field153 = this.loan.GetField("978");
          string field154 = this.loan.GetField("1049");
          this.setValues(lineID, str47, field153, field154, field152);
          break;
        case 1108:
          if (!this.checkValues3("411", "385", "585"))
            break;
          string field155 = this.loan.GetField("SYS.X177");
          string str48 = "Title Insurance Premium to " + this.loan.GetField("411");
          string field156 = this.loan.GetField("385");
          string field157 = this.loan.GetField("585");
          this.setValues(lineID, str48, field156, field157, field155);
          break;
        case 1109:
          if (!this.checkValues3("652", "646", "592"))
            break;
          string field158 = this.loan.GetField("SYS.X181");
          string field159 = this.loan.GetField("652");
          string field160 = this.loan.GetField("646");
          string field161 = this.loan.GetField("592");
          this.setValues(lineID, field159, field160, field161, field158);
          break;
        case 1110:
          if (!this.checkValues3("1633", "1634", "1635"))
            break;
          string field162 = this.loan.GetField("SYS.X240");
          string field163 = this.loan.GetField("1633");
          string field164 = this.loan.GetField("1634");
          string field165 = this.loan.GetField("1635");
          this.setValues(lineID, field163, field164, field165, field162);
          break;
        case 1111:
          if (!this.checkValues3("1762", "1763", "1764"))
            break;
          string field166 = this.loan.GetField("SYS.X244");
          string field167 = this.loan.GetField("1762");
          string field168 = this.loan.GetField("1763");
          string field169 = this.loan.GetField("1764");
          this.setValues(lineID, field167, field168, field169, field166);
          break;
        case 1112:
          if (!this.checkValues3("1767", "1768", "1769"))
            break;
          string field170 = this.loan.GetField("SYS.X245");
          string field171 = this.loan.GetField("1767");
          string field172 = this.loan.GetField("1768");
          string field173 = this.loan.GetField("1769");
          this.setValues(lineID, field171, field172, field173, field170);
          break;
        case 1113:
          if (!this.checkValues3("1772", "1773", "1774"))
            break;
          string field174 = this.loan.GetField("SYS.X246");
          string field175 = this.loan.GetField("1772");
          string field176 = this.loan.GetField("1773");
          string field177 = this.loan.GetField("1774");
          this.setValues(lineID, field175, field176, field177, field174);
          break;
        case 1114:
          if (!this.checkValues3("1777", "1778", "1779"))
            break;
          string field178 = this.loan.GetField("SYS.X247");
          string field179 = this.loan.GetField("1777");
          string field180 = this.loan.GetField("1778");
          string field181 = this.loan.GetField("1779");
          this.setValues(lineID, field179, field180, field181, field178);
          break;
        case 1201:
          if (!this.checkValues3("1636", "390", "587"))
            break;
          string field182 = this.loan.GetField("SYS.X182");
          string str49 = "Recording Fee to " + this.loan.GetField("1636");
          string field183 = this.loan.GetField("390");
          string field184 = this.loan.GetField("587");
          this.setValues(lineID, str49, field183, field184, field182);
          break;
        case 1202:
          if (!this.checkValues3("1637", "647", "593"))
            break;
          string field185 = this.loan.GetField("SYS.X183");
          string str50 = "Local Tax/Stamps to " + this.loan.GetField("1637");
          string field186 = this.loan.GetField("647");
          string field187 = this.loan.GetField("593");
          this.setValues(lineID, str50, field186, field187, field185);
          break;
        case 1203:
          if (!this.checkValues3("1638", "648", "594"))
            break;
          string field188 = this.loan.GetField("SYS.X184");
          string str51 = "State Tax/Stamps to " + this.loan.GetField("1638");
          string field189 = this.loan.GetField("648");
          string field190 = this.loan.GetField("594");
          this.setValues(lineID, str51, field189, field190, field188);
          break;
        case 1204:
          if (!this.checkValues3("373", "374", "576"))
            break;
          string field191 = this.loan.GetField("SYS.X185");
          string field192 = this.loan.GetField("373");
          string field193 = this.loan.GetField("374");
          string field194 = this.loan.GetField("576");
          this.setValues(lineID, field192, field193, field194, field191);
          break;
        case 1205:
          if (!this.checkValues3("1640", "1641", "1642"))
            break;
          string field195 = this.loan.GetField("SYS.X241");
          string field196 = this.loan.GetField("1640");
          string field197 = this.loan.GetField("1641");
          string field198 = this.loan.GetField("1642");
          this.setValues(lineID, field196, field197, field198, field195);
          break;
        case 1206:
          if (!this.checkValues3("1643", "1644", "1645"))
            break;
          string field199 = this.loan.GetField("SYS.X242");
          string field200 = this.loan.GetField("1643");
          string field201 = this.loan.GetField("1644");
          string field202 = this.loan.GetField("1645");
          this.setValues(lineID, field200, field201, field202, field199);
          break;
        case 1301:
          if (!this.checkValues3("375", "383", "577"))
            break;
          string field203 = this.loan.GetField("SYS.X186");
          string str52 = "Survey Fee to " + this.loan.GetField("375");
          string field204 = this.loan.GetField("383");
          string field205 = this.loan.GetField("577");
          this.setValues(lineID, str52, field204, field205, field203);
          break;
        case 1302:
          if (!this.checkValues3("REGZGFE.X15", "339", "564"))
            break;
          string field206 = this.loan.GetField("SYS.X187");
          string str53 = "Termite/Pest Inspection Fee to " + this.loan.GetField("REGZGFE.X15");
          string field207 = this.loan.GetField("339");
          string field208 = this.loan.GetField("564");
          this.setValues(lineID, str53, field207, field208, field206);
          break;
        case 1303:
          if (!this.checkValues3("650", "644", "590"))
            break;
          string field209 = this.loan.GetField("SYS.X190");
          string field210 = this.loan.GetField("650");
          string field211 = this.loan.GetField("644");
          string field212 = this.loan.GetField("590");
          this.setValues(lineID, field210, field211, field212, field209);
          break;
        case 1304:
          if (!this.checkValues3("651", "645", "591"))
            break;
          string field213 = this.loan.GetField("SYS.X191");
          string field214 = this.loan.GetField("651");
          string field215 = this.loan.GetField("645");
          string field216 = this.loan.GetField("591");
          this.setValues(lineID, field214, field215, field216, field213);
          break;
        case 1305:
          if (!this.checkValues3("40", "41", "42"))
            break;
          string field217 = this.loan.GetField("SYS.X192");
          string field218 = this.loan.GetField("40");
          string field219 = this.loan.GetField("41");
          string field220 = this.loan.GetField("42");
          this.setValues(lineID, field218, field219, field220, field217);
          break;
        case 1306:
          if (!this.checkValues3("43", "44", "55"))
            break;
          string field221 = this.loan.GetField("SYS.X193");
          string field222 = this.loan.GetField("43");
          string field223 = this.loan.GetField("44");
          string field224 = this.loan.GetField("55");
          this.setValues(lineID, field222, field223, field224, field221);
          break;
        case 1307:
          if (!this.checkValues3("1782", "1783", "1784"))
            break;
          string field225 = this.loan.GetField("SYS.X248");
          string field226 = this.loan.GetField("1782");
          string field227 = this.loan.GetField("1783");
          string field228 = this.loan.GetField("1784");
          this.setValues(lineID, field226, field227, field228, field225);
          break;
        case 1308:
          if (!this.checkValues3("1787", "1788", "1789"))
            break;
          string field229 = this.loan.GetField("SYS.X249");
          string field230 = this.loan.GetField("1787");
          string field231 = this.loan.GetField("1788");
          string field232 = this.loan.GetField("1789");
          this.setValues(lineID, field230, field231, field232, field229);
          break;
        case 1309:
          if (!this.checkValues3("1792", "1793", "1794"))
            break;
          string field233 = this.loan.GetField("SYS.X250");
          string field234 = this.loan.GetField("1792");
          string field235 = this.loan.GetField("1793");
          string field236 = this.loan.GetField("1794");
          this.setValues(lineID, field234, field235, field236, field233);
          break;
      }
    }

    private void setValues(int lineID, string value1, string value2, string value3, string poc)
    {
      ++this.subItemCnt;
      this.fieldPos = this.lineCount * 5;
      int num1 = this.fieldPos + 1;
      if (lineID == 1400 || lineID == 1401)
        this.fieldCollect.Set(this.blockID + num1.ToString(), string.Empty);
      else
        this.fieldCollect.Set(this.blockID + num1.ToString(), lineID.ToString());
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 2).ToString(), value1);
      bool flag = false;
      double num2 = 0.0;
      if (lineID == 803 || lineID == 804 || lineID == 810)
      {
        switch (lineID)
        {
          case 803:
            if (this.loan.GetField("GFE1") == "Y" || this.ToDouble(this.loan.GetField("448")) > 0.0)
            {
              flag = true;
              num2 = this.ToDouble(this.loan.GetField("448"));
              break;
            }
            break;
          case 804:
            if (this.loan.GetField("GFE2") == "Y" || this.ToDouble(this.loan.GetField("449")) > 0.0)
            {
              flag = true;
              num2 = this.ToDouble(this.loan.GetField("449"));
              break;
            }
            break;
          default:
            if (lineID == 810 && (this.loan.GetField("GFE3") == "Y" || this.ToDouble(this.loan.GetField("1878")) > 0.0))
            {
              flag = true;
              num2 = this.ToDouble(this.loan.GetField("1878"));
              break;
            }
            break;
        }
        if (flag && poc == "Y")
        {
          double num3 = num2 + this.ToDouble(value2);
          value2 = num3 == 0.0 ? string.Empty : num3.ToString("N2");
        }
      }
      if (poc == "Y")
      {
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 3).ToString(), value2);
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 4).ToString(), "");
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 5).ToString(), value3);
      }
      else
      {
        int num4 = this.fieldPos + 3;
        if (num2 > 0.0)
          this.fieldCollect.Set(this.blockID + num4.ToString(), num2.ToString("N2"));
        else
          this.fieldCollect.Set(this.blockID + num4.ToString(), "");
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 4).ToString(), value2);
        this.fieldCollect.Set(this.blockID + (this.fieldPos + 5).ToString(), value3);
      }
      ++this.lineCount;
    }

    private void setSubtitle(string value1, string value2)
    {
      this.fieldPos = this.lineCount * 5;
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 1).ToString(), "");
      int num = this.fieldPos + 2;
      if (value1 == string.Empty)
        this.fieldCollect.Set(this.blockID + num.ToString(), value2);
      else
        this.fieldCollect.Set(this.blockID + num.ToString(), value1 + "  " + value2);
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 3).ToString(), "");
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 4).ToString(), "");
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 5).ToString(), "");
      ++this.lineCount;
    }

    private bool checkValues(string sFldID1, string sFldID2)
    {
      string s1 = "";
      if (sFldID1 != "")
        s1 = this.loan.GetField(sFldID1);
      string s2 = "";
      if (sFldID2 != "")
        s2 = this.loan.GetField(sFldID2);
      double result;
      if (!double.TryParse(s1, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
        s1 = string.Empty;
      if (!double.TryParse(s2, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
        s2 = string.Empty;
      return s1 != string.Empty || s2 != string.Empty;
    }

    private void addGFEEmptyLine(int lines)
    {
      this.fieldPos = this.lineCount * lines;
      for (int index = 1; index <= lines; ++index)
        this.fieldCollect.Set(this.blockID + (this.fieldPos + index).ToString(), "");
      ++this.lineCount;
    }

    private void setGFEDescription(int lineID, string value1)
    {
      if (this.blockID == "REGZGFE_")
      {
        this.fieldPos = this.lineCount * 6;
      }
      else
      {
        ++this.subItemCnt;
        this.fieldPos = this.lineCount * 5;
      }
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 1).ToString(), lineID.ToString());
      this.fieldCollect.Set(this.blockID + (this.fieldPos + 2).ToString(), value1);
      int num = this.fieldPos + 3;
      this.fieldCollect.Set(this.blockID + num.ToString(), "");
      num = this.fieldPos + 4;
      this.fieldCollect.Set(this.blockID + num.ToString(), "");
      num = this.fieldPos + 5;
      this.fieldCollect.Set(this.blockID + num.ToString(), "");
      if (this.blockID == "REGZGFE_")
      {
        num = this.fieldPos + 6;
        this.fieldCollect.Set(this.blockID + num.ToString(), "");
      }
      ++this.lineCount;
    }

    private bool checkValues3(string sFldID1, string sFldID2, string sFldID3)
    {
      string str = "";
      if (sFldID1 != "")
        str = this.loan.GetField(sFldID1);
      string s1 = "";
      if (sFldID2 != "")
        s1 = this.loan.GetField(sFldID2);
      string s2 = "";
      if (sFldID3 != "")
        s2 = this.loan.GetField(sFldID3);
      double result;
      if (!double.TryParse(s1, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
        s1 = string.Empty;
      if (!double.TryParse(s2, NumberStyles.Any, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result))
        s2 = string.Empty;
      return str != string.Empty || s1 != string.Empty || s2 != string.Empty;
    }

    private void checkTodayDate(string id) => this.checkTodayDate(this.loan, id);

    private void checkTodayDate(LoanData currentLoan, string id)
    {
      string simpleField = currentLoan.GetSimpleField(id);
      if (!((simpleField ?? "") == string.Empty) && !(simpleField == "//"))
        return;
      string str = DateTime.Now.ToString("MM/dd/yyyy");
      this.fieldCollect.Set(id, str);
    }
  }
}
