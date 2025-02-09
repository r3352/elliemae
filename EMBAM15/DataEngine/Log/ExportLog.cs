// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ExportLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class ExportLog : LogRecordBase
  {
    public static readonly string XmlType = "Export";
    private string exportedBy = "";
    private string exportedByFullName = "";
    private ExportLog.ExportCategory category;
    private TrackedItemList itemList = new TrackedItemList();

    public ExportLog(DateTime date)
      : base(date, "")
    {
    }

    public ExportLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.exportedBy = attributeReader.GetString(nameof (ExportedBy));
      this.exportedByFullName = attributeReader.GetString(nameof (ExportedByFullName));
      try
      {
        this.category = (ExportLog.ExportCategory) Enum.Parse(typeof (ExportLog.ExportCategory), attributeReader.GetString(nameof (Category)), true);
      }
      catch
      {
      }
      XmlNodeList xmlNodeList = e.SelectNodes("ServiceType");
      for (int i = 0; i < xmlNodeList.Count; ++i)
        this.ItemList.Add((object) new AttributeReader((XmlElement) xmlNodeList[i]).GetString("name"));
      this.MarkAsClean();
    }

    public string ExportedBy
    {
      get => this.exportedBy;
      set
      {
        this.exportedBy = value;
        this.MarkAsDirty();
      }
    }

    public string ExportedByFullName
    {
      get => this.exportedByFullName;
      set
      {
        this.exportedByFullName = value;
        this.MarkAsDirty();
      }
    }

    public ExportLog.ExportCategory Category
    {
      get => this.category;
      set
      {
        this.category = value;
        this.MarkAsDirty();
      }
    }

    public TrackedItemList ItemList => this.itemList;

    public override bool IsLoanOperationalLog => true;

    public override void MarkAsClean()
    {
      base.MarkAsClean();
      this.ItemList.IsDirty = false;
    }

    public override bool IsDirty() => base.IsDirty() || this.ItemList.IsDirty;

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) ExportLog.XmlType);
      attributeWriter.Write("ExportedBy", (object) this.ExportedBy);
      attributeWriter.Write("ExportedByFullName", (object) this.ExportedByFullName);
      attributeWriter.Write("Category", (object) this.Category.ToString());
      for (int index = 0; index < this.ItemList.Count; ++index)
      {
        string str = (string) this.ItemList[index];
        XmlElement element = e.OwnerDocument.CreateElement("ServiceType");
        element.SetAttribute("name", str);
        e.AppendChild((XmlNode) element);
      }
    }

    public enum ExportCategory
    {
      None,
      GSEServices,
      ComplianceServices,
    }
  }
}
