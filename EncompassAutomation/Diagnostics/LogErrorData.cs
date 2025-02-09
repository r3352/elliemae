// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.LogErrorData
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using Encompass.Diagnostics.PII;

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  /// <summary>Class for mapping data in thrown excpetion</summary>
  public class LogErrorData : ILogErrorData
  {
    internal LogErrorData(Encompass.Diagnostics.Logging.Schema.LogErrorData logErrorData, bool maskSQL)
    {
      this.Type = logErrorData.Type;
      this.Message = MaskUtilities.MaskPII(logErrorData.Message, maskSQL);
      this.StackTrace = MaskUtilities.MaskPII(logErrorData.StackTrace, maskSQL);
      if (logErrorData.Xcause == null)
        return;
      this.Xcause = (ILogErrorData) new LogErrorData(logErrorData.Xcause, maskSQL);
    }

    /// <summary>Name of Exception's Type</summary>
    public string Type { get; }

    /// <summary>Exception message</summary>
    public string Message { get; }

    /// <summary>Stack trace of exception</summary>
    public string StackTrace { get; }

    /// <summary>
    /// In case the mapped exception has InnerException Xcause will be populated from InnerException
    /// </summary>
    public ILogErrorData Xcause { get; }
  }
}
