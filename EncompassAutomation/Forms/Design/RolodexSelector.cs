// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.RolodexSelector
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
  /// Provides a type editor for the Rolodex selection field of the TextBox control.
  /// </summary>
  /// <exclude />
  public class RolodexSelector : BaseDropdownTypeEditor
  {
    internal override void FillInList(
      ITypeDescriptorContext pContext,
      System.IServiceProvider pProvider,
      ListBox pListBox)
    {
      EllieMae.Encompass.Forms.Control instance = (EllieMae.Encompass.Forms.Control) pContext.Instance;
      pListBox.Items.Add((object) RolodexGroup.Empty);
      foreach (Rolodex rolodex in instance.Form.FindControlsByType(typeof (Rolodex)))
        pListBox.Items.Add((object) rolodex.Group);
      pListBox.IntegralHeight = false;
      pListBox.ClientSize = new Size(pListBox.ClientSize.Width, Math.Min(pListBox.Items.Count, 10) * (pListBox.ItemHeight + 1));
    }
  }
}
