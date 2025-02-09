// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ServerMajorUpdateInfo
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ServerMajorUpdateInfo : UpdateInfoBase
  {
    public ServerMajorUpdateInfo(
      string updateFile,
      string updateUrl,
      string description,
      DateTime releaseDate)
      : base(updateFile, updateUrl, description, releaseDate)
    {
      if (!this.UpdateFile.ToLower().EndsWith(".emzip"))
        return;
      this.ServerRestartRequired = true;
    }

    public ServerMajorUpdateInfo(string xml)
      : base(xml)
    {
      if (!this.UpdateFile.ToLower().EndsWith(".emzip"))
        return;
      this.ServerRestartRequired = false;
    }
  }
}
