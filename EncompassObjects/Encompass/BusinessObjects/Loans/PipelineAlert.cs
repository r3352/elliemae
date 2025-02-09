// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents an alert on a loan in the Pipeline.</summary>
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

    /// <summary>Gets the type of alert represented by this object.</summary>
    public AlertType Type => (AlertType) this.alert.AlertID;

    /// <summary>Gets the source of the event.</summary>
    /// <remarks>The source of the event will depend on the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert.Type" /> of
    /// event represented by this object. For example, a Milestone event's source
    /// will be the name of the Milestone which has triggered an event.</remarks>
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

    /// <summary>Gets the status of the event.</summary>
    /// <remarks>The status of the event will depend on the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.PipelineAlert.Type" /> of
    /// event represented by this object.</remarks>
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

    /// <summary>Gets the target role for an alert</summary>
    /// <remarks>
    /// The target of an alert is the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" /> which is intended to handle the alert
    /// and resolve it. For example, for a reminder alert, it is the role that should
    /// take action on the reminder. This property may be <c>null</c> for alerts which do not
    /// target a specific role.
    /// </remarks>
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

    /// <summary>Gets the date of the triggering event, if any.</summary>
    /// <remarks>The alert date is the date of the event which has triggered the
    /// alert. For example, a Milestone alert's date will be the date the milestone
    /// is/was expeted to be completed.</remarks>
    public object Date
    {
      get
      {
        return this.alert.Date.Date == DateTime.MinValue.Date ? (object) null : (object) this.alert.Date;
      }
    }
  }
}
