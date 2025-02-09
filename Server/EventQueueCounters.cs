// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EventQueueCounters
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class EventQueueCounters
  {
    private static readonly EventQueueCounters.ServerCounter eventQueue = new EventQueueCounters.ServerCounter();

    public static EventQueueCounters.ServerCounter EventQueue => EventQueueCounters.eventQueue;

    public class ServerCounter
    {
      private int counter;

      public void Set(int value) => Interlocked.Exchange(ref this.counter, value);

      public void Increment() => Interlocked.Increment(ref this.counter);

      public void Decrement() => Interlocked.Decrement(ref this.counter);

      public int Value => this.counter;
    }
  }
}
