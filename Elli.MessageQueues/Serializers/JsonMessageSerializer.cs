// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Serializers.JsonMessageSerializer
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters;
using System.Text;

#nullable disable
namespace Elli.MessageQueues.Serializers
{
  [ExcludeFromCodeCoverage]
  public class JsonMessageSerializer : ISerializer
  {
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
    {
      TypeNameHandling = TypeNameHandling.Auto,
      TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
    };

    public byte[] Serialize<T>(T message)
    {
      return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) message, Formatting.Indented, this._settings));
    }

    public T Deserialize<T>(byte[] bytes)
    {
      return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes), this._settings);
    }
  }
}
