// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoan.IClickLoan
// Assembly: EMClickLoan, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 81504B97-FAE4-453C-9546-2FF46D747061
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMClickLoan.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.ClickLoan
{
  [Guid("87145C94-9EF4-334F-8BCD-4B266DDB2DCF")]
  public interface IClickLoan
  {
    bool InsideEncompass();

    string GetUserid();

    string GetPassword();

    string GetEncompassServer();

    int Login(string serverName, string loginName, string password);

    string GetFolderList();

    string GetWorkingFolder();

    string GetWorkingLoanName();

    string GetLoanList(string loanFolder);

    int LoadLoanFile(string loanFolder, string loanName);

    string GetField(string id);

    int ValidateData(string format, bool allowContinue);

    int CreateExportFile(string format, string filePath);

    string GetClientID();

    int Logout();

    string[] GetOffers(string companyID);

    string GetOfferCompanyProgram(string offerID);

    string GetInstallDir();

    string GetEpassDir();

    string GetLocalSettingsDir();

    int OpenLoan(string loanFolder, string loanName);

    int SaveLoan();

    int CloseLoan();

    int AddeFolderAttachment(byte[] fileContent, string fileFormat, string fileDesc);

    int AddeFolderAttachment(string fileName, string fileFormat, string fileDesc);

    string SCEncompassServer { get; }

    string[] GetDocumentList();

    string GetDocumentTitle(string docID);

    string GetDocumentCompany(string docID);

    string GetDocumentPairID(string docID);

    bool CanAddAttachment(string docID);

    string AddAttachment(string filepath, string title, string docID);

    string GetDocumentListXml();

    int AddeFolderAttachmentToOpenLoan(byte[] fileContent, string fileFormat, string fileDesc);

    int AddeFolderAttachmentToOpenLoan(string fileName, string fileFormat, string fileDesc);

    int EncLogin(string completeServerUriPath, string userid, string password, string loanGuid);

    string GetOpenLoanGuid();

    string GetAllOpenLoans(string completeServerUriPath, string userid, string password);

    string[] GetDocumentListFromOpenLoan();

    string GetDocumentTitleFromOpenLoan(string docID);

    string GetDocumentCompanyFromOpenLoan(string docID);

    string GetDocumentPairIDFromOpenLoan(string docID);

    bool CanAddAttachmentToOpenLoan(string docID);

    string AddAttachmentToOpenLoan(string filepath, string title, string docID);

    string GetDocumentListXmlFromOpenLoan();

    void SetChannelType(bool useTCP);

    string NewLoan(string pipeline, string loanName);

    void SetField(string id, string val);

    string GetVersion();

    int LoginFromServer(string serverName, string loginName, string password);

    string GetLoanFolderRuleInfo(string loanFolder);

    string ProductPricingPartnerSetting(string partnerID, string property);

    bool SupportsImageAttachments();

    string AddAttachment(string[] imageFiles, string title, string docID);

    bool SupportsImageAttachmentsToOpenLoan();

    string AddAttachmentToOpenLoan(string[] imageFiles, string title, string docID);

    string GetAllLoanFolderDetails();

    int LoginWithAuthCode(string serverName, string loginName, string authCode);

    string ShowLoginScreen(
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "",
      string iconBase64 = "");

    int LoginWithAccessToken(
      string serverName,
      string loginName,
      string accessToken,
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "https://encompass.elliemae.com/homepage/atest.asp");
  }
}
