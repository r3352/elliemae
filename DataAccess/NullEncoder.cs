// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.NullEncoder
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class NullEncoder : IDbEncoder
  {
    private object nullValue;

    public NullEncoder(object nullValue) => this.nullValue = nullValue;

    public string Encode(object value, DbColumnInfo columnInfo)
    {
      if (this.nullValue.Equals(value))
        return SQL.Encode((object) null);
      return columnInfo != null ? columnInfo.Encode(value) : SQL.Encode(value);
    }

    public object Decode(object value)
    {
      return this.nullValue.Equals(value) ? (object) null : SQL.Decode(value, (object) null);
    }
  }
}
