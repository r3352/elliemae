// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockVoidLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LockVoidLog : LogRecordBase
  {
    public static readonly string XmlType = "LockVoid";
    private string timeVoided = string.Empty;
    private string voidedBy = string.Empty;
    private string voidedByFullName = string.Empty;
    private string requestGUID = string.Empty;
    private bool alertLoanOfficer;
    private bool isVoided;

    public LockVoidLog()
    {
    }

    public LockVoidLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.timeVoided = attributeReader.GetString(nameof (TimeVoided));
      this.voidedByFullName = attributeReader.GetString(nameof (VoidedBy));
      this.voidedBy = attributeReader.GetString("VoidedByID");
      this.requestGUID = attributeReader.GetString(nameof (RequestGUID));
      this.alertLoanOfficer = attributeReader.GetBoolean("Alert");
      this.isVoided = attributeReader.GetBoolean(nameof (Voided));
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockVoidLog.XmlType);
      attributeWriter.Write("TimeVoided", (object) this.timeVoided);
      attributeWriter.Write("VoidedBy", (object) this.voidedByFullName);
      attributeWriter.Write("VoidedByID", (object) this.voidedBy);
      attributeWriter.Write("RequestGUID", (object) this.requestGUID);
      attributeWriter.Write("Alert", (object) this.alertLoanOfficer);
      attributeWriter.Write("Voided", (object) this.isVoided);
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      log.Loan.TriggerCalculation("LOCKRATE.REQUESTSTATUS", log.Loan.GetField("LOCKRATE.REQUESTSTATUS"));
      log.Loan.TriggerCalculation("LOCKRATE.LASTACTIONTIME", log.Loan.GetField("LOCKRATE.LASTACTIONTIME"));
    }

    public override DateTime GetSortDate() => this.DateTimeVoided;

    public DateTime DateTimeVoided
    {
      get => DateTime.Parse(this.Date.ToString("MM/dd/yyyy") + " " + this.timeVoided);
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(69, "Lock Voided by " + this.voidedByFullName, "", this.Date, this.RequestGUID, this.Guid)
      };
    }

    public string TimeVoided
    {
      get => this.timeVoided;
      set
      {
        this.timeVoided = value;
        this.MarkAsDirty();
      }
    }

    public string VoidedBy => this.voidedBy;

    public string VoidedByFullName => this.voidedByFullName;

    public bool Voided
    {
      set
      {
        this.isVoided = value;
        this.MarkAsDirty();
      }
      get => this.isVoided;
    }

    public void SetVoidingUser(UserInfo user)
    {
      this.voidedBy = user.Userid;
      this.voidedByFullName = user.FullName;
      this.MarkAsDirty();
    }

    public string RequestGUID
    {
      get => this.requestGUID;
      set
      {
        this.requestGUID = value;
        this.MarkAsDirty();
      }
    }

    public bool AlertLoanOfficer
    {
      get => this.alertLoanOfficer;
      set
      {
        this.alertLoanOfficer = value;
        this.MarkAsDirty();
      }
    }
  }
}
