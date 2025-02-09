// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.BlinkSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace TreeViewSearchProvider
{
  public class BlinkSettings
  {
    private bool _canBlink;
    private int _repeat = 2;
    private int _speedInMilliseconds = 100;

    public event EventHandler BlinkSettingsChanged;

    public bool CanBlink
    {
      get => this._canBlink;
      set
      {
        this._canBlink = value;
        EventHandler blinkSettingsChanged = this.BlinkSettingsChanged;
        if (blinkSettingsChanged == null)
          return;
        blinkSettingsChanged();
      }
    }

    public int Repeat
    {
      get => this._repeat;
      set
      {
        if (!Validator.Range(value, 2, 5, "Blink: Repeat").Item1)
          return;
        this._repeat = value;
        EventHandler blinkSettingsChanged = this.BlinkSettingsChanged;
        if (blinkSettingsChanged == null)
          return;
        blinkSettingsChanged();
      }
    }

    public int SpeedInMilliseconds
    {
      get => this._speedInMilliseconds;
      set
      {
        if (!Validator.Range(value, 100, 1000, "Blink: Speed in milliseconds").Item1)
          return;
        this._speedInMilliseconds = value;
        EventHandler blinkSettingsChanged = this.BlinkSettingsChanged;
        if (blinkSettingsChanged == null)
          return;
        blinkSettingsChanged();
      }
    }
  }
}
