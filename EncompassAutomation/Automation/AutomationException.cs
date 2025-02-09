// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.AutomationException
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>Reprensents a generic Encompass automation error.</summary>
  [Serializable]
  public class AutomationException : Exception
  {
    /// <summary>Constructor using a specific error message.</summary>
    /// <param name="message">The description of the error.</param>
    public AutomationException(string message)
      : base(message)
    {
    }

    /// <summary>Constructor for a re-thrown exception.</summary>
    /// <param name="message">The description of the exception.</param>
    /// <param name="innerException">The inner exception being re-thrown.</param>
    public AutomationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
