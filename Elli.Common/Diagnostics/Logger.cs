// Decompiled with JetBrains decompiler
// Type: Elli.Common.Diagnostics.Logger
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Elli.Common.Diagnostics
{
  public static class Logger
  {
    private const string Name = "EncompassPlatform.Log�";
    private const string Description = "�";
    private static Elli.Log.Logger _logger = new Elli.Log.Logger("EncompassPlatform.Log")
    {
      Description = "",
      IsDebug = true
    };

    public static bool IsDebug
    {
      set => Logger._logger.IsDebug = value;
    }

    public static LogPropertyDictionary CreateLogProperties(params object[] propertyList)
    {
      if (propertyList == null)
        throw new ArgumentNullException();
      if (propertyList.Length % 2 != 0)
        throw new ArgumentException("The number of parameters is not even; the parameters provided must be pairs of property names and values.");
      LogPropertyDictionary logProperties = new LogPropertyDictionary();
      for (int index = 0; index < propertyList.Length; index += 2)
      {
        string property1 = propertyList[index] as string;
        if (string.IsNullOrEmpty(property1))
          throw new ArgumentException("The value provided for a property name is not a string type or does not contain a value.");
        object property2 = propertyList[index + 1];
        logProperties.Add(property1, property2);
      }
      return logProperties;
    }

    public static void Log(string message) => Logger.Log(TraceLevel.Verbose, message, "");

    public static void Log(TraceLevel level, string message) => Logger.Log(level, message, "");

    public static void Log(TraceLevel level, Exception ex, string className)
    {
      Logger.Log(level, ex.ToString(), className);
    }

    public static void Log(TraceLevel level, string message, string className)
    {
      Logger._logger.Write(level, className, message);
    }

    public static void LogError(string message) => Logger.LogError(message, "");

    public static void LogError(string message, Exception ex)
    {
      Logger.LogError(message + Environment.NewLine + ex.ToString());
    }

    public static void LogError(string message, string className)
    {
      Logger.Log(TraceLevel.Error, message, className);
    }

    public static void LogWarning(string message) => Logger.LogWarning(message, "");

    public static void LogWarning(string message, Exception ex)
    {
      Logger.LogWarning(message + Environment.NewLine + ex.ToString());
    }

    public static void LogWarning(Exception ex, string className)
    {
      Logger.LogWarning(ex.ToString(), className);
    }

    public static void LogWarning(string message, string className)
    {
      Logger.Log(TraceLevel.Warning, message, className);
    }

    public static void LogDebug(string message) => Logger.LogDebug(message, "");

    public static void LogDebug(string message, Exception ex)
    {
      Logger.LogDebug(message + Environment.NewLine + ex.ToString());
    }

    public static void LogDebug(Exception ex, string className)
    {
      Logger.LogDebug(ex.ToString(), className);
    }

    public static void LogDebug(string message, string className)
    {
      Logger.Log(TraceLevel.Verbose, message, className);
    }

    public static void LogInfo(string message) => Logger.LogInfo(message, "");

    public static void LogInfo(string message, Exception ex)
    {
      Logger.LogDebug(message + Environment.NewLine + ex.ToString());
    }

    public static void LogInfo(Exception ex, string className)
    {
      Logger.LogInfo(ex.ToString(), className);
    }

    public static void LogInfo(string message, string className)
    {
      Logger.Log(TraceLevel.Info, message, className);
    }
  }
}
