// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UCDXmlParser
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class UCDXmlParser
  {
    private XmlDocument xmlDocument;
    private Dictionary<string, string> formControlToDataMap = new Dictionary<string, string>();
    private XmlNamespaceManager nsmgr;
    private string cdXPath = "";

    public UCDXmlParser(XmlDocument doc)
    {
      this.xmlDocument = doc;
      this.nsmgr = new XmlNamespaceManager(doc.NameTable);
      this.nsmgr.AddNamespace("mismo", "http://www.mismo.org/residential/2009/schemas");
    }

    public Dictionary<string, string> ParseXml() => this.ParseXml(false);

    public Dictionary<string, string> ParseXml(bool forLoanEstimate)
    {
      this.cdXPath = "";
      this.BuildLoanCosts("OriginationCharges");
      this.BuildLoanCosts("ServicesBorrowerDidNotShopFor");
      this.BuildLoanCosts("ServicesBorrowerDidShopFor");
      this.BuildLoanCosts("TaxesAndOtherGovernmentFees");
      this.BuildLoanCosts("OtherCosts");
      this.BuildEscrowCosts("InitialEscrowPaymentAtClosing");
      this.BuildPrepaidCosts("Prepaids");
      return this.formControlToDataMap;
    }

    public void BuildLoanCosts(string section)
    {
      int row = 1;
      string str1 = "mismo:MESSAGE/" + this.cdXPath + "mismo:DEAL_SETS/mismo:DEAL_SET/mismo:DEALS/mismo:DEAL/mismo:LOANS/mismo:LOAN/mismo:FEE_INFORMATION/mismo:FEES/mismo:FEE[mismo:FEE_DETAIL";
      switch (section)
      {
        case "OriginationCharges":
          string xpath1 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType='LoanDiscountPoints']";
          XmlNode xmlNode1 = this.xmlDocument.SelectSingleNode(xpath1 + "/mismo:FEE_DETAIL/mismo:FeeTotalPercent", this.nsmgr);
          if (xmlNode1 != null)
            this.formControlToDataMap.Add(section + (object) row + "A1", Utils.RemoveEndingZeros(Utils.FormatLEAndCDPercentageValue(xmlNode1.InnerText)));
          XmlNode parentNode = this.xmlDocument.SelectSingleNode(xpath1 + "/mismo:FEE_PAID_TO/mismo:LEGAL_ENTITY/mismo:LEGAL_ENTITY_DETAIL/mismo:FullName", this.nsmgr);
          if (parentNode != null)
            this.formControlToDataMap.Add(section + (object) row + "A2", parentNode.InnerText);
          XmlNodeList xmlNodeList1 = this.xmlDocument.SelectNodes(xpath1, this.nsmgr);
          this.addCDLineInfo(parentNode, section, row);
          foreach (XmlNode XNode in xmlNodeList1)
            this.Buildfees(xpath1, XNode, row, section);
          ++row;
          break;
        case "TaxesAndOtherGovernmentFees":
          string str2 = "";
          XmlNode xmlNode2 = this.xmlDocument.SelectSingleNode(str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType='RecordingFeeForDeed']" + "/mismo:FEE_DETAIL/mismo:FeeActualTotalAmount", this.nsmgr);
          if (xmlNode2 != null)
          {
            this.formControlToDataMap.Add(section + (object) row + "A1", xmlNode2.InnerText);
            str2 = "RecordingFeeForDeed";
          }
          XmlNode xmlNode3 = this.xmlDocument.SelectSingleNode(str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType='RecordingFeeForMortgage']" + "/mismo:FEE_DETAIL/mismo:FeeActualTotalAmount", this.nsmgr);
          if (xmlNode3 != null)
          {
            this.formControlToDataMap.Add(section + (object) row + "A2", xmlNode3.InnerText);
            str2 = "RecordingFeeForMortgage";
          }
          string xpath2 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType='RecordingFeeForReleses']";
          if (this.xmlDocument.SelectSingleNode(xpath2 + "/mismo:FEE_DETAIL/mismo:FeeActualTotalAmount", this.nsmgr) != null)
          {
            this.xmlDocument.SelectNodes(xpath2, this.nsmgr);
            str2 = "RecordingFeeForReleses";
          }
          string xpath3;
          XmlNodeList xmlNodeList2;
          if (str2 != "")
          {
            xpath3 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType='" + str2 + "']";
            xmlNodeList2 = this.xmlDocument.SelectNodes(xpath3, this.nsmgr);
          }
          else
          {
            xpath3 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType='RecordingFeeTotal']";
            xmlNodeList2 = this.xmlDocument.SelectNodes(xpath3, this.nsmgr);
          }
          this.addCDLineInfo(this.xmlDocument.SelectSingleNode(xpath3, this.nsmgr), section, row);
          foreach (XmlNode XNode in xmlNodeList2)
            this.Buildfees(xpath3, XNode, row, section);
          ++row;
          break;
      }
      string xpath4 = "";
      if (section == "OriginationCharges")
        xpath4 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType!='LoanDiscountPoints']";
      else if (section == "ServicesBorrowerDidShopFor" || section == "ServicesBorrowerDidNotShopFor" || section == "OtherCosts")
        xpath4 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "']";
      else if (section == "TaxesAndOtherGovernmentFees")
        xpath4 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:FEE_DETAIL/mismo:FeeType!='RecordingFeeForDeed' and mismo:FEE_DETAIL/mismo:FeeType!='RecordingFeeForMortgage' and  mismo:FEE_DETAIL/mismo:FeeType!='RecordingFeeForReleses' and  mismo:FEE_DETAIL/mismo:FeeType!='RecordingFeeTotal']";
      foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath4, this.nsmgr))
      {
        XmlNode xmlNode4 = selectNode.SelectSingleNode("mismo:FEE_DETAIL/mismo:FeeType", this.nsmgr);
        string str3 = "";
        string str4 = "";
        string str5 = "";
        if (xmlNode4 != null)
          str3 = xmlNode4.InnerText;
        if (section == "TaxesAndOtherGovernmentFees")
        {
          XmlNode xmlNode5 = selectNode.SelectSingleNode("mismo:FEE_DETAIL/mismo:FeeActualTotalAmount", this.nsmgr);
          if (xmlNode5 != null)
            str4 = xmlNode5.InnerText;
        }
        XmlNode xmlNode6 = selectNode.SelectSingleNode("mismo:FEE_PAID_TO/mismo:LEGAL_ENTITY/mismo:LEGAL_ENTITY_DETAIL/mismo:FullName", this.nsmgr);
        if (xmlNode6 != null)
          str5 = xmlNode6.InnerText;
        string str6 = str3;
        selectNode.SelectSingleNode("mismo:FEE_DETAIL/mismo:EXTENSION/EllieMae/LineItemIn2015Itemization", this.nsmgr);
        if (!this.ShouldAppendTitle("mismo:FEE_DETAIL", selectNode, -1, ""))
          this.formControlToDataMap.Add(section + (object) row + "A1", str6);
        else
          this.formControlToDataMap.Add(section + (object) row + "A1", "Title - " + str6);
        this.formControlToDataMap.Add(section + (object) row + "A2", str5);
        this.Buildfees(xpath4, selectNode, row, section);
        this.addCDLineInfo(selectNode, section, row);
        ++row;
      }
      if ((section == "OriginationCharges" || section == "ServicesBorrowerDidNotShopFor") && row > 13)
      {
        Decimal num = 0M;
        for (int index = 13; index < row; ++index)
        {
          if (this.formControlToDataMap.ContainsKey(section + (object) index + "L"))
          {
            num += Utils.ParseDecimal((object) this.formControlToDataMap[section + (object) index + "L"]);
            if (row == 14)
              break;
          }
        }
        this.formControlToDataMap.Add("LE" + section + "13A1", row == 14 ? this.formControlToDataMap[section + "13A1"] : "Additional Charges");
        this.formControlToDataMap.Add("LE" + section + "13L", num.ToString("N0"));
      }
      else if (section == "ServicesBorrowerDidShopFor" && row > 14)
      {
        Decimal num = 0M;
        for (int index = 14; index < row; ++index)
        {
          if (this.formControlToDataMap.ContainsKey(section + (object) index + "L"))
          {
            num += Utils.ParseDecimal((object) this.formControlToDataMap[section + (object) index + "L"]);
            if (row == 15)
              break;
          }
        }
        this.formControlToDataMap.Add("LE" + section + "14A1", row == 15 ? this.formControlToDataMap[section + "14A1"] : this.getAdditionalText());
        this.formControlToDataMap.Add("LE" + section + "14L", num.ToString("N0"));
      }
      else
      {
        if (!(section == "OtherCosts") || row <= 5)
          return;
        Decimal num = 0M;
        for (int index = 5; index < row; ++index)
        {
          if (this.formControlToDataMap.ContainsKey(section + (object) index + "L"))
          {
            num += Utils.ParseDecimal((object) this.formControlToDataMap[section + (object) index + "L"]);
            if (row == 6)
              break;
          }
        }
        this.formControlToDataMap.Add("LE" + section + "5A1", row == 6 ? this.formControlToDataMap[section + "5A1"] : "Additional Charges");
        this.formControlToDataMap.Add("LE" + section + "5L", num.ToString("N0"));
      }
    }

    private string getAdditionalText()
    {
      XmlNode xmlNode = this.xmlDocument.SelectSingleNode("mismo:MESSAGE/" + this.cdXPath + "mismo:DEAL_SETS/mismo:DEAL_SET/mismo:DEALS/mismo:DEAL/mismo:LOANS/mismo:LOAN/mismo:FEE_INFORMATION/mismo:EXTENSION/EllieMae/ItemizeServicesType", this.nsmgr);
      return xmlNode != null && xmlNode.InnerText == "Y" ? "See attached page for additional items you can shop for" : "Additional Charges";
    }

    public void Buildfees(string xpath, XmlNode XNode, int row, string section)
    {
      string str1 = "";
      XmlNode xmlNode1 = XNode.SelectSingleNode("mismo:FEE_DETAIL/mismo:EXTENSION/EllieMae/LineItemIn2015Itemization", this.nsmgr);
      if (xmlNode1 != null)
        str1 = xmlNode1.InnerText;
      Decimal val = 0M;
      Decimal num1 = 0M;
      XmlNode xmlNode2 = XNode.SelectSingleNode("mismo:FEE_PAYMENTS/mismo:FEE_PAYMENT[mismo:FeePaymentPaidByType='Buyer' and mismo:FeePaymentPaidOutsideOfClosingIndicator='False']/mismo:FeeActualPaymentAmount", this.nsmgr);
      if (xmlNode2 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "B", xmlNode2.InnerText);
        val += Utils.ParseDecimal((object) xmlNode2.InnerText);
        num1 += Utils.ParseDecimal((object) xmlNode2.InnerText);
      }
      XmlNode xmlNode3 = XNode.SelectSingleNode("mismo:FEE_PAYMENTS/mismo:FEE_PAYMENT[mismo:FeePaymentPaidByType='Buyer' and mismo:FeePaymentPaidOutsideOfClosingIndicator='True']/mismo:FeeActualPaymentAmount", this.nsmgr);
      if (xmlNode3 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "C", xmlNode3.InnerText);
        val += Utils.ParseDecimal((object) xmlNode3.InnerText);
        Decimal num2 = num1 + Utils.ParseDecimal((object) xmlNode3.InnerText);
      }
      XmlNode xmlNode4 = XNode.SelectSingleNode("mismo:FEE_PAYMENTS/mismo:FEE_PAYMENT[mismo:FeePaymentPaidByType='Seller' and mismo:FeePaymentPaidOutsideOfClosingIndicator='False']/mismo:FeeActualPaymentAmount", this.nsmgr);
      if (xmlNode4 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "D", xmlNode4.InnerText);
        if (str1 != "701" && str1 != "702" && str1 != "704")
          val += Utils.ParseDecimal((object) xmlNode4.InnerText);
      }
      XmlNode xmlNode5 = XNode.SelectSingleNode("mismo:FEE_PAYMENTS/mismo:FEE_PAYMENT[mismo:FeePaymentPaidByType='Seller' and mismo:FeePaymentPaidOutsideOfClosingIndicator='True']/mismo:FeeActualPaymentAmount", this.nsmgr);
      if (xmlNode5 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "E", xmlNode5.InnerText);
        if (str1 != "701" && str1 != "702" && str1 != "704")
          val += Utils.ParseDecimal((object) xmlNode5.InnerText);
      }
      XmlNodeList xmlNodeList = XNode.SelectNodes("mismo:FEE_PAYMENTS/mismo:FEE_PAYMENT[mismo:FeePaymentPaidByType!='Buyer' and mismo:FeePaymentPaidByType!='Seller']/mismo:FeeActualPaymentAmount", this.nsmgr);
      if (xmlNodeList != null)
      {
        Decimal paidByOthers = 0M;
        foreach (XmlNode xmlNode6 in xmlNodeList)
          paidByOthers += Utils.ParseDecimal((object) xmlNode6.InnerText);
        string str2 = this.addPaidByLenderOnlyFee(section + (object) row, paidByOthers, XNode);
        this.formControlToDataMap.Add(section + (object) row + "F", paidByOthers != 0M ? str2 + paidByOthers.ToString("N2") : "");
        val += paidByOthers;
      }
      if (str1 != "701" && str1 != "702" && str1 != "704")
        val -= this.getSellerObligatedAmount(XNode.SelectSingleNode("mismo:FEE_DETAIL/mismo:EXTENSION/EllieMae/CDPaymentInfo", this.nsmgr));
      if (str1 == "1203")
        val = Utils.ArithmeticRounding(val, 0);
      if (!(val != 0M))
        return;
      this.formControlToDataMap.Add(section + (object) row + "L", Utils.ArithmeticRounding(val, 0).ToString("N0"));
    }

    private string addPaidByLenderOnlyFee(string nodeID, Decimal paidByOthers, XmlNode XNode)
    {
      Decimal num = 0M;
      if (XNode != null)
      {
        XmlElement xmlElement = !nodeID.StartsWith("Prepaids") ? (!nodeID.StartsWith("InitialEscrowPaymentAtClosing") ? (XmlElement) XNode.SelectSingleNode("mismo:FEE_DETAIL/mismo:EXTENSION/EllieMae/CDPaymentInfo", this.nsmgr) : (XmlElement) XNode.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EXTENSION/EllieMae/CDPaymentInfo", this.nsmgr)) : (XmlElement) XNode.SelectSingleNode("mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/CDPaymentInfo", this.nsmgr);
        if (xmlElement != null && xmlElement.HasAttribute("LenderPaidAmount"))
          num = Utils.ParseDecimal((object) xmlElement.GetAttribute("LenderPaidAmount"), 0M);
      }
      this.formControlToDataMap.Add(nodeID + "PaidByLender", !(num != 0M) || !(num == paidByOthers) ? "" : num.ToString("N2"));
      return !(num != 0M) || !(num == paidByOthers) ? "" : "(L) ";
    }

    private Decimal getSellerObligatedAmount(XmlNode feeNode)
    {
      XmlElement xmlElement = (XmlElement) feeNode;
      return xmlElement == null || !xmlElement.HasAttribute("SellerObligatedAmount") ? 0M : Utils.ParseDecimal((object) xmlElement.GetAttribute("SellerObligatedAmount"), 0M);
    }

    public void BuildEscrowCosts(string section)
    {
      int row = 1;
      string str1 = "mismo:MESSAGE/" + this.cdXPath + "mismo:DEAL_SETS/mismo:DEAL_SET/mismo:DEALS/mismo:DEAL/mismo:LOANS/mismo:LOAN/mismo:ESCROW/mismo:ESCROW_ITEMS/mismo:ESCROW_ITEM[mismo:ESCROW_ITEM_DETAIL";
      if (section == "InitialEscrowPaymentAtClosing")
      {
        foreach (string str2 in new List<string>()
        {
          "HomeownersInsurance",
          "MI"
        })
        {
          string xpath = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:ESCROW_ITEM_DETAIL/mismo:EscrowItemCategoryType='" + str2 + "']";
          XmlNode xmlNode1 = this.xmlDocument.SelectSingleNode(xpath + "/mismo:ESCROW_ITEM_DETAIL/mismo:EscrowMonthlyPaymentAmount", this.nsmgr);
          if (xmlNode1 != null)
            this.formControlToDataMap.Add(section + (object) row + "A1", xmlNode1.InnerText);
          XmlNode xmlNode2 = this.xmlDocument.SelectSingleNode(xpath + "/mismo:ESCROW_ITEM_DETAIL/mismo:EscrowCollectedNumberOfMonthsCount", this.nsmgr);
          if (xmlNode2 != null)
            this.formControlToDataMap.Add(section + (object) row + "A2", xmlNode2.InnerText);
          foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath, this.nsmgr))
            this.BuildEscrowfees(xpath, selectNode, row, section);
          this.addCDLineInfo(this.xmlDocument.SelectSingleNode(xpath, this.nsmgr), section, row);
          ++row;
        }
        string xpath1 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:ESCROW_ITEM_DETAIL/mismo:EscrowItemCategoryType='PropertyTaxes']";
        XmlNodeList source = this.xmlDocument.SelectNodes(xpath1, this.nsmgr);
        if (source != null)
        {
          foreach (XmlNode xmlNode3 in source.Cast<XmlNode>().Where<XmlNode>((Func<XmlNode, bool>) (np => np != null)))
          {
            XmlNode xmlNode4 = xmlNode3.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EscrowMonthlyPaymentAmount", this.nsmgr);
            if (xmlNode4 != null)
              this.formControlToDataMap.Add(section + (object) row + "A1", xmlNode4.InnerText);
            XmlNode xmlNode5 = xmlNode3.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EscrowCollectedNumberOfMonthsCount", this.nsmgr);
            if (xmlNode5 != null)
              this.formControlToDataMap.Add(section + (object) row + "A2", xmlNode5.InnerText);
            this.addCDLineInfo(xmlNode3, section, row);
            xmlNode3.SelectNodes(xpath1, this.nsmgr);
            this.BuildEscrowfees(xpath1, xmlNode3, row, section);
          }
        }
        ++row;
        XmlNode xmlNode = this.xmlDocument.SelectSingleNode("mismo:MESSAGE/" + this.cdXPath + "mismo:DEAL_SETS/mismo:DEAL_SET/mismo:DEALS/mismo:DEAL/mismo:LOANS/mismo:LOAN/mismo:ESCROW/mismo:ESCROW_DETAIL/mismo:EscrowAggregateAccountingAdjustmentAmount", this.nsmgr);
        if (xmlNode != null)
          this.formControlToDataMap.Add(section + (object) 8 + "B", xmlNode.InnerText);
      }
      string xpath2 = "";
      if (section == "InitialEscrowPaymentAtClosing")
        xpath2 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:ESCROW_ITEM_DETAIL/mismo:EscrowItemCategoryType != 'HomeownersInsurance' and  mismo:ESCROW_ITEM_DETAIL/mismo:EscrowItemCategoryType != 'MI' and  mismo:ESCROW_ITEM_DETAIL/mismo:EscrowItemCategoryType != 'PropertyTaxes']";
      if (row == 8)
        ++row;
      foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath2, this.nsmgr))
      {
        if (row == 8)
          ++row;
        string str3;
        string str4 = str3 = "";
        string str5 = str3;
        string str6 = str3;
        XmlNode xmlNode6 = selectNode.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EscrowItemCategoryType", this.nsmgr);
        if (xmlNode6 != null)
          str6 = xmlNode6.InnerText;
        XmlNode xmlNode7 = selectNode.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EscrowMonthlyPaymentAmount", this.nsmgr);
        if (xmlNode7 != null)
          str5 = xmlNode7.InnerText;
        XmlNode xmlNode8 = selectNode.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EscrowCollectedNumberOfMonthsCount", this.nsmgr);
        if (xmlNode8 != null)
          str4 = xmlNode8.InnerText;
        if (!this.ShouldAppendTitle("mismo:ESCROW_ITEM_DETAIL", selectNode, -1, ""))
          this.formControlToDataMap.Add(section + (object) row + "A1", str6);
        else
          this.formControlToDataMap.Add(section + (object) row + "A1", "Title - " + str6);
        this.formControlToDataMap.Add(section + (object) row + "A2", str5);
        this.formControlToDataMap.Add(section + (object) row + "A3", str4);
        this.addCDLineInfo(selectNode, section, row);
        if (row == 9)
        {
          this.formControlToDataMap.Add("LE" + section + "8A1", str6);
          this.formControlToDataMap.Add("LE" + section + "8A2", str5);
          this.formControlToDataMap.Add("LE" + section + "8A3", str4);
        }
        this.BuildEscrowfees(xpath2, selectNode, row, section);
        ++row;
      }
      if (!(section == "InitialEscrowPaymentAtClosing"))
        return;
      this.formControlToDataMap.Add("1011", "G." + (row >= 9 ? "14" : "08"));
    }

    public void BuildEscrowfees(string xpath, XmlNode XNode, int row, string section)
    {
      Decimal num = 0M;
      XmlNode xmlNode1 = XNode.SelectSingleNode("mismo:ESCROW_ITEM_PAYMENTS/mismo:ESCROW_ITEM_PAYMENT[mismo:EscrowItemPaymentPaidByType='Buyer' ]/mismo:EscrowItemActualPaymentAmount", this.nsmgr);
      if (xmlNode1 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "B", xmlNode1.InnerText);
        num += Utils.ParseDecimal((object) xmlNode1.InnerText);
      }
      XmlNode xmlNode2 = XNode.SelectSingleNode("mismo:ESCROW_ITEM_PAYMENTS/mismo:ESCROW_ITEM_PAYMENT[mismo:EscrowItemPaymentPaidByType='Seller']/mismo:EscrowItemActualPaymentAmount", this.nsmgr);
      if (xmlNode2 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "D", xmlNode2.InnerText);
        num += Utils.ParseDecimal((object) xmlNode2.InnerText);
      }
      XmlNodeList xmlNodeList = XNode.SelectNodes("mismo:ESCROW_ITEM_PAYMENTS/mismo:ESCROW_ITEM_PAYMENT[mismo:EscrowItemPaymentPaidByType!='Buyer' and mismo:EscrowItemPaymentPaidByType!='Seller']/mismo:EscrowItemActualPaymentAmount", this.nsmgr);
      if (xmlNodeList != null)
      {
        Decimal paidByOthers = 0M;
        foreach (XmlNode xmlNode3 in xmlNodeList)
          paidByOthers += Utils.ParseDecimal((object) xmlNode3.InnerText);
        string str = this.addPaidByLenderOnlyFee(section + (object) row, paidByOthers, XNode);
        this.formControlToDataMap.Add(section + (object) row + "F", paidByOthers != 0M ? str + paidByOthers.ToString("N2") : "");
        num += paidByOthers;
      }
      Decimal val = num - this.getSellerObligatedAmount(XNode.SelectSingleNode("mismo:ESCROW_ITEM_DETAIL/mismo:EXTENSION/EllieMae/CDPaymentInfo", this.nsmgr));
      if (!(val != 0M))
        return;
      if (row == 9)
        this.formControlToDataMap.Add("LE" + section + "8L", Utils.ArithmeticRounding(val, 0).ToString("N0"));
      else
        this.formControlToDataMap.Add(section + (object) row + "L", Utils.ArithmeticRounding(val, 0).ToString("N0"));
    }

    public void BuildPrepaidCosts(string section)
    {
      int row1 = 1;
      string str1 = "mismo:MESSAGE/" + this.cdXPath + "mismo:DEAL_SETS/mismo:DEAL_SET/mismo:DEALS/mismo:DEAL/mismo:LOANS/mismo:LOAN/mismo:CLOSING_INFORMATION/mismo:PREPAID_ITEMS/mismo:PREPAID_ITEM[mismo:PREPAID_ITEM_DETAIL";
      if (section == "Prepaids")
      {
        string xpath1 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType='HomeownersInsurancePremium']";
        XmlNode xmlNode1 = this.xmlDocument.SelectSingleNode(xpath1 + "/mismo:PREPAID_ITEM_PAID_TO/mismo:LEGAL_ENTITY/mismo:LEGAL_ENTITY_DETAIL/mismo:FullName", this.nsmgr);
        if (xmlNode1 != null)
          this.formControlToDataMap.Add(section + (object) row1 + "A1", xmlNode1.InnerText);
        XmlNode xmlNode2 = this.xmlDocument.SelectSingleNode(xpath1 + "/mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemMonthsPaidCount", this.nsmgr);
        if (xmlNode2 != null)
          this.formControlToDataMap.Add(section + (object) row1 + "A2", xmlNode2.InnerText);
        XmlNode xmlNode3 = this.xmlDocument.SelectSingleNode(xpath1 + "/mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/PrepaidItemMonthlyPaymentAmount", this.nsmgr);
        if (xmlNode3 != null)
          this.formControlToDataMap.Add(section + (object) row1 + "A3", xmlNode3.InnerText);
        this.addCDLineInfo(this.xmlDocument.SelectSingleNode(xpath1, this.nsmgr), section, row1);
        foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath1, this.nsmgr))
          this.BuildPrePaidfees(xpath1, selectNode, row1, section);
        int row2 = row1 + 1;
        string xpath2 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType='MortgageInsurancePremium']";
        XmlNode xmlNode4 = this.xmlDocument.SelectSingleNode(xpath2 + "/mismo:PREPAID_ITEM_PAID_TO/mismo:LEGAL_ENTITY/mismo:LEGAL_ENTITY_DETAIL/mismo:FullName", this.nsmgr);
        if (xmlNode4 != null)
          this.formControlToDataMap.Add(section + (object) row2 + "A1", xmlNode4.InnerText);
        XmlNode xmlNode5 = this.xmlDocument.SelectSingleNode(xpath2 + "/mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemMonthsPaidCount", this.nsmgr);
        if (xmlNode5 != null)
          this.formControlToDataMap.Add(section + (object) row2 + "A2", xmlNode5.InnerText);
        XmlNode xmlNode6 = this.xmlDocument.SelectSingleNode(xpath2 + "/mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/PrepaidItemMonthlyPaymentAmount", this.nsmgr);
        if (xmlNode6 != null)
          this.formControlToDataMap.Add(section + (object) row2 + "A3", xmlNode6.InnerText);
        this.addCDLineInfo(this.xmlDocument.SelectSingleNode(xpath2, this.nsmgr), section, row2);
        foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath2, this.nsmgr))
          this.BuildPrePaidfees(xpath2, selectNode, row2, section);
        int row3 = row2 + 1;
        string xpath3 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType='PrepaidInterest']";
        XmlNode xmlNode7 = this.xmlDocument.SelectSingleNode(xpath3 + "/mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemPerDiemAmount", this.nsmgr);
        if (xmlNode7 != null)
          this.formControlToDataMap.Add(section + (object) row3 + "A1", Utils.Remove2EndingZeros(xmlNode7.InnerText, false));
        XmlNode xmlNode8 = this.xmlDocument.SelectSingleNode(xpath3 + "/mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemPaidFromDate", this.nsmgr);
        DateTime dateTime;
        if (xmlNode8 != null)
        {
          Dictionary<string, string> controlToDataMap = this.formControlToDataMap;
          string key = section + (object) row3 + "A2";
          dateTime = Convert.ToDateTime(xmlNode8.InnerText);
          string str2 = dateTime.ToString("MM/dd/yyyy");
          controlToDataMap.Add(key, str2);
        }
        XmlNode xmlNode9 = this.xmlDocument.SelectSingleNode(xpath3 + "/mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemPaidThroughDate", this.nsmgr);
        if (xmlNode9 != null)
        {
          Dictionary<string, string> controlToDataMap = this.formControlToDataMap;
          string key = section + (object) row3 + "A3";
          dateTime = Convert.ToDateTime(xmlNode9.InnerText);
          string str3 = dateTime.ToString("MM/dd/yyyy");
          controlToDataMap.Add(key, str3);
        }
        XmlNode ellieNode = this.xmlDocument.SelectSingleNode(xpath3 + "/mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/LEInformation", this.nsmgr);
        if (ellieNode != null)
        {
          this.formControlToDataMap.Add(section + (object) row3 + "A4", this.readEllieMaeExtension(ellieNode, "PrepaidInterestDays"));
          this.formControlToDataMap.Add(section + (object) row3 + "A5", Utils.RemoveEndingZeros(Utils.FormatLEAndCDPercentageValue(this.readEllieMaeExtension(ellieNode, "PrepaidInterestRate"))));
        }
        this.addCDLineInfo(this.xmlDocument.SelectSingleNode(xpath3, this.nsmgr), section, row3);
        foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath3, this.nsmgr))
          this.BuildPrePaidfees(xpath3, selectNode, row3, section);
        int row4 = row3 + 1;
        string xpath4 = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType='CountyPropertyTax']";
        XmlNode xmlNode10 = this.xmlDocument.SelectSingleNode(xpath4 + "/mismo:PREPAID_ITEM_PAID_TO/mismo:LEGAL_ENTITY/mismo:LEGAL_ENTITY_DETAIL/mismo:FullName", this.nsmgr);
        if (xmlNode10 != null)
          this.formControlToDataMap.Add(section + (object) row4 + "A1", xmlNode10.InnerText);
        XmlNode xmlNode11 = this.xmlDocument.SelectSingleNode(xpath4 + "/mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemMonthsPaidCount", this.nsmgr);
        if (xmlNode11 != null)
          this.formControlToDataMap.Add(section + (object) row4 + "A2", xmlNode11.InnerText);
        XmlNode xmlNode12 = this.xmlDocument.SelectSingleNode(xpath4 + "/mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/PrepaidItemMonthlyPaymentAmount", this.nsmgr);
        if (xmlNode12 != null)
          this.formControlToDataMap.Add(section + (object) row4 + "A3", xmlNode12.InnerText);
        this.addCDLineInfo(this.xmlDocument.SelectSingleNode(xpath4, this.nsmgr), section, row4);
        foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath4, this.nsmgr))
          this.BuildPrePaidfees(xpath4, selectNode, row4, section);
        row1 = row4 + 1;
      }
      string xpath = "";
      if (section == "Prepaids")
        xpath = str1 + "/mismo:IntegratedDisclosureSectionType='" + section + "' and mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType!='HomeownersInsurancePremium' and  mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType!='MortgageInsurancePremium' and  mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType!='PrepaidInterest'  and  mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType!='CountyPropertyTax']";
      foreach (XmlNode selectNode in this.xmlDocument.SelectNodes(xpath, this.nsmgr))
      {
        XmlNode xmlNode13 = selectNode.SelectSingleNode("mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemType", this.nsmgr);
        string str4 = "";
        string str5 = "";
        string str6 = "";
        string str7 = "";
        if (xmlNode13 != null)
          str4 = xmlNode13.InnerText;
        XmlNode xmlNode14 = selectNode.SelectSingleNode("mismo:PREPAID_ITEM_PAID_TO/mismo:LEGAL_ENTITY/mismo:LEGAL_ENTITY_DETAIL/mismo:FullName", this.nsmgr);
        if (xmlNode14 != null)
          str6 = xmlNode14.InnerText;
        XmlNode xmlNode15 = selectNode.SelectSingleNode("mismo:PREPAID_ITEM_DETAIL/mismo:PrepaidItemMonthsPaidCount", this.nsmgr);
        if (xmlNode15 != null)
          str5 = xmlNode15.InnerText;
        XmlNode xmlNode16 = selectNode.SelectSingleNode("mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/PrepaidItemMonthlyPaymentAmount", this.nsmgr);
        if (xmlNode16 != null)
          str7 = xmlNode16.InnerText;
        if (!this.ShouldAppendTitle("mismo:PREPAID_ITEM_DETAIL", selectNode, -1, ""))
          this.formControlToDataMap.Add(section + (object) row1 + "A1", str4);
        else
          this.formControlToDataMap.Add(section + (object) row1 + "A1", "Title - " + str4);
        this.formControlToDataMap.Add(section + (object) row1 + "A2", str6);
        this.formControlToDataMap.Add(section + (object) row1 + "A3", str5);
        this.formControlToDataMap.Add(section + (object) row1 + "A4", str7);
        this.addCDLineInfo(selectNode, section, row1);
        this.BuildPrePaidfees(xpath, selectNode, row1, section);
        ++row1;
      }
      if (row1 <= 7)
        return;
      Decimal num = 0M;
      for (int index = 7; index < row1; ++index)
      {
        if (this.formControlToDataMap.ContainsKey(section + (object) index + "L"))
        {
          num += Utils.ParseDecimal((object) this.formControlToDataMap[section + (object) index + "L"]);
          if (row1 == 8)
            break;
        }
      }
      this.formControlToDataMap.Add("LE" + section + "7A1", row1 == 8 ? this.formControlToDataMap[section + "7A1"] : "Additional Charges");
      this.formControlToDataMap.Add("LE" + section + "7A3", row1 == 8 ? this.formControlToDataMap[section + "7A3"] : "");
      this.formControlToDataMap.Add("LE" + section + "7L", num.ToString("N0"));
    }

    public void BuildPrePaidfees(string xpath, XmlNode XNode, int row, string section)
    {
      Decimal num = 0M;
      XmlNode xmlNode1 = XNode.SelectSingleNode("mismo:PREPAID_ITEM_PAYMENTS/mismo:PREPAID_ITEM_PAYMENT[mismo:PrepaidItemPaymentPaidByType='Buyer' and mismo:PrepaidItemPaymentTimingType='AtClosing' ]/mismo:PrepaidItemActualPaymentAmount", this.nsmgr);
      if (xmlNode1 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "B", xmlNode1.InnerText);
        num += Utils.ParseDecimal((object) xmlNode1.InnerText);
      }
      XmlNode xmlNode2 = XNode.SelectSingleNode("mismo:PREPAID_ITEM_PAYMENTS/mismo:PREPAID_ITEM_PAYMENT[mismo:PrepaidItemPaymentPaidByType='Buyer' and mismo:PrepaidItemPaymentTimingType='BeforeClosing']/mismo:PrepaidItemActualPaymentAmount", this.nsmgr);
      if (xmlNode2 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "C", xmlNode2.InnerText);
        num += Utils.ParseDecimal((object) xmlNode2.InnerText);
      }
      XmlNode xmlNode3 = XNode.SelectSingleNode("mismo:PREPAID_ITEM_PAYMENTS/mismo:PREPAID_ITEM_PAYMENT[mismo:PrepaidItemPaymentPaidByType='Seller' and mismo:PrepaidItemPaymentTimingType='AtClosing' ]/mismo:PrepaidItemActualPaymentAmount", this.nsmgr);
      if (xmlNode3 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "D", xmlNode3.InnerText);
        num += Utils.ParseDecimal((object) xmlNode3.InnerText);
      }
      XmlNode xmlNode4 = XNode.SelectSingleNode("mismo:PREPAID_ITEM_PAYMENTS/mismo:PREPAID_ITEM_PAYMENT[mismo:PrepaidItemPaymentPaidByType='Seller' and mismo:PrepaidItemPaymentTimingType='BeforeClosing' ]/mismo:PrepaidItemActualPaymentAmount", this.nsmgr);
      if (xmlNode4 != null)
      {
        this.formControlToDataMap.Add(section + (object) row + "E", xmlNode4.InnerText);
        num += Utils.ParseDecimal((object) xmlNode4.InnerText);
      }
      XmlNodeList xmlNodeList = XNode.SelectNodes("mismo:PREPAID_ITEM_PAYMENTS/mismo:PREPAID_ITEM_PAYMENT[mismo:PrepaidItemPaymentPaidByType!='Buyer' and mismo:PrepaidItemPaymentPaidByType!='Seller']/mismo:PrepaidItemActualPaymentAmount", this.nsmgr);
      if (xmlNodeList != null)
      {
        Decimal paidByOthers = 0M;
        foreach (XmlNode xmlNode5 in xmlNodeList)
          paidByOthers += Utils.ParseDecimal((object) xmlNode5.InnerText);
        string str = this.addPaidByLenderOnlyFee(section + (object) row, paidByOthers, XNode);
        this.formControlToDataMap.Add(section + (object) row + "F", paidByOthers != 0M ? str + paidByOthers.ToString("N2") : "");
        num += paidByOthers;
      }
      Decimal val = num - this.getSellerObligatedAmount(XNode.SelectSingleNode("mismo:PREPAID_ITEM_DETAIL/mismo:EXTENSION/EllieMae/CDPaymentInfo", this.nsmgr));
      if (!(val != 0M))
        return;
      this.formControlToDataMap.Add(section + (object) row + "L", Utils.ArithmeticRounding(val, 0).ToString("N0"));
    }

    private void addCDLineInfo(XmlNode parentNode, string section, int row)
    {
      if (parentNode == null)
        return;
      XmlNode xmlNode1 = parentNode;
      string str1;
      switch (section)
      {
        case "Prepaids":
          str1 = "mismo:PREPAID_ITEM_DETAIL";
          break;
        case "InitialEscrowPaymentAtClosing":
          str1 = "mismo:ESCROW_ITEM_DETAIL";
          break;
        default:
          str1 = "mismo:FEE_DETAIL";
          break;
      }
      string xpath = str1 + "/mismo:EXTENSION/EllieMae/LineItemIn2015Itemization";
      XmlNamespaceManager nsmgr = this.nsmgr;
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode(xpath, nsmgr);
      if (xmlNode2 == null || this.formControlToDataMap.ContainsKey("LINE:" + xmlNode2.InnerText))
        return;
      string empty = string.Empty;
      string str2;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(section))
      {
        case 419034766:
          if (!(section == "InitialEscrowPaymentAtClosing"))
            return;
          str2 = "G";
          break;
        case 1557511805:
          if (!(section == "Prepaids"))
            return;
          str2 = "F";
          break;
        case 1749817759:
          if (!(section == "ServicesBorrowerDidShopFor"))
            return;
          str2 = "C";
          break;
        case 2077386153:
          if (!(section == "OriginationCharges"))
            return;
          str2 = "A";
          break;
        case 2272558785:
          if (!(section == "OtherCosts"))
            return;
          str2 = "H";
          break;
        case 3063389092:
          if (!(section == "ServicesBorrowerDidNotShopFor"))
            return;
          str2 = "B";
          break;
        case 4010153161:
          if (!(section == "TaxesAndOtherGovernmentFees"))
            return;
          str2 = "E";
          break;
        default:
          return;
      }
      if (str2 == "G" && row > 8)
        this.formControlToDataMap.Add("LINE:" + xmlNode2.InnerText, str2 + "." + (row - 1).ToString("00"));
      else if (str2 == "G" && xmlNode2.InnerText == "1011")
        this.formControlToDataMap.Add("LINE:" + xmlNode2.InnerText, str2 + ".14");
      else
        this.formControlToDataMap.Add("LINE:" + xmlNode2.InnerText, str2 + "." + row.ToString("00"));
    }

    public bool ShouldAppendTitle(string xpath, XmlNode XNode, int row, string section) => false;

    private string readEllieMaeExtension(XmlNode ellieNode, string attributeID)
    {
      XmlElement xmlElement = (XmlElement) ellieNode;
      return xmlElement != null && xmlElement.HasAttribute(attributeID) ? xmlElement.GetAttribute(attributeID) : string.Empty;
    }
  }
}
