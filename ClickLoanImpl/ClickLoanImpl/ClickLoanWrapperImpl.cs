// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanImpl.ClickLoanWrapperImpl
// Assembly: ClickLoanImpl, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 9549E162-7E74-49E9-BCDA-CB0A69B5F0B5
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClickLoanImpl.dll

using EllieMae.EMLite.ClickLoanWrapperUtil;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClickLoanImpl
{
  public class ClickLoanWrapperImpl
  {
    private const string className = "ClickLoanWrapperImpl";
    private const int defaultTimeoutSeconds = 180;
    private static TraceSwitch sw = new TraceSwitch("ClickLoanImpl", "ClickLoan Wrapper Impl");
    public static readonly string EncIpcRegistryRoot = "Software\\Ellie Mae\\Encompass\\IPC";
    private bool isSmartClient;
    private string installationURL;
    private EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl iCLImpl;
    private Process clProxy;
    private static IChannel encClientChannel = (IChannel) null;
    private static IChannel clientChannel = (IChannel) null;
    private CLRemoteObject clRemoteObject;
    private bool loggedIn;
    private static bool tcp = false;
    private static bool tcpSetExplicitly = false;
    private static string tempFolder = (string) null;
    private string portFile;
    private string clickLoanProxyExePath;
    private static int? timeoutSecondsRegistrySetting;
    private bool enableTimeout;
    private int timeoutSeconds;
    private ISponsor encRemoteObjSponsor = (ISponsor) new EncRemoteObjSponsor();
    private EncRemoteObject _encRemoteObject;
    private bool createIpcChannel = true;
    private bool ipcLoginCheckServer = true;
    private string completeServerUriPath;
    private string userid;
    private string password;
    private string loanGuid;

    static ClickLoanWrapperImpl()
    {
      string str = "1";
      RegistryKey registryKey1 = (RegistryKey) null;
      try
      {
        registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\ClickLoan");
        if (registryKey1 != null)
        {
          str = (string) registryKey1.GetValue("TraceLevel");
          if (str != null)
            str = str.Trim();
        }
      }
      catch
      {
      }
      finally
      {
        registryKey1?.Close();
      }
      ClickLoanWrapperImpl.tempFolder = Environment.GetEnvironmentVariable("TEMP");
      Tracing.Init(Path.Combine(ClickLoanWrapperImpl.tempFolder, "ClickLoan"));
      ClickLoanWrapperImpl.sw.Level = !(str == "0") ? (!(str == "1") ? (!(str == "2") ? (!(str == "3") ? (!(str == "4") ? TraceLevel.Error : TraceLevel.Verbose) : TraceLevel.Info) : TraceLevel.Warning) : TraceLevel.Error) : TraceLevel.Off;
      ClickLoanWrapperImpl.tcp = Environment.OSVersion.Version >= new Version("6.0.0.0");
      try
      {
        using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass\\ClickLoan"))
        {
          if (registryKey2 != null)
          {
            string s = string.Concat(registryKey2.GetValue("TimeoutSeconds")).Trim();
            if (s == "")
            {
              ClickLoanWrapperImpl.timeoutSecondsRegistrySetting = new int?();
            }
            else
            {
              int result;
              if (!int.TryParse(s, out result))
                throw new Exception("Invalid TimeoutSeconds value: " + s);
              if (result < 10)
              {
                result = 10;
                Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Info, "Resetting TimeoutSeconds to 10. Registry value was: " + s);
              }
              ClickLoanWrapperImpl.timeoutSecondsRegistrySetting = new int?(result);
            }
            string strA = string.Concat(registryKey2.GetValue("ChannelType")).Trim();
            if (string.Compare(strA, "IPC", true) == 0)
            {
              ClickLoanWrapperImpl.tcp = false;
              ClickLoanWrapperImpl.tcpSetExplicitly = true;
            }
            else
            {
              if (string.Compare(strA, "TCP", true) != 0)
                throw new Exception("Unrecognized channel type '" + strA + "'");
              ClickLoanWrapperImpl.tcp = true;
              ClickLoanWrapperImpl.tcpSetExplicitly = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Unable to get registry entry: " + ex.Message);
      }
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Info, "OS: " + Environment.OSVersion.VersionString);
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Info, "Channel type: " + (ClickLoanWrapperImpl.tcp ? "TCP" : "IPC"));
    }

    public static void GetIpcChannelRegistrySettings(
      ref bool createIpcChannel,
      ref bool ipcLoginCheckServer)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
      {
        if (registryKey == null)
          return;
        if (((string) registryKey.GetValue("CreateIpcChannel") ?? "").Trim() == "0")
          createIpcChannel = false;
        if (!(((string) registryKey.GetValue("IpcLoginCheckServer") ?? "").Trim() == "0"))
          return;
        ipcLoginCheckServer = false;
      }
    }

    public ClickLoanWrapperImpl(
      string clickLoanProxyExePath,
      bool isSmartClient,
      string installationURL)
    {
      this.isSmartClient = isSmartClient;
      if (this.isSmartClient)
        this.installationURL = installationURL;
      this.clickLoanProxyExePath = clickLoanProxyExePath;
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, isSmartClient ? "Smart Client" : "Non-Smart Client");
      this.init();
    }

    private void init()
    {
      try
      {
        Assembly entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != (Assembly) null)
        {
          string fullName = entryAssembly.FullName;
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Entry assembly name: " + fullName);
          if (fullName != null && fullName.IndexOf("Encompass,") == 0)
            this.iCLImpl = new EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl();
        }
        ClickLoanWrapperImpl.GetIpcChannelRegistrySettings(ref this.createIpcChannel, ref this.ipcLoginCheckServer);
        if (!ClickLoanWrapperImpl.timeoutSecondsRegistrySetting.HasValue)
          this.timeoutSeconds = 180;
        else
          this.timeoutSeconds = ClickLoanWrapperImpl.timeoutSecondsRegistrySetting.Value;
      }
      catch (Exception ex)
      {
      }
    }

    private IChannel registerClientChannel(IChannel channel, bool tcp)
    {
      if (channel != null)
        return channel;
      if (tcp)
      {
        string str = "0";
        try
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass\\ClickLoan"))
          {
            if (registryKey != null)
              str = string.Concat(registryKey.GetValue("TcpSetServerProvider")).Trim();
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Unable to get registry entry \"TcpSetServerProvider\": " + ex.Message);
        }
        if (str == "1")
          channel = (IChannel) new TcpChannel((IDictionary) new Dictionary<string, string>()
          {
            {
              "authorizedGroup",
              "Everyone"
            }
          }, (IClientChannelSinkProvider) null, (IServerChannelSinkProvider) new BinaryServerFormatterSinkProvider()
          {
            TypeFilterLevel = TypeFilterLevel.Full
          });
        else
          channel = (IChannel) new TcpChannel();
      }
      else
        channel = (IChannel) new IpcChannel((IDictionary) new Dictionary<string, string>()
        {
          {
            "authorizedGroup",
            "Everyone"
          },
          {
            "portName",
            Guid.NewGuid().ToString()
          }
        }, (IClientChannelSinkProvider) null, (IServerChannelSinkProvider) new BinaryServerFormatterSinkProvider()
        {
          TypeFilterLevel = TypeFilterLevel.Full
        });
      ChannelServices.RegisterChannel(channel, false);
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Created and registered client " + (tcp ? "TCP" : "IPC") + "channel.");
      return channel;
    }

    public bool InsideEncompass() => this.iCLImpl != null;

    private void startClickLoanProxy()
    {
      if (this.iCLImpl != null || this.clProxy != null)
        return;
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "ClickLoanProxy executable path: " + this.clickLoanProxyExePath);
      string path2 = Guid.NewGuid().ToString();
      this.clProxy = new Process();
      this.clProxy.StartInfo.FileName = this.clickLoanProxyExePath;
      this.clProxy.StartInfo.UseShellExecute = false;
      this.clProxy.StartInfo.CreateNoWindow = true;
      this.clProxy.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      if (ClickLoanWrapperImpl.tcp)
      {
        this.portFile = Path.Combine(ClickLoanWrapperImpl.tempFolder, path2);
        this.clProxy.StartInfo.Arguments = "tcp " + this.portFile;
      }
      else
        this.clProxy.StartInfo.Arguments = "ipc " + path2;
      if (this.enableTimeout)
      {
        ProcessStartInfo startInfo = this.clProxy.StartInfo;
        startInfo.Arguments = startInfo.Arguments + " -TimeoutSeconds " + this.timeoutSeconds.ToString();
      }
      if (this.isSmartClient && this.installationURL != null)
        this.clProxy.StartInfo.Arguments = this.clProxy.StartInfo.Arguments + " -InstallationURL " + this.installationURL;
      this.clProxy.Start();
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "ClickLoan Proxy started with args: " + this.clProxy.StartInfo.Arguments);
      int num1 = 50;
      int millisecondsTimeout = 100;
      if (this.isSmartClient)
      {
        num1 = 1200;
        millisecondsTimeout = 100;
      }
      string str = (string) null;
      if (ClickLoanWrapperImpl.tcp)
      {
        for (int index = 0; index < num1; ++index)
        {
          if (File.Exists(this.portFile))
          {
            try
            {
              StreamReader streamReader = new StreamReader(this.portFile);
              str = streamReader.ReadToEnd();
              streamReader.Close();
              if (str != null)
              {
                if (str != "")
                  break;
              }
            }
            catch
            {
            }
          }
          Thread.Sleep(millisecondsTimeout);
        }
        try
        {
          if (File.Exists(this.portFile))
            File.Delete(this.portFile);
        }
        catch (Exception ex)
        {
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Info, "Error deleting port file: " + ex.Message);
        }
      }
      ClickLoanWrapperImpl.clientChannel = this.registerClientChannel(ClickLoanWrapperImpl.clientChannel, ClickLoanWrapperImpl.tcp);
      if (!ClickLoanWrapperImpl.tcp)
        ClickLoanWrapperImpl.encClientChannel = ClickLoanWrapperImpl.clientChannel;
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Channel registered with " + (ClickLoanWrapperImpl.tcp ? " port = " + str : " remote = " + path2));
      this.clRemoteObject = (CLRemoteObject) Activator.GetObject(typeof (CLRemoteObject), !ClickLoanWrapperImpl.tcp ? "ipc://" + path2 + "/CLRemoteObject.rem" : "tcp://127.0.0.1:" + str + "/CLRemoteObject.rem");
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Get remote object");
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        try
        {
          num2 = this.clRemoteObject.CheckAndInstallHotfix();
          break;
        }
        catch (RemotingException ex)
        {
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Info, ex.Message);
          Thread.Sleep(millisecondsTimeout);
        }
        catch (Exception ex)
        {
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, ex.Message);
          break;
        }
      }
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Connection OK");
      if (num2 == 1)
      {
        if (this.clProxy != null && !this.clProxy.HasExited)
        {
          this.clProxy.Kill();
          this.clProxy = (Process) null;
        }
        foreach (Process process in Process.GetProcessesByName("VersionControl"))
          process.WaitForExit();
        this.startClickLoanProxy();
      }
    }

    private void exitClickLoanProxy()
    {
      if (this.clProxy == null)
        return;
      try
      {
        if (!this.clProxy.HasExited)
        {
          if (this.loggedIn)
            this.Logout();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Error trying to logout: " + ex.Message);
      }
      try
      {
        if (this.clProxy.HasExited)
          return;
        this.clProxy.Kill();
      }
      catch (Exception ex)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Error trying to terminate ClickLoan Proxy: " + ex.Message);
      }
      finally
      {
        this.clProxy = (Process) null;
      }
    }

    private int resetClickLoanProxy()
    {
      try
      {
        this.exitClickLoanProxy();
        this.startClickLoanProxy();
      }
      catch (Exception ex)
      {
        return -1;
      }
      return 0;
    }

    ~ClickLoanWrapperImpl()
    {
      try
      {
        this.encRemoteObject = (EncRemoteObject) null;
      }
      catch
      {
      }
      try
      {
        this.exitClickLoanProxy();
      }
      catch
      {
      }
      try
      {
        Tracing.Close();
      }
      catch
      {
      }
    }

    public string GetUserid()
    {
      if (this.iCLImpl != null)
        return this.iCLImpl.GetUserid();
      this.startClickLoanProxy();
      return this.clRemoteObject.GetUserid();
    }

    public string GetPassword()
    {
      if (this.iCLImpl != null)
        return this.iCLImpl.GetPassword();
      this.startClickLoanProxy();
      return this.clRemoteObject.GetPassword();
    }

    public string GetEncompassServer()
    {
      if (this.iCLImpl != null)
        return this.iCLImpl.GetEncompassServer();
      this.startClickLoanProxy();
      return this.clRemoteObject.GetEncompassServer();
    }

    public int LoginWithAuthCode(string serverName, string loginName, string AuthCode)
    {
      this.startClickLoanProxy();
      string userid = this.GetUserid();
      if (userid != null && userid != string.Empty)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Warning, "Userid not null; shouldn't login again.");
        return 11;
      }
      if (this.iCLImpl != null)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Inside of Encompass");
        return this.iCLImpl.LoginWithAuthCode(serverName, loginName, AuthCode);
      }
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Try logging to server " + serverName + " with login name " + loginName);
      int num = this.clRemoteObject.LoginWithAuthCode(serverName, loginName, AuthCode);
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Login reutrn code: " + (object) num);
      switch (num)
      {
        case 0:
          this.loggedIn = true;
          break;
        case 100:
          Process.GetCurrentProcess().Kill();
          break;
        case 101:
          if (this.clProxy != null && !this.clProxy.HasExited)
            this.clProxy.Kill();
          if (MessageBox.Show((IWin32Window) null, "Version update started.  ClickLoan procedure won't continue.  Do you want to exit from your current application?", "Version Update Started", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          {
            Process.GetCurrentProcess().Kill();
            break;
          }
          break;
        default:
          if (num < 0)
          {
            this.resetClickLoanProxy();
            break;
          }
          break;
      }
      return num;
    }

    public int Login(string serverName, string loginName, string password)
    {
      this.startClickLoanProxy();
      string userid = this.GetUserid();
      if (userid != null && userid != string.Empty)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Warning, "Userid not null; shouldn't login again.");
        return 11;
      }
      if (this.iCLImpl != null)
      {
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Inside of Encompass");
        return this.iCLImpl.Login(serverName, loginName, password);
      }
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Try logging to server " + serverName + " with login name " + loginName);
      int num = this.clRemoteObject.Login(serverName, loginName, password);
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Login reutrn code: " + (object) num);
      switch (num)
      {
        case 0:
          this.loggedIn = true;
          break;
        case 100:
          Process.GetCurrentProcess().Kill();
          break;
        case 101:
          if (this.clProxy != null && !this.clProxy.HasExited)
            this.clProxy.Kill();
          if (MessageBox.Show((IWin32Window) null, "Version update started.  ClickLoan procedure won't continue.  Do you want to exit from your current application?", "Version Update Started", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          {
            Process.GetCurrentProcess().Kill();
            break;
          }
          break;
        default:
          if (num < 0)
          {
            this.resetClickLoanProxy();
            break;
          }
          break;
      }
      return num;
    }

    public string GetFolderList()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetFolderList() : this.clRemoteObject.GetFolderList();
    }

    public string GetWorkingFolder()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetWorkingFolder() : this.clRemoteObject.GetWorkingFolder();
    }

    public string GetWorkingLoanName()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetWorkingLoanName() : this.clRemoteObject.GetWorkingLoanName();
    }

    public string GetLoanList(string loanFolder)
    {
      if (loanFolder == null || loanFolder == "")
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "GetLoanList(): loanFolder is null or empty");
      return this.iCLImpl != null ? this.iCLImpl.GetLoanList(loanFolder) : this.clRemoteObject.GetLoanList(loanFolder);
    }

    public int LoadLoanFile(string loanFolder, string loanName)
    {
      if (loanFolder == null || loanFolder == "")
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "LoadLoanFile(): loanFolder is null or empty");
      if (loanName == null || loanFolder == "")
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "LoadLoanFile(): loanName is null or empty");
      return this.iCLImpl != null ? this.iCLImpl.LoadLoanFile(loanFolder, loanName) : this.clRemoteObject.LoadLoanFile(loanFolder, loanName);
    }

    public string GetField(string id)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetField(id) : this.clRemoteObject.GetField(id);
    }

    public int ValidateData(string format, bool allowContinue)
    {
      return this.iCLImpl != null ? this.iCLImpl.ValidateData(format, allowContinue) : this.clRemoteObject.ValidateData(format, allowContinue);
    }

    public int CreateExportFile(string format, string filePath)
    {
      if (this.iCLImpl != null)
        return this.iCLImpl.CreateExportFile(format, filePath);
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "CreateExportFile() filePath = " + filePath);
      return this.clRemoteObject.CreateExportFile(format, filePath);
    }

    public string GetClientID()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetClientID() : this.clRemoteObject.GetClientID();
    }

    public int Logout()
    {
      if (this.iCLImpl != null)
      {
        int num = this.iCLImpl.Logout();
        this.loggedIn = false;
        return num;
      }
      try
      {
        this.clRemoteObject.Logout();
      }
      finally
      {
        this.loggedIn = false;
      }
      this.exitClickLoanProxy();
      return 0;
    }

    public string[] GetOffers(string companyID)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetOffers(companyID) : this.clRemoteObject.GetOffers(companyID);
    }

    public string GetOfferCompanyProgram(string offerID)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetOfferCompanyProgram(offerID) : this.clRemoteObject.GetOfferCompanyProgram(offerID);
    }

    public int OpenLoan(string loanFolder, string loanName)
    {
      return this.iCLImpl != null ? this.iCLImpl.OpenLoan(loanFolder, loanName) : this.clRemoteObject.OpenLoan(loanFolder, loanName);
    }

    public int SaveLoan()
    {
      return this.iCLImpl != null ? this.iCLImpl.SaveLoan() : this.clRemoteObject.SaveLoan();
    }

    public int CloseLoan()
    {
      return this.iCLImpl != null ? this.iCLImpl.CloseLoan() : this.clRemoteObject.CloseLoan();
    }

    public int AddeFolderAttachment(byte[] fileContent, string fileFormat, string fileDesc)
    {
      return this.iCLImpl != null ? this.iCLImpl.AddeFolderAttachment(fileContent, fileFormat, fileDesc) : this.clRemoteObject.AddeFolderAttachment(fileContent, fileFormat, fileDesc);
    }

    public int AddeFolderAttachment(string fileName, string fileFormat, string fileDesc)
    {
      return this.iCLImpl != null ? this.iCLImpl.AddeFolderAttachment(fileName, fileFormat, fileDesc) : this.clRemoteObject.AddeFolderAttachment(fileName, fileFormat, fileDesc);
    }

    public string[] GetDocumentList()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetDocumentList() : this.clRemoteObject.GetDocumentList();
    }

    public string GetDocumentListXml()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetDocumentListXml() : this.clRemoteObject.GetDocumentListXml();
    }

    public string GetDocumentTitle(string docID)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetDocumentTitle(docID) : this.clRemoteObject.GetDocumentTitle(docID);
    }

    public string GetDocumentCompany(string docID)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetDocumentCompany(docID) : this.clRemoteObject.GetDocumentCompany(docID);
    }

    public string GetDocumentPairID(string docID)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetDocumentPairID(docID) : this.clRemoteObject.GetDocumentCompany(docID);
    }

    public bool SupportsImageAttachments()
    {
      return this.iCLImpl != null ? this.iCLImpl.SupportsImageAttachments() : this.clRemoteObject.SupportsImageAttachments();
    }

    public bool CanAddAttachment(string docID)
    {
      return this.iCLImpl != null ? this.iCLImpl.CanAddAttachment(docID) : this.clRemoteObject.CanAddAttachment(docID);
    }

    public string AddAttachment(string filepath, string title, string docID)
    {
      return this.iCLImpl != null ? this.iCLImpl.AddAttachment(filepath, title, docID) : this.clRemoteObject.AddAttachment(filepath, title, docID);
    }

    public string AddAttachment(string[] imageFiles, string title, string docID)
    {
      return this.iCLImpl != null ? this.iCLImpl.AddAttachment(imageFiles, title, docID) : this.clRemoteObject.AddAttachment(imageFiles, title, docID);
    }

    private EncRemoteObject encRemoteObject
    {
      get => this._encRemoteObject;
      set
      {
        if (this._encRemoteObject != null)
        {
          try
          {
            ((ILease) RemotingServices.GetLifetimeService((MarshalByRefObject) this._encRemoteObject)).Unregister(this.encRemoteObjSponsor);
            this._encRemoteObject = (EncRemoteObject) null;
          }
          catch
          {
          }
        }
        this._encRemoteObject = value;
        if (this._encRemoteObject == null)
          return;
        ((ILease) RemotingServices.GetLifetimeService((MarshalByRefObject) this._encRemoteObject)).Register(this.encRemoteObjSponsor);
      }
    }

    private bool canAddDoc
    {
      get
      {
        if (this.iCLImpl != null)
          return this.iCLImpl.CanAddDoc;
        if (this.clRemoteObject != null)
          return this.clRemoteObject.CanAddDoc;
        int num = (int) MessageBox.Show("CC: ClickLoan remote object not set.");
        return true;
      }
    }

    public int EncLogin(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      int encompass = this.loginToEncompass(this.ipcLoginCheckServer ? completeServerUriPath : (string) null, userid, password, loanGuid);
      if (encompass == 0)
        return encompass;
      this.completeServerUriPath = this.userid = this.password = this.loanGuid = (string) null;
      return encompass;
    }

    private int loginToEncompass(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      if (this.encRemoteObject != null && (this.completeServerUriPath == null || completeServerUriPath == null || string.Compare(this.completeServerUriPath, completeServerUriPath, true) == 0) && string.Compare(this.userid, userid, true) == 0 && string.Compare(this.password ?? "", password ?? "", true) == 0 && loanGuid == null)
      {
        this.loanGuid = (string) null;
        return 0;
      }
      this.encRemoteObject = (EncRemoteObject) null;
      this.completeServerUriPath = completeServerUriPath;
      this.userid = userid;
      this.password = password;
      this.loanGuid = loanGuid;
      int encompass = -1000;
      ClickLoanWrapperImpl.clientChannel = ClickLoanWrapperImpl.encClientChannel = this.registerClientChannel(ClickLoanWrapperImpl.encClientChannel, false);
      foreach (string encIpcPortName in this.getEncIpcPortNames())
      {
        try
        {
          this.encRemoteObject = (EncRemoteObject) Activator.CreateInstance(typeof (EncRemoteObject), (object[]) null, new object[1]
          {
            (object) new UrlAttribute("ipc://" + encIpcPortName + "/EncRemoteObject.rem")
          });
          if (this.encRemoteObject != null)
          {
            Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Channel registered with port name " + encIpcPortName);
            encompass = this.encRemoteObject.Login(completeServerUriPath, userid, password, loanGuid);
            if (encompass != 0)
              this.encRemoteObject = (EncRemoteObject) null;
            if (loanGuid != null)
            {
              if (encompass == 0)
                break;
            }
            else
              break;
          }
          else
            encompass = -1000;
        }
        catch (Exception ex)
        {
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Try connecting to IPC port " + encIpcPortName + ": " + ex.Message);
          this.encRemoteObject = (EncRemoteObject) null;
          this.removePortNameRegistryKey(encIpcPortName);
          encompass = -1000;
        }
      }
      if (this.encRemoteObject == null)
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "No matching Encompass process running on local machine.");
      return encompass;
    }

    private string[] getEncIpcPortNames()
    {
      List<KeyValuePair<DateTime, string>> keyValuePairList = new List<KeyValuePair<DateTime, string>>();
      using (RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot))
      {
        if (registryKey1 != null)
        {
          foreach (string subKeyName in registryKey1.GetSubKeyNames())
          {
            using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot + "\\" + subKeyName))
            {
              DateTime minValue = DateTime.MinValue;
              try
              {
                minValue = DateTime.Parse((string) registryKey2.GetValue("CreationTime"));
              }
              catch
              {
              }
              KeyValuePair<DateTime, string> keyValuePair = new KeyValuePair<DateTime, string>(minValue, subKeyName);
              keyValuePairList.Add(keyValuePair);
            }
          }
          ClickLoanWrapperImpl.PortNameComparer portNameComparer = new ClickLoanWrapperImpl.PortNameComparer();
          keyValuePairList.Sort((IComparer<KeyValuePair<DateTime, string>>) portNameComparer);
        }
      }
      string[] encIpcPortNames = new string[keyValuePairList.Count];
      for (int index = 0; index < encIpcPortNames.Length; ++index)
        encIpcPortNames[index] = keyValuePairList[index].Value;
      return encIpcPortNames;
    }

    private void removePortNameRegistryKey(string portName)
    {
      bool flag = false;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot + "\\" + portName, true))
      {
        if (registryKey == null)
          return;
        int num;
        if ((num = (int) registryKey.GetValue("ErrorCount") + 1) >= 3)
          flag = true;
        else
          registryKey.SetValue("ErrorCount", (object) num, RegistryValueKind.DWord);
      }
      if (!flag)
        return;
      Registry.CurrentUser.DeleteSubKey(ClickLoanWrapperImpl.EncIpcRegistryRoot + "\\" + portName);
    }

    private bool ensureLoggedIn()
    {
      if (this.encRemoteObject != null)
        return true;
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "ensureLoggedIn() returns null encRemoteObject");
      return false;
    }

    public string GetOpenLoanGuid()
    {
      if (!this.ensureLoggedIn())
        return (string) null;
      try
      {
        return this.encRemoteObject.GetOpenLoanGuid(this.completeServerUriPath, this.userid, this.password);
      }
      catch
      {
        this.encRemoteObject = (EncRemoteObject) null;
        return this.GetOpenLoanGuid();
      }
    }

    public string GetAllOpenLoans(string completeServerUriPath, string userid, string password)
    {
      ClpXml clpXml = new ClpXml("LoanList");
      ClickLoanWrapperImpl.clientChannel = ClickLoanWrapperImpl.encClientChannel = this.registerClientChannel(ClickLoanWrapperImpl.encClientChannel, false);
      foreach (string encIpcPortName in this.getEncIpcPortNames())
      {
        try
        {
          EncRemoteObject instance = (EncRemoteObject) Activator.CreateInstance(typeof (EncRemoteObject), (object[]) null, new object[1]
          {
            (object) new UrlAttribute("ipc://" + encIpcPortName + "/EncRemoteObject.rem")
          });
          if (instance != null)
          {
            Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Channel registered with port name " + encIpcPortName);
            if (instance.Login(completeServerUriPath, userid, password, (string) null) == 0)
            {
              string openLoanInfo = instance.GetOpenLoanInfo(completeServerUriPath, userid, password);
              if (openLoanInfo != null)
              {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(openLoanInfo);
                XmlElement xmlElement1 = (XmlElement) null;
                if (xmlDocument.DocumentElement.HasChildNodes)
                  xmlElement1 = xmlDocument.DocumentElement.FirstChild as XmlElement;
                if (xmlElement1 != null)
                {
                  XmlElement xmlElement2 = clpXml.AddChildElement(clpXml.Root, xmlElement1.Name);
                  xmlElement2.SetAttribute("EncIpcPortName", encIpcPortName);
                  foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlElement1.Attributes)
                    xmlElement2.SetAttribute(attribute.Name, attribute.Value);
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Try connecting to IPC port " + encIpcPortName + ": " + ex.Message);
          this.removePortNameRegistryKey(encIpcPortName);
        }
      }
      return clpXml.ToString();
    }

    public int AddeFolderAttachmentToOpenLoan(
      byte[] fileContent,
      string fileFormat,
      string fileDesc)
    {
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Trying to add attachment '" + fileDesc + "' to Encompass current loan");
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        if (!this.canAddDoc)
          throw new Exception("Cannot add doc");
        return this.encRemoteObject.AddeFolderAttachment(this.completeServerUriPath, this.userid, this.password, this.loanGuid, fileContent, fileFormat, fileDesc);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
    }

    public int AddeFolderAttachmentToOpenLoan(string fileName, string fileFormat, string fileDesc)
    {
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Trying to add attachment '" + fileName + "/" + fileDesc + "'");
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        if (!this.canAddDoc)
          throw new Exception("Cannot add doc");
        return this.encRemoteObject.AddeFolderAttachment(this.completeServerUriPath, this.userid, this.password, this.loanGuid, fileName, fileFormat, fileDesc);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
    }

    public string[] GetDocumentListFromOpenLoan()
    {
      if (!this.ensureLoggedIn())
        return (string[]) null;
      try
      {
        return this.encRemoteObject.GetDocumentList(this.completeServerUriPath, this.userid, this.password, this.loanGuid);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error getting document list: " + ex.Message);
        return (string[]) null;
      }
    }

    public string GetDocumentListXmlFromOpenLoan()
    {
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        return this.encRemoteObject.GetDocumentListXml(this.completeServerUriPath, this.userid, this.password, this.loanGuid);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error getting document list Xml: " + ex.Message);
        throw ex;
      }
    }

    public string GetDocumentTitleFromOpenLoan(string docID)
    {
      if (!this.ensureLoggedIn())
        return (string) null;
      try
      {
        return this.encRemoteObject.GetDocumentTitle(this.completeServerUriPath, this.userid, this.password, this.loanGuid, docID);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error getting document title: " + ex.Message);
        return (string) null;
      }
    }

    public string GetDocumentCompanyFromOpenLoan(string docID)
    {
      if (!this.ensureLoggedIn())
        return (string) null;
      try
      {
        return this.encRemoteObject.GetDocumentCompany(this.completeServerUriPath, this.userid, this.password, this.loanGuid, docID);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error getting document company: " + ex.Message);
        return (string) null;
      }
    }

    public string GetDocumentPairIDFromOpenLoan(string docID)
    {
      if (!this.ensureLoggedIn())
        return (string) null;
      try
      {
        return this.encRemoteObject.GetDocumentPairID(this.completeServerUriPath, this.userid, this.password, this.loanGuid, docID);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error getting document company: " + ex.Message);
        return (string) null;
      }
    }

    public bool CanAddAttachmentToOpenLoan(string docID)
    {
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        return this.encRemoteObject.CanAddAttachment(this.completeServerUriPath, this.userid, this.password, this.loanGuid, docID);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error checking if user can add attachment: " + ex.Message);
        throw ex;
      }
    }

    public string AddAttachmentToOpenLoan(string filepath, string title, string docID)
    {
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        return this.encRemoteObject.AddAttachment(this.completeServerUriPath, this.userid, this.password, this.loanGuid, filepath, title, docID);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
    }

    public bool SupportsImageAttachmentsToOpenLoan()
    {
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        return this.encRemoteObject.SupportsImageAttachments(this.completeServerUriPath, this.userid, this.password, this.loanGuid);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
    }

    public string AddAttachmentToOpenLoan(string[] imageFiles, string title, string docID)
    {
      if (!this.ensureLoggedIn())
        throw new Exception("Not logged in yet");
      try
      {
        return this.encRemoteObject.AddAttachment(this.completeServerUriPath, this.userid, this.password, this.loanGuid, imageFiles, title, docID);
      }
      catch (Exception ex)
      {
        this.encRemoteObject = (EncRemoteObject) null;
        Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
    }

    public void SetChannelType(bool useTCP) => ClickLoanWrapperImpl.tcp = useTCP;

    public string NewLoan(string pipeline, string loanName)
    {
      return this.iCLImpl != null ? this.iCLImpl.NewLoan(pipeline, loanName) : this.clRemoteObject.NewLoan(pipeline, loanName);
    }

    public void SetField(string id, string val)
    {
      if (this.iCLImpl != null)
        this.iCLImpl.SetField(id, val);
      this.clRemoteObject.SetField(id, val);
    }

    public string GetVersion()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetVersion() : this.clRemoteObject.GetVersion();
    }

    public int LoginFromServer(string serverName, string loginName, string password)
    {
      this.enableTimeout = true;
      if (!ClickLoanWrapperImpl.tcpSetExplicitly)
        this.SetChannelType(true);
      return this.Login(serverName, loginName, password);
    }

    public string GetLoanFolderRuleInfo(string loanFolder)
    {
      return this.iCLImpl != null ? this.iCLImpl.GetLoanFolderRuleInfo(loanFolder) : this.clRemoteObject.GetLoanFolderRuleInfo(loanFolder);
    }

    public string ProductPricingPartnerSetting(string partnerID, string property)
    {
      return this.iCLImpl != null ? this.iCLImpl.ProductPricingPartnerSetting(partnerID, property) : this.clRemoteObject.ProductPricingPartnerSetting(partnerID, property);
    }

    public string GetAllLoanFolderDetails()
    {
      return this.iCLImpl != null ? this.iCLImpl.GetAllLoanFolderDetails() : this.clRemoteObject.GetAllLoanFolderDetails();
    }

    public string ShowLoginScreen(
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "",
      string iconBase64 = "")
    {
      this.startClickLoanProxy();
      return this.iCLImpl != null ? this.iCLImpl.ShowLoginScreen(clientId, scope, oauthUrl, redirectUrl, iconBase64) : this.clRemoteObject.ShowLoginScreen(clientId, scope, oauthUrl, redirectUrl, iconBase64);
    }

    public int LoginWithAccessToken(
      string serverName,
      string loginName,
      string accessToken,
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl)
    {
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "Inside LoginWithAccessToken method");
      this.startClickLoanProxy();
      string AuthCode = this.iCLImpl == null ? this.clRemoteObject.GenerateAuthCodeFromToken(accessToken, clientId, scope, oauthUrl, redirectUrl) : this.iCLImpl.GenerateAuthCodeFromToken(accessToken, clientId, scope, oauthUrl, redirectUrl);
      Tracing.Log(ClickLoanWrapperImpl.sw, nameof (ClickLoanWrapperImpl), TraceLevel.Verbose, "LoginWithAccessToken: Authcode generated successfully from accessToken");
      return this.LoginWithAuthCode(serverName, loginName, AuthCode);
    }

    private class PortNameComparer : IComparer<KeyValuePair<DateTime, string>>
    {
      public int Compare(
        KeyValuePair<DateTime, string> keyVal1,
        KeyValuePair<DateTime, string> keyVal2)
      {
        if (keyVal1.Key < keyVal2.Key)
          return -1;
        return keyVal1.Key == keyVal2.Key ? 0 : 1;
      }
    }
  }
}
