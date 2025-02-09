// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InputFormRulePanel
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
  public class InputFormRulePanel : RuleListBase
  {
    private const string className = "InputFormRulePanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private InputFormsBpmManager formRuleManager;

    public InputFormRulePanel(Sessions.Session session, bool allowMultiSelect)
      : base(session, "Input Form Rules", allowMultiSelect)
    {
      this.formRuleManager = (InputFormsBpmManager) this.session.BPM.GetBpmManager(BpmCategory.InputForms);
      this.objectRuleManager = (object) this.formRuleManager;
      this.initForm();
      this.setHeader();
      this.listViewRule_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      using (InputFormRuleDialog inputFormRuleDialog = new InputFormRuleDialog(this.session, (InputFormRuleInfo) null, (InputFormInfo[]) null))
      {
        int num1 = (int) inputFormRuleDialog.ShowDialog((IWin32Window) this);
        if (inputFormRuleDialog.DialogResult == DialogResult.OK)
        {
          try
          {
            InputFormRuleInfo newRule = (InputFormRuleInfo) this.formRuleManager.CreateNewRule((BizRuleInfo) inputFormRuleDialog.InputFormRule);
            this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The new Input Form Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        inputFormRuleDialog.Dispose();
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
      InputFormRuleInfo tag = (InputFormRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Input Form Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        InputFormInfo[] forms = this.formRuleManager.GetForms(tag);
        string[] strArray = new string[forms.Length];
        for (int index = 0; index < forms.Length; ++index)
          strArray[index] = forms[index].Name;
        using (InputFormRuleDialog inputFormRuleDialog = new InputFormRuleDialog(this.session, (InputFormRuleInfo) tag.Clone(), forms))
        {
          if (inputFormRuleDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          try
          {
            InputFormRuleInfo inputFormRule = inputFormRuleDialog.InputFormRule;
            this.formRuleManager.UpdateRule((BizRuleInfo) inputFormRule);
            if (BizRule.RuleStatusStrings[1] == this.listViewRule.SelectedItems[0].SubItems[3].Text)
              this.listViewRule.SelectedItems[0].SubItems[3].Text = this.GetCurrentRuleStatus(inputFormRule.Condition, inputFormRule.Condition2, inputFormRule.ConditionState, inputFormRule.ConditionState2, inputFormRule.RuleID);
            this.editViewItemOnList(inputFormRule.RuleName, this.BuildChannelString((BizRuleInfo) inputFormRule), this.BuildConditionString((BizRuleInfo) inputFormRule), this.listViewRule.SelectedItems[0].SubItems[3].Text, (object) inputFormRule);
            this.listViewRule_SelectedIndexChanged((object) null, (EventArgs) null);
          }
          catch (Exception ex)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The Input Form Rule can not be changed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }

    protected override void dupBtn_Click(object sender, EventArgs e)
    {
      base.dupBtn_Click(sender, e);
      if (this.listViewRule.SelectedItems.Count == 0)
        return;
      InputFormRuleInfo tag = (InputFormRuleInfo) this.listViewRule.SelectedItems[0].Tag;
      if (tag == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This Input Form Rule can not be opened.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        InputFormInfo[] forms = this.formRuleManager.GetForms(tag);
        string[] strArray = new string[forms.Length];
        for (int index = 0; index < forms.Length; ++index)
          strArray[index] = forms[index].Name;
        BizRuleInfo[] allRules = this.formRuleManager.GetAllRules();
        string duplicateName = this.FindDuplicateName(tag.RuleName, allRules);
        using (InputFormRuleDialog inputFormRuleDialog = new InputFormRuleDialog(this.session, (InputFormRuleInfo) tag.Duplicate(duplicateName), forms))
        {
          int num2 = (int) inputFormRuleDialog.ShowDialog((IWin32Window) this);
          if (inputFormRuleDialog.DialogResult == DialogResult.OK)
          {
            try
            {
              InputFormRuleInfo newRule = (InputFormRuleInfo) this.formRuleManager.CreateNewRule((BizRuleInfo) inputFormRuleDialog.InputFormRule);
              this.addViewItemToList(newRule.RuleName, this.BuildChannelString((BizRuleInfo) newRule), this.BuildConditionString((BizRuleInfo) newRule), BizRule.RuleStatusStrings[0], newRule.LastModifiedByUserInfo, newRule.LastModTime.ToString("MM/dd/yyyy hh:mm tt"), (object) newRule, true, true);
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The new Input Form Rule can not be saved. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          inputFormRuleDialog.Dispose();
        }
        this.setHeader();
      }
    }

    protected override string GetExportURL()
    {
      return "/inputformlist/" + ((BizRuleInfo) this.listViewRule.SelectedItems[0].Tag).RuleID.ToString() + "?format=xml";
    }

    protected override string GetImportURL() => "/inputformlist";

    protected override BizRuleType GetBizRuleType() => BizRuleType.InputForms;
  }
}
