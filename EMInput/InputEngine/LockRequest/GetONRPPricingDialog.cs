// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockRequest.GetONRPPricingDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.LockRequest
{
  public class GetONRPPricingDialog : Form
  {
    private IClientSession clientSession;
    private SessionObjects sessionObjs;
    private LoanDataMgr loanDataMgr;
    private IContainer components;
    private Button buttonContinue;
    private Button buttonCancel;
    private BorderPanel borderPanel1;
    private DatePicker datePickerLockDate;
    private ComboBox cmbAMPM;
    private Label labelLockDate;
    private Label label12;
    private Label labelLockReqTime;
    private ComboBox cmbMinute;
    private ComboBox cmbHour;

    public string LockDate { get; set; }

    public string LockTime { get; set; }

    public GetONRPPricingDialog(
      IClientSession clientSession,
      SessionObjects sessionObjs,
      LoanDataMgr loanDataMgr)
    {
      this.clientSession = clientSession;
      this.sessionObjs = sessionObjs;
      this.loanDataMgr = loanDataMgr;
      this.InitializeComponent();
    }

    private void GetONRPPricingDialog_Load(object sender, EventArgs e)
    {
      this.cmbHour.SelectedIndex = 0;
      this.cmbMinute.SelectedIndex = 0;
      this.cmbAMPM.SelectedIndex = 0;
    }

    private bool ValidateForm()
    {
      DateTime serverEasternTime = new EncompassLockDeskHoursHelper(this.clientSession, this.sessionObjs, this.loanDataMgr).GetServerEasternTime();
      if (this.datePickerLockDate.Value == DateTime.MinValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A valid ONRP Lock Date is required. Please correct.");
        return false;
      }
      if (this.cmbHour.Text.Trim() == "" || this.cmbMinute.Text.Trim() == "" || this.cmbAMPM.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A valid ONRP Lock Time is required. Please correct.");
        return false;
      }
      int num1 = Utils.ParseInt((object) this.cmbHour.Text, 0);
      if (((!(this.cmbAMPM.Text == "PM") ? this.datePickerLockDate.Value.AddHours(num1 < 12 ? (double) num1 : 0.0).AddMinutes((double) Utils.ParseInt((object) this.cmbMinute.Text, 0)) : this.datePickerLockDate.Value.AddHours(num1 < 12 ? (double) (num1 + 12) : (double) num1).AddMinutes((double) Utils.ParseInt((object) this.cmbMinute.Text, 0))) - serverEasternTime).TotalDays <= 0.0)
        return true;
      int num2 = (int) Utils.Dialog((IWin32Window) this, "A Future ONRP Lock Date and Time are not acceptable for the Get ONRP Pricing feature.");
      return false;
    }

    private void buttonContinue_Click(object sender, EventArgs e)
    {
      if (!this.ValidateForm())
      {
        this.DialogResult = DialogResult.None;
      }
      else
      {
        DateTime result;
        if (!DateTime.TryParse(this.datePickerLockDate.Value.ToShortDateString() + " " + this.cmbHour.Text.Trim().ToString() + ":" + this.cmbMinute.Text.Trim().ToString() + " " + this.cmbAMPM.Text.Trim().ToString(), out result))
          return;
        this.LockDate = result.ToShortDateString();
        this.LockTime = result.ToString("h:mm:ss tt");
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.buttonContinue = new Button();
      this.buttonCancel = new Button();
      this.borderPanel1 = new BorderPanel();
      this.labelLockDate = new Label();
      this.labelLockReqTime = new Label();
      this.cmbHour = new ComboBox();
      this.cmbMinute = new ComboBox();
      this.label12 = new Label();
      this.cmbAMPM = new ComboBox();
      this.datePickerLockDate = new DatePicker();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.buttonContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonContinue.DialogResult = DialogResult.OK;
      this.buttonContinue.Location = new Point(193, 114);
      this.buttonContinue.Name = "buttonContinue";
      this.buttonContinue.Size = new Size(75, 23);
      this.buttonContinue.TabIndex = 1;
      this.buttonContinue.Text = "Continue";
      this.buttonContinue.UseVisualStyleBackColor = true;
      this.buttonContinue.Click += new EventHandler(this.buttonContinue_Click);
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(274, 114);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.borderPanel1.BackColor = SystemColors.Window;
      this.borderPanel1.Controls.Add((Control) this.datePickerLockDate);
      this.borderPanel1.Controls.Add((Control) this.cmbAMPM);
      this.borderPanel1.Controls.Add((Control) this.labelLockDate);
      this.borderPanel1.Controls.Add((Control) this.label12);
      this.borderPanel1.Controls.Add((Control) this.labelLockReqTime);
      this.borderPanel1.Controls.Add((Control) this.cmbMinute);
      this.borderPanel1.Controls.Add((Control) this.cmbHour);
      this.borderPanel1.Location = new Point(12, 12);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(337, 90);
      this.borderPanel1.TabIndex = 3;
      this.datePickerLockDate.BackColor = SystemColors.Window;
      this.datePickerLockDate.Location = new Point(140, 16);
      this.datePickerLockDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datePickerLockDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datePickerLockDate.Name = "datePickerLockDate";
      this.datePickerLockDate.Size = new Size(132, 21);
      this.datePickerLockDate.TabIndex = 57;
      this.datePickerLockDate.ToolTip = "";
      this.datePickerLockDate.Value = new DateTime(0L);
      this.cmbAMPM.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAMPM.FormattingEnabled = true;
      this.cmbAMPM.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "AM",
        (object) "PM"
      });
      this.cmbAMPM.Location = new Point(232, 42);
      this.cmbAMPM.Name = "cmbAMPM";
      this.cmbAMPM.Size = new Size(40, 21);
      this.cmbAMPM.TabIndex = 56;
      this.labelLockDate.AutoSize = true;
      this.labelLockDate.Location = new Point(13, 22);
      this.labelLockDate.Name = "labelLockDate";
      this.labelLockDate.Size = new Size(91, 13);
      this.labelLockDate.TabIndex = 0;
      this.labelLockDate.Text = "ONRP Lock Date";
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label12.Location = new Point(278, 45);
      this.label12.Name = "label12";
      this.label12.Size = new Size(28, 13);
      this.label12.TabIndex = 55;
      this.label12.Text = "EST";
      this.labelLockReqTime.AutoSize = true;
      this.labelLockReqTime.Location = new Point(13, 46);
      this.labelLockReqTime.Name = "labelLockReqTime";
      this.labelLockReqTime.Size = new Size(91, 13);
      this.labelLockReqTime.TabIndex = 1;
      this.labelLockReqTime.Text = "ONRP Lock Time";
      this.cmbMinute.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbMinute.FormattingEnabled = true;
      this.cmbMinute.Items.AddRange(new object[61]
      {
        (object) "",
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29",
        (object) "30",
        (object) "31",
        (object) "32",
        (object) "33",
        (object) "34",
        (object) "35",
        (object) "36",
        (object) "37",
        (object) "38",
        (object) "39",
        (object) "40",
        (object) "41",
        (object) "42",
        (object) "43",
        (object) "44",
        (object) "45",
        (object) "46",
        (object) "47",
        (object) "48",
        (object) "49",
        (object) "50",
        (object) "51",
        (object) "52",
        (object) "53",
        (object) "54",
        (object) "55",
        (object) "56",
        (object) "57",
        (object) "58",
        (object) "59"
      });
      this.cmbMinute.Location = new Point(186, 42);
      this.cmbMinute.Name = "cmbMinute";
      this.cmbMinute.Size = new Size(40, 21);
      this.cmbMinute.TabIndex = 54;
      this.cmbHour.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbHour.FormattingEnabled = true;
      this.cmbHour.Items.AddRange(new object[13]
      {
        (object) "",
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "6",
        (object) "7",
        (object) "8",
        (object) "9",
        (object) "10",
        (object) "11",
        (object) "12"
      });
      this.cmbHour.Location = new Point(140, 42);
      this.cmbHour.Name = "cmbHour";
      this.cmbHour.Size = new Size(40, 21);
      this.cmbHour.TabIndex = 53;
      this.AcceptButton = (IButtonControl) this.buttonContinue;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(361, 148);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonContinue);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GetONRPPricingDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Get ONRP Pricing";
      this.Load += new EventHandler(this.GetONRPPricingDialog_Load);
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
