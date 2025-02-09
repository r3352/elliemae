// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AuthResultCode
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

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
