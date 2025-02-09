// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchUpdateFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class BatchUpdateFields
  {
    private BatchUpdate batch;
    private Dictionary<string, BatchUpdateField> fields = new Dictionary<string, BatchUpdateField>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    internal BatchUpdateFields(BatchUpdate batch) => this.batch = batch;

    public int Count => this.fields.Count;

    public BatchUpdateField this[string fieldId]
    {
      get => !this.fields.ContainsKey(fieldId) ? (BatchUpdateField) null : this.fields[fieldId];
    }

    public BatchUpdateField Add(string fieldId, object fieldValue)
    {
      this.Remove(fieldId);
      LoanBatchField field = new LoanBatchField(fieldId, fieldValue);
      this.batch.Unwrap().Fields.Add(field);
      BatchUpdateField batchUpdateField = new BatchUpdateField(field);
      this.fields[fieldId] = batchUpdateField;
      return batchUpdateField;
    }

    public void Remove(string fieldId)
    {
      if (!this.fields.ContainsKey(fieldId))
        return;
      this.batch.Unwrap().Fields.Remove(this.fields[fieldId].Unwrap());
      this.fields.Remove(fieldId);
    }

    public bool Contains(string fieldId) => this.fields.ContainsKey(fieldId);
  }
}
