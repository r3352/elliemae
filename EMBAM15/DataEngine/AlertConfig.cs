// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertConfig
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertConfig : ICloneable, IFieldSearchable
  {
    protected AlertDefinition alertDef;
    protected string message = "";
    protected int daysBefore;
    protected bool alertEnabled;
    protected bool notificationEnabled;
    protected List<string> milestoneGuidList;
    protected List<int> notificationRoleList;
    protected List<string> notificationUserList;
    protected List<string> triggerFieldList;

    public bool UseApplicationDate { get; set; }

    public bool DoNoShowDeclinedConsent { get; set; }

    public AlertConfig(
      AlertDefinition alertDef,
      string message,
      int daysBefore,
      bool alertEnabled,
      bool notificationEnabled,
      List<string> milestoneGuidList,
      List<int> notificationRoleList,
      List<string> notificationUserList,
      List<string> triggerFieldList)
    {
      this.alertDef = alertDef;
      this.message = message;
      this.daysBefore = daysBefore;
      this.alertEnabled = alertEnabled;
      this.notificationEnabled = notificationEnabled;
      this.milestoneGuidList = milestoneGuidList;
      this.notificationRoleList = notificationRoleList;
      this.notificationUserList = notificationUserList;
      this.triggerFieldList = triggerFieldList;
    }

    public AlertConfig()
    {
      this.alertDef = (AlertDefinition) new CustomAlert();
      this.milestoneGuidList = new List<string>();
      this.notificationRoleList = new List<int>();
      this.notificationUserList = new List<string>();
      this.triggerFieldList = new List<string>();
    }

    public int AlertID => this.alertDef.AlertID;

    public AlertDefinition Definition => this.alertDef;

    public AlertTiming AlertTiming => this.alertDef.GetAlertTiming(this);

    public string Message
    {
      get => this.alertDef.SupportsCustomMessage ? this.message : (string) null;
      set => this.message = value;
    }

    public int DaysBefore
    {
      get => this.AlertTiming == AlertTiming.DaysBefore ? this.daysBefore : -1;
      set => this.daysBefore = value;
    }

    public bool AlertEnabled
    {
      get => this.alertEnabled;
      set => this.alertEnabled = value;
    }

    public bool NotificationEnabled
    {
      get
      {
        return this.Definition.NotificationType != AlertNotificationType.None && this.notificationEnabled;
      }
      set => this.notificationEnabled = value;
    }

    public List<string> MilestoneGuidList => this.milestoneGuidList;

    public List<string> NotificationUserList
    {
      get
      {
        return this.Definition.NotificationType == AlertNotificationType.Configurable ? this.notificationUserList : (List<string>) null;
      }
    }

    public List<int> NotificationRoleList
    {
      get
      {
        return this.Definition.NotificationType == AlertNotificationType.Configurable ? this.notificationRoleList : (List<int>) null;
      }
    }

    public List<string> TriggerFieldList => this.triggerFieldList;

    public void AddMilestone(string guid)
    {
      if (this.milestoneGuidList.Contains(guid))
        return;
      this.milestoneGuidList.Add(guid);
    }

    public object Clone()
    {
      return (object) new AlertConfig((AlertDefinition) this.alertDef.Clone(), this.message, this.daysBefore, this.alertEnabled, this.notificationEnabled, new List<string>((IEnumerable<string>) this.milestoneGuidList), new List<int>((IEnumerable<int>) this.notificationRoleList), new List<string>((IEnumerable<string>) this.notificationUserList), new List<string>((IEnumerable<string>) this.triggerFieldList));
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      if (this.alertEnabled)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, "MS.STATUS");
      if (this.triggerFieldList != null)
      {
        foreach (string triggerField in this.triggerFieldList)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, triggerField);
      }
      if (this.alertDef.Category == AlertCategory.Custom)
      {
        CustomAlert alertDef = (CustomAlert) this.alertDef;
        if (alertDef.ConditionXml != null && "" != alertDef.ConditionXml)
        {
          foreach (KeyValuePair<RelationshipType, string> field in this.ConvertXMLToObject(alertDef.ConditionXml))
            yield return field;
        }
      }
      else if (this.alertDef.Category == AlertCategory.Regulation)
      {
        RegulationAlert alertDef = (RegulationAlert) this.alertDef;
        if (alertDef.TriggerFields != null)
        {
          foreach (AlertTriggerField triggerField in (List<AlertTriggerField>) alertDef.TriggerFields)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, triggerField.FieldID);
        }
      }
    }

    private IEnumerable<KeyValuePair<RelationshipType, string>> ConvertXMLToObject(string xmlData)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(xmlData);
      foreach (XmlNode selectNode in xmlDocument.SelectNodes("objdata/element/element/element[@name='fieldID']"))
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, selectNode.InnerText);
    }
  }
}
