// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Represents a borrower payment against the loan balance.
  /// </summary>
  /// <example>
  ///       The following code demonstrates how to record a payment into the servicing
  ///       history of the loan.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
  /// 
  /// class SampleApp
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.StartOffline("mary", "maryspwd");
  /// 
  ///       // Open the loan using the GUID specified on the command line
  ///       Loan loan = session.Loans.Open(args[0]);
  ///       loan.Lock();
  /// 
  ///       // Add a new payment to the servicing history of the loan.
  ///       // The payment will be pre-populated with the data for the next scheduled payment.
  ///       Payment pmt = loan.Servicing.Transactions.AddPayment(DateTime.Now);
  /// 
  ///       // Adjust the transaction amount to the amount actually received from the customer
  ///       // If this is more than the scheduled payment amount, the balanced will be applied
  ///       // as an additional principal payment. If it's less, the portion of the
  ///       // payment allocated to principal will be reduced by the difference.
  ///       pmt.TransactionAmount = 2000;
  /// 
  ///       // Display the portion allocated to each balance
  ///       Console.WriteLine("Principal: " + pmt.Principal);
  ///       Console.WriteLine("Interest:  " + pmt.Interest);
  ///       Console.WriteLine("Escrow:    " + pmt.Escrow);
  ///       Console.WriteLine("Late Fee:  " + pmt.LateFee);
  ///       Console.WriteLine("Addl Prin: " + pmt.AdditionalPrincipal);
  ///       Console.WriteLine("Addl Escr: " + pmt.AdditionalEscrow);
  ///       Console.WriteLine("Misc Fees: " + pmt.MiscFee);
  /// 
  ///       // Close the loan, releasing its resources
  ///       loan.Commit();
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///       </code>
  ///     </example>
  public class Payment : ServicingTransaction, IPayment
  {
    internal Payment(Loan loan, PaymentTransactionLog trans)
      : base(loan, (ServicingTransactionBase) trans)
    {
    }

    /// <summary>Gets the sequential index of the payment.</summary>
    public int PaymentNumber => this.baseTrans.PaymentNo;

    /// <summary>
    /// Gets the date on which this payment was originally scheduled to be due.
    /// </summary>
    /// <remarks>This date will differ from the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.PaymentDueDate" /> only if the
    /// PaymetDueDate has been override by the user. In that case, this property will indicate
    /// the original due date of the scheduled payment.</remarks>
    public DateTime ScheduledDueDate => this.baseTrans.PaymentIndexDate;

    /// <summary>Gets or sets the date the statement was issued.</summary>
    public DateTime StatementDate
    {
      get => this.baseTrans.StatementDate;
      set
      {
        this.baseTrans.StatementDate = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets the payment due date.</summary>
    public DateTime PaymentDueDate
    {
      get => this.baseTrans.PaymentDueDate;
      set
      {
        this.baseTrans.PaymentDueDate = value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the date after which payment is considered late and the late fee applies.
    /// </summary>
    public DateTime LatePaymentDate
    {
      get => this.baseTrans.LatePaymentDate;
      set
      {
        this.baseTrans.LatePaymentDate = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets the date the payment was received.</summary>
    public override DateTime TransactionDate
    {
      get => this.baseTrans.PaymentReceivedDate;
      set
      {
        this.baseTrans.PaymentReceivedDate = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets the date the payment was deposited.</summary>
    public DateTime PaymentDepositedDate
    {
      get => this.baseTrans.PaymentDepositedDate;
      set
      {
        this.baseTrans.PaymentDepositedDate = value;
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets the current rate of the index used to calculate this payment in an adjustable mortgage.
    /// </summary>
    public Decimal IndexRate => Convert.ToDecimal(this.baseTrans.IndexRate);

    /// <summary>
    /// Gets the current interest rate on the loan used to calculate the payment.
    /// </summary>
    public Decimal InterestRate => Convert.ToDecimal(this.baseTrans.InterestRate);

    /// <summary>Gets the total amount due for the payment.</summary>
    public Decimal AmountDue => Convert.ToDecimal(this.baseTrans.TotalAmountDue);

    /// <summary>Gets or sets the total payment received.</summary>
    /// <remarks>
    /// This property represents the sum of the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Principal" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Escrow" />,
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Interest" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.LateFee" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.MiscFee" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.AdditionalPrincipal" />,
    /// and <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.AdditionalEscrow" /> values, which determine how the payment is to allocated.
    /// <p>If you set
    /// this property directly and its value is greater than the sum of the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Principal" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Escrow" />,
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Interest" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.LateFee" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.MiscFee" />
    /// and <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.AdditionalEscrow" /> values, the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.AdditionalPrincipal" /> amount will be
    /// adjusted to ensure that the above properties sum to the specified total transaction amount.</p>
    /// <p>If you set
    /// this property directly and its value is less than the sum of the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Principal" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Escrow" />,
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.Interest" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.LateFee" />, <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.MiscFee" />
    /// and <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.AdditionalEscrow" /> values, the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.AdditionalPrincipal" /> amount will be
    /// set to 0 and the Principal amount paid will be reduced by the difference in the amounts to
    /// ensure the transaction total is accurate.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to record a payment into the servicing
    ///       history of the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Add a new payment to the servicing history of the loan.
    ///       // The payment will be pre-populated with the data for the next scheduled payment.
    ///       Payment pmt = loan.Servicing.Transactions.AddPayment(DateTime.Now);
    /// 
    ///       // Adjust the transaction amount to the amount actually received from the customer
    ///       // If this is more than the scheduled payment amount, the balanced will be applied
    ///       // as an additional principal payment. If it's less, the portion of the
    ///       // payment allocated to principal will be reduced by the difference.
    ///       pmt.TransactionAmount = 2000;
    /// 
    ///       // Display the portion allocated to each balance
    ///       Console.WriteLine("Principal: " + pmt.Principal);
    ///       Console.WriteLine("Interest:  " + pmt.Interest);
    ///       Console.WriteLine("Escrow:    " + pmt.Escrow);
    ///       Console.WriteLine("Late Fee:  " + pmt.LateFee);
    ///       Console.WriteLine("Addl Prin: " + pmt.AdditionalPrincipal);
    ///       Console.WriteLine("Addl Escr: " + pmt.AdditionalEscrow);
    ///       Console.WriteLine("Misc Fees: " + pmt.MiscFee);
    /// 
    ///       // Close the loan, releasing its resources
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
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

    /// <summary>
    /// Gets or sets the amount of principal which is included in this payment.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the amount of interest included in this payment.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the portion of the payment which will be directed to the escrow account.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the late fee which is included in this payment.
    /// </summary>
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

    /// <summary>
    /// Gets or sets any extra fees which are included in this payment.
    /// </summary>
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

    /// <summary>
    /// Gets or sets any additional principal which included in this payment.
    /// </summary>
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

    /// <summary>
    /// Gets or sets any additional funds to be directed to the escrow account from this payment.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the late fee which will be applied if payment is not received by the
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.PaymentDueDate" />.
    /// </summary>
    public Decimal LateFeeIfLate
    {
      get => Convert.ToDecimal(this.baseTrans.LateFeeIfLate);
      set
      {
        this.baseTrans.LateFeeIfLate = Convert.ToDouble(value);
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the name of the institution from which payment is drawn.
    /// </summary>
    public string InstitutionName
    {
      get => this.baseTrans.InstitutionName;
      set
      {
        this.baseTrans.InstitutionName = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the routing number of the institution from which payment is drawn.
    /// </summary>
    public string InstitutionRouting
    {
      get => this.baseTrans.InstitutionRouting;
      set
      {
        this.baseTrans.InstitutionRouting = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the account number for which payment is drawn.
    /// </summary>
    public string AccountNumber
    {
      get => this.baseTrans.AccountNumber;
      set
      {
        this.baseTrans.AccountNumber = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the name on the account from which payment is drawn.
    /// </summary>
    public string AccountHolder
    {
      get => this.baseTrans.AccountHolder;
      set
      {
        this.baseTrans.AccountHolder = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets a reference number for this transaction.</summary>
    public string Reference
    {
      get => this.baseTrans.Reference;
      set
      {
        this.baseTrans.Reference = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the check number used for this payment if paid by check.
    /// </summary>
    public string CheckNumber
    {
      get => this.baseTrans.CheckNumber;
      set
      {
        this.baseTrans.CheckNumber = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the actual amount received from the borrower as payment.
    /// </summary>
    public Decimal PaymentAmount
    {
      get => Convert.ToDecimal(this.baseTrans.CheckAmount);
      set
      {
        this.baseTrans.CheckAmount = Convert.ToDouble(value);
        this.setLastModified();
      }
    }

    /// <summary>
    /// Gets or sets the date shown on the payment, e.g. the date on a check.
    /// </summary>
    /// <remarks>This property does nto represent the actual date the payment was received.
    /// That date is stored in the <see cref="P:EllieMae.EMLite.DataEngine.InterimServicing.PaymentTransactionLog.PaymentReceivedDate" /> property.</remarks>
    public DateTime PaymentDate
    {
      get => this.baseTrans.CheckDate;
      set
      {
        this.baseTrans.CheckDate = value;
        this.setLastModified();
      }
    }

    /// <summary>Gets or sets additional comments for the transaction.</summary>
    public string Comments
    {
      get => this.baseTrans.Comments;
      set
      {
        this.baseTrans.Comments = value ?? "";
        this.setLastModified();
      }
    }

    /// <summary>Determines if the current payment has been reversed.</summary>
    /// <returns>Returns <c>true</c> if the payment has been reversed, <c>false</c> otherwise.</returns>
    /// <remarks>You can use the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment.GetReversal" /> method to retrieve the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentReversal" /> object if this property returns <c>true</c>.</remarks>
    /// <example>
    ///       The following code demonstrates how to calculate the total amount
    ///       of principal paid on the loan by adding the principal payments together
    ///       from the servicing history.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Retrieve all Payment transactions from the loan and add up the total principal
    ///       // paid (excluding any payment that's been reversed).
    ///       ServicingTransactionList payments = loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.Payment);
    /// 
    ///       decimal principalPaid = 0;
    ///       foreach (Payment pmt in payments)
    ///         if (!pmt.IsReversed())
    ///           principalPaid += pmt.Principal;
    /// 
    ///       Console.WriteLine("Total Principal Paid: " + principalPaid);
    /// 
    ///       // Close the loan, releasing its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public bool IsReversed() => this.GetReversal() != null;

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentReversal" /> object for a payment which has been reversed.
    /// </summary>
    /// <returns>Returns the reversal object if the payment has been reversed or <c>null</c> if the
    /// payment has not been reversed.</returns>
    public PaymentReversal GetReversal()
    {
      foreach (PaymentReversal transaction in (CollectionBase) this.Loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.PaymentReversal))
      {
        if (((PaymentReversalLog) transaction.Unwrap()).PaymentGUID == this.ID)
          return transaction;
      }
      return (PaymentReversal) null;
    }

    /// <summary>
    /// Reverses the payment and restores the balance of the loan.
    /// </summary>
    /// <param name="reversalDate">The date on which the reversal took place.</param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentReversal" /> object representing
    /// the current reversal.</returns>
    /// <example>
    ///       The following code demonstrates how to reverse an existing payment in
    ///       the servicing transaction history.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
    /// 
    /// class SampleApp
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Open the loan using the GUID specified on the command line
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Lock the loan to enable editing
    ///       loan.Lock();
    /// 
    ///       // Prints the current balance of the loan
    ///       Console.WriteLine("Prior balance: " + loan.Fields["SERVICE.X57"].FormattedValue);
    /// 
    ///       // Retrieve the most recent payment from the servicing history
    ///       ServicingTransactionList payments = loan.Servicing.Transactions.GetTransactions(ServicingTransactionType.Payment);
    ///       Payment lastPayment = (Payment) payments[payments.Count - 1];
    /// 
    ///       // Write the amount of principal included in that payment
    ///       Console.WriteLine("Last payment included $" + lastPayment.Principal + " in principal");
    /// 
    ///       // Reverse the payment, which will re-apply the principal back to the loan balance.
    ///       lastPayment.Reverse(DateTime.Now);
    /// 
    ///       // Prints the updated balance of the loan.
    ///       Console.WriteLine("New balance: " + loan.Fields["SERVICE.X57"].FormattedValue);
    /// 
    ///       // Save the loan back to the server
    ///       loan.Commit();
    /// 
    ///       // Close the loan, releasing its resources
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public PaymentReversal Reverse(DateTime reversalDate)
    {
      if (this.IsReversed())
        throw new Exception("A reversal already exists for the current payment");
      PaymentReversalLog servicingTransaction = (PaymentReversalLog) this.Loan.Unwrap().CreateNextServicingTransaction(ServicingTransactionTypes.EscrowDisbursementReversal);
      servicingTransaction.TransactionDate = reversalDate;
      servicingTransaction.PaymentGUID = this.ID;
      servicingTransaction.TransactionAmount = Convert.ToDouble(-1M * this.TransactionAmount);
      servicingTransaction.ReversalType = ServicingTransactionTypes.PaymentReversal;
      this.Loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new PaymentReversal(this.Loan, servicingTransaction);
    }

    private PaymentTransactionLog baseTrans => (PaymentTransactionLog) this.Unwrap();

    private void adjustTransAmount()
    {
      this.baseTrans.TransactionAmount = Convert.ToDouble(this.Principal + this.Interest + this.Escrow + this.LateFee + this.MiscFee + this.AdditionalPrincipal + this.AdditionalEscrow);
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
