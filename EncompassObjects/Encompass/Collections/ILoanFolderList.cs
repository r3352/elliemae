// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ILoanFolderList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for LoanFolderList class.</summary>
  /// <exclude />
  [Guid("08D348BF-7EBC-4c68-9EFF-3A165ADD6389")]
  public interface ILoanFolderList
  {
    LoanFolder this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(LoanFolder value);

    bool Contains(LoanFolder value);

    int IndexOf(LoanFolder value);

    void Insert(int index, LoanFolder value);

    void Remove(LoanFolder value);

    LoanFolder[] ToArray();

    IEnumerator GetEnumerator();
  }
}
