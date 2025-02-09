// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalNotesList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalNotesList : ListBase, IExternalNotesList
  {
    public ExternalNotesList()
      : base(typeof (ExternalNote))
    {
    }

    public ExternalNotesList(IList source)
      : base(typeof (ExternalNote), source)
    {
    }

    public ExternalNote this[int index]
    {
      get => (ExternalNote) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalNote value) => this.List.Add((object) value);

    public bool Contains(ExternalNote value) => this.List.Contains((object) value);

    public void Remove(ExternalNote value) => this.List.Remove((object) value);

    public ExternalNote[] ToArray()
    {
      ExternalNote[] array = new ExternalNote[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
