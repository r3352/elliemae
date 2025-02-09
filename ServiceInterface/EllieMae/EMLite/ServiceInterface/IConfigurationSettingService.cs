// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IConfigurationSettingService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.Domain.Mortgage;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IConfigurationSettingService
  {
    MIRecord[] GetMIRecords(Loan loan);

    MIRecord[] GetMIRecordsCustom(Loan loan);

    MIRecord[] GetMIRecordsDownload(Loan loan);

    MIRecord[] GetMIRecords(Guid loanGuid);

    int GetCountyLimit(string stateAbb, string countyName, int numOfUnits);

    ISettingsProvider GetSettingsProvider();

    CompanyInfo GetCompanyInfo();

    OrgInfo GetFirstOrganizationWithLEI(int orgId);

    LicenseInfo GetLicenseInfo();

    string GetCompanySetting(string section, string key);

    Hashtable GetCompanySettings(string section);

    IDictionary GetAllServerSettings();

    IDictionary GetCategoryServerSettings(string categoryName);

    object GetSystemPolicy(string path);

    object GetSystemCompliance(string path);

    BinaryObject GetSystemSettings(string name);

    EncompassSystemInfo GetEncompassSystemInfo();

    List<object> GetTPOForClosingVendorInformation(string tpoOrgID, string tpoLOID);

    BusinessCalendar GetBusinessCalendar(CalendarType type, DateTime startDate, DateTime endDate);

    BusinessCalendar GetBusinessCalendar(CalendarType calendarType);

    List<ExternalOriginatorManagementData> GetExternalOrganizationByTpoId(string tpoId);

    List<ExternalFeeManagement> GetFeeManagement(int externalOrgId);

    bool IsNumeric(string fieldId);

    Hashtable GetSystemSettings(string[] names);

    HMDAInformation GetHMDAInformation();

    List<ChangeCircumstanceSettings> GetAllChangeCircumstanceSettings();

    ProductPricingSetting GetActiveProductPricingPartner();
  }
}
