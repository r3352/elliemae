// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.DeliveryMethodDetailDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class DeliveryMethodDetailDialog : Form
  {
    private Sessions.Session session;
    private MasterCommitmentDeliveryInfo deliveryInfo;
    private IContainer components;
    private Panel pnlTaskDetails;
    private Label label1;
    private Panel pnlButtons;
    private Button btnCancel;
    private Button btnSave;
    private ComboBox cmbRateSheet;
    private Label label12;
    private TextBox txtDeliveryDays;
    private Label label10;
    private TextBox txtCommitmentAmount;
    private Label label9;
    private DatePicker dtEndDate;
    private TextBox txtTolerance;
    private DatePicker dtStartDate;
    private TextBox txtAvailableAmount;
    private Label label5;
    private Label label3;
    private Label label4;
    private Label label6;
    private ComboBox cboTerm;
    private Label label7;
    private Label label8;
    private ComboBox comboBox1;

    public DeliveryMethodDetailDialog(
      MasterCommitmentDeliveryInfo deliveryInfo,
      Sessions.Session session)
    {
      this.session = session;
      this.deliveryInfo = deliveryInfo;
      this.InitializeComponent();
    }

    private void initForm()
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlTaskDetails = new Panel();
      this.label1 = new Label();
      this.pnlButtons = new Panel();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.cmbRateSheet = new ComboBox();
      this.label12 = new Label();
      this.txtDeliveryDays = new TextBox();
      this.label10 = new Label();
      this.txtCommitmentAmount = new TextBox();
      this.label9 = new Label();
      this.dtEndDate = new DatePicker();
      this.txtTolerance = new TextBox();
      this.dtStartDate = new DatePicker();
      this.txtAvailableAmount = new TextBox();
      this.label5 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label6 = new Label();
      this.cboTerm = new ComboBox();
      this.label7 = new Label();
      this.label8 = new Label();
      this.comboBox1 = new ComboBox();
      this.pnlTaskDetails.SuspendLayout();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      this.pnlTaskDetails.Controls.Add((Control) this.comboBox1);
      this.pnlTaskDetails.Controls.Add((Control) this.cmbRateSheet);
      this.pnlTaskDetails.Controls.Add((Control) this.label12);
      this.pnlTaskDetails.Controls.Add((Control) this.txtDeliveryDays);
      this.pnlTaskDetails.Controls.Add((Control) this.label10);
      this.pnlTaskDetails.Controls.Add((Control) this.txtCommitmentAmount);
      this.pnlTaskDetails.Controls.Add((Control) this.label9);
      this.pnlTaskDetails.Controls.Add((Control) this.dtEndDate);
      this.pnlTaskDetails.Controls.Add((Control) this.txtTolerance);
      this.pnlTaskDetails.Controls.Add((Control) this.dtStartDate);
      this.pnlTaskDetails.Controls.Add((Control) this.txtAvailableAmount);
      this.pnlTaskDetails.Controls.Add((Control) this.label5);
      this.pnlTaskDetails.Controls.Add((Control) this.label3);
      this.pnlTaskDetails.Controls.Add((Control) this.label4);
      this.pnlTaskDetails.Controls.Add((Control) this.label6);
      this.pnlTaskDetails.Controls.Add((Control) this.cboTerm);
      this.pnlTaskDetails.Controls.Add((Control) this.label7);
      this.pnlTaskDetails.Controls.Add((Control) this.label8);
      this.pnlTaskDetails.Controls.Add((Control) this.pnlButtons);
      this.pnlTaskDetails.Controls.Add((Control) this.label1);
      this.pnlTaskDetails.Dock = DockStyle.Fill;
      this.pnlTaskDetails.Location = new Point(0, 0);
      this.pnlTaskDetails.Name = "pnlTaskDetails";
      this.pnlTaskDetails.Size = new Size(351, 313);
      this.pnlTaskDetails.TabIndex = 24;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(111, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Delivery Method Type";
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnSave);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 269);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(351, 44);
      this.pnlButtons.TabIndex = 45;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(264, 10);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(183, 10);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 19;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.cmbRateSheet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRateSheet.FormattingEnabled = true;
      this.cmbRateSheet.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Monthly",
        (object) "Quarterly",
        (object) "Annually"
      });
      this.cmbRateSheet.Location = new Point(158, 39);
      this.cmbRateSheet.Name = "cmbRateSheet";
      this.cmbRateSheet.Size = new Size(146, 21);
      this.cmbRateSheet.TabIndex = 62;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(8, 45);
      this.label12.Name = "label12";
      this.label12.Size = new Size(61, 13);
      this.label12.TabIndex = 63;
      this.label12.Text = "Rate Sheet";
      this.txtDeliveryDays.Location = new Point(159, 62);
      this.txtDeliveryDays.MaxLength = 12;
      this.txtDeliveryDays.Name = "txtDeliveryDays";
      this.txtDeliveryDays.Size = new Size(145, 20);
      this.txtDeliveryDays.TabIndex = 60;
      this.txtDeliveryDays.TextAlign = HorizontalAlignment.Right;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 66);
      this.label10.Name = "label10";
      this.label10.Size = new Size(72, 13);
      this.label10.TabIndex = 61;
      this.label10.Text = "Delivery Days";
      this.txtCommitmentAmount.Location = new Point(159, 84);
      this.txtCommitmentAmount.MaxLength = 12;
      this.txtCommitmentAmount.Name = "txtCommitmentAmount";
      this.txtCommitmentAmount.Size = new Size(145, 20);
      this.txtCommitmentAmount.TabIndex = 58;
      this.txtCommitmentAmount.TextAlign = HorizontalAlignment.Right;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 88);
      this.label9.Name = "label9";
      this.label9.Size = new Size(138, 13);
      this.label9.TabIndex = 59;
      this.label9.Text = "Master Commitment Amount";
      this.dtEndDate.BackColor = SystemColors.Window;
      this.dtEndDate.Location = new Point(159, 175);
      this.dtEndDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtEndDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtEndDate.Name = "dtEndDate";
      this.dtEndDate.Size = new Size(145, 21);
      this.dtEndDate.TabIndex = 51;
      this.dtEndDate.ToolTip = "";
      this.dtEndDate.Value = new DateTime(0L);
      this.txtTolerance.Location = new Point(159, 128);
      this.txtTolerance.MaxLength = 6;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.Size = new Size(68, 20);
      this.txtTolerance.TabIndex = 49;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.dtStartDate.BackColor = SystemColors.Window;
      this.dtStartDate.Location = new Point(159, 150);
      this.dtStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtStartDate.Name = "dtStartDate";
      this.dtStartDate.Size = new Size(145, 21);
      this.dtStartDate.TabIndex = 50;
      this.dtStartDate.ToolTip = "";
      this.dtStartDate.Value = new DateTime(0L);
      this.txtAvailableAmount.BackColor = SystemColors.Control;
      this.txtAvailableAmount.Enabled = false;
      this.txtAvailableAmount.Location = new Point(159, 106);
      this.txtAvailableAmount.MaxLength = 12;
      this.txtAvailableAmount.Name = "txtAvailableAmount";
      this.txtAvailableAmount.Size = new Size(145, 20);
      this.txtAvailableAmount.TabIndex = 47;
      this.txtAvailableAmount.TextAlign = HorizontalAlignment.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 133);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 13);
      this.label5.TabIndex = 52;
      this.label5.Text = "Tolerance";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(230, 131);
      this.label3.Name = "label3";
      this.label3.Size = new Size(15, 13);
      this.label3.TabIndex = 57;
      this.label3.Text = "%";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 110);
      this.label4.Name = "label4";
      this.label4.Size = new Size(89, 13);
      this.label4.TabIndex = 48;
      this.label4.Text = "Available Amount";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 157);
      this.label6.Name = "label6";
      this.label6.Size = new Size(75, 13);
      this.label6.TabIndex = 54;
      this.label6.Text = "Effective Date";
      this.cboTerm.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTerm.FormattingEnabled = true;
      this.cboTerm.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Monthly",
        (object) "Quarterly",
        (object) "Annually"
      });
      this.cboTerm.Location = new Point(195, 198);
      this.cboTerm.Name = "cboTerm";
      this.cboTerm.Size = new Size(109, 21);
      this.cboTerm.TabIndex = 53;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 182);
      this.label7.Name = "label7";
      this.label7.Size = new Size(62, 13);
      this.label7.TabIndex = 55;
      this.label7.Text = "Expire Date";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(9, 206);
      this.label8.Name = "label8";
      this.label8.Size = new Size(31, 13);
      this.label8.TabIndex = 56;
      this.label8.Text = "Term";
      this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Monthly",
        (object) "Quarterly",
        (object) "Annually"
      });
      this.comboBox1.Location = new Point(159, 16);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new Size(146, 21);
      this.comboBox1.TabIndex = 64;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(351, 313);
      this.Controls.Add((Control) this.pnlTaskDetails);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "dlgDeliveryMethodDetail";
      this.ShowInTaskbar = false;
      this.Text = "Delivery Method";
      this.pnlTaskDetails.ResumeLayout(false);
      this.pnlTaskDetails.PerformLayout();
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
