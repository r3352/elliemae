// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.CachingServerChannelSink
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using EllieMae.EMLite.ClientServer.Cache;
using Encompass.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class CachingServerChannelSink : IServerChannelSink, IChannelSinkBase
  {
    private readonly IServerChannelSink _innerSink;

    public CachingServerChannelSink(IServerChannelSink innerSink) => this._innerSink = innerSink;

    public IServerChannelSink NextChannelSink => this._innerSink.NextChannelSink;

    public IDictionary Properties => this._innerSink.Properties;

    public void AsyncProcessResponse(
      IServerResponseChannelSinkStack sinkStack,
      object state,
      IMessage msg,
      ITransportHeaders headers,
      Stream stream)
    {
      this._innerSink.AsyncProcessResponse(sinkStack, state, msg, headers, stream);
    }

    public Stream GetResponseStream(
      IServerResponseChannelSinkStack sinkStack,
      object state,
      IMessage msg,
      ITransportHeaders headers)
    {
      return this._innerSink.GetResponseStream(sinkStack, state, msg, headers);
    }

    public ServerProcessing ProcessMessage(
      IServerChannelSinkStack sinkStack,
      IMessage requestMsg,
      ITransportHeaders requestHeaders,
      Stream requestStream,
      out IMessage responseMsg,
      out ITransportHeaders responseHeaders,
      out Stream responseStream)
    {
      ServerProcessing serverProcessing = this._innerSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
      if (responseMsg is IMethodReturnMessage methodReturnMessage)
      {
        if (requestMsg is IMethodCallMessage mcm && mcm.Properties.Contains((object) "__enClETgs"))
        {
          PersistentClientCacheableAttribute customAttribute = mcm.MethodBase.GetCustomAttribute<PersistentClientCacheableAttribute>(false);
          if (customAttribute != null)
          {
            try
            {
              Dictionary<string, string> property = mcm.Properties[(object) "__enClETgs"] as Dictionary<string, string>;
              Type provider = customAttribute.Provider;
              IETagsProvider etagsProvider = !typeof (DefaultETagsProvider).Equals(provider) ? Activator.CreateInstance(provider) as IETagsProvider : (IETagsProvider) new DefaultETagsProvider(customAttribute.KeyFormat);
              Dictionary<string, string> serverETags = new Dictionary<string, string>();
              object serverValueAndEtags = etagsProvider.GetServerValueAndETags(methodReturnMessage.ReturnValue, property, serverETags, mcm.Args);
              if (serverValueAndEtags != methodReturnMessage.ReturnValue)
                responseMsg = (IMessage) new ReturnMessage(serverValueAndEtags, methodReturnMessage.OutArgs, methodReturnMessage.OutArgCount, methodReturnMessage.LogicalCallContext, mcm);
              responseMsg.Properties[(object) "__enSrETgs"] = (object) serverETags;
            }
            catch (Exception ex)
            {
              DiagUtility.GlobalLogger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, "CachingClientChannelSink", "Error in ProcessMessage.", ex);
            }
          }
        }
        if (methodReturnMessage.ReturnValue is IClientCacheSettings returnValue && returnValue.PersistentClientCacheEnabled)
          responseMsg.Properties[(object) "__enSysId"] = (object) returnValue.SystemID;
      }
      return serverProcessing;
    }
  }
}
