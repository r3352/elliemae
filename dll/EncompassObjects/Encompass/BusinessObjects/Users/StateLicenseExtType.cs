// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("DCF77EE1-8874-421E-874F-6078E16C9BDE")]
  public class StateLicenseExtType
  {
    private string stateAbbrevation = string.Empty;
    private string licenseType = string.Empty;
    private bool selected;
    private bool exempt;
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
    }

    public string StateAbbrevation
    {
      get => this.stateAbbrevation;
      set => this.stateAbbrevation = value;
    }

    public string LicenseType
    {
      get => this.licenseType;
      set => this.licenseType = value;
    }

    public bool Selected
    {
      get => this.selected;
      set => this.selected = value;
    }

    public bool Exempt
    {
      get => this.exempt;
      set => this.exempt = value;
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
