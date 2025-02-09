// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanHistoryStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanHistoryStore
  {
    public static LoanHistoryEntry[] GetHistory(string filename, string[] objectIDList)
    {
      List<LoanHistoryEntry> loanHistoryEntryList = new List<LoanHistoryEntry>();
      string xml = (string) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(filename))
        xml = !latestVersion.Exists ? "<HISTORY_STORE/>" : latestVersion.GetText();
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xml);
      foreach (XmlElement selectNode in xmlDocument.DocumentElement.SelectNodes("ENTRY"))
      {
        LoanHistoryEntry loanHistoryEntry = new LoanHistoryEntry(selectNode);
        if (objectIDList == null || Array.IndexOf<string>(objectIDList, loanHistoryEntry.ObjectID) >= 0)
          loanHistoryEntryList.Add(loanHistoryEntry);
      }
      return loanHistoryEntryList.ToArray();
    }

    public static void AppendHistory(string filename, LoanHistoryEntry[] entryList)
    {
      long ticks = DateTime.UtcNow.Ticks;
      using (DataFile dataFile = FileStore.CheckOut(filename, MutexAccess.Write))
      {
        string xml = !dataFile.Exists ? "<HISTORY_STORE/>" : dataFile.GetText();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xml);
        foreach (LoanHistoryEntry entry in entryList)
        {
          XmlElement element = xmlDocument.CreateElement("ENTRY");
          xmlDocument.DocumentElement.AppendChild((XmlNode) element);
          entry.ToXml(ticks++, element);
        }
        BinaryObject data = new BinaryObject(xmlDocument.OuterXml, Encoding.ASCII);
        dataFile.CheckIn(data);
      }
    }
  }
}
