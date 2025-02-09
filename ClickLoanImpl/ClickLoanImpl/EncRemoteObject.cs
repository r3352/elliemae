// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanImpl.EncRemoteObject
// Assembly: ClickLoanImpl, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 9549E162-7E74-49E9-BCDA-CB0A69B5F0B5
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClickLoanImpl.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Runtime.Remoting.Lifetime;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClickLoanImpl
{
  public class EncRemoteObject : MarshalByRefObject
  {
    private string completeServerUriPath;
    private string userid;
    private string password;
    private string loanGuid;
    private Bam _bam;

    public int Login(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      if (this.userid != null && (this.completeServerUriPath == null || completeServerUriPath == null || string.Compare(this.completeServerUriPath, completeServerUriPath, true) == 0) && string.Compare(this.userid, userid, true) == 0 && string.Compare(this.password ?? "", password ?? "", true) == 0 && this.checkLoanGuid(loanGuid))
        return 0;
      int num = this.login(completeServerUriPath, userid, password);
      if (num != 0)
        return num;
      if (!this.checkLoanGuid(loanGuid))
        num = 999;
      if (num == 0)
      {
        this.completeServerUriPath = completeServerUriPath;
        this.userid = userid;
        this.password = password;
        this.loanGuid = loanGuid;
      }
      return num;
    }

    private bool checkLoanGuid(string loanGuid)
    {
      if (loanGuid == null)
        return true;
      if (Session.DefaultInstance == null || Session.DefaultInstance.LoanDataMgr == null || Session.DefaultInstance.LoanData == null)
        return false;
      string guid = Session.DefaultInstance.LoanData.GUID;
      return string.Compare(loanGuid, guid ?? "", true) == 0;
    }

    private int login(string completeServerUriPath, string userid, string password)
    {
      if (Session.DefaultInstance == null)
        return -1;
      if (completeServerUriPath != null)
      {
        if (!(completeServerUriPath.Trim().ToLower() == "(local)"))
        {
          if (!(completeServerUriPath.Trim() == ""))
          {
            try
            {
              if (Session.DefaultInstance.ServerIdentity == null)
                return 1;
              ServerIdentity serverIdentity = ServerIdentity.Parse(completeServerUriPath);
              if (Session.DefaultInstance.ServerIdentity.InstanceName.ToLower() != serverIdentity.InstanceName.ToLower())
                return 1;
              goto label_12;
            }
            catch (Exception ex)
            {
              Tracing.Log(true, "ERROR", "EncRemoteObj", "Error checking servers: " + ex.Message);
              return 100;
            }
          }
        }
        if (Session.DefaultInstance.ServerIdentity != null)
          return 1;
      }
label_12:
      return (Session.DefaultInstance.UserID ?? "").Trim().ToLower() != (userid ?? "").Trim().ToLower() ? 2 : 0;
    }

    public string GetOpenLoanGuid(string completeServerUriPath, string userid, string password)
    {
      if (this.Login(completeServerUriPath, userid, password, (string) null) != 0)
        return (string) null;
      return Session.DefaultInstance == null || Session.DefaultInstance.LoanData == null ? (string) null : Session.DefaultInstance.LoanData.GUID;
    }

    public string GetOpenLoanInfo(string completeServerUriPath, string userid, string password)
    {
      string openLoanGuid = this.GetOpenLoanGuid(completeServerUriPath, userid, password);
      if (openLoanGuid == null)
        return (string) null;
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(new string[1]
      {
        openLoanGuid
      }, new string[9]
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
      ClpXml clpXml = new ClpXml("LoanInfo");
      XmlElement elm = clpXml.AddChildElement(clpXml.Root, "Loan");
      clpXml.AddAttribute(elm, "LoanFolder", pipeline[0].LoanFolder);
      clpXml.AddAttribute(elm, "LoanName", pipeline[0].LoanName);
      clpXml.AddAttribute(elm, "LoanNumber", pipeline[0].LoanNumber);
      clpXml.AddAttribute(elm, "BorrowerLastName", pipeline[0].LastName);
      clpXml.AddAttribute(elm, "BorrowerFirstName", pipeline[0].FirstName);
      clpXml.AddAttribute(elm, "PropAddress", (string) pipeline[0].Info[(object) "Address1"]);
      clpXml.AddAttribute(elm, "LoanOfficer", (string) pipeline[0].Info[(object) "LoanOfficerName"]);
      clpXml.AddAttribute(elm, "LoanProcessor", (string) pipeline[0].Info[(object) "LoanProcessorName"]);
      string val = (string) pipeline[0].Info[(object) "CurrentMilestoneName"];
      clpXml.AddAttribute(elm, "CurrentMilestone", val);
      clpXml.AddAttribute(elm, "GUID", pipeline[0].GUID);
      return clpXml.ToString();
    }

    public int AddeFolderAttachment(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      byte[] fileContent,
      string fileFormat,
      string fileDesc)
    {
      switch (this.Login(completeServerUriPath, userid, password, loanGuid))
      {
        case 0:
          using (BinaryObject data = new BinaryObject(fileContent))
            Session.DefaultInstance.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, data, AppSecurity.EncodeFileExtension(fileFormat), fileDesc, (DocumentLog) null);
          return 0;
        case 999:
          throw new Exception("Loan '" + loanGuid + "' is not currently opened");
        default:
          throw new Exception("Error login to " + completeServerUriPath);
      }
    }

    public int AddeFolderAttachment(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string fileName,
      string fileFormat,
      string fileDesc)
    {
      switch (this.Login(completeServerUriPath, userid, password, loanGuid))
      {
        case 0:
          using (BinaryObject data = new BinaryObject(fileName))
            Session.DefaultInstance.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, data, AppSecurity.EncodeFileExtension(fileFormat), fileDesc, (DocumentLog) null);
          return 0;
        case 999:
          throw new Exception("Loan '" + loanGuid + "' is not currently opened");
        default:
          throw new Exception("Error login to " + completeServerUriPath);
      }
    }

    public object GetAclManager(
      string completeServerUriPath,
      string userid,
      string password,
      AclCategory category)
    {
      if (this.Login(completeServerUriPath, userid, password, (string) null) != 0)
        return (object) null;
      return Session.DefaultInstance == null || Session.DefaultInstance.ACL == null ? (object) null : Session.DefaultInstance.ACL.GetAclManager(category);
    }

    private Bam bam
    {
      get
      {
        if (Session.DefaultInstance.LoanDataMgr == null || Session.DefaultInstance.LoanData == null)
          return (Bam) null;
        if (this._bam != null && (this._bam.LoanDataMgr != Session.DefaultInstance.LoanDataMgr || this._bam.LoanData.GUID != Session.DefaultInstance.LoanData.GUID))
          this._bam = (Bam) null;
        if (this._bam == null)
          this._bam = new Bam((IBrowser) null, Session.DefaultInstance.LoanDataMgr, Session.DefaultInstance.LoanData);
        return this._bam;
      }
    }

    public string[] GetDocumentList(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      return this.Login(completeServerUriPath, userid, password, loanGuid) != 0 ? (string[]) null : this.bam.GetDocumentList();
    }

    public string GetDocumentListXml(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      return this.Login(completeServerUriPath, userid, password, loanGuid) != 0 ? (string) null : this.bam.GetDocumentListXml();
    }

    public string GetDocumentTitle(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string docID)
    {
      return this.Login(completeServerUriPath, userid, password, loanGuid) != 0 ? (string) null : this.bam.GetDocumentTitle(docID);
    }

    public string GetDocumentCompany(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string docID)
    {
      return this.Login(completeServerUriPath, userid, password, loanGuid) != 0 ? (string) null : this.bam.GetDocumentCompany(docID);
    }

    public string GetDocumentPairID(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string docID)
    {
      return this.Login(completeServerUriPath, userid, password, loanGuid) != 0 ? (string) null : this.bam.GetDocumentPairID(docID);
    }

    public bool CanAddAttachment(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string docID)
    {
      switch (this.Login(completeServerUriPath, userid, password, loanGuid))
      {
        case 0:
          return this.bam.CanAddAttachment(docID);
        case 999:
          throw new Exception("Loan '" + loanGuid + "' is not currently opened");
        default:
          throw new Exception("Error login to " + completeServerUriPath);
      }
    }

    public string AddAttachment(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string filepath,
      string title,
      string docID)
    {
      if (this.Login(completeServerUriPath, userid, password, loanGuid) != 0)
        return (string) null;
      if (!this.bam.CanAddAttachment(docID))
        return (string) null;
      DocumentLog documentLog = this.getDocumentLog(docID);
      return this.bam.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, filepath, title, documentLog).ID;
    }

    public bool SupportsImageAttachments(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid)
    {
      switch (this.Login(completeServerUriPath, userid, password, loanGuid))
      {
        case 0:
          return !this.bam.LoanDataMgr.UseSkyDrive && this.bam.LoanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
        case 999:
          throw new Exception("Loan '" + loanGuid + "' is not currently opened");
        default:
          throw new Exception("Error login to " + completeServerUriPath);
      }
    }

    public string AddAttachment(
      string completeServerUriPath,
      string userid,
      string password,
      string loanGuid,
      string[] imageFiles,
      string title,
      string docID)
    {
      if (this.Login(completeServerUriPath, userid, password, loanGuid) != 0)
        return (string) null;
      if (!this.bam.CanAddAttachment(docID))
        return (string) null;
      DocumentLog documentLog = this.getDocumentLog(docID);
      return this.bam.LoanDataMgr.FileAttachments.AddAttachment(AddReasonType.Printer, imageFiles, title, documentLog).ID;
    }

    private DocumentLog getDocumentLog(string docID)
    {
      LogRecordBase recordById = this.bam.LoanData.GetLogList().GetRecordByID(docID);
      return recordById is DocumentLog ? (DocumentLog) recordById : (DocumentLog) null;
    }

    public override object InitializeLifetimeService()
    {
      ILease lease = (ILease) base.InitializeLifetimeService();
      if (lease.CurrentState == LeaseState.Initial)
      {
        lease.InitialLeaseTime = TimeSpan.FromMinutes(20.0);
        lease.RenewOnCallTime = TimeSpan.FromMinutes(20.0);
        lease.SponsorshipTimeout = TimeSpan.FromSeconds(30.0);
      }
      return (object) lease;
    }
  }
}
