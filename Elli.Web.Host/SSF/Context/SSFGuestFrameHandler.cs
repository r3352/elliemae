// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Context.SSFGuestFrameHandler
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Frames;
using DotNetBrowser.Js;
using EllieMae.EMLite.Common;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable disable
namespace Elli.Web.Host.SSF.Context
{
  internal class SSFGuestFrameHandler
  {
    private const string className = "SSFGuestFrameHandler";
    private static readonly string sw = Tracing.SwThinThick;
    private IFrame guestFrame;

    internal static SSFGuestFrameHandler Initialize(IFrame guestFrame)
    {
      Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Entering Initialize");
      try
      {
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Getting 'window' object from iframe");
        IJsObject result1 = guestFrame.ExecuteJavaScript<IJsObject>("window")?.Result;
        if (result1 == null)
        {
          Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Error, nameof (SSFGuestFrameHandler), "Failed to get 'window' object from iframe");
          return (SSFGuestFrameHandler) null;
        }
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Creating SSFGuestEventHandler");
        SSFGuestFrameHandler guestFrameHandler = new SSFGuestFrameHandler(guestFrame);
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Storing SSFGuestEventHandler in 'window' object");
        result1.Properties["ssfGuestFrameHandler"] = (object) guestFrameHandler;
        string javaScript = "window.addEventListener(\"beforeunload\", function() {window.ssfGuestFrameHandler.beforeUnload();});";
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Adding event listener for 'window.beforeunload' event");
        Task<object> task = guestFrame.ExecuteJavaScript<object>(javaScript);
        if (task != null)
        {
          object result2 = task.Result;
        }
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Event listener successfully added");
        return guestFrameHandler;
      }
      catch (Exception ex)
      {
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Error, nameof (SSFGuestFrameHandler), "Failed to Initialize: " + ex.ToString());
        return (SSFGuestFrameHandler) null;
      }
    }

    private SSFGuestFrameHandler(IFrame guestFrame) => this.guestFrame = guestFrame;

    public bool beforeUnload()
    {
      Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "SSFGuestFrameEventHandler: beforeUnload");
      bool flag = false;
      Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Checking Frame.Parent.IsDisposed");
      if (this.guestFrame.Parent != null && !this.guestFrame.Parent.IsDisposed)
      {
        string javaScript = "resetGuest";
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Calling Browser.ExecuteJavaScriptAndReturnValue: " + javaScript);
        IJsFunction result = this.guestFrame.Parent.ExecuteJavaScript<IJsFunction>(javaScript)?.Result;
        Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Checking Browser.ExecuteJavaScriptAndReturnValue Result");
        if (result != null)
        {
          Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Calling Function.Invoke");
          result.Invoke((IJsObject) null);
          Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Verbose, nameof (SSFGuestFrameHandler), "Completed Function.Invoke");
          flag = true;
        }
        else
          Tracing.Log(SSFGuestFrameHandler.sw, TraceLevel.Warning, nameof (SSFGuestFrameHandler), "Unable to find " + javaScript);
      }
      return flag;
    }
  }
}
