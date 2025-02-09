// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanFolders
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for LoanFolders class.</summary>
  /// <exclude />
  [Guid("765D5CE7-19D1-4543-9B5D-D064079222FF")]
  public interface ILoanFolders
  {
    int Count { get; }

    LoanFolder this[string name] { get; }

    LoanFolder Add(string name);

    void Remove(LoanFolder folder);

    void Refresh();

    IEnumerator GetEnumerator();

    void RebuildAll();
  }
}
