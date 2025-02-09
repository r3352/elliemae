// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.PrintLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class PrintLog : LogRecordBase
  {
    public static readonly string XmlType = "Print";
    private string printedBy = "";
    private string printedByFullName = "";
    private PrintLog.PrintAction action = PrintLog.PrintAction.Print;
    private TrackedItemList itemList = new TrackedItemList();

    public PrintLog(DateTime date)
      : base(date, "")
    {
    }

    public PrintLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.printedBy = attributeReader.GetString(nameof (PrintedBy));
      this.printedByFullName = attributeReader.GetString(nameof (PrintedByFullName));
      try
      {
        this.action = (PrintLog.PrintAction) Enum.Parse(typeof (PrintLog.PrintAction), attributeReader.GetString(nameof (Action)), true);
      }
      catch
      {
      }
      XmlNodeList xmlNodeList = e.SelectNodes("Form");
      for (int i = 0; i < xmlNodeList.Count; ++i)
        this.ItemList.Add((object) new AttributeReader((XmlElement) xmlNodeList[i]).GetString("name"));
      this.MarkAsClean();
    }

    public string PrintedBy
    {
      get => this.printedBy;
      set
      {
        this.printedBy = value;
        this.MarkAsDirty();
      }
    }

    public string PrintedByFullName
    {
      get => this.printedByFullName;
      set
      {
        this.printedByFullName = value;
        this.MarkAsDirty();
      }
    }

    public PrintLog.PrintAction Action
    {
      get => this.action;
      set
      {
        this.action = value;
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
      attributeWriter.Write("Type", (object) PrintLog.XmlType);
      attributeWriter.Write("PrintedBy", (object) this.PrintedBy);
      attributeWriter.Write("PrintedByFullName", (object) this.PrintedByFullName);
      attributeWriter.Write("Action", (object) this.Action.ToString());
      for (int index = 0; index < this.ItemList.Count; ++index)
      {
        string str = (string) this.ItemList[index];
        XmlElement element = e.OwnerDocument.CreateElement("Form");
        element.SetAttribute("name", str);
        e.AppendChild((XmlNode) element);
      }
    }

    public enum PrintAction
    {
      None,
      Print,
      PrintToFile,
      Preview,
    }
  }
}
