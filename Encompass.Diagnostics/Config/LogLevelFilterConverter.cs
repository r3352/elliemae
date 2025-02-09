// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.LogLevelFilterConverter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public class LogLevelFilterConverter : StringEnumConverter
  {
    public override bool CanConvert(Type objectType) => typeof (LogLevelFilter).Equals(objectType);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      object obj = reader.Value;
      if (!(obj is string))
        return base.ReadJson(reader, objectType, existingValue, serializer);
      LogLevelFilter logLevelFilter = LogLevelFilter.Off;
      string str1 = (obj as string).Trim();
      char[] chArray = new char[1]{ '|' };
      foreach (string str2 in str1.Split(chArray))
      {
        LogLevelFilter result;
        if (Enum.TryParse<LogLevelFilter>(str2, true, out result))
          logLevelFilter |= result;
      }
      return (object) logLevelFilter;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotSupportedException("Write is not supported.");
    }
  }
}
