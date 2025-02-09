// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ExternalLicensing
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Represents a loan officers license to originate loans in a particular state.
  /// </summary>
  [Guid("E8847A40-C5B7-471E-BB85-D67A5C214144")]
  public class ExternalLicensing : IExternalLicensing
  {
    private BranchExtLicensing licensing;

    internal BranchExtLicensing BranchExternalLicensing => this.licensing;

    /// <summary>constructor</summary>
    /// <param name="useParentInfo">Flag indicates to use parent info</param>
    /// <param name="allowLoansWithIssues">Flag indicates loans with issues to be submitted.</param>
    /// <param name="msgUploadNonApprovedLoans">Message for uploading unapproved loans</param>
    /// <param name="lenderType">Lender type</param>
    /// <param name="homeState">Home state</param>
    /// <param name="optOut">OptOut</param>
    /// <param name="statutoryElectionInMaryland">Flag for statutory election in Maryland</param>
    /// <param name="statutoryElectionInMaryland2">Additonal value of statutory election in Maryland</param>
    /// <param name="statutoryElectionInKansas">Flag for statutory election in Kansas</param>
    /// <param name="stateLicense">List of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType">StateLicenseExtType</see></param>
    /// <param name="useCustomLenderProfile">Flag indicates use custom lender profile</param>
    /// <param name="atrSmallCreditor"><see cref="T:EllieMae.Encompass.BusinessObjects.Users.ATRSmallCreditors">ATRSmallCreditors</see> setting</param>
    /// <param name="atrExemptCreditor"><see cref="T:EllieMae.Encompass.BusinessObjects.Users.ATRExemptCreditors">ATRExemptCreditors</see> setting</param>
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
      this.licensing = new BranchExtLicensing(useParentInfo, allowLoansWithIssues, msgUploadNonApprovedLoans, lenderType, homeState, optOut, statutoryElectionInMaryland, statutoryElectionInMaryland2, statutoryElectionInKansas, (List<EllieMae.EMLite.RemotingServices.StateLicenseExtType>) null, useCustomLenderProfile, BranchLicensing.ATRSmallCreditors.None, BranchLicensing.ATRExemptCreditors.None);
      this.ATRSmallCreditor = atrSmallCreditor;
      this.ATRExemptCreditor = atrExemptCreditor;
      if (stateLicense == null)
        return;
      for (int index = 0; index < stateLicense.Count; ++index)
        this.licensing.AddStateLicenseExtType(new EllieMae.EMLite.RemotingServices.StateLicenseExtType(stateLicense[index].StateAbbrevation, stateLicense[index].LicenseType, stateLicense[index].LicenseNo, stateLicense[index].IssueDate, stateLicense[index].StartDate, stateLicense[index].EndDate, stateLicense[index].LicenseStatus, stateLicense[index].StatusDate, stateLicense[index].Approved, stateLicense[index].Exempt, stateLicense[index].LastChecked, stateLicense[index].SortIndex));
    }

    internal ExternalLicensing(BranchExtLicensing licensing) => this.licensing = licensing;

    /// <summary>Gets or sets user parent info</summary>
    public bool UseParentInfo
    {
      get => this.licensing.UseParentInfo;
      set => this.licensing.UseParentInfo = value;
    }

    /// <summary>Gets or sets lender type</summary>
    public string LenderType
    {
      get => this.licensing.LenderType;
      set => this.licensing.LenderType = value;
    }

    /// <summary>Gets or sets state information</summary>
    public string HomeState
    {
      get => this.licensing.HomeState;
      set => this.licensing.HomeState = value;
    }

    /// <summary>Gets or sets optout option</summary>
    public string OptOut
    {
      get => this.licensing.OptOut;
      set => this.licensing.OptOut = value;
    }

    /// <summary>Gets or sets flag for Statutory Election in Maryland</summary>
    public bool StatutoryElectionInMaryland
    {
      get => this.licensing.StatutoryElectionInMaryland;
      set => this.licensing.StatutoryElectionInMaryland = value;
    }

    /// <summary>
    /// Gets or sets additional value of Statutory Election in Maryland
    /// </summary>
    public string StatutoryElectionInMaryland2
    {
      get => this.licensing.StatutoryElectionInMaryland2;
      set => this.licensing.StatutoryElectionInMaryland2 = value;
    }

    /// <summary>Gets or sets flag for Statutory Election in Kansas</summary>
    public bool StatutoryElectionInKansas
    {
      get => this.licensing.StatutoryElectionInKansas;
      set => this.licensing.StatutoryElectionInKansas = value;
    }

    /// <summary>Gets or sets flag to use custom lender profile</summary>
    public bool UseCustomLenderProfile
    {
      get => this.licensing.UseCustomLenderProfile;
      set => this.licensing.UseCustomLenderProfile = value;
    }

    /// <summary>Gets or sets ATR small creditor</summary>
    public ATRSmallCreditors ATRSmallCreditor
    {
      get
      {
        switch (this.licensing.ATRSmallCreditor)
        {
          case BranchLicensing.ATRSmallCreditors.SmallCreditor:
            return ATRSmallCreditors.SmallCreditor;
          case BranchLicensing.ATRSmallCreditors.RuralSmallCreditor:
            return ATRSmallCreditors.RuralSmallCreditor;
          default:
            return ATRSmallCreditors.None;
        }
      }
      set
      {
        if (value != ATRSmallCreditors.SmallCreditor)
        {
          if (value == ATRSmallCreditors.RuralSmallCreditor)
            this.licensing.ATRSmallCreditor = BranchLicensing.ATRSmallCreditors.RuralSmallCreditor;
          else
            this.licensing.ATRSmallCreditor = BranchLicensing.ATRSmallCreditors.None;
        }
        else
          this.licensing.ATRSmallCreditor = BranchLicensing.ATRSmallCreditors.SmallCreditor;
      }
    }

    /// <summary>
    /// Convert integer value to corresponding ATRSmallCreditors enum value
    /// </summary>
    /// <param name="value">integer value of the enum representation</param>
    /// <returns>ATRSmallCreditors type</returns>
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

    /// <summary>Returns display value of ATRSmallCreditor setting</summary>
    /// <returns>string representation of ATRSmallCreditors setting</returns>
    public string ATRSmallCreditorToString()
    {
      switch (this.licensing.ATRSmallCreditor)
      {
        case BranchLicensing.ATRSmallCreditors.SmallCreditor:
          return "Small Creditor";
        case BranchLicensing.ATRSmallCreditors.RuralSmallCreditor:
          return "Rural Small Creditor";
        default:
          return "";
      }
    }

    /// <summary>Gets or sets ATR Exempt Creditor</summary>
    public ATRExemptCreditors ATRExemptCreditor
    {
      get
      {
        switch (this.licensing.ATRExemptCreditor)
        {
          case BranchLicensing.ATRExemptCreditors.CommunityDevelopmentFinancialInstitution:
            return ATRExemptCreditors.CommunityDevelopmentFinancialInstitution;
          case BranchLicensing.ATRExemptCreditors.CommunityHousingDevelopmentOrganization:
            return ATRExemptCreditors.CommunityHousingDevelopmentOrganization;
          case BranchLicensing.ATRExemptCreditors.DownpaymentAssistanceProvider:
            return ATRExemptCreditors.DownpaymentAssistanceProvider;
          case BranchLicensing.ATRExemptCreditors.NonprofitOrganization:
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
            this.licensing.ATRExemptCreditor = BranchLicensing.ATRExemptCreditors.CommunityDevelopmentFinancialInstitution;
            break;
          case ATRExemptCreditors.CommunityHousingDevelopmentOrganization:
            this.licensing.ATRExemptCreditor = BranchLicensing.ATRExemptCreditors.CommunityHousingDevelopmentOrganization;
            break;
          case ATRExemptCreditors.DownpaymentAssistanceProvider:
            this.licensing.ATRExemptCreditor = BranchLicensing.ATRExemptCreditors.DownpaymentAssistanceProvider;
            break;
          case ATRExemptCreditors.NonprofitOrganization:
            this.licensing.ATRExemptCreditor = BranchLicensing.ATRExemptCreditors.NonprofitOrganization;
            break;
          default:
            this.licensing.ATRExemptCreditor = BranchLicensing.ATRExemptCreditors.None;
            break;
        }
      }
    }

    /// <summary>
    /// Convert integer value to corresponding ATRExemptCreditor enum value
    /// </summary>
    /// <param name="value">integer value of the enum representation</param>
    /// <returns>ATRExemptCreditors type</returns>
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

    /// <summary>Return display value of ATRExemptCreditor</summary>
    /// <returns>string representation of ATRExemptCreditors setting</returns>
    public string ATRExemptCreditorToString()
    {
      switch (this.licensing.ATRExemptCreditor)
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

    /// <summary>Gets or sets a flag to allow loans with issues</summary>
    public int AllowLoansWithIssues
    {
      get => this.licensing.AllowLoansWithIssues;
      set => this.licensing.AllowLoansWithIssues = value;
    }

    /// <summary>
    /// Gets or sets warning message for uploading non-approved loans
    /// </summary>
    public string MsgUploadNonApprovedLoans
    {
      get => this.licensing.MsgUploadNonApprovedLoans;
      set => this.licensing.MsgUploadNonApprovedLoans = value;
    }

    /// <summary>
    /// Gets a list of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType"></see>
    /// </summary>
    public List<StateLicenseExtType> StateLicenseExtTypes
    {
      get
      {
        if (this.licensing.StateLicenseExtTypes == null)
          return (List<StateLicenseExtType>) null;
        List<StateLicenseExtType> stateLicenseExtTypes = new List<StateLicenseExtType>();
        for (int index = 0; index < this.licensing.StateLicenseExtTypes.Count; ++index)
          stateLicenseExtTypes.Add(new StateLicenseExtType(this.licensing.StateLicenseExtTypes[index].StateAbbrevation, this.licensing.StateLicenseExtTypes[index].LicenseType, this.licensing.StateLicenseExtTypes[index].LicenseNo, this.licensing.StateLicenseExtTypes[index].IssueDate, this.licensing.StateLicenseExtTypes[index].StartDate, this.licensing.StateLicenseExtTypes[index].EndDate, this.licensing.StateLicenseExtTypes[index].LicenseStatus, this.licensing.StateLicenseExtTypes[index].StatusDate, this.licensing.StateLicenseExtTypes[index].Approved, this.licensing.StateLicenseExtTypes[index].Exempt, this.licensing.StateLicenseExtTypes[index].LastChecked));
        return stateLicenseExtTypes;
      }
    }

    /// <summary>Add a new state license to the license list</summary>
    /// <param name="stateLicenseExtType">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType">StateLicenseExtType</see> to add.</param>
    public void AddStateLicenseExtType(StateLicenseExtType stateLicenseExtType)
    {
      this.licensing.AddStateLicenseExtType(new EllieMae.EMLite.RemotingServices.StateLicenseExtType(stateLicenseExtType.StateAbbrevation, stateLicenseExtType.LicenseType, stateLicenseExtType.LicenseNo, stateLicenseExtType.IssueDate, stateLicenseExtType.StartDate, stateLicenseExtType.EndDate, stateLicenseExtType.LicenseStatus, stateLicenseExtType.StatusDate, stateLicenseExtType.Approved, stateLicenseExtType.Exempt, stateLicenseExtType.LastChecked, stateLicenseExtType.SortIndex));
    }
  }
}
