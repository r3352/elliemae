// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.TemplateMilestones
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;
using System;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see>s that
  /// belong to a <see cref="T:EllieMae.Encompass.BusinessEnums.MilestoneTemplate">MilestoneTemplate</see> as configured for the Encompass system.
  /// </summary>
  /// <example>
  /// The following code lists the channels that the MilestoneTemplate apples to.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class MilestoneTemplateSample
  /// {
  ///   public static void Main()
  ///   {
  ///     // Open the session to the remote server
  ///     Session session = new Session();
  ///     session.Start("myserver", "mary", "maryspwd");
  /// 
  ///     // Open a loan using its GUID
  ///     Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  /// 
  ///     // Gets the MilestoneEvent for a each TemplateMilestone
  ///     foreach (TemplateMilestone tm in this.currentLoan.MilestoneTemplate.TemplateMilestones)
  ///     {
  ///         Milestone ms = session.Loans.Milestones.GetItemByID(tm.ID);
  ///         MilestoneEvent mEv = loan.Log.MilestoneEvents.GetEventForMilestone(ms.Name);
  ///         Console.WriteLine(string.Format("The Milestone {0} completed.", mEv.Completed ? "is" : "is not");
  ///     }
  /// 
  ///      // Close the loan
  ///      loan.Close();
  /// 
  ///      // End the session to gracefully disconnect from the server
  ///      session.End();
  /// 
  ///   }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class TemplateMilestones : EnumBase, ITemplateMilestones
  {
    private Session session;
    private EllieMae.EMLite.Workflow.MilestoneTemplate templateMilestone;

    internal TemplateMilestones(EllieMae.EMLite.Workflow.MilestoneTemplate templateMilestone, Session session)
    {
      this.session = session;
      this.templateMilestone = templateMilestone;
      for (int index = 0; index < this.templateMilestone.SequentialMilestones.Count(); ++index)
      {
        EllieMae.EMLite.Workflow.TemplateMilestone tm = this.templateMilestone.SequentialMilestones[index];
        EllieMae.EMLite.Workflow.Milestone milestone = this.session.SessionObjects.StartupInfo.Milestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID == tm.MilestoneID));
        int id = this.session.SessionObjects.StartupInfo.Milestones.IndexOf(milestone) + 1;
        this.AddItem((EnumItem) new TemplateMilestone(tm, id, milestone.Name, milestone.MilestoneID));
      }
      TemplateMilestone prev = (TemplateMilestone) null;
      for (int index = 0; index < this.Count; ++index)
      {
        TemplateMilestone next = this[index];
        if ((EnumItem) prev != (EnumItem) null)
        {
          next.SetPreviousTemplateMilestone(prev);
          prev.SetNextTemplateMilestone(next);
        }
        prev = next;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see> by it's index.
    /// </summary>
    /// <param name="index">The index of the <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see> in the list.</param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see></returns>
    public TemplateMilestone this[int index] => (TemplateMilestone) this.GetItem(index);

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see> by it's ID.
    /// </summary>
    /// <param name="itemId">The ID of the <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see></param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see></returns>
    public TemplateMilestone GetItemByID(int itemId)
    {
      return (TemplateMilestone) base.GetItemByID(itemId);
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see> by name.
    /// </summary>
    /// <param name="itemName">The name of the <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see></param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see></returns>
    public TemplateMilestone GetItemByName(string itemName)
    {
      return (TemplateMilestone) base.GetItemByName(itemName);
    }

    /// <summary>
    /// Returns the first TemplateMilestone in the sequence of TemplateMilestones for a MilestoneTemplate.
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
    public TemplateMilestone First => this[0];
  }
}
