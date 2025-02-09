// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.HTMLFontEditor
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
  /// Provides a UITypeEditor for the Font property of the ContentControl.
  /// </summary>
  /// <exclude />
  public class HTMLFontEditor : UITypeEditor
  {
    /// <summary>Indicates that the property editor is a modal dialog.</summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    /// <summary>
    /// Displays the HTMLFontSelector dialog to allow the user to select the font for use in a control.
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
      using (HTMLFontSelector htmlFontSelector = new HTMLFontSelector((HTMLFont) value))
        return htmlFontSelector.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm) == DialogResult.OK ? (object) htmlFontSelector.SelectedFont : (object) (HTMLFont) value;
    }
  }
}
