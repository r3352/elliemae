// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.PackagePreviewSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public enum PackagePreviewSettings
  {
    Disabled = 0,
    [Description("1 day before signing date and time")] OneDayBeforeSigningDateAndTime = 2,
    [Description("2 days before signing date and time")] TwoDaysBeforeSigningDateAndTime = 3,
    [Description("3 days before signing date and time")] ThreeDaysBeforeSigningDateAndTime = 4,
  }
}
