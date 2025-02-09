// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogAlertList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LogAlertList : CollectionBase
  {
    private LogRecordBase parent;
    private bool isDirty;
    private ArrayList otherSystemAlerts = new ArrayList();

    internal LogAlertList(LogRecordBase parent) => this.parent = parent;

    internal LogAlertList(LogRecordBase parent, XmlElement e)
      : this(parent)
    {
      foreach (XmlElement selectNode in e.SelectNodes("Alert"))
      {
        LogAlert logAlert = new LogAlert(selectNode);
        if (logAlert.SystemID == parent.Log.Loan.SystemID)
          this.Add(logAlert);
        else
          this.otherSystemAlerts.Add((object) logAlert);
      }
      this.isDirty = false;
    }

    public LogAlert this[int index]
    {
      get => (LogAlert) this.List[index];
      set
      {
        this.List[index] = (object) value;
        this.isDirty = true;
      }
    }

    public int Add(LogAlert logAlert)
    {
      if (this.List.Contains((object) logAlert))
        return this.List.IndexOf((object) logAlert);
      this.isDirty = true;
      return this.List.Add((object) logAlert);
    }

    public void Insert(int index, LogAlert logAlert)
    {
      if (this.List.Contains((object) logAlert))
        return;
      this.isDirty = true;
      this.List.Insert(index, (object) logAlert);
    }

    public void Remove(LogAlert logAlert)
    {
      this.isDirty = true;
      this.List.Remove((object) logAlert);
    }

    public bool Contains(LogAlert logAlert) => this.List.Contains((object) logAlert);

    public LogAlert[] GetAlertsForRoles(int[] roleIds)
    {
      ArrayList arrayList = new ArrayList();
      foreach (LogAlert logAlert in (CollectionBase) this)
      {
        if (Array.IndexOf<int>(roleIds, logAlert.RoleId) >= 0)
          arrayList.Add((object) logAlert);
      }
      return (LogAlert[]) arrayList.ToArray(typeof (LogAlert));
    }

    protected override void OnValidate(object value)
    {
      if (value == null)
        throw new ArgumentNullException();
      if (this.parent.Log == null)
        return;
      ((LogAlert) value).SetSystemID(this.parent.Log.Loan.SystemID);
    }

    public bool IsDirty()
    {
      if (this.isDirty)
        return true;
      foreach (LogAlert logAlert in (CollectionBase) this)
      {
        if (logAlert.IsDirty)
          return true;
      }
      return false;
    }

    public void ClearAllDirtyItems()
    {
      this.isDirty = false;
      foreach (LogAlert logAlert in (CollectionBase) this)
        logAlert.IsDirty = false;
    }

    public bool IsFollowUpRequired()
    {
      if (this.Count == 0)
        return false;
      foreach (LogAlert logAlert in (CollectionBase) this)
      {
        if (0 < logAlert.RoleId && logAlert.FollowedUpDate.Equals(DateTime.MinValue))
          return true;
      }
      return false;
    }

    public LogAlert GetMostCriticalAlert()
    {
      if (this.Count == 0)
        return (LogAlert) null;
      int index1 = -1;
      int index2 = -1;
      DateTime dateTime1 = DateTime.MaxValue;
      DateTime dateTime2 = DateTime.MinValue;
      for (int index3 = 0; index3 < this.Count; ++index3)
      {
        if (this.parent.Log != null && 0 < this[index3].RoleId && this.parent.Log.Loan.AccessRules.IsAlertApplicableToUser(this[index3]))
        {
          if (DateTime.MinValue == this[index3].FollowedUpDate)
          {
            if (this[index3].DueDate < dateTime1)
            {
              dateTime1 = this[index3].DueDate;
              index1 = index3;
            }
          }
          else if (this[index3].FollowedUpDate > dateTime2)
          {
            dateTime2 = this[index3].FollowedUpDate;
            index2 = index3;
          }
        }
      }
      if (-1 != index1)
        return this[index1];
      return -1 == index2 ? (LogAlert) null : this[index2];
    }

    public bool DidUserFollowUp()
    {
      if (this.Count == 0)
        return false;
      foreach (LogAlert logAlert in (CollectionBase) this)
      {
        if (!logAlert.IsFollowedUp && DateTime.MinValue != logAlert.FollowedUpDate)
          return true;
      }
      return false;
    }

    internal void SetSystemIDs(string systemId)
    {
      foreach (LogAlert logAlert in (CollectionBase) this)
        logAlert.SetSystemID(systemId);
    }

    internal PipelineInfo.Alert[] ToPipelineAlerts(StandardAlertID alertId, string description)
    {
      ArrayList arrayList1 = new ArrayList();
      for (int index = 0; index < this.Count; ++index)
      {
        LogAlert logAlert = this[index];
        if (logAlert.FollowedUpDate == DateTime.MinValue && logAlert.RoleId > 0)
        {
          ArrayList arrayList2 = new ArrayList();
          ArrayList arrayList3 = new ArrayList();
          foreach (LoanAssociateLog assignedAssociate in this.parent.Log.GetAssignedAssociates(logAlert.RoleId))
          {
            if (assignedAssociate.LoanAssociateType == LoanAssociateType.User && !arrayList2.Contains((object) assignedAssociate.LoanAssociateID))
            {
              PipelineInfo.Alert alert = new PipelineInfo.Alert((int) alertId, description, logAlert.RoleId.ToString(), logAlert.DueDate == DateTime.MinValue ? DateTime.Today : logAlert.DueDate, assignedAssociate.LoanAssociateID, -1, this.parent.Guid + index.ToString(), this.parent.Guid);
              arrayList1.Add((object) alert);
              arrayList2.Add((object) assignedAssociate.LoanAssociateID);
            }
            else if (assignedAssociate.LoanAssociateType == LoanAssociateType.Group && !arrayList3.Contains((object) assignedAssociate.LoanAssociateID))
            {
              PipelineInfo.Alert alert = new PipelineInfo.Alert((int) alertId, description, logAlert.RoleId.ToString(), logAlert.DueDate == DateTime.MinValue ? DateTime.Today : logAlert.DueDate, (string) null, Utils.ParseInt((object) assignedAssociate.LoanAssociateID), this.parent.Guid + index.ToString(), this.parent.Guid);
              arrayList1.Add((object) alert);
              arrayList3.Add((object) assignedAssociate.LoanAssociateID);
            }
          }
        }
      }
      return (PipelineInfo.Alert[]) arrayList1.ToArray(typeof (PipelineInfo.Alert));
    }

    public void ToXml(XmlElement parentElement)
    {
      foreach (LogAlert logAlert in (CollectionBase) this)
        logAlert.ToXml(parentElement);
      foreach (LogAlert otherSystemAlert in this.otherSystemAlerts)
        otherSystemAlert.ToXml(parentElement);
    }
  }
}
