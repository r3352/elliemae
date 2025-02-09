// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.PgDbAccessManager
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class PgDbAccessManager : IDbAccessManager, IDisposable
  {
    private const string ClassName = "PgDbAccessManager";
    private readonly IDbContext _dbContext;
    private IDbConnection _connection;

    public PgDbAccessManager(IDbContext dbContext)
    {
      this._dbContext = dbContext;
      try
      {
        this._connection = (IDbConnection) new NpgsqlConnection(this._dbContext.ConnectionString);
        this._connection.Open();
      }
      catch (Exception ex)
      {
        string message = "Error opening connection to '" + this.GetSanitizedConnectionString(this._dbContext.ConnectionString) + "'. Error: " + ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (PgDbAccessManager));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteExceptionI(nameof (PgDbAccessManager), ex);
        if (ServerGlobals.Err == null)
          return;
        ServerGlobals.Err.RaiseI(nameof (PgDbAccessManager), new ServerDataException(message));
      }
    }

    public IDbConnection Connection => this._connection;

    public DataTable ExecuteTableQuery(string sql)
    {
      return this.ExecuteTableQuery((IDbCommand) new NpgsqlCommand(sql));
    }

    public DataTable ExecuteTableQuery(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteTableQuery((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public DataTable ExecuteTableQuery(string sql, TimeSpan timeout)
    {
      return this.ExecuteTableQuery((IDbCommand) new NpgsqlCommand(sql), timeout, DbTransactionType.Default);
    }

    public DataTable ExecuteTableQuery(IDbCommand command)
    {
      return this.ExecuteTableQuery(command, DbTransactionType.Default);
    }

    public DataTable ExecuteTableQuery(IDbCommand command, DbTransactionType transactionType)
    {
      return this.ExecuteTableQuery(command, this._dbContext.SQLTimeout, transactionType);
    }

    public DataTable ExecuteTableQuery(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType)
    {
      DataSet dataSet = this.ExecuteSetQuery(command, timeout, transactionType);
      return dataSet.Tables.Count != 0 ? dataSet.Tables[0] : (DataTable) null;
    }

    public DataRow ExecuteRowQuery(string sql)
    {
      return this.ExecuteRowQuery((IDbCommand) new NpgsqlCommand(sql));
    }

    public DataRow ExecuteRowQuery(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteRowQuery((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public DataRow ExecuteRowQuery(IDbCommand command)
    {
      return this.ExecuteRowQuery(command, DbTransactionType.Default);
    }

    public DataRow ExecuteRowQuery(IDbCommand command, DbTransactionType transactionType)
    {
      DataSet dataSet = this.ExecuteSetQuery(command, transactionType);
      if (dataSet.Tables.Count == 0)
        return (DataRow) null;
      return dataSet.Tables[0].Rows.Count != 0 ? dataSet.Tables[0].Rows[0] : (DataRow) null;
    }

    public DataRowCollection ExecuteQuery(
      string sql,
      DbCommandParameter[] dbCommandParameters = null,
      DbTransactionType transType = DbTransactionType.Default)
    {
      throw new NotImplementedException();
    }

    public DataRowCollection ExecuteQuery(string sql)
    {
      return this.ExecuteQuery((IDbCommand) new NpgsqlCommand(sql));
    }

    public DataRowCollection ExecuteQuery(IDbCommand command)
    {
      return this.ExecuteQuery(command, DbTransactionType.Default);
    }

    public DataRowCollection ExecuteQuery(IDbCommand command, DbTransactionType transactionType)
    {
      return this.ExecuteTableQuery(command, transactionType)?.Rows;
    }

    public DataRowCollection ExecuteQuery(string sql, TimeSpan timeout)
    {
      return this.ExecuteTableQuery((IDbCommand) new NpgsqlCommand(sql), timeout, DbTransactionType.Default)?.Rows;
    }

    public object ExecuteScalar(string sql)
    {
      return this.ExecuteScalar((IDbCommand) new NpgsqlCommand(sql));
    }

    public object ExecuteScalar(IDbCommand command)
    {
      return this.ExecuteScalar(command, DbTransactionType.Default);
    }

    public object ExecuteScalar(IDbCommand command, DbTransactionType transactionType)
    {
      return this.ExecuteScalar(command, this._dbContext.SQLTimeout, transactionType);
    }

    public object ExecuteScalar(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType)
    {
      try
      {
        this.LogQuery(command.CommandText);
        return new DbTransaction(this._connection, transactionType, this._dbContext).Execute((DbCommandInvoker) new ScalarCommandInvoker(command), timeout);
      }
      catch (Exception ex)
      {
        this.LogRaiseError(ex, command);
        throw;
      }
    }

    public void ExecuteNonQuery(string sql, DbTransactionType transactionType = DbTransactionType.Default)
    {
      this.ExecuteNonQuery((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public void ExecuteNonQuery(IDbCommand command)
    {
      this.ExecuteNonQuery(command, DbTransactionType.Default);
    }

    public void ExecuteNonQuery(IDbCommand command, DbTransactionType transactionType)
    {
      this.ExecuteNonQuery(command, this._dbContext.SQLTimeout, transactionType);
    }

    public void ExecuteNonQuery(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType)
    {
      try
      {
        this.LogQuery(command.CommandText);
        new DbTransaction(this._connection, transactionType, this._dbContext).Execute((DbCommandInvoker) new NonQueryCommandInvoker(command), timeout);
      }
      catch (Exception ex)
      {
        this.LogRaiseError(ex, command);
        throw;
      }
    }

    public void ExecuteNonQuery(
      IEnumerable<IDbCommand> commands,
      TimeSpan timeout,
      DbTransactionType transactionType)
    {
      DbTransaction dbTransaction = new DbTransaction(this._connection, transactionType, this._dbContext);
      foreach (IDbCommand command in commands)
      {
        this.LogQuery(command.CommandText);
        try
        {
          dbTransaction.Execute((DbCommandInvoker) new NonQueryCommandInvoker(command), timeout);
        }
        catch (Exception ex)
        {
          this.LogRaiseError(ex, command);
          throw;
        }
      }
    }

    public DataSet ExecuteSetQuery(string sql)
    {
      return this.ExecuteSetQuery((IDbCommand) new NpgsqlCommand(sql), DbTransactionType.Default);
    }

    public DataSet ExecuteSetQuery(IDbCommand command, DbTransactionType transactionType = DbTransactionType.Default)
    {
      return this.ExecuteSetQuery(command, this._dbContext.SQLTimeout, transactionType);
    }

    public DataSet ExecuteSetQuery(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType)
    {
      try
      {
        this.LogQuery(command.CommandText);
        return (DataSet) new DbTransaction(this._connection, transactionType, this._dbContext).Execute((DbCommandInvoker) new DataSetCommandInvoker(command, (IDbDataAdapter) new NpgsqlDataAdapter()), timeout);
      }
      catch (Exception ex)
      {
        this.LogRaiseError(ex, command);
        throw;
      }
    }

    public IDataReader ExecuteReaderQuery(string sql, DbTransactionType transactionType = DbTransactionType.Default)
    {
      return this.ExecuteReaderQuery((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public IDataReader ExecuteReaderQuery(IDbCommand command, DbTransactionType transactionType = DbTransactionType.Default)
    {
      return this.ExecuteReaderQuery(command, this._dbContext.SQLTimeout, transactionType);
    }

    public IDataReader ExecuteReaderQuery(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType)
    {
      if (transactionType != DbTransactionType.None)
      {
        DataSet dataSet = this.ExecuteSetQuery(command, timeout, transactionType);
        return dataSet == null ? (IDataReader) null : (IDataReader) dataSet.CreateDataReader();
      }
      try
      {
        this.LogQuery(command.CommandText);
        return (IDataReader) new DbTransaction(this._connection, transactionType, this._dbContext).Execute((DbCommandInvoker) new DataReaderCommandInvoker(command), timeout);
      }
      catch (Exception ex)
      {
        this.LogRaiseError(ex, command);
        throw;
      }
    }

    public IDbCommand GetDbCommand(string statement)
    {
      return (IDbCommand) new NpgsqlCommand(statement, (NpgsqlConnection) this._connection);
    }

    private void LogQuery(string query)
    {
      string message = "Executing query: [" + query + "]";
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Verbose, nameof (PgDbAccessManager), message);
      if (ServerGlobals.TraceLog == null)
        return;
      ServerGlobals.TraceLog.WriteDebugI(nameof (PgDbAccessManager), message);
    }

    public DataRowCollection ExecuteQuery(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteQuery((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public object ExecuteScalar(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteScalar((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public void ExecuteNonQuery(string sql, TimeSpan timeout, DbTransactionType transactionType)
    {
      this.ExecuteNonQuery((IDbCommand) new NpgsqlCommand(sql), timeout, transactionType);
    }

    public DataSet ExecuteSetQuery(string sql, DbTransactionType transactionType)
    {
      return this.ExecuteSetQuery((IDbCommand) new NpgsqlCommand(sql), transactionType);
    }

    public DataSet ExecuteSetQuery(string sql, TimeSpan timeout, DbTransactionType transactionType)
    {
      return this.ExecuteSetQuery((IDbCommand) new NpgsqlCommand(sql), timeout, transactionType);
    }

    private void LogRaiseError(Exception ex, IDbCommand command)
    {
      string message = "Error executing query: [" + command.CommandText + "]\r\n" + ex.Message;
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(message, nameof (PgDbAccessManager));
      if (ServerGlobals.TraceLog != null)
        ServerGlobals.TraceLog.WriteErrorI(nameof (PgDbAccessManager), message);
      if (ServerGlobals.Err == null)
        return;
      ServerGlobals.Err.RaiseI(nameof (PgDbAccessManager), new ServerDataException("Error executing query: " + ex.Message, ex));
    }

    private string GetSanitizedConnectionString(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
        return "";
      string connectionString1 = connectionString;
      int startIndex = connectionString.IndexOf("password", StringComparison.InvariantCultureIgnoreCase);
      if (startIndex > 0)
      {
        int num = connectionString.IndexOf(";", startIndex, StringComparison.Ordinal);
        connectionString1 = connectionString1.Remove(startIndex + 9, num - (startIndex + 9));
      }
      return connectionString1;
    }

    public void Dispose()
    {
      try
      {
        this._connection.Close();
        this._connection.Dispose();
      }
      catch
      {
      }
    }

    public DataRowCollection ExecuteStoredProc(
      string spName,
      DbTransactionType transType,
      DbValueList parameters)
    {
      throw new NotImplementedException();
    }

    public void DoBulkCopy(
      string tableName,
      DataTable tableRows,
      Dictionary<string, string> columnMapping = null)
    {
      throw new NotImplementedException();
    }
  }
}
