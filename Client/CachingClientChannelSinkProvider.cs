// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.CachingClientChannelSinkProvider
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using System.Collections.Concurrent;
using System.Runtime.Remoting.Channels;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class CachingClientChannelSinkProvider : 
    IClientFormatterSinkProvider,
    IClientChannelSinkProvider
  {
    private readonly ConcurrentDictionary<string, string> _systemIds;

    public CachingClientChannelSinkProvider()
    {
      this._systemIds = new ConcurrentDictionary<string, string>();
    }

    public IClientChannelSinkProvider Next { get; set; }

    public IClientChannelSink CreateSink(
      IChannelSender channel,
      string url,
      object remoteChannelData)
    {
      return this.Next.CreateSink(channel, url, remoteChannelData) is IClientFormatterSink sink ? (IClientChannelSink) new CachingClientChannelSink(sink, this._systemIds) : (IClientChannelSink) null;
    }
  }
}
