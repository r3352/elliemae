// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.AssemblyCache
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  internal class AssemblyCache : MarshalByRefObject
  {
    private Hashtable assemblies = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);

    public AssemblyCache()
    {
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.onAssemblyResolve);
    }

    public ObjRef CreateInstance(TypeIdentifier typeId, Type interfaceType)
    {
      MarshalByRefObject instance = (MarshalByRefObject) this.getAssembly(typeId.AssemblyName).CreateInstance(typeId.ClassName);
      if (instance == null)
        throw new ArgumentException("The specified class is not a valid memeber of this assembly");
      return interfaceType.IsAssignableFrom(instance.GetType()) ? RemotingServices.Marshal(instance, (string) null, interfaceType) : throw new InvalidCastException("The specified interface type is not valid for the object");
    }

    public object CreateLocalInstance(TypeIdentifier typeId, Type interfaceType)
    {
      object instance = this.getAssembly(typeId.AssemblyName).CreateInstance(typeId.ClassName);
      if (instance == null)
        throw new ArgumentException("The specified class is not a valid memeber of this assembly");
      return interfaceType.IsAssignableFrom(instance.GetType()) ? instance : throw new InvalidCastException("The specified interface type is not valid for the object");
    }

    public TypeIdentifier[] GetTypes(string assemblyName, Type baseType)
    {
      Assembly assembly = this.getAssembly(assemblyName);
      ArrayList arrayList = new ArrayList();
      foreach (Type type in assembly.GetTypes())
      {
        if (baseType == (Type) null || baseType.IsAssignableFrom(type))
          arrayList.Add((object) new TypeIdentifier(assemblyName, type.FullName));
      }
      return (TypeIdentifier[]) arrayList.ToArray(typeof (TypeIdentifier));
    }

    public TypeDescriptor GetTypeInformation(TypeIdentifier typeId)
    {
      Type type = this.getType(typeId);
      return type == (Type) null ? (TypeDescriptor) null : new TypeDescriptor(typeId, type);
    }

    public bool IsAssemblyLoaded(string assemblyName)
    {
      lock (this.assemblies)
        return this.assemblies.Contains((object) assemblyName);
    }

    public Assembly GetAssembly(string assemblyName)
    {
      return !this.IsAssemblyLoaded(assemblyName) ? (Assembly) null : this.getAssembly(assemblyName);
    }

    public AssemblyName GetAssemblyName(string assemblyName)
    {
      return this.getAssembly(assemblyName).GetName();
    }

    public void CompileAssembly(
      string assemblyName,
      string sourceCode,
      CodeLanguage language,
      string[] dependentAssemblies)
    {
      Assembly assembly = new EllieMae.EMLite.Compiler.Compiler(language).Compile(sourceCode, dependentAssemblies);
      lock (this.assemblies)
        this.assemblies[(object) assemblyName] = (object) assembly;
    }

    public void LoadAssembly(string assemblyName, byte[] data)
    {
      Assembly assembly = Assembly.Load(data);
      lock (this.assemblies)
        this.assemblies[(object) assemblyName] = (object) assembly;
    }

    public Assembly LoadAssembly(string assemblyPath, bool revertPluginChanges)
    {
      Assembly assembly = !revertPluginChanges ? Assembly.LoadFrom(assemblyPath) : Assembly.Load(File.ReadAllBytes(assemblyPath));
      lock (this.assemblies)
        this.assemblies[(object) Guid.NewGuid().ToString()] = (object) assembly;
      return assembly;
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

    private Type getType(TypeIdentifier typeId)
    {
      Type type = this.getAssembly(typeId.AssemblyName).GetType(typeId.ClassName);
      return type == (Type) null ? (Type) null : type;
    }

    public override object InitializeLifetimeService() => (object) null;
  }
}
