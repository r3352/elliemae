// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanAlertMonitor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class LoanAlertMonitor : IAlertMonitor
  {
    private const string className = "LoanAlertMonitor�";
    private static readonly string sw = Tracing.SwDataEngine;
    private UserInfo currentUser;
    private BusinessCalendar businessCalendar;
    private BusinessCalendar postalCalendar;
    private AlertConfig[] alertConfigs;
    private Dictionary<int, FilterEvaluator> conditionEvaluators = new Dictionary<int, FilterEvaluator>();
    private LoanData monitoredLoan;
    private Dictionary<string, bool> registeredFields;

    public event EventHandler AlertActivation;

    public LoanAlertMonitor(SessionObjects sessionObjects, LoanData loan)
    {
      Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Info, "Attaching LoanAlertMonitor to LoanData object");
      this.currentUser = sessionObjects.UserInfo;
      this.monitoredLoan = loan;
      this.alertConfigs = loan.Settings.AlertSetupData.AlertConfigList;
      this.registeredFields = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (AlertConfig alertConfig in this.alertConfigs)
      {
        if (alertConfig.Definition is CustomAlert)
        {
          CustomAlert definition = (CustomAlert) alertConfig.Definition;
          if (alertConfig.TriggerFieldList.Count > 0 && definition.DateAdjustment != 0)
          {
            if (definition.AdjustmentDayType == DayType.Business && this.businessCalendar == null)
              this.businessCalendar = sessionObjects.GetBusinessCalendar(CalendarType.Business);
            else if (definition.AdjustmentDayType == DayType.Postal && this.postalCalendar == null)
              this.postalCalendar = sessionObjects.GetBusinessCalendar(CalendarType.Postal);
          }
          foreach (string triggerField in alertConfig.TriggerFieldList)
          {
            if (!this.registeredFields.ContainsKey(triggerField))
            {
              Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Verbose, "Attaching trigger field monitor for field '" + triggerField + "'");
              this.monitoredLoan.RegisterCustomFieldValueChangeEventHandler(triggerField, new Routine(this.onAlertTriggerFieldChange));
              this.registeredFields[triggerField] = true;
            }
          }
          if ((definition.ConditionXml ?? "") != "")
          {
            FieldFilterList fieldFilterList = FieldFilterList.Parse(definition.ConditionXml);
            foreach (FieldFilter filter in (List<FieldFilter>) fieldFilterList)
            {
              this.updateOptionValueList(filter, loan);
              if (!this.registeredFields.ContainsKey(filter.FieldID))
              {
                Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Verbose, "Attaching condition field monitor for field '" + filter.FieldID + "'");
                this.monitoredLoan.RegisterCustomFieldValueChangeEventHandler(filter.FieldID, new Routine(this.onAlertTriggerFieldChange));
                this.registeredFields[filter.FieldID] = true;
              }
            }
            this.conditionEvaluators[alertConfig.AlertID] = fieldFilterList.CreateEvaluator();
          }
        }
      }
      loan.AttachAlertMonitor((IAlertMonitor) this);
      this.ActivateAlerts(this.monitoredLoan, this.currentUser);
    }

    public LoanAlertMonitor(
      UserInfo userInfo,
      LoanData loan,
      BusinessCalendar businessCalendar,
      BusinessCalendar postalCalendar)
    {
      Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Info, "Attaching LoanAlertMonitor to LoanData object");
      this.currentUser = userInfo;
      this.monitoredLoan = loan;
      this.alertConfigs = loan.Settings.AlertSetupData.AlertConfigList;
      this.registeredFields = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (AlertConfig alertConfig in this.alertConfigs)
      {
        if (alertConfig.Definition is CustomAlert)
        {
          CustomAlert definition = (CustomAlert) alertConfig.Definition;
          if (alertConfig.TriggerFieldList.Count > 0 && definition.DateAdjustment != 0)
          {
            if (definition.AdjustmentDayType == DayType.Business && this.businessCalendar == null)
              this.businessCalendar = businessCalendar;
            else if (definition.AdjustmentDayType == DayType.Postal && this.postalCalendar == null)
              this.postalCalendar = postalCalendar;
          }
          foreach (string triggerField in alertConfig.TriggerFieldList)
          {
            if (!this.registeredFields.ContainsKey(triggerField))
            {
              Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Verbose, "Attaching trigger field monitor for field '" + triggerField + "'");
              this.monitoredLoan.RegisterCustomFieldValueChangeEventHandler(triggerField, new Routine(this.onAlertTriggerFieldChange));
              this.registeredFields[triggerField] = true;
            }
          }
          if ((definition.ConditionXml ?? "") != "")
          {
            FieldFilterList fieldFilterList = FieldFilterList.Parse(definition.ConditionXml);
            foreach (FieldFilter filter in (List<FieldFilter>) fieldFilterList)
            {
              this.updateOptionValueList(filter, loan);
              if (!this.registeredFields.ContainsKey(filter.FieldID))
              {
                Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Verbose, "Attaching condition field monitor for field '" + filter.FieldID + "'");
                this.monitoredLoan.RegisterCustomFieldValueChangeEventHandler(filter.FieldID, new Routine(this.onAlertTriggerFieldChange));
                this.registeredFields[filter.FieldID] = true;
              }
            }
            this.conditionEvaluators[alertConfig.AlertID] = fieldFilterList.CreateEvaluator();
          }
        }
      }
      loan.AttachAlertMonitor((IAlertMonitor) this);
      this.ActivateAlerts(this.monitoredLoan, this.currentUser);
    }

    public LoanAlertMonitor(
      BusinessCalendar businessCalendar,
      BusinessCalendar postalCalendar,
      AlertSetupData alertSetupData)
    {
      this.businessCalendar = businessCalendar;
      this.postalCalendar = postalCalendar;
      this.alertConfigs = alertSetupData.AlertConfigList;
      foreach (AlertConfig alertConfig in this.alertConfigs)
      {
        if (alertConfig.Definition is CustomAlert)
        {
          CustomAlert definition = (CustomAlert) alertConfig.Definition;
          if ((definition.ConditionXml ?? "") != "")
          {
            FieldFilterList fieldFilterList = FieldFilterList.Parse(definition.ConditionXml);
            this.conditionEvaluators[alertConfig.AlertID] = fieldFilterList.CreateEvaluator();
          }
        }
      }
    }

    public PipelineInfo.Alert[] GetAlerts(LoanData loan)
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      foreach (AlertConfig alertConfig in this.alertConfigs)
      {
        if (alertConfig.Definition is CustomAlert)
        {
          PipelineInfo.Alert alert = this.GetAlert(loan, alertConfig);
          if (alert != null)
            alertList.Add(alert);
        }
      }
      return alertList.ToArray();
    }

    public PipelineInfo.Alert GetAlert(LoanData loan, AlertConfig alertConfig)
    {
      if (alertConfig.Definition.Category != AlertCategory.Custom)
        throw new ArgumentException("The specified method can only be called for Custom Alerts");
      CustomAlert definition = (CustomAlert) alertConfig.Definition;
      AlertStatus alertStatus = loan.GetAlertStatus(definition.Guid);
      if (!alertStatus.Active)
        return (PipelineInfo.Alert) null;
      DateTime date1 = alertStatus.ActivationTime;
      if (alertConfig.TriggerFieldList.Count > 0)
      {
        DateTime date2 = Utils.ParseDate((object) loan.GetSimpleField(alertConfig.TriggerFieldList[0]), DateTime.MinValue);
        if (date2 == DateTime.MinValue)
          return (PipelineInfo.Alert) null;
        date1 = this.applyDateAdjustment(date2, definition);
      }
      return new PipelineInfo.Alert(alertConfig.AlertID, "", "", date1, definition.Guid, (string) null);
    }

    private DateTime applyDateAdjustment(DateTime date, CustomAlert alertDef)
    {
      try
      {
        if (alertDef.DateAdjustment == 0)
          return date;
        if (alertDef.AdjustmentDayType == DayType.Business)
          return this.businessCalendar.AddBusinessDays(date, alertDef.DateAdjustment, false);
        return alertDef.AdjustmentDayType == DayType.Postal ? this.postalCalendar.AddBusinessDays(date, alertDef.DateAdjustment, false) : date.AddDays((double) alertDef.DateAdjustment);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Error, "Failed to apply date adjustment to date '" + date.ToShortDateString() + "' for alert '" + alertDef.Name + "': " + ex.Message);
        return date.AddDays((double) alertDef.DateAdjustment);
      }
    }

    private void onAlertTriggerFieldChange(string fieldId, string val)
    {
      Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Verbose, "Alert activation field modification detected on field '" + fieldId + "'.");
      this.ActivateAlerts();
    }

    public void ActivateAlerts()
    {
      this.ActivateAlerts(this.monitoredLoan, this.currentUser);
      if (this.AlertActivation == null)
        return;
      this.AlertActivation((object) this, EventArgs.Empty);
    }

    public void ActivateAlerts(LoanData loan, UserInfo user)
    {
      Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Info, "Evaluating alerts on loan '" + loan.GUID + "'");
      foreach (AlertConfig alertConfig in this.alertConfigs)
      {
        if (alertConfig.Definition is CustomAlert)
        {
          CustomAlert definition = (CustomAlert) alertConfig.Definition;
          AlertStatus alertStatus = loan.GetAlertStatus(definition.Guid);
          bool flag = true;
          if (this.conditionEvaluators.ContainsKey(definition.AlertID))
          {
            FilterEvaluator conditionEvaluator = this.conditionEvaluators[definition.AlertID];
            flag &= conditionEvaluator.Evaluate(loan, FilterEvaluationOption.None);
          }
          if (alertConfig.TriggerFieldList.Count > 0)
          {
            DateTime date = Utils.ParseDate((object) loan.GetSimpleField(alertConfig.TriggerFieldList[0]), DateTime.MinValue);
            flag &= date != DateTime.MinValue;
          }
          Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Verbose, "Alert '" + alertConfig.Definition.Name + "' is now " + (flag ? "Active" : "Inactive") + ". Previous state = " + (alertStatus.Active ? "Active" : (alertStatus.Dismissed ? "Dismissed" : "Not Activated")));
          if (flag && !alertStatus.Activated)
          {
            loan.ActivateAlert(alertStatus.AlertID, user == (UserInfo) null ? "" : user.Userid, DateTime.Now);
            Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Info, "Alert '" + alertConfig.Definition.Name + "' has been activated by user '" + user.Userid + "'");
          }
          else if (!flag && alertStatus.Activated)
          {
            loan.DeactivateAlert(alertStatus.AlertID);
            Tracing.Log(LoanAlertMonitor.sw, nameof (LoanAlertMonitor), TraceLevel.Info, "Alert '" + alertConfig.Definition.Name + "' has been deactivated");
          }
        }
      }
    }

    private void updateOptionValueList(FieldFilter filter, LoanData loan)
    {
      if (filter.FieldType != EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList)
        return;
      List<string> values = new List<string>();
      FieldDefinition fieldDefinition = loan.GetFieldDefinition(filter.FieldID);
      string valueFrom = filter.ValueFrom;
      char[] separator = new char[1]{ ';' };
      foreach (string strA in valueFrom.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        foreach (FieldOption option in fieldDefinition.Options)
        {
          if (string.Compare(strA, option.ReportingDatabaseValue, true) == 0)
          {
            if (!string.IsNullOrEmpty(option.Value) || filter.FieldID == "1393")
              values.Add(option.Value);
            if (!values.Contains(option.ReportingDatabaseValue))
              values.Add(option.ReportingDatabaseValue);
          }
        }
      }
      filter.ValueFrom = string.Join(";", (IEnumerable<string>) values);
    }
  }
}
