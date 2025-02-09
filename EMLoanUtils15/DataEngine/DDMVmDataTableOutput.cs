// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMVmDataTableOutput
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMVmDataTableOutput
  {
    public int Criteria { get; set; }

    public string Value { get; set; }

    public string FormattedLine { get; set; }

    public void Process()
    {
      switch (this.Criteria)
      {
        case 16:
          this.FormattedLine = string.Format("\"{0}\"", (object) Utils.EscapeDoubleQuotesForVB(this.Value));
          break;
        case 17:
          this.FormattedLine = string.Format("Convert.ToString({0})", (object) DDMViewModelConverter.RemoveNewlineCharacters(this.Value));
          break;
        case 27:
          this.FormattedLine = "\" \"";
          break;
        default:
          this.FormattedLine = "Nothing";
          break;
      }
    }
  }
}
