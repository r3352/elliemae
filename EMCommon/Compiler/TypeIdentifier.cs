// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Compiler.TypeIdentifier
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Compiler
{
  [Serializable]
  public class TypeIdentifier
  {
    public static readonly TypeIdentifier Empty = new TypeIdentifier();
    private string assemblyName;
    private string className;

    public TypeIdentifier(string assemblyName, string className)
    {
      this.assemblyName = assemblyName;
      this.className = className;
    }

    private TypeIdentifier()
    {
      this.assemblyName = (string) null;
      this.className = (string) null;
    }

    public string AssemblyName => this.assemblyName;

    public string ClassName => this.className;

    public override string ToString() => this.assemblyName + "." + this.className;
  }
}
