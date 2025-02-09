// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FieldRulePanel
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
  public class FieldRulePanel : RuleListBase
  {
    private const string className = "FieldRulePanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private FieldRulesBpmManager fieldRuleManager;

    public FieldRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Field Rules", allowMultiSelect)
    {
      this.fieldRuleManager = (FieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldRules);
      this.objectRuleManager = (object) this.fieldRuleManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (FieldRuleDialog fieldRuleDialog = new FieldRuleDialog(this.session, (FieldRuleInfo) null))
      {
        int num1 = (int) fieldRuleDialog.ShowDialog((IWin32Window) this);
        if (fieldRuleDialog.DialogResult == DialogResult.OK)
        {
          try
          {
            FieldRuleInfo newRule = (FieldRuleInfo) this.fieldRuleManager.CreateNewRule((BizRuleInfo) fieldRuleDialog.FieldRule);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The new Field Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        fieldRuleDialog.Dispose();
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
      FieldRuleInfo tag = (FieldRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Field Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (FieldRuleDialog fieldRuleDialog = new FieldRuleDialog(this.session, (FieldRuleInfo) tag.Clone()))
        {
          if (fieldRuleDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            try
            {
              FieldRuleInfo fieldRule = fieldRuleDialog.FieldRule;
              this.fieldRuleManager.UpdateRule((BizRuleInfo) fieldRule);
              if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
                this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(fieldRule.Condition, fieldRule.Condition2, fieldRule.ConditionState, fieldRule.ConditionState2, fieldRule.RuleID);
              this.editViewItemOnList(fieldRule.RuleName, this.BuildChannelString((BizRuleInfo) fieldRule), this.BuildConditionString((BizRuleInfo) fieldRule), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) fieldRule);
              this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
            }
            catch (Exception ex)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The Field Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          fieldRuleDialog.Dispose();
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      FieldRuleInfo tag = (FieldRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Field Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = this.fieldRuleManager.GetRules(tag.IsGeneralRule);
        string duplicateName = this.FindDuplicateName(tag.RuleName, rules);
        using (FieldRuleDialog fieldRuleDialog = new FieldRuleDialog(this.session, (FieldRuleInfo) tag.Duplicate(duplicateName)))
        {
          int num2 = (int) fieldRuleDialog.ShowDialog((IWin32Window) this);
          if (fieldRuleDialog.DialogResult == DialogResult.OK)
          {
            try
            {
              FieldRuleInfo newRule = (FieldRuleInfo) this.fieldRuleManager.CreateNewRule((BizRuleInfo) fieldRuleDialog.FieldRule);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Field Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          fieldRuleDialog.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/fieldrules/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/fieldrules";

    protected override BizRuleType GetBizRuleType() => BizRuleType.FieldRules;
  }
}
