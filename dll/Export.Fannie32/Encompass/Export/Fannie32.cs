// Decompiled with JetBrains decompiler
// Type: Encompass.Export.Fannie32
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using EllieMae.EMLite.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Encompass.Export
{
  public class Fannie32 : IExport
  {
    private const int iENV = 0;
    private const int i000 = 1;
    private const int i00A = 2;
    private const int i01A = 3;
    private const int i02A = 4;
    private const int i02B = 5;
    private const int i02C = 6;
    private const int i02D = 7;
    private const int i02E = 8;
    private const int i03A = 9;
    private const int i03B = 10;
    private const int i03C = 11;
    private const int i04A = 12;
    private const int i04B = 13;
    private const int i05H = 14;
    private const int i05I = 15;
    private const int i06A = 16;
    private const int i06B = 17;
    private const int i06C = 18;
    private const int i06D = 19;
    private const int i06F = 20;
    private const int i06G = 21;
    private const int i06H = 22;
    private const int i06L = 23;
    private const int i06S = 24;
    private const int i07A = 25;
    private const int i07B = 26;
    private const int i08A = 27;
    private const int i08B = 28;
    private const int i09A = 29;
    private const int i10A = 30;
    private const int i10B = 31;
    private const int i10R = 32;
    private const int i99B = 33;
    private const int iADS = 34;
    private const int iBUA = 35;
    private const int iLNC = 36;
    private const int iGOA = 37;
    private const int iGOD = 38;
    private const int iGOE = 39;
    private const int iLMD = 40;
    private const int iCENV = 41;
    private string newLine = Environment.NewLine;
    private string loanPurpose = string.Empty;
    private int startingBorPair;
    private int endingBorPair;
    private readonly string[] amtFlds = new string[2]
    {
      "3015",
      "3022"
    };
    private readonly string[] dscFlds = new string[2]
    {
      "3009",
      "3016"
    };
    private readonly string[,] codesFlg = new string[2, 4]
    {
      {
        "3011",
        "3012",
        "3013",
        "3010"
      },
      {
        "3018",
        "3019",
        "3020",
        "3017"
      }
    };
    private readonly string[] codes = new string[4]
    {
      "H5",
      "H1",
      "H6",
      "H3"
    };
    private readonly HmdaFieldsExport _hmdaFieldsExport;
    private bool typeCode92Exported;
    private bool typeCode96Exported;
    private bool _exportVendorFieldsForBorrower;
    private bool _exportVendorFieldsForCoBorrower;
    private bool _printFips = true;
    private StringBuilder[] fnmData = new StringBuilder[42];
    private StringBuilder element;
    private SortedList reoProperties = new SortedList();
    private IBam loan;

    public Fannie32() => this._hmdaFieldsExport = new HmdaFieldsExport(this);

    public IBam Bam
    {
      get => this.loan;
      set => this.loan = value;
    }

    public string ExportData() => this.ExportData(false);

    public string ExportData(bool currentBorPairOnly)
    {
      for (int index = 0; index < this.fnmData.Length; ++index)
        this.fnmData[index] = new StringBuilder();
      this.BuildHeader();
      this.Build000();
      this.Build00A();
      this.Build01A();
      this.Build02A();
      this.Build02B();
      this.Build02C();
      this.Build02D();
      this.Build02E(currentBorPairOnly);
      int currentBorrowerPair = this.loan.GetCurrentBorrowerPair();
      if (currentBorPairOnly)
      {
        this.startingBorPair = currentBorrowerPair;
        this.endingBorPair = currentBorrowerPair;
      }
      else
      {
        this.startingBorPair = 0;
        this.endingBorPair = this.loan.GetNumberOfBorrowerPairs() - 1;
      }
      int num = 0;
      for (int startingBorPair1 = this.startingBorPair; startingBorPair1 <= this.endingBorPair; ++startingBorPair1)
      {
        this.loan.SetBorrowerPair(startingBorPair1);
        this._printFips = startingBorPair1 < 1;
        if (!string.IsNullOrEmpty(this.GetField("4000")))
        {
          ++num;
          this._exportVendorFieldsForBorrower = num < 5;
        }
        if (!string.IsNullOrEmpty(this.GetField("4004")))
        {
          ++num;
          this._exportVendorFieldsForCoBorrower = num < 5;
        }
        if (this.GetField("65").Length != 11)
          throw new ArgumentException("Borrower social security number does not have the correct format");
        if (this.GetField("68") != string.Empty && this.GetField("97").Length != 11)
          throw new ArgumentException("Coborrower social security number does not have the correct format");
        this.Build03A();
        this.Build03B();
        this.Build03C();
        this.Build04A();
        this.Build04B();
        this.Build05H(startingBorPair1 == this.startingBorPair);
        this.Build05I(startingBorPair1 == this.startingBorPair);
        this.Build06A();
        this.Build06B();
        this.Build06C();
        this.Build06D();
        this.Build06F();
        this.Build06G();
        this.Build06H();
        this.Build06L();
        if (startingBorPair1 == this.startingBorPair)
        {
          this.Build06S();
          this.Build07A();
          this.Build07B();
        }
        if (startingBorPair1 == this.startingBorPair)
        {
          for (int startingBorPair2 = this.startingBorPair; startingBorPair2 <= this.endingBorPair; ++startingBorPair2)
          {
            this.loan.SetBorrowerPair(startingBorPair2);
            this.Build08A();
          }
          for (int startingBorPair3 = this.startingBorPair; startingBorPair3 <= this.endingBorPair; ++startingBorPair3)
          {
            this.loan.SetBorrowerPair(startingBorPair3);
            this.Build08B();
          }
          this.loan.SetBorrowerPair(startingBorPair1);
        }
        this.Build09A();
        this.Build10A();
        if (startingBorPair1 == this.startingBorPair)
          this.Build10B();
        this.Build10R();
        if (startingBorPair1 == this.startingBorPair)
        {
          this.Build99B();
          this.BuildADS(startingBorPair1 == this.startingBorPair);
          this.BuildBUA();
          this.BuildLNC();
          this.BuildGOA();
        }
        else
        {
          this.BuildADSPartnerReferences(startingBorPair1 == this.startingBorPair);
          this.BuildAdsForHmda();
        }
        this.BuildGOD();
        this.BuildGOE();
      }
      this.BuildLMD();
      this.BuildFooter();
      string empty = string.Empty;
      foreach (StringBuilder stringBuilder in this.fnmData)
        empty += stringBuilder.ToString();
      this.loan.SetBorrowerPair(currentBorrowerPair);
      return empty;
    }

    private void BuildHeader()
    {
      this.element = this.fnmData[0];
      string val1 = DateTime.Now.Date.ToString("yyyyMMdd");
      this.AddValue("EH", 3, false);
      this.AddValue("", 31, false);
      this.AddValue(val1, 11, false);
      this.AddValue("ENV1", 9, false);
      this.AddSingleValue(this.newLine);
      this.AddValue("TH", 3, false);
      this.AddSingleValue("T100099-002");
      this.AddValue("TRAN1", 9, false);
      this.AddSingleValue(this.newLine);
      string str1 = this.GetField("MORNET.X4");
      if (str1 == "0")
        str1 = string.Empty;
      string val2 = str1;
      string str2 = str1 != "" ? "R" : "N";
      this.AddSingleValue("TPI");
      this.AddValue("1.00", 5, false);
      this.AddSingleValue("01");
      this.AddValue(val2, 30, false);
      this.AddSingleValue(str2);
      this.AddSingleValue(this.newLine);
    }

    private void Build000()
    {
      this.element = this.fnmData[1];
      this.element.Append("000");
      this.AddValue("1", 3, false);
      this.AddValue("3.20", 5, false);
      this.AddSingleValue("W");
      this.AddSingleValue(this.newLine);
    }

    private void Build00A()
    {
      this.element = this.fnmData[2];
      this.element.Append("00A");
      this.AddFieldYN("307");
      this.AddFieldYN("35");
      this.AddSingleValue(this.newLine);
    }

    private void Build01A()
    {
      string empty1 = string.Empty;
      string val1 = string.Empty;
      string empty2 = string.Empty;
      string val2 = string.Empty;
      string val3 = string.Empty;
      this.element = this.fnmData[3];
      this.element.Append("01A");
      string str1;
      switch (this.GetField("1172"))
      {
        case "Conventional":
          str1 = "01";
          break;
        case "VA":
          str1 = "02";
          break;
        case "FHA":
          str1 = "03";
          break;
        case "FarmersHomeAdministration":
          str1 = "04";
          break;
        case "Other":
          str1 = "07";
          val1 = this.GetField("1063");
          break;
        default:
          str1 = "  ";
          break;
      }
      string str2;
      switch (this.GetField("608"))
      {
        case "Fixed":
          str2 = "05";
          break;
        case "AdjustableRate":
          str2 = "01";
          val3 = this.GetField("995");
          break;
        case "GraduatedPaymentMortgage":
          str2 = "06";
          break;
        case "GrowingEquityMortgage":
          str2 = "04";
          break;
        case "OtherAmortizationType":
          str2 = "13";
          val2 = this.GetField("994");
          break;
        default:
          str2 = "  ";
          break;
      }
      this.AddSingleValue(str1);
      this.AddValue(val1, 80, false);
      this.AddField("1040", 30, false, false);
      this.AddField("305", 15, false, false);
      this.AddFieldNumeric("1109", 15, 2, true);
      this.AddFieldNumeric("3", 7, 3, true);
      this.AddField("4", 3, false, true);
      this.AddSingleValue(str2);
      this.AddValue(val2, 80, false);
      this.AddValue(val3, 80, false);
      this.AddSingleValue(this.newLine);
    }

    private void Build02A()
    {
      this.element = this.fnmData[4];
      this.element.Append("02A");
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string val1 = this.GetField("16");
      if (this.nValue(val1) > 4.0)
        val1 = "4";
      string upper = this.GetField("17").ToUpper();
      string str = upper.IndexOf("METES") >= 0 || upper.IndexOf("BONDS") >= 0 ? "02" : "F1";
      string val2 = upper + " " + this.GetField("1824").Trim().ToUpper();
      this.AddField("11", 50, false, false);
      this.AddField("12", 35, false, false);
      this.AddField("14", 2, false, false);
      this.AddFieldZip("15", 5);
      this.AddFieldZip("15", 4);
      this.AddValue(val1, 3, true);
      this.AddSingleValue(str);
      this.AddValue(val2, 80, false);
      this.AddField("18", 4, false, true);
      this.AddSingleValue(this.newLine);
    }

    private void Build02B()
    {
      this.element = this.fnmData[5];
      this.element.Append("02B");
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string fieldYn = this.GetFieldYN("CONSTR.REFI");
      this.loanPurpose = this.GetField("19");
      switch (this.loanPurpose)
      {
        case "Purchase":
          this.loanPurpose = "16";
          break;
        case "Cash-Out Refinance":
        case "NoCash-Out Refinance":
          this.loanPurpose = "05";
          break;
        case "ConstructionOnly":
        case "ConstructionToPermanent":
          this.loanPurpose = "04";
          if (fieldYn == "Y")
          {
            this.loanPurpose = "13";
            break;
          }
          break;
        case "Other":
          this.loanPurpose = "15";
          break;
        default:
          this.loanPurpose = "  ";
          break;
      }
      string str1;
      switch (this.GetField("1811"))
      {
        case "Investor":
          str1 = "D";
          break;
        case "PrimaryResidence":
          str1 = "1";
          break;
        case "SecondHome":
          str1 = "2";
          break;
        default:
          str1 = " ";
          break;
      }
      string str2;
      switch (this.GetField("1066"))
      {
        case "FeeSimple":
          str2 = "1";
          break;
        case "Leasehold":
          str2 = "2";
          break;
        default:
          str2 = " ";
          break;
      }
      this.AddSingleValue(this.Space(2));
      this.AddSingleValue(this.loanPurpose);
      this.AddField("9", 80, false, false);
      this.AddSingleValue(str1);
      this.AddField("33", 60, false, false);
      this.AddSingleValue(str2);
      if (str2 == "2")
        this.AddFieldDate("1034", 8);
      else
        this.AddSingleValue(this.Space(8));
      this.AddSingleValue(this.newLine);
    }

    private void Build02C()
    {
      this.element = this.fnmData[6];
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string field1 = this.GetField("31");
      if (field1 != string.Empty)
      {
        this.element.Append("02C");
        this.AddValue(field1, 60, false);
        this.AddSingleValue(this.newLine);
      }
      string field2 = this.GetField("1602");
      if (!(field2 != string.Empty))
        return;
      this.element.Append("02C");
      this.AddValue(field2, 60, false);
      this.AddSingleValue(this.newLine);
    }

    private void Build02D()
    {
      this.element = this.fnmData[7];
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string field1 = string.Empty;
      string field2 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      if (!(this.loanPurpose == "05") && !(this.loanPurpose == "04"))
        return;
      this.element.Append("02D");
      string field3;
      string field4;
      string field5;
      if (this.loanPurpose == "05")
      {
        field3 = "24";
        field4 = "25";
        field5 = "26";
      }
      else
      {
        field3 = "20";
        field4 = "21";
        field1 = "22";
        field2 = "23";
        field5 = "10";
      }
      string str1;
      if (this.GetField("19") == "NoCash-Out Refinance")
      {
        str1 = "F1";
      }
      else
      {
        switch (this.GetField("299"))
        {
          case "CashOutDebtConsolidation":
            str1 = "11";
            break;
          case "CashOutHomeImprovement":
            str1 = "04";
            break;
          case "CashOutLimited":
            str1 = "13";
            break;
          default:
            str1 = "01";
            break;
        }
      }
      string str2;
      if (str1 == "04")
      {
        switch (this.GetField("30"))
        {
          case "Made":
            str2 = "Y";
            break;
          case "ToBeMade":
            str2 = "N";
            break;
          default:
            str2 = "U";
            break;
        }
      }
      else
        str2 = " ";
      this.AddField(field3, 4, false, true);
      this.AddFieldNumeric(field4, 15, 2, true);
      this.AddFieldNumeric(field5, 15, 2, true);
      this.AddFieldNumeric(field1, 15, 2, true);
      this.AddFieldNumeric(field2, 15, 2, true);
      this.AddSingleValue(str1);
      this.AddField("205", 80, false, false);
      this.AddSingleValue(str2);
      this.AddFieldNumeric("29", 15, 2, true);
      this.AddSingleValue(this.newLine);
    }

    private List<Fannie32.DownPayment> getGiftFunds(bool currentBorPairOnly)
    {
      List<Fannie32.DownPayment> giftFunds = new List<Fannie32.DownPayment>();
      for (int index1 = 0; index1 < 2; ++index1)
      {
        double amt = this.nValue(this.GetField(this.amtFlds[index1]));
        string code = string.Empty;
        if (amt != 0.0)
        {
          for (int index2 = 0; index2 < this.codes.Length; ++index2)
          {
            if (this.GetField(this.codesFlg[index1, index2]) == "Y")
            {
              code = this.codes[index2];
              break;
            }
          }
          if (!string.IsNullOrEmpty(code))
            giftFunds.Add(new Fannie32.DownPayment(code, amt, this.GetField(this.dscFlds[index1])));
        }
      }
      return giftFunds;
    }

    private void Build02E(bool currentBorPairOnly)
    {
      this.element = this.fnmData[8];
      List<Fannie32.DownPayment> downPaymentList;
      if (this.GetField("1172") == "FHA")
      {
        downPaymentList = this.getGiftFunds(currentBorPairOnly);
      }
      else
      {
        downPaymentList = new List<Fannie32.DownPayment>();
        string code;
        switch (this.GetField("34"))
        {
          case "BridgeLoan":
            code = "09";
            break;
          case "CashOnHand":
            code = "02";
            break;
          case "CheckingSavings":
            code = "F1";
            break;
          case "DepositOnSalesContract":
            code = "F2";
            break;
          case "EquityOnPendingSale":
            code = "03";
            break;
          case "EquityOnSoldProperty":
            code = "F3";
            break;
          case "EquityOnSubjectProperty":
            code = "F4";
            break;
          case "GiftFunds":
            code = "04";
            break;
          case "LifeInsuranceCashValue":
            code = "F8";
            break;
          case "LotEquity":
            code = "10";
            break;
          case "OtherTypeOfDownPayment":
            code = "13";
            break;
          case "RentWithOptionToPurchase":
            code = "11";
            break;
          case "RetirementFunds":
            code = "F7";
            break;
          case "SaleOfChattel":
            code = "14";
            break;
          case "SecuredBorrowedFunds":
            code = "28";
            break;
          case "StocksAndBonds":
            code = "F5";
            break;
          case "SweatEquity":
            code = "06";
            break;
          case "TradeEquity":
            code = "07";
            break;
          case "TrustFunds":
            code = "F6";
            break;
          case "UnsecuredBorrowedFunds":
            code = "01";
            break;
          case "FHAGiftSourceNA":
            code = "H0";
            break;
          case "FHAGiftSourceRelative":
            code = "H1";
            break;
          case "FHAGiftSourceGovernmentAssistance":
            code = "H3";
            break;
          case "FHAGiftSourceNonprofitSellerFunded":
            code = "H4";
            break;
          case "FHAGiftSourceNonprofitNonSellerFunded":
            code = "H5";
            break;
          case "FHAGiftSourceEmployer":
            code = "H6";
            break;
          default:
            code = "  ";
            break;
        }
        Fannie32.DownPayment downPayment = new Fannie32.DownPayment(code, this.nValue(this.GetField("1335")), this.GetField("191"));
        downPaymentList.Add(downPayment);
      }
      foreach (Fannie32.DownPayment downPayment in downPaymentList)
      {
        if (downPayment.downPayAmt != 0.0 && !(downPayment.downPayCode == string.Empty))
        {
          this.element.Append("02E");
          this.AddSingleValue(downPayment.downPayCode);
          this.AddValueNumeric(downPayment.downPayAmt, 15, 2, true);
          this.AddValue(downPayment.downPayDesc, 80, false);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build03A()
    {
      string empty = string.Empty;
      this.element = this.fnmData[9];
      string str1 = this.MaritalStatus(this.GetField("52"));
      if (!(this.CheckSSN("65") != string.Empty))
        return;
      string str2;
      switch (this.GetField("181"))
      {
        case "Jointly":
          str2 = "Y";
          break;
        case "NotJointly":
          str2 = "N";
          break;
        default:
          str2 = " ";
          break;
      }
      string str3 = this.GetField("4000");
      string val1 = this.GetField("4001");
      string str4 = this.GetField("4002");
      string val2 = this.GetField("4003");
      if (str3 == string.Empty && str4 == string.Empty)
      {
        NameParser nameParser = new NameParser(this.GetField("36"), this.GetField("37"));
        str3 = nameParser.FirstName;
        val1 = nameParser.MiddleInitial;
        str4 = nameParser.LastName;
        val2 = nameParser.Title;
      }
      else
      {
        if (val2 == string.Empty)
        {
          NameParser nameParser = new NameParser();
          nameParser.ParseLastName(str4);
          str4 = nameParser.LastName;
          val2 = nameParser.Title;
        }
        if (val1 == string.Empty)
        {
          NameParser nameParser = new NameParser();
          nameParser.ParseFirstName(str3);
          str3 = nameParser.FirstName;
          val1 = nameParser.MiddleInitial;
        }
      }
      this.element.Append("03ABW");
      this.AddFieldSSN("65", 9);
      this.AddValue(str3, 35, false);
      this.AddValue(val1, 35, false);
      this.AddValue(str4, 35, false);
      this.AddValue(val2, 4, false);
      this.AddFieldPhone("66", 10);
      this.AddField("38", 3, false, true);
      this.AddField("39", 2, false, false);
      this.AddSingleValue(str1);
      this.AddFieldNumeric("53", 2, 0, true);
      this.AddSingleValue(str2);
      this.AddFieldSSN("97", 9);
      this.AddFieldDate("1402", 8);
      this.AddField("1240", 80, false, false);
      this.AddSingleValue(this.newLine);
      if (!(this.CheckSSN("97") != string.Empty))
        return;
      string str5 = this.MaritalStatus(this.GetField("84"));
      string str6 = this.GetField("4004");
      string val3 = this.GetField("4005");
      string str7 = this.GetField("4006");
      string val4 = this.GetField("4007");
      if (str6 == string.Empty && str7 == string.Empty)
      {
        NameParser nameParser = new NameParser(this.GetField("68"), this.GetField("69"));
        str6 = nameParser.FirstName;
        val3 = nameParser.MiddleInitial;
        str7 = nameParser.LastName;
        val4 = nameParser.Title;
      }
      else
      {
        if (val4 == string.Empty)
        {
          NameParser nameParser = new NameParser();
          nameParser.ParseLastName(str7);
          str7 = nameParser.LastName;
          val4 = nameParser.Title;
        }
        if (val3 == string.Empty)
        {
          NameParser nameParser = new NameParser();
          nameParser.ParseFirstName(str6);
          str6 = nameParser.FirstName;
          val3 = nameParser.MiddleInitial;
        }
      }
      this.AddSingleValue("03AQZ");
      this.AddFieldSSN("97", 9);
      this.AddValue(str6, 35, false);
      this.AddValue(val3, 35, false);
      this.AddValue(str7, 35, false);
      this.AddValue(val4, 4, false);
      this.AddFieldPhone("98", 10);
      this.AddField("70", 3, false, true);
      this.AddField("71", 2, false, false);
      this.AddSingleValue(str5);
      this.AddFieldNumeric("85", 2, 0, true);
      this.AddSingleValue(str2);
      this.AddFieldSSN("65", 9);
      this.AddFieldDate("1403", 8);
      this.AddField("1268", 80, false, false);
      this.AddSingleValue(this.newLine);
    }

    private void Build03B()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      this.element = this.fnmData[10];
      for (int index1 = 1; index1 <= 2; ++index1)
      {
        string val1;
        double num1;
        string str1;
        if (index1 == 1)
        {
          val1 = this.CheckSSN("65");
          num1 = this.nValue(this.GetField("53"));
          str1 = this.GetField("54");
        }
        else
        {
          val1 = this.CheckSSN("97");
          num1 = this.nValue(this.GetField("85"));
          str1 = this.GetField("86");
        }
        if (val1 != string.Empty)
        {
          int num2 = 0;
          while (str1 != string.Empty)
          {
            int length = str1.IndexOf(",");
            string str2;
            if (length > -1)
            {
              str2 = str1.Substring(0, length).Trim();
              str1 = str1.Substring(length + 1, str1.Length - (length + 1));
            }
            else
            {
              str2 = str1;
              str1 = string.Empty;
            }
            for (int startIndex = 0; startIndex <= str2.Length - 1; ++startIndex)
            {
              char c = Convert.ToChar(str2.Substring(startIndex, 1));
              if (char.IsNumber(c))
                empty4 += Convert.ToString(c);
            }
            if ((double) num2 < num1)
            {
              ++num2;
              string val2 = empty4;
              empty4 = string.Empty;
              this.AddSingleValue("03B");
              this.AddValue(val1, 9, false);
              this.AddValue(val2, 3, true);
              this.AddSingleValue(this.newLine);
            }
          }
          for (int index2 = num2; (double) index2 < num1; ++index2)
          {
            this.AddSingleValue("03B");
            this.AddValue(val1, 9, false);
            this.AddValue("1", 3, true);
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void Build03C()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      this.element = this.fnmData[11];
      for (int index1 = 1; index1 <= 2; ++index1)
      {
        bool flag;
        string str1;
        if (index1 == 1)
        {
          flag = true;
          str1 = "BR";
        }
        else
        {
          flag = false;
          str1 = "CR";
        }
        string val1 = !flag ? this.CheckSSN("97") : this.CheckSSN("65");
        if (val1 != string.Empty)
        {
          if (this.GetField("FR" + index1.ToString("00") + "04") != string.Empty)
          {
            string str2;
            switch (this.GetField("FR" + index1.ToString("00") + "15"))
            {
              case "LivingRentFree":
              case "NoPrimaryHousingExpense":
                str2 = "X";
                break;
              case "Own":
                str2 = "O";
                break;
              case "Rent":
                str2 = "R";
                break;
              default:
                str2 = " ";
                break;
            }
            string val2 = this.GetField("FR" + index1.ToString("00") + "24");
            if (this.nValue(val2) < 1.0 || this.nValue(val2) > 11.0)
              val2 = this.Space(2);
            this.AddSingleValue("03C");
            this.AddValue(val1, 9, false);
            this.AddSingleValue("ZG");
            this.AddField("FR" + index1.ToString("00") + "04", 50, false, false);
            this.AddField("FR" + index1.ToString("00") + "06", 35, false, false);
            this.AddField("FR" + index1.ToString("00") + "07", 2, false, false);
            this.AddFieldZip("FR" + index1.ToString("00") + "08", 5);
            this.AddFieldZip("FR" + index1.ToString("00") + "08", 4);
            this.AddSingleValue(str2);
            this.AddField("FR" + index1.ToString("00") + "12", 2, true, true);
            this.AddValue(val2, 2, true);
            this.AddSingleValue(this.Space(50));
            this.AddSingleValue(this.newLine);
          }
          int numberOfResidence = this.loan.GetNumberOfResidence(flag);
          for (int index2 = 1; index2 <= numberOfResidence; ++index2)
          {
            string str3 = str1 + index2.ToString("00");
            if (!(this.GetField(str3 + "23") == "Current") && !(this.GetField(str3 + "04") == string.Empty))
            {
              string str4;
              switch (this.GetField(str3 + "15"))
              {
                case "LivingRentFree":
                case "NoPrimaryHousingExpense":
                  str4 = "X";
                  break;
                case "Own":
                  str4 = "O";
                  break;
                case "Rent":
                  str4 = "R";
                  break;
                default:
                  str4 = " ";
                  break;
              }
              string val3 = this.GetField(str3 + "24");
              if (this.nValue(val3) < 1.0 || this.nValue(val3) > 11.0)
                val3 = this.Space(2);
              this.AddSingleValue("03C");
              this.AddValue(val1, 9, false);
              this.AddSingleValue("F4");
              this.AddField(str3 + "04", 50, false, false);
              this.AddField(str3 + "06", 35, false, false);
              this.AddField(str3 + "07", 2, false, false);
              this.AddFieldZip(str3 + "08", 5);
              this.AddFieldZip(str3 + "08", 4);
              this.AddSingleValue(str4);
              this.AddField(str3 + "12", 2, true, true);
              this.AddValue(val3, 2, true);
              this.AddSingleValue(this.Space(50));
              this.AddSingleValue(this.newLine);
            }
          }
          string empty5 = string.Empty;
          string empty6 = string.Empty;
          string empty7 = string.Empty;
          string empty8 = string.Empty;
          string empty9 = string.Empty;
          string id;
          string str5;
          string field1;
          string field2;
          string field3;
          if (index1 == 1)
          {
            id = "1819";
            str5 = "1416";
            field1 = "1417";
            field2 = "1418";
            field3 = "1419";
          }
          else
          {
            id = "1820";
            str5 = "1519";
            field1 = "1520";
            field2 = "1521";
            field3 = "1522";
          }
          if (this.GetField(id) != "Y" && this.GetField(str5) != string.Empty)
          {
            this.AddSingleValue("03C");
            this.AddValue(val1, 9, false);
            this.AddSingleValue("BH");
            this.AddField(str5, 50, false, false);
            this.AddField(field1, 35, false, false);
            this.AddField(field2, 2, false, false);
            this.AddFieldZip(field3, 5);
            this.AddFieldZip(field3, 4);
            this.AddSingleValue(this.Space(55));
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void Build04A()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.element = this.fnmData[12];
      for (int index = 1; index <= 2; ++index)
      {
        string str1 = this.GetField("FE" + index.ToString("00") + "02").Trim();
        if (str1 != string.Empty && str1 != null)
        {
          string val = index != 1 ? this.CheckSSN("97") : this.CheckSSN("65");
          if (val != string.Empty)
          {
            string str2 = "FE" + index.ToString("00");
            this.AddSingleValue("04A");
            this.AddValue(val, 9, false);
            this.AddField(str2 + "02", 35, false, false);
            this.AddField(str2 + "04", 35, false, false);
            this.AddField(str2 + "05", 35, false, false);
            this.AddField(str2 + "06", 2, false, false);
            this.AddFieldZip(str2 + "07", 5);
            this.AddFieldZip(str2 + "07", 4);
            this.AddFieldYN(str2 + "15");
            this.AddFieldNumeric(str2 + "13", 2, 0, false);
            this.AddFieldNumeric(str2 + "33", 2, 0, false);
            this.AddFieldNumeric(str2 + "16", 2, 0, false);
            this.AddField(str2 + "10", 25, false, false);
            this.AddFieldPhone(str2 + "17", 10);
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void Build04B()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.element = this.fnmData[13];
      for (int index1 = 1; index1 <= 2; ++index1)
      {
        bool flag = true;
        string str1 = "BE";
        if (index1 == 2)
        {
          flag = false;
          str1 = "CE";
        }
        int numberOfEmployer = this.loan.GetNumberOfEmployer(flag);
        for (int index2 = 1; index2 <= numberOfEmployer; ++index2)
        {
          string str2 = str1 + index2.ToString("00");
          if (!(this.GetField(str1 + index2.ToString("00") + "02") == string.Empty) && (!(this.GetField(str2 + "02") == this.GetField("FE0102")) || !(this.GetField(str2 + "04") == this.GetField("FE0104")) || !(this.GetField(str2 + "05") == this.GetField("FE0105")) || !(this.GetField(str2 + "06") == this.GetField("FE0106")) || !(this.GetField(str2 + "07") == this.GetField("FE0107"))) && (!(this.GetField(str2 + "02") == this.GetField("FE0202")) || !(this.GetField(str2 + "04") == this.GetField("FE0204")) || !(this.GetField(str2 + "05") == this.GetField("FE0205")) || !(this.GetField(str2 + "06") == this.GetField("FE0206")) || !(this.GetField(str2 + "07") == this.GetField("FE0207"))))
          {
            string val;
            if (index1 == 1)
            {
              if (!(this.GetField(str1 + index2.ToString("00") + "08") != "Borrower"))
                val = this.CheckSSN("65");
              else
                continue;
            }
            else if (!(this.GetField(str1 + index2.ToString("00") + "08") != "CoBorrower"))
              val = this.CheckSSN("97");
            else
              continue;
            if (!(val == string.Empty))
            {
              this.AddSingleValue("04B");
              this.AddValue(val, 9, false);
              this.AddField(str2 + "02", 35, false, false);
              this.AddField(str2 + "04", 35, false, false);
              this.AddField(str2 + "05", 35, false, false);
              this.AddField(str2 + "06", 2, false, false);
              this.AddFieldZip(str2 + "07", 5);
              this.AddFieldZip(str2 + "07", 4);
              this.AddFieldYN(str2 + "15");
              this.AddFieldYN(str2 + "09");
              this.AddFieldDate(str2 + "11", 8);
              this.AddFieldDate(str2 + "14", 8);
              this.AddFieldNumeric(str2 + "12", 15, 2, true);
              this.AddField(str2 + "10", 25, false, false);
              this.AddFieldPhone(str2 + "17", 10);
              this.AddSingleValue(this.newLine);
            }
          }
        }
      }
    }

    private void Build05H(bool primaryPair)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      this.element = this.fnmData[14];
      int num = !primaryPair ? 8 : 15;
      for (int index = 1; index <= num; ++index)
      {
        switch (index.ToString())
        {
          case "1":
            str1 = "119";
            str2 = "25";
            break;
          case "2":
            str1 = "120";
            str2 = "26";
            break;
          case "3":
            str1 = "121";
            str2 = "22";
            break;
          case "4":
            str1 = "122";
            str2 = "01";
            break;
          case "5":
            str1 = "123";
            str2 = "14";
            break;
          case "6":
            str1 = "124";
            str2 = "02";
            break;
          case "7":
            str1 = "125";
            str2 = "06";
            break;
          case "8":
            str1 = "126";
            str2 = "23";
            break;
          case "9":
            str1 = "228";
            str2 = "26";
            break;
          case "10":
            str1 = "229";
            str2 = "22";
            break;
          case "11":
            str1 = "230";
            str2 = "01";
            break;
          case "12":
            str1 = "1405";
            str2 = "14";
            break;
          case "13":
            str1 = "232";
            str2 = "02";
            break;
          case "14":
            str1 = "233";
            str2 = "06";
            break;
          case "15":
            str1 = "234";
            str2 = "23";
            break;
        }
        string field = this.GetField(str1);
        if (field != string.Empty && this.nValue(field) != 0.0)
        {
          string str3 = index > 8 ? "2" : "1";
          this.AddSingleValue("05H");
          this.AddFieldSSN("65", 9);
          this.AddSingleValue(str3);
          this.AddSingleValue(str2);
          this.AddFieldNumeric(str1, 15, 2, true);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build05I(bool primaryPair)
    {
      string id = string.Empty;
      string str = string.Empty;
      this.element = this.fnmData[15];
      for (int index = 1; index <= 16; ++index)
      {
        switch (index)
        {
          case 1:
            id = "101";
            str = "20";
            break;
          case 2:
            id = "102";
            str = "09";
            break;
          case 3:
            id = "103";
            str = "08";
            break;
          case 4:
            id = "104";
            str = "10";
            break;
          case 5:
            id = "105";
            str = "17";
            break;
          case 6:
            id = "106";
            str = "33";
            break;
          case 7:
            id = "107";
            str = "45";
            break;
          case 8:
            id = "462";
            str = "SI";
            break;
          case 9:
            id = "1171";
            str = "SI";
            break;
          case 10:
            id = "110";
            str = "20";
            break;
          case 11:
            id = "111";
            str = "09";
            break;
          case 12:
            id = "112";
            str = "08";
            break;
          case 13:
            id = "113";
            str = "10";
            break;
          case 14:
            id = "114";
            str = "17";
            break;
          case 15:
            id = "115";
            str = "33";
            break;
          case 16:
            id = "116";
            str = "45";
            break;
        }
        if (!(str == "SI") || !(this.GetField("1811") == "SecondHome") && primaryPair)
        {
          string val1 = this.GetField(id);
          if (this.nValue(val1) != 0.0)
          {
            string empty = string.Empty;
            string val2 = index < 1 || index > 9 ? this.CheckSSN("97") : this.CheckSSN("65");
            if (!(val2 == string.Empty))
            {
              switch (id)
              {
                case "462":
                  val1 = "-" + val1;
                  break;
                case "106":
                  string field = this.GetField("1811");
                  double num1 = this.nValue(this.GetField("1171"));
                  double num2 = this.nValue(this.GetField("1005"));
                  if ((field == "PrimaryResidence" || field == "Investor") && num2 > 0.0 && num1 > 0.0)
                  {
                    val1 = Convert.ToString(this.nValue(val1) - num1);
                    break;
                  }
                  break;
              }
              this.AddSingleValue("05I");
              this.AddValue(val2, 9, false);
              this.AddSingleValue(str);
              this.AddValueNumeric(val1, 15, 2, true);
              this.AddSingleValue(this.newLine);
            }
          }
        }
      }
      this.AddOtherIncome();
    }

    private void Build06A()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      this.element = this.fnmData[16];
      string[,] strArray = new string[2, 2];
      for (int index1 = 0; index1 <= 1; ++index1)
      {
        for (int index2 = 0; index2 <= 1; ++index2)
          strArray[index1, index2] = string.Empty;
      }
      string field1 = this.GetField("183");
      string field2 = this.GetField("182");
      string field3 = this.GetField("1716");
      string field4 = this.GetField("1715");
      if (field1 != string.Empty && this.nValue(field1) != 0.0)
      {
        strArray[0, 0] = field2;
        strArray[0, 1] = field1;
      }
      if (field3 != string.Empty && this.nValue(field3) != 0.0)
      {
        strArray[1, 0] = field4;
        strArray[1, 1] = field3;
      }
      for (int index = 0; index <= 1; ++index)
      {
        if (strArray[index, 1] != string.Empty)
        {
          double num = this.nValue(strArray[index, 1]);
          strArray[index, 1] = num.ToString("###########0.00");
          this.AddSingleValue("06A");
          this.AddFieldSSN("65", 9);
          this.AddValue(strArray[index, 0], 35, false);
          this.AddValue(strArray[index, 1], 15, true);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build06B()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.element = this.fnmData[17];
      string field1 = this.GetField("303");
      string field2 = this.GetField("210");
      if (field1 == string.Empty && field2 == string.Empty)
        return;
      this.AddSingleValue("06B");
      this.AddFieldSSN("65", 9);
      this.AddSingleValue(this.Space(30));
      this.AddFieldNumeric("210", 15, 2, true);
      this.AddFieldNumeric("303", 15, 2, true);
      this.AddSingleValue(this.newLine);
    }

    private void Build06C()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      this.element = this.fnmData[18];
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      for (int index1 = 1; index1 <= numberOfDeposits; ++index1)
      {
        for (int index2 = 11; index2 <= 23; index2 += 4)
        {
          string field = this.GetField("DD" + index1.ToString("00") + index2.ToString());
          if (field != string.Empty && this.nValue(field) != 0.0)
          {
            string val = !(this.GetField("DD" + index1.ToString("00") + "24") == "CoBorrower") ? this.CheckSSN("65") : this.CheckSSN("97");
            if (val != string.Empty)
            {
              string str1 = "DD" + index1.ToString("00");
              int num1 = index2 - 3;
              string str2 = (this.TranslateAssetType(str1 + num1.ToString("00")) + " ").Substring(0, 3);
              this.AddSingleValue("06C");
              this.AddValue(val, 9, false);
              this.AddSingleValue(str2);
              this.AddField(str1 + "02", 35, false, false);
              this.AddField(str1 + "04", 35, false, false);
              this.AddField(str1 + "05", 35, false, false);
              this.AddField(str1 + "06", 2, false, false);
              this.AddFieldZip(str1 + "07", 5);
              this.AddFieldZip(str1 + "07", 4);
              int num2 = index2 - 1;
              this.AddField(str1 + num2.ToString(), 30, false, false);
              this.AddFieldNumeric(str1 + index2.ToString(), 15, 2, true);
              this.AddSingleValue(this.Space(90));
              this.AddSingleValue(this.newLine);
            }
          }
        }
      }
      string field1 = string.Empty;
      string empty7 = string.Empty;
      string str3 = string.Empty;
      for (int index = 1; index <= 3; ++index)
      {
        switch (index)
        {
          case 1:
            field1 = "1604";
            str3 = "1605";
            break;
          case 2:
            field1 = "1606";
            str3 = "1607";
            break;
          case 3:
            field1 = "1608";
            str3 = "1609";
            break;
        }
        string field2 = this.GetField(str3);
        if (field2 != string.Empty && field2 != null)
        {
          this.AddSingleValue("06C");
          this.AddFieldSSN("65", 9);
          this.AddSingleValue("05 ");
          this.AddField(field1, 35, false, false);
          this.AddSingleValue(this.Space(111));
          this.AddFieldNumeric(str3, 15, 2, true);
          this.AddSingleValue(this.Space(90));
          this.AddSingleValue(this.newLine);
        }
      }
      string val1 = string.Empty;
      string str4 = string.Empty;
      string field3 = string.Empty;
      string empty8 = string.Empty;
      for (int index = 1; index <= 6; ++index)
      {
        switch (index)
        {
          case 1:
            val1 = "08";
            str4 = "212";
            field3 = string.Empty;
            break;
          case 2:
            val1 = "F8";
            str4 = "213";
            field3 = string.Empty;
            break;
          case 3:
            val1 = "M1";
            str4 = "222";
            field3 = "221";
            break;
          case 4:
            val1 = "M1";
            str4 = "224";
            field3 = "223";
            break;
          case 5:
            val1 = "M1";
            str4 = "1053";
            field3 = "1052";
            break;
          case 6:
            val1 = "M1";
            str4 = "1055";
            field3 = "1054";
            break;
        }
        string field4 = this.GetField(str4);
        if (field4 != string.Empty && this.nValue(field4) != 0.0)
        {
          this.AddSingleValue("06C");
          this.AddFieldSSN("65", 9);
          this.AddValue(val1, 3, false);
          this.AddSingleValue(this.Space(146));
          this.AddFieldNumeric(str4, 15, 2, true);
          this.AddSingleValue(this.Space(7));
          this.AddField(field3, 80, false, false);
          this.AddSingleValue(this.Space(3));
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build06D()
    {
      string fieldID = string.Empty;
      string str = string.Empty;
      string empty = string.Empty;
      this.element = this.fnmData[19];
      for (int index = 1; index <= 3; ++index)
      {
        switch (index)
        {
          case 1:
            fieldID = "214";
            str = "215";
            break;
          case 2:
            fieldID = "216";
            str = "217";
            break;
          case 3:
            fieldID = "1717";
            str = "1718";
            break;
        }
        string field = this.GetField(str);
        if (field != string.Empty && field != null)
        {
          this.AddSingleValue("06D");
          this.AddFieldSSN("65", 9);
          string fieldAutoModel = this.getFieldAutoModel(fieldID);
          string fieldAutoYear = this.getFieldAutoYear(fieldID);
          this.AddValue(fieldAutoModel, 30, false);
          this.AddValue(fieldAutoYear, 4, false);
          this.AddFieldNumeric(str, 15, 2, true);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build06F()
    {
      string str = string.Empty;
      string field1 = string.Empty;
      string val = string.Empty;
      string empty = string.Empty;
      string field2 = string.Empty;
      this.element = this.fnmData[20];
      for (int index = 1; index <= 3; ++index)
      {
        switch (index)
        {
          case 1:
            str = "272";
            field1 = "271";
            val = "DR";
            field2 = "1835";
            break;
          case 2:
            str = "256";
            field1 = "255";
            val = "DZ";
            field2 = "1836";
            break;
          case 3:
            str = "1062";
            field1 = "1058";
            val = "DZ";
            field2 = "1837";
            break;
        }
        string field3 = this.GetField(str);
        if (field3 != string.Empty && this.nValue(field3) != 0.0)
        {
          this.AddSingleValue("06F");
          this.AddFieldSSN("65", 9);
          this.AddValue(val, 3, false);
          this.AddFieldNumeric(str, 15, 2, true);
          this.AddField(field2, 3, true, true);
          this.AddField(field1, 60, false, false);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build06G()
    {
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      this.element = this.fnmData[21];
      for (int index1 = 1; index1 <= numberOfMortgages; ++index1)
      {
        string str1 = this.GetField("FM" + index1.ToString("00") + "04").Trim();
        if (str1 != string.Empty && str1 != null)
        {
          string str2 = "FM" + index1.ToString("00");
          string str3;
          switch (this.GetField(str2 + "24").Trim().ToLower())
          {
            case "sold":
            case "s":
              str3 = "S";
              break;
            case "pendingsale":
            case "p":
            case "ps":
              str3 = "P";
              break;
            case "retainforrental":
            case "r":
              str3 = "R";
              break;
            default:
              str3 = "H";
              break;
          }
          string str4 = this.TranslateReoPropType(str2 + "18");
          string residenceIndicator = this.GetResidenceIndicator(str2 + "41", str1);
          string str5 = this.GetField(str2 + "28");
          if (str5 != "Y")
            str5 = "N";
          string str6 = !(this.GetField(str2 + "24") == "RetainForRental") ? "N" : "Y";
          string str7 = "65";
          string field = this.GetField(str2 + "43");
          int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
          for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
          {
            string str8 = "FL" + index1.ToString("00");
            if (field == this.GetField(str8 + "25"))
            {
              if (this.GetField(str8 + "15") == "CoBorrower")
              {
                str7 = "97";
                break;
              }
              break;
            }
          }
          MortgageInfo mortgageInfo = new MortgageInfo()
          {
            MortgageID = Convert.ToString(this.reoProperties.Count + 1),
            SubjectIndicator = str5,
            RentalIndicator = str6,
            OwnerSSN = this.CheckSSN(str7)
          };
          this.reoProperties.Add((object) field, (object) mortgageInfo);
          string val = this.GetField(str2 + "32");
          if (this.GetField(str2 + "41") == "SecondHome" && residenceIndicator == "Y" && this.nValue(val) < 0.0)
            val = "0";
          this.AddSingleValue("06G");
          this.AddFieldSSN(str7, 9);
          this.AddValue(str1, 35, false);
          this.AddField(str2 + "06", 35, false, false);
          this.AddField(str2 + "07", 2, false, false);
          this.AddFieldZip(str2 + "08", 5);
          this.AddFieldZip(str2 + "08", 4);
          this.AddSingleValue(str3);
          this.AddSingleValue(str4);
          this.AddFieldNumeric(str2 + "19", 15, 2, true);
          this.AddFieldNumeric(str2 + "17", 15, 2, true);
          this.AddFieldNumeric(str2 + "20", 15, 2, true);
          this.AddFieldNumeric(str2 + "16", 15, 2, true);
          this.AddFieldNumeric(str2 + "21", 15, 2, true);
          this.AddValueNumeric(val, 15, 2, true);
          this.AddSingleValue(residenceIndicator);
          this.AddFieldYN(str2 + "28");
          this.AddValue(mortgageInfo.MortgageID, 2, false);
          this.AddSingleValue(this.Space(15));
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build06H()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string val1 = string.Empty;
      string val2 = string.Empty;
      this.element = this.fnmData[22];
      for (int index = 1; index <= 2; ++index)
      {
        string val3;
        string id;
        if (index == 1)
        {
          val3 = this.CheckSSN("65");
          id = "206";
        }
        else
        {
          val3 = this.CheckSSN("97");
          id = "203";
        }
        string field = this.GetField(id);
        if (field != string.Empty && field != null)
        {
          string str = field.Trim();
          if (str.IndexOf(" ") > -1)
          {
            val1 = str.Substring(0, str.IndexOf(" ")).Trim();
            val2 = str.Substring(str.IndexOf(" ") + 1).Trim();
          }
          if (val3 != string.Empty)
          {
            this.AddSingleValue("06H");
            this.AddValue(val3, 9, false);
            this.AddValue(val1, 35, false);
            this.AddSingleValue(this.Space(35));
            this.AddValue(val2, 35, false);
            this.AddSingleValue(this.Space(35));
            this.AddSingleValue(this.Space(30));
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void Build06L()
    {
      string empty = string.Empty;
      this.element = this.fnmData[23];
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str1 = "FL" + index.ToString("00");
        string field = this.GetField(str1 + "02");
        if (field != string.Empty && field != null)
        {
          string val1 = !(this.GetField(str1 + "15") == "CoBorrower") ? this.CheckSSN("65") : this.CheckSSN("97");
          if (val1 != string.Empty)
          {
            string str2;
            switch (this.GetField(str1 + "08"))
            {
              case "Installment":
                str2 = "I ";
                break;
              case "MortgageLoan":
                str2 = "M ";
                break;
              case "Revolving":
                str2 = "R ";
                break;
              case "HELOC":
                str2 = "C ";
                break;
              case "Open30DayChargeAccount":
                str2 = "O ";
                break;
              case "LeasePayments":
                str2 = "F ";
                break;
              case "CollectionsJudgementsAndLiens":
                str2 = "N ";
                break;
              case "Taxes":
                str2 = "A ";
                break;
              default:
                str2 = "Z ";
                break;
            }
            string val2 = string.Empty;
            string val3 = string.Empty;
            string val4 = string.Empty;
            if ((str2.Trim() == "M" || str2.Trim() == "C") && this.reoProperties.ContainsKey((object) this.GetField(str1 + "25")))
            {
              MortgageInfo reoProperty = (MortgageInfo) this.reoProperties[(object) this.GetField(str1 + "25")];
              val4 = reoProperty.MortgageID;
              val2 = reoProperty.SubjectIndicator;
              val3 = reoProperty.RentalIndicator;
              val1 = reoProperty.OwnerSSN;
            }
            this.AddSingleValue("06L");
            this.AddValue(val1, 9, false);
            this.AddSingleValue(str2);
            this.AddField(str1 + "02", 35, false, false);
            this.AddField(str1 + "04", 35, false, false);
            this.AddField(str1 + "05", 35, false, false);
            this.AddField(str1 + "06", 2, false, false);
            this.AddFieldZip(str1 + "07", 5);
            this.AddFieldZip(str1 + "07", 4);
            this.AddField(str1 + "10", 30, false, false);
            this.AddFieldNumeric(str1 + "11", 15, 2, true);
            this.AddFieldNumeric(str1 + "12", 3, 0, true);
            this.AddFieldNumeric(str1 + "13", 15, 2, true);
            this.AddFieldYN(str1 + "18");
            this.AddValue(val4, 2, false);
            this.AddFieldYN(str1 + "26");
            this.AddFieldYN(str1 + "17");
            this.AddValue(val2, 1, false);
            this.AddValue(val3, 1, false);
            this.AddSingleValue(this.newLine);
          }
        }
      }
      string field1 = this.GetField("190");
      if (field1 != "SecondHome" && field1 != "Investor" || this.GetField("418") == "Y" || this.GetField("1343") == "Y")
        return;
      double num1 = 0.0;
      for (int index = 122; index <= 126; ++index)
        num1 += this.nValue(this.GetField(index.ToString()));
      if (num1 <= 0.0)
        return;
      string val = this.CheckSSN("65");
      if (val == string.Empty)
        return;
      double num2 = num1 * 12.0;
      this.AddSingleValue("06L");
      this.AddValue(val, 9, false);
      this.AddSingleValue("Z ");
      this.AddValue("Taxes, Ins, Dues", 35, false);
      this.AddValue(string.Empty, 35, false);
      this.AddValue(string.Empty, 35, false);
      this.AddValue(string.Empty, 2, false);
      this.AddValue(string.Empty, 5, false);
      this.AddValue(string.Empty, 4, false);
      this.AddValue(string.Empty, 30, false);
      this.AddValueNumeric(num1.ToString(), 15, 2, true);
      this.AddValue("12", 3, true);
      this.AddValueNumeric(num2.ToString(), 15, 2, true);
      this.AddValue("N", 1, false);
      this.AddValue("", 2, false);
      this.AddValue("N", 1, false);
      this.AddValue("N", 1, false);
      this.AddValue(string.Empty, 1, false);
      this.AddValue(string.Empty, 1, false);
      this.AddSingleValue(this.newLine);
    }

    private void Build06S()
    {
      string empty = string.Empty;
      this.element = this.fnmData[24];
      string val = this.CheckSSN("65");
      double num = this.nValue(this.GetField("CASASRN.X168")) - this.nValue(this.GetField("CASASRN.X167"));
      if (!(val != string.Empty) || num <= 0.0)
        return;
      this.AddSingleValue("06S");
      this.AddValue(val, 9, false);
      this.AddSingleValue("HMB");
      this.AddValueNumeric(num.ToString(), 15, 2, true);
      this.AddSingleValue(this.newLine);
    }

    private void Build07A()
    {
      this.element = this.fnmData[25];
      this.AddSingleValue("07A");
      this.AddFieldNumeric("136", 15, 2, true);
      this.AddFieldNumeric("967", 15, 2, true);
      this.AddFieldNumeric("968", 15, 2, true);
      this.AddFieldNumeric("1092", 15, 2, true);
      this.AddFieldNumeric("138", 15, 2, true);
      this.AddFieldNumeric("137", 15, 2, true);
      this.AddFieldNumeric("969", 15, 2, true);
      this.AddFieldNumeric("1093", 15, 2, true);
      this.AddFieldNumeric("140", 15, 2, true);
      this.AddFieldNumeric("143", 15, 2, true);
      this.AddFieldNumeric("1045", 15, 2, true);
      this.AddSingleValue(this.newLine);
    }

    private void Build07B()
    {
      string str = string.Empty;
      string descId = string.Empty;
      string empty = string.Empty;
      this.element = this.fnmData[26];
      for (int index = 1; index <= 7; ++index)
      {
        switch (index)
        {
          case 1:
            str = "141";
            descId = "202";
            break;
          case 2:
            str = "1095";
            descId = "1091";
            break;
          case 3:
            str = "1115";
            descId = "1106";
            break;
          case 4:
            str = "1647";
            descId = "1646";
            break;
          case 5:
            str = "1845";
            descId = "";
            break;
          case 6:
            str = "1851";
            descId = "";
            break;
          case 7:
            str = "1852";
            descId = "";
            break;
        }
        string field = this.GetField(str);
        if (field != string.Empty && this.nValue(field) != 0.0)
        {
          this.AddSingleValue("07B");
          this.TranslateOtherCredit(descId);
          this.AddFieldNumeric(str, 15, 2, true);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build08A()
    {
      this.element = this.fnmData[27];
      string val1 = this.CheckSSN("65");
      if (!(val1 != string.Empty) || !(val1 != "0"))
        return;
      string str1;
      switch (this.GetField("981"))
      {
        case "PrimaryResidence":
          str1 = "1";
          break;
        case "SecondaryResidence":
          str1 = "2";
          break;
        case "Investment":
          str1 = "D";
          break;
        default:
          str1 = " ";
          break;
      }
      string str2;
      switch (this.GetField("1069"))
      {
        case "Sole":
          str2 = "01";
          break;
        case "JointWithSpouse":
          str2 = "25";
          break;
        case "JointWithOtherThanSpouse":
          str2 = "26";
          break;
        default:
          str2 = "  ";
          break;
      }
      this.AddSingleValue("08A");
      this.AddValue(val1, 9, false);
      this.AddFieldYNBlank("169");
      this.AddFieldYNBlank("265");
      this.AddFieldYNBlank("170");
      this.AddFieldYNBlank("172");
      this.AddFieldYNBlank("1057");
      this.AddFieldYNBlank("463");
      this.AddFieldYNBlank("173");
      this.AddFieldYNBlank("174");
      this.AddFieldYNBlank("171");
      this.TranslateCitizenAlien("965", "466");
      this.AddFieldYNBlank("418");
      this.AddFieldYNBlank("403");
      this.AddSingleValue(str1);
      this.AddSingleValue(str2);
      this.AddSingleValue(this.newLine);
      string val2 = this.CheckSSN("97");
      if (!(val2 != string.Empty))
        return;
      string str3;
      switch (this.GetField("1015"))
      {
        case "PrimaryResidence":
          str3 = "1";
          break;
        case "SecondaryResidence":
          str3 = "2";
          break;
        case "Investment":
          str3 = "D";
          break;
        default:
          str3 = " ";
          break;
      }
      string str4;
      switch (this.GetField("1070"))
      {
        case "Sole":
          str4 = "01";
          break;
        case "JointWithSpouse":
          str4 = "25";
          break;
        case "JointWithOtherThanSpouse":
          str4 = "26";
          break;
        default:
          str4 = "  ";
          break;
      }
      this.AddSingleValue("08A");
      this.AddValue(val2, 9, false);
      this.AddFieldYNBlank("175");
      this.AddFieldYNBlank("266");
      this.AddFieldYNBlank("176");
      this.AddFieldYNBlank("178");
      this.AddFieldYNBlank("1197");
      this.AddFieldYNBlank("464");
      this.AddFieldYNBlank("179");
      this.AddFieldYNBlank("180");
      this.AddFieldYNBlank("177");
      this.TranslateCitizenAlien("985", "467");
      this.AddFieldYNBlank("1343");
      this.AddFieldYNBlank("1108");
      this.AddSingleValue(str3);
      this.AddSingleValue(str4);
      this.AddSingleValue(this.newLine);
    }

    private void Build08B()
    {
      this.element = this.fnmData[28];
      string[] strArray1 = new string[9]
      {
        "91",
        "92",
        "93",
        "94",
        "95",
        "96",
        "97",
        "98",
        "99"
      };
      Func<string, bool> func = (Func<string, bool>) (typeCode =>
      {
        if (string.Compare(this.GetEncompassVersion(), "18.02", StringComparison.Ordinal) < 0)
          return false;
        return ((IEnumerable<string>) new string[3]
        {
          "92",
          "93",
          "96"
        }).Contains<string>(typeCode);
      });
      string str1 = this.CheckSSN("65");
      if (!(str1 != string.Empty))
        return;
      this.typeCode92Exported = false;
      this.typeCode96Exported = false;
      string[] strArray2 = new string[9]
      {
        "169",
        "265",
        "170",
        "172",
        "1057",
        "463",
        "173",
        "174",
        "171"
      };
      this.WriteAdditional08BLinesForBorrower(str1);
      for (int index = 0; index < 9; ++index)
      {
        if (this.GetFieldYN(strArray2[index]) == "Y" || func(strArray1[index]))
        {
          string val1 = strArray1[index];
          string val2 = val1 == "93" ? this.GetTypeCode93Desc(true) : string.Empty;
          if ((!(val1 == "92") || !this.typeCode92Exported) && (!(val1 == "96") || !this.typeCode96Exported))
          {
            this.AddSingleValue("08B");
            this.AddValue(str1, 9, false);
            this.AddValue(val1, 2, false);
            this.AddValue(val2, (int) byte.MaxValue, false);
            this.AddSingleValue(this.newLine);
          }
        }
      }
      string str2 = this.CheckSSN("97");
      if (!(str2 != string.Empty))
        return;
      this.typeCode92Exported = false;
      this.typeCode96Exported = false;
      string[] strArray3 = new string[9]
      {
        "175",
        "266",
        "176",
        "178",
        "1197",
        "464",
        "179",
        "180",
        "177"
      };
      this.WriteAdditional08BLinesForCoborrower(str2);
      for (int index = 0; index < 9; ++index)
      {
        if (this.GetFieldYN(strArray3[index]) == "Y" || func(strArray1[index]))
        {
          string val3 = strArray1[index];
          string val4 = val3 == "93" ? this.GetTypeCode93Desc(false) : string.Empty;
          if ((!(val3 == "92") || !this.typeCode92Exported) && (!(val3 == "96") || !this.typeCode96Exported))
          {
            this.AddSingleValue("08B");
            this.AddValue(str2, 9, false);
            this.AddValue(val3, 2, false);
            this.AddValue(val4, (int) byte.MaxValue, false);
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void WriteAdditional08BLinesForBorrower(string ssn)
    {
      this.WriteAdditional08BLines(ssn, "MORNET.X151", "MORNET.X152", "MORNET.X154", "MORNET.X83", "MORNET.X84", "265", "463");
    }

    private void WriteAdditional08BLinesForCoborrower(string ssn)
    {
      this.WriteAdditional08BLines(ssn, "MORNET.X155", "MORNET.X156", "MORNET.X153", "MORNET.X89", "MORNET.X90", "266", "464");
    }

    private void WriteAdditional08BLines(string ssn, params string[] fields)
    {
      bool needToCheck08ASegmentDeclaration = string.Compare(this.GetEncompassVersion(), "18.02", StringComparison.Ordinal) < 0;
      string otherDesc = this.GetFieldYN(fields[3]) == "Y" ? this.GetField(fields[4]).ToUpper() : string.Empty;
      Func<string, string, bool> func = (Func<string, string, bool>) ((fieldIdForSegementDeclaration, lookupText) => (!needToCheck08ASegmentDeclaration || this.GetField(fieldIdForSegementDeclaration) == "Y") && otherDesc.Contains(lookupText));
      string desc = string.Empty;
      if (this.GetFieldYN(fields[0]) == "Y" || func(fields[5], "BK INCORRECT"))
        desc = "Confirmed CR BK Incorrect";
      if (this.GetFieldYN(fields[1]) == "Y" || func(fields[5], "BK EC"))
      {
        if (!string.IsNullOrEmpty(desc))
          desc += ", ";
        desc += "Confirmed CR BK EC";
      }
      if (!string.IsNullOrEmpty(desc))
      {
        this.Add08BLine(ssn, "92", desc);
        this.typeCode92Exported = true;
      }
      if (!(this.GetFieldYN(fields[2]) == "Y") && !func(fields[6], "MTG DEL"))
        return;
      this.Add08BLine(ssn, "96", "Confirmed Mtg Del Incorrect");
      this.typeCode96Exported = true;
    }

    private void Add08BLine(string ssn, string typeCode, string desc)
    {
      this.AddSingleValue("08B");
      this.AddValue(ssn, 9, false);
      this.AddValue(typeCode, 2, false);
      this.AddValue(desc, (int) byte.MaxValue, false);
      this.AddSingleValue(this.newLine);
    }

    private string GetTypeCode93Desc(bool isBorr)
    {
      string empty = string.Empty;
      int num = isBorr ? 79 : 85;
      for (int index = 0; index < 6; ++index)
      {
        string str1 = "MORNET.X" + (object) (num + index);
        if (str1 == "MORNET.X83" || str1 == "MORNET.X89")
        {
          if (this.Bam.GetSimpleField(str1) != "Y")
            break;
        }
        else
        {
          string simpleField = this.Bam.GetSimpleField(str1);
          if ((simpleField.Equals("Y") || str1 == "MORNET.X84" || str1 == "MORNET.X90") && !simpleField.ToUpper().Contains("BK EC") && !simpleField.ToUpper().Contains("BK INCORRECT") && !simpleField.ToUpper().Contains("MTG DEL"))
          {
            if (!empty.Equals(string.Empty))
              empty += ",";
            string str2 = string.Empty;
            switch (index)
            {
              case 0:
                str2 = "Confirmed CR FC Incorrect";
                break;
              case 1:
                str2 = "Confirmed CR FC EC";
                break;
              case 2:
                str2 = "Confirmed CR DIL";
                break;
              case 3:
                str2 = "Confirmed CR PFS";
                break;
              case 5:
                str2 = simpleField;
                break;
            }
            empty += str2;
          }
        }
      }
      return empty;
    }

    private void Build09A()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.element = this.fnmData[29];
      for (int index = 1; index <= 2; ++index)
      {
        string val = index != 1 ? this.CheckSSN("97") : this.CheckSSN("65");
        if (val != string.Empty)
        {
          string field = this.GetField("MORNET.X68");
          if (field != string.Empty && field != "0")
          {
            this.AddSingleValue("09A");
            this.AddValue(val, 9, false);
            this.AddFieldDate("MORNET.X68", 8);
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void Build10A()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      this.element = this.fnmData[30];
      for (int index = 1; index <= 2; ++index)
      {
        string val;
        string field;
        string id1;
        string id2;
        if (index == 1)
        {
          val = this.CheckSSN("65");
          field = "188";
          id1 = "1523";
          id2 = "471";
        }
        else
        {
          val = this.CheckSSN("97");
          field = "189";
          id1 = "1531";
          id2 = "478";
        }
        if (val != string.Empty)
        {
          string str1;
          switch (this.GetField(id1))
          {
            case "Hispanic or Latino":
              str1 = "1";
              break;
            case "Not Hispanic or Latino":
              str1 = "2";
              break;
            case "Information not provided":
              str1 = "3";
              break;
            case "Not applicable":
              str1 = "4";
              break;
            default:
              str1 = " ";
              break;
          }
          string str2;
          switch (this.GetField(id2))
          {
            case "Male":
              str2 = "M";
              break;
            case "Female":
              str2 = "F";
              break;
            case "InformationNotProvidedUnknown":
              str2 = "I";
              break;
            case "NotApplicable":
              str2 = "N";
              break;
            default:
              str2 = !(str1 == "3") ? " " : "I";
              break;
          }
          this.AddSingleValue("10A");
          this.AddValue(val, 9, false);
          this.AddFieldYN(field);
          this.AddSingleValue(str1);
          this.AddSingleValue(this.Space(30));
          this.AddSingleValue(str2);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void Build10R()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.element = this.fnmData[32];
      for (int index1 = 1; index1 <= 2; ++index1)
      {
        string val;
        int num1;
        int num2;
        int num3;
        bool flag;
        if (index1 == 1)
        {
          val = this.CheckSSN("65");
          num1 = 1524;
          num2 = 1530;
          num3 = 0;
          flag = false;
        }
        else
        {
          val = this.CheckSSN("97");
          num1 = 1532;
          num2 = 1538;
          num3 = 0;
          flag = false;
        }
        if (val != string.Empty)
        {
          for (int index2 = num1; index2 <= num2; ++index2)
          {
            string field = this.GetField(index2.ToString());
            ++num3;
            if (field != string.Empty && field != "N")
            {
              string str;
              if (field == "Y")
              {
                str = num3.ToString() + " ";
                flag = true;
              }
              else
                str = "  ";
              this.AddSingleValue("10R");
              this.AddValue(val, 9, false);
              this.AddSingleValue(str);
              this.AddSingleValue(this.newLine);
            }
          }
          if (!flag)
          {
            this.AddSingleValue("10R");
            this.AddValue(val, 9, false);
            this.AddSingleValue("6 ");
            this.AddSingleValue(this.newLine);
          }
        }
      }
    }

    private void Build10B()
    {
      string empty = string.Empty;
      this.element = this.fnmData[31];
      string str;
      switch (this.GetField("479"))
      {
        case "FaceToFace":
          str = "F";
          break;
        case "Mail":
          str = "M";
          break;
        case "Telephone":
          str = "T";
          break;
        case "Internet":
          str = "I";
          break;
        default:
          str = " ";
          break;
      }
      this.AddSingleValue("10B");
      this.AddSingleValue(str);
      this.AddField("1612", 60, false, false);
      this.AddFieldDate("MORNET.X69", 8);
      this.AddFieldPhone("1823", 10);
      this.AddField("315", 35, false, false);
      this.AddField("319", 35, false, false);
      this.AddSingleValue(this.Space(35));
      this.AddField("313", 35, false, false);
      this.AddField("321", 2, false, false);
      this.AddFieldZip("323", 5);
      this.AddFieldZip("323", 4);
      this.AddSingleValue(this.newLine);
    }

    private void Build99B()
    {
      string empty = string.Empty;
      this.element = this.fnmData[33];
      this.AddSingleValue("000");
      this.AddValue("70", 3, false);
      this.AddValue("3.20", 5, false);
      this.AddSingleValue(this.newLine);
      string str1;
      switch (this.GetField("991"))
      {
        case "FNM":
          str1 = "01";
          break;
        case "FRE":
          str1 = "02";
          break;
        case "Other":
          str1 = "03";
          break;
        default:
          str1 = "F1";
          break;
      }
      this.AddSingleValue("99B");
      if (this.loanPurpose == "16")
        this.AddFieldYN("MORNET.X12");
      else
        this.AddSingleValue(this.Space(1));
      if (this.loanPurpose == "05")
        this.AddSingleValue(str1);
      else
        this.AddSingleValue(this.Space(2));
      string str2 = "356";
      string str3 = "  ";
      if (this.GetField(str2) != string.Empty)
        str3 = "01";
      else if (this.GetField("1821") != string.Empty)
      {
        str2 = "1821";
        str3 = "02";
      }
      this.AddFieldNumeric(str2, 15, 2, true);
      this.AddFieldNumeric(string.Equals(this.Bam.GetSimpleField("CASASRN.X141"), "Borrower", StringComparison.InvariantCultureIgnoreCase) ? "1269" : "4535", 7, 3, false);
      this.AddSingleValue(str3);
      this.AddSingleValue(this.Space(3));
      this.AddField("618", 60, false, false);
      this.AddField("617", 35, false, false);
      this.AddField("974", 15, false, false);
      this.AddSingleValue(this.Space(2));
      this.AddSingleValue(this.newLine);
    }

    private void BuildADS(bool startingBorrowerPair)
    {
      this.element = this.fnmData[34];
      this.AddSingleValue("ADS");
      this.AddValue("LoanOriginatorID", 35, false);
      this.AddField("3238", 50, false, false);
      this.AddSingleValue(this.newLine);
      this.AddSingleValue("ADS");
      this.AddValue("LoanOriginationCompanyID", 35, false);
      this.AddField("3237", 50, false, false);
      this.AddSingleValue(this.newLine);
      string field1 = this.GetField("3243");
      if (field1 != string.Empty)
      {
        this.AddSingleValue("ADS");
        this.AddValue("SupervisoryAppraiserLicenseNumber", 35, false);
        this.AddValue(field1, 50, false);
        this.AddSingleValue(this.newLine);
      }
      string field2 = this.GetField("ULDD.X31");
      this.AddSingleValue("ADS");
      this.AddValue("AppraisalIdentifier", 35, false);
      this.AddValue(field2, 50, false);
      this.AddSingleValue(this.newLine);
      this.AddTotalMortgagedPropertiesCount();
      this.BuildADSPartnerReferences(startingBorrowerPair);
      this.BuildAdsForHmda();
    }

    private void BuildADSPartnerReferences(bool startingBorrowerPair)
    {
      this.element = this.fnmData[34];
      string field1 = this.GetField("4000");
      string str1 = this.GetField("65").Replace("-", "");
      string field2 = this.GetField("4004");
      string str2 = this.GetField("97").Replace("-", "");
      this.BuildADSForGSE(field1, str1, field2, str2, startingBorrowerPair);
      this.BuildADSForVendors(str1, str2, field1, field2);
      if (!startingBorrowerPair)
        return;
      this.BuildAdsForUserEnteredVendors(str1, str2, field1, field2);
    }

    private void BuildADSForGSE(
      string borFirstName,
      string borSSN,
      string coborFirstName,
      string coborSSN,
      bool startingBorPair = false)
    {
      string field1 = this.GetField("GSEVENDOR.X1");
      string field2 = this.GetField("GSEVENDOR.X2");
      string field3 = this.GetField("GSEVENDOR.X3");
      string field4 = this.GetField("FANNIESERVICE.X3");
      this.PrintAdsVendorValue(borSSN, borFirstName, field4, "Formfree", this._exportVendorFieldsForBorrower);
      string field5 = this.GetField("FANNIESERVICE.X4");
      this.PrintAdsVendorValue(coborSSN, coborFirstName, field5, "Formfree", this._exportVendorFieldsForCoBorrower);
      string val1 = this.GetField("TQL.X95").Trim();
      if (startingBorPair && !string.IsNullOrEmpty(val1))
      {
        this.AddSingleValue("ADS");
        this.AddValue("PropertyDataID", 35, false);
        this.AddValue(val1, 50, false);
        this.AddSingleValue(this.newLine);
      }
      if (this._printFips && this.GetField("MORNET.X27") == "08")
      {
        string str1 = this.GetField("1395").Trim();
        if (str1.Length > 2)
          str1 = str1.Substring(0, 2);
        string str2 = this.GetField("1396").Trim();
        if (str2.Length > 3)
          str2 = str2.Substring(0, 3);
        string str3 = this.GetField("700").Replace(".", "").Trim();
        if (str3.Length > 6)
          str3 = str3.Substring(0, 6);
        string val2 = str1.PadRight(2, ' ') + str2.PadRight(3, ' ') + str3.PadRight(6, ' ');
        this.AddSingleValue("ADS");
        this.AddValue("FIPSCodeIdentifier", 35, false);
        this.AddValue(val2, 50, false);
        this.AddSingleValue(this.newLine);
      }
      this.PrintAdsVendorValue(borSSN, borFirstName, field1, "CoreLogic", this._exportVendorFieldsForBorrower);
      this.PrintAdsVendorValue(coborSSN, coborFirstName, field2, "CoreLogic", this._exportVendorFieldsForCoBorrower);
      this.PrintAdsVendorValue(borSSN, borFirstName, field3, "DataVerify", this._exportVendorFieldsForBorrower);
      this.PrintAdsVendorValue(coborSSN, coborFirstName, field3, "DataVerify", this._exportVendorFieldsForCoBorrower);
    }

    private void BuildADSForVendors(
      string borSSN,
      string coborSSN,
      string borFirstName,
      string coborFirstName)
    {
      try
      {
        List<Tuple<string, List<int>, bool>> tupleList1 = new List<Tuple<string, List<int>, bool>>();
        tupleList1.Add(new Tuple<string, List<int>, bool>("Finicity", new List<int>()
        {
          4,
          5
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Finicity", new List<int>()
        {
          41
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Informative", new List<int>()
        {
          6,
          7
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Blend", new List<int>()
        {
          8,
          9
        }, false));
        List<Tuple<string, List<int>, bool>> tupleList2 = tupleList1;
        Tuple<string, List<int>, bool> tuple1;
        if (string.Compare(this.GetEncompassVersion(), "18.04", StringComparison.Ordinal) < 0)
          tuple1 = new Tuple<string, List<int>, bool>("Avantus", new List<int>()
          {
            10,
            11
          }, false);
        else
          tuple1 = new Tuple<string, List<int>, bool>("Avantus", new List<int>()
          {
            10
          }, false);
        tupleList2.Add(tuple1);
        tupleList1.Add(new Tuple<string, List<int>, bool>("Universal", new List<int>()
        {
          12,
          13
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Chronos", new List<int>()
        {
          14
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("FinLocker", new List<int>()
        {
          15
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("FirstAmerican", new List<int>()
        {
          16
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("IncoCheck", new List<int>()
        {
          17
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("MeridianLink", new List<int>()
        {
          18
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("NCS", new List<int>()
        {
          19
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Plaid", new List<int>()
        {
          20
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("PointServ", new List<int>()
        {
          21
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("SharperLending", new List<int>()
        {
          22
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("VeriTax", new List<int>()
        {
          23
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Yodlee", new List<int>()
        {
          24
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("BankVOD", new List<int>()
        {
          25
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("LendSnap", new List<int>()
        {
          26
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Quovo", new List<int>()
        {
          27
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Roostify", new List<int>()
        {
          28
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("CoreLogic", new List<int>()
        {
          29
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("AdvancedData", new List<int>()
        {
          30
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("CreditInterlink", new List<int>()
        {
          31
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("Chronos", new List<int>()
        {
          32
        }, true));
        tupleList1.Add(new Tuple<string, List<int>, bool>("ComplianceEase", new List<int>()
        {
          33
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("PrivateEyes", new List<int>()
        {
          34
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("BankVOD", new List<int>()
        {
          35
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("QuestSoft", new List<int>()
        {
          36
        }, false));
        tupleList1.Add(new Tuple<string, List<int>, bool>("CreditPlus", new List<int>()
        {
          45
        }, false));
        foreach (Tuple<string, List<int>, bool> tuple2 in tupleList1)
        {
          if (tuple2.Item2.Count > 1)
          {
            if (!tuple2.Item3)
            {
              string field1 = this.GetField("GSEVENDOR.X" + (object) tuple2.Item2[0]);
              string field2 = this.GetField("GSEVENDOR.X" + (object) tuple2.Item2[1]);
              this.PrintAdsVendorValue(borSSN, borFirstName, field1, tuple2.Item1, this._exportVendorFieldsForBorrower);
              this.PrintAdsVendorValue(coborSSN, coborFirstName, field2, tuple2.Item1, this._exportVendorFieldsForCoBorrower);
            }
            else
            {
              List<string> list1 = ((IEnumerable<string>) this.GetField("GSEVENDOR.X" + (object) tuple2.Item2[0]).Split(',')).ToList<string>();
              for (int index = 0; index < list1.Count; ++index)
              {
                string borVendorFieldValue = list1[index];
                this.PrintAdsVendorValue(borSSN, borFirstName, borVendorFieldValue, tuple2.Item1, this._exportVendorFieldsForBorrower);
                if (index == 1)
                  break;
              }
              List<string> list2 = ((IEnumerable<string>) this.GetField("GSEVENDOR.X" + (object) tuple2.Item2[1]).Split(',')).ToList<string>();
              for (int index = 0; index < list2.Count; ++index)
              {
                string borVendorFieldValue = list2[index];
                this.PrintAdsVendorValue(coborSSN, coborFirstName, borVendorFieldValue, tuple2.Item1, this._exportVendorFieldsForCoBorrower);
                if (index == 1)
                  break;
              }
            }
          }
          else if (!tuple2.Item3)
          {
            List<string> list = ((IEnumerable<string>) this.GetField("GSEVENDOR.X" + (object) tuple2.Item2[0]).Split(',')).ToList<string>();
            string borVendorFieldValue1 = list.ElementAtOrDefault<string>(0);
            string borVendorFieldValue2 = list.ElementAtOrDefault<string>(1);
            this.PrintAdsVendorValue(borSSN, borFirstName, borVendorFieldValue1, tuple2.Item1, this._exportVendorFieldsForBorrower);
            this.PrintAdsVendorValue(coborSSN, coborFirstName, borVendorFieldValue2, tuple2.Item1, this._exportVendorFieldsForCoBorrower);
          }
          else
          {
            List<string> list = ((IEnumerable<string>) this.GetField("GSEVENDOR.X" + (object) tuple2.Item2[0]).Split(',')).ToList<string>();
            bool flag = true;
            for (int index = 0; index < list.Count; ++index)
            {
              string borVendorFieldValue = list[index];
              if (!flag)
              {
                this.PrintAdsVendorValue(coborSSN, coborFirstName, borVendorFieldValue, tuple2.Item1, this._exportVendorFieldsForCoBorrower);
                flag = true;
              }
              else
              {
                this.PrintAdsVendorValue(borSSN, borFirstName, borVendorFieldValue, tuple2.Item1, this._exportVendorFieldsForBorrower);
                flag = false;
              }
              if (index == 3)
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void BuildAdsForUserEnteredVendors(
      string borSsn,
      string coborSsn,
      string borFirstName,
      string coborFirstName)
    {
      List<string> list = ((IEnumerable<string>) this.GetField("GSEVENDOR.X37").Split(',')).ToList<string>();
      List<string> source1 = new List<string>();
      foreach (string[] source2 in list.Select<string, string[]>((Func<string, string[]>) (vendor => vendor.Split(':'))))
      {
        string vendorName = string.IsNullOrEmpty(((IEnumerable<string>) source2).ElementAtOrDefault<string>(0)) ? string.Empty : ((IEnumerable<string>) source2).ElementAtOrDefault<string>(0);
        if (!string.IsNullOrEmpty(vendorName))
          source1.Add(vendorName.ToLower());
        if (source1.Count<string>((Func<string, bool>) (s => s.Equals(vendorName.ToLower()))) < 3)
          this.PrintAdsVendorValue(borSsn, borFirstName, ((IEnumerable<string>) source2).ElementAtOrDefault<string>(1), ((IEnumerable<string>) source2).ElementAtOrDefault<string>(0), this._exportVendorFieldsForBorrower);
      }
    }

    private void BuildAdsForHmda()
    {
      try
      {
        this.element = this.fnmData[34];
        string field1 = this.GetField("4000");
        string borSsn = this.GetField("65").Replace("-", "");
        string field2 = this.GetField("4004");
        string coborSsn = this.GetField("97").Replace("-", "");
        this._hmdaFieldsExport.BuildAdsForHmda(borSsn, coborSsn, field1, field2);
      }
      catch (Exception ex)
      {
      }
    }

    private void PrintAdsVendorValue(
      string borSsn,
      string borFirstName,
      string borVendorFieldValue,
      string vendor,
      bool exportVendorValue)
    {
      if (!exportVendorValue || string.IsNullOrEmpty(borFirstName) || string.IsNullOrEmpty(borSsn) || string.IsNullOrEmpty(borVendorFieldValue))
        return;
      string val = borSsn + ":" + borVendorFieldValue;
      if (vendor.Trim().ToLower() == "placeholder" || vendor.Trim().ToLower() == "place holder")
      {
        val = this.Space(10) + borVendorFieldValue;
        vendor = "Placeholder";
      }
      this.AddSingleValue("ADS");
      this.AddValue(vendor, 35, false);
      this.AddValue(val, 50, false);
      this.AddSingleValue(this.newLine);
    }

    private void BuildBUA()
    {
      if (this.GetFieldYN("425") != "Y")
        return;
      this.element = this.fnmData[35];
      this.AddSingleValue("BUA");
      this.AddSingleValue(this.Space(3));
      this.AddSingleValue(this.Space(3));
      this.AddSingleValue(this.Space(7));
      this.AddSingleValue(this.Space(1));
      this.AddSingleValue(this.Space(1));
      this.AddSingleValue("1");
      this.AddSingleValue(this.newLine);
    }

    private void AddTotalMortgagedPropertiesCount()
    {
      int result;
      if (!int.TryParse(this.GetField("ULDD.TotalMortgagedPropertiesCount"), out result) || result <= 0)
        return;
      this.AddSingleValue("ADS");
      this.AddValue("TotalMortgagedPropertiesCount", 35, false);
      this.AddValue(result.ToString(), 50, false);
      this.AddSingleValue(this.newLine);
    }

    private void BuildLNC()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      this.element = this.fnmData[36];
      this.AddSingleValue("000");
      this.AddValue("11", 3, false);
      this.AddValue("3.20", 5, false);
      this.AddSingleValue(this.newLine);
      string str1;
      switch (this.GetField("420"))
      {
        case "FirstLien":
          str1 = "1";
          break;
        case "SecondLien":
          str1 = "2";
          break;
        default:
          str1 = "F";
          break;
      }
      string str2;
      switch (this.GetField("MORNET.X67"))
      {
        case "Alternative":
          str2 = "A";
          break;
        case "FullDocumentation":
          str2 = "F";
          break;
        case "NoDepositVerification":
          str2 = "H";
          break;
        case "NoDepositVerificationEmploymentVerificationOrIncomeVerification":
          str2 = "G";
          break;
        case "NoDocumentation":
          str2 = "C";
          break;
        case "NoEmploymentVerificationOrIncomeVerification":
          str2 = "I";
          break;
        case "Reduced":
          str2 = "R";
          break;
        case "StreamlineRefinance":
          str2 = "B";
          break;
        case "NoRatio":
          str2 = "D";
          break;
        case "NoIncomeNoEmploymentNoAssetsOn1003":
          str2 = "U";
          break;
        case "NoIncomeOn1003":
          str2 = "J";
          break;
        case "NoVerificationOfStatedIncomeEmploymentOrAssets":
          str2 = "K";
          break;
        case "NoVerificationOfStatedIncomeOrAssets":
          str2 = "L";
          break;
        case "NoVerificationOfStatedAssets":
          str2 = "M";
          break;
        case "NoVerificationOfStatedIncomeOrEmployment":
          str2 = "N";
          break;
        case "NoVerificationOfStatedIncome":
          str2 = "O";
          break;
        case "VerbalVerificationOfEmployment":
          str2 = "P";
          break;
        case "OnePaystub":
          str2 = "Q";
          break;
        case "OnePaystubAndVerbalVerificationOfEmployment":
          str2 = "S";
          break;
        case "OnePaystubAndOneW2AndVerbalVerificationOfEmploymentOrOneYear1040":
          str2 = "T";
          break;
        case "(L) No Verification of Stated Income or Assets":
          str2 = "L";
          break;
        default:
          str2 = " ";
          break;
      }
      string str3;
      switch (this.GetField("1041"))
      {
        case "Attached":
          str3 = "02";
          break;
        case "Condominium":
          str3 = "03";
          break;
        case "Cooperative":
          str3 = "05";
          break;
        case "Detached":
          str3 = "01";
          break;
        case "HighRiseCondominium":
          str3 = "07";
          break;
        case "ManufacturedHousing":
          str3 = "08";
          break;
        case "PUD":
          str3 = "04";
          break;
        case "DetachedCondo":
          str3 = "09";
          break;
        case "ManufacturedHomeCondoPUDCoOp":
          str3 = "10";
          break;
        case "MHSelect":
        case "MHAdvantage":
          str3 = "11";
          break;
        default:
          str3 = "  ";
          break;
      }
      string str4;
      switch (this.GetField("1012"))
      {
        case "A_IIICondominium":
          str4 = "01";
          break;
        case "B_IICondominium":
          str4 = "02";
          break;
        case "C_ICondominium":
          str4 = "03";
          break;
        case "OneCooperative":
          str4 = "07";
          break;
        case "TwoCooperative":
          str4 = "08";
          break;
        case "E_PUD":
          str4 = "04";
          break;
        case "F_PUD":
          str4 = "05";
          break;
        case "III_PUD":
          str4 = "06";
          break;
        case "P_LimitedReviewNew":
          str4 = "09";
          break;
        case "Q_LimitedReviewEst":
          str4 = "10";
          break;
        case "R_ExpeditedNew":
          str4 = "11";
          break;
        case "S_ExpeditedEst":
          str4 = "12";
          break;
        case "T_FannieMaeReview":
          str4 = "13";
          break;
        case "U_FHAapproved":
          str4 = "14";
          break;
        case "V_RefiPlus":
          str4 = "15";
          break;
        case "G_NotInAProjectOrDevelopment":
          str4 = "16";
          break;
        case "T_PUD":
          str4 = "17";
          break;
        case "TCooperative":
          str4 = "18";
          break;
        default:
          str4 = "  ";
          break;
      }
      string str5;
      switch (this.GetField("HMDA.X13"))
      {
        case "HOEPA Loan":
          str5 = "Y";
          break;
        case "Not a HOEPA Loan":
          str5 = "N";
          break;
        default:
          str5 = " ";
          break;
      }
      string str6;
      switch (this.GetField("HMDA.X12"))
      {
        case "Preapproval was requested":
          str6 = "Y";
          break;
        case "Preapproval was not requested":
          str6 = "N";
          break;
        default:
          str6 = " ";
          break;
      }
      string val = string.Empty;
      if (this.GetField("608") == "AdjustableRate")
        val = this.GetField("247");
      string str7;
      switch (this.GetField("1552"))
      {
        case "Y":
        case "Yes":
          str7 = " ";
          break;
        case "N":
        case "No":
          str7 = " ";
          break;
        case "Homeowner education completed":
          str7 = "1";
          break;
        case "1x1 counseling completed":
          str7 = "2";
          break;
        case "Both completed":
          str7 = " ";
          break;
        default:
          str7 = " ";
          break;
      }
      this.AddSingleValue("LNC");
      this.AddSingleValue(str1);
      this.AddSingleValue(str2);
      this.AddSingleValue(str3);
      this.AddSingleValue(this.Space(2));
      this.AddSingleValue(this.Space(2));
      this.AddSingleValue(this.Space(2));
      this.AddSingleValue(this.Space(2));
      this.AddSingleValue(str4);
      this.AddSingleValue(this.Space(7));
      this.TranslateBallonYN();
      this.AddSingleValue(this.Space(1));
      this.AddSingleValue(this.Space(1));
      this.AddSingleValue(str7);
      this.AddValueNumeric(val, 7, 3, true);
      this.AddSingleValue(this.Space(22));
      this.AddFieldYN("MORNET.X15");
      this.AddFieldDate("763", 8);
      this.AddFieldDate("682", 8);
      this.AddFieldNumeric("430", 7, 3, true);
      this.AddSingleValue(this.Space(3));
      this.AddFieldNumeric("HMDA.X15", 5, 3, true);
      this.AddSingleValue(str5);
      this.AddSingleValue(str6);
      this.AddSingleValue(this.newLine);
      this.AddSingleValue("PID");
      this.AddField("1401", 30, false, false);
      this.AddField("MORNET.X66", 15, false, false);
      this.AddField("995", 5, false, false);
      this.AddSingleValue(this.newLine);
      string str8 = !(this.GetField("423") == "Biweekly") ? "01" : "02";
      string str9;
      switch (this.GetField("677"))
      {
        case "May_SubjectToConditions":
          str9 = "Y";
          break;
        case "May":
          str9 = "Y";
          break;
        default:
          str9 = "N";
          break;
      }
      this.AddSingleValue("PCH");
      this.AddField("325", 3, false, true);
      this.AddSingleValue(str9);
      this.AddSingleValue(str8);
      this.AddFieldYN("675");
      this.AddSingleValue(this.Space(1));
      this.TranslateRepaymentCode("424");
      this.AddSingleValue(this.newLine);
      if (!(this.GetField("608") == "AdjustableRate"))
        return;
      this.AddSingleValue("ARM");
      this.AddFieldNumeric("688", 7, 3, false);
      this.TranslateIndexType("MORNET.X70");
      this.AddFieldNumeric("689", 7, 3, false);
      this.AddFieldNumeric("1014", 7, 3, false);
      this.AddSingleValue(this.newLine);
    }

    private void BuildGOA()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.element = this.fnmData[37];
      string field = this.GetField("1172");
      if (!(field == "FHA") && !(field == "VA"))
        return;
      this.AddSingleValue("000");
      this.AddValue("20", 3, false);
      this.AddValue("3.20", 5, false);
      this.AddSingleValue(this.newLine);
      this.AddSingleValue("IDA");
      this.AddSingleValue(this.Space(23));
      this.AddSingleValue(this.newLine);
      if (field == "VA")
      {
        this.AddSingleValue("LEA");
        this.AddSingleValue(this.Space(63));
        this.AddSingleValue(this.newLine);
      }
      else
      {
        this.AddSingleValue("LEA");
        this.AddField("1059", 20, false, false);
        this.AddField("1060", 20, false, false);
        this.AddField("3658", 10, false, false);
        this.AddSingleValue(this.Space(23));
        this.AddSingleValue(this.newLine);
      }
      string str1;
      switch (this.GetField("MORNET.X40"))
      {
        case "FullDocumentation":
          str1 = "1";
          break;
        case "InterestRateReductionRefinanceLoan":
          str1 = "4";
          break;
        case "StreamlineWithAppraisal":
          str1 = !string.Equals(field, "FHA") || !string.Equals(this.GetField("19"), "NoCash-Out Refinance") ? "2" : " ";
          break;
        case "StreamlineWithoutAppraisal":
          str1 = "3";
          break;
        case "HOPEForHomeowners":
          str1 = !string.Equals(field, "FHA") || !string.Equals(this.GetField("19"), "NoCash-Out Refinance") ? "H" : " ";
          break;
        case "PriorFHA":
          str1 = "R";
          break;
        default:
          str1 = " ";
          break;
      }
      this.AddSingleValue("GOA");
      this.AddFieldYN("157");
      this.AddFieldNumeric("MORNET.X33", 15, 2, true);
      this.AddFieldNumeric("MORNET.X71", 15, 2, true);
      this.AddSingleValue(this.Space(66));
      this.AddSingleValue(str1);
      this.AddField("13", 35, false, false);
      this.AddSingleValue(this.newLine);
      if (field == "FHA")
      {
        string val;
        switch (this.GetField("1039"))
        {
          case "203B":
            val = "203(b)";
            break;
          case "203B251":
            val = "203(b)/251";
            break;
          case "203B2":
            val = "203(b)2";
            break;
          case "203K":
            val = "203(k)";
            break;
          case "203K251":
            val = "203(k)/251";
            break;
          case "221D2":
            val = "221(d)2";
            break;
          case "221D2251":
            val = "221(d)(2)/251";
            break;
          case "234C":
            val = "234(c)";
            break;
          case "234C251":
            val = "234(c)/251";
            break;
          case "257":
            val = " ";
            break;
          default:
            val = string.Empty;
            break;
        }
        this.AddSingleValue("GOB");
        this.AddValue(val, 13, false);
        this.AddFieldNumeric("29", 15, 2, true);
        this.AddFieldNumeric("1107", 7, 3, true);
        this.AddFieldNumeric(string.Empty, 15, 2, false);
        this.AddSingleValue(this.Space(10));
        this.AddSingleValue(this.newLine);
      }
      if (!(field == "VA"))
        return;
      string str2 = !(this.GetField("100") == "Y") || !(this.CheckSSN("97") != string.Empty) ? "N" : "Y";
      this.AddSingleValue("GOC");
      this.AddSingleValue(str2);
      this.AddFieldNumeric("VASUMM.X3", 15, 2, true);
      this.AddFieldNumeric("1147", 7, 2, true);
      this.AddFieldNumeric("1148", 7, 2, true);
      this.AddFieldNumeric("1107", 7, 3, true);
      this.AddSingleValue(this.newLine);
    }

    private void BuildGOD()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      this.element = this.fnmData[38];
      if (!(this.GetField("1172") == "VA"))
        return;
      for (int index = 1; index <= 2; ++index)
      {
        string val;
        string field1;
        string field2;
        string field3;
        string field4;
        if (index == 1)
        {
          val = this.CheckSSN("65");
          field1 = this.GetField("1156");
          field2 = this.GetField("1158");
          field3 = this.GetField("VALA.X19");
          field4 = this.GetField("1159");
        }
        else
        {
          val = this.CheckSSN("97");
          field1 = this.GetField("1306");
          field2 = this.GetField("1307");
          field3 = this.GetField("1309");
          field4 = this.GetField("1308");
        }
        if (val != string.Empty)
        {
          double num = this.nValue(field1) + this.nValue(field2) + this.nValue(field3) + this.nValue(field4);
          this.AddSingleValue("GOD");
          this.AddValue(val, 9, false);
          this.AddValueNumeric(num.ToString(), 15, 2, true);
          this.AddValueNumeric("0", 15, 2, true);
          this.AddValueNumeric("0", 15, 2, true);
          this.AddValueNumeric("0", 15, 2, true);
          this.AddSingleValue(this.Space(60));
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void BuildGOE()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      this.element = this.fnmData[39];
      string field1 = this.GetField("1172");
      if (!(field1 == "VA") && !(field1 == "FHA"))
        return;
      for (int index = 1; index <= 2; ++index)
      {
        string val;
        string field2;
        string field3;
        if (index == 1)
        {
          val = this.CheckSSN("65");
          field2 = "1018";
          field3 = this.GetField("3062");
        }
        else
        {
          val = this.CheckSSN("97");
          field2 = "1144";
          field3 = this.GetField("3062");
        }
        if (val != string.Empty)
        {
          string str;
          switch (field3)
          {
            case "Not Counseled":
              str = "A";
              break;
            case "HUD Approved Counseling Agency":
              str = "D";
              break;
            default:
              str = " ";
              break;
          }
          this.AddSingleValue("GOE");
          this.AddValue(val, 9, false);
          this.AddField(field2, 10, false, false);
          this.AddSingleValue(this.Space(9));
          this.AddSingleValue(str);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void BuildLMD()
    {
      if (this.GetFieldYN("MORNET.X72") != "Y")
        return;
      this.element = this.fnmData[40];
      this.AddSingleValue("000");
      this.AddValue("30", 3, false);
      this.AddValue("3.20", 5, false);
      this.AddSingleValue(this.newLine);
      string str = this.GetField("MORNET.X27").Trim();
      if (str != "04" && str != "06" && str != "07" && str != "08")
        str = "  ";
      this.AddSingleValue("LMD");
      this.AddField("MORNET.X26", 40, false, false);
      this.AddSingleValue(str);
      if (this.GetField("MORNET.X29").Trim() != string.Empty)
        this.AddField("MORNET.X73", 38, false, false);
      else
        this.AddSingleValue(this.Space(38));
      this.AddFieldYN("MORNET.X28");
      this.AddFieldYN("MORNET.X29");
      this.AddFieldNumeric("MORNET.X30", 15, 2, true);
      this.AddFieldNumeric("MORNET.X31", 15, 2, true);
      this.AddFieldNumeric("MORNET.X32", 15, 2, true);
      this.AddSingleValue(this.newLine);
    }

    private void BuildFooter()
    {
      this.element = this.fnmData[41];
      this.AddSingleValue("TT ");
      this.AddValue("TRAN1", 9, false);
      this.AddSingleValue(this.newLine + "ET ");
      this.AddValue("ENV1", 9, false);
    }

    internal void AddSingleValue(string str) => this.element.Append(str);

    private void AddField(string field, int maxLen, bool zero, bool right)
    {
      string val = this.GetField(field).Trim();
      int length = val.IndexOf(Environment.NewLine);
      if (length >= 0 && length < maxLen)
        val = length != 0 ? val.Substring(0, length).Trim() : string.Empty;
      if (val == string.Empty)
        val = zero ? "0" : string.Empty;
      this.AddValue(val, maxLen, right);
    }

    private void AddFieldNumeric(string field, int maxLen, int decimals, bool numeric)
    {
      this.AddValueNumeric(this.GetField(field), maxLen, decimals, numeric);
    }

    private void AddValueNumeric(string val, int maxLen, int decimals, bool numeric)
    {
      this.AddValueNumeric(this.nValue(val), maxLen, decimals, numeric);
    }

    private void AddValueNumeric(double doubVal, int maxLen, int decimals, bool numeric)
    {
      this.AddValue(numeric || doubVal != 0.0 ? (decimals != 0 ? doubVal.ToString("#0.".PadRight(3 + decimals, '0')) : doubVal.ToString("#0")) : "", maxLen, true);
    }

    private string RemoveLastNewline()
    {
      string str = this.element.ToString();
      if (str.EndsWith(this.newLine))
        str = str.Substring(0, str.LastIndexOf(this.newLine));
      return str;
    }

    private string Space(int num)
    {
      string empty = string.Empty;
      for (int index = 1; index <= num; ++index)
        empty += " ";
      return empty;
    }

    private string CheckSSN(string fieldId)
    {
      string str = this.GetField(fieldId);
      if (str != string.Empty)
        str = str.Replace("-", "");
      return str;
    }

    private string MaritalStatus(string marStatus)
    {
      switch (marStatus)
      {
        case "Married":
          marStatus = "M";
          break;
        case "Separated":
          marStatus = "S";
          break;
        case "Unmarried":
          marStatus = "U";
          break;
        default:
          marStatus = " ";
          break;
      }
      return marStatus;
    }

    private void AddFieldDate(string field, int maxLen)
    {
      string field1 = this.GetField(field);
      string val;
      if (this.IsDate(field1))
      {
        DateTime dateTime = Convert.ToDateTime(field1);
        switch (field)
        {
          case "1034":
            val = dateTime.ToString("20yyMMdd");
            break;
          default:
            val = dateTime.ToString("yyyyMMdd");
            break;
        }
      }
      else
        val = this.Space(8);
      this.AddValue(val, maxLen, false);
    }

    private bool IsDate(string date)
    {
      bool flag = true;
      try
      {
        DateTime.Parse(date);
      }
      catch (FormatException ex)
      {
        flag = false;
      }
      return flag;
    }

    internal void AddValue(string val, int maxLen, bool right)
    {
      val = val.Length >= maxLen ? val.Substring(0, maxLen) : (right ? val.PadLeft(maxLen) : val.PadRight(maxLen));
      this.element.Append(val);
    }

    private double nValue(string val)
    {
      string s = val.Trim();
      if (s == string.Empty)
        return 0.0;
      double num = 0.0;
      try
      {
        num = double.Parse(s);
      }
      catch
      {
      }
      return num;
    }

    private void AddFieldYN(string field)
    {
      string field1 = this.GetField(field);
      this.element.Append(field1 == "Y" || field1 == "Yes" ? "Y" : "N");
    }

    private void AddFieldYNBlank(string field)
    {
      string str;
      switch (this.GetField(field))
      {
        case "Y":
        case "Yes":
          str = "Y";
          break;
        case "N":
        case "No":
          str = "N";
          break;
        default:
          str = " ";
          break;
      }
      this.element.Append(str);
    }

    private void AddFieldSSN(string field, int maxLen)
    {
      string val = this.GetField(field);
      if (val != null && val != string.Empty)
        val = val.Replace("-", "");
      this.AddValue(val, maxLen, true);
    }

    private void AddOtherIncome()
    {
      string[,] strArray = new string[3, 3];
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      bool flag = false;
      for (int index1 = 0; index1 <= 2; ++index1)
      {
        for (int index2 = 0; index2 <= 2; ++index2)
          strArray[index1, index2] = string.Empty;
      }
      string field1 = this.GetField("144");
      string field2 = this.GetField("145");
      string field3 = this.GetField("146");
      string field4 = this.GetField("147");
      string field5 = this.GetField("148");
      string field6 = this.GetField("149");
      string field7 = this.GetField("150");
      string field8 = this.GetField("151");
      string field9 = this.GetField("152");
      if (!(field3 != string.Empty) || field3 == null)
        return;
      string str1 = !(field1.ToUpper() == "B") ? this.CheckSSN("97") : this.CheckSSN("65");
      if (str1 != string.Empty)
      {
        strArray[0, 0] = str1;
        strArray[0, 1] = this.TranslateOtherIncome(field2);
        strArray[0, 2] = field3;
      }
      if (field6 != string.Empty && field6 != null)
      {
        string str2 = !(field4.ToUpper() == "B") ? this.CheckSSN("97") : this.CheckSSN("65");
        if (str2 != string.Empty)
        {
          if (str2 == strArray[0, 0] && this.TranslateOtherIncome(field5) == strArray[0, 1])
          {
            double num = this.nValue(strArray[0, 2]) + this.nValue(field6);
            strArray[0, 2] = num.ToString();
          }
          else
          {
            strArray[1, 0] = str2;
            strArray[1, 1] = this.TranslateOtherIncome(field5);
            strArray[1, 2] = field6;
          }
        }
      }
      if (field9 != string.Empty && field9 != null)
      {
        string str3 = !(field7.ToUpper() == "B") ? this.CheckSSN("97") : this.CheckSSN("65");
        if (str3 != string.Empty)
        {
          if (str3 == strArray[0, 0] && this.TranslateOtherIncome(field8) == strArray[0, 1])
          {
            double num = this.nValue(strArray[0, 2]) + this.nValue(field9);
            strArray[0, 2] = num.ToString();
            flag = true;
          }
          if (str3 == strArray[1, 0] && this.TranslateOtherIncome(field8) == strArray[1, 1])
          {
            double num = this.nValue(strArray[1, 2]) + this.nValue(field9);
            strArray[1, 2] = num.ToString();
            flag = true;
          }
          if (!flag)
          {
            if (strArray[1, 0] != string.Empty && strArray[1, 1] != string.Empty && strArray[1, 2] != string.Empty)
            {
              strArray[2, 0] = str3;
              strArray[2, 1] = this.TranslateOtherIncome(field8);
              strArray[2, 2] = field9;
            }
            else
            {
              strArray[1, 0] = str3;
              strArray[1, 1] = this.TranslateOtherIncome(field8);
              strArray[1, 2] = field9;
            }
          }
        }
      }
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      for (int index = 0; index <= 2; ++index)
      {
        if (strArray[index, 2] != string.Empty)
        {
          string val1 = strArray[index, 0];
          string str4 = strArray[index, 1];
          string val2 = this.nValue(strArray[index, 2]).ToString("#######0.00");
          this.AddSingleValue("05I");
          this.AddValue(val1, 9, false);
          this.AddSingleValue(str4);
          this.AddValue(val2, 15, true);
          this.AddSingleValue(this.newLine);
        }
      }
    }

    private void TranslateBallonYN()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string field1 = this.GetField("4");
      string field2 = this.GetField("325");
      this.element.Append(!(this.GetField("1659") == "Y") ? (this.nValue(field2) <= 0.0 || this.nValue(field2) >= this.nValue(field1) ? "N" : "Y") : "Y");
    }

    private void TranslateOtherCredit(string descId)
    {
      string str;
      switch (this.loan.GetField(descId))
      {
        case "RelocationFunds":
          str = "04";
          break;
        case "EmployerAssistedHousing":
          str = "05";
          break;
        case "LeasePurchaseFund":
          str = "06";
          break;
        case "Other":
          str = "07";
          break;
        case "CashDepositOnSalesContract":
          str = "01";
          break;
        case "SellerCredit":
          str = "02";
          break;
        case "LenderCredit":
          str = "03";
          break;
        case "BorrowerPaidFees":
          str = "08";
          break;
        case "SweatEquity":
          str = "09";
          break;
        default:
          str = "  ";
          break;
      }
      this.element.Append(str);
    }

    private void TranslateCitizenAlien(string citizen, string permanent)
    {
      string str;
      if (this.GetField(citizen).ToUpper() == "Y")
      {
        str = "01";
      }
      else
      {
        switch (this.GetField(permanent).ToUpper())
        {
          case "Y":
            str = "03";
            break;
          case "N":
            str = "05";
            break;
          default:
            str = "  ";
            break;
        }
      }
      this.element.Append(str);
    }

    private string TranslateOtherIncome(string incDesc)
    {
      string empty = string.Empty;
      string str;
      switch (incDesc)
      {
        case "MilitaryBasePay":
          str = "F1";
          break;
        case "MilitaryRationsAllowance":
          str = "07";
          break;
        case "MilitaryFlightPay":
          str = "F2";
          break;
        case "MilitaryHazardPay":
          str = "F3";
          break;
        case "MilitaryClothesAllowance":
          str = "02";
          break;
        case "MilitaryQuartersAllowance":
          str = "04";
          break;
        case "MilitaryPropPay":
          str = "03";
          break;
        case "MilitaryOverseasPay":
          str = "F4";
          break;
        case "MilitaryCombatPay":
          str = "F5";
          break;
        case "MilitaryVariableHousingAllowance":
          str = "F6";
          break;
        case "AlimonyChildSupport":
          str = "F7";
          break;
        case "NotesReceivableInstallment":
          str = "F8";
          break;
        case "Pension":
          str = "41";
          break;
        case "SocialSecurity":
          str = "42";
          break;
        case "Trust":
          str = "F9";
          break;
        case "Unemployment":
          str = "M1";
          break;
        case "AutomobileExpenseAccount":
          str = "M2";
          break;
        case "FosterCare":
          str = "M3";
          break;
        case "VABenefitsNonEducational":
          str = "M4";
          break;
        case "MortgageDifferential":
          str = "30";
          break;
        case "FNMBoarderIncome":
          str = "BI";
          break;
        case "FNMGovernmentMortgageCreditCertificate":
          str = "MC";
          break;
        case "FNMTrailingCoBorrower":
          str = "TC";
          break;
        case "CapitalGains":
          str = "CG";
          break;
        case "EmploymentRelatedAssets":
          str = "EA";
          break;
        case "ForeignIncome":
          str = "FI";
          break;
        case "RoyaltyPayment":
          str = "RP";
          break;
        case "SeasonalIncome":
          str = "SE";
          break;
        case "TemporaryLeave":
          str = "TL";
          break;
        case "TipIncome":
          str = "TI";
          break;
        case "Section8":
          str = "S8";
          break;
        case "AccessoryUnitIncome":
          str = "AU";
          break;
        case "Non-borrowerHouseholdIncome":
          str = "NB";
          break;
        default:
          str = "45";
          break;
      }
      return str;
    }

    private string TranslateAssetType(string fieldId)
    {
      string empty = string.Empty;
      string str;
      switch (this.GetField(fieldId).Replace(" ", ""))
      {
        case "BridgeLoanNotDeposited":
          str = "F7";
          break;
        case "CashOnHand":
          str = "COH";
          break;
        case "CashDepositOnSalesContract":
          str = "F1";
          break;
        case "CertificateOfDeposit":
        case "CertificateOfDepositTimeDeposit":
          str = "01";
          break;
        case "CheckingAccount":
          str = "03";
          break;
        case "GiftOfEquity":
          str = "GE";
          break;
        case "GiftsTotal":
        case "GiftsNotDeposited":
          str = "F2";
          break;
        case "MoneyMarketFund":
          str = "F3";
          break;
        case "MutualFund":
        case "MutualFunds":
          str = "F4";
          break;
        case "NetEquity":
          str = "NE";
          break;
        case "NetWorthOfBusinessOwned":
          str = "F8";
          break;
        case "OtherLiquidAssets":
          str = "OL";
          break;
        case "NetProceedsFromRealEstateAssets":
        case "PendingNetSaleProceedsFromRealEstateAssets":
          str = "NE";
          break;
        case "RetirementFunds":
        case "RetirementFund":
          str = "08";
          break;
        case "SavingsAccount":
          str = "SG";
          break;
        case "SecuredBorrowedFundsNotDeposited":
        case "SecuredBorrowedFundNotDeposited":
          str = "F5";
          break;
        case "TrustAccount":
          str = "11";
          break;
        default:
          str = "M1";
          break;
      }
      return str;
    }

    private void TranslateRepaymentCode(string fieldId)
    {
      string empty = string.Empty;
      string str;
      switch (this.GetField(fieldId))
      {
        case "InterestOnly":
          str = "F2";
          break;
        case "NoNegativeAmortization":
          str = "N ";
          break;
        case "PotentialNegativeAmortization":
          str = "P ";
          break;
        case "ScheduledAmortization":
          str = "F1";
          break;
        case "ScheduledNegativeAmortization":
          str = "S ";
          break;
        default:
          str = "  ";
          break;
      }
      this.element.Append(str);
    }

    private void TranslateIndexType(string fieldId)
    {
      string empty = string.Empty;
      string str;
      switch (this.GetField(fieldId))
      {
        case "EleventhDistrictCostOfFunds":
          str = "9 ";
          break;
        case "DailyCertificateOfDepositRate":
          str = "5 ";
          break;
        case "FNM60DayRequiredNetYield":
          str = "16";
          break;
        case "FNM_LIBOR":
          str = "12";
          break;
        case "FederalCostOfFunds":
          str = "15";
          break;
        case "FRE60DayRequiredNetYield":
          str = "17";
          break;
        case "FRE_LIBOR":
          str = "13";
          break;
        case "MonthlyAverageConstantMaturingTreasury":
          str = "1 ";
          break;
        case "NationalAverageContractRateFHLBB":
          str = "14";
          break;
        case "NationalMonthlyMedianCostOfFunds":
          str = "10";
          break;
        case "TreasuryBillDailyValue":
          str = "8 ";
          break;
        case "WallStreetJournalLIBOR":
          str = "11";
          break;
        case "WeeklyAverageCertificateOfDepositRate":
          str = "6 ";
          break;
        case "WeeklyAverageConstantMaturingTreasury":
          str = "0 ";
          break;
        case "WeeklyAveragePrimeRate":
          str = "7 ";
          break;
        case "WeeklyAverageSecondaryMarketTreasuryBillInvestmentYield":
          str = "4 ";
          break;
        case "WeeklyAverageTreasuryAuctionAverageBondDiscountYield":
          str = "3 ";
          break;
        case "WeeklyAverageTreasuryAuctionAverageInvestmentYield":
          str = "2 ";
          break;
        default:
          str = "  ";
          break;
      }
      this.element.Append(str);
    }

    private string TranslateReoPropType(string fieldId)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string str;
      switch (this.GetField(fieldId))
      {
        case "SingleFamily":
          str = "14";
          break;
        case "Condominium":
          str = "04";
          break;
        case "Townhouse":
          str = "16";
          break;
        case "Cooperative":
          str = "13";
          break;
        case "TwoToFourUnitProperty":
          str = "15";
          break;
        case "MultifamilyMoreThanFourUnits":
          str = "18";
          break;
        case "ManufacturedMobileHome":
          str = "08";
          break;
        case "CommercialNonResidential":
          str = "02";
          break;
        case "MixedUseResidential":
          str = "F1";
          break;
        case "Farm":
          str = "05";
          break;
        case "HomeAndBusinessCombined":
          str = "03";
          break;
        case "Land":
          str = "07";
          break;
        default:
          str = "  ";
          break;
      }
      return str;
    }

    private string GetResidenceIndicator(string fieldId, string propAddress)
    {
      string empty = string.Empty;
      return !(this.GetField(fieldId) == "PrimaryResidence") ? (this.GetField("FR0104") == propAddress ? "Y" : "N") : "Y";
    }

    private void AddFieldPhone(string field, int maxLen)
    {
      string empty = string.Empty;
      string field1 = this.GetField(field);
      this.AddValue(!field1.StartsWith("(") ? (field1.Length < 12 ? this.Space(maxLen) : field1.Substring(0, 3) + field1.Substring(4, 3) + field1.Substring(8, 4)) : (field1.Length < 14 ? this.Space(maxLen) : field1.Substring(1, 3) + field1.Substring(6, 3) + field1.Substring(10, 4)), maxLen, true);
    }

    private void AddFieldZip(string field, int maxLen)
    {
      string empty = string.Empty;
      string str = this.GetField(field);
      if (str.Length < maxLen)
        str = string.Empty;
      int num = str.IndexOf("-");
      this.AddValue(maxLen != 5 || !(str != string.Empty) ? (maxLen != 4 || num <= -1 ? string.Empty : (str.Length >= 10 ? str.Substring(num + 1, maxLen) : string.Empty)) : str.Substring(0, maxLen), maxLen, true);
    }

    private void AddFieldAutoYear(string fieldId, int maxLen)
    {
      string empty = string.Empty;
      string val = string.Empty;
      string field = this.GetField(fieldId);
      if (!(field != string.Empty))
        return;
      for (int startIndex = 0; startIndex <= field.Length - 1; ++startIndex)
      {
        char c = Convert.ToChar(field.Substring(startIndex, 1));
        if (char.IsNumber(c))
          val += Convert.ToString(c);
      }
      if (val.Length < 2)
        val = string.Empty;
      else if (val.Length == 2)
      {
        if (Convert.ToInt32(val) < 50)
          val = "20" + val;
        else if (Convert.ToInt32(val) > 50)
          val = "19" + val;
      }
      this.AddValue(val, maxLen, true);
    }

    private void AddFieldAutoModel(string fieldId, int maxLen)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string field = this.GetField(fieldId);
      if (!(field != string.Empty))
        return;
      for (int startIndex = 0; startIndex <= field.Length - 1; ++startIndex)
      {
        char c = Convert.ToChar(field.Substring(startIndex, 1));
        if (char.IsLetter(c) || char.IsWhiteSpace(c))
          empty2 += Convert.ToString(c);
      }
      this.AddValue(empty2.Trim(), maxLen, false);
    }

    internal string GetField(string id)
    {
      string simpleField = this.loan.GetSimpleField(id);
      string empty;
      return simpleField == "" || simpleField == null || simpleField == "0" ? (empty = string.Empty) : simpleField;
    }

    private string GetFieldYN(string id)
    {
      string field = this.GetField(id);
      return field == "Y" || field == "Yes" ? "Y" : "N";
    }

    private string GetFieldYNBlank(string id)
    {
      string fieldYnBlank;
      switch (this.GetField(id))
      {
        case "Y":
        case "Yes":
          fieldYnBlank = "Y";
          break;
        case "N":
        case "No":
          fieldYnBlank = "N";
          break;
        default:
          fieldYnBlank = " ";
          break;
      }
      return fieldYnBlank;
    }

    private string getFieldAutoModel(string fieldID)
    {
      string input = this.GetField(fieldID).ToString();
      if (Regex.IsMatch(input, "\\A[0-9]{4}\\s"))
        input = input.Substring(4);
      else if (Regex.IsMatch(input, "\\A[0-9]{2}\\s"))
        input = input.Substring(2);
      return input.Trim();
    }

    private string getFieldAutoYear(string fieldID)
    {
      string input = this.GetField(fieldID).ToString();
      string fieldAutoYear = string.Empty;
      if (Regex.IsMatch(input, "\\A[0-9]{4}\\s"))
        fieldAutoYear = input.Substring(0, 4);
      else if (Regex.IsMatch(input, "\\A[0-9]{2}\\s"))
      {
        string val = input.Substring(0, 2);
        fieldAutoYear = this.intValue(val) >= 20 ? "19" + val : "20" + val;
      }
      return fieldAutoYear;
    }

    private int intValue(string val)
    {
      if (val == string.Empty)
        return 0;
      try
      {
        return int.Parse(val.Replace(",", string.Empty).Replace("]", string.Empty));
      }
      catch (Exception ex)
      {
        return 0;
      }
    }

    private string GetEncompassVersion()
    {
      MethodInfo method = this.Bam.GetType().GetMethod("GetVersion");
      return !(method == (MethodInfo) null) ? method.Invoke((object) this.Bam, (object[]) null).ToString() : (string) null;
    }

    private class DownPayment
    {
      public string downPayCode = string.Empty;
      public double downPayAmt;
      public string downPayDesc = string.Empty;

      public DownPayment(string code, double amt, string desc)
      {
        this.downPayAmt = amt;
        this.downPayCode = code;
        if (!string.IsNullOrEmpty(desc) && desc.Length > 80)
          desc = desc.Substring(0, 80);
        this.downPayDesc = desc;
      }
    }
  }
}
