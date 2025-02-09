// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSiteUrl
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalSiteUrl : IExternalSiteUrl
  {
    private string url;
    private string siteId;
    private DateTime dateAdded;
    private int urlID = -1;

    public ExternalSiteUrl(string url, string siteId)
    {
      this.url = url;
      this.siteId = this.SiteId;
      this.dateAdded = DateTime.Now;
    }

    internal ExternalSiteUrl(ExternalOrgURL externalOrgUrl)
    {
      this.url = externalOrgUrl.URL;
      this.urlID = externalOrgUrl.URLID;
      this.siteId = externalOrgUrl.siteId;
      this.dateAdded = externalOrgUrl.DateAdded;
    }

    public DateTime DateAdded
    {
      get => this.dateAdded;
      set => this.dateAdded = value;
    }

    public string URL
    {
      get => this.url;
      set => this.url = value;
    }

    public string SiteId
    {
      get => this.siteId;
      set => this.siteId = value;
    }

    public int URLID => this.urlID;
  }
}
