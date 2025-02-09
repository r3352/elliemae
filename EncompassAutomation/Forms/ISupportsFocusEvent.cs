// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ISupportsFocusEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Interface signaling that the control support the FocusIn and FocusOut events
  /// </summary>
  /// <exclude />
  public interface ISupportsFocusEvent : ISupportsEvents
  {
    /// <summary>The FocusIn event, which indicates when a FieldControl has gained the cursor focus.</summary>
    event EventHandler FocusIn;

    /// <summary>The FocusOut event, which indicates when a FieldControl has lost the cursor focus.</summary>
    event EventHandler FocusOut;

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    void InvokeFocusIn();

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    void InvokeFocusOut();
  }
}
