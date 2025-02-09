// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.FieldEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.BusinessObjects.Loans;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class FieldEditor : UITypeEditor
  {
    private static FieldSelector selector;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    public override object EditValue(
      ITypeDescriptorContext context,
      IServiceProvider provider,
      object value)
    {
      if (FieldEditor.selector == null)
        FieldEditor.selector = new FieldSelector();
      return FieldEditor.selector.ShowDialog(System.Windows.Forms.Form.ActiveForm, (FieldDescriptor) value) == DialogResult.OK ? (object) FieldEditor.selector.SelectedField : (object) (FieldDescriptor) value;
    }
  }
}
