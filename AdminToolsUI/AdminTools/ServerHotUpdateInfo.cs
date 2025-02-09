// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ServerHotUpdateInfo
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ServerHotUpdateInfo : UpdateInfoBase
  {
    public ServerHotUpdateInfo(
      string updateFile,
      string updateUrl,
      string description,
      DateTime releaseDate)
      : base(updateFile, updateUrl, description, releaseDate)
    {
      if (!this.UpdateFile.ToLower().EndsWith(".cemzip"))
        return;
      this.ServerRestartRequired = false;
    }

    public ServerHotUpdateInfo(string xml)
      : base(xml)
    {
      if (!this.UpdateFile.ToLower().EndsWith(".cemzip"))
        return;
      this.ServerRestartRequired = false;
    }
  }
}
