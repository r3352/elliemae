// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Authentication.RetrySettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Authentication
{
  public class RetrySettings
  {
    public readonly int BaseDelay;
    public readonly int MaxDelay;
    public readonly int TimeOut;
    public readonly RetryMode RetryMode;
    public readonly int Retries;

    public RetrySettings()
    {
      this.BaseDelay = 400;
      this.MaxDelay = 1000;
      this.TimeOut = 10000;
      this.RetryMode = RetryMode.Linear;
      this.Retries = 3;
    }

    public RetrySettings(
      int baseDelay,
      int maxDelay,
      int timeOut,
      RetryMode retryMode = RetryMode.Linear,
      int retries = 3)
    {
      this.BaseDelay = baseDelay;
      this.MaxDelay = maxDelay;
      this.TimeOut = timeOut;
      this.RetryMode = retryMode;
      this.Retries = retries;
    }

    public RetrySettings(SessionObjects session)
    {
      this.BaseDelay = session.StartupInfo.AccessTokenBaseDelay > 0 ? session.StartupInfo.AccessTokenBaseDelay : 400;
      this.MaxDelay = session.StartupInfo.AccessTokenMaxDelay > 0 ? session.StartupInfo.AccessTokenMaxDelay : 1000;
      this.TimeOut = session.StartupInfo.AccessTokenTimeOut > 0 ? session.StartupInfo.AccessTokenTimeOut : 10000;
      this.RetryMode = RetryMode.Linear;
      this.Retries = 3;
    }

    public int GetDelay(int currentRetry)
    {
      if (this.Retries <= 1 || currentRetry <= 0 || this.BaseDelay <= 0 || this.MaxDelay <= 0)
        return 0;
      int val1 = 0;
      switch (this.RetryMode)
      {
        case RetryMode.Static:
          val1 = this.BaseDelay;
          break;
        case RetryMode.Linear:
          val1 = this.BaseDelay * currentRetry;
          break;
        case RetryMode.Exponential:
          val1 = (int) Math.Pow(2.0, (double) (currentRetry - 1)) * this.BaseDelay;
          break;
      }
      return Math.Min(val1, this.MaxDelay);
    }
  }
}
