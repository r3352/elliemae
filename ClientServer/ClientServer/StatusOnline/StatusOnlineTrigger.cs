// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.StatusOnline.StatusOnlineTrigger
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.StatusOnline
{
  [Serializable]
  public class StatusOnlineTrigger : IXmlSerializable, IFieldSearchable
  {
    private string guid;
    private string ownerID;
    private TriggerPortalType portalType;
    private string name = string.Empty;
    private string description = string.Empty;
    private TriggerUpdateType updateType;
    private TriggerReminderType reminderType;
    private TriggerRequirementType requirementType;
    private string requirementData;
    private string emailTemplate;
    private string emailTemplateOwner;
    private TriggerEmailFromType emailFromType;
    private string[] emailRecipients;
    private DateTime dateTriggered = DateTime.MinValue;
    private DateTime datePublished = DateTime.MinValue;

    public StatusOnlineTrigger(string ownerID, TriggerPortalType portalType)
      : this(System.Guid.NewGuid().ToString(), ownerID, portalType)
    {
    }

    public StatusOnlineTrigger(string guid, string ownerID, TriggerPortalType portalType)
    {
      this.guid = guid;
      this.ownerID = ownerID;
      this.portalType = portalType;
      if (ownerID == null)
        return;
      this.emailFromType = TriggerEmailFromType.Owner;
    }

    public StatusOnlineTrigger(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (Guid));
      this.ownerID = info.GetString(nameof (OwnerID));
      this.portalType = info.GetEnum<TriggerPortalType>(nameof (PortalType), TriggerPortalType.WebCenter);
      this.name = info.GetString(nameof (Name));
      this.description = info.GetString(nameof (Description), string.Empty);
      this.updateType = info.GetEnum<TriggerUpdateType>(nameof (UpdateType));
      this.reminderType = info.GetEnum<TriggerReminderType>(nameof (ReminderType));
      this.requirementType = !(info.GetString(nameof (RequirementType), string.Empty) == "CustomMilestone") ? info.GetEnum<TriggerRequirementType>(nameof (RequirementType)) : TriggerRequirementType.Milestone;
      this.requirementData = info.GetString(nameof (RequirementData));
      this.emailTemplate = info.GetString(nameof (EmailTemplate));
      this.emailTemplateOwner = info.GetString(nameof (EmailTemplateOwner));
      this.emailFromType = info.GetEnum<TriggerEmailFromType>(nameof (EmailFromType), TriggerEmailFromType.CurrentUser);
      XmlList<string> xmlList = (XmlList<string>) info.GetValue(nameof (EmailRecipients), typeof (XmlList<string>), (object) null);
      this.emailRecipients = xmlList == null ? (string[]) null : xmlList.ToArray();
      this.dateTriggered = info.GetDateTime(nameof (DateTriggered), DateTime.MinValue);
      this.datePublished = info.GetDateTime(nameof (DatePublished), DateTime.MinValue);
    }

    public string Guid
    {
      set => this.guid = value;
      get => this.guid;
    }

    public string OwnerID
    {
      set => this.ownerID = value;
      get => this.ownerID;
    }

    public TriggerPortalType PortalType => this.portalType;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public TriggerUpdateType UpdateType
    {
      get => this.updateType;
      set => this.updateType = value;
    }

    public TriggerReminderType ReminderType
    {
      get => this.reminderType;
      set => this.reminderType = value;
    }

    public TriggerRequirementType RequirementType
    {
      get => this.requirementType;
      set => this.requirementType = value;
    }

    public string RequirementData
    {
      get => this.requirementData;
      set => this.requirementData = value;
    }

    public string EmailTemplate
    {
      get => this.emailTemplate;
      set => this.emailTemplate = value;
    }

    public string EmailTemplateOwner
    {
      get => this.emailTemplateOwner;
      set => this.emailTemplateOwner = value;
    }

    public TriggerEmailFromType EmailFromType
    {
      get => this.emailFromType;
      set => this.emailFromType = value;
    }

    public string[] EmailRecipients
    {
      get => this.emailRecipients;
      set => this.emailRecipients = value;
    }

    public DateTime DateTriggered
    {
      get => this.dateTriggered;
      set => this.dateTriggered = value;
    }

    public DateTime DatePublished
    {
      get => this.datePublished;
      set => this.datePublished = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Guid", (object) this.guid);
      info.AddValue("OwnerID", (object) this.ownerID);
      info.AddValue("PortalType", (object) this.portalType);
      info.AddValue("Name", (object) this.name);
      info.AddValue("Description", (object) this.description);
      info.AddValue("UpdateType", (object) this.updateType);
      info.AddValue("ReminderType", (object) this.reminderType);
      info.AddValue("RequirementType", (object) this.requirementType);
      info.AddValue("RequirementData", (object) this.requirementData);
      info.AddValue("EmailTemplate", (object) this.emailTemplate);
      info.AddValue("EmailTemplateOwner", (object) this.emailTemplateOwner);
      info.AddValue("EmailFromType", (object) this.emailFromType);
      if (this.emailRecipients != null)
        info.AddValue("EmailRecipients", (object) new XmlList<string>((IEnumerable<string>) this.emailRecipients));
      else
        info.AddValue("EmailRecipients", (object) null);
      if (this.dateTriggered != DateTime.MinValue)
        info.AddValue("DateTriggered", (object) this.dateTriggered);
      if (!(this.datePublished != DateTime.MinValue))
        return;
      info.AddValue("DatePublished", (object) this.datePublished);
    }

    public override int GetHashCode() => this.guid.GetHashCode();

    public static bool operator ==(StatusOnlineTrigger o1, StatusOnlineTrigger o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(StatusOnlineTrigger o1, StatusOnlineTrigger o2)
    {
      return !object.Equals((object) o1, (object) o2);
    }

    public override bool Equals(object obj)
    {
      StatusOnlineTrigger statusOnlineTrigger = obj as StatusOnlineTrigger;
      return statusOnlineTrigger != (StatusOnlineTrigger) null && this.guid == statusOnlineTrigger.guid;
    }

    public StatusOnlineTrigger Clone(string ownerID, TriggerPortalType portalType)
    {
      return new StatusOnlineTrigger(ownerID, portalType)
      {
        name = this.name,
        description = this.description,
        updateType = this.updateType,
        reminderType = this.reminderType,
        requirementType = this.requirementType,
        requirementData = this.requirementData,
        emailTemplate = this.emailTemplate,
        emailTemplateOwner = this.emailTemplateOwner,
        emailFromType = this.emailFromType,
        emailRecipients = this.emailRecipients
      };
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      if (this.requirementType == TriggerRequirementType.Milestone)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "MS.STATUS");
      else if (this.requirementType == TriggerRequirementType.Fields)
      {
        string[] strArray = this.requirementData.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, strArray[index]);
        strArray = (string[]) null;
      }
    }
  }
}
