// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.DocumentMapper
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.Common.Extensions;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  public class DocumentMapper
  {
    public void WriteTo(
      DocumentCreateContract source,
      DocumentLog target,
      string userId,
      string userName)
    {
      if (source == null)
        return;
      if (source.Title != null)
        target.Title = source.Title;
      if (source.Description != null)
        target.Description = source.Description;
      if (source.RequestedFrom != null)
        target.RequestedFrom = source.RequestedFrom;
      if (source.DateRequested.HasValue)
        target.MarkAsRequested(source.DateRequested.Value, userId);
      if (source.DateExpected.HasValue)
      {
        DatetimeUtils datetimeUtils = new DatetimeUtils(target.DateRequested, DateTimeType.Calendar);
        target.DaysDue = (int) (short) datetimeUtils.NumberOfDaysFrom(source.DateExpected.Value);
      }
      if (source.DateReceived.HasValue)
        target.MarkAsReceived(source.DateReceived.Value, userId);
      if (source.DateReviewed.HasValue)
        target.MarkAsReviewed(source.DateReviewed.Value, userId);
      if (source.DateReadyForUW.HasValue)
        target.MarkAsUnderwritingReady(source.DateReadyForUW.Value, userId);
      if (source.DateReadyToShip.HasValue)
        target.MarkAsShippingReady(source.DateReadyToShip.Value, userId);
      if (source.EMNSignature != null)
      {
        target.IsePASS = source.EMNSignature != string.Empty;
        target.EPASSSignature = source.EMNSignature;
      }
      if (source.Comments != null)
      {
        foreach (CommentCreateContract comment in source.Comments)
        {
          int? forRoleId = comment.ForRoleId;
          if (forRoleId.HasValue)
          {
            CommentEntryCollection comments1 = target.Comments;
            string comments2 = comment.Comments;
            string addedBy = userId;
            string addedByName = userName;
            forRoleId = comment.ForRoleId;
            int alertRoleID = forRoleId.Value;
            comments1.Add(comments2, addedBy, addedByName, false, alertRoleID);
          }
          else
            target.Comments.Add(comment.Comments, userId, userName);
        }
      }
      if (source.FileAttachments != null)
      {
        foreach (AttachmentReferenceCreateContract fileAttachment in source.FileAttachments)
          target.Files.Add(fileAttachment.AttachmentId, userId, ((int) fileAttachment.IsActive ?? 1) != 0);
      }
      if (source.Conditions == null)
        return;
      foreach (ConditionReferenceCreateContract condition in source.Conditions)
        target.Conditions.Add(condition.ConditionId.ToString());
    }

    public void WriteTo(DocumentSaveContract source, DocumentLog target)
    {
      if (source == null)
        return;
      if (source.Title != null)
        target.Title = source.Title;
      if (source.Description != null)
        target.Description = source.Description;
      if (source.ApplicationId != null)
        target.PairId = source.ApplicationId;
      if (source.MilestoneId != null)
        target.Stage = source.MilestoneId;
      if (source.WebCenterAllowed.HasValue)
        target.IsWebcenter = source.WebCenterAllowed.Value;
      if (source.TPOAllowed.HasValue)
        target.IsTPOWebcenterPortal = source.TPOAllowed.Value;
      if (source.ThirdPartyAllowed.HasValue)
        target.IsThirdPartyDoc = source.ThirdPartyAllowed.Value;
      if (source.RequestedFrom != null)
        target.RequestedFrom = source.RequestedFrom;
      if (source.DaysDue.HasValue)
        target.DaysDue = source.DaysDue.Value;
      if (source.DaysTillExpire.HasValue)
        target.DaysTillExpire = source.DaysTillExpire.Value;
      if (source.IsAssetVerification.HasValue)
        target.IsAssetVerification = source.IsAssetVerification.Value;
      if (source.IsEmploymentVerification.HasValue)
        target.IsEmploymentVerification = source.IsEmploymentVerification.Value;
      if (source.IsIncomeVerification.HasValue)
        target.IsIncomeVerification = source.IsIncomeVerification.Value;
      if (source.IsObligationVerification.HasValue)
        target.IsObligationVerification = source.IsObligationVerification.Value;
      if (source.EMNSignature != null)
      {
        target.IsePASS = source.EMNSignature != string.Empty;
        target.EPASSSignature = source.EMNSignature;
      }
      MapperCommon.UpdateStatusInfo(source.IsRequested, source.DateRequested, source.RequestedBy, target.DateRequested, target.RequestedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsRequested), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsRequested));
      MapperCommon.UpdateStatusInfo(source.IsRerequested, source.DateRerequested, source.RerequestedBy, target.DateRerequested, target.RerequestedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsRerequested), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsRerequested));
      MapperCommon.UpdateStatusInfo(source.IsReceived, source.DateReceived, source.ReceivedBy, target.DateReceived, target.ReceivedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsReceived), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsReceived));
      MapperCommon.UpdateStatusInfo(source.IsReviewed, source.DateReviewed, source.ReviewedBy, target.DateReviewed, target.ReviewedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsReviewed), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsReviewed));
      MapperCommon.UpdateStatusInfo(source.IsReadyForUW, source.DateReadyForUW, source.ReadyForUWBy, target.DateUnderwritingReady, target.UnderwritingReadyBy, new MapperCommon.MarkStatusDelegate(target.MarkAsUnderwritingReady), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsUnderwritingReady));
      MapperCommon.UpdateStatusInfo(source.IsReadyToShip, source.DateReadyToShip, source.ReadyToShipBy, target.DateShippingReady, target.ShippingReadyBy, new MapperCommon.MarkStatusDelegate(target.MarkAsShippingReady), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsShippingReady));
    }

    public void WriteTo(
      DocumentLog source,
      DocumentGetContract target,
      FileAttachment[] fileAttachments,
      RoleInfo[] roles,
      LoanData loanData,
      string titleWithIndex)
    {
      if (source == null)
        return;
      target.DocumentId = Guid.Parse(source.Guid);
      target.Title = source.Title;
      target.TitleWithIndex = titleWithIndex;
      target.Description = source.Description;
      target.ApplicationId = source.PairId;
      target.ApplicationName = MapperCommon.GetBorrowerPairName(source.PairId, loanData);
      target.MilestoneId = source.Stage;
      target.WebCenterAllowed = source.IsWebcenter;
      target.TPOAllowed = source.IsTPOWebcenterPortal;
      target.ThirdPartyAllowed = source.IsThirdPartyDoc;
      target.DateCreated = source.DateAdded;
      target.CreatedBy = source.AddedBy;
      target.DateRequested = source.DateRequested.ToDateTimeOrDefault();
      target.RequestedBy = source.RequestedBy;
      target.RequestedFrom = source.RequestedFrom;
      target.DateRerequested = source.DateRerequested.ToDateTimeOrDefault();
      target.RerequestedBy = source.RerequestedBy;
      target.DaysDue = source.DaysDue;
      target.DateExpected = source.DateExpected.ToDateTimeOrDefault();
      target.DateReceived = source.DateReceived.ToDateTimeOrDefault();
      target.ReceivedBy = source.ReceivedBy;
      target.DaysTillExpire = source.DaysTillExpire;
      target.DateExpires = new DateTime?(source.DateExpires);
      target.DateReviewed = source.DateReviewed.ToDateTimeOrDefault();
      target.ReviewedBy = source.ReviewedBy;
      target.DateReadyForUW = source.DateUnderwritingReady.ToDateTimeOrDefault();
      target.ReadyForUWBy = source.UnderwritingReadyBy;
      target.DateReadyToShip = source.DateShippingReady.ToDateTimeOrDefault();
      target.ReadyToShipBy = source.ShippingReadyBy;
      target.IsAssetVerification = source.IsAssetVerification;
      target.IsEmploymentVerification = source.IsEmploymentVerification;
      target.IsIncomeVerification = source.IsIncomeVerification;
      target.IsObligationVerification = source.IsObligationVerification;
      target.IsProtected = loanData.AccessRules.IsLogEntryProtected((LogRecordBase) source);
      target.EMNSignature = source.EPASSSignature;
      target.Comments = new List<CommentGetContract>();
      foreach (CommentEntry comment in (CollectionBase) source.Comments)
      {
        CommentGetContract commentGetContract1 = new CommentGetContract();
        commentGetContract1.CommentId = Guid.Parse(comment.Guid);
        commentGetContract1.DateCreated = comment.Date;
        commentGetContract1.CreatedBy = comment.AddedBy;
        commentGetContract1.CreatedByName = comment.AddedByName;
        commentGetContract1.Comments = comment.Comments;
        commentGetContract1.ForRoleId = comment.ForRoleID.ToRoleIdValue();
        commentGetContract1.DateReviewed = comment.ReviewedDate.ToDateTimeOrDefault();
        commentGetContract1.ReviewedBy = comment.ReviewedBy;
        CommentGetContract commentGetContract2 = commentGetContract1;
        target.Comments.Add(commentGetContract2);
      }
      target.Conditions = new List<ConditionReferenceGetContract>();
      foreach (ConditionLog condition in source.Conditions)
      {
        ConditionReferenceGetContract referenceGetContract = new ConditionReferenceGetContract();
        referenceGetContract.ConditionId = Guid.Parse(condition.Guid);
        referenceGetContract.Title = condition.Title;
        referenceGetContract.ConditionType = (int) condition.ConditionType;
        target.Conditions.Add(referenceGetContract);
      }
      target.FileAttachments = new List<AttachmentReferenceGetContract>();
      foreach (FileAttachment fileAttachment1 in fileAttachments)
      {
        FileAttachment fileAttachment = fileAttachment1;
        Enumerable.Cast<FileAttachmentReference>(source.Files).ForEach<FileAttachmentReference>((Action<FileAttachmentReference>) (docFile =>
        {
          if (!(fileAttachment.ID == docFile.AttachmentID))
            return;
          target.FileAttachments.Add(new AttachmentReferenceGetContract()
          {
            AttachmentId = fileAttachment.ID,
            Title = fileAttachment.Title,
            DateCreated = fileAttachment.Date,
            CreatedBy = fileAttachment.UserID,
            CreatedByName = fileAttachment.UserName,
            IsActive = new bool?(docFile.IsActive),
            FileSize = fileAttachment.FileSize,
            AttachmentType = (int) fileAttachment.AttachmentType
          });
        }));
      }
      target.Roles = new List<RoleGetContract>();
      foreach (int allowedRole in source.AllowedRoles)
      {
        int docRole = allowedRole;
        foreach (RoleInfo roleInfo in ((IEnumerable<RoleInfo>) roles).Where<RoleInfo>((Func<RoleInfo, bool>) (r => r.RoleID == docRole)))
        {
          RoleGetContract roleGetContract = new RoleGetContract();
          roleGetContract.RoleId = roleInfo.RoleID;
          roleGetContract.RoleAbbreviation = roleInfo.RoleAbbr;
          target.Roles.Add(roleGetContract);
        }
      }
    }
  }
}
