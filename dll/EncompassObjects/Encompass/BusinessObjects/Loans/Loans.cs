// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Loans
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class Loans : SessionBoundObject, ILoans
  {
    private ILoanManager mngr;
    private LoanFolders folders;
    private Milestones milestones;
    private MilestoneTemplates milestoneTemplates;
    private EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates templates;
    private LoanFieldDescriptors fieldDescriptors;
    private Roles roles;
    private AdjustableRateTypes rateTypes;
    private bool canAccessArchiveLoans;
    private bool loanSoftArchivalenabled;

    internal Loans(Session session)
      : base(session)
    {
      this.mngr = (ILoanManager) session.GetObject(nameof (LoanManager));
      this.canAccessArchiveLoans = this.Session.GetUserInfo().IsAdministrator() || (bool) session.SessionObjects.StartupInfo.UserAclFeatureRights[(object) (AclFeature) 233];
      this.loanSoftArchivalenabled = this.Session.SessionObjects.StartupInfo.EnableLoanSoftArchival;
    }

    public BatchReassign CreateBatchReassign() => new BatchReassign(this.Session);

    public Loan Open(string guid)
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
          return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, guid ?? "", false));
      }
      catch (ObjectNotFoundException ex)
      {
        return (Loan) null;
      }
    }

    public Loan Open(string guid, bool loanlock, bool exclusive)
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
        {
          if (!loanlock)
            return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, guid ?? "", false));
          lock (this)
          {
            try
            {
              LoanDataMgr.ImmediateExclusiveLockType exclusiveLockType = exclusive ? (LoanDataMgr.ImmediateExclusiveLockType) 1 : (LoanDataMgr.ImmediateExclusiveLockType) 2;
              return new Loan(this.Session, LoanDataMgr.OpenLoan(this.Session.SessionObjects, guid ?? "", false, 0, true, exclusiveLockType));
            }
            catch (LockException ex)
            {
              throw new LockException(ex);
            }
          }
        }
      }
      catch (ObjectNotFoundException ex)
      {
        return (Loan) null;
      }
      catch (LockException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public LoanIdentityList Query(QueryCriterion criterion)
    {
      return LoanIdentity.ToList(this.mngr.QueryLoans(new QueryCriterion[1]
      {
        criterion.Unwrap()
      }, false));
    }

    public StringList SelectFields(string guid, StringList fieldIds)
    {
      ILoan iloan = this.mngr.OpenLoan(guid ?? "");
      if (iloan == null)
        return (StringList) null;
      try
      {
        return new StringList((IList) iloan.SelectFields(fieldIds.ToArray()));
      }
      finally
      {
        iloan.Close();
      }
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
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
      {
        if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
          throw new Exception("User doesn't have access to archive loans.");
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string) null, (LoanInfo.Right) 1, (string[]) null, (PipelineData) 1023, (PipelineSortOrder) sortOrder, (QueryCriterion) null, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
      }
    }

    public PipelineCursor OpenPipelineEx(SortCriterionList sortCriteria)
    {
      return this.OpenPipelineEx(sortCriteria, true);
    }

    public PipelineCursor OpenPipelineEx(SortCriterionList sortCriteria, bool excludeArchivedLoans = true)
    {
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      });
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
      {
        if (sortCriteria == null)
          sortCriteria = new SortCriterionList();
        if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
          throw new Exception("User doesn't have access to archive loans.");
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string[]) null, (LoanInfo.Right) 1, (string[]) null, (PipelineData) 1023, sortCriteria.ToSortFieldList(), (QueryCriterion) null, false, 0, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
      }
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
      ClientMetricsProviderFactory.IncrementCounter("PipelineQueryIncCounter", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      });
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineQueryIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
      {
        if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
          throw new Exception("User doesn't have access to archive loans.");
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string) null, (LoanInfo.Right) 1, (string[]) null, (PipelineData) 1023, (PipelineSortOrder) sortOrder, criterion.Unwrap(), false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
      }
    }

    public PipelineCursor QueryPipelineEx(QueryCriterion criterion, SortCriterionList sortCriteria)
    {
      return this.QueryPipelineEx(criterion, sortCriteria, true);
    }

    public PipelineCursor QueryPipelineEx(
      QueryCriterion criterion,
      SortCriterionList sortCriteria,
      bool excludeArchivedLoans = true)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (sortCriteria == null)
        sortCriteria = new SortCriterionList();
      if (this.loanSoftArchivalenabled && !this.canAccessArchiveLoans && !excludeArchivedLoans)
        throw new Exception("User doesn't have access to archive loans.");
      ClientMetricsProviderFactory.IncrementCounter("PipelineOpenIncCounter", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      });
      using (ClientMetricsProviderFactory.GetIncrementalTimer("PipelineOpenIncTimer", new SFxTag[1]
      {
        (SFxTag) new SFxSdkTag()
      }))
        return new PipelineCursor(this.Session, this.mngr.OpenPipeline((string[]) null, (LoanInfo.Right) 1, (string[]) null, (PipelineData) 1023, sortCriteria.ToSortFieldList(), criterion.Unwrap(), false, 0, false, this.loanSoftArchivalenabled && (!this.canAccessArchiveLoans || excludeArchivedLoans)));
    }

    public Loan CreateNew()
    {
      return new Loan(this.Session, LoanDataMgr.NewLoan(this.Session.SessionObjects));
    }

    public bool Exists(string guid)
    {
      return LoanIdentity.op_Inequality(this.mngr.GetLoanIdentity(guid ?? ""), (LoanIdentity) null);
    }

    public Loan Import(string filePath, LoanImportFormat format)
    {
      return this.ImportWithTemplate(filePath, format, (LoanTemplate) null);
    }

    public Loan ImportWithTemplate(string filePath, LoanImportFormat format, LoanTemplate template)
    {
      return this.ImportWithLoanOfficer(filePath, format, template, (User) null);
    }

    public Loan ImportWithLoanOfficer(
      string filePath,
      LoanImportFormat format,
      LoanTemplate template,
      User user)
    {
      string loanOfficerId = user != null ? user.ID : string.Empty;
      return this.openImportedLoan(Loan.ImportFile(filePath, format, template, this.Session, loanOfficerId));
    }

    public Loan ImportFromBytes(ref byte[] importData, LoanImportFormat format)
    {
      return this.openImportedLoan(Loan.Import(importData, format, (LoanTemplate) null, this.Session, string.Empty));
    }

    public Loan ImportFromBytesWithTemplate(
      ref byte[] importData,
      LoanImportFormat format,
      LoanTemplate template)
    {
      return this.openImportedLoan(Loan.Import(importData, format, template, this.Session, string.Empty));
    }

    public Loan ImportFromTPO(
      ref byte[] importData,
      LoanImportFormat format,
      LoanTemplate template,
      bool suppressCalcs)
    {
      return this.openImportedLoan(Loan.Import(importData, format, template, this.Session, string.Empty, suppressCalcs, true));
    }

    public void Delete(string guid) => this.mngr.DeleteLoan(guid ?? "");

    public void SubmitBatchUpdate(BatchUpdate batch)
    {
      if (batch == null)
        throw new ArgumentNullException(nameof (batch));
      if (batch.Fields.Count == 0)
        throw new ArgumentException("BatchUpdate must contain at least one field to be updated");
      this.LoanManager.SubmitBatch(batch.Unwrap(), false);
    }

    public LoanFolders Folders
    {
      get
      {
        if (this.folders == null)
          this.folders = new LoanFolders(this.Session);
        return this.folders;
      }
    }

    public Roles Roles
    {
      get
      {
        if (this.roles == null)
          this.roles = new Roles(this.Session);
        return this.roles;
      }
    }

    public Milestones Milestones
    {
      get
      {
        if (this.milestones == null)
          this.milestones = new Milestones(this.Session);
        return this.milestones;
      }
    }

    public MilestoneTemplates MilestoneTemplates
    {
      get
      {
        if (this.milestoneTemplates == null)
          this.milestoneTemplates = new MilestoneTemplates(this.Session);
        return this.milestoneTemplates;
      }
    }

    public AdjustableRateTypes AdjustableRateTypes
    {
      get
      {
        if (this.rateTypes == null)
          this.rateTypes = new AdjustableRateTypes();
        return this.rateTypes;
      }
    }

    public EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates Templates
    {
      get
      {
        lock (this)
        {
          if (this.templates == null)
            this.templates = new EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates(this.Session);
        }
        return this.templates;
      }
    }

    public LoanFieldDescriptors FieldDescriptors
    {
      get
      {
        lock (this)
        {
          if (this.fieldDescriptors == null)
            this.fieldDescriptors = new LoanFieldDescriptors(this.Session);
        }
        return this.fieldDescriptors;
      }
    }

    internal ILoanManager LoanManager => this.mngr;

    private Loan openImportedLoan(LoanDataMgr dataMgr)
    {
      if (dataMgr.LoanData.GUID != "")
      {
        Loan loan = this.Open(dataMgr.LoanData.GUID);
        if (loan != null)
        {
          loan.Attach(dataMgr.LoanData);
          dataMgr.Close();
          return loan;
        }
      }
      return new Loan(this.Session, dataMgr);
    }
  }
}
