// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.HTMLFontEditor
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class HTMLFontEditor : UITypeEditor
  {
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    public override object EditValue(
      ITypeDescriptorContext context,
      IServiceProvider provider,
      object value)
    {
      using (HTMLFontSelector htmlFontSelector = new HTMLFontSelector((HTMLFont) value))
        return htmlFontSelector.ShowDialog((IWin32Window) System.Windows.Forms.Form.ActiveForm) == DialogResult.OK ? (object) htmlFontSelector.SelectedFont : (object) (HTMLFont) value;
    }
  }
}
