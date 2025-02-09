// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.IBam
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Export.BamObjects;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public interface IBam
  {
    int GetCurrentBorrowerPair();

    string GetField(string id);

    int GetNumberOfDeposits();

    int GetNumberOfEmployer(bool borrower);

    int GetNumberOfLiabilitesExlcudingAlimonyJobExp();

    int GetNumberOfMortgages();

    int GetNumberOfResidence(bool borrower);

    int GetNumberOfBorrowerPairs();

    string GetSimpleField(string id);

    string GetSimpleField(string id, int pairIndex);

    bool IsLocked(string id);

    void SetBorrowerPair(int pairIndex);

    string GetCompanySetting(string section, string key);

    string GetUserSetting(string section, string key);

    string GetUserID();

    string ExportData(string format);

    bool ValidateData(string format, bool allowContinue);

    string ToXml();

    string GetParentOranizationNMLSID(string loanOfficerID);

    string GetVersion();

    string GetClientID();

    bool IsSuperAdministrator();

    bool IsAdministrator();

    bool IsTopLevelAdministrator();

    void GoToField(string fieldID, string exportType);

    void GoToField(string fieldID, bool findNext, string exportType);

    string GetFullVersion();

    int GetNumberOfSettlementServiceProviders();

    DisclosureTrackingRecord2015[] GetDisclosureTracking2015Records();

    List<Dictionary<string, string>> GetFeeManagementSettings();

    Dictionary<string, string> GetFeeManagementPersonaSettings(int personaID);

    List<Dictionary<string, string>> GetFeeManagementPersonaSettings();

    int GetNumberOfAdditionalLoans();

    int GetNumberOfGiftsAndGrants();

    int GetNumberOfOtherIncomeSources();

    int GetNumberOfOtherLiabilities();

    int GetNumberOfOtherAssets();

    int GetNumberOfAlternateNames(bool borrower);

    string GetAccessToken(string scope);

    string GetUserPassword();

    EnhancedDisclosureTrackingRecord2015[] GetDisclosureTracking2015EnhancedRecords();
  }
}
