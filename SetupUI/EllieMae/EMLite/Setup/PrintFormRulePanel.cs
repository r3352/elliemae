// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintFormRulePanel
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
  public class PrintFormRulePanel : RuleListBase
  {
    private PrintFormsBpmManager printFormRuleManager;

    public PrintFormRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Loan Form Printing", allowMultiSelect)
    {
      this.printFormRuleManager = (PrintFormsBpmManager) this.session.BPM.GetBpmManager(BpmCategory.PrintForms);
      this.objectRuleManager = (object) this.printFormRuleManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (PrintFormRuleDialog printFormRuleDialog = new PrintFormRuleDialog(this.session, (PrintFormRuleInfo) null))
      {
        if (printFormRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          try
          {
            PrintFormRuleInfo newRule = (PrintFormRuleInfo) this.printFormRuleManager.CreateNewRule((BizRuleInfo) printFormRuleDialog.PrintRuleInfo);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new Print Suppression Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
      PrintFormRuleInfo tag = (PrintFormRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Print Suppression Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PrintFormRuleDialog printFormRuleDialog = new PrintFormRuleDialog(this.session, (PrintFormRuleInfo) tag.Clone()))
        {
          if (printFormRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              PrintFormRuleInfo printRuleInfo = printFormRuleDialog.PrintRuleInfo;
              this.printFormRuleManager.UpdateRule((BizRuleInfo) printRuleInfo);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(printRuleInfo.Condition, printRuleInfo.Condition2, printRuleInfo.ConditionState, printRuleInfo.ConditionState2, printRuleInfo.RuleID);
              this.editViewItemOnList(printRuleInfo.RuleName, this.BuildChannelString((BizRuleInfo) printRuleInfo), this.BuildConditionString((BizRuleInfo) printRuleInfo), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) printRuleInfo);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Print Suppression Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          printFormRuleDialog.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      PrintFormRuleInfo tag = (PrintFormRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Print Suppression Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.printFormRuleManager.GetRules(tag.IsGeneralRule);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (PrintFormRuleDialog printFormRuleDialog = new PrintFormRuleDialog(this.session, (PrintFormRuleInfo) tag.Duplicate(duplicateName)))
        {
          if (printFormRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              PrintFormRuleInfo newRule = (PrintFormRuleInfo) this.printFormRuleManager.CreateNewRule((BizRuleInfo) printFormRuleDialog.PrintRuleInfo);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The new Print Suppression Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/LoanFormPrinting/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/LoanFormPrinting";

    protected override BizRuleType GetBizRuleType() => BizRuleType.PrintForms;
  }
}
