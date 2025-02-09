// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.CommentEntryCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class CommentEntryCollection : CollectionBase
  {
    private LogRecordBase logRecord;

    public CommentEntryCollection(LogRecordBase logRecord) => this.logRecord = logRecord;

    public CommentEntryCollection(
      LogRecordBase logRecord,
      XmlElement parentElement,
      string groupName)
    {
      this.logRecord = logRecord;
      XmlElement xmlElement = (XmlElement) parentElement.SelectSingleNode(groupName);
      if (xmlElement == null)
        return;
      foreach (XmlElement selectNode in xmlElement.SelectNodes("Entry"))
        this.List.Add((object) new CommentEntry(logRecord, selectNode));
    }

    public CommentEntry this[int index] => (CommentEntry) this.List[index];

    public int Add(CommentEntry entry)
    {
      if (this.List.Contains((object) entry))
        return this.List.IndexOf((object) entry);
      int num = this.List.Add((object) entry);
      entry.AttachToLogEntry(this.logRecord);
      string str = "Comment added";
      if (entry.ForRoleID != -1)
      {
        str += " - Update Alert sent";
        if (this.logRecord.IsAttachedToLog)
        {
          RoleInfo role = this.logRecord.Log.Loan.Settings.GetRole(entry.ForRoleID);
          if (role != null)
            str = str + " to " + role.RoleAbbr;
        }
      }
      this.logRecord.TrackChange(str + " \"" + entry.Comments + "\"");
      return num;
    }

    public CommentEntry Add(string comments, string addedBy, string addedByName)
    {
      CommentEntry entry = new CommentEntry(comments, addedBy, addedByName);
      this.Add(entry);
      return entry;
    }

    public CommentEntry Add(string comments, string addedBy, string addedByName, bool isInternal)
    {
      CommentEntry entry = new CommentEntry(comments, addedBy, addedByName, isInternal);
      this.Add(entry);
      return entry;
    }

    public CommentEntry Add(
      string comments,
      string addedBy,
      string addedByName,
      bool isInternal,
      int alertRoleID)
    {
      CommentEntry entry = new CommentEntry(comments, addedBy, addedByName, isInternal, alertRoleID);
      this.Add(entry);
      return entry;
    }

    public bool Contains(CommentEntry entry) => this.List.Contains((object) entry);

    public void Remove(CommentEntry entry)
    {
      if (!this.List.Contains((object) entry))
        return;
      this.List.Remove((object) entry);
      this.logRecord.TrackChange("Comment deleted \"" + entry.Comments + "\"");
    }

    public bool HasUnreviewedEntry(int[] roleList)
    {
      foreach (CommentEntry commentEntry in (IEnumerable) this.List)
      {
        if (Array.IndexOf<int>(roleList, commentEntry.ForRoleID) >= 0 && !commentEntry.Reviewed)
          return true;
      }
      return false;
    }

    public void MarkAsReviewed(DateTime date, string user, int[] roleList)
    {
      foreach (CommentEntry commentEntry in (IEnumerable) this.List)
      {
        if (Array.IndexOf<int>(roleList, commentEntry.ForRoleID) >= 0 && !commentEntry.Reviewed)
          commentEntry.MarkAsReviewed(date, user);
      }
    }

    public PipelineInfo.Alert[] GetPipelineAlerts()
    {
      System.Collections.Generic.List<PipelineInfo.Alert> alertList = new System.Collections.Generic.List<PipelineInfo.Alert>();
      foreach (CommentEntry commentEntry in (IEnumerable) this.List)
      {
        if (commentEntry.ForRoleID != -1 && !commentEntry.Reviewed)
        {
          System.Collections.Generic.List<string> stringList1 = new System.Collections.Generic.List<string>();
          System.Collections.Generic.List<string> stringList2 = new System.Collections.Generic.List<string>();
          foreach (LoanAssociateLog assignedAssociate in this.logRecord.Log.GetAssignedAssociates(commentEntry.ForRoleID))
          {
            PipelineInfo.Alert alert = (PipelineInfo.Alert) null;
            int forRoleId;
            if (assignedAssociate.LoanAssociateType == LoanAssociateType.User && !stringList1.Contains(assignedAssociate.LoanAssociateID))
            {
              string comments = commentEntry.Comments;
              forRoleId = commentEntry.ForRoleID;
              string status = forRoleId.ToString();
              DateTime today = DateTime.Today;
              string loanAssociateId = assignedAssociate.LoanAssociateID;
              string guid1 = commentEntry.Guid;
              string guid2 = this.logRecord.Guid;
              alert = new PipelineInfo.Alert(30, comments, status, today, loanAssociateId, -1, guid1, guid2);
              stringList1.Add(assignedAssociate.LoanAssociateID);
            }
            else if (assignedAssociate.LoanAssociateType == LoanAssociateType.Group && !stringList2.Contains(assignedAssociate.LoanAssociateID))
            {
              string comments = commentEntry.Comments;
              forRoleId = commentEntry.ForRoleID;
              string status = forRoleId.ToString();
              DateTime today = DateTime.Today;
              int groupId = Utils.ParseInt((object) assignedAssociate.LoanAssociateID);
              string guid3 = commentEntry.Guid;
              string guid4 = this.logRecord.Guid;
              alert = new PipelineInfo.Alert(30, comments, status, today, (string) null, groupId, guid3, guid4);
              stringList2.Add(assignedAssociate.LoanAssociateID);
            }
            if (alert != null)
              alertList.Add(alert);
          }
        }
      }
      return alertList.ToArray();
    }

    public void ToXml(XmlElement e, string groupName)
    {
      if (this.List.Count == 0)
        return;
      XmlElement element1 = e.OwnerDocument.CreateElement(groupName);
      e.AppendChild((XmlNode) element1);
      foreach (CommentEntry commentEntry in (IEnumerable) this.List)
      {
        XmlElement element2 = e.OwnerDocument.CreateElement("Entry");
        element1.AppendChild((XmlNode) element2);
        commentEntry.ToXml(element2);
      }
    }

    public override string ToString()
    {
      string str = string.Empty;
      foreach (CommentEntry commentEntry in (IEnumerable) this.List)
        str = !(str == string.Empty) ? str + "\r\n\r\n" + commentEntry.ToString() : commentEntry.ToString();
      return str;
    }
  }
}
