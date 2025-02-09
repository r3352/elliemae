// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.StateLicense
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class StateLicense : IStateLicense
  {
    private LOLicenseInfo license;

    internal StateLicense(LOLicenseInfo license) => this.license = license;

    public string State => this.license.StateAbbr;

    public string LicenseNumber
    {
      get => this.license.License;
      set => this.license.License = value ?? "";
    }

    public object ExpirationDate
    {
      get
      {
        return !(this.license.ExpirationDate == DateTime.MaxValue) ? (object) this.license.ExpirationDate : (object) null;
      }
      set
      {
        if (value == null)
          this.license.ExpirationDate = DateTime.MaxValue;
        else
          this.license.ExpirationDate = Convert.ToDateTime(value);
      }
    }

    void IStateLicense.SetExpirationDate(object value) => this.ExpirationDate = value;

    public bool Enabled
    {
      get => this.license.Enabled;
      set => this.license.Enabled = value;
    }

    public bool Selected
    {
      get => ((StateLicenseType) this.license).Selected;
      set => ((StateLicenseType) this.license).Selected = value;
    }

    public bool Exempt
    {
      get => ((StateLicenseType) this.license).Exempt;
      set => ((StateLicenseType) this.license).Exempt = value;
    }

    public object IssueDate
    {
      get
      {
        return !(((StateLicenseExtType) this.license).IssueDate == DateTime.MaxValue) ? (object) ((StateLicenseExtType) this.license).IssueDate : (object) null;
      }
      set
      {
        if (value == null)
          ((StateLicenseExtType) this.license).IssueDate = DateTime.MaxValue;
        else
          ((StateLicenseExtType) this.license).IssueDate = Convert.ToDateTime(value);
      }
    }

    public object StartDate
    {
      get
      {
        return !(((StateLicenseExtType) this.license).StartDate == DateTime.MaxValue) ? (object) ((StateLicenseExtType) this.license).StartDate : (object) null;
      }
      set
      {
        if (value == null)
          ((StateLicenseExtType) this.license).StartDate = DateTime.MaxValue;
        else
          ((StateLicenseExtType) this.license).StartDate = Convert.ToDateTime(value);
      }
    }

    public string LicenseStatus
    {
      get => ((StateLicenseExtType) this.license).LicenseStatus;
      set => ((StateLicenseExtType) this.license).LicenseStatus = value;
    }

    public object StatusDate
    {
      get
      {
        return !(((StateLicenseExtType) this.license).StatusDate == DateTime.MaxValue) ? (object) ((StateLicenseExtType) this.license).StatusDate : (object) null;
      }
      set
      {
        if (value == null)
          ((StateLicenseExtType) this.license).StatusDate = DateTime.MaxValue;
        else
          ((StateLicenseExtType) this.license).StatusDate = Convert.ToDateTime(value);
      }
    }

    public object LastChecked
    {
      get
      {
        return !(((StateLicenseExtType) this.license).LastChecked == DateTime.MaxValue) ? (object) ((StateLicenseExtType) this.license).LastChecked : (object) null;
      }
      set
      {
        if (value == null)
          ((StateLicenseExtType) this.license).LastChecked = DateTime.MaxValue;
        else
          ((StateLicenseExtType) this.license).LastChecked = Convert.ToDateTime(value);
      }
    }

    internal LOLicenseInfo Unwrap() => this.license;
  }
}
