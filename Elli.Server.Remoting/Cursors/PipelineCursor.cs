// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.PipelineCursor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public class PipelineCursor : CursorBase
  {
    private const string className = "PipelineCursor";
    private string[] fields;
    private PipelineData dataToInclude = PipelineData.All;

    public PipelineCursor Initialize(
      ISession session,
      PipelineInfo[] pinfoArray,
      PipelineSortOrder sortOrder)
    {
      this.InitializeInternal(session);
      this.sortLoans(pinfoArray, sortOrder);
      for (int index = 0; index < pinfoArray.Length; ++index)
        this.Items.Add((object) pinfoArray[index].GUID);
      return this;
    }

    public PipelineCursor Initialize(ISession session, string[] guids)
    {
      this.InitializeInternal(session);
      this.AddRange((object[]) guids);
      return this;
    }

    public PipelineCursor Initialize(
      ISession session,
      PipelineInfo[] pinfoArray,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder)
    {
      this.Initialize(session, pinfoArray, sortOrder);
      this.fields = fields;
      this.dataToInclude = dataToInclude;
      return this;
    }

    public PipelineCursor Initialize(
      ISession session,
      string folderName,
      LoanInfo.Right rights,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.Initialize(session, folderName, rights, (string[]) null, PipelineData.All, sortOrder, filter, isExternalOrganization);
    }

    public PipelineCursor Initialize(
      ISession session,
      LoanInfo.Right rights,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.Initialize(session, (string) null, rights, (string[]) null, dataToInclude, sortOrder, filter, isExternalOrganization);
    }

    public PipelineCursor Initialize(
      ISession session,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      return this.Initialize(session, (string) null, rights, fields, dataToInclude, sortOrder, filter, isExternalOrganization);
    }

    public PipelineCursor Initialize(
      ISession session,
      string folderName,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization,
      bool excludeArchivedLoans = false)
    {
      this.InitializeInternal(session);
      this.fields = fields;
      this.dataToInclude = dataToInclude;
      string[] strArray;
      if (!string.IsNullOrWhiteSpace(folderName))
        strArray = new string[1]{ folderName };
      else
        strArray = (string[]) null;
      string[] folderNames = strArray;
      UserInfo userInfo = UserStore.GetLatestVersion(session.UserID).UserInfo;
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("PipelineCursor Generation", 84, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\Cursors\\PipelineCursor.cs"))
      {
        PipelineInfo[] pinfoArray;
        switch (sortOrder)
        {
          case PipelineSortOrder.Alert:
            pinfoArray = Pipeline.Generate(userInfo, folderNames, rights, new string[2]
            {
              "BorrowerFirstName",
              "BorrowerLastName"
            }, PipelineData.AlertSummary, filter, (SortField[]) null, (ICriterionTranslator) null, (isExternalOrganization ? 1 : 0) != 0, 0, TradeType.None, new int?(), (excludeArchivedLoans ? 1 : 0) != 0);
            break;
          case PipelineSortOrder.LastName:
            pinfoArray = Pipeline.Generate(userInfo, folderNames, rights, new string[2]
            {
              "BorrowerFirstName",
              "BorrowerLastName"
            }, PipelineData.Fields, filter, (SortField[]) null, (ICriterionTranslator) null, (isExternalOrganization ? 1 : 0) != 0, 0, TradeType.None, new int?(), (excludeArchivedLoans ? 1 : 0) != 0);
            break;
          case PipelineSortOrder.Milestone:
            pinfoArray = Pipeline.Generate(userInfo, folderNames, rights, new string[8]
            {
              "BorrowerFirstName",
              "BorrowerLastName",
              "CurrentMilestoneName",
              "NextMilestoneName",
              "CurrentMilestoneDate",
              "NextMilestoneDate",
              "CurrentCoreMilestoneName",
              "NextCoreMilestoneName"
            }, PipelineData.Fields, filter, (SortField[]) null, (ICriterionTranslator) null, (isExternalOrganization ? 1 : 0) != 0, 0, TradeType.None, new int?(), (excludeArchivedLoans ? 1 : 0) != 0);
            break;
          case PipelineSortOrder.RateLock:
            pinfoArray = Pipeline.Generate(userInfo, folderNames, rights, new string[12]
            {
              "BorrowerFirstName",
              "BorrowerLastName",
              "CurrentMilestoneName",
              "NextMilestoneName",
              "CurrentMilestoneDate",
              "NextMilestoneDate",
              "CurrentCoreMilestoneName",
              "NextCoreMilestoneName",
              "LockExpirationDate",
              "RateLockStatus",
              "LockRequestDate",
              "loanStatus"
            }, PipelineData.Fields, filter, (SortField[]) null, (ICriterionTranslator) null, (isExternalOrganization ? 1 : 0) != 0, 0, TradeType.None, new int?(), (excludeArchivedLoans ? 1 : 0) != 0);
            break;
          default:
            pinfoArray = Pipeline.Generate(userInfo, folderNames, rights, new string[0], PipelineData.Fields, filter, (SortField[]) null, (ICriterionTranslator) null, isExternalOrganization, 0, TradeType.None, new int?(), excludeArchivedLoans);
            break;
        }
        performanceMeter.AddCheckpoint("Retrieved Database data for pipeline", 106, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\Cursors\\PipelineCursor.cs");
        this.sortLoans(pinfoArray, sortOrder);
        performanceMeter.AddCheckpoint("Sorted loans", 110, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\Cursors\\PipelineCursor.cs");
        for (int index = 0; index < pinfoArray.Length; ++index)
          this.Items.Add((object) pinfoArray[index].GUID);
        performanceMeter.AddCheckpoint("Completed build of pipeline", 116, nameof (Initialize), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\Cursors\\PipelineCursor.cs");
      }
      return this;
    }

    public override object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      return this.GetItems(startIndex, count, isExternalOrganization, 0);
    }

    public override object[] GetItems(
      int startIndex,
      int count,
      bool isExternalOrganization,
      int sqlRead)
    {
      return this.GetItems(startIndex, count, isExternalOrganization, 0, false);
    }

    public override object[] GetItems(
      int startIndex,
      int count,
      bool isExternalOrganization,
      int sqlRead,
      bool excludeArchivedLoans)
    {
      CursorBase.CursorApi(this.Session, nameof (PipelineCursor), nameof (GetItems), (object) startIndex, (object) count);
      string[] array = (string[]) this.Items.GetRange(startIndex, count).ToArray(typeof (string));
      return (object[]) Pipeline.Generate(this.Session.GetUserInfo(), array, this.fields, this.dataToInclude, isExternalOrganization, sqlRead, excludeArchivedLoans: excludeArchivedLoans);
    }

    public override object GetItem(int index, bool isExternalOrganization)
    {
      return this.GetItems(index, 1, isExternalOrganization)[0];
    }

    public override object GetItem(
      int index,
      bool isExternalOrganization,
      bool excludeArchivedLoans)
    {
      return this.GetItems(index, 1, isExternalOrganization, 0, excludeArchivedLoans)[0];
    }

    private void sortLoans(PipelineInfo[] pinfoArray, PipelineSortOrder sortOrder)
    {
      string[] keys = (string[]) null;
      using (PerformanceMeter.Current.BeginOperation("PipelineCursor.getXXXSortKeys"))
      {
        switch (sortOrder)
        {
          case PipelineSortOrder.Alert:
            keys = this.getAlertSortKeys(pinfoArray);
            break;
          case PipelineSortOrder.LastName:
            keys = this.getLastNameSortKeys(pinfoArray);
            break;
          case PipelineSortOrder.Milestone:
            keys = this.getMilestoneSortKeys(pinfoArray);
            break;
          case PipelineSortOrder.RateLock:
            keys = this.getRateLockSortKeys(pinfoArray);
            break;
        }
      }
      if (keys == null)
        return;
      using (PerformanceMeter.Current.BeginOperation("PipelineCursor.sortKeyArray"))
        Array.Sort<string, PipelineInfo>(keys, pinfoArray);
    }

    private string[] getAlertSortKeys(PipelineInfo[] pinfoArray)
    {
      int length = pinfoArray.Length;
      string[] alertSortKeys = new string[length];
      for (int index = 0; index < length; ++index)
      {
        PipelineInfo pinfo = pinfoArray[index];
        string str = (99 - pinfo.AlertSummary.AlertCount).ToString("00");
        if (pinfo.AlertSummary.AlertCount > 0)
          str += pinfo.AlertSummary.EarliestAlertTime.ToString("yyyyMMdd");
        alertSortKeys[index] = str + pinfo.LastName;
      }
      return alertSortKeys;
    }

    private string[] getLastNameSortKeys(PipelineInfo[] pinfoArray)
    {
      int length = pinfoArray.Length;
      string[] lastNameSortKeys = new string[length];
      for (int index = 0; index < length; ++index)
        lastNameSortKeys[index] = pinfoArray[index].LastName + " " + pinfoArray[index].FirstName;
      return lastNameSortKeys;
    }

    private string[] getMilestoneSortKeys(PipelineInfo[] pinfoArray)
    {
      int length = pinfoArray.Length;
      string[] milestoneSortKeys = new string[length];
      Hashtable milestoneWeightTable = this.createMilestoneWeightTable();
      for (int index = 0; index < length; ++index)
      {
        PipelineInfo pinfo = pinfoArray[index];
        string key1 = (string) pinfo.Info[(object) "CurrentCoreMilestoneName"];
        string str1 = ((int) milestoneWeightTable[(object) key1]).ToString("0000");
        string key2 = (string) pinfo.Info[(object) "CurrentMilestoneName"];
        object obj1 = milestoneWeightTable[(object) key2];
        string str2 = obj1 == null ? str1 + "9999" : str1 + ((int) obj1).ToString("0000");
        object obj2 = pinfo.Info[key2 == "Started " || key2 == "Completion" ? (object) "CurrentMilestoneDate" : (object) "NextMilestoneDate"];
        bool flag = false;
        if (obj2 is DateTime dateTime && dateTime.Date != DateTime.MinValue.Date)
        {
          flag = true;
          str2 += dateTime.ToString("yyyyMMdd");
        }
        if (!flag)
          str2 += "99999999";
        milestoneSortKeys[index] = str2;
      }
      return milestoneSortKeys;
    }

    private Hashtable createMilestoneWeightTable()
    {
      Hashtable milestoneWeightTable = new Hashtable();
      List<EllieMae.EMLite.Workflow.Milestone> milestones = WorkflowBpmDbAccessor.GetMilestones(false);
      for (int index = 0; index < milestones.Count; ++index)
      {
        EllieMae.EMLite.Workflow.Milestone milestone = milestones[index];
        milestoneWeightTable[(object) milestone.Name] = (object) (index * 100);
      }
      return milestoneWeightTable;
    }

    private string[] getRateLockSortKeys(PipelineInfo[] pinfoArray)
    {
      int length = pinfoArray.Length;
      string[] rateLockSortKeys = new string[length];
      Hashtable milestoneWeightTable = this.createMilestoneWeightTable();
      DateTime now = DateTime.Now;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      for (int index = 0; index < length; ++index)
      {
        PipelineInfo pinfo = pinfoArray[index];
        int num1 = 90000000;
        int num2 = 90000000;
        string str1 = (string) pinfo.Info[(object) "NextMilestoneName"];
        string str2 = (string) pinfo.Info[(object) "RateLockStatus"];
        DateTime date1 = Utils.ParseDate(pinfo.Info[(object) "LockExpirationDate"]);
        DateTime date2 = Utils.ParseDate(pinfo.Info[(object) "LockRequestDate"]);
        string str3 = pinfo.Info[(object) "loanStatus"].ToString();
        DateTime date3 = Utils.ParseDate(pinfo.Info[(object) "CurrentMilestoneDate"]);
        int num3 = 9999 - (int) (milestoneWeightTable[pinfo.Info[(object) "CurrentMilestoneName"]] ?? milestoneWeightTable[pinfo.Info[(object) "CurrentCoreMilestoneName"]]);
        if (num3 < 0)
          num3 = 0;
        string empty4 = string.Empty;
        TimeSpan timeSpan;
        if (str1 == empty4)
        {
          timeSpan = now.Date - date3.Date;
          num1 = (int) timeSpan.TotalMinutes + 90000000;
        }
        else if (str3 != "1" && str3 != "0" && str3 != "6")
        {
          num1 = 80000000;
          timeSpan = date3.Date - now.Date;
          num2 = (int) timeSpan.TotalMinutes + 50000000;
        }
        else if (str2 == "1")
        {
          timeSpan = date2 - now;
          num1 = (int) timeSpan.TotalMinutes + 10000000;
        }
        else if (date1 != DateTime.MinValue)
        {
          timeSpan = date1.Date - now.Date;
          int days = timeSpan.Days;
          switch (days)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
              timeSpan = date1 - now;
              num1 = (int) timeSpan.TotalMinutes + 20000000;
              break;
            default:
              if (days < 0)
              {
                timeSpan = now - date1;
                num1 = (int) timeSpan.TotalMinutes + 30000000;
                break;
              }
              if (days > 5)
              {
                timeSpan = date1 - now;
                num1 = (int) timeSpan.TotalMinutes + 40000000;
                break;
              }
              break;
          }
        }
        else
        {
          num1 = 50000000;
          timeSpan = date3.Date - now.Date;
          num2 = (int) timeSpan.TotalMinutes + 50000000;
        }
        rateLockSortKeys[index] = num1.ToString("00000000") + num3.ToString("0000") + num2.ToString("00000000") + pinfo.LastName + " " + pinfo.FirstName;
      }
      return rateLockSortKeys;
    }
  }
}
