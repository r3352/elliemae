// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ListBase
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  [ComVisible(false)]
  public abstract class ListBase : CollectionBase
  {
    private Type listType;

    protected internal ListBase(Type listType) => this.listType = listType;

    protected internal ListBase(Type listType, IList source)
      : this(listType)
    {
      for (int index = 0; index < source.Count; ++index)
        this.List.Add(source[index]);
    }

    public object GetItemAt(int index) => this.List[index];

    public void Sort(IComparer comparer) => this.InnerList.Sort(comparer);

    protected override void OnValidate(object value)
    {
      base.OnValidate(value);
      this.validateType(value);
    }

    protected void validateType(object o)
    {
      if (o != null && !this.listType.IsAssignableFrom(o.GetType()))
        throw new ApplicationException("Invalid object type for list.");
    }
  }
}
