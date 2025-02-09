// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Caching.ICacheProvider
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

#nullable disable
namespace Elli.Service.Common.Caching
{
  public interface ICacheProvider
  {
    ICache BuildCache(string region);
  }
}
