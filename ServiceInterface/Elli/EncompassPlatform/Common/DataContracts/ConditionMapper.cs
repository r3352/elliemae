// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.ConditionMapper
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  public class ConditionMapper
  {
    public static void WriteTo(
      PreliminaryConditionCreateContract source,
      PreliminaryConditionLog target,
      string userId,
      string userName)
    {
      if (source == null)
        return;
      if (source.Title != null)
        target.Title = source.Title;
      if (source.Description != null)
        target.Description = source.Description;
      if (source.PriorTo.HasValue)
        target.PriorTo = LoanConstants.PriorToValueConversion(((ConditionPriorTo) source.PriorTo.Value).ToString());
      if (source.Category.HasValue)
        target.Category = ((ConditionCategory) source.Category.Value).ToString();
      if (source.Source != null)
        target.Source = source.Source;
      if (source.ApplicationId != null)
        target.PairId = source.ApplicationId;
      if (source.DescriptionDetails != null)
        target.Details = source.DescriptionDetails;
      if (source.Comments == null)
        return;
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

    public static void WriteTo(
      PreliminaryConditionSaveContract source,
      PreliminaryConditionLog target,
      string userId)
    {
      if (source == null)
        return;
      if (source.Title != null)
        target.Title = source.Title;
      if (source.Description != null)
        target.Description = source.Description;
      if (source.PriorTo.HasValue)
        target.PriorTo = LoanConstants.PriorToValueConversion(((ConditionPriorTo) source.PriorTo.Value).ToString());
      if (source.Category.HasValue)
        target.Category = ((ConditionCategory) source.Category.Value).ToString();
      if (source.Source != null)
        target.Source = source.Source;
      if (source.ApplicationId != null)
        target.PairId = source.ApplicationId;
      if (source.DescriptionDetails != null)
        target.Details = source.DescriptionDetails;
      if (source.UWCanAccess.HasValue)
        target.UnderwriterAccess = source.UWCanAccess.Value;
      if (source.DaysToReceive.HasValue)
        target.DaysTillDue = source.DaysToReceive.Value;
      if (source.RequestedFrom != null)
        target.RequestedFrom = source.RequestedFrom;
      MapperCommon.UpdateStatusInfo(source.IsFulfilled, source.DateFulfilled, source.FulfilledBy, target.DateFulfilled, target.FulfilledBy, new MapperCommon.MarkStatusDelegate(target.MarkAsFulfilled), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsFulfilled));
      MapperCommon.UpdateStatusInfo(source.IsRequested, source.DateRequested, source.RequestedBy, target.DateRequested, target.RequestedBy, new MapperCommon.MarkStatusDelegate(((StandardConditionLog) target).MarkAsRequested), new MapperCommon.UnMarkStatusDelegate(((StandardConditionLog) target).UnmarkAsRequested));
      MapperCommon.UpdateStatusInfo(source.IsRerequested, source.DateRerequested, source.RerequestedBy, target.DateRerequested, target.RerequestedBy, new MapperCommon.MarkStatusDelegate(((StandardConditionLog) target).MarkAsRerequested), new MapperCommon.UnMarkStatusDelegate(((StandardConditionLog) target).UnmarkAsRerequested));
      MapperCommon.UpdateStatusInfo(source.IsReceived, source.DateReceived, source.ReceivedBy, target.DateReceived, target.ReceivedBy, new MapperCommon.MarkStatusDelegate(((StandardConditionLog) target).MarkAsReceived), new MapperCommon.UnMarkStatusDelegate(((StandardConditionLog) target).UnmarkAsReceived));
    }

    public static void WriteTo(
      UnderwritingConditionSaveContract source,
      UnderwritingConditionLog target,
      string userId)
    {
      if (source == null)
        return;
      if (source.Title != null)
        target.Title = source.Title;
      if (source.Description != null)
        target.Description = source.Description;
      if (source.PriorTo.HasValue)
        target.PriorTo = LoanConstants.PriorToValueConversion(((ConditionPriorTo) source.PriorTo.Value).ToString());
      if (source.Category.HasValue)
        target.Category = ((ConditionCategory) source.Category.Value).ToString();
      if (source.Source != null)
        target.Source = source.Source;
      if (source.ApplicationId != null)
        target.PairId = source.ApplicationId;
      if (source.DescriptionDetails != null)
        target.Details = source.DescriptionDetails;
      if (source.OwnerId.HasValue)
        target.ForRoleID = source.OwnerId.Value;
      if (source.AllowToClear.HasValue)
        target.AllowToClear = source.AllowToClear.Value;
      if (source.PrintInternally.HasValue)
        target.IsInternal = source.PrintInternally.Value;
      if (source.PrintExternally.HasValue)
        target.IsExternal = source.PrintExternally.Value;
      if (source.DaysToReceive.HasValue)
        target.DaysTillDue = source.DaysToReceive.Value;
      if (source.RequestedFrom != null)
        target.RequestedFrom = source.RequestedFrom;
      MapperCommon.UpdateStatusInfo(source.IsFulfilled, source.DateFulfilled, source.FulfilledBy, target.DateFulfilled, target.FulfilledBy, new MapperCommon.MarkStatusDelegate(target.MarkAsFulfilled), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsFulfilled));
      MapperCommon.UpdateStatusInfo(source.IsRequested, source.DateRequested, source.RequestedBy, target.DateRequested, target.RequestedBy, new MapperCommon.MarkStatusDelegate(((StandardConditionLog) target).MarkAsRequested), new MapperCommon.UnMarkStatusDelegate(((StandardConditionLog) target).UnmarkAsRequested));
      MapperCommon.UpdateStatusInfo(source.IsRerequested, source.DateRerequested, source.RerequestedBy, target.DateRerequested, target.RerequestedBy, new MapperCommon.MarkStatusDelegate(((StandardConditionLog) target).MarkAsRerequested), new MapperCommon.UnMarkStatusDelegate(((StandardConditionLog) target).UnmarkAsRerequested));
      MapperCommon.UpdateStatusInfo(source.IsReceived, source.DateReceived, source.ReceivedBy, target.DateReceived, target.ReceivedBy, new MapperCommon.MarkStatusDelegate(((StandardConditionLog) target).MarkAsReceived), new MapperCommon.UnMarkStatusDelegate(((StandardConditionLog) target).UnmarkAsReceived));
      MapperCommon.UpdateStatusInfo(source.IsReviewed, source.DateReviewed, source.ReviewedBy, target.DateReviewed, target.ReviewedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsReviewed), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsReviewed));
      MapperCommon.UpdateStatusInfo(source.IsRejected, source.DateRejected, source.RejectedBy, target.DateRejected, target.RejectedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsRejected), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsRejected));
      MapperCommon.UpdateStatusInfo(source.IsCleared, source.DateCleared, source.ClearedBy, target.DateCleared, target.ClearedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsCleared), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsCleared));
      MapperCommon.UpdateStatusInfo(source.IsWaived, source.DateWaived, source.WaivedBy, target.DateWaived, target.WaivedBy, new MapperCommon.MarkStatusDelegate(target.MarkAsWaived), new MapperCommon.UnMarkStatusDelegate(target.UnmarkAsWaived));
    }

    public static void WriteTo(
      UnderwritingConditionCreateContract source,
      UnderwritingConditionLog target,
      string userId,
      string userName)
    {
      if (source == null)
        return;
      if (source.Title != null)
        target.Title = source.Title;
      if (source.Description != null)
        target.Description = source.Description;
      if (source.PriorTo.HasValue)
        target.PriorTo = LoanConstants.PriorToValueConversion(((ConditionPriorTo) source.PriorTo.Value).ToString());
      if (source.Category.HasValue)
        target.Category = ((ConditionCategory) source.Category.Value).ToString();
      if (source.Source != null)
        target.Source = source.Source;
      if (source.ApplicationId != null)
        target.PairId = source.ApplicationId;
      if (source.DescriptionDetails != null)
        target.Details = source.DescriptionDetails;
      if (source.Comments == null)
        return;
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

    public static void WriteTo(
      UnderwritingConditionLog source,
      UnderwritingConditionGetContract target,
      LoanData loanData,
      Dictionary<DocumentLog, string> documentLogsWithIndex)
    {
      if (source == null)
        return;
      target.ConditionId = Guid.Parse(source.Guid);
      target.Title = source.Title;
      target.Source = source.Source;
      target.Status = (int) source.Status;
      target.StatusDate = source.Date;
      target.ApplicationId = source.PairId;
      target.ApplicationName = MapperCommon.GetBorrowerPairName(source.PairId, loanData);
      target.Description = source.Description;
      target.DescriptionDetails = source.Details;
      target.AllowToClear = source.AllowToClear;
      target.DaysToReceive = source.DaysTillDue;
      target.RequestedFrom = source.RequestedFrom;
      target.OwnerId = source.ForRoleID.ToRoleIdValue();
      target.PrintExternally = source.IsExternal;
      target.PrintInternally = source.IsInternal;
      if (!string.IsNullOrEmpty(source.Category))
        target.Category = new int?((int) Enum.Parse(typeof (ConditionCategory), source.Category, true));
      if (!string.IsNullOrEmpty(source.PriorTo))
        target.PriorTo = new int?((int) Enum.Parse(typeof (ConditionPriorTo), LoanConstants.PriorToUIConversion(source.PriorTo), true));
      target.DateCreated = source.DateAdded;
      target.CreatedBy = source.AddedBy;
      target.DateFulfilled = source.DateFulfilled.ToDateTimeOrDefault();
      target.FulfilledBy = source.FulfilledBy;
      target.DateRequested = source.DateRequested.ToDateTimeOrDefault();
      target.RequestedBy = source.RequestedBy;
      target.DateRerequested = source.DateRerequested.ToDateTimeOrDefault();
      target.RerequestedBy = source.RerequestedBy;
      target.DateReceived = source.DateReceived.ToDateTimeOrDefault();
      target.ReceivedBy = source.ReceivedBy;
      target.DateReviewed = source.DateReviewed.ToDateTimeOrDefault();
      target.ReviewedBy = source.ReviewedBy;
      target.DateRejected = source.DateRejected.ToDateTimeOrDefault();
      target.RejectedBy = source.RejectedBy;
      target.DateCleared = source.DateCleared.ToDateTimeOrDefault();
      target.ClearedBy = source.ClearedBy;
      target.DateWaived = source.DateWaived.ToDateTimeOrDefault();
      target.WaivedBy = source.WaivedBy;
      target.Comments = new List<CommentGetContract>();
      foreach (CommentEntry comment in (CollectionBase) source.Comments)
      {
        CommentGetContract commentGetContract = new CommentGetContract();
        commentGetContract.CommentId = Guid.Parse(comment.Guid);
        commentGetContract.DateCreated = comment.Date;
        commentGetContract.CreatedBy = comment.AddedBy;
        commentGetContract.CreatedByName = comment.AddedByName;
        commentGetContract.Comments = comment.Comments;
        commentGetContract.ForRoleId = comment.ForRoleID.ToRoleIdValue();
        commentGetContract.DateReviewed = comment.ReviewedDate.ToDateTimeOrDefault();
        commentGetContract.ReviewedBy = comment.ReviewedBy;
        target.Comments.Add(commentGetContract);
      }
      target.Documents = new List<DocumentReferenceGetContract>();
      foreach (KeyValuePair<DocumentLog, string> keyValuePair in documentLogsWithIndex)
      {
        DocumentLog key = keyValuePair.Key;
        if (key.Conditions.Contains((ConditionLog) source))
        {
          DocumentReferenceGetContract referenceGetContract = new DocumentReferenceGetContract();
          referenceGetContract.DocumentId = Guid.Parse(key.Guid);
          referenceGetContract.Title = key.Title;
          referenceGetContract.TitleWithIndex = keyValuePair.Value;
          target.Documents.Add(referenceGetContract);
        }
      }
    }

    public static void WriteTo(
      PreliminaryConditionLog source,
      PreliminaryConditionGetContract target,
      LoanData loanData,
      Dictionary<DocumentLog, string> documentLogsWithIndex)
    {
      if (source == null)
        return;
      target.ConditionId = Guid.Parse(source.Guid);
      target.Title = source.Title;
      target.Source = source.Source;
      target.Status = (int) source.Status;
      target.StatusDate = source.Date;
      target.ApplicationId = source.PairId;
      target.ApplicationName = MapperCommon.GetBorrowerPairName(source.PairId, loanData);
      target.Description = source.Description;
      target.DescriptionDetails = source.Details;
      target.UWCanAccess = source.UnderwriterAccess;
      target.DaysToReceive = source.DaysTillDue;
      target.RequestedFrom = source.RequestedFrom;
      if (!string.IsNullOrEmpty(source.Category))
        target.Category = new int?((int) Enum.Parse(typeof (ConditionCategory), source.Category, true));
      if (!string.IsNullOrEmpty(source.PriorTo))
        target.PriorTo = new int?((int) Enum.Parse(typeof (ConditionPriorTo), LoanConstants.PriorToUIConversion(source.PriorTo), true));
      target.DateCreated = source.DateAdded;
      target.CreatedBy = source.AddedBy;
      target.DateFulfilled = source.DateFulfilled.ToDateTimeOrDefault();
      target.FulFilledBy = source.FulfilledBy;
      target.DateRequested = source.DateRequested.ToDateTimeOrDefault();
      target.RequestedBy = source.RequestedBy;
      target.DateRerequested = source.DateRerequested.ToDateTimeOrDefault();
      target.ReRequestedBy = source.RerequestedBy;
      target.DateReceived = source.DateReceived.ToDateTimeOrDefault();
      target.ReceivedBy = source.ReceivedBy;
      target.Comments = new List<CommentGetContract>();
      foreach (CommentEntry comment in (CollectionBase) source.Comments)
      {
        CommentGetContract commentGetContract = new CommentGetContract();
        commentGetContract.CommentId = Guid.Parse(comment.Guid);
        commentGetContract.DateCreated = comment.Date;
        commentGetContract.CreatedBy = comment.AddedBy;
        commentGetContract.CreatedByName = comment.AddedByName;
        commentGetContract.Comments = comment.Comments;
        commentGetContract.ForRoleId = comment.ForRoleID.ToRoleIdValue();
        commentGetContract.DateReviewed = comment.ReviewedDate.ToDateTimeOrDefault();
        commentGetContract.ReviewedBy = comment.ReviewedBy;
        target.Comments.Add(commentGetContract);
      }
      target.Documents = new List<DocumentReferenceGetContract>();
      foreach (KeyValuePair<DocumentLog, string> keyValuePair in documentLogsWithIndex)
      {
        DocumentLog key = keyValuePair.Key;
        if (key.Conditions.Contains((ConditionLog) source))
        {
          DocumentReferenceGetContract referenceGetContract = new DocumentReferenceGetContract();
          referenceGetContract.DocumentId = Guid.Parse(key.Guid);
          referenceGetContract.Title = key.Title;
          referenceGetContract.TitleWithIndex = keyValuePair.Value;
          target.Documents.Add(referenceGetContract);
        }
      }
    }
  }
}
