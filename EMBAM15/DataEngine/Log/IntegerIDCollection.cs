// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.IntegerIDCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class IntegerIDCollection : IEnumerable
  {
    private ObjectIDCollection ids;

    public IntegerIDCollection(LogRecordBase logRecord)
    {
      this.ids = new ObjectIDCollection(logRecord);
    }

    public IntegerIDCollection(LogRecordBase logRecord, XmlElement parentElement, string groupName)
    {
      this.ids = new ObjectIDCollection(logRecord, parentElement, groupName);
    }

    public void Add(int id) => this.ids.Add(string.Concat((object) id));

    public void AddRange(int[] ids)
    {
      foreach (int id in ids)
        this.Add(id);
    }

    public void Remove(int id) => this.ids.Remove(string.Concat((object) id));

    public void Replace(int[] ids)
    {
      string[] ids1 = new string[ids.Length];
      for (int index = 0; index < ids1.Length; ++index)
        ids1[index] = string.Concat((object) ids[index]);
      this.ids.Replace(ids1);
    }

    public void Clear() => this.ids.Clear();

    public bool Contains(int value) => this.ids.Contains(string.Concat((object) value));

    public int this[int index] => int.Parse(this.ids[index]);

    public int Count => this.ids.Count;

    public IEnumerator GetEnumerator()
    {
      ArrayList arrayList = new ArrayList();
      foreach (string id in this.ids)
        arrayList.Add((object) int.Parse(id));
      return arrayList.GetEnumerator();
    }

    public int[] ToArray()
    {
      ArrayList arrayList = new ArrayList();
      foreach (string id in this.ids)
        arrayList.Add((object) int.Parse(id));
      return (int[]) arrayList.ToArray(typeof (int));
    }

    public void ToXml(XmlElement e, string groupName) => this.ids.ToXml(e, groupName);
  }
}
