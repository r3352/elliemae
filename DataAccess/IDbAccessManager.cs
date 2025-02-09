// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.IDbAccessManager
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public interface IDbAccessManager : IDisposable
  {
    IDbConnection Connection { get; }

    DataTable ExecuteTableQuery(string sql);

    DataTable ExecuteTableQuery(string sql, DbTransactionType transactionType);

    DataTable ExecuteTableQuery(string sql, TimeSpan timeout);

    DataTable ExecuteTableQuery(IDbCommand command);

    DataTable ExecuteTableQuery(IDbCommand command, DbTransactionType transactionType);

    DataRow ExecuteRowQuery(string sql);

    DataRow ExecuteRowQuery(string sql, DbTransactionType transactionType);

    DataRow ExecuteRowQuery(IDbCommand command);

    DataRow ExecuteRowQuery(IDbCommand command, DbTransactionType transactionType);

    DataRowCollection ExecuteQuery(
      string sql,
      DbCommandParameter[] dbCommandParameters = null,
      DbTransactionType transType = DbTransactionType.Default);

    DataRowCollection ExecuteStoredProc(
      string spName,
      DbTransactionType transType,
      DbValueList parameters);

    object ExecuteScalar(string sql);

    object ExecuteScalar(IDbCommand command);

    object ExecuteScalar(string sql, DbTransactionType transactionType);

    object ExecuteScalar(IDbCommand command, DbTransactionType transactionType);

    object ExecuteScalar(IDbCommand command, TimeSpan timeout, DbTransactionType transactionType);

    void ExecuteNonQuery(string sql, DbTransactionType transactionType = DbTransactionType.Default);

    void ExecuteNonQuery(IDbCommand command);

    void ExecuteNonQuery(IDbCommand command, DbTransactionType transactionType);

    void ExecuteNonQuery(string sql, TimeSpan timeout, DbTransactionType transactionType);

    void ExecuteNonQuery(IDbCommand command, TimeSpan timeout, DbTransactionType transactionType);

    DataSet ExecuteSetQuery(string sql);

    DataSet ExecuteSetQuery(string sql, DbTransactionType transactionType);

    DataSet ExecuteSetQuery(IDbCommand command, DbTransactionType transactionType = DbTransactionType.Default);

    DataSet ExecuteSetQuery(string sql, TimeSpan timeout, DbTransactionType transactionType);

    DataSet ExecuteSetQuery(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType);

    IDataReader ExecuteReaderQuery(string sql, DbTransactionType transactionType = DbTransactionType.Default);

    IDataReader ExecuteReaderQuery(IDbCommand command, DbTransactionType transactionType = DbTransactionType.Default);

    IDataReader ExecuteReaderQuery(
      IDbCommand command,
      TimeSpan timeout,
      DbTransactionType transactionType);

    IDbCommand GetDbCommand(string statement);

    void DoBulkCopy(
      string tableName,
      DataTable tableRows,
      Dictionary<string, string> columnMapping = null);
  }
}
