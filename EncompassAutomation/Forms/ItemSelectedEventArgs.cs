// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ItemSelectedEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Class representing the arguments for the ItemSelected event
  /// </summary>
  public class ItemSelectedEventArgs : EventArgs
  {
    private DropdownOption selectedItem;

    internal ItemSelectedEventArgs(DropdownOption selectedItem) => this.selectedItem = selectedItem;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.Forms.DropdownOption" /> selected by the user.
    /// </summary>
    public DropdownOption SelectedItem => this.selectedItem;
  }
}
