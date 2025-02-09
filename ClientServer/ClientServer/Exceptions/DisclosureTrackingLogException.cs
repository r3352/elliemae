// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.DisclosureTrackingLogException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class DisclosureTrackingLogException : ServerException
  {
    private List<DisclosureTrackingLog> missingLogs;
    private List<DisclosureTracking2015Log> missing2015Logs;
    private List<IDisclosureTracking2015Log> missingIDisclosureTracking2015Logs;

    private DisclosureTrackingLogException(string message, List<DisclosureTrackingLog> missingLogs)
      : base(message)
    {
      this.missingLogs = missingLogs;
    }

    private DisclosureTrackingLogException(
      string message,
      List<DisclosureTracking2015Log> missing2015Logs)
      : base(message)
    {
      this.missing2015Logs = missing2015Logs;
    }

    private DisclosureTrackingLogException(
      string message,
      List<IDisclosureTracking2015Log> missingIDisclosureTrackingLogs)
      : base(message)
    {
      this.missingIDisclosureTracking2015Logs = missingIDisclosureTrackingLogs;
    }

    public static DisclosureTrackingLogException GetDisclosureTrackingLogException(
      List<DisclosureTrackingLog> missingLogs)
    {
      string message = "The following disclosure tracking records are getting dropped.";
      missingLogs.ForEach((Action<DisclosureTrackingLog>) (X => message = message + Environment.NewLine + X.ToString()));
      return new DisclosureTrackingLogException(message, missingLogs);
    }

    public static DisclosureTrackingLogException GetDisclosureTracking2015LogException(
      List<DisclosureTracking2015Log> missing2015Logs)
    {
      string message = "The following disclosure tracking records are getting dropped.";
      missing2015Logs.ForEach((Action<DisclosureTracking2015Log>) (X => message = message + Environment.NewLine + X.ToString()));
      return new DisclosureTrackingLogException(message, missing2015Logs);
    }

    public static DisclosureTrackingLogException GetDisclosureTracking2015LogException(
      List<IDisclosureTracking2015Log> missingIDisclosureTracking2015Logs)
    {
      string message = "The following disclosure tracking records are getting dropped.";
      missingIDisclosureTracking2015Logs.ForEach((Action<IDisclosureTracking2015Log>) (X => message = message + Environment.NewLine + X.ToString()));
      return new DisclosureTrackingLogException(message, missingIDisclosureTracking2015Logs);
    }

    public string MissingLogData()
    {
      string str = "";
      XmlDocument xmlDocument = new XmlDocument();
      foreach (DisclosureTrackingLog disclosureTrackingLog in this.missingLogs.ToArray())
      {
        XmlElement element = xmlDocument.CreateElement("LogData");
        disclosureTrackingLog.ToXml(element);
        str = str + Environment.NewLine + element.OuterXml;
      }
      return str + Environment.NewLine;
    }

    public string Missing2015LogData()
    {
      string str = "";
      XmlDocument xmlDocument = new XmlDocument();
      foreach (DisclosureTracking2015Log disclosureTracking2015Log in this.missing2015Logs.ToArray())
      {
        XmlElement element = xmlDocument.CreateElement("LogData");
        disclosureTracking2015Log.ToXml(element);
        str = str + Environment.NewLine + element.OuterXml;
      }
      return str + Environment.NewLine;
    }

    public string MissingIDisclosureTracking2015LogData()
    {
      string str = "";
      XmlDocument xmlDocument = new XmlDocument();
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.missingIDisclosureTracking2015Logs.ToArray())
      {
        switch (disclosureTracking2015Log)
        {
          case DisclosureTracking2015Log _:
            XmlElement element1 = xmlDocument.CreateElement("LogData");
            ((LogRecordBase) disclosureTracking2015Log).ToXml(element1);
            str = str + Environment.NewLine + element1.OuterXml;
            break;
          case EnhancedDisclosureTracking2015Log _:
            XmlElement element2 = xmlDocument.CreateElement("LogData");
            ((LogRecordBase) disclosureTracking2015Log).ToXml(element2);
            str = str + Environment.NewLine + element2.OuterXml;
            break;
        }
      }
      return str + Environment.NewLine;
    }
  }
}
