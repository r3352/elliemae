// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents the collection of fields that are stored for a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" />.
  /// </summary>
  /// <example>
  ///       The following code creates a new Lock Request and populates the buy-side
  ///       data from the request.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessEnums;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open and lock the loan
  ///       Loan loan = session.Loans.Open(args[0]);
  ///       loan.Lock();
  /// 
  ///       // Retrieve the current lock request, if any, from the loan
  ///       LockRequest req = loan.Log.LockRequests.Add();
  /// 
  ///       // Set the Buy side lock date and period
  ///       req.Fields["2149"].Value = DateTime.Today;
  ///       req.Fields["2150"].Value = 45;
  /// 
  ///       // Set the buy-side base rate and adjustments
  ///       req.Fields["2152"].Value = 6.75;
  ///       req.Fields["2153"].Value = "45 Day Lock Period";
  ///       req.Fields["2154"].Value = 0.125;
  /// 
  ///       // Set the buy-side base price and adjustments
  ///       req.Fields["2161"].Value = 99.75;
  ///       req.Fields["2162"].Value = "FICO >= 720";
  ///       req.Fields["2163"].Value = 0.25;
  /// 
  ///       // Force a re-calculation of the lock fields. We must call this method before
  ///       // retrieving the value of the Net Buy Price (2203) in order to ensure that field's
  ///       // value is up-to-date.
  ///       req.Fields.Recalculate();
  /// 
  ///       // Display the Net Buy Prices from the lock
  ///       Console.WriteLine("Net Buy Price: " + req.Fields["2203"].FormattedValue);
  /// 
  ///       // Commit the field changes back into the lock request
  ///       req.Fields.CommitChanges();
  /// 
  ///       // Save and close the loan file
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
  public class LockRequestFields : ILockRequestFields
  {
    private static FieldDescriptors snapshotFields = new FieldDescriptors();
    private LockRequest request;
    private Hashtable fieldValues;
    private LockRequestCalculator calculator;
    private Hashtable fields = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);

    static LockRequestFields()
    {
      foreach (string snapshotField in LockRequestLog.SnapshotFields)
      {
        if (LockRequestFields.snapshotFields[snapshotField] == null)
          LockRequestFields.snapshotFields.Add(FieldDescriptors.StandardFields[snapshotField]);
      }
      foreach (string requestField in LockRequestLog.RequestFields)
      {
        if (LockRequestFields.snapshotFields[requestField] == null)
          LockRequestFields.snapshotFields.Add(FieldDescriptors.StandardFields[requestField]);
      }
      foreach (string lockExtensionField in LockRequestLog.LockExtensionFields)
      {
        if (LockRequestFields.snapshotFields[lockExtensionField] == null)
          LockRequestFields.snapshotFields.Add(FieldDescriptors.StandardFields[lockExtensionField]);
      }
    }

    internal LockRequestFields(LockRequest request, Hashtable fieldValues)
    {
      this.request = request;
      this.fieldValues = fieldValues;
      this.calculator = new LockRequestCalculator(request.Loan.Session.SessionObjects, request.Loan.LoanData);
    }

    /// <summary>
    /// Gets the set of field descriptors which are stored when a lock request snapshot is made.
    /// </summary>
    public FieldDescriptors Descriptors => LockRequestFields.snapshotFields;

    /// <summary>
    /// Gets the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanField">LoanField</see> based on the
    /// field ID provided.
    /// </summary>
    public LockRequestField this[string fieldId]
    {
      get
      {
        LockRequestField field = (LockRequestField) this.fields[(object) (fieldId ?? "")];
        if (field != null)
          return field;
        LockRequestField lockRequestField = new LockRequestField(this.fieldValues, this.request.Loan.Session.Loans.FieldDescriptors[fieldId] ?? throw new ArgumentException("The specified field ID is invalid", nameof (fieldId)));
        this.fields.Add((object) fieldId, (object) lockRequestField);
        return lockRequestField;
      }
    }

    /// <summary>
    /// Forces a recalculation of the calculated lock request fields.
    /// </summary>
    /// <remarks>The lock request fields do not automatically recalculate when a field's value is changed.
    /// Typically, the calculated field values are updated when you call <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestFields.CommitChanges" />,
    /// but there may be cases when you need to see the results of the calculations prior to
    /// calling that method. You can use this method to force a recalculation of the fields within
    /// the lock request at any time.</remarks>
    /// <example>
    ///       The following code creates a new Lock Request and populates the buy-side
    ///       data from the request.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.Add();
    /// 
    ///       // Set the Buy side lock date and period
    ///       req.Fields["2149"].Value = DateTime.Today;
    ///       req.Fields["2150"].Value = 45;
    /// 
    ///       // Set the buy-side base rate and adjustments
    ///       req.Fields["2152"].Value = 6.75;
    ///       req.Fields["2153"].Value = "45 Day Lock Period";
    ///       req.Fields["2154"].Value = 0.125;
    /// 
    ///       // Set the buy-side base price and adjustments
    ///       req.Fields["2161"].Value = 99.75;
    ///       req.Fields["2162"].Value = "FICO >= 720";
    ///       req.Fields["2163"].Value = 0.25;
    /// 
    ///       // Force a re-calculation of the lock fields. We must call this method before
    ///       // retrieving the value of the Net Buy Price (2203) in order to ensure that field's
    ///       // value is up-to-date.
    ///       req.Fields.Recalculate();
    /// 
    ///       // Display the Net Buy Prices from the lock
    ///       Console.WriteLine("Net Buy Price: " + req.Fields["2203"].FormattedValue);
    /// 
    ///       // Commit the field changes back into the lock request
    ///       req.Fields.CommitChanges();
    /// 
    ///       // Save and close the loan file
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
    public void Recalculate() => this.calculator.PerformSnapshotCalculations(this.fieldValues);

    /// <summary>
    /// Commits the changes made to the lock request snapshot field collection.
    /// </summary>
    /// <remarks>
    /// When this collection of fields is modified, the changes are not saved unless you call CommitChanges,
    /// which pushes the modifications into the loan file. This method should be invoked before
    /// calling the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Commit" /> method to save the loan to the server.
    /// </remarks>
    /// <example>
    ///       The following code creates a new Lock Request and populates the buy-side
    ///       data from the request.
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessEnums;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the loan
    ///       Loan loan = session.Loans.Open(args[0]);
    ///       loan.Lock();
    /// 
    ///       // Retrieve the current lock request, if any, from the loan
    ///       LockRequest req = loan.Log.LockRequests.Add();
    /// 
    ///       // Set the Buy side lock date and period
    ///       req.Fields["2149"].Value = DateTime.Today;
    ///       req.Fields["2150"].Value = 45;
    /// 
    ///       // Set the buy-side base rate and adjustments
    ///       req.Fields["2152"].Value = 6.75;
    ///       req.Fields["2153"].Value = "45 Day Lock Period";
    ///       req.Fields["2154"].Value = 0.125;
    /// 
    ///       // Set the buy-side base price and adjustments
    ///       req.Fields["2161"].Value = 99.75;
    ///       req.Fields["2162"].Value = "FICO >= 720";
    ///       req.Fields["2163"].Value = 0.25;
    /// 
    ///       // Force a re-calculation of the lock fields. We must call this method before
    ///       // retrieving the value of the Net Buy Price (2203) in order to ensure that field's
    ///       // value is up-to-date.
    ///       req.Fields.Recalculate();
    /// 
    ///       // Display the Net Buy Prices from the lock
    ///       Console.WriteLine("Net Buy Price: " + req.Fields["2203"].FormattedValue);
    /// 
    ///       // Commit the field changes back into the lock request
    ///       req.Fields.CommitChanges();
    /// 
    ///       // Save and close the loan file
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
    public void CommitChanges()
    {
      this.Recalculate();
      this.request.LockRequestLog.AddLockRequestSnapshot(this.fieldValues);
    }

    /// <summary>Retrieves the underlying field Hashtable</summary>
    internal Hashtable FieldTable => this.fieldValues;
  }
}
