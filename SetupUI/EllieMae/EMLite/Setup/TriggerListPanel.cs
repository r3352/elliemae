// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerListPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerListPanel : RuleListBase
  {
    private const string className = "TriggerListPanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private TriggersBpmManager triggerManager;

    public TriggerListPanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Triggers", allowMultiSelect)
    {
      this.triggerManager = (TriggersBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Triggers);
      this.objectRuleManager = (object) this.triggerManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (TriggerEditor triggerEditor = new TriggerEditor((TriggerInfo) null, this.session))
      {
        int num1 = (int) triggerEditor.ShowDialog((IWin32Window) this);
        if (triggerEditor.DialogResult == DialogResult.OK)
        {
          try
          {
            TriggerInfo newRule = (TriggerInfo) this.triggerManager.CreateNewRule((BizRuleInfo) triggerEditor.Trigger);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The new Trigger cannot be saved. Error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        triggerEditor.Dispose();
      }
      this.setHeader();
    }

    protected override void editBtn_Click(object sender, EventArgs e)
    {
      base.editBtn_Click(sender, e);
      this.editSelectedItem();
    }

    protected override void listViewRule_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      base.listViewRule_ItemDoubleClick(source, e);
      this.editSelectedItem();
    }

    public string[] SelectedMilestones
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.listViewRule.SelectedItems)
        {
          TriggerInfo tag = (TriggerInfo) selectedItem.Tag;
          if (tag != null)
          {
            for (int index = 0; index < tag.Events.Count; ++index)
            {
              TriggerCondition condition = tag.Events[index].Conditions[0];
              if (condition is TriggerMilestoneCompletionCondition)
              {
                string mid = ((TriggerMilestoneCompletionCondition) condition).MilestoneID;
                EllieMae.EMLite.Workflow.Milestone milestone = this.msList.Find((Predicate<EllieMae.EMLite.Workflow.Milestone>) (ms => ms.MilestoneID == mid));
                stringList.Add(string.Format("{0},{1},{2},{3}", (object) mid, milestone != null ? (object) milestone.Name : (object) "", (object) index, (object) tag.RuleName));
              }
            }
          }
        }
        return stringList.ToArray();
      }
    }

    private void editSelectedItem()
    {
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      TriggerInfo tag = (TriggerInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Trigger cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (TriggerEditor triggerEditor = new TriggerEditor((TriggerInfo) tag.Clone(), this.session))
        {
          if (triggerEditor.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              TriggerInfo trigger = triggerEditor.Trigger;
              this.triggerManager.UpdateRule((BizRuleInfo) trigger);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(trigger.Condition, trigger.Condition2, trigger.ConditionState, trigger.ConditionState2, trigger.RuleID);
              this.editViewItemOnList(trigger.RuleName, this.BuildChannelString((BizRuleInfo) trigger), this.BuildConditionString((BizRuleInfo) trigger), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) trigger);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Field Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          triggerEditor.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      TriggerInfo tag = (TriggerInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Field Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.triggerManager.GetRules(tag.IsGeneralRule, true);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (TriggerEditor triggerEditor = new TriggerEditor((TriggerInfo) tag.Duplicate(duplicateName), this.session))
        {
          int num2 = (int) triggerEditor.ShowDialog((IWin32Window) this);
          if (triggerEditor.DialogResult == DialogResult.OK)
          {
            try
            {
              TriggerInfo newRule = (TriggerInfo) this.triggerManager.CreateNewRule((BizRuleInfo) triggerEditor.Trigger);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Field Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          triggerEditor.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/fieldtriggers/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/fieldtriggers";

    protected override BizRuleType GetBizRuleType() => BizRuleType.Triggers;
  }
}
