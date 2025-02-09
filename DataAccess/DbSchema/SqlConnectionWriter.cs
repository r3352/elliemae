// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbSchema.SqlConnectionWriter
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.DataAccess.DbSchema
{
  public class SqlConnectionWriter : IScriptWriter
  {
    private int commandTimeout = 30;

    public SqlConnectionWriter(SqlConnection conn) => this.Connection = conn;

    public SqlConnectionWriter(SqlConnection conn, int commandTimeout)
    {
      this.Connection = conn;
      this.commandTimeout = commandTimeout;
    }

    public SqlConnection Connection { get; private set; }

    public void WriteTransaction(string text)
    {
      try
      {
        using (SqlTransaction sqlTransaction = this.Connection.BeginTransaction())
        {
          SqlCommand sqlCommand = new SqlCommand(text, this.Connection);
          sqlCommand.CommandTimeout = this.commandTimeout;
          sqlCommand.Transaction = sqlTransaction;
          sqlCommand.ExecuteNonQuery();
          sqlTransaction.Commit();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error executing query: " + ex.Message + ". Query text:" + Environment.NewLine + text, ex);
      }
    }
  }
}
