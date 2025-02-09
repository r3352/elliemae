// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.DeferrableLoanTransactionContext
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common;
using Elli.ElliEnum;
using Elli.Service.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  public class DeferrableLoanTransactionContext : IDeferrableTransactionContext, IDisposable
  {
    private AsyncProcessMessage _message;
    private LoanData _priorLoanData;
    private LoanData _afterLoanData;
    private string _targetLoanFolderPath = "";
    private bool _urnInitialized;

    public DeferrableProcessorRole Role { get; private set; }

    public DeferrableDataBag<AsyncProcessMessage> DataBag { get; private set; }

    public UserInfo CurrentUser { get; private set; }

    public Loan CurrentLoan { get; private set; }

    public ILoanSettings CurrenLoanSetting { get; private set; }

    public bool PostProcessorExecuted { get; set; }

    public string AuditUserId { get; set; }

    public DeferrableLoanTransactionContext(
      DeferrableProcessorRole role,
      UserInfo currentUser,
      Loan currentLoan,
      ILoanSettings currentLoanSetting = null,
      string auditUserId = null)
    {
      this.Role = role;
      this.CurrentUser = currentUser;
      this.CurrentLoan = currentLoan;
      this.CurrenLoanSetting = currentLoanSetting;
      this.DataBag = DeferrableDataBag<AsyncProcessMessage>.GetInstance();
      this.AuditUserId = auditUserId;
    }

    public AsyncProcessMessage GetMessage()
    {
      return this._message != null ? this._message : throw new Exception("No message specified");
    }

    public LoanData GetPriorLoanDataPerRole()
    {
      switch (this.Role)
      {
        case DeferrableProcessorRole.Publisher:
          if (this.DataBag.Get<string>("EventId") == "CreateLoan")
            return (LoanData) null;
          return this.DataBag.Get<bool>("UpdateForceRebuild") ? (LoanData) null : this.DataBag.Get<LoanData>("PriorLoanData") ?? this.GetPriorLoanData(this.CurrentLoan.Identity, this.CurrenLoanSetting);
        case DeferrableProcessorRole.Subscriber:
          AsyncProcessMessage message = this.GetMessage();
          return this.GetPriorLoanData(new LoanIdentity(Path.Combine(ClientContext.GetCurrent().Settings.EncompassDataDir, message.LoanPath), message.PriorLoanFileName, message.LoanId, -1, true), this.CurrenLoanSetting);
        case DeferrableProcessorRole.RelaySubscriber:
          LoanData priorLoanDataPerRole = this.DataBag.Get<LoanData>("PriorLoanData");
          if (priorLoanDataPerRole != null)
            return priorLoanDataPerRole;
          string guid = this.DataBag.Get<string>("LoanId");
          string loanName = this.DataBag.Get<string>("_^_Before_loan.em");
          string path2 = this.DataBag.Get<string>("LoanPath");
          return this.GetPriorLoanData(new LoanIdentity(Path.Combine(ClientContext.GetCurrent().Settings.EncompassDataDir, path2), loanName, guid, -1, true), this.CurrenLoanSetting);
        default:
          return (LoanData) null;
      }
    }

    public LoanData GetAfterLoanDataPerRole()
    {
      switch (this.Role)
      {
        case DeferrableProcessorRole.Publisher:
          return this.CurrentLoan.GetLoanData(this.CurrenLoanSetting, false, false);
        case DeferrableProcessorRole.Subscriber:
          AsyncProcessMessage message = this.GetMessage();
          return this.GetAfterLoanData(new LoanIdentity(Path.Combine(ClientContext.GetCurrent().Settings.EncompassDataDir, message.LoanPath), message.AfterLoanFileName, message.LoanId, -1, true), this.CurrenLoanSetting);
        case DeferrableProcessorRole.RelaySubscriber:
          return this.DataBag.Get<LoanData>("CurrentLoanData") ?? this.CurrentLoan.GetLoanData(this.CurrenLoanSetting, false, false);
        default:
          return (LoanData) null;
      }
    }

    public LoanData GetPriorLoanData(LoanIdentity identity, ILoanSettings loanSettings = null)
    {
      if (this._priorLoanData != null)
        return this._priorLoanData;
      this._priorLoanData = this.CurrentLoan.InternalDeserializeLoanData(identity, loanSettings, true);
      return this._priorLoanData;
    }

    public LoanData GetAfterLoanData(LoanIdentity identity, ILoanSettings loanSettings = null)
    {
      if (this._afterLoanData != null)
        return this._afterLoanData;
      this._afterLoanData = this.CurrentLoan.InternalDeserializeLoanData(identity, loanSettings, true);
      return this._afterLoanData;
    }

    public string GetTargetLoanFolderPath()
    {
      if (!string.IsNullOrWhiteSpace(this._targetLoanFolderPath))
        return this._targetLoanFolderPath;
      this._targetLoanFolderPath = Path.Combine("DeferredLoans", Guid.NewGuid().ToString());
      return this._targetLoanFolderPath;
    }

    public void Dispose() => this._message = (AsyncProcessMessage) null;

    public string GetLoanFilePath()
    {
      return this.CurrentLoan == null || (LoanIdentity) null == this.CurrentLoan.Identity ? (string) null : new LoanFolder(this.CurrentLoan.Identity.LoanFolder).GetFullLoanFilePath(this.CurrentLoan.Identity.LoanName);
    }

    public DeferrableLoanTransactionContext SetMessage(IMessage message)
    {
      this._message = message as AsyncProcessMessage;
      this.AuditUserId = message.AuditUserId;
      return this;
    }

    public DeferrableLoanTransactionContext SetUrn(
      string applicationId,
      string serviceId,
      string instanceId,
      string siteId,
      string eventId,
      string userId)
    {
      this.DataBag.Set("ApplicationId", (object) applicationId).Set("ServiceId", (object) serviceId).Set("InstanceId", (object) instanceId).Set("SiteId", (object) siteId).Set("EventId", (object) eventId).Set("UserId", (object) userId);
      this._urnInitialized = true;
      return this;
    }

    public DeferrableLoanTransactionContext SetServerMode(EncompassServerMode mode)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("ServerMode", (object) mode);
      return this;
    }

    public DeferrableLoanTransactionContext SetLoanId(string id)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("LoanId", (object) id);
      return this;
    }

    public DeferrableLoanTransactionContext SetXDBModifiedTime(DateTime date)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("XDBModifiedTime", (object) date);
      return this;
    }

    public DeferrableLoanTransactionContext SetAuditModifiedTime(DateTime date)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("AuditModifiedTime", (object) date);
      return this;
    }

    public DeferrableLoanTransactionContext SetAuditCurrentTime(DateTime date)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("AuditCurrentTime", (object) date);
      return this;
    }

    public DeferrableLoanTransactionContext SetUpdateForceRebuild(bool flag)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("UpdateForceRebuild", (object) flag);
      return this;
    }

    public DeferrableLoanTransactionContext SetPriorLoanData(LoanData loanData)
    {
      this.DataBag.Set("PriorLoanData", (object) loanData);
      return this;
    }

    public DeferrableLoanTransactionContext SetCurrentLoanData(LoanData loanData)
    {
      this.DataBag.Set("CurrentLoanData", (object) loanData);
      return this;
    }

    public DeferrableLoanTransactionContext SetLoanFolder(string path)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("LoanFolder", (object) path);
      return this;
    }

    public DeferrableLoanTransactionContext SetLoanPath(string path)
    {
      if (!this._urnInitialized)
        throw new Exception("Urn has not been initialized.");
      this.DataBag.Set("LoanPath", (object) path);
      return this;
    }
  }
}
