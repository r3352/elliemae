// Decompiled with JetBrains decompiler
// Type: Elli.Data.Repositories.LoanRepository
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using Elli.Data.Orm;
using Elli.Domain.Mortgage;
using EllieMae.EMLite.RemotingServices;
using NHibernate;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Data.Repositories
{
  public class LoanRepository : ILoanRepository
  {
    public void LoanSave(Loan loan)
    {
      ISession currentSession = UnitOfWork.GetCurrentSession();
      ITransaction transaction = currentSession.BeginTransaction();
      currentSession.Save((object) loan);
      if (NHibernateUtil.IsInitialized((object) loan.LoanProductData))
        currentSession.Save((object) loan.LoanProductData);
      if (NHibernateUtil.IsInitialized((object) loan.Property))
        currentSession.Save((object) loan.Property);
      if (NHibernateUtil.IsInitialized((object) loan.RateLock))
        currentSession.Save((object) loan.RateLock);
      if (NHibernateUtil.IsInitialized((object) loan.DownPayment))
        currentSession.Save((object) loan.DownPayment);
      if (NHibernateUtil.IsInitialized((object) loan.ClosingDocument))
        currentSession.Save((object) loan.ClosingDocument);
      if (NHibernateUtil.IsInitialized((object) loan.EmDocument))
        currentSession.Save((object) loan.EmDocument);
      if (NHibernateUtil.IsInitialized((object) loan.EmDocumentInvestor))
        currentSession.Save((object) loan.EmDocumentInvestor);
      if (NHibernateUtil.IsInitialized((object) loan.EmDocumentLender))
        currentSession.Save((object) loan.EmDocumentLender);
      if (NHibernateUtil.IsInitialized((object) loan.AdditionalRequests))
        currentSession.Save((object) loan.AdditionalRequests);
      if (NHibernateUtil.IsInitialized((object) loan.ClosingCost))
      {
        currentSession.Save((object) loan.ClosingCost);
        if (loan.ClosingCost.Gfe2010 != null)
          currentSession.Save((object) loan.ClosingCost.Gfe2010);
        if (loan.ClosingCost.Gfe2010Page != null)
          currentSession.Save((object) loan.ClosingCost.Gfe2010Page);
        if (loan.ClosingCost.Gfe2010Section != null)
          currentSession.Save((object) loan.ClosingCost.Gfe2010Section);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.LoanEstimate1))
          currentSession.Save((object) loan.ClosingCost.LoanEstimate1);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.LoanEstimate2))
          currentSession.Save((object) loan.ClosingCost.LoanEstimate2);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.LoanEstimate3))
          currentSession.Save((object) loan.ClosingCost.LoanEstimate3);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.ClosingDisclosure1))
          currentSession.Save((object) loan.ClosingCost.ClosingDisclosure1);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.ClosingDisclosure2))
          currentSession.Save((object) loan.ClosingCost.ClosingDisclosure2);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.ClosingDisclosure3))
          currentSession.Save((object) loan.ClosingCost.ClosingDisclosure3);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.ClosingDisclosure4))
          currentSession.Save((object) loan.ClosingCost.ClosingDisclosure4);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.ClosingDisclosure5))
          currentSession.Save((object) loan.ClosingCost.ClosingDisclosure5);
        if (NHibernateUtil.IsInitialized((object) loan.ClosingCost.FeeVarianceOther))
          currentSession.Save((object) loan.ClosingCost.FeeVarianceOther);
      }
      if (NHibernateUtil.IsInitialized((object) loan.FhaVaLoan))
        currentSession.Save((object) loan.FhaVaLoan);
      if (NHibernateUtil.IsInitialized((object) loan.StatementCreditDenial))
        currentSession.Save((object) loan.StatementCreditDenial);
      if (NHibernateUtil.IsInitialized((object) loan.Tsum))
        currentSession.Save((object) loan.Tsum);
      if (NHibernateUtil.IsInitialized((object) loan.FannieMae))
        currentSession.Save((object) loan.FannieMae);
      if (NHibernateUtil.IsInitialized((object) loan.FreddieMac))
        currentSession.Save((object) loan.FreddieMac);
      if (NHibernateUtil.IsInitialized((object) loan.HudLoanData))
        currentSession.Save((object) loan.HudLoanData);
      if (NHibernateUtil.IsInitialized((object) loan.VaLoanData))
        currentSession.Save((object) loan.VaLoanData);
      if (NHibernateUtil.IsInitialized((object) loan.DisclosureNotices))
        currentSession.Save((object) loan.DisclosureNotices);
      if (NHibernateUtil.IsInitialized((object) loan.Funding))
        currentSession.Save((object) loan.Funding);
      if (NHibernateUtil.IsInitialized((object) loan.Gfe))
        currentSession.Save((object) loan.Gfe);
      if (NHibernateUtil.IsInitialized((object) loan.Hmda))
        currentSession.Save((object) loan.Hmda);
      if (NHibernateUtil.IsInitialized((object) loan.Hud1Es))
        currentSession.Save((object) loan.Hud1Es);
      if (NHibernateUtil.IsInitialized((object) loan.InterimServicing))
        currentSession.Save((object) loan.InterimServicing);
      if (NHibernateUtil.IsInitialized((object) loan.NetTangibleBenefit))
        currentSession.Save((object) loan.NetTangibleBenefit);
      if (NHibernateUtil.IsInitialized((object) loan.LoanSubmission))
        currentSession.Save((object) loan.LoanSubmission);
      if (NHibernateUtil.IsInitialized((object) loan.Prequalification))
        currentSession.Save((object) loan.Prequalification);
      if (NHibernateUtil.IsInitialized((object) loan.PrivacyPolicy))
        currentSession.Save((object) loan.PrivacyPolicy);
      if (NHibernateUtil.IsInitialized((object) loan.Section32))
        currentSession.Save((object) loan.Section32);
      if (NHibernateUtil.IsInitialized((object) loan.SelectedHomeCounselingProvider))
        currentSession.Save((object) loan.SelectedHomeCounselingProvider);
      if (NHibernateUtil.IsInitialized((object) loan.Mcaw))
        currentSession.Save((object) loan.Mcaw);
      if (NHibernateUtil.IsInitialized((object) loan.RegulationZ))
        currentSession.Save((object) loan.RegulationZ);
      if (NHibernateUtil.IsInitialized((object) loan.ServicingDisclosure))
        currentSession.Save((object) loan.ServicingDisclosure);
      if (NHibernateUtil.IsInitialized((object) loan.Shipping))
        currentSession.Save((object) loan.Shipping);
      if (NHibernateUtil.IsInitialized((object) loan.StateDisclosure))
        currentSession.Save((object) loan.StateDisclosure);
      if (NHibernateUtil.IsInitialized((object) loan.Uldd))
        currentSession.Save((object) loan.Uldd);
      if (NHibernateUtil.IsInitialized((object) loan.Usda))
        currentSession.Save((object) loan.Usda);
      if (NHibernateUtil.IsInitialized((object) loan.Miscellaneous))
        currentSession.Save((object) loan.Miscellaneous);
      if (NHibernateUtil.IsInitialized((object) loan.CommitmentTerms))
        currentSession.Save((object) loan.CommitmentTerms);
      if (NHibernateUtil.IsInitialized((object) loan.ProfitManagement))
        currentSession.Save((object) loan.ProfitManagement);
      if (NHibernateUtil.IsInitialized((object) loan.TrustAccount))
        currentSession.Save((object) loan.TrustAccount);
      if (NHibernateUtil.IsInitialized((object) loan.UnderwriterSummary))
        currentSession.Save((object) loan.UnderwriterSummary);
      if (NHibernateUtil.IsInitialized((object) loan.LOCompensation))
        currentSession.Save((object) loan.LOCompensation);
      if (NHibernateUtil.IsInitialized((object) loan.TQL))
        currentSession.Save((object) loan.TQL);
      if (NHibernateUtil.IsInitialized((object) loan.ATRQMCommon))
        currentSession.Save((object) loan.ATRQMCommon);
      if (NHibernateUtil.IsInitialized((object) loan.Correspondent))
        currentSession.Save((object) loan.Correspondent);
      if (NHibernateUtil.IsInitialized((object) loan.TPO))
        currentSession.Save((object) loan.TPO);
      if (NHibernateUtil.IsInitialized((object) loan.ConstructionManagement))
        currentSession.Save((object) loan.ConstructionManagement);
      if (NHibernateUtil.IsInitialized((object) loan.CollateralTracking))
        currentSession.Save((object) loan.CollateralTracking);
      if (NHibernateUtil.IsInitialized((object) loan.Aiq))
        currentSession.Save((object) loan.Aiq);
      if (NHibernateUtil.IsInitialized((object) loan.EClose))
        currentSession.Save((object) loan.EClose);
      currentSession.Flush();
      transaction.Commit();
    }

    public Loan LoanGet(Guid id)
    {
      Loan loan = UnitOfWork.GetCurrentSession().CreateQuery("select l from Loan l where l.Id = :loanId").SetGuid("loanId", id).UniqueResult<Loan>();
      if (loan != null)
        loan.DBIndicator = true;
      return loan;
    }

    public Loan LoanGetCompleteEncompassId(Guid encompassId)
    {
      Loan completeEncompassId = UnitOfWork.GetCurrentSession().CreateQuery("select l from Loan l where l.EncompassId = :encompassId").SetGuid(nameof (encompassId), encompassId).UniqueResult<Loan>();
      if (completeEncompassId != null)
        completeEncompassId = this.LoanGetComplete(completeEncompassId.Id);
      return completeEncompassId;
    }

    public Loan LoanGetComplete(Guid loanId)
    {
      Loan firstResult = OrmUtility.GetFirstResult<Loan>(UnitOfWork.GetCurrentSession().CreateMultiQuery().Add<Loan>("from Loan where Id = :id").Add<Property>("from Property where Id = :id").Add<RateLock>("from RateLock where Id = :id").Add<ClosingDocument>("from ClosingDocument where Id = :id").Add<LoanProductData>("from LoanProductData where Id = :id").Add<AdditionalRequests>("from AdditionalRequests where Id = :id").Add<ClosingCost>("from ClosingCost where Id = :id").Add<EmDocument>("from EmDocument where Id = :id").Add<EmDocumentInvestor>("from EmDocumentInvestor where Id = :id").Add<EmDocumentLender>("from EmDocumentLender where Id = :id").Add<FhaVaLoan>("from FhaVaLoan where Id = :id").Add<HudLoanData>("from HudLoanData where Id = :id").Add<VaLoanData>("from VaLoanData where Id = :id").Add<Funding>("from Funding where Id = :id").Add<Gfe>("from Gfe where Id = :id").Add<StatementCreditDenial>("from StatementCreditDenial where Id = :id").Add<Tsum>("from Tsum where Id = :id").Add<DisclosureNotices>("from DisclosureNotices where Id = :id").Add<FannieMae>("from FannieMae where Id = :id").Add<FreddieMac>("from FreddieMac where Id = :id").Add<Hmda>("from Hmda where Id = :id").Add<Hud1Es>("from Hud1Es where Id = :id").Add<InterimServicing>("from InterimServicing where Id = :id").Add<LoanSubmission>("from LoanSubmission where Id = :id").Add<Mcaw>("from Mcaw where Id = :id").Add<NetTangibleBenefit>("from NetTangibleBenefit where Id = :id").Add<Prequalification>("from Prequalification where Id = :id").Add<PrivacyPolicy>("from PrivacyPolicy where Id = :id").Add<Section32>("from Section32 where Id = :id").Add<SelectedHomeCounselingProvider>("from SelectedHomeCounselingProvider where Id = :id").Add<RegulationZ>("from RegulationZ where Id = :id").Add<ServicingDisclosure>("from ServicingDisclosure where Id = :id").Add<Shipping>("from Shipping where Id = :id").Add<StateDisclosure>("from StateDisclosure where Id = :id").Add<Uldd>("from Uldd where Id = :id").Add<Usda>("from Usda where Id = :id").Add<Miscellaneous>("from Miscellaneous where Id = :id").Add<CommitmentTerms>("from CommitmentTerms where Id = :id").Add<ProfitManagement>("from ProfitManagement where Id = :id").Add<TrustAccount>("from TrustAccount where Id = :id").Add<UnderwriterSummary>("from UnderwriterSummary where Id = :id").Add<ElliLOCompensation>("from ElliLOCompensation where Id = :id").Add<TQL>("from TQL where Id = :id").Add<ATRQMCommon>("from ATRQMCommon where Id = :id").Add<TPO>("from TPO where Id = :id").Add<LoanEstimate1>("from LoanEstimate1 where Id = :id").Add<LoanEstimate2>("from LoanEstimate2 where Id = :id").Add<LoanEstimate3>("from LoanEstimate3 where Id = :id").Add<ClosingDisclosure1>("from ClosingDisclosure1 where Id = :id").Add<ClosingDisclosure2>("from ClosingDisclosure2 where Id = :id").Add<ClosingDisclosure3>("from ClosingDisclosure3 where Id = :id").Add<ClosingDisclosure4>("from ClosingDisclosure4 where Id = :id").Add<ClosingDisclosure5>("from ClosingDisclosure5 where Id = :id").Add<FeeVariance>("from FeeVariance where Id = :id").Add<FeeVarianceOther>("from FeeVarianceOther where Id = :id").Add<Correspondent>("from Correspondent where Id = :id").Add<ConstructionManagement>("from ConstructionManagement where Id = :id").Add<CollateralTracking>("from CollateralTracking where Id = :id").Add<Aiq>("from Aiq where Id = :id").Add<EClose>("from EClose where Id = :id").SetGuid("id", loanId).List(), 0);
      if (firstResult != null)
        firstResult.DBIndicator = true;
      return firstResult;
    }

    public void LoanDeleteEncompassId(Guid encompassId)
    {
      ISession currentSession = UnitOfWork.GetCurrentSession();
      ITransaction transaction = currentSession.BeginTransaction();
      currentSession.GetNamedQuery("loan-delete").SetGuid("AltId", encompassId).ExecuteUpdate();
      transaction.Commit();
      currentSession.Clear();
    }

    public void LoanDelete(Guid loanId)
    {
      ISession currentSession = UnitOfWork.GetCurrentSession();
      Loan loan = this.LoanGet(loanId);
      ITransaction transaction = currentSession.BeginTransaction();
      currentSession.GetNamedQuery("loan-delete").SetGuid("AltId", loan.EncompassId).ExecuteUpdate();
      transaction.Commit();
      currentSession.Clear();
    }

    public Loan LoanArchiveGet(Guid encompassLoanGuid) => (Loan) null;

    public void LoanArchiveSave(Loan loan)
    {
    }

    public void LoanArchiveRemove(Guid encompassLoanGuid)
    {
    }

    public LoanHistory LoanHistoryGet(Guid encompassLoanGuid) => (LoanHistory) null;

    public void LoanHistorySave(LoanHistory loanHistory)
    {
    }

    public void LoanHistoryRemove(Guid encompassLoanGuid)
    {
    }

    public CompressedLoanFile CompressedLoanFileGet(string id) => (CompressedLoanFile) null;

    public void CompressedLoanFileSave(CompressedLoanFile compressedLoanFile)
    {
    }

    public void CompressedLoanFileRemove(string id)
    {
    }

    public void LoanSnapshotDataSave(Loan loan)
    {
    }

    public void LoanSnapshotDataGet(Loan loan)
    {
    }

    public bool SupportsArchive => false;

    public void LoanHistorySave(LoanHistory loanHistory, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void CompressedLoanFileSave(CompressedLoanFile compressedLoanFile, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void LoanSnapshotDataSave(Loan loan, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public Loan LoanGet(Guid id, DataStores dataStores = DataStores.ActiveStore) => (Loan) null;

    public Loan LoanGetEncompassId(Guid encompassId, DataStores dataStores = DataStores.ActiveStore)
    {
      return (Loan) null;
    }

    public void LoanSnapshotDataGet(Loan loan, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void LoanSnapshotDataGetComplete(Loan loan)
    {
    }

    public SnapshotDocument LoanSnapshotDataGet(Guid snapshotGuid, DataStores dataStores = DataStores.ActiveStore)
    {
      return (SnapshotDocument) null;
    }

    public SnapshotDocument LoanSnapshotDataGetComplete(Guid snapshotGuid)
    {
      return (SnapshotDocument) null;
    }

    public LoanHistory LoanHistoryGet(Guid encompassLoanGuid, DataStores dataStores = DataStores.ActiveStore)
    {
      return (LoanHistory) null;
    }

    public LoanHistory LoanHistoryGetComplete(Guid encompassLoanGuid) => (LoanHistory) null;

    public CompressedLoanFile CompressedLoanFileGet(string id, DataStores dataStores = DataStores.ActiveStore)
    {
      return (CompressedLoanFile) null;
    }

    public CompressedLoanFile CompressedLoanFileGetComplete(string id) => (CompressedLoanFile) null;

    public void LoanDelete(Guid loanId, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void LoanDeleteComplete(Guid loanId)
    {
    }

    public void LoanDeleteEncompassId(Guid encompassId, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void LoanDeleteEncompassIdComplete(Guid encompassId)
    {
    }

    public void LoanSnapshotDataDelete(Guid encompassId, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void LoanSnapshotDataDeleteComplete(Guid encompassId)
    {
    }

    public void LoanHistoryRemove(Guid encompassLoanGuid, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void LoanHistoryRemoveComplete(Guid encompassLoanGuid)
    {
    }

    public void CompressedLoanFileRemove(string id, DataStores dataStores = DataStores.ActiveStore)
    {
    }

    public void CompressedLoanFileRemoveComplete(string id)
    {
    }

    public object LoanContactsGet(Guid loanId) => (object) null;

    public List<string> GetSubmitLoanGuid(
      UserInfo userInfo,
      int start,
      int limit,
      string sort,
      string filter)
    {
      return (List<string>) null;
    }
  }
}
