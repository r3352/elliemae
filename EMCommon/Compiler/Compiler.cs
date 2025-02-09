// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.Compiler
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Compiler
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

    public Assembly Compile(string sourceCode, string[] dependentAssemblies)
    {
      string path = Path.Combine(EnConfigurationSettings.GlobalSettings.AppTempDirectory, "Assemblies\\" + Guid.NewGuid().ToString("N") + ".dll");
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      string str;
      if (AssemblyResolver.IsSmartClient)
      {
        AssemblyResolver.GetResourceFileFullPath("EncompassObjects.dll");
        str = Path.GetDirectoryName(AssemblyResolver.GetResourceFileFullPath("EncompassAutomation.dll"));
      }
      else
        str = AppDomain.CurrentDomain.SetupInformation.ApplicationName == null || !AppDomain.CurrentDomain.SetupInformation.ApplicationName.EndsWith("FormBuilder - SmartClient") ? EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory : AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
      if (!str.EndsWith("\\"))
        str += "\\";
      CompilerResults compilerResults = this.compiler.CompileAssemblyFromSource(new CompilerParameters(dependentAssemblies)
      {
        GenerateInMemory = false,
        TreatWarningsAsErrors = false,
        IncludeDebugInformation = false,
        OutputAssembly = path,
        CompilerOptions = this.language != CodeLanguage.CSharp ? "\"/libpath:" + str + "\\\"" : "\"/lib:" + str + "\\\""
      }, sourceCode);
      if (compilerResults.Errors.Count > 0)
        throw new CompileException("The assembly failed to compile", sourceCode, compilerResults.Errors);
      if (compilerResults.NativeCompilerReturnValue != 0)
        throw new CompileException("The assembly failed to compile", sourceCode, compilerResults.Output);
      using (BinaryObject binaryObject = new BinaryObject(path))
      {
        try
        {
          File.Delete(path);
        }
        catch
        {
        }
        return Assembly.Load(binaryObject.GetBytes());
      }
    }
  }
}
