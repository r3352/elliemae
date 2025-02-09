// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public override LogEntryType EntryType => LogEntryType.TrackedDocument;

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

    public string DocumentType
    {
      get
      {
        return this.docItem.GetDocumentType(this.Loan.Unwrap().SystemConfiguration.DocumentTrackingSetup);
      }
    }

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

    public EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments Comments => this.comments;

    public bool IncludeInEDisclosurePackage
    {
      get => this.docItem.OpeningDocument;
      set => this.docItem.OpeningDocument = value;
    }

    public bool IsWebcenter
    {
      get => this.docItem.IsWebcenter;
      set => this.docItem.IsWebcenter = value;
    }

    public bool IsTPOWebcenterPortal
    {
      get => this.docItem.IsTPOWebcenterPortal;
      set => this.docItem.IsTPOWebcenterPortal = value;
    }

    public bool IsThirdPartyDoc
    {
      get => this.docItem.IsThirdPartyDoc;
      set => this.docItem.IsThirdPartyDoc = value;
    }

    public bool IsClosingDocument => this.docItem.ClosingDocument;

    public bool IsPreClosingDocument => this.docItem.PreClosingDocument;

    public bool IsOpeningDocument => this.docItem.OpeningDocument;

    public bool IsEMNDocument => this.docItem.IsePASS;

    public string EPassSignature => this.docItem.EPASSSignature;

    public string AddedBy
    {
      get
      {
        this.EnsureValid();
        return this.docItem.AddedBy;
      }
    }

    public object DateAdded
    {
      get
      {
        this.EnsureValid();
        return !(this.docItem.DateAdded == DateTime.MaxValue) ? (object) this.docItem.DateAdded : (object) null;
      }
    }

    public bool Ordered
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Requested;
      }
    }

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

    void ITrackedDocument.SetOrderDate(object value) => this.OrderDate = value;

    public bool Reordered
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Rerequested;
      }
    }

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

    void ITrackedDocument.SetReorderDate(object value) => this.ReorderDate = value;

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

    public bool Due
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Expected;
      }
    }

    public object DueDate
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Expected ? (object) this.docItem.DateExpected : (object) null;
      }
    }

    public bool PastDue
    {
      get
      {
        this.EnsureValid();
        return this.docItem.IsPastDue;
      }
    }

    public bool Received
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Received;
      }
    }

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

    void ITrackedDocument.SetReceivedDate(object value) => this.ReceivedDate = value;

    public bool Reviewed
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Reviewed;
      }
    }

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

    void ITrackedDocument.SetReviewedDate(object value) => this.ReviewedDate = value;

    public bool ShippingReady
    {
      get
      {
        this.EnsureValid();
        return this.docItem.ShippingReady;
      }
    }

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

    void ITrackedDocument.SetShippingReadyDate(object value) => this.ShippingReadyDate = value;

    public bool UnderwritingReady
    {
      get
      {
        this.EnsureValid();
        return this.docItem.UnderwritingReady;
      }
    }

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

    void ITrackedDocument.SetUnderwritingReadyDate(object value)
    {
      this.UnderwritingReadyDate = value;
    }

    public bool Archived
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Archived;
      }
    }

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

    void ITrackedDocument.SetArchiveDate(object value) => this.ArchiveDate = value;

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

    public object ExpirationDate
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Expires ? (object) this.docItem.DateExpires : (object) null;
      }
    }

    public bool Expired
    {
      get
      {
        this.EnsureValid();
        return this.ExpirationDate != null && Convert.ToDateTime(this.ExpirationDate) < DateTime.Today;
      }
    }

    public string LastAccessedBy
    {
      get
      {
        this.EnsureValid();
        return this.docItem.AccessedBy;
      }
    }

    public object LastAccessDate
    {
      get
      {
        this.EnsureValid();
        return this.docItem.Accessed ? (object) this.docItem.DateAccessed : (object) null;
      }
    }

    public void MarkAsAccessed(string accessUserId)
    {
      if (!this.Loan.Session.GetUserInfo().IsAdministrator() && accessUserId != this.Loan.Session.UserID)
        throw new SecurityException("Only a user with the admin persona can specify a UserID other than their own for this operation.");
      this.docItem.MarkAsAccessed(DateTime.Now, accessUserId);
    }

    public override bool IsAlert
    {
      get
      {
        if (this.ReceivedDate == null && this.DueDate != null && Convert.ToDateTime(this.DueDate) <= DateTime.Today)
          return true;
        return this.ExpirationDate != null && Convert.ToDateTime(this.ExpirationDate) <= DateTime.Today;
      }
    }

    public LogEntryList GetLinkedConditions()
    {
      LogEntryList linkedConditions = new LogEntryList();
      foreach (LogRecordBase condition in this.docItem.Conditions)
        linkedConditions.Add(this.Loan.Log.Find(condition, true));
      return linkedConditions;
    }

    public void LinkToCondition(Condition condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      if (this.loan.Log.Find(condition.Unwrap(), false) == null)
        throw new ArgumentException("The specified condition must be added to the current loan before it can be attached to the document.");
      this.docItem.Conditions.Add(condition.Unwrap() as ConditionLog);
    }

    public void UnlinkCondition(Condition condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      this.docItem.Conditions.Remove(condition.Unwrap() as ConditionLog);
    }

    public RoleAccessList RoleAccessList => this.accessList;

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

    public void Attach(Attachment attachment)
    {
      if (!attachment.Loan.Equals((object) this.loan))
        throw new ArgumentException("The specified attachment is not from the same loan.");
      this.docItem.Files.Add(attachment.Name, this.loan.Session.UserID, true);
    }

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
