// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.FieldEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// Provides a UIType editor for the FieldControl's Field property.
  /// </summary>
  /// <exclude />
  public class FieldEditor : UITypeEditor
  {
    private static FieldSelector selector;

    /// <summary>
    /// Indicates that the property's editor will be a modal dialog.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    /// <summary>
    /// Displays the field selection dialog for editing of a FieldControl's Field property.
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
      if (FieldEditor.selector == null)
        FieldEditor.selector = new FieldSelector();
      return FieldEditor.selector.ShowDialog(System.Windows.Forms.Form.ActiveForm, (FieldDescriptor) value) == DialogResult.OK ? (object) FieldEditor.selector.SelectedField : (object) (FieldDescriptor) value;
    }
  }
}
