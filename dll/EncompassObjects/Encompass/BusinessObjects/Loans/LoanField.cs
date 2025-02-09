// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanField : Field, ILoanField
  {
    private Loan loan;

    internal LoanField(Loan loan, FieldDescriptor descriptor)
      : base(descriptor)
    {
      this.loan = loan;
    }

    public override string UnformattedValue
    {
      get => this.Descriptor.UnformatValue(this.loan.LoanData.GetSimpleField(this.ID));
    }

    internal override void setFieldValue(string value)
    {
      if (this.Descriptor.RequiresExclusiveLock)
        this.loan.EnsureExclusive();
      if (this.loan.LoanAccessExceptionsEnabled && this.ReadOnly)
        throw new InvalidOperationException("Edit access to the field '" + this.Descriptor.FieldID + "' is denied");
      if (this.loan.CalculationsEnabled)
        this.loan.LoanData.SetField(this.ID, value);
      else
        this.loan.LoanData.SetCurrentField(this.ID, value);
    }

    string ILoanField.Value
    {
      get => this.FormattedValue;
      set => this.Value = (object) value;
    }

    public string OriginalValue => this.loan.LoanData.GetOrgField(this.ID);

    public bool Locked
    {
      get => this.loan.LoanData.IsLocked(this.ID);
      set
      {
        this.loan.EnsureExclusive();
        if (value)
          this.loan.LoanData.AddLock(this.ID);
        else
          this.loan.LoanData.RemoveLock(this.ID);
      }
    }

    public bool ReadOnly
    {
      get
      {
        if (this.Descriptor.ReadOnly || this.loan.LoanData.IsFieldReadOnly(this.Descriptor.FieldID))
          return true;
        if (this.loan.LoanData.ContentAccess == 16777215)
          return false;
        if ((this.loan.LoanData.ContentAccess & 4096) == null)
          return true;
        return this.loan.LoanData.EditableFields != null && !this.loan.LoanData.EditableFields.ContainsKey((object) this.Descriptor.FieldID);
      }
    }

    public string GetValueForBorrowerPair(BorrowerPair pair)
    {
      return this.loan.LoanData.GetSimpleField(this.ID, pair.Unwrap());
    }

    public void SetValueForBorrowerPair(BorrowerPair pair, string value)
    {
      string str = (string) null;
      BorrowerPair[] borrowerPairArray = (BorrowerPair[]) null;
      if (pair != (BorrowerPair) null && pair.Borrower != (Borrower) null && !string.IsNullOrEmpty(pair.Borrower.ID) && this.loan.LoanData.CurrentBorrowerPair.Id != pair.Borrower.ID)
      {
        str = this.loan.LoanData.CurrentBorrowerPair.Id;
        borrowerPairArray = this.loan.LoanData.GetBorrowerPairs();
        if (borrowerPairArray.Length > 1)
        {
          for (int index = 0; index < borrowerPairArray.Length; ++index)
          {
            if (borrowerPairArray[index].Borrower.Id == pair.Borrower.ID)
            {
              this.loan.LoanData.SetBorrowerPair(borrowerPairArray[index]);
              break;
            }
          }
        }
      }
      this.loan.LoanData.SetCurrentField(this.ID, this.Descriptor.ValidateInput(value ?? ""), pair.Unwrap());
      if (this.loan.CalculationsEnabled)
        this.loan.Recalculate();
      if (string.IsNullOrEmpty(str) || borrowerPairArray == null)
        return;
      for (int index = 0; index < borrowerPairArray.Length; ++index)
      {
        if (borrowerPairArray[index].Borrower.Id == str)
        {
          this.loan.LoanData.SetBorrowerPair(borrowerPairArray[index]);
          break;
        }
      }
    }
  }
}
