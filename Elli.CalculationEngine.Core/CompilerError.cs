// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CompilerError
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  [Serializable]
  public class CompilerError
  {
    private string message;
    private int lineIndex;
    private int charIndex;
    private CodeRegion region;

    internal CompilerError(CompileException ex, System.CodeDom.Compiler.CompilerError error)
    {
      this.message = error.ErrorText;
      this.lineIndex = error.Line;
      this.charIndex = error.Column;
      this.region = ex.GetRegionOfLine(this.lineIndex);
    }

    internal CompilerError(CompileException ex, string error)
    {
      this.message = error;
      this.lineIndex = 0;
      this.charIndex = 0;
      this.region = new CodeRegion("Source Code", 0, 0, 0, ex.SourceCode);
    }

    public string Message => this.message;

    public int Line => this.lineIndex;

    public int CharIndex => this.charIndex;

    public CodeRegion Region => this.region;

    public int CharIndexOfRegion
    {
      get => this.region != null ? this.region.ToRegionCharOffset(this.charIndex) : 0;
    }

    public int LineIndexOfRegion
    {
      get => this.region != null ? this.region.ToRegionLineIndex(this.lineIndex) : 0;
    }

    public override string ToString()
    {
      string str = "Error on line " + (object) this.lineIndex + ", char " + (object) this.charIndex + ": " + this.message;
      if (this.region != null)
        str = str + " - Source Code: " + this.region.SourceCode;
      return str;
    }
  }
}
