// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.CodeBaseEditor
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
  /// Provides a UITypeEditor for the Form's CodeBase property to be used in the Property Grid.
  /// </summary>
  /// <exclude />
  public class CodeBaseEditor : UITypeEditor
  {
    /// <summary>Returns the editor style for the type editor.</summary>
    /// <param name="context">The current control context.</param>
    /// <returns>This method always returns UITypeEditorEditStyle.Modal.</returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    /// <summary>
    /// Overrides the type editor's default bevahior to display the CodeBaseEditor.
    /// </summary>
    /// <param name="context">The current control context.</param>
    /// <param name="provider">The services provider.</param>
    /// <param name="value">The value beind edited.</param>
    /// <returns>Returns the new value based on the user's input.</returns>
    public override object EditValue(
      ITypeDescriptorContext context,
      System.IServiceProvider provider,
      object value)
    {
      using (CodeBaseSelector codeBaseSelector = new CodeBaseSelector((CodeBase) value))
      {
        if (codeBaseSelector.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm) == DialogResult.OK)
          return (object) codeBaseSelector.CodeBase;
      }
      return value;
    }
  }
}
