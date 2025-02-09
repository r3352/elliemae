// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.CodeBase
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public class CodeBase
  {
    public static readonly CodeBase Empty = new CodeBase();
    private string assemblyPath = "";
    private string assemblyName = "";
    private string assemblyVersion = "";
    private string className = "";

    public CodeBase(
      string assemblyPath,
      string assemblyName,
      string assemblyVersion,
      string className)
    {
      if ((assemblyName ?? "") == "")
        throw new ArgumentException("Assembly name cannot be blank or null");
      if ((className ?? "") == "")
        throw new ArgumentException("Class name cannot be blank or null");
      this.assemblyPath = assemblyPath ?? "";
      this.assemblyName = assemblyName ?? "";
      this.assemblyVersion = assemblyVersion ?? "";
      this.className = className ?? "";
    }

    private CodeBase()
    {
    }

    public string AssemblyName => this.assemblyName;

    public string AssemblyVersion => this.assemblyVersion;

    public string ClassName => this.className;

    public string AssemblyPath => this.assemblyPath;

    public override string ToString()
    {
      return this.ClassName == "" ? "" : this.AssemblyName + ", " + this.ClassName;
    }
  }
}
