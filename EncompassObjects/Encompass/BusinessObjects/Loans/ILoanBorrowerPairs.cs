// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanBorrowerPairs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for LoanBorrowerPairs class.</summary>
  /// <exclude />
  [Guid("E8AF7367-0FF0-4aa2-A5EB-08DE70869FA3")]
  public interface ILoanBorrowerPairs
  {
    BorrowerPair this[int index] { get; }

    int Count { get; }

    BorrowerPair Add();

    void Remove(BorrowerPair pair);

    void Swap(BorrowerPair pairA, BorrowerPair pairB);

    BorrowerPair Current { get; set; }

    IEnumerator GetEnumerator();

    void Refresh();
  }
}
