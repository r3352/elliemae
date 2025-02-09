// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.AuditTrailEvent
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class AuditTrailEvent
  {
    public AuditTrailEvent() => this.ExtensionData = string.Empty;

    public DateTime LogTime { get; set; }

    public string Source { get; set; }

    public string UserName { get; set; }

    public string ExtensionData { get; set; }

    public string Action { get; set; }

    public string Message { get; set; }

    public string EnvelopeStatus { get; set; }

    public string ClientIpAddress { get; set; }

    public string Information { get; set; }

    public string GeoLocation { get; set; }

    public string Language { get; set; }
  }
}
