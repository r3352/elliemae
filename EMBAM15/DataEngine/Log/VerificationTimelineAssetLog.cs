// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationTimelineAssetLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationTimelineAssetLog : VerificationTimelineLog
  {
    private bool isBankStatements;
    private bool isRentalPropertyIncome;
    private bool isMutualFunds;
    private bool isOther;
    private string otherDescription = string.Empty;

    public VerificationTimelineAssetLog(XmlElement e)
      : base(e)
    {
    }

    public VerificationTimelineAssetLog()
    {
    }

    public bool IsBankStatements
    {
      set => this.isBankStatements = value;
      get => this.isBankStatements;
    }

    public bool IsRentalPropertyIncome
    {
      set => this.isRentalPropertyIncome = value;
      get => this.isRentalPropertyIncome;
    }

    public bool IsMutualFunds
    {
      set => this.isMutualFunds = value;
      get => this.isMutualFunds;
    }

    public bool IsOther
    {
      set => this.isOther = value;
      get => this.isOther;
    }

    public string OtherDescription
    {
      set => this.otherDescription = value;
      get => this.otherDescription;
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.IsBankStatements)
        str += "Bank Statements";
      if (this.IsRentalPropertyIncome)
        str = str + (str != string.Empty ? "," : "") + "Rental Property Income - Schedule E";
      if (this.IsMutualFunds)
        str = str + (str != string.Empty ? "," : "") + "Mutual Funds";
      if (this.IsOther)
        str = str + (str != string.Empty ? "," : "") + "Other";
      return str;
    }

    public void SetStatusToXml(XmlElement fieldXml)
    {
      fieldXml.SetAttribute("IsBankStatements", this.IsBankStatements ? "Y" : "N");
      fieldXml.SetAttribute("IsRentalPropertyIncome", this.IsRentalPropertyIncome ? "Y" : "N");
      fieldXml.SetAttribute("IsMutualFunds", this.IsMutualFunds ? "Y" : "N");
      fieldXml.SetAttribute("IsOther", this.IsOther ? "Y" : "N");
      fieldXml.SetAttribute("OtherDescription", this.OtherDescription);
    }

    public void GetStatusFromXml(XmlElement e)
    {
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode("STATUS");
      if (xmlElement == null)
        return;
      if (xmlElement.HasAttribute("IsBankStatements"))
        this.IsBankStatements = xmlElement.GetAttribute("IsBankStatements") == "Y";
      if (xmlElement.HasAttribute("IsRentalPropertyIncome"))
        this.IsRentalPropertyIncome = xmlElement.GetAttribute("IsRentalPropertyIncome") == "Y";
      if (xmlElement.HasAttribute("IsMutualFunds"))
        this.IsMutualFunds = xmlElement.GetAttribute("IsMutualFunds") == "Y";
      if (xmlElement.HasAttribute("IsOther"))
        this.IsOther = xmlElement.GetAttribute("IsOther") == "Y";
      if (!xmlElement.HasAttribute("OtherDescription"))
        return;
      this.OtherDescription = xmlElement.GetAttribute("OtherDescription");
    }
  }
}
