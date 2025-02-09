// Decompiled with JetBrains decompiler
// Type: CICDAssemblyAttributes.CICDAssemblyTypeAttribute
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;

#nullable disable
namespace CICDAssemblyAttributes
{
  [AttributeUsage(AttributeTargets.Assembly)]
  public class CICDAssemblyTypeAttribute : Attribute
  {
    private bool _requiresServerDeployment;
    private AssemblyType _assemblyType;

    public CICDAssemblyTypeAttribute(AssemblyType assemblyType)
    {
      this._assemblyType = assemblyType;
      this._requiresServerDeployment = assemblyType != 0;
    }

    public bool RequireServerDeployment => this._requiresServerDeployment;

    public AssemblyType AssemblyType => this._assemblyType;
  }
}
