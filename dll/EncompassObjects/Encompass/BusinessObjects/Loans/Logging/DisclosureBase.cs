// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosureBase
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public abstract class DisclosureBase : LogEntry
  {
    protected DisclosureTrackingBase discItem;
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

    public string DisclosedBy
    {
      get
      {
        this.EnsureValid();
        return this.discItem.DisclosedBy;
      }
    }

    public DateTime DateAdded => this.discItem.DateAdded;

    public DateTime Date
    {
      get
      {
        this.EnsureValid();
        return ((LogRecordBase) this.discItem).Date;
      }
      set
      {
        this.EnsureEditable();
        this.discItem.IsLocked = true;
        this.discItem.DisclosedDate = value;
      }
    }

    public DisclosureFields Fields
    {
      get
      {
        if (this.fields == null)
        {
          this.loan.LoanData.GetSnapshotDataForDisclosureTracking2015LogsForLoan(((LogRecordBase) this.discItem).Guid);
          this.fields = new DisclosureFields(this);
        }
        return this.fields;
      }
    }

    public DisclosedDocuments Documents
    {
      get
      {
        if (this.docs == null)
          this.docs = new DisclosedDocuments(this, this.discItem.DisclosedFormList);
        return this.docs;
      }
    }

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

    public abstract object ReceivedDate { get; set; }

    internal override void Dispose()
    {
      this.loan = (Loan) null;
      this.discItem = (DisclosureTrackingBase) null;
    }
  }
}
