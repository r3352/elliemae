// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.UserViolationType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  [Guid("85927A66-B929-48C8-9A65-6EEA98EDB1AB")]
  public enum UserViolationType
  {
    LoginCredentialNotFound = 1,
    AccountDisabled = 2,
    CompanyOnWatchlist = 3,
    OrganizationDisabled = 4,
    InvalidExternalUserAddress = 5,
  }
}
