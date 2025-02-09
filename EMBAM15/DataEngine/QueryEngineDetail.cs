// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.QueryEngineDetail
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class QueryEngineDetail
  {
    private List<Hashtable> details = new List<Hashtable>();

    public QueryEngineDetail(List<Hashtable> details) => this.details = details;

    public string Id => (string) this.GetField("Guid");

    public List<Hashtable> Details => this.details;

    public object GetField(string name)
    {
      return !this.details.Any<Hashtable>() ? (object) null : this.GetField(this.details[0], name, true);
    }

    public object GetField(Hashtable details, string name, bool allowAutoPrefix)
    {
      if (details.ContainsKey((object) name))
        return details[(object) name];
      if (details.ContainsKey((object) name.ToLower()))
        return details[(object) name.ToLower()];
      string key1 = "Loan." + name;
      if (details.ContainsKey((object) key1))
        return details[(object) key1];
      name = name.ToLower();
      foreach (string key2 in (IEnumerable) details.Keys)
      {
        if (key2.ToLower().EndsWith("." + name))
          return details[(object) key2];
      }
      return (object) null;
    }
  }
}
