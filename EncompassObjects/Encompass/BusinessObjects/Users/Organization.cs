// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Organization
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Represents an item in the Encompass organization hierarchy.
  /// </summary>
  /// <remarks>If the organization is set to use parent info the values
  /// of the internal properties will return the values of the parent
  /// organization properties. Otherwise the properties will return their
  /// own values. If a property value is changed the Organization will be changed
  /// to not use the parent info and the values will be returned from the actual Oranization.</remarks>
  /// <example>
  /// The following code demonstrates how to display the entire organization
  /// hierarchy.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class UserManager
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server. We will need to be logged
  ///       // in as an Administrator to modify the user accounts.
  ///       Session session = new Session();
  ///       session.Start("myserver", "admin", "adminpwd");
  /// 
  ///       // Fetch the root organization
  ///       Organization root = session.Organizations.GetTopMostOrganization();
  /// 
  ///       // Recursively display this organization and its children
  ///       displayOrgHierarchy(root, 0);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// 
  ///    // Used to recursively generate the organization hierarchy
  ///    private static void displayOrgHierarchy(Organization parentOrg, int depth)
  ///    {
  ///       // Write out the parent organization's name
  ///       Console.WriteLine(new string('-', depth * 2) + parentOrg.Name);
  /// 
  ///       // Iterate over the organization's children, recursively calling this function
  ///       foreach (Organization suborg in parentOrg.GetSuborganizations())
  ///          displayOrgHierarchy(suborg, depth + 1);
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class Organization : SessionBoundObject, IOrganization
  {
    private IOrganizationManager mngr;
    private OrgInfo info;
    private OrgInfo topParent;
    private bool useParentInfo;
    private bool fetchedTopParent;
    private ScopedEventHandler<PersistentObjectEventArgs> committed;

    /// <summary>Event indicating that the object has been committed to the server.</summary>
    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    internal Organization(Session session, OrgInfo info)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Organization), "Committed");
      this.mngr = (IOrganizationManager) session.GetObject("OrganizationManager");
      this.info = info;
    }

    /// <summary>
    ///             Indicates if the organization is using parent info.
    ///             <remarks>If this value is set to true the organization will inherit its info from the
    ///             parent organization. If this value is set to false it will copy the current
    ///             values from the previous parent and use those values. If the organization has this value
    ///             set to true and a property is changed, this value will automatically be set to false and all
    ///             other property values will be copied from the parent.
    ///             </remarks>
    ///             <example>
    ///     The following code demonstrates how to the UseParentInfo property works.
    ///     <code>
    ///       <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Get at organization that is using parent info (UseParentInfo is true)
    ///       Organization org = session.Organizations.GetOrganization(7);
    /// 
    ///       // Output starting values
    ///       Console.WriteLine("Before Update");
    ///       Console.WriteLine(org.UseParentInfo);
    ///       Console.WriteLine(org.OrgCode);
    ///       Console.WriteLine(org.CompanyName);
    /// 
    ///       // Change organization to not use parent in
    ///       org.CompanyName = "Different Company"
    /// 
    ///       // Output values after change
    ///       Console.WriteLine("After Update");
    ///       Console.WriteLine(org.UseParentInfo);
    ///       Console.WriteLine(org.OrgCode);
    ///       Console.WriteLine(org.CompanyName);
    /// 
    ///       // Save the Organization
    ///       org.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///     </code>
    ///   </example>
    ///             </summary>
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

    /// <summary>Gets the unqiue identifier for this organization.</summary>
    public int ID => this.info.Oid;

    /// <summary>
    /// Gets or sets the name of the organization as it appears in the organization hierarchy.
    /// </summary>
    /// <remarks>This value differs from the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.Organization.CompanyName" /> property, which
    /// represents the company's name as it should appear on printed forms.</remarks>
    public string Name
    {
      get => this.info.OrgName;
      set
      {
        this.info.OrgName = !((value ?? "") == "") ? value : throw new ArgumentException("Invalid organization name");
      }
    }

    /// <summary>Gets or sets the OrgCode for this organization.</summary>
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

    /// <summary>Gets the address of record for the organization.</summary>
    public EllieMae.Encompass.BusinessObjects.Contacts.Address CompanyAddress
    {
      get
      {
        if (!this.fetchedTopParent)
          this.GetTopParent();
        return new EllieMae.Encompass.BusinessObjects.Contacts.Address(this.info.CompanyAddress, this.topParent.CompanyAddress, this);
      }
    }

    /// <summary>Gets or sets the name of the company.</summary>
    /// <remarks>This value differs from the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.Organization.Name" /> property in that it represents
    /// the company name which should be shown on forms when printed by users within this
    /// organization.</remarks>
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

    /// <summary>Gets or set the phone number for this organization.</summary>
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

    /// <summary>Gets or sets the fax number for this organization.</summary>
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

    /// <summary>Gets or sets the 1st DBA name for this organization.</summary>
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

    /// <summary>Gets or sets the 2nd DBA name for this organization.</summary>
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

    /// <summary>Gets or sets the 3rd DBA name for this organization.</summary>
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

    /// <summary>Gets or sets the 4th DBA name for this organization.</summary>
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

    /// <summary>Gets or sets the description of the organization.</summary>
    public string Description
    {
      get => this.info.Description;
      set => this.info.Description = value ?? "";
    }

    /// <summary>
    /// Commits the changes to the organization to the Encompass Server.
    /// </summary>
    /// <example>
    /// The following code demonstrates how to display the entire organization
    /// hierarchy.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the root organization
    ///       Organization root = session.Organizations.GetTopMostOrganization();
    /// 
    ///       // Modify the organization's name and org code
    ///       root.Name = "MyCompany Headquarters";
    ///       root.OrgCode = "0001";
    /// 
    ///       // Save the changes
    ///       root.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.ID.ToString()));
    }

    /// <summary>Deletes the organization from the Encompass Server.</summary>
    /// <remarks>The organization must contain no users and no suborganizations
    /// in order to be deleted.</remarks>
    /// <example>
    /// The following code deletes all leaf nodes on the organization hierarchy
    /// tree which all contain no users.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch a list of all the organizations in the hierarchy
    ///       OrganizationList orgs = session.Organizations.GetAllOrganizations();
    /// 
    ///       foreach (Organization org in orgs)
    ///       {
    ///          // If the organization is empty (no users or suborganizations),
    ///          // delete it from the tree
    ///          if ((org.GetUsers().Count == 0) && (org.GetSuborganizations().Count == 0))
    ///             org.Delete();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Delete()
    {
      this.ensureAdmin();
      this.ensureExists();
      if (this.IsTopMostOrganization)
        throw new InvalidOperationException("The root organization cannot be deleted");
      this.mngr.DeleteOrganization(this.info.Oid);
      this.info = (OrgInfo) null;
    }

    /// <summary>
    /// Refreshes the data for the current organization, causing any changes to be discarded.
    /// </summary>
    public void Refresh()
    {
      this.ensureExists();
      this.info = this.mngr.GetOrganization(this.info.Oid);
    }

    /// <summary>
    /// Indicates if this object represents a new, unsaved organization.
    /// </summary>
    /// <remarks>A new organization is not saved to the server unless the <see cref="M:EllieMae.Encompass.BusinessObjects.Users.Organization.Commit" />
    /// method is invoked.</remarks>
    public bool IsNew => this.info.Oid < 0;

    /// <summary>
    /// Indicates if the current organization is the root organization in the hierarchy.
    /// </summary>
    /// <example>
    /// The following code moves every user up one level in the organization hierarchy
    /// except those users which are already at the top-most level.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch a list of all the organizations in the hierarchy
    ///       UserList users = session.Users.GetAllUsers();
    /// 
    ///       foreach (User user in users)
    ///       {
    ///          // Get the organization to which the user already belongs
    ///          Organization currentOrg = session.Organizations.GetOrganization(user.OrganizationID);
    /// 
    ///          // Check if the user is already at the top level of the tree
    ///          if (!currentOrg.IsTopMostOrganization)
    ///          {
    ///             // Fetch the parent organization of the current org
    ///             Organization parentOrg = currentOrg.GetParentOrganization();
    /// 
    ///             // Move the user into the parent organization
    ///             user.ChangeOrganization(parentOrg);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool IsTopMostOrganization => this.info.Oid == this.info.Parent;

    /// <summary>
    /// Gets the top most parent that does not have its info inherited from a parent.
    /// </summary>
    private void GetTopParent(bool fromParent = false)
    {
      this.fetchedTopParent = true;
      this.topParent = this.mngr.GetFirstAvaliableOrganization(fromParent ? this.info.Parent : this.info.Oid, true);
      if (this.topParent.Oid == this.info.Oid)
        return;
      this.useParentInfo = true;
    }

    /// <summary>
    /// Retrieves the parent organization of the current organization.
    /// </summary>
    /// <returns>Returns the parent <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization" /> object unless
    /// the current object is the root organization, in which case it returns
    /// <c>null</c>.</returns>
    /// <example>
    /// The following code moves every user up one level in the organization hierarchy
    /// except those users which are already at the top-most level.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch a list of all the organizations in the hierarchy
    ///       UserList users = session.Users.GetAllUsers();
    /// 
    ///       foreach (User user in users)
    ///       {
    ///          // Get the organization to which the user already belongs
    ///          Organization currentOrg = session.Organizations.GetOrganization(user.OrganizationID);
    /// 
    ///          // Check if the user is already at the top level of the tree
    ///          if (!currentOrg.IsTopMostOrganization)
    ///          {
    ///             // Fetch the parent organization of the current org
    ///             Organization parentOrg = currentOrg.GetParentOrganization();
    /// 
    ///             // Move the user into the parent organization
    ///             user.ChangeOrganization(parentOrg);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Organization GetParentOrganization()
    {
      return this.IsTopMostOrganization ? (Organization) null : new Organization(this.Session, this.mngr.GetOrganization(this.info.Parent));
    }

    /// <summary>
    /// Returns the list of suborganizations of the current organization;
    /// </summary>
    /// <returns />
    /// <example>
    /// The following code demonstrates how to display the entire organization
    /// hierarchy.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the root organization
    ///       Organization root = session.Organizations.GetTopMostOrganization();
    /// 
    ///       // Recursively display this organization and its children
    ///       displayOrgHierarchy(root, 0);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Used to recursively generate the organization hierarchy
    ///    private static void displayOrgHierarchy(Organization parentOrg, int depth)
    ///    {
    ///       // Write out the parent organization's name
    ///       Console.WriteLine(new string('-', depth * 2) + parentOrg.Name);
    /// 
    ///       // Iterate over the organization's children, recursively calling this function
    ///       foreach (Organization suborg in parentOrg.GetSuborganizations())
    ///          displayOrgHierarchy(suborg, depth + 1);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public OrganizationList GetSuborganizations()
    {
      return this.info.Children == null ? new OrganizationList() : Organization.ToList(this.Session, this.mngr.GetOrganizations(this.info.Children));
    }

    /// <summary>
    /// Creates a new suborganization for the current organization.
    /// </summary>
    /// <param name="name">The name of the new suborganization.</param>
    /// <returns>A new Organization object for the suborganization. This object
    /// will not be saved to the Encompass Server until the <see cref="M:EllieMae.Encompass.BusinessObjects.Users.Organization.Commit" />
    /// method is invoked on it.</returns>
    /// <example>
    /// The following code creates a new suborganization of the top-most organization
    /// named "Loan Officers" and moves all users with the LO Persona into the organization.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the top-level organization
    ///       Organization rootOrg = session.Organizations.GetTopMostOrganization();
    /// 
    ///       // Create a new suborganization to hold the Loan Officers
    ///       // The organization must be committed before we add users to it.
    ///       Organization loOrg = rootOrg.CreateSuborganization("Loan Officers");
    ///       loOrg.Commit();
    /// 
    ///       // Get the "Loan Officer" Persona
    ///       Persona lop = session.Users.Personas.GetPersonaByName("Loan Officer");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList los = session.Users.GetUsersWithPersona(lop, false);
    /// 
    ///       // For each LO, move the user into the new organization
    ///       foreach (User lo in los)
    ///          lo.ChangeOrganization(loOrg);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Organization CreateSuborganization(string name)
    {
      this.ensureExists();
      OrgInfo info = new OrgInfo(-1, name, "", this.info.Oid, new int[0]);
      OrgInfo orgInfo = info;
      BranchExtLicensing branchExtLicensing = new BranchExtLicensing();
      branchExtLicensing.UseParentInfo = true;
      orgInfo.OrgBranchLicensing = branchExtLicensing;
      return new Organization(this.Session, info);
    }

    /// <summary>
    /// Returns the list of users in the current organization.
    /// </summary>
    /// <returns>The list of users in this organization.</returns>
    /// <remarks>This method is not recursive; that is, the set of users returned are only
    /// those in the current organization, not in its suborganizations.</remarks>
    /// <example>
    /// The following code disables all of the users that reside under the
    /// "Loan Officers" organization(s).
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the "Loan Officers" organization(s)
    ///       OrganizationList orgs = session.Organizations.GetOrganizationsByName("Loan Officers");
    /// 
    ///       foreach (Organization org in orgs)
    ///       {
    ///          // Fetch all of the users from the organization
    ///          UserList users = org.GetUsers();
    /// 
    ///          // For each LO, move the user into the new organization
    ///          foreach (User user in users)
    ///          {
    ///             if (user.Enabled)
    ///                user.Disable();
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public UserList GetUsers()
    {
      this.ensureExists();
      return User.ToList(this.Session, this.mngr.GetUsersInOrganization(this.info.Oid));
    }

    /// <summary>Creates a new user in the current organization.</summary>
    /// <param name="userId">The Login ID of the user to create. This value must be unique
    /// from all other Encompass users.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="personas">A <see cref="T:EllieMae.Encompass.Collections.PersonaList" /> containing one or more <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona">Personas</see>
    /// which should be assigned to the new user.</param>
    /// <returns>A new <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> within the current organization. This user is
    /// not persisted on the Encompass Server until the <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.Commit" /> method
    /// is invoked on it.</returns>
    /// <example>
    /// The following code creates a new user account within the "Loan Officers"
    /// organization.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the organizations with the name "Loan Officers"
    ///       OrganizationList orgs = session.Organizations.GetOrganizationsByName("Loan Officers");
    /// 
    ///       // Get the "Loan Officer" Persona
    ///       Persona lo = session.Users.Personas.GetPersonaByName("Loan Officer");
    ///       PersonaList personas = new PersonaList();
    ///       personas.Add(lo);
    /// 
    ///       // We'll assume there's just one matching org, so we'll use that one
    ///       // to create out user in.
    ///       Organization loOrg = orgs[0];
    ///       User newLo = loOrg.CreateUser("james", "jamespwd", personas);
    /// 
    ///       // Set some properties on the new users
    ///       newLo.FirstName = "James";
    ///       newLo.LastName = "Smith";
    ///       newLo.Email = "james@mycompany.com";
    ///       newLo.Phone = "333-333-3333";
    ///       newLo.WorkingFolder = "My Pipeline";
    ///       newLo.SubordinateLoanAccessRight = SubordinateLoanAccessRight.ReadOnly;
    /// 
    ///       // Assign some rights to the new LO
    ///       newLo.GrantAccessTo(Feature.PersonalLoanTemplateSets);
    ///       newLo.GrantAccessTo(Feature.PersonalePASS);
    /// 
    ///       // Save the new user
    ///       newLo.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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

    /// <summary>
    /// Returns the list of User Groups to which the organization belongs.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.UserGroupList" /> containing the set of user groups to which
    /// the organization's memers are assigned, either through direct assignment or by virtue of its position
    /// within the organization hierarchy.</returns>
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

    /// <summary>Provides a string representation for the object.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.Organization.Name" /> of the organization.</returns>
    public override string ToString() => this.info.OrgName;

    /// <summary>Provides a hash code for the organization object</summary>
    /// <returns>A hash code for the object.</returns>
    public override int GetHashCode() => this.ID;

    /// <summary>
    /// Indicates if two Organization objects represent the same persistent organization.
    /// </summary>
    /// <param name="obj">The Organization object to which to compare the current object.</param>
    /// <returns>Returns <c>true</c> if the organizations have the same ID and come from the same
    /// <see cref="T:EllieMae.Encompass.Client.Session">Session</see>, <c>false</c> otherwise.</returns>
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

    /// <summary>
    /// Gets or sets a flag indicating if the SSO Settings are inherited from the parent
    /// </summary>
    public bool SSO_UseParentInfo
    {
      get => this.info.SSOSettings.UseParentInfo;
      set => this.info.SSOSettings.UseParentInfo = value;
    }

    /// <summary>
    /// Gets or sets a flag indicating if the Login Access is Restricted or Full
    /// True if Restricted and False if Full
    /// </summary>
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
