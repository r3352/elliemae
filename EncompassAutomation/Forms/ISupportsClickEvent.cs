// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ISupportsClickEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Interface signaling that the control supports the Click event
  /// </summary>
  /// <exclude />
  public interface ISupportsClickEvent : ISupportsEvents
  {
    /// <summary>The Click event provided by controls such as the <see cref="T:EllieMae.Encompass.Forms.Button" /> control.</summary>
    event EventHandler Click;

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    bool InvokeClick();
  }
}
