// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Caching.Timers.TimerWrapper
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Timers;

#nullable disable
namespace Elli.Service.Common.Caching.Timers
{
  public class TimerWrapper : ITimer, IDisposable
  {
    private readonly Timer timer;

    public event EventHandler Elapsed;

    public TimerWrapper(double interval) => this.timer = new Timer(interval);

    public void Start()
    {
      this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
      this.timer.Start();
    }

    private void timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (this.Elapsed == null)
        return;
      this.Elapsed((object) this, new EventArgs());
    }

    public void Stop()
    {
      this.timer.Elapsed -= new ElapsedEventHandler(this.timer_Elapsed);
      this.timer.Stop();
    }

    public void Dispose()
    {
      this.timer.Elapsed -= new ElapsedEventHandler(this.timer_Elapsed);
      this.timer.Dispose();
    }
  }
}
