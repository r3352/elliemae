// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompDetailDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOCompDetailDialog : Form
  {
    private LoanCompPlan lcp;
    private LoanCompHistory loanCompHistory;
    private LoanCompDefaultPlan loanCompDefaultPlan;
    private IContainer components;
    private TextBox txtActivationDate;
    private TextBox txtTriggerField;
    private TextBox txtMaxAmt;
    private Label lblMaxAmount;
    private TextBox txtMinAmt;
    private Label lblMinAmount;
    private TextBox txtAmount2;
    private Label lblAmount2;
    private ComboBox cmbRounding;
    private ComboBox cmbAmount;
    private TextBox txtAmount;
    private TextBox textMinTerm;
    private ComboBox cmbType;
    private TextBox txtDescription;
    private TextBox txtName;
    private Label lblTriggerField;
    private Label label1;
    private Label lblAmount;
    private Label lblMinTerm;
    private Label lblActivation;
    private Label lblType;
    private Label lblDescription;
    private Label lblName;
    private Button btnOK;
    private Label label2;

    public LOCompDetailDialog(
      LoanCompPlan lcp,
      LoanCompDefaultPlan loanCompDefaultPlan,
      LoanCompHistory loanCompHistory)
    {
      this.lcp = lcp;
      this.loanCompHistory = loanCompHistory;
      this.loanCompDefaultPlan = loanCompDefaultPlan;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.txtName.Text = this.lcp.Name;
      this.txtDescription.Text = this.lcp.Description;
      this.cmbType.SelectedIndex = this.lcp.PlanType == LoanCompPlanType.LoanOfficer ? 1 : (this.lcp.PlanType == LoanCompPlanType.Broker ? 2 : (this.lcp.PlanType == LoanCompPlanType.Both ? 3 : 0));
      this.txtActivationDate.Text = !(this.loanCompHistory.StartDate.Date != DateTime.MinValue) || !(this.loanCompHistory.StartDate.Date != DateTime.MaxValue) ? "" : this.loanCompHistory.StartDate.ToString("MM/dd/yyyy");
      this.textMinTerm.Text = this.lcp.MinTermDays != 0 ? this.lcp.MinTermDays.ToString() : "";
      this.txtAmount.Text = this.lcp.PercentAmt != 0M ? this.lcp.PercentAmt.ToString("N5") : "";
      this.cmbAmount.SelectedIndex = this.lcp.PercentAmtBase;
      this.cmbRounding.SelectedIndex = this.lcp.RoundingMethod > 1 ? 1 : 0;
      this.txtAmount2.Text = this.lcp.DollarAmount != 0M ? this.lcp.DollarAmount.ToString("N2") : "";
      this.txtMinAmt.Text = this.lcp.MinDollarAmount != 0M ? this.lcp.MinDollarAmount.ToString("N2") : "";
      this.txtMaxAmt.Text = this.lcp.MaxDollarAmount != 0M ? this.lcp.MaxDollarAmount.ToString("N2") : "";
      this.txtTriggerField.Text = this.loanCompDefaultPlan != null ? this.loanCompDefaultPlan.TriggerField : "";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtActivationDate = new TextBox();
      this.txtTriggerField = new TextBox();
      this.txtMaxAmt = new TextBox();
      this.lblMaxAmount = new Label();
      this.txtMinAmt = new TextBox();
      this.lblMinAmount = new Label();
      this.txtAmount2 = new TextBox();
      this.lblAmount2 = new Label();
      this.cmbRounding = new ComboBox();
      this.cmbAmount = new ComboBox();
      this.txtAmount = new TextBox();
      this.textMinTerm = new TextBox();
      this.cmbType = new ComboBox();
      this.txtDescription = new TextBox();
      this.txtName = new TextBox();
      this.lblTriggerField = new Label();
      this.label1 = new Label();
      this.lblAmount = new Label();
      this.lblMinTerm = new Label();
      this.lblActivation = new Label();
      this.lblType = new Label();
      this.lblDescription = new Label();
      this.lblName = new Label();
      this.btnOK = new Button();
      this.label2 = new Label();
      this.SuspendLayout();
      this.txtActivationDate.Enabled = false;
      this.txtActivationDate.Location = new Point(146, 90);
      this.txtActivationDate.Name = "txtActivationDate";
      this.txtActivationDate.ReadOnly = true;
      this.txtActivationDate.Size = new Size(100, 20);
      this.txtActivationDate.TabIndex = 33;
      this.txtActivationDate.TabStop = false;
      this.txtTriggerField.Location = new Point(146, 220);
      this.txtTriggerField.MaxLength = 50;
      this.txtTriggerField.Name = "txtTriggerField";
      this.txtTriggerField.ReadOnly = true;
      this.txtTriggerField.Size = new Size(100, 20);
      this.txtTriggerField.TabIndex = 44;
      this.txtMaxAmt.Location = new Point(145, 194);
      this.txtMaxAmt.MaxLength = 15;
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.ReadOnly = true;
      this.txtMaxAmt.Size = new Size(100, 20);
      this.txtMaxAmt.TabIndex = 43;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.lblMaxAmount.AutoSize = true;
      this.lblMaxAmount.Location = new Point(10, 197);
      this.lblMaxAmount.Name = "lblMaxAmount";
      this.lblMaxAmount.Size = new Size(90, 13);
      this.lblMaxAmount.TabIndex = 47;
      this.lblMaxAmount.Text = "Maximum $";
      this.txtMinAmt.Location = new Point(145, 168);
      this.txtMinAmt.MaxLength = 15;
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.ReadOnly = true;
      this.txtMinAmt.Size = new Size(100, 20);
      this.txtMinAmt.TabIndex = 42;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.lblMinAmount.AutoSize = true;
      this.lblMinAmount.Location = new Point(10, 171);
      this.lblMinAmount.Name = "lblMinAmount";
      this.lblMinAmount.Size = new Size(87, 13);
      this.lblMinAmount.TabIndex = 46;
      this.lblMinAmount.Text = "Minimum $";
      this.txtAmount2.Location = new Point(146, 142);
      this.txtAmount2.MaxLength = 15;
      this.txtAmount2.Name = "txtAmount2";
      this.txtAmount2.ReadOnly = true;
      this.txtAmount2.Size = new Size(99, 20);
      this.txtAmount2.TabIndex = 41;
      this.txtAmount2.TextAlign = HorizontalAlignment.Right;
      this.lblAmount2.AutoSize = true;
      this.lblAmount2.Location = new Point(10, 145);
      this.lblAmount2.Name = "lblAmount2";
      this.lblAmount2.Size = new Size(52, 13);
      this.lblAmount2.TabIndex = 45;
      this.lblAmount2.Text = "$ Amount";
      this.cmbRounding.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRounding.Enabled = false;
      this.cmbRounding.FormattingEnabled = true;
      this.cmbRounding.Items.AddRange(new object[2]
      {
        (object) "",
        (object) "To Nearest $"
      });
      this.cmbRounding.Location = new Point(377, 90);
      this.cmbRounding.Name = "cmbRounding";
      this.cmbRounding.Size = new Size(115, 21);
      this.cmbRounding.TabIndex = 34;
      this.cmbAmount.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAmount.Enabled = false;
      this.cmbAmount.FormattingEnabled = true;
      this.cmbAmount.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Total Loan",
        (object) "Base Loan"
      });
      this.cmbAmount.Location = new Point(265, 116);
      this.cmbAmount.Name = "cmbAmount";
      this.cmbAmount.Size = new Size(101, 21);
      this.cmbAmount.TabIndex = 39;
      this.txtAmount.Location = new Point(146, 116);
      this.txtAmount.MaxLength = 15;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.ReadOnly = true;
      this.txtAmount.Size = new Size(99, 20);
      this.txtAmount.TabIndex = 36;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.textMinTerm.Location = new Point(377, 64);
      this.textMinTerm.MaxLength = 5;
      this.textMinTerm.Name = "textMinTerm";
      this.textMinTerm.ReadOnly = true;
      this.textMinTerm.Size = new Size(115, 20);
      this.textMinTerm.TabIndex = 30;
      this.textMinTerm.TextAlign = HorizontalAlignment.Right;
      this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbType.Enabled = false;
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Loan Officer",
        (object) "Broker",
        (object) "Both"
      });
      this.cmbType.Location = new Point(146, 64);
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new Size(99, 21);
      this.cmbType.TabIndex = 28;
      this.txtDescription.Location = new Point(146, 38);
      this.txtDescription.MaxLength = 256;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new Size(345, 20);
      this.txtDescription.TabIndex = 27;
      this.txtName.Location = new Point(146, 12);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(345, 20);
      this.txtName.TabIndex = 25;
      this.lblTriggerField.AutoSize = true;
      this.lblTriggerField.Location = new Point(10, 223);
      this.lblTriggerField.Name = "lblTriggerField";
      this.lblTriggerField.Size = new Size(68, 13);
      this.lblTriggerField.TabIndex = 40;
      this.lblTriggerField.Text = "Trigger Basis";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(262, 93);
      this.label1.Name = "label1";
      this.label1.Size = new Size(53, 13);
      this.label1.TabIndex = 38;
      this.label1.Text = "Rounding";
      this.lblAmount.AutoSize = true;
      this.lblAmount.Location = new Point(10, 119);
      this.lblAmount.Name = "lblAmount";
      this.lblAmount.Size = new Size(54, 13);
      this.lblAmount.TabIndex = 37;
      this.lblAmount.Text = "% Amount";
      this.lblMinTerm.AutoSize = true;
      this.lblMinTerm.Location = new Point(262, 68);
      this.lblMinTerm.Name = "lblMinTerm";
      this.lblMinTerm.Size = new Size(112, 13);
      this.lblMinTerm.TabIndex = 35;
      this.lblMinTerm.Text = "Minimum Term # Days";
      this.lblActivation.AutoSize = true;
      this.lblActivation.Location = new Point(10, 93);
      this.lblActivation.Name = "lblActivation";
      this.lblActivation.Size = new Size(55, 13);
      this.lblActivation.TabIndex = 32;
      this.lblActivation.Text = "Start Date";
      this.lblType.AutoSize = true;
      this.lblType.Location = new Point(10, 68);
      this.lblType.Name = "lblType";
      this.lblType.Size = new Size(131, 13);
      this.lblType.TabIndex = 31;
      this.lblType.Text = "Loan Officer/Broker Value";
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(10, 41);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(60, 13);
      this.lblDescription.TabIndex = 29;
      this.lblDescription.Text = "Description";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(10, 15);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(35, 13);
      this.lblName.TabIndex = 26;
      this.lblName.Text = "Name";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(416, 261);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 51;
      this.btnOK.Text = "&OK";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(247, 119);
      this.label2.Name = "label2";
      this.label2.Size = new Size(16, 13);
      this.label2.TabIndex = 52;
      this.label2.Text = "of";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(503, 292);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtActivationDate);
      this.Controls.Add((Control) this.txtTriggerField);
      this.Controls.Add((Control) this.txtMaxAmt);
      this.Controls.Add((Control) this.lblMaxAmount);
      this.Controls.Add((Control) this.txtMinAmt);
      this.Controls.Add((Control) this.lblMinAmount);
      this.Controls.Add((Control) this.txtAmount2);
      this.Controls.Add((Control) this.lblAmount2);
      this.Controls.Add((Control) this.cmbRounding);
      this.Controls.Add((Control) this.cmbAmount);
      this.Controls.Add((Control) this.txtAmount);
      this.Controls.Add((Control) this.textMinTerm);
      this.Controls.Add((Control) this.cmbType);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.lblTriggerField);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblAmount);
      this.Controls.Add((Control) this.lblMinTerm);
      this.Controls.Add((Control) this.lblActivation);
      this.Controls.Add((Control) this.lblType);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.lblName);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LOCompDetailDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "LO Comp Details";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
