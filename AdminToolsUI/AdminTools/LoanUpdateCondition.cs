// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanUpdateCondition
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanUpdateCondition
  {
    private Dictionary<string, string> loanStatusList;
    private List<object[]> loanInfos;
    private HMDAProfile newProfile;
    private bool inTestMode = true;
    private bool processPendingList;
    private LoanUpdateCondition.UpdateOptions updateOption;
    private string csvFile = "";
    private List<HMDAProfile> hmdaProfiles;
    private Dictionary<int, HMDAProfile> hmdaProfilesLookup;

    public int NewProfileID => this.newProfile == null ? -1 : this.newProfile.HMDAProfileID;

    public LoanUpdateCondition(HMDAProfile newProfile)
    {
      this.newProfile = newProfile;
      this.loanInfos = new List<object[]>();
      this.loanStatusList = new Dictionary<string, string>();
    }

    public void AddStatus(object[] loanInfo, string processStatus)
    {
      this.loanInfos.Add(loanInfo);
      string key = loanInfo[0].ToString();
      if (!(key != "") || this.loanStatusList.ContainsKey(key))
        return;
      this.loanStatusList.Add(key, processStatus);
    }

    public bool IsProcessingInPending => File.Exists(LoanBatchUpdates.PendingListFile_AssignNew);

    public bool InTestMode
    {
      get => this.inTestMode;
      set => this.inTestMode = value;
    }

    public bool ProcessPendingList
    {
      get => this.processPendingList;
      set => this.processPendingList = value;
    }

    public LoanUpdateCondition.UpdateOptions UpdateOption
    {
      get => this.updateOption;
      set => this.updateOption = value;
    }

    public string CSVFile
    {
      get => this.csvFile;
      set => this.csvFile = value;
    }

    public List<HMDAProfile> HmdaProfiles
    {
      get => this.hmdaProfiles;
      set
      {
        this.hmdaProfiles = value;
        this.hmdaProfilesLookup = new Dictionary<int, HMDAProfile>();
        if (this.hmdaProfiles == null || this.hmdaProfiles.Count <= 0)
          return;
        foreach (HMDAProfile hmdaProfile in this.hmdaProfiles)
          this.hmdaProfilesLookup.Add(hmdaProfile.HMDAProfileID, hmdaProfile);
      }
    }

    public Dictionary<int, HMDAProfile> HmdaProfilesLookup => this.hmdaProfilesLookup;

    public enum UpdateOptions
    {
      AssignNewHMDA,
      UpdateOnly,
    }
  }
}
