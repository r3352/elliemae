// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalFeesList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalFeesList : ListBase, IExternalFeesList
  {
    public ExternalFeesList()
      : base(typeof (ExternalFees))
    {
    }

    public ExternalFeesList(IList source)
      : base(typeof (ExternalFees), source)
    {
    }

    public ExternalFees this[int index]
    {
      get => (ExternalFees) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalFees value) => this.List.Add((object) value);

    public bool Contains(ExternalFees value) => this.List.Contains((object) value);

    public void Remove(ExternalFees value) => this.List.Remove((object) value);

    public ExternalFees[] ToArray()
    {
      ExternalFees[] array = new ExternalFees[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
