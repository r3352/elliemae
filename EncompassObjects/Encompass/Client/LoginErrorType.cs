// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LoginErrorType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Enumeration of the possible login failure reasons.</summary>
  [Guid("457D85B5-9125-4803-9CA1-EDA33AA8A888")]
  public enum LoginErrorType
  {
    /// <summary>UserID is invalid</summary>
    UserNotFound = 1,
    /// <summary>Password provided does not match stored value</summary>
    InvalidPassword = 2,
    /// <summary>User's account is disabled</summary>
    UserDiabled = 4,
    /// <summary>All logins are currently disabled on the server</summary>
    LoginsDisabled = 5,
    /// <summary>Internal server failure during login</summary>
    ServerError = 6,
    /// <summary>User's account is locked</summary>
    UserLocked = 7,
    /// <summary>User's persona is not defined properly</summary>
    InvalidPersona = 8,
    /// <summary>The user is not allowed to concurrently edit while offline</summary>
    ConcurrentEditingOfflineNotAllowed = 9,
    /// <summary>User's IP address has been blocked</summary>
    IPBlocked = 10, // 0x0000000A
    /// <summary>Server is currently busy</summary>
    ServerBusy = 12, // 0x0000000C
    /// <summary>API user cannot login</summary>
    APIUserRestricted = 13, // 0x0000000D
  }
}
