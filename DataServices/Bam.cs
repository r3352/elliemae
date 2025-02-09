// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataServices.Bam
// Assembly: DataServices, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 227B0203-DF45-468D-9C1B-FA6CED472E23
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataServices.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataServices
{
  public class Bam : IBam
  {
    private LoanDataMgr loanDataMgr;
    private LoanData loanData;

    public Bam(LoanDataMgr loanDataMgr)
      : this(loanDataMgr, false)
    {
    }

    public Bam(LoanDataMgr loanDataMgr, bool useLinkedLoan)
    {
      this.loanDataMgr = loanDataMgr;
      if (useLinkedLoan)
        this.loanData = loanDataMgr.LinkedLoan.LoanData;
      else
        this.loanData = loanDataMgr.LoanData;
    }

    public string[] GetEligibleOfferList() => new string[0];

    public string[] GetEnabledOffers() => new string[0];

    public string GetCurrentCoreMilestone()
    {
      string currentCoreMilestone = "";
      try
      {
        MilestoneLog milestoneLog = (MilestoneLog) null;
        MilestoneLog[] allMilestones = this.loanData.GetLogList().GetAllMilestones();
        for (int index = allMilestones.Length - 1; index >= 0; --index)
        {
          if (allMilestones[index].Done)
          {
            milestoneLog = allMilestones[index];
            break;
          }
        }
        if (milestoneLog != null)
          currentCoreMilestone = milestoneLog.Stage;
      }
      catch (Exception ex)
      {
      }
      return currentCoreMilestone;
    }

    public bool OrderedService(string docType)
    {
      bool flag = false;
      try
      {
        foreach (DocumentLog allePassDocument in this.loanData.GetLogList().GetAllePASSDocuments())
        {
          if (allePassDocument.Title == docType)
          {
            flag = true;
            break;
          }
        }
      }
      catch (Exception ex)
      {
      }
      return flag;
    }

    public int GetCurrentBorrowerPair() => this.loanData.GetPairIndex(this.loanData.PairId);

    public string GetField(string id) => this.loanData.GetField(id);

    public int GetNumberOfDeposits() => this.loanData.GetNumberOfDeposits();

    public int GetNumberOfEmployer(bool borrower) => this.loanData.GetNumberOfEmployer(borrower);

    public int GetNumberOfLiabilitesExlcudingAlimonyJobExp()
    {
      return this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
    }

    public int GetNumberOfMortgages() => this.loanData.GetNumberOfMortgages();

    public int GetNumberOfResidence(bool borrower) => this.loanData.GetNumberOfResidence(borrower);

    public int GetNumberOfBorrowerPairs() => this.loanData.GetNumberOfBorrowerPairs();

    public string GetPairID() => this.loanData.PairId;

    public string GetSimpleField(string id) => this.loanData.GetSimpleField(id);

    public string GetSimpleField(string id, int pairIndex)
    {
      return this.loanData.GetSimpleField(id, pairIndex);
    }

    public void GoToField(string fieldID)
    {
      Session.Application.GetService<ILoanEditor>().GoToField(fieldID);
    }

    public bool IsLocked(string id) => this.loanData.IsLocked(id);

    public void SetBorrowerPair(int pairIndex) => this.loanData.SetBorrowerPair(pairIndex);

    public void SetCurrentField(string id, string val) => this.loanData.SetCurrentField(id, val);

    public void SetField(string id, string val) => this.loanData.SetField(id, val);

    public string GetUserID() => Session.UserID;

    public string GetUserFirstName() => Session.UserInfo.FirstName;

    public string GetUserLastName() => Session.UserInfo.LastName;

    public string GetUserPersona() => "";

    public string GetUserPhone() => Session.UserInfo.Phone;

    public string GetUserEmail() => Session.UserInfo.Email;

    public string GetCompanyClientID() => Session.CompanyInfo.ClientID;

    public string GetCompanyName()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyName : Session.CompanyInfo.Name;
    }

    public string GetCompanyAddress1()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyAddress.Street1 : Session.CompanyInfo.Address;
    }

    public string GetCompanyAddress2()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyAddress.Street2 : string.Empty;
    }

    public string GetCompanyCity()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyAddress.City : Session.CompanyInfo.City;
    }

    public string GetCompanyState()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyAddress.State : Session.CompanyInfo.State;
    }

    public string GetCompanyZip()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyAddress.Zip : Session.CompanyInfo.Zip;
    }

    public string GetCompanyPhone()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyPhone : Session.CompanyInfo.Phone;
    }

    public string GetCompanyFax()
    {
      OrgInfo organizationInfo = this.getOrganizationInfo();
      return organizationInfo != null ? organizationInfo.CompanyFax : Session.CompanyInfo.Fax;
    }

    private OrgInfo getOrganizationInfo()
    {
      return Session.UserInfo.OrgId != 0 ? this.loanDataMgr.SystemConfiguration.DisplayOrganization : (OrgInfo) null;
    }

    public string GetCompanySetting(string section, string key)
    {
      return Session.ConfigurationManager.GetCompanySetting(section, key);
    }

    public string GetUserSetting(string section, string key)
    {
      return Session.GetPrivateProfileString(section, key);
    }

    public void SetCompanySetting(string section, string key, string val)
    {
      Session.ConfigurationManager.SetCompanySetting(section, key, val);
    }

    public void SetUserSetting(string section, string key, string val)
    {
      Session.WritePrivateProfileString(section, key, val);
    }

    public string GetTempFolder() => SystemSettings.TempFolderRoot;

    public string GetLoanFolder() => this.loanDataMgr.LoanFolder;

    public string GetLoanName() => this.loanDataMgr.LoanName;

    public bool FileExists(string filename) => this.loanDataMgr.SupportingDataExists(filename);

    public bool IsReadOnly() => !this.loanDataMgr.Writable;

    public byte[] LoadBinary(string filename)
    {
      using (BinaryObject supportingData = this.loanDataMgr.GetSupportingData(filename))
        return supportingData?.GetBytes();
    }

    public string LoadFile(string filename)
    {
      return this.loanDataMgr.GetSupportingData(filename)?.ToString();
    }

    public void LockLoan()
    {
      this.loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
    }

    public void SaveBinary(string filename, byte[] content)
    {
      BinaryObject data = (BinaryObject) null;
      if (content != null)
        data = new BinaryObject(content);
      this.loanDataMgr.SaveSupportingData(filename, data);
    }

    public void SaveFile(string filename, string content)
    {
      BinaryObject data = (BinaryObject) null;
      if (content != null)
        data = new BinaryObject(content, Encoding.ASCII);
      this.loanDataMgr.SaveSupportingData(filename, data);
    }

    public void SaveLoan() => this.loanDataMgr.Save(false);

    public void UnlockLoan() => this.loanDataMgr.Unlock();

    public string GetVersion()
    {
      return VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition);
    }

    public string GetEdition()
    {
      string edition = string.Empty;
      switch (Session.EncompassEdition)
      {
        case EncompassEdition.Broker:
          edition = "Broker";
          break;
        case EncompassEdition.Banker:
          edition = "Banker";
          break;
      }
      if (Session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        edition = "Anywhere";
      return edition;
    }

    public string GetSystemID()
    {
      string systemId = "";
      try
      {
        systemId = Session.SessionObjects.SystemID;
      }
      catch (Exception ex)
      {
      }
      return systemId;
    }

    public bool IsRuntimeEnvironment(string type)
    {
      RuntimeEnvironment runtimeEnvironment = RuntimeEnvironment.Default;
      switch (type.ToUpper().Trim())
      {
        case "DEFAULT":
          runtimeEnvironment = RuntimeEnvironment.Default;
          break;
        case "HOSTED":
          runtimeEnvironment = RuntimeEnvironment.Hosted;
          break;
      }
      return EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == runtimeEnvironment;
    }

    public void SaveLoan(bool triggerCalcAll) => this.loanDataMgr.Save(false, triggerCalcAll);

    public Bam GetLinkedBam()
    {
      return this.loanDataMgr.LinkedLoan == null || this.loanDataMgr.LinkedLoan.LoanData == null ? (Bam) null : new Bam(this.loanDataMgr, true);
    }

    public LinkSyncType GetLinkSyncType() => this.loanData.LinkSyncType;
  }
}
