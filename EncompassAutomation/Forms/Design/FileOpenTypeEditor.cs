// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.FileOpenTypeEditor
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
  /// Provides a user interface for allowing users to select image files.
  /// </summary>
  /// <exclude />
  public class FileOpenTypeEditor : UITypeEditor
  {
    /// <summary>
    /// Indicates that the UI provided will be a modal dialog window.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    /// <summary>
    /// Invoked to edit or select an image file from the local disk.
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
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = value as string;
      openFileDialog.Filter = "Supported Image Files|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG;*.TIF;*.TIFF|All Files|*.*";
      return openFileDialog.ShowDialog() == DialogResult.OK ? (object) openFileDialog.FileName : base.EditValue(context, provider, value);
    }
  }
}
