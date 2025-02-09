// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EnhancedConditionViewAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.EnhancedCondition;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class EnhancedConditionViewAccessor
  {
    private static readonly string _tableName = "EnhancedConditionView";

    public static IList<EnhancedConditionViewSummary> GetViewSummaryList(string userId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionViewAccessor._tableName), new string[2]
      {
        "Id",
        "Name"
      }, new DbValue("UserId", (object) userId));
      dbQueryBuilder.OrderBy(new List<SortColumn>()
      {
        new SortColumn("Name", SortOrder.Ascending)
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      List<EnhancedConditionViewSummary> viewSummaryList = new List<EnhancedConditionViewSummary>();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return (IList<EnhancedConditionViewSummary>) viewSummaryList;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        EnhancedConditionViewSummary conditionViewSummary = new EnhancedConditionViewSummary()
        {
          Id = new Guid(dataRow["Id"].ToString()),
          Name = dataRow["Name"].ToString()
        };
        viewSummaryList.Add(conditionViewSummary);
      }
      return (IList<EnhancedConditionViewSummary>) viewSummaryList;
    }

    public static EnhancedConditionView GetView(Guid viewId, string userId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionViewAccessor._tableName), new DbValueList()
      {
        {
          "Id",
          (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) viewId)
        },
        {
          "UserId",
          (object) userId
        }
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection == null || dataRowCollection.Count <= 0 ? (EnhancedConditionView) null : EnhancedConditionViewAccessor.ToObject(dataRowCollection[0]["Data"].ToString());
    }

    public static EnhancedConditionView CreateView(
      EnhancedConditionView conditionView,
      string userId)
    {
      try
      {
        if (conditionView == null)
          throw new ArgumentNullException(nameof (conditionView), "conditionView cannot be null.");
        if (string.IsNullOrWhiteSpace(userId))
          throw new ArgumentNullException(nameof (userId), "userId cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(conditionView.Name))
          throw new ArgumentException("Name", "A valid 'Name' is required.");
        conditionView.Id = Guid.NewGuid();
        conditionView.CreatedDate = conditionView.LastModifiedDate = DateTimeOffset.UtcNow;
        DbValueList values = new DbValueList()
        {
          {
            "Id",
            (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) conditionView.Id)
          },
          {
            "Name",
            (object) conditionView.Name
          },
          {
            "UserId",
            (object) userId
          },
          {
            "CreatedDate",
            (object) conditionView.CreatedDate.ToString()
          },
          {
            "LastModifiedDate",
            (object) conditionView.LastModifiedDate.ToString()
          },
          {
            "Data",
            (object) EnhancedConditionViewAccessor.ToJson(conditionView)
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionViewAccessor._tableName);
        dbQueryBuilder.IfExists(table, new DbValueList()
        {
          {
            "UserId",
            (object) userId
          },
          {
            "Name",
            (object) conditionView.Name
          }
        });
        dbQueryBuilder.Begin();
        dbQueryBuilder.RaiseError("ViewWithNameExists");
        dbQueryBuilder.End();
        dbQueryBuilder.Else();
        dbQueryBuilder.Begin();
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.SelectFrom(table, new DbValue("Id", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) conditionView.Id)));
        dbQueryBuilder.End();
        DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
        return dataRow == null ? (EnhancedConditionView) null : EnhancedConditionViewAccessor.ToObject(dataRow["Data"].ToString());
      }
      catch (Exception ex)
      {
        if (string.Equals(ex.Message, "ViewWithNameExists", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && string.Equals(ex.InnerException.Message, "ViewWithNameExists", StringComparison.CurrentCultureIgnoreCase))
          throw new DuplicateObjectException("A view with the same name already exists for the user.", ObjectType.ConditionView, (object) conditionView.Name);
        throw;
      }
    }

    public static EnhancedConditionView UpdateView(
      EnhancedConditionView conditionView,
      string userId)
    {
      try
      {
        if (conditionView == null || Guid.Empty.Equals(conditionView.Id))
          throw new ArgumentNullException(nameof (conditionView), "conditionView cannot be null.");
        if (Guid.Empty.Equals(conditionView.Id))
          throw new ArgumentException("Id cannot be null or empty.", "Id");
        conditionView.LastModifiedDate = DateTimeOffset.UtcNow;
        DbValueList values = new DbValueList()
        {
          {
            "Id",
            (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) conditionView.Id)
          },
          {
            "Name",
            (object) conditionView.Name
          },
          {
            "Data",
            (object) EnhancedConditionViewAccessor.ToJson(conditionView)
          },
          {
            "LastModifiedDate",
            (object) conditionView.LastModifiedDate.ToString()
          }
        };
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionViewAccessor._tableName);
        dbQueryBuilder.IfNotExists(table, new DbValueList()
        {
          {
            "Id",
            (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) conditionView.Id)
          },
          {
            "UserId",
            (object) userId
          }
        });
        dbQueryBuilder.Begin();
        dbQueryBuilder.RaiseError("ViewNotFound");
        dbQueryBuilder.End();
        dbQueryBuilder.AppendLine(string.Format("else if exists (select 1 from {0} where UserId = '{1}' and Name = '{2}' and Id != '{3}')", (object) table.Name, (object) userId, (object) conditionView.Name, (object) conditionView.Id));
        dbQueryBuilder.Begin();
        dbQueryBuilder.RaiseError("ViewWithNameExists");
        dbQueryBuilder.End();
        dbQueryBuilder.Else();
        dbQueryBuilder.Begin();
        dbQueryBuilder.Update(table, values, new DbValueList()
        {
          {
            "Id",
            (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) conditionView.Id)
          },
          {
            "UserId",
            (object) userId
          }
        });
        dbQueryBuilder.SelectFrom(table, new DbValueList()
        {
          {
            "Id",
            (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) conditionView.Id)
          },
          {
            "UserId",
            (object) userId
          }
        });
        dbQueryBuilder.End();
        DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
        return dataRow == null ? (EnhancedConditionView) null : EnhancedConditionViewAccessor.ToObject(dataRow["Data"].ToString());
      }
      catch (Exception ex)
      {
        if (string.Equals(ex.Message, "ViewNotFound", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && string.Equals(ex.InnerException.Message, "ViewNotFound", StringComparison.CurrentCultureIgnoreCase))
          throw new ObjectNotFoundException("A view with the given Id does not exist for the user.", ObjectType.ConditionView, (object) conditionView.Id);
        if (string.Equals(ex.Message, "ViewWithNameExists", StringComparison.CurrentCultureIgnoreCase) || ex.InnerException != null && string.Equals(ex.InnerException.Message, "ViewWithNameExists", StringComparison.CurrentCultureIgnoreCase))
          throw new DuplicateObjectException("A view with the same name already exists for the user.", ObjectType.ConditionView, (object) conditionView.Name);
        throw;
      }
    }

    public static void DeleteView(Guid viewId, string userId)
    {
      if (Guid.Empty.Equals(viewId))
        throw new ArgumentException("viewId cannot be null or empty.", nameof (viewId));
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EnhancedConditionViewAccessor._tableName);
      dbQueryBuilder.DeleteFrom(table, new DbValueList()
      {
        {
          "Id",
          (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) viewId)
        },
        {
          "UserId",
          (object) userId
        }
      });
      if ((int) dbQueryBuilder.ExecuteNonQueryWithRowCount() <= 0)
        throw new ObjectNotFoundException("A view with the given Id does not exist for the user.", ObjectType.ConditionView, (object) viewId);
    }

    private static string ToJson(EnhancedConditionView view)
    {
      return JsonConvert.SerializeObject((object) view, Formatting.None);
    }

    private static EnhancedConditionView ToObject(string json)
    {
      return JsonConvert.DeserializeObject<EnhancedConditionView>(json);
    }
  }
}
