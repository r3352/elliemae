// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanCompHistoryList
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanCompHistoryList
  {
    private List<LoanCompHistory> historyList;
    private string userOrOrgID = string.Empty;
    private bool isDirty;
    private string userName = string.Empty;
    private bool useParentInfo;
    private bool uncheckParentInfo;

    public LoanCompHistoryList(string userOrOrgID) => this.ClearHistory();

    public void AddHistory(LoanCompHistory history) => this.historyList.Add(history);

    public void RemoveHistory(LoanCompHistory history)
    {
      try
      {
        this.historyList.Remove(history);
      }
      catch (Exception ex)
      {
        throw new Exception("The plan '" + history.PlanName + "' cannot be deleted. Error: " + ex.Message);
      }
    }

    public void ClearHistory() => this.historyList = new List<LoanCompHistory>();

    public LoanCompHistory getLoanCompHistoryByCompPlanId(int compPlanId)
    {
      return this.historyList.Find((Predicate<LoanCompHistory>) (e1 => e1.CompPlanId == compPlanId));
    }

    public int Count => this.historyList != null ? this.historyList.Count : 0;

    public LoanCompHistory GetHistoryAt(int i)
    {
      try
      {
        return this.historyList[i];
      }
      catch (Exception ex)
      {
        throw new Exception("LoanCompHistoryList.GetHistoryAt: Cannot get history record due to this error - " + ex.Message);
      }
    }

    public void SortPlans(bool changeEndDate)
    {
      if (this.historyList == null || this.historyList.Count == 0)
        return;
      DateTime startDate;
      for (int index1 = 0; index1 < this.historyList.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.historyList.Count; ++index2)
        {
          startDate = this.historyList[index1].StartDate;
          DateTime date1 = startDate.Date;
          startDate = this.historyList[index2].StartDate;
          DateTime date2 = startDate.Date;
          if (date1 > date2)
          {
            LoanCompHistory history = this.historyList[index1];
            this.historyList[index1] = this.historyList[index2];
            this.historyList[index2] = history;
          }
        }
      }
      if (!changeEndDate)
        return;
      for (int index = 0; index < this.historyList.Count - 1; ++index)
      {
        LoanCompHistory history = this.historyList[index];
        startDate = this.historyList[index + 1].StartDate;
        DateTime dateTime = startDate.AddDays(-1.0);
        history.EndDate = dateTime;
      }
      this.historyList[this.historyList.Count - 1].EndDate = DateTime.MaxValue;
    }

    public bool IsNewPlanStartDateValid(LoanCompHistory selectedHistory, DateTime newDate)
    {
      if (this.historyList == null || this.historyList.Count == 0)
        return true;
      this.SortPlans(true);
      for (int index = 0; index < this.historyList.Count; ++index)
      {
        if (this.historyList[index] != selectedHistory)
        {
          DateTime date1 = newDate.Date;
          DateTime dateTime = this.historyList[index].StartDate;
          DateTime date2 = dateTime.Date;
          if (date1 == date2)
            return false;
          dateTime = this.historyList[index].EndDate;
          DateTime date3 = dateTime.Date;
          dateTime = DateTime.MaxValue;
          DateTime date4 = dateTime.Date;
          if (date3 != date4)
          {
            DateTime date5 = newDate.Date;
            dateTime = this.historyList[index].StartDate;
            DateTime date6 = dateTime.Date;
            if (date5 >= date6)
            {
              DateTime date7 = newDate.Date;
              dateTime = this.historyList[index].EndDate;
              DateTime date8 = dateTime.Date;
              if (date7 <= date8)
              {
                dateTime = this.historyList[index].EndDate;
                DateTime date9 = dateTime.Date;
                dateTime = DateTime.Today;
                DateTime date10 = dateTime.Date;
                if (date9 <= date10)
                  return false;
              }
            }
          }
          DateTime date11 = newDate.Date;
          dateTime = this.historyList[index].StartDate;
          DateTime date12 = dateTime.Date;
          if (date11 <= date12)
          {
            dateTime = this.historyList[index].StartDate;
            DateTime date13 = dateTime.Date;
            dateTime = DateTime.Today;
            DateTime date14 = dateTime.Date;
            if (date13 <= date14)
              return false;
          }
        }
      }
      return true;
    }

    public bool IsNewPlanStartDateValid(LoanCompHistory newPlan)
    {
      if (this.historyList == null || this.historyList.Count == 0)
        return true;
      this.SortPlans(true);
      for (int index = 0; index < this.historyList.Count; ++index)
      {
        DateTime dateTime = newPlan.StartDate;
        DateTime date1 = dateTime.Date;
        dateTime = this.historyList[index].StartDate;
        DateTime date2 = dateTime.Date;
        if (date1 == date2)
          return false;
        dateTime = this.historyList[index].EndDate;
        DateTime date3 = dateTime.Date;
        dateTime = DateTime.MaxValue;
        DateTime date4 = dateTime.Date;
        if (date3 != date4)
        {
          dateTime = newPlan.StartDate;
          DateTime date5 = dateTime.Date;
          dateTime = this.historyList[index].StartDate;
          DateTime date6 = dateTime.Date;
          if (date5 >= date6)
          {
            dateTime = newPlan.StartDate;
            DateTime date7 = dateTime.Date;
            dateTime = this.historyList[index].EndDate;
            DateTime date8 = dateTime.Date;
            if (date7 <= date8)
            {
              dateTime = this.historyList[index].EndDate;
              DateTime date9 = dateTime.Date;
              dateTime = DateTime.Today;
              DateTime date10 = dateTime.Date;
              if (date9 <= date10)
                return false;
            }
          }
        }
        dateTime = newPlan.StartDate;
        DateTime date11 = dateTime.Date;
        dateTime = this.historyList[index].StartDate;
        DateTime date12 = dateTime.Date;
        if (date11 <= date12)
        {
          dateTime = this.historyList[index].StartDate;
          DateTime date13 = dateTime.Date;
          dateTime = DateTime.Today;
          DateTime date14 = dateTime.Date;
          if (date13 <= date14)
            return false;
        }
        dateTime = newPlan.StartDate;
        DateTime date15 = dateTime.Date;
        dateTime = DateTime.Today;
        DateTime date16 = dateTime.Date;
        if (date15 <= date16)
        {
          dateTime = newPlan.StartDate;
          DateTime date17 = dateTime.Date;
          dateTime = this.historyList[index].StartDate;
          DateTime date18 = dateTime.Date;
          if (date17 >= date18)
          {
            dateTime = newPlan.EndDate;
            DateTime date19 = dateTime.Date;
            dateTime = this.historyList[index].EndDate;
            DateTime date20 = dateTime.Date;
            if (date19 <= date20)
              return false;
          }
        }
      }
      return true;
    }

    public bool IsNewPlanStartDateValidWithMinimumTermDays(
      LoanCompHistory newPlan,
      DateTime newDate)
    {
      if (this.historyList == null || this.historyList.Count == 0)
        return true;
      this.SortPlans(true);
      LoanCompHistory loanCompHistory = (LoanCompHistory) null;
      DateTime dateTime1;
      for (int index = 0; index < this.historyList.Count; ++index)
      {
        if (newPlan != this.historyList[index])
        {
          dateTime1 = newPlan.StartDate;
          DateTime date1 = dateTime1.Date;
          dateTime1 = this.historyList[index].StartDate;
          DateTime date2 = dateTime1.Date;
          if (date1 > date2)
          {
            dateTime1 = this.historyList[index].EndDate;
            DateTime date3 = dateTime1.Date;
            dateTime1 = DateTime.Today;
            DateTime date4 = dateTime1.Date;
            if (date3 > date4)
              goto label_9;
          }
          dateTime1 = newPlan.StartDate;
          DateTime date5 = dateTime1.Date;
          dateTime1 = this.historyList[index].EndDate;
          DateTime date6 = dateTime1.Date;
          if (!(date5 > date6))
          {
            dateTime1 = this.historyList[index].EndDate;
            DateTime date7 = dateTime1.Date;
            dateTime1 = DateTime.MaxValue;
            DateTime date8 = dateTime1.Date;
            if (date7 == date8)
            {
              dateTime1 = newPlan.StartDate;
              DateTime date9 = dateTime1.Date;
              dateTime1 = this.historyList[index].StartDate;
              DateTime date10 = dateTime1.Date;
              if (!(date9 > date10))
                continue;
            }
            else
              continue;
          }
label_9:
          loanCompHistory = this.historyList[index];
        }
      }
      if (loanCompHistory != null && loanCompHistory.MinTermDays > 0)
      {
        dateTime1 = loanCompHistory.StartDate;
        dateTime1 = dateTime1.Date;
        DateTime dateTime2 = dateTime1.AddDays((double) loanCompHistory.MinTermDays);
        if (newDate.Date < dateTime2.Date)
          return false;
      }
      return true;
    }

    public LoanCompHistory GetCurrentPlan(DateTime triggerDateTime)
    {
      if (triggerDateTime.Date == DateTime.MaxValue.Date || triggerDateTime.Date == DateTime.MinValue.Date)
        triggerDateTime = DateTime.Today.Date;
      if (this.Count == 0)
        return (LoanCompHistory) null;
      this.SortPlans(true);
      for (int i = 0; i < this.Count; ++i)
      {
        LoanCompHistory historyAt = this.GetHistoryAt(i);
        DateTime dateTime = historyAt.StartDate;
        if (dateTime.Date <= triggerDateTime.Date)
        {
          dateTime = historyAt.EndDate;
          if (dateTime.Date >= triggerDateTime.Date)
            return historyAt;
        }
      }
      return (LoanCompHistory) null;
    }

    public List<LoanCompHistory> GetCurrentAndFuturePlans(DateTime todayDate)
    {
      this.SortPlans(true);
      List<LoanCompHistory> currentAndFuturePlans = new List<LoanCompHistory>();
      for (int i = 0; i < this.Count; ++i)
      {
        LoanCompHistory historyAt = this.GetHistoryAt(i);
        if (historyAt.StartDate.Date > todayDate.Date || historyAt.EndDate.Date > todayDate.Date)
          currentAndFuturePlans.Add(historyAt);
      }
      return currentAndFuturePlans;
    }

    public List<LoanCompHistory> GetFuturePlans(DateTime todayDate)
    {
      this.SortPlans(true);
      List<LoanCompHistory> futurePlans = new List<LoanCompHistory>();
      for (int i = 0; i < this.Count; ++i)
      {
        LoanCompHistory historyAt = this.GetHistoryAt(i);
        if (historyAt.StartDate.Date > todayDate.Date)
          futurePlans.Add(historyAt);
      }
      return futurePlans;
    }

    public bool AddParentPlans(List<LoanCompHistory> parentPlans, DateTime todayDate)
    {
      this.SortPlans(true);
      for (int index = this.historyList.Count - 1; index >= 0; --index)
      {
        if (this.historyList[index].StartDate.Date > todayDate.Date)
          this.historyList.RemoveAt(index);
      }
      LoanCompHistory currentPlan = this.GetCurrentPlan(todayDate.Date);
      DateTime dateTime1 = DateTime.MinValue;
      if (parentPlans != null && parentPlans.Count > 0)
      {
        for (int index = 0; index < parentPlans.Count; ++index)
        {
          LoanCompHistory history = (LoanCompHistory) parentPlans[index].Clone();
          history.Id = this.userOrOrgID;
          DateTime dateTime2 = history.StartDate;
          if (dateTime2.Date <= todayDate.Date)
          {
            LoanCompHistory loanCompHistory = history;
            dateTime2 = todayDate.AddDays(1.0);
            DateTime date = dateTime2.Date;
            loanCompHistory.StartDate = date;
            if (currentPlan != null)
              currentPlan.EndDate = todayDate.Date;
          }
          DateTime date1 = dateTime1.Date;
          dateTime2 = history.StartDate;
          DateTime date2 = dateTime2.Date;
          if (date1 == date2)
          {
            LoanCompHistory loanCompHistory = history;
            dateTime2 = dateTime1.Date;
            dateTime2 = dateTime2.AddDays(1.0);
            DateTime date3 = dateTime2.Date;
            loanCompHistory.StartDate = date3;
          }
          dateTime2 = history.StartDate;
          dateTime1 = dateTime2.Date;
          this.AddHistory(history);
        }
      }
      this.SortPlans(true);
      return true;
    }

    public void SetID(string id)
    {
      this.userOrOrgID = id;
      for (int i = 0; i < this.Count; ++i)
        this.GetHistoryAt(i).Id = id;
    }

    public bool IsDirty => this.isDirty;

    public string UserName
    {
      get => this.userName;
      set => this.userName = value;
    }

    public bool UseParentInfo
    {
      set => this.useParentInfo = value;
      get => this.useParentInfo;
    }

    public bool UncheckParentInfo
    {
      set => this.uncheckParentInfo = value;
      get => this.uncheckParentInfo;
    }

    public object Clone()
    {
      LoanCompHistoryList loanCompHistoryList = new LoanCompHistoryList(this.userOrOrgID);
      for (int i = 0; i < this.Count; ++i)
        loanCompHistoryList.AddHistory((LoanCompHistory) this.GetHistoryAt(i).Clone());
      loanCompHistoryList.UseParentInfo = this.useParentInfo;
      loanCompHistoryList.UncheckParentInfo = this.uncheckParentInfo;
      return (object) loanCompHistoryList;
    }
  }
}
