// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.Alert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class Alert
  {
    private ECloseLog _parent;

    public Alert()
    {
    }

    public string Source { get; set; }

    public string Type { get; set; }

    public string Text { get; set; }

    public string Fields { get; set; }

    public Alert(ECloseLog parent) => this._parent = parent;
  }
}
