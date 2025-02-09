// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ExternalBanksList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class ExternalBanksList : ListBase, IExternalBanksList
  {
    public ExternalBanksList()
      : base(typeof (ExternalBanks))
    {
    }

    public ExternalBanksList(IList source)
      : base(typeof (ExternalBanks), source)
    {
    }

    public ExternalBanks this[int index]
    {
      get => (ExternalBanks) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(ExternalBanks value) => this.List.Add((object) value);

    public bool Contains(ExternalBanks value) => this.List.Contains((object) value);

    public void Remove(ExternalBanks value) => this.List.Remove((object) value);

    public ExternalBanks[] ToArray()
    {
      ExternalBanks[] array = new ExternalBanks[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
