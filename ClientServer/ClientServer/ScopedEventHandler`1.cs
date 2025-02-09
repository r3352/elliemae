// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ScopedEventHandler`1
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class ScopedEventHandler<ArgsT>
  {
    private readonly string _eventName;
    private readonly Dictionary<object, Stack<ScopedEventHandler<ArgsT>.EventHandlerT>> _mappedHandlers = new Dictionary<object, Stack<ScopedEventHandler<ArgsT>.EventHandlerT>>();
    private readonly List<Delegate> _originalHandlers = new List<Delegate>();
    private readonly ScopedEventHandler<ArgsT>.BeforeDelegateExecution _beforeEach;
    private readonly ScopedEventHandler<ArgsT>.AfterDelegateExecution _afterEach;
    private readonly bool _suppressLogs;

    private event ScopedEventHandler<ArgsT>.EventHandlerT _event;

    public ScopedEventHandler(string typeName, string eventName)
      : this(typeName, eventName, (ScopedEventHandler<ArgsT>.BeforeDelegateExecution) null, (ScopedEventHandler<ArgsT>.AfterDelegateExecution) null, false)
    {
    }

    public ScopedEventHandler(string typeName, string eventName, bool suppressLogs)
      : this(typeName, eventName, (ScopedEventHandler<ArgsT>.BeforeDelegateExecution) null, (ScopedEventHandler<ArgsT>.AfterDelegateExecution) null, suppressLogs)
    {
    }

    public ScopedEventHandler(
      string typeName,
      string eventName,
      ScopedEventHandler<ArgsT>.BeforeDelegateExecution beforeEach,
      ScopedEventHandler<ArgsT>.AfterDelegateExecution afterEach)
      : this(typeName, eventName, beforeEach, afterEach, false)
    {
    }

    public ScopedEventHandler(
      string typeName,
      string eventName,
      ScopedEventHandler<ArgsT>.BeforeDelegateExecution beforeEach,
      ScopedEventHandler<ArgsT>.AfterDelegateExecution afterEach,
      bool suppressLogs)
    {
      this._eventName = typeName + "." + eventName;
      this._beforeEach = beforeEach;
      this._afterEach = afterEach;
      this._suppressLogs = suppressLogs;
    }

    public MethodInfo Method => this._event?.Method;

    public void Add(ScopedEventHandler<ArgsT>.EventHandlerT handler)
    {
      lock (this._mappedHandlers)
      {
        Delegate target = handler.Target as Delegate;
        Stack<ScopedEventHandler<ArgsT>.EventHandlerT> eventHandlerTStack;
        if (!this._mappedHandlers.TryGetValue((object) target, out eventHandlerTStack))
        {
          eventHandlerTStack = new Stack<ScopedEventHandler<ArgsT>.EventHandlerT>();
          this._mappedHandlers.Add((object) target, eventHandlerTStack);
        }
        APICallContext contextAtRegister = this.GetApiCallContextForAddHandler();
        Type type = target.Target?.GetType();
        if ((object) type == null)
          type = target.Method.DeclaringType;
        Type targetType = type;
        ScopedEventHandler<ArgsT>.EventHandlerT eventHandlerT = (ScopedEventHandler<ArgsT>.EventHandlerT) ((sender, args) =>
        {
          DateTime utcNow1 = DateTime.UtcNow;
          using (APICallContext.CreateExecutionBlock((IApiSourceContext) contextAtRegister))
          {
            try
            {
              try
              {
                ScopedEventHandler<ArgsT>.BeforeDelegateExecution beforeEach = this._beforeEach;
                if (beforeEach != null)
                  beforeEach(targetType, utcNow1);
              }
              catch
              {
              }
              handler(sender, args);
            }
            finally
            {
              DateTime utcNow2 = DateTime.UtcNow;
              this.WriteLog((utcNow2 - utcNow1).TotalMilliseconds, 2000.0, contextAtRegister);
              try
              {
                ScopedEventHandler<ArgsT>.AfterDelegateExecution afterEach = this._afterEach;
                if (afterEach != null)
                  afterEach(targetType, utcNow1, utcNow2);
              }
              catch
              {
              }
            }
          }
        });
        this._originalHandlers.Insert(0, target);
        eventHandlerTStack.Push(eventHandlerT);
        this._event += eventHandlerT;
      }
    }

    private void WriteLog(
      double durationMS,
      double warnDurationMS,
      APICallContext contextAtRegister)
    {
      if (this._suppressLogs)
        return;
      try
      {
        Encompass.Diagnostics.Logging.LogLevel level = durationMS > warnDurationMS ? Encompass.Diagnostics.Logging.LogLevel.WARN.Force() : Encompass.Diagnostics.Logging.LogLevel.DEBUG;
        ILogger logger = DiagUtility.LogManager.GetLogger("ApplictionEvents");
        if (!logger.IsEnabled(level))
          return;
        LogFields info = new LogFields().Set<string>(Log.CommonFields.CallerEvent, this._eventName).Set<double>(Log.CommonFields.DurationMS, durationMS);
        if (contextAtRegister == null)
        {
          logger.Write(level, nameof (ScopedEventHandler<ArgsT>), string.Format("Executed listeners for {0} in {1}ms", (object) this._eventName, (object) durationMS), info);
        }
        else
        {
          info.Set<string>(Log.CommonFields.CallerCategory, contextAtRegister.SourceType.ToString()).Set<string>(Log.CommonFields.CallerModuleName, contextAtRegister.SourceApp).Set<string>(Log.CommonFields.CallerAssembly, contextAtRegister.SourceAssembly);
          logger.Write(level, nameof (ScopedEventHandler<ArgsT>), string.Format("Executed listener for {0} from {1}:{2} Assembly:{3} in {4}ms", (object) this._eventName, (object) contextAtRegister.SourceType, (object) contextAtRegister.SourceApp, (object) contextAtRegister.SourceAssembly, (object) durationMS), info);
        }
      }
      catch
      {
      }
    }

    public bool Remove(ScopedEventHandler<ArgsT>.EventHandlerT handler)
    {
      return this.Remove(handler, out int _);
    }

    public bool Remove(ScopedEventHandler<ArgsT>.EventHandlerT handler, out int count)
    {
      lock (this._mappedHandlers)
      {
        Delegate target = handler.Target as Delegate;
        Stack<ScopedEventHandler<ArgsT>.EventHandlerT> eventHandlerTStack;
        if (this._mappedHandlers.TryGetValue((object) target, out eventHandlerTStack))
        {
          this._event -= eventHandlerTStack.Pop();
          if (eventHandlerTStack.Count == 0)
            this._mappedHandlers.Remove((object) target);
          this._originalHandlers.Remove(target);
          count = this._originalHandlers.Count;
          return true;
        }
        count = this._originalHandlers.Count;
        return false;
      }
    }

    public bool Contains(ScopedEventHandler<ArgsT>.EventHandlerT handler)
    {
      lock (this._mappedHandlers)
        return this._originalHandlers.Contains(handler.Target as Delegate);
    }

    public void Invoke(object sender, ArgsT e)
    {
      DateTime utcNow = DateTime.UtcNow;
      ScopedEventHandler<ArgsT>.EventHandlerT eventHandlerT = this._event;
      if (eventHandlerT != null)
        eventHandlerT(sender, e);
      this.WriteLog((DateTime.UtcNow - utcNow).TotalSeconds, 10000.0, (APICallContext) null);
    }

    public bool IsNull() => this._event == null;

    public IList<Type> GetTargetTypes()
    {
      return (IList<Type>) this._originalHandlers.Select<Delegate, Type>((Func<Delegate, Type>) (originalHandler =>
      {
        Type type = originalHandler.Target?.GetType();
        return (object) type != null ? type : originalHandler.Method.DeclaringType;
      })).ToList<Type>();
    }

    private APICallContext GetApiCallContextForAddHandler()
    {
      IApiSourceContext current = APICallContext.GetCurrent();
      return current != null ? new APICallContext(current.SourceApp, current.SourceAssembly, current.SourceType, this._eventName) : (APICallContext) null;
    }

    public delegate void EventHandlerT(object sender, ArgsT e);

    public delegate void BeforeDelegateExecution(Type target, DateTime start);

    public delegate void AfterDelegateExecution(Type target, DateTime start, DateTime end);
  }
}
