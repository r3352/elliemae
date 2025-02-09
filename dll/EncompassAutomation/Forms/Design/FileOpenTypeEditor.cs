// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.FileOpenTypeEditor
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
  public class FileOpenTypeEditor : UITypeEditor
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
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = value as string;
      openFileDialog.Filter = "Supported Image Files|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.TIF;*.TIFF|All Files|*.*";
      return openFileDialog.ShowDialog() == DialogResult.OK ? (object) openFileDialog.FileName : base.EditValue(context, provider, value);
    }
  }
}
