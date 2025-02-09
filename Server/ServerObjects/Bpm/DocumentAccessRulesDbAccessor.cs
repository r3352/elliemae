// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.DocumentAccessRulesDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public static class DocumentAccessRulesDbAccessor
  {
    private const string ruleTableName = "BR_DefaultDocumentAccessRules�";

    public static DocumentDefaultAccessRuleInfo[] GetDocumentDefaultAccessRules()
    {
      return ClientContext.GetCurrent().Cache.Get<DocumentDefaultAccessRuleInfo[]>("BR_DefaultDocumentAccessRules", new Func<DocumentDefaultAccessRuleInfo[]>(DocumentAccessRulesDbAccessor.getDocumentDefaultAccessRulesFromDatabase), CacheSetting.Low);
    }

    public static DocumentDefaultAccessRuleInfo GetDocumentDefaultAccessRule(int roleAddedBy)
    {
      if (ClientContext.GetCurrent().Settings.CacheSetting == CacheSetting.Disabled)
        return DocumentAccessRulesDbAccessor.getDocumentDefaultAccessRuleFromDatabase(roleAddedBy);
      foreach (DocumentDefaultAccessRuleInfo defaultAccessRule in DocumentAccessRulesDbAccessor.GetDocumentDefaultAccessRules())
      {
        if (defaultAccessRule.RoleAddedBy == roleAddedBy)
          return defaultAccessRule;
      }
      return (DocumentDefaultAccessRuleInfo) null;
    }

    public static void SaveDocumentDefaultAccessRules(DocumentDefaultAccessRuleInfo[] rules)
    {
      ClientContext.GetCurrent().Cache.Put<DocumentDefaultAccessRuleInfo[]>("BR_DefaultDocumentAccessRules", (Action) (() => DocumentAccessRulesDbAccessor.saveDocumentDefaultAccessRulesToDatabase(rules)), new Func<DocumentDefaultAccessRuleInfo[]>(DocumentAccessRulesDbAccessor.getDocumentDefaultAccessRulesFromDatabase), CacheSetting.Low);
      DocumentAccessRulesDbAccessor.raiseCacheControlEvent();
    }

    [PgReady]
    private static DocumentDefaultAccessRuleInfo[] getDocumentDefaultAccessRulesFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_DefaultDocumentAccessRules"));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        Hashtable hashtable = new Hashtable();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          int num1 = (int) dataRow["RoleAddedBy"];
          int num2 = (int) dataRow["PermitAccessBy"];
          if (!hashtable.Contains((object) num1))
            hashtable.Add((object) num1, (object) new DocumentDefaultAccessRuleInfo(num1));
          ((DocumentDefaultAccessRuleInfo) hashtable[(object) num1]).RolesAllowedAccess.Add(num2);
        }
        DocumentDefaultAccessRuleInfo[] rulesFromDatabase = new DocumentDefaultAccessRuleInfo[hashtable.Count];
        hashtable.Values.CopyTo((Array) rulesFromDatabase, 0);
        return rulesFromDatabase;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_DefaultDocumentAccessRules"));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      Hashtable hashtable1 = new Hashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
      {
        int num3 = (int) dataRow["RoleAddedBy"];
        int num4 = (int) dataRow["PermitAccessBy"];
        if (!hashtable1.Contains((object) num3))
          hashtable1.Add((object) num3, (object) new DocumentDefaultAccessRuleInfo(num3));
        ((DocumentDefaultAccessRuleInfo) hashtable1[(object) num3]).RolesAllowedAccess.Add(num4);
      }
      DocumentDefaultAccessRuleInfo[] rulesFromDatabase1 = new DocumentDefaultAccessRuleInfo[hashtable1.Count];
      hashtable1.Values.CopyTo((Array) rulesFromDatabase1, 0);
      return rulesFromDatabase1;
    }

    private static DocumentDefaultAccessRuleInfo getDocumentDefaultAccessRuleFromDatabase(
      int roleAddedBy)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_DefaultDocumentAccessRules"), new DbValue("RoleAddedBy", (object) roleAddedBy));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      DocumentDefaultAccessRuleInfo ruleFromDatabase = new DocumentDefaultAccessRuleInfo(roleAddedBy);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        ruleFromDatabase.RolesAllowedAccess.Add((int) dataRow["PermitAccessBy"]);
      return ruleFromDatabase;
    }

    private static void saveDocumentDefaultAccessRulesToDatabase(
      DocumentDefaultAccessRuleInfo[] rules)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("BR_DefaultDocumentAccessRules");
      dbQueryBuilder.DeleteFrom(table);
      foreach (DocumentDefaultAccessRuleInfo rule in rules)
      {
        foreach (int num in (CollectionBase) rule.RolesAllowedAccess)
          dbQueryBuilder.InsertInto(table, new DbValueList()
          {
            {
              "RoleAddedBy",
              (object) rule.RoleAddedBy
            },
            {
              "PermitAccessBy",
              (object) num
            }
          }, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void raiseCacheControlEvent()
    {
      ClientContext.GetCurrent().Sessions.BroadcastMessage((Message) new CacheControlMessage(ClientSessionCacheID.DocumentAccessRule), false);
    }
  }
}
