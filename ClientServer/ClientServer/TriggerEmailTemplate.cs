// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerEmailTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerEmailTemplate : IXmlSerializable
  {
    private string subject;
    private string body;
    private string[] recipientUsers;
    private int[] recipientRoles;
    private bool displayInLog;

    public TriggerEmailTemplate(
      string subject,
      string body,
      string[] recipientUsers,
      int[] recipientRoles,
      bool displayInLog)
    {
      this.subject = subject;
      this.body = this.encodeBodyText(body);
      this.recipientUsers = recipientUsers;
      this.recipientRoles = recipientRoles;
      this.displayInLog = displayInLog;
    }

    public TriggerEmailTemplate(XmlSerializationInfo info)
    {
      this.subject = info.GetString(nameof (subject));
      this.body = this.decodeBodyText(info.GetString(nameof (body)));
      this.recipientUsers = ((List<string>) info.GetValue("users", typeof (XmlList<string>))).ToArray();
      this.recipientRoles = ((List<int>) info.GetValue("roles", typeof (XmlList<int>))).ToArray();
      this.displayInLog = info.GetBoolean(nameof (displayInLog), true);
    }

    public string Subject => this.subject;

    public string Body
    {
      get
      {
        this.body = this.decodeBodyText(this.body);
        return this.body;
      }
    }

    public string[] RecipientUsers => this.recipientUsers;

    public int[] RecipientRoles => this.recipientRoles;

    public bool DisplayInLog => this.displayInLog;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("subject", (object) this.subject);
      info.AddValue("body", (object) this.encodeBodyText(this.body));
      info.AddValue("users", (object) new XmlList<string>((IEnumerable<string>) this.recipientUsers));
      info.AddValue("roles", (object) new XmlList<int>((IEnumerable<int>) this.recipientRoles));
      info.AddValue("displayInLog", (object) this.displayInLog);
    }

    private string encodeBodyText(string inputText)
    {
      if (!string.IsNullOrEmpty(inputText) && (inputText.Contains("\r") || inputText.Contains("\n")))
        inputText = inputText.Replace("\r", "&#xA").Replace("\n", "&#13");
      return inputText;
    }

    private string decodeBodyText(string inputText)
    {
      if (!string.IsNullOrEmpty(inputText) && inputText.Contains("&#"))
        inputText = inputText.Replace("&#xA", "\r").Replace("&#13", "\n");
      return inputText;
    }
  }
}
