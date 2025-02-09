// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Loggers.ConsoleLogger
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;

#nullable disable
namespace Elli.MessageQueues.Loggers
{
  internal class ConsoleLogger : ILogger
  {
    private readonly object _consoleLock = new object();

    public bool IsDebugEnable { get; set; }

    public void DebugFormat(string format, params object[] args)
    {
      if (!this.IsDebugEnable)
        return;
      this.Write(ConsoleColor.Gray, format, args);
    }

    public void InfoFormat(string format, params object[] args)
    {
      this.Write(ConsoleColor.Green, format, args);
    }

    public void WarnFormat(string format, params object[] args)
    {
      this.Write(ConsoleColor.Yellow, format, args);
    }

    public void ErrorFormat(string format, params object[] args)
    {
      this.Write(ConsoleColor.Red, format, args);
    }

    public void Error(Exception exception) => this.Write(ConsoleColor.Red, exception.ToString());

    private void Write(ConsoleColor color, string format, params object[] args)
    {
      string str;
      try
      {
        str = string.Format(format, args);
      }
      catch (Exception ex)
      {
        str = format;
      }
      lock (this._consoleLock)
      {
        ConsoleColor foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(str);
        Console.ForegroundColor = foregroundColor;
      }
    }
  }
}
