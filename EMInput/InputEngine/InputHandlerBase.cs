// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InputHandlerBase
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.ExternalOriginatorManagement;
using EllieMae.EMLite.InputEngine.MilestoneManagement;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using EllieMae.EMLite.Xml;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms;
using JEDCOMBO2Lib;
using Microsoft.Win32;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InputHandlerBase : 
    HTMLInputTextElementEvents2,
    HTMLSelectElementEvents2,
    HTMLButtonElementEvents2,
    HTMLAnchorEvents2,
    HTMLImgEvents2,
    IInputHandler,
    IFormScreen,
    IRefreshContents,
    _IComboFullEvents
  {
    private const string className = "InputHandlerBase";
    protected static string sw = Tracing.SwInputEngine;
    public static System.Drawing.Color ExistingFieldColor = System.Drawing.Color.FromArgb(200, 227, 254);
    public static System.Drawing.Color SelectedFieldColor = System.Drawing.Color.FromArgb(254, 232, 208);
    public static string HTMLExistingColor = "2px solid #00FFFF";
    public static string HTMLSelectedColor = "2px solid #DEB887";
    protected bool LockRequestInfoDiff;
    private bool forceComRelease;
    private bool notAllowedPricingChangeSetting;
    private bool Use5DecimalForIndexRates;
    private static int lockLoanNumber = -1;
    private static readonly Guid guidInput = typeof (HTMLInputTextElementEvents2).GUID;
    private static readonly Guid guidSelect = typeof (HTMLSelectElementEvents2).GUID;
    private static readonly Guid guidButton = typeof (HTMLButtonElementEvents2).GUID;
    private static readonly Guid guidAnchor = typeof (HTMLAnchorEvents2).GUID;
    private static readonly Guid guidImage = typeof (HTMLImgEvents2).GUID;
    private static readonly Guid guidJedCombo = typeof (_IComboFullEvents).GUID;
    private static Hashtable docIdTbl = new Hashtable();
    private static HashSet<string> StateCodeFields = new HashSet<string>()
    {
      "VEND.X683",
      "VEND.X688",
      "VEND.X692",
      "VEND.X697",
      "VEND.X701",
      "VEND.X706",
      "VEND.X710",
      "VEND.X715",
      "VEND.X719",
      "VEND.X724",
      "VEND.X728",
      "VEND.X733",
      "VEND.X737",
      "VEND.X742",
      "VEND.X926",
      "VEND.X931",
      "VEND.X746",
      "VEND.X751",
      "VEND.X755",
      "VEND.X760",
      "VEND.X764",
      "VEND.X769",
      "VEND.X773",
      "VEND.X778",
      "VEND.X782",
      "VEND.X787",
      "VEND.X791",
      "VEND.X796",
      "VEND.X800",
      "VEND.X805",
      "VEND.X809",
      "VEND.X814",
      "VEND.X818",
      "VEND.X823",
      "VEND.X827",
      "VEND.X832",
      "VEND.X836",
      "VEND.X841",
      "VEND.X845",
      "VEND.X850",
      "VEND.X854",
      "VEND.X859",
      "VEND.X863",
      "VEND.X868",
      "VEND.X872",
      "VEND.X877",
      "VEND.X881",
      "VEND.X886",
      "VEND.X890",
      "VEND.X895",
      "VEND.X899",
      "VEND.X904",
      "VEND.X908",
      "VEND.X913",
      "VEND.X917",
      "VEND.X922",
      "VEND.X666",
      "VEND.X679",
      "LE3.X20",
      "LE3.X21",
      "LE3.X22",
      "LE3.X23"
    };
    protected HTMLDocument htmldoc;
    protected EllieMae.Encompass.Forms.Form currentForm;
    protected LoanData loan;
    protected IHtmlInput inputData;
    protected ILoanEditor editor;
    protected IEFolder efolder;
    protected IEPass epass;
    protected bool modified;
    protected FieldControl currentField;
    protected ArrayList cons = new ArrayList();
    public bool FlushOutEnabled = true;
    private bool reformatPending;
    private bool isDeactivating;
    private bool isDeleted;
    private bool forceChangeEvent;
    private Stack eventStack = new Stack();
    protected IMainScreen mainScreen;
    private bool isTabbingForward = true;
    private bool isMouseClicked;
    private bool ignoreAsyncFocus;
    private FeaturesAclManager aclMgr;
    private Hashtable fieldRights;
    private FeeManagementSetting feeManagementSetting;
    private FeeManagementPersonaInfo feeManagementPermission;
    internal LOCompensationSetting loCompensationSetting;
    private POCInputHandler pocInputHandler;
    private bool isSuperAdmin;
    public PiggybackFields PiggyFields;
    private bool? isBorrowerContactLinked;
    private bool? isCoBorrowerContactLinked;
    private string previousZip = "";
    private bool isUsingTemplate;
    private const string trigList = "|36|37|68|69|31|1602|11|URLA.X73|URLA.X74|URLA.X75|12|14|15|FR0104|FR0106|FR0107|FR0108|FR0204|FR0206|FR0207|FR0208|4000|4001|4002|4003|4004|4005|4006|4007";
    private const string fieldsToTriggerPaySchedule = "|1724|1725|78|81|109|118|799|1206|1391|1961|3034|3054|3119|3120|4088|NEWHUD.X11|NEWHUD.X217|RE88395.X290|RE88395.X291|RE88395.X294|RE88395.X296|RE88395.X323|RE88395.X324|RE88395.X325|RE88395.X327|LE1.X92";
    protected string CurrentFormName = string.Empty;
    protected Sessions.Session session;
    private string itemizationLineNumber;
    private static int inputFormAutoFocus = -1;
    private Hashtable lateChargeTable;
    private int fieldLockClickCount;
    internal ExternalOriginatorManagementData company;
    internal ExternalOriginatorManagementData branch;
    internal ExternalUserInfo loUser;
    internal bool loFlag = true;
    private List<ExternalOriginatorManagementData> orgs;
    internal List<ExternalOriginatorManagementData> TPOBranches;
    private Hashtable allowedForms;
    internal bool allFieldsAreReadonly;

    public event EventHandler OnFieldChanged;

    public static bool LockLoanNumber
    {
      get
      {
        if (UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas) || Session.UserInfo.IsAdministrator() && Session.UserInfo.OrgId == 0)
          return false;
        if (InputHandlerBase.lockLoanNumber < 0 && Session.LoanDataMgr != null)
          InputHandlerBase.lockLoanNumber = Session.LoanDataMgr.SystemConfiguration.LockLoanNumber ? 1 : 0;
        return InputHandlerBase.lockLoanNumber > 0;
      }
    }

    public Hashtable FieldRights => this.fieldRights;

    public bool IsUsingTemplate => this.isUsingTemplate;

    public bool IsAnyFieldTouched { get; set; }

    static InputHandlerBase()
    {
      InputHandlerBase.docIdTbl[(object) "FL"] = (object) new string[4]
      {
        "VOL",
        "99",
        "02",
        "97"
      };
      InputHandlerBase.docIdTbl[(object) "DD"] = (object) new string[4]
      {
        "VOD",
        "35",
        "02",
        "97"
      };
      InputHandlerBase.docIdTbl[(object) "FM"] = (object) new string[4]
      {
        "VOM",
        "43",
        "04",
        "97"
      };
      string[] strArray1 = new string[4]
      {
        "VOR",
        "99",
        "04",
        "97"
      };
      InputHandlerBase.docIdTbl[(object) "FR"] = (object) strArray1;
      InputHandlerBase.docIdTbl[(object) "BR"] = (object) strArray1;
      InputHandlerBase.docIdTbl[(object) "CR"] = (object) strArray1;
      string[] strArray2 = new string[4]
      {
        "VOE",
        "99",
        "02",
        "97"
      };
      InputHandlerBase.docIdTbl[(object) "FE"] = (object) strArray2;
      InputHandlerBase.docIdTbl[(object) "BE"] = (object) strArray2;
      InputHandlerBase.docIdTbl[(object) "CE"] = (object) strArray2;
      string[] strArray3 = new string[4]
      {
        "IRS4506T - Trans Request",
        "63",
        "02",
        ""
      };
      InputHandlerBase.docIdTbl[(object) "IR"] = (object) strArray3;
      string[] strArray4 = new string[4]
      {
        "IRS4506 - Copy Request",
        "63",
        "02",
        ""
      };
      InputHandlerBase.docIdTbl[(object) "AR"] = (object) strArray4;
    }

    internal InputHandlerBase(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form inputForm,
      object property)
    {
      this.session = session;
      this.init();
      try
      {
        this.CurrentFormName = (string) htmldoc.body.getAttribute("id");
      }
      catch (Exception ex)
      {
      }
      if (this.session.UserInfo.IsSuperAdministrator())
        this.isSuperAdmin = true;
      this.mainScreen = mainScreen;
      this.loan = this.session.LoanData;
      if (this.session.LoanDataMgr != null)
      {
        this.feeManagementSetting = this.session.LoanDataMgr.SystemConfiguration.FeeManagementList;
        this.feeManagementPermission = this.session.LoanDataMgr.SystemConfiguration.FeeManagementPersonaPermission;
      }
      else
      {
        this.feeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
        this.feeManagementPermission = this.loadFeeManagementPermission();
      }
      this.inputData = (IHtmlInput) this.loan;
      if (this.session.LoanDataMgr != null && this.session.LoanDataMgr.LinkedLoan != null)
        this.loan.LinkedData = this.session.LoanDataMgr.LinkedLoan.LoanData;
      this.initializeHandler(htmldoc, inputForm, property);
      if (this.session.LoanDataMgr != null)
      {
        this.session.LoanDataMgr.AccessRightsChanged += new EventHandler(this.onLoanAccessRightsChanged);
        this.session.LoanDataMgr.FieldRulesChanged += new EventHandler(this.onLoanFieldRulesChanged);
      }
      if (this.loan == null)
        return;
      this.loan.CountyLimitInvalidLoanAmount += new EventHandler(this.ExceededLoanAmount);
      if (this.loan.Calculator == null || this.loan.Calculator.OnUSDACalculationChangedHooked)
        return;
      this.loan.Calculator.OnUSDACalculationChanged += new EventHandler(this.Calculator_OnUSDACalculationChanged);
      this.loan.Calculator.OnUSDACalculationChangedHooked = true;
    }

    private void Calculator_OnUSDACalculationChanged(object sender, EventArgs e)
    {
      this.confirmUSDAInLine819(false);
    }

    private bool confirmUSDAInLine819(bool allowCancel)
    {
      if (this.inputData.GetField("1172") != "FarmersHomeAdministration" || this.inputData.GetField("337") == string.Empty || this.inputData.GetField("NEWHUD.X1299") == "Guarantee Fee")
        return true;
      string str = "For USDA loans, line 819 on the ";
      string text = (!Utils.CheckIf2015RespaTila(this.inputData.GetField("3969")) ? str + "2010 Itemization and 2010 HUD-1 Page 2" : str + "2015 Itemization") + " is now designated for the USDA Guarantee Fee. Existing data on line 819 has been overwritten with the USDA Guarantee Fee.";
      if (allowCancel)
        return Utils.Dialog((IWin32Window) Session.MainForm, text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK;
      int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return true;
    }

    private FeeManagementPersonaInfo loadFeeManagementPermission()
    {
      if (this.feeManagementPermission == null)
      {
        List<int> intList = new List<int>();
        for (int index = 0; index < this.session.UserInfo.UserPersonas.Length; ++index)
          intList.Add(this.session.UserInfo.UserPersonas[index].ID);
        this.feeManagementPermission = ((FieldAccessAclManager) this.session.ACL.GetAclManager(AclCategory.FieldAccess)).GetFeeManagementPermission(intList.ToArray());
      }
      return this.feeManagementPermission;
    }

    internal InputHandlerBase(
      Sessions.Session session,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form inputForm,
      object property)
    {
      this.session = session;
      if (this.session.LoanDataMgr != null)
      {
        this.feeManagementSetting = this.session.LoanDataMgr.SystemConfiguration.FeeManagementList;
        this.feeManagementPermission = this.session.LoanDataMgr.SystemConfiguration.FeeManagementPersonaPermission;
      }
      else
      {
        this.feeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
        this.feeManagementPermission = this.loadFeeManagementPermission();
      }
      this.init();
      if (this.session.UserInfo.IsSuperAdministrator())
        this.isSuperAdmin = true;
      this.isUsingTemplate = true;
      this.loan = dataTemplate;
      this.inputData = (IHtmlInput) dataTemplate;
      if (this.loan != null && this.loan.IsInFindFieldForm)
        this.FlushOutEnabled = false;
      this.initializeHandler(htmldoc, inputForm, property);
    }

    internal InputHandlerBase(
      Sessions.Session session,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form inputForm,
      object property)
    {
      this.session = session;
      if (this.session.LoanDataMgr != null)
      {
        this.feeManagementSetting = this.session.LoanDataMgr.SystemConfiguration.FeeManagementList;
        this.feeManagementPermission = this.session.LoanDataMgr.SystemConfiguration.FeeManagementPersonaPermission;
      }
      else
      {
        this.feeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
        this.feeManagementPermission = this.loadFeeManagementPermission();
      }
      this.init();
      if (this.session.UserInfo.IsSuperAdministrator())
        this.isSuperAdmin = true;
      this.isUsingTemplate = true;
      this.inputData = input;
      this.initializeHandler(htmldoc, inputForm, property);
    }

    private void init()
    {
      if (this.session.Application != null)
      {
        this.editor = this.session.Application.GetService<ILoanEditor>();
        this.efolder = this.session.Application.GetService<IEFolder>();
        this.epass = this.session.Application.GetService<IEPass>();
      }
      this.notAllowedPricingChangeSetting = this.session.StartupInfo.PolicySettings.Contains((object) "Policies.NotAllowPricingChange") && (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.NotAllowPricingChange"];
      this.Use5DecimalForIndexRates = "FiveDecimals" == Session.SessionObjects.GetCompanySettingFromCache("Policies", "ARMIndexPrecision");
    }

    public void ExceededLoanAmount(object sender, EventArgs e) => this.SetGoToFieldFocus("1109", 0);

    public virtual object Property
    {
      get => (object) 0;
      set
      {
      }
    }

    public virtual bool IsDeleted
    {
      get => this.isDeleted;
      set => this.isDeleted = value;
    }

    public object GetAutomationFormObject() => (object) this.currentForm;

    public string ItemizationLineNumber => this.itemizationLineNumber;

    internal virtual void CreateControls()
    {
      try
      {
        using (Tracing.StartTimer(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Verbose, "Invoking Form.CreateControls()"))
          this.currentForm.CreateControls();
        if (this.loan == null || this.loan.Calculator == null)
          return;
        foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
        {
          if ("|1724|1725|78|81|109|118|799|1206|1391|1961|3034|3054|3119|3120|4088|NEWHUD.X11|NEWHUD.X217|RE88395.X290|RE88395.X291|RE88395.X294|RE88395.X296|RE88395.X323|RE88395.X324|RE88395.X325|RE88395.X327|LE1.X92".IndexOf("|" + allFieldControl.Field.FieldID + "|") > -1)
          {
            this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
            break;
          }
        }
      }
      catch (Exception ex)
      {
        throw new AutomationException("CreateControls method threw an exception", ex);
      }
    }

    public static bool InputFormAutoFocus(Sessions.Session session)
    {
      if (InputHandlerBase.inputFormAutoFocus < 0)
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
        {
          if (registryKey != null)
          {
            string str = (string) registryKey.GetValue(nameof (InputFormAutoFocus));
            if (str != null)
            {
              if (str.Trim() == "1")
                InputHandlerBase.inputFormAutoFocus = 1;
              else if (str.Trim() == "0")
                InputHandlerBase.inputFormAutoFocus = 0;
            }
          }
        }
        if (InputHandlerBase.inputFormAutoFocus < 0)
          InputHandlerBase.inputFormAutoFocus = (bool) session.StartupInfo.UnpublishedSettings[(object) "Unpublished.InputFormAutoFocus"] ? 1 : 0;
      }
      return InputHandlerBase.inputFormAutoFocus == 1;
    }

    private void initializeHandler(HTMLDocument htmldoc, EllieMae.Encompass.Forms.Form inputForm, object property)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("InputHandlerBase.initializeHandler", 514, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs"))
      {
        try
        {
          this.CurrentFormName = (string) htmldoc.body.getAttribute("id");
        }
        catch (Exception ex)
        {
        }
        if (this.loan != null && !this.FormIsForTemplate && this.session.LoanDataMgr != null)
        {
          this.loCompensationSetting = this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting;
          this.loan.BeforeFieldChanged += new LoanDataBeforeChangedEventHandler(this.loan_BeforeFieldChanged);
        }
        else
          this.loCompensationSetting = new LOCompensationSetting(this.session.ConfigurationManager.GetCompanySetting("LOCompensation", "Rule"));
        this.PiggyFields = this.session.LoanDataMgr != null ? this.session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields : (PiggybackFields) this.session.GetSystemSettings(typeof (PiggybackFields));
        if (property != null && property is string)
        {
          this.itemizationLineNumber = (string) property;
          property = (object) null;
        }
        this.htmldoc = htmldoc;
        this.currentForm = inputForm;
        this.Property = property;
        this.attachFormToDocument();
        performanceMeter.AddCheckpoint("Attached Form object to HTML document", 556, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        this.refreshToolTips(false);
        performanceMeter.AddCheckpoint("Refreshed tooltips", 560, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        this.autoAssignTextStyles();
        performanceMeter.AddCheckpoint("Assigned text styles to fields", 564, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        this.ApplyFieldAccessRights(true);
        this.applyFieldRules();
        this.applyEncompassVersion();
        performanceMeter.AddCheckpoint("Applied field access and field rules", 570, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        this.attachFormEventHandlers();
        performanceMeter.AddCheckpoint("Attached form event handlers", 574, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        if (this.session != null && InputHandlerBase.InputFormAutoFocus(this.session))
          this.RefreshContents(true);
        else
          this.RefreshContents(false);
        performanceMeter.AddCheckpoint("Populated form data into controls", 582, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        if (this.session != null && InputHandlerBase.InputFormAutoFocus(this.session))
        {
          this.setFocusToFirstField();
          performanceMeter.AddCheckpoint("Set focus to first form field in tab order", 588, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        }
        this.executeLoadEvent();
        performanceMeter.AddCheckpoint("Invoked Form's custom Load event", 593, nameof (initializeHandler), "D:\\ws\\24.3.0.0\\EmLite\\Input\\InputEngine\\InputHandlerBase.cs");
        if (this.loan == null)
          return;
        this.ulddExportChecking(false, false, this.loan.ULDDExportType);
      }
    }

    private void specialFeatureCodeLookup(string type)
    {
      int num = (int) new SpecialFeatureCodeDialog(this.loan, type).ShowDialog((IWin32Window) Session.MainForm);
    }

    private void ulddExportChecking(bool forceDisplay, bool displayMessage, string source)
    {
      if (this.loan == null || !forceDisplay && !this.loan.IsULDDExporting || this.loan.Calculator == null)
        return;
      List<string> stringList = this.loan.Calculator.ULDDExportChecking(source);
      if (stringList == null)
      {
        Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Info, "ULDDExportChecking cannot get any invalid field array.");
      }
      else
      {
        if (displayMessage)
        {
          if (stringList.Count == 0)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, "The preliminary edit check has been performed and the export data is ready for export. Final loan validation will take place once submitted to the GSE site.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            string msg = "Warning: ULDD export for " + source + ": ";
            foreach (string str in stringList.ToArray())
              msg = msg + str + ", ";
            Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Warning, msg);
            int num2 = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, "There is incomplete or invalid data in the loan file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
        {
          if (stringList.Contains(allFieldControl.Field.FieldID))
          {
            this.setControlBackColor(allFieldControl, (System.Drawing.Color) new ColorConverter().ConvertFromString("#FCDCDF"));
            if (allFieldControl.Field.FieldID == "ULDD.X52" || allFieldControl.Field.FieldID == "1039" || allFieldControl.Field.FieldID == "ULDD.FNM.ARMIndexType")
              this.setHighlightContainerColor(allFieldControl, (System.Drawing.Color) new ColorConverter().ConvertFromString("#FCDCDF"));
          }
          else
          {
            this.setControlBackColor(allFieldControl, System.Drawing.Color.White);
            if (allFieldControl.Field.FieldID == "ULDD.X52" || allFieldControl.Field.FieldID == "1039" || allFieldControl.Field.FieldID == "ULDD.FNM.ARMIndexType")
              this.setHighlightContainerColor(allFieldControl, (System.Drawing.Color) new ColorConverter().ConvertFromString("#FCFCFC"));
          }
        }
      }
    }

    private void setHighlightContainerColor(FieldControl ctrl, System.Drawing.Color containerColor)
    {
      EllieMae.Encompass.Forms.ContainerControl container = ctrl.GetContainer();
      if (!container.ControlID.ToLower().Contains("highlight"))
        return;
      container.BackColor = containerColor;
    }

    private bool loan_BeforeFieldChanged(string id, string newVal, string orgVal)
    {
      return !(id == "14") || this.StatePermission(newVal, false);
    }

    private void attachFormToDocument()
    {
      this.currentForm.AttachToFormScreen((IFormScreen) this, (string) null);
      this.currentForm.AttachToDocument(this.htmldoc, FormOptions.None);
      this.CreateControls();
      int length = this.currentForm.GetAllFieldControls().Length;
      Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Info, "Form '" + this.currentForm.Name + "' contains " + (object) length + " field elements");
    }

    private void attachFormEventHandlers()
    {
      this.attachHTMLEventHandlersToFragment(this.currentForm.GetHTMLDocument().body);
      this.currentForm.ControlAdded += new FormEventHandler(this.onNewControlAddedToForm);
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
        allFieldControl.ValueChanged += new EventHandler(this.onFieldValueChanged);
    }

    private void attachHTMLEventHandlersToFragment(mshtml.IHTMLElement rootElement)
    {
      this.attachHTMLEventHandlerToElement(rootElement);
      string[] strArray = new string[7]
      {
        "INPUT",
        "TEXTAREA",
        "BUTTON",
        "SELECT",
        "OBJECT",
        "A",
        "IMG"
      };
      foreach (string v in strArray)
      {
        foreach (mshtml.IHTMLElement elm in ((mshtml.IHTMLElement2) rootElement).getElementsByTagName(v))
          this.attachHTMLEventHandlerToElement(elm);
      }
    }

    private void attachHTMLEventHandlerToElement(mshtml.IHTMLElement elm)
    {
      string upper = elm.tagName.ToUpper();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(upper))
      {
        case 341872817:
          if (!(upper == "BUTTON"))
            break;
          this.hookupEventHandlers((object) elm, InputHandlerBase.guidButton);
          break;
        case 456003450:
          if (!(upper == "OBJECT"))
            break;
          this.hookupEventHandlers(((IHTMLObjectElement) elm).@object, InputHandlerBase.guidJedCombo);
          break;
        case 1168905659:
          if (!(upper == "TEXTAREA"))
            break;
          this.hookupEventHandlers((object) elm, InputHandlerBase.guidInput);
          break;
        case 1638025307:
          if (!(upper == "INPUT"))
            break;
          this.hookupEventHandlers((object) elm, InputHandlerBase.guidInput);
          break;
        case 3022600877:
          if (!(upper == "SELECT"))
            break;
          this.hookupEventHandlers((object) elm, InputHandlerBase.guidSelect);
          break;
        case 3289118412:
          if (!(upper == "A"))
            break;
          this.hookupEventHandlers((object) elm, InputHandlerBase.guidAnchor);
          break;
        case 3351329828:
          if (!(upper == "IMG"))
            break;
          this.hookupEventHandlers((object) elm, InputHandlerBase.guidImage);
          break;
      }
    }

    private void refreshHTMLEventHandlers()
    {
      this.releaseHTMLEventHandlers();
      this.attachHTMLEventHandlersToFragment(this.currentForm.GetHTMLDocument().body);
    }

    private void releaseHTMLEventHandlers()
    {
      if (this.cons == null)
        return;
      for (int index = 0; index < this.cons.Count; ++index)
      {
        InputHandlerBase.ConInfo con = (InputHandlerBase.ConInfo) this.cons[index];
        try
        {
          con.ConPt.Unadvise(con.Cookie);
          if (this.forceComRelease)
            Marshal.ReleaseComObject((object) con.ConPt);
        }
        catch
        {
        }
      }
      this.cons.Clear();
    }

    private void onNewControlAddedToForm(object source, FormEventArgs e)
    {
      this.refreshHTMLEventHandlers();
      if (!(e.Control is FieldControl))
        return;
      ((FieldControl) e.Control).ValueChanged += new EventHandler(this.onFieldValueChanged);
    }

    private void onFieldValueChanged(object sender, EventArgs e)
    {
      this.ProcessChangeEvent(sender as FieldControl);
    }

    private void onLoanAccessRightsChanged(object sender, EventArgs e)
    {
      this.ApplyFieldAccessRights(false);
    }

    private void onLoanFieldRulesChanged(object sender, EventArgs e) => this.applyFieldRules();

    private void autoAssignTextStyles()
    {
      foreach (EllieMae.Encompass.Forms.TextBox textBox in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.TextBox)))
      {
        if (textBox.Alignment == TextAlignment.Auto)
        {
          string id = this.MapFieldId(textBox.Field.FieldID);
          if (id != "")
          {
            FieldFormat fieldFormat = this.GetFieldFormat(id);
            textBox.Alignment = !FieldFormatEnumUtil.IsNumeric(fieldFormat) ? (fieldFormat == FieldFormat.RA_STRING || fieldFormat == FieldFormat.RA_INTEGER || fieldFormat == FieldFormat.RA_DECIMAL_2 || fieldFormat == FieldFormat.RA_DECIMAL_3 ? TextAlignment.Right : TextAlignment.Left) : TextAlignment.Right;
          }
        }
      }
    }

    public void RefreshToolTips() => this.refreshToolTips(true);

    private void refreshToolTips(bool forceHelpKeyTips)
    {
      bool flag = this.session.GetPrivateProfileString("HELP", "FieldHelp") != "OFF";
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
      {
        try
        {
          if (!(allFieldControl is DropdownBox))
          {
            if (allFieldControl.HoverText == "" | forceHelpKeyTips)
            {
              string str = this.MapFieldId(allFieldControl.Field.FieldID);
              string helpKey = this.MapHelpKey(allFieldControl.HelpKey);
              string text = helpKey != "" ? FieldHelp.GetText(helpKey) : "";
              allFieldControl.HoverText = !flag || !(text != "") ? str : str + ": " + text;
            }
          }
          else if (allFieldControl.Field.FieldID == "2626")
          {
            int[] channelOptions = this.session.LoanDataMgr.SystemConfiguration.ChannelOptions;
            if (channelOptions != null)
              ((DropdownBox) allFieldControl).KeepOptions(channelOptions);
          }
        }
        catch
        {
        }
      }
      foreach (ContactButton contactButton in this.currentForm.FindControlsByType(typeof (ContactButton)))
      {
        if (contactButton.HoverText == "")
          contactButton.HoverText = "Open the Conversation Log" + (contactButton.ContactMethod == ContactMethod.Email ? " and send an email" : "");
      }
    }

    private bool execFormAction(string action)
    {
      if (action == "" || this.loan != null && this.loan.IsInFindFieldForm && this.loan.ButtonSelectionEnabled)
        return true;
      if (this.loan != null && !this.FormIsForTemplate && this.poppingUpPrerequiredDialog((MissingPrerequisiteException) null, "BUTTON_" + action.ToUpper()))
        return false;
      this.currentForm.ExecAction(action);
      return true;
    }

    private void hookupEventHandlers(object o, Guid interfaceGuid)
    {
      if (o == null)
        return;
      System.Runtime.InteropServices.ComTypes.IConnectionPoint ppCP = (System.Runtime.InteropServices.ComTypes.IConnectionPoint) null;
      ((System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) o).FindConnectionPoint(ref interfaceGuid, out ppCP);
      int pdwCookie;
      ppCP.Advise((object) this, out pdwCookie);
      this.cons.Add((object) new InputHandlerBase.ConInfo(ppCP, pdwCookie));
    }

    protected virtual string MapFieldId(string fieldId) => fieldId;

    protected virtual string MapHelpKey(string helpKey) => helpKey;

    public virtual bool AllowUnload() => true;

    public virtual void Unload()
    {
      if (Tracing.IsSwitchActive(InputHandlerBase.sw, TraceLevel.Verbose))
        Tracing.Log(InputHandlerBase.sw, TraceLevel.Verbose, nameof (InputHandlerBase), "UnregisterElementHandlers");
      if (this.cons != null)
      {
        this.FlushOutCurrentField();
        this.executeUnloadEvent();
        this.releaseHTMLEventHandlers();
        this.cons = (ArrayList) null;
      }
      if (this.session.LoanDataMgr != null)
      {
        this.session.LoanDataMgr.AccessRightsChanged -= new EventHandler(this.onLoanAccessRightsChanged);
        this.session.LoanDataMgr.FieldRulesChanged -= new EventHandler(this.onLoanFieldRulesChanged);
      }
      if (this.loan != null)
      {
        this.loan.CountyLimitInvalidLoanAmount -= new EventHandler(this.ExceededLoanAmount);
        this.loan.BeforeFieldChanged -= new LoanDataBeforeChangedEventHandler(this.loan_BeforeFieldChanged);
        if (this.loan.Calculator != null)
        {
          this.loan.Calculator.OnUSDACalculationChanged -= new EventHandler(this.Calculator_OnUSDACalculationChanged);
          this.loan.Calculator.OnUSDACalculationChangedHooked = false;
        }
      }
      this.clearStatusBarInformation();
    }

    protected string CurrentValue
    {
      get => this.currentField == null ? "" : this.currentField.Value;
      set
      {
        if (this.currentField == null)
          throw new InvalidOperationException("The current control is not a Field Control");
        this.currentField.BindTo(value);
      }
    }

    protected string CurrentFieldID
    {
      get => this.currentField == null ? "" : this.MapFieldId(this.currentField.Field.FieldID);
    }

    public virtual void UpdateCurrentField() => this.FlushOutCurrentField();

    protected virtual void FlushOutCurrentField()
    {
      if (!this.FlushOutEnabled || !this.forceChangeEvent)
        return;
      this.ProcessChangeEvent(this.currentField);
    }

    private bool syncControlToDataSource(FieldControl ctrl)
    {
      try
      {
        if (ctrl.AccessMode != FieldAccessMode.NoRestrictions)
          return false;
        string id = this.MapFieldId(this.parseFieldID(ctrl.Field.FieldID));
        string val = ctrl.Value;
        if (ctrl is DropdownEditBox && val.Trim() == "")
          val = "";
        if (!this.executeDataCommitEvent(ctrl, ref val))
          return false;
        if (ctrl.FieldSource == FieldSource.Other)
          return true;
        string field = this.inputData.GetField(id);
        if (ctrl.FieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData != null)
          field = this.loan.LinkedData.GetField(id);
        if (!(val != field))
          return false;
        this.WriteDebug("Syncing field '" + id + "':  Prior = '" + field + "', New = '" + val + "'");
        return this.updateFieldValue(id, ctrl.FieldSource, val);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.populateFieldFromDataSource(ctrl);
        return false;
      }
    }

    private bool isNumeric(LoanFieldFormat format)
    {
      return FieldFormatEnumUtil.IsNumeric((FieldFormat) format);
    }

    private int compareNumeric(string val1, string val2)
    {
      try
      {
        return Utils.ParseDecimal((object) val1).CompareTo(Utils.ParseDecimal((object) val2));
      }
      catch
      {
        return 0;
      }
    }

    public void SaveCurrentField() => this.FlushOutCurrentField();

    protected virtual bool HasExclusiveRights(bool interactive)
    {
      using (CursorActivator.Wait())
      {
        try
        {
          if (!this.session.LoanDataMgr.LockLoanWithExclusiveA(interactive))
            return false;
        }
        catch (Exception ex)
        {
          return false;
        }
      }
      return true;
    }

    internal virtual ControlState GetControlState(RuntimeControl ctrl, string fieldIdOrAction)
    {
      return ctrl is FieldControl && ((FieldControl) ctrl).FieldSource == FieldSource.LinkedLoan && (this.loan == null || this.loan.LinkedData == null) || this.inputData is DisclosedCDHandler || this.inputData is DisclosedLEHandler ? ControlState.Disabled : this.getControlState(ctrl, fieldIdOrAction, ControlState.Default);
    }

    protected ControlState getControlState(
      RuntimeControl ctrl,
      string fieldIdOrAction,
      ControlState defaultValue)
    {
      string lower = fieldIdOrAction.ToLower();
      switch (lower)
      {
        case "101":
        case "102":
        case "103":
        case "104":
        case "107":
        case "108":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
          return this.loan.GetField("1825") == "2020" ? ControlState.Disabled : ControlState.Enabled;
        case "1034":
          if (this.loan.GetField("1066") != "Leasehold")
          {
            this.SetControlState("f1034Date", false);
            return ControlState.Disabled;
          }
          this.SetControlState("f1034Date", true);
          return ControlState.Enabled;
        case "1063":
          return this.inputData.GetField("1172") == "Other" ? ControlState.Enabled : ControlState.Disabled;
        case "11":
        case "319":
        case "fe0104":
        case "fe0204":
        case "fr0104":
        case "fr0204":
        case "fr0304":
        case "fr0404":
          return this.loan.GetField("1825") == "2020" ? ControlState.Disabled : ControlState.Default;
        case "1107":
          if (this.loan.GetField("3533") == "Y")
            return ControlState.Disabled;
          goto case null;
        case "1134":
          if (this.GetField("19") == "Purchase")
            return ControlState.Disabled;
          goto case null;
        case "1405":
          if (this.GetField("USEGFETAX") == "Y")
            return ControlState.Disabled;
          goto case null;
        case "1416":
          return this.loan.GetField("1819") == "Y" || this.loan.GetField("1825") == "2020" ? ControlState.Disabled : ControlState.Enabled;
        case "1519":
          return this.loan.GetField("1820") == "Y" || this.loan.GetField("1825") == "2020" ? ControlState.Disabled : ControlState.Enabled;
        case "1524":
        case "1525":
        case "1526":
        case "1527":
        case "1528":
        case "1529":
        case "4148":
        case "4149":
        case "4150":
        case "4151":
        case "4152":
        case "4153":
        case "4154":
        case "4155":
        case "4156":
        case "4157":
        case "4158":
          if (this.GetField("1524") == "Y" || this.GetField("1525") == "Y" || this.GetField("1526") == "Y" || this.GetField("1527") == "Y" || this.GetField("1528") == "Y" || this.GetField("1529") == "Y" || this.GetField("4148") == "Y" || this.GetField("4149") == "Y" || this.GetField("4150") == "Y" || this.GetField("4151") == "Y" || this.GetField("4152") == "Y" || this.GetField("4153") == "Y" || this.GetField("4154") == "Y" || this.GetField("4155") == "Y" || this.GetField("4156") == "Y" || this.GetField("4157") == "Y" || this.GetField("4158") == "Y")
          {
            this.SetControlState("chkBorrRaceNA", false);
            goto case null;
          }
          else
          {
            this.SetControlState("chkBorrRaceNA", true);
            goto case null;
          }
        case "1532":
        case "1533":
        case "1534":
        case "1535":
        case "1536":
        case "1537":
        case "4163":
        case "4164":
        case "4165":
        case "4166":
        case "4167":
        case "4168":
        case "4169":
        case "4170":
        case "4171":
        case "4172":
        case "4173":
          if (this.GetField("1532") == "Y" || this.GetField("1533") == "Y" || this.GetField("1534") == "Y" || this.GetField("1535") == "Y" || this.GetField("1536") == "Y" || this.GetField("1537") == "Y" || this.GetField("4163") == "Y" || this.GetField("4164") == "Y" || this.GetField("4165") == "Y" || this.GetField("4166") == "Y" || this.GetField("4167") == "Y" || this.GetField("4168") == "Y" || this.GetField("4169") == "Y" || this.GetField("4170") == "Y" || this.GetField("4171") == "Y" || this.GetField("4172") == "Y" || this.GetField("4173") == "Y")
          {
            this.SetControlState("chkCoBorrRaceNA", false);
            goto case null;
          }
          else
          {
            this.SetControlState("chkCoBorrRaceNA", true);
            goto case null;
          }
        case "3162":
          this.FormatAlphaNumericField(ctrl, fieldIdOrAction);
          return defaultValue;
        case "3164":
          if (this.loan.GetField("3149") == "" || this.loan.GetField("3149") == "//" || this.loan.GetField("3153") == "" || this.loan.GetField("3153") == "//")
            return ControlState.Disabled;
          goto case null;
        case "3197":
          if (this.loan.GetField("3164") != "Y")
            return ControlState.Disabled;
          goto case null;
        case "3612":
          return this.loan.GetField("4666") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        case "3613":
          return this.loan.GetField("4666") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
            return ControlState.Disabled;
          goto case null;
        case "3894":
          if (!this.session.UserInfo.IsAdministrator() && (!this.fieldRights.ContainsKey((object) "3894") || this.session.LoanDataMgr.GetFieldAccessRights("3894") != BizRule.FieldAccessRight.Edit))
            return ControlState.Disabled;
          goto case null;
        case "4144":
        case "4145":
        case "4146":
        case "4147":
        case "4205":
        case "4210":
        case "4211":
          if (this.GetField("4205") == "Y" || this.GetField("4210") == "Y" || this.GetField("4211") == "Y" || this.GetField("4144") == "Y" || this.GetField("4145") == "Y" || this.GetField("4146") == "Y" || this.GetField("4147") == "Y")
          {
            this.SetControlState("chkEthnicityBorrNA", false);
            goto case null;
          }
          else
          {
            this.SetControlState("chkEthnicityBorrNA", true);
            goto case null;
          }
        case "4159":
        case "4160":
        case "4161":
        case "4162":
        case "4206":
        case "4213":
        case "4214":
          if (this.GetField("4206") == "Y" || this.GetField("4213") == "Y" || this.GetField("4214") == "Y" || this.GetField("4159") == "Y" || this.GetField("4160") == "Y" || this.GetField("4161") == "Y" || this.GetField("4162") == "Y")
          {
            this.SetControlState("chkEthnicityCoBorrNA", false);
            goto case null;
          }
          else
          {
            this.SetControlState("chkEthnicityCoBorrNA", true);
            goto case null;
          }
        case "432":
        case "761":
        case "762":
          if (!(this.GetField("3941") == "Y"))
            return ControlState.Enabled;
          if (fieldIdOrAction == "761")
          {
            EllieMae.Encompass.Forms.Calendar control = (EllieMae.Encompass.Forms.Calendar) this.currentForm.FindControl("Calendar4");
            if (control != null)
              control.Enabled = false;
          }
          return ControlState.Disabled;
        case "4970":
        case "4971":
        case "4972":
        case "MORNET.X30":
          return this.GetField("5027") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "5018":
        case "5019":
        case "5020":
        case "5021":
          return this.GetField("5028") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "677":
          if (this.currentForm.ToString() == "STATE_SPECIFIC_NY" && this.currentForm.FindControl("dd_Assumption") is FieldControl control1)
          {
            DropdownBox dropdownBox = (DropdownBox) control1;
            if (Utils.CheckIf2015RespaTila(this.inputData.GetField("3969")))
            {
              if (dropdownBox.Options.Contains(new DropdownOption("may", "May")))
                dropdownBox.Options.Remove(new DropdownOption("may", "May"));
            }
            else if (!dropdownBox.Options.Contains(new DropdownOption("may", "May")))
              dropdownBox.Options.Insert(1, new DropdownOption("may", "May"));
            control1.BindTo(this.inputData.GetField("677"));
            goto case null;
          }
          else
            goto case null;
        case "aff.x32":
          return string.IsNullOrEmpty(this.loan.GetField("AFF.X3")) ? ControlState.Disabled : ControlState.Enabled;
        case "exportulddfannie":
        case "exportulddfreddie":
          if (this.loan.IsULDDExporting)
            return ControlState.Disabled;
          goto case null;
        case "getami":
          return this.FormIsForTemplate || this.GetField("5027") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        case "getmfi":
          return this.FormIsForTemplate || this.GetField("5028") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        case "importfundingdetails":
          if (!this.session.LoanDataMgr.Writable)
            return ControlState.Disabled;
          goto case null;
        case "lookup4970m":
          return this.GetField("5027") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        case "mfilookup":
          return this.GetField("5028") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        case "mornet.x41":
          defaultValue = this.GetSpecialControlStatus(fieldIdOrAction, defaultValue);
          goto case null;
        case "openitemization":
          if (this.FormIsForTemplate)
            return ControlState.Disabled;
          goto case null;
        case "paymentexample":
          return this.inputData.GetField("608") == "AdjustableRate" ? ControlState.Enabled : ControlState.Disabled;
        case "updatelinkedborrower":
          bool flag1;
          if (this.isBorrowerContactLinked.HasValue)
          {
            flag1 = this.isBorrowerContactLinked.Value;
          }
          else
          {
            flag1 = this.isBorrowerLinked(false);
            this.isBorrowerContactLinked = new bool?(flag1);
          }
          return flag1 ? ControlState.Enabled : ControlState.Disabled;
        case "updatelinkedcoborrower":
          bool flag2;
          if (this.isCoBorrowerContactLinked.HasValue)
          {
            flag2 = this.isCoBorrowerContactLinked.Value;
          }
          else
          {
            flag2 = this.isBorrowerLinked(true);
            this.isCoBorrowerContactLinked = new bool?(flag2);
          }
          return flag2 ? ControlState.Enabled : ControlState.Disabled;
        case null:
          return this.loan != null && ctrl is EllieMae.Encompass.Forms.Button && this.loan.IsInFindFieldForm ? ControlState.Disabled : defaultValue;
        default:
          string str = lower;
          if (str.StartsWith("attachliens"))
          {
            if (str.Length > 11)
            {
              string s = str.Substring(11);
              int result = 0;
              if (int.TryParse(s, out result))
              {
                string id = "FM" + s + "43";
                return this.FormIsForTemplate || string.IsNullOrEmpty(this.loan.GetField(id)) ? ControlState.Disabled : ControlState.Enabled;
              }
            }
            return defaultValue;
          }
          goto case null;
      }
    }

    public ControlState GetSpecialControlStatus(string id, ControlState defaultValue)
    {
      switch (id)
      {
        case "MORNET.X41":
          if (this.GetField("19") != "NoCash-Out Refinance" && this.GetField("19") != "Cash-Out Refinance")
            return ControlState.Disabled;
          break;
        case "140":
          EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_140");
          EllieMae.Encompass.Forms.Panel control2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel_140");
          if (control2 != null)
            control2.Top = control1.Top;
          EllieMae.Encompass.Forms.TextBox textBox = (EllieMae.Encompass.Forms.TextBox) null;
          EllieMae.Encompass.Forms.Panel panel = (EllieMae.Encompass.Forms.Panel) null;
          if (this is PIGGYBACKInputHandler)
          {
            textBox = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_140_link");
            panel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel_140_linked");
            if (panel != null)
              panel.Top = textBox.Top;
          }
          if (control2 != null && control1 != null)
          {
            if (this.inputData.GetField("1825") == "2020" || this.loan != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked) && this.loan.LinkedData != null)
            {
              if (control2 != null)
                control2.Visible = true;
              if (control1 != null)
                control1.Visible = false;
            }
            else
            {
              if (control2 != null)
                control2.Visible = false;
              if (control1 != null)
                control1.Visible = true;
            }
            if (textBox != null && panel != null)
            {
              textBox.Visible = control1.Visible;
              panel.Visible = control2.Visible;
            }
          }
          return ControlState.Default;
      }
      return defaultValue;
    }

    public void RefreshControl(string controlId)
    {
      EllieMae.Encompass.Forms.Control control = this.currentForm.FindControl(controlId);
      if (control == null)
        return;
      this.refreshControl(control, true, false);
    }

    public void RefreshAllControls(bool force) => this.UpdateContents(force);

    protected virtual void UpdateContents() => this.UpdateContents(false);

    protected virtual void UpdateContents(bool refreshAllFields)
    {
      this.UpdateContents(refreshAllFields, false, false);
    }

    protected virtual void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      Hashtable hashtable = new Hashtable();
      EllieMae.Encompass.Forms.Control ctrl1 = (EllieMae.Encompass.Forms.Control) null;
      EllieMae.Encompass.Forms.Control ctrl2 = (EllieMae.Encompass.Forms.Control) null;
      if (this.currentForm != null)
      {
        if (this is LOLOCKREQUESTInputHandler)
          ((LOLOCKREQUESTInputHandler) this).SetDisablePricingChange();
        foreach (EllieMae.Encompass.Forms.Control allControl in this.currentForm.GetAllControls())
        {
          try
          {
            if (allControl.ControlID == "l_X1301")
              ctrl1 = allControl;
            if (allControl.ControlID == "FieldLock819")
              ctrl2 = allControl;
            string key = this.refreshControl(allControl, refreshAllFields, fireChangeEvents);
            if (key != "")
              hashtable[(object) key] = (object) true;
          }
          catch (Exception ex)
          {
            Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Error, "Error updating contents of control '" + allControl.ControlID + "': " + (object) ex);
          }
        }
      }
      if (ctrl1 != null && ctrl2 != null)
      {
        this.refreshControl(ctrl2, refreshAllFields, fireChangeEvents);
        this.refreshControl(ctrl1, refreshAllFields, fireChangeEvents);
      }
      if (!skipButtonFieldLockRules && this.currentForm != null)
      {
        this.applyButtonAccessRights();
        this.applyFieldLockAccessRights();
      }
      if (this.inputData != null)
      {
        foreach (string key in (System.Collections.IEnumerable) hashtable.Keys)
          this.inputData.CleanField(key);
      }
      if (this.loan == null || this.loan.LinkedData == null)
        return;
      foreach (string key in (System.Collections.IEnumerable) hashtable.Keys)
        this.loan.LinkedData.CleanField(key);
    }

    private string refreshControl(EllieMae.Encompass.Forms.Control ctrl, bool force, bool fireEventOnChange)
    {
      if (ctrl is FieldControl)
      {
        FieldControl ctrl1 = (FieldControl) ctrl;
        string id = this.refreshFieldControlContents(ctrl1, force, fireEventOnChange);
        if (this.loan != null && this.loan.IsInFindFieldForm && ctrl1.SupportsBackColor)
        {
          switch (this.loan.SelectedFieldType(id))
          {
            case LoanData.FindFieldTypes.NewSelect:
              ctrl1.BackColor = InputHandlerBase.SelectedFieldColor;
              break;
            case LoanData.FindFieldTypes.Existing:
              ctrl1.BackColor = InputHandlerBase.ExistingFieldColor;
              break;
            default:
              ctrl1.BackColor = System.Drawing.Color.White;
              break;
          }
        }
        else if (this.loan != null && !this.FormIsForTemplate && ctrl1.SupportsBackColor)
        {
          if (this.session.LoanDataMgr.FieldIsRequired(id))
            ctrl1.BackColor = InputHandlerBase.ExistingFieldColor;
          else if (ctrl1.BackColor == InputHandlerBase.ExistingFieldColor)
            ctrl1.BackColor = System.Drawing.Color.White;
        }
        return id;
      }
      if (ctrl is FieldLock)
      {
        if (this.loan != null && this.loan.IsInFindFieldForm)
        {
          string fieldLockId = this.findFieldLockID((FieldLock) ctrl);
          if (fieldLockId != string.Empty)
          {
            FieldLock fieldLock = (FieldLock) ctrl;
            switch (this.loan.SelectedFieldType("LOCKBUTTON_" + fieldLockId))
            {
              case LoanData.FindFieldTypes.NewSelect:
              case LoanData.FindFieldTypes.Existing:
                fieldLock.DisplayImage(true);
                break;
              default:
                fieldLock.DisplayImage(false);
                break;
            }
          }
        }
        else
        {
          FieldLock fieldLock = (FieldLock) ctrl;
          if (this.loan != null && this.loan.LinkedData == null)
          {
            if (fieldLock.ControlToLock != null)
            {
              if (((FieldControl) fieldLock.ControlToLock).FieldSource == FieldSource.LinkedLoan)
              {
                fieldLock.Enabled = false;
                fieldLock.DisplayImage(false);
              }
              else
                this.syncFieldLockState(fieldLock);
            }
          }
          else
            this.syncFieldLockState(fieldLock);
        }
      }
      else if (ctrl is EllieMae.Encompass.Forms.Calendar)
        this.syncCalendarLockState(ctrl as EllieMae.Encompass.Forms.Calendar);
      else if (ctrl is IActionable)
        this.setEnabledDisabledState(ctrl as RuntimeControl, ((IActionable) ctrl).Action);
      else if (ctrl is EllieMae.Encompass.Forms.BorrowerLink)
      {
        EllieMae.Encompass.Forms.BorrowerLink borrowerLink = (EllieMae.Encompass.Forms.BorrowerLink) ctrl;
        if (borrowerLink.BorrowerType == EllieMae.Encompass.Forms.BorrowerType.Borrower)
        {
          if (!this.isBorrowerContactLinked.HasValue)
            this.isBorrowerContactLinked = new bool?(this.isBorrowerLinked(false));
          borrowerLink.DisplayImage(this.isBorrowerContactLinked.Value);
        }
        else
        {
          if (!this.isCoBorrowerContactLinked.HasValue)
            this.isCoBorrowerContactLinked = new bool?(this.isBorrowerLinked(true));
          borrowerLink.DisplayImage(this.isCoBorrowerContactLinked.Value);
        }
      }
      if ((ctrl is FieldLock || ctrl is EllieMae.Encompass.Forms.Calendar || ctrl is IActionable || ctrl is EllieMae.Encompass.Forms.BorrowerLink || ctrl is Rolodex) && (this.inputData is DisclosedCDHandler || this.inputData is DisclosedLEHandler || this.inputData is DisclosedItemizationHandler && this is REGZGFE_2015_DETAILSInputHandler))
        (ctrl as RuntimeControl).Enabled = false;
      return "";
    }

    private bool isBorrowerLinked(bool isCoBorrower)
    {
      string contactGuid = "";
      string field;
      if (!isCoBorrower)
      {
        field = this.loan.GetField("65");
        CRMLog crmMapping = this.loan.GetLogList().GetCRMMapping(this.loan.CurrentBorrowerPair.Borrower.Id);
        if (crmMapping != null)
          contactGuid = crmMapping.ContactGuid;
      }
      else
      {
        field = this.loan.GetField("97");
        CRMLog crmMapping = this.loan.GetLogList().GetCRMMapping(this.loan.CurrentBorrowerPair.CoBorrower.Id);
        if (crmMapping != null)
          contactGuid = crmMapping.ContactGuid;
      }
      if (contactGuid == "")
        return false;
      BorrowerInfo borrower = this.session.ContactManager.GetBorrower(contactGuid);
      return borrower != null && !(borrower.SSN.Trim() != field.Trim());
    }

    private string refreshFieldControlContents(
      FieldControl ctrl,
      bool forceRefresh,
      bool fireEventOnChange)
    {
      string fieldId = this.parseFieldID(ctrl.Field.FieldID);
      if (!(fieldId != ""))
        return fieldId;
      string str = this.MapFieldId(fieldId);
      if (this.inputData != null && (forceRefresh || this.inputData.IsDirty(str)))
        this.populateFieldFromDataSource(ctrl, fireEventOnChange);
      this.setEnabledDisabledState((RuntimeControl) ctrl, str);
      return str;
    }

    private bool isControlFieldDirty(FieldControl ctrl)
    {
      return !(ctrl.Field.FieldID == "") && this.inputData.IsDirty(this.MapFieldId(this.parseFieldID(ctrl.Field.FieldID)));
    }

    public void ApplyTemplateFieldAccessRights() => this.ApplyFieldAccessRights(true, false);

    internal void ApplyFieldAccessRights(bool skipButtonFieldLockRules, bool ignoreTemplates = true)
    {
      if (ignoreTemplates && this.isUsingTemplate || this.isSuperAdmin || this is PLANCODECONFLICTSInputHandler)
        return;
      if (this.session.LoanDataMgr != null)
        this.fieldRights = this.session.LoanDataMgr.GetFieldAccessList();
      string empty = string.Empty;
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
      {
        FieldAccessMode fieldAccessMode = FieldAccessMode.NoRestrictions;
        if (this.fieldRights != null)
        {
          string fieldID = this.parseFieldID(allFieldControl.Field.FieldID);
          if (allFieldControl.ControlID.Equals("hmdaprofilename"))
            fieldID = "HMDA.X100";
          fieldAccessMode = this.GetFieldAccessMode(fieldID);
        }
        if (fieldAccessMode != allFieldControl.AccessMode)
          allFieldControl.AccessMode = fieldAccessMode;
      }
      foreach (EllieMae.Encompass.Forms.Calendar calendar in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.Calendar)))
      {
        FieldAccessMode fieldAccessMode = FieldAccessMode.NoRestrictions;
        if (this.fieldRights != null)
          fieldAccessMode = this.GetFieldAccessMode(this.parseFieldID(calendar.DateField.FieldID));
        if (fieldAccessMode != calendar.AccessMode)
          calendar.AccessMode = fieldAccessMode;
      }
      foreach (EllieMae.Encompass.Forms.TextBox textBox1 in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.TextBox)))
      {
        if (textBox1.ControlID.Contains("TextBoxPayout") && this is PURCHASEADVICEInputHandler)
        {
          PickList control = (PickList) this.currentForm.FindControl(textBox1.ControlID.Replace("TextBoxPayout", "PickListPayout"));
          this.GetFieldAccessMode(textBox1.Field.FieldID);
          if (textBox1.AccessMode != FieldAccessMode.NoRestrictions)
          {
            if (control != null)
              control.Visible = false;
          }
          else if (textBox1.Field == FieldDescriptor.Empty && control != null)
            control.Visible = false;
          Size size1;
          if (control.Visible)
          {
            EllieMae.Encompass.Forms.TextBox textBox2 = textBox1;
            int width = control.Left - textBox1.Left - 1;
            size1 = textBox1.Size;
            int height = size1.Height;
            Size size2 = new Size(width, height);
            textBox2.Size = size2;
          }
          else
          {
            EllieMae.Encompass.Forms.TextBox textBox3 = textBox1;
            int left = control.Left;
            size1 = control.Size;
            int width1 = size1.Width;
            int width2 = left + width1 - textBox1.Left - 1;
            size1 = textBox1.Size;
            int height = size1.Height;
            Size size3 = new Size(width2, height);
            textBox3.Size = size3;
          }
        }
      }
      if (skipButtonFieldLockRules)
        return;
      this.applyButtonAccessRights();
      this.applyFieldLockAccessRights();
    }

    internal FieldAccessMode GetFieldAccessMode(string fieldID)
    {
      if (this.fieldRights != null && this.session.LoanDataMgr != null)
      {
        string key = (string) null;
        switch (this)
        {
          case AFFILIATEDBAInputHandler _:
            key = "AB" + this.Property + (fieldID.Length > 6 ? (object) fieldID.Substring(5) : (object) fieldID.Substring(4));
            break;
          case VORInputHandler _:
          case VOMInputHandler _:
            if (!this.fieldRights.ContainsKey((object) fieldID))
            {
              int num = !(this is VOMInputHandler) ? ((VORInputHandler) this).ind.Length : this.Property.ToString().Length + 2;
              key = this.MapFieldId(fieldID).Substring(0, num) + fieldID.Substring(num);
              if (!this.fieldRights.ContainsKey((object) key))
              {
                key = key.Substring(0, num - 2) + "00" + key.Substring(num);
                break;
              }
              break;
            }
            break;
          case DISASTERInputHandler _:
            key = "FEMA" + this.Property.ToString() + (fieldID.Length > 8 ? fieldID.Substring(7) : fieldID.Substring(6));
            break;
        }
        bool flag = key != null && this.fieldRights.ContainsKey((object) key);
        if (this.fieldRights.ContainsKey((object) fieldID) | flag)
        {
          switch (this.session.LoanDataMgr.GetFieldAccessRights(flag ? key : fieldID))
          {
            case BizRule.FieldAccessRight.Hide:
              return FieldAccessMode.Hidden;
            case BizRule.FieldAccessRight.ViewOnly:
              return FieldAccessMode.ReadOnly;
          }
        }
      }
      return FieldAccessMode.NoRestrictions;
    }

    private void applyButtonAccessRights()
    {
      this.applyLoanTemplateRules();
      if (this.isUsingTemplate || this.isSuperAdmin)
        return;
      List<EllieMae.Encompass.Forms.Control> controlList = new List<EllieMae.Encompass.Forms.Control>();
      controlList.AddRange((IEnumerable<EllieMae.Encompass.Forms.Control>) this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.Button)));
      controlList.AddRange((IEnumerable<EllieMae.Encompass.Forms.Control>) this.currentForm.FindControlsByType(typeof (StandardButton)));
      controlList.AddRange((IEnumerable<EllieMae.Encompass.Forms.Control>) this.currentForm.FindControlsByType(typeof (ImageButton)));
      if (controlList == null || controlList.Count == 0)
        return;
      string empty = string.Empty;
      for (int index = 0; index < controlList.Count; ++index)
      {
        IActionable actionable = (IActionable) controlList[index];
        string str;
        try
        {
          str = "Button_" + actionable.Action;
        }
        catch (Exception ex)
        {
          continue;
        }
        switch (this.fieldRights == null || !this.fieldRights.ContainsKey((object) str) || this.session.LoanDataMgr == null ? BizRule.FieldAccessRight.DoesNotApply : this.session.LoanDataMgr.GetFieldAccessRights(str))
        {
          case BizRule.FieldAccessRight.Hide:
            if (controlList[index] is EllieMae.Encompass.Forms.Button)
            {
              ((RuntimeControl) controlList[index]).Visible = false;
              continue;
            }
            if (controlList[index] is ImageButton)
            {
              ((RuntimeControl) controlList[index]).Visible = false;
              continue;
            }
            ((RuntimeControl) controlList[index]).Visible = false;
            continue;
          case BizRule.FieldAccessRight.ViewOnly:
            if (controlList[index] is EllieMae.Encompass.Forms.Button)
            {
              ((RuntimeControl) controlList[index]).Enabled = false;
              continue;
            }
            if (controlList[index] is ImageButton)
            {
              ((RuntimeControl) controlList[index]).Enabled = false;
              continue;
            }
            ((RuntimeControl) controlList[index]).Enabled = false;
            continue;
          default:
            if (string.Compare(actionable.Action, "aiqanalyzers", true) == 0)
            {
              bool? nullable;
              ref bool? local = ref nullable;
              int num;
              if (this.session.StartupInfo.UserAclFeaturConfigRights.ContainsKey(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers))
                num = ((IEnumerable<int>) new int[2]
                {
                  1,
                  int.MaxValue
                }).Contains<int>(this.session.StartupInfo.UserAclFeaturConfigRights[AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers]) ? 1 : 0;
              else
                num = 0;
              local = new bool?(num != 0);
              if (!nullable.Value)
              {
                if (controlList[index] is EllieMae.Encompass.Forms.Button)
                {
                  ((RuntimeControl) controlList[index]).Enabled = false;
                  continue;
                }
                if (controlList[index] is ImageButton)
                {
                  ((RuntimeControl) controlList[index]).Enabled = false;
                  continue;
                }
                ((RuntimeControl) controlList[index]).Enabled = false;
                continue;
              }
              continue;
            }
            continue;
        }
      }
    }

    public void EnableDisableControl(
      ImageButton button,
      Dictionary<string, bool> controlWithPermission,
      bool result,
      string sourceEnable,
      string sourceDisable)
    {
      if (!controlWithPermission.ContainsKey(button.Action))
        return;
      button.Enabled = !controlWithPermission[button.Action] && result;
      button.Source = !result || controlWithPermission[button.Action] ? sourceDisable : sourceEnable;
    }

    private void applyLoanTemplateRules()
    {
      if (this.loan != null && !this.FormIsForTemplate)
        return;
      EllieMae.Encompass.Forms.Control[] controlsByType = this.currentForm != null ? this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.Button)) : (EllieMae.Encompass.Forms.Control[]) null;
      if (controlsByType == null || controlsByType.Length == 0)
        return;
      for (int index = 0; index < controlsByType.Length; ++index)
      {
        EllieMae.Encompass.Forms.Button button = (EllieMae.Encompass.Forms.Button) controlsByType[index];
        string action = button.Action;
        if (action == "prequal_vod" || action == "vol")
          button.Enabled = false;
      }
    }

    private void applyEncompassVersion()
    {
      foreach (EllieMae.Encompass.Forms.Button button in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.Button)))
      {
        if (button.Action.ToLower() == "mersmin" && this.session.EncompassEdition == EncompassEdition.Broker)
        {
          button.Visible = false;
          EllieMae.Encompass.Forms.Label label = new EllieMae.Encompass.Forms.Label();
          button.GetContainer().Controls.Insert((EllieMae.Encompass.Forms.Control) label);
          label.Size = button.Size;
          label.Text = button.Text;
          Point absolutePosition = button.AbsolutePosition;
          absolutePosition.Offset(1, 3);
          label.AbsolutePosition = absolutePosition;
          button.Delete();
        }
      }
    }

    private void applyFieldLockAccessRights()
    {
      if (this.isUsingTemplate || this.fieldRights == null || this.session.LoanDataMgr == null || this.isSuperAdmin)
        return;
      EllieMae.Encompass.Forms.Control[] controlsByType = this.currentForm.FindControlsByType(typeof (FieldLock));
      if (controlsByType == null || controlsByType.Length == 0)
        return;
      string empty = string.Empty;
      for (int index = 0; index < controlsByType.Length; ++index)
      {
        FieldLock lockCtrl = (FieldLock) controlsByType[index];
        string fieldLockId = this.findFieldLockID(lockCtrl);
        if (!(fieldLockId == string.Empty))
        {
          string str = "LOCKBUTTON_" + fieldLockId;
          if (this.fieldRights.ContainsKey((object) str))
          {
            switch (this.session.LoanDataMgr.GetFieldAccessRights(str))
            {
              case BizRule.FieldAccessRight.Hide:
                lockCtrl.Visible = false;
                break;
              case BizRule.FieldAccessRight.ViewOnly:
                lockCtrl.Enabled = false;
                break;
              default:
                lockCtrl.Visible = true;
                break;
            }
          }
          else
          {
            lockCtrl.Visible = true;
            if (this.currentForm.Name == "Loan Estimate Page 1" && lockCtrl.ControlID == "FieldLock20")
              lockCtrl.Enabled = this.loan.GetLogList().GetAllIDisclosureTracking2015Log(false).Length == 0;
            else
              lockCtrl.Enabled = true;
          }
          if (lockCtrl.ControlID == "ToleranceCureLock" && !((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_CureToleranceAlert))
            lockCtrl.Enabled = false;
        }
      }
    }

    private void applyFieldRules()
    {
      if (this is PLANCODECONFLICTSInputHandler)
        return;
      bool flag = this.GetField("1172") == "FarmersHomeAdministration";
      if (this.loan == null)
      {
        foreach (EllieMae.Encompass.Forms.TextBox textBox1 in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.TextBox)))
        {
          PickList control = (PickList) this.currentForm.FindControl("pklist_" + textBox1.Field.FieldID.Replace(".", ""));
          if (control != null)
          {
            if (this.feeManagementSetting != null && this.feeManagementSetting.CompanyOptIn)
            {
              FeeSectionEnum fieldSectionEnum = HUDGFE2010Fields.GetFieldSectionEnum(textBox1.Field.FieldID);
              if (fieldSectionEnum != FeeSectionEnum.Nothing)
              {
                FRList frList = new FRList(this.feeManagementSetting.GetFeeNames(HUDGFE2010Fields.GetFieldSectionEnum(textBox1.Field.FieldID)), false);
                if (frList.IsLock && !this.isSuperAdmin)
                  textBox1.AccessMode = FieldAccessMode.ReadOnly;
                if (!this.feeManagementPermission.IsSectionEditable(fieldSectionEnum))
                  textBox1.AccessMode = FieldAccessMode.ReadOnly;
                if (textBox1.Field.FieldID == "NEWHUD.X1299" & flag && (this.loan == null || this.loan != null && !this.loan.IsLocked("NEWHUD.X1301")))
                {
                  if (control != null)
                    control.Visible = false;
                  textBox1.AccessMode = FieldAccessMode.ReadOnly;
                }
                else if (frList.List != null && (frList.List.Length > 1 || frList.List.Length == 1 && textBox1.AccessMode == FieldAccessMode.ReadOnly))
                {
                  if (this is REGZGFE_2010InputHandler)
                  {
                    DropdownOption[] optionList = new DropdownOption[frList.List.Length];
                    for (int index = 0; index < frList.List.Length; ++index)
                      optionList[index] = new DropdownOption(frList.List[index]);
                    control.Options.Clear();
                    control.Options.AddRange((ICollection) optionList);
                    control.Visible = true;
                    textBox1.AttachedPickList(control);
                  }
                  else
                    textBox1.CreatePickList(frList.List);
                }
              }
            }
            else if (control != null)
              control.Visible = false;
            Size size1;
            if (control != null)
            {
              if (control.Visible)
              {
                EllieMae.Encompass.Forms.TextBox textBox2 = textBox1;
                int width = control.Left - textBox1.Left - 1;
                size1 = textBox1.Size;
                int height = size1.Height;
                Size size2 = new Size(width, height);
                textBox2.Size = size2;
              }
              else
              {
                EllieMae.Encompass.Forms.TextBox textBox3 = textBox1;
                int left = control.Left;
                size1 = control.Size;
                int width1 = size1.Width;
                int width2 = left + width1 - textBox1.Left - 1;
                size1 = textBox1.Size;
                int height = size1.Height;
                Size size3 = new Size(width2, height);
                textBox3.Size = size3;
              }
            }
          }
        }
      }
      else
      {
        Hashtable dropdownFieldRuleList = this.session.LoanDataMgr != null ? this.session.LoanDataMgr.GetDropdownFieldRuleList() : (Hashtable) null;
        Dictionary<FeeSectionEnum, FRList> dictionary1 = new Dictionary<FeeSectionEnum, FRList>();
        foreach (EllieMae.Encompass.Forms.TextBox textBox4 in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.TextBox)))
        {
          PickList control = (PickList) this.currentForm.FindControl("pklist_" + textBox4.Field.FieldID.Replace(".", ""));
          FieldAccessMode fieldAccessMode = this.GetFieldAccessMode(textBox4.Field.FieldID);
          if (textBox4.Field.FieldID == "NEWHUD.X1299" & flag && (this.loan == null || this.loan != null && !this.loan.IsLocked("NEWHUD.X1301")))
            fieldAccessMode = FieldAccessMode.ReadOnly;
          FRList frList1 = (FRList) null;
          if (textBox4.AccessMode != FieldAccessMode.NoRestrictions)
          {
            if (control != null)
              control.Visible = false;
          }
          else if (dropdownFieldRuleList == null)
          {
            if (control != null)
              control.Visible = false;
          }
          else if (textBox4.Field == FieldDescriptor.Empty)
          {
            if (control != null)
              control.Visible = false;
          }
          else if (!dropdownFieldRuleList.ContainsKey((object) textBox4.Field.FieldID))
          {
            if (control != null)
              control.Visible = false;
          }
          else if (fieldAccessMode == FieldAccessMode.NoRestrictions)
          {
            frList1 = (FRList) dropdownFieldRuleList[(object) textBox4.Field.FieldID];
            if (frList1.IsLock && !this.isSuperAdmin)
              textBox4.AccessMode = FieldAccessMode.ReadOnly;
            textBox4.CreatePickList(frList1.List);
          }
          else if (control != null)
            control.Visible = false;
          if (textBox4.Field.FieldID == "315" && fieldAccessMode == FieldAccessMode.NoRestrictions && (dropdownFieldRuleList == null || !dropdownFieldRuleList.ContainsKey((object) textBox4.Field.FieldID)) && this.loan != null)
          {
            if (this.loan.DBANames == null)
              this.loan.DBANames = this.getDBANames();
            if (this.loan.DBANames != null)
              textBox4.CreatePickList(this.loan.DBANames);
            else if (control != null)
              control.Visible = false;
          }
          if (control != null)
          {
            if (this.feeManagementSetting != null && this.feeManagementSetting.CompanyOptIn && fieldAccessMode == FieldAccessMode.NoRestrictions)
            {
              FeeSectionEnum fieldSectionEnum = HUDGFE2010Fields.GetFieldSectionEnum(textBox4.Field.FieldID);
              if (fieldSectionEnum != FeeSectionEnum.Nothing)
              {
                FRList frList2;
                if (dictionary1.ContainsKey(fieldSectionEnum))
                {
                  frList2 = dictionary1[fieldSectionEnum];
                }
                else
                {
                  frList2 = new FRList(this.feeManagementSetting.GetFeeNames(fieldSectionEnum), false);
                  dictionary1.Add(fieldSectionEnum, frList2);
                }
                if (textBox4.AccessMode != FieldAccessMode.ReadOnly && !this.feeManagementPermission.IsSectionEditable(fieldSectionEnum))
                  textBox4.AccessMode = FieldAccessMode.ReadOnly;
                string[] listItems = frList2.List;
                if (frList1 != null)
                {
                  Dictionary<string, string> dictionary2 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
                  for (int index = 0; index < frList2.List.Length; ++index)
                  {
                    if (!dictionary2.ContainsKey(frList2.List[index]))
                      dictionary2.Add(frList2.List[index], "");
                  }
                  List<string> stringList = new List<string>();
                  for (int index = 0; index < frList1.List.Length; ++index)
                  {
                    if (dictionary2.ContainsKey(frList1.List[index]))
                      stringList.Add(frList1.List[index]);
                  }
                  listItems = stringList.ToArray();
                }
                switch (this)
                {
                  case REGZGFE_2010InputHandler _:
                  case HUD1PG2_2010InputHandler _:
                  case SECTION32InputHandler _:
                  case ULDDInputHandler _:
                  case HUD1ESInputHandler _:
                    if (control != null)
                      control.Visible = false;
                    if (listItems.Length != 0 && fieldAccessMode == FieldAccessMode.NoRestrictions)
                    {
                      control.Visible = true;
                      textBox4.AttachedPickList(control);
                      break;
                    }
                    control.Visible = false;
                    break;
                  default:
                    if (listItems.Length != 0 && fieldAccessMode == FieldAccessMode.NoRestrictions)
                    {
                      textBox4.CreatePickList(listItems);
                      break;
                    }
                    control.Visible = false;
                    break;
                }
              }
            }
            else if (this.feeManagementSetting == null || !this.feeManagementSetting.CompanyOptIn)
            {
              switch (this)
              {
                case REGZGFE_2010InputHandler _:
                case HUD1PG2_2010InputHandler _:
                case SECTION32InputHandler _:
                case ULDDInputHandler _:
                  control.Visible = false;
                  break;
              }
            }
            Size size4;
            if (control.Visible)
            {
              EllieMae.Encompass.Forms.TextBox textBox5 = textBox4;
              int width = control.Left - textBox4.Left - 1;
              size4 = textBox4.Size;
              int height = size4.Height;
              Size size5 = new Size(width, height);
              textBox5.Size = size5;
            }
            else
            {
              EllieMae.Encompass.Forms.TextBox textBox6 = textBox4;
              int left = control.Left;
              size4 = control.Size;
              int width3 = size4.Width;
              int width4 = left + width3 - textBox4.Left - 1;
              size4 = textBox4.Size;
              int height = size4.Height;
              Size size6 = new Size(width4, height);
              textBox6.Size = size6;
            }
          }
        }
        if (this.loan == null || this.loan.IsTemplate || dropdownFieldRuleList == null || !dropdownFieldRuleList.ContainsKey((object) "33"))
          return;
        foreach (DropdownBox dropdownBox in this.currentForm.FindControlsByType(typeof (DropdownBox)))
        {
          if (!(dropdownBox.Field.FieldID != "33"))
          {
            string[] source = new string[5]
            {
              "Sole Ownership",
              "Life Estate",
              "Tenancy in Common",
              "Joint Tenancy with Right of Survivorship",
              "Tenancy by the Entirety"
            };
            FRList frList = (FRList) dropdownFieldRuleList[(object) "33"];
            ArrayList arrayList = new ArrayList((ICollection) frList.List);
            foreach (string str in frList.List)
            {
              if (this.loan.GetField("1825") != "2020" && ((IEnumerable<string>) source).Contains<string>(str))
                arrayList.Remove((object) str);
            }
            if (dropdownBox is DropdownEditBox)
            {
              ((DropdownEditBox) dropdownBox).CreatePickList((string[]) arrayList.ToArray(typeof (string)));
            }
            else
            {
              dropdownBox.Options.Clear();
              dropdownBox.Options.Add(new DropdownOption("", ""));
              for (int index = 0; index < arrayList.Count; ++index)
                dropdownBox.Options.Add(new DropdownOption((string) arrayList[index], (string) arrayList[index]));
            }
          }
        }
      }
    }

    private string[] getDBANames()
    {
      UserInfo userInfo = (UserInfo) null;
      if (this.loan != null)
      {
        string userId = this.loan.GetField("LOID");
        if (userId == string.Empty)
        {
          MilestoneLog[] allMilestones = this.loan.GetLogList().GetAllMilestones();
          if (allMilestones == null)
            return (string[]) null;
          if (string.Compare(allMilestones[0].Stage, "Started", true) == 0)
            userId = allMilestones[0].LoanAssociateID;
        }
        if (userId == string.Empty)
          return (string[]) null;
        userInfo = this.session.OrganizationManager.GetUser(userId);
      }
      if (userInfo == (UserInfo) null)
        userInfo = this.session.UserInfo;
      if (userInfo == (UserInfo) null)
        return (string[]) null;
      return this.session.OrganizationManager.GetFirstAvaliableOrganization(userInfo.OrgId)?.GetDBANames();
    }

    private void setEnabledDisabledState(RuntimeControl control, string fieldActionId)
    {
      ControlState controlState = this.GetControlState(control, fieldActionId);
      if (this.loan != null && this.loan.IsInFindFieldForm)
      {
        switch (control)
        {
          case EllieMae.Encompass.Forms.Button _:
            if (this.loan.ButtonSelectionEnabled)
            {
              control.Enabled = true;
              EllieMae.Encompass.Forms.Button button = (EllieMae.Encompass.Forms.Button) control;
              switch (this.loan.SelectedFieldType("Button_" + button.Action))
              {
                case LoanData.FindFieldTypes.NewSelect:
                  button.BackColor = InputHandlerBase.SelectedFieldColor;
                  return;
                case LoanData.FindFieldTypes.Existing:
                  button.BackColor = InputHandlerBase.ExistingFieldColor;
                  return;
                default:
                  button.BackColor = System.Drawing.Color.LightGray;
                  return;
              }
            }
            else
            {
              control.Enabled = false;
              return;
            }
          case ImageButton _:
            if (this.loan.ButtonSelectionEnabled)
            {
              control.Enabled = true;
              switch (this.loan.SelectedFieldType("Button_" + ((ImageButton) control).Action))
              {
                case LoanData.FindFieldTypes.NewSelect:
                  control.HTMLElement.style.width = control.HTMLElement.style.height = (object) "20px";
                  control.HTMLElement.style.border = InputHandlerBase.HTMLSelectedColor;
                  break;
                case LoanData.FindFieldTypes.Existing:
                  control.HTMLElement.style.width = control.HTMLElement.style.height = (object) "20px";
                  control.HTMLElement.style.border = InputHandlerBase.HTMLExistingColor;
                  break;
                default:
                  control.HTMLElement.style.width = control.HTMLElement.style.height = (object) "16px";
                  control.HTMLElement.style.border = "";
                  break;
              }
            }
            else
              break;
            break;
          case StandardButton _:
            if (this.loan.ButtonSelectionEnabled)
            {
              control.Enabled = true;
              switch (this.loan.SelectedFieldType("Button_" + ((StandardButton) control).Action))
              {
                case LoanData.FindFieldTypes.NewSelect:
                  control.HTMLElement.style.width = control.HTMLElement.style.height = (object) "20px";
                  control.HTMLElement.style.border = InputHandlerBase.HTMLSelectedColor;
                  return;
                case LoanData.FindFieldTypes.Existing:
                  control.HTMLElement.style.width = control.HTMLElement.style.height = (object) "20px";
                  control.HTMLElement.style.border = InputHandlerBase.HTMLExistingColor;
                  return;
                default:
                  control.HTMLElement.style.width = control.HTMLElement.style.height = (object) "16px";
                  control.HTMLElement.style.border = "";
                  return;
              }
            }
            else
            {
              control.Enabled = false;
              return;
            }
        }
      }
      if (this.loan != null && control is FieldControl)
      {
        FieldControl fieldControl = (FieldControl) control;
        if (fieldControl.Field.Format == LoanFieldFormat.STATE || fieldControl.Field.Format == LoanFieldFormat.ZIPCODE)
          ((EllieMae.Encompass.Forms.TextBox) control).MaxLength = !this.loan.IsForeignIndicatorSelected(this.MapFieldId(fieldControl.Field.FieldID)) ? (fieldControl.Field.Format == LoanFieldFormat.STATE ? 2 : 10) : (fieldControl.Field.Format == LoanFieldFormat.STATE ? 128 : 64);
      }
      if (control is FieldControl && controlState != ControlState.Default)
      {
        FieldControl fieldControl = (FieldControl) control;
        if (fieldControl.Field != null && fieldControl.Field.ReadOnly)
        {
          string fieldId = fieldControl.Field.FieldID;
          if (((!fieldId.StartsWith("FE") || fieldId.StartsWith("FEMA")) && !fieldId.StartsWith("FR") || !fieldId.EndsWith("37")) && (!fieldId.StartsWith("FE") || !fieldId.EndsWith("56") && !fieldId.EndsWith("19") && !fieldId.EndsWith("79")) && (!fieldId.StartsWith("FR") || !fieldId.EndsWith("30")) && !fieldId.EndsWith("40") && (!fieldId.StartsWith("FM") || !fieldId.EndsWith("57")) && (!fieldId.StartsWith("DD") || !fieldId.EndsWith("40")) && (!fieldId.StartsWith("FL") || !fieldId.EndsWith("68")) && (!fieldId.StartsWith("URLAROL") || !fieldId.EndsWith("22")))
            return;
        }
      }
      if (controlState == ControlState.Enabled)
      {
        control.Enabled = true;
      }
      else
      {
        if (controlState != ControlState.Disabled)
          return;
        control.Enabled = false;
      }
    }

    protected virtual void SetFieldFocus(string controlId)
    {
      if (this.currentForm == null || !(this.currentForm.FindControl(controlId) is FieldControl control))
        return;
      control.Focus();
    }

    protected virtual void SetFieldSelect(string controlId)
    {
      if (!(this.currentForm.FindControl(controlId) is FieldControl control))
        return;
      control.Select();
    }

    protected string GetEMAttribute(mshtml.IHTMLElement elm, string attr)
    {
      object attribute = elm.getAttribute(attr);
      string emAttribute = string.Empty;
      if (attribute is string)
        emAttribute = (string) attribute;
      return emAttribute;
    }

    protected virtual void ClearFileContact(string id)
    {
      switch (id)
      {
        case "617":
          this.UpdateFieldValue("618", "");
          this.UpdateFieldValue("622", "");
          this.UpdateFieldValue("89", "");
          this.UpdateFieldValue("1246", "");
          this.UpdateFieldValue("619", "");
          this.UpdateFieldValue("620", "");
          this.UpdateFieldValue("1244", "");
          this.UpdateFieldValue("621", "");
          break;
        case "624":
          this.UpdateFieldValue("625", "");
          this.UpdateFieldValue("629", "");
          this.UpdateFieldValue("90", "");
          this.UpdateFieldValue("1247", "");
          this.UpdateFieldValue("626", "");
          this.UpdateFieldValue("627", "");
          this.UpdateFieldValue("1245", "");
          this.UpdateFieldValue("628", "");
          break;
        case "REGZGFE.X8":
          this.UpdateFieldValue("984", "");
          this.UpdateFieldValue("1410", "");
          this.UpdateFieldValue("1411", "");
          this.UpdateFieldValue("VEND.X176", "");
          this.UpdateFieldValue("VEND.X171", "");
          this.UpdateFieldValue("VEND.X172", "");
          this.UpdateFieldValue("VEND.X173", "");
          this.UpdateFieldValue("VEND.X174", "");
          break;
        case "L248":
          this.UpdateFieldValue("707", "");
          this.UpdateFieldValue("711", "");
          this.UpdateFieldValue("93", "");
          this.UpdateFieldValue("1254", "");
          this.UpdateFieldValue("708", "");
          this.UpdateFieldValue("709", "");
          this.UpdateFieldValue("1252", "");
          this.UpdateFieldValue("710", "");
          break;
        case "L252":
          this.UpdateFieldValue("VEND.X162", "");
          this.UpdateFieldValue("VEND.X163", "");
          this.UpdateFieldValue("VEND.X164", "");
          this.UpdateFieldValue("VEND.X165", "");
          this.UpdateFieldValue("VEND.X157", "");
          this.UpdateFieldValue("VEND.X158", "");
          this.UpdateFieldValue("VEND.X159", "");
          this.UpdateFieldValue("VEND.X160", "");
          break;
        case "1500":
          this.UpdateFieldValue("VEND.X13", "");
          this.UpdateFieldValue("VEND.X19", "");
          this.UpdateFieldValue("VEND.X21", "");
          this.UpdateFieldValue("VEND.X20", "");
          this.UpdateFieldValue("VEND.X14", "");
          this.UpdateFieldValue("VEND.X15", "");
          this.UpdateFieldValue("VEND.X16", "");
          this.UpdateFieldValue("VEND.X17", "");
          break;
        case "610":
          this.UpdateFieldValue("611", "");
          this.UpdateFieldValue("615", "");
          this.UpdateFieldValue("87", "");
          this.UpdateFieldValue("1011", "");
          this.UpdateFieldValue("612", "");
          this.UpdateFieldValue("613", "");
          this.UpdateFieldValue("1175", "");
          this.UpdateFieldValue("614", "");
          break;
        case "395":
          this.UpdateFieldValue("VEND.X195", "");
          this.UpdateFieldValue("VEND.X196", "");
          this.UpdateFieldValue("VEND.X197", "");
          this.UpdateFieldValue("VEND.X198", "");
          this.UpdateFieldValue("VEND.X190", "");
          this.UpdateFieldValue("VEND.X191", "");
          this.UpdateFieldValue("VEND.X192", "");
          this.UpdateFieldValue("VEND.X193", "");
          break;
        case "411":
          this.UpdateFieldValue("416", "");
          this.UpdateFieldValue("417", "");
          this.UpdateFieldValue("88", "");
          this.UpdateFieldValue("1243", "");
          this.UpdateFieldValue("412", "");
          this.UpdateFieldValue("413", "");
          this.UpdateFieldValue("1174", "");
          this.UpdateFieldValue("414", "");
          break;
        case "56":
          this.UpdateFieldValue("VEND.X117", "");
          this.UpdateFieldValue("VEND.X118", "");
          this.UpdateFieldValue("VEND.X119", "");
          this.UpdateFieldValue("VEND.X120", "");
          this.UpdateFieldValue("VEND.X112", "");
          this.UpdateFieldValue("VEND.X113", "");
          this.UpdateFieldValue("VEND.X114", "");
          this.UpdateFieldValue("VEND.X115", "");
          break;
        case "1264":
          this.UpdateFieldValue("1257", "");
          this.UpdateFieldValue("1258", "");
          this.UpdateFieldValue("1259", "");
          this.UpdateFieldValue("1260", "");
          this.UpdateFieldValue("1256", "");
          this.UpdateFieldValue("1262", "");
          this.UpdateFieldValue("95", "");
          this.UpdateFieldValue("1263", "");
          this.UpdateFieldValue("305", "");
          this.UpdateFieldValue("VEND.X1034", "");
          this.UpdateFieldValue("VEND.X1035", "");
          break;
      }
    }

    private void setStatusBarInformation(FieldControl control)
    {
      if (this.mainScreen == null)
        return;
      if (control == null)
      {
        this.clearStatusBarInformation();
      }
      else
      {
        IStatusDisplay service = this.session.Application.GetService<IStatusDisplay>();
        service.DisplayFieldID(this.MapFieldId(this.parseFieldID(control.Field.FieldID)));
        service.DisplayHelpText("Press F1 for Help");
        if (!(control is EllieMae.Encompass.Forms.TextBox) || ((EllieMae.Encompass.Forms.TextBox) control).RolodexField == EllieMae.Encompass.Forms.RolodexField.None)
          return;
        service.DisplayHelpText("Right click to bring up Rolodex");
      }
    }

    private void clearStatusBarInformation()
    {
      if (this.mainScreen == null)
        return;
      IStatusDisplay service = this.session.Application.GetService<IStatusDisplay>();
      service.DisplayFieldID("");
      service.DisplayHelpText("");
    }

    public virtual void RefreshContents(string id) => this.refreshContents(id, false);

    private void refreshContents(string id, bool fireEventOnChange)
    {
      if (this.loan == null || this.isUsingTemplate)
        return;
      this.loan = this.session.LoanData;
      this.inputData = (IHtmlInput) this.loan;
      if (this.session.LoanDataMgr != null)
      {
        this.feeManagementSetting = this.session.LoanDataMgr.SystemConfiguration.FeeManagementList;
        this.feeManagementPermission = this.session.LoanDataMgr.SystemConfiguration.FeeManagementPersonaPermission;
      }
      else
      {
        this.feeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
        this.feeManagementPermission = this.loadFeeManagementPermission();
      }
      if (this.currentForm == null)
        return;
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
      {
        if (allFieldControl.Field.FieldID == id)
          this.populateFieldFromDataSource(allFieldControl, fireEventOnChange);
      }
    }

    public virtual void RefreshContents() => this.RefreshContents(false);

    public virtual void RefreshContents(bool skipButtonFieldLockRules)
    {
      if (this.loan != null && !this.isUsingTemplate && this.session.LoanDataMgr != null)
      {
        this.loan = this.session.LoanData;
        this.inputData = (IHtmlInput) this.loan;
        if (this.session.LoanDataMgr != null)
        {
          this.feeManagementSetting = this.session.LoanDataMgr.SystemConfiguration.FeeManagementList;
          this.feeManagementPermission = this.session.LoanDataMgr.SystemConfiguration.FeeManagementPersonaPermission;
        }
        else
        {
          this.feeManagementSetting = this.session.ConfigurationManager.GetFeeManagement();
          this.feeManagementPermission = this.loadFeeManagementPermission();
        }
      }
      this.UpdateContents(true, false, skipButtonFieldLockRules);
      this.setStatusBarInformation(this.currentField);
    }

    public void RefreshLoanContents() => this.RefreshContents();

    public bool ValidateCommitmentTypeDeliveryType()
    {
      if (this.session.LoanData.GetField("3966") == "Y" && string.IsNullOrEmpty(this.session.LoanData.GetField("4187")))
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, "Commitment Type is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.loan == null || !this.loan.GetField("2626").Equals("Correspondent") || string.IsNullOrEmpty(this.loan.GetField("TPO.X14")) || !string.IsNullOrEmpty(this.loan.GetField("4187")) || !(this.session.LoanData.GetField("3966") == "N"))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this.session.Application, "Individual Loan Locks (Mandatory or Best Efforts), have not been setup for this TPO Company [TPO.X14]. Please contact Secondary.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    public bool SetGoToFieldFocus(string fieldID, int count)
    {
      FieldControl state = (FieldControl) null;
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
      {
        if (string.Compare(this.parseFieldID(allFieldControl.Field.FieldID), fieldID, true) == 0)
        {
          state = allFieldControl;
          break;
        }
      }
      if (state == null)
        return false;
      state.EnsureVisible();
      if (state is DropdownEditBox)
      {
        this.ignoreAsyncFocus = false;
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.setFocusAsync), (object) state);
      }
      else if ((state is EllieMae.Encompass.Forms.TextBox || state is DropdownBox) && state.Interactive)
      {
        this.ignoreAsyncFocus = false;
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.setFocusAsync), (object) state);
      }
      else
      {
        ThreadPool.QueueUserWorkItem(new WaitCallback(this.flashGoToField), (object) state);
        this.ignoreAsyncFocus = true;
      }
      return true;
    }

    private void flashGoToField(object state)
    {
      FieldControl c = (FieldControl) state;
      if (c == null)
        return;
      c.EnsureVisible();
      System.Drawing.Color backColor = c.BackColor;
      for (int index = 0; index < 5; ++index)
      {
        this.setControlBackColor(c, System.Drawing.Color.Red);
        Thread.Sleep(300);
        this.setControlBackColor(c, backColor);
        Thread.Sleep(300);
      }
    }

    private void setControlBackColor(FieldControl c, System.Drawing.Color backColor)
    {
      if (this.session.Application.InvokeRequired)
      {
        this.session.Application.Invoke((Delegate) new InputHandlerBase.setControlBackColorDelegate(this.setControlBackColor), new object[2]
        {
          (object) c,
          (object) backColor
        });
      }
      else
      {
        try
        {
          c.BackColor = backColor;
        }
        catch
        {
        }
      }
    }

    private void setFocusToFirstField()
    {
      FieldControl state = (FieldControl) null;
      this.ignoreAsyncFocus = false;
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
      {
        if (allFieldControl.TabIndex > 0 && (state == null || allFieldControl.TabIndex < state.TabIndex) && allFieldControl.Interactive)
        {
          state = allFieldControl;
          if (state.TabIndex == 1)
            break;
        }
      }
      if (state == null)
        return;
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.setFocusAsync), (object) state);
    }

    private void setFocusAsync(object state)
    {
      try
      {
        FieldControl fieldControl = (FieldControl) state;
        fieldControl.EnsureVisible();
        if (this.session.MainScreen.InvokeRequired)
        {
          this.session.MainScreen.Invoke((Delegate) new WaitCallback(this.setFocusAsync), new object[1]
          {
            state
          });
        }
        else
        {
          if (this.ignoreAsyncFocus)
            return;
          fieldControl.Focus();
        }
      }
      catch
      {
      }
    }

    private bool pushEvent(EllieMae.Encompass.Forms.Control ctrl, string eventName)
    {
      InputHandlerBase.EventInfo eventInfo = new InputHandlerBase.EventInfo(ctrl, eventName);
      if (this.eventStack.Contains((object) eventInfo))
        return false;
      this.eventStack.Push((object) eventInfo);
      return true;
    }

    private void popEvent() => this.eventStack.Pop();

    private void populateFieldFromDataSource(FieldControl control)
    {
      this.populateFieldFromDataSource(control, false);
    }

    private void populateFieldFromDataSource(FieldControl control, bool fireEventOnChange)
    {
      if (control.Field.FieldID == "")
        return;
      string id = this.MapFieldId(this.parseFieldID(control.Field.FieldID));
      if (!this.isUsingTemplate && !this.isSuperAdmin && (this.session.LoanDataMgr == null || this.session.LoanDataMgr.GetFieldAccessRights(id) == BizRule.FieldAccessRight.Hide))
        return;
      using (PerformanceMeter.Current.BeginOperation("InputHandlerBase.populateField"))
      {
        string fieldValue = this.GetFieldValue(id, control.FieldSource);
        if (!this.executeDataBindEvent(control, ref fieldValue))
          return;
        bool flag = false;
        this.WriteDebug("Auto-binding control:  Control ID = '" + control.ControlID + "', Field ID = '" + id + "', Source = '" + (object) control.FieldSource + "', Value = '" + fieldValue + "'");
        using (PerformanceMeter.Current.BeginOperation("InputHandlerBase.populateField.BindTo"))
          flag = control.BindTo(fieldValue);
        if (!(flag & fireEventOnChange))
          return;
        this.executeChangeEvent((RuntimeControl) control);
      }
    }

    private bool executeDataBindEvent(FieldControl control, ref string value)
    {
      bool flag = this.pushEvent((EllieMae.Encompass.Forms.Control) control, "DataBind");
      if (!flag)
        return false;
      try
      {
        ISupportsDataBindEvent supportsDataBindEvent = (ISupportsDataBindEvent) control;
        using (PerformanceMeter.Current.BeginOperation("InputHandlerBase.populateField.InvokeDataBind"))
        {
          if (supportsDataBindEvent != null)
            flag = supportsDataBindEvent.InvokeDataBind(ref value);
        }
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "DataBind", ex);
      }
      finally
      {
        this.popEvent();
      }
      return flag;
    }

    private bool executeDataCommitEvent(FieldControl control, ref string value)
    {
      bool flag = true;
      try
      {
        ISupportsDataBindEvent supportsDataBindEvent = (ISupportsDataBindEvent) control;
        if (supportsDataBindEvent != null)
          flag = supportsDataBindEvent.InvokeDataCommit(ref value);
        return flag;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "DataCommit", ex);
        return false;
      }
    }

    private bool executeClickEvent(RuntimeControl control, out bool retVal)
    {
      retVal = true;
      try
      {
        if (control is ISupportsClickEvent supportsClickEvent)
          retVal = supportsClickEvent.InvokeClick();
        return supportsClickEvent != null;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "Click", ex);
        return true;
      }
    }

    private bool executeChangeEvent(RuntimeControl control)
    {
      try
      {
        if (control is ISupportsChangeEvent supportsChangeEvent)
          supportsChangeEvent.InvokeChange();
        return supportsChangeEvent != null;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "Change", ex);
        return true;
      }
    }

    public virtual bool executeDateSelectedEvent(RuntimeControl control, DateTime selectedDate)
    {
      try
      {
        if (control is ISupportsDateSelectedEvent dateSelectedEvent)
          dateSelectedEvent.InvokeDateSelected(selectedDate);
        return dateSelectedEvent != null;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "DateSelected", ex);
        return true;
      }
    }

    private bool executeFocusInEvent(RuntimeControl control)
    {
      try
      {
        if (control is ISupportsFocusEvent supportsFocusEvent)
          supportsFocusEvent.InvokeFocusIn();
        return supportsFocusEvent != null;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "FocusIn", ex);
        return true;
      }
    }

    private bool executeFocusOutEvent(RuntimeControl control)
    {
      try
      {
        if (control is ISupportsFocusEvent supportsFocusEvent)
          supportsFocusEvent.InvokeFocusOut();
        return supportsFocusEvent != null;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) control, "FocusOut", ex);
        return true;
      }
    }

    private void executeLoadEvent()
    {
      try
      {
        this.currentForm.InvokeLoad();
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) this.currentForm, "Load", ex);
      }
    }

    private void executeUnloadEvent()
    {
      try
      {
        this.currentForm.InvokeUnload();
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) this.currentForm, "Unload", ex);
      }
      this.currentForm.Close();
      this.currentForm = (EllieMae.Encompass.Forms.Form) null;
    }

    private void displayCustomCodeRuntimeError(EllieMae.Encompass.Forms.Control c, string eventName, Exception ex)
    {
      ErrorDialog.Display("An error has occurred in the '" + eventName + "' event handler for control '" + c.ControlID + "': " + ex.Message, ex);
    }

    private void setCurrentField(FieldControl ctrl)
    {
      bool flag = false;
      if (this.currentField != null)
      {
        flag |= this.executeFocusOutEvent((RuntimeControl) this.currentField);
        this.currentField = (FieldControl) null;
      }
      if (ctrl != null)
      {
        this.currentField = ctrl;
        flag |= this.executeFocusInEvent((RuntimeControl) ctrl);
      }
      if (flag)
        this.UpdateContents();
      this.setStatusBarInformation(ctrl);
      this.isDeactivating = false;
      this.reformatPending = false;
      this.forceChangeEvent = false;
    }

    private void reformatControlValue(FieldControl ctrl)
    {
      if (!(ctrl is ISupportsFormatEvent supportsFormatEvent))
        return;
      string orgval = ctrl.Value;
      string str1 = ctrl.Value;
      bool flag = true;
      bool needsUpdate = false;
      try
      {
        if (ctrl.Field.FieldID.Length == 6 && (ctrl.Field.FieldID.EndsWith("04") || ctrl.Field.FieldID.EndsWith("05")) && (ctrl.Field.FieldID.StartsWith("IR") || ctrl.Field.FieldID.StartsWith("AR")))
        {
          string str2 = ctrl.Field.FieldID.Substring(0, 2);
          if (this is TAX4506InputHandler)
          {
            TAX4506InputHandler x4506InputHandler = (TAX4506InputHandler) this;
            str2 += x4506InputHandler.CurrentIndex;
          }
          else if (this is TAX4506TInputHandler)
          {
            TAX4506TInputHandler x4506TinputHandler = (TAX4506TInputHandler) this;
            str2 += x4506TinputHandler.CurrentIndex;
          }
          if (ctrl.Field.FieldID.EndsWith("04") && this.loan.GetField(str2 + "57") != "Y" || ctrl.Field.FieldID.EndsWith("05") && this.loan.GetField(str2 + "58") != "Y")
          {
            flag = true;
            str1 = Utils.FormatInput(orgval, FieldFormat.SSN, ref needsUpdate);
          }
          else if (ctrl.Field.FieldID.EndsWith("04") && this.loan.GetField(str2 + "57") == "Y" || ctrl.Field.FieldID.EndsWith("05") && this.loan.GetField(str2 + "58") == "Y")
          {
            flag = true;
            str1 = str1.Replace("-", "");
            if (str1.Length > 2)
              str1 = str1.Substring(0, 2) + "-" + (str1.Length >= 9 ? str1.Substring(2, 7) : str1.Substring(2));
          }
        }
        else if (ctrl.Field.FieldID == "IRS4506.X4" || ctrl.Field.FieldID == "IRS4506.X5")
        {
          if (ctrl.Field.FieldID == "IRS4506.X4" && this.loan.GetField("IRS4506.X57") != "Y" || ctrl.Field.FieldID == "IRS4506.X5" && this.loan.GetField("IRS4506.X58") != "Y")
          {
            flag = true;
            str1 = Utils.FormatInput(orgval, FieldFormat.SSN, ref needsUpdate);
          }
          else if (ctrl.Field.FieldID == "IRS4506.X4" && this.loan.GetField("IRS4506.X57") == "Y" || ctrl.Field.FieldID == "IRS4506.X5" && this.loan.GetField("IRS4506.X58") == "Y")
          {
            flag = true;
            str1 = str1.Replace("-", "");
            if (str1.Length > 2)
              str1 = str1.Substring(0, 2) + "-" + (str1.Length >= 9 ? str1.Substring(2, 7) : str1.Substring(2));
          }
        }
        else
          flag = supportsFormatEvent.InvokeFormat(ref str1);
        needsUpdate = orgval != str1;
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) ctrl, "Format", ex);
      }
      if (flag)
      {
        this.WriteDebug("Auto-reformatting field: Control ID = '" + ctrl.ControlID + "', Original Value = '" + orgval + "'");
        if (this.loan != null && (ctrl.Field.Format == LoanFieldFormat.STATE || ctrl.Field.Format == LoanFieldFormat.ZIPCODE) && this.loan.IsForeignIndicatorSelected(this.MapFieldId(ctrl.Field.FieldID)))
        {
          str1 = orgval;
          needsUpdate = true;
        }
        else
          needsUpdate |= this.reformatInput(ctrl.Field.FieldID, ref str1);
      }
      if (!needsUpdate)
        return;
      this.WriteDebug("Re-binding field to formatted value: Control ID = '" + ctrl.ControlID + "', Original Value = '" + orgval + "', Formatted Value = '" + str1 + "'");
      ctrl.BindTo(str1);
    }

    internal virtual void FlipLockField(FieldLock fieldLock)
    {
      if (this.loan != null && this.loan.IsInFindFieldForm || !(fieldLock.ControlToLock is FieldControl controlToLock))
        return;
      string id = this.MapFieldId(controlToLock.Field.FieldID);
      switch (id)
      {
        case "":
          return;
        case "NEWHUD.X1301":
          if (this.isFieldLocked(id, controlToLock.FieldSource) && !this.confirmUSDAInLine819(true))
            return;
          break;
      }
      if (this.isFieldLocked(id, controlToLock.FieldSource))
      {
        if (this.loan != null)
          this.loan.BusinessRuleTrigger = BusinessRuleOnDemandEnum.None;
        this.removeFieldLock(id, controlToLock.FieldSource);
        if (this.loan != null)
        {
          if (controlToLock.Field.FieldID == "137" || controlToLock.Field.FieldID == "138")
            this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
          else if (controlToLock.Field.FieldID == "304")
            this.loan.Calculator.SpecialCalculation(CalculationActionID.GFEFormCalculation);
          else if (controlToLock.Field.FieldID == "137" || controlToLock.Field.FieldID == "138")
            this.loan.Calculator.FormCalculation(controlToLock.Field.FieldID, (string) null, (string) null);
          else if (controlToLock.Field.FieldID == "3043")
            this.setFieldValue("3043", controlToLock.FieldSource, this.GetFieldValue("1109", controlToLock.FieldSource));
          else if (controlToLock.Field.FieldID == "1699")
            this.loan.Calculator.FormCalculation("1699", (string) null, (string) null);
          else if (controlToLock.Field.FieldID == "NEWHUD.X217")
          {
            this.loan.RemoveLock("NEWHUD.X355");
            this.loan.RemoveLock("NEWHUD.X356");
            this.loan.RemoveLock("NEWHUD.X357");
            this.loan.Calculator.FormCalculation("608", (string) null, (string) null);
          }
          else if (controlToLock.Field.FieldID == "NEWHUD.X1301")
            this.applyFieldRules();
          else if (controlToLock.Field.FieldID == "NEWHUD.X591")
            this.loan.TriggerCalculation("NEWHUD.X594", "");
          else if (controlToLock.Field.FieldID == "643")
            this.loan.TriggerCalculation("579", "");
          else if (controlToLock.Field.FieldID == "L260")
            this.loan.TriggerCalculation("L261", "");
          else if (controlToLock.Field.FieldID == "1667")
            this.loan.TriggerCalculation("1668", "");
          else if (controlToLock.Field.FieldID == "NEWHUD.X592")
            this.loan.TriggerCalculation("NEWHUD.X595", "");
          else if (controlToLock.Field.FieldID == "NEWHUD.X593")
            this.loan.TriggerCalculation("NEWHUD.X596", "");
          else if (controlToLock.Field.FieldID == "NEWHUD.X1588")
            this.loan.TriggerCalculation("NEWHUD.X1589", "");
          else if (controlToLock.Field.FieldID == "NEWHUD.X1596")
            this.loan.TriggerCalculation("NEWHUD.X1597", "");
          else if (controlToLock.Field.FieldID == "3973")
          {
            foreach (IDisclosureTracking2015Log disclosureTracking2015Log in Session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true))
            {
              if (disclosureTracking2015Log.DisclosedForLE && disclosureTracking2015Log.IntentToProceed)
              {
                if (disclosureTracking2015Log.IntentToProceedReceivedBy != "")
                {
                  this.SetField("3973", disclosureTracking2015Log.IntentToProceedReceivedBy);
                  break;
                }
                this.SetField("3973", this.session.UserInfo.FullName + "(" + this.session.UserInfo.Userid + ")");
                break;
              }
            }
          }
          else if (controlToLock.Field.FieldID == "FV.X348")
          {
            string field1 = this.loan.GetField("FV.X347");
            if (field1 != "")
            {
              this.loan.SetField("FV.X348", field1);
            }
            else
            {
              string field2 = this.loan.GetField("FV.X345");
              if (field2 != "")
                this.loan.SetField("FV.X348", field2);
            }
          }
          else if (controlToLock.Field.FieldID == "140")
            this.loan.Calculator.FormCalculation("4493", (string) null, (string) null);
          else if (this.loan.IsTemplate)
            this.loan.Calculator.FormCalculation(controlToLock.Field.FieldID, (string) null, (string) null);
          this.loan.TriggerCalculation(id, this.GetFieldValue(id));
          if (this.loan.LinkedData != null)
            this.SyncPiggyBackField(id, controlToLock.FieldSource == FieldSource.LinkedLoan ? FieldSource.CurrentLoan : FieldSource.LinkedLoan, this.GetFieldValue(id, controlToLock.FieldSource));
          else if (id == "2207")
            this.setFieldValue("2207", controlToLock.FieldSource, this.GetFieldValue("2274", controlToLock.FieldSource));
          else if (id == "2209")
            this.setFieldValue("2209", controlToLock.FieldSource, this.GetFieldValue("2276", controlToLock.FieldSource));
          if (id == "1269")
          {
            if (!string.IsNullOrWhiteSpace(this.loan.GetField("1269")))
              this.loan.SetField("425", "Y");
            else
              this.loan.SetField("425", "N");
          }
          this.applyOnDemandBusinessRule();
        }
      }
      else
      {
        this.addFieldLock(id, controlToLock.FieldSource);
        if (this.loan != null)
        {
          bool flag1 = this.inputData.GetField("CASASRN.X141") == "Borrower";
          bool flag2 = this.inputData.GetField("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.inputData.GetField("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
          if (flag1 | flag2 && (controlToLock.Field.FieldID == "1269" || controlToLock.Field.FieldID == "1270" || controlToLock.Field.FieldID == "1271" || controlToLock.Field.FieldID == "1272" || controlToLock.Field.FieldID == "1273" || controlToLock.Field.FieldID == "1274" || controlToLock.Field.FieldID == "1613" || controlToLock.Field.FieldID == "1614" || controlToLock.Field.FieldID == "1615" || controlToLock.Field.FieldID == "1616" || controlToLock.Field.FieldID == "1617" || controlToLock.Field.FieldID == "1618"))
          {
            int num1 = 0;
            for (int index = 1269; index <= 1274; ++index)
            {
              if (this.inputData.IsLocked(string.Concat((object) index)))
                ++num1;
              if (this.inputData.IsLocked(string.Concat((object) (index + 344))))
                ++num1;
              if (num1 > 1)
                break;
            }
            if (num1 == 1)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, flag1 | flag2 ? "The system does not support temporary buydowns when the Contributor selected is Borrower." : "The System does not support temporary buydowns for loans with an application date on or after 01/30/2011. Although the Federal Reserve Board provided new model payment summary tables, its guidance does not address temporary buydowns. We have temporarily suspended the System's buydown functionality until the regulation is clarified.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
        }
        if (this.loan != null)
        {
          if (controlToLock.Field.FieldID == "NEWHUD.X217")
          {
            this.loan.AddLock("NEWHUD.X355");
            this.loan.AddLock("NEWHUD.X356");
            this.loan.AddLock("NEWHUD.X357");
          }
          else if (controlToLock.Field.FieldID == "NEWHUD.X1301")
          {
            this.applyFieldRules();
            if (this.GetField("1172") == "FarmersHomeAdministration")
              this.loan.Calculator.FormCalculation("COPYLINE819TOLINE902", (string) null, (string) null);
          }
          else if (controlToLock.Field.FieldID == "3973")
            this.SetField("3973", "");
          this.loan.TriggerCalculation(id, this.GetFieldValue(id));
        }
        if (this.loan != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
        {
          this.SyncPiggyBackField(id, controlToLock.FieldSource == FieldSource.LinkedLoan ? FieldSource.CurrentLoan : FieldSource.LinkedLoan, this.GetFieldValue(id, controlToLock.FieldSource));
          this.loan.SyncPiggyBackFiles((string[]) null, true, false, (string) null, (string) null, false);
        }
        this.RefreshContents();
      }
      if (this.loan != null && !this.loan.IsTemplate)
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      this.syncFieldLockState(fieldLock);
    }

    private bool copyToCDPage()
    {
      List<string> stringList1 = new List<string>()
      {
        "CD1.X52",
        "CD1.X53",
        "CD1.X54"
      };
      List<string> stringList2 = new List<string>()
      {
        "CD1.X55",
        "CD1.X66",
        "CD1.X67",
        "CD1.X68"
      };
      bool flag = false;
      foreach (string id in stringList2)
      {
        if (this.loan.GetField(id) == "Y")
        {
          flag = true;
          break;
        }
      }
      foreach (string id in stringList1)
      {
        if (this.loan.GetField(id) == "Y")
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        BusinessCalendar businessCalendar = this.session.SessionObjects.GetBusinessCalendar(CalendarType.RegZ);
        IDisclosureTracking2015Log tracking2015LogByType = this.loan.GetLogList().GetIDisclosureTracking2015LogByType(DisclosureTracking2015Log.DisclosureTrackingType.CD, DisclosureTracking2015Log.DisclosureTypeEnum.Initial);
        DateTime date1 = Utils.ParseDate((object) tracking2015LogByType.GetDisclosedField("CD1.X1"));
        DateTime dateTime = date1 != DateTime.MinValue ? businessCalendar.AddBusinessDays(date1, 7, true) : DateTime.MinValue;
        DateTime date2 = Utils.ParseDate((object) tracking2015LogByType.GetDisclosedField("748"));
        if (date2 > date1 && date2 <= dateTime && Utils.ParseDate((object) this.loan.GetField("748")) <= date2)
          return false;
      }
      return true;
    }

    protected virtual void SetControlState(string controlID, bool enabled)
    {
      if (this.currentForm == null || !(this.currentForm.FindControl(controlID) is RuntimeControl control))
        return;
      control.Enabled = enabled;
    }

    protected virtual void SetControlState(string controlID, bool enabled, bool visible)
    {
      if (this.currentForm == null || !(this.currentForm.FindControl(controlID) is RuntimeControl control))
        return;
      control.Enabled = enabled;
      control.Visible = visible;
    }

    private void syncFieldLockState(FieldLock fieldLock)
    {
      if (!(fieldLock.ControlToLock is FieldControl controlToLock))
        return;
      string id = this.MapFieldId(controlToLock.Field.FieldID);
      if (id == "")
        return;
      bool locked = this.isFieldLocked(id, controlToLock.FieldSource);
      controlToLock.Enabled = false;
      controlToLock.Enabled = locked;
      fieldLock.DisplayImage(locked);
    }

    private void syncCalendarLockState(EllieMae.Encompass.Forms.Calendar calendar)
    {
      if (this.MapFieldId(calendar.DateField.FieldID) == "")
        return;
      FieldLock fieldLockForField = this.getFieldLockForField(calendar.DateField.FieldID, calendar.FieldSource);
      if (fieldLockForField == null)
        return;
      calendar.Enabled = fieldLockForField.Locked;
    }

    private FieldLock getFieldLockForField(string fieldId, FieldSource fieldSource)
    {
      foreach (FieldLock fieldLock in this.currentForm.FindControlsByType(typeof (FieldLock)))
      {
        if (fieldLock.ControlToLock is FieldControl controlToLock && controlToLock.Field.FieldID == fieldId && controlToLock.FieldSource == fieldSource)
        {
          this.syncFieldLockState(fieldLock);
          return fieldLock;
        }
      }
      return (FieldLock) null;
    }

    private void updateLoanDataForContactFields(
      RxContactInfo contactInfo,
      RolodexGroup rolodexGroup,
      FieldSource fieldSource)
    {
      foreach (EllieMae.Encompass.Forms.RolodexField field in Enum.GetValues(typeof (EllieMae.Encompass.Forms.RolodexField)))
      {
        string fieldId = rolodexGroup[field];
        if (fieldId != "")
        {
          string id = this.MapFieldId(fieldId);
          switch (id)
          {
            case "contactapp":
              id = "618";
              break;
            case "contactatt":
              id = "VEND.X117";
              break;
            case "contactcre":
              id = "625";
              break;
            case "contactdoc":
              id = "VEND.X195";
              break;
            case "contactesc":
              id = "611";
              break;
            case "contactflo":
              id = "VEND.X13";
              break;
            case "contacthoi":
              id = "VEND.X162";
              break;
            case "contactmic":
              id = "707";
              break;
            case "contacttit":
              id = "416";
              break;
            case "contactund":
              id = "984";
              break;
          }
          if ((this.loan == null || !this.loan.IsLocked(id)) && (this.loan == null || !this.loan.IsFieldReadOnly(id)))
          {
            if (id == "ECOA_NAME")
              this.setFieldValue(id, fieldSource, contactInfo[EllieMae.EMLite.Common.Contact.RolodexField.Company]);
            else
              this.setFieldValue(id, fieldSource, contactInfo[(EllieMae.EMLite.Common.Contact.RolodexField) field]);
          }
        }
      }
    }

    private RolodexGroup getRolodexGroup(string fieldId)
    {
      foreach (Rolodex rolodex in this.currentForm.FindControlsByType(typeof (Rolodex)))
      {
        if (rolodex.Enabled && (rolodex.ControlID == fieldId || rolodex.Group.GetFieldMap(fieldId) != EllieMae.Encompass.Forms.RolodexField.None))
          return rolodex.Group;
      }
      return (RolodexGroup) null;
    }

    protected bool GetContactItem(string fieldId)
    {
      return this.GetContactItem(fieldId, FieldSource.CurrentLoan);
    }

    protected bool GetContactItem(string fieldId, FieldSource fieldSource)
    {
      this.FlushOutCurrentField();
      RolodexGroup rolodexGroup = this.getRolodexGroup(fieldId);
      if (rolodexGroup == null || this.loan != null && this.loan.IsFieldReadOnly(fieldId))
        return false;
      this.openRolodex(rolodexGroup, fieldId, fieldSource);
      return true;
    }

    private void openRolodex(RolodexGroup rolodexGroup, string fieldId, FieldSource fieldSource)
    {
      string category = string.Concat((object) rolodexGroup.BusinessCategory);
      RxContactInfo rxContact = new RxContactInfo();
      bool bMatchCompany = false;
      bool flag1 = false;
      bool flag2 = false;
      foreach (EllieMae.Encompass.Forms.RolodexField field in Enum.GetValues(typeof (EllieMae.Encompass.Forms.RolodexField)))
      {
        string str = rolodexGroup[field];
        if (str != "")
        {
          if (this.loan != null && this.loan.IsFieldReadOnly(str))
            flag2 = true;
          rxContact[(EllieMae.EMLite.Common.Contact.RolodexField) field] = this.GetFieldValue(this.MapFieldId(str), fieldSource);
          bMatchCompany |= field == EllieMae.Encompass.Forms.RolodexField.Company;
          flag1 |= field == EllieMae.Encompass.Forms.RolodexField.Name;
          if (fieldId == string.Empty && field == EllieMae.Encompass.Forms.RolodexField.Company)
            fieldId = str;
          else if (fieldId == string.Empty && field == EllieMae.Encompass.Forms.RolodexField.Name)
            fieldId = str;
        }
      }
      if (rxContact.CategoryName != "")
        category = rxContact.CategoryName;
      CRMRoleType crmRoleType = CRMRoleType.NotFound;
      string associateMappingID = "";
      if (fieldId != "")
      {
        string loanFieldID = this.MapFieldId(this.parseFieldID(fieldId));
        crmRoleType = ContactUtil.GetCRMRoleType(loanFieldID);
        associateMappingID = ContactUtil.getCRMMappingID(crmRoleType, loanFieldID);
      }
      else if (rolodexGroup[EllieMae.Encompass.Forms.RolodexField.Company] != "")
      {
        crmRoleType = ContactUtil.GetCRMRoleType(rolodexGroup[EllieMae.Encompass.Forms.RolodexField.Company]);
        associateMappingID = ContactUtil.getCRMMappingID(crmRoleType, rolodexGroup[EllieMae.Encompass.Forms.RolodexField.Company]);
      }
      else if (rolodexGroup[EllieMae.Encompass.Forms.RolodexField.Name] != "")
      {
        crmRoleType = ContactUtil.GetCRMRoleType(rolodexGroup[EllieMae.Encompass.Forms.RolodexField.Name]);
        associateMappingID = ContactUtil.getCRMMappingID(crmRoleType, rolodexGroup[EllieMae.Encompass.Forms.RolodexField.Name]);
      }
      RxBusinessContact rxBusinessContact;
      if (this.loan == null || this.FormIsForTemplate)
      {
        if (fieldId == "1822")
          rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, false, crmRoleType, false, RxBusinessContact.ActionMode.SelectMode, associateMappingID);
        else if (fieldId == "1264" && this is QALOInputHandler)
          rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, true, true, crmRoleType, false, RxBusinessContact.ActionMode.SelectMode, associateMappingID);
        else if (fieldId == "VEND.X184" && this is HUD1ESInputHandler)
          rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, true, true, crmRoleType, false, RxBusinessContact.ActionMode.SelectMode, associateMappingID);
        else if (fieldId == "L74" && this is HUD1PG1InputHandler)
          rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, string.Empty, rxContact, crmRoleType, false, associateMappingID);
        else if (fieldId == "VEND.X488" || fieldId == "VEND.X489" || fieldId == "VEND.X490" || fieldId == "VEND.X491" || fieldId == "VEND.X492" || fieldId == "VEND.X493" || fieldId == "VEND.X494" || fieldId == "VEND.X495" || fieldId == "VEND.X496")
        {
          rxContact.CategoryID = rolodexGroup.BusinessCategory.ID;
          rxContact.MortgageeClauseCompany = this.GetFieldValue("VEND.X488");
          rxContact.MortgageeClauseName = this.GetFieldValue("VEND.X489");
          rxContact.MortgageeClauseAddressLine = this.GetFieldValue("VEND.X490");
          rxContact.MortgageeClauseCity = this.GetFieldValue("VEND.X491");
          rxContact.MortgageeClauseState = this.GetFieldValue("VEND.X492");
          rxContact.MortgageeClauseZipCode = this.GetFieldValue("VEND.X493");
          rxContact.MortgageeClausePhone = this.GetFieldValue("VEND.X494");
          rxContact.MortgageeClauseFax = this.GetFieldValue("VEND.X495");
          rxContact.MortgageeClauseText = this.GetFieldValue("VEND.X496");
          rxBusinessContact = new RxBusinessContact(category, rxContact.MortgageeClauseCompany, rxContact, crmRoleType, false, RxBusinessContact.ActionMode.SelectMode, associateMappingID);
        }
        else
          rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, bMatchCompany, false, !flag1, crmRoleType, false, RxBusinessContact.ActionMode.SelectMode, associateMappingID);
      }
      else if (fieldId == "1822")
        rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, false, crmRoleType, true, associateMappingID);
      else if (fieldId == "1264" && this is QALOInputHandler)
        rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, true, true, crmRoleType, true, associateMappingID);
      else if (fieldId == "VEND.X184" && this is HUD1ESInputHandler)
        rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, true, true, crmRoleType, true, associateMappingID);
      else if (fieldId == "L74" && this is HUD1PG1InputHandler)
        rxBusinessContact = new RxBusinessContact(category, rxContact.CompanyName, string.Empty, rxContact, crmRoleType, true, associateMappingID);
      else if (fieldId == "VEND.X488" || fieldId == "VEND.X489" || fieldId == "VEND.X490" || fieldId == "VEND.X491" || fieldId == "VEND.X492" || fieldId == "VEND.X493" || fieldId == "VEND.X494" || fieldId == "VEND.X495" || fieldId == "VEND.X496")
      {
        rxContact.CategoryID = rolodexGroup.BusinessCategory.ID;
        rxContact.MortgageeClauseCompany = this.GetFieldValue("VEND.X488");
        rxContact.MortgageeClauseName = this.GetFieldValue("VEND.X489");
        rxContact.MortgageeClauseAddressLine = this.GetFieldValue("VEND.X490");
        rxContact.MortgageeClauseCity = this.GetFieldValue("VEND.X491");
        rxContact.MortgageeClauseState = this.GetFieldValue("VEND.X492");
        rxContact.MortgageeClauseZipCode = this.GetFieldValue("VEND.X493");
        rxContact.MortgageeClausePhone = this.GetFieldValue("VEND.X494");
        rxContact.MortgageeClauseFax = this.GetFieldValue("VEND.X495");
        rxContact.MortgageeClauseText = this.GetFieldValue("VEND.X496");
        rxBusinessContact = new RxBusinessContact(category, rxContact.MortgageeClauseCompany, rxContact, crmRoleType, true, associateMappingID);
      }
      else
        rxBusinessContact = this.loan == null || this.loan.IsInFindFieldForm || !(fieldId == "REQUEST.X29") || !(this.GetField("REQUEST.X25") != string.Empty) ? (this.loan == null || this.loan.IsInFindFieldForm ? new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, bMatchCompany, false, !flag1, crmRoleType, true, RxBusinessContact.ActionMode.SelectMode, associateMappingID) : new RxBusinessContact(category, rxContact.CompanyName, rxContact.LastName, rxContact, bMatchCompany, !flag1, crmRoleType, false, associateMappingID)) : new RxBusinessContact(this.GetField("REQUEST.X25"), rxContact.CompanyName, rxContact.LastName, rxContact, bMatchCompany, !flag1, crmRoleType, false, associateMappingID);
      if (flag2)
        rxBusinessContact.SetReadOnlyMode();
      if (rxBusinessContact.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      if (rxBusinessContact.GoToContact)
      {
        this.session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
      }
      else
      {
        switch (fieldId)
        {
          case "4673":
            this.SetField("4672", "");
            break;
          case "1612":
            this.SetField("4802", "");
            break;
          case "1256":
          case "1264":
            this.SetField("4806", "");
            break;
          case "VEND.X302":
          case "VEND.X293":
            this.SetField("4809", "");
            break;
          case "USDA.X31":
            this.SetField("4811", "");
            break;
          case "1754":
            this.SetField("4814", "");
            break;
          case "3194":
            this.SetField("4818", "");
            break;
          case "NOTICES.X31":
          case "NOTICES.X32":
            this.SetField("4824", "");
            break;
        }
        RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
        this.updateLoanDataForContactFields(rxContactRecord, rolodexGroup, fieldSource);
        if (fieldId == "L74" && this is HUD1PG1InputHandler || fieldId == "VEND.X655" && this is FileContactInputHandler)
        {
          string str1 = rxContactRecord.BizAddress1.Trim();
          string str2 = str1 + (str1 == string.Empty ? "" : ",") + rxContactRecord.BizCity.Trim();
          string str3 = str2 + (str2 == string.Empty ? "" : ",") + rxContactRecord.BizState.Trim();
          this.setFieldValue("L75", FieldSource.CurrentLoan, str3 + (str3 == string.Empty ? "" : " ") + rxContactRecord.BizZip);
          this.setFieldValue("L74", FieldSource.CurrentLoan, (rxContactRecord.CompanyName == "" ? "" : rxContactRecord.CompanyName + "\r\n") + (!(rxContactRecord.FirstName == "") || !(rxContactRecord.LastName == "") ? rxContactRecord.FirstName + " " + rxContactRecord.LastName + "\r\n" : "") + rxContactRecord.WorkPhone);
          if (fieldId == "VEND.X655")
          {
            this.setFieldValue("VEND.X1040", FieldSource.CurrentLoan, rxContactRecord.WebSite);
            this.setFieldValue("VEND.X1041", FieldSource.CurrentLoan, rxContactRecord.WebSite);
          }
        }
        else if (fieldId == "REQUEST.X29" && this is REQUESTSInputHandler)
        {
          this.setFieldValue("REQUEST.X25", FieldSource.CurrentLoan, rxContactRecord.CategoryName);
        }
        else
        {
          switch (fieldId)
          {
            case "1264":
              this.setFieldValue("1262", FieldSource.CurrentLoan, rxContactRecord.WorkPhone);
              this.setFieldValue("VEND.X1034", FieldSource.CurrentLoan, rxContactRecord.WebSite);
              this.setFieldValue("VEND.X1035", FieldSource.CurrentLoan, rxContactRecord.WebSite);
              break;
            case "VEND.X293":
              this.setFieldValue("VEND.X1036", FieldSource.CurrentLoan, rxContactRecord.WebSite);
              this.setFieldValue("VEND.X1037", FieldSource.CurrentLoan, rxContactRecord.WebSite);
              break;
            case "VEND.X144":
              this.setFieldValue("VEND.X1038", FieldSource.CurrentLoan, rxContactRecord.WebSite);
              this.setFieldValue("VEND.X1039", FieldSource.CurrentLoan, rxContactRecord.WebSite);
              break;
            case "DISCLOSURE.X1106":
              this.setBizContactJobTitle("DISCLOSURE.X1030", rxContactRecord.ContactID);
              break;
            case "DISCLOSURE.X1033":
              this.setBizContactJobTitle("DISCLOSURE.X1034", rxContactRecord.ContactID);
              break;
            case "NEWHUD.X202":
              this.setFieldValue("411", FieldSource.CurrentLoan, rxContactRecord.CompanyName);
              break;
            case "411":
              this.setFieldValue("NEWHUD.X202", FieldSource.CurrentLoan, rxContactRecord.CompanyName);
              break;
            case "CD5.X31":
              this.loan.Calculator.FormCalculation("COPYFROMREALESTATEBROKER(B)", (string) null, (string) null);
              break;
            case "CD5.X43":
              this.loan.Calculator.FormCalculation("COPYFROMREALESTATEBROKER(S)", (string) null, (string) null);
              break;
            case "CD5.X55":
              this.loan.Calculator.FormCalculation("SETTLEMENTAGENT", (string) null, (string) null);
              break;
          }
        }
        this.updateLoanDataFromCategoryCustomFields(rxContactRecord, rolodexGroup, fieldSource);
        this.UpdateContents(false, true, false);
        this.updateCategoryCustomFieldsFromLoanData(rxContactRecord, rolodexGroup, fieldSource);
        switch (this)
        {
          case VOEInputHandler _:
            ((VOEInputHandler) this).SummaryChanged("FE0002", rxContactRecord.CompanyName);
            break;
          case VODInputHandler _:
            ((VODInputHandler) this).SummaryChanged("DD0002", rxContactRecord.CompanyName);
            break;
          case VOGGInputHandler _:
            ((VOGGInputHandler) this).SummaryChanged("URLARGG0005", rxContactRecord.CompanyName);
            break;
          case VOOIInputHandler _:
            ((VOOIInputHandler) this).SummaryChanged("URLAROIS0005", rxContactRecord.CompanyName);
            break;
          case VOOLInputHandler _:
            ((VOGGInputHandler) this).SummaryChanged("URLAROL0005", rxContactRecord.CompanyName);
            break;
          case VOOAInputHandler _:
            ((VOOAInputHandler) this).SummaryChanged("URLAROA0005", rxContactRecord.CompanyName);
            break;
          case VOLInputHandler _:
            ((VOLInputHandler) this).SummaryChanged("FL0002", rxContactRecord.CompanyName);
            break;
          case VOALInputHandler _:
            ((VOALInputHandler) this).SummaryChanged("URLARAL0002", rxContactRecord.CompanyName);
            break;
          case FileContactInputHandler _:
            ((FileContactInputHandler) this).ContactChanged(rxContactRecord);
            break;
          case SETTLEMENTSERVICEPROVIDERInputHandler _:
            SETTLEMENTSERVICEPROVIDERInputHandler settlementserviceproviderInputHandler = (SETTLEMENTSERVICEPROVIDERInputHandler) this;
            settlementserviceproviderInputHandler.SummaryChanged("SP0001", rxContactRecord.CategoryName);
            settlementserviceproviderInputHandler.SummaryChanged("SP0002", rxContactRecord.CompanyName);
            settlementserviceproviderInputHandler.SummaryChanged("SP0003", rxContactRecord.BizAddress1 + (rxContactRecord.BizAddress2 != string.Empty ? ", " : "") + rxContactRecord.BizAddress2);
            settlementserviceproviderInputHandler.SummaryChanged("SP0004", rxContactRecord.BizCity);
            settlementserviceproviderInputHandler.SummaryChanged("SP0005", rxContactRecord.BizState);
            break;
          case HOMECOUNSELINGPROVIDERSInputHandler _:
            HOMECOUNSELINGPROVIDERSInputHandler homecounselingprovidersInputHandler = (HOMECOUNSELINGPROVIDERSInputHandler) this;
            homecounselingprovidersInputHandler.SummaryChanged("HC0002", rxContactRecord.CompanyName);
            homecounselingprovidersInputHandler.SummaryChanged("HC0003", HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(rxContactRecord.BizAddress1 + " " + rxContactRecord.BizAddress2, rxContactRecord.BizCity, rxContactRecord.BizState, rxContactRecord.BizZip));
            homecounselingprovidersInputHandler.SummaryChanged("HC0007", rxContactRecord.WorkPhone);
            break;
          case AFFILIATEDBAInputHandler _:
            ((AFFILIATEDBAInputHandler) this).SummaryChanged(fieldId, rxContactRecord.CompanyName);
            break;
          case LOANESTIMATEPAGE3InputHandler _:
            this.UpdateLenderInfo(rxContactRecord, fieldSource);
            this.RefreshContents();
            break;
        }
        if (this.OnFieldChanged == null)
          return;
        this.OnFieldChanged((object) fieldId, new EventArgs());
      }
    }

    private void UpdateLenderInfo(RxContactInfo newRxContactInfo, FieldSource fieldSource)
    {
      this.setFieldValue("1264", fieldSource, newRxContactInfo.CompanyName);
      this.setFieldValue("1257", fieldSource, newRxContactInfo.BizAddress1 + "," + newRxContactInfo.BizAddress2);
      this.setFieldValue("1258", fieldSource, newRxContactInfo.BizCity);
      this.setFieldValue("1259", fieldSource, newRxContactInfo.BizState);
      this.setFieldValue("1260", fieldSource, newRxContactInfo.BizZip);
      this.setFieldValue("3032", fieldSource, newRxContactInfo.LicenseNumber);
      this.setFieldValue("VEND.X681", fieldSource, newRxContactInfo.CompanyLicAuthName);
      this.setFieldValue("VEND.X682", fieldSource, newRxContactInfo.CompanyLicAuthType);
      this.setFieldValue("VEND.X683", fieldSource, newRxContactInfo.CompanyLicAuthStateCode);
      this.setFieldValue("VEND.X684", fieldSource, newRxContactInfo.CompanyLicIssDate);
      this.setFieldValue("VEND.X686", fieldSource, newRxContactInfo.ContactLicAuthName);
      this.setFieldValue("VEND.X687", fieldSource, newRxContactInfo.ContactLicAuthType);
      this.setFieldValue("VEND.X688", fieldSource, newRxContactInfo.ContactLicAuthStateCode);
      this.setFieldValue("VEND.X689", fieldSource, newRxContactInfo.ContactLicIssDate);
      this.setFieldValue("VEND.X685", fieldSource, newRxContactInfo.ContactLicNo);
      this.setFieldValue("1256", fieldSource, newRxContactInfo.FirstName + " " + newRxContactInfo.LastName);
      this.setFieldValue("1262", fieldSource, newRxContactInfo.WorkPhone);
      this.setFieldValue("95", fieldSource, newRxContactInfo.BizEmail);
      this.setFieldValue("1263", fieldSource, newRxContactInfo.FaxNumber);
      this.setFieldValue("VEND.X497", fieldSource, newRxContactInfo.CellPhone);
      this.setFieldValue("VEND.X1034", fieldSource, newRxContactInfo.WebSite);
      this.setFieldValue("VEND.X1035", fieldSource, newRxContactInfo.WebSite);
    }

    private void updateLoanDataFromCategoryCustomFields(
      RxContactInfo rxContact,
      RolodexGroup rolodexGroup,
      FieldSource fieldSource)
    {
      CustomFieldMappingCollection mappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(this.session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.BizCategoryCustom, rxContact.CategoryID, false));
      if (mappingCollection.Count == 0)
        return;
      CustomFieldValueCollection fieldValueCollection = CustomFieldValueCollection.GetCustomFieldValueCollection(this.session.SessionObjects, new CustomFieldValueCollection.Criteria(rxContact.ContactID, rxContact.CategoryID));
      if (fieldValueCollection.Count == 0)
        return;
      foreach (CustomFieldMapping customFieldMapping in (CollectionBase) mappingCollection)
      {
        CustomFieldValue customFieldValue = fieldValueCollection.Find(customFieldMapping.RecordId);
        if (customFieldValue != null)
        {
          try
          {
            this.setFieldValue(customFieldMapping.LoanFieldId, fieldSource, customFieldValue.FieldValue);
          }
          catch (Exception ex)
          {
            Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Business Category '{0} - Custom Field {1}', Value '{2}' to Loan Field ID '{3}' failed.", (object) rxContact.CategoryName, (object) customFieldMapping.FieldNumber.ToString(), (object) customFieldValue, (object) customFieldMapping.LoanFieldId));
          }
        }
      }
    }

    private void updateCategoryCustomFieldsFromLoanData(
      RxContactInfo rxContact,
      RolodexGroup rolodexGroup,
      FieldSource fieldSource)
    {
      CustomFieldMappingCollection mappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(this.session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.BizCategoryCustom, rxContact.CategoryID, true));
      if (mappingCollection.Count == 0)
        return;
      CustomFieldValueCollection fieldValueCollection = CustomFieldValueCollection.NewCustomFieldValueCollection(this.session.SessionObjects, new CustomFieldValueCollection.Criteria(rxContact.ContactID, rxContact.CategoryID));
      foreach (CustomFieldMapping customFieldMapping in (CollectionBase) mappingCollection)
      {
        string str1 = (string) null;
        string str2;
        try
        {
          str2 = this.inputData.GetField(customFieldMapping.LoanFieldId);
        }
        catch (Exception ex)
        {
          Tracing.Log(InputHandlerBase.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{0}', Value '{1}' to Business Category '{0} - Custom Field {1}' failed.", (object) customFieldMapping.LoanFieldId, str1 == null ? (object) "UNKNOWN" : (object) str1, (object) rxContact.CategoryName, (object) customFieldMapping.FieldNumber.ToString()));
          str2 = (string) null;
        }
        if (str2 != null)
        {
          CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(customFieldMapping.RecordId, rxContact.ContactID, customFieldMapping.FieldFormat);
          customFieldValue.FieldValue = str2;
          fieldValueCollection.Add(customFieldValue);
        }
      }
      if (0 >= fieldValueCollection.Count)
        return;
      fieldValueCollection.Save();
    }

    private void setBizContactJobTitle(string jobTitleFieldID, int contactID)
    {
      BizPartnerInfo bizPartner = this.session.ContactManager.GetBizPartner(contactID);
      if (bizPartner != null)
        this.setFieldValue(jobTitleFieldID, FieldSource.CurrentLoan, bizPartner.JobTitle);
      else
        this.setFieldValue(jobTitleFieldID, FieldSource.CurrentLoan, "");
    }

    internal bool StatePermission(string stateName, bool clearFields)
    {
      return this.StatePermission(stateName, FieldSource.CurrentLoan, clearFields);
    }

    internal bool StatePermission(string stateName, FieldSource fieldSource, bool clearFields)
    {
      if (stateName.Trim() == "")
        return true;
      bool flag = false;
      string fieldValue = this.GetFieldValue("LOID", fieldSource);
      if (fieldValue == string.Empty)
        return true;
      LOLicenseInfo loLicense = this.session.OrganizationManager.GetLOLicense(fieldValue, stateName);
      if (loLicense != null && loLicense.Enabled && DateTime.Today.Date.CompareTo(loLicense.ExpirationDate) <= 0)
      {
        if ((loLicense.License ?? "") != string.Empty)
          this.inputData.SetCurrentField("2306", loLicense.License);
        else
          this.inputData.SetCurrentField("2306", "");
        flag = true;
      }
      if (!flag)
      {
        if (clearFields)
        {
          this.setFieldValue("14", fieldSource, "");
          this.setFieldValue("12", fieldSource, "");
          this.setFieldValue("15", fieldSource, "");
        }
        int num = (int) Utils.Dialog((IWin32Window) null, "You have selected a state in which the loan officer is not licensed to originate loans or the license expiration date is expired. Contact your system administrator for more details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return flag;
    }

    internal virtual void UpdateFieldValue(string id, string val)
    {
      if (this.loan != null)
      {
        this.loan.BusinessRuleTrigger = BusinessRuleOnDemandEnum.None;
        if (EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic && this.loan.Calculator != null)
          this.loan.Calculator.ResetTriggerCounter();
      }
      this.UpdateFieldValue(id, val, false);
      if (this.OnFieldChanged != null)
        this.OnFieldChanged((object) id, new EventArgs());
      this.applyOnDemandBusinessRule();
      this.IsAnyFieldTouched = true;
      if (!EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic || this.loan.Calculator == null)
        return;
      Tracing.Log(true, "Calculation", "Calculation Diagnostic", "Calculation Trigger Counts - FieldId: " + id + " | Value: " + val + "\r\n" + this.loan.Calculator.DumpTriggerCounter());
    }

    internal virtual void UpdateFieldValue(string id, string val, bool skipSyncPiggy)
    {
      string field = this.GetField(id);
      FieldSource fieldSource = FieldSource.CurrentLoan;
      if (this.currentField != null)
        fieldSource = this.currentField.FieldSource;
      if (this.GetFieldFormat(id) == FieldFormat.RA_STRING)
      {
        if (val.StartsWith("$"))
          val = val.Substring(1);
        if (!Utils.ValidateRAString(val, (List<string>) null))
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The value \"" + val + "\" is not in correct format.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      DialogResult dialogResult = DialogResult.No;
      if ((id == "19" && field == "ConstructionOnly" && val != "ConstructionOnly" || id == "4084" && field == "Y" && val != "Y") && this.loan != null && this.loan.LinkedData != null && this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary)
      {
        if (this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true).Length != 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "You cannot change the loan purpose for linked loan files in a construction-to-permanent transaction after disclosures have been ordered.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        dialogResult = Utils.Dialog((IWin32Window) this.session.MainForm, "Changing the loan purpose will remove the link between the two loan files for your construction-to-permanent transaction. Both loans will be saved after the link is removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
        if (dialogResult != DialogResult.Yes || !Session.LoanDataMgr.LinkedLoan.LockLoanWithExclusiveA(true, (string) null, false) || !Session.LoanDataMgr.LockLoanWithExclusiveA(true, (string) null, false))
          return;
        if (this.GetField("HMDA.X24") == "Y")
        {
          if (DialogResult.OK != Utils.Dialog((IWin32Window) this.session.MainForm, "Removing this link will no longer exclude the Construction loan from HMDA reporting. Do you wish to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
            return;
          this.inputData.SetField("HMDA.X24", "");
        }
      }
      if (id == "VASUMM.X132" && Utils.ParseDouble((object) this.inputData.GetField("961")) < Utils.ParseDouble((object) val) || id == "961" && this.isFieldLocked("VASUMM.X132", FieldSource.CurrentLoan) && Utils.ParseDouble((object) this.inputData.GetField("VASUMM.X132")) > Utils.ParseDouble((object) val))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The Energy Efficient Improvements Financed Amount cannot exceed Energy Efficient Improvements Amount!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (id == "4254" || id == "4255")
        {
          if (id == "4254" && val == "Y")
            this.inputData.SetField("4255", "N");
          else if (id == "4255" && val == "Y")
            this.inputData.SetField("4254", "N");
        }
        if (id == "FL" + this.Property.ToString() + "29" || id == "4494" || id == "420")
        {
          string empty = string.Empty;
          string str1;
          if (id == "420")
          {
            switch (val)
            {
              case "FirstLien":
                str1 = "1";
                break;
              case "SecondLien":
                str1 = "2";
                break;
              default:
                str1 = this.loan == null || this.loan.LinkedData == null || this.loan.LinkSyncType != LinkSyncType.PiggybackLinked && this.loan.LinkSyncType != LinkSyncType.PiggybackPrimary ? string.Empty : "1";
                break;
            }
          }
          else
            str1 = val;
          if (!string.IsNullOrEmpty(str1) && this.loan != null)
          {
            if (id == "FL" + this.Property.ToString() + "29" && (this.GetField("4494") == str1 || this.loan != null && this.loan.LinkedData != null && this.loan.LinkedData.GetField("4494") == str1))
            {
              int num2 = (int) Utils.Dialog((IWin32Window) null, "A different lien is already in this planned position. Only one lien can be in each position. Please select a different position for this lien.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
            for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
            {
              string str2 = index.ToString("00");
              if (!(str2 == this.Property.ToString()) && this.GetField("FL" + str2 + "29") == str1)
              {
                int num3 = (int) Utils.Dialog((IWin32Window) null, "A different lien is already in this planned position. Only one lien can be in each position. Please select a different position for this lien.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
            }
          }
        }
        if ((id == "PAYMENTTABLE.CUSTOMIZE" || id == "LOANTERMTABLE.CUSTOMIZE") && val != "Y" && Utils.Dialog((IWin32Window) this.session.MainForm, "All custom data and changes made to the " + (id == "PAYMENTTABLE.CUSTOMIZE" ? "Projected Payments Table" : "Loan Term Table") + " will be removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        if ((id == "VEND.X1034" || id == "VEND.X1035" || id == "VEND.X1036" || id == "VEND.X1037" || id == "VEND.X1038" || id == "VEND.X1039" || id == "VEND.X1040" || id == "VEND.X1041") && val != "" && !SystemUtil.IsValidURL(val))
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Not a valid URL for field - " + id, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (id == "FV.X397")
          {
            if (new PayoffsAndPaymentsDialog(this.loan, this.mainScreen)
            {
              ByPassCopyCondition = true,
              PocOrgAmount = field.Replace(",", ""),
              PocNewAmount = Utils.ApplyFieldFormatting(val, FieldFormat.DECIMAL_2).Replace(",", "")
            }.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
              return;
            if (this.loan != null && this.loan.Calculator != null)
              this.loan.Calculator.FormCalculation("CD3.X139", (string) null, (string) null);
          }
          if (id == "3197")
          {
            if (this.GetField("3164") == "Y")
            {
              if (Utils.IsDate((object) val))
              {
                DateTime borrowerReceivedDate = this.loan.Calculator.GetInitialBorrowerReceivedDate();
                if (borrowerReceivedDate == DateTime.MinValue)
                {
                  int num5 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Field 3197 cannot be configured if initial borrower received date is empty.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  val = "";
                }
                else if (borrowerReceivedDate > Utils.ParseDate((object) val))
                {
                  int num6 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Date value for field 3197 cannot be earlier than the initial borrower received date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  if (!this.setFieldValue(id, fieldSource, val ?? ""))
                    throw new FieldValidationException(id, val, "The value '" + val + "' is invalid for field '" + id + "'");
                  val = field;
                }
              }
              else
              {
                int num7 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Date value for field 3197 cannot be earlier than the initial borrower received date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                val = field;
              }
            }
          }
          else if (id == "LCP.X9" || id == "LCP.X10")
          {
            if (val != string.Empty && (id == "LCP.X9" && Utils.ParseDouble((object) this.GetField("LCP.X10")) > 0.0 && Utils.ParseDouble((object) val) > Utils.ParseDouble((object) this.GetField("LCP.X10")) || id == "LCP.X10" && Utils.ParseDouble((object) this.GetField("LCP.X9")) > 0.0 && Utils.ParseDouble((object) val) < Utils.ParseDouble((object) this.GetField("LCP.X9"))))
            {
              int num8 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The LO Comp Minimum value cannot be greater than Maximum value.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else if (id == "LCP.X28" || id == "LCP.X29")
          {
            if (val != string.Empty && (id == "LCP.X28" && Utils.ParseDouble((object) this.GetField("LCP.X29")) > 0.0 && Utils.ParseDouble((object) val) > Utils.ParseDouble((object) this.GetField("LCP.X29")) || id == "LCP.X29" && Utils.ParseDouble((object) this.GetField("LCP.X28")) > 0.0 && Utils.ParseDouble((object) val) < Utils.ParseDouble((object) this.GetField("LCP.X28"))))
            {
              int num9 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The LO Comp Minimum value cannot be greater than Maximum value.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else if (id == "1172" && field != val)
          {
            string[] strArray = new string[11]
            {
              "1107",
              "1199",
              "1198",
              "1201",
              "1200",
              "1205",
              "3531",
              "3532",
              "3533",
              "1209",
              "2978"
            };
            string str = string.Empty;
            for (int index = 0; index < strArray.Length - (val != "FHA" ? 2 : 0); ++index)
            {
              if ((strArray[index] == "3531" || strArray[index] == "3532" || strArray[index] == "3533") && this.GetField(strArray[index]) == "Y")
                str = str + (str != string.Empty ? "," : "") + strArray[index];
              else if (this.GetField(strArray[index]) != string.Empty && this.GetField(strArray[index]) != "0.000000")
                str = str + (str != string.Empty ? "," : "") + strArray[index];
            }
            if (str != string.Empty)
            {
              if (Utils.Dialog((IWin32Window) Session.MainForm, "Changing the loan type will result in the Mortgage Insurance Factors and Terms being removed from the MI Details pop-up window. Click OK to proceed.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
                return;
              if (this.inputData is LoanProgram)
                ((LoanProgram) this.inputData).ClearMIPData();
            }
          }
          else if (id == "761" && this.session.EncompassEdition == EncompassEdition.Broker)
            this.UpdateFieldValue("3253", val);
          else if (id == "3164" && val == "N" && Utils.ParseDate((object) this.GetField("LE1.X28")) != DateTime.MinValue)
          {
            int num10 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "This action will result in a change to the Closing Costs Estimate Expiration date (LE1.X28). Please verify the Closing Costs Estimate Expiration date before disclosing.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else if (this.loan != null && (id == "19" || id == "1176" || id == "608"))
          {
            string str3 = id == "19" ? val : this.GetField("19");
            string str4 = id == "1176" ? val : this.GetField("1176");
            string str5 = id == "608" ? val : this.GetField("608");
            if (Utils.ParseInt((object) str4, 0) > 24 && str3 == "ConstructionOnly" && str5 == "AdjustableRate")
            {
              int num11 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The Projected Payments Table is not calculated because Construction Only loans greater than 24 months are not supported.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            if (id == "608" && Utils.CheckIf2015RespaTila(this.GetField("3969")) && val == "OtherAmortizationType")
            {
              int num12 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The Other Amortization type is not supported through Integrated Mortgage Disclosures.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (id == "19" && field != val && (val == "NoCash-Out Refinance" || val == "Purchase" || val == "Cash-Out Refinance" || val == "ConstructionOnly" || val == "ConstructionToPermanent"))
            {
              this.inputData.SetField("PAYMENTTABLE.CUSTOMIZE", "");
              this.inputData.SetField("LOANTERMTABLE.CUSTOMIZE", "");
            }
          }
          else if (this.loan != null && id == "4084" && val == "Y" && this.GetField("19") == "ConstructionToPermanent")
          {
            this.inputData.SetField("PAYMENTTABLE.CUSTOMIZE", "");
            this.inputData.SetField("LOANTERMTABLE.CUSTOMIZE", "");
          }
          else if (id.StartsWith("IR") && id.Length == 6 && id.EndsWith("93") && val == "8821" && this.inputData.GetSimpleField(id.Substring(0, 4) + "01") == "Both")
          {
            int num13 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "IRS Form 8821 may only be used for an Individual Taxpayer. Please select Borrower, Co-Borrower or Other as applicable.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (!this.setFieldValue(id, fieldSource, val ?? ""))
            throw new FieldValidationException(id, val, "The value '" + val + "' is invalid for field '" + id + "'");
          if (dialogResult == DialogResult.Yes)
            this.RemoveLinkedLoan(false);
          if (id == "CASASRN.X141")
          {
            if (field != "" && field != val && (field == "Borrower" || val == "Borrower"))
              this.inputData.SetCurrentField("4645", "");
            bool flag1 = val == "Borrower";
            bool flag2 = this.inputData.GetField("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.inputData.GetField("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
            if (flag1 | flag2)
            {
              int num14 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, flag1 | flag2 ? "The system does not support temporary buydowns when the Contributor selected is Borrower." : "The System does not support temporary buydowns for loans with an application date on or after 01/30/2011. Although the Federal Reserve Board provided new model payment summary tables, its guidance does not address temporary buydowns. We have temporarily suspended the System's buydown functionality until the regulation is clarified.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
          if (this.inputData is LoanProgram)
          {
            if (id == "CD4.X43" && val == "none" && (this.inputData.GetField("CD4.X3") == "apply partial payment" || this.inputData.GetField("CD4.X42") == "hold until complete amount"))
            {
              this.inputData.SetField("CD4.X3", "");
              this.inputData.SetField("CD4.X42", "");
            }
            else
            {
              if (!(this.inputData.GetField("CD4.X43") == "none") || (!(id == "CD4.X3") || !(val == "apply partial payment")) && (!(id == "CD4.X42") || !(val == "hold until complete amount")))
                return;
              this.inputData.SetField("CD4.X43", "");
            }
          }
          else
          {
            this.loCompensationSetting.SetDefaultPaidBy(this.inputData, id, val);
            this.UpdateDocTrackingForVerifs(id, false);
            if (!skipSyncPiggy && this.loan != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
              this.loan.SyncPiggyBackFiles((string[]) null, true, false, id, val, fieldSource != FieldSource.CurrentLoan);
            if (id == "232" && this.GetField("1172") != "FarmersHomeAdministration" || id == "NEWHUD.X1707" && this.GetField("1172") != "FarmersHomeAdministration")
            {
              if (this.loan != null && !this.loan.IsLocked("232"))
                this.UpdateFieldValue("1199", "");
              this.UpdateFieldValue("USEREGZMI", "");
              this.UpdateFieldValue("1766", "");
            }
            else
            {
              switch (id)
              {
                case "NEWHUD.X572":
                  this.UpdateFieldValue("2010TITLE_TABLE", "");
                  break;
                case "NEWHUD.X639":
                  this.UpdateFieldValue("TITLE_TABLE", "");
                  break;
                case "NEWHUD.X808":
                  this.UpdateFieldValue("ESCROW_TABLE", "");
                  break;
                default:
                  if ((id == "1109" || id == "136" || id == "1771" || id == "1335") && this.loan != null)
                  {
                    this.loan.Calculator.PopulateFeeList(id, true);
                    break;
                  }
                  if (id == "NEWHUD.X1718" && val == "Y" && this.loan != null)
                  {
                    this.loan.Calculator.FormCalculation("COPYLOCOMPTO2010ITEMIZATION", id, val);
                    break;
                  }
                  if ((id == "2626" || id == "LCP.X1" || this.session.LoanDataMgr != null && this.session.LoanDataMgr.SystemConfiguration.LoanCompDefaultPlan != null && id == this.session.LoanDataMgr.SystemConfiguration.LoanCompDefaultPlan.TriggerField) && this.GetField("LCP.X19") == string.Empty)
                  {
                    this.updateLOCompensation(id, val);
                    break;
                  }
                  switch (id)
                  {
                    case "1959":
                      this.inputData.SetCurrentField("LE2.X96", this.inputData.GetField("1959"));
                      break;
                    case "LE2.X96":
                      this.inputData.SetCurrentField("1959", this.inputData.GetField("LE2.X96"));
                      this.loan.Calculator.FormCalculation("1959");
                      break;
                  }
                  break;
              }
            }
            if (id == "2626" && this.loan != null)
              this.updateTPOAndClosingVendorInformation(field, val);
            if (id == "11" || id == "12" || id == "14" || id == "4000" || id == "4002" || id == "4004" || id == "4006" || id == "65" || id == "66" || id == "1490" || id == "1240" || id == "1178" || id == "97" || id == "98" || id == "1480" || id == "1268" || id == "1179")
            {
              EllieMae.EMLite.ClientServer.Address address1 = new EllieMae.EMLite.ClientServer.Address(this.GetField("11"), "", this.GetField("12"), this.GetField("14"), this.GetField("15"));
              List<LoanDuplicateChecker> loanduplicates = new List<LoanDuplicateChecker>();
              string duplicateLoanCheck = Session.LoanManager.GetAllIncludeInDuplicateLoanCheck();
              bool flag3 = false;
              if (this.session.ServerManager.GetServerSetting("Components.DuplicateLoanCheck", false) != null)
                flag3 = Session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckGlobal;
              if (((this.loan == null ? 0 : (!this.loan.IsTemplate ? 1 : 0)) & (flag3 ? 1 : 0)) != 0 && Session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckLoanOnly)
              {
                Dictionary<LoanDuplicateChecker.CheckField, string> info1 = new Dictionary<LoanDuplicateChecker.CheckField, string>();
                info1.Add(LoanDuplicateChecker.CheckField.FirstName, this.GetField("4000"));
                info1.Add(LoanDuplicateChecker.CheckField.LastName, this.GetField("4002"));
                info1.Add(LoanDuplicateChecker.CheckField.SSN, this.GetField("65"));
                info1.Add(LoanDuplicateChecker.CheckField.HomePhone, this.GetField("66"));
                info1.Add(LoanDuplicateChecker.CheckField.MobilePhone, this.GetField("1490"));
                info1.Add(LoanDuplicateChecker.CheckField.Email, this.GetField("1240"));
                info1.Add(LoanDuplicateChecker.CheckField.WorkEmail, this.GetField("1178"));
                Dictionary<LoanDuplicateChecker.CheckField, string> info2 = new Dictionary<LoanDuplicateChecker.CheckField, string>();
                info2.Add(LoanDuplicateChecker.CheckField.FirstName, this.GetField("4004"));
                info2.Add(LoanDuplicateChecker.CheckField.LastName, this.GetField("4006"));
                info2.Add(LoanDuplicateChecker.CheckField.SSN, this.GetField("97"));
                info2.Add(LoanDuplicateChecker.CheckField.HomePhone, this.GetField("98"));
                info2.Add(LoanDuplicateChecker.CheckField.MobilePhone, this.GetField("1480"));
                info2.Add(LoanDuplicateChecker.CheckField.Email, this.GetField("1268"));
                info2.Add(LoanDuplicateChecker.CheckField.WorkEmail, this.GetField("1179"));
                bool flag4 = InputHandlerBase.canCheckDuplicate(info1, address1);
                bool flag5 = InputHandlerBase.canCheckDuplicate(info2, address1);
                if ((id == "11" || id == "12" || id == "14") && address1.Street1 != "TBD" && address1.Zip != "")
                {
                  List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo = new List<Dictionary<LoanDuplicateChecker.CheckField, string>>();
                  if (flag4)
                    borrowerInfo.Add(info1);
                  if (flag5)
                    borrowerInfo.Add(info2);
                  if (borrowerInfo.Count > 0)
                    loanduplicates = this.session.ConfigurationManager.GetLoanDuplicateInfo(this.loan.GUID, borrowerInfo, address1, duplicateLoanCheck);
                }
                if (((id == "4000" || id == "4002" || id == "65" || id == "66" || id == "1490" || id == "1240" ? 1 : (id == "1178" ? 1 : 0)) & (flag4 ? 1 : 0)) != 0)
                {
                  IConfigurationManager configurationManager = this.session.ConfigurationManager;
                  string guid = this.loan.GUID;
                  List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo = new List<Dictionary<LoanDuplicateChecker.CheckField, string>>();
                  borrowerInfo.Add(info1);
                  EllieMae.EMLite.ClientServer.Address address2 = address1;
                  string loanFolder = duplicateLoanCheck;
                  loanduplicates = configurationManager.GetLoanDuplicateInfo(guid, borrowerInfo, address2, loanFolder);
                }
                if (((id == "4004" || id == "4006" || id == "97" || id == "98" || id == "1480" || id == "1268" ? 1 : (id == "1179" ? 1 : 0)) & (flag5 ? 1 : 0)) != 0)
                {
                  IConfigurationManager configurationManager = this.session.ConfigurationManager;
                  string guid = this.loan.GUID;
                  List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo = new List<Dictionary<LoanDuplicateChecker.CheckField, string>>();
                  borrowerInfo.Add(info2);
                  EllieMae.EMLite.ClientServer.Address address3 = address1;
                  string loanFolder = duplicateLoanCheck;
                  loanduplicates = configurationManager.GetLoanDuplicateInfo(guid, borrowerInfo, address3, loanFolder);
                }
                this.lookForDuplicateLoans(loanduplicates);
              }
            }
            if (id == "3164" && Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
            {
              if (val == "Y")
              {
                this.loan.Calculator.GetIntentToProceedIDisclosureTracking2015Log(this.showDisclosedLELogs(true, true));
              }
              else
              {
                this.loan.SetField("3972", "//");
                foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.loan.GetLogList().GetAllIDisclosureTracking2015Log(true))
                {
                  if (disclosureTracking2015Log.IntentToProceed)
                    disclosureTracking2015Log.IntentToProceed = false;
                }
              }
            }
            if ((id == "3926" || id == "3917" || id == "3918" || id == "3919" || id == "3921" || id == "3927" || id == "3928" || id == "3929" || id == "3930" || id == "3931" || id == "3932" || id == "4110" || id == "TPO.X15") && !string.IsNullOrEmpty(this.loan.GetField("3571")) && Utils.ParseDecimal((object) this.loan.GetField("3571")) <= 0M)
            {
              int num15 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Current Principal cannot be zero or a negative amount. Please correct the amount before proceeding.");
              this.editor.GoToField("3571");
            }
            else
            {
              if (this.loan != null && !this.loan.IsTemplate)
                Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
              if (this.CurrentFormName == "PaymentExample" && (id == "LE1.X2" || id == "3" || id == "247" || id == "2625"))
                this.loan.Calculator.FormCalculation("IRPCEXAMPLE");
              if ((id == "LE2.X96" || id == "4512" || id == "1959") && val == "FHFB_NACMR")
              {
                DateTime date = Utils.ParseDate((object) "05/29/2019");
                date = date.Date;
                if (date.CompareTo(Utils.ParseDate((object) this.GetField("2553"))) <= 0)
                {
                  int num16 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The FHFA determined that due to dwindling participation, the Monthly Interest Rate Survey would be discontinued as of May 29, 2019.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
              }
              if (id == "3569")
              {
                if (this.loan.GetField("682") == "//" && val != "//")
                {
                  int num17 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Loan First Payment Date [682] must first be present before entering the Paid to Date or the First Payment Due.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  this.SetField("3569", "//");
                  return;
                }
                DateTime date1 = Utils.ParseDate((object) this.loan.GetField("682"));
                DateTime date2 = Utils.ParseDate((object) val);
                if (date2 != DateTime.MinValue && date2.CompareTo(date1.AddMonths(-1)) < 0)
                {
                  int num18 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Paid To Date must not be earlier than one month before the Loan First Payment Date", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  this.SetField("3569", "//");
                  return;
                }
                if (date2 != DateTime.MinValue && date2.Day != date1.Day)
                {
                  int num19 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Paid To Date must be on the same day of the month as the Loan First Payment Date", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  this.SetField("3569", "//");
                  return;
                }
                if (this.loan.GetField("3570") != "//")
                {
                  DateTime date3 = Utils.ParseDate((object) this.loan.GetField("3570"));
                  if (date2 != DateTime.MinValue && date2.CompareTo(date3) > 0)
                  {
                    int num20 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "First Payment Due must be on or after the Paid to Date", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.SetField("3570", "//");
                    return;
                  }
                }
              }
              if (!(id == "3570"))
                return;
              if (this.loan.GetField("682") == "//" && val != "//")
              {
                int num21 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Loan First Payment Date [682] must first be present before entering the Paid to Date or the First Payment Due.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.SetField("3570", "//");
              }
              else
              {
                DateTime date4 = Utils.ParseDate((object) this.loan.GetField("682"));
                DateTime date5 = Utils.ParseDate((object) val);
                if (date5 != DateTime.MinValue && date5.CompareTo(date4) < 0)
                {
                  int num22 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "First Payment Due must on or after the Loan First Payment Date", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  this.SetField("3570", "//");
                }
                else
                {
                  if (this.loan.GetField("3569") != "//")
                  {
                    DateTime date6 = Utils.ParseDate((object) this.loan.GetField("3569"));
                    if (date5 != DateTime.MinValue && date5.CompareTo(date6) < 0)
                    {
                      int num23 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "First Payment Due must be on or after the Paid to Date", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                      this.SetField("3570", "//");
                      return;
                    }
                  }
                  if (!(date5 != DateTime.MinValue) || date5.Day == date4.Day)
                    return;
                  int num24 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "First Payment Due must be on the same day of the month as the Loan First Payment Date", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  this.SetField("3570", "//");
                }
              }
            }
          }
        }
      }
    }

    public static bool canCheckDuplicate(
      Dictionary<LoanDuplicateChecker.CheckField, string> info,
      EllieMae.EMLite.ClientServer.Address address)
    {
      if (!string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.FirstName]) || !string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.LastName]) || !string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.SSN]) || !string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.HomePhone]) || !string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.MobilePhone]) || !string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.Email]) || !string.IsNullOrEmpty(info[LoanDuplicateChecker.CheckField.WorkEmail]))
        return true;
      return !string.IsNullOrEmpty(address.Street1) && !string.Equals(address.Street1, "TBD") && !string.IsNullOrEmpty(address.Zip);
    }

    private bool updateFieldValue(string id, FieldSource fieldSource, string val)
    {
      if (fieldSource == FieldSource.CurrentLoan)
      {
        try
        {
          this.UpdateFieldValue(id, val);
          this.applyLoanTemplateTriggerRule(id, val);
          if (this.session.LoanData != null && this.session.LoanData.DDMTrigger != null)
            this.session.LoanDataMgr.DDMTriggerExecute(DDMStartPopulationTrigger.FieldChange, id, true);
          return true;
        }
        catch
        {
          return false;
        }
      }
      else
      {
        bool flag = this.setFieldValue(id, fieldSource, val ?? "");
        if (this.loan != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
          this.loan.SyncPiggyBackFiles((string[]) null, true, false, id, val, fieldSource != FieldSource.CurrentLoan);
        if (this.loan != null && fieldSource == FieldSource.LinkedLoan)
          this.loan.Calculator.FormCalculation(id);
        this.RefreshAllControls(true);
        return flag;
      }
    }

    private void applyLoanTemplateTriggerRule(string id, string val)
    {
      if (this.loan == null || this.loan.IsTemplate)
        return;
      TriggerImplDef def = this.session.LoanDataMgr.ApplyLoanTemplateTrigger(id, val);
      if (def == null)
        return;
      InputHandlerUtil inputHandlerUtil = new InputHandlerUtil(this.session);
      inputHandlerUtil.ShowApplyLoanTemplateProgress();
      string field1 = this.loan.GetField("3969");
      string field2 = this.loan.GetField("1825");
      this.session.LoanDataMgr.ApplyLoanTemplate(def);
      if (this.editor != null && string.Compare(field1, this.loan.GetField("3969"), true) != 0 || string.Compare(field2, this.loan.GetField("1825"), true) != 0)
        this.editor.RefreshLoanContents();
      inputHandlerUtil.CloseProgress();
    }

    private void lookForDuplicateLoans(List<LoanDuplicateChecker> loanduplicates)
    {
      if (loanduplicates == null || loanduplicates.Count == 0)
        return;
      string str1 = this.session.ConfigurationManager.GetDuplicates(this.loan.GUID);
      List<LoanDuplicateChecker> duplicateCheckerList = new List<LoanDuplicateChecker>();
      foreach (LoanDuplicateChecker loanduplicate in loanduplicates)
      {
        string str2 = str1;
        Guid guid = loanduplicate.GUID;
        string str3 = guid.ToString();
        if (str2.Contains(str3))
        {
          duplicateCheckerList.Add(loanduplicate);
        }
        else
        {
          string str4 = str1;
          guid = loanduplicate.GUID;
          string str5 = guid.ToString();
          str1 = str4 + str5 + ",";
        }
      }
      this.session.ConfigurationManager.SaveDuplicate(this.loan.GUID, str1.Trim(','));
      if (duplicateCheckerList.Count >= loanduplicates.Count)
        return;
      using (MatchedLoanDuplicate matchedLoanDuplicate = new MatchedLoanDuplicate(this.session, loanduplicates))
      {
        int num = (int) matchedLoanDuplicate.ShowDialog();
      }
    }

    protected bool setFieldValue(string id, FieldSource fieldSource, string val)
    {
      try
      {
        switch (id)
        {
          case "1051":
            val = Utils.FormatMERS(val);
            goto label_30;
          case "1240":
          case "1268":
          case "95":
          case "89":
          case "87":
          case "88":
          case "VEND.X119":
          case "VEND.X130":
          case "VEND.X141":
          case "VEND.X152":
          case "92":
          case "VEND.X419":
          case "Seller3.Email":
          case "Seller4.Email":
          case "VEND.X432":
          case "94":
          case "VEND.X164":
          case "93":
          case "VEND.X43":
          case "VEND.X21":
          case "90":
          case "1411":
          case "VEND.X186":
          case "VEND.X197":
          case "VEND.X208":
          case "VEND.X53":
          case "VEND.X273":
          case "VEND.X288":
          case "VEND.X305":
          case "VEND.X319":
          case "VEND.X670":
          case "VEND.X63":
          case "VEND.X73":
          case "VEND.X83":
          case "VEND.X10":
            if (!(val != string.Empty) || Utils.ValidateEmail(val))
              break;
            goto label_5;
          default:
            if (!id.StartsWith("NBOC") || !id.EndsWith("11"))
              break;
            goto case "1240";
        }
        if ((!(id == "1178") && !(id == "1179") && !(id == "LE3.X2") && !(id == "LE3.X7") || !(val != string.Empty) || Utils.ValidateEmail(val)) && (!(id == "HOC.X10") || !(val != string.Empty) || Utils.ValidateEmail(val)) && (!(id == "CD5.X17") && !(id == "CD5.X29") && !(id == "CD5.X41") && !(id == "CD5.X53") && !(id == "CD5.X65") || !(val != string.Empty) || Utils.ValidateEmail(val)) && (!(id == "VEND.X1005") && !(id == "VEND.X1022") && !(id == "VEND.X1030") || !(val != string.Empty) || Utils.ValidateEmail(val)))
        {
          if (id == "SP0112" && val != string.Empty && !SystemUtil.IsValidURL(val) || id == "2011" && val != string.Empty && !SystemUtil.IsValidURL(val))
            throw new FieldValidationException(id, val, "The value '" + val + "' is not a valid web url format.");
          switch (id)
          {
            case "388":
              this.removeFieldLock("454", FieldSource.CurrentLoan);
              goto label_30;
            case "NEWHUD.X769":
              this.removeFieldLock("NEWHUD.X770", FieldSource.CurrentLoan);
              goto label_30;
            default:
              if ((id == "3237" || id == "3238") && val != string.Empty)
              {
                if (Utils.ParseLong((object) val).ToString() != val)
                  throw new FieldValidationException(id, val, "The value '" + val + "' is not a valid format");
                goto label_30;
              }
              else if (id == "1172" && val == "VA")
              {
                bool flag = false;
                if (!Utils.CheckIf2015RespaTila(this.GetField("3969")))
                {
                  for (int index = 809; index <= 818; ++index)
                  {
                    if (this.GetFieldValue("NEWHUD.X" + (object) index) != string.Empty)
                    {
                      flag = true;
                      break;
                    }
                  }
                }
                if (flag)
                  throw new FieldValidationException(id, val, "Before changing the loan type to VA, click the Edit icon on line 1102 of the 2010 Itemization or 2010 HUD-1, and make sure fees are entered in the Escrow Fee field only.");
                goto label_30;
              }
              else if (id == "230" || id == "1405" || id == "232" || id == "233" || id == "CASASRN.X167" || id == "CASASRN.X168")
              {
                while (val.IndexOf("..") > -1)
                  val = val.Replace("..", ".");
                if (val != string.Empty && Utils.ParseDouble((object) val) != 0.0)
                {
                  val = Utils.ParseDouble((object) val).ToString("N2");
                  goto label_30;
                }
                else
                  goto label_30;
              }
              else if (id == "682" && this.loan != null && this.loan.Calculator != null)
              {
                this.loan.Calculator.FormCalculation("ShiftPaymentFrequence", "682", val);
                goto label_30;
              }
              else
                goto label_30;
          }
        }
label_5:
        throw new FieldValidationException(id, val, "The value '" + val + "' is not a valid email format");
label_30:
        if (id == "NEWHUD.X719" || id == "NEWHUD.X3")
          val = Utils.ParseInt((object) val) != -1 || !(val != string.Empty) || !(val.ToLower() != "na") ? val.ToUpper() : throw new FieldValidationException(id, val, "The value '" + val + "' is not a valid format");
        else if ((id == "NEWHUD.X725" || id == "LE1.X6" || id == "LE1.X8") && val != "")
          val = Utils.ParseTime(val, true);
        if ((id == "NEWHUD.X572" || id == "NEWHUD.X39") && val.ToLower() == "na")
          val = val.ToUpper();
        if (id == "NEWHUD.X1" && val != string.Empty)
          val = !(Utils.ParseDate((object) val) == DateTime.MinValue) || !(val.ToLower() != "na") ? val.ToUpper() : throw new FieldValidationException(id, val, "The value '" + val + "' is not a valid date format or value \"NA\".");
        if (id == "HMDA.X87" && val != "")
        {
          if (!ZipCodeUtils.IsValid(val) && val.ToLower() != "na" && val.ToLower() != "exempt")
            throw new FieldValidationException(id, val, "The value '" + val + "' is not a valid format");
          if (val.ToLower() != "na" && val.ToLower() != "exempt")
          {
            EllieMae.Encompass.Forms.Control control = this.currentForm.FindControl("l_HMDAX87");
            if (control != null)
              this.populateZipCodeRelatedFields((FieldControl) control, val);
          }
          val = val.ToUpper();
        }
        if (this.loan != null && this.loan.LinkedData != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked) && this.PiggyFields != null && this.PiggyFields.IsSycnField(id) && this.loan.LinkedData.ContentAccess != LoanContentAccess.FullAccess && this.loan.LinkedData.IsFieldReadOnly(id))
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "Linked loan field cannot be set when the field " + id + " in linked loan is read-only field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.refreshContents(id, false);
          return false;
        }
        if (id == "65" || id == "97")
        {
          if (id == "65" && this.GetFieldValue("65") != val)
          {
            CRMLog crmMapping = this.loan.GetLogList().GetCRMMapping(this.loan.CurrentBorrowerPair.Borrower.Id);
            if (crmMapping != null)
            {
              BorrowerInfo borrower = this.session.ContactManager.GetBorrower(crmMapping.ContactGuid);
              if (borrower != null)
              {
                RxBorrowerSSN rxBorrowerSsn = new RxBorrowerSSN(false, borrower, false, val);
                if (rxBorrowerSsn.HasConflict)
                {
                  if (rxBorrowerSsn.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
                    return true;
                  val = rxBorrowerSsn.BorrowerObj.SSN;
                }
              }
            }
          }
          else if (this.GetFieldValue("97") != val)
          {
            CRMLog crmMapping = this.loan.GetLogList().GetCRMMapping(this.loan.CurrentBorrowerPair.CoBorrower.Id);
            if (crmMapping != null)
            {
              BorrowerInfo borrower = this.session.ContactManager.GetBorrower(crmMapping.ContactGuid);
              if (borrower != null)
              {
                RxBorrowerSSN rxBorrowerSsn = new RxBorrowerSSN(true, borrower, false, val);
                if (rxBorrowerSsn.HasConflict)
                {
                  if (rxBorrowerSsn.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
                    return true;
                  val = rxBorrowerSsn.BorrowerObj.SSN;
                }
              }
            }
          }
        }
        if (id == "14" && val != string.Empty)
        {
          if (!this.StatePermission(val, false))
          {
            this.refreshContents(id, false);
            return false;
          }
          if (val != string.Empty && this.loan != null)
          {
            if (this.lateChargeTable == null)
              this.lateChargeTable = this.session.ConfigurationManager.GetServicingLateCharge(val);
            if (this.lateChargeTable != null && this.lateChargeTable.ContainsKey((object) val))
            {
              double[] numArray = (double[]) this.lateChargeTable[(object) val];
              this.loan.SetField("2831", numArray[0].ToString("N2"));
              this.loan.SetField("2832", numArray[1].ToString("N2"));
            }
            else
            {
              this.loan.SetField("2831", "");
              this.loan.SetField("2832", "");
            }
          }
        }
        else if (id == "1811" && val == "N")
          val = "";
        else if (id.StartsWith("FR") && id.EndsWith("15") && val == "N")
          val = "";
        else if (id == "VASUMM.X23")
          this.loan.SetField("2853", val);
        FieldDefinition fieldDefinition = this.inputData.GetFieldDefinition(id);
        val = this.loan != null ? this.loan.ReformatFieldValue(fieldDefinition, val) : fieldDefinition.ValidateFormat(val);
        if (id.Equals("MORNET.X67"))
        {
          EllieMae.EMLite.DataEngine.FieldOption fieldOption = fieldDefinition.Options.GetOptionByValue(val.Trim()) ?? fieldDefinition.Options.GetOptionByText(val.Trim());
          if (fieldOption != null && !val.Equals(fieldOption.Value))
            val = fieldOption.Value;
        }
        if (id == "3928" && !this.compareLateFeeBeginEndDate(Utils.ParseDate((object) val), Utils.ParseDate((object) this.inputData.GetField("3929"))) || id == "3929" && !this.compareLateFeeBeginEndDate(Utils.ParseDate((object) this.inputData.GetField("3928")), Utils.ParseDate((object) val)))
          return false;
        if (id == "3927" && val != string.Empty && Utils.ParseInt((object) val) < 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The Grace Period # of Days cannot be negative number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (id == "3930" && val != string.Empty && Utils.ParseInt((object) val) < 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The Total Late Days cannot be negative number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (id == "LE1.XG9" && val == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The time zone cannot be updated to blank from a specified time zone.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (id == "1172" && val == "FHA" && this.loan != null && this.loan.Calculator != null)
          this.loan.Calculator.FormCalculation("ValidateFHACountyLimit");
        else if (id == "15")
          this.previousZip = this.loan.GetSimpleField("15");
        if (fieldSource == FieldSource.CurrentLoan)
        {
          this.inputData.SetField(id, val, true);
          if (this.loan != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
            this.SyncPiggyBackField(id, FieldSource.LinkedLoan, val);
        }
        else if (this.loan != null && this.loan.LinkSyncType != LinkSyncType.None)
        {
          this.loan.LinkedData.SetField(id, val);
          if (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked)
            this.SyncPiggyBackField(id, FieldSource.CurrentLoan, val);
        }
        if ("|36|37|68|69|31|1602|11|URLA.X73|URLA.X74|URLA.X75|12|14|15|FR0104|FR0106|FR0107|FR0108|FR0204|FR0206|FR0207|FR0208|4000|4001|4002|4003|4004|4005|4006|4007".IndexOf(id) > -1 && this.loan != null && this.loan.Calculator != null)
          this.loan.Calculator.UpdateAccountName(id);
        if (this.loan != null && this.loan.Calculator != null && this.loan.Calculator.GetCorrespondentLateFeeOtherDateField() != null)
          this.loan.Calculator.FormCalculation("3918", (string) null, (string) null);
        return true;
      }
      catch (MissingPrerequisiteException ex)
      {
        if (!this.FormIsForTemplate)
        {
          if (this.poppingUpPrerequiredDialog(ex, id))
            return true;
        }
      }
      catch (CountyLimitException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (GFEDaysToExpireException ex)
      {
        int num = (int) new ComplianceAlertMessage("GFE Expiration Date", ex.Message).ShowDialog((IWin32Window) this.mainScreen);
      }
      catch (ComplianceCalendarException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.refreshContents(id, false);
      return false;
    }

    private bool poppingUpPrerequiredDialog(MissingPrerequisiteException ex, string currentFieldID)
    {
      BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
      if (!businessRuleCheck.HasPrerequiredFields(this.session.LoanData, currentFieldID))
        return false;
      PreRequiredDialog.Instance.InitForm(this.session.LoanData, ex, currentFieldID, businessRuleCheck.PrerequiredFields);
      if (!PreRequiredDialog.Instance.Visible)
      {
        PreRequiredDialog.Instance.Owner = this.session.MainForm;
        PreRequiredDialog.Instance.FieldSelectedDoubleClick += new EventHandler(this.requireDialog_FieldSelectedDoubleClick);
        PreRequiredDialog.Instance.Show((IWin32Window) this.session.MainForm);
      }
      if (PreRequiredDialog.Instance.WindowState == FormWindowState.Minimized)
        PreRequiredDialog.Instance.WindowState = FormWindowState.Normal;
      PreRequiredDialog.Instance.Activate();
      return true;
    }

    private void requireDialog_FieldSelectedDoubleClick(object sender, EventArgs e)
    {
      this.editor.GoToField(PreRequiredDialog.Instance.SelectedFieldID);
    }

    private bool compareLateFeeBeginEndDate(DateTime lateFeeBeginDate, DateTime lateFeeEndDate)
    {
      if (!(lateFeeBeginDate != DateTime.MinValue) || !(lateFeeEndDate != DateTime.MinValue) || DateTime.Compare(lateFeeBeginDate, lateFeeEndDate) <= 0)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The Late Days Begin cannot be later than Late Days End!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private bool populateZipCodeRelatedFields(FieldControl ctrl, string val)
    {
      if (val.Length < 5 || ctrl.Field.Format != LoanFieldFormat.ZIPCODE && ctrl.Field.FieldID != "FR0008" && ctrl.Field.FieldID != "FR0011" && ctrl.Field.FieldID != "FE0007" && ctrl.Field.FieldID != "HMDA.X87" || this.loan != null && this.loan.IsForeignIndicatorSelected(this.MapFieldId(ctrl.Field.FieldID)))
        return true;
      ZipCodeInfo infoWithUserDefined = ZipcodeSelector.GetZipCodeInfoWithUserDefined(val.Substring(0, 5));
      if (infoWithUserDefined == null)
        return true;
      if (ctrl.Field.FieldID == "HMDA.X87")
      {
        this.setFieldValue("HMDA.X89", ctrl.FieldSource, infoWithUserDefined.City);
        this.setFieldValue("HMDA.X90", ctrl.FieldSource, infoWithUserDefined.State);
        return true;
      }
      if (ctrl.Field.FieldID == "HMDA.X87")
      {
        this.setFieldValue("HMDA.X89", ctrl.FieldSource, infoWithUserDefined.City);
        this.setFieldValue("HMDA.X90", ctrl.FieldSource, infoWithUserDefined.State);
        return true;
      }
      if (ctrl.Field.FieldID == "FR0108")
        this.setFieldValue("FR0109", ctrl.FieldSource, infoWithUserDefined.County);
      if (ctrl.Field.FieldID.StartsWith("FR") && ctrl.Field.FieldID.EndsWith("08") && this.loan != null)
        this.populateCountyToVOR(ctrl.Field.FieldID, ctrl.FieldSource, infoWithUserDefined.County);
      EllieMae.Encompass.Forms.Control[] controlsByType = this.currentForm.FindControlsByType(typeof (ZipCodeLookup));
      if (controlsByType.Length == 0)
        return true;
      foreach (ZipCodeLookup zipCodeLookup in controlsByType)
      {
        if (ctrl.Equals((object) zipCodeLookup.ZipControl))
        {
          string fieldId = ctrl.Field.FieldID;
          string id1 = this.MapFieldId(zipCodeLookup.CityField.FieldID);
          string id2 = this.MapFieldId(zipCodeLookup.CountyField.FieldID);
          string id3 = this.MapFieldId(zipCodeLookup.StateField.FieldID);
          if (this.session.LoanData != null)
            this.session.LoanData.SkipCountyLimitCalculation = true;
          if (fieldId.StartsWith("FM"))
          {
            string str = "";
            if (id1 != "")
              str = id1.Substring(0, 4);
            else if (id2 != "")
              str = id2.Substring(0, 4);
            else if (id3 != "")
              str = id3.Substring(0, 4);
            if (this.loan.GetField(str + "28") == "Y")
              return true;
          }
          if (id3 != "")
          {
            if (fieldId == "15" && infoWithUserDefined.State != string.Empty && !this.StatePermission(infoWithUserDefined.State, false))
            {
              this.setFieldValue("15", ctrl.FieldSource, this.previousZip);
              this.refreshContents(fieldId, false);
              return false;
            }
            this.setFieldValue(id3, ctrl.FieldSource, infoWithUserDefined.State);
          }
          if (this.session.LoanData != null)
            this.session.LoanData.SkipCountyLimitCalculation = false;
          if (id1 != "")
            this.setFieldValue(id1, ctrl.FieldSource, Utils.CapsConvert(infoWithUserDefined.City, false));
          if (id2 != "")
          {
            if (infoWithUserDefined.County != string.Empty)
              this.setFieldValue(id2, ctrl.FieldSource, Utils.CapsConvert(infoWithUserDefined.County, false));
            else
              this.setFieldValue(id2, ctrl.FieldSource, string.Empty);
          }
        }
      }
      if (ctrl.Field.FieldID == "15")
      {
        bool flag = false;
        if (this.session.ServerManager.GetServerSetting("Components.DuplicateLoanCheck", false) != null)
          flag = Session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckGlobal;
        if (((this.loan == null ? 0 : (!this.loan.IsTemplate ? 1 : 0)) & (flag ? 1 : 0)) != 0 && Session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckLoanOnly)
        {
          string duplicateLoanCheck = Session.LoanManager.GetAllIncludeInDuplicateLoanCheck();
          EllieMae.EMLite.ClientServer.Address address1 = new EllieMae.EMLite.ClientServer.Address(this.GetField("11"), "", this.GetField("12"), this.GetField("14"), this.GetField("15"));
          Dictionary<LoanDuplicateChecker.CheckField, string> info1 = new Dictionary<LoanDuplicateChecker.CheckField, string>();
          info1.Add(LoanDuplicateChecker.CheckField.FirstName, this.GetField("4000"));
          info1.Add(LoanDuplicateChecker.CheckField.LastName, this.GetField("4002"));
          info1.Add(LoanDuplicateChecker.CheckField.SSN, this.GetField("65"));
          info1.Add(LoanDuplicateChecker.CheckField.HomePhone, this.GetField("66"));
          info1.Add(LoanDuplicateChecker.CheckField.MobilePhone, this.GetField("1490"));
          info1.Add(LoanDuplicateChecker.CheckField.Email, this.GetField("1240"));
          info1.Add(LoanDuplicateChecker.CheckField.WorkEmail, this.GetField("1178"));
          if (InputHandlerBase.canCheckDuplicate(info1, address1))
          {
            List<LoanDuplicateChecker> duplicateCheckerList = new List<LoanDuplicateChecker>();
            IConfigurationManager configurationManager = this.session.ConfigurationManager;
            string guid = this.loan.GUID;
            List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo = new List<Dictionary<LoanDuplicateChecker.CheckField, string>>();
            borrowerInfo.Add(info1);
            EllieMae.EMLite.ClientServer.Address address2 = address1;
            string loanFolder = duplicateLoanCheck;
            this.lookForDuplicateLoans(configurationManager.GetLoanDuplicateInfo(guid, borrowerInfo, address2, loanFolder));
          }
          Dictionary<LoanDuplicateChecker.CheckField, string> info2 = new Dictionary<LoanDuplicateChecker.CheckField, string>();
          info2.Add(LoanDuplicateChecker.CheckField.FirstName, this.GetField("4004"));
          info2.Add(LoanDuplicateChecker.CheckField.LastName, this.GetField("4006"));
          info2.Add(LoanDuplicateChecker.CheckField.SSN, this.GetField("97"));
          info2.Add(LoanDuplicateChecker.CheckField.HomePhone, this.GetField("98"));
          info2.Add(LoanDuplicateChecker.CheckField.MobilePhone, this.GetField("1480"));
          info2.Add(LoanDuplicateChecker.CheckField.Email, this.GetField("1268"));
          info2.Add(LoanDuplicateChecker.CheckField.WorkEmail, this.GetField("1179"));
          if (InputHandlerBase.canCheckDuplicate(info2, address1))
          {
            List<LoanDuplicateChecker> loanduplicates = new List<LoanDuplicateChecker>();
            List<LoanDuplicateChecker> duplicateCheckerList = loanduplicates;
            IConfigurationManager configurationManager = this.session.ConfigurationManager;
            string guid = this.loan.GUID;
            List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo = new List<Dictionary<LoanDuplicateChecker.CheckField, string>>();
            borrowerInfo.Add(info2);
            EllieMae.EMLite.ClientServer.Address address3 = address1;
            string loanFolder = duplicateLoanCheck;
            List<LoanDuplicateChecker> loanDuplicateInfo = configurationManager.GetLoanDuplicateInfo(guid, borrowerInfo, address3, loanFolder);
            duplicateCheckerList.AddRange((IEnumerable<LoanDuplicateChecker>) loanDuplicateInfo);
            this.lookForDuplicateLoans(loanduplicates);
          }
        }
      }
      return true;
    }

    private void populateCountyToVOR(string id, FieldSource fieldSource, string countyName)
    {
      int num1 = Utils.ParseInt(id.Length > 6 ? (object) id.Substring(2, 3) : (object) id.Substring(2, 2));
      if (num1 < 1 || num1 > 4)
        return;
      string str = num1 == 1 || num1 == 3 ? "BR" : "CR";
      int numberOfResidence = this.loan.GetNumberOfResidence(num1 == 1 || num1 == 3);
      int num2 = 0;
      for (int index = 1; index <= numberOfResidence; ++index)
      {
        if ((num1 != 1 && num1 != 2 || !(this.loan.GetSimpleField(str + index.ToString("00") + "23") != "Current")) && (num1 != 3 && num1 != 4 || !(this.loan.GetSimpleField(str + index.ToString("00") + "23") == "Current")))
        {
          ++num2;
          if (num2 == 1)
          {
            this.setFieldValue(str + index.ToString("00") + "22", fieldSource, countyName);
            break;
          }
        }
      }
    }

    public bool UpdateDocTrackingForVerifs(string id, bool createNew)
    {
      if (this.loan == null)
        return false;
      int length = id.Length;
      if (length < 6)
        return false;
      object obj = InputHandlerBase.docIdTbl[(object) id.Substring(0, 2)];
      if (obj == null)
        return false;
      string[] strArray = (string[]) obj;
      string str1 = id.Substring(0, 4);
      if (length > 6)
        str1 = id.Substring(0, 5);
      string simpleField = this.loan.GetSimpleField(str1 + strArray[1]);
      if (id.Substring(0, 2) != "IR" && id.Substring(0, 2) != "AR" && this.loan.GetSimpleField(str1 + strArray[3]) == "Y")
        return false;
      LogList logList = this.loan.GetLogList();
      VerifLog rec = logList.GetVerif(simpleField);
      if (rec == null & createNew)
      {
        rec = new VerifLog(simpleField, this.session.UserID, this.loan.PairId);
        rec.Stage = logList.NextStage;
        if (strArray[0] == "IRS4506 - Copy Request" || strArray[0] == "IRS4506T - Trans Request")
        {
          DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
          DocumentTemplate documentTemplate1 = (DocumentTemplate) null;
          foreach (DocumentTemplate documentTemplate2 in documentTrackingSetup)
          {
            if (!(documentTemplate2.SourceType != "Standard Form") && documentTemplate2.Source == strArray[0])
            {
              documentTemplate1 = documentTemplate2;
              break;
            }
          }
          if (documentTemplate1 == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) System.Windows.Forms.Form.ActiveForm, "In order to create a document in the eFolder, your system administrator needs to setup a document that is linked to the '" + strArray[0] + "' standard form", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          rec.DaysTillExpire = documentTemplate1.DaysTillExpire;
          rec.DaysDue = documentTemplate1.DaysTillDue;
          rec.Title = documentTemplate1.Name;
        }
        else
        {
          VerifDays systemSettings = (VerifDays) this.session.GetSystemSettings(typeof (VerifDays));
          rec.DaysTillExpire = systemSettings.GetExpDays(strArray[0]);
          rec.DaysDue = systemSettings.GetRecvDays(strArray[0]);
          rec.Title = strArray[0];
        }
        logList.AddRecord((LogRecordBase) rec);
      }
      else if (rec == null)
        return false;
      string str2 = this.loan.GetSimpleField(str1 + strArray[2]);
      if (id.Substring(0, 2) == "IR" || id.Substring(0, 2) == "AR")
        str2 = str2 + " " + this.loan.GetSimpleField(str1 + "03");
      rec.RequestedFrom = str2.Trim();
      return true;
    }

    internal void SetField(string id, string val) => this.UpdateFieldValue(id, val, false);

    internal string GetField(string id) => this.GetFieldValue(id);

    protected virtual string GetFieldValue(string id)
    {
      if (this.inputData == null)
        return string.Empty;
      string fieldValue = this.inputData.GetField(id);
      if (id == "FICC.X7" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (id == "FICC.X18" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (id == "FICC.X11" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (id == "FICC.X22" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (id == "HMDA.X27" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (id == "HMDA.X92" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (id == "333" && this.inputData.GetField("SYS.X8") == "Y" && fieldValue != string.Empty)
        fieldValue = Utils.ArithmeticRounding(Utils.ParseDouble((object) fieldValue), 2).ToString("N2");
      if (id == "VASUMM.X23" && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if ((id == "4970" || id == "5018" || id == "5019") && fieldValue.IndexOf(",") >= 0)
        fieldValue = fieldValue.Replace(",", "");
      if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(id) && !string.IsNullOrEmpty(fieldValue) && Utils.IsDouble((object) fieldValue))
        fieldValue = !this.session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? Utils.ParseDouble((object) fieldValue).ToString("N3") : Utils.ParseDouble((object) fieldValue).ToString("N10");
      return fieldValue;
    }

    protected virtual string GetFieldValue(string id, FieldSource source)
    {
      using (PerformanceMeter.Current.BeginOperation("InputHandlerBase.GetFieldValue"))
      {
        if (id == "" || source == FieldSource.Other)
          return "";
        if (source == FieldSource.CurrentLoan)
          return this.GetFieldValue(id);
        return this.loan.LinkedData == null ? "" : this.loan.LinkedData.GetField(id);
      }
    }

    protected virtual FieldFormat GetFieldFormat(string id)
    {
      if (id == "FE0011" || id == "FE0014" || id == "FE0051" || id == "FE0098" || id == "FR0098")
        return FieldFormat.DATE;
      return this.inputData != null ? this.inputData.GetFormat(id) : FieldFormat.NONE;
    }

    protected virtual string FormatValue(string fieldId, string value) => (string) null;

    private bool reformatInput(string fieldId, ref string value)
    {
      string str1 = value;
      string str2 = this.FormatValue(fieldId, value);
      if (str2 != null)
      {
        value = str2;
        return value != str1;
      }
      if (fieldId == "")
        return false;
      bool needsUpdate = false;
      if (fieldId.Length == 6 && (fieldId.EndsWith("04") || fieldId.EndsWith("05")) && (fieldId.StartsWith("IR") || fieldId.StartsWith("AR")))
      {
        string str3 = fieldId.Substring(0, 2);
        if (this is TAX4506InputHandler)
        {
          TAX4506InputHandler x4506InputHandler = (TAX4506InputHandler) this;
          str3 += x4506InputHandler.CurrentIndex;
        }
        else if (this is TAX4506TInputHandler)
        {
          TAX4506TInputHandler x4506TinputHandler = (TAX4506TInputHandler) this;
          str3 += x4506TinputHandler.CurrentIndex;
        }
        if (fieldId.EndsWith("04") && this.loan != null && this.loan.GetField(str3 + "57") != "Y" || fieldId.EndsWith("05") && this.loan != null && this.loan.GetField(str3 + "58") != "Y")
          value = Utils.FormatInput(value, FieldFormat.SSN, ref needsUpdate);
        if (value.Length > 2 && (fieldId.EndsWith("04") && this.loan != null && this.loan.GetField(str3 + "57") == "Y" || fieldId.EndsWith("05") && this.loan != null && this.loan.GetField(str3 + "58") == "Y"))
        {
          value = value.Replace("-", "");
          value = value.Substring(0, 2) + "-" + (value.Length >= 9 ? value.Substring(2, 7) : value.Substring(2));
        }
      }
      else if (fieldId == "IRS4506.X4" || fieldId == "IRS4506.X5")
      {
        if (fieldId == "IRS4506.X4" && this.loan != null && this.loan.GetField("IRS4506.X57") != "Y" || fieldId == "IRS4506.X5" && this.loan != null && this.loan.GetField("IRS4506.X58") != "Y")
          value = Utils.FormatInput(value, FieldFormat.SSN, ref needsUpdate);
        if (value.Length > 2 && (fieldId == "IRS4506.X4" && this.loan != null && this.loan.GetField("IRS4506.X57") == "Y" || fieldId == "IRS4506.X5" && this.loan != null && this.loan.GetField("IRS4506.X58") == "Y"))
        {
          value = value.Replace("-", "");
          value = value.Substring(0, 2) + "-" + (value.Length >= 9 ? value.Substring(2, 7) : value.Substring(2));
        }
      }
      else
        value = Utils.FormatInput(value, this.GetFieldFormat(fieldId), ref needsUpdate);
      return needsUpdate;
    }

    public virtual bool ProcessDialogKey(Keys keyData) => false;

    internal double ToDouble(string value)
    {
      try
      {
        return double.Parse(value);
      }
      catch
      {
        return 0.0;
      }
    }

    private void createConversationLogFromControl(ContactButton ctrl)
    {
      ConversationLog con = new ConversationLog(DateTime.Now, this.session.UserInfo.Userid);
      if (ctrl.CompanyField != FieldDescriptor.Empty)
        con.Company = this.GetFieldValue(this.MapFieldId(ctrl.CompanyField.FieldID));
      if (ctrl.PhoneField != FieldDescriptor.Empty)
        con.Phone = this.GetFieldValue(this.MapFieldId(ctrl.PhoneField.FieldID));
      if (ctrl.EmailField != FieldDescriptor.Empty)
        con.Email = this.GetFieldValue(this.MapFieldId(ctrl.EmailField.FieldID));
      string str1 = "";
      string str2 = "";
      if (ctrl.FirstNameField != FieldDescriptor.Empty)
        str1 = this.GetFieldValue(this.MapFieldId(ctrl.FirstNameField.FieldID));
      if (ctrl.LastNameField != FieldDescriptor.Empty)
        str2 = this.GetFieldValue(this.MapFieldId(ctrl.LastNameField.FieldID));
      con.Name = !(str1 != "") || !(str2 != "") ? (!(str1 != "") ? str2 : str1) : str1 + " " + str2;
      con.IsEmail = ctrl.ContactMethod == ContactMethod.Email;
      this.editor.StartConversation(con);
    }

    public virtual void onabort(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onactivate(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnActivate: " + pEvtObj.srcElement.id);
    }

    public virtual void onafterupdate(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onbeforeactivate(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool onbeforecopy(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool onbeforecut(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool onbeforedeactivate(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onbeforeeditfocus(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onbeforepaste(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool onbeforeupdate(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void oncellchange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnClick: " + pEvtObj.srcElement.id);
      if (this.loan != null)
        this.loan.BusinessRuleTrigger = BusinessRuleOnDemandEnum.None;
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (pEvtObj.ctrlKey && controlForElement is FieldControl && this.displayHelpPad(controlForElement as FieldControl))
      {
        pEvtObj.returnValue = (object) false;
        return false;
      }
      if (controlForElement != this.currentField)
        this.onactivate(pEvtObj);
      switch (controlForElement)
      {
        case FieldControl _:
          if (this.mainScreen != null)
          {
            this.session.Application.GetService<IStatusDisplay>().DisplayFieldID(this.MapFieldId(this.parseFieldID(((FieldControl) controlForElement).Field.FieldID)));
            break;
          }
          break;
        case EllieMae.Encompass.Forms.Button _:
          if (this.mainScreen != null)
          {
            this.session.Application.GetService<IStatusDisplay>().DisplayFieldID(((EllieMae.Encompass.Forms.Button) controlForElement).Action);
            break;
          }
          break;
      }
      bool flag = true;
      switch (controlForElement)
      {
        case EllieMae.Encompass.Forms.CheckBox _:
          if (this.inputData != null)
          {
            if (this.pocInputHandler == null)
              this.pocInputHandler = new POCInputHandler(this.inputData, this.currentForm, this, this.session);
            if (this.pocInputHandler != null && this.pocInputHandler.IsPOCFieldClicked(this.CurrentFieldID))
            {
              this.pocInputHandler.TurnOnPOCEntryBox(this.CurrentFieldID, pEvtObj);
              return false;
            }
          }
          this.ProcessChangeEvent(controlForElement as FieldControl);
          break;
        case EllieMae.Encompass.Forms.RadioButton _:
          this.ProcessChangeEvent(controlForElement as FieldControl);
          break;
        case EllieMae.Encompass.Forms.Calendar _:
          this.DisplayDatePicker(controlForElement as EllieMae.Encompass.Forms.Calendar);
          break;
        case FieldLock _:
          this.FlipLockField(controlForElement as FieldLock);
          break;
        case PickList _:
          this.displayPickListOptions(controlForElement as PickList);
          break;
        case Rolodex _:
          this.openRolodex(((Rolodex) controlForElement).Group, "", FieldSource.CurrentLoan);
          break;
        case IActionable _:
          flag = this.execFormAction(((IActionable) controlForElement).Action);
          break;
        case ContactButton _:
          this.createConversationLogFromControl(controlForElement as ContactButton);
          break;
        case EllieMae.Encompass.Forms.BorrowerLink _:
          this.popupBorrowerContactRolodex(((EllieMae.Encompass.Forms.BorrowerLink) controlForElement).BorrowerType == EllieMae.Encompass.Forms.BorrowerType.CoBorrower);
          break;
      }
      this.applyOnDemandBusinessRule();
      bool retVal = true;
      if (flag && this.executeClickEvent(controlForElement, out retVal))
        this.UpdateContents();
      pEvtObj.returnValue = (object) retVal;
      return false;
    }

    public virtual void DisplayDatePicker(EllieMae.Encompass.Forms.Calendar ctrl)
    {
      DateTime selectedDate = DateTime.Now;
      if (ctrl.DateField != FieldDescriptor.Empty)
        selectedDate = Utils.ParseDate((object) this.GetFieldValue(ctrl.DateField.FieldID));
      CalendarPopup calendarPopup = new CalendarPopup(selectedDate);
      calendarPopup.Tag = (object) ctrl;
      calendarPopup.FormClosed += new FormClosedEventHandler(this.onCalendarPopupClosed);
      calendarPopup.Show((IWin32Window) this.session.MainScreen);
    }

    public virtual void onCalendarPopupClosed(object sender, FormClosedEventArgs e)
    {
      CalendarPopup calendarPopup = (CalendarPopup) sender;
      if (calendarPopup.DialogResult != DialogResult.OK)
        return;
      EllieMae.Encompass.Forms.Calendar tag = calendarPopup.Tag as EllieMae.Encompass.Forms.Calendar;
      if (tag.DateField != FieldDescriptor.Empty && !this.updateFieldValue(this.MapFieldId(this.parseFieldID(tag.DateField.FieldID)), tag.FieldSource, calendarPopup.SelectedDate.ToString("MM/dd/yyyy")) || !this.executeDateSelectedEvent((RuntimeControl) tag, calendarPopup.SelectedDate))
        return;
      this.RefreshContents();
    }

    private bool displayHelpPad(FieldControl field)
    {
      string str = this.MapFieldId(field.Field.FieldID);
      string helpKey = field.HelpKey ?? "";
      string text = field.HoverText ?? "";
      if (helpKey != "")
        text = helpKey != "" ? str + ": " + FieldHelp.GetText(helpKey) : text;
      if (text == "")
        return false;
      FieldHelpDialog.ShowHelp(text);
      return true;
    }

    public virtual bool oncontextmenu(mshtml.IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (this.loan != null && this.loan.IsInFindFieldForm)
      {
        string id;
        if (this.loan.ButtonSelectionEnabled && controlForElement is EllieMae.Encompass.Forms.Button)
          id = "Button_" + ((EllieMae.Encompass.Forms.Button) controlForElement).Action;
        else if (this.loan.ButtonSelectionEnabled && controlForElement is ImageButton)
        {
          ++this.fieldLockClickCount;
          if (this.fieldLockClickCount > 1)
          {
            this.fieldLockClickCount = 0;
            return false;
          }
          id = "Button_" + ((ImageButton) controlForElement).Action;
        }
        else if (this.loan.ButtonSelectionEnabled && controlForElement is StandardButton)
        {
          ++this.fieldLockClickCount;
          if (this.fieldLockClickCount > 1)
          {
            this.fieldLockClickCount = 0;
            return false;
          }
          id = "Button_" + ((StandardButton) controlForElement).Action;
        }
        else if (this.loan.ButtonSelectionEnabled && controlForElement is FieldLock)
        {
          ++this.fieldLockClickCount;
          if (this.fieldLockClickCount > 1)
          {
            this.fieldLockClickCount = 0;
            return false;
          }
          string fieldLockId = this.findFieldLockID((FieldLock) controlForElement);
          if (fieldLockId == string.Empty)
          {
            int num = (int) Utils.Dialog((IWin32Window) null, "Cannot find Encompass field that is associated with this Field Lock button.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          id = "LOCKBUTTON_" + fieldLockId;
        }
        else
          id = !controlForElement.ControlID.Equals("hmdaprofilename") ? ((FieldControl) controlForElement).Field.FieldID : "HMDA.X100";
        switch (this.loan.SelectedFieldType(id))
        {
          case LoanData.FindFieldTypes.None:
            this.loan.AddSelectedField(id);
            break;
          case LoanData.FindFieldTypes.NewSelect:
            this.loan.RemoveSelectedField(id);
            break;
          case LoanData.FindFieldTypes.Existing:
            int num1 = (int) Utils.Dialog((IWin32Window) null, "You can't remove existing selected field in current list. Please use 'Remove' button to remove existing fields.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
        }
        this.UpdateContents();
        return false;
      }
      if (controlForElement is EllieMae.Encompass.Forms.TextBox)
        this.GetContactItem(((FieldControl) controlForElement).Field.FieldID);
      if (controlForElement is EllieMae.Encompass.Forms.TextBox && this.getBorrowerType(((FieldControl) controlForElement).Field.FieldID) == InputHandlerBase.BorrowerRolodexType.Borrower)
        this.popupBorrowerContactRolodex(false);
      else if (controlForElement is EllieMae.Encompass.Forms.TextBox && this.getBorrowerType(((FieldControl) controlForElement).Field.FieldID) == InputHandlerBase.BorrowerRolodexType.CoBorrower)
        this.popupBorrowerContactRolodex(true);
      return false;
    }

    private InputHandlerBase.BorrowerRolodexType getBorrowerType(string fieldID)
    {
      switch (fieldID)
      {
        case "4000":
        case "4001":
        case "4002":
        case "4003":
        case "65":
          return InputHandlerBase.BorrowerRolodexType.Borrower;
        case "4004":
        case "4005":
        case "4006":
        case "4007":
        case "97":
          return InputHandlerBase.BorrowerRolodexType.CoBorrower;
        default:
          return InputHandlerBase.BorrowerRolodexType.None;
      }
    }

    private void popupBorrowerContactRolodex(bool isCoborrower)
    {
      if (this.loan != null)
      {
        string empty = string.Empty;
        string[] strArray;
        if (isCoborrower)
          strArray = new string[5]
          {
            "4004",
            "4005",
            "4006",
            "4007",
            "97"
          };
        else
          strArray = new string[5]
          {
            "4000",
            "4001",
            "4002",
            "4003",
            "65"
          };
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (this.session.LoanDataMgr.GetFieldAccessRights(strArray[index]) == BizRule.FieldAccessRight.Hide)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Borrower Contact is not available due to the hidden field '" + strArray[index] + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }
      }
      RxBorrowerContact rxBorrowerContact = new RxBorrowerContact(isCoborrower, true);
      if (rxBorrowerContact.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      if (rxBorrowerContact.GoToContact)
      {
        this.session.MainScreen.NavigateToContact(rxBorrowerContact.SelectedContactInfo);
      }
      else
      {
        if (!isCoborrower)
          this.isBorrowerContactLinked = new bool?();
        else
          this.isCoBorrowerContactLinked = new bool?();
        this.UpdateContents(true, true, false);
        if (this.OnFieldChanged == null)
          return;
        this.OnFieldChanged((object) null, new EventArgs());
      }
    }

    private string findFieldLockID(FieldLock lockCtrl)
    {
      if (lockCtrl.ControlToLock == null)
        return string.Empty;
      EllieMae.Encompass.Forms.Control control = this.currentForm.FindControl(lockCtrl.ControlToLock.ControlID);
      if (control == null)
        return string.Empty;
      FieldControl fieldControl = (FieldControl) control;
      return fieldControl == null ? string.Empty : fieldControl.Field.FieldID;
    }

    public virtual bool oncontrolselect(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool oncopy(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool oncut(mshtml.IHTMLEventObj pEvtObj)
    {
      this.reformatPending = true;
      this.forceChangeEvent = true;
      return true;
    }

    public virtual void ondataavailable(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void ondatasetchanged(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void ondatasetcomplete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool ondblclick(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void ondeactivate(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnDeactivate: " + pEvtObj.srcElement.id);
      this.isDeactivating = true;
    }

    public virtual bool ondrag(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void ondragend(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool ondragenter(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void ondragleave(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool ondragover(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool ondragstart(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual bool ondrop(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onerror(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onerrorupdate(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onfilterchange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onblur(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnBlur: " + pEvtObj.srcElement.id);
    }

    public virtual void onfocus(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnFocus: " + pEvtObj.srcElement.id);
    }

    public virtual void onfocusin(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnFocusIn: " + pEvtObj.srcElement.id + ", TabIndex = " + (object) ((mshtml.IHTMLElement2) pEvtObj.srcElement).tabIndex);
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      if (this.currentField != controlForElement && controlForElement.Interactive)
      {
        this.setCurrentField(controlForElement);
      }
      else
      {
        if (controlForElement.Interactive || this.isMouseClicked)
          return;
        if (this.isTabbingForward)
          controlForElement.MoveNext();
        else
          controlForElement.MovePrevious();
      }
    }

    public virtual void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnFocusOut: " + pEvtObj.srcElement.id);
      if (this.forceChangeEvent)
      {
        this.forceChangeEvent = false;
        if (this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl)
          this.ProcessChangeEvent(pEvtObj);
      }
      if (!this.isDeactivating)
        return;
      this.setCurrentField((FieldControl) null);
    }

    private bool ValidateState(string val)
    {
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) Utils.GetStates());
      if (!(val != string.Empty) || stringSet.Contains(val))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Invalid State Code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public virtual bool onhelp(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onkeydown(mshtml.IHTMLEventObj pEvtObj)
    {
      this.isTabbingForward = !pEvtObj.shiftKey;
      if (pEvtObj.ctrlKey && pEvtObj.keyCode == 68)
      {
        if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !controlForElement.Enabled)
          return;
        if (controlForElement.Field.Format == LoanFieldFormat.DATE || controlForElement.Field.FieldID.StartsWith("TA") && controlForElement.Field.FieldID.EndsWith("DT") || controlForElement.Field.FieldID == "FR0098" || controlForElement.Field.FieldID == "FE0098" || controlForElement.Field.FieldID == "FE0011" || controlForElement.Field.FieldID == "FE0014" || controlForElement.Field.FieldID == "FE0051")
        {
          controlForElement.BindTo(DateTime.Today.ToString("MM/dd/yyyy"));
          this.forceChangeEvent = true;
        }
      }
      if (pEvtObj.keyCode == 46 || pEvtObj.keyCode == 8)
      {
        this.forceChangeEvent = true;
        this.reformatPending = true;
      }
      if (!pEvtObj.ctrlKey || pEvtObj.keyCode != 65)
        return;
      this.SelectAllFields();
    }

    public void SelectAllFields()
    {
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
        this.loan.AddSelectedField(this.parseFieldID(allFieldControl.Field.FieldID));
      this.UpdateContents();
    }

    public void DeselectAllFields()
    {
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
        this.loan.RemoveSelectedField(this.parseFieldID(allFieldControl.Field.FieldID));
      this.UpdateContents();
    }

    public virtual bool onkeypress(mshtml.IHTMLEventObj pEvtObj)
    {
      this.reformatPending = true;
      this.forceChangeEvent = true;
      return true;
    }

    public virtual void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.keyCode == 27)
      {
        this.RefreshContents();
      }
      else
      {
        if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
          return;
        if (controlForElement.Field.FieldID == "CORRESPONDENT.X405" && pEvtObj.keyCode == 189)
          controlForElement.BindTo(controlForElement.Value.Replace("-", string.Empty));
        if (controlForElement.Field.FieldID == "USDA.X120" || controlForElement.Field.FieldID == "USDA.X121" || controlForElement.Field.FieldID == "3238")
        {
          bool needsUpdate = false;
          string str = Utils.FormatInput(controlForElement.Value, FieldFormat.INTEGER, ref needsUpdate);
          if (!needsUpdate)
            return;
          controlForElement.BindTo(str);
        }
        else if (controlForElement.Field.FieldID == "HMDA.X34")
        {
          string str = controlForElement.Value;
          if (str.Length <= (int) byte.MaxValue)
            return;
          controlForElement.BindTo(str.Remove((int) byte.MaxValue));
        }
        else if (controlForElement.Field.FieldID == "HMDA.X49" || controlForElement.Field.FieldID == "HMDA.X55")
        {
          string str = controlForElement.Value;
          if (str.Length <= (int) byte.MaxValue)
            return;
          controlForElement.BindTo(str.Remove((int) byte.MaxValue));
        }
        else
        {
          if (!(controlForElement.Field.FieldID == "MAX23K.X131") && !(controlForElement.Field.FieldID == "MAX23K.X130") && !(controlForElement.Field.FieldID == "MAX23K.X129") && !(controlForElement.Field.FieldID == "MAX23K.X128") && !(controlForElement.Field.FieldID == "MAX23K.X127") && !(controlForElement.Field.FieldID == "MAX23K.X126") && !(controlForElement.Field.FieldID == "MAX23K.X125") && !(controlForElement.Field.FieldID == "MAX23K.X124") || string.IsNullOrEmpty(controlForElement.Value) || !(Utils.ParseDecimal((object) controlForElement.Value) <= 0M))
            return;
          controlForElement.BindTo(controlForElement.Value.Remove(0, 1));
        }
      }
    }

    public virtual void onlayoutcomplete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onload(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onlosecapture(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onmousedown(mshtml.IHTMLEventObj pEvtObj) => this.isMouseClicked = true;

    public virtual void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(pEvtObj.srcElement.tagName == "TEXTAREA") || !(this.currentForm.FindControl(pEvtObj.srcElement.id) is FieldControl control) || !control.Enabled)
        return;
      control.Focus();
    }

    public virtual void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onmousemove(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onmouseout(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onmouseover(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onmouseup(mshtml.IHTMLEventObj pEvtObj) => this.isMouseClicked = false;

    public virtual bool onmousewheel(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onmove(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onmoveend(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onmovestart(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onpage(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onpaste(mshtml.IHTMLEventObj pEvtObj)
    {
      this.reformatPending = true;
      this.forceChangeEvent = true;
      return true;
    }

    public virtual void onpropertychange(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!this.reformatPending)
        return;
      this.reformatPending = false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is ISupportsFormatEvent controlForElement))
        return;
      this.reformatControlValue(controlForElement as FieldControl);
    }

    public virtual void onreadystatechange(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onresize(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onresizeend(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onresizestart(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onrowenter(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onrowexit(mshtml.IHTMLEventObj pEvtObj) => true;

    public virtual void onrowsdelete(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onrowsinserted(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onscroll(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual void onselect(mshtml.IHTMLEventObj pEvtObj)
    {
    }

    public virtual bool onselectstart(mshtml.IHTMLEventObj pEvtObj) => true;

    void HTMLSelectElementEvents2.onchange(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnChange: " + pEvtObj.srcElement.id);
      this.ProcessChangeEvent(pEvtObj);
    }

    bool HTMLInputTextElementEvents2.onchange(mshtml.IHTMLEventObj pEvtObj)
    {
      this.WriteDebug("OnChange: " + pEvtObj.srcElement.id);
      this.forceChangeEvent = true;
      return true;
    }

    protected void ProcessChangeEvent(mshtml.IHTMLEventObj pEvtObj)
    {
      this.ProcessChangeEvent(this.currentForm.FindControlForElement(pEvtObj.srcElement) as FieldControl);
    }

    protected void ProcessChangeEvent(FieldControl ctrl)
    {
      if (ctrl == null)
        return;
      if (InputHandlerBase.StateCodeFields.Contains(ctrl.HelpKey) && !this.ValidateState(ctrl.Value))
      {
        this.SetField(ctrl.HelpKey, "");
        ctrl.Refresh();
      }
      else
      {
        using (Tracing.StartTimer(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Info, "Input Form Field Change, id: " + ctrl.Field.FieldID))
        {
          bool flag1 = true;
          if (ctrl.Field != FieldDescriptor.Empty)
            flag1 = this.syncControlToDataSource(ctrl);
          if (flag1)
          {
            this.executeChangeEvent((RuntimeControl) ctrl);
            if (!this.populateZipCodeRelatedFields(ctrl, ctrl.Value))
              return;
          }
          bool flag2 = this.isControlFieldDirty(ctrl);
          this.refreshContents(ctrl.Field.FieldID, false);
          this.UpdateContents(false, true, false);
          if (flag2)
            return;
          this.populateFieldFromDataSource(ctrl);
        }
      }
    }

    private void populateFeeManagementPickList(PickList pickList)
    {
      switch (this)
      {
        case REGZGFE_2010InputHandler _:
        case HUD1PG2_2010InputHandler _:
        case SECTION32InputHandler _:
        case ULDDInputHandler _:
        case HUD1ESInputHandler _:
          if (!(pickList.BoundControl is EllieMae.Encompass.Forms.TextBox boundControl) || boundControl.GetPickList() != null && boundControl.GetPickList().Options != null && boundControl.GetPickList().Options.Count != 0 || this.feeManagementSetting == null || !this.feeManagementSetting.CompanyOptIn)
            break;
          FeeSectionEnum fieldSectionEnum = HUDGFE2010Fields.GetFieldSectionEnum(boundControl.Field.FieldID);
          if (fieldSectionEnum == FeeSectionEnum.Nothing)
            break;
          FRList frList1 = new FRList(this.feeManagementSetting.GetFeeNames(fieldSectionEnum), false);
          Hashtable dropdownFieldRuleList = this.session.LoanDataMgr != null ? this.session.LoanDataMgr.GetDropdownFieldRuleList() : (Hashtable) null;
          FRList frList2 = dropdownFieldRuleList != null ? (FRList) dropdownFieldRuleList[(object) boundControl.Field.FieldID] : (FRList) null;
          string[] collection = frList1.List;
          if (frList2 != null)
          {
            List<string> stringList1 = new List<string>((IEnumerable<string>) collection);
            List<string> stringList2 = new List<string>();
            for (int index = 0; index < frList2.List.Length; ++index)
            {
              if (stringList1.Contains(frList2.List[index]))
                stringList2.Add(frList2.List[index]);
            }
            collection = stringList2.ToArray();
          }
          if (collection.Length == 0)
            break;
          DropdownOption[] optionList = new DropdownOption[collection.Length];
          for (int index = 0; index < collection.Length; ++index)
            optionList[index] = new DropdownOption(collection[index]);
          pickList.Options.Clear();
          pickList.Options.AddRange((ICollection) optionList);
          boundControl.AttachedPickList(pickList);
          break;
      }
    }

    private void displayPickListOptions(PickList pickList)
    {
      try
      {
        this.populateFeeManagementPickList(pickList);
        pickList.InvokePopup();
      }
      catch (Exception ex)
      {
        this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) pickList, "Popup", ex);
        return;
      }
      if (pickList.Options.Count == 0)
        return;
      int w = Math.Max(((RuntimeControl) pickList.BoundControl ?? (RuntimeControl) pickList).Bounds.Width, 260);
      int x1 = Cursor.Position.X;
      int y1 = Cursor.Position.Y;
      int x2 = x1 - w;
      int y2 = y1 + 10;
      using (FieldRuleDropdownDialog ruleDropdownDialog = new FieldRuleDropdownDialog(pickList.Options.ToArray(), pickList.Title))
      {
        if (ruleDropdownDialog.Height + y2 > System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
          y2 = y1 - ruleDropdownDialog.Height - 40;
        ruleDropdownDialog.SetListBoxWidth(w);
        ruleDropdownDialog.Location = new Point(x2, y2);
        if (ruleDropdownDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        try
        {
          pickList.InvokeItemSelected(ruleDropdownDialog.SelectedItem);
        }
        catch (Exception ex)
        {
          this.displayCustomCodeRuntimeError((EllieMae.Encompass.Forms.Control) pickList, "ItemSelected", ex);
          return;
        }
        if (pickList.BoundControl != null)
        {
          FieldAccessMode accessMode = pickList.BoundControl.AccessMode;
          if (accessMode == FieldAccessMode.ReadOnly)
            pickList.BoundControl.AccessMode = FieldAccessMode.NoRestrictions;
          pickList.BoundControl.BindTo(ruleDropdownDialog.SelectedItem.Value);
          this.ProcessChangeEvent(pickList.BoundControl);
          if (accessMode == FieldAccessMode.NoRestrictions)
            pickList.BoundControl.Focus();
          if (accessMode == FieldAccessMode.ReadOnly)
            pickList.BoundControl.AccessMode = accessMode;
        }
        this.UpdateContents();
      }
    }

    void _IComboFullEvents.OnEnter(string comboName)
    {
      foreach (DropdownEditBox dropdownEditBox in this.currentForm.FindControlsByType(typeof (DropdownEditBox)))
      {
        if (dropdownEditBox.Field.FieldID == comboName && dropdownEditBox.Visible)
        {
          if (this is TAX4506TInputHandler && comboName == "IR0028")
          {
            TAX4506TInputHandler x4506TinputHandler = (TAX4506TInputHandler) this;
            if (this.GetFieldValue("IR" + x4506TinputHandler.CurrentIndex + "93") == "8821" && dropdownEditBox.ControlID == "cboTitleFor8821" || this.GetFieldValue("IR" + x4506TinputHandler.CurrentIndex + "93") != "8821" && dropdownEditBox.ControlID == "cboTitleFor4506")
            {
              this.setCurrentField((FieldControl) dropdownEditBox);
              break;
            }
          }
          else
          {
            if (comboName == "4174" || comboName == "4177" || comboName == "HMDA.X116" || comboName == "HMDA.X118")
            {
              this.setStatusBarInformation((FieldControl) dropdownEditBox);
              dropdownEditBox.Focus();
              break;
            }
            if (comboName == "202" || comboName == "1091" || comboName == "1106" || comboName == "1646")
            {
              if (dropdownEditBox.FieldSource != FieldSource.LinkedLoan)
              {
                this.setCurrentField((FieldControl) dropdownEditBox);
                break;
              }
            }
            else
            {
              this.setCurrentField((FieldControl) dropdownEditBox);
              break;
            }
          }
        }
      }
    }

    void _IComboFullEvents.OnLeave(string comboName)
    {
      foreach (DropdownEditBox ctrl in this.currentForm.FindControlsByType(typeof (DropdownEditBox)))
      {
        if (ctrl.Field.FieldID == comboName && ctrl.Visible)
        {
          if (this is TAX4506TInputHandler && comboName == "IR0028")
          {
            TAX4506TInputHandler x4506TinputHandler = (TAX4506TInputHandler) this;
            if (this.GetFieldValue("IR" + x4506TinputHandler.CurrentIndex + "93") == "8821" && ctrl.ControlID != "cboTitleFor8821" || this.GetFieldValue("IR" + x4506TinputHandler.CurrentIndex + "93") != "8821" && ctrl.ControlID != "cboTitleFor4506")
              continue;
          }
          if (!(comboName == "202") && !(comboName == "1091") && !(comboName == "1106") && !(comboName == "1646") || ctrl.FieldSource != FieldSource.LinkedLoan)
          {
            this.ProcessChangeEvent((FieldControl) ctrl);
            if (this.currentField != ctrl)
              break;
            this.setCurrentField((FieldControl) null);
            break;
          }
        }
      }
    }

    void _IComboFullEvents.OnSelectedIndexChange(
      int index,
      string emid,
      string itemName,
      string itemValue)
    {
      switch (emid)
      {
        case "1959":
          this.UpdateFieldValue(emid, itemValue);
          break;
        case "REQUEST.X25":
          this.UpdateFieldValue(emid, itemValue);
          this.RefreshContents();
          break;
        default:
          this.FlushOutCurrentField();
          break;
      }
      if (!(emid == "FL0008") || this.loan == null || !(this is VOLInputHandler))
        return;
      foreach (DropdownEditBox dropdownEditBox in this.currentForm.FindControlsByType(typeof (DropdownEditBox)))
      {
        if (dropdownEditBox.Field.FieldID == emid && dropdownEditBox.Visible)
        {
          itemValue = dropdownEditBox.Value;
          break;
        }
      }
      ((VOLInputHandler) this).SummaryChanged(emid, itemValue);
    }

    void _IComboFullEvents.OnEditChange(string emid, string comboText)
    {
      this.reformatControlValue(this.currentField);
      if (!(emid == "FL0008") || this.loan == null || !(this is VOLInputHandler))
        return;
      ((VOLInputHandler) this).SummaryChanged(emid, comboText);
    }

    private string parseFieldID(string originalFieldID)
    {
      string fieldId = originalFieldID;
      int length = originalFieldID.IndexOf("|");
      if (length > -1)
        fieldId = originalFieldID.Substring(0, length);
      return fieldId;
    }

    public void Popup(string formName, string title, int width, int height)
    {
      InputFormInfo formInfoByName = this.session.FormManager.GetFormInfoByName(formName);
      if (formInfoByName == (InputFormInfo) null)
        throw new Exception("The form '" + formName + "' is invalid or cannot be accessed by the current user.");
      using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, title, formInfoByName, width, height, FieldSource.CurrentLoan, "", this.session))
      {
        int num = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
      }
      this.UpdateContents(true);
    }

    private void loadVerificationEntryScreen(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight)
    {
      this.LoadQuickLinkForm(formTitle, htmFile, sizeWidth, sizeHeight, FieldSource.CurrentLoan, htmFile);
    }

    public virtual void ExecAction(string action)
    {
      if (this.GetContactItem(action))
        return;
      string str1 = string.Empty;
      if (action.StartsWith("2020baseincome"))
      {
        str1 = action.Replace("2020baseincome", "");
        action = "2020baseincome";
      }
      else if (action.StartsWith("militaryent"))
      {
        str1 = action.Replace("militaryent", "");
        action = "militaryent";
      }
      else if (action.StartsWith("monthlypi"))
      {
        str1 = action;
        action = "monthlypi";
      }
      else if (action.StartsWith("maximumpi"))
      {
        str1 = action;
        action = "maximumpi";
      }
      else if (action.StartsWith("selectcountry_"))
      {
        str1 = action.Replace("selectcountry_", "");
        action = "selectcountry";
      }
      IEPass service = Session.Application.GetService<IEPass>();
      string lower = action.ToLower();
      switch (lower)
      {
        case "1stmor":
          this.showPaymentDialog();
          break;
        case "2010escrowinsurance":
          this.show2010EscrowTitleFeeDialog();
          break;
        case "2010lendertitleinsurance":
          this.show2010LenderTitleFeeDialog();
          break;
        case "2010ownertitleinsurance":
          this.show2010OwnerTitleFeeDialog();
          break;
        case "2010settlementfee":
          this.show2010SettlementFeeDialog();
          break;
        case "2020baseincome":
          this.show2020BaseIncomeDialog(str1);
          break;
        case "203kws":
          this.LoadQuickLinkForm("203k Max Mortgage WS", "MAX23K", 700, 600);
          break;
        case "accesslenders":
          this.sendRequestToePASS(Epass.Lender.Url, false);
          break;
        case "addaiqmessage":
        case "deleteaiqmessage":
          this.handleAIQMessage(action);
          break;
        case "addregistration":
          this.showRegistrationDialog(true);
          break;
        case "aiqanalyzers":
          if (this.loan == null || this.loan.IsInFindFieldForm || this.loan.IsTemplate)
            break;
          this.session.Application.GetService<ILoanEditor>()?.LaunchAIQIncomeAnalyzer();
          break;
        case "allotherpay":
          this.LoadQuickLinkForm("All Other Payments", "VOLPanel/VOMPanel", 688, 512);
          break;
        case "allotherpayments":
          this.LoadQuickLinkForm("All Other Payments", "VOLPanel/VOMPanel", 688, 512);
          break;
        case "altlender":
          this.showAltLenderDialog();
          break;
        case "applymi":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;MIAPPLICATION", false);
          break;
        case "applypartialexemption":
          this.loan.Calculator.FormCalculation("APPLYPARTIALEXEMPTION");
          break;
        case "appraiserlookup":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;APPRAISERLOOKUP", false);
          break;
        case "atrqmcountryregion":
          this.LoadQuickLinkForm("Region", "VATool_Region", 650, 600, FieldSource.CurrentLoan, "ATR/QM Management", true);
          break;
        case "aussnapshot":
          this.showeFolderDocument();
          break;
        case "basecost":
          this.LoadQuickLinkForm("Base Cost", "PREQUAL_BASECOST", 400, 260);
          break;
        case "baseincome":
          this.showBaseIncomeDialog();
          break;
        case "baseincome1":
          this.showBaseIncomeDialog(1);
          break;
        case "baseincome2":
          this.showBaseIncomeDialog(2);
          break;
        case "brokerlookup":
          this.viewLenderRepDialog("broker", this.loan);
          break;
        case "builderlookup":
          WebViewer.OpenURL("https://vip.vba.va.gov/portal/VBAH/VBAHome/buildersearch", "Builder Lookup", 1100, 700);
          break;
        case "buydowndisbursement":
          this.LoadQuickLinkForm("Buydown Disbursement Summary", "Buydownsummary", 650, 350);
          break;
        case "buydowntypelookup":
          this.updateBuydownFields(this.selectBuydownType((List<TemporaryBuydown>) null));
          break;
        case "calcall":
          if (this.loan == null)
            break;
          this.loan.Calculator.CalculateAll(false);
          break;
        case "calculatedividendincome":
          this.calculateDividendIncome();
          break;
        case "calculatehoepaapr":
          this.calculateHOEPAAPR();
          break;
        case "calculatemilitaryallowancesincome":
          this.calculateMilitaryAllowancesIncome();
          break;
        case "calculatemilitaryincome":
          this.calculateMilitaryIncome();
          break;
        case "calculateotherincome":
          this.calculateOtherIncome();
          break;
        case "calculateovertimeincome":
          this.calculateOvertimeIncome();
          break;
        case "cashavailable":
          this.LoadQuickLinkForm("Cash Available", "PREQUAL_ASSETS", 400, 512);
          break;
        case "cashflow":
          this.LoadQuickLinkForm("Cash Flow", "PREQUAL_CASHFLOW", 320, 320);
          break;
        case "cashinfo":
          this.LoadQuickLinkForm("Cash-to-Close", "CASHINFO", 688, 512);
          break;
        case "ccprog":
          this.showClosingCostSelectionDialog(FieldSource.CurrentLoan);
          break;
        case "ccprog2":
          this.showClosingCostSelectionDialog(FieldSource.LinkedLoan);
          break;
        case "checkstatus":
        case "orderssnverification":
          this.showSSNServiceDialog(action);
          break;
        case "cityfee":
          this.showCityFeeDialog();
          break;
        case "cityfeeca":
          this.showCityFeeMLDSDialog();
          break;
        case "clearaltlender":
          this.clearAltLender();
          break;
        case "cleardiscplancode":
          this.clearPlanCode(DocumentOrderType.Opening);
          break;
        case "clearlp":
          this.clearLoanProgram();
          break;
        case "clearplancode":
          this.clearPlanCode(DocumentOrderType.Closing);
          break;
        case "closingcosts":
          if (this.loan == null || this.loan.GetField("NEWHUD.X354") == "Y" || Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
          {
            if (this.loan == null || Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
            {
              this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 750, 600);
              break;
            }
            this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600);
            break;
          }
          this.LoadQuickLinkForm("Closing Costs", "PREQUAL_REGZGFE", 710, 512);
          break;
        case "closinginvestor":
          this.selectClosingInvestor();
          break;
        case "comments0":
          this.editLoanComparisonComments(0);
          break;
        case "comments1":
          this.editLoanComparisonComments(1);
          break;
        case "comments2":
          this.editLoanComparisonComments(2);
          break;
        case "commitmentissuedlookup":
          this.viewLenderRepDialog("commitment", this.loan);
          break;
        case "condlist":
          this.showConditionListDialog();
          break;
        case "copyaddr":
          this.copyAddressFields();
          break;
        case "copyaddrto1098":
          this.copyAddressIRS1098();
          break;
        case "copybrw":
        case "copybrw2":
          this.event_CopyFromBorrower();
          break;
        case "copyeemx75":
          this.copyFieldValuesBetweenForms("1109", "EEM.X75", "EEM.X75");
          break;
        case "copyeemx77":
          this.copyFieldValuesBetweenForms("EEM.X77", "1109", "1109");
          break;
        case "copyformeradd":
          this.event_CopyFormerAddress();
          break;
        case "copyfrom1033":
          if (this.loan == null || this.loan.Calculator == null)
            break;
          this.loan.Calculator.FormCalculation(action, (string) null, (string) null);
          break;
        case "copyfrombordenial":
        case "copyfromcobdenial":
          this.copyAddressInStatementOfDenialScreen(action.ToLower());
          break;
        case "copyfromitemization":
          this.copyFromItemization();
          break;
        case "copyfromsafeharbor":
          this.copySafeHarborToLoan();
          break;
        case "copyhcfrombor":
          this.copyHomeCounselingInfoFromBor();
          break;
        case "copyhefrombor":
          this.copyHomeOwnerEducationInfoFromBor();
          break;
        case "copyitemizationtomlds":
          this.copyTo2010MLDS();
          break;
        case "copymailadd":
          this.event_CopyMailingAddress();
          break;
        case "copymax23kx101":
          this.copyFieldValuesBetweenForms("MAX23K.X101", "1109", "1109");
          break;
        case "copymax23kx29":
          this.copyFieldValuesBetweenForms("MAX23K.X80", "967", "967");
          break;
        case "copymax23kx39":
          this.copyFieldValuesBetweenForms("MAX23K.X101", "1109", "1109");
          break;
        case "copymax23kx47":
          this.copyFieldValuesBetweenForms("MAX23K.X101", "1109", "1109");
          break;
        case "copymax23kx80":
          this.copyFieldValuesBetweenForms("MAX23K.X80", "967", "967");
          break;
        case "copymodifiedterms":
          this.copyModifiedTerms();
          break;
        case "copytohudgfe":
          this.copyToHUDGFE();
          break;
        case "copytosafeharbor":
          this.copyLoanToSafeHarbor();
          break;
        case "copyvoltomlds":
          this.copyVOLtoMLDS(1);
          break;
        case "copyvoltomlds2":
          this.copyVOLtoMLDS(2);
          break;
        case "creditinsurance":
          this.LoadQuickLinkForm("Credit Insurance", "SECTION32_2015_CREDITINSURANCE", 600, 230, FieldSource.CurrentLoan, "Credit Insurance", true);
          break;
        case "detail0":
          this.loadPrequalDetailDialog(0);
          break;
        case "detail1":
          this.loadPrequalDetailDialog(1);
          break;
        case "detail2":
          this.loadPrequalDetailDialog(2);
          break;
        case "detailrequest":
          this.showDetailRequest();
          break;
        case "disaster":
          this.loadVerificationEntryScreen("Disaster", "DISPanel", 750, 600);
          break;
        case "discloseapr":
          this.copyDisclosedAPR();
          break;
        case "discountpoints":
          this.LoadQuickLinkForm("Bona Fide Discount Point Assessment", "ATR_BonaFide", 770, 350, FieldSource.CurrentLoan, "Bona Fide Discount Point Assessment", false);
          break;
        case "docdataviewer":
          this.showClosingDocumentDataViewer();
          break;
        case "docsigningdate":
          this.setDocSignDate();
          break;
        case "editbankruptcyh":
        case "editbankruptcyk":
        case "editbankruptcyl":
        case "editbankruptcym":
          this.showBankruptcyDialog();
          break;
        case "editcheck":
          this.epass.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;EDIT_CHECKS;SINGLE");
          break;
        case "edithudgfe1203":
          this.LoadQuickLinkForm("Transfer Taxes", "REGZGFEHUD_Detail4", 500, 300);
          break;
        case "edithudgfe801":
          this.LoadQuickLinkForm("Our Origination Charge", "REGZGFEHUD_Detail1", 590, 550);
          break;
        case "edithudgfe802":
          if (this.inputData != null && this.inputData.GetField("NEWHUD.X1139") == "Y")
          {
            this.LoadQuickLinkForm("Your Credit or Charge", "REGZGFEHUD_Detail3", 590, 680);
            break;
          }
          this.LoadQuickLinkForm("Your Credit or Charge", "REGZGFEHUD_Detail2", 590, 340);
          break;
        case "editprojectedpaymenttable":
          this.showCustomizedProjectedPaymentTable();
          break;
        case "editpuradvintcal":
          this.showPurchaseAdviceInterestCalculationDialog();
          break;
        case "editregistration":
          this.showRegistrationDialog(false);
          break;
        case "editsupplementalpropertyinsurance":
          this.showOtherExpenseDialog("URLA.X144");
          break;
        case "eds":
          this.loan.Calculator.FormCalculation("EDS");
          break;
        case "enerfyimprove":
          this.LoadQuickLinkForm("Energy Efficient Mortgage Calculation", "EEMCALCULATION", 600, 600);
          break;
        case "escrowtable":
          this.showTitleEscrowTable("Escrow", "387");
          break;
        case "existing23kdebt":
          this.LoadQuickLinkForm("203k Existing Debt", "FHA_Existing203kDebt", 530, 380, FieldSource.CurrentLoan, "203K Existing Debt", false);
          break;
        case "exportulddfannie":
          this.exportULDD("Fannie");
          break;
        case "exportulddfreddie":
          this.exportULDD("Freddie");
          break;
        case "exportulddginnie":
          this.exportULDD("GinnieMaePdd12.161");
          break;
        case "fanniesfclookup":
          this.specialFeatureCodeLookup("Fannie Mae");
          break;
        case "feedetails":
          if (this.loan.Use2015RESPA)
          {
            this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 750, 600, FieldSource.CurrentLoan, "2015 Itemization", false);
            break;
          }
          this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600, FieldSource.CurrentLoan, "2010 Itemization", true);
          break;
        case "fhacashworksheet":
          this.LoadQuickLinkForm("FHA Maximum Mortgage And Cash Needed Worksheet", "FHACASHWORKSHEET", 688, 512);
          break;
        case "fhamaxloan":
          this.performFHAMaxLoanCalc();
          break;
        case "fhavalookup":
          this.viewLenderRepDialog("fhaVa", this.loan);
          break;
        case "fhavamortageelookup":
          this.viewLenderRepDialog("fhaVaMortgagee", this.loan);
          break;
        case "freddiesfclookup":
          this.specialFeatureCodeLookup("Freddie Mac");
          break;
        case "geocode":
          this.performGeocoding();
          break;
        case "getami":
          this.loan.Calculator.GetAMILimits(true);
          break;
        case "getindex":
          this.getindex();
          break;
        case "getlatefee":
          this.getLateFees();
          break;
        case "getmfi":
          this.loan.Calculator.GetMFILimits(true);
          break;
        case "getpricing":
          this.SetField("4060", "");
          this.SetField("4069", "");
          this.execFormAction("getpricingfromppe");
          break;
        case "getpricingfromppe":
          this.getPricingFromOptimalBlue("getpricing");
          break;
        case "getresidualincome":
          this.getResidualIncome();
          break;
        case "gotosection32":
          this.LoadQuickLinkForm("Section 32 HOEPA", "SECTION32", 640, 560);
          break;
        case "haz":
        case "hazins":
          this.showHazardInsuranceDialog("Hazard Insurance");
          break;
        case "helponnewhudseller":
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.session.MainForm, SystemSettings.HelpFile, "ManagingSplitFees");
          break;
        case "helponnewhudseller2015":
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.session.MainForm, SystemSettings.HelpFile, "ManagingSplitFees2015");
          break;
        case "helponpoc":
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.session.MainForm, SystemSettings.HelpFile, "paidoutsideclosing");
          break;
        case "helponunmarriedaddendumurla1003":
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.session.MainForm, SystemSettings.HelpFile, "UnmarriedAddendum");
          break;
        case "helponurla2020printadditionalborrower":
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.session.MainForm, SystemSettings.HelpFile, "URLA2020PrintAdditionalBorrower");
          break;
        case "heoblig":
          this.LoadQuickLinkForm("Subject Housing Expense", "PREQUAL_PRIEXPENSE", 400, 300);
          break;
        case "hid":
          this.showHelocCalculatorDialog(FieldSource.CurrentLoan);
          break;
        case "hidl":
          this.showHelocCalculatorDialog(FieldSource.LinkedLoan);
          break;
        case "historicalindex":
          this.selectHELOCHistoricalIndex();
          break;
        case "hmda":
          this.LoadQuickLinkForm("HMDA Information", "HMDA_DENIAL", 750, 600, FieldSource.CurrentLoan, "HMDA Information", true);
          break;
        case "hmdatransmittalsheet":
          this.LoadQuickLinkForm("HMDA Transmittal Sheet", "HMDATransmittal_2018", 700, 550, FieldSource.CurrentLoan, "HMDA Information 2018", false);
          break;
        case "holdingsfha":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;FHACASEHOLDINGSVIEW", false);
          break;
        case "hud1a":
          this.showHUD1ADialog();
          break;
        case "hud1escitytaxsetup":
          this.LoadQuickLinkForm("City Property Tax", "HUD1ES_CityTaxInfo", 600, 300);
          break;
        case "hud1escustomsetup1":
          this.LoadQuickLinkForm("Custom Escrow #1", "HUD1ES_Custom1Info", 600, 500);
          break;
        case "hud1escustomsetup2":
          this.LoadQuickLinkForm("Custom Escrow #2", "HUD1ES_Custom2Info", 600, 500);
          break;
        case "hud1escustomsetup3":
          this.LoadQuickLinkForm("Custom Escrow #3", "HUD1ES_Custom3Info", 600, 500);
          break;
        case "hud1esfloodsetup":
          this.LoadQuickLinkForm("Flood Insurance", "HUD1ES_FloodInfo", 600, 300);
          break;
        case "hud1eshazardsetup":
          this.LoadQuickLinkForm("Hazard Insurance", "HUD1ES_HazardInfo", 600, 300);
          break;
        case "hud1estaxsetup":
          this.LoadQuickLinkForm("Tax", "HUD1ES_TaxInfo", 600, 300);
          break;
        case "hud92900lt":
          this.LoadQuickLinkForm("FHA Loan Transmittal", "HUD92900LT", 688, 512);
          break;
        case "hudsetup":
        case "setup":
          this.showHUD1ESSetupDialog();
          break;
        case "importfundingdetails":
          this.importFundingDetails();
          break;
        case "importincome":
          this.importIncomeToATRQM();
          break;
        case "importliab":
          this.importLiabilities();
          break;
        case "income":
          this.LoadQuickLinkForm("Monthly Income", "PREQUAL_INCOME", 480, 580);
          break;
        case "incomeexpense":
          this.LoadQuickLinkForm("Income & Expenses", "FPMS_IncomeExpense", 600, 420);
          break;
        case "intenttoproceed2015":
          this.loan.Calculator.GetIntentToProceedIDisclosureTracking2015Log(this.showDisclosedLELogs(false, false));
          break;
        case "investor":
          this.showInvestorListDialog();
          break;
        case "latefeedetails":
          this.showLateFeeDetails();
          break;
        case "legaldescription":
          WebViewer.OpenURL("http://publicrecords.netronline.com", "Legal Description", 1100, 700);
          break;
        case "lenderbrokerdata":
          if (this.loan == null || this.loan.Calculator == null)
            break;
          if (this.loan.GetField("1264") == "" && this.loan.GetField("VEND.X293") == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Please enter Lender/Broker info.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          this.loan.Calculator.FormCalculation("LENDERBROKERDATA", (string) null, (string) null);
          break;
        case "lendercoverage":
          this.setCoverageFee(true);
          break;
        case "lenderlookup":
          this.viewLenderRepDialog("lender", this.loan);
          break;
        case "lenderreplookup":
          this.viewLenderRepDialog("lenderRep", this.loan);
          break;
        case "lifedetail":
          this.sendRequestToePASS("https://rbcins.elliemaeservices.com/sf/quote.aspx", true);
          break;
        case "loanoriginatorlookup":
          this.viewLenderRepDialog("loanOriginator", this.loan);
          break;
        case "loanprog":
        case "loanprog1":
          this.showLoanProgramSelectionDialog(FieldSource.CurrentLoan);
          break;
        case "loanprog2":
          this.showLoanProgramSelectionDialog(FieldSource.LinkedLoan);
          break;
        case "loanprogrequest":
          this.showLoanProgramSelectionDialog(FieldSource.CurrentLoan, true);
          break;
        case "localtax":
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.LocalTax);
          break;
        case "locksubfin":
          this.openSubordinateFinancingDialog(FieldSource.CurrentLoan, true);
          break;
        case "locompensationtool":
          this.LoadQuickLinkForm("Manage Adjusted Origination Charge Details", "REGZGFE_2010_LOCOMP", 800, 512, FieldSource.CurrentLoan, "LO Compensation Tool");
          break;
        case "logappraisal":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;APPRAISALLOGGING", false);
          break;
        case "lookup3169":
          this.showField3169Options();
          break;
        case "lookup3169m":
          this.showFieldCoCOptions("3169", "LE1.X86", "LE1.X90");
          break;
        case "lookup4970m":
          this.viewAreaMedianIncomDialog();
          break;
        case "lookupcd1x64m":
          this.showFieldCoCOptions("CD1.X64", "CD1.X65", "CD1.X70");
          break;
        case "lookuplocomp":
          this.lookupBrokerLOComp(FieldSource.CurrentLoan);
          break;
        case "lookuptpobranch":
          this.lookupTPOBranch();
          break;
        case "lookuptpobranchdbaname":
          this.lookupTPOBranchDBAName();
          break;
        case "lookuptpodbaname":
          this.lookupTPODBAName();
          break;
        case "lookuptpoloanofficer":
          this.lookupTPOLoanOfficer();
          break;
        case "lookuptpoorganization":
          this.lookupTPOOrganization();
          break;
        case "manageborrowers":
          this.showSwapBorrowerPairDialog();
          break;
        case "maximumpi":
        case "monthlypi":
          this.showMPICalculatorDialog(str1);
          break;
        case "maxloanamount":
          if (this.loan == null)
            break;
          this.loan.Calculator.CalculateMaxLoanAmt();
          this.LoadQuickLinkForm("Maximum Loan Amount", "PREQUAL_MAXLOANAMOUNT", 400, 300);
          break;
        case "mcawtotalcc":
          if (this.loan != null && !this.loan.Use2010RESPA && !this.loan.Use2015RESPA)
          {
            this.LoadQuickLinkForm("GFE - Itemization", "REGZGFE", 700, 600);
            break;
          }
          if (this.loan != null && this.loan.Use2015RESPA)
          {
            this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 750, 600, FieldSource.CurrentLoan, "2015 Itemization");
            break;
          }
          this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600, FieldSource.CurrentLoan, "2010 Itemization");
          break;
        case "mersmin":
          this.showMersMIN();
          break;
        case "mfilookup":
          this.viewMedianFamilyIncomeDialog();
          break;
        case "militaryent":
          this.showMilitaryEntitlementDialog(str1);
          break;
        case "mipff":
        case "mtgins":
          this.showMIPDialog(FieldSource.CurrentLoan);
          break;
        case "mipff2":
          this.showMIPDialog(FieldSource.LinkedLoan);
          break;
        case "mipusda":
          this.showMIPDialog(FieldSource.CurrentLoan, true);
          break;
        case "monthlyhousingexpenses":
          this.LoadQuickLinkForm("Monthly Housing Expenses", "D1003_2020MonthlyHousingExpenses", 450, 420);
          break;
        case "mortg":
          this.showMIPDialog(FieldSource.CurrentLoan);
          break;
        case "mtginsprem":
          this.showMIPDialog(FieldSource.CurrentLoan);
          break;
        case "mtginspremca":
          this.showMIPDialog(FieldSource.CurrentLoan);
          break;
        case "mtginsreserv":
          this.showMIPDialog(FieldSource.CurrentLoan);
          break;
        case "nmls":
          this.showNMLSDialog();
          break;
        case "obtaincaivrs":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;CAIVRSNUMBER", false);
          break;
        case "obtainfha":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;FHACASENUMBER", false);
          break;
        case "ocredit":
        case "ordercredit":
          this.sendRequestToePASS(Epass.Credit.Url, false);
          break;
        case "openitemization":
          if (Utils.CheckIf2015RespaTila(this.inputData.GetField("3969")))
          {
            this.LoadQuickLinkForm("2015 Itemization", "REGZGFE_2015", 800, 600, FieldSource.CurrentLoan, "2015 Itemization");
            break;
          }
          this.LoadQuickLinkForm("2010 Itemization", "REGZGFE_2010", 750, 600, FieldSource.CurrentLoan, "2010 Itemization");
          break;
        case "orderappraisal":
          this.sendRequestToePASS(Epass.Appraisal.Url, false);
          break;
        case "orderdatatrac":
          this.orderDataTracService();
          break;
        case "orderflood":
          this.orderFloodService();
          break;
        case "orderfraud":
          this.orderFraudService();
          break;
        case "orderlife":
          this.sendRequestToePASS("https://rbcins.elliemaeservices.com/sf/order.aspx", true);
          break;
        case "orderrefi":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;REFIAUTHORIZATION", false);
          break;
        case "ordertitle":
          this.sendRequestToePASS(Epass.Title.Url, false);
          break;
        case "other":
          this.showOtherExpenseDialog("234");
          break;
        case "otherf":
          this.showOtherFinancingDialog();
          break;
        case "otherincome":
          this.LoadQuickLinkForm("Additional Liabilities", "PREQUAL_OTHERINCOME", 480, 300);
          break;
        case "otherpayoffs":
          this.LoadQuickLinkForm("Other Payoffs", "VOLPanel", 700, 512);
          break;
        case "othersummaries":
          this.LoadQuickLinkForm("Other Summaries", "POPUP_OTHERSUMMARIES", 450, 350);
          break;
        case "ownercoverage":
          this.setCoverageFee(false);
          break;
        case "ownerins":
          this.showHazardInsuranceDialog("Homeowner's Insurance");
          break;
        case "paymentexample":
          this.loan.Calculator.FormCalculation("IRPCEXAMPLE");
          this.LoadQuickLinkForm("Payment Example", "PaymentExample", 800, 360);
          break;
        case "payoffmortgages":
          this.LoadQuickLinkForm("Payoff Mortgages", "VOLPanel/VOMPanel", 688, 512);
          break;
        case "plancode":
          this.showPlanCodeDialog();
          break;
        case "plancodecompare":
          this.showPlanCodeConflictDialog();
          break;
        case "popup_adjustmentscreditdetails":
          this.LoadQuickLinkForm("Adjustments and Other Credits Details", "Popup_AdjustmentsBorrowerCreditsUCD", 1020, 580, FieldSource.CurrentLoan, "Adjustments and Other Credits Details", false);
          break;
        case "popup_adjustmentsellercredit":
          this.LoadQuickLinkForm("Adjustments and Other Credits Details", "Popup_AdjustmentsSellerCreditsUCD", 840, 600, FieldSource.CurrentLoan, "Adjustments and Other Credits Details", false);
          break;
        case "prepayment":
          this.LoadQuickLinkForm("Prepayment Penalty", "REGZ50CLOSER_PREPAYMENT", 550, 390);
          break;
        case "prequal_vod":
        case "vod":
          this.loadVerificationEntryScreen("VOD", "VODPanel", 688, 512);
          break;
        case "primaryexpense":
          this.LoadQuickLinkForm("Proposed Housing Expenses", "PREQUAL_PRIEXPENSE", 400, 480);
          break;
        case "productandpricing":
          this.SetField("OPTIMAL.REQUEST", "");
          this.SetField("OPTIMAL.RESPONSE", "");
          this.sendRequestToePASS("_EPASS_SIGNATURE;EPASSAI;2;;SOURCE_FORM=QALO;Product+and+Pricing", false);
          break;
        case "profit&losstotals":
          this.LoadQuickLinkForm("Profit & Loss Totals", "FM1084B_ProfitLoss", 600, 400, FieldSource.CurrentLoan, "Self-Employed Income 1084", true);
          break;
        case "ratespread":
          this.processRateSpread();
          break;
        case "recalculatehmda":
          this.loan.Calculator.FormCalculation("recalculatehmda");
          break;
        case "recordingfee":
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.RecordingFee);
          break;
        case "refreshfromsetting":
          this.resetFHAConsumerChoiceSetting();
          break;
        case "regz":
          if (this.loan != null && this.loan.Use2015RESPA)
          {
            this.LoadQuickLinkForm("RegZ", "REGZLE", 730, 512);
            break;
          }
          this.LoadQuickLinkForm("REGZ - TIL", "PREQUAL_REGZ50", 688, 512);
          break;
        case "regz-tilaudit":
          if (this.loan == null || this.loan.IsInFindFieldForm || this.loan.IsTemplate)
            break;
          QuickButtonsControl.TriggerCalcAll();
          if (!this.editor.ShowRegulationAlerts() || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILAudit", this.loan))
            break;
          service.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;AUDIT", false);
          this.editor.RefreshContents("1885");
          break;
        case "regz-tileclose":
          if (this.loan == null || this.loan.IsInFindFieldForm || this.loan.IsTemplate || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILeClose", this.loan))
            break;
          Session.Application.GetService<IEFolder>().LaunchEClose(Session.LoanDataMgr);
          break;
        case "regz-tilorder docs":
          if (this.loan == null || this.loan.IsInFindFieldForm || this.loan.IsTemplate)
            break;
          QuickButtonsControl.TriggerCalcAll();
          if (!this.editor.ShowRegulationAlertsOrderDoc() || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILOrder Docs", this.loan))
            break;
          service.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;PROCESS", false);
          this.editor.RefreshContents();
          break;
        case "regz-tilview":
          if (this.loan == null || this.loan.IsInFindFieldForm || this.loan.IsTemplate || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("Button_REGZ-TILView", this.loan))
            break;
          service.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;VIEWDOCS", false);
          break;
        case "requestextension":
          this.extensionRequest();
          break;
        case "requestlock":
          this.sendLockRequest("requestlock");
          break;
        case "requestlockcancellation":
          this.lockCancellationRequest();
          break;
        case "resetcomp":
          this.updateLOCompensation("", "");
          break;
        case "resetrequestform":
          this.resetRequestForm();
          break;
        case "retaxes":
          this.showRETaxesDialog();
          break;
        case "safeharboroption1":
        case "safeharboroption2":
        case "safeharboroption3":
        case "safeharboroption4":
          this.LoadQuickLinkForm("Anti-Steering Safe Harbor Disclosure", action, 620, 512);
          break;
        case "searchcondo":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;CONDOPUDLOOKUP", false);
          break;
        case "searchgsa":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;GSASEARCH", false);
          break;
        case "searchldp":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;LDPSEARCH", false);
          break;
        case "selectcountry":
          this.selectCountryName(str1);
          break;
        case "selecttemplate":
          this.selectPurchaseAdviceTemplate();
          break;
        case "showalternatenamesborrower":
          this.showAlternatenamesDialog(true);
          break;
        case "showalternatenamescoborrower":
          this.showAlternatenamesDialog(false);
          break;
        case "showbestcontactdaytime_borrower":
          this.LoadQuickLinkForm("Borrower Contact Preference", "BestContactDayTime_Borrower", 450, 420);
          break;
        case "showbestcontactdaytime_coborrower":
          this.LoadQuickLinkForm("Co-Borrower Contact Preference", "BestContactDayTime_CoBorrower", 450, 420);
          break;
        case "showdisclosedcdlogscanchange":
          this.showDisclosedCDLogsCanChange();
          break;
        case "showdisclosedcdlogscannotdecrease":
          this.showDisclosedCDLogsCannotDecrease();
          break;
        case "showdisclosedcdlogscannotincrease":
          this.showDisclosedCDLogsCannotIncrease();
          break;
        case "showdisclosedcdlogscannotincrease10":
          this.showDisclosedCDLogsCannotIncrease10();
          break;
        case "showdisclosedlelogscanchange":
          this.showDisclosedLELogsCanChange();
          break;
        case "showdisclosedlelogscannotdecrease":
          this.showDisclosedLELogsCannotDecrease();
          break;
        case "showdisclosedlelogscannotincrease":
          this.showDisclosedLELogsCannotIncrease();
          break;
        case "showdisclosedlelogscannotincrease10":
          this.showDisclosedLELogsCannotIncrease10();
          break;
        case "showhomecounsel":
          this.LoadQuickLinkForm("Home Counseling Providers", "HomeCounselingProviders", 920, 700, FieldSource.CurrentLoan, "Home Counseling Providers", false);
          break;
        case "showpaymentsandpayoffsdialog":
          this.showPaymentsAndPayoffsDialog();
          break;
        case "showqmpaymentstream":
          this.viewQMPaymentSchedule();
          break;
        case "statefee":
          this.showStateFeeDialog();
          break;
        case "statefeeca":
          this.showStateFeeMLDSDialog();
          break;
        case "statetax":
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.StateTax);
          break;
        case "subfin":
          this.openSubordinateFinancingDialog(FieldSource.CurrentLoan, false);
          break;
        case "subfin2":
          this.openSubordinateFinancingDialog(FieldSource.LinkedLoan, false);
          break;
        case "subfinheloc":
          this.openSubordinateHELOCFinancingDialog(FieldSource.CurrentLoan);
          break;
        case "syncbrw":
          this.showContactSyncDialog();
          break;
        case "taxes":
          this.showTaxesDialog();
          break;
        case "taxesreserv":
          this.showTaxesReservedDialog();
          break;
        case "titletable":
          this.showTitleEscrowTable("Title", "385");
          break;
        case "totalcosts":
          this.LoadQuickLinkForm("Total Costs", "PREQUAL_TOTALCOSTS", 400, 358);
          break;
        case "totalfinancing":
          this.LoadQuickLinkForm("Total Financing", "PREQUAL_TOTALFINANCING", 460, 480);
          break;
        case "totalmonthlypayment":
          this.LoadQuickLinkForm("Total Monthly Payment", "BorrowerSummary_TotalPayment", 360, 420);
          break;
        case "transferto":
          this.showTransferToDialog();
          break;
        case "trusteedb":
          this.showTrusteeDatabase();
          break;
        case "updatebalance":
        case "updatecorrespondentbalance":
        case "updatecorrespondentfees":
          this.showUpdatePurchaseAdvice(action.ToLower());
          break;
        case "updatebrokerlicense":
        case "updateinvestorlicense":
        case "updatelenderlicense":
          this.updateClosingVendorLicense(action.ToLower());
          break;
        case "updatelinkedborrower":
          this.updateLinkedBorrowerContact(false);
          break;
        case "updatelinkedcoborrower":
          this.updateLinkedBorrowerContact(true);
          break;
        case "uploadcasebinder":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;CASEBINDER", false);
          break;
        case "url_freddie":
          Process.Start("https://cfpb.github.io/hmda-platform-tools/rate-spread/");
          break;
        case "urla2020acknowledgmentsandagreements":
          this.LoadQuickLinkForm("URLA 2020 Acknowledgments and Agreements", "D1003_2020ACKNOWLEDGEMENT", 920, 920, FieldSource.CurrentLoan, "URLA 2020 Acknowledgments and Agreements", false);
          break;
        case "usdaincomelink":
          WebViewer.OpenURL("https://www.rd.usda.gov/files/RD-GRHLimitMap.pdf", "Moderate Income Limit", 810, 500);
          break;
        case "usdalookup":
          this.viewLenderRepDialog("usda", this.loan);
          break;
        case "usdatotalincome":
          this.LoadQuickLinkForm("USDA Household Income", "USDA_TOTALINCOME", 790, 850);
          break;
        case "userfee1":
          this.showUserFee1Dialog();
          break;
        case "userfee1ca":
          this.showUserFee1MLDSDialog();
          break;
        case "userfee2":
          this.showUserFee2Dialog();
          break;
        case "userfee2ca":
          this.showUserFee2MLDSDialog();
          break;
        case "userfee3":
          this.showUserFee3Dialog();
          break;
        case "userfee3ca":
          this.showUserFee3MLDSDialog();
          break;
        case "vacountryregion":
          this.LoadQuickLinkForm("Region", "VATool_Region", 650, 600, FieldSource.CurrentLoan, "VA Management", true);
          break;
        case "vaguidelines":
          WebViewer.OpenURL("https://www.benefits.va.gov/warms/pam26_7.asp", "VA Guidelines", 1100, 700, true);
          break;
        case "validateaddress":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;ADDRESSVALIDATION", false);
          break;
        case "validateulddexportfannie":
          this.ulddExportChecking(true, true, "Fannie");
          break;
        case "validateulddexportfreddie":
          this.ulddExportChecking(true, true, "Freddie");
          break;
        case "validateulddexportginnie":
          this.ulddExportChecking(true, true, "GinnieMaePdd12.161");
          break;
        case "vcredit":
        case "viewcredit":
          this.epass.View(Epass.Credit.FullName);
          break;
        case "viewexampleschedules":
          this.viewHELOCExamplePaymentSchedules();
          break;
        case "viewfha":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;FHACASENUMBERVIEW", false);
          break;
        case "viewmi":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;MIAPPLICATIONVIEW", false);
          break;
        case "viewscorecard":
          this.sendRequestToePASS("_EPASS_SIGNATURE;FHACHUMS;2;TOTALSCORE", false);
          break;
        case "viewtpobranch":
          this.viewTPOExternalOrganizationInfo(true);
          break;
        case "viewtpocompany":
          this.viewTPOExternalOrganizationInfo(false);
          break;
        case "viewtradesummary":
          this.viewTradeSummaryDialog();
          break;
        case "viewworstcase":
          this.viewWorstCaseScenarioPayment();
          break;
        case "viewworstcaseinregz":
          this.viewWorstCaseScenarioPaymentInRegz();
          break;
        case "voal":
          this.loadVerificationEntryScreen("VOAL", "VOALPanel", 688, 512);
          break;
        case "voe":
          this.loadVerificationEntryScreen("VOE", "VOEPanel", 688, 512);
          break;
        case "vogg":
          this.loadVerificationEntryScreen("VOGG", "VOGGPanel", 688, 512);
          break;
        case "vol":
          this.loadVerificationEntryScreen("VOL", "VOLPanel", 900, 512);
          break;
        case "vom":
          this.loadVerificationEntryScreen("VOM", "VOMPanel", 688, 512);
          break;
        case "vooa":
          this.loadVerificationEntryScreen("VOOA", "VOOAPanel", 688, 512);
          break;
        case "vooi":
          this.loadVerificationEntryScreen("VOOI", "VOOIPanel", 688, 512);
          break;
        case "vool":
          this.loadVerificationEntryScreen("VOOL", "VOOLPanel", 688, 512);
          break;
        case "vor":
          this.loadVerificationEntryScreen("VOR", "VORPanel", 688, 512);
          break;
        case "warehousebank":
          this.selectWarehouseBank();
          break;
        case "zoomadlsigvbg":
          this.ShowFieldOptions("PlanCode.AdtlSgVbgTyp");
          break;
        case "zoomarm":
          this.showARMTypeDetails(false);
          break;
        case "zoomarmrequest":
          this.showARMTypeDetails(true);
          break;
        case "zoomflrvbg":
          this.ShowFieldOptions("ARM.FlrVerbgTyp");
          break;
        case "zoomprepayvbg":
          this.ShowFieldOptions("Terms.PrepyVrbgTyp");
          break;
        case null:
          break;
        default:
          string str2 = lower;
          if (str2.StartsWith("attachliens"))
          {
            if (str2.Length <= 11)
              break;
            string str3 = str2.Substring(11);
            int result = 0;
            if (!int.TryParse(str3, out result) || !this.attachLiensToVOM(str3))
              break;
            this.RefreshContents();
            break;
          }
          if (!lower.StartsWith("bestcontactdaytime_nboc"))
            break;
          this.LoadQuickLinkForm("Non-Borrowing Owner Contact Preference", action, 450, 420);
          break;
      }
    }

    private void handleAIQMessage(string action)
    {
      if (Session.ServerLicense.ClientID != "3010000024" && Session.ServerLicense.ClientID != "3011220192" && Session.ServerLicense.ClientID != "3011220390")
        return;
      EPassMessageInfo[] epassMessagesForLoan = Session.ConfigurationManager.GetEPassMessagesForLoan(this.loan.GUID);
      if (action.ToLower() == "addaiqmessage" && epassMessagesForLoan != null && epassMessagesForLoan.Length != 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "This loan already contains a green alert for Analyzer Income! You cannot add another ePASS Message for Analyzers!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (action.ToLower() == "deleteaiqmessage" && (epassMessagesForLoan == null || epassMessagesForLoan.Length == 0))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "This loan doesn't have any green alert for Analyzer Income.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (action.ToLower() == "addaiqmessage")
      {
        string str = this.loan.GUID;
        if (str.StartsWith("{"))
          str = str.Substring(1);
        if (str.EndsWith("}"))
          str = str.Substring(0, str.Length - 1);
        int hashCode = (str + ":AIQ.Income").GetHashCode();
        string msgXml = "<MSG ID=\"" + hashCode.ToString() + "\" ActionType=\"AIQ.INCOME\" LoanUID=\"" + this.loan.GUID + "\"></MSG>";
        try
        {
          Session.ConfigurationManager.UpsertEPassMessage(new EPassMessageInfo(hashCode.ToString(), "income", "Income Analyzer", this.loan.GUID, (string) null, "Income analyzer reported issues", DateTime.Now, true, msgXml));
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The Income Analyzer alert cannot be added due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        int num4 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "A new Income Analyzer alert is added successfully! Please close the loan and re-open the loan to get the green alert on alert panel.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        List<string> stringList = new List<string>();
        for (int index = 0; index < epassMessagesForLoan.Length; ++index)
          stringList.Add(epassMessagesForLoan[index].MessageID);
        try
        {
          Session.ConfigurationManager.DeleteEPassMessages(stringList.ToArray());
        }
        catch (Exception ex)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The Income Analyzer alert cannot be deleted due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        int num6 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "All Income Analyzer alerts are deleted successfully! Please close the loan and re-open the loan again!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void showAlternatenamesDialog(bool isBorrower)
    {
      using (AlternateNamesDialog alternateNamesDialog = new AlternateNamesDialog(this.loan, this.mainScreen, isBorrower))
      {
        if (alternateNamesDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    internal TemporaryBuydown selectBuydownType(List<TemporaryBuydown> buydownTemplates)
    {
      try
      {
        if (buydownTemplates == null)
          buydownTemplates = this.session.TemporaryBuydownTypeBpmManager.GetAllTemporaryBuydowns();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass cannot load buydown templates from setting due to this error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (TemporaryBuydown) null;
      }
      if (buydownTemplates == null || buydownTemplates.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "You do not have any buydown template available in setting.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (TemporaryBuydown) null;
      }
      using (TemporaryBuydownSelectDialog buydownSelectDialog = new TemporaryBuydownSelectDialog(buydownTemplates))
      {
        if (buydownSelectDialog.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
          return buydownSelectDialog.SelectedBuydownTemplate;
      }
      return (TemporaryBuydown) null;
    }

    private void updateBuydownFields(TemporaryBuydown buyDownTemplate)
    {
      if (buyDownTemplate == null)
        return;
      string field = this.GetField("CASASRN.X141");
      this.inputData.SetCurrentField("4645", buyDownTemplate.BuydownType);
      this.UpdateFieldValue("4645", buyDownTemplate.BuydownType);
      if (field.Equals("Borrower"))
      {
        string val = "";
        for (int index1 = 1; index1 <= 2; ++index1)
        {
          int num1 = index1 == 1 ? 1269 : 1613;
          int num2 = index1 == 1 ? 1274 : 1618;
          for (int index2 = num1; index2 <= num2; ++index2)
          {
            switch (index2)
            {
              case 1269:
                val = buyDownTemplate.Rate1;
                break;
              case 1270:
                val = buyDownTemplate.Rate2;
                break;
              case 1271:
                val = buyDownTemplate.Rate3;
                break;
              case 1272:
                val = buyDownTemplate.Rate4;
                break;
              case 1273:
                val = buyDownTemplate.Rate5;
                break;
              case 1274:
                val = buyDownTemplate.Rate6;
                break;
              case 1613:
                val = buyDownTemplate.Term1;
                break;
              case 1614:
                val = buyDownTemplate.Term2;
                break;
              case 1615:
                val = buyDownTemplate.Term3;
                break;
              case 1616:
                val = buyDownTemplate.Term4;
                break;
              case 1617:
                val = buyDownTemplate.Term5;
                break;
              case 1618:
                val = buyDownTemplate.Term6;
                break;
            }
            if (val != "")
            {
              if (!this.inputData.IsLocked(string.Concat((object) index2)))
                this.inputData.AddLock(string.Concat((object) index2));
              this.inputData.SetCurrentField(string.Concat((object) index2), val);
            }
            else if (this.inputData.IsLocked(string.Concat((object) index2)))
              this.inputData.RemoveLock(string.Concat((object) index2));
          }
        }
      }
      else
      {
        this.inputData.SetCurrentField("4535", buyDownTemplate.Rate1);
        this.inputData.SetCurrentField("4541", buyDownTemplate.Term1);
        this.inputData.SetCurrentField("4536", buyDownTemplate.Rate2);
        this.inputData.SetCurrentField("4542", buyDownTemplate.Term2);
        this.inputData.SetCurrentField("4537", buyDownTemplate.Rate3);
        this.inputData.SetCurrentField("4543", buyDownTemplate.Term3);
        this.inputData.SetCurrentField("4538", buyDownTemplate.Rate4);
        this.inputData.SetCurrentField("4544", buyDownTemplate.Term4);
        this.inputData.SetCurrentField("4539", buyDownTemplate.Rate5);
        this.inputData.SetCurrentField("4545", buyDownTemplate.Term5);
        this.inputData.SetCurrentField("4540", buyDownTemplate.Rate6);
        this.inputData.SetCurrentField("4546", buyDownTemplate.Term6);
      }
      if (!(this.inputData is LoanData))
        return;
      this.loan.Calculator.FormCalculation(field.Equals("Borrower") ? "1269" : "4535");
      this.loan.Calculator.FormCalculation("4645");
    }

    private void selectHELOCHistoricalIndex()
    {
      using (SelectHelocTableForm selectHelocTableForm = new SelectHelocTableForm(true))
      {
        if (selectHelocTableForm.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        HelocRateTable helocTable = (HelocRateTable) Session.ConfigurationManager.GetHelocTable(selectHelocTableForm.TableName);
        if (helocTable == null)
          return;
        if (this.loan != null)
        {
          if (!this.loan.SetDrawRepayPeriod(helocTable))
            return;
          this.loan.SetCurrentField("1985", selectHelocTableForm.TableName);
          this.loan.SetCurrentField("4630", helocTable.IsNewHELOC ? "Y" : "");
          this.UpdateContents(true);
        }
        else
        {
          this.inputData.SetCurrentField("1985", selectHelocTableForm.TableName);
          this.inputData.SetCurrentField("4630", helocTable.IsNewHELOC ? "Y" : "");
        }
      }
    }

    private void showBankruptcyDialog()
    {
      using (AddBankruptcyDialog bankruptcyDialog = new AddBankruptcyDialog(this.inputData, this.session))
      {
        if (bankruptcyDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.RefreshContents();
      }
    }

    protected void ShowFieldOptions(string fieldId)
    {
      FieldDefinition field = EncompassFields.GetField(fieldId, Session.LoanDataMgr == null ? (FieldSettings) null : Session.LoanDataMgr.LoanData.Settings.FieldSettings, true);
      if (field == null)
        return;
      using (FieldValuePicker fieldValuePicker = new FieldValuePicker(field))
      {
        if (fieldValuePicker.ShowDialog((IWin32Window) Session.Application) != DialogResult.OK)
          return;
        string val = fieldValuePicker.SelectedOption.Value;
        if (val.IndexOf("[ARM Life Low Percentage Rate]") > -1)
        {
          double num = Utils.ParseDouble((object) this.inputData.GetField("1699"));
          val = val.Replace("[ARM Life Low Percentage Rate]", num.ToString("N3") + "%");
        }
        this.SetField(fieldId, val);
        this.UpdateContents();
      }
    }

    private void showField3169Options()
    {
      using (ChangeCircumstanceSelector circumstanceSelector = new ChangeCircumstanceSelector())
      {
        if (circumstanceSelector.ShowDialog() != DialogResult.OK)
          return;
        this.inputData.SetCurrentField("3627", circumstanceSelector.OptionCode);
        this.inputData.SetCurrentField("3169", circumstanceSelector.OptionValue);
        this.inputData.SetCurrentField("3166", circumstanceSelector.OptionComment);
        this.inputData.SetCurrentField("LE1.X90", circumstanceSelector.OptionReason);
        this.UpdateContents();
      }
    }

    private void showFieldCoCOptions(string fieldId, string commentFieldID, string reasonFieldID)
    {
      string _appliesTo = fieldId == "3169" ? "LE" : "CD";
      this.loan.Calculator.UseNewCompliance(19.2M);
      using (ChangeCircumstanceSelector circumstanceSelector = new ChangeCircumstanceSelector(true, _appliesTo, this.GetField("4461") == "Y"))
      {
        if (circumstanceSelector.ShowDialog() != DialogResult.OK)
          return;
        string str1 = "";
        string str2 = "";
        string val1 = "";
        List<string[]> allOptions = circumstanceSelector.AllOptions;
        if (str1.Length != 0)
          str1 += Environment.NewLine;
        if (str2.Length != 0)
          str2 += Environment.NewLine;
        string str3 = "";
        string str4 = "";
        string val2 = "";
        string val3 = "";
        foreach (string[] strArray in allOptions)
        {
          str3 = str3 + strArray[1] + Environment.NewLine;
          if (!str4.Contains(Environment.NewLine + strArray[2] + Environment.NewLine) && !str4.StartsWith(strArray[2]))
            str4 = str4 + strArray[2] + Environment.NewLine;
          val1 = val1 + (val1 != "" ? "," : "") + strArray[3];
          val2 = val2 + (val2 != "" ? "," : "") + strArray[0];
          val3 = val3 + (val3 != "" ? "," : "") + strArray[1];
        }
        if (str3.Length >= 2)
          str3 = str3.Substring(0, str3.Length - 2);
        if (str4.Length >= 2)
          str4 = str4.Substring(0, str4.Length - 2);
        string val4 = str1 + str3;
        string val5 = str2 + str4;
        if (val4 != "")
        {
          if (this.GetField("4461") == "Y")
          {
            if (_appliesTo == "LE")
            {
              if (this.loan.FeeLevel_COC_LE_Warning)
              {
                int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please note: Any updates made on the Good Faith Variance Violated Alert page or 2015 itemization may overwrite your existing Changed Circumstances.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.loan.FeeLevel_COC_LE_Warning = false;
              }
            }
            else if (this.loan.FeeLevel_COC_CD_Warning)
            {
              int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please note: Any updates made on the Good Faith Variance Violated Alert page or 2015 itemization may overwrite your existing Changed Circumstances.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              this.loan.FeeLevel_COC_CD_Warning = false;
            }
            if (!this.loan.Locked_COC_Fields.Contains((object) fieldId))
              this.loan.Locked_COC_Fields.Add((object) fieldId);
          }
          this.inputData.SetCurrentField(fieldId, val4);
        }
        if (val5 != "")
        {
          if (this.GetField("4461") == "Y" && !this.loan.Locked_COC_Fields.Contains((object) commentFieldID))
            this.loan.Locked_COC_Fields.Add((object) commentFieldID);
          this.inputData.SetCurrentField(commentFieldID, val5);
        }
        if (val1 != "")
          this.inputData.SetCurrentField(reasonFieldID, val1);
        this.inputData.SetCurrentField("3627", val2);
        this.inputData.SetCurrentField("3166", val3);
        if (this.loan != null)
          this.loan.Calculator.FormCalculation(fieldId, "", "");
        this.UpdateContents();
      }
    }

    private void showClosingDocumentDataViewer()
    {
      ILoanServices service = this.session.Application.GetService<ILoanServices>();
      if (!service.IsDocServiceInstalled())
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.Application, "The Encompass Document Services are not currently available. Please log out of Encompass and log back in.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!service.VerifyDocServiceSetup(DocumentOrderType.Closing))
          return;
        using (ClosingDocumentDataViewer documentDataViewer = new ClosingDocumentDataViewer())
        {
          int num2 = (int) documentDataViewer.ShowDialog((IWin32Window) this.session.Application);
        }
      }
    }

    private void showSSNServiceDialog(string action)
    {
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      switch (action)
      {
        case "orderssnverification":
          service.OrderSSNService();
          break;
        case "checkstatus":
          service.CheckSSNServiceStatus();
          break;
      }
    }

    private void selectCountryName(string fieldID)
    {
      if (string.IsNullOrEmpty(fieldID))
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The lookup button doesn't have correct setting. Please use \"selectcountry_(field ID)\" in \"Action\" field in Form Builder, for example: selectcountry_FR0130.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (fieldID.Contains("_"))
          fieldID = fieldID.Replace("_", ".");
        using (SelectCountryForm selectCountryForm = new SelectCountryForm())
        {
          if (selectCountryForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          this.loan.SetCurrentField(fieldID, selectCountryForm.SelectedCountryName);
        }
      }
    }

    private void exportULDD(string format)
    {
      if (this.loan.Dirty && Utils.Dialog((IWin32Window) this.session.MainForm, "Do you want to save loan file before exporting?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        this.session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(this.session, false, true, ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen)), false);
      bool flag = false;
      switch (format)
      {
        case "Fannie":
          flag = new ExportFNMData(format).Export(this.session.LoanDataMgr, new string[1]
          {
            this.loan.GUID
          });
          break;
        case "Freddie":
          flag = new ExportFREData(format).Export(this.session.LoanDataMgr, new string[1]
          {
            this.loan.GUID
          });
          break;
        case "GinnieMaePdd12.161":
          flag = new ExportGNMData(format).Export(this.session.LoanDataMgr, new string[1]
          {
            this.loan.GUID
          });
          break;
      }
      if (!flag)
      {
        this.ulddExportChecking(true, false, format);
      }
      else
      {
        try
        {
          DataServices2 asyncState = new DataServices2(this.session?.SessionObjects?.StartupInfo?.ServiceUrls?.DataServices2Url);
          asyncState.BeginLogExportServices(Session.CompanyInfo.ClientID, Session.UserID, format, new string[1]
          {
            this.loan.GUID
          }, "Source: ULDD input form", new AsyncCallback(InputHandlerBase.onLogSubmissionComplete), (object) asyncState);
        }
        catch (Exception ex)
        {
          Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Warning, "Error while sending Export Log: " + (object) ex);
        }
      }
    }

    private static void onLogSubmissionComplete(IAsyncResult result)
    {
      try
      {
        ((DataServices2) result.AsyncState).EndLogExportServices(result);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Warning, "Error while sending Export Log: " + (object) ex);
      }
    }

    private void orderFraudService()
    {
      this.session.Application.GetService<ILoanServices>()?.OrderFraud();
    }

    private void orderFloodService()
    {
      this.session.Application.GetService<ILoanServices>()?.OrderFlood();
    }

    private void orderDataTracService()
    {
      this.session.Application.GetService<ILoanServices>()?.OrderDataTrac();
    }

    private void showNMLSDialog()
    {
      using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, "NMLS Report Information", new InputFormInfo("NMLSINFO", "NMLS Report Information", InputFormType.Standard, InputFormCategory.Tool, 0, true, true), 600, 610, FieldSource.CurrentLoan, "", this.session))
      {
        int num = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
      }
      this.UpdateContents(true);
    }

    private void showPurchaseAdviceInterestCalculationDialog()
    {
      using (InterestInfoForm interestInfoForm = new InterestInfoForm(this.loan))
      {
        if (interestInfoForm.ShowDialog((IWin32Window) Session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents(true);
      }
    }

    public virtual void AddToEFolder()
    {
      throw new InvalidOperationException("This operation is not valid in base class.");
    }

    private void showTitleEscrowTable(string tableType, string triggerField)
    {
      if (this.loan == null || !new TableHandler(this.loan, "REGZGFE", this.session).LookUpTable(tableType))
        return;
      this.loan.Calculator.FormCalculation("REGZ", triggerField, this.GetFieldValue(triggerField));
      this.UpdateContents();
    }

    private void setCoverageFee(bool forLender)
    {
      if (this.isUsingTemplate)
      {
        using (CoverageDialog coverageDialog = new CoverageDialog(this.inputData, forLender, false))
        {
          if (coverageDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          this.RefreshContents();
        }
      }
      else
      {
        using (CoverageDialog coverageDialog = new CoverageDialog(this.loan, forLender, false))
        {
          if (coverageDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          this.RefreshContents();
        }
      }
    }

    private void setRecordingFee(RecordingFeeDialog.FeeTypes feeType)
    {
      RecordingFeeDialog.FormTypes formType = this.editor == null || !(this.editor.CurrentForm == "MLDS - CA GFE") ? RecordingFeeDialog.FormTypes.GFEItemization : RecordingFeeDialog.FormTypes.MLDS;
      if (this.inputData is LoanData)
      {
        LoanData inputData = (LoanData) this.inputData;
        bool syncGFE = inputData.GetField("14").ToUpper() == "CA";
        using (RecordingFeeDialog recordingFeeDialog = new RecordingFeeDialog(feeType, inputData, false, formType, syncGFE))
        {
          if (recordingFeeDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          this.RefreshContents();
        }
      }
      else
      {
        using (RecordingFeeDialog recordingFeeDialog = new RecordingFeeDialog(feeType, this.inputData, false, formType))
        {
          if (recordingFeeDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          this.RefreshContents();
        }
      }
    }

    private void setDocSignDate()
    {
      if (this.loan == null)
        return;
      MilestoneLog milestone = this.loan.GetLogList().GetMilestone("Docs Signing");
      CalendarPopup calendarPopup = milestone == null || !(milestone.Date != DateTime.MinValue) ? new CalendarPopup(DateTime.Now) : new CalendarPopup(milestone.Date);
      calendarPopup.FormClosed += new FormClosedEventHandler(this.onDocSigningCalendarFormClosing);
      calendarPopup.Show((IWin32Window) this.session.MainScreen);
    }

    private void onDocSigningCalendarFormClosing(object sender, EventArgs e)
    {
      CalendarPopup calendarPopup = sender as CalendarPopup;
      if (calendarPopup.DialogResult != DialogResult.OK)
        return;
      LoanData loan1 = this.loan;
      DateTime selectedDate = calendarPopup.SelectedDate;
      string val1 = selectedDate.ToString("MM/dd/yyyy");
      loan1.SetCurrentField("1887", val1);
      LoanData loan2 = this.loan;
      selectedDate = calendarPopup.SelectedDate;
      string val2 = selectedDate.ToString("MM/dd/yyyy");
      loan2.TriggerCalculation("1887", val2);
      this.RefreshContents("1887");
    }

    private void updateLinkedBorrowerContact(bool isCoborrower)
    {
      EllieMae.EMLite.DataEngine.Borrower borrower = !isCoborrower ? this.loan.CurrentBorrowerPair.Borrower : this.loan.CurrentBorrowerPair.CoBorrower;
      CRMLog crmMapping = this.session.LoanData.GetLogList().GetCRMMapping(borrower.Id);
      if (crmMapping == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "There is no linked contact.");
      }
      else
      {
        BorrowerInfo borrowerInfo = this.session.ContactManager.GetBorrower(crmMapping.ContactGuid);
        if (borrowerInfo == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) null, "Linked borrower contact record can not be found.");
        }
        else
        {
          if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || borrowerInfo.OwnerID == this.session.UserID || this.session.AclGroupManager.GetBorrowerContactAccessRight(this.session.UserInfo, borrowerInfo.OwnerID) == AclResourceAccess.ReadWrite)
          {
            if ((!isCoborrower ? this.loan.GetField("65") : this.loan.GetField("97")) != borrowerInfo.SSN)
            {
              RxBorrowerSSN rxBorrowerSsn = new RxBorrowerSSN(isCoborrower, borrowerInfo, true);
              if (rxBorrowerSsn.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.Cancel)
                return;
              borrowerInfo = rxBorrowerSsn.BorrowerObj;
            }
            RxBorrowerSync rxBorrowerSync = new RxBorrowerSync(isCoborrower, borrowerInfo);
            if (rxBorrowerSync.HasConflict)
            {
              int num3 = (int) rxBorrowerSync.ShowDialog((IWin32Window) this.mainScreen);
            }
            else
            {
              int num4 = (int) Utils.Dialog((IWin32Window) null, "The borrower fields in the loan file and linked borrower contact are synchronized.");
            }
          }
          else
          {
            int num5 = (int) Utils.Dialog((IWin32Window) null, "You do not have edit access right to update linked borrower contact with loan data.");
          }
          this.UpdateContents(true, true, false);
        }
      }
    }

    private void getPricingFromOptimalBlue(string execAction)
    {
      if (this.GetField("3907") != string.Empty && this.GetFieldValue("3841") == "NewLock")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "This loan must be removed from the Correspondent Trade \"" + this.GetField("3907") + "\" before a new lock can be created.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.ValidateCommitmentTypeDeliveryType())
          return;
        if (this.loan.Calculator != null)
        {
          this.loan.Calculator.CalcOnDemand();
          bool skipLockRequestSync = this.loan.Calculator.SkipLockRequestSync;
          this.loan.Calculator.SkipLockRequestSync = true;
          this.loan.Calculator.CalculateAll(false);
          this.loan.Calculator.SkipLockRequestSync = skipLockRequestSync;
        }
        bool flag1 = this.session.ConfigurationManager.GetCompanySetting("POLICIES", "AllowPPESelection") == "True";
        ServiceSetupEvaluatorResponse svcSetupResponse = (ServiceSetupEvaluatorResponse) null;
        if (execAction == "getpricing" & flag1)
        {
          if (!this.SelectPricingProvider(ref svcSetupResponse))
            return;
        }
        else if (!this.GetProductPricingProvider(ref svcSetupResponse))
          return;
        this.session.LoanData.SetField("OPTIMAL.REQUEST", "");
        this.session.LoanData.SetField("OPTIMAL.RESPONSE", "");
        this.session.LoanData.SetField("OPTIMAL.REQUEST", this.buildRequestData(execAction));
        bool flag2 = false;
        if (this is LOLOCKREQUESTInputHandler)
          flag2 = (this as LOLOCKREQUESTInputHandler).IsOnrpButtonClicked;
        if (flag2 && this.session.StartupInfo.ProductPricingPartner.IsEPPS)
          this.epassProcessUrl(execAction, true);
        else if (this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2 && execAction == "getpricing" && (this.GetFieldValue("3841") == "NewLock" || LockUtils.IsRelock(this.GetFieldValue("3841"))))
          this.epassProcessUrlEpc2(svcSetupResponse);
        else
          this.epassProcessUrl(execAction, false);
        if (!(execAction == "getpricing" & flag1))
          return;
        this.session.StartupInfo.ProductPricingPartner = this.session.ConfigurationManager.GetActiveProductPricingPartner();
      }
    }

    private bool GetProductPricingProvider(ref ServiceSetupEvaluatorResponse svcSetupResponse)
    {
      bool productPricingProvider = false;
      if (this.session.StartupInfo.ProductPricingPartner == null)
      {
        if (this.session.UserInfo.IsTopLevelAdministrator())
        {
          ProductPricingLightAdmin pricingLightAdmin = new ProductPricingLightAdmin(this.session);
          if (pricingLightAdmin.ShowDialog((IWin32Window) this.session.Application) == DialogResult.OK)
          {
            this.session.StartupInfo.ProductPricingPartner = this.session.ConfigurationManager.GetActiveProductPricingPartner();
            if (this.session.StartupInfo.ProductPricingPartner != null)
            {
              svcSetupResponse = pricingLightAdmin.svcSetupResponse;
              productPricingProvider = true;
            }
          }
        }
      }
      else if (this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2)
      {
        string accessToken = new Bam((LoanData) null).GetAccessToken("sc");
        try
        {
          Task<ServiceSetupEvaluatorResponse> task = Task.Run<ServiceSetupEvaluatorResponse>((Func<Task<ServiceSetupEvaluatorResponse>>) (async () => await Epc2ServiceClient.GetServiceSetupEvaluatorResponse(this.session.SessionObjects, accessToken, this.session.LoanData.GUID, this.session.StartupInfo.ProductPricingPartner.ProviderID)));
          svcSetupResponse = task.Result;
        }
        catch
        {
          int num = (int) Utils.Dialog((IWin32Window) null, string.Format("Please contact your administrator for \"{0}\" access.", (object) this.session.StartupInfo.ProductPricingPartner.PartnerName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return productPricingProvider;
        }
        ServiceSetupEvaluatorResponse evaluatorResponse = svcSetupResponse;
        int num1;
        if (evaluatorResponse == null)
        {
          num1 = 0;
        }
        else
        {
          int? count = evaluatorResponse.MatchingResults?.Count;
          int num2 = 0;
          num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
        }
        if (num1 != 0)
        {
          productPricingProvider = true;
        }
        else
        {
          int num3 = (int) Utils.Dialog((IWin32Window) null, string.Format("Please contact your administrator for \"{0}\" access.", (object) this.session.StartupInfo.ProductPricingPartner.PartnerName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return productPricingProvider;
        }
      }
      else
        productPricingProvider = true;
      if (!productPricingProvider)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) null, "The Product and Pricing partner has not been selected by your administrator." + Environment.NewLine + "If no partner has been selected by your Encompass administrator, this feature will not be available.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return productPricingProvider;
    }

    private bool SelectPricingProvider(ref ServiceSetupEvaluatorResponse svcSetupResponse)
    {
      bool flag = false;
      ProductPricingLightAdmin pricingLightAdmin = new ProductPricingLightAdmin(this.session, true);
      if (pricingLightAdmin.ShowDialog((IWin32Window) this.session.Application) == DialogResult.OK)
      {
        ProductPricingSetting productPricingSetting = pricingLightAdmin.settings.SingleOrDefault<ProductPricingSetting>((Func<ProductPricingSetting, bool>) (s => s.Active));
        if (productPricingSetting != null)
        {
          this.session.StartupInfo.ProductPricingPartner = productPricingSetting;
          svcSetupResponse = pricingLightAdmin.svcSetupResponse;
          flag = true;
        }
      }
      return flag;
    }

    private void epassProcessUrlEpc2(ServiceSetupEvaluatorResponse svcSetupResponse)
    {
      string url = new Epc2ServiceClient().ComposeEPassPayloadAndUrlForEPC2(LockUtils.IsRelock(this.loan.GetField("3841")) ? "urn:elli:services:form:lockrequest:relock" : "urn:elli:services:form:lockrequest:getpricing", this.session.StartupInfo.ProductPricingPartner, svcSetupResponse);
      if (url == null)
        return;
      this.epass.ProcessURL(url);
    }

    private void epassProcessUrl(string execAction, bool isFork)
    {
      string str = this.session.StartupInfo.ProductPricingPartner.PartnerID;
      if (isFork)
        str = "MPS";
      string url;
      switch (execAction)
      {
        case "requestlock":
          url = "_EPASS_SIGNATURE;" + str + ";;GetPricing_LO;RequestLock";
          break;
        case "getpricing":
          if (LockUtils.IsRelock(this.loan.GetField("3841")))
          {
            string field = this.loan.GetField("LOCKRATE.RATESTATUS");
            url = !(field == "Cancelled") && !(field == "Expired") ? "_EPASS_SIGNATURE;" + str + ";;ValidateRelock;;SOURCE_FORM=GetPricing_LO_LOCK" : "_EPASS_SIGNATURE;" + str + ";;GetPricing_LO;;SOURCE_FORM=GetPricing_LO_LOCK";
            break;
          }
          goto default;
        default:
          url = "_EPASS_SIGNATURE;" + str + ";;GetPricing_LO;;SOURCE_FORM=GetPricing_LO_LOCK";
          break;
      }
      this.epass.ProcessURL(url);
    }

    private string buildRequestData(string execAction)
    {
      ValuePairXmlWriter valuePairXmlWriter = new ValuePairXmlWriter("FieldID", "FieldValue");
      valuePairXmlWriter.Write("ReturnPage", "LOLockRequest");
      if (execAction == "getpricing" && LockUtils.IsRelock(this.loan.GetField("3841")) && this.session.StartupInfo != null && this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && this is LOLOCKREQUESTInputHandler && ((LOLOCKREQUESTInputHandler) this).RelockCurrentLockLog != null)
      {
        valuePairXmlWriter.Write("ValidateRelock", "true");
        LockRequestLog relockCurrentLockLog = ((LOLOCKREQUESTInputHandler) this).RelockCurrentLockLog;
        if (relockCurrentLockLog != null)
        {
          Hashtable lockRequestSnapshot = relockCurrentLockLog.GetLockRequestSnapshot();
          if (lockRequestSnapshot.ContainsKey((object) "OPTIMAL.HISTORY"))
            valuePairXmlWriter.Write("OPTIMAL.HISTORY", (string) lockRequestSnapshot[(object) "OPTIMAL.HISTORY"]);
        }
      }
      return valuePairXmlWriter.ToXML();
    }

    private void editLoanComparisonComments(int col)
    {
      string id = string.Empty;
      switch (col)
      {
        case 0:
          id = "PREQUAL.X273";
          break;
        case 1:
          id = "PREQUAL.X286";
          break;
        case 2:
          id = "PREQUAL.X287";
          break;
      }
      using (PrequalCommentDialog prequalCommentDialog = new PrequalCommentDialog(this.loan.GetSimpleField(id), col + 1))
      {
        if (prequalCommentDialog.ShowDialog() != DialogResult.OK)
          return;
        this.loan.SetField(id, prequalCommentDialog.Comments);
        this.UpdateContents();
      }
    }

    private void showInvestorListDialog()
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        if (this.loan != null && this.loan.IsTemplate)
        {
          this.loan.SetField("VEND.X263", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.EntityName);
          this.loan.SetField("VEND.X264", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.Address.Street1);
          this.loan.SetField("VEND.X265", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.Address.City);
          this.loan.SetField("VEND.X266", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.Address.State);
          this.loan.SetField("VEND.X267", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.Address.Zip);
          this.loan.SetField("VEND.X271", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.ContactName);
          this.loan.SetField("VEND.X272", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.PhoneNumber);
          this.loan.SetField("VEND.X273", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.EmailAddress);
          this.loan.SetField("VEND.X274", templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.FaxNumber);
          this.loan.SetField("VEND.X369", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.EntityName);
          this.loan.SetField("VEND.X370", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.Address.Street1);
          this.loan.SetField("VEND.X371", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.Address.City);
          this.loan.SetField("VEND.X372", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.Address.State);
          this.loan.SetField("VEND.X373", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.Address.Zip);
          this.loan.SetField("VEND.X374", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.ContactName);
          this.loan.SetField("VEND.X375", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.PhoneNumber);
          this.loan.SetField("VEND.X376", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.EmailAddress);
          this.loan.SetField("VEND.X377", templateSelector.SelectedTemplate.CompanyInformation.ShippingInformation.FaxNumber);
          this.loan.SetField("VEND.X378", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.EntityName);
          this.loan.SetField("VEND.X379", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.Address.Street1);
          this.loan.SetField("VEND.X380", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.Address.City);
          this.loan.SetField("VEND.X381", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.Address.State);
          this.loan.SetField("VEND.X382", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.Address.Zip);
          this.loan.SetField("VEND.X383", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.ContactName);
          this.loan.SetField("VEND.X384", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.PhoneNumber);
          this.loan.SetField("VEND.X385", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.EmailAddress);
          this.loan.SetField("VEND.X386", templateSelector.SelectedTemplate.CompanyInformation.CustomerServiceInformation.FaxNumber);
          this.loan.SetField("VEND.X387", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.EntityName);
          this.loan.SetField("VEND.X388", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.Address.Street1);
          this.loan.SetField("VEND.X389", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.Address.City);
          this.loan.SetField("VEND.X390", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.Address.State);
          this.loan.SetField("VEND.X391", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.Address.Zip);
          this.loan.SetField("VEND.X392", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.ContactName);
          this.loan.SetField("VEND.X393", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.PhoneNumber);
          this.loan.SetField("VEND.X394", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.EmailAddress);
          this.loan.SetField("VEND.X395", templateSelector.SelectedTemplate.CompanyInformation.TrailingDocumentsInformation.FaxNumber);
          this.loan.SetField("VEND.X529", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).EntityName);
          this.loan.SetField("VEND.X530", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).Address.Street1);
          this.loan.SetField("VEND.X532", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).Address.City);
          this.loan.SetField("VEND.X533", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).Address.State);
          this.loan.SetField("VEND.X534", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).Address.Zip);
          this.loan.SetField("VEND.X535", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).ContactName);
          this.loan.SetField("VEND.X536", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).PhoneNumber);
          this.loan.SetField("VEND.X537", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).EmailAddress);
          this.loan.SetField("VEND.X538", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.payment).FaxNumber);
          this.loan.SetField("VEND.X539", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).EntityName);
          this.loan.SetField("VEND.X540", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).Address.Street1);
          this.loan.SetField("VEND.X542", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).Address.City);
          this.loan.SetField("VEND.X543", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).Address.State);
          this.loan.SetField("VEND.X544", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).Address.Zip);
          this.loan.SetField("VEND.X545", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).ContactName);
          this.loan.SetField("VEND.X546", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).PhoneNumber);
          this.loan.SetField("VEND.X547", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).EmailAddress);
          this.loan.SetField("VEND.X548", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.insurance).FaxNumber);
          this.loan.SetField("VEND.X549", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).EntityName);
          this.loan.SetField("VEND.X550", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).Address.Street1);
          this.loan.SetField("VEND.X552", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).Address.City);
          this.loan.SetField("VEND.X553", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).Address.State);
          this.loan.SetField("VEND.X554", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).Address.Zip);
          this.loan.SetField("VEND.X555", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).ContactName);
          this.loan.SetField("VEND.X556", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).PhoneNumber);
          this.loan.SetField("VEND.X557", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).EmailAddress);
          this.loan.SetField("VEND.X558", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.notedelivery).FaxNumber);
          this.loan.SetField("VEND.X559", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).EntityName);
          this.loan.SetField("VEND.X560", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).Address.Street1);
          this.loan.SetField("VEND.X562", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).Address.City);
          this.loan.SetField("VEND.X563", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).Address.State);
          this.loan.SetField("VEND.X564", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).Address.Zip);
          this.loan.SetField("VEND.X565", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).ContactName);
          this.loan.SetField("VEND.X566", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).PhoneNumber);
          this.loan.SetField("VEND.X567", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).EmailAddress);
          this.loan.SetField("VEND.X568", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.taxnotice).FaxNumber);
          this.loan.SetField("VEND.X569", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).EntityName);
          this.loan.SetField("VEND.X570", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).Address.Street1);
          this.loan.SetField("VEND.X572", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).Address.City);
          this.loan.SetField("VEND.X573", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).Address.State);
          this.loan.SetField("VEND.X574", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).Address.Zip);
          this.loan.SetField("VEND.X575", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).ContactName);
          this.loan.SetField("VEND.X576", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).PhoneNumber);
          this.loan.SetField("VEND.X577", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).EmailAddress);
          this.loan.SetField("VEND.X578", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.mortgageinsurance).FaxNumber);
          this.loan.SetField("VEND.X579", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).EntityName);
          this.loan.SetField("VEND.X580", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).Address.Street1);
          this.loan.SetField("VEND.X582", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).Address.City);
          this.loan.SetField("VEND.X583", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).Address.State);
          this.loan.SetField("VEND.X584", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).Address.Zip);
          this.loan.SetField("VEND.X585", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).ContactName);
          this.loan.SetField("VEND.X586", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).PhoneNumber);
          this.loan.SetField("VEND.X587", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).EmailAddress);
          this.loan.SetField("VEND.X588", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.loandelivery).FaxNumber);
          this.loan.SetField("VEND.X589", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).EntityName);
          this.loan.SetField("VEND.X590", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).Address.Street1);
          this.loan.SetField("VEND.X592", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).Address.City);
          this.loan.SetField("VEND.X593", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).Address.State);
          this.loan.SetField("VEND.X594", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).Address.Zip);
          this.loan.SetField("VEND.X595", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).ContactName);
          this.loan.SetField("VEND.X596", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).PhoneNumber);
          this.loan.SetField("VEND.X597", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).EmailAddress);
          this.loan.SetField("VEND.X598", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.assignment).FaxNumber);
          this.loan.SetField("VEND.X599", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).EntityName);
          this.loan.SetField("VEND.X600", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).Address.Street1);
          this.loan.SetField("VEND.X602", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).Address.City);
          this.loan.SetField("VEND.X603", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).Address.State);
          this.loan.SetField("VEND.X604", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).Address.Zip);
          this.loan.SetField("VEND.X605", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).ContactName);
          this.loan.SetField("VEND.X606", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).PhoneNumber);
          this.loan.SetField("VEND.X607", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).EmailAddress);
          this.loan.SetField("VEND.X608", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.correspondence).FaxNumber);
          this.loan.SetField("VEND.X609", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).EntityName);
          this.loan.SetField("VEND.X610", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).Address.Street1);
          this.loan.SetField("VEND.X612", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).Address.City);
          this.loan.SetField("VEND.X613", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).Address.State);
          this.loan.SetField("VEND.X614", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).Address.Zip);
          this.loan.SetField("VEND.X615", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).ContactName);
          this.loan.SetField("VEND.X616", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).PhoneNumber);
          this.loan.SetField("VEND.X617", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).EmailAddress);
          this.loan.SetField("VEND.X618", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic1).FaxNumber);
          this.loan.SetField("VEND.X619", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).EntityName);
          this.loan.SetField("VEND.X620", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).Address.Street1);
          this.loan.SetField("VEND.X622", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).Address.City);
          this.loan.SetField("VEND.X623", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).Address.State);
          this.loan.SetField("VEND.X624", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).Address.Zip);
          this.loan.SetField("VEND.X625", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).ContactName);
          this.loan.SetField("VEND.X626", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).PhoneNumber);
          this.loan.SetField("VEND.X627", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).EmailAddress);
          this.loan.SetField("VEND.X628", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic2).FaxNumber);
          this.loan.SetField("VEND.X629", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).EntityName);
          this.loan.SetField("VEND.X630", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).Address.Street1);
          this.loan.SetField("VEND.X632", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).Address.City);
          this.loan.SetField("VEND.X633", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).Address.State);
          this.loan.SetField("VEND.X634", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).Address.Zip);
          this.loan.SetField("VEND.X635", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).ContactName);
          this.loan.SetField("VEND.X636", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).PhoneNumber);
          this.loan.SetField("VEND.X637", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).EmailAddress);
          this.loan.SetField("VEND.X638", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic3).FaxNumber);
          this.loan.SetField("VEND.X639", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).EntityName);
          this.loan.SetField("VEND.X640", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).Address.Street1);
          this.loan.SetField("VEND.X642", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).Address.City);
          this.loan.SetField("VEND.X643", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).Address.State);
          this.loan.SetField("VEND.X644", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).Address.Zip);
          this.loan.SetField("VEND.X645", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).ContactName);
          this.loan.SetField("VEND.X646", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).PhoneNumber);
          this.loan.SetField("VEND.X647", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).EmailAddress);
          this.loan.SetField("VEND.X648", templateSelector.SelectedTemplate.CompanyInformation.GetContactInformation(InvestorContactType.generic4).FaxNumber);
          FieldDefinition field = EncompassFields.GetField("1397");
          EllieMae.EMLite.DataEngine.FieldOption fieldOption = field.Options.GetOptionByValue(templateSelector.SelectedTemplate.CompanyInformation.TypeOfPurchaser) ?? field.Options.GetOptionByText(templateSelector.SelectedTemplate.CompanyInformation.TypeOfPurchaser);
          if (fieldOption != null)
            this.loan.SetField("1397", fieldOption.Value);
        }
        else
          this.session.LoanDataMgr.ApplyInvestorToLoan(templateSelector.SelectedTemplate.CompanyInformation, (ContactInformation) null, true);
        this.UpdateContents(false, true, false);
      }
    }

    private void showPaymentDialog()
    {
      string fieldValue1 = this.GetFieldValue("LOANAMT1");
      string fieldValue2 = this.GetFieldValue("INTRATE1");
      string fieldValue3 = this.GetFieldValue("TERM1");
      string fieldValue4 = this.GetFieldValue("PAYMENT1");
      using (PaymentCalDialog paymentCalDialog = new PaymentCalDialog(this.loan, "Mortgage Payment Calculation"))
      {
        paymentCalDialog.UpdateCalcPymt(fieldValue1, fieldValue2, fieldValue3, fieldValue4);
        if (paymentCalDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        if (paymentCalDialog.LoanAmt != 0.0)
          this.UpdateFieldValue("LOANAMT1", paymentCalDialog.LoanAmt.ToString());
        else
          this.UpdateFieldValue("LOANAMT1", "");
        if (paymentCalDialog.IntRate != 0.0)
          this.UpdateFieldValue("INTRATE1", paymentCalDialog.IntRate.ToString());
        else
          this.UpdateFieldValue("INTRATE1", "");
        if (paymentCalDialog.Term != 0.0)
          this.UpdateFieldValue("TERM1", paymentCalDialog.Term.ToString());
        else
          this.UpdateFieldValue("TERM1", "");
        if (paymentCalDialog.Amount != 0.0)
          this.UpdateFieldValue("PAYMENT1", paymentCalDialog.Amount.ToString());
        else
          this.UpdateFieldValue("PAYMENT1", "");
        if (paymentCalDialog.Amount != 0.0)
          this.UpdateFieldValue("228", paymentCalDialog.Amount.ToString());
        else
          this.UpdateFieldValue("228", "");
        this.UpdateContents();
      }
    }

    private void showOtherFinancingDialog()
    {
      string fieldValue1 = this.GetFieldValue("LOANAMT2");
      string fieldValue2 = this.GetFieldValue("INTRATE2");
      string fieldValue3 = this.GetFieldValue("TERM2");
      string fieldValue4 = this.GetFieldValue("PAYMENT2");
      using (PaymentCalDialog paymentCalDialog = new PaymentCalDialog(this.loan, "Other Financial Payment Calculation"))
      {
        paymentCalDialog.UpdateCalcPymt(fieldValue1, fieldValue2, fieldValue3, fieldValue4);
        if (paymentCalDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        if (paymentCalDialog.LoanAmt != 0.0)
          this.UpdateFieldValue("LOANAMT2", paymentCalDialog.LoanAmt.ToString());
        else
          this.UpdateFieldValue("LOANAMT2", "");
        if (paymentCalDialog.IntRate != 0.0)
          this.UpdateFieldValue("INTRATE2", paymentCalDialog.IntRate.ToString());
        else
          this.UpdateFieldValue("INTRATE2", "");
        if (paymentCalDialog.Term != 0.0)
          this.UpdateFieldValue("TERM2", paymentCalDialog.Term.ToString());
        else
          this.UpdateFieldValue("TERM2", "");
        if (paymentCalDialog.Amount != 0.0)
          this.UpdateFieldValue("PAYMENT2", paymentCalDialog.Amount.ToString());
        else
          this.UpdateFieldValue("PAYMENT2", "");
        if (paymentCalDialog.Amount != 0.0)
          this.setFieldValue("229", FieldSource.CurrentLoan, paymentCalDialog.Amount.ToString());
        else
          this.UpdateFieldValue("229", "");
        this.UpdateContents();
      }
    }

    private void showDetailRequest()
    {
      if (this.GetFieldValue("2088") == string.Empty && this.GetFieldValue("2090") == string.Empty && (this.GetFieldValue("2091") == string.Empty || this.GetFieldValue("2091") == "//") && (this.GetFieldValue("2089") == string.Empty || this.GetFieldValue("2089") == "//"))
        this.UpdateFieldValue("2089", DateTime.Today.ToString("MM/dd/yyyy"));
      this.LoadQuickLinkForm("Detail Lock Request", "LODETAILEDLOCKREQUEST", 800, 600, FieldSource.CurrentLoan, "Detailed Lock Request Form");
      this.UpdateContents();
    }

    private void showHazardInsuranceDialog(string formTitle)
    {
      InsuranceDialog insuranceDialog = !this.isUsingTemplate ? new InsuranceDialog(formTitle, this.loan, this.session) : new InsuranceDialog(formTitle, this.inputData, this.session);
      if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
      {
        this.UpdateFieldValue("1750", insuranceDialog.PriceType.ToString());
        this.UpdateFieldValue("1322", insuranceDialog.RateFactor > 0.0 ? insuranceDialog.RateFactor.ToString() : "");
        this.UpdateFieldValue("230", insuranceDialog.Amount > 0.0 ? insuranceDialog.Amount.ToString() : "");
        this.UpdateContents();
      }
      insuranceDialog.Dispose();
    }

    private void showTaxesDialog()
    {
      using (InsuranceDialog insuranceDialog = new InsuranceDialog("Taxes", this.loan, this.session))
      {
        if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateFieldValue("1751", insuranceDialog.PriceType.ToString());
        double num;
        string val1;
        if (insuranceDialog.RateFactor <= 0.0)
        {
          val1 = "";
        }
        else
        {
          num = insuranceDialog.RateFactor;
          val1 = num.ToString();
        }
        this.UpdateFieldValue("1752", val1);
        string val2;
        if (insuranceDialog.Amount <= 0.0)
        {
          val2 = "";
        }
        else
        {
          num = insuranceDialog.Amount;
          val2 = num.ToString();
        }
        this.UpdateFieldValue("1405", val2);
        this.UpdateContents();
      }
    }

    private void showRETaxesDialog()
    {
      using (RETaxesDialog reTaxesDialog = new RETaxesDialog("Taxes", this.loan, this.session, this.feeManagementSetting, this.feeManagementPermission))
      {
        if (reTaxesDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.loan.Calculator.FormCalculation("HUD1ES", (string) null, (string) null);
        if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
        {
          if (this.loan.Calculator.UseNew2015GFEHUD)
          {
            this.loan.Calculator.FormCalculation("NEWHUD2.X4397", (string) null, (string) null);
            this.loan.Calculator.FormCalculation("URLA.X144", (string) null, (string) null);
            this.loan.Calculator.FormCalculation("234", (string) null, (string) null);
          }
          this.loan.Calculator.FormCalculation("ITEMIZATION_SECTION1000", (string) null, (string) null);
          if (this.editor.CurrentForm == "2010 Itemization" || this.editor.CurrentForm == "2015 Itemization")
            this.loan.Calculator.CopyHUD2010ToGFE2010("231", false);
          this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
        }
        this.UpdateContents();
      }
    }

    private void showTaxesReservedDialog()
    {
      if (this.isUsingTemplate)
      {
        using (InsuranceDialog insuranceDialog = new InsuranceDialog("Taxes Reserved", this.inputData, this.session))
        {
          if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
            return;
          this.UpdateFieldValue("1751", insuranceDialog.PriceType.ToString());
          this.UpdateFieldValue("1752", insuranceDialog.RateFactor.ToString());
          this.UpdateFieldValue("231", insuranceDialog.Amount.ToString());
          this.UpdateContents();
        }
      }
      else
      {
        using (InsuranceDialog insuranceDialog = new InsuranceDialog("Taxes Reserved", this.loan, this.session))
        {
          if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
            return;
          this.setFieldValue("1751", FieldSource.CurrentLoan, insuranceDialog.PriceType.ToString());
          this.setFieldValue("1752", FieldSource.CurrentLoan, insuranceDialog.RateFactor.ToString());
          this.UpdateFieldValue("231", insuranceDialog.Amount.ToString());
          this.UpdateContents();
        }
      }
    }

    private void showMortgageInsuranceDialog()
    {
      InsuranceDialog insuranceDialog = new InsuranceDialog("Mortgage Insurance", this.loan, this.session);
      if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      this.UpdateFieldValue("USEREGZMI", insuranceDialog.UseREGZMI);
      this.UpdateFieldValue("1757", insuranceDialog.PriceType.ToString());
      double num;
      string val;
      if (insuranceDialog.RateFactor == 0.0)
      {
        val = "";
      }
      else
      {
        num = insuranceDialog.RateFactor;
        val = num.ToString();
      }
      this.UpdateFieldValue("1199", val);
      num = insuranceDialog.Amount;
      this.setFieldValue("232", FieldSource.CurrentLoan, num.ToString("N2"));
      this.UpdateContents();
    }

    private void showMortgageInsurancePremiumDialog()
    {
      InsuranceDialog insuranceDialog = new InsuranceDialog("Mortgage Insurance Premium", this.loan, this.session);
      if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      this.UpdateFieldValue("1807", insuranceDialog.RateFactor.ToString());
      this.UpdateFieldValue("1806", insuranceDialog.PriceType);
      if (insuranceDialog.MonthsMIP > 0)
        this.UpdateFieldValue("1209", insuranceDialog.MonthsMIP.ToString());
      else
        this.UpdateFieldValue("1209", "");
      this.UpdateFieldValue("2978", insuranceDialog.IsPrepaid);
      this.UpdateFieldValue("RE88395.X43", insuranceDialog.Amount.ToString());
      this.UpdateFieldValue("969", insuranceDialog.Amount.ToString());
      if (this.GetFieldValue("1172") != "VA")
        this.UpdateFieldValue("337", insuranceDialog.Amount.ToString());
      else
        this.UpdateFieldValue("1050", insuranceDialog.Amount.ToString());
      this.UpdateContents();
    }

    private void showMortgageInsurancePremiumMLDSDialog()
    {
      InsuranceDialog insuranceDialog = new InsuranceDialog("Mortgage Insurance Premium", this.loan, this.session);
      if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      this.UpdateFieldValue("1806", insuranceDialog.PriceType);
      this.UpdateFieldValue("1209", insuranceDialog.MonthsMIP.ToString());
      this.UpdateFieldValue("2978", insuranceDialog.IsPrepaid);
      this.UpdateFieldValue("1807", insuranceDialog.RateFactor.ToString());
      if (this.loan.GetSimpleField("1172") != "VA")
        this.UpdateFieldValue("337", insuranceDialog.Amount.ToString());
      else
        this.UpdateFieldValue("1050", insuranceDialog.Amount.ToString());
      this.UpdateContents();
    }

    private void showSwapBorrowerPairDialog()
    {
      using (SwapBorrowerPairForm borrowerPairForm = new SwapBorrowerPairForm(this.loan))
      {
        int num = (int) borrowerPairForm.ShowDialog((IWin32Window) null);
      }
    }

    private void showMortgageInsuranceReservedDialog()
    {
      if (this.loan.GetSimpleField("1172") != "FHA")
      {
        InsuranceDialog insuranceDialog = new InsuranceDialog("Mortgage Insurance Reserved", this.loan, this.session);
        if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateFieldValue("USEREGZMI", insuranceDialog.UseREGZMI);
        this.UpdateFieldValue("1757", insuranceDialog.PriceType);
        double num;
        string val;
        if (insuranceDialog.RateFactor == 0.0)
        {
          val = "";
        }
        else
        {
          num = insuranceDialog.RateFactor;
          val = num.ToString();
        }
        this.UpdateFieldValue("1199", val);
        if (insuranceDialog.Amount != 0.0)
        {
          num = insuranceDialog.Amount;
          this.setFieldValue("232", FieldSource.CurrentLoan, num.ToString("N2"));
        }
        else
          this.setFieldValue("232", FieldSource.CurrentLoan, "");
        this.UpdateContents();
      }
      else
      {
        if (new MIPDialog(this.loan).ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateFieldValue("1045", this.loan.GetField("1045"));
        this.UpdateContents();
      }
    }

    private void showOtherExpenseDialog(string calculatedField)
    {
      using (OtherExpenseDialog otherExpenseDialog = new OtherExpenseDialog(this.loan, calculatedField, this.session, this.feeManagementSetting, this.feeManagementPermission))
      {
        if (otherExpenseDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        if (!this.loan.IsLocked(calculatedField))
        {
          this.inputData.SetCurrentField(calculatedField, otherExpenseDialog.OtherExpense.ToString("N2"));
          if (this.loan != null && this.loan.Calculator != null)
            this.loan.Calculator.FormCalculation(calculatedField);
        }
        this.UpdateContents();
      }
    }

    private void showBaseIncomeDialog(int borrPairIndex = 0)
    {
      using (BorIncomeDialog borIncomeDialog = new BorIncomeDialog(this.loan))
      {
        if (borIncomeDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        if (borIncomeDialog.CopyVOE)
        {
          double num;
          string val1;
          if (borIncomeDialog.BorBaseIncome == 0.0)
          {
            val1 = "";
          }
          else
          {
            num = borIncomeDialog.BorBaseIncome;
            val1 = num.ToString();
          }
          this.UpdateFieldValue("101", val1);
          string val2;
          if (borIncomeDialog.BorOverTime == 0.0)
          {
            val2 = "";
          }
          else
          {
            num = borIncomeDialog.BorOverTime;
            val2 = num.ToString();
          }
          this.UpdateFieldValue("102", val2);
          string val3;
          if (borIncomeDialog.BorBonus == 0.0)
          {
            val3 = "";
          }
          else
          {
            num = borIncomeDialog.BorBonus;
            val3 = num.ToString();
          }
          this.UpdateFieldValue("103", val3);
          string val4;
          if (borIncomeDialog.BorCommissions == 0.0)
          {
            val4 = "";
          }
          else
          {
            num = borIncomeDialog.BorCommissions;
            val4 = num.ToString();
          }
          this.UpdateFieldValue("104", val4);
          string val5;
          if (borIncomeDialog.BorOthers == 0.0)
          {
            val5 = "";
          }
          else
          {
            num = borIncomeDialog.BorOthers;
            val5 = num.ToString();
          }
          this.UpdateFieldValue("107", val5);
          string val6;
          if (borIncomeDialog.CobBaseIncome == 0.0)
          {
            val6 = "";
          }
          else
          {
            num = borIncomeDialog.CobBaseIncome;
            val6 = num.ToString();
          }
          this.UpdateFieldValue("110", val6);
          string val7;
          if (borIncomeDialog.CobOverTime == 0.0)
          {
            val7 = "";
          }
          else
          {
            num = borIncomeDialog.CobOverTime;
            val7 = num.ToString();
          }
          this.UpdateFieldValue("111", val7);
          string val8;
          if (borIncomeDialog.CobBonus == 0.0)
          {
            val8 = "";
          }
          else
          {
            num = borIncomeDialog.CobBonus;
            val8 = num.ToString();
          }
          this.UpdateFieldValue("112", val8);
          string val9;
          if (borIncomeDialog.CobCommissions == 0.0)
          {
            val9 = "";
          }
          else
          {
            num = borIncomeDialog.CobCommissions;
            val9 = num.ToString();
          }
          this.UpdateFieldValue("113", val9);
          string val10;
          if (borIncomeDialog.CobOthers == 0.0)
          {
            val10 = "";
          }
          else
          {
            num = borIncomeDialog.CobOthers;
            val10 = num.ToString();
          }
          this.UpdateFieldValue("116", val10);
          num = borIncomeDialog.BorBaseIncome;
          this.UpdateFieldValue("QM.X137", num.ToString());
          num = borIncomeDialog.CobBaseIncome;
          this.UpdateFieldValue("QM.X145", num.ToString());
        }
        else
        {
          double num = borIncomeDialog.BorBaseIncome;
          if (num > 0.0)
          {
            switch (borrPairIndex)
            {
              case 0:
                this.UpdateFieldValue("101", num != 0.0 ? num.ToString() : "");
                this.UpdateFieldValue("QM.X137", num.ToString());
                break;
              case 1:
                this.UpdateFieldValue("FE0119", num != 0.0 ? num.ToString() : "");
                break;
              case 2:
                this.UpdateFieldValue("FE0319", num != 0.0 ? num.ToString() : "");
                break;
            }
          }
          num = borIncomeDialog.CobBaseIncome;
          if (num > 0.0)
          {
            switch (borrPairIndex)
            {
              case 0:
                this.UpdateFieldValue("110", num != 0.0 ? num.ToString() : "");
                this.UpdateFieldValue("QM.X145", num.ToString());
                break;
              case 1:
                this.UpdateFieldValue("FE0219", num != 0.0 ? num.ToString() : "");
                break;
              case 2:
                this.UpdateFieldValue("FE0419", num != 0.0 ? num.ToString() : "");
                break;
            }
          }
        }
        this.UpdateContents();
      }
    }

    private void show2020BaseIncomeDialog(string borrPairIndex)
    {
      using (MonthlyIncomeDialog monthlyIncomeDialog = new MonthlyIncomeDialog(this.loan, borrPairIndex))
      {
        if (monthlyIncomeDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        double borBaseIncome = monthlyIncomeDialog.BorBaseIncome;
        if (borBaseIncome > 0.0)
          this.UpdateFieldValue(borrPairIndex + "19", borBaseIncome != 0.0 ? borBaseIncome.ToString() : "");
        this.UpdateContents();
      }
    }

    private void showMilitaryEntitlementDialog(string borrPairIndex)
    {
      using (MilitaryEntitlements militaryEntitlements = new MilitaryEntitlements(this.loan, borrPairIndex))
      {
        if (militaryEntitlements.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        double entitlementsTotal = militaryEntitlements.MilitaryEntitlementsTotal;
        double baseIncome = militaryEntitlements.BaseIncome;
        this.UpdateFieldValue(borrPairIndex + "53", entitlementsTotal != 0.0 ? entitlementsTotal.ToString() : "");
        this.UpdateContents();
      }
    }

    private void showMPICalculatorDialog(string borrPairIndex)
    {
      using (MPICalcDialog mpiCalcDialog = new MPICalcDialog(this.loan, borrPairIndex))
      {
        int num = (int) mpiCalcDialog.ShowDialog((IWin32Window) this.mainScreen);
      }
    }

    private void showClosingCostSelectionDialog(FieldSource fieldSource)
    {
      if ((fieldSource != FieldSource.LinkedLoan || this.loan.LinkedData == null ? (System.Windows.Forms.Form) new ClosingCostSelect(this.loan) : (System.Windows.Forms.Form) new ClosingCostSelect(this.loan.LinkedData)).ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      this.UpdateContents();
      if (fieldSource != FieldSource.LinkedLoan)
        return;
      try
      {
        this.session.Application.GetService<ILoanEditor>().RefreshContents();
      }
      catch (Exception ex)
      {
      }
    }

    private void showLoanProgramSelectionDialog(FieldSource fieldSource)
    {
      this.showLoanProgramSelectionDialog(fieldSource, false);
    }

    private void showLoanProgramSelectionDialog(FieldSource fieldSource, bool applyToLockRequest)
    {
      LoanProgramSelect loanProgramSelect = fieldSource != FieldSource.LinkedLoan || this.loan.LinkedData == null ? new LoanProgramSelect(this.loan, applyToLockRequest ? LoanProgramSelect.SelectTypes.ForLockRequest : LoanProgramSelect.SelectTypes.ForLoan, this.session) : new LoanProgramSelect(this.loan.LinkedData, applyToLockRequest ? LoanProgramSelect.SelectTypes.ForLockRequest : LoanProgramSelect.SelectTypes.ForLoan, this.session);
      if (loanProgramSelect.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
      {
        try
        {
          this.applyLoanTemplateTriggerRule("1401", this.GetField("1401"));
          this.UpdateContents();
          if (fieldSource == FieldSource.LinkedLoan)
            this.session.Application.GetService<ILoanEditor>().RefreshContents();
        }
        catch (Exception ex)
        {
        }
      }
      loanProgramSelect.Dispose();
    }

    private void sendRequestToePASS(string signature, bool useNavigate)
    {
      if (useNavigate)
        this.epass.Navigate(signature);
      else
        this.epass.ProcessURL(signature);
      this.mainScreen.RefreshCE();
    }

    private void openSubordinateFinancingDialog(FieldSource fieldSource, bool forLockRequest)
    {
      bool flag = !forLockRequest;
      System.Windows.Forms.Form form = !flag ? (fieldSource != FieldSource.LinkedLoan || this.loan.LinkedData == null ? (System.Windows.Forms.Form) new SubFinancingDialog(this.loan) : (System.Windows.Forms.Form) new SubFinancingDialog(this.loan.LinkedData)) : (System.Windows.Forms.Form) new SubFinancingHelocDialog(this.session, fieldSource != FieldSource.LinkedLoan || this.loan.LinkedData == null ? this.loan : this.loan.LinkedData, fieldSource);
      if (form.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
      {
        if (this.loan != null && this.loan.Calculator != null && !flag)
          this.loan.Calculator.CalcPaymentSchedule();
        this.UpdateContents(true);
      }
      form.Close();
      form.Dispose();
    }

    private void openSubordinateHELOCFinancingDialog(FieldSource fieldSource)
    {
      using (SubFinancingHelocDialog financingHelocDialog = new SubFinancingHelocDialog(this.session, this.loan, fieldSource))
      {
        int num = (int) financingHelocDialog.ShowDialog((IWin32Window) this.mainScreen);
      }
      this.UpdateContents(true);
    }

    private void extensionRequest()
    {
      if (this.LockRequestInfoDiff && Utils.Dialog((IWin32Window) this.session.MainForm, "Request information changes will not apply on Extension Requests. Do you want to continue to Extend this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      string providerId = this.loan.GetProviderId();
      if (ProductPricingUtils.IsProviderICEPPE(providerId))
      {
        string source = "urn:elli:services:form:lockrequest:extendlock";
        string partnerName;
        string epC2EpassUrl = Epc2ServiceClient.GetEPC2EPassURL(this.session.SessionObjects, this.session.LoanData.GUID, providerId, source, out partnerName);
        if (string.IsNullOrWhiteSpace(epC2EpassUrl))
        {
          int num = (int) Utils.Dialog((IWin32Window) null, string.Format("Please contact your administrator for \"{0}\" access.", (object) partnerName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
          Session.Application.GetService<IEPass>().ProcessURL(epC2EpassUrl, true, true);
      }
      else
      {
        LockExtensionUtils settings = new LockExtensionUtils(this.session.SessionObjects, this.loan);
        int currentExtNumber = this.loan.GetLogList().GetCurrentExtNumber((Hashtable) null);
        if (settings.IsCompanyControlledOccur && settings.AdjOccurrence != null && currentExtNumber + 1 > settings.AdjOccurrence.Length || settings.IfExceedNumExtensionsLimit(currentExtNumber + 1))
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "This request exceeds the max number of extensions allowed. Lock has not been extended.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (new LockExtensionDialog(this.session, this.loan, settings, currentExtNumber + 1).ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK || this.session.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockExtensionRequest)
            return;
          using (LockRequestProcessDialog requestProcessDialog = new LockRequestProcessDialog(LockRequestProcessDialog.MessageSubjectType.Extention))
          {
            if (requestProcessDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
              return;
            ILoanConsole service = this.session.Application.GetService<ILoanConsole>();
            if (!service.SaveLoan() || !requestProcessDialog.CloseLoanFile)
              return;
            service.CloseLoan(false);
          }
        }
      }
    }

    private void lockCancellationRequest()
    {
      if (new LockCancellationDialog(this.session, this.loan, LockCancellationDialog.CancellationActionType.RequestCancellation).ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK || this.session.SessionObjects.StartupInfo.ProductPricingPartner == null || this.session.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoCancelRequest)
        return;
      using (LockRequestProcessDialog requestProcessDialog = new LockRequestProcessDialog(LockRequestProcessDialog.MessageSubjectType.LockCancellation))
      {
        if (requestProcessDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        ILoanConsole service = this.session.Application.GetService<ILoanConsole>();
        if (!service.SaveLoan() || !requestProcessDialog.CloseLoanFile)
          return;
        service.CloseLoan(false);
      }
    }

    private void sendLockRequest(string execAction)
    {
      if (!this.ValidateCommitmentTypeDeliveryType())
        return;
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.CalcOnDemand();
      bool relock = LockUtils.IsRelock(this.loan.GetField("3841"));
      try
      {
        string str = string.Empty;
        ProductPricingUtils.SynchronizeProductPricingSettingsWithServer(this.session);
        if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.PartnerRequestLock && (this.session.ConfigurationManager.GetCompanySetting("POLICIES", "ReqeustLockFromPPEOnlyWOCurrentLock") != "True" || this.loan.GetLogList().GetCurrentLockRequest() == null && (!this.session.StartupInfo.ProductPricingPartner.PartnerRequestLockWhenNoCurrentLock || this.loan.GetLogList().GetCurrentLockRequest() == null || LoanLockUtils.TranslateRateLockRequestStatus(this.loan.GetLogList().GetCurrentLockRequest()) != RateLockRequestStatus.RateLocked)))
          this.getPricingFromOptimalBlue(execAction);
        else
          str = this.session.StartupInfo.ProductPricingPartner == null || !this.session.StartupInfo.ProductPricingPartner.PartnerRequestLock || !this.session.StartupInfo.ProductPricingPartner.PartnerRequestLockWhenNoCurrentLock || !(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "ReqeustLockFromPPEOnlyWOCurrentLock") == "True") || this.loan.GetLogList().GetCurrentLockRequest() == null || LoanLockUtils.TranslateRateLockRequestStatus(this.loan.GetLogList().GetCurrentLockRequest()) != RateLockRequestStatus.RateLocked ? new InputHandlerUtil(this.session).sendLockRequest(this.inputData, false, relock, true, true) : new InputHandlerUtil(this.session).sendLockRequest(this.inputData, true, relock, true);
        if (!this.notAllowedPricingChangeSetting || string.IsNullOrEmpty(str))
          return;
        string field = this.inputData.GetField("3039");
        if (!(field != "//") || !Utils.IsDate((object) field))
          return;
        this.UpdateFieldValue("3039", "", true);
      }
      catch (LockDeskClosedException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (LockDeskONRPException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (RateLockRejectedException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void copyFieldValuesBetweenForms(string sourceID, string targetID, string triggerID)
    {
      if (this.loan == null)
        return;
      string field = this.loan.GetField(sourceID);
      this.loan.SetCurrentField(targetID, field);
      if (targetID == "1109")
        this.loan.SetCurrentField("1765", "");
      if (!(triggerID != string.Empty))
        return;
      this.loan.TriggerCalculation(triggerID, field);
    }

    private void showARMTypeDetails(bool applyToLockRequest)
    {
      string simpleField = this.inputData.GetSimpleField("995");
      if (applyToLockRequest)
        simpleField = this.inputData.GetSimpleField("2956");
      ARMTypeDetails armTypeDetails = new ARMTypeDetails("995", simpleField);
      if (armTypeDetails.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      if (applyToLockRequest)
        this.inputData.SetCurrentField("2956", armTypeDetails.ArmTypeID);
      else
        this.inputData.SetCurrentField("995", armTypeDetails.ArmTypeID);
      this.UpdateContents();
    }

    private void copyVOLtoMLDS(int location)
    {
      using (SelectVOLForm selectVolForm = new SelectVOLForm(this.loan, location))
      {
        if (selectVolForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        int num1 = 0;
        for (int index = 0; index < selectVolForm.SelectedVOLs.Count; ++index)
        {
          string[] selectedVoL = (string[]) selectVolForm.SelectedVOLs[index];
          ++num1;
          switch (location)
          {
            case 1:
              int num2 = (num1 - 1) * 3 + 128;
              this.loan.SetCurrentField("RE88395.X" + (object) num2, selectedVoL[0]);
              int num3 = num2 + 1;
              this.loan.SetCurrentField("RE88395.X" + (object) num3, selectedVoL[1]);
              int num4 = num3 + 1;
              if (selectedVoL[0] == string.Empty && selectedVoL[1] == string.Empty)
              {
                this.loan.SetCurrentField("RE88395.X" + (object) num4, "");
                break;
              }
              this.loan.SetCurrentField("RE88395.X" + (object) num4, num1.ToString());
              break;
            case 2:
              int num5 = (num1 - 1) * 3 + 137;
              this.loan.SetCurrentField("RE88395.X" + (object) num5, selectedVoL[0]);
              int num6 = num5 + 1;
              this.loan.SetCurrentField("RE88395.X" + (object) num6, selectedVoL[1]);
              int num7 = num6 + 1;
              if (selectedVoL[0] == string.Empty && selectedVoL[1] == string.Empty)
              {
                this.loan.SetCurrentField("RE88395.X" + (object) num7, "");
                break;
              }
              this.loan.SetCurrentField("RE88395.X" + (object) num7, num1.ToString());
              break;
          }
          if (num1 >= 3)
            break;
        }
        this.UpdateContents();
      }
    }

    private void copyDisclosedAPR()
    {
      this.removeFieldLock("3121", FieldSource.CurrentLoan);
      this.removeFieldLock("363", FieldSource.CurrentLoan);
      this.inputData.SetCurrentField("3121", this.inputData.GetField("799"));
      this.inputData.SetCurrentField("363", DateTime.Now.ToString("MM/dd/yyyy"));
      if (this.inputData is LoanData && this.loan != null && this.loan.IsTemplate)
        return;
      this.session.Application.GetService<ILoanEditor>().RefreshLogPanel();
    }

    private void copyAddressInStatementOfDenialScreen(string actionID)
    {
      string str = actionID == "copyfrombordenial" ? this.inputData.GetField("DENIAL.X81") : this.inputData.GetField("DENIAL.X86");
      string field = this.inputData.GetField("1825");
      if (str == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please select address type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.SetFieldFocus(actionID == "copyfrombordenial" ? "l_X81" : "l_X86");
      }
      switch (str)
      {
        case "Present Address":
          if (field == "2020")
          {
            this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X97" : "DENIAL.X99", actionID == "copyfrombordenial" ? this.inputData.GetField("FR0129") : this.inputData.GetField("FR0229"));
            this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X98" : "DENIAL.X100", actionID == "copyfrombordenial" ? this.inputData.GetField("FR0130") : this.inputData.GetField("FR0230"));
          }
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X82" : "DENIAL.X87", actionID == "copyfrombordenial" ? this.inputData.GetField("FR0104") : this.inputData.GetField("FR0204"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X83" : "DENIAL.X88", actionID == "copyfrombordenial" ? this.inputData.GetField("FR0106") : this.inputData.GetField("FR0206"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X84" : "DENIAL.X89", actionID == "copyfrombordenial" ? this.inputData.GetField("FR0107") : this.inputData.GetField("FR0207"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X85" : "DENIAL.X90", actionID == "copyfrombordenial" ? this.inputData.GetField("FR0108") : this.inputData.GetField("FR0208"));
          break;
        case "Mailing Address":
          if (field == "2020")
          {
            this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X97" : "DENIAL.X99", actionID == "copyfrombordenial" ? this.inputData.GetField("URLA.X267") : this.inputData.GetField("URLA.X268"));
            this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X98" : "DENIAL.X100", actionID == "copyfrombordenial" ? this.inputData.GetField("URLA.X269") : this.inputData.GetField("URLA.X270"));
          }
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X82" : "DENIAL.X87", actionID == "copyfrombordenial" ? this.inputData.GetField("1416") : this.inputData.GetField("1519"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X83" : "DENIAL.X88", actionID == "copyfrombordenial" ? this.inputData.GetField("1417") : this.inputData.GetField("1520"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X84" : "DENIAL.X89", actionID == "copyfrombordenial" ? this.inputData.GetField("1418") : this.inputData.GetField("1521"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X85" : "DENIAL.X90", actionID == "copyfrombordenial" ? this.inputData.GetField("1419") : this.inputData.GetField("1522"));
          break;
        case "Subject Address":
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X82" : "DENIAL.X87", this.inputData.GetField("11"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X83" : "DENIAL.X88", this.inputData.GetField("12"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X84" : "DENIAL.X89", this.inputData.GetField("14"));
          this.inputData.SetField(actionID == "copyfrombordenial" ? "DENIAL.X85" : "DENIAL.X90", this.inputData.GetField("15"));
          break;
      }
    }

    private void copyTo2010MLDS()
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to copy Itemization Fees to MLDS?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.loan.Calculator.CopyGFEToMLDS((string) null);
    }

    private void copyToHUDGFE()
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to copy Borrower Paid column to GFE column?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.loan.Calculator.CopyHUD2010ToGFE2010();
    }

    private void copyModifiedTerms()
    {
      string fieldValue1 = this.GetFieldValue("2991");
      string fieldValue2 = this.GetFieldValue("2992");
      string fieldValue3 = this.GetFieldValue("2993");
      if (fieldValue1 == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "The Loan Amount cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (fieldValue2 == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "The Loan Rate cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (fieldValue3 == string.Empty)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The Loan Term cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.UpdateFieldValue("1109", fieldValue1);
        this.UpdateFieldValue("3", fieldValue2);
        this.UpdateFieldValue("KBYO.XD3", Utils.RemoveEndingZeros(fieldValue2));
        this.UpdateFieldValue("4", fieldValue3);
        this.UpdateContents();
      }
    }

    private void calculateHOEPAAPR()
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.FormCalculation(nameof (calculateHOEPAAPR), (string) null, (string) null);
      Cursor.Current = Cursors.Default;
    }

    private void calculateOvertimeIncome()
    {
      using (IncomeBonusForm incomeBonusForm = new IncomeBonusForm(this.inputData, this.session))
      {
        if (incomeBonusForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    private void calculateDividendIncome()
    {
      using (IncomeDividendForm incomeDividendForm = new IncomeDividendForm(this.inputData, this.session))
      {
        if (incomeDividendForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    private void calculateOtherIncome()
    {
      using (IncomeOtherForm incomeOtherForm = new IncomeOtherForm(this.inputData, this.session))
      {
        if (incomeOtherForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    private void calculateMilitaryIncome()
    {
      using (IncomeMilitaryForm incomeMilitaryForm = new IncomeMilitaryForm(this.inputData, this.session))
      {
        if (incomeMilitaryForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    private void calculateMilitaryAllowancesIncome()
    {
      using (IncomeAllowancesForm incomeAllowancesForm = new IncomeAllowancesForm(this.inputData, this.session))
      {
        if (incomeAllowancesForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    private void copyAddressFields()
    {
      this.UpdateFieldValue("4103", "Y");
      if (this.inputData.GetField("1825") == "2020" && this.inputData.GetField("FR0129") == "Y")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Borrower's current address is foreign address. You cannot copy foreign address to subject property address!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.StatePermission(this.GetFieldValue("FR0107"), false))
          return;
        if (this.inputData.GetField("1825") == "2020")
        {
          this.UpdateFieldValue("URLA.X73", this.GetFieldValue("FR0126"));
          this.UpdateFieldValue("URLA.X74", this.GetFieldValue("FR0125"));
          this.UpdateFieldValue("URLA.X75", this.GetFieldValue("FR0127"));
        }
        else
        {
          this.UpdateFieldValue("11", this.GetFieldValue("FR0104"));
          this.UpdateFieldValue("URLA.X73", "");
          this.UpdateFieldValue("URLA.X74", "");
          this.UpdateFieldValue("URLA.X75", "");
        }
        this.UpdateFieldValue("12", this.GetFieldValue("FR0106"));
        this.UpdateFieldValue("14", this.GetFieldValue("FR0107"));
        string fieldValue1 = this.GetFieldValue("FR0108");
        this.UpdateFieldValue("15", fieldValue1);
        string fieldValue2 = this.GetFieldValue("FR0109");
        if (!string.IsNullOrEmpty(fieldValue2) || string.IsNullOrEmpty(fieldValue2) && fieldValue1.Length == 0)
          this.UpdateFieldValue("13", this.GetFieldValue("FR0109"));
        else if (fieldValue1.Length > 4)
        {
          ZipCodeInfo infoWithUserDefined = ZipcodeSelector.GetZipCodeInfoWithUserDefined(fieldValue1);
          if (infoWithUserDefined != null)
            this.UpdateFieldValue("13", infoWithUserDefined.County);
        }
        this.UpdateContents();
      }
    }

    private void copyAddressIRS1098()
    {
      this.UpdateFieldValue("4097", this.GetFieldValue("11"));
      this.UpdateFieldValue("4098", this.GetFieldValue("12"));
      this.UpdateFieldValue("4099", this.GetFieldValue("14"));
      this.UpdateFieldValue("4100", this.GetFieldValue("15"));
    }

    private void copyHomeCounselingInfoFromBor()
    {
      this.UpdateFieldValue("URLA.X159", this.GetFieldValue("URLA.X153"));
      this.UpdateFieldValue("URLA.X160", this.GetFieldValue("URLA.X154"));
      this.UpdateFieldValue("URLA.X161", this.GetFieldValue("URLA.X155"));
      this.UpdateFieldValue("URLA.X244", this.GetFieldValue("URLA.X232"));
      this.UpdateFieldValue("URLA.X243", this.GetFieldValue("URLA.X233"));
      this.UpdateFieldValue("URLA.X215", "Y");
      this.UpdateContents();
    }

    private void copyHomeOwnerEducationInfoFromBor()
    {
      this.UpdateFieldValue("URLA.X300", this.GetFieldValue("URLA.X299"));
      this.UpdateFieldValue("URLA.X302", this.GetFieldValue("URLA.X301"));
      this.UpdateFieldValue("URLA.X304", this.GetFieldValue("URLA.X303"));
      this.UpdateFieldValue("URLA.X306", this.GetFieldValue("URLA.X305"));
      this.UpdateFieldValue("URLA.X308", this.GetFieldValue("URLA.X307"));
      this.UpdateContents();
    }

    private void copyLoanToSafeHarbor()
    {
      using (CopySafeHarborForm copySafeHarborForm = new CopySafeHarborForm(this.inputData))
      {
        if (copySafeHarborForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateContents();
      }
    }

    private void copySafeHarborToLoan()
    {
      if (!this.loan.Calculator.IsSyncGFERequired)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "You cannot copy information from the Loan Options screen since you already disclosed a GFE to the borrower.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        bool flag = false;
        switch (this.loan.GetField("DISCLOSURE.X732"))
        {
          case "Loan Option 1":
            flag = this.loan.GetField("DISCLOSURE.X695") != string.Empty;
            break;
          case "Loan Option 2":
            flag = this.loan.GetField("DISCLOSURE.X707") != string.Empty;
            break;
          case "Loan Option 3":
            flag = this.loan.GetField("DISCLOSURE.X714") != string.Empty;
            break;
          case "Loan Option 4":
            flag = this.loan.GetField("DISCLOSURE.X726") != string.Empty;
            break;
        }
        string str = string.Empty;
        if (flag)
        {
          for (int index = 1269; index <= 1274; ++index)
          {
            if (this.loan.GetField(string.Concat((object) index)) != string.Empty || this.loan.GetField(string.Concat((object) (index + 344))) != string.Empty)
            {
              str = "Buydown fields will be cleared";
              break;
            }
          }
        }
        string empty = string.Empty;
        if (MessageBox.Show((IWin32Window) this.session.MainForm, !this.loan.Use2015RESPA ? "The following field data (including blank fields) will overwrite the current loan data:\r\n\r\nLoan Type\r\nLoan Term\r\nInterest Rate\r\nInitial Fixed Interest Rate Period (if applicable)\r\nOrigination Points or Fees (to 2010 Itemization 801 Section)\r\nDiscount Points (to 2010 Itemization 802 Section" + (str != string.Empty ? " and " + str : "") + ")\r\n\r\nDo you want to continue?" : "The following field data (including blank fields) will overwrite the current loan data:\r\n\r\nLoan Type\r\nLoan Term\r\nInterest Rate\r\nInitial Fixed Interest Rate Period (if applicable)\r\nOrigination Points or Fees (to 2015 Itemization 801 Section)\r\nDiscount Points (to 2015 Itemization 802 Section" + (str != string.Empty ? " and " + str : "") + ")\r\n\r\nDo you want to continue?", "Copy Selected Option to Loan", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.loan.Calculator.CopySafeHarborToLoan();
      }
    }

    private void viewLenderRepDialog(string eSignerType, LoanData loan)
    {
      using (LenderRepresentativeLookup representativeLookup = new LenderRepresentativeLookup(eSignerType, loan))
      {
        int num = (int) representativeLookup.ShowDialog();
      }
    }

    private void viewTradeSummaryDialog()
    {
      if (this.loan == null || this.FormIsForTemplate)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "You cannot use Trade Summary in a template.");
      }
      else if (this.loan.GetField("2032") == "" && Session.LoanTradeManager.GetTradeHistoryForLoan(this.loan.GUID).Length == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "There is no trade history for this loan.");
      }
      else
      {
        using (TradeSummaryDialog tradeSummaryDialog = new TradeSummaryDialog(this.loan.GUID))
        {
          int num3 = (int) tradeSummaryDialog.ShowDialog((IWin32Window) Session.MainForm);
        }
      }
    }

    private void show2010SettlementFeeDialog()
    {
      IHtmlInput iData;
      if (this.loan != null)
      {
        iData = (IHtmlInput) this.loan;
      }
      else
      {
        if (this.inputData == null)
          return;
        iData = this.inputData;
      }
      using (SettlementFeeDialog settlementFeeDialog = new SettlementFeeDialog(iData, false))
      {
        if (settlementFeeDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        if (this.loan != null && this.editor != null)
          this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X645", false);
        if (this.loan != null)
        {
          this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
          if (this.loan.GetField("14").ToUpper() == "CA")
          {
            if (this.editor == null || this.editor.CurrentForm == "2010 Itemization")
              this.loan.Calculator.CopyGFEToMLDS();
            else if (this.editor.CurrentForm == "MLDS - CA GFE")
            {
              this.SetField("RE882.X56", this.GetFieldValue("NEWHUD.X782"));
              this.SetField("RE882.X57", this.GetFieldValue("NEWHUD.X645"));
              if (this.GetFieldValue("NEWHUD.X645") != string.Empty)
                this.SetField("NEWHUD.X803", "Other");
              else if (this.GetFieldValue("NEWHUD.X782") != string.Empty)
                this.SetField("NEWHUD.X803", "Broker");
              else
                this.SetField("NEWHUD.X803", "");
            }
            this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
          }
        }
        this.UpdateContents();
      }
    }

    private void show2010EscrowTitleFeeDialog()
    {
      TableHandler tableHandler = this.loan == null ? new TableHandler((LoanData) null, "2010_Escrow", this.session) : new TableHandler(this.loan, "2010_Escrow", this.session);
      if (!tableHandler.LookUpTable("Escrow"))
        return;
      if (this.loan != null)
      {
        if (!(this is HUD1PG2_2010InputHandler))
          this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X808", false);
        this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      }
      else
        this.inputData.SetCurrentField("ESCROW_TABLE", tableHandler.EscrowTableName);
      this.UpdateContents();
    }

    private void show2010OwnerTitleFeeDialog()
    {
      TableHandler tableHandler = this.loan == null ? new TableHandler((LoanData) null, "2010_OwnerTitle", this.session) : new TableHandler(this.loan, "2010_OwnerTitle", this.session);
      if (!tableHandler.LookUpTable("Title"))
        return;
      if (this.loan != null)
      {
        if (!(this is HUD1PG2_2010InputHandler))
          this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X572", false);
        this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      }
      else
        this.inputData.SetCurrentField("2010TITLE_TABLE", tableHandler.EscrowTableName);
      this.UpdateContents();
    }

    private void show2010LenderTitleFeeDialog()
    {
      TableHandler tableHandler = this.loan == null ? new TableHandler((LoanData) null, "2010_LenderTitle", this.session) : new TableHandler(this.loan, "2010_LenderTitle", this.session);
      if (!tableHandler.LookUpTable("Title"))
        return;
      if (this.loan != null)
      {
        if (!(this is HUD1PG2_2010InputHandler))
          this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X639", false);
        if (this is REGZGFE_2010InputHandler)
        {
          this.loan.Calculator.FormCalculation("REGZGFE_2010", "NEWHUD.X639", this.GetField("NEWHUD.X639"));
          this.loan.Calculator.CopyGFEToMLDS("NEWHUD.X639");
        }
        else if (this.loan.GetField("14").ToUpper() == "CA" && this is RE88395InputHandler)
          this.loan.Calculator.FormCalculation("MLDS", "RE882.X12", this.loan.GetField("RE882.X12"));
        this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      }
      else
        this.inputData.SetCurrentField("TITLE_TABLE", tableHandler.EscrowTableName);
      this.UpdateContents();
    }

    private void showContactSyncDialog()
    {
      string field1 = this.loan.GetField("37");
      string field2 = this.loan.GetField("36");
      string field3 = this.loan.GetField("69");
      string field4 = this.loan.GetField("68");
      bool flag1 = false;
      bool flag2 = false;
      if (field1 == string.Empty && field2 == string.Empty)
        flag1 = true;
      if (field3 == string.Empty && field4 == string.Empty)
        flag2 = true;
      if (flag1 & flag2)
        return;
      int num = (int) new SyncContactDialog(this.loan).ShowDialog();
    }

    private void showConditionListDialog()
    {
      if (this.loan.EnableEnhancedConditions)
      {
        using (EnhancedConditionCopyDialog conditionCopyDialog = new EnhancedConditionCopyDialog(this.loan))
        {
          if (conditionCopyDialog.ShowDialog((IWin32Window) null) != DialogResult.OK)
            return;
          string simpleField = conditionCopyDialog.IsListAppended ? this.loan.GetSimpleField("1952") : "";
          if (simpleField != string.Empty)
            simpleField += "\r\n\r\n";
          this.loan.SetCurrentField("1952", simpleField + conditionCopyDialog.CondList);
          this.UpdateContents();
        }
      }
      else
      {
        using (ClosingConditionCopyDialog conditionCopyDialog = new ClosingConditionCopyDialog(this.loan))
        {
          if (conditionCopyDialog.ShowDialog((IWin32Window) null) != DialogResult.OK)
            return;
          string simpleField = conditionCopyDialog.IsListAppended ? this.loan.GetSimpleField("1952") : "";
          if (simpleField != string.Empty)
            simpleField += "\r\n\r\n";
          this.loan.SetCurrentField("1952", simpleField + conditionCopyDialog.CondList);
          this.UpdateContents();
        }
      }
    }

    private void showMersMIN()
    {
      if (this.GetFieldValue("1051") != string.Empty && Utils.Dialog((IWin32Window) null, "The MERS MIN number exists. Do you want to set a new number?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      string userId = this.loan.GetField("LOID");
      if (userId == "")
        userId = this.loan.GetLogList().GetMilestone("Started").LoanAssociateID;
      if (userId == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "This loan doesn't have Loan Officer assigned.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        UserInfo user = this.session.OrganizationManager.GetUser(userId);
        if (user == (UserInfo) null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) null, "The assigned Loan Officer and File Starter can not be found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          OrgInfo organizationWithMersmin = this.session.OrganizationManager.GetFirstOrganizationWithMERSMIN(user.OrgId);
          if (organizationWithMersmin.MERSMINCode == "")
          {
            MersNumberingInfo mersNumberingInfo = this.session.ConfigurationManager.GetMersNumberingInfo();
            if (mersNumberingInfo.CompanyID.Trim() == "" || mersNumberingInfo.NextNumber.Trim() == string.Empty)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) null, "The MERS MIN number setup is not correct. Please contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          else
          {
            BranchMERSMINNumberingInfo mersNumberingInfo = this.session.ConfigurationManager.GetBranchMERSNumberingInfo(organizationWithMersmin.MERSMINCode);
            if (mersNumberingInfo.MERSMINCode.Trim() == "" || mersNumberingInfo.NextNumber.Trim() == string.Empty)
            {
              int num4 = (int) Utils.Dialog((IWin32Window) null, "The MERS MIN number setup is not correct. Please contact your Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          string nextMersNumber = this.session.LoanManager.GetNextMersNumber(true, organizationWithMersmin);
          this.UpdateFieldValue("1051", nextMersNumber);
          if (string.IsNullOrEmpty(nextMersNumber))
            return;
          this.UpdateFieldValue("4723", "Y");
          if (nextMersNumber.Length < 7)
            return;
          this.loan.SetCurrentFieldFromCal("4722", nextMersNumber.Substring(0, 7));
        }
      }
    }

    private void showMIPDialog(FieldSource fieldSource) => this.showMIPDialog(fieldSource, false);

    private void showMIPDialog(FieldSource fieldSource, bool readOnly)
    {
      bool flag1 = this.inputData.GetField("1172") == "FarmersHomeAdministration";
      MIPUSDADialog mipusdaDialog1 = (MIPUSDADialog) null;
      MIPDialog mipDialog1 = (MIPDialog) null;
      if (this.isUsingTemplate)
      {
        if (flag1)
        {
          MIPUSDADialog mipusdaDialog2;
          using (mipusdaDialog2 = new MIPUSDADialog(this.inputData))
          {
            int num = (int) mipusdaDialog2.ShowDialog((IWin32Window) this.mainScreen);
          }
        }
        else
        {
          MIPDialog mipDialog2;
          using (mipDialog2 = new MIPDialog(this.inputData, false, this.session))
          {
            int num = (int) mipDialog2.ShowDialog((IWin32Window) this.mainScreen);
          }
        }
      }
      else
      {
        DialogResult dialogResult;
        if (flag1)
        {
          mipusdaDialog1 = fieldSource != FieldSource.LinkedLoan || this.loan == null || this.loan.LinkedData == null ? new MIPUSDADialog(this.inputData) : new MIPUSDADialog((IHtmlInput) this.loan.LinkedData);
          dialogResult = mipusdaDialog1.ShowDialog((IWin32Window) this.mainScreen);
        }
        else
        {
          mipDialog1 = fieldSource != FieldSource.LinkedLoan || this.loan == null || this.loan.LinkedData == null ? new MIPDialog(this.loan) : new MIPDialog(this.loan.LinkedData);
          if (readOnly)
            mipDialog1.SetScreenDisabled();
          dialogResult = mipDialog1.ShowDialog((IWin32Window) this.mainScreen);
        }
        if (dialogResult == DialogResult.OK)
        {
          bool flag2 = false;
          if (this.loan != null && this.loan.Calculator != null)
          {
            flag2 = this.loan.Calculator.PerformanceEnabled;
            this.loan.Calculator.PerformanceEnabled = false;
          }
          if (this.loan != null && (this.loan.Use2010RESPA || this.loan.Use2015RESPA))
          {
            this.loan.Calculator.CopyHUD2010ToGFE2010("338", false);
            this.loan.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X1708", false);
          }
          try
          {
            if (this.loan != null)
            {
              if (this.loan.GetField("1172") == "FarmersHomeAdministration")
              {
                this.loan.SetField("234", "");
                this.loan.Calculator.FormCalculation("NEWHUD.X1707", (string) null, (string) null);
              }
              if (fieldSource == FieldSource.LinkedLoan && this.loan.LinkedData != null)
              {
                this.loan.LinkedData.Calculator.FormCalculation("1109", (string) null, (string) null);
                this.loan.LinkedData.TriggerCalculation("EEM.X75", (string) null);
              }
              else
              {
                this.loan.Calculator.FormCalculation("1109", (string) null, (string) null);
                this.loan.TriggerCalculation("EEM.X75", (string) null);
              }
            }
          }
          catch (CountyLimitException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          if (this.loan != null && this.loan.Calculator != null)
          {
            this.loan.Calculator.FormCalculation("LOANESTIMATEPAGE2", "", "");
            this.loan.Calculator.PerformanceEnabled = flag2;
          }
          this.UpdateContents();
        }
        if (mipDialog1 != null)
        {
          mipDialog1.Close();
          mipDialog1.Dispose();
        }
        if (mipusdaDialog1 == null)
          return;
        mipusdaDialog1.Close();
        mipusdaDialog1.Dispose();
      }
    }

    private void showCityFeeDialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("city", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        IHtmlInput htmlInput = !this.IsUsingTemplate ? (IHtmlInput) this.loan : this.inputData;
        htmlInput.SetCurrentField("647", feeListDialog.FeeTotal);
        htmlInput.SetField("2405", "");
        htmlInput.SetField("2406", "");
        if (htmlInput is LoanData)
        {
          this.UpdateFieldValue("1637", feeListDialog.FeeDescription);
          if (this.editor == null || this.editor.CurrentForm == "2010 Itemization")
          {
            if (htmlInput.GetField("14").ToUpper() == "CA")
              this.loan.Calculator.CopyGFEToMLDS("647");
            this.loan.Calculator.CopyHUD2010ToGFE2010("647", false);
          }
          else if (this.editor.CurrentForm == "MLDS - CA GFE")
          {
            this.UpdateFieldValue("RE88395.X94", feeListDialog.FeeTotal);
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X94", feeListDialog.FeeTotal);
          }
          this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        }
        else
          htmlInput.SetField("1637", feeListDialog.FeeDescription);
        this.UpdateContents();
      }
    }

    private void showCityFeeMLDSDialog()
    {
      FeeListDialog feeListDialog = new FeeListDialog("city", this.loan, this.session);
      if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      this.UpdateFieldValue("RE88395.X94", feeListDialog.FeeTotal);
      this.UpdateFieldValue("1637", feeListDialog.FeeDescription);
      this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
      this.UpdateContents();
    }

    private void selectPurchaseAdviceTemplate()
    {
      using (PurchaseAdviceTemplateSelector templateSelector = new PurchaseAdviceTemplateSelector(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice))
      {
        if (templateSelector.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK || this.loan == null)
          return;
        this.loan.SetPurchaseAdviceTemplate((PurchaseAdviceTemplate) templateSelector.SelectedTemplate, templateSelector.AppendTemplate);
        this.loan.TriggerCalculation("2596", this.GetFieldValue("2596"));
        this.RefreshContents();
      }
    }

    private void resetRequestForm()
    {
      if (this.loan == null || Utils.Dialog((IWin32Window) null, "Are you sure you want to reset this form with loan information?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      for (int index = 0; index < LockRequestLog.RequestFieldMap.Count; ++index)
        this.UpdateFieldValue(LockRequestLog.RequestFieldMap[index].Key, this.loan.GetField(LockRequestLog.RequestFieldMap[index].Value));
      string[] fields = this.loan.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true);
      for (int index = 0; index < fields.Length; ++index)
        this.UpdateFieldValue(LockRequestCustomField.GenerateCustomFieldID(fields[index]), this.GetFieldValue(fields[index]));
      this.loan.TriggerCalculation("2951", this.GetFieldValue("2951"));
      string field = this.GetField("CASASRN.X141");
      this.SetField("4631", field);
      if (field == "Borrower")
      {
        this.SetField("4633", this.GetField("1269"));
        this.SetField("4634", this.GetField("1270"));
        this.SetField("4635", this.GetField("1271"));
        this.SetField("4636", this.GetField("1272"));
        this.SetField("4637", this.GetField("1273"));
        this.SetField("4638", this.GetField("1274"));
        this.SetField("4639", this.GetField("1613"));
        this.SetField("4640", this.GetField("1614"));
        this.SetField("4641", this.GetField("1615"));
        this.SetField("4642", this.GetField("1616"));
        this.SetField("4643", this.GetField("1617"));
        this.SetField("4644", this.GetField("1618"));
      }
      else
      {
        this.SetField("4633", this.GetField("4535"));
        this.SetField("4634", this.GetField("4536"));
        this.SetField("4635", this.GetField("4537"));
        this.SetField("4636", this.GetField("4538"));
        this.SetField("4637", this.GetField("4539"));
        this.SetField("4638", this.GetField("4540"));
        this.SetField("4639", this.GetField("4541"));
        this.SetField("4640", this.GetField("4542"));
        this.SetField("4641", this.GetField("4543"));
        this.SetField("4642", this.GetField("4544"));
        this.SetField("4643", this.GetField("4545"));
        this.SetField("4644", this.GetField("4546"));
      }
      this.editor.RefreshContents();
    }

    private void showStateFeeDialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("state", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        IHtmlInput htmlInput = !this.IsUsingTemplate ? (IHtmlInput) this.loan : this.inputData;
        double num = Utils.ParseDouble((object) feeListDialog.FeeTotal) - Utils.ParseDouble((object) htmlInput.GetField("594"));
        htmlInput.SetCurrentField("648", num != 0.0 ? num.ToString("N2") : "");
        htmlInput.SetField("2407", "");
        htmlInput.SetField("2408", "");
        if (htmlInput is LoanData && (this.editor == null || this.editor.CurrentForm == "2010 Itemization"))
          this.loan.Calculator.CopyHUD2010ToGFE2010("648", false);
        if (htmlInput is LoanData)
        {
          this.UpdateFieldValue("1638", feeListDialog.FeeDescription);
          this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        }
        else
        {
          htmlInput.SetField("1638", feeListDialog.FeeDescription);
          if (htmlInput is ClosingCost && this is REGZGFE_2010InputHandler && !(this is REGZGFE_2015InputHandler))
          {
            REGZGFE_2010InputHandler obj = (REGZGFE_2010InputHandler) this;
            if (obj.ccLoan != null)
            {
              obj.SetClosingCostLoan(false, (string) null);
              obj.ccLoan.Calculator.CopyHUD2010ToGFE2010("648", false);
              obj.SetTemplate();
            }
          }
        }
        this.UpdateContents();
      }
    }

    private void showStateFeeMLDSDialog()
    {
      FeeListDialog feeListDialog = new FeeListDialog("state", this.loan, this.session);
      if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return;
      this.UpdateFieldValue("RE88395.X89", feeListDialog.FeeTotal);
      this.UpdateFieldValue("1638", feeListDialog.FeeDescription);
      this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
      this.UpdateContents();
    }

    private void showCustomizedProjectedPaymentTable()
    {
      using (EditProjectedPaymentTable projectedPaymentTable = new EditProjectedPaymentTable(this.session, (IHtmlInput) this.loan))
      {
        if (projectedPaymentTable.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.RefreshContents();
      }
    }

    private void showRegistrationDialog(bool isNew)
    {
      RegistrationLog latestLog = (RegistrationLog) null;
      if (!isNew)
      {
        latestLog = this.loan.GetLogList().GetLatestRegistrationLog();
        if (latestLog == null && Utils.Dialog((IWin32Window) this.session.MainForm, "You don't have valid Registration in this loan. Would you like to add one?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
      }
      using (AddRegistrationDialog registrationDialog = new AddRegistrationDialog(this.loan, latestLog))
      {
        if (registrationDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateContents();
        this.editor.RefreshContents();
      }
    }

    private void showUserFee1Dialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("user", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        IHtmlInput htmlInput = !this.IsUsingTemplate ? (IHtmlInput) this.loan : this.inputData;
        double num = Utils.ParseDouble((object) feeListDialog.FeeTotal) - Utils.ParseDouble((object) htmlInput.GetField("576"));
        htmlInput.SetCurrentField("374", num != 0.0 ? num.ToString("N2") : "");
        htmlInput.SetField("373", feeListDialog.FeeDescription);
        if (htmlInput is LoanData)
        {
          if (this.editor == null || this.editor.CurrentForm == "2010 Itemization")
          {
            if (htmlInput.GetField("14").ToUpper() == "CA")
              this.loan.Calculator.CopyGFEToMLDS("374");
            this.loan.Calculator.CopyHUD2010ToGFE2010("374", false);
          }
          else if (this.editor.CurrentForm == "MLDS - CA GFE")
          {
            this.UpdateFieldValue("RE88395.X99", feeListDialog.FeeTotal);
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X99", feeListDialog.FeeTotal);
          }
          this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        }
        this.UpdateContents();
      }
    }

    private void showUserFee1MLDSDialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("user", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateFieldValue("RE88395.X99", feeListDialog.FeeTotal);
        this.UpdateFieldValue("373", feeListDialog.FeeDescription);
        this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
        this.UpdateContents();
      }
    }

    private void showUserFee2Dialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("user", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        IHtmlInput htmlInput = !this.IsUsingTemplate ? (IHtmlInput) this.loan : this.inputData;
        double num = Utils.ParseDouble((object) feeListDialog.FeeTotal) - Utils.ParseDouble((object) htmlInput.GetField("1642"));
        htmlInput.SetCurrentField("1641", num != 0.0 ? num.ToString("N2") : "");
        htmlInput.SetField("1640", feeListDialog.FeeDescription);
        if (htmlInput is LoanData)
        {
          if (this.editor == null || this.editor.CurrentForm == "2010 Itemization")
            this.loan.Calculator.CopyHUD2010ToGFE2010("1641", false);
          this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        }
        this.UpdateContents();
      }
    }

    private void showUserFee2MLDSDialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("user", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateFieldValue("RE88395.X100", feeListDialog.FeeTotal);
        this.UpdateFieldValue("1640", feeListDialog.FeeDescription);
        this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
        this.UpdateContents();
      }
    }

    private void showUserFee3Dialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("user", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        IHtmlInput htmlInput = !this.IsUsingTemplate ? (IHtmlInput) this.loan : this.inputData;
        double num = Utils.ParseDouble((object) feeListDialog.FeeTotal) - Utils.ParseDouble((object) htmlInput.GetField("1645"));
        htmlInput.SetCurrentField("1644", num != 0.0 ? num.ToString("N2") : "");
        htmlInput.SetField("1643", feeListDialog.FeeDescription);
        if (htmlInput is LoanData)
        {
          if (this.editor == null || this.editor.CurrentForm == "2010 Itemization")
            this.loan.Calculator.CopyHUD2010ToGFE2010("1644", false);
          this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("SEC32", (string) null, (string) null);
          this.loan.Calculator.FormCalculation("HUD1PG2_2010", (string) null, (string) null);
        }
        this.UpdateContents();
      }
    }

    private void showUserFee3MLDSDialog()
    {
      using (FeeListDialog feeListDialog = new FeeListDialog("user", this.loan, this.session))
      {
        if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateFieldValue("RE88395.X169", feeListDialog.FeeTotal);
        this.UpdateFieldValue("1643", feeListDialog.FeeDescription);
        this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
        this.UpdateContents();
      }
    }

    private void updateLOCompensation(string id, string val)
    {
      if (this.loan == null)
        return;
      try
      {
        this.loan.Calculator.FormCalculation("UPDATECOMP", id, val);
      }
      catch (Exception ex)
      {
        if (id != string.Empty && id != "LCP.X1" || id == "LCP.X1" && this.GetField("LCP.X19") == string.Empty)
          return;
        string field = this.GetField("2626");
        if (field == "Brokered" && this.GetField("317") == string.Empty)
          return;
        if (field == "Brokered" && (ex.Message.IndexOf("no LO compensation plans can be applied for loan officer") > -1 || ex.Message.StartsWith("An LO compensation plan could not be found for loan officer")))
        {
          if (this.loan.Calculator.LOCompensationIsApplied(false, true, false))
          {
            if (Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message + " All fields in the Loan Officer Plan Details will be cleared. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
              this.loan.Calculator.FormCalculation("ClearLOcompInOfficerKeepName", (string) null, (string) null);
          }
          else
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        else if (field == "Brokered" && ex.Message.StartsWith("An LO compensation Plan could not be found for broker"))
        {
          if (this.loan.Calculator.LOCompensationIsApplied(true, false, false))
          {
            if (Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message + " All fields in the Wholesale/Broker Plan Details will be cleared. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
              this.loan.Calculator.FormCalculation("ClearLOcompInBrokerKeepName", (string) null, (string) null);
          }
          else
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        else
        {
          switch (field)
          {
            case "Banked - Retail":
              if (this.loan.Calculator.LOCompensationIsApplied(false, true, false))
              {
                if (Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message + " All fields in the Loan Officer Plan Details will be cleared. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                  this.loan.Calculator.FormCalculation("ClearLOcompInOfficerKeepName", (string) null, (string) null);
                  break;
                }
                break;
              }
              int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            case "Banked - Wholesale":
              if (this.loan.Calculator.LOCompensationIsApplied(true, false, false))
              {
                if (Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message + " All fields in the Wholesale/Broker Plan Details will be cleared. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                  this.loan.Calculator.FormCalculation("ClearLOcompInBrokerKeepName", (string) null, (string) null);
                  break;
                }
                break;
              }
              int num4 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            default:
              if (this.loan.Calculator.LOCompensationIsApplied(true, true, false))
              {
                if (Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message + " All fields in the LO Compensation tool will be cleared. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                  this.loan.Calculator.FormCalculation("clearlocomp", (string) null, (string) null);
                  break;
                }
                break;
              }
              int num5 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
          }
        }
      }
      if (id == string.Empty && val == string.Empty && this.loan != null && this.session != null && this.session.LoanDataMgr != null && this.session.LoanDataMgr.SystemConfiguration != null && this.session.LoanDataMgr.SystemConfiguration.LoanCompDefaultPlan != null && this.GetField(this.session.LoanDataMgr.SystemConfiguration.LoanCompDefaultPlan.TriggerField) != "//")
      {
        DateTime now = DateTime.Now;
        this.SetField("LCP.X19", now.ToString("MM/dd/yyyy h:mm:ss tt"));
        now = DateTime.Now;
        this.SetField("LCP.X20", now.Date.ToString("MM/dd/yyyy"));
      }
      this.validateLOCompTool(id, val);
    }

    private void lookupBrokerLOComp(FieldSource fieldSource)
    {
      using (LOCompBrokerSelector compBrokerSelector = new LOCompBrokerSelector(this.inputData.GetField("2626") == "Brokered"))
      {
        if (compBrokerSelector.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK || this.loan == null)
          return;
        this.loan.SetField("LCP.X2", compBrokerSelector.SelectedCompany.CompanyLegalName);
        this.loan.SetField("LCP.X18", compBrokerSelector.SelectedCompany.oid.ToString());
        if (this.loan.GetField("LCP.X19") != string.Empty || (EnableDisableSetting) Session.ServerManager.GetServerSetting("Components.LOCompensation") != EnableDisableSetting.Enabled)
          return;
        this.updateLOCompensation(string.Empty, string.Empty);
      }
    }

    private void validateLOCompTool(string id, string val)
    {
      string field = this.GetField("2626");
      if (field == "Banked - Wholesale" && this.loan.Calculator.LOCompensationIsApplied(false, true, true))
      {
        if (Utils.Dialog((IWin32Window) this.session.MainForm, "Would you like to clear Loan Officer compensation plan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.loan.Calculator.FormCalculation("CLEARLOCOMPINOFFICER", id, val);
      }
      else
      {
        if (!(field == "Banked - Retail") || !this.loan.Calculator.LOCompensationIsApplied(true, false, true) || Utils.Dialog((IWin32Window) this.session.MainForm, "Would you like to clear Wholesale/Broker compensation plan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.loan.Calculator.FormCalculation("CLEARLOCOMPINBROKER", id, val);
      }
    }

    private void lookupTPOLoanOfficer()
    {
      if (this.orgs == null)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.orgs = Session.ConfigurationManager.GetExternalOrganizationsWithoutExtension(Session.UserID, (string) null);
        Cursor.Current = Cursors.Default;
      }
      if (this.orgs == null || this.orgs.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No External Organization found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (TPOLoanOfficerSelectorForm officerSelectorForm = new TPOLoanOfficerSelectorForm(this.orgs, this.session))
        {
          if (officerSelectorForm.ShowDialog((IWin32Window) this.session.MainForm) == DialogResult.OK)
          {
            this.company = officerSelectorForm.SelectedOrganization;
            this.branch = officerSelectorForm.SelectedBranch;
            this.loUser = officerSelectorForm.getSelectedLO();
            this.loFlag = true;
          }
          else
            this.loFlag = false;
        }
      }
    }

    private void lookupTPOOrganization()
    {
      if (this.orgs == null)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.orgs = Session.ConfigurationManager.GetExternalOrganizationsWithoutExtension(Session.UserID, (string) null);
        Cursor.Current = Cursors.Default;
      }
      if (this.orgs == null || this.orgs.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No External Organization found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (TPOCompanySelectorForm companySelectorForm = new TPOCompanySelectorForm(this.orgs, this.session, false))
        {
          if (companySelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          this.UpdateFieldValue("TPO.X14", companySelectorForm.SelectedOrganization.OrganizationName);
          this.UpdateFieldValue("TPO.X15", companySelectorForm.SelectedOrganization.ExternalID);
          this.loan.Calculator.FormCalculation("COPYTPOCUSTOMFIELDSTOLOANFIELDS", (string) null, "company");
          if (this.loan == null || string.Compare(this.GetField("2626"), "Banked - Wholesale", true) != 0)
            return;
          this.UpdateFieldValue("LCP.X2", companySelectorForm.SelectedOrganization.OrganizationName);
          this.UpdateFieldValue("LCP.X18", string.Concat((object) companySelectorForm.SelectedOrganization.oid));
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The selected company name will be copied to LO Compensation Tool.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          if (this.GetField("LCP.X19") != string.Empty)
            return;
          this.updateLOCompensation(string.Empty, string.Empty);
        }
      }
    }

    private void lookupTPODBAName()
    {
      Cursor.Current = Cursors.WaitCursor;
      Cursor.Current = Cursors.WaitCursor;
      string fieldValue = this.GetFieldValue("TPO.X15");
      if (fieldValue == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No External Organization found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<ExternalOriginatorManagementData> organizationByTpoid = Session.ConfigurationManager.GetExternalOrganizationByTPOID(fieldValue);
        ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
        foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid)
        {
          if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company)
          {
            originatorManagementData1 = originatorManagementData2;
            break;
          }
        }
        if (originatorManagementData1 == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No External Organization found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          List<ExternalOrgDBAName> dbaNames = Session.ConfigurationManager.GetDBANames(originatorManagementData1.oid);
          Cursor.Current = Cursors.Default;
          if (dbaNames == null || dbaNames.Count == 0)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No DBA Names found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            using (TPODBANameSelectorForm nameSelectorForm = new TPODBANameSelectorForm(dbaNames))
            {
              if (nameSelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
                return;
              this.UpdateFieldValue("TPO.X24", nameSelectorForm.SelectedDBAName.Name);
            }
          }
        }
      }
    }

    private void lookupTPOBranchDBAName()
    {
      Cursor.Current = Cursors.WaitCursor;
      string fieldValue = this.GetFieldValue("TPO.X39");
      if (fieldValue == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No External Organization found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<ExternalOriginatorManagementData> organizationByTpoid = Session.ConfigurationManager.GetExternalOrganizationByTPOID(fieldValue);
        ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
        foreach (ExternalOriginatorManagementData originatorManagementData2 in organizationByTpoid)
        {
          if (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Branch)
          {
            originatorManagementData1 = originatorManagementData2;
            break;
          }
        }
        if (originatorManagementData1 == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No External Organization found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          List<ExternalOrgDBAName> dbaNames = Session.ConfigurationManager.GetDBANames(originatorManagementData1.oid);
          Cursor.Current = Cursors.Default;
          if (dbaNames == null || dbaNames.Count == 0)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "No DBA Names found!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            using (TPODBANameSelectorForm nameSelectorForm = new TPODBANameSelectorForm(dbaNames))
            {
              if (nameSelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
                return;
              this.UpdateFieldValue("TPO.X48", nameSelectorForm.SelectedDBAName.Name);
            }
          }
        }
      }
    }

    private void lookupTPOBranch()
    {
      if (this.TPOBranches == null)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.TPOBranches = this.session.ConfigurationManager.GetExternalOrganizationsWithoutExtension(Session.UserID, this.GetField("TPO.X15"));
        Cursor.Current = Cursors.Default;
      }
      using (TPOCompanySelectorForm companySelectorForm = new TPOCompanySelectorForm(this.TPOBranches, this.session, true))
      {
        if (companySelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.UpdateFieldValue("TPO.X38", companySelectorForm.SelectedOrganization.OrganizationName);
        this.UpdateFieldValue("TPO.X39", companySelectorForm.SelectedOrganization.ExternalID);
        this.loan.Calculator.FormCalculation("COPYTPOCUSTOMFIELDSTOLOANFIELDS", (string) null, "branch");
      }
    }

    private void viewTPOExternalOrganizationInfo(bool viewBranch)
    {
      string TPOId = viewBranch ? this.GetField("TPO.X39") : this.GetField("TPO.X15");
      if (string.IsNullOrEmpty(TPOId))
        return;
      ExternalOriginatorManagementData externalCompanyByTpoid = this.session.ConfigurationManager.GetExternalCompanyByTPOID(false, TPOId);
      if (externalCompanyByTpoid == null)
        return;
      Session.MainScreen.DisplayTPOCompanySetting(externalCompanyByTpoid);
    }

    private void updateClosingVendorLicense(string action)
    {
      if (this.loan == null)
        return;
      string field1 = this.loan.GetField("2626");
      string field2 = this.loan.GetField("LOID");
      string field3 = this.loan.GetField("14");
      if (field1 == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "To update state license, you have to select a channel type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string field4 = this.loan.GetField("TPO.X39");
        if (field4 == string.Empty)
          field4 = this.loan.GetField("TPO.X15");
        string field5 = this.loan.GetField("TPO.X62");
        if (string.Compare(field1, "Banked - Retail", true) == 0 || string.Compare(field1, "Brokered", true) == 0)
        {
          if (field2 == string.Empty)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "To update license for channel '" + field1 + "', you need to assign loan to a loan officer first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            UserInfo user = this.session.OrganizationManager.GetUser(field2);
            OrgInfo vendorInformation = user != (UserInfo) null ? this.session.OrganizationManager.GetOrganizationForClosingVendorInformation(user.OrgId) : (OrgInfo) null;
            List<StateLicenseExtType> licenses = vendorInformation?.OrgBranchLicensing.GetLicenses(field3);
            if (licenses == null || licenses.Count == 0)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "You don't have any state license for state '" + field3 + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              using (StateLicenseSelectorForm licenseSelectorForm = new StateLicenseSelectorForm(licenses, true))
              {
                if (licenseSelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
                  return;
                if (string.Compare(action, "updatelenderlicense", true) == 0)
                {
                  this.loan.Calculator.SetInternalLenderLicense(vendorInformation != null ? vendorInformation.CompanyName : "", vendorInformation != null ? vendorInformation.CompanyAddress.Street1 + " " + vendorInformation.CompanyAddress.Street2 : "", vendorInformation != null ? vendorInformation.CompanyAddress.City : "", vendorInformation != null ? vendorInformation.CompanyAddress.State : "", vendorInformation != null ? vendorInformation.CompanyAddress.Zip : "", vendorInformation != null ? vendorInformation.NMLSCode : "", vendorInformation?.OrgBranchLicensing, licenseSelectorForm.SelectedLicense);
                }
                else
                {
                  if (string.Compare(action, "updatebrokerlicense", true) != 0)
                    return;
                  this.loan.Calculator.SetInternalBrokerLicense(vendorInformation != null ? vendorInformation.CompanyName : "", vendorInformation != null ? vendorInformation.CompanyAddress.Street1 + " " + vendorInformation.CompanyAddress.Street2 : "", vendorInformation != null ? vendorInformation.CompanyAddress.City : "", vendorInformation != null ? vendorInformation.CompanyAddress.State : "", vendorInformation != null ? vendorInformation.CompanyAddress.Zip : "", vendorInformation != null ? vendorInformation.NMLSCode : "", vendorInformation?.OrgBranchLicensing, licenseSelectorForm.SelectedLicense);
                }
              }
            }
          }
        }
        else
        {
          if (string.Compare(field1, "Banked - Wholesale", true) != 0 && string.Compare(field1, "Correspondent", true) != 0)
            return;
          if (field4 == string.Empty)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "To update license for channel '" + field1 + "', you need to select a TPO Organization in the TPO Information Tool input screen.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            ExternalOriginatorManagementData originatorManagementData = (ExternalOriginatorManagementData) null;
            BranchExtLicensing branchExtLicensing = (BranchExtLicensing) null;
            OrgInfo orgInfo = (OrgInfo) null;
            List<object> vendorInformation = this.session.ConfigurationManager.GetTPOForClosingVendorInformation(field4, field5);
            if (vendorInformation != null && vendorInformation.Count > 0)
            {
              foreach (object obj in vendorInformation)
              {
                switch (obj)
                {
                  case ExternalOriginatorManagementData _:
                    originatorManagementData = (ExternalOriginatorManagementData) obj;
                    continue;
                  case BranchExtLicensing _:
                    branchExtLicensing = (BranchExtLicensing) obj;
                    continue;
                  case OrgInfo _:
                    orgInfo = (OrgInfo) obj;
                    continue;
                  default:
                    continue;
                }
              }
            }
            bool translateLicenseType = true;
            List<StateLicenseExtType> licenses;
            if (string.Compare(action, "updateinvestorlicense", true) == 0 || string.Compare(action, "updatelenderlicense", true) == 0 && string.Compare(field1, "Banked - Wholesale", true) == 0)
            {
              licenses = orgInfo == null || orgInfo.OrgBranchLicensing == null ? (List<StateLicenseExtType>) null : orgInfo.OrgBranchLicensing.GetLicenses(field3);
            }
            else
            {
              licenses = branchExtLicensing?.GetLicenses(field3);
              translateLicenseType = false;
            }
            if (licenses == null || licenses.Count == 0)
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "You don't have any state license for state '" + field3 + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              using (StateLicenseSelectorForm licenseSelectorForm = new StateLicenseSelectorForm(licenses, translateLicenseType))
              {
                if (licenseSelectorForm.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
                  return;
                string tpoEntityType = originatorManagementData.TypeOfEntity <= -1 || originatorManagementData.TypeOfEntity >= Utils.TPOEntityTypes.Count ? "" : Utils.TPOEntityTypes[originatorManagementData.TypeOfEntity];
                if (string.Compare(action, "updatelenderlicense", true) == 0)
                {
                  if (string.Compare(field1, "Banked - Wholesale", true) == 0)
                    this.loan.Calculator.SetInternalLenderLicense(orgInfo != null ? orgInfo.CompanyName : "", orgInfo != null ? orgInfo.CompanyAddress.Street1 + " " + orgInfo.CompanyAddress.Street2 : "", orgInfo != null ? orgInfo.CompanyAddress.City : "", orgInfo != null ? orgInfo.CompanyAddress.State : "", orgInfo != null ? orgInfo.CompanyAddress.Zip : "", orgInfo != null ? orgInfo.NMLSCode : "", orgInfo?.OrgBranchLicensing, licenseSelectorForm.SelectedLicense);
                  else
                    this.loan.Calculator.SetExternalLenderLicense(originatorManagementData != null ? (originatorManagementData.CompanyDBAName == string.Empty ? originatorManagementData.CompanyLegalName : originatorManagementData.CompanyDBAName) : "", originatorManagementData != null ? originatorManagementData.Address : "", originatorManagementData != null ? originatorManagementData.City : "", originatorManagementData != null ? originatorManagementData.State : "", originatorManagementData != null ? originatorManagementData.Zip : "", originatorManagementData != null ? originatorManagementData.StateIncorp : "", tpoEntityType, originatorManagementData != null ? originatorManagementData.NmlsId : "", branchExtLicensing, licenseSelectorForm.SelectedLicense);
                }
                else if (string.Compare(action, "updatebrokerlicense", true) == 0)
                {
                  this.loan.Calculator.SetExternalBrokerLicense(originatorManagementData != null ? (originatorManagementData.CompanyDBAName == string.Empty ? originatorManagementData.CompanyLegalName : originatorManagementData.CompanyDBAName) : "", originatorManagementData != null ? originatorManagementData.Address : "", originatorManagementData != null ? originatorManagementData.City : "", originatorManagementData != null ? originatorManagementData.State : "", originatorManagementData != null ? originatorManagementData.Zip : "", originatorManagementData != null ? originatorManagementData.StateIncorp : "", tpoEntityType, originatorManagementData != null ? originatorManagementData.TaxID : "", originatorManagementData != null ? originatorManagementData.NmlsId : "", branchExtLicensing, licenseSelectorForm.SelectedLicense);
                }
                else
                {
                  if (string.Compare(action, "updateinvestorlicense", true) != 0)
                    return;
                  this.loan.Calculator.SetInternalInvestorLicense(orgInfo != null ? orgInfo.CompanyName : "", orgInfo != null ? orgInfo.CompanyAddress.Street1 + " " + orgInfo.CompanyAddress.Street2 : "", orgInfo != null ? orgInfo.CompanyAddress.City : "", orgInfo != null ? orgInfo.CompanyAddress.State : "", orgInfo != null ? orgInfo.CompanyAddress.Zip : "", licenseSelectorForm.SelectedLicense);
                }
              }
            }
          }
        }
      }
    }

    private void viewHELOCExamplePaymentSchedules()
    {
      if (this.loan == null || this.loan.IsTemplate || this.loan.IsInFindFieldForm)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "HELOC Example Payment cannot be generated in setting.");
      }
      else
      {
        List<List<string[]>> exampleSchedules = this.loan.Calculator.GetHelocExampleSchedules();
        if (exampleSchedules == null || exampleSchedules.Count == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "HELOC Example Payment cannot be generated based on current loan data." + Environment.NewLine + Environment.NewLine + this.loan.Calculator.GetLastErrorFromHelocExampleSchedules());
        }
        else
        {
          string field = this.loan.GetField("HHI.X4");
          using (HELOCExamplePaymentsForm examplePaymentsForm = new HELOCExamplePaymentsForm(exampleSchedules, field))
          {
            int num3 = (int) examplePaymentsForm.ShowDialog((IWin32Window) this.session.MainForm);
          }
        }
      }
    }

    private void viewQMPaymentSchedule()
    {
      if (this.loan == null || this.loan.IsTemplate || this.loan.IsInFindFieldForm)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "QM Payment Stream cannot be generated in setting.");
      }
      else
      {
        List<string[]> atrqmPaymentSchedule = this.loan.Calculator.GetATRQMPaymentSchedule();
        if (atrqmPaymentSchedule == null || atrqmPaymentSchedule.Count == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Based on this loan's data, there's no separate QM Payment Stream. QM APR (field ID: QM.X381) is set to the same as loan APR (field ID: 799).");
        }
        else
        {
          using (QMPaymentsForm qmPaymentsForm = new QMPaymentsForm(atrqmPaymentSchedule))
          {
            int num3 = (int) qmPaymentsForm.ShowDialog((IWin32Window) this.session.MainForm);
          }
        }
      }
    }

    private void viewWorstCaseScenarioPayment()
    {
      if (this.loan == null || this.FormIsForTemplate)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass can't generate worst case scenario payment schedule according to current loan data.");
      }
      else
      {
        LoanData worstCaseScenario = this.loan.Calculator.CalculateWorstCaseScenario();
        PaymentScheduleSnapshot paymentSchedule = worstCaseScenario.Calculator.GetPaymentSchedule(false);
        if (paymentSchedule == null || paymentSchedule.MonthlyPayments == null || paymentSchedule.MonthlyPayments.Length == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass can't generate worst case scenario payment schedule according to current loan data.");
        }
        else
        {
          using (AmortSchDialog amortSchDialog = new AmortSchDialog(worstCaseScenario, paymentSchedule, true, (PaymentScheduleSnapshot) null))
          {
            int num3 = (int) amortSchDialog.ShowDialog((IWin32Window) this.session.MainForm);
          }
        }
      }
    }

    private void getLateFees()
    {
      if (Session.Application.GetService<ILoanServices>().GetAllowedDocumentOrderTypes() == DocumentOrderType.None)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Your Encompass license does not allow you access to this feature. Contact your ICE Mortgage Technology Account Representative for more information.");
      }
      else
      {
        if (!this.lateFeeRequiredFieldCheck())
          return;
        LateFees lateFees = new DocEngineService(this.session.SessionObjects).GetLateFees(new Dictionary<string, string>()
        {
          {
            "InvestorCode",
            this.session.LoanData.GetField("PlanCode.InvCd")
          },
          {
            "PropertyState",
            this.session.LoanData.GetField("14")
          },
          {
            "LoanAmount",
            this.session.LoanData.GetField("2")
          },
          {
            "LoanType",
            this.session.LoanData.GetField("1172")
          },
          {
            "LienPosition",
            this.session.LoanData.GetField("420")
          },
          {
            "PlanCdId",
            this.session.LoanData.GetField("PlanCode.ID")
          },
          {
            "PrincipalAndInterest",
            this.session.LoanData.GetField("5")
          },
          {
            "AltLenderDescription",
            this.session.LoanData.GetField("1882")
          },
          {
            "LenderLicenseType",
            this.session.LoanData.GetField("3897")
          },
          {
            "AmortizationType",
            this.session.LoanData.GetField("608")
          },
          {
            "LoanDisbursementDate",
            this.session.LoanData.GetField("2553")
          },
          {
            "PropertyType",
            this.session.LoanData.GetField("1041")
          },
          {
            "NumberOfUnits",
            this.session.LoanData.GetField("16")
          },
          {
            "TransactionType",
            this.session.LoanData.GetField("384")
          },
          {
            "PIAmount",
            this.session.LoanData.GetField("1822")
          },
          {
            "EstClosingDt",
            this.session.LoanData.GetField("763")
          },
          {
            "ClosingDt",
            this.session.LoanData.GetField("748")
          },
          {
            "DocSigningDt",
            this.session.LoanData.GetField("1887")
          },
          {
            "OccupancyType",
            this.session.LoanData.GetField("190")
          },
          {
            "UnitCount",
            this.session.LoanData.GetField("16")
          },
          {
            "BrokerLicenseType",
            this.session.LoanData.GetField("VEND.X652")
          },
          {
            "BrokerLicenseTypeExemptionFlag",
            this.session.LoanData.GetField("VEND.X653")
          },
          {
            "LenderEntityType",
            this.session.LoanData.GetField("3895")
          },
          {
            "LenderLicenseExemptionFlag",
            this.session.LoanData.GetField("3898")
          },
          {
            "APR",
            this.session.LoanData.GetField("799")
          },
          {
            "RateLockDate",
            this.session.LoanData.GetField("761")
          },
          {
            "ApplicationDate",
            this.session.LoanData.GetField("745")
          },
          {
            "RateSetDate",
            this.session.LoanData.GetField("3253")
          },
          {
            "ConstructionInitialAcquisitionOfLand",
            this.session.LoanData.GetField("1964")
          },
          {
            "HomeState",
            this.session.LoanData.GetField("3896")
          },
          {
            "DisclosedAPR",
            this.session.LoanData.GetField("799")
          },
          {
            "NoteRate",
            this.session.LoanData.GetField("3")
          },
          {
            "FullyIndexedRate",
            this.session.LoanData.GetField("1827")
          },
          {
            "LoanPurpose",
            this.session.LoanData.GetField("19")
          },
          {
            "PlanCode",
            this.session.LoanData.GetField("1881")
          },
          {
            "Print012001GSEInstruments",
            this.session.LoanData.GetField("4793")
          },
          {
            "PropertyProjectType",
            this.session.LoanData.GetField("1012")
          },
          {
            "ConstructionRefiIndicator",
            this.session.LoanData.GetField("Constr.Refi")
          },
          {
            "LoanToValue",
            this.session.LoanData.GetField("353")
          },
          {
            "TermDueIn",
            this.session.LoanData.GetField("325")
          },
          {
            "TotalAmtFinanced",
            this.session.LoanData.GetField("948")
          },
          {
            "Sec32HighCostMtg",
            this.session.LoanData.GetField("S32DISC.X51")
          },
          {
            "Sec35HigherPricedMtgLoan",
            this.session.LoanData.GetField("3135")
          },
          {
            "LoanInfoChannel",
            this.session.LoanData.GetField("2626")
          },
          {
            "LateFeeAdditionalDetails",
            this.session.LoanData.GetField("1854")
          }
        });
        if (lateFees == null)
          return;
        this.session.LoanData.SetField("672", string.Concat((object) lateFees.LateFeeGracePeriodDays));
        this.session.LoanData.SetField("674", lateFees.LateChargePercent.ToString("0.000"));
        this.session.LoanData.SetField("2831", lateFees.LateFeeMin.ToString("0.00"));
        this.session.LoanData.SetField("2832", lateFees.LateFeeMax.ToString("0.00"));
        this.session.LoanData.SetField("1719", lateFees.Basis);
      }
    }

    private bool lateFeeRequiredFieldCheck()
    {
      bool flag = true;
      List<string> stringList = new List<string>();
      if (this.inputData.GetField("1172") == string.Empty)
        stringList.Add("1172");
      if (this.inputData.GetField("14") == string.Empty)
        stringList.Add("14");
      if (this.inputData.GetField("2") == string.Empty)
        stringList.Add("2");
      if (this.inputData.GetField("420") == string.Empty)
        stringList.Add("420");
      if (this.inputData.GetField("5") == string.Empty)
        stringList.Add("5");
      if (stringList.Count > 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please provide data for the following fields to obtain the Late Fee:" + Environment.NewLine + string.Join(", ", stringList.ToArray()), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    private void viewWorstCaseScenarioPaymentInRegz()
    {
      if (this.loan == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass can't generate worst case scenario payment schedule according to current loan data.");
      }
      else
      {
        if (this.loan.Calculator != null)
          this.loan.Calculator.PerformanceEnabled = false;
        Cursor.Current = Cursors.WaitCursor;
        PaymentScheduleSnapshot bestPaySnapshot = (PaymentScheduleSnapshot) null;
        LoanData loanData;
        PaymentScheduleSnapshot paymentSchedule;
        if (this.loan.Calculator.UseNew2015GFEHUD || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMGreater5Years || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMIntOnly || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMIntOnly31 || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMIntOnly51 || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMIntOnly7_1or10_1 || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMIntOnly3C || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMIO_L60 || this.loan.Calculator.RegzSummaryType == RegzSummaryTableType.ARMLess5Years)
        {
          loanData = this.loan.Calculator.CalculateWorstCaseScenario(false);
          paymentSchedule = loanData.Calculator.GetPaymentSchedule(false);
          if (this.loan.GetField("608") != "Fixed")
            bestPaySnapshot = this.loan.Calculator.GetBestCaseScenarioPaymentSchedule(loanData);
        }
        else
        {
          loanData = this.loan;
          paymentSchedule = this.loan.Calculator.GetPaymentSchedule(false);
        }
        if (paymentSchedule == null || paymentSchedule.MonthlyPayments == null || paymentSchedule.MonthlyPayments.Length == 0)
        {
          Cursor.Current = Cursors.Default;
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass can't generate worst case scenario payment schedule according to current loan data.");
        }
        else
        {
          Cursor.Current = Cursors.Default;
          using (AmortSchDialog amortSchDialog = new AmortSchDialog(loanData, paymentSchedule, true, bestPaySnapshot))
          {
            if (this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMGreater5Years && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMIntOnly && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMIntOnly31 && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMIntOnly51 && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMIntOnly7_1or10_1 && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMIntOnly3C && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMIO_L60 && this.loan.Calculator.RegzSummaryType != RegzSummaryTableType.ARMLess5Years)
              amortSchDialog.Text = "Amortization Schedule";
            int num3 = (int) amortSchDialog.ShowDialog((IWin32Window) this.session.MainForm);
          }
        }
      }
    }

    private void event_CopyFromBorrower()
    {
      string field = this.loan.GetField("1825");
      if (field != "2020")
        this.UpdateFieldValue("4006", this.GetFieldValue("4002"));
      this.UpdateFieldValue("98", this.GetFieldValue("66"));
      if (field != "2020")
      {
        this.UpdateFieldValue("1820", this.GetFieldValue("1819"));
        this.UpdateFieldValue("1519", this.GetFieldValue("1416"));
        this.UpdateFieldValue("1520", this.GetFieldValue("1417"));
        this.UpdateFieldValue("1521", this.GetFieldValue("1418"));
        this.UpdateFieldValue("1522", this.GetFieldValue("1419"));
        this.UpdateFieldValue("84", this.GetFieldValue("52"));
      }
      if (field == "2020")
      {
        this.UpdateFieldValue("FR0229", this.GetFieldValue("FR0129"));
        this.UpdateFieldValue("FR0204", this.GetFieldValue("FR0104"));
        this.UpdateFieldValue("FR0206", this.GetFieldValue("FR0106"));
        this.UpdateFieldValue("FR0207", this.GetFieldValue("FR0107"));
        this.UpdateFieldValue("FR0208", this.GetFieldValue("FR0108"));
        this.UpdateFieldValue("FR0226", this.GetFieldValue("FR0126"));
        this.UpdateFieldValue("FR0225", this.GetFieldValue("FR0125"));
        this.UpdateFieldValue("FR0227", this.GetFieldValue("FR0127"));
        this.UpdateFieldValue("FR0228", this.GetFieldValue("FR0128"));
        this.UpdateFieldValue("FR0212", this.GetFieldValue("FR0112"));
        this.UpdateFieldValue("FR0224", this.GetFieldValue("FR0124"));
        this.UpdateFieldValue("FR0230", this.GetFieldValue("FR0130"));
        this.populateCountyToVOR("FR0208", FieldSource.CurrentLoan, this.GetFieldValue("FR0109"));
      }
      this.UpdateFieldValue("4009", this.GetFieldValue("4008"));
      if (this.loan != null)
      {
        this.loan.CopyResidence();
        this.loan.Calculator.UpdateAccountName("68");
      }
      this.UpdateContents();
    }

    private void event_CopyMailingAddress()
    {
      this.UpdateFieldValue("URLA.X268", this.GetFieldValue("URLA.X267"));
      this.UpdateFieldValue("1519", this.GetFieldValue("1416"));
      this.UpdateFieldValue("URLA.X198", this.GetFieldValue("URLA.X197"));
      this.UpdateFieldValue("URLA.X9", this.GetFieldValue("URLA.X7"));
      this.UpdateFieldValue("URLA.X10", this.GetFieldValue("URLA.X8"));
      this.UpdateFieldValue("1520", this.GetFieldValue("1417"));
      this.UpdateFieldValue("1521", this.GetFieldValue("1418"));
      this.UpdateFieldValue("1522", this.GetFieldValue("1419"));
      this.UpdateFieldValue("URLA.X12", this.GetFieldValue("URLA.X11"));
      this.UpdateFieldValue("URLA.X270", this.GetFieldValue("URLA.X269"));
      this.UpdateContents();
    }

    private void event_CopyFormerAddress()
    {
      this.SetField("FR0404", this.GetFieldValue("FR0304"));
      string fieldValue1 = this.GetFieldValue("FR0329");
      this.UpdateFieldValue("FR0429", fieldValue1);
      this.UpdateFieldValue("FR0404", this.GetFieldValue("FR0304"));
      this.UpdateFieldValue("FR0426", this.GetFieldValue("FR0326"));
      this.UpdateFieldValue("FR0425", this.GetFieldValue("FR0325"));
      this.UpdateFieldValue("FR0427", this.GetFieldValue("FR0327"));
      this.UpdateFieldValue("FR0406", this.GetFieldValue("FR0306"));
      this.UpdateFieldValue("FR0407", this.GetFieldValue("FR0307"));
      this.UpdateFieldValue("FR0428", this.GetFieldValue("FR0328"));
      this.UpdateFieldValue("FR0412", this.GetFieldValue("FR0312"));
      this.UpdateFieldValue("FR0424", this.GetFieldValue("FR0324"));
      this.UpdateFieldValue("FR0430", this.GetFieldValue("FR0330"));
      string fieldValue2 = this.GetFieldValue("FR0308");
      this.UpdateFieldValue("FR0408", fieldValue2);
      if (fieldValue1 != "Y" && fieldValue2 != "" && fieldValue2.Length >= 5)
      {
        string countyName = "";
        int numberOfResidence = this.loan.GetNumberOfResidence(true);
        for (int index = 1; index <= numberOfResidence; ++index)
        {
          if (this.GetFieldValue("BR" + index.ToString("00") + "23") == "Prior")
          {
            countyName = this.GetFieldValue("BR" + index.ToString("00") + "22");
            break;
          }
        }
        if (!string.IsNullOrEmpty(countyName))
        {
          this.populateCountyToVOR("FR0408", FieldSource.CurrentLoan, countyName);
        }
        else
        {
          ZipCodeInfo infoWithUserDefined = ZipcodeSelector.GetZipCodeInfoWithUserDefined(fieldValue2.Substring(0, 5));
          if (infoWithUserDefined != null)
            this.populateCountyToVOR("FR0408", FieldSource.CurrentLoan, infoWithUserDefined.County);
        }
      }
      this.UpdateContents();
    }

    private void showHUD1ESSetupDialog()
    {
      if (!this.HasExclusiveRights(true) && this.loan != null && !this.FormIsForTemplate)
        return;
      if (!this.FormIsForTemplate && (this.loan.GetSimpleField("682") == "" || this.loan.GetSimpleField("682") == "\\"))
      {
        int num = (int) MessageBox.Show((IWin32Window) null, "You must enter the 1st Payment Date first.", "Error - 1st Payment Date Required", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (HUD1ESSetupDialog huD1EsSetupDialog = new HUD1ESSetupDialog(this.loan))
        {
          if (huD1EsSetupDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return;
          if (!this.FormIsForTemplate)
          {
            this.loan.Calculator.FormCalculation("HUD1ES", (string) null, (string) null);
            if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
            {
              this.loan.Calculator.FormCalculation("ITEMIZATION_SECTION1000", (string) null, (string) null);
              if (this.editor.CurrentForm == "2010 Itemization" || this.editor.CurrentForm == "2015 Itemization")
              {
                this.loan.Calculator.CopyHUD2010ToGFE2010("1387", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("656", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("1296", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("338", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("1386", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("655", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("L267", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("L269", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("1388", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("657", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("1629", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("1631", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("340", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("658", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("341", false);
                this.loan.Calculator.CopyHUD2010ToGFE2010("659", false);
              }
              this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
            }
          }
          this.UpdateContents();
        }
      }
    }

    private void showHUD1ADialog()
    {
      if (new HUD1ADialog(this.loan, this.mainScreen).ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
        return;
      this.UpdateContents();
    }

    private void showAltLenderDialog()
    {
      this.epass.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;ALTLENDERLIST", false);
      this.UpdateContents();
    }

    private void clearLoanProgram()
    {
      if (this.IsUsingTemplate || Utils.Dialog((IWin32Window) Session.Application, "Clear the selected Loan Program from the loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      Session.LoanDataMgr.ClearLoanProgram();
      this.UpdateContents();
    }

    private void selectClosingInvestor()
    {
      if (!EncompassDocs.IsUsingEncompassDocsSolution(this.inputData))
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "This loan is not configured to use the Encompass Closing Docs service.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (InvestorSelectionDialog investorSelectionDialog = new InvestorSelectionDialog(DocumentOrderType.Closing))
        {
          if (investorSelectionDialog.ShowDialog((IWin32Window) Session.Application) != DialogResult.OK)
            return;
          investorSelectionDialog.GetSelectedInvestor().Apply(this.inputData, DocumentOrderType.Closing);
          this.RefreshContents();
        }
      }
    }

    private void clearPlanCode(DocumentOrderType orderType)
    {
      if (Utils.Dialog((IWin32Window) Session.Application, "Are you sure you want to clear the selected Plan Code?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      Plan.ClearPlanMetadata(this.inputData, orderType);
      if (!EncompassDocs.IsUsingEncompassDocsSolution(this.inputData))
        EncompassDocs.ClearDocProvider(this.inputData);
      this.UpdateContents();
    }

    private void clearAltLender()
    {
      for (int index = 1897; index <= 1945; ++index)
        this.UpdateFieldValue(index.ToString(), "");
      this.UpdateFieldValue("AltLender.ID", "");
      this.UpdateFieldValue("L423", "");
      this.UpdateFieldValue("L425", "");
      this.UpdateFieldValue("L426", "");
      this.UpdateFieldValue("L427", "");
      this.UpdateFieldValue("1882", "");
      this.UpdateFieldValue("1953", "");
      this.UpdateContents();
    }

    private void showPlanCodeDialog()
    {
      string field = this.inputData.GetField("663");
      this.epass.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;PLANCODELIST", false);
      if (this.inputData.GetField("663") == "" && field == "N")
        this.inputData.SetField("663", "N");
      this.RefreshContents();
    }

    private void showPlanCodeConflictDialog()
    {
      using (PlanCodeConflictDialog codeConflictDialog = new PlanCodeConflictDialog(this.inputData, DocumentOrderType.Closing))
      {
        int num = (int) codeConflictDialog.ShowDialog((IWin32Window) Session.Application);
      }
      this.RefreshAllControls(true);
    }

    private void showTrusteeDatabase()
    {
      TrusteeRecord rec = (TrusteeRecord) null;
      if (this.GetFieldValue("L427") != string.Empty)
        rec = new TrusteeRecord(this.GetFieldValue("L427"), this.GetFieldValue("1909"), this.GetFieldValue("1910"), this.GetFieldValue("1911"), this.GetFieldValue("1912"), this.GetFieldValue("3901"), this.GetFieldValue("3552"), Utils.ParseDate((object) this.GetFieldValue("2979")), this.GetFieldValue("1913"), this.GetFieldValue("1914"));
      using (TrusteeDatabaseContainer databaseContainer = new TrusteeDatabaseContainer(rec))
      {
        if (databaseContainer.ShowDialog((IWin32Window) null) != DialogResult.OK || this.loan == null)
          return;
        this.UpdateFieldValue("L427", databaseContainer.SelectedTrustee.ContactName);
        this.UpdateFieldValue("1909", databaseContainer.SelectedTrustee.Address);
        this.UpdateFieldValue("1910", databaseContainer.SelectedTrustee.City);
        this.UpdateFieldValue("1911", databaseContainer.SelectedTrustee.State);
        this.UpdateFieldValue("1912", databaseContainer.SelectedTrustee.ZipCode);
        if (string.IsNullOrEmpty(databaseContainer.SelectedTrustee.County) && !string.IsNullOrEmpty(databaseContainer.SelectedTrustee.ZipCode) && databaseContainer.SelectedTrustee.ZipCode.Length >= 5)
        {
          ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(databaseContainer.SelectedTrustee.ZipCode.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(databaseContainer.SelectedTrustee.ZipCode.Trim().Substring(0, 5)), true);
          if (zipCodeInfo != null)
            this.UpdateFieldValue("3901", Utils.CapsConvert(zipCodeInfo.County, false));
          else
            this.UpdateFieldValue("3901", "");
        }
        else
          this.UpdateFieldValue("3901", databaseContainer.SelectedTrustee.County);
        this.UpdateFieldValue("3552", databaseContainer.SelectedTrustee.Phone);
        if (databaseContainer.SelectedTrustee.TrustDate != DateTime.MinValue)
          this.UpdateFieldValue("2979", databaseContainer.SelectedTrustee.TrustDate.ToString("MM/dd/yyyy"));
        else
          this.UpdateFieldValue("2979", "");
        this.UpdateFieldValue("1913", databaseContainer.SelectedTrustee.OrgState);
        this.UpdateFieldValue("1914", databaseContainer.SelectedTrustee.OrgType);
        this.UpdateContents();
      }
    }

    private void showTransferToDialog()
    {
      this.epass.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSCLOSING2;2;TRANSFERTOLIST", false);
      this.UpdateContents();
    }

    protected void showUpdatePurchaseAdvice(string actionID)
    {
      switch (actionID)
      {
        case "updatecorrespondentbalance":
          bool policySetting1 = Session.SessionObjects.GetPolicySetting("EnablePaymentHistoryAndCalc");
          bool policySetting2 = Session.SessionObjects.GetPolicySetting("ENABLEESCROWDETAILSANDCALC");
          if (policySetting1)
          {
            try
            {
              string field1 = this.inputData.GetField("3569");
              if (!string.IsNullOrEmpty(field1) && field1 != "//")
                this.inputData.SetField("CPA.PaymentHistory.FirstInvestorPaymentDate", Utils.ParseDate((object) field1).AddMonths(1).ToString("MM/dd/yyyy"));
              string field2 = this.inputData.GetField("CPA.PaymentHistory.CalculatedPurchasedPrincipal");
              if (!string.IsNullOrEmpty(field2))
              {
                this.inputData.SetField("3571", field2);
                string field3 = this.inputData.GetField("CPA.PaymentHistory.FirstBorrowerPaymentDueDate");
                if (!string.IsNullOrEmpty(field3) && field3 != "//")
                  this.inputData.SetField("682", field3);
                this.inputData.SetField("3570", this.inputData.GetField("CPA.PaymentHistory.FirstInvestorPaymentDate"));
                if (policySetting2)
                {
                  string field4 = this.inputData.GetField("CPA.ESCROWDISBURSE.EsrowFundedByInvestor");
                  this.inputData.SetField("3568", field4);
                  this.inputData.SetField("3582", field4);
                }
              }
            }
            catch (Exception ex)
            {
              Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Error, ex.ToString());
            }
          }
          this.loan.Calculator.SpecialCalculation(CalculationActionID.UpdateCorrespondentPurchaseAdviceBalance);
          break;
        case "updatecorrespondentfees":
          bool flag = false;
          for (int index = 3587; index <= 3610; ++index)
          {
            if (this.GetFieldValue(index.ToString()) != string.Empty)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            if (Utils.Dialog((IWin32Window) Session.MainForm, "All fees except late fees will be cleared. Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
              this.loan.Calculator.SpecialCalculation(CalculationActionID.UpdateCorrespondentFees);
              break;
            }
            break;
          }
          this.loan.Calculator.SpecialCalculation(CalculationActionID.UpdateCorrespondentFees);
          break;
        default:
          this.loan.Calculator.SpecialCalculation(CalculationActionID.UpdatePurchaseAdviceBalance);
          break;
      }
      this.UpdateContents();
    }

    private void importLiabilities()
    {
      this.epass.ProcessURL("_EPASS_SIGNATURE;LIABILITYIMPORT");
      this.loan.Calculator.SpecialCalculation(CalculationActionID.UpdateBorrowerOnLockRequestForm);
      this.loan.Calculator.FormCalculation("FL0015");
      this.UpdateContents();
      this.editor.RefreshContents();
    }

    private void processRateSpread()
    {
      this.epass.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;RATE_SPREAD");
      this.UpdateContents();
    }

    private void resetFHAConsumerChoiceSetting()
    {
      LoanDefaults loanDefaultData = this.session.LoanManager.GetLoanDefaultData();
      if (loanDefaultData.FHAConsumerChoiceFieldList.Map == null)
        return;
      IDictionaryEnumerator enumerator = loanDefaultData.FHAConsumerChoiceFieldList.Map.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string key = (string) enumerator.Key;
        string val = (string) enumerator.Value;
        if (key != string.Empty && val != string.Empty && val != null)
          this.loan.SetCurrentField(key, val);
      }
    }

    private void performFHAMaxLoanCalc()
    {
      if (this.loan == null)
        return;
      this.UpdateFieldValue("1720", this.loan.Calculator.CalculateFHAMaxLoanAmt(Utils.ParseDouble((object) this.loan.GetSimpleField("136")), false).ToString());
      this.UpdateContents();
    }

    private void performGeocoding()
    {
      this.epass.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;GEOCODING;SINGLE");
      this.UpdateContents();
    }

    private void loadPrequalDetailDialog(int col)
    {
      string fieldID = string.Empty;
      using (PrequalDetailDialog prequalDetailDialog = new PrequalDetailDialog(col, this.loan, true))
      {
        if (prequalDetailDialog.ShowDialog() == DialogResult.OK)
          fieldID = prequalDetailDialog.FieldID;
      }
      if (!(fieldID != string.Empty))
        return;
      switch (fieldID)
      {
        case "912":
          this.LoadQuickLinkForm("Proposed Housing Expenses", "PREQUAL_PRIEXPENSE", 400, 480);
          break;
        case "1389":
          this.LoadQuickLinkForm("Monthly Income", "PREQUAL_INCOME", 480, 580);
          break;
        case "2":
        case "353":
        case "976":
        case "742":
          this.editor.GoToField(fieldID, this.currentForm.Name);
          break;
        default:
          this.editor.GoToField(fieldID, false, true);
          break;
      }
    }

    protected virtual void FormatAlphaNumericField(RuntimeControl ctrl, string fieldID)
    {
      if (!(ctrl is EllieMae.Encompass.Forms.TextBox))
        return;
      EllieMae.Encompass.Forms.TextBox textBox = (EllieMae.Encompass.Forms.TextBox) ctrl;
      string str = string.Empty;
      if (this.isUsingTemplate && this.inputData != null)
        str = this.inputData.GetField(fieldID);
      else if (this.loan != null)
        str = this.GetFieldValue(fieldID);
      if (Utils.IsDecimal((object) str) || Utils.ParseDouble((object) str) > 0.0)
        textBox.Alignment = TextAlignment.Right;
      else
        textBox.Alignment = TextAlignment.Left;
    }

    public void LoadQuickLinkForm(string formTitle, string htmFile, int sizeWidth, int sizeHeight)
    {
      this.LoadQuickLinkForm(formTitle, htmFile, sizeWidth, sizeHeight, FieldSource.CurrentLoan);
    }

    public void LoadQuickLinkForm(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight,
      FieldSource fieldSource)
    {
      this.LoadQuickLinkForm(formTitle, htmFile, sizeWidth, sizeHeight, fieldSource, string.Empty);
    }

    public void LoadQuickLinkForm(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight,
      FieldSource fieldSource,
      string helpTag)
    {
      this.LoadQuickLinkForm(formTitle, htmFile, sizeWidth, sizeHeight, fieldSource, helpTag, false);
    }

    public Hashtable getAllowedForms()
    {
      this.populateAllowedForms();
      return this.allowedForms;
    }

    private void populateAllowedForms()
    {
      if (this.allowedForms != null)
        return;
      this.allowedForms = new Hashtable();
      InputFormInfo[] formList = new InputFormList(this.session.SessionObjects).GetFormList("All");
      for (int index = 0; index < formList.Length; ++index)
      {
        if (!this.allowedForms.ContainsKey((object) formList[index].FormID.ToLower()))
          this.allowedForms.Add((object) formList[index].FormID.ToLower(), (object) formList[index]);
      }
    }

    public void LoadQuickLinkForm(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight,
      FieldSource fieldSource,
      string helpTag,
      bool readOnly)
    {
      this.populateAllowedForms();
      string str = htmFile.ToUpper();
      if (str == "VOLPANEL")
        str = "VOL";
      if (str == "VORPANEL")
        str = "VOR";
      if (str == "VOMPANEL")
        str = "VOM";
      if (str == "VOEPANEL")
        str = "VOE";
      if (str == "VODPANEL")
        str = "VOD";
      if (str == "VOGGPANEL")
        str = "VOGG";
      if (str == "VOOIPANEL")
        str = "VOOI";
      if (str == "VOOLPANEL")
        str = "VOOL";
      if (str == "VOALPANEL")
        str = "VOAL";
      if (str == "VOOAPANEL")
        str = "VOOA";
      if (str == "DISPANEL")
        str = "Disaster";
      if (str == "VOLPANEL/VOMPANEL")
        str = "VOL";
      if (str == "FPMS_INCOMEEXPENSE")
        str = "VOD";
      if (str == "REGZGFEHUD_FORPG3")
        str = "HUD1PG3_2010";
      if (str == "VATOOL_REGION")
        str = "VAManagement";
      if (str == "ATR_BONAFIDE")
        str = "ATRManagement";
      if (str == "FM1084B_PROFITLOSS")
        str = "fm1084";
      if (str == "HMDATRANSMITTAL_2018")
        str = "hmda_denial";
      if (str != "CASHINFO" && str != "REGZGFE_2010_LOCOMP" && str != "REGZGFE_2015_DETAILS" && !this.allowedForms.ContainsKey((object) str.ToLower()) && !str.ToUpper().StartsWith("HUD1ES_") && !str.ToUpper().StartsWith("PREQUAL_") && !str.ToUpper().StartsWith("BORROWERSUMMARY_") && !str.ToUpper().StartsWith("REGZ50CLOSER_") && !str.ToUpper().StartsWith("POPUP_") && !str.ToUpper().StartsWith("LODETAILEDLOCKREQUEST") && !str.ToUpper().StartsWith("REGZGFEHUD_DETAIL") && !str.ToUpper().StartsWith("SAFEHARBOROPTION") && !str.ToUpper().StartsWith("SECTION32_2015_CREDITINSURANCE") && str != "FHA_EXISTING203KDEBT" && str != "D1003_2020ACKNOWLEDGEMENT" && str != "D1003_2020MONTHLYHOUSINGEXPENSES" && !str.ToUpper().StartsWith("PAYMENTEXAMPLE") && str == "BUYDOWNSUMMARY" && !this.session.IsBrokerEdition())
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "You don't have user's rights to access '" + formTitle + "' input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (htmFile == "CASHINFO")
        {
          if (this.aclMgr == null)
            this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
          if (!this.isSuperAdmin && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_CashToClose))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) null, "You don't have user's rights to access '" + formTitle + "' input form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        InputFormInfo formInfo = !htmFile.ToLower().StartsWith("bestcontactdaytime_nboc") ? new InputFormInfo(htmFile, htmFile) : new InputFormInfo("bestcontactdaytime_nboc", htmFile);
        if (this.loan == null)
        {
          QuickEntryPopupDialog entryPopupDialog;
          using (entryPopupDialog = new QuickEntryPopupDialog(this.inputData, formTitle, formInfo, sizeWidth, sizeHeight, FieldSource.CurrentLoan, helpTag, this.session))
          {
            int num3 = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
          }
        }
        else if (fieldSource == FieldSource.LinkedLoan && this.loan.LinkedData != null)
        {
          QuickEntryPopupDialog entryPopupDialog;
          using (entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan.LinkedData, formTitle, formInfo, sizeWidth, sizeHeight, fieldSource, helpTag, this.session))
          {
            int num4 = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
          }
          if (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked)
            this.loan.SyncPiggyBackFiles((string[]) null, true, false, (string) null, (string) null, false);
        }
        else
        {
          QuickEntryPopupDialog entryPopupDialog;
          using (entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, formTitle, formInfo, sizeWidth, sizeHeight, FieldSource.CurrentLoan, helpTag, this.session))
          {
            if (readOnly)
              entryPopupDialog.SetFieldsReadonly();
            int num5 = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
          }
        }
        if (this.loan != null && htmFile.ToUpper() == "PREQUAL_MAXLOANAMOUNT")
          this.loan.Calculator.CalculateMaxLoanAmt();
        if (this.loan != null && (htmFile.ToUpper().StartsWith("PREQUAL_") || htmFile.ToUpper().StartsWith("VOLPANEL")))
          this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
        if (this.loan != null && htmFile.ToUpper().StartsWith("REGZGFE_2015") && this is ATR_QUALIFICATIONInputHandler)
          this.loan.Calculator.FormCalculation("QMAPR");
        this.UpdateContents(true);
      }
    }

    protected void WriteDebug(string text)
    {
      if (!SystemSettings.Debug)
        return;
      Tracing.Log(InputHandlerBase.sw, nameof (InputHandlerBase), TraceLevel.Verbose, text);
    }

    private bool isFieldLocked(string id, FieldSource fieldSource)
    {
      return fieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData != null ? this.loan.LinkedData.IsLocked(id) : this.inputData.IsLocked(id);
    }

    private void addFieldLock(string id, FieldSource fieldSource)
    {
      if (fieldSource == FieldSource.LinkedLoan)
      {
        if (this.loan == null || this.loan.LinkedData == null)
          return;
        this.loan.LinkedData.AddLock(id);
      }
      else
        this.inputData.AddLock(id);
    }

    private void removeFieldLock(string id, FieldSource fieldSource)
    {
      if (fieldSource == FieldSource.LinkedLoan)
      {
        if (this.loan == null || this.loan.LinkedData == null)
          return;
        this.loan.LinkedData.RemoveLock(id);
      }
      else
      {
        this.inputData.RemoveLock(id);
        if (this.loan == null || !this.loan.Use2010RESPA && !this.loan.Use2015RESPA || !(id == "NEWHUD.X770") && !(id == "NEWHUD.X4") && !(id == "454"))
          return;
        this.loan.Calculator.FormCalculation("REGZGFE_2010", id, this.GetFieldValue(id));
      }
    }

    protected void SyncPiggyBackField(string id, FieldSource fieldSource, string val)
    {
      if (this.loan == null || this.PiggyFields == null || !this.PiggyFields.IsSycnField(id) || id == "428" && this.loan.Calculator.IsPiggybackHELOC)
        return;
      if (this.loan.LinkedData == null)
        throw new InvalidOperationException("Linked loan field cannot be set when no linked loan is available");
      if (this.loan.LinkedData.IsFieldReadOnly(id))
        throw new InvalidOperationException("Linked loan field cannot be set when the field " + id + " in linked loan is read-only field.");
      string empty = string.Empty;
      EllieMae.EMLite.DataEngine.BorrowerPair[] borrowerPairs1 = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs1.Length; ++index)
      {
        if (borrowerPairs1[index].Borrower.Id == this.loan.CurrentBorrowerPair.Id)
        {
          EllieMae.EMLite.DataEngine.BorrowerPair[] borrowerPairs2 = this.loan.LinkedData.GetBorrowerPairs();
          if (borrowerPairs2 == null || index >= borrowerPairs2.Length)
            return;
          this.loan.LinkedData.SetBorrowerPair(borrowerPairs2[index]);
          break;
        }
      }
      if (fieldSource == FieldSource.CurrentLoan)
      {
        if (this.loan.LinkedData.IsLocked(id))
        {
          if (!this.inputData.IsLocked(id))
            this.inputData.AddLock(id);
        }
        else if (this.inputData.IsLocked(id))
          this.inputData.RemoveLock(id);
        if (!(this.inputData.GetField(id) != val))
          return;
        this.inputData.SetField(id, val);
      }
      else
      {
        if (this.inputData.IsLocked(id))
        {
          if (!this.loan.LinkedData.IsLocked(id))
            this.loan.LinkedData.AddLock(id);
        }
        else if (this.loan.LinkedData.IsLocked(id))
          this.loan.LinkedData.RemoveLock(id);
        if (!(this.loan.LinkedData.GetField(id) != val))
          return;
        this.loan.LinkedData.SetField(id, val);
      }
    }

    internal void SwitchLienPosition(FieldSource fieldSource, string val)
    {
      if (this.loan == null || this.loan.LinkedData == null)
        return;
      string val1 = val == "SecondLien" ? "FirstLien" : "SecondLien";
      if (fieldSource == FieldSource.LinkedLoan)
      {
        this.updateFieldValue("420", FieldSource.CurrentLoan, val1);
        if (val1 == "SecondLien")
        {
          this.updateFieldValue("427", FieldSource.CurrentLoan, this.GetFieldValue("1109", FieldSource.LinkedLoan));
          this.updateFieldValue("428", FieldSource.LinkedLoan, this.GetFieldValue("1109", FieldSource.CurrentLoan));
        }
        else
        {
          this.updateFieldValue("428", FieldSource.CurrentLoan, this.GetFieldValue("1109", FieldSource.LinkedLoan));
          this.updateFieldValue("427", FieldSource.LinkedLoan, this.GetFieldValue("1109", FieldSource.CurrentLoan));
        }
      }
      else
      {
        this.updateFieldValue("420", FieldSource.LinkedLoan, val1);
        if (val1 == "SecondLien")
        {
          this.updateFieldValue("427", FieldSource.LinkedLoan, this.GetFieldValue("1109", FieldSource.CurrentLoan));
          this.updateFieldValue("428", FieldSource.CurrentLoan, this.GetFieldValue("1109", FieldSource.LinkedLoan));
        }
        else
        {
          this.updateFieldValue("428", FieldSource.LinkedLoan, this.GetFieldValue("1109", FieldSource.CurrentLoan));
          this.updateFieldValue("427", FieldSource.CurrentLoan, this.GetFieldValue("1109", FieldSource.LinkedLoan));
        }
      }
    }

    internal bool FormIsForTemplate
    {
      get
      {
        if (this.loan == null)
          return false;
        return this.loan.IsInFindFieldForm || this.loan.IsTemplate || this.loan.IsULDDExporting;
      }
    }

    public void SetFieldReadOnly()
    {
      this.allFieldsAreReadonly = true;
      this.RefreshContents();
      foreach (RuntimeControl runtimeControl in this.currentForm.FindControlsByType(typeof (EllieMae.Encompass.Forms.Calendar)))
        runtimeControl.Enabled = false;
      foreach (RuntimeControl runtimeControl in this.currentForm.FindControlsByType(typeof (FieldLock)))
        runtimeControl.Enabled = false;
      foreach (RuntimeControl runtimeControl in this.currentForm.FindControlsByType(typeof (PickList)))
        runtimeControl.Enabled = false;
      foreach (RuntimeControl runtimeControl in this.currentForm.FindControlsByType(typeof (Rolodex)))
        runtimeControl.Enabled = false;
    }

    public void SetFieldReadOnly(string controlID)
    {
      if (!(this.currentForm.FindControl(controlID) is FieldControl control))
        return;
      control.Enabled = false;
    }

    public void SetFieldDisabled(string controlID)
    {
      if (!(this.currentForm.FindControl(controlID) is RuntimeControl control))
        return;
      control.Enabled = false;
    }

    public void SetFieldEnabled(string controlID)
    {
      if (!(this.currentForm.FindControl(controlID) is RuntimeControl control))
        return;
      control.Enabled = true;
    }

    public void SetFieldAvailable(string controlID)
    {
      if (!(this.currentForm.FindControl(controlID) is FieldControl control))
        return;
      control.Enabled = true;
    }

    public string GetControlFieldValue(string controlID)
    {
      return this.currentForm.FindControl(controlID) is FieldControl control ? control.Value : "";
    }

    public bool GetControlEnableState(string controlID)
    {
      return this.currentForm != null && this.currentForm.FindControl(controlID) is RuntimeControl control && control.Enabled && control.Visible;
    }

    private void importIncomeToATRQM()
    {
      if (!(this.loan.GetField("1825") == "2020"))
      {
        this.UpdateFieldValue("QM.X137", this.GetFieldValue("101"));
        this.UpdateFieldValue("QM.X138", this.GetFieldValue("102"));
        this.UpdateFieldValue("QM.X139", this.GetFieldValue("103"));
        this.UpdateFieldValue("QM.X140", this.GetFieldValue("104"));
        this.UpdateFieldValue("QM.X141", this.GetFieldValue("105"));
        this.UpdateFieldValue("QM.X142", this.GetFieldValue("107"));
        this.UpdateFieldValue("QM.X143", this.GetFieldValue("108"));
        this.UpdateFieldValue("QM.X145", this.GetFieldValue("110"));
        this.UpdateFieldValue("QM.X146", this.GetFieldValue("111"));
        this.UpdateFieldValue("QM.X147", this.GetFieldValue("112"));
        this.UpdateFieldValue("QM.X148", this.GetFieldValue("113"));
        this.UpdateFieldValue("QM.X149", this.GetFieldValue("114"));
        this.UpdateFieldValue("QM.X150", this.GetFieldValue("116"));
        this.UpdateFieldValue("QM.X151", this.GetFieldValue("117"));
        if (this.loan == null)
          return;
        this.loan.Calculator.FormCalculation("QM.X137", (string) null, (string) null);
      }
      else
      {
        if (this.loan == null)
          return;
        this.loan.Calculator.FormCalculation("CopyATRQMIncome", (string) null, (string) null);
      }
    }

    private void copyFromItemization()
    {
      if (Utils.CheckIf2015RespaTila(this.inputData.GetField("3969")))
      {
        this.UpdateFieldValue("DISCLOSURE.X1175", this.GetFieldValue("NEWHUD2.X342"));
        this.UpdateFieldValue("DISCLOSURE.X1176", this.GetFieldValue("NEWHUD2.X1134"));
        this.UpdateFieldValue("DISCLOSURE.X1177", this.GetFieldValue("NEWHUD2.X1101"));
        this.UpdateFieldValue("DISCLOSURE.X1187", this.GetFieldValue("NEWHUD2.X333"));
        this.UpdateFieldValue("DISCLOSURE.X1188", this.GetFieldValue("NEWHUD2.X1092"));
        this.UpdateFieldValue("DISCLOSURE.X1189", this.GetFieldValue("NEWHUD2.X1125"));
        this.UpdateFieldValue("DISCLOSURE.X1190", this.GetFieldValue("NEWHUD2.X300"));
      }
      else
      {
        this.UpdateFieldValue("DISCLOSURE.X1175", this.GetFieldValue("NEWHUD.X822"));
        this.UpdateFieldValue("DISCLOSURE.X1176", this.GetFieldValue("NEWHUD.X833"));
        this.UpdateFieldValue("DISCLOSURE.X1177", this.GetFieldValue("NEWHUD.X832"));
        this.UpdateFieldValue("DISCLOSURE.X1187", this.GetFieldValue("NEWHUD.X702"));
        this.UpdateFieldValue("DISCLOSURE.X1188", this.GetFieldValue("NEWHUD.X609"));
        this.UpdateFieldValue("DISCLOSURE.X1189", this.GetFieldValue("NEWHUD.X610"));
        this.UpdateFieldValue("DISCLOSURE.X1190", this.GetFieldValue("NEWHUD.X770"));
      }
    }

    private void getindex()
    {
      Cursor.Current = Cursors.WaitCursor;
      MaventService maventService = new MaventService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.MaventServiceUrl);
      string field = this.GetField("1959");
      if (field == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "ARM Index Type cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string empty = string.Empty;
        string armIndexValue;
        try
        {
          armIndexValue = maventService.GetARMIndexValue(field, DateTime.Today.ToString("MM/dd/yyyy"));
        }
        catch (Exception ex)
        {
          Cursor.Current = Cursors.Default;
          int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass can't get ARM Index due to this error:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (string.IsNullOrEmpty(armIndexValue))
        {
          Cursor.Current = Cursors.Default;
          int num3 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The ARM Index value is blank.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml(armIndexValue);
          XmlElement xmlElement = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("Service/Response/Error");
          if (xmlElement != null)
          {
            Cursor.Current = Cursors.Default;
            string attribute1 = xmlElement.GetAttribute("code");
            string attribute2 = xmlElement.GetAttribute("message");
            if (attribute1 == "39" || attribute1 == "46")
            {
              int num4 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The requested index rate could not be found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, attribute2 + "\r\n\r\nError Code: " + attribute1, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
          else
          {
            bool flag = false;
            double num6 = 0.0;
            string str = string.Empty;
            foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("Service/Response/Return"))
            {
              if (selectNode.GetAttribute("name") == "index_rate")
              {
                num6 = Utils.ParseDouble((object) selectNode.GetAttribute("value"));
                flag = true;
              }
              else if (selectNode.GetAttribute("name") == "index_name_code")
              {
                str = selectNode.GetAttribute("value");
                flag = true;
              }
              if (num6 != 0.0)
              {
                if (str != string.Empty)
                  break;
              }
            }
            if (!flag)
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Cannot find index rate in returned file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              string val = num6 != 0.0 ? num6.ToString(this.Use5DecimalForIndexRates ? "N5" : "N3") : "";
              if (this.loan != null)
              {
                this.loan.SetCurrentField("688", val);
                bool performanceEnabled = this.loan.Calculator.PerformanceEnabled;
                this.loan.Calculator.PerformanceEnabled = false;
                this.loan.TriggerCalculation("688", val);
                this.loan.Calculator.PerformanceEnabled = performanceEnabled;
                if (!this.loan.IsTemplate)
                {
                  GetIndexLog rec = new GetIndexLog(str + " (" + field + ")", num6.ToString("N3"), this.session.UserInfo.FullName, this.session.UserInfo.Userid);
                  this.session.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
                }
              }
              else
                this.SetField("688", val);
              this.loan.SetCurrentField("4898", DateTime.Today.ToString("MM/dd/yyyy"));
            }
            if (this.loan != null && !this.loan.IsTemplate)
            {
              this.session.Application.GetService<ILoanEditor>().RefreshContents();
              this.session.Application.GetService<ILoanEditor>().RefreshLogPanel();
            }
            Cursor.Current = Cursors.Default;
          }
        }
      }
    }

    private void getResidualIncome()
    {
      Cursor.Current = Cursors.WaitCursor;
      string field1 = this.inputData.GetField("14");
      if (field1 == string.Empty)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "To get residual income, please enter property state to the field '14'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string field2 = this.inputData.GetField("CASASRN.X145");
        if (field2 == string.Empty || Utils.ParseInt((object) field2) > 100 || Utils.ParseInt((object) field2) < 1)
        {
          Cursor.Current = Cursors.Default;
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "To get residual income, the Family Size field 'CASASRN.X145' must be in the range from 1 to 100.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string loanAmount = this.inputData.GetField("2").Replace(",", "");
          if (loanAmount == string.Empty)
          {
            Cursor.Current = Cursors.Default;
            int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "To get residual income, please enter loan amount to field '1109'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            string field3 = DateTime.Now.ToString("MM/dd/yyyy");
            if (this.inputData.GetField("1887") != string.Empty && this.inputData.GetField("1887") != "//")
              field3 = this.inputData.GetField("1887");
            else if (this.inputData.GetField("2553") != string.Empty && this.inputData.GetField("2553") != "//")
              field3 = this.inputData.GetField("2553");
            else if (this.inputData.GetField("763") != string.Empty && this.inputData.GetField("763") != "//")
              field3 = this.inputData.GetField("763");
            MaventService maventService = new MaventService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.MaventServiceUrl);
            string empty = string.Empty;
            string residualIncomeValue;
            try
            {
              residualIncomeValue = maventService.GetResidualIncomeValue(field1, field2, loanAmount, field3);
            }
            catch (Exception ex)
            {
              Cursor.Current = Cursors.Default;
              int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Encompass can't get residual income due to this error:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            if (string.IsNullOrEmpty(residualIncomeValue))
            {
              Cursor.Current = Cursors.Default;
              int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The residual income value is blank.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              Cursor.Current = Cursors.Default;
              XmlDocument xmlDocument = new XmlDocument();
              xmlDocument.LoadXml(residualIncomeValue);
              bool flag = false;
              string str = string.Empty;
              XmlElement xmlElement1 = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("Service/Response");
              if (xmlElement1 != null)
              {
                if (string.Compare(xmlElement1.GetAttribute("status"), "Success", true) != 0)
                {
                  XmlElement xmlElement2 = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("Service/Response/Message");
                  if (xmlElement2 != null)
                    str = xmlElement2.GetAttribute("data");
                }
                else
                {
                  XmlElement xmlElement3 = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("Service/Response/Data");
                  if (xmlElement3 != null && xmlElement3.HasAttribute("name") && xmlElement3.HasAttribute("value") && string.Compare(xmlElement3.GetAttribute("name").ToString(), "residual_income", true) == 0)
                  {
                    this.inputData.SetField("1340", xmlElement3.GetAttribute("value").ToString());
                    flag = true;
                  }
                }
              }
              if (!flag)
              {
                if (str != string.Empty)
                {
                  int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The requested residual income could not be found due to this error: " + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                  int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The requested residual income could not be found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
              }
              else
              {
                Cursor.Current = Cursors.WaitCursor;
                if (this.loan != null && !this.loan.IsTemplate)
                  this.session.Application.GetService<ILoanEditor>().RefreshContents();
                else
                  this.RefreshContents();
                Cursor.Current = Cursors.Default;
              }
            }
          }
        }
      }
    }

    private void showeFolderDocument()
    {
      string field = this.inputData.GetField("AUS.EFOLDERGUID");
      if (field == string.Empty)
        return;
      DocumentLog doc = (DocumentLog) null;
      DocumentLog[] allDocuments = this.session.LoanDataMgr.LoanData.GetLogList().GetAllDocuments();
      for (int index = 0; index < allDocuments.Length; ++index)
      {
        if (string.Compare(field, allDocuments[index].Guid, true) == 0)
        {
          doc = allDocuments[index];
          break;
        }
      }
      if (doc != null)
      {
        Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, doc);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The eFolder document in this AUS Tracking has been removed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void showLateFeeDetails()
    {
      using (LateFeeDetailsForm lateFeeDetailsForm = new LateFeeDetailsForm(this.inputData, this.session))
      {
        int num = (int) lateFeeDetailsForm.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void updateTPOAndClosingVendorInformation(string origValue, string newValue)
    {
      string str1 = "The origination Channel type has been changed from " + origValue + " to " + newValue + ". ";
      string str2 = string.Empty;
      if (string.Compare(origValue, "Correspondent", true) == 0)
      {
        if (string.Compare(newValue, "Banked - Wholesale", true) == 0 && !this.loan.Calculator.IsClosingVendorInformationEmpty(false, true, false))
        {
          if (Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the Investor information on the Closing Vendor Information form to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.ClearClosingVendorInformation(false, true, false);
        }
        else if (string.Compare(newValue, "Banked - Retail", true) == 0)
        {
          if (!this.loan.Calculator.IsTPOInformationEmpty())
            str2 = "TPO Information Tool";
          if (!this.loan.Calculator.IsClosingVendorInformationEmpty(false, true, false))
            str2 = str2 + (str2 != string.Empty ? " and the " : "") + "Investor information on the Closing Vendor Information form";
          if (!(str2 != string.Empty) || Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the " + str2 + " to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.FormCalculation("CLEARTPOINFORMATION", (string) null, (string) null);
          this.loan.Calculator.ClearClosingVendorInformation(false, true, false);
        }
        else
        {
          if (string.Compare(newValue, "Brokered", true) != 0)
            return;
          if (!this.loan.Calculator.IsTPOInformationEmpty())
            str2 = "TPO Information Tool";
          if (!this.loan.Calculator.IsClosingVendorInformationEmpty(true, true, false))
            str2 = str2 + (str2 != string.Empty ? ", and the " : "") + "Lender and Investor information on the Closing Vendor Information";
          if (!(str2 != string.Empty) || Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the " + str2 + " to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.FormCalculation("CLEARTPOINFORMATION", (string) null, (string) null);
          this.loan.Calculator.ClearClosingVendorInformation(true, true, false);
        }
      }
      else if (string.Compare(origValue, "Banked - Wholesale", true) == 0)
      {
        if (string.Compare(newValue, "Correspondent", true) == 0 && !this.loan.Calculator.IsClosingVendorInformationEmpty(false, false, true))
        {
          if (Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the Broker information on the Closing Vendor Information to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.ClearClosingVendorInformation(false, false, true);
        }
        else if (string.Compare(newValue, "Banked - Retail", true) == 0)
        {
          if (!this.loan.Calculator.IsTPOInformationEmpty())
            str2 = "TPO Information Tool";
          if (!this.loan.Calculator.IsClosingVendorInformationEmpty(false, false, true))
            str2 = str2 + (str2 != string.Empty ? " and the " : "") + "Broker information on the Closing Vendor Information";
          if (!(str2 != string.Empty) || Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the " + str2 + " to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.FormCalculation("CLEARTPOINFORMATION", (string) null, (string) null);
          this.loan.Calculator.ClearClosingVendorInformation(false, false, true);
        }
        else
        {
          if (string.Compare(newValue, "Brokered", true) != 0)
            return;
          if (!this.loan.Calculator.IsTPOInformationEmpty())
            str2 = "TPO Information Tool";
          if (!this.loan.Calculator.IsClosingVendorInformationEmpty(true, false, false))
            str2 = str2 + (str2 != string.Empty ? " and the " : "") + "Lender information on the Closing Vendor Information";
          if (!(str2 != string.Empty) || Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the " + str2 + " to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.FormCalculation("CLEARTPOINFORMATION", (string) null, (string) null);
          this.loan.Calculator.ClearClosingVendorInformation(true, false, false);
        }
      }
      else if (string.Compare(origValue, "Banked - Retail", true) == 0)
      {
        if (string.Compare(newValue, "Brokered", true) != 0 || this.loan.Calculator.IsClosingVendorInformationEmpty(true, false, false) || Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the Lender information on the Closing Vendor Information to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
          return;
        this.loan.Calculator.ClearClosingVendorInformation(true, false, false);
      }
      else
      {
        if (string.Compare(origValue, "Brokered", true) != 0)
          return;
        if (string.Compare(newValue, "Correspondent", true) == 0 && !this.loan.Calculator.IsClosingVendorInformationEmpty(false, false, true))
        {
          if (Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the Broker information on the Closing Vendor Information to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.ClearClosingVendorInformation(false, false, true);
        }
        else
        {
          if (string.Compare(newValue, "Banked - Retail", true) != 0 || this.loan.Calculator.IsClosingVendorInformationEmpty(false, false, true) || Utils.Dialog((IWin32Window) Session.MainForm, str1 + "Do you want the Broker information on the Closing Vendor Information to be cleared?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
            return;
          this.loan.Calculator.ClearClosingVendorInformation(false, false, true);
        }
      }
    }

    private void selectWarehouseBank()
    {
      using (WarehouseSelectionDialog warehouseSelectionDialog = new WarehouseSelectionDialog(this.inputData, this.session))
      {
        int num = (int) warehouseSelectionDialog.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private Dictionary<IDisclosureTracking2015Log, bool> showDisclosedLELogs(
      bool updatedAlready,
      bool cancelClear)
    {
      Dictionary<IDisclosureTracking2015Log, bool> dictionary = new Dictionary<IDisclosureTracking2015Log, bool>();
      using (LEDisclosureSnapshot disclosureSnapshot = new LEDisclosureSnapshot(this.session))
      {
        if (disclosureSnapshot.DialogResult == DialogResult.Cancel)
        {
          this.loan.SetField("3164", "N");
          this.loan.SetField("3972", "//");
          return (Dictionary<IDisclosureTracking2015Log, bool>) null;
        }
        if (disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
        {
          if (this.loan.GetField("3164") == "Y" && !updatedAlready)
          {
            dictionary.Add(disclosureSnapshot.SelectedDT, false);
            return dictionary;
          }
          this.loan.SetField("3164", "Y");
          dictionary.Add(disclosureSnapshot.SelectedDT, true);
          return dictionary;
        }
        if (disclosureSnapshot.DialogResult == DialogResult.Cancel & cancelClear)
        {
          this.loan.SetField("3164", "N");
          this.loan.SetField("3972", "//");
          return (Dictionary<IDisclosureTracking2015Log, bool>) null;
        }
      }
      return (Dictionary<IDisclosureTracking2015Log, bool>) null;
    }

    private Dictionary<IDisclosureTracking2015Log, bool> showDisclosedCDLogs(
      bool updatedAlready,
      bool cancelClear)
    {
      Dictionary<IDisclosureTracking2015Log, bool> dictionary = new Dictionary<IDisclosureTracking2015Log, bool>();
      using (CDDisclosureSnapshot disclosureSnapshot = new CDDisclosureSnapshot(this.session))
      {
        if (disclosureSnapshot.DialogResult == DialogResult.Cancel)
        {
          this.loan.SetField("3164", "N");
          this.loan.SetField("3972", "//");
          return (Dictionary<IDisclosureTracking2015Log, bool>) null;
        }
        if (disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
        {
          if (this.loan.GetField("3164") == "Y" && !updatedAlready)
          {
            dictionary.Add(disclosureSnapshot.SelectedDT, false);
            return dictionary;
          }
          this.loan.SetField("3164", "Y");
          dictionary.Add(disclosureSnapshot.SelectedDT, true);
          return dictionary;
        }
        if (disclosureSnapshot.DialogResult == DialogResult.Cancel & cancelClear)
        {
          this.loan.SetField("3164", "N");
          this.loan.SetField("3972", "//");
          return (Dictionary<IDisclosureTracking2015Log, bool>) null;
        }
      }
      return (Dictionary<IDisclosureTracking2015Log, bool>) null;
    }

    private void showHelocCalculatorDialog(FieldSource fs)
    {
      if (new HelocCalcDialog(this.loan, this.mainScreen, fs).ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
        return;
      this.UpdateContents();
    }

    private void showPaymentsAndPayoffsDialog()
    {
      if (new PayoffsAndPaymentsDialog(this.loan, this.mainScreen).ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
        return;
      if (this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.FormCalculation("CD3.X139", (string) null, (string) null);
      this.UpdateContents();
    }

    private void showDisclosedLELogsCannotDecrease()
    {
      using (LEDisclosureSnapshot disclosureSnapshot = new LEDisclosureSnapshot(this.session, true))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X360"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedLELogsCannotIncrease()
    {
      using (LEDisclosureSnapshot disclosureSnapshot = new LEDisclosureSnapshot(this.session, true))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X362"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedLELogsCannotIncrease10()
    {
      using (LEDisclosureSnapshot disclosureSnapshot = new LEDisclosureSnapshot(this.session, true))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X364"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedLELogsCanChange()
    {
      using (LEDisclosureSnapshot disclosureSnapshot = new LEDisclosureSnapshot(this.session, true))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X357"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedCDLogsCannotDecrease()
    {
      using (CDDisclosureSnapshot disclosureSnapshot = new CDDisclosureSnapshot(this.session))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X361"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedCDLogsCannotIncrease()
    {
      using (CDDisclosureSnapshot disclosureSnapshot = new CDDisclosureSnapshot(this.session))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X363"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedCDLogsCannotIncrease10()
    {
      using (CDDisclosureSnapshot disclosureSnapshot = new CDDisclosureSnapshot(this.session))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X365"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void showDisclosedCDLogsCanChange()
    {
      using (CDDisclosureSnapshot disclosureSnapshot = new CDDisclosureSnapshot(this.session))
      {
        disclosureSnapshot.DefaultSelection(this.session.LoanData.GetField("FV.X359"));
        int num = (int) disclosureSnapshot.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    private void importFundingDetails()
    {
      using (System.Windows.Forms.Form importFundingForm = ImportFundingFactory.GetImportFundingForm(this.session.LoanDataMgr))
      {
        if (importFundingForm == null)
          return;
        importFundingForm.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm);
        if (!ImportFundingFactory.Success)
          return;
        Session.Application.GetService<ILoanEditor>().RefreshLoanContents();
      }
    }

    public void ClearCurrentFieldObject() => this.currentField = (FieldControl) null;

    public virtual void Dispose()
    {
    }

    internal void RemoveLinkedLoan(bool askConfirmed)
    {
      PIGGYBACKInputHandler.RemoveLinkedLoan(this.session, this.inputData, this, askConfirmed);
    }

    private void applyOnDemandBusinessRule()
    {
      if (this.loan == null)
        return;
      if (this.loan.BusinessRuleTrigger == BusinessRuleOnDemandEnum.None)
        return;
      try
      {
        this.session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }

    protected void ShowHmdaLock(string fieldId, string lockId, string noLockFieldId)
    {
      if (this.currentForm == null)
        return;
      FieldLock control1 = (FieldLock) this.currentForm.FindControl(lockId);
      if (control1 == null)
        return;
      FieldControl controlToLock = (FieldControl) control1.ControlToLock;
      if (controlToLock == null)
        return;
      FieldControl control2 = (FieldControl) this.currentForm.FindControl(noLockFieldId);
      if (control2 == null || this.loan == null)
        return;
      bool flag = this.loan.Calculator != null && this.loan.Calculator.IsHmdaFieldCalculated(fieldId) || this.loan.IsLocked(fieldId);
      control1.Visible = flag;
      controlToLock.Visible = flag;
      control2.Visible = !flag;
      if (!flag)
        control2.Enabled = true;
      control2.Top = controlToLock.Top;
      control2.Left = controlToLock.Left;
    }

    private bool attachLiensToVOM(string index)
    {
      string fieldValue1 = this.GetFieldValue("FM" + index + "43");
      string fieldValue2 = this.GetFieldValue("FM" + index + "46");
      bool flag = false;
      NewMortgageDialog newMortgageDialog = new NewMortgageDialog(this.loan, fieldValue1);
      if (newMortgageDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return false;
      string[] strArray = newMortgageDialog.SelectedVOL.Split('|');
      if (newMortgageDialog.SelectedVOL.Length > 0)
      {
        for (int index1 = 0; index1 < strArray.Length; ++index1)
        {
          string fieldValue3 = this.GetFieldValue("FL" + double.Parse(strArray[index1]).ToString("00") + "15");
          if (fieldValue2 != fieldValue3 && fieldValue2 != "")
            flag = true;
        }
      }
      this.loan.AttachMortgage(index, newMortgageDialog.SelectedVOL);
      if (flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please review - selected liability is not associated with the current owner of the property.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return true;
    }

    public void enableOrDisableDropdownPanels(EllieMae.Encompass.Forms.Panel panelToDisplay, EllieMae.Encompass.Forms.Panel panelToHide)
    {
      panelToDisplay.Enabled = true;
      panelToDisplay.Visible = true;
      foreach (RuntimeControl control in panelToDisplay.Controls)
        control.Visible = true;
      panelToHide.Visible = false;
      panelToHide.Enabled = false;
      foreach (RuntimeControl control in panelToHide.Controls)
        control.Visible = false;
    }

    private void viewAreaMedianIncomDialog()
    {
      using (AreaMedianIncomeLookup medianIncomeLookup = new AreaMedianIncomeLookup(this.loan))
      {
        int num = (int) medianIncomeLookup.ShowDialog();
      }
    }

    private void viewMedianFamilyIncomeDialog()
    {
      using (MedianFamilyIncomeLookup familyIncomeLookup = new MedianFamilyIncomeLookup(this.loan))
      {
        int num = (int) familyIncomeLookup.ShowDialog();
      }
    }

    private delegate void setControlBackColorDelegate(FieldControl c, System.Drawing.Color backColor);

    private enum BorrowerRolodexType
    {
      Borrower,
      CoBorrower,
      None,
    }

    private class ConInfo
    {
      internal int Cookie;
      internal System.Runtime.InteropServices.ComTypes.IConnectionPoint ConPt;

      internal ConInfo(System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt, int cookie)
      {
        this.ConPt = conPt;
        this.Cookie = cookie;
      }
    }

    private class EventInfo
    {
      private EllieMae.Encompass.Forms.Control ctrl;
      private string eventName;

      public EventInfo(EllieMae.Encompass.Forms.Control ctrl, string eventName)
      {
        this.ctrl = ctrl;
        this.eventName = eventName;
      }

      public override int GetHashCode() => this.ctrl.GetHashCode() ^ this.eventName.GetHashCode();

      public override bool Equals(object obj)
      {
        InputHandlerBase.EventInfo eventInfo = obj as InputHandlerBase.EventInfo;
        return this.ctrl.Equals((object) eventInfo.ctrl) && this.eventName == eventInfo.eventName;
      }
    }
  }
}
