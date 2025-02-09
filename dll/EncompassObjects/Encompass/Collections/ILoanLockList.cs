// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ILoanLockList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("A011E752-3D0C-48f3-B449-7976DFA40C7A")]
  public interface ILoanLockList
  {
    LoanLock this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(LoanLock value);

    bool Contains(LoanLock value);

    int IndexOf(LoanLock value);

    void Insert(int index, LoanLock value);

    void Remove(LoanLock value);

    LoanLock[] ToArray();

    IEnumerator GetEnumerator();
  }
}
