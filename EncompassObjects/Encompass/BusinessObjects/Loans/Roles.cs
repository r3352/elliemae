// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Roles
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Provides access to the collection of defined Roles.</summary>
  /// <example>
  ///       The following code retrieves the Loan Officer role using its abbreviation,
  ///       LO, and assigns a user to that role within the loan.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class LoanManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
  ///       loan.Lock();
  /// 
  ///       // Retrieve the Loan Officer role (LO)
  ///       Role role = session.Loans.Roles.GetRoleByAbbrev("LO");
  /// 
  ///       // Retrieve the desired user for the role
  ///       User user = session.Users.GetUser("jsmith");
  /// 
  ///       // Assign the user to the specified role
  ///       loan.Associates.AssignUser(role, user);
  /// 
  ///       // Save and close the loan file
  ///       loan.Commit();
  ///       loan.Close();
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class Roles : SessionBoundObject, IRoles, IEnumerable
  {
    private ArrayList roles;
    private Role others;
    private Role fileStarter;
    private RolesMappingInfo[] roleMaps;

    internal Roles(Session session)
      : base(session)
    {
      this.fileStarter = new Role(session, RoleInfo.FileStarter);
      this.others = new Role(session, RoleInfo.Others);
      this.Refresh();
    }

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> objects in the collection.
    /// </summary>
    public int Count => this.roles.Count;

    /// <summary>
    /// Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> from the collection by index.
    /// </summary>
    public Role this[int index] => (Role) this.roles[index];

    /// <summary>Returns a role based on its role ID.</summary>
    /// <param name="roleId">The ID of the role to be returned.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> object with the given ID, or <c>null</c>
    /// if no role is found.</returns>
    public Role GetRoleByID(int roleId)
    {
      foreach (Role role in this.roles)
      {
        if (role.ID == roleId)
          return role;
      }
      return this.others.ID == roleId ? this.others : (Role) null;
    }

    /// <summary>Returns a role based on its 2-letter abbreviation.</summary>
    /// <param name="abbrev">The 2-letter abbreviation for the role.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> object with the abbreviation, or <c>null</c>
    /// if no role is found.</returns>
    /// <example>
    ///       The following code retrieves the Loan Officer role using its abbreviation,
    ///       LO, and assigns a user to that role within the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Retrieve the Loan Officer role (LO)
    ///       Role role = session.Loans.Roles.GetRoleByAbbrev("LO");
    /// 
    ///       // Retrieve the desired user for the role
    ///       User user = session.Users.GetUser("jsmith");
    /// 
    ///       // Assign the user to the specified role
    ///       loan.Associates.AssignUser(role, user);
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public Role GetRoleByAbbrev(string abbrev)
    {
      foreach (Role role in this.roles)
      {
        if (string.Compare(role.Abbreviation, abbrev, true) == 0)
          return role;
      }
      return string.Compare(this.others.Abbreviation, abbrev, true) == 0 ? this.others : (Role) null;
    }

    /// <summary>Returns a role based on its name.</summary>
    /// <param name="name">The name of the role.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> object with the given name, or <c>null</c>
    /// if no role is found.</returns>
    public Role GetRoleByName(string name)
    {
      foreach (Role role in this.roles)
      {
        if (string.Compare(role.Name, name, true) == 0)
          return role;
      }
      return string.Compare(this.others.Name, name, true) == 0 ? this.others : (Role) null;
    }

    /// <summary>Retrieves one of the "fixed" roles from Encompass.</summary>
    /// <param name="roleType">The type of the fixed role to retrieve.</param>
    /// <returns>The mapped <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> for the role type.</returns>
    /// <remarks>In the Banker Edition, this method may return <c>null</c> if no role is mapped to the specified
    /// fixed role type. In the Broker Edition, this method will always return a non-null value.</remarks>
    /// <example>
    ///       The following code removes the Loan Officer assignment from the loan,
    ///       clearing out any existing user or user group assigned to that role.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Find the Role object that is mapped to the fixed Loan Officer role, if any
    ///       Role role = session.Loans.Roles.GetFixedRole(FixedRole.LoanOfficer);
    /// 
    ///       if (role != null)
    ///       {
    ///         // Fetch the users in the specified role
    ///         LoanAssociateList los = loan.Associates.GetAssociatesByRole(role);
    /// 
    ///         // Clear the assigned user or user group from the role
    ///         foreach (LoanAssociate lo in los)
    ///           lo.Unassign();
    ///       }
    /// 
    ///       // Save and close the loan file
    ///       loan.Commit();
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public Role GetFixedRole(FixedRole roleType)
    {
      foreach (RolesMappingInfo roleMap in this.roleMaps)
      {
        if (roleMap.RealWorldRoleID == (RealWorldRoleID) roleType && roleMap.RoleIDList != null && roleMap.RoleIDList.Length != 0)
          return this.GetRoleByID(roleMap.RoleIDList[0]);
      }
      return (Role) null;
    }

    /// <summary>
    /// Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> object which represents the "Others" role when detemining
    /// access rights to Document Tracking items within a loan.
    /// </summary>
    public Role Others => this.others;

    /// <summary>Returns the FileStarter Role.</summary>
    /// <remarks>The File Starter (FS) role is the role automatically assigned to the user
    /// who creates the loan.</remarks>
    public Role FileStarter => this.fileStarter;

    /// <summary>Refreshes the role list if it has become out-of-date.</summary>
    public void Refresh()
    {
      this.roles = new ArrayList();
      this.roles.Add((object) this.fileStarter);
      foreach (RoleInfo allRoleFunction in this.WorkflowManager.GetAllRoleFunctions())
        this.roles.Add((object) new Role(this.Session, allRoleFunction));
      this.roleMaps = this.Session.SessionObjects.BpmManager.GetAllRoleMappingInfos();
    }

    /// <summary>Provides an enumerator for the collection of Roles.</summary>
    /// <returns>A enumerator for iterating over the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> objects
    /// in the collection.</returns>
    public IEnumerator GetEnumerator() => this.roles.GetEnumerator();

    private IBpmManager WorkflowManager => (IBpmManager) this.Session.GetObject("BpmManager");

    internal Role Find(RoleInfo roleInfo)
    {
      foreach (Role role in this.roles)
      {
        if (role.ID == roleInfo.RoleID)
          return role;
      }
      return (Role) null;
    }
  }
}
