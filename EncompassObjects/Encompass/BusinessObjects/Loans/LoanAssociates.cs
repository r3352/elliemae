// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAssociates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides access to the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> objects for the loan.
  /// </summary>
  /// <remarks>A Loan Associate is a user who is assigned to a role within a loan. Common examples
  /// are the Loan Officer and Loan Processor for the loan. Every milestone in the loan can have
  /// assigned to it a distinct loan associate. However, an associate can also be assigned to a role
  /// which is not associated with any milestone.</remarks>
  /// <example>
  ///       The following code demonstrates how retrieve all users who are assigned
  ///       to the Loan Officer role on a loan.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.StartOffline("mary", "maryspwd");
  /// 
  ///       // Open the loan using the GUID specified on the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  /// 
  ///       // Retrieve the Role object for the LO role
  ///       Role loRole = session.Loans.Roles.GetRoleByAbbrev("LO");
  /// 
  ///       // Get all LoanAssociate objects for this role
  ///       LoanAssociateList los = loan.Associates.GetAssociatesByRole(loRole);
  /// 
  ///       foreach (LoanAssociate lo in los)
  ///       {
  ///         if (lo.MilestoneEvent != null)
  ///           Console.WriteLine("The user " + lo.User.ID + " is the LO for the "
  ///             + lo.MilestoneEvent.MilestoneName + " milestone.");
  ///         else
  ///           Console.WriteLine("The user " + lo.User.ID + " is the LO for the "
  ///             + "loan (not assigned to a milestone).");
  ///       }
  /// 
  ///       // Close the loan
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class LoanAssociates : ILoanAssociates, IEnumerable
  {
    private Loan loan;

    internal LoanAssociates(Loan loan) => this.loan = loan;

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate">LoanAssociates</see> that fill
    /// the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" />.
    /// </summary>
    /// <param name="selectedRole">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> for which users are found.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanAssociateList" /> containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> records
    /// for the specified role.</returns>
    /// <remarks>
    /// A role can be associated with multiple users by means of having multiple <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneEvent" />
    /// objects which are mapped to the same Role. In all other cases there can only be a single user
    /// associated with a given role.
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how retrieve all users who are assigned
    ///       to the Loan Officer role on a loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Retrieve the Role object for the LO role
    ///       Role loRole = session.Loans.Roles.GetRoleByAbbrev("LO");
    /// 
    ///       // Get all LoanAssociate objects for this role
    ///       LoanAssociateList los = loan.Associates.GetAssociatesByRole(loRole);
    /// 
    ///       foreach (LoanAssociate lo in los)
    ///       {
    ///         if (lo.MilestoneEvent != null)
    ///           Console.WriteLine("The user " + lo.User.ID + " is the LO for the "
    ///             + lo.MilestoneEvent.MilestoneName + " milestone.");
    ///         else
    ///           Console.WriteLine("The user " + lo.User.ID + " is the LO for the "
    ///             + "loan (not assigned to a milestone).");
    ///       }
    /// 
    ///       // Close the loan
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList GetAssociatesByRole(Role selectedRole)
    {
      if (selectedRole == null)
        throw new ArgumentNullException("Specified role cannot be null");
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList associatesByRole = new LoanAssociateList();
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.WorkflowRole.Equals((object) selectedRole))
          associatesByRole.Add(loanAssociate);
      }
      return associatesByRole;
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate">LoanAssociates</see> that fill
    /// the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" />.
    /// </summary>
    /// <param name="selectedRole">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" /> for which the associates will be found.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanAssociateList" /> containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> records
    /// for the specified role.</returns>
    /// <remarks>If there is no <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> mapped to the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" />,
    /// then an empty collection will be returned.</remarks>
    public LoanAssociateList GetAssociatesByRole(FixedRole selectedRole)
    {
      Role fixedRole = this.loan.Session.Loans.Roles.GetFixedRole(selectedRole);
      return fixedRole == null ? new LoanAssociateList() : this.GetAssociatesByRole(fixedRole);
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> objects that apply to a particular <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" />.
    /// </summary>
    /// <param name="associate">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> whose associate records are being retrieved.</param>
    /// <returns>Returns a LoanAssociateList containing the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" />
    /// objects assigned to the user.</returns>
    /// <remarks>Only LoanAssociate objects to which the user is directly assigned (i.e. not assigned
    /// thru membership of a UserGroup) will be returned.</remarks>
    /// <example>
    ///       The following code demonstrates how reassign all of the roles in a loan
    ///       from one user to another.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Get the two user objects -- the source (officer) and target (newofficer)
    ///       User priorUser = session.Users.GetUser("officer");
    ///       User newUser = session.Users.GetUser("newofficer");
    /// 
    ///       // Find all roles the user is assigned to
    ///       LoanAssociateList las = loan.Associates.GetAssociatesByUser(priorUser);
    /// 
    ///       foreach (LoanAssociate la in las)
    ///         la.User = newUser;
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanAssociateList GetAssociatesByUser(User associate)
    {
      return this.GetAssociatesByUser(associate, false);
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> objects that apply to a particular <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" />.
    /// </summary>
    /// <param name="associate">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> whose associate records are being retrieved.</param>
    /// <param name="includeGroups">Indicates if the method should inclyde LoanAssociate records
    /// that are assigned to UsersGroups to which the specified user belongs.</param>
    /// <returns>Returns a LoanAssociateList containing the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" />
    /// objects assigned to the user.</returns>
    /// <example>
    ///       The following code demonstrates how reassign all of the roles in a loan
    ///       from one user to another.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Get the two user objects -- the source (officer) and target (newofficer)
    ///       User priorUser = session.Users.GetUser("officer");
    ///       User newUser = session.Users.GetUser("newofficer");
    /// 
    ///       // Find all roles the user is assigned to
    ///       LoanAssociateList las = loan.Associates.GetAssociatesByUser(priorUser);
    /// 
    ///       foreach (LoanAssociate la in las)
    ///         la.User = newUser;
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public LoanAssociateList GetAssociatesByUser(User associate, bool includeGroups)
    {
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList associatesByUser = new LoanAssociateList();
      UserGroupList userGroupList = (UserGroupList) null;
      if (includeGroups)
        userGroupList = new UserGroupList((IList) associate.GetUserGroups());
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.User != null && loanAssociate.User.Equals((object) associate))
          associatesByUser.Add(loanAssociate);
        else if (includeGroups && loanAssociate.UserGroup != null && userGroupList.Contains(loanAssociate.UserGroup))
          associatesByUser.Add(loanAssociate);
      }
      return associatesByUser;
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> objects that apply to a particular <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" />.
    /// </summary>
    /// <param name="associate">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> whose associate records are being retrieved.</param>
    /// <returns>Returns a LoanAssociateList containing the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" />
    /// objects assigned to the user group.</returns>
    /// <example>
    /// The following code identifies all LoanAssociate records assigned to a
    /// particular UserGroup and reassigns them to a new User.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Retrieve the Loan Processor Team user group
    ///       UserGroup lpteam = session.Users.Groups.GetGroupByName("Loan Processor Team");
    /// 
    ///       // Get the user to which the role will be reassigned
    ///       User lpuser = session.Users.GetUser("jshipley");
    /// 
    ///       // Loop over all LoanAssociate records assigned to this group and reassign them
    ///       // to the new user.
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByUserGroup(lpteam))
    ///         la.AssignUser(lpuser);
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList GetAssociatesByUserGroup(UserGroup associate)
    {
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList associatesByUserGroup = new LoanAssociateList();
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.UserGroup != null && loanAssociate.UserGroup.Equals((object) associate))
          associatesByUserGroup.Add(loanAssociate);
      }
      return associatesByUserGroup;
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate" /> objects associated with Milestone events.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanAssociateList" /> containing the matching Loan Associates.
    /// </returns>
    /// <example>
    ///       The following code identifies all LoanAssociate records assigned to a
    ///       particular UserGroup and reassigns them to a new User.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Fetch the core Processing milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       // Get the User who will be the assigned user for the milestones
    ///       User processingUser = session.Users.GetUser("sstevens");
    /// 
    ///       // Loop over the LoanAssociate records that are tied to Milestones, looking for
    ///       // any record that's for the Processing milestone or any sub-milestone of Processing.
    ///       foreach (LoanAssociate la in loan.Associates.GetMilestoneAssociates())
    ///       {
    ///         // Fetch the milestone for the record
    ///         Milestone ms = session.Loans.Milestones.GetItemByName(la.MilestoneEvent.MilestoneName);
    /// 
    ///         if (ms == processing || ms.CoreMilestone == processing)
    ///           la.AssignUser(processingUser);
    ///       }
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList GetMilestoneAssociates()
    {
      LoanAssociateList loanAssociates = this.getLoanAssociates();
      LoanAssociateList milestoneAssociates = new LoanAssociateList();
      foreach (LoanAssociate loanAssociate in (CollectionBase) loanAssociates)
      {
        if (loanAssociate.MilestoneEvent != null)
          milestoneAssociates.Add(loanAssociate);
      }
      return milestoneAssociates;
    }

    /// <summary>
    /// Assigns the specified user as the Loan Associate for the specified role.
    /// </summary>
    /// <param name="roleToAssign">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> for which the user is assigned.</param>
    /// <param name="assignedUser">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> to be assigned to the role.</param>
    /// <remarks>If there are multiple milestones that share the same role assignment, the specified
    /// user is assigned as the loan associate for all of the milestones.</remarks>
    /// <example>
    ///       The following code assigns a specified user to all LoanAssociate records
    ///       that are tied to the Loan Officer role.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Get the user to which the role will be reassigned
    ///       User user = session.Users.GetUser("kjones");
    /// 
    ///       // Fetch the Loan Officer role
    ///       Role role = session.Loans.Roles.GetRoleByAbbrev("LO");
    /// 
    ///       // Assign the user to the specified role. This will update all LoanAssociate
    ///       // records tied to the specified role.
    ///       loan.Associates.AssignUser(role, user);
    /// 
    ///       // Iterate over all of the LoanAssociate records for the role and verify the user was
    ///       // assigned. The error message should never be printed out.
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByRole(role))
    ///         if (!user.Equals(la.User))
    ///         {
    ///           Console.WriteLine("Error! The user was not assigned to the correct role!");
    ///         }
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList AssignUser(Role roleToAssign, User assignedUser)
    {
      LoanAssociateList associatesByRole = this.GetAssociatesByRole(roleToAssign);
      foreach (LoanAssociate loanAssociate in (CollectionBase) associatesByRole)
        loanAssociate.AssignUser(assignedUser);
      return associatesByRole;
    }

    /// <summary>
    /// Assigns the specified user as the Loan Associate for the specified role.
    /// </summary>
    /// <param name="roleToAssign">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" /> for which the user is assigned.</param>
    /// <param name="assignedUser">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> to be assigned to the role.</param>
    /// <remarks>If there are multiple milestones that share the same role assignment, the specified
    /// user is assigned as the loan associate for all of the milestones.</remarks>
    /// <example>
    ///       The following code assigns a specified user to all LoanAssociate records
    ///       that are tied to the fixed Loan Officer role.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Get the user to which the role will be reassigned
    ///       User user = session.Users.GetUser("kjones");
    /// 
    ///       // Assign the user to the specified role. This will update all LoanAssociate
    ///       // records tied to the specified fixed role.
    ///       loan.Associates.AssignUser(FixedRole.LoanOfficer, user);
    /// 
    ///       // Translate the FixedRole into an actual Role object
    ///       Role role = session.Loans.Roles.GetFixedRole(FixedRole.LoanOfficer);
    /// 
    ///       // Iterate over all of the LoanAssociate records for the role and verify the user was
    ///       // assigned. The error message should never be printed out.
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByRole(role))
    ///         if (!user.Equals(la.User))
    ///         {
    ///           Console.WriteLine("Error! The user was not assigned to the correct role!");
    ///         }
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList AssignUser(FixedRole roleToAssign, User assignedUser)
    {
      Role fixedRole = this.loan.Session.Loans.Roles.GetFixedRole(roleToAssign);
      return fixedRole == null ? new LoanAssociateList() : this.AssignUser(fixedRole, assignedUser);
    }

    /// <summary>
    /// Assigns the specified User Group as the Loan Associate for the specified role.
    /// </summary>
    /// <param name="roleToAssign">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> for which the user is assigned.</param>
    /// <param name="assignedGroup">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> to be assigned to the role.</param>
    /// <remarks>If there are multiple milestones that share the same role assignment, the specified
    /// group is assigned as the loan associate for all of the milestones.</remarks>
    /// <example>
    ///       The following code assigns a team of users to all LoanAssociate records
    ///       that are tied to the Loan Processor role.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Get the user to which the role will be reassigned
    ///       UserGroup lpteam = session.Users.Groups.GetGroupByName("Loan Processing Team");
    /// 
    ///       // Get the Loan Processor role
    ///       Role role = session.Loans.Roles.GetRoleByName("Loan Processor");
    /// 
    ///       // Assign the user to the specified role. This will update all LoanAssociate
    ///       // records tied to the specified fixed role.
    ///       loan.Associates.AssignUserGroup(role, lpteam);
    /// 
    ///       // Iterate over all of the LoanAssociate records for the role and verify the user was
    ///       // assigned. The error message should never be printed out.
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByRole(role))
    ///         if (!lpteam.Equals(la.UserGroup))
    ///         {
    ///           Console.WriteLine("Error! The user was not assigned to the correct role!");
    ///         }
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList AssignUserGroup(Role roleToAssign, UserGroup assignedGroup)
    {
      LoanAssociateList associatesByRole = this.GetAssociatesByRole(roleToAssign);
      foreach (LoanAssociate loanAssociate in (CollectionBase) associatesByRole)
        loanAssociate.AssignUserGroup(assignedGroup);
      return associatesByRole;
    }

    /// <summary>
    /// Assigns the specified User Group as the Loan Associate for the specified role.
    /// </summary>
    /// <param name="roleToAssign">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FixedRole" /> for which the user is assigned.</param>
    /// <param name="assignedGroup">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup" /> to be assigned to the role.</param>
    /// <remarks>If there are multiple milestones that share the same role assignment, the specified
    /// group is assigned as the loan associate for all of the milestones.</remarks>
    /// <example>
    ///       The following code assigns a team of users to all LoanAssociate records
    ///       that are tied to the fixed Loan Processor role.
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
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Get the user to which the role will be reassigned
    ///       UserGroup lpteam = session.Users.Groups.GetGroupByName("Loan Processing Team");
    /// 
    ///       // Assign the user to the specified role. This will update all LoanAssociate
    ///       // records tied to the specified fixed role.
    ///       loan.Associates.AssignUserGroup(FixedRole.LoanProcessor, lpteam);
    /// 
    ///       // Map the FixedRole to the underlying Role object
    ///       Role lprole = session.Loans.Roles.GetFixedRole(FixedRole.LoanProcessor);
    /// 
    ///       // Iterate over all of the LoanAssociate records for the role and verify the user was
    ///       // assigned. The error message should never be printed out.
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByRole(lprole))
    ///         if (!lpteam.Equals(la.UserGroup))
    ///         {
    ///           Console.WriteLine("Error! The user was not assigned to the correct role!");
    ///         }
    /// 
    ///       // Save the loan to commit this new payment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateList AssignUserGroup(FixedRole roleToAssign, UserGroup assignedGroup)
    {
      Role fixedRole = this.loan.Session.Loans.Roles.GetFixedRole(roleToAssign);
      return fixedRole == null ? new LoanAssociateList() : this.AssignUserGroup(fixedRole, assignedGroup);
    }

    /// <summary>Returns an enumerator for the list of role associates</summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.getLoanAssociates().GetEnumerator();

    private LoanAssociateList getLoanAssociates()
    {
      LoanAssociateList loanAssociates = new LoanAssociateList();
      Hashtable hashtable = new Hashtable();
      LogList logList = this.loan.Unwrap().LoanData.GetLogList();
      foreach (MilestoneEvent milestoneEvent in (LoanLogEntryCollection) this.loan.Log.MilestoneEvents)
      {
        LoanAssociate loanAssociate = milestoneEvent.LoanAssociate;
        if (loanAssociate != null)
        {
          loanAssociates.Add(loanAssociate);
          hashtable[(object) loanAssociate.WorkflowRole.ID] = (object) true;
        }
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in logList.GetAllMilestoneFreeRoles())
      {
        Role roleById = this.loan.Session.Loans.Roles.GetRoleByID(milestoneFreeRole.RoleID);
        if (roleById != null && !hashtable.Contains((object) roleById.ID))
        {
          loanAssociates.Add(new LoanAssociate(this.loan, (LoanAssociateLog) milestoneFreeRole, roleById));
          hashtable[(object) roleById.ID] = (object) true;
        }
      }
      foreach (Role role in this.loan.Session.Loans.Roles)
      {
        if (!hashtable.Contains((object) role.ID))
        {
          MilestoneFreeRoleLog milestoneFreeRoleLog = new MilestoneFreeRoleLog();
          milestoneFreeRoleLog.RoleID = role.ID;
          milestoneFreeRoleLog.RoleName = role.Name;
          loanAssociates.Add(new LoanAssociate(this.loan, (LoanAssociateLog) milestoneFreeRoleLog, role));
          hashtable[(object) role.ID] = (object) true;
          logList.AddRecord((LogRecordBase) milestoneFreeRoleLog);
        }
      }
      return loanAssociates;
    }
  }
}
