// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSalesRep
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalSalesRep : IExternalSalesRep
  {
    private ExternalOrgSalesRep externalOrgSalesRep;
    private int currentOrgId;

    internal ExternalSalesRep(ExternalOrgSalesRep externalOrgSalesRep, int currentOrgId)
    {
      this.externalOrgSalesRep = externalOrgSalesRep;
      this.currentOrgId = currentOrgId;
    }

    public int salesRepId => this.externalOrgSalesRep.salesRepId;

    public int externalOrgId => this.externalOrgSalesRep.externalOrgId;

    public string userId => this.externalOrgSalesRep.userId;

    public bool isDeletable => this.currentOrgId == this.externalOrgId;

    public string companyDBAName => this.externalOrgSalesRep.companyDBAName;

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
