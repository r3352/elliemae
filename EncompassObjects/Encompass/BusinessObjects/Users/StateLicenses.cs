// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.StateLicenses
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Provides access to the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.StateLicense" /> objects associated with
  /// a particular <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" />.
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

    /// <summary>Returns the license for a particular state.</summary>
    /// <remarks>The state should be a 2-character abbreviation and is not
    /// case sensitive.</remarks>
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
    public StateLicense this[string state]
    {
      get
      {
        return (state ?? "").Length == 2 ? (StateLicense) this.licenses[(object) state] : throw new ArgumentException("Invalid state specified");
      }
    }

    /// <summary>Adds a license for a particular state.</summary>
    /// <param name="state">The state in which to add the license.</param>
    /// <returns>A new state license object or, if a license already exists for
    /// the specific state, the existing StateLicense object.</returns>
    /// <example>
    /// The following code demonstrates how to add a new state license to an existing
    /// Loan Officer account so that the LO can originate loans in that state.
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
    ///       // Add a new license for the state of Maryland
    ///       StateLicense license = officer.StateLicenses.Add("MD");
    /// 
    ///       // Set the license number and enable the license
    ///       license.LicenseNumber = "ABCD01234";
    ///       license.Enabled = true;
    /// 
    ///       // Commit the user to save changes to the license
    ///       officer.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public StateLicense Add(string state)
    {
      if ((state ?? "").Length != 2)
        throw new ArgumentException("Invalid state specified");
      if (!this.licenses.Contains((object) state))
        this.licenses.Add((object) state, (object) new StateLicense(new LOLicenseInfo(this.parentUser.ID, state)));
      return (StateLicense) this.licenses[(object) state];
    }

    /// <summary>Adds a license for a particular state.</summary>
    /// <param name="state">The state in which to add the license.</param>
    /// <param name="licenseNo" />
    /// <param name="issueDate" />
    /// <param name="startDate" />
    /// <param name="endDate" />
    /// <param name="licenseStatus" />
    /// <param name="statusDate" />
    /// <param name="approved" />
    /// <param name="exempt" />
    /// <param name="lastChecked" />
    /// <returns>A new state license object or, if a license already exists for
    /// the specific state, the existing StateLicense object.</returns>
    /// <example>
    /// The following code demonstrates how to add a new state license to an existing
    /// Loan Officer account so that the LO can originate loans in that state.
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
    ///       // Add a new license for the state of Maryland
    ///       StateLicense license = officer.StateLicenses.Add("MD");
    /// 
    ///       // Set the license number and enable the license
    ///       license.LicenseNumber = "ABCD01234";
    ///       license.Enabled = true;
    /// 
    ///       // Commit the user to save changes to the license
    ///       officer.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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
        this.licenses.Add((object) state, (object) new StateLicense(new LOLicenseInfo(new EllieMae.EMLite.RemotingServices.StateLicenseExtType(state, string.Empty, licenseNo, issueDate, startDate, endDate, licenseStatus, statusDate, approved, exempt, lastChecked))
        {
          UserID = this.parentUser.ID
        }));
      return (StateLicense) this.licenses[(object) state];
    }

    /// <summary>Removes a license from the collection.</summary>
    /// <param name="state">The state for which the license should be removed.</param>
    /// <example>
    /// The following code demonstrates how to remove a license from a Loan Officer
    /// so they are no longer able to originate loans in a particular state.
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
    ///       // Removes the user's existing license for the state of Maryland, if any
    ///       officer.StateLicenses.Remove("MD");
    /// 
    ///       // Commit the user to save changes to the license
    ///       officer.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(string state)
    {
      if (!this.licenses.Contains((object) state))
        return;
      this.licenses.Remove((object) state);
    }

    /// <summary>Removes all licenses for the current user.</summary>
    public void Clear() => this.licenses.Clear();

    /// <summary>
    /// Provides an enumerator for the StateLicense objects contained in the collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.licenses.Values.GetEnumerator();

    internal void Commit()
    {
      LOLicenseInfo[] licenses = new LOLicenseInfo[this.licenses.Count];
      int num = 0;
      foreach (StateLicense stateLicense in (IEnumerable) this.licenses.Values)
      {
        LOLicenseInfo loLicenseInfo = new LOLicenseInfo(this.parentUser.ID, stateLicense.Unwrap());
        loLicenseInfo.Selected = loLicenseInfo.Approved = stateLicense.Selected;
        loLicenseInfo.EndDate = stateLicense.ExpirationDate == null ? DateTime.MinValue : (DateTime) stateLicense.ExpirationDate;
        loLicenseInfo.Exempt = stateLicense.Exempt;
        loLicenseInfo.IssueDate = stateLicense.IssueDate == null ? DateTime.MinValue : (DateTime) stateLicense.IssueDate;
        loLicenseInfo.LastChecked = stateLicense.LastChecked == null ? DateTime.MinValue : (DateTime) stateLicense.LastChecked;
        loLicenseInfo.LicenseStatus = stateLicense.LicenseStatus;
        loLicenseInfo.StartDate = stateLicense.StartDate == null ? DateTime.MinValue : (DateTime) stateLicense.StartDate;
        loLicenseInfo.StatusDate = stateLicense.StatusDate == null ? DateTime.MinValue : (DateTime) stateLicense.StatusDate;
        licenses[num++] = loLicenseInfo;
      }
      this.mngr.SetLOLicenses(this.parentUser.ID, licenses);
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
