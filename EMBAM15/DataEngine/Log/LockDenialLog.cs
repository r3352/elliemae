// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockDenialLog
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
  public class LockDenialLog : LogRecordBase
  {
    public static readonly string XmlType = "LockDenial";
    private string timeDenied = string.Empty;
    private string deniedBy = string.Empty;
    private string deniedByFullName = string.Empty;
    private string requestGUID = string.Empty;
    private bool alertLoanOfficer;

    public LockDenialLog()
    {
    }

    public LockDenialLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.timeDenied = attributeReader.GetString(nameof (TimeDenied));
      this.deniedByFullName = attributeReader.GetString(nameof (DeniedBy));
      this.deniedBy = attributeReader.GetString("DeniedByID");
      this.requestGUID = attributeReader.GetString(nameof (RequestGUID));
      this.alertLoanOfficer = attributeReader.GetBoolean("Alert");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockDenialLog.XmlType);
      attributeWriter.Write("TimeDenied", (object) this.timeDenied);
      attributeWriter.Write("DeniedBy", (object) this.deniedByFullName);
      attributeWriter.Write("DeniedByID", (object) this.deniedBy);
      attributeWriter.Write("RequestGUID", (object) this.requestGUID);
      attributeWriter.Write("Alert", (object) this.alertLoanOfficer);
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      log.Loan.TriggerCalculation("LOCKRATE.REQUESTSTATUS", log.Loan.GetField("LOCKRATE.REQUESTSTATUS"));
      log.Loan.TriggerCalculation("LOCKRATE.LASTACTIONTIME", log.Loan.GetField("LOCKRATE.LASTACTIONTIME"));
      log.Loan.TriggerCalculation("LOCKRATE.DENIALCOUNT", log.Loan.GetField("LOCKRATE.DENIALCOUNT"));
    }

    public override DateTime GetSortDate() => this.DateTimeDenied;

    public DateTime DateTimeDenied
    {
      get => DateTime.Parse(this.Date.ToString("MM/dd/yyyy") + " " + this.timeDenied);
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (this.Log.GetLastLockAlertLog() != this)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(19, "Lock denied by " + this.deniedByFullName, "", this.Date, this.RequestGUID, this.Guid)
      };
    }

    public string TimeDenied
    {
      get => this.timeDenied;
      set
      {
        this.timeDenied = value;
        this.MarkAsDirty();
      }
    }

    public string DeniedBy => this.deniedBy;

    public string DeniedByFullName => this.deniedByFullName;

    public void SetDenyingUser(UserInfo user)
    {
      this.deniedBy = user.Userid;
      this.deniedByFullName = user.FullName;
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
