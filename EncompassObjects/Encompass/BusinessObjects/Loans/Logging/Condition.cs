// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Condition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Represents an Underwriting or Post-Closing condition.</summary>
  public abstract class Condition : LogEntry, ICondition
  {
    private StandardConditionLog logItem;
    private EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments comments;

    internal Condition(Loan loan, StandardConditionLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
      this.comments = new EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments((LogEntry) this, logItem.Comments);
    }

    /// <summary>Gets the title of the condition</summary>
    public string Title
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Title;
      }
      set
      {
        this.EnsureEditable();
        if ((value ?? "") == "")
          throw new ArgumentException("Condition title cannot be blank or null");
        this.logItem.Title = value ?? "";
      }
    }

    /// <summary>
    /// Indicates the type of condition represented by the object
    /// </summary>
    public ConditionType ConditionType => (ConditionType) this.logItem.ConditionType;

    /// <summary>
    /// Gets or sets the BorrowerPair associated with a condition.
    /// </summary>
    public BorrowerPair BorrowerPair
    {
      get => this.Loan.BorrowerPairs.GetPairByID(this.logItem.PairId);
      set
      {
        if (value == (BorrowerPair) null)
          this.logItem.PairId = (string) null;
        else
          this.logItem.PairId = value.Borrower.ID;
      }
    }

    /// <summary>
    /// Gets the ID of the user who added this condition to the loan.
    /// </summary>
    public string AddedBy
    {
      get
      {
        this.EnsureValid();
        return this.logItem.AddedBy;
      }
    }

    /// <summary>
    /// Gets the Date and time on which the condition was added to the loan.
    /// </summary>
    public DateTime DateAdded
    {
      get
      {
        this.EnsureValid();
        return this.logItem.DateAdded;
      }
    }

    /// <summary>Gets or sets the description of the Condition.</summary>
    public string Description
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Description;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Description = value ?? "";
      }
    }

    /// <summary>Gets or sets the details of the condition.</summary>
    public string Details
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Details;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Details = value ?? "";
      }
    }

    /// <summary>Gets or sets the source of the Condition.</summary>
    public string Source
    {
      get
      {
        this.EnsureValid();
        return this.logItem.Source;
      }
      set
      {
        this.EnsureEditable();
        this.logItem.Source = value ?? "";
      }
    }

    /// <summary>Gets the current status of the condition.</summary>
    public ConditionStatus Status
    {
      get
      {
        return (ConditionStatus) Enum.Parse(typeof (ConditionStatus), this.logItem.Status.ToString(), true);
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.Condition.Comments" /> collection for the condition.
    /// </summary>
    public EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments Comments => this.comments;

    /// <summary>
    /// Returns the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.TrackedDocument" /> objects which are linked to this condition.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LogEntryList" /> containing the linked documents.</returns>
    public LogEntryList GetLinkedDocuments()
    {
      LogEntryList linkedDocuments = new LogEntryList();
      foreach (LogRecordBase linkedDocument in this.logItem.GetLinkedDocuments())
      {
        LogEntry logEntry = this.Loan.Log.TrackedDocuments.Find(linkedDocument, true);
        linkedDocuments.Add(logEntry);
      }
      return linkedDocuments;
    }
  }
}
