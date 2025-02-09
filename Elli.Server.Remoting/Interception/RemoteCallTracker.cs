// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Interception.RemoteCallTracker
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Diagnostics;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Unity.Interception.PolicyInjection.Pipeline;

#nullable disable
namespace Elli.Server.Remoting.Interception
{
  public class RemoteCallTracker : IDisposable, ILogicalThreadAffinative
  {
    private const string className = "RemoteCallTracker";
    private readonly IApiSourceContext _srcContext;
    private readonly ILogger _logger;
    private readonly MethodBase _method;
    private readonly object _target;
    private readonly IDictionary<string, object> _arguments;
    private readonly DateTime _start;
    private readonly List<IRemoteCallLogDecorator> _logDecorators;
    private ISession _session;
    private RemoteCallTracker.MethodReturn _methodReturn;

    private RemoteCallTracker(
      IApiSourceContext srcContext,
      ILogger logger,
      MethodBase method,
      object target,
      IDictionary<string, object> arguments)
    {
      this._srcContext = srcContext;
      this._logger = logger;
      this._method = method;
      this._target = target;
      this._arguments = arguments;
      this._start = DateTime.UtcNow;
      this._logDecorators = new List<IRemoteCallLogDecorator>();
      CallContext.SetData(nameof (RemoteCallTracker), (object) new RemoteCallTracker.RemoteCallTrackerWrapper()
      {
        Value = this
      });
    }

    public void Dispose()
    {
      try
      {
        ApiTraceV2Log log = new ApiTraceV2Log();
        Dictionary<string, object> source = new Dictionary<string, object>();
        foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>) this._arguments)
        {
          source[keyValuePair.Key] = (object) new RemoteMethodParameter(keyValuePair.Value);
          if (keyValuePair.Value is IRemoteCallLogDecorator callLogDecorator)
            this._logDecorators.Add(callLogDecorator);
        }
        if (this._target is IRemoteCallLogDecorator target)
          this._logDecorators.Add(target);
        bool flag;
        if (this._methodReturn != null)
        {
          if (this._methodReturn.Exception != null)
          {
            flag = false;
            log.Error = new LogErrorData(this._methodReturn.Exception);
          }
          else
            flag = true;
        }
        else
          flag = false;
        if (this._methodReturn.ReturnValue is IRemoteCallLogDecorator returnValue)
          this._logDecorators.Add(returnValue);
        if (this._methodReturn.Outputs != null)
        {
          foreach (object obj in (IEnumerable<object>) this._methodReturn.Outputs.Values)
          {
            if (obj is IRemoteCallLogDecorator callLogDecorator)
              this._logDecorators.Add(callLogDecorator);
          }
        }
        foreach (IRemoteCallLogDecorator logDecorator in this._logDecorators)
          logDecorator.Decorate((Log) log);
        log.Src = nameof (RemoteCallTracker);
        log.Level = flag ? Encompass.Diagnostics.Logging.LogLevel.DEBUG : Encompass.Diagnostics.Logging.LogLevel.ERROR;
        string str1;
        if (this._method is MethodInfo method)
        {
          string str2 = string.Join(",", ((IEnumerable<ParameterInfo>) method.GetParameters()).Select<ParameterInfo, string>((Func<ParameterInfo, string>) (o => string.Format("{0} {1}", (object) o.ParameterType, (object) o.Name))));
          str1 = string.Format("{0} {1}.{2}({3})", (object) method.ReturnType, (object) method.ReflectedType, (object) method.Name, (object) str2);
        }
        else
          str1 = this._method.ToString();
        log.Set<string>(Log.CommonFields.ApiPathTemplate, str1);
        if (this._session != null)
        {
          log.UserId = this._session.UserID;
          log.Set<string>(Log.CommonFields.CallerAppName, this._session.LoginParams.AppName);
          log.SessionId = this._session.SessionID;
        }
        if (this._srcContext != null)
        {
          log.Set<string>(Log.CommonFields.CallerCategory, this._srcContext.SourceType.ToString());
          log.Set<string>(Log.CommonFields.CallerModuleName, this._srcContext.SourceApp);
          log.Set<string>(Log.CommonFields.CallerAssembly, this._srcContext.SourceAssembly);
          log.Set<string>(Log.CommonFields.CallerEvent, this._srcContext.SourceEvent);
        }
        if (source.Any<KeyValuePair<string, object>>())
          log.Set<Dictionary<string, object>>(Log.CommonFields.ApiRequestParameters, source);
        log.Message = "Remote Call " + (flag ? "Completed" : "Errored");
        DateTime utcNow = DateTime.UtcNow;
        log.Set<DateTime>(Log.CommonFields.StartTime, this._start);
        log.Set<DateTime>(Log.CommonFields.EndTime, utcNow);
        log.Set<double>(Log.CommonFields.DurationMS, (utcNow - this._start).TotalMilliseconds);
        this._logger.Write<ApiTraceV2Log>(log);
        CallContext.FreeNamedDataSlot(nameof (RemoteCallTracker));
      }
      catch
      {
      }
    }

    public void SetMethodReturn(IMethodReturn methodReturn)
    {
      this._methodReturn = new RemoteCallTracker.MethodReturn();
      this._methodReturn.ReturnValue = methodReturn.ReturnValue;
      this._methodReturn.Exception = methodReturn.Exception;
      if (methodReturn.Outputs == null)
        return;
      this._methodReturn.Outputs = (IDictionary<string, object>) new Dictionary<string, object>();
      for (int index = 0; index < this._methodReturn.Outputs.Count; ++index)
      {
        string str = methodReturn.Outputs.ParameterName(index);
        this._methodReturn.Outputs[str] = methodReturn.Outputs[str];
      }
    }

    public void SetSuccessMethodReturn(object returnValue, Dictionary<string, object> outputs)
    {
      this._methodReturn = new RemoteCallTracker.MethodReturn();
      this._methodReturn.ReturnValue = returnValue;
      this._methodReturn.Outputs = (IDictionary<string, object>) outputs;
    }

    public void SetFailureMethodReturn(Exception exception)
    {
      this._methodReturn = new RemoteCallTracker.MethodReturn();
      this._methodReturn.Exception = exception;
    }

    public void SetSession(ISession session) => this._session = session;

    public void AddDecorator(IRemoteCallLogDecorator decorator)
    {
      this._logDecorators.Add(decorator);
    }

    public static RemoteCallTracker New(
      ISession session,
      IRequestContext request,
      IMethodInvocation input)
    {
      if (request != null && request.ThreadNestLevel > 1)
        return (RemoteCallTracker) null;
      ILogger logger;
      if (!RemoteCallTracker.TryGetLogger(out logger))
        return (RemoteCallTracker) null;
      if (RemoteCallTracker.GetCurrent() != null)
        return (RemoteCallTracker) null;
      IApiSourceContext current = APICallContext.GetCurrent();
      Dictionary<string, object> arguments = new Dictionary<string, object>();
      for (int index = 0; index < input.Arguments.Count; ++index)
      {
        string str = input.Arguments.ParameterName(index);
        object obj = input.Arguments[str];
        arguments[str] = obj;
      }
      RemoteCallTracker remoteCallTracker = new RemoteCallTracker(current, logger, input.MethodBase, input.Target, (IDictionary<string, object>) arguments);
      if (session != null)
        remoteCallTracker.SetSession(session);
      return remoteCallTracker;
    }

    private static bool TryGetLogger(out ILogger logger)
    {
      logger = DiagUtility.LogManager.GetLogger("APITrace.V2");
      if (logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG))
        return true;
      logger = (ILogger) null;
      return false;
    }

    public static RemoteCallTracker GetCurrent()
    {
      return !(CallContext.GetData(nameof (RemoteCallTracker)) is RemoteCallTracker.RemoteCallTrackerWrapper data) ? (RemoteCallTracker) null : data.Value;
    }

    [Serializable]
    private class RemoteCallTrackerWrapper : ILogicalThreadAffinative
    {
      [NonSerialized]
      public RemoteCallTracker Value;
    }

    public class MethodReturn
    {
      public Exception Exception { get; set; }

      public object ReturnValue { get; set; }

      public IDictionary<string, object> Outputs { get; set; }
    }
  }
}
