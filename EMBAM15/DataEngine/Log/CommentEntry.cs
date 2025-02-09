// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.CommentEntry
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class CommentEntry
  {
    private LogRecordBase logRecord;
    private string guid;
    private DateTime date = DateTime.MinValue;
    private string comments = string.Empty;
    private string addedBy = string.Empty;
    private string addedByName = string.Empty;
    private bool isInternal = true;
    private int forRoleID = -1;
    private string reviewedBy = string.Empty;
    private DateTime reviewedDate = DateTime.MinValue;
    private bool canDeliverComment;
    private bool displayInternal;

    public CommentEntry(string comments, string addedBy, string addedByName)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.date = DateTime.Now;
      this.comments = comments;
      this.addedBy = addedBy;
      this.addedByName = addedByName;
    }

    public CommentEntry(
      string comments,
      string addedBy,
      string addedByName,
      bool isInternal,
      bool canDeliverComment = false)
      : this(comments, addedBy, addedByName)
    {
      this.isInternal = isInternal;
      this.canDeliverComment = canDeliverComment;
    }

    public CommentEntry(
      string comments,
      string addedBy,
      string addedByName,
      bool isInternal,
      int forRoleID)
      : this(comments, addedBy, addedByName, isInternal)
    {
      this.forRoleID = forRoleID;
    }

    public CommentEntry(LogRecordBase logRecord, XmlElement e)
    {
      this.logRecord = logRecord;
      if (logRecord is SellConditionLog)
        this.canDeliverComment = true;
      AttributeReader attributeReader = new AttributeReader(e);
      this.guid = attributeReader.GetString(nameof (Guid), System.Guid.NewGuid().ToString());
      this.date = attributeReader.GetDate(nameof (Date));
      this.comments = attributeReader.GetString(nameof (Comments));
      this.addedBy = attributeReader.GetString(nameof (AddedBy));
      this.addedByName = attributeReader.GetString(nameof (AddedByName));
      this.isInternal = attributeReader.GetBoolean(nameof (IsInternal), this.isInternal);
      this.forRoleID = attributeReader.GetInteger(nameof (ForRoleID), this.forRoleID);
      this.reviewedBy = attributeReader.GetString(nameof (ReviewedBy));
      this.reviewedDate = attributeReader.GetDate(nameof (ReviewedDate));
    }

    public string Guid => this.guid;

    public DateTime Date
    {
      get => this.date;
      set
      {
        if (!(this.date != value))
          return;
        this.date = value;
        this.MarkAsDirty();
      }
    }

    public string Comments
    {
      get => this.comments;
      set
      {
        if (!(this.comments != value))
          return;
        this.comments = value;
        this.MarkAsDirty();
      }
    }

    public string AddedBy
    {
      get => this.addedBy;
      set
      {
        if (!(this.addedBy != value))
          return;
        this.addedBy = value;
        this.MarkAsDirty();
      }
    }

    public string AddedByName
    {
      get => this.addedByName;
      set
      {
        if (!(this.addedByName != value))
          return;
        this.addedByName = value;
        this.MarkAsDirty();
      }
    }

    public bool IsInternal
    {
      get => this.isInternal;
      set
      {
        if (this.isInternal == value)
          return;
        this.isInternal = value;
        this.MarkAsDirty();
      }
    }

    public bool DisplayIsInternal
    {
      get => this.displayInternal;
      set => this.displayInternal = value;
    }

    public int ForRoleID
    {
      get => this.forRoleID;
      set
      {
        if (this.forRoleID == value)
          return;
        this.forRoleID = value;
        this.MarkAsDirty();
      }
    }

    public bool Reviewed => this.reviewedDate.Date != DateTime.MinValue.Date;

    public string ReviewedBy => this.reviewedBy;

    public DateTime ReviewedDate => this.reviewedDate;

    public void MarkAsReviewed(DateTime date, string user)
    {
      this.reviewedBy = user;
      this.reviewedDate = date;
      this.MarkAsDirty();
    }

    public void UnmarkAsReviewed()
    {
      this.reviewedBy = string.Empty;
      this.reviewedDate = DateTime.MinValue;
      this.MarkAsDirty();
    }

    internal void AttachToLogEntry(LogRecordBase logRecord) => this.logRecord = logRecord;

    public bool IsAttachedToLogEntry => this.logRecord != null;

    internal void MarkAsDirty()
    {
      if (this.logRecord == null)
        return;
      this.logRecord.MarkAsDirty();
    }

    public void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Guid", (object) this.guid);
      attributeWriter.Write("Date", (object) this.date);
      attributeWriter.Write("Comments", (object) this.comments);
      attributeWriter.Write("AddedBy", (object) this.addedBy);
      attributeWriter.Write("AddedByName", (object) this.addedByName);
      attributeWriter.Write("IsInternal", (object) this.isInternal);
      attributeWriter.Write("ForRoleID", (object) this.forRoleID);
      attributeWriter.Write("ReviewedBy", (object) this.reviewedBy);
      attributeWriter.Write("ReviewedDate", (object) this.reviewedDate);
    }

    public override int GetHashCode() => this.guid.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is CommentEntry && this.guid == (obj as CommentEntry).guid;
    }

    public override string ToString()
    {
      string str1 = string.Empty;
      if (this.date.Date != DateTime.MinValue.Date)
        str1 = str1 + this.date.ToString("MM/dd/yy hh:mm tt") + " ";
      if (!this.displayInternal && !this.isInternal && this.canDeliverComment)
        str1 += "External ";
      if (this.addedBy != string.Empty)
        str1 = str1 + this.addedBy + " > ";
      string str2 = str1 + this.comments;
      if (this.displayInternal)
        str2 += this.isInternal ? " - Internal" : " - External";
      return str2;
    }
  }
}
