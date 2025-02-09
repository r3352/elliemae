// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAuditTrail
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides access to Audit Trail data for the current loan.
  /// </summary>
  /// <remarks>When a field is added to the Encompass Audit Trail through the Admin Tools/Reporting
  /// Database interface, Encompass will store every change to the field along with the identity
  /// of the user who made teh change and the date/time of the change. This class provides the
  /// methods for retrieving the audit trail history for the loan.</remarks>
  /// <example>
  ///       The following code demonstrates how to retrieve the list of all fields
  ///       included in the Audit Trail and, for each field, display the history of changes
  ///       to its value.
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
  ///       // Iterate over the list of fields that are included in the audit trail
  ///       foreach (string fieldId in loan.AuditTrail.GetAuditFieldList())
  ///       {
  ///         Console.WriteLine("Audit trail for field " + fieldId);
  /// 
  ///         // Retrieve the history for the current field
  ///         AuditTrailEntryList entries = loan.AuditTrail.GetHistory(fieldId);
  /// 
  ///         // Iterate over the historical changes and print the time of the change and
  ///         // the user's identity that made the change.
  ///         foreach (AuditTrailEntry e in entries)
  ///           Console.WriteLine("   -> " + e.Timestamp + " by " + e.UserName + " (" + e.UserID + ") -> " + e.Field.FormattedValue);
  /// 
  ///         Console.WriteLine();
  ///       }
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
  public class LoanAuditTrail : ILoanAuditTrail
  {
    private Loan loan;

    internal LoanAuditTrail(Loan loan) => this.loan = loan;

    /// <summary>Returns the full audit trail history for a field.</summary>
    /// <param name="fieldId">The ID of the auditable field.</param>
    /// <returns>Returns an AuditTrailEntryList object containing the history of the modifications
    /// to the specified field.</returns>
    /// <remarks>The specified field must be marked for inclusion in the Audit Trail, otherwise
    /// this method will throw an ArgumentException. The list that is returned will be sorted
    /// chronologically with the most recent change first.</remarks>
    /// <example>
    ///       The following code demonstrates how to retrieve the list of all fields
    ///       included in the Audit Trail and, for each field, display the history of changes
    ///       to its value.
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
    ///       // Iterate over the list of fields that are included in the audit trail
    ///       foreach (string fieldId in loan.AuditTrail.GetAuditFieldList())
    ///       {
    ///         Console.WriteLine("Audit trail for field " + fieldId);
    /// 
    ///         // Retrieve the history for the current field
    ///         AuditTrailEntryList entries = loan.AuditTrail.GetHistory(fieldId);
    /// 
    ///         // Iterate over the historical changes and print the time of the change and
    ///         // the user's identity that made the change.
    ///         foreach (AuditTrailEntry e in entries)
    ///           Console.WriteLine("   -> " + e.Timestamp + " by " + e.UserName + " (" + e.UserID + ") -> " + e.Field.FormattedValue);
    /// 
    ///         Console.WriteLine();
    ///       }
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
    public AuditTrailEntryList GetHistory(string fieldId)
    {
      try
      {
        return AuditTrailEntry.ToList(this.loan.Session, this.loan.Session.SessionObjects.LoanManager.GetAuditRecords(this.loan.Guid, fieldId));
      }
      catch (ObjectNotFoundException ex)
      {
        throw new Exception("The field '" + fieldId + "' is not in the Audit Trail.");
      }
    }

    /// <summary>Returns the full audit trail history for a field.</summary>
    /// <param name="fieldIds">The List of IDs of the auditable fields.</param>
    /// <returns>Returns an AuditTrailEntryList object containing the history of the modifications
    /// to the specified field.</returns>
    /// <remarks>The specified field must be marked for inclusion in the Audit Trail, otherwise
    /// this method will throw an ArgumentException. The list that is returned will be sorted
    /// chronologically with the most recent change first.</remarks>
    /// <example>
    ///       The following code demonstrates how to retrieve the list of all fields
    ///       included in the Audit Trail and, for each field, display the history of changes
    ///       to its value.
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
    ///       // Iterate over the list of fields that are included in the audit trail
    ///       foreach (string fieldId in loan.AuditTrail.GetAuditFieldList())
    ///       {
    ///         Console.WriteLine("Audit trail for field " + fieldId);
    /// 
    ///         // Retrieve the history for the current field
    ///         AuditTrailEntryList entries = loan.AuditTrail.GetHistory(fieldId);
    /// 
    ///         // Iterate over the historical changes and print the time of the change and
    ///         // the user's identity that made the change.
    ///         foreach (AuditTrailEntry e in entries)
    ///           Console.WriteLine("   -> " + e.Timestamp + " by " + e.UserName + " (" + e.UserID + ") -> " + e.Field.FormattedValue);
    /// 
    ///         Console.WriteLine();
    ///       }
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
    public AuditTrailEntryList GetHistory(string[] fieldIds)
    {
      try
      {
        return AuditTrailEntry.ToList(this.loan.Session, this.loan.Session.SessionObjects.LoanManager.GetAuditRecords(this.loan.Guid, fieldIds));
      }
      catch (ObjectNotFoundException ex)
      {
        throw new Exception("The fields are not in the Audit Trail. Please check the input");
      }
    }

    /// <summary>
    /// Gets the audit information for the most recent change to a field.
    /// </summary>
    /// <param name="fieldId">The ID of the auditable field.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.AuditTrailEntry" /> for the most recent change to the
    /// specified field.</returns>
    /// <remarks>The specified field must be marked for inclusion in the Audit Trail, otherwise
    /// this method will throw an ArgumentException.</remarks>
    /// <example>
    ///       The following code demonstrates how to retrieve the most recent change
    ///       events for each field in the audit trail.
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
    ///       // Get the most recent change for each field in the audit trail
    ///       AuditTrailEntryList auditEntries = loan.AuditTrail.GetMostRecentEntries();
    /// 
    ///       // Display each field ID and the identity of the user who made the most recent change
    ///       foreach (AuditTrailEntry entry in auditEntries)
    ///         Console.WriteLine("Field " + entry.Field.ID + " last changed " + entry.Timestamp
    ///           + " by " + entry.UserID);
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
    public AuditTrailEntry GetMostRecentEntry(string fieldId) => this.GetHistory(fieldId)[0];

    /// <summary>
    /// Gets the audit information for the most recent changes to al auditable fields.
    /// </summary>
    /// <returns>Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.AuditTrailEntry" /> objects for the most recent changes to
    /// all fields currently set to be auditable.</returns>
    /// <example>
    ///       The following code demonstrates how to retrieve the most recent change
    ///       events for each field in the audit trail.
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
    ///       // Get the most recent change for each field in the audit trail
    ///       AuditTrailEntryList auditEntries = loan.AuditTrail.GetMostRecentEntries();
    /// 
    ///       // Display each field ID and the identity of the user who made the most recent change
    ///       foreach (AuditTrailEntry entry in auditEntries)
    ///         Console.WriteLine("Field " + entry.Field.ID + " last changed " + entry.Timestamp
    ///           + " by " + entry.UserID);
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
    public AuditTrailEntryList GetMostRecentEntries()
    {
      Dictionary<string, AuditRecord> auditRecordsForLoan = this.loan.Session.SessionObjects.LoanManager.GetLastAuditRecordsForLoan(this.loan.Guid);
      AuditRecord[] auditRecordArray = new AuditRecord[auditRecordsForLoan.Count];
      auditRecordsForLoan.Values.CopyTo(auditRecordArray, 0);
      return AuditTrailEntry.ToList(this.loan.Session, auditRecordArray);
    }

    /// <summary>
    /// Returns the list of Field IDs which are part of the audit trail.
    /// </summary>
    /// <returns>Returns a StringList containing the IDs of the fields that are marked for
    /// inclusion in the Audit Trail.</returns>
    /// <example>
    ///       The following code demonstrates how to retrieve the list of all fields
    ///       included in the Audit Trail and, for each field, display the history of changes
    ///       to its value.
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
    ///       // Iterate over the list of fields that are included in the audit trail
    ///       foreach (string fieldId in loan.AuditTrail.GetAuditFieldList())
    ///       {
    ///         Console.WriteLine("Audit trail for field " + fieldId);
    /// 
    ///         // Retrieve the history for the current field
    ///         AuditTrailEntryList entries = loan.AuditTrail.GetHistory(fieldId);
    /// 
    ///         // Iterate over the historical changes and print the time of the change and
    ///         // the user's identity that made the change.
    ///         foreach (AuditTrailEntry e in entries)
    ///           Console.WriteLine("   -> " + e.Timestamp + " by " + e.UserName + " (" + e.UserID + ") -> " + e.Field.FormattedValue);
    /// 
    ///         Console.WriteLine();
    ///       }
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
    public StringList GetAuditFieldList()
    {
      LoanXDBField[] trailLoanXdbField = this.loan.Session.SessionObjects.LoanManager.GetAuditTrailLoanXDBField();
      StringList auditFieldList = new StringList();
      foreach (LoanXDBField loanXdbField in trailLoanXdbField)
        auditFieldList.Add(loanXdbField.FieldIDWithCoMortgagor);
      return auditFieldList;
    }
  }
}
