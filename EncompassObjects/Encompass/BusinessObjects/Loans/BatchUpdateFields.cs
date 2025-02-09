// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchUpdateFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BatchUpdateField" /> values associated with a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BatchUpdate" />.
  /// </summary>
  public class BatchUpdateFields
  {
    private BatchUpdate batch;
    private Dictionary<string, BatchUpdateField> fields = new Dictionary<string, BatchUpdateField>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    internal BatchUpdateFields(BatchUpdate batch) => this.batch = batch;

    /// <summary>Gets the number of fields in the batch.</summary>
    public int Count => this.fields.Count;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BatchUpdateField" /> for a specified Field ID.
    /// </summary>
    /// <param name="fieldId">The Field ID for which the record should be retrieved.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.BatchUpdateField" /> is one is present,
    /// <c>null</c> otherwise.</returns>
    public BatchUpdateField this[string fieldId]
    {
      get => !this.fields.ContainsKey(fieldId) ? (BatchUpdateField) null : this.fields[fieldId];
    }

    /// <summary>Adds a new field value to the batch update.</summary>
    /// <param name="fieldId">The Field ID for the field to be updated.</param>
    /// <param name="fieldValue">The value to be assigned to the field.</param>
    /// <remarks>The Field ID specified can be any loan field except for virtual field values.
    /// The field value specified should have a type which is correct for the specified
    /// field. If a record already exists in the batch for this field ID, it will be
    /// replaced with this new value.</remarks>
    public BatchUpdateField Add(string fieldId, object fieldValue)
    {
      this.Remove(fieldId);
      LoanBatchField field = new LoanBatchField(fieldId, fieldValue);
      this.batch.Unwrap().Fields.Add(field);
      BatchUpdateField batchUpdateField = new BatchUpdateField(field);
      this.fields[fieldId] = batchUpdateField;
      return batchUpdateField;
    }

    /// <summary>
    /// Removes a field from the collection based using its Field ID.
    /// </summary>
    /// <param name="fieldId">The Field ID of the field to be removed.</param>
    public void Remove(string fieldId)
    {
      if (!this.fields.ContainsKey(fieldId))
        return;
      this.batch.Unwrap().Fields.Remove(this.fields[fieldId].Unwrap());
      this.fields.Remove(fieldId);
    }

    /// <summary>Determines if a field is contained in the batch.</summary>
    /// <param name="fieldId">The Field ID to be checked.</param>
    /// <returns>Returns <c>true</c> if the batch contains a record for the specified field,
    /// <c>false</c> otherwise.</returns>
    public bool Contains(string fieldId) => this.fields.ContainsKey(fieldId);
  }
}
