// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbTableInfo
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  [Serializable]
  public class DbTableInfo : IEnumerable
  {
    private string name;
    private Hashtable columns = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public DbTableInfo(DataTable tableSchema)
    {
      this.name = tableSchema.TableName;
      foreach (DataColumn column in (InternalDataCollectionBase) tableSchema.Columns)
        this.columns.Add((object) column.ColumnName, (object) new DbColumnInfo(column));
    }

    public string Name => this.name;

    public DbColumnInfo this[string columnName] => (DbColumnInfo) this.columns[(object) columnName];

    public int Count => this.columns.Count;

    public IEnumerator GetEnumerator() => (IEnumerator) this.columns.GetEnumerator();
  }
}
