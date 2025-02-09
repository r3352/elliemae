// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintSelectionRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintSelectionRuleDialog : Form
  {
    private Sessions.Session session;
    private RuleConditionControl ruleCondForm;
    private ChannelConditionControl channelControl;
    private FieldSettings fieldSettings;
    private bool hasNoAccess;
    private PrintSelectionRuleInfo printSelectionRule;
    private IContainer components;
    private Label label6;
    private EMHelpLink emHelpLink1;
    private Label label3;
    private Label label2;
    private Label label1;
    private Panel panelCondition;
    private TextBox textBoxName;
    private Panel panelChannel;
    private GridView gridViewList;
    private DialogButtons dialogButtons1;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnRemove;
    private Label label4;
    private TextBox commentsTxt;

    public PrintSelectionRuleInfo PrintSelectionRule => this.printSelectionRule;

    public PrintSelectionRuleDialog(
      Sessions.Session session,
      PrintSelectionRuleInfo printSelectionRule,
      FieldSettings fieldSettings)
    {
      this.fieldSettings = fieldSettings;
      this.printSelectionRule = printSelectionRule;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.channelControl = new ChannelConditionControl();
      if (this.printSelectionRule != null)
        this.channelControl.ChannelValue = this.printSelectionRule.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.PrintSelection);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.gridViewList.DoubleClick += new EventHandler(this.btnEdit_Click);
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_PrintAutoSelection);
      this.btnAdd.Enabled = this.btnEdit.Enabled = this.btnRemove.Enabled = this.dialogButtons1.OKButton.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    private void initForm()
    {
      this.gridViewList.Items.Clear();
      if (this.printSelectionRule == null)
        return;
      this.textBoxName.Text = this.printSelectionRule.RuleName;
      if (this.printSelectionRule.CommentsTxt.Contains("\n") && !this.printSelectionRule.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.printSelectionRule.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.printSelectionRule.CommentsTxt;
      this.ruleCondForm.SetCondition((BizRuleInfo) this.printSelectionRule);
      if (this.printSelectionRule.Events == null)
        return;
      this.gridViewList.BeginUpdate();
      for (int index = 0; index < this.printSelectionRule.Events.Count; ++index)
        this.addObjectToList(this.printSelectionRule.Events[index], false);
      this.gridViewList.EndUpdate();
    }

    private void addObjectToList(PrintSelectionEvent printEvent, bool selected)
    {
      string[] activationFields = printEvent.GetActivationFields();
      GVItem gvItem = new GVItem(activationFields[0]);
      gvItem.SubItems.Add((object) EncompassFields.GetDescription(activationFields[0]));
      gvItem.SubItems.Add((object) this.buildFieldCondition(printEvent.Conditions[0]));
      FormInfo[] selectedForms = printEvent.SelectedForms;
      gvItem.SubItems.Add((object) new ObjectWithImage(this.buildFormNameString(selectedForms), (Image) Resources.document_group_public));
      gvItem.Tag = (object) printEvent;
      if (selected)
        gvItem.Selected = true;
      this.gridViewList.Items.Add(gvItem);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (PrintSelectionEventEditor selectionEventEditor = new PrintSelectionEventEditor(this.session, this.fieldSettings, (PrintSelectionEvent) null))
      {
        if (selectionEventEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addObjectToList(selectionEventEditor.PrintSelectionEvent, true);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.hasNoAccess)
        return;
      if (this.gridViewList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a rule.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PrintSelectionEventEditor selectionEventEditor = new PrintSelectionEventEditor(this.session, this.fieldSettings, (PrintSelectionEvent) this.gridViewList.SelectedItems[0].Tag))
        {
          if (selectionEventEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          PrintSelectionEvent printSelectionEvent = selectionEventEditor.PrintSelectionEvent;
          string[] activationFields = printSelectionEvent.GetActivationFields();
          this.gridViewList.SelectedItems[0].Text = activationFields[0];
          this.gridViewList.SelectedItems[0].SubItems[1].Text = EncompassFields.GetDescription(activationFields[0], this.fieldSettings);
          this.gridViewList.SelectedItems[0].SubItems[2].Text = this.buildFieldCondition(printSelectionEvent.Conditions[0]);
          this.gridViewList.SelectedItems[0].SubItems[3].Text = this.buildFormNameString(printSelectionEvent.SelectedForms);
          this.gridViewList.SelectedItems[0].Tag = (object) printSelectionEvent;
        }
      }
    }

    private string buildFormNameString(FormInfo[] forms)
    {
      if (forms == null || forms.Length == 0)
        return string.Empty;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < forms.Length; ++index)
      {
        string strA = forms[index].Name;
        if (string.Compare(strA, "FHA Informed Consumer Choice Dis", true) == 0)
          strA = "FHA Informed Consumer Choice Disclosure";
        else if (string.Compare(strA, "Loans Where Credit Score is Not Available", true) == 0)
          strA += " Model H5";
        else if (string.Compare(strA, "Risk-Based Pricing Notice", true) == 0)
          strA += " Model H1";
        int num = strA.LastIndexOf("\\");
        if (num > -1)
          strA = strA.Substring(num + 1);
        if (empty2 != string.Empty)
          empty2 += ",";
        empty2 += strA;
      }
      return empty2;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gridViewList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.gridViewList.SelectedItems[0].Index;
        this.gridViewList.Items.Remove(this.gridViewList.SelectedItems[0]);
        if (this.gridViewList.Items.Count == 0)
          return;
        if (index > this.gridViewList.Items.Count - 1)
          this.gridViewList.Items[this.gridViewList.Items.Count - 1].Selected = true;
        else
          this.gridViewList.Items[index].Selected = true;
      }
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.gridViewList.Items.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to add a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.PrintSelection)).GetRules(this.ruleCondForm.IsGeneralRule);
        for (int index = 0; index < rules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), rules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.printSelectionRule == null)
              flag = true;
            else if (this.printSelectionRule.RuleID != rules[index].RuleID || this.printSelectionRule.RuleID == 0)
              flag = true;
            if (flag)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The rule name that you entered is already in use. Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.textBoxName.Focus();
              return;
            }
          }
        }
        string channelValue = this.channelControl.ChannelValue;
        if (!this.ruleCondForm.ValidateCondition())
          return;
        PrintSelectionEvent[] collection = new PrintSelectionEvent[this.gridViewList.Items.Count];
        for (int nItemIndex = 0; nItemIndex < this.gridViewList.Items.Count; ++nItemIndex)
          collection[nItemIndex] = (PrintSelectionEvent) this.gridViewList.Items[nItemIndex].Tag;
        this.printSelectionRule = this.printSelectionRule == null ? new PrintSelectionRuleInfo(this.textBoxName.Text.Trim()) : new PrintSelectionRuleInfo(this.printSelectionRule.RuleID, this.textBoxName.Text.Trim());
        this.printSelectionRule.Events.AddRange((IEnumerable<PrintSelectionEvent>) collection);
        this.ruleCondForm.ApplyCondition((BizRuleInfo) this.printSelectionRule);
        this.printSelectionRule.Condition2 = channelValue;
        this.printSelectionRule.CommentsTxt = this.commentsTxt.Text;
        this.DialogResult = DialogResult.OK;
      }
    }

    private string buildFieldCondition(PrintSelectionCondition printCondition)
    {
      string str = string.Empty;
      switch (printCondition.ConditionType)
      {
        case PrintSelectionConditionType.FixedValue:
          str = "Value is " + ((PrintSelectionFixedValueCondition) printCondition).Value;
          break;
        case PrintSelectionConditionType.Range:
          PrintSelectionRangeCondition selectionRangeCondition = (PrintSelectionRangeCondition) printCondition;
          str = "Value Is Between " + selectionRangeCondition.Minimum + " AND " + selectionRangeCondition.Maximum;
          break;
        case PrintSelectionConditionType.ValueList:
          PrintSelectionValueListCondition valueListCondition = (PrintSelectionValueListCondition) printCondition;
          str = "Value is ";
          for (int index = 0; index < valueListCondition.Values.Length; ++index)
          {
            if (str != "Value is ")
              str += ",";
            str += valueListCondition.Values[index];
          }
          break;
      }
      return str;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.label6 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelCondition = new Panel();
      this.textBoxName = new TextBox();
      this.panelChannel = new Panel();
      this.gridViewList = new GridView();
      this.dialogButtons1 = new DialogButtons();
      this.btnAdd = new Button();
      this.btnEdit = new Button();
      this.btnRemove = new Button();
      this.label4 = new Label();
      this.commentsTxt = new TextBox();
      this.SuspendLayout();
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(12, 62);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Print Auto Selection";
      this.emHelpLink1.Location = new Point(29, 558);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 13;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(12, 312);
      this.label3.Name = "label3";
      this.label3.Size = new Size(365, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Select print forms to be automatically added in Print window.";
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(13, 175);
      this.label2.Name = "label2";
      this.label2.Size = new Size(264, 16);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this rule";
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(13, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(164, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Rule Name";
      this.panelCondition.Location = new Point(29, 203);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(653, 92);
      this.panelCondition.TabIndex = 5;
      this.textBoxName.Location = new Point(29, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(653, 20);
      this.textBoxName.TabIndex = 1;
      this.panelChannel.Location = new Point(29, 81);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(653, 79);
      this.panelChannel.TabIndex = 3;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Field Condition";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Auto Select Forms or Form Groups";
      gvColumn4.Width = 170;
      this.gridViewList.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridViewList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewList.Location = new Point(29, 344);
      this.gridViewList.Name = "gridViewList";
      this.gridViewList.Size = new Size(572, 200);
      this.gridViewList.TabIndex = 7;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 546);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Save";
      this.dialogButtons1.Size = new Size(1112, 40);
      this.dialogButtons1.TabIndex = 14;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.btnAdd.Location = new Point(607, 343);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 8;
      this.btnAdd.Text = "&Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnEdit.Location = new Point(607, 372);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(75, 23);
      this.btnEdit.TabIndex = 9;
      this.btnEdit.Text = "&Edit";
      this.btnEdit.UseVisualStyleBackColor = true;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnRemove.Location = new Point(607, 401);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(75, 23);
      this.btnRemove.TabIndex = 10;
      this.btnRemove.Text = "&Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(711, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Notes/Comments";
      this.commentsTxt.Location = new Point(714, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(364, 130);
      this.commentsTxt.TabIndex = 12;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1112, 586);
      this.Controls.Add((Control) this.commentsTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnEdit);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.dialogButtons1);
      this.Controls.Add((Control) this.gridViewList);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.panelCondition);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.panelChannel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PrintSelectionRuleDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Print Auto Selection Rule Details";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
