// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.CompilerErrorCollection
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  [Serializable]
  public class CompilerErrorCollection : IEnumerable
  {
    private ArrayList errors = new ArrayList();

    internal CompilerErrorCollection(CompileException ex, System.CodeDom.Compiler.CompilerErrorCollection errors)
    {
      foreach (System.CodeDom.Compiler.CompilerError error in (CollectionBase) errors)
      {
        if (!error.IsWarning)
          this.errors.Add((object) new CompilerError(ex, error));
      }
    }

    internal CompilerErrorCollection(CompileException ex, StringCollection errors)
    {
      foreach (string error in errors)
      {
        if (error.ToLower().IndexOf("error") >= 0)
          this.errors.Add((object) new CompilerError(ex, error));
      }
    }

    public int Count => this.errors.Count;

    public CompilerError this[int index] => (CompilerError) this.errors[index];

    public IEnumerator GetEnumerator() => this.errors.GetEnumerator();
  }
}
