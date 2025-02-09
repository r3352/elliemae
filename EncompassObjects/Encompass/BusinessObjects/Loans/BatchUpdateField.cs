// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.BatchUpdateField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents a single field value to be updated with the Batch Loan Update function.
  /// </summary>
  public class BatchUpdateField
  {
    private LoanBatchField field;

    internal BatchUpdateField(LoanBatchField field) => this.field = field;

    /// <summary>Gets the FieldID of the field to be updated.</summary>
    public string FieldID => this.field.FieldID;

    /// <summary>Gets the value of the field to be stored.</summary>
    public object FieldValue => this.field.FieldValue;

    internal LoanBatchField Unwrap() => this.field;
  }
}
