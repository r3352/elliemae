// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.IDbQueryBuilder
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public interface IDbQueryBuilder
  {
    int Length { get; }

    void Reset();

    void AppendFormat(string format, params object[] args);

    void AppendLineFormat(string format, params object[] args);

    void Append(string text);

    void AppendLine(string text);

    void Remove(int startIndex, int length);

    void Where(DbValueList values);

    int Count(string selection);

    string OrderBy(List<SortColumn> sortColumnList, bool forUseInRowNumber = false);

    DataTable GetPage(
      string selection,
      int pageSize,
      int pageNumber,
      List<SortColumn> sortColumnList);

    void InsertInto(DbTableInfo table, DbValueList values);

    void Update(DbTableInfo table, DbValueList values, DbValueList keys);

    void Update(DbTableInfo table, DbValueList values, DbValue key);

    void SelectFrom(DbTableInfo table);

    void SelectFrom(DbTableInfo table, string[] columnNames);

    void SelectFrom(DbTableInfo table, DbValue key);

    void SelectFrom(DbTableInfo table, string[] columnNames, DbValue key);

    void SelectFrom(DbTableInfo table, DbValueList keys);

    void SelectFrom(DbTableInfo table, string[] columnNames, DbValueList keys);

    void DeleteFrom(DbTableInfo table);

    void DeleteFrom(DbTableInfo table, DbValue key);

    void DeleteFrom(DbTableInfo table, DbValueList keys);

    void Select(string varName);

    void Declare(string varName, string varType);

    void SelectVar(string varName, object value);

    void SelectVar(string varName, object value, IDbEncoder encoding);

    void SelectIdentity(string varName);

    void SelectIdentity();

    void RaiseError(string description);

    void Begin();

    void End();

    void If(string sql);

    void IfExists(DbTableInfo table, DbValue keyValue);

    void IfExists(DbTableInfo table, DbValueList keyValues);

    void IfNotExists(DbTableInfo table, DbValue keyValue);

    void IfNotExists(DbTableInfo table, DbValueList keyValues);

    void Else();

    void Replace(string oldString, string newString);

    DataRowCollection Execute();

    DataRowCollection Execute(DbTransactionType transType);

    DataRowCollection ExecuteStoredProc(DbTransactionType transType, DbValueList parameters);

    void ExecuteNonQuery();

    void ExecuteNonQuery(DbTransactionType transType);

    void ExecuteNonQuery(TimeSpan timeout, DbTransactionType transType);

    object ExecuteScalar();

    object ExecuteScalar(DbTransactionType transType);

    object ExecuteScalar(TimeSpan timeout);

    DataSet ExecuteSetQuery();

    DataSet ExecuteSetQuery(DbTransactionType transType);

    DataSet ExecuteSetQuery(TimeSpan timeout, DbTransactionType transType);

    DataTable ExecuteTableQuery();

    DataRow ExecuteRowQuery();

    DbDataReader ExecuteReaderQuery();
  }
}
