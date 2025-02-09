// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanImpl.CLRemoteObject
// Assembly: ClickLoanImpl, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 9549E162-7E74-49E9-BCDA-CB0A69B5F0B5
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClickLoanImpl.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClickLoanImpl
{
  public class CLRemoteObject : MarshalByRefObject
  {
    private EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl iCLImpl = new EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl();

    public string GetUserid() => this.iCLImpl.GetUserid();

    public string GetPassword() => this.iCLImpl.GetPassword();

    public string GetEncompassServer() => this.iCLImpl.GetEncompassServer();

    public int Login(string server, string userid, string password)
    {
      return this.iCLImpl.Login(server, userid, password);
    }

    public int LoginWithAuthCode(string server, string userid, string authCode)
    {
      return this.iCLImpl.LoginWithAuthCode(server, userid, authCode);
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

    public void Logout()
    {
      this.iCLImpl.Logout();
      Application.Exit();
    }

    public int CheckAndInstallHotfix() => this.iCLImpl.CheckAndInstallHotfix();

    public string[] GetOffers(string companyID) => this.iCLImpl.GetOffers(companyID);

    public string GetOfferCompanyProgram(string offerID)
    {
      return this.iCLImpl.GetOfferCompanyProgram(offerID);
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

    public bool CanAddDoc => this.iCLImpl.CanAddDoc;

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

    public string NewLoan(string pipeline, string loanName)
    {
      return this.iCLImpl.NewLoan(pipeline, loanName);
    }

    public void SetField(string id, string val) => this.iCLImpl.SetField(id, val);

    public string GetVersion() => this.iCLImpl.GetVersion();

    public string GetLoanFolderRuleInfo(string loanFolder)
    {
      return this.iCLImpl.GetLoanFolderRuleInfo(loanFolder);
    }

    public string ProductPricingPartnerSetting(string partnerID, string property)
    {
      return this.iCLImpl.ProductPricingPartnerSetting(partnerID, property);
    }

    public string GetAllLoanFolderDetails() => this.iCLImpl.GetAllLoanFolderDetails();

    public override object InitializeLifetimeService() => (object) null;

    public string ShowLoginScreen(
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "",
      string iconBase64 = "")
    {
      return this.iCLImpl.ShowLoginScreen(clientId, scope, oauthUrl, redirectUrl, iconBase64);
    }

    public string GenerateAuthCodeFromToken(
      string accessToken,
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl)
    {
      return this.iCLImpl.GenerateAuthCodeFromToken(accessToken, clientId, scope, oauthUrl, redirectUrl);
    }
  }
}
