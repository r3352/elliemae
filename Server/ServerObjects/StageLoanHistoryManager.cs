// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StageLoanHistoryManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class StageLoanHistoryManager : IStageLoanHistoryManager
  {
    public List<LoanHistoryEntry> StageLoanHistory { get; private set; }

    public IEnumerable HistoryEntries
    {
      get => (IEnumerable) this.StageLoanHistory.AsEnumerable<LoanHistoryEntry>();
    }

    public StageLoanHistoryManager() => this.StageLoanHistory = new List<LoanHistoryEntry>();

    void IStageLoanHistoryManager.TrackChange(
      string objectID,
      string objectType,
      string details,
      string userId,
      string linkedObjectID,
      string linkedObjectType)
    {
      this.StageLoanHistory.Add(new LoanHistoryEntry(objectID, this.GetHistoryObjectType(objectType), userId, details, linkedObjectID, this.GetLinkedObjectTypee(linkedObjectType)));
    }

    void IStageLoanHistoryManager.TrackChange(string xmlHistoryString)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlHistoryString);
      foreach (XmlElement childNode in xmlDocument.DocumentElement.ChildNodes)
        this.StageLoanHistory.Add(new LoanHistoryEntry(childNode));
    }

    void IStageLoanHistoryManager.TrackChange(XmlElement xmlHistoryElement)
    {
      if (xmlHistoryElement == null)
        return;
      this.StageLoanHistory.Add(new LoanHistoryEntry(xmlHistoryElement));
    }

    void IStageLoanHistoryManager.TrackChange(List<XmlElement> xmlHistoryElements)
    {
      foreach (XmlElement xmlHistoryElement in xmlHistoryElements)
      {
        if (xmlHistoryElement != null)
          this.StageLoanHistory.Add(new LoanHistoryEntry(xmlHistoryElement));
      }
    }

    void IStageLoanHistoryManager.ClearHistoryEntrie()
    {
      if (this.StageLoanHistory == null)
        return;
      this.StageLoanHistory.Clear();
    }

    private HistoryObjectType GetHistoryObjectType(string type)
    {
      HistoryObjectType result = HistoryObjectType.None;
      if (!string.IsNullOrEmpty(type))
        Enum.TryParse<HistoryObjectType>(type, out result);
      return result;
    }

    private LinkedObjectType GetLinkedObjectTypee(string type)
    {
      LinkedObjectType result = LinkedObjectType.None;
      if (!string.IsNullOrEmpty(type))
        Enum.TryParse<LinkedObjectType>(type, out result);
      return result;
    }
  }
}
