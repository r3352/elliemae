// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CodeWriter
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Text;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class CodeWriter
  {
    private StringBuilder code;
    private int indentLevel;
    private string indentChars = "";
    private int lineCount;
    private bool bol = true;
    private CodeLanguage language;

    public CodeWriter(CodeLanguage language)
    {
      this.language = language;
      this.Clear();
    }

    public void Clear()
    {
      this.code = new StringBuilder();
      this.indentLevel = 0;
      this.indentChars = "";
      this.lineCount = 0;
    }

    public void WriteLine() => this.WriteLine("");

    public void WriteLine(string text)
    {
      this.Write(text);
      this.code.Append(Environment.NewLine);
      this.bol = true;
      ++this.lineCount;
    }

    public void Write(string text)
    {
      string[] strArray = text.Replace(Environment.NewLine, "\n").Replace("\r", "\n").Split('\n');
      for (int index = 0; index < strArray.Length - 1; ++index)
        this.WriteLine(strArray[index]);
      if (strArray.Length == 0 || !(strArray[strArray.Length - 1] != ""))
        return;
      if (this.bol)
        this.code.Append(this.indentChars);
      this.code.Append(strArray[strArray.Length - 1]);
      this.bol = false;
    }

    public void StartBlock(string text)
    {
      if (!this.bol)
        this.WriteLine();
      this.WriteLine(text);
      if (this.language == CodeLanguage.CSharp)
        this.WriteLine("{");
      this.increaseIndent();
    }

    public void EndBlock() => this.EndBlock((string) null);

    public void EndBlock(string text)
    {
      if (!this.bol)
        this.WriteLine();
      this.decreaseIndent();
      if (text != null)
        this.WriteLine(text);
      else if (this.language == CodeLanguage.CSharp)
        this.WriteLine("}");
      this.WriteLine();
    }

    public void ContinueBlock(string text)
    {
      if (!this.bol)
        this.WriteLine();
      this.decreaseIndent();
      this.WriteLine(text);
      this.increaseIndent();
    }

    public void WriteComment(string commentText)
    {
      if (this.language == CodeLanguage.CSharp)
        this.Write("// " + commentText);
      else
        this.Write("'" + commentText);
    }

    public void WriteCommentLine(string commentText)
    {
      this.WriteComment(commentText);
      this.WriteLine();
    }

    public void StartRegion(string regionName) => this.StartRegion(regionName, 0);

    public void StartRegion(string regionName, int charOffset)
    {
      if (!this.bol)
        this.WriteLine();
      this.WriteCommentLine("$Region " + regionName + " (" + (object) (this.lineCount + 1) + ", " + (object) (this.indentLevel + charOffset) + ")");
    }

    public void EndRegion(string regionName)
    {
      if (!this.bol)
        this.WriteLine();
      this.WriteCommentLine("$EndRegion " + regionName);
    }

    public override string ToString() => this.code.ToString();

    private void increaseIndent() => this.indentChars = new string('\t', ++this.indentLevel);

    private void decreaseIndent() => this.indentChars = new string('\t', --this.indentLevel);

    public string ToLiteralString(string text)
    {
      string literalString = "\"" + text.Replace("\"", "\"\"") + "\"";
      if (this.language == CodeLanguage.CSharp)
        literalString = ("@" + literalString).Replace('[', '[').Replace(']', ']');
      else if (this.language == CodeLanguage.VB)
        literalString = literalString.Replace("\r", "\" & vbCRLF & \"").Replace("\n", "").Replace("[", "\" & Chr(91) & \"").Replace("]", "\" & Chr(93) & \"");
      return literalString;
    }
  }
}
