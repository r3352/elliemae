// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.StateLicenses
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class StateLicenses : SessionBoundObject, IStateLicenses, IEnumerable
  {
    private IOrganizationManager mngr;
    private User parentUser;
    private Hashtable licenses = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal StateLicenses(User parentUser)
      : base(parentUser.Session)
    {
      this.mngr = (IOrganizationManager) this.Session.GetObject("OrganizationManager");
      this.parentUser = parentUser;
      this.Refresh();
    }

    public StateLicense this[string state]
    {
      get
      {
        return (state ?? "").Length == 2 ? (StateLicense) this.licenses[(object) state] : throw new ArgumentException("Invalid state specified");
      }
    }

    public StateLicense Add(string state)
    {
      if ((state ?? "").Length != 2)
        throw new ArgumentException("Invalid state specified");
      if (!this.licenses.Contains((object) state))
        this.licenses.Add((object) state, (object) new StateLicense(new LOLicenseInfo(this.parentUser.ID, state)));
      return (StateLicense) this.licenses[(object) state];
    }

    public StateLicense Add(
      string state,
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
      if ((state ?? "").Length != 2)
        throw new ArgumentException("Invalid state specified");
      if (!this.licenses.Contains((object) state))
        this.licenses.Add((object) state, (object) new StateLicense(new LOLicenseInfo(new StateLicenseExtType(state, string.Empty, licenseNo, issueDate, startDate, endDate, licenseStatus, statusDate, approved, exempt, lastChecked))
        {
          UserID = this.parentUser.ID
        }));
      return (StateLicense) this.licenses[(object) state];
    }

    public void Remove(string state)
    {
      if (!this.licenses.Contains((object) state))
        return;
      this.licenses.Remove((object) state);
    }

    public void Clear() => this.licenses.Clear();

    public IEnumerator GetEnumerator() => this.licenses.Values.GetEnumerator();

    internal void Commit()
    {
      LOLicenseInfo[] loLicenseInfoArray = new LOLicenseInfo[this.licenses.Count];
      int num = 0;
      foreach (StateLicense stateLicense in (IEnumerable) this.licenses.Values)
      {
        LOLicenseInfo loLicenseInfo = new LOLicenseInfo(this.parentUser.ID, stateLicense.Unwrap());
        ((StateLicenseType) loLicenseInfo).Selected = ((StateLicenseExtType) loLicenseInfo).Approved = stateLicense.Selected;
        ((StateLicenseExtType) loLicenseInfo).EndDate = stateLicense.ExpirationDate == null ? DateTime.MinValue : (DateTime) stateLicense.ExpirationDate;
        ((StateLicenseType) loLicenseInfo).Exempt = stateLicense.Exempt;
        ((StateLicenseExtType) loLicenseInfo).IssueDate = stateLicense.IssueDate == null ? DateTime.MinValue : (DateTime) stateLicense.IssueDate;
        ((StateLicenseExtType) loLicenseInfo).LastChecked = stateLicense.LastChecked == null ? DateTime.MinValue : (DateTime) stateLicense.LastChecked;
        ((StateLicenseExtType) loLicenseInfo).LicenseStatus = stateLicense.LicenseStatus;
        ((StateLicenseExtType) loLicenseInfo).StartDate = stateLicense.StartDate == null ? DateTime.MinValue : (DateTime) stateLicense.StartDate;
        ((StateLicenseExtType) loLicenseInfo).StatusDate = stateLicense.StatusDate == null ? DateTime.MinValue : (DateTime) stateLicense.StatusDate;
        loLicenseInfoArray[num++] = loLicenseInfo;
      }
      this.mngr.SetLOLicenses(this.parentUser.ID, loLicenseInfoArray);
    }

    internal void Refresh()
    {
      LOLicenseInfo[] loLicenses = this.mngr.GetLOLicenses(this.parentUser.ID);
      this.licenses.Clear();
      foreach (LOLicenseInfo license in loLicenses)
        this.licenses[(object) license.StateAbbr] = (object) new StateLicense(license);
    }
  }
}
