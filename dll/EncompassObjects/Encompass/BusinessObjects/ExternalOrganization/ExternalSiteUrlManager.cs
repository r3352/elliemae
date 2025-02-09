// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSiteUrlManager
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Client;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  internal class ExternalSiteUrlManager : SessionBoundObject, IExternalSiteUrlManager
  {
    private IConfigurationManager mngr;
    private ExternalSiteUrl siteUrl;

    internal ExternalSiteUrlManager(Session session, ExternalSiteUrl Url)
      : base(session)
    {
      this.siteUrl = Url;
    }

    public void AddSiteUrl(ExternalSiteUrl url)
    {
      this.mngr.AddExternalOrganizationURL(url.URL, url.SiteId);
    }

    public void DeleteSiteUrl(ExternalSiteUrl url)
    {
      this.mngr.DeleteExternalOrganizationURL(url.SiteId);
    }

    public void UpdateSiteUrl(ExternalSiteUrl url)
    {
      this.mngr.UpdateExternalOrganizationURL(new ExternalOrgURL()
      {
        siteId = url.SiteId,
        URL = url.URL
      });
    }

    public List<ExternalSiteUrl> GetSiteUrls()
    {
      ExternalOrgURL[] organizationUrLs = this.mngr.GetExternalOrganizationURLs();
      List<ExternalSiteUrl> siteUrls = new List<ExternalSiteUrl>();
      if (organizationUrLs != null)
      {
        foreach (ExternalOrgURL externalOrgUrl in organizationUrLs)
        {
          if (!externalOrgUrl.isDeleted)
            siteUrls.Add(new ExternalSiteUrl(externalOrgUrl));
        }
      }
      return siteUrls;
    }
  }
}
