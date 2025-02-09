// Decompiled with JetBrains decompiler
// Type: Elli.Data.Orm.OrmUtility
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.Data.Orm
{
  public static class OrmUtility
  {
    public static T GetFirstResult<T>(IList results, int index)
    {
      IList result = (IList) results[index];
      T firstResult = default (T);
      if (result.Count > 0)
        firstResult = (T) result[0];
      return firstResult;
    }

    public static IList<T> GetResultList<T>(IList results, int index) => (IList<T>) results[index];
  }
}
