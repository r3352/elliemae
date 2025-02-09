// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.ListBase
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Collections
{
  [Serializable]
  public abstract class ListBase : CollectionBase
  {
    private Type listType;
    private bool requireUnique;

    protected internal ListBase(Type listType, bool requireUnique)
    {
      this.listType = listType;
      this.requireUnique = requireUnique;
    }

    protected internal ListBase(Type listType, bool requireUnique, IList source)
      : this(listType, requireUnique)
    {
      this.AddRange((IEnumerable) source);
    }

    public object GetItemAt(int index) => this.List[index];

    public void Sort() => this.InnerList.Sort();

    public void Sort(IComparer comparer) => this.InnerList.Sort(comparer);

    protected void Add(object value)
    {
      if (this.requireUnique && this.List.Contains(value))
        return;
      this.List.Add(value);
    }

    protected void AddRange(IEnumerable items)
    {
      foreach (object obj in items)
        this.Add(obj);
    }

    protected override void OnValidate(object value)
    {
      base.OnValidate(value);
      this.validateType(value);
    }

    protected void validateType(object o)
    {
      if (!this.listType.IsAssignableFrom(o.GetType()))
        throw new ApplicationException("Invalid object type for list.");
    }
  }
}
