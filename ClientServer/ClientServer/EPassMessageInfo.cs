// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPassMessageInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EPassMessageInfo
  {
    public const string LeadCenterMessageType = "LC_LEADS�";
    public const string FileTransferMessageType = "FILETRANSFER�";
    private string messageId;
    private string messageType;
    private string source;
    private string loanGuid;
    private string userId;
    private string description;
    private DateTime timestamp;
    private bool active;
    private string msgXml;

    public EPassMessageInfo(
      string messageId,
      string messageType,
      string source,
      string loanGuid,
      string userId,
      string description,
      DateTime timestamp,
      bool active,
      string msgXml)
    {
      this.messageId = messageId;
      this.messageType = messageType;
      this.source = source;
      this.loanGuid = loanGuid;
      this.userId = userId;
      this.description = description;
      this.timestamp = timestamp;
      this.active = active;
      this.msgXml = msgXml;
    }

    public string MessageID => this.messageId;

    public string MessageType => this.messageType;

    public string Source => this.source;

    public string Description => this.description;

    public string UserID => this.userId;

    public string LoanGuid => this.loanGuid;

    public DateTime Timestamp => this.timestamp;

    public bool Active => this.active;

    public string MessageXml => this.msgXml;

    public bool IsLoanMailboxMessage
    {
      get => string.IsNullOrEmpty(this.loanGuid) && this.messageType != "LEADS";
    }

    public EPassMessageAction GetAction()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.msgXml);
      string str1 = xmlDocument.DocumentElement.GetAttribute("ActionType") ?? "";
      string str2 = xmlDocument.DocumentElement.GetAttribute("ActionValue") ?? "";
      if (str1 == "")
        return (EPassMessageAction) null;
      string description = "Go there now";
      XmlElement detailsElement = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("DETAILS");
      if (detailsElement != null)
        description = detailsElement.GetAttribute("ActionText") ?? "";
      if (str1.StartsWith("AIQ"))
        description = this.Description;
      string messageId = this.MessageID;
      switch (str1)
      {
        case "ENCMD":
          return (EPassMessageAction) new EPassMessageCommandAction(str2, description, this.parseCommandParameters(detailsElement));
        case "EPASS":
          return (EPassMessageAction) new EPassMessageSignatureAction(str2, description);
        case "URL":
          return (EPassMessageAction) new EPassMessageUrlAction(str2, description);
        case "AIQ.INCOME":
          return (EPassMessageAction) new AIQIncomeComparisonAction(description, this.timestamp, messageId);
        case "AIQ.ASSET":
          return (EPassMessageAction) new AIQAssetComparisonAction(description, this.timestamp, messageId);
        case "AIQ.AUS":
          return (EPassMessageAction) new AIQAUSComparisonAction(description, this.timestamp, messageId);
        case "AIQ.AUDIT":
          return (EPassMessageAction) new AIQAuditComparisonAction(description, this.timestamp, messageId);
        default:
          throw new NotSupportedException("The specified action type '" + str1 + "' is not supported");
      }
    }

    private Dictionary<string, string> parseCommandParameters(XmlElement detailsElement)
    {
      Dictionary<string, string> commandParameters = new Dictionary<string, string>();
      if (detailsElement != null)
      {
        foreach (XmlAttribute attribute in (XmlNamedNodeMap) detailsElement.Attributes)
        {
          if (attribute.Name != "ActionText" && attribute.Name != "Desc")
            commandParameters[attribute.Name] = attribute.Value;
        }
      }
      return commandParameters;
    }

    public EPassMessageInfo.BorrowerName GetBorrowerName()
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.msgXml);
      XmlElement xmlElement = (XmlElement) xmlDocument.DocumentElement.SelectSingleNode("DETAILS");
      if (xmlElement == null)
        return (EPassMessageInfo.BorrowerName) null;
      string firstName = (xmlElement.GetAttribute("BFName") ?? "").Trim();
      string lastName = (xmlElement.GetAttribute("BLName") ?? "").Trim();
      return firstName == "" && lastName == "" ? (EPassMessageInfo.BorrowerName) null : new EPassMessageInfo.BorrowerName(firstName, lastName);
    }

    public class BorrowerName
    {
      private string firstName;
      private string lastName;

      public BorrowerName(string firstName, string lastName)
      {
        this.firstName = firstName;
        this.lastName = lastName;
      }

      public string FirstName => this.firstName;

      public string LastName => this.lastName;

      public override string ToString() => this.FirstName + " " + this.LastName;
    }
  }
}
