// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ReportingFieldDescriptorList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Reporting;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ReportingFieldDescriptorList : ListBase, IReportingFieldDescriptorList
  {
    public ReportingFieldDescriptorList()
      : base(typeof (ReportingFieldDescriptor))
    {
    }

    public ReportingFieldDescriptorList(IList source)
      : base(typeof (ReportingFieldDescriptor), source)
    {
    }

    public ReportingFieldDescriptor this[int index]
    {
      get => (ReportingFieldDescriptor) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ReportingFieldDescriptor value) => this.List.Add((object) value);

    public bool Contains(ReportingFieldDescriptor value) => this.List.Contains((object) value);

    public int IndexOf(ReportingFieldDescriptor value) => this.List.IndexOf((object) value);

    public void Insert(int index, ReportingFieldDescriptor value)
    {
      this.List.Insert(index, (object) value);
    }

    public void Remove(ReportingFieldDescriptor value) => this.List.Remove((object) value);

    public ReportingFieldDescriptor[] ToArray()
    {
      ReportingFieldDescriptor[] array = new ReportingFieldDescriptor[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
