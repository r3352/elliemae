// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.NotePhrases
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class NotePhrases
  {
    private const string className = "NotePhrases�";
    private const string notePhrasesRelativePath = "Resource\\NotePhrases.xml�";

    public static string[] GetAllPhrases()
    {
      ClientContext current = ClientContext.GetCurrent();
      XmlDocument xmlDocument = (XmlDocument) null;
      using (current.Cache.Lock(nameof (NotePhrases), LockType.ReadOnly))
        xmlDocument = NotePhrases.readFromDisk(current);
      XmlNodeList xmlNodeList = xmlDocument.SelectNodes("NotePhrases/Phrase");
      string[] allPhrases = new string[xmlNodeList.Count];
      for (int i = 0; i < xmlNodeList.Count; ++i)
        allPhrases[i] = xmlNodeList[i].InnerText;
      return allPhrases;
    }

    public static void Add(string phrase)
    {
      NotePhrases.Add(new string[1]{ phrase });
    }

    public static void Add(string[] phrases)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (NotePhrases)))
      {
        XmlDocument xml = NotePhrases.readFromDisk(current);
        for (int index = 0; index < phrases.Length; ++index)
        {
          if (!((phrases[index] ?? "") == "") && NotePhrases.getPhraseElement(xml, phrases[index]) == null)
          {
            NotePhrases.getRootElement(xml).AppendChild((XmlNode) xml.CreateElement("Phrase")).InnerText = phrases[index];
            TraceLog.WriteVerbose(nameof (NotePhrases), "Note phrase added: " + phrases[index]);
          }
        }
        NotePhrases.writeToDisk(current, xml);
      }
    }

    public static void Remove(string phrase)
    {
      NotePhrases.Remove(new string[1]{ phrase });
    }

    public static void Remove(string[] phrases)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (NotePhrases)))
      {
        XmlDocument xml = NotePhrases.readFromDisk(current);
        for (int index = 0; index < phrases.Length; ++index)
        {
          XmlElement phraseElement = NotePhrases.getPhraseElement(xml, phrases[index]);
          if (phraseElement != null)
          {
            NotePhrases.getRootElement(xml).RemoveChild((XmlNode) phraseElement);
            TraceLog.WriteVerbose(nameof (NotePhrases), "Note phrase removed: " + phrases[index]);
          }
        }
        NotePhrases.writeToDisk(current, xml);
      }
    }

    private static XmlElement getRootElement(XmlDocument xml)
    {
      return (XmlElement) xml.SelectSingleNode(nameof (NotePhrases));
    }

    private static XmlElement getPhraseElement(XmlDocument xml, string phrase)
    {
      XmlNodeList xmlNodeList = xml.SelectNodes("NotePhrases/Phrase");
      for (int i = 0; i < xmlNodeList.Count; ++i)
      {
        if (xmlNodeList[i].InnerText == phrase)
          return (XmlElement) xmlNodeList[i];
      }
      return (XmlElement) null;
    }

    private static XmlDocument readFromDisk(ClientContext context)
    {
      string xml = context.Cache.Get<string>(nameof (NotePhrases));
      XmlDocument xmlDocument1 = new XmlDocument();
      if (xml != null)
      {
        xmlDocument1.LoadXml(xml);
        return xmlDocument1;
      }
      XmlDocument xmlDocument2 = new XmlDocument();
      using (DataFile latestVersion = FileStore.GetLatestVersion(NotePhrases.getFilePath(context)))
      {
        try
        {
          xmlDocument2.LoadXml(latestVersion.GetText());
        }
        catch
        {
          xmlDocument2.AppendChild((XmlNode) xmlDocument2.CreateElement(nameof (NotePhrases)));
          TraceLog.WriteInfo(nameof (NotePhrases), "Failed to open Form Groups document " + NotePhrases.getFilePath(context) + ". New document initialized.");
        }
      }
      context.Cache.Put(nameof (NotePhrases), (object) xmlDocument2.OuterXml, CacheSetting.Low);
      return xmlDocument2;
    }

    private static void writeToDisk(ClientContext context, XmlDocument xml)
    {
      string filePath = NotePhrases.getFilePath(context);
      context.Cache.Remove(nameof (NotePhrases));
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(filePath, MutexAccess.Write))
          dataFile.CheckIn(new BinaryObject(xml.OuterXml, Encoding.Default));
        TraceLog.WriteVerbose(nameof (NotePhrases), "Note phrases saved to file " + filePath);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (NotePhrases), "Error saving note phrases to file " + filePath + ": " + (object) ex);
        Err.Reraise(nameof (NotePhrases), ex);
      }
      context.Cache.Put(nameof (NotePhrases), (object) xml.OuterXml, CacheSetting.Low);
    }

    private static string getFilePath(ClientContext context)
    {
      return context.Settings.GetDataFilePath("Resource\\NotePhrases.xml");
    }
  }
}
