// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.IRS4506TTemplateDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class IRS4506TTemplateDbAccessor
  {
    private static string IRS4506TTemplateTable = "TaxTranscriptRequestTemplate";
    private static string[] irs4506TTemplateColumnNames = new string[6]
    {
      "TemplateID",
      "TemplateName",
      "RequestVersion",
      "RequestYears",
      "LastModifiedBy",
      "LastModifiedDateTime"
    };

    public static List<IRS4506TTemplate> GetIRS4506TTemplates()
    {
      return IRS4506TTemplateDbAccessor.GetIRS4506TTemplates(false);
    }

    public static List<IRS4506TTemplate> GetIRS4506TTemplates(bool listOnly)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      if (listOnly)
        dbQueryBuilder.SelectFrom(table, IRS4506TTemplateDbAccessor.irs4506TTemplateColumnNames);
      else
        dbQueryBuilder.SelectFrom(table);
      dbQueryBuilder.Append("order by LastModifiedDateTime desc");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<IRS4506TTemplate> irS4506Ttemplates = new List<IRS4506TTemplate>();
      if (dataRowCollection != null)
      {
        foreach (DataRow dr in (InternalDataCollectionBase) dataRowCollection)
        {
          IRS4506TTemplate template = IRS4506TTemplateDbAccessor.convertDataRowToTemplate(dr, listOnly);
          if (template != null)
            irS4506Ttemplates.Add(template);
        }
      }
      return irS4506Ttemplates;
    }

    public static (List<IRS4506TTemplate> Templates, int TotalCount) GetPagedIRS4506TTemplates(
      string[] formVersions = null,
      int start = 0,
      int limit = 0)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      DbValueList dbValueList;
      if ((formVersions != null ? (!((IEnumerable<string>) formVersions).Any<string>() ? 1 : 0) : 1) == 0)
        dbValueList = new DbValueList()
        {
          {
            "RequestVersion",
            (object) formVersions
          }
        };
      else
        dbValueList = (DbValueList) null;
      DbValueList keys = dbValueList;
      dbQueryBuilder.SelectFrom(table, IRS4506TTemplateDbAccessor.irs4506TTemplateColumnNames, keys);
      bool flag = start > 0 || limit > 0;
      DataRowCollection dataRowCollection;
      if (!flag)
      {
        dataRowCollection = dbQueryBuilder.Execute();
      }
      else
      {
        int startRecordNumber = start <= 0 ? 1 : start + 1;
        int endRecordNumber = limit <= 0 ? int.MaxValue : startRecordNumber + limit - 1;
        dataRowCollection = new EllieMae.EMLite.Server.DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), startRecordNumber, endRecordNumber, (List<SortColumn>) null).Rows;
      }
      List<IRS4506TTemplate> irS4506TtemplateList = new List<IRS4506TTemplate>();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return (irS4506TtemplateList, 0);
      int result = !flag || !int.TryParse(dataRowCollection[0]["TotalRowCount"].ToString(), out result) ? dataRowCollection.Count : result;
      foreach (DataRow dr in (InternalDataCollectionBase) dataRowCollection)
      {
        IRS4506TTemplate template = IRS4506TTemplateDbAccessor.convertDataRowToTemplate(dr, true);
        if (template != null)
          irS4506TtemplateList.Add(template);
      }
      return (irS4506TtemplateList, result);
    }

    public static IRS4506TTemplate GetIRS4506TTemplate(int templateID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      dbQueryBuilder.SelectFrom(table, new DbValue("TemplateID", (object) templateID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return (IRS4506TTemplate) null;
      IRS4506TTemplate irS4506Ttemplate = (IRS4506TTemplate) null;
      IEnumerator enumerator = dataRowCollection.GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
          irS4506Ttemplate = IRS4506TTemplateDbAccessor.convertDataRowToTemplate((DataRow) enumerator.Current, false);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return irS4506Ttemplate;
    }

    public static IRS4506TTemplate GetIRS4506TTemplate(string templateName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      dbQueryBuilder.SelectFrom(table, new DbValue("TemplateName", (object) templateName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return (IRS4506TTemplate) null;
      IRS4506TTemplate irS4506Ttemplate = (IRS4506TTemplate) null;
      IEnumerator enumerator = dataRowCollection.GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
          irS4506Ttemplate = IRS4506TTemplateDbAccessor.convertDataRowToTemplate((DataRow) enumerator.Current, false);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return irS4506Ttemplate;
    }

    private static IRS4506TTemplate convertDataRowToTemplate(DataRow dr, bool listOnly)
    {
      IRS4506TTemplate template = new IRS4506TTemplate(listOnly || dr["TemplateData"] == DBNull.Value ? string.Empty : (string) dr["TemplateData"]);
      if (template != null)
      {
        template.TemplateID = (int) dr["TemplateID"];
        template.TemplateName = (string) dr["TemplateName"];
        template.RequestVersion = dr.IsNull("RequestVersion") ? "" : (string) dr["RequestVersion"];
        template.RequestYears = dr.IsNull("RequestYears") ? "" : (string) dr["RequestYears"];
        template.LastModifiedBy = (string) dr["LastModifiedBy"];
        template.LastModifiedDateTime = (DateTime) dr["LastModifiedDateTime"];
      }
      if (!listOnly && (string.IsNullOrEmpty(template.RequestVersion) || string.IsNullOrEmpty(template.RequestYears)))
      {
        IRS4506TFields templateData = template.GetTemplateData();
        template.RequestVersion = templateData.Version;
        template.RequestYears = templateData.YearsRequested;
      }
      return template;
    }

    public static int CreateIRS4506TTemplate(IRS4506TTemplate template)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      dbQueryBuilder.InsertInto(table, new DbValueList()
      {
        {
          "TemplateName",
          (object) template.TemplateName
        },
        {
          "RequestVersion",
          (object) template.RequestVersion
        },
        {
          "RequestYears",
          (object) template.RequestYears
        },
        {
          "LastModifiedBy",
          (object) template.LastModifiedBy
        },
        {
          "LastModifiedDateTime",
          (object) template.LastModifiedDateTime
        },
        {
          "TemplateData",
          (object) template.XmlData
        }
      }, true, false);
      dbQueryBuilder.SelectIdentity();
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
    }

    public static void UpdateIRS4506TTemplate(IRS4506TTemplate template)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      DbValueList values = new DbValueList();
      values.Add("TemplateName", (object) template.TemplateName);
      values.Add("RequestVersion", (object) template.RequestVersion);
      values.Add("RequestYears", (object) template.RequestYears);
      values.Add("LastModifiedBy", (object) template.LastModifiedBy);
      values.Add("LastModifiedDateTime", (object) template.LastModifiedDateTime);
      values.Add("TemplateData", (object) template.XmlData);
      DbValue key = new DbValue("TemplateID", (object) template.TemplateID, (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.Update(table, values, key);
      dbQueryBuilder.ExecuteScalar();
    }

    public static void DeleteIRS4506TTemplate(string templateName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(IRS4506TTemplateDbAccessor.IRS4506TTemplateTable);
      DbValue key = new DbValue("TemplateName", (object) templateName);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteScalar();
    }
  }
}
