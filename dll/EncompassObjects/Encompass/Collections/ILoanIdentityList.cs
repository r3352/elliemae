// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ILoanIdentityList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("5B1122CB-7A16-4217-BCF2-293AB892E870")]
  public interface ILoanIdentityList
  {
    LoanIdentity this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(LoanIdentity value);

    bool Contains(LoanIdentity value);

    int IndexOf(LoanIdentity value);

    void Insert(int index, LoanIdentity value);

    void Remove(LoanIdentity value);

    LoanIdentity[] ToArray();

    IEnumerator GetEnumerator();
  }
}
