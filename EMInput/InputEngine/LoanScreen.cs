// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanScreen
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanScreen : WorkAreaPanelBase, IFormScreen, IRefreshContents
  {
    private const string className = "LoanScreen";
    private static string sw = Tracing.SwInputEngine;
    protected AxWebBrowser axWebBrowser;
    private TextBox page4Box;
    private Panel customPanel;
    protected LoanData loan;
    protected FrmBrowserHandler brwHandler;
    protected static object nobj = (object) Missing.Value;
    private IWin32Window parentWindow;
    private IContainer components;
    private StandardIconButton iconReset;
    private StandardIconButton iconNBOCNew;
    private StandardIconButton iconNBOCRemove;
    private ToolTip toolTip1;
    private IHtmlInput inputData;
    public static string STATESPECIFICFORMS = ",AK,AR,AZ,CA,CO,CT,DE,DC,FL,GA,HI,ID,IL,IA,IN,KS,KY,LA,ME,MD,MA,MI,MN,MS,MO,MT,NE,NV,NH,NJ,NM,NY,NC,ND,OH,OR,PA,RI,SC,TN,TX,VA,VT,WA,WI,WY,";
    private Sessions.Session session;
    private InputFormInfo currentForm;
    private bool initPage4;
    private string orgVal = string.Empty;
    private object objectContainer;

    public event EventHandler FormChanged;

    public event EventHandler OnFieldChanged;

    public event EventHandler FieldOnKeyUp;

    public event EventHandler ButtonClicked;

    public event EventHandler ContactFieldsChanged;

    public event EventHandler FormLoaded;

    public LoanScreen(Sessions.Session session, IWin32Window parentWindow, IHtmlInput inputData)
    {
      this.session = session;
      this.parentWindow = parentWindow;
      this.inputData = inputData;
      this.loan = !(this.inputData is LoanData) ? (LoanData) null : (LoanData) this.inputData;
      this.InitializeComponent();
      this.titleLbl.ForeColor = Color.Black;
      this.hookUpBrowserHandler();
    }

    public LoanScreen(Sessions.Session session)
      : this(session, (IWin32Window) Session.MainScreen, (IHtmlInput) session.LoanData)
    {
    }

    public void ShowFormTitle() => this.titlePanel.Visible = true;

    public void HideFormTitle() => this.titlePanel.Visible = false;

    public void SetHelpTarget(IOnlineHelpTarget helpTarget)
    {
      this.brwHandler.SetHelpTarget(helpTarget);
    }

    public FrmBrowserHandler BrwHandler => this.brwHandler;

    public IInputHandler GetInputHandler() => this.brwHandler.GetInputHandler();

    public object GetAutomationFormObject() => this.brwHandler.GetAutomationFormObject();

    public void ExecAction(string action) => this.brwHandler.ExecAction(action);

    public void UpdateCurrentField() => this.brwHandler.UpdateCurrentField();

    public void RefreshControl(string controlId) => this.brwHandler.RefreshControl(controlId);

    public void RefreshAllControls(bool force) => this.brwHandler.RefreshAllControls(force);

    public void Popup(string formName, string title, int width, int height)
    {
      this.brwHandler.Popup(formName, title, width, height);
    }

    private void hookUpBrowserHandler()
    {
      this.brwHandler = !(this.inputData is LoanData) ? new FrmBrowserHandler(this.session, (IWin32Window) null, this.inputData) : new FrmBrowserHandler(this.session, this.parentWindow, this.loan);
      this.brwHandler.FieldOnKeyUp += new EventHandler(this.brwHandler_FieldOnKeyUp);
      this.brwHandler.ButtonClicked += new EventHandler(this.brwHandler_ButtonClicked);
      this.brwHandler.ContactFieldsChanged += new EventHandler(this.brwHandler_ContactFieldsChanged);
      this.brwHandler.OnFieldChanged += new EventHandler(this.brwHandler_OnFieldChanged);
      this.brwHandler.DocumentCompleted += new DocumentCompleteEventHandler(this.brwHandler_DocumentCompleted);
      this.brwHandler.AttachToBrowser(this.axWebBrowser);
      this.axWebBrowser.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(this.frmBrowser_DocumentComplete);
    }

    private void brwHandler_DocumentCompleted()
    {
      if (this.FormLoaded == null)
        return;
      this.FormLoaded((object) this, EventArgs.Empty);
    }

    private void brwHandler_OnFieldChanged(object sender, EventArgs e)
    {
      if (this.OnFieldChanged == null)
        return;
      this.OnFieldChanged(sender, e);
    }

    private void frmBrowser_DocumentComplete(
      object sender,
      DWebBrowserEvents2_DocumentCompleteEvent e)
    {
      if (this.FormChanged == null)
        return;
      string sender1 = e.uRL.ToString();
      int length = sender1.LastIndexOf("\\");
      if (length > -1)
      {
        sender1 = sender1.Substring(0, length);
        int num = sender1.LastIndexOf("\\");
        if (num > -1)
          sender1 = sender1.Substring(num + 1);
      }
      EventArgs e1 = new EventArgs();
      this.FormChanged((object) sender1, e1);
    }

    private void brwHandler_ButtonClicked(object sender, EventArgs e)
    {
      if (this.ButtonClicked == null)
        return;
      this.ButtonClicked(sender, e);
    }

    protected override bool ProcessDialogKey(Keys keyData)
    {
      return this.brwHandler.ProcessDialogKey(keyData) || base.ProcessDialogKey(keyData);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.Unload();
        this.ClearCustomPanel();
        if (this.brwHandler != null)
          this.brwHandler.Dispose();
        this.DisposeCustomControl((Control) this.customPanel);
        this.DisposeCustomControl((Control) this.titlePanel);
        this.DisposeCustomControl((Control) this.iconReset);
        this.DisposeCustomControl((Control) this.iconNBOCNew);
        this.DisposeCustomControl((Control) this.iconNBOCRemove);
        this.toolTip1.Dispose();
        if (this.brwHandler != null)
          this.brwHandler.Dispose();
        this.axWebBrowser.Dispose();
        this.axWebBrowser = (AxWebBrowser) null;
        this.components.Dispose();
      }
      base.Dispose(disposing);
      GC.Collect();
      GC.WaitForPendingFinalizers();
      Marshal.CleanupUnusedObjectsInCurrentContext();
      GC.Collect();
    }

    private void DisposeCustomControl(Control myCtrl)
    {
      if (myCtrl.Controls != null && myCtrl.Controls.Count > 0)
      {
        foreach (Component control in (ArrangedElementCollection) myCtrl.Controls)
          control.Dispose();
      }
      myCtrl.Controls.Clear();
      myCtrl.Dispose();
    }

    public InputFormInfo CurrentForm => this.currentForm;

    public LoanData CurrentLoan
    {
      get => this.loan;
      set
      {
        this.loan = value;
        if (this.brwHandler == null)
          return;
        this.brwHandler.CurrentLoan = value;
      }
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanScreen));
      this.axWebBrowser = new AxWebBrowser();
      this.customPanel = new Panel();
      this.page4Box = new TextBox();
      this.iconReset = new StandardIconButton();
      this.iconNBOCNew = new StandardIconButton();
      this.iconNBOCRemove = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.titlePanel.SuspendLayout();
      this.contentPanel.SuspendLayout();
      ((ISupportInitialize) this.iconReset).BeginInit();
      ((ISupportInitialize) this.iconNBOCNew).BeginInit();
      ((ISupportInitialize) this.iconNBOCRemove).BeginInit();
      this.axWebBrowser.BeginInit();
      this.SuspendLayout();
      this.titlePanel.Size = new Size(1167, 26);
      this.iconReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconReset.BackColor = Color.Transparent;
      this.iconReset.Location = new Point(0, 5);
      this.iconReset.Name = "iconReset";
      this.iconReset.Size = new Size(16, 16);
      this.iconReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.iconReset.TabIndex = 9;
      this.iconReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconReset, "Reset Lock Request Form");
      this.iconReset.Visible = false;
      this.iconReset.Click += new EventHandler(this.iconReset_Click);
      this.iconNBOCNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconNBOCNew.BackColor = Color.Transparent;
      this.iconNBOCNew.Location = new Point(0, 5);
      this.iconNBOCNew.Name = "iconNBOCNew";
      this.iconNBOCNew.Size = new Size(16, 16);
      this.iconNBOCNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.iconNBOCNew.TabIndex = 9;
      this.iconNBOCNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconNBOCNew, "Add New Non-Borrowing Owner File Contact");
      this.iconNBOCNew.Visible = false;
      this.iconNBOCNew.Click += new EventHandler(this.iconNBOCNew_Click);
      this.iconNBOCRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconNBOCRemove.BackColor = Color.Transparent;
      this.iconNBOCRemove.Location = new Point(0, 5);
      this.iconNBOCRemove.Name = "iconNBOCNew";
      this.iconNBOCRemove.Size = new Size(16, 16);
      this.iconNBOCRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.iconNBOCRemove.TabIndex = 9;
      this.iconNBOCRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconNBOCRemove, "Remove New Non-Borrowing Owner File Contact");
      this.iconNBOCRemove.Visible = false;
      this.iconNBOCRemove.Click += new EventHandler(this.iconNBOCRemove_Click);
      this.contentPanel.Controls.Add((Control) this.customPanel);
      this.contentPanel.Controls.Add((Control) this.page4Box);
      this.contentPanel.Controls.Add((Control) this.axWebBrowser);
      this.contentPanel.Size = new Size(1167, 556);
      this.axWebBrowser.Dock = DockStyle.Fill;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(0, 0);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(1167, 556);
      this.axWebBrowser.TabIndex = 0;
      this.customPanel.AutoScroll = true;
      this.customPanel.BorderStyle = BorderStyle.Fixed3D;
      this.customPanel.Dock = DockStyle.Fill;
      this.customPanel.Location = new Point(0, 0);
      this.customPanel.Name = "customPanel";
      this.customPanel.Size = new Size(1167, 556);
      this.customPanel.TabIndex = 0;
      this.page4Box.Dock = DockStyle.Fill;
      this.page4Box.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.page4Box.Location = new Point(0, 0);
      this.page4Box.Multiline = true;
      this.page4Box.Name = "page4Box";
      this.page4Box.ScrollBars = ScrollBars.Both;
      this.page4Box.Size = new Size(1167, 556);
      this.page4Box.TabIndex = 1;
      this.page4Box.Leave += new EventHandler(this.page4Box_Leave);
      this.page4Box.BorderStyle = BorderStyle.None;
      this.AutoScroll = true;
      this.Size = new Size(1167, 582);
      this.titlePanel.ResumeLayout(false);
      this.contentPanel.ResumeLayout(false);
      this.contentPanel.PerformLayout();
      ((ISupportInitialize) this.iconReset).EndInit();
      ((ISupportInitialize) this.iconNBOCNew).EndInit();
      ((ISupportInitialize) this.iconNBOCRemove).EndInit();
      this.axWebBrowser.EndInit();
      this.ResumeLayout(false);
    }

    private InputFormInfo getInputFormInfo(string formId)
    {
      if (formId == "CUSTOMFIELDS")
        formId = "CF_1";
      return formId == "CF_1" || formId == "CF_2" || formId == "CF_3" || formId == "CF_4" ? new InputFormInfo(formId, "Custom Fields", InputFormType.Virtual) : this.session.FormManager.GetFormInfo(formId);
    }

    public void LoadForm(string formId)
    {
      InputFormInfo inputFormInfo = this.getInputFormInfo(formId);
      if (inputFormInfo == (InputFormInfo) null)
        throw new ArgumentException("The form identifier '" + formId + "' is not found");
      this.LoadForm(inputFormInfo);
    }

    public bool AllowUnloadForm() => this.brwHandler == null || this.brwHandler.AllowUnloadForm();

    public void LoadForm(InputFormInfo form)
    {
      if (form.FormID == "STATEDISCLOSUREINFO")
        form = this.getStateSpecificForm();
      else if (form.FormID == "SECTION32" && !this.loan.Use2010RESPA && !this.loan.Use2015RESPA)
        form = new InputFormInfo("SECTION32_2009", "Section 32 &HOEPA");
      else if (form.FormID == "SECTION32" && this.loan.Use2015RESPA)
        form = new InputFormInfo("SECTION32_2015", "Section 32 &HOEPA");
      this.currentForm = form;
      if (this.FormChanged != null)
        this.FormChanged((object) this, new EventArgs());
      this.iconReset.Visible = false;
      this.iconNBOCNew.Visible = false;
      this.iconNBOCRemove.Visible = false;
      this.ClearCustomPanel();
      switch (form.FormID)
      {
        case "BORVESTING":
        case "BROKERCHECKCALCULATION":
        case "CORRPURCHASEADVISEPAYMENTHISTORY":
        case "FUNDBALANCINGWORKSHEET":
        case "FUNDINGWORKSHEET":
        case "INTERIMSERVICING":
        case "REGZGFE_2015":
          this.LoadWindowsForm(form.FormID);
          return;
        case "CF_1":
        case "CUSTOMFIELDS":
          this.LoadCustomFields(1);
          return;
        case "CF_2":
          this.LoadCustomFields(2);
          return;
        case "CF_3":
          this.LoadCustomFields(3);
          return;
        case "CF_4":
          this.LoadCustomFields(4);
          return;
        case "D10034":
          this.loadD1003Page4();
          this.BringToFront();
          return;
        case "FILECONTACTS":
          this.titlePanel.Controls.Add((Control) this.iconNBOCRemove);
          this.iconNBOCRemove.Left = this.titlePanel.Width - this.iconNBOCRemove.Width - 8;
          this.iconNBOCRemove.Visible = true;
          this.iconNBOCRemove.BringToFront();
          this.titlePanel.Controls.Add((Control) this.iconNBOCNew);
          this.iconNBOCNew.Left = this.iconNBOCRemove.Left - this.iconNBOCNew.Width - 8;
          this.iconNBOCNew.Visible = true;
          this.iconNBOCNew.BringToFront();
          this.LoadWindowsForm(form.FormID);
          return;
        case "LOLOCKREQUEST":
          this.titlePanel.Controls.Add((Control) this.iconReset);
          this.iconReset.Left = this.titlePanel.Width - this.iconReset.Width - 8;
          this.iconReset.Visible = true;
          this.iconReset.BringToFront();
          break;
      }
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.PerformanceEnabled = !(form.FormID == "REGZ50") && !(form.FormID == "REGZ50CLOSER");
      if (form != (InputFormInfo) null && form.FormID == "REGZGFE_2015HTML")
      {
        form = new InputFormInfo("REGZGFE_2015", "2015 Itemization");
        this.currentForm = form;
      }
      this.axWebBrowser.Enabled = true;
      this.page4Box.Enabled = false;
      this.customPanel.Enabled = false;
      this.Text = form.Name;
      this.brwHandler.UnloadForm();
      string formHtmlPath = FormStore.GetFormHTMLPath(this.session, form);
      this.axWebBrowser.Navigate(formHtmlPath, ref LoanScreen.nobj, ref LoanScreen.nobj, ref LoanScreen.nobj, ref LoanScreen.nobj);
      if (Tracing.IsSwitchActive(LoanScreen.sw, TraceLevel.Verbose))
        Tracing.Log(LoanScreen.sw, TraceLevel.Verbose, nameof (LoanScreen), "LoadForm: " + formHtmlPath);
      this.axWebBrowser.BringToFront();
      this.BringToFront();
    }

    public void Unload()
    {
      if (this.brwHandler != null)
      {
        this.brwHandler.UnloadForm();
        this.brwHandler.FieldOnKeyUp -= new EventHandler(this.brwHandler_FieldOnKeyUp);
        this.brwHandler.ButtonClicked -= new EventHandler(this.brwHandler_ButtonClicked);
        this.brwHandler.ContactFieldsChanged -= new EventHandler(this.brwHandler_ContactFieldsChanged);
        this.brwHandler.OnFieldChanged -= new EventHandler(this.brwHandler_OnFieldChanged);
        this.brwHandler.DocumentCompleted -= new DocumentCompleteEventHandler(this.brwHandler_DocumentCompleted);
        this.brwHandler.CurrentLoan = (LoanData) null;
      }
      if (this.axWebBrowser != null)
        this.axWebBrowser.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(this.frmBrowser_DocumentComplete);
      this.ClearCustomPanel();
      this.loan = (LoanData) null;
    }

    public void RefreshContents(string id) => this.brwHandler.RefreshContents(id);

    public void RefreshContents()
    {
      if (this.customPanel.Enabled)
      {
        foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
        {
          if (control is IRefreshContents)
            ((IRefreshContents) control).RefreshContents();
        }
      }
      else
        this.brwHandler.RefreshContents();
    }

    public void RefreshLoanContents()
    {
      if (this.customPanel.Enabled)
      {
        foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
        {
          if (control is IRefreshContents)
            ((IRefreshContents) control).RefreshLoanContents();
        }
      }
      else
        this.brwHandler.RefreshLoanContents();
    }

    public void RefreshToolTips() => this.brwHandler.RefreshToolTips();

    private void loadD1003Page4()
    {
      this.page4Box.Enabled = true;
      this.initPage4 = true;
      this.orgVal = this.loan != null ? this.loan.GetField("1003p4") : "";
      this.page4Box.Text = this.orgVal;
      this.page4Box.BringToFront();
      this.customPanel.Enabled = false;
      this.axWebBrowser.Enabled = false;
      this.page4Box.Focus();
      this.initPage4 = false;
    }

    public void ClearCustomPanel()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
      {
        if (control is Itemization2015Container)
          ((Itemization2015Container) control).ClearCurrentFieldObject();
        arrayList.Add((object) control);
      }
      this.customPanel.Controls.Clear();
      this.customPanel.PerformLayout();
      foreach (Component component in arrayList)
        component.Dispose();
    }

    public void LoadWindowsForm(string formID)
    {
      if (formID == "D10034")
      {
        this.loadD1003Page4();
      }
      else
      {
        this.customPanel.Enabled = true;
        this.ClearCustomPanel();
        Control control = (Control) null;
        if (formID == "BORVESTING")
          control = (Control) new BorrowerVestingForm(this.loan);
        else if (formID == "FILECONTACTS")
        {
          FileContactForm fileContactForm = new FileContactForm(this.session, this.loan, this.parentWindow);
          fileContactForm.FileContactSelectedIndexChanged += new EventHandler(this.fileContact_SelectedIndexChanged);
          control = (Control) fileContactForm;
          this.iconNBOCRemove.Enabled = false;
        }
        else if (formID == "FUNDINGWORKSHEET")
          control = (Control) new FundingWorksheet(this.loan, this.session);
        else if (formID == "BROKERCHECKCALCULATION")
          control = (Control) new BrokerCheckForm(this.loan);
        else if (formID == "CORRPURCHASEADVISEPAYMENTHISTORY")
          control = (Control) new CorrPurchAdviceAmortizationForm(this.loan);
        else if (formID == "FUNDBALANCINGWORKSHEET")
          control = (Control) new LoanBalancingWorksheet(this.loan);
        else if (formID == "INTERIMSERVICING")
        {
          ServicingDetailsWorksheet detailsWorksheet = new ServicingDetailsWorksheet(this.loan);
          this.brwHandler = detailsWorksheet.BrowserHandler;
          this.objectContainer = (object) detailsWorksheet;
          control = (Control) detailsWorksheet;
        }
        else if (formID == "REGZGFE_2015")
        {
          Itemization2015Container itemization2015Container = new Itemization2015Container(this.session, this.parentWindow, this.loan);
          itemization2015Container.FormLoaded += new EventHandler(this.itemizationContainer_FormLoaded);
          control = (Control) itemization2015Container;
        }
        else if (formID == "HomeCounselingProviders")
        {
          List<KeyValuePair<string, string>> serviceList = new List<KeyValuePair<string, string>>();
          List<KeyValuePair<string, string>> languageList = new List<KeyValuePair<string, string>>();
          HomeCounselingProviderAPI.updateLanguageServiceCodes(this.session, ref serviceList, ref languageList);
          control = (Control) new AgencyControl(this.session, (IHtmlInput) this.loan, serviceList, languageList);
        }
        this.customPanel.Controls.Add(control);
        this.customPanel.BorderStyle = BorderStyle.None;
        this.customPanel.BringToFront();
        this.customPanel.BackColor = Color.Gainsboro;
        this.page4Box.Enabled = false;
        this.axWebBrowser.Enabled = false;
        control.Focus();
      }
    }

    private void itemizationContainer_FormLoaded(object sender, EventArgs e)
    {
      if (this.brwHandler.GetInputHandler() is REGZGFE_2015InputHandler)
        return;
      this.brwHandler.SetInputHandler(((Itemization2015Container) sender).GetInputHandler());
    }

    public void Clear2015Itemization()
    {
      if (this.customPanel == null)
        return;
      foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
      {
        if (control is Itemization2015Container)
        {
          control.Dispose();
          break;
        }
      }
    }

    public void ClearFeeDetailsPopup()
    {
      if (this.customPanel == null)
        return;
      foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
      {
        if (control is Itemization2015Container)
        {
          ((Itemization2015Container) control).ClearFeeDetailsPopup();
          break;
        }
      }
    }

    public void SetGoToFieldInHybridForm(string fieldID)
    {
      this.recursiveSetGoToFieldInHybridForm(fieldID, this.customPanel.Controls);
    }

    private bool recursiveSetGoToFieldInHybridForm(
      string fieldID,
      Control.ControlCollection conCollection)
    {
      foreach (Control con in (ArrangedElementCollection) conCollection)
      {
        if (con is LoanScreen)
        {
          ((LoanScreen) con).SetGoToFieldFocus(fieldID, 1);
          return true;
        }
        if (con.Controls != null && con.Controls.Count > 0 && this.recursiveSetGoToFieldInHybridForm(fieldID, con.Controls))
          return true;
      }
      return false;
    }

    public void LoadCustomFields(int page)
    {
      this.customPanel.Enabled = true;
      this.ClearCustomPanel();
      CustomFieldsForm customFieldsForm = new CustomFieldsForm(page, this.loan);
      this.customPanel.Controls.Add((Control) customFieldsForm);
      this.customPanel.Visible = true;
      this.customPanel.BringToFront();
      this.page4Box.Enabled = false;
      this.axWebBrowser.Enabled = false;
      customFieldsForm.Focus();
    }

    public void SelectAllFields() => this.brwHandler.SelectAllFields();

    public void DeselectAllFields() => this.brwHandler.DeselectAllFields();

    public bool SetGoToFieldFocus(string fieldID, int count)
    {
      return this.brwHandler.SetGoToFieldFocus(fieldID, count);
    }

    public void ApplyTemplateFieldAccessRights()
    {
      this.brwHandler.ApplyTemplateFieldAccessRights();
    }

    private void page4Box_Leave(object sender, EventArgs e)
    {
      if (this.initPage4)
        return;
      string text = this.page4Box.Text;
      if (!(this.orgVal != text))
        return;
      this.loan.SetField("1003p4", text);
      this.orgVal = text;
    }

    private void brwHandler_FieldOnKeyUp(object sender, EventArgs e)
    {
      if (this.FieldOnKeyUp == null)
        return;
      this.FieldOnKeyUp(sender, e);
    }

    private void brwHandler_ContactFieldsChanged(object sender, EventArgs e)
    {
      if (this.ContactFieldsChanged == null)
        return;
      this.ContactFieldsChanged(sender, e);
    }

    public bool GetInputEngineService(LoanData loan, InputEngineServiceType serviceType)
    {
      switch (serviceType)
      {
        case InputEngineServiceType.ShowLPSelectorForLoan:
        case InputEngineServiceType.ShowLPSelectorForLockRequest:
          using (LoanProgramSelect loanProgramSelect = new LoanProgramSelect(loan, serviceType == InputEngineServiceType.ShowLPSelectorForLoan ? LoanProgramSelect.SelectTypes.ForLoan : LoanProgramSelect.SelectTypes.ForLockRequest, this.session))
          {
            if (loanProgramSelect.ShowDialog((IWin32Window) Session.MainScreen) == DialogResult.OK)
              return true;
            break;
          }
        case InputEngineServiceType.ShowCCSelectorForLoan:
          using (ClosingCostSelect closingCostSelect = new ClosingCostSelect(loan))
          {
            if (closingCostSelect.ShowDialog((IWin32Window) Session.MainScreen) == DialogResult.OK)
              return true;
            break;
          }
      }
      return false;
    }

    public FrmBrowserHandler BrowserHandler => this.brwHandler;

    public object ObjectContainer => this.objectContainer;

    private void iconReset_Click(object sender, EventArgs e) => this.ExecAction("resetrequestform");

    private void iconNBOCNew_Click(object sender, EventArgs e)
    {
      foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
      {
        if (control is FileContactForm && control.Visible)
          ((FileContactForm) control).AddNewNonBorrowingOwnerContact();
      }
    }

    private void iconNBOCRemove_Click(object sender, EventArgs e)
    {
      foreach (Control control in (ArrangedElementCollection) this.customPanel.Controls)
      {
        if (control is FileContactForm && control.Visible)
        {
          ((FileContactForm) control).RemoveNewNonBorrowingOwnerContact();
          this.loan.SetCurrentField("NewVestingNboAlert", "");
          Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        }
      }
    }

    private void fileContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (sender is FileContactRecord.ContactFields && ((FileContactRecord.ContactFields) sender).ContactType == FileContactRecord.ContactTypes.NonBorrowingOwnerContact)
        this.iconNBOCRemove.Enabled = true;
      else
        this.iconNBOCRemove.Enabled = false;
    }

    private InputFormInfo getStateSpecificForm()
    {
      string field = this.loan.GetField("14");
      if (LoanScreen.STATESPECIFICFORMS.IndexOf("," + field + ",") <= -1)
        return new InputFormInfo("State_Specific_Warning", "State Specific Information");
      string str = "State Specific Information - " + Utils.GetFullStateName(field);
      this.SetTitle(str);
      return new InputFormInfo("State_Specific_" + field, str);
    }

    internal void SetFieldsReadonly() => this.BrowserHandler.SetFieldsReadonly = true;
  }
}
