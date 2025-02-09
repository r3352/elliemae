// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationTimelineLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationTimelineLog : LogRecordBase
  {
    private VerificationTimelineType timelineType;
    private LoanBorrowerType borrowerType;
    private string howCompleted = string.Empty;
    private string completedBy = string.Empty;
    private DateTime dateCompleted;
    private string reviewedBy = string.Empty;
    private DateTime dateReviewed;
    private bool eFolderAttached;
    private DateTime dateUploaded;

    public VerificationTimelineLog(XmlElement e)
    {
      if (e.HasAttribute("GUID"))
        this.SetGuid(e.GetAttribute("GUID"));
      if (e.HasAttribute("CreatedOn"))
        this.Date = Utils.ParseDate((object) e.GetAttribute("CreatedOn"));
      if (!e.HasAttribute(nameof (TimelineType)))
        return;
      switch (e.GetAttribute(nameof (TimelineType)))
      {
        case "Employment":
          this.TimelineType = VerificationTimelineType.Employment;
          break;
        case "Income":
          this.TimelineType = VerificationTimelineType.Income;
          break;
        case "Asset":
          this.TimelineType = VerificationTimelineType.Asset;
          break;
        case "Obligation":
          this.TimelineType = VerificationTimelineType.Obligation;
          break;
        default:
          this.TimelineType = VerificationTimelineType.None;
          break;
      }
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode("FIELD");
      if (xmlElement == null)
        return;
      if (xmlElement.HasAttribute(nameof (BorrowerType)))
        this.BorrowerType = xmlElement.GetAttribute(nameof (BorrowerType)) == "Coborrower" ? LoanBorrowerType.Coborrower : LoanBorrowerType.Borrower;
      if (xmlElement.HasAttribute("HowToComplete"))
        this.HowCompleted = xmlElement.GetAttribute("HowToComplete");
      if (xmlElement.HasAttribute(nameof (CompletedBy)))
        this.CompletedBy = xmlElement.GetAttribute(nameof (CompletedBy));
      if (xmlElement.HasAttribute("DateToComplete"))
        this.DateCompleted = xmlElement.GetAttribute("DateToComplete") == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) xmlElement.GetAttribute("DateToComplete"));
      if (xmlElement.HasAttribute(nameof (ReviewedBy)))
        this.ReviewedBy = xmlElement.GetAttribute(nameof (ReviewedBy));
      if (xmlElement.HasAttribute("DateToReview"))
        this.DateReviewed = xmlElement.GetAttribute("DateToReview") == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) xmlElement.GetAttribute("DateToReview"));
      if (xmlElement.HasAttribute(nameof (eFolderAttached)))
        this.EFolderAttached = xmlElement.GetAttribute(nameof (eFolderAttached)) == "Y";
      if (!xmlElement.HasAttribute("DateToUpload"))
        return;
      this.DateUploaded = xmlElement.GetAttribute("DateToUpload") == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) xmlElement.GetAttribute("DateToUpload"));
    }

    public VerificationTimelineLog()
      : this(VerificationTimelineType.None, LoanBorrowerType.None, "", "", DateTime.MinValue, "", DateTime.MinValue, false, DateTime.MinValue)
    {
    }

    public VerificationTimelineLog(
      VerificationTimelineType timelineType,
      LoanBorrowerType borrowerType,
      string howCompleted,
      string completedBy,
      DateTime dateCompleted,
      string reviewedBy,
      DateTime dateReviewed,
      bool eFolderAttached,
      DateTime dateUploaded)
    {
      this.timelineType = timelineType;
      this.borrowerType = borrowerType;
      this.howCompleted = howCompleted;
      this.completedBy = completedBy;
      this.dateCompleted = dateCompleted;
      this.reviewedBy = reviewedBy;
      this.dateReviewed = dateReviewed;
      this.eFolderAttached = eFolderAttached;
      this.dateUploaded = dateUploaded;
    }

    public VerificationTimelineType TimelineType
    {
      get => this.timelineType;
      set => this.timelineType = value;
    }

    public LoanBorrowerType BorrowerType
    {
      get => this.borrowerType;
      set => this.borrowerType = value;
    }

    public string HowCompleted
    {
      get => this.howCompleted;
      set => this.howCompleted = value;
    }

    public string CompletedBy
    {
      get => this.completedBy;
      set => this.completedBy = value;
    }

    public DateTime DateCompleted
    {
      get => this.dateCompleted;
      set => this.dateCompleted = value;
    }

    public string ReviewedBy
    {
      get => this.reviewedBy;
      set => this.reviewedBy = value;
    }

    public DateTime DateReviewed
    {
      get => this.dateReviewed;
      set => this.dateReviewed = value;
    }

    public bool EFolderAttached
    {
      get => this.eFolderAttached;
      set => this.eFolderAttached = value;
    }

    public DateTime DateUploaded
    {
      get => this.dateUploaded;
      set => this.dateUploaded = value;
    }
  }
}
