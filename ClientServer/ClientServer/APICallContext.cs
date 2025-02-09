// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.APICallContext
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Remoting.Messaging;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class APICallContext : IApiSourceContext, ILogicalThreadAffinative
  {
    public const string Key = "APICall�";

    public APICallContext(
      string app,
      string assembly,
      APICallSourceType sourceType,
      string sourceEvent)
    {
      this.SourceApp = app;
      this.SourceAssembly = assembly;
      this.SourceType = sourceType;
      this.SourceEvent = sourceEvent;
    }

    public string SourceApp { get; }

    public string SourceAssembly { get; }

    public APICallSourceType SourceType { get; }

    public string SourceEvent { get; }

    public static IApiSourceContext GetCurrent()
    {
      return CallContext.GetData("APICall") as IApiSourceContext;
    }

    public static void SetAsCurrent(IApiSourceContext context)
    {
      if (context != null)
        CallContext.SetData("APICall", (object) context);
      else
        CallContext.FreeNamedDataSlot("APICall");
    }

    public static IDisposable CreateExecutionBlock(IApiSourceContext context)
    {
      return (IDisposable) new APICallContext.ExecutionBlock(context);
    }

    private class ExecutionBlock : IDisposable
    {
      private IApiSourceContext previousContext;
      private bool contextReplaced;

      public ExecutionBlock(IApiSourceContext newContext)
      {
        if (newContext != null)
        {
          this.contextReplaced = true;
          this.previousContext = APICallContext.GetCurrent();
          APICallContext.SetAsCurrent(newContext);
        }
        else
          this.contextReplaced = false;
      }

      public void Dispose()
      {
        if (!this.contextReplaced)
          return;
        APICallContext.SetAsCurrent(this.previousContext);
      }
    }
  }
}
