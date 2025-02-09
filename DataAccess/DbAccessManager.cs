// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbAccessManager
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess.Postgres;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbAccessManager : IDbAccessManager, IDisposable
  {
    private const string className = "DbAccessManager";
    private DbConnection conn;
    private static Hashtable tables = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static ReaderWriterLock tableLock = new ReaderWriterLock();
    private static ReaderWriterLockSlim tableLockSlim = (ReaderWriterLockSlim) null;
    private static Hashtable dbVersionMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static DbProviderFactory _dbProviderFactory;
    private DbServerType dbServerType;

    static DbAccessManager()
    {
      if (!SmartClientUtils.UseReaderWriterLockSlim)
        return;
      if (SmartClientUtils.LockSlimNoRecursion)
        DbAccessManager.tableLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
      else
        DbAccessManager.tableLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    }

    public DbAccessManager(string dbConnectionString, DbServerType dbServerType)
    {
      try
      {
        this.dbServerType = dbServerType;
        DbAccessManager._dbProviderFactory = DbServerTypeHelpers.GetFactory(dbServerType);
        this.conn = DbAccessManager._dbProviderFactory.CreateConnection();
        this.conn.ConnectionString = dbConnectionString;
        this.conn.Open();
      }
      catch (Exception ex)
      {
        string lower = (dbConnectionString ?? "").ToLower();
        bool flag = dbConnectionString.Contains("ApplicationIntent=ReadOnly");
        int length = lower.IndexOf("password");
        dbConnectionString = length <= 0 ? "" : "\r\n" + dbConnectionString.Substring(0, length);
        string message = "Error creating connection object: " + ex.Message;
        if (flag)
          message = "Error creating read-replica connection object: " + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message + dbConnectionString, nameof (DbAccessManager));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteExceptionI(nameof (DbAccessManager), ex);
        if (ServerGlobals.Err == null)
          return;
        ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException(message));
      }
    }

    public void Initialize(string connectionString)
    {
    }

    public IDbConnection Connection => (IDbConnection) this.conn;

    public IDbCommand GetDbCommand(string statement)
    {
      return this.GetDbCommand(statement, (DbCommandParameter[]) null);
    }

    public IDbCommand GetDbCommand(string sql, DbCommandParameter[] parameters)
    {
      DbCommand command = DbAccessManager._dbProviderFactory.CreateCommand();
      command.CommandText = sql;
      command.Connection = this.conn;
      DbAccessManager.AddCommandParameters((IDbCommand) command, (IEnumerable<DbCommandParameter>) parameters);
      if (this.dbServerType == DbServerType.Postgres)
        command.CommandText = PgQueryHelpers.QuoteNames(sql);
      return (IDbCommand) command;
    }

    public DataTable ExecuteTableQuery(string sql)
    {
      return this.ExecuteTableQuery(sql, DbTransactionType.Default);
    }

    public DataTable ExecuteTableQuery(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteTableQuery(this.GetDbCommand(sql), transactionType);
    }

    public DataTable ExecuteTableQuery(string sql, TimeSpan timeout)
    {
      return this.ExecuteTableQuery((IDbCommand) new SqlCommand(sql), timeout, DbTransactionType.Default);
    }

    public DataTable ExecuteTableQuery(string sql, DbCommandParameter[] parameters)
    {
      return this.ExecuteTableQuery(this.GetDbCommand(sql, parameters));
    }

    public DataRow ExecuteRowQuery(string sql)
    {
      return this.ExecuteRowQuery(sql, DbTransactionType.Default);
    }

    public DataRow ExecuteRowQuery(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteRowQuery(this.GetDbCommand(sql), transactionType);
    }

    public object ExecuteScalar(string sql) => this.ExecuteScalar(this.GetDbCommand(sql));

    public object ExecuteScalar(string sql, DbTransactionType transType)
    {
      return this.ExecuteScalar(this.GetDbCommand(sql), transType);
    }

    public object ExecuteScalar(string sql, TimeSpan timeout)
    {
      return this.ExecuteScalar(this.GetDbCommand(sql), timeout, DbTransactionType.Default);
    }

    public object ExecuteScalar(string sql, DbCommandParameter[] parameters)
    {
      return this.ExecuteScalar(sql, parameters, DbTransactionType.Default, ServerGlobals.SQLTimeout);
    }

    public object ExecuteScalar(
      string sql,
      DbCommandParameter[] parameters,
      DbTransactionType transactionType,
      TimeSpan timeout)
    {
      return this.ExecuteScalar(this.GetDbCommand(sql, parameters), timeout, transactionType);
    }

    public void ExecuteNonQuery(string sql) => this.ExecuteNonQuery(this.GetDbCommand(sql));

    public void ExecuteNonQuery(string sql, DbTransactionType transType)
    {
      this.ExecuteNonQuery(this.GetDbCommand(sql), transType);
    }

    public void ExecuteNonQuery(string sql, TimeSpan timeout, DbTransactionType transType)
    {
      this.ExecuteNonQuery(this.GetDbCommand(sql), timeout, transType);
    }

    public object ExecuteNonQueryWithRowCount(string sql)
    {
      return this.ExecuteNonQueryWithRowCount(this.GetDbCommand(sql));
    }

    public DataSet ExecuteSetQuery(string sql) => this.ExecuteSetQuery(this.GetDbCommand(sql));

    public DataSet ExecuteSetQuery(string sql, DbTransactionType transType)
    {
      return this.ExecuteSetQuery(this.GetDbCommand(sql), transType);
    }

    public DataSet ExecuteSetQuery(string sql, TimeSpan timeout, DbTransactionType transType)
    {
      return this.ExecuteSetQuery(this.GetDbCommand(sql), timeout, transType);
    }

    public DataSet ExecuteSetQuery(IDbCommand sqlCmd)
    {
      return this.ExecuteSetQuery(sqlCmd, DbTransactionType.Default);
    }

    public DataSet ExecuteSetQuery(IDbCommand sqlCmd, DbTransactionType transType)
    {
      return this.ExecuteSetQuery(sqlCmd, ServerGlobals.SQLTimeout, transType);
    }

    public DataSet ExecuteSetQuery(IDbCommand sqlCmd, TimeSpan timeout)
    {
      return this.ExecuteSetQuery(sqlCmd, timeout, DbTransactionType.Default);
    }

    public DataSet ExecuteSetQuery(
      IDbCommand sqlCmd,
      TimeSpan timeout,
      DbTransactionType transType)
    {
      string commandText = sqlCmd.CommandText;
      try
      {
        if (sqlCmd.CommandType == CommandType.StoredProcedure)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("Stored Procedure: ").AppendLine(sqlCmd.CommandText);
          foreach (DbParameter parameter in (IEnumerable) sqlCmd.Parameters)
            stringBuilder.AppendLine(string.Format("\t{0}: {1} ({2})", (object) parameter.ParameterName, (object) parameter.Value.ToString(), (object) parameter.Direction.ToString()));
          commandText = stringBuilder.ToString();
        }
        if (sqlCmd.CommandType == CommandType.StoredProcedure)
          this.logQuery(string.Format("Stored Procedure: {0}\r\nParameters:{1}", (object) sqlCmd.CommandText, (object) sqlCmd.Parameters));
        else
          this.logQuery(sqlCmd.CommandText);
        return (DataSet) new DbTransaction((IDbConnection) this.conn, transType).Execute((DbCommandInvoker) new DataSetCommandInvoker(sqlCmd, (IDbDataAdapter) DbAccessManager._dbProviderFactory.CreateDataAdapter()), timeout);
      }
      catch (Exception ex)
      {
        string message = "Error executing query: [" + commandText + "]\r\n" + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.Err != null)
          ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException("Error executing query: " + ex.Message, ex));
        return (DataSet) null;
      }
    }

    public IDataReader ExecuteReaderQuery(string sql, DbTransactionType transactionType = DbTransactionType.Default)
    {
      return this.ExecuteReaderQuery(this.GetDbCommand(sql), transactionType);
    }

    public IDataReader ExecuteReaderQuery(IDbCommand sqlCmd, DbTransactionType transactionType = DbTransactionType.Default)
    {
      return this.ExecuteReaderQuery(sqlCmd, ServerGlobals.SQLTimeout, transactionType);
    }

    public IDataReader ExecuteReaderQuery(IDbCommand sqlCmd, TimeSpan timeout)
    {
      return this.ExecuteReaderQuery(sqlCmd, timeout, DbTransactionType.Default);
    }

    public IDataReader ExecuteReaderQuery(
      IDbCommand sqlCmd,
      TimeSpan timeout,
      DbTransactionType transType)
    {
      try
      {
        this.logQuery(sqlCmd.CommandText);
        return (IDataReader) new DbTransaction((IDbConnection) this.conn, transType).Execute((DbCommandInvoker) new DataReaderCommandInvoker(sqlCmd), timeout);
      }
      catch (Exception ex)
      {
        string message = "Error running query: " + sqlCmd.CommandText + "\r\n" + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
        if (!ServerGlobals.Debug && ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.Err != null)
          ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException("Error executing query: " + ex.Message, ex));
        return (IDataReader) null;
      }
    }

    public DataTable ExecuteTableQuery(IDbCommand sqlCmd)
    {
      return this.ExecuteTableQuery(sqlCmd, DbTransactionType.Default);
    }

    public DataTable ExecuteTableQuery(IDbCommand sqlCmd, DbTransactionType transType)
    {
      DataSet dataSet = this.ExecuteSetQuery(sqlCmd, transType);
      if (dataSet == null)
        return (DataTable) null;
      return dataSet.Tables.Count == 0 ? (DataTable) null : dataSet.Tables[0];
    }

    public DataRow ExecuteRowQuery(IDbCommand sqlCmd)
    {
      return this.ExecuteRowQuery(sqlCmd, DbTransactionType.Default);
    }

    public DataRow ExecuteRowQuery(IDbCommand sqlCmd, DbTransactionType transType)
    {
      DataSet dataSet = this.ExecuteSetQuery(sqlCmd, transType);
      if (dataSet == null)
        return (DataRow) null;
      if (dataSet.Tables.Count == 0)
        return (DataRow) null;
      return dataSet.Tables[0].Rows.Count == 0 ? (DataRow) null : dataSet.Tables[0].Rows[0];
    }

    public DataTable ExecuteTableQuery(
      IDbCommand sqlCmd,
      TimeSpan timeout,
      DbTransactionType transType)
    {
      DataSet dataSet = this.ExecuteSetQuery(sqlCmd, timeout, transType);
      if (dataSet == null)
        return (DataTable) null;
      return dataSet.Tables.Count == 0 ? (DataTable) null : dataSet.Tables[0];
    }

    public DataRowCollection ExecuteStoredProc(
      string spName,
      DbTransactionType transType,
      DbValueList parameters)
    {
      IDbCommand dbCommand = this.GetDbCommand(spName.Trim());
      dbCommand.CommandType = CommandType.StoredProcedure;
      foreach (DbValue parameter1 in parameters)
      {
        IDbDataParameter parameter2 = dbCommand.CreateParameter();
        parameter2.ParameterName = parameter1.ColumnName;
        parameter2.Value = parameter1.Value;
        dbCommand.Parameters.Add((object) parameter2);
      }
      return this.ExecuteTableQuery(dbCommand, transType)?.Rows;
    }

    private static void AddCommandParameters(
      IDbCommand command,
      IEnumerable<DbCommandParameter> parameters)
    {
      if (parameters == null)
        return;
      foreach (DbCommandParameter parameter1 in parameters)
      {
        IDbDataParameter parameter2 = command.CreateParameter();
        parameter2.DbType = parameter1.DbType;
        parameter2.ParameterName = parameter1.ParameterName;
        parameter2.Value = parameter1.Value;
        command.Parameters.Add((object) parameter2);
      }
    }

    public DataRowCollection ExecuteQuery(string sql)
    {
      return this.ExecuteQuery(sql, (DbCommandParameter[]) null, DbTransactionType.Default);
    }

    public DataRowCollection ExecuteQuery(string sql, DbTransactionType transType)
    {
      return this.ExecuteQuery(sql, (DbCommandParameter[]) null, transType);
    }

    public DataRowCollection ExecuteQuery(string sql, TimeSpan timeout)
    {
      return this.ExecuteTableQuery((IDbCommand) new SqlCommand(sql), timeout, DbTransactionType.Default)?.Rows;
    }

    public DataRowCollection ExecuteQuery(
      string sql,
      DbCommandParameter[] dbCommandParameters = null,
      DbTransactionType transType = DbTransactionType.Default)
    {
      IDbCommand dbCommand = this.GetDbCommand(sql);
      dbCommand.CommandType = CommandType.Text;
      DbAccessManager.AddCommandParameters(dbCommand, (IEnumerable<DbCommandParameter>) dbCommandParameters);
      return this.ExecuteTableQuery(dbCommand, transType)?.Rows;
    }

    public void ExecuteNonQuery(IDbCommand sqlCmd)
    {
      this.ExecuteNonQuery(sqlCmd, DbTransactionType.Default);
    }

    public void ExecuteNonQuery(IDbCommand sqlCmd, DbTransactionType transType)
    {
      this.ExecuteNonQuery(sqlCmd, ServerGlobals.SQLTimeout, transType);
    }

    public void ExecuteNonQuery(IDbCommand sqlCmd, TimeSpan timeout, DbTransactionType transType)
    {
      try
      {
        this.logQuery(sqlCmd.CommandText);
        new DbTransaction((IDbConnection) this.conn, transType).Execute((DbCommandInvoker) new NonQueryCommandInvoker(sqlCmd), timeout);
      }
      catch (Exception ex)
      {
        string message = "Error executing query: [" + sqlCmd.CommandText + "]\r\n" + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.Err != null)
          ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException(message, ex));
        throw;
      }
    }

    public object ExecuteNonQueryWithRowCount(IDbCommand sqlCmd)
    {
      return this.ExecuteNonQueryWithRowCount(sqlCmd, DbTransactionType.Default);
    }

    public object ExecuteNonQueryWithRowCount(IDbCommand sqlCmd, DbTransactionType transType)
    {
      return this.ExecuteNonQueryWithRowCount(sqlCmd, ServerGlobals.SQLTimeout, transType);
    }

    public object ExecuteNonQueryWithRowCount(
      IDbCommand sqlCmd,
      TimeSpan timeout,
      DbTransactionType transType)
    {
      try
      {
        this.logQuery(sqlCmd.CommandText);
        return new DbTransaction((IDbConnection) this.conn, transType).Execute((DbCommandInvoker) new NonQueryCommandInvoker(sqlCmd), timeout);
      }
      catch (Exception ex)
      {
        string message = "Error executing query: [" + sqlCmd.CommandText + "]\r\n" + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.Err != null)
          ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException(message, ex));
        return (object) null;
      }
    }

    public object ExecuteScalar(IDbCommand sqlCmd)
    {
      return this.ExecuteScalar(sqlCmd, DbTransactionType.Default);
    }

    public object ExecuteScalar(IDbCommand sqlCmd, DbTransactionType transType)
    {
      return this.ExecuteScalar(sqlCmd, ServerGlobals.SQLTimeout, transType);
    }

    public object ExecuteScalar(IDbCommand sqlCmd, TimeSpan timeout, DbTransactionType transType)
    {
      try
      {
        this.logQuery(sqlCmd.CommandText);
        return new DbTransaction((IDbConnection) this.conn, transType).Execute((DbCommandInvoker) new ScalarCommandInvoker(sqlCmd), timeout);
      }
      catch (Exception ex)
      {
        string message = "Error executing query: [" + sqlCmd.CommandText + "]\r\n" + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
        if (!ServerGlobals.Debug && ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
        if (ServerGlobals.Err != null)
          ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException("Error executing query: " + ex.Message, ex));
        return (object) null;
      }
    }

    public void Dispose()
    {
      try
      {
        this.conn.Close();
        this.conn.Dispose();
      }
      catch
      {
      }
    }

    private void logQuery(string query)
    {
      string message = "Executing query: [" + query + "]";
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Verbose, nameof (DbAccessManager), message);
      if (ServerGlobals.TraceLog == null)
        return;
      ServerGlobals.TraceLog.WriteDebugI(nameof (DbAccessManager), message);
    }

    public static void RemoveTableFromCache(string tableName)
    {
      int num = SmartClientUtils.UseReaderWriterLockSlim ? 1 : 0;
      if (num != 0)
        EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(DbAccessManager.tableLockSlim);
      else
        EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(DbAccessManager.tableLock);
      if (DbAccessManager.tables != null && DbAccessManager.tables.ContainsKey((object) tableName))
        DbAccessManager.tables.Remove((object) tableName);
      if (num != 0)
        EllieMae.EMLite.DataAccess.Threading.ReleaseReaderLock(DbAccessManager.tableLockSlim);
      else
        EllieMae.EMLite.DataAccess.Threading.ReleaseReaderLock(DbAccessManager.tableLock);
    }

    private static DataTable GetSchemaTable(
      string dbConnectionString,
      DbServerType dbServerType,
      string tableName)
    {
      DataTable dataTable = new DataTable(tableName);
      try
      {
        using (DbAccessManager dbAccessManager = new DbAccessManager(dbConnectionString, dbServerType))
        {
          DbDataAdapter dataAdapter = DbAccessManager._dbProviderFactory.CreateDataAdapter();
          IDbCommand dbCommand = dbAccessManager.GetDbCommand("select * from " + tableName);
          if (dataAdapter == null || dbCommand == null)
            return dataTable;
          dataAdapter.SelectCommand = dbCommand as DbCommand;
          dataAdapter.FillSchema(dataTable, SchemaType.Source);
        }
      }
      catch (Exception ex)
      {
        if (ServerGlobals.Err != null)
          ServerGlobals.Err.RaiseI(nameof (DbAccessManager), new ServerDataException("Error reading schema for table '" + tableName + "'", ex));
      }
      return dataTable;
    }

    public static DbTableInfo GetTable(
      string dbConnectionString,
      DbServerType dbServerType,
      string tableName)
    {
      if (tableName.ToLower().StartsWith("loanxdb_"))
        throw new InvalidOperationException("The GetTable() method cannot be used for table '" + tableName + "'");
      bool readerWriterLockSlim = SmartClientUtils.UseReaderWriterLockSlim;
      if (readerWriterLockSlim)
        EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(DbAccessManager.tableLockSlim);
      else
        EllieMae.EMLite.DataAccess.Threading.AquireReaderLock(DbAccessManager.tableLock);
      try
      {
        DbTableInfo table = (DbTableInfo) DbAccessManager.tables[(object) tableName];
        if (table != null)
          return table;
      }
      finally
      {
        if (readerWriterLockSlim)
          EllieMae.EMLite.DataAccess.Threading.ReleaseReaderLock(DbAccessManager.tableLockSlim);
        else
          EllieMae.EMLite.DataAccess.Threading.ReleaseReaderLock(DbAccessManager.tableLock);
      }
      DataTable schemaTable = DbAccessManager.GetSchemaTable(dbConnectionString, dbServerType, tableName);
      if (readerWriterLockSlim)
        EllieMae.EMLite.DataAccess.Threading.AquireWriterLock(DbAccessManager.tableLockSlim);
      else
        EllieMae.EMLite.DataAccess.Threading.AquireWriterLock(DbAccessManager.tableLock);
      try
      {
        DbTableInfo table1 = (DbTableInfo) DbAccessManager.tables[(object) tableName];
        if (table1 != null)
          return table1;
        DbTableInfo table2 = new DbTableInfo(schemaTable);
        DbAccessManager.tables.Add((object) tableName, (object) table2);
        return table2;
      }
      finally
      {
        if (readerWriterLockSlim)
          EllieMae.EMLite.DataAccess.Threading.ReleaseWriterLock(DbAccessManager.tableLockSlim);
        else
          EllieMae.EMLite.DataAccess.Threading.ReleaseWriterLock(DbAccessManager.tableLock);
      }
    }

    public static DbTableInfo GetDynamicTable(
      string dbConnectionString,
      DbServerType dbServerType,
      string tableName)
    {
      return new DbTableInfo(DbAccessManager.GetSchemaTable(dbConnectionString, dbServerType, tableName));
    }

    public DbVersion GetDbVersion()
    {
      lock (DbAccessManager.dbVersionMap)
      {
        if (DbAccessManager.dbVersionMap.ContainsKey((object) this.conn.ConnectionString))
          return (DbVersion) DbAccessManager.dbVersionMap[(object) this.conn.ConnectionString];
        IDbCommand dbCommand = this.GetDbCommand("select version()");
        if (dbCommand is SqlCommand)
          dbCommand.CommandText = "select @@version";
        string str = string.Concat(dbCommand.ExecuteScalar());
        DbVersion dbVersion = !str.Contains("Server  2000") ? (!str.Contains("Server 2005") ? (!str.Contains("2008") ? (!str.Contains("2012") ? (!str.Contains("Server 2016") ? (!str.Contains("PostgreSQL 11.0") ? DbVersion.Other : DbVersion.PostgreSQL11) : DbVersion.SQL2016) : DbVersion.SQL2012) : DbVersion.SQL2008) : DbVersion.SQL2005) : DbVersion.SQL2000;
        DbAccessManager.dbVersionMap[(object) this.conn.ConnectionString] = (object) dbVersion;
      }
      return (DbVersion) DbAccessManager.dbVersionMap[(object) this.conn.ConnectionString];
    }

    public static void DefragmentDatabase(
      string dbConnectionString,
      IServerProgressFeedback feedback)
    {
      feedback?.SetFeedback("Scanning database tables...", (string) null, 0);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(dbConnectionString);
      dbQueryBuilder.AppendLine("select so.id, so.name from sysobjects so");
      dbQueryBuilder.AppendLine("where OBJECTPROPERTY(so.id, N'IsUserTable') = 1");
      dbQueryBuilder.AppendLine("  and so.id = object_id(so.name)");
      dbQueryBuilder.AppendLine("  and so.name not like 'AuditTrail[_]%'");
      dbQueryBuilder.AppendLine("order by so.name");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      feedback?.ResetCounter(dataRowCollection1.Count);
      for (int index = 0; index < dataRowCollection1.Count; ++index)
      {
        string str = string.Concat(dataRowCollection1[index]["name"]);
        int num = SQL.DecodeInt(dataRowCollection1[index]["id"], -1);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("select si.indid, si.name from sysindexes si");
        dbQueryBuilder.AppendLine("where si.id = " + (object) num);
        dbQueryBuilder.AppendLine("  and si.keycnt > 0");
        dbQueryBuilder.AppendLine("  and si.first is not null");
        dbQueryBuilder.AppendLine("order by si.indid");
        DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
        if (feedback != null)
        {
          if (!feedback.SetFeedback("Defragmenting...", "Rebuilding " + (object) dataRowCollection2.Count + " index(es) on " + str, index))
            break;
        }
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
        {
          try
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("dbcc indexdefrag (0, " + (object) num + ", " + dataRow["indid"] + ")");
            dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(60.0), DbTransactionType.None);
            string message = "Defragmented index " + str + "." + dataRow["name"];
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbAccessManager), message);
            if (ServerGlobals.TraceLog != null)
              ServerGlobals.TraceLog.WriteInfoI(nameof (DbAccessManager), message);
          }
          catch (Exception ex)
          {
            string message = "Failed to defragment index " + str + "." + dataRow["name"] + ": " + ex.Message;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbAccessManager), message);
            if (ServerGlobals.TraceLog != null)
              ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
            feedback?.SetFeedback((string) null, "Failed to defragment index " + str + "." + dataRow["name"] + ": " + ex.Message, -1);
          }
        }
        if (feedback == null || feedback.SetFeedback("Defragmenting...", "Updating statistics on " + str, index))
        {
          try
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("update statistics [" + str + "] with fullscan");
            dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(60.0), DbTransactionType.None);
            string message = "Updated statistics on " + str;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbAccessManager), message);
            if (ServerGlobals.TraceLog != null)
              ServerGlobals.TraceLog.WriteInfoI(nameof (DbAccessManager), message);
          }
          catch (Exception ex)
          {
            string message = "Failed to update statistics on " + str + ": " + ex.Message;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
            if (ServerGlobals.TraceLog != null)
              ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
            feedback?.SetFeedback((string) null, "Failed to update statistics on " + str + ". " + ex.Message, -1);
          }
        }
        else
          break;
      }
      feedback?.SetFeedback("Done", "", dataRowCollection1.Count);
    }

    public static int GetAvgFragmentationLevel(string dbConnectionString, DbServerType dbServerType)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(dbConnectionString);
      dbQueryBuilder.AppendLine("select so.name from sysobjects so");
      dbQueryBuilder.AppendLine("  inner join sysindexes si on so.id = si.id");
      dbQueryBuilder.AppendLine("where OBJECTPROPERTY(so.id, N'IsUserTable') = 1");
      dbQueryBuilder.AppendLine("  and so.id = object_id(so.name)");
      dbQueryBuilder.AppendLine("  and so.name not like 'AuditTrail[_]%'");
      dbQueryBuilder.AppendLine("  and si.indid = 1");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        insensitiveHashtable[(object) string.Concat(dataRow["name"])] = (object) true;
      string str = (string) null;
      using (DbAccessManager dbAccessManager = new DbAccessManager(dbConnectionString, dbServerType))
      {
        using (DbInfoMessages dbInfoMessages = new DbInfoMessages((SqlConnection) dbAccessManager.Connection))
        {
          dbAccessManager.ExecuteNonQuery("dbcc showcontig", TimeSpan.FromMinutes(10.0), DbTransactionType.None);
          str = dbInfoMessages.ToString();
        }
      }
      Regex regex = new Regex("Table: '(?<table>.*?)'.*?Pages Scanned[^0-9]*(?<pages>[0-9]+).*?(Logical Scan Fragmentation[^0-9]*(?<lfrag>[0-9]+).*?)?Extent Scan Fragmentation[^0-9]*(?<efrag>[0-9]+)", RegexOptions.ExplicitCapture | RegexOptions.Singleline);
      int num1 = 0;
      long num2 = 0;
      string input = str;
      for (Match match = regex.Match(input); match.Success; match = match.NextMatch())
      {
        string key = match.Groups["table"].Value;
        int num3 = Utils.ParseInt((object) match.Groups["pages"].Value, -1);
        int num4 = Utils.ParseInt((object) match.Groups["lfrag"].Value, -1);
        if (insensitiveHashtable.Contains((object) key) && num3 >= 10 && num4 >= 0)
        {
          num1 += num3;
          num2 += (long) (num4 * num3);
        }
      }
      return num1 == 0 ? 0 : (int) (num2 / (long) num1);
    }

    public static void UpdateDatabaseStatistics(
      string dbConnectionString,
      IServerProgressFeedback feedback)
    {
      feedback?.SetFeedback("Scanning database tables...", (string) null, 0);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(dbConnectionString);
      dbQueryBuilder.AppendLine("select so.name from sysobjects so");
      dbQueryBuilder.AppendLine("where OBJECTPROPERTY(so.id, N'IsUserTable') = 1");
      dbQueryBuilder.AppendLine("  and so.id = object_id(so.name)");
      dbQueryBuilder.AppendLine("  and so.name not like 'AuditTrail%'");
      dbQueryBuilder.AppendLine("order by so.name");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      feedback?.ResetCounter(dataRowCollection.Count);
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        string str = string.Concat(dataRowCollection[index]["name"]);
        if (feedback == null || feedback.SetFeedback("Updating Statistics...", "Updating statistics on " + str, index))
        {
          try
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("update statistics [" + str + "] with fullscan");
            dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.None);
            string message = "Updated statistics on " + str;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbAccessManager), message);
            if (ServerGlobals.TraceLog != null)
              ServerGlobals.TraceLog.WriteInfoI(nameof (DbAccessManager), message);
          }
          catch (Exception ex)
          {
            string message = "Failed to update statistics on " + str + ": " + ex.Message;
            if (ServerGlobals.CurrentContextTraceLog != null)
              ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbAccessManager));
            if (ServerGlobals.TraceLog != null)
              ServerGlobals.TraceLog.WriteErrorI(nameof (DbAccessManager), message);
            feedback?.SetFeedback((string) null, "Failed to update statistics on " + str + ". " + ex.Message, -1);
          }
        }
        else
          break;
      }
      feedback?.SetFeedback("Done", "", dataRowCollection.Count);
    }

    public static int GetConsistencyErrorCount(string dbConnectionString, DbServerType dbServerType)
    {
      string input = (string) null;
      using (DbAccessManager dbAccessManager = new DbAccessManager(dbConnectionString, dbServerType))
      {
        using (DbInfoMessages dbInfoMessages = new DbInfoMessages((SqlConnection) dbAccessManager.Connection))
        {
          dbAccessManager.ExecuteNonQuery("SET ARITHABORT ON; SET QUOTED_IDENTIFIER ON; DBCC CHECKDB;", TimeSpan.FromMinutes(10.0), DbTransactionType.None);
          input = dbInfoMessages.ToString();
        }
      }
      Match match = new Regex("CHECKDB found (?<allocErrs>[0-9]+) allocation errors and (?<conErrs>[0-9]+) consistency errors", RegexOptions.ExplicitCapture | RegexOptions.Singleline).Match(input);
      return match.Success ? Utils.ParseInt((object) match.Groups["allocErrs"].Value, 0) + Utils.ParseInt((object) match.Groups["conErrs"].Value, 0) : 0;
    }

    public static DbSize GetDatabaseSize(string dbConnectionString)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(dbConnectionString);
      dbQueryBuilder.AppendLine("exec sp_spaceused @updateusage='TRUE'");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.None);
      return new DbSize(SQL.DecodeString(dataSet.Tables[0].Rows[0]["database_name"]), Convert.ToInt32(Utils.ParseDecimal((object) SQL.DecodeString(dataSet.Tables[0].Rows[0]["database_size"]).Replace(" MB", "")) * 1024M), Convert.ToInt32(Utils.ParseDecimal((object) SQL.DecodeString(dataSet.Tables[1].Rows[0]["reserved"]).Replace(" KB", ""))), Convert.ToInt32(Utils.ParseDecimal((object) SQL.DecodeString(dataSet.Tables[1].Rows[0]["data"]).Replace(" KB", ""))), Convert.ToInt32(Utils.ParseDecimal((object) SQL.DecodeString(dataSet.Tables[1].Rows[0]["index_size"]).Replace(" KB", ""))));
    }

    public static DbUsageInfo GetUsageInfo(string dbConnnectionString, DbServerType dbServerType)
    {
      DbUsageInfo usageInfo = new DbUsageInfo();
      using (DbAccessManager dbAccessManager = new DbAccessManager(dbConnnectionString, dbServerType))
      {
        usageInfo.AlertCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from LoanAlerts", DbTransactionType.None));
        usageInfo.BizContactCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from BizPartner", DbTransactionType.None));
        usageInfo.BorrowerCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from Borrower", DbTransactionType.None));
        usageInfo.GroupCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from AclGroups", DbTransactionType.None));
        usageInfo.LoanCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from LoanSummary", DbTransactionType.None));
        usageInfo.OrgCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from org_chart where org_type = " + (object) DBNull.Value, DbTransactionType.None));
        usageInfo.OrgMaxDepth = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select max(depth) from org_chart where org_type = " + (object) DBNull.Value, DbTransactionType.None));
        usageInfo.PersonaCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from Personas", DbTransactionType.None));
        usageInfo.UserCount = SQL.DecodeInt(dbAccessManager.ExecuteScalar("select count(*) from users", DbTransactionType.None));
      }
      return usageInfo;
    }

    public static bool ValidateConnection(string connString)
    {
      DbConnection dbConnection = (DbConnection) null;
      try
      {
        dbConnection = DbAccessManager._dbProviderFactory.CreateConnection();
        dbConnection.ConnectionString = connString;
        dbConnection.Open();
      }
      catch
      {
        return false;
      }
      finally
      {
        dbConnection?.Close();
      }
      return true;
    }

    public void DoBulkCopy(
      string tableName,
      DataTable tableRows,
      Dictionary<string, string> columnMapping = null)
    {
      using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(this.conn as SqlConnection))
      {
        sqlBulkCopy.DestinationTableName = tableName;
        if (columnMapping != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in columnMapping)
            sqlBulkCopy.ColumnMappings.Add(keyValuePair.Key, keyValuePair.Value);
        }
        sqlBulkCopy.WriteToServer(tableRows, DataRowState.Unchanged);
      }
    }
  }
}
