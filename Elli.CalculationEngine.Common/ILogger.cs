// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.ILogger
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using System;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public interface ILogger
  {
    void Debug(string message);

    void Debug(string message, Exception e);

    void Information(string message);

    void Information(string message, Exception e);

    void Fatal(string message);

    void Fatal(string message, Exception e);

    void Warning(string message);

    void Warning(string message, Exception e);

    void Trace(string message);

    void Trace(string message, Exception e);
  }
}
