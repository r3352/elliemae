// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.AssemblyCache
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.CalculationLibrary;
using System;
using System.Collections;
using System.Reflection;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  internal class AssemblyCache
  {
    private Hashtable assemblies = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);

    public AssemblyCache()
    {
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.onAssemblyResolve);
    }

    public object CreateInstance(IElementIdentity elementId, Type interfaceType)
    {
      return this.CreateInstance(elementId.AssemblyName, elementId.ClassName, interfaceType);
    }

    public object CreateInstance(string assemblyName, string className, Type interfaceType)
    {
      object instance = this.getAssembly(assemblyName).CreateInstance(Utility.ValidateCalculationIdentity(className));
      return interfaceType.IsInstanceOfType(instance) ? instance : throw new InvalidCastException(string.Format("The specified interface type {0} is not valid for the object {1}", (object) interfaceType.ToString(), (object) className));
    }

    public Version CompileAssembly(
      string assemblyName,
      string sourceCode,
      CodeLanguage language,
      string[] dependentAssemblies,
      string[] resources,
      string assemblyPath,
      bool saveAssembly = true,
      bool optionStrict = false,
      bool includeDebugInfo = false)
    {
      Assembly assembly = new Compiler(language).Compile(sourceCode, dependentAssemblies, resources, assemblyName, assemblyPath, saveAssembly, optionStrict, includeDebugInfo);
      lock (this.assemblies)
        this.assemblies[(object) assemblyName] = (object) assembly;
      return !(assembly != (Assembly) null) ? (Version) null : assembly.GetName().Version;
    }

    public Version LoadAssembly(string assemblyName, byte[] data)
    {
      Assembly assembly = Assembly.Load(data);
      string key = Utility.ValidateAssemblyName(assemblyName);
      lock (this.assemblies)
        this.assemblies[(object) key] = (object) assembly;
      return !(assembly != (Assembly) null) ? (Version) null : assembly.GetName().Version;
    }

    private Assembly onAssemblyResolve(object sender, ResolveEventArgs args)
    {
      lock (this.assemblies)
      {
        foreach (Assembly assembly in (IEnumerable) this.assemblies.Values)
        {
          if (assembly.FullName == args.Name)
            return assembly;
        }
      }
      return (Assembly) null;
    }

    private Assembly getAssembly(string assemblyName)
    {
      lock (this.assemblies)
      {
        Assembly assembly = (Assembly) this.assemblies[(object) assemblyName];
        return !(assembly == (Assembly) null) ? assembly : throw new ArgumentException("The assembly '" + assemblyName + "' is not loaded in this context");
      }
    }
  }
}
