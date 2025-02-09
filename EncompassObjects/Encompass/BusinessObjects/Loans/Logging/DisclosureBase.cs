// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosureBase
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Disclosure Base Class</summary>
  public abstract class DisclosureBase : LogEntry
  {
    /// <summary>Object of the DisclosureTrackingBase Class</summary>
    protected DisclosureTrackingBase discItem;
    /// <summary>Object of the loan class</summary>
    protected Loan loan;
    private DisclosureFields fields;
    private DisclosedDocuments docs;
    private EDisclosureStatus edisclosureStatus;

    internal DisclosureBase(Loan loan, DisclosureTrackingBase discItem)
      : base(loan, (LogRecordBase) discItem)
    {
      this.discItem = discItem;
      this.loan = loan;
    }

    /// <summary>Gets the User ID of the user who made the disclosure.</summary>
    public string DisclosedBy
    {
      get
      {
        this.EnsureValid();
        return this.discItem.DisclosedBy;
      }
    }

    /// <summary>Gets the date the disclosure record was created.</summary>
    public DateTime DateAdded => this.discItem.DateAdded;

    /// <summary>
    /// Gets or sets the Date on which the disclosure was made.
    /// </summary>
    public DateTime Date
    {
      get
      {
        this.EnsureValid();
        return this.discItem.Date;
      }
      set
      {
        this.EnsureEditable();
        this.discItem.IsLocked = true;
        this.discItem.DisclosedDate = value;
      }
    }

    /// <summary>
    /// Gets the collection of fields values which are stored when a disclosure is made.
    /// </summary>
    /// <remarks>When a disclosure is made, some field values from the loan are copied into the
    /// disclosure record </remarks>
    public DisclosureFields Fields
    {
      get
      {
        if (this.fields == null)
        {
          this.loan.LoanData.GetSnapshotDataForDisclosureTracking2015LogsForLoan(this.discItem.Guid);
          this.fields = new DisclosureFields(this);
        }
        return this.fields;
      }
    }

    /// <summary>
    /// Gets the collection of documents that were included in the disclosure.
    /// </summary>
    public DisclosedDocuments Documents
    {
      get
      {
        if (this.docs == null)
          this.docs = new DisclosedDocuments(this, this.discItem.DisclosedFormList);
        return this.docs;
      }
    }

    /// <summary>
    /// Gets the eDisclosure status information for the disclosure.
    /// </summary>
    /// <remarks>This property will return <c>null</c> unless the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.DeliveryMethod" />
    /// property has the value <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.DeliveryMethod.eDisclosure" />.</remarks>
    public EDisclosureStatus EDisclosureStatus
    {
      get
      {
        if (!this.IseDiclosure)
          return (EDisclosureStatus) null;
        if (this.edisclosureStatus == null)
          this.edisclosureStatus = new EDisclosureStatus(this.loan, this.discItem);
        return this.edisclosureStatus;
      }
    }

    internal abstract bool IseDiclosure { get; }

    /// <summary>Gets or sets the Received Date</summary>
    public abstract object ReceivedDate { get; set; }

    internal override void Dispose()
    {
      this.loan = (Loan) null;
      this.discItem = (DisclosureTrackingBase) null;
    }
  }
}
