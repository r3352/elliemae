// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.StatusOnlineMessage
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class StatusOnlineMessage
  {
    private LoanDataMgr loanDataMgr;
    private TriggerPortalType portalType;
    private string to;
    private string cc;
    private string replyTo;
    private string subject;
    private string body;
    private bool readReceipt;
    private XmlDocument xmlDoc;
    private bool useBranchAddress;
    private string consentModel;

    public StatusOnlineMessage(LoanDataMgr loanDataMgr, TriggerPortalType portalType)
    {
      this.loanDataMgr = loanDataMgr;
      this.portalType = portalType;
      this.replyTo = Session.UserInfo.Email;
    }

    public LoanDataMgr LoanDataMgr => this.loanDataMgr;

    public TriggerPortalType PortalType => this.portalType;

    public string ReplyTo
    {
      get => this.replyTo;
      set => this.replyTo = value;
    }

    public string To
    {
      get => this.to;
      set => this.to = value;
    }

    public string CC
    {
      get => this.cc;
      set => this.cc = value;
    }

    public string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public string Body
    {
      get => this.body;
      set => this.body = value;
    }

    public bool ReadReceipt
    {
      get => this.readReceipt;
      set => this.readReceipt = value;
    }

    public bool UseSameWebCenter
    {
      get => this.loanDataMgr.SystemConfiguration.WebCenterSettings.UseSameWebcenter;
    }

    public bool UseBranchAddress
    {
      get => this.loanDataMgr.ConfigurationManager.GetEDisclosureSetup().UseBranchAddress;
      set => this.useBranchAddress = value;
    }

    public string ConsentModel
    {
      get => this.loanDataMgr.ConfigurationManager.GetEDisclosureSetup().ConsentModelType;
      set => this.consentModel = value;
    }

    public bool CheckRequirements()
    {
      if (this.portalType == TriggerPortalType.WebCenter)
      {
        string simpleField1 = this.loanDataMgr.LoanData.GetSimpleField("11");
        string simpleField2 = this.loanDataMgr.LoanData.GetSimpleField("FR0104");
        if (!simpleField1.Contains(" ") && !simpleField2.Contains(" "))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Status online cannot be published without the Borrower's Present Address or Property Address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private void setNodeText(string xpath, string val)
    {
      if (val == null || val == string.Empty)
        return;
      XmlElement xmlElement = this.xmlDoc.DocumentElement;
      string str1 = xpath;
      char[] chArray = new char[1]{ '/' };
      foreach (string xpath1 in str1.Split(chArray))
      {
        if (xpath1.StartsWith("@"))
        {
          xmlElement.SetAttribute(xpath1.Substring(1), val);
          return;
        }
        if (xpath1 == "!CDATA")
        {
          xmlElement.AppendChild((XmlNode) this.xmlDoc.CreateCDataSection(val));
          return;
        }
        string name1 = xpath1;
        if (name1.EndsWith("]"))
          name1 = name1.Substring(0, name1.IndexOf("["));
        XmlNode xmlNode = xmlElement.SelectSingleNode(xpath1);
        while (xmlNode == null)
        {
          xmlNode = xmlElement.AppendChild((XmlNode) this.xmlDoc.CreateElement(name1));
          if (xpath1.IndexOf("[@") > 0)
          {
            string str2 = xpath1.Substring(xpath1.IndexOf("[@") + 2);
            string str3 = str2.Substring(0, str2.LastIndexOf("]"));
            string name2 = str3.Substring(0, str3.IndexOf("=")).Trim();
            string str4 = str3.Substring(str3.IndexOf("\"") + 1);
            string str5 = str4.Substring(0, str4.LastIndexOf("\""));
            ((XmlElement) xmlNode).SetAttribute(name2, str5);
          }
          else
            xmlNode = xmlElement.SelectSingleNode(xpath1);
        }
        xmlElement = (XmlElement) xmlNode;
      }
      xmlElement.InnerText = val;
    }

    private void setNodeField(string xpath, string val)
    {
      this.setNodeText(xpath, this.loanDataMgr.LoanData.GetField(val));
    }

    public string ToRegisterXml()
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      CompanyInfo companyInfo = Session.CompanyInfo;
      UserInfo userInfo = Session.UserInfo;
      OrgInfo orgInfo = Session.OrganizationManager.GetFirstAvaliableOrganization(Session.UserInfo.OrgId);
      if (orgInfo == null)
      {
        orgInfo = new OrgInfo();
        orgInfo.CompanyName = companyInfo.Name;
        orgInfo.CompanyAddress.Street1 = companyInfo.Address;
        orgInfo.CompanyAddress.City = companyInfo.City;
        orgInfo.CompanyAddress.State = companyInfo.State;
        orgInfo.CompanyAddress.Zip = companyInfo.Zip;
        orgInfo.CompanyPhone = companyInfo.Phone;
        orgInfo.CompanyFax = companyInfo.Fax;
      }
      string val1 = (string) null;
      switch (this.portalType)
      {
        case TriggerPortalType.WebCenter:
          val1 = "StatusOnline";
          break;
        case TriggerPortalType.TPOWC:
          val1 = "StatusOnlineTPO";
          break;
      }
      string val2 = (string) null;
      try
      {
        val2 = this.loanDataMgr.LoanData.GetField("TPO.X1");
      }
      catch
      {
      }
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml("<DOCUMENTSUBMISSION/>");
      this.setNodeText("@Type", val1);
      this.setNodeText("@UseSameWebCenter", this.UseSameWebCenter ? "Y" : "N");
      this.setNodeText("@Version", VersionInformation.CurrentVersion.DisplayVersion);
      this.setNodeText("@UseBranchAddress", this.UseBranchAddress ? "Y" : "N");
      this.setNodeText("@ConsentModel", this.ConsentModel);
      this.setNodeText("FROM/@ClientID", companyInfo.ClientID);
      this.setNodeText("FROM/@UserID", userInfo.Userid);
      this.setNodeText("FROM/@UserPassword", EpassLogin.LoginPassword);
      this.setNodeText("FROM/COMPANY/@Name", orgInfo.CompanyName);
      this.setNodeText("FROM/COMPANY/@StreetAddress1", orgInfo.CompanyAddress.Street1);
      this.setNodeText("FROM/COMPANY/@StreetAddress2", orgInfo.CompanyAddress.Street2);
      this.setNodeText("FROM/COMPANY/@City", orgInfo.CompanyAddress.City);
      this.setNodeText("FROM/COMPANY/@State", orgInfo.CompanyAddress.State);
      this.setNodeText("FROM/COMPANY/@PostalCode", orgInfo.CompanyAddress.Zip);
      this.setNodeText("FROM/COMPANY/@Phone", orgInfo.CompanyPhone);
      this.setNodeText("FROM/COMPANY/@Fax", orgInfo.CompanyFax);
      this.setNodeText("FROM/USER/@Name", userInfo.FullName);
      this.setNodeText("FROM/USER/@Phone", userInfo.Phone);
      this.setNodeText("FROM/USER/@Fax", userInfo.Fax);
      this.setNodeText("FROM/USER/@Email", userInfo.Email);
      this.setNodeField("LOAN/@LoanGUID", "GUID");
      this.setNodeField("LOAN/@LoanNumber", "364");
      this.setNodeText("LOAN/@BorrowerPair", loanData.PairId);
      this.setNodeField("LOAN/@WebsiteID", "WEBSITEID");
      this.setNodeText("LOAN/@TPOWebsiteID", val2);
      this.setNodeField("LOAN/@ApplicationDate", "745");
      this.setNodeField("LOAN/BORROWER/@FirstName", "4000");
      this.setNodeField("LOAN/BORROWER/@LastName", "4002");
      this.setNodeField("LOAN/BORROWER/@Name", "1868");
      this.setNodeField("LOAN/BORROWER/@StreetAddress", "FR0104");
      this.setNodeField("LOAN/BORROWER/@City", "FR0106");
      this.setNodeField("LOAN/BORROWER/@State", "FR0107");
      this.setNodeField("LOAN/BORROWER/@PostalCode", "FR0108");
      this.setNodeField("LOAN/BORROWER/@HomePhone", "66");
      this.setNodeField("LOAN/BORROWER/@WorkPhone", "FE0117");
      this.setNodeField("LOAN/BORROWER/@CellPhone", "1490");
      this.setNodeField("LOAN/BORROWER/@Email", "1240");
      if (loanData.GetSimpleField("4004") != string.Empty)
      {
        this.setNodeField("LOAN/COBORROWER/@FirstName", "4004");
        this.setNodeField("LOAN/COBORROWER/@LastName", "4006");
        this.setNodeField("LOAN/COBORROWER/@Name", "1873");
        this.setNodeField("LOAN/COBORROWER/@StreetAddress", "FR0204");
        this.setNodeField("LOAN/COBORROWER/@City", "FR0206");
        this.setNodeField("LOAN/COBORROWER/@State", "FR0207");
        this.setNodeField("LOAN/COBORROWER/@PostalCode", "FR0208");
        this.setNodeField("LOAN/COBORROWER/@HomePhone", "98");
        this.setNodeField("LOAN/COBORROWER/@WorkPhone", "FE0217");
        this.setNodeField("LOAN/COBORROWER/@CellPhone", "1480");
        this.setNodeField("LOAN/COBORROWER/@Email", "1268");
      }
      List<NonBorrowingOwner> byBorrowerPairId = this.loanDataMgr.LoanData.GetNboByBorrowerPairId(this.loanDataMgr.LoanData.CurrentBorrowerPair.Id);
      int num = 0;
      foreach (NonBorrowingOwner nonBorrowingOwner in byBorrowerPairId)
      {
        ++num;
        string str1 = "LOAN/ENCOMPASSCONTACTS/ENCOMPASSCONTACT[" + num.ToString() + "]/";
        this.setNodeText(str1 + "@Name", string.Join(" ", ((IEnumerable<string>) new string[4]
        {
          nonBorrowingOwner.FirstName,
          nonBorrowingOwner.MiddleName,
          nonBorrowingOwner.LastName,
          nonBorrowingOwner.SuffixName
        }).Where<string>((Func<string, bool>) (str => !string.IsNullOrEmpty(str)))));
        this.setNodeText(str1 + "@Email", nonBorrowingOwner.EmailAddress);
        this.setNodeText(str1 + "@LastName", nonBorrowingOwner.LastName);
        this.setNodeText(str1 + "@FirstName", nonBorrowingOwner.FirstName);
        this.setNodeText(str1 + "@EncompassContactGUID", nonBorrowingOwner.NBOID);
      }
      this.setNodeField("LOAN/PROPERTY/@StreetAddress", "11");
      this.setNodeField("LOAN/PROPERTY/@City", "12");
      this.setNodeField("LOAN/PROPERTY/@State", "14");
      this.setNodeField("LOAN/PROPERTY/@PostalCode", "15");
      this.setNodeField("LOAN/PROPERTY/@AppraisedValue", "356");
      this.setNodeField("LOAN/MORTGAGE/@MortgageType", "1172");
      this.setNodeField("LOAN/MORTGAGE/@LoanAmortizationType", "608");
      this.setNodeField("LOAN/MORTGAGE/@LoanPurpose", "19");
      this.setNodeField("LOAN/MORTGAGE/@InterestRatePercent", "3");
      this.setNodeField("LOAN/MORTGAGE/@LoanAmount", "1109");
      this.setNodeField("LOAN/ORIGINATOR/@UserID", "3239");
      this.setNodeField("LOAN/ORIGINATOR/@Name", "1612");
      this.setNodeField("LOAN/ORIGINATOR/@StreetAddress", "319");
      this.setNodeField("LOAN/ORIGINATOR/@City", "313");
      this.setNodeField("LOAN/ORIGINATOR/@State", "321");
      this.setNodeField("LOAN/ORIGINATOR/@PostalCode", "323");
      this.setNodeField("LOAN/ORIGINATOR/@Phone", "1823");
      this.setNodeField("LOAN/ORIGINATOR/@Fax", "326");
      this.setNodeField("LOAN/OFFICER/@UserID", "LOID");
      this.setNodeField("LOAN/OFFICER/@Name", "317");
      this.setNodeField("LOAN/OFFICER/@Title", "4508");
      this.setNodeField("LOAN/OFFICER/@StreetAddress", "319");
      this.setNodeField("LOAN/OFFICER/@City", "313");
      this.setNodeField("LOAN/OFFICER/@State", "321");
      this.setNodeField("LOAN/OFFICER/@PostalCode", "323");
      this.setNodeField("LOAN/OFFICER/@Phone", "1406");
      this.setNodeField("LOAN/OFFICER/@Fax", "2411");
      this.setNodeField("LOAN/OFFICER/@Email", "1407");
      this.setNodeField("LOAN/PROCESSOR/@UserID", "LPID");
      this.setNodeField("LOAN/PROCESSOR/@Name", "362");
      this.setNodeField("LOAN/PROCESSOR/@Title", "4509");
      this.setNodeField("LOAN/PROCESSOR/@StreetAddress", "319");
      this.setNodeField("LOAN/PROCESSOR/@City", "313");
      this.setNodeField("LOAN/PROCESSOR/@State", "321");
      this.setNodeField("LOAN/PROCESSOR/@PostalCode", "323");
      this.setNodeField("LOAN/PROCESSOR/@Phone", "1408");
      this.setNodeField("LOAN/PROCESSOR/@Fax", "2412");
      this.setNodeField("LOAN/PROCESSOR/@Email", "1409");
      this.setNodeField("LOAN/CLOSER/@UserID", "CLID");
      this.setNodeField("LOAN/CLOSER/@Name", "1855");
      this.setNodeField("LOAN/CLOSER/@StreetAddress", "319");
      this.setNodeField("LOAN/CLOSER/@City", "313");
      this.setNodeField("LOAN/CLOSER/@State", "321");
      this.setNodeField("LOAN/CLOSER/@PostalCode", "323");
      this.setNodeField("LOAN/CLOSER/@Phone", "1856");
      this.setNodeField("LOAN/CLOSER/@Fax", "2413");
      this.setNodeField("LOAN/CLOSER/@Email", "1857");
      return this.xmlDoc.OuterXml;
    }

    public string ToStatusXml(List<StatusOnlineTrigger> statusTriggers)
    {
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml("<STATUSONLINEPUBLISH/>");
      this.xmlDoc.DocumentElement.SetAttribute("name", "StatusOnlinePublish");
      XmlElement element1 = this.xmlDoc.CreateElement("element");
      this.xmlDoc.DocumentElement.AppendChild((XmlNode) element1);
      element1.SetAttribute("name", "Items");
      for (int index = 0; index < statusTriggers.Count; ++index)
      {
        StatusOnlineTrigger statusTrigger = statusTriggers[index];
        XmlElement element2 = this.xmlDoc.CreateElement("element");
        element1.AppendChild((XmlNode) element2);
        element2.SetAttribute("name", index.ToString());
        XmlElement element3 = this.xmlDoc.CreateElement("element");
        element2.AppendChild((XmlNode) element3);
        element3.SetAttribute("name", "GUID");
        element3.InnerText = statusTrigger.Guid;
        XmlElement element4 = this.xmlDoc.CreateElement("element");
        element2.AppendChild((XmlNode) element4);
        element4.SetAttribute("name", "description");
        element4.InnerText = statusTrigger.Name;
        XmlElement element5 = this.xmlDoc.CreateElement("element");
        element2.AppendChild((XmlNode) element5);
        element5.SetAttribute("name", "detailedDescription");
        if (statusTrigger.Description != null)
          element5.InnerText = statusTrigger.Description;
        else
          element5.InnerText = string.Empty;
        XmlElement element6 = this.xmlDoc.CreateElement("element");
        element2.AppendChild((XmlNode) element6);
        element6.SetAttribute("name", "statusDate");
        XmlElement xmlElement1 = element6;
        DateTime dateTime = statusTrigger.DateTriggered;
        string str1 = dateTime.ToString();
        xmlElement1.InnerText = str1;
        XmlElement element7 = this.xmlDoc.CreateElement("element");
        element2.AppendChild((XmlNode) element7);
        element7.SetAttribute("name", "publishDate");
        XmlElement xmlElement2 = element7;
        dateTime = statusTrigger.DatePublished;
        string str2 = dateTime.ToString();
        xmlElement2.InnerText = str2;
        XmlElement element8 = this.xmlDoc.CreateElement("element");
        element2.AppendChild((XmlNode) element8);
        element8.SetAttribute("name", "state");
        if (statusTrigger.DatePublished != DateTime.MinValue)
          element8.InnerText = "2";
        else
          element8.InnerText = "0";
      }
      return this.xmlDoc.OuterXml;
    }
  }
}
