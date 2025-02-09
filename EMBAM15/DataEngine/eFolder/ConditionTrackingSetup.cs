// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.ConditionTrackingSetup
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
  public abstract class ConditionTrackingSetup : CollectionBase, IXmlSerializable
  {
    public abstract ConditionType ConditionType { get; }

    public int Add(ConditionTemplate template)
    {
      if (template.ConditionType != this.ConditionType)
        throw new ArgumentException("The specified condition is of the wrong type for this set.");
      ConditionTemplate byName = this.GetByName(template.Name);
      if (byName != null)
        this.Remove(byName);
      return this.List.Add((object) template);
    }

    public ConditionTemplate this[int index] => this.GetByIndex(index);

    public ConditionTemplate this[string guid] => this.GetByID(guid);

    public ConditionTemplate GetByIndex(int index) => (ConditionTemplate) this.List[index];

    public ConditionTemplate GetByName(string name)
    {
      foreach (ConditionTemplate byName in (IEnumerable) this.List)
      {
        if (byName.Name == name)
          return byName;
      }
      return (ConditionTemplate) null;
    }

    public ConditionTemplate GetByID(string guid)
    {
      foreach (ConditionTemplate byId in (IEnumerable) this.List)
      {
        if (byId.Guid == guid)
          return byId;
      }
      return (ConditionTemplate) null;
    }

    public bool Contains(ConditionTemplate template) => this.List.Contains((object) template);

    public bool Contains(string name) => this.GetByName(name) != null;

    public void Remove(ConditionTemplate template) => this.List.Remove((object) template);

    public void Remove(string name)
    {
      ConditionTemplate byName = this.GetByName(name);
      if (byName == null)
        return;
      this.Remove(byName);
    }

    public ConditionTemplate[] ToArray()
    {
      return (ConditionTemplate[]) new ArrayList((ICollection) this.List).ToArray(typeof (ConditionTemplate));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.Count; ++index)
        info.AddValue(index.ToString(), (object) this[index]);
    }
  }
}
