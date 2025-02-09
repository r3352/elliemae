// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbColumnInfo
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  [Serializable]
  public class DbColumnInfo : ISerializable
  {
    private const string className = "DbColumnInfo";
    private DataColumn columnInfo;

    public DbColumnInfo(DataColumn columnInfo) => this.columnInfo = columnInfo;

    private DbColumnInfo(SerializationInfo info, StreamingContext context)
    {
      string columnName = info.GetString("ColumnName");
      int int32 = info.GetInt32(nameof (MaxLength));
      Type dataType = JsonConvert.DeserializeObject<Type>(info.GetString(nameof (DataType)));
      bool boolean1 = info.GetBoolean("AllowDBNull");
      bool boolean2 = info.GetBoolean(nameof (AutoIncrement));
      string tableName = info.GetString("TableName");
      this.columnInfo = new DataColumn(columnName, dataType);
      this.columnInfo.MaxLength = int32;
      this.columnInfo.AllowDBNull = boolean1;
      this.columnInfo.AutoIncrement = boolean2;
      new DataTable(tableName).Columns.Add(this.columnInfo);
    }

    public string Name => this.columnInfo.ColumnName;

    public int MaxLength => this.columnInfo.MaxLength;

    public Type DataType => this.columnInfo.DataType;

    public bool AllowNull => this.columnInfo.AllowDBNull;

    public bool AutoIncrement => this.columnInfo.AutoIncrement;

    public string FullName => this.columnInfo.Table.TableName + "." + this.Name;

    public object NullValue
    {
      get
      {
        if (this.AllowNull)
          return (object) null;
        if (this.DataType == typeof (string))
          return (object) "";
        return this.DataType == typeof (DateTime) ? (object) DateTime.MinValue : (object) 0;
      }
    }

    public string SizeToFit(string value)
    {
      if (value == null)
        return (string) null;
      if (this.MaxLength <= 0 || value.Length <= this.MaxLength)
        return value;
      string message = "Truncating data value \"" + value + "\" which exceeds maximum field length " + (object) this.MaxLength + " for field " + this.columnInfo.Table.TableName + "." + this.columnInfo.ColumnName;
      if (ServerGlobals.CurrentContextTraceLog != null)
        ServerGlobals.CurrentContextTraceLog.Write(TraceLevel.Warning, nameof (DbColumnInfo), message);
      if (ServerGlobals.TraceLog != null)
        ServerGlobals.TraceLog.WriteWarningI(nameof (DbColumnInfo), message);
      return value.Substring(0, this.MaxLength);
    }

    public string Encode(object value)
    {
      if ((value == null || value is DBNull) && this.AllowNull)
        return SQL.Encode(this.NullValue);
      if (this.DataType == typeof (byte[]) && value is byte[] bytes)
        return "0x" + HexEncoding.Instance.GetString(bytes);
      if (this.DataType == typeof (DateTime))
        return SQL.EncodeDateTime(Utils.ParseDate(value, DateTime.MinValue), DateTime.MinValue);
      if (this.DataType == typeof (Decimal))
        return SQL.Encode((object) Utils.ParseDecimal(value, Decimal.MinValue), (object) Decimal.MinValue);
      if (this.DataType == typeof (int))
        return SQL.Encode((object) Utils.ParseInt(value, int.MinValue), (object) int.MinValue);
      if (this.DataType == typeof (long))
        return SQL.Encode((object) Utils.ParseLong(value, long.MinValue), (object) long.MinValue);
      if (this.DataType == typeof (double))
        return SQL.Encode((object) Utils.ParseDouble(value, double.MinValue), (object) double.MinValue);
      if (this.DataType == typeof (float))
        return SQL.Encode((object) Utils.ParseSingle(value, float.MinValue), (object) float.MinValue);
      if (this.DataType == typeof (object) && value is DateTime)
        return "CAST(" + SQL.EncodeDateTime(Utils.ParseDate(value, DateTime.MinValue), DateTime.MinValue) + " as DateTime)";
      return this.MaxLength >= 0 && value is string ? SQL.EncodeString(this.SizeToFit((string) value)) : SQL.Encode(value);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("ColumnName", (object) this.columnInfo.ColumnName);
      info.AddValue("MaxLength", this.columnInfo.MaxLength);
      string str = JsonConvert.SerializeObject((object) this.columnInfo.DataType);
      info.AddValue("DataType", (object) str);
      info.AddValue("AllowDBNull", this.columnInfo.AllowDBNull);
      info.AddValue("AutoIncrement", this.columnInfo.AutoIncrement);
      info.AddValue("TableName", (object) this.columnInfo.Table.TableName);
    }
  }
}
