// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.LoanServicingTransactions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Provides access to the collection of servicing transactions on a loan.
  /// </summary>
  /// <example>
  ///       The following code demonstrates how to add a new payment into the servicing
  ///       history of a loan. It also displays the outstanding balance of the loan
  ///       both before and after the payment is applied.
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
  ///       // Display the balance of the loan prior to the payment
  ///       Console.WriteLine("Prior balance: " + loan.Fields["SERVICE.X57"].FormattedValue);
  /// 
  ///       // Add a new payment to the servicing history of the loan
  ///       Payment pmt = loan.Servicing.Transactions.AddPayment(DateTime.Now);
  /// 
  ///       // Set the properties of the payment to match what was received
  ///       pmt.Principal = 700;
  ///       pmt.Interest = 1500;
  ///       pmt.Escrow = 250;
  ///       pmt.AdditionalPrincipal = 150;
  /// 
  ///       // Indicate the manner in which payment was made
  ///       pmt.PaymentMethod = ServicingPaymentMethod.AutomatedClearingHouse;
  ///       pmt.AccountHolder = "Janet T. Barnes";
  ///       pmt.AccountNumber = "10339442-4";
  ///       pmt.InstitutionName = "First Federated Bank";
  ///       pmt.InstitutionRouting = "444444444";
  ///       pmt.Reference = "AB3-4434-223";
  /// 
  ///       // Check the new balance due
  ///       Console.WriteLine("New balance: " + loan.Fields["SERVICE.X57"].FormattedValue);
  /// 
  ///       // Save the loan to commit this new payment
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
  public class LoanServicingTransactions : ILoanServicingTransactions, IEnumerable
  {
    private Loan loan;

    internal LoanServicingTransactions(Loan loan) => this.loan = loan;

    /// <summary>Retrieves a transaction by its unique ID.</summary>
    /// <param name="transId">The unique ID of the transaction to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.ServicingTransaction" />.</returns>
    public ServicingTransaction GetTransactionByID(string transId)
    {
      foreach (ServicingTransaction transactionById in this)
      {
        if (transactionById.ID == transId)
          return transactionById;
      }
      return (ServicingTransaction) null;
    }

    /// <summary>Retrieves all transactions of a specified type.</summary>
    /// <param name="transType">The type of transaction to be retrieved.</param>
    /// <returns>Returns a collection containing the requested transactions.</returns>
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
    public ServicingTransactionList GetTransactions(ServicingTransactionType transType)
    {
      ServicingTransactionList transactions = new ServicingTransactionList();
      foreach (ServicingTransaction servicingTransaction in this)
      {
        if (servicingTransaction.TransactionType == transType)
          transactions.Add(servicingTransaction);
      }
      return transactions;
    }

    /// <summary>Adds a payment transaction to the servicing history.</summary>
    /// <param name="dateReceived">The date of the statement that corresponds to the payment.</param>
    /// <returns>Returns a new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment" /> object for this transaction.</returns>
    /// <remarks>The returned Payment object will be pre-populated with the balance and payment
    /// information based on the next scheduled payment. This data can then be overridden to match
    /// the actual payment details.</remarks>
    /// <example>
    ///       The following code demonstrates how to add a new payment into the servicing
    ///       history of a loan. It also displays the outstanding balance of the loan
    ///       both before and after the payment is applied.
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
    ///       // Display the balance of the loan prior to the payment
    ///       Console.WriteLine("Prior balance: " + loan.Fields["SERVICE.X57"].FormattedValue);
    /// 
    ///       // Add a new payment to the servicing history of the loan
    ///       Payment pmt = loan.Servicing.Transactions.AddPayment(DateTime.Now);
    /// 
    ///       // Set the properties of the payment to match what was received
    ///       pmt.Principal = 700;
    ///       pmt.Interest = 1500;
    ///       pmt.Escrow = 250;
    ///       pmt.AdditionalPrincipal = 150;
    /// 
    ///       // Indicate the manner in which payment was made
    ///       pmt.PaymentMethod = ServicingPaymentMethod.AutomatedClearingHouse;
    ///       pmt.AccountHolder = "Janet T. Barnes";
    ///       pmt.AccountNumber = "10339442-4";
    ///       pmt.InstitutionName = "First Federated Bank";
    ///       pmt.InstitutionRouting = "444444444";
    ///       pmt.Reference = "AB3-4434-223";
    /// 
    ///       // Check the new balance due
    ///       Console.WriteLine("New balance: " + loan.Fields["SERVICE.X57"].FormattedValue);
    /// 
    ///       // Save the loan to commit this new payment
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
    public Payment AddPayment(DateTime dateReceived)
    {
      this.loan.EnsureExclusive();
      PaymentTransactionLog servicingTransaction = (PaymentTransactionLog) this.loan.Unwrap().CreateNextServicingTransaction(ServicingTransactionTypes.Payment);
      this.loan.Unwrap().PopulateNextServicingPaymentInformation(servicingTransaction);
      servicingTransaction.PaymentReceivedDate = dateReceived;
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new Payment(this.loan, servicingTransaction);
    }

    /// <summary>
    /// Adds a transaction for a disbursement from the escrow account.
    /// </summary>
    /// <param name="transDate">The date on which the disbursement was made.</param>
    /// <param name="disAmount">The amount of the disbursement.</param>
    /// <param name="disType">The type of disbursement made.</param>
    /// <returns>Returns as <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursement" /> object which represents
    /// the new transaction.</returns>
    /// <example>
    ///       The following code demonstrates how to add an escrow disbursement to
    ///       the servicing history of a loan.
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
    /// 			// Create a new EscrowDisbursement transaction for the loan to represent
    /// 			// a $450 payment of hazard insurance.
    /// 			EscrowDisbursement disb = loan.Servicing.Transactions.AddDisbursement(DateTime.Now, 450,
    /// 				ServicingDisbursementType.HazardInsurance);
    /// 
    /// 			disb.InstitutionName = "State Farm Insurance";
    /// 			disb.DisbursementDueDate = DateTime.Parse("6/5/2008");
    /// 			disb.Comments = "Covers 3 month period from 6/1/08 - 8/31/08";
    /// 
    ///       // Save the loan to commit this new payment
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
    /// </code>
    /// </example>
    public EscrowDisbursement AddDisbursement(
      DateTime transDate,
      Decimal disAmount,
      ServicingDisbursementType disType)
    {
      this.loan.EnsureExclusive();
      EscrowDisbursementLog servicingTransaction = (EscrowDisbursementLog) this.loan.Unwrap().CreateNextServicingTransaction(ServicingTransactionTypes.EscrowDisbursement);
      servicingTransaction.DisbursementType = (ServicingDisbursementTypes) disType;
      servicingTransaction.TransactionDate = transDate;
      servicingTransaction.TransactionAmount = Convert.ToDouble(disAmount);
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new EscrowDisbursement(this.loan, servicingTransaction);
    }

    /// <summary>
    /// Adds a transaction for interest accrued on the escrow account balance.
    /// </summary>
    /// <param name="transDate">The date on which the interest was paid.</param>
    /// <param name="transAmount">The amount of the interest payment.</param>
    /// <returns>Returns an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowInterest" /> object which represents
    /// the new transaction.</returns>
    public EscrowInterest AddEscrowInterest(DateTime transDate, Decimal transAmount)
    {
      this.loan.EnsureExclusive();
      EscrowInterestLog servicingTransaction = (EscrowInterestLog) this.loan.Unwrap().CreateNextServicingTransaction(ServicingTransactionTypes.EscrowInterest);
      servicingTransaction.TransactionDate = transDate;
      servicingTransaction.TransactionAmount = Convert.ToDouble(transAmount);
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new EscrowInterest(this.loan, servicingTransaction);
    }

    /// <summary>
    /// Adds a user-defined transaction to the servicing history.
    /// </summary>
    /// <param name="transDate">The date on which the transaction occurred.</param>
    /// <param name="transAmount">The amount of the transaction.</param>
    /// <returns>Returns an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.OtherTransaction" /> object which represents
    /// the new transaction.</returns>
    public OtherTransaction AddOtherTransaction(DateTime transDate, Decimal transAmount)
    {
      this.loan.EnsureExclusive();
      OtherTransactionLog servicingTransaction = (OtherTransactionLog) this.loan.Unwrap().CreateNextServicingTransaction(ServicingTransactionTypes.Other);
      servicingTransaction.TransactionDate = transDate;
      servicingTransaction.TransactionAmount = Convert.ToDouble(transAmount);
      this.loan.Unwrap().LoanData.AddServicingTransaction((ServicingTransactionBase) servicingTransaction);
      return new OtherTransaction(this.loan, servicingTransaction);
    }

    /// <summary>
    /// Removes a transaction from the servicing history of the loan.
    /// </summary>
    /// <param name="trans">The transaction to be removed.</param>
    /// <remarks>If the transaction being removed is a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.Payment" /> (or
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursement" />) transaction for which a
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentReversal" /> (or <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.EscrowDisbursementReversal" />)
    /// transaction exists, the reversal transaction will also be removed from the
    /// servicing history as well.</remarks>
    public void Remove(ServicingTransaction trans)
    {
      this.loan.EnsureExclusive();
      this.loan.Unwrap().LoanData.RemoveServicingTransaction(trans.Unwrap());
    }

    /// <summary>
    /// Provides an enumerator to iterate over the set of servicing transactions.
    /// </summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.createTransactionCollection().GetEnumerator();

    private ArrayList createTransactionCollection()
    {
      ArrayList transactionCollection = new ArrayList();
      ServicingTransactionBase[] servicingTransactions = this.loan.Unwrap().LoanData.GetServicingTransactions(true);
      if (servicingTransactions != null)
      {
        foreach (ServicingTransactionBase servicingTransactionBase in servicingTransactions)
        {
          ServicingTransaction servicingTransaction = ServicingTransaction.Wrap(this.loan, servicingTransactionBase);
          if (servicingTransaction != null)
            transactionCollection.Add((object) servicingTransaction);
        }
      }
      return transactionCollection;
    }
  }
}
