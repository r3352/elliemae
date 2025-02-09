// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ExternalUserRoles
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Defines possible roles an external user can represent</summary>
  public enum ExternalUserRoles : byte
  {
    /// <summary>Indicates the user has no role</summary>
    None = 0,
    /// <summary>Indicates the user is assigned with loan officer role</summary>
    LoanOfficer = 1,
    /// <summary>Indicates the user is assigned with loan processor role</summary>
    LoanProcessor = 2,
    /// <summary>Indicates the user is assigned with manager role</summary>
    Manager = 4,
    /// <summary>Indicates the user is assigned with administrator role</summary>
    Administrator = 8,
  }
}
