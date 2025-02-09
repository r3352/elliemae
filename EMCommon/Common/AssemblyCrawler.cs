// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.AssemblyCrawler
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class AssemblyCrawler
  {
    private SortedDictionary<string, Assembly> _dependentAssemblyDict;
    private List<AssemblyCrawler.MissingAssembly> _missingAssemblyList;

    public void ExamineAssemblies()
    {
      this._dependentAssemblyDict = new SortedDictionary<string, Assembly>();
      this._missingAssemblyList = new List<AssemblyCrawler.MissingAssembly>();
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        this.internalGetDependentAssembliesRecursive(assembly, true);
        this._missingAssemblyList.Sort();
      }
    }

    public List<Assembly> DependentAssemblies
    {
      get
      {
        return this._dependentAssemblyDict != null ? this._dependentAssemblyDict.Values.ToList<Assembly>() : (List<Assembly>) null;
      }
    }

    public List<AssemblyCrawler.MissingAssembly> MissingAssemblies => this._missingAssemblyList;

    private void internalGetDependentAssembliesRecursive(Assembly assembly, bool includeMe = false)
    {
      if (includeMe)
        this._dependentAssemblyDict[assembly.FullName.Split(',')[0]] = assembly;
      foreach (AssemblyName assemblyName in (IEnumerable<AssemblyName>) ((IEnumerable<AssemblyName>) assembly.GetReferencedAssemblies()).OrderByDescending<AssemblyName, Version>((Func<AssemblyName, Version>) (o => o.Version)))
      {
        if (!string.IsNullOrEmpty(assembly.FullName))
        {
          if (!this._dependentAssemblyDict.ContainsKey(assemblyName.FullName.Split(',')[0]))
          {
            try
            {
              Assembly assembly1 = Assembly.ReflectionOnlyLoad(assemblyName.FullName);
              this._dependentAssemblyDict[assembly1.FullName.Split(',')[0]] = assembly1;
              this.internalGetDependentAssembliesRecursive(assembly1);
            }
            catch (Exception ex)
            {
              AssemblyCrawler.MissingAssembly missingAssembly = new AssemblyCrawler.MissingAssembly(assemblyName.FullName.Split(',')[0], assembly);
              if (!this._missingAssemblyList.Contains(missingAssembly))
                this._missingAssemblyList.Add(missingAssembly);
            }
          }
        }
      }
    }

    public override string ToString()
    {
      string str1 = "";
      if (this._dependentAssemblyDict != null && this._dependentAssemblyDict.Count > 0)
      {
        str1 = str1 + "#Found Assemblies (" + (object) this._dependentAssemblyDict.Count + ") - " + Environment.NewLine;
        foreach (Assembly assembly in this._dependentAssemblyDict.Values)
        {
          string str2 = "Dynamic";
          try
          {
            str2 = assembly.Location;
          }
          catch
          {
          }
          str1 = str1 + assembly.FullName.Split(',')[0] + "," + assembly.FullName.Split(',')[1].Split('=')[1] + "," + (assembly.GlobalAssemblyCache ? "GAC" : "App") + "," + str2 + Environment.NewLine;
        }
      }
      string str3 = str1 + Environment.NewLine;
      if (this._missingAssemblyList != null && this._missingAssemblyList.Count > 0)
      {
        str3 = str3 + "#Missing Assemblies (" + (object) this._missingAssemblyList.Count + ") - " + Environment.NewLine;
        foreach (AssemblyCrawler.MissingAssembly missingAssembly in this._missingAssemblyList)
          str3 = str3 + missingAssembly.Name + "," + missingAssembly.Owner.FullName.Split(',')[0] + "," + (missingAssembly.Owner.GlobalAssemblyCache ? "GAC" : "App") + Environment.NewLine;
      }
      return str3;
    }

    public class MissingAssembly : 
      IComparable<AssemblyCrawler.MissingAssembly>,
      IEquatable<AssemblyCrawler.MissingAssembly>
    {
      private string assemblyName_;
      private Assembly ownerAssembly_;

      public MissingAssembly(string assemblyName, Assembly ownerAssembly)
      {
        this.assemblyName_ = assemblyName;
        this.ownerAssembly_ = ownerAssembly;
      }

      public string Name => this.assemblyName_;

      public Assembly Owner => this.ownerAssembly_;

      public bool Equals(AssemblyCrawler.MissingAssembly other)
      {
        return this.CompareTo(other) == 0 && string.Compare(this.Owner.ToString(), other.Owner.ToString(), StringComparison.InvariantCultureIgnoreCase) == 0;
      }

      public int CompareTo(AssemblyCrawler.MissingAssembly other)
      {
        return this.assemblyName_.CompareTo(other.assemblyName_);
      }
    }
  }
}
