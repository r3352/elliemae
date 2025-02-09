// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalDBAList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalDBAList : ListBase, IExternalDBAList
  {
    public ExternalDBAList()
      : base(typeof (ExternalDBAName))
    {
    }

    public ExternalDBAList(IList source)
      : base(typeof (ExternalDBAName), source)
    {
    }

    public ExternalDBAName this[int index]
    {
      get => (ExternalDBAName) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalDBAName value) => this.List.Add((object) value);

    public bool Contains(ExternalDBAName value) => this.List.Contains((object) value);

    public void Remove(ExternalDBAName value) => this.List.Remove((object) value);

    public ExternalDBAName[] ToArray()
    {
      ExternalDBAName[] array = new ExternalDBAName[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
