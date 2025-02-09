// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.StringSearchAlgorithm
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

#nullable disable
namespace EllieMae.EMLite.Collections
{
  public class StringSearchAlgorithm
  {
    private string matchText;
    private bool ignoreCase;

    public StringSearchAlgorithm(string matchText)
      : this(matchText, true)
    {
    }

    public StringSearchAlgorithm(string matchText, bool ignoreCase)
    {
      this.matchText = matchText;
      this.ignoreCase = ignoreCase;
    }

    public string MatchText => this.matchText;

    public bool IgnoreCase => this.ignoreCase;

    public bool Match(string text)
    {
      return text != null && string.Compare(this.matchText, text, this.ignoreCase) == 0;
    }
  }
}
