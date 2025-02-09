// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.ClearTrailSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace TreeViewSearchProvider
{
  public class ClearTrailSettings
  {
    private bool _canClearTrail;
    private int _delayInMilliseconds;

    public event EventHandler ClearTrailSettingsChanged;

    public bool CanClear
    {
      get => this._canClearTrail;
      set
      {
        this._canClearTrail = value;
        EventHandler trailSettingsChanged = this.ClearTrailSettingsChanged;
        if (trailSettingsChanged == null)
          return;
        trailSettingsChanged();
      }
    }

    public int DelayInMilliseconds
    {
      get => this._delayInMilliseconds;
      set
      {
        if (!Validator.Range(value, 0, 1000, "Clear Trail: Delay in milliseconds").Item1)
          return;
        this._delayInMilliseconds = value;
        EventHandler trailSettingsChanged = this.ClearTrailSettingsChanged;
        if (trailSettingsChanged == null)
          return;
        trailSettingsChanged();
      }
    }
  }
}
