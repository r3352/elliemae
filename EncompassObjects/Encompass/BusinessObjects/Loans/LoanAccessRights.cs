// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAccessRights
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Enumerates the rights that a user may have on a loan file.
  /// </summary>
  [Guid("37FF0764-1C1A-44ad-906A-33F4ECF7699B")]
  public enum LoanAccessRights
  {
    /// <summary>No access</summary>
    None,
    /// <summary>Read-only access</summary>
    ReadOnly,
    /// <summary>Read and write access</summary>
    ReadWrite,
    /// <summary>Read, write and reassignment rights</summary>
    Full,
  }
}
