// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.StateLicenseExtType
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class StateLicenseExtType : StateLicenseType
  {
    private string licenseNo = string.Empty;
    private DateTime issueDate = DateTime.MinValue;
    private DateTime startDate = DateTime.MinValue;
    private DateTime endDate = DateTime.MinValue;
    private string licenseStatus = string.Empty;
    private DateTime statusDate = DateTime.MinValue;
    private DateTime lastChecked = DateTime.MinValue;
    private int sortIndex;

    public StateLicenseExtType(
      string stateAbbrevation,
      string licenseType,
      bool approved,
      bool exempt)
      : this(stateAbbrevation, licenseType, "", DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "", DateTime.MinValue, approved, exempt, DateTime.MinValue, 0)
    {
    }

    public StateLicenseExtType(
      string stateAbbrevation,
      string licenseType,
      string licenseNo,
      DateTime issueDate,
      DateTime startDate,
      DateTime endDate,
      string licenseStatus,
      DateTime statusDate,
      bool approved,
      bool exempt,
      DateTime lastChecked)
    {
      this.StateAbbrevation = stateAbbrevation;
      this.LicenseType = licenseType;
      this.licenseNo = licenseNo;
      this.issueDate = issueDate;
      this.startDate = startDate;
      this.endDate = endDate;
      this.licenseStatus = licenseStatus;
      this.statusDate = statusDate;
      this.Selected = approved;
      this.Exempt = exempt;
      this.lastChecked = lastChecked;
      this.sortIndex = 0;
    }

    public StateLicenseExtType(
      string stateAbbrevation,
      string licenseType,
      string licenseNo,
      DateTime issueDate,
      DateTime startDate,
      DateTime endDate,
      string licenseStatus,
      DateTime statusDate,
      bool approved,
      bool exempt,
      DateTime lastChecked,
      int sortIndex)
    {
      this.StateAbbrevation = stateAbbrevation;
      this.LicenseType = licenseType;
      this.licenseNo = licenseNo;
      this.issueDate = issueDate;
      this.startDate = startDate;
      this.endDate = endDate;
      this.licenseStatus = licenseStatus;
      this.statusDate = statusDate;
      this.Selected = approved;
      this.Exempt = exempt;
      this.lastChecked = lastChecked;
      this.sortIndex = sortIndex;
    }

    public string LicenseNo
    {
      get => this.licenseNo;
      set => this.licenseNo = value;
    }

    public DateTime IssueDate
    {
      get => this.issueDate;
      set => this.issueDate = value;
    }

    public DateTime StartDate
    {
      get => this.startDate;
      set => this.startDate = value;
    }

    public DateTime EndDate
    {
      get => this.endDate;
      set => this.endDate = value;
    }

    public string LicenseStatus
    {
      get => this.licenseStatus;
      set => this.licenseStatus = value;
    }

    public DateTime StatusDate
    {
      get => this.statusDate;
      set => this.statusDate = value;
    }

    public bool Approved
    {
      get => this.Selected;
      set => this.Selected = value;
    }

    public DateTime LastChecked
    {
      get => this.lastChecked;
      set => this.lastChecked = value;
    }

    public int SortIndex
    {
      get => this.sortIndex;
      set => this.sortIndex = value;
    }
  }
}
