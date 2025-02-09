// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.eFolder.DocumentCollector
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.eFolder
{
  public class DocumentCollector : Form
  {
    private DocumentLog doc;
    private List<FileAttachment> fileList = new List<FileAttachment>();
    private IContainer components;
    private BrowserControl browser;

    public DocumentCollector(DocumentLog doc)
    {
      this.InitializeComponent();
      this.doc = doc;
    }

    public FileAttachment[] Files => this.fileList.ToArray();

    public bool Retrieve()
    {
      if (!this.doc.IsePASS)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You have not ordered the '" + this.doc.Title + "' from an ICE Mortgage Technology Network provider yet.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.doc.EPASSSignature == string.Empty)
      {
        string str = this.doc.RequestedFrom;
        if (str == string.Empty)
          str = "The ICE Mortgage Technology Network provider";
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, str + " does not support retrieving '" + this.doc.Title + "' documents.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (!new eFolderAccessRights(Session.LoanDataMgr, (LogRecordBase) this.doc).CanRetrieveServices)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to retrieve documents from ICE Mortgage Technology Network Services.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (FileAttachment fileAttachment in Session.LoanDataMgr.FileAttachments)
        fileAttachmentList.Add(fileAttachment);
      this.browser.ProcessURL(this.doc.EPASSSignature);
      Bam.LinkAttachments(Session.LoanDataMgr, this.doc);
      foreach (FileAttachment fileAttachment in Session.LoanDataMgr.FileAttachments)
      {
        if (!fileAttachmentList.Contains(fileAttachment))
          this.fileList.Add(fileAttachment);
      }
      return this.fileList.Count > 0;
    }

    private void downloadFile(Uri uri)
    {
      string str;
      if (!uri.IsFile)
      {
        str = SystemSettings.GetTempFileNameWithExtension("pdf");
        DownloadManager downloadManager = new DownloadManager();
        DownloadResult downloadResult = downloadManager.BeginDownload(uri, str);
        int num = (int) downloadManager.ShowDialog((IWin32Window) Form.ActiveForm);
        if (downloadResult.DownloadStatus != DownloadStatus.Complete)
          return;
      }
      else
        str = !uri.LocalPath.ToLower().EndsWith("\\dodirectmain.htm") ? uri.LocalPath : this.downloadFannie(uri.LocalPath);
      Session.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Services, str, this.doc.Title, this.doc);
    }

    private string downloadFannie(string filepath)
    {
      using (BinaryObject binaryObject = new BinaryObject(filepath))
      {
        string str = binaryObject.ToString();
        int startIndex = str.ToLower().IndexOf(".findingshtml.htm\"");
        if (startIndex < 0)
          return filepath;
        int num = str.LastIndexOf("\"", startIndex);
        return num < 0 ? filepath : str.Substring(num + 1, startIndex - num + 16);
      }
    }

    private void browser_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (e.Url.AbsoluteUri.IndexOf("_EPASS_SIGNATURE;") >= 0)
        return;
      this.downloadFile(e.Url);
      e.Cancel = true;
    }

    private void browser_LoginUser(object sender, LoginUserEventArgs e)
    {
      e.Response = EpassLogin.LoginUser(e.ShowDialogs);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.browser = new BrowserControl();
      this.SuspendLayout();
      this.browser.Location = new Point(20, 20);
      this.browser.Name = "browser";
      this.browser.Size = new Size(332, 64);
      this.browser.TabIndex = 0;
      this.browser.LoginUser += new LoginUserEventHandler(this.browser_LoginUser);
      this.browser.BeforeNavigate += new WebBrowserNavigatingEventHandler(this.browser_BeforeNavigate);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(380, 112);
      this.Controls.Add((Control) this.browser);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentCollector);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Document Collector";
      this.ResumeLayout(false);
    }
  }
}
