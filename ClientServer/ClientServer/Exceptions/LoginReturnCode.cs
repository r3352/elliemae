// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.LoginReturnCode
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  public enum LoginReturnCode
  {
    SUCCESS = 0,
    USERID_NOT_FOUND = 1,
    PASSWORD_MISMATCH = 2,
    USER_DISABLED = 4,
    LOGIN_DISABLED = 5,
    SERVER_FAILURE = 6,
    USER_LOCKED = 7,
    PERSONA_NOT_FOUND = 8,
    Concurrent_Editing_Offline_Not_Allowed = 9,
    IP_Blocked = 10, // 0x0000000A
    TPO_LOGIN_RESTRICTED = 11, // 0x0000000B
    SERVER_BUSY = 12, // 0x0000000C
    API_USER_RESTRICTED = 13, // 0x0000000D
    Concurrent_User_Not_Allowed = 14, // 0x0000000E
    Allow_Mobile_Disabled = 15, // 0x0000000F
    AccessToken_Required_For_MFA_Enabled = 16, // 0x00000010
    SSO_USER_PASSWORD_NOT_ALLOWED = 17, // 0x00000011
    USER_LOGIN_DISABLED = 18, // 0x00000012
  }
}
