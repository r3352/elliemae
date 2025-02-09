// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalUrlList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalUrlList : ListBase, IExternalUrlList
  {
    public ExternalUrlList()
      : base(typeof (ExternalUrl))
    {
    }

    public ExternalUrlList(IList source)
      : base(typeof (ExternalUrl), source)
    {
    }

    public ExternalUrl this[int index]
    {
      get => (ExternalUrl) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalUrl value) => this.List.Add((object) value);

    public bool Contains(ExternalUrl value) => this.List.Contains((object) value);

    public void Remove(ExternalUrl value) => this.List.Remove((object) value);

    public ExternalUrl[] ToArray()
    {
      ExternalUrl[] array = new ExternalUrl[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
