// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.CompilerError
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Compiler
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
      return "Error on line " + (object) this.lineIndex + ", char " + (object) this.charIndex + ": " + this.message;
    }

    public string ToRegionalString()
    {
      if (this.region == null)
        return this.ToString();
      return "Error on line " + (object) this.LineIndexOfRegion + ", char " + (object) this.CharIndexOfRegion + " of " + this.region.Name + ": " + this.message;
    }
  }
}
