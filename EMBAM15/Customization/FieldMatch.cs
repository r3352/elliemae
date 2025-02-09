// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.FieldMatch
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class FieldMatch
  {
    private Match fieldMatch;

    internal FieldMatch(Match m) => this.fieldMatch = m;

    public string FieldID => this.fieldMatch.Groups["id"].Value;

    public string FieldPrefix => this.fieldMatch.Groups["prefix"].Value;

    public string Value => this.fieldMatch.Groups["field"].Value;

    public int Index => this.fieldMatch.Index;
  }
}
