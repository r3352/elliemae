// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.LogConverter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class LogConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType) => typeof (Log).IsAssignableFrom(objectType);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      Log instance = Activator.CreateInstance(objectType) as Log;
      while (reader.Read() && reader.TokenType != JsonToken.EndObject)
      {
        ILogField key = reader.TokenType == JsonToken.PropertyName ? LogFields.GetForDeserialize(reader.Value.ToString()) : throw new JsonSerializationException("Can not read Log at " + reader.Path);
        reader.Read();
        object obj = serializer.Deserialize(reader, key.Type);
        instance.SetInternal(key, obj);
      }
      return (object) instance;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value is Log log)
      {
        writer.WriteStartObject();
        foreach (ILogField key in log.GetKeys())
        {
          writer.WritePropertyName(key.Name);
          serializer.Serialize(writer, log.GetInternal(key));
        }
        writer.WriteEndObject();
      }
      else
        writer.WriteNull();
    }
  }
}
