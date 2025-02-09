// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CompilerErrorCollection
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace Elli.CalculationEngine.Core
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

    public IEnumerator GetEnumerator() => this.errors.GetEnumerator();
  }
}
