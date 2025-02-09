// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.TemplateEntryList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class TemplateEntryList : ListBase, ITemplateEntryList
  {
    public TemplateEntryList()
      : base(typeof (TemplateEntry))
    {
    }

    public TemplateEntryList(IList source)
      : base(typeof (TemplateEntry), source)
    {
    }

    public TemplateEntry this[int index]
    {
      get => (TemplateEntry) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(TemplateEntry value) => this.List.Add((object) value);

    public bool Contains(TemplateEntry value) => this.List.Contains((object) value);

    public int IndexOf(TemplateEntry value) => this.List.IndexOf((object) value);

    public void Insert(int index, TemplateEntry value) => this.List.Insert(index, (object) value);

    public void Remove(TemplateEntry value) => this.List.Remove((object) value);

    public TemplateEntry[] ToArray()
    {
      TemplateEntry[] array = new TemplateEntry[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
