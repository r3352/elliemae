// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.HMDAInformation
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class HMDAInformation : IComparable<HMDAInformation>
  {
    private static Dictionary<string, string> _agencyNames = new Dictionary<string, string>();

    public string HMDARespondentID { get; set; }

    public string HMDARespondentTaxID { get; set; }

    public string HMDARespondentAgency { get; set; }

    public string HMDALEI { get; set; }

    public string HMDACompanyName { get; set; }

    public string HMDAContactName { get; set; }

    public string HMDAContactAddressLine1 { get; set; }

    public string HMDAContactCity { get; set; }

    public string HMDAContactState { get; set; }

    public string HMDAContactZipCode { get; set; }

    public string HMDAContactPhone { get; set; }

    public string HMDAContactFax { get; set; }

    public string HMDAContactEmail { get; set; }

    public string HMDAParentName { get; set; }

    public string HMDAParentAddressLine1 { get; set; }

    public string HMDAParentCity { get; set; }

    public string HMDAParentState { get; set; }

    public string HMDAParentZipCode { get; set; }

    public string HMDAApplicationDate { get; set; }

    public bool HMDAInstitutionPurchaseLoans { get; set; }

    public string HMDAChannelInfoNoChannel { get; set; }

    public string HMDAChannelInfoCorrespondent { get; set; }

    public bool HMDAShowDemographicInfo { get; set; }

    public bool HMDAReportIncome { get; set; }

    public bool HMDADisplayRateSpreadTo3Decimals { get; set; }

    public bool HMDAReportAgeOfBorrower { get; set; }

    public bool HMDAReportAgeOfCoBorrower { get; set; }

    public string HMDADTI { get; set; }

    public string HMDACLTV { get; set; }

    public string HMDAIncome { get; set; }

    public string HMDAProfileID { get; set; }

    public bool HMDANuli { get; set; }

    public string HMDARespondentAgencyName
    {
      get
      {
        string respondentAgencyName = (string) null;
        if (this.HMDARespondentAgency != null)
          HMDAInformation._agencyNames.TryGetValue(this.HMDARespondentAgency, out respondentAgencyName);
        return respondentAgencyName;
      }
    }

    static HMDAInformation()
    {
      HMDAInformation._agencyNames.Add("0", string.Empty);
      HMDAInformation._agencyNames.Add("1", "1. The Office of Comptroller of the Currency (OCC)");
      HMDAInformation._agencyNames.Add("2", "2. Federal Reserve System (FRS)");
      HMDAInformation._agencyNames.Add("3", "3. Federal Deposit Insurance Corporation (FDIC)");
      HMDAInformation._agencyNames.Add("5", "5. National Credit Union Administration (NCUA)");
      HMDAInformation._agencyNames.Add("7", "7. United States Department of Housing and Urban Development (HUD)");
      HMDAInformation._agencyNames.Add("9", "9. Consumer Financial Protection Bureau (CFPB)");
    }

    public HMDAInformation(string xmlString = "")
    {
      if (string.IsNullOrEmpty(xmlString))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlString);
      this.Initialize(xmlDocument.ChildNodes.Item(0));
    }

    public void Initialize(XmlNode xmlNode)
    {
      this.HMDARespondentID = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ID");
      this.HMDARespondentTaxID = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/TaxID");
      this.HMDARespondentAgency = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/Agency");
      this.HMDALEI = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/LEI");
      this.HMDACompanyName = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/CompanyName");
      this.HMDAContactName = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/Name");
      this.HMDAContactAddressLine1 = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/Address");
      this.HMDAContactCity = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/City");
      this.HMDAContactState = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/State");
      this.HMDAContactZipCode = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/ZipCode");
      this.HMDAContactPhone = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/ContactPhone");
      this.HMDAContactFax = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/ContactFax");
      this.HMDAContactEmail = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ContactInformation/ContactEmail");
      this.HMDAParentName = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ParentMailingAddress/Name");
      this.HMDAParentAddressLine1 = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ParentMailingAddress/Address");
      this.HMDAParentCity = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ParentMailingAddress/City");
      this.HMDAParentState = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ParentMailingAddress/State");
      this.HMDAParentZipCode = this.GetElementValue(xmlNode, "/HMDASettings/RespondentInformation/ParentMailingAddress/ZipCode");
      this.HMDAApplicationDate = this.GetElementValue(xmlNode, "/HMDASettings/HMDAApplicationInformation/ApplicationDate");
      this.HMDAInstitutionPurchaseLoans = Convert.ToBoolean(this.GetElementValue(xmlNode, "/HMDASettings/HMDAApplicationInformation/InstitutionType"));
      this.HMDAChannelInfoNoChannel = this.GetElementValue(xmlNode, "/HMDASettings/ChannelOption/NoChannel");
      this.HMDAChannelInfoCorrespondent = this.GetElementValue(xmlNode, "/HMDASettings/ChannelOption/Correspondent");
      this.HMDAShowDemographicInfo = Convert.ToBoolean(this.GetElementValue(xmlNode, "/HMDASettings/PurchasedLoans/ShowDemographicInformation"));
      this.HMDAReportIncome = Convert.ToBoolean(this.GetElementValue(xmlNode, "/HMDASettings/PurchasedLoans/ReportIncome"));
      this.HMDAReportAgeOfBorrower = Convert.ToBoolean(this.GetElementValue(xmlNode, "/HMDASettings/PurchasedLoans/ReportAgeBorrower"));
      this.HMDAReportAgeOfCoBorrower = Convert.ToBoolean(this.GetElementValue(xmlNode, "/HMDASettings/PurchasedLoans/ReportAgeCoBorrower"));
      this.HMDADisplayRateSpreadTo3Decimals = Convert.ToBoolean(this.GetElementValue(xmlNode, "/HMDASettings/RateSpread/DisplayRateSpreadTo3Decimals"));
      this.HMDADTI = this.GetElementValue(xmlNode, "/HMDASettings/HMDAReliedUponFactorsInformation/DTI");
      this.HMDACLTV = this.GetElementValue(xmlNode, "/HMDASettings/HMDAReliedUponFactorsInformation/CLTV");
      this.HMDAIncome = this.GetElementValue(xmlNode, "/HMDASettings/HMDAReliedUponFactorsInformation/Income");
      string elementValue = this.GetElementValue(xmlNode, "/HMDASettings/HMDAReliedUponFactorsInformation/HMDANuli");
      this.HMDANuli = !string.IsNullOrEmpty(elementValue) && Convert.ToBoolean(elementValue);
    }

    private string GetElementValue(XmlNode xmlNode, string xPath)
    {
      if (xPath.StartsWith("@"))
        return xmlNode.Attributes[xPath.Substring(1, xPath.Length - 1)] != null ? xmlNode.Attributes[xPath.Substring(1, xPath.Length - 1)].Value : "";
      try
      {
        if (xmlNode.SelectSingleNode(xPath) != null)
          return xmlNode.SelectSingleNode(xPath).InnerText;
      }
      catch
      {
        return "";
      }
      return "";
    }

    public string ToXmlString()
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement element1 = xmlDocument.CreateElement("HMDASettings");
      XmlElement element2 = xmlDocument.CreateElement("RespondentInformation");
      XmlElement element3 = xmlDocument.CreateElement("ID");
      element3.InnerText = this.HMDARespondentID;
      element2.AppendChild((XmlNode) element3);
      XmlElement element4 = xmlDocument.CreateElement("TaxID");
      element4.InnerText = this.HMDARespondentTaxID;
      element2.AppendChild((XmlNode) element4);
      XmlElement element5 = xmlDocument.CreateElement("Agency");
      element5.InnerText = this.HMDARespondentAgency;
      element2.AppendChild((XmlNode) element5);
      XmlElement element6 = xmlDocument.CreateElement("LEI");
      element6.InnerText = this.HMDALEI;
      element2.AppendChild((XmlNode) element6);
      XmlElement element7 = xmlDocument.CreateElement("ContactInformation");
      XmlElement element8 = xmlDocument.CreateElement("CompanyName");
      element8.InnerText = this.HMDACompanyName;
      element7.AppendChild((XmlNode) element8);
      XmlElement element9 = xmlDocument.CreateElement("Name");
      element9.InnerText = this.HMDAContactName;
      element7.AppendChild((XmlNode) element9);
      XmlElement element10 = xmlDocument.CreateElement("Address");
      element10.InnerText = this.HMDAContactAddressLine1;
      element7.AppendChild((XmlNode) element10);
      XmlElement element11 = xmlDocument.CreateElement("City");
      element11.InnerText = this.HMDAContactCity;
      element7.AppendChild((XmlNode) element11);
      XmlElement element12 = xmlDocument.CreateElement("State");
      element12.InnerText = this.HMDAContactState;
      element7.AppendChild((XmlNode) element12);
      XmlElement element13 = xmlDocument.CreateElement("ZipCode");
      element13.InnerText = this.HMDAContactZipCode;
      element7.AppendChild((XmlNode) element13);
      XmlElement element14 = xmlDocument.CreateElement("ContactPhone");
      element14.InnerText = this.HMDAContactPhone;
      element7.AppendChild((XmlNode) element14);
      XmlElement element15 = xmlDocument.CreateElement("ContactFax");
      element15.InnerText = this.HMDAContactFax;
      element7.AppendChild((XmlNode) element15);
      XmlElement element16 = xmlDocument.CreateElement("ContactEmail");
      element16.InnerText = this.HMDAContactEmail;
      element7.AppendChild((XmlNode) element16);
      element2.AppendChild((XmlNode) element7);
      XmlElement element17 = xmlDocument.CreateElement("ParentMailingAddress");
      XmlElement element18 = xmlDocument.CreateElement("Name");
      element18.InnerText = this.HMDAParentName;
      element17.AppendChild((XmlNode) element18);
      XmlElement element19 = xmlDocument.CreateElement("Address");
      element19.InnerText = this.HMDAParentAddressLine1;
      element17.AppendChild((XmlNode) element19);
      XmlElement element20 = xmlDocument.CreateElement("City");
      element20.InnerText = this.HMDAParentCity;
      element17.AppendChild((XmlNode) element20);
      XmlElement element21 = xmlDocument.CreateElement("State");
      element21.InnerText = this.HMDAParentState;
      element17.AppendChild((XmlNode) element21);
      XmlElement element22 = xmlDocument.CreateElement("ZipCode");
      element22.InnerText = this.HMDAParentZipCode;
      element17.AppendChild((XmlNode) element22);
      element2.AppendChild((XmlNode) element17);
      element1.AppendChild((XmlNode) element2);
      XmlElement element23 = xmlDocument.CreateElement("HMDAApplicationInformation");
      XmlElement element24 = xmlDocument.CreateElement("ApplicationDate");
      element24.InnerText = this.HMDAApplicationDate;
      element23.AppendChild((XmlNode) element24);
      XmlElement element25 = xmlDocument.CreateElement("InstitutionType");
      element25.InnerText = this.HMDAInstitutionPurchaseLoans.ToString();
      element23.AppendChild((XmlNode) element25);
      element1.AppendChild((XmlNode) element23);
      XmlElement element26 = xmlDocument.CreateElement("ChannelOption");
      XmlElement element27 = xmlDocument.CreateElement("NoChannel");
      element27.InnerText = this.HMDAChannelInfoNoChannel;
      element26.AppendChild((XmlNode) element27);
      XmlElement element28 = xmlDocument.CreateElement("Correspondent");
      element28.InnerText = this.HMDAChannelInfoCorrespondent;
      element26.AppendChild((XmlNode) element28);
      element1.AppendChild((XmlNode) element26);
      XmlElement element29 = xmlDocument.CreateElement("PurchasedLoans");
      XmlElement element30 = xmlDocument.CreateElement("ShowDemographicInformation");
      element30.InnerText = this.HMDAShowDemographicInfo.ToString();
      element29.AppendChild((XmlNode) element30);
      XmlElement element31 = xmlDocument.CreateElement("ReportIncome");
      element31.InnerText = this.HMDAReportIncome.ToString();
      element29.AppendChild((XmlNode) element31);
      XmlElement element32 = xmlDocument.CreateElement("ReportAgeBorrower");
      element32.InnerText = this.HMDAReportAgeOfBorrower.ToString();
      element29.AppendChild((XmlNode) element32);
      XmlElement element33 = xmlDocument.CreateElement("ReportAgeCoBorrower");
      element33.InnerText = this.HMDAReportAgeOfCoBorrower.ToString();
      element29.AppendChild((XmlNode) element33);
      element1.AppendChild((XmlNode) element29);
      XmlElement element34 = xmlDocument.CreateElement("RateSpread");
      XmlElement element35 = xmlDocument.CreateElement("DisplayRateSpreadTo3Decimals");
      element35.InnerText = this.HMDADisplayRateSpreadTo3Decimals.ToString();
      element34.AppendChild((XmlNode) element35);
      element1.AppendChild((XmlNode) element34);
      XmlElement element36 = xmlDocument.CreateElement("HMDAReliedUponFactorsInformation");
      XmlElement element37 = xmlDocument.CreateElement("DTI");
      element37.InnerText = this.HMDADTI;
      element36.AppendChild((XmlNode) element37);
      XmlElement element38 = xmlDocument.CreateElement("CLTV");
      element38.InnerText = this.HMDACLTV;
      element36.AppendChild((XmlNode) element38);
      XmlElement element39 = xmlDocument.CreateElement("Income");
      element39.InnerText = this.HMDAIncome;
      element36.AppendChild((XmlNode) element39);
      XmlElement element40 = xmlDocument.CreateElement("HMDANuli");
      element40.InnerText = this.HMDANuli.ToString();
      element36.AppendChild((XmlNode) element40);
      element1.AppendChild((XmlNode) element36);
      return element1.OuterXml;
    }

    public int CompareTo(HMDAInformation other)
    {
      return string.Compare(this.HMDARespondentID, other.HMDARespondentID) + string.Compare(this.HMDARespondentTaxID, other.HMDARespondentTaxID) + string.Compare(this.HMDARespondentAgency, other.HMDARespondentAgency) + string.Compare(this.HMDALEI, other.HMDALEI) + string.Compare(this.HMDACompanyName, other.HMDACompanyName) + string.Compare(this.HMDAContactName, other.HMDAContactName) + string.Compare(this.HMDAContactAddressLine1, other.HMDAContactAddressLine1) + string.Compare(this.HMDAContactCity, other.HMDAContactCity) + string.Compare(this.HMDAContactState, other.HMDAContactState) + string.Compare(this.HMDAContactZipCode, other.HMDAContactZipCode) + string.Compare(this.HMDAContactPhone, other.HMDAContactPhone) + string.Compare(this.HMDAContactFax, other.HMDAContactFax) + string.Compare(this.HMDAContactEmail, other.HMDAContactEmail) + string.Compare(this.HMDAParentName, other.HMDAParentName) + string.Compare(this.HMDAParentAddressLine1, other.HMDAParentAddressLine1) + string.Compare(this.HMDAParentCity, other.HMDAParentCity) + string.Compare(this.HMDAParentState, other.HMDAParentState) + string.Compare(this.HMDAParentZipCode, other.HMDAParentZipCode) + string.Compare(this.HMDAApplicationDate, other.HMDAApplicationDate) + (this.HMDAInstitutionPurchaseLoans == other.HMDAInstitutionPurchaseLoans ? 0 : 1) + string.Compare(this.HMDAChannelInfoNoChannel, other.HMDAChannelInfoNoChannel) + string.Compare(this.HMDAChannelInfoCorrespondent, other.HMDAChannelInfoCorrespondent) + (this.HMDAShowDemographicInfo == other.HMDAShowDemographicInfo ? 0 : 1) + (this.HMDAReportIncome == other.HMDAReportIncome ? 0 : 1) + (this.HMDAReportAgeOfBorrower == other.HMDAReportAgeOfBorrower ? 0 : 1) + (this.HMDAReportAgeOfCoBorrower == other.HMDAReportAgeOfCoBorrower ? 0 : 1) + string.Compare(this.HMDADTI, other.HMDADTI) + string.Compare(this.HMDACLTV, other.HMDACLTV) + string.Compare(this.HMDAIncome, other.HMDAIncome) + (this.HMDANuli == other.HMDANuli ? 0 : 1) + (this.HMDADisplayRateSpreadTo3Decimals == other.HMDADisplayRateSpreadTo3Decimals ? 0 : 1);
    }
  }
}
