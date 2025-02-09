// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactHistoryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Contact;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactHistoryItem
  {
    private int historyId = -1;
    private string eventType;
    private DateTime timestamp;
    private int loanId;
    private string letterName;
    private string sender;
    private string subject;
    private ContactSource source = ContactSource.NotAvailable;
    private string campaignName;
    private string campaignStepName;
    private string campaignActivityStatus;
    private DateTime campaignScheduledDate;
    private int campaignStepNumber;

    public ContactHistoryItem(
      int historyId,
      string eventType,
      DateTime timestamp,
      int loanId,
      string letterName,
      string sender,
      string subject,
      ContactSource contactSource,
      string campaignName,
      string campaignStepName,
      string campaignActivityStatus,
      DateTime campaignScheduledDate,
      int campaignStepNumber)
    {
      this.historyId = historyId;
      this.eventType = eventType;
      this.timestamp = timestamp;
      this.loanId = loanId;
      this.letterName = letterName;
      this.sender = sender;
      this.subject = subject;
      this.source = contactSource;
      this.campaignName = campaignName;
      this.campaignStepName = campaignStepName;
      this.campaignActivityStatus = campaignActivityStatus;
      this.campaignScheduledDate = campaignScheduledDate;
      this.campaignStepNumber = campaignStepNumber;
    }

    public ContactHistoryItem(
      int historyId,
      string eventType,
      DateTime timestamp,
      int loanId,
      string letterName,
      string sender,
      string subject)
      : this(historyId, eventType, timestamp, loanId, letterName, sender, subject, ContactSource.NotAvailable, string.Empty, string.Empty, string.Empty, DateTime.MinValue, 0)
    {
    }

    public ContactHistoryItem(int historyId, string eventType, DateTime timestamp, int loanId)
      : this(historyId, eventType, timestamp, loanId, string.Empty, string.Empty, string.Empty, ContactSource.NotAvailable, string.Empty, string.Empty, string.Empty, DateTime.MinValue, 0)
    {
    }

    public ContactHistoryItem(string eventType, DateTime timestamp, int loanId)
      : this(-1, eventType, timestamp, loanId)
    {
    }

    public ContactHistoryItem(string eventType, int loanId)
      : this(eventType, DateTime.Now, loanId)
    {
    }

    public ContactHistoryItem(string eventType, DateTime timestamp)
      : this(-1, eventType, timestamp, -1)
    {
    }

    public ContactHistoryItem(string eventType)
      : this(eventType, -1)
    {
    }

    public ContactHistoryItem()
      : this("")
    {
    }

    public int HistoryItemID => this.historyId;

    public string EventType
    {
      get => this.eventType;
      set => this.eventType = value;
    }

    public DateTime Timestamp
    {
      get => this.timestamp;
      set => this.timestamp = value;
    }

    public int LoanID
    {
      get => this.loanId;
      set => this.loanId = value;
    }

    public string LetterName
    {
      get => this.letterName;
      set => this.letterName = value;
    }

    public string Sender
    {
      get => this.sender;
      set => this.sender = value;
    }

    public string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public ContactSource Source
    {
      get => this.source;
      set => this.source = value;
    }

    public string CampaignName => this.campaignName;

    public string CampaignStepName => this.campaignStepName;

    public string CampaignActivityStatus => this.campaignActivityStatus;

    public DateTime CampaignScheduledDate => this.campaignScheduledDate;

    public int CampaignStepNumber => this.campaignStepNumber;
  }
}
