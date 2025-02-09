// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchUpdate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class BatchUpdate
  {
    private LoanBatch batch;
    private BatchUpdateFields fields;

    public BatchUpdate(string loanGuid)
      : this(new StringList((IList) new string[1]
      {
        loanGuid
      }))
    {
    }

    public BatchUpdate(StringList loanGuids)
    {
      LoanSetBatch loanSetBatch = new LoanSetBatch();
      loanSetBatch.LoanGuids.AddRange((IEnumerable<string>) loanGuids.ToArray());
      this.batch = (LoanBatch) loanSetBatch;
      this.fields = new BatchUpdateFields(this);
    }

    public BatchUpdate(QueryCriterion selectionCriteria)
    {
      this.batch = (LoanBatch) new LoanQueryBatch(selectionCriteria.Unwrap());
      this.fields = new BatchUpdateFields(this);
    }

    public BatchUpdateFields Fields => this.fields;

    internal LoanBatch Unwrap() => this.batch;
  }
}
