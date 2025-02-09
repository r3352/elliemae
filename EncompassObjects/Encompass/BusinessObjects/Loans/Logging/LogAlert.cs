// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogAlert
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Summary description for LogAlert.</summary>
  public class LogAlert : ILogAlert
  {
    private LogEntry entry;
    private EllieMae.EMLite.DataEngine.Log.LogAlert alert;
    private Role alertRole;

    internal LogAlert(LogEntry e, EllieMae.EMLite.DataEngine.Log.LogAlert alert)
    {
      this.entry = e;
      this.alert = alert;
    }

    /// <summary>
    /// Gets or sets the date at which this alert will be considered past due.
    /// </summary>
    public DateTime DueDate
    {
      get => this.alert.DueDate;
      set => this.alert.DueDate = value;
    }

    /// <summary>
    /// Gets or sets the date on which the alert was followed up.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the Role of the user who should be alerted.
    /// </summary>
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

    /// <summary>Gets the current state of the alert.</summary>
    public AlertState AlertState
    {
      get
      {
        if (this.alert.IsFollowedUp)
          return AlertState.Complete;
        return DateTime.Now >= this.DueDate ? AlertState.Overdue : AlertState.Pending;
      }
    }

    internal EllieMae.EMLite.DataEngine.Log.LogAlert Unwrap() => this.alert;
  }
}
