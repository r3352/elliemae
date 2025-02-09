// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.TipParameters
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class TipParameters
  {
    public string TipID = "";
    public string Header = "";
    public string Text = "";

    public TipParameters(string tipId, string header, string text)
    {
      this.TipID = tipId;
      this.Header = header;
      this.Text = text;
    }
  }
}
