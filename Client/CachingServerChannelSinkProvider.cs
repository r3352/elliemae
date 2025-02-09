// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.CachingServerChannelSinkProvider
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using System.Runtime.Remoting.Channels;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class CachingServerChannelSinkProvider : IServerChannelSinkProvider
  {
    public IServerChannelSinkProvider Next { get; set; }

    public IServerChannelSink CreateSink(IChannelReceiver channel)
    {
      return (IServerChannelSink) new CachingServerChannelSink(this.Next?.CreateSink(channel));
    }

    public void GetChannelData(IChannelDataStore channelData)
    {
    }
  }
}
