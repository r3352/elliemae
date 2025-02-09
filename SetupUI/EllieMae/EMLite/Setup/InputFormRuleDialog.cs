// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InputFormRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InputFormRuleDialog : Form
  {
    private const string className = "InputFormRuleDialog";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private bool hasNoAccess;
    private Sessions.Session session;
    private RuleConditionControl ruleCondForm;
    private InputFormInfo[] inputForms;
    private TextBox textBoxName;
    private Button cancelBtn;
    private Button okBtn;
    private GroupBox groupBox1;
    private Button addBtn;
    private Button removeBtn;
    private System.ComponentModel.Container components;
    private InputFormInfo[] formOrder;
    private Label label1;
    private Panel panelCondition;
    private Label label2;
    private Label label3;
    private EMHelpLink emHelpLink1;
    private Label label6;
    private Panel panelChannel;
    private Hashtable currentFormList;
    private GridView listViewSelects;
    private Label label4;
    private TextBox commentsTxt;
    private ChannelConditionControl channelControl;
    private InputFormRuleInfo inputFormRule;

    public InputFormRuleDialog(
      Sessions.Session session,
      InputFormRuleInfo inputFormRule,
      InputFormInfo[] inputForms)
    {
      this.inputFormRule = inputFormRule;
      this.inputForms = inputForms;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.ruleCondForm = new RuleConditionControl(this.session);
      try
      {
        this.formOrder = this.session.FormManager.GetFormInfos(InputFormCategory.Form);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputFormRuleDialog.sw, TraceLevel.Error, nameof (InputFormRuleDialog), "InputFormRuleDialog: Can't access Form List. Error: " + ex.Message);
      }
      this.channelControl = new ChannelConditionControl();
      if (this.inputFormRule != null)
        this.channelControl.ChannelValue = this.inputFormRule.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.ruleCondForm.Dock = DockStyle.Fill;
      this.ruleCondForm.Location = new Point(3, 16);
      this.ruleCondForm.Name = nameof (ruleCondForm);
      this.ruleCondForm.Size = new Size(562, 29);
      this.ruleCondForm.TabIndex = 0;
      this.ruleCondForm.InitControl(BpmCategory.InputForms);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_InputFormList);
      this.addBtn.Enabled = this.removeBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public InputFormRuleInfo InputFormRule => this.inputFormRule;

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.textBoxName = new TextBox();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.groupBox1 = new GroupBox();
      this.listViewSelects = new GridView();
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.label1 = new Label();
      this.panelCondition = new Panel();
      this.label2 = new Label();
      this.label3 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.label4 = new Label();
      this.commentsTxt = new TextBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(567, 20);
      this.textBoxName.TabIndex = 1;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(901, 532);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 13;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(820, 532);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 12;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.groupBox1.Controls.Add((Control) this.listViewSelects);
      this.groupBox1.Controls.Add((Control) this.removeBtn);
      this.groupBox1.Controls.Add((Control) this.addBtn);
      this.groupBox1.Location = new Point(33, 314);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(568, 198);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Form List";
      gvColumn.Width = 466;
      this.listViewSelects.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.listViewSelects.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewSelects.Location = new Point(10, 19);
      this.listViewSelects.Name = "listViewSelects";
      this.listViewSelects.Size = new Size(468, 168);
      this.listViewSelects.TabIndex = 8;
      this.removeBtn.Location = new Point(484, 49);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(75, 23);
      this.removeBtn.TabIndex = 8;
      this.removeBtn.Text = "&Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Location = new Point(484, 20);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 23);
      this.addBtn.TabIndex = 7;
      this.addBtn.Text = "&Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(16, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(136, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Rule Name";
      this.panelCondition.Location = new Point(33, 207);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(568, 68);
      this.panelCondition.TabIndex = 5;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(20, 179);
      this.label2.Name = "label2";
      this.label2.Size = new Size(200, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Is there a condition for this rule";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(20, 287);
      this.label3.Name = "label3";
      this.label3.Size = new Size(164, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Define Input Form to Add";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Input Form List";
      this.emHelpLink1.Location = new Point(32, 532);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 11;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 62);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(33, 87);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(567, 79);
      this.panelChannel.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(639, 12);
      this.label4.Name = "label4";
      this.label4.Size = new Size(103, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Notes/Comments";
      this.commentsTxt.Location = new Point(642, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(334, 136);
      this.commentsTxt.TabIndex = 10;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(1002, 583);
      this.Controls.Add((Control) this.commentsTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.panelChannel);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.panelCondition);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InputFormRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Input Form Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initForm()
    {
      this.currentFormList = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      if (this.inputFormRule == null)
        return;
      for (int index = 0; index < this.inputForms.Length; ++index)
        this.currentFormList.Add((object) this.inputForms[index].FormID, (object) InputFormInfo.GetCorrectFormName(this.inputForms[index].FormID, this.inputForms[index].Name));
      if (this.currentFormList != null && this.currentFormList.ContainsKey((object) "MAX23K"))
      {
        if (!this.currentFormList.ContainsKey((object) "FHAPROCESSMGT"))
          this.currentFormList.Add((object) "FHAPROCESSMGT", (object) "FHA Management");
        this.currentFormList.Remove((object) "MAX23K");
      }
      this.refreshFormOrder();
      this.ruleCondForm.SetCondition((BizRuleInfo) this.inputFormRule);
      this.textBoxName.Text = this.inputFormRule.RuleName;
      if (this.inputFormRule.CommentsTxt.Contains("\n") && !this.inputFormRule.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.inputFormRule.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.inputFormRule.CommentsTxt;
    }

    private void refreshFormOrder()
    {
      this.listViewSelects.BeginUpdate();
      this.listViewSelects.Items.Clear();
      foreach (InputFormInfo inputFormInfo in this.formOrder)
      {
        if (this.currentFormList.ContainsKey((object) inputFormInfo.FormID))
          this.listViewSelects.Items.Add(new GVItem(InputFormInfo.GetCorrectFormName(inputFormInfo.FormID, inputFormInfo.Name))
          {
            Tag = (object) inputFormInfo.FormID
          });
      }
      this.listViewSelects.EndUpdate();
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      using (AddForms addForms = new AddForms(this.session, this.currentFormList, true))
      {
        if (addForms.ShowDialog((IWin32Window) this) != DialogResult.OK || addForms.SelectedFormTable.Count <= 0)
          return;
        foreach (string[] strArray in addForms.SelectedFormTable)
        {
          if (!this.currentFormList.ContainsKey((object) strArray[0]))
            this.currentFormList.Add((object) strArray[0], (object) strArray[1]);
        }
        this.refreshFormOrder();
      }
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewSelects.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an input form first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int nItemIndex = -1;
        foreach (GVItem selectedItem in this.listViewSelects.SelectedItems)
        {
          this.currentFormList.Remove((object) (string) selectedItem.Tag);
          if (nItemIndex == -1)
            nItemIndex = selectedItem.Index;
          this.listViewSelects.Items.Remove(selectedItem);
        }
        if (this.listViewSelects.Items.Count == 0)
          return;
        if (nItemIndex > this.listViewSelects.Items.Count - 1)
          this.listViewSelects.Items[this.listViewSelects.Items.Count - 1].Selected = true;
        else
          this.listViewSelects.Items[nItemIndex].Selected = true;
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] allRules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.InputForms)).GetAllRules();
        for (int index = 0; index < allRules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), allRules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.inputFormRule == null)
              flag = true;
            else if (this.inputFormRule.RuleID != allRules[index].RuleID || this.inputFormRule.RuleID == 0)
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
        if (this.listViewSelects.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add an input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string[] strArray = new string[this.listViewSelects.Items.Count];
          for (int nItemIndex = 0; nItemIndex < this.listViewSelects.Items.Count; ++nItemIndex)
            strArray[nItemIndex] = this.listViewSelects.Items[nItemIndex].Tag.ToString();
          this.inputFormRule = this.inputFormRule == null ? new InputFormRuleInfo(this.textBoxName.Text.Trim()) : new InputFormRuleInfo(this.inputFormRule.RuleID, this.textBoxName.Text.Trim());
          this.inputFormRule.Forms = strArray;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.inputFormRule);
          this.inputFormRule.Condition2 = this.channelControl.ChannelValue;
          this.inputFormRule.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
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
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (InputFormRuleDialog));
    }
  }
}
