// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ObjectIDCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class ObjectIDCollection : IEnumerable
  {
    private LogRecordBase logRecord;
    private ArrayList items = new ArrayList();

    public ObjectIDCollection(LogRecordBase logRecord) => this.logRecord = logRecord;

    public ObjectIDCollection(LogRecordBase logRecord, XmlElement parentElement, string groupName)
    {
      this.logRecord = logRecord;
      if (!(parentElement.SelectSingleNode(groupName) is XmlElement xmlElement))
        return;
      foreach (XmlElement selectNode in xmlElement.SelectNodes("ref"))
        this.items.Add((object) selectNode.GetAttribute("id"));
    }

    internal LogRecordBase LogRecord => this.logRecord;

    public void Add(string id)
    {
      if (this.items.Contains((object) id))
        return;
      this.items.Add((object) id);
      this.logRecord.MarkAsDirty();
    }

    public void Remove(string id)
    {
      if (!this.items.Contains((object) id))
        return;
      this.items.Remove((object) id);
      this.logRecord.MarkAsDirty();
    }

    public void Replace(string[] ids)
    {
      bool flag = this.items.Count.Equals(ids.Length);
      foreach (object id in ids)
      {
        if (!this.items.Contains(id))
          flag = false;
      }
      if (flag)
        return;
      this.items = new ArrayList((ICollection) ids);
      this.logRecord.MarkAsDirty();
    }

    public void Clear()
    {
      if (this.items.Count <= 0)
        return;
      this.items.Clear();
      this.logRecord.MarkAsDirty();
    }

    public bool Contains(string value) => this.items.Contains((object) value);

    public string this[int index] => (string) this.items[index];

    public int Count => this.items.Count;

    public IEnumerator GetEnumerator() => this.items.GetEnumerator();

    public string[] ToArray() => (string[]) this.items.ToArray(typeof (string));

    public void ToXml(XmlElement e, string groupName)
    {
      if (this.items.Count == 0)
        return;
      XmlElement xmlElement = (XmlElement) e.AppendChild((XmlNode) e.OwnerDocument.CreateElement(groupName));
      foreach (string str in this.items)
      {
        XmlElement element = e.OwnerDocument.CreateElement("ref");
        element.SetAttribute("id", str);
        xmlElement.AppendChild((XmlNode) element);
      }
    }
  }
}
