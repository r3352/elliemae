// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.GlobalFeeRuleDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class GlobalFeeRuleDlg : Form, IHelp
  {
    private int ruleID;
    private Sessions.Session sessionObject;
    private string oldRuleName;
    private string oldFeeLine;
    private bool isRuleNameChanged;
    private bool isLineNumChanged;
    private IContainer components;
    private GroupContainer gcFeeRule;
    private GroupContainer gcFeeLine;
    private TextBox textBox2;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private TextBox ruleNameTxtBx;
    private Label label4;
    private Label label3;
    private Label label2;
    private GroupContainer groupContainer2;
    private Button findFeeLineBtn;
    private TextBox feeLineTxtBx;
    private Label label9;
    private Label label11;
    private Label label12;
    private GroupContainer gcStopAutoPopulate;
    private StandardIconButton btnSelect;
    private TextBox conditionTextBox;
    private CheckBox conditionchkBx;
    private CheckBox afterLESentChkBx;
    private Label label15;
    private GradientPanel gradientPanel2;
    private Button btn_Cancel;
    private Button btn_OK;
    private PictureBox pictureBox1;
    private System.Windows.Forms.LinkLabel lnkLblLearnMore;

    public GlobalFeeRuleDlg(int ruleID, Sessions.Session session)
    {
      this.ruleID = ruleID;
      this.sessionObject = session;
      this.InitializeComponent();
      this.loadFeeRule();
    }

    public DDMFeeRule DDMFeeRule { get; set; }

    private void loadFeeRule()
    {
      DDMFeeRule ruleAndScenarioById = ((DDMFeeRulesBpmManager) this.sessionObject.BPM.GetBpmManager(BpmCategory.DDMFeeRules)).GetDDMFeeRuleAndScenarioByID(this.ruleID, true);
      this.oldRuleName = ruleAndScenarioById.RuleName;
      this.oldFeeLine = ruleAndScenarioById.FeeLine;
      this.ruleNameTxtBx.Text = ruleAndScenarioById.RuleName;
      this.feeLineTxtBx.Text = ruleAndScenarioById.FeeLine;
    }

    private void ruleNameTxtBx_TextChanged(object sender, EventArgs e)
    {
      if (this.ruleNameTxtBx.Text != this.oldRuleName)
      {
        this.isRuleNameChanged = true;
        this.btn_OK.Enabled = true;
      }
      else
      {
        this.isRuleNameChanged = false;
        if (this.isChanged())
          return;
        this.btn_OK.Enabled = false;
      }
    }

    private void feeLineTxtBx_TextChanged(object sender, EventArgs e)
    {
      if (this.feeLineTxtBx.Text != this.oldFeeLine)
      {
        this.isLineNumChanged = true;
        this.btn_OK.Enabled = true;
      }
      else
      {
        this.isLineNumChanged = false;
        if (this.isChanged())
          return;
        this.btn_OK.Enabled = false;
      }
    }

    private void btn_Cancel_Click(object sender, EventArgs e) => this.Close();

    private void btn_OK_Click(object sender, EventArgs e)
    {
      if (!this.isChanged())
        return;
      if (this.isLineNumChanged)
      {
        if (!this.isLineNumberInRange(Convert.ToInt32(this.feeLineTxtBx.Text)))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The Line number is not in range.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (MessageBox.Show("Fee Scenarios may be affected by changing the Fee line. Do you want to continue?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
          this.setDDMFeeRule();
      }
      DDMFeeRulesBpmManager bpmManager = (DDMFeeRulesBpmManager) this.sessionObject.BPM.GetBpmManager(BpmCategory.DDMFeeRules);
      if (this.ruleNameTxtBx.Text != this.oldRuleName && bpmManager.DDMFeeRuleExist(this.ruleNameTxtBx.Text, true))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The Fee rule name that you entered already exists.  Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private bool isChanged() => this.isRuleNameChanged || this.isLineNumChanged;

    private void setDDMFeeRule()
    {
      this.DDMFeeRule = new DDMFeeRule(this.ruleID);
      this.DDMFeeRule.RuleName = this.ruleNameTxtBx.Text;
      this.DDMFeeRule.FeeLine = this.feeLineTxtBx.Text;
    }

    private bool isLineNumberInRange(int linenumber)
    {
      return linenumber >= 808 && linenumber <= 835 || linenumber >= 1302 && linenumber <= 1320 || linenumber >= 1109 && linenumber <= 1116;
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
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Fee Rules");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcFeeRule = new GroupContainer();
      this.gcFeeLine = new GroupContainer();
      this.textBox2 = new TextBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.ruleNameTxtBx = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.label12 = new Label();
      this.findFeeLineBtn = new Button();
      this.feeLineTxtBx = new TextBox();
      this.label9 = new Label();
      this.label11 = new Label();
      this.gcStopAutoPopulate = new GroupContainer();
      this.btnSelect = new StandardIconButton();
      this.conditionTextBox = new TextBox();
      this.conditionchkBx = new CheckBox();
      this.afterLESentChkBx = new CheckBox();
      this.label15 = new Label();
      this.gradientPanel2 = new GradientPanel();
      this.btn_Cancel = new Button();
      this.btn_OK = new Button();
      this.pictureBox1 = new PictureBox();
      this.lnkLblLearnMore = new System.Windows.Forms.LinkLabel();
      this.gcFeeRule.SuspendLayout();
      this.gcFeeLine.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.gcStopAutoPopulate.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.gcFeeRule.Borders = AnchorStyles.Bottom;
      this.gcFeeRule.Controls.Add((Control) this.gcFeeLine);
      this.gcFeeRule.Controls.Add((Control) this.ruleNameTxtBx);
      this.gcFeeRule.Controls.Add((Control) this.label4);
      this.gcFeeRule.Controls.Add((Control) this.label3);
      this.gcFeeRule.Controls.Add((Control) this.label2);
      this.gcFeeRule.Dock = DockStyle.Top;
      this.gcFeeRule.HeaderForeColor = SystemColors.ControlText;
      this.gcFeeRule.Location = new Point(0, 0);
      this.gcFeeRule.Name = "gcFeeRule";
      this.gcFeeRule.Size = new Size(612, 96);
      this.gcFeeRule.TabIndex = 5;
      this.gcFeeLine.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcFeeLine.Controls.Add((Control) this.textBox2);
      this.gcFeeLine.Controls.Add((Control) this.label5);
      this.gcFeeLine.Controls.Add((Control) this.label6);
      this.gcFeeLine.Controls.Add((Control) this.label7);
      this.gcFeeLine.Controls.Add((Control) this.label8);
      this.gcFeeLine.HeaderForeColor = SystemColors.ControlText;
      this.gcFeeLine.Location = new Point(-1, 106);
      this.gcFeeLine.Name = "gcFeeLine";
      this.gcFeeLine.Size = new Size(614, 97);
      this.gcFeeLine.TabIndex = 5;
      this.gcFeeLine.Text = "Target Fee Line/Line Group";
      this.textBox2.Location = new Point(118, 58);
      this.textBox2.Margin = new Padding(2);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(396, 20);
      this.textBox2.TabIndex = 4;
      this.label5.AutoSize = true;
      this.label5.ForeColor = Color.FromArgb(192, 0, 0);
      this.label5.Location = new Point(103, 58);
      this.label5.Margin = new Padding(2, 0, 2, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(11, 13);
      this.label5.TabIndex = 3;
      this.label5.Text = "*";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 10.2f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(4, 56);
      this.label6.Margin = new Padding(2, 0, 2, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(106, 17);
      this.label6.TabIndex = 2;
      this.label6.Text = "Fee Rule Name";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(-11, -12);
      this.label7.Margin = new Padding(2, 0, 2, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(35, 13);
      this.label7.TabIndex = 1;
      this.label7.Text = "label7";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(4, 24);
      this.label8.Margin = new Padding(2, 0, 2, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(319, 13);
      this.label8.TabIndex = 0;
      this.label8.Text = "These global settings apply to this fee.  Select your settings below.";
      this.ruleNameTxtBx.Location = new Point(105, 47);
      this.ruleNameTxtBx.Margin = new Padding(2);
      this.ruleNameTxtBx.Name = "ruleNameTxtBx";
      this.ruleNameTxtBx.Size = new Size(268, 20);
      this.ruleNameTxtBx.TabIndex = 1;
      this.ruleNameTxtBx.TextChanged += new EventHandler(this.ruleNameTxtBx_TextChanged);
      this.label4.AutoSize = true;
      this.label4.ForeColor = Color.FromArgb(192, 0, 0);
      this.label4.Location = new Point(86, 47);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(11, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "*";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(10, 50);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(81, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Fee Rule Name";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(-12, -13);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(35, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "label2";
      this.groupContainer2.Borders = AnchorStyles.Bottom;
      this.groupContainer2.Controls.Add((Control) this.label12);
      this.groupContainer2.Controls.Add((Control) this.findFeeLineBtn);
      this.groupContainer2.Controls.Add((Control) this.feeLineTxtBx);
      this.groupContainer2.Controls.Add((Control) this.label9);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Dock = DockStyle.Top;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 96);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(612, 100);
      this.groupContainer2.TabIndex = 7;
      this.groupContainer2.Text = "Target Fee Line";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(10, 41);
      this.label12.Margin = new Padding(2, 0, 2, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(135, 13);
      this.label12.TabIndex = 2;
      this.label12.Text = "Auto-populate fee in Line #";
      this.findFeeLineBtn.Location = new Point(324, 38);
      this.findFeeLineBtn.Margin = new Padding(2);
      this.findFeeLineBtn.Name = "findFeeLineBtn";
      this.findFeeLineBtn.Size = new Size(95, 23);
      this.findFeeLineBtn.TabIndex = 4;
      this.findFeeLineBtn.Text = "Select Fee Line";
      this.findFeeLineBtn.UseVisualStyleBackColor = true;
      this.feeLineTxtBx.Location = new Point(165, 39);
      this.feeLineTxtBx.Margin = new Padding(2);
      this.feeLineTxtBx.Name = "feeLineTxtBx";
      this.feeLineTxtBx.Size = new Size(155, 20);
      this.feeLineTxtBx.TabIndex = 3;
      this.feeLineTxtBx.TextChanged += new EventHandler(this.feeLineTxtBx_TextChanged);
      this.label9.AutoSize = true;
      this.label9.ForeColor = Color.FromArgb(192, 0, 0);
      this.label9.Location = new Point(142, 39);
      this.label9.Margin = new Padding(2, 0, 2, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(11, 13);
      this.label9.TabIndex = 3;
      this.label9.Text = "*";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(-12, -13);
      this.label11.Margin = new Padding(2, 0, 2, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(41, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "label11";
      this.gcStopAutoPopulate.Borders = AnchorStyles.Bottom;
      this.gcStopAutoPopulate.Controls.Add((Control) this.btnSelect);
      this.gcStopAutoPopulate.Controls.Add((Control) this.conditionTextBox);
      this.gcStopAutoPopulate.Controls.Add((Control) this.conditionchkBx);
      this.gcStopAutoPopulate.Controls.Add((Control) this.afterLESentChkBx);
      this.gcStopAutoPopulate.Controls.Add((Control) this.label15);
      this.gcStopAutoPopulate.Dock = DockStyle.Fill;
      this.gcStopAutoPopulate.HeaderForeColor = SystemColors.ControlText;
      this.gcStopAutoPopulate.Location = new Point(0, 196);
      this.gcStopAutoPopulate.Name = "gcStopAutoPopulate";
      this.gcStopAutoPopulate.Size = new Size(612, 272);
      this.gcStopAutoPopulate.TabIndex = 9;
      this.gcStopAutoPopulate.Text = "Stop Automatically Populating Data";
      this.btnSelect.Anchor = AnchorStyles.Right;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Enabled = false;
      this.btnSelect.Location = new Point(581, 78);
      this.btnSelect.Margin = new Padding(2);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(20, 20);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 33;
      this.btnSelect.TabStop = false;
      this.conditionTextBox.Enabled = false;
      this.conditionTextBox.Location = new Point(14, 78);
      this.conditionTextBox.Margin = new Padding(2);
      this.conditionTextBox.Multiline = true;
      this.conditionTextBox.Name = "conditionTextBox";
      this.conditionTextBox.Size = new Size(559, 137);
      this.conditionTextBox.TabIndex = 7;
      this.conditionchkBx.AutoSize = true;
      this.conditionchkBx.Location = new Point(13, 54);
      this.conditionchkBx.Margin = new Padding(2);
      this.conditionchkBx.Name = "conditionchkBx";
      this.conditionchkBx.Size = new Size(162, 17);
      this.conditionchkBx.TabIndex = 6;
      this.conditionchkBx.Text = "When condition below is met";
      this.conditionchkBx.UseVisualStyleBackColor = true;
      this.afterLESentChkBx.AutoSize = true;
      this.afterLESentChkBx.Location = new Point(13, 32);
      this.afterLESentChkBx.Margin = new Padding(2);
      this.afterLESentChkBx.Name = "afterLESentChkBx";
      this.afterLESentChkBx.Size = new Size(177, 17);
      this.afterLESentChkBx.TabIndex = 5;
      this.afterLESentChkBx.Text = "After initial Loan Estimate is sent";
      this.afterLESentChkBx.UseVisualStyleBackColor = true;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(-12, -13);
      this.label15.Margin = new Padding(2, 0, 2, 0);
      this.label15.Name = "label15";
      this.label15.Size = new Size(41, 13);
      this.label15.TabIndex = 1;
      this.label15.Text = "label15";
      this.gradientPanel2.BackColor = Color.Transparent;
      this.gradientPanel2.BackgroundImageLayout = ImageLayout.None;
      this.gradientPanel2.BorderColor = Color.Transparent;
      this.gradientPanel2.BorderColorStyle = BorderColorStyle.None;
      this.gradientPanel2.Borders = AnchorStyles.None;
      this.gradientPanel2.Controls.Add((Control) this.btn_Cancel);
      this.gradientPanel2.Controls.Add((Control) this.btn_OK);
      this.gradientPanel2.Controls.Add((Control) this.pictureBox1);
      this.gradientPanel2.Controls.Add((Control) this.lnkLblLearnMore);
      this.gradientPanel2.Dock = DockStyle.Bottom;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 418);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(612, 50);
      this.gradientPanel2.TabIndex = 22;
      this.btn_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btn_Cancel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btn_Cancel.Location = new Point(526, 14);
      this.btn_Cancel.Margin = new Padding(2);
      this.btn_Cancel.Name = "btn_Cancel";
      this.btn_Cancel.Size = new Size(75, 23);
      this.btn_Cancel.TabIndex = 20;
      this.btn_Cancel.Text = "Cancel";
      this.btn_Cancel.UseVisualStyleBackColor = true;
      this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
      this.btn_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btn_OK.Enabled = false;
      this.btn_OK.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btn_OK.Location = new Point(447, 14);
      this.btn_OK.Margin = new Padding(2);
      this.btn_OK.Name = "btn_OK";
      this.btn_OK.Size = new Size(75, 23);
      this.btn_OK.TabIndex = 19;
      this.btn_OK.Text = "OK";
      this.btn_OK.UseVisualStyleBackColor = true;
      this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
      this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) Resources.help;
      this.pictureBox1.Location = new Point(12, 14);
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
      this.lnkLblLearnMore.Location = new Point(39, 18);
      this.lnkLblLearnMore.Name = "lnkLblLearnMore";
      this.lnkLblLearnMore.Size = new Size(70, 13);
      this.lnkLblLearnMore.TabIndex = 18;
      this.lnkLblLearnMore.TabStop = true;
      this.lnkLblLearnMore.Text = "Learn More...";
      this.lnkLblLearnMore.Click += new EventHandler(this.LearnMore_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(612, 468);
      this.Controls.Add((Control) this.gradientPanel2);
      this.Controls.Add((Control) this.gcStopAutoPopulate);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.gcFeeRule);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GlobalFeeRuleDlg);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Rule Settings";
      this.KeyPreview = true;
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.gcFeeRule.ResumeLayout(false);
      this.gcFeeRule.PerformLayout();
      this.gcFeeLine.ResumeLayout(false);
      this.gcFeeLine.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.gcStopAutoPopulate.ResumeLayout(false);
      this.gcStopAutoPopulate.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
