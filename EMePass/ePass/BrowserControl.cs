// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BrowserControl
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using AxGNETMXLib;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass.eFolder;
using EllieMae.EMLite.ePass.PartnerAPI;
using EllieMae.EMLite.ePass.Properties;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class BrowserControl : UserControl, IBrowser
  {
    private const string EPASS_MESSAGE_URL = "https://www.epassbusinesscenter.com/epassai/getmessage.asp";
    private const string className = "BrowserControl";
    private static readonly string sw = Tracing.SwEpass;
    private BrowserControl.PopulateFormDelegate populateFormDelegate;
    private bool m_bShowToolbar = true;
    private bool m_bShowHomeButton = true;
    private bool m_bShowBackButton = true;
    private bool m_bShowForwardButton = true;
    private bool m_bShowRefreshButton = true;
    private bool m_bShowStopButton = true;
    private bool m_bShowPrintButton = true;
    private string m_sHelpKey = "";
    private bool loggedIn;
    private bool setProperties;
    private bool browserVisible;
    private object iPartner;
    private IContainer components;
    private GradientPanel pnlToolbar;
    private IconButton btnHome;
    private IconButton btnForward;
    private IconButton btnBack;
    private IconButton btnPrint;
    private IconButton btnStop;
    private IconButton btnRefresh;
    private FlowLayoutPanel pnlToolbarLayout;
    private BorderPanel pnlBorder;
    private Panel pnlWorkspace;
    private AxGNetMX gnetBrowser;
    private Panel pnlBrowser;
    private ToolTip toolTip;
    private ContextMenuStrip mnuBrowser;
    private ToolStripMenuItem mnuItemHome;
    private ToolStripMenuItem mnuItemBack;
    private ToolStripMenuItem mnuItemForward;
    private ToolStripMenuItem mnuItemRefresh;
    private ToolStripMenuItem mnuItemStop;
    private ToolStripMenuItem mnuItemPrint;

    public BrowserControl()
    {
      this.InitializeComponent();
      this.gnetBrowser.ContainerMode = 1;
      this.gnetBrowser.DisplayMode = 1;
      this.gnetBrowser.HomeURL = string.Empty;
      this.gnetBrowser.Password = "459DM629Y6";
      this.populateFormDelegate = new BrowserControl.PopulateFormDelegate(this.PopulateForm);
      this.mnuItemHome.ShortcutKeys = Keys.Home | Keys.Alt;
    }

    public object Partner => this.iPartner;

    private void gnetBrowser_ePASSVBEvent(object sender, _DGNetMXEvents_ePASSVBEventEvent e)
    {
      switch (e.gNetEID)
      {
        case 0:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_IMPORT_WEBAPP: " + e.pData);
          this.importWebApp(e.pData);
          break;
        case 1:
          this.processSetStatus(e.pData);
          break;
        case 2:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_PROCESS_MAP: " + e.pData);
          this.iPartner = this.processMap(e.pData);
          break;
        case 3:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_LOANINFO");
          e.pData = this.getLoanInfo();
          break;
        case 5:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_PATHINFO");
          e.pData = this.getPathInfo();
          break;
        case 6:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_GET_VERSION");
          e.pData = EpassLogin.EpassVersion(Session.DefaultInstance);
          break;
        case 7:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_IMPORT_FILE: " + e.pData);
          this.importTransfer(e.pData);
          break;
        case 8:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_GET_EPASS_ACCESS_RIGHT");
          e.pData = this.getAutoConnect();
          break;
        case 9:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_GETCLIENTNAME");
          e.pData = this.getClientInfo();
          break;
        case 11:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_NAVIGATE: " + e.pData);
          if (this.checkNavigate(e.pData))
            break;
          e.pData = "N";
          break;
        case 12:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_AUTO_POPULATE: " + e.pData);
          e.pData = this.getAutoPopulate(e.pData);
          break;
        case 14:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_BEFORE_DOWNLOAD");
          e.pData = this.processBeforeDownload();
          break;
        case 22:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_EXTRALOANINFO");
          e.pData = this.getExtraLoanInfo();
          break;
        case 26:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_GET_ENVIRONMENT");
          e.pData = this.getEnvironment();
          break;
        case 27:
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "GNET_EVENTID_GET_CMDSTATECHANGE: " + e.pData);
          this.processCmdStateChange(e.pData);
          break;
      }
    }

    public void SetProperties()
    {
      if (this.setProperties)
        return;
      this.setProperties = true;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), nameof (SetProperties));
      this.gnetBrowser.SetProperties(Session.CompanyInfo.Name, Session.CompanyInfo.ClientID, string.Empty, SystemSettings.EpassDataDir, Session.UserID);
    }

    private bool loginUser(bool showDialogs)
    {
      if (this.loggedIn)
        return true;
      if (!this.setProperties)
      {
        this.setProperties = true;
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "SetProperties");
        this.gnetBrowser.SetProperties(Session.CompanyInfo.Name, Session.CompanyInfo.ClientID, string.Empty, SystemSettings.EpassDataDir, Session.UserID);
      }
      LoginUserEventArgs e = new LoginUserEventArgs(showDialogs);
      if (this.LoginUser != null)
      {
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "LoginUser");
        this.LoginUser((object) this, e);
      }
      this.loggedIn = e.Response;
      return this.loggedIn;
    }

    private bool checkNavigate(string url)
    {
      if (this.InvokeRequired)
        return (bool) this.Invoke((Delegate) new BrowserControl.CheckNavigateDelegate(this.checkNavigate), (object) url);
      WebBrowserNavigatingEventArgs e = new WebBrowserNavigatingEventArgs(new Uri(url), (string) null);
      if (this.BeforeNavigate != null)
      {
        this.BeforeNavigate((object) this, e);
        if (e.Cancel)
          return false;
      }
      if (!this.browserVisible)
      {
        this.browserVisible = true;
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Show GNETMX");
        this.gnetBrowser.Visible = true;
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Dock GNETMX");
        this.gnetBrowser.Dock = DockStyle.Fill;
      }
      if (url.Equals("https://www.epassbusinesscenter.com/"))
      {
        this.loggedIn = false;
        if (!this.loginUser(true))
          e.Cancel = true;
        this.gnetBrowser.Invalidate();
        this.gnetBrowser.Update();
      }
      return !e.Cancel;
    }

    private string getAutoConnect() => this.loggedIn ? "9" : "1";

    private string getAutoPopulate(string url)
    {
      Tracing.Log(BrowserControl.sw, nameof (BrowserControl), TraceLevel.Verbose, "Completed navigation to URL: " + url);
      WebBrowserNavigatedEventArgs e = new WebBrowserNavigatedEventArgs(new Uri(url));
      if (this.AfterNavigate != null)
        this.AfterNavigate((object) this, e);
      return "Y";
    }

    private string getClientInfo() => Session.CompanyInfo.ClientID + "\n" + Session.UserID + "\n";

    private string getEnvironment()
    {
      string environment = "Unknown";
      if (AssemblyResolver.IsSmartClient)
        environment = "SmartClient";
      else if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Hosted)
        environment = "Hosted";
      else if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Default)
        environment = "Default";
      return environment;
    }

    private string getExtraLoanInfo()
    {
      LoanData loanData = Session.LoanData;
      if (loanData == null)
        return string.Empty;
      string simpleField1 = loanData.GetSimpleField("2");
      string simpleField2 = loanData.GetSimpleField("3");
      string str1 = loanData.GetSimpleField("1172");
      string str2 = loanData.GetSimpleField("19");
      string str3 = loanData.GetSimpleField("608");
      string simpleField3 = loanData.GetSimpleField("356");
      string str4 = loanData.GetSimpleField("420");
      if (str1 == "FarmersHomeAdministration")
        str1 = "FmHA";
      switch (str2)
      {
        case "ConstructionOnly":
          str2 = "Construction";
          break;
        case "ConstructionToPermanent":
          str2 = "Construction-Permanent";
          break;
        case "Cash-Out Refinance":
          str2 = "Refinance";
          break;
        case "NoCash-Out Refinance":
          str2 = "Refinance";
          break;
      }
      switch (str3)
      {
        case "GraduatedPaymentMortgage":
          str3 = "GPM";
          break;
        case "AdjustableRate":
          str3 = "ARM";
          break;
        case "OtherAmortizationType":
          str3 = "Other";
          break;
      }
      switch (str4)
      {
        case "FirstLien":
          str4 = "1st";
          break;
        case "SecondLien":
          str4 = "2nd";
          break;
      }
      return simpleField1 + "\n" + simpleField2 + "\n" + str1 + "\n" + str2 + "\n" + str3 + "\n" + simpleField3 + "\n" + str4 + "\n";
    }

    private string getLoanInfo()
    {
      LoanData loanData = Session.LoanData;
      if (loanData == null)
        return "\n\n\n\n\n";
      string str = string.Empty;
      if (!Session.LoanDataMgr.IsNew())
        str = Session.LoanDataMgr.LoanFolder + "\\" + Session.LoanDataMgr.LoanName + "\\loan.em";
      return str + "\n" + loanData.GUID + "\n" + loanData.GetSimpleField("364") + "\n" + loanData.GetSimpleField("36") + " " + loanData.GetSimpleField("37") + "\n" + loanData.GetSimpleField("11") + "\n";
    }

    private string getPathInfo()
    {
      return SystemSettings.EpassDir + "\n" + SystemSettings.EpassDataDir + "\n" + SystemSettings.UpdatesDir;
    }

    private void importTransfer(string filepath)
    {
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanMgmt_Transfer))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have access right to transfer loan file from loan mailbox.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (ImportTransfer importTransfer = new ImportTransfer(filepath, Session.DefaultInstance))
        {
          if (importTransfer.HaveValidFolders)
          {
            int num2 = (int) importTransfer.ShowDialog();
          }
          else
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "You do not have access right to loan folders for import.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
    }

    private void importWebApp(string webData)
    {
      using (ImportWeb importWeb = new ImportWeb(new StringReader(webData).ReadLine()))
      {
        int num = (int) importWeb.ShowDialog();
      }
    }

    private string processBeforeDownload()
    {
      if (Session.LoanDataMgr != null)
      {
        if (Session.LoanDataMgr.IsNew() && !Session.LoanDataMgr.Save() || !Session.LoanDataMgr.LockLoanWithExclusiveA())
          return "N";
        if (Session.LoanData.Calculator.CalcOnDemand())
          Session.Application.GetService<ILoanEditor>().RefreshContents();
      }
      return "Y";
    }

    private void processCmdStateChange(string stateData)
    {
      switch (stateData)
      {
        case "BACKBUTTON=Disable":
          this.btnBack.Enabled = false;
          this.mnuItemBack.Enabled = false;
          break;
        case "BACKBUTTON=Enable":
          this.btnBack.Enabled = true;
          this.mnuItemBack.Enabled = true;
          break;
        case "FORWARDBUTTON=Disable":
          this.btnForward.Enabled = false;
          this.mnuItemForward.Enabled = false;
          break;
        case "FORWARDBUTTON=Enable":
          this.btnForward.Enabled = true;
          this.mnuItemForward.Enabled = true;
          break;
      }
    }

    private string changeEpassDllFilePath(string displayName, string defaultAssemblyFile)
    {
      try
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\ePassDLL"))
        {
          if (registryKey != null)
          {
            string[] strArray = (displayName ?? "").Split(new char[1]
            {
              ','
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray != null)
            {
              if (strArray.Length != 0)
              {
                string str = (string) registryKey.GetValue(strArray[0].Trim());
                if (!string.IsNullOrWhiteSpace(str))
                  return str;
              }
            }
          }
        }
      }
      catch
      {
      }
      return defaultAssemblyFile;
    }

    private object processMap(string mapData)
    {
      try
      {
        StringReader stringReader = new StringReader(mapData);
        stringReader.ReadLine();
        string str1 = stringReader.ReadLine();
        string str2 = stringReader.ReadLine();
        string str3 = stringReader.ReadLine();
        string str4 = (string) null;
        bool flag = true;
        string[] strArray = (str3 ?? "").Split(new char[1]
        {
          ';'
        }, StringSplitOptions.None);
        if (strArray != null && strArray.Length >= 3)
          str4 = strArray[2];
        if (str4 == "GetIPartner" || str4 == "GetIPartner2")
          flag = false;
        string fullName = AssemblyName.GetAssemblyName(str1).FullName;
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Display Name: " + fullName);
        Assembly epassAssembly = (Assembly) null;
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly.FullName == fullName)
            epassAssembly = assembly;
        }
        if (epassAssembly == (Assembly) null)
        {
          string path = this.changeEpassDllFilePath(fullName, str1);
          byte[] numArray = (byte[]) null;
          using (FileStream fileStream = System.IO.File.OpenRead(path))
          {
            numArray = new byte[fileStream.Length];
            fileStream.Read(numArray, 0, numArray.Length);
          }
          Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Loading Assembly: " + Debugger.IsAttached.ToString());
          epassAssembly = !Debugger.IsAttached || PapiSettings.Debug ? Assembly.Load(numArray) : Assembly.LoadFile(path);
        }
        string interfaceTypeName = this.getPartnerInterfaceTypeName(epassAssembly, str4 == "GetIPartner2" ? typeof (IPartner2) : typeof (IPartner));
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Creating Instance: " + interfaceTypeName);
        object instance = epassAssembly.CreateInstance(interfaceTypeName);
        if (str4 == "GetIPartner2")
        {
          IPartner2 partner2 = (IPartner2) instance;
          partner2.Bam = (IBam) new Bam((IBrowser) this);
          partner2.OriginalURL = str3;
          partner2.TransactionID = str2;
        }
        else
        {
          IPartner partner = (IPartner) instance;
          partner.Bam = (IBam) new Bam((IBrowser) this);
          partner.OriginalURL = str3;
          partner.TransactionID = str2;
          if (flag)
          {
            string filepath = partner.Main();
            if (!string.IsNullOrWhiteSpace(filepath))
              this.PopulateForm(filepath);
          }
        }
        return instance;
      }
      catch (ReflectionTypeLoadException ex)
      {
        string msg = ex.ToString();
        foreach (Exception loaderException in ex.LoaderExceptions)
          msg = msg + "\r\n\r\n" + loaderException.ToString();
        Tracing.Log(BrowserControl.sw, TraceLevel.Error, nameof (BrowserControl), msg);
        int num = (int) Utils.Dialog((IWin32Window) null, "The following error occurred when trying to load the assembly:\n\n" + msg + "\n\nPlease contact technical support concerning this error.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserControl.sw, TraceLevel.Error, nameof (BrowserControl), ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) null, "The following error occurred when trying to invoke the assembly:\n\n" + ex.ToString() + "\n\nPlease contact technical support concerning this error.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return (object) null;
    }

    private string getPartnerInterfaceTypeName(Assembly epassAssembly, System.Type type)
    {
      try
      {
        object[] customAttributes = epassAssembly.GetCustomAttributes(typeof (PartnerInterfaceAttribute), false);
        if (customAttributes.Length != 0)
          return ((PartnerInterfaceAttribute) customAttributes[0]).TypeName;
      }
      catch
      {
      }
      string fullName = type.FullName;
      foreach (System.Type type1 in epassAssembly.GetTypes())
      {
        if (type1.GetInterface(fullName) != (System.Type) null)
          return type1.FullName;
      }
      return (string) null;
    }

    private void processSetStatus(string statusData)
    {
      StringReader stringReader = new StringReader(statusData);
      string str = stringReader.ReadLine();
      if (!(str == "2") && !(str == "3"))
        return;
      string text = stringReader.ReadLine();
      if (text == "(null)")
        text = string.Empty;
      Session.Application.GetService<IStatusDisplay>()?.DisplayHelpText(text);
    }

    [Description("Draw border")]
    [Category("Appearance")]
    [DefaultValue(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right)]
    [Browsable(true)]
    public AnchorStyles Borders
    {
      get => this.pnlBorder.Borders;
      set => this.pnlBorder.Borders = value;
    }

    [Description("Returns the current url")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Browsable(false)]
    public string CurrentUrl => this.gnetBrowser.CurrentURL;

    [Description("Url that is considered the home")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Browsable(true)]
    public string HomeUrl
    {
      get => this.gnetBrowser.HomeURL;
      set => this.gnetBrowser.HomeURL = value;
    }

    [Description("Page that is shown when the login fails")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Browsable(true)]
    public string OfflinePage
    {
      get => this.gnetBrowser.StaticHomePage;
      set => this.gnetBrowser.StaticHomePage = value;
    }

    [Description("Whether or not to show toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowToolbar
    {
      get => this.m_bShowToolbar;
      set
      {
        this.m_bShowToolbar = value;
        this.pnlToolbar.Visible = value;
      }
    }

    [Description("Whether or not to show home button on toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowHomeButton
    {
      get => this.m_bShowHomeButton;
      set
      {
        this.m_bShowHomeButton = value;
        this.btnHome.Visible = value;
        this.mnuItemHome.Visible = value;
      }
    }

    [Description("Whether or not to show back button on toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowBackButton
    {
      get => this.m_bShowBackButton;
      set
      {
        this.m_bShowBackButton = value;
        this.btnBack.Visible = value;
        this.mnuItemBack.Visible = value;
      }
    }

    [Description("Whether or not to show forward button on toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowForwardButton
    {
      get => this.m_bShowForwardButton;
      set
      {
        this.m_bShowForwardButton = value;
        this.btnForward.Visible = value;
        this.mnuItemForward.Visible = value;
      }
    }

    [Description("Whether or not to show refresh button on toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowRefreshButton
    {
      get => this.m_bShowRefreshButton;
      set
      {
        this.m_bShowRefreshButton = value;
        this.btnRefresh.Visible = value;
        this.mnuItemRefresh.Visible = value;
      }
    }

    [Description("Whether or not to show stop button on toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowStopButton
    {
      get => this.m_bShowStopButton;
      set
      {
        this.m_bShowStopButton = value;
        this.btnStop.Visible = value;
        this.mnuItemStop.Visible = value;
      }
    }

    [Description("Whether or not to show print button on toolbar.")]
    [Category("Appearance")]
    [DefaultValue(true)]
    [Browsable(true)]
    public bool ShowPrintButton
    {
      get => this.m_bShowPrintButton;
      set
      {
        this.m_bShowPrintButton = value;
        this.btnPrint.Visible = value;
        this.mnuItemPrint.Visible = value;
      }
    }

    [Description("The topic HelpKey for the F1 help key.")]
    [Category("Behavior")]
    [DefaultValue("")]
    [Browsable(true)]
    public string HelpKey
    {
      get => this.m_sHelpKey;
      set => this.m_sHelpKey = value;
    }

    [Description("Returns the menu for the browser control.")]
    [Category("Appearance")]
    [DefaultValue("")]
    [Browsable(false)]
    public ToolStripDropDown Menu => (ToolStripDropDown) this.mnuBrowser;

    public void GoBack()
    {
      if (this.fireEvent(this.BeforeGoBack).Cancel)
        return;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), nameof (GoBack));
      if (!this.doCommand(21, string.Empty))
        return;
      this.fireEvent(this.AfterGoBack);
    }

    public void GoForward()
    {
      if (this.fireEvent(this.BeforeGoForward).Cancel)
        return;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), nameof (GoForward));
      if (!this.doCommand(22, string.Empty))
        return;
      this.fireEvent(this.AfterGoForward);
    }

    public void GoHome()
    {
      if (this.HomeUrl == string.Empty || this.fireEvent(this.BeforeGoHome).Cancel)
        return;
      this.loginUser(this.gnetBrowser.Visible);
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Check Navigate: " + this.HomeUrl);
      if (!this.checkNavigate(this.HomeUrl))
        return;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), nameof (GoHome));
      if (!this.doCommand(25, string.Empty))
        return;
      this.fireEvent(this.AfterGoHome);
    }

    public void Navigate(string url)
    {
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Navigate: " + url);
      if (url == string.Empty)
      {
        this.GoHome();
      }
      else
      {
        if (!this.loginUser(true))
          return;
        this.gnetBrowser.CurrentURL = url;
        this.gnetBrowser.CtlRefresh();
      }
    }

    private void ShowLoadingMessage(string url)
    {
      string str1 = url ?? string.Empty;
      if (str1.StartsWith("_EPASS_SIGNATURE;EPASSAI;2", StringComparison.OrdinalIgnoreCase) || str1.ToUpper() == "_EPASS_SIGNATURE;PREFERREDAPPRAISER;2" || str1.ToUpper() == "_EPASS_SIGNATURE;PREFERREDTITLE;2" || str1.ToUpper().Contains("SSFBOOTSTRAP"))
        return;
      string str2 = "RequestURL=" + HttpUtility.UrlEncode(str1);
      XmlDocument xmlDocument = new XmlDocument();
      int num1 = 2;
      int num2 = 1;
      bool flag = false;
      string requestUriString = "https://www.epassbusinesscenter.com/epassai/getmessage.asp";
      for (; !flag && num2 <= num1; ++num2)
      {
        flag = true;
        try
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
          httpWebRequest.KeepAlive = false;
          httpWebRequest.Method = "POST";
          httpWebRequest.ContentType = "application/x-www-form-urlencoded";
          httpWebRequest.ContentLength = (long) str2.Length;
          httpWebRequest.Timeout = 3000;
          using (Stream requestStream = httpWebRequest.GetRequestStream())
          {
            using (StreamWriter streamWriter = new StreamWriter(requestStream))
              streamWriter.Write(str2);
          }
          using (WebResponse response = httpWebRequest.GetResponse())
          {
            using (Stream responseStream = response.GetResponseStream())
            {
              using (StreamReader streamReader = new StreamReader(responseStream))
                xmlDocument.LoadXml(streamReader.ReadToEnd());
            }
          }
        }
        catch (Exception ex)
        {
          flag = false;
          if (num2 == num1)
          {
            string message = ex.Message;
            Tracing.Log(BrowserControl.sw, TraceLevel.Warning, nameof (BrowserControl), "Get EPASS Message: " + message);
            return;
          }
        }
      }
      XmlElement xmlElement = (XmlElement) xmlDocument.DocumentElement?.SelectSingleNode("/MESSAGE");
      if (xmlElement == null)
        return;
      string innerText = xmlElement.InnerText;
      if (string.IsNullOrEmpty(innerText))
        return;
      int num3 = (int) MessageBox.Show(innerText, "EMN Services", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public void PrintPage()
    {
      if (this.fireEvent(this.BeforePrintPage).Cancel)
        return;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), nameof (PrintPage));
      if (!this.doCommand(26, string.Empty))
        return;
      this.fireEvent(this.AfterPrintPage);
    }

    public bool ProcessURL(string url)
    {
      if (PapiSettings.Debug)
        url = url.Replace(";EPASSAI;", ";PAPISERVICES;");
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "ProcessURL: " + url);
      if (!this.loginUser(true))
        return false;
      this.ShowLoadingMessage(url);
      return this.gnetBrowser.ProcessURL(url);
    }

    public void RefreshPage()
    {
      if (this.fireEvent(this.BeforeRefreshPage).Cancel)
        return;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), nameof (RefreshPage));
      if (!this.doCommand(24, string.Empty))
        return;
      this.fireEvent(this.AfterRefreshPage);
    }

    public bool SendMessage(string msgFile)
    {
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "SendMessage: " + msgFile);
      return this.loginUser(true) && this.doCommand(6, msgFile);
    }

    public void StopNavigation()
    {
      if (this.fireEvent(this.BeforeStopNavigation).Cancel)
        return;
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "Stop");
      if (!this.doCommand(23, string.Empty))
        return;
      this.fireEvent(this.AfterStopNavigation);
    }

    public bool PopulateForm(string filepath)
    {
      Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "PopulateForm: " + filepath);
      switch (filepath)
      {
        case "LeadCenter":
          Session.Application.GetService<IBorrowerContacts>().ShowLeadCenter();
          break;
        case "Borrowers":
          Session.Application.GetService<IBorrowerContacts>().ShowLeads();
          break;
        default:
          if (!this.doCommand(7, filepath))
            return false;
          break;
      }
      return true;
    }

    public bool PopulateFormFromThread(string filepath)
    {
      try
      {
        Tracing.Log(BrowserControl.sw, TraceLevel.Verbose, nameof (BrowserControl), "PopulateFormFromThread: " + filepath);
        return (bool) this.Invoke((Delegate) this.populateFormDelegate, (object) filepath);
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserControl.sw, TraceLevel.Error, nameof (BrowserControl), ex.ToString());
        return false;
      }
    }

    private bool doCommand(int cmdID, string pStrKey)
    {
      try
      {
        return this.gnetBrowser.DoCommand(cmdID, pStrKey);
      }
      catch (Exception ex)
      {
        Tracing.Log(BrowserControl.sw, TraceLevel.Error, nameof (BrowserControl), ex.ToString());
        return false;
      }
    }

    public event EventHandler AfterGoBack;

    public event EventHandler AfterGoForward;

    public event EventHandler AfterGoHome;

    public event WebBrowserNavigatedEventHandler AfterNavigate;

    public event EventHandler AfterPrintPage;

    public event EventHandler AfterRefreshPage;

    public event EventHandler AfterStopNavigation;

    public event CancelEventHandler BeforeGoBack;

    public event CancelEventHandler BeforeGoForward;

    public event CancelEventHandler BeforeGoHome;

    public event WebBrowserNavigatingEventHandler BeforeNavigate;

    public event CancelEventHandler BeforePrintPage;

    public event CancelEventHandler BeforeRefreshPage;

    public event CancelEventHandler BeforeStopNavigation;

    public event LoginUserEventHandler LoginUser;

    private CancelEventArgs fireEvent(CancelEventHandler handler)
    {
      CancelEventArgs e = new CancelEventArgs();
      if (handler != null)
        handler((object) this, e);
      return e;
    }

    private void fireEvent(EventHandler handler)
    {
      if (handler == null)
        return;
      handler((object) this, EventArgs.Empty);
    }

    private void btnHome_Click(object sender, EventArgs e) => this.GoHome();

    private void btnBack_Click(object sender, EventArgs e) => this.GoBack();

    private void btnForward_Click(object sender, EventArgs e) => this.GoForward();

    private void btnRefresh_Click(object sender, EventArgs e) => this.RefreshPage();

    private void btnStop_Click(object sender, EventArgs e) => this.StopNavigation();

    private void btnPrint_Click(object sender, EventArgs e) => this.PrintPage();

    private void gnetBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.displayHelpTopic();
    }

    private void displayHelpTopic()
    {
      if (string.IsNullOrEmpty(this.m_sHelpKey))
        return;
      JedHelp.ShowHelp((Control) this, this.m_sHelpKey, this.m_sHelpKey);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BrowserControl));
      this.pnlToolbar = new GradientPanel();
      this.pnlToolbarLayout = new FlowLayoutPanel();
      this.btnHome = new IconButton();
      this.btnBack = new IconButton();
      this.btnForward = new IconButton();
      this.btnRefresh = new IconButton();
      this.btnStop = new IconButton();
      this.btnPrint = new IconButton();
      this.pnlBorder = new BorderPanel();
      this.pnlWorkspace = new Panel();
      this.pnlBrowser = new Panel();
      this.gnetBrowser = new AxGNetMX();
      this.toolTip = new ToolTip(this.components);
      this.mnuBrowser = new ContextMenuStrip(this.components);
      this.mnuItemHome = new ToolStripMenuItem();
      this.mnuItemBack = new ToolStripMenuItem();
      this.mnuItemForward = new ToolStripMenuItem();
      this.mnuItemRefresh = new ToolStripMenuItem();
      this.mnuItemStop = new ToolStripMenuItem();
      this.mnuItemPrint = new ToolStripMenuItem();
      this.pnlToolbar.SuspendLayout();
      this.pnlToolbarLayout.SuspendLayout();
      ((ISupportInitialize) this.btnHome).BeginInit();
      ((ISupportInitialize) this.btnBack).BeginInit();
      ((ISupportInitialize) this.btnForward).BeginInit();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      ((ISupportInitialize) this.btnStop).BeginInit();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      this.pnlBorder.SuspendLayout();
      this.pnlWorkspace.SuspendLayout();
      this.pnlBrowser.SuspendLayout();
      this.gnetBrowser.BeginInit();
      this.mnuBrowser.SuspendLayout();
      this.SuspendLayout();
      this.pnlToolbar.Borders = AnchorStyles.Bottom;
      this.pnlToolbar.Controls.Add((Control) this.pnlToolbarLayout);
      this.pnlToolbar.Dock = DockStyle.Top;
      this.pnlToolbar.GradientColor1 = System.Drawing.Color.FromArgb(252, 252, 252);
      this.pnlToolbar.GradientColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
      this.pnlToolbar.Location = new Point(1, 1);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Padding = new Padding(6, 0, 0, 0);
      this.pnlToolbar.Size = new Size(515, 31);
      this.pnlToolbar.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlToolbar.TabIndex = 1;
      this.pnlToolbarLayout.BackColor = System.Drawing.Color.Transparent;
      this.pnlToolbarLayout.Controls.Add((Control) this.btnHome);
      this.pnlToolbarLayout.Controls.Add((Control) this.btnBack);
      this.pnlToolbarLayout.Controls.Add((Control) this.btnForward);
      this.pnlToolbarLayout.Controls.Add((Control) this.btnRefresh);
      this.pnlToolbarLayout.Controls.Add((Control) this.btnStop);
      this.pnlToolbarLayout.Controls.Add((Control) this.btnPrint);
      this.pnlToolbarLayout.Dock = DockStyle.Fill;
      this.pnlToolbarLayout.Location = new Point(6, 0);
      this.pnlToolbarLayout.Name = "pnlToolbarLayout";
      this.pnlToolbarLayout.Size = new Size(509, 31);
      this.pnlToolbarLayout.TabIndex = 2;
      this.btnHome.BackColor = System.Drawing.Color.Transparent;
      this.btnHome.DisabledImage = (Image) null;
      this.btnHome.Image = (Image) Resources.home_browser;
      this.btnHome.Location = new Point(1, 2);
      this.btnHome.Margin = new Padding(1, 2, 3, 2);
      this.btnHome.MouseOverImage = (Image) Resources.home_browser_over;
      this.btnHome.Name = "btnHome";
      this.btnHome.Size = new Size(25, 25);
      this.btnHome.TabIndex = 6;
      this.btnHome.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnHome, "Home");
      this.btnHome.Click += new EventHandler(this.btnHome_Click);
      this.btnBack.BackColor = System.Drawing.Color.Transparent;
      this.btnBack.DisabledImage = (Image) Resources.back_browser_disabled;
      this.btnBack.Image = (Image) Resources.back_browser;
      this.btnBack.Location = new Point(29, 2);
      this.btnBack.Margin = new Padding(0, 2, 3, 2);
      this.btnBack.MouseOverImage = (Image) Resources.back_browser_over;
      this.btnBack.Name = "btnBack";
      this.btnBack.Size = new Size(25, 25);
      this.btnBack.TabIndex = 7;
      this.btnBack.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnBack, "Go Back One Page");
      this.btnBack.Click += new EventHandler(this.btnBack_Click);
      this.btnForward.BackColor = System.Drawing.Color.Transparent;
      this.btnForward.DisabledImage = (Image) Resources.forward_browser_disabled;
      this.btnForward.Image = (Image) Resources.forward_browser;
      this.btnForward.Location = new Point(57, 2);
      this.btnForward.Margin = new Padding(0, 2, 3, 2);
      this.btnForward.MouseOverImage = (Image) Resources.forward_browser_over;
      this.btnForward.Name = "btnForward";
      this.btnForward.Size = new Size(25, 25);
      this.btnForward.TabIndex = 8;
      this.btnForward.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnForward, "Go Forward One Page");
      this.btnForward.Click += new EventHandler(this.btnForward_Click);
      this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
      this.btnRefresh.DisabledImage = (Image) null;
      this.btnRefresh.Image = (Image) Resources.refresh_browser;
      this.btnRefresh.Location = new Point(87, 2);
      this.btnRefresh.Margin = new Padding(2, 2, 4, 2);
      this.btnRefresh.MouseOverImage = (Image) Resources.refresh_browser_over;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(25, 25);
      this.btnRefresh.TabIndex = 9;
      this.btnRefresh.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnRefresh, "Refresh Current Page");
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.btnStop.BackColor = System.Drawing.Color.Transparent;
      this.btnStop.DisabledImage = (Image) Resources.stop_browser_disabled;
      this.btnStop.Image = (Image) Resources.stop_browser;
      this.btnStop.Location = new Point(117, 2);
      this.btnStop.Margin = new Padding(1, 2, 3, 2);
      this.btnStop.MouseOverImage = (Image) Resources.stop_browser_over;
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(25, 25);
      this.btnStop.TabIndex = 10;
      this.btnStop.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnStop, "Stop Loading This Page");
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.btnPrint.BackColor = System.Drawing.Color.Transparent;
      this.btnPrint.DisabledImage = (Image) null;
      this.btnPrint.Image = (Image) Resources.print_browser;
      this.btnPrint.Location = new Point(146, 2);
      this.btnPrint.Margin = new Padding(1, 2, 0, 2);
      this.btnPrint.MouseOverImage = (Image) Resources.print_browser_over;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(25, 25);
      this.btnPrint.TabIndex = 11;
      this.btnPrint.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnPrint, "Print This Page");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.pnlBorder.Controls.Add((Control) this.pnlWorkspace);
      this.pnlBorder.Controls.Add((Control) this.pnlToolbar);
      this.pnlBorder.Dock = DockStyle.Fill;
      this.pnlBorder.Location = new Point(0, 0);
      this.pnlBorder.Name = "pnlBorder";
      this.pnlBorder.Size = new Size(517, 317);
      this.pnlBorder.TabIndex = 0;
      this.pnlWorkspace.Controls.Add((Control) this.pnlBrowser);
      this.pnlWorkspace.Dock = DockStyle.Fill;
      this.pnlWorkspace.Location = new Point(1, 32);
      this.pnlWorkspace.Name = "pnlWorkspace";
      this.pnlWorkspace.Size = new Size(515, 284);
      this.pnlWorkspace.TabIndex = 3;
      this.pnlBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlBrowser.BackColor = SystemColors.Window;
      this.pnlBrowser.Controls.Add((Control) this.gnetBrowser);
      this.pnlBrowser.Location = new Point(-2, -2);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(519, 288);
      this.pnlBrowser.TabIndex = 4;
      this.gnetBrowser.Enabled = true;
      this.gnetBrowser.Location = new Point(0, 0);
      this.gnetBrowser.Name = "gnetBrowser";
      this.gnetBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("gnetBrowser.OcxState");
      this.gnetBrowser.Size = new Size(404, 236);
      this.gnetBrowser.TabIndex = 5;
      this.gnetBrowser.Visible = false;
      this.gnetBrowser.PreviewKeyDown += new PreviewKeyDownEventHandler(this.gnetBrowser_PreviewKeyDown);
      this.gnetBrowser.ePASSVBEvent += new _DGNetMXEvents_ePASSVBEventEventHandler(this.gnetBrowser_ePASSVBEvent);
      this.mnuBrowser.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.mnuItemHome,
        (ToolStripItem) this.mnuItemBack,
        (ToolStripItem) this.mnuItemForward,
        (ToolStripItem) this.mnuItemRefresh,
        (ToolStripItem) this.mnuItemStop,
        (ToolStripItem) this.mnuItemPrint
      });
      this.mnuBrowser.Name = "mnuBrowser";
      this.mnuBrowser.Size = new Size(199, 186);
      this.mnuItemHome.Image = (Image) Resources.home;
      this.mnuItemHome.Name = "mnuItemHome";
      this.mnuItemHome.Size = new Size(198, 22);
      this.mnuItemHome.Text = "&Home Page";
      this.mnuItemHome.Click += new EventHandler(this.btnHome_Click);
      this.mnuItemBack.Image = (Image) Resources.arrow_back;
      this.mnuItemBack.Name = "mnuItemBack";
      this.mnuItemBack.ShortcutKeyDisplayString = "";
      this.mnuItemBack.ShortcutKeys = Keys.Left | Keys.Alt;
      this.mnuItemBack.Size = new Size(198, 22);
      this.mnuItemBack.Text = "&Back";
      this.mnuItemBack.Click += new EventHandler(this.btnBack_Click);
      this.mnuItemForward.Image = (Image) Resources.arrow_forward;
      this.mnuItemForward.Name = "mnuItemForward";
      this.mnuItemForward.ShortcutKeyDisplayString = "";
      this.mnuItemForward.ShortcutKeys = Keys.Right | Keys.Alt;
      this.mnuItemForward.Size = new Size(198, 22);
      this.mnuItemForward.Text = "&Forward";
      this.mnuItemForward.Click += new EventHandler(this.btnForward_Click);
      this.mnuItemRefresh.Image = (Image) Resources.refresh;
      this.mnuItemRefresh.Name = "mnuItemRefresh";
      this.mnuItemRefresh.ShortcutKeys = Keys.F5;
      this.mnuItemRefresh.Size = new Size(198, 22);
      this.mnuItemRefresh.Text = "&Refresh";
      this.mnuItemRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.mnuItemStop.Image = (Image) Resources.stop;
      this.mnuItemStop.Name = "mnuItemStop";
      this.mnuItemStop.Size = new Size(198, 22);
      this.mnuItemStop.Text = "&Stop";
      this.mnuItemStop.Click += new EventHandler(this.btnStop_Click);
      this.mnuItemPrint.Image = (Image) Resources.print;
      this.mnuItemPrint.Name = "mnuItemPrint";
      this.mnuItemPrint.ShortcutKeys = Keys.P | Keys.Control;
      this.mnuItemPrint.Size = new Size(198, 22);
      this.mnuItemPrint.Text = "&Print...";
      this.mnuItemPrint.Click += new EventHandler(this.btnPrint_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBorder);
      this.DoubleBuffered = true;
      this.Name = nameof (BrowserControl);
      this.Size = new Size(517, 317);
      this.pnlToolbar.ResumeLayout(false);
      this.pnlToolbarLayout.ResumeLayout(false);
      ((ISupportInitialize) this.btnHome).EndInit();
      ((ISupportInitialize) this.btnBack).EndInit();
      ((ISupportInitialize) this.btnForward).EndInit();
      ((ISupportInitialize) this.btnRefresh).EndInit();
      ((ISupportInitialize) this.btnStop).EndInit();
      ((ISupportInitialize) this.btnPrint).EndInit();
      this.pnlBorder.ResumeLayout(false);
      this.pnlWorkspace.ResumeLayout(false);
      this.pnlBrowser.ResumeLayout(false);
      this.gnetBrowser.EndInit();
      this.mnuBrowser.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private delegate bool PopulateFormDelegate(string filepath);

    private delegate bool CheckNavigateDelegate(string url);
  }
}
