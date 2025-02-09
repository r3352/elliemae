// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.PipelineDataList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class PipelineDataList : ListBase, IPipelineDataList
  {
    public PipelineDataList()
      : base(typeof (PipelineData))
    {
    }

    public PipelineDataList(IList source)
      : base(typeof (PipelineData), source)
    {
    }

    public PipelineData this[int index]
    {
      get => (PipelineData) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(PipelineData value) => this.List.Add((object) value);

    public bool Contains(PipelineData value) => this.List.Contains((object) value);

    public int IndexOf(PipelineData value) => this.List.IndexOf((object) value);

    public void Insert(int index, PipelineData value) => this.List.Insert(index, (object) value);

    public void Remove(PipelineData value) => this.List.Remove((object) value);

    public PipelineData[] ToArray()
    {
      PipelineData[] array = new PipelineData[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
