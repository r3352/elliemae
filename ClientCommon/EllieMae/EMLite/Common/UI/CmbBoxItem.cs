// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CmbBoxItem
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.UI;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CmbBoxItem : ObjectWithImage, IComboBoxExItem
  {
    private bool isFormGroup;
    private bool isPublic;
    private string fullPath;
    private string displayStr;
    private int indentWidth;

    public CmbBoxItem(
      string fullPath,
      string displayStr,
      bool isPublic,
      int indentWidth,
      bool isFormGroup)
      : base(displayStr, (Image) null)
    {
      if (isFormGroup)
      {
        if (isPublic)
          this.Image = (Image) Resources.document_group_public;
        else
          this.Image = (Image) Resources.document_group_private;
      }
      else if (isPublic)
        this.Image = (Image) Resources.share_folder_open;
      else
        this.Image = (Image) Resources.folder_open;
      this.isFormGroup = isFormGroup;
      this.isPublic = isPublic;
      this.fullPath = fullPath.Trim();
      this.displayStr = displayStr;
      this.indentWidth = indentWidth;
    }

    public CmbBoxItem(string fullPath, string displayStr, bool isPublic, int indentWidth)
      : this(fullPath, displayStr, isPublic, indentWidth, false)
    {
    }

    public string FullPath => this.fullPath;

    public string TrimedFullPath => this.fullPath.TrimEnd('\\');

    public string DisplayStr => this.displayStr;

    public bool IsPublic => this.isPublic;

    public override string ToString() => this.displayStr;

    public int IndentWidth
    {
      get => this.indentWidth;
      set => this.indentWidth = value;
    }
  }
}
