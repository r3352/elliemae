// Decompiled with JetBrains decompiler
// Type: Elli.Common.Diagnostics.LoanProcessErrorLogger
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Log;
using System;
using System.Text;

#nullable disable
namespace Elli.Common.Diagnostics
{
  public class LoanProcessErrorLogger
  {
    private const string Name = "EncompassPlatform.LoanProcessError�";
    private const string FileName = "LoanProcessErrorLog�";
    private const string PathName = "LoanProcessErrorLogFile�";
    private const string Description = "LoanProcessErrorLog�";
    private readonly StringBuilder _stringBuilder;
    private ProcessLogger _logger;

    public string LogPath { get; private set; }

    public LoanProcessErrorLogger(string logPath)
    {
      this._stringBuilder = new StringBuilder();
      this.CreateLogger(logPath);
    }

    public void Log(string message)
    {
      if (this._logger == null)
        return;
      this._logger.Info(message);
    }

    public void Error(string message, Exception ex)
    {
      this._logger.Error(message + Environment.NewLine + (object) ex);
    }

    public void AddMessage(string message)
    {
      this._stringBuilder.Append(message);
      this._stringBuilder.AppendLine();
    }

    public string GetMessage()
    {
      return this._stringBuilder == null ? string.Empty : this._stringBuilder.ToString();
    }

    public bool IsLogCreated() => this._logger != null;

    public void CreateLogger(string logPath)
    {
      if (string.IsNullOrEmpty(logPath))
        return;
      this.LogPath = logPath;
      this._logger = new ProcessLogger("EncompassPlatform.LoanProcessError", "LoanProcessErrorLog", "LoanProcessErrorLogFile", this.LogPath)
      {
        Description = "LoanProcessErrorLog"
      };
    }
  }
}
