// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.CodeRegion
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  [Serializable]
  public class CodeRegion
  {
    private static Regex regionRegex = new Regex("^\\s*(?:'|//)\\$Region ([^\\(]+) \\(([0-9]+),\\s*([0-9]+)\\).*\\n([\\s\\S]*?)^\\s*(?:'|//)\\$EndRegion", RegexOptions.Multiline);
    private string name;
    private int lineOffset = -1;
    private int lineCount = -1;
    private int charOffset = -1;
    private string sourceCode;

    internal CodeRegion(
      string name,
      int lineOffset,
      int lineCount,
      int charOffset,
      string sourceCode)
    {
      this.name = name;
      this.lineOffset = lineOffset;
      this.lineCount = lineCount;
      this.charOffset = charOffset;
      this.sourceCode = sourceCode;
    }

    public string Name => this.name;

    public int LineOffset => this.lineOffset;

    public int LineCount => this.lineCount;

    public int CharOffset => this.charOffset;

    public string SourceCode => this.sourceCode;

    public int ToRegionLineIndex(int lineIndex) => lineIndex - this.lineOffset;

    public int ToRegionCharOffset(int charOffset) => charOffset - this.charOffset;

    public bool IsLineInRegion(int lineIndex)
    {
      return lineIndex >= this.lineOffset && lineIndex <= this.lineOffset + this.lineCount;
    }

    public static CodeRegion[] ParseRegions(string sourceCode)
    {
      Match match = CodeRegion.regionRegex.Match(sourceCode);
      ArrayList arrayList = new ArrayList();
      for (; match.Success; match = match.NextMatch())
      {
        try
        {
          int lineOffset = int.Parse(match.Groups[2].Value);
          int charOffset = int.Parse(match.Groups[3].Value);
          int lineCount = 0;
          string sourceCode1 = CodeRegion.formatRegionCode(match.Groups[4].Value, charOffset, out lineCount);
          arrayList.Add((object) new CodeRegion(match.Groups[1].Value, lineOffset, lineCount, charOffset, sourceCode1));
        }
        catch
        {
        }
      }
      return (CodeRegion[]) arrayList.ToArray(typeof (CodeRegion));
    }

    private static string formatRegionCode(string sourceCode, int charOffset, out int lineCount)
    {
      lineCount = 0;
      StringBuilder stringBuilder = new StringBuilder();
      using (StringReader stringReader = new StringReader(sourceCode))
      {
        string str;
        while ((str = stringReader.ReadLine()) != null)
        {
          if (str.Length > charOffset)
            stringBuilder.Append(str.Substring(charOffset));
          stringBuilder.Append(Environment.NewLine);
          ++lineCount;
        }
      }
      return stringBuilder.ToString();
    }
  }
}
