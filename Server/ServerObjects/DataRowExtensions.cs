// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.DataRowExtensions
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class DataRowExtensions
  {
    public static T GetColumn<T>(
      this DataRow row,
      string id,
      T defaultIfDbNull = null,
      T defaultIfMissing = null)
      where T : class
    {
      if (!row.Table.Columns.Contains(id))
        return defaultIfMissing;
      return !DBNull.Value.Equals(row[id]) ? (T) row[id] : defaultIfDbNull;
    }
  }
}
