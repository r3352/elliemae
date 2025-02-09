// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.BaseDbQueryBuilder
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public abstract class BaseDbQueryBuilder
  {
    private const string className = "BaseDbQueryBuilder";
    protected StringBuilder sql = new StringBuilder();
    protected string dbConnectionString;
    private readonly DbServerType dbServerType;
    private static bool? _sqlReadHAEnabled;

    protected BaseDbQueryBuilder(DbServerType dbServerType, string dbConnectionString)
    {
      this.dbServerType = dbServerType;
      this.dbConnectionString = dbConnectionString;
    }

    public abstract void AppendStoredProcedureCall(string spName, IEnumerable<string> values);

    public abstract string Variable(string baseName);

    public abstract void InsertInto(
      DbTableInfo table,
      DbValueList values,
      bool withSemicolon = true,
      bool returnInsertedId = false);

    public abstract void InsertInto(DbTableInfo table, List<DbValueList> values, DbVersion dbVer);

    public abstract void Update(DbTableInfo table, DbValueList values, DbValueList keys);

    public abstract void SelectFrom(DbTableInfo table, string[] columnNames, DbValueList keys);

    protected abstract void appendWhereClause(
      DbTableInfo table,
      DbValueList keys,
      bool withsemicolon = true);

    public abstract void RaiseError(string description);

    public abstract void AppendTake(int toTake, string query);

    public int Length => this.sql.Length;

    public virtual void Reset() => this.sql = new StringBuilder();

    public virtual void AppendFormat(string format, params object[] args)
    {
      this.sql.Append(string.Format(format, args));
    }

    public virtual void AppendLineFormat(string format, params object[] args)
    {
      this.sql.AppendLine(string.Format(format, args));
    }

    public virtual void Append(string text) => this.sql.Append(text);

    public virtual void AppendLine(string text) => this.sql.Append(text + Environment.NewLine);

    public virtual void Remove(int startIndex, int length) => this.sql.Remove(startIndex, length);

    public virtual void Where(DbValueList values)
    {
      for (int index = 0; index < values.Count; ++index)
      {
        this.Append(index == 0 ? "where " : "and ");
        DbFilterValue dbFilterValue = (DbFilterValue) values[index];
        DbTableInfo table = dbFilterValue.Table;
        string dbColumnName = dbFilterValue.DbColumnName;
        string columnName = dbFilterValue.ColumnName;
        List<string> stringList;
        if (!(dbFilterValue.Value is List<string>))
          stringList = new List<string>()
          {
            (string) dbFilterValue.Value
          };
        else
          stringList = (List<string>) dbFilterValue.Value;
        List<string> values1 = new List<string>();
        foreach (string filterValue in stringList)
          values1.Add(dbFilterValue.Encode(table[dbColumnName], (object) filterValue));
        this.AppendLine(columnName + " in (" + string.Join(", ", (IEnumerable<string>) values1) + ")");
      }
    }

    public virtual int Count(string selection)
    {
      this.AppendLine("with TempView as");
      this.AppendLine("(");
      this.AppendLine(selection);
      this.AppendLine(")");
      this.AppendLine("select count(*) from TempView");
      return Convert.ToInt32(this.ExecuteScalar());
    }

    public virtual string OrderBy(List<SortColumn> sortColumnList, bool forUseInRowNumber = false)
    {
      if (sortColumnList == null || sortColumnList.Count == 0)
        return "order by (select null)";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("order by ");
      string[] strArray = new string[sortColumnList.Count];
      for (int index = 0; index < sortColumnList.Count; ++index)
        strArray[index] = sortColumnList[index].Name + " " + (sortColumnList[index].SortOrder == SortOrder.Ascending ? "ASC" : "DESC");
      stringBuilder.Append(string.Join(", ", strArray));
      if (!forUseInRowNumber)
        this.sql.Append((object) stringBuilder);
      return stringBuilder.ToString();
    }

    public virtual DataTable GetPage(
      string selection,
      int pageSize,
      int pageNumber,
      List<SortColumn> sortColumnList)
    {
      this.AppendLine("with TempView as");
      this.AppendLine("(");
      this.AppendLine(selection);
      this.AppendLine("),");
      this.AppendLine("TempViewNumbered as");
      this.AppendLine("(");
      this.AppendLine(string.Format("select *, row_number() over ({0}) as RowNumber from TempView", (object) this.OrderBy(sortColumnList, true)));
      this.AppendLine(")");
      this.AppendLine("select * from TempViewNumbered");
      if (pageSize != 0)
        this.AppendLine("where RowNumber between " + (object) (pageSize * pageNumber + 1) + " and " + (object) ((pageNumber + 1) * pageSize));
      return this.ExecuteTableQuery();
    }

    public virtual DataTable GetPaginatedRecords(
      string selection,
      int startRecordNumber,
      int endRecordNumber,
      List<SortColumn> sortColumnList)
    {
      this.AppendLine("with TempView as");
      this.AppendLine("(");
      this.AppendLine(selection);
      this.AppendLine("),");
      return this.ExecutePaginatedRecords(startRecordNumber, endRecordNumber, sortColumnList);
    }

    public virtual DataTable GetCTEPaginatedRecords(
      string cte,
      int startRecordNumber,
      int endRecordNumber,
      List<SortColumn> sortColumnList)
    {
      this.AppendLine(cte + ",");
      return this.ExecutePaginatedRecords(startRecordNumber, endRecordNumber, sortColumnList);
    }

    private DataTable ExecutePaginatedRecords(
      int startRecordNumber,
      int endRecordNumber,
      List<SortColumn> sortColumnList)
    {
      this.AppendLine("TempViewNumbered as");
      this.AppendLine("(");
      this.AppendLine(string.Format("select *, row_number() over ({0}) as RowNumber from TempView", (object) this.OrderBy(sortColumnList, true)));
      this.AppendLine(")");
      this.AppendLine("select * from TempViewNumbered cross join (SELECT count(1) as TotalRowCount FROM TempViewNumbered) AS TotalRecords");
      this.AppendLine("where RowNumber between " + (object) startRecordNumber + " and " + (object) endRecordNumber);
      return this.ExecuteTableQuery();
    }

    public virtual void Update(DbTableInfo table, DbValueList values, DbValue key)
    {
      this.Update(table, values, new DbValueList(new DbValue[1]
      {
        key
      }));
    }

    public abstract void Upsert(DbTableInfo table, DbValueList values, DbValue key);

    public abstract void Upsert(DbTableInfo table, DbValueList values, DbValueList key);

    public abstract void Upsert(
      DbTableInfo table,
      DbValueList insertValues,
      DbValueList updateValues,
      DbValueList keys);

    public virtual void SelectFrom(DbTableInfo table)
    {
      this.SelectFrom(table, (string[]) null, new DbValueList());
    }

    public virtual void SelectFrom(DbTableInfo table, string[] columnNames)
    {
      this.SelectFrom(table, columnNames, new DbValueList());
    }

    public virtual void SelectFrom(DbTableInfo table, DbValue key)
    {
      this.SelectFrom(table, (string[]) null, new DbValueList(new DbValue[1]
      {
        key
      }));
    }

    public virtual void SelectFrom(DbTableInfo table, string[] columnNames, DbValue key)
    {
      this.SelectFrom(table, columnNames, new DbValueList(new DbValue[1]
      {
        key
      }));
    }

    public virtual void SelectFrom(DbTableInfo table, DbValueList keys)
    {
      this.SelectFrom(table, (string[]) null, keys);
    }

    public virtual void DeleteFrom(DbTableInfo table) => this.DeleteFrom(table, new DbValueList());

    public virtual void DeleteFrom(DbTableInfo table, DbValue key)
    {
      this.DeleteFrom(table, new DbValueList(new DbValue[1]
      {
        key
      }));
    }

    public virtual void DeleteFrom(DbTableInfo table, DbValueList keys)
    {
      this.AppendLine("delete from " + table.Name);
      this.appendWhereClause(table, keys);
    }

    public override string ToString() => this.sql.ToString();

    public virtual void Replace(string oldString, string newString)
    {
      this.sql = this.sql.Replace(oldString, newString);
    }

    public virtual DbDataReader ExecuteReaderQuery()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return (DbDataReader) dbAccessManager.ExecuteReaderQuery(this.ToString(), DbTransactionType.Default);
    }

    public virtual DataTable ExecuteTableQuery()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteTableQuery(this.ToString());
    }

    public virtual DataTable ExecuteTableQuery(DbTransactionType transactionType)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteTableQuery(this.ToString(), transactionType);
    }

    public virtual DataTable ExecuteTableQuery(DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteTableQuery(this.ToString(), parameters);
    }

    public virtual DataTable ExecuteTableQuery(TimeSpan timeout)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteTableQuery(this.ToString(), timeout);
    }

    public virtual DataRow ExecuteRowQuery()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteRowQuery(this.ToString());
    }

    public virtual DataRow ExecuteRowQuery(DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
      {
        using (IDbCommand dbCommand = dbAccessManager.GetDbCommand(this.ToString(), parameters))
          return dbAccessManager.ExecuteRowQuery(dbCommand);
      }
    }

    public virtual DataRow ExecuteRowQuery(DbTransactionType transType)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteRowQuery(this.ToString(), transType);
    }

    public virtual DataSet ExecuteSetQuery(
      TimeSpan timeout,
      DbTransactionType transType,
      DbCommandParameter[] parameters = null)
    {
      try
      {
        using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        {
          IDbCommand dbCommand = dbAccessManager.GetDbCommand(this.ToString(), parameters);
          return dbAccessManager.ExecuteSetQuery(dbCommand, timeout, transType);
        }
      }
      catch (Exception ex)
      {
        if (this.dbConnectionString != null && string.Compare(this.dbConnectionString, this.dbConnectionString, true) != 0)
        {
          if (this.sqlReadHAEnabled)
          {
            try
            {
              string message = "Retry with master DB due to error: " + ex.Message;
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteWarningI(nameof (BaseDbQueryBuilder), message);
            }
            catch
            {
            }
            using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
              return dbAccessManager.ExecuteSetQuery(this.ToString(), timeout, transType);
          }
        }
        throw ex;
      }
    }

    public virtual DataSet ExecuteSetQuery(
      DbTransactionType transType,
      DbCommandParameter[] parameters = null)
    {
      return this.ExecuteSetQuery(ServerGlobals.SQLTimeout, transType, parameters);
    }

    public virtual DataRowCollection Execute()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteQuery(this.ToString());
    }

    public virtual DataRowCollection Execute(DbCommandParameter parameter)
    {
      return this.Execute(parameter.ToArray());
    }

    public virtual DataRowCollection Execute(DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteQuery(this.ToString(), parameters, DbTransactionType.Default);
    }

    public virtual DataRowCollection Execute(DbTransactionType transType)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteQuery(this.ToString(), transType);
    }

    public virtual DataRowCollection Execute(
      DbTransactionType transType,
      DbCommandParameter parameter)
    {
      return this.Execute(transType, parameter.ToArray());
    }

    public virtual DataRowCollection Execute(
      DbTransactionType transType,
      DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteQuery(this.ToString(), parameters, transType);
    }

    public virtual DataRowCollection Execute(TimeSpan timeout)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteQuery(this.ToString());
    }

    public virtual void ExecuteNonQuery(DbCommandParameter parameter)
    {
      this.ExecuteNonQuery(parameter.ToArray());
    }

    public virtual void ExecuteNonQuery(DbCommandParameter[] parameters)
    {
      this.ExecuteNonQuery(DbTransactionType.Default, parameters);
    }

    public virtual void ExecuteNonQuery(
      DbTransactionType transType,
      DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        dbAccessManager.ExecuteQuery(this.ToString(), parameters, transType);
    }

    public virtual DataRowCollection ExecuteStoredProc(
      DbTransactionType transType,
      DbValueList parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteStoredProc(this.ToString(), transType, parameters);
    }

    public virtual void ExecuteNonQuery()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        dbAccessManager.ExecuteNonQuery(this.ToString());
    }

    public virtual void ExecuteNonQuery(DbTransactionType transType)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        dbAccessManager.ExecuteNonQuery(this.ToString(), transType);
    }

    public virtual void ExecuteNonQuery(TimeSpan timeout, DbTransactionType transType)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        dbAccessManager.ExecuteNonQuery(this.ToString(), timeout, transType);
    }

    public virtual object ExecuteScalar()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteScalar(this.ToString());
    }

    public virtual object ExecuteScalar(DbCommandParameter parameter)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteScalar(this.ToString(), parameter.ToArray());
    }

    public virtual object ExecuteScalar(DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteScalar(this.ToString(), parameters);
    }

    public virtual object ExecuteScalar(DbTransactionType transType)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteScalar(this.ToString(), transType);
    }

    public virtual object ExecuteScalar(TimeSpan timeout)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteScalar(this.ToString());
    }

    private bool sqlReadHAEnabled
    {
      get
      {
        if (!BaseDbQueryBuilder._sqlReadHAEnabled.HasValue)
        {
          try
          {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
            {
              if (registryKey != null)
              {
                if (string.Concat(registryKey.GetValue("SqlReadHA")).Trim() == "0")
                  BaseDbQueryBuilder._sqlReadHAEnabled = new bool?(false);
              }
            }
          }
          catch
          {
          }
          if (!BaseDbQueryBuilder._sqlReadHAEnabled.HasValue)
            BaseDbQueryBuilder._sqlReadHAEnabled = new bool?(true);
        }
        bool? sqlReadHaEnabled = BaseDbQueryBuilder._sqlReadHAEnabled;
        bool flag = false;
        return !(sqlReadHaEnabled.GetValueOrDefault() == flag & sqlReadHaEnabled.HasValue);
      }
    }

    public virtual DataSet ExecuteSetQuery()
    {
      try
      {
        using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
          return dbAccessManager.ExecuteSetQuery(this.ToString());
      }
      catch (Exception ex)
      {
        if (this.dbConnectionString != null && string.Compare(this.dbConnectionString, this.dbConnectionString, true) != 0)
        {
          if (this.sqlReadHAEnabled)
          {
            try
            {
              string message = "Retry with master DB due to error: " + ex.Message;
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteWarningI(nameof (BaseDbQueryBuilder), message);
            }
            catch
            {
            }
            using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
              return dbAccessManager.ExecuteSetQuery(this.ToString());
          }
        }
        throw ex;
      }
    }

    public virtual void DoBulkCopy(
      string tableName,
      DataTable tableRows,
      Dictionary<string, string> columnMapping = null)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        dbAccessManager.DoBulkCopy(tableName, tableRows, columnMapping);
    }

    public virtual object ExecuteNonQueryWithRowCount()
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
        return dbAccessManager.ExecuteNonQueryWithRowCount(this.ToString());
    }

    public virtual object ExecuteNonQueryWithRowCount(DbCommandParameter[] parameters)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager(this.dbConnectionString, this.dbServerType))
      {
        using (IDbCommand dbCommand = dbAccessManager.GetDbCommand(this.ToString(), parameters))
          return dbAccessManager.ExecuteNonQueryWithRowCount(dbCommand);
      }
    }
  }
}
