// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPassMessageServerResponse
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class EPassMessageServerResponse
  {
    public const string WebServiceURL = "https://epass.elliemaeservices.com/epassws/GetMessageAlerts.aspx�";
    private const string className = "EPassMessagePollTaskHandler�";
    private static string sw = Tracing.SwEpass;
    private XmlDocument xml;

    public EPassMessageServerResponse(XmlDocument xml) => this.xml = xml;

    public EPassMessageInfo[] GetMessages()
    {
      List<EPassMessageInfo> epassMessageInfoList = new List<EPassMessageInfo>();
      foreach (XmlElement selectNode in this.xml.SelectNodes("/MESSAGEENVELOP/MESSAGES"))
        epassMessageInfoList.AddRange((IEnumerable<EPassMessageInfo>) this.parseUserMessages(selectNode));
      return epassMessageInfoList.ToArray();
    }

    public string[] GetReadMessageIDs()
    {
      XmlElement xmlElement = (XmlElement) this.xml.SelectSingleNode("/MESSAGEENVELOP/MESSAGESREAD/ID");
      if (xmlElement == null || xmlElement.InnerText == null)
        return new string[0];
      return xmlElement.InnerText.Split(',');
    }

    public string GetLastMessageID()
    {
      XmlElement xmlElement = (XmlElement) this.xml.SelectSingleNode("/MESSAGEENVELOP/RESPONSEDETAILS");
      if (xmlElement == null)
        throw new Exception("EPassMessage response does not contain expected RESPONSEDETAILS element");
      return xmlElement.GetAttribute("LastMsgID") ?? "";
    }

    public string GetLastReadMessageDate()
    {
      XmlElement xmlElement = (XmlElement) this.xml.SelectSingleNode("/MESSAGEENVELOP/RESPONSEDETAILS");
      if (xmlElement == null)
        throw new Exception("EPassMessage response does not contain expected RESPONSEDETAILS element");
      return xmlElement.GetAttribute("LastReadMsgDate") ?? "";
    }

    public DateTime GetFirstMessageDate()
    {
      return Utils.ParseDate((object) (((XmlElement) this.xml.SelectSingleNode("/MESSAGEENVELOP/RESPONSEDETAILS") ?? throw new Exception("EPassMessage response does not contain expected RESPONSEDETAILS element")).GetAttribute("FirstMsgDate") ?? ""));
    }

    private EPassMessageInfo[] parseUserMessages(XmlElement msgsXml)
    {
      List<EPassMessageInfo> epassMessageInfoList = new List<EPassMessageInfo>();
      string lower = (msgsXml.GetAttribute("UserName") ?? "").ToLower();
      foreach (XmlElement selectNode in msgsXml.SelectNodes("MSGCLASS"))
        epassMessageInfoList.AddRange((IEnumerable<EPassMessageInfo>) this.parseMessageClass(lower, selectNode));
      return epassMessageInfoList.ToArray();
    }

    private EPassMessageInfo[] parseMessageClass(string userId, XmlElement msgClassXml)
    {
      List<EPassMessageInfo> epassMessageInfoList = new List<EPassMessageInfo>();
      try
      {
        string msgType = msgClassXml.GetAttribute("Type") ?? "";
        bool loanRelated = (msgClassXml.GetAttribute("LoanRelated") ?? "") == "1";
        foreach (XmlElement selectNode in msgClassXml.SelectNodes("MSG"))
        {
          EPassMessageInfo message = this.parseMessage(userId, msgType, loanRelated, selectNode);
          if (message != null)
            epassMessageInfoList.Add(message);
        }
      }
      catch
      {
      }
      return epassMessageInfoList.ToArray();
    }

    private EPassMessageInfo parseMessage(
      string userId,
      string msgType,
      bool loanRelated,
      XmlElement msgXml)
    {
      string messageId = msgXml.GetAttribute("ID") ?? "";
      string loanGuid = loanRelated ? msgXml.GetAttribute("LoanUID") ?? "" : "";
      XmlElement xmlElement = (XmlElement) msgXml.SelectSingleNode("DETAILS");
      if (xmlElement == null)
      {
        Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Warning, "Invalid ePASS Message received. Missing required DETAILS element.");
        Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Warning, msgXml.OuterXml);
        return (EPassMessageInfo) null;
      }
      string description = xmlElement.GetAttribute("Desc") ?? "";
      DateTime date = Utils.ParseDate((object) (xmlElement.GetAttribute("MsgDate") ?? ""), DateTime.MinValue);
      string source = xmlElement.GetAttribute("SenderName") ?? "";
      if (messageId == "")
      {
        Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Error, "Invalid ePASS Message received. Missing required ID attribute.");
        Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Error, msgXml.OuterXml);
        return (EPassMessageInfo) null;
      }
      if (!(date == DateTime.MinValue))
        return new EPassMessageInfo(messageId, msgType, source, loanGuid, userId, description, date, true, msgXml.OuterXml);
      Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Error, "Invalid ePASS Message received. Missing required TimeStamp attribute.");
      Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Error, msgXml.OuterXml);
      return (EPassMessageInfo) null;
    }

    public static EPassMessageServerResponse QueryServer(
      string clientId,
      string lastMsgId,
      string lastReadMsgDate,
      bool readMessagesOnly,
      TimeSpan requestTimeout,
      string serviceUrl = "�")
    {
      string requestUriString = !string.IsNullOrWhiteSpace(serviceUrl) ? serviceUrl : "https://epass.elliemaeservices.com/epassws/GetMessageAlerts.aspx";
      string args = "ClientID=" + clientId;
      if ((lastMsgId ?? "") != "")
        args = args + "&LastMsgID=" + HttpUtility.UrlEncode(lastMsgId);
      if (lastReadMsgDate != "")
        args = args + "&LastReadMsgDate=" + HttpUtility.UrlEncode(lastReadMsgDate);
      if (readMessagesOnly)
        args += "&ReadOnly=1";
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUriString);
      req.Method = "POST";
      req.ContentType = "application/x-www-form-urlencoded";
      req.AllowAutoRedirect = true;
      req.Proxy = WebRequest.DefaultWebProxy;
      req.Proxy.Credentials = CredentialCache.DefaultCredentials;
      req.KeepAlive = false;
      req.ReadWriteTimeout = (int) requestTimeout.TotalMilliseconds;
      req.Timeout = (int) requestTimeout.TotalMilliseconds;
      string xmlText = EPassMessageServerResponse.executeWebRequest(req, args);
      return xmlText == "" ? (EPassMessageServerResponse) null : new EPassMessageServerResponse(EPassMessageServerResponse.parseXml(xmlText));
    }

    private static string executeWebRequest(HttpWebRequest req, string args)
    {
      try
      {
        req.ContentLength = (long) args.Length;
        using (Stream requestStream = req.GetRequestStream())
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream))
            streamWriter.Write(args);
        }
        using (HttpWebResponse response = (HttpWebResponse) req.GetResponse())
          return new StreamReader(response.GetResponseStream()).ReadToEnd();
      }
      catch (Exception ex)
      {
        throw new Exception("Error executing web request: " + ex.Message, ex);
      }
    }

    private static XmlDocument parseXml(string xmlText)
    {
      try
      {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlText);
        return xml;
      }
      catch (Exception ex)
      {
        Tracing.Log(EPassMessageServerResponse.sw, "EPassMessagePollTaskHandler", TraceLevel.Error, "Error in ePASS Message reponse: " + xmlText);
        throw new Exception("Invalid response from ePASS Message Server: " + ex.Message, ex);
      }
    }
  }
}
