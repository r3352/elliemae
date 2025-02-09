// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.Milestones
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The Milestones class represents the set of all events (milestones) that can occur during the
  /// lifetime of a loan.
  /// </summary>
  /// <remarks>
  /// During the lifetime of a loan, the following milestones are achieved by loans
  /// in this order by default:
  /// <list type="bullet">
  /// <item>Started</item>
  /// <item>Processing</item>
  /// <item>Submittal</item>
  /// <item>Approval</item>
  /// <item>Docs Signing</item>
  /// <item>Funding</item>
  /// <item>Completion</item>
  /// </list>
  /// The values above are the milestones that exist by default and are common to every loan unless renamed or
  /// reordered. Additionally, every Encompass system can define its own set of custom milestones, additional
  /// stages in the lifetime sequence that are unique to the company's business process.
  /// <p>The milestones defined for any given loan are determined when the loan is first created.
  /// At that point, whatever milestones are defined in the source
  /// Encompass system determined the lifetime sequence for that loan unless a MilestoneTemplate is applied to the loan.
  /// Thus, it is possible to have loans with different milestone sequences</p>
  /// <p>The values above represent the names of the default milestones and can be used
  /// with the <see cref="M:EllieMae.Encompass.BusinessEnums.Milestones.GetItemByName(System.String)">GetItemByName()</see> function to
  /// retrieve a particular milestone. Alternatively, you may use the <see cref="P:EllieMae.Encompass.BusinessEnums.Milestones.First">First</see>
  /// property to obtain a reference to the first milestone (Started) and then use the
  /// <see cref="P:EllieMae.Encompass.BusinessEnums.Milestone.Next">Next</see> and <see cref="P:EllieMae.Encompass.BusinessEnums.Milestone.Previous">Previous</see>
  /// properties to traverse the non-archived milestone sequence in order. However, keep in mind that the set of milestones
  /// currently defined in the system may be different than those which are defined for any particular loan.</p>
  /// <para>
  /// When iterating over the entire Milestons collection the archived Milestones will be included and the end of the list.
  /// </para>
  /// </remarks>
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
  public class Milestones : EnumBase, IMilestones
  {
    private Session session;
    private Hashtable coreMilestones = new Hashtable();
    private Milestone started;
    private Milestone processing;
    private Milestone submittal;
    private Milestone approval;
    private Milestone docSigning;
    private Milestone funding;
    private Milestone completion;

    internal Milestones(Session session)
    {
      this.session = session;
      IConfigurationManager configurationManager = (IConfigurationManager) session.GetObject("ConfigurationManager");
      List<EllieMae.EMLite.Workflow.Milestone> milestones = this.session.SessionObjects.StartupInfo.Milestones;
      for (int index = 0; index <= milestones.Count - 1; ++index)
      {
        Milestone milestone = new Milestone(index + 1, milestones[index].Name, milestones[index].Archived, milestones[index].MilestoneID);
        this.AddItem((EnumItem) milestone);
        this.coreMilestones.Add((object) milestone.InternalName, (object) milestone);
      }
      Milestone prev = (Milestone) null;
      for (int index = 0; index < this.Count; ++index)
      {
        Milestone next = this[index];
        if (!next.IsArchived)
        {
          if ((EnumItem) prev != (EnumItem) null)
          {
            next.SetPreviousMilestone(prev);
            prev.SetNextMilestone(next);
          }
          prev = next;
        }
      }
      this.InitCoreMilestones();
    }

    private void InitCoreMilestones()
    {
      foreach (Milestone milestone in (EnumBase) this)
      {
        switch (milestone.Name.ToUpper())
        {
          case "APPROVAL":
            this.approval = milestone;
            continue;
          case "COMPLETION":
            this.completion = milestone;
            continue;
          case "DOCS SIGNING":
            this.docSigning = milestone;
            continue;
          case "FUNDING":
            this.funding = milestone;
            continue;
          case "PROCESSING":
            this.processing = milestone;
            continue;
          case "STARTED":
            this.started = milestone;
            continue;
          case "SUBMITTAL":
            this.submittal = milestone;
            continue;
          default:
            continue;
        }
      }
    }

    /// <summary>
    /// Returns the first Milestone in the sequence of Milestones that loans pass through.
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
    public Milestone First => this[0];

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> with the specified index.</summary>
    public Milestone this[int index] => (Milestone) this.GetItem(index);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> with the specified index.</summary>
    public Milestone GetItemByID(int itemId) => (Milestone) base.GetItemByID(itemId);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> with the specified name.</summary>
    /// <param name="name">The name of the item being retrieved (case insensitive).</param>
    public Milestone GetItemByName(string name) => (Milestone) base.GetItemByName(name);

    /// <summary>
    /// Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> by the MilestoneID
    /// </summary>
    /// <param name="milestoneID"></param>
    /// <returns></returns>
    public Milestone GetItemByMilestoneID(string milestoneID)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (this[index].MilestoneID == milestoneID)
          return this[index];
      }
      return (Milestone) null;
    }

    /// <summary>Gets the Started milestone.</summary>
    public Milestone Started
    {
      get
      {
        return !((EnumItem) this.started == (EnumItem) null) ? this.started : throw new Exception("Started Milestone not found.");
      }
    }

    /// <summary>
    /// Gets the Processing milestone.
    /// <exception cref="T:System.Exception">Thrown when the Processing <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see>
    /// is not found due to being renamed</exception>
    /// </summary>
    public Milestone Processing
    {
      get
      {
        return !((EnumItem) this.processing == (EnumItem) null) ? this.processing : throw new Exception("Processing Milestone not found.");
      }
    }

    /// <summary>
    /// Gets the Submittal milestone.
    /// <exception cref="T:System.Exception">Thrown when the Submittal <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see>
    /// is not found due to being renamed</exception>
    /// </summary>
    public Milestone Submittal
    {
      get
      {
        return !((EnumItem) this.submittal == (EnumItem) null) ? this.submittal : throw new Exception("Submittal Milestone not found.");
      }
    }

    /// <summary>
    /// Gets the Approval milestone.
    /// <exception cref="T:System.Exception">Thrown when the Approval <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see>
    /// is not found due to being renamed</exception>
    /// </summary>
    public Milestone Approval
    {
      get
      {
        return !((EnumItem) this.approval == (EnumItem) null) ? this.approval : throw new Exception("Approval Milestone not found.");
      }
    }

    /// <summary>
    /// Gets the Docs Signing milestone.
    /// <exception cref="T:System.Exception">Thrown when the Docs Signing <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see>
    /// is not found due to being renamed.</exception>
    /// </summary>
    public Milestone DocsSigning
    {
      get
      {
        return !((EnumItem) this.docSigning == (EnumItem) null) ? this.docSigning : throw new Exception("Docs Signing Milestone not found.");
      }
    }

    /// <summary>
    /// Gets the Funding milestone.
    /// <exception cref="T:System.Exception">Thrown when the Funding <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see>
    /// is not found due to being renamed.</exception>
    /// </summary>
    public Milestone Funding
    {
      get
      {
        return !((EnumItem) this.funding == (EnumItem) null) ? this.funding : throw new Exception("Funding Milestone not found.");
      }
    }

    /// <summary>Gets the Completion milestone.</summary>
    public Milestone Completion
    {
      get
      {
        return !((EnumItem) this.completion == (EnumItem) null) ? this.completion : throw new Exception("Completion Milestone not found.");
      }
    }
  }
}
