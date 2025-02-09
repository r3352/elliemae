// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ISupportsPopupEvent
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Interface signaling that the control supports the Popup event
  /// </summary>
  /// <exclude />
  public interface ISupportsPopupEvent : ISupportsEvents
  {
    /// <summary>The Popup event, used by the <see cref="T:EllieMae.Encompass.Forms.PickList" /> control to allow runtime formatting of its pick list.</summary>
    event EventHandler Popup;

    /// <summary>The ItemSelected event, used by the <see cref="T:EllieMae.Encompass.Forms.PickList" /> control to allow runtime handling of the user's selection.</summary>
    event ItemSelectedEventHandler ItemSelected;

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    void InvokePopup();

    /// <summary>This method is intended for internal use within Encompass only.</summary>
    void InvokeItemSelected(DropdownOption option);
  }
}
