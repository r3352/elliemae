// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts.AlertChange
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts
{
  public class AlertChange
  {
    public string AlertTypeId { get; set; }

    public string Name { get; set; }

    public AlertType Type { get; set; }

    public AlertStatus Status { get; set; }

    public DateTime AlertDate { get; set; }

    public DateTime DisplayDate { get; set; }

    public string Message { get; set; }

    public List<EntityRef> Associations { get; set; }

    public static AlertChange GetAlertChange(
      PipelineInfo.Alert alert,
      AlertConfig alertConfig,
      AlertStatus alertStatus,
      Dictionary<string, EllieMae.EMLite.Workflow.Milestone> milestones,
      string userName)
    {
      List<EntityRef> entityRefList = new List<EntityRef>();
      if (!string.IsNullOrEmpty(alert.MilestoneID))
        entityRefList.Add(new EntityRef()
        {
          EntityId = alert.MilestoneID,
          EntityName = milestones[alert.MilestoneID].Name,
          EntityType = EntityRefType.Milestone.ToString()
        });
      if (!string.IsNullOrEmpty(alert.UserID))
        entityRefList.Add(new EntityRef()
        {
          EntityId = alert.UserID,
          EntityName = userName,
          EntityType = EntityRefType.Milestone.ToString()
        });
      return new AlertChange()
      {
        AlertTypeId = alert.MilestoneID != null ? alertConfig.Definition.Category.ToString().Replace(" ", string.Empty) + ":" + milestones[alert.MilestoneID].Name.Replace(" ", string.Empty) + alertConfig.Definition.Name.Replace(" ", string.Empty) : alertConfig.Definition.Category.ToString().Replace(" ", string.Empty) + ":" + alertConfig.Definition.Name.Replace(" ", string.Empty),
        Name = alertConfig.Definition.Name,
        Type = (AlertType) alertConfig.Definition.Category,
        Status = alertStatus,
        Message = alertConfig.Message,
        AlertDate = alert.Date.ToUniversalTime(),
        DisplayDate = alertConfig.AlertTiming == AlertTiming.DaysBefore ? alert.Date.AddDays((double) -alertConfig.DaysBefore).ToUniversalTime() : alert.Date.ToUniversalTime(),
        Associations = entityRefList
      };
    }
  }
}
