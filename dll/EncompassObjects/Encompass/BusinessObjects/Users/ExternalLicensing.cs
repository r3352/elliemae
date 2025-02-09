// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ExternalLicensing
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("E8847A40-C5B7-471E-BB85-D67A5C214144")]
  public class ExternalLicensing : IExternalLicensing
  {
    private BranchExtLicensing licensing;

    internal BranchExtLicensing BranchExternalLicensing => this.licensing;

    public ExternalLicensing(
      bool useParentInfo,
      int allowLoansWithIssues,
      string msgUploadNonApprovedLoans,
      string lenderType,
      string homeState,
      string optOut,
      bool statutoryElectionInMaryland,
      string statutoryElectionInMaryland2,
      bool statutoryElectionInKansas,
      List<StateLicenseExtType> stateLicense,
      bool useCustomLenderProfile,
      ATRSmallCreditors atrSmallCreditor,
      ATRExemptCreditors atrExemptCreditor)
    {
      this.licensing = new BranchExtLicensing(useParentInfo, allowLoansWithIssues, msgUploadNonApprovedLoans, lenderType, homeState, optOut, statutoryElectionInMaryland, statutoryElectionInMaryland2, statutoryElectionInKansas, (List<StateLicenseExtType>) null, useCustomLenderProfile, (BranchLicensing.ATRSmallCreditors) 0, (BranchLicensing.ATRExemptCreditors) 0);
      this.ATRSmallCreditor = atrSmallCreditor;
      this.ATRExemptCreditor = atrExemptCreditor;
      if (stateLicense == null)
        return;
      for (int index = 0; index < stateLicense.Count; ++index)
        this.licensing.AddStateLicenseExtType(new StateLicenseExtType(stateLicense[index].StateAbbrevation, stateLicense[index].LicenseType, stateLicense[index].LicenseNo, stateLicense[index].IssueDate, stateLicense[index].StartDate, stateLicense[index].EndDate, stateLicense[index].LicenseStatus, stateLicense[index].StatusDate, stateLicense[index].Approved, stateLicense[index].Exempt, stateLicense[index].LastChecked, stateLicense[index].SortIndex));
    }

    internal ExternalLicensing(BranchExtLicensing licensing) => this.licensing = licensing;

    public bool UseParentInfo
    {
      get => ((BranchLicensing) this.licensing).UseParentInfo;
      set => ((BranchLicensing) this.licensing).UseParentInfo = value;
    }

    public string LenderType
    {
      get => ((BranchLicensing) this.licensing).LenderType;
      set => ((BranchLicensing) this.licensing).LenderType = value;
    }

    public string HomeState
    {
      get => ((BranchLicensing) this.licensing).HomeState;
      set => ((BranchLicensing) this.licensing).HomeState = value;
    }

    public string OptOut
    {
      get => ((BranchLicensing) this.licensing).OptOut;
      set => ((BranchLicensing) this.licensing).OptOut = value;
    }

    public bool StatutoryElectionInMaryland
    {
      get => ((BranchLicensing) this.licensing).StatutoryElectionInMaryland;
      set => ((BranchLicensing) this.licensing).StatutoryElectionInMaryland = value;
    }

    public string StatutoryElectionInMaryland2
    {
      get => ((BranchLicensing) this.licensing).StatutoryElectionInMaryland2;
      set => ((BranchLicensing) this.licensing).StatutoryElectionInMaryland2 = value;
    }

    public bool StatutoryElectionInKansas
    {
      get => ((BranchLicensing) this.licensing).StatutoryElectionInKansas;
      set => ((BranchLicensing) this.licensing).StatutoryElectionInKansas = value;
    }

    public bool UseCustomLenderProfile
    {
      get => ((BranchLicensing) this.licensing).UseCustomLenderProfile;
      set => ((BranchLicensing) this.licensing).UseCustomLenderProfile = value;
    }

    public ATRSmallCreditors ATRSmallCreditor
    {
      get
      {
        BranchLicensing.ATRSmallCreditors atrSmallCreditor = ((BranchLicensing) this.licensing).ATRSmallCreditor;
        if (atrSmallCreditor == 1)
          return ATRSmallCreditors.SmallCreditor;
        return atrSmallCreditor == 2 ? ATRSmallCreditors.RuralSmallCreditor : ATRSmallCreditors.None;
      }
      set
      {
        if (value != ATRSmallCreditors.SmallCreditor)
        {
          if (value == ATRSmallCreditors.RuralSmallCreditor)
            ((BranchLicensing) this.licensing).ATRSmallCreditor = (BranchLicensing.ATRSmallCreditors) 2;
          else
            ((BranchLicensing) this.licensing).ATRSmallCreditor = (BranchLicensing.ATRSmallCreditors) 0;
        }
        else
          ((BranchLicensing) this.licensing).ATRSmallCreditor = (BranchLicensing.ATRSmallCreditors) 1;
      }
    }

    public static ATRSmallCreditors ATRSmallCreditorToEnum(int value)
    {
      try
      {
        return (ATRSmallCreditors) Enum.Parse(typeof (ATRSmallCreditors), value.ToString(), true);
      }
      catch
      {
        return ATRSmallCreditors.None;
      }
    }

    public string ATRSmallCreditorToString()
    {
      BranchLicensing.ATRSmallCreditors atrSmallCreditor = ((BranchLicensing) this.licensing).ATRSmallCreditor;
      if (atrSmallCreditor == 1)
        return "Small Creditor";
      return atrSmallCreditor == 2 ? "Rural Small Creditor" : "";
    }

    public ATRExemptCreditors ATRExemptCreditor
    {
      get
      {
        switch (((BranchLicensing) this.licensing).ATRExemptCreditor - 1)
        {
          case 0:
            return ATRExemptCreditors.CommunityDevelopmentFinancialInstitution;
          case 1:
            return ATRExemptCreditors.CommunityHousingDevelopmentOrganization;
          case 2:
            return ATRExemptCreditors.DownpaymentAssistanceProvider;
          case 3:
            return ATRExemptCreditors.NonprofitOrganization;
          default:
            return ATRExemptCreditors.None;
        }
      }
      set
      {
        switch (value)
        {
          case ATRExemptCreditors.CommunityDevelopmentFinancialInstitution:
            ((BranchLicensing) this.licensing).ATRExemptCreditor = (BranchLicensing.ATRExemptCreditors) 1;
            break;
          case ATRExemptCreditors.CommunityHousingDevelopmentOrganization:
            ((BranchLicensing) this.licensing).ATRExemptCreditor = (BranchLicensing.ATRExemptCreditors) 2;
            break;
          case ATRExemptCreditors.DownpaymentAssistanceProvider:
            ((BranchLicensing) this.licensing).ATRExemptCreditor = (BranchLicensing.ATRExemptCreditors) 3;
            break;
          case ATRExemptCreditors.NonprofitOrganization:
            ((BranchLicensing) this.licensing).ATRExemptCreditor = (BranchLicensing.ATRExemptCreditors) 4;
            break;
          default:
            ((BranchLicensing) this.licensing).ATRExemptCreditor = (BranchLicensing.ATRExemptCreditors) 0;
            break;
        }
      }
    }

    public static ATRExemptCreditors ATRExemptCreditorToEnum(int value)
    {
      try
      {
        return (ATRExemptCreditors) Enum.Parse(typeof (ATRExemptCreditors), value.ToString(), true);
      }
      catch
      {
        return ATRExemptCreditors.None;
      }
    }

    public string ATRExemptCreditorToString()
    {
      switch (((BranchLicensing) this.licensing).ATRExemptCreditor - 1)
      {
        case 0:
          return "Community Development Financial Institution";
        case 1:
          return "Community Housing Development Organization";
        case 2:
          return "Downpayment Assistance Provider";
        case 3:
          return "Nonprofit Organization";
        default:
          return "";
      }
    }

    public int AllowLoansWithIssues
    {
      get => this.licensing.AllowLoansWithIssues;
      set => this.licensing.AllowLoansWithIssues = value;
    }

    public string MsgUploadNonApprovedLoans
    {
      get => this.licensing.MsgUploadNonApprovedLoans;
      set => this.licensing.MsgUploadNonApprovedLoans = value;
    }

    public List<StateLicenseExtType> StateLicenseExtTypes
    {
      get
      {
        if (this.licensing.StateLicenseExtTypes == null)
          return (List<StateLicenseExtType>) null;
        List<StateLicenseExtType> stateLicenseExtTypes = new List<StateLicenseExtType>();
        for (int index = 0; index < this.licensing.StateLicenseExtTypes.Count; ++index)
          stateLicenseExtTypes.Add(new StateLicenseExtType(((StateLicenseType) this.licensing.StateLicenseExtTypes[index]).StateAbbrevation, ((StateLicenseType) this.licensing.StateLicenseExtTypes[index]).LicenseType, this.licensing.StateLicenseExtTypes[index].LicenseNo, this.licensing.StateLicenseExtTypes[index].IssueDate, this.licensing.StateLicenseExtTypes[index].StartDate, this.licensing.StateLicenseExtTypes[index].EndDate, this.licensing.StateLicenseExtTypes[index].LicenseStatus, this.licensing.StateLicenseExtTypes[index].StatusDate, this.licensing.StateLicenseExtTypes[index].Approved, ((StateLicenseType) this.licensing.StateLicenseExtTypes[index]).Exempt, this.licensing.StateLicenseExtTypes[index].LastChecked));
        return stateLicenseExtTypes;
      }
    }

    public void AddStateLicenseExtType(StateLicenseExtType stateLicenseExtType)
    {
      this.licensing.AddStateLicenseExtType(new StateLicenseExtType(stateLicenseExtType.StateAbbrevation, stateLicenseExtType.LicenseType, stateLicenseExtType.LicenseNo, stateLicenseExtType.IssueDate, stateLicenseExtType.StartDate, stateLicenseExtType.EndDate, stateLicenseExtType.LicenseStatus, stateLicenseExtType.StatusDate, stateLicenseExtType.Approved, stateLicenseExtType.Exempt, stateLicenseExtType.LastChecked, stateLicenseExtType.SortIndex));
    }
  }
}
