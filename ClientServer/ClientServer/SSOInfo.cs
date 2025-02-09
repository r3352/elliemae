// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SSOInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SSOInfo
  {
    private bool loginAccess;
    private string id = string.Empty;
    private string siteId = string.Empty;
    private bool useParentInfo;

    public bool LoginAccess
    {
      get => this.loginAccess;
      set => this.loginAccess = value;
    }

    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    public bool UseParentInfo
    {
      get => this.useParentInfo;
      set => this.useParentInfo = value;
    }

    public SSOInfo()
    {
      this.useParentInfo = true;
      this.loginAccess = false;
    }

    public SSOInfo(string id, bool useParentInfo = false, bool LoginAccess = false)
    {
      this.id = id;
      this.useParentInfo = useParentInfo;
      this.loginAccess = LoginAccess;
    }
  }
}
