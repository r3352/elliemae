// Decompiled with JetBrains decompiler
// Type: Jose.IJsonMapper
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

#nullable disable
namespace Jose
{
  public interface IJsonMapper
  {
    string Serialize(object obj);

    T Parse<T>(string json);
  }
}
