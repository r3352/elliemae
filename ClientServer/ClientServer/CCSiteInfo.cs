// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CCSiteInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class CCSiteInfo
  {
    private string url = string.Empty;
    private string id = string.Empty;
    private string siteId = string.Empty;
    private bool useParentInfo;

    public string Url
    {
      get => this.url;
      set => this.url = value;
    }

    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    public string SiteId
    {
      get => this.siteId;
      set => this.siteId = value;
    }

    public bool UseParentInfo
    {
      get => this.useParentInfo;
      set => this.useParentInfo = value;
    }

    public CCSiteInfo()
    {
    }

    public CCSiteInfo(string id, string siteId = "�", bool useParentInfo = false, string url = "�")
    {
      this.id = id;
      this.siteId = siteId;
      this.useParentInfo = useParentInfo;
    }
  }
}
