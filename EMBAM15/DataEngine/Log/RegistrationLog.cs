// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.RegistrationLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class RegistrationLog : LogRecordBase
  {
    public static string XmlType = "Registration";
    private string registeredByID = string.Empty;
    private string registeredByName = string.Empty;
    private DateTime registeredDate = DateTime.MinValue;
    private DateTime expiredDate = DateTime.MaxValue;
    private string investorName = string.Empty;
    private string reference = string.Empty;
    private bool isCurrent = true;

    public RegistrationLog()
    {
    }

    public RegistrationLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.registeredByID = attributeReader.GetString(nameof (RegisteredByID));
      this.registeredByName = attributeReader.GetString(nameof (RegisteredByName));
      this.investorName = attributeReader.GetString(nameof (InvestorName));
      this.registeredDate = attributeReader.GetDate(nameof (RegisteredDate));
      this.expiredDate = attributeReader.GetDate(nameof (ExpiredDate));
      this.reference = attributeReader.GetString(nameof (Reference));
      this.isCurrent = attributeReader.GetBoolean("Current", false);
      this.MarkAsClean();
    }

    public string RegisteredByID
    {
      get => this.registeredByID;
      set
      {
        this.registeredByID = value;
        this.MarkAsDirty();
      }
    }

    public string RegisteredByName
    {
      get => this.registeredByName;
      set
      {
        this.registeredByName = value;
        this.MarkAsDirty();
      }
    }

    public string InvestorName
    {
      get => this.investorName;
      set
      {
        this.investorName = value;
        this.MarkAsDirty();
      }
    }

    public DateTime RegisteredDate
    {
      get => this.registeredDate;
      set
      {
        this.registeredDate = value;
        this.date = value;
        this.MarkAsDirty();
      }
    }

    public DateTime ExpiredDate
    {
      get => this.expiredDate;
      set
      {
        this.expiredDate = value;
        this.MarkAsDirty();
      }
    }

    public bool Expires => this.expiredDate != DateTime.MinValue;

    public bool IsExpired
    {
      get => this.expiredDate != DateTime.MinValue && DateTime.Today > this.expiredDate.Date;
    }

    public bool IsCurrent => this.isCurrent;

    public string Reference
    {
      get => this.reference;
      set
      {
        this.reference = value;
        this.MarkAsDirty();
      }
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (!this.Expires || !this.IsCurrent)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(14, this.InvestorName, "expected", this.expiredDate, "", this.Guid)
      };
    }

    internal void SetCurrent(bool current) => this.isCurrent = current;

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      if (!this.isCurrent)
        return;
      foreach (RegistrationLog registrationLog in log.GetAllRecordsOfType(typeof (RegistrationLog)))
      {
        if (registrationLog != this && registrationLog.IsCurrent)
          registrationLog.SetCurrent(false);
      }
    }

    public override void ToXml(XmlElement e)
    {
      this.date = this.registeredDate;
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) RegistrationLog.XmlType);
      attributeWriter.Write("RegisteredByID", (object) this.registeredByID);
      attributeWriter.Write("RegisteredByName", (object) this.registeredByName);
      attributeWriter.Write("RegisteredDate", (object) this.registeredDate);
      attributeWriter.Write("ExpiredDate", (object) this.expiredDate);
      attributeWriter.Write("InvestorName", (object) this.investorName);
      attributeWriter.Write("Reference", (object) this.reference);
      attributeWriter.Write("Current", (object) this.isCurrent);
    }
  }
}
