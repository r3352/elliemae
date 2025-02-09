// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.BusinessObjects.Users;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents a Loan Associate within a loan.</summary>
  /// <remarks>A Loan Associate is a <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.User" /> or <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.UserGroup" /> which has been
  /// assigned to a role within the loan. A loan associate is typically reposnsible for completeing
  /// one or more tasks during the lifetime of a loan.</remarks>
  /// <example>
  ///       The following code displays the list of all assigned loan associates. If an
  ///       individual user is assigned to the role, that user's name is displayed. If
  ///       a User Group is assigned, the group's name is displayed instead.
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
  /// 
  ///       // Loop thru the assigned loan associates and display the user info for each.
  ///       foreach (LoanAssociate la in loan.Associates)
  ///       {
  ///         // Write the role abbreviation and the user's name
  ///         if (la.AssociateType == LoanAssociateType.User)
  ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.User.FullName);
  ///         else if (la.AssociateType == LoanAssociateType.UserGroup)
  ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.UserGroup.Name);
  ///       }
  /// 
  ///       // Close the loan file
  ///       loan.Close();
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class LoanAssociate : ILoanAssociate
  {
    private Loan loan;
    private LoanAssociateLog logItem;
    private Role role;
    private User user;
    private UserGroup userGroup;
    private MilestoneEvent msEvent;

    internal LoanAssociate(Loan loan, LoanAssociateLog logItem, Role role)
    {
      this.loan = loan;
      this.logItem = logItem;
      this.role = role;
    }

    /// <summary>Gets the type of associate represented by the object.</summary>
    /// <example>
    ///       The following code displays the list of all assigned loan associates. If an
    ///       individual user is assigned to the role, that user's name is displayed. If
    ///       a User Group is assigned, the group's name is displayed instead.
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
    /// 
    ///       // Loop thru the assigned loan associates and display the user info for each.
    ///       foreach (LoanAssociate la in loan.Associates)
    ///       {
    ///         // Write the role abbreviation and the user's name
    ///         if (la.AssociateType == LoanAssociateType.User)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.User.FullName);
    ///         else if (la.AssociateType == LoanAssociateType.UserGroup)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.UserGroup.Name);
    ///       }
    /// 
    ///       // Close the loan file
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LoanAssociateType AssociateType => (LoanAssociateType) this.logItem.LoanAssociateType;

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.User" /> who is acting as the loan associate for the specified
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.WorkflowRole" />.
    /// </summary>
    /// <remarks>This property will return <c>null</c> if there is no user currently
    /// associated with the specified role/milestone. See also the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.AssignUser(EllieMae.Encompass.BusinessObjects.Users.User)" /> method
    /// as an alternate to setting this property from a COM-based environment.</remarks>
    /// <example>
    ///       The following code displays the list of all assigned loan associates. If an
    ///       individual user is assigned to the role, that user's name is displayed. If
    ///       a User Group is assigned, the group's name is displayed instead.
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
    /// 
    ///       // Loop thru the assigned loan associates and display the user info for each.
    ///       foreach (LoanAssociate la in loan.Associates)
    ///       {
    ///         // Write the role abbreviation and the user's name
    ///         if (la.AssociateType == LoanAssociateType.User)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.User.FullName);
    ///         else if (la.AssociateType == LoanAssociateType.UserGroup)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.UserGroup.Name);
    ///       }
    /// 
    ///       // Close the loan file
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public User User
    {
      get
      {
        if (this.AssociateType != LoanAssociateType.User || (this.logItem.LoanAssociateID ?? "") == "")
          return (User) null;
        if (this.user == null || this.user.ID != this.logItem.LoanAssociateID)
          this.user = this.loan.Session.Users.GetUser(this.logItem.LoanAssociateID);
        return this.user;
      }
      set
      {
        if (value == null && this.AssociateType == LoanAssociateType.User)
        {
          this.loan.Unwrap().ClearLoanAssociate(this.logItem);
        }
        else
        {
          if (value == null)
            return;
          this.loan.Unwrap().AssignLoanAssociate(this.logItem, value.Unwrap());
        }
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.UserGroup" /> who is acting as the loan associate for the specified
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.WorkflowRole" />.
    /// </summary>
    /// <remarks>This property will return <c>null</c> if there is no User Group currently
    /// associated with the specified role/milestone. See also the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.AssignUserGroup(EllieMae.Encompass.BusinessObjects.Users.UserGroup)" /> method
    /// as an alternate to setting this property from a COM-based environment.</remarks>
    /// <example>
    ///       The following code displays the list of all assigned loan associates. If an
    ///       individual user is assigned to the role, that user's name is displayed. If
    ///       a User Group is assigned, the group's name is displayed instead.
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
    /// 
    ///       // Loop thru the assigned loan associates and display the user info for each.
    ///       foreach (LoanAssociate la in loan.Associates)
    ///       {
    ///         // Write the role abbreviation and the user's name
    ///         if (la.AssociateType == LoanAssociateType.User)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.User.FullName);
    ///         else if (la.AssociateType == LoanAssociateType.UserGroup)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.UserGroup.Name);
    ///       }
    /// 
    ///       // Close the loan file
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public UserGroup UserGroup
    {
      get
      {
        if (this.AssociateType != LoanAssociateType.UserGroup || (this.logItem.LoanAssociateID ?? "") == "")
          return (UserGroup) null;
        if (this.userGroup == null || this.userGroup.ID.ToString() != this.logItem.LoanAssociateID)
          this.userGroup = this.loan.Session.Users.Groups.GetGroupByID(Utils.ParseInt((object) this.logItem.LoanAssociateID));
        return this.userGroup;
      }
      set
      {
        if (value == null && this.AssociateType == LoanAssociateType.UserGroup)
        {
          this.loan.Unwrap().ClearLoanAssociate(this.logItem);
        }
        else
        {
          if (value == null)
            return;
          this.loan.Unwrap().AssignLoanAssociate(this.logItem, value.Unwrap());
        }
      }
    }

    /// <summary>
    /// Gets or sets the contact email for the loan associate.
    /// </summary>
    public string ContactName => this.logItem.LoanAssociateName ?? "";

    /// <summary>
    /// Gets or sets the contact email for the loan associate.
    /// </summary>
    public string ContactEmail
    {
      get => this.logItem.LoanAssociateEmail ?? "";
      set => this.logItem.LoanAssociateEmail = value ?? "";
    }

    /// <summary>
    /// Gets or sets the contact phone number for the loan associate.
    /// </summary>
    public string ContactPhone
    {
      get => this.logItem.LoanAssociatePhone ?? "";
      set => this.logItem.LoanAssociatePhone = value ?? "";
    }

    /// <summary>
    /// Gets or sets the contact phone number for the loan associate.
    /// </summary>
    public string ContactFax
    {
      get => this.logItem.LoanAssociateFax ?? "";
      set => this.logItem.LoanAssociateFax = value ?? "";
    }

    /// <summary>
    /// Gets or sets the contact phone number for the loan associate.
    /// </summary>
    public string ContactCellPhone
    {
      get => this.logItem.LoanAssociateCellPhone ?? "";
      set => this.logItem.LoanAssociateCellPhone = value ?? "";
    }

    /// <summary>
    /// Gets the MilestoneEvent through which the loan associate became linked to the specified
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" />.
    /// </summary>
    /// <remarks>If this user is directly associated to a Role without being linked to a milestone
    /// (i.e a "milestone-free role"), this property will return <c>null</c>.</remarks>
    /// <example>
    ///       The following example locates all LoanAssociate records assigned to a
    ///       particular user and removes the user from the record. This will ensure the
    ///       user is no longer a loan associate for the loan.
    ///       <code>
    ///         <![CDATA[
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
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Fetch the core Processing milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       // Retrieve the Loan Processor Team user group
    ///       UserGroup lpteam = session.Users.Groups.GetGroupByName("Loan Processor Team");
    /// 
    ///       // Iterate thru the LoanAssociates, looking for any associate which is tied to
    ///       // the Processing milestone or any milestone which is a sub-milestone of Processing.
    ///       // When found, assign the user group to that associate record.
    ///       foreach (LoanAssociate la in loan.Associates)
    ///         if (la.MilestoneEvent != null)
    ///         {
    ///           // Retrieve the milestone event
    ///           Milestone ms = session.Loans.Milestones.GetItemByName(la.MilestoneEvent.MilestoneName);
    /// 
    ///           if (ms == processing || ms.CoreMilestone == processing)
    ///             la.AssignUserGroup(lpteam);
    ///         }
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
    public MilestoneEvent MilestoneEvent
    {
      get
      {
        if (this.msEvent == null && this.logItem is MilestoneLog)
          this.msEvent = (MilestoneEvent) this.loan.Log.MilestoneEvents.Find((LogRecordBase) this.logItem, false);
        return this.msEvent;
      }
    }

    /// <summary>Gets the user's role within the loan.</summary>
    /// <remarks>This property, which is never <c>null</c>, returns the user's assigned Role within the
    /// loan.</remarks>
    /// <example>
    ///       The following code displays the list of all assigned loan associates. If an
    ///       individual user is assigned to the role, that user's name is displayed. If
    ///       a User Group is assigned, the group's name is displayed instead.
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
    /// 
    ///       // Loop thru the assigned loan associates and display the user info for each.
    ///       foreach (LoanAssociate la in loan.Associates)
    ///       {
    ///         // Write the role abbreviation and the user's name
    ///         if (la.AssociateType == LoanAssociateType.User)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.User.FullName);
    ///         else if (la.AssociateType == LoanAssociateType.UserGroup)
    ///           Console.WriteLine(la.WorkflowRole.Abbreviation + ": " + la.UserGroup.Name);
    ///       }
    /// 
    ///       // Close the loan file
    ///       loan.Close();
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public Role WorkflowRole => this.role;

    /// <summary>
    /// Gets or sets whether the loan associate for this role/milestone should be grated write
    /// access to the loan.
    /// </summary>
    public bool AllowWriteAccess
    {
      get => this.logItem.LoanAssociateAccess;
      set => this.logItem.LoanAssociateAccess = value;
    }

    /// <summary>Assigns a User to be the loan associate.</summary>
    /// <param name="associateUser">The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.User" /> to assign, or <c>null</c> to remove the assignment.</param>
    /// <remarks>This method is provided as an alternative to using the User property when the API is
    /// used from within a COM-based environment.</remarks>
    /// <example>
    ///       The following code assigns a user to the role which is tied to the
    ///       Approval milestone.
    ///       <code>
    ///         <![CDATA[
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
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan from the My Pipeline folder and retrieve the information on the current lock
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Example#1");
    ///       loan.Lock();
    /// 
    ///       // Retrieve the Approval Milestone event
    ///       Milestone approval = session.Loans.Milestones.Approval;
    ///       MilestoneEvent approvalEvent = loan.Log.MilestoneEvents.GetEventForMilestone(approval.Name);
    /// 
    ///       // Retrieve the user to be assigned to the milestone
    ///       User user = session.Users.GetUser("kjones");
    /// 
    ///       // Assign the user to the milestones
    ///       approvalEvent.LoanAssociate.AssignUser(user);
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
    public void AssignUser(User associateUser) => this.User = associateUser;

    /// <summary>Assigns a User to be the loan associate.</summary>
    /// <param name="associateGroup">The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.UserGroup" /> to assign, or <c>null</c> to remove the assignment.</param>
    /// <remarks>This method is provided as an alternative to using the User property when the API is
    /// used from within a COM-based environment.</remarks>
    /// <example>
    ///       The following code assigns a user group to the Lock Desk role in the loan.
    ///       The assignnment is only made if the role is not already assigned to another
    ///       user or group.
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
    ///       // Retrieve the Lock Desk role from the Roles collection
    ///       Role lockDeskRole = session.Loans.Roles.GetRoleByName("Lock Desk");
    /// 
    ///       // Retrieve the Lock Desk UserGroup that will be assigned to the role
    ///       UserGroup lockDeskGroup = session.Users.Groups.GetGroupByName("Lock Desk Users");
    /// 
    ///       // Retrieve the LoanAssociate record(s) from the role and assign the group
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByRole(lockDeskRole))
    ///         if (!la.Assigned)
    ///           la.AssignUserGroup(lockDeskGroup);
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
    public void AssignUserGroup(UserGroup associateGroup) => this.UserGroup = associateGroup;

    /// <summary>
    /// Clears the current assignment, if any, to the currently assigned <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.User" />
    /// or <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.UserGroup" />.
    /// </summary>
    /// <example>
    ///       The following example locates all LoanAssociate records assigned to a
    ///       particular user and removes the user from the record. This will ensure the
    ///       user is no longer a loan associate for the loan.
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
    ///       // Retrieve the user to be removed from the loan
    ///       User user = session.Users.GetUser("rsmith");
    /// 
    ///       // For each loan associate record assigned to this user, unassign the record
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByUser(user))
    ///         la.Unassign();
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
    public void Unassign()
    {
      if (this.logItem.LoanAssociateType == EllieMae.EMLite.DataEngine.Log.LoanAssociateType.None)
        return;
      this.loan.Unwrap().ClearLoanAssociate(this.logItem);
    }

    /// <summary>
    /// Determines if the current LoanAssociate record is assigned to a <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.User" />
    /// or <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.LoanAssociate.UserGroup" />.
    /// </summary>
    /// <example>
    ///       The following code assigns a user group to the Lock Desk role in the loan.
    ///       The assignnment is only made if the role is not already assigned to another
    ///       user or group.
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
    ///       // Retrieve the Lock Desk role from the Roles collection
    ///       Role lockDeskRole = session.Loans.Roles.GetRoleByName("Lock Desk");
    /// 
    ///       // Retrieve the Lock Desk UserGroup that will be assigned to the role
    ///       UserGroup lockDeskGroup = session.Users.Groups.GetGroupByName("Lock Desk Users");
    /// 
    ///       // Retrieve the LoanAssociate record(s) from the role and assign the group
    ///       foreach (LoanAssociate la in loan.Associates.GetAssociatesByRole(lockDeskRole))
    ///         if (!la.Assigned)
    ///           la.AssignUserGroup(lockDeskGroup);
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
    public bool Assigned => this.logItem.LoanAssociateType != 0;

    /// <summary>
    /// Provides an equality operator for two LoanAssociate objects.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      return obj is LoanAssociate loanAssociate && loanAssociate.logItem.LoanAssociateID == this.logItem.LoanAssociateID && this.role.ID == loanAssociate.role.ID;
    }

    /// <summary>Provides a hash code for the Loan Associate.</summary>
    /// <returns></returns>
    public override int GetHashCode() => this.logItem.LoanAssociateID.GetHashCode();
  }
}
