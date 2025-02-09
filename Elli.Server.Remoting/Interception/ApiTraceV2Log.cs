// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Interception.ApiTraceV2Log
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Encompass.Diagnostics.Logging.Formatters;
using Encompass.Diagnostics.Logging.Schema;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.Server.Remoting.Interception
{
  public class ApiTraceV2Log : Log
  {
    public ApiTraceV2Log()
    {
    }

    protected ApiTraceV2Log(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public override bool SupportsFormatter(LogFormat logFormat)
    {
      return logFormat == LogFormat.Json || logFormat == LogFormat.PrettyJson;
    }
  }
}
