// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.StoredProcedureSignature
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public struct StoredProcedureSignature(string name, List<SqlDbType> parameters)
  {
    public string StoreProcedureName = name;
    public List<SqlDbType> Parameters = parameters;

    public string GetStoredProcedureCall(string[] inputData)
    {
      string str1 = "exec " + this.StoreProcedureName + " ";
      string str2 = "";
      for (int index = 0; index < this.Parameters.Count; ++index)
      {
        if (inputData.Length < index + 1)
          str2 += str2 == "" ? "NULL" : str2 + ", NULL";
        switch (this.Parameters[index])
        {
          case SqlDbType.Char:
          case SqlDbType.NChar:
          case SqlDbType.NText:
          case SqlDbType.NVarChar:
          case SqlDbType.Text:
          case SqlDbType.VarChar:
            str2 += str2 == "" ? "'" + inputData[index].Replace("'", "''") + "'" : ", '" + inputData[index].Replace("'", "''") + "'";
            break;
          case SqlDbType.Int:
            str2 += str2 == "" ? inputData[index] : ", " + inputData[index];
            break;
        }
      }
      return str1 + str2;
    }
  }
}
