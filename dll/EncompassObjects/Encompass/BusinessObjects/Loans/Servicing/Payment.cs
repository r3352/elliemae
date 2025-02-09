// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  public class Payment : ServicingTransaction, IPayment
  {
    internal Payment(Loan loan, PaymentTransactionLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    public int PaymentNumber => this.baseTrans.PaymentNo;

    public DateTime ScheduledDueDate => this.baseTrans.PaymentIndexDate;

    public DateTime StatementDate
    {
      get => this.baseTrans.StatementDate;
      set
      {
        this.baseTrans.StatementDate = value;
        this.setLastModified();
      }
    }

    public DateTime PaymentDueDate
    {
      get => this.baseTrans.PaymentDueDate;
      set
      {
        this.baseTrans.PaymentDueDate = value;
        this.setLastModified();
      }
    }

    public DateTime LatePaymentDate
    {
      get => this.baseTrans.LatePaymentDate;
      set
      {
        this.baseTrans.LatePaymentDate = value;
        this.setLastModified();
      }
    }

    public override DateTime TransactionDate
    {
      get => this.baseTrans.PaymentReceivedDate;
      set
      {
        this.baseTrans.PaymentReceivedDate = value;
        this.setLastModified();
      }
    }

    public DateTime PaymentDepositedDate
    {
      get => this.baseTrans.PaymentDepositedDate;
      set
      {
        this.baseTrans.PaymentDepositedDate = value;
        this.setLastModified();
      }
    }

    public Decimal IndexRate => Convert.ToDecimal(this.baseTrans.IndexRate);

    public Decimal InterestRate => Convert.ToDecimal(this.baseTrans.InterestRate);

    public Decimal AmountDue => Convert.ToDecimal(this.baseTrans.TotalAmountDue);

    public override Decimal TransactionAmount
    {
      get => base.TransactionAmount;
      set
      {
        base.TransactionAmount = value;
        this.adjustAddlPrincipal();
        this.setLastModified();
      }
    }

    public Decimal Principal
    {
      get => Convert.ToDecimal(this.baseTrans.Principal);
      set
      {
        this.baseTrans.Principal = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal Interest
    {
      get => Convert.ToDecimal(this.baseTrans.Interest);
      set
      {
        this.baseTrans.Interest = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal Escrow
    {
      get => Convert.ToDecimal(this.baseTrans.Escrow);
      set
      {
        this.baseTrans.Escrow = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal LateFee
    {
      get => Convert.ToDecimal(this.baseTrans.LateFee);
      set
      {
        this.baseTrans.LateFee = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal MiscFee
    {
      get => Convert.ToDecimal(this.baseTrans.MiscFee);
      set
      {
        this.baseTrans.MiscFee = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal AdditionalPrincipal
    {
      get => Convert.ToDecimal(this.baseTrans.AdditionalPrincipal);
      set
      {
        this.baseTrans.AdditionalPrincipal = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal AdditionalEscrow
    {
      get => Convert.ToDecimal(this.baseTrans.AdditionalEscrow);
      set
      {
        this.baseTrans.AdditionalEscrow = Convert.ToDouble(value);
        this.adjustTransAmount();
        this.setLastModified();
      }
    }

    public Decimal LateFeeIfLate
    {
      get => Convert.ToDecimal(this.baseTrans.LateFeeIfLate);
      set
      {
        this.baseTrans.LateFeeIfLate = Convert.ToDouble(value);
        this.setLastModified();
      }
    }

    public string InstitutionName
    {
      get => this.baseTrans.InstitutionName;
      set
      {
        this.baseTrans.InstitutionName = value ?? "";
        this.setLastModified();
      }
    }

    public string InstitutionRouting
    {
      get => this.baseTrans.InstitutionRouting;
      set
      {
        this.baseTrans.InstitutionRouting = value ?? "";
        this.setLastModified();
      }
    }

    public string AccountNumber
    {
      get => this.baseTrans.AccountNumber;
      set
      {
        this.baseTrans.AccountNumber = value ?? "";
        this.setLastModified();
      }
    }

    public string AccountHolder
    {
      get => this.baseTrans.AccountHolder;
      set
      {
        this.baseTrans.AccountHolder = value ?? "";
        this.setLastModified();
      }
    }

    public string Reference
    {
      get => this.baseTrans.Reference;
      set
      {
        this.baseTrans.Reference = value ?? "";
        this.setLastModified();
      }
    }

    public string CheckNumber
    {
      get => this.baseTrans.CheckNumber;
      set
      {
        this.baseTrans.CheckNumber = value ?? "";
        this.setLastModified();
      }
    }

    public Decimal PaymentAmount
    {
      get => Convert.ToDecimal(this.baseTrans.CheckAmount);
      set
      {
        this.baseTrans.CheckAmount = Convert.ToDouble(value);
        this.setLastModified();
      }
    }

    public DateTime PaymentDate
    {
      get => this.baseTrans.CheckDate;
      set
      {
        this.baseTrans.CheckDate = value;
        this.setLastModified();
      }
    }

    public string Comments
    {
      get => this.baseTrans.Comments;
      set
      {
        this.baseTrans.Comments = value ?? "";
        this.setLastModified();
      }
    }

    public bool IsReversed() => this.GetReversal() != null;

    public PaymentReversal GetReversal()
    {
      foreach (PaymentReversal transaction in (CollectionBase) this.Loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.PaymentReversal))
      {
        if (((PaymentReversalLog) transaction.Unwrap()).PaymentGUID == this.ID)
          return transaction;
      }
      return (PaymentReversal) null;
    }

    public PaymentReversal Reverse(DateTime reversalDate)
    {
      if (this.IsReversed())
        throw new Exception("A reversal already exists for the current payment");
      PaymentReversalLog servicingTransaction = (PaymentReversalLog) this.Loan.Unwrap().CreateNextServicingTransaction((ServicingTransactionTypes) 5);
      ((ServicingTransactionBase) servicingTransaction).TransactionDate = reversalDate;
      servicingTransaction.PaymentGUID = this.ID;
      ((ServicingTransactionBase) servicingTransaction).TransactionAmount = Convert.ToDouble(-1M * this.TransactionAmount);
      servicingTransaction.ReversalType = (ServicingTransactionTypes) 2;
      this.Loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new PaymentReversal(this.Loan, servicingTransaction);
    }

    private PaymentTransactionLog baseTrans => (PaymentTransactionLog) this.Unwrap();

    private void adjustTransAmount()
    {
      ((ServicingTransactionBase) this.baseTrans).TransactionAmount = Convert.ToDouble(this.Principal + this.Interest + this.Escrow + this.LateFee + this.MiscFee + this.AdditionalPrincipal + this.AdditionalEscrow);
    }

    private void adjustAddlPrincipal()
    {
      Decimal num = this.TransactionAmount - this.Principal - this.Interest - this.Escrow - this.LateFee - this.MiscFee - this.AdditionalEscrow;
      if (num >= 0M)
      {
        this.baseTrans.AdditionalPrincipal = Convert.ToDouble(num);
      }
      else
      {
        this.baseTrans.AdditionalPrincipal = 0.0;
        this.baseTrans.Principal -= Convert.ToDouble(num);
      }
    }
  }
}
