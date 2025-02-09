// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BranchExtLicensing
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class BranchExtLicensing : BranchLicensing
  {
    private int allowLoansWithIssues;
    private string msgUploadNonApprovedLoans = "";
    private List<StateLicenseExtType> stateLicenseExtTypes;

    public BranchExtLicensing(
      bool useParentInfo,
      int allowLoansWithIssues,
      string msgUploadNonApprovedLoans,
      string lenderType,
      string homeState,
      string optOut,
      bool statutoryElectionInMaryland,
      string statutoryElectionInMaryland2,
      bool statutoryElectionInKansas,
      List<StateLicenseExtType> stateLicenseExtTypes,
      bool useCustomLenderProfile,
      BranchLicensing.ATRSmallCreditors atrSmallCreditor,
      BranchLicensing.ATRExemptCreditors atrExemptCreditor)
      : base(useParentInfo, lenderType, homeState, optOut, statutoryElectionInMaryland, statutoryElectionInMaryland2, statutoryElectionInKansas, (List<StateLicenseType>) null, useCustomLenderProfile, atrSmallCreditor, atrExemptCreditor)
    {
      this.stateLicenseExtTypes = new List<StateLicenseExtType>();
      this.allowLoansWithIssues = allowLoansWithIssues;
      this.msgUploadNonApprovedLoans = msgUploadNonApprovedLoans;
      if (stateLicenseExtTypes == null)
        return;
      this.stateLicenseExtTypes = stateLicenseExtTypes;
    }

    public BranchExtLicensing()
      : this(false, (string) null, (string) null, (string) null, false, "00", false, (List<StateLicenseType>) null, false, BranchLicensing.ATRSmallCreditors.None, BranchLicensing.ATRExemptCreditors.None)
    {
    }

    public BranchExtLicensing(
      string lenderType,
      string homeState,
      string optOut,
      bool statutoryElectionInMaryland,
      string staturoryElectionInMaryland2,
      bool statutoryElectionInKansas,
      List<StateLicenseType> stateLicenseTypes,
      bool useCustomLenderProfile,
      BranchLicensing.ATRSmallCreditors atrSmallCreditor,
      BranchLicensing.ATRExemptCreditors atrExemptCreditor)
      : this(false, lenderType, homeState, optOut, statutoryElectionInMaryland, staturoryElectionInMaryland2, statutoryElectionInKansas, stateLicenseTypes, useCustomLenderProfile, atrSmallCreditor, atrExemptCreditor)
    {
    }

    public BranchExtLicensing(
      bool useParentInfo,
      string lenderType,
      string homeState,
      string optOut,
      bool statutoryElectionInMaryland,
      string statutoryElectionInMaryland2,
      bool statutoryElectionInKansas,
      List<StateLicenseType> stateLicenseTypes,
      bool useCustomLenderProfile,
      BranchLicensing.ATRSmallCreditors atrSmallCreditor,
      BranchLicensing.ATRExemptCreditors atrExemptCreditor)
      : base(useParentInfo, lenderType, homeState, optOut, statutoryElectionInMaryland, statutoryElectionInMaryland2, statutoryElectionInKansas, (List<StateLicenseType>) null, useCustomLenderProfile, atrSmallCreditor, atrExemptCreditor)
    {
      this.stateLicenseExtTypes = new List<StateLicenseExtType>();
      this.allowLoansWithIssues = 0;
      this.msgUploadNonApprovedLoans = "";
    }

    public BranchExtLicensing(BranchLicensing licensing)
      : base(licensing.UseParentInfo, licensing.LenderType, licensing.HomeState, licensing.OptOut, licensing.StatutoryElectionInMaryland, licensing.StatutoryElectionInMaryland2, licensing.StatutoryElectionInKansas, (List<StateLicenseType>) null, licensing.UseCustomLenderProfile, licensing.ATRSmallCreditor, licensing.ATRExemptCreditor)
    {
      this.stateLicenseExtTypes = new List<StateLicenseExtType>();
      foreach (StateLicenseType stateLicenseType in licensing.StateLicenseTypes)
        this.AddStateLicenseExtType(new StateLicenseExtType(stateLicenseType.StateAbbrevation, stateLicenseType.LicenseType, "", DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "", DateTime.MinValue, stateLicenseType.Selected, stateLicenseType.Exempt, DateTime.MinValue));
      this.allowLoansWithIssues = 0;
      this.msgUploadNonApprovedLoans = "";
    }

    public int AllowLoansWithIssues
    {
      get => this.allowLoansWithIssues;
      set => this.allowLoansWithIssues = value;
    }

    public string MsgUploadNonApprovedLoans
    {
      get => this.msgUploadNonApprovedLoans;
      set => this.msgUploadNonApprovedLoans = value;
    }

    public List<StateLicenseExtType> StateLicenseExtTypes
    {
      get => this.stateLicenseExtTypes;
      set => this.stateLicenseExtTypes = value;
    }

    public void AddStateLicenseExtType(StateLicenseExtType stateLicenseExtType)
    {
      this.stateLicenseExtTypes.Add(stateLicenseExtType);
    }

    public void RemoveStateLicenseExtType(StateLicenseExtType stateLicenseExtType)
    {
      this.stateLicenseExtTypes.Remove(stateLicenseExtType);
    }

    public bool IsExists(StateLicenseExtType stateLicenseExtType)
    {
      foreach (StateLicenseExtType stateLicenseExtType1 in this.stateLicenseExtTypes)
      {
        if (stateLicenseExtType1.StateAbbrevation == stateLicenseExtType.StateAbbrevation && stateLicenseExtType1.LicenseType == stateLicenseExtType.LicenseType)
          return true;
      }
      return false;
    }

    public StateLicenseExtType IsExists(string stateName)
    {
      foreach (StateLicenseExtType stateLicenseExtType in this.stateLicenseExtTypes)
      {
        if (stateLicenseExtType.StateAbbrevation == stateName && stateLicenseExtType.Selected)
          return stateLicenseExtType;
      }
      return (StateLicenseExtType) null;
    }

    public List<StateLicenseExtType> GetLicenses(string stateName)
    {
      List<StateLicenseExtType> licenses = new List<StateLicenseExtType>();
      foreach (StateLicenseExtType stateLicenseExtType in this.stateLicenseExtTypes)
      {
        if (stateLicenseExtType.StateAbbrevation == stateName)
          licenses.Add(stateLicenseExtType);
      }
      return licenses;
    }

    public override object Clone()
    {
      BranchExtLicensing branchExtLicensing = new BranchExtLicensing(this.UseParentInfo, this.AllowLoansWithIssues, this.MsgUploadNonApprovedLoans, this.LenderType, this.HomeState, this.OptOut, this.StatutoryElectionInMaryland, this.StatutoryElectionInMaryland2, this.StatutoryElectionInKansas, (List<StateLicenseExtType>) null, this.UseCustomLenderProfile, this.ATRSmallCreditor, this.ATRExemptCreditor);
      if (this.stateLicenseExtTypes != null)
      {
        for (int index = 0; index < this.stateLicenseExtTypes.Count; ++index)
          branchExtLicensing.AddStateLicenseExtType(new StateLicenseExtType(this.stateLicenseExtTypes[index].StateAbbrevation, this.stateLicenseExtTypes[index].LicenseType, this.stateLicenseExtTypes[index].LicenseNo, this.stateLicenseExtTypes[index].IssueDate, this.stateLicenseExtTypes[index].StartDate, this.stateLicenseExtTypes[index].EndDate, this.stateLicenseExtTypes[index].LicenseStatus, this.stateLicenseExtTypes[index].StatusDate, this.stateLicenseExtTypes[index].Approved, this.stateLicenseExtTypes[index].Exempt, this.stateLicenseExtTypes[index].LastChecked, this.stateLicenseExtTypes[index].SortIndex));
      }
      return (object) branchExtLicensing;
    }
  }
}
