// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FeeManagementSetting
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FeeManagementSetting
  {
    private List<FeeManagementRecord> allRecords = new List<FeeManagementRecord>();
    private bool ucdFeesValidated;
    private static string[] fullUcdFeeList;
    private bool companyOptIn;

    public void AddFee(FeeManagementRecord fee) => this.AddFee(fee, false);

    public void AddFee(FeeManagementRecord fee, bool onlyAddIfFeeDoesNotExist)
    {
      if (onlyAddIfFeeDoesNotExist)
      {
        for (int index = 0; index < this.allRecords.Count; ++index)
        {
          if (string.Compare(this.allRecords[index].FeeName, fee.FeeName, true) == 0)
            return;
        }
      }
      this.allRecords.Add(fee);
    }

    public void UpdateFee(FeeManagementRecord fee)
    {
      for (int index = 0; index < this.allRecords.Count; ++index)
      {
        if (string.Compare(this.allRecords[index].FeeName, fee.FeeName, true) == 0)
        {
          this.allRecords[index].FeeName = fee.FeeName;
          this.allRecords[index].FeeNameInMavent = fee.FeeNameInMavent;
          this.allRecords[index].FeeIDInMavent = fee.FeeIDInMavent;
          this.allRecords[index].FeeSource = fee.FeeSource;
          if (fee.For700)
          {
            this.allRecords[index].For700 = true;
            return;
          }
          if (fee.For800)
          {
            this.allRecords[index].For800 = true;
            return;
          }
          if (fee.For900)
          {
            this.allRecords[index].For900 = true;
            return;
          }
          if (fee.For1000)
          {
            this.allRecords[index].For1000 = true;
            return;
          }
          if (fee.For1100)
          {
            this.allRecords[index].For1100 = true;
            return;
          }
          if (fee.For1200)
          {
            this.allRecords[index].For1200 = true;
            return;
          }
          if (fee.For1300)
          {
            this.allRecords[index].For1300 = true;
            return;
          }
          if (!fee.ForPC)
            return;
          this.allRecords[index].ForPC = true;
          return;
        }
      }
      this.allRecords.Add(fee);
    }

    public int NumberOfFees => this.allRecords.Count;

    public FeeManagementRecord[] GetAllFees()
    {
      this.validateFeeNameInUCD();
      return this.allRecords.ToArray();
    }

    public string[] GetFeeNames(FeeSectionEnum sectionEnum)
    {
      List<string> stringList = new List<string>();
      stringList.Add("");
      for (int index = 0; index < this.allRecords.Count; ++index)
      {
        if (this.allRecords[index].Included(sectionEnum) && !stringList.Contains(this.allRecords[index].FeeName))
          stringList.Add(this.allRecords[index].FeeName);
      }
      stringList.Sort();
      return stringList.ToArray();
    }

    public FeeManagementRecord GetFeeManagementRecord(string feeName)
    {
      for (int index = 0; index < this.allRecords.Count; ++index)
      {
        if (string.Compare(this.allRecords[index].FeeName, feeName, true) == 0)
          return this.allRecords[index];
      }
      return (FeeManagementRecord) null;
    }

    public bool CompanyOptIn
    {
      set => this.companyOptIn = value;
      get => this.companyOptIn;
    }

    public FeeManagementSetting Clone()
    {
      FeeManagementSetting managementSetting = new FeeManagementSetting();
      managementSetting.CompanyOptIn = this.companyOptIn;
      for (int index = 0; index < this.allRecords.Count; ++index)
        managementSetting.AddFee(new FeeManagementRecord(this.allRecords[index].FeeName, this.allRecords[index].For700, this.allRecords[index].For800, this.allRecords[index].For900, this.allRecords[index].For1000, this.allRecords[index].For1100, this.allRecords[index].For1200, this.allRecords[index].For1300, this.allRecords[index].ForPC, this.allRecords[index].FeeSource, this.allRecords[index].FeeNameInMavent, this.allRecords[index].FeeIDInMavent, this.allRecords[index].FeeNameInUCD));
      return managementSetting;
    }

    private void validateFeeNameInUCD()
    {
      if (this.ucdFeesValidated)
        return;
      if (FeeManagementSetting.fullUcdFeeList == null || FeeManagementSetting.fullUcdFeeList.Length == 0)
        FeeManagementSetting.fullUcdFeeList = SystemSettings.readUcdFeeList();
      if (FeeManagementSetting.fullUcdFeeList == null || FeeManagementSetting.fullUcdFeeList.Length == 0)
        return;
      for (int index = 0; index < this.allRecords.Count; ++index)
      {
        if (!string.IsNullOrEmpty(this.allRecords[index].FeeNameInUCD) && !((IEnumerable<string>) FeeManagementSetting.fullUcdFeeList).Contains<string>(this.allRecords[index].FeeNameInUCD))
          this.allRecords[index].FeeNameInUCD = string.Empty;
      }
      this.ucdFeesValidated = true;
    }
  }
}
