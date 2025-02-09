// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldEvents
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class FieldEvents
  {
    private static readonly string sw = Tracing.SwDataEngine;
    private const string className = "FieldEvents";
    [ThreadStatic]
    private static Stack<string> eventStack = (Stack<string>) null;

    public event Routine CalProc;

    public event Routine CustomCalProc;

    public event Validation ValidationProc;

    public void RaiseCalProc(string id, string val)
    {
      if (this.CalProc == null)
        return;
      if (FieldEvents.eventStack == null)
        FieldEvents.eventStack = new Stack<string>();
      DateTime now = DateTime.Now;
      if (Tracing.IsSwitchActive(FieldEvents.sw, TraceLevel.Verbose))
        Tracing.Log(FieldEvents.sw, TraceLevel.Verbose, nameof (FieldEvents), "Internal field calculation sequence start, id: " + id + " (Stack: " + string.Join(" -> ", FieldEvents.eventStack.ToArray()) + ")");
      FieldEvents.eventStack.Push(id);
      try
      {
        this.CalProc(id, val);
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldEvents.sw, TraceLevel.Warning, nameof (FieldEvents), "Internal field calculation sequence error, id: " + id + ": " + (object) ex);
        throw;
      }
      finally
      {
        if (FieldEvents.eventStack.Count > 0)
          FieldEvents.eventStack.Pop();
        if (Tracing.IsSwitchActive(FieldEvents.sw, TraceLevel.Verbose))
          Tracing.Log(FieldEvents.sw, TraceLevel.Verbose, nameof (FieldEvents), "Internal field calculation sequence end, id: " + id + ", duration = " + (DateTime.Now - now).TotalMilliseconds.ToString("0") + "ms");
      }
    }

    public void RaiseCustomCalProc(string id, string val)
    {
      if (this.CustomCalProc == null)
        return;
      if (FieldEvents.eventStack == null)
        FieldEvents.eventStack = new Stack<string>();
      DateTime now = DateTime.Now;
      string str = id.IndexOf("#") > -1 ? id.Substring(0, id.IndexOf("#")) : id;
      if (Tracing.IsSwitchActive(FieldEvents.sw, TraceLevel.Verbose))
        Tracing.Log(FieldEvents.sw, TraceLevel.Verbose, nameof (FieldEvents), "Field custom event sequence start, id: " + str + " (Stack: " + string.Join(" -> ", FieldEvents.eventStack.ToArray()) + ")");
      FieldEvents.eventStack.Push("@" + str);
      try
      {
        this.CustomCalProc(id, val);
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldEvents.sw, TraceLevel.Warning, nameof (FieldEvents), "Field custom event sequence error, id: " + str + ": " + (object) ex);
      }
      finally
      {
        if (FieldEvents.eventStack.Count > 0)
          FieldEvents.eventStack.Pop();
        if (Tracing.IsSwitchActive(FieldEvents.sw, TraceLevel.Verbose))
          Tracing.Log(FieldEvents.sw, TraceLevel.Verbose, nameof (FieldEvents), "Field custom event sequence end, id: " + str + ", duration = " + (DateTime.Now - now).TotalMilliseconds.ToString("0") + "ms");
      }
    }

    public bool PerformValidationProc(string id, string val)
    {
      if (this.ValidationProc == null)
        return true;
      if (FieldEvents.eventStack == null)
        FieldEvents.eventStack = new Stack<string>();
      DateTime now = DateTime.Now;
      if (Tracing.IsSwitchActive(FieldEvents.sw, TraceLevel.Verbose))
        Tracing.Log(FieldEvents.sw, TraceLevel.Verbose, nameof (FieldEvents), "Internal field validation sequence start, id: " + id + " (Stack: " + string.Join(" -> ", FieldEvents.eventStack.ToArray()) + ")");
      FieldEvents.eventStack.Push(id);
      try
      {
        return this.ValidationProc(id, val);
      }
      catch (Exception ex)
      {
        Tracing.Log(FieldEvents.sw, TraceLevel.Warning, nameof (FieldEvents), "Internal field validation sequence error, id: " + id + ": " + (object) ex);
        throw;
      }
      finally
      {
        if (FieldEvents.eventStack.Count > 0)
          FieldEvents.eventStack.Pop();
        if (Tracing.IsSwitchActive(FieldEvents.sw, TraceLevel.Verbose))
          Tracing.Log(FieldEvents.sw, TraceLevel.Verbose, nameof (FieldEvents), "Internal field validation sequence end, id: " + id + ", duration = " + (DateTime.Now - now).TotalMilliseconds.ToString("0") + "ms");
      }
    }
  }
}
