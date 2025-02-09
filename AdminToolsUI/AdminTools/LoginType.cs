// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoginType
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  [Flags]
  public enum LoginType
  {
    None = 0,
    Offline = 1,
    Server = 2,
    All = Server | Offline, // 0x00000003
  }
}
