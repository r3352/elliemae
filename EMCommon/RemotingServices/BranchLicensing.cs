// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.BranchLicensing
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class BranchLicensing
  {
    private bool useParentInfo;
    private string lenderType = string.Empty;
    private string homeState = string.Empty;
    private string optOut = string.Empty;
    private bool statutoryElectionInMaryland;
    private string statutoryElectionInMaryland2 = string.Empty;
    private bool statutoryElectionInKansas;
    private bool useCustomLenderProfile;
    private BranchLicensing.ATRSmallCreditors atrSmallCreditor;
    private BranchLicensing.ATRExemptCreditors atrExemptCreditor;
    private List<StateLicenseType> stateLicenseTypes;

    public BranchLicensing()
      : this(false, (string) null, (string) null, (string) null, false, "00", false, (List<StateLicenseType>) null, false, BranchLicensing.ATRSmallCreditors.None, BranchLicensing.ATRExemptCreditors.None)
    {
    }

    public BranchLicensing(
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
      : this(false, lenderType, homeState, optOut, statutoryElectionInMaryland, statutoryElectionInMaryland2, statutoryElectionInKansas, stateLicenseTypes, useCustomLenderProfile, atrSmallCreditor, atrExemptCreditor)
    {
    }

    public BranchLicensing(
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
    {
      this.useParentInfo = useParentInfo;
      this.stateLicenseTypes = new List<StateLicenseType>();
      this.lenderType = lenderType;
      this.homeState = homeState;
      this.optOut = optOut;
      this.statutoryElectionInMaryland = statutoryElectionInMaryland;
      this.statutoryElectionInKansas = statutoryElectionInKansas;
      this.statutoryElectionInMaryland2 = statutoryElectionInMaryland2;
      this.useCustomLenderProfile = useCustomLenderProfile;
      if (stateLicenseTypes != null)
        this.stateLicenseTypes = stateLicenseTypes;
      this.atrSmallCreditor = atrSmallCreditor;
      this.atrExemptCreditor = atrExemptCreditor;
    }

    public bool UseParentInfo
    {
      get => this.useParentInfo;
      set => this.useParentInfo = value;
    }

    public string LenderType
    {
      get => this.lenderType;
      set => this.lenderType = value;
    }

    public string HomeState
    {
      get => this.homeState;
      set => this.homeState = value;
    }

    public string OptOut
    {
      get => this.optOut;
      set => this.optOut = value;
    }

    public bool StatutoryElectionInMaryland
    {
      get => this.statutoryElectionInMaryland;
      set => this.statutoryElectionInMaryland = value;
    }

    public string StatutoryElectionInMaryland2
    {
      get => this.statutoryElectionInMaryland2;
      set => this.statutoryElectionInMaryland2 = value;
    }

    public bool StatutoryElectionInKansas
    {
      get => this.statutoryElectionInKansas;
      set => this.statutoryElectionInKansas = value;
    }

    public bool UseCustomLenderProfile
    {
      get => this.useCustomLenderProfile;
      set => this.useCustomLenderProfile = value;
    }

    public BranchLicensing.ATRSmallCreditors ATRSmallCreditor
    {
      get => this.atrSmallCreditor;
      set => this.atrSmallCreditor = value;
    }

    public static BranchLicensing.ATRSmallCreditors ATRSmallCreditorToEnum(int value)
    {
      try
      {
        return (BranchLicensing.ATRSmallCreditors) Enum.Parse(typeof (BranchLicensing.ATRSmallCreditors), value.ToString(), true);
      }
      catch
      {
        return BranchLicensing.ATRSmallCreditors.None;
      }
    }

    public string ATRSmallCreditorToString()
    {
      switch (this.atrSmallCreditor)
      {
        case BranchLicensing.ATRSmallCreditors.SmallCreditor:
          return "Small Creditor";
        case BranchLicensing.ATRSmallCreditors.RuralSmallCreditor:
          return "Rural Small Creditor";
        default:
          return "";
      }
    }

    public BranchLicensing.ATRExemptCreditors ATRExemptCreditor
    {
      get => this.atrExemptCreditor;
      set => this.atrExemptCreditor = value;
    }

    public static BranchLicensing.ATRExemptCreditors ATRExemptCreditorToEnum(int value)
    {
      try
      {
        return (BranchLicensing.ATRExemptCreditors) Enum.Parse(typeof (BranchLicensing.ATRExemptCreditors), value.ToString(), true);
      }
      catch
      {
        return BranchLicensing.ATRExemptCreditors.None;
      }
    }

    public string ATRExemptCreditorToString()
    {
      switch (this.atrExemptCreditor)
      {
        case BranchLicensing.ATRExemptCreditors.CommunityDevelopmentFinancialInstitution:
          return "Community Development Financial Institution";
        case BranchLicensing.ATRExemptCreditors.CommunityHousingDevelopmentOrganization:
          return "Community Housing Development Organization";
        case BranchLicensing.ATRExemptCreditors.DownpaymentAssistanceProvider:
          return "Downpayment Assistance Provider";
        case BranchLicensing.ATRExemptCreditors.NonprofitOrganization:
          return "Nonprofit Organization";
        default:
          return "";
      }
    }

    public List<StateLicenseType> StateLicenseTypes => this.stateLicenseTypes;

    public virtual void AddStateLicenseType(StateLicenseType stateLicenseType)
    {
      this.stateLicenseTypes.Add(stateLicenseType);
    }

    public virtual void AddStateLicenseType(
      string stateAbbrevation,
      string licenseType,
      bool selected,
      bool exempt)
    {
      this.stateLicenseTypes.Add(new StateLicenseType(stateAbbrevation, licenseType, selected, exempt));
    }

    public virtual object Clone()
    {
      BranchLicensing branchLicensing = new BranchLicensing(this.useParentInfo, this.lenderType, this.homeState, this.optOut, this.statutoryElectionInMaryland, this.statutoryElectionInMaryland2, this.statutoryElectionInKansas, (List<StateLicenseType>) null, this.useCustomLenderProfile, this.atrSmallCreditor, this.atrExemptCreditor);
      if (this.stateLicenseTypes != null)
      {
        for (int index = 0; index < this.stateLicenseTypes.Count; ++index)
          branchLicensing.AddStateLicenseType(new StateLicenseType(this.stateLicenseTypes[index].StateAbbrevation, this.stateLicenseTypes[index].LicenseType, this.stateLicenseTypes[index].Selected, this.stateLicenseTypes[index].Exempt));
      }
      return (object) branchLicensing;
    }

    public enum ATRSmallCreditors
    {
      None,
      SmallCreditor,
      RuralSmallCreditor,
    }

    public enum ATRExemptCreditors
    {
      None,
      CommunityDevelopmentFinancialInstitution,
      CommunityHousingDevelopmentOrganization,
      DownpaymentAssistanceProvider,
      NonprofitOrganization,
    }
  }
}
