// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataServices.IBam
// Assembly: DataServices, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 227B0203-DF45-468D-9C1B-FA6CED472E23
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataServices.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.DataServices
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

    string GetPairID();

    string GetSimpleField(string id);

    string GetSimpleField(string id, int pairIndex);

    void GoToField(string fieldID);

    bool IsLocked(string id);

    void SetBorrowerPair(int pairIndex);

    void SetCurrentField(string id, string val);

    void SetField(string id, string val);

    string GetCurrentCoreMilestone();

    bool OrderedService(string docType);

    string[] GetEligibleOfferList();

    string[] GetEnabledOffers();

    string GetUserID();

    string GetUserFirstName();

    string GetUserLastName();

    string GetUserPersona();

    string GetUserPhone();

    string GetUserEmail();

    string GetCompanyClientID();

    string GetCompanyName();

    string GetCompanyAddress1();

    string GetCompanyAddress2();

    string GetCompanyCity();

    string GetCompanyState();

    string GetCompanyZip();

    string GetCompanyPhone();

    string GetCompanyFax();

    string GetCompanySetting(string section, string key);

    string GetUserSetting(string section, string key);

    void SetCompanySetting(string section, string key, string val);

    void SetUserSetting(string section, string key, string val);

    string GetTempFolder();

    string GetLoanFolder();

    string GetLoanName();

    bool FileExists(string filename);

    bool IsReadOnly();

    byte[] LoadBinary(string filename);

    string LoadFile(string filename);

    void LockLoan();

    void SaveBinary(string filename, byte[] content);

    void SaveFile(string filename, string content);

    void SaveLoan();

    void UnlockLoan();

    string GetVersion();

    string GetEdition();

    string GetSystemID();

    bool IsRuntimeEnvironment(string type);

    void SaveLoan(bool triggerCalcAll);

    Bam GetLinkedBam();

    LinkSyncType GetLinkSyncType();
  }
}
