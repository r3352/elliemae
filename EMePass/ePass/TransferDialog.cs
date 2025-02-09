// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.TransferDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using GNETPARSERXLib;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class TransferDialog : Form, IHelp
  {
    private const string className = "TransferDialog";
    private static readonly string sw = Tracing.SwEpass;
    private Button cancelBtn;
    private Button okBtn;
    private TextBox messageTxt;
    private Label label4;
    private TextBox subjectTxt;
    private Label label3;
    private Label label2;
    private TextBox userTxt;
    private Label label1;
    private TextBox clientTxt;
    private System.ComponentModel.Container components;
    private string fileList = string.Empty;
    private string guidList = string.Empty;
    private string loanAmt = string.Empty;
    private string loanNo = string.Empty;
    private string borFirst = string.Empty;
    private Label label5;
    private string borLast = string.Empty;

    public TransferDialog(string fileList, string guidList)
    {
      this.InitializeComponent();
      this.fileList = fileList;
      this.guidList = guidList;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.messageTxt = new TextBox();
      this.label4 = new Label();
      this.subjectTxt = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.userTxt = new TextBox();
      this.label1 = new Label();
      this.clientTxt = new TextBox();
      this.label5 = new Label();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(409, 406);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom;
      this.okBtn.Location = new Point(326, 406);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "&Send";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.messageTxt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.messageTxt.Location = new Point(13, 106);
      this.messageTxt.Multiline = true;
      this.messageTxt.Name = "messageTxt";
      this.messageTxt.Size = new Size(471, 290);
      this.messageTxt.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 89);
      this.label4.Name = "label4";
      this.label4.Size = new Size(51, 14);
      this.label4.TabIndex = 31;
      this.label4.Text = "Message";
      this.subjectTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.subjectTxt.Location = new Point(62, 57);
      this.subjectTxt.MaxLength = (int) byte.MaxValue;
      this.subjectTxt.Name = "subjectTxt";
      this.subjectTxt.Size = new Size(422, 20);
      this.subjectTxt.TabIndex = 2;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(11, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(43, 14);
      this.label3.TabIndex = 29;
      this.label3.Text = "Subject";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(42, 14);
      this.label2.TabIndex = 28;
      this.label2.Text = "User ID";
      this.userTxt.Location = new Point(62, 35);
      this.userTxt.MaxLength = 16;
      this.userTxt.Name = "userTxt";
      this.userTxt.Size = new Size(128, 20);
      this.userTxt.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(11, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(45, 14);
      this.label1.TabIndex = 26;
      this.label1.Text = "Client ID";
      this.clientTxt.Location = new Point(62, 13);
      this.clientTxt.MaxLength = 10;
      this.clientTxt.Name = "clientTxt";
      this.clientTxt.Size = new Size(128, 20);
      this.clientTxt.TabIndex = 0;
      this.clientTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(197, 37);
      this.label5.Name = "label5";
      this.label5.Size = new Size(270, 14);
      this.label5.TabIndex = 32;
      this.label5.Text = "(Leave User ID blank if recipient uses Personal Edition)";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(494, 437);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.messageTxt);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.subjectTxt);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.userTxt);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.clientTxt);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TransferDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "File Transfer";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string str1 = this.clientTxt.Text.Trim();
      string str2 = this.userTxt.Text.Trim();
      if (str1.Length != 10)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The client id should be 10 digits.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.clientTxt.Focus();
      }
      else
      {
        if (str2 == string.Empty)
          str2 = "admin";
        string branchID = str1 + "/" + str2;
        Cursor.Current = Cursors.WaitCursor;
        if (!this.transferZipFiles(branchID))
          Cursor.Current = Cursors.Default;
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool transferZipFiles(string branchID)
    {
      Tracing.Log(TransferDialog.sw, nameof (TransferDialog), TraceLevel.Info, "transferZipFiles: branchID(" + branchID + ") starts at " + DateTime.Now.ToString());
      string str1 = Path.Combine(SystemSettings.TempFolderRoot, "Transfer");
      string str2 = Path.Combine(str1, "Temp");
      string str3 = Path.Combine(SystemSettings.TempFolderRoot, "Transfer.zip");
      if (Directory.Exists(str1))
      {
        try
        {
          Directory.Delete(str1, true);
        }
        catch (Exception ex)
        {
          Tracing.Log(TransferDialog.sw, TraceLevel.Error, nameof (TransferDialog), "Cannot delete '" + str1 + "' folder. " + ex.Message);
          int num = (int) Utils.Dialog((IWin32Window) this, "The working folder cannot be created. Please make sure you have user's right to create subfolder under USERS/" + Session.UserInfo.Userid + " folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      try
      {
        Directory.CreateDirectory(str1);
        Directory.CreateDirectory(str1 + "\\Files");
        Directory.CreateDirectory(str1 + "\\Temp");
        File.Delete(str3);
      }
      catch (Exception ex)
      {
        Tracing.Log(TransferDialog.sw, TraceLevel.Error, nameof (TransferDialog), "Cannot create '" + str1 + "' folder. " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "The working folder cannot be created. Please make sure you have user's right to create subfolder under USERS/" + Session.UserInfo.Userid + " folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      string[] strArray1 = this.fileList.Split('|');
      string[] strArray2 = this.guidList.Split('|');
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      bool flag1 = false;
      bool flag2 = false;
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        string str4 = str2 + "\\" + strArray1[index1];
        ILoan loan = Session.LoanManager.OpenLoan(strArray2[index1]);
        if (loan.GetLockInfo().LockedFor == LoanInfo.LockReason.OpenForWork)
        {
          switch (Utils.Dialog((IWin32Window) this, "The loan '" + strArray1[index1] + "' is open for work by someone. Do you still want to transfer it?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
          {
            case DialogResult.Cancel:
              flag1 = true;
              goto label_43;
            case DialogResult.No:
              continue;
          }
        }
        if (!flag2)
        {
          LoanData loanData = loan.GetLoanData(false);
          if (Utils.ParseDouble((object) loanData.OriginalLoanVersion) < 3.61)
          {
            try
            {
              loan.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
              loan.Save(loanData, "");
              loan.Unlock();
            }
            catch (Exception ex)
            {
              Tracing.Log(TransferDialog.sw, TraceLevel.Error, nameof (TransferDialog), "This loan has older version. Encompass is trying to migrate the latest data to loan but this loan cannot be opened due to this reason: " + ex.Message);
              int num = (int) Utils.Dialog((IWin32Window) this, "This loan has older version. Encompass is trying to migrate the latest format to loan but this loan cannot be opened due to this reason: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return false;
            }
          }
          this.loanAmt = loanData.GetSimpleField("2");
          this.loanNo = loanData.GetSimpleField("364");
          this.borFirst = loanData.GetSimpleField("36");
          this.borLast = loanData.GetSimpleField("37");
          flag2 = true;
        }
        BinaryObject binaryObject = loan.Export();
        Directory.CreateDirectory(str4);
        string path = Path.Combine(str4, "loan.em");
        binaryObject.Write(path);
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        foreach (LoanProperty loanPropertySetting in loan.GetLoanPropertySettings())
        {
          if (string.Equals(loanPropertySetting.Category, "LoanStorage", StringComparison.OrdinalIgnoreCase) && string.Equals(loanPropertySetting.Attribute, "SupportingData", StringComparison.OrdinalIgnoreCase))
          {
            if (string.Equals(loanPropertySetting.Value, "SkyDrive", StringComparison.OrdinalIgnoreCase))
            {
              flag3 = true;
              break;
            }
            if (string.Equals(loanPropertySetting.Value, "SkyDriveLite", StringComparison.OrdinalIgnoreCase))
            {
              flag4 = true;
              break;
            }
            if (string.Equals(loanPropertySetting.Value, "SkyDriveClassic", StringComparison.OrdinalIgnoreCase))
            {
              flag5 = true;
              break;
            }
            break;
          }
        }
        string[] supportingDataKeysOnCiFs = loan.GetSupportingDataKeysOnCIFs();
        for (int index2 = 0; index2 < supportingDataKeysOnCiFs.Length; ++index2)
        {
          string lower = supportingDataKeysOnCiFs[index2].ToLower();
          if (!lower.Equals("attachments.xml") && !lower.StartsWith("attachment-") && !lower.StartsWith("thumbnails-") && !lower.StartsWith("images-") && !lower.StartsWith("disclosures-") && !lower.StartsWith("opening-") && !lower.StartsWith("edisclosure_") && !lower.StartsWith("closing-") && !lower.EndsWith(".pdf"))
          {
            BinaryObject supportingDataOnCiFs = loan.GetSupportingDataOnCIFs(lower);
            if (supportingDataOnCiFs != null)
            {
              Tracing.Log(TransferDialog.sw, nameof (TransferDialog), TraceLevel.Info, "transferZipFiles: branchID(" + branchID + ") key(" + supportingDataKeysOnCiFs[index2] + ")", supportingDataOnCiFs.Length);
              supportingDataOnCiFs.Write(Path.Combine(str4, supportingDataKeysOnCiFs[index2]));
            }
          }
        }
        if (flag3 | flag4 | flag5)
        {
          SkyDriveStreamingClient driveStreamingClient = new SkyDriveStreamingClient(loan, Session.StartupInfo);
          string[] supportingDataKeys = loan.GetSkyDriveSupportingDataKeys();
          for (int index3 = 0; index3 < supportingDataKeys.Length; ++index3)
          {
            string lower = supportingDataKeys[index3].ToLower();
            if (!lower.Equals("attachments.xml") && !lower.StartsWith("attachment-") && !lower.StartsWith("thumbnails-") && !lower.StartsWith("images-") && !lower.StartsWith("disclosures-") && !lower.StartsWith("opening-") && !lower.StartsWith("edisclosure_") && !lower.StartsWith("closing-") && !lower.EndsWith(".pdf"))
            {
              Task<BinaryObject> supportingData = driveStreamingClient.GetSupportingData(supportingDataKeys[index3]);
              Task.WaitAll((Task) supportingData);
              BinaryObject result = supportingData.Result;
              if (result != null)
              {
                Tracing.Log(TransferDialog.sw, nameof (TransferDialog), TraceLevel.Info, "transferZipFiles: branchID(" + branchID + ") key(" + supportingDataKeys[index3] + ")", result.Length);
                result.Write(Path.Combine(str4, supportingDataKeys[index3]));
              }
            }
          }
        }
        string str5 = strArray1[index1];
        int num1 = strArray1[index1].LastIndexOf("\\");
        if (num1 >= 0)
          str5 = strArray1[index1].Substring(num1 + 1);
        string destPath = Path.Combine(str1, "Files\\" + loan.Guid + "(" + str5 + ").ZIP");
        FileCompressor.Instance.ZipDirectory(str4, destPath);
      }
label_43:
      bool flag6 = false;
      int length = Directory.GetFiles(Path.Combine(str1, "Files")).Length;
      if (!flag1 && length > 0)
      {
        string str6 = Path.Combine(str1, "Transfer.zip");
        FileCompressor.Instance.ZipDirectory(Path.Combine(str1, "Files"), str6);
        if (this.addMessageInfo(str6, branchID, str3, length))
          flag6 = Session.Application.GetService<IEPass>().SendMessage(str3);
      }
      try
      {
        Directory.Delete(str1, true);
      }
      catch (Exception ex)
      {
        Tracing.Log(TransferDialog.sw, TraceLevel.Error, nameof (TransferDialog), "Cannot delete '" + str1 + "' folder. " + ex.Message);
      }
      if (!flag1 & flag6)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The loan file(s) was successfully sent to branch #" + branchID, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      Tracing.Log(TransferDialog.sw, nameof (TransferDialog), TraceLevel.Info, "transferZipFiles: branchID(" + branchID + ") ends at " + DateTime.Now.ToString());
      return true;
    }

    private bool addMessageInfo(string zip, string branchID, string transferFile, int loanCount)
    {
      GNetMessageParserClass messageParserClass = new GNetMessageParserClass();
      if (messageParserClass != null)
      {
        if (zip != string.Empty)
        {
          try
          {
            CompanyInfo companyInfo = Session.RecacheCompanyInfo();
            UserInfo userInfo = Session.UserInfo;
            messageParserClass.SetProperty("FileSignature", "File Transfer", 1, 0U);
            messageParserClass.SetProperty("FileVersion", "1.00", 1, 0U);
            messageParserClass.SetProperty("MsgClass", "File Transfer", 1, 0U);
            messageParserClass.SetProperty("UserName", userInfo.Userid, 1, 0U);
            messageParserClass.SetProperty("BrokerCompanyName", companyInfo.Name, 1, 0U);
            messageParserClass.SetProperty("BrokerCompanyID", companyInfo.ClientID, 1, 0U);
            messageParserClass.SetProperty("LoanAmount", this.loanAmt, 1, 0U);
            messageParserClass.SetProperty("LoanRelationID", "{" + Guid.NewGuid().ToString() + "}", 1, 0U);
            messageParserClass.SetProperty("LoanNumber", this.loanNo, 1, 0U);
            if (loanCount > 1)
            {
              messageParserClass.SetProperty("BorrowerFirstName", "Batch - ", 1, 0U);
              messageParserClass.SetProperty("BorrowerLastName", loanCount.ToString() + " loans (" + this.borFirst + " " + this.borLast + "...)", 1, 0U);
            }
            else
            {
              messageParserClass.SetProperty("BorrowerFirstName", this.borFirst, 1, 0U);
              messageParserClass.SetProperty("BorrowerLastName", this.borLast, 1, 0U);
            }
            long hForm = (long) messageParserClass.AddForm(false, 8, "Description");
            long hHandle = (long) messageParserClass.AddPage((uint) hForm, 8, "Description");
            messageParserClass.SetProperty("CCID", "", 3, (uint) hHandle);
            messageParserClass.SetProperty("RecipientID", branchID, 3, (uint) hHandle);
            messageParserClass.SetProperty("SenderID", userInfo.Email, 3, (uint) hHandle);
            messageParserClass.SetProperty("SenderName", userInfo.FullName, 3, (uint) hHandle);
            object pVar1 = (object) this.subjectTxt.Text.ToString().Trim();
            messageParserClass.SetBinProperty("Subject", ref pVar1, 3, (uint) hHandle);
            object pVar2 = (object) this.messageTxt.Text.ToString().Trim();
            messageParserClass.SetBinProperty("Body", ref pVar2, 3, (uint) hHandle);
            messageParserClass.SetProperty("Attach1Type", "3", 3, (uint) hHandle);
            messageParserClass.SetPageDataPropertyToFile("Attach1", zip, (uint) hHandle);
            messageParserClass.CreateNew(transferFile);
            messageParserClass.Save();
            messageParserClass.DestroyThisMessage();
          }
          catch (Exception ex)
          {
            Tracing.Log(TransferDialog.sw, TraceLevel.Error, nameof (TransferDialog), "Cannot add message to zip file. " + ex.Message);
            int num = (int) Utils.Dialog((IWin32Window) this, "Cannot create ZIP file for file transfer. Please make sure you have latest version.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
      }
      return true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (TransferDialog));
    }
  }
}
