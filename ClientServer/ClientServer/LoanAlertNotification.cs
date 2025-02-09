// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanAlertNotification
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanAlertNotification : UserNotification
  {
    private PipelineInfo pinfo;
    private PipelineInfo.Alert alert;
    private UserInfo sender;

    public LoanAlertNotification(
      string userId,
      PipelineInfo pinfo,
      PipelineInfo.Alert alert,
      UserInfo sender)
      : base(userId, alert.Date)
    {
      this.pinfo = pinfo;
      this.alert = alert;
      this.sender = sender;
    }

    public string LoanGuid => this.pinfo.GUID;

    public PipelineInfo PipelineInfo => this.pinfo;

    public PipelineInfo.Alert Alert => this.alert;

    public UserInfo Sender => this.sender;

    public override string ToString()
    {
      return "Alert -> Recipient = " + this.UserID + ", Loan = " + this.LoanGuid + ", Type = " + (object) (StandardAlertID) this.alert.AlertID + ", Sender = " + (object) this.Sender;
    }
  }
}
