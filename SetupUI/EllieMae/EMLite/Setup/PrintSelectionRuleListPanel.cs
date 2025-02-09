// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintSelectionRuleListPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintSelectionRuleListPanel : RuleListBase
  {
    private const string className = "PrintSelectionRuleListPanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private PrintSelectionBpmManager printSelectionRuleManager;
    private FieldSettings fieldSettings;

    public PrintSelectionRuleListPanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Print Auto Selection", allowMultiSelect)
    {
      this.printSelectionRuleManager = (PrintSelectionBpmManager) this.session.BPM.GetBpmManager(BpmCategory.PrintSelection);
      this.objectRuleManager = (object) this.printSelectionRuleManager;
      this.initForm();
      try
      {
        this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      }
      catch (Exception ex)
      {
        Tracing.Log(PrintSelectionRuleListPanel.sw, TraceLevel.Error, nameof (PrintSelectionRuleListPanel), "PreRequiredDialog: can't load custom fields. Error: " + ex.Message);
      }
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (PrintSelectionRuleDialog selectionRuleDialog = new PrintSelectionRuleDialog(this.session, (PrintSelectionRuleInfo) null, this.fieldSettings))
      {
        if (selectionRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          try
          {
            PrintSelectionRuleInfo newRule = (PrintSelectionRuleInfo) this.printSelectionRuleManager.CreateNewRule((BizRuleInfo) selectionRuleDialog.PrintSelectionRule);
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
      PrintSelectionRuleInfo tag = (PrintSelectionRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Milestone Requirement Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PrintSelectionRuleDialog selectionRuleDialog = new PrintSelectionRuleDialog(this.session, tag, this.fieldSettings))
        {
          if (selectionRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              PrintSelectionRuleInfo printSelectionRule = selectionRuleDialog.PrintSelectionRule;
              this.printSelectionRuleManager.UpdateRule((BizRuleInfo) printSelectionRule);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(printSelectionRule.Condition, printSelectionRule.Condition2, printSelectionRule.ConditionState, printSelectionRule.ConditionState2, printSelectionRule.RuleID);
              this.editViewItemOnList(printSelectionRule.RuleName, this.BuildChannelString((BizRuleInfo) printSelectionRule), this.BuildConditionString((BizRuleInfo) printSelectionRule), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) printSelectionRule);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Milestone Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          selectionRuleDialog.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      PrintSelectionRuleInfo tag = (PrintSelectionRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Milestone Requirement Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.printSelectionRuleManager.GetRules(tag.IsGeneralRule);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (PrintSelectionRuleDialog selectionRuleDialog = new PrintSelectionRuleDialog(this.session, (PrintSelectionRuleInfo) tag.Duplicate(duplicateName), this.fieldSettings))
        {
          int num2 = (int) selectionRuleDialog.ShowDialog((IWin32Window) this);
          if (selectionRuleDialog.DialogResult == DialogResult.OK)
          {
            try
            {
              PrintSelectionRuleInfo newRule = (PrintSelectionRuleInfo) this.printSelectionRuleManager.CreateNewRule((BizRuleInfo) selectionRuleDialog.PrintSelectionRule);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Print Auto Selection Business Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          selectionRuleDialog.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/printautoselection/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/printautoselection";

    protected override BizRuleType GetBizRuleType() => BizRuleType.PrintSelection;
  }
}
