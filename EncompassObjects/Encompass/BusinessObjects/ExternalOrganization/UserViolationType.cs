// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.UserViolationType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Enumeration of the possible violation reasons.</summary>
  [Guid("85927A66-B929-48C8-9A65-6EEA98EDB1AB")]
  public enum UserViolationType
  {
    /// <summary>Password provided does not match stored value</summary>
    LoginCredentialNotFound = 1,
    /// <summary>User's account is disabled</summary>
    AccountDisabled = 2,
    /// <summary>company is on watchlist</summary>
    CompanyOnWatchlist = 3,
    /// <summary>Organization is disabled</summary>
    OrganizationDisabled = 4,
    /// <summary>External User address is invalid</summary>
    InvalidExternalUserAddress = 5,
  }
}
