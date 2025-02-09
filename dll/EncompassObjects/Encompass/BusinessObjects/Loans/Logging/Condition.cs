// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Condition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public abstract class Condition : LogEntry, ICondition
  {
    private StandardConditionLog logItem;
    private EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments comments;

    internal Condition(Loan loan, StandardConditionLog logItem)
      : base(loan, (LogRecordBase) logItem)
    {
      this.logItem = logItem;
      this.comments = new EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments((LogEntry) this, ((ConditionLog) logItem).Comments);
    }

    public string Title
    {
      get
      {
        this.EnsureValid();
        return ((ConditionLog) this.logItem).Title;
      }
      set
      {
        this.EnsureEditable();
        if ((value ?? "") == "")
          throw new ArgumentException("Condition title cannot be blank or null");
        ((ConditionLog) this.logItem).Title = value ?? "";
      }
    }

    public ConditionType ConditionType
    {
      get => (ConditionType) ((ConditionLog) this.logItem).ConditionType;
    }

    public BorrowerPair BorrowerPair
    {
      get => this.Loan.BorrowerPairs.GetPairByID(((ConditionLog) this.logItem).PairId);
      set
      {
        if (value == (BorrowerPair) null)
          ((ConditionLog) this.logItem).PairId = (string) null;
        else
          ((ConditionLog) this.logItem).PairId = value.Borrower.ID;
      }
    }

    public string AddedBy
    {
      get
      {
        this.EnsureValid();
        return ((ConditionLog) this.logItem).AddedBy;
      }
    }

    public DateTime DateAdded
    {
      get
      {
        this.EnsureValid();
        return ((ConditionLog) this.logItem).DateAdded;
      }
    }

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

    public string Source
    {
      get
      {
        this.EnsureValid();
        return ((ConditionLog) this.logItem).Source;
      }
      set
      {
        this.EnsureEditable();
        ((ConditionLog) this.logItem).Source = value ?? "";
      }
    }

    public ConditionStatus Status
    {
      get
      {
        return (ConditionStatus) Enum.Parse(typeof (ConditionStatus), this.logItem.Status.ToString(), true);
      }
    }

    public EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments Comments => this.comments;

    public LogEntryList GetLinkedDocuments()
    {
      LogEntryList linkedDocuments = new LogEntryList();
      foreach (LogRecordBase linkedDocument in ((ConditionLog) this.logItem).GetLinkedDocuments())
      {
        LogEntry logEntry = this.Loan.Log.TrackedDocuments.Find(linkedDocument, true);
        linkedDocuments.Add(logEntry);
      }
      return linkedDocuments;
    }
  }
}
