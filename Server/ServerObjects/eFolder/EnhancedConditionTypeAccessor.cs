// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EnhancedConditionTypeAccessor
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
  public sealed class EnhancedConditionTypeAccessor
  {
    private static readonly string _tableName = "EnhancedConditionTypes";

    public static (IList<EnhancedConditionType> ConditionTypes, int TotalCount) GetEnhancedConditionTypes(
      bool activeOnly,
      IEnumerable<string> conditionTypes,
      string view = "summary�",
      int start = 0,
      int limit = 0)
    {
      DbValueList keys = new DbValueList();
      if (activeOnly)
        keys.Add(new DbValueList()
        {
          (DbValue) new DbFilterValue("Active", (object) Convert.ToByte(activeOnly))
        });
      if (conditionTypes != null && conditionTypes.Any<string>())
        keys.Add("Title", (object) conditionTypes.ToArray<string>());
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      bool flag1 = view.Equals("summary", StringComparison.CurrentCultureIgnoreCase);
      if (flag1)
      {
        string[] columnNames = new string[3]
        {
          "id",
          "title",
          "active"
        };
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), columnNames, keys);
      }
      else
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), new string[1]
        {
          "Data"
        }, keys);
      bool flag2 = start > 0 || limit > 0;
      DataTable dataTable;
      if (!flag2)
      {
        dataTable = dbQueryBuilder.ExecuteTableQuery(DbTransactionType.None);
      }
      else
      {
        int startRecordNumber = start <= 0 ? 1 : start + 1;
        int endRecordNumber = limit <= 0 ? int.MaxValue : startRecordNumber + limit - 1;
        dataTable = new EllieMae.EMLite.Server.DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), startRecordNumber, endRecordNumber, (List<SortColumn>) null);
      }
      List<EnhancedConditionType> enhancedConditionTypeList = new List<EnhancedConditionType>();
      if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count <= 0)
        return ((IList<EnhancedConditionType>) enhancedConditionTypeList, 0);
      int result = !flag2 || !int.TryParse(dataTable.Rows[0]["TotalRowCount"].ToString(), out result) ? dataTable.Rows.Count : result;
      if (flag1)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          enhancedConditionTypeList.Add(EnhancedConditionTypeAccessor.DeserializeSummaryModel(row));
      }
      else
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          enhancedConditionTypeList.Add(EnhancedConditionTypeAccessor.DeserializeModel((string) row["Data"]));
      }
      return ((IList<EnhancedConditionType>) enhancedConditionTypeList, result);
    }

    public static EnhancedConditionType GetEnhancedConditionTypeById(Guid enhancedConditionTypeId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), new string[1]
      {
        "Data"
      }, new DbValue("Id", (object) enhancedConditionTypeId.ToString()));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection.Count == 0 ? (EnhancedConditionType) null : EnhancedConditionTypeAccessor.DeserializeModel((string) dataRowCollection[0]["Data"]);
    }

    public static List<EnhancedConditionType> GetEnhancedConditionTypesByIds(
      List<Guid> enhancedConditionTypeIds)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValueList keys = new DbValueList();
      if (enhancedConditionTypeIds == null || !enhancedConditionTypeIds.Any<Guid>())
        throw new ArgumentException("Must provide at least one enhanced condition type id.");
      keys.Add(new DbValueList()
      {
        (DbValue) new DbFilterValue("Id", (object) enhancedConditionTypeIds.Select<Guid, string>((System.Func<Guid, string>) (x => x.ToString())).ToArray<string>())
      });
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), new string[1]
      {
        "Data"
      }, keys);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      List<EnhancedConditionType> conditionTypesByIds = new List<EnhancedConditionType>();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return conditionTypesByIds;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        conditionTypesByIds.Add(EnhancedConditionTypeAccessor.DeserializeModel((string) dataRow["Data"]));
      return conditionTypesByIds;
    }

    public static EnhancedConditionType GetEnhancedConditionTypeByTitle(string title)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), new string[1]
      {
        "Data"
      }, new DbValue("Title", (object) title));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection.Count == 0 ? (EnhancedConditionType) null : EnhancedConditionTypeAccessor.DeserializeModel((string) dataRowCollection[0]["Data"]);
    }

    public static List<string> GetEnhancedConditionTypeList(bool active)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select title ");
      dbQueryBuilder.AppendLine(" from EnhancedConditionTypes ");
      if (active)
        dbQueryBuilder.AppendLine(" where active = " + SQL.EncodeFlag(active));
      dbQueryBuilder.AppendLine(" order by title ");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery(DbTransactionType.None);
      List<string> conditionTypeList = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        conditionTypeList.Add(row["title"].ToString());
      return conditionTypeList;
    }

    public static List<EnhancedConditionType> CreateEnhancedConditionTypes(
      List<EnhancedConditionType> enhancedConditionTypes)
    {
      try
      {
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionTypes");
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        List<EnhancedConditionType> enhancedConditionTypes1 = new List<EnhancedConditionType>();
        if (enhancedConditionTypes == null || enhancedConditionTypes.Count <= 0)
          throw new ArgumentException("Must pass in EnhancedConditionTypes to Create EnhancedConditionType");
        foreach (EnhancedConditionType enhancedConditionType in enhancedConditionTypes)
        {
          enhancedConditionType.LastModifiedDate = DateTime.UtcNow;
          enhancedConditionType.CreatedDate = DateTime.UtcNow;
          enhancedConditionType.Id = Guid.NewGuid();
          enhancedConditionTypes1.Add(enhancedConditionType);
          DbValueList values = new DbValueList()
          {
            {
              "Id",
              (object) enhancedConditionType.Id.ToString()
            },
            {
              "Title",
              (object) enhancedConditionType.Title
            },
            {
              "Data",
              (object) EnhancedConditionTypeAccessor.SerializeModel(enhancedConditionType)
            },
            {
              "Active",
              (object) Convert.ToByte(enhancedConditionType.Active)
            },
            {
              "LastModifiedDate",
              (object) enhancedConditionType.LastModifiedDate
            },
            {
              "LastModifiedBy",
              (object) enhancedConditionType.LastModifiedBy
            },
            {
              "CreatedDate",
              (object) enhancedConditionType.CreatedDate
            },
            {
              "CreatedBy",
              (object) enhancedConditionType.CreatedBy
            }
          };
          dbQueryBuilder.IfExists(table, new DbValueList()
          {
            {
              "Title",
              (object) enhancedConditionType.Title
            }
          });
          dbQueryBuilder.Begin();
          dbQueryBuilder.RaiseError("TypeWithTitleExists|" + enhancedConditionType.Title);
          dbQueryBuilder.Append("return;");
          dbQueryBuilder.End();
          dbQueryBuilder.Else();
          dbQueryBuilder.Begin();
          dbQueryBuilder.InsertInto(table, values, true, false);
          dbQueryBuilder.End();
        }
        dbQueryBuilder.ExecuteNonQuery();
        return enhancedConditionTypes1;
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf("TypeWithTitleExists", StringComparison.CurrentCultureIgnoreCase) >= 0 || ex.InnerException != null && ex.InnerException.Message.IndexOf("TypeWithTitleExists", StringComparison.CurrentCultureIgnoreCase) >= 0)
          throw new DuplicateObjectException("A type with the same title already exists.", ObjectType.ConditionType, (object) string.Join(", ", ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>().LastOrDefault<string>()));
        throw;
      }
    }

    public static void UpdateEnhancedConditionTypes(
      List<EnhancedConditionType> enhancedConditionTypes,
      string userid)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionTypes");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (enhancedConditionTypes == null && !enhancedConditionTypes.Any<EnhancedConditionType>())
          throw new ArgumentException("Enhanced Types passed is NULL or Empty");
        foreach (EnhancedConditionType enhancedConditionType in enhancedConditionTypes)
        {
          enhancedConditionType.LastModifiedDate = DateTime.UtcNow;
          enhancedConditionType.LastModifiedBy = userid;
          DbValueList values = new DbValueList()
          {
            {
              "Title",
              (object) enhancedConditionType.Title
            },
            {
              "Data",
              (object) EnhancedConditionTypeAccessor.SerializeModel(enhancedConditionType)
            },
            {
              "Active",
              (object) Convert.ToByte(enhancedConditionType.Active)
            },
            {
              "LastModifiedDate",
              (object) enhancedConditionType.LastModifiedDate
            },
            {
              "LastModifiedBy",
              (object) enhancedConditionType.LastModifiedBy
            }
          };
          DbValue key = new DbValue("Id", (object) enhancedConditionType.Id.ToString());
          dbQueryBuilder.IfNotExists(table, new DbValueList()
          {
            {
              "Id",
              (object) SQL.Encode((object) enhancedConditionType.Id)
            }
          });
          dbQueryBuilder.Begin();
          dbQueryBuilder.RaiseError("TypeNotFound|" + (object) enhancedConditionType.Id);
          dbQueryBuilder.End();
          dbQueryBuilder.AppendLine(string.Format("else if exists (select 1 from {0} where Title = '{1}' and Id != '{2}')", (object) table.Name, (object) enhancedConditionType.Title, (object) enhancedConditionType.Id));
          dbQueryBuilder.Begin();
          dbQueryBuilder.RaiseError("TypeWithTitleExists|" + enhancedConditionType.Title);
          dbQueryBuilder.End();
          dbQueryBuilder.Else();
          dbQueryBuilder.Begin();
          dbQueryBuilder.Update(table, values, key);
          dbQueryBuilder.End();
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        if (string.Equals(ex.Message, "TypeNotFound", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("TypeNotFound", StringComparison.CurrentCultureIgnoreCase) >= 0)
          throw new ObjectNotFoundException("No type found for the given id.", ObjectType.ConditionType, (object) ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>().LastOrDefault<string>());
        if (string.Equals(ex.Message, "TypeWithTitleExists", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("TypeWithTitleExists", StringComparison.CurrentCultureIgnoreCase) >= 0)
          throw new DuplicateObjectException("Type with given title already exists.", ObjectType.ConditionType, (object) ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>().LastOrDefault<string>());
        throw;
      }
    }

    public static void DeleteConditionTypes(
      List<Guid> conditionsTypeIdsToDelete,
      Guid? reassignToTypeId,
      string userid)
    {
      EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionTypes");
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("EnhancedConditionTemplates");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        if (!conditionsTypeIdsToDelete.Any<Guid>())
          throw new ArgumentException("ConditionType Ids passed are NULL or Empty");
        dbQueryBuilder.Begin();
        if (reassignToTypeId.HasValue)
        {
          EnhancedConditionType conditionTypeById = EnhancedConditionTypeAccessor.GetEnhancedConditionTypeById(reassignToTypeId.Value);
          if (conditionTypeById == null)
            throw new ObjectNotFoundException("ConditionType with the given id does not exist.", ObjectType.ConditionType, (object) reassignToTypeId.ToString());
          List<EnhancedConditionTemplate> conditionTypeIds = EnhancedConditionTemplateAccessor.GetEnhancedConditionTemplatesByConditionTypeIds(conditionsTypeIdsToDelete);
          if (conditionTypeIds != null && conditionTypeIds.Any<EnhancedConditionTemplate>())
          {
            dbQueryBuilder.Declare("@TemplateTitle", "VARCHAR(64)");
            dbQueryBuilder.Declare("@ConditionTypeToReassign", "VARCHAR(64)");
            dbQueryBuilder.AppendLine("BEGIN TRY");
            foreach (EnhancedConditionTemplate template in conditionTypeIds)
            {
              string conditionType = template.ConditionType;
              dbQueryBuilder.AppendLine("SET @TemplateTitle='" + template.Title + "'");
              dbQueryBuilder.AppendLine("SET @ConditionTypeToReassign='" + conditionTypeById.Title + "'");
              template.LastModifiedBy = userid;
              template.ConditionType = conditionTypeById.Title;
              DbValueList values = EnhancedConditionTemplateAccessor.TemplateRowGeneratorForUpdate(template);
              dbQueryBuilder.Update(table, values, new DbValueList()
              {
                new DbValue("Id", (object) SQL.Encode((object) template.Id)),
                new DbValue("ConditionType", (object) conditionType)
              });
            }
            dbQueryBuilder.AppendLine("END TRY ");
            dbQueryBuilder.AppendLine("BEGIN CATCH ");
            dbQueryBuilder.Declare("@ErrorMessage", "VARCHAR(250)");
            dbQueryBuilder.AppendLine("Select @ErrorMessage='TemplateWithTypeExists|' + @TemplateTitle + ',' + @ConditionTypeToReassign");
            dbQueryBuilder.AppendLine("RAISERROR ( @errormessage,16,1)");
            dbQueryBuilder.AppendLine("return");
            dbQueryBuilder.AppendLine("END CATCH ");
          }
        }
        else
        {
          string str = string.Join(",", conditionsTypeIdsToDelete.Select<Guid, string>((System.Func<Guid, string>) (x => string.Format("'{0:D}'", (object) x))));
          dbQueryBuilder.AppendLine("DELETE FROM [EnhancedConditionSetTemplates]\r\n                                     FROM [EnhancedConditionSetTemplates] est\r\n                                     INNER JOIN [EnhancedConditionTemplates] et ON est.[TemplateID] = et.[Id]\r\n                                     INNER JOIN [EnhancedConditionTypes] ec ON et.[ConditionType] = ec.[Title]\r\n                                     WHERE ec.[Id] IN (" + str + ")");
          dbQueryBuilder.AppendLine("DELETE FROM [EnhancedConditionTemplates]\r\n                                    FROM EnhancedConditionTemplates et\r\n                                    INNER JOIN EnhancedConditionTypes ec ON et.ConditionType=ec.Title\r\n                                    WHERE ec.Id IN (" + str + ")");
        }
        dbQueryBuilder.AppendLine("BEGIN TRY");
        dbQueryBuilder.AppendLine("DELETE FROM [EnhancedConditionTypes]  WHERE Id IN (" + string.Join(",", conditionsTypeIdsToDelete.Select<Guid, string>((System.Func<Guid, string>) (x => string.Format("'{0}'", (object) x)))) + ")");
        dbQueryBuilder.AppendLine("END TRY ");
        dbQueryBuilder.AppendLine("BEGIN CATCH ");
        dbQueryBuilder.AppendLine("RAISERROR ('ConditionTypeModified',16,1)");
        dbQueryBuilder.AppendLine("return");
        dbQueryBuilder.AppendLine("END CATCH ");
        dbQueryBuilder.End();
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      }
      catch (Exception ex)
      {
        if (string.Equals(ex.Message, "TemplateWithTypeExists", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("TemplateWithTypeExists", StringComparison.CurrentCultureIgnoreCase) >= 0)
        {
          List<string> list = ((IEnumerable<string>) ((IEnumerable<string>) ex.InnerException.Message.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((System.Func<string, string>) (x => x.Trim())).ToList<string>().LastOrDefault<string>().ToString().Split(',')).ToList<string>();
          throw new DuplicateObjectException("The template title / condition type combination already exist.", ObjectType.ConditionTemplate, (object) new Dictionary<string, string>()
          {
            {
              list.FirstOrDefault<string>(),
              list.LastOrDefault<string>()
            }
          });
        }
        if (string.Equals(ex.Message, "ConditionTypeModified", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && ex.InnerException.Message.IndexOf("ConditionTypeModified", StringComparison.CurrentCultureIgnoreCase) >= 0)
          throw new ObjectModifiedException("Can not delete condition type , referenced records are modified.");
        throw;
      }
    }

    public static List<string> GetTrackingDefinitionsOfConditionType(string title)
    {
      try
      {
        DbValueList keys = new DbValueList();
        keys.Add("Title", (object) title);
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), new string[1]
        {
          "Data"
        }, keys);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection == null || dataRowCollection.Count == 0)
          return (List<string>) null;
        EnhancedConditionType enhancedConditionType = EnhancedConditionTypeAccessor.DeserializeModel((string) dataRowCollection[0]["Data"]);
        List<string> definitionsOfConditionType = new List<string>();
        foreach (TrackingTypeDefinition trackingDefinition in (IEnumerable<TrackingTypeDefinition>) enhancedConditionType.Definitions.TrackingDefinitions)
          definitionsOfConditionType.Add(trackingDefinition.Name);
        return definitionsOfConditionType;
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTypeAccessor._tableName, ex);
      }
      return (List<string>) null;
    }

    public static HashSet<string> GetOptionsFromConditionType(string attribute)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionTypeAccessor._tableName), new string[1]
        {
          "Data"
        });
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        HashSet<string> fromConditionType = new HashSet<string>();
        if (dataRowCollection == null || dataRowCollection.Count == 0)
          return (HashSet<string>) null;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          EnhancedConditionType enhancedConditionType = EnhancedConditionTypeAccessor.DeserializeModel((string) dataRow["Data"]);
          switch (attribute)
          {
            case "Prior To":
              if (enhancedConditionType != null && enhancedConditionType.Definitions != null && enhancedConditionType.Definitions.PriorToDefinitions != null)
              {
                using (IEnumerator<OptionTypeDefinition> enumerator = enhancedConditionType.Definitions.PriorToDefinitions.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    OptionTypeDefinition current = enumerator.Current;
                    fromConditionType.Add(current.Name);
                  }
                  continue;
                }
              }
              else
                continue;
            case "Category":
              if (enhancedConditionType != null && enhancedConditionType.Definitions != null && enhancedConditionType.Definitions.CategoryDefinitions != null)
              {
                using (IEnumerator<OptionTypeDefinition> enumerator = enhancedConditionType.Definitions.CategoryDefinitions.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    OptionTypeDefinition current = enumerator.Current;
                    fromConditionType.Add(current.Name);
                  }
                  continue;
                }
              }
              else
                continue;
            case "Source":
              if (enhancedConditionType != null && enhancedConditionType.Definitions != null && enhancedConditionType.Definitions.SourceDefinitions != null)
              {
                using (IEnumerator<OptionTypeDefinition> enumerator = enhancedConditionType.Definitions.SourceDefinitions.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    OptionTypeDefinition current = enumerator.Current;
                    fromConditionType.Add(current.Name);
                  }
                  continue;
                }
              }
              else
                continue;
            case "Recipient":
              if (enhancedConditionType != null && enhancedConditionType.Definitions != null && enhancedConditionType.Definitions.RecipientDefinitions != null)
              {
                using (IEnumerator<OptionTypeDefinition> enumerator = enhancedConditionType.Definitions.RecipientDefinitions.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    OptionTypeDefinition current = enumerator.Current;
                    fromConditionType.Add(current.Name);
                  }
                  continue;
                }
              }
              else
                continue;
            case "Tracking Option":
              if (enhancedConditionType != null && enhancedConditionType.Definitions != null && enhancedConditionType.Definitions.TrackingDefinitions != null)
              {
                using (IEnumerator<TrackingTypeDefinition> enumerator = enhancedConditionType.Definitions.TrackingDefinitions.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    TrackingTypeDefinition current = enumerator.Current;
                    fromConditionType.Add(current.Name);
                  }
                  continue;
                }
              }
              else
                continue;
            default:
              continue;
          }
        }
        return fromConditionType;
      }
      catch (Exception ex)
      {
        Err.Reraise(EnhancedConditionTypeAccessor._tableName, ex);
      }
      return (HashSet<string>) null;
    }

    public static string SerializeModel(EnhancedConditionType type)
    {
      return JsonConvert.SerializeObject((object) type, Formatting.None, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        Converters = (IList<JsonConverter>) new List<JsonConverter>()
        {
          (JsonConverter) new StringEnumConverter()
        }
      });
    }

    public static EnhancedConditionType DeserializeModel(string data)
    {
      return JsonConvert.DeserializeObject<EnhancedConditionType>(data);
    }

    public static EnhancedConditionType DeserializeSummaryModel(DataRow row)
    {
      return new EnhancedConditionType()
      {
        Id = (Guid) row["Id"],
        Title = (string) row["Title"],
        Active = (bool) row["Active"]
      };
    }
  }
}
