// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ITrackedDocument
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for TrackedDocument class.</summary>
  /// <exclude />
  [Guid("3F1E6A28-3F49-4f63-BE01-209A7D1ADAD0")]
  public interface ITrackedDocument
  {
    string ID { get; }

    object Date { get; }

    LogEntryType EntryType { get; }

    Comments Comments { get; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    string Title { get; }

    string Company { get; set; }

    string MilestoneName { get; set; }

    BorrowerPair BorrowerPair { get; }

    object OrderDate { get; set; }

    void SetOrderDate(object newDate);

    object ReceivedDate { get; set; }

    void SetReceivedDate(object newDate);

    object ReorderDate { get; set; }

    void SetReorderDate(object newDate);

    int DueDays { get; set; }

    object DueDate { get; }

    bool PastDue { get; }

    int ExpirationDays { get; set; }

    object ExpirationDate { get; }

    bool Expired { get; }

    AttachmentList GetAttachments();

    void Attach(Attachment attchmnt);

    void Detach(Attachment attchmnt);

    RoleAccessList RoleAccessList { get; }

    string AddedBy { get; }

    object DateAdded { get; }

    bool Due { get; }

    bool Received { get; }

    string ReceivedBy { get; set; }

    bool Ordered { get; }

    string OrderedBy { get; set; }

    bool Reordered { get; }

    string ReorderedBy { get; set; }

    bool Archived { get; }

    object ArchiveDate { get; set; }

    void SetArchiveDate(object newDate);

    string ArchivedBy { get; set; }

    LogEntryList GetLinkedConditions();

    void LinkToCondition(Condition cond);

    void UnlinkCondition(Condition cond);

    bool IncludeInEDisclosurePackage { get; set; }

    bool IsClosingDocument { get; }

    bool IsPreClosingDocument { get; }

    bool IsOpeningDocument { get; }

    bool IsEMNDocument { get; }

    bool ShippingReady { get; }

    string ShippingReadyBy { get; set; }

    object ShippingReadyDate { get; }

    void SetShippingReadyDate(object date);

    bool UnderwritingReady { get; }

    string UnderwritingReadyBy { get; set; }

    object UnderwritingReadyDate { get; }

    void SetUnderwritingReadyDate(object date);

    string Description { get; set; }

    bool IsWebcenter { get; set; }

    bool IsTPOWebcenterPortal { get; set; }

    bool IsThirdPartyDoc { get; set; }

    object ReviewedDate { get; set; }

    void SetReviewedDate(object newDate);

    bool Reviewed { get; }

    string ReviewedBy { get; set; }
  }
}
