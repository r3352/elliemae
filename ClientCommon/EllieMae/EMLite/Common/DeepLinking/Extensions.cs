// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DeepLinking.Extensions
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common.DeepLinking
{
  public static class Extensions
  {
    public static string Combine(this string uri, params string[] segments)
    {
      if (string.IsNullOrWhiteSpace(uri))
        return (string) null;
      return segments == null || segments.Length == 0 ? uri : ((IEnumerable<string>) segments).Aggregate<string, string>(uri, (Func<string, string, string>) ((current, segment) => current.TrimEnd('/') + "/" + segment.TrimStart('/')));
    }
  }
}
