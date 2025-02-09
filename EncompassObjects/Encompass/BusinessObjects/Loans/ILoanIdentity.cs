// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanIdentity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for LoanIdentity class.</summary>
  /// <exclude />
  [System.Runtime.InteropServices.Guid("A43B8F5E-DF9C-4d79-A0AA-2E18F5BC6B19")]
  public interface ILoanIdentity
  {
    string Guid { get; }

    string LoanFolder { get; }

    string LoanName { get; }

    string ToString();
  }
}
