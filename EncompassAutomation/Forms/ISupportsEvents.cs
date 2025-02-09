// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ISupportsEvents
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Marker interface to indicate the type supports one or more events
  /// </summary>
  /// <exclude />
  public interface ISupportsEvents
  {
    /// <summary>
    /// Provides a list of the names of the events supported by the control.
    /// </summary>
    string[] SupportedEvents { get; }
  }
}
