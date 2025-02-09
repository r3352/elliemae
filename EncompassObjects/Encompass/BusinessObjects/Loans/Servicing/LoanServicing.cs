// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Servicing.LoanServicing
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Servicing
{
  /// <summary>
  /// Provides an interface into the servicing features on a loan.
  /// </summary>
  /// <example>
  ///       The following code demonstrates how to activate the servicing activities
  ///       for a loan and display the payment schedule for the loan.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
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
  ///       // Set the first payment date on the loan. This is a required field before
  ///       // starting servicing since it determines the payment schedule for the loan
  ///       loan.Fields["682"].Value = "6/1/2008";
  /// 
  ///       // First, activate the servicing if not already started. This will calculate
  ///       // the initial payment schedule for the loan.
  ///       if (!loan.Servicing.IsStarted())
  ///         loan.Servicing.Start();
  /// 
  ///       // Display the payment schedule on the screen, showing the date of each
  ///       // payment along with the amount of principal and interest due.
  ///       PaymentSchedule schedule = loan.Servicing.GetPaymentSchedule();
  /// 
  ///       foreach (ScheduledPayment payment in schedule.Payments)
  ///         Console.WriteLine(payment.DueDate + ": P = " + payment.Principal + ", I = " + payment.Interest);
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
  public class LoanServicing : ILoanServicing
  {
    private Loan loan;
    private LoanServicingTransactions transactions;
    private bool isStarted;

    internal LoanServicing(Loan loan)
    {
      this.loan = loan;
      this.transactions = new LoanServicingTransactions(loan);
    }

    /// <summary>
    /// Determines if loan servicing has been started for this loan.
    /// </summary>
    /// <returns>Returns <c>true</c> if servicing has been previously enabled, false otherwise.</returns>
    /// <example>
    ///       The following code demonstrates how to activate the servicing activities
    ///       for a loan and display the payment schedule for the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Set the first payment date on the loan. This is a required field before
    ///       // starting servicing since it determines the payment schedule for the loan
    ///       loan.Fields["682"].Value = "6/1/2008";
    /// 
    ///       // First, activate the servicing if not already started. This will calculate
    ///       // the initial payment schedule for the loan.
    ///       if (!loan.Servicing.IsStarted())
    ///         loan.Servicing.Start();
    /// 
    ///       // Display the payment schedule on the screen, showing the date of each
    ///       // payment along with the amount of principal and interest due.
    ///       PaymentSchedule schedule = loan.Servicing.GetPaymentSchedule();
    /// 
    ///       foreach (ScheduledPayment payment in schedule.Payments)
    ///         Console.WriteLine(payment.DueDate + ": P = " + payment.Principal + ", I = " + payment.Interest);
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
    public bool IsStarted()
    {
      if (this.isStarted)
        return true;
      this.isStarted = this.loan.Unwrap().LoanData.GetPaymentScheduleSnapshot() != null;
      return this.isStarted;
    }

    /// <summary>Starts the servicing process for the loan.</summary>
    /// <remarks><p>The Start method must be called once on the loan prior to processing transactions
    /// to enable the servicing features. If Start is called a second time, it will be ignored.</p>
    /// <p>This method is supported in Banker Edition only.</p>
    /// </remarks>
    /// <example>
    ///       The following code demonstrates how to activate the servicing activities
    ///       for a loan and display the payment schedule for the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Set the first payment date on the loan. This is a required field before
    ///       // starting servicing since it determines the payment schedule for the loan
    ///       loan.Fields["682"].Value = "6/1/2008";
    /// 
    ///       // First, activate the servicing if not already started. This will calculate
    ///       // the initial payment schedule for the loan.
    ///       if (!loan.Servicing.IsStarted())
    ///         loan.Servicing.Start();
    /// 
    ///       // Display the payment schedule on the screen, showing the date of each
    ///       // payment along with the amount of principal and interest due.
    ///       PaymentSchedule schedule = loan.Servicing.GetPaymentSchedule();
    /// 
    ///       foreach (ScheduledPayment payment in schedule.Payments)
    ///         Console.WriteLine(payment.DueDate + ": P = " + payment.Principal + ", I = " + payment.Interest);
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
    public void Start()
    {
      if (this.IsStarted())
        return;
      if (!this.loan.Session.SessionObjects.ServerLicense.IsBankerEdition)
        throw new NotSupportedException("The specified operation is not supported by the current version of Encompass");
      string unformattedValue = this.loan.Fields["SERVICE.X8"].UnformattedValue;
      if (unformattedValue == "Foreclosure" || unformattedValue == "Servicing Released")
        throw new InvalidOperationException("Servicing cannot be started on a loan that has been foreclosure or released.");
      if (this.loan.Fields["682"].IsEmpty())
        throw new InvalidOperationException("First Payment Date (field 682) must be set prior to start of servicing.");
      this.loan.EnsureExclusive();
      this.loan.Unwrap().LoanData.StartInterimServicing();
      this.isStarted = true;
    }

    /// <summary>
    /// Gets the collection of servicing transactions from the loan.
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
    public LoanServicingTransactions Transactions
    {
      get
      {
        if (!this.IsStarted())
          throw new InvalidOperationException("Servicing has not been started for this loan. Call Start() method first.");
        return this.transactions;
      }
    }

    /// <summary>
    /// Forces a recalculation of the interim servicing payment schedule.
    /// </summary>
    public void Recalculate()
    {
      this.loan.EnsureExclusive();
      this.loan.Unwrap().Calculator.CalculateInterimServicing(true);
    }

    /// <summary>Gets the current payment schedule for the loan.</summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Servicing.PaymentSchedule" /> object with the loan's current payment
    /// schedule.</returns>
    /// <example>
    ///       The following code demonstrates how to activate the servicing activities
    ///       for a loan and display the payment schedule for the loan.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
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
    ///       // Set the first payment date on the loan. This is a required field before
    ///       // starting servicing since it determines the payment schedule for the loan
    ///       loan.Fields["682"].Value = "6/1/2008";
    /// 
    ///       // First, activate the servicing if not already started. This will calculate
    ///       // the initial payment schedule for the loan.
    ///       if (!loan.Servicing.IsStarted())
    ///         loan.Servicing.Start();
    /// 
    ///       // Display the payment schedule on the screen, showing the date of each
    ///       // payment along with the amount of principal and interest due.
    ///       PaymentSchedule schedule = loan.Servicing.GetPaymentSchedule();
    /// 
    ///       foreach (ScheduledPayment payment in schedule.Payments)
    ///         Console.WriteLine(payment.DueDate + ": P = " + payment.Principal + ", I = " + payment.Interest);
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
    public PaymentSchedule GetPaymentSchedule()
    {
      return new PaymentSchedule(this.loan.Unwrap().LoanData.GetPaymentScheduleSnapshot() ?? throw new InvalidOperationException("Servicing has not been started on this loan. Call the Start() method first."));
    }
  }
}
