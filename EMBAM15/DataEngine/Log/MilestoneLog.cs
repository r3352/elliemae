// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MilestoneLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class MilestoneLog : LoanAssociateLog
  {
    private const int STAGE_TEXT = 0;
    private const int DONE_TEXT = 1;
    private const int EXP_TEXT = 2;
    private static string[][] fieldIds = new string[6][]
    {
      new string[2]{ "Processing", "MS.PROC" },
      new string[2]{ "submittal", "MS.SUB" },
      new string[2]{ "Approval", "MS.APP" },
      new string[2]{ "Docs Signing", "MS.DOC" },
      new string[2]{ "Funding", "MS.FUN" },
      new string[2]{ "Completion", "MS.CLO" }
    };
    private bool done;
    private string milestoneID = "";
    private string stage = "";
    private int days;
    private int duration;
    private bool reviewed;
    private string roleRequired = "";
    private int sortIndex;
    private string tpoConnecStatus = "";
    private string consumerStatus = "";
    private DateTimeType dateTimeType;
    private IMilestoneDateCalculator milestoneDateCalculator;

    internal MilestoneLog(
      string milestoneID,
      string stage,
      LogList log,
      DateTimeType dateTimeType,
      IMilestoneDateCalculator milestoneDateCalculator)
      : this(stage, log, dateTimeType, milestoneDateCalculator)
    {
      this.milestoneID = (milestoneID ?? "").Trim();
    }

    internal MilestoneLog(
      string stage,
      LogList log,
      DateTimeType dateTimeType,
      IMilestoneDateCalculator milestoneDateCalculator)
      : base(log)
    {
      this.dateTimeType = dateTimeType;
      this.stage = stage;
      this.date = DateTime.MaxValue;
      this.milestoneDateCalculator = milestoneDateCalculator;
      if (!(this.stage == "Started"))
        return;
      this.SetRoleInformation(RoleInfo.FileStarter.ID, RoleInfo.FileStarter.Name);
    }

    public MilestoneLog(
      LogList log,
      XmlElement e,
      DateTimeType dateTimeType,
      IMilestoneDateCalculator milestoneDateCalculator)
      : base(log, e)
    {
      this.dateTimeType = dateTimeType;
      this.milestoneDateCalculator = milestoneDateCalculator;
      AttributeReader attributeReader = new AttributeReader(e);
      this.stage = attributeReader.GetString(nameof (Stage));
      this.milestoneID = attributeReader.GetString(nameof (MilestoneID));
      this.tpoConnecStatus = attributeReader.GetString(nameof (TPOConnectStatus));
      this.days = attributeReader.GetInteger(nameof (Days));
      this.duration = attributeReader.GetInteger(nameof (Duration));
      this.done = attributeReader.GetBoolean(nameof (Done));
      this.reviewed = attributeReader.GetBoolean(nameof (Reviewed));
      this.roleRequired = attributeReader.GetString(nameof (RoleRequired));
      this.sortIndex = attributeReader.GetInteger(nameof (SortIndex));
      this.ConsumerStatus = attributeReader.GetString(nameof (ConsumerStatus));
      if (this.stage == "Started")
      {
        this.done = true;
        this.SetRoleInformation(RoleInfo.FileStarter.ID, RoleInfo.FileStarter.Name);
      }
      else if (this.RoleID == RoleInfo.FileStarter.ID)
        this.SetRoleInformation(-1, "");
      if (this.date >= DateTime.MaxValue.Date)
        this.date = DateTime.MaxValue;
      this.AttachToLog(log);
      this.MarkAsClean();
    }

    internal MilestoneLog(XmlElement e, IMilestoneDateCalculator milestoneDateCalculator)
      : base((LogList) null, e)
    {
      this.milestoneDateCalculator = milestoneDateCalculator;
      AttributeReader attributeReader = new AttributeReader(e);
      this.stage = attributeReader.GetString(nameof (Stage));
      this.milestoneID = attributeReader.GetString(nameof (MilestoneID));
      this.tpoConnecStatus = attributeReader.GetString(nameof (TPOConnectStatus));
      this.days = attributeReader.GetInteger(nameof (Days));
      this.duration = attributeReader.GetInteger(nameof (Duration));
      this.done = attributeReader.GetBoolean(nameof (Done));
      this.reviewed = attributeReader.GetBoolean(nameof (Reviewed));
      this.roleRequired = attributeReader.GetString(nameof (RoleRequired));
      this.sortIndex = attributeReader.GetInteger(nameof (SortIndex));
      this.date = attributeReader.GetDate(nameof (Date));
      this.RoleID = attributeReader.GetInteger("RoleID");
      this.RoleName = attributeReader.GetString("RoleName");
      this.consumerStatus = attributeReader.GetString(nameof (ConsumerStatus));
    }

    public bool Done
    {
      get => this.done;
      set
      {
        if (this.done == value)
          return;
        if (value && this.Log != null && this.Log.Loan != null && !this.Log.Loan.NotifyBeforeMilestoneCompleted(this))
          throw new ActionCanceledException();
        if (value && this.date.Date == DateTime.MaxValue.Date)
          this.AdjustDate(this.getAdjustedMilestoneCompletionDate(DateTime.Now), true, true);
        this.SetDoneInternal(value);
        this.onDoneChanged();
        if (!value || this.Log == null || this.Log.Loan == null)
          return;
        this.Log.Loan.NotifyMilestoneCompleted(this);
      }
    }

    public int Days
    {
      get => this.days;
      set
      {
        if (value == this.days)
          return;
        this.days = value;
        this.MarkAsDirty();
      }
    }

    public int Duration
    {
      get => this.duration;
      set
      {
        if (value == this.duration)
          return;
        this.duration = value;
        this.MarkAsDirty();
      }
    }

    public bool Reviewed
    {
      get => this.reviewed;
      set
      {
        this.reviewed = value;
        this.MarkAsDirty();
      }
    }

    public string MilestoneID
    {
      get => this.milestoneID;
      set
      {
        this.milestoneID = (value ?? "").Trim();
        this.MarkAsDirty();
      }
    }

    public string TPOConnectStatus
    {
      get => this.tpoConnecStatus;
      set
      {
        this.tpoConnecStatus = (value ?? "").Trim();
        this.MarkAsDirty();
      }
    }

    public string ConsumerStatus
    {
      get => this.consumerStatus;
      set
      {
        this.consumerStatus = (value ?? "").Trim();
        this.MarkAsDirty();
      }
    }

    public string RoleRequired
    {
      get => this.roleRequired;
      set
      {
        this.roleRequired = (value ?? "").Trim();
        this.MarkAsDirty();
      }
    }

    public int SortIndex
    {
      get => this.sortIndex;
      set => this.sortIndex = value;
    }

    internal void FixMilestoneID(Hashtable msGuidMapping)
    {
      if (!((this.milestoneID ?? "").Trim() == ""))
        return;
      this.milestoneID = MilestoneLog.MilestoneNameToID(this.stage, msGuidMapping);
      this.MarkAsDirty();
    }

    [CLSCompliant(false)]
    public new string Comments
    {
      get => this.comments;
      set
      {
        this.comments = value;
        this.MarkAsDirtyNoContentChange();
      }
    }

    internal void SetDoneInternal(bool done)
    {
      if (this.done == done)
        return;
      if (this.date.Date == DateTime.MaxValue.Date)
        throw new InvalidOperationException("A valid date must be set on the milestone before it can be marked as completed");
      this.done = done;
      this.setFieldData();
      this.MarkAsDirty(MilestoneProperty.Status);
    }

    internal void SetDateInternal(DateTime newDate)
    {
      if (newDate == this.Date)
        return;
      DateTime maxValue;
      if (this.Stage.Contains("Started"))
      {
        DateTime date1 = newDate.Date;
        maxValue = DateTime.MaxValue;
        DateTime date2 = maxValue.Date;
        if (date1 == date2)
          return;
      }
      if (this.done)
      {
        DateTime date3 = newDate.Date;
        maxValue = DateTime.MaxValue;
        DateTime date4 = maxValue.Date;
        if (date3 == date4)
          throw new ArgumentException("The date for a completed milestone cannot be cleared.");
      }
      if (!(this.Date != newDate))
        return;
      DateTime date5 = newDate.Date;
      maxValue = DateTime.MaxValue;
      DateTime date6 = maxValue.Date;
      if (date5 != date6 && (newDate.Year < 1900 || newDate.Year > 2199))
        throw new ArgumentException("The specified date (" + newDate.ToString("MM/dd/yyyy") + ") must be between the years 1900 and 2199");
      this.date = newDate;
      this.setFieldData();
      this.MarkAsDirty(MilestoneProperty.Date);
    }

    public override DateTime Date
    {
      get
      {
        return this.date.Date == DateTime.MaxValue.Date || this.date.Date == DateTime.MinValue.Date ? DateTime.MaxValue : this.date;
      }
      set => this.AdjustAllDates(value);
    }

    public void AdjustAllDates(DateTime newDate)
    {
      if (newDate == this.Date)
        return;
      DateTime date1 = newDate.Date;
      DateTime dateTime = DateTime.MinValue;
      DateTime date2 = dateTime.Date;
      if (date1 == date2)
        newDate = DateTime.MaxValue;
      DateTime date3 = newDate.Date;
      dateTime = DateTime.MaxValue;
      DateTime date4 = dateTime.Date;
      if (date3 != date4 && (newDate.Year < 1900 || newDate.Year > 2199))
        throw new ArgumentException("The specified date must be between the years 1900 and 2199");
      this.setMilestoneDate(newDate, false, false, true);
      this.Log.CleanUpMilestoneDates();
      this.Log.UpdateMilestoneStatus();
    }

    public void AdjustCorrectionDatesOnly(DateTime newDate)
    {
      if (newDate == this.Date)
        return;
      this.setMilestoneDate(newDate, false, false, true);
      for (int i = this.getStageIndex() + 1; i <= this.Log.GetNumberOfMilestones() - 1; ++i)
      {
        MilestoneLog milestoneAt = this.Log.GetMilestoneAt(i);
        if (milestoneAt.Date < this.Date)
          milestoneAt.SetDateInternal(this.Date);
      }
    }

    public void AdjustDate(
      DateTime newDate,
      bool allowAdjustPreviousMilestones,
      bool allowAdjustFutureMilestones)
    {
      if (newDate == this.Date)
        return;
      DateTime date1 = newDate.Date;
      DateTime dateTime1 = DateTime.MinValue;
      DateTime date2 = dateTime1.Date;
      if (date1 == date2)
        newDate = DateTime.MaxValue;
      DateTime date3 = newDate.Date;
      dateTime1 = DateTime.MaxValue;
      DateTime date4 = dateTime1.Date;
      if (date3 != date4 && (newDate.Year < 1900 || newDate.Year > 2199))
        throw new ArgumentException("The specified date must be between the years 1900 and 2199");
      for (int i = 0; i < this.getStageIndex(); ++i)
      {
        MilestoneLog milestoneAt = this.Log.GetMilestoneAt(i);
        DateTime date5 = newDate.Date;
        dateTime1 = milestoneAt.Date;
        DateTime date6 = dateTime1.Date;
        if (date5 < date6)
        {
          DateTime date7 = newDate.Date;
          dateTime1 = DateTime.MaxValue;
          DateTime date8 = dateTime1.Date;
          if (date7 != date8)
          {
            if (milestoneAt.Done)
              throw new ArgumentException("The milestone date must be later than all previous, completed milestones dates.");
            if (!allowAdjustPreviousMilestones)
            {
              dateTime1 = milestoneAt.Date;
              DateTime date9 = dateTime1.Date;
              dateTime1 = DateTime.MaxValue;
              DateTime date10 = dateTime1.Date;
              if (date9 != date10)
                throw new ArgumentException("This milestone date must be later than all previous milestone dates.");
            }
            if (!allowAdjustPreviousMilestones)
            {
              dateTime1 = milestoneAt.Date;
              DateTime date11 = dateTime1.Date;
              dateTime1 = DateTime.MaxValue;
              DateTime date12 = dateTime1.Date;
              if (date11 == date12)
                throw new ArgumentException("This milestone date cannot be set when preceded by an unscheduled milestone.");
            }
          }
        }
      }
      if (this.getStageIndex() < this.Log.GetNumberOfMilestones() - 1)
      {
        MilestoneLog milestoneAt = this.Log.GetMilestoneAt(this.getStageIndex() + 1);
        DateTime date13 = newDate.Date;
        dateTime1 = milestoneAt.Date;
        DateTime date14 = dateTime1.Date;
        if (date13 > date14)
        {
          if (milestoneAt.Done)
            throw new ArgumentException("This milestone date must be earlier than all subsequent, completed milestone dates.");
          if (!allowAdjustFutureMilestones)
          {
            DateTime date15 = newDate.Date;
            dateTime1 = DateTime.MaxValue;
            DateTime date16 = dateTime1.Date;
            if (date15 == date16)
              throw new ArgumentException("This milestone cannot be unscheduled when followed by scheduled milestones.");
          }
        }
      }
      DateTime dateTime2 = newDate;
      this.setMilestoneDate(newDate, allowAdjustPreviousMilestones, allowAdjustFutureMilestones, true);
      TimeSpan timeSpan = new TimeSpan(1, 0, 0);
      if (this.Done)
      {
        DateTime date17 = newDate.Date;
        dateTime1 = DateTime.MaxValue;
        DateTime date18 = dateTime1.Date;
        if (date17 != date18)
        {
          if (!(newDate.Subtract(DateTime.Now) > timeSpan))
          {
            dateTime1 = DateTime.Now;
            if (!(dateTime1.Subtract(newDate) > timeSpan))
              goto label_30;
          }
          Tracing.SendMessageToServer(TraceLevel.Warning, string.Format("AdjustDate method called for milestone: {0}. DateTime before adjusted = {1}, DateTime after adjusted = {2}, System DateTime = {3}", (object) this.Stage, (object) dateTime2, (object) this.Date, (object) DateTime.Now));
        }
      }
label_30:
      this.Log.CleanUpMilestoneDates();
      this.Log.UpdateMilestoneStatus();
    }

    public static string MilestoneNameToID(string milestoneName, Hashtable msGuidMapping)
    {
      return Milestone.GetCoreMilestoneID(milestoneName) ?? (string) msGuidMapping[(object) milestoneName] ?? "<Unknown>";
    }

    private void setFieldData()
    {
      DateTime dateTime = this.Date;
      DateTime date1 = dateTime.Date;
      dateTime = DateTime.MaxValue;
      DateTime date2 = dateTime.Date;
      if (date1 == date2)
        return;
      string val = this.Date.ToString("MM/dd/yyyy");
      LoanData loan = this.Log.Loan;
      if (this.stage == "Started")
      {
        loan.SetField("MS.START", val);
      }
      else
      {
        foreach (string[] fieldId in MilestoneLog.fieldIds)
        {
          if (string.Compare(this.stage, fieldId[0], StringComparison.OrdinalIgnoreCase) == 0)
          {
            if (this.done)
            {
              this.Log.Loan.SetField(fieldId[1], this.Date.ToString("MM/dd/yyyy"));
              if (string.Compare(this.stage, "Processing", StringComparison.OrdinalIgnoreCase) == 0)
                break;
              bool flag = true;
              try
              {
                if (DateTime.Parse(this.Log.Loan.GetField(fieldId[1] + ".DUE")).Date != DateTime.MaxValue.Date)
                  flag = false;
              }
              catch (Exception ex)
              {
              }
              if (!flag)
                break;
              this.Log.Loan.SetField(fieldId[1] + ".DUE", val);
              break;
            }
            this.Log.Loan.SetField(fieldId[1], "");
            if (string.Compare(this.stage, "Processing", StringComparison.OrdinalIgnoreCase) == 0)
              break;
            this.Log.Loan.SetField(fieldId[1] + ".DUE", val);
            break;
          }
        }
      }
    }

    private void onDoneChanged()
    {
      Math.Min(this.getStageIndex() + 1, this.Log.GetNumberOfMilestones() - 1);
      if (this.done)
      {
        for (int i = 0; i < this.getStageIndex(); ++i)
        {
          MilestoneLog milestoneAt = this.Log.GetMilestoneAt(i);
          if (!milestoneAt.Done)
          {
            milestoneAt.SetDoneInternal(true);
            milestoneAt.SetDateInternal(this.Date);
          }
        }
      }
      else
      {
        for (int i = this.getStageIndex() + 1; i < this.Log.GetNumberOfMilestones(); ++i)
        {
          MilestoneLog milestoneAt = this.Log.GetMilestoneAt(i);
          if (milestoneAt.Done)
            milestoneAt.SetDoneInternal(false);
        }
      }
      this.Log.CleanUpMilestoneDates();
      this.Log.UpdateMilestoneStatus();
    }

    private void setMilestoneDate(
      DateTime newDate,
      bool allowAdjustPreviousMilestones,
      bool allowAdjustFutureMilestones,
      bool dontChangeDate)
    {
      if (newDate.Date == DateTime.MaxValue.Date && this.stage == "Started")
        throw new ArgumentException("Invalid date for Started milestone");
      DatetimeUtils datetimeUtils = new DatetimeUtils(DateTime.MinValue.Date, this.dateTimeType);
      int numOfDays = 0;
      DateTime date1 = this.date.Date;
      DateTime dateTime = DateTime.MaxValue;
      DateTime date2 = dateTime.Date;
      if (date1 != date2)
      {
        DateTime date3 = newDate.Date;
        dateTime = DateTime.MaxValue;
        DateTime date4 = dateTime.Date;
        if (date3 != date4)
        {
          datetimeUtils = new DatetimeUtils(this.date, this.dateTimeType);
          numOfDays = datetimeUtils.NumberOfDaysFrom(newDate);
        }
      }
      if (!dontChangeDate)
      {
        if (this.getStageIndex() != 0 && this.Log.GetMilestoneAt(this.getStageIndex() - 1).Done && this.Log.GetMilestoneAt(this.getStageIndex() - 1).Date > newDate)
          newDate = this.Log.GetMilestoneAt(this.getStageIndex() - 1).Date;
        if (this.getStageIndex() != this.Log.GetNumberOfMilestones() - 1 && this.Log.GetMilestoneAt(this.getStageIndex() + 1).Date < newDate)
          newDate = this.Log.GetMilestoneAt(this.getStageIndex() + 1).Date;
      }
      this.SetDateInternal(newDate);
      if (allowAdjustPreviousMilestones)
      {
        for (int i = 0; i < this.getStageIndex(); ++i)
        {
          MilestoneLog milestoneAt = this.Log.GetMilestoneAt(i);
          if (milestoneAt.Date > this.Date)
            milestoneAt.SetDateInternal(this.Date);
        }
      }
      if (!allowAdjustFutureMilestones)
        return;
      if (this.dateTimeType == DateTimeType.Company)
      {
        KeyValuePair<DateTime, int> getValue = new KeyValuePair<DateTime, int>(datetimeUtils.Date, numOfDays);
        if (this.milestoneDateCalculator == null || !(datetimeUtils.Date != DateTime.MinValue))
          return;
        this.milestoneDateCalculator.AdjustMilestoneDateWithBusinessCalendar(this.getStageIndex(), getValue, this.Log, this);
      }
      else
      {
        for (int i = this.getStageIndex() + 1; i < this.Log.GetNumberOfMilestones(); ++i)
        {
          MilestoneLog milestoneAt = this.Log.GetMilestoneAt(i);
          if (milestoneAt.Done)
            break;
          dateTime = milestoneAt.Date;
          DateTime date5 = dateTime.Date;
          dateTime = DateTime.MaxValue;
          DateTime date6 = dateTime.Date;
          if (date5 != date6)
          {
            dateTime = this.Date;
            DateTime date7 = dateTime.Date;
            dateTime = DateTime.MaxValue;
            DateTime date8 = dateTime.Date;
            if (date7 == date8)
            {
              milestoneAt.SetDateInternal(DateTime.MaxValue);
            }
            else
            {
              try
              {
                DateTime newDate1 = DatetimeUtils.GetDateTime(milestoneAt.Date, this.dateTimeType, numOfDays);
                DateTime date9 = newDate1.Date;
                dateTime = this.Date;
                DateTime date10 = dateTime.Date;
                if (date9 == date10 && newDate1 < this.Date)
                  newDate1 = this.Date;
                milestoneAt.SetDateInternal(newDate1);
              }
              catch
              {
                milestoneAt.SetDateInternal(DateTime.MaxValue);
              }
            }
          }
        }
      }
    }

    public string Stage => this.stage;

    public string DoneText => this.stage;

    public string ExpText => this.stage + " expected";

    public override DateTime GetSortDate()
    {
      if (this.Done || this.Date == DateTime.MaxValue)
        return this.Date;
      DateTime date = this.Date;
      date = date.Date;
      return date.AddDays(1.0);
    }

    private int getStageIndex() => this.Log.GetMilestoneIndex(this.stage);

    private DateTime getAdjustedMilestoneCompletionDate(DateTime baseDate)
    {
      return this.Log == null ? baseDate : this.Log.GetAdjustedMilestoneCompletionDate(this.getStageIndex(), baseDate);
    }

    private void MarkAsDirty(MilestoneProperty property)
    {
      this.MarkAsDirty("Log.MS." + (object) property + "." + this.Stage);
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("MilestoneID", (object) this.MilestoneID);
      attributeWriter.Write("TPOConnectStatus", (object) this.TPOConnectStatus);
      attributeWriter.Write("ConsumerStatus", (object) this.ConsumerStatus);
      attributeWriter.Write("Stage", (object) this.Stage);
      attributeWriter.Write("Days", (object) this.Days);
      attributeWriter.Write("Duration", (object) this.Duration);
      attributeWriter.Write("Done", (object) this.Done);
      attributeWriter.Write("Reviewed", (object) this.Reviewed);
      attributeWriter.Write("RoleRequired", (object) this.roleRequired);
      attributeWriter.Write("SortIndex", (object) this.sortIndex);
      if (this.date.Kind == DateTimeKind.Utc)
        return;
      attributeWriter.Write("Date", (object) this.date.ToUniversalTime());
    }

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      string alertTargetID = this.milestoneID == "" ? this.Guid : this.milestoneID;
      DateTime dateTime;
      if (!this.Done)
      {
        dateTime = this.Date;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.MaxValue;
        DateTime date2 = dateTime.Date;
        if (date1 != date2)
        {
          dateTime = this.Date;
          DateTime date3 = dateTime.Date;
          dateTime = DateTime.MinValue;
          DateTime date4 = dateTime.Date;
          if (date3 != date4)
          {
            PipelineInfo.Alert alert = new PipelineInfo.Alert(5, "", "expected", this.Date, alertTargetID, this.Guid);
            alertList.Add(alert);
          }
        }
      }
      if (this.Done && !this.Stage.Equals("Started", StringComparison.InvariantCultureIgnoreCase) && !this.Stage.Equals("Completion", StringComparison.InvariantCultureIgnoreCase) && this == this.Log.GetLastCompletedMilestone())
      {
        MilestoneLog currentMilestone = this.Log.GetCurrentMilestone();
        if (currentMilestone != null && !currentMilestone.Reviewed)
        {
          PipelineInfo.Alert alert = (PipelineInfo.Alert) null;
          if (currentMilestone.LoanAssociateType == LoanAssociateType.User)
          {
            dateTime = this.Date;
            alert = new PipelineInfo.Alert(0, "", "", dateTime.Date, currentMilestone.LoanAssociateID, -1, alertTargetID, currentMilestone.Guid);
          }
          else if (currentMilestone.LoanAssociateType == LoanAssociateType.Group)
          {
            dateTime = this.Date;
            alert = new PipelineInfo.Alert(0, "", "", dateTime.Date, (string) null, Utils.ParseInt((object) currentMilestone.LoanAssociateID), alertTargetID, currentMilestone.Guid);
          }
          if (alert != null)
            alertList.Add(alert);
        }
      }
      return alertList.ToArray();
    }
  }
}
