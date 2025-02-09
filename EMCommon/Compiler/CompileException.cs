// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.CompileException
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  [Serializable]
  public class CompileException : Exception
  {
    private string sourceCode;
    private CompilerErrorCollection errors;
    private CodeRegion[] regions;

    public CompileException(string message, string sourceCode, System.CodeDom.Compiler.CompilerErrorCollection errors)
      : base(message)
    {
      this.sourceCode = sourceCode;
      this.regions = CodeRegion.ParseRegions(sourceCode);
      this.errors = new CompilerErrorCollection(this, errors);
    }

    public CompileException(string message, string sourceCode, StringCollection errors)
      : base(message)
    {
      this.sourceCode = sourceCode;
      this.regions = CodeRegion.ParseRegions(sourceCode);
      this.errors = new CompilerErrorCollection(this, errors);
    }

    private CompileException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.sourceCode = info.GetString(nameof (sourceCode));
      this.errors = (CompilerErrorCollection) info.GetValue(nameof (errors), typeof (CompilerErrorCollection));
    }

    public string SourceCode => this.sourceCode;

    public CompilerErrorCollection Errors => this.errors;

    public CodeRegion GetRegionOfLine(int lineIndex)
    {
      for (int index = 0; index < this.regions.Length; ++index)
      {
        if (this.regions[index].IsLineInRegion(lineIndex))
          return this.regions[index];
      }
      return (CodeRegion) null;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("sourceCode", (object) this.sourceCode);
      info.AddValue("errors", (object) this.errors);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Exception Information:" + Environment.NewLine + base.ToString() + Environment.NewLine);
      stringBuilder.Append("Source Code:" + Environment.NewLine + this.sourceCode + Environment.NewLine);
      stringBuilder.Append("Errors:" + Environment.NewLine);
      foreach (CompilerError error in this.errors)
        stringBuilder.Append("ERROR: " + error.ToString() + Environment.NewLine);
      return stringBuilder.ToString();
    }
  }
}
