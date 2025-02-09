// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EnhancedConditionTemplateAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.EnhancedCondition;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class EnhancedConditionTemplateAccessor
  {
    private static readonly string _tableName = "EnhancedConditionTemplates";
    private static readonly string _className = "EnhancedConditionListAccessor";

    public static (IList<EnhancedConditionTemplate> ConditionTemplates, int TotalCount) GetEnhancedConditionTemplates(
      List<string> conditionTypes = null,
      bool activeOnly = true,
      int start = 0,
      int limit = 0,
      string title = null)
    {
      try
      {
        DbValueList keys = new DbValueList();
        if (activeOnly)
          keys.Add("Active", (object) 1);
        if (conditionTypes != null && conditionTypes.Any<string>())
          keys.Add("ConditionType", (object) conditionTypes.ToArray());
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new string[1]
        {
          "Data"
        }, keys);
        if (!string.IsNullOrWhiteSpace(title))
        {
          string str = SQL.Encode((object) ("%" + title + "%"));
          if (keys.Count > 0)
            dbQueryBuilder.AppendLine(" AND Title like " + str);
          else
            dbQueryBuilder.AppendLine(" Where Title like " + str);
        }
        bool flag = start > 0 || limit > 0;
        DataTable dataTable;
        if (!flag)
        {
          dataTable = dbQueryBuilder.ExecuteTableQuery(DbTransactionType.None);
        }
        else
        {
          int startRecordNumber = start <= 0 ? 1 : start + 1;
          int endRecordNumber = limit <= 0 ? int.MaxValue : startRecordNumber + limit - 1;
          dataTable = new EllieMae.EMLite.Server.DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), startRecordNumber, endRecordNumber, (List<SortColumn>) null);
        }
        List<EnhancedConditionTemplate> conditionTemplateList = new List<EnhancedConditionTemplate>();
        if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
          return ((IList<EnhancedConditionTemplate>) conditionTemplateList, 0);
        int result;
        int num = !flag || !int.TryParse(dataTable.Rows[0]["TotalRowCount"].ToString(), out result) ? dataTable.Rows.Count : result;
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          conditionTemplateList.Add(EnhancedConditionTemplateAccessor.TemplateModelGeneratorFor(row));
        return ((IList<EnhancedConditionTemplate>) conditionTemplateList, num);
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
      return ((IList<EnhancedConditionTemplate>) null, 0);
    }

    public static DataSet GetTemplateNameAndType(Guid setid)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select id as setid, setname, description ");
      dbQueryBuilder.AppendLine(" from EnhancedConditionSets ");
      dbQueryBuilder.AppendLine(" where id='" + (object) setid + "' ");
      dbQueryBuilder.AppendLine("select ct.id as tempId, ct.Title, ct.ConditionType ");
      dbQueryBuilder.AppendLine(" from EnhancedConditionSetTemplates cst ");
      dbQueryBuilder.AppendLine(" inner join EnhancedConditionTemplates ct on cst.TemplateId = ct.Id ");
      dbQueryBuilder.AppendLine(" where cst.SetId = '" + (object) setid + "' ");
      dbQueryBuilder.AppendLine("select id as tempId, title, conditiontype ");
      dbQueryBuilder.AppendLine(" from EnhancedConditionTemplates ");
      return dbQueryBuilder.ExecuteSetQuery();
    }

    public static EnhancedConditionTemplate GetEnhancedConditionTemplate(Guid templateId)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new string[1]
        {
          "Data"
        });
        dbQueryBuilder.Append("WHERE id = @templateId");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None, new DbCommandParameter(nameof (templateId), (object) templateId, DbType.Guid));
        return dataRowCollection == null || dataRowCollection.Count == 0 ? (EnhancedConditionTemplate) null : EnhancedConditionTemplateAccessor.TemplateModelGeneratorFor(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
      return (EnhancedConditionTemplate) null;
    }

    public static EnhancedConditionTemplate GetEnhancedConditionTemplate(
      string conditionType,
      string title)
    {
      try
      {
        DbValueList keys = new DbValueList();
        keys.Add("ConditionType", (object) conditionType);
        keys.Add("Title", (object) title);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new string[1]
        {
          "Data"
        }, keys);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        return dataRowCollection == null || dataRowCollection.Count == 0 ? (EnhancedConditionTemplate) null : EnhancedConditionTemplateAccessor.TemplateModelGeneratorFor(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
      return (EnhancedConditionTemplate) null;
    }

    public static List<EnhancedConditionTemplate> GetEnhancedConditionTemplatesByIds(
      List<Guid> templateIds)
    {
      try
      {
        if (templateIds == null || templateIds.Count<Guid>() <= 0)
          throw new ArgumentException("Must provide at least one enhanced condition template id");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbValueList keys = new DbValueList();
        keys.Add(new DbValueList()
        {
          (DbValue) new DbFilterValue("Id", (object) templateIds.Select<Guid, string>((System.Func<Guid, string>) (x => x.ToString())).ToArray<string>())
        });
        List<EnhancedConditionTemplate> conditionTemplatesByIds = new List<EnhancedConditionTemplate>();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new string[1]
        {
          "Data"
        }, keys);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection == null || dataRowCollection.Count == 0)
          return conditionTemplatesByIds;
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          EnhancedConditionTemplate conditionTemplate = EnhancedConditionTemplateAccessor.TemplateModelGeneratorFor(row);
          conditionTemplatesByIds.Add(conditionTemplate);
        }
        return conditionTemplatesByIds;
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
      return (List<EnhancedConditionTemplate>) null;
    }

    public static void CreateEnhancedConditionTemplates(IList<EnhancedConditionTemplate> templates)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      try
      {
        if (templates == null || templates.Count == 0)
          throw new ArgumentException("Enhanced Templates passed is NULL or Empty");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName);
        List<DbValueList> values = new List<DbValueList>();
        foreach (EnhancedConditionTemplate template in (IEnumerable<EnhancedConditionTemplate>) templates)
        {
          Guid guid = template != null ? template.Id : throw new ArgumentException("Enhanced Template passed is NULL or Template Id is NULL.");
          dbQueryBuilder.IfExists(table, new DbValueList()
          {
            {
              "Title",
              (object) template.Title
            },
            {
              "ConditionType",
              (object) template.ConditionType
            }
          });
          dbQueryBuilder.Begin();
          dbQueryBuilder.RaiseError("TemplateWithTitleAndTypeExists|" + string.Join(",", (IEnumerable<string>) new List<string>()
          {
            template.Title ?? "",
            template.ConditionType ?? ""
          }));
          dbQueryBuilder.AppendLine("return;");
          dbQueryBuilder.End();
          values.Add(EnhancedConditionTemplateAccessor.TemplateRowGeneratorForInsert(template));
        }
        dbQueryBuilder.InsertInto(table, values, DbVersion.Other);
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        if (string.Equals(ex.Message, "TemplateWithTitleAndTypeExists", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("TemplateWithTitleAndTypeExists", StringComparison.CurrentCultureIgnoreCase) >= 0)
        {
          List<string> list = ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>();
          throw new DuplicateObjectException("Templates already exist with the template title / condition type combinations.", ObjectType.ConditionTemplate, (object) new Dictionary<string, string>()
          {
            {
              list.FirstOrDefault<string>(),
              list.LastOrDefault<string>()
            }
          });
        }
        throw;
      }
    }

    private static EnhancedConditionTemplate TemplateModelGeneratorFor(DataRow row)
    {
      return EnhancedConditionTemplateAccessor.ToObject(row["Data"].ToString());
    }

    private static DbValueList TemplateRowGeneratorForInsert(EnhancedConditionTemplate template)
    {
      DateTime utcNow = DateTime.UtcNow;
      template.Id = Guid.NewGuid();
      template.LastModifiedDate = utcNow;
      template.CreatedDate = utcNow;
      return new DbValueList()
      {
        new DbValue("Id", (object) SQL.Encode((object) template.Id)),
        new DbValue("Title", (object) template.Title),
        new DbValue("ConditionType", (object) template.ConditionType),
        new DbValue("Data", (object) EnhancedConditionTemplateAccessor.ToJson(template)),
        new DbValue("Active", (object) (template.Active ? 1 : 0)),
        new DbValue("LastModifiedBy", (object) template.LastModifiedBy),
        new DbValue("LastModifiedDate", (object) template.LastModifiedDate.ToString("yyyy-MM-ddTHH:mm:ssZ")),
        new DbValue("CreatedBy", (object) template.CreatedBy),
        new DbValue("CreatedDate", (object) template.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ssZ"))
      };
    }

    public static DbValueList TemplateRowGeneratorForUpdate(EnhancedConditionTemplate template)
    {
      template.LastModifiedDate = DateTime.UtcNow;
      return new DbValueList()
      {
        new DbValue("Title", (object) template.Title),
        new DbValue("ConditionType", (object) template.ConditionType),
        new DbValue("Data", (object) EnhancedConditionTemplateAccessor.ToJson(template)),
        new DbValue("Active", (object) (template.Active ? 1 : 0)),
        new DbValue("LastModifiedBy", (object) template.LastModifiedBy),
        new DbValue("LastModifiedDate", (object) template.LastModifiedDate.ToString("yyyy-MM-ddTHH:mm:ssZ"))
      };
    }

    public static void UpdateEnhancedConditionTemplates(IList<EnhancedConditionTemplate> templates)
    {
      try
      {
        if (templates == null || !templates.Any<EnhancedConditionTemplate>())
          throw new ArgumentException("Enhanced Templates passed is NULL or Empty");
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        foreach (EnhancedConditionTemplate template in (IEnumerable<EnhancedConditionTemplate>) templates)
        {
          Guid guid = template != null ? template.Id : throw new ArgumentException("Enhanced Template passed is NULL or Template Id is NULL.");
          dbQueryBuilder.IfNotExists(table, new DbValueList()
          {
            {
              "Id",
              (object) SQL.Encode((object) template.Id)
            }
          });
          dbQueryBuilder.Begin();
          dbQueryBuilder.RaiseError("TemplateNotFound|" + template.Id.ToString());
          dbQueryBuilder.AppendLine("return;");
          dbQueryBuilder.End();
          dbQueryBuilder.AppendLine(string.Format("else if exists (select 1 from {0} where ConditionType = {1} and Title = {2} and Id != '{3}')", (object) table.Name, (object) SQL.Encode((object) template.ConditionType), (object) SQL.Encode((object) template.Title), (object) template.Id));
          dbQueryBuilder.Begin();
          dbQueryBuilder.RaiseError("TemplateWithTypeExists|" + string.Join(",", (IEnumerable<string>) new List<string>()
          {
            template.Title ?? "",
            template.ConditionType ?? ""
          }));
          dbQueryBuilder.AppendLine("return;");
          dbQueryBuilder.End();
          dbQueryBuilder.Else();
          dbQueryBuilder.Begin();
          dbQueryBuilder.Update(table, EnhancedConditionTemplateAccessor.TemplateRowGeneratorForUpdate(template), new DbValue("Id", (object) SQL.Encode((object) template.Id)));
          dbQueryBuilder.End();
        }
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        if (string.Equals(ex.Message, "TemplateNotFound", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("TemplateNotFound", StringComparison.CurrentCultureIgnoreCase) >= 0)
          throw new ObjectNotFoundException("A template with the given id does not exist for the user.", ObjectType.ConditionTemplate, (object) ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>().LastOrDefault<string>());
        if (string.Equals(ex.Message, "TemplateWithTypeExists", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("TemplateWithTypeExists", StringComparison.CurrentCultureIgnoreCase) >= 0)
        {
          List<string> list = ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>();
          throw new DuplicateObjectException("The template title / condition type combinations already exist.", ObjectType.ConditionTemplate, (object) new Dictionary<string, string>()
          {
            {
              list.FirstOrDefault<string>(),
              list.LastOrDefault<string>()
            }
          });
        }
        throw;
      }
    }

    public static void DeleteEnhancedConditionTemplates(IList<Guid> templateIds)
    {
      try
      {
        string[] strArray = templateIds != null && templateIds.Count != 0 ? new string[templateIds.Count] : throw new ArgumentException("Enhanced Templates passed is NULL or Empty");
        int num = 0;
        foreach (Guid templateId in (IEnumerable<Guid>) templateIds)
          strArray[num++] = SQL.Encode((object) templateId);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionSetTemplates"), new DbValue("TemplateId", (object) strArray));
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new DbValue("Id", (object) strArray));
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
    }

    public static void DeleteEnhancedConditionTemplate(Guid templateId)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionSetTemplates"), new DbValue("TemplateId", (object) SQL.Encode((object) templateId)));
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new DbValue("Id", (object) SQL.Encode((object) templateId)));
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
    }

    public static List<EnhancedConditionTemplate> GetEnhancedConditionTemplatesByConditionTypeIds(
      List<Guid> conditionTypeIds)
    {
      try
      {
        if (conditionTypeIds == null || conditionTypeIds.Count<Guid>() <= 0)
          throw new ArgumentException("Must provide at least one condition type id");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbValueList dbValueList = new DbValueList()
        {
          new DbValueList()
          {
            (DbValue) new DbFilterValue("Id", (object) conditionTypeIds.Select<Guid, string>((System.Func<Guid, string>) (x => x.ToString())).ToArray<string>())
          }
        };
        List<EnhancedConditionTemplate> conditionTypeIds1 = new List<EnhancedConditionTemplate>();
        dbQueryBuilder.AppendLine("SELECT et.Data FROM EnhancedConditionTemplates et\r\n                                INNER JOIN  EnhancedConditionTypes ec ON et.ConditionType=ec.Title\r\n                                WHERE ec.Id IN (" + string.Join(",", conditionTypeIds.Select<Guid, string>((System.Func<Guid, string>) (x => string.Format("'{0}'", (object) x)))) + ")");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection == null || dataRowCollection.Count == 0)
          return conditionTypeIds1;
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          EnhancedConditionTemplate conditionTemplate = EnhancedConditionTemplateAccessor.TemplateModelGeneratorFor(row);
          conditionTypeIds1.Add(conditionTemplate);
        }
        return conditionTypeIds1;
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
      return (List<EnhancedConditionTemplate>) null;
    }

    public static string ToJson(EnhancedConditionTemplate obj)
    {
      return JsonConvert.SerializeObject((object) obj, Formatting.None, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = (IList<JsonConverter>) new List<JsonConverter>()
        {
          (JsonConverter) new StringEnumConverter()
        }
      });
    }

    public static EnhancedConditionTemplate ToObject(string serializedObject)
    {
      return JsonConvert.DeserializeObject<EnhancedConditionTemplate>(serializedObject);
    }

    public static DataRowCollection GetEnhancedConditionTemplateFromConditionNameAndType(
      string conditionType,
      string title)
    {
      try
      {
        DbValueList keys = new DbValueList();
        keys.Add("ConditionType", (object) conditionType);
        keys.Add("Title", (object) title);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTemplateAccessor._tableName), new string[1]
        {
          "Data"
        }, keys);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection == null || dataRowCollection.Count == 0 ? (DataRowCollection) null : dataRowCollection;
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTemplateAccessor._className, ex);
      }
      return (DataRowCollection) null;
    }
  }
}
