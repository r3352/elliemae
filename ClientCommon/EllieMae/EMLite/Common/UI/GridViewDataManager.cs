// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.GridViewDataManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class GridViewDataManager
  {
    private Sessions.Session session;
    private GridView gridView;
    private LoanDataMgr loanDataMgr;
    private GridViewLayoutManager layoutMgr;
    private GridViewFilterManager filterMgr;
    private DocumentTrackingSetup docSetup;
    private Hashtable roleTable;
    private Hashtable roleNameTable;
    private readonly StringEnum sourceOfCondStringEnum = new StringEnum(typeof (SourceOfCondition));
    private const string ACCESSEDBY = "ACCESSEDBY";
    private const string ACCESSEDDATE = "ACCESSEDDATE";
    private const string ACTIVE = "ACTIVE";
    private const string ADDEDBY = "ADDEDBY";
    private const string ADDEDDATE = "ADDEDDATE";
    private const string ALLOWTOCLEAR = "ALLOWTOCLEAR";
    private const string ARCHIVEDDATE = "ARCHIVEDDATE";
    private const string AVAILABLEEXTERNALLY = "AVAILABLEEXTERNALLY";
    private const string BORROWER = "BORROWER";
    private const string CATEGORY = "CATEGORY";
    private const string CHECKBOX = "CHECKBOX";
    private const string CLEAREDBY = "CLEAREDBY";
    private const string CLEAREDDATE = "CLEAREDDATE";
    private const string CLOSINGDOCUMENT = "CLOSINGDOCUMENT";
    private const string COMMENT = "COMMENT";
    private const string COMMENTCOUNT = "COMMENTCOUNT";
    private const string CONDSOURCE = "CONDSOURCE";
    private const string CONDSOURCEENHANCED = "CONDSOURCEENHANCED";
    private const string CONDSTATUS = "CONDSTATUS";
    private const string CONDSTATUSDATE = "CONDSTATUSDATE";
    private const string CONDLATESTSTATUS = "CONDLATESTSTATUS";
    private const string CONDSTATUSDATETIME = "CONDSTATUSDATETIME";
    private const string CONDSTATUSUSER = "CONDSTATUSUSER";
    private const string CONVERSIONTYPE = "CONVERSIONTYPE";
    private const string CURRENTROW = "CURRENTROW";
    private const string DATE = "DATE";
    private const string DATETIME = "DATETIME";
    private const string DAYSTILLDUE = "DAYSTILLDUE";
    private const string DAYSTILLEXPIRE = "DAYSTILLEXPIRE";
    private const string DESCRIPTION = "DESCRIPTION";
    private const string DISPOSITION = "DISPOSITION";
    private const string DOCACCESS = "DOCACCESS";
    private const string DOCSOURCE = "DOCSOURCE";
    private const string DOCSTATUS = "DOCSTATUS";
    private const string DOCTYPE = "DOCTYPE";
    private const string DOCUMENTCOUNT = "DOCUMENTCOUNT";
    private const string DOCUMENTRECEIPTDATE = "DOCUMENTRECEIPTDATE";
    private const string EVENT = "EVENT";
    private const string EFFECTIVEENDDATE = "EFFECTIVEENDDATE";
    private const string EFFECTIVESTARTDATE = "EFFECTIVESTARTDATE";
    private const string EXPECTEDDATE = "EXPECTEDDATE";
    private const string EXPIRATIONDATE = "EXPIRATIONDATE";
    private const string EMAILMESSAGE = "EMAILMESSAGE";
    private const string EMAILREQUIRED = "EMAILREQUIRED";
    private const string EXTERNALDESCRIPTION = "EXTERNALDESCRIPTION";
    private const string CONDITIONTYPE = "CONDITIONTYPE";
    private const string EXTERNALID = "EXTERNALID";
    private const string EMAILTEMPLATETYPE = "EMAILTEMPLATETYPE";
    private const string FILESIZE = "FILESIZE";
    private const string FULFILLEDBY = "FULFILLEDBY";
    private const string FULFILLEDDATE = "FULFILLEDDATE";
    private const string HASATTACHMENTS = "HASATTACHMENTS";
    private const string HASCONDITIONS = "HASCONDITIONS";
    private const string HASDOCUMENTS = "HASDOCUMENTS";
    private const string INTENDEDFOR = "INTENDEDFOR";
    private const string LASTATTACHMENT = "LASTATTACHMENT";
    private const string LASTUPDATED = "LASTUPDATED";
    private const string MAINTAINORIGINAL = "MAINTAINORIGINAL";
    private const string MILESTONE = "MILESTONE";
    private const string NAME = "NAME";
    private const string NAMEWITHICON = "NAMEWITHICON";
    private const string OBJECTTYPE = "OBJECTTYPE";
    private const string OPENINGDOCUMENT = "OPENINGDOCUMENT";
    private const string OWNER = "OWNER";
    private const string OWNERNAME = "OWNERNAME";
    private const string PRECLOSINGDOCUMENT = "PRECLOSINGDOCUMENT";
    private const string PRINTEXTERNALLY = "PRINTEXTERNALLY";
    private const string PRINTINTERNALLY = "PRINTINTERNALLY";
    private const string PRINTINTERNALENHANCED = "PRINTINTERNALENHANCED";
    private const string PRIORTO = "PRIORTO";
    private const string PRIORTOENHANCED = "PRIORTOENHANCED";
    private const string RECEIVEDBY = "RECEIVEDBY";
    private const string RECEIVEDDATE = "RECEIVEDDATE";
    private const string RECIPIENT = "RECIPIENT";
    private const string RECIPIENTENHANCED = "RECIPIENTENHANCED";
    private const string REJECTEDBY = "REJECTEDBY";
    private const string REJECTEDDATE = "REJECTEDDATE";
    private const string REQUESTEDBY = "REQUESTEDBY";
    private const string REQUESTEDDATE = "REQUESTEDDATE";
    private const string REQUESTEDFROM = "REQUESTEDFROM";
    private const string REQUIRED = "REQUIRED";
    private const string REREQUESTEDBY = "REREQUESTEDBY";
    private const string REREQUESTEDDATE = "REREQUESTEDDATE";
    private const string REVIEWEDBY = "REVIEWEDBY";
    private const string REVIEWEDDATE = "REVIEWEDDATE";
    private const string SENDER = "SENDER";
    private const string SENTBY = "SENTBY";
    private const string SENTDATE = "SENTDATE";
    private const string SHIPPINGREADYBY = "SHIPPINGREADYBY";
    private const string SHIPPINGREADYDATE = "SHIPPINGREADYDATE";
    private const string SIGNATURETYPE = "SIGNATURETYPE";
    private const string SOURCEOFCONDITION = "SOURCEOFCONDITION";
    private const string STATUSTRIGGER = "STATUSTRIGGER";
    private const string UNDERWRITERACCESS = "UNDERWRITERACCESS";
    private const string UNDERWRITINGREADYBY = "UNDERWRITINGREADYBY";
    private const string UNDERWRITINGREADYDATE = "UNDERWRITINGREADYDATE";
    private const string UPDATEMETHOD = "UPDATEMETHOD";
    private const string USER = "USER";
    private const string WAIVEDBY = "WAIVEDBY";
    private const string WAIVEDDATE = "WAIVEDDATE";
    private const string TOTALFILESIZE = "TOTALFILESIZE";
    private const string LOANNUMBER = "LOANNUMBER";
    private const string PROGRESSSTATUS = "PROGRESSSTATUS";
    private const string MARKEDASFINAL = "MARKEDASFINAL";
    private const string CONSUMERCONNECT = " CONSUMERCONNECT";
    private const string WEBCENTER = " WEBCENTER";
    private const string SOURCEID = " SOURCEID";
    private const string XREFID = " XREFID";
    private const string CONDITIONDIRECTION = " CONDITIONDIRECTION";
    private const string ID = "ID";
    private const string InternalId = "InternalId";
    private const string INTERNALDESCRIPTION = "INTERNALDESCRIPTION";
    private const string TYPE = "TYPE";
    private const string CONDITIONSTATUS = "CONDITIONSTATUS";
    private const string LASTMODIFIEDBY = "LASTMODIFIEDBY";
    private const string LASTMODIFIEDDATETIME = "LASTMODIFIEDDATETIME";
    private const string CONDITIONCODE = "CONDITIONCODE";
    private const string CUSTOMIZED = "CUSTOMIZED";
    private const string AIQTAGS = "TAGS";
    private const string AIQADDITIONALINFO = "ADDITIONALINFO";
    private const string AIQSTATUS = "STATUS";
    private const string AIQID = "ID";
    private const string AIQLASTUPDATED = "LASTUPDATED";
    private const string PARTNER = "PARTNER";
    private const string STANDARDVIEWNAME = "Standard View";
    private const string ALLDOCUMENTGROUPNAME = "(All Documents)";
    private const string DEFAULTSTACKINGORDERNAME = "None";

    public event EventHandler LayoutChanged;

    public event EventHandler FilterChanged;

    public GridViewDataManager(GridView gridView, LoanDataMgr loanDataMgr)
      : this(Session.DefaultInstance, gridView, loanDataMgr)
    {
    }

    public GridViewDataManager(
      Sessions.Session session,
      GridView gridView,
      LoanDataMgr loanDataMgr)
    {
      this.session = session;
      this.gridView = gridView;
      this.loanDataMgr = loanDataMgr;
      if (loanDataMgr == null)
        return;
      this.docSetup = loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
    }

    public static TableLayout.Column AccessedByColumn
    {
      get => new TableLayout.Column("ACCESSEDBY", "Accessed By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column AccessedDateColumn
    {
      get => new TableLayout.Column("ACCESSEDDATE", "Accessed On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column ActiveColumn
    {
      get => new TableLayout.Column("ACTIVE", "Current Version", HorizontalAlignment.Center, 95);
    }

    public static TableLayout.Column AddedByColumn
    {
      get => new TableLayout.Column("ADDEDBY", "Added By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column AddedDateColumn
    {
      get => new TableLayout.Column("ADDEDDATE", "Added On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column AllowToClearColumn
    {
      get => new TableLayout.Column("ALLOWTOCLEAR", "Allow to Clear", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column ArchivedDateColumn
    {
      get => new TableLayout.Column("ARCHIVEDDATE", "Archived On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column AvailableExternallyColumn
    {
      get
      {
        return new TableLayout.Column("AVAILABLEEXTERNALLY", "External", HorizontalAlignment.Left, 50);
      }
    }

    public static TableLayout.Column BorrowerColumn
    {
      get => new TableLayout.Column("BORROWER", "For Borrower Pair", HorizontalAlignment.Left, 145);
    }

    public static TableLayout.Column CategoryColumn
    {
      get => new TableLayout.Column("CATEGORY", "Category", HorizontalAlignment.Left, 70);
    }

    public static TableLayout.Column CheckBoxColumn
    {
      get => new TableLayout.Column("CHECKBOX", string.Empty, HorizontalAlignment.Left, 24);
    }

    public static TableLayout.Column ClearedByColumn
    {
      get => new TableLayout.Column("CLEAREDBY", "Cleared By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column ClearedDateColumn
    {
      get => new TableLayout.Column("CLEAREDDATE", "Cleared On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column ClosingDocumentColumn
    {
      get => new TableLayout.Column("CLOSINGDOCUMENT", "Closing Doc", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column CommentColumn
    {
      get => new TableLayout.Column("COMMENT", "Comment", HorizontalAlignment.Left, 110);
    }

    public static TableLayout.Column CommentCountColumn
    {
      get => new TableLayout.Column("COMMENTCOUNT", "Comment", HorizontalAlignment.Left, 60);
    }

    public static TableLayout.Column CondNameColumn
    {
      get => new TableLayout.Column("NAME", "Condition Name", HorizontalAlignment.Left, 300);
    }

    public static TableLayout.Column CondSourceColumn
    {
      get => new TableLayout.Column("CONDSOURCE", "Source", HorizontalAlignment.Left, 100);
    }

    public static TableLayout.Column CondSourceEnhancedColumn
    {
      get => new TableLayout.Column("CONDSOURCEENHANCED", "Source", HorizontalAlignment.Left, 100);
    }

    public static TableLayout.Column CondStatusColumn
    {
      get => new TableLayout.Column("CONDSTATUS", "Status", HorizontalAlignment.Left, 80);
    }

    public static TableLayout.Column CondStatusDateColumn
    {
      get
      {
        return new TableLayout.Column("CONDSTATUSDATE", "Latest Status Date", HorizontalAlignment.Left, 120);
      }
    }

    public static TableLayout.Column CondLatestStatusColumn
    {
      get
      {
        return new TableLayout.Column("CONDLATESTSTATUS", "Latest Status", HorizontalAlignment.Left, 80);
      }
    }

    public static TableLayout.Column CondStatusDateTimeColumn
    {
      get
      {
        return new TableLayout.Column("CONDSTATUSDATETIME", "Latest Status Date/Time", HorizontalAlignment.Left, 130);
      }
    }

    public static TableLayout.Column CondStatusUserColumn
    {
      get
      {
        return new TableLayout.Column("CONDSTATUSUSER", "Latest Status User", HorizontalAlignment.Left, 110);
      }
    }

    public static TableLayout.Column ConversionTypeColumn
    {
      get
      {
        return new TableLayout.Column("CONVERSIONTYPE", "Conversion Preference", HorizontalAlignment.Left, 125);
      }
    }

    public static TableLayout.Column CurrentRowColumn
    {
      get => new TableLayout.Column("CURRENTROW", "", HorizontalAlignment.Center, 24);
    }

    public static TableLayout.Column DateColumn
    {
      get => new TableLayout.Column("DATE", "Date", HorizontalAlignment.Left, 80);
    }

    public static TableLayout.Column DateTimeColumn
    {
      get => new TableLayout.Column("DATETIME", "Date", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column DaysTillDueColumn
    {
      get
      {
        return new TableLayout.Column("DAYSTILLDUE", "Days to Receive", HorizontalAlignment.Right, 95);
      }
    }

    public static TableLayout.Column DaysTillExpireColumn
    {
      get
      {
        return new TableLayout.Column("DAYSTILLEXPIRE", "Days to Expire", HorizontalAlignment.Right, 95);
      }
    }

    public static TableLayout.Column DescriptionColumn
    {
      get => new TableLayout.Column("DESCRIPTION", "Description", HorizontalAlignment.Left, 225);
    }

    public static TableLayout.Column DispositionColumn
    {
      get => new TableLayout.Column("DISPOSITION", "Disposition", HorizontalAlignment.Center, 65);
    }

    public static TableLayout.Column DocumentCountColumn
    {
      get => new TableLayout.Column("DOCUMENTCOUNT", "Documents", HorizontalAlignment.Center, 40);
    }

    public static TableLayout.Column DocumentReceiptDateColumn
    {
      get
      {
        return new TableLayout.Column("DOCUMENTRECEIPTDATE", "Document Receipt Date", HorizontalAlignment.Left, 80);
      }
    }

    public static TableLayout.Column DocAccessColumn
    {
      get => new TableLayout.Column("DOCACCESS", "Access", HorizontalAlignment.Left, 110);
    }

    public static TableLayout.Column DocSourceColumn
    {
      get => new TableLayout.Column("DOCSOURCE", "Source", HorizontalAlignment.Left, 170);
    }

    public static TableLayout.Column DocStatusColumn
    {
      get => new TableLayout.Column("DOCSTATUS", "Status", HorizontalAlignment.Left, 75);
    }

    public static TableLayout.Column DocTypeColumn
    {
      get => new TableLayout.Column("DOCTYPE", "Type", HorizontalAlignment.Left, 110);
    }

    public static TableLayout.Column EffectiveEndDateColumn
    {
      get
      {
        return new TableLayout.Column("EFFECTIVEENDDATE", "Effective End Date", HorizontalAlignment.Left, 80);
      }
    }

    public static TableLayout.Column EffectiveStartDateColumn
    {
      get
      {
        return new TableLayout.Column("EFFECTIVESTARTDATE", "Effective Start Date", HorizontalAlignment.Left, 80);
      }
    }

    public static TableLayout.Column EmailMessageColumn
    {
      get => new TableLayout.Column("EMAILMESSAGE", "Email Message", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column EmailRequiredColumn
    {
      get
      {
        return new TableLayout.Column("EMAILREQUIRED", "Send Notification Email", HorizontalAlignment.Left, 50);
      }
    }

    public static TableLayout.Column EventColumn
    {
      get => new TableLayout.Column("EVENT", "Event", HorizontalAlignment.Left, 275);
    }

    public static TableLayout.Column ExpectedDateColumn
    {
      get => new TableLayout.Column("EXPECTEDDATE", "Expected Date", HorizontalAlignment.Left, 80);
    }

    public static TableLayout.Column ExpirationDateColumn
    {
      get
      {
        return new TableLayout.Column("EXPIRATIONDATE", "Expiration Date", HorizontalAlignment.Left, 80);
      }
    }

    public static TableLayout.Column ExternalDescriptionColumn
    {
      get
      {
        return new TableLayout.Column("EXTERNALDESCRIPTION", "External Description", HorizontalAlignment.Left, 300);
      }
    }

    public static TableLayout.Column ConditionTypeColumn
    {
      get
      {
        return new TableLayout.Column("CONDITIONTYPE", "Condition Type", HorizontalAlignment.Left, 300);
      }
    }

    public static TableLayout.Column ExternalIdColumn
    {
      get => new TableLayout.Column("EXTERNALID", "External Id", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column FulfilledByColumn
    {
      get => new TableLayout.Column("FULFILLEDBY", "Fulfilled By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column FulfilledDateColumn
    {
      get => new TableLayout.Column("FULFILLEDDATE", "Fulfilled On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column HasAttachmentsColumn
    {
      get
      {
        return new TableLayout.Column("HASATTACHMENTS", "Attachments", HorizontalAlignment.Center, 26);
      }
    }

    public static TableLayout.Column HasConditionsColumn
    {
      get
      {
        return new TableLayout.Column("HASCONDITIONS", "For Underwriting Condition", HorizontalAlignment.Center, 26);
      }
    }

    public static TableLayout.Column HasDocumentsColumn
    {
      get => new TableLayout.Column("HASDOCUMENTS", "Documents", HorizontalAlignment.Center, 26);
    }

    public static TableLayout.Column IntendedForColumn
    {
      get => new TableLayout.Column("INTENDEDFOR", "Intended For", HorizontalAlignment.Left, 100);
    }

    public static TableLayout.Column InternalDescriptionColumn
    {
      get
      {
        return new TableLayout.Column("INTERNALDESCRIPTION", "Internal Description", HorizontalAlignment.Left, 300);
      }
    }

    public static TableLayout.Column InternalIdColumn
    {
      get => new TableLayout.Column("InternalId", "Internal Id", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column LastAttachmentColumn
    {
      get
      {
        return new TableLayout.Column("LASTATTACHMENT", "Last Attachment", HorizontalAlignment.Left, 105);
      }
    }

    public static TableLayout.Column LastUpdatedColumn
    {
      get => new TableLayout.Column("LASTUPDATED", "Updated", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column MaintainOriginalColumn
    {
      get
      {
        return new TableLayout.Column("MAINTAINORIGINAL", "Maintain Original", HorizontalAlignment.Left, 50);
      }
    }

    public static TableLayout.Column MilestoneColumn
    {
      get => new TableLayout.Column("MILESTONE", "For Milestone", HorizontalAlignment.Left, 125);
    }

    public static TableLayout.Column NameColumn
    {
      get => new TableLayout.Column("NAME", "Name", HorizontalAlignment.Left, 225);
    }

    public static TableLayout.Column NameWithIconColumn
    {
      get => new TableLayout.Column("NAMEWITHICON", "Name", HorizontalAlignment.Left, 225);
    }

    public static TableLayout.Column ObjectTypeColumn
    {
      get => new TableLayout.Column("OBJECTTYPE", "Type", HorizontalAlignment.Left, 47);
    }

    public static TableLayout.Column OpeningDocumentColumn
    {
      get => new TableLayout.Column("OPENINGDOCUMENT", "eDisclosure", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column OwnerColumn
    {
      get => new TableLayout.Column("OWNER", "Owner", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column OwnerNameColumn
    {
      get => new TableLayout.Column("OWNERNAME", "Owner", HorizontalAlignment.Left, 110);
    }

    public static TableLayout.Column PreClosingDocumentColumn
    {
      get
      {
        return new TableLayout.Column("PRECLOSINGDOCUMENT", "Pre-Closing", HorizontalAlignment.Left, 65);
      }
    }

    public static TableLayout.Column PrintExternallyColumn
    {
      get
      {
        return Session.IsBankerEdition() ? new TableLayout.Column("PRINTEXTERNALLY", "External", HorizontalAlignment.Left, 55) : new TableLayout.Column("PRINTEXTERNALLY", "MLC", HorizontalAlignment.Left, 50);
      }
    }

    public static TableLayout.Column GetPrintExternallyColumn(Sessions.Session session)
    {
      return session.IsBankerEdition() ? new TableLayout.Column("PRINTEXTERNALLY", "External", HorizontalAlignment.Left, 55) : new TableLayout.Column("PRINTEXTERNALLY", "MLC", HorizontalAlignment.Left, 50);
    }

    public static TableLayout.Column PrintInternallyColumn
    {
      get => new TableLayout.Column("PRINTINTERNALLY", "Internal", HorizontalAlignment.Left, 50);
    }

    public static TableLayout.Column PrintInternalEnhancedColumn
    {
      get
      {
        return new TableLayout.Column("PRINTINTERNALENHANCED", "Internal", HorizontalAlignment.Left, 50);
      }
    }

    public static TableLayout.Column PriorToColumn
    {
      get => new TableLayout.Column("PRIORTO", "Prior To", HorizontalAlignment.Left, 80);
    }

    public static TableLayout.Column PriorToEnhancedColumn
    {
      get => new TableLayout.Column("PRIORTOENHANCED", "Prior To", HorizontalAlignment.Left, 75);
    }

    public static TableLayout.Column ReceivedByColumn
    {
      get => new TableLayout.Column("RECEIVEDBY", "Received By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column ReceivedDateColumn
    {
      get => new TableLayout.Column("RECEIVEDDATE", "Received On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column RecipientColumn
    {
      get => new TableLayout.Column("RECIPIENT", "Recipient", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column RecipientEnhancedColumn
    {
      get => new TableLayout.Column("RECIPIENTENHANCED", "Recipient", HorizontalAlignment.Left, 65);
    }

    public static TableLayout.Column RejectedByColumn
    {
      get => new TableLayout.Column("REJECTEDBY", "Rejected By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column RejectedDateColumn
    {
      get => new TableLayout.Column("REJECTEDDATE", "Rejected On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column RequestedByColumn
    {
      get => new TableLayout.Column("REQUESTEDBY", "Requested By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column RequestedDateColumn
    {
      get => new TableLayout.Column("REQUESTEDDATE", "Requested On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column RequestedFromColumn
    {
      get
      {
        return new TableLayout.Column("REQUESTEDFROM", "Requested From", HorizontalAlignment.Left, 160);
      }
    }

    public static TableLayout.Column RequiredColumn
    {
      get => new TableLayout.Column("REQUIRED", "Required", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column RerequestedByColumn
    {
      get
      {
        return new TableLayout.Column("REREQUESTEDBY", "Re-requested By", HorizontalAlignment.Left, 85);
      }
    }

    public static TableLayout.Column RerequestedDateColumn
    {
      get
      {
        return new TableLayout.Column("REREQUESTEDDATE", "Re-requested On", HorizontalAlignment.Left, 105);
      }
    }

    public static TableLayout.Column ReviewedByColumn
    {
      get => new TableLayout.Column("REVIEWEDBY", "Reviewed By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column ReviewedDateColumn
    {
      get => new TableLayout.Column("REVIEWEDDATE", "Reviewed On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column SenderColumn
    {
      get => new TableLayout.Column("SENDER", "Sender", HorizontalAlignment.Left, 160);
    }

    public static TableLayout.Column SentByColumn
    {
      get => new TableLayout.Column("SENTBY", "Sent By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column SentDateColumn
    {
      get => new TableLayout.Column("SENTDATE", "Sent On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column ShippingReadyByColumn
    {
      get
      {
        return new TableLayout.Column("SHIPPINGREADYBY", "Ready to Ship By", HorizontalAlignment.Left, 85);
      }
    }

    public static TableLayout.Column ShippingReadyDateColumn
    {
      get
      {
        return new TableLayout.Column("SHIPPINGREADYDATE", "Ready to Ship On", HorizontalAlignment.Left, 105);
      }
    }

    public static TableLayout.Column SignatureTypeColumn
    {
      get => new TableLayout.Column("SIGNATURETYPE", "Sign Type", HorizontalAlignment.Left, 100);
    }

    public static TableLayout.Column SourceOfConditionColumn
    {
      get
      {
        return new TableLayout.Column("SOURCEOFCONDITION", "Source of Condition", HorizontalAlignment.Left, 110);
      }
    }

    public static TableLayout.Column StatusTriggerColumn
    {
      get
      {
        return new TableLayout.Column("STATUSTRIGGER", "Status Trigger", HorizontalAlignment.Left, 105);
      }
    }

    public static TableLayout.Column UnderwriterAccessColumn
    {
      get => new TableLayout.Column("UNDERWRITERACCESS", "UW Access", HorizontalAlignment.Left, 50);
    }

    public static TableLayout.Column UnderwritingReadyByColumn
    {
      get
      {
        return new TableLayout.Column("UNDERWRITINGREADYBY", "Ready for UW By", HorizontalAlignment.Left, 85);
      }
    }

    public static TableLayout.Column UnderwritingReadyDateColumn
    {
      get
      {
        return new TableLayout.Column("UNDERWRITINGREADYDATE", "Ready for UW On", HorizontalAlignment.Left, 105);
      }
    }

    public static TableLayout.Column UpdateMethodColumn
    {
      get => new TableLayout.Column("UPDATEMETHOD", "Update Method", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column UserColumn
    {
      get => new TableLayout.Column("USER", "User", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column WaivedByColumn
    {
      get => new TableLayout.Column("WAIVEDBY", "Waived By", HorizontalAlignment.Left, 85);
    }

    public static TableLayout.Column WaivedDateColumn
    {
      get => new TableLayout.Column("WAIVEDDATE", "Waived On", HorizontalAlignment.Left, 105);
    }

    public static TableLayout.Column EmailTemplateTypeColumn
    {
      get => new TableLayout.Column("EMAILTEMPLATETYPE", "Type", HorizontalAlignment.Left, 160);
    }

    public static TableLayout.Column FileSizeColumn
    {
      get => new TableLayout.Column("FILESIZE", "Size", HorizontalAlignment.Right, 65);
    }

    public static TableLayout.Column TotalFileSizeColumn
    {
      get => new TableLayout.Column("TOTALFILESIZE", "Size Total", HorizontalAlignment.Right, 65);
    }

    public static TableLayout.Column LoanNumberColumn
    {
      get => new TableLayout.Column("LOANNUMBER", "Loan #", HorizontalAlignment.Left, 160);
    }

    public static TableLayout.Column ProgressStatusColumn
    {
      get
      {
        return new TableLayout.Column("PROGRESSSTATUS", "Progress Status", HorizontalAlignment.Left, 160);
      }
    }

    public static TableLayout.Column MarkedAsFinalColumn
    {
      get
      {
        return new TableLayout.Column("MARKEDASFINAL", "Marked as Final", HorizontalAlignment.Left, 150);
      }
    }

    public static TableLayout.Column ConsumerConnectColumn
    {
      get
      {
        return new TableLayout.Column(" CONSUMERCONNECT", "Consumer Connect", HorizontalAlignment.Left, 150);
      }
    }

    public static TableLayout.Column WebCenterColumn
    {
      get
      {
        return new TableLayout.Column(" WEBCENTER", "Non-Consumer Connect", HorizontalAlignment.Left, 150);
      }
    }

    public static TableLayout.Column SourceIdColumn
    {
      get
      {
        return new TableLayout.Column(" SOURCEID", "Non-Consumer Connect", HorizontalAlignment.Left, 150);
      }
    }

    public static TableLayout.Column XREFIdColumn
    {
      get
      {
        return new TableLayout.Column(" XREFID", "Non-Consumer Connect", HorizontalAlignment.Left, 150);
      }
    }

    public static TableLayout.Column ConditionDirectionColumn
    {
      get
      {
        return new TableLayout.Column(" CONDITIONDIRECTION", "Non-Consumer Connect", HorizontalAlignment.Left, 150);
      }
    }

    public static TableLayout.Column ConditionCodeColumn
    {
      get
      {
        return new TableLayout.Column("CONDITIONCODE", "Condition Code", HorizontalAlignment.Left, 100);
      }
    }

    public static TableLayout.Column AIQTagsColumn
    {
      get => new TableLayout.Column("TAGS", "Tags", HorizontalAlignment.Left, 150);
    }

    public static TableLayout.Column AIQAdditionalInfoColumn
    {
      get
      {
        return new TableLayout.Column("ADDITIONALINFO", "Additional Info", HorizontalAlignment.Left, 200);
      }
    }

    public static TableLayout.Column AIQStatusColumn
    {
      get => new TableLayout.Column("STATUS", "Status", HorizontalAlignment.Left, 150);
    }

    public static TableLayout.Column AIQLastUpdatedColumn
    {
      get => new TableLayout.Column("LASTUPDATED", "Last Updated", HorizontalAlignment.Left, 130);
    }

    public static TableLayout.Column AIQIDColumn
    {
      get => new TableLayout.Column("ID", "ID", HorizontalAlignment.Left, 100);
    }

    public static TableLayout.Column PartnerColumn
    {
      get => new TableLayout.Column("PARTNER", "Service", HorizontalAlignment.Left, 160);
    }

    public void CreateLayout(TableLayout.Column[] columnList)
    {
      TableLayout layout = new TableLayout(columnList);
      if (this.session.IsBrokerEdition())
      {
        layout.Remove("DOCACCESS");
        layout.Remove("PRINTINTERNALLY");
        layout.Remove("UNDERWRITERACCESS");
      }
      GridViewLayoutManager.ApplyLayoutToGridView(this.gridView, layout);
      this.createColumnSorts();
      if (!this.gridView.HeaderVisible)
        this.gridView.Columns[0].SpringToFit = true;
      if (!this.gridView.FilterVisible)
        return;
      this.createFilters();
    }

    public void CreateLayout(TableLayout.Column[] defaultList, TableLayout.Column[] fullList)
    {
      TableLayout defaultColumnLayout = new TableLayout(defaultList);
      TableLayout fullColumnLayout = new TableLayout(fullList);
      if (this.session.IsBrokerEdition())
      {
        defaultColumnLayout.Remove("DOCACCESS");
        defaultColumnLayout.Remove("PRINTINTERNALLY");
        defaultColumnLayout.Remove("UNDERWRITERACCESS");
        fullColumnLayout.Remove("DOCACCESS");
        fullColumnLayout.Remove("PRINTINTERNALLY");
        fullColumnLayout.Remove("UNDERWRITERACCESS");
      }
      this.layoutMgr = new GridViewLayoutManager(this.gridView, fullColumnLayout, defaultColumnLayout);
      this.layoutMgr.LayoutChanged += new EventHandler(this.layoutMgr_LayoutChanged);
      this.createColumnSorts();
      if (!this.gridView.FilterVisible)
        return;
      this.createFilters();
    }

    public void CreateLayout(TableLayout.Column[] columnList, bool customizable)
    {
      if (!customizable)
      {
        this.CreateLayout(columnList);
      }
      else
      {
        TableLayout fullColumnLayout = new TableLayout(columnList);
        if (this.session.IsBrokerEdition())
        {
          fullColumnLayout.Remove("DOCACCESS");
          fullColumnLayout.Remove("PRINTINTERNALLY");
          fullColumnLayout.Remove("UNDERWRITERACCESS");
        }
        this.layoutMgr = new GridViewLayoutManager(this.gridView, fullColumnLayout);
        this.layoutMgr.LayoutChanged += new EventHandler(this.layoutMgr_LayoutChanged);
      }
    }

    private void createColumnSorts()
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ACCESSEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "ACTIVE":
            column.CheckBoxes = true;
            column.SortMethod = GVSortMethod.Checkbox;
            continue;
          case "ADDEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "ARCHIVEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "CHECKBOX":
            column.CheckBoxes = true;
            column.ColumnHeaderCheckbox = true;
            column.SortMethod = GVSortMethod.Checkbox;
            continue;
          case "CLEAREDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "DATE":
            column.SortMethod = GVSortMethod.Date;
            continue;
          case "DATETIME":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "DAYSTILLDUE":
            column.SortMethod = GVSortMethod.Numeric;
            continue;
          case "DAYSTILLEXPIRE":
            column.SortMethod = GVSortMethod.Numeric;
            continue;
          case "EXPECTEDDATE":
            column.SortMethod = GVSortMethod.Date;
            continue;
          case "EXPIRATIONDATE":
            column.SortMethod = GVSortMethod.Date;
            continue;
          case "FULFILLEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "LASTATTACHMENT":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "LASTUPDATED":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "OBJECTTYPE":
            column.SortMethod = GVSortMethod.Custom;
            continue;
          case "RECEIVEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "REJECTEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "REQUESTEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "REREQUESTEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "REVIEWEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "SENTDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          case "WAIVEDDATE":
            column.SortMethod = GVSortMethod.DateTime;
            continue;
          default:
            continue;
        }
      }
    }

    private void layoutMgr_LayoutChanged(object sender, EventArgs e)
    {
      this.createColumnSorts();
      if (this.filterMgr != null)
        this.createFilters();
      if (this.LayoutChanged == null)
        return;
      this.LayoutChanged((object) this, e);
    }

    private void createFilters()
    {
      if (this.filterMgr == null)
      {
        this.filterMgr = new GridViewFilterManager(this.session, this.gridView, false);
        this.filterMgr.FilterChanged += new EventHandler(this.filterMgr_FilterChanged);
      }
      foreach (GVColumn column in this.gridView.Columns)
      {
        TableLayout.Column tag = (TableLayout.Column) column.Tag;
        if (column.FilterControl == null)
        {
          GridViewFilterControlType controlType = GridViewFilterControlType.Text;
          switch (tag.ColumnID)
          {
            case "ACCESSEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "ADDEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "ARCHIVEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "AVAILABLEEXTERNALLY":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "BORROWER":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CATEGORY":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CLEAREDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "COMMENTCOUNT":
              controlType = GridViewFilterControlType.Integer;
              break;
            case "CONDITIONTYPE":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CONDLATESTSTATUS":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CONDSOURCEENHANCED":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CONDSTATUS":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CONDSTATUSDATE":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "CONDSTATUSDATETIME":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "DATE":
              controlType = GridViewFilterControlType.Date;
              break;
            case "DATETIME":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "DAYSTILLDUE":
              controlType = GridViewFilterControlType.Integer;
              break;
            case "DAYSTILLEXPIRE":
              controlType = GridViewFilterControlType.Integer;
              break;
            case "DISPOSITION":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "DOCSTATUS":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "DOCTYPE":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "DOCUMENTCOUNT":
              controlType = GridViewFilterControlType.Integer;
              break;
            case "DOCUMENTRECEIPTDATE":
              controlType = GridViewFilterControlType.Date;
              break;
            case "EFFECTIVEENDDATE":
              controlType = GridViewFilterControlType.Date;
              break;
            case "EFFECTIVESTARTDATE":
              controlType = GridViewFilterControlType.Date;
              break;
            case "EXPECTEDDATE":
              controlType = GridViewFilterControlType.Date;
              break;
            case "EXPIRATIONDATE":
              controlType = GridViewFilterControlType.Date;
              break;
            case "EXTERNALID":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "FULFILLEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "InternalId":
              controlType = !(this.gridView.Name == "gvConditionTemplates") ? GridViewFilterControlType.DropdownList : GridViewFilterControlType.Dropdown;
              break;
            case "LASTATTACHMENT":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "LASTUPDATED":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "MARKEDASFINAL":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "MILESTONE":
              controlType = GridViewFilterControlType.Milestone;
              break;
            case "OBJECTTYPE":
              controlType = GridViewFilterControlType.ObjectType;
              break;
            case "OWNERNAME":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "PARTNER":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "PRINTINTERNALENHANCED":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "PRIORTO":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "PRIORTOENHANCED":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "RECEIVEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "RECIPIENT":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "RECIPIENTENHANCED":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "REJECTEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "REQUESTEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "REREQUESTEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "REVIEWEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "SENTDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
            case "SOURCEOFCONDITION":
              controlType = GridViewFilterControlType.DropdownList;
              break;
            case "WAIVEDDATE":
              controlType = GridViewFilterControlType.DateTime;
              break;
          }
          Control columnFilter = this.filterMgr.CreateColumnFilter(column.Index, controlType);
          if (tag.ColumnID == "AVAILABLEEXTERNALLY")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Yes", "Yes"));
            comboBox.Items.Add((object) new FieldOption("No", "No"));
          }
          else if (tag.ColumnID == "BORROWER")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
              comboBox.Items.Add((object) new FieldOption(borrowerPair.ToString(), borrowerPair.ToString()));
            comboBox.Items.Add((object) new FieldOption(BorrowerPair.All.ToString(), BorrowerPair.All.ToString()));
          }
          else if (tag.ColumnID == "CATEGORY")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Assets", "Assets"));
            comboBox.Items.Add((object) new FieldOption("Credit", "Credit"));
            comboBox.Items.Add((object) new FieldOption("Income", "Income"));
            comboBox.Items.Add((object) new FieldOption("Legal", "Legal"));
            comboBox.Items.Add((object) new FieldOption("Misc", "Misc"));
            comboBox.Items.Add((object) new FieldOption("Property", "Property"));
          }
          else if (tag.ColumnID == "CONDSTATUS")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Added", "Added"));
            comboBox.Items.Add((object) new FieldOption("Cleared", "Cleared"));
            comboBox.Items.Add((object) new FieldOption("Expected", "Expected"));
            comboBox.Items.Add((object) new FieldOption("Expired", "Expired"));
            comboBox.Items.Add((object) new FieldOption("Fulfilled", "Fulfilled"));
            comboBox.Items.Add((object) new FieldOption("Past Due", "Past Due"));
            comboBox.Items.Add((object) new FieldOption("Received", "Received"));
            comboBox.Items.Add((object) new FieldOption("Requested", "Requested"));
            comboBox.Items.Add((object) new FieldOption("Re-requested", "Re-requested"));
            comboBox.Items.Add((object) new FieldOption("Rejected", "Rejected"));
            comboBox.Items.Add((object) new FieldOption("Reviewed", "Reviewed"));
            comboBox.Items.Add((object) new FieldOption("Sent", "Sent"));
            comboBox.Items.Add((object) new FieldOption("Waived", "Waived"));
          }
          else if (tag.ColumnID == "DOCSTATUS")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Expected", "Expected"));
            comboBox.Items.Add((object) new FieldOption("Expired", "Expired"));
            comboBox.Items.Add((object) new FieldOption("Past Due", "Past Due"));
            comboBox.Items.Add((object) new FieldOption("Ready for UW", "Ready for UW"));
            comboBox.Items.Add((object) new FieldOption("Ready to Ship", "Ready to Ship"));
            comboBox.Items.Add((object) new FieldOption("Received", "Received"));
            comboBox.Items.Add((object) new FieldOption("Requested", "Requested"));
            comboBox.Items.Add((object) new FieldOption("Re-requested", "Re-requested"));
            comboBox.Items.Add((object) new FieldOption("Reviewed", "Reviewed"));
          }
          else if (tag.ColumnID == "DOCTYPE")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Settlement Service", "Settlement Service"));
            comboBox.Items.Add((object) new FieldOption("Standard Form", "Standard Form"));
            comboBox.Items.Add((object) new FieldOption("Custom Form", "Custom Form"));
            comboBox.Items.Add((object) new FieldOption("eDisclosure", "eDisclosure"));
            comboBox.Items.Add((object) new FieldOption("Closing Document", "Closing Document"));
            comboBox.Items.Add((object) new FieldOption("PreClosing Document", "PreClosing Document"));
            comboBox.Items.Add((object) new FieldOption("Needed", "Needed"));
            comboBox.Items.Add((object) new FieldOption("Verification", "Verification"));
          }
          else if (tag.ColumnID == "MILESTONE")
          {
            MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones);
            MilestoneLog[] allMilestones = this.loanDataMgr.LoanData.GetLogList().GetAllMilestones();
            List<EllieMae.EMLite.Workflow.Milestone> allActiveMilestones = new List<EllieMae.EMLite.Workflow.Milestone>();
            foreach (MilestoneLog milestoneLog in allMilestones)
            {
              EllieMae.EMLite.Workflow.Milestone milestoneById = bpmManager.GetMilestoneByID(milestoneLog.MilestoneID, milestoneLog.Stage, false, milestoneLog.Days, milestoneLog.DoneText, milestoneLog.ExpText, milestoneLog.RoleRequired == "Y", milestoneLog.RoleID);
              if (milestoneById != null)
                allActiveMilestones.Add(milestoneById);
            }
            ((MilestoneDropdownBox) columnFilter).PopulateAllMilestones((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) allActiveMilestones, true, true);
          }
          else if (tag.ColumnID == "PRINTINTERNALENHANCED")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Yes", "Yes"));
            comboBox.Items.Add((object) new FieldOption("No", "No"));
          }
          else if (tag.ColumnID == "PRIORTO")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Approval", "Approval"));
            comboBox.Items.Add((object) new FieldOption("Docs", "Docs"));
            comboBox.Items.Add((object) new FieldOption("Funding", "Funding"));
            comboBox.Items.Add((object) new FieldOption("Closing", "Closing"));
            comboBox.Items.Add((object) new FieldOption("Purchase", "Purchase"));
          }
          else if (tag.ColumnID == "RECIPIENT")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Investor", "Investor"));
            comboBox.Items.Add((object) new FieldOption("MERS", "MERS"));
          }
          else if (tag.ColumnID == "MARKEDASFINAL")
          {
            ComboBox comboBox = (ComboBox) columnFilter;
            comboBox.Items.Add((object) string.Empty);
            comboBox.Items.Add((object) new FieldOption("Yes", "Yes"));
          }
        }
      }
    }

    public void ApplyFilter()
    {
      this.filterMgr.ApplyFilter();
      if (this.FilterChanged == null)
        return;
      this.FilterChanged((object) this, EventArgs.Empty);
    }

    private void filterMgr_FilterChanged(object sender, EventArgs e) => this.ApplyFilter();

    public bool ApplyEmptyFilters
    {
      get => this.filterMgr.ApplyEmptyFilters;
      set => this.filterMgr.ApplyEmptyFilters = value;
    }

    public GVItem AddItem(CommentEntry entry, string parentName)
    {
      GVItem gvItem = this.CreateItem(entry, parentName);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(ConditionLog cond, DocumentLog[] docList)
    {
      GVItem gvItem = this.CreateItem(cond, docList);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(ConditionTemplate template)
    {
      GVItem gvItem = this.CreateItem(template);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(EnhancedConditionTemplate template, bool selected = false)
    {
      GVItem gvItem = this.CreateItem(template);
      gvItem.Selected = selected;
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(EllieMae.EMLite.Workflow.Milestone milestone)
    {
      GVItem gvItem = this.CreateItem(milestone);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(DocumentGroup group)
    {
      GVItem gvItem = this.CreateItem(group);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(DocumentLog doc)
    {
      GVItem gvItem = this.CreateItem(doc);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(DocumentLog doc, bool isRequired)
    {
      GVItem gvItem = this.CreateItem(doc, isRequired);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(DocumentTemplate template)
    {
      GVItem gvItem = this.CreateItem(template);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(DownloadLog download)
    {
      GVItem gvItem = this.CreateItem(download);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(FileAttachment file, FileAttachmentReference fileRef)
    {
      GVItem gvItem = this.CreateItem(file, fileRef);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(HtmlEmailTemplate template)
    {
      GVItem gvItem = this.CreateItem(template);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(LoanHistoryEntry entry)
    {
      GVItem gvItem = this.CreateItem(entry);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(MilestoneLog milestone)
    {
      GVItem gvItem = this.CreateItem(milestone);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(StatusOnlineTrigger trigger, HtmlEmailTemplate template)
    {
      GVItem gvItem = this.CreateItem(trigger, template);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(LoanCenterDocument document, DocumentLog documentLog)
    {
      GVItem gvItem = this.CreateItem(document, documentLog);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public GVItem AddItem(EDeliverySignedDocument document, DocumentLog documentLog)
    {
      GVItem gvItem = this.CreateItem(document, documentLog);
      this.gridView.Items.Add(gvItem);
      return gvItem;
    }

    public void AddItem(GVItem item) => this.gridView.Items.Add(item);

    public void ClearItems() => this.gridView.Items.Clear();

    public GVItem CreateItem(CommentEntry entry, string parentName)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, entry, parentName);
      gvItem.Tag = (object) entry;
      return gvItem;
    }

    public GVItem CreateItem(ConditionLog cond, DocumentLog[] docList)
    {
      switch (cond)
      {
        case PreliminaryConditionLog _:
          return this.CreateItem((PreliminaryConditionLog) cond, docList);
        case EnhancedConditionLog _:
          return this.CreateItem((EnhancedConditionLog) cond, docList);
        case UnderwritingConditionLog _:
          return this.CreateItem((UnderwritingConditionLog) cond, docList);
        case SellConditionLog _:
          return this.CreateItem((SellConditionLog) cond, docList);
        case PostClosingConditionLog _:
          return this.CreateItem((PostClosingConditionLog) cond, docList);
        default:
          return (GVItem) null;
      }
    }

    public GVItem CreateItem(ConditionTemplate template)
    {
      switch (template)
      {
        case UnderwritingConditionTemplate _:
          return this.CreateItem((UnderwritingConditionTemplate) template);
        case SellConditionTemplate _:
          return this.CreateItem((SellConditionTemplate) template);
        case PostClosingConditionTemplate _:
          return this.CreateItem((PostClosingConditionTemplate) template);
        default:
          return (GVItem) null;
      }
    }

    public GVItem CreateItem(EnhancedConditionTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, template);
      gvItem.Tag = (object) template;
      return gvItem;
    }

    public GVItem CreateItem(EllieMae.EMLite.Workflow.Milestone milestone)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, milestone);
      gvItem.Tag = (object) milestone;
      return gvItem;
    }

    public GVItem CreateItem(DocumentGroup group)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, group);
      gvItem.Tag = (object) group;
      return gvItem;
    }

    public GVItem CreateItem(DocumentLog doc)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, doc);
      gvItem.Tag = (object) doc;
      return gvItem;
    }

    public GVItem CreateItem(DocumentLog doc, bool isRequired)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, doc, isRequired);
      gvItem.Tag = (object) doc;
      return gvItem;
    }

    public GVItem CreateItem(DocumentTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, template);
      gvItem.Tag = (object) template;
      return gvItem;
    }

    public GVItem CreateItem(DownloadLog download)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, download);
      gvItem.Tag = (object) download;
      return gvItem;
    }

    public GVItem CreateItem(FileAttachment file, FileAttachmentReference fileRef)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, file, fileRef);
      gvItem.Tag = (object) file;
      return gvItem;
    }

    public GVItem CreateItem(HtmlEmailTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, template);
      gvItem.Tag = (object) template;
      return gvItem;
    }

    public GVItem CreateItem(LoanHistoryEntry entry)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, entry);
      gvItem.Tag = (object) entry;
      return gvItem;
    }

    public GVItem CreateItem(MilestoneLog milestone)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, milestone);
      gvItem.Tag = (object) milestone;
      return gvItem;
    }

    public GVItem CreateItem(PostClosingConditionLog cond, DocumentLog[] docList)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, cond, docList);
      gvItem.Tag = (object) cond;
      return gvItem;
    }

    public GVItem CreateItem(PostClosingConditionTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, template);
      gvItem.Tag = (object) template;
      return gvItem;
    }

    public GVItem CreateItem(PreliminaryConditionLog cond, DocumentLog[] docList)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, cond, docList);
      gvItem.Tag = (object) cond;
      return gvItem;
    }

    public GVItem CreateItem(EnhancedConditionLog cond, DocumentLog[] docList)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, cond, docList);
      gvItem.Tag = (object) cond;
      return gvItem;
    }

    public GVItem CreateItem(UnderwritingConditionLog cond, DocumentLog[] docList)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, cond, docList);
      gvItem.Tag = (object) cond;
      return gvItem;
    }

    public GVItem CreateItem(UnderwritingConditionTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, template);
      gvItem.Tag = (object) template;
      return gvItem;
    }

    public GVItem CreateItem(SellConditionLog cond, DocumentLog[] docList)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, cond, docList);
      gvItem.Tag = (object) cond;
      return gvItem;
    }

    public GVItem CreateItem(SellConditionTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, template);
      gvItem.Tag = (object) template;
      return gvItem;
    }

    public GVItem CreateItem(StatusOnlineTrigger trigger, HtmlEmailTemplate template)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, trigger, template);
      gvItem.Tag = (object) trigger;
      return gvItem;
    }

    public GVItem CreateItem(LoanCenterDocument document, DocumentLog documentLog)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, document, documentLog);
      gvItem.Tag = (object) document;
      return gvItem;
    }

    public GVItem CreateItem(EDeliverySignedDocument document, DocumentLog documentLog)
    {
      GVItem gvItem = new GVItem();
      this.RefreshItem(gvItem, document, documentLog);
      gvItem.Tag = (object) document;
      return gvItem;
    }

    public void RefreshItem(GVItem item, CommentEntry entry, string parentName)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "NAME":
            subItem.Value = (object) parentName;
            continue;
          case "DATE":
            subItem.Value = (object) entry.Date;
            continue;
          case "USER":
            subItem.Value = (object) entry.AddedBy;
            continue;
          case "COMMENT":
            subItem.Value = (object) entry.Comments;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, ConditionLog cond, DocumentLog[] docList)
    {
      if (cond is PreliminaryConditionLog)
        this.RefreshItem(item, (PreliminaryConditionLog) cond, docList);
      if (cond is EnhancedConditionLog)
        this.RefreshItem(item, (EnhancedConditionLog) cond, docList);
      else if (cond is UnderwritingConditionLog)
        this.RefreshItem(item, (UnderwritingConditionLog) cond, docList);
      else if (cond is PostClosingConditionLog)
      {
        this.RefreshItem(item, (PostClosingConditionLog) cond, docList);
      }
      else
      {
        if (!(cond is SellConditionLog))
          return;
        this.RefreshItem(item, (SellConditionLog) cond, docList);
      }
    }

    public void RefreshItem(GVItem item, EllieMae.EMLite.Workflow.Milestone milestone)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "NAME":
            subItem.Value = (object) milestone.Name;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, milestone);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, DocumentGroup group)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        if (((TableLayout.Column) column.Tag).ColumnID == "NAME")
          subItem.Value = (object) group.Name;
      }
    }

    public void RefreshItem(GVItem item, DocumentLog doc) => this.RefreshItem(item, doc, false);

    public void RefreshItem(GVItem item, DocumentLog doc, bool isRequired)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ACCESSEDBY":
            subItem.Value = (object) doc.AccessedBy;
            continue;
          case "ACCESSEDDATE":
            this.setDateTimeValue(subItem, doc.DateAccessed);
            continue;
          case "ARCHIVEDDATE":
            this.setDateTimeValue(subItem, doc.DateArchived);
            continue;
          case "AVAILABLEEXTERNALLY":
            subItem.Value = (object) this.getDocExternalAvailabilityValue(doc);
            continue;
          case "BORROWER":
            subItem.Value = doc == null ? (object) string.Empty : (object) this.getBorrowerValue(doc.PairId);
            continue;
          case "CLOSINGDOCUMENT":
            subItem.Value = (object) this.getBoolValue(doc.ClosingDocument);
            continue;
          case "DATE":
            subItem.Value = doc == null ? (object) string.Empty : (object) this.getDateValue(doc.Date);
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(doc.DaysDue);
            continue;
          case "DAYSTILLEXPIRE":
            subItem.Value = (object) this.getIntValue(doc.DaysTillExpire);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) doc.Description;
            continue;
          case "DOCACCESS":
            subItem.Value = (object) this.getDocAccessValue(doc);
            continue;
          case "DOCSTATUS":
            subItem.Value = (object) this.getDocStatusValue(doc);
            continue;
          case "DOCTYPE":
            subItem.Value = (object) this.getDocTypeValue(doc);
            continue;
          case "EXPECTEDDATE":
            subItem.Value = (object) this.getDateValue(doc.DateExpected);
            continue;
          case "EXPIRATIONDATE":
            subItem.Value = (object) this.getDateValue(doc.DateExpires);
            continue;
          case "FILESIZE":
            subItem.Value = (object) this.getTotalAttachmentSize(doc, false);
            continue;
          case "HASATTACHMENTS":
            subItem.Value = (object) this.getHasAttachmentsValue(doc);
            continue;
          case "HASCONDITIONS":
            subItem.Value = (object) this.getHasConditionsValue(doc);
            continue;
          case "LASTATTACHMENT":
            this.setDateTimeValue(subItem, doc.DateLastAttachment);
            continue;
          case "LASTUPDATED":
            this.setDateTimeValue(subItem, doc.DateUpdated);
            continue;
          case "MARKEDASFINAL":
            subItem.Value = (object) this.checkMarkedAsFinal(doc);
            continue;
          case "MILESTONE":
            subItem.Value = (object) this.getMilestoneValue(doc.Stage);
            continue;
          case "NAME":
            this.setNameValue(subItem, doc);
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, doc);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Document);
            continue;
          case "OPENINGDOCUMENT":
            subItem.Value = (object) this.getBoolValue(doc.OpeningDocument);
            continue;
          case "PRECLOSINGDOCUMENT":
            subItem.Value = (object) this.getBoolValue(doc.PreClosingDocument);
            continue;
          case "RECEIVEDBY":
            subItem.Value = (object) doc.ReceivedBy;
            continue;
          case "RECEIVEDDATE":
            this.setDateTimeValue(subItem, doc.DateReceived);
            continue;
          case "REQUESTEDBY":
            subItem.Value = (object) doc.RequestedBy;
            continue;
          case "REQUESTEDDATE":
            this.setDateTimeValue(subItem, doc.DateRequested);
            continue;
          case "REQUESTEDFROM":
            subItem.Value = doc == null ? (object) string.Empty : (object) doc.RequestedFrom;
            continue;
          case "REQUIRED":
            subItem.Value = (object) this.getBoolValue(isRequired);
            continue;
          case "REREQUESTEDBY":
            subItem.Value = (object) doc.RerequestedBy;
            continue;
          case "REREQUESTEDDATE":
            this.setDateTimeValue(subItem, doc.DateRerequested);
            continue;
          case "REVIEWEDBY":
            subItem.Value = (object) doc.ReviewedBy;
            continue;
          case "REVIEWEDDATE":
            this.setDateTimeValue(subItem, doc.DateReviewed);
            continue;
          case "SHIPPINGREADYBY":
            subItem.Value = (object) doc.ShippingReadyBy;
            continue;
          case "SHIPPINGREADYDATE":
            this.setDateTimeValue(subItem, doc.DateShippingReady);
            continue;
          case "TOTALFILESIZE":
            subItem.Value = (object) this.getTotalAttachmentSize(doc, true);
            continue;
          case "UNDERWRITINGREADYBY":
            subItem.Value = (object) doc.UnderwritingReadyBy;
            continue;
          case "UNDERWRITINGREADYDATE":
            this.setDateTimeValue(subItem, doc.DateUnderwritingReady);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, DocumentTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "AVAILABLEEXTERNALLY":
            subItem.Value = (object) this.getDocExternalAvailabilityValue(template);
            continue;
          case "CLOSINGDOCUMENT":
            subItem.Value = (object) this.getBoolValue(template.ClosingDocument);
            continue;
          case "CONVERSIONTYPE":
            subItem.Value = (object) this.getConversionTypeValue(template);
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(template.DaysTillDue);
            continue;
          case "DAYSTILLEXPIRE":
            subItem.Value = (object) this.getIntValue(template.DaysTillExpire);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) template.Description;
            continue;
          case "DOCSOURCE":
            subItem.Value = (object) this.getDocSourceValue(template);
            continue;
          case "DOCTYPE":
            subItem.Value = (object) this.getDocTypeValue(template);
            continue;
          case "MAINTAINORIGINAL":
            subItem.Value = (object) this.getBoolValue(template.SaveOriginalFormat);
            continue;
          case "NAME":
            subItem.Value = (object) template.Name;
            continue;
          case "OPENINGDOCUMENT":
            subItem.Value = (object) this.getBoolValue(template.OpeningDocument);
            continue;
          case "PRECLOSINGDOCUMENT":
            subItem.Value = (object) this.getBoolValue(template.PreClosingDocument);
            continue;
          case "SIGNATURETYPE":
            subItem.Value = (object) template.SignatureType;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, DownloadLog download)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "DATETIME":
            this.setDateTimeValue(subItem, download.Date.ToLocalTime());
            continue;
          case "NAME":
            subItem.Value = (object) download.Title;
            continue;
          case "SENDER":
            subItem.Value = (object) download.Sender;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, FileAttachment file, FileAttachmentReference fileRef)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ACTIVE":
            eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) fileRef.Document);
            subItem.CheckBoxVisible = folderAccessRights.CanMarkFileAsCurrent;
            if (folderAccessRights.CanMarkFileAsCurrent)
            {
              subItem.Checked = fileRef.IsActive;
              continue;
            }
            subItem.Value = (object) this.getBoolValue(fileRef.IsActive);
            continue;
          case "ADDITIONALINFO":
            if (file.AiqMetadata != null && !string.IsNullOrWhiteSpace(file.AiqMetadata.Comments))
            {
              subItem.Value = (object) file.AiqMetadata.Comments;
              continue;
            }
            continue;
          case "DATETIME":
            this.setDateTimeValue(subItem, file.Date);
            continue;
          case "FILESIZE":
            if (file.FileSize > 0L)
            {
              subItem.Value = (object) ((file.FileSize / 1000L).ToString() + " KB");
              continue;
            }
            continue;
          case "ID":
            if (file.AiqMetadata != null && !string.IsNullOrWhiteSpace(file.AiqMetadata.FriendlyId))
            {
              subItem.Value = (object) file.AiqMetadata.FriendlyId;
              continue;
            }
            continue;
          case "LASTUPDATED":
            if (file.AiqMetadata != null && file.AiqMetadata.LastUpdated.HasValue)
            {
              subItem.Value = (object) file.AiqMetadata.LastUpdated;
              continue;
            }
            continue;
          case "NAME":
            subItem.Value = (object) file.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, file);
            continue;
          case "OBJECTTYPE":
            switch (file)
            {
              case NativeAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.FileAttachment);
                continue;
              case ImageAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.ImageAttachment);
                continue;
              case BackgroundAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.BackgroundAttachment);
                continue;
              case CloudAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.CloudAttachment);
                continue;
              default:
                continue;
            }
          case "STATUS":
            if (file.AiqMetadata != null && !string.IsNullOrWhiteSpace(file.AiqMetadata.Status))
            {
              subItem.Value = (object) file.AiqMetadata.Status;
              continue;
            }
            continue;
          case "TAGS":
            if (file.AiqMetadata != null && file.AiqMetadata.Tags != null)
            {
              string str = string.Join(",", ((IEnumerable<string>) file.AiqMetadata.Tags).Select<string, string>((Func<string, string>) (x => x)));
              subItem.Value = (object) str;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, HtmlEmailTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "NAME":
            subItem.Value = (object) template.Subject;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, template);
            continue;
          case "EMAILTEMPLATETYPE":
            this.setNameWithIconValue(subItem, template.Type);
            continue;
          case " CONSUMERCONNECT":
            this.setCheckCCType(subItem, template);
            continue;
          case " WEBCENTER":
            this.setCheckType(subItem, template);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, LoanHistoryEntry entry)
    {
      if (entry.ObjectType == HistoryObjectType.FileAttachment)
      {
        FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[entry.ObjectID, false, true];
        if (fileAttachment == null)
          return;
        this.RefreshItem(item, entry, fileAttachment);
      }
      else if (entry.ObjectType == HistoryObjectType.LogRecord)
      {
        LogRecordBase recordById = this.loanDataMgr.LoanData.GetLogList().GetRecordByID(entry.ObjectID, false, true);
        switch (recordById)
        {
          case DocumentLog _:
            this.RefreshItem(item, entry, (DocumentLog) recordById);
            break;
          case ConditionLog _:
            this.RefreshItem(item, entry, (ConditionLog) recordById);
            break;
        }
      }
      else
      {
        if (entry.ObjectType != HistoryObjectType.PageImage)
          return;
        foreach (FileAttachment allFile in this.loanDataMgr.FileAttachments.GetAllFiles(false, true))
        {
          if (allFile is ImageAttachment)
          {
            foreach (PageImage page in (allFile as ImageAttachment).Pages)
            {
              if (entry.ObjectID == page.ImageKey)
              {
                this.RefreshItem(item, entry, page);
                return;
              }
            }
          }
        }
      }
    }

    public void RefreshItem(GVItem item, LoanHistoryEntry entry, FileAttachment file)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "DATETIME":
            this.setDateTimeValue(subItem, entry.Date);
            continue;
          case "OBJECTTYPE":
            switch (file)
            {
              case NativeAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.FileAttachment);
                continue;
              case ImageAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.ImageAttachment);
                continue;
              case BackgroundAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.BackgroundAttachment);
                continue;
              case CloudAttachment _:
                subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.CloudAttachment);
                continue;
              default:
                continue;
            }
          case "NAME":
            subItem.Value = (object) file.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, file);
            continue;
          case "EVENT":
            subItem.Value = this.getHistoryDetails(entry);
            continue;
          case "USER":
            subItem.Value = (object) entry.UserID;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, LoanHistoryEntry entry, PageImage page)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "DATETIME":
            this.setDateTimeValue(subItem, entry.Date);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.PageImage);
            continue;
          case "NAME":
            subItem.Value = (object) page.Attachment.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, page);
            continue;
          case "EVENT":
            subItem.Value = this.getHistoryDetails(entry);
            continue;
          case "USER":
            subItem.Value = (object) entry.UserID;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, LoanHistoryEntry entry, DocumentLog doc)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(doc.PairId);
            continue;
          case "DATETIME":
            this.setDateTimeValue(subItem, entry.Date);
            continue;
          case "EVENT":
            subItem.Value = this.getHistoryDetails(entry);
            continue;
          case "NAME":
            this.setNameValue(subItem, doc);
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, doc);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Document);
            continue;
          case "USER":
            subItem.Value = (object) entry.UserID;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, LoanHistoryEntry entry, ConditionLog cond)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(cond.PairId);
            continue;
          case "DATETIME":
            this.setDateTimeValue(subItem, entry.Date);
            continue;
          case "EVENT":
            subItem.Value = this.getHistoryDetails(entry);
            continue;
          case "NAME":
            subItem.Value = (object) cond.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, cond);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Condition);
            continue;
          case "USER":
            subItem.Value = (object) entry.UserID;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, MilestoneLog milestone)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "NAME":
            subItem.Value = (object) milestone.Stage;
            continue;
          case "NAMEWITHICON":
            subItem.Value = (object) this.getMilestoneValue(milestone.Stage);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, PostClosingConditionLog cond, DocumentLog[] docList)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ADDEDBY":
            subItem.Value = (object) cond.AddedBy;
            continue;
          case "ADDEDDATE":
            this.setDateTimeValue(subItem, cond.DateAdded);
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(cond.PairId);
            continue;
          case "CLEAREDBY":
            subItem.Value = (object) cond.ClearedBy;
            continue;
          case "CLEAREDDATE":
            this.setDateTimeValue(subItem, cond.DateCleared);
            continue;
          case "CONDSOURCE":
            subItem.Value = (object) cond.Source;
            continue;
          case "CONDSTATUS":
            subItem.Value = (object) this.getCondStatusValue(cond.Status);
            continue;
          case "DATE":
            subItem.Value = (object) this.getDateValue(cond.Date);
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(cond.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) cond.Description;
            continue;
          case "EXPECTEDDATE":
            subItem.Value = (object) this.getDateValue(cond.DateExpected);
            continue;
          case "HASDOCUMENTS":
            subItem.Value = (object) this.getHasDocumentsValue((ConditionLog) cond, docList);
            continue;
          case "NAME":
            subItem.Value = (object) cond.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, (ConditionLog) cond);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Condition);
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(cond.IsExternal);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(cond.IsInternal);
            continue;
          case "RECEIVEDBY":
            subItem.Value = (object) cond.ReceivedBy;
            continue;
          case "RECEIVEDDATE":
            this.setDateTimeValue(subItem, cond.DateReceived);
            continue;
          case "RECIPIENT":
            subItem.Value = (object) cond.Recipient;
            continue;
          case "REQUESTEDBY":
            subItem.Value = (object) cond.RequestedBy;
            continue;
          case "REQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRequested);
            continue;
          case "REQUESTEDFROM":
            subItem.Value = (object) cond.RequestedFrom;
            continue;
          case "REREQUESTEDBY":
            subItem.Value = (object) cond.RerequestedBy;
            continue;
          case "REREQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRerequested);
            continue;
          case "SENTBY":
            subItem.Value = (object) cond.SentBy;
            continue;
          case "SENTDATE":
            this.setDateTimeValue(subItem, cond.DateSent);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, PostClosingConditionTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "CONDSOURCE":
            subItem.Value = (object) template.Source;
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(template.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) template.Description;
            continue;
          case "NAME":
            subItem.Value = (object) template.Name;
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsExternal);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsInternal);
            continue;
          case "RECIPIENT":
            subItem.Value = (object) template.Recipient;
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, PreliminaryConditionLog cond, DocumentLog[] docList)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ADDEDBY":
            subItem.Value = (object) cond.AddedBy;
            continue;
          case "ADDEDDATE":
            this.setDateTimeValue(subItem, cond.DateAdded);
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(cond.PairId);
            continue;
          case "CATEGORY":
            subItem.Value = (object) cond.Category;
            continue;
          case "CONDSOURCE":
            subItem.Value = (object) cond.Source;
            continue;
          case "CONDSTATUS":
            subItem.Value = (object) this.getCondStatusValue(cond.Status);
            continue;
          case "DATE":
            subItem.Value = (object) this.getDateValue(cond.Date);
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(cond.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) cond.Description;
            continue;
          case "EXPECTEDDATE":
            subItem.Value = (object) this.getDateValue(cond.DateExpected);
            continue;
          case "FULFILLEDBY":
            subItem.Value = (object) cond.FulfilledBy;
            continue;
          case "FULFILLEDDATE":
            this.setDateTimeValue(subItem, cond.DateFulfilled);
            continue;
          case "HASDOCUMENTS":
            subItem.Value = (object) this.getHasDocumentsValue((ConditionLog) cond, docList);
            continue;
          case "NAME":
            subItem.Value = (object) cond.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, (ConditionLog) cond);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Condition);
            continue;
          case "PRIORTO":
            subItem.Value = (object) this.getPriorToValue(cond.PriorTo);
            continue;
          case "RECEIVEDBY":
            subItem.Value = (object) cond.ReceivedBy;
            continue;
          case "RECEIVEDDATE":
            this.setDateTimeValue(subItem, cond.DateReceived);
            continue;
          case "REQUESTEDBY":
            subItem.Value = (object) cond.RequestedBy;
            continue;
          case "REQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRequested);
            continue;
          case "REQUESTEDFROM":
            subItem.Value = (object) cond.RequestedFrom;
            continue;
          case "REREQUESTEDBY":
            subItem.Value = (object) cond.RerequestedBy;
            continue;
          case "REREQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRerequested);
            continue;
          case "UNDERWRITERACCESS":
            subItem.Value = (object) this.getBoolValue(cond.UnderwriterAccess);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, EnhancedConditionLog cond, DocumentLog[] docList)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        bool? nullable1;
        DateTime? nullable2;
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ADDEDBY":
            subItem.Value = (object) cond.AddedBy;
            continue;
          case "ADDEDDATE":
            this.setDateTimeValueLocal(subItem, cond.DateAdded, true);
            continue;
          case "AVAILABLEEXTERNALLY":
            GVSubItem gvSubItem1 = subItem;
            nullable1 = cond.ExternalPrint;
            string boolValue1 = this.getBoolValue(nullable1.Value);
            gvSubItem1.Value = (object) boolValue1;
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(cond.PairId);
            continue;
          case "CATEGORY":
            subItem.Value = (object) cond.Category;
            continue;
          case "COMMENTCOUNT":
            subItem.Value = (object) cond.Comments.Count;
            continue;
          case "CONDITIONTYPE":
            subItem.Value = (object) cond.ConditionType;
            continue;
          case "CONDLATESTSTATUS":
            subItem.Value = (object) cond.Status;
            continue;
          case "CONDSOURCEENHANCED":
            subItem.Value = (object) cond.Source;
            continue;
          case "CONDSTATUS":
            subItem.Value = (object) this.getCondLastStatusUpdateValue(cond);
            continue;
          case "CONDSTATUSDATE":
            subItem.Value = (object) this.getCondLastStatusUpdateValue(cond);
            continue;
          case "CONDSTATUSDATETIME":
            subItem.Value = (object) this.getCondLastStatusDateTimeValue(cond);
            continue;
          case "CONDSTATUSUSER":
            subItem.Value = (object) cond.StatusUser;
            continue;
          case "DAYSTILLDUE":
            GVSubItem gvSubItem2 = subItem;
            int? daysToReceive = cond.DaysToReceive;
            string str1;
            if (!daysToReceive.HasValue)
            {
              str1 = string.Empty;
            }
            else
            {
              daysToReceive = cond.DaysToReceive;
              str1 = this.getIntValue(daysToReceive.Value);
            }
            gvSubItem2.Value = (object) str1;
            continue;
          case "DISPOSITION":
            this.setDispositionValue(subItem, cond.StatusOpen);
            continue;
          case "DOCUMENTCOUNT":
            subItem.Value = (object) this.getDocumentCount((ConditionLog) cond, docList);
            continue;
          case "DOCUMENTRECEIPTDATE":
            GVSubItem gvSubItem3 = subItem;
            nullable2 = cond.DocumentReceiptDate;
            string str2;
            if (nullable2.HasValue)
            {
              nullable2 = cond.DocumentReceiptDate;
              str2 = this.getDateValue(nullable2.Value, true);
            }
            else
              str2 = string.Empty;
            gvSubItem3.Value = (object) str2;
            continue;
          case "EFFECTIVEENDDATE":
            GVSubItem gvSubItem4 = subItem;
            nullable2 = cond.EndDate;
            string str3;
            if (nullable2.HasValue)
            {
              nullable2 = cond.EndDate;
              str3 = this.getDateValue(nullable2.Value, true);
            }
            else
              str3 = string.Empty;
            gvSubItem4.Value = (object) str3;
            continue;
          case "EFFECTIVESTARTDATE":
            GVSubItem gvSubItem5 = subItem;
            nullable2 = cond.StartDate;
            string str4;
            if (nullable2.HasValue)
            {
              nullable2 = cond.StartDate;
              str4 = this.getDateValue(nullable2.Value, true);
            }
            else
              str4 = string.Empty;
            gvSubItem5.Value = (object) str4;
            continue;
          case "EXTERNALDESCRIPTION":
            subItem.Value = (object) cond.ExternalDescription;
            continue;
          case "EXTERNALID":
            subItem.Value = (object) cond.ExternalId;
            continue;
          case "INTERNALDESCRIPTION":
            subItem.Value = (object) cond.InternalDescription;
            continue;
          case "InternalId":
            subItem.Value = (object) cond.InternalId;
            continue;
          case "NAME":
            subItem.Value = (object) cond.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, (ConditionLog) cond);
            continue;
          case "OWNER":
            subItem.Value = (object) this.GetOwners(cond);
            continue;
          case "OWNERNAME":
            subItem.Value = (object) this.getOwnerName(Convert.ToInt32((object) cond.Owner));
            continue;
          case "PARTNER":
            subItem.Value = (object) this.getPartnerValue(cond);
            continue;
          case "PRINTINTERNALENHANCED":
            GVSubItem gvSubItem6 = subItem;
            nullable1 = cond.InternalPrint;
            string boolValue2 = this.getBoolValue(nullable1.Value);
            gvSubItem6.Value = (object) boolValue2;
            continue;
          case "PRIORTOENHANCED":
            subItem.Value = (object) cond.PriorTo;
            continue;
          case "RECIPIENTENHANCED":
            subItem.Value = (object) cond.Recipient;
            continue;
          case "REQUESTEDFROM":
            subItem.Value = (object) cond.RequestedFrom;
            continue;
          case "SOURCEOFCONDITION":
            subItem.Value = (object) this.getSourceOfConditionValue(cond);
            continue;
          default:
            continue;
        }
      }
    }

    private string GetOwners(EnhancedConditionLog ecl)
    {
      Hashtable hashtable = new Hashtable();
      foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) ecl.Definitions.TrackingDefinitions)
      {
        foreach (int allowedRole in trackingDefinition.AllowedRoles)
        {
          if (!hashtable.ContainsValue((object) allowedRole))
          {
            string ownerValue = this.getOwnerValue(allowedRole);
            if (!string.IsNullOrEmpty(ownerValue))
              hashtable.Add((object) ownerValue, (object) allowedRole);
          }
        }
      }
      return hashtable.Count > 0 ? string.Join(",", hashtable.Keys.Cast<object>().Select<object, string>((Func<object, string>) (x => x.ToString())).ToArray<string>()) : "";
    }

    public void RefreshItem(GVItem item, UnderwritingConditionLog cond, DocumentLog[] docList)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ADDEDBY":
            subItem.Value = (object) cond.AddedBy;
            continue;
          case "ADDEDDATE":
            this.setDateTimeValue(subItem, cond.DateAdded);
            continue;
          case "ALLOWTOCLEAR":
            subItem.Value = (object) this.getBoolValue(cond.AllowToClear);
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(cond.PairId);
            continue;
          case "CATEGORY":
            subItem.Value = (object) cond.Category;
            continue;
          case "CLEAREDBY":
            subItem.Value = (object) cond.ClearedBy;
            continue;
          case "CLEAREDDATE":
            this.setDateTimeValue(subItem, cond.DateCleared);
            continue;
          case "CONDSOURCE":
            subItem.Value = (object) cond.Source;
            continue;
          case "CONDSTATUS":
            subItem.Value = (object) this.getCondStatusValue(cond.Status);
            continue;
          case "DATE":
            subItem.Value = (object) this.getDateValue(cond.Date);
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(cond.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) cond.Description;
            continue;
          case "EXPECTEDDATE":
            subItem.Value = (object) this.getDateValue(cond.DateExpected);
            continue;
          case "FULFILLEDBY":
            subItem.Value = (object) cond.FulfilledBy;
            continue;
          case "FULFILLEDDATE":
            this.setDateTimeValue(subItem, cond.DateFulfilled);
            continue;
          case "HASDOCUMENTS":
            subItem.Value = (object) this.getHasDocumentsValue((ConditionLog) cond, docList);
            continue;
          case "NAME":
            subItem.Value = (object) cond.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, (ConditionLog) cond);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Condition);
            continue;
          case "OWNER":
            subItem.Value = (object) this.getOwnerValue(cond.ForRoleID);
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(cond.IsExternal);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(cond.IsInternal);
            continue;
          case "PRIORTO":
            subItem.Value = (object) this.getPriorToValue(cond.PriorTo);
            continue;
          case "RECEIVEDBY":
            subItem.Value = (object) cond.ReceivedBy;
            continue;
          case "RECEIVEDDATE":
            this.setDateTimeValue(subItem, cond.DateReceived);
            continue;
          case "REJECTEDBY":
            subItem.Value = (object) cond.RejectedBy;
            continue;
          case "REJECTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRejected);
            continue;
          case "REQUESTEDBY":
            subItem.Value = (object) cond.RequestedBy;
            continue;
          case "REQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRequested);
            continue;
          case "REQUESTEDFROM":
            subItem.Value = (object) cond.RequestedFrom;
            continue;
          case "REREQUESTEDBY":
            subItem.Value = (object) cond.RerequestedBy;
            continue;
          case "REREQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRerequested);
            continue;
          case "REVIEWEDBY":
            subItem.Value = (object) cond.ReviewedBy;
            continue;
          case "REVIEWEDDATE":
            this.setDateTimeValue(subItem, cond.DateReviewed);
            continue;
          case "WAIVEDBY":
            subItem.Value = (object) cond.WaivedBy;
            continue;
          case "WAIVEDDATE":
            this.setDateTimeValue(subItem, cond.DateWaived);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, SellConditionLog cond, DocumentLog[] docList)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ADDEDBY":
            subItem.Value = (object) cond.AddedBy;
            continue;
          case "ADDEDDATE":
            this.setDateTimeValue(subItem, cond.DateAdded);
            continue;
          case "ALLOWTOCLEAR":
            subItem.Value = (object) this.getBoolValue(cond.AllowToClear);
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(cond.PairId);
            continue;
          case "CATEGORY":
            subItem.Value = (object) cond.Category;
            continue;
          case "CLEAREDBY":
            subItem.Value = (object) cond.ClearedBy;
            continue;
          case "CLEAREDDATE":
            this.setDateTimeValue(subItem, cond.DateCleared);
            continue;
          case "CONDITIONCODE":
            subItem.Value = (object) cond.ConditionCode;
            continue;
          case "CONDSOURCE":
            subItem.Value = (object) cond.Source;
            continue;
          case "CONDSTATUS":
            subItem.Value = (object) this.getCondStatusValue(cond.Status);
            continue;
          case "DATE":
            subItem.Value = (object) this.getDateValue(cond.Date);
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(cond.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) cond.Description;
            continue;
          case "EXPECTEDDATE":
            subItem.Value = (object) this.getDateValue(cond.DateExpected);
            continue;
          case "FULFILLEDBY":
            subItem.Value = (object) cond.FulfilledBy;
            continue;
          case "FULFILLEDDATE":
            this.setDateTimeValue(subItem, cond.DateFulfilled);
            continue;
          case "HASDOCUMENTS":
            subItem.Value = (object) this.getHasDocumentsValue((ConditionLog) cond, docList);
            continue;
          case "NAME":
            subItem.Value = (object) cond.Title;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, (ConditionLog) cond);
            continue;
          case "OBJECTTYPE":
            subItem.Value = (object) new ObjectTypeLabel(ObjectTypeEnum.Condition);
            continue;
          case "OWNER":
            subItem.Value = (object) this.getOwnerValue(cond.ForRoleID);
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(cond.IsExternal);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(cond.IsInternal);
            continue;
          case "PRIORTO":
            subItem.Value = (object) this.getPriorToValue(cond.PriorTo);
            continue;
          case "RECEIVEDBY":
            subItem.Value = (object) cond.ReceivedBy;
            continue;
          case "RECEIVEDDATE":
            this.setDateTimeValue(subItem, cond.DateReceived);
            continue;
          case "REJECTEDBY":
            subItem.Value = (object) cond.RejectedBy;
            continue;
          case "REJECTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRejected);
            continue;
          case "REQUESTEDBY":
            subItem.Value = (object) cond.RequestedBy;
            continue;
          case "REQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRequested);
            continue;
          case "REQUESTEDFROM":
            subItem.Value = (object) cond.RequestedFrom;
            continue;
          case "REREQUESTEDBY":
            subItem.Value = (object) cond.RerequestedBy;
            continue;
          case "REREQUESTEDDATE":
            this.setDateTimeValue(subItem, cond.DateRerequested);
            continue;
          case "REVIEWEDBY":
            subItem.Value = (object) cond.ReviewedBy;
            continue;
          case "REVIEWEDDATE":
            this.setDateTimeValue(subItem, cond.DateReviewed);
            continue;
          case "WAIVEDBY":
            subItem.Value = (object) cond.WaivedBy;
            continue;
          case "WAIVEDDATE":
            this.setDateTimeValue(subItem, cond.DateWaived);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, UnderwritingConditionTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ALLOWTOCLEAR":
            subItem.Value = (object) this.getBoolValue(template.AllowToClear);
            continue;
          case "CATEGORY":
            subItem.Value = (object) template.Category;
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(template.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) template.Description;
            continue;
          case "NAME":
            subItem.Value = (object) template.Name;
            continue;
          case "OWNER":
            subItem.Value = (object) this.getOwnerValue(template.ForRoleID);
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsExternal);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsInternal);
            continue;
          case "PRIORTO":
            subItem.Value = (object) this.getPriorToValue(template.PriorTo);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, SellConditionTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "ALLOWTOCLEAR":
            subItem.Value = (object) this.getBoolValue(template.AllowToClear);
            continue;
          case "CATEGORY":
            subItem.Value = (object) template.Category;
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) this.getIntValue(template.DaysTillDue);
            continue;
          case "DESCRIPTION":
            subItem.Value = (object) template.Description;
            continue;
          case "NAME":
            subItem.Value = (object) template.Name;
            continue;
          case "OWNER":
            subItem.Value = (object) this.getOwnerValue(template.ForRoleID);
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsExternal);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsInternal);
            continue;
          case "PRIORTO":
            subItem.Value = (object) this.getPriorToValue(template.PriorTo);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, EnhancedConditionTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (column.Tag.ToString())
        {
          case "CATEGORY":
            subItem.Value = (object) Convert.ToString(template.Category);
            continue;
          case "COMMENTCOUNT":
            subItem.Value = (object) 0;
            continue;
          case "CONDITIONSTATUS":
            subItem.Value = (object) this.getActiveStatus(template.Active);
            continue;
          case "CONDITIONTYPE":
            subItem.Value = (object) Convert.ToString(template.ConditionType);
            continue;
          case "CUSTOMIZED":
            GVSubItem gvSubItem = subItem;
            bool? customizeTypeDefinition = template.CustomizeTypeDefinition;
            bool flag = true;
            string str = Convert.ToString(customizeTypeDefinition.GetValueOrDefault() == flag & customizeTypeDefinition.HasValue ? "Yes" : "No");
            gvSubItem.Value = (object) str;
            continue;
          case "DAYSTILLDUE":
            subItem.Value = (object) Convert.ToString((object) template.DaysToReceive);
            continue;
          case "DISPOSITION":
            this.setDispositionValue(subItem, true);
            continue;
          case "DOCUMENTCOUNT":
            subItem.Value = (object) 0;
            continue;
          case "EXTERNALDESCRIPTION":
            subItem.Value = (object) Convert.ToString(template.ExternalDescription);
            continue;
          case "ID":
            subItem.Value = (object) Convert.ToString((object) template.Id);
            continue;
          case "INTERNALDESCRIPTION":
            subItem.Value = (object) Convert.ToString(template.InternalDescription);
            continue;
          case "InternalId":
            subItem.Value = (object) Convert.ToString(template.InternalId);
            continue;
          case "LASTMODIFIEDBY":
            subItem.Value = (object) Convert.ToString(template.LastModifiedBy.entityId);
            continue;
          case "LASTMODIFIEDDATETIME":
            subItem.Value = (object) Utils.ParseUTCDateTime(Convert.ToString((object) template.LastModifiedDate)).ToString("MM/dd/yyyy hh:mm tt");
            continue;
          case "NAME":
            subItem.Value = (object) Convert.ToString(template.Title);
            continue;
          case "PRINTEXTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsExternalPrint);
            continue;
          case "PRINTINTERNALLY":
            subItem.Value = (object) this.getBoolValue(template.IsInternalPrint);
            continue;
          case "PRIORTO":
            subItem.Value = !string.IsNullOrWhiteSpace(template.PriorTo) ? (object) template.PriorTo : (object) string.Empty;
            continue;
          case "PRIORTOENHANCED":
            subItem.Value = (object) Convert.ToString(template.PriorTo);
            continue;
          case "TYPE":
            subItem.Value = (object) Convert.ToString(template.ConditionType);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, StatusOnlineTrigger trigger, HtmlEmailTemplate template)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "DATE":
            subItem.Value = (object) this.getDateValue(trigger.DateTriggered);
            continue;
          case "EMAILMESSAGE":
            subItem.Value = (object) this.getEmailMessageValue(template);
            continue;
          case "EMAILREQUIRED":
            subItem.Value = (object) this.getEmailRequired(trigger);
            continue;
          case "NAME":
            subItem.Value = (object) trigger.Name;
            continue;
          case "NAMEWITHICON":
            this.setNameWithIconValue(subItem, trigger);
            continue;
          case "STATUSTRIGGER":
            this.setStatusTriggerValue(subItem, trigger);
            continue;
          case "UPDATEMETHOD":
            subItem.Value = (object) this.getUpdateMethodValue(trigger);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, LoanCenterDocument document, DocumentLog documentLog)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "NAME":
            subItem.Value = (object) documentLog.Title;
            continue;
          case "REQUESTEDFROM":
            subItem.Value = documentLog == null ? (object) string.Empty : (object) documentLog.RequestedFrom;
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(documentLog.PairId);
            continue;
          case "DOCSTATUS":
            subItem.Value = (object) this.getDocStatusValue(documentLog);
            continue;
          case "DATETIME":
            this.setDateTimeValuePST(subItem, document.packageDetails.SignedDate);
            continue;
          default:
            continue;
        }
      }
    }

    public void RefreshItem(GVItem item, EDeliverySignedDocument document, DocumentLog documentLog)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        GVSubItem subItem = item.SubItems[column.Index];
        switch (((TableLayout.Column) column.Tag).ColumnID)
        {
          case "NAME":
            subItem.Value = (object) documentLog.Title;
            continue;
          case "REQUESTEDFROM":
            subItem.Value = documentLog == null ? (object) string.Empty : (object) documentLog.RequestedFrom;
            continue;
          case "BORROWER":
            subItem.Value = (object) this.getBorrowerValue(documentLog.PairId);
            continue;
          case "DOCSTATUS":
            subItem.Value = (object) this.getDocStatusValue(documentLog);
            continue;
          case "DATETIME":
            this.setDateTimeValuePST(subItem, document.signedDate);
            continue;
          default:
            continue;
        }
      }
    }

    private void loadRoleTable(bool includeOthers)
    {
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.roleTable = new Hashtable();
      foreach (RoleInfo roleInfo in allRoleFunctions)
        this.roleTable[(object) roleInfo.ID] = (object) roleInfo.RoleAbbr;
      if (!includeOthers)
        return;
      this.roleTable[(object) RoleInfo.Others.ID] = (object) RoleInfo.Others.RoleAbbr;
    }

    private void loadRoleNameTable(bool includeOthers)
    {
      RoleInfo[] allRoleFunctions = ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      this.roleNameTable = new Hashtable();
      foreach (RoleInfo roleInfo in allRoleFunctions)
        this.roleNameTable[(object) roleInfo.ID] = (object) roleInfo.RoleName;
      if (!includeOthers)
        return;
      this.roleNameTable[(object) RoleInfo.Others.ID] = (object) RoleInfo.Others.RoleName;
    }

    private string getBoolValue(bool val) => !val ? "No" : "Yes";

    private string getActiveStatus(bool? val)
    {
      if (!val.HasValue)
        return "Inactive";
      bool? nullable = val;
      bool flag = true;
      return !(nullable.GetValueOrDefault() == flag & nullable.HasValue) ? "Inactive" : "Active";
    }

    private string getBorrowerValue(string pairID)
    {
      if (pairID == BorrowerPair.All.Id)
        return BorrowerPair.All.ToString();
      foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
      {
        if (borrowerPair.Id == pairID)
          return borrowerPair.ToString();
      }
      return (string) null;
    }

    private string getCondLastStatusUpdateValue(EnhancedConditionLog cond)
    {
      string status = cond.Status;
      DateTime? statusDate = cond.StatusDate;
      ref DateTime? local = ref statusDate;
      string str;
      if (!local.HasValue)
      {
        str = (string) null;
      }
      else
      {
        DateTime dateTime = local.GetValueOrDefault();
        dateTime = dateTime.ToLocalTime();
        str = dateTime.ToString("MM/dd/yyyy");
      }
      return status + " on " + str;
    }

    private string getCondLastStatusDateTimeValue(EnhancedConditionLog cond)
    {
      DateTime? statusDate = cond.StatusDate;
      ref DateTime? local = ref statusDate;
      if (!local.HasValue)
        return (string) null;
      DateTime dateTime = local.GetValueOrDefault();
      dateTime = dateTime.ToLocalTime();
      return dateTime.ToString("MM/dd/yyyy hh:mm tt");
    }

    private string getCondStatusValue(ConditionStatus status)
    {
      return StandardConditionLog.GetStatusString(status);
    }

    private string getDateValue(DateTime val, bool fourDigitYear = false)
    {
      string format = fourDigitYear ? "MM/dd/yyyy" : "MM/dd/yy";
      return val.Date != DateTime.MinValue.Date ? val.ToString(format) : (string) null;
    }

    private void setDateTimeValue(GVSubItem subItem, DateTime val, bool fourDigitYear = false)
    {
      string format = fourDigitYear ? "MM/dd/yyyy hh:mm tt " : "MM/dd/yy hh:mm tt ";
      DateTime date1 = val.Date;
      DateTime date2 = DateTime.MinValue.Date;
      subItem.Value = !(date1 != date2) ? (object) null : (object) val.ToString(format);
      subItem.SortValue = (object) val;
    }

    private void setDateTimeValuePST(GVSubItem subItem, DateTime val)
    {
      DateTime date1 = val.Date;
      DateTime date2 = DateTime.MinValue.Date;
      subItem.Value = !(date1 != date2) ? (object) null : (object) val.ToString("MM/dd/yy hh:mm tt 'PST'");
      subItem.SortValue = (object) val;
    }

    private void setDateTimeValueLocal(GVSubItem subItem, DateTime val, bool fourDigitYear = false)
    {
      string format = fourDigitYear ? "MM/dd/yyyy hh:mm tt " : "MM/dd/yy hh:mm tt ";
      DateTime date1 = val.Date;
      DateTime dateTime = DateTime.MinValue;
      DateTime date2 = dateTime.Date;
      if (date1 != date2)
      {
        GVSubItem gvSubItem = subItem;
        dateTime = val.ToLocalTime();
        string str = dateTime.ToString(format);
        gvSubItem.Value = (object) str;
      }
      else
        subItem.Value = (object) null;
      subItem.SortValue = (object) val;
    }

    private string getConversionTypeValue(DocumentTemplate template)
    {
      return template.ConversionType.Equals((object) ImageConversionType.BlackAndWhite) ? "Black & White" : "Color";
    }

    private string getDocAccessValue(DocumentLog doc)
    {
      if (this.roleTable == null)
        this.loadRoleTable(true);
      ArrayList arrayList = new ArrayList();
      foreach (int allowedRole in doc.AllowedRoles)
      {
        if (this.roleTable.Contains((object) allowedRole))
          arrayList.Add(this.roleTable[(object) allowedRole]);
      }
      if (arrayList.Count > 0)
        arrayList.Sort((IComparer) new CaseInsensitiveComparer());
      else
        arrayList.Add((object) RoleInfo.All.RoleAbbr);
      return string.Join(", ", (string[]) arrayList.ToArray(typeof (string)));
    }

    private string getDocExternalAvailabilityValue(DocumentTemplate template)
    {
      string empty = string.Empty;
      if (template.IsWebcenter)
        empty += "Webcenter";
      if (template.IsTPOWebcenterPortal)
        empty += string.IsNullOrEmpty(empty) ? "TPO" : ", TPO";
      if (template.IsThirdPartyDoc)
        empty += string.IsNullOrEmpty(empty) ? "EDM Lenders" : ", EDM Lenders";
      return empty;
    }

    private string getDocExternalAvailabilityValue(DocumentLog doc)
    {
      string empty = string.Empty;
      if (doc.IsWebcenter)
        empty += "Webcenter";
      if (doc.IsTPOWebcenterPortal)
        empty += string.IsNullOrEmpty(empty) ? "TPO" : ", TPO";
      if (doc.IsThirdPartyDoc)
        empty += string.IsNullOrEmpty(empty) ? "EDM Lenders" : ", EDM Lenders";
      return empty;
    }

    private string getDocSourceValue(DocumentTemplate template)
    {
      switch (template.SourceType)
      {
        case "Standard Form":
        case "Custom Form":
          return template.Source;
        case "Borrower Specific Custom Form":
          if (string.IsNullOrEmpty(template.SourceBorrower))
            return template.SourceCoborrower;
          return string.IsNullOrEmpty(template.SourceCoborrower) ? template.SourceBorrower : template.SourceBorrower + "; " + template.SourceCoborrower;
        default:
          return (string) null;
      }
    }

    private string getDocStatusValue(DocumentLog doc)
    {
      if (doc == null)
        return (string) null;
      switch (doc.Status)
      {
        case "expected":
          return "Expected";
        case "expected!":
          return "Past Due";
        case "expired!":
          return "Expired";
        case "ordered":
          return "Requested";
        case "ready for UW":
          return "Ready for UW";
        case "ready to ship":
          return "Ready to Ship";
        case "received":
          return "Received";
        case "reordered":
          return "Re-requested";
        case "reviewed":
          return "Reviewed";
        default:
          return (string) null;
      }
    }

    private string getDocTypeValue(DocumentLog doc)
    {
      if (this.docSetup == null)
        this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      return doc.GetDocumentType(this.docSetup);
    }

    private string getDocTypeValue(DocumentTemplate template)
    {
      if (Epass.IsEpassDoc(template.Name))
        return "Settlement Service";
      return template.SourceType == "Settlement Service" ? "Needed" : template.SourceType;
    }

    private string getEmailMessageValue(HtmlEmailTemplate template)
    {
      return template != (HtmlEmailTemplate) null ? template.Subject : (string) null;
    }

    private string getEmailRequired(StatusOnlineTrigger trigger)
    {
      return !string.IsNullOrEmpty(trigger.EmailTemplate) ? "Yes" : "No";
    }

    private string getTotalAttachmentSize(DocumentLog doc, bool includeNonActive)
    {
      string totalAttachmentSize = string.Empty;
      long num = 0;
      foreach (FileAttachmentReference attachmentReference in doc.Files.ToArray())
      {
        if (attachmentReference.IsActive | includeNonActive)
        {
          FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[attachmentReference.AttachmentID];
          if (fileAttachment != null)
            num += fileAttachment.FileSize / 1000L;
        }
      }
      if (num > 0L)
        totalAttachmentSize = num.ToString() + " KB";
      return totalAttachmentSize;
    }

    private ImageElement getHasAttachmentsValue(DocumentLog doc)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (doc == null)
        return (ImageElement) null;
      foreach (FileAttachmentReference attachmentReference in doc.Files.ToArray())
      {
        if (attachmentReference.IsActive)
        {
          FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[attachmentReference.AttachmentID];
          if (fileAttachment is NativeAttachment)
            return new ImageElement((Image) Resources.paper_clip);
          if (fileAttachment is ImageAttachment)
            flag1 = true;
          if (fileAttachment is BackgroundAttachment)
            flag2 = true;
          if (fileAttachment is CloudAttachment)
            flag3 = true;
        }
      }
      if (flag1)
        return new ImageElement((Image) Resources.image);
      if (flag2)
        return new ImageElement((Image) Resources.background_image);
      return flag3 ? new ImageElement((Image) Resources.cloud) : (ImageElement) null;
    }

    private ImageElement getHasConditionsValue(DocumentLog doc)
    {
      foreach (ConditionLog condition in doc.Conditions)
      {
        if (condition.ConditionType == ConditionType.Underwriting)
          return new ImageElement((Image) Resources.condition);
      }
      return (ImageElement) null;
    }

    private ImageElement getHasDocumentsValue(ConditionLog cond, DocumentLog[] docList)
    {
      bool flag = false;
      foreach (DocumentLog doc in docList)
      {
        if (doc.Conditions.Contains(cond))
        {
          if (this.loanDataMgr.FileAttachments.ContainsAttachment(doc))
            return new ImageElement((Image) Resources.document_with_attachment);
          flag = true;
        }
      }
      return flag ? new ImageElement((Image) Resources.document) : (ImageElement) null;
    }

    private int getDocumentCount(ConditionLog cond, DocumentLog[] docList)
    {
      int documentCount = 0;
      foreach (DocumentLog doc in docList)
      {
        if (doc.Conditions.Contains(cond))
          ++documentCount;
      }
      return documentCount;
    }

    private object getHistoryDetails(LoanHistoryEntry entry)
    {
      if (entry.LinkedObjectType == LinkedObjectType.LogRecord)
      {
        LogRecordBase recordById = this.loanDataMgr.LoanData.GetLogList().GetRecordByID(entry.LinkedObjectID, true, true);
        switch (recordById)
        {
          case DocumentLog _:
            DocumentLog documentLog = (DocumentLog) recordById;
            return (object) (entry.Details + " \"" + documentLog.Title + "\"");
          case ConditionLog _:
            ConditionLog conditionLog = (ConditionLog) recordById;
            return (object) (entry.Details + " \"" + conditionLog.Title + "\"");
          case DisclosureTrackingLog _:
            return (object) new LogRecordLink(entry.Details, recordById);
          case EDMLog _:
            return (object) new LogRecordLink(entry.Details, recordById);
          case DataTracLog _:
            return (object) new LogRecordLink(entry.Details, recordById);
          case HtmlEmailLog _:
            return (object) new LogRecordLink(entry.Details, recordById);
        }
      }
      else if (entry.LinkedObjectType == LinkedObjectType.FileAttachment)
      {
        FileAttachment fileAttachment = this.loanDataMgr.FileAttachments[entry.LinkedObjectID, true, true];
        if (fileAttachment != null)
          return (object) (entry.Details + " \"" + fileAttachment.Title + "\"");
      }
      else if (entry.LinkedObjectType == LinkedObjectType.PageImage)
      {
        foreach (FileAttachment allFile in this.loanDataMgr.FileAttachments.GetAllFiles(true, true))
        {
          if (allFile is ImageAttachment)
          {
            foreach (PageImage page in (allFile as ImageAttachment).Pages)
            {
              if (entry.LinkedObjectID == page.ImageKey)
                return (object) entry.Details;
            }
          }
        }
      }
      return (object) entry.Details;
    }

    private string getIntValue(int val) => val != 0 ? val.ToString() : (string) null;

    private MilestoneLabel getMilestoneValue(string stage)
    {
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones);
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      MilestoneLog milestoneByName = logList.GetMilestoneByName(stage);
      EllieMae.EMLite.Workflow.Milestone milestone = (EllieMae.EMLite.Workflow.Milestone) null;
      if (milestoneByName != null)
        milestone = bpmManager.GetMilestoneByID(milestoneByName.MilestoneID, milestoneByName.Stage, false, milestoneByName.Days, milestoneByName.DoneText, milestoneByName.ExpText, milestoneByName.RoleRequired == "Y", milestoneByName.RoleID);
      if (milestone == null)
      {
        milestone = bpmManager.GetMilestoneByName(stage);
        if (milestone == null)
          return (MilestoneLabel) null;
        if (logList.GetMilestoneByID(milestone.MilestoneID) == null)
          return (MilestoneLabel) null;
      }
      return new MilestoneLabel(milestone);
    }

    private string getOwnerValue(int roleID)
    {
      if (this.roleTable == null)
        this.loadRoleTable(false);
      return this.roleTable.ContainsKey((object) roleID) ? (string) this.roleTable[(object) roleID] : (string) null;
    }

    private string getOwnerName(int roleID)
    {
      if (this.roleNameTable == null)
        this.loadRoleNameTable(false);
      return this.roleNameTable.ContainsKey((object) roleID) ? (string) this.roleNameTable[(object) roleID] : (string) null;
    }

    private string getPriorToValue(string priorTo)
    {
      switch (priorTo)
      {
        case "PTA":
          return "Approval";
        case "PTD":
          return "Docs";
        case "PTF":
          return "Funding";
        case "AC":
          return "Closing";
        case "PTP":
          return "Purchase";
        default:
          return (string) null;
      }
    }

    private string getUpdateMethodValue(StatusOnlineTrigger trigger)
    {
      switch (trigger.UpdateType)
      {
        case TriggerUpdateType.Manual:
          return "Manually Update";
        case TriggerUpdateType.Automatic:
          return "Automatically Update";
        default:
          return (string) null;
      }
    }

    private string getSourceOfConditionValue(EnhancedConditionLog cond)
    {
      return this.sourceOfCondStringEnum.GetStringValue(cond.SourceOfCondition.ToString());
    }

    private string getPartnerValue(EnhancedConditionLog cond) => cond.Partner;

    private void setDispositionValue(GVSubItem subItem, bool isStatusOpen)
    {
      ImageElement imageElement;
      if (isStatusOpen)
      {
        imageElement = new ImageElement((Image) Resources.three_pm);
        imageElement.Tag = (object) "Open";
      }
      else
      {
        imageElement = new ImageElement((Image) Resources.greenCheck);
        imageElement.Tag = (object) "Satisfied";
      }
      subItem.Value = (object) imageElement;
      subItem.SortValue = imageElement.Tag;
    }

    private void setNameValue(GVSubItem subItem, DocumentLog doc)
    {
      if (doc == null)
        return;
      subItem.Value = (object) doc.Title;
      subItem.SortValue = (object) (doc.Title + doc.PairId + doc.Guid);
    }

    private void setNameWithIconValue(GVSubItem subItem, ConditionLog cond)
    {
      subItem.Value = (object) new ObjectWithImage(cond.Title, (Image) Resources.condition, 4);
      subItem.SortValue = (object) cond.Title;
    }

    private void setNameWithIconValue(GVSubItem subItem, EllieMae.EMLite.Workflow.Milestone milestone)
    {
      subItem.Value = (object) new MilestoneLabel(milestone);
    }

    private void setNameWithIconValue(GVSubItem subItem, DocumentLog doc)
    {
      subItem.Value = (object) new ObjectWithImage(doc.Title, (Image) Resources.document, 4);
      subItem.SortValue = (object) (doc.Title + doc.PairId + doc.Guid);
    }

    private void setNameWithIconValue(GVSubItem subItem, FileAttachment file)
    {
      switch (file)
      {
        case NativeAttachment _:
          subItem.Value = (object) new ObjectWithImage(file.Title, (Image) Resources.paper_clip, 4);
          break;
        case ImageAttachment _:
          subItem.Value = (object) new ObjectWithImage(file.Title, (Image) Resources.image, 4);
          break;
        case BackgroundAttachment _:
          subItem.Value = (object) new ObjectWithImage(file.Title, (Image) Resources.background_image, 4);
          break;
        case CloudAttachment _:
          subItem.Value = (object) new ObjectWithImage(file.Title, (Image) Resources.cloud, 4);
          break;
      }
      subItem.SortValue = (object) file.Title;
    }

    private void setNameWithIconValue(GVSubItem subItem, PageImage page)
    {
      subItem.Value = (object) new ObjectWithImage(page.Attachment.Title, (Image) Resources.page, 4);
      subItem.SortValue = (object) page.Attachment.Title;
    }

    private void setNameWithIconValue(GVSubItem subItem, HtmlEmailTemplate template)
    {
      if (string.IsNullOrEmpty(template.OwnerID))
      {
        subItem.Value = (object) new ObjectWithImage(template.Subject, (Image) Resources.document_group_public, 4);
        subItem.SortValue = (object) ("0:" + template.Subject);
      }
      else
      {
        subItem.Value = (object) new ObjectWithImage(template.Subject, (Image) Resources.document_group_private, 4);
        subItem.SortValue = (object) ("1:" + template.Subject);
      }
    }

    private void setNameWithIconValue(GVSubItem subItem, HtmlEmailTemplateType templateType)
    {
      string text = string.Empty;
      switch (templateType)
      {
        case HtmlEmailTemplateType.StatusOnline:
          text = "Status Online";
          break;
        case HtmlEmailTemplateType.RequestDocuments:
          text = "Document Requests";
          break;
        case HtmlEmailTemplateType.SendDocuments:
          text = "Sending Files";
          break;
        case HtmlEmailTemplateType.InitialDisclosures:
          text = "eDisclosures";
          break;
        case HtmlEmailTemplateType.LoanLevelConsent:
          text = "Loan Level Consent";
          break;
        case HtmlEmailTemplateType.PreClosing:
          text = "Pre-Closing Documents";
          break;
        case HtmlEmailTemplateType.ConsumerConnectPreClosing:
          text = "Pre-Closing Documents";
          break;
        case HtmlEmailTemplateType.ConsumerConnectRequestDocuments:
          text = "Document Requests";
          break;
        case HtmlEmailTemplateType.ConsumerConnectSendDocuments:
          text = "Sending Files";
          break;
        case HtmlEmailTemplateType.ConsumerConnectInitialDisclosures:
          text = "eDisclosures";
          break;
        case HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent:
          text = "Loan Level Consent";
          break;
        case HtmlEmailTemplateType.ConsumerConnectStatusOnline:
          text = "Status Online";
          break;
      }
      subItem.Value = (object) new ObjectWithImage(text, (Image) Resources.document_group_public, 4);
    }

    private void setCheckType(GVSubItem subItem, HtmlEmailTemplate template)
    {
      string str1 = '√'.ToString();
      string str2 = string.Empty;
      switch (template.Type)
      {
        case HtmlEmailTemplateType.StatusOnline:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.RequestDocuments:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.SendDocuments:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.InitialDisclosures:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.LoanLevelConsent:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.PreClosing:
          str2 = str1;
          break;
      }
      subItem.Value = (object) str2;
    }

    private void setCheckCCType(GVSubItem subItem, HtmlEmailTemplate template)
    {
      string str1 = '√'.ToString();
      string str2 = string.Empty;
      switch (template.Type)
      {
        case HtmlEmailTemplateType.ConsumerConnectPreClosing:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.ConsumerConnectRequestDocuments:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.ConsumerConnectSendDocuments:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.ConsumerConnectInitialDisclosures:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.ConsumerConnectLoanLevelConsent:
          str2 = str1;
          break;
        case HtmlEmailTemplateType.ConsumerConnectStatusOnline:
          str2 = str1;
          break;
      }
      subItem.Value = (object) str2;
    }

    private void setNameWithIconValue(GVSubItem subItem, StatusOnlineTrigger trigger)
    {
      if (string.IsNullOrEmpty(trigger.OwnerID))
      {
        subItem.Value = (object) new ObjectWithImage(trigger.Name, (Image) Resources.document_group_public, 4);
        subItem.SortValue = (object) ("0:" + trigger.Name);
      }
      else
      {
        subItem.Value = (object) new ObjectWithImage(trigger.Name, (Image) Resources.document_group_private, 4);
        subItem.SortValue = (object) ("1:" + trigger.Name);
      }
    }

    private void setStatusTriggerValue(GVSubItem item, StatusOnlineTrigger trigger)
    {
      if (trigger.RequirementType == TriggerRequirementType.Milestone)
      {
        EllieMae.EMLite.Workflow.Milestone milestoneById = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(trigger.RequirementData);
        if (milestoneById != null)
          this.setNameWithIconValue(item, milestoneById);
        else
          item.Value = (object) null;
      }
      else if (trigger.RequirementType == TriggerRequirementType.MilestoneLog)
      {
        MilestoneLog milestoneById = this.loanDataMgr.LoanData.GetLogList().GetMilestoneByID(trigger.RequirementData);
        if (milestoneById != null)
          item.Value = (object) this.getMilestoneValue(milestoneById.Stage);
        else
          item.Value = (object) null;
      }
      else if (trigger.RequirementType == TriggerRequirementType.DocumentTemplate)
      {
        if (this.docSetup == null)
          this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
        DocumentTemplate byId = this.docSetup.GetByID(trigger.RequirementData);
        if (byId != null)
          item.Value = (object) byId.Name;
        else
          item.Value = (object) null;
      }
      else if (trigger.RequirementType == TriggerRequirementType.DocumentLog)
      {
        if (this.loanDataMgr.LoanData.GetLogList().GetRecordByID(trigger.RequirementData, false) is DocumentLog recordById)
          item.Value = (object) recordById.Title;
        else
          item.Value = (object) null;
      }
      else if (trigger.RequirementType == TriggerRequirementType.Fields)
      {
        item.Value = (object) trigger.RequirementData;
      }
      else
      {
        if (trigger.RequirementType != TriggerRequirementType.DocumentName)
          return;
        item.Value = (object) trigger.RequirementData;
      }
    }

    private void setUtcTicksValue(GVSubItem subItem, long ticks)
    {
      DateTime dateTime = new DateTime(ticks, DateTimeKind.Utc);
      if (!(dateTime.Date != DateTime.MinValue.Date))
        return;
      subItem.Value = (object) dateTime.ToLocalTime().ToString("MM/dd/yy hh:mm tt");
      subItem.SortValue = (object) ticks;
    }

    private string checkMarkedAsFinal(DocumentLog doc)
    {
      return this.loanDataMgr.LoanData.GetField("UCD.X1") == doc.Guid || this.loanDataMgr.LoanData.GetField("UCD.X3") == doc.Guid || this.loanDataMgr.LoanData.GetField("UCD.X5") == doc.Guid ? "Yes" : string.Empty;
    }

    public static string StandardViewName => "Standard View";

    public static FileSystemEntry StandardViewFileSystemEntry
    {
      get => new FileSystemEntry("\\Standard View", FileSystemEntry.Types.File, Session.UserID);
    }

    public static string DefaultStackingOrderName => "None";

    public static string AllDocumentGroupName => "(All Documents)";

    public static DocumentTrackingView StandardDocumentTrackingView
    {
      get
      {
        DocumentTrackingView documentTrackingView = new DocumentTrackingView("Standard View");
        TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
        nameColumn.SortOrder = SortOrder.Ascending;
        nameColumn.SortPriority = 0;
        List<TableLayout.Column> columnList = new List<TableLayout.Column>();
        columnList.InsertRange(0, (IEnumerable<TableLayout.Column>) new TableLayout.Column[10]
        {
          GridViewDataManager.HasAttachmentsColumn,
          GridViewDataManager.HasConditionsColumn,
          nameColumn,
          GridViewDataManager.DescriptionColumn,
          GridViewDataManager.BorrowerColumn,
          GridViewDataManager.DocTypeColumn,
          GridViewDataManager.DocAccessColumn,
          GridViewDataManager.MilestoneColumn,
          GridViewDataManager.DocStatusColumn,
          GridViewDataManager.DateColumn
        });
        TableLayout tableLayout = new TableLayout(columnList.ToArray());
        documentTrackingView.Layout = tableLayout;
        documentTrackingView.DocGroup = "(All Documents)";
        documentTrackingView.StackingOrder = "None";
        return documentTrackingView;
      }
    }

    public static ConditionTrackingView StandardPreliminaryConditionTrackingView
    {
      get
      {
        ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
        TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
        nameColumn.SortOrder = SortOrder.Ascending;
        nameColumn.SortPriority = 0;
        TableLayout tableLayout = new TableLayout(new TableLayout.Column[9]
        {
          GridViewDataManager.HasDocumentsColumn,
          nameColumn,
          GridViewDataManager.DescriptionColumn,
          GridViewDataManager.CondSourceColumn,
          GridViewDataManager.UnderwriterAccessColumn,
          GridViewDataManager.CategoryColumn,
          GridViewDataManager.PriorToColumn,
          GridViewDataManager.CondStatusColumn,
          GridViewDataManager.DateColumn
        });
        conditionTrackingView.Layout = tableLayout;
        return conditionTrackingView;
      }
    }

    public static ConditionTrackingView StandardUnderwritingConditionTrackingView
    {
      get
      {
        ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
        TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
        nameColumn.SortOrder = SortOrder.Ascending;
        nameColumn.SortPriority = 0;
        TableLayout tableLayout = new TableLayout(new TableLayout.Column[11]
        {
          GridViewDataManager.HasDocumentsColumn,
          nameColumn,
          GridViewDataManager.DescriptionColumn,
          GridViewDataManager.CondSourceColumn,
          GridViewDataManager.PrintInternallyColumn,
          GridViewDataManager.PrintExternallyColumn,
          GridViewDataManager.OwnerColumn,
          GridViewDataManager.CategoryColumn,
          GridViewDataManager.PriorToColumn,
          GridViewDataManager.CondStatusColumn,
          GridViewDataManager.DateColumn
        });
        conditionTrackingView.Layout = tableLayout;
        return conditionTrackingView;
      }
    }

    public static ConditionTrackingView StandardSellConditionTrackingView
    {
      get
      {
        ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
        TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
        nameColumn.SortOrder = SortOrder.Ascending;
        nameColumn.SortPriority = 0;
        TableLayout tableLayout = new TableLayout(new TableLayout.Column[12]
        {
          GridViewDataManager.HasDocumentsColumn,
          GridViewDataManager.ConditionCodeColumn,
          nameColumn,
          GridViewDataManager.DescriptionColumn,
          GridViewDataManager.CondSourceColumn,
          GridViewDataManager.PrintInternallyColumn,
          GridViewDataManager.PrintExternallyColumn,
          GridViewDataManager.OwnerColumn,
          GridViewDataManager.CategoryColumn,
          GridViewDataManager.PriorToColumn,
          GridViewDataManager.CondStatusColumn,
          GridViewDataManager.DateColumn
        });
        conditionTrackingView.Layout = tableLayout;
        return conditionTrackingView;
      }
    }

    public static ConditionTrackingView GetStandardUnderwritingConditionTrackingView(
      Sessions.Session session)
    {
      ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
      TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
      nameColumn.SortOrder = SortOrder.Ascending;
      nameColumn.SortPriority = 0;
      TableLayout tableLayout = new TableLayout(new TableLayout.Column[11]
      {
        GridViewDataManager.HasDocumentsColumn,
        nameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.GetPrintExternallyColumn(session),
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn
      });
      conditionTrackingView.Layout = tableLayout;
      return conditionTrackingView;
    }

    public static ConditionTrackingView GetStandardSellConditionTrackingView(
      Sessions.Session session)
    {
      ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
      TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
      nameColumn.SortOrder = SortOrder.Ascending;
      nameColumn.SortPriority = 0;
      TableLayout tableLayout = new TableLayout(new TableLayout.Column[12]
      {
        GridViewDataManager.HasDocumentsColumn,
        GridViewDataManager.ConditionCodeColumn,
        nameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.GetPrintExternallyColumn(session),
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn
      });
      conditionTrackingView.Layout = tableLayout;
      return conditionTrackingView;
    }

    public static ConditionTrackingView StandardPostClosingConditionTrackingView
    {
      get
      {
        ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
        TableLayout.Column nameColumn = GridViewDataManager.NameColumn;
        nameColumn.SortOrder = SortOrder.Ascending;
        nameColumn.SortPriority = 0;
        TableLayout tableLayout = new TableLayout(new TableLayout.Column[9]
        {
          GridViewDataManager.HasDocumentsColumn,
          nameColumn,
          GridViewDataManager.DescriptionColumn,
          GridViewDataManager.CondSourceColumn,
          GridViewDataManager.RecipientColumn,
          GridViewDataManager.CondStatusColumn,
          GridViewDataManager.DateColumn,
          GridViewDataManager.PrintInternallyColumn,
          GridViewDataManager.PrintExternallyColumn
        });
        conditionTrackingView.Layout = tableLayout;
        return conditionTrackingView;
      }
    }

    public static ConditionTrackingView StandardEnhancedConditionTrackingView
    {
      get
      {
        ConditionTrackingView conditionTrackingView = new ConditionTrackingView("Standard View");
        TableLayout.Column condNameColumn = GridViewDataManager.CondNameColumn;
        condNameColumn.SortOrder = SortOrder.Ascending;
        condNameColumn.SortPriority = 0;
        TableLayout tableLayout = new TableLayout(new TableLayout.Column[9]
        {
          condNameColumn,
          GridViewDataManager.DocumentCountColumn,
          GridViewDataManager.ExternalDescriptionColumn,
          GridViewDataManager.PriorToEnhancedColumn,
          GridViewDataManager.CommentCountColumn,
          GridViewDataManager.DispositionColumn,
          GridViewDataManager.CondLatestStatusColumn,
          GridViewDataManager.CondStatusDateTimeColumn,
          GridViewDataManager.CondStatusUserColumn
        });
        conditionTrackingView.Layout = tableLayout;
        return conditionTrackingView;
      }
    }

    public TableLayout GetCurrentLayout() => this.layoutMgr.GetCurrentLayout();

    public FieldFilterList GetCurrentFilter() => this.filterMgr.ToFieldFilterList();

    private void setFilters(Hashtable filterColumnList)
    {
      foreach (GVColumn column in this.gridView.Columns)
      {
        TableLayout.Column tag = (TableLayout.Column) column.Tag;
        Control filterControl = column.FilterControl;
        if (filterColumnList.ContainsKey((object) tag.ColumnID))
        {
          FieldFilter filterColumn = (FieldFilter) filterColumnList[(object) tag.ColumnID];
          if (filterColumn != null)
            this.filterMgr.SetFilterValue(filterControl, filterColumn.ValueFrom);
        }
        else
          this.filterMgr.ClearFilterValue(filterControl);
      }
    }

    public void ApplyView(DocumentTrackingView documentView)
    {
      this.layoutMgr.ApplyLayout(documentView.Layout, false, true);
      this.createColumnSorts();
      if (!this.gridView.FilterVisible)
        return;
      this.createFilters();
      Hashtable filterColumnList = new Hashtable();
      if (documentView.Filter != null)
      {
        foreach (FieldFilter fieldFilter in (List<FieldFilter>) documentView.Filter)
          filterColumnList.Add((object) fieldFilter.FieldID, (object) fieldFilter);
      }
      this.setFilters(filterColumnList);
    }

    public void ApplyView(ConditionTrackingView conditionView)
    {
      this.layoutMgr.ApplyLayout(conditionView.Layout, false, true);
      this.createColumnSorts();
      if (!this.gridView.FilterVisible)
        return;
      this.createFilters();
      Hashtable filterColumnList = new Hashtable();
      if (conditionView.Filter != null)
      {
        foreach (FieldFilter fieldFilter in (List<FieldFilter>) conditionView.Filter)
          filterColumnList.Add((object) fieldFilter.FieldID, (object) fieldFilter);
      }
      this.setFilters(filterColumnList);
    }
  }
}
