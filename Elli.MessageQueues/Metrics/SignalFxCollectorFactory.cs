// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Metrics.SignalFxCollectorFactory
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Metrics;
using Metrics.Reports;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues.Metrics
{
  [ExcludeFromCodeCoverage]
  public class SignalFxCollectorFactory : IServiceFactory<IMetricsCollector>
  {
    public SignalFxCollectorFactory(string contextName, bool initializeFromAppConfig = false)
    {
      if (initializeFromAppConfig)
        Metric.Config.WithReporting((Action<MetricsReports>) (reports => reports.WithSignalFxFromAppConfig()));
      this.ContextName = contextName;
    }

    public string ContextName { get; private set; }

    public IMetricsCollector CreateInstance()
    {
      return (IMetricsCollector) new SignalFxNetCollector(this.ContextName);
    }
  }
}
