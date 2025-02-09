// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LOCompAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class LOCompAccessor
  {
    private const string className = "LOCompAccessor�";
    private static string tableName = "CompPlans";

    public static LoanCompDefaultPlan GetDefaultLoanCompPlan()
    {
      LoanCompDefaultPlan defaultLoanCompPlan = new LoanCompDefaultPlan();
      Hashtable companySettings = Company.GetCompanySettings("LOCompDefault");
      defaultLoanCompPlan.PlanType = (LoanCompPlanType) Enum.ToObject(typeof (LoanCompPlanType), Convert.ToInt32(string.Concat(companySettings[(object) "PlanType"]) == "" ? (object) "0" : companySettings[(object) "PlanType"]));
      defaultLoanCompPlan.TriggerField = (string) companySettings[(object) "TriggerField"];
      defaultLoanCompPlan.MinTermDays = Convert.ToInt32(string.Concat(companySettings[(object) "MinTermDays"]) == "" ? (object) "0" : companySettings[(object) "MinTermDays"]);
      defaultLoanCompPlan.RoundingMethod = Convert.ToInt32(string.Concat(companySettings[(object) "RoundingMethod"]) == "" ? (object) "0" : companySettings[(object) "RoundingMethod"]);
      defaultLoanCompPlan.LoansExempt = Convert.ToInt32(string.Concat(companySettings[(object) "LoansExempt"]) == "" ? (object) "0" : companySettings[(object) "LoansExempt"]);
      defaultLoanCompPlan.PaidBy = (LoanCompPaidBy) Enum.ToObject(typeof (LoanCompPaidBy), Convert.ToInt32(string.Concat(companySettings[(object) "LoanCompPaidBy"]) == "" ? (object) "0" : companySettings[(object) "LoanCompPaidBy"]));
      return defaultLoanCompPlan;
    }

    public static void SetLoanCompDefaultPlan(LoanCompDefaultPlan loanCompDefaultPlan)
    {
      Company.SetCompanySetting("LOCompDefault", "PlanType", loanCompDefaultPlan != null ? (loanCompDefaultPlan.PlanType == LoanCompPlanType.LoanOfficer ? "1" : (loanCompDefaultPlan.PlanType == LoanCompPlanType.Broker ? "2" : (loanCompDefaultPlan.PlanType == LoanCompPlanType.Both ? "3" : ""))) : "");
      Company.SetCompanySetting("LOCompDefault", "TriggerField", loanCompDefaultPlan != null ? loanCompDefaultPlan.TriggerField : "");
      int num;
      string str1;
      if (loanCompDefaultPlan == null)
      {
        str1 = "";
      }
      else
      {
        num = loanCompDefaultPlan.MinTermDays;
        str1 = num.ToString("0");
      }
      Company.SetCompanySetting("LOCompDefault", "MinTermDays", str1);
      string str2;
      if (loanCompDefaultPlan == null)
      {
        str2 = "";
      }
      else
      {
        num = loanCompDefaultPlan.RoundingMethod;
        str2 = num.ToString();
      }
      Company.SetCompanySetting("LOCompDefault", "RoundingMethod", str2);
      string str3;
      if (loanCompDefaultPlan == null)
      {
        str3 = "";
      }
      else
      {
        num = loanCompDefaultPlan.LoansExempt;
        str3 = num.ToString();
      }
      Company.SetCompanySetting("LOCompDefault", "LoansExempt", str3);
      Company.SetCompanySetting("LOCompDefault", "LoanCompPaidBy", loanCompDefaultPlan != null ? (loanCompDefaultPlan.PaidBy == LoanCompPaidBy.Lender ? "1" : (loanCompDefaultPlan.PaidBy == LoanCompPaidBy.Borrower ? "2" : (loanCompDefaultPlan.PaidBy == LoanCompPaidBy.Exempt ? "3" : ""))) : "");
    }

    [PgReady]
    public static List<LoanCompPlan> GetAllCompPlans(bool activatedOnly, int compType)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * FROM [CompPlans] ");
        if (activatedOnly)
          pgDbQueryBuilder.AppendLine("WHERE status = 1");
        if (compType > 0)
          pgDbQueryBuilder.AppendLine(" AND [type] IN (" + (object) compType + ", 3)");
        List<LoanCompPlan> allCompPlans = new List<LoanCompPlan>();
        foreach (DataRow row in (InternalDataCollectionBase) pgDbQueryBuilder.ExecuteSetQuery().Tables[0].Rows)
          allCompPlans.Add(LOCompAccessor.getLoanCompPlansFromDatarow(row));
        return allCompPlans;
      }
      DbTableInfo table = DbAccessManager.GetTable(LOCompAccessor.tableName);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(table);
      DbValueList values = new DbValueList();
      if (activatedOnly)
        values.Add((DbValue) new DbFilterValue(table, "status", "status", (object) "1"));
      if (compType > 0)
        values.Add((DbValue) new DbFilterValue(table, "type", "type", (object) new List<string>()
        {
          compType.ToString(),
          "3"
        }));
      dbQueryBuilder.Where(values);
      List<LoanCompPlan> allCompPlans1 = new List<LoanCompPlan>();
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        allCompPlans1.Add(LOCompAccessor.getLoanCompPlansFromDatarow(row));
      return allCompPlans1;
    }

    public static LoanCompPlan GetLoanCompPlanByID(int ID)
    {
      List<LoanCompPlan> loanCompPlanList = new List<LoanCompPlan>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [CompPlans] WHERE [id] = " + SQL.Encode((object) ID));
      IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
          return LOCompAccessor.getLoanCompPlansFromDatarow((DataRow) enumerator.Current);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return (LoanCompPlan) null;
    }

    private static LoanCompPlan getLoanCompPlansFromDatarow(DataRow row)
    {
      return new LoanCompPlan()
      {
        Id = Convert.ToInt32(row["Id"]),
        Name = string.Concat(SQL.Decode(row["name"])),
        Description = string.Concat(SQL.Decode(row["description"], (object) "")),
        PlanType = Convert.ToInt32(row["type"]) == 1 ? LoanCompPlanType.LoanOfficer : (Convert.ToInt32(row["type"]) == 2 ? LoanCompPlanType.Broker : (Convert.ToInt32(row["type"]) == 3 ? LoanCompPlanType.Both : LoanCompPlanType.None)),
        Status = (bool) row["status"],
        ActivationDate = SQL.DecodeDateTime(row["activationDate"], DateTime.MaxValue),
        MinTermDays = Convert.ToInt32(row["minTermDays"]),
        PercentAmt = Convert.ToDecimal(row["percentAmt"]),
        PercentAmtBase = Convert.ToInt32(row["percentAmtBase"]),
        RoundingMethod = Convert.ToInt32(row["roundingMethod"]),
        DollarAmount = Convert.ToDecimal(row["dollarAmt"] == DBNull.Value ? (object) "0" : row["dollarAmt"]),
        MinDollarAmount = Convert.ToDecimal(row["minDollarAmt"]),
        MaxDollarAmount = Convert.ToDecimal(row["maxDollarAmt"]),
        TriggerField = string.Concat(SQL.Decode(row["triggerFieldID"], (object) ""))
      };
    }

    public static int AddCompPlan(LoanCompPlan newLoanCompPlan)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      TraceLog.WriteInfo(nameof (LOCompAccessor), "LOCompAccessor.AddCompPlan: Check duplicated record in table 'CompPlans'.");
      dbQueryBuilder.AppendLine("SELECT 1 FROM [CompPlans] WHERE [name] = " + SQL.Encode((object) newLoanCompPlan.Name));
      try
      {
        TraceLog.WriteInfo(nameof (LOCompAccessor), dbQueryBuilder.ToString());
        if (Utils.ParseInt(dbQueryBuilder.ExecuteScalar()) > -1)
          return -1;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (LOCompAccessor), ex);
        throw new Exception("Cannot duplicated record in table CompPlans table Due to the following problem:\r\n" + ex.Message);
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.Declare("@id", "int");
      TraceLog.WriteInfo(nameof (LOCompAccessor), "LOCompAccessor.AddCompPlan: Creating SQL commands for table 'CompPlans'.");
      dbQueryBuilder.AppendLine("INSERT INTO CompPlans ([name], [description], [type], [status], [activationDate], [minTermDays], [percentAmt], [percentAmtBase], [roundingMethod], [dollarAmt], [minDollarAmt], [maxDollarAmt], [triggerFieldID]) VALUES (" + SQL.Encode((object) newLoanCompPlan.Name) + "," + SQL.Encode((object) newLoanCompPlan.Description) + "," + (object) (newLoanCompPlan.PlanType == LoanCompPlanType.LoanOfficer ? 1 : (newLoanCompPlan.PlanType == LoanCompPlanType.Broker ? 2 : (newLoanCompPlan.PlanType == LoanCompPlanType.Both ? 3 : 0))) + "," + (newLoanCompPlan.Status ? (object) "'True'" : (object) "'False'") + "," + SQL.EncodeDateTime(newLoanCompPlan.ActivationDate, DateTime.MaxValue, true) + "," + (object) newLoanCompPlan.MinTermDays + "," + (object) newLoanCompPlan.PercentAmt + "," + (object) newLoanCompPlan.PercentAmtBase + "," + (object) newLoanCompPlan.RoundingMethod + "," + (object) newLoanCompPlan.DollarAmount + "," + (object) newLoanCompPlan.MinDollarAmount + "," + (object) newLoanCompPlan.MaxDollarAmount + "," + SQL.Encode((object) newLoanCompPlan.TriggerField) + ")");
      dbQueryBuilder.SelectIdentity("@id");
      dbQueryBuilder.Select("@id");
      try
      {
        TraceLog.WriteInfo(nameof (LOCompAccessor), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (LOCompAccessor), ex);
        throw new Exception("Cannot update CompPlans table Due to the following problem:\r\n" + ex.Message);
      }
    }

    public static int UpdateCompPlan(LoanCompPlan loanCompPlan)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      TraceLog.WriteInfo(nameof (LOCompAccessor), "LOCompAccessor.UpdateCompPlan: Check duplicated record in table 'CompPlans'.");
      dbQueryBuilder.AppendLine("SELECT 1 FROM [CompPlans] WHERE [name] = " + SQL.Encode((object) loanCompPlan.Name) + "AND [id] <> " + (object) loanCompPlan.Id);
      try
      {
        TraceLog.WriteInfo(nameof (LOCompAccessor), dbQueryBuilder.ToString());
        if (Utils.ParseInt(dbQueryBuilder.ExecuteScalar()) > -1)
          return -1;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (LOCompAccessor), ex);
        throw new Exception("Cannot duplicated record in table CompPlans table Due to the following problem:\r\n" + ex.Message);
      }
      dbQueryBuilder.Reset();
      try
      {
        string text = "UPDATE CompPlans SET [name] = " + SQL.Encode((object) loanCompPlan.Name) + ",[description] = " + SQL.Encode((object) loanCompPlan.Description) + ",[type] = " + (object) (loanCompPlan.PlanType == LoanCompPlanType.LoanOfficer ? 1 : (loanCompPlan.PlanType == LoanCompPlanType.Broker ? 2 : (loanCompPlan.PlanType == LoanCompPlanType.Both ? 3 : 0))) + ",[status] = " + (loanCompPlan.Status ? (object) "'True'" : (object) "'False'") + ",[activationDate] = " + SQL.EncodeDateTime(loanCompPlan.ActivationDate, DateTime.MaxValue, true) + ",[minTermDays] = " + (object) loanCompPlan.MinTermDays + ",[percentAmt] = " + (object) loanCompPlan.PercentAmt + ",[percentAmtBase] = " + (object) loanCompPlan.PercentAmtBase + ",[roundingMethod] = " + (object) loanCompPlan.RoundingMethod + ",[dollarAmt] = " + (object) loanCompPlan.DollarAmount + ",[minDollarAmt] = " + (object) loanCompPlan.MinDollarAmount + ",[maxDollarAmt] = " + (object) loanCompPlan.MaxDollarAmount + ",[triggerFieldID] = " + SQL.Encode((object) loanCompPlan.TriggerField) + " WHERE id = " + (object) loanCompPlan.Id;
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (LOCompAccessor), ex);
        throw new Exception("Cannot update LO Comp record in CompPlans table.\r\n" + ex.Message);
      }
      return loanCompPlan.Id;
    }

    public static List<int> RemoveCompPlans(int[] IDplansToRemove)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<int> intList = new List<int>();
      try
      {
        foreach (int num in IDplansToRemove)
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("SELECT COUNT(oid) FROM [org_CompPlans] WHERE [compplanid] = " + (object) num);
          object obj1 = dbQueryBuilder.ExecuteScalar();
          if (obj1 == null || Utils.ParseInt(obj1) <= 0)
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("SELECT COUNT(oid) FROM [Ext_LenderCompPlans] WHERE [compplanid] = " + (object) num);
            object obj2 = dbQueryBuilder.ExecuteScalar();
            if (obj2 == null || Utils.ParseInt(obj2) <= 0)
            {
              dbQueryBuilder.Reset();
              dbQueryBuilder.AppendLine("SELECT COUNT(oid) FROM [Ext_CompPlans] WHERE [compplanid] = " + (object) num);
              object obj3 = dbQueryBuilder.ExecuteScalar();
              if (obj3 == null || Utils.ParseInt(obj3) <= 0)
              {
                dbQueryBuilder.Reset();
                dbQueryBuilder.AppendLine("SELECT COUNT(userid) FROM [users_CompPlans] WHERE [compplanid] = " + (object) num);
                object obj4 = dbQueryBuilder.ExecuteScalar();
                if (obj4 == null || Utils.ParseInt(obj4) <= 0)
                {
                  dbQueryBuilder.Reset();
                  dbQueryBuilder.AppendLine("DELETE FROM [CompPlans] WHERE id = " + (object) num);
                  dbQueryBuilder.ExecuteNonQuery();
                  intList.Add(num);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (LOCompAccessor), ex);
        throw new Exception("Cannot delete selected LO Comp record in CompPlans table.\r\n" + ex.Message);
      }
      return intList;
    }

    public static List<OrgInfo> GetOrganizationsUsingCompPlan(int loCompID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT DISTINCT org.oid, org.* FROM [org_chart] org");
      dbQueryBuilder.AppendLine("  INNER JOIN [org_CompPlans] orgPlan ON org.oid = orgPlan.oid");
      dbQueryBuilder.AppendLine("WHERE orgPlan.compplanid = " + (object) loCompID);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null)
        return (List<OrgInfo>) null;
      List<OrgInfo> organizationsUsingCompPlan = new List<OrgInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        OrgInfo basicOrgInfo = Organization.CreateBasicOrgInfo(row, (int[]) null);
        OrgInfo organizationForLoComp = OrganizationStore.GetFirstAvaliableOrganizationForLOComp(basicOrgInfo.Oid);
        if (organizationForLoComp != null)
          basicOrgInfo.CompanyAddress = organizationForLoComp.CompanyAddress;
        organizationsUsingCompPlan.Add(basicOrgInfo);
      }
      return organizationsUsingCompPlan;
    }

    public static List<object[]> GetUsersUsingCompPlan(int loCompID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT distinct u.userid FROM [users] u");
      dbQueryBuilder.AppendLine("  INNER JOIN [users_CompPlans] userPlan ON u.userid = userPlan.userid");
      dbQueryBuilder.AppendLine("WHERE userPlan.compplanid = " + (object) loCompID);
      UserInfo[] usersFromSql = User.GetUsersFromSQL(dbQueryBuilder.ToString());
      if (usersFromSql == null || usersFromSql.Length == 0)
        return (List<object[]>) null;
      Hashtable hashtable = new Hashtable();
      List<object[]> usersUsingCompPlan = new List<object[]>();
      for (int index = 0; index < usersFromSql.Length; ++index)
      {
        if (!hashtable.ContainsKey((object) usersFromSql[index].OrgId))
          hashtable.Add((object) usersFromSql[index].OrgId, (object) OrganizationStore.GetFirstAvaliableOrganizationForLOComp(usersFromSql[index].OrgId));
        usersUsingCompPlan.Add(new object[2]
        {
          (object) usersFromSql[index],
          (object) (OrgInfo) hashtable[(object) usersFromSql[index].OrgId]
        });
      }
      return usersUsingCompPlan;
    }

    [PgReady]
    internal static DataTable GetComPlanHistoryDataTable(
      bool forUser,
      bool forExteranl,
      bool forExternalLender,
      string[] ids)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        string str1 = "org_CompPlans";
        if (forExteranl)
          str1 = !forExternalLender ? "Ext_CompPlans" : "Ext_LenderCompPlans";
        else if (forUser)
          str1 = "users_CompPlans";
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT historyPlans.*, plans.name, plans.minTermDays, plans.percentAmt, plans.dollarAmt, plans.minDollarAmt, plans.maxDollarAmt, plans.percentAmtBase, plans.roundingMethod, plans.description, plans.status, plans.type, plans.triggerFieldID FROM [" + str1 + "] historyPlans");
        pgDbQueryBuilder.AppendLine("  INNER JOIN [CompPlans] plans ON historyPlans.compplanid = plans.id");
        if (ids != null && ids.Length != 0)
        {
          if (forUser)
            pgDbQueryBuilder.Append(" WHERE historyPlans.userid in (");
          else
            pgDbQueryBuilder.Append(" WHERE historyPlans.oid in (");
          string str2 = "";
          foreach (string id in ids)
          {
            if (!string.IsNullOrEmpty(str2))
              str2 += ",";
            str2 = !forUser ? str2 + SQL.Encode((object) Utils.ParseInt((object) id)) : str2 + SQL.Encode((object) id);
          }
          pgDbQueryBuilder.AppendLine(str2 + ")");
        }
        pgDbQueryBuilder.AppendLine("  ORDER BY historyPlans.startDate DESC");
        DataSet dataSet = pgDbQueryBuilder.ExecuteSetQuery();
        return dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 ? (DataTable) null : dataSet.Tables[0];
      }
      string str3 = "org_CompPlans";
      if (forExteranl)
        str3 = !forExternalLender ? "Ext_CompPlans" : "Ext_LenderCompPlans";
      else if (forUser)
        str3 = "users_CompPlans";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT historyPlans.*, plans.name, plans.minTermDays, plans.percentAmt, plans.dollarAmt, plans.minDollarAmt, plans.maxDollarAmt, plans.percentAmtBase, plans.roundingMethod, plans.description, plans.status, plans.type, plans.triggerFieldID FROM [" + str3 + "] historyPlans");
      dbQueryBuilder.AppendLine("  INNER JOIN [CompPlans] plans ON historyPlans.compplanid = plans.id");
      if (ids != null && ids.Length != 0)
      {
        if (forUser)
          dbQueryBuilder.Append(" WHERE historyPlans.userid in (");
        else
          dbQueryBuilder.Append(" WHERE historyPlans.oid in (");
        string str4 = "";
        foreach (string id in ids)
        {
          if (!string.IsNullOrEmpty(str4))
            str4 += ",";
          str4 = !forUser ? str4 + SQL.Encode((object) Utils.ParseInt((object) id)) : str4 + SQL.Encode((object) id);
        }
        dbQueryBuilder.AppendLine(str4 + ")");
      }
      dbQueryBuilder.AppendLine("  ORDER BY historyPlans.startDate DESC");
      DataSet dataSet1 = dbQueryBuilder.ExecuteSetQuery();
      return dataSet1 == null || dataSet1.Tables == null || dataSet1.Tables.Count == 0 ? (DataTable) null : dataSet1.Tables[0];
    }

    internal static DataTable GetComPlanHistoryDataTable(
      bool forUser,
      bool forExteranl,
      bool forExternalLender,
      string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        return LOCompAccessor.GetComPlanHistoryDataTable(forUser, forExteranl, forExternalLender, (string[]) null);
      return LOCompAccessor.GetComPlanHistoryDataTable((forUser ? 1 : 0) != 0, (forExteranl ? 1 : 0) != 0, (forExternalLender ? 1 : 0) != 0, new string[1]
      {
        id
      });
    }

    public static LoanCompHistoryList GetComPlanHistoryforOrg(int orgID, bool forExteranl)
    {
      return LOCompAccessor.GetComPlanHistoryforOrg(orgID, forExteranl, false);
    }

    public static LoanCompHistoryList GetComPlanHistoryforOrg(
      int orgID,
      bool forExteranl,
      bool forExternalLender)
    {
      DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(false, forExteranl, forExternalLender, orgID == -1 ? (string) null : orgID.ToString());
      if (historyDataTable == null)
        return (LoanCompHistoryList) null;
      LoanCompHistoryList planHistoryforOrg = new LoanCompHistoryList(orgID.ToString());
      foreach (DataRow row in (InternalDataCollectionBase) historyDataTable.Rows)
        planHistoryforOrg.AddHistory(LOCompAccessor.GetLoanCompHistoryFromDatarow(row, false));
      return planHistoryforOrg;
    }

    public static LoanCompHistoryList GetComPlanHistoryforUser(
      string userID,
      bool forExternal,
      bool forExternalLender)
    {
      DataTable historyDataTable = LOCompAccessor.GetComPlanHistoryDataTable(!forExternal, forExternal, forExternalLender, userID);
      if (historyDataTable == null)
        return (LoanCompHistoryList) null;
      LoanCompHistoryList planHistoryforUser = new LoanCompHistoryList(userID);
      foreach (DataRow row in (InternalDataCollectionBase) historyDataTable.Rows)
        planHistoryforUser.AddHistory(LOCompAccessor.GetLoanCompHistoryFromDatarow(row, !forExternal));
      return planHistoryforUser;
    }

    public static List<LoanCompHistoryList> GetComPlanHistoryforAllOriginators()
    {
      string empty = string.Empty;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<LoanCompHistoryList> historyforAllOriginators = new List<LoanCompHistoryList>();
      for (int index = 1; index <= 4; ++index)
      {
        dbQueryBuilder.Reset();
        switch (index)
        {
          case 1:
            string str1 = "Ext_CompPlans";
            dbQueryBuilder.AppendLine("SELECT historyPlans.*, extOrg.CompanyLegalName, plans.name, plans.minTermDays, plans.percentAmt, plans.dollarAmt, plans.minDollarAmt, plans.maxDollarAmt, plans.percentAmtBase, plans.roundingMethod, plans.description, plans.status, plans.type, plans.triggerFieldID FROM [" + str1 + "] historyPlans");
            dbQueryBuilder.AppendLine("  LEFT JOIN [CompPlans] plans ON historyPlans.compplanid = plans.id");
            dbQueryBuilder.AppendLine("  LEFT JOIN [ExternalOriginatorManagement] extOrg ON historyPlans.oid = extOrg.oid");
            dbQueryBuilder.AppendLine("  ORDER BY extOrg.CompanyLegalName ASC, historyPlans.startDate DESC");
            break;
          case 2:
            string str2 = "Ext_LenderCompPlans";
            dbQueryBuilder.AppendLine("SELECT historyPlans.*, extLender.CompanyLegalName, plans.name, plans.minTermDays, plans.percentAmt, plans.dollarAmt, plans.minDollarAmt, plans.maxDollarAmt, plans.percentAmtBase, plans.roundingMethod, plans.description, plans.status, plans.type, plans.triggerFieldID FROM [" + str2 + "] historyPlans");
            dbQueryBuilder.AppendLine("  LEFT JOIN [CompPlans] plans ON historyPlans.compplanid = plans.id");
            dbQueryBuilder.AppendLine("  LEFT JOIN [ExternalLenders] extLender ON historyPlans.oid = extLender.oid");
            dbQueryBuilder.AppendLine("  ORDER BY extLender.CompanyLegalName ASC, historyPlans.startDate DESC");
            break;
          case 3:
            string str3 = "org_CompPlans";
            dbQueryBuilder.AppendLine("SELECT historyPlans.*, orgs.org_name, plans.name, plans.minTermDays, plans.percentAmt, plans.dollarAmt, plans.minDollarAmt, plans.maxDollarAmt, plans.percentAmtBase, plans.roundingMethod, plans.description, plans.status, plans.type, plans.triggerFieldID FROM [" + str3 + "] historyPlans");
            dbQueryBuilder.AppendLine("  LEFT JOIN [CompPlans] plans ON historyPlans.compplanid = plans.id");
            dbQueryBuilder.AppendLine("  LEFT JOIN [org_chart] orgs ON historyPlans.oid = orgs.oid");
            dbQueryBuilder.AppendLine("  ORDER BY orgs.org_name ASC, historyPlans.startDate DESC");
            break;
          default:
            string str4 = "users_CompPlans";
            dbQueryBuilder.AppendLine("SELECT historyPlans.*, users.FirstLastName, plans.name, plans.minTermDays, plans.percentAmt, plans.dollarAmt, plans.minDollarAmt, plans.maxDollarAmt, plans.percentAmtBase, plans.roundingMethod, plans.description, plans.status, plans.type, plans.triggerFieldID FROM [" + str4 + "] historyPlans");
            dbQueryBuilder.AppendLine("  LEFT JOIN [CompPlans] plans ON historyPlans.compplanid = plans.id");
            dbQueryBuilder.AppendLine("  LEFT JOIN [users] users ON historyPlans.userid = users.userid");
            dbQueryBuilder.AppendLine("  ORDER BY users.FirstLastName ASC, historyPlans.startDate DESC");
            break;
        }
        DataSet dataSet = (DataSet) null;
        try
        {
          dataSet = dbQueryBuilder.ExecuteSetQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LOCompAccessor), "LOCompAccessor.GetCompPlanHistoryforAllOriginators: Cannot get LO Comp History, Error: " + ex.Message);
        }
        if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count != 0)
        {
          string str5 = (string) null;
          LoanCompHistoryList loanCompHistoryList = (LoanCompHistoryList) null;
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
          {
            LoanCompHistory historyFromDatarow = LOCompAccessor.GetLoanCompHistoryFromDatarow(row, index == 4);
            if (str5 != historyFromDatarow.Id)
            {
              if (loanCompHistoryList != null)
                historyforAllOriginators.Add(loanCompHistoryList);
              loanCompHistoryList = new LoanCompHistoryList(historyFromDatarow.Id);
              switch (index)
              {
                case 3:
                  loanCompHistoryList.UserName = (string) SQL.Decode(row["org_name"], (object) "");
                  break;
                case 4:
                  loanCompHistoryList.UserName = (string) SQL.Decode(row["FirstLastName"], (object) "");
                  break;
                default:
                  loanCompHistoryList.UserName = (string) SQL.Decode(row["CompanyLegalName"], (object) "");
                  break;
              }
              str5 = historyFromDatarow.Id;
            }
            loanCompHistoryList.AddHistory(historyFromDatarow);
          }
          historyforAllOriginators.Add(loanCompHistoryList);
        }
      }
      return historyforAllOriginators;
    }

    public static LoanCompHistory GetLoanCompHistoryFromDatarow(DataRow r, bool forUser)
    {
      string empty = string.Empty;
      string id = forUser ? (string) r["userid"] : ((int) r["oid"]).ToString();
      string planName = (string) SQL.Decode(r["name"], (object) "");
      int compPlanId = (int) SQL.Decode(r["compplanid"], (object) -1);
      object obj1 = r["startDate"];
      DateTime dateTime = DateTime.MinValue;
      string defaultValue1 = dateTime.ToString("MM/dd/yyyy");
      DateTime date1 = Utils.ParseDate(SQL.Decode(obj1, (object) defaultValue1));
      object obj2 = r["endDate"];
      dateTime = DateTime.MaxValue;
      string defaultValue2 = dateTime.ToString("MM/dd/yyyy");
      DateTime date2 = Utils.ParseDate(SQL.Decode(obj2, (object) defaultValue2));
      return new LoanCompHistory(id, planName, compPlanId, date1, date2)
      {
        MinTermDays = (int) SQL.Decode(r["minTermDays"], (object) -1),
        PercentAmt = (Decimal) SQL.Decode(r["percentAmt"], (object) -1),
        DollarAmount = (Decimal) SQL.Decode(r["dollarAmt"], (object) -1),
        MinDollarAmount = (Decimal) SQL.Decode(r["minDollarAmt"], (object) -1),
        MaxDollarAmount = (Decimal) SQL.Decode(r["maxDollarAmt"], (object) -1),
        PercentAmtBase = (int) SQL.Decode(r["percentAmtBase"], (object) -1),
        RoundingMethod = (int) SQL.Decode(r["roundingMethod"], (object) -1),
        Description = (string) SQL.Decode(r["description"]),
        Status = (string) SQL.Decode((object) r["status"].ToString()),
        Type = (string) SQL.Decode((object) r["type"].ToString()),
        TriggerFieldID = (string) SQL.Decode(r["triggerFieldID"])
      };
    }

    public static LoanCompPlan GetCurrentComPlanforUser(string userID, DateTime triggerDateTime)
    {
      LoanCompHistoryList planHistoryforUser = LOCompAccessor.GetComPlanHistoryforUser(userID, false, false);
      if (planHistoryforUser == null)
        return (LoanCompPlan) null;
      LoanCompHistory currentPlan = planHistoryforUser.GetCurrentPlan(triggerDateTime.Date);
      return currentPlan == null ? (LoanCompPlan) null : LOCompAccessor.GetLoanCompPlanByID(currentPlan.CompPlanId);
    }

    internal static void CreateHistoryCompPlansForOrg(
      LoanCompHistoryList loanCompHistoryList,
      int orgId,
      bool forExternal,
      bool forExternalLender)
    {
      LOCompAccessor.CreateHistoryCompPlans(loanCompHistoryList, orgId, (string) null, forExternal, forExternalLender);
    }

    public static void CreateHistoryCompPlansForUser(
      LoanCompHistoryList loanCompHistoryList,
      string userid)
    {
      LOCompAccessor.CreateHistoryCompPlans(loanCompHistoryList, -1, userid, false, false);
    }

    [PgReady]
    public static void CreateHistoryCompPlans(
      LoanCompHistoryList loanCompHistoryList,
      int orgId,
      string userid,
      bool forExternal,
      bool forExternalLender)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string str1 = "org_CompPlans";
        if (forExternal)
          str1 = !forExternalLender ? "Ext_CompPlans" : "Ext_LenderCompPlans";
        else if (userid != null)
          str1 = "users_CompPlans";
        string str2 = userid != null ? "[userid]" : "[oid]";
        PgDbQueryBuilder pgDbQueryBuilder1 = new PgDbQueryBuilder();
        pgDbQueryBuilder1.AppendLine("DELETE FROM [" + str1 + "] WHERE " + str2 + " = " + (userid != null ? SQL.Encode((object) userid) : string.Concat((object) orgId)) + " AND [startDate]> " + SQL.EncodeDateTime(DateTime.Today, DateTime.MaxValue) + ";");
        try
        {
          pgDbQueryBuilder1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LOCompAccessor), "Cannot delete Loan Compensation History Information from the table \"" + str1 + "\" due to this error: " + ex.Message);
          return;
        }
        if (loanCompHistoryList == null || loanCompHistoryList.Count == 0)
          return;
        loanCompHistoryList.SortPlans(true);
        string str3 = string.Empty;
        DataRowCollection dataRowCollection = (DataRowCollection) null;
        for (int i = 0; i < loanCompHistoryList.Count; ++i)
        {
          str3 = "";
          LoanCompHistory historyAt = loanCompHistoryList.GetHistoryAt(i);
          if (!(historyAt.EndDate < DateTime.Today))
          {
            PgDbQueryBuilder pgDbQueryBuilder2 = new PgDbQueryBuilder();
            pgDbQueryBuilder2.Append("select 1 from " + str1 + " WHERE [startDate]= " + SQL.EncodeDateTime(historyAt.StartDate, DateTime.MaxValue));
            try
            {
              dataRowCollection = pgDbQueryBuilder2.Execute();
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(nameof (LOCompAccessor), "Cannot get Loan Compensation History Information due to this error: " + ex.Message);
            }
            string text;
            if (historyAt.StartDate <= DateTime.Today && dataRowCollection.Count != 0)
              text = "UPDATE " + str1 + " SET [endDate] = " + SQL.EncodeDateTime(historyAt.EndDate, DateTime.MaxValue) + " WHERE [startDate]=" + SQL.EncodeDateTime(historyAt.StartDate, DateTime.MaxValue) + " and " + str2 + " = " + (userid != null ? SQL.Encode((object) userid) : string.Concat((object) orgId));
            else
              text = "INSERT INTO " + str1 + " (" + str2 + ", compplanid, startDate, endDate) VALUES (" + (userid != null ? (object) SQL.Encode((object) userid) : (object) string.Concat((object) orgId)) + ", " + (object) historyAt.CompPlanId + ", " + SQL.EncodeDateTime(historyAt.StartDate, DateTime.MinValue) + ", " + SQL.EncodeDateTime(historyAt.EndDate, DateTime.MaxValue) + ") ";
            pgDbQueryBuilder1.AppendLine(text);
            pgDbQueryBuilder1.Append(";");
          }
        }
        try
        {
          pgDbQueryBuilder1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LOCompAccessor), "Cannot insert Loan Compensation History Information back to table \"" + str1 + "\" due to this error: " + ex.Message);
        }
      }
      else
      {
        string tableName = "org_CompPlans";
        if (forExternal)
          tableName = !forExternalLender ? "Ext_CompPlans" : "Ext_LenderCompPlans";
        else if (userid != null)
          tableName = "users_CompPlans";
        string idName = userid != null ? "[userid]" : "[oid]";
        DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
        dbQueryBuilder1.AppendLine("DELETE FROM [" + tableName + "] WHERE " + idName + " = " + (userid != null ? SQL.Encode((object) userid) : string.Concat((object) orgId)) + " AND [startDate]> " + SQL.EncodeDateTime(DateTime.Today, DateTime.MaxValue) ?? "");
        try
        {
          dbQueryBuilder1.ExecuteNonQuery();
          dbQueryBuilder1.Reset();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LOCompAccessor), "Cannot delete Loan Compensation History Information from the table \"" + tableName + "\" due to this error: " + ex.Message);
          return;
        }
        if (loanCompHistoryList == null || loanCompHistoryList.Count == 0)
          return;
        loanCompHistoryList.SortPlans(true);
        string str = string.Empty;
        DataRowCollection dataRowCollection = (DataRowCollection) null;
        dbQueryBuilder1.Declare("@oid", "int");
        dbQueryBuilder1.SelectVar("@oid", (object) orgId);
        for (int i = 0; i < loanCompHistoryList.Count; ++i)
        {
          str = "";
          LoanCompHistory historyAt = loanCompHistoryList.GetHistoryAt(i);
          if (!(historyAt.EndDate < DateTime.Today))
          {
            DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
            dbQueryBuilder2.Append("select 1 from " + tableName + " WHERE [startDate]= " + SQL.EncodeDateTime(historyAt.StartDate, DateTime.MaxValue) + " and " + idName + " = " + (userid != null ? SQL.Encode((object) userid) : string.Concat((object) orgId)));
            try
            {
              dataRowCollection = dbQueryBuilder2.Execute();
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(nameof (LOCompAccessor), "Cannot get Loan Compensation History Information due to this error: " + ex.Message);
            }
            string text;
            if (historyAt.StartDate <= DateTime.Today && dataRowCollection.Count != 0)
              text = "UPDATE " + tableName + " SET [endDate] = " + SQL.EncodeDateTime(historyAt.EndDate, DateTime.MaxValue) + " WHERE [startDate]=" + SQL.EncodeDateTime(historyAt.StartDate, DateTime.MaxValue) + " and " + idName + " = " + (userid != null ? SQL.Encode((object) userid) : string.Concat((object) orgId));
            else
              text = LOCompAccessor.InsertLoCompHistoryQuery(tableName, idName, userid, historyAt);
            dbQueryBuilder1.AppendLine(text);
          }
        }
        try
        {
          dbQueryBuilder1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LOCompAccessor), "Cannot insert Loan Compensation History Information back to table \"" + tableName + "\" due to this error: " + ex.Message);
        }
      }
    }

    public static string InsertLoCompHistoryQuery(
      string tableName,
      string idName,
      string userId,
      LoanCompHistory plan)
    {
      return "INSERT INTO " + tableName + " (" + idName + ", compplanid, startDate, endDate) VALUES (" + (userId != null ? (object) SQL.Encode((object) userId) : (object) "@oid") + ", " + (object) plan.CompPlanId + ", " + SQL.EncodeDateTime(plan.StartDate, DateTime.MinValue) + ", " + SQL.EncodeDateTime(plan.EndDate, DateTime.MaxValue) + ") ";
    }
  }
}
