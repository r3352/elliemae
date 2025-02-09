// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ConcurrentUpdateXmlHelper
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class ConcurrentUpdateXmlHelper
  {
    public static XmlElement getMergeUpdateElement(
      bool createIfMissing,
      XmlElement root,
      XmlDocument xmldoc,
      string systemId)
    {
      XmlElement mergeUpdateElement = (XmlElement) root.SelectSingleNode("EllieMae/MergeUpdate[@SysID='" + systemId + "']");
      if (mergeUpdateElement == null)
      {
        if (!createIfMissing)
          return (XmlElement) null;
        XmlElement xmlElement = (XmlElement) root.SelectSingleNode("EllieMae");
        mergeUpdateElement = (XmlElement) xmlElement.SelectSingleNode("MergeUpdate") ?? (XmlElement) xmlElement.AppendChild((XmlNode) xmldoc.CreateElement("MergeUpdate"));
        mergeUpdateElement.SetAttribute("SysID", systemId);
      }
      return mergeUpdateElement;
    }

    public static void SetMergeUpdateSequenceNum(
      long sequenceNumber,
      XmlElement root,
      XmlDocument xmldoc,
      string systemId)
    {
      ConcurrentUpdateXmlHelper.getMergeUpdateElement(true, root, xmldoc, systemId).SetAttribute("SequenceNumber", sequenceNumber.ToString());
    }

    public static long GetMergeUpdateSequenceNum(
      XmlElement root,
      XmlDocument xmldoc,
      string systemId)
    {
      XmlElement mergeUpdateElement = ConcurrentUpdateXmlHelper.getMergeUpdateElement(false, root, xmldoc, systemId);
      if (mergeUpdateElement == null)
        return -1;
      try
      {
        return long.Parse(mergeUpdateElement.GetAttribute("SequenceNumber"));
      }
      catch
      {
        return -1;
      }
    }

    public static string getLoanGuid(XmlElement root, XmlDocument xmldoc)
    {
      XmlElement xmlElement = (XmlElement) root.SelectSingleNode("EllieMae");
      try
      {
        return xmlElement.GetAttribute("GUID");
      }
      catch
      {
        return "";
      }
    }

    public static void addAttachmentsToDoc(
      XmlElement e,
      XmlDocument xmlDoc,
      Dictionary<string, string> attachmentList,
      string systemId)
    {
      foreach (KeyValuePair<string, string> attachment in attachmentList)
      {
        XmlElement xmlElement = (XmlElement) e.AppendChild((XmlNode) xmlDoc.CreateElement("Reference"));
        xmlElement.SetAttribute("AttachmentID", attachment.Key);
        xmlElement.SetAttribute("IsActive", attachment.Value);
      }
    }

    public static void addConditionsToDoc(
      XmlElement e,
      XmlDocument xmlDoc,
      List<string> conditionList,
      string systemId)
    {
      foreach (string condition in conditionList)
        ((XmlElement) e.AppendChild((XmlNode) xmlDoc.CreateElement("ref"))).SetAttribute("id", condition);
    }

    public static void addRolesConditionsToDoc(
      XmlElement e,
      XmlDocument xmlDoc,
      List<string> roleList,
      string systemId)
    {
      foreach (string role in roleList)
        ((XmlElement) e.AppendChild((XmlNode) xmlDoc.CreateElement("ref"))).SetAttribute("id", role);
    }

    public static List<string> getAllowedRoles(XmlDocument doc, XmlElement docRoot)
    {
      List<string> allowedRoles = new List<string>();
      XmlElement xmlElement = (XmlElement) docRoot.SelectSingleNode("AllowedRoles");
      if (xmlElement != null)
      {
        foreach (XmlNode childNode in xmlElement.ChildNodes)
          allowedRoles.Add(childNode.Attributes["id"].Value);
      }
      return allowedRoles;
    }

    public static string getRecordGuid(XmlDocument doc, XmlElement docRoot)
    {
      XmlElement xmlElement = (XmlElement) docRoot.SelectSingleNode("Record");
      try
      {
        return docRoot.GetAttribute("Guid");
      }
      catch
      {
        return "";
      }
    }

    public static List<string> getAttachmentIDs(XmlDocument doc, XmlElement docRoot)
    {
      List<string> attachmentIds = new List<string>();
      XmlElement xmlElement = (XmlElement) docRoot.SelectSingleNode("Files");
      if (xmlElement != null)
      {
        foreach (XmlNode childNode in xmlElement.ChildNodes)
          attachmentIds.Add(childNode.Attributes["AttachmentID"].Value);
      }
      return attachmentIds;
    }

    public static Dictionary<string, string> getAttachmentIDsWithFlag(
      XmlDocument doc,
      XmlElement docRoot)
    {
      Dictionary<string, string> attachmentIdsWithFlag = new Dictionary<string, string>();
      XmlElement xmlElement = (XmlElement) docRoot.SelectSingleNode("Files");
      if (xmlElement != null)
      {
        XmlNodeList childNodes = xmlElement.ChildNodes;
        string str = "Y";
        foreach (XmlNode xmlNode in childNodes)
        {
          str = xmlNode.Attributes["IsActive"] != null ? xmlNode.Attributes["IsActive"].Value : str;
          attachmentIdsWithFlag.Add(xmlNode.Attributes["AttachmentID"].Value, str);
        }
      }
      return attachmentIdsWithFlag;
    }

    public static List<string> getConditions(XmlDocument doc, XmlElement docRoot)
    {
      List<string> conditions = new List<string>();
      XmlElement xmlElement = (XmlElement) docRoot.SelectSingleNode("Conditions");
      if (xmlElement != null)
      {
        foreach (XmlNode childNode in xmlElement.ChildNodes)
          conditions.Add(childNode.Attributes["id"].Value);
      }
      return conditions;
    }

    public static XmlElement getEditDocElement(XmlElement root, string systemId, string guid)
    {
      return (XmlElement) root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + guid + "']");
    }

    public static void EditDocAttachmentElement(
      XmlElement root,
      XmlDocument xmldoc,
      string systemId,
      string guid,
      string attachmentId,
      string isActive)
    {
      ((XmlElement) root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + guid + "']/Files/Reference[@AttachmentID='" + attachmentId + "']"))?.SetAttribute("IsActive", isActive);
    }

    public static bool CheckForActiveAtachment(XmlElement root, string systemId, string guid)
    {
      return (XmlElement) root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + guid + "']/Files/Reference[@IsActive='Y']") != null;
    }

    public static string getPropertyValue(XmlElement docRoot, string attributeName)
    {
      try
      {
        return docRoot.GetAttribute(attributeName);
      }
      catch
      {
        return "";
      }
    }

    public static string getPropertyValueFromLoan(
      XmlElement root,
      string systemId,
      string guid,
      string attributeName)
    {
      XmlElement xmlElement = (XmlElement) root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + guid + "']");
      try
      {
        return xmlElement.GetAttribute(attributeName);
      }
      catch
      {
        return "";
      }
    }

    public static XmlElement getLogEntry(XmlDocument historydoc, string linkedObjectId)
    {
      return (XmlElement) historydoc.SelectSingleNode("HISTORY_STORE/ENTRY[@LinkedObjectID='" + linkedObjectId + "']");
    }

    public static XmlElement getObjectLogEntry(XmlDocument historydoc, string objectID)
    {
      return (XmlElement) historydoc.SelectSingleNode("HISTORY_STORE/ENTRY[@ObjectID='" + objectID + "']");
    }

    public static List<XmlElement> GetHistoryEntriesForComment(
      XmlDocument historydoc,
      string commentText,
      string commentId)
    {
      List<XmlElement> entriesForComment = new List<XmlElement>();
      if (commentText != null)
      {
        entriesForComment.Add((XmlElement) historydoc.SelectSingleNode("HISTORY_STORE//ENTRY[@Details='Comment added \"" + commentText + "\"']"));
        entriesForComment.Add((XmlElement) historydoc.SelectSingleNode("HISTORY_STORE//ENTRY[@Details='Comment deleted \"" + commentText + "\"']"));
      }
      if (commentId != null)
        entriesForComment.Add((XmlElement) historydoc.SelectSingleNode("HISTORY_STORE/ENTRY[@LinkedObjectID='" + commentId + "']"));
      return entriesForComment;
    }

    public static XmlElement getCreateDocElement(
      bool createIfMissing,
      XmlElement root,
      XmlDocument xmldoc,
      string systemId)
    {
      XmlElement createDocElement = (XmlElement) root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']");
      if (createDocElement == null)
      {
        if (!createIfMissing)
          return (XmlElement) null;
        createDocElement = (XmlElement) root.SelectSingleNode("EllieMae").AppendChild((XmlNode) xmldoc.CreateElement("SystemLog"));
        createDocElement.SetAttribute("SysID", systemId);
      }
      return createDocElement;
    }

    public static XmlElement getAddConditionsList(
      XmlElement e,
      XmlDocument xmlDoc,
      List<string> conditionList,
      out List<string> addConditionsList)
    {
      XmlElement addConditionsList1 = (XmlElement) e.SelectSingleNode("Conditions");
      addConditionsList = new List<string>();
      if (addConditionsList1 == null)
      {
        addConditionsList1 = (XmlElement) e.PrependChild((XmlNode) xmlDoc.CreateElement("Conditions"));
        addConditionsList = conditionList;
      }
      else if (conditionList != null)
      {
        foreach (string condition in conditionList)
        {
          if (addConditionsList1.SelectSingleNode("ref[@id='" + condition + "']") == null)
            addConditionsList.Add(condition);
        }
      }
      return addConditionsList1;
    }

    public static bool attachmentsAlreadyAssignedToDocument(
      string attachmentid,
      XmlElement root,
      string systemId)
    {
      return root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Type='Document']/Files/Reference[@AttachmentID='" + attachmentid + "']") != null;
    }

    public static XmlElement getEditConditionElement(XmlElement root, string systemId, string guid)
    {
      return (XmlElement) root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + guid + "']");
    }

    public static bool removeAttachmentFromDocument(
      string attachment,
      string docId,
      XmlElement root,
      string systemId)
    {
      XmlNode oldChild = root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + docId + "']/Files/Reference[@AttachmentID='" + attachment + "']");
      if (oldChild == null)
        return false;
      root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + docId + "']/Files").RemoveChild(oldChild);
      return true;
    }

    public static List<XmlElement> GetObjectLogEntryWithoutLinkedType(
      XmlDocument historydoc,
      string objectID)
    {
      List<XmlElement> withoutLinkedType = new List<XmlElement>();
      foreach (XmlElement selectNode in historydoc.SelectNodes("HISTORY_STORE/ENTRY[@ObjectID='" + objectID + "']"))
      {
        if (selectNode.Attributes != null && selectNode.Attributes["LinkedObjectType"] == null)
          withoutLinkedType.Add(selectNode);
      }
      return withoutLinkedType;
    }

    public static void removeConditionFromDocument(
      string condition,
      string docId,
      XmlElement root,
      string systemId)
    {
      XmlNode oldChild = root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + docId + "']/Conditions/ref[@id='" + condition + "']");
      if (oldChild == null)
        return;
      root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + docId + "']/Conditions").RemoveChild(oldChild);
    }

    public static void removeAllowedRoleFromDocument(
      string role,
      string docId,
      XmlElement root,
      string systemId)
    {
      XmlNode oldChild = root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + docId + "']/AllowedRoles/ref[@id='" + role + "']");
      if (oldChild == null)
        return;
      root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']/Record[@Guid='" + docId + "']/AllowedRoles").RemoveChild(oldChild);
    }

    public static List<XmlElement> GetHistoryEntriesForAddEnhanceConditionTrackings(
      XmlDocument historydoc,
      string status)
    {
      return new List<XmlElement>()
      {
        (XmlElement) historydoc.SelectSingleNode("HISTORY_STORE//ENTRY[@Details='Status marked \"" + status + "\"']")
      };
    }
  }
}
