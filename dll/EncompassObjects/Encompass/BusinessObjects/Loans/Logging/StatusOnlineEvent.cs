// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.StatusOnlineEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Globalization;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class StatusOnlineEvent : IStatusOnlineEvent
  {
    private string description;
    private DateTime date;

    internal StatusOnlineEvent(string description, string dateStr)
    {
      this.description = description;
      try
      {
        this.date = DateTime.ParseExact(dateStr, new string[2]
        {
          "MM/dd/yyyy",
          "M/d/yy"
        }, (IFormatProvider) null, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
      }
      catch
      {
        this.date = DateTime.MinValue;
      }
    }

    public string Description => this.description;

    public DateTime Date => this.date;
  }
}
