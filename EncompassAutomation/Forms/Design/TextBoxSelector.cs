// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.TextBoxSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Provides a type editor for selecting a control from the Form's control list.
  /// </summary>
  /// <exclude />
  public class TextBoxSelector : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      System.IServiceProvider pProvider,
      ListBox pListBox)
    {
      if (!(pContext.Instance is EllieMae.Encompass.Forms.Control instance))
        return;
      pListBox.Items.Add((object) EllieMae.Encompass.Forms.Control.Empty);
      foreach (FieldControl fieldControl in instance.Form.FindControlsByType(typeof (EllieMae.Encompass.Forms.TextBox)))
        pListBox.Items.Add((object) fieldControl);
      pListBox.Sorted = true;
      pListBox.ClientSize = new Size(pListBox.ClientSize.Width, Math.Min(pListBox.Items.Count, 10) * pListBox.ItemHeight);
    }

    /// <summary>
    /// Performs transaction on the selection value to convert Control.Empty to <c>null</c>.
    /// </summary>
    /// <param name="value">The value to translate.</param>
    /// <returns>Returns null if the value is Control.Empty, otherwise it returns the value itself.</returns>
    protected override object TranslateValue(object value)
    {
      return value == EllieMae.Encompass.Forms.Control.Empty ? (object) null : value;
    }
  }
}
