// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LoginErrorType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("457D85B5-9125-4803-9CA1-EDA33AA8A888")]
  public enum LoginErrorType
  {
    UserNotFound = 1,
    InvalidPassword = 2,
    UserDiabled = 4,
    LoginsDisabled = 5,
    ServerError = 6,
    UserLocked = 7,
    InvalidPersona = 8,
    ConcurrentEditingOfflineNotAllowed = 9,
    IPBlocked = 10, // 0x0000000A
    ServerBusy = 12, // 0x0000000C
    APIUserRestricted = 13, // 0x0000000D
  }
}
