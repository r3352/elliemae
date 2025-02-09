// Decompiled with JetBrains decompiler
// Type: Elli.Service.IMortgageService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain.FileFormats;
using Elli.Domain.Mortgage;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Service
{
  public interface IMortgageService
  {
    void LoanSave(Loan loan);

    Loan LoanFileImport(Loan loan);

    Loan LoanFileImport(string fileData, LoanFileFormatType fileFormatType);

    Loan LoanFileImport(string fileData, LoanFileFormatType fileFormatType, string loanFolder);

    Loan LoanFileImport(
      string fileData,
      LoanFileFormatType fileFormatType,
      string folderName,
      string userName,
      LoanHistoryActionType actionType);

    Loan LoanFileImportToDataStore(
      string fileData,
      LoanFileFormatType fileFormatType,
      DataStores dataStores = DataStores.ActiveStore);

    Loan LoanGet(Guid loanId);

    Loan LoanGetArchive(Guid loanId);

    Loan LoanGetComplete(Guid loanId);

    Loan LoanGetCompleteEncompassId(Guid encompassId);

    string LoanFileExportEncompassId(Guid encompassId, LoanFileFormatType fileFormatType);

    string LoanFileExport(Guid loanId, LoanFileFormatType fileFormatType);

    void LoanDelete(Guid loanId);

    void LoanDeleteComplete(Guid loanId);

    void LoanDeleteArchive(Guid loanId);

    void LoanDeleteEncompassId(Guid encompassGuid);

    void LoanDeleteEncompassIdComplete(Guid encompassGuid);

    void LoanDeleteArchiveEncompassId(Guid encompassGuid);

    Loan MoveLoanToAnotherFolder(Guid encompassGuid, string destinationFolderName, string userId);

    void LoanFileArchive(
      string userId,
      string fileData,
      LoanHistoryActionType action,
      DateTime archivedUtc);

    void LoanFileArchive(
      string userId,
      string fileData,
      LoanHistoryActionType action,
      DateTime archivedUtc,
      string folderName);

    void LoanFileArchive(
      string userId,
      Loan loan,
      LoanHistoryActionType action,
      DateTime archivedUtc);

    void LoanFileArchive(
      string userId,
      Loan loan,
      LoanHistoryActionType action,
      DateTime archivedUtc,
      string folderName);

    object GetLoanContacts(Guid loanId);

    List<string> GetSubmitLoanGuid(
      UserInfo userInfo,
      int start,
      int limit,
      string sort,
      string filter);
  }
}
