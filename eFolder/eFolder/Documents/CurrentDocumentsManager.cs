// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.CurrentDocumentsManager
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class CurrentDocumentsManager
  {
    private LoanDataMgr loanDataMgr;
    private Hashtable roleTbl;
    private DocumentLog[] docList;
    private bool conditionDocuments;
    private bool canEditDocuments;
    private bool canSetDocumentAccess;
    private string docsPairId = "NOMATCH";
    private string docsStage = string.Empty;
    private int webCenterIntValue = -1;
    private int tpoWebCenterPortalIntValue = -1;
    private int thirdPartyIntValue = -1;
    private int daysDue;
    private int daysExpire;
    private string dateDue = string.Empty;
    private string dateExpire = string.Empty;
    private string requestedFrom = string.Empty;
    private int requestedIntValue = -1;
    private string dateRequestedString = string.Empty;
    private string requestedBy = string.Empty;
    private int reRequestedIntValue = -1;
    private string dateRerequestedString = string.Empty;
    private string reRequestedBy = string.Empty;
    private int receivedIntValue = -1;
    private string dateReceivedString = string.Empty;
    private string receivedBy = string.Empty;
    private int reviewedIntValue = -1;
    private string dateReviewedString = string.Empty;
    private string reviewedBy = string.Empty;
    private int underwritingReadyIntValue = -1;
    private string dateUnderwritingReadyString = string.Empty;
    private string underwritingReadyBy = string.Empty;
    private int shippingReadyIntValue = -1;
    private string dateShippingReadyString = string.Empty;
    private string shippingReadyBy = string.Empty;
    private bool canAddComments;
    private bool canDeleteComments;

    public CurrentDocumentsManager(LoanDataMgr loanDataMgr)
    {
      this.loanDataMgr = loanDataMgr;
      this.initRoleTbl();
    }

    public DocumentLog[] DocumentList
    {
      get => this.docList;
      set
      {
        this.docList = value;
        this.Refresh();
      }
    }

    public void Refresh()
    {
      this.resetDefaults();
      this.setFirstDocValues();
      this.setCommonDocValues();
    }

    public bool ConditionDocuments
    {
      get => this.conditionDocuments;
      set => this.conditionDocuments = value;
    }

    public bool CanEditDocuments => this.canEditDocuments;

    public bool CanSetDocumentAccess => this.canSetDocumentAccess;

    public string DocsPairId
    {
      get => this.docsPairId;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.PairId = value;
      }
    }

    public string DocsStage
    {
      get => this.docsStage;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.Stage = value;
      }
    }

    public string Access
    {
      get
      {
        if (this.docList == null || this.docList.Length == 0)
          return string.Empty;
        ArrayList arrayList = new ArrayList();
        foreach (DictionaryEntry dictionaryEntry in this.roleTbl)
        {
          int key = (int) dictionaryEntry.Key;
          bool flag1 = true;
          foreach (DocumentLog doc in this.docList)
          {
            if (flag1)
            {
              bool flag2 = false;
              foreach (int allowedRole in doc.AllowedRoles)
              {
                if (!flag2 && allowedRole == key)
                  flag2 = true;
              }
              if (!flag2)
                flag1 = false;
            }
          }
          if (flag1 && this.roleTbl.Contains((object) key))
            arrayList.Add(this.roleTbl[(object) key]);
        }
        if (arrayList.Count > 0)
          arrayList.Sort((IComparer) new CaseInsensitiveComparer());
        else
          arrayList.Add((object) RoleInfo.All.RoleAbbr);
        return string.Join(", ", (string[]) arrayList.ToArray(typeof (string)));
      }
    }

    public int WebCenterIntValue
    {
      get => this.webCenterIntValue;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.IsWebcenter = Convert.ToBoolean(value);
      }
    }

    public int TPOWebCenterPortalIntValue
    {
      get => this.tpoWebCenterPortalIntValue;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.IsTPOWebcenterPortal = Convert.ToBoolean(value);
      }
    }

    public int ThirdPartyIntValue
    {
      get => this.thirdPartyIntValue;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.IsThirdPartyDoc = Convert.ToBoolean(value);
      }
    }

    public int DaysDue
    {
      get => this.daysDue;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (doc.DaysDue != value)
            doc.DaysDue = value;
        }
      }
    }

    public int DaysExpire
    {
      get => this.daysExpire;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (doc.DaysTillExpire != value)
            doc.DaysTillExpire = value;
        }
      }
    }

    public string DateDue => this.dateDue;

    public string DateExpire => this.dateExpire;

    public string RequestedFrom
    {
      get => this.requestedFrom;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (doc.RequestedFrom != value)
            doc.RequestedFrom = value;
        }
      }
    }

    public bool Requested
    {
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (value)
            doc.MarkAsRequested(DateTime.Now, Session.UserID);
          else
            doc.UnmarkAsRequested();
        }
      }
    }

    public int RequestedIntValue => this.requestedIntValue;

    public string DateRequestedString
    {
      get => this.dateRequestedString;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (Convert.ToDateTime(value) != doc.DateRequested)
            doc.MarkAsRequested(Convert.ToDateTime(value), doc.RequestedBy);
        }
      }
    }

    public string RequestedBy
    {
      get => this.requestedBy;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.MarkAsRequested(doc.DateRequested, value);
      }
    }

    public bool Rerequested
    {
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (value)
            doc.MarkAsRerequested(DateTime.Now, Session.UserID);
          else
            doc.UnmarkAsRerequested();
        }
      }
    }

    public int RerequestedIntValue => this.reRequestedIntValue;

    public string DateRerequestedString
    {
      get => this.dateRerequestedString;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (Convert.ToDateTime(value) != doc.DateRerequested)
            doc.MarkAsRerequested(Convert.ToDateTime(value), doc.RerequestedBy);
        }
      }
    }

    public string RerequestedBy
    {
      get => this.reRequestedBy;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.MarkAsRerequested(doc.DateRerequested, value);
      }
    }

    public bool Received
    {
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (value)
            doc.MarkAsReceived(DateTime.Now, Session.UserID);
          else
            doc.UnmarkAsReceived();
        }
      }
    }

    public int ReceivedIntValue => this.receivedIntValue;

    public string DateReceivedString
    {
      get => this.dateReceivedString;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (Convert.ToDateTime(value) != doc.DateReceived)
            doc.MarkAsReceived(Convert.ToDateTime(value), doc.ReceivedBy);
        }
      }
    }

    public string ReceivedBy
    {
      get => this.receivedBy;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.MarkAsReceived(doc.DateReceived, value);
      }
    }

    public bool Reviewed
    {
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (value)
            doc.MarkAsReviewed(DateTime.Now, Session.UserID);
          else
            doc.UnmarkAsReviewed();
        }
      }
    }

    public int ReviewedIntValue => this.reviewedIntValue;

    public string DateReviewedString
    {
      get => this.dateReviewedString;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (Convert.ToDateTime(value) != doc.DateReviewed)
            doc.MarkAsReviewed(Convert.ToDateTime(value), doc.ReviewedBy);
        }
      }
    }

    public string ReviewedBy
    {
      get => this.reviewedBy;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.MarkAsReviewed(doc.DateReviewed, value);
      }
    }

    public bool UnderwritingReady
    {
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (value)
            doc.MarkAsUnderwritingReady(DateTime.Now, Session.UserID);
          else
            doc.UnmarkAsUnderwritingReady();
        }
      }
    }

    public int UnderwritingReadyIntValue => this.underwritingReadyIntValue;

    public string DateUnderwritingReadyString
    {
      get => this.dateUnderwritingReadyString;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (Convert.ToDateTime(value) != doc.DateUnderwritingReady)
            doc.MarkAsUnderwritingReady(Convert.ToDateTime(value), doc.UnderwritingReadyBy);
        }
      }
    }

    public string UnderwritingReadyBy
    {
      get => this.underwritingReadyBy;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.MarkAsUnderwritingReady(doc.DateUnderwritingReady, value);
      }
    }

    public bool ShippingReady
    {
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (value)
            doc.MarkAsShippingReady(DateTime.Now, Session.UserID);
          else
            doc.UnmarkAsShippingReady();
        }
      }
    }

    public int ShippingReadyIntValue => this.shippingReadyIntValue;

    public string DateShippingReadyString
    {
      get => this.dateShippingReadyString;
      set
      {
        foreach (DocumentLog doc in this.docList)
        {
          if (Convert.ToDateTime(value) != doc.DateShippingReady)
            doc.MarkAsShippingReady(Convert.ToDateTime(value), doc.ShippingReadyBy);
        }
      }
    }

    public string ShippingReadyBy
    {
      get => this.shippingReadyBy;
      set
      {
        foreach (DocumentLog doc in this.docList)
          doc.MarkAsShippingReady(doc.DateShippingReady, value);
      }
    }

    public bool CanAddComments => this.canAddComments;

    public bool CanDeleteComments => this.canDeleteComments;

    public void AddComment(CommentEntry entry)
    {
      foreach (DocumentLog doc in this.docList)
      {
        doc.Comments.Add(entry);
        doc.MarkLastUpdated();
      }
    }

    private void initRoleTbl()
    {
      this.roleTbl = new Hashtable();
      foreach (RoleInfo allRole in this.loanDataMgr.SystemConfiguration.AllRoles)
        this.roleTbl[(object) allRole.ID] = (object) allRole.RoleAbbr;
      this.roleTbl[(object) RoleInfo.Others.ID] = (object) RoleInfo.Others.RoleAbbr;
    }

    private void resetDefaults()
    {
      this.canEditDocuments = false;
      this.canSetDocumentAccess = false;
      this.docsPairId = "NOMATCH";
      this.docsStage = string.Empty;
      this.webCenterIntValue = -1;
      this.tpoWebCenterPortalIntValue = -1;
      this.thirdPartyIntValue = -1;
      this.daysDue = 0;
      this.daysExpire = 0;
      this.dateDue = string.Empty;
      this.dateExpire = string.Empty;
      this.requestedFrom = string.Empty;
      this.requestedIntValue = -1;
      this.dateRequestedString = string.Empty;
      this.requestedBy = string.Empty;
      this.reRequestedIntValue = -1;
      this.dateRerequestedString = string.Empty;
      this.reRequestedBy = string.Empty;
      this.receivedIntValue = -1;
      this.dateReceivedString = string.Empty;
      this.receivedBy = string.Empty;
      this.reviewedIntValue = -1;
      this.dateReviewedString = string.Empty;
      this.reviewedBy = string.Empty;
      this.underwritingReadyIntValue = -1;
      this.dateUnderwritingReadyString = string.Empty;
      this.underwritingReadyBy = string.Empty;
      this.shippingReadyIntValue = -1;
      this.dateShippingReadyString = string.Empty;
      this.shippingReadyBy = string.Empty;
      this.canAddComments = false;
      this.canDeleteComments = false;
    }

    private void setFirstDocValues()
    {
      if (((IEnumerable<DocumentLog>) this.docList).Count<DocumentLog>() <= 0)
        return;
      DocumentLog doc = this.docList[0];
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
      this.canEditDocuments = folderAccessRights.CanEditDocument;
      this.canSetDocumentAccess = folderAccessRights.CanSetDocumentAccess;
      this.canAddComments = folderAccessRights.CanAddDocumentComments;
      this.canDeleteComments = folderAccessRights.CanDeleteDocumentComments;
      this.docsPairId = doc.PairId;
      this.docsStage = doc.Stage;
      this.webCenterIntValue = Convert.ToInt32(doc.IsWebcenter);
      this.tpoWebCenterPortalIntValue = Convert.ToInt32(doc.IsTPOWebcenterPortal);
      this.thirdPartyIntValue = Convert.ToInt32(doc.IsThirdPartyDoc);
      this.daysDue = doc.DaysDue;
      this.daysExpire = doc.DaysTillExpire;
      DateTime dateTime;
      if (doc.Expected)
      {
        dateTime = doc.DateExpected;
        this.dateDue = dateTime.ToString("MM/dd/yy");
      }
      if (doc.Expires)
      {
        dateTime = doc.DateExpires;
        this.dateExpire = dateTime.ToString("MM/dd/yy");
      }
      this.requestedFrom = doc.RequestedFrom;
      this.requestedIntValue = Convert.ToInt32(doc.Requested);
      if (doc.Requested)
      {
        dateTime = doc.DateRequested;
        this.dateRequestedString = dateTime.ToString("MM/dd/yy");
      }
      this.requestedBy = doc.RequestedBy;
      this.reRequestedIntValue = Convert.ToInt32(doc.Rerequested);
      if (doc.Rerequested)
      {
        dateTime = doc.DateRerequested;
        this.dateRerequestedString = dateTime.ToString("MM/dd/yy");
      }
      this.reRequestedBy = doc.RerequestedBy;
      this.receivedIntValue = Convert.ToInt32(doc.Received);
      if (doc.Received)
      {
        dateTime = doc.DateReceived;
        this.dateReceivedString = dateTime.ToString("MM/dd/yy");
      }
      this.receivedBy = doc.ReceivedBy;
      this.reviewedIntValue = Convert.ToInt32(doc.Reviewed);
      if (doc.Reviewed)
      {
        dateTime = doc.DateReviewed;
        this.dateReviewedString = dateTime.ToString("MM/dd/yy");
      }
      this.reviewedBy = doc.ReviewedBy;
      this.underwritingReadyIntValue = Convert.ToInt32(doc.UnderwritingReady);
      if (doc.UnderwritingReady)
      {
        dateTime = doc.DateUnderwritingReady;
        this.dateUnderwritingReadyString = dateTime.ToString("MM/dd/yy");
      }
      this.underwritingReadyBy = doc.UnderwritingReadyBy;
      this.shippingReadyIntValue = Convert.ToInt32(doc.ShippingReady);
      if (doc.ShippingReady)
      {
        dateTime = doc.DateShippingReady;
        this.dateShippingReadyString = dateTime.ToString("MM/dd/yy");
      }
      this.shippingReadyBy = doc.ShippingReadyBy;
    }

    private void setCommonDocValues()
    {
      foreach (DocumentLog doc in this.docList)
      {
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc);
        if (!folderAccessRights.CanEditDocument)
          this.canEditDocuments = false;
        if (!folderAccessRights.CanSetDocumentAccess)
          this.canSetDocumentAccess = false;
        if (!folderAccessRights.CanAddDocumentComments)
          this.canAddComments = false;
        if (!folderAccessRights.CanDeleteDocumentComments)
          this.canDeleteComments = false;
        if (doc.PairId != this.docsPairId)
          this.docsPairId = "NOMATCH";
        if (doc.Stage != this.docsStage)
          this.docsStage = string.Empty;
        if (Convert.ToInt32(doc.IsWebcenter) != this.webCenterIntValue)
          this.webCenterIntValue = -1;
        if (Convert.ToInt32(doc.IsTPOWebcenterPortal) != this.tpoWebCenterPortalIntValue)
          this.tpoWebCenterPortalIntValue = -1;
        if (Convert.ToInt32(doc.IsThirdPartyDoc) != this.thirdPartyIntValue)
          this.thirdPartyIntValue = -1;
        if (doc.DaysDue != this.daysDue)
          this.daysDue = 0;
        if (doc.DaysTillExpire != this.daysExpire)
          this.daysExpire = 0;
        if (this.dateDue != string.Empty && doc.Expected && doc.DateExpected.ToString("MM/dd/yy") != this.dateDue)
          this.dateDue = string.Empty;
        if (this.dateExpire != string.Empty && doc.Expires && doc.DateExpires.ToString("MM/dd/yy") != this.dateExpire)
          this.dateExpire = string.Empty;
        if (doc.RequestedFrom != this.requestedFrom)
          this.requestedFrom = string.Empty;
        if (Convert.ToInt32(doc.Requested) != this.requestedIntValue)
          this.requestedIntValue = -1;
        if (this.dateRequestedString != string.Empty && doc.Requested && doc.DateRequested.ToString("MM/dd/yy") != this.dateRequestedString)
          this.dateRequestedString = string.Empty;
        if (doc.RequestedBy != this.requestedBy)
          this.requestedBy = string.Empty;
        if (Convert.ToInt32(doc.Rerequested) != this.reRequestedIntValue)
          this.reRequestedIntValue = -1;
        if (this.dateRerequestedString != string.Empty && doc.Rerequested && doc.DateRerequested.ToString("MM/dd/yy") != this.dateRerequestedString)
          this.dateRerequestedString = string.Empty;
        if (doc.RerequestedBy != this.reRequestedBy)
          this.reRequestedBy = string.Empty;
        if (Convert.ToInt32(doc.Received) != this.receivedIntValue)
          this.receivedIntValue = -1;
        if (this.dateReceivedString != string.Empty && doc.Received && doc.DateReceived.ToString("MM/dd/yy") != this.dateReceivedString)
          this.dateReceivedString = string.Empty;
        if (doc.ReceivedBy != this.receivedBy)
          this.receivedBy = string.Empty;
        if (Convert.ToInt32(doc.Reviewed) != this.reviewedIntValue)
          this.reviewedIntValue = -1;
        if (this.dateReviewedString != string.Empty && doc.Reviewed && doc.DateReviewed.ToString("MM/dd/yy") != this.dateReviewedString)
          this.dateReviewedString = string.Empty;
        if (doc.ReviewedBy != this.reviewedBy)
          this.reviewedBy = string.Empty;
        if (Convert.ToInt32(doc.UnderwritingReady) != this.underwritingReadyIntValue)
          this.underwritingReadyIntValue = -1;
        if (this.dateUnderwritingReadyString != string.Empty && doc.UnderwritingReady && doc.DateUnderwritingReady.ToString("MM/dd/yy") != this.dateUnderwritingReadyString)
          this.dateUnderwritingReadyString = string.Empty;
        if (doc.UnderwritingReadyBy != this.underwritingReadyBy)
          this.underwritingReadyBy = string.Empty;
        if (Convert.ToInt32(doc.ShippingReady) != this.shippingReadyIntValue)
          this.shippingReadyIntValue = -1;
        if (this.dateShippingReadyString != string.Empty && doc.ShippingReady && doc.DateShippingReady.ToString("MM/dd/yy") != this.dateShippingReadyString)
          this.dateShippingReadyString = string.Empty;
        if (doc.ShippingReadyBy != this.shippingReadyBy)
          this.shippingReadyBy = string.Empty;
      }
    }
  }
}
