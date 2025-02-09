// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanActionsAccessRulePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanActionsAccessRulePanel : RuleListBase
  {
    private const string className = "LoanActionsAccessRulePanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private LoanActionAccessBpmManager accessRuleManager;

    public LoanActionsAccessRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Loan Actions Access Rules", allowMultiSelect)
    {
      this.accessRuleManager = (LoanActionAccessBpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanActionAccess);
      this.objectRuleManager = (object) this.accessRuleManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (LoanActionAccessRuleDialog accessRuleDialog = new LoanActionAccessRuleDialog(this.session, (LoanActionAccessRuleInfo) null))
      {
        if (accessRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          try
          {
            LoanActionAccessRuleInfo newRule = (LoanActionAccessRuleInfo) this.accessRuleManager.CreateNewRule((BizRuleInfo) accessRuleDialog.RuleInfo);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new Loan Action Access Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        accessRuleDialog.Dispose();
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
      LoanActionAccessRuleInfo tag = (LoanActionAccessRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Loan Action Access Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (LoanActionAccessRuleDialog accessRuleDialog = new LoanActionAccessRuleDialog(this.session, (LoanActionAccessRuleInfo) tag.Clone()))
        {
          if (accessRuleDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          Cursor.Current = Cursors.WaitCursor;
          try
          {
            LoanActionAccessRuleInfo ruleInfo = accessRuleDialog.RuleInfo;
            this.accessRuleManager.UpdateRule((BizRuleInfo) ruleInfo);
            if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
              this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(ruleInfo.Condition, ruleInfo.Condition2, ruleInfo.ConditionState, ruleInfo.ConditionState2, ruleInfo.RuleID);
            this.editViewItemOnList(ruleInfo.RuleName, this.BuildChannelString((BizRuleInfo) ruleInfo), this.BuildConditionString((BizRuleInfo) ruleInfo), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) ruleInfo);
            this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The Loan Action Access Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          Cursor.Current = Cursors.Default;
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      LoanActionAccessRuleInfo tag = (LoanActionAccessRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Loan Action Access Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] allRules = this.accessRuleManager.GetAllRules();
        string duplicateName = this.FindDuplicateName(tag.RuleName, allRules);
        using (LoanActionAccessRuleDialog accessRuleDialog = new LoanActionAccessRuleDialog(this.session, (LoanActionAccessRuleInfo) tag.Duplicate(duplicateName)))
        {
          if (accessRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              LoanActionAccessRuleInfo newRule = (LoanActionAccessRuleInfo) this.accessRuleManager.CreateNewRule((BizRuleInfo) accessRuleDialog.RuleInfo);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The new Loan Action Access Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/PersonaAccessToLoanActions/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/PersonaAccessToLoanActions";

    protected override BizRuleType GetBizRuleType() => BizRuleType.LoanActionAccess;
  }
}
