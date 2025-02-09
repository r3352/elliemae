// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.PeerLoanAccessRight
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Enumeration for determining a user's access to loans based on the rights of other users
  /// at the same level of the organization hierarchy (i.e. their peers).
  /// </summary>
  [Guid("C76CAA05-A5C5-4580-A182-31FE21964353")]
  public enum PeerLoanAccessRight
  {
    /// <summary>
    /// User cannot access loans based on assigned rights of other users at the same level in the
    /// hierarchy.
    /// </summary>
    None,
    /// <summary>Allows read-only access to the loans of peers (unless full access
    /// is explicitly assigned).</summary>
    ReadOnly,
    /// <summary>Allows full read and write access to the loans of users at the same
    /// level in the organization hierarchy.</summary>
    ReadWrite,
  }
}
