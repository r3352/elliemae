// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CustomMilestoneList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class CustomMilestoneList : CollectionBase
  {
    public CustomMilestone this[int index] => (CustomMilestone) this.List[index];

    public void Add(CustomMilestone ms) => this.List.Add((object) ms);

    public void AddRange(ICollection items)
    {
      foreach (object obj in (IEnumerable) items)
        this.List.Add(obj);
    }

    public void Insert(int index, CustomMilestone ms) => this.List.Insert(index, (object) ms);

    public void MarkArchivedMilestones(Hashtable archivedGUID)
    {
      if (this.List == null)
        return;
      for (int index = 0; index < this.List.Count; ++index)
      {
        CustomMilestone customMilestone = (CustomMilestone) this.List[index];
        if (archivedGUID.ContainsKey((object) customMilestone.MilestoneID))
          customMilestone.Archived = true;
      }
    }

    public void Swap(int low, int high)
    {
      object obj = this.List[low];
      this.List[low] = this.List[high];
      this.List[high] = obj;
    }

    public void Reverse(int index, int count) => this.InnerList.Reverse(index, count);

    public void Remove(CustomMilestone ms) => this.List.Remove((object) ms);

    protected override void OnValidate(object value)
    {
      if (!(value is CustomMilestone))
        throw new ArgumentException();
    }

    public bool ContainsGuid(string guid)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (this[index].MilestoneID == guid)
          return true;
      }
      return false;
    }
  }
}
