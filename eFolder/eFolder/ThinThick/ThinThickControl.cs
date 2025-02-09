// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ThinThick.ThinThickControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using Elli.Web.Host.Adapter;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.eFolder.Conditions;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ThinThick;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.ThinThick
{
  public class ThinThickControl : UserControl, IRefreshContents
  {
    private const string className = "ThinThickDialog";
    private static readonly string sw = Tracing.SwEFolder;
    public WebHost webHost;
    private LoanDataMgr loanDataMgr;
    private ThinThickType thinThickType;
    private string thinThickUrl;
    private IContainer components;
    private Panel pnlBrowser;
    private Label lblMessage;

    public ThinThickControl(LoanDataMgr loanDataMgr, string url, ThinThickType type)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.thinThickUrl = url;
      this.thinThickType = type;
      this.lblMessage.Location = new Point(0, 0);
    }

    private void WebHost_helpBrowserInvoked(object sender, InvokeHelpBrowserEventArgs args)
    {
      if (args == null || string.IsNullOrEmpty(args.helpBrowserUrl))
        return;
      this.pnlBrowser.Focus();
      JedHelp.ShowHelpPage(args.helpBrowserUrl);
    }

    private void showBrowser(string url)
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "showBrowser: " + url);
      try
      {
        this.setWebHost();
        ModuleParameters parameters = new ModuleParameters()
        {
          User = this.getModuleUser(),
          Parameters = this.getWebPageParams()
        };
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\EnableWebHostDebugMode"))
        {
          if (registryKey != null)
            this.webHost.ShowConsole();
        }
        Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "Calling LoadModule");
        if (this.thinThickType == ThinThickType.eClose || this.thinThickType == ThinThickType.eSignPackages || this.thinThickType == ThinThickType.eNote || this.thinThickType == ThinThickType.eConsent)
          this.webHost.LoadModule(url + "host.html", url + "index.html", parameters);
        else
          this.webHost.LoadModule(url, parameters);
      }
      catch (Exception ex)
      {
        Tracing.Log(ThinThickControl.sw, TraceLevel.Error, "ThinThickDialog", ex.ToString());
        this.showMessage("Something went wrong trying to show the browser. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support.\n\n" + this.getCorrelationId(ex));
      }
    }

    private void setWebHost()
    {
      if (this.webHost != null)
        return;
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "Creating WebHost");
      if (this.thinThickType == ThinThickType.eClose)
        this.webHost = new WebHost("sc", new Elli.Web.Host.PostMessageHandler(this.PostMessageHandler), new Action(this.UnloadHandler), new AuditWindowHandler(this.ShowAuditWindowHandler), new UpdateTemplateWindowHandler(this.ShowUpdateTemplateWindowHandler));
      else if (this.thinThickType == ThinThickType.eSignPackages || this.thinThickType == ThinThickType.eNote)
        this.webHost = new WebHost("sc", unloadHandler: new Action(this.UnloadHandler));
      else if (this.thinThickType == ThinThickType.eConsent)
      {
        this.webHost = new WebHost("sc", new Elli.Web.Host.PostMessageHandler(this.PostMessageHandler), new Action(this.UnloadHandler));
      }
      else
      {
        this.webHost = new WebHost("apiplatform", unloadHandler: new Action(this.UnloadHandler));
        this.webHost.HelpInvoked += new WebHost.HelpInvokedEventHandler(this.WebHost_helpBrowserInvoked);
      }
      this.webHost.FrameComplete += new WebHost.FrameCompleteEventHandler(this.WebHost_finishLoadingFrameEvent);
      this.Controls.Add((Control) this.webHost);
      WebHost webHost = this.webHost;
      Size clientSize = this.ClientSize;
      int width = clientSize.Width;
      clientSize = this.ClientSize;
      int height = clientSize.Height;
      webHost.SetBounds(0, 0, width, height);
    }

    private ModuleUser getModuleUser()
    {
      UserInfo userInfo = Session.UserInfo;
      return new ModuleUser()
      {
        ID = userInfo.Userid,
        LastName = userInfo.LastName,
        FirstName = userInfo.FirstName,
        Email = userInfo.Email
      };
    }

    private Dictionary<string, object> getWebPageParams()
    {
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      if (this.thinThickType == ThinThickType.eClose)
      {
        webPageParams.Add("loanGuid", (object) Guid.Parse(this.loanDataMgr.LoanData.GUID).ToString());
        webPageParams.Add("oapiBaseUrl", (object) this.loanDataMgr.SessionObjects.StartupInfo.OAPIGatewayBaseUri);
        webPageParams.Add("permissions", (object) this.getPermissions());
        webPageParams.Add("lockId", Session.SessionObjects == null ? (object) string.Empty : (object) Session.SessionObjects.SessionID);
        webPageParams.Add("allowHybridWithENote", (object) Session.SettingsManager.GetServerSetting("eClose.AllowHybridWithENoteClosing").ToString());
        webPageParams.Add("allowUserToDiscardPackage", (object) Session.SettingsManager.GetServerSetting("eClose.AllowUserToDiscardPackage").ToString());
        webPageParams.Add("packageExpirationDefaultDays", (object) Session.SettingsManager.GetServerSetting("eClose.PackageExpirationDefaultDays").ToString());
        webPageParams.Add("packagePreview", (object) Session.SettingsManager.GetServerSetting("eClose.PackagePreview").ToString());
        webPageParams.Add("approveSigningDefaultDate", (object) Session.SettingsManager.GetServerSetting("eClose.ApproveSigningDefaultDate").ToString());
        webPageParams.Add("allowUserToSendPackageToThirdParty", (object) Session.SettingsManager.GetServerSetting("eClose.AllowClosingPackageTPseClosing").ToString());
        string companySetting = Session.SessionObjects.ConfigurationManager.GetCompanySetting("closingDocs", "EnableClosingDocsWorkflow");
        webPageParams.Add("isClosingDocsWorkflowEnabled", string.IsNullOrEmpty(companySetting) || !Convert.ToBoolean(companySetting) ? (object) "false" : (object) "true");
      }
      else if (this.thinThickType == ThinThickType.eSignPackages || this.thinThickType == ThinThickType.eNote || this.thinThickType == ThinThickType.eConsent)
      {
        if (this.loanDataMgr != null)
          webPageParams.Add("loanGuid", (object) Guid.Parse(this.loanDataMgr.LoanData.GUID).ToString());
        webPageParams.Add("oapiBaseUrl", (object) Session.SessionObjects.StartupInfo.OAPIGatewayBaseUri);
        if (this.thinThickType == ThinThickType.eNote)
          webPageParams.Add("permissions", (object) this.getPermissions());
        if (this.thinThickType == ThinThickType.eConsent)
          webPageParams.Add("lockId", Session.SessionObjects == null ? (object) string.Empty : (object) Session.SessionObjects.SessionID);
      }
      else
      {
        var data1 = new
        {
          clientId = Session.DefaultInstance.CompanyInfo.ClientID,
          version = Session.DefaultInstance.UserInfo.EncompassVersion,
          loans = new LoanDetail[1]
          {
            new LoanDetail()
            {
              id = Guid.Parse(this.loanDataMgr.LoanData.GUID).ToString(),
              loanNumber = this.loanDataMgr.LoanData.LoanNumber,
              lockId = Session.SessionObjects == null ? string.Empty : Session.SessionObjects.SessionID,
              enhancedConditionEnabled = true
            }
          },
          user = new
          {
            id = "Encompass\\" + this.loanDataMgr.SessionObjects.StartupInfo.ServerInstanceName + "\\" + Session.DefaultInstance.UserInfo.Userid
          }
        };
        webPageParams.Add("encompass", (object) data1);
        if (this.thinThickType == ThinThickType.ImportAll || this.thinThickType == ThinThickType.ReviewAndImport)
        {
          var data2 = new{ eFolder = new{  } };
          webPageParams.Add("condition", (object) data2);
        }
        webPageParams.Add("errorMessages", (object) new List<string>());
      }
      return webPageParams;
    }

    private string[] getPermissions()
    {
      List<string> stringList = new List<string>();
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      if (this.thinThickType == ThinThickType.eClose)
      {
        if (folderAccessRights.CanOrderEcloseDocsWithComplianceFailures)
          stringList.Add("CanOrderDocsWithComplianceFailures");
        if (folderAccessRights.CanAddAdditionalEcloseDocs)
          stringList.Add("CanAddAdditionalDocs");
        if (folderAccessRights.CanMoveECloseDocsUpDown)
          stringList.Add("CanMoveDocsUpDown");
        if (folderAccessRights.CanDeselectECloseDocs)
          stringList.Add("CanDeselectDocs");
        if (folderAccessRights.CanApproveECloseForSigning)
          stringList.Add("CanApproveForSigning");
        if (folderAccessRights.CanUpdateClosingStackingTemplates)
          stringList.Add("CanUpdateClosingStackingTemplates");
        if (folderAccessRights.CanConfigureClosingOptions)
          stringList.Add("CanConfigureClosingOptions");
        if (folderAccessRights.HasPackageSigningDateRight)
          stringList.Add("PackageSigningDate");
        if (folderAccessRights.HasPackagePreviewRight)
          stringList.Add("PackagePreview");
        if (folderAccessRights.HasPackageExpirationRight)
          stringList.Add("PackageExpiration");
      }
      if (folderAccessRights.HasReverseRegistrationRight)
        stringList.Add("ReverseRegistration");
      if (this.thinThickType == ThinThickType.eNote)
      {
        if (folderAccessRights.CanTransfer)
          stringList.Add("CanTransfer");
        if (folderAccessRights.CanDeactivateEnote)
          stringList.Add("CanDeactivateEnote");
        if (folderAccessRights.CanReverseDeactivation)
          stringList.Add("CanReverseDeactivation");
      }
      return stringList.ToArray();
    }

    private void showMessage(string message)
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "showMessage: " + message);
      this.lblMessage.Text = message;
      this.lblMessage.BringToFront();
    }

    public void ShowMessage(string message) => this.showMessage(message);

    private string getCorrelationId(Exception ex)
    {
      for (; ex != null; ex = ex.InnerException)
      {
        int startIndex = ex.Message.LastIndexOf("Correlation");
        if (startIndex >= 0)
          return ex.Message.Substring(startIndex);
      }
      return string.Empty;
    }

    public bool CanCloseWebHost()
    {
      bool flag = false;
      try
      {
        Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "Calling WebHost.RaiseEvent: BeforeBrowserClose");
        bool[] array = this.webHost.RaiseEvent<bool[]>("BeforeBrowserClose", (object) null, 5000);
        if (array != null)
          flag = Array.Exists<bool>(array, (Predicate<bool>) (x => !x));
      }
      catch (Exception ex)
      {
        Tracing.Log(ThinThickControl.sw, "ThinThickDialog", TraceLevel.Error, "BeforeBrowserClose Callback Failed: " + ex.ToString());
        flag = true;
      }
      return !flag || Utils.Dialog((IWin32Window) this, "There are background processes currently running right now. If you choose to close this window, it can result in you having to exit the loan and re-open it again for editing.\n\nAre you sure that you want to close this window?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No;
    }

    private void ThinThickControl_Resize(object sender, EventArgs e)
    {
      this.lblMessage.Size = this.ClientSize;
      if (this.webHost == null)
        return;
      this.webHost.Size = this.ClientSize;
    }

    private void UnloadHandler()
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "UnloadHandler Triggered");
      this.OnWebHostUnloaded(EventArgs.Empty);
    }

    public event EventHandler WebHostUnloaded;

    protected virtual void OnWebHostUnloaded(EventArgs e)
    {
      if (this.WebHostUnloaded == null)
        return;
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.WebHostUnloaded((object) this, e)));
      else
        this.WebHostUnloaded((object) this, e);
    }

    public string PostMessageHandler(string payload)
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "PostMessage: " + payload);
      try
      {
        ThinThickPostMessage thickPostMessage = JsonConvert.DeserializeObject<ThinThickPostMessage>(payload);
        switch (thickPostMessage.action)
        {
          case "NotifyAdditionalUsers":
            LoanDisplayInfo loanDisplayInfo = new LoanDisplayInfo()
            {
              LoanGuid = new Guid(this.loanDataMgr.LoanData.GUID)
            };
            using (NotifyUsersDialog notifyUsersDialog = new NotifyUsersDialog(new List<LoanDisplayInfo>()
            {
              loanDisplayInfo
            }))
            {
              int num = (int) notifyUsersDialog.ShowDialog((IWin32Window) this);
            }
            return string.Empty;
          case "CloseEConsentWindow":
            this.ParentForm.DialogResult = DialogResult.OK;
            return string.Empty;
          default:
            throw new Exception("Unsupported action:" + thickPostMessage.action + ".");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ThinThickControl.sw, "ThinThickDialog", TraceLevel.Error, "PostMessage failed: " + ex.ToString());
        return "Something went wrong with this operation. Please try again.\n\nIf the error persists, please contact ICE Mortgage Technology support. Details:\n\n" + ex.ToString();
      }
    }

    public bool ShowAuditWindowHandler(string payload)
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "ShowAuditWindow: " + payload);
      try
      {
        using (ThinThickEditorDialog thickEditorDialog = new ThinThickEditorDialog(Session.DefaultInstance, this.loanDataMgr.LoanData.GUID, JsonConvert.DeserializeObject<AuditMessage>(payload)))
          return thickEditorDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(ThinThickControl.sw, "ThinThickDialog", TraceLevel.Error, "ShowAuditWindow failed: " + ex.ToString());
        return false;
      }
    }

    public bool ShowUpdateTemplateWindowHandler(string payload)
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "ShowUpdateTemplateWindow: " + payload);
      try
      {
        UpdateTemplateMessage updateTemplateMessage = JsonConvert.DeserializeObject<UpdateTemplateMessage>(payload);
        DocumentOrderType documentOrderType = DocumentOrderType.None;
        DocumentOrderType orderType;
        switch (updateTemplateMessage.orderType)
        {
          case "none":
            orderType = DocumentOrderType.None;
            break;
          case "opening":
            orderType = DocumentOrderType.Opening;
            break;
          case "closing":
            orderType = DocumentOrderType.Closing;
            break;
          default:
            throw new ArgumentException("The orderType '" + (object) documentOrderType + "' is not recognized.");
        }
        DocEngineStackingOrder engineStackingOrder = Session.ConfigurationManager.GetDocEngineStackingOrder(orderType, updateTemplateMessage.templateName);
        List<StackingElement> stackingElementList = new List<StackingElement>();
        foreach (TemplateDocument document in updateTemplateMessage.documents)
          stackingElementList.Add(new StackingElement(StackingElementType.Document, document.name));
        using (StackingOrderTrainingDialog orderTrainingDialog = new StackingOrderTrainingDialog(engineStackingOrder, stackingElementList.ToArray()))
          return orderTrainingDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(ThinThickControl.sw, "ThinThickDialog", TraceLevel.Error, "ShowUpdateTemplateWindow failed: " + ex.ToString());
        return false;
      }
    }

    private void WebHost_finishLoadingFrameEvent(object sender, FinishedLoadingEventArgs e)
    {
      Tracing.Log(ThinThickControl.sw, TraceLevel.Verbose, "ThinThickDialog", "Finished Loading Frame: " + e.ValidatedURL);
      if (!e.IsMainFrame)
        return;
      this.webHost.BringToFront();
    }

    public void RefreshContents()
    {
      if (this.webHost != null)
        return;
      this.showMessage("Loading...");
      this.showBrowser(this.thinThickUrl);
    }

    public void RefreshLoanContents()
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
      this.pnlBrowser = new Panel();
      this.lblMessage = new Label();
      this.pnlBrowser.SuspendLayout();
      this.SuspendLayout();
      this.pnlBrowser.BackColor = SystemColors.Control;
      this.pnlBrowser.Controls.Add((Control) this.lblMessage);
      this.pnlBrowser.Dock = DockStyle.Fill;
      this.pnlBrowser.Location = new Point(0, 0);
      this.pnlBrowser.Margin = new Padding(2);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(1166, 574);
      this.pnlBrowser.TabIndex = 6;
      this.lblMessage.BackColor = SystemColors.AppWorkspace;
      this.lblMessage.Dock = DockStyle.Fill;
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(0, 0);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(1166, 574);
      this.lblMessage.TabIndex = 5;
      this.lblMessage.Text = "Loading...";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBrowser);
      this.Name = nameof (ThinThickControl);
      this.Size = new Size(1166, 574);
      this.Resize += new EventHandler(this.ThinThickControl_Resize);
      this.pnlBrowser.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
