// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.Compiler
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Reflection;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  internal class Compiler
  {
    private CodeLanguage language;
    private CodeDomProvider compiler;

    public Compiler(CodeLanguage language)
    {
      this.language = language;
      if (language == CodeLanguage.CSharp)
      {
        this.compiler = CodeDomProvider.CreateProvider("CSharp");
      }
      else
      {
        if (language != CodeLanguage.VB)
          throw new ArgumentException("Invalid language specified");
        this.compiler = CodeDomProvider.CreateProvider("VisualBasic");
      }
    }

    public Assembly Compile(
      string sourceCode,
      string[] dependentAssemblies,
      string[] resources,
      string assemblyName,
      string assemblyPath,
      bool saveAssembly = false,
      bool optionStrict = false,
      bool includeDebugInfo = false)
    {
      string path1 = Path.Combine(assemblyPath, assemblyName + ".dll");
      Directory.CreateDirectory(Path.GetDirectoryName(path1));
      string fullName = Directory.GetParent(assemblyPath).FullName;
      if (!fullName.EndsWith("\\"))
      {
        string str1 = fullName + "\\";
      }
      string str2 = Path.Combine(assemblyPath, "Encompass.snk");
      CompilerParameters options = new CompilerParameters(dependentAssemblies);
      options.GenerateInMemory = false;
      options.TreatWarningsAsErrors = false;
      options.IncludeDebugInformation = includeDebugInfo;
      options.OutputAssembly = path1;
      string empty = string.Empty;
      if (includeDebugInfo)
      {
        options.TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("TEMP"), true);
        options.TempFiles.KeepFiles = true;
      }
      else
        empty += " /optimize ";
      if (saveAssembly)
        empty += string.Format(" /keyfile:\"{0}\" ", (object) str2);
      options.CompilerOptions = empty;
      string str3;
      if (this.language == CodeLanguage.CSharp)
      {
        str3 = ".cs";
      }
      else
      {
        string str4 = empty + " /optioninfer+ ";
        str3 = ".vb";
      }
      foreach (string resource in resources)
        options.EmbeddedResources.Add(resource);
      CompilerResults compilerResults;
      if (saveAssembly)
      {
        string path2 = Path.Combine(assemblyPath, assemblyName + str3);
        File.WriteAllText(path2, sourceCode);
        compilerResults = this.compiler.CompileAssemblyFromFile(options, path2);
      }
      else
        compilerResults = this.compiler.CompileAssemblyFromSource(options, sourceCode);
      if (compilerResults.Errors.Count > 0)
      {
        string str5 = string.Empty;
        foreach (System.CodeDom.Compiler.CompilerError error in (CollectionBase) compilerResults.Errors)
        {
          if (!error.IsWarning)
            str5 = str5 + "Error: " + error.ErrorText + ", Line: " + (object) error.Line + "\n";
        }
        throw new CompileException("Errors: The assembly failed to compile:" + str5, sourceCode, compilerResults.Errors);
      }
      if (compilerResults.NativeCompilerReturnValue != 0)
        throw new CompileException("NativeCompilerReturnValue: The assembly failed to compile", sourceCode, compilerResults.Output);
      BinaryObject binaryObject = new BinaryObject(Utility.ValidateAssemblyPath(path1));
      if (!saveAssembly)
      {
        try
        {
          File.Delete(path1);
        }
        catch
        {
        }
      }
      return Assembly.Load(binaryObject.Data);
    }
  }
}
