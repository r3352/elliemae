// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CustomCodeContextDataProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class CustomCodeContextDataProvider : IServerDataProvider
  {
    private ClientContext context;
    private BusinessCalendar businessCalendar;
    private BusinessCalendar postalCalendar;
    private BusinessCalendar regZbusinessCalendar;
    private List<Dictionary<string, List<string>>> sfcLists;

    public CustomCodeContextDataProvider(ClientContext context) => this.context = context;

    public object AddBusinessDays(object date, int count, bool moveToNext)
    {
      if (this.businessCalendar == null)
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          this.businessCalendar = BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Business);
      }
      try
      {
        return (object) this.businessCalendar.AddBusinessDays(Utils.ParseDate(date), count, moveToNext);
      }
      catch
      {
        return (object) null;
      }
    }

    public object AddPostalDays(object date, int count, bool moveToNext)
    {
      if (this.postalCalendar == null)
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          this.postalCalendar = BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Postal);
      }
      try
      {
        return (object) this.postalCalendar.AddBusinessDays(Utils.ParseDate(date), count, moveToNext);
      }
      catch
      {
        return (object) null;
      }
    }

    public object AddRegZBusinessDays(object date, int count, bool moveToNext)
    {
      if (this.regZbusinessCalendar == null)
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          this.regZbusinessCalendar = BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.RegZ);
      }
      try
      {
        return (object) this.regZbusinessCalendar.AddBusinessDays(Utils.ParseDate(date), count, moveToNext);
      }
      catch
      {
        return (object) null;
      }
    }

    public DateTime Timestamp => DateTime.Now;

    public List<Dictionary<string, List<string>>> GetActiveSpecialFeatureCodes()
    {
      if (this.sfcLists == null)
      {
        try
        {
          this.sfcLists = CustomCodeSessionDataProvider.BuildSpeicalFeatureCodeList(new SpecialFeatureCodeDefinitionAccessor().GetAll(true));
        }
        catch (Exception ex)
        {
          return (List<Dictionary<string, List<string>>>) null;
        }
      }
      return this.sfcLists;
    }
  }
}
