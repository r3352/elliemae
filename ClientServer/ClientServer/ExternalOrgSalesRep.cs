// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOrgSalesRep
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalOrgSalesRep
  {
    public int salesRepId { get; set; }

    public int externalOrgId { get; set; }

    public string userId { get; set; }

    public string userName { get; set; }

    public string title { get; set; }

    public string companyLegalName { get; set; }

    public string organizationName { get; set; }

    public string companyDBAName { get; set; }

    public string email { get; set; }

    public string phone { get; set; }

    public bool isPrimarySalesRep { get; set; }

    public bool isWholesaleChannelEnabled { get; set; }

    public bool isDelegatedChannelEnabled { get; set; }

    public bool isNonDelegatedChannelEnabled { get; set; }

    public ExternalOrgSalesRep()
    {
    }

    public ExternalOrgSalesRep(
      int salesRepId,
      int externalOrgId,
      string userId,
      string companyLegalName = "�",
      string organizationName = "�",
      string userName = "�",
      string title = "�",
      string phone = "�",
      string email = "�",
      bool isPrimarySalesRep = false,
      bool isWholesaleChannelEnabled = false,
      bool isDelegatedChannelEnabled = false,
      bool isNonDelegatedChannelEnabled = false)
    {
      this.salesRepId = salesRepId;
      this.externalOrgId = externalOrgId;
      this.userId = userId;
      this.userName = userName;
      this.title = title;
      this.companyLegalName = companyLegalName;
      this.organizationName = organizationName;
      this.email = email;
      this.phone = phone;
      this.isPrimarySalesRep = isPrimarySalesRep;
      this.isWholesaleChannelEnabled = isWholesaleChannelEnabled;
      this.isDelegatedChannelEnabled = isDelegatedChannelEnabled;
      this.isNonDelegatedChannelEnabled = isNonDelegatedChannelEnabled;
    }
  }
}
