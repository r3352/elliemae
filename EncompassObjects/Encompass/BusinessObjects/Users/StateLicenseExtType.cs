// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Represents a state license setting</summary>
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

    /// <summary>Represents a state license setting</summary>
    /// <param name="stateAbbrevation">State Abbrevation</param>
    /// <param name="licenseType">license type</param>
    /// <param name="licenseNo">license number</param>
    /// <param name="issueDate">issue date</param>
    /// <param name="startDate">start date</param>
    /// <param name="endDate">end date</param>
    /// <param name="licenseStatus">license status</param>
    /// <param name="statusDate">status date</param>
    /// <param name="approved">flag indicates if approved</param>
    /// <param name="exempt">flag indicates if exempted</param>
    /// <param name="lastChecked">date time last checked</param>
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

    /// <summary>Gets or sets state abbrevation</summary>
    public string StateAbbrevation
    {
      get => this.stateAbbrevation;
      set => this.stateAbbrevation = value;
    }

    /// <summary>Gets or sets license type</summary>
    public string LicenseType
    {
      get => this.licenseType;
      set => this.licenseType = value;
    }

    /// <summary>Gets or sets selected flag</summary>
    public bool Selected
    {
      get => this.selected;
      set => this.selected = value;
    }

    /// <summary>Gets or sets exempt flag</summary>
    public bool Exempt
    {
      get => this.exempt;
      set => this.exempt = value;
    }

    /// <summary>Gets or sets license number</summary>
    public string LicenseNo
    {
      get => this.licenseNo;
      set => this.licenseNo = value;
    }

    /// <summary>Gets or sets date issued</summary>
    public DateTime IssueDate
    {
      get => this.issueDate;
      set => this.issueDate = value;
    }

    /// <summary>Gets or sets date started</summary>
    public DateTime StartDate
    {
      get => this.startDate;
      set => this.startDate = value;
    }

    /// <summary>Gets or sets date ends</summary>
    public DateTime EndDate
    {
      get => this.endDate;
      set => this.endDate = value;
    }

    /// <summary>Gets or sets license status</summary>
    public string LicenseStatus
    {
      get => this.licenseStatus;
      set => this.licenseStatus = value;
    }

    /// <summary>Gets or sets status date</summary>
    public DateTime StatusDate
    {
      get => this.statusDate;
      set => this.statusDate = value;
    }

    /// <summary>Gets or sets approved flag</summary>
    public bool Approved
    {
      get => this.Selected;
      set => this.Selected = value;
    }

    /// <summary>Gets or sets date last checked</summary>
    public DateTime LastChecked
    {
      get => this.lastChecked;
      set => this.lastChecked = value;
    }

    /// <summary>Gets or sets sorting index</summary>
    public int SortIndex
    {
      get => this.sortIndex;
      set => this.sortIndex = value;
    }
  }
}
