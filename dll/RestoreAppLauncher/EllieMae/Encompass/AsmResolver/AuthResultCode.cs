// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AuthResultCode
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public enum AuthResultCode
  {
    Success = 0,
    UseridNotFound = 1,
    InvalidPwd = 2,
    NullInstallationURL = 3,
    ServerInternalError = 4,
    AccountDisabled = 5,
    UnhandledException = 200, // 0x000000C8
    OfflineMode = 201, // 0x000000C9
  }
}
