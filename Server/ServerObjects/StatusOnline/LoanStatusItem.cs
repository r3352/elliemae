// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.LoanStatusItem
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  internal class LoanStatusItem : IXmlSerializable
  {
    private string guid;
    public string description;
    public int index;
    public LoanStatusTrigger trigger;
    public LoanStatusStateEnum state;
    public DateTime statusDate;
    public string emailTemplateGUID;
    public string emailTemplateOwnerID;

    public LoanStatusItem()
    {
      this.guid = Guid.NewGuid().ToString();
      this.description = string.Empty;
      this.index = -1;
      this.trigger = new LoanStatusTrigger();
      this.state = LoanStatusStateEnum.NotReached;
      this.statusDate = DateTime.Now;
      this.emailTemplateGUID = string.Empty;
      this.emailTemplateOwnerID = string.Empty;
    }

    public LoanStatusItem(XmlSerializationInfo info)
    {
      this.guid = info.GetString("GUID");
      if (string.IsNullOrEmpty(this.guid))
        this.guid = Guid.NewGuid().ToString();
      this.description = info.GetString(nameof (description));
      this.index = info.GetInteger(nameof (index));
      this.trigger = (LoanStatusTrigger) info.GetValue(nameof (trigger), typeof (LoanStatusTrigger));
      this.statusDate = (DateTime) info.GetValue(nameof (statusDate), typeof (DateTime));
      this.state = (LoanStatusStateEnum) info.GetValue(nameof (state), typeof (LoanStatusStateEnum));
      this.emailTemplateGUID = info.GetString(nameof (emailTemplateGUID), string.Empty);
      this.emailTemplateOwnerID = info.GetString(nameof (emailTemplateOwnerID), string.Empty);
    }

    public StatusOnlineTrigger MigrateData(
      string ownerID,
      LoanData loanData,
      string[] recipientList)
    {
      StatusOnlineTrigger statusOnlineTrigger = new StatusOnlineTrigger(this.guid, ownerID, TriggerPortalType.WebCenter);
      statusOnlineTrigger.Name = this.description;
      switch (this.trigger.TriggerType)
      {
        case LoanStatusTriggerEnum.None:
          statusOnlineTrigger.RequirementType = TriggerRequirementType.None;
          statusOnlineTrigger.RequirementData = (string) null;
          break;
        case LoanStatusTriggerEnum.CustomMilestone:
          statusOnlineTrigger.RequirementType = TriggerRequirementType.Milestone;
          statusOnlineTrigger.RequirementData = this.trigger.TriggerId;
          break;
        case LoanStatusTriggerEnum.CoreMilestone:
          statusOnlineTrigger.RequirementType = TriggerRequirementType.Milestone;
          statusOnlineTrigger.RequirementData = Milestone.GetCoreMilestoneID(this.trigger.TriggerId);
          break;
        case LoanStatusTriggerEnum.Document:
          statusOnlineTrigger.RequirementType = TriggerRequirementType.DocumentTemplate;
          statusOnlineTrigger.RequirementData = this.trigger.TriggerId;
          break;
        case LoanStatusTriggerEnum.MilestoneLog:
          MilestoneLog milestone = loanData.GetLogList().GetMilestone(this.trigger.TriggerId);
          if (milestone != null)
          {
            statusOnlineTrigger.RequirementType = TriggerRequirementType.MilestoneLog;
            statusOnlineTrigger.RequirementData = milestone.MilestoneID;
            break;
          }
          statusOnlineTrigger.RequirementType = TriggerRequirementType.None;
          statusOnlineTrigger.RequirementData = (string) null;
          break;
        case LoanStatusTriggerEnum.DocumentLog:
          VerifLog verif = loanData.GetLogList().GetVerif(this.trigger.TriggerId);
          if (verif != null)
          {
            statusOnlineTrigger.RequirementType = TriggerRequirementType.DocumentLog;
            statusOnlineTrigger.RequirementData = verif.Guid;
            break;
          }
          statusOnlineTrigger.RequirementType = TriggerRequirementType.DocumentName;
          statusOnlineTrigger.RequirementData = this.trigger.TriggerId;
          break;
      }
      statusOnlineTrigger.UpdateType = TriggerUpdateType.Manual;
      statusOnlineTrigger.ReminderType = statusOnlineTrigger.RequirementType == TriggerRequirementType.None ? TriggerReminderType.NoReminder : TriggerReminderType.RemindOnExit;
      if (!string.IsNullOrEmpty(this.emailTemplateGUID) && !string.IsNullOrEmpty(this.emailTemplateOwnerID))
      {
        statusOnlineTrigger.EmailTemplate = this.emailTemplateGUID;
        statusOnlineTrigger.EmailTemplateOwner = this.emailTemplateOwnerID;
        statusOnlineTrigger.EmailFromType = TriggerEmailFromType.CurrentUser;
        statusOnlineTrigger.EmailRecipients = recipientList;
      }
      if (this.state != LoanStatusStateEnum.NotReached)
        statusOnlineTrigger.DateTriggered = this.statusDate;
      if (this.state == LoanStatusStateEnum.Published)
        statusOnlineTrigger.DatePublished = this.statusDate;
      return statusOnlineTrigger;
    }

    public void GetXmlObjectData(XmlSerializationInfo info) => throw new NotSupportedException();
  }
}
