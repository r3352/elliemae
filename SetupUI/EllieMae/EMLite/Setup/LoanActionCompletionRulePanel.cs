// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanActionCompletionRulePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanActionCompletionRulePanel : RuleListBase
  {
    private const string className = "LoanActionCompletionRulePanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private LoanActionCompletionRulesBpmManager loanActionCompletionRuleManager;
    internal string[] MilestoneList;
    internal Hashtable MSGUIDTable;
    internal Hashtable MSNameTable;
    internal ArrayList ArchivedMilestone;
    internal DocumentTrackingSetup DocsInforInSettings;
    internal Hashtable TasksInforInSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private IContainer components;

    public LoanActionCompletionRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Loan Action Requirements", allowMultiSelect)
    {
      this.InitializeComponent();
      this.loadSystemSettings();
      this.loanActionCompletionRuleManager = (LoanActionCompletionRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanActionCompletionRules);
      this.objectRuleManager = (object) this.loanActionCompletionRuleManager;
      this.initForm();
      this.HideImportExport();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void loadSystemSettings()
    {
      this.MSGUIDTable = new Hashtable();
      this.MSNameTable = new Hashtable();
      this.ArchivedMilestone = new ArrayList();
      for (int index = 0; index < this.msList.Count; ++index)
      {
        string milestoneId = this.msList[index].MilestoneID;
        string name = this.msList[index].Name;
        if (!this.MSGUIDTable.ContainsKey((object) name))
          this.MSGUIDTable.Add((object) name, (object) milestoneId);
        if (!this.MSNameTable.ContainsKey((object) milestoneId))
          this.MSNameTable.Add((object) milestoneId, (object) name);
        if (this.msList[index].Archived)
          this.ArchivedMilestone.Add((object) name);
      }
      try
      {
        this.DocsInforInSettings = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanActionCompletionRulePanel.sw, TraceLevel.Error, nameof (LoanActionCompletionRulePanel), "loadSystemSettings: can't load document tracking settings. Error: " + ex.Message);
      }
      try
      {
        MilestoneTaskDefinition[] milestoneTasks = this.session.ConfigurationManager.GetMilestoneTasks((string[]) null);
        if (milestoneTasks == null)
          return;
        foreach (MilestoneTaskDefinition milestoneTaskDefinition in milestoneTasks)
        {
          if (!this.TasksInforInSettings.ContainsKey((object) milestoneTaskDefinition.TaskGUID))
            this.TasksInforInSettings.Add((object) milestoneTaskDefinition.TaskGUID, (object) milestoneTaskDefinition);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanActionCompletionRulePanel.sw, TraceLevel.Error, nameof (LoanActionCompletionRulePanel), "loadSystemSettings: can't load milestone task settings. Error: " + ex.Message);
      }
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (LoanActionCompletionRuleDialog completionRuleDialog = new LoanActionCompletionRuleDialog(this.session, (LoanActionCompletionRuleInfo) null, this))
      {
        if (completionRuleDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          try
          {
            LoanActionCompletionRuleInfo newRule = (LoanActionCompletionRuleInfo) this.loanActionCompletionRuleManager.CreateNewRule((BizRuleInfo) completionRuleDialog.LoanActionCompletionRule);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new Loan Action Completion Business Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
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

    protected override void ctxMenuLstView1_Popup(object sender, EventArgs e)
    {
      base.ctxMenuLstView1_Popup(sender, e);
      this.menuItemExport.Visible = this.menuItemImport.Visible = false;
    }

    private void editSelectedItem()
    {
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      LoanActionCompletionRuleInfo tag = (LoanActionCompletionRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Loan Action Completion Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (LoanActionCompletionRuleDialog completionRuleDialog = new LoanActionCompletionRuleDialog(this.session, (LoanActionCompletionRuleInfo) tag.Clone(), this))
        {
          if (completionRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              LoanActionCompletionRuleInfo actionCompletionRule = completionRuleDialog.LoanActionCompletionRule;
              this.loanActionCompletionRuleManager.UpdateRule((BizRuleInfo) actionCompletionRule);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(actionCompletionRule.Condition, actionCompletionRule.Condition2, actionCompletionRule.ConditionState, actionCompletionRule.ConditionState2, actionCompletionRule.RuleID);
              this.editViewItemOnList(actionCompletionRule.RuleName, this.BuildChannelString((BizRuleInfo) actionCompletionRule), this.BuildConditionString((BizRuleInfo) actionCompletionRule), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) actionCompletionRule);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Loan Action Completion Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          completionRuleDialog.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      LoanActionCompletionRuleInfo tag = (LoanActionCompletionRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Loan Action Completion Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.loanActionCompletionRuleManager.GetRules(tag.IsGeneralRule);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (LoanActionCompletionRuleDialog completionRuleDialog = new LoanActionCompletionRuleDialog(this.session, (LoanActionCompletionRuleInfo) tag.Duplicate(duplicateName), this))
        {
          int num2 = (int) completionRuleDialog.ShowDialog((IWin32Window) this);
          if (completionRuleDialog.DialogResult == DialogResult.OK)
          {
            try
            {
              LoanActionCompletionRuleInfo newRule = (LoanActionCompletionRuleInfo) this.loanActionCompletionRuleManager.CreateNewRule((BizRuleInfo) completionRuleDialog.LoanActionCompletionRule);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Loan Action Completion Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          completionRuleDialog.Dispose();
        }
        this.setHeader();
      }
    }

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
