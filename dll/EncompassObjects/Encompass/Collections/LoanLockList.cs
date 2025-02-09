// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LoanLockList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class LoanLockList : ListBase, ILoanLockList
  {
    public LoanLockList()
      : base(typeof (LoanLock))
    {
    }

    public LoanLockList(IList source)
      : base(typeof (LoanLock), source)
    {
    }

    public LoanLock this[int index]
    {
      get => (LoanLock) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(LoanLock value) => this.List.Add((object) value);

    public bool Contains(LoanLock value) => this.List.Contains((object) value);

    public int IndexOf(LoanLock value) => this.List.IndexOf((object) value);

    public void Insert(int index, LoanLock value) => this.List.Insert(index, (object) value);

    public void Remove(LoanLock value) => this.List.Remove((object) value);

    public LoanLock[] ToArray()
    {
      LoanLock[] array = new LoanLock[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
