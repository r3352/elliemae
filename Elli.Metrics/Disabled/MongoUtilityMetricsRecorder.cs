// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.Disabled.MongoUtilityMetricsRecorder
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

using System;

#nullable disable
namespace Elli.Metrics.Disabled
{
  public class MongoUtilityMetricsRecorder : IMongoUtilityMetricsRecorder
  {
    public IDisposable RecordsMongoUtilityApiTime(string apiName) => (IDisposable) null;

    public void IncrementLoanLoadCount()
    {
    }

    public void IncrementReadPipelineCount()
    {
    }

    public void IncrementLoanGetCount()
    {
    }

    public void IncrementLoanUpdateCount()
    {
    }
  }
}
