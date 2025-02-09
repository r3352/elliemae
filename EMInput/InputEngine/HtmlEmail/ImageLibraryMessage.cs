// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.ImageLibraryMessage
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class ImageLibraryMessage
  {
    private XmlDocument xmlDoc;
    private ResourceAccessType accessType;
    private ResourceActionType actionType;
    private string filename = string.Empty;
    private Sessions.Session session;

    public ImageLibraryMessage(
      ResourceAccessType accessType,
      ResourceActionType actionType,
      string filename)
      : this(accessType, actionType, filename, Session.DefaultInstance)
    {
    }

    public ImageLibraryMessage(
      ResourceAccessType accessType,
      ResourceActionType actionType,
      string filename,
      Sessions.Session session)
    {
      this.accessType = accessType;
      this.actionType = actionType;
      this.filename = filename;
      this.session = session;
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

    public string ToXml()
    {
      CompanyInfo companyInfo = this.session.CompanyInfo;
      UserInfo userInfo = this.session.UserInfo;
      IEPass service = this.session.Application.GetService<IEPass>();
      string val1 = service == null ? this.session.Password : service.UserPassword;
      OrgInfo orgInfo = this.session.OrganizationManager.GetFirstAvaliableOrganization(this.session.UserInfo.OrgId);
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
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.LoadXml("<RESOURCEACCESS/>");
      string val2 = string.Empty;
      switch (this.actionType)
      {
        case ResourceActionType.Upload:
          val2 = "Upload";
          break;
        case ResourceActionType.Delete:
          val2 = "Delete";
          break;
        case ResourceActionType.GetList:
          val2 = "GetList";
          break;
      }
      string val3 = string.Empty;
      switch (this.accessType)
      {
        case ResourceAccessType.Company:
          val3 = "Company";
          break;
        case ResourceAccessType.User:
          val3 = "User";
          break;
        case ResourceAccessType.All:
          if (this.actionType == ResourceActionType.GetList)
          {
            val3 = "All";
            break;
          }
          break;
      }
      this.setNodeText("@Version", VersionInformation.CurrentVersion.DisplayVersion);
      this.setNodeText("@Type", val3);
      this.setNodeText("@Action", val2);
      if (this.actionType != ResourceActionType.GetList)
        this.setNodeText("@Filename", this.filename);
      this.setNodeText("FROM/@ClientID", companyInfo.ClientID);
      this.setNodeText("FROM/@UserID", userInfo.Userid);
      this.setNodeText("FROM/@UserPassword", val1);
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
      return this.xmlDoc.OuterXml;
    }

    public void FromXml(
      string xmlResponse,
      List<ImageInfo> companyImageInfo,
      List<ImageInfo> userImageInfo)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlResponse);
      if (this.accessType == ResourceAccessType.Company || this.accessType == ResourceAccessType.All)
      {
        foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("COMPANY/IMAGE"))
        {
          DateTime result1 = DateTime.MinValue;
          int result2 = 0;
          string nodeText1 = this.getNodeText(selectNode, "@Filename", 0);
          int.TryParse(this.getNodeText(selectNode, "@Filesize", 0), out result2);
          DateTime.TryParse(this.getNodeText(selectNode, "@ReceivedDate", 0), out result1);
          string nodeText2 = this.getNodeText(selectNode, "@UploadedBy", 0);
          string nodeText3 = this.getNodeText(selectNode, "@Url", 0);
          ImageInfo imageInfo = new ImageInfo(nodeText1, result2, result1, nodeText2, ResourceAccessType.Company, nodeText3);
          companyImageInfo?.Add(imageInfo);
        }
      }
      if (this.accessType != ResourceAccessType.User && this.accessType != ResourceAccessType.All)
        return;
      foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("USER/IMAGE"))
      {
        DateTime result3 = DateTime.MinValue;
        int result4 = 0;
        string nodeText4 = this.getNodeText(selectNode, "@Filename", 0);
        int.TryParse(this.getNodeText(selectNode, "@Filesize", 0), out result4);
        DateTime.TryParse(this.getNodeText(selectNode, "@ReceivedDate", 0), out result3);
        string nodeText5 = this.getNodeText(selectNode, "@UploadedBy", 0);
        string nodeText6 = this.getNodeText(selectNode, "@Url", 0);
        ImageInfo imageInfo = new ImageInfo(nodeText4, result4, result3, nodeText5, ResourceAccessType.User, nodeText6);
        userImageInfo?.Add(imageInfo);
      }
    }

    private string getNodeText(XmlElement elm, string xpath, int length)
    {
      XmlNode xmlNode = elm.SelectSingleNode(xpath);
      if (xmlNode == null)
        return string.Empty;
      string str = xmlNode.InnerText.Trim();
      return str.Length > length && length > 0 ? str.Substring(0, length) : str;
    }
  }
}
