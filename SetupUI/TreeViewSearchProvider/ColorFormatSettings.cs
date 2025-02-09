// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.ColorFormatSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Drawing;

#nullable disable
namespace TreeViewSearchProvider
{
  public class ColorFormatSettings
  {
    public Color ForeColor { get; set; } = SystemColors.HighlightText;

    public Color BackColor { get; set; } = SystemColors.Highlight;

    internal static ColorFormatSettings DefaultSettings => new ColorFormatSettings();

    internal static ColorFormatSettings GetNewSetting(Color ForeColor, Color BackColor)
    {
      return new ColorFormatSettings()
      {
        ForeColor = ForeColor,
        BackColor = BackColor
      };
    }
  }
}
