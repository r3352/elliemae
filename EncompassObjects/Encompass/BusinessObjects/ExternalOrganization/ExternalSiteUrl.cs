// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSiteUrl
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External site url</summary>
  public class ExternalSiteUrl : IExternalSiteUrl
  {
    private string url;
    private string siteId;
    private DateTime dateAdded;
    private int urlID = -1;

    /// <summary>Constructor</summary>
    /// <param name="url">url of the site</param>
    /// <param name="siteId">TPO Web center site id.</param>
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

    /// <summary>Gets or sets date added</summary>
    public DateTime DateAdded
    {
      get => this.dateAdded;
      set => this.dateAdded = value;
    }

    /// <summary>Gets or sets URL</summary>
    public string URL
    {
      get => this.url;
      set => this.url = value;
    }

    /// <summary>Gets or sets SiteId</summary>
    public string SiteId
    {
      get => this.siteId;
      set => this.siteId = value;
    }

    /// <summary>Gets internal URLID</summary>
    public int URLID => this.urlID;
  }
}
