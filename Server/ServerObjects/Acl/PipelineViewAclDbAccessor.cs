// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.PipelineViewAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public static class PipelineViewAclDbAccessor
  {
    private const string className = "PipelineViewAclDbAccessor�";

    public static PersonaPipelineView GetPipelineView(int viewID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select pv.*, p.personaName from Acl_PipelineViews pv");
      dbQueryBuilder.AppendLine("  inner join Personas p on pv.personaID = p.personaID");
      dbQueryBuilder.AppendLine("where pv.viewID = " + (object) viewID);
      dbQueryBuilder.AppendLine("select * from Acl_PipelineView_Columns pvc");
      dbQueryBuilder.AppendLine("where pvc.viewID = " + (object) viewID);
      dbQueryBuilder.AppendLine("order by pvc.orderIndex");
      PersonaPipelineView[] personaPiplineViews = PipelineViewAclDbAccessor.dataSetToPersonaPiplineViews(dbQueryBuilder.ExecuteSetQuery());
      return personaPiplineViews.Length == 0 ? (PersonaPipelineView) null : personaPiplineViews[0];
    }

    public static UserPipelineView GetUserPipelineView(int viewID)
    {
      ClientContext.GetCurrent();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select uv.* from Acl_UserPipelineViews uv");
      dbQueryBuilder.AppendLine("  inner join users u on uv.userId = u.userId");
      dbQueryBuilder.AppendLine("where uv.viewID = " + SQL.Encode((object) viewID));
      dbQueryBuilder.AppendLine("select puv.* from Acl_UserPipelineView_Columns puv");
      dbQueryBuilder.AppendLine("  inner join Acl_UserPipelineViews uv on uv.viewID = puv.viewID");
      dbQueryBuilder.AppendLine("where puv.viewID = " + SQL.Encode((object) viewID));
      dbQueryBuilder.AppendLine("order by puv.sortOrder");
      UserPipelineView[] userPiplineViews = PipelineViewAclDbAccessor.dataSetToUserPiplineViews(dbQueryBuilder.ExecuteSetQuery());
      return userPiplineViews.Length == 0 ? (UserPipelineView) null : userPiplineViews[0];
    }

    public static UserPipelineView GetUserPipelineView(string userID, int viewID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select uv.* from Acl_UserPipelineViews uv");
      dbQueryBuilder.AppendLine("  inner join users u on uv.userId = u.userId");
      dbQueryBuilder.AppendLine("where uv.viewID = " + SQL.Encode((object) viewID));
      dbQueryBuilder.AppendLine("And uv.userId = " + SQL.Encode((object) userID));
      dbQueryBuilder.AppendLine("select puv.* from Acl_UserPipelineView_Columns puv");
      dbQueryBuilder.AppendLine("  inner join Acl_UserPipelineViews uv on uv.viewID = puv.viewID");
      dbQueryBuilder.AppendLine("where puv.viewID = " + SQL.Encode((object) viewID));
      dbQueryBuilder.AppendLine("order by puv.sortOrder");
      UserPipelineView[] userPiplineViews = PipelineViewAclDbAccessor.dataSetToUserPiplineViews(dbQueryBuilder.ExecuteSetQuery());
      return userPiplineViews.Length == 0 ? (UserPipelineView) null : userPiplineViews[0];
    }

    public static PersonaPipelineView GetPersonaPipelineView(int personaID, string viewName)
    {
      PersonaPipelineView[] personaPiplineViews = PipelineViewAclDbAccessor.dataSetToPersonaPiplineViews(PipelineViewAclDbAccessor.GetPersonaPipelineViewDbQuery(personaID, viewName).ExecuteSetQuery());
      return personaPiplineViews.Length == 0 ? (PersonaPipelineView) null : personaPiplineViews[0];
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder GetPersonaPipelineViewDbQuery(
      int personaID,
      string viewName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder pipelineViewDbQuery = new EllieMae.EMLite.Server.DbQueryBuilder();
      pipelineViewDbQuery.AppendLine("select pv.*, p.personaName from Acl_PipelineViews pv");
      pipelineViewDbQuery.AppendLine("  inner join Personas p on pv.personaID = p.personaID");
      pipelineViewDbQuery.AppendLine("where pv.personaID = " + (object) personaID);
      pipelineViewDbQuery.AppendLine("  and pv.name = " + SQL.Encode((object) viewName));
      pipelineViewDbQuery.AppendLine("select pvc.* from Acl_PipelineView_Columns pvc");
      pipelineViewDbQuery.AppendLine("  inner join Acl_PipelineViews pv on pv.viewID = pvc.viewID");
      pipelineViewDbQuery.AppendLine("where pv.personaID = " + (object) personaID);
      pipelineViewDbQuery.AppendLine("  and pv.name = " + SQL.Encode((object) viewName));
      pipelineViewDbQuery.AppendLine("order by pvc.orderIndex");
      return pipelineViewDbQuery;
    }

    public static UserPipelineView GetUserPipelineView(string userID, string viewName)
    {
      if (!PipelineViewAclDbAccessor.IsDbSaveEnabled())
        return PipelineViewAclDbAccessor.GetTemplateFromViewName(userID, viewName);
      UserPipelineView[] userPiplineViews = PipelineViewAclDbAccessor.dataSetToUserPiplineViews(PipelineViewAclDbAccessor.GetUserPipelineViewDbQuery(userID, viewName).ExecuteSetQuery());
      return userPiplineViews.Length == 0 ? (UserPipelineView) null : userPiplineViews[0];
    }

    public static PersonaPipelineView[] GetPersonaPipelineViews(int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select pv.*, p.personaName from Acl_PipelineViews pv");
      dbQueryBuilder.AppendLine("  inner join Personas p on pv.personaID = p.personaID");
      dbQueryBuilder.AppendLine("where pv.personaID = " + (object) personaID);
      dbQueryBuilder.AppendLine("order by pv.orderIndex");
      dbQueryBuilder.AppendLine("select pvc.* from Acl_PipelineView_Columns pvc");
      dbQueryBuilder.AppendLine("  inner join Acl_PipelineViews pv on pv.viewID = pvc.viewID");
      dbQueryBuilder.AppendLine("where pv.personaID = " + (object) personaID);
      dbQueryBuilder.AppendLine("order by pvc.orderIndex");
      return PipelineViewAclDbAccessor.dataSetToPersonaPiplineViews(dbQueryBuilder.ExecuteSetQuery());
    }

    public static PersonaPipelineView[] GetPersonaPipelineViews(List<int> personaIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select pv.*, p.personaName from Acl_PipelineViews pv");
      dbQueryBuilder.AppendLine("  inner join Personas p on pv.personaID = p.personaID");
      if (personaIds != null && personaIds.Any<int>())
        dbQueryBuilder.AppendLine("where pv.personaID in (" + string.Join(",", personaIds.Select<int, string>((System.Func<int, string>) (n => n.ToString())).ToArray<string>()) + ")");
      dbQueryBuilder.AppendLine("order by pv.orderIndex");
      dbQueryBuilder.AppendLine("select pvc.* from Acl_PipelineView_Columns pvc");
      dbQueryBuilder.AppendLine("  inner join Acl_PipelineViews pv on pv.viewID = pvc.viewID");
      if (personaIds != null && personaIds.Any<int>())
        dbQueryBuilder.AppendLine("where pv.personaID in (" + string.Join(",", personaIds.Select<int, string>((System.Func<int, string>) (n => n.ToString())).ToArray<string>()) + ")");
      dbQueryBuilder.AppendLine("order by pvc.orderIndex");
      return PipelineViewAclDbAccessor.dataSetToPersonaPiplineViews(dbQueryBuilder.ExecuteSetQuery());
    }

    [PgReady]
    public static PersonaPipelineView[] GetUserPipelineViews(string userId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select pv.*, p.personaName from Acl_PipelineViews pv");
        pgDbQueryBuilder.AppendLine("  inner join Personas p on pv.personaID = p.personaID");
        pgDbQueryBuilder.AppendLine("  inner join UserPersona up on pv.personaID = up.personaID");
        pgDbQueryBuilder.AppendLine("where up.userid = @userid");
        pgDbQueryBuilder.AppendLine("order by p.personaName, pv.orderIndex;");
        pgDbQueryBuilder.AppendLine("select pvc.* from Acl_PipelineView_Columns pvc");
        pgDbQueryBuilder.AppendLine("  inner join Acl_PipelineViews pv on pv.viewID = pvc.viewID");
        pgDbQueryBuilder.AppendLine("  inner join UserPersona up on pv.personaID = up.personaID");
        pgDbQueryBuilder.AppendLine("where up.userid = @userid");
        pgDbQueryBuilder.AppendLine("order by pvc.orderIndex;");
        return PipelineViewAclDbAccessor.dataSetToPersonaPiplineViews(pgDbQueryBuilder.ExecuteSetQuery(DbTransactionType.Default, new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.AnsiString).ToArray()));
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select pv.*, p.personaName from Acl_PipelineViews pv");
      dbQueryBuilder.AppendLine("  inner join Personas p on pv.personaID = p.personaID");
      dbQueryBuilder.AppendLine("  inner join UserPersona up on pv.personaID = up.personaID");
      dbQueryBuilder.AppendLine("where up.userid = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("order by p.personaName, pv.orderIndex");
      dbQueryBuilder.AppendLine("select pvc.* from Acl_PipelineView_Columns pvc");
      dbQueryBuilder.AppendLine("  inner join Acl_PipelineViews pv on pv.viewID = pvc.viewID");
      dbQueryBuilder.AppendLine("  inner join UserPersona up on pv.personaID = up.personaID");
      dbQueryBuilder.AppendLine("where up.userid = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("order by pvc.orderIndex");
      return PipelineViewAclDbAccessor.dataSetToPersonaPiplineViews(dbQueryBuilder.ExecuteSetQuery());
    }

    public static bool DeleteUserCustomPipelineView(string userId, string viewName)
    {
      if (PipelineViewAclDbAccessor.IsDbSaveEnabled())
      {
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineViews");
          dbQueryBuilder.DeleteFrom(table, new DbValueList()
          {
            {
              nameof (userId),
              (object) userId
            },
            {
              "name",
              (object) viewName
            }
          });
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "User Pipeline View can not be deleted. Error: " + ex.Message);
          Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
        }
      }
      else
        PipelineViewAclDbAccessor.DeletePipelineUserViewsFromFileSystem(userId, (IEnumerable<string>) new List<string>()
        {
          viewName
        });
      PipelineViewAclDbAccessor.updateCacheUserPipelineCache(userId);
      return true;
    }

    private static PersonaPipelineView[] dataSetToPersonaPiplineViews(DataSet viewColumnData)
    {
      List<PersonaPipelineView> personaPipelineViewList = new List<PersonaPipelineView>();
      foreach (DataRow row in (InternalDataCollectionBase) viewColumnData.Tables[0].Rows)
        personaPipelineViewList.Add(PipelineViewAclDbAccessor.dataRowToPersonaPiplineView(row, viewColumnData.Tables[1]));
      return personaPipelineViewList.ToArray();
    }

    private static PersonaPipelineView dataRowToPersonaPiplineView(
      DataRow viewRow,
      DataTable columnTable)
    {
      int viewID = (int) viewRow["viewID"];
      List<PersonaPipelineViewColumn> pipelineViewColumnList = new List<PersonaPipelineViewColumn>();
      foreach (DataRow r in columnTable.Select("viewID = " + (object) viewID))
        pipelineViewColumnList.Add(PipelineViewAclDbAccessor.dataRowToPersonaPiplineViewColumn(r));
      return new PersonaPipelineView(viewID, (int) viewRow["personaID"], (string) viewRow["personaName"], (string) viewRow["name"], SQL.DecodeInt(viewRow["orderIndex"]), (string) SQL.Decode(viewRow["loanFolders"], (object) null), SQL.DecodeEnum<PipelineViewLoanOwnership>(viewRow["ownership"], PipelineViewLoanOwnership.All), pipelineViewColumnList.ToArray(), (string) SQL.Decode(viewRow["filterXml"], (object) null));
    }

    private static PersonaPipelineViewColumn dataRowToPersonaPiplineViewColumn(DataRow r)
    {
      return new PersonaPipelineViewColumn((string) r["fieldDBName"], (int) r["orderIndex"], SQL.DecodeEnum<SortOrder>(r["sortby"]), SQL.DecodeInt(r["width"], -1));
    }

    public static PersonaPipelineView CreatePipelineView(PersonaPipelineView view, string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineViews");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineView_Columns");
      dbQueryBuilder.AppendLine("select max(orderIndex) from Acl_PipelineViews where personaID = " + (object) view.PersonaID);
      int num = SQL.DecodeInt(dbQueryBuilder.ExecuteScalar(), -1);
      DbValueList dbValueList1 = PipelineViewAclDbAccessor.createDbValueList(view, num + 1, auditUser, true);
      dbQueryBuilder.Reset();
      dbQueryBuilder.Declare("@viewID", "int");
      dbQueryBuilder.InsertInto(table1, dbValueList1, true, false);
      dbQueryBuilder.SelectIdentity("@viewID");
      for (int index = 0; index < view.Columns.Count; ++index)
      {
        DbValueList dbValueList2 = PipelineViewAclDbAccessor.createDbValueList(view.Columns[index]);
        dbValueList2.Add("viewID", (object) "@viewID", (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
      }
      dbQueryBuilder.Select("@viewID");
      return PipelineViewAclDbAccessor.GetPipelineView((int) dbQueryBuilder.ExecuteScalar());
    }

    public static UserPipelineView CreatePipelineUserView(
      UserPipelineView view,
      string userId,
      string auditUser)
    {
      view.Name = view.Name?.Trim();
      UserPipelineView pipelineUserView;
      if (PipelineViewAclDbAccessor.IsDbSaveEnabled())
      {
        pipelineUserView = PipelineViewAclDbAccessor.GetUserPipelineView(PipelineViewAclDbAccessor.CreatePipelineUserViewInDatabase(view, auditUser));
      }
      else
      {
        string viewInFileSystem = PipelineViewAclDbAccessor.CreatePipelineUserViewInFileSystem(view, userId);
        pipelineUserView = PipelineViewAclDbAccessor.GetTemplateFromViewName(userId, viewInFileSystem);
      }
      PipelineViewAclDbAccessor.updateCacheUserPipelineCache(userId);
      return pipelineUserView;
    }

    public static void UpdatePipelineView(PersonaPipelineView view, string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineViews");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineView_Columns");
      DbValueList dbValueList1 = PipelineViewAclDbAccessor.createDbValueList(view, -1, auditUser, false);
      DbValue key = new DbValue("viewID", (object) view.ViewID);
      dbQueryBuilder.Update(table1, dbValueList1, key);
      dbQueryBuilder.DeleteFrom(table2, key);
      for (int index = 0; index < view.Columns.Count; ++index)
      {
        DbValueList dbValueList2 = PipelineViewAclDbAccessor.createDbValueList(view.Columns[index]);
        dbValueList2.Add("viewID", (object) view.ViewID);
        dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool UpdatePipelineUserViewInDatabase(UserPipelineView view, string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineViews");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineView_Columns");
      try
      {
        DbValueList dbValueList1 = PipelineViewAclDbAccessor.createDbValueList(view, auditUser, false);
        DbValue key = new DbValue("viewID", (object) view.ViewID);
        dbQueryBuilder.Update(table1, dbValueList1, key);
        dbQueryBuilder.DeleteFrom(table2, key);
        for (int index = 0; index < view.Columns.Count; ++index)
        {
          DbValueList dbValueList2 = PipelineViewAclDbAccessor.createDbValueList(view.Columns[index]);
          dbValueList2.Add("viewID", (object) view.ViewID);
          dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "UpdatePipelineUserView cannot be updated. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return true;
    }

    public static bool UpdateUserPipelineViewName(
      UserPipelineView view,
      string oldViewName,
      string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineViews");
      try
      {
        DbValueList valueListForName = PipelineViewAclDbAccessor.createDbValueListForName(view, auditUser);
        dbQueryBuilder.Update(table, valueListForName, new DbValueList()
        {
          {
            "name",
            (object) oldViewName
          },
          {
            "userid",
            (object) auditUser
          }
        });
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "Pipeline View can not be updated. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return true;
    }

    public static void DeletePipelineView(int viewID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineViews");
      DbValue key = new DbValue(nameof (viewID), (object) viewID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool DeleteUserCustomPipelineView(string userId, int viewID)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineViews");
        DbValueList keys = new DbValueList()
        {
          new DbValue(nameof (viewID), (object) viewID),
          new DbValue("userid", (object) userId)
        };
        dbQueryBuilder.DeleteFrom(table, keys);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "Pipeline View can not be deleted. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return true;
    }

    public static void ReplacePersonaPipelineViews(
      int personaID,
      PersonaPipelineView[] views,
      string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineViews");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_PipelineView_Columns");
      DbValue key = new DbValue(nameof (personaID), (object) personaID);
      dbQueryBuilder.DeleteFrom(table1, key);
      dbQueryBuilder.Declare("@viewID", "int");
      for (int orderIndex = 0; orderIndex < views.Length; ++orderIndex)
      {
        PersonaPipelineView view = views[orderIndex];
        DbValueList dbValueList1 = PipelineViewAclDbAccessor.createDbValueList(view, orderIndex, auditUser, true);
        dbQueryBuilder.InsertInto(table1, dbValueList1, true, false);
        dbQueryBuilder.SelectIdentity("@viewID");
        for (int index = 0; index < view.Columns.Count; ++index)
        {
          DbValueList dbValueList2 = PipelineViewAclDbAccessor.createDbValueList(view.Columns[index]);
          dbValueList2.Add("viewID", (object) "@viewID", (IDbEncoder) DbEncoding.None);
          dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
        }
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void updateCacheUserPipelineCache(string userId)
    {
      ClientContext.GetCurrent().Cache.Remove(string.Format("{0}_{1}", (object) "UserCustomPipeline", (object) userId));
      PipelineViewAclDbAccessor.GetUserCustomPipelineViews(userId);
    }

    public static bool ReplaceUserPipelineViews(
      string userID,
      UserPipelineView[] views,
      string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineViews");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineView_Columns");
      try
      {
        dbQueryBuilder.Declare("@viewID", "int");
        for (int index1 = 0; index1 < views.Length; ++index1)
        {
          UserPipelineView view = views[index1];
          DbValueList keys = new DbValueList();
          keys.Add(new DbValue("viewID", (object) view.ViewID));
          dbQueryBuilder.DeleteFrom(table2, keys);
          keys.Clear();
          keys.Add(new DbValue(nameof (userID), (object) userID));
          keys.Add(new DbValue("name", (object) view.Name));
          dbQueryBuilder.DeleteFrom(table1, keys);
          view.Name = view.Name?.Trim();
          DbValueList dbValueList1 = PipelineViewAclDbAccessor.createDbValueList(view, auditUser, true);
          dbQueryBuilder.InsertInto(table1, dbValueList1, true, false);
          dbQueryBuilder.SelectIdentity("@viewID");
          for (int index2 = 0; index2 < view.Columns.Count; ++index2)
          {
            DbValueList dbValueList2 = PipelineViewAclDbAccessor.createDbValueList(view.Columns[index2], false);
            dbValueList2.Add("viewID", (object) "@viewID", (IDbEncoder) DbEncoding.None);
            dbValueList2.Add("IsRequired", (object) (view.Columns[index2].IsRequired ? 1 : 0), (IDbEncoder) DbEncoding.None);
            dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
          }
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "Pipeline View can not be Updated. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return true;
    }

    public static void DuplicatePipelineViews(
      int sourcePersonaID,
      int targetPersonaID,
      string auditUser)
    {
      foreach (PersonaPipelineView personaPipelineView in PipelineViewAclDbAccessor.GetPersonaPipelineViews(sourcePersonaID))
        PipelineViewAclDbAccessor.CreatePipelineView(personaPipelineView.Duplicate(targetPersonaID), auditUser);
    }

    public static bool DuplicateUserPipelineViews(
      string sourceUserID,
      string targetUserID,
      string auditUser)
    {
      try
      {
        foreach (UserPipelineView customPipelineView in PipelineViewAclDbAccessor.GetUserCustomPipelineViews(sourceUserID))
          PipelineViewAclDbAccessor.CreatePipelineUserView(customPipelineView.Duplicate(targetUserID), targetUserID, auditUser);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "Pipeline View can not be Duplicated. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return true;
    }

    public static bool UserPipelineViewWithNameExists(string userID, string viewName)
    {
      return PipelineViewAclDbAccessor.IsDbSaveEnabled() ? PipelineViewAclDbAccessor.GetUserPipelineViewDbQuery(userID, viewName).ExecuteSetQuery().Tables[0].Rows.Count != 0 : TemplateSettingsStore.Exists(TemplateSettingsType.PipelineView, FileSystemEntry.PrivateRoot(userID).Combine(viewName));
    }

    public static UserPipelineView UpdatePipelineUserView(
      UserPipelineView view,
      string userId,
      string oldViewName,
      string auditUser)
    {
      view.Name = view.Name?.Trim();
      UserPipelineView userPipelineView;
      if (PipelineViewAclDbAccessor.IsDbSaveEnabled())
      {
        PipelineViewAclDbAccessor.UpdatePipelineUserViewInDatabase(view, auditUser);
        userPipelineView = PipelineViewAclDbAccessor.GetUserPipelineView(view.ViewID);
      }
      else
      {
        PipelineViewAclDbAccessor.UpdatePipelineUserViewInFileSystem(view, userId, oldViewName);
        userPipelineView = PipelineViewAclDbAccessor.GetTemplateFromViewName(userId, view.Name);
      }
      PipelineViewAclDbAccessor.updateCacheUserPipelineCache(userId);
      return userPipelineView;
    }

    public static bool DeletePipelineUserViews(string userId, IEnumerable<string> viewIds)
    {
      try
      {
        if (PipelineViewAclDbAccessor.IsDbSaveEnabled())
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.Append("delete from Acl_UserPipelineViews where [viewID] ");
          dbQueryBuilder.Append("in (" + string.Join(", ", viewIds.Select<string, string>((System.Func<string, string>) (x => SQL.Encode((object) x)))) + ") AND userId = " + SQL.Encode((object) userId));
          dbQueryBuilder.ExecuteTableQuery();
        }
        else
          PipelineViewAclDbAccessor.DeletePipelineUserViewsFromFileSystem(userId, viewIds);
        PipelineViewAclDbAccessor.updateCacheUserPipelineCache(userId);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "Pipeline User View can not be Deleted. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return true;
    }

    public static Dictionary<string, string> GetExistingPipelineUserViewIds(
      string userId,
      IEnumerable<string> viewIds)
    {
      return PipelineViewAclDbAccessor.IsDbSaveEnabled() ? PipelineViewAclDbAccessor.GetExistingPipelineUserViewIdsDbQuery(userId, viewIds).ExecuteTableQuery().Rows.OfType<DataRow>().ToDictionary<DataRow, string, string>((System.Func<DataRow, string>) (x => x["viewID"].ToString()), (System.Func<DataRow, string>) (x => x["name"].ToString()), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) : ((IEnumerable<FileSystemEntry>) TemplateSettingsStore.GetDirectoryEntries(TemplateSettingsType.PipelineView, FileSystemEntry.PrivateRoot(userId))).Where<FileSystemEntry>((System.Func<FileSystemEntry, bool>) (x => viewIds.Contains<string>(x.Name, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))).ToDictionary<FileSystemEntry, string, string>((System.Func<FileSystemEntry, string>) (x => x.Name), (System.Func<FileSystemEntry, string>) (x => x.Name), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    }

    private static int CreatePipelineUserViewInDatabase(UserPipelineView view, string auditUser)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineViews");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Acl_UserPipelineView_Columns");
      DbValueList dbValueList1 = PipelineViewAclDbAccessor.createDbValueList(view, auditUser, true);
      dbQueryBuilder.Reset();
      dbQueryBuilder.Declare("@viewID", "int");
      dbQueryBuilder.InsertInto(table1, dbValueList1, true, false);
      dbQueryBuilder.SelectIdentity("@viewID");
      for (int index = 0; index < view.Columns.Count; ++index)
      {
        DbValueList dbValueList2 = PipelineViewAclDbAccessor.createDbValueList(view.Columns[index]);
        dbValueList2.Add("viewID", (object) "@viewID", (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
      }
      dbQueryBuilder.Select("@viewID");
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    private static DbValueList createDbValueList(
      PersonaPipelineView view,
      int orderIndex,
      string auditUser,
      bool isNew)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("name", (object) view.Name);
      dbValueList.Add("personaID", (object) view.PersonaID);
      dbValueList.Add("loanFolders", (object) view.LoanFolders);
      dbValueList.Add("ownership", (object) (int) view.Ownership);
      if (orderIndex >= 0)
        dbValueList.Add(nameof (orderIndex), (object) orderIndex);
      dbValueList.Add("filterXml", view.Filter == null ? (object) (string) null : (object) view.Filter.ToXml());
      if (isNew)
      {
        dbValueList.Add("CreatedDate", (object) DateTime.UtcNow);
        dbValueList.Add("CreatedBy", (object) auditUser);
      }
      dbValueList.Add("LastModifiedDate", (object) DateTime.UtcNow);
      dbValueList.Add("LastModifiedBy", (object) auditUser);
      return dbValueList;
    }

    private static DbValueList createDbValueListForName(UserPipelineView view, string auditUser)
    {
      return new DbValueList()
      {
        {
          "name",
          (object) view.Name
        },
        {
          "LastModifiedDate",
          (object) DateTime.UtcNow
        },
        {
          "LastModifiedBy",
          (object) auditUser
        }
      };
    }

    private static DbValueList createDbValueList(
      UserPipelineView view,
      string auditUser,
      bool isNew)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("name", (object) view.Name);
      dbValueList.Add("userID", (object) view.UserID);
      dbValueList.Add("loanFolders", (object) view.LoanFolders);
      dbValueList.Add("ownership", (object) view.Ownership.ToString());
      dbValueList.Add("orgType", (object) view.OrgType.ToString());
      dbValueList.Add("externalOrgId", (object) view.ExternalOrgId);
      dbValueList.Add("filterXml", view.Filter == null ? (object) (string) null : (object) view.Filter.ToXml());
      if (isNew)
      {
        dbValueList.Add("CreatedDate", (object) DateTime.UtcNow);
        dbValueList.Add("CreatedBy", (object) auditUser);
      }
      dbValueList.Add("LastModifiedDate", (object) DateTime.UtcNow);
      dbValueList.Add("LastModifiedBy", (object) auditUser);
      return dbValueList;
    }

    private static DbValueList createDbValueList(PersonaPipelineViewColumn column)
    {
      return new DbValueList()
      {
        {
          "fieldDBName",
          (object) column.ColumnDBName
        },
        {
          "orderIndex",
          (object) column.OrderIndex
        },
        {
          "sortby",
          (object) (int) column.SortOrder
        },
        {
          "width",
          (object) column.Width
        }
      };
    }

    private static DbValueList createDbValueList(
      UserPipelineViewColumn column,
      bool includeRequired = true)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("fieldDBName", (object) column.ColumnDBName);
      dbValueList.Add("orderIndex", (object) column.OrderIndex);
      dbValueList.Add("sortOrder", (object) column.SortOrder.ToString());
      dbValueList.Add("sortPriority", (object) column.SortPriority);
      dbValueList.Add("alignment", (object) column.Alignment);
      if (includeRequired)
        dbValueList.Add("IsRequired", (object) (column.IsRequired ? 1 : 0), (IDbEncoder) DbEncoding.None);
      dbValueList.Add("width", (object) column.Width);
      return dbValueList;
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder GetUserPipelineViewDbQuery(
      string userID,
      string viewName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder pipelineViewDbQuery = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      pipelineViewDbQuery.AppendLine("select uv.* from Acl_UserPipelineViews uv");
      pipelineViewDbQuery.AppendLine("  inner join users u on uv.userId = u.userId");
      pipelineViewDbQuery.AppendLine("where uv.userId = " + SQL.Encode((object) userID));
      pipelineViewDbQuery.AppendLine("And uv.name = " + SQL.Encode((object) viewName));
      pipelineViewDbQuery.AppendLine("select puv.* from Acl_UserPipelineView_Columns puv");
      pipelineViewDbQuery.AppendLine("  inner join Acl_UserPipelineViews uv on uv.viewID = puv.viewID");
      pipelineViewDbQuery.AppendLine("where uv.userId = " + SQL.Encode((object) userID));
      pipelineViewDbQuery.AppendLine("order by puv.sortOrder");
      return pipelineViewDbQuery;
    }

    private static UserPipelineView[] GetTemplateFromCIFS(string userId)
    {
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(TemplateSettingsType.PipelineView, FileSystemEntry.PrivateRoot(userId));
      List<UserPipelineView> userPipelineViewList = new List<UserPipelineView>();
      foreach (FileSystemEntry entry in directoryEntries)
      {
        TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.PipelineView, entry);
        if (latestVersion != null)
          userPipelineViewList.Add((UserPipelineView) latestVersion.Data);
      }
      return userPipelineViewList.ToArray();
    }

    public static UserPipelineView[] GetPipelineViewsFromDataSource(string userId)
    {
      if (!PipelineViewAclDbAccessor.IsDbSaveEnabled())
        return PipelineViewAclDbAccessor.GetTemplateFromCIFS(userId);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select uv.* from Acl_UserPipelineViews uv");
      dbQueryBuilder.AppendLine("  inner join users u on uv.userId = u.userId");
      dbQueryBuilder.AppendLine("where u.userid = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("select puv.* from Acl_UserPipelineView_Columns puv");
      dbQueryBuilder.AppendLine("  inner join Acl_UserPipelineViews uv on uv.viewID = puv.viewID");
      dbQueryBuilder.AppendLine("where uv.userid = " + SQL.Encode((object) userId));
      dbQueryBuilder.AppendLine("order by puv.sortOrder");
      return PipelineViewAclDbAccessor.dataSetToUserPiplineViews(dbQueryBuilder.ExecuteSetQuery());
    }

    public static UserPipelineView[] GetUserCustomPipelineViews(string userId)
    {
      return ClientContext.GetCurrent().Cache.Get<UserPipelineView[]>("UserCustomPipeline", userId, (Func<UserPipelineView[]>) (() => PipelineViewAclDbAccessor.GetPipelineViewsFromDataSource(userId)));
    }

    public static bool IsDbSaveEnabled()
    {
      ClientContext current = ClientContext.GetCurrent();
      object serverSetting = current.Settings.GetServerSetting("POLICIES.UseUserPipelineViewFromDB", false);
      string str = current.Settings.GetServerSetting("MIGRATION.MigrateUserPipelineView", false).ToString();
      return Utils.ParseBoolean(serverSetting) && str == "1";
    }

    private static UserPipelineView[] dataSetToUserPiplineViews(DataSet viewColumnData)
    {
      List<UserPipelineView> userPipelineViewList = new List<UserPipelineView>();
      foreach (DataRow row in (InternalDataCollectionBase) viewColumnData.Tables[0].Rows)
        userPipelineViewList.Add(PipelineViewAclDbAccessor.dataRowToUserPiplineView(row, viewColumnData.Tables[1]));
      return userPipelineViewList.ToArray();
    }

    private static UserPipelineViewColumn dataRowToUserPiplineViewColumn(DataRow r)
    {
      return new UserPipelineViewColumn((string) r["fieldDBName"], SQL.DecodeInt(r["orderIndex"], -1), SQL.DecodeEnum<SortOrder>(r["sortOrder"]), SQL.DecodeInt(r["width"], -1), (int) r["sortPriority"], (string) r["alignment"], (bool) r["isRequired"]);
    }

    private static UserPipelineView dataRowToUserPiplineView(DataRow viewRow, DataTable columnTable)
    {
      int viewID = (int) viewRow["viewID"];
      List<UserPipelineViewColumn> pipelineViewColumnList = new List<UserPipelineViewColumn>();
      foreach (DataRow r in columnTable.Select("viewID = " + (object) viewID))
        pipelineViewColumnList.Add(PipelineViewAclDbAccessor.dataRowToUserPiplineViewColumn(r));
      return new UserPipelineView(viewID, (string) viewRow["userID"], (string) viewRow["name"], (string) SQL.Decode(viewRow["loanFolders"], (object) null), SQL.DecodeEnum<PipelineViewLoanOwnership>(viewRow["ownership"], PipelineViewLoanOwnership.All), SQL.DecodeEnum<PipelineViewLoanOrgType>(viewRow["orgType"], PipelineViewLoanOrgType.None), (string) SQL.Decode(viewRow["externalOrgId"], (object) null), pipelineViewColumnList.ToArray(), (string) SQL.Decode(viewRow["filterXml"], (object) null));
    }

    public static bool PersonaPipelineViewWithNameExists(int personaID, string viewName)
    {
      return PipelineViewAclDbAccessor.GetPersonaPipelineViewDbQuery(personaID, viewName).ExecuteSetQuery().Tables[0].Rows.Count != 0;
    }

    private static UserPipelineView GetTemplateFromViewName(string userId, string viewName)
    {
      FileSystemEntry entry = FileSystemEntry.PrivateRoot(userId).Combine(viewName);
      if (TemplateSettings.IsExists(TemplateSettingsType.PipelineView, entry))
      {
        TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.PipelineView, entry);
        if (latestVersion != null)
          return (UserPipelineView) latestVersion.Data;
      }
      return (UserPipelineView) null;
    }

    private static string CreatePipelineUserViewInFileSystem(UserPipelineView view, string userId)
    {
      FileSystemEntry entry = new FileSystemEntry("\\" + view.Name, FileSystemEntry.Types.File, userId);
      if (entry == null)
        Err.Raise(nameof (PipelineViewAclDbAccessor), (ServerException) new ServerArgumentException("Specified file system entry cannot be null. SaveTemplateToCIFS()"));
      BinaryObject pipelineView = (BinaryObject) (BinaryConvertibleObject) PipelineViewAclDbAccessor.UserViewToPipelineView(view);
      pipelineView.Download();
      try
      {
        bool flag = true;
        using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(TemplateSettingsType.PipelineView, entry))
        {
          if (!templateSettings.Exists)
            flag = false;
          templateSettings.CheckIn(pipelineView);
        }
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(userId, User.GetUserById(userId).FullName, flag ? ActionType.TemplateModified : ActionType.TemplateCreated, DateTime.Now, entry.Name, entry.Path));
        return view.Name;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "User Pipeline View can not be Created. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return (string) null;
    }

    private static UserPipelineView UpdatePipelineUserViewInFileSystem(
      UserPipelineView view,
      string userId,
      string oldViewName)
    {
      if (string.IsNullOrWhiteSpace(oldViewName))
        throw new ArgumentException("Argument cannot be blank or null", nameof (oldViewName));
      FileSystemEntry entry1 = new FileSystemEntry("\\" + view.Name, FileSystemEntry.Types.File, userId);
      FileSystemEntry entry2 = (FileSystemEntry) null;
      bool flag = !string.Equals(oldViewName, view.Name, StringComparison.CurrentCultureIgnoreCase);
      if (flag)
      {
        entry2 = new FileSystemEntry("\\" + oldViewName, FileSystemEntry.Types.File, userId);
        if (!TemplateSettingsStore.Exists(TemplateSettingsType.PipelineView, entry2))
          throw new ObjectNotFoundException("View not found with name '" + oldViewName + "'", ObjectType.Template, (object) oldViewName);
        if (TemplateSettingsStore.Exists(TemplateSettingsType.PipelineView, entry1))
          throw new DuplicateObjectException(string.Format("View already exists with name '{0}'", (object) entry1), ObjectType.Template, (object) view.Name);
      }
      BinaryObject pipelineView = (BinaryObject) (BinaryConvertibleObject) PipelineViewAclDbAccessor.UserViewToPipelineView(view);
      pipelineView.Download();
      try
      {
        using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(TemplateSettingsType.PipelineView, entry1))
          templateSettings.CheckIn(pipelineView);
        if (flag)
        {
          using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(TemplateSettingsType.PipelineView, entry2))
            templateSettings.Delete();
        }
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(userId, User.GetUserById(userId).FullName, ActionType.TemplateModified, DateTime.Now, entry1.Name, entry1.Path));
        return PipelineViewAclDbAccessor.GetTemplateFromViewName(userId, entry1.Name);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "User Pipeline View can not be updated. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
      return (UserPipelineView) null;
    }

    private static void DeletePipelineUserViewsFromFileSystem(
      string userId,
      IEnumerable<string> viewNames)
    {
      try
      {
        foreach (string viewName in viewNames)
          TemplateSettingsStore.Delete(TemplateSettingsType.PipelineView, FileSystemEntry.PrivateRoot(userId).Combine(viewName));
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PipelineViewAclDbAccessor), "Pipeline User Views can not be deleted. Error: " + ex.Message);
        Err.Reraise(nameof (PipelineViewAclDbAccessor), ex);
      }
    }

    private static PipelineView UserViewToPipelineView(UserPipelineView view)
    {
      return new PipelineView(view.Name)
      {
        LoanFolder = view.LoanFolders,
        Filter = view.Filter,
        IsUserView = true,
        LoanOwnership = view.Ownership,
        LoanOrgType = view.OrgType,
        ExternalOrgId = view.ExternalOrgId,
        Layout = PipelineViewAclDbAccessor.GetTableLayout(view.Columns)
      };
    }

    private static TableLayout GetTableLayout(UserPipelineViewColumnCollection columns)
    {
      TableLayout tableLayout = new TableLayout();
      foreach (UserPipelineViewColumn column in columns)
      {
        HorizontalAlignment result;
        Enum.TryParse<HorizontalAlignment>(column.Alignment, out result);
        tableLayout.AddColumn(new TableLayout.Column(column.ColumnDBName, column.ColumnDBName, "", column.ColumnDBName, result, column.Width, column.IsRequired)
        {
          SortOrder = column.SortOrder
        });
      }
      return tableLayout;
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder GetExistingPipelineUserViewIdsDbQuery(
      string userID,
      IEnumerable<string> viewIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder userViewIdsDbQuery = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      userViewIdsDbQuery.AppendLine("select uv.viewId, uv.name from Acl_UserPipelineViews uv");
      userViewIdsDbQuery.AppendLine("  inner join users u on uv.userId = u.userId");
      userViewIdsDbQuery.AppendLine("where uv.userId = " + SQL.Encode((object) userID) + " AND uv.viewId in (" + string.Join(", ", viewIds.Select<string, string>((System.Func<string, string>) (x => SQL.Encode((object) x)))) + ")");
      return userViewIdsDbQuery;
    }
  }
}
