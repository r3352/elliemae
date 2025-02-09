// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DbAccessManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DbAccessManager
  {
    private SqlConnection conn;

    public DbAccessManager(
      string server,
      string database,
      string dbUserid,
      string dbPassword,
      bool pooling,
      int minPoolSize,
      int maxPoolSize,
      int connectTimeout)
    {
      this.init(server, database, dbUserid, dbPassword, pooling, minPoolSize, maxPoolSize, connectTimeout);
    }

    public DbAccessManager()
    {
      this.conn = new SqlConnection(EnConfigurationSettings.GlobalSettings.DatabaseConnectionString);
    }

    private void init(
      string server,
      string database,
      string dbUserid,
      string dbPassword,
      bool pooling,
      int minPoolSize,
      int maxPoolSize,
      int connectTimeout)
    {
      this.conn = new SqlConnection(new SqlConnectionStringBuilder()
      {
        DataSource = server,
        InitialCatalog = database,
        UserID = dbUserid,
        Password = dbPassword,
        Pooling = pooling,
        MinPoolSize = minPoolSize,
        MaxPoolSize = maxPoolSize,
        ConnectTimeout = connectTimeout
      }.ConnectionString);
    }

    public void Open()
    {
      if (this.conn.State == ConnectionState.Open)
        return;
      this.conn.Open();
    }

    public void Close() => this.conn.Close();

    public DataTable ExecuteTableQuery(string sql)
    {
      this.Open();
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(new SqlCommand(sql, this.conn));
      DataSet dataSet1 = new DataSet();
      DataSet dataSet2 = dataSet1;
      sqlDataAdapter.Fill(dataSet2);
      return dataSet1.Tables.Count == 0 ? (DataTable) null : dataSet1.Tables[0];
    }

    public DataRowCollection ExecuteQuery(string sql) => this.ExecuteTableQuery(sql)?.Rows;

    public object ExecuteScalar(string sql) => new SqlCommand(sql, this.conn).ExecuteScalar();

    public void ExecuteNonQuery(string sql) => new SqlCommand(sql, this.conn).ExecuteNonQuery();

    public void ExecuteNonQuery(string sql, Hashtable sqlParameters)
    {
      SqlCommand sqlCommand = new SqlCommand(sql, this.conn);
      if (sqlParameters != null)
      {
        foreach (string key in (IEnumerable) sqlParameters.Keys)
          sqlCommand.Parameters.AddWithValue(key, sqlParameters[(object) key] == null ? (object) DBNull.Value : sqlParameters[(object) key]);
      }
      sqlCommand.ExecuteNonQuery();
    }

    public bool isOpen() => this.conn.State == ConnectionState.Open;
  }
}
