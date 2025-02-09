// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.JsonSerializer`1
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Newtonsoft.Json;

#nullable disable
namespace Elli.Common.Serialization
{
  public class JsonSerializer<T>
  {
    public string Serialize(T obj)
    {
      return this.Serialize(obj, Formatting.None, (JsonSerializerSettings) null);
    }

    public string Serialize(T obj, Formatting formatting, JsonSerializerSettings settings)
    {
      return JsonConvert.SerializeObject((object) obj, formatting, settings);
    }

    public T Deserialize(string jsonText)
    {
      return this.Deserialize(jsonText, (JsonSerializerSettings) null);
    }

    public T Deserialize(string jsonText, JsonSerializerSettings settings)
    {
      return JsonConvert.DeserializeObject<T>(jsonText, settings);
    }
  }
}
