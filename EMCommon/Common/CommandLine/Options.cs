// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CommandLine.Options
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common.CommandLine
{
  public class Options
  {
    public Dictionary<string, string> _keyValues { get; set; } = new Dictionary<string, string>();

    public Variant Variant { get; set; }

    public IList<string> Keys => (IList<string>) this._keyValues.Keys.ToList<string>();

    public string this[string key]
    {
      get
      {
        string str;
        return !this._keyValues.TryGetValue(key, out str) ? (string) null : str;
      }
      set => this._keyValues[key] = value;
    }

    public bool IsSet(string key) => this[key] != null;

    public int Count => this._keyValues.Count;

    public bool NeedsHelp => this.Count == 0 || this.Variant == null || this.Variant.Name == "help";
  }
}
