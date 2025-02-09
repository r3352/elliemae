// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LoanFolderList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class LoanFolderList : ListBase, ILoanFolderList
  {
    public LoanFolderList()
      : base(typeof (LoanFolder))
    {
    }

    public LoanFolderList(IList source)
      : base(typeof (LoanFolder), source)
    {
    }

    public LoanFolder this[int index]
    {
      get => (LoanFolder) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(LoanFolder value) => this.List.Add((object) value);

    public bool Contains(LoanFolder value) => this.List.Contains((object) value);

    public int IndexOf(LoanFolder value) => this.List.IndexOf((object) value);

    public void Insert(int index, LoanFolder value) => this.List.Insert(index, (object) value);

    public void Remove(LoanFolder value) => this.List.Remove((object) value);

    public LoanFolder[] ToArray()
    {
      LoanFolder[] array = new LoanFolder[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
