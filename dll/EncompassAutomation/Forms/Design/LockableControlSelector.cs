// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.LockableControlSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class LockableControlSelector : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      IServiceProvider pProvider,
      ListBox pListBox)
    {
      if (!(pContext.Instance is EllieMae.Encompass.Forms.Control instance))
        return;
      pListBox.Items.Add((object) EllieMae.Encompass.Forms.Control.Empty);
      foreach (FieldControl fieldControl in instance.Form.FindControlsByType(typeof (EllieMae.Encompass.Forms.TextBox)))
        pListBox.Items.Add((object) fieldControl);
      foreach (FieldControl fieldControl in instance.Form.FindControlsByType(typeof (DropdownBox)))
        pListBox.Items.Add((object) fieldControl);
      pListBox.Sorted = true;
      pListBox.ClientSize = new Size(pListBox.ClientSize.Width, Math.Min(pListBox.Items.Count, 10) * pListBox.ItemHeight);
    }

    protected override object TranslateValue(object value)
    {
      return value == EllieMae.Encompass.Forms.Control.Empty ? (object) null : value;
    }
  }
}
