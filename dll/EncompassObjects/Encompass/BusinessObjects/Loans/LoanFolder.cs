// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFolder
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanFolder : SessionBoundObject, ILoanFolder
  {
    private ILoanManager mngr;
    private string folderName;
    private LoanFolderInfo info;
    private int size = -1;
    private bool canAccessArchiveLoans;
    private bool loanSoftArchivalenabled;

    internal LoanFolder(Session session, string folderName)
      : base(session)
    {
      this.mngr = session.Loans.LoanManager;
      this.folderName = folderName;
      this.canAccessArchiveLoans = this.Session.GetUserInfo().IsAdministrator() || (bool) session.SessionObjects.StartupInfo.UserAclFeatureRights[(object) (AclFeature) 233];
      this.loanSoftArchivalenabled = this.Session.SessionObjects.StartupInfo.EnableLoanSoftArchival;
    }

    public string Name => this.folderName;

    public string DisplayName
    {
      get
      {
        this.ensureLoaded();
        return this.info.DisplayName;
      }
    }

    public bool IsArchive
    {
      get
      {
        this.ensureLoaded();
        return this.info.Type == 2;
      }
    }

    public bool IsTrash
    {
      get
      {
        this.ensureLoaded();
        return this.info.Type == 1;
      }
    }

    public int Size
    {
      get
      {
        if (this.size == -1)
          this.size = this.mngr.GetLoanFolderPhysicalSize(this.folderName);
        return this.size;
      }
    }

    public LoanIdentityList GetContents()
    {
      return LoanIdentity.ToList(this.mngr.GetLoanFolderContents(this.folderName, (LoanInfo.Right) 1, false));
    }

    public PipelineCursor OpenPipeline(PipelineSortOrder sortOrder)
    {
      return this.OpenPipeline(sortOrder, true);
    }

    public PipelineCursor OpenPipeline(PipelineSortOrder sortOrder, bool excludeArchivedLoans = true)
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      });
      if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans || this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && this.IsArchive)
        throw new Exception("User doesn't have access to archive loans.");
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline(this.folderName, (LoanInfo.Right) 1, (string[]) null, (PipelineData) 1023, (PipelineSortOrder) sortOrder, (QueryCriterion) null, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
    }

    public PipelineCursor QueryPipeline(QueryCriterion criterion, PipelineSortOrder sortOrder)
    {
      return this.QueryPipeline(criterion, sortOrder, true);
    }

    public PipelineCursor QueryPipeline(
      QueryCriterion criterion,
      PipelineSortOrder sortOrder,
      bool excludeArchivedLoans = true)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans || this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && this.IsArchive)
        throw new Exception("User doesn't have access to archive loans.");
      ClientMetricsProviderFactory.IncrementCounter("PipelineQueryIncCounter", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      });
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineQueryIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline(this.folderName, (LoanInfo.Right) 1, (string[]) null, (PipelineData) 1023, (PipelineSortOrder) sortOrder, criterion.Unwrap(), false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
    }

    public Loan OpenLoan(string name)
    {
      try
      {
        ClientMetricsProviderFactory.IncrementCounter("LoanOpenIncCounter", new SFxTag[1]
        {
          (SFxTag) new SFxSdkTag()
        });
        using (ClientMetricsProviderFactory.GetIncrementalTimer("LoanOpenIncTimer", new SFxTag[1]
        {
          (SFxTag) new SFxSdkTag()
        }))
          return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, this.Name, name ?? "", false));
      }
      catch (ObjectNotFoundException ex)
      {
        return (Loan) null;
      }
    }

    public Loan NewLoan(string name)
    {
      Loan loan = this.Session.Loans.CreateNew();
      loan.LoanFolder = this.folderName;
      loan.LoanName = name ?? "";
      return loan;
    }

    public bool LoanExists(string name)
    {
      return LoanIdentity.op_Inequality(this.mngr.GetLoanIdentity(this.Name, name ?? ""), (LoanIdentity) null);
    }

    public void DeleteLoan(string name)
    {
      LoanIdentity loanIdentity = this.mngr.GetLoanIdentity(this.Name, name ?? "");
      if (!LoanIdentity.op_Inequality(loanIdentity, (LoanIdentity) null))
        return;
      this.mngr.DeleteLoan(loanIdentity.Guid);
    }

    public void Rebuild()
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineRefreshIncCounter", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      });
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineRefreshIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
        this.mngr.RebuildPipeline(this.Name, (IServerProgressFeedback) null, (DatabaseToRebuild) 2);
    }

    public void Refresh()
    {
      this.info = (LoanFolderInfo) null;
      this.size = -1;
    }

    public override string ToString() => this.folderName;

    private void ensureLoaded()
    {
      if (this.info != null)
        return;
      this.info = this.Session.SessionObjects.LoanManager.GetLoanFolder(this.folderName);
    }
  }
}
