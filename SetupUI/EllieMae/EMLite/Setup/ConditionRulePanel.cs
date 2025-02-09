// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionRulePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionRulePanel : RuleListBase
  {
    private const string className = "ConditionRulePanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private MilestoneRulesBpmManager milestoneRuleManager;
    internal string[] MilestoneList;
    internal Hashtable MSGUIDTable;
    internal Hashtable MSNameTable;
    internal ArrayList ArchiveMilestone;
    internal DocumentTrackingSetup DocsInforInSettings;
    internal Hashtable TasksInforInSettings = CollectionsUtil.CreateCaseInsensitiveHashtable();
    internal Lazy<Hashtable> WorkflowTaskGroupTemplatesInfoInSettings;
    internal Lazy<Hashtable> WorkflowTaskTemplatesInfoInSettings;

    internal bool IsWorkflowTaskAccessible { get; set; }

    public ConditionRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Milestone Requirements", allowMultiSelect)
    {
      this.Dock = DockStyle.Fill;
      this.loadSystemSettings();
      this.milestoneRuleManager = (MilestoneRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.MilestoneRules);
      this.objectRuleManager = (object) this.milestoneRuleManager;
      this.IsWorkflowTaskAccessible = this.session.EncompassEdition == EncompassEdition.Banker && Utils.ParseBoolean(this.session.ServerManager.GetServerSetting("FEATURE.ENABLEWORKFLOWTASKS"));
      this.WorkflowTaskGroupTemplatesInfoInSettings = new Lazy<Hashtable>((Func<Hashtable>) (() => session.SessionObjects.CachedWorkflowTaskGroupTemplates));
      this.WorkflowTaskTemplatesInfoInSettings = new Lazy<Hashtable>((Func<Hashtable>) (() => session.SessionObjects.CachedWorkflowTaskTemplates));
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void loadSystemSettings()
    {
      bool flag = false;
      ArrayList arrayList = new ArrayList();
      for (int index = 1; index < this.msList.Count - 1; ++index)
      {
        if (this.msList[index].Name.Equals("Completion"))
          flag = true;
        arrayList.Add((object) this.msList[index].Name);
      }
      if (!flag)
        arrayList.Add((object) "Completion");
      this.MilestoneList = (string[]) arrayList.ToArray(typeof (string));
      this.MSGUIDTable = new Hashtable();
      this.MSNameTable = new Hashtable();
      this.ArchiveMilestone = new ArrayList();
      for (int index = 0; index < this.msList.Count; ++index)
      {
        string milestoneId = this.msList[index].MilestoneID;
        string name = this.msList[index].Name;
        if (!this.MSGUIDTable.ContainsKey((object) name))
          this.MSGUIDTable.Add((object) name, (object) milestoneId);
        if (!this.MSNameTable.ContainsKey((object) milestoneId))
          this.MSNameTable.Add((object) milestoneId, (object) name);
        if (this.msList[index].Archived)
          this.ArchiveMilestone.Add((object) name);
      }
      try
      {
        this.DocsInforInSettings = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      }
      catch (Exception ex)
      {
        Tracing.Log(ConditionRulePanel.sw, TraceLevel.Error, nameof (ConditionRulePanel), "loadSystemSettings: can't load document tracking settings. Error: " + ex.Message);
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
        Tracing.Log(ConditionRulePanel.sw, TraceLevel.Error, nameof (ConditionRulePanel), "loadSystemSettings: can't load milestone tasks settings. Error: " + ex.Message);
      }
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (ConditionRuleDialog conditionRuleDialog = new ConditionRuleDialog(this.session, (MilestoneRuleInfo) null, this))
      {
        if (conditionRuleDialog.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
        {
          try
          {
            MilestoneRuleInfo newRule = (MilestoneRuleInfo) this.milestoneRuleManager.CreateNewRule((BizRuleInfo) conditionRuleDialog.MilestoneRule);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new Milestone Business Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

    private void editSelectedItem()
    {
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      MilestoneRuleInfo tag = (MilestoneRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Milestone Requirement Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (ConditionRuleDialog conditionRuleDialog = new ConditionRuleDialog(this.session, (MilestoneRuleInfo) tag.Clone(), this))
        {
          if (conditionRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              MilestoneRuleInfo milestoneRule = conditionRuleDialog.MilestoneRule;
              this.milestoneRuleManager.UpdateRule((BizRuleInfo) milestoneRule);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(milestoneRule.Condition, milestoneRule.Condition2, milestoneRule.ConditionState, milestoneRule.ConditionState2, milestoneRule.RuleID);
              this.editViewItemOnList(milestoneRule.RuleName, this.BuildChannelString((BizRuleInfo) milestoneRule), this.BuildConditionString((BizRuleInfo) milestoneRule), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) milestoneRule);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Milestone Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          conditionRuleDialog.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      MilestoneRuleInfo tag = (MilestoneRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Milestone Requirement Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.milestoneRuleManager.GetRules(tag.IsGeneralRule);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (ConditionRuleDialog conditionRuleDialog = new ConditionRuleDialog(this.session, (MilestoneRuleInfo) tag.Duplicate(duplicateName), this))
        {
          int num2 = (int) conditionRuleDialog.ShowDialog((IWin32Window) this);
          if (conditionRuleDialog.DialogResult == DialogResult.OK)
          {
            try
            {
              MilestoneRuleInfo newRule = (MilestoneRuleInfo) this.milestoneRuleManager.CreateNewRule((BizRuleInfo) conditionRuleDialog.MilestoneRule);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Milestone Business Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          conditionRuleDialog.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/milestonecompletions/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/milestonecompletions";

    protected override BizRuleType GetBizRuleType() => BizRuleType.MilestoneRules;
  }
}
