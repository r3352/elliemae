// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalOrgWrapper
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  public class ExternalOrgWrapper
  {
    public ExternalOrgManagementDataCount ExternalManagementDataCount { get; set; }

    public List<ExternalOriginatorManagementData> ChildOrganizations { get; set; }

    public List<ExternalOrgSalesRepPlatform> SalesReps { get; set; }

    public List<ExternalOrgDBAName> DBADetails { get; set; }

    public List<ExternalOrgWarehouse> WareHousesDetails { get; set; }

    public ExternalOriginatorManagementData CommitmentsDetails { get; set; }

    public ExternalOrgLoanTypes LoanCriteria { get; set; }

    public ExternalOrgCustomFields CustomFieldsDetails { get; set; }

    public List<ExternalOrgURL> TpoConnectSetup { get; set; }
  }
}
