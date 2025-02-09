// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationTimelineIncomeLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationTimelineIncomeLog : VerificationTimelineLog
  {
    private bool isPaystubs;
    private bool isTaxReturns;
    private int taxReturnYear;
    private bool isPension;
    private bool isMilitary;
    private bool isW2;
    private bool is1099;
    private bool isSocialSecurity;
    private bool is401K;
    private bool isOtherEmployment;
    private string otherEmploymentDescription = string.Empty;
    private bool isAlimonyOrMaintenance;
    private bool isRentalIncome;
    private bool isChildSupport;
    private bool isOtherNonEmployment;
    private string otherNonEmploymentDescription = string.Empty;

    public VerificationTimelineIncomeLog(XmlElement e)
      : base(e)
    {
    }

    public VerificationTimelineIncomeLog()
    {
    }

    public bool IsPaystubs
    {
      set => this.isPaystubs = value;
      get => this.isPaystubs;
    }

    public bool IsTaxReturns
    {
      set => this.isTaxReturns = value;
      get => this.isTaxReturns;
    }

    public int TaxReturnYear
    {
      set => this.taxReturnYear = value;
      get => this.taxReturnYear;
    }

    public bool IsPension
    {
      set => this.isPension = value;
      get => this.isPension;
    }

    public bool IsMilitary
    {
      set => this.isMilitary = value;
      get => this.isMilitary;
    }

    public bool IsW2
    {
      set => this.isW2 = value;
      get => this.isW2;
    }

    public bool Is1099
    {
      set => this.is1099 = value;
      get => this.is1099;
    }

    public bool IsSocialSecurity
    {
      set => this.isSocialSecurity = value;
      get => this.isSocialSecurity;
    }

    public bool Is401K
    {
      set => this.is401K = value;
      get => this.is401K;
    }

    public bool IsOtherEmployment
    {
      set => this.isOtherEmployment = value;
      get => this.isOtherEmployment;
    }

    public string OtherEmploymentDescription
    {
      set => this.otherEmploymentDescription = value;
      get => this.otherEmploymentDescription;
    }

    public bool IsAlimonyOrMaintenance
    {
      set => this.isAlimonyOrMaintenance = value;
      get => this.isAlimonyOrMaintenance;
    }

    public bool IsRentalIncome
    {
      set => this.isRentalIncome = value;
      get => this.isRentalIncome;
    }

    public bool IsChildSupport
    {
      set => this.isChildSupport = value;
      get => this.isChildSupport;
    }

    public bool IsOtherNonEmployment
    {
      set => this.isOtherNonEmployment = value;
      get => this.isOtherNonEmployment;
    }

    public string OtherNonEmploymentDescription
    {
      set => this.otherNonEmploymentDescription = value;
      get => this.otherNonEmploymentDescription;
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.IsPaystubs)
        str += "Paystubs";
      if (this.IsTaxReturns)
        str = str + (str != string.Empty ? "," : "") + "Tax Returns";
      if (this.IsPension)
        str = str + (str != string.Empty ? "," : "") + "Pension";
      if (this.IsMilitary)
        str = str + (str != string.Empty ? "," : "") + "Military";
      if (this.IsW2)
        str = str + (str != string.Empty ? "," : "") + "W2";
      if (this.Is1099)
        str = str + (str != string.Empty ? "," : "") + "1099";
      if (this.IsSocialSecurity)
        str = str + (str != string.Empty ? "," : "") + "Social Security";
      if (this.Is401K)
        str = str + (str != string.Empty ? "," : "") + "401K";
      if (this.IsOtherEmployment)
        str = str + (str != string.Empty ? "," : "") + "Other Employment";
      if (this.IsAlimonyOrMaintenance)
        str = str + (str != string.Empty ? "," : "") + "Alimony/Maintenance";
      if (this.IsRentalIncome)
        str = str + (str != string.Empty ? "," : "") + "Rental Income";
      if (this.IsChildSupport)
        str = str + (str != string.Empty ? "," : "") + "Child Support";
      if (this.IsOtherNonEmployment)
        str = str + (str != string.Empty ? "," : "") + "Other Non-Employment";
      return str;
    }

    public void SetStatusToXml(XmlElement fieldXml)
    {
      fieldXml.SetAttribute("IsPaystubs", this.IsPaystubs ? "Y" : "N");
      fieldXml.SetAttribute("IsTaxReturns", this.IsTaxReturns ? "Y" : "N");
      fieldXml.SetAttribute("TaxReturnYear", this.TaxReturnYear == 0 ? "" : this.TaxReturnYear.ToString());
      fieldXml.SetAttribute("IsPension", this.IsPension ? "Y" : "N");
      fieldXml.SetAttribute("IsMilitary", this.IsMilitary ? "Y" : "N");
      fieldXml.SetAttribute("IsW2", this.IsW2 ? "Y" : "N");
      fieldXml.SetAttribute("Is1099", this.Is1099 ? "Y" : "N");
      fieldXml.SetAttribute("IsSocialSecurity", this.IsSocialSecurity ? "Y" : "N");
      fieldXml.SetAttribute("Is401K", this.Is401K ? "Y" : "N");
      fieldXml.SetAttribute("IsOtherEmployment", this.IsOtherEmployment ? "Y" : "N");
      fieldXml.SetAttribute("OtherEmploymentDescription", this.OtherEmploymentDescription);
      fieldXml.SetAttribute("IsAlimonyOrMaintenance", this.IsAlimonyOrMaintenance ? "Y" : "N");
      fieldXml.SetAttribute("IsRentalIncome", this.IsRentalIncome ? "Y" : "N");
      fieldXml.SetAttribute("IsChildSupport", this.IsChildSupport ? "Y" : "N");
      fieldXml.SetAttribute("IsOtherNonEmployment", this.IsOtherNonEmployment ? "Y" : "N");
      fieldXml.SetAttribute("OtherNonEmploymentDescription", this.OtherNonEmploymentDescription);
    }

    public void GetStatusFromXml(XmlElement e)
    {
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode("STATUS");
      if (xmlElement == null)
        return;
      if (xmlElement.HasAttribute("IsPaystubs"))
        this.IsPaystubs = xmlElement.GetAttribute("IsPaystubs") == "Y";
      if (xmlElement.HasAttribute("IsTaxReturns"))
        this.IsTaxReturns = xmlElement.GetAttribute("IsTaxReturns") == "Y";
      if (xmlElement.HasAttribute("TaxReturnYear"))
        this.TaxReturnYear = xmlElement.GetAttribute("TaxReturnYear") == "" ? 0 : Utils.ParseInt((object) xmlElement.GetAttribute("TaxReturnYear"));
      if (xmlElement.HasAttribute("IsPension"))
        this.IsPension = xmlElement.GetAttribute("IsPension") == "Y";
      if (xmlElement.HasAttribute("IsMilitary"))
        this.IsMilitary = xmlElement.GetAttribute("IsMilitary") == "Y";
      if (xmlElement.HasAttribute("IsW2"))
        this.IsW2 = xmlElement.GetAttribute("IsW2") == "Y";
      if (xmlElement.HasAttribute("Is1099"))
        this.Is1099 = xmlElement.GetAttribute("Is1099") == "Y";
      if (xmlElement.HasAttribute("IsSocialSecurity"))
        this.IsSocialSecurity = xmlElement.GetAttribute("IsSocialSecurity") == "Y";
      if (xmlElement.HasAttribute("Is401K"))
        this.Is401K = xmlElement.GetAttribute("Is401K") == "Y";
      if (xmlElement.HasAttribute("IsOtherEmployment"))
        this.IsOtherEmployment = xmlElement.GetAttribute("IsOtherEmployment") == "Y";
      if (xmlElement.HasAttribute("OtherEmploymentDescription"))
        this.OtherEmploymentDescription = xmlElement.GetAttribute("OtherEmploymentDescription");
      if (xmlElement.HasAttribute("IsAlimonyOrMaintenance"))
        this.IsAlimonyOrMaintenance = xmlElement.GetAttribute("IsAlimonyOrMaintenance") == "Y";
      if (xmlElement.HasAttribute("IsRentalIncome"))
        this.IsRentalIncome = xmlElement.GetAttribute("IsRentalIncome") == "Y";
      if (xmlElement.HasAttribute("IsChildSupport"))
        this.IsChildSupport = xmlElement.GetAttribute("IsChildSupport") == "Y";
      if (xmlElement.HasAttribute("IsOtherNonEmployment"))
        this.IsOtherNonEmployment = xmlElement.GetAttribute("IsOtherNonEmployment") == "Y";
      if (!xmlElement.HasAttribute("OtherNonEmploymentDescription"))
        return;
      this.OtherNonEmploymentDescription = xmlElement.GetAttribute("OtherNonEmploymentDescription");
    }
  }
}
