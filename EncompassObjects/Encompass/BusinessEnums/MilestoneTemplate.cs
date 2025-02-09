// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.MilestoneTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// Represents a milestone template, which is a sequence of milestones which can be applied
  /// to a loan.
  /// </summary>
  public class MilestoneTemplate : EnumItem, IMilestoneTemplate
  {
    private EllieMae.EMLite.Workflow.MilestoneTemplate milestoneTemplate;
    private TemplateMilestones templateMilestones;
    private Session session;
    private List<TemplateChannel> channels;
    private TemplateCondition condition;

    internal MilestoneTemplate(Loan loan)
      : this(loan.LoanData.GetLogList().MilestoneTemplate, loan.Session)
    {
    }

    internal MilestoneTemplate(EllieMae.EMLite.Workflow.MilestoneTemplate milestoneTemplate, Session session)
      : base(int.Parse(milestoneTemplate.TemplateID), milestoneTemplate.Name)
    {
      this.milestoneTemplate = milestoneTemplate;
      this.session = session;
    }

    /// <summary>Gets the sort index of the template.</summary>
    public int SortIndex
    {
      get
      {
        return this.session.Loans.MilestoneTemplates.GetOrderByIndex(this.milestoneTemplate.TemplateID);
      }
    }

    /// <summary>Gets the state of the template.</summary>
    public bool Active
    {
      get
      {
        EllieMae.EMLite.Workflow.MilestoneTemplate milestoneTemplate = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(false).FirstOrDefault<EllieMae.EMLite.Workflow.MilestoneTemplate>((Func<EllieMae.EMLite.Workflow.MilestoneTemplate, bool>) (mt => mt.TemplateID == this.milestoneTemplate.TemplateID));
        return milestoneTemplate != null && milestoneTemplate.Active;
      }
    }

    /// <summary>
    /// Gets the list of all <see cref="T:EllieMae.Encompass.BusinessEnums.TemplateMilestone">TemplateMilestone</see> for the MilestoneTemplate
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
    public TemplateMilestones TemplateMilestones
    {
      get
      {
        if (this.templateMilestones == null)
          this.templateMilestones = new TemplateMilestones(this.milestoneTemplate, this.session);
        return this.templateMilestones;
      }
    }

    /// <summary>
    /// Get the loan conditions used to determine if the MilestoneTemplate matches the loan data.
    /// </summary>
    /// <example>
    /// The following code lists the details of the loan condition
    /// that is used to apply a MilestoneTemplate.
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
    ///     // Get the Loan's current MilestoneTemplate
    ///     MilestoneTemplate mt = loan.MilestoneTemplate;
    /// 
    ///     // Print the Condition details
    ///     switch (mt.Condition.ConditionType)
    ///     {
    ///         case TemplateConditionType.None:
    ///             Console.WriteLine("The MilestoneTemplate has no condition.");
    ///             break;
    ///         case TemplateConditionType.LoanPurpose:
    ///         case TemplateConditionType.LoanType:
    ///              Console.WriteLine(string.Format("The condition is {0} = {1}.", mt.Condition.ConditionType.ToString(), mt.Condition.Condition));
    ///              break;
    ///         case TemplateConditionType.AdvancedCondition:
    ///               Console.WriteLine("The advanced condition is " + mt.Condition.AdvancedCondition);
    ///               break;
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
    public TemplateCondition Condition
    {
      get
      {
        if (this.condition == null)
          this.condition = new TemplateCondition(this.session.SessionObjects.StartupInfo.MilestoneTemplate.FirstOrDefault<FieldRuleInfo>((Func<FieldRuleInfo, bool>) (fr => fr.RuleName == this.milestoneTemplate.Name)));
        return this.condition;
      }
    }

    /// <summary>
    /// Returns the channels that the MilestoneTemplate applies to.
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
    /// 
    /// class MilestoneTemplateSample
    /// {
    ///   public static void Main()
    ///   {
    ///     // Open the session to the remote server
    ///     Session session = new Session();
    ///     session.Start("myserver", "mary", "maryspwd");
    /// 
    ///     // Get a MilestoneTemplate
    ///     MilestoneTemplate mt = session.Loans.MilestoneTemplates.GetItemByName("Test Template");
    /// 
    ///     // Print out all the channels for the MilestoneTemplate
    ///     foreach (TemplateChannel tc in mt.Channels)
    ///       Console.WriteLine("MilestoneTemplate applies to channel: " + tc.ToString());
    /// 
    ///      // End the session to gracefully disconnect from the server
    ///      session.End();
    /// 
    ///   }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public IEnumerable<TemplateChannel> Channels
    {
      get
      {
        if (this.channels == null)
        {
          string[] strArray = this.session.SessionObjects.StartupInfo.MilestoneTemplate.FirstOrDefault<FieldRuleInfo>((Func<FieldRuleInfo, bool>) (fr => fr.RuleName == this.milestoneTemplate.Name)).Condition2.Split(new string[1]
          {
            ","
          }, StringSplitOptions.RemoveEmptyEntries);
          this.channels = new List<TemplateChannel>();
          foreach (string s in strArray)
            this.channels.Add((TemplateChannel) short.Parse(s));
        }
        return (IEnumerable<TemplateChannel>) this.channels;
      }
    }
  }
}
