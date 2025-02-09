// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WebCenterImpotStatus
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class WebCenterImpotStatus
  {
    private string importID = string.Empty;
    private string emSiteID = string.Empty;
    private string loanGUID = string.Empty;
    private DateTime importDateTime = DateTime.MinValue;
    private string whoImports = string.Empty;

    public WebCenterImpotStatus()
    {
    }

    public WebCenterImpotStatus(
      string importID,
      string emSiteID,
      string loanGUID,
      DateTime importDateTime,
      string whoImports)
    {
      this.importID = importID;
      this.emSiteID = emSiteID;
      this.loanGUID = loanGUID;
      this.importDateTime = importDateTime;
      this.whoImports = whoImports;
    }

    public string ImportID
    {
      set => this.importID = value;
      get => this.importID;
    }

    public string EMSiteID
    {
      set => this.emSiteID = value;
      get => this.emSiteID;
    }

    public string LoanGUID
    {
      set => this.loanGUID = value;
      get => this.loanGUID;
    }

    public DateTime ImportDateTime
    {
      set => this.importDateTime = value;
      get => this.importDateTime;
    }

    public string WhoImports
    {
      set => this.whoImports = value;
      get => this.whoImports;
    }
  }
}
