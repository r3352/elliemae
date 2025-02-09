// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TracingManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TracingManager : ITracingManager
  {
    private const string className = "TracingManager�";
    private const int minTraceLevel = 1;
    private const int maxTraceLevel = 4;
    private ArrayList[] traceListeners = new ArrayList[5];
    private bool isListening;
    private IClientContext context;

    public TracingManager(IClientContext context)
    {
      this.context = context;
      for (int index = 1; index <= 4; ++index)
        this.traceListeners[index] = new ArrayList(1);
    }

    public void RegisterListener(TraceLevel traceLevel, IClientSession session)
    {
      lock (this.traceListeners)
      {
        int val1 = (int) traceLevel;
        for (int index = 1; index <= 4; ++index)
        {
          if (this.traceListeners[index].Contains((object) session))
            this.traceListeners[index].Remove((object) session);
        }
        for (int index = 1; index <= Math.Min(val1, 4); ++index)
          this.traceListeners[index].Add((object) session);
        if (val1 < 1 || this.isListening)
          return;
        this.isListening = true;
      }
    }

    public void UnregisterListener(IClientSession session)
    {
      lock (this.traceListeners)
      {
        bool flag = false;
        for (int index = 1; index <= 4; ++index)
        {
          if (this.traceListeners[index].Contains((object) session))
            this.traceListeners[index].Remove((object) session);
          if (this.traceListeners[index].Count > 0)
            flag = true;
        }
        if (flag || !this.isListening)
          return;
        this.isListening = false;
      }
    }
  }
}
