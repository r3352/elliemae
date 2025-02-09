// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.GCUtility
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using Belikov.GenuineChannels;
using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels.TransportContext;
using System;
using System.Runtime.Remoting.Channels;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public static class GCUtility
  {
    public static void SetTransportCorrelationIdentifer(string id)
    {
      try
      {
        IChannel channel = ChannelServices.GetChannel("ghttp-client");
        if (channel == null)
          return;
        ((BasicChannelWithSecurity) channel).ITransportContext.CorrelationIdentifier = id;
      }
      catch (Exception ex)
      {
      }
    }

    public static string GetCurrentCorrelationIdentifer()
    {
      try
      {
        return GenuineUtility.CurrentCorrelationIdentifer;
      }
      catch (Exception ex)
      {
      }
      return (string) null;
    }

    public static void AddBeforeStartRequestEventHandler(BeforeStartRequestEventHandler handler)
    {
      try
      {
        IChannel channel = ChannelServices.GetChannel("ghttp-client");
        if (channel == null)
          return;
        ((BasicChannelWithSecurity) channel).ITransportContext.BeforeStartRequest += handler;
      }
      catch (Exception ex)
      {
      }
    }

    public static void AddAfterFinishRequestEventHandler(AfterFinishRequestEventHandler handler)
    {
      try
      {
        IChannel channel = ChannelServices.GetChannel("ghttp-client");
        if (channel == null)
          return;
        ((BasicChannelWithSecurity) channel).ITransportContext.AfterFinishRequest += handler;
      }
      catch (Exception ex)
      {
      }
    }
  }
}
