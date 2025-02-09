// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.CustomDecimalSerializer
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using MongoDB.Bson.Serialization;
using System;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class CustomDecimalSerializer : IBsonSerializer
  {
    private readonly Decimal _conversionConstant;

    public CustomDecimalSerializer(DecimalPlace decimalPlace)
    {
      switch (decimalPlace)
      {
        case DecimalPlace.Six:
          this._conversionConstant = 1000000M;
          break;
        case DecimalPlace.Ten:
          this._conversionConstant = 10000000000M;
          break;
        default:
          this._conversionConstant = 1000000M;
          break;
      }
    }

    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
      return (object) ((Decimal) context.Reader.ReadInt64() / this._conversionConstant);
    }

    public void Serialize(
      BsonSerializationContext context,
      BsonSerializationArgs args,
      object value)
    {
      long num = this.MaxLongVariableCapacityForMongoDb(value);
      context.Writer.WriteInt64(num);
    }

    public Type ValueType => typeof (Decimal?);

    private long MaxLongVariableCapacityForMongoDb(object value)
    {
      Decimal num1 = Convert.ToDecimal(value) * this._conversionConstant;
      if (num1 <= 9223372036854775807M)
        return (long) num1;
      object obj = value;
      Decimal num2;
      do
      {
        obj = (object) (Convert.ToDecimal(obj) / 10M);
        num2 = Convert.ToDecimal(obj) * this._conversionConstant;
      }
      while (num2 > 9223372036854775807M);
      return (long) num2;
    }
  }
}
