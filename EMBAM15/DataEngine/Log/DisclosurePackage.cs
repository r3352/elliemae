// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DisclosurePackage
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DisclosurePackage
  {
    private string packageGuid = "";
    private string eSignatures = "";
    private string fulfillment = "";
    private string fulfillmentOrderedBy = "";
    private string fulfillmentProcessedDate = "";
    private string name_Borrower = "";
    private string email_Borrower = "";
    private string authenticatedDate_Borrower = "";
    private string authenticatedIP_Borrower = "";
    private string viewedDate_Borrower = "";
    private string consentAcceptedDate_Borrower = "";
    private string consentRejectedDate_Borrower = "";
    private string consentIP_Borrower = "";
    private string eSignedDate_Borrower = "";
    private string eSignedIP_Borrower = "";
    private string name_CoBorrower = "";
    private string email_CoBorrower = "";
    private string authenticatedDate_CoBorrower = "";
    private string authenticatedIP_CoBorrower = "";
    private string viewedDate_CoBorrower = "";
    private string consentAcceptedDate_CoBorrower = "";
    private string consentRejectedDate_CoBorrower = "";
    private string consentIP_CoBorrower = "";
    private string eSignedDate_CoBorrower = "";
    private string eSignedIP_CoBorrower = "";
    private string name_LO = "";
    private string id_LO = "";
    private string viewedDate_LO = "";
    private string eSignedDate_LO = "";
    private string eSignedIP_LO = "";
    private string packageCreatedDate = "";
    private string packageExpiryNotificationDate = "";
    private string fulfillmentScheduledDate = "";
    private string fulfillmentOrderedBy_CoBorrower = "";
    private string fulfillmentProcessedDate_CoBorrower = "";
    private string fulfillmentScheduledDate_CoBorrower = "";
    private string documentViewedDate_Borrower = "";
    private string documentViewedDate_CoBorrower = "";
    private string eDisclosureBorrowerLoanLevelConsent = "";
    private string eDisclosureCoBorrowerLoanLevelConsent = "";
    private string eDisclosureBorrowerPackageLevelConsent = "";
    private string eDisclosureCoBorrowerPackageLevelConsent = "";
    private const string className = "DisclosurePackage";
    private static readonly string sw = Tracing.SwEFolder;
    private bool containCoBorrower;
    private bool containLO;
    private string consentPDF = "";
    private Dictionary<string, global::NBODetail> NBOitems = new Dictionary<string, global::NBODetail>((IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    private System.TimeZoneInfo timeZoneInfo;

    public DisclosurePackage(
      XmlNode disclosurePackageNode,
      Dictionary<string, bool> guidToWetSignFlag,
      System.TimeZoneInfo timeZoneInfo)
    {
      this.timeZoneInfo = timeZoneInfo;
      this.packageGuid = DisclosurePackage.getElementValue(disclosurePackageNode, "@packageguid");
      this.eSignatures = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/@esignatures");
      this.fulfillment = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment/@packageid");
      this.packageExpiryNotificationDate = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/@notificationdate");
      this.consentPDF = DisclosurePackage.getElementValue(disclosurePackageNode, "consentdata/@consentdata");
      this.packageCreatedDate = DisclosurePackage.getElementValue(disclosurePackageNode, "@datecreated");
      XmlNodeList nodeList = disclosurePackageNode.SelectNodes("packagetypedisclosure/userdetails[@encompassContactGuid]");
      XmlNodeList xmlNodeList = disclosurePackageNode.SelectNodes("packagetypedisclosure/userdetails[not(@encompassContactGuid)]");
      int count1 = nodeList != null ? nodeList.Count : 0;
      int count2 = xmlNodeList != null ? xmlNodeList.Count : 0;
      bool flag = guidToWetSignFlag.ContainsKey(this.packageGuid.ToLower()) && guidToWetSignFlag[this.packageGuid.ToLower()];
      bool markedForeSignatures = this.MarkedForeSignatures;
      if (!flag && !this.MarkedForeSignatures)
        flag = true;
      try
      {
        if (!flag)
        {
          this.documentViewedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@DocViewed");
          this.eDisclosureBorrowerLoanLevelConsent = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@loanConsentStatusAtPackageSend");
          this.containCoBorrower = !(DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@usertype") == "");
          if (this.containCoBorrower)
          {
            this.documentViewedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@DocViewed");
            this.eDisclosureCoBorrowerLoanLevelConsent = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@loanConsentStatusAtPackageSend");
          }
        }
        else if (count2 > 0)
        {
          this.documentViewedDate_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@DocViewed");
          this.eDisclosureBorrowerLoanLevelConsent = DisclosurePackage.getElementValue(xmlNodeList[0], "@loanConsentStatusAtPackageSend");
          this.containCoBorrower = count2 > 1;
          if (this.containCoBorrower)
          {
            this.documentViewedDate_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@DocViewed");
            this.eDisclosureCoBorrowerLoanLevelConsent = DisclosurePackage.getElementValue(xmlNodeList[1], "@loanConsentStatusAtPackageSend");
          }
        }
      }
      catch
      {
      }
      if (this.MarkedForFulfillment)
      {
        this.fulfillmentOrderedBy = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment[@intendedfor='B']/@fromname");
        if (this.fulfillmentOrderedBy != string.Empty)
        {
          this.fulfillmentProcessedDate = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment[@intendedfor='B']/@processeddate");
          this.fulfillmentScheduledDate = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment[@intendedfor='B']/@scheduleddate");
          this.fulfillmentOrderedBy_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment[@intendedfor='B']/@fromname");
          if (this.fulfillmentOrderedBy_CoBorrower != string.Empty)
          {
            this.fulfillmentProcessedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment[@intendedfor='B']/@processeddate");
            this.fulfillmentScheduledDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment[@intendedfor='B']/@scheduleddate");
          }
        }
        else
        {
          this.fulfillmentOrderedBy = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment/@fromname");
          this.fulfillmentProcessedDate = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment/@processeddate");
          this.fulfillmentScheduledDate = DisclosurePackage.getElementValue(disclosurePackageNode, "fulfillment/@scheduleddate");
        }
      }
      if (!flag)
      {
        this.name_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@signername");
        this.email_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@signerEmail");
        this.authenticatedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@authenticatedDate");
        this.authenticatedIP_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@authenticatedip");
        this.viewedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@viewed");
        this.consentAcceptedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@actualDeliveryAccepted");
        this.consentRejectedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@deliveryrejected");
        this.consentIP_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@consentip");
        this.eSignedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@esigncomplete");
        this.eSignedIP_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@esignip");
        this.containCoBorrower = !(DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@usertype") == "");
        this.containLO = !(DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='O']/@usertype") == "");
        if (this.containCoBorrower)
        {
          this.name_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@signername");
          this.email_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@signerEmail");
          this.authenticatedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@authenticatedDate");
          this.authenticatedIP_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@authenticatedip");
          this.viewedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@viewed");
          this.consentAcceptedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@actualDeliveryAccepted");
          this.consentRejectedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@deliveryrejected");
          this.consentIP_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@consentip");
          this.eSignedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@esigncomplete");
          this.eSignedIP_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@esignip");
        }
        if (this.containLO)
        {
          this.name_LO = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='O']/@signername");
          this.viewedDate_LO = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='O']/@viewed");
          this.eSignedDate_LO = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='O']/@esigncomplete");
          this.eSignedIP_LO = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='O']/@esignip");
        }
      }
      else
      {
        this.name_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@signername");
        if (this.name_Borrower != string.Empty)
        {
          this.email_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@signerEmail");
          this.viewedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@viewed");
          this.consentAcceptedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@actualDeliveryAccepted");
          this.consentRejectedDate_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@deliveryrejected");
          this.consentIP_Borrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='B']/@consentip");
          this.containCoBorrower = !(DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@signername") == "");
          if (this.containCoBorrower)
          {
            this.name_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@signername");
            this.email_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@signerEmail");
            this.viewedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@viewed");
            this.consentAcceptedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@actualDeliveryAccepted");
            this.consentRejectedDate_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@deliveryrejected");
            this.consentIP_CoBorrower = DisclosurePackage.getElementValue(disclosurePackageNode, "packagetypedisclosure/userdetails[@usertype='C']/@consentip");
          }
        }
        else
        {
          if (count2 <= 0)
            return;
          this.name_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@signername");
          this.email_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@signerEmail");
          this.viewedDate_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@viewed");
          this.consentAcceptedDate_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@actualDeliveryAccepted");
          this.consentRejectedDate_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@deliveryrejected");
          this.consentIP_Borrower = DisclosurePackage.getElementValue(xmlNodeList[0], "@consentip");
          this.containCoBorrower = count2 > 1;
          if (this.containCoBorrower)
          {
            this.name_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@signername");
            this.email_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@signerEmail");
            this.viewedDate_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@viewed");
            this.consentAcceptedDate_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@actualDeliveryAccepted");
            this.consentRejectedDate_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@deliveryrejected");
            this.consentIP_CoBorrower = DisclosurePackage.getElementValue(xmlNodeList[1], "@consentip");
          }
        }
      }
      if (count1 <= 0)
        return;
      this.UpdateNBODetails(nodeList);
    }

    public DisclosurePackage()
    {
    }

    private void UpdateNBODetails(XmlNodeList nodeList)
    {
      foreach (XmlNode node in nodeList)
      {
        string elementValue1 = DisclosurePackage.getElementValue(node, "@encompassContactGuid");
        string elementValue2 = DisclosurePackage.getElementValue(node, "@DocViewed");
        string elementValue3 = DisclosurePackage.getElementValue(node, "@loanConsentStatusAtPackageSend");
        string elementValue4 = DisclosurePackage.getElementValue(node, "@signername");
        string elementValue5 = DisclosurePackage.getElementValue(node, "@signerEmail");
        string elementValue6 = DisclosurePackage.getElementValue(node, "@authenticatedDate");
        string elementValue7 = DisclosurePackage.getElementValue(node, "@authenticatedip");
        string elementValue8 = DisclosurePackage.getElementValue(node, "@viewed");
        string elementValue9 = DisclosurePackage.getElementValue(node, "@actualDeliveryAccepted");
        string elementValue10 = DisclosurePackage.getElementValue(node, "@deliveryrejected");
        string elementValue11 = DisclosurePackage.getElementValue(node, "@consentip");
        string elementValue12 = DisclosurePackage.getElementValue(node, "@esigncomplete");
        string elementValue13 = DisclosurePackage.getElementValue(node, "@esignip");
        DisclosurePackage.getElementValue(node, "@loanConsentDateAtPackageSend");
        global::NBODetail nboDetail = new global::NBODetail();
        nboDetail.authenticatedDate = this.ParseDate(elementValue6);
        nboDetail.authenticatedIP = elementValue7;
        nboDetail.consentAcceptedDate = this.ParseDate(elementValue9);
        nboDetail.consentIP = elementValue11;
        nboDetail.consentRejectedDate = this.ParseDate(elementValue10);
        nboDetail.documentViewedDate = this.ParseDate(elementValue2);
        nboDetail.email = elementValue5;
        nboDetail.eSignatures = this.eSignatures;
        nboDetail.eSignedDate = this.ParseDate(elementValue12);
        nboDetail.eSignedIP = elementValue13;
        nboDetail.name = elementValue4;
        nboDetail.viewedDate = this.ParseDate(elementValue8);
        if (elementValue3 == "1")
          nboDetail.loanLevelConsent = "Accepted";
        if (elementValue3 == "0")
          nboDetail.loanLevelConsent = "Declined";
        if (string.IsNullOrEmpty(elementValue3))
          nboDetail.loanLevelConsent = "";
        this.NBOitems.Add(elementValue1, nboDetail);
      }
    }

    public void SetPackageLevelFields(
      string packageGuid,
      string packageExpiryNotificationDate,
      string packageCreationDate,
      string fulfillmentOrderBy,
      string fulfillmentProcessedDate,
      string fulfillmentScheduledDate,
      string consentPDF)
    {
      if (!string.IsNullOrEmpty(packageGuid))
        this.packageGuid = packageGuid;
      if (!string.IsNullOrEmpty(packageExpiryNotificationDate))
        this.packageExpiryNotificationDate = packageExpiryNotificationDate;
      if (!string.IsNullOrEmpty(packageCreationDate))
        this.packageCreatedDate = packageCreationDate;
      if (!string.IsNullOrEmpty(fulfillmentOrderBy))
        this.fulfillmentOrderedBy = fulfillmentOrderBy;
      if (!string.IsNullOrEmpty(fulfillmentProcessedDate))
        this.fulfillmentProcessedDate = fulfillmentProcessedDate;
      if (!string.IsNullOrEmpty(fulfillmentScheduledDate))
        this.fulfillmentScheduledDate = fulfillmentScheduledDate;
      if (string.IsNullOrEmpty(consentPDF))
        return;
      this.consentPDF = consentPDF;
    }

    public void ToggleMakedForeSignatures(string toggle)
    {
      this.eSignatures = string.IsNullOrEmpty(toggle) || !(toggle != "1") ? toggle : throw new Exception("The toggle can be \"\" or \"1\". No other value is allowed.");
    }

    public void ToggleMakedForNBOeSignatures(string nboNameEmail, string toggle)
    {
      if (!string.IsNullOrEmpty(toggle) && toggle != "1")
        throw new Exception("The toggle can be \"\" or \"1\". No other value is allowed.");
      if (!this.NBOitems.ContainsKey(nboNameEmail))
        this.NBOitems.Add(nboNameEmail, new global::NBODetail());
      this.NBOitems[nboNameEmail].eSignatures = toggle;
    }

    public void SetBorrowerFields(
      string nameBorrower,
      string emailBorrower,
      string authenticatedDateBorrower = null,
      string authenticatedIPBorrower = null,
      string viewedDateBorrower = null,
      string consentAccpetedDateBorrower = null,
      string consentRejectedDateBorrower = null,
      string consentIPBorrower = null,
      string eSignedDateBorrower = null,
      string eSignedIPBorrower = null)
    {
      if (!string.IsNullOrEmpty(nameBorrower))
        this.name_Borrower = nameBorrower;
      if (!string.IsNullOrEmpty(emailBorrower))
        this.email_Borrower = emailBorrower;
      if (!string.IsNullOrEmpty(authenticatedDateBorrower))
        this.authenticatedDate_Borrower = authenticatedDateBorrower;
      if (!string.IsNullOrEmpty(authenticatedIPBorrower))
        this.authenticatedIP_Borrower = authenticatedIPBorrower;
      if (!string.IsNullOrEmpty(viewedDateBorrower))
        this.viewedDate_Borrower = viewedDateBorrower;
      if (!string.IsNullOrEmpty(consentAccpetedDateBorrower))
        this.consentAcceptedDate_Borrower = consentAccpetedDateBorrower;
      if (!string.IsNullOrEmpty(consentRejectedDateBorrower))
        this.consentRejectedDate_Borrower = consentRejectedDateBorrower;
      if (!string.IsNullOrEmpty(consentIPBorrower))
        this.consentIP_Borrower = consentIPBorrower;
      if (!string.IsNullOrEmpty(eSignedDateBorrower))
        this.eSignedDate_Borrower = eSignedDateBorrower;
      if (string.IsNullOrEmpty(eSignedIPBorrower))
        return;
      this.eSignedIP_Borrower = eSignedIPBorrower;
    }

    public void SetCoBorrowerFields(
      string nameCoBorrower = null,
      string emailCoBorrower = null,
      string authenticatedDateCoBorrower = null,
      string authenticatedIPCoBorrower = null,
      string viewedDateCoBorrower = null,
      string consentAccpetedDateCoBorrower = null,
      string consentRejectedDateCoBorrower = null,
      string consentIPCoBorrower = null,
      string eSignedDateCoBorrower = null,
      string eSignedIPCoBorrower = null)
    {
      if (!string.IsNullOrEmpty(nameCoBorrower))
        this.name_CoBorrower = nameCoBorrower;
      if (!string.IsNullOrEmpty(emailCoBorrower))
        this.email_CoBorrower = emailCoBorrower;
      if (!string.IsNullOrEmpty(authenticatedDateCoBorrower))
        this.authenticatedDate_CoBorrower = authenticatedDateCoBorrower;
      if (!string.IsNullOrEmpty(authenticatedIPCoBorrower))
        this.authenticatedIP_CoBorrower = authenticatedIPCoBorrower;
      if (!string.IsNullOrEmpty(viewedDateCoBorrower))
        this.viewedDate_CoBorrower = viewedDateCoBorrower;
      if (!string.IsNullOrEmpty(consentAccpetedDateCoBorrower))
        this.consentAcceptedDate_CoBorrower = consentAccpetedDateCoBorrower;
      if (!string.IsNullOrEmpty(consentRejectedDateCoBorrower))
        this.consentRejectedDate_CoBorrower = consentRejectedDateCoBorrower;
      if (!string.IsNullOrEmpty(consentIPCoBorrower))
        this.consentIP_CoBorrower = consentIPCoBorrower;
      if (!string.IsNullOrEmpty(eSignedDateCoBorrower))
        this.eSignedDate_CoBorrower = eSignedDateCoBorrower;
      if (string.IsNullOrEmpty(eSignedIPCoBorrower))
        return;
      this.eSignedIP_CoBorrower = eSignedIPCoBorrower;
    }

    public void SetNBOFields(
      string nboNameEmail,
      string emailNBO,
      string authenticatedDateNBO = null,
      string authenticatedIPNBO = null,
      string viewedDateNBO = null,
      string consentAcceptedDateNBO = null,
      string consentRejectedDateNBO = null,
      string consentIPNBO = null,
      string eSignedDateNBO = null,
      string eSignedIPNBO = null)
    {
      if (!this.NBOitems.ContainsKey(nboNameEmail))
        this.NBOitems.Add(nboNameEmail, new global::NBODetail());
      if (!string.IsNullOrEmpty(emailNBO))
        this.NBOitems[nboNameEmail].email = emailNBO;
      if (!string.IsNullOrEmpty(authenticatedDateNBO))
        this.NBOitems[nboNameEmail].authenticatedDate = this.ParseDate(authenticatedDateNBO);
      if (!string.IsNullOrEmpty(authenticatedIPNBO))
        this.NBOitems[nboNameEmail].authenticatedIP = authenticatedIPNBO;
      if (!string.IsNullOrEmpty(viewedDateNBO))
        this.NBOitems[nboNameEmail].viewedDate = this.ParseDate(viewedDateNBO);
      if (!string.IsNullOrEmpty(consentAcceptedDateNBO))
        this.NBOitems[nboNameEmail].consentAcceptedDate = this.ParseDate(consentAcceptedDateNBO);
      if (!string.IsNullOrEmpty(consentRejectedDateNBO))
        this.NBOitems[nboNameEmail].consentRejectedDate = this.ParseDate(consentRejectedDateNBO);
      if (!string.IsNullOrEmpty(consentIPNBO))
        this.NBOitems[nboNameEmail].consentIP = consentIPNBO;
      if (!string.IsNullOrEmpty(eSignedDateNBO))
        this.NBOitems[nboNameEmail].eSignedDate = this.ParseDate(eSignedDateNBO);
      if (string.IsNullOrEmpty(eSignedIPNBO))
        return;
      this.NBOitems[nboNameEmail].eSignedIP = eSignedIPNBO;
    }

    public void SetLoanOfficerFields(
      string nameLO = null,
      string idLO = null,
      string viewedDateLO = null,
      string eSignedDateLO = null,
      string eSignedIPLO = null)
    {
      if (!string.IsNullOrEmpty(nameLO))
        this.name_LO = nameLO;
      if (!string.IsNullOrEmpty(idLO))
        this.id_LO = idLO;
      if (!string.IsNullOrEmpty(viewedDateLO))
        this.viewedDate_LO = viewedDateLO;
      if (!string.IsNullOrEmpty(eSignedDateLO))
        this.eSignedDate_LO = eSignedDateLO;
      if (string.IsNullOrEmpty(eSignedIPLO))
        return;
      this.eSignedIP_LO = eSignedIPLO;
    }

    public void SetCoBorrowerFulfillmentFields(
      string fulfillmentOrderedByCoBorrower = null,
      string fulfillmentProcessedDateCoBorrower = null,
      string fulfillmentScheduledDateCoBorrower = null)
    {
      if (!string.IsNullOrEmpty(fulfillmentOrderedByCoBorrower))
        this.fulfillmentOrderedBy_CoBorrower = fulfillmentOrderedByCoBorrower;
      if (!string.IsNullOrEmpty(fulfillmentProcessedDateCoBorrower))
        this.fulfillmentProcessedDate_CoBorrower = fulfillmentProcessedDateCoBorrower;
      if (string.IsNullOrEmpty(fulfillmentScheduledDateCoBorrower))
        return;
      this.fulfillmentScheduledDate_CoBorrower = fulfillmentScheduledDateCoBorrower;
    }

    public void SetNBODocumentViewedDateFields(string nboNameEmail, string documentViewedDateNBO = null)
    {
      if (!this.NBOitems.ContainsKey(nboNameEmail))
        this.NBOitems.Add(nboNameEmail, new global::NBODetail());
      if (string.IsNullOrEmpty(documentViewedDateNBO))
        return;
      this.NBOitems[nboNameEmail].documentViewedDate = this.ParseDate(documentViewedDateNBO);
    }

    public void SetDocumentViewedDateFields(
      string documentViewedDateBorrower = null,
      string documentViewedDateCoBorrower = null)
    {
      if (!string.IsNullOrEmpty(documentViewedDateBorrower))
        this.documentViewedDate_Borrower = documentViewedDateBorrower;
      if (string.IsNullOrEmpty(documentViewedDateCoBorrower))
        return;
      this.documentViewedDate_CoBorrower = documentViewedDateCoBorrower;
    }

    public void SetEDisclosureLoanLevelConsentFields(
      string eDisclosureBorrowerLoanLevelConsent = null,
      string eDisclosureCoBorrowerLoanLevelConsent = null)
    {
      if (!string.IsNullOrEmpty(eDisclosureBorrowerLoanLevelConsent))
        this.eDisclosureBorrowerLoanLevelConsent = eDisclosureBorrowerLoanLevelConsent;
      if (string.IsNullOrEmpty(eDisclosureCoBorrowerLoanLevelConsent))
        return;
      this.eDisclosureCoBorrowerLoanLevelConsent = eDisclosureCoBorrowerLoanLevelConsent;
    }

    public void SetNBOEDisclosureLoanLevelConsentFields(
      string nboNameEmail,
      string eDisclosureNBOLoanLevelConsent = null)
    {
      if (!this.NBOitems.ContainsKey(nboNameEmail))
        this.NBOitems.Add(nboNameEmail, new global::NBODetail());
      if (string.IsNullOrEmpty(eDisclosureNBOLoanLevelConsent))
        return;
      this.NBOitems[nboNameEmail].loanLevelConsent = eDisclosureNBOLoanLevelConsent;
    }

    public string PackageGuid => this.packageGuid;

    public bool MarkedForeSignatures => this.eSignatures == "1";

    public bool MarkedForFulfillment => this.fulfillment != "";

    public string FulfillmentOrderedBy => this.fulfillmentOrderedBy;

    public DateTime FulfillmentProcessedDate => this.ParseDate(this.fulfillmentProcessedDate);

    public string FulfillmentOrderedBy_CoBorrower => this.fulfillmentOrderedBy_CoBorrower;

    public DateTime FulfillmentProcessedDate_CoBorrower
    {
      get => this.ParseDate(this.fulfillmentProcessedDate_CoBorrower);
    }

    public bool ContainCoBorrower => this.containCoBorrower;

    public bool ContainLoanOfficer => this.containLO;

    public byte[] ConsentPDF => Convert.FromBase64String(this.consentPDF);

    private DateTime PackageExpiryNotificationDate
    {
      get => this.ParseDate(this.packageExpiryNotificationDate);
    }

    private DateTime FulfillmentScheduledDate => this.ParseDate(this.fulfillmentScheduledDate);

    public DateTime PackageCreatedDate => this.ParseDate(this.packageCreatedDate);

    public string BorrowerName => this.name_Borrower;

    public string BorrowerEmail => this.email_Borrower;

    public DateTime BorrowerAuthenticatedDate => this.ParseDate(this.authenticatedDate_Borrower);

    public string BorrowerAuthenticatedIP => this.authenticatedIP_Borrower;

    public DateTime BorrowerViewedDate => this.ParseDate(this.viewedDate_Borrower);

    public DateTime BorrowerConsentAcceptedDate
    {
      get => this.ParseDate(this.consentAcceptedDate_Borrower);
    }

    public DateTime BorrowerConsentRejectedDate
    {
      get => this.ParseDate(this.consentRejectedDate_Borrower);
    }

    public string BorrowerConsentAcceptedIP
    {
      get
      {
        return this.BorrowerConsentAcceptedDate.Equals(DateTime.MinValue) ? "" : this.consentIP_Borrower;
      }
    }

    public string BorrowerConsentRejectedIP
    {
      get
      {
        return this.BorrowerConsentRejectedDate.Equals(DateTime.MinValue) ? "" : this.consentIP_Borrower;
      }
    }

    public DateTime BorrowereSignedDate => this.ParseDate(this.eSignedDate_Borrower);

    public string BorrowereSignedIP => this.eSignedIP_Borrower;

    public string CoborrowerName => this.name_CoBorrower;

    public string CoborrowerEmail => this.email_CoBorrower;

    public DateTime CoborrowerAuthenticatedDate
    {
      get => this.ParseDate(this.authenticatedDate_CoBorrower);
    }

    public string CoborrowerAuthenticatedIP => this.authenticatedIP_CoBorrower;

    public DateTime CoborrowerViewedDate => this.ParseDate(this.viewedDate_CoBorrower);

    public DateTime CoborrowerConsentAcceptedDate
    {
      get => this.ParseDate(this.consentAcceptedDate_CoBorrower);
    }

    public DateTime CoborrowerConsentRejectedDate
    {
      get => this.ParseDate(this.consentRejectedDate_CoBorrower);
    }

    public string CoborrowerConsentAcceptedIP
    {
      get
      {
        return this.CoborrowerConsentAcceptedDate.Equals(DateTime.MinValue) ? "" : this.consentIP_CoBorrower;
      }
    }

    public string CoborrowerConsentRejectedIP
    {
      get
      {
        return this.CoborrowerConsentRejectedDate.Equals(DateTime.MinValue) ? "" : this.consentIP_CoBorrower;
      }
    }

    public DateTime CoborrowereSignedDate => this.ParseDate(this.eSignedDate_CoBorrower);

    public string CoborrowereSignedIP => this.eSignedIP_CoBorrower;

    public DateTime DocumentViewedDate_Borrower => this.ParseDate(this.documentViewedDate_Borrower);

    public DateTime DocumentViewedDate_CoBorrower
    {
      get => this.ParseDate(this.documentViewedDate_CoBorrower);
    }

    public string EDisclosureBorrowerLoanLevelConsent => this.eDisclosureBorrowerLoanLevelConsent;

    public string EDisclosureCoBorrowerLoanLevelConsent
    {
      get => this.eDisclosureCoBorrowerLoanLevelConsent;
    }

    public string EDisclosureBorrowerPackageLevelConsent
    {
      get => this.eDisclosureBorrowerPackageLevelConsent;
    }

    public string EDisclosureCoBorrowerPackageLevelConsent
    {
      get => this.eDisclosureCoBorrowerPackageLevelConsent;
    }

    public string LoanOfficerName => this.name_LO;

    public string LoanOfficerUserId => this.id_LO;

    public DateTime LoanOfficerViewedDate => this.ParseDate(this.viewedDate_LO);

    public DateTime LoanOfficereSignedDate => this.ParseDate(this.eSignedDate_LO);

    public string LoanOfficereSignedIP => this.eSignedIP_LO;

    public Dictionary<string, global::NBODetail> NBODetail => this.NBOitems;

    private static string getElementValue(XmlNode xmlNode, string xPath)
    {
      if (xPath.StartsWith("@"))
        return xmlNode.Attributes[xPath.Substring(1, xPath.Length - 1)] != null ? xmlNode.Attributes[xPath.Substring(1, xPath.Length - 1)].Value : "";
      try
      {
        if (xmlNode.SelectSingleNode(xPath) != null)
          return xmlNode.SelectSingleNode(xPath).Value;
      }
      catch
      {
        return "";
      }
      return "";
    }

    public static string GetErrorMessage(string errorXml)
    {
      XmlDocument xmlDocument = new XmlDocument();
      try
      {
        xmlDocument.LoadXml(errorXml);
        XmlNode xmlNode = xmlDocument.SelectSingleNode("Error");
        return xmlNode != null ? xmlNode.InnerText : "";
      }
      catch (Exception ex)
      {
        Tracing.Log(DisclosurePackage.sw, TraceLevel.Warning, nameof (DisclosurePackage), "Invalid xml format for disclosure package detail:" + ex.Message);
        return "Invalid xml format for disclosure package detail:" + ex.Message;
      }
    }

    private DateTime ParseDate(string strValue)
    {
      if (string.IsNullOrEmpty(strValue))
        return DateTime.MinValue;
      DateTime dateTime = Utils.ParseDate((object) strValue);
      if (this.timeZoneInfo != null)
        dateTime = System.TimeZoneInfo.ConvertTime(dateTime, Utils.GetTimeZoneInfo("PST"), this.timeZoneInfo);
      return dateTime;
    }
  }
}
