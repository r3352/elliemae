// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutomatedConditionsListPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutomatedConditionsListPanel : RuleListBase
  {
    private const string className = "AutomatedConditionsListPanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private AutomatedConditionBpmManager automatedConditionManager;
    private IContainer components;

    public string[] SelectedMilestones
    {
      get
      {
        return this.listViewRule.SelectedItems.Count == 0 ? (string[]) null : this.automatedConditionManager.GetMilestoneNamesByRuleIds(this.listViewRule.SelectedItems.Select<GVItem, int>((Func<GVItem, int>) (item => ((BizRuleInfo) item.Tag).RuleID)).ToArray<int>());
      }
    }

    public string[] SelectedConditionRules
    {
      get
      {
        return this.listViewRule.SelectedItems.Count == 0 ? (string[]) null : this.listViewRule.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        foreach (GVItem gvItem in this.listViewRule.Items.Where<GVItem>((Func<GVItem, bool>) (item => ((IEnumerable<string>) value).Contains<string>(item.SubItems[0].Text))))
          gvItem.Selected = true;
      }
    }

    public AutomatedConditionsListPanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Automated Conditions", allowMultiSelect)
    {
      this.InitializeComponent();
      this.automatedConditionManager = (AutomatedConditionBpmManager) this.session.BPM.GetBpmManager(BpmCategory.AutomatedConditions);
      this.objectRuleManager = (object) this.automatedConditionManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (AutomatedConditionEditor automatedConditionEditor = new AutomatedConditionEditor((AutomatedConditionRuleInfo) null, this.session))
      {
        int num1 = (int) automatedConditionEditor.ShowDialog((IWin32Window) this);
        if (automatedConditionEditor.DialogResult == DialogResult.OK)
        {
          try
          {
            AutomatedConditionRuleInfo newRule = (AutomatedConditionRuleInfo) this.automatedConditionManager.CreateNewRule((BizRuleInfo) automatedConditionEditor.AutomatedCondition);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The new Automated Conditions cannot be saved. Error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        automatedConditionEditor.Dispose();
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

    private void editSelectedItem()
    {
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      AutomatedConditionRuleInfo tag = (AutomatedConditionRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Automated Conditions cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (AutomatedConditionEditor automatedConditionEditor = new AutomatedConditionEditor((AutomatedConditionRuleInfo) tag.Clone(), this.session))
        {
          if (automatedConditionEditor.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              AutomatedConditionRuleInfo automatedCondition = automatedConditionEditor.AutomatedCondition;
              this.automatedConditionManager.UpdateRule((BizRuleInfo) automatedCondition);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(automatedCondition.Condition, automatedCondition.Condition2, automatedCondition.ConditionState, automatedCondition.ConditionState2, automatedCondition.RuleID);
              this.editViewItemOnList(automatedCondition.RuleName, this.BuildChannelString((BizRuleInfo) automatedCondition), this.BuildConditionString((BizRuleInfo) automatedCondition), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) automatedCondition);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Automated Conditions can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          automatedConditionEditor.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      AutomatedConditionRuleInfo tag = (AutomatedConditionRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Automated Conditions can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.automatedConditionManager.GetRules(tag.IsGeneralRule);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (AutomatedConditionEditor automatedConditionEditor = new AutomatedConditionEditor((AutomatedConditionRuleInfo) tag.Duplicate(duplicateName), this.session))
        {
          int num2 = (int) automatedConditionEditor.ShowDialog((IWin32Window) this);
          if (automatedConditionEditor.DialogResult == DialogResult.OK)
          {
            try
            {
              AutomatedConditionRuleInfo newRule = (AutomatedConditionRuleInfo) this.automatedConditionManager.CreateNewRule((BizRuleInfo) automatedConditionEditor.AutomatedCondition);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Automated Conditions can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          automatedConditionEditor.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/automatedconditions/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/automatedconditions";

    protected override BizRuleType GetBizRuleType() => BizRuleType.AutomatedConditions;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private new void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
