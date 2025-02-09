// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationTimelineEmploymentLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationTimelineEmploymentLog : VerificationTimelineLog
  {
    private bool isFullTime;
    private bool isSeasonal;
    private bool isRetired;
    private bool isPartTime;
    private bool isSelfEmployed;
    private bool isIrregular;
    private bool isMilitary;

    public VerificationTimelineEmploymentLog(XmlElement e)
      : base(e)
    {
    }

    public VerificationTimelineEmploymentLog()
    {
    }

    public bool IsFullTime
    {
      set => this.isFullTime = value;
      get => this.isFullTime;
    }

    public bool IsSeasonal
    {
      set => this.isSeasonal = value;
      get => this.isSeasonal;
    }

    public bool IsRetired
    {
      set => this.isRetired = value;
      get => this.isRetired;
    }

    public bool IsPartTime
    {
      set => this.isPartTime = value;
      get => this.isPartTime;
    }

    public bool IsSelfEmployed
    {
      set => this.isSelfEmployed = value;
      get => this.isSelfEmployed;
    }

    public bool IsIrregular
    {
      set => this.isIrregular = value;
      get => this.isIrregular;
    }

    public bool IsMilitary
    {
      set => this.isMilitary = value;
      get => this.isMilitary;
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.IsFullTime)
        str += "Full Time";
      if (this.IsSeasonal)
        str = str + (str != string.Empty ? "," : "") + "Seasonal";
      if (this.IsRetired)
        str = str + (str != string.Empty ? "," : "") + "Retired";
      if (this.IsPartTime)
        str = str + (str != string.Empty ? "," : "") + "Part-Time";
      if (this.IsSelfEmployed)
        str = str + (str != string.Empty ? "," : "") + "Self-Employed";
      if (this.IsIrregular)
        str = str + (str != string.Empty ? "," : "") + "Irregular";
      if (this.IsMilitary)
        str = str + (str != string.Empty ? "," : "") + "Military";
      return str;
    }

    public void SetStatusToXml(XmlElement fieldXml)
    {
      fieldXml.SetAttribute("IsFullTime", this.IsFullTime ? "Y" : "N");
      fieldXml.SetAttribute("IsSeasonal", this.IsSeasonal ? "Y" : "N");
      fieldXml.SetAttribute("IsRetired", this.IsRetired ? "Y" : "N");
      fieldXml.SetAttribute("IsPartTime", this.IsPartTime ? "Y" : "N");
      fieldXml.SetAttribute("IsSelfEmployed", this.IsSelfEmployed ? "Y" : "N");
      fieldXml.SetAttribute("IsIrregular", this.IsIrregular ? "Y" : "N");
      fieldXml.SetAttribute("IsMilitary", this.IsMilitary ? "Y" : "N");
    }

    public void GetStatusFromXml(XmlElement e)
    {
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode("STATUS");
      if (xmlElement == null)
        return;
      if (xmlElement.HasAttribute("IsFullTime"))
        this.IsFullTime = xmlElement.GetAttribute("IsFullTime") == "Y";
      if (xmlElement.HasAttribute("IsSeasonal"))
        this.IsSeasonal = xmlElement.GetAttribute("IsSeasonal") == "Y";
      if (xmlElement.HasAttribute("IsRetired"))
        this.IsRetired = xmlElement.GetAttribute("IsRetired") == "Y";
      if (xmlElement.HasAttribute("IsPartTime"))
        this.IsPartTime = xmlElement.GetAttribute("IsPartTime") == "Y";
      if (xmlElement.HasAttribute("IsSelfEmployed"))
        this.IsSelfEmployed = xmlElement.GetAttribute("IsSelfEmployed") == "Y";
      if (xmlElement.HasAttribute("IsIrregular"))
        this.IsIrregular = xmlElement.GetAttribute("IsIrregular") == "Y";
      if (!xmlElement.HasAttribute("IsMilitary"))
        return;
      this.IsMilitary = xmlElement.GetAttribute("IsMilitary") == "Y";
    }
  }
}
