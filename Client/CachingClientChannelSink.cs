// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.CachingClientChannelSink
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using EllieMae.EMLite.ClientServer.Cache;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class CachingClientChannelSink : 
    IClientFormatterSink,
    IMessageSink,
    IClientChannelSink,
    IChannelSinkBase
  {
    private readonly IClientFormatterSink _innerSink;
    private readonly ConcurrentDictionary<string, string> _systemIds;

    public CachingClientChannelSink(
      IClientFormatterSink innerSink,
      ConcurrentDictionary<string, string> systemIds)
    {
      this._innerSink = innerSink;
      this._systemIds = systemIds;
    }

    public IClientChannelSink NextChannelSink => this._innerSink?.NextChannelSink;

    public IDictionary Properties => this._innerSink?.Properties;

    public IMessageSink NextSink => this._innerSink?.NextSink;

    public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
    {
      return this._innerSink?.AsyncProcessMessage(msg, replySink);
    }

    public void AsyncProcessRequest(
      IClientChannelSinkStack sinkStack,
      IMessage msg,
      ITransportHeaders headers,
      Stream stream)
    {
      throw new NotSupportedException();
    }

    public void AsyncProcessResponse(
      IClientResponseChannelSinkStack sinkStack,
      object state,
      ITransportHeaders headers,
      Stream stream)
    {
      throw new NotSupportedException();
    }

    public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
    {
      throw new NotSupportedException();
    }

    public void ProcessMessage(
      IMessage msg,
      ITransportHeaders requestHeaders,
      Stream requestStream,
      out ITransportHeaders responseHeaders,
      out Stream responseStream)
    {
      throw new NotSupportedException();
    }

    public IMessage SyncProcessMessage(IMessage msg)
    {
      ILogger defaultLogger = DiagUtility.DefaultLogger;
      if (msg is IMethodCallMessage methodCallMessage)
      {
        PersistentClientCacheableAttribute customAttribute = methodCallMessage.MethodBase.GetCustomAttribute<PersistentClientCacheableAttribute>(false);
        if (customAttribute != null)
        {
          string systemId;
          if (this.TryGetSystemId(methodCallMessage, out systemId))
          {
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (PersistentClientCacheStore cacheStore = new PersistentClientCacheStore(systemId))
            {
              IETagsProvider etagsProvider;
              try
              {
                Type provider = customAttribute.Provider;
                etagsProvider = !typeof (DefaultETagsProvider).Equals(provider) ? Activator.CreateInstance(provider) as IETagsProvider : (IETagsProvider) new DefaultETagsProvider(customAttribute.KeyFormat);
                Dictionary<string, string> clientEtags = new Dictionary<string, string>();
                etagsProvider.GetClientETags((IClientCacheStore) cacheStore, clientEtags, methodCallMessage.Args);
                msg.Properties[(object) "__enClETgs"] = (object) clientEtags;
              }
              catch (Exception ex)
              {
                defaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (CachingClientChannelSink), "Error in SyncProcessMessage.", ex);
                return this._innerSink?.SyncProcessMessage(msg);
              }
              IMessage message = this._innerSink?.SyncProcessMessage(msg);
              try
              {
                if (!message.Properties.Contains((object) "__enSrETgs") || !(message is MethodResponse methodResponse))
                  return message;
                Dictionary<string, string> property = message.Properties[(object) "__enSrETgs"] as Dictionary<string, string>;
                object returnValue = methodResponse.ReturnValue;
                object ret = etagsProvider.MergeWithCache((IClientCacheStore) cacheStore, returnValue, property);
                return returnValue == ret ? message : (IMessage) new ReturnMessage(ret, methodResponse.OutArgs, methodResponse.OutArgCount, methodResponse.LogicalCallContext, methodCallMessage);
              }
              catch (Exception ex)
              {
                defaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (CachingClientChannelSink), "Error in SyncProcessMessage.", ex);
                return message;
              }
              finally
              {
                stopwatch.Stop();
                defaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (CachingClientChannelSink), string.Format("{0}() took {1}ms.", (object) nameof (SyncProcessMessage), (object) stopwatch.ElapsedMilliseconds));
              }
            }
          }
        }
        else
        {
          IMessage message = this._innerSink?.SyncProcessMessage(msg);
          if (message.Properties.Contains((object) "__enSysId") && message is MethodResponse methodResponse)
          {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
              string property = message.Properties[(object) "__enSysId"] as string;
              if (!string.IsNullOrEmpty(property))
              {
                if (methodResponse.ReturnValue is MarshalByRefObject returnValue)
                {
                  string sessionId;
                  if (this.TryGetSessionId(RemotingServices.GetObjectUri(returnValue), out sessionId))
                    this._systemIds[sessionId] = property;
                }
              }
            }
            finally
            {
              stopwatch.Stop();
              defaultLogger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (CachingClientChannelSink), string.Format("{0}() took {1}ms.", (object) nameof (SyncProcessMessage), (object) stopwatch.ElapsedMilliseconds));
            }
          }
          return message;
        }
      }
      return this._innerSink?.SyncProcessMessage(msg);
    }

    private bool TryGetSessionId(string uri, out string sessionId)
    {
      if (!string.IsNullOrEmpty(uri))
      {
        uri = uri.TrimStart('/');
        int num1 = uri.IndexOf('/');
        int num2 = uri.IndexOf('/', num1 + 1);
        sessionId = uri.Substring(num1 + 1, num2 - num1 - 1);
        return true;
      }
      sessionId = (string) null;
      return false;
    }

    private bool TryGetSystemId(IMethodCallMessage mthdMsg, out string systemId)
    {
      systemId = (string) null;
      string sessionId;
      return mthdMsg != null && this.TryGetSessionId(mthdMsg.Uri, out sessionId) && this._systemIds.TryGetValue(sessionId, out systemId);
    }
  }
}
