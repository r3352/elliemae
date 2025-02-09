// Decompiled with JetBrains decompiler
// Type: Elli.Common.Diagnostics.PerformanceLogger
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.Diagnostics
{
  public static class PerformanceLogger
  {
    private const string Name = "EncompassPlatform.Performance�";
    private const string Description = "�";

    [CLSCompliant(false)]
    public static Elli.Log.PerformanceLogger StartMeter(string operationName)
    {
      return PerformanceLogger.StartMeter(operationName, "");
    }

    [CLSCompliant(false)]
    public static Elli.Log.PerformanceLogger StartMeter(
      string operationName,
      string operationDescription)
    {
      Elli.Log.PerformanceLogger performanceLogger = new Elli.Log.PerformanceLogger("EncompassPlatform.Performance");
      performanceLogger.Start(operationName, operationDescription);
      return performanceLogger;
    }
  }
}
