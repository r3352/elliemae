// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FormatEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class FormatEventArgs : EventArgs
  {
    private string value = "";
    private bool cancel;

    internal FormatEventArgs(string value) => this.value = value;

    public string Value
    {
      get => this.value;
      set => this.value = value;
    }

    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
