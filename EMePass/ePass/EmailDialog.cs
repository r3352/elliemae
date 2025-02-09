// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.EmailDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using GNETPARSERXLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class EmailDialog : Form, IHelp
  {
    private const string className = "EmailDialog";
    private static readonly string sw = Tracing.SwEpass;
    private string pdfFile;
    private string ssn;
    private Button emailToBtn;
    private TextBox emailToTxt;
    private Button emailCCBtn;
    private TextBox emailCCTxt;
    private Label subjectLbl;
    private TextBox subjectTxt;
    private Label messageLbl;
    private TextBox messageTxt;
    private Label noteLbl;
    private Button okBtn;
    private Button cancelBtn;
    private System.ComponentModel.Container components;

    public EmailDialog(string pdfFile, string ssn)
    {
      this.InitializeComponent();
      this.pdfFile = pdfFile;
      this.ssn = ssn.Substring(ssn.Length - 4);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.emailToBtn = new Button();
      this.emailToTxt = new TextBox();
      this.emailCCBtn = new Button();
      this.emailCCTxt = new TextBox();
      this.subjectLbl = new Label();
      this.subjectTxt = new TextBox();
      this.messageLbl = new Label();
      this.messageTxt = new TextBox();
      this.noteLbl = new Label();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.SuspendLayout();
      this.emailToBtn.Location = new Point(12, 12);
      this.emailToBtn.Name = "emailToBtn";
      this.emailToBtn.Size = new Size(60, 20);
      this.emailToBtn.TabIndex = 0;
      this.emailToBtn.Text = "Mail To:";
      this.emailToBtn.Click += new EventHandler(this.emailToBtn_Click);
      this.emailToTxt.Location = new Point(80, 12);
      this.emailToTxt.Name = "emailToTxt";
      this.emailToTxt.Size = new Size(344, 20);
      this.emailToTxt.TabIndex = 1;
      this.emailToTxt.Text = "";
      this.emailCCBtn.Location = new Point(12, 40);
      this.emailCCBtn.Name = "emailCCBtn";
      this.emailCCBtn.Size = new Size(60, 20);
      this.emailCCBtn.TabIndex = 2;
      this.emailCCBtn.Text = "CC:";
      this.emailCCBtn.Click += new EventHandler(this.emailCCBtn_Click);
      this.emailCCTxt.Location = new Point(80, 40);
      this.emailCCTxt.Name = "emailCCTxt";
      this.emailCCTxt.Size = new Size(344, 20);
      this.emailCCTxt.TabIndex = 3;
      this.emailCCTxt.Text = "";
      this.subjectLbl.AutoSize = true;
      this.subjectLbl.Location = new Point(12, 72);
      this.subjectLbl.Name = "subjectLbl";
      this.subjectLbl.Size = new Size(45, 16);
      this.subjectLbl.TabIndex = 4;
      this.subjectLbl.Text = "&Subject:";
      this.subjectTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.subjectTxt.Location = new Point(80, 68);
      this.subjectTxt.Name = "subjectTxt";
      this.subjectTxt.Size = new Size(344, 20);
      this.subjectTxt.TabIndex = 5;
      this.subjectTxt.Text = "";
      this.messageLbl.AutoSize = true;
      this.messageLbl.Location = new Point(12, 100);
      this.messageLbl.Name = "messageLbl";
      this.messageLbl.Size = new Size(53, 16);
      this.messageLbl.TabIndex = 6;
      this.messageLbl.Text = "&Message:";
      this.messageTxt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.messageTxt.Location = new Point(12, 120);
      this.messageTxt.Multiline = true;
      this.messageTxt.Name = "messageTxt";
      this.messageTxt.Size = new Size(412, 245);
      this.messageTxt.TabIndex = 7;
      this.messageTxt.Text = "";
      this.noteLbl.Location = new Point(12, 376);
      this.noteLbl.Name = "noteLbl";
      this.noteLbl.Size = new Size(412, 32);
      this.noteLbl.TabIndex = 8;
      this.noteLbl.Text = "Note: The recipient(s) must know the last four digits of the primary borrower's social security number to view the forms.";
      this.okBtn.Anchor = AnchorStyles.Bottom;
      this.okBtn.Location = new Point(128, 416);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(84, 24);
      this.okBtn.TabIndex = 9;
      this.okBtn.Text = "&Send Forms";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(225, 416);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 10;
      this.cancelBtn.Text = "&Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(436, 452);
      this.Controls.Add((Control) this.emailToBtn);
      this.Controls.Add((Control) this.emailToTxt);
      this.Controls.Add((Control) this.emailCCBtn);
      this.Controls.Add((Control) this.emailCCTxt);
      this.Controls.Add((Control) this.subjectTxt);
      this.Controls.Add((Control) this.subjectLbl);
      this.Controls.Add((Control) this.messageTxt);
      this.Controls.Add((Control) this.messageLbl);
      this.Controls.Add((Control) this.noteLbl);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EmailDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Secure Form Transfer";
      this.KeyDown += new KeyEventHandler(this.EmailDialog_KeyDown);
      this.ResumeLayout(false);
    }

    private void emailToBtn_Click(object sender, EventArgs e)
    {
      EmailListDialog emailListDialog = new EmailListDialog(Session.LoanData);
      if (emailListDialog.ShowDialog() == DialogResult.Cancel)
        return;
      string str = this.emailToTxt.Text.Trim();
      this.emailToTxt.Text = !(str == string.Empty) ? str + "; " + emailListDialog.EmailList : emailListDialog.EmailList;
      this.emailToTxt.Focus();
    }

    private void emailCCBtn_Click(object sender, EventArgs e)
    {
      EmailListDialog emailListDialog = new EmailListDialog(Session.LoanData);
      if (emailListDialog.ShowDialog() == DialogResult.Cancel)
        return;
      string str = this.emailCCTxt.Text.Trim();
      this.emailCCTxt.Text = !(str == string.Empty) ? str + "; " + emailListDialog.EmailList : emailListDialog.EmailList;
      this.emailCCTxt.Focus();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string str1 = this.emailToTxt.Text.Trim();
      string str2 = this.emailCCTxt.Text.Trim();
      string str3 = this.subjectTxt.Text.Trim();
      string str4 = this.messageTxt.Text.Trim();
      if (str1 == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must enter an email address of who you want to send the message to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!Utils.ValidateEmail(str1))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The recipient's e-mail address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.emailToTxt.Focus();
      }
      else if (str2 != string.Empty && !Utils.ValidateEmail(str2))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The recipient's e-mail address format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.emailCCTxt.Focus();
      }
      else
      {
        CompanyInfo companyInfo = Session.CompanyInfo;
        UserInfo userInfo = Session.UserInfo;
        LoanData loanData = Session.LoanData;
        bool flag = false;
        try
        {
          GNetMessageParserClass messageParserClass = new GNetMessageParserClass();
          messageParserClass.SetProperty("FileSignature", "GNet_Message_File", 1, 0U);
          messageParserClass.SetProperty("FileVersion", "1.00", 1, 0U);
          messageParserClass.SetProperty("MsgClass", "GCeM", 1, 0U);
          messageParserClass.SetProperty("MSG_certificate", "", 1, 0U);
          messageParserClass.SetProperty("UserName", userInfo.Userid, 1, 0U);
          messageParserClass.SetProperty("BrokerCompanyName", companyInfo.Name, 1, 0U);
          messageParserClass.SetProperty("BrokerCompanyID", companyInfo.ClientID, 1, 0U);
          messageParserClass.SetProperty("LoanRelationID", loanData.GUID, 1, 0U);
          messageParserClass.SetProperty("LoanNumber", loanData.GetSimpleField("364"), 1, 0U);
          messageParserClass.SetProperty("LoanAmount", loanData.GetSimpleField("2"), 1, 0U);
          messageParserClass.SetProperty("PropertyStreet", loanData.GetSimpleField("11"), 1, 0U);
          messageParserClass.SetProperty("PropertyCity", loanData.GetSimpleField("12"), 1, 0U);
          messageParserClass.SetProperty("PropertyState", loanData.GetSimpleField("14"), 1, 0U);
          messageParserClass.SetProperty("PropertyZip", loanData.GetSimpleField("15"), 1, 0U);
          messageParserClass.SetProperty("BorrowerFirstName", loanData.GetSimpleField("36"), 1, 0U);
          messageParserClass.SetProperty("BorrowerLastName", loanData.GetSimpleField("37"), 1, 0U);
          messageParserClass.SetProperty("BorrowerMiddleName", "", 1, 0U);
          messageParserClass.SetProperty("BorrowerNameSuffix", "", 1, 0U);
          long hForm = (long) messageParserClass.AddForm(false, 8, "GCeM");
          long hHandle = (long) messageParserClass.AddPage((uint) hForm, 8, "GCeM");
          messageParserClass.SetProperty("MsgPassword", this.ssn, 3, (uint) hHandle);
          messageParserClass.SetProperty("SenderName", userInfo.FullName, 3, (uint) hHandle);
          messageParserClass.SetProperty("SenderID", userInfo.Email, 3, (uint) hHandle);
          messageParserClass.SetProperty("RecipientID", str1, 3, (uint) hHandle);
          messageParserClass.SetProperty("CCID", str2, 3, (uint) hHandle);
          object pVar1 = (object) str3;
          messageParserClass.SetBinProperty("Subject", ref pVar1, 3, (uint) hHandle);
          object pVar2 = (object) str4;
          messageParserClass.SetBinProperty("Body", ref pVar2, 3, (uint) hHandle);
          messageParserClass.SetProperty("Attach1Type", "6", 3, (uint) hHandle);
          messageParserClass.SetPageDataPropertyToFile("Attach1", this.pdfFile, (uint) hHandle);
          string tempFileName = Path.GetTempFileName();
          messageParserClass.CreateNew(tempFileName);
          messageParserClass.Save();
          messageParserClass.DestroyThisMessage();
          flag = Session.Application.GetService<IEPass>().SendMessage(tempFileName);
        }
        catch (Exception ex)
        {
          Tracing.Log(EmailDialog.sw, TraceLevel.Error, nameof (EmailDialog), ex.Message);
        }
        if (!flag)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Unable to send the Secure Form Transfer message.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.DialogResult = DialogResult.OK;
      }
    }

    private void EmailDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Secure Form Transfer");
    }
  }
}
