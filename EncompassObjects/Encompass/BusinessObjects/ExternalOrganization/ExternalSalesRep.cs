// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSalesRep
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External sales rep</summary>
  public class ExternalSalesRep : IExternalSalesRep
  {
    private ExternalOrgSalesRep externalOrgSalesRep;
    private int currentOrgId;

    internal ExternalSalesRep(ExternalOrgSalesRep externalOrgSalesRep, int currentOrgId)
    {
      this.externalOrgSalesRep = externalOrgSalesRep;
      this.currentOrgId = currentOrgId;
    }

    /// <summary>Gets sales rep id</summary>
    public int salesRepId => this.externalOrgSalesRep.salesRepId;

    /// <summary>Gets external organization id</summary>
    public int externalOrgId => this.externalOrgSalesRep.externalOrgId;

    /// <summary>Gets Encompass user id</summary>
    public string userId => this.externalOrgSalesRep.userId;

    /// <summary>Gets flag for indicating if a sales rep is deletable</summary>
    public bool isDeletable => this.currentOrgId == this.externalOrgId;

    /// <summary>Gets Company DBA Name</summary>
    public string companyDBAName => this.externalOrgSalesRep.companyDBAName;

    /// <summary>Gets Company Legal Name</summary>
    public string companyLegalName => this.externalOrgSalesRep.companyLegalName;

    internal static List<ExternalSalesRep> ToList(List<ExternalOrgSalesRep> comp, int currentOrgId)
    {
      List<ExternalSalesRep> list = new List<ExternalSalesRep>();
      for (int index = 0; index < comp.Count; ++index)
        list.Add(new ExternalSalesRep(comp[index], currentOrgId));
      return list;
    }
  }
}
