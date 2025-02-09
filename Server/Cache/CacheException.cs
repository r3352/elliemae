// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Cache.CacheException
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.Cache
{
  [Serializable]
  internal class CacheException : ApplicationException
  {
    internal CacheException(string message)
      : base(message)
    {
    }

    protected CacheException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
