// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.PostClosingConditionLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class PostClosingConditionLog : StandardConditionLog
  {
    public static readonly string XmlType = "PostClosingCondition";
    private string recipient = string.Empty;
    private DateTime dateSent = DateTime.MinValue;
    private string sentBy = string.Empty;
    private DateTime dateCleared = DateTime.MinValue;
    private string clearedBy = string.Empty;
    private bool isInternal;
    private bool isExternal;

    public PostClosingConditionLog(string addedBy, string pairId)
      : base(addedBy, pairId)
    {
    }

    public PostClosingConditionLog(
      PostClosingConditionTemplate template,
      string addedBy,
      string pairId)
      : base((ConditionTemplate) template, addedBy, pairId)
    {
      this.Source = template.Source;
      this.recipient = template.Recipient;
      this.isInternal = template.IsInternal;
      this.isExternal = template.IsExternal;
    }

    public PostClosingConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.recipient = attributeReader.GetString(nameof (Recipient));
      this.dateSent = attributeReader.GetDate(nameof (DateSent));
      this.sentBy = attributeReader.GetString(nameof (SentBy));
      this.dateCleared = attributeReader.GetDate(nameof (DateCleared));
      this.clearedBy = attributeReader.GetString(nameof (ClearedBy));
      this.isInternal = attributeReader.GetBoolean(nameof (IsInternal), false);
      this.isExternal = attributeReader.GetBoolean(nameof (IsExternal), false);
      this.MarkAsClean();
    }

    public override ConditionType ConditionType => ConditionType.PostClosing;

    public string Recipient
    {
      get => this.recipient;
      set
      {
        if (!(this.recipient != value))
          return;
        this.recipient = value;
        this.TrackChange("Recipient changed to \"" + this.recipient + "\"");
      }
    }

    public bool IsInternal
    {
      get => this.isInternal;
      set
      {
        if (this.isInternal == value)
          return;
        this.isInternal = value;
        string str = "Print Internally";
        this.TrackChange(!this.isInternal ? str + " unchecked" : str + " checked");
      }
    }

    public bool IsExternal
    {
      get => this.isExternal;
      set
      {
        if (this.isExternal == value)
          return;
        this.isExternal = value;
        string str = "Print Externally";
        this.TrackChange(!this.isExternal ? str + " unchecked" : str + " checked");
      }
    }

    public bool Sent => this.dateSent.Date != DateTime.MinValue.Date;

    public DateTime DateSent => this.dateSent;

    public string SentBy => this.sentBy;

    public void MarkAsSent(DateTime date, string user)
    {
      this.dateSent = date;
      this.sentBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsSent()
    {
      this.dateSent = DateTime.MinValue;
      this.sentBy = string.Empty;
      this.MarkAsDirty();
    }

    public bool Cleared => this.dateCleared.Date != DateTime.MinValue.Date;

    public DateTime DateCleared => this.dateCleared;

    public string ClearedBy => this.clearedBy;

    public void MarkAsCleared(DateTime date, string user)
    {
      this.dateCleared = date;
      this.clearedBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsCleared()
    {
      this.dateCleared = DateTime.MinValue;
      this.clearedBy = string.Empty;
      this.MarkAsDirty();
    }

    public override ConditionStatus Status
    {
      get
      {
        if (this.Cleared)
          return ConditionStatus.Cleared;
        if (this.Sent)
          return ConditionStatus.Sent;
        if (this.Received)
          return ConditionStatus.Received;
        if (this.IsPastDue)
          return ConditionStatus.PastDue;
        if (this.Expected)
          return ConditionStatus.Expected;
        if (this.Rerequested)
          return ConditionStatus.Rerequested;
        return this.Requested ? ConditionStatus.Requested : ConditionStatus.Added;
      }
    }

    public override string StatusDescription
    {
      get
      {
        switch (this.Status)
        {
          case ConditionStatus.Requested:
            return "Requested on " + this.DateRequested.ToShortDateString();
          case ConditionStatus.Rerequested:
            return "Re-requested on " + this.DateRerequested.ToShortDateString();
          case ConditionStatus.Expected:
            return "Expected on " + this.DateExpected.ToShortDateString();
          case ConditionStatus.PastDue:
            return "Expected on " + this.DateExpected.ToShortDateString();
          case ConditionStatus.Received:
            return "Received on " + this.DateReceived.ToShortDateString();
          case ConditionStatus.Sent:
            return "Sent on " + this.dateSent.ToShortDateString();
          case ConditionStatus.Cleared:
            return "Cleared on " + this.dateCleared.ToShortDateString();
          default:
            return "Added on " + this.DateAdded.ToShortDateString();
        }
      }
    }

    public override DateTime Date
    {
      get
      {
        switch (this.Status)
        {
          case ConditionStatus.Requested:
            return this.DateRequested;
          case ConditionStatus.Rerequested:
            return this.DateRerequested;
          case ConditionStatus.Expected:
            return this.DateExpected;
          case ConditionStatus.PastDue:
            return this.DateExpected;
          case ConditionStatus.Received:
            return this.DateReceived;
          case ConditionStatus.Sent:
            return this.dateSent;
          case ConditionStatus.Cleared:
            return this.dateCleared;
          default:
            return this.DateAdded;
        }
      }
    }

    internal override void AttachToLog(LogList log) => base.AttachToLog(log);

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      if (this.Expected && !this.Received)
      {
        PipelineInfo.Alert alert = new PipelineInfo.Alert(15, this.Title, "expected", this.DateExpected, this.Guid, this.Guid);
        alertList.Add(alert);
      }
      alertList.AddRange((IEnumerable<PipelineInfo.Alert>) base.GetPipelineAlerts());
      return alertList.ToArray();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) PostClosingConditionLog.XmlType);
      attributeWriter.Write("Recipient", (object) this.recipient);
      attributeWriter.Write("DateSent", (object) this.dateSent);
      attributeWriter.Write("SentBy", (object) this.sentBy);
      attributeWriter.Write("DateCleared", (object) this.dateCleared);
      attributeWriter.Write("ClearedBy", (object) this.clearedBy);
      attributeWriter.Write("IsInternal", (object) this.isInternal);
      attributeWriter.Write("IsExternal", (object) this.isExternal);
    }
  }
}
