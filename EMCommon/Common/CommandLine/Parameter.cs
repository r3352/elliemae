// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CommandLine.Parameter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.CommandLine
{
  public class Parameter
  {
    public string Name { get; private set; }

    public Type Type { get; private set; }

    public bool IsRequired { get; private set; }

    public string Description { get; private set; }

    public Parameter(string name, Type type, bool isRequired = false, string description = "")
    {
      this.Name = name;
      this.Type = type;
      this.IsRequired = isRequired;
      this.Description = description;
    }

    public override string ToString() => !this.IsRequired ? "[" + this.Usage + "]" : this.Usage;

    public string Usage => "--" + this.Name + (this.Type == (Type) null ? "" : " <value>");
  }
}
