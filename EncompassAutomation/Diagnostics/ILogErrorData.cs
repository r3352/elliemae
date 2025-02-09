// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Diagnostics.ILogErrorData
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Diagnostics
{
  /// <summary>Interface for mapping data in thrown excpetion</summary>
  public interface ILogErrorData
  {
    /// <summary>Name of Exception's Type</summary>
    string Type { get; }

    /// <summary>Exception message</summary>
    string Message { get; }

    /// <summary>Stack trace of exception</summary>
    string StackTrace { get; }

    /// <summary>
    /// In case the mapped exception has InnerException Xcause will be populated from InnerException
    /// </summary>
    ILogErrorData Xcause { get; }
  }
}
