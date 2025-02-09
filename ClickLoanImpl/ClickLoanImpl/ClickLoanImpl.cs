// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl
// Assembly: ClickLoanImpl, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 9549E162-7E74-49E9-BCDA-CB0A69B5F0B5
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClickLoanImpl.dll

using Elli.Web.Host.Login;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.Export;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClickLoanImpl
{
  public class ClickLoanImpl
  {
    private const string className = "ClickLoanImpl";
    private static readonly string sw = Tracing.SwClickLoanProxy;
    private string serverDateTime;
    private LoginContext loginContext;
    private EllieMae.EMLite.ePass.Bam _bam;

    public bool InsideEncompass()
    {
      return Assembly.GetEntryAssembly().FullName.IndexOf("Encompass,") == 0;
    }

    public string GetUserid()
    {
      return !Session.IsConnected || Session.UserID == null ? "" : Session.UserID;
    }

    public string GetPassword() => Session.Password;

    public string GetEncompassServer() => Session.RemoteServer == null ? "" : Session.RemoteServer;

    public int LoginWithAuthCode(string server, string userid, string authCode)
    {
      if (Session.IsConnected)
        return 11;
      try
      {
        Session.Start(server, userid, "", "ClickLoan", false, (string) null, authCode);
      }
      catch (LoginException ex)
      {
        return (int) ex.LoginReturnCode;
      }
      catch (VersionMismatchException ex)
      {
        if (ex.IsVersionUpdateAvailable() && VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          return 101;
        int num = (int) MessageBox.Show("Encompass client and server are incompatible.  ClickLoan cannot continue.", "Version Incompatible", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -101;
      }
      catch (SocketException ex)
      {
        switch (ex.ErrorCode)
        {
          case 10061:
            return -3;
          case 11001:
            return -2;
          default:
            return -1;
        }
      }
      catch (Exception ex)
      {
        return -1;
      }
      try
      {
        this.serverDateTime = Session.ServerTime.ToString();
        Tracing.Log(true, "Init", nameof (ClickLoanImpl), "Server time is " + this.serverDateTime);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Cannot get server date time.  " + ex.Message);
      }
      return 0;
    }

    public int Login(string server, string userid, string password)
    {
      if (Session.IsConnected)
        return 11;
      try
      {
        if (server == null || server == string.Empty || "(local)".Equals(server))
          Session.Start(userid, password, "ClickLoan", false);
        else
          Session.Start(server, userid, password, "ClickLoan", false);
      }
      catch (LoginException ex)
      {
        return (int) ex.LoginReturnCode;
      }
      catch (VersionMismatchException ex)
      {
        if (ex.IsVersionUpdateAvailable() && VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          return 101;
        int num = (int) MessageBox.Show("Encompass client and server are incompatible.  ClickLoan cannot continue.", "Version Incompatible", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return -101;
      }
      catch (SocketException ex)
      {
        switch (ex.ErrorCode)
        {
          case 10061:
            return -3;
          case 11001:
            return -2;
          default:
            return -1;
        }
      }
      catch (Exception ex)
      {
        return -1;
      }
      try
      {
        this.serverDateTime = Session.ServerTime.ToString();
        Tracing.Log(true, "Init", nameof (ClickLoanImpl), "Server time is " + this.serverDateTime);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Cannot get server date time.  " + ex.Message);
      }
      return 0;
    }

    public string GetFolderList()
    {
      string[] allLoanFolderNames = Session.LoanManager.GetAllLoanFolderNames(false);
      if (allLoanFolderNames == null)
        return "";
      string folderList = "";
      for (int index = 0; index < allLoanFolderNames.Length; ++index)
      {
        folderList += allLoanFolderNames[index];
        if (index < allLoanFolderNames.Length - 1)
          folderList += "\n";
      }
      return folderList;
    }

    public string GetWorkingFolder()
    {
      return Session.UserInfo.WorkingFolder == null ? "" : Session.UserInfo.WorkingFolder;
    }

    public string GetWorkingLoanName()
    {
      return Session.LoanDataMgr == null || Session.LoanDataMgr.LoanName == null ? "" : Session.LoanDataMgr.LoanName;
    }

    public string GetLoanList(string loanFolder)
    {
      Session.WorkingFolder = loanFolder;
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(loanFolder, new string[9]
      {
        "LoanFolder",
        "LoanName",
        "LoanNumber",
        "BorrowerLastName",
        "BorrowerFirstName",
        "Address1",
        "LoanOfficerName",
        "LoanProcessorName",
        "CurrentMilestoneName"
      }, PipelineData.Fields, false);
      ClpXml clpXml = new ClpXml("LoanList");
      for (int index = 0; index < pipeline.Length; ++index)
      {
        XmlElement elm = clpXml.AddChildElement(clpXml.Root, "Loan");
        clpXml.AddAttribute(elm, "LoanFolder", pipeline[index].LoanFolder);
        clpXml.AddAttribute(elm, "LoanName", pipeline[index].LoanName);
        clpXml.AddAttribute(elm, "LoanNumber", pipeline[index].LoanNumber);
        clpXml.AddAttribute(elm, "BorrowerLastName", pipeline[index].LastName);
        clpXml.AddAttribute(elm, "BorrowerFirstName", pipeline[index].FirstName);
        clpXml.AddAttribute(elm, "PropAddress", (string) pipeline[index].Info[(object) "Address1"]);
        clpXml.AddAttribute(elm, "LoanOfficer", (string) pipeline[index].Info[(object) "LoanOfficerName"]);
        clpXml.AddAttribute(elm, "LoanProcessor", (string) pipeline[index].Info[(object) "LoanProcessorName"]);
        string val = (string) pipeline[index].Info[(object) "CurrentMilestoneName"];
        clpXml.AddAttribute(elm, "CurrentMilestone", val);
        clpXml.AddAttribute(elm, "GUID", pipeline[index].GUID);
      }
      return clpXml.ToString();
    }

    public int LoadLoanFile(string loanFolder, string loanName)
    {
      LoanDataMgr val = LoanDataMgr.OpenLoan(Session.SessionObjects, loanFolder, loanName, false);
      if (Session.LoanDataMgr != null && Session.LoanDataMgr.LoanData != null)
      {
        if (Session.LoanDataMgr.LoanData.GUID == val.LoanData.GUID)
          return 0;
        if (Session.LoanDataMgr.Writable)
          return 1;
      }
      Session.SetLoanDataMgr(val);
      return 0;
    }

    public string GetField(string id)
    {
      return Session.LoanDataMgr == null || Session.LoanData == null ? "" : Session.LoanData.GetField(id);
    }

    public int ValidateData(string format, bool allowContinue)
    {
      return new ExportData(Session.LoanDataMgr, Session.LoanData).Validate(format, allowContinue) ? 0 : 1;
    }

    public int CreateExportFile(string format, string filePath)
    {
      try
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "CreateExportFile() filePath = " + filePath);
        string str = new ExportData(Session.LoanDataMgr, Session.LoanData).Export(format);
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "CreateExportFile(): export result = " + str);
        if (str == string.Empty)
          return 1;
        StreamWriter streamWriter = new StreamWriter(filePath, false);
        streamWriter.Write(str);
        streamWriter.Flush();
        streamWriter.Close();
        return 0;
      }
      catch (ArgumentException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message + ". Please modify this field and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return 1;
      }
    }

    public string GetClientID()
    {
      return Session.CompanyInfo.ClientID == null ? "" : Session.CompanyInfo.ClientID;
    }

    public int Logout()
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Logout(): end session");
      this.CloseLoan();
      Session.End();
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Logout(): session ended");
      WebLoginUtil.StopBrowserEngine(nameof (ClickLoanImpl));
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Logout(): browser engine stopped");
      return 0;
    }

    public int CheckAndInstallHotfix() => VersionControl.ApplyAvailableHotfixes() ? 1 : 0;

    public string[] GetOffers(string companyID) => new string[0];

    public string GetOfferCompanyProgram(string offerID) => (string) null;

    public int OpenLoan(string loanFolder, string loanName)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to open loan '" + loanFolder + "/" + loanName + "'");
      LoanDataMgr loanDataMgr;
      try
      {
        loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, loanFolder, loanName, false);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Cannot open the loan '" + loanFolder + "/" + loanName + "': " + ex.Message);
        return -3;
      }
      if (loanDataMgr == null)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "LoanDataMgr.OpenLoan() returns null");
        return -10;
      }
      if (Session.LoanDataMgr != null && Session.LoanDataMgr.LoanData != null)
      {
        if (Session.LoanDataMgr.LoanData.GUID == loanDataMgr.LoanData.GUID)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Info, "The loan '" + loanFolder + "/" + loanName + "' has already been opened");
          return 1;
        }
        if (Session.LoanDataMgr.Writable)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, "A different loan '" + Session.LoanDataMgr.LoanData.GUID + "' has already been opened");
          return -1;
        }
      }
      try
      {
        string guid = loanDataMgr.LoanData.GUID;
        PipelineInfo pipelineInfo = Session.LoanManager.GetPipeline(new string[1]
        {
          guid
        }, false)[0];
        if (pipelineInfo == null)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, guid + ": the selected loan has been deleted or is no longer accessible.");
          return -11;
        }
        LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) Session.BPM.GetBpmManager(BpmCategory.LoanAccess);
        LoanInfo.Right effectiveRightsForLoan = bpmManager.GetEffectiveRightsForLoan(pipelineInfo);
        LoanContentAccess loanContentAccess = bpmManager.GetLoanContentAccess(pipelineInfo, loanDataMgr.LoanData);
        if (loanDataMgr.AccessRules.AllowFullAccess())
          loanContentAccess = LoanContentAccess.FullAccess;
        loanDataMgr.LoanData.ContentAccess = loanContentAccess;
        if (effectiveRightsForLoan == LoanInfo.Right.NoRight)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, guid + ": user no longer has the necessary rights to access this loan file.");
          return 11;
        }
        if (effectiveRightsForLoan == LoanInfo.Right.Read)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, guid + ": user only has read-only access to this loan file.");
          return 12;
        }
        if (Session.LoanManager.GetLoanFolder(loanFolder).Type == LoanFolderInfo.LoanFolderType.Trash)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, guid + ": the loan is currently in a Trash folder. User won't be able to save any changes.");
          return 13;
        }
        if (new eFolderAccessRights(loanDataMgr).CanAddDocuments)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, guid + ": user has no right to access document tracking.");
          return 14;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Access check error: " + ex.Message);
        return -12;
      }
      try
      {
        loanDataMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Nonexclusive);
      }
      catch (Exception ex)
      {
        string msg = "Unable to get exclusive lock for loan '" + loanFolder + "/" + loanName + "': " + ex.Message;
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, msg);
        return -2;
      }
      try
      {
        Session.SetLoanDataMgr(loanDataMgr);
      }
      catch (Exception ex)
      {
        loanDataMgr.Unlock(true);
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Unable to set Session LoanDataMgr: " + ex.Message);
        return -4;
      }
      return 0;
    }

    public int SaveLoan()
    {
      if (Session.LoanDataMgr == null)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, "LoanDataMgr is null");
        return 1;
      }
      if (Session.LoanDataMgr.LoanData != null)
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to save loan '" + Session.LoanDataMgr.LoanData.GUID + "'");
      try
      {
        bool loanFileMerged = false;
        Session.LoanDataMgr.SaveLoan(false, false, true, (ILoanMilestoneTemplateOrchestrator) null, false, out loanFileMerged);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error trying to save loan '" + Session.LoanDataMgr.LoanData.GUID + "': " + ex.Message);
        return -1;
      }
      return 0;
    }

    public int CloseLoan()
    {
      if (Session.LoanDataMgr == null)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Warning, "LoanDataMgr is null");
        return 1;
      }
      if (Session.LoanDataMgr.LoanData != null)
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to close loan '" + Session.LoanDataMgr.LoanData.GUID + "'");
      Session.LoanDataMgr.Close();
      Session.SetLoanDataMgr((LoanDataMgr) null);
      return 0;
    }

    public bool CanAddDoc
    {
      get
      {
        return ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_eFolder_DT_NewEditFiles);
      }
    }

    public int AddeFolderAttachment(byte[] fileContent, string fileFormat, string fileDesc)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to add attachment '" + fileDesc + "'");
      try
      {
        if (!this.CanAddDoc)
          throw new Exception("Cannot add doc");
        using (BinaryObject data = new BinaryObject(fileContent))
          Session.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, data, fileFormat, fileDesc, (DocumentLog) null);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
      return 0;
    }

    public int AddeFolderAttachment(string fileName, string fileFormat, string fileDesc)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to add attachment '" + fileName + "/" + fileDesc + "'");
      try
      {
        if (!this.CanAddDoc)
          throw new Exception("Cannot add doc");
        using (BinaryObject data = new BinaryObject(fileName))
          Session.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, data, fileFormat, fileDesc, (DocumentLog) null);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
      return 0;
    }

    private EllieMae.EMLite.ePass.Bam bam
    {
      get
      {
        if (Session.LoanDataMgr == null || Session.LoanData == null)
          return (EllieMae.EMLite.ePass.Bam) null;
        if (this._bam != null && (this._bam.LoanDataMgr != Session.LoanDataMgr || this._bam.LoanData.GUID != Session.LoanData.GUID))
          this._bam = (EllieMae.EMLite.ePass.Bam) null;
        if (this._bam == null)
          this._bam = new EllieMae.EMLite.ePass.Bam();
        return this._bam;
      }
    }

    public string[] GetDocumentList()
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to get document list.");
      try
      {
        return this.bam.GetDocumentList();
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        return (string[]) null;
      }
    }

    public string GetDocumentListXml()
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to get document list Xml.");
      try
      {
        return this.bam.GetDocumentListXml();
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error to get document list Xml: " + ex.Message);
        return (string) null;
      }
    }

    public string GetDocumentTitle(string docID)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to get document title.");
      try
      {
        return this.bam.GetDocumentTitle(docID);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Trying to get document title: " + ex.Message);
        return (string) null;
      }
    }

    public string GetDocumentCompany(string docID)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to get document company.");
      try
      {
        return this.bam.GetDocumentCompany(docID);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Trying to get document company: " + ex.Message);
        return (string) null;
      }
    }

    public string GetDocumentPairID(string docID)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to get document pair DI.");
      try
      {
        return this.bam.GetDocumentPairID(docID);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Trying to get document pair ID: " + ex.Message);
        return (string) null;
      }
    }

    public bool CanAddAttachment(string docID)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Checking if user can add attachment.");
      try
      {
        return this.bam.CanAddAttachment(docID);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error checking if user can add attachment: " + ex.Message);
        throw ex;
      }
    }

    public string AddAttachment(string filepath, string title, string docID)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to add attachment '" + filepath + "/" + title + "/" + docID + "'");
      try
      {
        if (!this.CanAddAttachment(docID))
          return (string) null;
        DocumentLog documentLog = this.getDocumentLog(docID);
        return this.bam.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, filepath, title, documentLog).ID;
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error adding attachment: " + ex.Message);
        throw ex;
      }
    }

    public string AddAttachment(string[] imageFiles, string title, string docID)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to add attachment from array '" + title + "/" + docID + "'");
      try
      {
        if (!this.CanAddAttachment(docID))
          return (string) null;
        DocumentLog documentLog = this.getDocumentLog(docID);
        if (documentLog == null)
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "ClickLoanImpl.cs: AddAttachment(string[], string, string): doc is null");
        if (this.bam.LoanDataMgr.FileAttachments == null)
        {
          Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "ClickLoanImpl.cs: AddAttachment(string[], string, string): bam.LoanDataMgr.FileAttachments is null");
          return (string) null;
        }
        FileAttachment fileAttachment = this.bam.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, imageFiles, title, documentLog);
        if (fileAttachment != null)
          return fileAttachment.ID;
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "ClickLoanImpl.cs: AddAttachment(string[], string, string): attachment is null");
        return (string) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error adding attachment: " + (object) ex);
        throw ex;
      }
    }

    public bool SupportsImageAttachments()
    {
      return !this.bam.LoanDataMgr.UseSkyDrive && this.bam.LoanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
    }

    private DocumentLog getDocumentLog(string docID)
    {
      LogRecordBase recordById = this.bam.LoanData.GetLogList().GetRecordByID(docID);
      return recordById is DocumentLog ? (DocumentLog) recordById : (DocumentLog) null;
    }

    public string NewLoan(string pipeline, string loanName)
    {
      Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Verbose, "Trying to create a new loan.");
      pipeline = pipeline ?? "";
      loanName = loanName ?? "";
      LoanDataMgr val = LoanDataMgr.NewLoan(Session.SessionObjects, pipeline, loanName);
      Session.SetLoanDataMgr(val);
      return val.LoanData.GUID.ToString();
    }

    public void SetField(string id, string val) => Session.LoanDataMgr.LoanData.SetField(id, val);

    public string GetVersion()
    {
      return VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition);
    }

    public string GetLoanFolderRuleInfo(string loanFolder)
    {
      LoanFolderRuleInfo rule = ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetRule(loanFolder);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<LoanFolderRuleInfo> <CanImportLoans value=\"\"/> </LoanFolderRuleInfo>");
      xmlDocument.DocumentElement.SelectSingleNode("CanImportLoans").Attributes["value"].Value = Convert.ToString(rule.CanImportLoans);
      return xmlDocument.OuterXml;
    }

    public string ProductPricingPartnerSetting(string partnerID, string property)
    {
      ProductPricingSetting productPricingPartner = Session.ConfigurationManager.GetProductPricingPartner(partnerID);
      if (productPricingPartner == null)
        return "";
      switch (property)
      {
        case "AdminURL":
          return productPricingPartner.AdminURL;
        case "AllowImportToLoan":
          return !productPricingPartner.ImportToLoan ? "n" : "y";
        case "AllowLockAndConfirm":
          return !productPricingPartner.PartnerLockConfirm ? "n" : "y";
        case "AllowRequestLock":
          return !productPricingPartner.PartnerRequestLock ? "n" : "y";
        case "IsCustomizeInvestorName":
          return !productPricingPartner.IsCustomizeInvestorName ? "n" : "y";
        case "MoreInfoURL":
          return productPricingPartner.MoreInfoURL;
        case "PartnerName":
          return productPricingPartner.PartnerName;
        case "SettingsSection":
          return productPricingPartner.SettingsSection;
        case "ShowSellSide":
          return !productPricingPartner.ShowSellSide ? "n" : "y";
        case "UseCustomizedInvestorName":
          return !productPricingPartner.UseCustomizedInvestorName ? "n" : "y";
        case "UseInvestorAndLenderName":
          return !productPricingPartner.UseInvestorAndLenderName ? "n" : "y";
        case "UseOnlyInvestorName":
          return !productPricingPartner.UseOnlyInvestorName ? "n" : "y";
        case "UseOnlyLenderName":
          return !productPricingPartner.UseOnlyLenderName ? "n" : "y";
        default:
          return "";
      }
    }

    public string GetAllLoanFolderDetails()
    {
      LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(false);
      LoanFolderRuleManager bpmManager = (LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<LoanFolderDetails></LoanFolderDetails>");
      foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
      {
        XmlElement element = xmlDocument.CreateElement("LoanFolderDetail");
        element.SetAttribute("Name", loanFolderInfo.Name ?? "");
        element.SetAttribute("DisplayName", loanFolderInfo.DisplayName ?? "");
        string str;
        switch (loanFolderInfo.Type)
        {
          case LoanFolderInfo.LoanFolderType.NotSpecified:
            str = "NotSpecified";
            break;
          case LoanFolderInfo.LoanFolderType.Regular:
            str = "Regular";
            break;
          case LoanFolderInfo.LoanFolderType.Trash:
            str = "Trash";
            break;
          case LoanFolderInfo.LoanFolderType.Archive:
            str = "Archive";
            break;
          default:
            str = "";
            break;
        }
        element.SetAttribute("Type", str);
        LoanFolderRuleInfo rule = bpmManager.GetRule(loanFolderInfo.Name);
        element.SetAttribute("Rule_CanImportLoans", rule.CanImportLoans ? "True" : "False");
        xmlDocument.DocumentElement.AppendChild((XmlNode) element);
      }
      return xmlDocument.OuterXml;
    }

    public string ShowLoginScreen(
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl = "https://encompass.elliemae.com/homepage/atest.asp",
      string iconBase64 = "")
    {
      LoginContextResponse loginContextResponse = (LoginContextResponse) null;
      this.loginContext = (LoginContext) null;
      DialogResult dialogResult = DialogResult.None;
      try
      {
        Form form = WebLoginUtil.IsChromiumForWebLoginEnabled ? (Form) new WebLoginUtil().GetLoginForm(AppName.ClickLoan, "", false, new Func<LoginContext, bool>(this.SetLoginContext), "", appClientId: clientId, appScope: scope, redirectUrl: redirectUrl) : (Form) new LoginUtil().GetLoginForm(AppName.ClickLoan, "", false, new Func<LoginContext, bool>(this.SetLoginContext), "", appClientId: clientId, appScope: scope, redirectUrl: redirectUrl);
        if (form != null)
        {
          this.SetFormIcon(form, iconBase64);
          dialogResult = form.ShowDialog();
        }
        if (this.loginContext != null && dialogResult == DialogResult.OK)
          loginContextResponse = new OauthServiceClient().GenerateToken(this.loginContext.AuthCode, clientId, scope, oauthUrl, redirectUrl);
        if (loginContextResponse == null)
          return (string) null;
        loginContextResponse.instanceID = this.loginContext.InstanceID;
        loginContextResponse.server = this.loginContext.Server;
        loginContextResponse.userId = this.loginContext.UserId;
        return JsonConvert.SerializeObject((object) loginContextResponse);
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error in showLogin method: " + ex.Message);
        throw ex;
      }
    }

    public string GenerateAuthCodeFromToken(
      string accessToken,
      string appClientId,
      string scope,
      string oauthUrl,
      string redirectUrl)
    {
      try
      {
        return new OauthServiceClient().GenerateAuthCodeFromToken(accessToken, appClientId, scope, oauthUrl, redirectUrl).authorization_code;
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error in GenerateAuthCodeFromToken method: " + ex.Message);
        throw ex;
      }
    }

    public bool SetLoginContext(LoginContext context)
    {
      if (context == null)
        return false;
      this.loginContext = context;
      return true;
    }

    private void SetFormIcon(Form form, string iconBase64)
    {
      try
      {
        if (string.IsNullOrEmpty(iconBase64))
          return;
        using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(iconBase64)))
        {
          memoryStream.Position = 0L;
          form.Icon = new Icon((Stream) memoryStream);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.ClickLoanImpl.ClickLoanImpl.sw, nameof (ClickLoanImpl), TraceLevel.Error, "Error while setting form icon: " + ex.Message);
      }
    }
  }
}
