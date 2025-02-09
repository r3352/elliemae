// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbTransaction
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using Npgsql;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public sealed class DbTransaction
  {
    private const string className = "DbTransaction";
    private const int SQLDeadlockExceptionNum = 1205;
    private const int SQLNetworkErrorExceptionNum = 11;
    private const int SQLSnapshotIsolationExceptionNum = 3961;
    private static readonly int MaxRetries = Utils.ParseInt((object) EnConfigurationSettings.AppSettings["SQLTransactionRetries"], 10);
    private static readonly int DeadlockDelay = Utils.ParseInt((object) EnConfigurationSettings.AppSettings["SQLTransactionDeadlockDelay"], 100);
    private IDbConnection conn;
    private IDbContext context;
    private DbTransactionType transactionType = DbTransactionType.Default;

    public DbTransaction(IDbConnection conn, IDbContext context = null)
      : this(conn, DbTransactionType.Default, context)
    {
    }

    public DbTransaction(IDbConnection conn, DbTransactionType transType, IDbContext context = null)
    {
      this.conn = conn;
      this.transactionType = transType;
      this.context = context ?? (IDbContext) new MsDbContext();
    }

    public IDbConnection Connection => this.conn;

    public object Execute(DbCommandInvoker cmd) => this.Execute(cmd, this.context.SQLTimeout);

    public object Execute(DbCommandInvoker cmd, TimeSpan timeout)
    {
      using (PerformanceMeter.Current.BeginOperation("DbTransaction.Execute"))
      {
        int num1 = 0;
        while (true)
        {
          try
          {
            return this.invokeInternal(cmd, timeout);
          }
          catch (SqlException ex)
          {
            if (ex.Number == 11)
            {
              if (++num1 > DbTransaction.MaxRetries)
                throw new Exception("SQL query failed due to general network error after retrying " + (object) DbTransaction.MaxRetries + " times");
              string message = "SQL general network error detected -- attempt to re-open connection will be made. Retry count = " + (object) num1;
              if (ServerGlobals.CurrentContextTraceLog != null)
                ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Warning, nameof (DbTransaction), message);
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteWarningI(nameof (DbTransaction), message);
              this.reopenConnection();
            }
            else if (ex.Number == 1205)
            {
              if (++num1 > DbTransaction.MaxRetries)
                throw new Exception("SQL query failed due to transaction blocking after retrying " + (object) DbTransaction.MaxRetries + " times");
              int num2 = (num1 - 1) * DbTransaction.DeadlockDelay;
              string message = "SQL Deadlock detected. Retry count = " + (object) num1 + ", Delay = " + (object) num2 + "ms";
              if (ServerGlobals.CurrentContextTraceLog != null)
                ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbTransaction), message);
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteInfoI(nameof (DbTransaction), message);
              Thread.Sleep(TimeSpan.FromMilliseconds((double) num2));
            }
            else if (ex.Number == 3961)
            {
              if (++num1 > DbTransaction.MaxRetries)
                throw new Exception("SQL query failed due to snapshot isolation conflict after retrying " + (object) DbTransaction.MaxRetries + " times");
              int num3 = (num1 - 1) * DbTransaction.DeadlockDelay;
              string message = "SQL Snapshot Isolation conflict detected. Retry count = " + (object) num1 + ", Delay = " + (object) num3 + "ms";
              if (ServerGlobals.CurrentContextTraceLog != null)
                ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbTransaction), message);
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteInfoI(nameof (DbTransaction), message);
              Thread.Sleep(TimeSpan.FromMilliseconds((double) num3));
            }
            else
              throw;
          }
          catch (PostgresException ex)
          {
            if (ex.SqlState == "40P01")
            {
              if (++num1 > DbTransaction.MaxRetries)
                throw new Exception("SQL query failed due to transaction blocking after retrying " + (object) DbTransaction.MaxRetries + " times");
              int num4 = (num1 - 1) * DbTransaction.DeadlockDelay;
              string message = "SQL Deadlock detected. Retry count = " + (object) num1 + ", Delay = " + (object) num4 + "ms";
              if (ServerGlobals.CurrentContextTraceLog != null)
                ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbTransaction), message);
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteInfoI(nameof (DbTransaction), message);
              Thread.Sleep(TimeSpan.FromMilliseconds((double) num4));
            }
            else
              throw;
          }
          catch (NpgsqlException ex)
          {
            if (ex.Message.StartsWith("The connection pool has been exhausted,"))
            {
              if (++num1 > DbTransaction.MaxRetries)
                throw new Exception("SQL query failed due to database connection timeout after retrying " + (object) DbTransaction.MaxRetries + " times");
              int num5 = (num1 - 1) * DbTransaction.DeadlockDelay;
              string message = "database connection timeout. Retry count = " + (object) num1 + ", Delay = " + (object) num5 + "ms";
              if (ServerGlobals.CurrentContextTraceLog != null)
                ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbTransaction), message);
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteInfoI(nameof (DbTransaction), message);
              Thread.Sleep(TimeSpan.FromMilliseconds((double) num5));
            }
            else
              throw;
          }
          catch (Exception ex)
          {
            if (ex.Source == "System.Data" && ex.Message.StartsWith("Timeout expired.") || ex.Source == "Npgsql" && ex.Message.StartsWith("The operation has timed out."))
            {
              if (++num1 > DbTransaction.MaxRetries)
                throw new Exception("SQL query failed due to database connection timeout after retrying " + (object) DbTransaction.MaxRetries + " times");
              int num6 = (num1 - 1) * DbTransaction.DeadlockDelay;
              string message = "database connection timeout. Retry count = " + (object) num1 + ", Delay = " + (object) num6 + "ms";
              if (ServerGlobals.CurrentContextTraceLog != null)
                ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbTransaction), message);
              if (ServerGlobals.TraceLog != null)
                ServerGlobals.TraceLog.WriteInfoI(nameof (DbTransaction), message);
              Thread.Sleep(TimeSpan.FromMilliseconds((double) num6));
            }
            else
              throw;
          }
        }
      }
    }

    private object invokeInternal(DbCommandInvoker cmd, TimeSpan timeout)
    {
      if (this.context.SQLLatency != TimeSpan.Zero)
        Thread.Sleep(this.context.SQLLatency);
      DateTime utcNow = DateTime.UtcNow;
      cmd.CommandToInvoke.CommandTimeout = (int) timeout.TotalSeconds;
      try
      {
        return this.transactionType == DbTransactionType.None ? this.invokeCommand(cmd) : this.invokeTransaction(cmd);
      }
      finally
      {
        string commandText = cmd.CommandToInvoke.CommandText;
        bool flag = this.Connection != null && this.Connection.ConnectionString != null && this.Connection.ConnectionString.Contains("ApplicationIntent=ReadOnly");
        if (cmd.CommandToInvoke.CommandType == CommandType.StoredProcedure)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("Stored Procedure: ").AppendLine(cmd.CommandToInvoke.CommandText);
          foreach (DbParameter parameter in (IEnumerable) cmd.CommandToInvoke.Parameters)
            stringBuilder.AppendLine(string.Format("\t{0}: {1} ({2})", (object) parameter.ParameterName, (object) parameter.Value.ToString(), (object) parameter.Direction.ToString()));
          commandText = stringBuilder.ToString();
        }
        string str = flag ? "[ReadReplicaConnectionString]:" + commandText : commandText;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Info, nameof (DbTransaction), str);
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteSqlTraceI(str, utcNow);
      }
    }

    private object invokeTransaction(DbCommandInvoker cmd)
    {
      using (IDbTransaction dbTransaction = this.conn.BeginTransaction(this.transactionTypeToIsolationLevel(this.conn, this.transactionType)))
      {
        cmd.CommandToInvoke.Connection = this.conn;
        cmd.CommandToInvoke.Transaction = dbTransaction;
        if (this.context is PgDbContext)
        {
          PgDbContext context = (PgDbContext) this.context;
          if (context.RLSEnabled)
            cmd.CommandToInvoke.CommandText = "set local role " + "\"" + context.Role + "\";" + cmd.CommandToInvoke.CommandText;
        }
        object obj = this.invokeCommand(cmd);
        dbTransaction.Commit();
        return obj;
      }
    }

    private object invokeCommand(DbCommandInvoker cmd)
    {
      cmd.CommandToInvoke.Connection = this.conn;
      return cmd.Execute();
    }

    private void reopenConnection()
    {
      try
      {
        this.conn.Close();
        this.conn.Open();
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        if (ServerGlobals.CurrentContextTraceLog != null)
          ServerGlobals.CurrentContextTraceLog.Write(message, nameof (DbTransaction));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteExceptionI(nameof (DbTransaction), ex);
        if (ServerGlobals.Err == null)
          return;
        ServerGlobals.Err.RaiseI(nameof (DbTransaction), new ServerDataException("Error attempting to re-open closed connection: " + ex.Message));
      }
    }

    private IsolationLevel transactionTypeToIsolationLevel(
      IDbConnection conn,
      DbTransactionType transType)
    {
      switch (transType)
      {
        case DbTransactionType.Default:
          return IsolationLevel.ReadCommitted;
        case DbTransactionType.Snapshot:
          return IsolationLevel.ReadCommitted;
        case DbTransactionType.Serialized:
          return IsolationLevel.Serializable;
        default:
          throw new ArgumentException("Invalid transaction type specified (" + (object) transType + ")");
      }
    }
  }
}
