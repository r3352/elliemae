// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DocumentLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DocumentLog : LogRecordBase
  {
    public static readonly string XmlType = "Document";
    private string title = string.Empty;
    private string groupName = string.Empty;
    private string description = string.Empty;
    private string pairId;
    private string stage = string.Empty;
    private bool isePASS;
    private string epassSignature = string.Empty;
    private bool closingDocument;
    private bool openingDocument;
    private bool preClosingDocument;
    private int daysTillDue;
    private int daysTillExpire;
    private string requestedFrom = string.Empty;
    private DateTime dateAdded = DateTime.MinValue.Date;
    private string addedBy = string.Empty;
    private DateTime dateRequested = DateTime.MinValue.Date;
    private string requestedBy = string.Empty;
    private DateTime dateRerequested = DateTime.MinValue.Date;
    private string rerequestedBy = string.Empty;
    private DateTime dateReceived = DateTime.MinValue.Date;
    private string receivedBy = string.Empty;
    private DateTime dateArchived = DateTime.MinValue.Date;
    private string archivedBy = string.Empty;
    private DateTime dateReviewed = DateTime.MinValue.Date;
    private string reviewedBy = string.Empty;
    private DateTime dateUnderwritingReady = DateTime.MinValue.Date;
    private string underwritingReadyBy = string.Empty;
    private DateTime dateShippingReady = DateTime.MinValue.Date;
    private string shippingReadyBy = string.Empty;
    private DateTime dateAccessed = DateTime.MinValue.Date;
    private string accessedBy = string.Empty;
    private DateTime dateLastAttachment = DateTime.MinValue.Date;
    private bool isWebcenter = true;
    private bool isTPOWebcenterPortal = true;
    private bool isThirdPartyDoc = true;
    private bool isExternal = true;
    private bool isAssetVerification;
    private bool isEmploymentVerification;
    private bool isIncomeVerification;
    private bool isObligationVerification;
    private IntegerIDCollection allowedRoles;
    private ObjectIDCollection linkedConditions;
    private FileAttachmentReferenceCollection linkedFiles;
    private CommentEntryCollection commentList;
    private DocumentVerificationTypeCollection verificationTypeCollection;
    private string packageId = string.Empty;
    private ConditionLogCollection conditionsAccessor;
    private DateTimeType dayCountSetting;
    private DateTime dateExpires = DateTime.MinValue.Date;
    private DateTime dateExpected = DateTime.MinValue.Date;

    public DocumentLog(string addedBy, string pairId)
    {
      this.addedBy = addedBy;
      this.dateAdded = DateTime.Now;
      this.pairId = pairId;
      this.allowedRoles = new IntegerIDCollection((LogRecordBase) this);
      this.linkedConditions = new ObjectIDCollection((LogRecordBase) this);
      this.linkedFiles = new FileAttachmentReferenceCollection(this);
      this.commentList = new CommentEntryCollection((LogRecordBase) this);
      this.verificationTypeCollection = new DocumentVerificationTypeCollection(this);
    }

    public DocumentLog(string addedBy, string pairId, System.Guid guid)
      : base(guid)
    {
      this.addedBy = addedBy;
      this.dateAdded = DateTime.Now;
      this.pairId = pairId;
      this.allowedRoles = new IntegerIDCollection((LogRecordBase) this);
      this.linkedConditions = new ObjectIDCollection((LogRecordBase) this);
      this.linkedFiles = new FileAttachmentReferenceCollection(this);
      this.commentList = new CommentEntryCollection((LogRecordBase) this);
      this.verificationTypeCollection = new DocumentVerificationTypeCollection(this);
    }

    public DocumentLog(DocumentTemplate template, string addedBy, string pairId)
      : this(addedBy, pairId)
    {
      this.title = template.Name;
      this.description = template.Description;
      this.daysTillDue = template.DaysTillDue;
      this.daysTillExpire = template.DaysTillExpire;
      this.isWebcenter = template.IsWebcenter;
      this.isTPOWebcenterPortal = template.IsTPOWebcenterPortal;
      this.isThirdPartyDoc = template.IsThirdPartyDoc;
    }

    public DocumentLog(DocumentTemplate template, string addedBy, string pairId, System.Guid guid)
      : this(addedBy, pairId, guid)
    {
      this.title = template.Name;
      this.description = template.Description;
      this.daysTillDue = template.DaysTillDue;
      this.daysTillExpire = template.DaysTillExpire;
      this.isWebcenter = template.IsWebcenter;
      this.isTPOWebcenterPortal = template.IsTPOWebcenterPortal;
      this.isThirdPartyDoc = template.IsThirdPartyDoc;
    }

    public DocumentLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.isePASS = attributeReader.GetBoolean(nameof (IsePASS));
      this.epassSignature = attributeReader.GetString(nameof (EPASSSignature));
      this.closingDocument = attributeReader.GetBoolean(nameof (ClosingDocument));
      this.preClosingDocument = attributeReader.GetBoolean(nameof (PreClosingDocument));
      this.openingDocument = attributeReader.GetBoolean("eDisclosure");
      this.dateArchived = attributeReader.GetDate("ArchiveDate");
      this.title = attributeReader.GetString(nameof (Title));
      this.groupName = attributeReader.GetString(nameof (GroupName));
      this.description = attributeReader.GetString(nameof (Description));
      this.requestedFrom = attributeReader.GetString("Company");
      this.dateRequested = attributeReader.GetDate("OrderDate");
      this.dateReceived = attributeReader.GetDate("ReceiveDate");
      this.dateRerequested = attributeReader.GetDate("ReorderDate");
      this.stage = attributeReader.GetString(nameof (Stage));
      this.pairId = attributeReader.GetString(nameof (PairId));
      if (this.pairId == string.Empty)
        this.pairId = BorrowerPair.All.Id;
      this.daysTillDue = attributeReader.GetInteger(nameof (DaysDue), 0);
      this.daysTillExpire = attributeReader.GetInteger(nameof (DaysTillExpire), 0);
      this.dateAdded = attributeReader.GetDate(nameof (DateAdded));
      this.addedBy = attributeReader.GetString(nameof (AddedBy));
      this.requestedBy = attributeReader.GetString(nameof (RequestedBy));
      this.rerequestedBy = attributeReader.GetString(nameof (RerequestedBy));
      this.receivedBy = attributeReader.GetString(nameof (ReceivedBy));
      this.archivedBy = attributeReader.GetString(nameof (ArchivedBy));
      this.dateReviewed = attributeReader.GetDate("ReviewedDate");
      this.reviewedBy = attributeReader.GetString(nameof (ReviewedBy));
      this.dateAccessed = attributeReader.GetDate("AccessedDate");
      this.accessedBy = attributeReader.GetString(nameof (AccessedBy));
      this.dateLastAttachment = attributeReader.GetDate("LastAttachmentDate");
      this.dateUnderwritingReady = attributeReader.GetDate("UnderwritingReadyDate");
      this.underwritingReadyBy = attributeReader.GetString(nameof (UnderwritingReadyBy));
      this.dateShippingReady = attributeReader.GetDate("ShippingReadyDate");
      this.shippingReadyBy = attributeReader.GetString(nameof (ShippingReadyBy));
      this.isIncomeVerification = attributeReader.GetBoolean(nameof (IsIncomeVerification));
      this.isAssetVerification = attributeReader.GetBoolean(nameof (IsAssetVerification));
      this.isEmploymentVerification = attributeReader.GetBoolean(nameof (IsEmploymentVerification));
      this.isObligationVerification = attributeReader.GetBoolean(nameof (IsObligationVerification));
      this.packageId = attributeReader.GetString(nameof (PackageId));
      if (!string.IsNullOrEmpty(attributeReader.GetString(nameof (IsWebcenter), (string) null)))
      {
        this.isWebcenter = attributeReader.GetBoolean(nameof (IsWebcenter), true);
        this.isTPOWebcenterPortal = attributeReader.GetBoolean(nameof (IsTPOWebcenterPortal), true);
        this.isThirdPartyDoc = attributeReader.GetBoolean(nameof (IsThirdPartyDoc), true);
      }
      else
      {
        this.isWebcenter = attributeReader.GetBoolean(nameof (IsExternal), true);
        this.isTPOWebcenterPortal = this.isWebcenter;
        this.isThirdPartyDoc = this.isWebcenter;
      }
      this.linkedConditions = new ObjectIDCollection((LogRecordBase) this, e, nameof (Conditions));
      this.linkedFiles = new FileAttachmentReferenceCollection(this, e, nameof (Files));
      this.allowedRoles = new IntegerIDCollection((LogRecordBase) this, e, nameof (AllowedRoles));
      this.commentList = new CommentEntryCollection((LogRecordBase) this, e, nameof (Comments));
      this.verificationTypeCollection = new DocumentVerificationTypeCollection(this, e, "Verification");
      this.MarkAsClean();
      this.dayCountSetting = log.Loan.DocumentDateTimeType;
      this.calculateExpectedDate();
      this.calculateExpirationDate();
    }

    public string Title
    {
      get => this.title;
      set
      {
        if (!(this.title != value))
          return;
        this.title = value;
        this.TrackChange("Doc Name changed to \"" + this.title + "\"");
      }
    }

    public string GroupName
    {
      get => this.groupName;
      set
      {
        if (!(this.groupName != value))
          return;
        this.groupName = value;
        this.TrackChange("Group Name changed to \"" + this.groupName + "\"");
      }
    }

    public string Description
    {
      get => this.description;
      set
      {
        if (!(this.description != value))
          return;
        this.description = value;
        this.TrackChange("Doc Description changed to \"" + this.description + "\"");
      }
    }

    public string PairId
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

    public string Stage
    {
      get => this.stage;
      set
      {
        if (!(this.stage != value))
          return;
        this.stage = value;
        string details = "For Milestone changed";
        if (this.IsAttachedToLog)
        {
          MilestoneLog milestone = this.Log.GetMilestone(this.stage);
          if (milestone != null)
            details = details + " to \"" + milestone.Stage + "\"";
        }
        this.TrackChange(details);
      }
    }

    public string RequestedFrom
    {
      get => this.requestedFrom;
      set
      {
        if (!(this.requestedFrom != value))
          return;
        this.requestedFrom = value;
        this.TrackChange("Doc requested from \"" + this.requestedFrom + "\"");
      }
    }

    public bool IsePASS
    {
      get => this.isePASS;
      set
      {
        if (this.isePASS == value)
          return;
        this.isePASS = value;
        this.MarkAsDirty();
      }
    }

    public string EPASSSignature
    {
      get => this.epassSignature;
      set
      {
        if (!(this.epassSignature != value))
          return;
        this.epassSignature = value;
        this.MarkAsDirty();
      }
    }

    public bool ClosingDocument
    {
      get => this.closingDocument;
      set
      {
        if (this.closingDocument == value)
          return;
        this.closingDocument = value;
        this.MarkAsDirty();
      }
    }

    public bool PreClosingDocument
    {
      get => this.preClosingDocument;
      set
      {
        if (this.preClosingDocument == value)
          return;
        this.preClosingDocument = value;
        this.MarkAsDirty();
      }
    }

    public bool OpeningDocument
    {
      get => this.openingDocument;
      set
      {
        if (this.openingDocument == value)
          return;
        this.openingDocument = value;
        this.MarkAsDirty();
      }
    }

    public bool IsWebcenter
    {
      get => this.isWebcenter;
      set
      {
        if (this.isWebcenter == value)
          return;
        this.isWebcenter = value;
        string str = "Available to Webcenter";
        this.TrackChange(!this.isWebcenter ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsTPOWebcenterPortal
    {
      get => this.isTPOWebcenterPortal;
      set
      {
        if (this.isTPOWebcenterPortal == value)
          return;
        this.isTPOWebcenterPortal = value;
        string str = "Available to TPO Portal";
        this.TrackChange(!this.isTPOWebcenterPortal ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsThirdPartyDoc
    {
      get => this.isThirdPartyDoc;
      set
      {
        if (this.isThirdPartyDoc == value)
          return;
        this.isThirdPartyDoc = value;
        string str = "Available to EDM Lenders and other service providers";
        this.TrackChange(!this.isThirdPartyDoc ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsExternal
    {
      get => this.isExternal;
      set
      {
        if (this.isExternal == value)
          return;
        this.isExternal = value;
        string str = "Available Externally";
        this.TrackChange(!this.isExternal ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsAssetVerification
    {
      get => this.isAssetVerification;
      set
      {
        if (this.isAssetVerification == value)
          return;
        this.isAssetVerification = value;
        string str = "Is An Asset Verification";
        this.TrackChange(!this.isAssetVerification ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsEmploymentVerification
    {
      get => this.isEmploymentVerification;
      set
      {
        if (this.isEmploymentVerification == value)
          return;
        this.isEmploymentVerification = value;
        string str = "Is An Employment Verification";
        this.TrackChange(!this.isEmploymentVerification ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsObligationVerification
    {
      get => this.isObligationVerification;
      set
      {
        if (this.isObligationVerification == value)
          return;
        this.isObligationVerification = value;
        string str = "Is An Obligation Verification";
        this.TrackChange(!this.isObligationVerification ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsIncomeVerification
    {
      get => this.isIncomeVerification;
      set
      {
        if (this.isIncomeVerification == value)
          return;
        this.isIncomeVerification = value;
        string str = "Is An Income Verification";
        this.TrackChange(!this.isIncomeVerification ? str + " unchecked" : str + " checked");
      }
    }

    [CLSCompliant(false)]
    public CommentEntryCollection Comments => this.commentList;

    public DocumentVerificationTypeCollection Verifications => this.verificationTypeCollection;

    public string PackageId
    {
      get => this.packageId;
      set
      {
        if (!(this.packageId != value))
          return;
        this.packageId = value;
      }
    }

    public DateTime DateAdded => this.dateAdded;

    public string AddedBy => this.addedBy;

    public bool Requested => this.dateRequested.Date != DateTime.MinValue.Date;

    public DateTime DateRequested => this.dateRequested;

    public string RequestedBy => this.requestedBy;

    public void MarkAsRequested(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateRequested != date)
      {
        if (!this.Requested)
          stringList.Add("Status Requested checked");
        this.dateRequested = date;
        stringList.Add("Status Requested date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.requestedBy != user)
      {
        this.requestedBy = user;
        stringList.Add("Status Requested by \"" + user + "\"");
      }
      this.calculateExpectedDate();
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsRequested()
    {
      this.dateRequested = DateTime.MinValue;
      this.requestedBy = string.Empty;
      this.calculateExpectedDate();
      this.TrackChange("Status Requested unchecked");
    }

    public bool Rerequested => this.dateRerequested.Date != DateTime.MinValue.Date;

    public DateTime DateRerequested => this.dateRerequested;

    public string RerequestedBy => this.rerequestedBy;

    public void MarkAsRerequested(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateRerequested != date)
      {
        if (!this.Rerequested)
          stringList.Add("Status Re-requested checked");
        this.dateRerequested = date;
        stringList.Add("Status Re-requested date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.rerequestedBy != user)
      {
        this.rerequestedBy = user;
        stringList.Add("Status Re-requested by \"" + user + "\"");
      }
      this.calculateExpectedDate();
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsRerequested()
    {
      this.dateRerequested = DateTime.MinValue;
      this.rerequestedBy = string.Empty;
      this.calculateExpectedDate();
      this.TrackChange("Status Re-requested unchecked");
    }

    public int DaysDue
    {
      get => this.daysTillDue;
      set
      {
        int num = Math.Max(0, value);
        if (this.daysTillDue == num)
          return;
        this.daysTillDue = num;
        this.calculateExpectedDate();
        this.TrackChange("Days to Receive changed to \"" + (object) this.daysTillDue + "\"");
      }
    }

    public bool Expected => this.dateExpected.Date != DateTime.MinValue.Date;

    public DateTime DateExpected => this.dateExpected.Date;

    public bool IsPastDue
    {
      get => !this.Received && this.Expected && this.dateExpected.Date <= DateTime.Today;
    }

    private void calculateExpectedDate()
    {
      this.dateExpected = DateTime.MinValue;
      if (this.DaysDue <= 0)
        return;
      if (this.Rerequested)
      {
        this.dateExpected = DatetimeUtils.GetDateTime(this.dateRerequested, this.dayCountSetting, this.daysTillDue);
      }
      else
      {
        if (!this.Requested)
          return;
        this.dateExpected = DatetimeUtils.GetDateTime(this.dateRequested, this.dayCountSetting, this.daysTillDue);
      }
    }

    public bool Received => this.dateReceived.Date != DateTime.MinValue.Date;

    public DateTime DateReceived => this.dateReceived;

    public string ReceivedBy => this.receivedBy;

    public void MarkAsReceived(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateReceived != date)
      {
        if (!this.Received)
          stringList.Add("Status Received checked");
        this.dateReceived = date;
        stringList.Add("Status Received date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.receivedBy != user)
      {
        this.receivedBy = user;
        stringList.Add("Status Received by \"" + user + "\"");
      }
      this.calculateExpirationDate();
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsReceived()
    {
      this.dateReceived = DateTime.MinValue;
      this.receivedBy = string.Empty;
      this.calculateExpirationDate();
      this.TrackChange("Status Received unchecked");
    }

    public int DaysTillExpire
    {
      get => this.daysTillExpire;
      set
      {
        int num = Math.Max(0, value);
        if (this.daysTillExpire == num)
          return;
        this.daysTillExpire = num;
        this.calculateExpirationDate();
        this.TrackChange("Days to Expire changed to \"" + (object) this.daysTillExpire + "\"");
      }
    }

    public bool Expires => this.dateExpires.Date != DateTime.MinValue.Date;

    public DateTime DateExpires => this.dateExpires.Date;

    public bool IsExpired => this.Expires && this.dateExpires.Date < DateTime.Today;

    private void calculateExpirationDate()
    {
      this.dateExpires = DateTime.MinValue;
      if (this.daysTillExpire <= 0 || !this.Received)
        return;
      this.dateExpires = DatetimeUtils.GetDateTime(this.dateReceived, this.dayCountSetting, this.daysTillExpire);
    }

    public string Status
    {
      get
      {
        if (this.IsExpired)
          return "expired!";
        if (this.ShippingReady)
          return "ready to ship";
        if (this.UnderwritingReady)
          return "ready for UW";
        if (this.Reviewed)
          return "reviewed";
        if (this.Received)
          return "received";
        if (this.IsPastDue)
          return "expected!";
        if (this.Expected)
          return "expected";
        if (this.Rerequested)
          return "reordered";
        return this.Requested ? "ordered" : "needed";
      }
    }

    public override DateTime Date
    {
      get
      {
        switch (this.Status)
        {
          case "expected":
            return this.dateExpected;
          case "expected!":
            return this.dateExpected;
          case "expired!":
            return this.dateExpires;
          case "ordered":
            return this.dateRequested;
          case "ready for UW":
            return this.dateUnderwritingReady;
          case "ready to ship":
            return this.dateShippingReady;
          case "received":
            return this.dateReceived;
          case "reordered":
            return this.dateRerequested;
          case "reviewed":
            return this.dateReviewed;
          default:
            return DateTime.MinValue;
        }
      }
    }

    public bool Archived => this.dateArchived.Date != DateTime.MinValue.Date;

    public DateTime DateArchived => this.dateArchived;

    public string ArchivedBy => this.archivedBy;

    public void MarkAsArchived(DateTime date, string user)
    {
      this.dateArchived = date;
      this.archivedBy = user;
      this.TrackChange("Doc archived");
    }

    public void UnmarkAsArchived()
    {
      this.dateArchived = DateTime.MinValue;
      this.archivedBy = string.Empty;
      this.MarkAsDirty();
    }

    public void GrantAccess(int roleId)
    {
      if (this.allowedRoles.Contains(roleId))
        return;
      this.allowedRoles.Add(roleId);
      this.MarkLastUpdated();
      if (!this.IsAttachedToLog || this.Log.Loan.LoanHistoryMonitor == null)
        return;
      RoleInfo role = this.Log.Loan.Settings.GetRole(roleId);
      if (role == null)
        return;
      this.Log.Loan.LoanHistoryMonitor.TrackChange((LogRecordBase) this, "Access granted to " + role.RoleAbbr);
    }

    public void RemoveAccess(int roleId)
    {
      if (!this.allowedRoles.Contains(roleId))
        return;
      this.allowedRoles.Remove(roleId);
      this.MarkLastUpdated();
      if (!this.IsAttachedToLog || this.Log.Loan.LoanHistoryMonitor == null)
        return;
      RoleInfo role = this.Log.Loan.Settings.GetRole(roleId);
      if (role == null)
        return;
      this.Log.Loan.LoanHistoryMonitor.TrackChange((LogRecordBase) this, "Access removed from " + role.RoleAbbr);
    }

    public int[] AllowedRoles => this.allowedRoles.ToArray();

    public bool IsAccessibleToRole(int roleId)
    {
      return this.allowedRoles.Count <= 0 || this.allowedRoles.Contains(roleId);
    }

    public bool IsAccessibleToAnyRole(int[] roleIds)
    {
      foreach (int roleId in roleIds)
      {
        if (this.IsAccessibleToRole(roleId))
          return true;
      }
      return false;
    }

    public ConditionLogCollection Conditions
    {
      get
      {
        if (this.conditionsAccessor == null)
          this.conditionsAccessor = new ConditionLogCollection(this, this.linkedConditions);
        return this.conditionsAccessor;
      }
    }

    internal override void RemoveReferencesTo(LogRecordBase record)
    {
      if (record == this)
      {
        this.Conditions.Clear(false);
      }
      else
      {
        if (!(record is ConditionLog))
          return;
        this.Conditions.Remove(record as ConditionLog);
      }
    }

    public FileAttachmentReferenceCollection Files => this.linkedFiles;

    public bool Reviewed => this.dateReviewed.Date != DateTime.MinValue.Date;

    public DateTime DateReviewed => this.dateReviewed;

    public string ReviewedBy => this.reviewedBy;

    public void MarkAsReviewed(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateReviewed != date)
      {
        if (!this.Reviewed)
          stringList.Add("Status Reviewed checked");
        this.dateReviewed = date;
        stringList.Add("Status Reviewed date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.reviewedBy != user)
      {
        this.reviewedBy = user;
        stringList.Add("Status Reviewed by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsReviewed()
    {
      this.dateReviewed = DateTime.MinValue;
      this.reviewedBy = string.Empty;
      this.TrackChange("Status Reviewed unchecked");
    }

    public bool UnderwritingReady => this.dateUnderwritingReady.Date != DateTime.MinValue.Date;

    public DateTime DateUnderwritingReady => this.dateUnderwritingReady;

    public string UnderwritingReadyBy => this.underwritingReadyBy;

    public void MarkAsUnderwritingReady(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateUnderwritingReady != date)
      {
        if (!this.UnderwritingReady)
          stringList.Add("Status Ready for UW checked");
        this.dateUnderwritingReady = date;
        stringList.Add("Status Ready for UW date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.underwritingReadyBy != user)
      {
        this.underwritingReadyBy = user;
        stringList.Add("Status Ready for UW by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsUnderwritingReady()
    {
      this.dateUnderwritingReady = DateTime.MinValue;
      this.underwritingReadyBy = string.Empty;
      this.TrackChange("Status Ready for UW unchecked");
    }

    public bool ShippingReady => this.dateShippingReady.Date != DateTime.MinValue.Date;

    public DateTime DateShippingReady => this.dateShippingReady;

    public string ShippingReadyBy => this.shippingReadyBy;

    public void MarkAsShippingReady(DateTime date, string user)
    {
      List<string> stringList = new List<string>();
      if (this.dateShippingReady != date)
      {
        if (!this.ShippingReady)
          stringList.Add("Status Ready for Shipping checked");
        this.dateShippingReady = date;
        stringList.Add("Status Ready for Shipping date set to \"" + date.ToString("MM/dd/yy hh:mm tt") + "\"");
      }
      if (this.shippingReadyBy != user)
      {
        this.shippingReadyBy = user;
        stringList.Add("Status Ready for Shipping by \"" + user + "\"");
      }
      this.TrackChanges(stringList.ToArray());
    }

    public void UnmarkAsShippingReady()
    {
      this.dateShippingReady = DateTime.MinValue;
      this.shippingReadyBy = string.Empty;
      this.TrackChange("Status Ready for Shipping unchecked");
    }

    public bool Accessed => this.dateAccessed.Date != DateTime.MinValue.Date;

    public DateTime DateAccessed => this.dateAccessed;

    public string AccessedBy => this.accessedBy;

    public void MarkAsAccessed(DateTime date, string user)
    {
      bool flag = false;
      if (this.dateAccessed != date)
      {
        this.dateAccessed = date;
        flag = true;
      }
      if (this.accessedBy != user)
      {
        this.accessedBy = user;
        flag = true;
      }
      if (!flag)
        return;
      this.MarkAsDirty();
    }

    public DateTime DateLastAttachment => this.dateLastAttachment;

    public void MarkLastAttachment()
    {
      this.dateLastAttachment = DateTime.Now;
      this.MarkAsDirty();
    }

    internal override void AttachToLog(LogList log)
    {
      this.allowedRoles.AddRange(log.Loan.AccessRules.GetDefaultRoleAccessForDocument(this));
      this.dayCountSetting = log.Loan.DocumentDateTimeType;
      this.calculateExpectedDate();
      this.calculateExpirationDate();
      base.AttachToLog(log);
      log.Loan.LoanHistoryMonitor.TrackChange((LogRecordBase) this, "Doc created");
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      if (this.Expires)
      {
        PipelineInfo.Alert alert = new PipelineInfo.Alert(17, this.title + " " + this.requestedFrom, "expired!", this.dateExpires, this.Guid, this.Guid);
        alertList.Add(alert);
      }
      else if (!this.Received && this.Expected)
      {
        PipelineInfo.Alert alert = new PipelineInfo.Alert(2, this.title + " " + this.requestedFrom, "expected", this.dateExpected, this.Guid, this.Guid);
        alertList.Add(alert);
      }
      alertList.AddRange((IEnumerable<PipelineInfo.Alert>) this.commentList.GetPipelineAlerts());
      return alertList.ToArray();
    }

    public string GetDocumentType(DocumentTrackingSetup docSetup)
    {
      if (this.IsePASS || Epass.IsEpassDoc(this.Title))
        return "Settlement Service";
      if (this.ClosingDocument)
        return "Closing Document";
      if (this.PreClosingDocument)
        return "PreClosing Document";
      if (this.OpeningDocument)
        return "eDisclosure";
      if (this is VerifLog)
        return "Verification";
      DocumentTemplate byName = docSetup.GetByName(this.Title);
      return byName == null || byName.SourceType == "Settlement Service" ? "Needed" : byName.SourceType;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DocumentLog.XmlType);
      attributeWriter.Write("IsePASS", (object) this.isePASS);
      attributeWriter.Write("EPASSSignature", (object) this.epassSignature);
      attributeWriter.Write("ClosingDocument", (object) this.closingDocument);
      attributeWriter.Write("PreClosingDocument", (object) this.preClosingDocument);
      attributeWriter.Write("eDisclosure", (object) this.openingDocument);
      attributeWriter.Write("ArchiveDate", (object) this.dateArchived);
      attributeWriter.Write("Title", (object) this.title);
      attributeWriter.Write("GroupName", (object) this.groupName);
      attributeWriter.Write("Description", (object) this.description);
      attributeWriter.Write("Company", (object) this.requestedFrom);
      attributeWriter.Write("OrderDate", (object) this.dateRequested);
      attributeWriter.Write("ReceiveDate", (object) this.dateReceived);
      attributeWriter.Write("ReorderDate", (object) this.dateRerequested);
      attributeWriter.Write("Stage", (object) this.stage);
      attributeWriter.Write("PairId", (object) this.pairId);
      attributeWriter.Write("DaysDue", (object) this.daysTillDue);
      attributeWriter.Write("DaysTillExpire", (object) this.daysTillExpire);
      attributeWriter.Write("DateAdded", (object) this.dateAdded);
      attributeWriter.Write("AddedBy", (object) this.addedBy);
      attributeWriter.Write("RequestedBy", (object) this.requestedBy);
      attributeWriter.Write("RerequestedBy", (object) this.rerequestedBy);
      attributeWriter.Write("ReceivedBy", (object) this.receivedBy);
      attributeWriter.Write("ArchivedBy", (object) this.archivedBy);
      attributeWriter.Write("IsWebcenter", (object) this.isWebcenter);
      attributeWriter.Write("IsTPOWebcenterPortal", (object) this.isTPOWebcenterPortal);
      attributeWriter.Write("IsThirdPartyDoc", (object) this.isThirdPartyDoc);
      attributeWriter.Write("IsObligationVerification", (object) this.isObligationVerification);
      attributeWriter.Write("IsAssetVerification", (object) this.isAssetVerification);
      attributeWriter.Write("IsEmploymentVerification", (object) this.isEmploymentVerification);
      attributeWriter.Write("IsIncomeVerification", (object) this.isIncomeVerification);
      attributeWriter.Write("ReviewedDate", (object) this.dateReviewed);
      attributeWriter.Write("ReviewedBy", (object) this.reviewedBy);
      attributeWriter.Write("AccessedDate", (object) this.dateAccessed);
      attributeWriter.Write("AccessedBy", (object) this.accessedBy);
      attributeWriter.Write("LastAttachmentDate", (object) this.dateLastAttachment);
      attributeWriter.Write("UnderwritingReadyDate", (object) this.dateUnderwritingReady);
      attributeWriter.Write("UnderwritingReadyBy", (object) this.underwritingReadyBy);
      attributeWriter.Write("ShippingReadyDate", (object) this.dateShippingReady);
      attributeWriter.Write("ShippingReadyBy", (object) this.shippingReadyBy);
      attributeWriter.Write("PackageId", (object) this.packageId);
      this.linkedConditions.ToXml(e, "Conditions");
      this.linkedFiles.ToXml(e, "Files");
      this.allowedRoles.ToXml(e, "AllowedRoles");
      this.commentList.ToXml(e, "Comments");
      this.verificationTypeCollection.ToXml(e, "Verification");
    }

    public override string ToString()
    {
      return this.requestedFrom != string.Empty ? this.title + " - " + this.requestedFrom : this.title;
    }

    internal override bool IsSystemSpecific() => true;

    internal override bool SupportsLoanHistory() => true;

    public void MarkAsFinalCD(LoanData loanData, string docTemplateSource, string signatureType)
    {
      switch (docTemplateSource)
      {
        case "Closing Disclosure":
          loanData.SetField("UCD.X1", this.Guid);
          loanData.SetField("UCD.X2", signatureType);
          break;
        case "Closing Disclosure (Alternate)":
          loanData.SetField("UCD.X3", this.Guid);
          loanData.SetField("UCD.X4", signatureType);
          break;
        case "Closing Disclosure (Seller)":
          loanData.SetField("UCD.X5", this.Guid);
          loanData.SetField("UCD.X6", signatureType);
          break;
      }
      this.MarkAsDirty();
    }

    public void UnmarkAsFinalCD(LoanData loanData)
    {
      if (this.Guid == loanData.GetField("UCD.X1"))
      {
        loanData.SetField("UCD.X1", string.Empty);
        loanData.SetField("UCD.X2", string.Empty);
      }
      if (this.Guid == loanData.GetField("UCD.X3"))
      {
        loanData.SetField("UCD.X3", string.Empty);
        loanData.SetField("UCD.X4", string.Empty);
      }
      if (this.Guid == loanData.GetField("UCD.X5"))
      {
        loanData.SetField("UCD.X5", string.Empty);
        loanData.SetField("UCD.X6", string.Empty);
      }
      this.MarkAsDirty();
    }
  }
}
