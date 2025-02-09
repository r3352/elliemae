// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IBorrowerPair
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for BorrowerPair class.</summary>
  /// <exclude />
  [Guid("11949A22-A2EF-4820-A683-67ABE33E5415")]
  public interface IBorrowerPair
  {
    Borrower Borrower { get; }

    Borrower CoBorrower { get; }

    string ToString();

    bool Equals(object obj);
  }
}
