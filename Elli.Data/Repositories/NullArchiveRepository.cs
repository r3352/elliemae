// Decompiled with JetBrains decompiler
// Type: Elli.Data.Repositories.NullArchiveRepository
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using Elli.Domain.Mortgage;
using System;

#nullable disable
namespace Elli.Data.Repositories
{
  public class NullArchiveRepository : IArchiveRepository
  {
    public LoanHistory LoanHistoryGet(Guid encompassLoanGuid) => (LoanHistory) null;

    public void LoanHistorySave(LoanHistory loanHistory)
    {
    }

    public void LoanHistoryRemove(Guid encompassLoanGuid)
    {
    }

    public Loan LoanArchiveGet(Guid encompassLoanGuid) => (Loan) null;

    public void LoanArchiveSave(Loan loan)
    {
    }

    public void LoanArchiveRemove(Guid encompassLoanGuid)
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

    public void LoanDeleteEncompassId(Guid encompassId) => throw new NotImplementedException();
  }
}
