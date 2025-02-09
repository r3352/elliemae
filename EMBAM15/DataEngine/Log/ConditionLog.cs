// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ConditionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public abstract class ConditionLog : LogRecordBase
  {
    protected string title = string.Empty;
    protected string source = string.Empty;
    protected string category = string.Empty;
    protected string pairId;
    protected CommentEntryCollection commentList;
    protected DateTime dateAdded = DateTime.MinValue;
    protected string addedBy = string.Empty;

    public ConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
    }

    public ConditionLog()
    {
    }

    public abstract ConditionType ConditionType { get; }

    public virtual string Title
    {
      get => this.title;
      set
      {
        if (!(this.title != value))
          return;
        this.title = value;
        this.TrackChange("Condition Name changed to \"" + this.title + "\"");
      }
    }

    public virtual string Source
    {
      get => this.source;
      set
      {
        if (!(this.source != value))
          return;
        this.source = value;
        this.TrackChange("Source changed to \"" + this.source + "\"");
      }
    }

    public virtual string Category
    {
      get => this.category;
      set
      {
        if (!(this.category != value))
          return;
        this.category = value;
        this.TrackChange("Category changed to \"" + this.category + "\"");
      }
    }

    public virtual string PairId
    {
      get => this.pairId;
      set
      {
        if (!(this.pairId != value))
          return;
        this.pairId = value;
        string details = "For Borrower changed";
        if (this.IsAttachedToLog)
        {
          BorrowerPair borrowerPair = !(this.pairId == BorrowerPair.All.Id) ? this.Log.Loan.GetBorrowerPair(this.pairId) : BorrowerPair.All;
          if (borrowerPair != null)
            details = details + " to \"" + borrowerPair.ToString() + "\"";
        }
        this.TrackChange(details);
      }
    }

    public DateTime DateAdded => this.dateAdded;

    public string AddedBy => this.addedBy;

    [CLSCompliant(false)]
    public CommentEntryCollection Comments => this.commentList;

    public virtual DocumentLog[] GetLinkedDocuments()
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog allDocument in this.Log.GetAllDocuments())
      {
        if (allDocument.Conditions.Contains(this))
          documentLogList.Add(allDocument);
      }
      return documentLogList.ToArray();
    }

    public void TrackChangeForCondition(string details) => this.TrackChange(details);

    public override string ToString() => this.Title;

    internal override bool IncludeInLog() => false;

    internal override bool IsSystemSpecific() => true;

    internal override bool SupportsLoanHistory() => true;
  }
}
