// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LockType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Defines the possible types of locks on a loan file.</summary>
  [Guid("32B6B179-478C-4a44-AF8B-1737A3271D43")]
  public enum LockType
  {
    /// <summary>Loan is not locked</summary>
    None,
    /// <summary>Loan is locked for editing in Encompass</summary>
    Edit,
    /// <summary>Loan has been downloaded to an offline system</summary>
    Download,
  }
}
