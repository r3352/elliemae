// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlert
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogAlert : ILogAlert
  {
    private LogEntry entry;
    private LogAlert alert;
    private Role alertRole;

    internal LogAlert(LogEntry e, LogAlert alert)
    {
      this.entry = e;
      this.alert = alert;
    }

    public DateTime DueDate
    {
      get => this.alert.DueDate;
      set => this.alert.DueDate = value;
    }

    public object FollowupDate
    {
      get
      {
        return !(this.alert.FollowedUpDate == DateTime.MinValue) ? (object) this.alert.FollowedUpDate : (object) null;
      }
      set
      {
        this.alert.FollowedUpDate = value == null ? DateTime.MinValue : Convert.ToDateTime(value);
      }
    }

    public Role AlertRole
    {
      get
      {
        if (this.alert.RoleId <= 0)
          return (Role) null;
        if (this.alertRole != null)
          return this.alertRole;
        RoleInfo roleFunction = this.entry.Loan.WorkflowManager.GetRoleFunction(this.alert.RoleId);
        if (roleFunction == null)
          return (Role) null;
        this.alertRole = new Role(this.entry.Loan.Session, roleFunction);
        return this.alertRole;
      }
    }

    public AlertState AlertState
    {
      get
      {
        if (this.alert.IsFollowedUp)
          return AlertState.Complete;
        return DateTime.Now >= this.DueDate ? AlertState.Overdue : AlertState.Pending;
      }
    }

    internal LogAlert Unwrap() => this.alert;
  }
}
