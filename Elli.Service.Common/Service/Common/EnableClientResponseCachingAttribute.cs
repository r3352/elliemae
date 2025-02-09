// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.EnableClientResponseCachingAttribute
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;

#nullable disable
namespace Elli.Service.Common
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public class EnableClientResponseCachingAttribute : Attribute, IEnableResponseCachingAttribute
  {
    public string Region { get; set; }

    public int Hours { get; set; }

    public int Minutes { get; set; }

    public int Seconds { get; set; }

    public TimeSpan Expiration
    {
      get
      {
        if (this.Hours == 0 && this.Minutes == 0 && this.Seconds == 0)
          throw new InvalidOperationException("You need to specify at least an hour value, a minute value or a second value");
        return new TimeSpan(0, this.Hours, this.Minutes, this.Seconds);
      }
    }
  }
}
