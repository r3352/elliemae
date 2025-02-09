// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External url</summary>
  public class ExternalUrl : IExternalUrl
  {
    private ExternalOrgURL externalOrgUrl;

    internal ExternalUrl(ExternalOrgURL externalOrgUrl) => this.externalOrgUrl = externalOrgUrl;

    internal ExternalUrl(string url, string siteId, ExternalOrganizationEntityType entityType)
    {
      this.externalOrgUrl = new ExternalOrgURL();
      this.externalOrgUrl.URL = url;
      this.externalOrgUrl.siteId = siteId;
      this.externalOrgUrl.EntityType = Convert.ToInt32((object) entityType);
    }

    /// <summary>Gets URL id</summary>
    public int URLID => this.externalOrgUrl.URLID;

    /// <summary>Gets or sets URL</summary>
    public string URL
    {
      get => this.externalOrgUrl.URL;
      set => this.externalOrgUrl.URL = value;
    }

    /// <summary>Gets or sets url type</summary>
    public ExternalOrganizationEntityType EntityType
    {
      get => (ExternalOrganizationEntityType) this.externalOrgUrl.EntityType;
      set => this.externalOrgUrl.EntityType = (int) value;
    }

    internal static ExternalUrlList ToList(ExternalOrgURL[] urls)
    {
      ExternalUrlList list = new ExternalUrlList();
      ((IEnumerable<ExternalOrgURL>) urls).ToList<ExternalOrgURL>().ForEach((Action<ExternalOrgURL>) (x => list.Add(new ExternalUrl(x))));
      return list;
    }

    internal static List<ExternalOrgURL> ToList(ExternalUrlList urls)
    {
      List<ExternalOrgURL> list = new List<ExternalOrgURL>();
      for (int index = 0; index < urls.Count; ++index)
        list.Add(new ExternalOrgURL()
        {
          URLID = urls[index].URLID,
          URL = urls[index].URL,
          EntityType = (int) urls[index].EntityType,
          siteId = urls[index].SiteId
        });
      return list;
    }

    /// <summary>Gets or sets date added</summary>
    public DateTime DateAdded
    {
      get => this.externalOrgUrl.DateAdded;
      set => this.externalOrgUrl.DateAdded = value;
    }

    /// <summary>Gets or sets site id</summary>
    public string SiteId
    {
      get => this.externalOrgUrl.siteId;
      set => this.externalOrgUrl.siteId = value;
    }

    /// <summary>
    /// Gets IsDeleted .These URLs are deleted from the TPO webcenter but is still configured till the user explicitly deletes it.
    /// </summary>
    public bool IsDeleted => this.externalOrgUrl.isDeleted;
  }
}
