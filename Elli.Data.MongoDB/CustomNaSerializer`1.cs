// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.CustomNaSerializer`1
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using MongoDB.Bson.Serialization;
using System;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class CustomNaSerializer<T> : IBsonSerializer where T : struct, IComparable<T>
  {
    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
      string stringValue = context.Reader.ReadString();
      if (stringValue == "null")
        stringValue = (string) null;
      return (object) NA<T>.Parse(stringValue);
    }

    public void Serialize(
      BsonSerializationContext context,
      BsonSerializationArgs args,
      object value)
    {
      string str = value.ToString();
      context.Writer.WriteString(str ?? "null");
    }

    public Type ValueType => typeof (NA<>);
  }
}
