// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CommandLine.Parser
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common.CommandLine
{
  public class Parser
  {
    public Config Config { get; private set; }

    public Parser(Config config) => this.Config = config;

    public T Parse<T>(string[] args) where T : Options, new()
    {
      T options = new T();
      string key = (string) null;
      foreach (string str in args)
      {
        if (str.StartsWith("--"))
        {
          key = str.Substring(2);
          if (!string.IsNullOrEmpty(key))
            ((T) options)[key] = (string) null;
        }
        else
        {
          if (!string.IsNullOrEmpty(key))
            ((T) options)[key] = str;
          key = (string) null;
        }
      }
      ((T) options).Variant = this.Config.Variants.FirstOrDefault<Variant>((Func<Variant, bool>) (v => options.Keys.All<string>((Func<string, bool>) (k => this.Config.GlobalParameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Name == k)) || v.Parameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Name == k)))) && v.Parameters.All<Parameter>((Func<Parameter, bool>) (p => !p.IsRequired || hasProperValue(p)))));
      return options;

      bool hasProperValue(Parameter p) => p.Type == (Type) null == (options[p.Name] == null);
    }
  }
}
