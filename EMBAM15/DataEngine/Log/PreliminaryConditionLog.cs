// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.PreliminaryConditionLog
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
  public class PreliminaryConditionLog : StandardConditionLog
  {
    public static readonly string XmlType = "PreliminaryCondition";
    private string priorTo = string.Empty;
    private bool underwriterAccess = true;
    private DateTime dateFulfilled = DateTime.MinValue;
    private string fulfilledBy = string.Empty;

    public PreliminaryConditionLog(string addedBy, string pairId)
      : base(addedBy, pairId)
    {
    }

    public PreliminaryConditionLog(
      UnderwritingConditionTemplate template,
      string addedBy,
      string pairId)
      : base((ConditionTemplate) template, addedBy, pairId)
    {
      this.category = template.Category;
      this.priorTo = template.PriorTo;
    }

    public PreliminaryConditionLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.category = attributeReader.GetString(nameof (Category));
      this.priorTo = attributeReader.GetString(nameof (PriorTo));
      this.underwriterAccess = attributeReader.GetBoolean(nameof (UnderwriterAccess));
      this.dateFulfilled = attributeReader.GetDate(nameof (DateFulfilled));
      this.fulfilledBy = attributeReader.GetString(nameof (FulfilledBy));
      this.MarkAsClean();
    }

    public override ConditionType ConditionType => ConditionType.Preliminary;

    public override string Category
    {
      get => this.category;
      set
      {
        if (!(this.category != value))
          return;
        this.category = value;
        this.MarkAsDirty();
      }
    }

    public string PriorTo
    {
      get => this.priorTo;
      set
      {
        if (!(this.priorTo != value))
          return;
        this.priorTo = value;
        this.MarkAsDirty();
      }
    }

    public bool UnderwriterAccess
    {
      get => this.underwriterAccess;
      set
      {
        if (this.underwriterAccess == value)
          return;
        this.underwriterAccess = value;
        string str = "UW Access";
        this.TrackChange(!this.underwriterAccess ? str + " unchecked" : str + " checked");
      }
    }

    public bool Fulfilled => this.dateFulfilled.Date != DateTime.MinValue.Date;

    public DateTime DateFulfilled => this.dateFulfilled;

    public string FulfilledBy => this.fulfilledBy;

    public void MarkAsFulfilled(DateTime date, string user)
    {
      this.dateFulfilled = date;
      this.fulfilledBy = user;
      this.MarkAsDirty();
    }

    public void UnmarkAsFulfilled()
    {
      this.dateFulfilled = DateTime.MinValue;
      this.fulfilledBy = string.Empty;
      this.MarkAsDirty();
    }

    public override ConditionStatus Status
    {
      get
      {
        if (this.Fulfilled)
          return ConditionStatus.Fulfilled;
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
          case ConditionStatus.Fulfilled:
            return "Fulfilled on " + this.dateFulfilled.ToShortDateString();
          case ConditionStatus.Requested:
            return "Requested on " + this.DateRequested.ToShortDateString();
          case ConditionStatus.Rerequested:
            return "Re-requested on " + this.DateRerequested.ToShortDateString();
          case ConditionStatus.Expected:
          case ConditionStatus.PastDue:
            return "Expected on " + this.DateExpected.ToShortDateString();
          case ConditionStatus.Received:
            return "Received on " + this.DateReceived.ToShortDateString();
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
          case ConditionStatus.Fulfilled:
            return this.DateFulfilled;
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
          default:
            return this.DateAdded;
        }
      }
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      if (this.Expected && !this.Received)
      {
        PipelineInfo.Alert alert = new PipelineInfo.Alert(31, this.Title, "expected", this.DateExpected, this.Guid, this.Guid);
        alertList.Add(alert);
      }
      alertList.AddRange((IEnumerable<PipelineInfo.Alert>) base.GetPipelineAlerts());
      return alertList.ToArray();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) PreliminaryConditionLog.XmlType);
      attributeWriter.Write("Category", (object) this.category);
      attributeWriter.Write("PriorTo", (object) this.priorTo);
      attributeWriter.Write("UnderwriterAccess", (object) this.underwriterAccess);
      attributeWriter.Write("DateFulfilled", (object) this.dateFulfilled);
      attributeWriter.Write("FulfilledBy", (object) this.fulfilledBy);
    }
  }
}
