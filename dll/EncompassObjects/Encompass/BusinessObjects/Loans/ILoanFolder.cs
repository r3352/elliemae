// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanFolder
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("9A404338-4341-4935-A5B4-F1780EA12076")]
  public interface ILoanFolder
  {
    string Name { get; }

    int Size { get; }

    LoanIdentityList GetContents();

    Loan OpenLoan(string name);

    Loan NewLoan(string name);

    bool LoanExists(string name);

    void DeleteLoan(string name);

    string ToString();

    void Rebuild();

    PipelineCursor OpenPipeline(PipelineSortOrder sortOrder, bool excludeArchivedLoans = true);

    PipelineCursor QueryPipeline(
      QueryCriterion criteria,
      PipelineSortOrder sortOrder,
      bool excludeArchivedLoans = true);

    PipelineCursor OpenPipeline(PipelineSortOrder sortOrder);

    PipelineCursor QueryPipeline(QueryCriterion criteria, PipelineSortOrder sortOrder);

    string DisplayName { get; }

    bool IsArchive { get; }

    bool IsTrash { get; }
  }
}
