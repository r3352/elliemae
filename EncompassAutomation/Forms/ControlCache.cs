// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ControlCache
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Summary description for ControlCache.</summary>
  internal class ControlCache : IEnumerable
  {
    private Hashtable cache = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal ControlCache()
    {
    }

    public void Add(Control control) => this.cache[(object) control.ControlID] = (object) control;

    public Control this[string controlId] => (Control) this.cache[(object) controlId];

    public bool Contains(string controlId) => this.cache.ContainsKey((object) controlId);

    public void Remove(string controlId) => this.cache.Remove((object) controlId);

    public void Clear() => this.cache = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public IEnumerator GetEnumerator() => this.cache.Values.GetEnumerator();
  }
}
