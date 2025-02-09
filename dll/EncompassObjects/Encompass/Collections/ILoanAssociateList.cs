// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ILoanAssociateList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [Guid("4242F4C8-CEB8-40f8-875D-61A8280F2599")]
  public interface ILoanAssociateList
  {
    LoanAssociate this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(LoanAssociate value);

    bool Contains(LoanAssociate value);

    int IndexOf(LoanAssociate value);

    void Insert(int index, LoanAssociate value);

    void Remove(LoanAssociate value);

    LoanAssociate[] ToArray();

    IEnumerator GetEnumerator();
  }
}
