// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoans
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("80E04FB1-10C9-4b77-BA04-B13D5B21217F")]
  public interface ILoans
  {
    Loan Open(string loanGuid);

    LoanIdentityList Query(QueryCriterion criterion);

    StringList SelectFields(string loadGuid, StringList fieldIds);

    Loan CreateNew();

    bool Exists(string loanGuid);

    Loan Import(string filePath, LoanImportFormat importFormat);

    void Delete(string loanGuid);

    LoanFolders Folders { get; }

    Milestones Milestones { get; }

    Loan ImportFromBytes(ref byte[] importData, LoanImportFormat format);

    EllieMae.Encompass.BusinessObjects.Loans.Templates.Templates Templates { get; }

    Loan ImportWithTemplate(string filePath, LoanImportFormat format, LoanTemplate template);

    PipelineCursor OpenPipeline(PipelineSortOrder sortOrder, bool excludeArchivedLoans = true);

    PipelineCursor OpenPipelineEx(SortCriterionList sortCriteria, bool excludeArchivedLoans = true);

    PipelineCursor QueryPipeline(
      QueryCriterion criteria,
      PipelineSortOrder sortOrder,
      bool excludeArchivedLoans = true);

    PipelineCursor QueryPipelineEx(
      QueryCriterion criteria,
      SortCriterionList sortCriteria,
      bool excludeArchivedLoans = true);

    PipelineCursor OpenPipeline(PipelineSortOrder sortOrder);

    PipelineCursor OpenPipelineEx(SortCriterionList sortCriteria);

    PipelineCursor QueryPipeline(QueryCriterion criteria, PipelineSortOrder sortOrder);

    PipelineCursor QueryPipelineEx(QueryCriterion criteria, SortCriterionList sortCriteria);

    LoanFieldDescriptors FieldDescriptors { get; }

    Roles Roles { get; }

    Loan ImportWithLoanOfficer(
      string filePath,
      LoanImportFormat format,
      LoanTemplate template,
      User user);

    Loan ImportFromBytesWithTemplate(
      ref byte[] importData,
      LoanImportFormat format,
      LoanTemplate template);
  }
}
