// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationDocument
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationDocument : LogRecordBase
  {
    private VerificationTimelineType timelineType;
    private string docName = string.Empty;
    private DateTime currentDate;
    private DateTime expirationDate;

    public VerificationDocument(XmlElement e)
    {
      if (e.HasAttribute("GUID"))
        this.SetGuid(e.GetAttribute("GUID"));
      if (e.HasAttribute("CreatedOn"))
        this.Date = Utils.ParseDate((object) e.GetAttribute("CreatedOn"));
      if (!e.HasAttribute(nameof (TimelineType)))
        return;
      switch (e.GetAttribute(nameof (TimelineType)))
      {
        case "Employment":
          this.TimelineType = VerificationTimelineType.Employment;
          break;
        case "Income":
          this.TimelineType = VerificationTimelineType.Income;
          break;
        case "Asset":
          this.TimelineType = VerificationTimelineType.Asset;
          break;
        case "Obligation":
          this.TimelineType = VerificationTimelineType.Obligation;
          break;
        default:
          this.TimelineType = VerificationTimelineType.None;
          break;
      }
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode("FIELD");
      if (xmlElement == null)
        return;
      if (xmlElement.HasAttribute("DocumentName"))
        this.DocName = xmlElement.GetAttribute("DocumentName");
      if (xmlElement.HasAttribute(nameof (CurrentDate)))
        this.CurrentDate = xmlElement.GetAttribute(nameof (CurrentDate)) == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) xmlElement.GetAttribute(nameof (CurrentDate)));
      if (!xmlElement.HasAttribute(nameof (ExpirationDate)))
        return;
      this.ExpirationDate = xmlElement.GetAttribute(nameof (ExpirationDate)) == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) xmlElement.GetAttribute(nameof (ExpirationDate)));
    }

    public VerificationDocument()
    {
    }

    public VerificationTimelineType TimelineType
    {
      get => this.timelineType;
      set => this.timelineType = value;
    }

    public string DocName
    {
      get => this.docName;
      set => this.docName = value;
    }

    public DateTime CurrentDate
    {
      get => this.currentDate;
      set => this.currentDate = value;
    }

    public DateTime ExpirationDate
    {
      get => this.expirationDate;
      set => this.expirationDate = value;
    }
  }
}
