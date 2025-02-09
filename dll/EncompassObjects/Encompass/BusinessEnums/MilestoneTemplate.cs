// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.MilestoneTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class MilestoneTemplate : EnumItem, IMilestoneTemplate
  {
    private MilestoneTemplate milestoneTemplate;
    private TemplateMilestones templateMilestones;
    private Session session;
    private List<TemplateChannel> channels;
    private TemplateCondition condition;

    internal MilestoneTemplate(Loan loan)
      : this(loan.LoanData.GetLogList().MilestoneTemplate, loan.Session)
    {
    }

    internal MilestoneTemplate(MilestoneTemplate milestoneTemplate, Session session)
      : base(int.Parse(milestoneTemplate.TemplateID), milestoneTemplate.Name)
    {
      this.milestoneTemplate = milestoneTemplate;
      this.session = session;
    }

    public int SortIndex
    {
      get
      {
        return this.session.Loans.MilestoneTemplates.GetOrderByIndex(this.milestoneTemplate.TemplateID);
      }
    }

    public bool Active
    {
      get
      {
        MilestoneTemplate milestoneTemplate = this.session.SessionObjects.BpmManager.GetMilestoneTemplates(false).FirstOrDefault<MilestoneTemplate>((Func<MilestoneTemplate, bool>) (mt => mt.TemplateID == this.milestoneTemplate.TemplateID));
        return milestoneTemplate != null && milestoneTemplate.Active;
      }
    }

    public TemplateMilestones TemplateMilestones
    {
      get
      {
        if (this.templateMilestones == null)
          this.templateMilestones = new TemplateMilestones(this.milestoneTemplate, this.session);
        return this.templateMilestones;
      }
    }

    public TemplateCondition Condition
    {
      get
      {
        if (this.condition == null)
          this.condition = new TemplateCondition(this.session.SessionObjects.StartupInfo.MilestoneTemplate.FirstOrDefault<FieldRuleInfo>((Func<FieldRuleInfo, bool>) (fr => ((BizRuleInfo) fr).RuleName == this.milestoneTemplate.Name)));
        return this.condition;
      }
    }

    public IEnumerable<TemplateChannel> Channels
    {
      get
      {
        if (this.channels == null)
        {
          string[] strArray = ((BizRuleInfo) this.session.SessionObjects.StartupInfo.MilestoneTemplate.FirstOrDefault<FieldRuleInfo>((Func<FieldRuleInfo, bool>) (fr => ((BizRuleInfo) fr).RuleName == this.milestoneTemplate.Name))).Condition2.Split(new string[1]
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
