// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.ConditionalLetterLayout
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class ConditionalLetterLayout
  {
    private bool useEnhancedCond;
    private List<ConditionLog> conditionsToPrint;
    private ConditionalLetterPrintOption letterOption;
    private Hashtable loanAssociates;
    private List<string[]> lineToPrints = new List<string[]>();
    private SessionObjects sessionObjects;
    private RoleInfo[] roles;

    public ConditionalLetterLayout(
      List<ConditionLog> conditionsToPrint,
      ConditionalLetterPrintOption letterOption,
      Hashtable loanAssociates,
      SessionObjects sessionObjects)
    {
      this.conditionsToPrint = conditionsToPrint;
      this.useEnhancedCond = this.conditionsToPrint != null && this.conditionsToPrint.Count > 0 && this.conditionsToPrint[0] is EnhancedConditionLog;
      this.letterOption = letterOption;
      this.loanAssociates = loanAssociates;
      this.sessionObjects = sessionObjects;
      this.sortConditions();
      this.buildLayout();
    }

    private void sortConditions()
    {
      List<object[]> objArrayList = new List<object[]>();
      for (int index = 0; index < this.conditionsToPrint.Count; ++index)
      {
        if (this.letterOption.SortBy == 1)
          objArrayList.Add(new object[2]
          {
            (object) this.getPriorToString(this.conditionsToPrint[index]),
            (object) this.conditionsToPrint[index]
          });
        else if (this.letterOption.SortBy == 2)
          objArrayList.Add(new object[2]
          {
            (object) this.conditionsToPrint[index].Category,
            (object) this.conditionsToPrint[index]
          });
        else if (this.letterOption.SortBy == 3)
          objArrayList.Add(new object[2]
          {
            (object) this.getUserNameForRoleID(this.conditionsToPrint[index]),
            (object) this.conditionsToPrint[index]
          });
        else if (this.letterOption.SortBy == 4)
          objArrayList.Add(new object[2]
          {
            (object) this.getStatus(this.conditionsToPrint[index], false),
            (object) this.conditionsToPrint[index]
          });
      }
      objArrayList.Sort((Comparison<object[]>) ((obj1, obj2) => ((string) obj1[0]).CompareTo((string) obj2[0])));
      if (this.letterOption.NeedGroup && this.letterOption.SortBy != this.letterOption.GroupBy)
      {
        for (int index = 0; index < objArrayList.Count; ++index)
        {
          object[] objArray = objArrayList[index];
          ConditionLog c = (ConditionLog) objArray[1];
          string str = string.Empty;
          if (this.letterOption.GroupBy == 1)
            str = this.getPriorToString(c);
          else if (this.letterOption.GroupBy == 2)
            str = c.Category;
          else if (this.letterOption.GroupBy == 3)
            str = this.getUserNameForRoleID(c);
          if (this.letterOption.SortBy == 4)
            str = str + ":" + this.getStatus(c, false);
          objArray[0] = (object) str;
        }
        objArrayList.Sort((Comparison<object[]>) ((obj1, obj2) => ((string) obj1[0]).CompareTo((string) obj2[0])));
      }
      this.conditionsToPrint = new List<ConditionLog>();
      for (int index = 0; index < objArrayList.Count; ++index)
        this.conditionsToPrint.Add((ConditionLog) objArrayList[index][1]);
    }

    private void buildLayout()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index1 = 0; index1 < this.conditionsToPrint.Count; ++index1)
      {
        if (index1 > 0)
          this.lineToPrints.Add((string[]) null);
        string str;
        switch (this.letterOption.GroupBy)
        {
          case 2:
            str = this.conditionsToPrint[index1].Category;
            break;
          case 3:
            str = this.getUserNameForRoleID(this.conditionsToPrint[index1]);
            break;
          default:
            str = this.getPriorToString(this.conditionsToPrint[index1]);
            break;
        }
        if (str == string.Empty)
          str = "!";
        string guid = this.conditionsToPrint[index1].Guid;
        List<string> condition = this.getCondition(this.conditionsToPrint[index1]);
        List<string[]> detail = this.getDetail(this.conditionsToPrint[index1]);
        int count = condition.Count;
        if (detail.Count > count)
          count = detail.Count;
        for (int index2 = 0; index2 < count; ++index2)
        {
          string empty3 = string.Empty;
          string[] strArray = new string[3]{ "", "", "" };
          if (index2 < condition.Count)
            empty3 = condition[index2];
          if (index2 < detail.Count)
            strArray = detail[index2];
          if (index2 == 0)
            this.lineToPrints.Add(new string[7]
            {
              str,
              guid,
              empty3,
              strArray[0],
              strArray[1],
              strArray[2],
              string.Concat((object) count)
            });
          else
            this.lineToPrints.Add(new string[7]
            {
              str,
              guid,
              empty3,
              strArray[0],
              strArray[1],
              strArray[2],
              "0"
            });
        }
      }
      if (!this.letterOption.NeedGroup)
        return;
      List<string[]> strArrayList = new List<string[]>();
      string empty4 = string.Empty;
      string str1 = "Owner: ";
      if (this.letterOption.GroupBy == 1)
        str1 = "Prior To: ";
      else if (this.letterOption.GroupBy == 2)
        str1 = "Category: ";
      for (int i = 0; i < this.lineToPrints.Count; ++i)
      {
        string[] printLine = this.GetPrintLine(i);
        if (printLine != null && empty4 != printLine[0] && printLine[0] != string.Empty)
        {
          if (this.letterOption.GroupingPage == 2 && i > 0)
            strArrayList.Add(new string[7]
            {
              "pagebreak",
              "",
              "",
              "",
              "",
              "",
              ""
            });
          strArrayList.Add(new string[7]
          {
            "",
            "",
            str1 + (printLine[0] == "!" ? "" : printLine[0]),
            "",
            "",
            "",
            ""
          });
          strArrayList.Add((string[]) null);
          empty4 = printLine[0];
        }
        strArrayList.Add(printLine);
      }
      this.lineToPrints = strArrayList;
    }

    private string getPriorToString(ConditionLog c)
    {
      string priorToString = c is UnderwritingConditionLog ? ((UnderwritingConditionLog) c).PriorTo : ((EnhancedConditionLog) c).PriorTo;
      switch (priorToString)
      {
        case "AC":
        case "At Close":
          return "Prior to Closing";
        case "Approval":
        case "PTA":
          return "Prior to Approval";
        case "Docs":
        case "PTD":
          return "Prior to Docs";
        case "Funding":
        case "PTF":
          return "Prior to Funding";
        case "PTP":
        case "Purchase":
          return "Prior to Purchase";
        default:
          return priorToString;
      }
    }

    private string getStatus(ConditionLog c, bool getStatusToPrint)
    {
      if (c is UnderwritingConditionLog)
        return getStatusToPrint ? ((UnderwritingConditionLog) c).StatusForPrint(true) : ((StandardConditionLog) c).Status.ToString();
      if (!getStatusToPrint)
        return ((EnhancedConditionLog) c).Status;
      return !((EnhancedConditionLog) c).StatusDate.HasValue ? "" : Utils.ParseDate((object) ((EnhancedConditionLog) c).StatusDate).ToString("MM/dd/yyyy");
    }

    private string getUserNameForRoleID(ConditionLog c)
    {
      if (c is UnderwritingConditionLog)
      {
        int forRoleId = ((UnderwritingConditionLog) c).ForRoleID;
        return !this.loanAssociates.ContainsKey((object) forRoleId) ? "" : this.loanAssociates[(object) forRoleId].ToString();
      }
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) ((EnhancedConditionLog) c).Definitions.TrackingDefinitions)
      {
        foreach (int allowedRole in trackingDefinition.AllowedRoles)
        {
          if (!dictionary.ContainsKey(allowedRole) && this.loanAssociates.ContainsKey((object) allowedRole))
            return this.loanAssociates[(object) allowedRole].ToString();
        }
      }
      return "";
    }

    private List<string> getCondition(ConditionLog c)
    {
      List<string> condition = new List<string>();
      condition.Add("       " + c.Title);
      if (!this.letterOption.IncludeDescription)
        return condition;
      string empty = string.Empty;
      string val = c is UnderwritingConditionLog ? ((StandardConditionLog) c).Description : ((EnhancedConditionLog) c).ExternalDescription;
      while (val != string.Empty)
      {
        string str = Utils.StringWrapping(ref val, 56, 1, 1);
        if (!(str.Trim() == string.Empty))
          condition.Add("              " + str.Trim());
      }
      return condition;
    }

    private List<string[]> getDetail(ConditionLog c)
    {
      List<string[]> detail = new List<string[]>();
      if (this.letterOption.UseDue)
        detail.Add(new string[3]
        {
          "Condition Due:",
          this.getPriorToString(c),
          ""
        });
      if (this.letterOption.UseCategory)
        detail.Add(new string[3]
        {
          "Category:",
          c.Category,
          ""
        });
      if (this.letterOption.UseOwnerName)
      {
        string fullName = this.getFullName(this.getUserNameForRoleID(c));
        detail.Add(new string[3]{ "Owner:", fullName, "" });
      }
      if (this.letterOption.UseCurrentStatus)
        detail.Add(new string[3]
        {
          "Status:",
          this.getStatus(c, false),
          this.letterOption.StatusCurrentType == 1 ? this.getStatus(c, true) : ""
        });
      if (this.letterOption.UseStatusAdded)
        detail.Add(new string[3]
        {
          "Added:",
          this.letterOption.StatusAddedType == 1 ? "" : this.getFullName(c.AddedBy),
          this.letterOption.StatusAddedType == 2 ? "" : (c.DateAdded != DateTime.MinValue ? c.DateAdded.ToString("MM/dd/yyyy") ?? "" : "")
        });
      DateTime statusDate;
      if (this.letterOption.UseStatusFulfilled)
      {
        List<string[]> strArrayList = detail;
        string[] strArray = new string[3]
        {
          "Fulfilled:",
          this.letterOption.StatusFulfilledType == 1 ? "" : this.getFullName(this.getUserID(c, "Fulfilled")),
          null
        };
        string str;
        if (this.letterOption.StatusFulfilledType != 2)
        {
          if (!(this.getStatusDate(c, "Fulfilled") != DateTime.MinValue))
          {
            str = "";
          }
          else
          {
            statusDate = this.getStatusDate(c, "Fulfilled");
            str = statusDate.ToString("MM/dd/yyyy") ?? "";
          }
        }
        else
          str = "";
        strArray[2] = str;
        strArrayList.Add(strArray);
      }
      if (this.letterOption.UseStatusReceived)
      {
        List<string[]> strArrayList = detail;
        string[] strArray = new string[3]
        {
          "Received:",
          this.letterOption.StatusReceivedType == 1 ? "" : this.getFullName(this.getUserID(c, "Received")),
          null
        };
        string str;
        if (this.letterOption.StatusReceivedType != 2)
        {
          if (!(this.getStatusDate(c, "Received") != DateTime.MinValue))
          {
            str = "";
          }
          else
          {
            statusDate = this.getStatusDate(c, "Received");
            str = statusDate.ToString("MM/dd/yyyy") ?? "";
          }
        }
        else
          str = "";
        strArray[2] = str;
        strArrayList.Add(strArray);
      }
      if (this.letterOption.UseStatusReviewed)
      {
        List<string[]> strArrayList = detail;
        string[] strArray = new string[3]
        {
          "Reviewed:",
          this.letterOption.StatusReviewedType == 1 ? "" : this.getFullName(this.getUserID(c, "Reviewed")),
          null
        };
        string str;
        if (this.letterOption.StatusReviewedType != 2)
        {
          if (!(this.getStatusDate(c, "Reviewed") != DateTime.MinValue))
          {
            str = "";
          }
          else
          {
            statusDate = this.getStatusDate(c, "Reviewed");
            str = statusDate.ToString("MM/dd/yyyy") ?? "";
          }
        }
        else
          str = "";
        strArray[2] = str;
        strArrayList.Add(strArray);
      }
      if (this.letterOption.UseStatusRejected)
      {
        List<string[]> strArrayList = detail;
        string[] strArray = new string[3]
        {
          "Rejected:",
          this.letterOption.StatusRejectedType == 1 ? "" : this.getFullName(this.getUserID(c, "Rejected")),
          null
        };
        string str;
        if (this.letterOption.StatusRejectedType != 2)
        {
          if (!(this.getStatusDate(c, "Rejected") != DateTime.MinValue))
          {
            str = "";
          }
          else
          {
            statusDate = this.getStatusDate(c, "Rejected");
            str = statusDate.ToString("MM/dd/yyyy") ?? "";
          }
        }
        else
          str = "";
        strArray[2] = str;
        strArrayList.Add(strArray);
      }
      if (this.letterOption.UseStatusCleared)
      {
        List<string[]> strArrayList = detail;
        string[] strArray = new string[3]
        {
          "Cleared:",
          this.letterOption.StatusClearedType == 1 ? "" : this.getFullName(this.getUserID(c, "Cleared")),
          null
        };
        string str;
        if (this.letterOption.StatusClearedType != 2)
        {
          if (!(this.getStatusDate(c, "Cleared") != DateTime.MinValue))
          {
            str = "";
          }
          else
          {
            statusDate = this.getStatusDate(c, "Cleared");
            str = statusDate.ToString("MM/dd/yyyy") ?? "";
          }
        }
        else
          str = "";
        strArray[2] = str;
        strArrayList.Add(strArray);
      }
      if (this.letterOption.UseStatusWaived)
      {
        List<string[]> strArrayList = detail;
        string[] strArray = new string[3]
        {
          "Waived:",
          this.letterOption.StatusWaivedType == 1 ? "" : this.getFullName(this.getUserID(c, "Waived")),
          null
        };
        string str;
        if (this.letterOption.StatusWaivedType != 2)
        {
          if (!(this.getStatusDate(c, "Waived") != DateTime.MinValue))
          {
            str = "";
          }
          else
          {
            statusDate = this.getStatusDate(c, "Waived");
            str = statusDate.ToString("MM/dd/yyyy") ?? "";
          }
        }
        else
          str = "";
        strArray[2] = str;
        strArrayList.Add(strArray);
      }
      return detail;
    }

    private string getUserID(ConditionLog c, string statusType)
    {
      if (c is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) c;
        switch (statusType)
        {
          case "Fulfilled":
            return underwritingConditionLog.FulfilledBy;
          case "Received":
            return underwritingConditionLog.ReceivedBy;
          case "Reviewed":
            return underwritingConditionLog.ReviewedBy;
          case "Rejected":
            return underwritingConditionLog.RejectedBy;
          case "Cleared":
            return underwritingConditionLog.ClearedBy;
          case "Waived":
            return underwritingConditionLog.WaivedBy;
        }
      }
      else
      {
        List<StatusTrackingEntry> statusTrackingEntries = ((EnhancedConditionLog) c).Trackings?.GetStatusTrackingEntries();
        if (statusTrackingEntries == null || statusTrackingEntries.Count == 0)
          return "";
        for (int index = 0; index < statusTrackingEntries.Count; ++index)
        {
          if (string.Compare(statusTrackingEntries[index].Status, statusType, true) == 0)
            return statusTrackingEntries[index].UserId;
        }
      }
      return "";
    }

    private string getFullName(string userID) => this.getFullName(userID, true);

    private string getFullName(string userID, bool getUserInfo)
    {
      if (userID == string.Empty)
        return userID;
      try
      {
        string str;
        if (getUserInfo)
        {
          UserInfo user = this.sessionObjects.OrganizationManager.GetUser(userID);
          if (user.FirstName == string.Empty)
            return user.FullName;
          str = user.FirstName.Substring(0, 1).ToUpper() + ". " + user.LastName;
        }
        else
        {
          str = userID;
          int num = str.Trim().IndexOf(" ");
          if (num > -1)
            str = str.Trim().Substring(0, 1).ToUpper() + ". " + str.Trim().Substring(num + 1);
        }
        return str.Trim();
      }
      catch (Exception ex)
      {
        return userID;
      }
    }

    private DateTime getStatusDate(ConditionLog c, string statusType)
    {
      if (c is UnderwritingConditionLog)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) c;
        switch (statusType)
        {
          case "Fulfilled":
            return underwritingConditionLog.DateFulfilled;
          case "Received":
            return underwritingConditionLog.DateReceived;
          case "Reviewed":
            return underwritingConditionLog.DateReviewed;
          case "Rejected":
            return underwritingConditionLog.DateRejected;
          case "Cleared":
            return underwritingConditionLog.DateCleared;
          case "Waived":
            return underwritingConditionLog.DateWaived;
        }
      }
      else
      {
        List<StatusTrackingEntry> statusTrackingEntries = ((EnhancedConditionLog) c).Trackings?.GetStatusTrackingEntries();
        if (statusTrackingEntries == null || statusTrackingEntries.Count == 0)
          return DateTime.MinValue;
        for (int index = 0; index < statusTrackingEntries.Count; ++index)
        {
          if (string.Compare(statusTrackingEntries[index].Status, statusType, true) == 0)
            return statusTrackingEntries[index].Date;
        }
      }
      return DateTime.MinValue;
    }

    private string getRoleName(int roleId)
    {
      if (this.roles == null)
        this.roles = this.sessionObjects.BpmManager.GetAllRoleFunctions();
      if (this.roles != null && this.roles.Length != 0)
      {
        foreach (RoleInfo role in this.roles)
        {
          if (role.RoleID == roleId)
            return role.RoleName;
        }
      }
      return string.Empty;
    }

    internal int LineCount => this.lineToPrints.Count;

    internal string[] GetPrintLine(int i) => this.lineToPrints[i];
  }
}
