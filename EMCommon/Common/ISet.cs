// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ISet
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public interface ISet : ICollection, IEnumerable, ICloneable
  {
    ISet Union(ISet a);

    ISet Intersect(ISet a);

    ISet Minus(ISet a);

    ISet ExclusiveOr(ISet a);

    bool Contains(object o);

    bool ContainsAll(ICollection c);

    bool IsEmpty { get; }

    bool Add(object o);

    bool AddAll(ICollection c);

    bool Remove(object o);

    bool RemoveAll(ICollection c);

    bool RetainAll(ICollection c);

    void Clear();
  }
}
