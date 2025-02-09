// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.FieldDescriptorList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class FieldDescriptorList : ListBase, IFieldDescriptorList
  {
    public FieldDescriptorList()
      : base(typeof (FieldDescriptor))
    {
    }

    public FieldDescriptorList(IList source)
      : base(typeof (FieldDescriptor), source)
    {
    }

    public FieldDescriptor this[int index]
    {
      get => (FieldDescriptor) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(FieldDescriptor value) => this.List.Add((object) value);

    public bool Contains(FieldDescriptor value) => this.List.Contains((object) value);

    public int IndexOf(FieldDescriptor value) => this.List.IndexOf((object) value);

    public void Insert(int index, FieldDescriptor value) => this.List.Insert(index, (object) value);

    public void Remove(FieldDescriptor value) => this.List.Remove((object) value);

    public FieldDescriptor[] ToArray()
    {
      FieldDescriptor[] array = new FieldDescriptor[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
