// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.LOLicenseInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class LOLicenseInfo : StateLicenseExtType
  {
    private string userId;

    public LOLicenseInfo(
      string userId,
      string state,
      bool enabled,
      string license,
      DateTime expirationDate)
      : base(state, string.Empty, license, DateTime.MinValue, DateTime.MinValue, expirationDate, string.Empty, DateTime.MinValue, enabled, false, DateTime.MinValue)
    {
      this.userId = userId.ToLower();
      this.StateAbbrevation = state;
      this.Approved = enabled;
      this.LicenseNo = license;
      this.EndDate = expirationDate;
    }

    public LOLicenseInfo(string userId, string state)
      : base(state, string.Empty, string.Empty, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, string.Empty, DateTime.MinValue, false, false, DateTime.MinValue)
    {
      this.userId = userId.ToLower();
      this.StateAbbrevation = state;
      this.Approved = false;
      this.LicenseNo = "";
      this.EndDate = DateTime.MaxValue;
    }

    public LOLicenseInfo(string userId, LOLicenseInfo source)
      : base(source.StateAbbrevation, string.Empty, source.LicenseNo, DateTime.MinValue, DateTime.MinValue, source.ExpirationDate, string.Empty, DateTime.MinValue, source.Approved, false, DateTime.MinValue)
    {
      this.userId = userId.ToLower();
      this.StateAbbrevation = source.StateAbbrevation;
      this.Approved = source.Approved;
      this.LicenseNo = source.LicenseNo;
      this.EndDate = source.EndDate;
    }

    public LOLicenseInfo(StateLicenseExtType source)
      : base(source.StateAbbrevation, source.LicenseType, source.LicenseNo, source.IssueDate, source.StartDate, source.EndDate, source.LicenseStatus, source.StatusDate, source.Approved, source.Exempt, source.LastChecked)
    {
    }

    public string UserID
    {
      set => this.userId = value;
      get => this.userId;
    }

    public string StateAbbr => this.StateAbbrevation;

    public bool Enabled
    {
      get => this.Approved;
      set => this.Approved = value;
    }

    public string License
    {
      get => this.LicenseNo;
      set => this.LicenseNo = value;
    }

    public DateTime ExpirationDate
    {
      get => this.EndDate;
      set => this.EndDate = value;
    }
  }
}
