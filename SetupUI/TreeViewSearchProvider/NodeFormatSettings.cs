// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.NodeFormatSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.ComponentModel;

#nullable disable
namespace TreeViewSearchProvider
{
  public class NodeFormatSettings
  {
    private NodeFormatSettings.Highlight _highlight = NodeFormatSettings.Highlight.NextOne;

    public event EventHandler HighlightValueChanged;

    public NodeFormatSettings.Highlight Highlighting
    {
      get => this._highlight;
      set
      {
        this._highlight = value;
        EventHandler highlightValueChanged = this.HighlightValueChanged;
        if (highlightValueChanged == null)
          return;
        highlightValueChanged();
      }
    }

    public ColorFormatSettings Color { get; set; } = new ColorFormatSettings();

    public BlinkSettings Blink { get; set; } = new BlinkSettings();

    public ClearTrailSettings ClearTrail { get; set; } = new ClearTrailSettings();

    internal static NodeFormatSettings DefaultSettings => new NodeFormatSettings();

    public enum Highlight
    {
      [Description("Find Next")] NextOne = 1,
      [Description("Highlight All")] All = 2,
    }
  }
}
