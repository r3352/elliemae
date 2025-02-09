// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CommandLine.Variant
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common.CommandLine
{
  public class Variant
  {
    public string Name { get; private set; }

    public string Description { get; private set; }

    public IEnumerable<Parameter> Parameters { get; private set; }

    public Variant(string name, string description, params Parameter[] parameters)
    {
      this.Name = name;
      this.Description = description;
      this.Parameters = (IEnumerable<Parameter>) parameters;
    }

    public string GetHelp(IEnumerable<Parameter> globalParameters)
    {
      IEnumerable<Parameter> parameters = this.Parameters.Union<Parameter>(globalParameters.Where<Parameter>((Func<Parameter, bool>) (gp => !this.Parameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Name == gp.Name)))));
      string str1 = string.Join<Parameter>(" ", parameters) + " - " + this.Description;
      string str2 = string.Join("\n", parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Name != this.Name || p.Type != (Type) null)).Select<Parameter, string>((Func<Parameter, string>) (p => string.Format("    {0,-24} {1}{2}", (object) p.Usage, p.IsRequired ? (object) "(Required) " : (object) "", (object) p.Description))));
      return str1 + (string.IsNullOrEmpty(str2) ? "" : "\n" + str2);
    }
  }
}
