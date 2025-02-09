// Decompiled with JetBrains decompiler
// Type: Elli.Common.Diagnostics.LoanProcessLogger
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Log;
using System.Text;

#nullable disable
namespace Elli.Common.Diagnostics
{
  public class LoanProcessLogger
  {
    private const string Name = "EncompassPlatform.LoanProcess�";
    private const string FileName = "LoanProcessLog�";
    private const string PathName = "LoanProcessLogFile�";
    private const string Description = "LoanProcessLog�";
    private readonly StringBuilder _stringBuilder;
    private ProcessLogger _logger;

    public string LogPath { get; private set; }

    public LoanProcessLogger(string logPath)
    {
      this._stringBuilder = new StringBuilder();
      this.CreateLogger(logPath);
    }

    public void Log(string message)
    {
      if (this._logger == null && string.IsNullOrEmpty(message))
        return;
      if (this._logger == null)
      {
        this.AddMessage(message);
      }
      else
      {
        if (this._stringBuilder != null && this._stringBuilder.Length > 0)
          this._logger.Info(this._stringBuilder.ToString());
        this._logger.Info(message);
      }
    }

    public bool IsLogCreated() => this._logger != null;

    public void CreateLogger(string logPath)
    {
      if (this._logger != null || string.IsNullOrEmpty(logPath))
        return;
      this.LogPath = logPath;
      this._logger = new ProcessLogger("EncompassPlatform.LoanProcess", "LoanProcessLog", "LoanProcessLogFile", this.LogPath)
      {
        Description = "LoanProcessLog"
      };
    }

    private void AddMessage(string message)
    {
      this._stringBuilder.Append(message);
      this._stringBuilder.AppendLine();
    }
  }
}
