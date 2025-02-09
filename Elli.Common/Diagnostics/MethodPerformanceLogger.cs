// Decompiled with JetBrains decompiler
// Type: Elli.Common.Diagnostics.MethodPerformanceLogger
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Log;

#nullable disable
namespace Elli.Common.Diagnostics
{
  public static class MethodPerformanceLogger
  {
    private const string Name = "EncompassPlatform.MethodPerformance�";
    private const string Description = "�";
    private static readonly PlainLogger Logger = new PlainLogger("EncompassPlatform.MethodPerformance")
    {
      Description = ""
    };

    public static void Log(string message) => MethodPerformanceLogger.Logger.Info(message);
  }
}
