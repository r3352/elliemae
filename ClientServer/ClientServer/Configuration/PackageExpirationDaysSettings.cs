// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.PackageExpirationDaysSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public enum PackageExpirationDaysSettings
  {
    [Description("Day of Signing")] DayofSigning,
    [Description("1 day after signing date")] OneDayAfterSigningDate,
    [Description("2 days after signing date")] TwoDaysAfterSigningDate,
    [Description("No Expiration")] NoExpiration,
  }
}
