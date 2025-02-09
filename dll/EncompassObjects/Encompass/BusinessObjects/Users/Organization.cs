// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Organization
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class Organization : SessionBoundObject, IOrganization
  {
    private IOrganizationManager mngr;
    private OrgInfo info;
    private OrgInfo topParent;
    private bool useParentInfo;
    private bool fetchedTopParent;
    private ScopedEventHandler<PersistentObjectEventArgs> committed;

    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    internal Organization(Session session, OrgInfo info)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Organization), "Committed");
      this.mngr = (IOrganizationManager) session.GetObject("OrganizationManager");
      this.info = info;
    }

    public bool UseParentInfo
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.useParentInfo;
      }
      set
      {
        if (value == this.useParentInfo)
          return;
        this.useParentInfo = value;
        if (this.useParentInfo)
        {
          this.ClearValues();
          this.GetTopParent(true);
        }
        else
          this.CopyFromParent();
      }
    }

    private void ClearValues()
    {
      this.info.CompanyName = this.info.OrgCode = this.info.CompanyPhone = this.info.CompanyFax = this.info.DBAName1 = this.info.DBAName2 = this.info.DBAName3 = this.info.DBAName4 = string.Empty;
      this.info.CompanyAddress.Street1 = this.info.CompanyAddress.Street2 = this.info.CompanyAddress.City = this.info.CompanyAddress.State = this.info.CompanyAddress.Zip = string.Empty;
    }

    private void CopyFromParent()
    {
      this.info.CompanyName = this.topParent.CompanyName;
      this.info.OrgCode = this.topParent.OrgCode;
      this.info.CompanyPhone = this.topParent.CompanyPhone;
      this.info.CompanyFax = this.topParent.CompanyFax;
      this.info.DBAName1 = this.topParent.DBAName1;
      this.info.DBAName2 = this.topParent.DBAName2;
      this.info.DBAName3 = this.topParent.DBAName3;
      this.info.DBAName4 = this.topParent.DBAName4;
      this.info.CompanyAddress.Street1 = this.topParent.CompanyAddress.Street1;
      this.info.CompanyAddress.Street2 = this.topParent.CompanyAddress.Street2;
      this.info.CompanyAddress.City = this.topParent.CompanyAddress.City;
      this.info.CompanyAddress.State = this.topParent.CompanyAddress.State;
      this.info.CompanyAddress.Zip = this.topParent.CompanyAddress.Zip;
    }

    public int ID => this.info.Oid;

    public string Name
    {
      get => this.info.OrgName;
      set
      {
        this.info.OrgName = !((value ?? "") == "") ? value : throw new ArgumentException("Invalid organization name");
      }
    }

    public string OrgCode
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.OrgCode : this.info.OrgCode;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.OrgCode = value ?? "";
      }
    }

    public Address CompanyAddress
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return new Address(this.info.CompanyAddress, this.topParent.CompanyAddress, this);
      }
    }

    public string CompanyName
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.CompanyName : this.info.CompanyName;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.CompanyName = value ?? "";
      }
    }

    public string CompanyPhone
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.CompanyPhone : this.info.CompanyPhone;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.CompanyPhone = value ?? "";
      }
    }

    public string CompanyFax
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.CompanyFax : this.info.CompanyFax;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.CompanyFax = value ?? "";
      }
    }

    public string DBAName1
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.DBAName1 : this.info.DBAName1;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.DBAName1 = value ?? "";
      }
    }

    public string DBAName2
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.DBAName2 : this.info.DBAName2;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.DBAName2 = value ?? "";
      }
    }

    public string DBAName3
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.DBAName3 : this.info.DBAName3;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.DBAName3 = value ?? "";
      }
    }

    public string DBAName4
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return this.UseParentInfo ? this.topParent.DBAName4 : this.info.DBAName4;
      }
      set
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        if (this.UseParentInfo)
          this.UseParentInfo = false;
        this.info.DBAName4 = value ?? "";
      }
    }

    public string Description
    {
      get => this.info.Description;
      set => this.info.Description = value ?? "";
    }

    public void Commit()
    {
      this.ensureAdmin();
      if (this.IsNew)
      {
        this.info = this.mngr.GetOrganization(this.mngr.CreateOrganization(this.info));
      }
      else
      {
        OrgInfo organization = this.mngr.GetOrganization(this.info.Oid);
        if (this.info.SSOSettings != null && organization.SSOSettings.LoginAccess != this.info.SSOSettings.LoginAccess)
        {
          EventLog eventLog = new EventLog()
          {
            Log = "Application",
            Source = "Encompass SDK"
          };
          eventLog.WriteEntry("Updates made to SSO Access settings will only be applied to users that have not been customized.", EventLogEntryType.Warning);
          if (this.info.SSOSettings.LoginAccess)
            eventLog.WriteEntry("Password is required for Full Access login. Please review that all updated users in this organization have passwords.", EventLogEntryType.Warning);
        }
        this.mngr.UpdateOrganization(this.info);
      }
      this.committed((object) this, new PersistentObjectEventArgs(this.ID.ToString()));
    }

    public void Delete()
    {
      this.ensureAdmin();
      this.ensureExists();
      if (this.IsTopMostOrganization)
        throw new InvalidOperationException("The root organization cannot be deleted");
      this.mngr.DeleteOrganization(this.info.Oid);
      this.info = (OrgInfo) null;
    }

    public void Refresh()
    {
      this.ensureExists();
      this.info = this.mngr.GetOrganization(this.info.Oid);
    }

    public bool IsNew => this.info.Oid < 0;

    public bool IsTopMostOrganization => this.info.Oid == this.info.Parent;

    private void GetTopParent(bool fromParent = false)
    {
      this.fetchedTopParent = true;
      this.topParent = this.mngr.GetFirstAvaliableOrganization(fromParent ? this.info.Parent : this.info.Oid, true);
      if (this.topParent.Oid == this.info.Oid)
        return;
      this.useParentInfo = true;
    }

    public Organization GetParentOrganization()
    {
      return this.IsTopMostOrganization ? (Organization) null : new Organization(this.Session, this.mngr.GetOrganization(this.info.Parent));
    }

    public OrganizationList GetSuborganizations()
    {
      return this.info.Children == null ? new OrganizationList() : Organization.ToList(this.Session, this.mngr.GetOrganizations(this.info.Children));
    }

    public Organization CreateSuborganization(string name)
    {
      this.ensureExists();
      OrgInfo info = new OrgInfo(-1, name, "", this.info.Oid, new int[0]);
      OrgInfo orgInfo = info;
      BranchExtLicensing branchExtLicensing = new BranchExtLicensing();
      ((BranchLicensing) branchExtLicensing).UseParentInfo = true;
      orgInfo.OrgBranchLicensing = branchExtLicensing;
      return new Organization(this.Session, info);
    }

    public UserList GetUsers()
    {
      this.ensureExists();
      return User.ToList(this.Session, this.mngr.GetUsersInOrganization(this.info.Oid));
    }

    public User CreateUser(string userId, string password, PersonaList personas)
    {
      this.ensureAdmin();
      this.ensureExists();
      if (personas == null)
        throw new ArgumentNullException(nameof (personas));
      if (personas.Count == 0)
        throw new ArgumentException("Persona list must contain at least one Persona");
      if (!UserInfo.IsValidUserID(userId))
        throw new ArgumentException("The format of the specified User ID is invalid.");
      return new User(this.Session, new UserInfo(userId, password, this.info.Oid, personas.Unwrap())
      {
        SSOOnly = this.info.SSOSettings.LoginAccess
      }, true);
    }

    public UserGroupList GetUserGroups()
    {
      AclGroup[] groupsOfOrganization = ((IAclGroupManager) this.Session.GetObject("AclGroupManager")).GetGroupsOfOrganization(this.ID);
      UserGroupList userGroups = new UserGroupList();
      foreach (AclGroup aclGroup in groupsOfOrganization)
      {
        UserGroup groupById = this.Session.Users.Groups.GetGroupByID(aclGroup.ID);
        if (groupById != null)
          userGroups.Add(groupById);
      }
      return userGroups;
    }

    public override string ToString() => this.info.OrgName;

    public override int GetHashCode() => this.ID;

    public override bool Equals(object obj)
    {
      return !object.Equals((object) (obj as Organization), (object) null) && ((SessionBoundObject) obj).Session == this.Session && ((Organization) obj).ID == this.ID;
    }

    private void ensureExists()
    {
      if (this.IsNew)
        throw new InvalidOperationException("This operation is not valid until object is commited");
    }

    internal OrgInfo Unwrap() => this.info;

    private void ensureAdmin()
    {
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
    }

    internal static OrganizationList ToList(Session session, OrgInfo[] infos)
    {
      OrganizationList list = new OrganizationList();
      for (int index = 0; index < infos.Length; ++index)
        list.Add(new Organization(session, infos[index]));
      return list;
    }

    public bool SSO_UseParentInfo
    {
      get => this.info.SSOSettings.UseParentInfo;
      set => this.info.SSOSettings.UseParentInfo = value;
    }

    public bool SSO_LoginAccess
    {
      get
      {
        if (!this.SSO_UseParentInfo)
          return this.info.SSOSettings.LoginAccess;
        OrgInfo organizationForSso = this.mngr.GetFirstOrganizationForSSO(this.info.Oid);
        return organizationForSso != null && organizationForSso.SSOSettings.LoginAccess;
      }
      set => this.info.SSOSettings.LoginAccess = value;
    }
  }
}
