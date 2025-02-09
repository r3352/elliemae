// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Bridges.ModuleBridge
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

#nullable disable
namespace Elli.Web.Host.SSF.Bridges
{
  public class ModuleBridge : Bridge
  {
    private const string className = "ModuleBridge";
    private static readonly string sw = Tracing.SwThinThick;

    internal ModuleBridge(SSFContext context)
      : base(context)
    {
    }

    public string getCapabilities()
    {
      Tracing.Log(ModuleBridge.sw, TraceLevel.Verbose, nameof (ModuleBridge), "Entering 'getCapabilities'");
      throw new NotImplementedException();
    }

    public string getParameters()
    {
      Tracing.Log(ModuleBridge.sw, TraceLevel.Verbose, nameof (ModuleBridge), "Entering 'getParameters'");
      try
      {
        return JsonConvert.SerializeObject((object) this.context.parameters);
      }
      catch (Exception ex)
      {
        Tracing.Log(ModuleBridge.sw, TraceLevel.Error, nameof (ModuleBridge), "Error occured 'getParameters': " + ex.Message);
        throw;
      }
    }

    public void unload()
    {
      Tracing.Log(ModuleBridge.sw, TraceLevel.Verbose, nameof (ModuleBridge), "Entering 'unload'");
      try
      {
        if (this.context.unloadHandler == null)
          return;
        this.context.unloadHandler();
      }
      catch (Exception ex)
      {
        Tracing.Log(ModuleBridge.sw, TraceLevel.Error, nameof (ModuleBridge), "Error occured 'unload': " + ex.Message);
        throw;
      }
    }

    public void IsDirty(bool isDirty, string formName)
    {
      Tracing.Log(ModuleBridge.sw, nameof (ModuleBridge), TraceLevel.Info, "IsDirty: " + isDirty.ToString());
      try
      {
        switch (formName)
        {
          case "LockComparisonSetting":
            Session.Application.GetService<ILockComparisonConsole>().SetIsDirty(isDirty);
            break;
          case "BidTapeTemplate":
            Session.Application.GetService<INormalizedBidTapeTemplate>().SetIsDirty(isDirty);
            break;
          case "ConfigurableWorkflowTemplate":
            Session.Application.GetService<IConfigurableWorkFlow>().SetIsDirty(isDirty);
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ModuleBridge.sw, nameof (ModuleBridge), TraceLevel.Error, "IsDirty failed: " + ex.ToString());
        throw;
      }
    }

    public void log(string message, string logLevel)
    {
      TraceLevel l = TraceLevel.Info;
      switch (logLevel.ToLower())
      {
        case "info":
          l = TraceLevel.Info;
          break;
        case "off":
          l = TraceLevel.Off;
          break;
        case "error":
          l = TraceLevel.Error;
          break;
        case "warning":
          l = TraceLevel.Warning;
          break;
        case "verbose":
          l = TraceLevel.Verbose;
          break;
      }
      Tracing.Log(ModuleBridge.sw, nameof (ModuleBridge), l, message);
    }
  }
}
