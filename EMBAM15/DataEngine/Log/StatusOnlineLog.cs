// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.StatusOnlineLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class StatusOnlineLog : SystemLog
  {
    public new static readonly string XmlType = "Status";
    private string creator = string.Empty;
    private TrackedItemList itemList = new TrackedItemList();

    public StatusOnlineLog(DateTime date)
      : base(date)
    {
    }

    public StatusOnlineLog(LogList log, XmlElement e)
      : base(log, e)
    {
      this.Creator = new AttributeReader(e).GetString(nameof (Creator));
      XmlNodeList xmlNodeList = e.SelectNodes("StringList");
      for (int i = 0; i < xmlNodeList.Count; ++i)
      {
        AttributeReader attributeReader = new AttributeReader((XmlElement) xmlNodeList[i]);
        this.ItemList.Add((object) new string[2]
        {
          attributeReader.GetString("subItem1"),
          attributeReader.GetString("subItem2")
        });
      }
      this.MarkAsClean();
    }

    public string Creator
    {
      get => this.creator;
      set
      {
        this.creator = value;
        this.MarkAsDirty();
      }
    }

    public TrackedItemList ItemList => this.itemList;

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
      attributeWriter.Write("Type", (object) StatusOnlineLog.XmlType);
      attributeWriter.Write("Creator", (object) this.Creator);
      for (int index = 0; index < this.ItemList.Count; ++index)
      {
        string[] strArray = (string[]) this.ItemList[index];
        XmlElement element = e.OwnerDocument.CreateElement("StringList");
        element.SetAttribute("subItem1", strArray[0]);
        element.SetAttribute("subItem2", strArray[1]);
        e.AppendChild((XmlNode) element);
      }
    }
  }
}
