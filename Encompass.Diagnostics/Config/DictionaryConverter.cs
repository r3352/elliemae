// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.DictionaryConverter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public class DictionaryConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof (Dictionary<,>);
    }

    public override bool CanWrite => false;

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      object instance = Activator.CreateInstance(objectType, existingValue);
      serializer.Populate(reader, instance);
      return instance;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
