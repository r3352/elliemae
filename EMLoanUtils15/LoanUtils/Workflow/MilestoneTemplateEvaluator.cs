// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.MilestoneTemplateEvaluator
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class MilestoneTemplateEvaluator
  {
    private List<LogRecordBase> _currentLogRecords;
    private readonly IEnumerable<EllieMae.EMLite.Workflow.Milestone> _allMilestones;
    private readonly IEnumerable<RoleInfo> _allRoles;
    private readonly List<MilestoneTemplateMilestoneInfo> _currentMilestones;
    private readonly List<MilestoneTemplateMilestoneInfo> _newMilestones;

    public bool IsTemplateChanged { get; private set; }

    public bool NotifyUsersGlobalSetting { get; private set; }

    public int DifferenceIndex { get; private set; }

    public List<MilestoneTemplateMilestoneInfo> CurrentMilestones => this._currentMilestones;

    public List<MilestoneTemplateMilestoneInfo> NewMilestones => this._newMilestones;

    public List<MilestoneTemplateMilestoneInfo> RemovedMilestones { get; private set; }

    public List<MilestoneTemplateMilestoneUserInfo> RemovedUsers { get; internal set; }

    public MilestoneTemplateEvaluator(
      SessionObjects sessionObjects,
      MilestoneTemplate currentTemplate,
      MilestoneTemplate newTemplate,
      MilestoneLog[] currentMilestoneLogs,
      int differnceIndex,
      IEnumerable<RoleInfo> roles)
    {
      this._currentLogRecords = new List<LogRecordBase>((IEnumerable<LogRecordBase>) currentMilestoneLogs);
      this._allRoles = roles;
      this._allMilestones = sessionObjects.BpmManager.GetMilestones(false);
      this.DifferenceIndex = differnceIndex;
      this.IsTemplateChanged = !newTemplate.Equals(currentTemplate);
      this._currentMilestones = this.GetTemplateMilestoneInfo(currentTemplate, currentMilestoneLogs);
      this._newMilestones = this.GetTemplateMilestoneInfo(newTemplate);
      this.ComputeEstimatedDates((IReadOnlyCollection<MilestoneLog>) currentMilestoneLogs, newTemplate.SequentialMilestones, sessionObjects);
      this.RemovedMilestones = this._currentMilestones.Where<MilestoneTemplateMilestoneInfo>((Func<MilestoneTemplateMilestoneInfo, bool>) (x => !this._newMilestones.Any<MilestoneTemplateMilestoneInfo>((Func<MilestoneTemplateMilestoneInfo, bool>) (y => y.MilestoneId.Equals(x.MilestoneId, StringComparison.OrdinalIgnoreCase))))).ToList<MilestoneTemplateMilestoneInfo>();
      this.NotifyUsersGlobalSetting = string.IsNullOrWhiteSpace(sessionObjects.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification")) ? (bool) sessionObjects.ServerManager.GetServerSetting("Policies.MilestoneTemplateChangeNotification") : bool.Parse(sessionObjects.ConfigurationManager.GetCompanySetting("POLICIES", "MilestoneTemplateChangeNotification"));
    }

    private void ComputeEstimatedDates(
      IReadOnlyCollection<MilestoneLog> currentMilestoneLogs,
      MilestoneTemplate.TemplateMilestones newMilestones,
      SessionObjects sessionObjects)
    {
      int i = 0;
      AutoDayCountSetting calendarType = (AutoDayCountSetting) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
      DateTime previousDate = new DateTime();
      currentMilestoneLogs.TakeWhile<MilestoneLog>((Func<MilestoneLog, int, bool>) ((item, index) => item.MilestoneID.Equals(newMilestones[index].MilestoneID))).ToList<MilestoneLog>().ForEach((Action<MilestoneLog>) (item =>
      {
        MilestoneLog currentLogRecord = (MilestoneLog) this._currentLogRecords[i];
        if (currentLogRecord.Done)
        {
          previousDate = currentLogRecord.Date;
          this._currentMilestones[i].ExpectedDate = currentLogRecord.Date;
          this._currentMilestones[i].IsCompeted = true;
        }
        else
        {
          DateTime date1 = item.Date.Date;
          DateTime dateTime = DateTime.MaxValue;
          DateTime date2 = dateTime.Date;
          if (date1 != date2)
          {
            dateTime = item.Date;
            DateTime date3 = dateTime.Date;
            dateTime = DateTime.MinValue;
            DateTime date4 = dateTime.Date;
            if (date3 != date4)
            {
              previousDate = item.Date;
              goto label_6;
            }
          }
          previousDate = MilestoneTemplateEvaluator.AddDays(previousDate, currentLogRecord.Days, calendarType, sessionObjects);
label_6:
          this._currentMilestones[i].ExpectedDate = previousDate;
        }
        this._newMilestones[i].IsEffected = false;
        ++i;
      }));
      DateTime dateTime1;
      for (int index = 0; index < i; ++index)
      {
        if (index >= 1 && index == i - 1)
        {
          if (((MilestoneLog) this._currentLogRecords[index]).Done)
          {
            MilestoneTemplateMilestoneInfo newMilestone = this._newMilestones[index];
            previousDate = dateTime1 = DateTime.Now;
            DateTime dateTime2 = dateTime1;
            newMilestone.ExpectedDate = dateTime2;
          }
          else
          {
            DateTime expectedDate = this._currentMilestones[index].ExpectedDate;
            dateTime1 = this._currentMilestones[index].ExpectedDate;
            DateTime date5 = dateTime1.Date;
            dateTime1 = DateTime.MaxValue;
            DateTime date6 = dateTime1.Date;
            if (date5 != date6)
            {
              dateTime1 = this._currentMilestones[index].ExpectedDate;
              DateTime date7 = dateTime1.Date;
              dateTime1 = DateTime.MinValue;
              DateTime date8 = dateTime1.Date;
              if (date7 != date8)
              {
                this._newMilestones[index].ExpectedDate = this._currentMilestones[index].ExpectedDate;
                continue;
              }
            }
            this._newMilestones[index].ExpectedDate = this._currentLogRecords[index].Date;
          }
        }
        else
          this._newMilestones[index] = this._currentMilestones[index];
      }
      for (int index = i; index < newMilestones.Count(); ++index)
      {
        this._newMilestones[index].IsEffected = true;
        previousDate = MilestoneTemplateEvaluator.AddDays(previousDate, newMilestones[index].DaysToComplete, calendarType, sessionObjects);
        this._newMilestones[index].ExpectedDate = previousDate;
      }
      previousDate = currentMilestoneLogs.ElementAt<MilestoneLog>(i - 1).Date;
      DateTime date9 = previousDate.Date;
      dateTime1 = DateTime.MaxValue;
      DateTime date10 = dateTime1.Date;
      if (!(date9 == date10))
      {
        DateTime date11 = previousDate.Date;
        dateTime1 = DateTime.MinValue;
        DateTime date12 = dateTime1.Date;
        if (!(date11 == date12))
          goto label_17;
      }
      previousDate = this._currentMilestones[i - 1].ExpectedDate;
label_17:
      for (int index = i; index < currentMilestoneLogs.Count; ++index)
      {
        MilestoneLog milestoneLog = currentMilestoneLogs.ElementAt<MilestoneLog>(index);
        if (milestoneLog.Done)
        {
          previousDate = milestoneLog.Date;
          this._currentMilestones[index].IsCompeted = true;
        }
        else
        {
          dateTime1 = milestoneLog.Date;
          DateTime date13 = dateTime1.Date;
          dateTime1 = DateTime.MaxValue;
          DateTime date14 = dateTime1.Date;
          if (date13 != date14)
          {
            dateTime1 = milestoneLog.Date;
            DateTime date15 = dateTime1.Date;
            dateTime1 = DateTime.MinValue;
            DateTime date16 = dateTime1.Date;
            if (date15 != date16)
            {
              previousDate = milestoneLog.Date;
              goto label_24;
            }
          }
          previousDate = MilestoneTemplateEvaluator.AddDays(previousDate, milestoneLog.Days, calendarType, sessionObjects);
        }
label_24:
        this._currentMilestones[index].ExpectedDate = previousDate;
      }
    }

    private List<MilestoneTemplateMilestoneInfo> GetTemplateMilestoneInfo(
      MilestoneTemplate template,
      MilestoneLog[] milestoneLogs)
    {
      if (milestoneLogs == null)
      {
        milestoneLogs = this.GetMilestoneLogsFromMilestoneTemplate(template);
        if (milestoneLogs == null)
          return (List<MilestoneTemplateMilestoneInfo>) null;
      }
      return ((IEnumerable<MilestoneLog>) milestoneLogs).Select(mlg => new
      {
        mlg = mlg,
        m = this.GetMilestoneByMilestoneLog(mlg)
      }).Select(_param1 => new MilestoneTemplateMilestoneInfo()
      {
        MilestoneId = _param1.m.MilestoneID,
        MilestoneName = this.GetMilestoneNameById(_param1.m.MilestoneID),
        RoleId = _param1.m.RoleID,
        RoleName = this.GetRoleNameById(_param1.m.RoleID),
        DaysToComplete = _param1.mlg.Days
      }).ToList<MilestoneTemplateMilestoneInfo>();
    }

    private List<MilestoneTemplateMilestoneInfo> GetTemplateMilestoneInfo(
      MilestoneTemplate milestoneTemplate)
    {
      return milestoneTemplate.SequentialMilestones.Select<TemplateMilestone, MilestoneTemplateMilestoneInfo>((Func<TemplateMilestone, MilestoneTemplateMilestoneInfo>) (templateMilestone => new MilestoneTemplateMilestoneInfo()
      {
        MilestoneId = templateMilestone.MilestoneID,
        MilestoneName = this.GetMilestoneNameById(templateMilestone.MilestoneID),
        RoleId = templateMilestone.RoleID,
        RoleName = this.GetRoleNameById(templateMilestone.RoleID),
        DaysToComplete = templateMilestone.DaysToComplete
      })).ToList<MilestoneTemplateMilestoneInfo>();
    }

    internal List<MilestoneTemplateMilestoneUserInfo> GetUsersInfoToNotify(
      Dictionary<UserInfo, List<string>> sendEmailList,
      Dictionary<UserInfo, List<string>> dontSendEmailList)
    {
      List<MilestoneTemplateMilestoneUserInfo> list = sendEmailList.Where<KeyValuePair<UserInfo, List<string>>>((Func<KeyValuePair<UserInfo, List<string>>, bool>) (x => x.Key != (UserInfo) null)).Select<KeyValuePair<UserInfo, List<string>>, MilestoneTemplateMilestoneUserInfo>((Func<KeyValuePair<UserInfo, List<string>>, MilestoneTemplateMilestoneUserInfo>) (y => new MilestoneTemplateMilestoneUserInfo()
      {
        UserId = y.Key.Userid,
        UserName = y.Key.FullName,
        RoleInfo = this.GetRoleInfo((IEnumerable<string>) y.Value),
        SendNotification = this.NotifyUsersGlobalSetting
      })).ToList<MilestoneTemplateMilestoneUserInfo>();
      list.AddRange((IEnumerable<MilestoneTemplateMilestoneUserInfo>) dontSendEmailList.Where<KeyValuePair<UserInfo, List<string>>>((Func<KeyValuePair<UserInfo, List<string>>, bool>) (x => x.Key != (UserInfo) null)).Select<KeyValuePair<UserInfo, List<string>>, MilestoneTemplateMilestoneUserInfo>((Func<KeyValuePair<UserInfo, List<string>>, MilestoneTemplateMilestoneUserInfo>) (y => new MilestoneTemplateMilestoneUserInfo()
      {
        UserId = y.Key.Userid,
        UserName = y.Key.FullName,
        RoleInfo = this.GetRoleInfo((IEnumerable<string>) y.Value),
        SendNotification = false
      })).ToList<MilestoneTemplateMilestoneUserInfo>());
      return list;
    }

    private Dictionary<int, string> GetRoleInfo(IEnumerable<string> roles)
    {
      Dictionary<int, string> roleInfo1 = new Dictionary<int, string>();
      foreach (string role in roles)
      {
        string roleName = role;
        RoleInfo roleInfo2 = this._allRoles.FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase)));
        if (roleInfo2 != null && !roleInfo1.ContainsKey(roleInfo2.RoleID))
          roleInfo1.Add(roleInfo2.RoleID, roleName);
      }
      return roleInfo1;
    }

    private string GetMilestoneNameById(string milestoneId)
    {
      EllieMae.EMLite.Workflow.Milestone milestone = this._allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID.Equals(milestoneId, StringComparison.OrdinalIgnoreCase)));
      return milestone == null ? string.Empty : milestone.Name;
    }

    private string GetRoleNameById(int roleId)
    {
      RoleInfo roleInfo = this._allRoles.FirstOrDefault<RoleInfo>((Func<RoleInfo, bool>) (x => x.RoleID == roleId));
      return roleInfo == null ? string.Empty : roleInfo.Name;
    }

    private EllieMae.EMLite.Workflow.Milestone GetMilestoneByMilestoneLog(MilestoneLog log)
    {
      EllieMae.EMLite.Workflow.Milestone milestoneByMilestoneLog = this._allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => x.MilestoneID.Equals(log.MilestoneID, StringComparison.OrdinalIgnoreCase)));
      if (milestoneByMilestoneLog == null)
        milestoneByMilestoneLog = new EllieMae.EMLite.Workflow.Milestone(log.MilestoneID, log.SortIndex, log.RoleID)
        {
          Name = log.Stage,
          Archived = false,
          DefaultDays = log.Days,
          DescTextAfter = log.DoneText,
          DescTextBefore = log.ExpText,
          RoleRequired = log.RoleRequired == "Y"
        };
      return milestoneByMilestoneLog;
    }

    private MilestoneLog[] GetMilestoneLogsFromMilestoneTemplate(MilestoneTemplate template)
    {
      LogList logList = new LogList((LoanData) null, (string) null, (IMilestoneDateCalculator) null);
      foreach (TemplateMilestone sequentialMilestone in template.SequentialMilestones)
      {
        TemplateMilestone ms = sequentialMilestone;
        EllieMae.EMLite.Workflow.Milestone milestone = this._allMilestones.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (m => m.MilestoneID.Equals(ms.MilestoneID, StringComparison.OrdinalIgnoreCase)));
        if (milestone != null)
        {
          MilestoneLog milestoneLog = logList.AddMilestone(milestone.Name, milestone.DefaultDays, milestone.MilestoneID, milestone.TPOConnectStatus, milestone.ConsumerStatus);
          if (milestoneLog.Stage.Equals("Started", StringComparison.OrdinalIgnoreCase))
          {
            milestoneLog.Done = true;
            milestoneLog.Date = DateTime.Now.Date;
          }
        }
      }
      this._currentLogRecords = new List<LogRecordBase>((IEnumerable<LogRecordBase>) logList.GetAllMilestones());
      return logList.GetAllMilestones();
    }

    private static DateTime AddDays(
      DateTime date,
      int dayCount,
      AutoDayCountSetting calenderType,
      SessionObjects sessionObjects)
    {
      DateTime date1 = date;
      switch (calenderType)
      {
        case AutoDayCountSetting.CalendarDays:
          date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
          break;
        case AutoDayCountSetting.CompanyDays:
          try
          {
            date1 = sessionObjects.GetBusinessCalendar(CalendarType.Business).AddBusinessDays(date1, dayCount, false);
            break;
          }
          catch (ArgumentOutOfRangeException ex)
          {
            break;
          }
        default:
          int num = dayCount;
          if (num == 0)
            date1 = date1.AddMinutes(1.0);
          while (num != 0)
          {
            date1 = date1.AddDays(1.0);
            if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
              --num;
          }
          break;
      }
      return date1;
    }
  }
}
