// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanAccessRulePanel
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
  public class LoanAccessRulePanel : RuleListBase
  {
    private const string className = "LoanAccessRulePanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private LoanAccessBpmManager accessRuleManager;

    public LoanAccessRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Loan Access Rules", allowMultiSelect)
    {
      this.accessRuleManager = (LoanAccessBpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanAccess);
      this.objectRuleManager = (object) this.accessRuleManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (LoanAccessRuleDialog accessRuleDialog = new LoanAccessRuleDialog(this.session, (LoanAccessRuleInfo) null))
      {
        if (accessRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          try
          {
            LoanAccessRuleInfo newRule = (LoanAccessRuleInfo) this.accessRuleManager.CreateNewRule((BizRuleInfo) accessRuleDialog.RuleInfo);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new Field Access Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
      LoanAccessRuleInfo tag = (LoanAccessRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Field Access Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        tag.GetLoanAccessRights();
        using (LoanAccessRuleDialog accessRuleDialog = new LoanAccessRuleDialog(this.session, (LoanAccessRuleInfo) tag.Clone()))
        {
          if (accessRuleDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          try
          {
            LoanAccessRuleInfo ruleInfo = accessRuleDialog.RuleInfo;
            this.accessRuleManager.UpdateRule((BizRuleInfo) ruleInfo);
            if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
              this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(ruleInfo.Condition, ruleInfo.Condition2, ruleInfo.ConditionState, ruleInfo.ConditionState2, ruleInfo.RuleID);
            this.editViewItemOnList(ruleInfo.RuleName, this.BuildChannelString((BizRuleInfo) ruleInfo), this.BuildConditionString((BizRuleInfo) ruleInfo), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) ruleInfo);
            this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The Field Access Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      LoanAccessRuleInfo tag = (LoanAccessRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Field Access Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] allRules = this.accessRuleManager.GetAllRules(true);
        string duplicateName = this.FindDuplicateName(tag.RuleName, allRules);
        using (LoanAccessRuleDialog accessRuleDialog = new LoanAccessRuleDialog(this.session, (LoanAccessRuleInfo) tag.Duplicate(duplicateName)))
        {
          int num2 = (int) accessRuleDialog.ShowDialog((IWin32Window) this);
          if (accessRuleDialog.DialogResult == DialogResult.OK)
          {
            try
            {
              LoanAccessRuleInfo newRule = (LoanAccessRuleInfo) this.accessRuleManager.CreateNewRule((BizRuleInfo) accessRuleDialog.RuleInfo);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Field Access Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          accessRuleDialog.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/personaaccesstoloans/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/personaaccesstoloans";

    protected override BizRuleType GetBizRuleType() => BizRuleType.LoanAccess;
  }
}
