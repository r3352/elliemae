// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ConcurrentUpdateManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class ConcurrentUpdateManager
  {
    private const string className = "ConcurrentUpdateManager�";

    public static void ApplyMergeUpdatesToLoan(
      XmlDocument loan,
      string systemId,
      out IStageLoanHistoryManager loanHistories)
    {
      XmlElement documentElement = loan.DocumentElement;
      loanHistories = (IStageLoanHistoryManager) new StageLoanHistoryManager();
      long sequenceNumber = ConcurrentUpdateXmlHelper.GetMergeUpdateSequenceNum(documentElement, loan, systemId);
      string loanGuid = ConcurrentUpdateXmlHelper.getLoanGuid(documentElement, loan);
      try
      {
        List<ConcurrentUpdateModel> concurrentUpdatesByLoanId = ConcurrentUpdateAccessor.GetConcurrentUpdatesByLoanId(loanGuid);
        if (concurrentUpdatesByLoanId.Count <= 0)
          return;
        foreach (ConcurrentUpdateModel concurrentUpdateModel in concurrentUpdatesByLoanId.OrderBy<ConcurrentUpdateModel, long>((Func<ConcurrentUpdateModel, long>) (o => o.SequenceNumber)).ToList<ConcurrentUpdateModel>())
        {
          if (concurrentUpdateModel.SequenceNumber > sequenceNumber)
          {
            switch (concurrentUpdateModel.ActionType)
            {
              case ConcurrentUpdateActionType.CreateDocument:
                ConcurrentUpdateManager.createDocument(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.AssignAttachmentToDocument:
                ConcurrentUpdateManager.assignAttachments(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.UnAssignedAttachmentFromDocument:
                ConcurrentUpdateManager.unassignAttachments(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.LinkDocumentToACondition:
                ConcurrentUpdateManager.linkConditions(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.UnlinkDocumentFromACondition:
                ConcurrentUpdateManager.unLinkConditions(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.UpdateDocument:
                ConcurrentUpdateManager.updateDocuments(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.UpdateCondition:
                ConcurrentUpdateManager.updateCondition(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.UpdateAssignedAttachment:
                ConcurrentUpdateManager.updateAssignedAttachments(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.AddAllowedRole:
                ConcurrentUpdateManager.addAllowedRoles(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid);
                break;
              case ConcurrentUpdateActionType.RemoveAllowedRole:
                ConcurrentUpdateManager.removeAllowedRoles(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid);
                break;
              case ConcurrentUpdateActionType.AddCommentsToRecord:
                ConcurrentUpdateManager.AddCommentsToRecord(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.UpdateCommentsInRecord:
                ConcurrentUpdateManager.UpdateCommentsToRecord(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.DeleteCommentsFromRecord:
                ConcurrentUpdateManager.DeleteCommentsFromRecord(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.AddEnhanceConditionTrackings:
                ConcurrentUpdateManager.AddEnhanceConditionTrackings(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.RemoveEnhanceConditionTrackings:
                ConcurrentUpdateManager.RemoveEnhanceConditionTrackings(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories);
                break;
              case ConcurrentUpdateActionType.SetReceivedStatus:
                ConcurrentUpdateManager.SetReceivedStatus(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid);
                break;
              case ConcurrentUpdateActionType.DeleteDocument:
                ConcurrentUpdateManager.DeleteDocument(documentElement, loan, concurrentUpdateModel.XmlStr, systemId, loanGuid, concurrentUpdateModel.XmlHistoryStr, loanHistories, concurrentUpdateModel.SequenceNumber, concurrentUpdateModel.MergeParamKeyValues);
                break;
            }
            sequenceNumber = concurrentUpdateModel.SequenceNumber;
          }
        }
        ConcurrentUpdateXmlHelper.SetMergeUpdateSequenceNum(sequenceNumber, documentElement, loan, systemId);
      }
      catch (Exception ex)
      {
        TraceLog.WriteException(nameof (ConcurrentUpdateManager), ex);
      }
    }

    private static void updateCondition(
      XmlElement root,
      XmlDocument loan,
      string xmlStr,
      string systemId,
      string loanGUID,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        string propertyValue1 = ConcurrentUpdateXmlHelper.getPropertyValue(documentElement, "UpdatedDate");
        string propertyValue2 = ConcurrentUpdateXmlHelper.getPropertyValue(documentElement, "UpdatedBy");
        string propertyValueFromLoan = ConcurrentUpdateXmlHelper.getPropertyValueFromLoan(root, systemId, recordGuid, "UpdatedDate");
        DateTime result1;
        DateTime result2;
        if (!string.IsNullOrEmpty(propertyValueFromLoan) && (!DateTime.TryParse(propertyValue1, out result1) || !DateTime.TryParse(propertyValueFromLoan, out result2) || !(result2 < result1)))
          return;
        editDocElement.SetAttribute("UpdatedDate", propertyValue1);
        editDocElement.SetAttribute("UpdatedBy", propertyValue2);
        List<XmlElement> withoutLinkedType = ConcurrentUpdateXmlHelper.GetObjectLogEntryWithoutLinkedType(historydoc, recordGuid);
        if (!withoutLinkedType.Any<XmlElement>())
          return;
        loanHistories.TrackChange(withoutLinkedType);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Update Enhance Condition in ConditionsMerge. Loan = '" + loanGUID + "', ConditionGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    public static void DeleteAppliedMergeUpdatesForLoan(XmlDocument loan, string systemId)
    {
      XmlElement documentElement = loan.DocumentElement;
      long updateSequenceNum = ConcurrentUpdateXmlHelper.GetMergeUpdateSequenceNum(documentElement, loan, systemId);
      string loanGuid = ConcurrentUpdateXmlHelper.getLoanGuid(documentElement, loan);
      if (updateSequenceNum < 0L)
        return;
      List<long> sequenceNumberList = new List<long>();
      foreach (ConcurrentUpdateModel concurrentUpdateModel in ConcurrentUpdateAccessor.GetConcurrentUpdatesByLoanId(loanGuid).OrderBy<ConcurrentUpdateModel, long>((Func<ConcurrentUpdateModel, long>) (o => o.SequenceNumber)).ToList<ConcurrentUpdateModel>())
      {
        if (concurrentUpdateModel.SequenceNumber <= updateSequenceNum)
          sequenceNumberList.Add(concurrentUpdateModel.SequenceNumber);
      }
      if (sequenceNumberList.Count > 0)
        ConcurrentUpdateAccessor.DeleteAllConcurrentUpdates(loanGuid, sequenceNumberList);
      ConcurrentUpdateXmlHelper.SetMergeUpdateSequenceNum(-1L, documentElement, loan, systemId);
    }

    private static void createDocument(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string guid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlStr);
      string attribute = xmlDocument.DocumentElement.GetAttribute("Guid");
      try
      {
        if (ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, attribute) != null)
          return;
        XmlDocumentFragment documentFragment = xmlDoc.CreateDocumentFragment();
        documentFragment.InnerXml = xmlStr;
        ConcurrentUpdateXmlHelper.getCreateDocElement(true, root, xmlDoc, systemId).AppendChild((XmlNode) documentFragment);
        if (!string.IsNullOrEmpty(xmlHistoryStr))
          loanHistories.TrackChange(xmlHistoryStr);
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Created Document in DocumentMerge. Loan = '" + guid + "', DocumentGuid = ''");
      }
      catch
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Create Document in DocumentMerge. Loan = '" + guid + "', DocumentGuid = '" + attribute + "'");
      }
    }

    private static void assignAttachments(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      try
      {
        Dictionary<string, string> attachmentIdsWithFlag = ConcurrentUpdateXmlHelper.getAttachmentIDsWithFlag(doc, documentElement);
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        XmlElement e = (XmlElement) editDocElement.SelectSingleNode("Files");
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        FileAttachment[] fileAttachments = ConcurrentUpdateManager.GetFileAttachments(loanGuid);
        XmlDocument xmlDocument = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        if (e == null)
        {
          e = (XmlElement) editDocElement.PrependChild((XmlNode) xmlDoc.CreateElement("Files"));
          e.SetAttribute("IsMigrated", "Y");
          foreach (KeyValuePair<string, string> keyValuePair in attachmentIdsWithFlag)
          {
            if (ConcurrentUpdateManager.CheckAttachmentExist(keyValuePair.Key, fileAttachments) && !ConcurrentUpdateXmlHelper.attachmentsAlreadyAssignedToDocument(keyValuePair.Key, root, systemId))
            {
              dictionary.Add(keyValuePair.Key, keyValuePair.Value);
              XmlElement logEntry = ConcurrentUpdateXmlHelper.getLogEntry(xmlDocument, keyValuePair.Key);
              if (logEntry != null)
                loanHistories.TrackChange(logEntry);
            }
          }
        }
        else
        {
          foreach (KeyValuePair<string, string> keyValuePair in attachmentIdsWithFlag)
          {
            if (e.SelectSingleNode("Reference[@AttachmentID='" + keyValuePair.Key + "']") == null && ConcurrentUpdateManager.CheckAttachmentExist(keyValuePair.Key, fileAttachments) && !ConcurrentUpdateXmlHelper.attachmentsAlreadyAssignedToDocument(keyValuePair.Key, root, systemId))
            {
              dictionary.Add(keyValuePair.Key, keyValuePair.Value);
              XmlElement logEntry = ConcurrentUpdateXmlHelper.getLogEntry(xmlDocument, keyValuePair.Key);
              if (logEntry != null)
                loanHistories.TrackChange(logEntry);
            }
          }
        }
        if (dictionary.Any<KeyValuePair<string, string>>())
        {
          ConcurrentUpdateXmlHelper.addAttachmentsToDoc(e, xmlDoc, dictionary, systemId);
          ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "LastAttachmentDate");
          ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
          if (ConcurrentUpdateXmlHelper.CheckForActiveAtachment(root, systemId, recordGuid))
            ConcurrentUpdateManager.SetReceived(root, systemId, doc, documentElement, recordGuid, editDocElement, xmlDocument, loanHistories);
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Assigned Attachment in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Assign Attachment in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void linkConditions(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      try
      {
        List<string> conditions = ConcurrentUpdateXmlHelper.getConditions(doc, documentElement);
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        List<string> stringList = new List<string>();
        XmlElement e = (XmlElement) editDocElement.SelectSingleNode("Conditions");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        if (e == null)
        {
          e = (XmlElement) editDocElement.PrependChild((XmlNode) xmlDoc.CreateElement("Conditions"));
          foreach (string str in conditions)
          {
            if (ConcurrentUpdateXmlHelper.getEditConditionElement(root, systemId, str) != null)
            {
              stringList.Add(str);
              XmlElement objectLogEntry = ConcurrentUpdateXmlHelper.getObjectLogEntry(historydoc, str);
              if (objectLogEntry != null)
                loanHistories.TrackChange(objectLogEntry);
            }
          }
        }
        else if (conditions != null)
        {
          foreach (string str in conditions)
          {
            XmlElement conditionElement = ConcurrentUpdateXmlHelper.getEditConditionElement(root, systemId, str);
            if (e.SelectSingleNode("ref[@id='" + str + "']") == null && conditionElement != null)
            {
              stringList.Add(str);
              XmlElement objectLogEntry = ConcurrentUpdateXmlHelper.getObjectLogEntry(historydoc, str);
              if (objectLogEntry != null)
                loanHistories.TrackChange(objectLogEntry);
            }
          }
        }
        if (stringList.Any<string>())
        {
          ConcurrentUpdateXmlHelper.addConditionsToDoc(e, xmlDoc, stringList, systemId);
          ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Linked Condition in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Link Condition in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void AddCommentsToRecord(
      XmlElement loanRootElement,
      XmlDocument loanXmlDoc,
      string stagedXml,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(stagedXml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(xmlDocument, documentElement);
      try
      {
        XmlNode xmlNode = documentElement.SelectSingleNode("Comments");
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(loanRootElement, systemId, recordGuid);
        if (editDocElement == null)
          return;
        List<string> stringList = new List<string>();
        XmlElement xmlElement = (XmlElement) editDocElement.SelectSingleNode("Comments");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        if (xmlElement == null)
          xmlElement = (XmlElement) editDocElement.PrependChild((XmlNode) loanXmlDoc.CreateElement("Comments"));
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          string commentId = childNode.Attributes["Guid"].Value;
          if (xmlElement?.SelectSingleNode("Entry[@Guid='" + commentId + "']") == null)
          {
            XmlNode newChild = xmlElement.OwnerDocument.ImportNode(childNode, true);
            xmlElement.AppendChild(newChild);
            List<XmlElement> entriesForComment = ConcurrentUpdateXmlHelper.GetHistoryEntriesForComment(historydoc, childNode.Attributes["Comments"].Value, commentId);
            if (entriesForComment != null && entriesForComment.Any<XmlElement>())
            {
              foreach (XmlElement xmlHistoryElement in entriesForComment)
                loanHistories.TrackChange(xmlHistoryElement);
            }
            ConcurrentUpdateManager.SetUpdatedDateAndUpdatedBy(loanRootElement, systemId, xmlDocument, documentElement, recordGuid, editDocElement);
            TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Added comment to Record in RecordMerge. Loan = '" + loanGuid + "', RecordGuid = '" + recordGuid + "', CommentGuid = '" + commentId + "'");
          }
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to add comments to Record in RecordMerge. Loan = '" + loanGuid + "', RecordGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void UpdateCommentsToRecord(
      XmlElement loanRootElement,
      XmlDocument loanXmlDoc,
      string stagedXml,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(stagedXml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(xmlDocument, documentElement);
      try
      {
        XmlNode xmlNode = documentElement.SelectSingleNode("Comments");
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(loanRootElement, systemId, recordGuid);
        if (editDocElement == null)
          return;
        XmlElement xmlElement = (XmlElement) editDocElement.SelectSingleNode("Comments");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        if (xmlElement == null)
          return;
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          string linkedObjectId = childNode.Attributes["Guid"].Value;
          XmlNode oldChild = xmlElement.SelectSingleNode("Entry[@Guid='" + linkedObjectId + "']");
          if (oldChild != null)
          {
            XmlNode newChild = xmlElement.OwnerDocument.ImportNode(childNode, true);
            xmlElement.ReplaceChild(newChild, oldChild);
            XmlElement logEntry = ConcurrentUpdateXmlHelper.getLogEntry(historydoc, linkedObjectId);
            loanHistories.TrackChange(logEntry);
            ConcurrentUpdateManager.SetUpdatedDateAndUpdatedBy(loanRootElement, systemId, xmlDocument, documentElement, recordGuid, editDocElement);
            TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Updated comment to Record in RecordMerge. Loan = '" + loanGuid + "', RecordGuid = '" + recordGuid + "', CommentGuid = '" + linkedObjectId + "'");
          }
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to update comments to Record in RecordMerge. Loan = '" + loanGuid + "', RecordGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void DeleteCommentsFromRecord(
      XmlElement loanRootElement,
      XmlDocument loanXmlDoc,
      string stagedXml,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(stagedXml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(xmlDocument, documentElement);
      try
      {
        XmlNode xmlNode = documentElement.SelectSingleNode("Comments");
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(loanRootElement, systemId, recordGuid);
        if (editDocElement == null)
          return;
        XmlElement xmlElement = (XmlElement) editDocElement.SelectSingleNode("Comments");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        if (xmlElement == null)
          return;
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          string str = childNode.Attributes["Guid"].Value;
          XmlNode oldChild = xmlElement.SelectSingleNode("Entry[@Guid='" + str + "']");
          if (oldChild != null)
          {
            xmlElement.RemoveChild(oldChild);
            List<XmlElement> entriesForComment = ConcurrentUpdateXmlHelper.GetHistoryEntriesForComment(historydoc, childNode.Attributes["Comments"].Value, (string) null);
            loanHistories.TrackChange(entriesForComment);
            ConcurrentUpdateManager.SetUpdatedDateAndUpdatedBy(loanRootElement, systemId, xmlDocument, documentElement, recordGuid, editDocElement);
            TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Deleted comment to Record in RecordMerge. Loan = '" + loanGuid + "', RecordGuid = '" + recordGuid + "', CommentGuid = '" + str + "'");
          }
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to delete comments from Record in RecordMerge. Loan = '" + loanGuid + "', RecordGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static XmlDocument LoadHistoryXml(string xmlHistoryStr)
    {
      XmlDocument xmlDocument = new XmlDocument();
      if (!string.IsNullOrEmpty(xmlHistoryStr))
        xmlDocument.LoadXml(xmlHistoryStr);
      return xmlDocument;
    }

    private static void unLinkConditions(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      List<string> conditions = ConcurrentUpdateXmlHelper.getConditions(doc, documentElement);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        foreach (string str in conditions)
        {
          ConcurrentUpdateXmlHelper.removeConditionFromDocument(str, recordGuid, root, systemId);
          XmlElement objectLogEntry = ConcurrentUpdateXmlHelper.getObjectLogEntry(historydoc, str);
          if (objectLogEntry != null)
            loanHistories.TrackChange(objectLogEntry);
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Unlink Condition from in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Unlink Condition from in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void updateAssignedAttachments(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGUID,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      List<string> attachmentIds = ConcurrentUpdateXmlHelper.getAttachmentIDs(doc, documentElement);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        FileAttachment[] fileAttachments = ConcurrentUpdateManager.GetFileAttachments(loanGUID);
        XmlDocument xmlDocument = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        foreach (string str in attachmentIds)
        {
          if (ConcurrentUpdateManager.CheckAttachmentExist(str, fileAttachments))
          {
            string attribute = ((XmlElement) documentElement.SelectSingleNode("Files/Reference[@AttachmentID='" + str + "']")).GetAttribute("IsActive");
            ConcurrentUpdateXmlHelper.EditDocAttachmentElement(root, xmlDoc, systemId, recordGuid, str, attribute);
            XmlElement logEntry = ConcurrentUpdateXmlHelper.getLogEntry(xmlDocument, str);
            if (logEntry != null)
              loanHistories.TrackChange(logEntry);
          }
          if (!ConcurrentUpdateXmlHelper.CheckForActiveAtachment(root, systemId, recordGuid))
          {
            editDocElement.RemoveAttribute("ReceiveDate");
            editDocElement.RemoveAttribute("ReceivedBy");
            List<XmlElement> withoutLinkedType = ConcurrentUpdateXmlHelper.GetObjectLogEntryWithoutLinkedType(xmlDocument, recordGuid);
            if (withoutLinkedType.Any<XmlElement>())
              loanHistories.TrackChange(withoutLinkedType);
          }
          else
            ConcurrentUpdateManager.SetReceived(root, systemId, doc, documentElement, recordGuid, editDocElement, xmlDocument, loanHistories);
        }
        ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Updated Attachmnet in DocumentMerge. Loan = '" + loanGUID + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to update attachment in DocumentMerge. Loan = '" + loanGUID + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void unassignAttachments(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      List<string> attachmentIds = ConcurrentUpdateXmlHelper.getAttachmentIDs(doc, documentElement);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        foreach (string str in attachmentIds)
        {
          if (ConcurrentUpdateXmlHelper.removeAttachmentFromDocument(str, recordGuid, root, systemId))
          {
            XmlElement logEntry = ConcurrentUpdateXmlHelper.getLogEntry(historydoc, str);
            if (logEntry != null)
              loanHistories.TrackChange(logEntry);
          }
        }
        if (!ConcurrentUpdateXmlHelper.CheckForActiveAtachment(root, systemId, recordGuid))
        {
          editDocElement.RemoveAttribute("ReceiveDate");
          editDocElement.RemoveAttribute("ReceivedBy");
          List<XmlElement> withoutLinkedType = ConcurrentUpdateXmlHelper.GetObjectLogEntryWithoutLinkedType(historydoc, recordGuid);
          if (withoutLinkedType.Any<XmlElement>())
            loanHistories.TrackChange(withoutLinkedType);
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "UnAssigned Attachmnet from in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to UnAssigned Attachmnet from in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void updateDocuments(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement != null)
        {
          foreach (XmlAttribute attribute1 in (XmlNamedNodeMap) documentElement.Attributes)
          {
            switch (attribute1.Name)
            {
              case "Type":
              case "Guid":
              case "LastAttachmentDate":
                continue;
              case "UpdatedDate":
                ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
                continue;
              default:
                string attribute2 = documentElement.GetAttribute(attribute1.Name);
                editDocElement.SetAttribute(attribute1.Name, attribute2);
                continue;
            }
          }
          if (!string.IsNullOrEmpty(xmlHistoryStr))
            loanHistories.TrackChange(xmlHistoryStr);
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Updated Document in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Update Document in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void addAllowedRoles(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      try
      {
        List<string> allowedRoles = ConcurrentUpdateXmlHelper.getAllowedRoles(doc, documentElement);
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        List<string> stringList = new List<string>();
        XmlElement e = (XmlElement) editDocElement.SelectSingleNode("AllowedRoles");
        if (e == null)
        {
          e = (XmlElement) editDocElement.PrependChild((XmlNode) xmlDoc.CreateElement("AllowedRoles"));
          foreach (string str in allowedRoles)
            stringList.Add(str);
        }
        else
        {
          foreach (string str in allowedRoles)
          {
            if (e.SelectSingleNode("ref[@id='" + str + "']") == null)
              stringList.Add(str);
          }
        }
        if (stringList.Any<string>())
        {
          ConcurrentUpdateXmlHelper.addRolesConditionsToDoc(e, xmlDoc, stringList, systemId);
          ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Add role in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Add Role in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void removeAllowedRoles(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      List<string> allowedRoles = ConcurrentUpdateXmlHelper.getAllowedRoles(doc, documentElement);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, recordGuid, editDocElement, "UpdatedDate");
        foreach (string role in allowedRoles)
          ConcurrentUpdateXmlHelper.removeAllowedRoleFromDocument(role, recordGuid, root, systemId);
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Remove Role from in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Remove Role in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void RemoveEnhanceConditionTrackings(
      XmlElement loanRootElement,
      XmlDocument loanXmlDoc,
      string stageXml,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(stageXml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(xmlDocument, documentElement);
      try
      {
        XmlNode xmlNode = documentElement.SelectSingleNode("Trackings");
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(loanRootElement, systemId, recordGuid);
        if (editDocElement == null)
          return;
        XmlElement xmlElement = (XmlElement) editDocElement.SelectSingleNode("Trackings");
        if (xmlElement == null)
          return;
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          string str = childNode.Attributes["Status"].Value;
          XmlNode oldChild = xmlElement.SelectSingleNode("Entry[@Status='" + str + "']");
          if (oldChild != null)
            xmlElement.RemoveChild(oldChild);
        }
        if (xmlHistoryStr != null)
        {
          loanHistories.TrackChange(xmlHistoryStr);
          ConcurrentUpdateManager.SetUpdatedDateAndUpdatedBy(loanRootElement, systemId, xmlDocument, documentElement, recordGuid, editDocElement);
        }
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Remove Enhance Condition Tracking in ConditionsMerge. Loan = '" + loanGuid + "', ConditionGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Remove Enhance Condition Tracking in ConditionsMerge. Loan = '" + loanGuid + "', ConditionGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void AddEnhanceConditionTrackings(
      XmlElement loanRootElement,
      XmlDocument loanXmlDoc,
      string stageXml,
      string systemId,
      string loanGuid,
      string xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(stageXml);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(xmlDocument, documentElement);
      try
      {
        XmlNode xmlNode = documentElement.SelectSingleNode("Trackings");
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(loanRootElement, systemId, recordGuid);
        if (editDocElement == null)
          return;
        XmlElement xmlElement = (XmlElement) editDocElement.SelectSingleNode("Trackings");
        XmlDocument historydoc = ConcurrentUpdateManager.LoadHistoryXml(xmlHistoryStr);
        if (xmlElement == null)
          xmlElement = (XmlElement) editDocElement.PrependChild((XmlNode) loanXmlDoc.CreateElement("Trackings"));
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          XmlNode newChild = xmlElement.OwnerDocument.ImportNode(childNode, true);
          string str = childNode.Attributes["Status"].Value;
          XmlNode oldChild = xmlElement.SelectSingleNode("Entry[@Status='" + str + "']");
          if (oldChild != null)
          {
            DateTime dateTime = Convert.ToDateTime(oldChild.Attributes["Date"].Value);
            if (Convert.ToDateTime(newChild.Attributes["Date"].Value) > dateTime)
              xmlElement.ReplaceChild(newChild, oldChild);
          }
          else
            xmlElement.AppendChild(newChild);
          List<XmlElement> conditionTrackings = ConcurrentUpdateXmlHelper.GetHistoryEntriesForAddEnhanceConditionTrackings(historydoc, childNode.Attributes["Status"].Value);
          if (conditionTrackings != null && conditionTrackings.Any<XmlElement>())
          {
            foreach (XmlElement xmlHistoryElement in conditionTrackings)
              loanHistories.TrackChange(xmlHistoryElement);
          }
          ConcurrentUpdateManager.SetUpdatedDateAndUpdatedBy(loanRootElement, systemId, xmlDocument, documentElement, recordGuid, editDocElement);
          TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Add Enhance Condition Tracking in ConditionsMerge. Loan = '" + loanGuid + "', ConditionGuid = '" + recordGuid + "'");
        }
        List<XmlElement> xmlElementList = new List<XmlElement>();
        List<XmlElement> list = xmlElement.GetElementsByTagName("Entry").OfType<XmlElement>().OrderBy<XmlElement, string>((Func<XmlElement, string>) (item => item.GetAttribute("Date"))).ToList<XmlElement>();
        if (list == null || list.Count <= 0)
          return;
        xmlElement.RemoveAll();
        foreach (XmlElement newChild in list)
          xmlElement.AppendChild((XmlNode) newChild);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Add Enhance Condition Tracking in ConditionsMerge. Loan = '" + loanGuid + "', ConditionGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static void SetReceivedStatus(
      XmlElement root,
      XmlDocument xmlDoc,
      string xmlStr,
      string systemId,
      string loanGuid)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(xmlStr);
      XmlElement documentElement = doc.DocumentElement;
      string recordGuid = ConcurrentUpdateXmlHelper.getRecordGuid(doc, documentElement);
      try
      {
        XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, recordGuid);
        if (editDocElement == null)
          return;
        editDocElement.SetAttribute("ReceiveDate", ConcurrentUpdateXmlHelper.getPropertyValue(documentElement, "ReceiveDate"));
        editDocElement.SetAttribute("ReceivedBy", ConcurrentUpdateXmlHelper.getPropertyValue(documentElement, "ReceivedBy"));
        TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "SetReceivedDetails from in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "'");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Set Received Details in DocumentMerge. Loan = '" + loanGuid + "', DocumentGuid = '" + recordGuid + "' Exception='" + ex.Message + "'");
      }
    }

    private static bool CheckAttachmentExist(string attachmentId, FileAttachment[] fileAttachments)
    {
      return ((IEnumerable<FileAttachment>) fileAttachments).Any<FileAttachment>() && ((IEnumerable<FileAttachment>) fileAttachments).FirstOrDefault<FileAttachment>((Func<FileAttachment, bool>) (attachment => attachment.ID == attachmentId && !attachment.IsRemoved)) != null;
    }

    private static FileAttachment[] GetFileAttachments(string loanGuid)
    {
      return LoanStore.GetLatestVersion(loanGuid).GetFileAttachments();
    }

    private static void SetLatestDate(
      XmlElement root,
      string systemId,
      XmlDocument doc,
      XmlElement docRoot,
      string docGuid,
      XmlElement e,
      string propertyName)
    {
      string propertyValue = ConcurrentUpdateXmlHelper.getPropertyValue(docRoot, propertyName);
      string propertyValueFromLoan = ConcurrentUpdateXmlHelper.getPropertyValueFromLoan(root, systemId, docGuid, propertyName);
      DateTime result1;
      DateTime result2;
      if (!string.IsNullOrEmpty(propertyValueFromLoan) && (!DateTime.TryParse(propertyValue, out result1) || !DateTime.TryParse(propertyValueFromLoan, out result2) || !(result2 < result1)))
        return;
      e.SetAttribute(propertyName, propertyValue);
    }

    private static void SetReceived(
      XmlElement root,
      string systemId,
      XmlDocument doc,
      XmlElement docRoot,
      string docGuid,
      XmlElement e,
      XmlDocument xmlHistoryStr,
      IStageLoanHistoryManager loanHistories)
    {
      string propertyValue = ConcurrentUpdateXmlHelper.getPropertyValue(docRoot, "ReceiveDate");
      if (!string.IsNullOrEmpty(ConcurrentUpdateXmlHelper.getPropertyValueFromLoan(root, systemId, docGuid, "ReceiveDate")) || string.IsNullOrEmpty(propertyValue))
        return;
      e.SetAttribute("ReceiveDate", propertyValue);
      e.SetAttribute("ReceivedBy", ConcurrentUpdateXmlHelper.getPropertyValue(docRoot, "ReceivedBy"));
      List<XmlElement> withoutLinkedType = ConcurrentUpdateXmlHelper.GetObjectLogEntryWithoutLinkedType(xmlHistoryStr, docGuid);
      if (!withoutLinkedType.Any<XmlElement>())
        return;
      loanHistories.TrackChange(withoutLinkedType);
    }

    private static void SetUpdatedDateAndUpdatedBy(
      XmlElement loanRootElement,
      string systemId,
      XmlDocument stagedXmlDocument,
      XmlElement stagedRootElement,
      string recordGuid,
      XmlElement logRecordElement)
    {
      string propertyValue1 = ConcurrentUpdateXmlHelper.getPropertyValue(stagedRootElement, "UpdatedDate");
      string propertyValue2 = ConcurrentUpdateXmlHelper.getPropertyValue(stagedRootElement, "UpdatedBy");
      string propertyValueFromLoan = ConcurrentUpdateXmlHelper.getPropertyValueFromLoan(loanRootElement, systemId, recordGuid, "UpdatedDate");
      DateTime result1;
      DateTime result2;
      if (!string.IsNullOrEmpty(propertyValueFromLoan) && (!DateTime.TryParse(propertyValue1, out result1) || !DateTime.TryParse(propertyValueFromLoan, out result2) || !(result2 < result1)))
        return;
      logRecordElement.SetAttribute("UpdatedDate", propertyValue1);
      logRecordElement.SetAttribute("UpdatedBy", propertyValue2);
    }

    private static void DeleteDocument(
      XmlElement root,
      XmlDocument xmlDoc,
      string dataXml,
      string systemId,
      string guid,
      string historyXml,
      IStageLoanHistoryManager loanHistories,
      long seqNumber,
      Dictionary<string, object> mergeParamKeyValues)
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml(dataXml);
      XmlElement documentElement = doc.DocumentElement;
      string attribute1 = documentElement.GetAttribute("Guid");
      if (mergeParamKeyValues.ContainsKey("removeOnlyIfEmpty") && mergeParamKeyValues.ContainsValue((object) "T") && ConcurrentUpdateXmlHelper.CheckForActiveAtachment(root, systemId, attribute1))
      {
        ConcurrentUpdateAccessor.DeleteConcurrentUpdates(guid, seqNumber);
        TraceLog.WriteWarning(nameof (ConcurrentUpdateManager), "Could not remove documents. Following document have attachments assigned to them: Loan = '" + guid + "', DocumentGuid = '" + attribute1 + "'");
      }
      else
      {
        try
        {
          XmlElement editDocElement = ConcurrentUpdateXmlHelper.getEditDocElement(root, systemId, attribute1);
          if (editDocElement == null)
          {
            TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Document is not found while DocumentMerge. Loan = '" + guid + "', DocumentGuid = '" + attribute1 + "'");
          }
          else
          {
            foreach (XmlAttribute attribute2 in (XmlNamedNodeMap) documentElement.Attributes)
            {
              if (attribute2.Name == "UpdatedDate")
              {
                ConcurrentUpdateManager.SetLatestDate(root, systemId, doc, documentElement, attribute1, editDocElement, "UpdatedDate");
              }
              else
              {
                string attribute3 = documentElement.GetAttribute(attribute2.Name);
                editDocElement.SetAttribute(attribute2.Name, attribute3);
              }
            }
            XmlNodeList elementsByTagName = root.GetElementsByTagName("Removed");
            (elementsByTagName == null || elementsByTagName.Count == 0 ? root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']").AppendChild((XmlNode) xmlDoc.CreateElement("Removed")) : root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Removed")).AppendChild((XmlNode) editDocElement);
            if (!string.IsNullOrEmpty(historyXml))
              loanHistories.TrackChange(historyXml);
            TraceLog.WriteVerbose(nameof (ConcurrentUpdateManager), "Remove Document in DocumentMerge. Loan = '" + guid + "', DocumentGuid = '" + attribute1 + "'");
          }
        }
        catch
        {
          TraceLog.WriteError(nameof (ConcurrentUpdateManager), "Failed to Remove Document in DocumentMerge. Loan = '" + guid + "', DocumentGuid = '" + attribute1 + "'");
        }
      }
    }
  }
}
