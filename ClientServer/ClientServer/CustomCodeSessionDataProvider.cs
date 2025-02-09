// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomCodeSessionDataProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class CustomCodeSessionDataProvider : IServerDataProvider
  {
    private SessionObjects sessionObjects;
    private BusinessCalendar businessCalendar;
    private BusinessCalendar postalCalendar;
    private BusinessCalendar regZBusinessCalendar;
    private DateTime cachedServerTime = DateTime.MinValue;
    private Stopwatch watch;
    private List<Dictionary<string, List<string>>> sfcLists;

    public CustomCodeSessionDataProvider(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
    }

    public object AddBusinessDays(object date, int count, bool moveToNext)
    {
      if (this.businessCalendar == null)
        this.businessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.Business);
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
        this.postalCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.Postal);
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
      if (this.regZBusinessCalendar == null)
        this.regZBusinessCalendar = this.sessionObjects.GetBusinessCalendar(CalendarType.RegZ);
      try
      {
        return (object) this.regZBusinessCalendar.AddBusinessDays(Utils.ParseDate(date), count, moveToNext);
      }
      catch
      {
        return (object) null;
      }
    }

    public DateTime Timestamp
    {
      get
      {
        if (!(this.cachedServerTime == DateTime.MinValue) && !(this.watch.Elapsed > TimeSpan.FromMinutes(5.0)))
          return this.cachedServerTime + this.watch.Elapsed;
        this.cachedServerTime = this.sessionObjects.Session.ServerTime;
        this.watch = new Stopwatch();
        this.watch.Start();
        return this.cachedServerTime;
      }
    }

    public List<Dictionary<string, List<string>>> GetActiveSpecialFeatureCodes()
    {
      if (this.sfcLists == null)
      {
        try
        {
          this.sfcLists = CustomCodeSessionDataProvider.BuildSpeicalFeatureCodeList(this.sessionObjects.SpecialFeatureCodeManager.GetActive());
        }
        catch (Exception ex)
        {
          return (List<Dictionary<string, List<string>>>) null;
        }
      }
      return this.sfcLists;
    }

    public static List<Dictionary<string, List<string>>> BuildSpeicalFeatureCodeList(
      IList<SpecialFeatureCodeDefinition> sfcAll)
    {
      Dictionary<string, List<string>> dictionary1 = new Dictionary<string, List<string>>();
      Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
      Dictionary<string, List<string>> dictionary3 = new Dictionary<string, List<string>>();
      if (sfcAll == null && sfcAll.Count == 0)
        return (List<Dictionary<string, List<string>>>) null;
      string str = "~~";
      for (int index = 0; index < sfcAll.Count; ++index)
      {
        if (sfcAll[index].IsActive)
        {
          List<string> stringList = new List<string>()
          {
            sfcAll[index].Code,
            sfcAll[index].Description,
            sfcAll[index].Comment,
            sfcAll[index].Source
          };
          if (sfcAll[index].Source == "Fannie Mae")
            dictionary1.Add(sfcAll[index].ID + str + sfcAll[index].Code, stringList);
          else if (sfcAll[index].Source == "Freddie Mac")
            dictionary2.Add(sfcAll[index].ID + str + sfcAll[index].Code, stringList);
          else
            dictionary3.Add(sfcAll[index].ID + str + sfcAll[index].Code, stringList);
        }
      }
      return new List<Dictionary<string, List<string>>>()
      {
        dictionary1,
        dictionary2,
        dictionary3
      };
    }
  }
}
