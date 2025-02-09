// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.CalendarDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class CalendarDataSource : RemotableDataSource, ICalendarDataSource
  {
    private IServerDataProvider provider;

    public CalendarDataSource(IServerDataProvider provider)
      : base(true)
    {
      this.provider = provider;
    }

    public object AddBusinessDays(object date, int count, bool moveToNext)
    {
      return this.provider.AddBusinessDays(date, count, moveToNext);
    }

    public object AddPostalDays(object date, int count, bool moveToNext)
    {
      return this.provider.AddPostalDays(date, count, moveToNext);
    }

    public object AddRegZBusinessDays(object date, int count, bool moveToNext)
    {
      return this.provider.AddRegZBusinessDays(date, count, moveToNext);
    }

    internal IServerDataProvider DataProvider => this.provider;

    public override void Dispose()
    {
      base.Dispose();
      this.provider = (IServerDataProvider) null;
    }

    public override object Clone() => (object) new CalendarDataSource(this.provider);
  }
}
