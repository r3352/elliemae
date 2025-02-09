// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbValue
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbValue
  {
    protected string columnName;
    protected object value;
    protected IDbEncoder encoder;

    public DbValue(string columnName, object value)
      : this(columnName, value, (IDbEncoder) DbEncoding.Default)
    {
    }

    public DbValue(string columnName, object value, IDbEncoder encoder)
    {
      this.columnName = columnName;
      this.value = value;
      this.encoder = encoder;
    }

    public string ColumnName => this.columnName;

    public object Value => this.value;

    public string Encode(DbTableInfo tableInfo) => this.Encode(tableInfo[this.columnName]);

    public string Encode(DbColumnInfo column = null) => this.encoder.Encode(this.value, column);

    public string Encode(DbColumnInfo column, object filterValue)
    {
      return this.encoder.Encode(filterValue, column);
    }
  }
}
