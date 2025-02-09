// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.DropdownOptionsEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// The UITypeEditor implementation to allow for use of the <see cref="T:EllieMae.Encompass.Forms.Design.DropdownOptionsEditor" />
  /// in the Property Grid.
  /// </summary>
  /// <exclude />
  public class DropdownOptionsEditor : UITypeEditor
  {
    /// <summary>Overrides the edit style to use a modal dialog.</summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    /// <summary>
    /// Invokes the <see cref="T:EllieMae.Encompass.Forms.Design.DropdownOptionsEditor" /> dialog when the user selects to edit
    /// a dropdown option list.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="provider"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override object EditValue(
      ITypeDescriptorContext context,
      System.IServiceProvider provider,
      object value)
    {
      using (DropdownOptionsDialog dropdownOptionsDialog = new DropdownOptionsDialog((DropdownOptionCollection) value))
      {
        int num = (int) dropdownOptionsDialog.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm);
      }
      return value;
    }
  }
}
