// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.UCDXmlExporterBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public abstract class UCDXmlExporterBase
  {
    protected const string xmlData = "<mismo:MESSAGE MISMOReferenceModelIdentifier=\"3.3.0299\" xmlns:mismo=\"http://www.mismo.org/residential/2009/schemas\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.mismo.org/residential/2009/schemas MISMO_3.3.0_B299.xsd\"></mismo:MESSAGE>�";
    protected XmlDocument xmlDoc = new XmlDocument();
    protected XmlNamespaceManager namespaces;
    protected bool forLoanEstimate;
    protected bool setTotalFields = true;
    protected string nodeBase = string.Empty;
    protected int feeCount;
    protected int prepaidItemCount;
    protected int escrowItemCount;
    protected const string mismoprefix = "mismo�";
    protected int secALines;
    protected int secBLines;
    protected int secCLines;
    protected int secELines;
    protected int secFLines;
    protected int secGLines;
    protected int secHLines;
    protected Decimal borTotalOfPAC;
    protected Decimal borTotalOfPOC;
    protected Decimal borTotalOfsectionA;
    protected Decimal borTotalOfsectionB;
    protected Decimal borTotalOfsectionC;
    protected Decimal borTotalOfPACInSecI;
    protected Decimal borTotalOfPOCInSecI;
    protected Decimal borTotalOfsectionE;
    protected Decimal borTotalOfsectionF;
    protected Decimal borTotalOfsectionG;
    protected Decimal borTotalOfsectionH;
    protected Decimal selTotalOfPAC;
    protected Decimal selTotalOfPOC;
    protected Decimal othTotal;
    protected Decimal borLETotalOfsectionA;
    protected Decimal borLETotalOfsectionB;
    protected Decimal borLETotalOfsectionC;
    protected Decimal borLETotalOfsectionE;
    protected Decimal borLETotalOfsectionF;
    protected Decimal borLETotalOfsectionG;
    protected Decimal borLETotalOfsectionH;
    protected Decimal borUnroundedLETotalOfsectionA;
    protected Decimal borUnroundedLETotalOfsectionB;
    protected Decimal borUnroundedLETotalOfsectionC;
    protected Decimal borUnroundedLETotalOfsectionE;
    protected Decimal borUnroundedLETotalOfsectionF;
    protected Decimal borUnroundedLETotalOfsectionG;
    protected Decimal borUnroundedLETotalOfsectionH;
    protected bool calculationOnly;
    protected string cdXPathBase = string.Empty;
    protected bool isBiweekly;
    protected bool setLE2X32ToEmpty;
    private List<string> excludeMismoNodes = new List<string>()
    {
      "EllieMae",
      "CDSection",
      "CDPageLineNo",
      "LineItemIn2015Itemization",
      "CDInformation",
      "LEInformation",
      "LELabel",
      "CDLabel",
      "CDPaymentInfo",
      "PrepaidItemMonthlyPaymentAmount",
      "ItemizeServicesType"
    };

    public UCDXmlExporterBase()
      : this(false, true, false)
    {
    }

    public UCDXmlExporterBase(bool forLoanEstimate, bool setTotalFields)
      : this(forLoanEstimate, setTotalFields, false)
    {
    }

    public UCDXmlExporterBase(bool forLoanEstimate, bool setTotalFields, bool fullUCD)
    {
      this.namespaces = new XmlNamespaceManager(this.xmlDoc.NameTable);
      this.forLoanEstimate = forLoanEstimate;
      this.setTotalFields = setTotalFields;
    }

    protected abstract void BuildXMLInner();

    public abstract XmlDocument BuildXMLDocument();

    protected void createHeader()
    {
      this.nodeBase = "MESSAGE/ABOUT_VERSIONS/ABOUT_VERSION/";
      this.priSetValueText("AboutVersionIndentifier", "GSE 1.0", "");
      this.priSetValueText("CreatedDatetime", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss"), "");
      this.priSetValueText("DataVersionIdentifier", "UCD 1.1", "");
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/ABOUT_VERSIONS/ABOUT_VERSION/";
      this.priSetValueText("AboutVersionIndentifier", "GSE 1.0", "");
      this.priSetValueText("CreatedDatetime", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss"), "");
      this.priSetValueText("DataVersionIdentifier", "UCD 1.1", "");
      this.priSetValueText("DataVersionName", "Version 1", "");
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/AMORTIZATION/AMORTIZATION_RULE/";
      this.priSetValueText("AmortizationType", this.getField("608"), "");
    }

    protected void createSectionA()
    {
      this.loopThroughFees(new List<string[]>()
      {
        new string[3]{ "", "0802", "e" }
      }, false, "OriginationCharges", "A");
      if (this.secALines != 1)
        this.secALines = 1;
      List<string[]> feeList = new List<string[]>();
      string str1 = "abcdefghijklmnopqr";
      bool flag = this.getField("NEWHUD.X750") == "Y";
      string str2 = "fgh";
      for (int index = 0; index < GFEItemCollection.GFEItems2010.Count; ++index)
      {
        GFEItem g = GFEItemCollection.GFEItems2010[index];
        if ((g.LineNumber == 801 || g.LineNumber == 802) && (g.LineNumber != 801 || (flag || !(g.ComponentID != "a")) && (!flag || str1.IndexOf(g.ComponentID) != -1)) && (g.LineNumber != 802 || str2.IndexOf(g.ComponentID) != -1) && (!this.forLoanEstimate || g.LineNumber != 801 || !(g.ComponentID == "f")))
          feeList.Add(new string[3]
          {
            this.getFeeDescription(g),
            g.LineNumber.ToString("0000"),
            g.ComponentID
          });
      }
      if (!flag)
      {
        if (!this.forLoanEstimate)
          this.loopThroughFees(new List<string[]>()
          {
            new string[3]{ "", "0801", "f" }
          }, false, "OriginationCharges", "A");
        this.loopThroughFees(new List<string[]>()
        {
          new string[3]{ "", "0801", "e" }
        }, false, "OriginationCharges", "A");
      }
      this.loopThroughFees(feeList, true, "OriginationCharges", "A");
    }

    protected void createSectionBandC()
    {
      bool flag = this.getField("NEWHUD.X1017") == "Y";
      this.setLE2X32ToEmpty = false;
      List<string[]> feeList = new List<string[]>();
      for (int index = 0; index < GFEItemCollection.GFEItems2010.Count; ++index)
      {
        GFEItem g = GFEItemCollection.GFEItems2010[index];
        if (g.LineNumber >= 803 && (!this.forLoanEstimate || g.LineNumber != 834 && g.LineNumber != 835) && g.LineNumber != 901 && g.LineNumber != 903 && g.LineNumber != 904 && (g.LineNumber < 906 || g.LineNumber > 1011) && g.LineNumber != 1103 && (!this.forLoanEstimate || g.LineNumber != 1115 && g.LineNumber != 1116) && (g.LineNumber < 1201 || g.LineNumber > 1210) && g.LineNumber < 1310 && (g.LineNumber != 902 || !(Utils.ParseDecimal((object) this.getField("1107")) == 0M)) && (flag || g.LineNumber != 1101))
          feeList.Add(new string[3]
          {
            this.getFeeDescription(g),
            g.LineNumber.ToString("0000"),
            g.ComponentID
          });
      }
      if (!flag)
        feeList.Add(new string[3]
        {
          "Title - Title Insurance Services",
          "1101",
          "x"
        });
      this.loopThroughFees(feeList, true, "ServicesBorrowerDidNotShopFor", "B");
      this.loopThroughFees(feeList, true, "ServicesBorrowerDidShopFor", "C");
      if (!this.forLoanEstimate)
        return;
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/EXTENSION/EllieMae/";
      this.priSetValueText("ItemizeServicesType", this.getField("LE2.X32"), "");
    }

    protected void createSectionE()
    {
      this.create_NodesFor1202();
      if (this.secELines != 1)
        this.secELines = 1;
      this.create1201FeePayments();
      if (this.forLoanEstimate)
        this.loopThroughFees(new List<string[]>()
        {
          new string[3]{ "", "1203", "" }
        }, false, "TaxesAndOtherGovernmentFees", "E");
      else
        this.loopThroughFees(new List<string[]>()
        {
          new string[3]{ "Transfer Taxes", "1203", "" },
          new string[3]{ "City/County Tax/Stamps", "1204", "" },
          new string[3]{ "State Tax/Stamps", "1205", "" },
          new string[3]{ this.getField("NEWHUD.X1618"), "1209", "" },
          new string[3]{ this.getField("NEWHUD.X1625"), "1210", "" }
        }, true, "TaxesAndOtherGovernmentFees", "E");
    }

    protected void createSectionF()
    {
      for (int i = 903; i >= 901; --i)
      {
        GFEItem item2010ByLineNumber = GFEItemCollection.FindGFEItem2010ByLineNumber(i);
        if (item2010ByLineNumber.LineNumber != 902 || !(Utils.ParseDecimal((object) this.getField("1107")) != 0M))
        {
          string[] pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == i.ToString("0000")));
          if (this.create_PrepaidItemNodes(pocFields, item2010ByLineNumber, "Prepaids"))
          {
            this.create_EllieMaeNodes(item2010ByLineNumber, "F", pocFields);
            this.create_FeePaidTo(pocFields, item2010ByLineNumber, "F");
            this.create_FeePayments(pocFields, item2010ByLineNumber, "F");
          }
        }
      }
      this.loopThroughFees(new List<string[]>()
      {
        new string[3]{ "", "904", "" }
      }, false, "Prepaids", "F");
      List<string[]> feeList = new List<string[]>();
      for (int index = 0; index < GFEItemCollection.GFEItems2010.Count; ++index)
      {
        GFEItem g = GFEItemCollection.GFEItems2010[index];
        if (g.LineNumber >= 906 && g.LineNumber <= 912)
          feeList.Add(new string[3]
          {
            this.getFeeDescription(g),
            g.LineNumber.ToString("0000"),
            g.ComponentID
          });
      }
      this.loopThroughFees(feeList, true, "Prepaids", "F");
    }

    protected void createSectionG()
    {
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/ESCROW/ESCROW_DETAIL/";
      this.priSetValueText("EscrowAggregateAccountingAdjustmentAmount", this.getField("558"), "");
      this.borTotalOfsectionG += Utils.ParseDecimal((object) this.getField("558"));
      this.borTotalOfPACInSecI += Utils.ParseDecimal((object) this.getField("558"));
      int lineNo = 0;
      for (int index = 1002; index <= 1004; ++index)
      {
        lineNo = index;
        if (this.getField("1172") == "FarmersHomeAdministration" && index == 1003)
          lineNo = 1010;
        if ((lineNo != 1002 || !(this.getField("NEWHUD2.X133") != "Y")) && (lineNo != 1004 || !(this.getField("NEWHUD2.X134") != "Y")) && (lineNo != 1010 || !(this.getField("NEWHUD2.X140") != "Y")))
        {
          GFEItem item2010ByLineNumber = GFEItemCollection.FindGFEItem2010ByLineNumber(lineNo);
          string[] pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == lineNo.ToString("0000")));
          if (this.create_EscrowItemNodes(pocFields, item2010ByLineNumber))
          {
            this.create_FeePaidTo(pocFields, item2010ByLineNumber, "G");
            this.create_FeePayments(pocFields, item2010ByLineNumber, "G");
            this.create_EllieMaeNodes(item2010ByLineNumber, "G", pocFields);
          }
        }
      }
      List<string[]> feeList = new List<string[]>();
      for (int index = 0; index < GFEItemCollection.GFEItems2010.Count; ++index)
      {
        GFEItem g = GFEItemCollection.GFEItems2010[index];
        if (g.LineNumber >= 1005 && g.LineNumber <= 1009 && (g.LineNumber != 1005 || !(this.getField("NEWHUD2.X135") != "Y")) && (g.LineNumber != 1006 || !(this.getField("NEWHUD2.X136") != "Y")) && (g.LineNumber != 1007 || !(this.getField("NEWHUD2.X137") != "Y")) && (g.LineNumber != 1008 || !(this.getField("NEWHUD2.X138") != "Y")) && (g.LineNumber != 1009 || !(this.getField("NEWHUD2.X139") != "Y")))
          feeList.Add(new string[3]
          {
            this.getFeeDescription(g),
            g.LineNumber.ToString("0000"),
            g.ComponentID
          });
      }
      this.loopThroughFees(feeList, true, "Escrow", "G");
    }

    protected void createSectionH()
    {
      List<string[]> feeList = new List<string[]>();
      for (int index = 0; index < GFEItemCollection.GFEItems2010.Count; ++index)
      {
        GFEItem g = GFEItemCollection.GFEItems2010[index];
        if (g.LineNumber >= 701 && g.LineNumber != 703 && g.LineNumber <= 704 || g.LineNumber == 1103 || g.LineNumber >= 1310 && g.LineNumber <= 1320)
          feeList.Add(new string[3]
          {
            this.getFeeDescription(g),
            g.LineNumber.ToString("0000"),
            g.ComponentID
          });
      }
      this.loopThroughFees(feeList, true, "OtherCosts", "H");
    }

    public static string GetNewFeeDescription(
      int feeLineNum,
      string originalDesc,
      string taxStampIndicator)
    {
      if (feeLineNum != 1204 && feeLineNum != 1205 || taxStampIndicator == "")
        return originalDesc;
      string str = "Transfer Tax - City/County ";
      if (feeLineNum == 1205)
        str = "Transfer Tax - State ";
      return str + taxStampIndicator;
    }

    private void loopThroughFees(
      List<string[]> feeList,
      bool sortFee,
      string sectionType,
      string sectionID)
    {
      if (feeList == null || feeList.Count == 0)
        return;
      if (sortFee)
        this.sortFees(feeList);
      GFEItem g = (GFEItem) null;
      bool flag1 = false;
      int num = 0;
      bool flag2 = this.getField("NEWHUD.X750") == "Y";
      bool flag3 = this.getField("NEWHUD.X1017") == "Y";
      for (int index = 0; index < feeList.Count; ++index)
      {
        string[] fee = feeList[index];
        g = GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentId(fee[1], fee[2]);
        if (g != null)
        {
          string[] pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == g.LineNumber.ToString("0000") && x[1] == g.ComponentID));
          if (pocFields != null)
          {
            if (!this.forLoanEstimate || g.LineNumber < 1302 || g.LineNumber > 1309)
            {
              if ((flag3 || g.LineNumber < 1101 || g.LineNumber > 1101) && (sectionID == "B" && (!this.forLoanEstimate && this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y" || this.forLoanEstimate && this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y") || sectionID == "C" && (!this.forLoanEstimate && this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) != "Y" || this.forLoanEstimate && this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) != "Y")))
                continue;
            }
            else if (sectionID == "B")
              continue;
            flag1 = false;
            if (!(sectionID == "F") ? (!(sectionID == "G") ? this.create_FeeDetailNodes(pocFields, g, sectionType) : this.create_EscrowItemNodes(pocFields, g)) : this.create_PrepaidItemNodes(pocFields, g, sectionType))
            {
              this.create_EllieMaeNodes(g, sectionID, pocFields);
              this.create_FeePaidTo(pocFields, g, sectionID);
              if (g.LineNumber == 801 && g.ComponentID == "a" && !flag2)
                this.create_801aFeePayments();
              else if (!flag3 && g.LineNumber == 1101)
                this.create_1101FeePayments(sectionID);
              else
                this.create_FeePayments(pocFields, g, sectionID);
              if (this.forLoanEstimate && sectionID == "C")
                ++num;
            }
          }
        }
      }
      if (!this.forLoanEstimate || !(sectionID == "C") || num > 14)
        return;
      this.setLE2X32ToEmpty = true;
    }

    protected void sortFees(List<string[]> feeList)
    {
      List<string[]> strArrayList = new List<string[]>();
      List<string[]> collection = new List<string[]>();
      string[] strArray1 = new string[6]
      {
        "^[ ]",
        "^[\\^-`]",
        "^[~]",
        "^[!-/]",
        "^[0-9]",
        "^[a-zA-Z]"
      };
      string feeDesc = "";
      strArrayList.AddRange((IEnumerable<string[]>) feeList.OrderBy<string[], string>((Func<string[], string>) (item =>
      {
        feeDesc = item[0] + new string(' ', item[0].Length < 240 ? 240 - item[0].Length : 0);
        return feeDesc + "|" + item[0] + "|" + item[1] + "|" + item[2];
      }), (IComparer<string>) StringComparer.OrdinalIgnoreCase));
      foreach (string str in strArray1)
      {
        string item = str;
        collection.AddRange(strArrayList.Where<string[]>((Func<string[], bool>) (x => Regex.Match(x[0], item).Success)));
      }
      if (strArrayList.Count == collection.Count)
      {
        strArrayList.Clear();
        strArrayList = collection;
      }
      else
      {
        foreach (string[] strArray2 in collection)
          strArrayList.Remove(strArray2);
        strArrayList.AddRange((IEnumerable<string[]>) collection);
      }
      feeList.Clear();
      feeList.AddRange((IEnumerable<string[]>) strArrayList);
    }

    protected bool create_FeeDetailNodes(string[] pocFields, GFEItem g, string sectionType)
    {
      if (g.LineNumber == 801 && g.ComponentID == "a" && this.getField("NEWHUD.X750") != "Y")
      {
        bool flag = true;
        for (int index = 300; index <= 861; index += 33)
        {
          if (index != 432 && index != 465 && Utils.ParseDecimal((object) this.getField("NEWHUD2.X" + (object) index)) != 0M)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return false;
      }
      else if (g.LineNumber == 1203 && this.forLoanEstimate)
      {
        if (Utils.ParseDecimal((object) this.getField("NEWHUD2.X3666")) == 0M && Utils.ParseDecimal((object) this.getField("NEWHUD2.X3699")) == 0M && Utils.ParseDecimal((object) this.getField("NEWHUD2.X3732")) == 0M)
          return false;
      }
      else if (g.LineNumber == 1101 && g.ComponentID == "x" && this.getField("NEWHUD.X1017") != "Y")
      {
        bool flag = true;
        for (int index = 2841; index <= 3006; index += 33)
        {
          if (Utils.ParseDecimal((object) this.getField("NEWHUD2.X" + (object) index)) != 0M && (sectionType == "ServicesBorrowerDidNotShopFor" && this.getField("NEWHUD2.X" + (object) (index + (this.forLoanEstimate ? 25 : 26))) != "Y" || sectionType == "ServicesBorrowerDidShopFor" && this.getField("NEWHUD2.X" + (object) (index + (this.forLoanEstimate ? 25 : 26))) == "Y"))
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return false;
      }
      else if (Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT])) == 0M || this.forLoanEstimate && this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]) == "Y" && Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT])) == Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID])) && Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT])) == Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID])))
        return false;
      ++this.feeCount;
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/";
      this.priSetValueText("FeePaidToType", this.getFeePaidToType(this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015PAIDToTYPE])), "");
      this.priSetValueText("FeePercentBasisType", "OriginalLoanAmount", "");
      if (!sectionType.StartsWith("ServicesBorrowerDid"))
      {
        Decimal totalCharge = Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT]));
        if (g.LineNumber == 801 && g.ComponentID == "a" && this.getField("NEWHUD.X750") != "Y")
        {
          for (int index = 300; index <= 861; index += 33)
          {
            if (index != 432 && index != 465 && Utils.ParseDecimal((object) this.getField("NEWHUD2.X" + (object) index)) != 0M)
              totalCharge += Utils.ParseDecimal((object) this.getField("NEWHUD2.X" + (object) index));
          }
        }
        else if (g.LineNumber == 1203 && this.forLoanEstimate)
          totalCharge += Utils.ParseDecimal((object) this.getField("NEWHUD2.X3699")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3732"));
        this.priSetValueText("FeeActualTotalAmount", totalCharge.ToString("N2"), "");
        Decimal num1 = 0M;
        if (g.LineNumber == 802 && g.ComponentID == "e")
          num1 = UCDXmlExporterBase.CalculateOriginationCharges(Utils.ParseDecimal((object) this.getField("2")), totalCharge, Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT])));
        else if (g.LineNumber == 801 && g.ComponentID == "a" && this.getField("NEWHUD.X750") != "Y" || g.LineNumber == 1203 && this.forLoanEstimate)
        {
          Decimal num2 = this.getField("1172") == "FHA" ? Utils.ParseDecimal((object) this.getField("1109")) : Utils.ParseDecimal((object) this.getField("2"));
          if (num2 != 0M)
            num1 = Utils.ArithmeticRounding(totalCharge / num2 * 100M, 3);
        }
        else
          num1 = Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEPERCENT]));
        this.priSetValueText("FeeTotalPercent", num1.ToString("N3"), "");
      }
      this.priSetValueText("FeeType", this.getFeeDescription(g), "", true);
      this.priSetValueText("IntegratedDisclosureSectionType", sectionType, "");
      if (Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT])) > 0M || this.forLoanEstimate && g.LineNumber == 1203 && (Utils.ParseDecimal((object) this.getField("NEWHUD2.X3690")) > 0M || Utils.ParseDecimal((object) this.getField("NEWHUD2.X3723")) > 0M || Utils.ParseDecimal((object) this.getField("NEWHUD2.X3756")) > 0M))
        this.priSetValueText("RegulationZPointsAndFeesIndicator", "True", "");
      else
        this.priSetValueText("RegulationZPointsAndFeesIndicator", "False", "");
      return true;
    }

    internal static Decimal CalculateOriginationCharges(
      Decimal loanAmount,
      Decimal totalCharge,
      Decimal sellerObligatedAmount)
    {
      totalCharge -= sellerObligatedAmount;
      return !(loanAmount != 0M) ? 0M : Utils.ArithmeticRounding(totalCharge / loanAmount * 100M, 3);
    }

    protected void create_FeePayments(string[] pocFields, GFEItem g, string sectionID)
    {
      int num1 = 1;
      string fieldID = (string) null;
      Decimal val = 0M;
      for (int index1 = 1; index1 <= 3; ++index1)
      {
        string feePaymentPaidByType;
        switch (index1)
        {
          case 1:
            feePaymentPaidByType = "Buyer";
            break;
          case 2:
            feePaymentPaidByType = "Seller";
            break;
          default:
            feePaymentPaidByType = "ThirdParty";
            break;
        }
        for (int index2 = 1; index2 <= 2; ++index2)
        {
          Decimal num2 = 0M;
          if (index1 == 1 && index2 == 1)
          {
            fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT];
            if (g.LineNumber == 1203 && this.forLoanEstimate)
              num2 = Utils.ParseDecimal((object) this.getField("NEWHUD2.X3707")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3740"));
          }
          else if (index1 == 1 && index2 == 2)
          {
            fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT];
            if (g.LineNumber == 1203 && this.forLoanEstimate)
              num2 = Utils.ParseDecimal((object) this.getField("NEWHUD2.X3708")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3741"));
          }
          else if (index1 == 2 && index2 == 1)
          {
            fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT];
            if (g.LineNumber == 1203 && this.forLoanEstimate)
              num2 = Utils.ParseDecimal((object) this.getField("NEWHUD2.X3710")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3743"));
          }
          else if (index1 == 2 && index2 == 2)
          {
            fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT];
            if (g.LineNumber == 1203 && this.forLoanEstimate)
              num2 = Utils.ParseDecimal((object) this.getField("NEWHUD2.X3711")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3744"));
          }
          else if (index1 == 3 && index2 == 1)
          {
            fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT];
            num2 = Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT])) + Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]));
            if (g.LineNumber == 1203 && this.forLoanEstimate)
              num2 += Utils.ParseDecimal((object) this.getField("NEWHUD2.X3719")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3752")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3713")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3746")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3716")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3749"));
          }
          else if (index1 == 3 && index2 == 2)
          {
            fieldID = pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT];
            num2 = Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT])) + Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
            if (g.LineNumber == 1203 && this.forLoanEstimate)
              num2 += Utils.ParseDecimal((object) this.getField("NEWHUD2.X3720")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3753")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3714")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3747")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3717")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3750"));
          }
          if (g.LineNumber != 801 || !(g.ComponentID == "f") || index1 == 3 || !(sectionID == "A"))
          {
            if (g.LineNumber == 902 && sectionID == "F" || g.LineNumber == 901 || g.LineNumber >= 903 && g.LineNumber <= 912 && g.LineNumber != 905)
              this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/CLOSING_INFORMATION/PREPAID_ITEMS/PREPAID_ITEM[" + (object) this.prepaidItemCount + "]/PREPAID_ITEM_PAYMENTS/PREPAID_ITEM_PAYMENT[" + (object) num1 + "]/";
            else if (g.LineNumber >= 1001 && g.LineNumber <= 1011)
              this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/ESCROW/ESCROW_ITEMS/ESCROW_ITEM[" + (object) this.escrowItemCount + "]/ESCROW_ITEM_PAYMENTS/ESCROW_ITEM_PAYMENT[" + (object) num1 + "]/";
            else
              this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_PAYMENTS/FEE_PAYMENT[" + (object) num1 + "]/";
            Decimal amt = num2 + (fieldID != "" ? Utils.ParseDecimal((object) this.getField(fieldID)) : 0M);
            if (g.LineNumber == 902 && sectionID == "F" || g.LineNumber == 901 || g.LineNumber >= 903 && g.LineNumber <= 912 && g.LineNumber != 905)
              num1 += this.create_FeePayment(amt, feePaymentPaidByType, index2 != 1, "PrepaidItem");
            else if (g.LineNumber >= 1001 && g.LineNumber <= 1011)
              num1 += this.create_FeePayment(amt, feePaymentPaidByType, index2 != 1, "EscrowItem");
            else
              num1 += this.create_FeePayment(amt, feePaymentPaidByType, index2 != 1, "Fee");
            if (g.LineNumber == 1202 && index1 == 3 && index2 == 1)
              this.priSetValueText("EXTENSION/EllieMae/CDPaymentInfo/@LenderPaidAmount", string.Concat((object) Utils.ParseDecimal((object) this.getField("NEWHUD2.X3652"))), "");
            if (g.LineNumber != 701 && g.LineNumber != 702 && g.LineNumber != 704 || (g.LineNumber == 701 || g.LineNumber == 702 || g.LineNumber == 704) && index1 != 2)
              val += amt;
            if (amt != 0M)
            {
              switch (feePaymentPaidByType)
              {
                case "Buyer":
                  switch (sectionID)
                  {
                    case "A":
                    case "B":
                    case "C":
                      if (index2 == 1)
                      {
                        this.borTotalOfPAC += amt;
                        break;
                      }
                      this.borTotalOfPOC += amt;
                      break;
                    case "E":
                    case "F":
                    case "G":
                    case "H":
                      if (index2 == 1)
                      {
                        this.borTotalOfPACInSecI += amt;
                        break;
                      }
                      this.borTotalOfPOCInSecI += amt;
                      break;
                  }
                  switch (sectionID)
                  {
                    case "A":
                      this.borTotalOfsectionA += amt;
                      continue;
                    case "B":
                      this.borTotalOfsectionB += amt;
                      continue;
                    case "C":
                      this.borTotalOfsectionC += amt;
                      continue;
                    case "E":
                      this.borTotalOfsectionE += amt;
                      continue;
                    case "F":
                      this.borTotalOfsectionF += amt;
                      continue;
                    case "G":
                      this.borTotalOfsectionG += amt;
                      continue;
                    case "H":
                      this.borTotalOfsectionH += amt;
                      continue;
                    default:
                      continue;
                  }
                case "Seller":
                  if (index2 == 1)
                  {
                    this.selTotalOfPAC += amt;
                    continue;
                  }
                  this.selTotalOfPOC += amt;
                  continue;
                default:
                  this.othTotal += amt;
                  continue;
              }
            }
          }
        }
      }
      if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "" && g.LineNumber != 701 && g.LineNumber != 702 && g.LineNumber != 704)
      {
        val -= Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]), 0M);
        if (g.LineNumber == 1203 && this.forLoanEstimate)
          val -= Utils.ParseDecimal((object) this.getField("NEWHUD2.X3728")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3761"));
      }
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(sectionID))
      {
        case 3222007936:
          if (!(sectionID == "E"))
            break;
          this.borLETotalOfsectionE += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionE += val;
          break;
        case 3255563174:
          if (!(sectionID == "G"))
            break;
          this.borLETotalOfsectionG += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionG += val;
          break;
        case 3272340793:
          if (!(sectionID == "F"))
            break;
          this.borLETotalOfsectionF += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionF += val;
          break;
        case 3289118412:
          if (!(sectionID == "A"))
            break;
          this.borLETotalOfsectionA += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionA += val;
          break;
        case 3322673650:
          if (!(sectionID == "C"))
            break;
          this.borLETotalOfsectionC += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionC += val;
          break;
        case 3339451269:
          if (!(sectionID == "B"))
            break;
          this.borLETotalOfsectionB += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionB += val;
          break;
        case 3440116983:
          if (!(sectionID == "H"))
            break;
          this.borLETotalOfsectionH += Utils.ArithmeticRounding(val, 0);
          this.borUnroundedLETotalOfsectionH += val;
          break;
      }
    }

    private int create_FeePayment(
      Decimal amt,
      string feePaymentPaidByType,
      bool isPOC,
      string prefix)
    {
      if (amt == 0M)
        return 0;
      this.priSetValueText(prefix + "ActualPaymentAmount", amt.ToString("N2"), "");
      this.priSetValueText(prefix + "PaymentPaidByType", feePaymentPaidByType, "");
      if (prefix == "PrepaidItem" || prefix == "EscrowItem")
        this.priSetValueText(prefix + "PaymentTimingType", isPOC ? "BeforeClosing" : "AtClosing", "");
      else
        this.priSetValueText("FeePaymentPaidOutsideOfClosingIndicator", isPOC ? "True" : "False", "");
      if (prefix == "PrepaidItem")
        this.priSetValueText("RegulationZPointsAndFeesIndicator", "False", "");
      return 1;
    }

    protected void create_FeePaidTo(string[] pocFields, GFEItem g, string sectionID)
    {
      if (!this.forLoanEstimate && (g.LineNumber == 1204 || g.LineNumber == 1205))
      {
        if (g.LineNumber == 1204 && this.getField(g.PayeeFieldID) == string.Empty && this.getField("NEWHUD2.X122") == "" || g.LineNumber == 1205 && this.getField(g.PayeeFieldID) == string.Empty && this.getField("NEWHUD2.X123") == "")
          return;
      }
      else if (g.PayeeFieldID == "" || this.getField(g.PayeeFieldID) == string.Empty)
        return;
      if (g.LineNumber >= 901 && g.LineNumber <= 912 && g.LineNumber != 905 && sectionID != "B" && sectionID != "C")
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/CLOSING_INFORMATION/PREPAID_ITEMS/PREPAID_ITEM[" + (object) this.prepaidItemCount + "]/PREPAID_ITEM_PAID_TO/LEGAL_ENTITY/LEGAL_ENTITY_DETAIL/";
      else if (g.LineNumber >= 1001 && g.LineNumber <= 1011)
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/ESCROW/ESCROW_ITEMS/ESCROW_ITEM[" + (object) this.escrowItemCount + "]/ESCROW_ITEM_PAID_TO/";
      else
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_PAID_TO/LEGAL_ENTITY/LEGAL_ENTITY_DETAIL/";
      if (!this.forLoanEstimate && g.LineNumber == 1204)
      {
        string field = this.getField("NEWHUD2.X122");
        if (!string.IsNullOrEmpty(field))
          this.priSetValueText("FullName", field, "");
        else
          this.priSetValueText("FullName", this.getField(g.PayeeFieldID), "");
      }
      else if (!this.forLoanEstimate && g.LineNumber == 1205)
      {
        string field = this.getField("NEWHUD2.X123");
        if (!string.IsNullOrEmpty(field))
          this.priSetValueText("FullName", field, "");
        else
          this.priSetValueText("FullName", this.getField(g.PayeeFieldID), "");
      }
      else
        this.priSetValueText("FullName", this.getField(g.PayeeFieldID), "");
    }

    protected void create_EllieMaeNodes(GFEItem g, string sectionID, string[] pocFields)
    {
      this.create_EllieMaeNodes(g, sectionID, (string) null, 0M, false, pocFields);
    }

    private void create_EllieMaeNodes(
      GFEItem g,
      string sectionID,
      string paidBy,
      Decimal paidAmount,
      bool isPOC,
      string[] pocFields)
    {
      if (g.LineNumber >= 901 && g.LineNumber <= 912 && g.LineNumber != 905 && sectionID == "F")
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/CLOSING_INFORMATION/PREPAID_ITEMS/PREPAID_ITEM[" + (object) this.prepaidItemCount + "]/PREPAID_ITEM_DETAIL/EXTENSION/EllieMae/";
      else if (g.LineNumber >= 1001 && g.LineNumber <= 1011)
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/ESCROW/ESCROW_ITEMS/ESCROW_ITEM[" + (object) this.escrowItemCount + "]/ESCROW_ITEM_DETAIL/EXTENSION/EllieMae/";
      else
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/EXTENSION/EllieMae/";
      if (paidBy == null)
        this.create_EllieMaeLabel(g, sectionID, pocFields);
      else
        this.create_EllieMaePaidBy(g, sectionID, paidBy, paidAmount, isPOC);
    }

    private void create_EllieMaeLabel(GFEItem g, string sectionID, string[] pocFields)
    {
      this.priSetValueText("@CDSection", sectionID, "");
      int num1 = 0;
      switch (sectionID)
      {
        case "A":
          num1 = ++this.secALines;
          break;
        case "B":
          num1 = ++this.secBLines;
          break;
        case "C":
          num1 = ++this.secCLines;
          break;
        case "E":
          num1 = ++this.secELines;
          break;
        case "F":
          if (g.LineNumber == 903)
          {
            num1 = this.secFLines = 1;
            break;
          }
          if (g.LineNumber == 902)
          {
            num1 = this.secFLines = 2;
            break;
          }
          if (g.LineNumber == 901)
          {
            num1 = this.secFLines = 3;
            break;
          }
          if (this.secFLines < 3)
            this.secFLines = 3;
          num1 = ++this.secFLines;
          break;
        case "G":
          num1 = ++this.secGLines;
          break;
        case "H":
          num1 = ++this.secHLines;
          break;
      }
      this.priSetValueText("@CDPageLineNo", string.Concat((object) num1), "");
      if (g.LineNumber == 801 && g.ComponentID == "a" && this.getField("NEWHUD.X750") != "Y" || g.LineNumber == 1101 && g.ComponentID == "x" && this.getField("NEWHUD.X1017") != "Y")
        this.priSetValueText("LineItemIn2015Itemization", g.LineNumber.ToString() + (g.ComponentID != "x" ? (object) g.ComponentID : (object) ""), "");
      else
        this.priSetValueText("LineItemIn2015Itemization", g.LineNumber.ToString() + g.ComponentID, "");
      string val = (string) null;
      if (g.LineNumber == 701)
        val = "Real Estate Commission to " + this.getField("L212") + " $ " + this.getField("NEWHUD2.X1");
      else if (g.LineNumber == 702)
        val = "Real Estate Commission to " + this.getField("L214") + " $ " + this.getField("NEWHUD2.X3");
      else if (g.LineNumber == 801 && g.ComponentID == "e")
        val = "Broker Fee to " + this.getField(g.PayeeFieldID);
      else if (g.LineNumber == 802 && g.ComponentID == "e")
      {
        this.priSetValueText("CDInformation/@OriginationPoints", this.getField("NEWHUD.X1150"), "");
        val = this.getField("NEWHUD.X1150") + " % of Loan Amount (Points)";
      }
      else if (g.LineNumber == 902 && sectionID == "F")
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("232"), "");
      else if (g.LineNumber == 901 && sectionID == "F")
      {
        this.priSetValueText("LEInformation/@PrepaidInterestRate", this.getField("3"), "");
        this.priSetValueText("LEInformation/@PrepaidInterestDays", this.getField("332"), "");
      }
      else if (g.LineNumber == 903)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("230"), "");
      else if (g.LineNumber == 904)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("231"), "");
      else if (g.LineNumber == 906)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4400"), "");
      else if (g.LineNumber == 907)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4402"), "");
      else if (g.LineNumber == 908)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4404"), "");
      else if (g.LineNumber == 909)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4406"), "");
      else if (g.LineNumber == 910)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4408"), "");
      else if (g.LineNumber == 911)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4410"), "");
      else if (g.LineNumber == 912)
        this.priSetValueText("PrepaidItemMonthlyPaymentAmount", this.getField("NEWHUD2.X4412"), "");
      else if (g.LineNumber == 1101 && g.ComponentID == "x" && this.getField("NEWHUD.X1017") != "Y")
        val = "Title Insurance Services to " + this.getField("NEWHUD.X202");
      else if (g.LineNumber == 1202)
        val = "Recording Recording Fees   Deed: $ " + this.getField("2402") + " Mortgage: $ " + this.getField("2403") + " Releases: $ " + this.getField("2404");
      else
        val = this.getFeeDescription(g) + " to " + this.getField(g.PayeeFieldID);
      if (val != null)
        this.priSetValueText("CDLabel", val, "");
      Decimal num2;
      if (g.LineNumber == 1203 && this.forLoanEstimate)
      {
        Decimal num3 = Utils.ParseDecimal((object) this.getField(g.BorrowerFieldID)) + Utils.ParseDecimal((object) this.getField("647")) + Utils.ParseDecimal((object) this.getField("648"));
        Decimal num4 = Utils.ParseDecimal((object) this.getField(g.SellerFieldID)) + Utils.ParseDecimal((object) this.getField("593")) + Utils.ParseDecimal((object) this.getField("594"));
        Decimal num5 = Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT])) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3728")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3761"));
        Decimal lenderPaidOnlyAmount = this.getLenderPaidOnlyAmount(g, pocFields);
        this.priSetValueText("CDPaymentInfo/@BorrowerPaidAmount", num3.ToString("N0"), "");
        this.priSetValueText("CDPaymentInfo/@SellerPaidAmount", num4.ToString("N0"), "", true);
        this.priSetValueText("CDPaymentInfo/@SellerObligatedAmount", pocFields == null || !(num5 != 0M) ? "" : num5.ToString("N2"), "", true);
        this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", lenderPaidOnlyAmount.ToString("N0"), "");
        num2 = !this.forLoanEstimate ? Utils.ArithmeticRounding(Utils.ParseDecimal((object) this.getField(g.BorrowerFieldID)) + (g.SellerFieldID != "" ? Utils.ParseDecimal((object) this.getField(g.SellerFieldID)) : 0M) - (pocFields == null || !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "") ? 0M : Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))), 0) : Utils.ArithmeticRounding(num3 + num4 - num5, 0);
      }
      else
      {
        this.priSetValueText("CDPaymentInfo/@BorrowerPaidAmount", this.getField(g.BorrowerFieldID), "");
        this.priSetValueText("CDPaymentInfo/@SellerPaidAmount", this.getField(g.SellerFieldID), "", true);
        this.priSetValueText("CDPaymentInfo/@SellerObligatedAmount", pocFields == null || !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "") ? "" : this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]), "", true);
        this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", string.Concat((object) this.getLenderPaidOnlyAmount(g, pocFields)), "");
        num2 = g.LineNumber == 701 || g.LineNumber == 702 || g.LineNumber == 704 ? Utils.ArithmeticRounding(Utils.ParseDecimal((object) this.getField(g.BorrowerFieldID), 0M), 0) : Utils.ArithmeticRounding(Utils.ParseDecimal((object) this.getField(g.BorrowerFieldID)) + (g.SellerFieldID != "" ? Utils.ParseDecimal((object) this.getField(g.SellerFieldID)) : 0M) - (pocFields == null || !(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "") ? 0M : Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]))), 0);
      }
      this.priSetValueText("CDPaymentInfo/@LoanEstimateAmount", num2.ToString("N0"), "");
    }

    private void create_EllieMaePaidBy(
      GFEItem g,
      string sectionID,
      string paidBy,
      Decimal paidAmount,
      bool isPOC)
    {
      this.priSetValueText("CDPaymentInfo", "", "");
      if (isPOC)
        this.priSetValueText("CDPaymentInfo/@" + paidBy + "POCAmount", paidAmount.ToString("N2"), "");
      else
        this.priSetValueText("CDPaymentInfo/@" + paidBy + "PACAmount", paidAmount.ToString("N2"), "");
    }

    protected bool create_PrepaidItemNodes(string[] pocFields, GFEItem g, string sectionType)
    {
      if (Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT])) == 0M)
        return false;
      ++this.prepaidItemCount;
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/CLOSING_INFORMATION/PREPAID_ITEMS/PREPAID_ITEM[" + (object) this.prepaidItemCount + "]/PREPAID_ITEM_DETAIL/";
      this.priSetValueText("FeePaidToType", this.getFeePaidToType(this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015PAIDToTYPE])), "", true);
      this.priSetValueText("IntegratedDisclosureSectionType", sectionType, "");
      if (g.LineNumber == 901)
      {
        this.priSetValueText("PrepaidItemPaidFromDate", this.getDate("L244"), "");
        this.priSetValueText("PrepaidItemPaidThroughDate", this.getDate("L245"), "");
        this.priSetValueText("PrepaidItemPerDiemAmount", this.getField("333"), "");
      }
      else if (g.LineNumber == 902)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("1209"), "");
      else if (g.LineNumber == 903)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("L251"), "");
      else if (g.LineNumber == 904)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4397"), "");
      else if (g.LineNumber == 906)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4399"), "");
      else if (g.LineNumber == 907)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4401"), "");
      else if (g.LineNumber == 908)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4403"), "");
      else if (g.LineNumber == 909)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4405"), "");
      else if (g.LineNumber == 910)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4407"), "");
      else if (g.LineNumber == 911)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4409"), "");
      else if (g.LineNumber == 912)
        this.priSetValueText("PrepaidItemMonthsPaidCount", this.getField("NEWHUD2.X4411"), "");
      this.priSetValueText("PrepaidItemType", this.getFeeDescription(g, "F"), "", true);
      return true;
    }

    protected bool create_EscrowItemNodes(string[] pocFields, GFEItem g)
    {
      if (Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT])) == 0M)
        return false;
      ++this.escrowItemCount;
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/ESCROW/ESCROW_ITEMS/ESCROW_ITEM[" + (object) this.escrowItemCount + "]/ESCROW_ITEM_DETAIL/";
      string fieldID1 = "";
      string fieldID2 = "";
      switch (g.LineNumber)
      {
        case 1002:
          fieldID1 = "1387";
          fieldID2 = this.isBiweekly ? "HUD53" : "230";
          break;
        case 1003:
          fieldID1 = "1296";
          fieldID2 = this.isBiweekly ? "HUD54" : "232";
          break;
        case 1004:
          fieldID1 = "1386";
          fieldID2 = this.isBiweekly ? "HUD52" : "231";
          break;
        case 1005:
          fieldID1 = "L267";
          fieldID2 = this.isBiweekly ? "HUD56" : "L268";
          break;
        case 1006:
          fieldID1 = "1388";
          fieldID2 = this.isBiweekly ? "HUD55" : "235";
          break;
        case 1007:
          fieldID1 = "1629";
          fieldID2 = this.isBiweekly ? "HUD58" : "1630";
          break;
        case 1008:
          fieldID1 = "340";
          fieldID2 = this.isBiweekly ? "HUD60" : "253";
          break;
        case 1009:
          fieldID1 = "341";
          fieldID2 = this.isBiweekly ? "HUD62" : "254";
          break;
        case 1010:
          fieldID1 = "NEWHUD.X1706";
          fieldID2 = this.isBiweekly ? "HUD63" : "NEWHUD.X1707";
          break;
      }
      this.priSetValueText("EscrowCollectedNumberOfMonthsCount", fieldID1 != string.Empty ? this.getField(fieldID1) : "", "");
      string val = "";
      switch (g.LineNumber)
      {
        case 1002:
          val = "HomeownersInsurance";
          break;
        case 1003:
        case 1010:
          val = "MI";
          break;
        case 1004:
          val = "PropertyTaxes";
          break;
        case 1005:
          val = "City Property Tax";
          break;
        case 1006:
          val = "Flood Insurance";
          break;
        case 1007:
          val = this.getField("1628");
          break;
        case 1008:
          val = this.getField("660");
          break;
        case 1009:
          val = this.getField("661");
          break;
      }
      this.priSetValueText("EscrowItemCategoryType", val, "", true);
      this.priSetValueText("EscrowMonthlyPaymentAmount", fieldID2 != string.Empty ? this.getField(fieldID2) : "", "");
      this.priSetValueText("FeePaidToType", this.getFeePaidToType(this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015PAIDToTYPE])), "");
      this.priSetValueText("IntegratedDisclosureSectionType", "InitialEscrowPaymentAtClosing", "");
      if (Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT])) > 0M)
        this.priSetValueText("RegulationZPointsAndFeesIndicator", "True", "");
      else
        this.priSetValueText("RegulationZPointsAndFeesIndicator", "False", "");
      return true;
    }

    private void create_NodesFor1202()
    {
      bool flag = false;
      Decimal num = Utils.ParseDecimal((object) this.getField("NEWHUD2.X3652")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3784")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3817")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3850"));
      for (int index = 2402; index <= 2404; ++index)
      {
        if (this.getField(string.Concat((object) index)) != string.Empty)
        {
          ++this.feeCount;
          this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/";
          this.priSetValueText("FeeActualTotalAmount", this.getField(string.Concat((object) index)), "");
          switch (index)
          {
            case 2402:
              this.priSetValueText("FeeType", "RecordingFeeForDeed", "");
              break;
            case 2403:
              this.priSetValueText("FeeType", "RecordingFeeForMortgage", "");
              break;
            case 2404:
              this.priSetValueText("FeeType", "RecordingFeeForReleses", "");
              break;
          }
          this.priSetValueText("IntegratedDisclosureSectionType", "TaxesAndOtherGovernmentFees", "");
          this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/EXTENSION/EllieMae/";
          this.priSetValueText("@CDPageLineNo", "1", "");
          this.priSetValueText("@CDSection", "E", "");
          this.priSetValueText("LineItemIn2015Itemization", "1202", "");
          this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", num.ToString("N2"), "");
          flag = true;
        }
      }
      if (flag)
        return;
      ++this.feeCount;
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/";
      this.priSetValueText("FeeActualTotalAmount", this.getField("NEWHUD2.X3633"), "");
      this.priSetValueText("FeeType", "RecordingFeeTotal", "");
      this.priSetValueText("IntegratedDisclosureSectionType", "TaxesAndOtherGovernmentFees", "");
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/EXTENSION/EllieMae/";
      this.priSetValueText("@CDPageLineNo", "1", "");
      this.priSetValueText("@CDSection", "E", "");
      this.priSetValueText("LineItemIn2015Itemization", "1202", "");
      this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", Utils.ParseDecimal((object) this.getField("NEWHUD2.X3652")).ToString("N0"), "");
    }

    protected void create_801aFeePayments()
    {
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      Decimal num8 = 0M;
      string[] lines = new string[16]
      {
        "a",
        "b",
        "c",
        "d",
        "g",
        "h",
        "i",
        "j",
        "k",
        "l",
        "m",
        "n",
        "o",
        "p",
        "q",
        "r"
      };
      GFEItem g = (GFEItem) null;
      for (int i = 0; i < lines.Length; ++i)
      {
        g = GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentId(801, lines[i]);
        string[] pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == "0801" && x[1] == lines[i]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != "")
          num1 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
          num2 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT] != "")
          num3 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "")
          num4 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
          num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
          num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
          num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
          num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
          num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
          num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
          num7 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
          num8 += this.getLenderPaidOnlyAmount(g, pocFields);
      }
      this.borTotalOfsectionA += num1 + num2;
      Decimal val = num1 + num2 + num3 + num4 + num5 + num6 - num7;
      this.borUnroundedLETotalOfsectionA += val;
      this.borLETotalOfsectionA += Utils.ArithmeticRounding(val, 0);
      this.borTotalOfPAC += num1;
      this.borTotalOfPOC += num2;
      this.selTotalOfPAC += num3;
      this.selTotalOfPOC += num4;
      this.othTotal += num5 + num6;
      List<Decimal> numList = new List<Decimal>()
      {
        num1,
        num2,
        num3,
        num4,
        num5,
        num6
      };
      int num9 = 1;
      for (int index = 0; index < numList.Count; ++index)
      {
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_PAYMENTS/FEE_PAYMENT[" + (object) num9 + "]/";
        num9 += this.create_FeePayment(numList[index], index < 2 ? "Buyer" : (index < 4 ? "Seller" : "ThirdParty"), (index + 1) % 2 == 0, "Fee");
        if (numList[index] != 0M)
          this.create_EllieMaeNodes(g, "A", index < 2 ? "Buyer" : (index < 4 ? "Seller" : "ThirdParty"), numList[index], (index + 1) % 2 == 0, (string[]) null);
      }
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/EXTENSION/EllieMae/";
      Decimal num10 = num1 + num2;
      this.priSetValueText("CDPaymentInfo/@BorrowerPaidAmount", num10.ToString("N2"), "");
      num10 = num3 + num4;
      this.priSetValueText("CDPaymentInfo/@SellerPaidAmount", num10.ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@SellerObligatedAmount", num7.ToString("N2"), "", true);
      this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", num8.ToString("N2"), "");
      num10 = Utils.ArithmeticRounding(num1 + num2 + num3 + num4 + this.othTotal - num7, 0);
      this.priSetValueText("CDPaymentInfo/@LoanEstimateAmount", num10.ToString("N0"), "");
    }

    protected void create_1101FeePayments(string sectionID)
    {
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      Decimal num8 = 0M;
      string[] lines = new string[6]
      {
        "1101a",
        "1101b",
        "1101c",
        "1101d",
        "1101e",
        "1101f"
      };
      GFEItem g = (GFEItem) null;
      for (int i = 0; i < lines.Length; ++i)
      {
        string[] pocFields;
        if (lines[i].Length > 4)
        {
          g = GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentId(lines[i].Substring(0, 4), lines[i].Substring(4, 1));
          pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == lines[i].Substring(0, 4) && x[1] == lines[i].Substring(4, 1)));
        }
        else
        {
          g = GFEItemCollection.FindGFEItem2010ByLineNumber(Utils.ParseInt((object) lines[i]));
          pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == lines[i]));
        }
        if ((!(sectionID == "B") || !(this.getField(this.forLoanEstimate ? pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] : pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y")) && (!(sectionID == "C") || !(this.getField(this.forLoanEstimate ? pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] : pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) != "Y")))
        {
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != "")
            num1 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
            num2 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT] != "")
            num3 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "")
            num4 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
            num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
            num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
            num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
            num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
            num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
            num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
            num7 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]));
          if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
            num8 += this.getLenderPaidOnlyAmount(g, pocFields);
        }
      }
      Decimal val = num1 + num2 + num3 + num4 + num5 + num6 - num7;
      switch (sectionID)
      {
        case "B":
          this.borTotalOfsectionB += num1 + num2;
          this.borUnroundedLETotalOfsectionB += val;
          this.borLETotalOfsectionB += Utils.ArithmeticRounding(val, 0);
          break;
        case "C":
          this.borTotalOfsectionC += num1 + num2;
          this.borUnroundedLETotalOfsectionC += val;
          this.borLETotalOfsectionC += Utils.ArithmeticRounding(val, 0);
          break;
      }
      this.borTotalOfPAC += num1;
      this.borTotalOfPOC += num2;
      this.selTotalOfPAC += num3;
      this.selTotalOfPOC += num4;
      this.othTotal += num5 + num6;
      List<Decimal> numList = new List<Decimal>()
      {
        num1,
        num2,
        num3,
        num4,
        num5,
        num6
      };
      int num9 = 1;
      for (int index = 0; index < numList.Count; ++index)
      {
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_PAYMENTS/FEE_PAYMENT[" + (object) num9 + "]/";
        num9 += this.create_FeePayment(numList[index], index < 2 ? "Buyer" : (index < 4 ? "Seller" : "ThirdParty"), (index + 1) % 2 == 0, "Fee");
        if (numList[index] != 0M)
          this.create_EllieMaeNodes(g, "A", index < 2 ? "Buyer" : (index < 4 ? "Seller" : "ThirdParty"), numList[index], (index + 1) % 2 == 0, (string[]) null);
      }
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/EXTENSION/EllieMae/";
      Decimal num10 = num1 + num2;
      this.priSetValueText("CDPaymentInfo/@BorrowerPaidAmount", num10.ToString("N2"), "");
      num10 = num3 + num4;
      this.priSetValueText("CDPaymentInfo/@SellerPaidAmount", num10.ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@SellerObligatedAmount", num7.ToString("N2"), "", true);
      this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", num8.ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@LoanEstimateAmount", Utils.ArithmeticRounding(num1 + num2 + num3 + num4 + this.othTotal - num7, 0).ToString("N0"), "");
    }

    private void create1201FeePayments()
    {
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      Decimal num8 = 0M;
      string[] lines = new string[4]
      {
        "1202",
        "1206",
        "1207",
        "1208"
      };
      GFEItem g = (GFEItem) null;
      for (int i = 0; i < lines.Length; ++i)
      {
        string[] pocFields;
        if (lines[i].Length > 4)
        {
          g = GFEItemCollection.FindGFEItem2010ByLineNumberAndComponentId(lines[i].Substring(0, 4), lines[i].Substring(4, 1));
          pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == lines[i].Substring(0, 4) && x[1] == lines[i].Substring(4, 1)));
        }
        else
        {
          g = GFEItemCollection.FindGFEItem2010ByLineNumber(Utils.ParseInt((object) lines[i]));
          pocFields = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == lines[i]));
        }
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != "")
          num1 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
          num2 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT] != "")
          num3 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "")
          num4 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
          num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
          num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
          num5 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
          num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
          num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
          num6 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
          num7 += Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]));
        if (pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "" || pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
          num8 += this.getLenderPaidOnlyAmount(g, pocFields);
      }
      this.borTotalOfsectionE += num1 + num2;
      Decimal val = num1 + num2 + num3 + num4 + num5 + num6 - num7;
      this.borUnroundedLETotalOfsectionE += val;
      this.borLETotalOfsectionE += Utils.ArithmeticRounding(val, 0);
      if (this.forLoanEstimate)
      {
        this.borTotalOfPAC += num1;
        this.borTotalOfPOC += num2;
      }
      else
      {
        this.borTotalOfPACInSecI += num1;
        this.borTotalOfPOCInSecI += num2;
      }
      this.selTotalOfPAC += num3;
      this.selTotalOfPOC += num4;
      this.othTotal += num5 + num6;
      List<Decimal> numList = new List<Decimal>()
      {
        num1,
        num2,
        num3,
        num4,
        num5,
        num6
      };
      int num9 = 1;
      for (int index = 0; index < numList.Count; ++index)
      {
        this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_PAYMENTS/FEE_PAYMENT[" + (object) num9 + "]/";
        num9 += this.create_FeePayment(numList[index], index < 2 ? "Buyer" : (index < 4 ? "Seller" : "ThirdParty"), (index + 1) % 2 == 0, "Fee");
        if (numList[index] != 0M)
          this.create_EllieMaeNodes(g, "A", index < 2 ? "Buyer" : (index < 4 ? "Seller" : "ThirdParty"), numList[index], (index + 1) % 2 == 0, (string[]) null);
      }
      this.nodeBase = "MESSAGE/" + this.cdXPathBase + "DEAL_SETS/DEAL_SET/DEALS/DEAL/LOANS/LOAN/FEE_INFORMATION/FEES/FEE[" + (object) this.feeCount + "]/FEE_DETAIL/EXTENSION/EllieMae/";
      this.priSetValueText("CDPaymentInfo/@BorrowerPaidAmount", (num1 + num2).ToString("N2") == "0.00" ? "" : (num1 + num2).ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@SellerPaidAmount", (num3 + num4).ToString("N2") == "0.00" ? "" : (num3 + num4).ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@SellerObligatedAmount", num7.ToString("N2") == "0.00" ? "" : num7.ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@LenderPaidAmount", num8.ToString("N2"), "");
      this.priSetValueText("CDPaymentInfo/@LoanEstimateAmount", Utils.ArithmeticRounding(num1 + num2 + num3 + num4 + this.othTotal - num7, 0).ToString("N0") == "0.00" ? "" : Utils.ArithmeticRounding(num1 + num2 + num3 + num4 + this.othTotal - num7, 0).ToString("N0"), "");
    }

    private Decimal getLenderPaidOnlyAmount(GFEItem g, string[] pocFields)
    {
      if (pocFields == null || g.LineNumber != 1203 && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] == "" && pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] == "")
        return 0M;
      return g.LineNumber == 1203 && this.forLoanEstimate ? Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT])) + Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT])) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3716")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3717")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3749")) + Utils.ParseDecimal((object) this.getField("NEWHUD2.X3750")) : Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT])) + Utils.ParseDecimal((object) this.getField(pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]));
    }

    private string getDate(string fieldID)
    {
      DateTime date = Utils.ParseDate((object) this.getField(fieldID));
      return date != DateTime.MinValue ? date.ToString("yyyy-MM-dd") : string.Empty;
    }

    protected abstract string getField(string fieldID);

    private string getFeePaidToType(string val)
    {
      switch (val.ToUpper())
      {
        case "Affiliate":
          return "AffiliateProvider";
        case "Lender":
        case "Investor":
        case "Broker":
          return val;
        default:
          return "ThirdPartyProvider";
      }
    }

    private string getFeeDescription(GFEItem g) => this.getFeeDescription(g, "");

    private string getFeeDescription(GFEItem g, string sectionID)
    {
      if (g.LineNumber == 801 && g.ComponentID == "a")
        return "Origination Fee";
      if (g.LineNumber == 801 && g.ComponentID == "e")
        return "Broker Fee";
      if (g.LineNumber == 802 && g.ComponentID == "e")
        return "LoanDiscountPoints";
      if (g.LineNumber == 701 || g.LineNumber == 702)
        return "Real Estate Commission";
      if (g.LineNumber == 901 && sectionID == "F")
        return "PrepaidInterest";
      if (g.LineNumber == 902 && sectionID == "F")
        return "MortgageInsurancePremium";
      if (g.LineNumber == 903 && sectionID == "F")
        return "HomeownersInsurancePremium";
      if (g.LineNumber == 904 && sectionID == "F")
        return "CountyPropertyTax";
      if (g.LineNumber == 906 && sectionID == "F")
        return "Flood Insurance";
      if (g.LineNumber == 1101 && g.ComponentID == "x" && this.getField("NEWHUD.X1017") != "Y")
        return "Title - Title Insurance Services";
      if (g.LineNumber == 1103)
        return "Title - Owner's Title Insurance" + (this.getField("NEWHUD2.X3335") == "Y" ? " (optional)" : "");
      if (g.LineNumber == 1204 || g.LineNumber == 1205)
      {
        string taxStampIndicator = g.LineNumber == 1204 ? this.getField("4855") : this.getField("4856");
        return UCDXmlExporterBase.GetNewFeeDescription(g.LineNumber, g.Description, taxStampIndicator);
      }
      string feeDescription = g.Description.Length <= 4 || g.Description.StartsWith("NEWHUD") ? this.getField(g.Description) : g.Description;
      if (g.LineNumber >= 1101 && g.LineNumber <= 1116 && !feeDescription.ToLower().StartsWith("title - "))
        feeDescription = "Title - " + feeDescription;
      else if (g.LineNumber >= 1310 && !feeDescription.ToLower().EndsWith("(optional)"))
      {
        switch (g.LineNumber)
        {
          case 1310:
            feeDescription += this.getField("NEWHUD2.X4196") == "Y" ? " (optional)" : "";
            break;
          case 1311:
            feeDescription += this.getField("NEWHUD2.X4229") == "Y" ? " (optional)" : "";
            break;
          case 1312:
            feeDescription += this.getField("NEWHUD2.X4262") == "Y" ? " (optional)" : "";
            break;
          case 1313:
            feeDescription += this.getField("NEWHUD2.X4295") == "Y" ? " (optional)" : "";
            break;
          case 1314:
            feeDescription += this.getField("NEWHUD2.X4328") == "Y" ? " (optional)" : "";
            break;
          case 1315:
            feeDescription += this.getField("NEWHUD2.X4361") == "Y" ? " (optional)" : "";
            break;
          case 1316:
            feeDescription += this.getField("NEWHUD2.X4447") == "Y" ? " (optional)" : "";
            break;
          case 1317:
            feeDescription += this.getField("NEWHUD2.X4480") == "Y" ? " (optional)" : "";
            break;
          case 1318:
            feeDescription += this.getField("NEWHUD2.X4513") == "Y" ? " (optional)" : "";
            break;
          case 1319:
            feeDescription += this.getField("NEWHUD2.X4546") == "Y" ? " (optional)" : "";
            break;
          case 1320:
            feeDescription += this.getField("NEWHUD2.X4579") == "Y" ? " (optional)" : "";
            break;
        }
      }
      return feeDescription;
    }

    private string getAmortizationType()
    {
      switch (this.getField("608"))
      {
        case "Fixed":
          return "Fixed";
        case "GraduatedPaymentMortgage":
          return "GPM";
        case "AdjustableRate":
          return "AdjustableRate";
        case "OtherAmortizationType":
          return "Step";
        default:
          return "";
      }
    }

    private string getMortgageType()
    {
      switch (this.getField("1172"))
      {
        case "Conventional":
          return "Conventional";
        case "VA":
          return "VA";
        case "FHA":
          return "FHA";
        case "FarmersHomeAdministration":
          return "USDARuralDevelopment";
        default:
          return "Other";
      }
    }

    private string getProrationItemAssessmentType()
    {
      switch (this.getField("1556"))
      {
        case "Condominium":
          return "CondominiumAssociationSpecialAssessment";
        case "Cooperative":
          return "CooperativeAssociationSpecialAssessment";
        default:
          return "HomeownersAssociationSpecialAssessment";
      }
    }

    private string getProrationItemOtherType(string fieldValue) => "Other";

    private string getOtherCreditsType(string fieldValue) => "Other";

    private string getDueFromBorrowerLiabilityType() => "Other";

    private string getSectionKAdjustmentType(string fieldValue) => "Other";

    private string getSectionLOtherItemType() => "Other";

    private string getSectionLAdjustmentType(string fieldValue) => "Other";

    private string getSectionMAdjustmentType(string fieldValue) => "Other";

    private string getPayoffOfMortgageLoanLiabilityOtherType(string fieldValue) => "Other";

    private string getPayoffAndPaymentsLiabilityType() => "Other";

    private string getLateChargeType()
    {
      if (this.getField("3876") != string.Empty)
        return "NoLateCharge";
      if (this.getField("674") != string.Empty)
        return "FlatDollarAmount";
      if (this.getField("1719") == "of the payment" || this.getField("1719") == "of any installment")
        return "PercentageOfTotalPayment";
      if (this.getField("1719") == "of the overdue payment")
        return "PercentageOfNetPayment";
      if (this.getField("1719") == "of the interest payment due")
        return "PercentageOfDelinquentInterest";
      return this.getField("1719") == "of the principal and interest overdue" ? "PercentOfPrincipalAndInterest" : "";
    }

    private void priSetValueText(string sNode, string val, string exportFieldIDorDescription)
    {
      if (this.calculationOnly)
        return;
      this.priSetValueText(sNode, val, exportFieldIDorDescription, false);
    }

    private void priSetValueText(
      string sNode,
      string val,
      string exportFieldIDorDescription,
      bool alwaysCreate)
    {
      if (this.calculationOnly || val == string.Empty && !alwaysCreate)
        return;
      this.XPath(this.nodeBase + sNode, val.Trim());
    }

    private void XPath(string xmlPath, string New_Value)
    {
      string[] strArray = xmlPath.Split('/');
      XmlNode xmlNode1 = (XmlNode) this.xmlDoc;
      try
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          string name1 = strArray[index];
          string xpath1 = this.excludeMismoNodes.Contains(name1) || !(name1.Substring(0, 1) != "@") ? name1 : "mismo:" + name1;
          if (xpath1.Substring(0, 1) == "@")
          {
            string name2 = xpath1.Substring(1);
            ((XmlElement) xmlNode1).SetAttribute(name2, New_Value);
            return;
          }
          XmlNode xmlNode2;
          if (xpath1.IndexOf("[@") > 0)
          {
            string str1 = xpath1.Substring(xpath1.IndexOf("[@") + 2);
            string str2 = str1.Substring(0, str1.LastIndexOf("]"));
            string str3 = str2.Substring(str2.IndexOf("=") + 1);
            string name3 = str2.Substring(0, str2.LastIndexOf("="));
            string str4 = xpath1.Substring(0, xpath1.LastIndexOf("[@"));
            string name4 = name1.Substring(0, name1.LastIndexOf("[@"));
            string xpath2 = str4 + "[@" + name3 + " = \"" + str3 + "\"]";
            xmlNode2 = xmlNode1.SelectSingleNode(xpath2, this.namespaces);
            if (xmlNode2 == null)
            {
              XmlNode newChild = !this.excludeMismoNodes.Contains(name4) ? this.xmlDoc.CreateNode(XmlNodeType.Element, "mismo", name4, "http://www.mismo.org/residential/2009/schemas") : this.xmlDoc.CreateNode(XmlNodeType.Element, name4, "");
              xmlNode2 = xmlNode1.AppendChild(newChild);
              ((XmlElement) xmlNode2).SetAttribute(name3, str3);
            }
          }
          else if (xpath1.IndexOf("[") != -1)
          {
            int num = Utils.ParseInt((object) xpath1.Substring(xpath1.LastIndexOf("[") + 1, xpath1.LastIndexOf("]") - xpath1.LastIndexOf("[") - 1), 0);
            string xpath3 = xpath1.Substring(0, xpath1.LastIndexOf("["));
            string name5 = name1.Substring(0, name1.LastIndexOf("["));
            XmlNodeList xmlNodeList;
            for (xmlNodeList = xmlNode1.SelectNodes(xpath3, this.namespaces); num > xmlNodeList.Count; xmlNodeList = xmlNode1.SelectNodes(xpath3, this.namespaces))
            {
              XmlNode newChild = !this.excludeMismoNodes.Contains(name5) ? this.xmlDoc.CreateNode(XmlNodeType.Element, "mismo", name5, "http://www.mismo.org/residential/2009/schemas") : this.xmlDoc.CreateNode(XmlNodeType.Element, name5, "");
              xmlNode1.AppendChild(newChild);
            }
            xmlNode2 = xmlNodeList.Item(num - 1);
          }
          else
          {
            xmlNode2 = xmlNode1.SelectSingleNode(xpath1, this.namespaces);
            if (xmlNode2 == null)
            {
              XmlNode newChild = !this.excludeMismoNodes.Contains(name1) ? this.xmlDoc.CreateNode(XmlNodeType.Element, "mismo", name1, "http://www.mismo.org/residential/2009/schemas") : this.xmlDoc.CreateNode(XmlNodeType.Element, name1, "");
              xmlNode2 = xmlNode1.AppendChild(newChild);
            }
          }
          xmlNode1 = xmlNode2;
        }
        xmlNode1.InnerText = New_Value;
      }
      catch
      {
      }
    }

    protected static string removeSpecialCharacters(string val)
    {
      val = val.Replace('\t', ' ');
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in val)
      {
        if (ch >= ' ' && ch <= '~' || ch == '\n' || ch == '\r')
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString().Trim();
    }
  }
}
