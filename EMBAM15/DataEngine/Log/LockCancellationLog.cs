// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockCancellationLog
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
  public class LockCancellationLog : LogRecordBase
  {
    public static readonly string XmlType = "LockCancellation";
    private string timeCancelled = string.Empty;
    private string cancelledBy = string.Empty;
    private string cancelledByFullName = string.Empty;
    private string requestGUID = string.Empty;
    private bool alertLoanOfficer;

    public LockCancellationLog()
    {
    }

    public LockCancellationLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.timeCancelled = attributeReader.GetString(nameof (timeCancelled));
      this.cancelledByFullName = attributeReader.GetString(nameof (cancelledBy));
      this.cancelledBy = attributeReader.GetString("cancelledByID");
      this.requestGUID = attributeReader.GetString(nameof (RequestGUID));
      this.alertLoanOfficer = attributeReader.GetBoolean("Alert");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockCancellationLog.XmlType);
      attributeWriter.Write("timeCancelled", (object) this.timeCancelled);
      attributeWriter.Write("cancelledBy", (object) this.cancelledByFullName);
      attributeWriter.Write("cancelledByID", (object) this.cancelledBy);
      attributeWriter.Write("RequestGUID", (object) this.requestGUID);
      attributeWriter.Write("Alert", (object) this.alertLoanOfficer);
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      log.Loan.TriggerCalculation("LOCKRATE.REQUESTSTATUS", log.Loan.GetField("LOCKRATE.REQUESTSTATUS"));
      log.Loan.TriggerCalculation("LOCKRATE.LASTACTIONTIME", log.Loan.GetField("LOCKRATE.LASTACTIONTIME"));
    }

    public override DateTime GetSortDate() => this.DateTimeCancelled;

    public DateTime DateTimeCancelled
    {
      get => DateTime.Parse(this.Date.ToString("MM/dd/yyyy") + " " + this.timeCancelled);
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (this.Log.GetLastLockAlertLog() != this)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(34, "Lock cancelled by " + this.cancelledByFullName, "", this.Date, this.RequestGUID, this.Guid)
      };
    }

    public string TimeCancelled
    {
      get => this.timeCancelled;
      set
      {
        this.timeCancelled = value;
        this.MarkAsDirty();
      }
    }

    public string CancelledBy => this.cancelledBy;

    public string CancelledByFullName => this.cancelledByFullName;

    public void SetCancellingUser(UserInfo user)
    {
      this.cancelledBy = user.Userid;
      this.cancelledByFullName = user.FullName;
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
