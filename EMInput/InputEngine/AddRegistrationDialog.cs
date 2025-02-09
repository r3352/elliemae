// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddRegistrationDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddRegistrationDialog : Form
  {
    private LoanData loan;
    private PopupBusinessRules popupRules;
    private RegistrationLog latestLog;
    private IContainer components;
    private Button cancelBtn;
    private Button okBtn;
    private Label label1;
    private TextBox boxCreatedBy;
    private TextBox boxInvestor;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private TextBox boxReference;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private DatePicker dtExpiredDate;
    private StandardIconButton btnInvestor;
    private DatePicker dtCreatedDate;
    private TextBox txtRegOnReadOnly;

    public AddRegistrationDialog(LoanData loan, RegistrationLog latestLog)
    {
      this.loan = loan;
      this.latestLog = latestLog;
      this.InitializeComponent();
      if (this.latestLog != null)
      {
        this.txtRegOnReadOnly.Location = new Point(this.dtCreatedDate.Location.X, this.dtCreatedDate.Location.Y);
        this.txtRegOnReadOnly.Width = this.dtCreatedDate.Width;
        this.boxCreatedBy.Text = this.latestLog.RegisteredByName;
        this.dtCreatedDate.Value = this.latestLog.RegisteredDate;
        this.txtRegOnReadOnly.Text = this.latestLog.RegisteredDate.ToString("MM/dd/yyyy");
        this.dtExpiredDate.Value = this.latestLog.ExpiredDate;
        this.boxInvestor.Text = this.latestLog.InvestorName;
        this.boxReference.Text = this.latestLog.Reference;
        this.dtCreatedDate.Enabled = false;
        this.txtRegOnReadOnly.Visible = true;
        this.dtCreatedDate.Visible = false;
      }
      else
      {
        this.boxCreatedBy.Text = Session.UserInfo.FullName;
        this.dtCreatedDate.Value = DateTime.Today;
      }
      if (Session.UserInfo.IsSuperAdministrator())
        return;
      ResourceManager resources = new ResourceManager(typeof (AddRegistrationDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      if (this.latestLog == null)
        this.popupRules.SetBusinessRules((object) this.dtCreatedDate, "2823");
      this.popupRules.SetBusinessRules((object) this.dtExpiredDate, "2824");
      this.popupRules.SetBusinessRules((object) this.boxInvestor, "2825");
      this.popupRules.SetBusinessRules((object) this.boxReference, "2826");
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.dtCreatedDate.Value == DateTime.MinValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Registration Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.dtCreatedDate.Focus();
      }
      else
      {
        DateTime dateTime1 = this.dtExpiredDate.Value;
        DateTime dateTime2;
        if (dateTime1 != DateTime.MinValue)
        {
          int year1 = dateTime1.Year;
          dateTime2 = this.dtCreatedDate.Value;
          int year2 = dateTime2.Year;
          if (year1 >= year2)
          {
            int year3 = dateTime1.Year;
            dateTime2 = this.dtCreatedDate.Value;
            int year4 = dateTime2.Year;
            if (year3 == year4)
            {
              int month1 = dateTime1.Month;
              dateTime2 = this.dtCreatedDate.Value;
              int month2 = dateTime2.Month;
              if (month1 < month2)
                goto label_9;
            }
            int year5 = dateTime1.Year;
            dateTime2 = this.dtCreatedDate.Value;
            int year6 = dateTime2.Year;
            if (year5 == year6)
            {
              int month3 = dateTime1.Month;
              dateTime2 = this.dtCreatedDate.Value;
              int month4 = dateTime2.Month;
              if (month3 == month4)
              {
                int day1 = dateTime1.Day;
                dateTime2 = this.dtCreatedDate.Value;
                int day2 = dateTime2.Day;
                if (day1 >= day2)
                  goto label_10;
              }
              else
                goto label_10;
            }
            else
              goto label_10;
          }
label_9:
          int num = (int) Utils.Dialog((IWin32Window) this, "The Registration Date cannot be greater than the Expiration Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.dtExpiredDate.Focus();
          return;
        }
label_10:
        RegistrationLog rec;
        if (this.latestLog == null)
        {
          rec = new RegistrationLog();
          rec.RegisteredByID = Session.UserInfo.Userid;
          rec.RegisteredByName = this.boxCreatedBy.Text;
          rec.RegisteredDate = this.dtCreatedDate.Value;
          rec.ExpiredDate = dateTime1;
          rec.InvestorName = this.boxInvestor.Text.Trim();
          rec.Reference = this.boxReference.Text.Trim();
          try
          {
            this.loan.GetLogList().AddRecord((LogRecordBase) rec);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The new registration information cannot be added. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        else
        {
          rec = this.latestLog;
          rec.ExpiredDate = dateTime1;
          rec.InvestorName = this.boxInvestor.Text.Trim();
          rec.Reference = this.boxReference.Text.Trim();
        }
        this.loan.SetField("2827", "Y");
        this.loan.SetField("2822", rec.RegisteredByName);
        this.loan.SetField("2825", rec.InvestorName);
        LoanData loan1 = this.loan;
        dateTime2 = rec.RegisteredDate;
        string val1 = dateTime2.ToString("MM/dd/yyyy");
        loan1.SetField("2823", val1);
        if (dateTime1 != DateTime.MinValue)
        {
          LoanData loan2 = this.loan;
          dateTime2 = rec.ExpiredDate;
          string val2 = dateTime2.ToString("MM/dd/yyyy");
          loan2.SetField("2824", val2);
        }
        else
          this.loan.SetField("2824", "");
        this.loan.SetField("2826", rec.Reference);
        this.loan.SetField("2828", rec.RegisteredByID);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void textBoxInvestorName_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.pictureBoxContact_Click((object) null, (EventArgs) null);
    }

    private void pictureBoxContact_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact[RolodexField.Company] = this.boxInvestor.Text.Trim();
      bool allowGoto = true;
      if (this.loan.IsInFindFieldForm || this.loan.IsTemplate)
        allowGoto = false;
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Investor", this.boxInvestor.Text.Trim(), "", rxContact, CRMRoleType.NotFound, allowGoto, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        else
          this.boxInvestor.Text = rxBusinessContact.RxContactRecord[RolodexField.Company];
      }
    }

    private void AddRegistrationDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddRegistrationDialog));
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.boxCreatedBy = new TextBox();
      this.boxInvestor = new TextBox();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.boxReference = new TextBox();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.dtExpiredDate = new DatePicker();
      this.btnInvestor = new StandardIconButton();
      this.dtCreatedDate = new DatePicker();
      this.txtRegOnReadOnly = new TextBox();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.btnInvestor).BeginInit();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(236, 136);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(152, 136);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(73, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Registered By";
      this.boxCreatedBy.Location = new Point(128, 10);
      this.boxCreatedBy.Name = "boxCreatedBy";
      this.boxCreatedBy.ReadOnly = true;
      this.boxCreatedBy.Size = new Size(184, 20);
      this.boxCreatedBy.TabIndex = 0;
      this.boxCreatedBy.TabStop = false;
      this.boxInvestor.Location = new Point(128, 78);
      this.boxInvestor.Name = "boxInvestor";
      this.boxInvestor.Size = new Size(162, 20);
      this.boxInvestor.TabIndex = 2;
      this.boxInvestor.MouseDown += new MouseEventHandler(this.textBoxInvestorName_MouseDown);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(75, 13);
      this.label2.TabIndex = 17;
      this.label2.Text = "Registered On";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(100, 13);
      this.label3.TabIndex = 18;
      this.label3.Text = "Registration Expires";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 82);
      this.label4.Name = "label4";
      this.label4.Size = new Size(76, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "Investor Name";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 104);
      this.label5.Name = "label5";
      this.label5.Size = new Size(97, 13);
      this.label5.TabIndex = 152;
      this.label5.Text = "Reference Number";
      this.boxReference.Location = new Point(128, 101);
      this.boxReference.Multiline = true;
      this.boxReference.Name = "boxReference";
      this.boxReference.Size = new Size(184, 20);
      this.boxReference.TabIndex = 3;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(40, 122);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 154;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.BackColor = Color.Transparent;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(8, 126);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 153;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.dtExpiredDate.BackColor = SystemColors.Window;
      this.dtExpiredDate.Location = new Point(128, 55);
      this.dtExpiredDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtExpiredDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtExpiredDate.Name = "dtExpiredDate";
      this.dtExpiredDate.Size = new Size(184, 21);
      this.dtExpiredDate.TabIndex = 1;
      this.dtExpiredDate.ToolTip = "";
      this.dtExpiredDate.Value = new DateTime(0L);
      this.btnInvestor.BackColor = Color.Transparent;
      this.btnInvestor.Location = new Point(295, 80);
      this.btnInvestor.Name = "btnInvestor";
      this.btnInvestor.Size = new Size(16, 16);
      this.btnInvestor.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnInvestor.TabIndex = 155;
      this.btnInvestor.TabStop = false;
      this.btnInvestor.Click += new EventHandler(this.pictureBoxContact_Click);
      this.dtCreatedDate.BackColor = SystemColors.Window;
      this.dtCreatedDate.Location = new Point(128, 32);
      this.dtCreatedDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtCreatedDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtCreatedDate.Name = "dtCreatedDate";
      this.dtCreatedDate.Size = new Size(184, 21);
      this.dtCreatedDate.TabIndex = 156;
      this.dtCreatedDate.ToolTip = "";
      this.dtCreatedDate.Value = new DateTime(0L);
      this.txtRegOnReadOnly.Location = new Point(8, 145);
      this.txtRegOnReadOnly.Name = "txtRegOnReadOnly";
      this.txtRegOnReadOnly.ReadOnly = true;
      this.txtRegOnReadOnly.Size = new Size(138, 20);
      this.txtRegOnReadOnly.TabIndex = 157;
      this.txtRegOnReadOnly.TabStop = false;
      this.txtRegOnReadOnly.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(327, 176);
      this.Controls.Add((Control) this.txtRegOnReadOnly);
      this.Controls.Add((Control) this.dtCreatedDate);
      this.Controls.Add((Control) this.btnInvestor);
      this.Controls.Add((Control) this.dtExpiredDate);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.boxReference);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.boxInvestor);
      this.Controls.Add((Control) this.boxCreatedBy);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddRegistrationDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Registration";
      this.KeyPress += new KeyPressEventHandler(this.AddRegistrationDialog_KeyPress);
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.btnInvestor).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
