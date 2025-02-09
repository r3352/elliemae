// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.Milestone
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The Milestone represents a single event in the lifetime of a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see>.
  /// </summary>
  /// <remarks>See the remarks for the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestones">Milestones</see> class
  /// for a complete list of the predefined milestones and their sequence in the
  /// loan's lifetime.</remarks>
  /// <example>
  ///       The following code displays all of the Milestone information for a loan.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open a loan using its GUID
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  /// 
  ///       // Get the first milestone from the processing sequence
  ///       Milestone ms = session.Loans.Milestones.First;
  /// 
  ///       while (ms != null)
  ///       {
  ///          // Get the loan's MilestoneEvent for the current milestone
  ///          MilestoneEvent entry = loan.Log.MilestoneEvents.GetEventForMilestone(ms.Name);
  /// 
  ///          if (entry != null)
  ///          {
  ///             if (entry.Completed)
  ///                Console.WriteLine("Milestone " + ms.Name + " was completed on " + entry.Date);
  ///             else if (entry.Date != null)
  ///                Console.WriteLine("Milestone " + ms.Name + " is scheduled for completion on " + entry.Date);
  ///             else
  ///                Console.WriteLine("Milestone " + ms.Name + " is not currently scheduled for completion");
  ///          }
  /// 
  ///          // Move to the next Milestone in the sequence
  ///          ms = ms.Next;
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
  public class Milestone : EnumItem, IMilestone
  {
    private Milestone next;
    private Milestone prev;
    private string internalName = "";
    private bool isArchived;
    private Milestone coreMilestone;
    private string milestoneID = "";

    internal Milestone(int id, string name, bool isArchived, string milestoneID)
      : this(id, name)
    {
      this.isArchived = isArchived;
      this.milestoneID = milestoneID;
    }

    internal Milestone(int id, string name)
      : base(id, Milestone.fixCapitalization(name))
    {
      this.internalName = name;
    }

    internal Milestone(int id, string name, Milestone coreMilestone, bool isArchived)
      : this(id, name)
    {
      this.coreMilestone = coreMilestone;
      this.isArchived = isArchived;
    }

    /// <summary>
    /// MilestoneID for the Milestone
    /// <remarks>A number value indicates a "core" Milestone. A GUID value indicates a "custom" Milestone.</remarks>
    /// </summary>
    /// <remarks>This value will be the same even when the name of the Milestone is changed.
    /// It will match with the value of MilestoneEvent.MilestoneID of a loan even when the names are different.</remarks>
    /// <example>
    ///       The following compares the Milestone name from the MilestoneEvent of the Loan to the Milestone name in Encompass settings.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       Session session = new Session();
    ///       session.Start("localhost", "admin", "password");
    /// 
    /// 
    ///       Loan loan = session.Loans.Open("{391cc4f4-e7e0-44d5-91f4-01c0d240cdae}");
    ///       foreach (MilestoneEvent msEvt in loan.Log.MilestoneEvents)
    ///       {
    ///           Milestone ms = session.Loans.Milestones.GetItemByMilestoneID(msEvt.MilestoneID);
    ///           Console.WriteLine(msEvt.MilestoneName);
    ///           if (msEvt.MilestoneName != ms.Name)
    ///             Console.WriteLine("\tThe Milestone " + msEvt.MilestoneName + " has been renamed to " + ms.Name);
    ///       }
    /// 
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public string MilestoneID => this.milestoneID;

    /// <summary>
    /// Gets the next non-archived milestone in the milestone sequence that defines the lifetime of a loan.
    /// </summary>
    /// <example>
    ///       The following code displays all of the Milestone information for a loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan using its GUID
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Get the first milestone from the processing sequence
    ///       Milestone ms = session.Loans.Milestones.First;
    /// 
    ///       while (ms != null)
    ///       {
    ///          // Get the loan's MilestoneEvent for the current milestone
    ///          MilestoneEvent entry = loan.Log.MilestoneEvents.GetEventForMilestone(ms.Name);
    /// 
    ///          if (entry != null)
    ///          {
    ///             if (entry.Completed)
    ///                Console.WriteLine("Milestone " + ms.Name + " was completed on " + entry.Date);
    ///             else if (entry.Date != null)
    ///                Console.WriteLine("Milestone " + ms.Name + " is scheduled for completion on " + entry.Date);
    ///             else
    ///                Console.WriteLine("Milestone " + ms.Name + " is not currently scheduled for completion");
    ///          }
    /// 
    ///          // Move to the next Milestone in the sequence
    ///          ms = ms.Next;
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
    public Milestone Next => this.next;

    /// <summary>
    /// Gets the previous non-archived milestone in the milestone sequence that defines the lifetime of a loan.
    /// </summary>
    public Milestone Previous => this.prev;

    /// <summary>Gets a flag indicating if this is a custom milestone.</summary>
    [Obsolete("There are no longer custom Milestones in Encompass 9.0 so all Milestones are basically custom Milestones. For backward compatibility this will return false for the previous core Milestones and true for all others.")]
    public bool IsCustom
    {
      get
      {
        return this.Name.ToUpper() != "STARTED" && this.Name.ToUpper() != "PROCESSING" && this.Name.ToUpper() != "SUBMITTAL" && this.Name.ToUpper() != "APPROVAL" && this.Name.ToUpper() != "DOCS SIGNING" && this.Name.ToUpper() != "FUNDING" && this.Name.ToUpper() != "COMPLETION";
      }
    }

    /// <summary>Gets a flag indicating if the milestone is archived</summary>
    public bool IsArchived => this.isArchived;

    /// <summary>
    /// Gets the core milestone with which a custom milestone is associated.
    /// </summary>
    /// <remarks>This property returns <c>null</c> if the current Milestone
    /// is not a custom milestone.</remarks>
    [Obsolete("Starting from Encompass 9.0 there is no longer a concept of \"Core\" Milestones. This property will always return null.")]
    public Milestone CoreMilestone => this.coreMilestone;

    /// <summary>
    /// Determines if the current milestone occurs before the one specified
    /// </summary>
    /// <param name="ms">The milestone to compare against the current milestone.
    /// A <c>null</c> value will always result in this function returning <c>false</c>.</param>
    /// <returns>Returns <c>true</c> if the current milestone occurs earlier in
    /// the loan lifetime sequence than the specified milestone, <c>false</c> otherwise.
    /// </returns>
    /// <remarks>In languages that support operator overloading, this is equivalent
    /// to the less-than (&lt;) operator.</remarks>
    /// <example>
    ///       The following code checks if a loans has been sent to processing by determining
    ///       if its last completed milestone occurs on or after the Processing milestone
    ///       in the loan lifetime sequence.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the desired loan
    ///       LoanIdentityList ids = session.Loans.Folders["My Pipeline"].GetContents();
    /// 
    ///       // Get the Processing milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the loan
    ///          Loan loan = session.Loans.Open(id.Guid);
    /// 
    ///          // Check if it's been sent to processing
    ///          Milestone ms = session.Loans.Milestones.GetItemByName(loan.Log.MilestoneEvents.LastCompletedEvent.MilestoneName);
    /// 
    ///          if ((ms != null) && (ms.OccursOnOrAfter(processing)))
    ///             Console.WriteLine("The loan " + id.ToString() + " has already been sent to processing");
    /// 
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public bool OccursBefore(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this < ms;

    /// <summary>
    /// Determines if the current milestone occurs before the one specified or is the
    /// same as the specified one.
    /// </summary>
    /// <param name="ms">The milestone to compare against the current milestone.
    /// A <c>null</c> value will always result in this function returning <c>false</c>.</param>
    /// <returns>Returns <c>true</c> if the current milestone occurs earlier in
    /// the loan lifetime sequence than or is the same as the specified milestone,
    /// <c>false</c> otherwise.
    /// </returns>
    /// <remarks>In languages that support operator overloading, this is equivalent
    /// to the less-than-or-equals (&lt;=) operator.</remarks>
    /// <example>
    ///       The following code checks if a loans has been sent to processing by determining
    ///       if its last completed milestone occurs on or after the Processing milestone
    ///       in the loan lifetime sequence.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the desired loan
    ///       LoanIdentityList ids = session.Loans.Folders["My Pipeline"].GetContents();
    /// 
    ///       // Get the Processing milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the loan
    ///          Loan loan = session.Loans.Open(id.Guid);
    /// 
    ///          // Check if it's been sent to processing
    ///          Milestone ms = session.Loans.Milestones.GetItemByName(loan.Log.MilestoneEvents.LastCompletedEvent.MilestoneName);
    /// 
    ///          if ((ms != null) && (ms.OccursOnOrAfter(processing)))
    ///             Console.WriteLine("The loan " + id.ToString() + " has already been sent to processing");
    /// 
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public bool OccursOnOrBefore(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this <= ms;

    /// <summary>
    /// Determines if the current milestone occurs after the one specified
    /// </summary>
    /// <param name="ms">The milestone to compare against the current milestone.
    /// A <c>null</c> value will always result in this function returning <c>false</c>.</param>
    /// <returns>Returns <c>true</c> if the current milestone occurs later in
    /// the loan lifetime sequence than the specified milestone, <c>false</c> otherwise.
    /// </returns>
    /// <remarks>In languages that support operator overloading, this is equivalent
    /// to the greater-than (&gt;) operator.</remarks>
    /// <example>
    ///       The following code checks if a loans has been sent to processing by determining
    ///       if its last completed milestone occurs on or after the Processing milestone
    ///       in the loan lifetime sequence.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the desired loan
    ///       LoanIdentityList ids = session.Loans.Folders["My Pipeline"].GetContents();
    /// 
    ///       // Get the Processing milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the loan
    ///          Loan loan = session.Loans.Open(id.Guid);
    /// 
    ///          // Check if it's been sent to processing
    ///          Milestone ms = session.Loans.Milestones.GetItemByName(loan.Log.MilestoneEvents.LastCompletedEvent.MilestoneName);
    /// 
    ///          if ((ms != null) && (ms.OccursOnOrAfter(processing)))
    ///             Console.WriteLine("The loan " + id.ToString() + " has already been sent to processing");
    /// 
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public bool OccursAfter(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this > ms;

    /// <summary>
    /// Determines if the current milestone occurs after the one specified or is the same
    /// as the one specified.
    /// </summary>
    /// <param name="ms">The milestone to compare against the current milestone.
    /// A <c>null</c> value will always result in this function returning <c>false</c>.</param>
    /// <returns>Returns <c>true</c> if the current milestone occurs later in
    /// the loan lifetime sequence than or is the same as the specified milestone,
    /// <c>false</c> otherwise.
    /// </returns>
    /// <remarks>In languages that support operator overloading, this is equivalent
    /// to the greater-than-or-equals (&gt;=) operator.</remarks>
    /// <example>
    ///       The following code checks if a loans has been sent to processing by determining
    ///       if its last completed milestone occurs on or after the Processing milestone
    ///       in the loan lifetime sequence.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Events;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the desired loan
    ///       LoanIdentityList ids = session.Loans.Folders["My Pipeline"].GetContents();
    /// 
    ///       // Get the Processing milestone
    ///       Milestone processing = session.Loans.Milestones.Processing;
    /// 
    ///       foreach (LoanIdentity id in ids)
    ///       {
    ///          // Open the loan
    ///          Loan loan = session.Loans.Open(id.Guid);
    /// 
    ///          // Check if it's been sent to processing
    ///          Milestone ms = session.Loans.Milestones.GetItemByName(loan.Log.MilestoneEvents.LastCompletedEvent.MilestoneName);
    /// 
    ///          if ((ms != null) && (ms.OccursOnOrAfter(processing)))
    ///             Console.WriteLine("The loan " + id.ToString() + " has already been sent to processing");
    /// 
    ///          loan.Close();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public bool OccursOnOrAfter(Milestone ms) => !((EnumItem) ms == (EnumItem) null) && this >= ms;

    /// <summary>
    /// Compares two milestones to determine if one is after the other in the
    /// loan lifetime sequence.
    /// </summary>
    /// <param name="m1">The first Milestone to compare.</param>
    /// <param name="m2">The second Milestone to compare.</param>
    /// <returns>Returns true if <code>m1</code> occurs after <code>m2</code>
    /// in the loan lifetime sequence.</returns>
    /// <remarks>This operator will return false if either operand is null.</remarks>
    public static bool operator >(Milestone m1, Milestone m2)
    {
      if ((EnumItem) m1 == (EnumItem) null || (EnumItem) m2 == (EnumItem) null)
        return false;
      for (Milestone previous = m1.Previous; (EnumItem) previous != (EnumItem) null; previous = previous.Previous)
      {
        if ((EnumItem) previous == (EnumItem) m2)
          return true;
      }
      return false;
    }

    /// <summary>
    /// Compares two milestones to determine if they are equal or if one occurs after
    /// the other in the loan lifetime sequence.
    /// </summary>
    /// <param name="m1">The first Milestone to compare.</param>
    /// <param name="m2">The second Milestone to compare.</param>
    /// <returns>Returns true if <code>m1</code> and <code>m2</code> are the same
    /// milestone or if <code>m1</code> occurs after <code>m2</code>
    /// in the loan lifetime sequence.</returns>
    public static bool operator >=(Milestone m1, Milestone m2)
    {
      return (EnumItem) m1 == (EnumItem) m2 || m1 > m2;
    }

    /// <summary>
    /// Compares two milestones to determine if one occurs prior
    /// the other in the loan lifetime sequence.
    /// </summary>
    /// <param name="m1">The first Milestone to compare.</param>
    /// <param name="m2">The second Milestone to compare.</param>
    /// <returns>Returns true if <code>m1</code> occurs before <code>m2</code>
    /// in the loan lifetime sequence.</returns>
    /// <remarks>This operator will return true if either operand is null.</remarks>
    public static bool operator <(Milestone m1, Milestone m2) => !(m1 >= m2);

    /// <summary>
    /// Compares two milestones to determine if they are equal or if one occurs before
    /// the other in the loan lifetime sequence.
    /// </summary>
    /// <param name="m1">The first Milestone to compare.</param>
    /// <param name="m2">The second Milestone to compare.</param>
    /// <returns>Returns true if <code>m1</code> and <code>m2</code> are the same
    /// milestone or if <code>m1</code> occurs before <code>m2</code>
    /// in the loan lifetime sequence.</returns>
    public static bool operator <=(Milestone m1, Milestone m2) => !(m1 > m2);

    internal void SetNextMilestone(Milestone next) => this.next = next;

    internal void SetPreviousMilestone(Milestone prev) => this.prev = prev;

    internal string InternalName => this.internalName;

    private static string fixCapitalization(string name)
    {
      return name.Substring(0, 1).ToUpper() + name.Substring(1);
    }
  }
}
