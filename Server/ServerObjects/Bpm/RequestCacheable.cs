// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.RequestCacheable
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common.Aspects.Cache;
using EllieMae.EMLite.Cache;
using PostSharp.Aspects;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  [Serializable]
  public class RequestCacheable : Cacheable
  {
    protected override IDataCache CreateDataCache(MethodExecutionArgs args)
    {
      return ClientContext.CurrentRequest == null ? (IDataCache) null : ClientContext.CurrentRequest.RequestCache;
    }
  }
}
