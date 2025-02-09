// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single Document Tracking entry associated with a Loan.
  /// </summary>
  /// <remarks>The Date property of a TrackedDocument represents either
  /// the date on which the document is due to be received or the day the
  /// document actually was received.
  /// <p>TrackedDocument instances become invalid
  /// when the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Refresh">Refresh</see> method is
  /// invoked on the parent <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> object. Attempting
  /// to access this object after invoking refresh will result in an
  /// exception.</p>
  /// </remarks>
  /// <example>
  /// The following code retrieves the set of all supporting documents associated
  /// with a loan and generates a report for all documents that are overdue.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Open a loan for reading
  ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
  /// 
  ///       // Look for any supporting document which is overdue
  ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
  ///       {
  ///          if (doc.DueDate != null)
  ///          {
  ///             if ((DateTime) doc.DueDate < DateTime.Now)
  ///                Console.WriteLine("The document \"" + doc.Title + "\" is overdue!");
  ///          }
  ///       }
  /// 
  ///       loan.Close();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class TrackedDocument : LogEntry, ITrackedDocument
  {
    private Loan loan;
    private DocumentLog docItem;
    private RoleAccessList accessList;
    private EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments comments;

    internal TrackedDocument(Loan loan, DocumentLog docItem)
      : base(loan, (LogRecordBase) docItem)
    {
      this.loan = loan;
      this.docItem = docItem;
      this.accessList = new RoleAccessList(loan, this.docItem);
      this.comments = new EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments((LogEntry) this, docItem.Comments);
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current item.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.TrackedDocument" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.TrackedDocument;

    /// <summary>Gets the title of the supporting document.</summary>
    /// <example>
    /// The following code sets the order date to today for all Document Events
    /// associated with the "Submittal" milestone. Once the OrderDate property has been
    /// set, the DueDate property is printed to indicate when the document should
    /// be received.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Look for any supporting document which is overdue
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Mark all of the documents associated with the Submittal milestone as ordered
    ///          if (doc.MilestoneName == session.Loans.Milestones.Submittal.Name)
    ///          {
    ///             // Set the order date to today and allow 10 days to be received
    ///             doc.OrderDate = DateTime.Now;
    ///             doc.DueDays = 10;
    /// 
    ///             // Write a message showing the date by which the document is due
    ///             Console.WriteLine("Document \"" + doc.Title + "\" has been ordered and is due on " + doc.DueDate);
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string Title
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Title;
      }
      set
      {
        this.EnsureEditable();
        if ((value ?? "") == "")
          throw new ArgumentException("Document title cannot be blank or null");
        this.docItem.Title = value ?? "";
      }
    }

    /// <summary>Gets/sets the description of the supporting document.</summary>
    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Description;
      }
      set
      {
        this.EnsureEditable();
        this.docItem.Description = value ?? "";
      }
    }

    /// <summary>Gets the eFolder type of the document.</summary>
    /// <remarks>
    /// This property returns one of the following values:
    /// <list type="bullet">
    /// <item>Settlement Service</item>
    /// <item>Closing Document</item>
    /// <item>eDisclosure</item>
    /// <item>Verification</item>
    /// <item>Needed</item>
    /// <item>Standard Form</item>
    /// <item>Custom Form</item>
    /// </list>
    /// </remarks>
    public string DocumentType
    {
      get
      {
        return this.docItem.GetDocumentType(this.Loan.Unwrap().SystemConfiguration.DocumentTrackingSetup);
      }
    }

    /// <summary>
    /// Gets or sets the company name associated with the supporting document.
    /// </summary>
    public string Company
    {
      get
      {
        this.EnsureValid();
        return this.docItem.RequestedFrom;
      }
      set
      {
        this.EnsureEditable();
        this.docItem.RequestedFrom = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="T:EllieMae.Encompass.BusinessEnums.Milestone">Milestone</see> associated with the supporting document.
    /// </summary>
    /// <remarks>If you attempt to set this property to null, an exception will be thrown.
    /// </remarks>
    /// <example>
    /// The following code sets the order date to today for all Document Events
    /// associated with the "Submittal" milestone. Once the OrderDate property has been
    /// set, the DueDate property is printed to indicate when the document should
    /// be received.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Look for any supporting document which is overdue
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Mark all of the documents associated with the Submittal milestone as ordered
    ///          if (doc.MilestoneName == session.Loans.Milestones.Submittal.Name)
    ///          {
    ///             // Set the order date to today and allow 10 days to be received
    ///             doc.OrderDate = DateTime.Now;
    ///             doc.DueDays = 10;
    /// 
    ///             // Write a message showing the date by which the document is due
    ///             Console.WriteLine("Document \"" + doc.Title + "\" has been ordered and is due on " + doc.DueDate);
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string MilestoneName
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Stage;
      }
      set
      {
        this.EnsureEditable();
        if ((value ?? "") == null)
          throw new ArgumentException("Milestone name cannot be blank or null");
        this.docItem.Stage = (this.loan.Log.MilestoneEvents.GetEventForMilestone(value) ?? throw new ArgumentException("Current loan does not have a milestone with the name \"" + value + "\"")).InternalName;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.BorrowerPair">BorrowerPair</see> with which this document is associated.
    /// </summary>
    /// <example>
    /// The following code displays the titles of all of the Supporting Documents
    /// associated with the loan's current borrower pair.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan for reading
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Loop over the list of supporting documents
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Get the borrower pair with which the document is associated
    ///          if (doc.BorrowerPair == loan.BorrowerPairs.Current)
    ///             Console.WriteLine(doc.Title);
    ///       }
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public BorrowerPair BorrowerPair
    {
      get
      {
        this.EnsureValid();
        foreach (BorrowerPair borrowerPair in this.loan.BorrowerPairs)
        {
          if (borrowerPair.Borrower.ID == this.docItem.PairId)
            return borrowerPair;
        }
        return (BorrowerPair) null;
      }
      set
      {
        this.EnsureValid();
        if (value == (BorrowerPair) null)
        {
          this.docItem.PairId = (string) null;
        }
        else
        {
          foreach (BorrowerPair borrowerPair in this.loan.BorrowerPairs)
          {
            if (borrowerPair.Borrower.ID == value.Borrower.ID)
            {
              this.docItem.PairId = borrowerPair.Borrower.ID;
              return;
            }
          }
          throw new ArgumentException("The specified borrower pair is invalid");
        }
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.Comments" /> associated with the TrackedDocument.
    /// </summary>
    public EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments Comments => this.comments;

    /// <summary>
    /// Indicates if the document is to be included in the eDisclosure package.
    /// </summary>
    public bool IncludeInEDisclosurePackage
    {
      get => this.docItem.OpeningDocument;
      set => this.docItem.OpeningDocument = value;
    }

    /// <summary>
    /// Indicates if the document is available to Webcenter (Send Files to borrower).
    /// </summary>
    public bool IsWebcenter
    {
      get => this.docItem.IsWebcenter;
      set => this.docItem.IsWebcenter = value;
    }

    /// <summary>
    /// Indicates if the document is available to TPO Webcenter Portal.
    /// </summary>
    public bool IsTPOWebcenterPortal
    {
      get => this.docItem.IsTPOWebcenterPortal;
      set => this.docItem.IsTPOWebcenterPortal = value;
    }

    /// <summary>
    /// Indicates if the document is available to EDM Lenders (Send Files to Lenders) or other service providers.
    /// </summary>
    public bool IsThirdPartyDoc
    {
      get => this.docItem.IsThirdPartyDoc;
      set => this.docItem.IsThirdPartyDoc = value;
    }

    /// <summary>Indicates if the document is a Closing document</summary>
    public bool IsClosingDocument => this.docItem.ClosingDocument;

    /// <summary>Indicates if the document is a PreClosing document</summary>
    public bool IsPreClosingDocument => this.docItem.PreClosingDocument;

    /// <summary>Indicates if the document is an opening document</summary>
    public bool IsOpeningDocument => this.docItem.OpeningDocument;

    /// <summary>
    /// Indicates if the document came thru the Ellie Mae Network
    /// </summary>
    public bool IsEMNDocument => this.docItem.IsePASS;

    /// <summary>Gets the EPass Signature of the document, if any</summary>
    /// <remarks>This property is intended for use within Encompass only.</remarks>
    public string EPassSignature => this.docItem.EPASSSignature;

    /// <summary>
    /// Gets the ID of the user who added this condition to the loan.
    /// </summary>
    public string AddedBy
    {
      get
      {
        this.EnsureValid();
        return this.docItem.AddedBy;
      }
    }

    /// <summary>
    /// Gets the Date and time on which the condition was added to the loan.
    /// </summary>
    /// <remarks>Some legacy TrackedDocument items do not contain a value for the DateAdded. For those
    /// items, this property will return <c>null</c>.</remarks>
    public object DateAdded
    {
      get
      {
        this.EnsureValid();
        return !(this.docItem.DateAdded == DateTime.MaxValue) ? (object) this.docItem.DateAdded : (object) null;
      }
    }

    /// <summary>Gets or sets whether the document has been ordered.</summary>
    public bool Ordered
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Requested;
      }
    }

    /// <summary>
    /// Gets or sets the date the document was ordered/requested.
    /// </summary>
    /// <remarks>If the document has not been ordered, this property will be null; otherwise,
    /// it will be a DateTime value. Modifying the OrderDate for a document will cause
    /// the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate">ExpirationDate</see> and <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate">DueDate</see>
    /// to be adjusted accordingly.
    /// <note type="implementnotes">Because of a limitation in the way COM marshals date values
    /// when exposed as a VARIANT, COM-based clients should call the <c>SetOrderDate()</c> method
    /// to set this property.</note>
    /// </remarks>
    /// <example>
    /// The following code sets the order date to today for all Document Events
    /// associated with the "Submittal" milestone. Once the OrderDate property has been
    /// set, the DueDate property is printed to indicate when the document should
    /// be received.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Look for any supporting document which is overdue
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Mark all of the documents associated with the Submittal milestone as ordered
    ///          if (doc.MilestoneName == session.Loans.Milestones.Submittal.Name)
    ///          {
    ///             // Set the order date to today and allow 10 days to be received
    ///             doc.OrderDate = DateTime.Now;
    ///             doc.DueDays = 10;
    /// 
    ///             // Write a message showing the date by which the document is due
    ///             Console.WriteLine("Document \"" + doc.Title + "\" has been ordered and is due on " + doc.DueDate);
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public object OrderDate
    {
      get => !this.docItem.Requested ? (object) null : (object) this.docItem.DateRequested;
      set
      {
        if (value == null)
          this.docItem.UnmarkAsRequested();
        else if (this.docItem.Requested)
          this.docItem.MarkAsRequested((DateTime) value, this.docItem.RequestedBy);
        else
          this.docItem.MarkAsRequested((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who ordered/requested this document.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.OrderDate" /> property must be set prior to setting this property.</remarks>
    public string OrderedBy
    {
      get => !this.docItem.Requested ? "" : this.docItem.RequestedBy;
      set
      {
        if (!this.Ordered)
          throw new InvalidOperationException("The DateRequested property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsRequested(this.docItem.DateRequested, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the OrderDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetOrderDate(object value) => this.OrderDate = value;

    /// <summary>
    /// Gets or sets whether the document has been re-ordered.
    /// </summary>
    public bool Reordered
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Rerequested;
      }
    }

    /// <summary>Gets or sets the date the document was re-ordered.</summary>
    /// <remarks>If the document has not been re-ordered, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a document as having been re-ordered, set this
    /// property to the approrpiate date. Once reordered, the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate">DueDate</see>
    /// and <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate">ExpirationDate</see> will be reset accordingly.
    /// <note type="implementnotes">Because of a limitation in the way COM marshals date values
    /// when exposed as a VARIANT, COM-based clients should call the <c>SetReorderDate()</c> method
    /// to set this property.</note>
    /// </remarks>
    /// <example>
    /// The following code locates all past-due documents in a loan and, for each,
    /// sets the re-order date to today.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Lock the loan
    ///       loan.Lock();
    /// 
    ///       // Loop over the list of supporting documents
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Check if the document has been ordered
    ///          if ((doc.OrderDate != null) && (doc.DueDays > 0))
    ///          {
    ///             // Check if it's past due
    ///             if (((DateTime) doc.DueDate < DateTime.Now) && (doc.ReceivedDate == null))
    ///             {
    ///                // Re-order the document
    ///                doc.ReorderDate = DateTime.Now;
    ///                Console.WriteLine("The document \"" + doc.Title + "\" has been reordered and is now due on " + doc.DueDate);
    ///             }
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public object ReorderDate
    {
      get => !this.docItem.Rerequested ? (object) null : (object) this.docItem.DateRerequested;
      set
      {
        if (value == null)
          this.docItem.UnmarkAsRerequested();
        else if (this.docItem.Rerequested)
          this.docItem.MarkAsRerequested((DateTime) value, this.docItem.RerequestedBy);
        else
          this.docItem.MarkAsRerequested((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who ordered/requested this document.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ReorderDate" /> property must be set prior to setting this property.</remarks>
    public string ReorderedBy
    {
      get => !this.docItem.Rerequested ? "" : this.docItem.RerequestedBy;
      set
      {
        if (!this.Reordered)
          throw new InvalidOperationException("The ReorderDate property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsRerequested(this.docItem.DateRerequested, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the ReorderDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetReorderDate(object value) => this.ReorderDate = value;

    /// <summary>
    /// Gets or sets the number of days from the order date until the document is due.
    /// </summary>
    /// <remarks>Modifying this property will cause the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate">DueDate</see>
    /// to be reset based on the number of days entered.</remarks>
    /// <example>
    /// The following code sets the order date to today for all Document Events
    /// associated with the "Submittal" milestone. Once the OrderDate property has been
    /// set, the DueDate property is printed to indicate when the document should
    /// be received.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan and lock it for writing
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    ///       loan.Lock();
    /// 
    ///       // Look for any supporting document which is overdue
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Mark all of the documents associated with the Submittal milestone as ordered
    ///          if (doc.MilestoneName == session.Loans.Milestones.Submittal.Name)
    ///          {
    ///             // Set the order date to today and allow 10 days to be received
    ///             doc.OrderDate = DateTime.Now;
    ///             doc.DueDays = 10;
    /// 
    ///             // Write a message showing the date by which the document is due
    ///             Console.WriteLine("Document \"" + doc.Title + "\" has been ordered and is due on " + doc.DueDate);
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int DueDays
    {
      get
      {
        this.EnsureValid();
        return this.docItem.DaysDue;
      }
      set
      {
        this.EnsureEditable();
        this.docItem.DaysDue = value;
      }
    }

    /// <summary>
    /// Indicates if the document is currently in the Due/Expected state.
    /// </summary>
    /// <remarks>A document is due if it has a valid <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate" />.</remarks>
    public bool Due
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Expected;
      }
    }

    /// <summary>Gets the date on which the document is due.</summary>
    /// <remarks>This property will return null until a valid <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.OrderDate">OrderDate</see>
    /// is set for the document. Once the OrderDate is set, this property is calculated by adding to
    /// it the value of the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDays">DueDays</see> property.</remarks>
    /// <example>
    /// The following code retrieves the set of all supporting documents associated
    /// with a loan and generates a report for all documents that are overdue.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a loan for reading
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Look for any supporting document which is overdue
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          if (doc.DueDate != null)
    ///          {
    ///             if ((DateTime) doc.DueDate < DateTime.Now)
    ///                Console.WriteLine("The document \"" + doc.Title + "\" is overdue!");
    ///          }
    ///       }
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public object DueDate
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Expected ? (object) this.docItem.DateExpected : (object) null;
      }
    }

    /// <summary>Gets a flag indicating if this document is past due.</summary>
    /// <remarks>
    /// If the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate" /> property is null, this property will always
    /// return <c>false</c>. Otherwise, it will return true if the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate" />
    /// is prior to the current date and the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ReceivedDate" /> is null.
    /// </remarks>
    public bool PastDue
    {
      get
      {
        this.EnsureValid();
        return this.docItem.IsPastDue;
      }
    }

    /// <summary>Gets or sets whether the document has been received.</summary>
    public bool Received
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Received;
      }
    }

    /// <summary>Gets or sets the date the document was received.</summary>
    /// <remarks>If the document has not been received, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a document as not having been received, set this
    /// property to null.
    /// <note type="implementnotes">Because of a limitation in the way COM marshals date values
    /// when exposed as a VARIANT, COM-based clients should call the <c>SetReceivedDate()</c> method
    /// to set this property.</note>
    /// </remarks>
    /// <example>
    /// The following code locates all past-due documents in a loan and, for each,
    /// sets the re-order date to today.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Lock the loan
    ///       loan.Lock();
    /// 
    ///       // Loop over the list of supporting documents
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Check if the document has been ordered
    ///          if ((doc.OrderDate != null) && (doc.DueDays > 0))
    ///          {
    ///             // Check if it's past due
    ///             if (((DateTime) doc.DueDate < DateTime.Now) && (doc.ReceivedDate == null))
    ///             {
    ///                // Re-order the document
    ///                doc.ReorderDate = DateTime.Now;
    ///                Console.WriteLine("The document \"" + doc.Title + "\" has been reordered and is now due on " + doc.DueDate);
    ///             }
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public object ReceivedDate
    {
      get => !this.docItem.Received ? (object) null : (object) this.docItem.DateReceived;
      set
      {
        if (value == null)
          this.docItem.UnmarkAsReceived();
        else if (this.docItem.Received)
          this.docItem.MarkAsReceived((DateTime) value, this.docItem.ReceivedBy);
        else
          this.docItem.MarkAsReceived((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has marked the document as received.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ReceivedDate" /> property must be set prior to setting this property.</remarks>
    public string ReceivedBy
    {
      get => !this.docItem.Received ? "" : this.docItem.ReceivedBy;
      set
      {
        if (!this.Received)
          throw new InvalidOperationException("The ReceivedDate property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsReceived(this.docItem.DateReceived, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the ReceivedDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetReceivedDate(object value) => this.ReceivedDate = value;

    /// <summary>Gets or sets whether the document has been reviewed.</summary>
    public bool Reviewed
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Reviewed;
      }
    }

    /// <summary>Gets or sets the date the document was reviewed</summary>
    /// <remarks>If the document has not been reviewed, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a document as not having been reviewed, set this
    /// property to null.
    /// <note type="implementnotes">Because of a limitation in the way COM marshals date values
    /// when exposed as a VARIANT, COM-based clients should call the <c>SetReviewedDate()</c> method
    /// to set this property.</note>
    /// </remarks>
    /// <include file="TrackedDocument.xml" path="Examples/Example[@name=&quot;TrackedDocument.ReviewedDate&quot;]/*" />
    public object ReviewedDate
    {
      get => !this.docItem.Reviewed ? (object) null : (object) this.docItem.DateReviewed;
      set
      {
        if (value == null)
          this.docItem.UnmarkAsReviewed();
        else if (this.docItem.Reviewed)
          this.docItem.MarkAsReviewed((DateTime) value, this.docItem.ReviewedBy);
        else
          this.docItem.MarkAsReviewed((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has marked the document as reviewed.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ReviewedDate" /> property must be set prior to setting this property.</remarks>
    public string ReviewedBy
    {
      get => !this.docItem.Reviewed ? "" : this.docItem.ReviewedBy;
      set
      {
        if (!this.Reviewed)
          throw new InvalidOperationException("The ReviewedDate property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsReviewed(this.docItem.DateReviewed, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the ReviewedDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetReviewedDate(object value) => this.ReviewedDate = value;

    /// <summary>
    /// Gets or sets whether the document has been marked as ready for shipping.
    /// </summary>
    public bool ShippingReady
    {
      get
      {
        this.EnsureValid();
        return this.docItem.ShippingReady;
      }
    }

    /// <summary>
    /// Gets or sets the date the document was marked as ready for shipping.
    /// </summary>
    /// <remarks>If the document has not been marked as Ready for Shipping, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a document as not having been received, set this
    /// property to null.
    /// </remarks>
    public object ShippingReadyDate
    {
      get => !this.docItem.ShippingReady ? (object) null : (object) this.docItem.DateShippingReady;
      set
      {
        if (value == null)
          this.docItem.UnmarkAsShippingReady();
        else if (this.docItem.ShippingReady)
          this.docItem.MarkAsShippingReady((DateTime) value, this.docItem.ShippingReadyBy);
        else
          this.docItem.MarkAsShippingReady((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has marked the document as ready to be shipped.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ShippingReadyDate" /> property must be set prior to setting this property.</remarks>
    public string ShippingReadyBy
    {
      get => !this.docItem.ShippingReady ? "" : this.docItem.ShippingReadyBy;
      set
      {
        if (!this.ShippingReady)
          throw new InvalidOperationException("The ShippingReadyDate property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsShippingReady(this.docItem.DateShippingReady, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the ShippingReadyDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetShippingReadyDate(object value) => this.ShippingReadyDate = value;

    /// <summary>
    /// Gets or sets whether the document has been marked as ready for underwriting.
    /// </summary>
    public bool UnderwritingReady
    {
      get
      {
        this.EnsureValid();
        return this.docItem.UnderwritingReady;
      }
    }

    /// <summary>
    /// Gets or sets the date the document was marked as ready for underwriting.
    /// </summary>
    /// <remarks>If the document has not been marked as Ready for Underwriting, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a document as not having been received, set this
    /// property to null.
    /// </remarks>
    public object UnderwritingReadyDate
    {
      get
      {
        return !this.docItem.UnderwritingReady ? (object) null : (object) this.docItem.DateUnderwritingReady;
      }
      set
      {
        if (value == null)
          this.docItem.UnmarkAsUnderwritingReady();
        else if (this.docItem.UnderwritingReady)
          this.docItem.MarkAsUnderwritingReady((DateTime) value, this.docItem.UnderwritingReadyBy);
        else
          this.docItem.MarkAsUnderwritingReady((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who has marked the document as ready to for underwriting.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.UnderwritingReadyDate" /> property must be set prior to setting this property.</remarks>
    public string UnderwritingReadyBy
    {
      get => !this.docItem.UnderwritingReady ? "" : this.docItem.UnderwritingReadyBy;
      set
      {
        if (!this.UnderwritingReady)
          throw new InvalidOperationException("The UnderwritingReadyDate property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsUnderwritingReady(this.docItem.DateUnderwritingReady, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the UnderwritingReadyDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetUnderwritingReadyDate(object value)
    {
      this.UnderwritingReadyDate = value;
    }

    /// <summary>Gets or sets whether the document has been archived.</summary>
    public bool Archived
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Archived;
      }
    }

    /// <summary>Gets or sets the date the document was re-ordered.</summary>
    /// <remarks>If the document has not been re-ordered, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a document as having been re-ordered, set this
    /// property to the approrpiate date. Once reordered, the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate">DueDate</see>
    /// and <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate">ExpirationDate</see> will be reset accordingly.
    /// <note type="implementnotes">Because of a limitation in the way COM marshals date values
    /// when exposed as a VARIANT, COM-based clients should call the <c>SetReorderDate()</c> method
    /// to set this property.</note>
    /// </remarks>
    /// <example>
    /// The following code locates all past-due documents in a loan and, for each,
    /// sets the re-order date to today.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Lock the loan
    ///       loan.Lock();
    /// 
    ///       // Loop over the list of supporting documents
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Check if the document has been ordered
    ///          if ((doc.OrderDate != null) && (doc.DueDays > 0))
    ///          {
    ///             // Check if it's past due
    ///             if (((DateTime) doc.DueDate < DateTime.Now) && (doc.ReceivedDate == null))
    ///             {
    ///                // Re-order the document
    ///                doc.ReorderDate = DateTime.Now;
    ///                Console.WriteLine("The document \"" + doc.Title + "\" has been reordered and is now due on " + doc.DueDate);
    ///             }
    ///          }
    ///       }
    /// 
    ///       loan.Commit();
    ///       loan.Unlock();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public object ArchiveDate
    {
      get => !this.docItem.Archived ? (object) null : (object) this.docItem.DateArchived;
      set
      {
        if (value == null)
          this.docItem.UnmarkAsArchived();
        else if (this.docItem.Archived)
          this.docItem.MarkAsArchived((DateTime) value, this.docItem.ArchivedBy);
        else
          this.docItem.MarkAsArchived((DateTime) value, this.Loan.Session.UserID);
      }
    }

    /// <summary>
    /// Gets or sets the ID of the user who archived this document.
    /// </summary>
    /// <remarks>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ArchiveDate" /> property must be set prior to setting this property.</remarks>
    public string ArchivedBy
    {
      get => !this.docItem.Archived ? "" : this.docItem.ArchivedBy;
      set
      {
        if (!this.Archived)
          throw new InvalidOperationException("The ArchiveDate property must be set prior to this operation");
        if ((value ?? "") == "")
          throw new ArgumentException("Invalid user ID specified");
        this.docItem.MarkAsArchived(this.docItem.DateArchived, value);
      }
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the ReorderDate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void ITrackedDocument.SetArchiveDate(object value) => this.ArchiveDate = value;

    /// <summary>
    /// Gets or sets the number of days from the date the document is received until the
    /// document is considered to be expired.
    /// </summary>
    /// <remarks>Modifying this property will cause the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate">ExpirationDate</see>
    /// to be reset based on the number of days entered.</remarks>
    public int ExpirationDays
    {
      get
      {
        this.EnsureValid();
        return this.docItem.DaysTillExpire;
      }
      set
      {
        this.EnsureEditable();
        this.docItem.DaysTillExpire = value;
      }
    }

    /// <summary>Gets the date on which the document expires.</summary>
    /// <remarks>This property will return null if the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ReceivedDate">ReceivedDate</see>
    /// has not been set. Once the document is received, this property will be set
    /// by adding the number of days specified in the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDays">ExpirationDays</see>
    /// property to the ReceivedDate.</remarks>
    /// <example>
    /// The following code generates a list of all of the expired conditions for
    /// a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Loop over the list of supporting documents
    ///       foreach (TrackedDocument doc in loan.Log.TrackedDocuments)
    ///       {
    ///          // Locate any documents which are expired conditions for the loan
    ///          if ((doc.ExpirationDate != null) &&
    ///             ((DateTime) doc.ExpirationDate < DateTime.Now))
    ///          {
    ///             Console.WriteLine("The document \"" + doc.Title + "\" is an expired condition for this loan");
    ///          }
    ///       }
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public object ExpirationDate
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Expires ? (object) this.docItem.DateExpires : (object) null;
      }
    }

    /// <summary>Gets a flag indicating if this document has expired.</summary>
    /// <remarks>
    /// If the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate" /> property is null, this property will always
    /// return <c>false</c>. Otherwise, it will return true if the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate" />
    /// is prior to the current date.
    /// </remarks>
    public bool Expired
    {
      get
      {
        this.EnsureValid();
        return this.ExpirationDate != null && Convert.ToDateTime(this.ExpirationDate) < DateTime.Today;
      }
    }

    /// <summary>Gets the UserID that last accessed this document.</summary>
    public string LastAccessedBy
    {
      get
      {
        this.EnsureValid();
        return this.docItem.AccessedBy;
      }
    }

    /// <summary>
    /// Gets the date on which the document was last accessed.
    /// </summary>
    /// <remarks>This property will return null if the document has not yet been accessed.
    /// Once the document has been accessed, this property will indicate the date of that event.
    /// </remarks>
    public object LastAccessDate
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Accessed ? (object) this.docItem.DateAccessed : (object) null;
      }
    }

    /// <summary>Marks the document as being accessed.</summary>
    /// <param name="accessUserId">The User ID that should be used to indicate access to this
    /// document. This value must equal the User ID of the current session unless the currently
    /// logged in user has the Administrator persona.</param>
    public void MarkAsAccessed(string accessUserId)
    {
      if (!this.Loan.Session.GetUserInfo().IsAdministrator() && accessUserId != this.Loan.Session.UserID)
        throw new SecurityException("Only a user with the admin persona can specify a UserID other than their own for this operation.");
      this.docItem.MarkAsAccessed(DateTime.Now, accessUserId);
    }

    /// <summary>Indicates if this TrackedDocument is in alert status.</summary>
    /// <remarks>A TrackedDocument is considered to be an alert if either of the following
    /// conditions are met:
    /// <list type="bullet">
    /// <item>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ReceivedDate" /> is null and the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.DueDate" /> is on or prior to today.</item>
    /// <item>The <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.ExpirationDate" /> is on or prior to today.</item>
    /// </list>
    /// Note that these conditions differ slightly from the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.PastDue" /> and
    /// <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.Expired" /> properties, which will not return <c>true</c> until the
    /// respective dates are in the past.
    /// </remarks>
    public override bool IsAlert
    {
      get
      {
        if (this.ReceivedDate == null && this.DueDate != null && Convert.ToDateTime(this.DueDate) <= DateTime.Today)
          return true;
        return this.ExpirationDate != null && Convert.ToDateTime(this.ExpirationDate) <= DateTime.Today;
      }
    }

    /// <summary>
    /// Gets the set of conditions linked to the current document.
    /// </summary>
    public LogEntryList GetLinkedConditions()
    {
      LogEntryList linkedConditions = new LogEntryList();
      foreach (LogRecordBase condition in this.docItem.Conditions)
        linkedConditions.Add(this.Loan.Log.Find(condition, true));
      return linkedConditions;
    }

    /// <summary>Links the TrackedDocument to a condition.</summary>
    /// <param name="condition">The condition to which the document will be linked.</param>
    public void LinkToCondition(Condition condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (this.loan.Log.Find(condition.Unwrap(), false) == null)
        throw new ArgumentException("The specified condition must be added to the current loan before it can be attached to the document.");
      this.docItem.Conditions.Add(condition.Unwrap() as ConditionLog);
    }

    /// <summary>
    /// Breaks the link between the TrackedDocument and a condition.
    /// </summary>
    /// <param name="condition">The condition to which the document will be unlinked.</param>
    public void UnlinkCondition(Condition condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      this.docItem.Conditions.Remove(condition.Unwrap() as ConditionLog);
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument.RoleAccessList" /> that defines which roles have access to the document.
    /// </summary>
    public RoleAccessList RoleAccessList => this.accessList;

    /// <summary>
    /// Retrieves the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> objects associated with the document.
    /// </summary>
    /// <returns>Returns an <see cref="T:EllieMae.Encompass.Collections.AttachmentList" /> containing the attachments.</returns>
    /// <example>
    /// The following code exports all of the attachments associated with the Appraisals
    /// in a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Get all Appraisals from the TrackedDocument list
    ///       LogEntryList appraisals = loan.Log.TrackedDocuments.GetDocumentsByTitle("Appraisal");
    /// 
    ///       // Save each attachment to the C:\Appraisals folder
    ///       foreach (TrackedDocument appraisal in appraisals)
    ///          foreach (Attachment att in appraisal.GetAttachments())
    ///          att.SaveToDisk("C:\\Appraisals\\" + att.Name);
    /// 
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public AttachmentList GetAttachments()
    {
      this.EnsureValid();
      FileAttachment[] attachments1 = this.loan.Unwrap().FileAttachments.GetAttachments(this.docItem);
      AttachmentList attachments2 = new AttachmentList();
      foreach (FileAttachment fileAttachment in attachments1)
      {
        Attachment attachmentByName = this.loan.Attachments.GetAttachmentByName(fileAttachment.ID);
        if (attachmentByName != null)
          attachments2.Add(attachmentByName);
      }
      return attachments2;
    }

    /// <summary>
    /// Links an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> to the document.
    /// </summary>
    /// <param name="attachment">The existing Attachment to be linked.</param>
    /// <example>
    /// The following code creates a new attachment to the loan and associates it
    /// with an existing Appraisal.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Lock the loan since we will be modifying it
    ///       loan.Lock();
    /// 
    ///       // Create a new attachment by importing it from a TIFF document on disk
    ///       Attachment att = loan.Attachments.Add("C:\\Scanner Output\\MyAppraisal.tif");
    /// 
    ///       // Now attach the new Attachment to the Appraisal on the loan
    ///       LogEntryList appraisals = loan.Log.TrackedDocuments.GetDocumentsByTitle("Appraisal");
    /// 
    ///       if (appraisals.Count > 0)
    ///       {
    ///          TrackedDocument appraisal = (TrackedDocument) appraisals[0];
    ///          appraisal.Attach(att);
    ///       }
    /// 
    ///       // Save the changes to the loan, which commits the new attachment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Attach(Attachment attachment)
    {
      if (!attachment.Loan.Equals((object) this.loan))
        throw new ArgumentException("The specified attachment is not from the same loan.");
      this.docItem.Files.Add(attachment.Name, this.loan.Session.UserID, true);
    }

    /// <summary>
    /// Removes the link to an <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Attachment" /> for the document.
    /// </summary>
    /// <param name="attachment">The existing Attachment to be detached.</param>
    /// <example>
    /// The following code creates a new attachment to the loan and associates it
    /// with an existing Appraisal.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open and lock the next loan in the loop
    ///       Loan loan = session.Loans.Folders["My Pipeline"].OpenLoan("Sample");
    /// 
    ///       // Lock the loan since we will be modifying it
    ///       loan.Lock();
    /// 
    ///       // Create a new attachment by importing it from a TIFF document on disk
    ///       Attachment att = loan.Attachments.Add("C:\\Scanner Output\\MyAppraisal.tif");
    /// 
    ///       // Now attach the new Attachment to the Appraisal on the loan
    ///       LogEntryList appraisals = loan.Log.TrackedDocuments.GetDocumentsByTitle("Appraisal");
    /// 
    ///       if (appraisals.Count > 0)
    ///       {
    ///          TrackedDocument appraisal = (TrackedDocument) appraisals[0];
    ///          appraisal.Attach(att);
    ///       }
    /// 
    ///       // Save the changes to the loan, which commits the new attachment
    ///       loan.Commit();
    ///       loan.Close();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Detach(Attachment attachment)
    {
      if (!attachment.Loan.Equals((object) this.loan))
        throw new ArgumentException("The specified attachment is not from the same loan.");
      if (!this.docItem.Files.Contains(attachment.Name))
        throw new ArgumentException("The specified attachment is not attached to the current document.");
      this.docItem.Files.Remove(attachment.Name);
    }
  }
}
