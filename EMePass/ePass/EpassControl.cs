// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.EpassControl
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.ePass.eFolder;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.ePass.Services;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  public class EpassControl : UserControl, IBrowser, IEPass
  {
    private IContainer components;
    private BrowserControl browser;

    public EpassControl()
    {
      this.InitializeComponent();
      Session.Application.RegisterService((object) this, typeof (IEPass));
      this.browser.OfflinePage = SystemSettings.EpassDir + "Epass.htm";
      PerformanceMeter.Current.AddCheckpoint("Epass - Setup", 41, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\ePass\\EpassControl.cs");
      if (EpassLogin.silentEpassLoginTask != null)
        EpassLogin.silentEpassLoginTask.Wait();
      PerformanceMeter.Current.AddCheckpoint("Epass - Silent Login", 47, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\ePass\\EpassControl.cs");
      this.browser.SetProperties();
    }

    private void browser_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (this.BeforeNavigate == null)
        return;
      this.BeforeNavigate((object) this, e);
    }

    private void browser_LoginUser(object sender, LoginUserEventArgs e)
    {
      e.Response = EpassLogin.LoginUser(e.ShowDialogs);
    }

    private bool checkAccessRight()
    {
      if (Session.LoanDataMgr == null || new eFolderAccessRights(Session.LoanDataMgr).CanRequestServices)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Encompass cannot perform this operation because you do not have rights to request documents from ICE Mortgage Technology Network Services.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    public void Navigate(string url) => this.Navigate(url, true);

    public void Navigate(string url, bool checkAccess)
    {
      if (checkAccess && !this.checkAccessRight())
        return;
      this.browser.Navigate(url);
    }

    public bool ProcessURL(string url) => this.ProcessURL(url, true);

    public bool ProcessURL(string url, bool checkAccess)
    {
      return this.ProcessURL(url, checkAccess, false);
    }

    public bool ProcessURL(string url, bool checkAccess, bool saveLoan)
    {
      if (checkAccess && !this.checkAccessRight())
        return false;
      if (!saveLoan || Session.LoanDataMgr == null || !Session.LoanDataMgr.Dirty || Session.LoanDataMgr.Save())
        return this.browser.ProcessURL(url);
      int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Encompass cannot perform this operation because the loan could not be saved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    public bool Retrieve(DocumentLog doc) => this.Retrieve(doc, true);

    public bool SendMessage(string msgFile) => this.browser.SendMessage(msgFile);

    public void View(string docTitle)
    {
      DocumentLog[] allDocuments = Session.LoanData.GetLogList().GetAllDocuments();
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog documentLog in allDocuments)
      {
        if (documentLog.Title.EndsWith(docTitle))
          documentLogList.Add(documentLog);
      }
      if (docTitle == Epass.Title.FullName)
      {
        foreach (DocumentLog documentLog in allDocuments)
        {
          if (documentLog.Title.EndsWith(Epass.Closing.FullName))
            documentLogList.Add(documentLog);
        }
      }
      if (documentLogList.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no '" + docTitle + "' documents available for viewing.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        using (ViewDocumentDialog viewDocumentDialog = new ViewDocumentDialog(Session.LoanDataMgr, documentLogList.ToArray()))
        {
          switch (viewDocumentDialog.ShowDialog((IWin32Window) Form.ActiveForm))
          {
            case DialogResult.OK:
              Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, viewDocumentDialog.Document);
              break;
            case DialogResult.Retry:
              this.Retrieve(viewDocumentDialog.Document);
              break;
          }
        }
      }
    }

    [Description("Returns the menu for the browser control.")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Browsable(false)]
    public ToolStripDropDown Menu => this.browser.Menu;

    [Description("Returns the user's ePASS password.")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Browsable(false)]
    public string UserPassword => EpassLogin.LoginPassword;

    public event WebBrowserNavigatingEventHandler BeforeNavigate;

    public object ProcessURL2(string url) => this.ProcessURL2(url, true);

    public object ProcessURL2(string url, bool checkAccess)
    {
      if (checkAccess && !this.checkAccessRight())
        return (object) null;
      return this.browser.ProcessURL(url) ? this.browser.Partner : (object) null;
    }

    public bool Retrieve(DocumentLog doc, bool showDocumentDetailsDialog)
    {
      using (DocumentCollector documentCollector = new DocumentCollector(doc))
      {
        if (!documentCollector.Retrieve())
          return false;
        if (showDocumentDetailsDialog)
          Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, doc);
        EPassMessages.SyncReadMessages(true);
        return true;
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
      this.browser = new BrowserControl();
      this.SuspendLayout();
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(0, 0);
      this.browser.Name = "browser";
      this.browser.ShowHomeButton = false;
      this.browser.Size = new Size(740, 400);
      this.browser.TabIndex = 0;
      this.browser.LoginUser += new LoginUserEventHandler(this.browser_LoginUser);
      this.browser.BeforeNavigate += new WebBrowserNavigatingEventHandler(this.browser_BeforeNavigate);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.browser);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EpassControl);
      this.Size = new Size(740, 400);
      this.ResumeLayout(false);
    }
  }
}
