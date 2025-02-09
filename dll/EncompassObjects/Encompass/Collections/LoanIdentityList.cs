// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LoanIdentityList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class LoanIdentityList : ListBase, ILoanIdentityList
  {
    public LoanIdentityList()
      : base(typeof (LoanIdentity))
    {
    }

    public LoanIdentityList(IList source)
      : base(typeof (LoanIdentity), source)
    {
    }

    public LoanIdentity this[int index]
    {
      get => (LoanIdentity) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(LoanIdentity value) => this.List.Add((object) value);

    public bool Contains(LoanIdentity value) => this.List.Contains((object) value);

    public int IndexOf(LoanIdentity value) => this.List.IndexOf((object) value);

    public void Insert(int index, LoanIdentity value) => this.List.Insert(index, (object) value);

    public void Remove(LoanIdentity value) => this.List.Remove((object) value);

    public LoanIdentity[] ToArray()
    {
      LoanIdentity[] array = new LoanIdentity[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
