// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockRemovedLog
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
  public class LockRemovedLog : LogRecordBase
  {
    public static readonly string XmlType = "LockRemoved";
    private string timeRemoved = string.Empty;
    private string removedBy = string.Empty;
    private string removedByFullName = string.Empty;
    private string requestGUID = string.Empty;
    private bool alertLoanOfficer;
    private bool hideLog;

    public LockRemovedLog()
    {
    }

    public LockRemovedLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.timeRemoved = attributeReader.GetString(nameof (timeRemoved));
      this.removedByFullName = attributeReader.GetString(nameof (removedBy));
      this.removedBy = attributeReader.GetString("removedByID");
      this.requestGUID = attributeReader.GetString(nameof (RequestGUID));
      this.alertLoanOfficer = attributeReader.GetBoolean("Alert");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockRemovedLog.XmlType);
      attributeWriter.Write("timeRemoved", (object) this.timeRemoved);
      attributeWriter.Write("removedBy", (object) this.removedByFullName);
      attributeWriter.Write("removedByID", (object) this.removedBy);
      attributeWriter.Write("RequestGUID", (object) this.requestGUID);
      attributeWriter.Write("Alert", (object) this.alertLoanOfficer);
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      log.Loan.TriggerCalculation("LOCKRATE.REQUESTSTATUS", log.Loan.GetField("LOCKRATE.REQUESTSTATUS"));
      log.Loan.TriggerCalculation("LOCKRATE.LASTACTIONTIME", log.Loan.GetField("LOCKRATE.LASTACTIONTIME"));
    }

    public override DateTime GetSortDate() => this.DateTimeRemoved;

    public DateTime DateTimeRemoved
    {
      get => DateTime.Parse(this.Date.ToString("MM/dd/yyyy") + " " + this.timeRemoved);
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (this.Log.GetLastLockAlertLog() != this)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(44, "Lock removed from Correspondent Trade by " + this.removedByFullName, "", this.Date, this.RequestGUID, this.Guid)
      };
    }

    public string TimeRemoved
    {
      get => this.timeRemoved;
      set
      {
        this.timeRemoved = value;
        this.MarkAsDirty();
      }
    }

    public string RemovedBy => this.removedBy;

    public string RemovedByFullName => this.removedByFullName;

    public void SetRemovedBy(UserInfo user)
    {
      this.removedBy = user.Userid;
      this.removedByFullName = user.FullName;
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

    public override bool DisplayInLog
    {
      get => !this.hideLog;
      set
      {
        this.hideLog = !value;
        this.MarkAsDirty();
      }
    }
  }
}
