// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.User
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Represents a single Encompass user.</summary>
  /// <example>
  /// The following code retrieves a user from the Encompass Server, modifies
  /// its name and email address and saves it back to the server.
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
  ///       // Open the user "tony"
  ///       User user = session.Users.GetUser("tony");
  /// 
  ///       // Change the user's name and email address
  ///       user.FirstName = "Tony";
  ///       user.LastName = "Johnson";
  ///       user.Email = "tony@mycompany.com";
  ///       user.JobTitle = "loanOfficer";
  /// 
  ///       // Save the user back to the server
  ///       user.Commit();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class User : SessionBoundObject, IUser
  {
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    private IOrganizationManager mngr;
    private IServicesAclManager servicesAclManager;
    internal UserInfo info;
    private bool isNew;
    private StateLicenses licenses;
    private UserPersonas personas;
    private string currentPassword = string.Empty;
    private const string LOGINURL = "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp�";
    private const string DEFAULTURL = "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp�";
    private const string SECONDARYLOGINURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp�";
    private const string SECONDARYDEFAULTURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp�";

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

    [DllImport("wininet.dll")]
    private static extern bool InternetSetCookieEx(
      string url,
      string name,
      string data,
      int flags,
      string p3p);

    internal User(Session session, UserInfo info, bool isNew)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (User), "Committed");
      this.info = info;
      this.mngr = (IOrganizationManager) session.GetObject("OrganizationManager");
      this.isNew = isNew;
      this.currentPassword = this.Session.SessionObjects.UserPassword;
    }

    /// <summary>Gets the user's Encompass Login ID.</summary>
    public string ID => this.info.Userid;

    /// <summary>Gets or sets the user's login password.</summary>
    /// <remarks>Use this property to modify a user's password. Only the user corresponding to this object or an Administrator can set
    /// this property.
    /// <p>When an existing User is retrieved from the Encompass Server, the Password
    /// property will return an empty string even if the user's password is non-empty.
    /// This property will only return a non-empty value if a password is set on it during
    /// the object's lifetime.</p>
    /// </remarks>
    public string Password
    {
      get => this.info.Password ?? "";
      set => this.info.Password = value ?? "";
    }

    /// <summary>Gets or sets the user's first name.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    /// <example>
    /// The following code retrieves a user from the Encompass Server, modifies
    /// its name and email address and saves it back to the server.
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
    ///       // Open the user "tony"
    ///       User user = session.Users.GetUser("tony");
    /// 
    ///       // Change the user's name and email address
    ///       user.FirstName = "Tony";
    ///       user.LastName = "Johnson";
    ///       user.Email = "tony@mycompany.com";
    ///       user.JobTitle = "loanOfficer";
    /// 
    ///       // Save the user back to the server
    ///       user.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string FirstName
    {
      get => this.info.FirstName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.FirstName = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's middle name.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    public string MiddleName
    {
      get => this.info.MiddleName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.MiddleName = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's suffix name.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    public string Suffix
    {
      get => this.info.SuffixName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.SuffixName = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's last name.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    /// <example>
    /// The following code retrieves a user from the Encompass Server, modifies
    /// its name and email address and saves it back to the server.
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
    ///       // Open the user "tony"
    ///       User user = session.Users.GetUser("tony");
    /// 
    ///       // Change the user's name and email address
    ///       user.FirstName = "Tony";
    ///       user.LastName = "Johnson";
    ///       user.Email = "tony@mycompany.com";
    ///       user.JobTitle = "loanOfficer";
    /// 
    ///       // Save the user back to the server
    ///       user.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string LastName
    {
      get => this.info.LastName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.LastName = value ?? "";
      }
    }

    /// <summary>Gets the full name of the user.</summary>
    public string FullName => this.info.FullName;

    /// <summary>Gets or sets the user's employee id.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    /// <example>
    /// The following code retrieves a user from the Encompass Server, modifies
    /// its name and email address and saves it back to the server.
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
    ///       // Open the user "tony"
    ///       User user = session.Users.GetUser("tony");
    /// 
    ///       // Change the user's name and email address
    ///       user.FirstName = "Tony";
    ///       user.LastName = "Johnson";
    ///       user.Email = "tony@mycompany.com";
    ///       user.JobTitle = "loanOfficer";
    /// 
    ///       // Save the user back to the server
    ///       user.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string EmployeeID
    {
      get => this.info.EmployeeID;
      set
      {
        this.EnsureAdminOrUser();
        this.info.EmployeeID = value ?? "";
      }
    }

    /// <summary>Gets the persona(s) assigned to the current user.</summary>
    /// <example>
    /// The following code retrieves all Loan Officers from the Encompass Server and
    /// displays them.
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
    ///       // Get the "Loan Officer" Persona
    ///       Persona lop = session.Users.Personas.GetPersonaByName("Loan Officer");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList los = session.Users.GetUsersWithPersona(lop, false);
    /// 
    ///       // List all users with Loan Officer persona
    ///       foreach (User lo in los)
    ///       {
    ///          Console.WriteLine(string.Format("{0} {1} ({2})", lo.FirstName, lo.LastName, lo.ID));
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public UserPersonas Personas
    {
      get
      {
        if (this.personas == null)
          this.personas = new UserPersonas(this, this.info.UserPersonas);
        return this.personas;
      }
    }

    /// <summary>Gets or sets the user's job title</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    /// <example>
    /// The following code retrieves a user from the Encompass Server, modifies
    /// its name and email address and saves it back to the server.
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
    ///       // Open the user "tony"
    ///       User user = session.Users.GetUser("tony");
    /// 
    ///       // Change the user's name and email address
    ///       user.FirstName = "Tony";
    ///       user.LastName = "Johnson";
    ///       user.Email = "tony@mycompany.com";
    ///       user.JobTitle = "loanOfficer";
    /// 
    ///       // Save the user back to the server
    ///       user.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string JobTitle
    {
      get => this.info.JobTitle;
      set
      {
        this.EnsureAdminOrUser();
        this.info.JobTitle = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's email address.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    /// <example>
    /// The following code retrieves a user from the Encompass Server, modifies
    /// its name and email address and saves it back to the server.
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
    ///       // Open the user "tony"
    ///       User user = session.Users.GetUser("tony");
    /// 
    ///       // Change the user's name and email address
    ///       user.FirstName = "Tony";
    ///       user.LastName = "Johnson";
    ///       user.Email = "tony@mycompany.com";
    ///       user.JobTitle = "loanOfficer";
    /// 
    ///       // Save the user back to the server
    ///       user.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string Email
    {
      get => this.info.Email;
      set
      {
        this.EnsureAdminOrUser();
        this.info.Email = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's phone number.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    public string Phone
    {
      get => this.info.Phone;
      set
      {
        this.EnsureAdminOrUser();
        this.info.Phone = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's cell phone number.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    public string CellPhone
    {
      get => this.info.CellPhone;
      set
      {
        this.EnsureAdminOrUser();
        this.info.CellPhone = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's fax number.</summary>
    /// <remarks>Only the user corresponding to this object or an Administrator can set
    /// this property.</remarks>
    public string Fax
    {
      get => this.info.Fax;
      set
      {
        this.EnsureAdminOrUser();
        this.info.Fax = value ?? "";
      }
    }

    /// <summary>Gets or sets the user's working folder.</summary>
    /// <remarks>The currently logged in user must have Administrator privileges to set this
    /// property.</remarks>
    public string WorkingFolder
    {
      get => this.info.WorkingFolder;
      set
      {
        this.EnsureAdmin();
        this.info.WorkingFolder = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets if the user must change their password at the next login.
    /// </summary>
    public bool RequirePasswordChange
    {
      get => this.info.RequirePasswordChange;
      set
      {
        this.EnsureAdmin();
        this.info.RequirePasswordChange = value;
      }
    }

    /// <summary>
    /// Gets the latest password change date to proactively inform my users about their expiring password
    /// </summary>
    public DateTime PasswordChangedDate => this.info.PasswordChangedDate;

    /// <summary>
    /// Gets or sets whether the user is prohibited for logging into their account.
    /// </summary>
    public bool AccountLocked
    {
      get => this.info.Locked;
      set
      {
        this.EnsureAdmin();
        this.info.Locked = value;
      }
    }

    /// <summary>Gets or sets the CHUM ID for the user</summary>
    public string CHUMID
    {
      get => this.info.CHUMId;
      set
      {
        this.EnsureAdmin();
        this.info.CHUMId = value ?? "";
      }
    }

    /// <summary>Gets or sets the NMLS Loan Originator ID for the user</summary>
    public string NMLSOriginatorID
    {
      get => this.info.NMLSOriginatorID;
      set
      {
        this.EnsureAdmin();
        this.info.NMLSOriginatorID = value ?? "";
      }
    }

    /// <summary>Gets or sets the NMLS Loan Originator ID for the user</summary>
    public DateTime NMLSExpirationDate
    {
      get => this.info.NMLSExpirationDate;
      set
      {
        this.EnsureAdmin();
        this.info.NMLSExpirationDate = value;
      }
    }

    /// <summary>
    /// Gets or sets the default access rights that the user has when accessing the
    /// loans of his subordinates.
    /// </summary>
    /// <remarks>The currently logged in user must have Administrator privileges to set this
    /// property.</remarks>
    public SubordinateLoanAccessRight SubordinateLoanAccessRight
    {
      get => (SubordinateLoanAccessRight) this.info.AccessMode;
      set
      {
        this.EnsureAdmin();
        this.info.AccessMode = (UserInfo.AccessModeEnum) value;
      }
    }

    /// <summary>
    /// Gets or sets the default access rights that the user has when accessing the
    /// loans of his subordinates.
    /// </summary>
    /// <remarks>The currently logged in user must have Administrator privileges to set this
    /// property.</remarks>
    public PeerLoanAccessRight PeerLoanAccessRight
    {
      get => (PeerLoanAccessRight) this.info.PeerView;
      set
      {
        this.EnsureAdmin();
        this.info.PeerView = (UserInfo.UserPeerView) value;
      }
    }

    /// <summary>
    /// Fetches the collection of state licensing information for the user.
    /// </summary>
    /// <example>
    /// The following code fetches all of the loans for which a loan officer has
    /// been assigned and, for each, verifies that the LO is licensed to originate loans in
    /// the state in which the subject property resides.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Query;
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
    ///       // Create a query to find all loans with a non-empty loan officer
    ///       StringFieldCriterion loCri = new StringFieldCriterion();
    ///       loCri.FieldName = "Loan.LoanOfficerID";
    ///       loCri.Value = "";
    ///       loCri.Include = false;
    /// 
    ///       // Fetch the loans that match the criteria
    ///       LoanIdentityList ids = session.Loans.Query(loCri);
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the loan identified by the id
    ///          Loan loan = session.Loans.Open(id.Guid);
    /// 
    ///          // Fetch the Loan Officer for this loan
    ///          User lo = session.Users.GetUser(loan.LoanOfficerID);
    /// 
    ///          // Get the subject property state for the loan, which is field 14
    ///          string propertyState = loan.Fields["14"].FormattedValue;
    /// 
    ///          if ((propertyState != "") && (lo != null))
    ///          {
    ///             // Check if the loan officer is licensed in that state
    ///             StateLicense license = lo.StateLicenses[propertyState];
    /// 
    ///             if ((license == null) || !license.Enabled)
    ///                Console.WriteLine("The loan '" + loan.Guid + "' is assigned to an unlicensed LO");
    ///          }
    /// 
    ///          // Close the loan
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public StateLicenses StateLicenses
    {
      get
      {
        if (this.licenses == null)
          this.licenses = new StateLicenses(this);
        return this.licenses;
      }
    }

    /// <summary>
    /// Gets the ID of the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization" /> to which the user belongs.
    /// </summary>
    /// <example>
    /// The following code prints out the names of the organizations which contain
    /// one or more Loan Officer.
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
    ///       // Get the "Loan Officer" Persona
    ///       Persona lop = session.Users.Personas.GetPersonaByName("Loan Officer");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList los = session.Users.GetUsersWithPersona(lop, false);
    /// 
    ///       // For each LO, fetch the organization in which the user belongs
    ///       // and print is name
    ///       foreach (User lo in los)
    ///       {
    ///          // Retrieve the organization
    ///          Organization org = session.Organizations.GetOrganization(lo.OrganizationID);
    /// 
    ///          // Write the name to the console
    ///          Console.WriteLine(org.Name);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int OrganizationID => this.info.OrgId;

    /// <summary>Gets the failed login attempts for AE User.</summary>
    /// <include file="User.xml" path="Examples/Example[@name=&quot;User.FailedLoginAttempts&quot;]/*" />
    public int FailedLoginAttempts => this.info.failed_login_attempts;

    /// <summary>
    /// Moves the user into a new <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization" />.
    /// </summary>
    /// <param name="newOrganization">The organization into which the user
    /// should be moved.</param>
    /// <remarks>This method may only be called by users with the Administrator
    /// persona. Additionally, it may only be called on Users which have already been
    /// committed to the Encompass Server.
    /// <p>The change of organization occurrs immediately upon calling this method. It
    /// is not necessary to invoke <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.Commit" /> to make this change permanent.</p>
    /// </remarks>
    /// <example>
    /// The following code moves all of the Loan Officers into a new Organization
    /// called "Loan Officers" which is placed directly under the root organization.
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
    public void ChangeOrganization(Organization newOrganization)
    {
      this.ensureExists();
      this.EnsureAdmin();
      if (newOrganization.IsNew)
        throw new InvalidOperationException("The target organization must be committed before users can be added");
      this.mngr.MoveUserIntoOrganization(this.info.Userid, newOrganization.ID);
      string password = this.info.Password;
      this.info = new UserInfo(this.info, newOrganization.ID, newOrganization.IsTopMostOrganization);
      this.info.Password = password;
    }

    /// <summary>
    /// Gets a flag indicating id the user's account is enabled.
    /// </summary>
    /// <remarks>To enable or disable a user's account use the <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.Enable" />
    /// and <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.Disable" /> methods, respectively.</remarks>
    public bool Enabled => this.info.Status == UserInfo.UserStatusEnum.Enabled;

    /// <summary>Enables the current user's account</summary>
    /// <remarks>This method can only be invoked by an Administrator. Additionally,
    /// a free license must be available for this user or an exception will be raised.
    /// <p>If invoked on an existing user (one for which <see cref="P:EllieMae.Encompass.BusinessObjects.Users.User.IsNew" /> return
    /// <c>false</c>, the change will occur immediately without a need to call the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.Commit" /> method.</p></remarks>
    /// <example>
    /// The following code enables all of the user accounts in the
    /// "Loan Officers" organization.
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
    ///             if (user.Enabled == false)
    ///                user.Enable();
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
    public void Enable()
    {
      this.EnsureAdmin();
      if (!this.isNew)
        this.mngr.EnableUser(this.info.Userid);
      this.info.Status = UserInfo.UserStatusEnum.Enabled;
    }

    /// <summary>Disables the current user's account.</summary>
    /// <remarks>This method can only be invoked by an Administrator.
    /// <p>If invoked on an existing user (one for which <see cref="P:EllieMae.Encompass.BusinessObjects.Users.User.IsNew" /> return
    /// <c>false</c>, the change will occur immediately without a need to call the
    /// <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.Commit" /> method.</p></remarks>
    /// <example>
    /// The following code disables all of the user accounts in the
    /// "Loan Officers" organization.
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
    public void Disable()
    {
      this.EnsureAdmin();
      if (this.info.Userid == "admin")
        throw new InvalidOperationException("The 'admin' cannot be disabled");
      if (!this.isNew)
        this.mngr.DisableUser(this.info.Userid);
      this.info.Status = UserInfo.UserStatusEnum.Disabled;
    }

    /// <summary>
    /// Determines if the user is authorized to access a given feature.
    /// </summary>
    /// <param name="feature">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Feature" /> to check permissions on</param>
    /// <returns>Returns <c>true</c> if the user has access, <c>false</c> otherwise.</returns>
    /// <remarks>A user can have access to a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Feature" /> either by virtue of their
    /// assigned <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona">Personas</see> or because of rights directly assigned to them.</remarks>
    public bool HasAccessTo(Feature feature)
    {
      this.ensureExists();
      return this.FeaturesAclManager.CheckPermission((AclFeature) feature, this.info);
    }

    /// <summary>
    /// Grants the user explicit access to a particular feature.
    /// </summary>
    /// <param name="feature">The feature to which the user should be granted access.</param>
    public void GrantAccessTo(Feature feature)
    {
      this.ensureExists();
      this.EnsureAdmin();
      this.FeaturesAclManager.SetPermission((AclFeature) feature, this.ID, AclTriState.True);
    }

    /// <summary>
    /// Revokes the user's explicitly assigned access to a particular feature.
    /// </summary>
    /// <param name="feature">The feature to which the user should be granted revoked.
    /// This method only revokes access rights that are explicitly assigned to this user. Access rights
    /// gained through membership in a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> are not affected by this method.</param>
    public void RevokeAccessTo(Feature feature)
    {
      this.ensureExists();
      this.EnsureAdmin();
      this.FeaturesAclManager.SetPermission((AclFeature) feature, this.ID, AclTriState.Unspecified);
    }

    /// <summary>
    /// Retrieves a custom <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> from the Encompass Server.
    /// </summary>
    /// <param name="fileName">The file name in which the object is stored.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> containing the data.</returns>
    /// <remarks>This method can be used to retrieve a custom data object which is stored
    /// on a per-user basis. Use the <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.SaveCustomDataObject(System.String,EllieMae.Encompass.BusinessObjects.DataObject)" /> to save a custom
    /// object to the Encompass Server.</remarks>
    public DataObject GetCustomDataObject(string fileName)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      BinaryObject customDataObject = ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetUserCustomDataObject(this.info.Userid, fileName);
      return customDataObject == null ? (DataObject) null : new DataObject(customDataObject);
    }

    /// <summary>
    /// Saves a custom <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> to the Encompass Server for the specified user.
    /// </summary>
    /// <param name="fileName">The file name to which the object will be stored.</param>
    /// <param name="dataObj">The custom data object to be stored on the Encompass Server.</param>
    /// <remarks>Use this method to store custom, user-specific data to the Encompass Server.
    /// Then use the <see cref="M:EllieMae.Encompass.BusinessObjects.Users.User.GetCustomDataObject(System.String)" /> method to later retrieve this object.</remarks>
    public void SaveCustomDataObject(string fileName, DataObject dataObj)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (dataObj == null)
        throw new ArgumentNullException(nameof (dataObj));
      ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).SaveUserCustomDataObject(this.info.Userid, fileName, dataObj.Unwrap());
    }

    /// <summary>
    /// Appends a <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> to a previously-created custom data object for the specified user.
    /// </summary>
    /// <param name="fileName">The file name of the custom data object.</param>
    /// <param name="dataObj">The custom data object to be appended.</param>
    /// <remarks>If the specified custom data object does not exist, one will be created
    /// and the specified content added to it.</remarks>
    public void AppendToCustomDataObject(string fileName, DataObject dataObj)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (dataObj == null)
        throw new ArgumentNullException(nameof (dataObj));
      ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).AppendToUserCustomDataObject(this.info.Userid, fileName, dataObj.Unwrap());
    }

    /// <summary>
    /// Deletes a <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject" /> custom data object for the specified user.
    /// </summary>
    /// <param name="fileName">The file name of the custom data object.</param>
    public void DeleteCustomDataObject(string fileName)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).SaveUserCustomDataObject(this.info.Userid, fileName, (BinaryObject) null);
    }

    /// <summary>Commits the changes to the current user.</summary>
    /// <example>
    /// The following code retrieves a user from the Encompass Server, modifies
    /// its name and email address and saves it back to the server.
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
    ///       // Open the user "tony"
    ///       User user = session.Users.GetUser("tony");
    /// 
    ///       // Change the user's name and email address
    ///       user.FirstName = "Tony";
    ///       user.LastName = "Johnson";
    ///       user.Email = "tony@mycompany.com";
    ///       user.JobTitle = "loanOfficer";
    /// 
    ///       // Save the user back to the server
    ///       user.Commit();
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
      if (this.Personas.Count == 0)
        throw new InvalidOperationException("Cannot commit a user without at least one assigned persona");
      this.info.UserPersonas = this.personas.Unwrap();
      bool isNew = this.isNew;
      if (this.isNew)
      {
        this.mngr.CreateNewUser(this.info);
        this.isNew = false;
      }
      else if (this.Session.GetUserInfo().IsAdministrator())
      {
        if (!this.info.SSOOnly && !this.info.PasswordExists && string.IsNullOrEmpty(this.info.Password))
          throw new InvalidOperationException("Password field is empty. Password is required for Full Access, please add password.");
        this.mngr.UpdateUser(this.info);
        CacheManager.GetSimpleCache("UserInfoCache").Remove(this.Session.SessionObjects.StartupInfo.ServerInstanceName + "@" + this.Session.SessionObjects.StartupInfo.ServerID + "_UserStore_" + Convert.ToString(this.info.Userid));
      }
      else
      {
        if (!(this.Session.UserID == this.info.Userid))
          throw new InvalidOperationException("Access denied");
        ICurrentUser user = this.Session.Unwrap().GetUser();
        user.UpdatePersonalInfo(this.info.FirstName, this.info.LastName, this.info.Email, this.info.Phone, this.info.CellPhone, this.info.Fax);
        if (!string.IsNullOrEmpty(this.info.Password))
        {
          user.ChangePassword(this.info.Password);
          this.currentPassword = this.info.Password;
        }
      }
      if (isNew)
      {
        bool isAdmin = !(this.Session.UserID == this.info.Userid) && this.Session.GetUserInfo().IsAdministrator();
        this.updateEMNPassword(isNew, isAdmin);
      }
      if (this.licenses != null)
        this.licenses.Commit();
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.ID));
    }

    /// <summary>Adds EMN user details</summary>
    /// <param name="url"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private string addEMNUser(string url, string password)
    {
      SessionObjects sessionObjects = this.Session.SessionObjects;
      CompanyInfo companyInfo = sessionObjects.CompanyInfo;
      OrgInfo branchInfo = sessionObjects.StartupInfo.BranchInfo;
      if (branchInfo != null)
        companyInfo = new CompanyInfo(companyInfo.ClientID, branchInfo.CompanyName, branchInfo.CompanyAddress.Street1, branchInfo.CompanyAddress.City, branchInfo.CompanyAddress.State, branchInfo.CompanyAddress.Zip, branchInfo.CompanyPhone, branchInfo.CompanyFax, companyInfo.Password, new string[4]
        {
          branchInfo.DBAName1,
          branchInfo.DBAName2,
          branchInfo.DBAName3,
          branchInfo.DBAName4
        }, (BranchExtLicensing) branchInfo.OrgBranchLicensing.Clone());
      string str1 = string.Empty;
      switch (sessionObjects.StartupInfo.RuntimeEnvironment)
      {
        case EllieMae.EMLite.Common.RuntimeEnvironment.Default:
          str1 = "Default";
          break;
        case EllieMae.EMLite.Common.RuntimeEnvironment.Hosted:
          str1 = "Hosted";
          break;
      }
      if (this.servicesAclManager == null)
        this.servicesAclManager = (IServicesAclManager) sessionObjects.Session.GetAclManager(AclCategory.Services);
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = this.servicesAclManager.GetServicesDefaultSetting(AclFeature.LoanTab_Other_ePASS, sessionObjects.UserID, sessionObjects.UserInfo.UserPersonas);
      string str2 = string.Empty;
      string str3 = (string) null;
      switch (servicesDefaultSetting)
      {
        case ServiceAclInfo.ServicesDefaultSetting.None:
          str3 = "n";
          break;
        case ServiceAclInfo.ServicesDefaultSetting.Custom:
          str3 = "c";
          str2 = this.getCompanyServices(sessionObjects);
          break;
        case ServiceAclInfo.ServicesDefaultSetting.All:
          str3 = "y";
          break;
      }
      EllieMae.EMLite.Common.Licensing.EncompassEdition edition = EllieMae.EMLite.Common.Licensing.EncompassEdition.None;
      switch (this.Session.EncompassEdition)
      {
        case EllieMae.Encompass.Client.EncompassEdition.Broker:
          edition = EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker;
          break;
        case EllieMae.Encompass.Client.EncompassEdition.Banker:
          edition = EllieMae.EMLite.Common.Licensing.EncompassEdition.Banker;
          break;
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(companyInfo.ClientID));
      stringBuilder.Append("&clientname=" + HttpUtility.UrlEncode(companyInfo.Name));
      stringBuilder.Append("&clientaddress=" + HttpUtility.UrlEncode(companyInfo.Address));
      stringBuilder.Append("&clientcity=" + HttpUtility.UrlEncode(companyInfo.City));
      stringBuilder.Append("&clientstate=" + HttpUtility.UrlEncode(companyInfo.State));
      stringBuilder.Append("&clientzip=" + HttpUtility.UrlEncode(companyInfo.Zip));
      stringBuilder.Append("&clientphone=" + HttpUtility.UrlEncode(companyInfo.Phone));
      stringBuilder.Append("&clientfax=" + HttpUtility.UrlEncode(companyInfo.Fax));
      stringBuilder.Append("&clientpassword=" + HttpUtility.UrlEncode(companyInfo.Password));
      stringBuilder.Append("&userid=" + HttpUtility.UrlEncode(this.info.Userid));
      stringBuilder.Append("&userfirstname=" + HttpUtility.UrlEncode(this.info.FirstName));
      stringBuilder.Append("&userlastname=" + HttpUtility.UrlEncode(this.info.LastName));
      stringBuilder.Append("&userpersona=" + HttpUtility.UrlEncode(sessionObjects.GetEPassPersonaDescriptor()));
      stringBuilder.Append("&useremail=" + HttpUtility.UrlEncode(this.info.Email));
      stringBuilder.Append("&userphone=" + HttpUtility.UrlEncode(this.info.Phone));
      stringBuilder.Append("&userpassword=" + HttpUtility.UrlEncode(password));
      stringBuilder.Append("&personal=" + HttpUtility.UrlEncode(str3));
      stringBuilder.Append("&version=" + HttpUtility.UrlEncode(VersionInformation.CurrentVersion.GetExtendedVersion(edition)));
      stringBuilder.Append("&environment=" + HttpUtility.UrlEncode(str1));
      stringBuilder.Append("&companyservices=" + HttpUtility.UrlEncode(str2));
      stringBuilder.Append("&NMLSOriginatorID=" + HttpUtility.UrlEncode(this.info.NMLSOriginatorID));
      stringBuilder.Append("&NMLSExpirationDate=" + HttpUtility.UrlEncode(this.info.NMLSExpirationDate == DateTime.MaxValue ? "" : this.info.NMLSExpirationDate.ToString("MM/dd/yyyy")));
      DateTime dateTime;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("<LOLICENSES></LOLICENSES>");
        XmlElement documentElement = xmlDocument.DocumentElement;
        foreach (LOLicenseInfo loLicense in ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetLOLicenses(sessionObjects.UserInfo.Userid))
        {
          if (loLicense.Enabled && loLicense.License != string.Empty)
          {
            XmlElement element = xmlDocument.CreateElement("LIC");
            documentElement.AppendChild((XmlNode) element);
            element.SetAttribute("ST", loLicense.StateAbbr.Trim());
            element.SetAttribute("LN", loLicense.License.Trim());
            XmlElement xmlElement = element;
            string str4;
            if (!(loLicense.ExpirationDate == DateTime.MaxValue))
            {
              dateTime = loLicense.ExpirationDate;
              str4 = dateTime.ToString("MM/dd/yyyy");
            }
            else
              str4 = "";
            xmlElement.SetAttribute("ED", str4);
          }
        }
        stringBuilder.Append("&lolicensesxml=" + HttpUtility.UrlEncode(xmlDocument.OuterXml));
      }
      catch
      {
        return (string) null;
      }
      string header1;
      string header2;
      string end;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp");
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) stringBuilder.Length;
        httpWebRequest.Timeout = 5000;
        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        streamWriter.Write(stringBuilder.ToString());
        streamWriter.Close();
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        header1 = response.Headers["Set-Cookie"];
        header2 = response.Headers["P3P"];
        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        end = streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch
      {
        switch (url)
        {
          case "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp":
            return this.addEMNUser("https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp", password);
          case "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp":
            return this.addEMNUser("https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp", password);
          default:
            return (string) null;
        }
      }
      if (header1 == null)
        return (string) null;
      Uri uri = new Uri("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp");
      CookieContainer cookieContainer = new CookieContainer();
      cookieContainer.SetCookies(uri, header1);
      foreach (Cookie cookie in cookieContainer.GetCookies(uri))
      {
        string str5 = cookie.ToString();
        if (cookie.Expires != DateTime.MinValue)
        {
          string str6 = str5;
          dateTime = cookie.Expires;
          string str7 = dateTime.ToString("r");
          str5 = str6 + "; expires=" + str7;
        }
        string data = str5 + "; path=" + cookie.Path;
        User.InternetSetCookieEx(uri.AbsoluteUri, (string) null, data, 64, header2);
      }
      return end;
    }

    private string[] getServices(SessionObjects sessionObjects, bool hasPermission)
    {
      ServiceAclInfo[] permissions = this.servicesAclManager.GetPermissions(AclFeature.LoanTab_Other_ePASS, sessionObjects.UserID, sessionObjects.UserInfo.UserPersonas);
      List<string> stringList = new List<string>();
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        if (serviceAclInfo.Access == hasPermission)
          stringList.Add(serviceAclInfo.ServiceTitle);
      }
      return stringList.ToArray();
    }

    /// <summary>
    /// Returns xml with a list of company services that the user has rights to customize.
    /// </summary>
    private string getCompanyServices(SessionObjects sessionObjects)
    {
      string[] services = this.getServices(sessionObjects, false);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<SERVICES/>");
      foreach (string str in services)
      {
        XmlElement element = xmlDocument.CreateElement("CATEGORY");
        element.SetAttribute("Title", str);
        xmlDocument.DocumentElement.AppendChild((XmlNode) element);
      }
      return xmlDocument.OuterXml;
    }

    /// <summary>Updates EMN password</summary>
    /// <param name="isNew"></param>
    /// <param name="isAdmin"></param>
    private void updateEMNPassword(bool isNew, bool isAdmin)
    {
      if (!isNew)
        return;
      this.addEMNUser("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp", this.Password);
    }

    /// <summary>Deletes the current user.</summary>
    /// <remarks>This method can only be invoked by an Administrator.</remarks>
    /// <example>
    /// The following code deletes all of the Loan Processors to whom no loans
    /// are currently assigned.
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
    ///       // Get "Loan Processor" persona
    ///       Persona lop = session.Users.Personas.GetPersonaByName("Loan Processor");
    /// 
    ///       // Fetch the list of Loan Processors from the server
    ///       UserList users = session.Users.GetUsersWithPersona(lop, false);
    /// 
    ///       foreach (User lp in lps)
    ///       {
    ///          // Create the query criterion that specifies an exact match based on the LP's ID
    ///          StringFieldCriterion lpCri = new StringFieldCriterion();
    ///          lpCri.FieldName = "Loan.LoanProcessorID";
    ///          lpCri.Value = lp.ID;
    /// 
    ///          // Run the query to get the list of matching loans
    ///          LoanIdentityList assignedLoans = session.Loans.Query(lpCri);
    /// 
    ///          // If no loans are assigned, delete the user
    ///          if (assignedLoans.Count == 0)
    ///             lp.Delete();
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
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
      if (this.ID == this.Session.UserID)
        throw new InvalidOperationException("Cannot delete currently logged in user");
      if (this.info.Userid == "admin")
        throw new InvalidOperationException("The admin user account cannot be deleted or disabled");
      this.mngr.DeleteUser(this.info.Userid);
    }

    /// <summary>Refreshes the information for the current user.</summary>
    /// <remarks>This method will cause any changes made since the object was last
    /// committed to be lost. This includes any changes made to the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Users.User.StateLicenses" /> associated with the user.</remarks>
    /// <example>
    /// The following code demonstrates the use of the Refresh method by making
    /// several changes to a user and then discarding them.
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
    ///       // Retrieve the "officer" user from the server
    ///       User officer = session.Users.GetUser("officer");
    /// 
    ///       // Make some changes to the object
    ///       officer.FirstName = "New";
    ///       officer.LastName = "Name";
    /// 
    ///       // Now discard the changes by refreshing the object
    ///       officer.Refresh();
    /// 
    ///       // Display the name to verify the original value is in place
    ///       Console.WriteLine(officer.FirstName + " " + officer.LastName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Refresh()
    {
      if (this.IsNew)
        throw new InvalidOperationException("Method not valid for new objects");
      this.info = this.mngr.GetUser(this.info.Userid);
      if (this.licenses == null)
        return;
      this.licenses.Refresh();
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup">UserGroups</see> to which the user belongs.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.UserGroupList" /> containing the set of user groups to which
    /// the user has membership, either through direct assignment or by virtue of its position
    /// within the organization hierarchy.</returns>
    public UserGroupList GetUserGroups()
    {
      AclGroup[] groupsOfUser = this.Session.SessionObjects.AclGroupManager.GetGroupsOfUser(this.ID);
      UserGroupList userGroups = new UserGroupList();
      foreach (AclGroup aclGroup in groupsOfUser)
      {
        UserGroup groupById = this.Session.Users.Groups.GetGroupByID(aclGroup.ID);
        if (groupById != null)
          userGroups.Add(groupById);
      }
      return userGroups;
    }

    /// <summary>
    /// A flag indicating if the user is new and yet to be committed to the database.
    /// </summary>
    public bool IsNew => this.isNew;

    /// <summary>Returns a string representation of the user.</summary>
    /// <returns>The current user's first and last names, separated by a space.</returns>
    public override string ToString() => this.FullName;

    /// <summary>Provides a hash code value for the user.</summary>
    /// <returns>A hash code that can be used in Hashtable objects.</returns>
    public override int GetHashCode() => this.ID.GetHashCode();

    /// <summary>
    /// Indicates if two User objects represent the same persistent user.
    /// </summary>
    /// <param name="obj">The User object to which to compare the current object.</param>
    /// <returns>Returns <c>true</c> if the users have the same UserID and come from the same
    /// <see cref="T:EllieMae.Encompass.Client.Session">Session</see>, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return !object.Equals((object) (obj as User), (object) null) && ((SessionBoundObject) obj).Session == this.Session && ((User) obj).ID == this.ID;
    }

    internal void EnsureAdmin()
    {
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
    }

    internal void EnsureAdminOrUser()
    {
      if (!(this.Session.UserID != this.info.Userid))
        return;
      this.EnsureAdmin();
    }

    private void ensureExists()
    {
      if (this.IsNew)
        throw new InvalidOperationException("This operation is not valid until object is commited");
    }

    private IFeaturesAclManager FeaturesAclManager
    {
      get => (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features);
    }

    internal UserInfo Unwrap() => this.info;

    internal static UserList ToList(Session session, UserInfo[] infos)
    {
      UserList list = new UserList();
      for (int index = 0; index < infos.Length; ++index)
        list.Add(new User(session, infos[index], false));
      return list;
    }

    /// <summary>
    /// This method is for internal Encompass use only and should not be called from
    /// your code.
    /// </summary>
    /// <exclude />
    public static User Wrap(Session session, UserInfo userInfo)
    {
      return new User(session, userInfo, false);
    }

    /// <summary>
    /// Gets or Sets a flag indicating if the user has Restricted or Full Access
    /// True if Restricted and False if Full
    /// </summary>
    public bool SSOOnly
    {
      get => this.info.SSOOnly;
      set
      {
        this.EnsureAdmin();
        this.info.SSOOnly = value;
        OrgInfo organizationForSso = this.mngr.GetFirstOrganizationForSSO(this.info.OrgId);
        this.info.SSODisconnectedFromOrg = value != organizationForSso.SSOSettings.LoginAccess;
      }
    }

    /// <summary>
    /// Gets or Sets a flag indicating if the user is connected/disconnected to the parent SSO Settings
    /// True if Disconnected and False if Connected
    /// </summary>
    public bool SSODisconnectedFromOrg
    {
      get => this.info.SSODisconnectedFromOrg;
      set
      {
        this.EnsureAdmin();
        this.info.SSODisconnectedFromOrg = value;
        OrgInfo organizationForSso = this.mngr.GetFirstOrganizationForSSO(this.info.OrgId);
        if (value)
          return;
        this.info.SSOOnly = organizationForSso.SSOSettings.LoginAccess;
      }
    }
  }
}
