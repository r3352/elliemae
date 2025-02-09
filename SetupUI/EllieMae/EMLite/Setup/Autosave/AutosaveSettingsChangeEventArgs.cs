// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Autosave.AutosaveSettingsChangeEventArgs
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Setup.Autosave
{
  public class AutosaveSettingsChangeEventArgs : EventArgs
  {
    public readonly bool Enabled;
    public readonly int Interval;

    public AutosaveSettingsChangeEventArgs(bool enabled, int interval)
    {
      this.Enabled = enabled;
      this.Interval = interval;
    }
  }
}
