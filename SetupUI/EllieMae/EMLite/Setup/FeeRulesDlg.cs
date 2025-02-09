// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeeRulesDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
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
  public class FeeRulesDlg : Form, IHelp
  {
    private string userid;
    private Sessions.Session session;
    private string advancedCodeXml;
    private Panel panel1;
    private Panel panel2;
    private DDMFeeRule _ddmFeeRule;
    private bool _isGlobalSetting;
    private Label label2;
    private Panel gcFeeRule;
    private TextBox ruleNameTxtBx;
    private Label label4;
    private Label label3;
    private Button findFeeLineBtn;
    private TextBox feeLineTxtBx;
    private Label label9;
    private Label label12;
    private ToolTip ttFeeRulesDlg;
    private IContainer components;
    private GroupContainer groupContainer2;
    private Label label11;
    private GroupContainer gcStopAutoPopulate;
    private TextBox conditionTextBox;
    private CheckBox conditionchkBx;
    private CheckBox afterLESentChkBx;
    private Label label15;
    private GradientPanel gradientPanel2;
    private Button cancelBtn;
    private Button okBtn;
    private PictureBox pictureBox1;
    private System.Windows.Forms.LinkLabel lnkLblLearnMore;
    private StandardIconButton btnSelect;
    private int ruleID;

    public DDMFeeRule DDMFeeRule => this._ddmFeeRule;

    public bool IsGlobalSetting => this._isGlobalSetting;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.gradientPanel2 = new GradientPanel();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.pictureBox1 = new PictureBox();
      this.lnkLblLearnMore = new System.Windows.Forms.LinkLabel();
      this.gcStopAutoPopulate = new GroupContainer();
      this.btnSelect = new StandardIconButton();
      this.conditionTextBox = new TextBox();
      this.conditionchkBx = new CheckBox();
      this.afterLESentChkBx = new CheckBox();
      this.label15 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.findFeeLineBtn = new Button();
      this.feeLineTxtBx = new TextBox();
      this.label9 = new Label();
      this.label12 = new Label();
      this.label11 = new Label();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.label2 = new Label();
      this.gcFeeRule = new Panel();
      this.label3 = new Label();
      this.ruleNameTxtBx = new TextBox();
      this.label4 = new Label();
      this.ttFeeRulesDlg = new ToolTip(this.components);
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.gcStopAutoPopulate.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.gcFeeRule.SuspendLayout();
      this.SuspendLayout();
      this.gradientPanel2.BackColor = Color.Transparent;
      this.gradientPanel2.BackgroundImageLayout = ImageLayout.None;
      this.gradientPanel2.BorderColor = Color.Transparent;
      this.gradientPanel2.BorderColorStyle = BorderColorStyle.None;
      this.gradientPanel2.Borders = AnchorStyles.None;
      this.gradientPanel2.Controls.Add((Control) this.cancelBtn);
      this.gradientPanel2.Controls.Add((Control) this.okBtn);
      this.gradientPanel2.Controls.Add((Control) this.pictureBox1);
      this.gradientPanel2.Controls.Add((Control) this.lnkLblLearnMore);
      this.gradientPanel2.Dock = DockStyle.Bottom;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 369);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(612, 38);
      this.gradientPanel2.TabIndex = 3;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancelBtn.Location = new Point(526, 8);
      this.cancelBtn.Margin = new Padding(2);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 1;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Enabled = false;
      this.okBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.okBtn.Location = new Point(447, 8);
      this.okBtn.Margin = new Padding(2);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 0;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) Resources.help;
      this.pictureBox1.Location = new Point(12, 8);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 17;
      this.pictureBox1.TabStop = false;
      this.lnkLblLearnMore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lnkLblLearnMore.AutoSize = true;
      this.lnkLblLearnMore.ImageAlign = ContentAlignment.MiddleLeft;
      this.lnkLblLearnMore.LinkBehavior = LinkBehavior.NeverUnderline;
      this.lnkLblLearnMore.LinkColor = Color.SteelBlue;
      this.lnkLblLearnMore.Location = new Point(32, 11);
      this.lnkLblLearnMore.Name = "lnkLblLearnMore";
      this.lnkLblLearnMore.Size = new Size(70, 13);
      this.lnkLblLearnMore.TabIndex = 2;
      this.lnkLblLearnMore.TabStop = true;
      this.lnkLblLearnMore.Text = "Learn More...";
      this.lnkLblLearnMore.Click += new EventHandler(this.LearnMore_Click);
      this.gcStopAutoPopulate.Borders = AnchorStyles.Bottom;
      this.gcStopAutoPopulate.Controls.Add((Control) this.btnSelect);
      this.gcStopAutoPopulate.Controls.Add((Control) this.conditionTextBox);
      this.gcStopAutoPopulate.Controls.Add((Control) this.conditionchkBx);
      this.gcStopAutoPopulate.Controls.Add((Control) this.afterLESentChkBx);
      this.gcStopAutoPopulate.Controls.Add((Control) this.label15);
      this.gcStopAutoPopulate.Dock = DockStyle.Fill;
      this.gcStopAutoPopulate.HeaderForeColor = SystemColors.ControlText;
      this.gcStopAutoPopulate.Location = new Point(0, 0);
      this.gcStopAutoPopulate.Name = "gcStopAutoPopulate";
      this.gcStopAutoPopulate.Size = new Size(612, 267);
      this.gcStopAutoPopulate.TabIndex = 2;
      this.gcStopAutoPopulate.Text = "Stop Automatically Populating Data";
      this.btnSelect.Anchor = AnchorStyles.Right;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Enabled = false;
      this.btnSelect.Location = new Point(581, 100);
      this.btnSelect.Margin = new Padding(2);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(20, 20);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 33;
      this.btnSelect.TabStop = false;
      this.ttFeeRulesDlg.SetToolTip((Control) this.btnSelect, "Edit Condition");
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.conditionTextBox.Enabled = false;
      this.conditionTextBox.Location = new Point(14, 75);
      this.conditionTextBox.Margin = new Padding(2);
      this.conditionTextBox.Multiline = true;
      this.conditionTextBox.Name = "conditionTextBox";
      this.conditionTextBox.Size = new Size(563, 137);
      this.conditionTextBox.TabIndex = 2;
      this.conditionchkBx.AutoSize = true;
      this.conditionchkBx.Location = new Point(13, 51);
      this.conditionchkBx.Margin = new Padding(2);
      this.conditionchkBx.Name = "conditionchkBx";
      this.conditionchkBx.Size = new Size(180, 17);
      this.conditionchkBx.TabIndex = 1;
      this.conditionchkBx.Text = "When the condition below is met";
      this.conditionchkBx.UseVisualStyleBackColor = true;
      this.conditionchkBx.CheckedChanged += new EventHandler(this.conditionchkBx_CheckedChanged);
      this.afterLESentChkBx.AutoSize = true;
      this.afterLESentChkBx.Location = new Point(13, 32);
      this.afterLESentChkBx.Margin = new Padding(2);
      this.afterLESentChkBx.Name = "afterLESentChkBx";
      this.afterLESentChkBx.Size = new Size(177, 17);
      this.afterLESentChkBx.TabIndex = 0;
      this.afterLESentChkBx.Text = "After initial Loan Estimate is sent";
      this.afterLESentChkBx.UseVisualStyleBackColor = true;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(-12, -13);
      this.label15.Margin = new Padding(2, 0, 2, 0);
      this.label15.Name = "label15";
      this.label15.Size = new Size(41, 13);
      this.label15.TabIndex = 2;
      this.label15.Text = "label15";
      this.groupContainer2.BackColor = Color.Transparent;
      this.groupContainer2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.groupContainer2.Controls.Add((Control) this.findFeeLineBtn);
      this.groupContainer2.Controls.Add((Control) this.feeLineTxtBx);
      this.groupContainer2.Controls.Add((Control) this.label9);
      this.groupContainer2.Controls.Add((Control) this.label12);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(612, 67);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Target Fee Line";
      this.findFeeLineBtn.Location = new Point(318, 39);
      this.findFeeLineBtn.Margin = new Padding(2);
      this.findFeeLineBtn.Name = "findFeeLineBtn";
      this.findFeeLineBtn.Size = new Size(95, 23);
      this.findFeeLineBtn.TabIndex = 1;
      this.findFeeLineBtn.Text = "Select Fee Line";
      this.findFeeLineBtn.UseVisualStyleBackColor = true;
      this.findFeeLineBtn.Click += new EventHandler(this.findFeeLineBtn_Click);
      this.feeLineTxtBx.Location = new Point(154, 40);
      this.feeLineTxtBx.Margin = new Padding(2);
      this.feeLineTxtBx.MaxLength = 100;
      this.feeLineTxtBx.Name = "feeLineTxtBx";
      this.feeLineTxtBx.Size = new Size(155, 20);
      this.feeLineTxtBx.TabIndex = 0;
      this.feeLineTxtBx.TextChanged += new EventHandler(this.feeLineTxtBx_TextChanged);
      this.label9.AutoSize = true;
      this.label9.ForeColor = Color.FromArgb(192, 0, 0);
      this.label9.Location = new Point(138, 40);
      this.label9.Margin = new Padding(2, 0, 2, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(11, 13);
      this.label9.TabIndex = 10;
      this.label9.Text = "*";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(7, 40);
      this.label12.Margin = new Padding(2, 0, 2, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(135, 13);
      this.label12.TabIndex = 1;
      this.label12.Text = "Auto-populate fee in Line #";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(-12, -12);
      this.label11.Margin = new Padding(2, 0, 2, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(41, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "label11";
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Controls.Add((Control) this.groupContainer2);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 35);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(612, 334);
      this.panel1.TabIndex = 1;
      this.panel2.Controls.Add((Control) this.gcStopAutoPopulate);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 67);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(612, 267);
      this.panel2.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(-12, -13);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(35, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "label2";
      this.gcFeeRule.Controls.Add((Control) this.label3);
      this.gcFeeRule.Controls.Add((Control) this.ruleNameTxtBx);
      this.gcFeeRule.Controls.Add((Control) this.label4);
      this.gcFeeRule.Controls.Add((Control) this.label2);
      this.gcFeeRule.Dock = DockStyle.Top;
      this.gcFeeRule.Location = new Point(0, 0);
      this.gcFeeRule.Name = "gcFeeRule";
      this.gcFeeRule.Size = new Size(612, 35);
      this.gcFeeRule.TabIndex = 0;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(7, 10);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(81, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Fee Rule Name";
      this.ruleNameTxtBx.Location = new Point(102, 10);
      this.ruleNameTxtBx.Margin = new Padding(2);
      this.ruleNameTxtBx.MaxLength = 64;
      this.ruleNameTxtBx.Name = "ruleNameTxtBx";
      this.ruleNameTxtBx.Size = new Size(268, 20);
      this.ruleNameTxtBx.TabIndex = 0;
      this.ruleNameTxtBx.TextChanged += new EventHandler(this.ruleNameTxtBx_TextChanged);
      this.label4.AutoSize = true;
      this.label4.ForeColor = Color.FromArgb(192, 0, 0);
      this.label4.Location = new Point(85, 10);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(11, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "*";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(612, 407);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gcFeeRule);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FeeRulesDlg);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Fee Rules";
      this.KeyPreview = true;
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.gcStopAutoPopulate.ResumeLayout(false);
      this.gcStopAutoPopulate.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.gcFeeRule.ResumeLayout(false);
      this.gcFeeRule.PerformLayout();
      this.ResumeLayout(false);
    }

    public FeeRulesDlg(Sessions.Session session, string userid)
    {
      this.session = session;
      this.userid = userid;
      this.InitializeComponent();
    }

    public FeeRulesDlg(int ruleID, Sessions.Session session)
    {
      this._isGlobalSetting = true;
      this.ruleID = ruleID;
      this.session = session;
      this.InitializeComponent();
      this.Text = "Rule Settings";
      this.loadFeeRule();
    }

    private void loadFeeRule()
    {
      this._ddmFeeRule = ((DDMFeeRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeRules)).GetDDMFeeRuleAndScenarioByID(this.ruleID, true);
      this.ruleNameTxtBx.Text = this._ddmFeeRule.RuleName;
      this.feeLineTxtBx.Text = this._ddmFeeRule.FeeLine;
      this.afterLESentChkBx.Checked = this._ddmFeeRule.InitLESent;
      this.conditionchkBx.Checked = this._ddmFeeRule.Condition;
      this.conditionTextBox.Text = this._ddmFeeRule.ConditionState;
      this.advancedCodeXml = this._ddmFeeRule.AdvCodeConditionXML;
      this.okBtn.Enabled = true;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.ruleNameTxtBx.Text.Length == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter Fee Rule Name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.feeLineTxtBx.Text.Length == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter Fee Line or Fee Group.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.isLineNumberInRange(this.feeLineTxtBx.Text) && !DDM_FieldAccess_Utils.IsValidSubLineNumber(this.feeLineTxtBx.Text))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The Line number is not in range.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.conditionchkBx.Checked && this.conditionTextBox.Text.Length == 0)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "You must provide code to determine the conditions under which this rule applies.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.ruleNameTxtBx.Text.Trim().Length == 0)
      {
        int num5 = (int) Utils.Dialog((IWin32Window) this, "Rule name can not be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        DDMFeeRulesBpmManager bpmManager = (DDMFeeRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
        if (this._isGlobalSetting)
        {
          if (this.ruleNameTxtBx.Text != this._ddmFeeRule.RuleName && bpmManager.DDMFeeRuleExist(this.ruleNameTxtBx.Text, true))
          {
            int num6 = (int) Utils.Dialog((IWin32Window) this, "The fee rule name that you entered already exists.  Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (this.feeLineTxtBx.Text != this._ddmFeeRule.FeeLine)
          {
            string text1 = this.feeLineTxtBx.Text;
            if (bpmManager.DDMFeeLineExist(text1.TrimStart('0'), true))
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "A fee rule already exists for the fee line you are attempting to create.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            string text2 = "Fee Scenarios may be affected by changing the Fee line. Do you want to continue?";
            if (text1.TrimStart('0').ToLower().Equals("801f"))
              text2 += " LO Compensation Rule conflicts on Fee lines 801e Broker Fees, 801f Broker Compensation can be resolved at the loan file level.";
            if (text1.TrimStart('0').ToLower().Equals("801e"))
              text2 += " LO Compensation Rule conflicts on Fee lines 801e Broker Fees, 801f Broker Compensation can be resolved at the loan file level.";
            if (Utils.Dialog((IWin32Window) this, text2, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
              return;
            this._ddmFeeRule.RuleName = this.ruleNameTxtBx.Text;
            this._ddmFeeRule.FeeLine = this.feeLineTxtBx.Text;
            this._ddmFeeRule.InitLESent = this.afterLESentChkBx.Checked;
            this._ddmFeeRule.Condition = this.conditionchkBx.Checked;
            this._ddmFeeRule.ConditionState = this.conditionTextBox.Text;
            this._ddmFeeRule.UpdateDt = DateTime.Now.ToString();
            this._ddmFeeRule.AdvCodeConditionXML = this.advancedCodeXml;
            this.DialogResult = DialogResult.OK;
            return;
          }
          this._ddmFeeRule.RuleName = this.ruleNameTxtBx.Text;
          this._ddmFeeRule.FeeLine = this.feeLineTxtBx.Text;
          this._ddmFeeRule.UpdateDt = DateTime.Now.ToString();
          this._ddmFeeRule.InitLESent = this.afterLESentChkBx.Checked;
          this._ddmFeeRule.Condition = this.conditionchkBx.Checked;
          this._ddmFeeRule.ConditionState = this.conditionTextBox.Text;
          this._ddmFeeRule.AdvCodeConditionXML = this.advancedCodeXml;
        }
        else
        {
          if (bpmManager.DDMFeeRuleExist(this.ruleNameTxtBx.Text, true))
          {
            int num8 = (int) Utils.Dialog((IWin32Window) this, "The Fee rule name that you entered already exists.  Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (bpmManager.DDMFeeLineExist(this.feeLineTxtBx.Text, true))
          {
            int num9 = (int) Utils.Dialog((IWin32Window) this, "A Fee rule already exists for the fee line you are attempting to create.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          string text3 = this.ruleNameTxtBx.Text;
          string text4 = this.feeLineTxtBx.Text;
          int num10 = this.afterLESentChkBx.Checked ? 1 : 0;
          string text5 = this.conditionTextBox.Text;
          int num11 = this.conditionchkBx.Checked ? 1 : 0;
          string userid = this.userid;
          string fullName = this.session.UserInfo.FullName;
          DateTime now = DateTime.Now;
          string createDt = now.ToString();
          now = DateTime.Now;
          string updateDt = now.ToString();
          string advancedCodeXml = this.advancedCodeXml;
          DDMFeeRule feeRule = new DDMFeeRule(text3, text4, num10 != 0, ruleStatus.Inactive, text5, num11 != 0, userid, fullName, createDt, updateDt, advancedCodeXml);
          feeRule.FeeType = type.line;
          feeRule.LastModByFullName = this.session.UserInfo.FullName;
          if (feeRule.FeeLine.TrimStart('0').ToLower().Equals("801f"))
          {
            int num12 = (int) Utils.Dialog((IWin32Window) this, "LO Compensation Rule conflicts on Fee lines 801e Broker Fees, 801f Broker Compensation can be resolved at the loan file level.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          if (feeRule.FeeLine.TrimStart('0').ToLower().Equals("801e"))
          {
            int num13 = (int) Utils.Dialog((IWin32Window) this, "LO Compensation Rule conflicts on Fee lines 801e Broker Fees, 801f Broker Compensation can be resolved at the loan file level.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          feeRule.RuleID = bpmManager.CreateFeeRule(feeRule, true);
          if (this.session.feerule == null)
            this.session.feerule = new List<DDMFeeRule>();
          this.session.feerule.Add(feeRule);
          this._ddmFeeRule = feeRule;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool isLineNumberInRange(string linenumber)
    {
      return DDM_FieldAccess_Utils.GetDDMLineRangeForCalculatedFields(linenumber) != "";
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.advancedCodeXml, true))
      {
        if (advConditionEditor.GetConditionScript() != this.conditionTextBox.Text)
          advConditionEditor.ClearFilters();
        if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.conditionTextBox.Text = advConditionEditor.GetConditionScript();
        this.advancedCodeXml = advConditionEditor.GetConditionXml();
      }
    }

    private void conditionchkBx_CheckedChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.conditionTextBox.Enabled = this.conditionchkBx.Checked;
    }

    private void feeLineTxtBx_TextChanged(object sender, EventArgs e)
    {
      this.okBtn.Enabled = !string.IsNullOrWhiteSpace(this.ruleNameTxtBx.Text) && !string.IsNullOrWhiteSpace(this.feeLineTxtBx.Text);
    }

    private void ruleNameTxtBx_TextChanged(object sender, EventArgs e)
    {
      this.feeLineTxtBx_TextChanged(sender, e);
    }

    private void feeLineTxtBx_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void findFeeLineBtn_Click(object sender, EventArgs e)
    {
      using (FeeLineSelectForm feeLineSelectForm = new FeeLineSelectForm())
      {
        if (feeLineSelectForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.feeLineTxtBx.Text = feeLineSelectForm.SelectedLine;
      }
    }

    private void LearnMore_Click(object sender, EventArgs e) => this.ShowHelp();

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      if (this._isGlobalSetting)
        JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Rule Settings");
      else
        JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Fee Rules");
    }
  }
}
