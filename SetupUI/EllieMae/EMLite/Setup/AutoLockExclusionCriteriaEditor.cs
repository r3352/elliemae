// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoLockExclusionCriteriaEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.AutoLockExclusionCriteria;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoLockExclusionCriteriaEditor : Form
  {
    private const string className = "AutoLockExclusionCriteriaEditor";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private AutoLockExclusionConditionControl ruleCondForm;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxName;
    private System.ComponentModel.Container components;
    private Label label2;
    private Label label1;
    private Panel panelCondition;
    private ArrayList existingFields;
    private EMHelpLink emHelpLink1;
    private FieldSettings fieldSettings;
    private ChannelConditionControl channelControl;
    private Panel panelDialog;
    private AutoLockExclusionRuleInfo fieldRule;

    public AutoLockExclusionCriteriaEditor(
      Sessions.Session session,
      AutoLockExclusionRuleInfo fieldRule)
    {
      this.fieldRule = fieldRule;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.channelControl = new ChannelConditionControl();
      if (this.fieldRule != null)
        this.channelControl.ChannelValue = this.fieldRule.Condition2;
      this.ruleCondForm = new AutoLockExclusionConditionControl(this.session);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public AutoLockExclusionRuleInfo FieldRule => this.fieldRule;

    private void InitializeComponent()
    {
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelCondition = new Panel();
      this.panelDialog = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.panelDialog.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.okBtn.Location = new Point(550, 286);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 8;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(634, 286);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 9;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxName.Location = new Point(32, 43);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(677, 20);
      this.textBoxName.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(16, 79);
      this.label2.Name = "label2";
      this.label2.Size = new Size(250, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "2. Apply Advanced Conditions for this Rule";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(136, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "1. Create a Rule Name";
      this.panelCondition.Location = new Point(32, 107);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(677, 94);
      this.panelCondition.TabIndex = 3;
      this.panelDialog.AutoScroll = true;
      this.panelDialog.Controls.Add((Control) this.cancelBtn);
      this.panelDialog.Controls.Add((Control) this.okBtn);
      this.panelDialog.Controls.Add((Control) this.textBoxName);
      this.panelDialog.Controls.Add((Control) this.panelCondition);
      this.panelDialog.Controls.Add((Control) this.emHelpLink1);
      this.panelDialog.Controls.Add((Control) this.label1);
      this.panelDialog.Controls.Add((Control) this.label2);
      this.panelDialog.Location = new Point(2, 2);
      this.panelDialog.Name = "panelDialog";
      this.panelDialog.Size = new Size(728, 318);
      this.panelDialog.TabIndex = 33;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Field Data Entry";
      this.emHelpLink1.Location = new Point(16, 286);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 16;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(747, 326);
      this.Controls.Add((Control) this.panelDialog);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (AutoLockExclusionCriteriaEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Auto-Lock Exclusion Criteria Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.panelDialog.ResumeLayout(false);
      this.panelDialog.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.existingFields = new ArrayList();
      if (this.fieldRule == null)
        return;
      this.ruleCondForm.SetCondition((BizRuleInfo) this.fieldRule);
      this.textBoxName.Text = this.fieldRule.RuleName;
    }

    private GVItem createGVItem(
      string col1,
      string col2,
      string[] requiredFields,
      object ruleObject)
    {
      GVItem gvItem = new GVItem(col1);
      gvItem.SubItems.Add((object) col2);
      string str1 = "";
      string str2 = "";
      string str3 = "";
      if (requiredFields != null && requiredFields.Length != 0)
      {
        for (int index = 0; index < requiredFields.Length; ++index)
        {
          if (!(requiredFields[index] == string.Empty))
            str3 = !(str3 == string.Empty) ? str3 + ", " + requiredFields[index] : requiredFields[index];
        }
      }
      if (str1 == string.Empty)
      {
        switch (ruleObject)
        {
          case FRRange _:
            FRRange frRange = (FRRange) ruleObject;
            str1 = "Range";
            str2 = !(frRange.LowerBound == string.Empty) || !(frRange.UpperBound == string.Empty) ? (!(frRange.LowerBound != string.Empty) || !(frRange.UpperBound == string.Empty) ? (!(frRange.LowerBound == string.Empty) || !(frRange.UpperBound != string.Empty) ? "Min " + frRange.LowerBound + ", Max " + frRange.UpperBound : "Max " + frRange.UpperBound) : "Min " + frRange.LowerBound) : string.Empty;
            break;
          case FRList _:
            FRList frList = (FRList) ruleObject;
            str1 = !frList.IsLock ? "Dropdown List (Editable)" : "Dropdown List";
            for (int index = 0; index < frList.List.Length; ++index)
            {
              if (!(frList.List[index] == string.Empty))
                str2 = !(str2 == string.Empty) ? str2 + ", " + frList.List[index] : frList.List[index];
            }
            break;
          default:
            str1 = "Advanced Coding";
            str2 = ruleObject.ToString();
            break;
        }
      }
      if (str1 == string.Empty && str3 != string.Empty)
        str1 = "Pre-Required Fields";
      gvItem.SubItems.Add((object) str1);
      gvItem.SubItems.Add((object) str2);
      gvItem.SubItems.Add((object) str3);
      return gvItem;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.AutoLockExclusionRules)).GetRules(false);
        for (int index = 0; index < rules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), rules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.fieldRule == null)
              flag = true;
            else if (this.fieldRule.RuleID != rules[index].RuleID || this.fieldRule.RuleID == 0)
              flag = true;
            if (flag)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The rule name that you entered is already in use. Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.textBoxName.Focus();
              return;
            }
          }
        }
        if (!this.ruleCondForm.ValidateCondition())
          return;
        this.fieldRule = this.fieldRule == null ? new AutoLockExclusionRuleInfo(this.textBoxName.Text.Trim()) : new AutoLockExclusionRuleInfo(this.fieldRule.RuleID, this.textBoxName.Text.Trim());
        this.ruleCondForm.ApplyCondition((BizRuleInfo) this.fieldRule);
        this.fieldRule.Condition2 = this.channelControl.ChannelValue;
        this.DialogResult = DialogResult.OK;
      }
    }

    private string getFieldDescription(string fieldID)
    {
      return EncompassFields.GetDescription(fieldID, this.fieldSettings);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F1)
      {
        this.ShowHelp();
      }
      else
      {
        if (e.KeyCode != Keys.Escape)
          return;
        this.cancelBtn.PerformClick();
      }
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (AutoLockExclusionCriteriaEditor));
    }
  }
}
