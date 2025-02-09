// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MIRecordSetupForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MIRecordSetupForm : Form
  {
    private int recordID = -1;
    private FieldFilter[] newScenarios;
    private bool viewOnly;
    private LoanTypeEnum loanType;
    private string tabName = string.Empty;
    private Sessions.Session session;
    private MIRecord miRecord;
    private IContainer components;
    private TextBox txtScenario;
    private Label label1;
    private Button cancelBtn;
    private Button okBtn;
    private Label labelPremium;
    private TextBox txtPremium1;
    private TextBox txtSub;
    private Label labelSubsequent;
    private TextBox txtMonthly1;
    private Label label4;
    private TextBox txtMonthly2;
    private Label label5;
    private TextBox txtMonth2;
    private Label label6;
    private TextBox txtMonth1;
    private Label label7;
    private TextBox txtCutoff;
    private Label label9;
    private Label label8;
    private Label label10;
    private Label labelPrem;
    private Label label1Sub;
    private Button btnCreate;
    private Label label13;

    public MIRecordSetupForm(
      Sessions.Session session,
      MIRecord miRecord,
      LoanTypeEnum loanType,
      string tabName)
      : this(session, miRecord, loanType, false)
    {
      this.tabName = tabName;
    }

    public MIRecordSetupForm(
      Sessions.Session session,
      MIRecord miRecord,
      LoanTypeEnum loanType,
      bool viewOnly)
    {
      this.session = session;
      this.loanType = loanType;
      this.viewOnly = viewOnly;
      this.InitializeComponent();
      if (this.loanType != LoanTypeEnum.VA)
      {
        this.txtSub.Visible = false;
        this.labelSubsequent.Visible = false;
        this.label1Sub.Visible = false;
      }
      if (this.loanType == LoanTypeEnum.Other)
      {
        this.txtPremium1.Visible = false;
        this.labelPrem.Visible = false;
        this.labelPremium.Visible = false;
      }
      else if (this.loanType == LoanTypeEnum.USDA)
      {
        this.txtCutoff.Enabled = this.txtMonthly2.Enabled = this.txtMonth2.Enabled = false;
        this.labelPremium.Text = "Guarantee Fee";
        this.label4.Text = "1st Annual Fee";
        this.label5.Text = "1st Annual Fee Mos";
        this.label6.Text = "2nd Annual Fee Mos";
        this.label7.Text = "2nd Annual Fee";
      }
      if (this.viewOnly)
      {
        this.btnCreate.Text = "&View";
        this.cancelBtn.Text = "&Close";
        this.okBtn.Visible = false;
        this.txtPremium1.ReadOnly = true;
        this.txtSub.ReadOnly = true;
        this.txtMonthly1.ReadOnly = true;
        this.txtMonthly2.ReadOnly = true;
        this.txtMonth1.ReadOnly = true;
        this.txtMonth2.ReadOnly = true;
        this.txtCutoff.ReadOnly = true;
      }
      if (miRecord == null)
        return;
      this.recordID = miRecord.Id;
      this.txtScenario.Text = miRecord.ScenarioKeyForUI;
      this.newScenarios = (FieldFilter[]) miRecord.Scenarios.Clone();
      double num1;
      if (miRecord.Premium1st > 0.0)
      {
        TextBox txtPremium1 = this.txtPremium1;
        num1 = miRecord.Premium1st;
        string str = num1.ToString("N3");
        txtPremium1.Text = str;
      }
      if (miRecord.SubsequentPremium > 0.0)
      {
        TextBox txtSub = this.txtSub;
        num1 = miRecord.SubsequentPremium;
        string str = num1.ToString("N3");
        txtSub.Text = str;
      }
      if (miRecord.Monthly1st > 0.0)
      {
        TextBox txtMonthly1 = this.txtMonthly1;
        num1 = miRecord.Monthly1st;
        string str = num1.ToString("N3");
        txtMonthly1.Text = str;
      }
      if (miRecord.Monthly2st > 0.0)
      {
        TextBox txtMonthly2 = this.txtMonthly2;
        num1 = miRecord.Monthly2st;
        string str = num1.ToString("N3");
        txtMonthly2.Text = str;
      }
      int num2;
      if (miRecord.Months1st > 0)
      {
        TextBox txtMonth1 = this.txtMonth1;
        num2 = miRecord.Months1st;
        string str = num2.ToString();
        txtMonth1.Text = str;
      }
      if (miRecord.Months2st > 0)
      {
        TextBox txtMonth2 = this.txtMonth2;
        num2 = miRecord.Months2st;
        string str = num2.ToString();
        txtMonth2.Text = str;
      }
      if (miRecord.Cutoff <= 0.0)
        return;
      TextBox txtCutoff = this.txtCutoff;
      num1 = miRecord.Cutoff;
      string str1 = num1.ToString("N3");
      txtCutoff.Text = str1;
    }

    public MIRecord NewMIRecord => this.miRecord;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.txtScenario.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The Scenario field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.session.ConfigurationManager.HasDuplicateMIRecord(this.recordID, MIRecord.GetScenarios(this.newScenarios, false), this.loanType, this.tabName))
      {
        if (this.loanType == LoanTypeEnum.Conventional)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The '" + this.tabName + "' tab in Conventional MI Table already contains this scenario.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The MI Table '" + this.loanType.ToString() + "' already contains this scenario.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        this.btnCreate.Focus();
      }
      else
      {
        if (this.txtMonth1.Text.Trim() == "")
          this.txtMonth1.Text = "0";
        if (this.txtMonth2.Text.Trim() == "")
          this.txtMonth2.Text = "0";
        if (this.loanType != LoanTypeEnum.VA)
          this.txtSub.Text = "0";
        if (this.loanType == LoanTypeEnum.Other)
          this.txtPremium1.Text = "0";
        this.miRecord = new MIRecord(MIRecord.GetScenarios(this.newScenarios, false), this.newScenarios, Utils.ParseDouble((object) this.txtPremium1.Text), Utils.ParseDouble((object) this.txtSub.Text), Utils.ParseDouble((object) this.txtMonthly1.Text), Utils.ParseInt((object) this.txtMonth1.Text), Utils.ParseDouble((object) this.txtMonthly2.Text), Utils.ParseInt((object) this.txtMonth2.Text), Utils.ParseDouble((object) this.txtCutoff.Text), 0);
        if (this.recordID != -1)
          this.miRecord.Id = this.recordID;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void decimal_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.verifyNumKeyPress(e, false);
    }

    private void int_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.verifyNumKeyPress(e, true);
    }

    private void verifyNumKeyPress(KeyPressEventArgs e, bool integerOnly)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!integerOnly && e.KeyChar.Equals('.'))
        e.Handled = false;
      else if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void decimal_FieldLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text);
      if (num != 0.0)
        textBox.Text = num.ToString("N3");
      else
        textBox.Text = "";
    }

    private void btnCreate_Click(object sender, EventArgs e)
    {
      using (QueryBuilderForm queryBuilderForm = new QueryBuilderForm(this.session, this.newScenarios, this.viewOnly))
      {
        if (queryBuilderForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.newScenarios = queryBuilderForm.NewFilters;
        this.txtScenario.Text = queryBuilderForm.ScenarioKey;
      }
    }

    private void MIRecordSetupForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.cancelBtn.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtScenario = new TextBox();
      this.label1 = new Label();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.labelPremium = new Label();
      this.txtPremium1 = new TextBox();
      this.txtSub = new TextBox();
      this.labelSubsequent = new Label();
      this.txtMonthly1 = new TextBox();
      this.label4 = new Label();
      this.txtMonthly2 = new TextBox();
      this.label5 = new Label();
      this.txtMonth2 = new TextBox();
      this.label6 = new Label();
      this.txtMonth1 = new TextBox();
      this.label7 = new Label();
      this.txtCutoff = new TextBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label10 = new Label();
      this.labelPrem = new Label();
      this.label1Sub = new Label();
      this.btnCreate = new Button();
      this.label13 = new Label();
      this.SuspendLayout();
      this.txtScenario.Location = new Point(132, 18);
      this.txtScenario.Multiline = true;
      this.txtScenario.Name = "txtScenario";
      this.txtScenario.ReadOnly = true;
      this.txtScenario.Size = new Size(323, 113);
      this.txtScenario.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(22, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Scenario";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(380, 294);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 20;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(299, 294);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 19;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.labelPremium.AutoSize = true;
      this.labelPremium.Location = new Point(22, 178);
      this.labelPremium.Name = "labelPremium";
      this.labelPremium.Size = new Size(64, 13);
      this.labelPremium.TabIndex = 21;
      this.labelPremium.Text = "1st Premium";
      this.txtPremium1.Location = new Point(132, 175);
      this.txtPremium1.MaxLength = 10;
      this.txtPremium1.Name = "txtPremium1";
      this.txtPremium1.Size = new Size(74, 20);
      this.txtPremium1.TabIndex = 1;
      this.txtPremium1.TextAlign = HorizontalAlignment.Right;
      this.txtPremium1.KeyPress += new KeyPressEventHandler(this.decimal_KeyPress);
      this.txtPremium1.Leave += new EventHandler(this.decimal_FieldLeave);
      this.txtSub.Location = new Point(368, 175);
      this.txtSub.MaxLength = 10;
      this.txtSub.Name = "txtSub";
      this.txtSub.Size = new Size(74, 20);
      this.txtSub.TabIndex = 2;
      this.txtSub.TextAlign = HorizontalAlignment.Right;
      this.txtSub.KeyPress += new KeyPressEventHandler(this.decimal_KeyPress);
      this.txtSub.Leave += new EventHandler(this.decimal_FieldLeave);
      this.labelSubsequent.AutoSize = true;
      this.labelSubsequent.Location = new Point(244, 178);
      this.labelSubsequent.Name = "labelSubsequent";
      this.labelSubsequent.Size = new Size(107, 13);
      this.labelSubsequent.TabIndex = 23;
      this.labelSubsequent.Text = "Subsequent Premium";
      this.txtMonthly1.Location = new Point(132, 201);
      this.txtMonthly1.MaxLength = 10;
      this.txtMonthly1.Name = "txtMonthly1";
      this.txtMonthly1.Size = new Size(74, 20);
      this.txtMonthly1.TabIndex = 3;
      this.txtMonthly1.TextAlign = HorizontalAlignment.Right;
      this.txtMonthly1.KeyPress += new KeyPressEventHandler(this.decimal_KeyPress);
      this.txtMonthly1.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(22, 204);
      this.label4.Name = "label4";
      this.label4.Size = new Size(76, 13);
      this.label4.TabIndex = 25;
      this.label4.Text = "1st Monthly MI";
      this.txtMonthly2.Location = new Point(132, 227);
      this.txtMonthly2.MaxLength = 10;
      this.txtMonthly2.Name = "txtMonthly2";
      this.txtMonthly2.Size = new Size(74, 20);
      this.txtMonthly2.TabIndex = 5;
      this.txtMonthly2.TextAlign = HorizontalAlignment.Right;
      this.txtMonthly2.KeyPress += new KeyPressEventHandler(this.decimal_KeyPress);
      this.txtMonthly2.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(245, 204);
      this.label5.Name = "label5";
      this.label5.Size = new Size(114, 13);
      this.label5.TabIndex = 27;
      this.label5.Text = "1st Monthly MI Months";
      this.txtMonth2.Location = new Point(368, 227);
      this.txtMonth2.MaxLength = 10;
      this.txtMonth2.Name = "txtMonth2";
      this.txtMonth2.Size = new Size(74, 20);
      this.txtMonth2.TabIndex = 6;
      this.txtMonth2.TextAlign = HorizontalAlignment.Right;
      this.txtMonth2.KeyPress += new KeyPressEventHandler(this.int_KeyPress);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(245, 230);
      this.label6.Name = "label6";
      this.label6.Size = new Size(118, 13);
      this.label6.TabIndex = 35;
      this.label6.Text = "2nd Monthly MI Months";
      this.txtMonth1.Location = new Point(368, 201);
      this.txtMonth1.MaxLength = 10;
      this.txtMonth1.Name = "txtMonth1";
      this.txtMonth1.Size = new Size(74, 20);
      this.txtMonth1.TabIndex = 4;
      this.txtMonth1.TextAlign = HorizontalAlignment.Right;
      this.txtMonth1.KeyPress += new KeyPressEventHandler(this.int_KeyPress);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(22, 230);
      this.label7.Name = "label7";
      this.label7.Size = new Size(80, 13);
      this.label7.TabIndex = 33;
      this.label7.Text = "2nd Monthly MI";
      this.txtCutoff.Location = new Point(132, 253);
      this.txtCutoff.MaxLength = 10;
      this.txtCutoff.Name = "txtCutoff";
      this.txtCutoff.Size = new Size(74, 20);
      this.txtCutoff.TabIndex = 7;
      this.txtCutoff.TextAlign = HorizontalAlignment.Right;
      this.txtCutoff.KeyPress += new KeyPressEventHandler(this.decimal_KeyPress);
      this.txtCutoff.Leave += new EventHandler(this.decimal_FieldLeave);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(22, 256);
      this.label9.Name = "label9";
      this.label9.Size = new Size(35, 13);
      this.label9.TabIndex = 29;
      this.label9.Text = "Cutoff";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(212, 204);
      this.label8.Name = "label8";
      this.label8.Size = new Size(15, 13);
      this.label8.TabIndex = 38;
      this.label8.Text = "%";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(212, 230);
      this.label10.Name = "label10";
      this.label10.Size = new Size(15, 13);
      this.label10.TabIndex = 39;
      this.label10.Text = "%";
      this.labelPrem.AutoSize = true;
      this.labelPrem.Location = new Point(212, 178);
      this.labelPrem.Name = "labelPrem";
      this.labelPrem.Size = new Size(15, 13);
      this.labelPrem.TabIndex = 40;
      this.labelPrem.Text = "%";
      this.label1Sub.AutoSize = true;
      this.label1Sub.Location = new Point(448, 178);
      this.label1Sub.Name = "label1Sub";
      this.label1Sub.Size = new Size(15, 13);
      this.label1Sub.TabIndex = 41;
      this.label1Sub.Text = "%";
      this.btnCreate.Location = new Point(380, 137);
      this.btnCreate.Name = "btnCreate";
      this.btnCreate.Size = new Size(75, 23);
      this.btnCreate.TabIndex = 42;
      this.btnCreate.Text = "&Create";
      this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(212, 256);
      this.label13.Name = "label13";
      this.label13.Size = new Size(15, 13);
      this.label13.TabIndex = 43;
      this.label13.Text = "%";
      this.AcceptButton = (IButtonControl) this.btnCreate;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(474, 331);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.btnCreate);
      this.Controls.Add((Control) this.label1Sub);
      this.Controls.Add((Control) this.labelPrem);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.txtMonth2);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtMonth1);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.txtCutoff);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.txtMonthly2);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtMonthly1);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtSub);
      this.Controls.Add((Control) this.labelSubsequent);
      this.Controls.Add((Control) this.txtPremium1);
      this.Controls.Add((Control) this.labelPremium);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtScenario);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MIRecordSetupForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "MI Scenario";
      this.KeyDown += new KeyEventHandler(this.MIRecordSetupForm_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
