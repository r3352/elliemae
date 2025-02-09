// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentGroupSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class DocumentGroupSetup : CollectionBase, IXmlSerializable
  {
    public DocumentGroupSetup()
    {
    }

    public DocumentGroupSetup(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.Add((DocumentGroup) info.GetValue(name, typeof (DocumentGroup)));
    }

    public int Add(DocumentGroup group)
    {
      DocumentGroup byName = this.GetByName(group.Name);
      if (byName != null)
        this.Remove(byName);
      return this.List.Add((object) group);
    }

    public DocumentGroup Add(string name)
    {
      DocumentGroup group = new DocumentGroup(name);
      this.Add(group);
      return group;
    }

    public DocumentGroup this[int index] => this.GetByIndex(index);

    public DocumentGroup GetByIndex(int index) => (DocumentGroup) this.List[index];

    public DocumentGroup GetByName(string name)
    {
      foreach (DocumentGroup byName in (IEnumerable) this.List)
      {
        if (byName.Name == name)
          return byName;
      }
      return (DocumentGroup) null;
    }

    public DocumentGroup GetByID(string guid)
    {
      foreach (DocumentGroup byId in (IEnumerable) this.List)
      {
        if (byId.Guid == guid)
          return byId;
      }
      return (DocumentGroup) null;
    }

    public bool Contains(DocumentGroup group) => this.List.Contains((object) group);

    public bool Contains(string name) => this.GetByName(name) != null;

    public void Remove(DocumentGroup group) => this.List.Remove((object) group);

    public void Remove(string name)
    {
      DocumentGroup byName = this.GetByName(name);
      if (byName == null)
        return;
      this.Remove(byName);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.Count; ++index)
        info.AddValue(index.ToString(), (object) this[index]);
    }
  }
}
