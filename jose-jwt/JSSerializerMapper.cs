// Decompiled with JetBrains decompiler
// Type: Jose.JSSerializerMapper
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.Web.Script.Serialization;

#nullable disable
namespace Jose
{
  public class JSSerializerMapper : IJsonMapper
  {
    private static JavaScriptSerializer js;

    private JavaScriptSerializer JS
    {
      get => JSSerializerMapper.js ?? (JSSerializerMapper.js = new JavaScriptSerializer());
    }

    public string Serialize(object obj) => this.JS.Serialize(obj);

    public T Parse<T>(string json) => this.JS.Deserialize<T>(json);
  }
}
