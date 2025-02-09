// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FrmBrowserHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.Licensing;
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FrmBrowserHandler : 
    SHDocVw.DWebBrowserEvents2,
    mshtml.HTMLDocumentEvents2,
    IFormScreen,
    IRefreshContents,
    IDisposable
  {
    private const string className = "FrmBrowserHandler";
    private static string sw = Tracing.SwInputEngine;
    private static Guid documentEvents2Guid = typeof (mshtml.HTMLDocumentEvents2).GUID;
    private AxWebBrowser browserControl;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint browserConnPt;
    private int browserCookie = -1;
    private bool forceComRelease;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint documentConnPt;
    private int documentCookie;
    private IOnlineHelpTarget helpTarget;
    private DateTime startTime = DateTime.MinValue;
    private LoanData loanData;
    private IHtmlInput input;
    private IWin32Window owner;
    private Sessions.Session session;
    private string currentFormId;
    private IInputHandler inputHandler;
    private object property;
    private string goToFieldID = "";
    private int goToFieldCount;
    private Dictionary<string, InputFormInfo> formCache = new Dictionary<string, InputFormInfo>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private bool setFieldsReadonly;

    public event EventHandler FieldOnKeyUp;

    public event EventHandler OnFieldChanged;

    public event EventHandler ButtonClicked;

    public event EventHandler ContactFieldsChanged;

    public FrmBrowserHandler(Sessions.Session session, IWin32Window owner, LoanData loanData)
    {
      this.session = session;
      this.loanData = loanData;
      this.owner = owner;
    }

    public FrmBrowserHandler(Sessions.Session session, IWin32Window owner, IHtmlInput input)
    {
      this.session = session;
      this.input = input;
      this.owner = owner;
    }

    public object Property
    {
      get
      {
        if (this.inputHandler != null)
          this.property = this.inputHandler.Property;
        return this.property;
      }
      set
      {
        this.property = value;
        if (this.inputHandler == null)
          return;
        this.inputHandler.Property = value;
      }
    }

    public bool IsDeleted
    {
      get => this.inputHandler != null && this.inputHandler.IsDeleted;
      set
      {
        if (this.inputHandler == null)
          return;
        this.inputHandler.IsDeleted = value;
      }
    }

    public IInputHandler GetInputHandler() => this.inputHandler;

    public void SetInputHandler(IInputHandler iInputHandler) => this.inputHandler = iInputHandler;

    public object GetAutomationFormObject() => this.inputHandler.GetAutomationFormObject();

    public void Popup(string formName, string title, int width, int height)
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.Popup(formName, title, width, height);
    }

    public void UpdateCurrentField()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.UpdateCurrentField();
    }

    public void ExecAction(string action)
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.ExecAction(action);
    }

    public void RefreshControl(string controlId)
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.RefreshControl(controlId);
    }

    public void RefreshAllControls(bool force)
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.RefreshAllControls(force);
    }

    public void AttachToBrowser(AxWebBrowser axWebBrowser)
    {
      System.Runtime.InteropServices.ComTypes.IConnectionPointContainer ocx = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) axWebBrowser.GetOcx();
      Guid guid = typeof (SHDocVw.DWebBrowserEvents2).GUID;
      ocx.FindConnectionPoint(ref guid, out this.browserConnPt);
      this.browserConnPt.Advise((object) this, out this.browserCookie);
      this.browserControl = axWebBrowser;
    }

    public void Dispose() => this.ReleaseBrowser();

    public void ReleaseBrowser() => this.ReleaseBrowser(false);

    public void ReleaseBrowser(bool keepAxControl)
    {
      this.disconnectInputHandler();
      if (this.browserConnPt == null)
        return;
      try
      {
        this.browserConnPt.Unadvise(this.browserCookie);
        if (this.forceComRelease)
          Marshal.ReleaseComObject((object) this.browserConnPt);
      }
      catch (InvalidComObjectException ex)
      {
      }
      if (!keepAxControl)
        this.browserControl.Dispose();
      this.browserControl = (AxWebBrowser) null;
      this.browserConnPt = (System.Runtime.InteropServices.ComTypes.IConnectionPoint) null;
      Marshal.CleanupUnusedObjectsInCurrentContext();
    }

    public void OpenForm(InputFormInfo formInfo)
    {
      if (this.browserControl == null)
        throw new InvalidOperationException("You must first call AttachToBrower to link object to a Browser control.");
      string formHtmlPath = FormStore.GetFormHTMLPath(this.session, formInfo);
      object obj = (object) Missing.Value;
      this.browserControl.Navigate(formHtmlPath, ref obj, ref obj, ref obj, ref obj);
    }

    public void OpenForm(InputFormInfo formInfo, Sessions.Session session)
    {
      this.session = session;
      this.OpenForm(formInfo);
    }

    internal void SetHelpTarget(IOnlineHelpTarget helpTarget) => this.helpTarget = helpTarget;

    public bool ProcessDialogKey(Keys keyData)
    {
      return this.inputHandler != null && this.inputHandler.ProcessDialogKey(keyData);
    }

    public bool AllowUnloadForm() => this.inputHandler == null || this.inputHandler.AllowUnload();

    public void UnloadForm()
    {
      if (this.inputHandler == null)
        return;
      try
      {
        this.session.InvokeFormUnloading(this.GetAutomationFormObject());
      }
      catch (AutomationException ex)
      {
        this.showAutomationError(ex);
      }
      catch (Exception ex)
      {
        this.showUnexpectedError(ex);
      }
      this.disconnectInputHandler();
      this.inputHandler.Dispose();
      this.inputHandler = (IInputHandler) null;
      if (this.input == null)
        return;
      this.input = (IHtmlInput) null;
    }

    private void disconnectInputHandler()
    {
      if (this.inputHandler != null)
      {
        try
        {
          this.inputHandler.Unload();
          this.inputHandler.OnFieldChanged -= new EventHandler(this.form_OnFieldChanged);
        }
        catch
        {
        }
      }
      if (this.documentConnPt == null)
        return;
      try
      {
        this.documentConnPt.Unadvise(this.documentCookie);
        if (this.forceComRelease)
          Marshal.ReleaseComObject((object) this.documentConnPt);
      }
      catch (InvalidComObjectException ex)
      {
      }
      this.documentConnPt = (System.Runtime.InteropServices.ComTypes.IConnectionPoint) null;
    }

    public void RefreshContents(string id)
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.RefreshContents(id);
    }

    public void RefreshContents()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.RefreshContents();
      if (!(this.goToFieldID != string.Empty))
        return;
      this.inputHandler.SetGoToFieldFocus(this.goToFieldID, this.goToFieldCount);
      this.goToFieldID = string.Empty;
      this.goToFieldCount = 0;
    }

    public void RefreshLoanContents()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.RefreshLoanContents();
      if (!(this.goToFieldID != string.Empty))
        return;
      this.inputHandler.SetGoToFieldFocus(this.goToFieldID, this.goToFieldCount);
      this.goToFieldID = string.Empty;
      this.goToFieldCount = 0;
    }

    public void RefreshToolTips()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.RefreshToolTips();
    }

    public void SelectAllFields()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.SelectAllFields();
    }

    public void DeselectAllFields()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.DeselectAllFields();
    }

    public bool SetGoToFieldFocus(string goToFieldID, int count)
    {
      this.goToFieldID = goToFieldID;
      this.goToFieldCount = count;
      return true;
    }

    public void ApplyTemplateFieldAccessRights()
    {
      if (this.inputHandler == null)
        return;
      this.inputHandler.ApplyTemplateFieldAccessRights();
    }

    public void PropertyChange(string szProperty)
    {
    }

    public void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
    {
    }

    public void OnFullScreen(bool FullScreen)
    {
    }

    public void OnStatusBar(bool StatusBar)
    {
    }

    public void NewWindow2(ref object ppDisp, ref bool Cancel)
    {
    }

    public void WindowSetResizable(bool Resizable)
    {
    }

    public void PrivacyImpactedStateChange(bool bImpacted)
    {
    }

    public void WindowClosing(bool IsChildWindow, ref bool Cancel)
    {
    }

    public void WindowSetLeft(int Left)
    {
    }

    public void OnQuit()
    {
    }

    public void FileDownload(ref bool Cancel)
    {
    }

    public void OnTheaterMode(bool TheaterMode)
    {
    }

    public void BeforeNavigate2(
      object pDisp,
      ref object URL,
      ref object Flags,
      ref object TargetFrameName,
      ref object PostData,
      ref object Headers,
      ref bool Cancel)
    {
      switch (Convert.ToUInt32(Flags))
      {
        case 0:
          break;
        case 256:
          break;
        default:
          Cancel = true;
          break;
      }
    }

    public void TitleChange(string Text)
    {
    }

    public void WindowSetTop(int Top)
    {
    }

    public void OnVisible(bool Visible)
    {
    }

    public void DownloadBegin()
    {
    }

    private bool ClickEventHandler(mshtml.IHTMLEventObj e) => true;

    public void DownloadComplete()
    {
    }

    public void NavigateError(
      object pDisp,
      ref object URL,
      ref object Frame,
      ref object StatusCode,
      ref bool Cancel)
    {
    }

    public void OnMenuBar(bool MenuBar)
    {
    }

    public void NavigateComplete2(object pDisp, ref object URL)
    {
    }

    public void OnToolBar(bool ToolBar)
    {
    }

    public event DocumentCompleteEventHandler DocumentCompleted;

    public void DocumentComplete(object pDisp, ref object URL)
    {
      this.startTime = DateTime.Now;
      Uri uri = new Uri(URL.ToString());
      if (uri.Scheme == "about")
        return;
      string formId = Path.GetFileNameWithoutExtension(uri.LocalPath).ToUpper();
      if (formId == "FORM")
        formId = Path.GetFileName(Path.GetDirectoryName(uri.LocalPath).ToUpper());
      Tracing.Log(FrmBrowserHandler.sw, TraceLevel.Info, nameof (FrmBrowserHandler), "DocumentComplete, formURL: " + uri.AbsolutePath);
      HTMLDocument document = (HTMLDocument) ((SHDocVw.IWebBrowser2) pDisp).Document;
      ((System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) document).FindConnectionPoint(ref FrmBrowserHandler.documentEvents2Guid, out this.documentConnPt);
      this.documentConnPt.Advise((object) this, out this.documentCookie);
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        if (uri.Scheme.ToLower() != "file")
          throw new LicenseException("The form has attempted to load an untrusted external resource.");
        if (document.getElementsByTagName("script").length > 0)
          throw new LicenseException("This form is incompatible with Encompass and cannot be loaded.");
        this.loadInputForm(formId, uri.LocalPath, document);
        this.session.InvokeFormLoaded(this.GetAutomationFormObject());
      }
      catch (Exception ex)
      {
        Tracing.Log(FrmBrowserHandler.sw, nameof (FrmBrowserHandler), TraceLevel.Error, "Error loading input form '" + formId + "': " + (object) ex);
        switch (ex)
        {
          case CompileException _:
            this.showCompilerError(ex as CompileException);
            break;
          case AutomationException _:
            this.showAutomationError(ex as AutomationException);
            break;
          case LicenseException _:
            this.showLicenseError(ex as LicenseException);
            break;
          default:
            this.showUnexpectedError(ex);
            break;
        }
        this.clearFormWindow((SHDocVw.IWebBrowser2) pDisp);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private string getInputHandlerName(string formName)
    {
      string str = formName;
      switch (formName)
      {
        case "BORROWERSUMMARY_TOTALPAYMENT":
        case "D1003_2020MONTHLYHOUSINGEXPENSES":
        case "PREQUAL_ASSETS":
        case "PREQUAL_INCOME":
        case "PREQUAL_OTHERINCOME":
        case "PREQUAL_PRIEXPENSE":
          str = "D10032";
          break;
        case "CONSTRUCTIONMANAGEMENT_BASICINFO":
          str = "QALO";
          break;
        case "CONSTRUCTIONMANAGEMENT_LINKCONSTPERM":
          str = "PIGGYBACK";
          break;
        case "CORRESPONDENTPURCHASEADVICE":
        case "WAREHOUSEBANKDETAILS":
          str = "PURCHASEADVICE";
          break;
        case "ESCROWDETAILS":
          str = "ESCROWDETAILS";
          break;
        case "FANNIEMAEADDITIONALDATA":
          str = "STREAMLINED1003";
          break;
        case "FC_NONBORROWINGOWNERCONTACT":
          str = "NBOC";
          break;
        case "FPMS_203KPURCHASE":
        case "FPMS_203KREFI":
          str = "FPMS_FHA203K";
          break;
        case "FUNDERWORKSHEET":
          str = "UNDERWRITERSUMMARY";
          break;
        case "GSEADDITIONALPROVIDERDATA":
          str = "STREAMLINED1003";
          break;
        case "HUD1PG1_2010":
          str = "HUD1PG1";
          break;
        case "HUD1PG2":
        case "PREQUAL_REGZGFE":
          str = "REGZGFE";
          break;
        case "MONTHLY_HOUSING_EXPENSES":
          str = "D1003_2020P1";
          break;
        case "PREQUAL_REGZ50":
        case "REGZ50CLOSER":
          str = "REGZ50";
          break;
        case "PREQUAL_TOTALFINANCING":
          str = "D10033";
          break;
        case "QALP":
          str = "QALO";
          break;
        case "RE882":
          str = "RE88395";
          break;
        case "REGZGFEHUD_DETAIL1":
        case "REGZGFEHUD_DETAIL2":
          str = "REGZGFEHUDDetail";
          break;
        case "REGZGFE_2010_LOCOMP":
          str = "REGZGFE_2010";
          break;
        case "SECTION32_2015":
          str = "SECTION32";
          break;
        case "SETTLEMENTSERVICEPROVIDER2010":
          str = "SETTLEMENTSERVICEPROVIDER";
          break;
        case "USDA_RURAL1":
          str = "D10031";
          break;
        case "USDA_RURAL2":
        case "USDA_RURAL3":
          str = "D10032";
          break;
        case "USDA_RURAL4":
        case "USDA_RURAL5":
          str = "D10033";
          break;
      }
      if (formName.StartsWith("FC_") && formName != "FC_NONBORROWINGOWNERCONTACT" && formName != "FC_BORROWER" && formName != "FC_COBORROWER" && formName != "FC_SELLER" && formName != "FC_SELLER2" && formName != "FC_SELLER3" && formName != "FC_SELLER4" && formName != "FC_SELLERCORPORATIONOFFICER")
        str = "FileContact";
      if (formName.StartsWith("STATE_SPECIFIC_") && formName != "STATE_SPECIFIC_WARNING")
        str = "STATE_SPECIFIC_Generic";
      if (formName.StartsWith("ULDD"))
        str = "ULDD";
      return "EllieMae.EMLite.InputEngine." + str + "InputHandler";
    }

    private void clearFormWindow(SHDocVw.IWebBrowser2 browser)
    {
      object missing = System.Type.Missing;
      browser.Navigate("about:blank", ref missing, ref missing, ref missing, ref missing);
    }

    private void showCompilerError(CompileException ex)
    {
      using (CompileErrorDialog compileErrorDialog = new CompileErrorDialog(ex))
      {
        int num = (int) compileErrorDialog.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm);
      }
    }

    private void showAutomationError(AutomationException ex) => ErrorDialog.Display((Exception) ex);

    private void showLicenseError(LicenseException ex)
    {
      int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, ex.Message);
    }

    private void showUnexpectedError(Exception ex)
    {
      while (ex is TargetInvocationException && ex.InnerException != null)
        ex = ex.InnerException;
      if (ex.Message.IndexOf("Key cannot be null") >= 0 && ex.StackTrace.IndexOf("GenuineHttp.HttpClientConnectionManager.Pool_GetConnectionForSending") > 0)
        throw ex;
      string text = "An error has occurred on the form: " + ex.Message;
      if (EnConfigurationSettings.GlobalSettings.Debug)
        text = text + Environment.NewLine + ex.StackTrace;
      int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private IDisposable GetAPICallContextForFormInitialize(EllieMae.Encompass.Forms.Form inputForm, string formName)
    {
      System.Type type = inputForm.GetType();
      return object.Equals((object) type, (object) typeof (EllieMae.Encompass.Forms.Form)) ? (IDisposable) null : APICallContext.CreateExecutionBlock((IApiSourceContext) new APICallContext(formName, type.Assembly.ToString(), APICallSourceType.CustomForm, "CustomForm.Initialize"));
    }

    private void loadInputForm(string formId, string formPath, HTMLDocument htmldoc)
    {
      this.currentFormId = formId;
      InputFormInfo inputFormInfo = this.getInputFormInfo(formId);
      FormDescriptor frm = new FormDescriptor(formId, inputFormInfo == (InputFormInfo) null ? formId : inputFormInfo.Name, htmldoc.documentElement.outerHTML);
      EllieMae.Encompass.Forms.Form inputFormInstance = this.getInputFormInstance(frm);
      using (this.GetAPICallContextForFormInitialize(inputFormInstance, frm.FormName))
      {
        inputFormInstance.AttachToFormScreen((IFormScreen) this, inputFormInfo == (InputFormInfo) null ? (string) null : inputFormInfo.Name);
        System.Type type = System.Type.GetType(this.getInputHandlerName(formId));
        if (this.inputHandler != null)
        {
          this.inputHandler.Unload();
          this.inputHandler.Dispose();
          this.inputHandler = (IInputHandler) null;
        }
        if (this.owner is IMainScreen)
        {
          if (type != (System.Type) null)
          {
            this.inputHandler = (IInputHandler) type.GetConstructor(new System.Type[5]
            {
              typeof (Sessions.Session),
              typeof (IMainScreen),
              typeof (HTMLDocument),
              typeof (EllieMae.Encompass.Forms.Form),
              typeof (object)
            }).Invoke(new object[5]
            {
              this.session == null ? (object) Session.DefaultInstance : (object) this.session,
              (object) (this.owner as IMainScreen),
              (object) htmldoc,
              (object) inputFormInstance,
              this.property
            });
          }
          else
          {
            Tracing.Log(FrmBrowserHandler.sw, TraceLevel.Warning, nameof (FrmBrowserHandler), "no handler for " + formId);
            this.inputHandler = (IInputHandler) new InputHandlerBase(this.session == null ? Session.DefaultInstance : this.session, this.owner as IMainScreen, htmldoc, inputFormInstance, this.property);
          }
        }
        else if (this.input != null)
        {
          if (type != (System.Type) null)
          {
            this.inputHandler = (IInputHandler) type.GetConstructor(new System.Type[6]
            {
              typeof (Sessions.Session),
              typeof (IWin32Window),
              typeof (IHtmlInput),
              typeof (HTMLDocument),
              typeof (EllieMae.Encompass.Forms.Form),
              typeof (object)
            }).Invoke(new object[6]
            {
              this.session == null ? (object) Session.DefaultInstance : (object) this.session,
              (object) this.owner,
              (object) this.input,
              (object) htmldoc,
              (object) inputFormInstance,
              this.property
            });
          }
          else
          {
            Tracing.Log(FrmBrowserHandler.sw, TraceLevel.Warning, nameof (FrmBrowserHandler), "no handler for " + formId + " when handling Loan Program or Closing Cost templates");
            this.inputHandler = (IInputHandler) new InputHandlerBase(this.session == null ? Session.DefaultInstance : this.session, this.input, htmldoc, inputFormInstance, this.property);
          }
        }
        else if (type != (System.Type) null)
        {
          this.inputHandler = (IInputHandler) type.GetConstructor(new System.Type[6]
          {
            typeof (Sessions.Session),
            typeof (IWin32Window),
            typeof (LoanData),
            typeof (HTMLDocument),
            typeof (EllieMae.Encompass.Forms.Form),
            typeof (object)
          }).Invoke(new object[6]
          {
            this.session == null ? (object) Session.DefaultInstance : (object) this.session,
            (object) this.owner,
            (object) this.loanData,
            (object) htmldoc,
            (object) inputFormInstance,
            this.property
          });
        }
        else
        {
          Tracing.Log(FrmBrowserHandler.sw, TraceLevel.Warning, nameof (FrmBrowserHandler), "no handler for " + formId + " when handling Data Template");
          this.inputHandler = (IInputHandler) new InputHandlerBase(this.session == null ? Session.DefaultInstance : this.session, this.loanData, htmldoc, inputFormInstance, this.property);
        }
        if (this.goToFieldID != string.Empty)
          this.inputHandler.SetGoToFieldFocus(this.goToFieldID, this.goToFieldCount);
        if (this.inputHandler != null)
          this.inputHandler.OnFieldChanged += new EventHandler(this.form_OnFieldChanged);
        this.goToFieldID = string.Empty;
        this.goToFieldCount = 0;
        if (this.DocumentCompleted != null)
          this.DocumentCompleted();
        if (!this.setFieldsReadonly)
          return;
        this.inputHandler.SetFieldReadOnly();
      }
    }

    public LoanData CurrentLoan
    {
      get => this.loanData;
      set => this.loanData = value;
    }

    private void form_OnFieldChanged(object sender, EventArgs e)
    {
      if (this.OnFieldChanged != null)
        this.OnFieldChanged(sender, e);
      if (this.ContactFieldsChanged == null)
        return;
      this.ContactFieldsChanged((object) null, new EventArgs());
    }

    private EllieMae.Encompass.Forms.Form getInputFormInstance(FormDescriptor frm)
    {
      TypeIdentifier typeId = TypeCache.Get(frm) ?? new FormBuilder().Compile(frm, RuntimeContext.Current);
      TypeCache.Put(frm, typeId);
      if (typeId == TypeIdentifier.Empty)
        return (EllieMae.Encompass.Forms.Form) null;
      if (this.session.IsBrokerEdition())
        return new EllieMae.Encompass.Forms.Form();
      if (this.session.StandardFormCodebaseTypes.Contains((object) frm.FormName))
        return (EllieMae.Encompass.Forms.Form) ((System.Type) this.session.StandardFormCodebaseTypes[(object) frm.FormName]).InvokeMember((string) null, BindingFlags.CreateInstance, (Binder) null, (object) null, (object[]) null);
      return typeId == FormBuilder.AutomationFormTypeID ? new EllieMae.Encompass.Forms.Form() : (EllieMae.Encompass.Forms.Form) RuntimeContext.Current.CreateInstance(typeId, typeof (EllieMae.Encompass.Forms.Form));
    }

    private void writeCheckpoint(string text)
    {
      Console.WriteLine((DateTime.Now - this.startTime).TotalMilliseconds.ToString("00000") + " " + text);
    }

    public void PrintTemplateInstantiation(object pDisp)
    {
    }

    public void SetSecureLockIcon(int SecureLockIcon)
    {
    }

    public void ClientToHostWindow(ref int CX, ref int CY)
    {
    }

    public void ProgressChange(int Progress, int ProgressMax)
    {
    }

    public void WindowSetWidth(int Width)
    {
    }

    public void StatusTextChange(string Text)
    {
    }

    public void WindowSetHeight(int Height)
    {
    }

    public void PrintTemplateTeardown(object pDisp)
    {
    }

    public void CommandStateChange(int Command, bool Enable)
    {
    }

    public void NewWindow3(
      ref object ppDisp,
      ref bool Cancel,
      uint dwFlags,
      string bstrUrlContext,
      string bstrUrl)
    {
    }

    public void ondataavailable(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onbeforedeactivate(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onstop(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onrowsinserted(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onselectstart(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onkeypress(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onhelp(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onpropertychange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void oncellchange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool oncontextmenu(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.ContactFieldsChanged != null)
        this.ContactFieldsChanged((object) pEvtObj, new EventArgs());
      return false;
    }

    public bool ondblclick(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onfocusin(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void ondatasetcomplete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.helpTarget != null && pEvtObj.keyCode == 112)
        JedHelp.ShowHelp((System.Windows.Forms.Control) this.owner, SystemSettings.HelpFile, this.helpTarget.GetHelpTargetName());
      else if (pEvtObj.altKey && pEvtObj.ctrlKey && pEvtObj.keyCode == 65)
      {
        if (!(this.currentFormId == "REGZ50CLOSER"))
          return;
        (this.session == null ? Session.Application.GetService<IEPass>() : this.session.Application.GetService<IEPass>()).ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;AUDIT", false);
        Session.Application.GetService<ILoanEditor>().RefreshContents("1885");
      }
      else
      {
        if (this.FieldOnKeyUp == null || pEvtObj.keyCode == 9)
          return;
        this.FieldOnKeyUp((object) pEvtObj, new EventArgs());
      }
    }

    public void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onbeforeeditfocus(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool ondragstart(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool oncontrolselect(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onactivate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.FieldOnKeyUp == null || pEvtObj.keyCode == 9)
        return;
      object attribute = pEvtObj.srcElement.getAttribute("emid");
      if (attribute == null)
        return;
      string str = attribute.ToString();
      if (str == string.Empty)
        str = pEvtObj.srcElement.getAttribute("for").ToString();
      if (!((str ?? "") != string.Empty))
        return;
      this.FieldOnKeyUp((object) pEvtObj, new EventArgs());
    }

    public bool onbeforeactivate(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onkeydown(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onrowexit(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onbeforeupdate(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onrowsdelete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onreadystatechange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmousemove(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onrowenter(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onafterupdate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void ondeactivate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onselectionchange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void ondatasetchanged(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmouseover(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public bool onmousewheel(mshtml.IHTMLEventObj pEvtObj) => true;

    public bool onerrorupdate(mshtml.IHTMLEventObj pEvtObj) => true;

    public void onmouseout(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public void onmousedown(mshtml.IHTMLEventObj pEvtObj)
    {
      (this.session == null ? Session.Application.GetService<IEncompassApplication>() : this.session.Application.GetService<IEncompassApplication>())?.GetTipControl().Close();
    }

    public bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.ButtonClicked != null && pEvtObj != null && (pEvtObj.srcElement.className == "inputButton" || pEvtObj.srcElement.className == "inputButtonImage"))
      {
        string sender = pEvtObj.srcElement.getAttribute("emid").ToString();
        if ((sender ?? "") != string.Empty)
          this.ButtonClicked((object) sender, new EventArgs());
      }
      return true;
    }

    private InputFormInfo getInputFormInfo(string formId)
    {
      lock (this.formCache)
      {
        if (this.formCache.Count == 0)
        {
          foreach (InputFormInfo allFormInfo in this.session.FormManager.GetAllFormInfos())
            this.formCache[allFormInfo.FormID] = allFormInfo;
        }
        return this.formCache.ContainsKey(formId) ? this.formCache[formId] : (InputFormInfo) null;
      }
    }

    internal bool SetFieldsReadonly
    {
      set => this.setFieldsReadonly = value;
    }
  }
}
