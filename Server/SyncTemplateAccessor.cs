// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SyncTemplateAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class SyncTemplateAccessor
  {
    private const string className = "SyncTemplateAccessor�";
    private static string SYNC_TEMPLATES_TABLE = "SyncTemplates";
    private static string SYNC_TEMPLATE_FIELDS_TABLE = "SyncTemplateFields";

    public static List<SyncTemplate> GetAllSyncTemplates()
    {
      List<SyncTemplate> syncTemplates = (List<SyncTemplate>) null;
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.AppendLine("SELECT * FROM [" + SyncTemplateAccessor.SYNC_TEMPLATES_TABLE + "]");
      DataSet dataSet = dbQueryBuilder1.ExecuteSetQuery();
      if (dataSet != null)
      {
        syncTemplates = new List<SyncTemplate>();
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          int int32 = Convert.ToInt32(row["TemplateID"]);
          string templateName = string.Concat(SQL.Decode(row["TemplateName"]));
          string templateDescription = string.Concat(SQL.Decode(row["TemplateDescription"]));
          syncTemplates.Add(new SyncTemplate(int32, templateName, templateDescription));
        }
        if (syncTemplates.Count > 0)
        {
          DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
          dbQueryBuilder2.AppendLine("SELECT * FROM [" + SyncTemplateAccessor.SYNC_TEMPLATE_FIELDS_TABLE + "]");
          DataSet data = dbQueryBuilder2.ExecuteSetQuery();
          if (data != null)
            SyncTemplateAccessor.populateSyncFieldsFromDataRow(syncTemplates, data);
        }
      }
      return syncTemplates;
    }

    private static void populateSyncFieldsFromDataRow(
      List<SyncTemplate> syncTemplates,
      DataSet data)
    {
      Hashtable hashtable1 = new Hashtable();
      foreach (DataRow row in (InternalDataCollectionBase) data.Tables[0].Rows)
      {
        int int32 = Convert.ToInt32(row["TemplateID"]);
        string str = string.Concat(SQL.Decode(row["TemplateFieldID"]));
        if (!hashtable1.ContainsKey((object) int32))
          hashtable1.Add((object) int32, (object) new List<string>());
        List<string> stringList = (List<string>) hashtable1[(object) int32];
        if (!stringList.Contains(str))
          stringList.Add(str);
      }
      Hashtable hashtable2 = new Hashtable();
      for (int index = 0; index < syncTemplates.Count; ++index)
      {
        if (!hashtable2.ContainsKey((object) syncTemplates[index].TemplateID))
          hashtable2.Add((object) syncTemplates[index].TemplateID, (object) syncTemplates[index]);
      }
      foreach (DictionaryEntry dictionaryEntry in hashtable1)
      {
        int key = (int) dictionaryEntry.Key;
        List<string> fieldIDs = (List<string>) dictionaryEntry.Value;
        if (hashtable2.ContainsKey((object) key))
          ((SyncTemplate) hashtable2[(object) key]).AddFields(fieldIDs);
      }
    }

    public static List<int> RemoveSyncTemplates(List<int> ids)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      string str = "";
      for (int index = 0; index < ids.Count; ++index)
        str = str + (str != "" ? (object) ", " : (object) "") + (object) ids[index];
      dbQueryBuilder1.AppendLine("delete from " + SyncTemplateAccessor.SYNC_TEMPLATES_TABLE + " where [TemplateID] in (" + str + ")");
      dbQueryBuilder1.AppendLine("select @@ROWCOUNT");
      int num;
      try
      {
        TraceLog.WriteInfo(nameof (SyncTemplateAccessor), dbQueryBuilder1.ToString());
        num = Utils.ParseInt(dbQueryBuilder1.ExecuteScalar());
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SyncTemplateAccessor), ex);
        throw new Exception("Cannot delete record(s) in SyncTemplates and SyncTemplateFields tables due to the following error:\r\n" + ex.Message);
      }
      if (num == ids.Count)
        return ids;
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      dbQueryBuilder2.AppendLine("select TemplateID from " + SyncTemplateAccessor.SYNC_TEMPLATES_TABLE);
      DataSet dataSet = dbQueryBuilder2.ExecuteSetQuery();
      List<int> intList1 = new List<int>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        int int32 = Convert.ToInt32(row["TemplateID"]);
        intList1.Add(int32);
      }
      List<int> intList2 = new List<int>();
      for (int index = 0; index < ids.Count; ++index)
      {
        if (!intList1.Contains(ids[index]))
          intList2.Add(ids[index]);
      }
      return intList2;
    }

    public static int UpdateSyncTemplate(SyncTemplate syncTemplate)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (syncTemplate.TemplateID == -1)
      {
        dbQueryBuilder.AppendLine("declare @templateID int");
        dbQueryBuilder.AppendLine("insert into " + SyncTemplateAccessor.SYNC_TEMPLATES_TABLE + " (TemplateName, TemplateDescription) values (" + SQL.EncodeString(syncTemplate.TemplateName) + ", " + SQL.EncodeString(syncTemplate.TemplateDescription) + ")");
        dbQueryBuilder.SelectIdentity("@templateID");
        dbQueryBuilder.AppendLine("delete from " + SyncTemplateAccessor.SYNC_TEMPLATE_FIELDS_TABLE + " where [TemplateID] = @templateID");
        for (int index = 0; index < syncTemplate.SyncFields.Count; ++index)
          dbQueryBuilder.AppendLine("insert into " + SyncTemplateAccessor.SYNC_TEMPLATE_FIELDS_TABLE + " (TemplateID, TemplateFieldID) values (@templateID, " + SQL.EncodeString(syncTemplate.SyncFields[index]) + ")");
        dbQueryBuilder.Select("@templateID");
        try
        {
          TraceLog.WriteInfo(nameof (SyncTemplateAccessor), dbQueryBuilder.ToString());
          return Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SyncTemplateAccessor), ex);
          throw new Exception("Cannot insert record to SyncTemplates and SyncTemplateFields tables due to the following error:\r\n" + ex.Message);
        }
      }
      else
      {
        dbQueryBuilder.AppendLine("update " + SyncTemplateAccessor.SYNC_TEMPLATES_TABLE + " set [TemplateName] = " + SQL.EncodeString(syncTemplate.TemplateName) + ", [TemplateDescription] = " + SQL.EncodeString(syncTemplate.TemplateDescription) + " where [TemplateID] = " + (object) syncTemplate.TemplateID);
        dbQueryBuilder.AppendLine("delete " + SyncTemplateAccessor.SYNC_TEMPLATE_FIELDS_TABLE + " where [TemplateID] = " + (object) syncTemplate.TemplateID);
        for (int index = 0; index < syncTemplate.SyncFields.Count; ++index)
          dbQueryBuilder.AppendLine("insert into " + SyncTemplateAccessor.SYNC_TEMPLATE_FIELDS_TABLE + " (TemplateID, TemplateFieldID) values (" + (object) syncTemplate.TemplateID + ", " + SQL.EncodeString(syncTemplate.SyncFields[index]) + ")");
        try
        {
          TraceLog.WriteInfo(nameof (SyncTemplateAccessor), dbQueryBuilder.ToString());
          dbQueryBuilder.ExecuteScalar();
          return syncTemplate.TemplateID;
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SyncTemplateAccessor), ex);
          throw new Exception("Cannot update SyncTemplates and SyncTemplateFields tables due to the following error:\r\n" + ex.Message);
        }
      }
    }

    public static bool SyncTemplateNameExist(string templateName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select count(*) from " + SyncTemplateAccessor.SYNC_TEMPLATES_TABLE + " where [TemplateName] = " + SQL.EncodeString(templateName));
      dbQueryBuilder.AppendLine("select @@ROWCOUNT");
      try
      {
        TraceLog.WriteInfo(nameof (SyncTemplateAccessor), dbQueryBuilder.ToString());
        return Utils.ParseInt(dbQueryBuilder.ExecuteScalar()) > 0;
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (SyncTemplateAccessor), ex);
        throw new Exception("Cannot check duplicated record in SyncTemplates due to the following error:\r\n" + ex.Message);
      }
    }
  }
}
