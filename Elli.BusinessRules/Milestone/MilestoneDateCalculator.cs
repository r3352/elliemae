// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.MilestoneDateCalculator
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.Domain.Mortgage;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public class MilestoneDateCalculator : IMilestoneDateCalculator
  {
    private static readonly string[][] FieldIds = new string[6][]
    {
      new string[2]{ "Processing", "MS.PROC" },
      new string[2]{ "submittal", "MS.SUB" },
      new string[2]{ "Approval", "MS.APP" },
      new string[2]{ "Docs Signing", "MS.DOC" },
      new string[2]{ "Funding", "MS.FUN" },
      new string[2]{ "Completion", "MS.CLO" }
    };
    private readonly BusinessCalendar _bizCalendar;
    private readonly Loan _loan;
    private readonly DateTimeType _milestoneDateTimeType;
    private readonly bool? _isSingleMilestoneUpdate;

    public MilestoneDateCalculator(
      Loan loan,
      BusinessCalendar businessCalendar,
      DateTimeType milestoneDateTimeType,
      bool? isSingleMilestoneUpdate)
    {
      this._bizCalendar = businessCalendar;
      this._loan = loan;
      this._milestoneDateTimeType = milestoneDateTimeType;
      this._isSingleMilestoneUpdate = isSingleMilestoneUpdate;
    }

    public void AdjustAllDates(MilestoneLog log, DateTime newDate)
    {
      DateTime dateTime1 = newDate;
      DateTime? dateUtc = log.DateUtc;
      if ((dateUtc.HasValue ? (dateTime1 == dateUtc.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        return;
      DateTime date1 = newDate.Date;
      DateTime dateTime2 = DateTime.MinValue;
      DateTime date2 = dateTime2.Date;
      if (date1 == date2)
        newDate = DateTime.MaxValue;
      DateTime date3 = newDate.Date;
      dateTime2 = DateTime.MaxValue;
      DateTime date4 = dateTime2.Date;
      if (date3 != date4 && (newDate.Year < 1900 || newDate.Year > 2199))
        throw new ArgumentException("The specified date must be between the years 1900 and 2199");
      this.SetMilestoneDate(log, newDate, false, false, true);
      this.CleanUpMilestoneDates(log);
      this.UpdateMilestoneStatus();
    }

    public void AdjustDate(
      MilestoneLog log,
      DateTime newDate,
      bool allowAdjustPreviousMilestones,
      bool allowAdjustFutureMilestones)
    {
      DateTime dateTime1 = newDate;
      DateTime? dateUtc = log.DateUtc;
      if ((dateUtc.HasValue ? (dateTime1 == dateUtc.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        return;
      DateTime date1 = newDate.Date;
      DateTime dateTime2 = DateTime.MinValue;
      DateTime date2 = dateTime2.Date;
      if (date1 == date2)
        newDate = DateTime.MaxValue;
      DateTime date3 = newDate.Date;
      dateTime2 = DateTime.MaxValue;
      DateTime date4 = dateTime2.Date;
      if (date3 != date4 && (newDate.Year < 1900 || newDate.Year > 2199))
        throw new ArgumentException("The specified date must be between the years 1900 and 2199");
      int milestoneIndex = this._loan.GetMilestoneIndex(log.Stage);
      bool? nullable;
      for (int index = 0; index < milestoneIndex; ++index)
      {
        MilestoneLog milestoneLog = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
        dateUtc = milestoneLog.DateUtc;
        if (dateUtc.HasValue)
        {
          DateTime date5 = newDate.Date;
          dateUtc = milestoneLog.DateUtc;
          dateTime2 = dateUtc.Value;
          DateTime date6 = dateTime2.Date;
          if (!(date5 >= date6))
          {
            DateTime date7 = newDate.Date;
            dateTime2 = DateTime.MaxValue;
            DateTime date8 = dateTime2.Date;
            if (!(date7 == date8))
            {
              nullable = log.DoneIndicator;
              bool flag = true;
              if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
                throw new ArgumentException("The milestone date must be later than all previous, completed milestones dates.");
              if (!allowAdjustPreviousMilestones)
              {
                dateUtc = log.DateUtc;
                dateTime2 = DateTime.MaxValue.Date;
                if ((dateUtc.HasValue ? (dateUtc.HasValue ? (dateUtc.GetValueOrDefault() != dateTime2 ? 1 : 0) : 0) : 1) != 0)
                  throw new ArgumentException("This milestone date must be later than all previous milestone dates.");
              }
              if (!allowAdjustPreviousMilestones)
              {
                dateUtc = log.DateUtc;
                dateTime2 = DateTime.MaxValue.Date;
                if ((dateUtc.HasValue ? (dateUtc.HasValue ? (dateUtc.GetValueOrDefault() == dateTime2 ? 1 : 0) : 1) : 0) != 0)
                  throw new ArgumentException("This milestone date cannot be set when preceded by an unscheduled milestone.");
              }
            }
          }
        }
      }
      if (milestoneIndex < this._loan.MilestoneLogs.Count<MilestoneLog>() - 1)
      {
        MilestoneLog milestoneLog = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(milestoneIndex + 1);
        dateUtc = milestoneLog.DateUtc;
        if (dateUtc.HasValue)
        {
          DateTime date9 = newDate.Date;
          dateUtc = milestoneLog.DateUtc;
          dateTime2 = dateUtc.Value;
          DateTime date10 = dateTime2.Date;
          if (!(date9 > date10))
            goto label_28;
        }
        nullable = milestoneLog.DoneIndicator;
        bool flag1 = true;
        if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
        {
          nullable = this._isSingleMilestoneUpdate;
          bool flag2 = true;
          if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
            throw new ArgumentException("This milestone date must be earlier than all subsequent, completed milestone dates.");
        }
        if (!allowAdjustFutureMilestones)
        {
          DateTime date11 = newDate.Date;
          dateTime2 = DateTime.MaxValue;
          DateTime date12 = dateTime2.Date;
          if (date11 == date12)
            throw new ArgumentException("This milestone cannot be unscheduled when followed by scheduled milestones.");
        }
      }
label_28:
      this.SetMilestoneDate(log, newDate, allowAdjustPreviousMilestones, allowAdjustFutureMilestones, true);
      this.CleanUpMilestoneDates(log);
      this.UpdateMilestoneStatus();
    }

    public void AdjustCorrectionDatesOnly(MilestoneLog log, DateTime newDate)
    {
      if (newDate.Date == log.DateUtc.Value.Date)
        return;
      this.SetMilestoneDate(log, newDate, false, false, true);
      int milestoneIndex = this._loan.GetMilestoneIndex(log.Stage);
      int num = this._loan.MilestoneLogs.Count<MilestoneLog>();
      for (int index = milestoneIndex + 1; index < num; ++index)
      {
        MilestoneLog log1 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
        if (log1.DateUtc.HasValue && log1.DateUtc.Value.Date < newDate.Date)
          this.SetDateInternal(log1, newDate);
      }
    }

    public void CleanUpMilestoneDates(MilestoneLog log)
    {
      MilestoneLog log1 = this._loan.MilestoneLogs.FirstOrDefault<MilestoneLog>((Func<MilestoneLog, bool>) (r => r.Stage == "Started"));
      if (log1 == null)
        return;
      DateTime newDate1 = DateTime.MaxValue;
      foreach (LogRecord logRecord in this._loan.LogRecords)
      {
        DateTime? dateUtc = logRecord.DateUtc;
        if (dateUtc.HasValue)
        {
          dateUtc = logRecord.DateUtc;
          DateTime minValue = dateUtc.Value;
          DateTime date1 = minValue.Date;
          minValue = DateTime.MinValue;
          DateTime date2 = minValue.Date;
          if (!(date1 == date2))
          {
            dateUtc = logRecord.DateUtc;
            DateTime universalTime = dateUtc.Value;
            dateUtc = logRecord.DateUtc;
            minValue = dateUtc.Value;
            if (minValue.Kind != DateTimeKind.Utc)
            {
              dateUtc = logRecord.DateUtc;
              minValue = dateUtc.Value;
              universalTime = minValue.ToUniversalTime();
            }
            if (universalTime < newDate1)
              newDate1 = universalTime;
          }
        }
      }
      DateTime? dateUtc1;
      if (newDate1.Date != DateTime.MaxValue.Date)
      {
        dateUtc1 = log1.DateUtc;
        if (dateUtc1.HasValue)
        {
          DateTime dateTime1 = newDate1;
          dateUtc1 = log1.DateUtc;
          DateTime dateTime2 = dateUtc1.Value;
          if (dateTime1 < dateTime2)
            this.SetDateInternal(log1, newDate1);
        }
      }
      int num1 = this._loan.MilestoneLogs.Count<MilestoneLog>();
      bool flag1 = false;
      bool? doneIndicator;
      for (int index = num1 - 1; index >= 0; --index)
      {
        MilestoneLog milestoneLog = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
        doneIndicator = milestoneLog.DoneIndicator;
        bool flag2 = true;
        if (doneIndicator.GetValueOrDefault() == flag2 & doneIndicator.HasValue)
          flag1 = true;
        else if (flag1)
          milestoneLog.DoneIndicator = new bool?(true);
      }
      DateTime maxValue1 = DateTime.MaxValue;
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        MilestoneLog milestoneLog = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
        dateUtc1 = milestoneLog.DateUtc;
        if (dateUtc1.HasValue)
        {
          dateUtc1 = milestoneLog.DateUtc;
          DateTime maxValue2 = DateTime.MaxValue;
          if ((dateUtc1.HasValue ? (dateUtc1.HasValue ? (dateUtc1.GetValueOrDefault() == maxValue2 ? 1 : 0) : 1) : 0) == 0)
          {
            dateUtc1 = milestoneLog.DateUtc;
            DateTime minValue = DateTime.MinValue;
            if ((dateUtc1.HasValue ? (dateUtc1.HasValue ? (dateUtc1.GetValueOrDefault() == minValue ? 1 : 0) : 1) : 0) == 0)
            {
              num2 = index;
              dateUtc1 = milestoneLog.DateUtc;
              if (dateUtc1.HasValue)
              {
                dateUtc1 = milestoneLog.DateUtc;
                if (dateUtc1.Value < maxValue1)
                {
                  dateUtc1 = milestoneLog.DateUtc;
                  maxValue1 = dateUtc1.Value;
                }
              }
            }
          }
        }
      }
      MilestoneLog log2 = log1;
      DateTime dateTime;
      DateTime newDate2;
      if (!(maxValue1 != DateTime.MaxValue))
      {
        dateTime = DateTime.Today;
        newDate2 = dateTime.Date;
      }
      else
        newDate2 = maxValue1;
      this.SetDateInternal(log2, newDate2);
      MilestoneLog milestoneLog1 = log1;
      DateTime? dateUtc2;
      for (int index = 1; index <= num2; ++index)
      {
        MilestoneLog milestoneLog2 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
        doneIndicator = milestoneLog2.DoneIndicator;
        bool flag3 = true;
        if (doneIndicator.GetValueOrDefault() == flag3 & doneIndicator.HasValue)
        {
          dateUtc1 = milestoneLog1.DateUtc;
          if (dateUtc1.HasValue)
          {
            dateUtc1 = milestoneLog2.DateUtc;
            dateUtc2 = milestoneLog1.DateUtc;
            if ((dateUtc1.HasValue & dateUtc2.HasValue ? (dateUtc1.GetValueOrDefault() < dateUtc2.GetValueOrDefault() ? 1 : 0) : 0) == 0)
            {
              dateUtc2 = milestoneLog2.DateUtc;
              dateTime = DateTime.MaxValue;
              if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0) == 0)
                goto label_43;
            }
            MilestoneLog log3 = milestoneLog2;
            dateUtc2 = milestoneLog1.DateUtc;
            DateTime newDate3 = dateUtc2.Value;
            this.SetDateInternal(log3, newDate3);
          }
        }
label_43:
        milestoneLog1 = milestoneLog2;
      }
      for (int index = num2 + 1; index < num1; ++index)
        this.SetDateInternal(this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index), DateTime.MaxValue);
      TimeSpan timeSpan;
      for (int index = 0; index < num1 - 1; ++index)
      {
        MilestoneLog milestoneLog3 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
        MilestoneLog milestoneLog4 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index + 1);
        dateUtc2 = milestoneLog3.DateUtc;
        if (dateUtc2.HasValue)
        {
          dateUtc2 = milestoneLog4.DateUtc;
          if (dateUtc2.HasValue)
          {
            dateUtc2 = milestoneLog4.DateUtc;
            dateTime = DateTime.MinValue;
            if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0) == 0)
            {
              dateUtc2 = milestoneLog4.DateUtc;
              dateTime = DateTime.MaxValue;
              if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() == dateTime ? 1 : 0) : 1) : 0) == 0)
              {
                doneIndicator = milestoneLog4.DoneIndicator;
                if (doneIndicator.HasValue)
                {
                  doneIndicator = milestoneLog4.DoneIndicator;
                  if (doneIndicator.Value)
                  {
                    doneIndicator = milestoneLog3.DoneIndicator;
                    if (doneIndicator.HasValue)
                    {
                      doneIndicator = milestoneLog3.DoneIndicator;
                      if (doneIndicator.Value)
                      {
                        dateUtc2 = milestoneLog3.DateUtc;
                        dateTime = dateUtc2.Value;
                        DateTime date = dateTime.Date;
                        dateUtc2 = milestoneLog4.DateUtc;
                        dateTime = dateUtc2.Value;
                        timeSpan = dateTime.Date.Subtract(date);
                        milestoneLog3.Duration = new short?((short) timeSpan.Days);
                        continue;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        milestoneLog3.Duration = new short?((short) -1);
      }
      int num3 = 0;
      if (this._loan.MilestoneLogs.Any<MilestoneLog>())
      {
        MilestoneLog milestoneLog5 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(num1 - 1);
        doneIndicator = milestoneLog5.DoneIndicator;
        bool flag4 = true;
        if (doneIndicator.GetValueOrDefault() == flag4 & doneIndicator.HasValue)
        {
          dateUtc2 = milestoneLog5.DateUtc;
          if (dateUtc2.HasValue)
          {
            dateUtc2 = milestoneLog5.DateUtc;
            dateTime = DateTime.MinValue;
            if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() != dateTime ? 1 : 0) : 0) : 1) != 0)
            {
              dateUtc2 = milestoneLog5.DateUtc;
              dateTime = DateTime.MaxValue;
              if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() != dateTime ? 1 : 0) : 0) : 1) != 0)
              {
                MilestoneLog milestoneLog6 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(0);
                dateUtc2 = milestoneLog6.DateUtc;
                if (dateUtc2.HasValue)
                {
                  dateUtc2 = milestoneLog6.DateUtc;
                  dateTime = DateTime.MinValue;
                  if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() != dateTime ? 1 : 0) : 0) : 1) != 0)
                  {
                    dateUtc2 = milestoneLog6.DateUtc;
                    dateTime = DateTime.MaxValue;
                    if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() != dateTime ? 1 : 0) : 0) : 1) != 0)
                    {
                      doneIndicator = milestoneLog6.DoneIndicator;
                      bool flag5 = true;
                      if (doneIndicator.GetValueOrDefault() == flag5 & doneIndicator.HasValue)
                      {
                        dateUtc2 = milestoneLog5.DateUtc;
                        dateTime = dateUtc2.Value;
                        dateTime = dateTime.Date;
                        ref DateTime local = ref dateTime;
                        dateUtc2 = milestoneLog6.DateUtc;
                        DateTime date = dateUtc2.Value.Date;
                        timeSpan = local.Subtract(date);
                        num3 = timeSpan.Days;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      this._loan.MilestoneDuration = new int?(num3);
    }

    public void UpdateMilestoneStatus()
    {
      string str = "Started";
      DateTime? nullable = new DateTime?();
      if (this._loan.MilestoneLogs.Any<MilestoneLog>())
      {
        List<MilestoneLog> list = this._loan.MilestoneLogs.Where<MilestoneLog>((Func<MilestoneLog, bool>) (r =>
        {
          bool? doneIndicator = r.DoneIndicator;
          bool flag = true;
          return doneIndicator.GetValueOrDefault() == flag & doneIndicator.HasValue;
        })).ToList<MilestoneLog>();
        if (list.Any<MilestoneLog>())
        {
          MilestoneLog ms = list.ElementAt<MilestoneLog>(list.Count<MilestoneLog>() - 1);
          str = MilestoneDateCalculator.GetStatus(ms);
          nullable = ms.DateUtc;
        }
        else
          nullable = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(0).DateUtc;
      }
      this._loan.MilestoneCurrentName = str;
      this._loan.MilestoneCurrentDateUtc = nullable;
    }

    private static string GetStatus(MilestoneLog ms)
    {
      string status = ms.Stage;
      if (Utils.IsInt((object) ms.MilestoneIdString))
      {
        switch (ms.MilestoneIdString)
        {
          case "1":
            status = "File started";
            break;
          case "2":
            status = "Sent to processing";
            break;
          case "3":
            status = "Submitted";
            break;
          case "4":
            status = "Approved";
            break;
          case "5":
            status = "Doc signed";
            break;
          case "6":
            status = "Funded";
            break;
          case "7":
            status = "Completed";
            break;
        }
      }
      return status;
    }

    private void SetMilestoneDate(
      MilestoneLog log,
      DateTime newDate,
      bool allowAdjustPreviousMilestones,
      bool allowAdjustFutureMilestones,
      bool dontChangeDate)
    {
      if (newDate.Date == DateTime.MaxValue.Date && log.Stage == "Started")
        throw new ArgumentException("Invalid date for Started milestone");
      DatetimeUtils datetimeUtils = new DatetimeUtils(DateTime.MinValue.Date, this._milestoneDateTimeType);
      int num = this._loan.MilestoneLogs.Count<MilestoneLog>();
      int numOfDays = 0;
      DateTime? dateUtc1 = log.DateUtc;
      if (dateUtc1.HasValue)
      {
        dateUtc1 = log.DateUtc;
        DateTime date = DateTime.MaxValue.Date;
        if ((dateUtc1.HasValue ? (dateUtc1.HasValue ? (dateUtc1.GetValueOrDefault() != date ? 1 : 0) : 0) : 1) != 0 && newDate.Date != DateTime.MaxValue.Date)
        {
          dateUtc1 = log.DateUtc;
          datetimeUtils = new DatetimeUtils(dateUtc1.Value, this._milestoneDateTimeType);
          numOfDays = datetimeUtils.NumberOfDaysFrom(newDate);
        }
      }
      int milestoneIndex = this._loan.GetMilestoneIndex(log.Stage);
      if (!dontChangeDate)
      {
        MilestoneLog milestoneLog1 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(milestoneIndex - 1);
        if (milestoneIndex != 0)
        {
          bool? doneIndicator = milestoneLog1.DoneIndicator;
          bool flag = true;
          if (doneIndicator.GetValueOrDefault() == flag & doneIndicator.HasValue)
          {
            dateUtc1 = milestoneLog1.DateUtc;
            if (dateUtc1.HasValue)
            {
              dateUtc1 = milestoneLog1.DateUtc;
              DateTime dateTime = newDate;
              if ((dateUtc1.HasValue ? (dateUtc1.GetValueOrDefault() > dateTime ? 1 : 0) : 0) != 0)
              {
                dateUtc1 = milestoneLog1.DateUtc;
                newDate = dateUtc1.Value;
              }
            }
          }
        }
        MilestoneLog milestoneLog2 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(milestoneIndex + 1);
        if (milestoneIndex != num - 1)
        {
          dateUtc1 = milestoneLog2.DateUtc;
          if (dateUtc1.HasValue)
          {
            dateUtc1 = milestoneLog2.DateUtc;
            DateTime dateTime = newDate;
            if ((dateUtc1.HasValue ? (dateUtc1.GetValueOrDefault() < dateTime ? 1 : 0) : 0) != 0)
            {
              dateUtc1 = milestoneLog2.DateUtc;
              newDate = dateUtc1.Value;
            }
          }
        }
      }
      this.SetDateInternal(log, newDate);
      DateTime? dateUtc2;
      if (allowAdjustPreviousMilestones)
      {
        for (int index = 0; index < milestoneIndex; ++index)
        {
          MilestoneLog milestoneLog = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
          dateUtc1 = log.DateUtc;
          if (dateUtc1.HasValue)
          {
            dateUtc1 = milestoneLog.DateUtc;
            dateUtc2 = log.DateUtc;
            if ((dateUtc1.HasValue & dateUtc2.HasValue ? (dateUtc1.GetValueOrDefault() > dateUtc2.GetValueOrDefault() ? 1 : 0) : 0) != 0)
            {
              MilestoneLog log1 = milestoneLog;
              dateUtc2 = log.DateUtc;
              DateTime newDate1 = dateUtc2.Value;
              this.SetDateInternal(log1, newDate1);
            }
          }
        }
      }
      if (!allowAdjustFutureMilestones)
        return;
      if (this._milestoneDateTimeType == DateTimeType.Company)
      {
        KeyValuePair<DateTime, int> getValue = new KeyValuePair<DateTime, int>(datetimeUtils.Date, numOfDays);
        if (!(datetimeUtils.Date != DateTime.MinValue))
          return;
        this.AdjustMilestoneDateWithBusinessCalendar(milestoneIndex, getValue, log);
      }
      else
      {
        for (int index = milestoneIndex + 1; index < num; ++index)
        {
          MilestoneLog log2 = this._loan.MilestoneLogs.ElementAt<MilestoneLog>(index);
          bool? nullable = log2.DoneIndicator;
          bool flag1 = true;
          if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
          {
            nullable = this._isSingleMilestoneUpdate;
            bool flag2 = true;
            if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
              break;
          }
          dateUtc2 = log2.DateUtc;
          if (dateUtc2.HasValue)
          {
            dateUtc2 = log2.DateUtc;
            if (!(dateUtc2.Value.Date == DateTime.MaxValue.Date))
            {
              dateUtc2 = log.DateUtc;
              DateTime date1 = DateTime.MaxValue.Date;
              if ((dateUtc2.HasValue ? (dateUtc2.HasValue ? (dateUtc2.GetValueOrDefault() == date1 ? 1 : 0) : 1) : 0) != 0)
              {
                this.SetDateInternal(log2, DateTime.MaxValue);
              }
              else
              {
                try
                {
                  dateUtc2 = log2.DateUtc;
                  DateTime dateTime1 = DatetimeUtils.GetDateTime(dateUtc2.Value, this._milestoneDateTimeType, numOfDays);
                  DateTime date2 = dateTime1.Date;
                  dateUtc2 = log.DateUtc;
                  DateTime date3 = dateUtc2.Value.Date;
                  if (date2 == date3)
                  {
                    DateTime dateTime2 = dateTime1;
                    dateUtc2 = log.DateUtc;
                    DateTime dateTime3 = dateUtc2.Value;
                    if (dateTime2 < dateTime3)
                    {
                      dateUtc2 = log.DateUtc;
                      dateTime1 = dateUtc2.Value;
                    }
                  }
                  this.SetDateInternal(log2, dateTime1);
                }
                catch
                {
                  this.SetDateInternal(log2, DateTime.MaxValue);
                }
              }
            }
          }
        }
      }
    }

    private void SetDateInternal(MilestoneLog log, DateTime newDate)
    {
      DateTime dateTime = newDate;
      DateTime? dateUtc = log.DateUtc;
      if ((dateUtc.HasValue ? (dateTime == dateUtc.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        return;
      DateTime maxValue;
      if (log.Stage.Contains("Started"))
      {
        DateTime date1 = newDate.Date;
        maxValue = DateTime.MaxValue;
        DateTime date2 = maxValue.Date;
        if (date1 == date2)
          return;
      }
      bool? doneIndicator = log.DoneIndicator;
      bool flag = true;
      if (doneIndicator.GetValueOrDefault() == flag & doneIndicator.HasValue)
      {
        DateTime date3 = newDate.Date;
        maxValue = DateTime.MaxValue;
        DateTime date4 = maxValue.Date;
        if (date3 == date4)
          throw new ArgumentException("The date for a completed milestone cannot be cleared.");
      }
      DateTime date5 = newDate.Date;
      maxValue = DateTime.MaxValue;
      DateTime date6 = maxValue.Date;
      if (date5 != date6 && (newDate.Year < 1900 || newDate.Year > 2199))
        throw new ArgumentException("The specified date (" + newDate.ToString("MM/dd/yyyy") + ") must be between the years 1900 and 2199");
      log.DateUtc = new DateTime?(newDate);
      this.SetFieldData(log);
    }

    private void SetFieldData(MilestoneLog log)
    {
      if (!log.DateUtc.HasValue)
        return;
      DateTime? dateUtc = log.DateUtc;
      DateTime date = DateTime.MaxValue.Date;
      if ((dateUtc.HasValue ? (dateUtc.HasValue ? (dateUtc.GetValueOrDefault() == date ? 1 : 0) : 1) : 0) != 0)
        return;
      if (log.Stage == "Started")
      {
        this._loan.MilestoneFileStartedDate = new DateTime?(log.DateUtc.Value);
      }
      else
      {
        foreach (string[] strArray in ((IEnumerable<string[]>) MilestoneDateCalculator.FieldIds).Where<string[]>((Func<string[], bool>) (pair => string.Equals(log.Stage, pair[0], StringComparison.CurrentCultureIgnoreCase))))
        {
          bool? doneIndicator = log.DoneIndicator;
          bool flag1 = true;
          if (doneIndicator.GetValueOrDefault() == flag1 & doneIndicator.HasValue)
          {
            this.SetField(strArray[1], log.DateUtc);
            if (!(log.Stage == "Processing"))
            {
              bool flag2 = true;
              try
              {
                DateTime? field = this.GetField(strArray[1] + ".DUE");
                if (field.HasValue)
                {
                  if (field.Value != DateTime.MaxValue.Date)
                    flag2 = false;
                }
              }
              catch (Exception ex)
              {
              }
              if (!flag2)
                break;
              this.SetField(strArray[1] + ".DUE", log.DateUtc);
              this._loan.MilestoneApprovedDueDate = log.DateUtc;
              break;
            }
          }
          else
          {
            this.SetField(strArray[1], new DateTime?());
            if (!(log.Stage != "Processing"))
              break;
            this.SetField(strArray[1] + ".DUE", log.DateUtc);
            break;
          }
        }
      }
    }

    private DateTime? GetField(string id)
    {
      if (id == null)
        return new DateTime?();
      switch (id.ToUpper(CultureInfo.CurrentCulture))
      {
        case "MS.APP":
          return this._loan.MilestoneApprovedDate;
        case "MS.APP.DUE":
          return this._loan.MilestoneApprovedDueDate;
        case "MS.CLO":
          return this._loan.MilestoneCompletedDate;
        case "MS.CLO.DUE":
          return this._loan.MilestoneCompletedDueDate;
        case "MS.DOC":
          return this._loan.MilestoneDocSignedDate;
        case "MS.DOC.DUE":
          return this._loan.MilestoneDocSignedDueDate;
        case "MS.FUN":
          return this._loan.MilestoneFundedDate;
        case "MS.FUN.DUE":
          return this._loan.MilestoneFundedDueDate;
        case "MS.PROC":
          return this._loan.MilestoneProcessedDate;
        case "MS.START":
          return this._loan.MilestoneFileStartedDate;
        case "MS.STATUSDATE":
          return this._loan.MilestoneCurrentDateUtc;
        case "MS.SUB":
          return this._loan.MilestoneSubmittedDate;
        case "MS.SUB.DUE":
          return this._loan.MilestoneSubmittedDueDate;
        default:
          return new DateTime?();
      }
    }

    private void SetField(string id, DateTime? value)
    {
      if (id == null)
        return;
      string upper = id.ToUpper(CultureInfo.CurrentCulture);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(upper))
      {
        case 46640787:
          if (!(upper == "MS.STATUSDATE"))
            break;
          this._loan.MilestoneCurrentDateUtc = value;
          break;
        case 107658983:
          if (!(upper == "MS.CLO"))
            break;
          this._loan.MilestoneCompletedDate = value;
          break;
        case 1635131273:
          if (!(upper == "MS.SUB.DUE"))
            break;
          this._loan.MilestoneSubmittedDueDate = value;
          break;
        case 1708047876:
          if (!(upper == "MS.APP"))
            break;
          this._loan.MilestoneApprovedDate = value;
          break;
        case 2342700610:
          if (!(upper == "MS.FUN.DUE"))
            break;
          this._loan.MilestoneFundedDueDate = value;
          break;
        case 2470469305:
          if (!(upper == "MS.PROC"))
            break;
          this._loan.MilestoneProcessedDate = value;
          break;
        case 3031505671:
          if (!(upper == "MS.CLO.DUE"))
            break;
          this._loan.MilestoneCompletedDueDate = value;
          break;
        case 3338390999:
          if (!(upper == "MS.START"))
            break;
          this._loan.MilestoneFileStartedDate = value;
          break;
        case 3341275769:
          if (!(upper == "MS.DOC.DUE"))
            break;
          this._loan.MilestoneDocSignedDueDate = value;
          break;
        case 3500393629:
          if (!(upper == "MS.DOC"))
            break;
          this._loan.MilestoneDocSignedDate = value;
          break;
        case 3714253254:
          if (!(upper == "MS.FUN"))
            break;
          this._loan.MilestoneFundedDate = value;
          break;
        case 3766966285:
          if (!(upper == "MS.SUB"))
            break;
          this._loan.MilestoneSubmittedDate = value;
          break;
        case 3801102188:
          if (!(upper == "MS.APP.DUE"))
            break;
          this._loan.MilestoneApprovedDueDate = value;
          break;
      }
    }

    private void AdjustMilestoneDateWithBusinessCalendar(
      int index,
      KeyValuePair<DateTime, int> getValue,
      MilestoneLog currentMilestoneLog)
    {
      int num = 0;
      DateTime key = getValue.Key;
      int dayCount = getValue.Value;
      DateTime? dateUtc1 = currentMilestoneLog.DateUtc;
      DateTime? nullable;
      DateTime maxValue;
      if (key != DateTime.MinValue && dateUtc1.HasValue)
      {
        DateTime dateTime = key;
        nullable = dateUtc1;
        if ((nullable.HasValue ? (dateTime < nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        {
          DateTime date1 = key.Date;
          while (true)
          {
            DateTime date2 = date1.Date;
            ref DateTime local = ref date2;
            maxValue = dateUtc1.Value;
            DateTime date3 = maxValue.Date;
            if (!local.Equals(date3))
            {
              if (!this._bizCalendar.IsBusinessDay(date1) && !this._bizCalendar.IsWeekendDay(date1))
                --num;
              date1 = date1.AddDays(1.0);
            }
            else
              goto label_14;
          }
        }
      }
      if (dateUtc1.HasValue)
      {
        for (DateTime date = dateUtc1.Value; !date.Date.Equals(key.Date); date = date.AddDays(1.0))
        {
          if (!this._bizCalendar.IsBusinessDay(date) && !this._bizCalendar.IsWeekendDay(date))
            --num;
        }
      }
label_14:
      if (dayCount < 0)
        dayCount -= num;
      else if (dayCount > 0)
        dayCount += num;
      Loan loan = currentMilestoneLog.Loan;
      for (int index1 = index + 1; index1 < loan.MilestoneLogs.Count<MilestoneLog>(); ++index1)
      {
        MilestoneLog log = loan.MilestoneLogs.ElementAt<MilestoneLog>(index1);
        bool? doneIndicator = log.DoneIndicator;
        bool flag = true;
        if (doneIndicator.GetValueOrDefault() == flag & doneIndicator.HasValue)
          break;
        nullable = log.DateUtc;
        maxValue = DateTime.MaxValue;
        DateTime date4 = maxValue.Date;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == date4 ? 1 : 0) : 1) : 0) == 0)
        {
          nullable = currentMilestoneLog.DateUtc;
          if (nullable.HasValue)
          {
            nullable = currentMilestoneLog.DateUtc;
            maxValue = DateTime.MaxValue;
            DateTime date5 = maxValue.Date;
            if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == date5 ? 1 : 0) : 1) : 0) == 0)
            {
              try
              {
                nullable = log.DateUtc;
                if (nullable.HasValue)
                {
                  nullable = log.DateUtc;
                  DateTime newDate = this.AddDays(nullable.Value, dayCount);
                  DateTime? dateUtc2 = loan.MilestoneLogs.ElementAt<MilestoneLog>(index1 - 1).DateUtc;
                  if (dateUtc2.HasValue)
                  {
                    DateTime date6 = newDate.Date;
                    nullable = dateUtc2;
                    if ((nullable.HasValue ? (date6 == nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0 && index1 > 0)
                      newDate = dateUtc2.Value;
                  }
                  this.AdjustAllDates(log, newDate);
                  continue;
                }
                continue;
              }
              catch
              {
                this.AdjustAllDates(log, DateTime.MaxValue);
                continue;
              }
            }
          }
          this.AdjustAllDates(log, DateTime.MaxValue);
        }
      }
    }

    private DateTime AddDays(DateTime date, int dayCount)
    {
      DateTime date1 = date;
      switch (this._milestoneDateTimeType)
      {
        case DateTimeType.Calendar:
          date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
          break;
        case DateTimeType.Company:
          try
          {
            date1 = this._bizCalendar.AddBusinessDays(date1, dayCount, false);
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
