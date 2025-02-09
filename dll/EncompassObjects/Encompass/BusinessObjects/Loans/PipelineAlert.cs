// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class PipelineAlert : SessionBoundObject, IPipelineAlert
  {
    private PipelineInfo pinfo;
    private PipelineInfo.Alert alert;
    private Role role;

    internal PipelineAlert(Session session, PipelineInfo pinfo, PipelineInfo.Alert alert)
      : base(session)
    {
      this.pinfo = pinfo;
      this.alert = alert;
    }

    public AlertType Type => (AlertType) this.alert.AlertID;

    public string Source
    {
      get
      {
        switch (this.Type)
        {
          case AlertType.Milestone:
            return string.Concat(this.pinfo.Info[(object) "NextMilestoneName"]);
          case AlertType.RateLock:
            return "Rate Lock";
          default:
            return this.alert.Event;
        }
      }
    }

    public AlertStatus Status
    {
      get
      {
        switch (this.Type)
        {
          case AlertType.Document:
          case AlertType.Condition:
            return this.alert.Status == "expected" ? AlertStatus.Due : AlertStatus.Expires;
          case AlertType.RateLock:
            return AlertStatus.Expires;
          case AlertType.DocumentExpiration:
            return AlertStatus.Expires;
          default:
            return AlertStatus.Due;
        }
      }
    }

    public Role WorkflowRole
    {
      get
      {
        if (this.role != null)
          return this.role;
        int roleId = Utils.ParseInt((object) this.alert.Status, -1);
        if (roleId <= 0)
          return (Role) null;
        this.role = this.Session.Loans.Roles.GetRoleByID(roleId);
        return this.role;
      }
    }

    public object Date
    {
      get
      {
        return this.alert.Date.Date == DateTime.MinValue.Date ? (object) null : (object) this.alert.Date;
      }
    }
  }
}
