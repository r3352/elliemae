// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoan.ClickLoan
// Assembly: EMClickLoan, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 81504B97-FAE4-453C-9546-2FF46D747061
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMClickLoan.dll

using EllieMae.EMLite.ClickLoanImpl;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClickLoan
{
  [Guid("84F3574F-CBB2-36DC-B3EF-2C3B85B4531C")]
  public class ClickLoan : IClickLoan
  {
    private bool isSmartClient;
    private ClickLoanWrapperImpl iCLImpl;
    private string encompassServer = "";

    public string SCEncompassServer => this.encompassServer ?? "";

    static ClickLoan()
    {
      string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      ServicePointManager.SecurityProtocol |= (SecurityProtocolType) 3840;
      AssemblyResolver.Start(directoryName, "ClickLoanMain.exe", (string[]) null, "Ellie Mae", "Encompass360");
      AssemblyResolver.FirstSmartClientAssembly = "ClickLoanImpl";
    }

    public ClickLoan()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string directoryName = Path.GetDirectoryName(executingAssembly.Location);
      string str = (string) null;
      if (AssemblyResolver.IsSmartClient)
      {
        this.isSmartClient = true;
        if (!AssemblyResolver.IsAsmFileUpToDate(executingAssembly.Location) && MessageBox.Show("Some Encompass ClickLoan program files may be out of date and ClickLoan may not work correctly. You can update ClickLoan files by closing the browser and then starting/stopping Encompass. Do you want to close your browser now?", "Encompass ClickLoan", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
          Process.GetCurrentProcess().Kill();
        if (AssemblyResolver.AuthResult != null && AssemblyResolver.AuthResult.ResultCode == AuthResultCode.Success)
        {
          if ((AssemblyResolver.AuthResult.ResultDescription ?? "").Trim() != "")
            str = AssemblyResolver.AuthResult.ResultDescription;
          if (AssemblyResolver.AuthResult.AppCmdLineArgs != null)
          {
            string appCmdLineArg = AssemblyResolver.AuthResult.AppCmdLineArgs["ClickLoanMain.exe"];
            if ((appCmdLineArg ?? "").Trim().StartsWith("-s"))
              this.encompassServer = appCmdLineArg.Substring(2).Trim();
          }
        }
      }
      this.iCLImpl = new ClickLoanWrapperImpl(Path.Combine(directoryName, "ClickLoanProxy.exe"), this.isSmartClient, str);
    }

    public static void Dummy()
    {
    }

    public bool InsideEncompass() => this.iCLImpl.InsideEncompass();

    ~ClickLoan() => this.iCLImpl = (ClickLoanWrapperImpl) null;

    public string GetUserid() => this.iCLImpl.GetUserid();

    public string GetPassword() => this.iCLImpl.GetPassword();

    public string GetEncompassServer() => this.iCLImpl.GetEncompassServer();

    public int Login(string serverName, string loginName, string password)
    {
      return this.iCLImpl.Login(serverName, loginName, password);
    }

    public int LoginWithAuthCode(string serverName, string loginName, string authCode)
    {
      return this.iCLImpl.LoginWithAuthCode(serverName, loginName, authCode);
    }

    public string ShowLoginScreen(
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "",
      string iconBase64 = "")
    {
      return this.iCLImpl.ShowLoginScreen(clientId, scope, oauthUrl, redirectUrl, iconBase64);
    }

    public int LoginWithAccessToken(
      string serverName,
      string loginName,
      string accessToken,
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "https://encompass.elliemae.com/homepage/atest.asp")
    {
      return this.iCLImpl.LoginWithAccessToken(serverName, loginName, accessToken, clientId, scope, oauthUrl, redirectUrl);
    }

    public string GetFolderList() => this.iCLImpl.GetFolderList();

    public string GetWorkingFolder() => this.iCLImpl.GetWorkingFolder();

    public string GetWorkingLoanName() => this.iCLImpl.GetWorkingLoanName();

    public string GetLoanList(string loanFolder) => this.iCLImpl.GetLoanList(loanFolder);

    public int LoadLoanFile(string loanFolder, string loanName)
    {
      return this.iCLImpl.LoadLoanFile(loanFolder, loanName);
    }

    public string GetField(string id) => this.iCLImpl.GetField(id);

    public int ValidateData(string format, bool allowContinue)
    {
      return this.iCLImpl.ValidateData(format, allowContinue);
    }

    public int CreateExportFile(string format, string filePath)
    {
      return this.iCLImpl.CreateExportFile(format, filePath);
    }

    public string GetClientID() => this.iCLImpl.GetClientID();

    public int Logout() => this.iCLImpl.Logout();

    public string[] GetOffers(string companyID) => this.iCLImpl.GetOffers(companyID);

    public string GetOfferCompanyProgram(string offerID)
    {
      return this.iCLImpl.GetOfferCompanyProgram(offerID);
    }

    public string GetInstallDir()
    {
      if (!this.isSmartClient)
        return EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory;
      string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      if (!directoryName.EndsWith("\\"))
        directoryName += "\\";
      return directoryName;
    }

    public string GetEpassDir()
    {
      return !this.isSmartClient ? SystemSettings.EpassDir : AssemblyResolver.GetResourceFileFolderPath("Epass") + "\\";
    }

    private object readRegistryValue(string valName)
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
        return registryKey == null ? (object) true : registryKey.GetValue(valName);
    }

    public string GetLocalSettingsDir()
    {
      object obj = this.readRegistryValue("SettingsDir");
      if (obj != null)
        return obj.ToString();
      return !this.isSmartClient ? SystemSettings.LocalSettingsDir : Path.Combine(AssemblyResolver.AppDataHashFolder, "EncompassData\\Settings\\LocalSettings\\");
    }

    public int OpenLoan(string loanFolder, string loanName)
    {
      return this.iCLImpl.OpenLoan(loanFolder, loanName);
    }

    public int SaveLoan() => this.iCLImpl.SaveLoan();

    public int CloseLoan() => this.iCLImpl.CloseLoan();

    public int AddeFolderAttachment(byte[] fileContent, string fileFormat, string fileDesc)
    {
      return this.iCLImpl.AddeFolderAttachment(fileContent, fileFormat, fileDesc);
    }

    public int AddeFolderAttachment(string fileName, string fileFormat, string fileDesc)
    {
      return this.iCLImpl.AddeFolderAttachment(fileName, fileFormat, fileDesc);
    }

    public string[] GetDocumentList() => this.iCLImpl.GetDocumentList();

    public string GetDocumentListXml() => this.iCLImpl.GetDocumentListXml();

    public string GetDocumentTitle(string docID) => this.iCLImpl.GetDocumentTitle(docID);

    public string GetDocumentCompany(string docID) => this.iCLImpl.GetDocumentCompany(docID);

    public string GetDocumentPairID(string docID) => this.iCLImpl.GetDocumentPairID(docID);

    public bool SupportsImageAttachments() => this.iCLImpl.SupportsImageAttachments();

    public bool CanAddAttachment(string docID) => this.iCLImpl.CanAddAttachment(docID);

    public string AddAttachment(string filepath, string title, string docID)
    {
      return this.iCLImpl.AddAttachment(filepath, title, docID);
    }

    public string AddAttachment(string[] imageFiles, string title, string docID)
    {
      return this.iCLImpl.AddAttachment(imageFiles, title, docID);
    }

    public int AddeFolderAttachmentToOpenLoan(
      byte[] fileContent,
      string fileFormat,
      string fileDesc)
    {
      return this.iCLImpl.AddeFolderAttachmentToOpenLoan(fileContent, fileFormat, fileDesc);
    }

    public int AddeFolderAttachmentToOpenLoan(string fileName, string fileFormat, string fileDesc)
    {
      return this.iCLImpl.AddeFolderAttachmentToOpenLoan(fileName, fileFormat, fileDesc);
    }

    public int EncLogin(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      return this.iCLImpl.EncLogin(completeServerUriPath, userid, password, loanGuid);
    }

    public string GetOpenLoanGuid() => this.iCLImpl.GetOpenLoanGuid();

    public string GetAllOpenLoans(string completeServerUriPath, string userid, string password)
    {
      return this.iCLImpl.GetAllOpenLoans(completeServerUriPath, userid, password);
    }

    public string[] GetDocumentListFromOpenLoan() => this.iCLImpl.GetDocumentListFromOpenLoan();

    public string GetDocumentListXmlFromOpenLoan() => this.iCLImpl.GetDocumentListXmlFromOpenLoan();

    public string GetDocumentTitleFromOpenLoan(string docID)
    {
      return this.iCLImpl.GetDocumentTitleFromOpenLoan(docID);
    }

    public string GetDocumentCompanyFromOpenLoan(string docID)
    {
      return this.iCLImpl.GetDocumentCompanyFromOpenLoan(docID);
    }

    public string GetDocumentPairIDFromOpenLoan(string docID)
    {
      return this.iCLImpl.GetDocumentPairIDFromOpenLoan(docID);
    }

    public bool CanAddAttachmentToOpenLoan(string docID)
    {
      return this.iCLImpl.CanAddAttachmentToOpenLoan(docID);
    }

    public string AddAttachmentToOpenLoan(string filepath, string title, string docID)
    {
      return this.iCLImpl.AddAttachmentToOpenLoan(filepath, title, docID);
    }

    public bool SupportsImageAttachmentsToOpenLoan()
    {
      return this.iCLImpl.SupportsImageAttachmentsToOpenLoan();
    }

    public string AddAttachmentToOpenLoan(string[] imageFiles, string title, string docID)
    {
      return this.iCLImpl.AddAttachmentToOpenLoan(imageFiles, title, docID);
    }

    public void SetChannelType(bool useTCP) => this.iCLImpl.SetChannelType(useTCP);

    public string NewLoan(string pipeLine, string loanName)
    {
      return this.iCLImpl.NewLoan(pipeLine, loanName);
    }

    public void SetField(string id, string val) => this.iCLImpl.SetField(id, val);

    public string GetVersion() => this.iCLImpl.GetVersion();

    public int LoginFromServer(string serverName, string loginName, string password)
    {
      return this.iCLImpl.LoginFromServer(serverName, loginName, password);
    }

    public string GetLoanFolderRuleInfo(string loanFolder)
    {
      return this.iCLImpl.GetLoanFolderRuleInfo(loanFolder);
    }

    public string ProductPricingPartnerSetting(string partnerID, string property)
    {
      return this.iCLImpl.ProductPricingPartnerSetting(partnerID, property);
    }

    public string GetAllLoanFolderDetails() => this.iCLImpl.GetAllLoanFolderDetails();
  }
}
