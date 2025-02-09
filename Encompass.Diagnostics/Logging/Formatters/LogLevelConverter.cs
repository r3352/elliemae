// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.LogLevelConverter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class LogLevelConverter : StringEnumConverter
  {
    public override bool CanConvert(Type objectType) => typeof (Encompass.Diagnostics.Logging.LogLevel).Equals(objectType);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      Encompass.Diagnostics.Logging.LogLevel result;
      return (object) (Encompass.Diagnostics.Logging.LogLevel) (Enum.TryParse<Encompass.Diagnostics.Logging.LogLevel>(reader.Value?.ToString(), out result) ? (int) result : 0);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value is Encompass.Diagnostics.Logging.LogLevel && ((int) value & 1) == 1 && (int) value > 1)
        base.WriteJson(writer, (object) (Encompass.Diagnostics.Logging.LogLevel) ((int) value ^ 1), serializer);
      else
        base.WriteJson(writer, value, serializer);
    }
  }
}
