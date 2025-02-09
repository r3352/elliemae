// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.JSonObjects.StatusConverter
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.JSonObjects
{
  public class StatusConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      serializer.Serialize(writer, value);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      switch (reader.TokenType)
      {
        case JsonToken.String:
          if (string.Compare((string) reader.Value, "Accepted", true) == 0)
            return (object) true;
          return string.Compare((string) reader.Value, "Declined", true) == 0 ? (object) false : (object) null;
        case JsonToken.Boolean:
          return reader.Value;
        default:
          throw new JsonReaderException("StatusConverter class: Expected boolean or empty string.");
      }
    }

    public override bool CanConvert(Type objectType) => objectType == typeof (bool);
  }
}
