// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanHistoryManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanHistoryManager : ILoanHistoryMonitor, IFileHistoryMonitor
  {
    private const string className = "LoanHistoryManager�";
    private static readonly string sw = Tracing.SwDataEngine;
    private LoanDataMgr loanDataMgr;
    private List<string> cachedObjectList;
    private List<LoanHistoryEntry> cachedHistoryList;
    private List<LoanHistoryEntry> pendingHistoryList;

    public LoanHistoryManager(LoanDataMgr loanDataMgr)
    {
      this.loanDataMgr = loanDataMgr;
      this.cachedObjectList = new List<string>();
      this.cachedHistoryList = new List<LoanHistoryEntry>();
      this.pendingHistoryList = new List<LoanHistoryEntry>();
      loanDataMgr.LoanData.AttachLoanHistoryMonitor((ILoanHistoryMonitor) this);
    }

    public LoanHistoryEntry[] GetHistory(string objectID)
    {
      return this.GetHistory(new string[1]{ objectID });
    }

    public LoanHistoryEntry[] GetHistory(string[] objectList)
    {
      this.loadCachedHistory(objectList);
      List<LoanHistoryEntry> loanHistoryEntryList = new List<LoanHistoryEntry>();
      foreach (LoanHistoryEntry cachedHistory in this.cachedHistoryList)
      {
        if (Array.IndexOf<string>(objectList, cachedHistory.ObjectID) >= 0)
          loanHistoryEntryList.Add(cachedHistory);
      }
      return loanHistoryEntryList.ToArray();
    }

    private void loadCachedHistory(string[] objectList)
    {
      List<string> collection = new List<string>();
      foreach (string str in objectList)
      {
        if (!this.cachedObjectList.Contains(str))
          collection.Add(str);
      }
      if (collection.Count <= 0)
        return;
      if (this.loanDataMgr.IsNew())
        this.loanDataMgr.Save();
      LoanHistoryEntry[] loanHistory = this.loanDataMgr.LoanObject.GetLoanHistory(collection.ToArray());
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      foreach (LoanHistoryEntry loanHistoryEntry in loanHistory)
      {
        if (loanHistoryEntry.LinkedObjectType == LinkedObjectType.LogRecord)
        {
          if (logList.GetRecordByID(loanHistoryEntry.LinkedObjectID, true, true) == null)
            continue;
        }
        else if (loanHistoryEntry.LinkedObjectType == LinkedObjectType.FileAttachment && this.loanDataMgr.FileAttachments[loanHistoryEntry.LinkedObjectID, true, true] == null)
          continue;
        this.cachedHistoryList.Add(loanHistoryEntry);
      }
      this.cachedObjectList.AddRange((IEnumerable<string>) collection);
    }

    public void ClearCachedHistory()
    {
      this.cachedObjectList.Clear();
      this.cachedHistoryList.Clear();
    }

    public void SavePendingHistory()
    {
      if (this.pendingHistoryList.Count <= 0)
        return;
      this.loanDataMgr.LoanObject.AppendLoanHistory(this.pendingHistoryList.ToArray());
      foreach (LoanHistoryEntry pendingHistory in this.pendingHistoryList)
      {
        if (this.cachedObjectList.Contains(pendingHistory.ObjectID))
        {
          this.cachedObjectList.Remove(pendingHistory.ObjectID);
          List<LoanHistoryEntry> loanHistoryEntryList = new List<LoanHistoryEntry>();
          foreach (LoanHistoryEntry cachedHistory in this.cachedHistoryList)
          {
            if (cachedHistory.ObjectID == pendingHistory.ObjectID)
              loanHistoryEntryList.Add(cachedHistory);
          }
          foreach (LoanHistoryEntry loanHistoryEntry in loanHistoryEntryList)
            this.cachedHistoryList.Remove(loanHistoryEntry);
        }
      }
      this.pendingHistoryList.Clear();
    }

    public string GetPendingHistory()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<PENDING_HISTORY_STORE/>");
      foreach (LoanHistoryEntry pendingHistory in this.pendingHistoryList)
      {
        XmlElement element = xmlDocument.CreateElement("ENTRY");
        xmlDocument.DocumentElement.AppendChild((XmlNode) element);
        pendingHistory.ToXml(0L, element);
      }
      return xmlDocument.OuterXml;
    }

    public void RestorePendingHistory(string xml)
    {
      this.pendingHistoryList.Clear();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("ENTRY"))
        this.pendingHistoryList.Add(new LoanHistoryEntry(selectNode));
    }

    public void TrackChange(LogRecordBase logEntry, string details)
    {
      this.trackChange(logEntry.Guid, HistoryObjectType.LogRecord, details);
    }

    public void TrackChange(LogRecordBase logEntry, string details, LogRecordBase linkedEntry)
    {
      this.trackChange(logEntry.Guid, HistoryObjectType.LogRecord, details, linkedEntry.Guid, LinkedObjectType.LogRecord);
    }

    public void TrackChange(
      LogRecordBase logEntry,
      string details,
      FileAttachmentReference linkedEntry)
    {
      this.trackChange(logEntry.Guid, HistoryObjectType.LogRecord, details, linkedEntry.AttachmentID, LinkedObjectType.FileAttachment);
    }

    public void TrackChange(FileAttachment file, string details)
    {
      this.trackChange(file.ID, HistoryObjectType.FileAttachment, details);
    }

    public void TrackChange(FileAttachment file, string details, FileAttachment linkedFile)
    {
      this.trackChange(file.ID, HistoryObjectType.FileAttachment, details, linkedFile.ID, LinkedObjectType.FileAttachment);
    }

    public void TrackChange(PageImage page, string details, FileAttachment linkedFile)
    {
      this.trackChange(page.ImageKey, HistoryObjectType.PageImage, details, linkedFile.ID, LinkedObjectType.FileAttachment);
    }

    private void trackChange(string objectID, HistoryObjectType objectType, string details)
    {
      if (string.IsNullOrEmpty(details))
        return;
      this.pendingHistoryList.Add(new LoanHistoryEntry(objectID, objectType, this.loanDataMgr.SessionObjects.UserID, details));
    }

    private void trackChange(
      string objectID,
      HistoryObjectType objectType,
      string details,
      string linkedObjectID,
      LinkedObjectType linkedObjectType)
    {
      if (string.IsNullOrEmpty(details))
        return;
      this.pendingHistoryList.Add(new LoanHistoryEntry(objectID, objectType, this.loanDataMgr.SessionObjects.UserID, details, linkedObjectID, linkedObjectType));
    }
  }
}
