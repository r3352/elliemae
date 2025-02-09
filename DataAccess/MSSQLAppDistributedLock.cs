// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.MSSQLAppDistributedLock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class MSSQLAppDistributedLock : IDisposable
  {
    private readonly string _uniqueId;
    private readonly SqlConnection _sqlConnection;
    private bool _isLockTaken;

    public MSSQLAppDistributedLock(string uniqueId, string connectionString)
    {
      this._uniqueId = uniqueId;
      this._sqlConnection = new SqlConnection(connectionString);
      this._sqlConnection.Open();
    }

    public int TakeLock(int millisecondsTimeout)
    {
      int num = -999;
      try
      {
        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
        {
          SqlCommand sqlCommand1 = new SqlCommand("sp_getapplock", this._sqlConnection);
          sqlCommand1.CommandType = CommandType.StoredProcedure;
          SqlCommand sqlCommand2 = sqlCommand1;
          sqlCommand2.Parameters.AddWithValue("Resource", (object) this._uniqueId);
          sqlCommand2.Parameters.AddWithValue("LockOwner", (object) "Session");
          sqlCommand2.Parameters.AddWithValue("LockMode", (object) "Exclusive");
          sqlCommand2.Parameters.AddWithValue("LockTimeout", (object) millisecondsTimeout);
          SqlParameter sqlParameter = sqlCommand2.Parameters.Add("ReturnValue", SqlDbType.Int);
          sqlParameter.Direction = ParameterDirection.ReturnValue;
          sqlCommand2.ExecuteNonQuery();
          num = (int) sqlParameter.Value;
          if (num >= 0)
            this._isLockTaken = true;
          transactionScope.Complete();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return num;
    }

    private void ReleaseLock()
    {
      using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Suppress))
      {
        SqlCommand sqlCommand = new SqlCommand("sp_releaseapplock", this._sqlConnection);
        sqlCommand.CommandType = CommandType.StoredProcedure;
        sqlCommand.Parameters.AddWithValue("Resource", (object) this._uniqueId);
        sqlCommand.Parameters.AddWithValue("LockOwner", (object) "Session");
        sqlCommand.ExecuteNonQuery();
        this._isLockTaken = false;
        transactionScope.Complete();
      }
    }

    public void Dispose()
    {
      if (this._isLockTaken)
        this.ReleaseLock();
      this._sqlConnection.Close();
    }
  }
}
