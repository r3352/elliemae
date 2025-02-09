// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanFields : ILoanFields
  {
    private Loan loan;
    private Hashtable fields = new Hashtable();

    internal LoanFields(Loan loan) => this.loan = loan;

    public LoanField this[string fieldId]
    {
      get
      {
        lock (this.loan)
        {
          LoanField field = (LoanField) this.fields[(object) (fieldId ?? "")];
          if (field != null)
            return field;
          FieldDefinition fieldDefinition = this.loan.Unwrap().LoanData.GetFieldDefinition(fieldId);
          LoanField loanField = fieldDefinition != null && fieldDefinition != FieldDefinition.Empty ? new LoanField(this.loan, new FieldDescriptor(fieldDefinition)) : throw new ArgumentException("The Field ID '" + fieldId + "' is invalid.", nameof (fieldId));
          this.fields.Add((object) fieldId, (object) loanField);
          return loanField;
        }
      }
    }

    public LoanField GetFieldAt(string fieldId, int itemIndex)
    {
      return this[this.generateFieldID(fieldId ?? "", itemIndex)];
    }

    private string generateFieldID(string baseId, int itemIndex)
    {
      for (int index = 0; index < baseId.Length; ++index)
      {
        if (!char.IsLetter(baseId[index]))
          return baseId.Substring(0, index) + itemIndex.ToString("00") + baseId.Substring(index);
      }
      return baseId;
    }
  }
}
